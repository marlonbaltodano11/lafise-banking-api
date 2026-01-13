# Lafise.BankingApi

> [!NOTE]
> This project is a technical test for a programming evaluation.
> It is not intended for production use.

---

## Description
**Lafise.BankingApi** is a banking API developed with .NET 8 using ASP.NET Core Web API.

It allows users to:

 - Create client profiles.
 - Create bank accounts.
 - Check account balances.
 - Record deposits and withdrawals.
 - View transaction history.

The project follows **Clean Architecture**, **Domain-Driven Design (DDD)** principles, **SOLID**, **dependency injection**, and includes **unit testing** for domain and application layers.

---

## System Requirements

* **.NET SDK 8.0**
* **Visual Studio 2022** or **Visual Studio Code** + C# extension
* **Git**

---

## Project's Folder Structure

```
Lafise.BankingApi/
│
├── src/
│   ├── BankingApi.Api/            → Web API (Controllers, DI, Configuration, Swagger)
│   ├── BankingApi.Application/    → Application services and use cases
│   ├── BankingApi.Domain/         → Entities, business rules, exceptions
│   └── BankingApi.Infrastructure/ → EF Core, SQLite, Repositories, Configurations
│
├── tests/
│   ├── BankingApi.Domain.Tests/      → Unit tests for domain logic
│   └── BankingApi.Application.Tests/ → Unit tests for application services
│
└── README.md
```

---

## Software Architecture

**Clean Architecture / Layered design**:

* **Domain:** Entities, business rules, domain exceptions. Pure business logic.
* **Application:** Services (use cases), orchestrates domain entities. Depends on **Domain**.
* **Infrastructure:** Persistence (EF Core + SQLite), repositories. Depends on **Domain/Application**.
* **API:** Controllers, DI, middleware, Swagger. Depends on **Application**.
* **Tests:** xUnit + NSubstitute for unit testing.

**Dependency Rules:**

* Higher layers depend only on lower layers.

  * Api → Application → Domain
  * Infrastructure → Application / Domain
* No circular dependencies.
* Ensures business logic is isolated and testable.

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/marlonbaltodano11/lafise-banking-api.git
cd lafise-banking-api
```

### 2. Install .NET tools and NuGet packages

```bash
dotnet tool restore
dotnet restore
```

> This will ensure all NuGet packages and .NET tools are installed.

---

### 3. Database Setup (SQLite)

1. Navigate to the **src/** folder:

```bash
cd src/
```

2. Generate the database:

```bash
dotnet ef database update --project BankingApi.Infrastructure --startup-project BankingApi.Api
```

* This will create the SQLite database according to the latest migrations.

---

### 4. Running the API

#### a) Using Visual Studio

1. Open `Lafise.BankingApi.sln` in Visual Studio 2022.
2. Set **BankingApi.Api** as the startup project.
3. Press **F5** to run the API.
4. Swagger UI will be available at: `https://localhost:7191/swagger` (default port).

#### b) Using VS Code / Command Line

```bash
cd src/BankingApi.Api
dotnet run
```

Swagger UI will be available at: `http://localhost:5121/swagger` (default port).

---

### 5. Running Unit Tests

All tests use **xUnit** and **NSubstitute** for mocking.

* **Domain tests:** `BankingApi.Domain.Tests`
* **Application tests:** `BankingApi.Application.Tests`

Run all tests from the root folder:

```bash
dotnet test
```

> This will build all projects, execute tests, and display results.
> Tests are deterministic, isolated, and easy to read.

To see detailed output use:

```bash
dotnet test --logger "console;verbosity=detailed"
```

---

### 6. Packages Used

**Tests:**

* `xunit`
* `xunit.runner.visualstudio`
* `NSubstitute`
* `Microsoft.NET.Test.Sdk`

**Persistence / EF Core:**

* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.Sqlite`
* `Microsoft.EntityFrameworkCore.Tools`

**API:**

* `Swashbuckle.AspNetCore` (Swagger)

---

## 7. Exception Handling

Custom middleware handles exceptions consistently:

* **InsufficientFundsException** → 400
* **NotFoundException** → 404
* **ArgumentException** → 400
* Other exceptions → 500

Example JSON response:

```json
{
  "success": false,
  "errors": [
    {
      "code": "NOT_FOUND",
      "message": "Account with number 123456789 not found"
    }
  ]
}
```

---

## 8. Conventions Followed

* **Clean Architecture** separation
* **Dependency Injection** everywhere (`AccountService`, repositories, generators)
* **Unit tests** cover all domain and application logic
* **SOLID principles** applied
* **Swagger** documents all endpoints

---
