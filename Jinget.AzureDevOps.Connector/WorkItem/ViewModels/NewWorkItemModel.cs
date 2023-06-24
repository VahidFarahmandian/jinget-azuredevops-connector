namespace Jinget.AzureDevOps.Connector.WorkItem.ViewModels
{
    public class NewWorkItemModel
    {

        public string op { get; } = "add";
        public string path { get; set; }
        public string value { get; set; }
    }
}
