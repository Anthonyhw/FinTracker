using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinTracker.App;
using MudBlazor.Services;
using FinTracker.App.Security;
using Microsoft.AspNetCore.Components.Authorization;
using FinTracker.Core.Handlers;
using FinTracker.App.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
Configuration.StripePublicKey = builder.Configuration.GetValue<string>("StripePublicKey") ?? string.Empty;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x => (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddHttpClient(name:Configuration.HttpClientName, configureClient: opt =>
{
    opt.BaseAddress = new Uri(Configuration.BackendUrl);
}).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddTransient<IAccountHandler, AccountHandler>()
                .AddTransient<ICategoryHandler, CategoryHandler>()
                .AddTransient<ITransactionHandler, TransactionHandler>()
                .AddTransient<IReportHandler, ReportHandler>()
                .AddTransient<IVoucherHandler, VoucherHandler>()
                .AddTransient<IProductHandler, ProductHandler>()
                .AddTransient<IOrderHandler, OrderHandler>()
                .AddTransient<IStripeHandler, StripeHandler>();

builder.Services.AddLocalization();

await builder.Build().RunAsync();