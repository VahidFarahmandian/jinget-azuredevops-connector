using System.Text.Json.Serialization;

namespace Jinget.AzureDevOps.ConnectorTests.ODataTests.CustomTypes;

public class MyCustomType
{
    [JsonPropertyName("@odata.context")]
    public string odatacontext { get; set; }
    public Value[] value { get; set; }

    public class Value
    {
        public int WorkItemId { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
    }
}
