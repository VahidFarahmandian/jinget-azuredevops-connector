using Jinget.AzureDevOps.Connector.WorkItem;
using Jinget.AzureDevOps.Connector.WorkItem.ViewModels;

namespace Jinget.AzureDevOps.ConnectorTests.WorkItemsTests;

[TestClass()]
public class WorkItemConnectorTests : BaseTests
{
    WorkItemConnector connector;

    [TestInitialize]
    public void TestInitialize()
        => connector = new WorkItemConnector(ServiceProvider, pat, organization, project: projectName);

    [TestMethod()]
    public async Task should_get_list_of_workitems()
    {
        GetWorkItemBatchModel request = new()
        {
            fields =
            [
                "System.Id",
                "System.Title",
                "System.WorkItemType",
                "Microsoft.VSTS.Scheduling.RemainingWork"
            ],
            ids = ["115", "116", "117", "118"]
        };
        var result = await connector.ListBatchAsync<WorkItemBatchViewModel>(request);
        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.Count);
    }

    [TestMethod()]
    public async Task should_get_list_of_workitems_using_wiql()
    {
        string query = "SELECT System.Id, System.Title, System.WorkItemType, Microsoft.VSTS.Scheduling.RemainingWork FROM WorkItems";
        var result = await connector.ListWIQLAsync<WorkItemBatchViewModel>(query);

        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.Count);
    }

    [TestMethod()]
    public async Task should_create_new_workitem()
    {
        List<NewWorkItemModel> properties =
        [
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
                value=projectName
            }
        ];
        var result = await connector.CreateAsync("$Task", properties);

        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.id);
    }
}