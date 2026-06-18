---
title: Codebase Map
description: Module and project inventory, key entry points, folder structure, and shared libraries for Pure Insurance
ms.date: 2026-04-28
---

## Overview

Pure Insurance is a 1000+ project solution organized into functional layers. The primary language is VB.NET (.NET Framework 4.8). Newer shared libraries use C# (.NET Standard 2.0). All compiled output goes to `C:\Pure\Application\`.

---

## Folder Structure

```
PureInsurance/
├── Sirius Architecture/        Core framework, data access, auth, navigation engine
├── Sirius Back Office Core/    Party, documents, instalments, fees, tax
├── Sirius For Underwriting/    Policy, risk, RI, treaties, product config
├── Sirius For Broking/         Broker-specific business workflows
├── Claims/                     Full claims lifecycle management
├── Orion/                      Accounting/financial transactions ledger
├── GIS Combined/               Geographic info + Product Builder UI
├── DME/                        Document Management Engine
├── Pure Service/               Windows Service for batch processing
├── STS/                        Secure Token Service (IdentityServer v2)
├── SSP.PureInsuranceRestAPIHandler/  REST API client library (C#/.NET Standard)
├── SSP.Shared/                 SSP-specific shared utilities
├── Shared Files/               Global shared libraries (gPMLibrary etc.)
├── SharedQuoteEngine/          Quote calculation engine
├── DRE Integration/            Decision Rules Engine integration stubs
├── Web Services/               Legacy WCF/SOAP services (SAM)
├── Web Portal/                 ASP.NET web portal (Nexus)
├── Navigator XM Roadmaps/      XML workflow definitions (80+ files)
├── Databases/                  SQL Server schema, data, stored procedures
├── Reports/                    Crystal Reports templates
├── Custom Controls/            Reusable WinForms controls (SListBar, TxTextControl)
├── Customer Specific/          Client customisations (BDX, GJW, NIA, SRIC)
├── SSP Product/                Fleet product add-on
├── Pure Build Process/         Build and installer tooling
└── scripts/                    Utility scripts
```

---

## Project Naming Conventions

| Prefix | Layer | Example |
|--------|-------|---------|
| `b` | Business logic | `bPMFunc`, `bCLMOpenClaim`, `bSIRInstalments` |
| `i` | Interface / WinForms UI | `iCLMOpenClaim`, `iPMNavigatorXM`, `iACTTransaction` |
| `d` | Data access | `dPMDAO`, `dPMDAOBridge` |
| `u` / `uct` | User control | `uctPMAddressControl`, `uctCLMReserve`, `uctCLMReceipt` |
| `g` | Global/shared library | `gPMFunctions`, `gSIRLibrary`, `gACTLibrary` |
| `a` | Application entry point | `aPMNav` |
| `PM*` | Pure Manager architecture | `PMProduct`, `PMUser`, `PMTask` |
| `SSP.*` | SSP .NET Standard components | `SSP.PureInsuranceRestAPIHandler` |
| `Sspi.*` | SSP internal shared | `Sspi.Common.Aws` |
| `Sirius.*` | Sirius modern layer | `Sirius.Achitecture.Data` |
| `SiriusFS.*` | Sirius web services | `SiriusFS.SAM.WCFService` |

Each business component typically consists of a matched pair: `bXxx` (business) + `iXxx` (interface).

---

## Key Areas

### Sirius Architecture (`Sirius Architecture/Components/`)

Core infrastructure used by every other area.

| Component | Projects | Purpose |
|-----------|---------|---------|
| dPMDAO | `dPMDAO.vbproj` (VB.NET, .NET Standard 2.0) | Central data access layer — all DB calls go through here |
| dPMDAOBridge | `dPMDAOBridge.csproj` (C#, .NET Standard 2.0) | C# wrapper exposing dPMDAO to .NET Standard consumers |
| Navigator V3 | `bPMNav`, `iPMNav`, `aPMNav` | Legacy workflow navigation engine |
| Navigator XM | `bPMNavigatorXM`, `iPMNavigatorXM`, `XMConvertor` | XML-driven workflow engine (current standard) |
| Workflow | `PMWorkflowMaintenance`, `PMStepMaintenance` | Workflow step management |
| Logon/Auth | `iLogonManager`, `iLogonServer`, `bPMLicenceManager` | User authentication and licence enforcement |
| Cache | `SiriusCacheController`, `bSIRCacheController` | Application-level caching |
| User Management | `PMUser`, `PMUserGroup`, `PMUserMaintenance` | User and group administration |
| Product Config | `PMProduct`, `PMProductLookup`, `PMSource` | Insurance product definitions |
| Address Control | `uctPMAddressControl`, `bPMAddressControl` | Reusable address UI + business logic |
| Lookup Control | `PMLookupControl` | Reusable lookup/search UI control |
| Auto Number | `bPMAutoNumber` | Reference number generation |
| Modern C# layer | `Sirius.Achitecture.Configuration.Local`, `Sirius.Achitecture.Data`, `Sirius.Achitecture.Data.BackOffice`, `Sirius.Achitecture.Utility` | Newer .NET Standard 2.0 components |

### Sirius Back Office Core (`Sirius Back Office Core/Components/`)

| Component | Purpose |
|-----------|---------|
| Party | Party (insured, broker, insurer, TPA) management |
| Document Production / Document Manager Wrapper | Policy schedule, letter, and document generation (Word interop) |
| Gemini List Manager | Configurable list and lookup management |
| Instalments (`bSIRPFInstalments`) | Premium instalment schedule processing |
| Fees | Fee calculation and management |
| Tax | Tax calculation on premiums |
| User Controls | `uctPartyTax`, `uctPMUFees`, `uctPMURITax` |

### Sirius For Underwriting (`Sirius For Underwriting/Components/`)

60+ components covering the full underwriting lifecycle.

| Area | Key Components |
|------|---------------|
| Policy | Policy, Quote Engine, List Policy, Find Policy By Product |
| Risk | Risk, Risk Type, Find Risk, List Risks, Risk Type Usage |
| Reinsurance | Reinsurance, ReinsuranceTransfer, Deferred RI Auto/Manual, Clone RI Transfer, RI Model, RI Band Version, RI Portfolio Transfer |
| Tax | Tax Calculation, Tax Band Rate, Tax Group Bands |
| Treaty | Treaty management |
| Product | Product, Product Builder, Find Product Type |
| Authority | Underwriting Authority, rules enforcement |
| Statistics | Statistics, Claims Stats |
| Other | Cover Note, Commission Rate, Coinsurance, Accumulations, Peril Allocation, Peril Type Usage, Short Period Rate, Source Defaults, MID Maintenance, Index Linking, Data Take On |

### Claims (`Claims/Components/`)

35+ components managing the full claims lifecycle.

| Component | Purpose |
|-----------|---------|
| Open Claim / Close Claim | FNOL intake and claim closure |
| Find Claim / Find Case | Claim and case search |
| Claim Diary | Diary entry management |
| Reserve Definition | Reserve type configuration |
| Financial Summary | Claim financial overview |
| Claim Payment Process / List Payments | Payment workflow |
| List Receipts | Receipt management |
| Third Party Recovery | TPR processing |
| Reinsurance Recoveries | RI recovery management |
| Coinsurance Recoveries | Coinsurance settlement |
| Salvage Recovery | Salvage processing |
| Authorise Payments | Payment authorisation workflow |
| User Controls | `uctCLMReserve`, `uctCLMReceipt`, `uctClaimParty` |

### Orion (`Orion/Components/`)

Accounting and financial transaction management.

| Component | Purpose |
|-----------|---------|
| Transaction | Financial transaction recording |
| Period / PeriodEnd | Accounting period management and close |
| Premium Finance | Instalment plan financing |
| Cheque Production | Cheque generation workflow |
| Suspended Transactions | Deferred transaction handling |
| Purchase Invoice | Purchase invoice and credit notes |
| TransMatch / TransDetail | Transaction matching and detail |
| Write-Off Reason | Write-off categorisation |
| User Authorities | Orion-specific authorisation |

### GIS Combined (`GIS Combined/`)

| Area | Purpose |
|------|---------|
| GIS | Geographic Information System — risk location, flood zones, peril mapping |
| Product Builder | Product screen designer, rule editor, sum insured config, wording management |

Product Builder user controls: `uctSumsInsured`, `uctStandardWordings`, `uctRiskScreen`

### DME (`DME/`)

Document Management Engine — document storage, retrieval, and lifecycle.

| Component | Purpose |
|-----------|---------|
| DOCViewer / DOCViewBatch | Document viewing UI |
| MS Office Document Viewer Control | Word/Excel interop viewer |
| DMEHarmoniser | DME integrity checker |
| DMETransfer | Document transfer utility |
| SFSDMEClaims | Claims-specific DME |

### Pure Service (`Pure Service/`)

Windows Service for background/batch job processing.

| Project | Purpose |
|---------|---------|
| `WindowsService.vbproj` | Main Windows Service host |
| `ProcessJobs.vbproj` | Job processing business logic |
| `WindowsService.TestHarness.vbproj` | WinForms harness for manual developer testing |

### STS — Secure Token Service (`STS/SSP.SecureTokenService/`)

Thinktecture IdentityServer v2 implementation. Issues JWT tokens for user and service-to-service authentication.

### SSP.PureInsuranceRestAPIHandler (`SSP.PureInsuranceRestAPIHandler/`)

C# / .NET Standard 2.0 client library for calling the external Pure REST API.
See [api-documentation.md](api-documentation.md) for details.

### Web Services — SAM (`Web Services/STS/SAM Solution/`)

Legacy WCF/SOAP services for Sirius Access Manager.

| Project | Purpose |
|---------|---------|
| `SiriusFS.SAM.WCFService` | WCF service host |
| `SiriusFS.SAM.ServiceAgent` | Service consumer proxy |
| `SiriusFS.SAM.CoreImplementation` | Business logic |
| `SiriusFS.SAM.Structures` | Data transfer objects |
| `WCF.TestClient` | Developer test harness |
| `SiriusFS.SAM.NUnit.SAMForInsurance` | NUnit tests (legacy/reference) |

### DRE Integration (`DRE Integration/`)

11 extension stubs for the Decision Rules Engine:

- `PureInsAddDecline` — Decline result handler
- `PureInsAddEntity` — Entity injection
- `PureInsAddHandlerTemplate` — Handler scaffold
- `PureInsAddOutput` — Standard output
- `PureInsAddOutputCommission` — Commission output
- `PureInsAddOutputPremiumBreakdown` — Premium breakdown
- `PureInsAddOutputReferrals` / `PureInsAddOutputReferralsAudit` — Referral handling
- `PureInsAddOutputTax` — Tax output
- `PureInsProperties` — DRE property definitions

---

## Shared Libraries

### `Shared Files/` (SharedFiles.vbproj — VB.NET .NET 4.8)

The single most-referenced library. Contains global utilities used everywhere.

| Sub-library | Key Files | Purpose |
|------------|-----------|---------|
| `gPMLibrary/` | `gPMFunctions.vb`, `gPMConstants.vb`, `gPMFunctions64.vb`, `gPMMaths.vb`, `gPMRegConst.vb` | Core utility functions, constants, registry access, Win32 P/Invoke |
| `gSIRLibrary/` | `gSIRLibrary.vb` | Sirius-specific shared functions |
| `gACTLibrary/` | — | Accounting/Orion shared functions |
| `Sspi.Common/Sspi.Common.Aws/` | `Sspi.Common.Aws.csproj` (C#, .NET Standard 2.0) | AWS S3 integration |
| `Components/` | `bPMFunc.vb`, `bACTFunc.vb`, `bGEMFunc.vb`, `bSIRPremFinConst.vb`, `ACTConst.vb`, `DOCConst.vb`, `GeminiConst.vb` | Shared business functions and constants |

### `SSP.Shared/`

Mirrors `Shared Files/` organisation for SSP-specific utilities. Contains `gPMLibrary/`, `gSIRLibrary/`, `gACTLibrary/`, `Components/`, `ComponentServices/`.

---

## Navigator XM Roadmaps (`Navigator XM Roadmaps/`)

80+ XML files defining screen navigation workflows. No code changes needed to alter workflow — only XML edits.

Key roadmaps:

| File | Business Process |
|------|----------------|
| `PFNEWQUOTE.XML` | New quotation |
| `NEWPLAN.XML` / `PFNEWPLAN.XML` | New policy binding |
| `PFQUOTEMTA.XML` | MTA quotation |
| `MAINCLM.XML` | Claims main workflow |
| `OPENCLM.XML` / `OPENCLMNT.XML` | Open claim |
| `PAYCLM.XML` / `PAYCLAIM.XML` | Claim payment |
| `RESERVE.XML` | Reserve management |
| `SALVAGE.XML` | Salvage processing |
| `TPRECOVERY.XML` / `TPRECOVER.XML` | Third party recovery |
| `JOURNAL.XML` | Journal entry |
| `ACTIPAY.XML` | Activity payment |
| `FINDPRTY*.XML` | Party search (8 variants) |

Schema defined by `navigatorxm.dtd` / `navigatorxmV2.dtd`.

---

## Database (`Databases/`)

| Folder | Content |
|--------|---------|
| `Pure/Structure/PURE_STRUCTURE.sql` | Full table schema definitions |
| `Pure/Data/PURE_DATA.sql` | Reference/lookup data |
| `Pure/Procedures/` | 5000+ stored procedures (A-B, C-E, F, G-O, P-R, S-Z, Reports, Views, spe) |
| `Installer/` | Installation and upgrade scripts |
| `After Change/` | Post-change migration scripts |
| `Utility Scripts/` | One-off database utilities |

See [data-models.md](data-models.md) for schema details.

---

## Key Entry Points

| Entry Point | Project | What It Does |
|------------|---------|-------------|
| `aPMNav` | `Sirius Architecture/` | Main WinForms application shell — loads Navigator XM and routes users |
| `WindowsService` | `Pure Service/` | Windows Service host — starts job processing loop |
| `ProcessJobs` | `Pure Service/` | Job dispatch logic called by Windows Service |
| `SSP.SecureTokenService` | `STS/` | IdentityServer v2 token endpoint |
| `SiriusFS.SAM.WCFService` | `Web Services/` | WCF endpoint for legacy SOAP consumers |
| `Nexus` | `Web Portal/` | ASP.NET web portal for browser-based access |
| `SSP.PureInsuranceRestAPIHandler` | Root | REST API client — called by consumers needing external API access |
