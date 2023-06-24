using Jinget.AzureDevOps.RestApi.Connector.Board.Tests;
using Jinget.AzureDevOps.RestApi.Connector.Projects;
using Jinget.AzureDevOps.RestApi.Connector.Projects.ViewModels;

namespace Jinget.AzureDevOps.RestApi.ConnectorTests.ProjectsTests
{
    [TestClass()]
    public class ProjectConnectorTests : _BaseTests
    {
        ProjectConnector connector;

        [TestInitialize]
        public void TestInitialize() => connector = new ProjectConnector(pat, organization);

        [TestMethod()]
        public async Task should_create_new_project()
        {
            NewProjectModel newProject = new()
            {
                name = "My Test Project",
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
            bool result = await connector.Create(newProject);

            Assert.IsTrue(result);
        }


        [TestMethod()]
        public async Task should_get_list_of_projects()
        {
            ProjectsListViewModel result = await connector.List();

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_first_two_projects()
        {
            Dictionary<string, string> urlParameters = new()
            {
                { "$top","2"}
            };
            ProjectsListViewModel result = await connector.List(urlParameters);

            Assert.IsTrue(result.count == 2);
        }

        [TestMethod()]
        public async Task should_get_specific_project_details()
        {
            ProjectViewModel result = await connector.Get("PMOSample");

            Assert.IsTrue(result.id != "");
        }

        [TestMethod()]
        public async Task should_get_specific_project_properties()
        {
            ProjectPropertiesViewModel result = await connector.GetProperties(Guid.Parse("5ec32e71-d47d-4908-8107-069a952c9550"));

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_delete_project()
        {
            bool result = await connector.Delete(Guid.Parse("d39dabc2-b7d4-4466-b6d3-dc3074da5f52"));
            Assert.IsTrue(result);
        }
    }
}