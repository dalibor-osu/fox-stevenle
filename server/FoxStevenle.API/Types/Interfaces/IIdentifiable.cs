namespace FoxStevenle.API.Types.Interfaces;

public interface IIdentifiable<TId>
{
    public TId Id { get; set; }

    public const string ColumnName = "id";
}

public interface IIdentifiable : IIdentifiable<int>;