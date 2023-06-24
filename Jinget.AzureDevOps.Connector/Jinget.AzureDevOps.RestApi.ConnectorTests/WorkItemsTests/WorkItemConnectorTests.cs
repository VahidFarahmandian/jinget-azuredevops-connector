using Jinget.AzureDevOps.RestApi.Connector.Board.Tests;
using Jinget.AzureDevOps.RestApi.Connector.Projects;
using Jinget.AzureDevOps.RestApi.Connector.Projects.ViewModels;
using Jinget.AzureDevOps.RestApi.Connector.WorkItem.ViewModels;

namespace Jinget.AzureDevOps.RestApi.ConnectorTests.ProjectsTests
{
    [TestClass()]
    public class WorkItemConnectorTests : _BaseTests
    {
        WorkItemConnector connector;

        [TestInitialize]
        public void TestInitialize() => connector = new WorkItemConnector(pat, organization, "PMOSample");

        //[TestMethod()]
        //public async Task should_create_new_project()
        //{
        //    NewProjectViewModel newProject = new()
        //    {
        //        name = "My Test Project",
        //        capabilities = new NewProjectViewModel.Capabilities
        //        {
        //            versioncontrol = new NewProjectViewModel.Versioncontrol
        //            {
        //                sourceControlType = "Git"
        //            },
        //            processTemplate = new NewProjectViewModel.Processtemplate
        //            {
        //                templateTypeId = "b8a3a935-7e91-48b8-a94c-606d37c3e9f2"
        //            }
        //        },
        //        description = "My new project description"
        //    };
        //    bool result = await connector.Create(newProject);

        //    Assert.IsTrue(result);
        //}


        [TestMethod()]
        public async Task should_get_list_of_workitems()
        {
            GetWorkItemBatchModel request = new GetWorkItemBatchModel
            {
                fields = new string[]
                {
                    "System.Id",
                    "System.Title",
                    "System.WorkItemType",
                    "Microsoft.VSTS.Scheduling.RemainingWork"
                },
                ids = new string[] { "96", "97", "98" }
            };
            WorkItemBatchViewModel result = await connector.ListBatch<WorkItemBatchViewModel>(request);

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_list_of_workitems_using_wiql()
        {
            string query = "SELECT System.Id, System.Title, System.WorkItemType, Microsoft.VSTS.Scheduling.RemainingWork FROM WorkItems";
            var result = await connector.ListWIQL<WorkItemBatchViewModel>(query);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod()]
        public async Task should_create_new_workitem()
        {
            List<NewWorkItemModel> properties = new List<NewWorkItemModel>()
            {
                new NewWorkItemModel()
                {
                    path="/fields/System.Title",
                    value="Sample WorkItem"
                },
                new NewWorkItemModel()
                {
                    path="/fields/System.Description",
                    value="Sample description"
                },
                new NewWorkItemModel()
                {
                    path="/fields/System.History",
                    value="Sample comment"
                },
                new NewWorkItemModel()
                {
                    path="/fields/System.AssignedTo",
                    value="farahmandian2011@gmail.com"
                },
                   new NewWorkItemModel()
                {
                    path="/fields/System.AreaPath",
                    value="PMOSample"
                }
            };
            var result = await connector.Create("$Task", properties);

            Assert.IsTrue(result.id > 0);
        }

    }
}