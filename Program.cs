using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using BeverageMcpServer.Model;

// Check if running in Docker/HTTP mode or STDIO mode
var useHttp = Environment.GetEnvironmentVariable("MCP_HTTP_MODE") == "true" ||
              args.Contains("--http") ||
              Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

if (useHttp)
{
    // ASP.NET Core Web Application for Docker deployment
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.AddConsole();

    // Add CORS for cross-origin requests
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Add the BeverageService
    builder.Services.AddSingleton<BeverageService>();

    var app = builder.Build();

    // Configure HTTP request pipeline
    app.UseRouting();
    app.UseCors();

    // Health check endpoint
    app.MapGet("/health", () => new { status = "healthy", service = "MCP Beverage Server" });

    // REST API endpoints that mirror the MCP tools
    app.MapGet("/api/beverages", async (BeverageService service) =>
    {
        var beverages = await service.GetBeverages();
        return Results.Ok(beverages);
    });

    app.MapGet("/api/beverages/{id:int}", async (int id, BeverageService service) =>
    {
        var beverage = await service.GetBeverageById(id);
        return beverage != null ? Results.Ok(beverage) : Results.NotFound();
    });

    app.MapGet("/api/beverages/name/{name}", async (string name, BeverageService service) =>
    {
        var beverages = await service.GetBeveragesByName(name);
        return Results.Ok(beverages);
    });

    app.MapGet("/api/beverages/type/{type}", async (string type, BeverageService service) =>
    {
        var beverages = await service.GetBeveragesByType(type);
        return Results.Ok(beverages);
    });

    app.MapGet("/api/beverages/ingredient/{ingredient}", async (string ingredient, BeverageService service) =>
    {
        var beverages = await service.GetBeveragesByMainIngredient(ingredient);
        return Results.Ok(beverages);
    });

    app.MapGet("/api/beverages/origin/{origin}", async (string origin, BeverageService service) =>
    {
        var beverages = await service.GetBeveragesByOrigin(origin);
        return Results.Ok(beverages);
    });

    app.MapGet("/api/beverages/calories/{calories:int}", async (int calories, BeverageService service) =>
    {
        var beverages = await service.GetBeveragesByCalories(calories);
        return Results.Ok(beverages);
    });

    app.MapGet("/api/beverages/count", async (BeverageService service) =>
    {
        var beverages = await service.GetBeverages();
        return Results.Ok(new { count = beverages.Count });
    });

    // API documentation endpoint
    app.MapGet("/api", () => new
    {
        message = "MCP Beverage Server REST API",
        endpoints = new[] {
            "GET /health - Health check",
            "GET /api/beverages - Get all beverages",
            "GET /api/beverages/{id} - Get beverage by ID",
            "GET /api/beverages/name/{name} - Get beverages by name",
            "GET /api/beverages/type/{type} - Get beverages by type",
            "GET /api/beverages/ingredient/{ingredient} - Get beverages by main ingredient",
            "GET /api/beverages/origin/{origin} - Get beverages by origin",
            "GET /api/beverages/calories/{calories} - Get beverages by calories",
            "GET /api/beverages/count - Get beverage count"
        }
    });

    Console.WriteLine("MCP Beverage Server running in HTTP mode");
    Console.WriteLine($"Health check: http://localhost:8080/health");
    Console.WriteLine($"API docs: http://localhost:8080/api");
    Console.WriteLine($"All beverages: http://localhost:8080/api/beverages");

    await app.RunAsync();
}
else
{
    // Original STDIO mode for local MCP development
    var builder = Host.CreateApplicationBuilder(args);
    builder.Logging.AddConsole(consoleLogOptions =>
    {
        consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
    });

    builder.Services
        .AddMcpServer()
        .WithStdioServerTransport()
        .WithToolsFromAssembly();

    await builder.Build().RunAsync();
}

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";
}