using Jinget.AzureDevOps.Connector.WorkItem.ViewModels;

namespace Jinget.AzureDevOps.Connector.WorkItem;

public class WorkItemConnector : AzureDevOpsConnector
{
    public WorkItemConnector(
        IServiceProvider serviceProvider,
        string pat,
        string organization,
        string project,
        string team = "",
        string apiVersion = Constants.DefaultRestApiVersion) :
        base(serviceProvider, pat, $"{Constants.DefaultRestApiBaseUrl}/{organization}/{project}/{team}", apiVersion, "_apis/wit")
    {
    }
    public WorkItemConnector(
        IServiceProvider serviceProvider,
        string pat,
        string url,
        string organization,
        string project,
        string team = "",
        string apiVersion = Constants.DefaultRestApiVersion) :
        base(serviceProvider, pat, $"{url}/{organization}/{project}/{team}", apiVersion, "_apis/wit")
    {
    }

    /// <summary>
    /// Get all the work items in a project (Maximum 200)
    /// </summary>
    public async Task<WorkItemBatchViewModel?> ListBatchAsync(GetWorkItemBatchModel requestBody, Dictionary<string, string>? urlParameters = null)
        => await PostAsync<GetWorkItemBatchModel, WorkItemBatchViewModel>($"{RootPathSegment}/workitemsbatch", requestBody, urlParameters);

    /// <summary>
    /// Get all the work items in a project (Maximum 200) and deserialize it to the custom T type
    /// </summary>
    public async Task<T?> ListBatchAsync<T>(GetWorkItemBatchModel requestBody, Dictionary<string, string>? urlParameters = null) where T : class, new()
        => await PostAsync<GetWorkItemBatchModel, T>($"{RootPathSegment}/workitemsbatch", requestBody, urlParameters);

    /// <summary>
    /// Get all the work items in a project (Maximum 200) using Work Item Query Language and deserialize it to the custom T type
    /// </summary>
    public async Task<List<WorkItemViewModel>> ListWIQLAsync<T>(string query, Dictionary<string, string>? urlParameters = null)
    {
        var wiqlResult = await PostAsync<object, WorkItemWIQLViewModel>($"{RootPathSegment}/wiql", new { query }, urlParameters);
        List<WorkItemViewModel> result = [];
        if (wiqlResult != null)
        {
            foreach (var item in wiqlResult.workItems)
            {
                var response = await GetAsync<WorkItemViewModel>($"{RootPathSegment}/workitems/{item.id}");
                if (response != null)
                {
                    result.Add(response);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Create a new workitem
    /// </summary>
    public async Task<NewWorkItemViewModel?> CreateAsync(string type, List<NewWorkItemModel> workItemProperties, Dictionary<string, string>? urlParameters = null)
        => await PostAsync<List<NewWorkItemModel>, NewWorkItemViewModel>($"{RootPathSegment}/workitems/{type}", workItemProperties, urlParameters, contentType: "application/json-patch+json");
}
