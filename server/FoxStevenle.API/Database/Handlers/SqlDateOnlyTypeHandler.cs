using System.Data;
using Dapper;

namespace FoxStevenle.API.Database.Handlers;

/// <summary>
/// Sql handler of <see cref="DateOnly"/> type
/// </summary>
public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc);
    }

    /// <inheritdoc />
    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }
}