using System;

namespace BeverageMcpServer.Model;

public class Beverage
{
    public int BeverageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string MainIngredient { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public int CaloriesPerServing { get; set; }

    public override string ToString()
    {
        return $"BeverageId: {BeverageId}, Name: {Name}, Type: {Type}, MainIngredient: {MainIngredient}, Origin: {Origin}, CaloriesPerServing: {CaloriesPerServing}";
    }

}
