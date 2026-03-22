using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Routing;
using TucBookingSystem.Client.Components;
using TucBookingSystem.Client.Services;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var apiUrl = builder.Configuration["ApiUrl"];

        // -----------------------------------------
        // HTTP Clients
        // -----------------------------------------
        builder.Services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri(apiUrl!);
        });

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

        // -----------------------------------------
        // Registrera dina services med interfaces
        // -----------------------------------------
        builder.Services.AddScoped<RoomService>();
        builder.Services.AddScoped<BookingService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IRoomService>(sp => sp.GetRequiredService<RoomService>());
        builder.Services.AddScoped<IBookingService>(sp => sp.GetRequiredService<BookingService>());
        builder.Services.AddScoped<IUserService>(sp => sp.GetRequiredService<UserService>());
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<ProtectedSessionStorage>();
        builder.Services.AddScoped<UserStateService>();
        builder.Services.AddScoped<IUserStateService>(sp => sp.GetRequiredService<UserStateService>());
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAntiforgery();
        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        var endpointSource = app.Services.GetRequiredService<EndpointDataSource>();

        var duplicates = endpointSource.Endpoints
            .OfType<RouteEndpoint>()
            .Where(e => e.RoutePattern.RawText == "/reset-password")
            .ToList();

        Console.WriteLine("=== RESET PASSWORD ENDPOINTS ===");
        foreach (var ep in duplicates)
        {
            Console.WriteLine($"Route: {ep.RoutePattern.RawText}");
            Console.WriteLine($"DisplayName: {ep.DisplayName}");
            Console.WriteLine("-----");
        }

        app.MapGet("/debug/routes", (EndpointDataSource endpointSource) =>
        {
            var routes = endpointSource.Endpoints
                .OfType<RouteEndpoint>()
                .Where(e => e.RoutePattern.RawText == "/reset-password")
                .Select(e =>
                {
                    var componentMetadata = e.Metadata.OfType<ComponentTypeMetadata>().FirstOrDefault();

                    return new
                    {
                        Route = e.RoutePattern.RawText,
                        e.DisplayName,
                        ComponentType = componentMetadata?.Type.FullName,
                        Assembly = componentMetadata?.Type.Assembly.FullName
                    };
                })
                .ToList();

            return Results.Json(routes);
        });

        app.Run();
    }
}