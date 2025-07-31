using Jinget.AzureDevOps.Connector.Projects;
using Jinget.AzureDevOps.Connector.Projects.ViewModels;

namespace Jinget.AzureDevOps.ConnectorTests.ProjectsTests;

[TestClass()]
public class ProjectConnectorTests : BaseTests
{
    ProjectConnector connector;

    private async Task<ProjectViewModel?> InitTestAsync()
    {
        string projectName = $"Test{Guid.NewGuid()}";
        NewProjectModel newProject = new()
        {
            name = projectName,
            capabilities = new NewProjectModel.Capabilities
            {
                versioncontrol = new NewProjectModel.Versioncontrol
                {
                    sourceControlType = "Git"
                },
                processTemplate = new NewProjectModel.Processtemplate
                {
                    templateTypeId = basicProcessId
                }
            },
            description = "My new project description"
        };
        await connector.CreateAsync(newProject);
        Thread.Sleep(500);
        return await connector.GetAsync(projectName);
    }

    private async Task<bool> CleanupAsync(string? id)
        => string.IsNullOrWhiteSpace(id) || await connector.DeleteAsync(Guid.Parse(id));

    [TestInitialize]
    public void TestInitialize()
        => connector = new ProjectConnector(ServiceProvider, pat, organization);

    [TestMethod()]
    public async Task should_create_new_project()
    {
        string projectName = $"Test{Guid.NewGuid()}";
        NewProjectModel newProject = new()
        {
            name = projectName,
            capabilities = new NewProjectModel.Capabilities
            {
                versioncontrol = new NewProjectModel.Versioncontrol
                {
                    sourceControlType = "Git"
                },
                processTemplate = new NewProjectModel.Processtemplate
                {
                    templateTypeId = basicProcessId
                }
            },
            description = "My new project description"
        };
        bool result = await connector.CreateAsync(newProject);

        Assert.IsTrue(result);
    }


    [TestMethod()]
    public async Task should_get_list_of_projects()
    {
        var result = await connector.ListAsync();
        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.count);
    }

    [TestMethod()]
    public async Task should_get_first_two_projects()
    {
        Dictionary<string, string> urlParameters = new()
        {
            { "$top","2"}
        };
        var result = await connector.ListAsync(urlParameters);
        Assert.AreEqual(2, result?.count);
    }

    [TestMethod()]
    public async Task should_get_specific_project_details()
    {
        var result = await connector.GetAsync(projectName);
        Assert.AreNotEqual("", result?.id);
    }

    [TestMethod()]
    public async Task should_get_specific_project_properties()
    {
        connector = new ProjectConnector(ServiceProvider, pat, organization, apiVersion: "7.1-preview.1");
        var result = await connector.GetPropertiesAsync(Guid.Parse(projectId));
        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.count);
    }

    [TestMethod()]
    public async Task should_delete_project()
    {
        var init = await InitTestAsync();
        bool result = await CleanupAsync(init.id);
        Assert.IsTrue(result);
    }
}