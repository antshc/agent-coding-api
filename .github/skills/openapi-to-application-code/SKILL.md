---
name: openapi-to-aspnetcore-efcore
description: 'Generate a complete, production-ready ASP.NET Core Web API application from an OpenAPI specification using Entity Framework Core'
---

# Generate ASP.NET Core Web API from OpenAPI Spec

Your goal is to generate a complete, working **ASP.NET Core Web API** application from an OpenAPI specification using **ASP.NET Core**, **Entity Framework Core**, and modern .NET best practices.

## Input Requirements

1. **OpenAPI Specification**: Provide either:
   - A URL to the OpenAPI spec (e.g., https://api.example.com/openapi.json)
   - A local file path to the OpenAPI spec
   - The full OpenAPI specification content pasted directly

2. **Project Details** (if not in spec):
   - Project name and description
   - Target .NET version (prefer .NET 8 or later)
   - Root namespace
   - Database provider (SQL Server, PostgreSQL, or SQLite)
   - Authentication method (JWT Bearer, OAuth2, API Key, etc.)
   - Whether to use code-first migrations

## Generation Process

### Step 1: Analyze the OpenAPI Specification
- Validate the OpenAPI specification for correctness and completeness
- Identify all endpoints, HTTP methods, request/response schemas
- Extract security schemes and authorization requirements
- Identify entity candidates and relationships from schemas
- Detect pagination, filtering, sorting, and common query patterns
- Flag ambiguous or incomplete definitions

### Step 2: Design ASP.NET Core Architecture
Design the solution using standard ASP.NET Core layering:

- Controllers for HTTP endpoints
- Application / Services layer for business logic
- Data / Infrastructure layer for EF Core access
- Domain / Entities layer for persistent models
- DTOs / Contracts for API request and response contracts

### Step 3: Generate Application Code
Generate a production-ready ASP.NET Core Web API with:
- Program.cs using minimal hosting model
- Controllers with proper route mappings
- DTOs generated from OpenAPI schemas
- Entity models for persistent data
- DbContext and EF Core configuration
- Service layer for business logic
- Validation
- Global exception handling middleware
- Structured logging
- Swagger / OpenAPI integration

### Step 4: Add Data Access with Entity Framework Core
Include:
- ApplicationDbContext
- IEntityTypeConfiguration mappings
- Relationships, indexes, constraints
- EF Core migrations support
- Async database operations

### Step 5: Supporting Files
Generate:
- Unit tests
- Integration tests
- README.md
- .gitignore
- appsettings.json
- docker-compose.yml (optional)

## Recommended Project Structure

project-name/
├── README.md
├── project-name.sln
├── src/
│   ├── ProjectName.Api/
│   ├── ProjectName.Application/
│   ├── ProjectName.Domain/
│   └── ProjectName.Infrastructure/
├── tests/
└── docker-compose.yml

## Best Practices

- Use dependency injection
- Prefer async/await
- Keep controllers thin
- Put logic in services
- Use DTOs at API boundaries
- Use EF Core fluent configuration
- Use migrations
- Add centralized exception handling
- Return proper HTTP status codes
