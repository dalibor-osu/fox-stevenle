using FoxStevenle.API.Constants;

namespace FoxStevenle.API.Extensions;

public static class DateOnlyExtensions
{
    public static string GetDateKey(this DateOnly dateOnly) => dateOnly.ToString(GeneralConstants.DateOnlyKeyFormat);
}