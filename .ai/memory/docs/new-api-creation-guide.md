# New API Creation Guide — Architecture Rules

> **Purpose:** Every new API endpoint in this solution MUST follow the layered architecture and checklist below.
> Use the `PayClaim` command (Claims microservice) as the canonical reference example.

---

## Architecture Overview

```
API Gateway (Ocelot)
  └─► MicroService.API  (Controller)
        └─► MicroService.Application  (MediatR Handler → Validator → Service)
              ├─► Contracts  (Service Interface)
              ├─► Services   (Service Implementation)
              └─► Common.Repositories  (Data Access)
```

Each microservice follows: **Claims, Policy, Account, Core, Party, Security, Messaging**.

---

## Step-by-Step Checklist

### 1. Determine Command vs Query

| Type | When | Folder |
|------|------|--------|
| **Command** | Creates, updates, or deletes data (POST/PUT/DELETE) | `Commands/{FeatureName}/` |
| **Query** | Reads data only (GET) | `Queries/{FeatureName}/` |

Location: `MicroServices\{Domain}\PureInsurance.REST.{Domain}.Application\{Domain}\Commands\` or `Queries\`

---

### 2. Create the Application Layer Files

All files go in **one folder** named after the feature under `Commands/` or `Queries/`.

#### 2a. Command/Query Class (the request DTO)

- **File:** `{FeatureName}Command.cs` or `{FeatureName}Query.cs`
- Inherits from a `Base` class and implements `IRequest<TResponse>` (MediatR).
- Keep this class thin — just the MediatR marker.

```csharp
// Example: PayClaimCommand.cs
using MediatR;

namespace PureInsurance.REST.Claims.Application.Claims.Commands.PayClaim
{
    public class PayClaimCommand : PayClaimCommandBase, IRequest<PayClaimCommandResponse>
    {
    }
}
```

#### 2b. Base Class (the actual properties)

- **File:** `{FeatureName}CommandBase.cs` or `{FeatureName}QueryBase.cs`
- Inherits from `REST.Common.Domain.Models.BaseRequestType`.
- Contains all request properties.
- Use `[JsonIgnore]` for internal-only properties not exposed to callers.

```csharp
// Example: PayClaimCommandBase.cs
public class PayClaimCommandBase : REST.Common.Domain.Models.BaseRequestType
{
    public BaseClaimPaymentType? ClaimPayment { get; set; }
    // ... public properties for API consumers

    [JsonIgnore]
    public bool IsDataTransferClaim { get; set; }
    // ... internal properties
}
```

#### 2c. Response Class

- **File:** `{FeatureName}CommandResponse.cs` (inherits from `BaseResponse`)
- **File:** `{FeatureName}CommandBaseResponse.cs` (contains actual response properties)

```csharp
// Response inherits BaseResponse
public class PayClaimCommandResponse : PayClaimCommandBaseResponse { }

// BaseResponse has the actual properties
public class PayClaimCommandBaseResponse
{
    public int ClaimKey { get; set; }
    public string? ClaimNumber { get; set; }
    // ...
}
```

#### 2d. Validator (FluentValidation)

- **File:** `{FeatureName}CommandValidator.cs`
- Extends `AbstractValidator<TCommand>`.
- Use `InvalidData.MandatoryInputMissing` / `InvalidData.MandatoryInputInvalid` error codes from `PureInsurance.REST.Policy.Application.Constants`.

```csharp
// Example: PayClaimCommandValidator.cs
public class PayClaimCommandValidator : AbstractValidator<PayClaimCommand>
{
    public PayClaimCommandValidator()
    {
        RuleFor(x => x.BranchCode)
            .NotEmpty()
            .WithErrorCode(InvalidData.MandatoryInputMissing.ToString())
            .WithMessage(nameof(InvalidData.MandatoryInputMissing));
    }
}
```

#### 2e. Handler (MediatR)

- **File:** `{FeatureName}CommandHandler.cs`
- Implements `IRequestHandler<TRequest, TResponse>`.
- **Always follows this order:**
  1. Validate request via injected `IValidator<T>`.
  2. Check identity/authority via `IIdentityService.CheckIdentityAndAuthority(...)`.
  3. Delegate business logic to a **Service** (never put business logic in the handler).
  4. Return the response.

```csharp
public class PayClaimCommandHandler : IRequestHandler<PayClaimCommand, PayClaimCommandResponse>
{
    private readonly IValidator<PayClaimCommand> _validator;
    private readonly IIdentityService _identityService;
    private readonly IPayClaimService _payClaimService;

    public PayClaimCommandHandler(
        IValidator<PayClaimCommand> validator,
        IIdentityService identityService,
        IPayClaimService payClaimService)
    {
        _validator = validator;
        _identityService = identityService;
        _payClaimService = payClaimService;
    }

    public async Task<PayClaimCommandResponse> Handle(PayClaimCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate
        var result = await _validator.ValidateAsync(request, CancellationToken.None);
        if (!result.IsValid)
            throw new ValidationException($"Invalid request", result.Errors);

        // 2. Authorize
        await _identityService.CheckIdentityAndAuthority(...);

        // 3. Business logic via Service
        var resultArray = _payClaimService.PayClaim(request);

        // 4. Return
        return (PayClaimCommandResponse)resultArray;
    }
}
```

---

### 3. Create the Service Layer

#### 3a. Service Interface

- **File:** `Contracts/I{FeatureName}Service.cs`
- Location: `MicroServices\{Domain}\PureInsurance.REST.{Domain}.Application\Contracts\`

```csharp
public interface IPayClaimService
{
    object PayClaim(PayClaimCommand request);
}
```

#### 3b. Service Implementation

- **File:** `Services/{FeatureName}Service.cs`
- Location: `MicroServices\{Domain}\PureInsurance.REST.{Domain}.Application\Services\`
- Inject repositories, common services, UnitOfWork, etc.

```csharp
public class PayClaimService : IPayClaimService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISiriusUserService _siriusUserService;
    // ...

    public object PayClaim(PayClaimCommand request) { /* business logic */ }
}
```

---

### 4. Register in Dependency Injection

- **File:** `MicroServices\{Domain}\PureInsurance.REST.{Domain}.Application\DependencyInjection.cs`
- Add a `services.AddScoped<>` line for the new service interface → implementation.
- MediatR handlers and FluentValidation validators are auto-registered via assembly scanning (already configured).

```csharp
services.AddScoped<IPayClaimService, PayClaimService>();
```

> **Note:** Validators and Handlers do NOT need manual registration — they are picked up by:
> ```csharp
> services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
> services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
> ```

---

### 5. Add the Controller Endpoint

- **File:** `MicroServices\{Domain}\PureInsurance.REST.{Domain}.API\Controllers\{Domain}Controller.cs`
- Controller inherits from `ApiControllerBase` (provides `Mediator` property).
- Controller has class-level attributes:
  - `[ApiController]`
  - `[Route("{Domain}")]` (e.g., `[Route("Claims")]`)
  - `[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]`
  - `[EnableRateLimiting("RateLimit")]`

#### Endpoint pattern:

```csharp
/// <summary>
/// {Description of what the endpoint does}.
/// </summary>
/// <param name="request">{CommandName}.</param>
/// <param name="cancellationToken">CancellationToken.</param>
/// <returns>{ResponseName}.</returns>
[Http{Verb}]                                               // HttpGet, HttpPost, HttpPut, HttpDelete
[Route("/{domain}/{routePath}")]                           // Absolute route, lowercase camelCase
[ProducesResponseType(typeof({ResponseType}), 200)]
[Produces("application/json", "application/xml")]
[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.{Verb}))]
public async Task<ActionResult<{ResponseType}>> {MethodName}([FromBody/FromQuery] {RequestType} request, CancellationToken cancellationToken)
{
    request.LoginUserName = await _identityService.GetLoggedInUserName(User, Request);
    request.Route = Request.Path.Value;
    var result = await Mediator.Send(request, cancellationToken);
    return Ok(result);
}
```

**Rules:**
- GET endpoints use `[FromQuery]`.
- POST/PUT/DELETE endpoints use `[FromBody]`.
- Route paths use **lowercase camelCase** (e.g., `/claims/payment`, `/claims/case/close`).
- Always set `request.LoginUserName` and `request.Route` before sending via MediatR.
- `ApiConventionMethod` must match the HTTP verb (Get, Post, Put, Delete).
- Add the `using` statement for the new command/query namespace at the top of the controller file.

---

### 6. Register Routes in Ocelot Gateway

**Both files must be updated:**

| File | Scheme | Host/Port |
|------|--------|-----------|
| `ocelot.json` | `https` | `localhost` : `{service port}` |
| `ocelot.installer.json` | `http` | `$APISERVER$` : `$APIPORT$` |

#### Port mapping by microservice:

| Microservice | localhost Port | Installer Placeholders |
|---|---|---|
| Claims | 7087 | `$CLAIMSAPISERVER$` / `$CLAIMSAPIPORT$` |
| Policy | 7056 | `$POLICYAPISERVER$` / `$POLICYAPIPORT$` |
| Account | 7035 | `$ACCOUNTAPISERVER$` / `$ACCOUNTAPIPORT$` |
| Core | 7121 | `$COREAPISERVER$` / `$COREAPIPORT$` |
| Party | 7103 | `$PARTYAPISERVER$` / `$PARTYAPIPORT$` |
| Security | 7107 | `$SECURITYAPISERVER$` / `$SECURITYAPIPORT$` |
| Messaging | 7123 | `$MESSAGINGAPISERVER$` / `$MESSAGINGAPIPORT$` |

#### Route entry template for `ocelot.json`:

```json
{
  "DownstreamPathTemplate": "/{domain}/{routePath}",
  "DownstreamScheme": "https",
  "DownstreamHostAndPorts": [
    {
      "Host": "localhost",
      "Port": {port}
    }
  ],
  "UpstreamHttpMethod": [ "{Verb}" ],
  "UpstreamPathTemplate": "/api/{domain}/{routePath}",
  "AuthenticationOptions": {
    "AuthenticationProviderKey": "Bearer"
  },
  "AddHeadersToRequest": {
    "preferred_username": "Claims[preferred_username] > value"
  }
}
```

#### Route entry template for `ocelot.installer.json`:

```json
{
  "DownstreamPathTemplate": "/{domain}/{routePath}",
  "DownstreamScheme": "http",
  "DownstreamHostAndPorts": [
    {
      "Host": "${DOMAIN}APISERVER$",
      "Port": ${DOMAIN}APIPORT$
    }
  ],
  "UpstreamHttpMethod": [ "{Verb}" ],
  "UpstreamPathTemplate": "/api/{domain}/{routePath}",
  "AuthenticationOptions": {
    "AuthenticationProviderKey": "Bearer"
  },
  "AddHeadersToRequest": {
    "preferred_username": "Claims[preferred_username] > value"
  }
}
```

> **Important:** Place new routes grouped with existing routes for the same microservice. Ocelot does NOT auto-discover routes.

---

### 7. Add Unit Tests

- **Location:** `MicroServices\{Domain}\PureInsurance.REST.{Domain}.Tests\{FeatureName}\`
- Create test files for:
  - `{FeatureName}CommandHandlerTests.cs` — test handler logic, mocking validator, identity, and service.
  - `{FeatureName}CommandValidatorTests.cs` — test validation rules.
  - `{FeatureName}ServiceTests.cs` — test service business logic.

---

## File Creation Summary (Command example)

For a new command `PayClaim` in the **Claims** microservice, create these files:

```
MicroServices/Claims/
├── PureInsurance.REST.Claims.Application/
│   ├── Claims/Commands/PayClaim/
│   │   ├── PayClaimCommand.cs              # IRequest<T> marker
│   │   ├── PayClaimCommandBase.cs          # Request properties
│   │   ├── PayClaimCommandResponse.cs      # Response marker
│   │   ├── PayClaimCommandBaseResponse.cs  # Response properties
│   │   ├── PayClaimCommandValidator.cs     # FluentValidation rules
│   │   └── PayClaimCommandHandler.cs       # MediatR handler
│   ├── Contracts/
│   │   └── IPayClaimService.cs             # Service interface
│   ├── Services/
│   │   └── PayClaimService.cs              # Service implementation
│   └── DependencyInjection.cs              # ← ADD service registration
├── PureInsurance.REST.Claims.API/
│   └── Controllers/
│       └── ClaimsController.cs             # ← ADD endpoint + using
└── PureInsurance.REST.Claims.Tests/
    └── PayClaim/
        ├── PayClaimCommandHandlerTests.cs
        ├── PayClaimCommandValidatorTests.cs
        └── PayClaimServiceTests.cs

ApiGateway/PureInsurance.REST.ApiGateway/
├── ocelot.json                             # ← ADD route (https/localhost)
└── ocelot.installer.json                   # ← ADD route (http/placeholders)
```

---

## Common Mistakes to Avoid

1. **Forgetting Ocelot routes** — New endpoints will return 404 from the gateway if not registered in BOTH `ocelot.json` AND `ocelot.installer.json`.
2. **Forgetting DI registration** — Services need manual `AddScoped<>` in `DependencyInjection.cs`. Handlers and validators are auto-registered.
3. **Wrong HTTP verb in Ocelot** — The `UpstreamHttpMethod` must exactly match the controller attribute (`Get`, `Post`, `Put`, `Delete`).
4. **Missing `request.LoginUserName` and `request.Route`** — Always set these in the controller before `Mediator.Send()`.
5. **Business logic in handler** — Keep handlers thin. Delegate to a Service.
6. **Wrong `[FromBody]` vs `[FromQuery]`** — GET uses `[FromQuery]`, POST/PUT/DELETE use `[FromBody]`.
7. **Route casing** — Use lowercase camelCase for route paths (e.g., `/claims/paymentTaxGroups` not `/claims/PaymentTaxGroups`).
8. **Wrong port in Ocelot** — Each microservice has a fixed port. Check the port mapping table above.
