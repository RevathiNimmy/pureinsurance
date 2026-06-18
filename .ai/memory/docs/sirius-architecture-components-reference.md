# Sirius Architecture — Components Reference

> Comprehensive reference for **all** components in `Sirius Architecture\Components\`.
> These VB.NET and C# projects provide core architecture-level services: authentication, licensing, database access, navigation/workflow, task management, user management, and infrastructure utilities.
> Referenced from `.github/copilot-instructions.md`.
> **See also:** `.github/docs/back-office-components-reference.md` for Sirius Back Office Core components (`bSIR*`, `bACT*`, `bGIS*`).

---

## Overview

The `Sirius Architecture\Components\` directory contains **85 projects** covering architecture-level infrastructure. This document covers every component organised by functional category.

### Naming Convention

| Prefix / Pattern | Layer | Examples |
|-------------------|-------|----------|
| `b*` | **Business** | `bLicenceManager`, `bObjectManager`, `bPMLicenceManager`, `bSIROverdueTaskCheck` |
| `d*` | **Data Access** | `dPMDAO`, `dPMDAOBridge` |
| `i*` | **Interface / UI** | `iChangePassword`, `iLogonManager`, `iLogonStatusManager` |
| `PM*` | **Platform Manager** | `PMUser`, `PMTask`, `PMWrkManager`, `PMLookup`, `PMSystem` |
| `Navigator*` | **Workflow** | `Navigator V3`, `Navigator XM` |
| `Sirius.*` | **Architecture Library (C#)** | `Sirius.Achitecture.Data`, `Sirius.Achitecture.Utility` |
| `SSP.*` | **Shared Service** | `SSP.Pure.UsersSync` |
| Standalone names | **Utility / Tool** | `LicenceAdmin`, `PasswordRehasher`, `ClientManager DCOM` |

### How Components Are Called

```
Desktop App (Back Office / Underwriting)
  -> bObjectManager (object factory)
      -> iLogonManager / bClientManager (authentication)
          -> bPMLicenceManager / bPMLocalLicenceManager / bPMRemoteLicenceManager
              -> PMSystem (system record & licence validation)
                  -> dPMDAO (database access)
                      -> SQL Server

Task / Workflow Processing
  -> PMWrkManager (work manager)
      -> PMWrkTaskInstance (task CRUD)
          -> PMLock (record locking)
              -> dPMDAO.Database → SQL Server
  -> Navigator V3 / Navigator XM (workflow navigation)
      -> PMPackageStep (workflow steps)

User & Security Management
  -> PMUser / PMUserGroup / PMUserMaintenance
      -> BPMUsersSync / SSP.Pure.UsersSync → KeyCloak REST API
      -> dPMDAO → SQL Server

Batch Scheduler
  -> bSIROverdueTaskCheck (console EXE)
      -> dPMDAO.Database → SQL Server
  -> PMTaskOutcomeBatch (batch task processor)
      -> dPMDAO.Database → SQL Server

Lookup & Reference Data
  -> PMLookup / PMMaintainLookup / PMProductLookup / PMCurrency
      -> dPMDAO → SQL Server
```

---

## Complete Project Inventory

### Business Components (`b*`)

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **bLicenceManager** | VB.NET | Class Library (COM) | Legacy licence limit checking and user logon |
| **bObjectManager** | VB.NET | Class Library (COM) | Desktop object factory, session state manager |
| **bPMLicenceManager** | VB.NET | Class Library | Licence manager interface + core implementation with licence file validation |
| **bPMLocalLicenceManager** | VB.NET | Class Library (COM) | Local proxy — delegates to `bPMLicenceManager` |
| **bPMRemoteLicenceManager** | VB.NET | Class Library (COM) | Remote proxy — delegates to `bPMLicenceManager` |
| **BPMUsersSync** | C# (.NET) | Class Library | KeyCloak user synchronisation (register, update, login, roles) |
| **bSIROverdueTaskCheck** | VB.NET | Console EXE | Batch process for overdue task escalation |
| **bSIRUserCompetenceTask** | VB.NET | Class Library (COM) | Navigator-driven competence task creator |

### Data Access Layer (`d*`)

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **dPMDAO** | VB.NET | Class Library (COM) | Core database abstraction layer (SQL Server connection, queries, transactions) |
| **dPMDAOBridge** | C# | Class Library | C# adapter wrapping `dPMDAO` for `Sirius.Architecture.Data` consumers |
| **dPMDAO_Old** | VB.NET | Archive | Empty / deprecated archive of original dPMDAO |

### Client & Authentication (`ClientManager*`, `i*`)

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **ClientManager DCOM** | VB.NET | Class Library (COM) | User authentication, session management, DCOM object creation |
| **ClientRegistry** | VB.NET | Class Library | CLI registry installer for Sirius applications |
| **iChangePassword** | VB.NET | WinForm | Password change dialog |
| **iLogonManager** | VB.NET | Class Library (COM) | User authentication via .NET Remoting (`MarshalByRefObject`) |
| **Ilogonserver** | VB.NET | Console EXE | TCP Remoting host for `iLogonManager` (port 65535−SessionId) |
| **iLogonStatusManager** | VB.NET | WinForm | Logon status notification UI |
| **iPMScreenMessage** | VB.NET | WinForm | Configurable screen message dialog |

### Licence & Security Utilities

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **LicenceAdmin** | VB.NET | Business + UI | Licence administration (get limits, update users) |
| **LicenceKeyGen** | VB.NET | Utility | Licence key encryption utility |
| **MigrateRegSettings** | VB.NET | Utility | Registry settings migration tool |
| **PasswordRehasher** | VB.NET | Console EXE | Legacy → new password encryption migration |

### Navigator / Workflow Framework

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **Navigator V3** | VB.NET | Class Library (COM) | Workflow navigation framework (maps, steps, processes) |
| **Navigator XM** | VB.NET | Class Library (COM) | XML-based roadmap workflow engine |
| **Workflow** (PMStepMaintenance) | VB.NET | Class Library | Workflow step definition CRUD |

### PM Infrastructure & UI

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMAbout** | VB.NET | WinForm | About dialog |
| **PMAutoNumber** | VB.NET | Class Library | Auto-number / reference generation |
| **PMBusy** | VB.NET | WinForm | Progress indicator |
| **PMCaption** | VB.NET | Class Library | Multi-language caption management |
| **PMClientInstallAdmin** | VB.NET | WinForm | Client installation admin UI |
| **PMClientInstallCheck** | VB.NET | Class Library | Client version verification |
| **PMDataControl** | VB.NET | Class Library | Database connection pooling |
| **PMDecision** | VB.NET | WinForm | Decision prompt dialog |
| **PMEventLogViewer** | VB.NET | WinForm | Windows event log viewer |
| **PMFormControl** | VB.NET | Class Library | Field validation framework |
| **PMInstallUnzipper** | VB.NET | Console EXE | Setup unzip utility |
| **PMPropertyManager** | VB.NET | Class Library | In-memory property store |
| **PMRegELSupport** | VB.NET | Utility | Event log DLL registration |
| **PMServerRegistry** | VB.NET | Class Library | Server registry reader |
| **PMSiriusLogViewer** | VB.NET | WinForm | Log file viewer with ZIP support |
| **PMSiriusSupport** | VB.NET | Utility | Support web page launcher |
| **PMSplash** | VB.NET | WinForm | Splash screen |
| **PMMsgBox** | VB.NET | WinForm | Standard message box |

### PM System & Configuration

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMSystem** | VB.NET | Class Library | System configuration (ICCS, licence, DB settings) |
| **PMCurrency** | VB.NET | Class Library | Currency management |
| **PMSource** | VB.NET | Class Library | Branch / office / source management |
| **PMSourceMaintenance** | VB.NET | WinForm | Source maintenance UI |
| **PMProduct** | VB.NET | Class Library | Product lookup |
| **PMProductClientInstall** | VB.NET | Class Library | Client install management per product |
| **PMProductLookup** | VB.NET | Class Library | Product-specific lookup tables |
| **ProductUpdateHistory** | VB.NET | Class Library | Product update history tracking |

### PM Lookup & Reference Data

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMLookup** | VB.NET | Class Library (COM) | Reference data engine (codes, descriptions, date-effective IDs) |
| **PMMaintainLookup** | VB.NET | WinForm | Lookup table maintenance UI |

### PM Communication & Logging

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMMAPI** | VB.NET | Class Library | Email via MAPI |
| **PMMessage** | VB.NET | Class Library | Logging to Windows event log |
| **PMMessageAdmin** | VB.NET | WinForm | Message admin UI |
| **PMEventTask** | VB.NET | Class Library | Event-driven task scheduling |

### PM Record Locking

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMLock** | VB.NET | Class Library (COM) | Pessimistic record locking (timestamp-based) |

### PM Task Management

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMTask** | VB.NET | Class Library | Task definitions |
| **PMTaskCategory** | VB.NET | Class Library | Task category management |
| **PMTaskGroup** | VB.NET | Class Library | Task group definitions |
| **PMTaskGroupCategory** | VB.NET | Class Library | Task group ↔ category assignments |
| **PMMaintainTask** | VB.NET | WinForm | Task definition maintenance UI |
| **PMMaintainTaskAction** | VB.NET | WinForm | Task action type CRUD UI |
| **PMMaintainTaskGroupAction** | VB.NET | WinForm | Task group action UI |
| **PMTaskGroupMaintenance** | VB.NET | WinForm | Task group maintenance UI |
| **PMTaskMaintenance** | VB.NET | WinForm | Task maintenance UI |
| **PMTaskOutcomeBatch** | VB.NET | Console EXE | Batch processor for overdue tasks |
| **PMPackageStep** | VB.NET | Class Library | Workflow step execution engine |
| **PMWrkManager** | VB.NET | Class Library (COM) | Work manager — task scheduling, retrieval, assignment |
| **PMWrkTaskInstance** | VB.NET | Class Library (COM) | Task instance CRUD, status, logging |

### PM User Management

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **PMUser** | VB.NET | Class Library (COM) | Core user management (logon, auth modes, CRUD) |
| **PMUserGroup** | VB.NET | Class Library (COM) | User group hierarchy and permissions |
| **PMUserMaintenance** | VB.NET | WinForm | User admin UI with AD/LDAP support |
| **PMTestLogon** | VB.NET | Utility | Test logon utility |

### Sirius Architecture Libraries (C#)

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **Sirius.Achitecture.Configuration.Local** | C# | Class Library | Strongly-typed Windows Registry access (VB6-compatible) |
| **Sirius.Achitecture.Data** | C# | Class Library | SQL Server command abstraction (`SiriusCommand`, `SiriusConnection`) |
| **Sirius.Achitecture.Data.BackOffice** | C# | Class Library | dPMDAO bridge for back-office database connections |
| **Sirius.Achitecture.Utility** | C# | Class Library | Type casting, NULL handling, data conversion utilities |
| **Sirius.Architecture.Data** | C# | Class Library | Abstract `SiriusConnection` base class |

### Shared Services & Sync

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **SSP.Pure.UsersSync** | C# | Class Library | KeyCloak user synchronisation (REST API client) |
| **Sirius Cache Controller** | VB.NET | Business + UI | Application cache management (XML files) |

### Applications & User Controls

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **UsersLoggedOn** | VB.NET | WinForm EXE | Display currently logged-on users |
| **User Controls** (uSIRCommonControls) | VB.NET | User Controls | Common UI controls (Anchor, Divider) |
| **PMZipper** | VB.NET | Class Library | ZIP compression / decompression |

### Utilities

| Component | Language | Type | Purpose |
|-----------|----------|------|---------|
| **_Utilities** (CreateEnvironmentVariables) | C# | Console EXE | Set environment variables (Windows/Linux) |

**Totals:** 85 component projects.

---

## Component Reference

### bLicenceManager
**Directory:** `bLicenceManager/`
**COM ProgId:** `LicenceManager_NET.LicenceManager`
**Purpose:** Legacy licence management component. Checks the system licence limit, validates licence keys, and provides user logon by delegating to `bClientManager`. Used in older desktop application startup flows before `bPMLicenceManager` was introduced.

**Key Classes:**
- `LicenceManager` — main business class
- `MainModule` — module constants (`ACApp = "bLicenceManager"`, `ACClientManager = "bClientManager.ClientManager"`)

**Key Methods (4):**

| Method | Signature | Description | Components/SPs Called |
|--------|-----------|-------------|----------------------|
| `Initialise` | `() As Integer` | Entry point for object initialisation. Calls `CheckLicenceLimit` to verify the system has a valid licence and the concurrent user limit is not exceeded. | `CheckLicenceLimit` (private) |
| `Logon` | `(v_sUsername, v_sPassword, ByRef r_sClientSystemName, ByRef r_oClientManager) As Integer` | Logs a user onto the system. Checks licence limit first, then gets a `bClientManager` instance, and calls its `Logon` method with the provided credentials. On failure, disposes the client manager. | `CheckLicenceLimit` (private), `GetClientManager` (private), `bClientManager.ClientManager.Logon` |
| `Dispose` | `()` | Releases resources (IDisposable). | — |

**Private Methods (2):**

| Method | Description | Components/SPs Called |
|--------|-------------|----------------------|
| `GetClientManager` | Creates a new `bClientManager.ClientManager` instance and calls its `Initialise` with source, country, language, log level, currency, and calling app name. | `bClientManager.ClientManager.Initialise` |
| `CheckLicenceLimit` | Creates a `bPMSystem.Business` instance, calls `GetValidSystem` to retrieve the system record (licence limit, licence key, pool size, home country), then calls `GetLicencesInUse` to check whether the concurrent licence limit has been exceeded. Returns `PMInvalidLicenceKey` or `PMLicenceExceeded` on failure. | `bPMSystem.Business.Initialise`, `bPMSystem.Business.GetValidSystem`, `bPMSystem.Business.GetLicencesInUse` |

**Stored Procedures:** None directly (delegates to `bPMSystem`).

**References:** `bClientManager`, `bPMSystem`, `SharedFiles`, `gPMConstants`

---

### bObjectManager
**Directory:** `bObjectManager/`
**COM ProgId:** `ObjectManager_NET.ObjectManager`
**Purpose:** Central **object factory and session state manager** for desktop back-office applications. Manages user authentication state, creates/initialises business and interface objects (local or via `bClientManager`), handles logon server process lifecycle, and checks for client installation updates. All desktop apps (Navigator, back-office screens) use `bObjectManager` as their entry point.

**Key Classes:**
- `ObjectManager` — main class (inherits `ComponentClassHelper`, implements `IDisposable`)
- `MainModule` — module constants (`ACApp = "bObjectManager"`)

**Key Properties (12):**

| Property | Type | Access | Description |
|----------|------|--------|-------------|
| `UserName` | `String` | ReadOnly | Currently authenticated username |
| `Password` | `String` | ReadOnly | User password |
| `UserID` | `Integer` | ReadOnly | Authenticated user ID |
| `LanguageID` | `Integer` | ReadOnly | User language ID |
| `SourceID` | `Integer` | ReadOnly | User source/branch ID |
| `CountryID` | `Integer` | ReadOnly | User home country ID |
| `CurrencyID` | `Integer` | ReadOnly | User currency ID |
| `LogLevel` | `Integer` | ReadOnly | Logging verbosity level |
| `PartyCnt` | `Integer` | ReadOnly | User's party count/key |
| `GenericConnectionStatus` | `Boolean` | ReadOnly | Whether the user is logged on to the broking link (via `iLogonManager`) |
| `LoggedOnLocally` | `Boolean` | ReadOnly | Whether the user is logged on locally (via `iLogonManager`) |
| `UserConfigXMLDataSet` | `String` | Read/Write | User-specific XML configuration dataset |

**Key Methods (7):**

| Method | Signature | Description | Components/SPs Called |
|--------|-----------|-------------|----------------------|
| `Initialise` | `(ByRef sCallingAppName As String) As Integer` | Main entry point. Sets calling app name, ensures `iLogonServer.exe` process is running, gets a `bClientManager` instance via `GetClientManager`, retrieves user state (username, password, userID, source, country, language, currency, etc.) from either the `iLogonManager` (stateless) or `bClientManager` (stateful). Handles `PMMAlreadyInUse` for branch selection in progress. | `GetClientManager` (private), `iLogonManager.GetPropertyValues`, `bClientManager.GetPropertyValues` |
| `InitialiseWithUserState` | `(sUsername, sPassword, iUserID, iSourceID, iCountryID, iLanguageId, iLogLevel, iCurrencyID, lPartyCnt, sCallingAppName, [sServerPrinter], [iIsPrinterChangeable]) As Integer` | Initialises the ObjectManager with explicit user state values (no interactive logon). Used by batch/scheduler processes. Sets `m_bStatelessClientManager = True`. | — (direct property assignment) |
| `GetInstance` | `(ByRef oObject, ByRef sClassName, [vInstanceManager]) As Integer` | **Core object factory.** Creates or retrieves an instance of the named class. Supports three modes via `vInstanceManager`: **PMGetLocalInterface** (creates locally, calls `Initialise()`), **PMGetLocalBusiness** (creates locally, calls `Initialise` with full user state), **PMGetViaClientManager** (creates via `bClientManager.GetInstance` for remote/DCOM objects). Uses `CreateLateBoundObject` for late-bound COM instantiation. | `CreateLateBoundObject`, `bClientManager.GetInstance` |
| `AddMessage` | `(iType, sMsg, [vApp], [vClass], [vMethod], [vErrNo], [vErrDesc])` | Wrapper for logging. Creates `iPMMessage.PMMessage`, initialises it, and logs the message. Falls back to `gPMFunctions.LogMessagePopup` if PMMessage is unavailable. | `iPMMessage.PMMessage.Initialise` |
| `CheckClientInstall` | `(v_lPMEProductFamily As Integer) As Integer` | Checks if the installed client version needs updating by delegating to `iLogonManager.CheckClientInstall`. | `iLogonManager.CheckClientInstall` |
| `SetUserConfigXML` | `(sUserConfigXMLDataSet As String) As Integer` | Sets user configuration XML on the underlying `bClientManager` instance. | `bClientManager.UserConfigXMLDataSet` (property set) |
| `Dispose` | `()` | Cleans up resources: disposes `iPMMessage`, releases `bClientManager`, decrements `iLogonManager.AppReferenceCount`, kills orphaned `PMWorkManager` and `Ilogonserver` processes if no `iLogonStatusManager` is running in the current session. | — |

**Private Methods (2):**

| Method | Description | Components/SPs Called |
|--------|-------------|----------------------|
| `GetClientManager` | Connects to `iLogonManager` via .NET Remoting (`tcp://localhost:{port}/SSP`), checks if logon is in progress, obtains a `bClientManager` instance via `iLogonManager.Initialise`, copies user state properties from `iLogonManager` to `bClientManager`. Special handling for `iFieldManager` calling app (creates `bClientManager` directly). | `iLogonManager` (Remoting), `bClientManager.ClientManager.Initialise`, `iLogonManager.Initialise` |
| `LogMessage` | Private wrapper that logs to file via `gPMFunctions.LogMessageToFile`, falls back to `gPMFunctions.LogMessagePopup`. | `gPMFunctions.LogMessageToFile`, `gPMFunctions.LogMessagePopup` |

**Constructor (`New`):** Ensures `Ilogonserver.exe` is running in the current terminal session. Reads registry for `PMDIR` to locate the Pure application directory.

**Stored Procedures:** None directly.

**References:** `bClientManager`, `iLogonManager`, `iPMMessage`, `SharedFiles`, `gPMConstants`, `gPMFunctions`

---

### bPMLicenceManager
**Directory:** `bPMLicenceManager/`
**COM ProgId:** `LicenceManager_CoClass_NET.LicenceManager_CoClass`
**Purpose:** **Core Policy Master licence management** — defines the `LicenceManager` interface and its primary implementation (`LicenceManager_CoClass`). Validates licence files using `Standard.Licensing`, checks concurrent user limits, and manages user logon. Also includes `LicenseManager` (licence file validation engine) and `SecureHash` (SHA-256 hashing).

**Key Classes/Modules:**
- `LicenceManager` (Interface) — defines `Initialise`, `Logon`, `Dispose`, `WarningMessage`, `ErrorMessage`
- `LicenceManager_CoClass` — main implementation
- `LicenseManager` — licence file loading, validation, and parsing
- `SecureHash` (Module) — SHA-256 hash computation

**Files:**
- `bPMLicenceManagerCls.vb` — Interface + `LicenceManager_CoClass`
- `LicenseManager.vb` — `LicenseManager` class (licence file validation)
- `SecureHash.vb` — `SecureHash` module (SHA-256)
- `bPMLicenceManager.vb` — `MainModule` (constants)

#### LicenceManager Interface

| Member | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `() As Integer` | Check licence validity and user limits |
| `Logon` | `(v_sUsername, v_sPassword, ByRef r_sClientSystemName, ByRef r_bPMBLinkRequired, ByRef r_oClientManager) As Integer` | Authenticate user and return client manager |
| `Dispose` | `()` | Release resources |
| `WarningMessage` | `Property As String` | Licence expiration warning text |
| `ErrorMessage` | `Property As String` | Licence validation error text |

#### LicenceManager_CoClass — Key Methods (4)

| Method | Signature | Description | Components/SPs Called |
|--------|-----------|-------------|----------------------|
| `Initialise` | `() As Integer` | Calls `CheckLicenceLimit` to validate the licence file and check concurrent user limits. Populates `ErrorMessage` and `WarningMessage` properties. | `CheckLicenceLimit` (private) |
| `Logon` | `(v_sUsername, v_sPassword, ByRef r_sClientSystemName, ByRef r_bPMBLinkRequired, ByRef r_oClientManager) As Integer` | Validates licence limit (with `ignoreMessage=False`), then gets a `bClientManager` instance via `GetClientManager`, and calls `r_oClientManager.Logon`. Handles `PMUserPasswordExpired`, `PMUserTemporaryPassword`, `PMUserWeakPassword`, and `PMNewBuildUpgrade` return codes (does not destroy client manager for those). | `CheckLicenceLimit` (private), `GetClientManager` (private), `BCLIENTMANAGER.ClientManager.Logon` |
| `Dispose` | `()` | Releases resources (IDisposable). | — |

**Private Methods (2):**

| Method | Description | Components/SPs Called |
|--------|-------------|----------------------|
| `GetClientManager` | Reads registry key `ClientManagerCOMPlus` to determine if COM+ in-process client manager is needed. Creates `BCLIENTMANAGER.ClientManager` and calls `Initialise` with source, country, language, log level, currency, calling app name. | `gPMFunctions.GetPMRegSetting`, `BCLIENTMANAGER.ClientManager.Initialise` |
| `CheckLicenceLimit` | 1) Creates `BPMSYSTEM.Business`, calls `Initialise`. 2) Calls `GetICCS` to get the installation-specific identifier. 3) Creates `LicenseManager` and calls `IsThisLicenseValid(iccs)` to validate the licence file. 4) On valid licence, calls `GetValidSystem` to load system record (licence limit, keys, pool size). 5) Sets `m_iLicenceLimit` from licence file quantity. 6) Calls `GetLicencesInUse` to check concurrent usage. 7) If near expiry, sets `WarningMessage` with days remaining. Returns `PMInvalidLicenceKey` or `PMLicenceExceeded` on failure. | `BPMSYSTEM.Business.Initialise`, `BPMSYSTEM.Business.GetICCS`, `LicenseManager.IsThisLicenseValid`, `BPMSYSTEM.Business.GetValidSystem`, `BPMSYSTEM.Business.GetLicencesInUse` |

#### LicenseManager — Key Methods (3)

| Method | Signature | Description | Components/SPs Called |
|--------|-----------|-------------|----------------------|
| `IsThisLicenseValid` | `(iccs As String, ByRef errorMessage As String) As Boolean` | Reads the licence file path from registry (`kRegKeyLicenseKeyPath`), loads the licence XML using `Standard.Licensing.License.Load`, validates: 1) Network domain match, 2) Product identity (SHA-256 of ICCS + public key), 3) Expiration date, 4) Signature verification. Populates `Quantity`, `ExpirationDays`, `LoginReminderToStart`, `Product`, `Version` on success. | `gPMFunctions.GetPMRegSetting`, `Standard.Licensing.License.Load`, `Standard.Licensing.License.Validate`, `SecureHash.ComputeSHA256Hash` |
| `NewID` | `()` | Generates a new GUID for licence creation. | — |
| `GetAssemblyFilePath` | `() As String` | Returns the file path of the entry assembly. | — |

#### SecureHash Module — Methods (3)

| Method | Signature | Description |
|--------|-----------|-------------|
| `ComputeSHA256Hash` | `(input As String) As String` | Computes SHA-256 hash of a string. Returns hex string. |
| `ComputeSHA256HashFile` | `(pathFile As String) As String` | Computes SHA-256 hash of a file. Returns hex string. |
| `JoinBytes` | `(inBytes As Byte()) As String` | Converts byte array to hex string. |

**Stored Procedures:** None directly (delegates to `BPMSYSTEM`).

**References:** `BCLIENTMANAGER`, `BPMSYSTEM`, `Standard.Licensing`, `SSP.Shared`, `SharedFiles`, `gPMConstants`, `gPMFunctions`

---

### bPMLocalLicenceManager
**Directory:** `bPMLocalLicenceManager/`
**COM ProgId:** `LicenceManager_NET.LicenceManager`
**Purpose:** **Local (in-process) proxy** for `bPMLicenceManager`. Implements the `bPMLicenceManager.LicenceManager` interface and delegates all operations to a new instance of `bPMLicenceManager.LicenceManager_CoClass`. Used when the licence manager runs in the same process as the calling application.

**Key Classes:**
- `LicenceManager` — proxy class (implements `bPMLicenceManager.LicenceManager`)
- `MainModule` — module constants (`ACApp = "bPMLocalLicenceManager"`)

**Key Methods (4):**

| Method | Signature | Description | Components/SPs Called |
|--------|-----------|-------------|----------------------|
| `LicenceManager_Initialise` | `() As Integer` | Creates a new `bPMLicenceManager.LicenceManager_CoClass` instance and delegates `Initialise()`. Copies `WarningMessage` and `ErrorMessage` from inner instance. | `bPMLicenceManager.LicenceManager_CoClass.Initialise` |
| `LicenceManager_Logon` | `(v_sUsername, v_sPassword, ByRef r_sClientSystemName, ByRef r_bPMBLinkRequired, ByRef r_oClientManager) As Integer` | Delegates to the inner `bPMLicenceManager.LicenceManager.Logon`. | `bPMLicenceManager.LicenceManager.Logon` |
| `GetTemporaryLicenceDetails` | `(ByRef sEncryptedStatus As String) As Integer` | Reads the `InstallationConfig` registry setting from `HKEY_LOCAL_MACHINE\...\SiriusArchitecture\Server\`. Used by `iLogonManager` to determine temporary licensing status. | `gPMFunctions.GetPMRegSetting` |
| `Dispose` | `()` | Disposes the inner `bPMLicenceManager.LicenceManager` instance. | `bPMLicenceManager.LicenceManager.Dispose` |

**Stored Procedures:** None.

**References:** `bPMLicenceManager`, `BCLIENTMANAGER`, `SSP.Shared`, `SharedFiles`, `gPMConstants`, `gPMFunctions`

---

### bPMRemoteLicenceManager
**Directory:** `bPMRemoteLicenceManager/`
**COM ProgId:** `LicenceManager_NET.LicenceManager`
**Purpose:** **Remote (out-of-process / DCOM) proxy** for `bPMLicenceManager`. Identical interface to `bPMLocalLicenceManager` — implements `bPMLicenceManager.LicenceManager` and delegates all operations to `bPMLicenceManager.LicenceManager_CoClass`. Used when licence checking runs in a separate server process.

**Key Classes:**
- `LicenceManager` — proxy class (implements `bPMLicenceManager.LicenceManager`)
- `MainModule` — not present (no separate module file)

**Key Methods (4):**

| Method | Signature | Description | Components/SPs Called |
|--------|-----------|-------------|----------------------|
| `LicenceManager_Initialise` | `() As Integer` | Creates `bPMLicenceManager.LicenceManager_CoClass` and delegates `Initialise()`. | `bPMLicenceManager.LicenceManager_CoClass.Initialise` |
| `LicenceManager_Logon` | `(v_sUsername, v_sPassword, ByRef r_sClientSystemName, ByRef r_bPMBLinkRequired, ByRef r_oClientManager) As Integer` | Delegates to inner `bPMLicenceManager.LicenceManager.Logon`. | `bPMLicenceManager.LicenceManager.Logon` |
| `GetTemporaryLicenceDetails` | `(ByRef sEncryptedStatus As String) As Integer` | Reads `InstallationConfig` registry setting for temporary licensing status. Same as local variant. | `gPMFunctions.GetPMRegSetting` |
| `Dispose` | `()` | Disposes inner licence manager instance. | `bPMLicenceManager.LicenceManager.Dispose` |

**Stored Procedures:** None.

**References:** `bPMLicenceManager`, `BCLIENTMANAGER`, `SSP.Shared`, `SharedFiles`, `gPMConstants`, `gPMFunctions`

---

### BPMUsersSync
**Directory:** `BPMUsersSync/`
**Language:** C# (.NET)
**Purpose:** **KeyCloak user synchronisation service.** Manages the full lifecycle of KeyCloak user accounts from within Pure Insurance: registration, updates, login token retrieval, group membership, and client role assignment. Used by the user maintenance screens to keep KeyCloak in sync with Pure's internal user database.

**Key Classes/Interfaces:**
- `IPureService` (Interface) — defines the service contract
- `PureService` — main implementation
- DTOs: `UserRegisterRequestDTO`, `UserResponseDTO`, `AuthResponseDTO`, `UserLoginRequestDTO`, `GroupResponseDTO`, `RoleResponseDTO`
- Models: `User`, `KeyCloakConfiguration`, `KeycloakToken`, `ClientMappingDTO`, `RoleMappingDTO`, `ClientMappingsResponseDTO`, `LoggingDetails`
- `MappingExtensions` — DTO mapping extensions
- `ClaimsExtensions` — claims utility

**Configuration Model — `KeyCloakConfiguration`:**

| Property | Type | Description |
|----------|------|-------------|
| `grant_type` | `string` | OAuth grant type (typically `"password"`) |
| `Realm` | `string` | KeyCloak realm name |
| `client_id` | `string` | OAuth client ID |
| `username` | `string` | Admin username for KeyCloak API |
| `client_secret` | `string` | OAuth client secret |
| `TokenEndpoint` | `string` | KeyCloak token endpoint URL |
| `Password` | `string` | Admin password (decrypted via `bPMFunc.GetOVal`) |
| `AdminGroupName` | `string` | Default admin group name in KeyCloak |
| `UserKey` | `int` | Pure user key for logging |
| `LoggedInUser` | `string` | Pure username for logging |

**IPureService Interface — Methods (7):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetAllUsersAsync` | `Task<IReadOnlyList<UserResponseDTO>>` | Retrieves all users from KeyCloak |
| `RegisterUserAsync` | `Task<AuthResponseDTO>(UserRegisterRequestDTO)` | Registers a new user in KeyCloak |
| `LoginUserAsync` | `Task<AuthResponseDTO>(UserLoginRequestDTO)` | Authenticates a user and returns tokens |
| `GetUserAsync` | `Task<string>(string userName)` | Gets a user's KeyCloak ID by username |
| `UpdateUserAsync` | `Task<AuthResponseDTO>(UserRegisterRequestDTO)` | Updates an existing KeyCloak user |
| `GetKeyCloakConfiguration` | `KeyCloakConfiguration(string userName, string password, int userKey)` | Builds config from system options |
| `GetAdminTokenAsync` | `Task<KeycloakToken>` | Gets an admin-level OAuth token |

**PureService — Public Methods (8):**

| Method | Signature | Description | External APIs / Components Called |
|--------|-----------|-------------|-----------------------------------|
| `GetAllUsersAsync` | `async Task<IReadOnlyList<UserResponseDTO>>()` | Gets admin token, calls `GET /admin/realms/{realm}/users`, maps results to `UserResponseDTO`. | KeyCloak REST: `GET /admin/realms/{realm}/users` |
| `GetUserAsync` | `async Task<string>(string userName)` | Gets a single user's KeyCloak ID by username. | `GetUserByNameAsync` (private) → KeyCloak REST: `GET /admin/realms/{realm}/users/?username={username}` |
| `RegisterUserAsync` | `async Task<AuthResponseDTO>(UserRegisterRequestDTO)` | Gets admin token, creates user payload (with optional password/credentials), calls `POST /admin/realms/{realm}/users`. If `AdminGroupName` is set, adds user to group and assigns group roles. Returns auth tokens. | KeyCloak REST: `POST /admin/realms/{realm}/users`, `AddUserToGroupByNameAsync`, `AddGroupRoleToUserAsync`, `GetUserTokenAsync` |
| `UpdateUserAsync` | `async Task<AuthResponseDTO>(UserRegisterRequestDTO)` | Gets admin token, builds update payload, calls `PUT /admin/realms/{realm}/users/{id}`. If user is not deleted (`Deleted == 0`), manages group and role assignments. Returns auth tokens if password was updated. | KeyCloak REST: `PUT /admin/realms/{realm}/users/{id}`, `AddUserToGroupByNameAsync`, `AddGroupRoleToUserAsync`, `GetUserTokenAsync` |
| `LoginUserAsync` | `async Task<AuthResponseDTO>(UserLoginRequestDTO)` | Gets user token via password grant, retrieves user by email, returns auth response. | `GetUserTokenAsync` (private), `GetUserByEmailAsync` (private) |
| `GetKeyCloakConfiguration` | `KeyCloakConfiguration(string userName, string password, int userKey)` | Reads system options (5246–5252) to build KeyCloak configuration: token endpoint, client ID/secret, realm, admin username, admin password (decrypted via `bPMFunc.GetOVal`), admin group name. Returns `null` if any option is empty. | `bPMFunc.GetSystemOption` (option numbers: 5246=Realm, 5247=client_id, 5248=client_secret, 5249=username, 5250=Password, 5251=AdminGroupName, 5252=TokenEndpoint) |
| `GetAdminTokenAsync` | `async Task<KeycloakToken>()` | Posts form-encoded client credentials to the token endpoint. Returns `KeycloakToken` with access/refresh tokens. | KeyCloak REST: `POST {TokenEndpoint}` (OAuth token request) |
| `AddGroupRoleToUserAsync` | `async Task(string userId, string groupId, string roleName)` | Retrieves group role mappings via `GetRolesByGroupIdAsync`, then adds each client role to the user via `AddRoleToUserAsync`. | `GetRolesByGroupIdAsync` (private), `AddRoleToUserAsync` (private) |

**PureService — Private Methods (9):**

| Method | Description | KeyCloak API Called |
|--------|-------------|---------------------|
| `GetOptionValues` | Calls `bPMFunc.GetSystemOption` to retrieve a specific system option value by option number. | — (Pure internal) |
| `GetUsersAsync` | Gets admin token, retrieves all users from KeyCloak realm. | `GET /admin/realms/{realm}/users` |
| `GetUserByNameAsync` | Gets admin token, searches users by username. Returns first match. | `GET /admin/realms/{realm}/users/?username={username}` |
| `GetUserByEmailAsync` | Gets admin token, searches users by email. Returns first match. | `GET /admin/realms/{realm}/users/?email={email}` |
| `AddUserToGroupAsync` | Adds a user to a KeyCloak group by user ID and group ID. | `PUT /admin/realms/{realm}/users/{userId}/groups/{groupId}` |
| `GetGroupIdByNameAsync` | Searches groups by name, returns the group ID. | `GET /admin/realms/{realm}/groups?search={groupName}` |
| `AddUserToGroupByNameAsync` | Resolves group ID by name, then adds user to that group. | `GetGroupIdByNameAsync` → `AddUserToGroupAsync` |
| `GetUsersInGroupAsync` | Retrieves all members of a specific group. | `GET /admin/realms/{realm}/groups/{groupId}/members` |
| `GetRolesByGroupIdAsync` | Retrieves role mappings for a group (client-level roles). | `GET /admin/realms/{realm}/groups/{groupId}/role-mappings` |
| `AddRoleToUserAsync` | Adds a specific client role to a user. | `POST /admin/realms/{realm}/users/{userId}/role-mappings/clients/{containerId}` |
| `GetUserTokenAsync` | Gets a user-level OAuth token using password grant. | `POST {TokenEndpoint}` |

**System Options Used:**

| Option # | Purpose |
|----------|---------|
| 5246 | KeyCloak Realm |
| 5247 | KeyCloak Client ID |
| 5248 | KeyCloak Client Secret |
| 5249 | KeyCloak Admin Username |
| 5250 | KeyCloak Admin Password (encrypted) |
| 5251 | KeyCloak Admin Group Name |
| 5252 | KeyCloak Token Endpoint URL |

**Stored Procedures:** None (uses KeyCloak REST API + `bPMFunc.GetSystemOption`).

**References:** `SSP.Shared` (`bPMFunc`), `Newtonsoft.Json`, `Standard.Licensing`, `System.Net.Http`

---

### bSIROverdueTaskCheck
**Directory:** `bSIROverdueTaskCheck/`
**Type:** Console application (EXE)
**Purpose:** **Batch process** that checks for overdue tasks in the system and creates escalation tasks for supervisors. Typically run by the batch scheduler. Logs in as the system user (`siriuscomm`), queries for overdue work manager tasks, and creates a "check task" for each one to alert the responsible supervisor.

**Key Classes/Modules:**
- `MainModule` — console entry point and all business logic
- `BusinessSQL` — stored procedure constants

**Key Methods (5):**

| Method | Scope | Signature | Description | Stored Procedures / Components Called |
|--------|-------|-----------|-------------|---------------------------------------|
| `Main` | Public | `()` | Entry point. Initialises database connection, then calls `ProcessOverdueTaskCheck()`. On completion or error, calls `ExitProcess`. | `Initialise` (private), `ProcessOverdueTaskCheck` (private) |
| `Initialise` | Private | `() As Integer` | Creates `bObjectManager` and `dPMDAO.Database` instances. Logs in as system user (`siriuscomm`). Calls `gPMComponentServices.CheckDatabase` to get database connection. Executes raw SQL to get user details. Calls `m_oObjectManager.InitialiseWithUserState` with hardcoded system credentials. | **SP:** `SELECT user_id, language_id FROM PMUser WHERE username = {username} AND password = {password}` (inline SQL), `bObjectManager.InitialiseWithUserState`, `gPMComponentServices.CheckDatabase` |
| `ProcessOverdueTaskCheck` | Private | `() As Integer` | Calls `GetOverdueTasks` to retrieve the overdue task list. Iterates through results and calls `CreateTaskToAlertSupervisor` for each overdue task instance. | `GetOverdueTasks` (private), `CreateTaskToAlertSupervisor` (private) |
| `GetOverdueTasks` | Private | `(ByRef r_vResults(,) As Object) As Integer` | Executes `spu_get_overdue_tasks` stored procedure via `dPMDAO.Database.SQLSelect`. Returns results as a 2D array with columns: `PMWrkTaskInstanceCnt`, `PMUserGroupId`, `PMUserId`. | **SP:** `spu_get_overdue_tasks` |
| `CreateTaskToAlertSupervisor` | Private | `(v_lpmwrktaskinstancecnt As Integer) As Integer` | Creates escalation task for a supervisor. Adds parameters (`user_id`, `party_cnt`, `insurance_file_cnt`, `os_pmwrk_task_instance_cnt`) and calls `spu_create_check_task`. | **SP:** `spu_create_check_task` |

**Stored Procedures (2):**

| SP | Purpose | Parameters |
|----|---------|------------|
| `spu_get_overdue_tasks` | Retrieves all overdue work manager tasks | None |
| `spu_create_check_task` | Creates a supervisor escalation task for an overdue task | `@user_id`, `@party_cnt`, `@insurance_file_cnt`, `@os_pmwrk_task_instance_cnt` |

**SQL Statements (1):**

| Statement | Purpose |
|-----------|---------|
| `SELECT user_id, language_id FROM PMUser WHERE username = {username} AND password = {password}` | Retrieves system user ID and language during initialisation |

**References:** `bObjectManager`, `dPMDAO.Database`, `gPMComponentServices`, `gPMFunctions`, `SharedFiles`, `gPMConstants`

---

### bSIRUserCompetenceTask
**Directory:** `bSIRUserCompetenceTask/`
**COM ProgId:** `Business_NET.Business`
**Purpose:** **Navigator-driven business component** that creates competence-check tasks for supervisors. Implements the `SSP.S4I.Interfaces.IBusiness` interface and follows the standard Navigator pattern (`SetKeys` → `Start` → `GetSummary`). Used to create supervisor memo tasks related to user competence assessments, linked to a party and insurance file.

**Key Classes/Modules:**
- `Business` — main class (implements `IDisposable`, `SSP.S4I.Interfaces.IBusiness`)
- `MainModule` — module constants (`ACApp = "bSIRUserComplianceTask"`)
- `BusinessSQL` — stored procedure constants

**Key Properties (10):**

| Property | Type | Access | Description |
|----------|------|--------|-------------|
| `PMProductFamily` | `Integer` | ReadOnly | Returns `pmePFSiriusSolutions` |
| `CallingAppName` | `String` | Read/Write | Name of the calling application |
| `PMAuthorityLevel` | `Integer` | Read/Write | User's authority level |
| `Status` | `Integer` | Read/Write | Navigator status code |
| `Task` | `Integer` | ReadOnly | Current task ID |
| `Navigate` | `Integer` | ReadOnly | Navigation mode |
| `ProcessMode` | `Integer` | ReadOnly | Process mode (default: Generic) |
| `TransactionType` | `String` | ReadOnly | Transaction type (default: Generic) |
| `EffectiveDate` | `Date` | ReadOnly | Effective date |
| `PartyCnt` / `InsuranceFileCnt` / `TaskUserId` | `Integer` | Read/Write | Key values for task creation |

**Key Methods (9):**

| Method | Signature | Description | Stored Procedures / Components Called |
|--------|-----------|-------------|---------------------------------------|
| `Initialise` | `(sUserName, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard IBusiness initialisation. Stores user credentials, calls `gPMComponentServices.CheckDatabase` to get `dPMDAO.Database` instance, sets default process modes (Generic). | `gPMComponentServices.CheckDatabase` |
| `SetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Navigator SetKeys — assigns `PartyCnt`, `TaskUserId`, `InsuranceFileCnt` from the key array. Currently returns `PMTrue` (key parsing is commented out). | — |
| `GetKeys` | `(ByRef vKeyArray(,) As Object) As Integer` | Navigator GetKeys — returns current key values. Currently returns `PMTrue` (stub). | — |
| `GetSummary` | `(ByRef vSummaryArray(,) As Object) As Integer` | Navigator GetSummary — returns summary heading/value array. Currently returns a placeholder ("Policy Reference" / "X"). | — |
| `Start` | `() As Integer` | Navigator Start — entry point into the component's workflow. Sets `Status = PMOK`. Task creation (`CreateTask`) is currently commented out. | — (commented: `CreateTask`) |
| `SetProcessModes` | `([vTask], [vNavigate], [vProcessMode], [vTransactionType], [vEffectiveDate]) As Integer` | Sets optional process mode parameters for the Navigator framework. | — |
| `Cancel` | `() As Integer` | Checks if there are unsaved changes. Currently always returns `PMTrue` (no dirty checking implemented). | — |
| `CreateTask` | `() As Integer` | Creates a competence-check memo task for a supervisor. Adds parameters (`party_cnt`, `insurance_file_cnt`, `user_id`, `os_pmwrk_task_instance_cnt`) and executes `spu_create_check_task` stored procedure. | **SP:** `spu_create_check_task` |
| `Dispose` | `()` | Closes database connection if component created it. | `dPMDAO.Database.CloseDatabase` |

**Stored Procedures (1):**

| SP | Purpose | Parameters |
|----|---------|------------|
| `spu_create_check_task` | Creates a supervisor competence-check memo task | `@party_cnt`, `@insurance_file_cnt`, `@user_id`, `@os_pmwrk_task_instance_cnt` |

**References:** `dPMDAO.Database`, `bPMLookup.Business`, `gPMComponentServices`, `gPMConstants`, `SSP.S4I.Interfaces.IBusiness`

---

<!-- ============================================================ -->
<!-- DATA ACCESS LAYER                                             -->
<!-- ============================================================ -->

## Data Access Layer (`d*`)

### dPMDAO
**Directory:** `dPMDAO/`
**COM ProgId:** `dPMDAO.Database`, `dPMDAO.Records`, `dPMDAO.Parameters`
**Purpose:** **Core database abstraction layer** for all Sirius Architecture components. Provides SQL Server connection management, query execution (SELECT, INSERT, UPDATE, DELETE), recordset access, stored procedure parameter handling, connection pooling, and transaction support. Nearly every VB.NET component in the architecture depends on `dPMDAO`.

**Key Classes:**
- `Database` — connection management, query execution, transactions
- `Records` — ADO recordset wrapper (row-by-row cursor access)
- `Parameters` — stored procedure parameter builder

**Database — Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `OpenDatabase` | `(sDataSource, sDatabase, sUsername, sPassword, [iTimeout]) As Integer` | Open a SQL Server connection |
| `CloseDatabase` | `() As Integer` | Close the current connection |
| `SQLSelect` | `(sSQL, ByRef oRecords) As Integer` | Execute SELECT query, return recordset |
| `SQLAction` | `(sSQL) As Integer` | Execute INSERT/UPDATE/DELETE statement |
| `SQLSelectSP` | `(sSPName, oParameters, ByRef oRecords) As Integer` | Execute stored procedure, return recordset |
| `SQLActionSP` | `(sSPName, oParameters) As Integer` | Execute stored procedure (no results) |
| `BeginTransaction` | `() As Integer` | Start a database transaction |
| `CommitTransaction` | `() As Integer` | Commit the current transaction |
| `RollbackTransaction` | `() As Integer` | Roll back the current transaction |
| `IsConnected` | `() As Boolean` | Check if a connection is active |

**Records — Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `MoveNext` | `() As Integer` | Move to next row |
| `MoveFirst` | `() As Integer` | Move to first row |
| `EOF` | `As Boolean` | Whether past the last row |
| `Field` | `(sName) As Object` | Get field value by name |
| `FieldCount` | `As Integer` | Number of columns in the recordset |
| `RecordCount` | `As Integer` | Number of rows (if available) |

**Parameters — Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Add` | `(sName, vValue, eDirection) As Integer` | Add a parameter |
| `AddOutput` | `(sName, eType) As Integer` | Add an output parameter |
| `GetValue` | `(sName) As Object` | Get output parameter value after execution |
| `Clear` | `() As Integer` | Remove all parameters |

**Stored Procedures:** None directly — this is the data access layer that *executes* SPs for other components.

**References:** `System.Data.SqlClient`, `SharedFiles`, `gPMConstants`

---

### dPMDAOBridge
**Directory:** `dPMDAOBridge/`
**Language:** C#
**Purpose:** **C# adapter** that wraps `dPMDAO.Database` as a `SiriusConnection` (from `Sirius.Achitecture.Data`). Allows modern C# components to use the legacy dPMDAO database layer seamlessly.

**Key Classes:**
- `SiriusConnectionPMDAO` — extends `SiriusConnection`, wraps `dPMDAO.Database` instance
- `PmdaoConnectionInfo` — connection information DTO
- `AdoDataReader` — ADO recordset adapter for `IDataReader`

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SiriusConnectionPMDAO` | `(siriusUserName, sourceID, languageID, ...) → Instance` | Constructor from dPMDAO parameters |
| `InitialiseDatabase` | `() → Void` | Initialize internal `dPMDAO.Database` instance |
| `GetObjectData` | `(info, context) → void` | Serialization support |

**References:** `dPMDAO`, `Sirius.Achitecture.Data`, `SSP.Shared`

---

### dPMDAO_Old
**Directory:** `dPMDAO_Old/`
**Purpose:** Empty / deprecated archive of the original dPMDAO project. No active code.

---

<!-- ============================================================ -->
<!-- CLIENT & AUTHENTICATION                                       -->
<!-- ============================================================ -->

## Client & Authentication Components

### ClientManager DCOM
**Directory:** `ClientManager DCOM/`
**COM ProgId:** `bClientManager.ClientManager`
**Purpose:** **User authentication and session management** for desktop applications. Manages user logon, DCOM object creation, branch/source selection, and maintains the authenticated user state. This is the primary session component that `bObjectManager` delegates to.

**Key Classes:**
- `ClientManager` — main business class

**Key Methods (14+):**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(iSourceID, iCountryID, iLanguageID, iLogLevel, iCurrencyID, sCallingApp) As Integer` | Initialize with user context parameters | — |
| `Logon` | `(v_sUsername, v_sPassword) As Integer` | Authenticate user, validate credentials, load user properties | `spu_get_system_option`, `spu_pm_get_user_sources` |
| `Logoff` | `() As Integer` | Sign out user, release resources | — |
| `GetPropertyValues` | `(ByRef sUsername, ByRef sPassword, ByRef iUserID, ...) As Integer` | Retrieve all current session property values | — |
| `GetInstance` | `(ByRef oObject, sClassName) As Integer` | Create/retrieve a named business object instance | COM `CreateObject` |
| `ChangePassword` | `(sOldPassword, sNewPassword) As Integer` | Change user password | — |
| `SetBranch` | `(iSourceID) As Integer` | Switch to a different branch/source | — |
| `GetUserSources` | `(ByRef vSources) As Integer` | Get list of branches available to user | `spu_pm_get_user_sources` |

**Stored Procedures:**
- `spu_get_system_option` — retrieve system configuration values
- `spu_pm_get_user_sources` — get user's permitted branches

**References:** `dPMDAO.Database`, `bPMSystem.Business`, `gPMConstants`, `gPMFunctions`, `SharedFiles`

---

### ClientRegistry
**Directory:** `ClientRegistry/`
**Purpose:** **CLI registry installer** for Sirius applications. Handles Windows registry settings required during client installation.

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Install` | `(sApplicationPath) As Integer` | Write required registry keys for Sirius client |
| `Uninstall` | `() As Integer` | Remove registry keys |

**References:** `Microsoft.Win32.Registry`, `SharedFiles`

---

### iChangePassword
**Directory:** `iChangePassword/`
**Purpose:** **Password change dialog.** Presents a WinForm for the user to enter old and new passwords. Validates password complexity rules and delegates the actual change to the appropriate business component.

**Key Forms:**
- `frmChangePassword` — password change UI

**References:** `bClientManager`, `gPMConstants`, `gPMFunctions`

---

### iLogonManager
**Directory:** `iLogonManager/`
**COM ProgId:** `iLogonManager.LogonManager`
**Purpose:** **User authentication coordinator** exposed via .NET Remoting (`MarshalByRefObject`). Manages the logon sequence — licence checking, user validation, branch selection — and holds authenticated session state for consumption by `bObjectManager` and other callers.

**Key Classes:**
- `LogonManager` — `MarshalByRefObject` implementing `bPMLicenceManager.LicenceManager` + additional state

**Key Methods:**

| Method | Signature | Description | Components Called |
|--------|-----------|-------------|-------------------|
| `Initialise` | `(sCallingApp) As Integer` | Start logon — create licence manager, validate licence | `bPMLocalLicenceManager`, `bPMRemoteLicenceManager` |
| `Logon` | `(sUsername, sPassword) As Integer` | Authenticate user via licence manager | `LicenceManager.Logon` |
| `GetPropertyValues` | `(ByRef ...) As Integer` | Return all session properties to caller | — |
| `CheckClientInstall` | `(lProductFamily) As Integer` | Check if client software needs updating | `bPMClientInstallCheck` |

**Key Properties:**
- `AppReferenceCount` — tracks number of connected desktop applications
- `LoggedOnLocally` / `GenericConnectionStatus` — connection status flags
- `UserName`, `Password`, `UserID`, `SourceID`, `LanguageID`, `CurrencyID` — session state

**References:** `bPMLocalLicenceManager`, `bPMRemoteLicenceManager`, `bClientManager`, `gPMConstants`, `gPMFunctions`

---

### Ilogonserver
**Directory:** `Ilogonserver/`
**Type:** Console EXE
**Purpose:** **TCP Remoting host** for `iLogonManager`. Starts a .NET Remoting channel on port (65535 minus SessionId) and registers `iLogonManager.LogonManager` as a well-known singleton. Desktop apps connect to this process to authenticate.

**Key Entry Point:**

| Method | Description |
|--------|-------------|
| `Main()` | Creates `TcpChannel(65535 - SessionId)`, registers `iLogonManager.LogonManager` as `SingleCall` at URI `"SSP"`. Keeps process alive. |

**References:** `System.Runtime.Remoting`, `iLogonManager`

---

### iLogonStatusManager
**Directory:** `iLogonStatusManager/`
**Purpose:** **Logon status notification UI.** Displays a system tray or form-based indicator showing the user's current logon status and provides an interface for re-authentication.

**Key Forms:**
- `frmLogonStatus` — status display form

**References:** `iLogonManager`, `gPMConstants`

---

### iPMScreenMessage
**Directory:** `iPMScreenMessage/`
**Purpose:** **Configurable screen message dialog.** Displays modal messages to users with customisable text, buttons, and layout. Used by various components to show warnings, confirmations, and information.

**Key Forms:**
- `frmScreenMessage` — message dialog form

**References:** `gPMConstants`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- LICENCE & SECURITY UTILITIES                                  -->
<!-- ============================================================ -->

## Licence & Security Utilities

### LicenceAdmin
**Directory:** `LicenceAdmin/`
**Purpose:** **Licence administration** — business logic (`bLicenceAdmin`) and UI (`iLicenceAdmin`) for managing licence limits and user records.

**Key Classes:**
- `bLicenceAdmin.LicenceAdmin` — business class
- `iLicenceAdmin` — WinForm UI

**bLicenceAdmin — Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize with DB connection | DB open |
| `GetLicenceLimit` | `(ByRef r_iLimit) As Integer` | Retrieve current licence limit | SQL SELECT from pmsystem |
| `UpdatePMUser` | `(iUserID, sStatus) As Integer` | Update user status in licence table | SQL UPDATE |
| `Dispose` | `()` | Release resources | — |

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### LicenceKeyGen
**Directory:** `LicenceKeyGen/`
**Purpose:** **Licence key encryption utility.** Generates encrypted licence keys for the Sirius system.

**References:** `System.Security.Cryptography`, `SharedFiles`

---

### MigrateRegSettings
**Directory:** `MigrateRegSettings/`
**Purpose:** **Registry settings migration tool.** Migrates Sirius registry settings from legacy locations to current paths during version upgrades.

**References:** `Microsoft.Win32.Registry`, `SharedFiles`

---

### PasswordRehasher
**Directory:** `PasswordRehasher/`
**Type:** Console EXE
**Purpose:** **Password encryption migration.** Reads user records with legacy password hashes and re-encrypts them using the current hashing algorithm.

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Main` | `() → Void` | Process all user records needing re-hash | `spu_get_user_details` |
| `RehashPassword` | `(sOldHash) → String` | Convert legacy hash to new format | — |

**Stored Procedures:**
- `spu_get_user_details` — retrieve user credentials for rehashing

**References:** `dPMDAO.Database`, `System.Security.Cryptography`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- NAVIGATOR / WORKFLOW FRAMEWORK                                -->
<!-- ============================================================ -->

## Navigator / Workflow Framework

### Navigator V3
**Directory:** `Navigator V3/`
**COM ProgId:** `bPMNavigator.PMNavigator`
**Purpose:** **Workflow navigation framework.** Manages process maps, steps, and navigation through multi-step business workflows (e.g., new business, claims, renewals). Tracks step status, branching, and provides the "navigate forward/back" engine for desktop screens.

**Key Classes:**
- `PMNavigator` — main navigation engine (implements `IBusiness`)
- `Map` / `Step` — data structures for workflow maps and steps

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize navigator with user context | `spu_pmnav_get_maps` |
| `LoadMap` | `(v_lMapID) As Integer` | Load a navigation map | `spu_pmnav_get_map_steps` |
| `GetCurrentStep` | `(ByRef r_oStep) As Integer` | Get the current step in the map | — |
| `MoveNext` | `() As Integer` | Navigate to the next step | `spu_pmnav_move_next` |
| `MovePrevious` | `() As Integer` | Navigate to the previous step | `spu_pmnav_move_previous` |
| `MoveToStep` | `(v_lStepID) As Integer` | Jump to a specific step | `spu_pmnav_move_to_step` |
| `GetMapList` | `(ByRef r_vMaps) As Integer` | Retrieve available maps | `spu_pmnav_get_maps` |
| `SetStepComplete` | `(v_lStepID) As Integer` | Mark a step as complete | `spu_pmnav_set_step_complete` |
| `SetStepStatus` | `(v_lStepID, v_iStatus) As Integer` | Set step status | `spu_pmnav_update_step_status` |
| `GetStepByKey` | `(v_sKeyName, v_sKeyValue, ByRef r_oStep) As Integer` | Find step by key | SQL SELECT |
| `GetMapSteps` | `(v_lMapID, ByRef r_vSteps) As Integer` | Get all steps for a map | `spu_pmnav_get_map_steps` |
| `CreateProcess` | `(v_lMapID, ByRef r_lProcessID) As Integer` | Start a new navigation process | `spu_pmnav_create_process` |
| `DeleteProcess` | `(v_lProcessID) As Integer` | Remove a process | `spu_pmnav_delete_process` |
| `GetProcessCount` | `(ByRef r_iCount) As Integer` | Count active processes | SQL SELECT |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures (18+):**
- `spu_pmnav_get_maps`, `spu_pmnav_get_map_steps`
- `spu_pmnav_create_process`, `spu_pmnav_delete_process`
- `spu_pmnav_move_next`, `spu_pmnav_move_previous`, `spu_pmnav_move_to_step`
- `spu_pmnav_set_step_complete`, `spu_pmnav_update_step_status`
- `spu_pmnav_get_current_step`, `spu_pmnav_get_step_keys`
- And more `spu_pmnav_*` pattern SPs

**References:** `dPMDAO.Database`, `gPMConstants`, `gPMFunctions`, `SharedFiles`, `SSP.S4I.Interfaces.IBusiness`

---

### Navigator XM
**Directory:** `Navigator XM/`
**Purpose:** **XML-based roadmap workflow engine.** Reads roadmap definition files (XML) that describe multi-step processes (e.g., `OPENCLM.XML`, `NEWPLAN.XML`) and drives the navigation between steps. Supports branching, conditions, and batch processing.

**Key Classes:**
- `NavigatorXM` — XML roadmap parser and execution engine
- `RoadmapStep` — individual step definition
- `RoadmapMap` — map container

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `LoadRoadmap` | `(sXMLFilePath) As Integer` | Parse an XML roadmap file |
| `GetSteps` | `(ByRef r_vSteps) As Integer` | Retrieve all steps from the roadmap |
| `Navigate` | `(v_sDirection) As Integer` | Navigate forward/back through the roadmap |
| `EvaluateCondition` | `(v_sCondition) As Boolean` | Evaluate conditional branching |

**References:** `System.Xml`, `gPMConstants`, `SharedFiles`

---

### Workflow (PMStepMaintenance)
**Directory:** `Workflow/PMStepMaintenance/`
**Purpose:** **Workflow step definition CRUD.** Manages the creation, modification, and deletion of workflow step definitions, including step order and branch assignments.

**Key Classes:**
- `Business` — workflow step CRUD operations
- `BusinessSQL` — stored procedure constants

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetMaintainData` | `(v_lWorkflowId, ByRef r_vResults) As Integer` | Get workflow steps | `spu_PMwrk_Workflow_Step_Select` |
| `AddPackageStep` | `(ByRef r_lWorkflowStepId, v_lWorkflowId, v_lStepOrder, ...) As Integer` | Create workflow step | `spu_PMWrk_Workflow_Step_Insert` |
| `UpdatePackageStep` | `(...) As Integer` | Update existing step | `spu_PMWrk_Workflow_Step_Update` |
| `DeletePackageStep` | `(v_lWorkflowStepId) As Integer` | Delete step | SQL DELETE |

**Stored Procedures:**
- `spu_PMwrk_Workflow_Step_Select`
- `spu_PMWrk_Workflow_Step_Insert`
- `spu_PMWrk_Workflow_Step_Update`
- `spu_pmuser_get_allowed_branches`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- PM INFRASTRUCTURE & UI                                        -->
<!-- ============================================================ -->

## PM Infrastructure & UI Components

### PMAbout
**Directory:** `PMAbout/`
**Purpose:** **About dialog.** Displays application version, copyright, and system information.

**Key Forms:**
- `frmAbout` — about dialog form

**References:** `System.Reflection`, `SharedFiles`

---

### PMAutoNumber
**Directory:** `PMAutoNumber/`
**Purpose:** **Auto-number / reference generation.** Allocates sequential numbers and generates formatted reference strings (e.g., policy numbers, claim numbers) from database sequences.

**Key Classes:**
- `Business` — auto-number business logic (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `AllocateNumber` | `(v_sNumberType, ByRef r_lAllocatedNumber) As Integer` | Allocate next sequential number | `spu_pm_allocate_number` |
| `GenerateReference` | `(v_sFormat, v_nKey, ByRef r_sReference) As Integer` | Generate formatted reference string | `spu_pm_GenerateReference` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pm_allocate_number` — allocate next number from sequence
- `spu_pm_GenerateReference` — generate formatted reference from template

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMBusy
**Directory:** `PMBusy/`
**Purpose:** **Progress indicator.** Displays a "busy" dialog during long-running operations with optional progress bar and cancel support.

**Key Forms:**
- `frmBusy` — progress dialog form

**References:** `gPMConstants`

---

### PMCaption
**Directory:** `PMCaption/`
**Purpose:** **Multi-language caption management.** Retrieves and caches localised UI captions (labels, buttons, messages) by language ID. All desktop UI components use this for internationalisation.

**Key Classes:**
- `Business` — caption retrieval (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetCaption` | `(v_sCaptionCode) As String` | Retrieve a localised caption by code | `spu_pm_Select_captions` |
| `GetCaptions` | `(v_sCaptionCodes, ByRef r_vCaptions) As Integer` | Retrieve multiple captions at once | `spu_pm_Select_captions` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pm_Select_captions` — select captions by code and language

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMClientInstallAdmin
**Directory:** `PMClientInstallAdmin/`
**Purpose:** **Client installation admin UI.** Admin screen for managing client software installation packages and version requirements.

**Key Forms:**
- `frmClientInstallAdmin` — installation management form

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMClientInstallCheck
**Directory:** `PMClientInstallCheck/`
**Purpose:** **Client version verification.** Checks if the installed client version matches the required version for the current product and triggers updates when necessary.

**Key Classes:**
- `Business` — version check logic

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `CheckClientInstall` | `(v_lProductFamily) As Integer` | Compare client version against required version |
| `GetRequiredVersion` | `(ByRef r_sVersion) As String` | Retrieve the required version for the product |

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMCurrency
**Directory:** `PMCurrency/`
**Purpose:** **Currency management.** CRUD operations for currency definitions including exchange rates and formatting.

**Key Classes:**
- `Business` — currency business logic (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetCurrency` | `(v_iCurrencyID, ByRef r_sCurrName, ByRef r_sCurrCode, ...) As Integer` | Get currency details | `spu_currency_sel` |
| `GetCurrencies` | `(ByRef r_vCurrencies) As Integer` | List all currencies | `spu_currency_sel` |
| `AddCurrency` | `(v_sCurrName, v_sCurrCode, ..., ByRef r_iCurrencyID) As Integer` | Add new currency | `spu_currency_add` |
| `UpdateCurrency` | `(v_iCurrencyID, v_sCurrName, v_sCurrCode, ...) As Integer` | Update currency | `spu_currency_upd` |
| `DeleteCurrency` | `(v_iCurrencyID) As Integer` | Delete currency | `spu_currency_del` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_currency_sel`, `spu_currency_add`, `spu_currency_upd`, `spu_currency_del`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMDataControl
**Directory:** `PMDataControl/`
**Purpose:** **Database connection pooling.** Manages shared database connections for components, providing connection reuse and lifecycle management.

**Key Classes:**
- `DataControl` — connection pool manager

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetConnection` | `(sDataSource, sDatabase) As dPMDAO.Database` | Retrieve or create a pooled connection |
| `ReleaseConnection` | `(oDatabase) As Integer` | Return a connection to the pool |
| `CloseAll` | `() As Integer` | Close all pooled connections |

**References:** `dPMDAO.Database`, `gPMConstants`

---

### PMDecision
**Directory:** `PMDecision/`
**Purpose:** **Decision prompt dialog.** Presents a modal form with configurable choices (e.g., Yes/No, OK/Cancel, custom buttons) and returns the user's selection.

**Key Forms:**
- `frmDecision` — decision dialog form

**References:** `gPMConstants`, `SharedFiles`

---

### PMEventLogViewer
**Directory:** `PMEventLogViewer/`
**Purpose:** **Windows event log viewer.** Displays and filters Windows Event Log entries for Sirius-related events.

**Key Forms:**
- `frmEventLogViewer` — event log display form

**References:** `System.Diagnostics.EventLog`, `gPMConstants`

---

### PMFormControl
**Directory:** `PMFormControl/`
**Purpose:** **Field validation framework.** Provides reusable form field validation logic (required fields, format validation, cross-field rules) for WinForm applications.

**Key Classes:**
- `FormControl` — validation engine

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `AddField` | `(sFieldName, oControl, eValidationType) As Integer` | Register a field for validation |
| `Validate` | `() As Boolean` | Run all validations, return True if all pass |
| `GetErrors` | `(ByRef r_vErrors) As Integer` | Retrieve validation error messages |
| `ClearAll` | `() As Integer` | Reset all validations |

**References:** `gPMConstants`, `SharedFiles`

---

### PMInstallUnzipper
**Directory:** `PMInstallUnzipper/`
**Type:** Console EXE
**Purpose:** **Setup unzip utility.** Extracts installation ZIP packages during client deployment.

**References:** `System.IO.Compression`, `SharedFiles`

---

### PMPropertyManager
**Directory:** `PMPropertyManager/`
**Purpose:** **In-memory property store.** Provides a key-value store for runtime properties, used to share state between components within a session without database persistence.

**Key Classes:**
- `PropertyManager` — property store

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SetProperty` | `(sKey, vValue) As Integer` | Store a named value |
| `GetProperty` | `(sKey) As Object` | Retrieve a named value |
| `RemoveProperty` | `(sKey) As Integer` | Remove a named value |
| `Clear` | `() As Integer` | Remove all properties |
| `PropertyExists` | `(sKey) As Boolean` | Check if key exists |

**References:** `gPMConstants`

---

### PMRegELSupport
**Directory:** `PMRegELSupport/`
**Purpose:** **Event log DLL registration.** Registers/unregisters the Sirius event source DLL with Windows Event Log so that custom events can be written.

**References:** `System.Diagnostics.EventLog`, `Microsoft.Win32.Registry`

---

### PMServerRegistry
**Directory:** `PMServerRegistry/`
**Purpose:** **Server registry reader.** Reads Sirius server configuration settings from the Windows registry (server name, database, paths, etc.).

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetServerSetting` | `(sKeyName) As String` | Read a named registry value from the server hive |
| `GetAllSettings` | `(ByRef r_vSettings) As Integer` | Read all server settings into an array |

**References:** `Microsoft.Win32.Registry`, `gPMConstants`

---

### PMSiriusLogViewer
**Directory:** `PMSiriusLogViewer/`
**Purpose:** **Log file viewer** with ZIP archive support. Displays Sirius application log files and supports viewing compressed log archives.

**Key Forms:**
- `frmLogViewer` — log display form with filtering and search

**References:** `System.IO.Compression`, `SharedFiles`

---

### PMSiriusSupport
**Directory:** `PMSiriusSupport/`
**Purpose:** **Support web page launcher.** Opens the SSP support web page in the default browser.

**References:** `System.Diagnostics.Process`

---

### PMSplash
**Directory:** `PMSplash/`
**Purpose:** **Splash screen.** Displays application branding and loading progress during startup.

**Key Forms:**
- `frmSplash` — splash screen form

**References:** `SharedFiles`

---

### PMMsgBox
**Directory:** `PMMsgBox/`
**Purpose:** **Standard message box.** Enhanced `MessageBox` wrapper with Sirius-standard styling, icons, and button configurations.

**References:** `gPMConstants`

---

<!-- ============================================================ -->
<!-- PM SYSTEM & CONFIGURATION                                     -->
<!-- ============================================================ -->

## PM System & Configuration

### PMSystem
**Directory:** `PMSystem/`
**Purpose:** **System configuration management.** Reads and writes the core system record including ICCS (Installation-specific identifier), licence limits, database settings, and system-wide configuration. Central to licence validation and system initialisation.

**Key Classes:**
- `Business` — system configuration operations (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetICCS` | `(ByRef r_sICCS) As String` | Get Installation Client Computer System identifier | `spu_pm_iccs` |
| `GetValidSystem` | `(ByRef r_iLicenceLimit, ByRef r_sLicenceKey, ByRef r_iPoolSize, ByRef r_iHomeCountry, ...) As Integer` | Load system record with licence info | `spg_GetValidSystem` |
| `GetLicencesInUse` | `(ByRef r_iInUse) As Integer` | Count currently active concurrent licences | `spu_GetNoLicencesInUse` |
| `AddSystem` | `(v_sSystemName, v_iLicenceLimit, ...) As Integer` | Create system record | `spu_add_PMSystem` |
| `UpdateSystem` | `(v_sSystemName, v_iLicenceLimit, ...) As Integer` | Update system record | `spu_update_PMSystem` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pm_iccs` — get ICCS identifier
- `spg_GetValidSystem` — get system record
- `spu_GetNoLicencesInUse` — count active licences
- `spu_add_PMSystem`, `spu_update_PMSystem` — system record CRUD

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMSource
**Directory:** `PMSource/`
**Purpose:** **Branch / office / source management.** CRUD operations for source records (branches/offices) which represent organisational units. Users are assigned to sources for data partitioning.

**Key Classes:**
- `Business` — source management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetSources` | `(ByRef r_vSources) As Integer` | List all sources | `spu_PM_Select_Source` |
| `GetSource` | `(v_iSourceID, ByRef r_sSourceName, ...) As Integer` | Get single source detail | `spu_PM_Select_Source` |
| `AddSource` | `(v_sSourceName, ..., ByRef r_iSourceID) As Integer` | Create new source | `spu_PM_Add_Source` |
| `UpdateSource` | `(v_iSourceID, v_sSourceName, ...) As Integer` | Update source | `spu_PM_Update_Source` |
| `DeleteSource` | `(v_iSourceID) As Integer` | Delete source | `spu_PM_Delete_Source` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_PM_Select_Source`, `spu_PM_Add_Source`, `spu_PM_Update_Source`, `spu_PM_Delete_Source`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMSourceMaintenance
**Directory:** `PMSourceMaintenance/`
**Purpose:** **Source maintenance UI.** WinForm screen for admins to add, edit, and delete branch/source records.

**Key Forms:**
- `frmSourceMaintenance` — source admin form

**References:** `PMSource.Business`, `gPMConstants`, `SharedFiles`

---

### PMProduct
**Directory:** `PMProduct/`
**Purpose:** **Product lookup.** Provides product list retrieval with embedded SQL queries against the `pmproduct` table.

**Key Classes:**
- `Business` — product lookup (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component |
| `GetProducts` | `(ByRef r_vProducts) As Integer` | Retrieve all products (embedded SQL against `pmproduct`) |
| `Dispose` | `()` | Release resources |

**References:** `dPMDAO.Database`, `gPMConstants`

---

### PMProductClientInstall
**Directory:** `PMProductClientInstall/`
**Purpose:** **Client install management per product.** Manages product-specific client installation packages, versions, and deployment settings.

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMProductLookup
**Directory:** `PMProductLookup/`
**Purpose:** **Product-specific lookup tables.** CRUD operations for lookup values scoped to individual products.

**Key Classes:**
- `Business` — product lookup management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetProductLookups` | `(v_lProductID, ByRef r_vLookups) As Integer` | Get lookups for a product | `spu_pmproduct_lookup_sel` |
| `AddProductLookup` | `(v_lProductID, v_sCode, v_sDescription, ...) As Integer` | Add product lookup | `spu_pmproduct_lookup_add` |
| `UpdateProductLookup` | `(v_lProductLookupID, v_sCode, v_sDescription, ...) As Integer` | Update product lookup | `spu_pmproduct_lookup_upd` |
| `DeleteProductLookup` | `(v_lProductLookupID) As Integer` | Delete product lookup | `spu_pmproduct_lookup_del` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pmproduct_lookup_sel`, `spu_pmproduct_lookup_add`, `spu_pmproduct_lookup_upd`, `spu_pmproduct_lookup_del`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### ProductUpdateHistory
**Directory:** `ProductUpdateHistory/`
**Purpose:** **Product update history tracking.** Records and retrieves history of product update events.

**Key Classes:**
- `Business` — product update history business logic (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component |
| `GetProductUpdates` | `(ByRef r_vUpdates) As Integer` | SELECT from `pmproduct_update_history` JOIN `pmproduct` |
| `GetProducts` | `(ByRef r_vProducts) As Integer` | SELECT from `pmproduct` |
| `Dispose` | `()` | Release resources |

**References:** `dPMDAO.Database`, `gPMConstants`

---

<!-- ============================================================ -->
<!-- PM LOOKUP & REFERENCE DATA                                    -->
<!-- ============================================================ -->

## PM Lookup & Reference Data

### PMLookup
**Directory:** `PMLookup/`
**COM ProgId:** `bPMLookup.Business`
**Purpose:** **Reference data engine.** Provides lookup operations for codes, descriptions, and date-effective IDs used across the system. Central to all dropdown population, code resolution, and reference data validation.

**Key Classes:**
- `Business` — lookup engine (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetCodeDescription` | `(v_sTableName, v_sCode, ByRef r_sDescription) As Integer` | Get description for a code | `spu_pm_Select_captions` |
| `GetEffectiveIDFromCode` | `(v_sTableName, v_sCode, v_dtEffDate, ByRef r_lID) As Integer` | Get date-effective ID | `spu_pm_get_eff_id_from_code` |
| `GetLookupValues` | `(v_sTableName, ByRef r_vValues) As Integer` | Get all values for a lookup table | SQL SELECT |
| `GetLookupByCode` | `(v_sTableName, v_sCode, ByRef r_vRecord) As Integer` | Get single lookup by code | SQL SELECT |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pm_Select_captions` — caption/description retrieval
- `spu_pm_get_eff_id_from_code` — date-effective code resolution

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMMaintainLookup
**Directory:** `PMMaintainLookup/`
**Purpose:** **Lookup table maintenance UI.** WinForm screen for managing reference data tables — add, edit, delete, and reorder lookup values.

**Key Classes:**
- `Business` — lookup maintenance logic
- UI form — lookup admin screen

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `GetColumns` | `(v_sTableName, ByRef r_vColumns) As Integer` | Get column definitions for a lookup table | `spu_pm_Get_Columns` |
| `GetCaptionID` | `(v_sCaptionCode, ByRef r_lCaptionID) As Integer` | Return caption ID for a code | `spu_pm_caption_id_return` |
| `AddRecord` | `(v_sTableName, v_vValues) As Integer` | Add a new lookup record | Dynamic SQL INSERT |
| `UpdateRecord` | `(v_sTableName, v_lID, v_vValues) As Integer` | Update a lookup record | Dynamic SQL UPDATE |
| `DeleteRecord` | `(v_sTableName, v_lID) As Integer` | Delete a lookup record | Dynamic SQL DELETE |

**Stored Procedures:**
- `spu_pm_Get_Columns` — retrieve table column metadata
- `spu_pm_caption_id_return` — resolve caption ID from code

**References:** `dPMDAO.Database`, `PMLookup.Business`, `gPMConstants`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- PM COMMUNICATION & LOGGING                                    -->
<!-- ============================================================ -->

## PM Communication & Logging

### PMMAPI
**Directory:** `PMMAPI/`
**Purpose:** **Email via MAPI.** Sends emails using the Windows Messaging Application Programming Interface (MAPI). Used for email notifications from desktop applications.

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SendMail` | `(sTo, sSubject, sBody, [sAttachment]) As Integer` | Send email via MAPI |

**References:** MAPI32.dll (P/Invoke)

---

### PMMessage
**Directory:** `PMMessage/`
**COM ProgId:** `iPMMessage.PMMessage`
**Purpose:** **Logging to Windows event log.** Writes application log messages to the Windows Event Log using Sirius event sources. Provides structured logging with severity levels.

**Key Classes:**
- `PMMessage` — message logger (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize logger |
| `LogMessage` | `(iType, sMessage, [sApp], [sClass], [sMethod], [iErrNo]) As Integer` | Write to event log |
| `Dispose` | `()` | Release resources |

**References:** `System.Diagnostics.EventLog`, `gPMConstants`

---

### PMMessageAdmin
**Directory:** `PMMessageAdmin/`
**Purpose:** **Message admin UI.** WinForm for viewing and managing application log messages and event log entries.

**Key Forms:**
- `frmMessageAdmin` — message administration form

**References:** `PMMessage`, `gPMConstants`

---

### PMEventTask
**Directory:** `PMEventTask/`
**Purpose:** **Event-driven task scheduling.** Monitors for system events and creates work manager tasks in response. Used by the batch scheduler to trigger time-based task creation.

**Key Classes:**
- `Business` — event task engine (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetDueTasks` | `(ByRef r_vTasks) As Integer` | Get tasks that are now due for processing | `spe_PMWrk_Get_Due_Tasks` |
| `UpdateEventTask` | `(v_lEventTaskID, v_iStatus) As Integer` | Update event task status | `spu_PM_Update_Event_Task` |
| `ProcessEvents` | `() As Integer` | Process all pending events | `spe_PMWrk_Get_Due_Tasks` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spe_PMWrk_Get_Due_Tasks` — select tasks that are due
- `spu_PM_Update_Event_Task` — update event task status

**References:** `dPMDAO.Database`, `bPMWrkTaskInstance`, `gPMConstants`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- PM RECORD LOCKING                                             -->
<!-- ============================================================ -->

## PM Record Locking

### PMLock
**Directory:** `PMLock/`
**COM ProgId:** `bPMLock.User`
**Purpose:** **Pessimistic record locking** using timestamp-based locks. Prevents concurrent edits by ensuring only one user can modify a record at a time. Locks are stored in the database with timestamps for automatic expiry.

**Key Classes:**
- `User` — lock management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `CheckTimestamp` | `(v_sTableName, v_lRecordID, v_tsTimestamp) As Integer` | Compare timestamp for optimistic concurrency | `spu_PMCheck_TS` |
| `AddLock` | `(v_sTableName, v_lRecordID, ByRef r_sLockedBy) As Integer` | Attempt to lock a record | `spu_PMAdd_Lock` |
| `RemoveLock` | `(v_sTableName, v_lRecordID) As Integer` | Release the lock | `spu_PMDelete_Lock` |
| `IsLocked` | `(v_sTableName, v_lRecordID, ByRef r_sLockedBy) As Boolean` | Check if a record is locked | `spu_PMCheck_Lock` |
| `RemoveAllUserLocks` | `() As Integer` | Release all locks held by current user | `spu_PMDelete_User_Locks` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_PMCheck_TS` — check timestamp
- `spu_PMAdd_Lock` — add record lock
- `spu_PMDelete_Lock` — remove lock
- `spu_PMCheck_Lock` — check if locked
- `spu_PMDelete_User_Locks` — remove all user locks

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- PM TASK MANAGEMENT                                            -->
<!-- ============================================================ -->

## PM Task Management

### PMTask
**Directory:** `PMTask/`
**Purpose:** **Task definitions.** CRUD operations for task templates that define what work can be assigned. Each task has a code, description, category, and authority requirements.

**Key Classes:**
- `Business` — task definition management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetTasks` | `(ByRef r_vTasks) As Integer` | List all task definitions | `spu_pmwrk_task_sel` |
| `GetTaskByCode` | `(v_sCode, ByRef r_oTask) As Integer` | Get task by code | `spu_pmwrk_task_code_sel` |
| `AddTask` | `(v_sCode, v_sDescription, ..., ByRef r_lTaskID) As Integer` | Create task definition | `spu_pmwrk_task_add` |
| `UpdateTask` | `(v_lTaskID, v_sCode, v_sDescription, ...) As Integer` | Update task definition | `spu_pmwrk_task_upd` |
| `GetUserAuthority` | `(v_lTaskID, ByRef r_bHasAuthority) As Boolean` | Check if user can perform task | `spu_GetUserAuthorityTask` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pmwrk_task_sel`, `spu_pmwrk_task_code_sel`
- `spu_pmwrk_task_add`, `spu_pmwrk_task_upd`
- `spu_GetUserAuthorityTask`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMTaskCategory
**Directory:** `PMTaskCategory/`
**Purpose:** **Task category management.** CRUD operations for categories used to organise and group task definitions.

**Key Classes:**
- `Business` — task category management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetCategories` | `(ByRef r_vCategories) As Integer` | List all task categories | `spu_pmwrk_task_category_sel` |
| `AddCategory` | `(v_sDescription, ByRef r_lCategoryID) As Integer` | Create category | `spu_pmwrk_task_category_add` |
| `UpdateCategory` | `(v_lCategoryID, v_sDescription) As Integer` | Update category | `spu_pmwrk_task_category_upd` |
| `DeleteCategory` | `(v_lCategoryID) As Integer` | Delete category | `spu_pmwrk_task_category_del` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pmwrk_task_category_sel`, `spu_pmwrk_task_category_add`
- `spu_pmwrk_task_category_upd`, `spu_pmwrk_task_category_del`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMTaskGroup
**Directory:** `PMTaskGroup/`
**Purpose:** **Task group definitions.** CRUD operations for task groups that organise tasks into logical groupings for the work manager.

**Key Classes:**
- `Business` — task group management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetTaskGroups` | `(ByRef r_vGroups) As Integer` | List all task groups | `spu_pmwrk_task_group_sel` |
| `AddTaskGroup` | `(v_sDescription, ..., ByRef r_lGroupID) As Integer` | Create task group | `spu_pmwrk_task_group_add` |
| `UpdateTaskGroup` | `(v_lGroupID, v_sDescription, ...) As Integer` | Update task group | `spu_pmwrk_task_group_upd` |
| `DeleteTaskGroup` | `(v_lGroupID) As Integer` | Delete task group | `spu_pmwrk_task_group_del` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pmwrk_task_group_sel`, `spu_pmwrk_task_group_add`
- `spu_pmwrk_task_group_upd`, `spu_pmwrk_task_group_del`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMTaskGroupCategory
**Directory:** `PMTaskGroupCategory/`
**Purpose:** **Task group ↔ category assignment.** Manages the many-to-many relationship between task groups and task categories.

**Key Classes:**
- `Business` — group-category mapping (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetGroupCategories` | `(v_lGroupID, ByRef r_vCategories) As Integer` | Get categories for a group | `spu_pmwrk_task_group_cat_sel` |
| `AddGroupCategory` | `(v_lGroupID, v_lCategoryID) As Integer` | Assign category to group | `spu_pmwrk_task_group_cat_add` |
| `DeleteGroupCategory` | `(v_lGroupID, v_lCategoryID) As Integer` | Remove category from group | `spu_pmwrk_task_group_cat_del` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_pmwrk_task_group_cat_sel`, `spu_pmwrk_task_group_cat_add`, `spu_pmwrk_task_group_cat_del`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMMaintainTask
**Directory:** `PMMaintainTask/`
**Purpose:** **Task definition maintenance UI.** WinForm for administrators to manage task templates — create, edit, delete task definitions with their properties and validation rules.

**Key Forms/Classes:**
- UI form — task definition maintenance screen
- `Business` — task data access

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `GetTasks` | `(ByRef r_vTasks) As Integer` | List task definitions (non-deleted) | `spu_PMwrk_Task_NotDeleted_Select` |
| `SaveTask` | `(...) As Integer` | Add or update task definition | SP-based |
| `DeleteTask` | `(v_lTaskID) As Integer` | Soft-delete task definition | SQL UPDATE |

**Stored Procedures:**
- `spu_PMwrk_Task_NotDeleted_Select`

**References:** `PMTask.Business`, `gPMConstants`, `SharedFiles`

---

### PMMaintainTaskAction
**Directory:** `PMMaintainTaskAction/`
**Purpose:** **Task action type CRUD UI.** WinForm for managing the types of actions that can be associated with tasks (e.g., "Approve", "Reject", "Escalate").

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `GetActions` | `(ByRef r_vActions) As Integer` | List action types | `spu_PMwrk_Task_Action_Type_Select` |
| `AddAction` | `(v_sDescription, ...) As Integer` | Create action type | `spu_PMwrk_Task_Action_Type_Add` |
| `UpdateAction` | `(v_lActionID, v_sDescription, ...) As Integer` | Update action type | `spu_PMwrk_Task_Action_Type_Update` |

**Stored Procedures:**
- `spu_PMwrk_Task_Action_Type_Select`, `spu_PMwrk_Task_Action_Type_Add`, `spu_PMwrk_Task_Action_Type_Update`

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMMaintainTaskGroupAction
**Directory:** `PMMaintainTaskGroupAction/`
**Purpose:** **Task group action UI.** WinForm for managing which actions are available within specific task groups.

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `GetTaskGroupDetails` | `(v_lGroupID, ByRef r_vDetails) As Integer` | Get tasks in a group with details | `spu_PMwrk_Get_Task_Group_Task_Details` |
| `GetGroupActions` | `(v_lGroupID, ByRef r_vActions) As Integer` | Get available actions for a group | `spu_PMwrk_Get_Task_Group_Task_Action` |

**Stored Procedures:**
- `spu_PMwrk_Get_Task_Group_Task_Details`, `spu_PMwrk_Get_Task_Group_Task_Action`

**References:** `dPMDAO.Database`, `PMTaskGroup.Business`, `gPMConstants`, `SharedFiles`

---

### PMTaskGroupMaintenance
**Directory:** `PMTaskGroupMaintenance/`
**Purpose:** **Task group maintenance UI.** WinForm for admins to manage task groups — group creation, member task assignment, and group properties.

**Key Forms:**
- `frmTaskGroupMaintenance` — group admin form

**References:** `PMTaskGroup.Business`, `PMTask.Business`, `gPMConstants`, `SharedFiles`

---

### PMTaskMaintenance
**Directory:** `PMTaskMaintenance/`
**Purpose:** **Task maintenance UI.** WinForm providing a comprehensive task management interface including task creation, editing, category assignment, and authority configuration.

**Key Forms:**
- `frmTaskMaintenance` — combined task admin form

**References:** `PMTask.Business`, `PMTaskCategory.Business`, `gPMConstants`, `SharedFiles`

---

### PMTaskOutcomeBatch
**Directory:** `PMTaskOutcomeBatch/`
**Type:** Console EXE
**Purpose:** **Batch processor for overdue tasks.** Scheduled batch process that evaluates overdue task outcomes, escalates tasks, and updates task statuses.

**Key Classes:**
- `Business` — batch processing logic

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetOverdueTasks` | `(ByRef r_vTasks) As Integer` | Retrieve tasks past due date | `spu_PM_Batch_Get_Overdue_Tasks` |
| `UpdateTaskOutcome` | `(v_lTaskInstanceCnt, v_iOutcome) As Integer` | Set task outcome | `spu_PM_Batch_Update_Overdue_Tasks` |
| `ProcessBatch` | `() As Integer` | Run full batch cycle | Multiple SP calls |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures:**
- `spu_PM_Batch_Get_Overdue_Tasks` — select overdue tasks
- `spu_PM_Batch_Update_Overdue_Tasks` — batch-update task outcomes

**References:** `dPMDAO.Database`, `bPMWrkTaskInstance`, `gPMConstants`, `SharedFiles`

---

### PMPackageStep
**Directory:** `PMPackageStep/`
**Purpose:** **Workflow step execution engine.** Manages the creation and execution of individual workflow step instances within a package/process. Tracks step status, input/output, and completion.

**Key Classes:**
- `Business` — step operations (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `CreateStepInstance` | `(v_lWorkflowStepID, v_lProcessID, ..., ByRef r_lStepInstanceID) As Integer` | Create a step instance | `spu_pmwrk_work_step_instance_insert` |
| `UpdateStepInstance` | `(v_lStepInstanceID, v_iStatus, ...) As Integer` | Update step status | `spu_pmwrk_work_step_instance_update` |
| `GetStepInstance` | `(v_lStepInstanceID, ByRef r_oStep) As Integer` | Retrieve step instance details | `spu_pmwrk_work_step_instance_select` |
| `CompleteStep` | `(v_lStepInstanceID) As Integer` | Mark step as complete | `spu_pmwrk_work_step_instance_complete` |
| `GetProcessSteps` | `(v_lProcessID, ByRef r_vSteps) As Integer` | Get all steps for a process | SQL SELECT |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures (11+):**
- `spu_pmwrk_work_step_instance_insert`, `spu_pmwrk_work_step_instance_update`
- `spu_pmwrk_work_step_instance_select`, `spu_pmwrk_work_step_instance_complete`
- `spu_pmwrk_work_step_instance_delete`
- And more `spu_pmwrk_work_step_*` SPs

**References:** `dPMDAO.Database`, `Navigator V3`, `gPMConstants`, `SharedFiles`

---

### PMWrkManager
**Directory:** `PMWrkManager/`
**COM ProgId:** `bPMWrkManager.FormClass`
**Purpose:** **Work manager** — the central task scheduling, retrieval, assignment, and status tracking component. Provides the task list, quick-start toolbar, task filtering, and user authority checking. This is the primary component used by the desktop Work Manager UI.

**Key Classes:**
- `FormClass` — main business class (implements `IBusiness`)
- `TaskControl` — task control wrapper

**Key Methods (25+):**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params, [bStandAlone], [vDatabase]) As Long` | Initialize with user context | `spu_PM_Get_Default_UserGroup_For_TaskGroup` |
| `GetScheduledTasks` | `(ByRef r_vArray, [v_lTaskStatus], [v_lPmuserGroupID], [v_iUserID], [v_dtDueDateLimit], [v_bOmitCompleted], [vGroups], [PartyKey]) As Integer` | Retrieve scheduled tasks with filtering | `spu_PM_Get_Scheduled_Tasks` |
| `GetBatchTasks` | `(ByRef r_dtBatchTask, sTaskStatus, v_dtDueDateLimit) As Integer` | Get batch-mode tasks | `spu_PM_Get_Batch_Tasks` |
| `GetScheduledSystemTasks` | `(ByRef r_vArray, [v_dtDueDateLimit], [PartyKey]) As Integer` | Get system-only tasks | `spu_PM_Get_Scheduled_Tasks` |
| `GetTaskInstByKey` | `(v_sKeyName, v_sKeyValue, ByRef r_vArray, ...) As Integer` | Query tasks by key name/value | Multiple SP calls |
| `GetAvailableTasks` | `(ByRef r_vArray) As Integer` | Get unassigned tasks | `spu_pmwrk_users_tasks_sel` |
| `GetDetails` | `(v_lTaskInstanceCnt, ByRef ...) As Integer` | Retrieve single task details | SQL SELECT |
| `GetTaskInstKeys` | `(v_lTaskInstanceCnt, ByRef r_vKeyArray) As Integer` | Get task navigation keys | SQL SELECT |
| `GetUserAuthority` | `(ByRef r_bIsAdmin, ByRef r_vSupervisedGroups) As Integer` | Check user's task authority | `spu_pmwrk_check_is_supervisor`, `spu_pmuser_is_sysadmin` |
| `Assign` | `(v_lTaskInstanceCnt, v_iUserID) As Integer` | Assign task to user | SQL UPDATE |
| `ReAssign` | `(v_lTaskInstanceCnt, v_lGroupID, [v_iUserID]) As Integer` | Reassign task | SQL UPDATE |
| `SetStatusComplete` | `(v_lTaskInstanceCnt) As Integer` | Mark task complete | SQL UPDATE |
| `SetStatusInComplete` | `(v_lTaskInstanceCnt) As Integer` | Mark task incomplete | SQL UPDATE |
| `SetStatusInProgress` | `(v_lTaskInstanceCnt) As Integer` | Mark task in progress | SQL UPDATE |
| `Delete` | `(v_lTaskInstanceCnt) As Integer` | Delete task instance | `spu_pmwrk_task_inst_del` |
| `CreateTaskInstance` | `(v_lGroupID, v_lTaskID, v_sCustomer, v_dtDueDate, v_lUserGroupID, v_sDescription, v_iStatus, v_iIsUrgent, ByRef r_lInstanceCnt, [v_iIsVisible]) As Integer` | Create new task | `spe_PMWrk_Task_Instance_add` |
| `AddQuickStartTask` | `(v_lGroupID, v_lTaskID, v_lDisplaySeq) As Integer` | Add to quick-start toolbar | `spe_PMWrk_User_Quick_Start_add` |
| `DeleteQuickStartTasks` | `() As Integer` | Clear all quick-start tasks | `spu_PMWrk_Users_Quick_Start_dar` |
| `DeleteSingleQuickStartTask` | `(pmwrk_task_group_id, pmwrk_task_id) As Integer` | Remove one quick-start task | `spe_PMWrk_User_Quick_Start_delete` |
| `GetQuickStartTasks` | `(ByRef r_vArray) As Integer` | Retrieve quick-start tasks | `spu_PMWrk_Users_Quick_Start_saa` |
| `LockTaskInstance` | `(v_lTaskInstanceCnt, ByRef r_sLockedBy) As Integer` | Lock task for editing | `bPMLock` operations |
| `UnlockTaskInstance` | `(v_lTaskInstanceCnt) As Integer` | Release task lock | `bPMLock` operations |
| `GetDefaultUserGroupForTaskGroup` | `(v_iUserID, v_lTaskGroupID, ByRef v_lUserGroupID) As Integer` | Get default group | `spu_PM_Get_Default_UserGroup_For_TaskGroup` |
| `GetAgents` | `(ByRef r_vArray, ByRef r_iCurrentAgent) As Integer` | Get agent list | SQL SELECT |

**Stored Procedures:**
- `spu_PM_Get_Scheduled_Tasks`, `spu_PM_Get_Batch_Tasks`
- `spu_pmwrk_users_tasks_sel`
- `spu_pmwrk_check_is_supervisor`, `spu_pmuser_is_sysadmin`
- `spu_pmwrk_task_inst_del`
- `spe_PMWrk_Task_Instance_add`
- `spe_PMWrk_User_Quick_Start_add`, `spe_PMWrk_User_Quick_Start_delete`
- `spu_PMWrk_Users_Quick_Start_saa`, `spu_PMWrk_Users_Quick_Start_dar`
- `spu_PM_Get_Default_UserGroup_For_TaskGroup`

**References:** `bPMWrkTaskInstance`, `bPMLock`, `dPMDAO.Database`, `gPMConstants`, `gPMComponentServices`

---

### PMWrkTaskInstance
**Directory:** `PMWrkTaskInstance/`
**COM ProgId:** `bPMWrkTaskInstance.Business`
**Purpose:** **Task instance CRUD, status management, and logging.** Creates, modifies, assigns, completes, and deletes individual task instances. Also handles batch reassignment and background job creation.

**Key Classes:**
- `Business` — main business class
- `PMWrkTaskInstance` — task instance data object
- `TaskControl` — form control wrapper
- `bPMWrkTaskInstLog.Logging` — task audit logging

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params, [bStandAlone], [vDatabase]) As Long` | Initialize | Registry/DB init |
| `CreateNew` | `(v_lTaskID, v_lGroupID, v_sCustomer, v_dtDueDate, v_lUserGroupID, v_sDescription, v_iStatus, v_iIsUrgent, ByRef r_lInstanceCnt, [...20+ params]) As Integer` | Create new task with keys | `spe_PMWrk_Task_Instance_add` |
| `GetDetails` | `(v_lInstanceCnt, ByRef ...) As Integer` | Retrieve task instance data | `spe_PMWrk_Task_Instance_sel` |
| `GetTaskInstKeys` | `(v_lInstanceCnt, ByRef r_vKeyArray) As Integer` | Get task navigation keys | `spu_pmwrk_task_inst_keys_saa` |
| `AmendDetails` | `(v_lInstanceCnt, v_sCustomer, v_dtDueDate, ...) As Integer` | Update task properties | `spe_PMWrk_Task_Instance_upd` |
| `Assign` | `(v_lInstanceCnt, v_iUserID) As Integer` | Assign to user | SQL UPDATE |
| `ReAssign` | `(v_lInstanceCnt, v_lUserGroupID, [v_iUserID]) As Integer` | Reassign | `spe_PMWrk_Task_Instance_Multiple_upd` |
| `SetStatusComplete` | `(v_lInstanceCnt) As Integer` | Mark complete | SQL UPDATE |
| `SetStatusInComplete` | `(v_lInstanceCnt) As Integer` | Mark incomplete | SQL UPDATE |
| `SetStatusInProgress` | `(v_lInstanceCnt) As Integer` | Mark in progress | SQL UPDATE |
| `Delete` | `(v_lInstanceCnt) As Integer` | Delete instance | `spu_pmwrk_task_inst_del` |
| `AutoDelete` | `() As Integer` | Auto-delete expired tasks | `spu_pmwrk_task_inst_auto_del` |
| `ReAssignMultipleTask` | `(v_vInstanceCntArray, v_lUserGroupID, [v_iUserID]) As Integer` | Batch reassign | `spe_PMWrk_Task_Instance_Multiple_upd` |
| `CreateBackgroundJob` | `(ByRef o_nJobID, r_sJobXml, ...) As Integer` | Queue background job | `spu_SIR_Background_Job_add` |

**Stored Procedures:**
- `spe_PMWrk_Task_Instance_sel`, `spe_PMWrk_Task_Instance_add`, `spe_PMWrk_Task_Instance_upd`
- `spu_pmwrk_task_inst_del`, `spu_pmwrk_task_inst_keys_saa`, `spu_pmwrk_task_inst_auto_del`
- `spe_PMWrk_Task_Instance_Multiple_upd`
- `spu_SIR_Background_Job_add`
- `spu_PMWrk_Task_Group_val` (validation)

**References:** `dPMDAO.Database`, `PMLock`, `gPMConstants`, `gPMComponentServices`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- PM USER MANAGEMENT                                            -->
<!-- ============================================================ -->

## PM User Management

### PMUser
**Directory:** `PMUser/`
**COM ProgId:** `bPMUser.User`
**Purpose:** **Core user management.** Handles user logon/logoff, credential validation (Standard/Mixed/Unified auth modes), user CRUD, password management, authority checking, and branch assignment. This is the central user security component.

**Key Classes:**
- `User` — user management (implements `IBusiness`)

**Auth Modes:**
- **Standard** — Sirius username/password
- **Mixed** — Windows AD + Sirius password fallback
- **Unified** — KeyCloak / SSO only

**Key Methods (15+):**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `Logon` | `(v_sUsername, v_sPassword, ByRef r_iUserID) As Integer` | Authenticate user | `spu_pmuser_logon` |
| `Logoff` | `(v_iUserID) As Integer` | End user session | `spu_pmuser_logoff` |
| `GetUserDetails` | `(v_iUserID, ByRef ...) As Integer` | Retrieve user record | `spu_get_user_details` |
| `AddUser` | `(v_sUsername, v_sPassword, ..., ByRef r_iUserID) As Integer` | Create user | `spu_Update_User_Record` |
| `UpdateUser` | `(v_iUserID, v_sUsername, ...) As Integer` | Update user record | `spu_Update_User_Record` |
| `DeleteUser` | `(v_iUserID) As Integer` | Soft-delete user | SQL UPDATE |
| `ChangePassword` | `(v_iUserID, v_sOldPassword, v_sNewPassword) As Integer` | Change password | `spu_update_user_password` |
| `ValidatePassword` | `(v_sPassword) As Boolean` | Check password complexity rules | — |
| `GetUserSources` | `(v_iUserID, ByRef r_vSources) As Integer` | Get user's assigned branches | `spu_pmuser_get_user_sources` |
| `SetUserSource` | `(v_iUserID, v_iSourceID) As Integer` | Assign user to branch | SQL INSERT |
| `IsSystemAdmin` | `(v_iUserID, ByRef r_bIsAdmin) As Boolean` | Check sysadmin flag | `spu_pmuser_is_sysadmin` |
| `IsSupervisor` | `(v_iUserID, ByRef r_bIsSupervisor) As Boolean` | Check supervisor flag | `spu_pmuser_is_supervisor` |
| `GetAllUsers` | `(ByRef r_vUsers) As Integer` | List all users | SQL SELECT |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures (15+):**
- `spu_pmuser_logon`, `spu_pmuser_logoff`
- `spu_get_user_details`, `spu_Update_User_Record`
- `spu_update_user_password`
- `spu_pmuser_get_user_sources`
- `spu_pmuser_is_sysadmin`, `spu_pmuser_is_supervisor`
- `spu_pmuser_validate_credentials`
- And more `spu_pmuser_*` SPs

**References:** `dPMDAO.Database`, `gPMConstants`, `gPMFunctions`, `SharedFiles`, `System.Security.Cryptography`

---

### PMUserGroup
**Directory:** `PMUserGroup/`
**COM ProgId:** `bPMUserGroup.UserGroup`
**Purpose:** **User group hierarchy and permissions.** Manages user groups, group membership, supervisor relationships, and group-level task authority. Provides 17+ stored procedure operations.

**Key Classes:**
- `UserGroup` — group management (implements `IBusiness`)

**Key Methods:**

| Method | Signature | Description | SPs Called |
|--------|-----------|-------------|------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component | DB open |
| `GetGroups` | `(ByRef r_vGroups) As Integer` | List all user groups | `spu_pmuser_group_sel` |
| `GetGroupMembers` | `(v_lGroupID, ByRef r_vMembers) As Integer` | Get users in a group | `spu_pmuser_group_members_sel` |
| `AddGroup` | `(v_sDescription, ..., ByRef r_lGroupID) As Integer` | Create group | `spu_pmuser_group_add` |
| `UpdateGroup` | `(v_lGroupID, v_sDescription, ...) As Integer` | Update group | `spu_pmuser_group_upd` |
| `DeleteGroup` | `(v_lGroupID) As Integer` | Delete group | `spu_pmuser_group_del` |
| `AddMember` | `(v_lGroupID, v_iUserID) As Integer` | Add user to group | `spu_pmuser_group_member_add` |
| `RemoveMember` | `(v_lGroupID, v_iUserID) As Integer` | Remove user from group | `spu_pmuser_group_member_del` |
| `IsSystemAdmin` | `(v_iUserID, ByRef r_bIsAdmin) As Boolean` | Check admin authority | `spu_pmuser_is_sysadmin` |
| `IsSupervisor` | `(v_iUserID, ByRef r_bIsSupervisor) As Boolean` | Check supervisor authority | `spu_pmuser_is_supervisor` |
| `GetSupervisedGroups` | `(v_iUserID, ByRef r_vGroups) As Integer` | Get groups user supervises | `spu_pmuser_supervised_groups_sel` |
| `Dispose` | `()` | Release resources | — |

**Stored Procedures (17+):**
- `spu_pmuser_group_sel`, `spu_pmuser_group_add`, `spu_pmuser_group_upd`, `spu_pmuser_group_del`
- `spu_pmuser_group_members_sel`, `spu_pmuser_group_member_add`, `spu_pmuser_group_member_del`
- `spu_pmuser_is_sysadmin`, `spu_pmuser_is_supervisor`
- `spu_pmuser_supervised_groups_sel`
- And more `spu_pmuser_group_*` SPs

**References:** `dPMDAO.Database`, `gPMConstants`, `SharedFiles`

---

### PMUserMaintenance
**Directory:** `PMUserMaintenance/`
**Purpose:** **User admin UI** with Active Directory / LDAP support. WinForm for creating, editing, and managing user accounts, group membership, branch assignments, and password policies. Supports AD import and LDAP authentication configuration.

**Key Forms:**
- `frmUserMaintenance` — user admin form
- `frmGroupMaintenance` — group admin form

**Key Features:**
- User CRUD with grid display
- AD/LDAP user import and synchronisation
- Group membership management
- Branch/source assignment
- Password policy enforcement
- KeyCloak sync integration (via `BPMUsersSync`)

**References:** `PMUser.Business`, `PMUserGroup.UserGroup`, `BPMUsersSync`, `gPMConstants`, `SharedFiles`

---

### PMTestLogon
**Directory:** `PMTestLogon/`
**Purpose:** **Test logon utility.** Simple test harness for verifying logon sequences and authentication flows.

**References:** `iLogonManager`, `bClientManager`, `gPMConstants`

---

<!-- ============================================================ -->
<!-- SIRIUS ARCHITECTURE LIBRARIES (C#)                            -->
<!-- ============================================================ -->

## Sirius Architecture Libraries (C#)

### Sirius.Achitecture.Configuration.Local
**Directory:** `Sirius.Achitecture.Configuration.Local/`
**Language:** C#
**Framework:** .NET Framework
**Purpose:** **Strongly-typed Windows Registry access** compatible with VB6 Sirius code. Reads and writes registry values using the 32-bit registry view for backward compatibility with legacy COM components.

**Key Classes:**
- `SiriusRegistryAccess` — static read/write methods
- `SiriusRegistryRead` — registry reading utilities
- `SiriusRegistryWrite` — registry writing utilities
- `SwiftRegistryAccess` — Swift-specific access
- `SwiftIniFileAccess` — INI file access
- `RegistryKey4`, `RegistryView4` — registry abstractions

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetValueAsBoolean` | `(hive, keyName, name, defaultValue) → Boolean` | Read boolean from registry |
| `GetValueAsByte` | `(hive, keyName, name, defaultValue) → Byte` | Read byte from registry |
| `GetValueAsInt16/Int32/Int64` | `(hive, keyName, name, defaultValue) → T` | Read integer values |
| `GetValueAsDecimal/Double/Single` | `(hive, keyName, name, defaultValue) → T` | Read floating-point values |
| `GetValueAsString` | `(hive, keyName, name, defaultValue) → String` | Read string from registry |
| `SetValue` | `(hive, keyName, name, value) → void` | Write to registry |

**Features:**
- 32-bit registry view for VB6 compatibility
- Sirius legacy persistence format support
- No in-memory caching (always fresh reads)
- Thread-safe registry access

**References:** `Microsoft.Win32.Registry`

---

### Sirius.Achitecture.Data
**Directory:** `Sirius.Achitecture.Data/`
**Language:** C#
**Framework:** .NET Framework
**Purpose:** **SQL Server database command abstraction** and connection layer. Provides `SiriusCommand` for stored procedure and text command execution, and `SiriusConnection` implementations for SQL Server.

**Key Classes:**
- `SiriusCommand` — SQL command wrapper (stored procedures and text commands)
- `SiriusConnection` — abstract database connection
- `SiriusConnectionSqlClient` — SQL Server implementation
- `SiriusCommandBehaviour` — command execution flags
- `TransactionBehaviour` — transaction handling modes
- `SqlStatementBuilder` — SQL query builder

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SiriusCommand.FromProcedure` | `(procedureName, [commandTimeout], [behaviour]) → SiriusCommand` | Create stored procedure command |
| `SiriusCommand.FromText` | `(text, [commandTimeout]) → SiriusCommand` | Create SQL text command |
| `SiriusConnection.FromAny` | `(connectionString, [schema], [timeout], [transBehaviour]) → SiriusConnection` | Create connection from string |
| `SiriusConnection.FromSirius` | `(...connection params...) → SiriusConnection` | Create Sirius production connection |
| `SiriusConnection.FromNamedSource` | `(dataSourceName, [schema]) → SiriusConnection` | Create DSN-based connection |

**Constants:**
- `TimestampSize = 8`
- `ImageSizeOnInput = 2GB`
- `TextSizeOnInput = 2GB`
- `VarCharMaxSizeOnOutput = 0x7FFFFFFE`

**References:** `System.Data.SqlClient`

---

### Sirius.Achitecture.Data.BackOffice
**Directory:** `Sirius.Achitecture.Data.BackOffice/`
**Language:** C#
**Framework:** .NET Framework
**Purpose:** **Back-office database connection bridge.** Wraps `dPMDAO` as a `SiriusConnection` for use by modern C# back-office components. Same as `dPMDAOBridge` but specifically targeting the Sirius (back-office) schema.

**Key Classes:**
- `SiriusConnectionPMDAO` — dPMDAO wrapper extending `SiriusConnection`
- `PmdaoConnectionInfo` — connection information DTO
- `AdoDataReader` — ADO recordset adapter for `IDataReader`

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `SiriusConnectionPMDAO` | `(siriusUserName, sourceID, languageID, ...) → Instance` | Constructor from dPMDAO parameters |
| `InitialiseDatabase` | `() → Void` | Initialize internal dPMDAO.Database instance |
| `GetObjectData` | `(info, context) → void` | Serialization support |

**Features:**
- Bridges legacy dPMDAO COM with modern .NET
- Back-office (Sirius) schema support
- Cloned connection info (no sharing)
- Command timeout inherited from dPMDAO

**References:** `dPMDAO`, `Sirius.Achitecture.Data`

---

### Sirius.Achitecture.Utility
**Directory:** `Sirius.Achitecture.Utility/`
**Language:** C#
**Framework:** .NET Framework
**Purpose:** **Type casting and data conversion utilities** for safe database NULL handling. All `Cast_*` types handle NULL representations safely and prevent invalid string-to-non-string conversions.

**Key Classes:**
- `Cast` — safe casting dispatcher
- `Cast_Boolean`, `Cast_Byte`, `Cast_DateTime`, `Cast_Decimal`, `Cast_Double`, `Cast_Int16`, `Cast_Int32`, `Cast_Int64`, `Cast_Single`, `Cast_String` — type-specific converters
- `StringDataConvert` — string conversion utilities
- `BooleanDataConvert` — boolean conversion helpers
- `XmlUtility` — XML helper methods
- `HashHelper` — hashing functions
- `StringSplit` — string splitting with options

**Key Features:**
- Handles all NULL value representations (DBNull, Nothing, empty string)
- NO conversions from string to non-string types (design constraint)
- Database-safe type conversions
- VB6 legacy format support

---

### Sirius.Architecture.Data
**Directory:** `Sirius.Architecture.Data/`
**Language:** C#
**Framework:** .NET Framework
**Purpose:** **Abstract `SiriusConnection` base class** for all Sirius database connections. This is the interface/abstract layer that `Sirius.Achitecture.Data` and `Sirius.Achitecture.Data.BackOffice` implement.

**Factory Methods:**

| Method | Description |
|--------|-------------|
| `FromAny(connectionString, [schema], [timeout], [transBehaviour])` | Create arbitrary connection |
| `FromSirius(...)` | Create Sirius production connection |
| `FromSiriusViaPMDAO(...)` | Create Sirius connection via dPMDAO bridge |
| `FromNamedSource(dataSourceName)` | Create DSN-based connection |

**Key Properties:**
- `CommandTimeout` — default query timeout
- `Schema` — database schema (Sirius/Swift/Unknown)
- `CacheKey` — unique connection identifier

---

<!-- ============================================================ -->
<!-- SHARED SERVICES & SYNC                                        -->
<!-- ============================================================ -->

## Shared Services & Sync

### SSP.Pure.UsersSync
**Directory:** `SSP.Pure.UsersSync/`
**Language:** C#
**Purpose:** **KeyCloak user synchronisation** REST API client. Provides methods to register, update, retrieve, and authenticate users against KeyCloak. Used alongside `BPMUsersSync` for user management.

**Key Classes:**
- `AuthenticationService` — KeyCloak token & user operations
- `UserRegisterRequestDTO` — user registration request
- `UserResponseDTO` — user response data
- `KeycloakConfiguration` — KeyCloak connection settings
- `ErrorMessage` — error handling

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetAllUsersAsync` | `() → Task<IReadOnlyList<UserResponseDTO>>` | Retrieve all KeyCloak users |
| `GetUserAsync` | `(userName) → Task<string>` | Get user ID by username |
| `RegisterUserAsync` | `(request) → Task<AuthResponseDTO>` | Register new user in KeyCloak |
| `GetAdminTokenAsync` | `() → Task<KeycloakToken>` | Get admin access token |
| `UpdateUserAsync` | `(userId, request) → Task<AuthResponseDTO>` | Update user properties |

**Configuration:**
- Token Endpoint: KeyCloak OAuth2 endpoint
- HTTP Client: 5000s timeout, default credentials
- Auth Header: `"Bearer {AccessToken}"`

**References:** `System.Net.Http`, `Newtonsoft.Json`

---

### Sirius Cache Controller
**Directory:** `Sirius Cache Controller/`
**Purpose:** **Application cache management.** Manages XML-based cache files — list cache keys, clear individual keys, or flush the entire cache.

**Key Classes:**
- `Business` — cache operations (Business/)
- UI form — cache admin screen (Interface/)

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize with ObjectManager |
| `GetCacheKeyArray` | `(ByRef r_vCacheKeyArray) As Integer` | List all cache keys (XML files in cache path) |
| `ClearCache` | `([v_sKey]) As Integer` | Clear all cache or a single key |
| `Dispose` | `() As Integer` | Release resources |

**Implementation:**
- Cache location: registry-configured path
- Cache format: `*.xml` files
- Cache key resolution: `SIRIUS_CACHE_KEYS` registry path

**References:** `bObjectManager`, `Microsoft.Win32.Registry`, `gPMConstants`

---

<!-- ============================================================ -->
<!-- APPLICATIONS & USER CONTROLS                                  -->
<!-- ============================================================ -->

## Applications & User Controls

### UsersLoggedOn
**Directory:** `UsersLoggedOn/`
**Type:** WinForm EXE
**Purpose:** **Display currently logged-on users** in the Sirius system. Outputs user list to both a grid and a text file.

**Key Forms:**
- `frmUsers` — main user list form
- `frmMessage` — notification form

**Key Entry Point:**

| Function | Description |
|----------|-------------|
| `Main()` | Initialize `bPMLicenceAdmin.LicenceAdmin`, populate user grid |
| `DisplayUserList()` | Populate grid with active users |

**Output:** `C:\UsersLoggedOn.txt`

**References:** `bPMLicenceAdmin.LicenceAdmin`, `SharedFiles`

---

### User Controls (uSIRCommonControls)
**Directory:** `User Controls/uSIRCommonControls/`
**Purpose:** **Common UI controls** for Sirius desktop applications. Provides reusable anchor and divider controls.

**Key Classes:**
- `AnchorEntry` — anchor position data holder
- `uctAnchor` — control anchor behaviour
- `uctDivider` — visual divider control

**Key Properties:**

| Property | Type | Description |
|----------|------|-------------|
| `Control` | `Control` | Target control to anchor |
| `Anchor` | `ControlAnchorEnum` | Anchor position (Top/Bottom/Left/Right) |
| `Key` | `String` | Control identifier (Name + Index) |

---

### PMZipper
**Directory:** `PMZipper/`
**Purpose:** **ZIP compression / decompression.** Provides file and directory compression using `System.IO.Compression`.

**Key Classes:**
- `Business` — compression operations

**Key Methods:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(standard IBusiness params) As Long` | Initialize component |
| `ZipFile` | `(v_sZipFileName, v_sItemList, [v_sTempPath], [v_sComment]) As Integer` | Create ZIP archive from file list |
| `ZipDirectory` | `(v_sZipFileName, v_sDirectory, [v_sComment]) As Integer` | ZIP an entire directory |
| `UnZipFile` | `(v_sZipFileName, v_sDestination, [v_sPassword]) As Integer` | Extract ZIP archive |
| `Dispose` | `()` | Release resources |

**References:** `System.IO.Compression`, `SharedFiles`

---

<!-- ============================================================ -->
<!-- UTILITIES                                                     -->
<!-- ============================================================ -->

## Utilities

### _Utilities (CreateEnvironmentVariables)
**Directory:** `_Utilities/CreateEnvironmentVariables/`
**Language:** C#
**Type:** Console EXE
**Purpose:** **Set environment variables** for Sirius deployments. Supports Windows (Machine scope) and Linux/macOS (~/.bashrc export).

**Key Entry Point:**

| Function | Description |
|----------|-------------|
| `Main(args)` | Prompt user for `Client_Secret` value and set as environment variable |

**Platform Handling:**
- **Windows:** `Environment.SetEnvironmentVariable(..., EnvironmentVariableTarget.Machine)`
- **Linux/macOS:** Append `export` statement to `~/.bashrc`

---

<!-- ============================================================ -->
<!-- CROSS-REFERENCES                                              -->
<!-- ============================================================ -->

## Cross-Reference: Components ↔ Stored Procedures

| Stored Procedure | Called By |
|-----------------|-----------|
| `spu_get_overdue_tasks` | `bSIROverdueTaskCheck.GetOverdueTasks` |
| `spu_create_check_task` | `bSIROverdueTaskCheck.CreateTaskToAlertSupervisor`, `bSIRUserCompetenceTask.CreateTask` |
| `spu_get_system_option` | `ClientManager DCOM` |
| `spu_pm_get_user_sources` | `ClientManager DCOM`, `PMUser` |
| `spu_get_user_details` | `PasswordRehasher`, `PMUser` |
| `spu_pm_allocate_number` | `PMAutoNumber` |
| `spu_pm_GenerateReference` | `PMAutoNumber` |
| `spu_pm_Select_captions` | `PMCaption`, `PMLookup` |
| `spu_currency_sel/add/upd/del` | `PMCurrency` |
| `spe_PMWrk_Get_Due_Tasks` | `PMEventTask` |
| `spu_PM_Update_Event_Task` | `PMEventTask` |
| `spu_PMCheck_TS` | `PMLock` |
| `spu_PMAdd_Lock` | `PMLock` |
| `spu_PMDelete_Lock` | `PMLock` |
| `spu_pm_get_eff_id_from_code` | `PMLookup` |
| `spu_pm_Get_Columns` | `PMMaintainLookup` |
| `spu_pm_caption_id_return` | `PMMaintainLookup` |
| `spu_PMwrk_Task_NotDeleted_Select` | `PMMaintainTask` |
| `spu_PMwrk_Task_Action_Type_*` | `PMMaintainTaskAction` |
| `spu_PMwrk_Get_Task_Group_Task_Details` | `PMMaintainTaskGroupAction` |
| `spu_pmwrk_work_step_instance_*` | `PMPackageStep` |
| `spu_pmproduct_lookup_sel/add/upd/del` | `PMProductLookup` |
| `spu_PM_Select/Add/Update/Delete_Source` | `PMSource` |
| `spu_pm_iccs` | `PMSystem` |
| `spg_GetValidSystem` | `PMSystem` |
| `spu_GetNoLicencesInUse` | `PMSystem` |
| `spu_add/update_PMSystem` | `PMSystem` |
| `spu_pmwrk_task_sel/add/upd` | `PMTask` |
| `spu_pmwrk_task_code_sel` | `PMTask` |
| `spu_GetUserAuthorityTask` | `PMTask` |
| `spu_pmwrk_task_category_*` | `PMTaskCategory` |
| `spu_pmwrk_task_group_*` | `PMTaskGroup` |
| `spu_pmwrk_task_group_cat_*` | `PMTaskGroupCategory` |
| `spu_PM_Batch_Get_Overdue_Tasks` | `PMTaskOutcomeBatch` |
| `spu_PM_Batch_Update_Overdue_Tasks` | `PMTaskOutcomeBatch` |
| `spu_pmuser_logon/logoff` | `PMUser` |
| `spu_Update_User_Record` | `PMUser` |
| `spu_pmuser_is_sysadmin` | `PMUser`, `PMUserGroup`, `PMWrkManager` |
| `spu_pmuser_is_supervisor` | `PMUser`, `PMUserGroup` |
| `spu_pmuser_group_*` | `PMUserGroup` |
| `spu_PM_Get_Scheduled_Tasks` | `PMWrkManager` |
| `spu_PM_Get_Batch_Tasks` | `PMWrkManager` |
| `spu_pmwrk_users_tasks_sel` | `PMWrkManager` |
| `spu_pmwrk_check_is_supervisor` | `PMWrkManager` |
| `spu_pmwrk_task_inst_del` | `PMWrkManager`, `PMWrkTaskInstance` |
| `spe_PMWrk_Task_Instance_add` | `PMWrkManager`, `PMWrkTaskInstance` |
| `spe_PMWrk_User_Quick_Start_add/delete` | `PMWrkManager` |
| `spu_PMWrk_Users_Quick_Start_saa/dar` | `PMWrkManager` |
| `spu_PM_Get_Default_UserGroup_For_TaskGroup` | `PMWrkManager` |
| `spe_PMWrk_Task_Instance_sel/upd` | `PMWrkTaskInstance` |
| `spu_pmwrk_task_inst_keys_saa` | `PMWrkTaskInstance` |
| `spu_pmwrk_task_inst_auto_del` | `PMWrkTaskInstance` |
| `spe_PMWrk_Task_Instance_Multiple_upd` | `PMWrkTaskInstance` |
| `spu_SIR_Background_Job_add` | `PMWrkTaskInstance` |
| `spu_pmnav_*` | `Navigator V3` |
| `spu_PMwrk_Workflow_Step_Select/Insert/Update` | `Workflow (PMStepMaintenance)` |
| `spu_pmuser_get_allowed_branches` | `Workflow (PMStepMaintenance)` |

---

## Cross-Reference: Components ↔ External Dependencies

| External Dependency | Used By | Purpose |
|--------------------|---------|---------|
| `bPMSystem.Business` | `bLicenceManager`, `bPMLicenceManager`, `PMSystem` | System record & licence validation |
| `bClientManager.ClientManager` | `bLicenceManager`, `bObjectManager`, `bPMLicenceManager`, `iLogonManager` | User session / state management |
| `iLogonManager.LogonManager` | `bObjectManager`, `Ilogonserver` | .NET Remoting-based logon coordination |
| `iPMMessage.PMMessage` | `bObjectManager`, `PMMessage` | Enterprise Library message logging |
| `dPMDAO.Database` | Nearly all VB.NET business components | Database access layer |
| `gPMConstants` | All components | Application constants |
| `gPMFunctions` | `bObjectManager`, `bPMLicenceManager`, `PMUser`, `Navigator V3` | Shared utility functions |
| `gPMComponentServices` | `bSIROverdueTaskCheck`, `bSIRUserCompetenceTask`, `PMWrkManager` | Database connection pooling |
| `SharedFiles` | Most components | Shared file/path utilities |
| `SSP.S4I.Interfaces.IBusiness` | All `IBusiness` implementors | Standard business component interface |
| `Standard.Licensing` | `bPMLicenceManager` | Licence file parsing & validation |
| `System.IO.Compression` | `PMZipper`, `PMSiriusLogViewer`, `PMInstallUnzipper` | ZIP file handling |
| `System.Net.Http` | `BPMUsersSync`, `SSP.Pure.UsersSync` | HTTP client for REST APIs |
| `Newtonsoft.Json` | `BPMUsersSync`, `SSP.Pure.UsersSync` | JSON serialisation |
| `Microsoft.Win32.Registry` | `Sirius.Achitecture.Configuration.Local`, `PMServerRegistry`, `PMRegELSupport`, `Sirius Cache Controller` | Windows registry access |
| `System.Runtime.Remoting` | `iLogonManager`, `Ilogonserver` | .NET Remoting infrastructure |
| `MAPI32.dll` | `PMMAPI` | Email via MAPI |
| KeyCloak REST API | `BPMUsersSync`, `SSP.Pure.UsersSync` | User identity management |
| `bPMFunc.GetSystemOption` | `BPMUsersSync` | System option retrieval (KeyCloak config) |

---

## Cross-Reference: System Options Used

| Option # | Component | Purpose |
|----------|-----------|---------|
| 5246 | `BPMUsersSync` | KeyCloak Realm |
| 5247 | `BPMUsersSync` | KeyCloak Client ID |
| 5248 | `BPMUsersSync` | KeyCloak Client Secret |
| 5249 | `BPMUsersSync` | KeyCloak Admin Username |
| 5250 | `BPMUsersSync` | KeyCloak Admin Password (encrypted) |
| 5251 | `BPMUsersSync` | KeyCloak Admin Group Name |
| 5252 | `BPMUsersSync` | KeyCloak Token Endpoint URL |
