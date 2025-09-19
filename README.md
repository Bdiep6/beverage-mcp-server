# Beverage MCP Server

A Model Context Protocol (MCP) server that provides access to beverage data with 33 different beverages from around the world.

## 🚀 Quick Start with Docker

### Option 1: Pull from GitHub Container Registry
```bash
# Pull the latest image
docker pull ghcr.io/bdiep6/beverage-mcp-server:latest

# Run the server
docker run -p 8080:8080 -d --name beverage-server ghcr.io/bdiep6/beverage-mcp-server:latest
```

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/bdiep6/beverage-mcp-server.git
cd beverage-mcp-server

# Build the Docker image
docker build -t beverage-mcp-server:latest .

# Run the container
docker run -p 8080:8080 -d --name beverage-server beverage-mcp-server:latest
```

## 📡 API Endpoints

Once running, the server provides these REST API endpoints:

- `GET /health` - Health check
- `GET /api/beverages` - Get all beverages
- `GET /api/beverages/{id}` - Get beverage by ID
- `GET /api/beverages/name/{name}` - Search by name
- `GET /api/beverages/type/{type}` - Search by type
- `GET /api/beverages/ingredient/{ingredient}` - Search by ingredient
- `GET /api/beverages/origin/{origin}` - Search by origin
- `GET /api/beverages/calories/{calories}` - Search by calories
- `GET /api/beverages/count` - Get total count

## 🔗 Consuming the Server

### From C# Console Application
```csharp
using var httpClient = new HttpClient();
var response = await httpClient.GetAsync("http://localhost:8080/api/beverages");
var json = await response.Content.ReadAsStringAsync();
```

### From JavaScript/Node.js
```javascript
const response = await fetch('http://localhost:8080/api/beverages');
const beverages = await response.json();
console.log(beverages);
```

### From Python
```python
import requests
response = requests.get('http://localhost:8080/api/beverages')
beverages = response.json()
print(beverages)
```

### From cURL
```bash
# Get all beverages
curl http://localhost:8080/api/beverages

# Search for tea beverages
curl http://localhost:8080/api/beverages/type/Tea

# Get beverage by ID
curl http://localhost:8080/api/beverages/1
```

## 📊 Sample Data

The server contains 33 beverages including:
- **Soft Drinks**: Coca-Cola, Pepsi, Lemonade
- **Coffee**: Espresso, Black Coffee, Latte
- **Tea**: Green Tea, Chai, Matcha Latte
- **Cocktails**: Margarita, Mojito, Pina Colada
- **And many more!**

## 🛠️ Development

### Local Development (MCP Mode)
```bash
dotnet run
```

### Docker Development (HTTP Mode)
```bash
docker build -t beverage-mcp-server .
docker run -p 8080:8080 beverage-mcp-server
```

## 🧪 Testing

Visit these URLs after starting the server:
- Health Check: http://localhost:8080/health
- API Documentation: http://localhost:8080/api
- All Beverages: http://localhost:8080/api/beverages

## 📦 Dependencies

- .NET 9.0
- ModelContextProtocol 0.3.0-preview.4
- ASP.NET Core (for HTTP mode)

## 🐳 Docker Hub

Also available on Docker Hub:
```bash
docker pull YOUR_DOCKERHUB_USERNAME/beverage-mcp-server:latest
```

## 📄 License

MIT License