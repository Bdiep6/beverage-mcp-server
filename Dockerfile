# Use the official .NET 9 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET 9 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BeverageMcpServer.csproj", "./"]
RUN dotnet restore "BeverageMcpServer.csproj"

# Copy all source files
COPY . .

# Build the application
RUN dotnet build "BeverageMcpServer.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BeverageMcpServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Create non-root user for security
RUN adduser --disabled-password --gecos '' --shell /bin/bash --uid 1001 appuser
USER appuser

ENTRYPOINT ["dotnet", "BeverageMcpServer.dll"]