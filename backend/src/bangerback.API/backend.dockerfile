# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files to restore dependencies
# Paths are relative to the 'context' defined in docker-compose (the /src folder)
COPY ["bangerback.API/bangerback.API.csproj", "bangerback.API/"]
COPY ["bangerback.Core/bangerback.Core.csproj", "bangerback.Core/"]
COPY ["bangerback.Infrastructure/bangerback.Infrastructure.csproj", "bangerback.Infrastructure/"]

RUN dotnet restore "bangerback.API/bangerback.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/bangerback.API"
RUN dotnet publish "bangerback.API.csproj" -c Release -o /app

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "bangerback.API.dll"]