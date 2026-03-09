# EF Core Migration: Add DateOfBirth to Employee

## Overview

You've added a `DateOfBirth` property to the `Employee` entity. The following steps create and apply the required EF Core migration.

## Prerequisites

Ensure the EF Core tools are installed:

```bash
dotnet tool restore
```

Or install globally if needed:

```bash
dotnet tool install --global dotnet-ef
```

## Step 1: Add the Migration

Run from the **solution root** (`src/`):

```bash
dotnet ef migrations add AddEmployeeDateOfBirth \
  --project src/Api/Api.csproj \
  --startup-project src/Api/Api.csproj \
  --output-dir Data/Migrations
```

On Windows (PowerShell), the backslash continuation is not needed  use a single line:

```powershell
dotnet ef migrations add AddEmployeeDateOfBirth --project src/Api/Api.csproj --startup-project src/Api/Api.csproj --output-dir Data/Migrations
```

This generates 3 files under `src/Api/Data/Migrations/`:
- `<timestamp>_AddEmployeeDateOfBirth.cs`  the migration class with `Up` and `Down` methods
- `<timestamp>_AddEmployeeDateOfBirth.Designer.cs`  EF Core snapshot metadata
- `DataContextModelSnapshot.cs`  updated model snapshot

## Step 2: Review the Migration

Open the generated migration file and verify the `Up` method contains something like:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<DateTime>(
        name: "DateOfBirth",
        table: "Employees",
        type: "datetime2",
        nullable: false,
        defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
}
```

And `Down` reverts it:

```csharp
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "DateOfBirth",
        table: "Employees");
}
```

> **Note:** If `DateOfBirth` is nullable (`DateTime?`), the generated column will be `nullable: true` with no `defaultValue`. Adjust accordingly if the property type differs.

## Step 3: Apply the Migration

```powershell
dotnet ef database update --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

This runs the migration against the database configured in `appsettings.Development.json` (or whichever environment is active via `ASPNETCORE_ENVIRONMENT`).

## Verification

Check the `__EFMigrationsHistory` table in your SQL Server database to confirm the migration was applied:

```sql
SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId DESC;
```

You should see a row like:
```
20260309XXXXXX_AddEmployeeDateOfBirth | <EF Core version>
```

## Rollback (if needed)

To revert to the previous migration:

```powershell
dotnet ef database update <PreviousMigrationName> --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

Then remove the migration file:

```powershell
dotnet ef migrations remove --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```
