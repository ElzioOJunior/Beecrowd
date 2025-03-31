FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7777

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj
COPY ["src/SalesApi/SalesApi.csproj", "SalesApi/"]
COPY ["src/SalesApi.Domain/SalesApi.Domain.csproj", "src/SalesApi.Domain/"]
COPY ["src/SalesApi.Application/SalesApi.Application.csproj", "SalesApi.Application/"]
COPY ["src/SalesApi.Infrastructure/SalesApi.Infrastructure.csproj", "SalesApi.Infrastructure/"]

# Restaurar dependências do projeto
RUN dotnet restore "SalesApi/SalesApi.csproj"


# Copiar os arquivos restantes
COPY /src .


# Compilar o projeto
WORKDIR "/src/SalesApi"
RUN dotnet build "SalesApi.csproj" -c Release -o /app/build

# Publicar os binários
FROM build AS publish
RUN dotnet publish "SalesApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SalesApi.dll"]
