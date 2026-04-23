# -----------------------------------------------------
# 🔹 Configuração da Imagem Base 🔹
# -----------------------------------------------------

# Usa a imagem oficial do .NET 10.0 como ambiente base (runtime).
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Define o diretório de trabalho dentro do container.
WORKDIR /app

# Expõe a porta 80 para acesso HTTP.
EXPOSE 80

# Define as URLs que o ASP.NET Core usará para servir a aplicação.
ENV ASPNETCORE_URLS=http://+:80

# Remove mensagens de boas-vindas e logs do .NET na primeira execução.
ENV DOTNET_NOLOGO=1

# Desativa a coleta de dados de telemetria do .NET CLI.
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1


# -----------------------------------------------------
# 🔹 Configuração da Build 🔹
# -----------------------------------------------------

# Usa a imagem SDK do .NET 10.0 para compilar e restaurar pacotes.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Define o diretório de trabalho dentro do container.
WORKDIR /src

# Copia arquivos de configuração globais (se existirem) para restaurar pacotes corretamente.
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]

# Copia os arquivos de projeto (CSProj) de cada módulo da aplicação.
COPY ["src/CustomerApi.WebApi/CustomerApi.WebApi.csproj", "src/CustomerApi.WebApi/"]
COPY ["src/CustomerApi.Application/CustomerApi.Application.csproj", "src/CustomerApi.Application/"]
COPY ["src/CustomerApi.Core/CustomerApi.Core.csproj", "src/CustomerApi.Core/"]
COPY ["src/CustomerApi.Domain/CustomerApi.Domain.csproj", "src/CustomerApi.Domain/"]
COPY ["src/CustomerApi.Infrastructure/CustomerApi.Infrastructure.csproj", "src/CustomerApi.Infrastructure/"]
COPY ["src/CustomerApi.Query/CustomerApi.Query.csproj", "src/CustomerApi.Query/"]

# Restaura as dependências do projeto principal.
RUN dotnet restore "src/CustomerApi.WebApi/CustomerApi.WebApi.csproj"

# Copia todos os arquivos da solução para o container.
COPY . .

# Define o diretório de trabalho para a compilação.
WORKDIR "/src/src/CustomerApi.WebApi"

# Compila a aplicação no modo Release sem restaurar novamente os pacotes.
RUN dotnet build "CustomerApi.WebApi.csproj" --configuration Release --no-restore --nologo --output /app/build


# -----------------------------------------------------
# 🔹 Configuração da Publicação 🔹
# -----------------------------------------------------

# Publica a aplicação em uma pasta temporária dentro do container.
FROM build AS publish
RUN dotnet publish "CustomerApi.WebApi.csproj" --configuration Release --no-restore --nologo --output /app/publish


# -----------------------------------------------------
# 🔹 Configuração Final 🔹
# -----------------------------------------------------

# Usa a imagem base novamente para gerar a versão final do container.
FROM base AS final

# Define o diretório de trabalho da aplicação dentro do container.
WORKDIR /app

# Copia os arquivos publicados na etapa anterior para a imagem final.
COPY --from=publish /app/publish .

# Define o comando de inicialização do container.
ENTRYPOINT ["dotnet", "CustomerApi.WebApi.dll"]


# -----------------------------------------------------
# 🔹 Explicação Detalhada 🔹
# -----------------------------------------------------

# ✅ Multi-stage build:
#   1️⃣ Base → ambiente runtime do .NET.
#   2️⃣ Build → compila todos os projetos da solução.
#   3️⃣ Publish → gera versão otimizada da aplicação.
#   4️⃣ Final → imagem enxuta só com o necessário para rodar.

# ✅ Benefícios:
#   🔹 Imagem final menor e mais segura.
#   🔹 Build mais rápido e limpo.
#   🔹 Facilita manutenção e entendimento.

# ✅ Configurações de Ambiente:
#   🔹 ASPNETCORE_URLS=http://+:80 → servidor escuta na porta 80.
#   🔹 DOTNET_NOLOGO=1 → remove mensagens iniciais.
#   🔹 DOTNET_CLI_TELEMETRY_OPTOUT=1 → desativa telemetria.

# ✅ ENTRYPOINT:
#   🔹 Define que o container executará `dotnet CustomerApi.WebApi.dll`.
