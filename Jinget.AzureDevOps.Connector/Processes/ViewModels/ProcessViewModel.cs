namespace Jinget.AzureDevOps.Connector.Processes.ViewModels;


public class ProcessViewModel
{
    public string id { get; set; }
    public string description { get; set; }
    public bool isDefault { get; set; }
    public _Links _links { get; set; }
    public string type { get; set; }
    public string url { get; set; }
    public string name { get; set; }


    public class _Links
    {
        public Self self { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }
}
