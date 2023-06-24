namespace Jinget.AzureDevOps.Connector.Projects.ViewModels
{

    public class GetWorkItemBatchModel
    {
        public string asOf { get; set; }
        public string[] fields { get; set; }
        public string[] ids { get; set; }
    }
}
