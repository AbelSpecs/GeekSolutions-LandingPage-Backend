# 1. SDK para compilar y publicar la app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos .csproj para restaurar dependencias de todas las capas
COPY ["src/GeekSolutions.Api/GeekSolutions.Api.csproj", "src/GeekSolutions.Api/"]
COPY ["src/GeekSolutions.Application/GeekSolutions.Application.csproj", "src/GeekSolutions.Application/"]
COPY ["src/GeekSolutions.Infrastructure/GeekSolutions.Infrastructure.csproj", "src/GeekSolutions.Infrastructure/"]

RUN dotnet restore "src/GeekSolutions.Api/GeekSolutions.Api.csproj"

# Copiar todo el código fuente y publicar
COPY . .
WORKDIR "/src/src/GeekSolutions.Api"
RUN dotnet publish "GeekSolutions.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime de Producción
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "GeekSolutions.Api.dll"]