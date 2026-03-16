using TucBookingSystem.Client.Components;
using TucBookingSystem.Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiUrl = builder.Configuration["ApiUrl"];

builder.Services.AddHttpClient<RoomService>(client =>
{
    client.BaseAddress = new Uri(apiUrl!);
});

builder.Services.AddHttpClient<BookingService>(client =>
{
    client.BaseAddress = new Uri(apiUrl!);
});

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(apiUrl!);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();