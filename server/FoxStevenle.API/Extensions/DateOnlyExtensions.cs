using FoxStevenle.API.Constants;

namespace FoxStevenle.API.Extensions;

public static class DateOnlyExtensions
{
    /// <summary>
    /// Creates a date key with the <see cref="GeneralConstants.DateOnlyKeyFormat"/>
    /// </summary>
    /// <param name="dateOnly">Date to get the key for</param>
    /// <returns>Created date key</returns>
    public static string GetDateKey(this DateOnly dateOnly) => dateOnly.ToString(GeneralConstants.DateOnlyKeyFormat);
}