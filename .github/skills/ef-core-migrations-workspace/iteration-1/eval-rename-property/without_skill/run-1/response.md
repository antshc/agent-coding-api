# EF Core Migration Steps: Renaming `FullName` to `DisplayName` on Employee

## Background

When you rename a C# property in an entity class and update its EF Core configuration, the migration scaffolder **does not automatically detect it as a rename**. Instead, it treats the change as:

- **Drop** the old column (`FullName`)
- **Add** the new column (`DisplayName`)

This would cause **data loss** in production. You must manually edit the generated migration to use `RenameColumn` instead.

---

## Step 1: Scaffold the Migration

Run the following command from the solution root:

```powershell
dotnet ef migrations add RenameFullNameToDisplayName `
  --project src/Api/Api.csproj `
  --startup-project src/Api/Api.csproj `
  --output-dir Data/Migrations
```

Or as a single line:

```powershell
dotnet ef migrations add RenameFullNameToDisplayName --project src/Api/Api.csproj --startup-project src/Api/Api.csproj --output-dir Data/Migrations
```

This creates two files in `src/Api/Data/Migrations/`:
- `{timestamp}_RenameFullNameToDisplayName.cs`
- `{timestamp}_RenameFullNameToDisplayName.Designer.cs`

---

## Step 2: Edit the Generated Migration File

Open `src/Api/Data/Migrations/{timestamp}_RenameFullNameToDisplayName.cs`.

The scaffolder will have generated something like this (incorrect for a rename):

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "FullName",
        table: "Employees");

    migrationBuilder.AddColumn<string>(
        name: "DisplayName",
        table: "Employees",
        type: "nvarchar(max)",
        nullable: false,
        defaultValue: "");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "DisplayName",
        table: "Employees");

    migrationBuilder.AddColumn<string>(
        name: "FullName",
        table: "Employees",
        type: "nvarchar(max)",
        nullable: false,
        defaultValue: "");
}
```

**Replace both methods** with the following, which uses `RenameColumn` to preserve existing data:

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

> **Why this matters:** `RenameColumn` issues an `sp_rename` (SQL Server) or `ALTER TABLE ... RENAME COLUMN` (other providers) under the hood, which is a metadata-only operation  no data is moved or lost. The `Down` method correctly reverses the rename for rollbacks.

---

## Step 3: Apply the Migration

```powershell
dotnet ef database update --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

This applies the pending migration to the database. EF Core will execute the `RenameColumn` operation.

---

## Step 4: Verify

Confirm the migration was applied successfully:

```powershell
dotnet ef migrations list --project src/Api/Api.csproj --startup-project src/Api/Api.csproj
```

The output should show `RenameFullNameToDisplayName` with an `[applied]` marker (or no marker indicating it is the latest applied migration).

You can also verify at the database level by inspecting the `Employees` table schema to confirm the column is now named `DisplayName`.

---

## Summary of Commands

| Step | Command |
|------|---------|
| Scaffold migration | `dotnet ef migrations add RenameFullNameToDisplayName --project src/Api/Api.csproj --startup-project src/Api/Api.csproj --output-dir Data/Migrations` |
| Edit migration | Replace `DropColumn`/`AddColumn` with `RenameColumn` in generated `.cs` file |
| Apply migration | `dotnet ef database update --project src/Api/Api.csproj --startup-project src/Api/Api.csproj` |
| Verify | `dotnet ef migrations list --project src/Api/Api.csproj --startup-project src/Api/Api.csproj` |

---

## Key Principle

> **EF Core cannot infer renames.** Any property rename must be manually corrected in the scaffolded migration to use `RenameColumn`. Failing to do so results in a destructive drop-and-add that deletes all existing column data.
