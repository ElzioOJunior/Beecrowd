# Base para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7777

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj do Gateway
COPY ["src/SalesApi.Gateway/SalesApi.Gateway.csproj", "SalesApi.Gateway/"]

# Restaurar dependências do projeto Gateway
RUN dotnet restore "SalesApi.Gateway/SalesApi.Gateway.csproj"

# Copiar os arquivos restantes
COPY /src .

# Compilar o projeto
WORKDIR "/src/SalesApi.Gateway"
RUN dotnet build "SalesApi.Gateway.csproj" -c Release -o /app/build

# Publicar os binários
FROM build AS publish
RUN dotnet publish "SalesApi.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:7777

ENTRYPOINT ["dotnet", "SalesApi.Gateway.dll"]
