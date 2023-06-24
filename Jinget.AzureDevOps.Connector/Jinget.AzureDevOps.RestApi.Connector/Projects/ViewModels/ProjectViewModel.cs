using System;

namespace Jinget.AzureDevOps.RestApi.Connector.Projects.ViewModels
{
    public class ProjectViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public _Links _links { get; set; }
        public string visibility { get; set; }
        public Defaultteam defaultTeam { get; set; }
        public DateTime lastUpdateTime { get; set; }

        public class _Links
        {
            public Self self { get; set; }
            public Collection collection { get; set; }
            public Web web { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Collection
        {
            public string href { get; set; }
        }

        public class Web
        {
            public string href { get; set; }
        }

        public class Defaultteam
        {
            public string id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }
    }
}
