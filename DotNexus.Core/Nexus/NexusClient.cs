using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNexus.Core.Nexus
{
    public class NexusClient : INexusClient
    {
        private readonly ILogger _log;
        private readonly HttpClient _client;

        public NexusClient(ILogger log, HttpClient client)
        {
            _log = log;
            _client = client;
        }

        public async Task<HttpResponseMessage> GetAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput)
        {
            var getRequest = request != null
                ? $"{path}?{request.GetParamString()}"
                : path;

            if (logOutput)
                _log.LogInformation($"{logHeader} {getRequest}");

            return await _client.GetAsync(getRequest, token);
        }

        public async Task<HttpResponseMessage> PostAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput)
        {
            var form = new FormUrlEncodedContent(request?.Param ?? null);

            if (logOutput)
                _log.LogInformation($"{logHeader} {await form.ReadAsStringAsync()}");

            return await _client.PostAsync(path, form, token).ConfigureAwait(false);
        }

        public static HttpClient ConfigureHttpClient(HttpClient client, string connectionString)
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