using System.Net.Http.Json;

namespace BeverageMcpServer.Model;


public class BeverageService
{

    readonly HttpClient _httpClient = new();
    private List<Beverage>? _beveragesCache = null;
    private DateTime _cacheTime;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10); // adjust as needed

    private async Task<List<Beverage>> FetchBeveragesFromApi()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://gist.githubusercontent.com/medhatelmasry/fab36e3fac4ddafac0f837c920741eae/raw/734f416e93967c02ce36d404916b06da6de5fa77/beverages.json");
            if (response.IsSuccessStatusCode)
            {
                var beveragesFromApi = await response.Content.ReadFromJsonAsync<List<Beverage>>(BeverageContext.Default.ListBeverage);
                return beveragesFromApi ?? [];
            }
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error fetching beverages from API: {ex.Message}");
        }
        return [];
    }

    public async Task<List<Beverage>> GetBeverages()
    {
        if (_beveragesCache == null || DateTime.UtcNow - _cacheTime > _cacheDuration)
        {
            _beveragesCache = await FetchBeveragesFromApi();
            _cacheTime = DateTime.UtcNow;
        }
        return _beveragesCache;
    }


    // Get beverage by BeverageId
    public async Task<Beverage?> GetBeverageById(int beverageId)
    {
        var beverages = await GetBeverages();
        var beverage = beverages.FirstOrDefault(b => b.BeverageId == beverageId);
        Console.WriteLine(beverage == null ? $"No beverage found with ID {beverageId}" : $"Found beverage: {beverage}");
        return beverage;
    }

    // Get beverages by Name (exact match)
    public async Task<List<Beverage>> GetBeveragesByName(string name)
    {
        var beverages = await GetBeverages();
        var filtered = beverages.Where(b => b.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true).ToList();
        Console.WriteLine(filtered.Count == 0 ? $"No beverages found with name: {name}" : $"Found {filtered.Count} beverages with name: {name}");
        return filtered;
    }

    // Get beverages by Type (exact match)
    public async Task<List<Beverage>> GetBeveragesByType(string type)
    {
        var beverages = await GetBeverages();
        var filtered = beverages.Where(b => b.Type?.Equals(type, StringComparison.OrdinalIgnoreCase) == true).ToList();
        Console.WriteLine(filtered.Count == 0 ? $"No beverages found with type: {type}" : $"Found {filtered.Count} beverages with type: {type}");
        return filtered;
    }

    // Get beverages by MainIngredient (exact match)
    public async Task<List<Beverage>> GetBeveragesByMainIngredient(string mainIngredient)
    {
        var beverages = await GetBeverages();
        var filtered = beverages.Where(b => b.MainIngredient?.Equals(mainIngredient, StringComparison.OrdinalIgnoreCase) == true).ToList();
        Console.WriteLine(filtered.Count == 0 ? $"No beverages found with main ingredient: {mainIngredient}" : $"Found {filtered.Count} beverages with main ingredient: {mainIngredient}");
        return filtered;
    }

    // Get beverages by Origin (exact match)
    public async Task<List<Beverage>> GetBeveragesByOrigin(string origin)
    {
        var beverages = await GetBeverages();
        var filtered = beverages.Where(b => b.Origin?.Equals(origin, StringComparison.OrdinalIgnoreCase) == true).ToList();
        Console.WriteLine(filtered.Count == 0 ? $"No beverages found with origin: {origin}" : $"Found {filtered.Count} beverages with origin: {origin}");
        return filtered;
    }

    // Get beverages by CaloriesPerServing (exact match)
    public async Task<List<Beverage>> GetBeveragesByCalories(int caloriesPerServing)
    {
        var beverages = await GetBeverages();
        var filtered = beverages.Where(b => b.CaloriesPerServing == caloriesPerServing).ToList();
        Console.WriteLine(filtered.Count == 0 ? $"No beverages found with calories per serving: {caloriesPerServing}" : $"Found {filtered.Count} beverages with calories per serving: {caloriesPerServing}");
        return filtered;
    }


    public async Task<string> GetBeveragesJson()
    {
        var beverages = await GetBeverages();
        return System.Text.Json.JsonSerializer.Serialize(beverages, BeverageContext.Default.ListBeverage);
    }
}