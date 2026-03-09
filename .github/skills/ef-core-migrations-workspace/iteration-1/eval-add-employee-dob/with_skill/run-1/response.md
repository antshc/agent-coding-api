# EF Core Migration: Add Employee DateOfBirth

## Overview

The `DateOfBirth` property (`DateOnly`) was added to the `Employee` entity. A migration named `AddEmployeeDateOfBirth` already exists in `Data/Migrations` that covers this change. The migration was reviewed, a formatting issue was fixed, and the project was confirmed to build cleanly.

---

## Step 1  Verify Entity Change

The `Employee` entity at `src/Api/Domain/Employees/Employee.cs` already has the `DateOfBirth` property:

```csharp
public DateOnly DateOfBirth { get; private set; }
```

The EF Core configuration in `src/Api/Data/Configurations/EmployeeConfiguration.cs` maps it as required:

```csharp
builder.Property(e => e.DateOfBirth)
    .IsRequired();
```

---

## Step 2  Build the Project

```bash
dotnet build src/Api/Api.csproj
```

**Result:** Build succeeded after fixing a formatting issue (IDE0055) in the existing migration file caused by incorrect class indentation under a file-scoped namespace.

---

## Step 3  Migration Already Exists

The migration `AddEmployeeDateOfBirth` was already generated at `src/Api/Data/Migrations/20260309095939_AddEmployeeDateOfBirth.cs`. It correctly includes the `DateOfBirth` column in the `Employees` table:

```csharp
DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false)
```

> **Note:** No new migration was needed. If you were generating the migration fresh, the command would be:
> ```bash
> dotnet ef migrations add AddEmployeeDateOfBirth \
>   --project src/Api/Api.csproj \
>   --startup-project src/Api/Api.csproj \
>   -o Data/Migrations
> ```

---

## Step 4  Review Migration

Migration reviewed for:

| Check | Result |
|---|---|
| Column name | `DateOfBirth`  |
| Column type | `date` (maps to `DateOnly`)  |
| Nullability | `nullable: false` (required)  |
| Indexes | None needed  |
| Foreign keys | Unaffected  |
| Data loss operations | None  |

No destructive operations (`DropColumn`, `DropTable`, `RenameColumn`)  safe to apply.

---

## Step 5  Apply Migration

```bash
dotnet ef database update --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

This applies all pending migrations to the database, including `AddEmployeeDateOfBirth`.

To verify after applying:
```bash
dotnet ef migrations list --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

The migration should appear **without** `(Pending)` next to its name.
