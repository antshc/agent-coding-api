## Overview
This system is designed to manage employee payroll and benefit deductions using a biweekly pay cycle, where employees receive 26 paychecks per year. The platform supports viewing employees and their dependents, and accurately calculates net paycheck amounts by applying predefined deduction policies.

This is a **.NET 10 Web API** monolith solution (`Benefits.sln`) using a **layered architecture with Domain-Driven Design (DDD)** patterns, all contained within a single `Api` project. The namespace root is `Api`.

The project follows CQRS-style separation between read queries and write commands. The test project (`ApiTests`) lives alongside the main project in the solution.

## Key Technology Decisions

- **.NET 10** / `net10.0` target framework (pinned via `global.json` SDK `10.0.102`)
- **EF Core 10** with SQL Server (`Microsoft.EntityFrameworkCore.SqlServer`)
- **Swashbuckle 10** for OpenAPI / Swagger documentation
- **xUnit** for unit and integration tests
- `InternalsVisibleTo("ApiTests")` allows `internal` query implementations to be tested directly
---

## Coding Guidelines

### Indentation

We use spaces.

### General Naming Conventions

- **Types, methods, properties, events, and local functions**: PascalCase
- **Enum values**: PascalCase
- **Constants** (`const` fields and locals): PascalCase
- **Non-private static and readonly fields**: PascalCase
- **Private instance fields**: camelCase with `_` prefix (e.g., `_myField`)
- **Private/internal static fields**: camelCase
- **Local variables and parameters**: camelCase
- Use whole words in names when possible

### Code Quality

- Always pass `CancellationToken` through async call chains — every public async method must accept and forward it.
- Do not use `async`/`await` in methods that merely return a `Task` without any `await` expression — return the `Task` directly to avoid unnecessary state machine overhead.
- Do not duplicate `using` directives. Reuse existing imports; never add a `using` that is already present in the file.
- Do not use `object`, untyped collections, or suppress nullable warnings (e.g., `!`) unless strictly necessary. Prefer proper types or domain-specific interfaces.
- Never catch exceptions by message string comparison (e.g., `when (e.Message == "...")`). Define and throw typed exceptions or use result types instead.
- Do not duplicate code. Always look for existing utility functions, helpers, or patterns in the codebase before implementing new functionality. Reuse and extend existing code whenever possible.
- Controllers must not reference `DataContext` or repository implementations directly — always go through a Query or Command abstraction.
- Domain entities must enforce invariants in their constructors. Never leave an entity in an invalid state.
- Register every new repository, query, or service in `DependencyInjection.cs` under the correct layer method (`ApplicationLayer`, `DomainLayer`, `DataLayer`, `SharedKernel`).
- Do not add tests to the wrong class. Unit tests go in `UnitTests/{Feature}/`, integration tests go in `IntegrationTests/`. Look for existing test classes before creating new ones.
- If you create any temporary files, scripts, or helpers during iteration, remove them before finishing the task.

---

## Directory Visualization

```
src/
├── Benefits.sln
├── Api/                              # Main Web API project
│   ├── Api.csproj
│   ├── Program.cs                    # Application entry point, middleware pipeline
│   ├── DependencyInjection.cs        # Centralized DI registration (extension method)
│   ├── PreRunHook.cs                 # Startup hooks (e.g., DB migrations)
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   │
│   ├── Application/                  # Presentation + Application layer
│   │   └── {Feature}/               # Feature folder (e.g., Users/)
│   │       ├── {Feature}Controller.cs
│   │       ├── Payload/             # DTOs (Data Transfer Objects)
│   │       │   └── Get{Feature}Dto.cs
│   │       └── Queries/             # Read-side query interfaces + implementations
│   │           ├── I{Feature}Query.cs
│   │           └── {Feature}Query.cs
│   │
│   ├── Domain/                       # Domain model layer
│   │   └── {Feature}/               # Feature folder (e.g., Users/)
│   │       ├── {Feature}.cs          # Aggregate root entity
│   │       └── I{Feature}Repository.cs
│   │
│   ├── Data/                         # Infrastructure / persistence layer
│   │   ├── DataContext.cs            # EF Core DbContext
│   │   ├── DatabaseInitializer.cs    # Seeding and schema initialization
│   │   ├── {Feature}Repository.cs    # Repository implementations
│   │   └── Configurations/          # EF Core entity type configurations
│   │       └── {Feature}Configuration.cs
│   │
│   ├── SharedKernel/                 # Cross-cutting, framework-agnostic abstractions
│   │   ├── IDateTimeProvider.cs
│   │   ├── DateTimeProvider.cs
│   │   ├── Domain/                  # DDD base types
│   │   │   ├── Entity.cs            # Base entity with Id and equality
│   │   │   ├── IAggregateRoot.cs    # Marker interface for aggregate roots
│   │   │   └── IBusinessRule.cs     # Business rule enforcement contract
│   │   └── Payload/                 # Shared API response wrappers
│   │       └── ApiResponse.cs
│   │
│   └── Properties/
│       └── launchSettings.json
│
└── ApiTests/                         # Test project
    ├── ApiTests.csproj
    ├── IntegrationTests/             # HTTP-level integration tests (live server)
    │   ├── IntegrationTest.cs        # Base class with HttpClient setup
    │   ├── ShouldExtensions.cs       # Assertion helpers
    │   └── {Feature}IntegrationTests.cs
    └── UnitTests/                    # Domain/unit tests
        └── {Feature}/               # Feature-grouped unit tests
            └── {Feature}UnitTests.cs
```

---

## Layer Responsibilities

### `Application/{Feature}/`
The presentation and application layer combined. Controllers handle HTTP concerns; Queries implement read-side application logic (CQRS read side).

- **`{Feature}Controller.cs`** — API controller, routes, HTTP status code mapping. Depends on `I{Feature}Query` for reads.
- **`Payload/`** — Output DTOs consumed by the controller and returned to callers.
- **`Queries/`** — `I{Feature}Query` (public interface) and `{Feature}Query` (internal implementation). Query classes compose domain repositories and map to DTOs.

### `Domain/{Feature}/`
Pure domain logic — no framework or infrastructure dependencies.

- **`{Feature}.cs`** — Aggregate root; inherits `Entity`, implements `IAggregateRoot`. Enforces invariants in constructor. Uses private setters. EF Core parameterless constructor is private.
- **`I{Feature}Repository.cs`** — Repository interface defined in the domain, implemented in the Data layer (Dependency Inversion Principle).

### `Data/`
EF Core infrastructure layer.

- **`DataContext.cs`** — `DbContext` subclass; applies all entity configurations via `ApplyConfiguration`.
- **`{Feature}Repository.cs`** — Implements `I{Feature}Repository` from the domain. Uses `DataContext` directly.
- **`DatabaseInitializer.cs`** — Called from `PreRunHook` at startup for migrations and seeding.
- **`Configurations/{Feature}Configuration.cs`** — Implements `IEntityTypeConfiguration<{Feature}>` for table mapping, constraints, and indexes.

### `SharedKernel/`
Framework-agnostic primitives shared across all layers.

- **`Domain/Entity.cs`** — Abstract base class providing `Id`, structural equality, and `CheckRule(IBusinessRule)`.
- **`Domain/IAggregateRoot.cs`** — Marker interface to identify aggregate roots.
- **`Domain/IBusinessRule.cs`** — Contract for validatable business rules (`void Check()`).
- **`Payload/ApiResponse<T>`** — Standard API response envelope with `Data`, `Success`, `Message`, `Error`.
- **`IDateTimeProvider` / `DateTimeProvider`** — Abstraction for `DateTime.UtcNow` to support testability.

### Root-level files
- **`Program.cs`** — Composes the host: registers services, middleware, CORS, Swagger, maps controllers.
- **`DependencyInjection.cs`** — Static extension class `AddDependencyRegistrations`; contains private methods `ApplicationLayer`, `DomainLayer`, `DataLayer`, `SharedKernel` for organized registration.
- **`PreRunHook.cs`** — Runs before `app.Run()`; currently calls `DatabaseInitializer.Initialize`.

---

## Naming Conventions

| Artifact | Convention | Example |
|---|---|---|
| Folders | PascalCase, plural for features | `Users/` |
| Controllers | `{Feature}Controller` | `UsersController` |
| Query interface | `I{Feature}Query` | `IUserQuery` |
| Query implementation | `{Feature}Query` (internal) | `UserQuery` |
| Repository interface | `I{Feature}Repository` | `IUserRepository` |
| Repository implementation | `{Feature}Repository` | `UserRepository` |
| Output DTOs | `Get{Feature}Dto`, `Create{Feature}Dto` | `GetUserDto` |
| EF Core config | `{Feature}Configuration` | `UserConfiguration` |
| Domain entities | Singular noun | `User` |
| Test classes | `{Feature}IntegrationTests`, `{Feature}UnitTests` | `UserIntegrationTests` |
| Interfaces | `I` prefix | `IUserQuery` |

---

## Dependency Flow

```
Application (Controllers + Queries)
    ↓ depends on
Domain (Entities + Repository Interfaces)
    ↑ implemented by
Data (Repositories + DbContext)

SharedKernel ← referenced by all layers
```

- Controllers **must not** directly reference `DataContext` or repositories — use Queries.
- Queries **may** reference domain repository interfaces (through DI).
- Domain entities **must not** reference any layer except `SharedKernel`.

---

## Adding a New Feature — Checklist

When adding a new domain feature (e.g., `Departments`):

1. **Domain** — Create `Domain/Departments/Department.cs` (extends `Entity`, implements `IAggregateRoot`) and `Domain/Departments/IDepartmentRepository.cs`.
2. **Data** — Create `Data/DepartmentRepository.cs` and `Data/Configurations/DepartmentConfiguration.cs`. Register the `DbSet<Department>` in `DataContext`.
3. **Application** — Create `Application/Departments/DepartmentsController.cs`, `Payload/GetDepartmentDto.cs`, `Queries/IDepartmentQuery.cs`, and `Queries/DepartmentQuery.cs`.
4. **DI** — Register the new repository and query in `DependencyInjection.cs` under the appropriate layer method.
5. **Tests** — Add `ApiTests/IntegrationTests/DepartmentIntegrationTests.cs` and `ApiTests/UnitTests/Departments/DepartmentUnitTests.cs`.
