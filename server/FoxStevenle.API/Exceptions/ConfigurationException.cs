namespace FoxStevenle.API.Exceptions;

/// <summary>
/// Exception that represents an error in application configuration
/// </summary>
/// <param name="message">Info message of the error</param>
public class ConfigurationException(string message) : Exception(message);