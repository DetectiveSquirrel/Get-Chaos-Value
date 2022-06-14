using System;

namespace Ninja_Price.Main;

public static class Extensions
{
    public static string FormatNumber(this double number, int significantDigits)
    {
        if (double.IsNaN(number))
        {
            return "NaN";
        }

        return Math.Round((decimal)number, significantDigits).ToString("#,##0.##");
    }
}
