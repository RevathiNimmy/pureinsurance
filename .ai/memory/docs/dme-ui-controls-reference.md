# DME (Document Management Engine) UI Interface & User Controls Reference

> This document covers all Interface_Renamed components and user controls found in
> `DME\Components\`.
> For business component (bDOC\*) details including stored procedures,
> see `.github/docs/dme-components-reference.md`.
>
> **DME Overview:** DME is the *Documaster Enterprise* document management system.
> It stores, retrieves, scans, and manages documents attached to insurance entities
> (parties, policies, claims, risks). Documents are stored in a cabinet-based folder
> hierarchy, optionally zipped, and accessed via a Windows Forms desktop application
> (`iDOCManager`) or the document viewer (`iDocViewer`).

---

## 1. Architecture Overview

### Interface Pattern

Most DME interface components follow the same pattern as the wider Sirius platform:

```
Interface_Renamed (public entry point class)
  └─ frmInterface (Windows Form — private implementation)
        └─ bDOCXxx.Form (business component via ObjectManager.GetInstance)
              └─ SQL Server Stored Procedures / Documaster 2 file system API
```

**Key differences from GIS/Orion interfaces:**
- Many DME interfaces use a **stand-alone mode** (`bStandAlone` parameter) that allows them
  to run outside of the Sirius host without a full ObjectManager logon.
- `iDOCManager` is the hub application; most other interfaces are launched from within it.
- `iDocViewer` operates without its own business component — it directly opens sub-forms
  for different document types (TIF, RTF, Word, Excel, PDF, etc.).
- `iDOCScanStub` and `iDOCLink` are standalone launcher stubs, not Navigator-pattern interfaces.

### Standard Properties (where present)

| Property | Direction | Description |
|---|---|---|
| `CallingAppName` | WO | Name of the host application for logging |
| `Status` | RO | Return status code after `Start` / `Scan` / `Show` completes |

---

## 2. Component Index

| Interface | Component Folder | Business Component | Description |
|---|---|---|---|
| `iDOCInformation` | DOCInformation | `bDOCInformation.Form` | Display/edit document metadata for a stored document |
| `iDOCKeywordAdmin` | DOCKeywordAdmin | `bDOCKeywordAdmin.Form` | Administer and assign keyword classification tags to documents |
| `iDOCManager` | DOCManager | `bDOCManager.Form`, `bDOCOptions.Business` | Main DME document management application — browse, edit, scan, email, and manage documents |
| `iDOCOptions` | DOCOptions | `bDOCOptions.Form`, `bDOCOptions.Business` | Configure DME system options and preferences |
| `iDOCPassword` | DOCPassword | `bDOCPassword.Form` | Manage password protection on document nodes (Set Password + Verify Password) |
| `iDOCPMBAPI` | DOCPMBAPI | `bDOCPMBAPI.Form` | Documaster 2 bridge — synchronises legacy DM2 document storage with DME Enterprise |
| `iDOCScan` | DOCScan | `bDOCScan.Form` | Document scanning interface — capture pages from scanner or TWAIN device |
| `iDOCScanStub` | DOCScanStub | (launches `iDOCScan`) | Standalone scan station launcher — starts `iDOCScan` in standalone mode |
| `iDOCSetAccessLevel` | DOCSetAccessLevel | `bDOCSetAccessLevel.Form` | Set per-user access levels on document nodes |
| `iDOCSplash` | DOCSplash | (none) | Splash/loading screen displayed during DME application startup |
| `iDOCTransfer` | DOCTransfer | (self-contained) | Migration utility — transfers documents from Documaster 2 to Documaster Enterprise |
| `iDOCUserAdmin` | DOCUserAdmin | `bDOCUserAdmin.Form` | Administer DME user accounts and authorities |
| `iDOCViewBatch` | DOCViewBatch | `bDOCViewBatch.Form` | Batch view all documents in a scanned document queue |
| `iDocViewer` | DOCViewer | `bPMZipper.Business` (zip only) | Document viewer — displays documents by type (TIF, RTF, Word, Excel, PDF, Email, etc.) |
| `iDOCLink` | DOCLink | (launches `iDOCManager`) | Standalone DME launcher — starts or activates the `iDOCManager` application |

### User Controls

| Control | Component Folder | Description |
|---|---|---|
| `PMResizerControl` (`uctPMResizer`) | DOCResizerControl | Form-resizing helper that proportionally repositions and/or resizes controls when a Windows Form is resized |

---

## 3. Interface Components

### 3.1 `iDOCInformation`

**Files:**
- Interface class: `DOCInformation\Interface\iDOCInformationCls.vb`
- Interface form: `DOCInformation\Interface\iDOCInformationFrm.vb`

**Purpose:** Displays and edits the metadata (name, description, keywords, access level) stored
against a specific document in the DME cabinet. Called from within `iDOCManager` when the user
views or edits document properties.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `Status` | RO | Integer | Return status |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the object manager; does NOT require a logon session — shares the manager from the calling `iDOCManager` context |
| `Dispose()` | — | Releases resources |
| `Start` | `lDocNum`, `sNewName` | Opens the document information dialog for the specified document number (`lDocNum`). On close, returns the (possibly changed) document name in `sNewName`. |

**Business Component:** `bDOCInformation.Form` — manages all metadata reads and writes for a document record.

---

### 3.2 `iDOCKeywordAdmin`

**Files:**
- Interface class: `DOCKeywordAdmin\Interface\iDOCKeywordAdminCls.vb`
- Interface form: `DOCKeywordAdmin\Interface\iDOCKeywordAdminForm.vb`

**Purpose:** Two-function interface for managing document classification keywords:
1. `AdministerKeywords` — opens the keyword administration screen to define the keyword taxonomy.
2. `AttachKeywords` — opens the keyword assignment dialog for a specific document.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `UserIsAdministrator` | RW | Boolean | Whether the current user has keyword administration rights |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the object manager |
| `Dispose()` | — | Releases resources |
| `AdministerKeywords()` | — | Opens the full keyword taxonomy administration screen, allowing keywords to be added, renamed, or deleted. Returns `PMTrue` / `PMFalse`. |
| `AttachKeywords` | `vKeywordID`, `lDocNum?` | Opens the keyword assignment dialog for the given document number. Pre-selects the keyword identified by `vKeywordID` if provided. Returns `PMTrue` / `PMFalse`. |

**Business Component:** `bDOCKeywordAdmin.Form` — manages all keyword taxonomy CRUD and document-keyword associations.

---

### 3.3 `iDOCManager`

**Files:**
- Interface class: `DOCManager\Interface\iDOCManagerCls.vb` (2 028 lines)
- Interface form: `DOCManager\Interface\iDOCManagerFrm.vb`
- Helper forms: `iDOCEmail.vb`, `iDOCSelectFolders.vb`, `frmEditAdmin.vb`, `frmExportFolder.vb`, `frmMessage.vb`, `frmProgress.vb`

**Purpose:** The main DME desktop application. Provides the complete document management UI —
the folder tree navigator, document list view, preview pane, toolbar, and integrated menus for
all document operations including scanning, import, export, email, printing, keyword
assignment, access control, and briefcase (offline) operations.

**Public Fields:**

| Field | Type | Description |
|---|---|---|
| `m_lDocNum` | Integer | Currently selected document number |
| `sCommand` | String | Last command string passed to `Activate` |
| `sExCodes(4)` | String() | Most recent external folder codes for navigation |
| `m_sCurrentUser` | String | Username of the current DME session user |

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `PMAuthorityLevel` | WO | Integer | Authority level bitmask from the Sirius session |
| `Status` | RO | Integer | Return status |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the object manager; performs full user logon to DME, loads cabinet and folder structure from the database |
| `Dispose()` | — | Releases the document manager session and all cached data |
| `Start` | `vCmd?` | Opens the main DME application window. Optionally accepts a command string (e.g. an external code navigation target). Blocks until the application is closed. |
| `Activate` | `sCommand` | If the DME window is already open, brings it to the foreground and navigates to the location specified by the command string. Used when a second instance is launched (avoids duplicate windows). |
| `GetExternalCodes` | `sCommand`, `sArgArr()` | Parses an external code navigation command string and populates `sArgArr` with the folder path components (cabinet, drawer, folder, document codes). |
| `BriefCaseProcess` | `sCommand` | Initiates a briefcase download/upload transaction — synchronises the local DME briefcase copy with the server based on the command string (e.g. `"BC <filename>"`). |
| `DownLoadProcess` | `sCabExCode`, `sDrawExCode`, `sFoldExCode` | Downloads documents from a specific cabinet/drawer/folder path into the local briefcase for offline working. |
| `SetBriefCaseDir()` | — | Prompts the user to choose the local briefcase directory and saves it to the registry. |
| `CopyPages` | `vPageArray(,)`, `sServerPath` | Copies a set of document page files to a specified server path (used in document archiving workflows). |
| `DetachDB` | `bKillFiles` | Detaches (disconnects) the DME database connection. If `bKillFiles` is True, also deletes local cached files. |
| `AttachDB()` | — | Reattaches (reconnects) the DME database after a detach. |
| `ProcessEnd` | `lProcessID` | Signals that a background process (e.g. batch import) with the given ID has completed, allowing the UI to refresh. |
| `SBODisplayClient` | `sCommand`, `bCalledViaActivate?` | Displays the Sirius Back Office client view associated with the external code in the command string. Optionally called via the Activate path. |
| `SBOViewDocument()` | — | Opens the currently selected document in the Sirius Back Office viewer. |
| `SetKeys` | `vKeyArray(,)` | Sets input key values for Navigator-pattern integration |
| `GetKeys` | `vKeyArray(,)` | Retrieves output key values after close |
| `GetSummary` | `vSummaryArray` | Returns a summary display string |
| `SetProcessModes` | `vTask?`, `vNavigate?`, `vProcessMode?`, `vTransactionType?`, `vEffectiveDate?` | Sets task/navigation/process mode flags |
| `PrintPDFs` | `sFilename` | Silently prints a PDF document by filename using the DME print pipeline |

**Helper Forms within iDOCManager:**

#### `iDOCEmail`

| Property | Description |
|---|---|
| `Addresses` | RW — collection of email addresses to send to |
| `Status` | RO — return status |
| `SendTo` | RW — primary `To:` email address |
| `SendSubject` | RW — email subject line |
| `SendNote` | RW — email body text |
| `SendFile` | RW — path to the document file to attach |

Displayed as a pop-up dialog when the user chooses to email a document from the DME toolbar.

#### `iDOCSelectFolders`

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `frm` | Initialises the folder selection control against the parent form |
| `Dispose()` | — | Releases resources |
| `SelectFolderList` | `sCaption`, `lMaxFoldersReturned`, `r_vResultArray(,)` | Displays a folder tree picker dialog and returns the selected folder paths in `r_vResultArray` |
| `ReturnSelectedFolders()` | — | Returns the currently selected folders from a previous `SelectFolderList` call |
| `SetFolderValues` | `lFolderNum`, `lChildren` | Loads folder metadata into the selection control for a specific folder |
| `SelectFolders` | `lFolderNum`, `lChildren` | Marks specific folders as selected in the tree control |

**Business Components:**

| Component | Purpose |
|---|---|
| `bDOCManager.Form` | All cabinet/folder/document CRUD, search, briefcase, and document routing |
| `bDOCOptions.Business` | System option retrieval during manager session initialisation |

---

### 3.4 `iDOCOptions`

**Files:**
- Interface class: `DOCOptions\Interface\iDOCOptionsCls.vb`
- Interface form: `DOCOptions\Interface\iDOCOptionsFrm.vb`

**Purpose:** Configure DME system-wide options, including scanner settings, timer intervals,
history root paths, cache settings, and integration preferences. Called from the DME
Tools → Options menu.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `bUserIsAdministrator` | Initialises the options interface. Sets the administrator flag that controls which option categories are visible/editable by the current user. |
| `Dispose()` | — | Releases resources |
| `Start` | `iStatus` | Opens the Options configuration dialog. Returns the resulting status in `iStatus` — `PMTrue` if saved, `PMCancel` if cancelled. |

**Business Components:**

| Component | Purpose |
|---|---|
| `bDOCOptions.Form` | Reads and writes all DME option values to the registry and database |
| `bDOCOptions.Business` | Provides validation and helper logic for option values |

---

### 3.5 `iDOCPassword`

**Files:**
- Interface class: `DOCPassword\Interface\iDOCPasswordCls.vb`
- Set password form: `DOCPassword\Interface\iDOCSetPassword.vb`
- Verify password form: `DOCPassword\Interface\iDOCVerifyPassword.vb`

**Purpose:** Manages password protection on DME document nodes. Provides two separate
operations: `AddPassword` (set or change a password on a node) and `VerifyPassword`
(prompt the user to enter the password before they can access a protected node).

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `Status` | RO | Integer | Return status |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `bStandAlone`, `sUsername`, `sPassword`, `iUserID`, `iSourceID`, `iLanguageID`, `iCurrencyID`, `iLogLevel`, `sCallingAppName`, `vDatabase?` | Initialises the password interface with the current session context. When `bStandAlone` is True, the interface manages its own logon; otherwise it uses the existing session credentials. |
| `Dispose()` | — | Releases resources |
| `AddPassword` | `lNodeNum?`, `iNodeLevel?`, `sEncryptedPassword?` | Opens the set/change password dialog for the specified node. `lNodeNum` is the DME node (folder or document) ID; `iNodeLevel` identifies whether it is a cabinet, drawer, folder, or document. Returns an encrypted password in `sEncryptedPassword`. |
| `VerifyPassword` | `lNodeNum`, `iNodeLevel`, `sNodeName` | Opens the password entry verification dialog for the named node. The user must enter the correct password to continue. Returns `PMTrue` if verified. |

**Business Component:** `bDOCPassword.Form` — validates entered passwords against the stored hash.

---

### 3.6 `iDOCPMBAPI`

**Files:**
- Module: `DOCPMBAPI\Interface\iDOCPMBAPI.vb` (main module; declares `g_oBusiness`, `g_oObjectManager`, `g_oPMBLog`)
- Business form: `DOCPMBAPI\Business\bDOCPMBAPIForm.vb`
- DMS API modules: `DMSJAPI.vb` (journal API constants/structs), `DMSGLOB.vb` (global vars), `DMSDDB.vb` (DM2 database), `DMSMEDIA.vb` (media ops), `DMSHIST.vb` (history), `DMSLOG.vb` (logging)
- Utility modules: `DOCGeneralFunc.vb` (registry, file, path helpers), `ENCRYPT.vb`, `PMFUNC.vb`, `PMERRORS.vb`
- UI forms: `ADDMEDIA.vb`, `DIRBOX.vb`, `EXISTLOG.vb`, `GETPASS2.vb`, `INISET.vb`, `PASSWORD.vb`, `PERGLOGS.vb`, `VIEWLOGS.vb`, `dmsjapit.vb`

**Purpose:** Bridge between legacy *Documaster 2* (DM2) and *Documaster Enterprise*. Runs as a
background process that reads the DM2 journal API and synchronises archived documents into the
DME database. Supports both accelerated import mode and retry of failed imports/exports.

**`bDOCPMBAPIForm` Public Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `sUserName`, `sPassword`, `iUserID`, `iSourceID`, `iLanguageID`, `iCurrencyID`, `iLogLevel`, `sCallingAppName`, `bStandAlone?`, `vDatabase?` | Creates the DM2 API connection, validates credentials, and prepares the synchronisation engine |
| `Dispose()` | — | Closes the DM2 API connection and releases resources |
| `Start` | `oPMBLog`, `bAccelerated`, `bRetryImports`, `bRetryExports` | Runs the DM2 → DME synchronisation loop. If `bAccelerated` is True, processes all pending records in a single pass. `bRetryImports`/`bRetryExports` control re-processing of previously failed records. `oPMBLog` receives the progress log. |
| `RebuildRemoteDB()` | — | Completely rebuilds the DME remote database from the DM2 journal — used for disaster recovery or initial migration. |

**`DOCGeneralFunc` Public Utility Functions:**

| Function | Parameters | Description |
|---|---|---|
| `GetDOCRegSettings` | optional `r_v*` params | Reads DME configuration from the Windows registry — returns timer interval, history root path, scan directory, overwrite flag, parent window handle, and scanner type (Kofax/TWAIN) |
| `ChangeDOCRegSettings` | optional `r_v*` params | Writes updated DME configuration values back to the Windows registry |
| `GetDMEDIR` | `r_sDMEDir` | Returns the DME installation directory path from the registry |
| `GetRegistryValue` | `sKey`, `sSubKey`, `sValue`, `iLocation` | Reads a single value from the Windows registry at the specified key path and location (user or system) |
| `SetRegistryValue` | `sKey`, `sSubKey`, `sValue`, `iLocation` | Writes a single value to the Windows registry |
| `IsSiriusInstalled` | `r_bSiriusInstalled` | Checks whether the Sirius platform is installed by reading the registry; returns True/False in `r_bSiriusInstalled` |
| `CopyFile` | `sFileIn`, `sFileOut` | Copies a file from `sFileIn` to `sFileOut` |
| `KillFile` | `sFile` | Deletes a single file |
| `KillDir` | `DirName` | Deletes a directory and all its contents |
| `RmDirectory` | `sDirectory` | Removes an empty directory |
| `MakePath` | `sPath` | Creates all missing directory levels in the specified path |
| `BrowseFolder` | `sFolder`, `sTitle`, `hWndParent` | Shows a folder browser dialog; returns the selected path |
| `SetCacheStatus` | `bCacheDisabled` | Enables or disables the DME document cache in the registry |
| `ClearFolder` | `sFolder` | Removes all files from the specified folder |
| `CacheFile` | `oZipper`, `sFilename`, `sNewFilename`, `sCachePath`, `bZipped?` | Caches a document file at the local cache path, unzipping it first if required |
| `ZipCheck` | `sFilename`, `bZipped` | Tests whether a file is a ZIP archive; returns True/False in `bZipped` |
| `ExtractNumFromKey` | `sKey`, `lNum` | Parses a numeric ID from a compound DME key string |
| `GetFileName` | `sFullPath`, `sFile`, `sDir` | Splits a full file path into its directory and filename components |
| `StripSlashes` | `sStringIn`, `sStringOut` | Removes trailing backslashes from a path string |
| `LogMessageScan` | `iType`, `sMsg`, optional params | Writes a scan-related log message to the DME event log |
| `SetComboText` | `Cbo`, `sText`, `iList` | Selects an item in a combo box by text value |

**`DMSJAPI` Global Structures:**

| Structure | Purpose |
|---|---|
| `g_utControlData` | Journal control record — holds DME process state flags |
| `g_utIndexListData` | Index list record — holds document index metadata |
| `g_utIndexCabinet` | Cabinet record — holds cabinet-level metadata |

**Business Component:** `bDOCPMBAPI.Form`

---

### 3.7 `iDOCScan`

**Files:**
- Interface class: `DOCScan\Interface\iDOCScanCls.vb`
- Interface form: `DOCScan\Interface\frmInterface.vb`
- Annotation form: `DOCScan\Interface\frmAnnotation.vb`
- Bad scan form: `DOCScan\Interface\frmBadScan.vb`

**Purpose:** Document scanning interface. Connects to a TWAIN or Kofax scanner, captures pages,
displays thumbnails for review, allows annotation/deletion of individual pages, and stores the
scanned document into the DME cabinet.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `HiddenForFolderSelect` | RO | Boolean | Whether the scan window is currently hidden because a folder-selection dialog is open |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `bStandAlone` | Initialises the scan interface. When `bStandAlone` is True, performs an independent ObjectManager logon (for ScanStation; otherwise `bStandAlone` is False when launched from `iDOCManager`). |
| `Dispose()` | — | Releases the scanner connection and ObjectManager |
| `Scan` | `vFolderTree(,)` | Opens the scan workspace. `vFolderTree` is a 2D array of folder nodes pre-loaded from the cabinet to allow the user to select a destination folder without a separate database call. Returns `PMTrue` when the user successfully completes a scan batch. |

**Business Component:** `bDOCScan.Form` — manages all scanner communication, page acquisition
(TWAIN/Kofax), TIF file assembly, and document storage to the DME database.

---

### 3.8 `iDOCScanStub`

**Files:**
- Module: `DOCScanStub\Interface\iDOCScanStubMod.vb`
- Form stub: `DOCScanStub\Interface\iDOCScanStub.vb` (empty form; provides .exe icon)

**Purpose:** Standalone ScanStation launcher. An independent `.exe` that creates an instance
of `iDOCScan.interface_Renamed`, calls `Initialise(True)` (stand-alone mode), and then calls
`Scan()`. Prevents duplicate instances from running (checks the process list). The form exists
solely as a host for the application icon.

**This is not a reusable interface** — it is the ScanStation executable entry point.

---

### 3.9 `iDOCSetAccessLevel`

**Files:**
- Interface class: `DOCSetAccessLevel\Interface\iDOCSetAccessLevelCls.vb`
- Interface form: `DOCSetAccessLevel\Interface\iDOCSetAccessLevelFrm.vb`

**Purpose:** Set the access level permissions on a DME document node for a specific user. Used
from within `iDOCManager` to control who can view, edit, or delete a particular document.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `Status` | RO | Integer | Return status |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the object manager |
| `Dispose()` | — | Releases resources |
| `Start()` | — | Opens the access level administration form showing all users and their current access levels for the DME system |
| `SetAccessLevel` | `iNodeType`, `lNodeNum`, `sNodeName`, `iUserAccessLevel` | Sets the access level for a specified node and user. `iNodeType` identifies the node level (cabinet/drawer/folder/document); `lNodeNum` is the node ID; `sNodeName` is the display name; `iUserAccessLevel` is the access level code to assign. Returns `PMTrue` / `PMFalse`. |

**Business Component:** `bDOCSetAccessLevel.Form` — performs access level reads and writes for document nodes.

---

### 3.10 `iDOCSplash`

**Files:**
- Interface class: `DOCSplash\Interface\iDOCSplash.vb`
- Interface form: `DOCSplash\Interface\iDOCSplashFrm.vb`

**Purpose:** Displays a splash/loading screen during DME application startup while the
document manager initialises and loads cabinet data. Supports multiple splash types
(e.g. standard startup, standalone mode, update).

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the splash form (does not require an ObjectManager) |
| `Dispose()` | — | Releases resources |
| `Show` | `iSplashType`, `sMessage?` | Shows the splash screen. `iSplashType` determines the image/layout variant. An optional `sMessage` string is displayed as a status line on the splash. Returns `PMTrue` when displayed. |
| `Hide()` | — | Hides and closes the splash screen (called once the main application has finished loading). |

**Business Component:** None — the splash screen is purely presentational.

---

### 3.11 `iDOCTransfer`

**Files:**
- Form (in Business folder): `DOCTransfer\Business\iDOCTransfer.vb` (form class `frmInterface`)

**Purpose:** One-off data migration utility for transferring document records from the legacy
*Documaster 2* database into the *Documaster Enterprise* database. Designed to run server-side.
Not a standard Navigator-pattern interface — it is a standalone migration tool.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Status` | RW | Integer | Migration status: `ACStarted`, `ACAbort`, `ACCancel`, `ACViewReport` |

**Usage:** The form is opened directly; the user clicks Start to begin the transfer, Abort
to halt mid-transfer, or View Report to review the transfer log.

---

### 3.12 `iDOCUserAdmin`

**Files:**
- Interface class: `DOCUserAdmin\Interface\iDOCUserAdminCls.vb`
- Interface form: `DOCUserAdmin\Interface\iDOCUserAdminFrm.vb`
- Helper form: `DOCUserAdmin\Interface\frmChange.vb`

**Purpose:** DME user account administration — add, edit, and delete DME users, set their
authority levels, and manage their group memberships. Called from the DME Tools → User
Administration menu.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `Status` | RO | Integer | Return status |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the object manager |
| `Dispose()` | — | Releases resources |
| `Start()` | — | Opens the user administration screen listing all DME users and their current settings |
| `UserAdmin()` | — | Opens the user administration dialog directly (alternative entry point — same as `Start` but intended for programmatic invocation from `iDOCManager`'s admin menu). Returns `PMTrue` / `PMFalse`. |

**Business Component:** `bDOCUserAdmin.Form` — manages all user account reads and writes for the DME user store.

---

### 3.13 `iDOCViewBatch`

**Files:**
- Interface class: `DOCViewBatch\Interface\iDOCViewBatchInterface.vb`
- Interface form: `DOCViewBatch\Interface\iDOCViewBatchForm.vb`

**Purpose:** Batch document viewing interface. Displays a queue of scanned documents (e.g. a
batch imported from a scanner or file drop) and allows the operator to review each one before
confirming it into the DME cabinet. Used in high-volume scanning workflows.

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `bStandAlone` | Initialises the batch viewer. When `bStandAlone` is True, manages its own logon. |
| `Dispose()` | — | Releases resources |
| `ViewBatch()` | — | Opens the batch viewer showing all documents in the pending queue. The operator can accept, annotate, or discard each document. Returns `PMTrue` when the batch is processed. |

**Business Component:** `bDOCViewBatch.Form` — reads the scan batch queue and updates document
records after operator review.

---

### 3.14 `iDocViewer`

**Files:**
- Interface class: `DOCViewer\Interface\iDocViewer.vb` (main class + module)
- Module: `DOCViewer\Interface\iDocViewerMod.vb` (constants, globals, and the `gSIRLibrary.vb` helper)
- Viewer forms by type:
  - `iDOCViewerMDI.vb` — MDI host frame (houses the individual document viewer child windows)
  - `iDOCViewerOFF.vb` — Microsoft Office document viewer (Word, Excel, PowerPoint, Access)
  - `iDOCViewerOLE.vb` — OLE-embedded document viewer (fallback for unsupported types)
  - `iDOCViewerRTF.vb` — Rich Text Format (RTF) viewer
  - `iDOCViewerTIF.vb` — TIFF image viewer with pan/zoom and page navigation

**Purpose:** Displays DME documents in a read-only (or optionally editable) viewer. Detects
the document type from its file extension and opens the appropriate viewer sub-form. Supports
TIF, RTF, Word, Excel, PowerPoint, PDF (via ShellExecute), HTML, JPEG, BMP, PNG, email (.eml),
and ZIP archives.

**Module Constants (`iDocViewerMod`):**

| Constant Group | Values |
|---|---|
| `ACFileType*` | 0=Unknown, 1=TIF, 2=TXT, 3=RTF, 4=Word, 5=Excel, 6=PowerPoint, 7=Access, 8=HTML, 9=GIF, 10=JPEG, 11=EML, 12=PDF, 13=HLP, 14=ZIP, 15=BMP, 16=PNG |
| `ACViewerType*` | Same set — identifies which viewer sub-form handles each type |
| `FIT_*` | -1=None, 0=Screen, 1=Width, 2=Height — TIF zoom fit modes |
| `MOUSE_*` | 0=None, 1=Move, 2=Zoom — TIF mouse interaction mode |
| `AC*Mode` | 0=Normal, 1=Merge, 2=Print, 3=PrintSilent, 4=SpoolDoc, 5=SpoolReport |

**Properties:**

| Property | Direction | Description |
|---|---|---|
| `Username` | RO | Returns the display name of the current logged-on DME user |
| `DefaultInstance` | RO (shared) | Returns the singleton `Interface_Renamed` instance |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise` | `frmManager`, `v_vUserID?`, `v_vUserName?`, `v_vUserPassword?`, `v_vSourceID?` | Initialises the viewer against the parent DME manager form. Accepts optional user credentials for use when launched standalone (not from within `iDOCManager`). |
| `Dispose()` | — | Releases all viewer sub-forms and resources |
| `ViewDocument` | `v_sDocumentKey`, `v_sDocumentName`, `v_sParents`, `v_vFileArray()`, `v_bZipped`, `v_bShowOnly?`, `v_vAdditionalDataArray(,)?`, `v_bAllowCopyPaste?` | Opens the viewer for the specified document. `v_DocumentKey` is the DME document key; `v_sDocumentName` is the display name; `v_sParents` is the full folder path; `v_vFileArray` contains the file paths to display; `v_bZipped` indicates whether the files are stored as ZIP archives; `v_bShowOnly` prevents editing; `v_bAllowCopyPaste` enables clipboard operations. |
| `ViewTIFDocument` | `v_sDocumentKey`, `v_sDocumentName`, `v_sParents`, `v_vFileArray` | Opens specifically the TIF viewer for the specified document (overload of `ViewDocument` for TIF-only workflows). |
| `RefreshFormControl` | `v_sExceptionDocumentKey` | Refreshes the document list in the MDI host, excluding the document identified by `v_sExceptionDocumentKey` (e.g. after a document has been deleted). |
| `SetWinPos` | `lHWnd` | Positions a window to be always-on-top relative to the DME manager window using `SetWindowPos` API. |

**Business Component:** `bPMZipper.Business` — used internally to extract ZIP-compressed
document files into the local cache before displaying. No DME business component is called;
the viewer works purely with cached file paths.

---

### 3.15 `iDOCLink`

**Files:**
- Module: `DOCLink\Interface\iDOCLinkMod.vb`
- Form stub: `DOCLink\Interface\iDOCLink.vb` (empty form; provides the `.exe` icon/window)
- Constants: `DOCLink\Interface\DOCConst.vb`
- Helpers: `DOCLink\Interface\iPMFunc.vb`

**Purpose:** Standalone launcher for the DME application. An independent `.exe` that starts
or activates `iDOCManager`. If `iDOCManager` is already running (detected via `FindWindow`
for the "DocuMaster Enterprise" window title), it calls `Activate()` on the running instance
to bring it to the foreground. If not running, it creates a new `iDOCManager.Interface_Renamed`,
calls `Initialise()`, then `Start(vCmd)` where `vCmd` is optionally passed from the command line
(e.g. for briefcase download: `"BC <filename>"`).

**This is not a reusable interface** — it is the DME application executable entry point.

---

## 4. User Controls

### 4.1 `PMResizerControl` (`uctPMResizer`)

**Files:**
- Control: `DOCResizerControl\User Control\PMResizerControl.vb`
- Designer: `PMResizerControl.Designer.vb`

**Purpose:** A Windows Forms `UserControl` that provides automatic proportional form resizing.
When a form containing this control is resized by the user, it repositions and/or resizes all
registered child controls according to their configured resize options. Used by several DME
forms to allow the user to resize the DME windows.

**Enums:**

| Enum | Values | Description |
|---|---|---|
| `PMEControlResizeOptions` | (bitmask values) | Controls which resize behaviours apply to each registered control (move horizontally, move vertically, resize width, resize height, or combinations) |
| `PMEControlResizeTypes` | (values) | Specifies whether a control is resized proportionally or anchored |

**Public Fields:**

| Field | Type | Description |
|---|---|---|
| `ResizeFont` | Boolean | Whether to proportionally scale control fonts during resize |
| `KeepRatio` | Boolean | Whether to maintain the form's original aspect ratio during resize |
| `FormMinWidth` | Integer | Minimum form width in pixels |
| `FormMinHeight` | Integer | Minimum form height in pixels |
| `NoResizeByDefault` | Boolean | If True, controls must be explicitly registered; otherwise all controls are resized by default |

**Inner Structure:**

| Structure | Description |
|---|---|
| `TControlInfo` | Holds the original position and size of a registered control; has `CreateInstance()` factory method |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `SaveControls()` | — | Captures the current positions and sizes of all controls on the parent form. Call this once in the parent `Form_Load` event after all controls are positioned. |
| `RefreshControls()` | — | Repositions and resizes all registered controls to match the current form size. Call this in the parent `Form_Resize` event. |
| `SetControlResizeOption` | `v_sControlName`, `v_lResizeOption`, `v_lResizeType`, `v_lControlArrayIndex?` | Registers a specific control by name with its resize behaviour options. `v_lControlArrayIndex` supports VB6-style control arrays. |

---

## 5. File Inventory

| Interface | Component Folder | Key Files |
|---|---|---|
| `iDOCInformation` | `DOCInformation\Interface\` | `iDOCInformationCls.vb`, `iDOCInformationFrm.vb` |
| `iDOCKeywordAdmin` | `DOCKeywordAdmin\Interface\` | `iDOCKeywordAdminCls.vb`, `iDOCKeywordAdminForm.vb` |
| `iDOCManager` | `DOCManager\Interface\` | `iDOCManagerCls.vb`, `iDOCManagerFrm.vb`, `iDOCEmail.vb`, `iDOCSelectFolders.vb`, `frmEditAdmin.vb`, `frmExportFolder.vb`, `frmMessage.vb`, `frmProgress.vb` |
| `iDOCOptions` | `DOCOptions\Interface\` | `iDOCOptionsCls.vb`, `iDOCOptionsFrm.vb` |
| `iDOCPassword` | `DOCPassword\Interface\` | `iDOCPasswordCls.vb`, `iDOCSetPassword.vb`, `iDOCVerifyPassword.vb` |
| `iDOCPMBAPI` | `DOCPMBAPI\Interface\` | `iDOCPMBAPI.vb`, `DOCGeneralFunc.vb`, `DMSJAPI.vb`, `DMSGLOB.vb`, `DMSDDB.vb`, `DMSMEDIA.vb`, `DMSHIST.vb`, `DMSLOG.vb`, `ENCRYPT.vb`, `PMAPI.vb`, `PMFUNC.vb`, `PMERRORS.vb` |
| `iDOCScan` | `DOCScan\Interface\` | `iDOCScanCls.vb`, `frmInterface.vb`, `frmAnnotation.vb`, `frmBadScan.vb` |
| `iDOCScanStub` | `DOCScanStub\Interface\` | `iDOCScanStubMod.vb`, `iDOCScanStub.vb` |
| `iDOCSetAccessLevel` | `DOCSetAccessLevel\Interface\` | `iDOCSetAccessLevelCls.vb`, `iDOCSetAccessLevelFrm.vb` |
| `iDOCSplash` | `DOCSplash\Interface\` | `iDOCSplash.vb`, `iDOCSplashFrm.vb`, `iDOCSplashMod.vb` |
| `iDOCTransfer` | `DOCTransfer\Business\` | `iDOCTransfer.vb` (form class) |
| `iDOCUserAdmin` | `DOCUserAdmin\Interface\` | `iDOCUserAdminCls.vb`, `iDOCUserAdminFrm.vb`, `frmChange.vb` |
| `iDOCViewBatch` | `DOCViewBatch\Interface\` | `iDOCViewBatchInterface.vb`, `iDOCViewBatchForm.vb` |
| `iDocViewer` | `DOCViewer\Interface\` | `iDocViewer.vb`, `iDocViewerMod.vb`, `gSIRLibrary.vb`, `iDOCViewerMDI.vb`, `iDOCViewerOFF.vb`, `iDOCViewerOLE.vb`, `iDOCViewerRTF.vb`, `iDOCViewerTIF.vb` |
| `iDOCLink` | `DOCLink\Interface\` | `iDOCLinkMod.vb`, `iDOCLink.vb`, `DOCConst.vb`, `iPMFunc.vb` |
| `PMResizerControl` | `DOCResizerControl\User Control\` | `PMResizerControl.vb`, `PMResizerControl.Designer.vb` |
