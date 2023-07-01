using Jinget.AzureDevOps.Connector.Board.Tests;
using Jinget.AzureDevOps.Connector.Projects;
using Jinget.AzureDevOps.Connector.Projects.ViewModels;

namespace Jinget.AzureDevOps.ConnectorTests.ProjectsTests
{
    [TestClass()]
    public class ProjectConnectorTests : _BaseTests
    {
        ProjectConnector connector;

        private async Task<ProjectViewModel> InitTestAsync()
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
                        templateTypeId = "b8a3a935-7e91-48b8-a94c-606d37c3e9f2"
                    }
                },
                description = "My new project description"
            };
            await connector.CreateAsync(newProject);

            return await connector.GetAsync(projectName);
        }

        private async Task<bool> Cleanup(string id) => await connector.DeleteAsync(Guid.Parse(id));

        [TestInitialize]
        public void TestInitialize() => connector = new ProjectConnector(pat, organization, apiVersion: "7.0");

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
                        templateTypeId = "b8a3a935-7e91-48b8-a94c-606d37c3e9f2"
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
            ProjectsListViewModel result = await connector.ListAsync();

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_first_two_projects()
        {
            Dictionary<string, string> urlParameters = new()
            {
                { "$top","2"}
            };
            ProjectsListViewModel result = await connector.ListAsync(urlParameters);

            Assert.IsTrue(result.count == 2);
        }

        [TestMethod()]
        public async Task should_get_specific_project_details()
        {
            var result = await InitTestAsync();
            await Cleanup(result.id);
            Assert.IsTrue(result.id != "");
        }

        [TestMethod()]
        public async Task should_get_specific_project_properties()
        {
            var projectConnector = new ProjectConnector(pat, organization, apiVersion: "7.0-preview.1");
            var init = await InitTestAsync();

            ProjectPropertiesViewModel result = await projectConnector.GetPropertiesAsync(Guid.Parse(init.id));

            await Cleanup(init.id);
            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_delete_project()
        {
            var init = await InitTestAsync();

            bool result = await Cleanup(init.id);

            Assert.IsTrue(result);
        }
    }
}