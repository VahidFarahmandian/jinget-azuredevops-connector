using Jinget.AzureDevOps.Connector.Projects.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.Connector.Projects
{
    public class ProjectConnector : AzureDevOpsConnector
    {
        public ProjectConnector(string pat, string organization, string apiVersion = Constants.DefaultRestApiVersion) : base(pat, $"{Constants.DefaultRestApiBaseUrl}/{organization}", apiVersion, "_apis/projects")
        {
        }
        public ProjectConnector(string pat, string url, string organization, string apiVersion = Constants.DefaultRestApiVersion) : base(pat, $"{url}/{organization}", apiVersion, "_apis/projects")
        {
        }

        /// <summary>
        /// Get all the projects in an organization/collection
        /// </summary>
        public async Task<ProjectsListViewModel> ListAsync(Dictionary<string, string>? urlParameters = null) => await GetAsync<ProjectsListViewModel>(RootPathSegment, urlParameters);

        /// <summary>
        /// Get the detail information about a specific project
        /// </summary>
        public async Task<ProjectViewModel> GetAsync(string projectName, Dictionary<string, string>? urlParameters = null) => await GetAsync<ProjectViewModel>($"{RootPathSegment}/{projectName}", urlParameters);

        /// <summary>
        /// Get all the properties of a specific project
        /// </summary>
        public async Task<ProjectPropertiesViewModel> GetPropertiesAsync(Guid projectId, Dictionary<string, string>? urlParameters = null) => await GetAsync<ProjectPropertiesViewModel>($"{RootPathSegment}/{projectId}/properties", urlParameters);

        /// <summary>
        /// Create a new project in Azure DevOps organization/collection
        /// </summary>
        public async Task<bool> CreateAsync(NewProjectModel newProject, Dictionary<string, string>? urlParameters = null) => await PostAsync($"{RootPathSegment}", newProject, urlParameters);

        /// <summary>
        /// Delete a project in organization/collection
        /// </summary>
        public async Task<bool> DeleteAsync(Guid projectId, Dictionary<string, string>? urlParameters = null) => await DeleteAsync($"{RootPathSegment}/{projectId}", urlParameters);
    }
}
