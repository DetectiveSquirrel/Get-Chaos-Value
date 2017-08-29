using System.Collections.Generic;

namespace GetValue.poe_ninja_api
{
    public class Currency
    {
        public class Pay
        {
            public int id { get; set; }
            public int leagueId { get; set; }
            public int payCurrencyId { get; set; }
            public int getCurrencyId { get; set; }
            public string sampleTimeUtc { get; set; }
            public int count { get; set; }
            public double value { get; set; }
            public int dataPointCount { get; set; }
        }

        public class Receive
        {
            public int id { get; set; }
            public int leagueId { get; set; }
            public int payCurrencyId { get; set; }
            public int getCurrencyId { get; set; }
            public string sampleTimeUtc { get; set; }
            public int count { get; set; }
            public double value { get; set; }
            public int dataPointCount { get; set; }
        }

        public class PaySparkLine
        {
            public List<object> data { get; set; }
            public double totalChange { get; set; }
        }

        public class ReceiveSparkLine
        {
            public List<object> data { get; set; }
            public double totalChange { get; set; }
        }

        public class Line
        {
            public string currencyTypeName { get; set; }
            public Pay pay { get; set; }
            public Receive receive { get; set; }
            public PaySparkLine paySparkLine { get; set; }
            public ReceiveSparkLine receiveSparkLine { get; set; }
            public double chaosEquivalent { get; set; }
        }

        public class CurrencyDetail
        {
            public int id { get; set; }
            public string name { get; set; }
            public int poeTradeId { get; set; }
            public List<string> shorthands { get; set; }
            public string icon { get; set; }
            public int type { get; set; }
        }

        public class RootObject
        {
            public List<Line> lines { get; set; }
            public List<CurrencyDetail> currencyDetails { get; set; }
        }
    }
}
