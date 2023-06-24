using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Jinget.AzureDevOps.RestApi.Connector
{
    public abstract class AzureDevOpsConnector
    {
        private readonly HttpClient client;
        private readonly string apiVersion;
        /// <summary>
        /// indicates the web api request path first segment. usually starts with '_apis/...'
        /// </summary>
        protected string RootPathSegment;

        public AzureDevOpsConnector(string pat) : this(pat, "") { }
        public AzureDevOpsConnector(string pat, string rootPathSegment) : this(pat, Constants.DefaultBaseUrl, rootPathSegment) { }
        public AzureDevOpsConnector(string pat, string url, string rootPathSegment) : this(pat, url, Constants.DefaultApiVersion, rootPathSegment) { }
        public AzureDevOpsConnector(string pat, string url, string apiVersion, string rootPathSegment)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{url}")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(pat))
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat)));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            }

            this.apiVersion = apiVersion;
            RootPathSegment = rootPathSegment;
        }
        private string GetUrl(string path, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true)
        {
            urlParameters ??= new Dictionary<string, string>();
            UriBuilder uri = new UriBuilder($"{client.BaseAddress}/{path}");
            var queryString = HttpUtility.ParseQueryString(uri.Query);
            foreach (var item in urlParameters)
            {
                queryString.Add(item.Key, item.Value);
            }
            if (appendApiVersion)
                queryString["api-version"] = apiVersion;
            uri.Query = queryString.ToString();
            return uri.ToString();
        }

        protected async Task<T> GetAsync<T>(string path, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true)
        {
            using var response = await client.GetAsync(GetUrl(path, urlParameters, appendApiVersion));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseBody);
        }


        protected async Task<TResponseBody> PostAsync<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true, string contentType = MediaTypeNames.Application.Json)
        {
            using StringContent content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, contentType);

            using var response = await client.PostAsync(GetUrl(path, urlParameters, appendApiVersion), content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponseBody>(responseBody);
        }

        protected async Task<bool> PostAsync<T>(string path, T requestBody, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true, string contentType = MediaTypeNames.Application.Json)
        {
            using StringContent content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, contentType);

            using var response = await client.PostAsync(GetUrl(path, urlParameters, appendApiVersion), content);
            return response.IsSuccessStatusCode;
        }

        protected async Task<bool> DeleteAsync(string path, Dictionary<string, string>? urlParameters = null, bool appendApiVersion = true)
        {
            using var response = await client.DeleteAsync(GetUrl(path, urlParameters, appendApiVersion));
            return response.IsSuccessStatusCode;
        }
    }
}
