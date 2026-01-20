# BancoKrt - Sistema de Gestão de Contas

Este projeto é uma solução robusta para gestão de contas bancárias, desenvolvida com foco em **Clean Architecture**, **Domain-Driven Design (DDD)** e alta performance através de **Caching**.

## 🚀 Tecnologias Utilizadas

* **ASP.NET Core 8.0 (MVC)**
* **Entity Framework Core**
* **PostgreSQL** (Banco de dados relacional)
* **Memory Cache** (Estratégia de Cache-Aside com Decorator Pattern)
* **xUnit** (Testes de Unidade)
* **Bootstrap 5** (Interface UI com Dark Theme)

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture, dividido em:
- **Domain**: Entidades de negócio e interfaces.
- **Application**: Serviços de aplicação e DTOs.
- **Infrastructure**: Implementação de repositórios, contexto do banco de dados e logs.
- **Api/Web**: Camada de apresentação (MVC).

---

## ⚙️ Configuração do Banco de Dados

Antes de rodar o projeto, você precisa configurar a sua string de conexão com o **PostgreSQL**.

1.  Abra o arquivo `BancoKrt.Api/appsettings.json`.
2.  Localize a seção `ConnectionStrings`.
3.  Altere as credenciais (Server, Port, Database, User Id, Password) conforme o seu ambiente local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=bancokrt_db;User Id=postgres;Password=SUA_SENHA_AQUI;"
}
```

## 🛠️ Como Rodar o Projeto

1. Restaurar dependências:
```bash
dotnet restore
```

2. Atualizar o Banco de Dados (Migrations):
```bash
dotnet ef database update --project BancoKrt.Infrastructure --startup-project BancoKrt.Api
```

3. Executar a aplicação:
```bash
dotnet run --project BancoKrt.Api
```

## 🧪 Testes

1. Executar os testes:
```bash
dotnet test
```
