# Use .NET 8 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AdminDashboardApi/AdminDashboardApi.csproj ./AdminDashboardApi/
RUN dotnet restore ./AdminDashboardApi/AdminDashboardApi.csproj

COPY AdminDashboardApi/. ./AdminDashboardApi/
WORKDIR /src/AdminDashboardApi
RUN dotnet publish -c Release -o /app/publish

# Runtime image for .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "AdminDashboardApi.dll"]
