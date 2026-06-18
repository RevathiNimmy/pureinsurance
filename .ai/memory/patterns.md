---
title: Code Patterns
description: Actual error handling, logging, authentication, configuration, and navigation patterns extracted from the Pure Insurance codebase
ms.date: 2026-04-28
---

## Overview

This document captures patterns extracted from the actual codebase. Where multiple different patterns exist in different areas, both are documented. These are descriptive (what IS) not prescriptive (what should be).

---

## Error Handling

### VB.NET Business Layer Pattern

The standard pattern in business components (`b*` projects) uses structured Try/Catch/Finally with logging via `PMFunctions.LogMessageToFile`.

**Extracted from `iPMUAccumulationValuesCls.vb` (Sirius For Underwriting):**

```vbnet
Try
    Set m_oAccumulationValues = New bPMUAccumulationValues.bPMUAccumulationValues
    ' ... business logic ...
    Initialise = PMConstants.PMEReturnCode.PMSuccess
Catch excep As System.Exception
    PMFunctions.LogMessageToFile(
        sUsername:=ACApp,
        iType:=PMConstants.PMELogLevel.PMLogOnError,
        sMsg:="Error in Initialise",
        vApp:=ACApp,
        vClass:=ACClass,
        vMethod:="Initialise",
        excep:=excep)
    Initialise = PMConstants.PMEReturnCode.PMError
Finally
    ' Cleanup
End Try
```

**Key characteristics:**
- `ACApp`, `ACClass`, `ACApp` are module-level constants identifying the component — used in all log calls
- Return values use `PMConstants.PMEReturnCode` enum (`PMSuccess`, `PMError`, etc.) rather than exceptions propagating to the caller
- `Finally` used for cleanup (COM object release, resource cleanup)
- Exception type is usually `System.Exception` — specific SQL exceptions handled in `dPMDAO`

### dPMDAO Error Handling Pattern

`dPMDAO` handles SQL-level errors and logs them with full DB context.

**From `dPMDAO.vb`:**

```vbnet
Public Sub LogDatabaseError(
        ByRef sSiriusUsername As String,
        ByRef sCallingAppName As String,
        ByRef iSourceID As Integer,
        ByRef iLanguageID As Integer,
        ByRef bConnectionPooling As Boolean,
        ByRef iType As Integer,
        ByRef sMsg As String,
        Optional ByRef vApp As String = "",
        Optional ByRef vClass As String = "",
        Optional ByRef vMethod As String = "",
        Optional ByRef vErrNo As Object = Nothing,
        Optional ByRef vErrDesc As String = "",
        Optional ByRef vErrSource As Object = Nothing,
        Optional ByRef oCon As SqlConnection = Nothing,
        Optional ByRef oCmd As SqlCommand = Nothing,
        Optional ByRef lError As Integer = 0)

    lError = PMConstants.PMEReturnCode.PMError
    sMsg = "****" & vbCrLf & sMsg

    ' Log SP parameters
    LogParameters(oCmd, sParams)

    ' Mask credentials
    sMsg = sMsg.Replace("Password=", "*********")

    ' Append context
    sMsg = sMsg & " Sirius username: (" & sSiriusUsername & ")"
    sMsg = sMsg & " Calling App Name: (" & sCallingAppName & ")"
    sMsg = sMsg & " SourceID: (" & CStr(iSourceID) & ")"

    LogDatabaseMessage(iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc)
End Sub
```

**Deadlock handling:** `ACDeadlock = -2147467259` constant is used to detect deadlock errors for retry logic in `dPMDAO`.

### C# Error Handling Pattern

**Extracted from `ApiClient.cs` (SSP.PureInsuranceRestAPIHandler):**

```csharp
try
{
    var response = HttpClient.PostAsync(_tokenUrl, content).Result;
    if (response.IsSuccessStatusCode)
    {
        var json = response.Content.ReadAsStringAsync().Result;
        // process response
    }
}
catch (Exception ex)
{
    // Errors here result in null/default return — not re-thrown
    // Caller must check for null response
}
```

**Note:** The C# REST API client tends to swallow exceptions and return null/default. Callers must null-check responses. This is inconsistent with the VB.NET pattern of explicit error codes.

---

## Logging

### Logging Infrastructure

**Framework:** Microsoft Enterprise Library 5.0 Logging Block

**Imports (from `gPMFunctions.vb`):**

```vbnet
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Filters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
```

**Central log writer (from `gPMFunctions.vb`):**

```vbnet
Public Module gPMFunctions
    Public Property LogWriter() As LogWriter
        Get
            Return m_LogWriter
        End Get
        Private Set(value As LogWriter)
            m_LogWriter = value
        End Set
    End Property
    Private m_LogWriter As LogWriter
End Module
```

### Log Levels (PMELogLevel enum)

From `gPMConstants.vb` (in `Shared Files/gPMLibrary/`):

```vbnet
Public Enum PMELogLevel
    PMLogOnError = 1
    PMLogDebug   = 2
    PMLogInfo    = 3
    PMLogWarning = 4
End Enum
```

### Logging Call Pattern

All logging goes through `PMFunctions.LogMessageToFile` with named parameters:

```vbnet
PMFunctions.LogMessageToFile(
    sUsername:="<sirius-username>",
    iType:=PMConstants.PMELogLevel.PMLogOnError,
    sMsg:="<descriptive message>",
    vApp:=ACApp,        ' Module-level constant: name of the app/component
    vClass:=ACClass,    ' Module-level constant: class name
    vMethod:="<method>",
    excep:=excep)
```

### Web Services Logging Pattern

In legacy WCF test harness pages:

```vbnet
WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
```

This pattern logs the page name, service name, operation, and start/end timestamps for performance tracking.

### Log Destinations

| Destination | When Used |
|------------|-----------|
| Enterprise Library log (file/event log) | Business errors, DB errors, infrastructure failures |
| `event_log` database table | Business events, user actions requiring audit trail |
| Windows Event Log | Infrastructure/service-level errors (via `advapi32.dll` P/Invoke in `gPMFunctions.vb`) |
| `Debug.Print` | Development only — used in legacy code, must not reach production |

### ACApp / ACClass Constants Pattern

Every VB.NET component module declares identifying constants used in all log calls:

```vbnet
Private Const ACApp As String = "iPMUAccumulationValues"
Private Const ACClass As String = "iPMUAccumulationValuesCls"
```

This provides a consistent way to identify the source of any log entry without stack trace inspection.

---

## Authentication and Authorisation

### User Authentication (Primary — WinForms)

**Mechanism:** Thinktecture IdentityServer v2 (`STS/SSP.SecureTokenService/`)

Users log in via the `iLogonManager` / `iLogonServer` components in `Sirius Architecture/`. The logon process:

1. User enters credentials in `iLogonManager` WinForms dialog
2. Credentials validated against STS token endpoint
3. JWT issued and stored in session
4. `bPMLicenceManager` checks licence validity

### Service-to-Service Authentication

**Mechanism:** OAuth 2.0 Client Credentials Grant via Keycloak

```csharp
// From TokenGeneration.cs
string clientSecret = Environment.GetEnvironmentVariable(
    "CLIENT_SECRET", EnvironmentVariableTarget.Machine);

var form = new Dictionary<string, string>
{
    { "grant_type",    "client_credentials" },
    { "client_id",     ClientId },
    { "client_secret", clientSecret }
};
```

**Token provider:** `http://ps-altova-ls01/realms/SSPStandard/protocol/openid-connect/token`

### Token Refresh (REST API Client)

```csharp
// From ApiClient.cs — called automatically before each API request
private static void RefreshAccessTokenAsync()
{
    var formParams = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string, string>("grant_type",    "refresh_token"),
        new KeyValuePair<string, string>("refresh_token", _refreshToken),
        new KeyValuePair<string, string>("client_id",     _tokenModel.ClientId),
        new KeyValuePair<string, string>("client_secret", _tokenModel.ClientSecret)
    };

    var response = HttpClient.PostAsync(_tokenUrl, formParams).Result;

    if (response.IsSuccessStatusCode)
    {
        lock (_tokenLock)
        {
            _accessToken = tokenResponse.access_token;
            _refreshToken = tokenResponse.refresh_token;
            _accessTokenExpiry = DateTime.Now.AddSeconds(tokenResponse.expires_in);
        }
        OnTokenRefreshed?.Invoke(_tokenModel);  // Sync back to session
    }
}
```

**Token lifetime:** Access token expiry tracked by `_accessTokenExpiry`. Refresh triggered when `DateTime.Now > _accessTokenExpiry`.

### Authorisation

Authorisation is enforced in the business layer via `UserAuthorityOptions` and `UserCanDoTasks` enums. The `PMUser` and `PMUserGroup` components in `Sirius Architecture/` manage user permissions. Orion has its own `UserAuthorities` component.

---

## Configuration Management

### Registry-Based Configuration (Primary — Legacy)

Most client-side configuration is stored in the Windows Registry. Access is via Win32 API P/Invoke declared in `gPMFunctions.vb`:

```vbnet
Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (...)
Declare Function RegQueryValueExString Lib "advapi32.dll" Alias "RegQueryValueExA" (...)
Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" (...)
```

**Key registry constants (from `gPMRegConst.vb` / `gPMConstants.vb`):**

```vbnet
Public Const PMRegArchitectureDebug As String        = "Architecture In Debug"
Public Const PMRegArchitectureLocalEnabled As String  = "Architecture Local Enabled"
Public Const PMRegArchitectureServerEnabled As String = "Architecture Server Enabled"
Public Const PMRegQueryTimeoutSeconds As String       = "QueryTimeoutSeconds"
Public Const PMRegSplashBitMap As String             = "SplashBitMap"
Public Const PMRegWorkManagerNewsTab As String        = "Work Manager News Tab"
Public Const PMRegWorkManagerCaption As String        = "Work Manager Main Form Caption"
Public Const PMRegPMSupportWebAddress As String       = "PM Support Web Address"
```

### App.Config / Web.Config

Used for:
- Database connection strings (consumed by `dPMDAO`)
- Enterprise Library logging configuration (TraceListeners, formatters, log categories)
- WCF service bindings and endpoints
- AppSettings key-value pairs

### Database-Stored Configuration

The `SysOptConfig` / `SysOptConfigS4I` tables store system-wide configuration options. Changed via `TScriptUpdateSysOption.sql` scripts.

### Environment Variables (Service-Level Secrets)

The `CLIENT_SECRET` environment variable (machine-level) is used for service-to-service OAuth credentials. This is the only externalized secret pattern found — all other credentials are in config files or (problematically) in source code.

---

## Navigator XM Workflow Pattern

The `Navigator XM` engine drives all main business workflows without code changes — only XML roadmap edits are needed.

**XML structure (`Navigator XM Roadmaps/PFNEWQUOTE.XML`):**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE MAP SYSTEM "navigatorxmv2.dtd">
<MAP WMTaskCode=""
     WMTaskDescription="Complete Instalments Quote"
     TransactionType="REN"
     ProcessMode="0"
     AutoClose="False"
     NavigatorDriven="True"
     RoadmapName="Instalments Quote"
     Core="1"
     Version="200"
     ElementID="E1">

    <STEP Description="Select Finance Transactions"
          Component="iPMBFinanceTransactions.NavigatorV3"
          Type="FF"
          CancelAction="AP"
          OKAction="F1"
          OKSteps="0"
          CancelSteps="0"
          ComponentAction="2"
          ServerSide="False"
          CreateWMTask="True"
          Core="1"
          ElementID="E2"/>

    <STEP Description="Quote Finance"
          Component="iPMBFinancePlanQuote.NavigatorV3"
          Type="DF"
          CancelAction="AP"
          OKAction="F1"
          ComponentAction="1"
          ElementID="E3"/>
</MAP>
```

**Key attributes:**

| Attribute | Meaning |
|-----------|---------|
| `TransactionType` | Business transaction type (REN=Renewal, NB=New Business, etc.) |
| `NavigatorDriven` | Navigator engine controls step flow |
| `Component` | COM component ProgID invoked for this step |
| `Type` | Step type: FF (form-first?), DF (data-first?) |
| `OKAction` / `CancelAction` | Navigation: F1=Forward, AP=Abort Process |
| `ComponentAction` | Integer code passed to component to indicate which action to perform |
| `CreateWMTask` | Creates a Work Manager task for this step |
| `Core` | Whether this is a core step (1) or optional (0) |

**Implication:** Adding a new screen to a business workflow means adding a `<STEP>` to the relevant XML file — no VB.NET code change required for the navigation itself.

---

## COM / Component Interaction Pattern

Components communicate via COM interfaces. The `b*` business component is instantiated by the `i*` interface component. This is the primary architectural pattern:

```vbnet
' In interface component (i*)
Private m_oBusinessComponent As bXxx.bXxxClass

Public Function Initialise(...) As Integer
    Try
        Set m_oBusinessComponent = New bXxx.bXxxClass
        ' Pass context to business layer
        m_oBusinessComponent.Initialise(sSiriusUsername, oDAO, ...)
        Initialise = PMConstants.PMEReturnCode.PMSuccess
    Catch excep As System.Exception
        PMFunctions.LogMessageToFile(...)
        Initialise = PMConstants.PMEReturnCode.PMError
    End Try
End Function
```

The `Set` keyword and `New` pattern is a legacy VB6 artifact preserved in the VB.NET migration.

---

## Inconsistencies Between Areas

| Area | Pattern Used | Notes |
|------|-------------|-------|
| VB.NET business layer | Return codes (`PMEReturnCode`) | Explicit error codes; exceptions logged but not propagated |
| C# REST client | Null returns | Exceptions swallowed; callers must null-check |
| Web services test pages | `WriteToLog` with timestamps | Performance-focused logging pattern |
| Legacy COM components | `Err.Number` / `Err.Description` | VB6-style error handling in oldest code |
| Claims components | `StringsHelper.ToDoubleSafe` | Unsafe type conversions for comparisons — a known anti-pattern |
