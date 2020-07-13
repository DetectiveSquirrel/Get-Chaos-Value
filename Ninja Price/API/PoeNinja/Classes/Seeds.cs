using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ninja_Price.API.PoeNinja.Classes
{
    public class Seeds
    {
        public partial class RootObject
        {
            [JsonProperty("lines", NullValueHandling = NullValueHandling.Ignore)]
            public List<Line> Lines { get; set; }
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
            
            [JsonProperty("itemClass", NullValueHandling = NullValueHandling.Ignore)]
            public long? ItemClass { get; set; }

            [JsonProperty("sparkline", NullValueHandling = NullValueHandling.Ignore)]
            public Sparkline Sparkline { get; set; }

            [JsonProperty("lowConfidenceSparkline", NullValueHandling = NullValueHandling.Ignore)]
            public Sparkline LowConfidenceSparkline { get; set; }

            [JsonProperty("chaosValue", NullValueHandling = NullValueHandling.Ignore)]
            public double? ChaosValue { get; set; }

            [JsonProperty("exaltedValue", NullValueHandling = NullValueHandling.Ignore)]
            public double? ExaltedValue { get; set; }

            [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
            public long? Count { get; set; }

            [JsonProperty("detailsId", NullValueHandling = NullValueHandling.Ignore)]
            public string DetailsId { get; set; }
        }

        public partial class Sparkline
        {
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public List<double?> Data { get; set; }

            [JsonProperty("totalChange", NullValueHandling = NullValueHandling.Ignore)]
            public double? TotalChange { get; set; }
        }

    }
}