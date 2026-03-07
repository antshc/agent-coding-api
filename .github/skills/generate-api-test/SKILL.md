---
name: generate-api-test
description: >
  Generate LightBDD-based API integration tests for ASP.NET Core endpoints in this codebase.
  Use this skill whenever the user asks to generate, write, create, or add integration tests for an API endpoint,
  controller, feature, or scenario. Also use it when the user mentions "BDD test", "feature test", "LightBDD",
  "scenario test", "Given/When/Then", or "acceptance test". Applies to any endpoint in the Controllers or Minimal API.
  After generating the test files, always run dotnet test to verify and fix failures using ReAct loops.
---

# Generate Integration Tests — LightBDD / ASP.NET Core

This skill generates fully working LightBDD-based API integration tests that follow the exact patterns used in this codebase. It uses the **ReAct loop** (Reason → Act → Observe → Repeat) to produce and verify tests.

---

## Mental Model: How Tests Are Structured

Every integration test lives across **three layers**:

| Layer | File | Role |
|---|---|---|
| Client | `IntegrationTests/Application/Clients/I{Feature}Client.cs` | RestEase typed HTTP client |
| Steps | `IntegrationTests/Features/{Feature_name}_steps.cs` | Given/When/Then step implementations |
| Feature | `IntegrationTests/Features/{Feature_name}.cs` | Scenario declarations (`[Scenario]` methods) |

Plus **two registration points**:
- `TestWebApplicationFactory.cs` — expose new client as a property
- `ConfiguredLightBddScopeAttribute.cs` — register the steps class in LightBDD DI

---

## ReAct Loop

Apply this loop rigorously. Do not skip steps.

### REASON — Before writing any code

Answer these questions by reading the relevant files:

1. **What is the endpoint?**
   - Route, HTTP method, request body type, response type
   - Read the controller file to confirm the route pattern and return types

2. **What is the domain?**
   - What feature folder does it belong to? (e.g., `Employees/`, `Users/`)
   - What is the DTO naming convention?

3. **What scenarios exist?**
   - Happy path (e.g., create succeeds → 201)
   - Error paths (e.g., not found → 404, validation failure → 400)
   - Any ID injection needed? (use `IdGeneratorMock` pattern)

4. **What test data exists?**
   - Is there a `Test{Entity}` static class in `Application/`? Create one if not present.
   - Are there existing clients in `Application/Clients/`? Extend rather than duplicate.

5. **What already exists?**
   - Check `ConfiguredLightBddScopeAttribute.ConfigureDI` for existing step registrations
   - Check `TestWebApplicationFactory` for existing clients
   - Check `Features/` for existing feature/step files

### ACT — Generate the files

Follow the patterns below and create files in order:

1. Client interface
2. Steps class
3. Feature class
4. Register in `TestWebApplicationFactory` (if new client)
5. Register steps class in `ConfiguredLightBddScopeAttribute`

### OBSERVE — Run the tests

```
dotnet test src/ApiTests/ApiTests.csproj --no-build -v minimal
```

If you just created and haven't built yet:
```
dotnet test src/ApiTests/ApiTests.csproj -v minimal
```

Read the output carefully. Look for:
- Build errors → fix compilation issues
- Test failures → read the assertion message, fix steps or assertions
- DI resolution errors → check `ConfigureDI` registrations

### REPEAT — Fix and re-run

If tests fail, reason about the failure, make targeted fixes, and re-run. Repeat until all new tests pass. Do not stop at the first green run — confirm the test count matches what you created.

---

## File Templates

### 1 — Client Interface

Location: `src/ApiTests/IntegrationTests/Application/Clients/I{Feature}Client.cs`

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Application.{Feature}.Payload;
using RestEase;

namespace ApiTests.IntegrationTests.Application.Clients;

[BasePath("{route-prefix}")]      // e.g., "employees"
[AllowAnyStatusCode]              // prevents RestEase from throwing on non-2xx
public interface I{Feature}Client
{
    [Post()]
    Task<HttpResponseMessage> Create{Entity}([Body] Create{Entity}Dto request);

    [Get("{id}")]
    Task<HttpResponseMessage> Get([Path] Guid id);

    [Get("")]
    Task<HttpResponseMessage> GetAll();

    [Delete("{id}")]
    Task<HttpResponseMessage> Delete([Path] Guid id);
}
```

Key rules:
- `[AllowAnyStatusCode]` is mandatory — prevents RestEase exceptions on 404/400/etc.
- Return type is always `Task<HttpResponseMessage>` — assertions happen in steps
- Use `[Body]` for POST/PUT bodies, `[Path]` for route params, `[Query]` for query strings

### 2 — Steps Class

Location: `src/ApiTests/IntegrationTests/Features/{Feature_name}_steps.cs`

Naming: Use `Snake_case` with spaces replaced by underscores. LightBDD extracts step names from method names.

```csharp
using System;
using System.Threading.Tasks;
using Api.Application.{Feature}.Payload;
using LightBDD.Framework;
using ApiTests.IntegrationTests.Application.Clients;
using ApiTests.IntegrationTests.Application;
using Features.Common;

namespace Features;

internal class {Feature_name}_steps : Base_api_steps, IDisposable
{
    private readonly I{Feature}Client _client;
    private State<Guid> _entityId;

    public {Feature_name}_steps(TestWebApplicationFactory app)
    {
        _client = app.{Feature}Client;
        App = app;
    }

    public TestWebApplicationFactory App { get; }

    // GIVEN — set up preconditions
    public Task Given_no_{entity}_exists_with_id(Guid id)
    {
        _entityId = id;

        App.IdGeneratorMock.Setup(m => m.New())
            .Returns(id);

        return Task.CompletedTask;
    }

    // WHEN — perform the HTTP action
    public async Task When_{action}_request_is_sent(string fieldA, string fieldB)
    {
        var request = new Create{Entity}Dto { FieldA = fieldA, FieldB = fieldB };
        Response = await _client.Create{Entity}(request);
    }

    public async Task When_get_{entity}_by_id_request_is_sent()
        => Response = await _client.Get(_entityId);

    // THEN — inherited from Base_api_steps (no need to repeat)
    // Then_response_should_have_status(HttpStatusCode status)
    // Then_response_body_equal<TBody>(VerifiableTree expected)

    public void Dispose() { }
}
```

Key rules:
- Must inherit `Base_api_steps` for `Response`, `Then_response_should_have_status`, etc.
- Must implement `IDisposable` (LightBDD DI lifecycle requirement)
- `State<T>` tracks data between steps — use `State.GetValue()` to read
- Don't duplicate `Then_*` methods that already exist on `Base_api_steps`
- Use `App.IdGeneratorMock` to control ID generation

### 3 — Feature Class

Location: `src/ApiTests/IntegrationTests/Features/{Feature_name}.cs`

```csharp
using System;
using System.Net;
using System.Threading.Tasks;
using LightBDD.XUnit2;
using ApiTests.IntegrationTests.Application;
using ApiTests.IntegrationTests.Features.Common;

namespace Features;

public class {Feature_name} : Base_feature
{
    [Scenario]
    public async Task Creating_{entity}()
    {
        var id = Guid.NewGuid();

        await RunScenarioAsync<{Feature_name}_steps>(
            s => s.Given_no_{entity}_exists_with_id(id),
            s => s.When_{action}_request_is_sent(Test{Entity}s.Default.FieldA, Test{Entity}s.Default.FieldB),
            s => s.Then_response_should_have_status(HttpStatusCode.Created));
    }

    [Scenario]
    public async Task Getting_{entity}_by_id()
    {
        var id = Guid.NewGuid();

        await RunScenarioAsync<{Feature_name}_steps>(
            s => s.Given_no_{entity}_exists_with_id(id),
            s => s.When_{action}_request_is_sent(Test{Entity}s.Default.FieldA, Test{Entity}s.Default.FieldB),
            s => s.When_get_{entity}_by_id_request_is_sent(),
            s => s.Then_response_should_have_status(HttpStatusCode.OK));
    }
}
```

Key rules:
- Must inherit `Base_feature`
- Each scenario method has `[Scenario]` attribute
- Scenario method names become the readable test name in LightBDD reports
- `RunScenarioAsync<TSteps>` injects `TSteps` via LightBDD DI — do NOT new up steps
- Use lambda expression steps pattern: `s => s.Step_method(...)`

### 4 — Register Client in TestWebApplicationFactory

File: `src/ApiTests/IntegrationTests/Application/TestWebApplicationFactory.cs`

Add a new property for the client, initialized in the constructor exactly like `UsersClient`:

```csharp
public I{Feature}Client {Feature}Client { get; }

// In constructor:
{Feature}Client = RestClient.For<I{Feature}Client>(CreateClientWithLogger());
```

### 5 — Register Steps in ConfiguredLightBddScopeAttribute

File: `src/ApiTests/IntegrationTests/ConfiguredLightBddScope.cs`

Add one line inside `ConfigureDI`:

```csharp
cfg.RegisterType<{Feature_name}_steps>(InstanceScope.Scenario);
```

---

## Test Data Classes

If no `Test{Entities}.cs` exists for the feature, create one at:
`src/ApiTests/IntegrationTests/Application/Test{Entities}.cs`

Pattern (follow `TestUsers.cs` exactly):

```csharp
using System;

namespace ApiTests.IntegrationTests.Application;

public static class Test{Entities}
{
    public static readonly Test{Entity} Default = new();
}

public record Test{Entity}
{
    public Guid Id { get; init; } = Guid.Parse("...");   // fixed deterministic guid
    public string FieldA { get; init; } = "...";
    public string FieldB { get; init; } = "...";
}
```

---

## Assertion Reference

These are the assertion helpers available in steps (sourced from `Base_api_steps`):

| Method | Use case |
|---|---|
| `Then_response_should_have_status(HttpStatusCode.Created)` | Assert HTTP status |
| `Then_response_message_equal("message text")` | Assert raw string body |
| `Then_response_body_equal<TDto>(VerifiableTree)` | Assert typed JSON body |

For `Then_response_body_equal`, build the expected value as an anonymous tree using LightBDD's `VerifiableTree.For(value)`.

---

## Common Mistakes to Avoid

- **Forgetting `[AllowAnyStatusCode]`** on the client interface causes RestEase to throw on non-2xx — tests will fail with a RestEase exception instead of an assertion failure
- **Not registering the step class** in `ConfigureDI` causes DI resolution errors at runtime
- **Not registering the client** in `TestWebApplicationFactory` causes null reference in step constructor
- **Catching exceptions by message** in controllers (violates code guidelines) — if an endpoint has `catch (e.Message == "...")`, flag it as a code smell but still write the test around the current behaviour
- **Using `new` to instantiate step classes** — steps must be resolved by LightBDD DI to get proper lifecycle management
- **Duplicate `using` directives** — do not add usings that already exist in the file

---

## Arguments

All `dotnet test` commands use the following arguments. Read their values from the `generate-api-test` section in `.github/copilot-instructions.md`:

| Placeholder | Description |
|---|---|
| `{testProject}` | Path to the test project `.csproj` file |

Example usage:

```
dotnet test {testProject} -v minimal
dotnet test {testProject} --no-build -v minimal
```

---

## Verification Checklist

Before declaring the task done, confirm:

- [ ] All new `[Scenario]` methods pass (zero failures in `dotnet test` output)
- [ ] No new compilation errors introduced
- [ ] Step class registered in `ConfigureDI`
- [ ] Client registered in `TestWebApplicationFactory`
- [ ] File placement matches directory conventions
- [ ] No temporary files left in the workspace
