﻿namespace RockPaperScissors.Core.Domain.ValueObjects.Pagination;

public class PaginationContext<TContext> where TContext : class
{
    public IEnumerable<TContext> Data { get; set; }
    public int Count { get; set; }
}
