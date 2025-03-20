namespace FoxStevenle.API.Models.DataTransferObjects;

/// <summary>
/// Wrapper for error message that gets returned to the client
/// </summary>
public class ErrorMessageWrapper
{
    public string ErrorMessage { get; set; } = string.Empty;
}