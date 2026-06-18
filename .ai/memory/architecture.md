# Architecture Overview — Pure Insurance

This file helps AI agents understand how Pure Insurance is structured and where new code fits.

**Last Updated**: 2026-05-07
**Owned By**: Pure Insurance Team
**Source**: Verified directly from codebase — `PureInsurance` repo (WinForms/Portal) and `PureInsurance.REST` repo (REST microservices). Previously this file incorrectly stated WCF/SAM as the Portal's backend; that has been corrected.

---

## System Overview

Pure Insurance is an enterprise insurance management platform developed by SSP (Software Solutions Partners Ltd). It consists of two main codebases:

1. **`PureInsurance`** — On-premises Windows desktop application (WinForms) + ASP.NET Web Forms Portal (Nexus)
2. **`PureInsurance.REST`** — REST API microservices layer (C# / .NET 6+) that the Portal calls

The system covers the full insurance lifecycle: party and policy management, underwriting, claims, broking, reinsurance, document production, and batch processing.

---

## Architecture Diagram

```
+------------------------------------------------------------------------+
|                    PURE INSURANCE CLIENT (Back Office)                  |
|              (WinForms rich client -- VB.NET / .NET 4.8)               |
|                                                                         |
|  +-----------------+  +-----------------+  +-----------------+         |
|  | Sirius Back     |  | Sirius For      |  | Sirius For      |         |
|  | Office Core     |  | Broking         |  | Underwriting    |         |
|  +-----------------+  +-----------------+  +-----------------+         |
|  +-----------------+  +-----------------+  +-----------------+         |
|  | Claims          |  | GIS /           |  | DME             |         |
|  | Management      |  | Product Builder |  | (Doc Mgmt)      |         |
|  +-----------------+  +-----------------+  +-----------------+         |
|                    +---------------------+                              |
|                    | Navigator XM        |  (XML-driven workflows)      |
|                    +---------------------+                              |
+----------------------------------+--------------------------------------+
                                   | dPMDAO / ADO.NET (stored procs only)
                                   v
                     +---------------------------+
                     |  Microsoft SQL Server     |
                     |  (Sirius / Pure DB)       |
                     |  snake_case schema        |
                     +-------------+-------------+
                                   ^
                                   | dPMDAO / ADO.NET (stored procs only)
+------------------------------------------------------------------------+
|                    PURE INSURANCE REST API (PureInsurance.REST repo)    |
|         C# / ASP.NET Core microservices — CQRS + MediatR pattern       |
|                                                                         |
|  +--------------------+   Microservices:                               |
|  | API Gateway        |   - Claims API                                 |
|  | (routing + auth)   |   - Policy API                                 |
|  +--------------------+   - Party API                                  |
|           |               - Account API                                |
|           |               - Core API                                   |
|           |               - Security API                               |
|           |               - Messaging API                              |
+-----------|------------------------------------------------------------+
            | HTTPS / JWT Bearer (Keycloak)
            v
+------------------------------------------------------------------------+
|                    PURE PORTAL (Nexus — Web Portal)                     |
|         ASP.NET Web Forms — C#/VB.NET / .NET 4.8                       |
|         Location: Web Portal/Nexus/Pure.Portals/                       |
|                                                                         |
|  .aspx pages call REST API via SSP.PureInsuranceRestAPIHandler          |
|  (NexusProvider pattern is LEGACY — new code uses REST API directly)   |
+------------------------------------------------------------------------+
            |
            | Keycloak OAuth2 (token issuer)
            v
+----------------------+   +-------------------+   +-------------------+
| Pure Service         |   | SSP STS           |   | External Services |
| (Windows Service,    |   | (Thinktecture      |   | - DRE             |
|  batch jobs)         |   |  IdentityServer v2 |   | - iMarket         |
+----------------------+   +-------------------+   | - AWS S3          |
                                                    | - Orion           |
                                                    +-------------------+
```

---

## Repository Routing — Where Code Lives

Pure Insurance spans **two repositories**, cloned side-by-side in the same parent folder on the developer's machine. Every code change must be made in the correct repo. **Never write code for the wrong repo.**

> **Path resolution**: Both repos share the same parent directory. If the current workspace is `{parent}\PureInsurance`, the REST repo is at `{parent}\PureInsurance.REST`. Resolve the sibling path at runtime from the IDE workspace — never hardcode a username or machine-specific path.

| Repo | Folder name | What goes here |
|------|-------------|----------------|
| `PureInsurance` | `PureInsurance` | Back-office WinForms client, ASP.NET Web Forms Portal (Nexus), VB.NET business components (`b*`/`i*` projects), Navigator XM roadmap XML, database stored procedures, migration scripts |
| `PureInsurance.REST` | `PureInsurance.REST` | REST API microservices (C# ASP.NET Core), CQRS Controllers, QueryHandlers, CommandHandlers, Services, Repositories, Domain models, API tests |

### Decision Rule — Which Repo?

```
IF the change is a:
  → new/modified .aspx page or .aspx.vb code-behind   → PureInsurance  (Portal)
  → new/modified b*, i*, g*Library VB.NET component   → PureInsurance  (Back Office / Business Logic)
  → Navigator XM roadmap XML change                   → PureInsurance  (Navigator XM Roadmaps/)
  → new/modified stored procedure or migration script → PureInsurance  (Databases/)
  → new REST API endpoint, Controller, Handler,
    Service, Repository, Domain model                 → PureInsurance.REST  (MicroServices/)
  → new REST API test                                 → PureInsurance.REST  (MicroServices/{Area}/{Area}.Tests/)
  → change to .ai/memory/, .aidlc/, .amazonq/         → PureInsurance  (spec/context files live here)
```

### Typical Feature — Files Changed in Both Repos

Most Portal features require changes in **both** repos:

```
PureInsurance.REST  (API layer — do this first)
  MicroServices/Claims/PureInsurance.REST.Claims.API/Controllers/ClaimsController.cs
  MicroServices/Claims/PureInsurance.REST.Claims.Application/Claims/Queries/{Op}/
  MicroServices/Claims/PureInsurance.REST.Claims.Application/Services/{Op}Service.cs
  PureInsurance.REST.Common.Repositories/{Op}Repository.cs
  PureInsurance.REST.Common.Domain/{Op}Model.cs

PureInsurance  (Portal + DB — after API is ready)
  Databases/Pure/Procedures/{spu_xxx}.sql
  Databases/After Change/{migration}.sql
  Web Portal/Nexus/Pure.Portals/{Area}/{Page}.aspx
  Web Portal/Nexus/Pure.Portals/{Area}/{Page}.aspx.vb
  Navigator XM Roadmaps/{WORKFLOW}.XML   (if menu/navigation changes)
```

---

## Key Components

### 1. Pure Insurance Back Office (WinForms)

The primary rich client application. All back-office operations.

| Property | Value |
|----------|-------|
| Language/Framework | VB.NET / .NET Framework 4.8 |
| Data Access | `dPMDAO` — stored procedures only, no inline SQL |
| UI Pattern | `b*` (business layer) + `i*` (interface/WinForms UI) project pairs |
| Shared Libraries | `gPMLibrary`, `gSIRLibrary`, `gCLMLibrary`, `gACTLibrary` |
| Navigation | Navigator XM (XML roadmap files in `Navigator XM Roadmaps/`) |

### 2. Pure Insurance REST API (`PureInsurance.REST` repo)

The modern REST microservices backend. **This is what the Portal calls — not WCF/SAM.**

| Property | Value |
|----------|-------|
| Language/Framework | C# / ASP.NET Core (.NET 6+) |
| Pattern | CQRS + MediatR + Repository + Unit of Work |
| Authentication | Keycloak (OAuth2 JWT Bearer) |
| Data Access | `IUnitOfWork` → Repository → `dPMDAO` → stored procedures |
| Microservices | Claims, Policy, Party, Account, Core, Security, Messaging |
| Location | Sibling repo: `PureInsurance.REST` (same parent folder as this repo) |

**Per-operation file structure (CQRS pattern):**

```
MicroServices/{Area}/{Area}.API/
  Controllers/{Area}Controller.cs       ← REST endpoint, routes to MediatR

MicroServices/{Area}/{Area}.Application/
  {Area}/Commands/{Operation}/
    {Operation}Command.cs               ← Request DTO
    {Operation}CommandHandler.cs        ← Validates + calls Service
    {Operation}CommandValidator.cs      ← FluentValidation rules
    {Operation}CommandResponse.cs       ← Response DTO
  {Area}/Queries/{Operation}/
    {Operation}Query.cs                 ← Request DTO (read)
    {Operation}QueryHandler.cs          ← Validates + calls Service
  Contracts/I{Operation}Service.cs      ← Service interface
  Services/{Operation}Service.cs        ← Business logic + calls UnitOfWork

PureInsurance.REST.Common.Repositories/
  Contracts/I{Area}Repository.cs        ← Repository interface
  {Operation}Repository.cs             ← Calls stored procedures via dPMDAO

MicroServices/{Area}/{Area}.Tests/
  {Operation}/
    {Operation}CommandHandlerTests.cs
    {Operation}CommandValidatorTests.cs
    {Operation}ServiceTests.cs
```

**Portal → API call flow:**
```
Portal .aspx page
  → SSP.PureInsuranceRestAPIHandler.ApiClient.CallApi()
  → HTTPS + JWT Bearer → API Gateway
  → Microservice Controller
  → MediatR → QueryHandler/CommandHandler
  → Service (business logic)
  → IUnitOfWork → Repository
  → dPMDAO → SQL Server stored procedure
```

### 3. Pure Portal (Nexus)

Browser-based portal for users without the WinForms client.

| Property | Value |
|----------|-------|
| Language/Framework | ASP.NET Web Forms, VB.NET / C# / .NET 4.8 |
| Location | `Web Portal/Nexus/Pure.Portals/` |
| Backend calls | REST API via `SSP.PureInsuranceRestAPIHandler` |
| Key folders | `PremiumFinance/`, `Claims/`, `Portal/`, `secure/` |
| Base classes | `App_Code/Nexus/BaseInstalment.vb`, `BaseClaim.vb`, `BaseClient.vb` etc. |

> **Note on NexusProvider**: The `NexusProvider` library (with `ProviderBase`, `ProviderManager`, `SAMForInsurance`) is **legacy**. It was the original WCF/SAM-based provider. New Portal pages call the REST API directly via `SSP.PureInsuranceRestAPIHandler`. Do not add new code to NexusProvider.

### 4. Sirius Architecture Layer

Core platform infrastructure shared by all modules.

| Property | Value |
|----------|-------|
| Language/Framework | VB.NET + C# / .NET Standard 2.0 |
| Responsibility | Licensing, session management, user/group management, task workflow, audit logging, registry/config |
| Key Projects | `dPMDAO`, `dPMDAOBridge`, `PM*` components |
| Data Access | ADO.NET via `dPMDAO` — stored procedures only |

### 5. Pure Service (Windows Service)

Background batch job processor.

| Property | Value |
|----------|-------|
| Language/Framework | VB.NET / C# / .NET 4.8 |
| Responsibility | Renewals, notifications, scheduled tasks, report scheduling |
| Projects | `WindowsService`, `ProcessJobs`, `WindowsService.TestHarness` |

### 6. SSP Secure Token Service (STS) — Legacy

| Property | Value |
|----------|-------|
| Language/Framework | C# / .NET 4.x, Thinktecture IdentityServer v2 |
| Responsibility | Issues authentication tokens for WinForms client and legacy consumers |
| Location | `STS/SSP.SecureTokenService/` |
| Note | REST API uses **Keycloak** (not STS) for token issuance |

---

## Data Flow Examples

### Portal User Action (e.g. Search Claims)
```
1. User submits form on PlanTransactions.aspx
2. Code-behind calls SSP.PureInsuranceRestAPIHandler.ApiClient.CallApi("/claims/find", GET)
3. ApiClient adds JWT Bearer token (from Keycloak), sends HTTPS request
4. API Gateway routes to Claims microservice
5. ClaimsController → MediatR.Send(FindClaimQuery)
6. FindClaimQueryHandler validates, calls FindClaimService
7. FindClaimService calls _unitOfWork.ClaimUoWRepository.FindClaim(...)
8. Repository executes stored procedure via dPMDAO
9. Results mapped to response DTO, returned as JSON
10. Portal binds JSON response to GridView
```

### Back Office User Action (WinForms)
```
1. User opens Navigator XM roadmap (e.g. MAINCLM.XML)
2. WinForms screen loads via Claims Management component (i* project)
3. i* component calls b* business library directly
4. b* component calls dPMDAO to execute stored procedure
5. Results returned to UI
```

---

## External Integrations

| Service | Purpose | Auth | Notes |
|---------|---------|------|-------|
| Keycloak | Token issuance for REST API and Portal | OAuth2 | `http://ps-altova-ls01/realms/SSPStandard` |
| DRE (Decision/Rules Engine) | Underwriting rating, rules, decline/referral | OAuth2 bearer | Called via REST API Handler |
| iMarket | Insurance market data / e-trade | SOAP / HTTP | `iMarket.XML` roadmap |
| AWS S3 | Document and file storage | AWS SDK (IAM/key-based) | `Sspi.Common.Aws` |
| Azure AD | Identity/credential management (newer components) | OAuth2 / MSAL | `Azure.Identity` 1.14.x |
| Crystal Reports | Report rendering | Embedded | SAP Crystal Reports for Visual Studio |
| Microsoft Word | Document production | Office 15+ Interop | `Microsoft.Office.Interop.Word` |

---

## Database Schema (High Level)

- **Platform**: Microsoft SQL Server (2016+)
- **Naming**: `snake_case` tables and columns
- **Access pattern**: Stored procedures only — no inline SQL from any layer

```
party                    -- individuals, companies, brokers, insurers
insurance_file / policy  -- policy records and versions
risk / risk_type         -- risk objects and type configuration
transaction              -- financial ledger entries
claim / case             -- claims and associated entities
event_log                -- system-wide audit trail
tax_group / tax_band     -- tax configuration
renewal_*                -- renewal processing
reinsurance / treaty     -- reinsurance structure
pf_prem_finance_cnt      -- premium finance / instalment plans
```

---

## Deployment Architecture

```
Application Server / Developer Workstation
  C:\Pure\Application\        ← WinForms compiled output (DLLs, EXEs)

Web Server (IIS)
  Pure Portal (Nexus)         ← ASP.NET Web Forms portal
  SSP Secure Token Service    ← Legacy token issuance

REST API Server
  PureInsurance.REST          ← ASP.NET Core microservices
  API Gateway                 ← Routing + rate limiting

Token Server
  Keycloak                    ← OAuth2 token issuer for REST API

Database Server
  SQL Server                  ← Pure / Sirius database

Windows Service Host
  Pure Service                ← Background batch processing
```

---

## Technology Stack Summary

| Layer | Technology | Version |
|-------|-----------|---------|
| Back Office UI | VB.NET WinForms | .NET Framework 4.8 |
| Web Portal | ASP.NET Web Forms, VB.NET/C# | .NET Framework 4.8 |
| REST API Microservices | C# ASP.NET Core, CQRS + MediatR | .NET 6+ |
| REST API Common | C# | .NET Standard 2.0 + .NET 6+ |
| Shared Libraries | C# / VB.NET | .NET Standard 2.0 + .NET 4.8 |
| Database | Microsoft SQL Server | 2016+ |
| Auth (REST/Portal) | Keycloak (OAuth2) | — |
| Auth (WinForms/Legacy) | Thinktecture IdentityServer v2 | .NET 4.x |
| Reports | Crystal Reports (SAP) | Embedded |
| Document Production | Microsoft Word Office Interop | Office 15+ |
| File Storage | AWS S3 via AWSSDK | 3.7.x |
| Background Jobs | Windows Service | .NET 4.8 |
| Workflow Config | Navigator XM (XML) | Runtime-parsed |

---

## Non-Functional Requirements

| Requirement | Notes |
|-------------|-------|
| Availability | On-premises; governed by customer infrastructure |
| Concurrency | Multi-user; SQL Server handles concurrency via locking/transactions |
| Security | REST API: Keycloak JWT. WinForms: STS. RBAC enforced in application + DB |
| Audit | All business events written to `event_log` table |
| Scalability | Vertical scaling; REST microservices can scale independently |
| Data Access | Stored procedures only — no inline SQL permitted in any layer |
