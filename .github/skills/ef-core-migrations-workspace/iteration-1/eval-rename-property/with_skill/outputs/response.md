# EF Core Migration Steps: Rename `FullName` to `DisplayName` on `Employee`

## 1. Verify Entity Change

The change involves renaming the `FullName` property to `DisplayName` on the `Employee` domain entity and updating the corresponding EF Core `IEntityTypeConfiguration<Employee>` (column mapping in Fluent API). Both changes are required  the C# property name and the EF configuration must be consistent.

> **Note:** In the current workspace, the `Domain/Employees/` folder is empty and no `EmployeeConfiguration.cs` exists in `Data/Configurations/`. If the entity has already been modified, confirm both files reflect `DisplayName` before proceeding.

---

## 2. Build the Project

Before generating any migration, the project must compile cleanly:

```bash
dotnet build src/Api/Api.csproj
```

**Observed result:** Build succeeded  `Api net10.0 succeeded`.

---

## 3. Add the Migration

Using the naming convention `Rename<EntityName><OldName>To<NewName>`, the migration name is **`RenameEmployeeFullNameToDisplayName`**.

```bash
dotnet ef migrations add RenameEmployeeFullNameToDisplayName `
  --project src/Api/Api.csproj `
  --startup-project src/Api/Api.csproj `
  -o Data/Migrations
```

This generates a migration file under `src/Api/Data/Migrations/`.

---

## 4. Review the Generated Migration  WARNING: Data Loss Risk

Open the generated `*_RenameEmployeeFullNameToDisplayName.cs` file and inspect the `Up()` method.

### Safe output (what you want to see):

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "FullName",
        table: "Employees",
        newName: "DisplayName");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "DisplayName",
        table: "Employees",
        newName: "FullName");
}
```

### Dangerous output  do NOT apply this:

```csharp
// BAD: All existing data in FullName will be permanently lost
migrationBuilder.DropColumn(name: "FullName", table: "Employees");
migrationBuilder.AddColumn<string>(name: "DisplayName", ...);
```

EF Core may generate a `DropColumn` + `AddColumn` pair if it cannot detect the rename was intentional. This **destroys all existing column data**.

**If you see DropColumn + AddColumn instead of RenameColumn:**

**Option A  Edit the migration manually (recommended):**
Replace the drop/add pair in `Up()` with:
```csharp
migrationBuilder.RenameColumn(
    name: "FullName",
    table: "Employees",
    newName: "DisplayName");
```
Update `Down()` to reverse the rename.

**Option B  Remove and re-check your changes:**
Verify both the entity property and the EF configuration are consistently updated, then remove and regenerate:
```bash
dotnet ef migrations remove `
  --project src/Api/Api.csproj `
  --startup-project src/Api/Api.csproj
```

---

## 5. Apply the Migration

Once the migration content is verified to use `RenameColumn` (not `DropColumn`):

```bash
dotnet ef database update --project src/Api/Api.csproj
```

---

## All Commands at a Glance

```bash
# 1. Build
dotnet build src/Api/Api.csproj

# 2. Add migration
dotnet ef migrations add RenameEmployeeFullNameToDisplayName \
  --project src/Api/Api.csproj \
  --startup-project src/Api/Api.csproj \
  -o Data/Migrations

# 3. REVIEW the generated migration BEFORE applying:
#    Confirm Up() uses RenameColumn, NOT DropColumn + AddColumn

# 4. Apply
dotnet ef database update --project src/Api/Api.csproj
```

---

## Pre-Apply Checklist

- [ ] Project builds with no errors
- [ ] Generated migration uses `RenameColumn`, not `DropColumn` + `AddColumn`
- [ ] `Down()` method correctly reverses the rename back to `FullName`
- [ ] Verified in a non-production environment first
