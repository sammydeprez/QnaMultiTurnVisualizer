using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace QnaMakerVisualization.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class KnowledgeBaseMetaDataList
    {
        [JsonProperty("knowledgebases")]
        public List<KnowledgebaseMetaData> KnowledgeBases { get; set; }
    }

    public partial class KnowledgebaseMetaData
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("hostName")]
        public Uri HostName { get; set; }

        [JsonProperty("lastAccessedTimestamp")]
        public DateTimeOffset LastAccessedTimestamp { get; set; }

        [JsonProperty("lastChangedTimestamp")]
        public DateTimeOffset LastChangedTimestamp { get; set; }

        [JsonProperty("lastPublishedTimestamp")]
        public DateTimeOffset LastPublishedTimestamp { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("urls")]
        public List<object> Urls { get; set; }

        [JsonProperty("sources")]
        public List<string> Sources { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("enableHierarchicalExtraction")]
        public bool EnableHierarchicalExtraction { get; set; }

        [JsonProperty("createdTimestamp")]
        public DateTimeOffset CreatedTimestamp { get; set; }
    }

    public partial class KnowledgeBaseMetaDataList
    {
        public static KnowledgeBaseMetaDataList FromJson(string json) => JsonConvert.DeserializeObject<KnowledgeBaseMetaDataList>(json, QnaMakerVisualization.Models.Converter.Settings);
    }
}

