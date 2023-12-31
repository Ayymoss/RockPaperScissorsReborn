﻿namespace RockPaperScissors.Core.Domain.ValueObjects.Pagination;

public class Pagination
{
    public int Top { get; set; }
    public int Skip { get; set; }
    public string? SearchString { get; set; }
    public IEnumerable<SortDescriptor> Sorts { get; set; }
}
