using Jinget.AzureDevOps.Connector.Projects.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.Connector.Projects
{
    public class ODataConnector : AzureDevOpsConnector
    {
        /// <summary>
        /// This constructor is used for cross project queries
        /// </summary>
        public ODataConnector(string pat, string url, string organization, string oDataApiVersion = Constants.DefaultODataApiVersion) : base(pat, $"{url}/{organization}", "", $"_odata/{oDataApiVersion}")
        {
        }

        /// <summary>
        /// This constructor is used for querying a ONE specific project
        /// </summary>
        /// <param name="pat">Personal access token obtained from Azure DevOps</param>
        /// <param name="url">Base url of Azure DevOps. For Azure DevOps Service this usually is something like https://dev.azure.com</param>
        /// <param name="organization">name of your organization or collection</param>
        /// <param name="oDataApiVersion">this is OData specific api version and is different from Azure DevOps RestApi version</param>
        public ODataConnector(string pat, string url, string organization, string project, string oDataApiVersion = Constants.DefaultODataApiVersion) : base(pat, $"{url}/{organization}/{project}", "", $"_odata/{oDataApiVersion}")
        {
        }

        /// <summary>
        /// send a request to odata endpoint.
        /// </summary>
        /// <param name="scopeName">Can be anything like WorkItems, Projects, Areas etc</param>
        /// <param name="queries">Usually contains $select, $filter, $orderby etc</param>
        public async Task<ODataResultViewModel> QueryAsync(string scopeName, Dictionary<string, string>? queries = null) => await GetAsync<ODataResultViewModel>($"{RootPathSegment}/{scopeName}", queries, appendApiVersion: false);

        public async Task<T> QueryAsync<T>(string scopeName, Dictionary<string, string>? queries = null) => await GetAsync<T>($"{RootPathSegment}/{scopeName}", queries, appendApiVersion: false);
    }
}
