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
        private readonly HttpClient _client;
        private readonly ILogger _log;

        public NexusClient(HttpClient client, ILogger log = null)
        {
            _client = client;
            _log = log;
        }

        public void ConfigureHttpClient(NexusNodeParameters parameters)
        {
            if (_client == null)
                throw new NullReferenceException("Http client is null");

            if (parameters == null)
                throw new ArgumentException("Parameters are missing");

            if (string.IsNullOrWhiteSpace(parameters.Url))
                throw new ArgumentException("URL is missing");

            if (string.IsNullOrWhiteSpace(parameters.Username))
                throw new ArgumentException("Username is missing");

            if (string.IsNullOrWhiteSpace(parameters.Password))
                throw new ArgumentException("Password is missing");

            var uriResult = Uri.TryCreate(parameters.Url, UriKind.Absolute, out var baseUri)
                            && (baseUri.Scheme == Uri.UriSchemeHttp || baseUri.Scheme == Uri.UriSchemeHttps);

            if (!uriResult)
                throw new Exception("Url is not valid");

            var auth64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{parameters.Username}:{parameters.Password}"));

            _client.BaseAddress = baseUri;
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Basic {auth64}");
            _client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
        }

        public async Task<HttpResponseMessage> GetAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput)
        {
            var getRequest = request != null
                ? $"{path}?{request.GetParamString()}"
                : path;

            if (_log != null && logOutput)
                _log.LogInformation($"{logHeader} {getRequest}");

            return await _client.GetAsync(getRequest, token);
        }

        public async Task<HttpResponseMessage> PostAsync(string path, string logHeader, NexusRequest request, CancellationToken token, bool logOutput)
        {
            var form = new FormUrlEncodedContent(request?.Param ?? null);

            if (_log != null && logOutput)
                _log.LogInformation($"{logHeader} {await form.ReadAsStringAsync()}");

            return await _client.PostAsync(path, form, token).ConfigureAwait(false);
        }
    }
}