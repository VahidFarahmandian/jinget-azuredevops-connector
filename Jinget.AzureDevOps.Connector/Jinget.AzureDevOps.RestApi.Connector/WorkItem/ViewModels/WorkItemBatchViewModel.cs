using System.Text.Json.Serialization;

namespace Jinget.AzureDevOps.RestApi.Connector.WorkItem.ViewModels
{
    public class WorkItemBatchViewModel
    {
        public int count { get; set; }
        public Value[] value { get; set; }


        public class Value
        {
            public int id { get; set; }
            public int rev { get; set; }
            public Fields fields { get; set; }
            public string url { get; set; }
        }

        public class Fields
        {
            [JsonPropertyName("System.Id")]
            public int SystemId { get; set; }

            [JsonPropertyName("System.WorkItemType")]
            public string SystemWorkItemType { get; set; }

            [JsonPropertyName("System.Title")]
            public string SystemTitle { get; set; }


            [JsonPropertyName("Microsoft.VSTS.Scheduling.RemainingWork")]
            public double? MicrosoftVSTSSchedulingRemainingWork { get; set; }
        }
    }
}