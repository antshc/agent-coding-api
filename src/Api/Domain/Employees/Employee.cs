using Api.SharedKernel.Domain;

namespace Api.Domain.Employees;

public class Employee : Entity, IAggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public decimal Salary { get; private set; }
    public DateOnly DateOfBirth { get; private set; }

    // EF Core requires a parameterless constructor
    private Employee()
    {
    }

    public Employee(string firstName, string lastName, decimal salary, DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be blank.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be blank.", nameof(lastName));

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.", nameof(salary));

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (dateOfBirth > today)
            throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

        if (dateOfBirth < today.AddYears(-120))
            throw new ArgumentException("Date of birth cannot be more than 120 years ago.", nameof(dateOfBirth));

        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        DateOfBirth = dateOfBirth;
    }
}
