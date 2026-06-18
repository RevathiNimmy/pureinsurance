# Pure Insurance - Technology Stack

## Languages

| Language | Usage |
|----------|-------|
| VB.NET | Legacy desktop/back-office components (Sirius Architecture, Orion, Claims, Back Office Core, Underwriting, Nexus portal) |
| C# | Newer web services and REST API handlers (SSP.PureInsuranceRestAPIHandler, InsurancePortal) |
| T-SQL | Stored procedures in SQL Server (`Databases/Pure/Procedures/`) |
| JavaScript | Client-side portal utilities (`Nexus.Utils/javascript/`) |
| ASP.NET Web Forms | Nexus broker portal (Pure.Portals) |

## Frameworks & Targets

| Framework | Usage |
|-----------|-------|
| .NET Framework | Core legacy libraries and REST API handler |
| .NET Standard 2.0 | Shared libraries (`Binaries/netstandard2.0/`, `Common Dlls/netstandard2.0/`) |
| .NET 10 | Newer test projects (InsurancePortal tests) |
| ASP.NET Web Forms | Nexus portal (Pure.Portals) |
| ASP.NET Core 8.0 | Target for new component APIs (per architecture roadmap) |

## Key Libraries & Packages

| Library | Purpose |
|---------|---------|
| Newtonsoft.Json 13.x | JSON serialisation in REST API handler |
| xUnit + FsCheck | Property-based testing in newer test projects |
| FluentAssertions | Test assertions |
| Moq | Mocking framework |
| Crystal Reports (.rpt) | Report generation |
| log4net 2.0.12 | Logging (Nexus portal) |
| Microsoft.IdentityModel 6.11.1 | JWT/OpenID Connect token handling |
| Microsoft.Owin 4.2.2 | OWIN middleware (auth, cookies, OpenIdConnect) |
| Microsoft.ReportingServices 150.x | SSRS report viewer |
| System.Configuration.ConfigurationManager 9.x | Configuration access |

## Solutions

| Solution | Purpose |
|----------|---------|
| `Pure.sln` | Main solution (~100+ VB.NET/C# projects) |
| `Pure.slnf` | Solution filter for minimal builds (SSP.Shared + dPMDAO) |
| `Pure.Portals.sln` | Web portal solution (`Web Portal/Nexus/`) |
| `Ssp.Pure.Service.Components.sln` | Windows service components |
| `InsurancePortal.slnx` | Newer insurance portal (Clean Architecture) |

## Build System

- **Primary:** MSBuild via Visual Studio 2017+ (VS 17.6+)
- **Legacy:** TFSBuild scripts (`TFSBuild/TFSBuild.proj`, `TFSBuild/RunBuild.cmd`)
- **CI/CD:** Azure DevOps pipelines (`Pure Build Process/pipelines/`)
- **Packaging:** InstallShield for deployment packaging
- **Default output:** `C:\Pure\Application`

## Common Build Commands

Build the main solution:
```
msbuild Pure.sln /p:Configuration=Release /p:Platform="Any CPU"
```

Build the portal solution:
```
msbuild "Web Portal/Nexus/Pure.Portals.sln" /p:Configuration=Release
```

Run tests (newer InsurancePortal test projects):
```
dotnet test "Web Portal/InsurancePortal/tests/OutOfSequenceMTASDD.BugfixTests/OutOfSequenceMTASDD.BugfixTests.csproj"
```

## Database

- **Engine:** SQL Server
- **Access pattern:** Stored procedures as the primary data access layer (via dPMDAO COM component)
- **Procedure location:** `Databases/Pure/Procedures/` (alphabetical subfolders A-B, C-F, G-O, P-R, S-Z)
- **Structure scripts:** `Databases/Pure/Structure/`
- **Seed data:** `Databases/Pure/Data/`

## Target Architecture (Roadmap)

| Layer | Target Technology |
|-------|------------------|
| API Framework | ASP.NET Core 8.0 Web API |
| Language | C# 12 |
| ORM | Entity Framework Core 8.0 |
| Messaging | Azure Service Bus / MassTransit |
| Caching | Redis |
| API Gateway | Azure API Management |
| Frontend | React / Angular (SPA) |
| Containers | Docker + Kubernetes (AKS) |
| Cloud | Microsoft Azure |
| Observability | Serilog + Azure Application Insights |
| Resilience | Polly (circuit breaker, retry, timeout) |

## Authentication & Security

- **Current:** Windows Authentication + custom membership/role providers (Nexus portal), WS-Federation, OpenID Connect
- **Token service:** SSP Secure Token Service (`STS/SSP.SecureTokenService/`)
- **Keycloak:** Supported via `Nexus.Library/KeyCloak.vb`
- **Target:** Azure AD B2C / IdentityServer4 with JWT tokens

## Version Control & DevOps

- **VCS:** Git (Azure Repos) — migrated from TFS/TFVC
- **CI/CD:** Azure DevOps Pipelines
- **Code quality target:** SonarQube
- **Testing target:** xUnit, NUnit, SpecFlow; 80% coverage for new components
