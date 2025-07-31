namespace Jinget.AzureDevOps.Connector.Processes.ViewModels;

public class ProcessListViewModel
{
    public int count { get; set; }
    public Value[] value { get; set; }


    public class Value
    {
        public string id { get; set; }
        public string description { get; set; }
        public bool isDefault { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string name { get; set; }
    }
}