namespace RockPaperScissors.Services.Client;

public class ClientPageService
{
    public event Action? OnNameProvided;
    public bool IsNameProvided { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string SessionId { get; private set; } = string.Empty;
    public int Streak { get; set; }

    public void NameHasChanged(string userName)
    {
        UserName = userName;
        IsNameProvided = true;
        OnNameProvided?.Invoke();
    }

    public void SetSessionId(string sessionId)
    {
        SessionId = sessionId;
    }
}
