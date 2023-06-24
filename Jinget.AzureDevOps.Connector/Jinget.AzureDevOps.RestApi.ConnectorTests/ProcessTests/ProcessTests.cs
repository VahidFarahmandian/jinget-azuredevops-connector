using Jinget.AzureDevOps.RestApi.Connector.Board.Tests;
using Jinget.AzureDevOps.RestApi.Connector.Processes;
using Jinget.AzureDevOps.RestApi.Connector.Processes.ViewModels;

namespace Jinget.AzureDevOps.RestApi.ConnectorTests.ProjectsTests
{
    [TestClass()]
    public class ProcessTests : _BaseTests
    {
        ProcessConnector connector;

        [TestInitialize]
        public void TestInitialize() => connector = new ProcessConnector(pat, organization);

        [TestMethod()]
        public async Task should_get_list_of_processes()
        {
            ProcessListViewModel result = await connector.List();

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_specific_process_detail()
        {
            ProcessViewModel result = await connector.Get(Guid.Parse("27450541-8e31-4150-9947-dc59f998fc01"));

            Assert.IsTrue(result.id != "");
        }
    }
}