using System.Text.Json.Serialization;

namespace Jinget.AzureDevOps.Connector.Projects.ViewModels
{

    public class ODataResultViewModel
    {
        [JsonPropertyName("@odata.context")]
        public string odatacontext { get; set; }
        public object[] value { get; set; }
    }
}
