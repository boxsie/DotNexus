﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Boxsie.Wrapplication.Config;
using Boxsie.Wrapplication.Data;
using Boxsie.Wrapplication.Logging;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Repository
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private readonly IBxLogger _logger;
        private readonly string _connectionString;
        private readonly Stopwatch _stopwatch;

        private string _tableName;

        public Repository(IBxLogger logger, GeneralConfig config)
        {
            _logger = logger;
            _connectionString = $"Data Source={Path.Combine(config.UserConfig.UserDataPath, config.DbFilename)};";
            _stopwatch = new Stopwatch();
        }

        public async Task CreateTable(string tableName)
        {
            _tableName = tableName;

            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    await con.ExecuteAsync(BuildCreateSql());
                }
            }
            catch (Exception e)
            {
                _logger.WriteLine(e.Message, LogLevel.Warning);
                throw;
            }
        }

        public async Task<int> CreateAsync(T entity)
        {
            try
            {
                var insertSql = BuildInsertSql();
                var param = CreateParams(entity);

                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    return await con.ExecuteAsync(insertSql, param);
                }
            }
            catch (Exception e)
            {
                _logger.WriteLine(e.Message, LogLevel.Warning);
            }

            return 0;
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
                _logger.WriteLine(e.Message, LogLevel.Warning);
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

            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    var result = (await con.QueryAsync(sql, new { Val = whereClauses.Select(x => x.Val)})).Select(Mapper.Map<T>);

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.WriteLine(e.Message, LogLevel.Warning);
            }

            return new List<T>();
        }

        public async Task<IEnumerable<T>> GetLastEntriesAsync(int limit = 0)
        {
            var sql = $"SELECT * FROM {_tableName} t ORDER BY t.Timestamp DESC {(limit > 0 ? $"LIMIT {limit}" : "" )};";

            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    var result = (await con.QueryAsync(sql)).Select(Mapper.Map<T>);

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.WriteLine(e.Message, LogLevel.Warning);
            }

            return default(IEnumerable<T>);
        }

        public async Task<T> GetLastEntryAsync()
        {
            var sql = $"SELECT * FROM {_tableName} t ORDER BY t.Timestamp DESC LIMIT 1";

            try
            {
                using (var con = new SQLiteConnection(_connectionString))
                {
                    await con.OpenAsync();

                    var result = (await con.QueryAsync(sql)).Select(Mapper.Map<T>);

                    return result.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _logger.WriteLine(e.Message, LogLevel.Warning);
            }

            return default(T);
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