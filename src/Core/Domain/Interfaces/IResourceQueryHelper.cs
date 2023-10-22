using RockPaperScissors.Core.Domain.ValueObjects.Pagination;

namespace RockPaperScissors.Core.Domain.Interfaces;

public interface IResourceQueryHelper<in TQuery, TResult> where TQuery : Pagination where TResult : class
{
    Task<PaginationContext<TResult>> QueryResourceAsync(TQuery request, CancellationToken cancellationToken);
}