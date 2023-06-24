namespace Jinget.AzureDevOps.Connector.Teams.ViewModels
{

    public class TeamListViewModel
    {
        public Value[] value { get; set; }
        public int count { get; set; }

        public class Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string description { get; set; }
            public string identityUrl { get; set; }
            public string projectName { get; set; }
            public string projectId { get; set; }
        }
    }
}