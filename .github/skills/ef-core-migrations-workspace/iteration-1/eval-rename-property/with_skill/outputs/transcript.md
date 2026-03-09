# Execution Transcript

## Task
Provide migration steps for renaming the `FullName` property to `DisplayName` on the `Employee` entity and its EF Core configuration.

## Steps Taken

### Step 1: Read Skill File
- Read `d:\_projects\agent-coding-api\.github\skills\ef-core-migrations\SKILL.md`
- Identified the 5-step workflow: Verify entity change  Build project  Add migration  Review migration  Apply migration
- Noted data loss protection rules: must warn if migration contains `DropColumn`, `DropTable`, or `RenameColumn`
- Identified migration naming convention for renames: `Rename<EntityName><OldName>To<NewName>`

### Step 2: Verify Entity Change
- Inspected `src/Api/Domain/Employees/`  folder is empty (no Employee entity present)
- Inspected `src/Api/Data/Configurations/`  only `UserConfiguration.cs` found (no EmployeeConfiguration)
- Inspected `src/Api/Data/DataContext.cs`  confirmed no `DbSet<Employee>` registered
- Inspected `src/Api/Data/Migrations/`  folder is empty
- **Finding:** The `Employee` entity and its EF Core configuration are not in the workspace yet. The user's described change cannot be verified against actual files, but the migration guidance applies correctly once the entity exists.

### Step 3: Build the Project
- Ran: `dotnet build src/Api/Api.csproj`
- **Result:** Build succeeded  `Api net10.0 succeeded (6.6s)`

### Step 4: Check Migration Status
- Ran: `dotnet ef migrations list --project src/Api/Api.csproj --startup-project src/Api/Api.csproj`
- **Result:** Command failed  SQL Server not accessible (no local instance running); this is expected in CI/local dev without Docker
- No existing migrations to list

### Step 5: Compose Response
- Applied skill naming convention: `RenameEmployeeFullNameToDisplayName`
- Documented the exact `dotnet ef migrations add` command with all required arguments
- Added data loss warning: EF Core may generate `DropColumn` + `AddColumn` instead of `RenameColumn` for property renames, which destroys all existing data
- Documented both remediation paths: manual migration edit, or remove + fix + regenerate
- Documented `dotnet ef database update` as the final apply step
- Included pre-apply checklist

## Commands Executed
1. `dotnet build src/Api/Api.csproj`  SUCCESS
2. `dotnet ef migrations list ...`  FAILED (no SQL Server, expected)

## Tool Calls
- Read: SKILL.md, Domain/Employees/ (empty), Data/Configurations/ (UserConfiguration.cs only), Data/DataContext.cs, Data/Migrations/ (empty)
- Run: dotnet build, dotnet ef migrations list

## Outcome
Full migration guidance written to `response.md` including the data loss warning for `RenameColumn` scenarios.
