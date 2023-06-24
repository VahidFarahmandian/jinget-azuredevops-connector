using Jinget.AzureDevOps.Connector.Teams.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jinget.AzureDevOps.Connector.Teams
{
    public class TeamConnector : AzureDevOpsConnector
    {
        public TeamConnector(string pat, string organization) : base(pat, $"{Constants.DefaultBaseUrl}/{organization}", "_apis/projects")
        {
        }
        public TeamConnector(string pat, string url, string organization) : base(pat, $"{url}/{organization}", "_apis/projects")
        {
        }

        /// <summary>
        /// Get all the teams in an organization/collection
        /// </summary>
        public async Task<TeamListViewModel> GetAllTeams(Dictionary<string, string>? urlParameters = null) => await GetAsync<TeamListViewModel>("_apis/teams", urlParameters);

        /// <summary>
        /// Get all the teams in a specific project inside an organization/collection
        /// </summary>
        public async Task<TeamListViewModel> GetTeams(Guid projectId, Dictionary<string, string>? urlParameters = null) => await GetAsync<TeamListViewModel>($"_apis/projects/{projectId}/teams", urlParameters);

        /// <summary>
        /// Get the detail information about a specific team inside a specific project
        /// </summary>
        public async Task<TeamViewModel> Get(Guid projectId, string teamId, Dictionary<string, string>? urlParameters = null) => await GetAsync<TeamViewModel>($"_apis/projects/{projectId}/teams/{teamId}", urlParameters);

    }
}
