# API Documentation — Pure Insurance REST API

**Last Updated**: 2026-05-07
**Owned By**: Pure Insurance Team
**Source**: Verified directly from the `PureInsurance.REST` sibling repo (same parent folder as `PureInsurance`)

> **Correction from previous version**: This file previously described only `SSP.PureInsuranceRestAPIHandler` as a client library calling an "external" API. That was incorrect. The REST API is **this organisation's own microservices solution** in the `PureInsurance.REST` repo. The Portal calls it. The WinForms client also uses `SSP.PureInsuranceRestAPIHandler` to call it.

---

## Overview

`PureInsurance.REST` is a C# ASP.NET Core microservices solution that serves as the backend for:
- The Pure Portal (Nexus ASP.NET Web Forms)
- The WinForms back-office client (via `SSP.PureInsuranceRestAPIHandler`)
- External service-to-service integrations

**Pattern**: CQRS + MediatR + Repository + Unit of Work
**Auth**: Keycloak OAuth2 JWT Bearer (`http://ps-altova-ls01/realms/SSPStandard`)
**Data Access**: All operations via stored procedures through `dPMDAO`

---

## Solution Structure (`PureInsurance.REST`)

```
PureInsurance.REST/
├── ApiGateway/
│   └── PureInsurance.REST.ApiGateway/         ← Ocelot/YARP gateway (routing + rate limiting)
├── IdentityServer/
│   └── PureInsurance.REST.Identity/           ← Identity integration
├── MicroServices/
│   ├── Account/                               ← Account/financial operations
│   ├── Claims/                                ← Claims lifecycle
│   ├── Core/                                  ← Cross-cutting (lists, events, work manager)
│   ├── Messaging/                             ← Messaging/notifications
│   ├── Party/                                 ← Party management
│   ├── Policy/                                ← Policy lifecycle
│   └── Security/                              ← Auth/authorisation
├── PureInsurance.REST.Common/                 ← Shared API infrastructure
├── PureInsurance.REST.Common.Application/     ← Shared application services + contracts
├── PureInsurance.REST.Common.Domain/          ← Shared domain models + enums
└── PureInsurance.REST.Common.Repositories/    ← Shared repositories
```

---

## Microservice Structure (per microservice)

Each microservice follows the same 3-project structure:

```
{Area}.API/
  Controllers/{Area}Controller.cs     ← REST endpoint, JWT auth, routes to MediatR

{Area}.Application/
  {Area}/Commands/{Operation}/
    {Operation}Command.cs             ← Request DTO (write operations)
    {Operation}CommandHandler.cs      ← Validates + calls Service via MediatR
    {Operation}CommandValidator.cs    ← FluentValidation rules
    {Operation}CommandResponse.cs     ← Response DTO
  {Area}/Queries/{Operation}/
    {Operation}Query.cs               ← Request DTO (read operations)
    {Operation}QueryHandler.cs        ← Validates + calls Service
    {Operation}QueryResponse.cs       ← Response DTO
  Contracts/I{Operation}Service.cs    ← Service interface
  Services/{Operation}Service.cs      ← Business logic + calls IUnitOfWork
  DependencyInjection.cs              ← DI registrations

{Area}.Tests/
  {Operation}/
    {Operation}CommandHandlerTests.cs
    {Operation}CommandValidatorTests.cs
    {Operation}ServiceTests.cs
```

---

## Authentication

### Keycloak (used by REST API and Portal)

Token endpoint: `http://ps-altova-ls01/realms/SSPStandard/protocol/openid-connect/token`

**User auth (password grant):**
```csharp
{ "grant_type", "password" },
{ "client_id",  "SSP-Red" },
{ "client_secret", "<secret>" },
{ "username", "<sirius-username>" },
{ "password", "<password>" },
{ "scope", "openid" }
```

**Service-to-service (client credentials):**
```csharp
{ "grant_type",    "client_credentials" },
{ "client_id",     ClientId },
{ "client_secret", Environment.GetEnvironmentVariable("CLIENT_SECRET") }
```

**Controller auth:**
```csharp
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableRateLimiting("RateLimit")]
```

---

## SSP.PureInsuranceRestAPIHandler (REST Client Library)

**Location:** `SSP.PureInsuranceRestAPIHandler/` (in `PureInsurance` repo)
**Used by:** WinForms back-office client and Portal code-behind to call the REST API

```csharp
// How Portal/WinForms calls the REST API
ApiClient.CallApi("/claims/find", HttpMethod.Get, requestBody);
// Returns typed response via DeserializeJson<T>()
```

Key files:
- `ApiClient.cs` — Static HTTP client wrapper, automatic token refresh
- `TokenGeneration.cs` — OAuth 2.0 token acquisition
- `BaseClasses/` — 1800+ typed command/response classes
- `Constants/` — Business constants
- `Enums/` — 90+ enum types

---

## Existing Microservices & Key Endpoints

### Claims API (`/claims/...`)

| Method | Route | Operation |
|--------|-------|-----------|
| GET | `/claims/find` | Find claims by criteria |
| GET | `/claims/details` | Get full claim details |
| POST | `/claims/open` | Open new claim |
| POST | `/claims/maintain` | Edit existing claim |
| POST | `/claims/payment` | Make claim payment |
| POST | `/claims/receipt` | Process claim receipt |
| POST | `/claims/bind` | Make claim live |
| GET | `/claims/risk` | Get claim risk data |
| GET | `/claims/recoveryCoinsurance` | Recovery coinsurance |
| GET | `/claims/recoveryReinsurance` | Recovery reinsurance |
| GET | `/claims/checkUnpaidPremium` | Check unpaid premium |
| POST | `/claims/processClaim` | Process claim transactions |

### Core API (`/core/...`)

Cross-cutting operations: lists, events, work manager, document management, lock management.

### Policy API (`/policy/...`)

Policy lifecycle: quote, bind, MTA, renewal, cancel.

### Party API (`/party/...`)

Party management: create/update party, bank details, contacts.

### Account API (`/account/...`)

Financial/accounting operations: transactions, allocations, cash lists.

---

## Adding a New Endpoint — Step-by-Step Pattern

When adding new API functionality (e.g. for SPEC-39489 Claim Recovery Instalment), follow this exact pattern using Claims API as the reference:

### 1. Domain model (if new) — `PureInsurance.REST.Common.Domain/`
```csharp
// New DTO file e.g. ClaimRecoveryInstalmentModel.cs
public class EligibleRecoveryTransaction { ... }
```

### 2. Repository (if new SP) — `PureInsurance.REST.Common.Repositories/`
```csharp
// Interface
public interface IClaimRecoveryInstalmentRepository
{
    DataSet GetEligibleRecoveryTransactions(string partyCode, string? claimNo);
}

// Implementation calls dPMDAO stored procedure
public class ClaimRecoveryInstalmentRepository : IClaimRecoveryInstalmentRepository
{
    public DataSet GetEligibleRecoveryTransactions(string partyCode, string? claimNo)
        => _unitOfWork.Execute("spu_CLR_EligibleRecoveryTransactions_Sel", partyCode, claimNo);
}
```

### 3. Query/Command + Handler — `Claims.Application/Claims/Queries/{Operation}/`
```csharp
// Query.cs
public class GetEligibleRecoveryTransactionsQuery : GetEligibleRecoveryTransactionsQueryBase,
    IRequest<GetEligibleRecoveryTransactionsQueryResponse> { }

// QueryHandler.cs
public class GetEligibleRecoveryTransactionsQueryHandler :
    IRequestHandler<GetEligibleRecoveryTransactionsQuery, GetEligibleRecoveryTransactionsQueryResponse>
{
    public async Task<...> Handle(GetEligibleRecoveryTransactionsQuery request, CancellationToken ct)
    {
        request.AgentKey = await _identityService.CheckIdentityAndAuthority(
            request.LoginUserName!, UserCanDoTasks.SAMFClm.ToString());
        // validate + call service
    }
}
```

### 4. Service — `Claims.Application/Services/`
```csharp
public class GetEligibleRecoveryTransactionsService : IGetEligibleRecoveryTransactionsService
{
    public GetEligibleRecoveryTransactionsQueryBaseResponse GetEligibleRecoveryTransactions(
        GetEligibleRecoveryTransactionsQuery request)
    {
        _unitOfWork.Initialize(_siriusUserService.GetSiriusUser());
        var ds = _unitOfWork.ClaimUoWRepository.GetEligibleRecoveryTransactions(
            request.PartyCode, request.ClaimNo);
        // map DataSet rows to response DTOs
    }
}
```

### 5. Controller endpoint — `Claims.API/Controllers/ClaimsController.cs`
```csharp
[HttpGet]
[Route("/claims/recovery/eligibleTransactions")]
[ProducesResponseType(typeof(GetEligibleRecoveryTransactionsQueryResponse), 200)]
public async Task<ActionResult<GetEligibleRecoveryTransactionsQueryResponse>> Get(
    [FromQuery] GetEligibleRecoveryTransactionsQuery request, CancellationToken cancellationToken)
{
    request.LoginUserName = await _identityService.GetLoggedInUserName(User, Request);
    request.Route = Request.Path.Value;
    var result = await Mediator.Send(request, cancellationToken);
    return Ok(result);
}
```

### 6. Tests — `Claims.Tests/{Operation}/`
```csharp
{Operation}QueryHandlerTests.cs
{Operation}QueryValidatorTests.cs
{Operation}ServiceTests.cs
```

---

## Common Domain Models (PureInsurance.REST.Common.Domain)

Key existing domain files relevant to this feature:

| File | Purpose |
|------|---------|
| `PremiumFinance.cs` | Premium finance plan models |
| `FinancePlan.cs` | Finance plan structure |
| `FinancePlanModel.cs` | Finance plan request/response model |
| `OutStandingTransactions.cs` | Outstanding transaction model |
| `PartyBankDetailsModel.cs` | Party bank details |
| `CalculateSettlePlanModel.cs` | Instalment plan calculation |

---

## Legacy: WCF/SAM Services (Do Not Use for New Code)

`Web Services/STS/SAM Solution/` contains legacy WCF services (`SiriusFS.SAM.WCFService`). These are consumed by the legacy `NexusProvider` in the Portal. **New Portal pages must NOT use NexusProvider/SAM — use the REST API instead.**

The `NexusProvider.ProviderBase` → `SAMForInsurance` chain is the old path. It is being replaced by `PureInsurance.REST` microservices.
