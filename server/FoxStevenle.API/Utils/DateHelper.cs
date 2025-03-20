using System.Globalization;
using FoxStevenle.API.Constants;

namespace FoxStevenle.API.Utils;

public static class DateOnlyHelper
{
    /// <summary>
    /// Gets a <see cref="DateOnly"/> representation of the current date
    /// </summary>
    /// <returns>Current date as <see cref="DateOnly"/></returns>
    public static DateOnly GetCurrentDateOnly() => DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// Parses a <see cref="DateOnly"/> from the passed string in a <see cref="GeneralConstants.DateOnlyKeyFormat"/>
    /// </summary>
    /// <param name="key"><see cref="string"/> key to parse the date from</param>
    /// <returns>Parsed <see cref="DateOnly"/> or null (if the string is in an incorrect format)</returns>
    public static DateOnly? GetFromStringKey(string key) =>
        DateOnly.TryParseExact(key, GeneralConstants.DateOnlyKeyFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var dateOnly)
            ? dateOnly
            : null;
}