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
    public class NexusServiceSettings
    {
        public bool ApiSessions { get; set; }
        public bool IndexHeight { get; set; }
    }

    public abstract class NexusService
    {
        protected readonly ILogger Log;
        protected readonly NexusServiceSettings ServiceSettings;

        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settings;

        protected NexusService(ILogger log, HttpClient client, string connectionString, NexusServiceSettings serviceSettings)
        {
            Log = log;
            ServiceSettings = serviceSettings;

            _client = ConfigureHttpClient(client, connectionString);

            _settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new LowercaseContractResolver()
            };
        }
        
        protected async Task<T> PostAsync<T>(string path, NexusRequest request, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var requestName = typeof(T).Name;
            var logHeader = $"API POST {path}:";
            
            try
            {
                if (path == null)
                {
                    Log.Error($"Path is missing for '{requestName}' get request");
                    return default;
                }

                if (path[0] == '/')
                    path = path.Remove(0, 1);

                var form = new FormUrlEncodedContent(request.Param);
                Log.Info($"{logHeader} {await form.ReadAsStringAsync()}");

                var response = await _client.PostAsync(path, form, token).ConfigureAwait(false);
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<NexusResponse<T>>(responseJson, _settings);

                if (response.IsSuccessStatusCode)
                {
                    Log.Info($"{logHeader} SUCCESS");
                    Log.Info($"{logHeader} {JsonConvert.SerializeObject(result.Result)}");

                    return result.Result;
                }

                Log.Error($"{logHeader} FAILED");
                Log.Error($"{logHeader} {response.StatusCode} {(result.Error != null ? $"From Nexus->'{result.Error.Code} - {result.Error.Message}'" : responseJson)}");

                return default;
            }
            catch (Exception e)
            {
                Log.Error($"{logHeader} FAILED");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);

                return default;
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