using Jinget.AzureDevOps.RestApi.Connector.Projects.ViewModels;
using Jinget.AzureDevOps.RestApi.Connector.WorkItem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.RestApi.Connector.Projects
{
    public class WorkItemConnector : AzureDevOpsConnector
    {
        public WorkItemConnector(string pat, string organization, string project, string team = "") : base(pat, $"{Constants.DefaultBaseUrl}/{organization}/{project}/{team}", "7.0", "_apis/wit")
        {
        }
        public WorkItemConnector(string pat, string url, string organization, string project, string team = "") : base(pat, $"{url}/{organization}/{project}/{team}", "7.0", "_apis/wit")
        {
        }

        /// <summary>
        /// Get all the work items in a project (Maximum 200)
        /// </summary>
        public async Task<WorkItemBatchViewModel> ListBatch(GetWorkItemBatchModel requestBody, Dictionary<string, string>? urlParameters = null) => await PostAsync<GetWorkItemBatchModel, WorkItemBatchViewModel>($"{RootPathSegment}/workitemsbatch", requestBody, urlParameters);

        /// <summary>
        /// Get all the work items in a project (Maximum 200) and deserialize it to the custom T type
        /// </summary>
        public async Task<T> ListBatch<T>(GetWorkItemBatchModel requestBody, Dictionary<string, string>? urlParameters = null) => await PostAsync<GetWorkItemBatchModel, T>($"{RootPathSegment}/workitemsbatch", requestBody, urlParameters);

        /// <summary>
        /// Get all the work items in a project (Maximum 200) using Work Item Query Language and deserialize it to the custom T type
        /// </summary>
        public async Task<List<WorkItemViewModel>> ListWIQL<T>(string query, Dictionary<string, string>? urlParameters = null)
        {
            var wiqlResult = await PostAsync<object, WorkItemWIQLViewModel>($"{RootPathSegment}/wiql", new { query = query }, urlParameters);
            List<WorkItemViewModel> result = new List<WorkItemViewModel>();
            foreach (var item in wiqlResult.workItems)
            {
                var response = await GetAsync<WorkItemViewModel>($"{RootPathSegment}/workitems/{item.id}");
                result.Add(response);
            }
            return result;
        }

        ///// <summary>
        ///// Get the detail information about a specific project
        ///// </summary>
        //public async Task<ProjectViewModel> Get(string projectName, Dictionary<string, string>? urlParameters = null) => await GetAsync<ProjectViewModel>($"{RootPathSegment}/{projectName}", urlParameters);

        ///// <summary>
        ///// Get all the properties of a specific project
        ///// </summary>
        //public async Task<ProjectPropertiesViewModel> GetProperties(Guid projectId, Dictionary<string, string>? urlParameters = null) => await GetAsync<ProjectPropertiesViewModel>($"{RootPathSegment}/{projectId}/properties", urlParameters);

        /// <summary>
        /// Create a new workitem
        /// </summary>
        public async Task<NewWorkItemViewModel> Create(string type, List<NewWorkItemModel> workItemProperties, Dictionary<string, string>? urlParameters = null) => await PostAsync<List<NewWorkItemModel>, NewWorkItemViewModel>($"{RootPathSegment}/workitems/{type}", workItemProperties, urlParameters, contentType: "application/json-patch+json");

        ///// <summary>
        ///// Delete a project in organization/collection
        ///// </summary>
        //public async Task<bool> Delete(Guid projectId, Dictionary<string, string>? urlParameters = null) => await DeleteAsync($"{RootPathSegment}/{projectId}", urlParameters);
    }
}
