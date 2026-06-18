# Coding Conventions — Pure Insurance

Standardized conventions for this codebase. These guidelines help AI agents write code consistent with existing patterns.

**Last Updated**: 2026-04-28
**Owned By**: Pure Insurance Team

---

## Language & Framework Versions

| Component | Language/Framework | Version | Notes |
|-----------|-------------------|---------|-------|
| WinForms client components | VB.NET | .NET Framework 4.8 | Primary — 996 VB projects |
| Shared libraries / REST handler | C# | .NET Standard 2.0 | 41 C# projects |
| Windows Service | VB.NET / C# | .NET Framework 4.8 | `Pure Service` |
| Database | T-SQL | SQL Server 2016+ | Stored procedures only |
| JSON serialisation | Newtonsoft.Json | Latest stable | Used in C# REST API handler |

**Compatibility Rules**:

- All WinForms components target `.NET Framework 4.8` — do not introduce .NET 5+ or .NET Core targets
- New shared utilities should target `.NET Standard 2.0` for maximum compatibility
- `Option Strict Off` is set in VB.NET projects — avoid relying on this; add explicit types in new code
- Compiled output goes to `C:\Pure\Application\` — project `OutputPath` must reflect this

---

## Naming Conventions

### Files and Projects

| Item | Convention | Example |
|------|-----------|---------|
| VB.NET business component | `b[Module][Feature].vbproj` | `bCLMCase.vbproj` |
| VB.NET interface component | `i[Module][Feature].vbproj` | `iCLMCaseHistory.vbproj` |
| C# library | `PascalCase.csproj` | `SSP.PureInsuranceRestAPIHandler.csproj` |
| VB source file | `PascalCase.vb` | `PaymentService.vb` |
| C# source file | `PascalCase.cs` | `ApiClient.cs` |
| SQL stored procedure file | `[prefix]_[table]_[action].sql` | follow existing `Procedures/` naming |

### Project Prefix Conventions (Critical)

| Prefix | Meaning | Language |
|--------|---------|---------|
| `b` | Business layer component | VB.NET |
| `i` | Interface / WinForms UI component | VB.NET |
| `d` | Data access object | VB.NET / C# |
| `PM*` | Pure Management (platform/architecture) | VB.NET / C# |
| `g*Library` | Global shared library (`gPMLibrary`, `gSIRLibrary`, `gCLMLibrary`, `gACTLibrary`) | VB.NET |
| `SSP.*` | SSP-authored shared/cross-cutting libraries | C# |
| `Sspi.*` | SSP internal shared libraries | C# |

### Classes and Types (VB.NET)

```vbnet
' Classes: PascalCase
Public Class PaymentService
Public Class InsuranceFile

' Interfaces: PascalCase with I prefix
Public Interface IPaymentRepository

' Enums: PascalCase
Public Enum PolicyStatus
    Active
    Lapsed
    Cancelled
End Enum

' Constants: UPPER_SNAKE_CASE
Public Const MAX_RETRY_COUNT As Integer = 3
Public Const DEFAULT_CURRENCY As String = "GBP"
```

### Classes and Types (C#)

```csharp
// Classes: PascalCase
public class ApiClient
public class TokenModel

// Interfaces: PascalCase with I prefix
public interface IApiClient

// Enums: PascalCase
public enum ClaimStatus { Open, Closed, Pending }

// Constants: UPPER_SNAKE_CASE
public const int MAX_RETRIES = 3;
```

### Functions and Variables

```vbnet
' VB.NET functions: PascalCase verb-noun
Public Function GetInsuranceFile(fileId As Integer) As InsuranceFile
Public Sub ProcessPayment(paymentId As Integer)
Public Function CalculatePremium(risk As RiskRecord) As Decimal

' VB.NET variables: camelCase
Dim insuranceFile As InsuranceFile
Dim premiumAmount As Decimal
Dim isActive As Boolean
```

```csharp
// C# methods: PascalCase
public static T DeserializeJson<T>(string json)
public async Task<TokenModel> GetTokenAsync()

// C# local variables: camelCase
var tokenModel = new TokenModel();
string accessToken = string.Empty;
bool isExpired = false;
```

### Database Objects

```sql
-- Tables: snake_case, singular
insurance_file
party_insurer_risk
tax_group

-- Columns: snake_case
policy_number
created_date
is_active

-- Stored Procedures: follow existing convention in Procedures/ directory
-- Typically: usp_[table]_[action] or descriptive name
```

---

## Code Organisation

### Project / Folder Structure

```
[Feature Area]/
  Components/
    [ComponentName]/        <- one folder per component
      b[Module][Feature].vbproj   <- business layer
      i[Module][Feature].vbproj   <- UI layer (if applicable)
      *.vb source files
```

For shared C# libraries:
```
Sspi.Common/
  Sspi.Common.Aws/          <- AWS integration
Shared Files/               <- SharedFiles.vbproj (VB.NET shared utilities)
SSP.PureInsuranceRestAPIHandler/   <- REST API client handler
```

### Single File Layout (VB.NET)

```vbnet
' 1. Imports
Imports System.Data
Imports SharedFiles

' 2. Module/Class declaration
Public Class PaymentService

    ' 3. Constants
    Private Const PAYMENT_TIMEOUT As Integer = 30

    ' 4. Private fields
    Private _daoContext As dPMDAO

    ' 5. Constructor / Initialisation
    Public Sub New(daoContext As dPMDAO)
        _daoContext = daoContext
    End Sub

    ' 6. Public methods
    Public Function ProcessPayment(...) As Boolean
        ' ...
    End Function

    ' 7. Private helper methods
    Private Function ValidatePayment(...) As Boolean
        ' ...
    End Function

End Class
```

---

## Error Handling Patterns

### VB.NET

```vbnet
' Standard pattern: Try/Catch with logging
Try
    Dim result = _daoContext.ExecuteProcedure("usp_payment_create", params)
    Return result
Catch ex As SqlException
    ' Log with context — use Sirius event log where appropriate
    EventLog.WriteEntry("Pure", $"Payment failed: {ex.Message}", EventLogEntryType.Error)
    Throw
Catch ex As Exception
    EventLog.WriteEntry("Pure", $"Unexpected error in ProcessPayment: {ex.Message}", EventLogEntryType.Error)
    Throw
End Try
```

### C# (REST API Handler)

```csharp
try
{
    var response = await HttpClient.PostAsync(url, content);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsStringAsync();
}
catch (HttpRequestException ex)
{
    // Log and rethrow or return null/default
    throw;
}
catch (JsonException ex)
{
    return default(T);
}
```

**Rules**:

- Never swallow exceptions silently
- Log with sufficient context (method name, entity IDs)
- Do not log passwords, tokens, or PII
- Rethrow after logging unless recovery is possible

---

## Logging Standards

Pure Insurance uses the Windows Event Log and the Sirius `event_log` database table.

| Log Target | When to use |
|-----------|------------|
| Windows Event Log | System/infrastructure errors, service failures |
| `event_log` table (Sirius) | Business events, audit trail entries |
| Debug output (`Debug.Print`) | Development only — never in release builds |

**Rules**:

- Always include the entity ID (policy number, claim ID, party ID) in log messages
- Never log sensitive data: passwords, tokens, national insurance numbers, bank details
- Use `EventLogEntryType.Error` for exceptions, `EventLogEntryType.Information` for business events

---

## Comments and Documentation

```vbnet
' Use XML doc comments for public APIs
''' <summary>
''' Processes an insurance payment and updates the ledger.
''' </summary>
''' <param name="paymentId">The ID of the payment record.</param>
''' <returns>True if the payment was processed successfully.</returns>
Public Function ProcessPayment(paymentId As Integer) As Boolean
```

**Rules**:

- Explain *why*, not *what*, for complex business logic
- Reference the relevant ISS/PS ticket number in comments for non-obvious decisions: `' ISS-1234: ...`
- Remove commented-out code before committing
- Keep comments current — outdated comments are worse than none

---

## Data Access Rules

- **Stored procedures only** — never write inline SQL from application code
- Use `dPMDAO` or `dPMDAOBridge` for all database operations
- Pass parameters using parameterised ADO.NET commands — never concatenate SQL strings
- Transactions must be explicitly managed and always committed or rolled back

---

## Configuration

- Application settings live in `app.config` / `App.config`
- Environment-specific config (connection strings, service URLs) must be externalised — never hardcoded in source
- The `SolutionConfig` component manages system-level configuration at runtime
