namespace Jinget.AzureDevOps.Connector.WorkItem.ViewModels;


public class GetWorkItemBatchModel
{
    public string asOf { get; set; }
    public string[] fields { get; set; }
    public string[] ids { get; set; }
}
