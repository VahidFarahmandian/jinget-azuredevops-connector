using System.Diagnostics;

namespace Jinget.AzureDevOps.Connector;

public abstract class AzureDevOpsConnector(
    IServiceProvider serviceProvider,
    string pat,
    string baseUrl,
    string apiVersion,
    string rootPathSegment)
{
    /// <summary>
    /// indicates the web api request path first segment. usually starts with '_apis/...'
    /// </summary>
    protected string RootPathSegment = rootPathSegment;

    private (string FullUrl, string Path) GetUrl(
        string path,
        Dictionary<string, string>? urlParameters = null,
        bool appendApiVersion = true)
    {
        urlParameters ??= [];
        UriBuilder uri = new($"{baseUrl}/{path}");
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
        headers ??= [];

        if (!string.IsNullOrWhiteSpace(pat))
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat)));
            headers.Add("Authorization", $"Basic {credentials}");
        }
        return headers;
    }

    private static object? NormalizeRequestBody(object? requestBody, string contentType)
        => contentType == MediaTypeNames.Application.Json ||
        contentType == MediaTypeNames.Application.JsonPatch ? requestBody : JsonSerializer.Serialize(requestBody);

    protected async Task<T?> GetAsync<T>(
        string path,
        Dictionary<string, string>? urlParameters = null,
        bool appendApiVersion = true)
        where T : class, new()
    {
        string uriPath = GetUrl(path, urlParameters, appendApiVersion).Path;

        var serviceHandler = new JingetServiceHandler<T>(serviceProvider, baseUrl);
        
        return await serviceHandler.GetAsync<T>(uriPath, GetDefaultHeaders());
    }

    protected async Task<TResponseBody?> PostAsync<TRequestBody, TResponseBody>(
        string path,
        TRequestBody requestBody,
        Dictionary<string, string>? urlParameters = null,
        bool appendApiVersion = true,
        string contentType = MediaTypeNames.Application.Json)
        where TResponseBody : class, new()
    {
        var serviceHandler = new JingetServiceHandler<TResponseBody>(serviceProvider, baseUrl);
        
        return await serviceHandler.PostAsync<TResponseBody>(
            GetUrl(path, urlParameters, appendApiVersion).Path,
            NormalizeRequestBody(requestBody, contentType),
            headers: GetDefaultHeaders(new Dictionary<string, string>
            {
                {"Content-Type",contentType }
            }));
    }

    protected async Task<bool> PostAsync<T>(
        string path,
        T requestBody,
        Dictionary<string, string>? urlParameters = null,
        bool appendApiVersion = true,
        string contentType = MediaTypeNames.Application.Json)
        where T : class, new()
    {
        bool isSucceeded = false;
        var serviceHandler = new JingetServiceHandler<T>(serviceProvider, baseUrl);
        serviceHandler.Events.ServiceCalledAsync += async (sender, e) =>
        {
            await Task.CompletedTask;
            isSucceeded = e.IsSuccessStatusCode;
        };
        await serviceHandler.PostAsync<T>(
            GetUrl(path, urlParameters, appendApiVersion).Path,
            NormalizeRequestBody(requestBody, contentType),
            headers: GetDefaultHeaders(new Dictionary<string, string>
            {
                {"Content-Type",contentType }
            }));
        return isSucceeded;
    }

    protected async Task<bool> DeleteAsync(
        string path,
        Dictionary<string, string>? urlParameters = null,
        bool appendApiVersion = true)
    {
        bool isSucceeded = false;
        var serviceHandler = new JingetServiceHandler<object>(serviceProvider, baseUrl);
        serviceHandler.Events.ServiceCalledAsync += async (sender, e) =>
        {
            await Task.CompletedTask;
            isSucceeded = e.IsSuccessStatusCode;
        };
        HttpRequestMessage requestMessage = new()
        {
            RequestUri = new Uri(GetUrl(path, urlParameters, appendApiVersion).FullUrl),
            Method = HttpMethod.Delete
        };
        foreach (var item in GetDefaultHeaders())
        {
            requestMessage.Headers.TryAddWithoutValidation(item.Key, item.Value);
        }

        await serviceHandler.SendAsync(requestMessage);
        return isSucceeded;
    }
}
