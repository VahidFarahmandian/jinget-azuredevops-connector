using Jinget.AzureDevOps.RestApi.Connector.Processes.ViewModels;
using System;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.RestApi.Connector.Processes
{
    public class ProcessConnector : AzureDevOpsConnector
    {
        public ProcessConnector(string pat, string organization) : base(pat, $"{Constants.DefaultBaseUrl}/{organization}", "_apis/process/processes")
        {
        }
        public ProcessConnector(string pat, string url, string organization) : base(pat, $"{url}/{organization}", "_apis/process/processes")
        {
        }

        /// <summary>
        /// Get all the process templates defined in organization/collection
        /// </summary>
        /// <returns></returns>
        public async Task<ProcessListViewModel> List() => await GetAsync<ProcessListViewModel>(RootPathSegment);

        /// <summary>
        /// Get the detail information about a specific process template
        /// </summary>
        public async Task<ProcessViewModel> Get(Guid processId) => await GetAsync<ProcessViewModel>($"{RootPathSegment}/{processId}", appendApiVersion: false);
    }
}
