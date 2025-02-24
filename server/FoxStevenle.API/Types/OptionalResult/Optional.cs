namespace FoxStevenle.API.Types.OptionalResult;

public class Optional<T>
{
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
}