using System.Text.Json.Serialization;
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

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseCors(ApiConfiguration.CorsPolicyName);

app.UseSecurity();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.MapEndpoints();

await app.RunAsync();
