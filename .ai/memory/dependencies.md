# Dependencies — Pure Insurance

Known internal and external dependencies, NuGet packages, and third-party services.

**Last Updated**: 2026-04-28
**Owned By**: Pure Insurance Team

---

## Runtime Platform

| Dependency | Version | Purpose | Notes |
|-----------|---------|---------|-------|
| .NET Framework | 4.8 | Primary runtime for all WinForms client components | Target for ~996 VB projects |
| .NET Standard | 2.0 | Shared libraries and REST API handler | `Sspi.Common.*`, `SSP.PureInsuranceRestAPIHandler` |
| SQL Server | 2016+ | Primary database | On-premises; accessed via stored procedures only |

---

## Key NuGet Packages

### Application Layer (.NET 4.8)

| Package | Version | Used In | Purpose |
|---------|---------|---------|---------|
| `Azure.Core` | 1.47.1 | ExportRegistry (+ newer components) | Azure SDK foundation |
| `Azure.Identity` | 1.14.2 | ExportRegistry (+ newer components) | Azure AD credential management |
| `Microsoft.Data.SqlClient` | 6.1.1 | ExportRegistry | Modern SQL Server data client |
| `Microsoft.Identity.Client` (MSAL) | 4.73.1 | ExportRegistry | OAuth2 / Azure AD token acquisition |
| `Microsoft.IdentityModel.JsonWebTokens` | 7.7.1 | ExportRegistry | JWT validation |
| `Microsoft.Extensions.Caching.Memory` | 8.0.1 | ExportRegistry | In-memory caching |
| `Microsoft.Office.Interop.Word` | 15.0.4797 | Shared Files | Word document production |

### Shared Libraries (.NET Standard 2.0)

| Package | Version | Used In | Purpose |
|---------|---------|---------|---------|
| `AWSSDK.Core` | 3.7.302.12 | `Sspi.Common.Aws` | AWS SDK foundation |
| `AWSSDK.S3` | 3.7.305.28 | `Sspi.Common.Aws` | S3 file storage |
| `Microsoft.CSharp` | 4.7.0 | `Sspi.Common.Aws` | Dynamic support |
| `Newtonsoft.Json` | (latest stable) | `SSP.PureInsuranceRestAPIHandler` | JSON serialisation for REST API calls |

### Windows Service (.NET 4.8)

| Package | Version | Used In | Purpose |
|---------|---------|---------|---------|
| `System.Security.AccessControl` | 6.0.0 | Pure Service | Windows ACL management |
| `System.Security.Permissions` | 8.0.0 | Pure Service | Code access security |
| `System.Security.Principal.Windows` | 5.0.0 | Pure Service | Windows identity |

---

## Internal Shared Libraries

| Library | Project | Language | Purpose |
|---------|---------|---------|---------|
| `SharedFiles` | `Shared Files/SharedFiles.vbproj` | VB.NET / .NET 4.8 | Cross-cutting utilities: `gPMLibrary`, `gSIRLibrary`, `gCLMLibrary`, `gACTLibrary` global libraries |
| `Sspi.Common.Aws` | `Shared Files/Sspi.Common.Aws/` | C# / .NET Standard 2.0 | AWS S3 integration |
| `SSP.PureInsuranceRestAPIHandler` | `SSP.PureInsuranceRestAPIHandler/` | C# / .NET Standard 2.0 | HTTP client for external REST APIs; OAuth2 token management |
| `dPMDAO` | `Sirius Architecture/Components/dPMDAO/` | VB.NET | Core data access object — all DB calls go through here |
| `dPMDAOBridge` | `Sirius Architecture/Components/dPMDAOBridge/` | C# / .NET Standard 2.0 | Bridge adapter exposing `dPMDAO` to .NET Standard consumers |

---

## External Services & Integrations

| Service | Purpose | Auth | Owner |
|---------|---------|------|-------|
| **DRE (Decision/Rules Engine)** | Underwriting rating, decline/refer rules, business logic rules | OAuth2 bearer token (via STS) | SSP / Insurer |
| **iMarket** | Insurance market e-trade integration | SOAP / HTTP | Third-party market |
| **AWS S3** | Document and file storage | AWS IAM (access key or role) | AWS |
| **Azure Active Directory** | Identity provider for newer authentication paths | OAuth2 / MSAL | Microsoft Azure |
| **Orion** | External system link | TBD | Third-party |
| **Crystal Reports (SAP)** | Embedded report rendering | N/A (embedded runtime) | SAP |
| **Microsoft Word** | Document production via Office Interop | N/A (local Office installation) | Microsoft |

---

## Authentication Dependencies

| Component | Technology | Notes |
|-----------|-----------|-------|
| SSP Secure Token Service (STS) | Thinktecture IdentityServer v2 | Primary auth token issuer for the application |
| Azure AD / MSAL | `Microsoft.Identity.Client` 4.73.x | Used in newer components (e.g. ExportRegistry) alongside or instead of STS |

---

## Database

| Item | Detail |
|------|--------|
| Engine | Microsoft SQL Server 2016+ |
| Access pattern | Stored procedures only (no inline SQL) |
| Client library | `System.Data.SqlClient` (legacy) / `Microsoft.Data.SqlClient` 6.1.x (newer) |
| Schema | `snake_case` tables and columns |
| Key databases | Pure/Sirius main database; utility scripts database |

---

## Build & Deployment

| Item | Detail |
|------|--------|
| Build system | MSBuild (Visual Studio solution files) |
| Output path | `C:\Pure\Application\` |
| Solution files | `Pure.sln` (full), `Pure.slnf` (filtered) |
| Package management | NuGet (`packages.config` style for .NET 4.8; `PackageReference` for .NET Standard 2.0) |
| Version control | Azure DevOps (TFS/Git) — organisation: `SSP-Insurer`, project: `Pure Insurance` |
