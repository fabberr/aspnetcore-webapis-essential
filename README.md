# Catalog API

## Descrição
Aplicação Web API desenvolvida no framework ASP.NET Core durante o curso [Web API ASP .NET Core Essencial (.NET 8)](https://www.udemy.com/course/curso-web-api-asp-net-core-essencial/?couponCode=KEEPLEARNING), ministrado por Jose Carlos Macoratti ([@macoratti](https://github.com/)).

Tecnologias utilziadas:
- .NET 8
- ASP.NET Core Web APIs
- Entity Framework Core
- SQLite

## Dependências

- SQLite 3.7.x
- .NET SDK 8.x
- .NET Runtime 8.x
- ASP.NET Core Runtime 8.x
- Entity Framework Core tools

## Migrations

Para utilizar o recurso de migrations do EF Core, primeiro instale/atualize a ferramente .NET [`dotnet-ef`](https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools) globalmente.

```bash
# Instala a ferramenta
dotnet tool install --global dotnet-ef

# Atualiza a ferramenta, caso já esteja instalada
dotnet tool update --global dotnet-ef
```
Devido a estrutura dos projetos, as opções `--project` e `--startup-project` devem ser adicionadas sempre que algum comando da ferramenta for chamado, informando os caminhos relevantes.
- `--project`: Caminho relativo até o diretório raiz do projeto `Catalog.Core.csproj`, contendo os modelos das entidades e a classe que implementa o `DbContext`. Migrations serão adicionadas a este projeto.
- `--startup-project`: Caminho relativo até o diretório raiz do projeto `Catalog.Api.csproj`, que contém a aplicação ASP.NET Core. A string de conexão e provedor do banco de dados ficam configurados no arquivo `appsettings.json`.

```bash
# Exemplo de comando dotenet-ef, executado na raiz do repositório
dotnet ef --project src/Catalog.Core --startup-project src/Catalog.Api <command> <subcommand>
```

### Criando Migrations

```bash
dotnet ef --project src/Catalog.Core --startup-project src/Catalog.Api migrations add <name>
```

### Executando Migrations

```bash
dotnet ef --project src/Catalog.Core --startup-project src/Catalog.Api database update
```
