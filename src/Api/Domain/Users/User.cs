using Api.SharedKernel.Domain;

namespace Api.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    // EF Core requires a parameterless constructor
    private User()
    {
    }

    public User(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }
}
