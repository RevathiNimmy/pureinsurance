# Coding Standards — Pure Insurance

All code contributions must follow these standards to ensure consistency and quality.

**Last Updated**: 2026-04-28

---

## Overview

Pure Insurance is predominantly a VB.NET / .NET Framework 4.8 WinForms application. New shared libraries and integration components use C# / .NET Standard 2.0. These standards apply to both languages unless noted.

---

## General Principles

- **Readability**: Code is read far more often than written — favour clarity over brevity
- **Consistency**: Follow established patterns in the surrounding component
- **Simplicity**: Prefer simple, clear solutions; avoid unnecessary abstraction
- **No inline SQL**: All database access must use stored procedures via `dPMDAO` or `dPMDAOBridge`

---

## Language-Specific Standards

### VB.NET (.NET Framework 4.8) — Primary Language

**Naming Conventions:**

| Element | Convention | Example |
|---------|-----------|---------|
| Classes, Modules | PascalCase | `PaymentService`, `InsuranceFile` |
| Interfaces | PascalCase with `I` prefix | `IPaymentRepository` |
| Enums | PascalCase | `PolicyStatus`, `ClaimType` |
| Public methods/functions | PascalCase | `GetInsuranceFile`, `ProcessPayment` |
| Private methods | PascalCase | `ValidatePayment` |
| Parameters | camelCase | `policyId`, `partyRecord` |
| Local variables | camelCase | `insuranceFile`, `premiumAmount` |
| Module-level fields | `_camelCase` with underscore prefix | `_daoContext`, `_currentUser` |
| Constants | UPPER_SNAKE_CASE | `MAX_RETRY_COUNT`, `DEFAULT_CURRENCY` |
| Boolean variables | camelCase with `is`/`has`/`can` prefix | `isActive`, `hasPermission` |

**Code Style:**

- Indentation: 4 spaces (no tabs)
- Always specify access modifiers explicitly (`Public`, `Private`, `Friend`)
- Use `Option Explicit On` — always declare variables before use
- Prefer explicit type declarations over inferred types in new code despite `Option Strict Off`
- Use `Using` statements for `IDisposable` objects
- One class or module per file

**File Organisation:**

```vbnet
' 1. Option statements
Option Strict Off
Option Explicit On

' 2. Imports (grouped: system, then project)
Imports System.Data
Imports Microsoft.VisualBasic
Imports SharedFiles

' 3. Namespace (optional — match project namespace)
Namespace PureInsurance.Claims

' 4. Class declaration
Public Class ClaimService

    ' 5. Constants
    Private Const CLAIM_LOCK_TIMEOUT As Integer = 30

    ' 6. Private fields
    Private _dao As dPMDAO

    ' 7. Constructor / New
    Public Sub New(dao As dPMDAO)
        _dao = dao
    End Sub

    ' 8. Public interface methods
    Public Function OpenClaim(claimRef As String) As Boolean
        ' ...
    End Function

    ' 9. Private helpers
    Private Function ValidateClaimRef(claimRef As String) As Boolean
        ' ...
    End Function

End Class

End Namespace
```

**Error Handling (VB.NET):**

```vbnet
Try
    Dim result = _dao.ExecuteProcedure("usp_claim_open", params)
    Return result
Catch ex As SqlException
    ' Log with context
    LogError($"SQL error opening claim {claimRef}: {ex.Message}", ex)
    Throw
Catch ex As Exception
    LogError($"Unexpected error opening claim {claimRef}: {ex.Message}", ex)
    Throw
End Try
```

---

### C# (.NET Standard 2.0) — Shared Libraries and Integration

**Naming Conventions:**

| Element | Convention | Example |
|---------|-----------|---------|
| Classes | PascalCase | `ApiClient`, `TokenModel` |
| Interfaces | PascalCase with `I` prefix | `IApiClient` |
| Public methods | PascalCase | `GetTokenAsync`, `DeserializeJson` |
| Private methods | PascalCase | `RefreshTokenAsync` |
| Parameters | camelCase | `requestUrl`, `timeoutSeconds` |
| Local variables | camelCase | `tokenModel`, `accessToken` |
| Private fields | `_camelCase` | `_httpClient`, `_tokenLock` |
| Constants | UPPER_SNAKE_CASE | `MAX_RETRIES`, `TOKEN_CACHE_KEY` |
| Properties | PascalCase | `AccessToken`, `IsExpired` |

**Code Style:**

- Indentation: 4 spaces
- Always use braces `{}` for `if`/`for`/`while` blocks — never omit for single-line bodies
- Use `var` where the type is obvious from the right-hand side; use explicit types otherwise
- Prefer `async`/`await` over `.Result`/`.Wait()` for async code
- Use `using` statements or `using` declarations for `IDisposable` objects
- One class per file; file name must match class name

**File Organisation:**

```csharp
// 1. System usings first, then third-party, then project
using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace SSP.PureInsuranceRestAPIHandler
{
    // 2. Class declaration
    public static class ApiClient
    {
        // 3. Constants
        private const string TokenCacheKey = "access_token";

        // 4. Private static fields
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly object _tokenLock = new object();

        // 5. Public static properties / methods
        public static async Task<T> GetAsync<T>(string url)
        {
            // ...
        }

        // 6. Private helpers
        private static async Task RefreshTokenAsync()
        {
            // ...
        }
    }
}
```

**Error Handling (C#):**

```csharp
try
{
    var response = await _httpClient.GetAsync(requestUrl);
    response.EnsureSuccessStatusCode();
    var json = await response.Content.ReadAsStringAsync();
    return DeserializeJson<T>(json);
}
catch (HttpRequestException ex)
{
    // Log and propagate — do not silently swallow
    throw;
}
catch (JsonException)
{
    return default;
}
```

---

## Database Standards (T-SQL Stored Procedures)

- **Naming**: Follow existing convention in `Databases/Pure/Procedures/` — prefix with domain area
- **Parameters**: Always use named, typed parameters — never dynamic SQL concatenation
- **Transactions**: Explicitly `BEGIN TRAN` / `COMMIT` / `ROLLBACK`; handle errors with `TRY/CATCH`
- **`SET` options**: Include `SET QUOTED_IDENTIFIER ON` and `SET ANSI_NULLS ON` at the top of each procedure
- **Author header**: Add the standard header comment block (see `PURE_STRUCTURE.sql` template)

```sql
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

-- ******************************************************************************
-- * <ISSxxx>: Brief description
-- * Author:   [Your name]
-- * Date:     [Date]
-- ******************************************************************************
CREATE PROCEDURE usp_claim_open
    @p_claim_ref    VARCHAR(20),
    @p_user_id      INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- procedure body

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH;
END
GO
```

---

## Code Quality

### Comments

- Use XML doc comments (`'''` in VB.NET, `///` in C#) for all public APIs
- Reference the ISS/PS ticket number for non-obvious decisions: `' ISS-1234: reason for this approach`
- Explain *why*, not *what*
- Remove commented-out code before committing

### Security

- **No hardcoded credentials**: Connection strings, API keys, passwords, and tokens must be externalised to `app.config` or system registry — never in source code
- **Parameterised database calls only**: No string concatenation of SQL
- **No PII in logs**: Do not log national insurance numbers, bank account details, passwords, or tokens
- Input validation must occur at the component boundary before passing data to `dPMDAO`

### Build Output

- All compiled output must target `C:\Pure\Application\` as the `OutputPath`
- Do not introduce new output paths without team agreement
- Avoid introducing `#if DEBUG` blocks that affect production behaviour

---

## Logging Standards

| Log Target | When to use |
|-----------|------------|
| Windows Event Log | Infrastructure errors, service failures, critical application errors |
| `event_log` table (Sirius DB) | Business events, user actions, audit trail entries |
| `Debug.Print` | Development only — must not appear in release builds |

Always include the relevant entity identifier (policy number, claim reference, party ID) in log messages.
