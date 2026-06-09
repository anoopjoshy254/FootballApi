FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["FootballApi.csproj", "./"]
RUN dotnet restore "FootballApi.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet publish "FootballApi.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "FootballApi.dll"]
