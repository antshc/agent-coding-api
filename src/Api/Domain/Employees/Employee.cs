using Api.SharedKernel.Domain;

namespace Api.Domain.Employees;

public class Employee : Entity, IAggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public decimal Salary { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    private readonly List<Dependent> _dependents = [];
    public IReadOnlyCollection<Dependent> Dependents => _dependents.AsReadOnly();
    // EF Core requires a parameterless constructor
    private Employee()
    {
    }

    public Employee(string firstName, string lastName, decimal salary, DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.", nameof(salary));

        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        DateOfBirth = dateOfBirth;
    }
}
