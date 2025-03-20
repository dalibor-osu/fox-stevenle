namespace FoxStevenle.API.Types.Interfaces;

/// <summary>
/// Represents an identifiable object
/// </summary>
/// <typeparam name="TId">Type to use for the ID</typeparam>
public interface IIdentifiable<TId>
{
    /// <summary>
    /// Identifier of the object
    /// </summary>
    public TId Id { get; set; }

    public const string ColumnName = "id";
}

/// <summary>
/// Default identifiable with <see cref="int"/> ID
/// </summary>
public interface IIdentifiable : IIdentifiable<int>;