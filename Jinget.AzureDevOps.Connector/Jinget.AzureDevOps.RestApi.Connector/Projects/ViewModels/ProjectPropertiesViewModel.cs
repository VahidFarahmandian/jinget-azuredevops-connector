namespace Jinget.AzureDevOps.Connector.Projects.ViewModels
{

    public class ProjectPropertiesViewModel
    {
        public int count { get; set; }
        public Value[] value { get; set; }

        public class Value
        {
            public string name { get; set; }
            public object value { get; set; }
        }
    }
}
