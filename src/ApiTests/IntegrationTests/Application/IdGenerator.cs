using System;

namespace ApiTests.IntegrationTests.Application;

public interface IIdGenerator
{
    Guid New();
}

public class IdGenerator : IIdGenerator
{
    public Guid New() => Guid.NewGuid();
}
