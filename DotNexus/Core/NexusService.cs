using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace DotNexus.Core
{
    public abstract class NexusService
    {
        private readonly ILogger _log;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settings;

        protected NexusService(ILogger log, HttpClient client, string connectionString)
        {
            _log = log;
            _client = ConfigureHttpClient(client, connectionString);

            _settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new LowercaseContractResolver()
            };
        }

        protected async Task<T> GetAsync<T>(string path, NexusRequest request, CancellationToken token)
        {
            var requestName = typeof(T).Name;
            var logHeader = $"API GET {path}:";

            try
            {
                if (path == null)
                {
                    _log.Error($"Path is missing for '{requestName}' get request");
                    return default(T);
                }

                if (path[0] == '/')
                    path = path.Remove(0, 1);
                
                var getRequest = $"{path}{(request != null ? $"?{request.GetParamString()}" : "")}";

                _log.Info($"{logHeader} {getRequest}");

                var httpResponseMessage = await _client.GetAsync(getRequest, token);
                var responseJson = await httpResponseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<NexusResponse<T>>(responseJson, _settings);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    _log.Info($"{logHeader} SUCCESS");
                    _log.Info($"{logHeader} {JsonConvert.SerializeObject(result.Result)}");

                    return result.Result;
                }

                _log.Error($"{logHeader} FAILED");
                _log.Error($"{logHeader} {httpResponseMessage.StatusCode} {(result.Error != null ? $"From Nexus->'{result.Error.Code} - {result.Error.Message}'" : responseJson)}");

                return default(T);
            }
            catch (Exception e)
            {
                _log.Error($"{logHeader} FAILED");
                _log.Error(e.Message);
                _log.Error(e.StackTrace);

                return default(T);
            }
        }

        private static HttpClient ConfigureHttpClient(HttpClient client, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string is missing");

            var connSplit = connectionString.Split(';');

            if (connSplit.Length < 3)
                throw new Exception("Connection string data is missing");

            var baseAddress = connSplit[0];

            if (string.IsNullOrWhiteSpace(baseAddress))
                throw new Exception("Base URL is missing");

            if (baseAddress.Last() != '/')
                baseAddress = $"{baseAddress}/";

            var uriResult = Uri.TryCreate(baseAddress, UriKind.Absolute, out var baseUri)
                            && (baseUri.Scheme == Uri.UriSchemeHttp || baseUri.Scheme == Uri.UriSchemeHttps);

            if (!uriResult)
                throw new Exception("Url is not valid");

            var username = connSplit[1];
            var password = connSplit[2];

            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Username field is missing");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password field is missing");

            var auth64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Basic {auth64}");
            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");

            return client;
        }
    }
}