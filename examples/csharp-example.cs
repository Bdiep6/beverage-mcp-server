// C# Example - Consuming Beverage MCP Server
using System.Text.Json;

public class Beverage
{
    public int BeverageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string MainIngredient { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public int CaloriesPerServing { get; set; }
}

public class BeverageClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public BeverageClient(string baseUrl = "http://localhost:8080")
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl;
    }

    public async Task<List<Beverage>> GetAllBeveragesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/beverages");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        
        return JsonSerializer.Deserialize<List<Beverage>>(json, options) ?? new List<Beverage>();
    }

    public async Task<List<Beverage>> GetBeveragesByTypeAsync(string type)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/beverages/type/{Uri.EscapeDataString(type)}");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        
        return JsonSerializer.Deserialize<List<Beverage>>(json, options) ?? new List<Beverage>();
    }
}

// Usage Example
public class Program
{
    public static async Task Main(string[] args)
    {
        var client = new BeverageClient("http://localhost:8080");
        
        // Get all beverages
        var allBeverages = await client.GetAllBeveragesAsync();
        Console.WriteLine($"Found {allBeverages.Count} beverages");
        
        // Get tea beverages
        var teas = await client.GetBeveragesByTypeAsync("Tea");
        Console.WriteLine($"Found {teas.Count} tea beverages:");
        foreach (var tea in teas)
        {
            Console.WriteLine($"- {tea.Name} from {tea.Origin} ({tea.CaloriesPerServing} calories)");
        }
    }
}