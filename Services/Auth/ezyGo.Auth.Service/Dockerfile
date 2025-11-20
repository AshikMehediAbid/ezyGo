# Use the official .NET 8 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["ezyGo.Auth.Service/ezyGo.Auth.Service.csproj", "ezyGo.Auth.Service/"]
COPY ["ezyGo.Auth.Domain/ezyGo.Auth.Domain.csproj", "ezyGo.Auth.Domain/"]
COPY ["ezyGo.Auth.Storage/ezyGo.Auth.Storage.csproj", "ezyGo.Auth.Storage/"]
COPY ["ezyGo.Auth.Middleware/ezyGo.Auth.Middleware.csproj", "ezyGo.Auth.Middleware/"]

# Restore dependencies
RUN dotnet restore "ezyGo.Auth.Service/ezyGo.Auth.Service.csproj"

# Copy everything else
COPY . .

# Build the application
WORKDIR "/src/ezyGo.Auth.Service"
RUN dotnet build "ezyGo.Auth.Service.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ezyGo.Auth.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "ezyGo.Auth.Service.dll"]
