
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["MultiTenantApp.csproj", "."]
RUN dotnet restore "./MultiTenantApp.csproj"
COPY . .
RUN dotnet build "MultiTenantApp.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 80

COPY --from=build /app/build .

HEALTHCHECK --interval=30s --timeout=5s CMD curl --fail http://localhost/health || exit 1

ENTRYPOINT ["dotnet", "MultiTenantApp.dll"]
