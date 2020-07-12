using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ninja_Price.API.PoeNinja.Classes
{
    public class DeliriumOrb
    {
        public partial class RootObject
        {
            [JsonProperty("lines", NullValueHandling = NullValueHandling.Ignore)]
            public List<Line> Lines { get; set; }

            [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
            public Language Language { get; set; }
        }

        public partial class Language
        {
            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("translations", NullValueHandling = NullValueHandling.Ignore)]
            public Translations Translations { get; set; }
        }

        public partial class Translations
        {
        }

        public partial class Line
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public long? Id { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
            public Uri Icon { get; set; }

            [JsonProperty("mapTier", NullValueHandling = NullValueHandling.Ignore)]
            public long? MapTier { get; set; }

            [JsonProperty("levelRequired", NullValueHandling = NullValueHandling.Ignore)]
            public long? LevelRequired { get; set; }

            [JsonProperty("baseType", NullValueHandling = NullValueHandling.Ignore)]
            public string BaseType { get; set; }

            [JsonProperty("stackSize", NullValueHandling = NullValueHandling.Ignore)]
            public long? StackSize { get; set; }

            [JsonProperty("variant")]
            public object Variant { get; set; }

            [JsonProperty("prophecyText")]
            public object ProphecyText { get; set; }

            [JsonProperty("artFilename")]
            public object ArtFilename { get; set; }

            [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
            public long? Links { get; set; }

            [JsonProperty("itemClass", NullValueHandling = NullValueHandling.Ignore)]
            public long? ItemClass { get; set; }

            [JsonProperty("sparkline", NullValueHandling = NullValueHandling.Ignore)]
            public Sparkline Sparkline { get; set; }

            [JsonProperty("lowConfidenceSparkline", NullValueHandling = NullValueHandling.Ignore)]
            public Sparkline LowConfidenceSparkline { get; set; }

            [JsonProperty("implicitModifiers", NullValueHandling = NullValueHandling.Ignore)]
            public List<object> ImplicitModifiers { get; set; }

            [JsonProperty("explicitModifiers", NullValueHandling = NullValueHandling.Ignore)]
            public List<ExplicitModifier> ExplicitModifiers { get; set; }

            [JsonProperty("flavourText", NullValueHandling = NullValueHandling.Ignore)]
            public string FlavourText { get; set; }

            [JsonProperty("corrupted", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Corrupted { get; set; }

            [JsonProperty("gemLevel", NullValueHandling = NullValueHandling.Ignore)]
            public long? GemLevel { get; set; }

            [JsonProperty("gemQuality", NullValueHandling = NullValueHandling.Ignore)]
            public long? GemQuality { get; set; }

            [JsonProperty("itemType", NullValueHandling = NullValueHandling.Ignore)]
            public ItemType? ItemType { get; set; }

            [JsonProperty("chaosValue", NullValueHandling = NullValueHandling.Ignore)]
            public double? ChaosValue { get; set; }

            [JsonProperty("exaltedValue", NullValueHandling = NullValueHandling.Ignore)]
            public double? ExaltedValue { get; set; }

            [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
            public long? Count { get; set; }

            [JsonProperty("detailsId", NullValueHandling = NullValueHandling.Ignore)]
            public string DetailsId { get; set; }

            [JsonProperty("tradeInfo")]
            public object TradeInfo { get; set; }

            [JsonProperty("mapRegion")]
            public object MapRegion { get; set; }
        }

        public partial class ExplicitModifier
        {
            [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
            public string Text { get; set; }

            [JsonProperty("optional", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Optional { get; set; }
        }

        public partial class Sparkline
        {
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public List<double> Data { get; set; }

            [JsonProperty("totalChange", NullValueHandling = NullValueHandling.Ignore)]
            public double? TotalChange { get; set; }
        }

        public enum ItemType { Unknown };

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                ItemTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class ItemTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(ItemType) || t == typeof(ItemType?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                if (value == "Unknown")
                {
                    return ItemType.Unknown;
                }
                throw new Exception("Cannot unmarshal type ItemType");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (ItemType)untypedValue;
                if (value == ItemType.Unknown)
                {
                    serializer.Serialize(writer, "Unknown");
                    return;
                }
                throw new Exception("Cannot marshal type ItemType");
            }

            public static readonly ItemTypeConverter Singleton = new ItemTypeConverter();
        }
    }
}