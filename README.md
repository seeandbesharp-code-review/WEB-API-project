# 🚀 Web API — .NET 9

A RESTful Web API built with **C#** and **.NET 9**, following clean architecture principles and modern backend development best practices.

---

## 🏗️ Architecture & Layers

The project is structured in clearly separated layers to ensure maintainability, testability, and scalability:

| Layer | Responsibility |
|---|---|
| **Application** | Business logic, use-case orchestration |
| **Service** | Domain services and operations |
| **Repository** | Data access abstraction over the database |
| **DTO** | Data transfer objects to decouple layers |

Layers are connected via **Dependency Injection (DI)**, achieving loose coupling (decoupling) between components and making the system easy to test and extend.

---

## 🔑 Key Features

### ✅ RESTful Design
The API is built following REST principles — proper use of HTTP verbs, status codes, and resource-oriented routing.

### 🗄️ Entity Framework Core (ORM)
Database access is handled through **Entity Framework Core**, providing a strongly-typed, LINQ-based interface to the database.

### ⚡ Asynchronous Data Access
All database operations are performed **asynchronously** (`async/await`), freeing threads and significantly improving scalability under load.

### 📦 DTO Layer with Records & AutoMapper
- **DTOs** (Data Transfer Objects) are used to break circular dependencies and decouple the data layer from the rest of the application.
- DTOs are implemented as **C# Records**, which are ideal for immutable data-carrying objects.
- Mapping between entities and DTOs is handled automatically using **AutoMapper**.

### ⚙️ Configuration Management
Application settings (connection strings, feature flags, etc.) are stored externally in `appsettings.json` , keeping configuration separate from code.

### 📋 Logging with NLog
The project makes extensive use of **NLog** for structured logging across all layers, providing detailed insight into application behavior and flow.

### 🛡️ Centralized Error Handling
All exceptions are caught and processed by a global **Error Handling Middleware**, ensuring consistent error responses and preventing raw exception details from leaking to clients.

### 📊 Traffic Auditing
All incoming requests are tracked and stored in a dedicated **Rating table** in the database, enabling traffic monitoring and analytics.

### 🧪 Unit Testing with xUnit
The project includes a comprehensive test suite powered by **xUnit**, covering the core logic .

---

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| .NET 9 / C# | Core framework & language |
| ASP.NET Core Web API | REST API host |
| Entity Framework Core | ORM / Data access |
| AutoMapper | Object-to-object mapping |
| NLog | Logging |
| xUnit | Unit testing |
| Dependency Injection (built-in) | Decoupling layers |

---

## 📁 Project Structure

```
├── API/                  # Entry point, controllers, middleware
├── Services/             # Business logic implementations
├── Repositories/         # Data access implementations
├── Entities/             # domain models
├── DTOs/                 # Record-based data transfer objects
├── Tests/                # xUnit test projects
└── appsettings.json      # External configuration
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- A supported database (SQL Server / PostgreSQL / etc.)

### Run the API

```bash
# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update

# Run the project
dotnet run --project API
```

### Run Tests

```bash
dotnet test
```

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).