# Inventory Management API
 
Simple inventory control API built with ASP.NET Core. Supports JWT authentication, stock movements, and a clean layered architecture.
 
---
 
## ⚙️ Tech Stack
 
- ASP.NET Core 8 + Entity Framework (SQLite)

- JWT Authentication with Refresh Token

- Swagger / OpenAPI

- xUnit for unit testing
 
---
 
## ✅ Features
 
- CRUD for Products and Categories

- Stock movements (Entry, Output, Adjustment)

- JWT login + refresh token

---
 
## 🧱 Project Structure
 
```

InventoryManagement.API          // API Controllers

├── Application                  // Services, DTOs, Interfaces

├── Domain                       // Core models, enums, interfaces

└── Infrastructure               // EF DbContext, repositories

```
 
 
> The architecture is intentionally simple and focused on maintainability. Clean Architecture was avoided for simplicity, but separation of concerns and SOLID principles were followed.
 
---
 
## 🚀 Getting Started
 
```bash

dotnet restore

dotnet ef database update -s InventoryManagement.API -p InventoryManagement.Infrastructure

dotnet run --project InventoryManagement.API
 
