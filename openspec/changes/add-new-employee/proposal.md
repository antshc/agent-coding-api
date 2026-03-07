## Why

There is no way to create new employees through the API, requiring direct database manipulation to onboard new staff. Adding a create-employee endpoint closes this gap and enables automated HR workflows.

## What Changes

- Add a `POST /api/v1/employees` endpoint that accepts employee details and persists a new employee record.
- Introduce a request payload DTO (`CreateEmployeeDto`) with `FirstName`, `LastName`, `Salary`, `DateOfBirth` fields.
- Extend the `IEmployeeRepository` and `EmployeeRepository` with a `AddAsync` method.
- Return the newly created employee in the response with HTTP 201 Created.

## Capabilities

### New Capabilities

- `create-employee`: Allows callers to create a new employee by posting data to the Employees endpoint. Returns the created employee record with its generated ID.

### Modified Capabilities

<!-- No existing spec-level requirement changes. -->

## Impact

- **API**: New `POST /api/v1/employees` endpoint added to new`EmployeesController`.
- **Domain**: `IEmployeeRepository` gains a `AddAsync` method.
- **Data**: `EmployeeRepository` implements `AddAsync` using EF Core.
- **Tests**: New integration test for the create-employee endpoint; new unit tests for domain validation.
