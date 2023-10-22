using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects.Pagination;
using SortDescriptor = RockPaperScissors.Core.Domain.ValueObjects.Pagination.SortDescriptor;

namespace RockPaperScissors.WebCore.Server.Components.Components;

public partial class Leaderboard
{
    // TODO: Reload this if a new player is on the leaderboard.
    [Inject] private IMediator Mediator { get; set; }

    private RadzenDataGrid<Core.Business.DTOs.Leaderboard> _dataGrid;
    private IEnumerable<Core.Business.DTOs.Leaderboard> _playerTable;
    private bool _isLoading = true;
    private int _count;
    private string? _searchString;

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;
        var paginationQuery = new GetLeaderboardCommand
        {
            Sorts = args.Sorts.Select(x => new SortDescriptor
            {
                Property = x.Property,
                SortOrder = x.SortOrder == SortOrder.Ascending
                    ? SortDirection.Ascending
                    : SortDirection.Descending
            }),
            SearchString = _searchString,
            Top = args.Top ?? 20,
            Skip = args.Skip ?? 0
        };

        var context = await Mediator.Send(paginationQuery);
        _playerTable = context.Data;
        _count = context.Count;
        _isLoading = false;
    }
}
