# Pure Insurance - Copilot Instructions

# Pure Insurance - Copilot Instructions

> These instructions provide context about the Pure Insurance codebase to improve AI coding assistant accuracy.
>
> **Detailed references:**
> - `.amazonq/docs/web-portal-reference.md` — complete page, modal, and control documentation with purpose, session usage, and page flows.
> - `.amazonq/docs/wcf-services-reference.md` — WCF service contracts, operations, CoreImplementation business logic, and internal method flows.
> - `.amazonq/docs/rest-api-reference.md` — PureInsurance.REST microservices API layer (C# .NET 8.0, MediatR CQRS, Ocelot gateway).
> - `.amazonq/docs/back-office-components-reference.md` — Sirius Back Office Core component reference (bSIR*/bACT*/bGIS* business components, methods, stored procedures, and cross-references).
> - `.amazonq/docs/back-office-ui-controls-reference.md` — Back Office Core UI interfaces and user controls.
> - `.amazonq/docs/underwriting-components-reference.md` — Sirius For Underwriting component reference (rating, renewal, reinsurance, risk management).
> - `.amazonq/docs/underwriting-ui-controls-reference.md` — Underwriting UI interfaces and controls.
> - `.amazonq/docs/claims-components-reference.md` — Claims module component reference (35 bCLM* business components in `Claims\Components\`, methods, stored procedures, and cross-references).
> - `.amazonq/docs/orion-components-reference.md` — Orion accounting module component reference (69 bACT* business components in `Orion\Components\`, cash list, allocation, payments, and ledger operations).
> - `.amazonq/docs/gis-components-reference.md` — GIS Combined component reference (bGIS*/bPMU*/bSIR*/bPB* components across `GIS Combined\GIS\` and `GIS Combined\Product Builder\`).
> - `.amazonq/docs/dme-components-reference.md` — Document Management Engine component reference (bDOC* components in `DME\Components\`, Documaster 2 document storage and retrieval).
> - `.amazonq/docs/sirius-architecture-components-reference.md` — Sirius Architecture component reference (core services: authentication, licensing, database access, navigation/workflow, task management, and infrastructure utilities).
> - `.amazonq/docs/shared-files-components-reference.md` — Shared Files component reference (104 cross-cutting modules in `Shared Files\`: platform constants, utility functions, GIS/GII constants, b* business components, Swift modules, and PM* platform functions).

---

## System Overview

Pure Insurance is a legacy insurance management platform built primarily in **VB.NET** targeting **.NET Framework 4.8**. It is an insurance underwriting, policy administration, claims management, and accounting system. The solution was originally built with Visual Studio 2008 and has been progressively upgraded.

### Technology Stack
- **Language**: VB.NET (primary), C# (some projects like IdentityClient, CSSFriendly, REST API handler)
- **Framework**: .NET Framework 4.8, ASP.NET WebForms
- **Database**: SQL Server (300+ tables, 6000+ stored procedures)
- **Authentication**: KeyCloak (primary) and Azure AD SSO via OWIN/OpenID Connect
- **Logging**: Microsoft Enterprise Library 5.0 (Logging Application Block)
- **Web Services**: WCF (SAM - Sirius Architecture Middleware) + REST API
- **CMS**: Built-in Content Management System (CMS.Library)
- **Document Management**: SharePoint integration + DME (Document Management Engine)
- **Build**: MSBuild, Web Deployment Projects

---

# Insurance Database — Rules

> When writing SQL or answering database questions, read and follow the rules in:
> - **[docs/database_rules.md](docs/database_rules.md)** — all SQL filters, join patterns, lookup codes and schema rules
> - **[docs/database_knowledge.md](docs/database_knowledge.md)** — full table/column/metric reference

---

## Repository Structure

```
PureInsurance/
??? Web Portal/Nexus/              ? Web Portal (MAIN PORTAL APPLICATION)
?   ??? Pure.Portals/              ? ASP.NET WebForms website (UI layer)
?   ?   ??? secure/                ? Authenticated pages (FindPolicy, FindClient, etc.)
?   ?   ??? Modal/                 ? Modal dialog pages (PremiumConfirmation, RenewalCatchUp)
?   ?   ??? Controls/              ? Reusable .ascx user controls (Allocation, CashListItem)
?   ?   ??? Products/              ? Product-specific configuration and screens
?   ?   ??? App_Code/Nexus/        ? Shared code-behind (BaseClient.vb, QuotePolicyActions.vb)
?   ?   ??? App_Themes/            ? Theme stylesheets
?   ?   ??? Masterpages/           ? Master page templates
?   ?   ??? EmailTemplates/        ? Email HTML templates
?   ?   ??? js/                    ? JavaScript files
?   ?   ??? web.config             ? Main config (providers, portals, products, reports)
?   ??? NexusProvider/             ? Provider abstraction layer (abstract base + data objects)
?   ?   ??? ProviderBase.vb        ? Abstract provider with 200+ MustOverride methods
?   ?   ??? ProviderManager.vb     ? Factory that instantiates the configured provider
?   ?   ??? Objects/               ? ~150 data transfer objects (Policy, Claim, Quote, Party)
?   ??? NexusProvider.SAMForInsurance/ ? Provider IMPLEMENTATION (calls WCF/REST)
?   ?   ??? ProviderSAMForInsuranceV2.vb       ? Main partial class, KeyCloak token handling
?   ?   ??? ProviderSAMForInsuranceV2.Core.vb  ? Core init, SAM web service calls
?   ?   ??? ProviderSAMForInsuranceV2.Party.vb ? Party/client operations
?   ?   ??? ProviderSAMForInsuranceV2.NB.vb    ? New business (AddQuote, AddRisk, UpdateRisk)
?   ?   ??? ProviderSAMForInsuranceV2.Policy.vb ? Policy operations (GetHeader, BindQuote)
?   ?   ??? ProviderSAMForInsuranceV2.MTA.vb   ? Mid-term adjustment operations
?   ?   ??? ProviderSAMForInsuranceV2.Renewal.vb ? Renewal processing
?   ?   ??? ProviderSAMForInsuranceV2.Claims.vb ? Claims operations
?   ?   ??? ProviderSAMForInsuranceV2.Account.vb ? Accounting/cash list operations
?   ?   ??? ProviderSAMForInsuranceV2.Document.vb ? Document operations
?   ?   ??? ProviderSAMForInsuranceV2.CoverNote.vb ? Cover note and MID file operations
?   ?   ??? ProviderSAMForInsuranceV2.Reinsurance2007.vb ? Reinsurance operations
?   ?   ??? ProviderSAMForInsuranceV2.Security.vb ? Password/authority operations
?   ?   ??? ProviderSAMForInsuranceV2.WorkManager.vb ? Events/tasks/audit trail
?   ?   ??? ApiMethods.vb             ? REST API helpers for newer integrations
?   ?   ??? PureService.vb            ? Auto-generated WCF proxy (~130K lines, DO NOT edit)
?   ??? Nexus.Library/             ? Framework configuration classes
?   ?   ??? Portals/Portal.vb     ? Portal config (products, tasks, reports, payments)
?   ?   ??? KeyCloak.vb           ? KeyCloak identity configuration
?   ??? CMS.Library/               ? Content Management System library
?   ?   ??? Portals/clsPortal.vb  ? Portal entity class
?   ??? Nexus.Session/             ? Session keys and app constants
?   ?   ??? Session.vb            ? ALL session key constants (prefixed with CN) + cleanup methods
?   ?   ??? Constant.vb           ? App constants, error codes, system option numbers
?   ??? Nexus.Utils/               ? Utility functions (date, email, security, formatting, caching)
?   ??? Nexus.Web.UI.WebControls/  ? Custom server controls (FindControl, GridView, PickList, ProgressIndicator)
?   ??? Nexus.HttpModules/         ? HTTP pipeline modules (Auth, Security, Error, URL Rewriting)
?   ??? MembershipProvider/        ? Custom ASP.NET membership provider
?   ??? Nexus.Reinsurance/         ? Reinsurance calculation module (VB.NET + C#)
?   ??? IdentityClient/            ? C# OWIN Startup for KeyCloak/SSO authentication
?   ??? Pure.Portals.sln          ? Main solution file
??? Web Services/STS/SAM Solution/ ? WCF web services (Sirius Architecture Middleware)
?   ??? SiriusFS.SAM.WCFService/   ? WCF service project (.svc endpoints)
?   ?   ??? ServiceContract/       ? Service interfaces (IPureService, IPurePolicyService, etc.)
?   ?   ??? ServiceImplementation/ ? Service implementations
?   ?   ??? *.svc                  ? Service endpoint files
?   ??? SiriusFS.SAM.CoreImplementation/ ? Business logic layer (partial class CoreSAMBusiness)
?   ?   ??? CoreSAMBusiness.vb     ? Main partial class
?   ?   ??? CoreSAMBusiness-Party.vb ? Party operations
?   ?   ??? CoreSamBusiness-Quote.vb ? Quote/policy operations
?   ?   ??? CoreSAMBusiness-Claims.vb ? Claims operations
?   ?   ??? CoreSAMBusiness-Accounts.vb ? Accounting operations
?   ?   ??? ParameterDefinitions.vb ? Stored procedure parameter constants
?   ??? SiriusFS.SAM.Structures/   ? WCF data contract types (request/response)
?   ??? SiriusFS.SAM.ServiceAgent/  ? Service agent proxy layer
?   ??? NUnit - SAM/               ? NUnit test projects
?   ??? XML Schemas/               ? XSD schemas for request/response types
??? Sirius For Underwriting/       ? Back-office underwriting business components
?   ??? Components/                ? VB.NET business logic (bSIR* prefix pattern)
??? Sirius Back Office Core/       ? Core back-office components
??? Sirius Architecture/           ? Architecture-level components and APIs
??? Databases/Pure/                ? SQL Server stored procedures and scripts
??? DME/                           ? Document Management Engine
??? Orion/                         ? Accounting components (CashList, Allocation, etc.)
??? STS/                           ? Secure Token Service
??? Pure Build Process/            ? Build scripts and deployment projects
??? SSP.PureInsuranceRestAPIHandler/ ? REST API handler (C#)
```

---

## Architecture & Request Flow

```
Browser ? Pure.Portals (ASP.NET WebForms)
           ? App_Code/BaseClient.vb (base page class)
             ? NexusProvider.ProviderManager (factory)
               ? NexusProvider.ProviderBase (abstract contract)
                 ? NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 (implementation)
                   ? WCF SAM Web Services OR REST API (SSP.PureInsuranceRestAPIHandler)
                     ? SQL Server Stored Procedures
```

### Key Architectural Patterns

1. **Provider Pattern**: All data access goes through `NexusProvider.ProviderBase` (abstract) ? `ProviderSAMForInsuranceV2` (concrete). The provider is resolved via `web.config` section `NexusProvider.Config`.

2. **Partial Classes**: `ProviderSAMForInsuranceV2` is split across multiple files by domain:
   - `.Core.vb` � initialization, authentication, core methods
   - `.Party.vb` � client/party operations
   - `.Document.vb` � document management
   - `.CoverNote.vb` � cover note operations

3. **Session-Based State**: Extensive use of ASP.NET Session. All session keys are constants defined in `Nexus.Constants.Session` module and prefixed with `CN` (e.g., `CNQuote`, `CNParty`, `CNAgentDetails`, `CNBranchCode`).

4. **CMS Page Inheritance**: All portal pages inherit from `CMS.Library.Frontend.clsCMSPage` (via `BaseClient.vb`), which provides theming, content management, and portal context.

5. **Configuration-Driven**: Portal behaviour is heavily driven by `web.config`:
   - `<NexusFrameWork>` � portal settings, currencies, security
   - `<NexusProvider.Config>` � data provider configuration
   - `<IdentityProvider>` � authentication (KeyCloak, SSO)
   - `<Portals><Portal>` � per-portal tasks/permissions, products, reports, payment types, claims, file types, email templates

---

## Naming Conventions

### VB.NET Code
- **Prefix `o`** for object variables: `oWebService`, `oQuote`, `oParty`, `oUserDetails`
- **Prefix `s`** for string variables: `sBranchCode`, `sRedirectPath`
- **Prefix `i` or `n`** for integer variables: `iKey`, `nInsuranceFileKey`
- **Prefix `b`** for boolean variables: `bEnableBranchSelectionAtLogin`
- **Prefix `dt`** for DateTime: `dtLastLogin`
- **Prefix `v_`** for method parameters: `v_sCommandName`, `v_nInsuranceFileKey`
- **Prefix `CN`** for session/constant names: `CNAgentDetails`, `CNQuote`, `CNBranchCode`
- **Prefix `cls`** for legacy class names: `clsPortal`, `clsCMSPage`
- **Private fields** use `i`, `s`, `b`, `dt` prefix matching the data type
- **Business component prefix `bSIR`**: `bSIRAutoMTA`, `bSIRRenewal` (Sirius business)
- **Business component prefix `bACT`**: `bACTCashList`, `bACTAllocation` (Orion accounting)

### Database (SQL Server)
- **Stored procedure prefix `spu_`**: Update/Insert/delete/get operations (e.g., `spu_add_stats_folder`)
- **Stored procedure prefix `spg_`**: These are non core product specific stored procedures (e.g., `spg_get_vehicles_for_quote`)
- Parameter names use `@` prefix with descriptive names

### Web Portal Pages
- Pages under `secure/` require authentication
- Pages under `Modal/` are modal popup dialogs
- User controls under `Controls/` with `.ascx` extension
- Product-specific screens under `Products/`
- Page code-behind files use `.aspx.vb` extension

---

## Key Domain Concepts

### Insurance Workflow
1. **Party/Client** � The insured person or company (Personal or Corporate)
2. **Quote** � An insurance quotation that can be converted to a policy
3. **Policy** � An active insurance policy with risks, premiums, and terms
4. **Risk** � Individual risk items on a policy (e.g., a vehicle on motor insurance)
5. **MTA (Mid-Term Adjustment)** � Changes to a policy during its term
6. **Renewal** � Policy renewal at end of term
7. **Claim** � Insurance claim with perils, reserves, payments, and recoveries
8. **Cash List** � Financial transaction processing (receipts and payments)
9. **Allocation** � Matching payments to policy transactions
10. **Premium Finance / Instalments** � Payment plan management

> **For detailed DTO properties, session constants, session cleanup methods, portal configuration, page flows, and common code patterns**, see `.amazonq/docs/web-portal-reference.md`.

---

## Important Notes for AI Assistants

1. **This is a LEGACY codebase** � do NOT suggest upgrading to .NET Core/6+, Blazor, or modern patterns unless explicitly asked. Maintain consistency with existing VB.NET WebForms patterns.
2. **Provider pattern is mandatory** � never bypass the provider layer to call services or database directly from UI code.
3. **Session is the state mechanism** � always use the `CN*` constants from `Nexus.Constants.Session` when reading/writing session values.
4. **Configuration is in web.config** � portal behaviour, products, permissions, reports are all XML-configured. Do not hardcode values that should come from config.
5. **VB.NET naming conventions must be followed** � use Hungarian notation prefixes (`o`, `s`, `i`, `b`, `v_`) as described above.
6. **Partial classes** � `ProviderSAMForInsuranceV2` is split across multiple `.vb` files. When looking for a method implementation, search ALL partial class files.
7. **The `ProviderBase.vb` file contains BOTH a Module (Constants) and the abstract Class** � the module is at the top of the file.
8. **CMS page lifecycle** � pages use `Page_PreInit` for theme setting and `Page_Load` for data binding. The CMS provides content management overlay.
9. **Branch-aware operations** � most operations require a branch code from session. Multi-branch users are prompted to select a branch at login.
10. **REST API is being introduced alongside WCF** � `SSP.PureInsuranceRestAPIHandler` is a newer C# project being integrated via `ProviderSAMForInsuranceV2` using `GetApiTokendetails()`.
