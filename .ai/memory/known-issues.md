---
title: Known Issues and Technical Debt
description: Documented TODOs, hardcoded credentials, obsolete code, warning suppressions, and legacy patterns in Pure Insurance
ms.date: 2026-04-28
---

## Overview

This file documents known issues, technical debt, and deprecated patterns found in the codebase. Items are categorised by severity. This is a living document — update when new issues are discovered or existing ones are resolved.

---

## Critical Security Issues

### Hardcoded Credentials

These credentials are committed to source control and should be treated as compromised.

**1. dPMDAO database credentials (`dPMDAO.vb`)**

```vbnet
Public Const ACDefaultUser As String     = "SIRIUS"
Public Const ACDefaultPassword As String = "<REDACTED>"
Public Const ACDefaultSwiftUser As String     = "Swift"
Public Const ACDefaultSwiftPassword As String = "<REDACTED>"
```

These are fallback credentials used when no explicit connection string is provided. Any process running with these defaults has database access. Likely set in App.config in production — but the fallbacks are exploitable if App.config is absent or incorrect.

**2. Keycloak OAuth credentials (`TokenGeneration.cs`)**

```csharp
// User auth defaults — development/test values
{ "client_id",     "SSP-Red" },
{ "client_secret", "<REDACTED>" },
{ "username",      "sirius" },
{ "password",      "<REDACTED>" }
```

These appear to be development defaults. The client secret is visible in source control. Confirm whether the `SSP-Red` Keycloak client is active in any environment.

**Recommended action:** Audit whether any of these credentials are active in production. Rotate compromised credentials. Move to environment variables or secrets manager.

---

## TODO Comments

### Custom Controls

**`Custom Controls/SListBar/ListBar.vb`**

Multiple TODO comments across the file:

| Line | Comment Summary |
|------|----------------|
| 970 | Unimplemented method body |
| 2185 | Refactoring needed |
| 2365 | Missing documentation |
| 3317, 3327, 3345 | Unimplemented feature blocks |
| 4469, 4480, 4498 | Incomplete implementation |
| 5520 | Work still needed |

**`Custom Controls/SListBar/PopupCancel.vb`**

| Line | Comment Summary |
|------|----------------|
| 213 | Missing documentation |
| 475 | X button not implemented |

### Claims Components

**`Claims/Components/Loss Schedule/Interface/iCLMLossSchedule/frmAdd.vb`**

| Line | Comment Summary |
|------|----------------|
| 46 | Supplier name should be retrieved from party table — not implemented |
| 151 | Find Party integration missing |
| 179 | Find Party integration missing |

**`Claims/Components/Loss Schedule/Interface/iCLMLossSchedule/frmInterface.vb`**

| Line | Comment |
|------|---------|
| 89, 103, 107, 131, 272 | Logic unclear / commented-out code |

**`Claims/Components/User Controls/uctCLMReserve/uctCLMReserve.Designer.vb`**

```vbnet
'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
```

The list view helper is disabled due to a display conflict that was never resolved.

### Data Access

**`dPMDAO.vb`** — date format handling

```vbnet
' This ignores AM\PM from the time. hence needs a fix
```

12-hour time format AM/PM values are discarded when formatting dates for SQL. Any time stored as PM will be stored as AM. This is a data integrity risk for time-sensitive operations.

### Assembly Information

Approximately 8 projects still contain:

```vbnet
' TODO: Review the values of the assembly attributes
```

in their `AssemblyInfo.vb` files. These are never addressed.

---

## Obsolete Code Still in Place

The following obsolete items remain in the codebase — they compile but may produce deprecation warnings at call sites:

| File | Line | Obsolete Item | Note |
|------|------|--------------|------|
| `GIS Combined/GIS/Components/LookupManagement/cGISLookupManager/Lookup.vb` | 105 | `<Obsolete>` attribute on a public member | Replacement not documented |
| `SSP.Shared/Components/ArraysHelper.vb` | 59 | `<Obsolete("Please refactor calling code to use normal Visual Basic assignment")>` | Array helper that bypasses standard VB syntax |
| `GIS Combined/Product Builder/.../bGISPMUExtrasBusiness.vb` | 351, 358 | `'OBSOLETE` comments | Deprecated business logic still being called |
| `Web Portal/Nexus/Pure.Portals/Controls/CalendarLookup.ascx.vb` | 116–137 | `<Obsolete("This property is obsolete")>` | Properties not yet removed from web portal |

---

## Compiler Warning Suppressions

All VB.NET projects in the solution suppress the following warnings via `<NoWarn>`:

```xml
<NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>
```

**What these suppressed warnings mean:**

| Code | Warning | Implication |
|------|---------|------------|
| 42016 | Implicit conversion | A numeric type is being widened/narrowed without explicit cast |
| 41999 | Late binding | An object typed as `Object` is being called without compile-time type checking |
| 42017 | Variable declared without `As` | Implicit `Object` type — untyped variable |
| 42018 | Property/method without `As` | Untyped return — treated as `Object` at runtime |
| 42019 | Operand converted to `Object` | Implicit boxing of value types |
| 42032 | Overloaded method | Ambiguous overload resolution |
| 42036 | `Select Case` operand is `Object` | Late-bound switch — evaluated at runtime |
| 42020 | Function/property/operator definition without `As` | Untyped function return |
| 42021 | Sub or function without return type | Return type inferred |
| 42022 | Namespace or type `X` contains member `Y` that conflicts with built-in member | Name collision |

**Practical impact:** Late binding (42016, 41999) means runtime failures for type mismatches that would otherwise be caught at compile time. This is pervasive throughout the codebase.

---

## VB6 Compatibility Dependencies

### Artinsoft.VB6

**Reference in:** `SharedFiles.vbproj`

```xml
<Reference Include="Artinsoft.VB6, Version=3.0.420.107, ..."/>
```

**Imports used:**

```vbnet
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
```

This library is the Artinsoft VB6-to-.NET migration toolkit. Its presence means parts of the codebase still use VB6 runtime behaviour emulation including:

- `Screen` object emulation
- `App` object emulation
- VB6 file I/O compatibility functions
- Legacy collection types

Artinsoft.VB6 is a **commercial third-party library**. If the licence lapses, builds will fail.

### Microsoft.VisualBasic.Compatibility

`Microsoft.VisualBasic.Compatibility.VB6` is a deprecated Microsoft library (removed in .NET Core / .NET 5+). The codebase cannot be migrated to .NET 5+ without replacing all usages.

---

## Win32 P/Invoke Issues

**File:** `Shared Files/gPMLibrary/gPMFunctions.vb`

20+ `Declare Function` P/Invoke declarations targeting Win32 APIs with known problems:

| Issue | Example | Risk |
|-------|---------|------|
| `Integer` used for handles | `ByRef lphKey As Integer` | Should be `IntPtr`; will corrupt on 64-bit if handle > 2^31 |
| ANSI API variants ("A") | `Alias "RegQueryValueExA"` | Should use "W" (Unicode) variants on modern Windows |
| Fixed `Integer` sizes for 32-bit structs | `Declare Function RegOpenKeyEx ... ByVal hKey As Integer` | 32-bit only; will fail if process ever runs as 64-bit |

**Examples:**

```vbnet
Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (
    ByVal hKey As Integer,
    ByVal lpSubKey As String,
    ByVal ulOptions As Integer,
    ByVal samDesired As Integer,
    ByRef phkResult As Integer) As Integer

Declare Function RegQueryValueExString Lib "advapi32.dll" Alias "RegQueryValueExA" (
    ByVal hKey As Integer,
    ByVal lpValueName As String,
    ByVal lpReserved As Integer,
    ByRef lpType As Integer,
    ByVal lpData As String,
    ByRef lpcbData As Integer) As Integer
```

The application currently runs as 32-bit (`<Prefer32Bit>true</Prefer32Bit>`). The P/Invoke signatures will work in 32-bit mode but are not safe for future 64-bit migration.

---

## Option Strict Off (Pervasive)

All VB.NET projects use `Option Strict Off` (the default). Combined with the suppressed compiler warnings above, this means:

- Type coercions happen silently at runtime
- `String` to `Integer`, `Double` to `Integer`, `Object` to `Date` conversions all succeed without error until a value is out of range
- The `StringsHelper.ToDoubleSafe` pattern exists specifically because these conversions fail at runtime:

```vbnet
' Anti-pattern — requires ToDoubleSafe because sPartyType is typed as String
If StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeAgent Then
```

This should be a typed comparison but the pervasive use of `Object` makes safe conversion helpers necessary.

---

## Mixed Framework Targets

The solution mixes:

| Framework | Projects |
|-----------|---------|
| .NET Framework 4.8 | ~996 VB.NET projects |
| .NET Standard 2.0 | ~41 C# projects (`dPMDAO`, `dPMDAOBridge`, `SSP.*`, `Sspi.*`, `Sirius.Achitecture.*`) |

There is **no .NET 5+ or .NET 6+ project** in the solution. The .NET Standard 2.0 projects were likely introduced to allow VB.NET (.NET 4.8) and newer libraries to share code, but the core application cannot move to .NET 6+ without addressing the VB6 compatibility and COM interop dependencies.

---

## COM and DCOM Architecture Debt

Distributed components communicate via DCOM (Distributed COM). This means:

- Component registration via `regsvr32` or `COM+` services is required for deployment
- DCOM configuration via `dcomcnfg` for remote components
- 32-bit COM components cannot be directly consumed from 64-bit processes
- No modern service discovery or load balancing — fixed DCOM server configuration

This is a significant modernisation constraint. Replacing DCOM with gRPC, REST, or message-based communication would require substantial rearchitecting.

---

## Hardcoded Output Path

**In project files:**

```xml
<OutputPath>C:\Pure\Application\</OutputPath>
```

All compiled binaries output to `C:\Pure\Application\`. This path is hardcoded in project files and assumed by the Navigator XM and COM registration scripts. Changing the installation path requires updating every project file.

---

## Areas with Missing Documentation

| Area | Gap |
|------|-----|
| Navigator XM STEP `ComponentAction` values | No documentation of what integer values mean per component |
| Navigator XM step `Type` attribute (FF/DF) | No explanation of FF vs DF found in code or comments |
| `dPMDAO` transaction COM+ integration | The `bConnectionPooling` and COM+ transaction flags are undocumented internally |
| `SysOptConfig` table keys | No centralised list of valid keys and their purpose |
| Artinsoft.VB6 usage | No list of which VB6 compatibility APIs are in active use vs remnants |
| DRE extension `ComponentAction` codes | 11 DRE extension projects — no documentation of which codes trigger which extensions |
