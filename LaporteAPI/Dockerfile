# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia o csproj e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código e publica a aplicação
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Imagem final para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/out .

# Expõe a porta 80
EXPOSE 80

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "LaporteAPI.dll"]
