# Etapa 1: Build da aplica��o
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia o csproj e restaura as depend�ncias
COPY *.csproj ./
RUN dotnet restore

# Copia todo o c�digo e publica a aplica��o
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Imagem final para rodar a aplica��o
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/out .

# Exp�e a porta 80
EXPOSE 80

# Comando para rodar a aplica��o
ENTRYPOINT ["dotnet", "LaporteAPI.dll"]
