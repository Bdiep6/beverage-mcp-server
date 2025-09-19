using System;
using System.Text.Json.Serialization;

namespace BeverageMcpServer.Model;


[JsonSourceGenerationOptions] // Removed PropertyNamingPolicy to use default PascalCase
[JsonSerializable(typeof(Beverage))]
[JsonSerializable(typeof(List<Beverage>))]

internal sealed partial class BeverageContext : JsonSerializerContext
{
}
