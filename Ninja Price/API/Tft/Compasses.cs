using System.Collections.Generic;

namespace Ninja_Price.API.Tft;

public class Compasses
{
    public class PriceData
    {
        public long Timestamp { get; set; }
        public List<Datum> Data { get; set; }

        public class Datum
        {
            public string Name { get; set; }
            public float Divine { get; set; }
            public float Chaos { get; set; }
            public bool LowConfidence { get; set; }
            public int Ratio { get; set; }
        }
    }
}