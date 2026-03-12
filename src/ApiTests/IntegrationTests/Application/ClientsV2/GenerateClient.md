# ClientsV2 — Generated .NET HTTP Client

This directory contains the auto-generated C# HTTP client for the **User Benefit Cost Calculation API**, produced from [`swagger.json`](../../../../../swagger.json) using [OpenAPI Generator](https://openapi-generator.tech).

---

## Prerequisites

Install the OpenAPI Generator CLI via npm (requires Node.js):

```bash
npm install -g @openapitools/openapi-generator-cli
```

Verify the installation:

```bash
openapi-generator-cli version
```

> **Alternative — Java JAR**: If you prefer not to use npm, download the standalone JAR from the [OpenAPI Generator releases page](https://github.com/OpenAPITools/openapi-generator/releases) and replace `openapi-generator-cli` with `java -jar openapi-generator-cli.jar` in the commands below.

---

## Validate the Spec (Optional but Recommended)

Before generating, validate the OpenAPI specification:

```bash
openapi-generator-cli validate -i swagger.json
```

Run this from the **repository root** (`d:\_projects\agent-coding-api`).

---

## Generate the Client

Run the following command from the **repository root**:

```bash
openapi-generator-cli generate \
  -g csharp \
  -i swagger.json \
  -o src/ApiTests/IntegrationTests/Application/ClientsV2 \
  --additional-properties=packageName=BenefitsApi.Client,targetFramework=net10.0,netCoreProjectFile=true,nullableReferenceTypes=true,useHttpClientCreationMethod=true
```

### Windows (PowerShell / Command Prompt)

```powershell
openapi-generator-cli generate `
  -g csharp `
  -i swagger.json `
  -o src/ApiTests/IntegrationTests/Application/ClientsV2 `
  --additional-properties=packageName=BenefitsApi.Client,targetFramework=net10.0,netCoreProjectFile=true,nullableReferenceTypes=true,useHttpClientCreationMethod=true
```

---

## Key Parameters

| Parameter | Value | Description |
|---|---|---|
| `-g` | `csharp` | Generator name — produces a C# HTTP client |
| `-i` | `swagger.json` | Path to the OpenAPI spec (relative to where the command runs) |
| `-o` | `src/ApiTests/IntegrationTests/Application/ClientsV2` | Output directory for generated files |
| `packageName` | `BenefitsApi.Client` | Root C# namespace and NuGet package name |
| `targetFramework` | `net10.0` | Target .NET version |
| `netCoreProjectFile` | `true` | Emit a SDK-style `.csproj` instead of the legacy format |
| `nullableReferenceTypes` | `true` | Enable C# nullable reference type annotations |
| `useHttpClientCreationMethod` | `true` | Use `IHttpClientFactory` pattern for `HttpClient` creation |

---

## Using a Configuration File

Instead of passing all properties inline, you can use a YAML config file. Create `openapi-client-config.yaml` at the repository root:

```yaml
packageName: BenefitsApi.Client
targetFramework: net10.0
netCoreProjectFile: true
nullableReferenceTypes: true
useHttpClientCreationMethod: true
```

Then run:

```bash
openapi-generator-cli generate \
  -g csharp \
  -i swagger.json \
  -o src/ApiTests/IntegrationTests/Application/ClientsV2 \
  -c openapi-client-config.yaml
```

---

## What Gets Generated

After running the command, the output directory will contain:

```
ClientsV2/
├── src/
│   └── BenefitsApi.Client/
│       ├── Api/
│       │   └── UsersApi.cs          # Typed client for the Users endpoints
│       ├── Model/
│       │   ├── CreateUserDto.cs
│       │   ├── GetUserDto.cs
│       │   └── ApiResponse.cs
│       ├── Client/
│       │   ├── ApiClient.cs
│       │   ├── ApiException.cs
│       │   └── Configuration.cs
│       └── BenefitsApi.Client.csproj
└── README.md
```

---

## Generated API Surface

The following endpoints from `swagger.json` will be available on the `UsersApi` client class:

| Method | Path | Client Method |
|---|---|---|
| `POST` | `/Users` | `CreateUser(CreateUserDto body)` |
| `GET` | `/Users` | `GetUsers()` |
| `GET` | `/Users/{id}` | `GetUsersById(int id)` |

---

## Using the Client in Integration Tests

After generating, reference the generated project in `ApiTests.csproj` and use it in tests:

```csharp
using BenefitsApi.Client.Api;
using BenefitsApi.Client.Client;

var config = new Configuration { BasePath = "http://localhost:5000" };
var client = new UsersApi(config);

var users = await client.GetUsersAsync();
```

Or, when using `TestWebApplicationFactory`, pass the factory's `HttpClient`:

```csharp
var httpClient = _factory.CreateClient();
var apiClient = new UsersApi(new Configuration { BasePath = httpClient.BaseAddress!.ToString() });
```

---

## Re-generating After API Changes

Whenever `swagger.json` is updated (e.g., new endpoints are added), re-run the generate command above to refresh the client. The `--skip-overwrite` flag can be added if you have local customizations you want to preserve in non-generated files.

```bash
openapi-generator-cli generate \
  -g csharp \
  -i swagger.json \
  -o src/ApiTests/IntegrationTests/Application/ClientsV2 \
  -c openapi-client-config.yaml \
  --skip-overwrite
```

---

## Reference

- [OpenAPI Generator — Usage](https://openapi-generator.tech/docs/usage)
- [OpenAPI Generator — C# Generator Options](https://openapi-generator.tech/docs/generators/csharp)
- [OpenAPI Generator — Installation](https://openapi-generator.tech/docs/installation)
