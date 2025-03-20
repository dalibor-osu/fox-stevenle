using FoxStevenle.API.Models.DataTransferObjects;

namespace FoxStevenle.API.Types.OptionalResult;

/// <summary>
/// Wrapper for optional results
/// </summary>
/// <typeparam name="T">Type of the object that is optionally returned</typeparam>
public class Optional<T>
{
    /// <summary>
    /// Value of the object
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if accessed and the current instance is an error and doesn't contain the value</exception>
    public T Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException();
            }

            return _value!;
        }
    }

    /// <summary>
    /// Error that occured
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if accessed and the current instance is not an error and contains the value</exception>
    public OptionalError Error
    {
        get
        {
            if (HasValue)
            {
                throw new InvalidOperationException();
            }

            return _error!;
        }
    }

    /// <summary>
    /// Whether the current instance has a value or not
    /// </summary>
    public bool HasValue => !Equals(_value, default(T));

    private readonly T? _value;
    private readonly OptionalError? _error;

    public Optional(T value)
    {
        _value = value;
        _error = null;
    }

    public Optional(OptionalError error)
    {
        _error = error;
        _value = default;
    }

    /// <summary>
    /// Creates a <see cref="ErrorMessageWrapper"/> containing the error which is returned to the user
    /// </summary>
    /// <returns>Created <see cref="ErrorMessageWrapper"/> wrapper</returns>
    /// <exception cref="InvalidOperationException">Thrown if the current instance is not an error and contains a value</exception>
    public ErrorMessageWrapper GetErrorMessageWrapper() => new()
        { ErrorMessage = _error?.Message ?? throw new InvalidOperationException() };
}