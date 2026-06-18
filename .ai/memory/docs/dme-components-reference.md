# DME Business Components Reference

> Document Management Engine (DME) — Sirius Documaster (Documaster 2 / bDOC* components)
>
> All components reside under `DME\Components\`. Each exposes a `Form` class (the public API), a `MainModule` (global state), and some include a `Miscellaneous` helper class and/or a `Cls` data-entity class. They all connect to the **Documaster SQL database** (`dPMDAO.Database`) and return `gPMConstants.PMEReturnCode` values (`PMTrue` / `PMFalse` / `PMError`).

---

## Component Architecture Pattern

Every `bDOC*` Form class follows the same structure:

| Element | Description |
|---|---|
| `MainModule` / `bDOC*.vb` | Declares module-level globals (`g_sUsername`, `g_iUserID`, `ACApp`, etc.) |
| `Form` class | Primary public API class. Implements `SSP.S4I.Interfaces.IBusiness` (on older components) |
| `Miscellaneous` class | Internal helper class with lower-level DME operations (shared across several components) |
| `Cls` class (where present) | Entity/DTO class holding database attributes with property accessors |
| `FormSQL` module | Declares stored procedure name constants (`ACAddSQL`, `ACAddName`, `ACAddStored`) |

**Return values**: All methods return `Integer` / `gPMConstants.PMEReturnCode` — `PMTrue` (1) = success, `PMFalse` (0) = business failure, `PMError` (-1) = exception.

---

## 1. bDOCDocNameAdmin

**Directory**: `DME\Components\DOCDocNameAdmin\Business\`  
**Files**: `bDOCDocNameAdmin.vb`, `bDOCDocNameAdminForm.vb`, `bDOCMiscellaneous.vb`

**Purpose**: Administers the document name lookup table (`DOC_doc_name`). Provides CRUD operations for document name entries that users select when indexing scanned documents, plus access to the full DME node-tree utilities via the shared `Miscellaneous` class.

### 1.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DB if not supplied, creates `bDOCDocName.Form` and internal `Miscellaneous` instances. | — | `bDOCDocName.Form` |
| `GetDocNames` | `(ByRef vDocNames As Object) As Integer` | Wrapper — delegates to `Miscellaneous.GetDocNames`. Returns a 2D array of `{doc_name, doc_name_id}` ordered by name. | — | `bDOCDocNameAdmin.Miscellaneous` |
| `AddDocName` | `(ByRef sDocName As String, ByRef iDocNameID As Integer) As Integer` | Validates the name string (SQL-injection check), then delegates to `bDOCDocName.Form.DirectAdd` to insert the new row. | *(via bDOCDocName)* | `bDOCDocName.Form` |
| `DeleteDocName` | `(ByRef iDocNameID As Integer) As Integer` | Deletes a row from `DOC_doc_name` by `doc_name_id` using inline SQL. | — | — |
| `UpdateDocName` | `(ByRef sNewDocName As String, ByRef iDocNameID As Integer) As Integer` | Validates the new name string, then updates `DOC_doc_name.doc_name` using inline SQL. | — | — |

### 1.2 Miscellaneous Class Public Methods

The `Miscellaneous` class is the shared DME infrastructure helper. It contains low-level operations on all DME node types (folders, documents, pages). **The same class (with identical methods) appears in bDOCFind and bDOCInformation.**

| Method | Signature | Purpose | SPs Called |
|---|---|---|---|
| `Initialise` | `(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, ..., [vDatabase], [vScanDatabase], [vHistory]) As PMEReturnCode` | Stores references to DB, scan DB, and history object. | — |
| `GetDataPath` | `(ByRef lVolumeID As Integer, ByRef sDataPath As String) As Integer` | Constructs the UNC/local filesystem path for a document volume. Reads `DOC_volume` and `DOC_device`; honours briefcase registry setting. | — |
| `GetAdminLevel` | `([ByRef iAdminLevel As Integer]) As Integer` | Reads `admin_level` from `DOC_system`. | — |
| `GetNextPageName` | `(ByRef sNextPageName As String) As Integer` | Thread-safe: starts a DB transaction with `UPDLOCK`, reads `next_page` from `DOC_system`, increments the 5-pair counter, writes it back, returns old value. | — |
| `ConstructDocRef` | `(ByRef lDocNum As Integer, ByRef sDocRef As String) As Integer` | Formats `lDocNum` as a zero-padded 9-character string (`"000000001"`) for use in the history table. | — |
| `DeleteDocKeyword` | `(ByRef lDocKeywordID As Integer) As Integer` | Deletes one row from `DOC_doc_keyword` by `doc_keyword_id`. | — |
| `DeleteFolder` | `(ByRef lFolderNum As Integer, ByRef sNoAccessName As String) As Integer` | Public entry point for folder deletion. Recursively calls `DeleteFolderTree` (private). Returns the name of the first inaccessible folder in `sNoAccessName` if permission denied. | — |
| `GetNodeParent` | `(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef lParentNum As Integer) As Integer` | Returns the immediate parent's ID — for `DOCNode_Folder`: from `DOC_folder.parent_num`; for `DOCNode_Document`: from `DOC_document.folder_num`. | — |
| `GetFolderTree` | `(ByRef lFolderNum As Integer, ByRef vFolderArray(,) As Object) As Integer` | Walks the ancestor chain from the given folder to the root. Returns a 2-row 2D array of `{folder_num, folder_name}` ordered closest-to-root last. | — |
| `GetNodeExCode` | `(ByRef iNodeType, ByRef lNodeNum, ByRef sExCode, [ByRef iFolderLevel]) As Integer` | Returns `ex_code` (and optionally `folder_level`) for a folder or document. | — |
| `GetNodeName` | `(ByRef iNodeType, ByRef lNodeNum, ByRef sNodeName) As Integer` | Returns `folder_name` or `doc_name` for the given node. | — |
| `GetNodeAccessLevel` | `(ByRef iNodeType, ByRef lNodeNum, ByRef iAccessLevel) As Integer` | Returns `access_level` for the given folder or document. | — |
| `RenameNode` | `(ByRef iNodeType, ByRef lNodeNum, ByRef sNewNodeName) As Integer` | Issues an inline `UPDATE` on `DOC_folder` or `DOC_document` to rename the node. | — |
| `RenameDoc` | `(ByRef lDocNum As Integer, ByRef sNewName As String) As Integer` | Calls `RenameNode`, then optionally updates history if the document is externally coded. | — |
| `AmIExternal` | `(ByRef iNodeType, ByRef lNodeNum, ByRef bExternal As Boolean) As Integer` | Returns `True` if the node has a non-empty `ex_code`. | — |
| `GetDocNames` | `(ByRef vDocNamesArray(,) As Object) As Integer` | Returns all rows from `DOC_doc_name` ordered by `doc_name` as a 2-row array `{doc_name, doc_name_id}`. | — |
| `GetDocDateOffSets` | `(ByRef iDocDateOffset As Integer, ByRef iExpiryDateOffset As Integer) As Integer` | Reads `doc_date` and `expiry_date` offset values from `DOC_system`. | — |
| `LinkOwner` | `(ByRef lDocNum As Integer, ByRef bLinkOwner As Boolean) As Integer` | Sets `bLinkOwner = True` if the document is a link owner (checked from `DOC_document.link`). | — |
| `DeleteDocInfo` | `(ByRef lDocNum As Integer) As Integer` | Deletes all rows for a document from `DOC_doc_info`. | — |
| `DeleteDocKeywords` | `(ByRef lDocNum As Integer) As Integer` | Deletes all rows for a document from `DOC_doc_keyword`. | — |
| `DeleteDocAnnotations` | `(ByRef lDocNum As Integer) As Integer` | Deletes all rows for a document from `DOC_annotation`. | — |
| `GetPageList` | `(ByRef lDocNum As Integer, ByRef vPageArray() As Object) As Integer` | Returns list of page entries for a document from `DOC_page`. | — |
| `GetPageType` | `(ByRef lDocNum As Integer, ByRef sPageType As String) As Integer` | Returns the `page_type` for the first page of a document. | — |
| `GetPageName` | `(ByRef lDocNum As Integer, ByRef sPageName As String) As Integer` | Returns the `page_name` (physical filename) for the first page of a document. | — |
| `GetDocDate` | `(ByRef lDocNum As Integer, ByRef dDocDate As Date) As Integer` | Returns `doc_date` from `DOC_doc_info`. | — |
| `GetDocLink` | `(ByRef lDocNum As Integer, ByRef lLinkNum As Integer) As Integer` | Returns the `link` field value from `DOC_document`. | — |
| `GetDocDetails` | `(ByRef lDocNum As Integer, ByRef sDocName As String, ByRef sDocType As String) As Integer` | Returns `doc_name` and `doc_type` from `DOC_document`. | — |
| `DeleteDocuments` | `(ByRef vDocArray As Object, ByRef bExternal As Boolean, ByRef sNoAccessName As String) As Integer` | Iterates an array of doc nums and calls `DeleteDoc` on each. | — |
| `DeleteDoc` | `(ByRef lDocNum As Integer, ByRef bExternal As Boolean, ByRef sNoAccessName As String) As Integer` | Full document deletion: removes pages (physical files optionally), `DOC_page`, `DOC_doc_info`, `DOC_doc_keyword`, `DOC_annotation`, `DOC_document` rows. | — |
| `AddDocToHistory` | `(ByRef lDocNum, ByRef lFoldNum, ByRef sDocName, ByRef dDocDate, ByRef sPageName, ByRef sDocType, ByRef sPageType) As Integer` | Writes an audit event row to the history database via the stored history object. | — |
| `DeleteScannedDoc` | `(ByRef lScanDocNum As Integer) As Integer` | Removes a scanned document staging record. | — |
| `DeleteScannedPageFiles` | `(ByRef lScanDocNum As Integer, ByRef sScanDirectory As String) As Integer` | Removes physical scanned page files from the scan directory. | — |
| `DeleteDocTask` | `(ByRef lDocNum As Integer) As Integer` | Deletes task records associated with a document. | — |
| `GetSADatabase` | `(ByRef vDatabase As Object) As Integer` | Returns a system-administrator-level database connection. | — |
| `IsSBOInstalled` | `(ByRef bInstalled As Boolean) As Integer` | Checks if the Small Business Object (SBO) component is installed. | — |
| `MergeFolders` | `(ByRef v_sInsuranceFileCnt, ByRef v_sPartyCnt, ByRef v_sCompanyID) As Integer` | Merges DME folder structures for insurance-file and party folders for a given company. | `spu_DOC_merge_folders` |
| `GetInsuranceFolderCnt` | `(ByRef vClaimCnt As Object, ByRef vInsuranceFolderCnt As Object) As Integer` | Returns the `insurance_folder_cnt` for a given claim via a JOIN across `Insurance_Folder`, `insurance_file`, and `claim`. | — |

### 1.3 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges folder structures for insurance files and parties |

> **Note**: `AddDocName` delegates to `bDOCDocName.Form.DirectAdd` which calls its own stored procedure (defined in the `bDOCDocName` component).

### 1.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCDocName.Form` | Instantiated in `Form.Initialise`; `DirectAdd` called by `Form.AddDocName` |
| `bDOCDocNameAdmin.Miscellaneous` | Instantiated internally by `Form.Initialise`; used for `GetDocNames` |

---

## 2. bDOCDocTrans

**Directory**: `DME\Components\DOCDocTrans\Business\`  
**Files**: `bDOCDocTrans.vb`, `bDOCDocTransForm.vb`, `bDOCDocTransFormSQL.vb`, `bDOCDocTransCls.vb`

**Purpose**: Provides a thin, lightweight transactional document-insert operation — adds a row to `DOC_document` without history tracking or ID-return. The "No_I" in the SP name indicates no auto-generated identity is returned. Used where a caller supplies the `doc_num` directly.

### 2.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DB if not supplied. | — | — |
| `DirectAdd` | `([vDocNum], [vFolderNum], [vDocName], [vExCode], [vDocType], [vAccessLevel], [vPassword], [vZipped], [vCreateDate], [vLink]) As Integer` | Creates a `bDOCDocTrans.Document` entity, validates and defaults all parameters, then calls `spu_DOC_add_No_I_document`. The caller supplies `doc_num`; no output parameter is used. | `spu_DOC_add_No_I_document` | — |

### 2.2 FormSQL Constants (`bDOCDocTransFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_No_I_document"` |
| `ACAddName` | `"AddDocument"` |
| `ACAddStored` | `True` |

### 2.3 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_No_I_document` | `Form.DirectAdd` (via `AddItem`) | Inserts a document row into `DOC_document`. No identity output parameter — caller provides `doc_num`. |

### 2.4 b\* Component References

None. The `bDOCDocTrans.Document` Cls class is internal (`Friend NotInheritable`).

---

## 3. bDOCDocument

**Directory**: `DME\Components\DOCDocument\Business\`  
**Files**: `bDOCDocument.vb`, `bDOCDocumentForm.vb`, `bDOCDocumentFormSQL.vb`, `bDOCDocumentCls.vb`

**Purpose**: Standard document creation component. Inserts a row into `DOC_document` using a stored procedure that auto-generates and returns the `doc_num` identity. Supports optional `DocumentTemplateID` and `VisibleFromWeb` fields (added in edit history RAM20030429).

### 3.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DB if not supplied. | — | — |
| `DirectAdd` | `([vDocNum], [vFolderNum], [vDocName], [vExCode], [vDocType], [vAccessLevel], [vPassword], [vZipped], [vCreateDate], [vLink], [vDocumentTemplateID], [bVisibleFromWeb]) As Integer` | Creates a `bDOCDocument.Document` entity, validates and defaults all parameters, then calls `spu_DOC_add_document`. Retrieves and returns the auto-generated `doc_num` via an OUTPUT parameter. | `spu_DOC_add_document` | — |

### 3.2 FormSQL Constants (`bDOCDocumentFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_document"` |
| `ACAddName` | `"AddDocument"` |
| `ACAddStored` | `True` |

### 3.3 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_document` | `Form.DirectAdd` (via `AddItem`) | Inserts a document row into `DOC_document` with `DocumentTemplateID` and `VisibleFromWeb` fields; returns auto-generated `doc_num` as OUTPUT parameter. |

### 3.4 b\* Component References

None. The `bDOCDocument.Document` Cls class is internal (`Friend NotInheritable`).

---

## 4. bDOCFind

**Directory**: `DME\Components\DOCFind\Business\`  
**Files**: `bDOCFind.vb`, `bDOCFindForm.vb`, `bDOCMiscellaneous.vb`

**Purpose**: Provides access-controlled document search by name. Finds documents matching a wildcard name pattern and filters results to only those where the current user (by access level) has permission on every folder in the document's ancestor chain up to the specified start folder.

### 4.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DB and instantiates `bDOCFind.Miscellaneous`. | — | `bDOCFind.Miscellaneous` |
| `FindDocument` | `(ByRef sDocName As String, ByRef lMaxReturned As Integer, ByRef vDocArray() As Object, ByRef lStartFoldNum As Integer, ByRef iAccessLevel As Integer, ByRef lMaxExceeded As Integer) As Integer` | Searches `DOC_document` for docs matching `sDocName` (wildcard). For each match, walks the full folder ancestor chain calling `Miscellaneous.GetNodeAccessLevel` and `Miscellaneous.GetNodeParent`. Returns array of accessible `doc_num` values into `vDocArray`. Sets `lMaxExceeded > 0` if result cap exceeded. | — | `bDOCFind.Miscellaneous` |

**Read-only Properties**: `PMProductFamily` (returns pmePFDocumaster), `SourceID`.

### 4.2 Miscellaneous Class

Identical to the `Miscellaneous` class in **bDOCDocNameAdmin** — see [Section 1.2](#12-miscellaneous-class-public-methods) for full method listing. The `bDOCFind.Miscellaneous` class is instantiated internally and only `GetNodeAccessLevel` and `GetNodeParent` are called during `FindDocument`.

### 4.3 Stored Procedures

None. All database access uses inline SQL statements:
- `SELECT doc_num, folder_num FROM DOC_document WHERE doc_name LIKE '...' AND access_level >= N`
- `SELECT access_level FROM DOC_folder WHERE folder_num = N` (via Miscellaneous)
- `SELECT parent_num FROM DOC_folder WHERE folder_num = N` (via Miscellaneous)

### 4.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCFind.Miscellaneous` | Instantiated internally by `Form.Initialise`; `GetNodeAccessLevel` and `GetNodeParent` used in `FindDocument.AccessOK` |

---

## 5. bDOCFolder

**Directory**: `DME\Components\DOCFolder\Business\`  
**Files**: `bDOCFolder.vb`, `bDOCFolderForm.vb`, `bDOCFolderFormSQL.vb`, `bDOCFolderCls.vb`

**Purpose**: Creates folder nodes in the DME folder hierarchy. Calls a stored procedure that auto-generates and returns the `folder_num` identity. Supports all folder attributes including access level, external code, and hierarchical parent.

### 5.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DB if not supplied. | — | — |
| `DirectAdd` | `([vFolderNum], [vFolderName], [vParentNum], [vExCode], [vFolderLevel], [vAccessLevel], [vPassword], [vCreateDate]) As Integer` | Creates a `bDOCFolder.Folder` entity, validates and defaults all parameters (access level defaults to `9`; create date defaults to `Now`), then calls `spu_DOC_add_folder`. Retrieves and returns the auto-generated `folder_num` via OUTPUT parameter. | `spu_DOC_add_folder` | — |

**Read-only Properties**: `PMProductFamily` (returns pmePFDocumaster).

### 5.2 FormSQL Constants (`bDOCFolderFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_folder"` |
| `ACAddName` | `"AddFolder"` |
| `ACAddStored` | `True` |

### 5.3 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_folder` | `Form.DirectAdd` (via `AddItem`) | Inserts a folder row into `DOC_folder`; returns auto-generated `folder_num` as OUTPUT parameter named `Folder_Num`. |

### 5.4 b\* Component References

None. The `bDOCFolder.Folder` Cls class is internal (`Friend NotInheritable`).

---

## 6. bDOCHistory

**Directory**: `DME\Components\DOCHistory\Business\`  
**Files**: `bDOCHistory.vb`, `bDOCHistoryForm.vb`, `bDOCHistoryFormSQL.vb`, `bDOCHistoryCls.vb`

**Purpose**: Records document lifecycle audit events to the DME history database. On initialisation checks whether history database updates are enabled (`UpdateHDBCheck`). `DirectAdd` is a no-op if history writing is disabled.

### 6.1 Form Class Public Properties

| Property | Type | Purpose |
|---|---|---|
| `UpdateHDB` | `Boolean` (R/W) | When `False`, `DirectAdd` returns immediately without writing. Set by `UpdateHDBCheck` on initialisation. |
| `PMProductFamily` | `Integer` (R/O) | Returns `pmePFDocumaster`. |

### 6.2 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(ByRef sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [vDatabase]) As Integer` | Standard lifecycle entry point. Opens DB, calls `UpdateHDBCheck` to set `m_bUpdateHDB`. Note: takes parameters `ByRef` (not `ByVal`) unlike other components. | — | — |
| `DirectAdd` | `([vHistoryID], [vTask], [vCabinetcode], [vCabinetname], [vDrawercode], [vDrawername], [vFoldercode], [vFoldername], [vDocref], [vRequestDate], [vRequestTime], [vEventtype], [vDescription], [vVolume], [vPagefile], [vDoctype], [vFiller], [vHderror], [vCreateDate], [vProcessed]) As Integer` | Writes an audit event row to the history table. Silently returns `PMTrue` if `m_bUpdateHDB = False`. Validates, defaults, and sets all fields on a `bDOCHistory.History` entity, then calls `spu_DOC_add_history`. Retrieves auto-generated `history_id` via OUTPUT parameter. | `spu_DOC_add_history` | — |

### 6.3 FormSQL Constants (`bDOCHistoryFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_history"` |
| `ACAddName` | `"AddHistory"` |
| `ACAddStored` | `True` |

### 6.4 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_history` | `Form.DirectAdd` (via `AddItem`) | Inserts a row into the DME history audit table; returns auto-generated `history_id` as OUTPUT parameter named `History_id`. |

### 6.5 b\* Component References

None. The `bDOCHistory.History` Cls class is `Public NotInheritable` but acts as an internal entity.

---

## 7. bDOCInformation

**Directory**: `DME\Components\DOCInformation\Business\`  
**Files**: `bDOCInformation.vb`, `bDOCInformationForm.vb`, `bDOCMiscellaneous.vb`

**Purpose**: Provides document information retrieval and modification operations for the DME viewer/portal. Orchestrates reads across `DOC_document`, `DOC_Folder`, and `DOC_doc_info`; delegates document renaming with optional history updates; and supports adding text annotations. Internally composes `bDOCHistory.Form` and `bDOCInformation.Miscellaneous`.

### 7.1 Form Class Public Properties

| Property | Type | Purpose |
|---|---|---|
| `EffectiveDate` | `Date` (R/O) | Set to `DateTime.Now` at `Initialise` time. |
| `SourceID` | `Integer` (R/W) | Source identifier for multi-source deployments. |
| `PMProductFamily` | `Integer` (R/O) | Returns `pmePFDocumaster`. |

### 7.2 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DB, creates `bDOCHistory.Form` (passing the DB connection), then creates `bDOCInformation.Miscellaneous` (passing DB and history object). | — | `bDOCHistory.Form`, `bDOCInformation.Miscellaneous` |
| `RenameDoc` | `(ByRef lDocNum As Integer, ByRef sNewName As String) As Integer` | Wraps `Miscellaneous.RenameDoc` in an explicit `BeginTrans`/`CommitTrans`/`RollbackTrans` cycle. | — | `bDOCInformation.Miscellaneous` |
| `GetDocInfo` | `(ByRef lDocNum, ByRef sFolderName, ByRef iAccessLevel, ByRef bExternal, ByRef dCreateDate, ByRef sDocName, ByRef dDocDate, ByRef sScanUser, ByRef dExpiryDate) As PMEReturnCode` | Three-query lookup: (1) reads `folder_num`, `access_level`, `create_date`, `doc_name` from `DOC_document`; (2) reads `folder_name`, `ex_code` from `DOC_Folder`; (3) reads `doc_date`, `expiry_date`, `scan_user` from `DOC_doc_info`. | — | — |
| `GetDocNames` | `(ByRef vDocNamesArray(,) As Object) As Integer` | Wrapper — delegates to `Miscellaneous.GetDocNames`. Returns `{doc_name, doc_name_id}` from `DOC_doc_name`. | — | `bDOCInformation.Miscellaneous` |
| `AddAnnotation` | `(ByRef lDocNum As Integer, ByRef sAnnText As String, ByRef sUsername As String) As Integer` | Instantiates `bDOCAnnotation.Form`, initialises it (using `"iDOCInformation"` as the calling app name), calls `DirectAdd` to create the annotation record, then disposes the object. | — | `bDOCAnnotation.Form` |

### 7.3 Miscellaneous Class

Identical to the `Miscellaneous` class in **bDOCDocNameAdmin** — see [Section 1.2](#12-miscellaneous-class-public-methods) for full method listing. In this component the `Miscellaneous` instance also holds a reference to the `bDOCHistory.Form` object (passed from the `Form` class during `Initialise`) so that `RenameDoc` and `AddDocToHistory` can write history records.

### 7.4 Stored Procedures

None called directly in the Form class. All data access uses inline SQL:

| Inline SQL Target | Used By |
|---|---|
| `DOC_document` | `GetDocInfo` (read) |
| `DOC_Folder` | `GetDocInfo` (read) |
| `DOC_doc_info` | `GetDocInfo` (read) |
| `DOC_doc_name` | `Miscellaneous.GetDocNames` (read) |
| `DOC_folder` / `DOC_document` | `Miscellaneous.RenameNode` (update) |

The `Miscellaneous.MergeFolders` method (shared) calls `spu_DOC_merge_folders` — see Section 1.3.

### 7.5 b\* Component References

| Component | Usage |
|---|---|
| `bDOCHistory.Form` | Instantiated in `Form.Initialise`; passed to `Miscellaneous.Initialise` for audit trail writes via `RenameDoc` / `AddDocToHistory` |
| `bDOCInformation.Miscellaneous` | Instantiated internally by `Form.Initialise`; used by `RenameDoc`, `GetDocNames` |
| `bDOCAnnotation.Form` | Instantiated on demand in `Form.AddAnnotation` |

---

## Complete Stored Procedure Summary

| Stored Procedure | Component(s) | Purpose |
|---|---|---|
| `spu_DOC_add_No_I_document` | bDOCDocTrans | Insert document — no identity return, caller supplies `doc_num` |
| `spu_DOC_add_document` | bDOCDocument | Insert document — returns auto-generated `doc_num` OUTPUT |
| `spu_DOC_add_folder` | bDOCFolder | Insert folder — returns auto-generated `folder_num` OUTPUT |
| `spu_DOC_add_history` | bDOCHistory | Insert history audit event — returns auto-generated `history_id` OUTPUT |
| `spu_DOC_merge_folders` | bDOCDocNameAdmin (Misc), bDOCFind (Misc), bDOCInformation (Misc) | Merge insurance-file and party folder structures for a company |

> All other database operations across these seven components use **inline dynamic SQL** (not stored procedures).

---

## b\* Component Cross-Reference

| Component Referenced | Referenced By |
|---|---|
| `bDOCDocName.Form` | bDOCDocNameAdmin — `Form.AddDocName` delegates to `DirectAdd` |
| `bDOCDocNameAdmin.Miscellaneous` | bDOCDocNameAdmin — internal |
| `bDOCFind.Miscellaneous` | bDOCFind — internal |
| `bDOCInformation.Miscellaneous` | bDOCInformation — internal |
| `bDOCHistory.Form` | bDOCInformation — composed by `Form.Initialise`; passed into Miscellaneous |
| `bDOCAnnotation.Form` | bDOCInformation — instantiated on demand in `Form.AddAnnotation` |

---

## Key DME Tables Referenced

| Table | Used By (operations) |
|---|---|
| `DOC_document` | bDOCDocTrans (INSERT), bDOCDocument (INSERT), bDOCFind (SELECT), bDOCInformation (SELECT), Miscellaneous (SELECT/UPDATE/DELETE) |
| `DOC_folder` | bDOCFolder (INSERT), bDOCFind (SELECT), bDOCInformation (SELECT), Miscellaneous (SELECT/UPDATE/DELETE) |
| `DOC_doc_name` | bDOCDocNameAdmin Form (INSERT/UPDATE/DELETE), Miscellaneous (SELECT) |
| `DOC_doc_info` | bDOCInformation (SELECT), Miscellaneous (DELETE) |
| `DOC_doc_keyword` | Miscellaneous (DELETE single/all) |
| `DOC_annotation` | Miscellaneous (DELETE all); `bDOCAnnotation.Form` (INSERT) |
| `DOC_page` | Miscellaneous (SELECT/DELETE) |
| `DOC_history` | bDOCHistory (INSERT via `spu_DOC_add_history`) |
| `DOC_system` | Miscellaneous (SELECT `admin_level`, `next_page` with UPDLOCK, `doc_date`/`expiry_date` offsets) |
| `DOC_volume` | Miscellaneous.GetDataPath (SELECT) |
| `DOC_device` | Miscellaneous.GetDataPath (SELECT) |
| `Insurance_Folder` | Miscellaneous.GetInsuranceFolderCnt (SELECT JOIN) |
| `insurance_file` | Miscellaneous.GetInsuranceFolderCnt (SELECT JOIN) |
| `claim` | Miscellaneous.GetInsuranceFolderCnt (SELECT JOIN) |

---

## 8. bDOCAnnotation

**Directory**: `DME\Components\DOCAnnotation\Business\`
**Files**: `bDOCAnnotation.vb`, `bDOCAnnotationForm.vb`, `bDOCAnnotationFormSQL.vb`, `bDOCAnnotationCls.vb`

**Purpose**: Creates annotation (free-text note) records attached to DocuMaster documents in the `DOC_annotation` table. Simple single-operation component: only supports `DirectAdd`.

### 8.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUserName, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens a DocuMaster DB connection if `vDatabase` not supplied; stores credentials in module globals. | — | — |
| `Dispose` | `()` | Closes DB if internally opened; releases all references. | — | — |
| `DirectAdd` | `([vAnnotationID], [vDocNum], [vAnnText], [vUsername], [vCreateDate]) As Integer` | Creates a new `Annotation` data object, validates/defaults parameters via `SetProperties`, inserts into `DOC_annotation` by calling `spu_DOC_add_annotation`. Returns new `AnnotationID` via `vAnnotationID` (output param). | `spu_DOC_add_annotation` | — |

### 8.2 FormSQL Constants (`bDOCAnnotationFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_annotation"` |
| `ACAddName` | `"AddAnnotation"` |
| `ACAddStored` | `True` |

### 8.3 Data Entity — `bDOCAnnotation.Annotation` (Cls)

Properties: `AnnotationID`, `DocNum`, `AnnText`, `UserName`, `CreateDate`, `DatabaseStatus`

### 8.4 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_annotation` | `Form.DirectAdd` (via private `AddItem`) | Inserts a row into `DOC_annotation`; returns new `annotation_id` as OUTPUT parameter named `Annotation_id` |

### 8.5 b\* Component References

None — self-contained. Consumed by `bDOCAPI.API`, `bDOCCommit.Form`, `bDOCInformation.Form`.

---

## 9. bDOCAPI

**Directory**: `DME\Components\DOCAPI\Business\`
**Files**: `bDOCAPI.vb`, `bDOCAPICls.vb`, `bDOCMiscellaneous.vb`

**Purpose**: The high-level public API facade for DocuMaster document ingestion. Orchestrates the complete document lifecycle — folder hierarchy creation, writing `DOC_document`, `DOC_doc_info`, `DOC_annotation`, `DOC_page`, and history records, plus physical file transfer (local copy or AWS S3 upload). Also exposes full folder-tree management and query utilities via `bDOCAPI.Miscellaneous`.

### 9.1 API Class Public Methods (`bDOCAPICls.vb`)

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [vDatabase]) As Integer` | Initialises all child business objects sharing the same DB connection. | — | `bDOCFolder.Form`, `bDOCDocument.Form`, `bDOCDocInfo.Form`, `bDOCAnnotation.Form`, `bDOCHistory.Form`, `bDOCPage.Form`, `bDOCAPI.Miscellaneous` |
| `Dispose` | `()` | Releases all child objects and DB connection. | — | — |
| `Add` | `(vIndexArray(,), sDocName, sFilename, sDocType, sPageType, iAccessLevel, sUsername, sKeywords(), [sAnnotation], [oPMBLog], [vDocumentTemplateID], [bVisibleFromWeb]) As Integer` | **Main document add.** Traverses/creates the folder tree, adds document + doc_info + optional annotation + page records, copies/uploads the physical file (local `CopyFile` or AWS S3), writes history. All in a DB transaction. | `spu_GetHiddenOption` (option #99 for `RenewalDocMapping`) | `bDOCDocument.DirectAdd`, `bDOCDocInfo.DirectAdd`, `bDOCAnnotation.DirectAdd`, `bDOCPage.DirectAdd`, `bDOCHistory.DirectAdd`, `m_oMisc.GetNextPageName`, `m_oMisc.GetDataPath`, `m_oMisc.ConstructDocRef`, `m_oMisc.MergeFolders` |
| `AddIndex` | `(vIndexArray(,), [oPMBLog], [bAccelerated]) As Integer` | Creates or updates the folder hierarchy from the index array. Detects existing nodes via `FolderExCodeExists`/`FolderNameExists`; adds missing nodes; renames on name changes; merges duplicate policy folders. Supports accelerated mode (leaf-only add). | — | `m_oMisc.MergeFolders` |
| `MergeIndex` | `(vIndexArray(,), ...) As Integer` | Merges/renames an existing folder index hierarchy. | — | — |
| `DelIndex` | `(vIndexArray(,), ...) As Integer` | Deletes a folder and its entire subtree and documents. | — | `m_oMisc.DeleteFolder` |
| `GetFolderRec` | `(lParentNum, ...) As Integer` | Returns the details of a folder record at a given parent. | — | — |
| `MergeFolder` | `(sTargetCabExCode, ...) As Integer` | Merges one folder's contents into another (de-duplication). | — | — |
| `AddDocumentDirect` | `(sDocName, ...) As Integer` | Adds a document directly, bypassing the folder-walk. | — | `bDOCDocument.DirectAdd`, `bDOCDocInfo.DirectAdd`, `bDOCPage.DirectAdd` |

**Public Properties**: `InsuranceNum` (Write-only Boolean — switches insurance vs. claim folder logic), `PMProductFamily` (ReadOnly), `EffectiveDate` (ReadOnly), `SourceID` (R/W), `DocNumber` (ReadOnly — doc_num of last `Add` call).

### 9.2 Miscellaneous Class Public Methods (`bDOCMiscellaneous.vb`)

The `Miscellaneous` class in bDOCAPI is functionally identical to the one described in **Section 1.2** (bDOCDocNameAdmin) with the same full list of 36 methods. In addition, it also holds the cloud-hosting utilities and policy-merge logic used by the API:

| Method | Additional Notes vs Section 1.2 |
|---|---|
| `GetDataPath` | Same implementation |
| `GetAdminLevel` | Same implementation |
| `GetNextPageName` | Same implementation (with `UPDLOCK` transaction) |
| `ConstructDocRef` | Same implementation |
| `DeleteDocKeyword` | Same implementation |
| `DeleteFolder` | Same implementation (recursive `DeleteFolderTree`) |
| `GetNodeParent` | Same implementation |
| `GetFolderTree` | Same implementation |
| `GetNodeExCode` | Same implementation |
| `GetNodeName` | Same implementation |
| `GetNodeAccessLevel` | Same implementation |
| `RenameNode` | Same implementation |
| `RenameDoc` | Same implementation |
| `AmIExternal` | Same implementation |
| `GetDocNames` | Same implementation |
| `GetDocDateOffSets` | Same implementation |
| `LinkOwner` | Same implementation |
| `DeleteDocInfo` | Same implementation |
| `DeleteDocKeywords` | Same implementation |
| `DeleteDocAnnotations` | Same implementation |
| `GetPageList` | Same implementation |
| `GetPageType` | Same implementation |
| `GetPageName` | Same implementation |
| `GetDocDate` | Same implementation |
| `GetDocLink` | Same implementation |
| `GetDocDetails` | Same implementation |
| `DeleteDocuments` | Same implementation |
| `DeleteDoc` | Same implementation |
| `AddDocToHistory` | Same implementation |
| `DeleteScannedDoc` | Same implementation |
| `DeleteScannedPageFiles` | Same implementation |
| `DeleteDocTask` | Same implementation |
| `GetSADatabase` | Same implementation |
| `IsSBOInstalled` | Same implementation |
| `MergeFolders` | Calls `spu_DOC_merge_folders` — merges duplicate policy-level document folders |
| `GetInsuranceFolderCnt` | Same implementation |

### 9.3 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_GetHiddenOption` | `API.Add` → private `RenewalDocMapping` | Checks hidden option #99 (`AddDocFolderPerInsRef`) to control whether renewal doc folder mapping is active |
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges duplicate policy document folders |

> All other database access in both classes uses inline SQL against `DOC_folder`, `DOC_document`, `DOC_page`, `DOC_doc_info`, `DOC_doc_keyword`, `DOC_annotation`, `DOC_system`, `DOC_volume`, `DOC_device`, `DOC_doc_name`.

### 9.4 b\* Component References

| Component | Declared In | Used For |
|---|---|---|
| `bDOCAnnotation.Form` | `API` private `m_oAnnotation` | Writing annotation records on document add |
| `bDOCDocument.Form` | `API` private `m_oDocument` | Writing document records |
| `bDOCDocInfo.Form` | `API` private `m_oDocInfo` | Writing doc_info records |
| `bDOCFolder.Form` | `API` private `m_oFolder` | Writing folder records |
| `bDOCHistory.Form` | `API` private `m_oHistory`; `Miscellaneous` private `m_oHistory` | Writing history records |
| `bDOCPage.Form` | `API` private `m_oPage` | Writing page records |

---

## 10. bDOCCommit

**Directory**: `DME\Components\DOCCommit\Business\`
**Files**: `bDOCCommit.vb`, `bDOCCommit.Designer.vb`, `bDOCCommitForm.vb`, `bDOCCommitMod.vb`, `bDOCMiscellaneous.vb`

**Purpose**: Client-side component for the DocuMaster scan-and-commit process. Opens both the **scan database** (local) and the **main database** (server, via `bObjectManager`), then commits all staged scanned documents in a batch — writing records to the main DB and physically moving page files to the server document store. An asynchronous Windows Forms timer (`frmTimer` — in `bDOCCommit.vb`) drives the commit loop without blocking the UI.

### 10.1 Module globals (`bDOCCommitMod.vb`)

Key module-level public globals:

| Variable | Type | Purpose |
|---|---|---|
| `g_oMainDB` | `dPMDAO.Database` | Main DocuMaster DB connection |
| `g_oScanDB` | `dPMDAO.Database` | Local scan DB connection |
| `g_oCommitServer` | `bDOCCommitServer.Commit` | Server-side commit object (resolved via object manager) |
| `g_oObjectManager` | `bObjectManager.ObjectManager` | COM+ object manager |
| `g_iRunStatus` | Integer | State machine: `DOCCommitStarted / DOCCommitFinished / DOCCommitCancelled / DOCCommitLocked` |
| `g_lDocsTotal/Done/Failed` | Integer | Progress counters |
| `g_sBatchAnnotation` | String | Default annotation applied to committed docs |
| `g_sScanDirectory` | String | Physical path to local scan directory |
| `g_vTaskCnt` | Byte | Task count for commit batch |

### 10.2 Form Class Public Methods (`bDOCCommitForm.vb`)

| Method | Signature | Purpose | SPs / SQL | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Opens `g_oScanDB` (`pmePFDocumasterScan`) and `g_oMainDB` (`pmePFDocumaster`); creates `bObjectManager.ObjectManager`; resolves `bDOCCommitServer.Commit` via object manager; loads `frmTimer` form. | — | `bObjectManager.ObjectManager`, `bDOCCommitServer.Commit` |
| `Dispose` | `()` | Releases `g_oCommitServer`; closes `frmTimer`. | — | — |
| `CommitBatch` | `([bCommitLocked], [sDefaultAnnotation]) As Integer` | **Main commit method.** Reads all `doc_num` from scan `DOC_document`; calls `GetBusinessObjects` to instantiate child components via object manager; loops calling private `CommitADocument` per doc; tracks progress in module globals; calls `ClearBusinessObjects` on completion. Respects `DOCCommitCancelled` flag. | `SELECT doc_num FROM DOC_document` (scan DB) | `bDOCFolder.Form`, `bDOCDocument.Form`, `bDOCDocInfo.Form`, `bDOCAnnotation.Form`, `bDOCDocKeyword.Form`, `bDOCpage.Form`, `bDOCCommitServer.Commit`, `bPMZipper.Business`, `bDOCCommit.Miscellaneous` |

### 10.3 Public Properties — `bDOCCommit.Form`

| Property | R/W | Type | Notes |
|---|---|---|---|
| `RunStatus` | R/W | Integer | State machine status for polling from the timer |
| `TaskCnt` | R/W | Byte | Number of tasks per commit |
| `BatchAnnotation` | R/W | String | Default annotation applied to all committed docs |
| `ScanDirectory` | R/W | String | Local scan directory path |
| `DocsTotal` | R | Integer | Total documents in current batch |
| `DocsFailed` | R | Integer | Failed commit count |
| `DocsDone` | R | Integer | Successful commit count |
| `PMProductFamily` | R | Integer | Returns `pmePFDocumaster` |

### 10.4 Private Workflow — `CommitADocument` (step sequence)

| Step | Private Method | Action |
|---|---|---|
| 1 | `DocumentCommit` | Reads scan DB doc record; writes to main DB via `bDOCDocument.DirectAdd` |
| 2 | `DocInfoCommit` | Reads scan DB doc_info; writes to main DB via `bDOCDocInfo.DirectAdd` |
| 3 | `AnnotationCommit` | Reads scan DB annotations; writes to main DB via `bDOCAnnotation.DirectAdd` |
| 4 | `KeywordCommit` | Reads scan DB keywords; writes to main DB via `bDOCDocKeyword.DirectAdd` |
| 5 | `PageCommit` | Reads scan DB pages; writes to main DB via `bDOCPage.DirectAdd` |
| 6 | `TaskCommit` | Moves tasks to main DB via `g_oCommitServer.TaskCommit` |
| 7 | `MovePageFiles` | Copies physical files from scan dir to server 00-tree via `bPMZipper.Business.ZipFile`; uses `g_oCommitServer.GetDataPath` to resolve destination path |
| 8 | `m_oMisc.DeleteScannedPageFiles` | Deletes local scan page files from disk |
| 9 | `m_oMisc.DeleteScannedDoc` | Removes document entry from scan DB |

### 10.5 Stored Procedures

None directly — uses inline SQL on the scan DB (`SELECT doc_num FROM DOC_document`). All main DB writes delegate to child components that use their own stored procedures.

### 10.6 b\* Component References

| Component | Usage |
|---|---|
| `bDOCAnnotation.Form` | Writing annotation records from scan DB |
| `bDOCCommitServer.Commit` | Server-side operations: transactions, task commit, data path, page name generation |
| `bDOCDocInfo.Form` | Writing doc_info records |
| `bDOCDocKeyword.Form` | Writing keyword records |
| `bDOCDocument.Form` | Writing document records |
| `bDOCFolder.Form` | Folder lookup/creation during commit |
| `bDOCpage.Form` | Writing page records |
| `bObjectManager.ObjectManager` | COM+ object lifecycle manager |
| `bPMZipper.Business` | File compression/transfer to server document store |

---

## 11. bDOCCommitServer

**Directory**: `DME\Components\DOCCommitServer\Business\`
**Files**: `bDOCCommitServer.vb`, `bDOCCommitServerForm.vb`, `bDOCMiscellaneous.vb`

**Purpose**: Server-side component consumed by `bDOCCommit` via `bObjectManager`. Manages transactions on the main DocuMaster database, provides registry-based commit locking to prevent simultaneous commits from multiple clients, generates next page names, resolves file system data paths, and creates work tasks in the Sirius Architecture (SA) database.

### 11.1 Commit Class Public Methods (`bDOCCommitServerForm.vb`)

| Method | Signature | Purpose | SQL / Notes | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Opens main DocuMaster DB; creates `bDOCHistory.Form` and `bDOCCommitServer.Miscellaneous`. | — | `bDOCHistory.Form` |
| `Dispose` | `()` | Releases all objects and DB connection. | — | — |
| `CheckCommitLock` | `(ByRef bLocked As Boolean) As Integer` | Reads Windows registry key `DOCCommitLockKey` in section `DOCScanSection` (`pmeRSRLocalMachine / pmePFDocumaster / pmeRSLCommon`). Returns `bLocked = True` if value is `"Y"`. | Registry read | — |
| `RemoveCommitLock` | `() As Integer` | Writes `"N"` to `DOCCommitLockKey` in registry. | Registry write | — |
| `PlaceCommitLock` | `() As Integer` | Writes `"Y"` to `DOCCommitLockKey` in registry. | Registry write | — |
| `GetNextPageName` | `(ByRef sNextPageName As String) As Integer` | Wrapper to `m_oMisc.GetNextPageName` — atomically reads and increments `next_page` in `DOC_system` using `WITH (UPDLOCK)` inside a transaction. | Inline SQL on `DOC_system` (via Misc) | `bDOCCommitServer.Miscellaneous` |
| `BeginTrans` | `() As Integer` | Begins a transaction on the main DocuMaster DB. | — | — |
| `GetScanFolderNum` | `(ByRef lFolderNum As Integer) As Integer` | Queries `DOC_folder` for the default standalone scan folder matching constant `DOCDefaultScanFolder`; returns 0 if not found. | `SELECT folder_num FROM DOC_folder WHERE folder_name = '<constant>'` | — |
| `CommitTrans` | `() As Integer` | Commits the main DB transaction. | — | — |
| `GetDataPath` | `(ByRef lVolumeID As Integer, ByRef sDataPath As String) As Integer` | Wrapper to `m_oMisc.GetDataPath` — resolves full UNC/local path from `DOC_volume` + `DOC_device` tables. | Inline SQL on `DOC_volume`, `DOC_device` (via Misc) | `bDOCCommitServer.Miscellaneous` |
| `AddDocToHistory` | `(lDocNum, lFoldNum, sDocName, dDocDate, sPageName, sDocType, sPageType) As Integer` | Wrapper to `m_oMisc.AddDocToHistory` — writes a history audit record. | — | `bDOCHistory.DirectAdd` (via Misc) |
| `RollbackTrans` | `() As Integer` | Rolls back the main DB transaction. | — | — |
| `GetSADatabase` | `(ByRef vDatabase As Object) As Integer` | Returns a live Sirius Architecture DB connection via `gPMComponentServices.CheckDatabase`. | — | — |
| `IsSBOInstalled` | `(ByRef bInstalled As Boolean) As Integer` | Checks if `pmePFSiriusSolutions` is installed via `CheckPMProductInstalled`. | — | — |
| `TaskCommit` | `(v_lPMWrkTaskID, v_lPMWrkTaskGroupID, v_sCustomer, v_dtTaskDueDate, v_lPMUserGroupID, v_sDescription, v_iTaskStatus, v_iIsUrgent, ByRef r_lPMWrkTaskInstanceCnt, v_dtDateCreated, v_iCreatedByID, v_iUserID, v_vKeyArray(,)) As Integer` | Creates a new scheduled work task. Calls `GetSADatabase`, initialises `bPMWrkTaskInstance.FormClass`, calls `CreateNew`. | — | `bPMWrkTaskInstance.FormClass` |
| `GetExCode` | `(ByRef v_vExCode As Object) As Integer` | Returns `max(doc_num) + 1` from `DOC_document` as a new unique external code. | `SELECT max(doc_num) FROM DOC_document` | — |

### 11.2 Stored Procedures

None — uses inline SQL only (`DOC_folder`, `DOC_document`, `DOC_system`, `DOC_volume`, `DOC_device`).

### 11.3 b\* Component References

| Component | Usage |
|---|---|
| `bDOCHistory.Form` | Writing document history records via `AddDocToHistory` |
| `bPMWrkTaskInstance.FormClass` | Creating scheduled work tasks via `TaskCommit` |

---

## 12. bDOCDocInfo

**Directory**: `DME\Components\DOCDocInfo\Business\`
**Files**: `bDOCDocInfo.vb`, `bDOCDocInfoForm.vb`, `bDOCDocInfoFormSQL.vb`, `bDOCDocInfoCls.vb`

**Purpose**: Manages document information records in the `DOC_doc_info` table. Each DocInfo row stores the supplementary metadata for a document: scan user, document date, expiry date, and last-modified user/date.

### 12.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DocuMaster DB if not supplied. | — | — |
| `Dispose` | `()` | Closes DB if internally opened. | — | — |
| `DirectAdd` | `([vDocNum], [vExpiryDate], [vScanUser], [vDocDate], [vLastUser], [vLastDate]) As Integer` | Creates a `DocInfo` entity, validates/defaults all parameters via `SetProperties`, inserts via `spu_DOC_add_doc_info`. | `spu_DOC_add_doc_info` | — |

### 12.2 FormSQL Constants (`bDOCDocInfoFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_doc_info"` |
| `ACAddName` | `"AddDocInfo"` |
| `ACAddStored` | `True` |

### 12.3 Data Entity — `bDOCDocInfo.DocInfo` (Cls)

Properties: `DocNum`, `ExpiryDate`, `ScanUser`, `DocDate`, `LastUser`, `LastDate`, `DatabaseStatus`

### 12.4 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_doc_info` | `Form.DirectAdd` (via private `AddItem`) | Inserts a row into `DOC_doc_info` with all six fields; no identity output (doc_num is the natural key) |

### 12.5 b\* Component References

None — consumed by `bDOCAPI.API`, `bDOCCommit.Form`.

---

## 13. bDOCDocKeyword

**Directory**: `DME\Components\DOCDocKeyword\Business\`
**Files**: `bDOCDocKeyword.vb`, `bDOCDocKeywordForm.vb`, `bDOCDocKeywordFormSQL.vb`, `bDOCDocKeywordCls.vb`

**Purpose**: Manages document keyword associations in the `DOC_doc_keyword` table. Each record links a keyword (by `keyword_id` from the keyword dictionary) to a specific `doc_num`, providing searchable metadata tagging for documents.

### 13.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DocuMaster DB if not supplied. | — | — |
| `Dispose` | `()` | Closes DB if internally opened. | — | — |
| `DirectAdd` | `([vDocKeywordID], [vKeywordID], [vDocNum], [vUsername], [vCreateDate]) As Integer` | Creates a `DocKeyword` entity, validates/defaults parameters, inserts via `spu_DOC_Add_doc_keyword`. Returns new `DocKeywordID` via `vDocKeywordID` output param. | `spu_DOC_Add_doc_keyword` | — |

### 13.2 FormSQL Constants (`bDOCDocKeywordFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_Add_doc_keyword"` |
| `ACAddName` | `"AddKeyword"` |
| `ACAddStored` | `True` |

### 13.3 Data Entity — `bDOCDocKeyword.DocKeyword` (Cls)

Properties: `DocKeywordID`, `KeywordID`, `DocNum`, `UserName`, `CreateDate`, `DatabaseStatus`

### 13.4 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_Add_doc_keyword` | `Form.DirectAdd` (via private `AddItem`) | Inserts a row into `DOC_doc_keyword`; returns new `Doc_Keyword_id` as OUTPUT parameter |

### 13.5 b\* Component References

None — consumed by `bDOCAPI.API`, `bDOCCommit.Form`.

---

## 14. bDOCDocName

**Directory**: `DME\Components\DOCDocName\Business\`
**Files**: `bDOCDocName.vb`, `bDOCDocNameForm.vb`, `bDOCDocNameFormSQL.vb`, `bDOCDocNameCls.vb`

**Purpose**: Manages document name lookup records (`DOC_doc_name` table). Provides a dictionary of reusable document names that are displayed to users when indexing scanned documents. `bDOCDocNameAdmin` builds on top of this component for full CRUD; this component only provides insert (`DirectAdd`).

### 14.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard lifecycle entry point. Opens DocuMaster DB if not supplied. | — | — |
| `Dispose` | `()` | Closes DB if internally opened. | — | — |
| `DirectAdd` | `([vDocNameID], [vDocName]) As Integer` | Creates a `DocName` entity, validates/defaults parameters, inserts via `spu_DOC_add_doc_name`. Returns new `DocNameID` via `vDocNameID` output param. | `spu_DOC_add_doc_name` | — |

### 14.2 FormSQL Constants (`bDOCDocNameFormSQL.vb`)

| Constant | Value |
|---|---|
| `ACAddSQL` | `"spu_DOC_add_doc_name"` |
| `ACAddName` | `"AddDocName"` |
| `ACAddStored` | `True` |

### 14.3 Data Entity — `bDOCDocName.DocName` (Cls)

Properties: `DocNameID`, `DocName`, `DatabaseStatus`

### 14.4 Stored Procedures

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `spu_DOC_add_doc_name` | `Form.DirectAdd` (via private `AddItem`) | Inserts a row into `DOC_doc_name`; returns new `Doc_Name_id` as OUTPUT parameter |

### 14.5 b\* Component References

None — consumed by `bDOCDocNameAdmin.Form.AddDocName` and `bDOCAPI.Miscellaneous.GetDocNames`.

---

## 15. bDOCKeyword

**Directory:** `DME\Components\DOCKeyword\Business\`
**VB Source Files:** `bDOCKeyword.vb` (module), `bDOCKeywordForm.vb` (class `Form`), `bDOCKeywordFormSQL.vb`, `bDOCKeywordMod.vb`, `bDOCKeywordCls.vb` (class `Keyword`)
**Purpose:** Manages global keyword records in the `DOC_keyword` table. Provides a single insert operation for creating keyword lookup entries consumed by `bDOCKeywordAdmin` and document indexing workflows.

### 15.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [vDatabase]) As Integer` | Standard init; opens DB if not supplied | — | — |
| `DirectAdd` | `([vKeywordID], [vKeyword], [vDeleted]) As Integer` | Validates inputs, defaults `deleted='N'`, inserts row into `DOC_keyword`; returns new `keyword_id` via `vKeywordID` OUTPUT param | `spu_DOC_add_keyword` | — |

### 15.2 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_add_keyword` | `Form.DirectAdd` | Inserts a row into `DOC_keyword`; returns new `keyword_id` OUTPUT |

### 15.3 b\* Component References

None — consumed by `bDOCKeywordAdmin.Form.AddKeyword`.

---

## 16. bDOCKeywordAdmin

**Directory:** `DME\Components\DOCKeywordAdmin\Business\`
**VB Source Files:** `bDOCKeywordAdmin.vb` (module), `bDOCKeywordAdminCls.vb` (class `Form`), `bDOCMiscellaneous.vb` (class `Miscellaneous`)
**Purpose:** Admin-level management of `DOC_keyword` records and document-keyword associations. Provides CRUD for keyword definitions and tools to attach/detach keywords on specific documents.

### 16.1 Form Class (`bDOCKeywordAdminCls.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard init; creates `bDOCKeyword.Form` and internal `Miscellaneous` | — | `bDOCKeyword.Form`, `bDOCKeywordAdmin.Miscellaneous` |
| `AddKeyword` | `(sKeyword) As Integer` | Creates a new keyword via `bDOCKeyword.Form.DirectAdd` with `deleted='N'` | `spu_DOC_add_keyword` (via bDOCKeyword) | `bDOCKeyword.Form` |
| `DeleteKeyword` | `(lKeywordID) As Integer` | `UPDATE DOC_keyword SET deleted='Y' WHERE keyword_id={id}` (soft-delete via inline SQL) | — (inline SQL) | — |
| `GetKeywordList` | `(vKeywords(,) ByRef) As Integer` | `SELECT keyword_id, keyword FROM DOC_keyword WHERE deleted='N' ORDER BY keyword` | — (inline SQL) | — |
| `AttachKeyword` | `(lDocNum, iKeyWord) As Integer` | Links a keyword to a document via `bDOCDocKeyword.Form.DirectAdd` | `spu_DOC_Add_doc_keyword` (via bDOCDocKeyword) | `bDOCDocKeyword.Form` |
| `GetDocKeywordIDs` | `(lDocNum, vKeywordIDs(,) ByRef) As Integer` | `SELECT keyword_id FROM DOC_doc_keyword WHERE doc_num={id}` | — (inline SQL) | — |
| `IsAttached` | `(lDocNum, iKeyWord, bAttached ByRef) As Integer` | `SELECT count FROM DOC_doc_keyword WHERE doc_num={id} AND keyword_id={kw}` | — (inline SQL) | — |
| `DeleteDocKeyword` | `(lDocKeywordID) As Integer` | Delegates to `Miscellaneous.DeleteDocKeyword` — `DELETE FROM DOC_doc_keyword WHERE doc_keyword_id={id}` | — | `bDOCKeywordAdmin.Miscellaneous` |

### 16.2 Miscellaneous Class

Shares the identical 37-method `Miscellaneous` class described in component 1 (bDOCDocNameAdmin). Key method called from `Form`:

| Method | Called By Form | Purpose |
|---|---|---|
| `DeleteDocKeyword` | `Form.DeleteDocKeyword` | Deletes a single `DOC_doc_keyword` row |
| `MergeFolders` | — | Merges policy folders; calls `spu_DOC_merge_folders` |

### 16.3 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_add_keyword` | `AddKeyword` (via bDOCKeyword) | Inserts keyword row |
| `spu_DOC_Add_doc_keyword` | `AttachKeyword` (via bDOCDocKeyword) | Links keyword to document |
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges insurance folder structures |

### 16.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCKeyword.Form` | `AddKeyword` |
| `bDOCDocKeyword.Form` | `AttachKeyword`, `GetDocKeywordIDs`, `IsAttached` |
| `bDOCKeywordAdmin.Miscellaneous` | `DeleteDocKeyword` |

---

## 17. bDOCManager

**Directory:** `DME\Components\DOCManager\Business\`
**VB Source Files:** `bDOCManager.vb` (module), `bDOCManagerForm.vb` (class `Form`), `bDOCMiscellaneous.vb` (class `Miscellaneous`)
**Purpose:** Primary DocuMaster document management façade. Provides the full DM client API: folder/document CRUD, tree navigation, copy/move/delete, keyword and annotation management, document import, access-level queries, and Briefcase download mode. Composes all direct-add sub-components and delegates low-level ops to the shared `Miscellaneous` class.

### 17.1 Form Class (`bDOCManagerForm.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Opens DB; instantiates and initialises all sub-components; loads user access level, admin level, home folder, and file/folder permission levels | — | `bDOCFolder.Form`, `bDOCDocument.Form`, `bDOCDocInfo.Form`, `bDOCAnnotation.Form`, `bDOCDocKeyword.Form`, `bDOCPage.Form`, `bDOCHistory.Form`, `bDOCManager.Miscellaneous` |
| `GetPageList` | `(lDocNum, vPageArray ByRef) As Integer` | Checks for linked docs; delegates to `Miscellaneous.GetPageList` | — | `bDOCManager.Miscellaneous` |
| `GetFolderList` | `(lParentNum, sFilter, lMaxFoldersReturned, vResultArray(,) ByRef) As Integer` | Returns child folders filtered by name prefix and access_level | `spu_DOC_select_folder` | — |
| `GetMatchedFolderList` | `(sFolderName, lMaxFoldersReturned, lParentNum, vResultArray(,) ByRef) As Integer` | Returns folders matching a wildcard pattern within a parent | `spu_DOC_select_matched_folders` | — |
| `GetNodeKey` | `(sExCode, iFolderLevel, lParentNum, sFolderNum ByRef, sPassword ByRef, dCreateDate ByRef, bNoAccess ByRef) As Integer` | Looks up folder by external code + level; returns key fields and access result | — (inline SQL) | — |
| `GetDocList` | `(lParentNum, sFilter, vResultArray(,) ByRef) As Integer` | Returns documents in a folder filtered by name and access_level | `spu_DOC_select_document` | — |
| `AmIZippedUp` | `(lDocNum, bZipped ByRef) As Integer` | Checks `DOC_document.zipped` flag | — (inline SQL) | — |
| `GetDocKeywordList` | `(lDocNum, vResultArray(,) ByRef) As Integer` | Returns all keywords for a document with user and date | — (inline SQL: JOIN `DOC_doc_keyword` + `DOC_keyword`) | — |
| `DeleteAnnotation` | `(lAnnId) As Integer` | `DELETE FROM DOC_annotation WHERE annotation_id={id}` | — (inline SQL) | — |
| `DeleteDocKeyword` | `(lDocKeywordID) As Integer` | Delegates to `Miscellaneous.DeleteDocKeyword` | — | `bDOCManager.Miscellaneous` |
| `AddAnnotation` | `(lDocNum, sAnnText) As Integer` | Creates annotation via `bDOCAnnotation.Form.DirectAdd` | — | `bDOCAnnotation.Form` |
| `SetHomeFolder` | `(lFolderNum) As Integer` | `UPDATE DOC_doc_user SET home_folder_num={id} WHERE user_id={user}` | — (inline SQL) | — |
| `GetAnnotationList` | `(lDocNum, vResultArray(,) ByRef) As Integer` | `SELECT annotation_id, ann_text, user_name, create_date FROM DOC_annotation WHERE doc_num={id}` | — (inline SQL) | — |
| `GetFolderTree` | `(lFolderNum, vFolderArray(,) ByRef) As Integer` | Delegates to `Miscellaneous.GetFolderTree` | — | `bDOCManager.Miscellaneous` |
| `GetNodeParent` | `(iNodeType, lNodeNum, lParentNum ByRef) As Integer` | Delegates to `Miscellaneous.GetNodeParent` | — | `bDOCManager.Miscellaneous` |
| `GetFullFolderTree` | `(lNodeNum, iNodeType, vFolderArray(,) ByRef) As Integer` | Walks full ancestry returning `{folder_num, folder_name, password, create_date}` array | — (inline SQL) | — |
| `AmIExternal` | `(iNodeType, lNodeNum, bExternal ByRef) As Integer` | Delegates to `Miscellaneous.AmIExternal` | — | `bDOCManager.Miscellaneous` |
| `CopyDocs` | `(lDestFolder, vDocArray(,), [vPastedDocs(,)]) As Integer` | Copies documents within a transaction; copies annotations and keywords; writes to history for external v2 folders | — (inline SQL) | `bDOCDocument.Form`, `bDOCDocInfo.Form`, `bDOCAnnotation.Form`, `bDOCDocKeyword.Form`, `bDOCHistory.Form`, `bDOCManager.Miscellaneous` |
| `AmIV2Folder` | `(lFolderNum, bV2Folder ByRef) As Integer` | Checks if `folder_level=DOCFolderLevelPolicy` constant | — (inline SQL) | — |
| `CopyFolders` | `(lDestFolder, vFolderArray(,)) As Integer` | Recursively copies folder trees (folders + documents) to a new destination | — (inline SQL) | `bDOCFolder.Form`, `bDOCDocument.Form` |
| `MoveDocs` | `(lDestFolder, vDocArray(,), [vPastedDocs(,)]) As Integer` | Moves docs to new folder in a transaction; writes history for external v2 source/dest | — (inline SQL) | `bDOCHistory.Form`, `bDOCManager.Miscellaneous` |
| `MoveFolders` | `(lDestFolder, vFolderArray(,)) As Integer` | `UPDATE DOC_folder SET parent_num={dest}` for each folder | — (inline SQL) | — |
| `DeleteFolders` | `(vFolderArray, sNoAccessName) As Integer` | Calls `Miscellaneous.DeleteFolder` for each folder | — | `bDOCManager.Miscellaneous` |
| `DeleteDocuments` | `(vDocArray(,), bExternal, sNoAccessName) As Integer` | Delegates to `Miscellaneous.DeleteDocuments` | — | `bDOCManager.Miscellaneous` |
| `RenameFolder` | `(lFoldNum, sNewName) As Integer` | `UPDATE DOC_folder SET folder_name='{name}' WHERE folder_num={id}` | — (inline SQL) | — |
| `NewFolder` | `(lParentNum, sFolderName, dCreateDate, lFolderNum ByRef) As Integer` | Creates new folder via `bDOCFolder.Form.DirectAdd` | — | `bDOCFolder.Form` |
| `RenameDoc` | `(lDocNum, sNewName) As Integer` | Renames document; writes MODDOCUMENT history if external folder | — (inline SQL) | `bDOCHistory.Form` |
| `ImportDocument` | `(sDocName, sPageType, lFoldNum, lPageSize, sTmpPageName, lDocNum ByRef, dDocDate, sZipped) As Integer` | Full document import: creates `DOC_document`, `DOC_doc_info`, `DOC_page` records; writes history if external v2 folder | — | `bDOCDocument.Form`, `bDOCDocInfo.Form`, `bDOCPage.Form`, `bDOCHistory.Form`, `bDOCManager.Miscellaneous` |
| `GetDataPath` | `(lVolumeID, sDataPath ByRef) As Integer` | Delegates to `Miscellaneous.GetDataPath` | — | `bDOCManager.Miscellaneous` |
| `GetFolderInformation` | `(lNodeNum, sExCode ByRef, iFolderLevel ByRef, iAccessLevel ByRef, sPassword ByRef, dCreateDate ByRef) As Integer` | Returns all key attributes of a folder record | — (inline SQL) | — |
| `GetDocumentInformation` | `(lNodeNum, lFolderNum ByRef, sExCode ByRef, sDocType ByRef, iAccessLevel ByRef, sPassword ByRef, dCreateDate ByRef, lLink ByRef, sZipped ByRef, dExpiryDate ByRef, sScanUser ByRef, dDocDate ByRef, sLastUser ByRef, dLastDate ByRef, vPageList ByRef, iVolumeID ByRef) As Integer` | Returns all attributes of a document plus its page list | — (inline SQL) | — |
| `CountChildren` | `(lFolder_Num, lChildren ByRef) As Integer` | Returns count of child nodes (folders + docs) | `spu_DOC_count_children` | — |
| `GetFolderValues` | `(lFolderNum, vResultArray(,) ByRef) As Integer` | Returns raw columns of a folder record | — (inline SQL) | — |
| `BriefcaseUser` | `(sMode, sUser ByRef) As Integer` | Gets or sets the Briefcase download mode user | — (inline SQL) | — |
| `ConnectToLocalDB` | `(sPCName) As Integer` | Connects to the Briefcase local MDF database | — | — |

### 17.2 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_select_folder` | `GetFolderList` | Returns child folders for a parent with access filtering |
| `spu_DOC_select_matched_folders` | `GetMatchedFolderList` | Returns folders matching a wildcard name pattern |
| `spu_DOC_select_document` | `GetDocList` | Returns documents in a folder with access filtering |
| `spu_DOC_count_children` | `CountChildren` | Returns count of child folders + documents |
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges duplicate insurance policy folders |
| `spu_DOC_select_access_levels` | `Miscellaneous` (internal) | Reads access level configuration |
| `spu_DOC_update_access_levels` | `Miscellaneous` (internal) | Updates access level configuration |

### 17.3 b\* Component References

| Component | Usage |
|---|---|
| `bDOCFolder.Form` | `NewFolder`, `CopyFolders` |
| `bDOCDocument.Form` | `CopyDocs`, `CopyFolders`, `ImportDocument` |
| `bDOCDocInfo.Form` | `CopyDocs`, `ImportDocument` |
| `bDOCAnnotation.Form` | `CopyDocs`, `AddAnnotation` |
| `bDOCDocKeyword.Form` | `CopyDocs` |
| `bDOCPage.Form` | `ImportDocument` |
| `bDOCHistory.Form` | `CopyDocs`, `MoveDocs`, `RenameDoc`, `ImportDocument` |
| `bDOCManager.Miscellaneous` | Internal helper |

---

## 18. bDOCOptions

**Directory:** `DME\Components\DOCOptions\Business\`
**VB Source Files:** `bDOCOptions.vb` (module), `bDOCOptionsForm.vb` (class `Form`), `Business.vb` (class `Business`), `bDOCOptionsAssemblyInfo.vb`
**Purpose:** Reads and writes DME system configuration. The legacy `Form` class targets arbitrary `DOC_*` table columns by name via inline SQL; the newer `Business` class manages name/value pairs in the `DOC_options` table via stored procedures.

### 18.1 Form Class (`bDOCOptionsForm.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard init; opens DB | — | — |
| `SetSetting` | `(sTable, sColumn, sValue, bNumber) As Integer` | Generic `UPDATE <sTable> SET <sColumn>=<sValue>` for DME config tables; derives row ID from table name pattern `DOC_<x>` → `<x>_id=1` | — (inline SQL) | — |
| `GetSetting` | `(sTable, sColumn, sValue ByRef) As Integer` | Generic `SELECT <sColumn> FROM <sTable>` for DME config tables | — (inline SQL) | — |
| `IsPMBEnvironment` | `(bPMBEnv ByRef) As Integer` | Stub — always returns `PMFalse`; historically read `update_history` from `DOC_system` | — (stubbed) | — |

### 18.2 Business Class (`Business.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard init; opens DB | — | — |
| `GetOption` | `(sOptionName, sOptionValue ByRef) As Integer` | Returns `option_value` from `DOC_options` WHERE `option_name=sOptionName` | `spu_DOC_get_option` | — |
| `SetOption` | `(sOptionName, sOptionValue) As Integer` | Upserts an option row in `DOC_options` (insert if not exists, update if exists) | `spu_DOC_set_option` | — |
| `CreateOption` | `() As Integer` | Stub — returns `PMTrue`, no implementation | — | — |
| `DeleteOption` | `() As Integer` | Stub — returns `PMTrue`, no implementation | — | — |

### 18.3 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_get_option` | `Business.GetOption` | Reads a named option value from `DOC_options` |
| `spu_DOC_set_option` | `Business.SetOption` | Upserts a named option value in `DOC_options` |

### 18.4 b\* Component References

None.

---

## 19. bDOCPage

**Directory:** `DME\Components\DOCPage\Business\`
**VB Source Files:** `bDOCPage.vb` (module), `bDOCPageForm.vb` (class `Form`), `bDOCPageFormSQL.vb`, `bDOCPageCls.vb` (class `Page`)
**Purpose:** Manages `DOC_page` records (physical page file entries for documents). Single insert operation for creating page entries during document import or scan commit.

### 19.1 Form Class Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [vDatabase]) As Integer` | Standard init; opens DB if not supplied | — | — |
| `DirectAdd` | `([vPageName], [vDocNum], [vPageNum], [vPageType], [vPageSize], [vCreateDate], [vVolumeID]) As Integer` | Validates, defaults (PageNum defaults to 1, CreateDate to Now), inserts row into `DOC_page` | `spu_DOC_add_page` | — |

### 19.2 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_add_page` | `Form.DirectAdd` | Inserts a row into `DOC_page` (page_name, doc_num, page_num, page_type, page_size, create_date, volume_id) |

### 19.3 b\* Component References

None — consumed by `bDOCAPI.API`, `bDOCManager.Form`, `bDOCCommit.Form`, `bDOCTransfer`.

---

## 20. bDOCPassword

**Directory:** `DME\Components\DOCPassword\Business\`
**VB Source Files:** `bDOCPassword.vb` (module), `bDOCPasswordCls.vb` (class `Form`)
**Purpose:** Manages encrypted passwords on DME folder and document nodes. Reads/writes the `password` column in `DOC_folder` and `DOC_document` using inline SQL. Password encryption is delegated to `bPMFunc.Encrypt`.

### 20.1 Form Class (`bDOCPasswordCls.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bstandalone], [vDatabase]) As Long` | Standard init; opens DB | — | — |
| `AddNodePassword` | `(lNodeNum, iNodeLevel, sPassword ByRef) As Integer` | Encrypts the supplied password via `bPMFunc.Encrypt`, then issues `UPDATE DOC_folder SET password=…` or `UPDATE DOC_document SET password=…` depending on `iNodeLevel`; returns encrypted password in `sPassword` | — (inline SQL) | `bPMFunc` |
| `GetNodePassword` | `(lNodeNum, iNodeLevel, sPassword ByRef) As Integer` | `SELECT password FROM DOC_folder WHERE folder_num=…` or `SELECT password FROM DOC_document WHERE doc_num=…` | — (inline SQL) | — |
| `EncryptPassword` | `(sPassword, sEncryptedPassword ByRef) As Integer` | Pure utility — calls `bPMFunc.Encrypt` and returns the encrypted string without touching the DB | — | `bPMFunc` |

### 20.2 Stored Procedures

None — all DB operations use inline SQL.

### 20.3 b\* Component References

| Component | Usage |
|---|---|
| `bPMFunc` | `AddNodePassword` and `EncryptPassword` — password encryption |

---

## 21. bDOCPMBAPI

**Directory:** `DME\Components\DOCPMBAPI\Business\`
**VB Source Files:** `bDOCPMBAPI.vb` (module + global struct declarations), `bDOCPMBAPIForm.vb` (class `Form`)
**Purpose:** PMBroking API daemon processor. Reads journal control files written by the PMB legacy DMS system and imports documents/index data into DocuMaster. Handles add, log, addindex, delindex, and mergeindex control file tasks, manages retry logic and journal error logs, and calls the native `bDOChstry.dll` DLL for DMS history database updates.

### 21.1 Key Global Structures (`bDOCPMBAPI.vb`)

| Structure | Purpose |
|---|---|
| `g_utControlData` | Control file field values (task, cabinetname, drawername, foldername, documentname, keywords, filename, access, etc.) |
| `g_utDMSHistData` | DMS history record (cabinetcode/name, drawercode/name, foldercode/name, docref, date, time, eventtype, pagefile, doctype) |
| `g_utDMSHistParams` | Wrapper around `g_utDMSHistData` with DMSDir, ReturnCode, FileStatus |

### 21.2 Form Class (`bDOCPMBAPIForm.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUserName, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Opens DB; instantiates and initialises `bDOCAPI.API`; creates `bDOCPMBLog.Log`; reads DME registry settings for history root path | — | `bDOCAPI.API`, `bDOCPMBLog.Log` |
| `Start` | `(oPMBLog, bAccelerated, bRetryImports, bRetryExports) As Sub` | Main daemon entry point: gets DME install dir, drives `ProcessMain` for normal import, calls `CommitHDB` to export to DMS history DB via `bDOChstry.dll`, handles retry passes for failed imports/exports; calls `SwapHistoryRoot` | — | `bDOCAPI.API`, `bDOCPMBLog.Log` |
| `RebuildRemoteDB` | `() As Integer` | Triggers rebuild of the remote DMS history database | — | — |

### 21.3 DLL P/Invoke Declarations (`bDOCPMBAPIForm.vb`)

| Function | Library | Purpose |
|---|---|---|
| `NewCab`, `NewDrw`, `NewFld`, `NewDoc` | `bDOChstry.dll` | Create new DMS cabinet/drawer/folder/document in history DB |
| `DelCab`, `DelDrw`, `DelFld`, `DelDoc` | `bDOChstry.dll` | Delete DMS history records |
| `ModCab`, `ModDrw`, `ModFld`, `ModDoc` | `bDOChstry.dll` | Modify DMS history records |

### 21.4 Stored Procedures

None — processing is journal-file and registry based; all DocuMaster writes go via `bDOCAPI.API` sub-components.

### 21.5 b\* Component References

| Component | Usage |
|---|---|
| `bDOCAPI.API` | Drives full document add pipeline for each journal entry |
| `bDOCPMBLog.Log` | Writes progress/error log entries |

---

## 22. bDOCPMBLog

**Directory:** `DME\Components\DOCPMBLog\Business\`
**VB Source Files:** `bDOCPMBLog.vb` (module), `bDOCPMBLogCls.vb` (class `Log`)
**Purpose:** File-based logging utility for DME daemon operations. Writes timestamped, type-tagged message lines to a daily `.log` file in a `logs\` subdirectory. No database access.

### 22.1 Log Class (`bDOCPMBLogCls.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername As String) As Integer` | Stores username; sets log level to 4; sets effective date to `DateTime.Now` | — | — |
| `OpenLogFile` | `(sRootName As String) As Integer` | Opens (or creates and appends to) `{sRootName}logs\{DDMMYY}.log` | — | — |
| `CloseLogFile` | `(sRootName As String)` | Closes the log file; deletes it if empty | — | — |
| `DOCLogMessage` | `(ilogType As Integer, sUsername As String, sMessage() As String)` | Writes `"hh:mm:ss {TYPE} - {username}, {message}"` line to open log file. `ilogType`: 1=MSG, 2=ERR, 3=LOG, 4=WRN, 5=DBG | — | — |

### 22.2 Stored Procedures

None.

### 22.3 b\* Component References

None — consumed by `bDOCPMBAPI.Form`.

---

## 23. bDOCScan

**Directory:** `DME\Components\DOCScan\Business\`
**VB Source Files:** `bDOCScan.vb` (module globals), `bDOCScanCls.vb` (class `Form`), `bDOCMiscellaneous.vb` (class `Miscellaneous`)
**Purpose:** Manages saving and retrieval of scanned documents into the DME scan-station local database (Jet/Access). Handles inserting document records, pages, annotations, keywords, doc-info, and task records into the local scan DB before they are committed to the main server.

### 23.1 Form Class (`bDOCScanCls.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard init; stores credentials and identifiers | — | — |
| `PostInitialise` | `(bStandAlone As Boolean) As Integer` | Opens scan DB and (if not standalone) main Documaster DB connections; creates and initialises internal `Miscellaneous` instance | — | `bDOCScan.Miscellaneous` |
| `SaveDocument` | `(iDocNum, sDocName, vPagesize(), dExpiryDate, dDocDate, vKeywordID(), vAnnotation(), [sPageType, sDocType, sPassword, iAccessLevel, lFolderNum, sScanUser, sCustomer, dtTaskDueDate, lPMUserGroup, lUserID, sDescription, lTaskStatus, iUrgent, dtDateCreated, lCreatedByID]) As PMEReturnCode` | Inserts rows into `DOC_document`, `DOC_annotation`, `DOC_doc_info`, `DOC_doc_keyword`, `DOC_page`, `DOC_Task` using inline INSERT SQL against the local scan DB | — (inline SQL) | — |
| `GetNextDocNum` | `(iDocNum ByRef) As PMEReturnCode` | `SELECT MAX(doc_num) FROM DOC_document`; returns max+1 as next document number | — (inline SQL) | — |
| `GetDocNames` | `(vDocNames ByRef) As PMEReturnCode` | Delegates to `Miscellaneous.GetDocNames` | — | `bDOCScan.Miscellaneous` |
| `GetDocDateOffSets` | `(iDocDateOffset ByRef, iExpiryDateOffset ByRef) As Integer` | Delegates to `Miscellaneous.GetDocDateOffSets` | — | `bDOCScan.Miscellaneous` |

### 23.2 Miscellaneous Class

Shares the identical 37-method `Miscellaneous` class as all other components carrying `bDOCMiscellaneous.vb`. See component 1 (bDOCDocNameAdmin) section 1.2 for the full method table. Key method for this component: `MergeFolders` → `spu_DOC_merge_folders`.

### 23.3 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges insurance folder structures |

*All other operations use inline SQL against local scan DB tables: `DOC_document`, `DOC_annotation`, `DOC_doc_info`, `DOC_doc_keyword`, `DOC_page`, `DOC_Task`, `DOC_doc_name`, `DOC_system`.*

### 23.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCScan.Miscellaneous` | Internal helper — `GetDocNames`, `GetDocDateOffSets` |

---

## 24. bDOCSetAccessLevel

**Directory:** `DME\Components\DOCSetAccessLevel\Business\`
**VB Source Files:** `bDOCSetAccessLevel.vb` (module), `bDOCSetAccessLevelCls.vb` (class `Form`), `bDOCMiscellaneous.vb` (class `Miscellaneous`)
**Purpose:** Reads and updates the access level on DME folder and document nodes. Thin wrapper: gets access level via shared `Miscellaneous` and sets it via inline SQL.

### 24.1 Form Class (`bDOCSetAccessLevelCls.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard init; opens DB; creates internal `Miscellaneous` instance | — | `bDOCSetAccessLevel.Miscellaneous` |
| `GetNodeAccessLevel` | `(iNodeType, lNodeNum, iAccessLevel ByRef) As Integer` | Delegates to `Miscellaneous.GetNodeAccessLevel` — returns `access_level` from `DOC_folder` or `DOC_document` | — | `bDOCSetAccessLevel.Miscellaneous` |
| `SetAccessLevel` | `(iNodeType, lNodeNum, iNewAccessLevel) As Integer` | `UPDATE DOC_folder SET access_level={value}` or `UPDATE DOC_document SET access_level={value}` depending on `iNodeType` | — (inline SQL) | — |

### 24.2 Miscellaneous Class

Same 37-method shared class as bDOCDocNameAdmin. Key method: `MergeFolders` → `spu_DOC_merge_folders`.

### 24.3 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges insurance folder structures |

### 24.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCSetAccessLevel.Miscellaneous` | Internal — `GetNodeAccessLevel` |

---

## 25. bDOCTransfer

**Directory:** `DME\Components\DOCTransfer\Business\`
**VB Source Files:** `bDOCTransfer.vb` (module + all transfer logic), `iDOCTransfer.vb` (UI form `frmInterface`), `iDOCTransferAssemblyInfo.vb`
**Purpose:** One-off data migration utility that transfers the complete cabinet/drawer/folder/document/page/annotation/keyword hierarchy from a Documaster v2 source database to a Documaster Enterprise destination database. All core transfer logic is in private methods; the public surface is minimal.

### 25.1 Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Main` | `Sub Main()` | Entry point — creates and initialises `bObjectManager.ObjectManager`; launches `frmInterface` WinForms dialog | — | `bObjectManager.ObjectManager` |
| `ProcessCommand` | `Sub ProcessCommand()` | Dispatches UI button actions: `ACStarted`→`Transfer()`, `ACAbort`→sets abort flag, `ACViewReport`→`ViewReport()` | — | — |

*All core logic is in private methods: `Transfer`, `TransferCabinets`, `TransferDrawers`, `TransferFolders`, `TransferDocuments`, `TransferPages`, `TransferAnnotations`, `TransferKeywords`, `TransferDocInfo`, `TransferSystem`, `TransferKeywordsDocNames`, `TransferLinkOwners`, `SyncKeywordsDocNames`, `SyncFolders`, `DeleteVirginData`, `GetTotals`, `GetBusinessObjects`, `OpenDatabases`, `CloseDatabases`, `OpenLog`, `CloseLog`, `WriteToLog`, `ViewReport`.*

### 25.2 Stored Procedures

None — all SQL is inline.

### 25.3 b\* Component References

| Component | Variable | Usage |
|---|---|---|
| `bObjectManager.ObjectManager` | `g_oObjectManager` | Initialises system, provides SourceID/LanguageID/UserName |
| `bDOCFolder.Form` | `m_oFolder` | Writes folder/cabinet/drawer records via `DirectAdd` |
| `bDOCDocTrans.Form` | `m_oDocument` | Writes document records (no identity return — v2 doc_nums reused) |
| `bDOCPage.Form` | `m_oPage` | Writes page records |
| `bDOCDocInfo.Form` | `m_oDocInfo` | Writes doc_info records |
| `bDOCDocName.Form` | `m_oDocName` | Writes doc_name records |
| `bDOCAnnotation.Form` | `m_oAnnotation` | Writes annotation records |
| `bDOCDocKeyword.Form` | `m_oDocKeyword` | Writes document-keyword link records |

---

## 26. bDOCUserAdmin

**Directory:** `DME\Components\DOCUserAdmin\Business\`
**VB Source Files:** `bDOCUserAdmin.vb` (module), `bDOCUserAdminCls.vb` (class `Form`), `bDOCMiscellaneous.vb` (class `Miscellaneous`)
**Purpose:** Administers DME user accounts — retrieves/updates per-user access levels, retires users from the DME user list, and manages the system-wide admin level stored in `DOC_system`.

### 26.1 Form Class (`bDOCUserAdminCls.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Long` | Standard init; opens DB | — | — |
| `GetAdminLevel` | `(iAdminLevel ByRef) As Integer` | Creates `Miscellaneous` instance; calls `Misc.GetAdminLevel()` which queries `SELECT admin_level FROM DOC_system` | — | `bDOCUserAdmin.Miscellaneous` |
| `SetAdminLevel` | `(iAdminLevel) As Integer` | `UPDATE DOC_system SET admin_level={value}` | — (inline SQL) | — |
| `GetUserNames` | `(vUserNames ByRef) As Integer` | `SELECT PMUser.user_id, access_level, username, ... FROM PMUser LEFT JOIN DOC_doc_user ON PMUser.user_id=DOC_doc_user.user_id` | — (inline SQL) | — |
| `RemoveUser` | `(iUser) As Integer` | `UPDATE DOC_doc_user SET retired='Y' WHERE user_id={id}` | — (inline SQL) | — |
| `UpdateUsers` | `(vAccessLevels(,)) As Integer` | For each user in array: UPDATE if exists in `DOC_doc_user`, else INSERT from `pmuser` with supplied access level | — (inline SQL) | — |

### 26.2 Miscellaneous Class

Same 37-method shared class as bDOCDocNameAdmin. Key method: `MergeFolders` → `spu_DOC_merge_folders`.

### 26.3 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges insurance folder structures |

### 26.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCUserAdmin.Miscellaneous` | `GetAdminLevel` |

---

## 27. bDOCViewBatch

**Directory:** `DME\Components\DOCViewBatch\Business\`
**VB Source Files:** `bDOCViewBatchMod.vb` (module), `bDOCViewBatch.vb` (class `Form`), `bDOCMiscellaneous.vb` (class `Miscellaneous`)
**Purpose:** Provides the scan-batch viewing interface for scan workstations. Allows browsing queued scanned documents, retrieving document/folder metadata, deleting bad scan batches (files + database records), and working around a Jet engine data-caching issue via database reconnection.

### 27.1 Form Class (`bDOCViewBatch.vb`) Public Methods

| Method | Signature | Purpose | SPs Called | Components Called |
|---|---|---|---|---|
| `Initialise` | `(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName, [bStandAlone], [vDatabase]) As Integer` | Opens scan DB; creates and initialises internal `Miscellaneous` | — | `bDOCViewBatch.Miscellaneous` |
| `GetParentTree` | `(lFoldNum, vFolderTree ByRef) As PMEReturnCode` | Delegates to `Miscellaneous.GetFolderTree` — returns parent folder ancestry | — | `bDOCViewBatch.Miscellaneous` |
| `GetNextDocument` | `(lDocNum ByRef) As Integer` | `SELECT MAX(doc_num) FROM DOC_document` — returns highest existing doc number | — (inline SQL) | — |
| `GetMaxPages` | `(lDocNum, lDocPages ByRef) As Integer` | `SELECT MAX(page_num) FROM DOC_page WHERE doc_num={id}` — returns page count | — (inline SQL) | — |
| `GetDocumentNames` | `(vDocNames(,) ByRef) As Integer` | `SELECT doc_name FROM DOC_document` — returns all doc names in scan DB | — (inline SQL) | — |
| `GetDocFolderNumber` | `(lDocNum, lFolderNum ByRef) As Integer` | `SELECT folder_num FROM DOC_document WHERE doc_num={id}` | — (inline SQL) | — |
| `GetFolderTree` | `(lFolderNum, vFolderArray(,) ByRef) As Integer` | Delegates to `Miscellaneous.GetFolderTree` | — | `bDOCViewBatch.Miscellaneous` |
| `DeleteDocument` | `(lDocNum, sScanDirectory) As Integer` | Calls `Miscellaneous.DeleteScannedPageFiles` then wraps `Miscellaneous.DeleteScannedDoc` in a DB transaction | — | `bDOCViewBatch.Miscellaneous` |
| `DoesDocumentExist` | `(lDocNum, bExists ByRef) As Integer` | `SELECT folder_num FROM DOC_document WHERE doc_num={id}` — returns `bExists=True` if found | — (inline SQL) | — |
| `OpenCloseDatabase` | `() As Integer` | Closes and immediately reopens the scan DB connection — workaround for Jet engine data-caching issue; resets `Miscellaneous.ScanDatabase` reference | — | — |

### 27.2 Miscellaneous Class

Same 37-method shared class as bDOCDocNameAdmin. Key methods used: `GetFolderTree`, `DeleteScannedPageFiles`, `DeleteScannedDoc`, `MergeFolders`.

### 27.3 Stored Procedures

| SP Name | Called From | Purpose |
|---|---|---|
| `spu_DOC_merge_folders` | `Miscellaneous.MergeFolders` | Merges insurance folder structures |

### 27.4 b\* Component References

| Component | Usage |
|---|---|
| `bDOCViewBatch.Miscellaneous` | `GetParentTree`, `GetFolderTree`, `DeleteDocument` |

---

## Complete Stored Procedure Reference

| Stored Procedure | Component(s) | Purpose |
|---|---|---|
| `spu_DOC_add_annotation` | bDOCAnnotation, bDOCAPI | Insert into `DOC_annotation`; returns `annotation_id` OUTPUT |
| `spu_DOC_add_doc_info` | bDOCDocInfo | Insert into `DOC_doc_info`; doc_num is key (no identity) |
| `spu_DOC_Add_doc_keyword` | bDOCDocKeyword, bDOCKeywordAdmin | Insert into `DOC_doc_keyword`; returns `Doc_Keyword_id` OUTPUT |
| `spu_DOC_add_doc_name` | bDOCDocName, bDOCDocNameAdmin | Insert into `DOC_doc_name`; returns `Doc_Name_id` OUTPUT |
| `spu_DOC_add_No_I_document` | bDOCDocTrans, bDOCTransfer | Insert into `DOC_document`; no identity return — caller supplies `doc_num` |
| `spu_DOC_add_document` | bDOCDocument, bDOCAPI | Insert into `DOC_document`; returns `doc_num` OUTPUT |
| `spu_DOC_add_folder` | bDOCFolder, bDOCManager, bDOCAPI | Insert into `DOC_folder`; returns `folder_num` OUTPUT |
| `spu_DOC_add_history` | bDOCHistory | Insert into `DOC_history`; returns `history_id` OUTPUT |
| `spu_DOC_add_keyword` | bDOCKeyword, bDOCKeywordAdmin | Insert into `DOC_keyword`; returns `keyword_id` OUTPUT |
| `spu_DOC_add_page` | bDOCPage, bDOCAPI, bDOCManager | Insert into `DOC_page` |
| `spu_DOC_count_children` | bDOCManager | Returns count of child folders + documents for a parent |
| `spu_DOC_get_option` | bDOCOptions (Business) | Reads named option from `DOC_options` |
| `spu_DOC_merge_folders` | bDOCDocNameAdmin (Misc), bDOCFind (Misc), bDOCInformation (Misc), bDOCAPI (Misc), bDOCKeywordAdmin (Misc), bDOCManager (Misc), bDOCScan (Misc), bDOCSetAccessLevel (Misc), bDOCUserAdmin (Misc), bDOCViewBatch (Misc) | Merges duplicate insurance/party folder structures |
| `spu_DOC_select_access_levels` | bDOCManager (Miscellaneous) | Reads access level configuration |
| `spu_DOC_select_document` | bDOCManager | Returns documents in a folder with access filtering |
| `spu_DOC_select_folder` | bDOCManager | Returns child folders for a parent with access filtering |
| `spu_DOC_select_matched_folders` | bDOCManager | Returns folders matching a wildcard name pattern |
| `spu_DOC_set_option` | bDOCOptions (Business) | Upserts named option in `DOC_options` |
| `spu_DOC_update_access_levels` | bDOCManager (Miscellaneous) | Updates access level configuration |
| `spu_GetHiddenOption` | bDOCAPI (API.Add) | Reads hidden option value (option #99 for renewal folder mapping) |

---

*End of DME Business Components Reference*

