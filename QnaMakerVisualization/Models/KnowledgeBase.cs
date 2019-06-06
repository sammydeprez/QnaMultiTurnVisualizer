﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QnaMakerVisualization.Models;
//
//    var knowledgeBase = KnowledgeBase.FromJson(jsonString);

namespace QnaMakerVisualization.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class KnowledgeBase
    {
        [JsonProperty("qnaDocuments")]
        public List<Qna> QnaDocuments { get; set; }
    }

    public partial class Prompt
    {
        [JsonProperty("displayOrder")]
        public long DisplayOrder { get; set; }

        [JsonProperty("qnaId")]
        public long QnaId { get; set; }

        [JsonProperty("qna")]
        public Qna Qna { get; set; }

        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }

    public partial class Context
    {
        [JsonProperty("isContextOnly")]
        public bool IsContextOnly { get; set; }

        [JsonProperty("prompts")]
        public List<Prompt> Prompts { get; set; }
    }

    public partial class Qna
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("questions")]
        public List<string> Questions { get; set; }

        [JsonProperty("metadata")]
        public List<Metadatum> Metadata { get; set; }

        [JsonProperty("alternateQuestions")]
        public string AlternateQuestions { get; set; }

        [JsonProperty("alternateQuestionClusters")]
        public List<object> AlternateQuestionClusters { get; set; }

        [JsonProperty("changeStatus")]
        public string ChangeStatus { get; set; }

        [JsonProperty("kbId")]
        public Guid KbId { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; }
    }

    public partial class Metadatum
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum ChangeStatus { Update };

    public enum Source { Editorial };

    public partial class KnowledgeBase
    {
        public static KnowledgeBase FromJson(string json) => JsonConvert.DeserializeObject<KnowledgeBase>(json, QnaMakerVisualization.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this KnowledgeBase self) => JsonConvert.SerializeObject(self, QnaMakerVisualization.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ChangeStatusConverter.Singleton,
                SourceConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ChangeStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ChangeStatus) || t == typeof(ChangeStatus?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Update")
            {
                return ChangeStatus.Update;
            }
            throw new Exception("Cannot unmarshal type ChangeStatus");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ChangeStatus)untypedValue;
            if (value == ChangeStatus.Update)
            {
                serializer.Serialize(writer, "Update");
                return;
            }
            throw new Exception("Cannot marshal type ChangeStatus");
        }

        public static readonly ChangeStatusConverter Singleton = new ChangeStatusConverter();
    }

    internal class SourceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Source) || t == typeof(Source?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Editorial")
            {
                return Source.Editorial;
            }
            throw new Exception("Cannot unmarshal type Source");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Source)untypedValue;
            if (value == Source.Editorial)
            {
                serializer.Serialize(writer, "Editorial");
                return;
            }
            throw new Exception("Cannot marshal type Source");
        }

        public static readonly SourceConverter Singleton = new SourceConverter();
    }
}