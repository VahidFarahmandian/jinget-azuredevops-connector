using System;
using System.Text.Json.Serialization;

namespace Jinget.AzureDevOps.Connector.WorkItem.ViewModels
{
    public class WorkItemViewModel
    {

        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public _Links2 _links { get; set; }
        public string url { get; set; }


        public class Fields
        {
            [JsonPropertyName("System.AreaPath")]
            public string SystemAreaPath { get; set; }

            [JsonPropertyName("System.TeamProject")]
            public string SystemTeamProject { get; set; }

            [JsonPropertyName("System.IterationPath")]
            public string SystemIterationPath { get; set; }

            [JsonPropertyName("System.WorkItemType")]
            public string SystemWorkItemType { get; set; }

            [JsonPropertyName("System.State")]
            public string SystemState { get; set; }
            
            [JsonPropertyName("System.Reason")]
            public string SystemReason { get; set; }

            [JsonPropertyName("System.CreatedDate")]
            public DateTime SystemCreatedDate { get; set; }

            [JsonPropertyName("System.CreatedBy")]
            public SystemCreatedby SystemCreatedBy { get; set; }

            [JsonPropertyName("System.ChangedDate")]
            public DateTime SystemChangedDate { get; set; }

            [JsonPropertyName("System.ChangedBy")]
            public SystemChangedby SystemChangedBy { get; set; }

            [JsonPropertyName("System.CommentCount")]
            public int SystemCommentCount { get; set; }

            [JsonPropertyName("System.Title")]
            public string SystemTitle { get; set; }

            [JsonPropertyName("Microsoft.VSTS.Scheduling.RemainingWork")]
            public float MicrosoftVSTSSchedulingRemainingWork { get; set; }

            [JsonPropertyName("Microsoft.VSTS.Common.StateChangeDate")]
            public DateTime MicrosoftVSTSCommonStateChangeDate { get; set; }

            [JsonPropertyName("Microsoft.VSTS.Common.Priority")]
            public int MicrosoftVSTSCommonPriority { get; set; }
        }

        public class SystemCreatedby
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public _Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class _Links
        {
            public Avatar avatar { get; set; }
        }

        public class Avatar
        {
            public string href { get; set; }
        }

        public class SystemChangedby
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public _Links1 _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class _Links1
        {
            public Avatar1 avatar { get; set; }
        }

        public class Avatar1
        {
            public string href { get; set; }
        }

        public class _Links2
        {
            public Self self { get; set; }
            public Workitemupdates workItemUpdates { get; set; }
            public Workitemrevisions workItemRevisions { get; set; }
            public Workitemcomments workItemComments { get; set; }
            public Html html { get; set; }
            public Workitemtype workItemType { get; set; }
            public Fields1 fields { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Workitemupdates
        {
            public string href { get; set; }
        }

        public class Workitemrevisions
        {
            public string href { get; set; }
        }

        public class Workitemcomments
        {
            public string href { get; set; }
        }

        public class Html
        {
            public string href { get; set; }
        }

        public class Workitemtype
        {
            public string href { get; set; }
        }

        public class Fields1
        {
            public string href { get; set; }
        }

    }
}
