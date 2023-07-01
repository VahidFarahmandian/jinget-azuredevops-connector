using Jinget.Handlers.ExternalServiceHandlers.ServiceHandler;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Jinget.AzureDevOps.Connector
{
    public class AzureDevOpsConnectorEvents
    {
        public event EventHandler<string> ResponseReceived;
        public virtual void OnResponseReceived(string response) => ResponseReceived?.Invoke(this, response);
    }

    public abstract class AzureDevOpsConnector : ServiceHandler<AzureDevOpsConnectorEvents>
    {
        private readonly string pat;
        private readonly string baseUrl;
        private readonly string apiVersion;

        /// <summary>
        /// indicates the web api request path first segment. usually starts with '_apis/...'
        /// </summary>
        protected string RootPathSegment;

        public AzureDevOpsConnector(string pat, string baseUrl, string apiVersion, string rootPathSegment) : base(baseUrl)
        {
            this.pat = pat;
            this.baseUrl = baseUrl;
            this.apiVersion = apiVersion;
            RootPathSegment = rootPathSegment;
        }
        private (string FullUrl, string Path) GetUrl(string path, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true)
        {
            urlParameters ??= new Dictionary<string, string>();
            UriBuilder uri = new UriBuilder($"{baseUrl}/{path}");
            var queryString = HttpUtility.ParseQueryString(uri.Query);
            foreach (var item in urlParameters)
            {
                queryString.Add(item.Key, item.Value);
            }
            if (appendApiVersion)
                queryString["api-version"] = apiVersion;
            uri.Query = queryString.ToString();
            return (uri.Uri.ToString(), uri.Uri.AbsoluteUri.Replace(baseUrl, "").TrimStart('/'));
        }
        private Dictionary<string, string> GetDefaultHeaders(Dictionary<string, string>? headers = null)
        {
            headers ??= new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(pat))
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat)));
                headers.Add("Authorization", $"Basic {credentials}");
            }
            return headers;
        }

        private object NormalizeRequestBody(object requestBody, string contentType)
        {
            return contentType == MediaTypeNames.Application.Json ? requestBody : JsonSerializer.Serialize(requestBody);
        }

        protected async Task<T> GetAsync<T>(string path, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true)
        {
            string uriPath = GetUrl(path, urlParameters, appendApiVersion).Path;
            using var response = await HttpClientFactory.GetAsync(uriPath, GetDefaultHeaders());

            var responseBody = await response.Content.ReadAsStringAsync();

            Events.OnResponseReceived(responseBody);

            response.EnsureSuccessStatusCode();

#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<T>(responseBody);
#pragma warning restore CS8603 // Possible null reference return.
        }


        protected async Task<TResponseBody> PostAsync<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true, string contentType = MediaTypeNames.Application.Json)
        {
            using var response = await HttpClientFactory.PostAsync(
                GetUrl(path, urlParameters, appendApiVersion).Path,
                NormalizeRequestBody(requestBody, contentType),
                headers: GetDefaultHeaders(new Dictionary<string, string>
                {
                    {"Content-Type",contentType }
                }));

            var responseBody = await response.Content.ReadAsStringAsync();

            Events.OnResponseReceived(responseBody);

            response.EnsureSuccessStatusCode();

#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<TResponseBody>(responseBody);
#pragma warning restore CS8603 // Possible null reference return.
        }

        protected async Task<bool> PostAsync<T>(string path, T requestBody, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true, string contentType = MediaTypeNames.Application.Json)
        {
            using var response = await HttpClientFactory.PostAsync(
                GetUrl(path, urlParameters, appendApiVersion).Path,
                NormalizeRequestBody(requestBody, contentType),
                headers: GetDefaultHeaders(new Dictionary<string, string>
                {
                    {"Content-Type",contentType }
                }));

            var responseBody = await response.Content.ReadAsStringAsync();
            Events.OnResponseReceived(responseBody);

            return response.IsSuccessStatusCode;
        }

        protected async Task<bool> DeleteAsync(string path, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(GetUrl(path, urlParameters, appendApiVersion).FullUrl),
                Method = HttpMethod.Delete
            };
            foreach (var item in GetDefaultHeaders())
            {
                requestMessage.Headers.TryAddWithoutValidation(item.Key, item.Value);
            }

            using var response = await HttpClientFactory.SendAsync(requestMessage);

            var responseBody = await response.Content.ReadAsStringAsync();
            Events.OnResponseReceived(responseBody);

            return response.IsSuccessStatusCode;
        }
    }
}
