﻿using Jinget.AzureDevOps.Connector.Processes.ViewModels;
using System;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.Connector.Processes
{
    public class ProcessConnector : AzureDevOpsConnector
    {
        public ProcessConnector(string pat, string organization, string apiVersion = Constants.DefaultRestApiVersion) : base(pat, $"{Constants.DefaultRestApiBaseUrl}/{organization}", apiVersion, "_apis/process/processes")
        {
        }
        public ProcessConnector(string pat, string url, string organization, string apiVersion = Constants.DefaultRestApiVersion) : base(pat, $"{url}/{organization}", apiVersion, "_apis/process/processes")
        {
        }

        /// <summary>
        /// Get all the process templates defined in organization/collection
        /// </summary>
        /// <returns></returns>
        public async Task<ProcessListViewModel> ListAsync() => await GetAsync<ProcessListViewModel>(RootPathSegment);

        /// <summary>
        /// Get the detail information about a specific process template
        /// </summary>
        public async Task<ProcessViewModel> GetAsync(Guid processId) => await GetAsync<ProcessViewModel>($"{RootPathSegment}/{processId}", appendApiVersion: false);
    }
}
