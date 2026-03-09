---
name: ef-core-migrations
description: >
  Triggers when database schema changes are required in an ASP.NET Core project using Entity Framework Core.
  Use this skill when:
  - A property is added, removed, or renamed in an entity class
  - A navigation property or foreign key is modified
  - A DbContext model configuration is changed
  - IEntityTypeConfiguration or Fluent API mapping is updated
  - Repository changes require a schema update

  Do NOT use this skill when:
  - Only LINQ queries changed
  - DTOs changed
  - Service logic changed
  - API controllers changed
  - Tests were updated
---

# EF Core Migrations

## Arguments

All `dotnet ef` commands use the following arguments. Read their values from the `ef-core-migrations` section in `.github/copilot-instructions.md`:

| Placeholder | Description |
|---|---|
| `{project}` | Path to the project `.csproj` file |
| `{startupProject}` | Path to the startup project `.csproj` file |
| `{migrationsOutput}` | Output folder for migration files |

---
## Preconditions

Before running migrations verify:

- The project uses Entity Framework Core
- A DbContext class exists
- The project builds successfully
- The EF CLI tool is installed


## Workflow

Whenever a domain entity is modified (property added, removed, or renamed), follow this sequence:

1. Verify entity change
2. Build project
3. Add migration
4. Review migration
5. Apply migration

---

## Verify entity configuration

- Decimal Precision Rule: When creating or updating Entity Framework Core entity configurations, all decimal properties must explicitly define precision using .HasPrecision(18, 2) in the Fluent API. Do not use .HasColumnType("decimal(18,2)")

--- 

## Review generated migration

After generating a migration, review:

- Column names
- Nullability
- Indexes
- Foreign keys
- Data loss operations

---

### Data Loss Protection

If a migration contains operations like:

- DropColumn
- DropTable
- RenameColumn

the agent must confirm the operation is intended.

---

## Migration naming conventions

| Change type | Example migration name |
|---|---|
| Add property | `Add<EntityName><PropertyName>` |
| Remove property | `Remove<EntityName><PropertyName>` |
| Rename property | `Rename<EntityName><OldName>To<NewName>` |
| Add entity/table | `Add<EntityName>` |
| Add relationship | `Add<EntityName><RelatedEntity>Relation` |
| Initial schema | `InitialMigration` |

---

## Commands
### Build the project

```bash
dotnet build {project} --configuration Debug -v q 2>&1 | Select-Object -Last <the number of lines in the build output that contain warnings or errors>
```

### Add a migration

Run after every entity or DbContext change that affects the schema:

```bash
dotnet ef migrations add <MigrationName> --project {project} --startup-project {startupProject} -o {migrationsOutput}
```

Use a descriptive `<MigrationName>` that reflects what changed, e.g. `AddUserEmailColumn`, `RenameProductTitle`, `RemoveOrderNotes`.

### Remove the last migration

Before removing, verify the most recent migration has **not** been applied to the database. List migrations and check their status — any migration marked with `(Pending)` has not been applied:

```bash
dotnet ef migrations list --project {project} --startup-project {startupProject}
```

If the last migration is pending, it is safe to remove:

```bash
dotnet ef migrations remove --project {project} --startup-project {startupProject}
```

> **Warning:** DO NOT REMOVE a migration that has already been applied to the database. Revert the database first with `dotnet ef database update <PreviousMigrationName>`, then remove.

### Apply migrations to the database

```bash
dotnet ef database update --project {project}
```

### Rollback to a previous migration

To revert the database to a specific earlier migration, pass the target migration name to `database update`:

```bash
dotnet ef database update <PreviousMigrationName> --project {project} --startup-project {startupProject}
```

This downgrades the database schema to the state at `<PreviousMigrationName>`. Any migrations applied after that point will be reverted. After rolling back, you can safely remove the unwanted migration(s) using the remove command above.

---

## Troubleshooting

### `dotnet ef` command not found

Install the EF Core global tool:

```bash
dotnet tool install --global dotnet-ef
```

### `Microsoft.EntityFrameworkCore.Design` not installed

Add the required design-time package to the project:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### SQL Server not available locally

Run a SQL Server Express instance in Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -e "MSSQL_PID=Express" -p 14330:1433 -d mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04
```

Update the connection string in your app configuration to use `localhost,14330`.

---
