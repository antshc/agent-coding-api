## 1. Domain Layer

- [ ] 1.1 Create `IEmployeeRepository`, add `AddAsync(Employee employee, CancellationToken cancellationToken)` method to `IEmployeeRepository`

## 2. Data Layer

- [ ] 2.1 Implement `AddAsync` in `EmployeeRepository` (EF Core `AddAsync` + `SaveChangesAsync`, return created entity)

## 3. Application Layer

- [ ] 3.1 Create `CreateEmployeeDto` request payload in `Application/Employees/Payload/` with `FirstName`, `LastName`, `Salary`, `DateOfBirth` properties
- [ ] 3.2 Add `POST` action to `EmployeesController` that accepts `CreateEmployeeDto`, calls `IEmployeeRepository.AddAsync`, and returns HTTP 201 Created with `GetEmployeeDto` response and `Location` header
- [ ] 3.3 Register `IEmployeeRepository` in `DependencyInjection.cs` if not already registered (verify it's accessible from the controller)

## 4. Tests

- [ ] 4.1 Add integration test in `EmployeeIntegrationTests` for successful employee creation (POST returns 201 and correct body)
- [ ] 4.2 Add integration test for blank `firstName` returns 400
- [ ] 4.3 Add integration test for blank `lastName` returns 400
- [ ] 4.4 Add integration test for blank `salary` returns 400
- [ ] 4.5 Add integration test for blank `dateOfBirth` returns 400
- [ ] 4.6 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for blank first name
- [ ] 4.7 Add unit test in `EmployeeUnitTests` verifying `Employee` constructor throws for blank last name
