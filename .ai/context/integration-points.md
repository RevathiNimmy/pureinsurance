---
title: Integration Points
description: Confirmed and inferred integration points between PureInsurance and sibling systems, derived from code and configuration analysis
ms.date: 2026-04-28
---

## Overview

This document maps integrations identified by scanning config files, project references, and source code. Cross-referenced against 33 sibling repositories in the `SSP-Insurer / Pure Insurance` ADO project. Only integrations with evidence are included.

---

## PureInsurance.REST

**Purpose:** The REST API server that this codebase calls for modern back-end operations (policy, BDX import, user management). Hosted separately; not in this repository.

**We consume:** REST endpoints at `https://{host}/api/` — authenticated with OAuth2 bearer token from Keycloak. Called via `SSP.PureInsuranceRestAPIHandler.ApiClient`.

**We provide:** Nothing back to this repo.

**Breaking-change risks:**

- `SSP.PureInsuranceRestAPIHandler` (1800+ command/response classes, 90+ enums) acts as a typed contract. Any breaking schema change in `PureInsurance.REST` requires a matching update here.
- `ApiEndpoint`/`RestAPIUrl` are configuration values; all callers must be reconfigured on host/port changes.
- Token refresh uses a static `HttpClient` with `OnTokenRefreshed` callback — if the token endpoint moves, every caller breaks.

**Evidence:**

| File | Detail |
|------|--------|
| `Pure Service/WindowsService/app.config:24` | `ApiEndpoint = https://localhost:7246/api/` |
| `Web Portal/Nexus/Pure.Portals/web.config:827` | `RestAPIUrl = https://localhost:7246/api` |
| `Orion/.../SiriusImport/app.config:51` | `RestAPIUrl = https://localhost:7246/api` |
| `Pure.sln:721` | `SSP.PureInsuranceRestAPIHandler.csproj` project reference |
| `Web Portal/Nexus/Pure.Portals.sln:66` | `SSP.PureInsuranceRestAPIHandler.csproj` project reference |
| `Pure Service/WindowsService/WindowsService.vbproj:98` | Assembly reference `SSP.PureInsuranceRestAPIHandler.dll` |
| `Pure Service/ProcessJobs/ProcessJobs.vbproj:103` | Assembly reference `SSP.PureInsuranceRestAPIHandler.dll` |

---

## DRE (Decision Rules Engine)

**Purpose:** Underwriting rules engine — evaluates rating, decline, referral, and commission rules for insurance products. Hosted as a separate web application. The `DRE Integration/` folder in this repo contains Pure-specific plug-in assemblies that run *inside* the DRE process.

**We consume:** `RulesEngine.BaseSystem`, `RulesEngine.EngineCommon`, `RulesEngine.EngineSupport`, `RulesEngine.RuleLineHandlers`, `RulesEngine.Website`, `RulesEngine.Website.WebCustomControls` — all as binary DLL references loaded from `..\..\Site\bin\`.

**We provide:** Extension plug-in assemblies: `RulesEngine.Website.PureInsAddDecline`, `RulesEngine.Website.PureInsAddEntity`, `RulesEngine.Website.PureInsAddOutput`, `RulesEngine.Website.PureInsAddOutputCommission`, `RulesEngine.Website.PureInsAddOutputPremiumBreakdown`, `RulesEngine.Website.PureInsAddOutputReferrals`, `RulesEngine.Website.PureInsAddOutputReferralsAudit`, `RulesEngine.Website.PureInsAddOutputTax`, `RulesEngine.Website.PureInsHandlerTemplate`, `RulesEngine.Website.PureInsProperties`. These are deployed into the DRE `Site/bin/` folder.

**Breaking-change risks:**

- DLL interface coupling — any change to `RulesEngine.*` public API types requires recompilation of all `DRE Integration/` projects.
- The DRE binary DLLs are not in source control here; they are pulled from a deploy path (`..\..\Site\bin\`). A DRE upgrade that changes public types will cause silent runtime failures until plug-ins are rebuilt.
- `PureInsExtensions.sln` is the solution file covering all extension plug-ins — must be kept in sync with the DRE version in deployment.

**Evidence:**

| File | Detail |
|------|--------|
| `DRE Integration/PureInsExtensions.sln` | Solution grouping all DRE extension plug-in projects |
| `DRE Integration/RulesEngine.Website.PureInsProperties/RulesEngine.Website.PureInsProperties.vbproj:52–74` | Binary refs to `RulesEngine.BaseSystem`, `RulesEngine.EngineCommon`, `RulesEngine.EngineSupport`, `RulesEngine.RuleLineHandlers`, `RulesEngine.Website`, `RulesEngine.Website.WebCustomControls` — all `HintPath` pointing to `..\..\Site\bin\` |
| `.ai/memory/architecture.md:184` | Documented as `DRE (Decision/Rules Engine)` integration |

---

## Keycloak (ps-altova-ls01)

**Purpose:** Self-hosted OAuth 2.0 / OIDC identity provider. Issues bearer tokens consumed by `PureInsurance.REST` and the Nexus web portal. Two realms in use: `SSPStandard` (REST API / Windows service) and `SSPPurePortal62` (Nexus portal login).

**We consume:** Token endpoint for client-credentials and password grants. Portal login redirect (OIDC authority).

**We provide:** Nothing.

**Breaking-change risks:**

- Token URL is hardcoded in `TokenGeneration.cs` (source control exposure — see `known-issues.md`). Realm rename or server move breaks all token acquisition silently.
- `Windows Service/app.config` and `TokenGeneration.cs` each hardcode the token URL independently — two places to update.
- The `SSPPurePortal62` realm is referenced in `web.config`; a realm rename breaks portal login.

**Evidence:**

| File | Detail |
|------|--------|
| `Pure Service/WindowsService/app.config:30` | `TokenUrl = http://ps-altova-ls01/realms/SSPStandard/protocol/openid-connect/token` |
| `Web Portal/Nexus/Pure.Portals/web.config:39` | `Authority = http://ps-altova-ls01/realms/SSPPurePortal62` |
| `SSP.PureInsuranceRestAPIHandler/TokenGeneration.cs` | Hardcoded token URL (realm `SSPStandard`) — see `known-issues.md` |

---

## SiriusFS SAM WCF Service (internal — Web Services/)

**Purpose:** Legacy ASMX/WCF service layer providing party, policy, list management, and work-management operations. Hosted at `{server}/PureWebServices/` (ASMX) or `/SiriusFS.SAM.ServiceAgent/` and `/siriusfs.sam.serviceagent/`. This is an **internal service within the PureInsurance repo** (not a sibling repo), but it is consumed externally via HTTP.

**We consume (internally):** `SAMForInsuranceV2.asmx`, `MessagingServiceForUnderwriting.asmx`. Called by `SiriusFS.SAM.ServiceAgent` proxy.

**We provide:** SOAP endpoints consumed by external sibling systems (e.g., Nexus portal, SAMClient, WCF test clients). URL pattern: `http://{server}/siriusfs.sam.serviceagent/samforinsurancev2.asmx`.

**Breaking-change risks:**

- WCF WSDL contract is the shared interface. Any change to `SiriusFS.SAM.Structures` (DTO types) requires regeneration of all consumer proxies.
- Multiple config files hardcode the ASMX URL, including production server names (`VM-NEXUSUS`, `vm-sam-sfi1810`).
- Legacy WSE 3.0 security policy (`wse3policyCache.config`) is in use — no modern transport security.

**Evidence:**

| File | Detail |
|------|--------|
| `Databases/Utility Scripts/CloseClaimUtility/app.config:56,60,64` | Endpoints for `SAMForInsuranceV2.asmx`, `MessagingServiceForUnderwriting.asmx` |
| `Sirius Back Office Core/.../DataModelRebuild/app.config:56,60,64` | Same endpoints |
| `Web Portal/SAMClient/app.config:6,7` | `PartiesAndPolicies.asmx`, `ListManagement.asmx` |
| `Web Portal/Nexus/Pure.Portals/web_us.config:81` | `VM-NEXUSUS/siriusfs.sam.serviceagent/samforinsurancev2.asmx` |
| `Web Services/STS/SAM Solution/SiriusFS.SAM.SAMClient/app.config:22` | `vm-sam-sfi1810/SIRIUSFS.SAM.SERVICEAGENT/ListManagement.asmx` |
| `Web Services/STS/TestHarness/SAM_WEB_V2/Work Manager Exposure/*.aspx.vb` | `SAMForInsuranceV2` instantiation with `SamClientPolicy` |

---

## Pure WCF Services (internal — Web Services/)

**Purpose:** WCF service tier exposing core business domains to the Nexus web portal and external consumers. Six services: `PureAccountService`, `PureSecurityService`, `PureCoreService`, `PurePolicyService`, `PurePartyService`, `PureClaimService`. Hosted at `http://{server}/PureWCFServices/`.

**We consume:** Nothing from outside (these are services *hosted* here).

**We provide:** WCF endpoints consumed by `Web Portal/Nexus` (configured via `web_WindowsAuthentication.config` and `web.config`) and by external installers.

**Breaking-change risks:**

- Nexus portal has six hardcoded WCF endpoint addresses (two sets — production and localhost). Any service rename or path change requires a config update in `web_WindowsAuthentication.config`.
- STS installer (`Pure Build Process/PureSTS Installer/Setup.Rul`) dynamically constructs `PureSecurityService.svc` URL from a configurable server name — this is the only install-time integration point.
- Patch installer (`PurePatch`) constructs all six service URLs from `g_szPureAppServerName`.

**Evidence:**

| File | Detail |
|------|--------|
| `Web Portal/Nexus/Pure.Portals/web_WindowsAuthentication.config:622–635` | Six endpoint elements for `PureAccountService`, `PureSecurityService`, `PureCoreService`, `PurePolicyService`, `PurePartyService`, `PureClaimService` |
| `Web Portal/Nexus/Pure.Portals.sln:66` | `Web.config` endpoint `http://localhost:51624/PurePartyService.svc` (dev) |
| `Pure Build Process/PureSTS Installer/Setup.Rul:1157` | `PureSecurityService.svc` URL constructed from `g_PureAppServer` |
| `Pure Build Process/PurePatch/Setup.Rul:1923–1926` | All six service URLs constructed at install time |

---

## AddressFinder (api.addressfinder.io)

**Purpose:** Third-party address lookup and autocomplete API — used in the Nexus web portal for NZ address entry. Widget JS served from the same provider.

**We consume:** `GET https://api.addressfinder.io/api/address` (address search), `http://api.addressfinder.io/assets/v3/widget.js` (UI widget).

**We provide:** Nothing.

**Breaking-change risks:**

- API key (`AddressFinderKey = [REDACTED — see web.config:834]`) is stored in `web.config` in plain text — exposed in source control.
- If AddressFinder changes API version or discontinues the service, address entry in the portal fails.

**Evidence:**

| File | Detail |
|------|--------|
| `Web Portal/Nexus/Pure.Portals/web.config:832–834` | `AddressFinderWidgetURL`, `AddressFinderURL`, `AddressFinderKey` |

---

## CarJam (www.carjam.co.nz)

**Purpose:** Third-party NZ vehicle lookup API — likely used for motor insurance risk entry in the Nexus web portal.

**We consume:** `http://www.carjam.co.nz/api/car` (vehicle data by plate).

**We provide:** Nothing.

**Breaking-change risks:**

- URL is in config only; no server-side code calling it was found in the scanned area. It may be consumed client-side via JavaScript or from an unscanned area.
- No API key visible in config — authentication mechanism unknown.

**Evidence:**

| File | Detail |
|------|--------|
| `Web Portal/Nexus/Pure.Portals/web.config:835` | `CarJamURL = http://www.carjam.co.nz/api/car` |

---

## ECB Eurofxref (www.ecb.int)

**Purpose:** European Central Bank XML feed for daily foreign exchange rates. Consumed to keep currency conversion rates current.

**We consume:** `http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml` (XML, polled).

**We provide:** Nothing.

**Breaking-change risks:**

- URL is in config only; no server-side calling code found in the scanned area. May be consumed from an unscanned component.
- The ECB feed is HTTP (not HTTPS). If the ECB enforces HTTPS only, the feed breaks.

**Evidence:**

| File | Detail |
|------|--------|
| `Web Portal/Nexus/Pure.Portals/web.config:814` | `RemoteXMLCurrency = http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml` |
| `Web Portal/Nexus/Pure.Portals/web_WindowsAuthentication.config:449` | Same key |
| `Web Portal/Nexus/Pure.Portals/web_us.config:460` | Same key |

---

## Postcode Lookup (postcodeservice.mmbox.co.uk)

**Purpose:** Third-party UK postcode lookup ASMX service — used for address entry in the Nexus portal.

**We consume:** `http://postcodeservice.mmbox.co.uk/service.asmx`.

**We provide:** Nothing.

**Breaking-change risks:**

- Currently **disabled** (`EnablePostCodeLookup="false"`) in all config variants. The URL is retained for potential reactivation.
- The service is HTTP only and ASMX-based — would need WSE or BasicHttpBinding client.

**Evidence:**

| File | Detail |
|------|--------|
| `Web Portal/Nexus/Pure.Portals/web.config:265,459,695` | `PostCodeWebServiceUrl` — disabled |
| `Web Portal/Nexus/Pure.Portals/web_WindowsAuthentication.config:222,397` | Same — disabled |
| `Web Portal/Nexus/Pure.Portals/web_us.config:162,272,382` | Same — disabled |

---

## Sibling Repos with No Code-Level Evidence

The following repos were found in the ADO project but have **no confirmed integration evidence** in this codebase (no URL references, no NuGet/assembly references, no project includes):

| Repo | Notes |
|------|-------|
| `AIChatBot` | No reference found |
| `APISharedComponents` | No reference found |
| `PureAIInsights` | No reference found |
| `PureAdminPortal` | No reference found |
| `PureInsuranceAPIs` | No reference found — may be a server-side API that `PureInsurance.REST` repo implements |
| `PureInsuranceGatewayAPI` | No reference found |
| `PureSelfService` | No reference found |
| `NextGen` | No reference found |
| `ProductBuilder2` | No reference found |
| `ProductStudio` | No reference found |
| `SSPEngineering_Pure` | No reference found |
| `DMU` | Referenced by name in installer scripts only — no API coupling |
| `DTU` | Referenced by name in installer scripts only — no API coupling |
| `QA` | Test/QA tooling — not an integration target |
| `LicenseUtility` | Tooling only |
| `CodeRefactoringTool` | Tooling only |
| `DataFixes` | Data maintenance scripts — no runtime coupling |
| `PURE_AI_Specs`, `PURE_AmazonQ_KnowledgeBank` | Documentation/knowledge repos |
| `PureInsuranceAutomation` | Automation/deployment scripts — no runtime coupling |
| `ProjectsBahamasFirstGroup`, `ProjectsESRIC`, `ProjectsGENUtil`, `ProjectsHollardN`, `ProjectsOMICO`, `ProjectsSACOS`, `ProjectsBFG` | Customer-specific project repos — not integrated at code level |

> If any of these repos are known to call into PureInsurance or be called by it, add evidence-backed entries above.
