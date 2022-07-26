using System;

namespace Ninja_Price.Main;

public static class Extensions
{
    public static string FormatNumber(this double number, int significantDigits, double maxInvertValue = 0)
    {
        if (double.IsNaN(number))
        {
            return "NaN";
        }

        if (number < maxInvertValue)
        {
            return $"1/{Math.Round((decimal)(1 / number), 1):#.#}";
        }

        return Math.Round((decimal)number, significantDigits).ToString("#,##0.##");
    }
}