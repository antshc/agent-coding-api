## 1. Domain Layer

- [ ] 1.1 Create `Relationship` enum (`Spouse`, `DomesticPartner`, `Child`) in `Domain/Employees/`
- [ ] 1.2 Create `Dependent` entity in `Domain/Employees/Dependent.cs` inheriting `Entity`, with `FirstName`, `LastName`, `DateOfBirth`, `Relationship`, and an `EmployeeId` FK; enforce invariants in the constructor
- [ ] 1.3 Create `SingleSpouseOrDomesticPartnerRule` business rule in `Domain/Employees/` implementing `IBusinessRule`; the rule is broken when a new Spouse/DomesticPartner is added and one already exists in the collection
- [ ] 1.4 Add `Dependents` collection (`IReadOnlyCollection<Dependent>`) and `AddDependent(Dependent)` method to `Employee`; call `CheckRule(new SingleSpouseOrDomesticPartnerRule(...))` inside `AddDependent`
- [ ] 1.5 Add `AddDependentAsync(int employeeId, Dependent dependent, CancellationToken ct)` to `IEmployeeRepository`

## 2. Data Layer

- [ ] 2.1 Create `DependentConfiguration` in `Data/Configurations/` implementing `IEntityTypeConfiguration<Dependent>`; map to `Dependents` table, configure FK to `Employees`, store `Relationship` as string
- [ ] 2.2 Add `DbSet<Dependent>` to `DataContext` and register `DependentConfiguration` in `OnModelCreating`
- [ ] 2.3 Implement `AddDependentAsync` in `EmployeeRepository`; load the employee (return `null` if not found), call `employee.AddDependent(dependent)`, and save changes
- [ ] 2.4 Add EF Core migration for the `Dependents` table

## 3. Application Layer

- [ ] 3.1 Create `GetDependentDto` in `Application/Employees/Payload/` with `Id`, `FirstName`, `LastName`, `DateOfBirth`, `Relationship`
- [ ] 3.2 Create `CreateDependentDto` in `Application/Employees/Payload/` with `FirstName`, `LastName`, `DateOfBirth`, `Relationship`
- [ ] 3.3 Add `POST /api/v1/employees/{employeeId}/dependents` action to `EmployeesController`; return 404 if employee not found, 400 on `ArgumentException`, 201 Created with `GetDependentDto` on success

## 4. Tests

- [ ] 4.1 Add unit tests for `Dependent` constructor invariants (blank name, future DOB)
- [ ] 4.2 Add unit tests for `SingleSpouseOrDomesticPartnerRule` (no existing exclusive-dependent passes, second spouse fails, spouse + domestic partner fails)
- [ ] 4.3 Add integration test: successfully add a `Child` dependent (expect 201)
- [ ] 4.4 Add integration test: successfully add a `Spouse` dependent (expect 201)
- [ ] 4.5 Add integration test: add second Spouse is rejected (expect 400)
- [ ] 4.6 Add integration test: add `DomesticPartner` when `Spouse` already exists is rejected (expect 400)
- [ ] 4.7 Add integration test: add dependent to non-existent employee (expect 404)
