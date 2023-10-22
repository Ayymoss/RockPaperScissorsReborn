using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using Radzen;
using RockPaperScissors.Core.Business.DTOs;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Business.Utilities;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.Context;
using RockPaperScissors.Core.Infrastructure.Repositories;
using RockPaperScissors.Core.Infrastructure.Repositories.Pagination;
using RockPaperScissors.Core.Infrastructure.SignalR;
using RockPaperScissors.WebCore.Server.Components;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(8123); });
#else
builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(8124, configure => configure.UseHttps()); });
#endif

var configuration = SetupConfiguration.ReadConfiguration();


configuration.DatabaseName = "RPS-Dev1";

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    options.UseNpgsql($"{configuration.ConnectionString};Database={configuration.DatabaseName}");
});

builder.Services.AddSingleton(configuration);

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IResourceQueryHelper<GetLeaderboardCommand, Leaderboard>, LeaderboardPaginationQueryHelper>();

builder.Services.AddScoped<RpsClientHub>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(GetLeaderboardCommand).Assembly); });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHub<RpsServerHub>("/SignalR/RpsServerHub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
