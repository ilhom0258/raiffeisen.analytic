using System.Globalization;

namespace Raiffeisen.Analytic.Bot;

public static class Utils
{
    private static readonly Dictionary<string, CultureInfo> IsoCurrenciesToACultureMap =
        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new {c, new RegionInfo(c.Name).ISOCurrencySymbol})
            .GroupBy(x => x.ISOCurrencySymbol)
            .ToDictionary(g => g.Key, g => g.First().c, StringComparer.OrdinalIgnoreCase);

    public static string FormatCurrency(this decimal amount, string currencyCode) {
    {
        return IsoCurrenciesToACultureMap.TryGetValue(currencyCode, out var culture) 
            ? string.Format(culture, "{0:C2}", amount) : amount.ToString("C2");
    }}
}