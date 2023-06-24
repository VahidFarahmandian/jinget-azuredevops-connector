namespace Jinget.AzureDevOps.RestApi.Connector.Projects.ViewModels
{
    public class NewProjectModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public Capabilities capabilities { get; set; }


        public class Capabilities
        {
            public Versioncontrol versioncontrol { get; set; }
            public Processtemplate processTemplate { get; set; }
        }

        public class Versioncontrol
        {
            public string sourceControlType { get; set; }
        }

        public class Processtemplate
        {
            public string templateTypeId { get; set; }
        }

    }
}
