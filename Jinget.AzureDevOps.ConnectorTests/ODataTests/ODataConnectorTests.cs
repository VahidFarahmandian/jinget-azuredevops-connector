using Jinget.AzureDevOps.Connector.Board.Tests;
using Jinget.AzureDevOps.Connector.Projects;
using Jinget.AzureDevOps.ConnectorTests.ODataTests.CustomTypes;

namespace Jinget.AzureDevOps.ConnectorTests.ProjectsTests
{
    [TestClass()]
    public class ODataConnectorTests : _BaseTests
    {
        ODataConnector connector;

        [TestInitialize]
        public void TestInitialize() => connector = new ODataConnector(pat, "https://analytics.dev.azure.com", organization, project: "PMOSample");

        [TestMethod()]
        public async Task should_get_workitems_from_one_project()
        {
            var queries = new Dictionary<string, string>()
            {
                {"$filter","WorkItemType eq 'Task' and State eq 'To Do'"},
                {"$select","WorkItemId,Title,AssignedTo,State" }
            };

            var result = await connector.QueryAsync("WorkItems", queries);
            Assert.IsTrue(result.odatacontext != null);
        }

        [TestMethod()]
        public async Task should_get_workitems_from_one_project_and_deserial_it_to_custom_type()
        {
            var queries = new Dictionary<string, string>()
            {
                {"$filter","WorkItemType eq 'Task' and State eq 'To Do'"},
                {"$select","WorkItemId,Title,AssignedTo,State" }
            };

            var result = await connector.QueryAsync<MyCustomType>("WorkItems", queries);
            Assert.IsTrue(result.odatacontext != null);
            Assert.IsTrue(result.value[0].Title != "");
        }

        [TestMethod()]
        public async Task should_get_workitems_from_multiple_projects()
        {
            var otherConnector = new ODataConnector(pat, "https://analytics.dev.azure.com", organization);

            var queries = new Dictionary<string, string>()
            {
                {"$filter","(Project/ProjectName eq 'PMOSample' or Project/ProjectName eq 'Test2') and WorkItemType eq 'Task' and State eq 'To Do'"},
                {"$select","WorkItemId,Title,AssignedTo,State" }
            };

            var result = await otherConnector.QueryAsync("WorkItems", queries);
            Assert.IsTrue(result.odatacontext != null);
        }
    }
}
