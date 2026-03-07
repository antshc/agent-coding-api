## Context

The Benefits API is an ASP.NET Core 10 monolith following DDD layered architecture with a read/write separation. Currently, employees can only be retrieved via `GET /api/v1/employees`. There is no write path through the API; employees must be seeded directly via `DatabaseInitializer`. This change adds the first mutating endpoint: `POST /api/v1/employees`.

The `Employee` aggregate root enforces its own invariants (non-empty first/last name/salary/dateOfBirth) in its constructor. The domain repository interface lives in the Domain layer; its implementation lives in the Data layer.

## Goals / Non-Goals

**Goals:**
- Expose a `POST /api/v1/employees` endpoint that creates and persists a new employee.
- Keep write concerns (commands) separate from read concerns (queries) in accordance with the existing pattern.
- Leverage the existing `Employee` domain constructor for invariant enforcement.
- Return HTTP 201 Created with the new employee's details and a `Location` header.

**Non-Goals:**
- Authentication or authorization (out of scope for this change).
- Adding dependents or benefit information to the creation payload.
- Introducing a formal command/handler abstraction (overkill for a single mutation; a direct repository call from the controller is sufficient and consistent with the codebase's current complexity level).

## Decisions

### 1. Direct repository call from the controller (no command handler)

The existing read path uses a Query object (`IEmployeeQuery` / `EmployeeQuery`). A symmetrical command handler infrastructure could be added, but the codebase has only one mutation. Adding a full command bus or `IEmployeeCommand` abstraction for a single operation is premature. The controller will inject `IEmployeeRepository` directly for the write path.

_Alternative considered_: Introduce `ICreateEmployeeCommand` + implementation. Rejected: unnecessary indirection at this stage.

### 2. Request validation via domain constructor

The `Employee` constructor must throws `ArgumentException` for blank names. No external validation library is introduced. The controller catches `ArgumentException` and maps it to HTTP 400.

_Alternative considered_: DataAnnotations on `CreateEmployeeDto`. Rejected: invariants belong in the domain, not the DTO layer. Using `[ApiController]`'s model validation would duplicate logic.

### 3. Return `GetEmployeeDto` in the 201 response

Reusing the existing `GetEmployeeDto` keeps the response contract consistent with the GET endpoint and avoids introducing a new DTO type for the created entity.

### 4. `IEmployeeRepository.AddAsync` returns the created `Employee`

EF Core's `AddAsync` + `SaveChangesAsync` will populate the generated `Id`. Returning the entity allows the controller to construct the response DTO without a second round-trip.

## Risks / Trade-offs

- **Concurrent duplicate names**: No uniqueness constraint on `FirstName`+`LastName`. HR operators could create duplicate employee records. → Acceptable for now; uniqueness is a business policy decision deferred to a future change.
- **No transactional saga**: A single `SaveChangesAsync` call is atomic. No distributed transaction complexity.
- **Controller coupled to IEmployeeRepository**: Breaks the convention that controllers only depend on queries. Documented as an accepted deviation until a command abstraction is warranted.
