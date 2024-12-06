using FinTracker.Api;
using FinTracker.Api.Common.Api;
using FinTracker.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddDocumentation();

builder.AddSecurity();

builder.AddConfiguration();

builder.AddDataContexts();

builder.AddCrossOrigin();

builder.AddServices();

var app = builder.Build();

app.UseCors(ApiConfiguration.CorsPolicyName);

app.UseSecurity();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.MapEndpoints();

app.Run();
