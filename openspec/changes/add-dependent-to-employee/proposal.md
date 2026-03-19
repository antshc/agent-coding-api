## Why

Employees can have dependents (spouse, domestic partner, children) whose benefit costs are deducted from their paycheck. Currently, there is no way to associate dependents with an employee through the API, making it impossible to accurately calculate benefit deductions.

## What Changes

- Add a `POST /api/v1/employees/{employeeId}/dependents` endpoint to add a dependent to an employee.
- Introduce a `Dependent` domain entity with `FirstName`, `LastName`, `DateOfBirth`, and `Relationship` (Spouse, DomesticPartner, Child).
- Enforce the business rule that an employee may have at most one Spouse or DomesticPartner.
- Extend `IEmployeeRepository` with a method to add a dependent to an existing employee.
- Return the newly created dependent in the response with HTTP 201 Created.

## Capabilities

### New Capabilities

- `add-dependent`: Allows callers to add a dependent to an existing employee by posting dependent details to the Employees endpoint. Enforces relationship constraints (only one Spouse or DomesticPartner per employee). Returns the created dependent record with its generated ID.

### Modified Capabilities

<!-- No existing spec-level requirement changes. -->

## Impact

- **API**: New `POST /api/v1/employees/{employeeId}/dependents` endpoint added to `EmployeesController`.
- **Domain**: New `Dependent` entity and business rule for relationship uniqueness; `Employee` aggregate gains a `Dependents` collection and an `AddDependent` method.
- **Data**: New `DependentConfiguration` for EF Core mapping; `EmployeeRepository` extended to persist dependents; migration required.
- **Tests**: New integration tests for the add-dependent endpoint; new unit tests for domain invariants.
