using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Logging;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Repository
{
    public class Repository<T> : IRepository<T>
    {
        private readonly ILogger<Repository<T>> _logger;
        private readonly string _connectionString;
        private readonly string _tableName;

        public Repository(ILogger<Repository<T>> logger, GeneralConfig config)
        {
            _logger = logger;
            _connectionString = $"Data Source={Path.Combine(config.UserConfig.UserDataPath, config.DbFilename)};";
            _tableName = typeof(T).Name;
        }

        public Task CreateTable()
        {
            return ExecuteAsync(BuildCreateSql());
        }

        public async Task<int> CreateAsync(T entity)
        {
            var insertSql = BuildInsertSql();
            var param = CreateParams(entity);

            return await ExecuteAsync(insertSql, param);
        }

        public async Task<IEnumerable<int>> CreateAsync(IEnumerable<T> entities)
        {
            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    using (var transaction = con.BeginTransaction())
                    {
                        var results = new List<int>();

                        foreach (var entity in entities)
                        {
                            var insertSql = BuildInsertSql();
                            var param = CreateParams(entity);

                            var result = await con.ExecuteAsync(insertSql, param, transaction);

                            results.Add(result);
                        }

                        transaction.Commit();

                        return results;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }
            
            return new int[0];
        }

        public async Task<IEnumerable<T>> GetWhereAsync(List<WhereClause> whereClauses)
        {
            var whereSql = new StringBuilder("WHERE ");

            foreach (var w in whereClauses)
            {
                whereSql.Append($"{w.PropertyName} {w.Op} @Val");
                whereSql.Append($" {w.AndOr} ");
            }
            
            var sql = $"SELECT * FROM {_tableName} {whereSql}";

            var dp = new DynamicParameters();

            dp.Add("Val", whereClauses.Select(x => x.Val));

            return await QueryAsync(sql, dp);
        }

        public async Task<IEnumerable<T>> GetLastEntriesAsync(int limit = 0)
        {
            var sql = $"SELECT * FROM {_tableName} t {(limit > 0 ? $"LIMIT {limit}" : "" )};";

            return await QueryAsync(sql);
        }

        public async Task<T> GetLastEntryAsync()
        {
            var sql = $"SELECT * FROM {_tableName} t LIMIT 1";

            return (await QueryAsync(sql)).FirstOrDefault();
        }

        private async Task<int> ExecuteAsync(string sql, DynamicParameters parameters = default)
        {
            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    return await con.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return 0;
            }
        }

        private async Task<IEnumerable<T>> QueryAsync(string sql, DynamicParameters parameters = default)
        {
            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    return await con.QueryAsync<T>(sql, parameters);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return default;
            }
        }

        private static DynamicParameters CreateParams(T entity)
        {
            var props = GetProps(typeof(T));
            var dp = new DynamicParameters();

            foreach (var prop in props)
            {
                var val = prop.GetValue(entity);

                if (prop == typeof(DateTime))
                    val = ((DateTime) val).Ticks;

                dp.Add(prop.Name, val);
            }

            return dp;
        }

        private string BuildInsertSql()
        {
            var props = GetProps(typeof(T));
            var sb = new StringBuilder();
            
            sb.Append($"INSERT INTO {_tableName} (");
            sb.Append(string.Join(',', props.Select(x => x.Name)));
            sb.Append(") values(");
            sb.Append(string.Join(',', props.Select(x => $"@{x.Name}")));
            sb.Append(")");

            return sb.ToString();
        }

        private string BuildCreateSql()
        {
            var props = GetProps(typeof(T));
            var sb = new StringBuilder();

            sb.Append($"CREATE TABLE IF NOT EXISTS {_tableName}(");
            sb.Append($"{string.Join(',', props.Select(x => $"{x.Name} {TypeToSql(x.PropertyType)} NOT NULL"))}");
            sb.Append(")");

            return sb.ToString();
        }

        private static string TypeToSql(MemberInfo type)
        {
            switch (type.Name)
            {
                case "Int32":
                case "Int64":
                case "Boolean":
                    return "int";
                case "Double":
                    return "float";
                case "Decimal":
                    return "numeric";
                case "String":
                    return "varchar(255)";
                case "DateTime":
                    return "datetime";
                default:
                    throw new NotSupportedException();
            }
        }

        private static List<PropertyInfo> GetProps(Type type)
        {
            return type.GetProperties().Where(x => !IgnoreProperty(x)).ToList();
        }

        private static bool IgnoreProperty(ICustomAttributeProvider target)
        {
            var attribs = target.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), false);

            return attribs.Length > 0;
        }
    }
}