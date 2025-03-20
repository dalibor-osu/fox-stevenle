namespace FoxStevenle.API.Types.OptionalResult;

/// <summary>
/// Wrapper with info about an error that occured
/// </summary>
public class OptionalError
{
    /// <summary>
    /// Message of the error
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Type of the error
    /// </summary>
    public OptionalErrorType Type { get; set; } = OptionalErrorType.Unknown;
}