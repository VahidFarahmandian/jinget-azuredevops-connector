using Jinget.AzureDevOps.Connector.Teams.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.Connector.Teams
{
    public class TeamConnector : AzureDevOpsConnector
    {
        public TeamConnector(string pat, string organization, string apiVersion = Constants.DefaultRestApiVersion) : base(pat, $"{Constants.DefaultRestApiBaseUrl}/{organization}", apiVersion, "_apis/projects")
        {
        }
        public TeamConnector(string pat, string url, string organization, string apiVersion = Constants.DefaultRestApiVersion) : base(pat, $"{url}/{organization}", apiVersion, "_apis/projects")
        {
        }

        /// <summary>
        /// Get all the teams in an organization/collection
        /// </summary>
        public async Task<TeamListViewModel> GetAllTeamsAsync(Dictionary<string, string>? urlParameters = null) => await GetAsync<TeamListViewModel>("_apis/teams", urlParameters);

        /// <summary>
        /// Get all the teams in a specific project inside an organization/collection
        /// </summary>
        public async Task<TeamListViewModel> GetTeamsAsync(Guid projectId, Dictionary<string, string>? urlParameters = null) => await GetAsync<TeamListViewModel>($"_apis/projects/{projectId}/teams", urlParameters);

        /// <summary>
        /// Get the detail information about a specific team inside a specific project
        /// </summary>
        public async Task<TeamViewModel> GetAsync(Guid projectId, string teamId, Dictionary<string, string>? urlParameters = null) => await GetAsync<TeamViewModel>($"_apis/projects/{projectId}/teams/{teamId}", urlParameters);

    }
}
