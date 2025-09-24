using System.Data;

namespace ProductService.Application.Core.Abstractions.Data;

/// <summary>
/// Represents the SQL connection factory interface.
/// </summary>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// Creates a new <see cref="IDbConnection"/> instance.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The new <see cref="IDbConnection"/> instance.</returns>
    Task<IDbConnection> CreateSqlConnectionAsync(CancellationToken cancellationToken = default);
}
