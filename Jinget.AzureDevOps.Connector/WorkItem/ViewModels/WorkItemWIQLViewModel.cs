using System;

namespace Jinget.AzureDevOps.Connector.WorkItem.ViewModels
{
    internal class WorkItemWIQLViewModel
    {

        public string queryType { get; set; }
        public string queryResultType { get; set; }
        public DateTime asOf { get; set; }
        public Column[] columns { get; set; }
        public Workitem[] workItems { get; set; }

        public class Column
        {
            public string referenceName { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Workitem
        {
            public int id { get; set; }
            public string url { get; set; }
        }

    }
}
