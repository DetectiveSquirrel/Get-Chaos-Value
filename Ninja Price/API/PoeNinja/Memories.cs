using System;
using System.Collections.Generic;

namespace Ninja_Price.API.PoeNinja;

public class Memories
{
    public class RootObject
    {
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Uri Icon { get; set; }
        public string BaseType { get; set; }
        public long ItemClass { get; set; }
        public Sparkline Sparkline { get; set; }
        public Sparkline LowConfidenceSparkline { get; set; }
        public List<object> ImplicitModifiers { get; set; }
        public List<ExplicitModifier> ExplicitModifiers { get; set; }
        public string FlavourText { get; set; }
        public long? ChaosValue { get; set; }
        public double? ExaltedValue { get; set; }
        public double? DivineValue { get; set; }
        public long Count { get; set; }
        public string DetailsId { get; set; }
        public List<TradeInfo> TradeInfo { get; set; }
        public long ListingCount { get; set; }
    }

    public class ExplicitModifier
    {
        public string Text { get; set; }
        public bool Optional { get; set; }
    }

    public class Sparkline
    {
        public List<double> Data { get; set; }
        public double? TotalChange { get; set; }
    }

    public class TradeInfo
    {
        public string Mod { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
    }
}