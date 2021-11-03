using System.Collections.Generic;
using Ninja_Price.Enums;

namespace Ninja_Price.Main
{
    public partial class Main
    {
        public class ReleventPriceData // store data that was got from checking the item against the poe.ninja data
        {
            public double ChaosValue { get; set; }
            public double ExaltedPrice { get; set; }
            public double ChangeInLast7Days { get; set; }
            public ItemTypes ItemType { get; set; }
            public List<double> ItemBasePrices { get; set; } = new List<double>();

            public override string ToString()
            {
                return $"ChaosValue: {ChaosValue}, ChangeInLast7Days: {ChangeInLast7Days}, ItemType: {ItemType}";
            }
        }
    }
}