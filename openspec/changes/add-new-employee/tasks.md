## 1. Domain Layer

- [x] 1.1 Create `IEmployeeRepository`, add `AddAsync(Employee employee, CancellationToken cancellationToken)` method to `IEmployeeRepository`

## 2. Data Layer

- [x] 2.1 Implement `AddAsync` in `EmployeeRepository` (EF Core `AddAsync` + `SaveChangesAsync`, return created entity)

## 3. Application Layer

- [x] 3.1 Create `CreateEmployeeDto` request payload in `Application/Employees/Payload/` with `FirstName`, `LastName`, `Salary`, `DateOfBirth` properties
- [x] 3.2 Add `POST` action to `EmployeesController` that accepts `CreateEmployeeDto`, calls `IEmployeeRepository.AddAsync`, and returns HTTP 201 Created with `GetEmployeeDto` response and `Location` header
- [x] 3.3 Register `IEmployeeRepository` in `DependencyInjection.cs` if not already registered (verify it's accessible from the controller)

## 4. Tests

- [x] 4.1 Create integration test for `Employee`, use create-employee spec.md for test scenarios. verify all scenrios implemented and pass.
- [x] 4.2 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for blank first name
- [x] 4.3 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for blank last name
- [x] 4.4 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for negative salary
- [x] 4.5 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for dateOfBirth in the future
- [x] 4.6 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for dateOfBirth more than 120 years ago
