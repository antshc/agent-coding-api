---
name: ef-core-migrations
description: "Applies when a domain entity class is updated — properties are added, removed, or renamed — or when repository changes require a database schema update. Triggers include: adding columns to an entity, removing properties from a domain model, renaming entity properties, changing property types, or updating relationships (navigation properties, foreign keys). Also applies after modifying DbContext configurations, entity configurations, or IRepository implementations that affect the schema. Not applicable for data seeding, query optimization, or changes that don't affect the database schema."
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

## Workflow

Whenever a domain entity is modified (property added, removed, or renamed), follow this sequence:

1. Verify the code change to the entity and repository.
2. Add a new migration to capture the schema change.
3. Apply the migration to the database.

---

## Commands

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

## Migration naming conventions

| Change type | Example migration name |
|---|---|
| Add property | `Add<EntityName><PropertyName>` |
| Remove property | `Remove<EntityName><PropertyName>` |
| Rename property | `Rename<EntityName><OldName>To<NewName>` |
| Add entity/table | `Add<EntityName>` |
| Add relationship | `Add<EntityName><RelatedEntity>Relation` |
| Initial schema | `InitialMigration` |
