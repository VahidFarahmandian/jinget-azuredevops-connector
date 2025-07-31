using System.Text.Json.Serialization;

namespace Jinget.AzureDevOps.Connector.WorkItem.ViewModels;

public class WorkItemBatchViewModel
{
    public int Count { get; set; }
    public ValueModel[] Value { get; set; }


    public class ValueModel
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public Fields Fields { get; set; }
        public string Url { get; set; }
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