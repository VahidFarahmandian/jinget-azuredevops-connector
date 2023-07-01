using Jinget.AzureDevOps.Connector.Board.Tests;
using Jinget.AzureDevOps.Connector.Teams;
using Jinget.AzureDevOps.Connector.Teams.ViewModels;

namespace Jinget.AzureDevOps.ConnectorTests.TeamsTests
{
    [TestClass()]
    public class TeamConnectorTests : _BaseTests
    {
        TeamConnector connector;

        [TestInitialize]
        public void TestInitialize() => connector = new TeamConnector(pat, organization,);


        [TestMethod()]
        public async Task should_get_raw_response()
        {
            connector.Events.ResponseReceived += (object? sender, string e) =>
            {
                Assert.IsTrue(e != "");
            };
            TeamListViewModel result = await connector.GetAllTeamsAsync();
        }

        [TestMethod()]
        public async Task should_get_list_of_all_teams()
        {
            TeamListViewModel result = await connector.GetAllTeamsAsync();

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_list_of_teams_in_specific_project()
        {
            TeamListViewModel result = await connector.GetTeamsAsync(Guid.Parse("5ec32e71-d47d-4908-8107-069a952c9550"));

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_specific_project_team()
        {
            TeamViewModel result = await connector.GetAsync(Guid.Parse("5ec32e71-d47d-4908-8107-069a952c9550"), "PMOSample Team");

            Assert.IsTrue(result.id != "");
        }
    }
}