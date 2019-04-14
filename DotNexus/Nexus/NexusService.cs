using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HttpMethod = DotNexus.Core.Enums.HttpMethod;

namespace DotNexus.Nexus
{
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

        protected Task<T> GetAsync<T>(string path, NexusRequest request, CancellationToken token = default, bool logOutput = true)
        {
            return RequestAsync<T>(HttpMethod.Get, path, request, token, logOutput);
        }

        protected Task<T> PostAsync<T>(string path, NexusRequest request, CancellationToken token = default, bool logOutput = true)
        {
            return RequestAsync<T>(HttpMethod.Post, path, request, token, logOutput);
        }

        private async Task<T> RequestAsync<T>(HttpMethod httpMethod, string path, NexusRequest request, CancellationToken token, bool logOutput)
        {
            token.ThrowIfCancellationRequested();

            var requestName = typeof(T).Name;
            var logHeader = $"API {(httpMethod == HttpMethod.Get ? "GET" : "POST")} {path}:";

            try
            {
                if (path == null)
                {
                    Log.LogError($"Path is missing for '{requestName}' get request");
                    return default;
                }

                if (path[0] == '/')
                    path = path.Remove(0, 1);

                var response = httpMethod == HttpMethod.Get
                    ? await GetAsync(path, logHeader, request, token, logOutput) 
                    : await PostAsync(path, logHeader, request, token, logOutput);

                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<NexusResponse<T>>(responseJson, _settings);

                if (response.IsSuccessStatusCode)
                {
                    Log.LogInformation($"{logHeader} SUCCESS");

                    if (logOutput)
                        Log.LogInformation($"{logHeader} {JsonConvert.SerializeObject(result.Result)}");

                    return result.Result;
                }

                Log.LogError($"{logHeader} FAILED");
                Log.LogError($"{logHeader} {response.StatusCode} {(result.Error != null ? $"From Nexus->'{result.Error.Code} - {result.Error.Message}'" : responseJson)}");

                return default;
            }
            catch (Exception e)
            {
                Log.LogError($"{logHeader} FAILED");
                Log.LogError(e.Message);
                Log.LogError(e.StackTrace);

                return default;
            }
        }

        private async Task<HttpResponseMessage> GetAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput)
        {
            var getRequest = request != null 
                ? $"{path}{request.GetParamString()}" 
                : path;

            if (logOutput)
                Log.LogInformation($"{logHeader} {getRequest}");

            return await _client.GetAsync(getRequest, token);
        }

        private async Task<HttpResponseMessage> PostAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput)
        {
            var form = new FormUrlEncodedContent(request?.Param ?? null);

            if (logOutput)
                Log.LogInformation($"{logHeader} {await form.ReadAsStringAsync()}");

            return await _client.PostAsync(path, form, token).ConfigureAwait(false);
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