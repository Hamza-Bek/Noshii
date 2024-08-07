using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Blazored.LocalStorage;
using NetcodeHub.Packages.Components.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetcodeHub.Packages.Extensions.LocalStorage;
using WebUI;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddNetcodeHubLocalStorageService();
builder.Services.AddScoped<Application.Extensions.LocalStorageService>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});


builder.Services.AddScoped<NetcodeHubToast>();

builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<CustomHttpHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<PlateService>();

builder.Services.AddScoped<IPlateService, PlateService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<ILocationService , LocationService>();

builder.Services.AddHttpClient("WebUIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7173/");
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7173/");
});

builder.Services.AddHttpClient<IPlateService, PlateService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7173/");
});

builder.Services.AddHttpClient<IClientService, ClientService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7173/");
});

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7173/");
});

builder.Services.AddHttpClient<IFilesService, FilesService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7173/");
});

builder.Services.AddHttpClient<ILocationService, LocationService>(client =>
{
	client.BaseAddress = new Uri("https://localhost:7173/");
});
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
