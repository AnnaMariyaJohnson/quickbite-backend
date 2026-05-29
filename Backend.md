**✅ Here is the complete backend documentation in Markdown format.**

```markdown
# QuickBite - Backend Development Documentation
**ASP.NET Core Web API + Clean Architecture**

---

## Project Overview
This is the **backend** for QuickBite food delivery app built with **.NET 8** using **Clean Architecture**.

**Goal**: Provide REST APIs for authentication, restaurants, menu, orders, etc.

---

## Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server** (or PostgreSQL)
- **JWT Authentication**
- **Clean Architecture**
- **AutoMapper**
- **FluentValidation**
- **Swagger** (for API documentation)

---

## Folder Structure

```bash
backend/
├── QuickBite.sln
├── QuickBite.Api/                  # Presentation Layer
│   ├── Controllers/
│   ├── Program.cs
│   ├── appsettings.json
│   └── Properties/
├── QuickBite.Domain/               # Domain Layer
│   ├── Entities/
│   ├── Enums/
│   └── Common/
├── QuickBite.Application/          # Application Layer
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   ├── Mappings/
│   └── Common/
├── QuickBite.Infrastructure/       # Infrastructure Layer
│   ├── Identity/
│   ├── Repositories/
│   ├── Authentication/
│   └── DependencyInjection.cs
├── QuickBite.Persistence/          # Persistence Layer
│   ├── DbContext/
│   ├── Configurations/
│   ├── Migrations/
│   └── DependencyInjection.cs
└── README.md
```

---

## Phase-wise Development Plan

### **Phase 1: Project Setup & Authentication (Week 1)**
- [ ] Setup Clean Architecture
- [ ] Database Configuration
- [ ] User Registration & Login (JWT)
- [ ] `/auth/me` endpoint

### **Phase 2: Core Entities (Week 2)**
- [ ] Restaurant + MenuItem
- [ ] Order + OrderItem
- [ ] Address

### **Phase 3: Main Features**
- [ ] Restaurant & Menu APIs
- [ ] Cart → Order Flow
- [ ] User Profile APIs

---

## Step-by-Step Setup Guide

### 1. Install Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (with ASP.NET workload)
- SQL Server Express

### 2. Create Solution

```bash
mkdir QuickBite-Backend && cd QuickBite-Backend
dotnet new sln -n QuickBite

dotnet new webapi -n QuickBite.Api -o QuickBite.Api
dotnet new classlib -n QuickBite.Domain -o QuickBite.Domain
dotnet new classlib -n QuickBite.Application -o QuickBite.Application
dotnet new classlib -n QuickBite.Infrastructure -o QuickBite.Infrastructure
dotnet new classlib -n QuickBite.Persistence -o QuickBite.Persistence

dotnet sln add QuickBite.Api/QuickBite.Api.csproj
dotnet sln add QuickBite.Domain/QuickBite.Domain.csproj
# ... add all projects
```

### 3. Add References (Important)

Refer to previous messages for reference commands.

### 4. Install NuGet Packages

**QuickBite.Persistence:**
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**QuickBite.Infrastructure:**
```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

**QuickBite.Api:**
```bash
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.Design
```

---

## Important Files (Initial Setup)

### `QuickBite.Domain/Entities/User.cs`
```csharp
using Microsoft.AspNetCore.Identity;

namespace QuickBite.Domain.Entities;

public class User : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

### `QuickBite.Persistence/ApplicationDbContext.cs`
```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickBite.Domain.Entities;

namespace QuickBite.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Add configurations later
    }
}
```

---

## Next Immediate Steps (Do Today)

1. Complete project setup with all references and packages.
2. Configure `Program.cs` in Api project (I'll give full code when you're ready).
3. Create first migration and test database connection.
4. Implement Register & Login endpoints.

---

## Connection with Frontend

**Base URL** (for development):
```env
BASE_URL=http://10.0.2.2:5000/api   # Android Emulator
# BASE_URL=http://192.168.x.x:5000/api   # Physical Device
```

**Important Endpoints to implement first:**
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`

---

## Learning Resources

- Clean Architecture in .NET: YouTube - "Milan Jovanović Clean Architecture"
- JWT Auth in .NET 8: Search "JWT Authentication .NET 8"
- EF Core: Microsoft Learn

---

**Last Updated**: May 29, 2026

---

