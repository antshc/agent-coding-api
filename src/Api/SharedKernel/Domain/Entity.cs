namespace Api.SharedKernel.Domain;

// why? Base Entity class provides identity handling, equality logic,
// and a place for shared generic logic across all domain entities in a DDD model.
// It can also be used to track domain events
public abstract class Entity
{
    public int Id { get; private set; }

    protected void CheckRule(IBusinessRule rule)
    {
        rule.Check();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}