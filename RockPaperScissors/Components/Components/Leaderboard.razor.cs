using Microsoft.AspNetCore.Components;
using RockPaperScissors.Models;
using RockPaperScissors.Services.Client;
using RockPaperScissors.Services.Server;

namespace RockPaperScissors.Components.Components;

public partial class Leaderboard
{
    [Inject] PersistenceManager PersistenceManager { get; set; }

    private IEnumerable<LeaderboardContext> _leaderboardContexts = Enumerable.Empty<LeaderboardContext>();

    protected override async Task OnInitializedAsync()
    {
        await GetLeaderboardContexts();
        PersistenceManager.OnLeaderboardChange += OnLeaderboardChangeHandler;
        await base.OnInitializedAsync();
    }

    private async void OnLeaderboardChangeHandler()
    {
        await GetLeaderboardContexts();
        await InvokeAsync(StateHasChanged);
    }

    private async Task GetLeaderboardContexts()
    {
        _leaderboardContexts = await PersistenceManager.GetLeaderboardContextAsync();

        if (!_leaderboardContexts.Any())
        {
            var add = new List<LeaderboardContext>
            {
                new()
                {
                    UserName = "Amos",
                    Duration = TimeSpan.FromMinutes(14),
                    Streak = 42,
                    Submitted = DateTimeOffset.UtcNow
                },
                new()
                {
                    UserName = "Jeff",
                    Duration = TimeSpan.FromDays(2),
                    Streak = 25,
                    Submitted = DateTimeOffset.UtcNow.AddDays(-1)
                },
                new()
                {
                    UserName = "Dave",
                    Duration = TimeSpan.FromDays(2),
                    Streak = 525,
                    Submitted = DateTimeOffset.UtcNow.AddDays(-10)
                }
            };

            _leaderboardContexts = add;
        }
    }
}
