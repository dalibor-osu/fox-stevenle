using System.Globalization;
using FoxStevenle.API.Constants;

namespace FoxStevenle.API.Utils;

public static class DateOnlyHelper
{
    public static DateOnly GetCurrentDateOnly() => DateOnly.FromDateTime(DateTime.UtcNow);

    public static DateOnly? GetFromStringKey(string key) =>
        DateOnly.TryParseExact(key, GeneralConstants.DateOnlyKeyFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var dateOnly)
            ? dateOnly
            : null;
}