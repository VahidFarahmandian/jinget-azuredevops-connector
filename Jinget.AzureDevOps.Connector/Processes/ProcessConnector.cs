using Jinget.AzureDevOps.Connector.Processes.ViewModels;

namespace Jinget.AzureDevOps.Connector.Processes;

public class ProcessConnector : AzureDevOpsConnector
{
    public ProcessConnector(
        IServiceProvider serviceProvider,
        string pat,
        string organization,
        string apiVersion = Constants.DefaultRestApiVersion) :
        base(serviceProvider, pat, $"{Constants.DefaultRestApiBaseUrl}/{organization}", apiVersion, "_apis/process/processes")
    {
    }
    public ProcessConnector(
        IServiceProvider serviceProvider,
        string pat,
        string url,
        string organization,
        string apiVersion = Constants.DefaultRestApiVersion) :
        base(serviceProvider, pat, $"{url}/{organization}", apiVersion, "_apis/process/processes")
    {
    }

    /// <summary>
    /// Get all the process templates defined in organization/collection
    /// </summary>
    /// <returns></returns>
    public async Task<ProcessListViewModel?> ListAsync()
        => await GetAsync<ProcessListViewModel>(RootPathSegment);

    /// <summary>
    /// Get the detail information about a specific process template
    /// </summary>
    public async Task<ProcessViewModel?> GetAsync(Guid processId)
        => await GetAsync<ProcessViewModel>($"{RootPathSegment}/{processId}", appendApiVersion: false);
}
