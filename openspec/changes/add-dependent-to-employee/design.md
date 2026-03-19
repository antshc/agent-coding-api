## Context

The Benefits API manages employee payroll and benefit deductions on a biweekly pay cycle (26 paychecks/year). Dependents (Spouse, DomesticPartner, Children) affect the benefit cost deducted from an employee's paycheck. Currently, the `Employee` aggregate has no concept of dependents; they cannot be associated through the API at all.

The codebase follows DDD layered architecture with a read/write separation. The `Employee` aggregate root is the natural owner of the `Dependent` collection since benefit deductions are calculated per employee including their dependents.

## Goals / Non-Goals

**Goals:**
- Model `Dependent` as a child entity owned by the `Employee` aggregate.
- Expose `POST /api/v1/employees/{employeeId}/dependents` to add a dependent to an existing employee.
- Enforce the business rule: an employee may have at most **one Spouse or DomesticPartner** (but any number of Children).
- Return HTTP 201 Created with the new dependent record.
- Return HTTP 404 if the employee does not exist.
- Return HTTP 400 if the relationship constraint is violated.

**Non-Goals:**
- Removing or updating dependents (deferred to a future change).
- Listing dependents via a dedicated endpoint (deferred; dependents may be included in a future GET employee detail endpoint).
- Authentication or authorization.
- Recalculating paycheck deductions in this change (dependents must exist before deduction logic can reference them).

## Decisions

### 1. `Dependent` as a domain entity owned by `Employee` aggregate

`Dependent` has no independent lifecycle outside of an `Employee`. It is a child entity (not an aggregate root), owned by `Employee`. The `Employee` aggregate exposes an `AddDependent(Dependent)` method that enforces the single-Spouse/DomesticPartner invariant.

_Alternative considered_: Separate `Dependent` aggregate root with its own repository. Rejected: dependents have no independent existence and the invariant (one spouse/partner per employee) must be enforced atomically at the aggregate boundary.

### 2. Direct repository call from controller (no command handler)

Consistent with the existing pattern established by `POST /api/v1/employees`. The controller injects `IEmployeeRepository` directly for the write path and constructs the domain objects inline.

_Alternative considered_: Introduce a command/handler abstraction. Rejected: premature at this codebase's current complexity level.

### 3. Relationship as an enum (`Relationship`: Spouse, DomesticPartner, Child)

A fixed enum is appropriate because the supported relationship types are defined by the benefits policy and are not open-ended. The string representation is stored in the database.

_Alternative considered_: Free-text string. Rejected: loses type safety and makes invariant enforcement harder.

### 4. Business rule enforced in `Employee.AddDependent`

The constraint "at most one Spouse or DomesticPartner" is a domain invariant. It is enforced in `Employee.AddDependent` using the `CheckRule` / `IBusinessRule` pattern already present in `SharedKernel`.

### 5. `IEmployeeRepository` extended with `AddDependentAsync`

Rather than loading the employee and then re-saving via a generic update method, adding a dedicated `AddDependentAsync(employeeId, dependent, ct)` keeps the repository contract explicit and avoids accidental aggregate replacement.

_Alternative considered_: Generic `UpdateAsync` method. Rejected: too broad; makes it easy to accidentally overwrite unrelated employee data.

### 6. EF Core owned-entity vs. separate table

`Dependent` will be mapped to its own table (`Dependents`) with a foreign key to `Employees`. Using a separate table (rather than EF Core owned-entity table splitting) gives clean querying and avoids EF Core owned-entity limitations with collections.

## Risks / Trade-offs

- **Concurrent add-dependent race**: Two concurrent requests could both pass the "one spouse" check before either commits. → Acceptable at current scale; a unique constraint on `(EmployeeId, Relationship)` filtered to Spouse/DomesticPartner can be added as a follow-up if needed.
- **Controller coupled to IEmployeeRepository**: Consistent accepted deviation from the pure query-only controller pattern, same as the create-employee change.
- **No listing endpoint yet**: Dependents are persisted but not yet surfaced via GET. This is intentional scope management; a follow-up change will include them in employee detail responses.
