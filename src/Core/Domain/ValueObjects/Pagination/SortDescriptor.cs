using RockPaperScissors.Core.Domain.Enums;

namespace RockPaperScissors.Core.Domain.ValueObjects.Pagination;

public class SortDescriptor
{
    public string Property { get; set; }
    public SortDirection SortOrder { get; set; }
}
