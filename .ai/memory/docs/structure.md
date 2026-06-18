# Pure Insurance - Project Structure

# Pure Insurance - Project Structure

## Top-Level Layout

```
Pure.sln                            # Main solution (~100+ projects, all components)
Pure.slnf                           # Solution filter (minimal build: SSP.Shared + dPMDAO)
```

### Solution Files (per subsystem)

```
Web Portal/Nexus/Pure.Portals.sln              # Web portal (15 projects)
Shared Files/SharedFiles.sln                   # Shared constants/functions library
SharedQuoteEngine/SharedQuoteEngine.sln        # Shared quote engine
SSP.Shared/SharedFilesCore.sln                 # Core shared files
DRE Integration/PureInsExtensions.sln          # DRE rules engine extensions
Pure Service/Ssp.Pure.Service.Components.sln   # Windows service components
Pure Service/Ssp.Pure.Service.Install.sln      # Service installer
Pure Service/Ssp.Pure.Service.TestHarness.sln  # Service test harness
```

---

## Core Application Layers

### Sirius Architecture (`Sirius Architecture/Components/` — 85 projects)
Platform framework — DAO, navigation, user management, licensing, locking, caching.

```
dPMDAO/                             # Core data access object (COM bridge to SQL Server)
dPMDAOBridge/                       # DAO bridge layer
Sirius.Architecture.Data/           # .NET data access (SiriusCommand, SiriusConnectionPMDAO)
Sirius.Achitecture.Data.BackOffice/ # Back-office specific data access
Sirius.Achitecture.Configuration.Local/ # Local configuration helpers
Sirius.Achitecture.Utility/         # Architecture utility functions
Navigator V3/                       # Navigator V3 UI framework
Navigator XM/                       # Navigator XM (XML-driven navigation)
Sirius Cache Controller/            # Distributed cache management
Workflow/                           # Workflow engine
PMUser/                             # User management
PMUserGroup/                        # User group management
PMUserMaintenance/                  # User admin screens
PMTask/ PMTaskGroup/                # Task/task group definitions
PMWrkManager/ PMWrkTaskInstance/    # Work manager + task instances
PMLock/                             # Record locking
PMLookup/ PMMaintainLookup/        # Lookup table management
PMProduct/ PMProductLookup/        # Product configuration
PMCurrency/                         # Currency management
PMSource/ PMSourceMaintenance/     # Branch/source management
PMCaption/                          # Multi-language caption management
PMAutoNumber/                       # Auto-numbering engine
PMMessage/ PMMessageAdmin/         # System messaging
PMEventLogViewer/ PMSiriusLogViewer/ # Log viewers
iLogonManager/ iLogonServer/       # Logon UI and server
iChangePassword/                    # Password change dialog
bLicenceManager/ bPMLicenceManager/ # Licence management
BPMUsersSync/ SSP.Pure.UsersSync/  # User synchronization
bSIROverdueTaskCheck/              # Overdue task monitoring
bSIRUserCompetenceTask/            # User competence checking
PMClientInstallAdmin/ PMClientInstallCheck/ # Client install management
_Utilities/                         # Internal utility tools
```

### Sirius Back Office Core (`Sirius Back Office Core/Components/` — 55 projects)
Party, contact, address, event, insurance file/folder, instalments, document production.
> See `.github/docs/back-office-components-reference.md` for full component reference.

```
Party/                              # Party management (217 methods, 242 SPs)
Insurance File/                     # Policy/quote data management (143 methods, 89 SPs)
Insurance Folder/                   # Policy version container
Insurance File System/              # Custom system fields on policies
Instalments/                        # Premium finance & instalments (262 methods, 279 SPs)
Document Production/                # Doc templates, spooler, SharePoint (142 methods, 85 SPs)
Find Insurance/                     # Policy search & copy (83 methods, 51 SPs)
Find Party/                         # Party search (42 methods, 18 SPs)
Event/                              # Work manager events/tasks (48 methods, 8 SPs)
Address/ Contact/ Conviction/ Lifestyle/ # Party sub-entities
Bank Guarantee/                     # Bank guarantee management (32 methods, 29 SPs)
Cash Deposit/                       # Client cash deposits (29 methods, 27 SPs)
Crystal Reports/                    # Report engine (45 methods, 18 SPs)
Orion Link/                         # Sirius-to-Orion accounting bridge
Policy Number Maintenance/          # Auto-numbering for policies/clients
System Options/                     # System-wide configuration
MediaTypeValidation/                # Bank account/card number validation
Text Files/                         # Free-text attachments
Transactions/                       # Premium posting engine
HandlerTransfer/                    # Account handler reassignment
Product Options/                    # Product-level configuration
Batch Scheduler/ TaskScheduler/     # Scheduled batch processing
Report Group/ Report Scheduler/     # Report access & scheduling
ExternalWorkFlowConfiguration/      # External workflow integration
Contact Type/ Party Loyalty Scheme/ # Lookup maintenance
GetChangeReason/ Export Control/    # MTA/claim change reasons, data export
OnlinePartyMaintenance/             # Portal access control
Client Manager/                     # Main back-office MDI shell
User Controls/                      # Shared UI controls
```

### Sirius For Underwriting (`Sirius For Underwriting/Components/` — 68 projects)
Underwriting business logic — tax, reinsurance, renewals, MTAs, commissions.

```
Tax Calculation/ Tax Band Rate/ Tax Group Bands/ # Tax engine
Peril Allocation/ Peril Type Usage/              # Peril/risk allocation
Reinsurance/ ReinsuranceTransfer/                # RI processing
RI Model/ RI Model Usage/ RI Band Version/       # RI model configuration
RI PortfolioTransfer/                            # Portfolio transfer
Treaty/                                          # Treaty management
Renewal/ Renewal Catch Up/ Batch Renewals/       # Renewal processing
Auto MTA/                                        # Automatic MTA processing
Change Policy Status/                            # Policy status workflow
Agent Commission/ Commission Rate/               # Commission management
Coinsurance/                                     # Coinsurance handling
Cover Note/                                      # Cover note management
MID Maintenance/                                 # Motor Insurance Database
Find Risk/ Find Risk Type/ Find Screen/          # Risk search/navigation
Find Policy By Product/ Find FAC Party/          # Specialized searches
List Policy/ List Policy Version/ List Risks/    # Policy/risk listing
Risk/ Risk Type/ Risk Type RI Limits/            # Risk configuration
Quote Engine/ Quote Collection Process/          # Quote processing
Short Period Rate/                               # Short period rating
Accumulations/                                   # Risk accumulation tracking
Pay Now Options/ PaymentHubWrapper/              # Payment integration
Data Take On/                                    # Data migration tool
Client Transfer/ Clone RI Transfer Auto/Manual/  # Client/policy transfer
Batch Quote Deletion/ BatchController/           # Batch operations
ChaseCycle/ Follow Up Tasks/                     # Workflow automation
Index Linking/ Lookup Detail/ Lookup Header/     # Data linking
Earning Pattern/ Statistics/ Claims Stats/       # Actuarial/stats
Underwriting Authority/ Source Defaults/         # Authority levels
Deferred RI Auto/ Deferred RI Manual/            # Deferred RI
Product/ Select Clauses/ Produce Certificate/    # Product tools
RepostTransaction/                               # Transaction repost
BatchNotification/                               # Batch notifications
```

### Sirius For Broking (`Sirius For Broking/Components/` — 12 projects)
Broking-specific features — roadmaps, workflows, free-form text.

```
Roadmap/                            # XML-based UI roadmap navigation
Work Manager Template/              # Work manager templates
Free Form Text/                     # Free-form text editing
Task List/                          # Task list management
Associates/                         # Policy associate management
Renewals/                           # Broker renewal workflows
Prospect/                           # Prospect/lead management
Relationship Maintenance/           # Client relationship tracking
Email/                              # Email integration
Doc Link/ DocuMaster Link/         # Document linking
Risk Group/                         # Risk grouping
```

---

## Business Modules

### Orion (`Orion/Components/` — 80 projects)
Accounting engine — full double-entry ledger, cash list, allocation, payment processing.

```
Account/ AccountExplorer/           # Account management and explorer
Ledger/                             # General ledger
Document/ DocumentPost/ DocumentReversal/ # Accounting documents
CashList/ CashListItem/ CashListPost/ CashListDrawer/ # Cash list processing
Allocate/ Allocation/ AllocationCalculate/ AllocationCreate/ # Allocation engine
AllocationDecision/ AllocationDetail/ AllocationManual/ AllocationPost/ # Allocation workflow
CashReceipt/                        # Receipt processing
ChequeProduction/                   # Cheque printing
Transaction/ TransDetail/ TransMatch/ # Transaction management
Currency/ CurrencyConvert/ CurrencyRate/ # Currency handling
CompanyCurrency/ Company/           # Company setup
Period/ PeriodEnd/                  # Accounting periods
Budget/ BudgetDetail/               # Budget management
Bank/ BankAccount/ BankReconciliation/ # Bank operations
FindAccount/ FindBank/ FindBudget/  # Search functions
FindCashList/ FindCashListItem/     # Cash list search
FindDocument/ FindInvoice/ FindTransaction/ # Document/transaction search
Credit Card/                        # Credit card processing
CreditControl/ CreditControlItem/  # Credit control
InsurerPayment/ InsurerPaymentAllocate/ InsurerPaymentGroups/ # Insurer payments
PremiumFinance/                     # Premium finance (Orion side)
Instalments/                        # Instalments (Orion side)
CommissionMovement/ CommissionPost/ # Commission posting
Agent Summary/                      # Agent financial summary
ImportExport/ ImportSiriusTrans/ ExportCashListItems/ ExportPFTrans/ # Import/export
Statement/ StatementControl/        # Statement generation
ManageDebtors/                      # Debtor management
MatchGroup/ MatchPost/              # Transaction matching
Payment Maintenance/ MaintainMediaTypeStatus/ # Payment admin
PurchaseInvoice/ PurchaseInvoiceCreditNote/ PurchaseInvoiceItem/ # Purchase invoices
AuditSet/                           # Audit configuration
AutoNumber/                         # Accounting auto-numbering
SuspendedTransactions/              # Suspended transaction handling
TypeTable/                          # Type table maintenance
UserAuthorities/ User Controls/     # User auth and UI controls
WriteOffReason/ CLIRepeatDecision/ MisAllocationHelper/ # Misc utilities
ClaimPaymentProcessing/ FinanceSpoke/ # Claims/finance integration
```

### Claims (`Claims/Components/` — 42 projects)
Claims lifecycle — open, assess, reserve, pay, recover, close.

```
Open Claim/                         # Claim creation
Close Claim/ Change Claim Status/   # Claim closure and status
Find Claim/ Find Case/ Find Insurance/ Find Party/ # Search
Case/                               # Claim case management
Generic Peril/ Peril Type/          # Peril/cause management
Peril Type Reserve Type/ Reserve Definition/ # Reserve configuration
Risk Details/ Risk Type/ Risk Type Information Checklist/ # Risk data
Financial Summary/ Loss Schedule/   # Financial overview
List Payments/ List Receipts/       # Payment/receipt listing
Claim Payment Process/ Payment Method/ # Payment processing
Authorise Payments/ User Authority Levels/ # Payment authorisation
Recovery/ Salvage Recovery/ Third Party Recovery/ # Recovery processing
Coinsurance Recoveries/ Reinsurance Recoveries/ # RI/co-insurance recoveries
Check Deferred RI/ Check Unpaid Premium/ # Validation checks
Claim Party/ Claim Party Link Maintenance/ Claim Address/ # Claim party data
Claim Diary/ Claim Letter/          # Diary and correspondence
Information Checklist/ Define Fields/ # Configurable fields
Document Production/ Back Office Link/ # Document generation
Roadmaps/ Unlock/ User Controls/    # UI and navigation
```

### DME (`DME/Components/` — 38 projects)
Document Management Engine — storage, retrieval, search, versioning, scanning.

```
DOCAPI/ DOCPMBAPI/                  # DME API layer
DOCDocument/ DOCDocInfo/            # Document management
DOCFolder/                          # Folder management
DOCFind/                            # Document search
DOCPage/ DOCViewer/                 # Page/viewer
DOCScan/ DOCScanStub/               # Scanning integration
DOCLink/ DOCHistory/                # Document linking & history
DOCKeyword/ DOCKeywordAdmin/ DOCDocKeyword/ # Keyword/metadata
DOCDocName/ DOCDocNameAdmin/        # Document naming
DOCDocTrans/                        # Document transactions
DOCAnnotation/                      # Document annotation
DOCCommit/ DOCCommitServer/         # Commit/save operations
DOCTransfer/                        # Document transfer
DOCPassword/ DOCSetAccessLevel/ DOCUserAdmin/ # Security
DOCOptions/                         # DME configuration
DOCManager/                         # Manager component
DOCResizerControl/                  # Image resizing
DOCViewBatch/ DOCSplash/ DOCInformation/ # UI components
DMEMigration/ DMESharedFiles/       # Migration and shared
Autorun/ Tools/ UpdateDevice/       # Utilities
MS Office Document Viewer Control/  # Office document viewing
```

### GIS Combined (`GIS Combined/` — 2 subsystems)
Gemini Insurance System — product data models, screens, rules, rating.

```
GIS/Components/                     # Core GIS engine (10 projects)
  Core GIS/                         # Core GIS dataset/screen engine
  Back Office Mapper/               # GIS-to-back-office data mapping
  List Management/ LookupManagement/ # GIS list/lookup maintenance
  GIS User Def Header/ GIS User Def Detail/ # User-defined fields
  User Defined Lookups/             # Custom lookup tables
  Insurer Scheme/                   # Insurer scheme configuration
  Premium Finance/                  # GIS premium finance integration
  Prompt Integration/               # Prompt/validation integration

Product Builder/                    # Product configuration tooling (15 projects)
  screen editor/ screen display/    # Screen design and rendering
  ruleeditor/ rulelookup/           # Business rules engine
  data model editor/                # Data model designer
  DataModelImportExport/            # Data model import/export
  import-export/ import-export v2/  # Configuration import/export
  list maintenance/                 # List data maintenance
  3D Rating/                        # 3D rating matrix
  compiled rules/                   # Pre-compiled rules cache
  gis pmu extras/                   # GIS underwriting extras
```

---

## Web & API

### Web Portal (`Web Portal/Nexus/` — 15 projects in Pure.Portals.sln)

```
Pure.Portals/                       # Main web app (ASP.NET WebForms, ASPX + VB code-behind)
NexusProvider/                      # Abstract provider base classes (~200 MustOverride methods)
NexusProvider.SAMForInsurance/      # Provider implementation (calls WCF/REST)
Nexus.Library/                      # Portal framework (Portal config, KeyCloak, Products)
Nexus.Session/                      # Session key constants (CN* prefixed) + cleanup methods
Nexus.Utils/                        # Utility functions (caching, formatting, DB helpers, JS)
Nexus.Web.UI.WebControls/           # Custom controls (FindControl, GridView, PickList)
Nexus.HttpModules/                  # HTTP pipeline (Auth, Security, Error, URL Rewriting)
Nexus.Reinsurance/                  # Reinsurance calculation module (C# + VB)
Nexus.DRE/                          # DRE (Decision Rules Engine) integration
CMS.Library/                        # Content management system library
IdentityClient/                     # C# OWIN Startup for KeyCloak/Azure AD SSO
MembershipProvider/                 # Custom ASP.NET membership provider
Nexus.RoleProvider/                 # Custom ASP.NET role provider
CSSFriendly/                        # CSS-friendly ASP.NET control adapters
SSP.PureInsuranceRestAPIHandler/    # REST API handler library (C#)
SAMClient/                          # WCF SAM client proxy
```

### Standalone REST API Handler (`SSP.PureInsuranceRestAPIHandler/`)
C# library for REST API integration, mirrors the copy in Web Portal.

```
BaseClasses/                        # Command/Query base classes
Constants/                          # API constants
Enums/                              # Enumerations
Extensions/                         # Extension methods
Services/                           # Service implementations
ApiClient.cs                        # HTTP client wrapper
TokenGeneration.cs                  # JWT token handling
```

### WCF Web Services (`Web Services/STS/SAM Solution/` — 12 projects)

```
SiriusFS.SAM.WCFService/            # WCF service endpoints (.svc files)
  ServiceContract/                  # Service interfaces (IPureService, IPurePolicyService, etc.)
  ServiceImplementation/            # Service implementations
SiriusFS.SAM.CoreImplementation/    # Business logic layer (CoreSAMBusiness partial class)
SiriusFS.SAM.Structures/            # WCF data contracts (request/response types)
SiriusFS.SAM.ServiceAgent/          # Service agent proxy layer
SiriusFS.SAM.SAMClient/             # SAM client library
SiriusFS.S4I.DataImport/            # S4I data import v1
SiriusFS.S4I.DataImport2/           # S4I data import v2
SiriusFS.S4I.DataImport3/           # S4I data import v3
NUnit - SAM/                        # NUnit test projects
WCF.TestClient/                     # Test client
XML Schemas/                        # XSD schemas for request/response
Installer/                          # WCF service installer
```

### Other Web/API

```
SSP.Shared/                         # Shared SSP library (VB.NET) — SharedFilesCore.sln
STS/SSP.SecureTokenService/         # Secure Token Service (authentication)
Web Services/Documents/             # WSDL/schema documentation
Web Services/Utility/GenerateWCFTokenID/ # Token ID generation utility
Web Services/WSE3/                  # WSE3 security extensions
```

---

## Shared Libraries

### Shared Files (`Shared Files/` — SharedFiles.sln)
Cross-cutting constants, functions, and helpers shared by all components.

```
Components/                         # Shared component wrappers
ComponentServices/                  # COM+ component services
ControlHelpers/                     # Shared control helper functions
gACTLibrary/                        # Accounting global constants (ACTConst, ACTFunc, ACTBatchConst)
gCLMLibrary/                        # Claims global constants
gPMLibrary/                         # Platform global constants (bPMFunc, bPMNavConstants, bPMShellFunc)
gSIRLibrary/                        # Sirius global constants (bSIRPremFinConst)
Sspi.Common/                        # Common SSPI types
Swift/                              # SWIFT payment integration types
```

Key shared files: `ACTConst.vb`, `ACTFunc.vb`, `bPMFunc.vb`, `bPMDocFunctions.vb`, `bPMWrkTaskInstance.vb`, `bSIRPremFinConst.vb`, `DOCConst.vb`

---

## Database

```
Databases/Pure/
  Procedures/                       # Stored procedures (~6000+)
    A-B/, C-F/, G-O/, P-R/, S-Z/   # Alphabetical by first letter after spu_
    spe/                            # Event-based stored procedures (spe_* prefix)
    Merge Fields/                   # Document merge field procedures
    DME/                            # Document Management Engine procedures
    Reports/                        # Report procedures
    SSP Product/                    # SSP product-specific procedures
    Triggers/                       # Database triggers
    Views/                          # Database views
  Structure/                        # Table/index creation scripts (~300+ tables)
  Data/                             # Seed data and configuration scripts
  Deployment/                       # Deployment scripts
  Maintenance/                      # Index maintenance scripts
```

---

## Supporting Infrastructure

### Pure Service (`Pure Service/` — 3 solutions)
Windows service for background/scheduled job execution.

```
ProcessJobs/                        # Job implementations (VB.NET)
WindowsService/                     # Service host + schema
WindowsService.Setup/               # Service installer
WindowsService.TestHarness/         # Test harness for debugging
```

### DRE Integration (`DRE Integration/` — PureInsExtensions.sln)
Decision Rules Engine — custom handlers for the external rules engine.

```
RulesEngine.Website.PureInsAddDecline/          # Decline rules handler
RulesEngine.Website.PureInsAddEntity/           # Entity creation handler
RulesEngine.Website.PureInsAddHandlerTemplate/  # Handler template
RulesEngine.Website.PureInsAddOutput/           # Output handler
RulesEngine.Website.PureInsAddOutputCommission/ # Commission output
RulesEngine.Website.PureInsAddOutputPremiumBreakdown/ # Premium breakdown
RulesEngine.Website.PureInsAddOutputReferrals/  # Referral output
RulesEngine.Website.PureInsAddOutputReferralsAudit/ # Referral audit
RulesEngine.Website.PureInsAddOutputTax/        # Tax output
RulesEngine.Website.PureInsProperties/          # Properties handler
```

### SSP Product (`SSP Product/`)
Product-specific configuration and rules, organized by product line.

```
FLEET/                              # Fleet motor product
  PMDocs/Documents/                 # Product document templates
  Rules/                            # Product-specific business rules
```

### Build, Deploy & Tools

```
Pure Build Process/                 # Build scripts, installers, Azure pipelines
  pipelines/                        # Azure DevOps YAML pipeline definitions
  Installshield/                    # Deployment packaging
Reports/                            # Crystal Reports (.rpt files)
Customer Specific/                  # Per-customer customisations (GJW, NIA, SRIC, BDX)
SharedQuoteEngine/                  # Shared quote engine (VB.NET, own .sln)
Product Builder 2/                  # Excel-based product configuration tooling (main)
Product Builder 2 ITL Jun162016/    # Product Builder 2 (ITL variant, legacy snapshot)
Product Builder 2 SSP/              # Product Builder 2 (SSP variant)
Navigator XM Roadmaps/              # XML roadmap definitions for UI navigation
TFSBuild/                           # Legacy TFS build scripts
Data Dictionary Help/               # Data model documentation
  Tables/                           # Table-level documentation
  Enumerations/                     # Enumeration documentation
  Presentations/                    # Data model presentations
  Pure Insurance Data Model v1.0.docx # Main data dictionary document
Utilities/                          # Standalone utility tools
  DTU4/                             # Data Take-On Utility v4
  PIE4/                             # Product Import/Export v4
  Product Builder 2/                # Product Builder utility copy
Update Reference Utility/           # Project reference update tool
  ProjectReferenceUpdate/           # Bulk .vbproj reference updater
```

### Runtime/Build Outputs (not source)

```
artifactWorkspace/                  # Build artifact workspace (CI/CD output, ~33K files)
Binaries/                           # Pre-compiled binary dependencies (netstandard2.0)
Common Dlls/                        # Shared DLL dependencies (netstandard2.0)
Custom Controls/                    # Third-party custom controls
  SListBar/                         # Sidebar list control
  TxTextControl/                    # TX Text Control (word processing)
Program Files/PM/                   # Installed program files layout
Pure/Application/                   # Application deployment layout
MigratedCode-115/                   # VB6-to-VB.NET migration artefacts (legacy)
```

---

## Component Naming Conventions

VB.NET projects follow a layer-prefix convention:

| Prefix | Layer | Example |
|--------|-------|---------|
| `b` | Business | `bACTAccount`, `bSIRContact`, `bCLMPerilType` |
| `d` | Data | `dPMDAO`, `dSIRContact`, `dSIREvent` |
| `g` | Global/Shared | `gACTLibrary`, `gPMLibrary`, `gSIRLibrary` |
| `a` | Application/UI | `aPMNav` |
| `i` | Interface/Controls | `iACTUserControls`, `iPMBContact` |

Module prefixes:

| Prefix | Domain | Components |
|--------|--------|------------|
| `ACT` | Accounting (Orion) | ~80 projects in `Orion/Components/` |
| `SIR` | Sirius (Back Office / Underwriting) | ~55 + 68 projects |
| `CLM` | Claims | ~42 projects in `Claims/Components/` |
| `PM` | Platform Management (Architecture) | ~85 projects in `Sirius Architecture/Components/` |
| `GIS` | GIS integration | ~25 projects in `GIS Combined/` |
| `DOC` | Document Management | ~38 projects in `DME/Components/` |
| `SAM` | SAM API layer | WCF service projects |
| `SAN` | SAN system layer | Security/auth layer |
| `PF` | Premium Finance | Instalments subsystem |
| `CCM` | Content/Communications | Document production subsystem |

## SQL Stored Procedure Naming

- `spu_` prefix — standard stored procedures
- `spe_` prefix — event-based stored procedures (CRUD on event staging tables)
- `spu_SAM_` — SAM API procedures (called from WCF/REST)
- `spu_SIR_` / `spu_SIRRen_` — Sirius / Renewal procedures
- `spu_ACT_` — Accounting (Orion) procedures
- `spu_CLM_` — Claims procedures
- `spu_TXN_` — Transaction posting procedures
- `spu_PF_` / `spu_PFPremiumFinance_` / `spu_PFInstalments_` — Premium finance
- `spu_calculate_` — Calculation procedures
- `spu_pmb_` / `spu_pmwrk_` / `spu_pmuser_` — Platform/work manager/user
- CRUD suffixes: `_add`, `_upd`, `_del`, `_sel`, `_saa` (select all), `_saa` (select one)
- Filed in `Databases/Pure/Procedures/` alphabetically by first letter after prefix

## Architectural Patterns

- **Legacy core:** Monolithic VB.NET with COM-based data access (dPMDAO)
- **REST API layer:** C# command/query pattern (SSP.PureInsuranceRestAPIHandler)
- **REST microservices:** C# .NET 8.0 CQRS with MediatR (PureInsurance.REST — separate repo)
- **Web portal:** ASP.NET Web Forms with code-behind (VB.NET)
- **WCF services:** SAM middleware layer with partial-class business logic (CoreSAMBusiness)
- **GIS engine:** Product Builder-driven data models, screens, and rules
- **New components:** Clean Architecture (Domain -> Application -> Infrastructure -> Presentation)
- **Migration strategy:** Strangler Fig — new C# components replace legacy VB.NET incrementally

## Specs & Documentation

```
.kiro/specs/{feature-name}/         # Feature/bugfix specs (requirements.md, design.md, tasks.md)
.kiro/steering/                     # Steering rules
.github/copilot-instructions.md     # Copilot coding instructions
.github/docs/                       # Detailed reference documentation
  web-portal-reference.md           # Web portal pages, modals, controls, session, page flows
  wcf-services-reference.md         # WCF service contracts, operations, CoreImplementation
  rest-api-reference.md             # REST API endpoints, SP/component mappings
  back-office-components-reference.md # Back office component reference (bSIR*/bACT*/bGIS*)
.amazonq/rules/memory-bank/         # Amazon Q memory bank
docs/Architecture/                  # Architecture documentation
Data Dictionary Help/               # Database table/field documentation
```
