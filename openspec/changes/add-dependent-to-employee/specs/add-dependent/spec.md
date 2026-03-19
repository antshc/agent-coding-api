## ADDED Requirements

### Requirement: Add dependent to employee
The system SHALL allow callers to add a dependent to an existing employee by posting to `POST /api/v1/employees/{employeeId}/dependents`. The dependent MUST have a `FirstName`, `LastName`, `DateOfBirth`, and `Relationship`. Valid relationship values are `Spouse`, `DomesticPartner`, and `Child`.

#### Scenario: Successfully add a child dependent
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with valid dependent data and `Relationship` = `Child`
- **THEN** the system returns HTTP 201 Created with the created dependent's `Id`, `FirstName`, `LastName`, `DateOfBirth`, and `Relationship`

#### Scenario: Successfully add a spouse dependent
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with valid dependent data and `Relationship` = `Spouse` and the employee has no existing Spouse or DomesticPartner
- **THEN** the system returns HTTP 201 Created with the created dependent record

#### Scenario: Successfully add a domestic partner dependent
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with valid dependent data and `Relationship` = `DomesticPartner` and the employee has no existing Spouse or DomesticPartner
- **THEN** the system returns HTTP 201 Created with the created dependent record

### Requirement: Employee not found returns 404
The system SHALL return HTTP 404 Not Found when a dependent is added to an employee that does not exist.

#### Scenario: Employee does not exist
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with a non-existent `employeeId`
- **THEN** the system returns HTTP 404 Not Found

### Requirement: Enforce single Spouse or DomesticPartner per employee
The system SHALL enforce that each employee has at most one Spouse or DomesticPartner. Attempting to add a second Spouse or DomesticPartner to an employee that already has one SHALL be rejected.

#### Scenario: Add second spouse is rejected
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with `Relationship` = `Spouse` and the employee already has a Spouse
- **THEN** the system returns HTTP 400 Bad Request with an error message indicating the constraint

#### Scenario: Add domestic partner when spouse exists is rejected
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with `Relationship` = `DomesticPartner` and the employee already has a Spouse
- **THEN** the system returns HTTP 400 Bad Request with an error message indicating the constraint

#### Scenario: Add spouse when domestic partner exists is rejected
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with `Relationship` = `Spouse` and the employee already has a DomesticPartner
- **THEN** the system returns HTTP 400 Bad Request with an error message indicating the constraint

### Requirement: Dependent fields are required and validated
The system SHALL reject requests that have blank `FirstName`, blank `LastName`, a `DateOfBirth` in the future, or a `DateOfBirth` more than 120 years in the past.

#### Scenario: Blank first name is rejected
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with an empty or whitespace-only `FirstName`
- **THEN** the system returns HTTP 400 Bad Request

#### Scenario: Blank last name is rejected
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with an empty or whitespace-only `LastName`
- **THEN** the system returns HTTP 400 Bad Request

#### Scenario: Future date of birth is rejected
- **WHEN** a `POST /api/v1/employees/{employeeId}/dependents` request is made with a `DateOfBirth` in the future
- **THEN** the system returns HTTP 400 Bad Request
