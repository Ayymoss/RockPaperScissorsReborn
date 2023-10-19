using Radzen;
using RockPaperScissors.Components;
using RockPaperScissors.Services.Client;
using RockPaperScissors.Services.Server;

// TODO Allow for restore-able Session ID

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(8123); });
#else
builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(8124, configure => configure.UseHttps()); });
#endif

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<ClientPageService>();

builder.Services.AddSingleton<RpsClientHub>();
builder.Services.AddSingleton<PersistenceManager>();


builder.Services.AddSignalR();

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
