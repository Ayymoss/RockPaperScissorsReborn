using Microsoft.AspNetCore.Components;
using RockPaperScissors.Services.Client;

namespace RockPaperScissors.Components.Components;

public partial class Game
{
    [Inject] ClientPageService ClientPageService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ClientPageService.OnNameProvided += () => InvokeAsync(StateHasChanged);
        await base.OnInitializedAsync();
    }
}
