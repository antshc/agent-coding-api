## ADDED Requirements

### Requirement: Create new employee
The system SHALL expose a `POST /api/v1/employees` endpoint that accepts a first name, last name, salary, date of birth and persists a new employee record, returning the created record with its generated identifier and HTTP 201 Created.

#### Scenario: Successful employee creation
- **WHEN** a caller sends `POST /api/v1/employees` with a valid `firstName`, `lastName`, `salary`, `dateOfBirth`
- **THEN** the system persists the new employee, returns HTTP 201 Created, a `Location` header pointing to the new resource, and the created employee data in the response body with `success: true`

### Requirement: Reject blank first name
The system SHALL return HTTP 400 Bad Request when the provided `firstName` is null, empty, or whitespace-only.

#### Scenario: Blank first name rejected
- **WHEN** a caller sends `POST /api/v1/employees` with a blank or missing `firstName`
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure

### Requirement: Reject blank last name
The system SHALL return HTTP 400 Bad Request when the provided `lastName` is null, empty, or whitespace-only.

#### Scenario: Blank last name rejected
- **WHEN** a caller sends `POST /api/v1/employees` with a blank or missing `lastName`
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure

### Requirement: Reject invalid salary
The system SHALL return HTTP 400 Bad Request when the provided `salary` is null/missing or less than zero.

#### Scenario: Missing salary rejected
- **WHEN** a caller sends `POST /api/v1/employees` without a `salary`
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure

#### Scenario: Negative salary rejected
- **WHEN** a caller sends `POST /api/v1/employees` with a `salary` less than `0`
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure

### Requirement: Reject invalid date of birth
The system SHALL return HTTP 400 Bad Request when the provided `dateOfBirth` is null/missing, is a future date, or represents an age greater than 120 years.

#### Scenario: Missing date of birth rejected
- **WHEN** a caller sends `POST /api/v1/employees` without a `dateOfBirth`
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure

#### Scenario: Future date of birth rejected
- **WHEN** a caller sends `POST /api/v1/employees` with a `dateOfBirth` that is in the future
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure

#### Scenario: Date of birth older than 120 years rejected
- **WHEN** a caller sends `POST /api/v1/employees` with a `dateOfBirth` more than 120 years in the past
- **THEN** the system returns HTTP 400 Bad Request with `success: false` and an error message describing the validation failure
