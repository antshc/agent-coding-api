# EF Core Migration: Add Dependent Entity with Foreign Key to Employee

## Overview

You've added a new `Dependent` entity with a foreign key back to `Employee`. To persist this schema change, you need to generate an EF Core migration.

## Prerequisites

Ensure the EF Core tools are installed:

```bash
dotnet tool restore
```

Or install globally if needed:

```bash
dotnet tool install --global dotnet-ef
```

## Step 1: Verify Your Entity and Configuration

Ensure your `Dependent` entity and `Employee` navigation are correctly defined. For example:

```csharp
// Domain/Employees/Dependent.cs
public class Dependent : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public int EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    private Dependent() { } // EF Core

    public Dependent(string name, int employeeId)
    {
        Name = name;
        EmployeeId = employeeId;
    }
}
```

And a collection navigation on `Employee`:

```csharp
public IReadOnlyCollection<Dependent> Dependents => _dependents.AsReadOnly();
private readonly List<Dependent> _dependents = new();
```

Also ensure `DataContext` has a `DbSet<Dependent>` and that your `DependentConfiguration` (implementing `IEntityTypeConfiguration<Dependent>`) is registered via `ApplyConfigurationsFromAssembly` or `ApplyConfiguration`.

## Step 2: Generate the Migration

Run the following command from the solution root (or any directory) using the `--project` and `--startup-project` flags:

```bash
dotnet ef migrations add AddDependentEntity \
  --project src/Api/Api.csproj \
  --startup-project src/Api/Api.csproj \
  --output-dir Data/Migrations
```

On Windows (PowerShell or cmd, no line continuation):

```powershell
dotnet ef migrations add AddDependentEntity --project src/Api/Api.csproj --startup-project src/Api/Api.csproj --output-dir Data/Migrations
```

This will generate 3 files under `src/Api/Data/Migrations/`:
- `<timestamp>_AddDependentEntity.cs`  the migration Up/Down methods
- `<timestamp>_AddDependentEntity.Designer.cs`  the migration snapshot metadata
- `DataContextModelSnapshot.cs`  updated model snapshot

## Step 3: Review the Generated Migration

Open the generated `<timestamp>_AddDependentEntity.cs` and verify:
- A `CreateTable` (or `AddColumn`) call for the `Dependents` table
- A foreign key constraint referencing the `Employees` table on `EmployeeId`
- Appropriate indexes on the foreign key column

Example expected output in the migration:

```csharp
migrationBuilder.CreateTable(
    name: "Dependents",
    columns: table => new
    {
        Id = table.Column<int>(nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        Name = table.Column<string>(nullable: false),
        EmployeeId = table.Column<int>(nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Dependents", x => x.Id);
        table.ForeignKey(
            name: "FK_Dependents_Employees_EmployeeId",
            column: x => x.EmployeeId,
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });

migrationBuilder.CreateIndex(
    name: "IX_Dependents_EmployeeId",
    table: "Dependents",
    column: "EmployeeId");
```

## Step 4: Apply the Migration (Optional  Local Dev)

To apply the migration to your local development database:

```powershell
dotnet ef database update --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

> **Note:** In production environments, migrations are typically applied automatically at startup via `PreRunHook.cs` / `DatabaseInitializer`, not via CLI.

## Troubleshooting

| Issue | Fix |
|---|---|
| `No DbContext was found` | Ensure `DataContext` is registered in `DependencyInjection.cs` |
| `Unable to create object of type 'DataContext'` | Implement `IDesignTimeDbContextFactory<DataContext>` or ensure a valid connection string in `appsettings.Development.json` |
| Migration is empty | Ensure `DbSet<Dependent>` is added to `DataContext` and configuration is applied |
| FK not detected | Ensure `DependentConfiguration` sets up the relationship via Fluent API |
