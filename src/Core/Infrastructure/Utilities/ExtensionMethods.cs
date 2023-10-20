using System.Linq.Expressions;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects.Pagination;

namespace RockPaperScissors.Core.Infrastructure.Utilities;

public static class ExtensionMethods
{
    public static IQueryable<TDomain> ApplySort<TDomain>(this IQueryable<TDomain> query, SortDescriptor sort,
        Expression<Func<TDomain, object>> property)
    {
        return sort.SortOrder is SortDirection.Ascending
            ? query.OrderBy(property)
            : query.OrderByDescending(property);
    }
}
