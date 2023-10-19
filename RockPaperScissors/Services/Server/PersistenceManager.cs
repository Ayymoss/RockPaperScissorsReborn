using System.Reflection;
using System.Text.Json;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services.Server;

public class PersistenceManager
{
    public IEnumerable<LeaderboardContext>? LeaderboardContexts { get; set; }
    public event Action? OnLeaderboardChange;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() {WriteIndented = true};
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public async Task<IEnumerable<LeaderboardContext>> GetLeaderboardContextAsync()
    {
        if (LeaderboardContexts is null) await LoadLeaderboardContextAsync();
        return LeaderboardContexts ?? Enumerable.Empty<LeaderboardContext>();
    }

    public async Task HandleNewEntryAsync(LeaderboardContext context)
    {
        try
        {
            await _semaphoreSlim.WaitAsync();

            if (LeaderboardContexts is null) await LoadLeaderboardContextAsync();
            LeaderboardContexts ??= Enumerable.Empty<LeaderboardContext>();

            var leaderboardContexts = LeaderboardContexts as LeaderboardContext[] ?? LeaderboardContexts.ToArray();

            var existingContext = leaderboardContexts.FirstOrDefault(x => x.UserName == context.UserName);
            if (existingContext is null)
            {
                LeaderboardContexts = leaderboardContexts.Append(context);
            }
            else
            {
                var index = leaderboardContexts.ToList().IndexOf(existingContext);
                leaderboardContexts[index] = context;
                LeaderboardContexts = leaderboardContexts;
            }

            OnLeaderboardChange?.Invoke();

            // TODO: Limit leaderboard size. Maybe sort and filter by streak then duration.

            await WriteLeaderboardContextAsync();
        }
        finally
        {
            if (_semaphoreSlim.CurrentCount is 0) _semaphoreSlim.Release();
        }
    }

    private async Task LoadLeaderboardContextAsync()
    {
        var filePath = GetFilePath();
        if (!File.Exists(filePath))
        {
            LeaderboardContexts = Enumerable.Empty<LeaderboardContext>();
            return;
        }

        var rawJson = await File.ReadAllTextAsync(filePath);
        LeaderboardContexts = JsonSerializer.Deserialize<List<LeaderboardContext>>(rawJson);
    }

    private async Task WriteLeaderboardContextAsync()
    {
        var filePath = GetFilePath();
        var rawJson = JsonSerializer.Serialize(LeaderboardContexts, _jsonSerializerOptions);
        await File.WriteAllTextAsync(filePath, rawJson);
    }

    private static string GetFilePath()
    {
        var workingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Data");
        return Path.Combine(workingDirectory, "LeaderboardContext.json");
    }
}
