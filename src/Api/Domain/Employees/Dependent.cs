using Api.SharedKernel.Domain;

namespace Api.Domain.Employees;

public class Dependent : Entity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Relationship { get; private set; } = null!;
    public int EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = null!;

    // EF Core requires a parameterless constructor
    private Dependent() { }

    public Dependent(string firstName, string lastName, string relationship, int employeeId)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(relationship))
            throw new ArgumentException("Relationship cannot be null or empty.", nameof(relationship));

        if (employeeId <= 0)
            throw new ArgumentException("EmployeeId must be a valid positive integer.", nameof(employeeId));

        FirstName = firstName;
        LastName = lastName;
        Relationship = relationship;
        EmployeeId = employeeId;
    }
}
