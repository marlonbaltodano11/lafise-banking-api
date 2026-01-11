# Lafise.BankingApi

> [!NOTE]
> This project is a technical test for a programming evaluation.
> It is not intended for production use.

## Description
**Lafise.BankingApi** is a banking API developed with .NET 8 using ASP.NET Core Web API.
It allows users to manage bank accounts, including:
 - Creating client profiles.
 - Creating bank accounts.
 - Checking account balances.
 - Recording deposits and withdrawals.
 - Viewing transaction history.
The project follows .NET best practices, SOLID principles, dependency injection, and unit testing.

## System Requirements
 - .NET SDK 8.0
 - Visual Studio 2022 or Visual Studio Code + C# extension
 - Git
 - SQLite

## Project's Folder Structure

```
Lafise.BankingApi/
│
├── src/
│   ├── BankingApi.Api/            → Web API (Controllers, DI, Configuration)
│   ├── BankingApi.Application/    → Services and use cases
│   ├── BankingApi.Domain/         → Entities and business rules
│   └── BankingApi.Infrastructure/ → EF Core, SQLite, Repositories
│
├── tests/
│   └── BankingApi.Tests/          → Unit tests
│
└── README.md
```

## Software Architecture
The project follows a **layered architecture** inspired by **Clean Architecture** principles. Responsibilities are separated to make the code maintainable, testable, and extensible.

### Layers
 1. Domain (`BankingApi.Domain`)
 2. Application (`BankingApi.Application`)
 3. Infrastructure (`BankingApi.Infrastructure`)
 4. API (`BankingApi.Api`)
 5. Tests (`BankingApi.Tests`)

### Dependency Rules
 - Higher layers depend on lower layers only:
   - **Api → Application → Domain**
   - **Infrastructure → Application / Domain**
 - No circular dependencies are allowed.
 - This ensures that **business logic is isolated** from external concerns.