# Shared Files — Components Reference

> **Location:** `Shared Files\` (Components\, ComponentServices\, ControlHelpers\, gACTLibrary\, gCLMLibrary\, gPMLibrary\, gSIRLibrary\, Swift\)
> **Purpose:** Cross-product shared modules used by every layer of PureInsurance — constants, utilities, foundation libraries, interface helpers, and business component utilities.
> **Referenced by:** `.github/copilot-instructions.md`

---

## Architecture Overview

Shared Files contains the lowest-level reusable modules in the platform. They form a strict layering hierarchy:

```
Product/Application layer (GII Motor, Household, Truck, schemes)
  └──► PB* (Product Builder risk screen engine, tab management)
  └──► GII*/GIS*/GEM* (product property constants, quote object names)
  └──► SIR* / Renewal* (policy lifecycle constants)
  └──► PM* / PMB* constants (broking, platform)

Interface layer (i* modules)
  └──► iPMFunc           (central interface utility — logging, encryption, options)
  └──► iPMForms          (form field registration + caption display)
  └──► iPMListViewFunc / iPMListView6Func (ListView helpers)
  └──► iPMValidate       (GotFocus/LostFocus input validation)
  └──► iGIIFunc / iGIISchemeFlags (GII screen/scheme helpers)
  └──► iGISSharedConstants (interface-layer GIS constants + path resolution)
  └──► iACTFunc / iSIRToolbarFunc / iSIRPremFinConst (domain interface utilities)
  └──► iPMBToolbarFunc / OrionFuncLink / PartyBuilderHandler / PartyFunc (form-level dispatchers)

Business component layer (b* modules)
  └──► bPMFunc           (platform: logging, encryption, options, currency)
  └──► bPMDocFunctions   (document lifecycle: Word, file I/O, ZIP)
  └──► bGEMFunc          (database + business object factory)
  └──► bPMAddParameter   (SP/SQL parameter builder)
  └──► bACTFunc          (accounting ledger/sub-branch lookups)
  └──► bGISTemp          (GIS transaction step constants + tracking)
  └──► bUnderwritingBranchFunc (UW branch + scheme/broker lookups)
  └──► bPMNavConstants   (ACE Navigator array column positions)
  └──► bSIRPremFinConst  (Premium Finance constants ~300+ fields)
  └──► bPMShellFunc      (blocking Win32 shell execution)
  └──► bPMWrkTaskInstance (named DB sequence lock enum)
  └──► bTempFunc         (legacy stub — do not reference in new code)

Foundation constants & utilities
  └──► gPMConstants      (master enums + constants for entire platform)
  └──► gPMFunctions      (core utilities: registry, logging, data conversion, format)
  └──► gPMMaths          (insurance-grade precision arithmetic)
  └──► gPMFunctions64    (WOW64 registry access)
  └──► gPMRegConst       (UIntPtr handles for Win32 registry hives)
  └──► gSIRLibrary       (insurance domain constants: party codes, GL codes, SPs)
  └──► gACTLibrary       (Orion accounting constants: doc types, statuses)
  └──► gCLMLibrary       (claims: EnableClaimVersions option + VCV registry)
  └──► gPMComponentServices (COM+ object factory + DB session management)
  └──► ListViewItemComparer (type-aware IComparer for ListView sorting)
```

> **Module name mismatches** (file name ≠ internal module name):
> `gSIRPFConst.vb` → module `gPFConst` | `DOCPrinterFunc.vb` → module `PrinterFunc` |
> `iPMBToolbarFunc.vb` → module `PMBToolbarFunc` | `iGIIFunc.vb` → module `GIIFunctions` (⚠ name conflict with `GIIFunc.vb`) |
> `SIRRenewalConst.vb` → module `RenewalConst` | `Validate.vb` → module `iValidateFunc` |
> `LstVwGdLine.vb` → module `modLst` | `bPMNavConstants.vb` → module `NavigatorConstants` |
> `bPMShellFunc.vb` → module `ShellFunc` | `bTempFunc.vb` → module `TempFunc` |
> `ACTCompanyCurrencyConst.vb` → module `CompanyCurrencyConst`

---
## Component Index

| # | File | Module | Location | Type | Methods | SPs | Purpose Summary |
|---|------|--------|----------|------|---------|-----|-----------------|
| 1 | ACTBatchConst.vb | `ACTBatchConst` | Components | Module | — | — | Orion batch processing array field positions |
| 2 | ACTCompanyCurrencyConst.vb | `CompanyCurrencyConst` | Components | Module | — | — | Currency query selectors + array positions |
| 3 | ACTConst.vb | `ACTConst` | Components | Module | — | — | Orion accounting global constants (doc types, statuses) |
| 4 | ACTExplorerConst.vb | `ACTExplorerConst` | Components | Module | — | — | Chart of Accounts explorer error codes + result positions |
| 5 | ACTFunc.vb | `ACTFunc` | Components | Module | 5 | — | Company base currency + array parse utilities |
| 6 | ADVReg.vb | `ADVReg` | Components | Module | 6 | — | Win32 advapi32 registry CRUD wrapper |
| 7 | bACTFunc.vb | `bACTFunc` | Components | Module | 3 | 2 | Accounting ledger/sub-branch ID lookups |
| 8 | bGEMFunc.vb | `bGEMFunc` | Components | Module | 4 | — | Gemini/GIS database + business object factory |
| 9 | bGISTemp.vb | `bGISTemp` | Components | Module | 1 | 1 | GIS transaction step constants + `gis_policy_link` update |
| 10 | bPMAddParameter.vb | `bPMAddParameter` | Components | Module | 2 | — | dPMDAO SP/SQL parameter builder |
| 11 | bPMDocFunctions.vb | `bPMDocFunctions` | Components | Module | 20 | — | Word lifecycle, file/dir ops, ZIP, conversion |
| 12 | bPMFunc.vb | `bPMFunc` | Components | Module | 28 | — | Core platform: logging, encryption, options, currency |
| 13 | bPMNavConstants.vb | `NavigatorConstants` ⚠ | Components | Module | — | — | ACE Navigator result array column positions |
| 14 | bPMShellFunc.vb | `ShellFunc` ⚠ | Components | Module | 1 | — | Blocking Win32 shell-process execution |
| 15 | bPMWrkTaskInstance.vb | `bPMWrkTaskInstance` | Components | Module | 2 props | — | Named DB lock enum + bidirectional lookup |
| 16 | bSIRPremFinConst.vb | `bSIRPremFinConst` | Components | Module | — | — | Premium Finance array constants (~300+ fields) |
| 17 | bTempFunc.vb | `TempFunc` ⚠ | Components | Module | 3 | — | Legacy stub — do not use in new code |
| 18 | bUnderwritingBranchFunc.vb | `bUnderwritingBranchFunc` | Components | Module | 3 | 2 | UW branch indicator + scheme/broker lookups |
| 19 | DOCConst.vb | `DOCConst` | Components | Module | — | — | DME/DocuMaster constants (doc types, folder levels, registry keys) |
| 20 | DOCGeneralFunc.vb | `DOCGeneralFunc` | Components | Module | 18 | — | DME file ops, registry, S3, cache utilities |
| 21 | DOCPrinterFunc.vb | `PrinterFunc` ⚠ | Components | Module | 5 | — | Word/Excel/RTF document printing via COM |
| 22 | gArrays.vb | `gArrays` | Components | Module | 2 | — | Array dimension test + bounds query |
| 23 | GeminiConst.vb | `GeminiConstants` | Components | Module | — | — | Gemini platform constants (data types, process IDs, status codes) |
| 24 | GeminiFunc.vb | `GeminiFunctions` | Components | Module | 7 | — | Tag parsing, registry settings, apostrophe escaping, bubble sort |
| 25 | GeminiNetFunctions.vb | `GeminiNetFunctions` | Components | Module | 3 | — | Primary risk data summary + email sending (Gemini/Transact) |
| 26 | GEMListCustomConst.vb | `ListCustomConst` | Components | Module | — | — | Custom list item field positions (8 constants) |
| 27 | GEMListMgrConst.vb | `GEMListMgrConst` | Components | Module | — | — | List manager field positions + list type/change flags |
| 28 | GEMListsConst.vb | `ListsConst` | Components | Module | — | — | List definition header positions (3 constants) |
| 29 | GEMListUserConst.vb | `ListUserConst` | Components | Module | — | — | User list item positions (4 constants) |
| 30 | GeneralConst.vb | `GeneralConst` | Components | Module | — | — | Foundation platform constants (return codes, log levels, system options) |
| 31 | GeneralFunc.vb | `GeneralFunc` | Components | Module | 10 | — | Rounding, formatting, registry read/write, encryption |
| 32 | GIIConst.vb | `GIIConstants` | Components | Module | — | — | Gemini II constants (statuses, QuoteType, COBOL fields, GIS property shortcuts) |
| 33 | GIIFunc.vb | `GIIFunctions` | Components | Module | 24 | — | WinForms UI helpers (TreeView, validation, form positioning, date fields) |
| 34 | GIIGISConstants.vb | `GIIGISConstants` | Components | Module | — | — | Motor GIS object/property name constants (~300+ properties) |
| 35 | GIIHConstants.vb | `GIIHConstants` | Components | Module | — | — | Household GIS property name constants (~500+ properties) |
| 36 | GIITConstants.vb | `GIITConstants` | Components | Module | — | — | Truck GIS property name constants (~200+ properties) |
| 37 | GISDataModelType.vb | `GISDataModelType` | Components | Module | — | — | GIS data model type discriminators (7 DM types, 11 object types) |
| 38 | GISPromptConstants.vb | `GISPromptConstants` | Components | Module | — | — | Prompt Premium Finance integration constants |
| 39 | GisSchemeConst.vb | `GisSchemeConst` | Components | Module | — | — | Scheme array positions + ABI insurer numbers + QM reference strings |
| 40 | GISSharedConstants.vb | `GISSharedConstants` | Components | Module | 2 | — | Central GIS constants + brand name / data type mapping |
| 41 | GISSharedPropertyConstants.vb | `GISSharedPropertyConstants` | Components | Module | — | — | Specials types, Swift display flags, GIS edit flags |
| 42 | gHUBSpokeConstants.vb | `gHUBSpokeConstants` | Components | Module | — | — | Hub-spoke batch processing constants (21 interface codes) |
| 43 | gPMGetRuleFileLocation.vb | `gPMGetRuleFileLocation` | Components | Module | 1 | — | Reads GIS rule file path from registry |
| 44 | gSIRPFConst.vb | `gPFConst` ⚠ | Components | Module | — | — | Premium Finance BACS/DDM transaction type constants |
| 45 | iACTFunc.vb | `iACTFunc` | Components | Module | 3 | — | SetListIndex, child form positioning, C-style sprintf |
| 46 | iGeneralFunc.vb | `iGeneralFunc` | Components | Module | 5 | — | Logging, text select, mouse pointer, form centering |
| 47 | iGIIFunc.vb | `GIIFunctions` ⚠ | Components | Module | 6 | — | GII screen/form display settings (GeminiII product family) |
| 48 | iGIISchemeFlags.vb | `SchemeFlags` | Components | Module | 2 | — | 14-bit scheme flag bitmask encode/decode |
| 49 | iGISSharedConstants.vb | `iGISSharedConstants` | Components | Module | 8 | — | Interface GIS constants + path resolution methods |
| 50 | InsuranceFileConst.vb | `InsuranceFileConst` | Components | Module | — | — | 138-field insurance file array positions |
| 51 | iPMBListEvents.vb | `iPMBListEvents` | Components | Module | 1 | — | Launches event browser component with context |
| 52 | iPMBToolbarFunc.vb | `PMBToolbarFunc` ⚠ | Components | Module | 6 | — | PMB toolbar button dispatcher (Find Policy/Claim/Account/Notes/Letter/Email) |
| 53 | iPMForms.vb | `iPMForms` | Components | Module | 10 | — | Form field format/type/caption/validation management |
| 54 | iPMFunc.vb | `iPMFunc` | Components | Module | ~35 | — | Central interface utility: logging, encryption, Windows, resources |
| 55 | iPMListView6Func.vb | `ListView6Func` | Components | Module | 8 | — | ListView V6 sort, batch start/end, move item |
| 56 | iPMListViewFunc.vb | `ListViewFunc` | Components | Module | 14 | — | ListView sort by date/value/currency, ledger styling |
| 57 | iPMValidate.vb | `iPMValidate` | Components | Module | 6 | — | Date/time/integer GotFocus/LostFocus validation + IsChar |
| 58 | IPTConsts.vb | `IPTConsts` | Components | Module | — | — | Insurance Premium Tax constants (exempt postcodes, result flags) |
| 59 | iSIRPremFinConst.vb | `iSIRPremFinConst` | Components | Module | 3 | — | PF shared state + ProductClass/StatusText/InstalmentStatus converters |
| 60 | iSIRToolbarFunc.vb | `SIRToolbarFunc` | Components | Module | 5 | — | SIR toolbar routing (Accounts, Notes, Letter, Cash Deposit, Commission) |
| 61 | JobConstants.vb | `JobConstants` | Components | Module | — | — | Windows AT job scheduling constants + renewal batch job names |
| 62 | LstVwGdLine.vb | `modLst` ⚠ | Components | Module | 2 | — | ListView gridlines via Win32 comctl32 |
| 63 | MSWordFunctions.vb | `MSWordFunctions` | Components | Module | 5 | — | Word bookmark insert/navigate/delete/get |
| 64 | NavProcConst.vb | `NavProcConst` | Components | Module | — | — | Navigator/Process Map framework constants |
| 65 | OrionFuncLink.vb | `OrionFuncLink` | Components | Module | 3 | — | Launches Orion accounting components (Accounts/Transactions/Fees) |
| 66 | PartyBuilderHandler.vb | `PartyBuilderHandler` | Components | Module | 4 | — | GIS Party Builder custom data screen dispatcher |
| 67 | PartyFunc.vb | `PartyFunc` | Components | Module | 10 | 1 | Party UI helpers, loyalty/alt-ID validation, XSD schema |
| 68 | PBDatabaseConsts.vb | `PBDatabaseConsts` | Components | Module | — | — | GIS screen metadata array positions (screen detail + header) |
| 69 | PBGetAddressFromAddressCnt.vb | `PBGetAddressFromAddressCnt` | Components | Module | 1 | — | Inline SQL address lookup by address_cnt |
| 70 | PBObjectAndPropertyConsts.vb | `pbObjectAndPropertyConsts` | Components | Module | — | — | GIS Object + GIS Property dictionary array positions |
| 71 | PBQuoteTypeEncode.vb | `PBQuoteTypeEncode` | Components | Module | 3 | — | Quote type encode/decode + description string |
| 72 | PBReadScriptColumn.vb | (none) | Components | Module | 1 | 1 | Reads large-text script column from GIS screen header |
| 73 | PBRiskScreenCommon.vb | `PBRiskScreenCommon` | Components | Module | 2 | — | Risk screen shared constants, Win32 APIs, global state |
| 74 | PBRiskScreenCommon2.vb | `PBRiskScreenCommon2` | Components | Module | 12 | — | Runtime risk screen UI engine (build/init/populate controls) |
| 75 | PBTabStripCommon.vb | `PBTabStripCommon` | Components | Module | 7 | — | Tab management (add/hide/select/caption) for TabControl |
| 76 | PIRConsts.vb | `PIRConsts` | Components | Module | — | — | Insurer Rate lookup array positions (10 constants) |
| 77 | PMAPIFunc.vb | `PMAPIFunc` | Components | Module | 6 | — | Win32 window management utilities (topmost, close button, find, maximize) |
| 78 | PMBConst.vb | `PMBConst` | Components | Module | — | — | Sirius Broking constants (party types, events, documents, lookup codes) |
| 79 | PMBGeneralFunc.vb | `PMBGeneralFunc` | Components | Module | 14 | — | Broking UI helpers (field focus, combo fill, postcode validation) |
| 80 | PMConst.vb | `PMConst` | Components | Module | — | — | Master application constants (return codes, log levels, Navigator, DB) |
| 81 | PMCopyFile.vb | `PMCopyFile` | Components | Module | 1 | — | Safe Win32-aware file copy with diagnostic error messages |
| 82 | PMFunc.vb | `PMFunc` | Components | Module | 13 | — | Round/truncate, format/unformat, registry, encrypt, SQL wildcard |
| 83 | PMHelpFunc.vb | `PMHelpFunc` | Components | Module | 2 | — | WinHelp context / search display via registry-configured help file |
| 84 | PMNavKeyConst.vb | `PMNavKeyConst` | Components | Module | — | — | All Navigator key-name string constants for the entire platform |
| 85 | PolicyNumberGen.vb | `PolicyNumberGen` | Components | Module | 3 | — | GII insurer-specific policy number generation (MMA/NU/NIG/etc) |
| 86 | QuoteConst.vb | `QuoteConst` | Components | Module | — | — | GII quote data-object hierarchy constants (object names, property keys) |
| 87 | SIRConst.vb | `SIRConst` | Components | Module | — | — | Sirius constants (party types, transaction codes, document types, RI arrays) |
| 88 | SiriusCoreFunc.vb | `SiriusCoreFunc` | Components | Module | 3 | 2 | Branch/sub-branch list retrieval + loyalty/alt-ID script loading |
| 89 | SIRRenewalConst.vb | `RenewalConst` ⚠ | Components | Module | — | — | Renewal lifecycle constants (status codes, action codes, EDI messages) |
| 90 | SSfunc.vb | `SSfunc` | Components | Module | 1 | — | WinForms help display via registry-configured help file |
| 91 | TopMost.vb | `TopMost` | Components | Module | 2 | — | Win32 always-on-top window flag set/clear |
| 92 | Validate.vb | `iValidateFunc` ⚠ | Components | Module | 6 | — | GotFocus/LostFocus date/time/integer validation + IsChar |
| 93 | gPMComponentServices.vb | `gPMComponentServices` | ComponentServices | Module | 7 | 1 | COM+ object factory + database session management |
| 94 | ListViewItemComparer.vb | `ListViewItemComparer` | ControlHelpers | Class | 1 | — | Type-aware IComparer for ListView column sorting |
| 95 | gACTLibrary.vb | `gACTLibrary` | gACTLibrary | Module | — | — | Orion accounting constants/enums (doc types, ledgers, statuses) |
| 96 | gCLMLibrary.vb | `gCLMLibrary` | gCLMLibrary | Module | 4 | — | Claims: EnableClaimVersions option + VCV registry helpers |
| 97 | gPMConstants.vb | `gPMConstants` | gPMLibrary | Module | — | — | Master platform enums: return codes, log levels, process modes, SIRHiddenOptions |
| 98 | gPMFunctions.vb | `gPMFunctions` | gPMLibrary | Module | ~50 | — | Core utilities: registry (Win32+VB), logging, data conversion, format, LDAP |
| 99 | gPMFunctions64.vb | `gPMRegistryFunctionsWOW6432` | gPMLibrary | Module | 3 | — | WOW64 registry access (32-bit and 64-bit hive access) |
| 100 | gPMMaths.vb | `gPMMaths` | gPMLibrary | Module | 8 | — | Insurance precision arithmetic (truncate/roundup VDecimal + Currency) |
| 101 | gPMRegConst.vb | (none) | gPMLibrary | Module | — | — | UIntPtr constants for Win32 HKEY_LOCAL_MACHINE / HKEY_CURRENT_USER |
| 102 | gSIRLibrary.vb | `gSIRLibrary` | gSIRLibrary | Module | — | — | Insurance domain constants/enums (GL codes, party types, doc types, lookup tables) |
| 103 | Registry.vb | `MRegistry` | Swift | Module | 4 | — | Swift FactFind registry read/write for HKLM and HKCU |
| 104 | SecurityConstants.vb | `MSecurityConstants` | Swift | Module | 4 | — | Swift security: bespoke flags, module/app IDs, user access rights |

---
---

## Foundation Libraries

### gPMConstants

**File:** `gPMLibrary\gPMConstants.vb` | **Module:** `gPMConstants` | **ProgId:** `gPMConstants_NET.gPMConstants`

The master constants/enumerations module for the entire Pure/Sirius platform. Every other module imports this. No executable methods — pure enums and constants.

**Key enumerations:**

| Enum | Purpose | Key Members |
|------|---------|-------------|
| `PMEReturnCode` | All system return codes | `PMFalse=0, PMTrue=1, PMError=11, PMSucceed=12, PMNotFound=811`; Auth (200s), Interface (300+), Business (600+), Navigator (700+), DB (800+) |
| `PMELogLevel` | Log severity levels | `PMLogFatal=1` through `PMLogFeedback=10` |
| `PMEFormatStyle` | Field display formats | `PMFormatString=0, PMFormatDateShort=1, PMFormatDateLong=2, PMFormatDateMed=3, PMFormatCurrency=6, PMFormatInteger=7, PMFormatBoolean=8` (32 values total) |
| `PMEComponentAction` | CRUD action codes | `View=0, Add=1, Edit=2, Delete=3, Added=10, Reverse=11, Copy=20` |
| `PMEProcessMode` | Transaction process modes | `Generic=0, Enquiry=1, NBQuote=2, NBLive=4, RNQuote=8, RNLive=16, MTAQuote=32, MTALive=64` |
| `PMEDataType` | DB parameter data types | `String=0, Date=1, Currency=2, Integer=3, Long=4, Double=5, Boolean=6, Lookup=13` |
| `PMEParameterDirection` | SP param directions | `Input=0, InputOutput=1, Output=2, ReturnValue=3` |
| `SIRHiddenOptions` | Product feature flags | 116 entries `SIROPTUnderwriting=1` through `SIROPTCopyRiskInMTA=116` |
| `PMEWrkManTaskType` | Work manager task types | `Memo=0, SingleComponent=1, NavigatorProcess=2` |
| `PMEAutoNumberType` | Auto-number lock types | `InsFile=1, InsFolder=2, Party=3, Claim=4, CoverNoteBook=5, Address=6` |
| `PMEVDecimalNoOfDP` | Variable decimal places | 0–6 |
| `PMECurrencyNoOfDP` | Currency decimal places | 0–4 |

**Key constant groups:** Registry paths (`ACRegRoot="SOFTWARE\Pure"`, `ACRegRoot64`), event log names (`EVENT_LOG_APP_NAME="PUREInsurance"`), type-of-business codes (`PMTypeOfBusinessNB="NB"`, `RN`, `ENN`, `ENR`), MTA types, renewal modes, transaction sub-types, insurance file risk link types.

---

### gPMFunctions

**File:** `gPMLibrary\gPMFunctions.vb` | **Module:** `gPMFunctions` | **ProgId:** `gPMFunctions_NET.gPMFunctions`

Core platform utility module (~3,656 lines). The most widely-used utility module after `gPMConstants`.

**Public Property:** `LogWriter As LogWriter` — Enterprise Library `LogWriter` (set: private)

**Key methods:**

| Method | Description |
|--------|-------------|
| `GetPMRegSetting` / `SetPMRegSetting` | Win32 `RegOpenKeyEx`/`RegCreateKeyEx` registry read/write using `SOFTWARE\Pure\` path hierarchy |
| `GetRegSettings` / `SaveRegSettings` / `DeleteRegSettings` | VB `Interaction.GetSetting`/`SaveSetting`/`DeleteSetting` wrappers |
| `BuildKeyString` | Constructs registry subkey from product family + level + optional sub-key |
| `LogMessageToFile` | Routes to `EventLogWrite` (Enterprise Library). 12+ overloads for partial param combinations |
| `LogMessagePopup` | Writes to event log + shows `MsgBox`. 8+ overloads |
| `EventLogWrite` | Writes to Enterprise Library Logging application block |
| `RaiseError` (×2) | Throws `System.Exception` with source+description |
| `FormatField` | Formats a value by `PMEFormatStyle` (date/time/currency/boolean/percent etc.) |
| `UnFormatField` | Reverses `FormatField`; converts to target `PMEDataType` |
| `NullToBoolean/Date/Decimal/Double/Integer/String` | DB null-safe conversions with type defaults |
| `ToSafeBoolean/Date/Decimal/Double/Integer/String` | Exception-safe conversions with optional default |
| `ShellSort1DArray` / `ShellSort2DArray` | In-place Shell sort of 1D or 2D arrays |
| `BinarySearch` | Binary search on sorted 1D array |
| `GetSystemName` / `GetNTUsername` / `GetWTSSessionID` | OS/user/session info via Win32 APIs |
| `GetUsersFromLDAP` | Queries LDAP path for user objects |
| `ConvertHTMLToPDF` / `ConvertHTMLToTxt` | Delegates to `msfilter.dll` / `MSPeelerMain` |
| `ConvertCurrencyStringToValue` | Removes currency symbols; parses to Decimal |
| `IsValidEmail` | Regex email validation |
| `IsStrongPassword` | Delegates to `bSIROptions.Business` password validation |
| `GetUniqueID` | Returns `Guid.NewGuid().ToString()` |
| `CreateManualWriter` | Manually initialises Enterprise Library `LogWriter` |
| `PMStartOfWeek` | Returns Monday of week for a given date |
| `ConvertWildCardsForSQL` | Converts `*`/`?` to `%`/`_` for SQL LIKE |
| `DeleteFolderAll` | Recursively deletes all files and sub-folders |
| `GetDocumentLibrary` | Retrieves document library path for a party |

**Deps:** `gPMConstants`, `gPMMaths`, `Microsoft.Practices.EnterpriseLibrary.Logging`, `System.DirectoryServices`, Win32 APIs (advapi32, kernel32, mpr, Wtsapi32)

---

### gPMMaths

**File:** `gPMLibrary\gPMMaths.vb` | **Module:** `gPMMaths` | **ProgId:** `gPMMaths_NET.gPMMaths`

Precision arithmetic for insurance calculations. All methods use `Decimal` to avoid floating-point errors. 8 public methods:

| Method | Description |
|--------|-------------|
| `PMTruncateVDecimal(value, eDP)` | String-based truncation to 0–6 decimal places for variable-decimal fields |
| `PMRoundupValueVDecimal(value, eDP, eFactor)` | Applies rounding correction factor then truncates |
| `PMCalcPercentValueVDecimal(total, pct, eDP, eFactor)` | `(total × pct / 100)` to 8dp then rounds |
| `PMTruncateCurrency(value, eDP)` | String-based truncation to 0–4 decimal places for currency |
| `PMRoundupValueCurrency(value, eDP, eFactor)` | Currency rounding with correction factor |
| `PMCalcPercentValueCurrency(total, pct, eDP, eFactor)` | Percentage of currency value |
| `Ceiling(expr)` | SQL-conformant `CEILING` — smallest integer ≥ expr |
| `Floor(expr)` | SQL-conformant `FLOOR` — largest integer ≤ expr |

**Deps:** `gPMConstants` (enums `PMEVDecimalNoOfDP`, `PMECurrencyNoOfDP`, `PMERoundupFactor`)

---

### gPMFunctions64

**File:** `gPMLibrary\gPMFunctions64.vb` | **Module:** `gPMRegistryFunctionsWOW6432`

WOW64 registry access for 32-bit processes reading from both 64-bit and 32-bit hives. Enum `RegSAM` defines access mask flags including `WOW64_32Key=0x200` and `WOW64_64Key=0x100`. Three methods: `GetRegKey64(hive, keyName, propName)` (64-bit hive), `GetRegKey32(hive, keyName, propName)` (32-bit hive), and the core overload that takes explicit `RegSAM` access flags. Uses P/Invoke `RegOpenKeyEx` / `RegQueryValueEx` directly. **Deps:** `gPMRegConst`

---

### gPMRegConst

**File:** `gPMLibrary\gPMRegConst.vb` | No ProgId

Two public `UIntPtr` module-level variables used as Win32 registry root handles:
- `HKEY_LOCAL_MACHINE = New UIntPtr(&H80000002UI)`
- `HKEY_CURRENT_USER = New UIntPtr(&H80000001UI)`

No methods. Used exclusively by `gPMFunctions64.vb`.

---

### gSIRLibrary

**File:** `gSIRLibrary\gSIRLibrary.vb` | **Module:** `gSIRLibrary` | **ProgId:** `gSIRLibrary_NET.gSIRLibrary`

Master constants/enum module for the Sirius For Insurance domain layer. Complements `gPMConstants` with insurance-specific constants. No executable methods.

**Key enumerations:**

| Enum | Key Members |
|------|-------------|
| `SIREDocumentStatus` | `SIRReady=1, SIRLocalPrint=2, SIRCollated=3, SIRBulkPrint=4, SIRDeleted=5, SIRFailed=99` |
| `SIREAccumulationArrayColPos` | `SIRAccumID=0` through `SIRAccumStatus=6` |
| `SIRECommissionBandArrayColPosition` | `SIRCommissionBand=0` through `SIRCommissionBandID=4` |
| `SIRETaxBandArrayColPosition` | `SIRTaxBand=0` through `SIRTaxValueID=7` |
| `SIRERILinesArrayColPosition` | `SIRRITreatyCode=0` through `SIRRIClaimRIMethod=15` |
| `SIRERIFACArrayColPosition` | `SIRFacPartyShortName=0` through `SIRFacPremiumPercent=11` |
| `SIREArcFileTypes` | `Quote=0, InitialLive=1, MTAQuote=2, MTALive=3` |

**Key constant groups:** Ledger type codes (`g_ksNominalLedgerTypeCode="G"`, Debtor/Creditor/Insurer), Orion ledger short names (`SIRACTSalesLedgerShortName="S"`), 27 GL codes (`SIRACTGLCodeIncome="INCOME"`, `SIRACTGLCodeWrittenPremium="PREMIUMIN"`), party type codes, insurance file statuses (POLICY/QUOTE/RENEWAL/MTAQUOTE/CANCEL), renewal statuses, transaction code strings (NB/AP/RP/RN/CO/CP etc.), document type IDs/codes, 40+ lookup table name strings, event/notes type codes, process codes, date boundaries (`SIRSystemLowDate=#1/1/1900#`).

**Public variable:** `SIROPTMasterOptions(5, SIROPTMasterOptionsLastEntry) As Object` — 2D array of live hidden option values (populated at runtime). **Deps:** `gPMConstants`

---

### gACTLibrary

**File:** `gACTLibrary\gACTLibrary.vb` | **Module:** `gACTLibrary` | **ProgId:** `gACTLibrary_NET.gACTLibrary`

Central constants/enum repository for the Orion accounting sub-system. No methods.

**Key enumerations:**

| Enum | Members |
|------|---------|
| `ACTEAccountSign` | `acteSignDebit=1, acteSignCredit=-1` |
| `eApprove_TransDetail` | `TransdetailID, DocumentId, DocumentRef, SourceID, Amount, CurrencyAmount, OSAmount, AuditSetId, AccountID, CurrencyID, CurrencyBaseXrate` |
| `eCashListItem` | `CashlistitemID, AllocationstatusID, MediaTypeID, CashlistID, AccountID, MediaRef, OurRef, TheirRef, Amount, TransdetailID, ContactName` |

**Key constant groups (no methods):**
- **Document types** (1–58): `ACTDocTypeJournal=1, DebitNote=2` through `ACTDocTypeRoundOff=56, ClaimCloneReversal=58`
- **Auto-number ranges**: 58 range codes (JN, SDN, SCN, CCR...)
- **Ledger/posting statuses**: `ACTLedgerTypeGeneral/Debtor/Creditor`, `ACTPostStatusRegistered/Authorised/Posted`
- **Cash list types**: `Payments=1, Receipts=2, ClaimPayments=3`
- **Allocation status**: `Unallocated=1, Posted=2, Allocated=3, Partial=4`
- **Payment type**: `Cheque=1, Cash=2, Electronic=3`
- **Trans detail array**: `k_ACTransDetail_id=0` through `k_ACTransDetailEx_id=16`
- **Solution config**: `ACTOrionSolutionMBP=1, SFORBroking=2, MultiCurrency`

---

### gCLMLibrary

**File:** `gCLMLibrary\gCLMLibrary.vb` | **Module:** `gCLMLibrary` | **ProgId:** `gCLMLibrary_NET.gCLMLibrary`

Claims-domain utility module. **Public variable:** `g_iSourceID As Integer`

| Method | Description |
|--------|-------------|
| `GetEnableClaimVersions() As Boolean` | Reads `SIROPTEnableClaimVersions` (option 35) for current branch. Returns `True` if value = 1. Uses `iPMFunc.getProductOptionValue` |
| `GetMaxVCV(r_sValue)` | Reads `MaxVCV` registry setting (default `"2"`) from `LocalMachine\...\Client` |
| `GetCurrVCV(r_sValue)` | Reads `CurrVCV` registry setting (default `"0"`) |
| `SetCurrVCV(v_sValue)` | Writes `CurrVCV` registry setting back to `LocalMachine\...\Client` |

**Deps:** `gPMConstants`, `gPMFunctions`, `iPMFunc.getProductOptionValue`

---

### gPMComponentServices

**File:** `ComponentServices\gPMComponentServices.vb` | **Module:** `gPMComponentServices` | **ProgId:** `gPMComponentServices_NET.gPMComponentServices`

Factory/helper for creating business components, database connections, and user properties within COM+. `[ThreadStatic] oCache As New Hashtable` for per-thread object caching.

| Method | Description |
|--------|-------------|
| `CheckDatabase(username, sourceID, langID, productFamily, r_bNewInstance, r_oCheckedDb, [vDb])` | Validates existing `dPMDAO.Database` or creates a new one |
| `NewDatabase(×2 overloads)` | Creates new `dPMDAO.Database` and calls `OpenDatabase` |
| `CreateBusinessObject(r_oObject, className, credentials...)` | Late-binds class by name, calls its `Initialise` method |
| `UpdateUserProperty` / `NewUpdateUserProperty` | Stores named user property via Property Manager |
| `GetUserProperty` / `NewGetUserProperty` | Retrieves named user property |

**SPs:** `spu_get_sys_admin_status` (referenced in constant, not directly called). **Deps:** `dPMDAO.Database`, `gPMConstants`, `gPMFunctions`

---

### ListViewItemComparer

**File:** `ControlHelpers\ListViewItemComparer.vb` | **Class:** `ListViewItemComparer` (NotInheritable) | Implements `IComparer`

Type-aware `ListView` column sorter. Constructor overloads: `New()` (col 0, ascending) and `New(column, order)`.

`Compare(x, y)` tries `DateTime.Parse` → `Decimal.Parse` → `Integer.Parse` → `String.Compare`. Reverses sign for descending. Used as `ListView.ListViewItemSorter`.

---
## General / Platform Constants

### GeneralConst

**File:** `Components\GeneralConst.vb` | **Module:** `GeneralConst` | **ProgId:** `GeneralConst_NET.GeneralConst`

Foundation platform constants module. No methods. Key groups:

- **Return codes:** `PMFalse=0, PMTrue=1, PMFail=10, PMError=11, PMOK=20, PMCancel=21`; wizard navigation 400–403; licence errors 600–602; record states 800–821; broker link errors 1001–1006
- **Log levels:** `PMLogFatal=1` through `PMLogDebug4=9`
- **System constants:** `PMProduct="SIRIUS"`, `PMCustomer="AIG"`, poll/timeout/attempt settings
- **Navigator constants:** caption array positions, collection key prefixes, action constants `PMNavAction*`, component type strings
- **Interface constants:** status types (View/Add/Edit/Delete), process mode bitmasks, business type codes, format types `PMFormatString`–`PMFormatLong`
- **Business constants:** lookup column positions, table names, accumulation, commission, tax, reinsurance
- **Database constants:** DSN names, parameter directions, data types `PMString=0` through `PMCode=11`
- **System option numbers:** `kSystemOptionDocumentArchive=10` through `kSystemOptionCCMWebServiceURL=5164`, CCM 5165–5181, QAS=13, SharePoint=5086

---

### GeneralFunc

**File:** `Components\GeneralFunc.vb` | **Module:** `GeneralFunc` | **ProgId:** `GeneralFunc_NET.GeneralFunc`

| Method | Description |
|--------|-------------|
| `PMRoundUp(value, places)` | Rounds up to n decimal places (0–8) |
| `PMStartOfWeek(date)` | Returns Monday of week |
| `PMTruncate(value, places)` | Truncates (floors) to n decimal places |
| `FormatField(formatType, value)` | Formats by `PMFormat*` type (ProperCase/Upper/Date/Time/Currency/Integer/Boolean/Percent) |
| `UnFormatField(formatTypeIn, dataTypeOut, value)` | Reverses formatting, returns as PM data type |
| `GetRegSettings` / `GetRegAllSettings` / `SaveRegSettings` / `DeleteRegSettings` | Registry CRUD via `Interaction.GetSetting` etc. |
| `Encrypt(password)` | Custom 57-char substitution cipher; returns 2 chars longer than input |

**Win32:** `GetComputerName`. **Deps:** `GeneralConst`, `iGeneralFunc`

---

### PMConst

**File:** `Components\PMConst.vb` | **Module:** `PMConst` | No ProgId

Master application-wide constants module for the Sirius Polaris Middleware framework. Parallel to `GeneralConst` — both define foundational platform constants. No methods.

Key groups: all return codes (PMFalse/PMTrue/PMFail/PMError/PMOK/PMCancel through navigator 700-series, database 800-series), log levels, system/product strings (`PMProduct="SIRIUS"`, `PMCustomer="AIG"`), Navigator key names (`PMKeyName*` — 30+ string constants), navigator process codes/actions/collection key prefixes, process mode bit flags, type of business codes, status values (PMView/Add/Edit/Delete), formatting constants (PMFormatString=0 through PMFormatDecimal=21), control type constants, business action values, mandatory values, variable data array positions (PMVarRecordID=0 through PMVarValue=11), lookup constants, transaction type basis codes, database DSN names (`PMSiriusDSN="Sirius"`, `PMOrionDSN="Orion"`, `PMGeminiDSN="Gemini"`), database parameter/type constants.

---

### PMFunc

**File:** `Components\PMFunc.vb` | **Module:** `PMFunc` | **ProgId:** `PMFunc_NET.PMFunc`

Application-wide general utility functions. Parallel to `GeneralFunc` but with additional SP-related utilities.

| Method | Description |
|--------|-------------|
| `PMRoundUp`, `PMStartOfWeek`, `PMTruncate` | Same as GeneralFunc equivalents |
| `UnFormatField` | Reverses format; converts to PM data type |
| `GetRegSettings` / `GetRegAllSettings` / `SaveRegSettings` / `DeleteRegSettings` | VB `Interaction` registry wrappers |
| `Encrypt(sPassword)` | Same 57-char substitution cipher as `GeneralFunc.Encrypt` |
| `GetSystemName(r_sSystemName)` | Calls `GetComputerName` API |
| `FindVarField(sRecordName, sFieldName, vVarDataBlock, r_lPosition)` | Searches 2D variable data array for matching record+field name |
| `GetCommandLine(r_vArgArray, [vMaxArgs])` | Splits `Interaction.Command()` into token array |
| `ConvertWildCardsForSQL(r_sTextString)` | Converts `*` → `%`, appends trailing `%` |

**Win32:** `GetComputerName`. **Deps:** `gPMConstants`, `bPMFunc.LogMessage`

---

### PMAPIFunc

**File:** `Components\PMAPIFunc.vb` | **Module:** `PMAPIFunc` | **ProgId:** `PMAPIFunc_NET.PMAPIFunc`

Win32 API wrapper functions for window management.

| Method | Description |
|--------|-------------|
| `SetTopMostWindow(hWnd, bTopmost)` | Makes window always-on-top or removes flag via `SetWindowPos` |
| `DisableFormCloseButton(hWnd)` | Removes X button via `GetSystemMenu` + `DeleteMenu(SC_CLOSE)` |
| `PMFindWindow(caption, r_bExists, r_hWnd, [className])` | Finds window by caption/class |
| `PMMaximizeWindow(hWnd)` | Maximizes via `ShowWindow(SW_SHOWMAXIMIZED)` |
| `PMGetTempPath(r_sTempPath)` | Gets system temp path via `GetTempPath` API |
| `PMShellWait(commandLine, windowStyle)` | Starts process via `Process.Start`, polls `GetExitCodeProcess` in `DoEvents` loop |

**Win32:** user32, kernel32. **Deps:** `gPMConstants`, `bPMFunc.LogMessage`

---

### PMHelpFunc

**File:** `Components\PMHelpFunc.vb` | **Module:** `PMHelpFunc` | **ProgId:** `PMHelpFunc_NET.PMHelpFunc`

Windows Help file display using the legacy `WinHelp` Win32 API. **Public variable:** `g_sProductFamily As PMEProductFamily`

| Method | Description |
|--------|-------------|
| `ShowHelp(control)` | Opens help search dialog. Reads `HelpFile` registry key via `gPMFunctions.GetPMRegSetting`, calls `WinHelp(hWnd, file, HelpSearch, 0)` |
| `ShowHelp(control, lContextID)` | Opens specific help topic by context ID. Calls `WinHelp(hWnd, file, HelpContext, lContextID)` |

**Win32:** `WinHelp` (User32). **Deps:** `gPMConstants`, `gPMFunctions.GetPMRegSetting`, `iPMFunc.LogMessage`

---

### PMNavKeyConst

**File:** `Components\PMNavKeyConst.vb` | **Module:** `PMNavKeyConst` | **ProgId:** `PMNavKeyConst_NET.PMNavKeyConst`

The definitive Navigator key-name constant registry for the entire platform. All constants are `Public Const String`. No methods.

**Constant groups:** Core navigator (client_key, policy_key, transaction_type), task/workflow (`PMKeyNameTask*`), user/security (`PMKeyNameUser*`), Orion accounting (`ACTKeyName*` — account_id, ledger_id, cashlist_id, allocation_id, document_id), cash-list/receipts, premium finance, claims, party/address, reports (`PMKeyNameParam*` — param_value1–16), renewal, GII navigator process maps (`PMNavProcMapGII*`), SFO process maps (`PMNavProcMapSQ*`), MTA, risk, FSA complaints, misc (nav_step, wm_task, batch_run, keep_window_on_top, MultiCurrencyFlag).

---
## ACT — Orion Accounting Constants & Utilities

### ACTBatchConst

**File:** `Components\ACTBatchConst.vb` | **Module:** `ACTBatchConst` | **ProgId:** `ACTBatchConst_NET.ACTBatchConst`

Pure constants for Orion accounting batch processing array field positions. No methods or dependencies.

| Group | Count | Range | Notes |
|-------|-------|-------|-------|
| Batch Input | 5 | 0–4 | Input document positions |
| Document Array | 8 | 0–7 | Document fields |
| Transaction Detail | 22 | 0–21 | `ACCTTempResultsLastItem=21` |
| Batch Output | 3 | 0–2 | Output positions |
| Import Transaction | 28 | 0–27 | `ACTTransImportArraySize=27` |
| Temp Results | 22 | 0–21 | `ACCTTempResultsLastItem=21` |

---

### ACTCompanyCurrencyConst

**File:** `Components\ACTCompanyCurrencyConst.vb` | **Module:** `CompanyCurrencyConst` ⚠ | No ProgId

9 constants: currency query selectors (`ACTGetCurrenciesInCompany=1` through `ACTGetBaseCurrencies=4`) and currency array positions (`ACTCurrencyId=0` through `ACTToInsert=4`). No methods or dependencies.

---

### ACTConst

**File:** `Components\ACTConst.vb` | **Module:** `ACTConst` | **ProgId:** `ACTConst_NET.ACTConst`

Main Orion accounting global constants. No methods.

**Key constant groups:** 41 document types (`ACTDocTypeJournal=1` through `ACTDocTypeClaimAmend=41`), 4 document type groups, posting statuses (Registered/Authorised/Posted), ledger types (General/Debtor/Creditor), purge frequency, 5 account types, 2 account statuses, 3 batch statuses, media/payment types, allocation statuses, cash list types/statuses, auto-numbering codes (41 range codes), Navigator/solution config constants. Notable: `ACTCredit=-1`, `ACTDebit=1` sign conventions.

---

### ACTExplorerConst

**File:** `Components\ACTExplorerConst.vb` | **Module:** `ACTExplorerConst` | **ProgId:** `ACTExplorerConst_NET.ACTExplorerConst`

Chart of Accounts explorer constants. No methods. 7 error codes (`ACExpErrFirst=0` through `ACExpErrLast=6`), `GetNode`/`GetChildrenOfNode` result columns (13 positions, 0–12), `GetElementRelationships` result columns (4 positions), `GetStructureTree` result columns (5 positions), defaults (`ACDefaultMapType=1`, `ACDefaultCompanyId=1`).

---

### ACTFunc

**File:** `Components\ACTFunc.vb` | **Module:** `ACTFunc` | **ProgId:** `ACTFunc_NET.ACTFunc`

| Method | Description |
|--------|-------------|
| `CompanyBaseCurrency() As Integer` | Returns hardcoded `26` (GBP currency ID) |
| `CurrentCompany() As Integer` | Returns hardcoded `1` |
| `GetSLSuspense() As Integer` | Returns hardcoded `32` (Sales Ledger Suspense account ID) |
| `GetPLSuspense() As Integer` | Returns hardcoded `33` (Purchase Ledger Suspense account ID) |
| `ParseArray(vArray, sString, bArrayToString)` | Bidirectional array ↔ pipe-delimited string converter. `True`=array→string, `False`=string→array |

**Deps:** `gPMConstants`, `iPMFunc.LogMessage`

---

## DOC — Document Management

### DOCConst

**File:** `Components\DOCConst.vb` | **Module:** `DOCConst` | No ProgId

DME/DocuMaster global constants. **Public Structures:** `DOCNodes` (Key, Text), `DOCExCodes` (CabExCode, DrawExCode, FoldExCode). No methods.

**Key constant groups:** View modes (4), node types (2), max lengths (3), 18 document type strings (TIF/Text/RTF/Word/Excel/PPT/Access/HTML/GIF/JPG/Email/PDF/HLP/ZIP/XML/BMP/Unknown), 18 single-char file type DB codes, 5-level folder hierarchy (`DOCFolderLevelBranch=0` through `DOCFolderLevelDocument=5`), 6 splash types, 6 registry sections, 30+ registry keys, 14+ scan option keys, 4 commit statuses (Started/Cancelled/Finished/Locked), 12 history task types.

---

### DOCGeneralFunc

**File:** `Components\DOCGeneralFunc.vb` | **Module:** `DOCGeneralFunc` | No ProgId

DME utility functions. **Public constants:** `REGISTRY_USER=1`, `REGISTRY_SYSTEM=2`. **Public struct:** `Setting_Type`. **Win32:** `SHBrowseForFolder`, `SHGetPathFromIDList`, `lstrcat`, `GetTempPath`, `RemoveDirectory`.

18 public methods include: `GetDOCRegSettings`, `CopyFile`, `RmDirectory`, `KillFile`, `KillDir`, `BrowseFolder`, `ChangeDOCRegSettings`, `MakePath` (cloud-hosting aware), `GetFileName`, `StripSlashes`, `ExtractNumFromKey`, `LogMessageScan`, `CacheFile` (S3 aware), `GetDMEDIR`, `GetRegistryValue`, `SetRegistryValue`, `SetComboText`, `IsSiriusInstalled`, `SetCacheStatus`, `ZipCheck`, `GetUniqueName`, `DeleteFile`.

**Deps:** `gPMFunctions`, `gPMConstants`, `bPMFunc`, `bObjectManager.ObjectManager`, `bDOCCommitServer.Commit`, `Sspi.Common.Aws.S3`

---

### DOCPrinterFunc

**File:** `Components\DOCPrinterFunc.vb` | **Module:** `PrinterFunc` ⚠ | No ProgId

Word/Excel/RTF document printing via COM. **Win32:** `SendMessage` (EM_FORMATRANGE), `GetDeviceCaps`, `FindWindow`, `IsWindow`. **Properties:** `sGetFileType` (R/W), `sGetFileCacheName` (R/W).

| Method | Description |
|--------|-------------|
| `PrintRTF(filePath, printerName)` | RTF printing via Win32 EM_FORMATRANGE pagination |
| `PrintDocumentSilent(filePath, printerName)` | Word COM silent print (no UI) |
| `ShutItDown()` | Closes the Word application |
| `eXcelPrint(filePath, printerName)` | Iterates all Excel worksheets to print |
| `GetFileExtension4Excl(ext)` | Maps file extension to canonical type string |

**Deps:** `gPMConstants`, `iPMFunc`, `gPMFunctions.PrinterHelper.Printer`, `Word.Application`, `Excel.Application`

---

## GEM — Gemini Platform

### GeminiConst

**File:** `Components\GeminiConst.vb` | **Module:** `GeminiConstants` | No ProgId

Massive Gemini platform constants module. No methods. **Public mutable fields:** `GEMPromptButtonForeColour As Color`, `GEMPromptButtonBackColour As Color`.

**Key constant groups:** Polaris data types (0–9), quotation return codes (`GemQuoteTrue=1` through `GemQuoteCalculationEnded=7`), screen/control/DB/user-field array columns, PM key names (~30), process/roadmap IDs by business type (CP 1–121, Renewal 5001–5040, MV 4001–7002), process code strings (~50), document types, policy status codes (GEMPolicyIncomplete=1 through GEMPolicyMTAReinstatement=1050), renewal/cancellation status codes, scheme array columns (11), quote breakdown array columns (10), authorisation data columns (9), GetAllSchemeDetails array columns (19), find client/policy mode constants (8).

---

### GeminiFunc

**File:** `Components\GeminiFunc.vb` | **Module:** `GeminiFunctions` | No ProgId

| Method | Description |
|--------|-------------|
| `ParseTag(tag)` | Parses Gemini screen control tag format |
| `GEMRegSettings(r_settings)` | Reads Polaris config from registry |
| `GEMApostrophes(r_sString)` | SQL apostrophe doubling in-place |
| `GEMApostrophesIn(r_sString)` | Reverse apostrophe processing |
| `BubbleSortArray(vArray, [descending])` | Bubble-sorts 2D Object array on first column |
| `ShowHelp(...)` | Stub — implementation commented out |
| `CreateTestCase(...)` | Test case helper |

**Deps:** `gPMConstants`, `gPMFunctions.GetRegSettings`, `bPMFunc.LogMessage`, `Artinsoft.VB6.Utils`

---

### GeminiNetFunctions

**File:** `Components\GeminiNetFunctions.vb` | **Module:** `GeminiNetFunctions` | No ProgId

| Method | Description |
|--------|-------------|
| `GetPrimaryRiskData(dataset, r_sText)` | Builds plain-text risk summary from `cGISDatasetControl` dataset (Motor I4M/standard) |
| `SendInformationEmail(...)` | Late-bound CDONTS stub — sends info email for online Transact transactions |
| `SendErrorEmail(...)` | Sends error notification for Transact failures with XML data attachment |

**Deps:** `GISSharedConstants`, early-bound `cGISDatasetControl`

---

### GEMListCustomConst / GEMListMgrConst / GEMListsConst / GEMListUserConst

Pure constants modules for Gemini list management. No methods or dependencies.

| File | Module | Constants |
|------|--------|-----------|
| `GEMListCustomConst.vb` | `ListCustomConst` | 8: ACListCustomFieldArraySize=6, ListCustomID, PositionID, ValueID, Text, AbiCode, Command, PropertyID |
| `GEMListMgrConst.vb` | `GEMListMgrConst` | 14: list item positions (LSTString=0 through LSTMax=8), list types (LSTTypeCustom=0/User=1), change flags |
| `GEMListsConst.vb` | `ListsConst` | 3: ACListsFieldArraySize=2, ACLists_ListID=0, ACLists_PropertyID=1, ACLists_Description=2 |
| `GEMListUserConst.vb` | `ListUserConst` | 4: ACListUserFieldArraySize=3, ACListUser_ListUserID=0, ACListUser_ListID=1, ACListUser_Text=2, ACListUser_AbiCode=3 |

---

## GII / GIS — Insurance System

### GIIConst

**File:** `Components\GIIConst.vb` | **Module:** `GIIConstants` | No ProgId

Massive Gemini II constants module. No methods. **Win32 declares:** `SetWindowLong`, `GetWindowLong`, `SetForegroundWindow`, `SetFocusAPI`, `SendMessage`, `FindWindow`, `GetSystemMenu`, `DeleteMenu`.

**Key constant groups:** Policy/GII status codes, date/time format strings (GEMShortDate/LongDate/ISODate), business type strings (Motor=GIIM/PC, Household=GIIH/PH/HH, Truck=GIIT/CV), data model codes, Win32 window style constants, QuoteType constants (ACQuoteTypeQuotesOnly=1 through ACQuoteTypeRenPremiumUpdate=40), ~15 form number constants, COBOL field names (ACGCob_*, ACG2_*, ACGS_*, qo-* — many), GIS object/property name shortcuts (ACGIIMQuickQuoteResult, ACGIIMPremiumAnalysis, ACGIIMNotesBreakdown, ACGIIMExcess, ACGIIMEndorsements, ACGIIMReferrals, ACGIIMDeclines, ACGIIMQuoteBinder, ACGIIMQuoteError), PROP_* shortcuts.

---

### GIIFunc

**File:** `Components\GIIFunc.vb` | **Module:** `GIIFunctions` | No ProgId

WinForms UI helper functions for GII screens. **Enum:** `CtlType` (14 values). **Win32:** `SendMessage`, `GetWindowLong`, `SetWindowLong`. **Module field:** `g_oGIS As Object`.

24 public methods include: `SetTreeViewBackground`, `FormShowInTaskBar`, `SetScreenDisplay`, `StoreScreenDisplay`, `GetFormSettings`, `SetFormSettings`, `ValidateDateField`, `ValidateCardDateField`, `ValidateNumericField`, `ValidateListField`, `ValidatePostcodeFormat` (5 UK formats), `HighlightContol`, `GetYesNoValue`, `GetYesNoValueString`, `GetCodeValue` (ABI code lookup via GIS engine), `ControlType` (classifies WinForms control), `CheckMandatoryControl`, `PromptForInput`, `FindMatchingLabel`, `FindOnTab`, `GetFormPositioning`, `SetFormPositioning`, `GetAviPath`, `GetFormState`.

**Deps:** `gPMConstants`, `gPMFunctions`, `iPMFunc`, `GIIConstants`, Artinsoft compat shims

---

### GIIGISConstants / GIIHConstants / GIITConstants

Pure GIS property name string constants. No methods or dependencies.

| File | Module | ProgId | Content |
|------|--------|--------|---------|
| `GIIGISConstants.vb` | `GIIGISConstants` | `GIIGISConstants_NET…` | **Motor** GIS object/property constants: ~20 objects (GIIMGemPolicy, GIIMDriver 40+ fields, GIIMVehicle 40+, GIIMPremiumAnalysis, GIIMPayment_Bank 25+, GIIMQuickQuoteResult 35+, GIIMProposer 30+, GIIMLegacyPolicy 60+, etc.) |
| `GIIHConstants.vb` | `GIIHConstants` | `GIIHConstants_NET…` | **Household** GIS property constants: ~25 objects + 20+ insurer-specific objects (Co-op, Zurich, Royal, NU, Folgate, AXA, L&G, NIG, Zenith, etc.) |
| `GIITConstants.vb` | `GIITConstants` | (none) | **Truck** GIS property constants: GIITGemPolicy, TruckClientDetails, TruckPolicyData, TruckVehicle, TkSecurityDetails, Payment_Bank 35+ fields, MTADetails 30+ fields, Legacy_Policy 30+ insurer fields |

---

### GISDataModelType

**File:** `Components\GISDataModelType.vb` | **Module:** `GISDataModelType` | **ProgId:** `GISDataModelType_NET.GISDataModelType`

Type discriminator constants only. No methods. GIS data model types: `GISDMTypeNotSet=-1, NonDatabase=0, Risk=1, Claim=2, Policy=3, Party=4, Case=5, GISDMTypeLast=5`. Data model codes: `GISDMCodeCommon/Policy/Party`. GIS object types: `GISOTRisk=0` through `GISOTLast=10` (11 values).

---

### GISPromptConstants

**File:** `Components\GISPromptConstants.vb` | **Module:** `GISPromptConstants` | **ProgId:** `GISPromptConstants_NET.GISPromptConstants`

Prompt Premium Finance integration constants only. Business status codes (NB/MTA/MTC/RN), payment methods (DD/DC/CC), card type descriptors (Delta/EuroCard/MasterCard/Switch/Visa/AmEx/Solo), sender IDs (XEL/FTYM/ITS4ME), 10 Prompt return codes (00–22), app-set status `PromptDisabled="50"`, 3 error codes (10500–10502). No methods or dependencies.

---

### GisSchemeConst

**File:** `Components\GisSchemeConst.vb` | **Module:** `GisSchemeConst` | **ProgId:** `GisSchemeConst_NET.GisSchemeConst`

35+ scheme array field positions (`ACGisSchemeFieldArraySize=31`, `ACGisScheme_GisSchemeID=0` through `ACGisScheme_SchemeCountryId=34`), 10 with-insurer list positions, ~50 insurer ABI number Integer constants, 100+ QM reference string constants. No methods or dependencies.

---

### GISSharedConstants

**File:** `Components\GISSharedConstants.vb` | **Module:** `GISSharedConstants` | **ProgId:** `GISSharedConstants_NET.GISSharedConstants`

Central GIS constants module with 2 utility methods.

**Constants:** 15 data types (`GISDataTypeUnknown=0` through `GISDataTypecode=24`), 4 dataset types, NB/MTA/Rename quote types, 50+ registry key names, 50 dataset action codes (`GISDSActionNone=0` through `GISDSActionRenUpdateRenewalControl=50`), object/property/hierarchy/QEM scheme array column positions, 11 Xelector brand codes/names, Polaris event codes, Datacash type codes, XML suffixes.

| Method | Description |
|--------|-------------|
| `GetBrandName(brandCode, r_sBrandName)` | Returns display name for an Xelector brand code |
| `GISToPMDataType(iGISDataType, r_iPMDataType)` | Maps GIS type integer → PM `PMEDataType` enum value |

**Deps:** `gPMConstants`, `gPMFunctions.GetPMRegSetting`, `iPMFunc.LogMessage`

---

### GISSharedPropertyConstants

**File:** `Components\GISSharedPropertyConstants.vb` | **Module:** `GISSharedPropertyConstants` | **ProgId:** `GISSharedPropertyConstants_NET.GISSharedPropertyConstants`

Specials type index constants (`ACOSpecialNone=0` through `ACOCaseClaimList=20`), Swift display mode bit flags (1/2/4/8), GIS edit flags (`GISDSEditNone=0/Mandatory=1/ReadOnly=2/NoDBColumn=4`), `cProperty_SequenceId="sequence_id"`. No methods or dependencies.

---

### gHUBSpokeConstants

**File:** `Components\gHUBSpokeConstants.vb` | **Module:** `gHUBSpokeConstants` | **ProgId:** `gHUBSpokeConstants_NET.gHUBSpokeConstants`

Hub-spoke batch architecture constants only. **Interface codes (21):** RECURRING, ONEOFF, REJECTIONS, CLOSEBATCH, AUTOBANK, 3RDPARTYCOLLECT, EXTRACTTRANS, LOYALTYEXPORT, ELECRECEIPT, PAYMENT_RUN, CREDITCONTROL, BANK_STMT, SWEEP_BALANCES, STALE_CHEQUES, CHEQUE_REMINDER, CREDIT_BALANCE, CREDITCARD, INST_GEN, INST_STMT, GLEXPORT, CHASECYCLE. Also: batch/record status codes, message strings, 8 volume testing codes, export/import detail array column indices. No methods or dependencies.

---

### gPMGetRuleFileLocation

**File:** `Components\gPMGetRuleFileLocation.vb` | **Module:** `gPMGetRuleFileLocation` | **ProgId:** `gPMGetRuleFileLocation_NET.gPMGetRuleFileLocation`

One public method: `GetRuleFileLocation(v_sGisBusinessTypeCode, v_sDataModelCode) As String` — reads registry key `ACOIMGISSubKey\[BusinessTypeCode]\[DataModelCode]\RulePath`; returns path or empty string on failure. **Deps:** `gPMFunctions.GetPMRegSetting`, `GISSharedConstants.ACOIMGISSubKey`, `bPMFunc.LogMessage`

---

### gSIRPFConst

**File:** `Components\gSIRPFConst.vb` | **Module:** `gPFConst` ⚠ | No ProgId

Premium Finance BACS/DDM transaction processing constants only. **PFTransaction types:** Create/Cancel/First/Ongoing/Represent/Last/Deposit = 1–7. **Instalment filter codes:** None/DDMControl/UnpaidOnly/PaidOnly = 0–3. **Batch selection codes:** GetForExportList/PostingList/RecallList/ExportAction/PostingAction/RecallAction = 1–6. **Batch mark codes:** AsExported/AsPosted = 1–2. No methods. Note: status constants were moved to `bSIRPremFinConst`.

---

### iGISSharedConstants

**File:** `Components\iGISSharedConstants.vb` | **Module:** `iGISSharedConstants` | **ProgId:** `iGISSharedConstants_NET.iGISSharedConstants`

Interface-layer version of `GISSharedConstants` with additional constants and 8 methods.

**Extra constants over GISSharedConstants:** `GISRegStateFilesPathAlternative`, `GISRegLoadSaveDBModeFastWithQuotesXML=4`, extra object/property array positions (ColIsSelectScreen, ColIsNonGIS, ColEditFlags, ColPolarisPropId, ColSpecialsType, ColSpecialsTypeReference, ColListId), Datacash address constants (HouseNo/Street/Town/County/Postcode=0–4), `GISQEMSchArraySize=18`, `GISPolicylBinderOIOffset=500000`.

| Method | Description |
|--------|-------------|
| `GetBrandName` / `GISToPMDataType` | Same as `GISSharedConstants` equivalents |
| `GetDataSetFileNames(...)` | Returns file names for all GIS datasets |
| `GetDefaultsFileName(...)` | Returns defaults file name |
| `GetSaveXSLFileName(...)` | Returns save XSL file name |
| `GetSetIDXSLFileName(...)` | Returns set-ID XSL file name |
| `GetDataSetsPath(...)` | Resolves registry path with fallback |
| `GetLoadSPPath(...)` | Returns load stored procedure path |

**Deps:** `gPMConstants`, `gPMFunctions.GetPMRegSetting`, `iPMFunc`

---
## i* — Interface Utility Modules

### iACTFunc

**File:** `Components\iACTFunc.vb` | **Module:** `iACTFunc` | **ProgId:** `iACTFunc_NET.iACTFunc`

| Method | Description |
|--------|-------------|
| `SetListIndex(cList, lItemData)` | Scans combo/list to select item by ItemData value |
| `SetChildFormPosition(frmParent, frmChild)` | Cascades child form +600 twips from parent position |
| `VBsprintf(sTarget, sSource, ParamArray vParams())` | C-style sprintf with %s/%c/%d/\n/\r/\t/\\ substitutions |

**Deps:** `gPMConstants`, `iPMFunc.LogMessage`, `Artinsoft.VB6.Utils.ReflectionHelper`

---

### iGeneralFunc

**File:** `Components\iGeneralFunc.vb` | **Module:** `iGeneralFunc` | **ProgId:** `iGeneralFunc_NET.iGeneralFunc`

| Method | Description |
|--------|-------------|
| `LogMessage(iType, sMsg, [optionals])` | Logging wrapper; falls back to popup on failure |
| `SelectText(ctlControl)` | Sets SelStart=0, SelLength=Len (selects all text) |
| `SetMousePointer(iMouseState)` | Reference-counted wait cursor (PMMouseBusy/Normal/Reset) |
| `CenterForm(frmForm)` | Centers form on primary screen |
| `PositionForm(frmForm, lTop, lLeft)` | Sets form position (twip-to-pixel coordinate conversion) |

**Deps:** `GeneralConst`, `Artinsoft.VB6.Utils.ReflectionHelper`

---

### iGIIFunc

**File:** `Components\iGIIFunc.vb` | **Module:** `GIIFunctions` ⚠ (same internal name as `GIIFunc.vb`) | No ProgId

6 public methods mirroring `GIIFunc.vb` but for the `pmePFGeminiII` product family registry entries (`SetTreeViewBackground`, `FormShowInTaskBar`, `SetScreenDisplay`, `StoreScreenDisplay`, `GetFormSettings`, `SetFormSettings`). **Deps:** `gPMConstants`, `gPMFunctions`, `iPMFunc`, Win32 API (SendMessage, GetWindowLong, SetWindowLong)

---

### iGIISchemeFlags

**File:** `Components\iGIISchemeFlags.vb` | **Module:** `SchemeFlags` | No ProgId

14 `SCHEMEFLAG_*` bit-flag constants: FORMS=1, VBS=2, VBS_ONLY=4, FULL_GUARANTEE=8, SECOND_CAR=16, PREMIUM_OVERRIDE=32, COMMISSION_OVERRIDE=64, ADD_ONS=128, QUOTE_GUARANTEE=256, EDI_MTA=512, POLICY_NUMBERS=1024, COVERNOTE_NUMBERS=2048, AUTO_RENEW=4096, PMD=8192.

| Method | Description |
|--------|-------------|
| `DecodeSchemeFlags(v_lSchemeFlags, [12 optional ByRef outputs])` | Bitmask → individual Boolean flags |
| `EncodeSchemeFlags(12 input params, r_lSchemeTypeFlags ByRef)` | Individual flags → bitmask integer |

**Deps:** `gPMConstants`, `iPMFunc.LogMessage`

---

### InsuranceFileConst

**File:** `Components\InsuranceFileConst.vb` | **Module:** `InsuranceFileConst` | **ProgId:** `InsuranceFileConst_NET.InsuranceFileConst`

138-field insurance file array positions (`ACFieldArraySize=137`, `ACInsuranceFileCnt=0` through `ACOriginalInsuranceFileTypeId=137`). Covers all policy fields: status, dates, parties, financial amounts, IPT/VAT, payment method, discount, FSA compliance, premium finance, correspondence, media type, IBAN/BIC. Policy Client sub-array: 10 positions (`ACPolicyClientPartyCnt=0` through `ACPolicyClientCorrespondenceDtls=9`). No methods or dependencies.

---

### IPTConsts

**File:** `Components\IPTConsts.vb` | **Module:** `IPTConsts` | **ProgId:** `IPTConsts_NET.IPTConsts`

11 Insurance Premium Tax constants: exempt postcodes (CI/IM/GY/JE), result flags (`PMExemptArea=1, PMZeroRated=2, PMNoRiskRecord=3`), lookup types (`PMIPTTableLookup=0, PMIPTExtrasLookup=1`), `PMReverseCalculation="R"`, `PMIPTThreshDate=1`. No methods or dependencies.

---

### iPMBListEvents

**File:** `Components\iPMBListEvents.vb` | **Module:** `iPMBListEvents` | **ProgId:** `iPMBListEvents_NET.iPMBListEvents`

One public method: `ShowEvents(v_lPartyCnt, [12 optional context params])` — creates event browser component via `bObjectManager.ObjectManager`, sets all context properties (FolderCnt, FileCnt, ClaimCnt, InsuranceRef, ClaimRef, TransactionType, AccountKey, SearchOnPartyCnt, SourceApp, BaseClaimId, CaseID, CaseNumber), calls `.Start()`. Special case for IACTFINDTRANSACTION source. **Deps:** `bObjectManager.ObjectManager`, `iPMBListEvents.Interface_Renamed`, `gPMConstants`, `iPMFunc.LogMessage`

---

### iPMBToolbarFunc

**File:** `Components\iPMBToolbarFunc.vb` | **Module:** `PMBToolbarFunc` ⚠ | No ProgId

**8 button constants:** `ACIButtonFinancial=1, Commission=2, Separator1=3, Notes=4, Letter=5, CD=6` (and key string constants FINANCIAL/POLICY/CLAIM/COMMISSION/NOTE/LETTER/EMAIL/WEB).

| Method | Description |
|--------|-------------|
| `ProcessToolbar(v_sButtonKey, [v_lPartyCnt], [v_ProductName])` | Main dispatcher by button key string |
| `CallFindPolicy(...)` | Gets `bObjectManager` component, launches Find Policy |
| `CallFindClaim(...)` | Launches Find Claim component |
| `CallFindAccount(...)` | Launches Find Account component |
| `CallNotes(...)` | Gets `iPMBFreeFormText`, launches notes |
| `CallLetter(...)` | Gets template via `GetTheTemplate` then `UseDocTemplate` |
| `CallEMail(...)` | Launches email component |

**Deps:** `bObjectManager.ObjectManager`, 7 Interface_Renamed components, `gSIRLibrary`, `gPMConstants`, `iPMFunc.LogMessage`

---

### iPMForms

**File:** `Components\iPMForms.vb` | **Module:** `iPMForms` | **ProgId:** `iPMForms_NET.iPMForms`

**Module-level:** `g_iLanguageID As Integer`.

| Method | Description |
|--------|-------------|
| `GetFieldFormat(cnt)` | Returns format code for a field by index |
| `GetFieldType(lFormat)` | Returns PM control type for a format code |
| `GetCaptionID(v_sTag)` | Returns caption resource ID from control tag |
| `GetMandatory(cnt)` | Returns mandatory status for a field |
| `LocateTag(v_sTag, v_sToken, [sValue])` | Finds tag token position/value |
| `SetFieldValidation(frmSource, r_oFormfields)` | Walks all controls, registers each with `iPMFormControl.FormFields` |
| `DisplayCaptions(frmSource)` / `DisplayCaptions(frmSource, bResFile)` | Loads resource captions onto form controls |
| `DisplayMsgBox(r_lTitleId, r_lMessageId, r_lOptions, ParamArray tokens)` | Shows resource-filed message box |
| `ReplacePlaceHolders(r_sMessage, ParamArray tokens)` | Replaces `{0}..{n}` tokens in a message string |

**Deps:** `iPMFunc.GetResData`, `iPMFormControl.FormFields`, `gPMConstants`, `Artinsoft.VB6.Gui`

---

### iPMFunc

**File:** `Components\iPMFunc.vb` | **Module:** `iPMFunc` | **ProgId:** `iPMFunc_NET.iPMFunc`

THE central interface utility module. Win32 public declarations: `FindWindow`, `SetWindowPos`, `SetFocusAPI`, `SetForegroundWindow`. Public constants: `SWP_NOMOVE`, `SWP_NOSIZE`, `HWND_TOPMOST/TOP/NOTOPMOST/BOTTOM`.

Key methods (~35 public): `SetWindowPlacement`, `LogMessage` (×15 overloads via `iPMMessage.PMMessageV2`), `LogExcepMessage`, `SelectText` (×2), `SetMousePointer`, `CenterForm`, `PositionForm`, `ForceLostFocus`, `GetResData` (×2 — reads resources by language ID), `Encrypt` (BCrypt hash), `LicenceEncrypt` (custom cipher), `GetCommandLine`, `ShowFormInTaskBar`, `ShellSort`, `ShellSortDistinct`, `CascadeForm`, `RunDocumaster`, `retrieveProductOptions`, `RetrieveSingleSystemOption`, `GetSystemOption`, `CreateUserControl` (adds late-bound control to form), `IsIn`, `IsInIDE`, `ForceForegroundWindow`, `getProductOptionValue`, `getUnderwritingOrAgency`, `getUnderwritingType`, `GetSystemSecurityModel`, `AutoSizeDropDownComboWidth`, `SetComboBoxValue`, `GetOptionValue`, `SSTabMoveNext`, `SSTabMovePrevious`, `GetGUID`, `GetTempPath`, `Set_System_Default_Printer`.

**Deps:** `iPMMessage.PMMessageV2`, `bObjectManager.ObjectManager`, `bSIRProductOptions.Business`, `bSIROptions.Business`, `iDOCManager.Interface_Renamed`, `gPMConstants`, `gPMFunctions`, `BCrypt.Net.BCrypt`, `Artinsoft.VB6.*`, `My.Resources.ResourceManager`

---

### iPMListView6Func

**File:** `Components\iPMListView6Func.vb` | **Module:** `ListView6Func` | No ProgId

**Enum:** `SIRListViewMoveItemEnum` (MoveNext/Previous/First/Last/ToIndex/BySteps). **Win32 (private):** `LockWindowUpdate`.

| Method | Description |
|--------|-------------|
| `ListViewAutoSize(listView)` | Auto-sizes all columns |
| `ListViewSortByDate(listView, col)` | Sorts by date column |
| `ListViewSortByCheck(listView, col)` | Sorts by checkbox column |
| `ListViewSortByValue(listView, col)` | Sorts by numeric value column |
| `ListViewSortByStringValue(listView, col)` | Sorts by string column |
| `ListViewBatchStart(listView)` | Begins batch update (LockWindowUpdate) |
| `ListViewBatchEnd(listView)` | Ends batch update |
| `ListViewMoveItem(listView, item, toEnum, [toIndex])` | Removes item and reinserts at new position, copying subitems |

**Deps:** `iPMFunc.LogMessage`, `gPMConstants`, `Artinsoft.VB6.Gui.ListViewHelper`

---

### iPMListViewFunc

**File:** `Components\iPMListViewFunc.vb` | **Module:** `ListViewFunc` | No ProgId

**Win32 (DllImport):** `GetScrollPos`, `SendMessage`, `LockWindowUpdate`. **Private constants:** LVM_* ListView messages, LVS_EX_* extended style flags.

14 public methods: `GetCheck`, `SetExtraListViewProperties` (10 LVS_EX_* flags), `ListViewAutoSize` (VB5 twips-based), `ListView6Autosize` (V6 LVSCW_AUTOSIZE), `ListViewSortByDate`, `ListViewSortByValue`, `ListViewSortByStringValue`, `ListViewBatchStart`, `ListViewBatchEnd`, `ListViewSort`, `ListViewSortByCurrencyValue` (uses `gPMFunctions.ConvertCurrencyStringToValue`), `SetListViewLedger` (alternating row colours via PictureBox background), `SortListView` (ColumnClick handler with toggle + `ListViewItemComparer`).

**Deps:** `iPMFunc`, `gPMConstants`, `gPMFunctions.ConvertCurrencyStringToValue`, `ListViewItemComparer`, `Artinsoft.VB6.Gui.ListViewHelper`

---

### iPMValidate

**File:** `Components\iPMValidate.vb` | **Module:** `iPMValidate` | **ProgId:** `iPMValidate_NET.iPMValidate`

| Method | Description |
|--------|-------------|
| `CheckDateGotFocus(ctl)` | Reformats control value to short date on focus |
| `CheckDateLostFocus(ctl)` | Reformats to long date on blur; shows error + refocuses if invalid |
| `CheckTimeGotFocus(ctl)` | Reformats to short time on focus |
| `CheckTimeLostFocus(ctl)` | Reformats to long time on blur; shows error + refocuses if invalid |
| `CheckIntegerLostFocus(ctl)` | Validates integer on blur; shows error + refocuses if invalid |
| `IsChar(sCharacter) As Integer` | Returns PMTrue if alphanumeric (ASCII 48–57/65–90/97–122) |

**Deps:** `gPMFunctions.FormatField`, `gPMConstants`, `iPMFunc.LogMessage`

---

### iSIRPremFinConst

**File:** `Components\iSIRPremFinConst.vb` | **Module:** `iSIRPremFinConst` | **ProgId:** `iSIRPremFinConst_NET.iSIRPremFinConst`

Interface-level shared state for the Premium Finance component. Holds ~25 public module-level variables (m_vResultArray, m_sProductName, m_sTransactionType, m_vProductClassCodes, m_vBusinessArray, m_vSchemeArray, g_vPlanSelectionArray, m_vRowSelectArray, g_bComplete, g_bCancel, oPMPremFinance, oObjectManager, g_oParent, etc.) plus `g_cInsurerType As Integer = 7`.

| Method | Description |
|--------|-------------|
| `GetProductClass(sProductClass) As String` | Maps single-char code to description (N=All NB, C=Commercial NB, P=Personal NB, R=All Renewal, M=Commercial Renewal, etc.) |
| `GetStatusText(sStatusInd) As String` | Converts PF status indicator to description via `bSIRPremFinConst` constants |
| `GetInstalmentStatus(sStatus) As String` | Converts instalment status integer to description |

**Deps:** `bSIRPremFinConst`, `iPMFunc.LogMessage`, `gPMConstants`

---

### iSIRToolbarFunc

**File:** `Components\iSIRToolbarFunc.vb` | **Module:** `SIRToolbarFunc` | No ProgId

**Constants:** `ACIButtonFinancial=1, Commission=2, Separator1=3, Notes=4, Letter=5, CD=6`. **Variables:** `g_oObjectManager As Object`, `g_iUserID As Integer`.

| Method | Description |
|--------|-------------|
| `ProcessToolbar(v_iButton, [v_lPartyCnt], [v_sPartyCode], [v_sPartyName])` | Main dispatcher by button constant |
| `CallFindAccount()` | Gets `iACTFindAccount.Interface_Renamed`, calls `.Start()` |
| `CallNotes(v_lPartyCnt)` | Gets `iPMBFreeFormText`, sets context, calls `.Start()` |
| `CallLetter(v_lPartyCnt, [InsuranceFolderCnt], [FileCnt], [ClaimCnt])` | Gets template, then calls `UseDocTemplate` |
| `CallCashDeposit(v_lPartyCnt, v_sPartyCode, v_sPartyName)` | Gets `iSIRCashDeposit.Interface_Renamed`, calls `.Start()` |

Private: `CallAgentCommission` (stub), `GetTheTemplate`, `UseDocTemplate`

**Deps:** `bObjectManager.ObjectManager`, `iACTFindAccount.Interface_Renamed`, `iPMBFreeFormText.Interface_Renamed`, `iSIRCashDeposit.Interface_Renamed`, `iPMBFindDocTemplate.Interface_Renamed`, `iPMBDocTemplate.Interface_Renamed`, `gSIRLibrary`, `gPMConstants`, `iPMFunc.LogMessage`

---
## Job / Misc Utilities

### JobConstants

**File:** `Components\JobConstants.vb` | **Module:** `JobConstants` | **ProgId:** `JobConstants_NET.JobConstants`

Pure constants for the Windows Job Schedule API (AT commands). No methods.

**Structures:** `AT_ENUM` (enumerated scheduled job), `AT_INFO` (job add info).

**Enumerations:** `PM_Job_Flags` (JOB_NO_FLAGS=0, PERIODICALLY=1, EXEC_ERROR=2, RUNS_TODAY=4, ADD_CURRENT_DATE=8, NONINTERACTIVE=16), `PM_Job_DaysOfWeek` (Windows AT: Mon=1 through Sun=64), `PM_Sql_Job_DaysOfWeek` (SQL Server Agent: different Sunday position, Sun=1).

**Key constants:** 7 service status codes (STOPPED=1 through PAUSED=7), Win32 error codes, 8 renewal batch job name constants (AC_PreRenSelection, AC_Selection, AC_Combined_Selection, AC_Quote_Insurer, AC_Quote_Broker, AC_Invite, AC_Reminder, AC_Complete, AC_ProcessEDI, AC_Auto_Renewal, AC_Confirm, AC_Lapse, AC_Housekeep), 10 executable name constants (PreRenSel.exe, RenSelection.exe, etc.).

---

### LstVwGdLine

**File:** `Components\LstVwGdLine.vb` | **Module:** `modLst` ⚠ | No ProgId

Win32 API wrapper for enabling ListView gridlines via `comctl32.dll`/`user32.dll`. **Constants:** `LVM_FIRST=&H1000`, `LVM_SETEXTENDEDLISTVIEWSTYLE=LVM_FIRST+54`, `LVM_GETEXTENDEDLISTVIEWSTYLE=LVM_FIRST+55`, `LVS_EX_GRIDLINES=&H1`, `ICC_LISTVIEW_CLASSES=&H1`. **Structure:** `tagINITCOMMONCONTROLSEX`.

| Method | Description |
|--------|-------------|
| `InitComctl32(dwFlags)` | Initialises Common Controls library via `InitCommonControlsEx`; falls back to legacy `InitCommonControls()` |
| `Gridlines(ctrlLstVw)` | Sends `LVM_SETEXTENDEDLISTVIEWSTYLE` (gridlines=1) to the ListView handle |

---

### MSWordFunctions

**File:** `Components\MSWordFunctions.vb` | **Module:** `MSWordFunctions` | No ProgId

Functions for driving a live Microsoft Word application instance. Depends on module-level `g_oCallingApp` (Word Application reference declared elsewhere) and external constants `LoopTag`, `EndLoopTag`, `Separator`.

| Method | Description |
|--------|-------------|
| `MSWordInsertBookmark(sName, sText, sLoop1–4)` | Inserts named bookmark with text at current cursor position; handles loop boundary bookmarks; finds unique name by appending counter |
| `MSWordInsertText(sText)` | Inserts plain text at cursor, advances selection, reactivates Word |
| `MSWordGetBookmarks(r_sBMlist())` | Populates array with all bookmark names from active document |
| `MSWordDeleteBookmark(sName)` | Selects bookmark, deletes text and bookmark object |
| `MSWordGotoBookmark(sName)` | Selects named bookmark and activates Word window |

**Deps:** `Microsoft.Office.Interop.Word`, `gPMConstants`

---

### NavProcConst

**File:** `Components\NavProcConst.vb` | **Module:** `NavProcConst` | **ProgId:** `NavProcConst_NET.NavProcConst`

Pure constants for the Navigator/Process Map framework. No methods.

**Constant groups:** Navigator group keys (Keys/Components/Processes/Maps/Steps/RoadMap), field validation types (Text=0/Numeric=1/Date=2/TextLookUp=3), 16 lookup table indices (`ACLTabPMNav_Map=0` through `ACLNavigatorModes=15`), process map node codes (ACRoot/ACRootProcess/ACRootComponent/ACRootMap/ACProcessMap/ACMapStep/ACStepMap/ACStepComponent), 9 node levels (NodeProcess=1 through NodeStepSubMap=9), key operations (ACSetKey/ACGetKey), SQL script generation constants (ACTAB/ACSQLPush/ACSQLPull/ACSQLMAXFILELENGTH=50000/ACSQLMAXFILES=100).

---

### OrionFuncLink

**File:** `Components\OrionFuncLink.vb` | **Module:** `OrionFuncLink` | No ProgId

Launches Orion accounting components from interface screens. **Constants:** `ACIGotoAccounts=1, ACIGoToTransactionDebit=2, ACIGoToTransactionCredit=3, ACIGotoTransactionCash=4, ACIGotoTransactionFee=5`.

| Method | Description |
|--------|-------------|
| `InitialiseOrionLinkFunc()` | Creates `bObjectManager.ObjectManager`, calls `Initialise(ACApp)` |
| `TerminateOrionLinkFunc()` | Disposes OrionLink, Transactions, Fees, ObjectManager references |
| `ProcessOrionFunc(v_iButton, [v_sShortName], [v_lInsuranceFileCnt], [v_lPartyCnt])` | Dispatches to ShowOutstandingTransactions / ProcessTransactionDebit / Credit / Cash / Fee |

Private methods: `ShowOutstandingTransactions` → `iPMBOrionLink.ShowHistory()`, `ProcessTransactionCash` → `iPMBOrionLink.TransactionCash()`, `ProcessTransactionFee` → `iPMBFeeTransaction.Interface_Renamed` (.Initialise/.Start/.Dispose), `ProcessTransactionDebit/Credit` → `iPMBTransactions.Interface_Renamed`.

**Deps:** `bObjectManager.ObjectManager`, `iPMBOrionLink.Interface_Renamed`, `iPMBTransactions.Interface_Renamed`, `iPMBFeeTransaction.Interface_Renamed`, `SSP.S4I.Interfaces.ILocalInterface`, `iPMFunc.LogMessage`, `gPMConstants`

---

### PartyBuilderHandler

**File:** `Components\PartyBuilderHandler.vb` | **Module:** `PartyBuilderHandler` | **ProgId:** `PartyBuilderHandler_NET.PartyBuilderHandler`

Handles GIS (Party Builder) custom data screens for a `PartyCnt`. **Public variable:** `g_oObjectManager As Object` `[ThreadStatic]`.

| Method | Description |
|--------|-------------|
| `OpenPartyBuilderScreen(iTask, lPartyCnt)` | Gets GIS screen ID via `bSIRParty.Business.GetGISScreenForParty`; checks for data model change (prompts user to reset if changed); calls `ShowAdditionalScreen` |
| `GetPartyBuilderFlags(lPartyCnt, r_bHasModel, r_bHasData)` | Determines whether a GIS model is assigned and whether custom data exists |
| `GetPreviousPartyBuilderDataModel(lPartyCnt, r_lPreviousDataModelId, r_lGISPolicyLinkID)` | Returns previous data model ID and GIS policy link ID |
| `DeleteCustomData(lGISPolicyLinkID)` | Calls `bSIRParty.Business.DeleteCustomData` |

Private: `ShowAdditionalScreen` → `iPMURisk.Interface_Renamed` (.Initialise/.SetProcessModes/.ScreenId/.Start).

**Deps:** `bSIRParty.Business`, `iPMURisk.Interface_Renamed`, `gPMConstants`, `iPMFunc.LogMessage`

---

### PartyFunc

**File:** `Components\PartyFunc.vb` | **Module:** `PartyFunc` | **ProgId:** `PartyFunc_NET.PartyFunc`

Shared utility functions for party/client data entry screens.

**Constants:** `KeyAsciiBackSpace=8`, `ksLoyaltyNumberPrefix="601435"`, `m_kBlankAlternativeIdentifier="000000000000000"` (15 zeros), `kSchemaName="Party History Schema"`, `kSchemaNameSQl="spu_Get_Party_History_Schema"`, `kPartyHistoryDataModelCode="PARTYHISTORYXSD"`.

| Method | Description |
|--------|-------------|
| `GetHiddenOptions(v_lSourceId, [10 optional ByRef outputs])` | Reads up to 10 product options via `iPMFunc.getProductOptionValue` (NRMA, ValidateAlternativeIdentifier, AONAffinity, RestrictedInsurerAccess, FutureDateAddressChanges, MultiTreeAccounting, LimitPersonalClientEditFields, ShareDisclosures, BusinessFieldMandatory, AONPRClientScreenChanges) |
| `SetAddressHeaders(r_oAddresses, sPostCode, sAddressUsage)` | Sets column headers on address `ListView` |
| `GetSubBranchDetails(r_oSubBranch, r_oBranch, r_oBusiness, v_lSubBranchId)` | Clears and re-populates sub-branch ComboBox |
| `AlternativeIdentifierChange(r_oAlternativeIdentifier)` | Maintains 15-char zero-padded format as user types |
| `StopNonNumericCharacters(r_iKeyAscii)` | Cancels keystroke unless digit or backspace |
| `LoyaltyNumberLostFocus(...)` | Validates loyalty number via VBScript; prompts to update risk numbers if changed |
| `ValidateLoyaltyNumber(...)` | Runs VBScript validation via `MSScriptControl.ScriptControl` |
| `AlternativeIdentifierLostFocus(...)` | Validates alt-ID via VBScript |
| `ValidateAlternativeIdentifier(...)` | Runs VBScript validation via `MSScriptControl.ScriptControl` |
| `CreateAndSavePartyHistorySchema(v_oDatabase)` | Calls `spu_Get_Party_History_Schema`, writes result to `<DataSetsPath>\PARTYHISTORYXSD.XSD` |

**SPs:** `spu_Get_Party_History_Schema`

**Deps:** `iPMFunc.getProductOptionValue`, `gPMConstants`, `gPMFunctions`, `bPMFunc.LogMessage`, `MSScriptControl.ScriptControl`, `GISSharedConstants`, `System.IO.StreamWriter`

---

## PB* — Product Builder

### PBDatabaseConsts

**File:** `Components\PBDatabaseConsts.vb` | **Module:** `PBDatabaseConsts` | **ProgId:** `PBDatabaseConsts_NET.PBDatabaseConsts`

Array-index constants for GIS screen metadata stored procedure results. No methods. Referenced SP: `spe_GIS_screen_sel` (as string constant `ACGetAllScreenHeaderSQL`).

**Screen Detail Array (ACD*, 0–25):** GISScreenId, ScreenDetailCnt, GISObjectId, GISPropertyId, IsFrame, TabNumber, Caption, Top, Left, Height, Width, ColumnWidth, PreQuoteRequirement, PostQuoteRequirement, PurchaseRequirement, ParentId, HelpText, DefaultObjectId, DefaultPropertyId, IsValuation, IsRateAndPremium, ChildScreenId, DataModelType, PMFormat, ColumnPosition, TabSetIndex.

**Extra Screen Detail Array (ACDExtra*, 26–35):** GISObjectName, GISTableName, GISPropertyName, GISColumnName, EditFlags, SpecialsType, SpecialsTypeReference, ParentObjectName, ObjectType, IsFormattedText.

**Screen Header Array (ACH*, 0–16):** ScreenId, CaptionId, Code, Description, IsDeleted, EffectiveDate, ParentId, IsMaintainable, GISDataModelId, ScriptDefaults, ScriptDynamicLogic, ScreenType, ScreenHeight, ScreenWidth, EnableCompiledRule, CompiledRuleAssemblyDefaults, CompiledRuleAssemblyValidation.

---

### PBGetAddressFromAddressCnt

**File:** `Components\PBGetAddressFromAddressCnt.vb` | **Module:** `PBGetAddressFromAddressCnt` | **ProgId:** `PBGetAddressFromAddressCnt_NET…`

One public method: `GetAddressFromAddressCnt(r_oDatabase, lAddressCnt, vAddressArray(,))` — executes inline SQL `SELECT a.address_cnt, a.address1–4, a.postal_code, c.description, a.address_id FROM address a, country c WHERE a.country_id=c.country_id AND a.address_cnt=[lAddressCnt]`. Post-load: if `postal_code` equals `address_id`, clears postal_code to `""`. **Deps:** `gPMConstants`, `bPMFunc.LogMessage`

---

### PBObjectAndPropertyConsts

**File:** `Components\PBObjectAndPropertyConsts.vb` | **Module:** `pbObjectAndPropertyConsts` | **ProgId:** `pbObjectAndPropertyConsts_NET…`

Array-index constants for the GIS Object dictionary and GIS Property dictionary. No methods.

**GIS Object Array (ACO*, 0–10):** GISObjectId, GISDataModelId, ObjectName, TableName, MaxInstances, IsQuoteObject, ParentObjectId, PolarisObjectId, IsSelectableForScreen, IsNonGIS, EditFlags.

**GIS Property Array (ACP*, 0–18):** GISPropertyId, GISObjectId, PropertyName, ColumnName, DataType, IsInputProperty, IsIdentifyingProperty, IsPrimaryKey, PolarisPropertyId, IsDeleted, IsSearchProperty, IndexLinkingId, EditFlags, SpecialsType, SpecialsTypeReference, IsInMISExport, IsFormattedText, IsChaseCycleProperty, ISClaim360Display.

---

### PBQuoteTypeEncode

**File:** `Components\PBQuoteTypeEncode.vb` | **Module:** `PBQuoteTypeEncode` | **ProgId:** `PBQuoteTypeEncode_NET.PBQuoteTypeEncode`

**Quote type constants:** `PBCQemQuoteTypeQuote=1, Validate=2, Ual=3, Default=4, Renewal=5, PreScreen=6, CopyRisk=7, RenewalLapse=8`.

| Method | Description |
|--------|-------------|
| `EncodeTransactionScreenAndType(r_lEncoded, r_lTransactionType, r_lGISScreenId, r_lQuoteType)` | Packs into `1TTTSSSSYY` format: `1,000,000,000 + (TxType×1,000,000) + (ScreenId×100) + QuoteType` |
| `decodeTransactionScreenAndType(r_lEncoded, r_lTransactionType, r_lGISScreenId, r_lQuoteType)` | Unpacks both old format (`TTTSSYY`) and new format (`1TTTSSSSYY`) |
| `GetQuoteTypeDesc(v_lQuoteType) As String` | Returns: 1→"Rating", 2→"Validation", 3→"User Authority Limits", 4/6→"Default", 5→"Renewal" |

**Deps:** `gPMConstants`, `bPMFunc.LogMessage`

---

### PBReadScriptColumn

**File:** `Components\PBReadScriptColumn.vb` | No ProgId

**Constants:** `ACNScriptDefaults="script_defaults"`, `ACNScriptDynamicLogic="script_dynamic_logic"`, `ACNScriptQuote="script_quote"`, `ACNScriptUAL="script_UAL"`.

One public method: `ReadScriptColumn(r_oDatabase, v_lScreenId, r_vScreenHeader, iHeaderColumnOffset, v_sColumnName)` — clears parameters, adds `key=v_lScreenId` and `required_column=v_sColumnName` parameters, calls `r_oDatabase.SQLSelectTextField(sSQL:="spu_PB_script_sel", bStoredProcedure:=True)`, writes result into `r_vScreenHeader(iHeaderColumnOffset, 0)`.

**SP:** `spu_PB_script_sel`. **Deps:** `gPMConstants`, `bPMFunc.LogMessage`

---

### PBRiskScreenCommon

**File:** `Components\PBRiskScreenCommon.vb` | **Module:** `PBRiskScreenCommon` | **ProgId:** `PBRiskScreenCommon_NET.PBRiskScreenCommon`

Shared constants, Win32 APIs, and global state for risk screen controls. **Win32 (user32.dll):** `BringWindowToTop`, `SetForegroundWindow`, `SetWindowPos`.

**Key constants:** Non-database control tags (`ndcFreeFormatText=-1, ndcHyperlink=-2, ndcFindControl=-3`), Frame Array positions (`ACF*`, 0–8), Control Array positions (`ACC*`, 0–13), checkbox offsets in twips, format mask constants (`ACFormatStandardMask=&H3F`, `ACFormatCalendarMask=&H40`).

**Public global variables:** `g_sControlName As String` (drag-move reference), `g_lx/g_ly As Integer` (cursor position), `g_iCheckBoxValue As Integer`, `g_vOriginalScreenValues As Object`, `ORIGINAL_VALUE_STR="Original Value: "`.

| Method | Description |
|--------|-------------|
| `SortThreeElementArray(vArray(,), [iMode])` | Bubble-sorts 2D array (3×n) ascending by row 0 |
| `StripColonFromCaption(sCaption)` | Removes trailing `:`, normalises CR+LF to space, collapses double spaces |

---

### PBRiskScreenCommon2

**File:** `Components\PBRiskScreenCommon2.vb` | **Module:** `PBRiskScreenCommon2` | **ProgId:** `PBRiskScreenCommon2_NET.PBRiskScreenCommon2`

Runtime UI engine for Product Builder risk screens (57KB). Handles building, initialising, and populating dynamically generated GIS property controls. **Global var:** `g_bGetEnableClaimVersions As Boolean`. **Constant:** `ACBlankCaption="[BLANK]"`.

| Method | Description |
|--------|-------------|
| `CalculateLinesInCaption(sCaption)` | Counts CR+LF occurrences for label height calculation |
| `simulateTriStateCheckBox(iCheckBoxValue, chkYesNo, [isCheckedStateChanged])` | Cycles Checked/Unchecked/Indeterminate with Yes/No/Unknown text |
| `textLabel_MouseMove(...)` | Screen designer drag-to-move handler with grid snap and clamping |
| `SetInitialControlValues(v_lControlIndex, r_controlArray, r_labelControl, r_EditControl, r_fraFrame, ...)` | **Core 30-parameter method**: decodes tag, populates control array, configures label+edit control, determines mandatory status, maps GIS data type → PM format, registers with FormFields, populates value, applies claim version highlighting, handles [BLANK] captions, applies mandatory styling |
| `addTabControl(r_lTabIndex, r_vTabArray, r_vScreenDetails, lTemp, r_TabStrip)` | Builds a tab entry from screen details row |
| `addFrameControl(r_lFrameIndex, r_vFrameArray, r_vScreenDetails, lTemp, fraFrame)` | Builds a GroupBox frame from screen details row (handles parent-child nesting) |
| `GetTag(lGISObjectId, lGISPropertyId, lTag, r_vDataDictionary, ...)` | Searches data dictionary for matching Object+Property entry; returns column index as tag |
| `IsInputControl(r_oCtrl, v_vDataDictionary, [InputPropertyIndex])` | Returns True if control is bound to an input (editable) property |
| `DisableForm(lDisabled, vForm)` | Enables/disables all TextBox/ComboBox/CheckBox on a form |
| `SetBlankCaption(r_cLabel, bInScreenEdit)` | Hides [BLANK] labels (runtime) or greys them (design-time) |

**Deps:** `PBRiskScreenCommon`, `PBDatabaseConsts`, `PBTabStripCommon`, `GISDataModelType`, `iGISSharedConstants`, `gPMConstants`, `iPMFunc`, `bPMFunc`, `gPMFunctions`, `Artinsoft.VB6.*`

---

### PBTabStripCommon

**File:** `Components\PBTabStripCommon.vb` | **Module:** `PBTabStripCommon` | **ProgId:** `PBTabStripCommon_NET.PBTabStripCommon`

Tab management abstraction layer for `System.Windows.Forms.TabControl`. Tabs are identified by logical index stored as `TabPage.Name="_<index>"`. **Public var:** `ACApp As String` (declared here — this module owns it).

| Method | Description |
|--------|-------------|
| `AddTab(r_TabStrip, v_iIndex, v_sCaption, [uFlatMenuTree])` | Adds TabPage named `_<index>`; duplicate check; insertion maintains sorted order; corrects selection on shift |
| `HideTab(r_TabStrip, v_iIndex, [r_uFlatMenuTree])` | Removes tab by logical index; restores focused tab intelligently |
| `SelectTab(r_TabStrip, v_iIndex, ...)` | Selects tab by logical index by parsing Name; falls back to tab 0 if not visible |
| `IsTabVisible(r_TabStrip, v_iIndex)` | Returns True if tab with given index exists |
| `TabSetCaption(r_TabStrip, v_iIndex, r_vValue)` | Updates tab caption only if different (avoids UI refresh) |
| `TabGetCaption(r_TabStrip, v_iIndex)` | Returns display text of tab at logical index |
| `GetCurrentTab(r_TabStrip)` | Returns logical index + 1 of focused tab |

**Deps:** `iPMFunc.LogMessage`, `gPMConstants`, `System.Windows.Forms.TabControl`

---
## PM* — Platform Modules

### PIRConsts

**File:** `Components\PIRConsts.vb` | **Module:** `PIRConsts` | No ProgId

Constants for the PIRLookup (Insurer Rate) business object. 10 constants: `ACFieldArraySize=9`, `ACPIRRateRate1=0, ACPIRRateValue1=1, ACPIRRateMinTotal1=2, ACPIRRateRate2=3, ACPIRRateValue2=4, ACPIRRateMinTotal2=5, ACPIRRateRate3=6, ACPIRRateValue3=7, ACPIRRateMinTotal3=8, ACPIRRateFound=9`. No methods or dependencies.

---

### PMBConst

**File:** `Components\PMBConst.vb` | **Module:** `PMBConst` | **ProgId:** `PMBConst_NET.PMBConst`

Sirius for Broking general constants. No methods.

**Key constant groups:** 20 party type codes (PMBPartyTypePersonalClient="PC", AGent="AG", CorporateClient="CC", etc.) with matching text constants, prospect types (PMBProspectTypeClient="P"/"C"), 30 event type integers (PMBEventNewClient=1 through PMBEventFSANotes=30), 13 event group constants, document type IDs (ClientTextFile=1 through ClauseTextFile=7), 6 policy types (Swift=1/GIIMotor=2/General=3/GIIHousehold=4/Underwriting=5/GIICV=6), 5 agent types with matching text, lookup list code/text pairs for Gender/Marital Status/Employment/Business/Occupation/Title/Payment Method etc., renewal criteria descriptions, `PMBAutoCancelLapsedCode="CCNTRL"`.

---

### PMBGeneralFunc

**File:** `Components\PMBGeneralFunc.vb` | **Module:** `PMBGeneralFunc` | **ProgId:** `PMBGeneralFunc_NET.PMBGeneralFunc`

General UI/screen utility functions for Sirius Broking forms. **Public variables:** `g_oComponentManager`, `g_oListManager`, `g_lPolicyKey`, `g_bInstanceChanged`, `g_lEdit`. **Enum:** `CtrType` (None/Label/TextBox/ComboBox/Checkbox/YesNoCheck/Command). **Polaris data type constants:** `GEMPolUnknown=0` through `GEMPolRef=9`.

| Method | Description |
|--------|-------------|
| `DoubleCharacter(r_sString, [v_sChar])` | Doubles each occurrence of char (default apostrophe) for SQL escaping |
| `ControlGotFocus(ctl, [vReferenceType])` | Formats date TextBox to short date; fills combo via `FillCombo`/`FillRefCombo` |
| `ControlLostFocus(ctl)` | Formats date to long date; validates combo text against list |
| `FillCombo(cboControl, bRefill)` | Populates ComboBox from `g_oListManager.GetList` by control Tag property |
| `FillRefCombo(cboControl, iRefType)` | Populates ComboBox from `g_oComponentManager.GetRef` by reference type |
| `FieldChange(frm, [vKeyCode], [vShift])` | Sets `g_bInstanceChanged` via `SetChangeFlag`; ignores Alt key modifier |
| `FieldOnControlChange(ctl, ...)` | Sets `g_bInstanceChanged` at control level |
| `ControlType(ctl)` | Returns `CtrType` enum value for a WinForms control |
| `ParseTag(sTag, iPropertyType, lPropertyID, sTable, sField)` | Parses Polaris screen control Tag string (type at char[0], 8-char property ID, table+field) |
| `SetChangeFlag(oForm, bValue)` | Sets `g_bInstanceChanged`; en/disables `cmdApply` button in edit mode |
| `SetControlChangeFlag(oCtl, bValue)` | Sets `g_bInstanceChanged` at control level only |
| `FormatPostCode(v_sInString, r_sOutString)` | Reformats UK postcode (normalize spaces) |
| `CheckValidPostCode(v_sPostCode, [bSpaceRequired])` | Validates UK postcode (normalises letters→X/digits→9, matches 6 patterns) |
| `CheckValidUSZipCode(v_sZipCode, [bSpaceRequired])` | Validates US zip code (5-digit or 9-digit with hyphen) |
| `HighlightContol2(ctl, [optDateField], [optDropDown])` | Selects all on focus; optionally opens ComboBox dropdown |
| `ValidateListField2(ddList)` | Validates list/combo text exists in list; sets correct casing; clears if not found |

**Deps:** `gPMConstants`, `iPMFunc.LogMessage`, `g_oListManager`, `g_oComponentManager`, `Artinsoft.VB6.Utils`

---

### PMCopyFile

**File:** `Components\PMCopyFile.vb` | **Module:** `PMCopyFile` | **ProgId:** `PMCopyFile_NET.PMCopyFile`

Safe Win32-aware file copy with diagnostic error messages. **Win32:** `PathFileExists`, `PathIsUNCServerShare`, `PathIsUNCServer` (all from shlwapi.dll).

One public method: `PMFileCopy(v_sSourceFile, v_sTarget, r_sMessage)` — null-checks source/target; checks source exists (walks UNC path to diagnose share/server accessibility); resolves target (directory vs file, appends source filename if target is dir); checks read/write permissions and `ReadOnly` attribute; calls `File.Copy`. Returns `PMTrue/PMError/PMMNoAccess/PMNotFound` with descriptive `r_sMessage`.

**Deps:** `gPMConstants`, `iPMFunc.LogMessage`, `Artinsoft.VB6.Utils`, `System.IO`

---

## Policy / SIR / Renewal

### PolicyNumberGen

**File:** `Components\PolicyNumberGen.vb` | **Module:** `PolicyNumberGen` | **ProgId:** `PolicyNumberGen_NET.PolicyNumberGen`

Generates GII insurer-specific policy numbers. **Constants:** Insurer ID integers (MMAInsurerNo=21, NUInsurerNo=6, CORNHILLInsurerNo=13, etc.) and corresponding range code strings (MMARangeCode="MMA", NURangeCode="NU", I4MRangeCode="I4M" etc.).

| Method | Description |
|--------|-------------|
| `GetAgencyRef(v_lInsurerNo, v_lSchemeNo, r_sAgencyRef)` | Returns hardcoded agency reference per insurer (MMA→"00470", NU→"2SN200", Cornhill→"6385569", NIG→"1234", others→"AGCYREF") |
| `GenPolicyNum(v_lInsurerNo, v_lSchemeNo, v_dtEffectiveDate, v_sQMMSuffix, r_sPolicyNum)` | Creates AutoNumber component via `SSP.S4I.Interfaces.IBusiness.Initialise`; maps insurer→range code; calls `GenerateNewNumber`; calls `GetAgencyRef` + `FormatPolicyNum` |
| `FormatPolicyNum(v_lInsurerNo, v_lNumber, sPrefix, sSuffix, sAgencyRef, dtEffectiveDate, v_sQMMSuffix, r_sPolicyNum)` | Formats: MMA=AgencyRef+6digits, NU=YY+Month+5digits, NIG/LINK=Prefix+6digits, PEGASUS=Prefix+8digits, I4M/QMM=YYYYMMDD+4digits+QMMSuffix |

**Deps:** `SSP.S4I.Interfaces.IBusiness` (AutoNumber), `GISSharedConstants.LogMessageFile`, `gPMConstants`, `Artinsoft.VB6.Utils.StringsHelper`

---

### QuoteConst

**File:** `Components\QuoteConst.vb` | **Module:** `QuoteConst` | No ProgId

GII quote data-object hierarchy constants only. No methods.

**Constant groups:** Object names (`GIIQD_OBJ_*`): GIIMGEMPOLICY, Quick_Quote_Result, Referrals, Refer_Reasons, Declines, Decline_Reasons, GemQuoteConfiguration, NCD, Cover, Excess_Breakdown, Notes_Breakdown, Endorsements_Breakdown, Premium_Analysis, Vehicle, Driver, Occupation, Policy; Saved-state objects (`GIIQD_OBJ_SAVED_*`): Saved_Quote, Saved_Excess, Saved_Notes, Saved_Analysis, Saved_Add_On; Add-on object (`GIIAO_OBJ_*`): Selected_Add_On; Policy property keys (`GIIQD_PROP_*` ~40 keys: Effective_Start_Date, Premium, Quote_Type, NCD_Discount, etc.); Premium override keys (`GIIPO_PROP_*` ~7); Add-on property keys (`GIIAO_PROP_*` ~4); IPT analysis codes (`GIIPO_IPT_ANALYSIS_CODE="199"`, `OVERRIDE_ANALYSIS_CODE="7998"`); scheme types (`GIISCHEME_TYPE_*`).

---

### SIRConst

**File:** `Components\SIRConst.vb` | **Module:** `SIRConst` | **ProgId:** `SIRConst_NET.SIRConst`

Sirius-wide general constants. No methods.

**Key constant groups:** Party type text/codes (PC/AG/CC/GC), ABI address descriptions/codes for Correspondence and Business addresses, contact type codes (TELEPHONE/E-MAIL/LETTER/FAX/MAIN), Navigator key `SIRNavKeyAgentOnly="agent_only"`, process codes (NBQDIRECT/NBQINDIR/MTA/RENEWAL/RVWQUOTE/RVWPOLICY), insurance file MTA types (MTAQUOTE/MTA PERM/MTA TEMP/MTA INCOMP/CANCEL), email session keys, 18 transaction code strings (NB/AP/RP/RN/CO/CR/CP/CC/CA/CI/LL/LA/CJ/EH/CQ/AF/NT/AS), transaction code IDs (NewBusiness=1 through ClaimPaid=7), document type IDs (Quotation=1 through Cancel=10) and codes (QUO/PRO/DCN/SCH/CER/RNC/CLM/LAP/DEC/CAN), RI treaty array indices (`SIRRITreatyCode=0` through `SIRRIMax=11`), FAC arrangement array indices, FAC summary code, rounding constant `SIR4DPRoundFactor=0.000049`.

---

### SiriusCoreFunc

**File:** `Components\SiriusCoreFunc.vb` | **Module:** `SiriusCoreFunc` | **ProgId:** `SiriusCoreFunc_NET.SiriusCoreFunc`

Core database utility functions for branch lists and validation scripts.

| Method | Description |
|--------|-------------|
| `GetBranches(v_oDatabase, r_vBranchArray(,))` | Executes `spu_PM_SelAll_Source` to get all branches |
| `GetSubBranches(v_oDatabase, v_lSourceID, r_vSubBranchArray(,))` | Executes `spu_sub_branch_sel` with `source_id` parameter |
| `GetNumberValidationScripts(v_sBranchPrefix, r_sLoyaltyScript, r_sAlternativeIdentifierScript)` | Reads scripts folder from registry; loads `Loyalty[_BranchPrefix].txt` and `AlternativeIdentifier[_BranchPrefix].txt`; falls back to non-prefix filename |

**SPs:** `spu_PM_SelAll_Source`, `spu_sub_branch_sel`

**Deps:** `gPMConstants`, `gPMFunctions.GetPMRegSetting`, `bPMFunc.LogMessage`, `Microsoft.VisualBasic.FileSystem`

---

### SIRRenewalConst

**File:** `Components\SIRRenewalConst.vb` | **Module:** `RenewalConst` ⚠ | No ProgId

All renewal-process constants. No methods.

**Key constant groups:** `PMRenewalDefaultSettingId=-1`, renewal status type codes (PRERENSEL/RENSEL/RENQUOTED/INVITED/CONFPEND/LAPSEPEND/RENEWCONF/LAPSECONF/LAPSED/RENEWED/COMPALT), event type codes (RENEWAL/RENPOLCHG), event description strings, suspension levels (None=0/Suspended=1/Stopped=2), motor renewal types (General=" "/Excess="X"/Endorse="E"), 20 renewal process return codes (PMRenewalOkay=0 through PMRenewalComplete=99), household renewal types, ACStatus* mirror constants (PRERENSEL/Selected/Quote/Invite/Confirm/ConfirmPending/LapsePending/LapseConfirmed/Renewed/Lapse/WhatIf/CompAlternate/Incomplete), 40+ renewal action codes (ACRen*: ALTCONF/MAINTCLM/MAINTNCD/RENSELEC etc.), Work Manager keys, insurance file type strings, EDI messages and numeric IDs (ACRenEDIMessage1–9, ACRenEDIMsg1–9=191–199), data model codes (GIIMotor/GIIHouse/GIITruck), business type codes (GIIM/GIIH/GIIT), renewal type integers (Both=0/BrokerLed=1/InsurerLed=2/WhatIf=4/InsurerMode=5), reset flags, status flags, renewal groups (Motor=4006/Household=4007), `PMRenewalLapsedReasonLapsedByInsurer="INSRLAPSED"`.

---

### SSfunc

**File:** `Components\SSfunc.vb` | **Module:** `SSfunc` | **ProgId:** `SSfunc_NET.SSfunc`

One public method: `ShowHelp(dlgHelp, lContextID)` — reads `HelpFile` registry setting via `gPMFunctions.GetPMRegSetting` (`LocalMachine/SiriusSolutions/Client`); uses `ReflectionHelper` to set `HelpFile`, `HelpCommand`, `HelpContext` properties on the control, then invokes `ShowHelp` on it. **Deps:** `gPMFunctions.GetPMRegSetting`, `gPMConstants`, `iPMFunc.LogMessage`, `Microsoft.VisualBasic.Compatibility.VB6.ReflectionHelper`

---

### TopMost

**File:** `Components\TopMost.vb` | **Module:** `TopMost` | **ProgId:** `TopMost_NET.TopMost`

**Constants:** `SWP_NOMOVE=2, SWP_NOSIZE=1, FLAGS=3, HWND_TOPMOST=-1, HWND_NOTOPMOST=-2`. **Win32:** `SetWindowPos` (user32).

| Method | Description |
|--------|-------------|
| `SetTopmost(frmForm) As Boolean` | Calls `SetWindowPos(..., HWND_TOPMOST, ..., FLAGS)`. Returns True on success |
| `ClearTopmost(frmForm) As Boolean` | Calls `SetWindowPos(..., HWND_NOTOPMOST, ..., FLAGS)`. Returns True on success |

---

### Validate

**File:** `Components\Validate.vb` | **Module:** `iValidateFunc` ⚠ | No ProgId

WinForms input-field validation helpers using `GeneralFunc.FormatField`.

| Method | Description |
|--------|-------------|
| `CheckDateGotFocus(ctlControl)` | Reformats to `PMFormatDateShort` on focus |
| `CheckDateLostFocus(ctlControl)` | Reformats to `PMFormatDateLong` on blur; shows "Invalid date" + refocuses if empty |
| `CheckTimeGotFocus(ctlControl)` | Reformats to `PMFormatTimeShort` on focus |
| `CheckTimeLostFocus(ctlControl)` | Reformats to `PMFormatTimeLong` on blur; shows error + refocuses if invalid |
| `CheckIntegerLostFocus(ctlControl)` | Validates integer; shows "Invalid number" + refocuses if invalid |
| `IsChar(sCharacter) As Integer` | Returns PMTrue if alphanumeric (ASCII 48–57 / 65–90 / 97–122) |

**Deps:** `GeneralFunc.FormatField`, `GeneralConst`, `iGeneralFunc.LogMessage`

---

### ADVReg

**File:** `Components\ADVReg.vb` | **Module:** `ADVReg` | **ProgId:** `ADVReg_NET.ADVReg`

Win32 `advapi32` registry wrapper. **Enum:** `InTypes` (ValNull=0, ValString=1, ValXString=2, ValBinary=3, ValDWord=4, ValLink=6, ValMultiString=7, ValResList=8). Uses `RegOpenKey`, `RegQueryValueEx`, `RegEnumValue`, `RegCreateKey`, `RegFlushKey`.

| Method | Description |
|--------|-------------|
| `ReadRegistry(hKey, sSubKey, sValue, r_vData, r_iType)` | Reads a single registry value |
| `ReadRegistryGetSubkey(hKey, sSubKey, r_sResult())` | Enumerates sub-key names |
| `ReadRegistryGetAll(hKey, sSubKey, r_sResult())` | Reads all name/value pairs from a key |
| `WriteRegistry(hKey, sSubKey, sValue, vData, iType)` | Creates or updates a registry value |
| `DeleteSubkey(hKey, sSubKey)` | ⚠ Recursively deletes a sub-key tree |
| `DeleteValue(hKey, sSubKey, sValue)` | ⚠ Deletes a single registry value |

**Deps:** `gPMConstants`, `gPMFunctions`, `Artinsoft.VB6.Utils.StringsHelper`

---

### gArrays

**File:** `Components\gArrays.vb` | **Module:** `gArrays` | **ProgId:** `gArrays_NET.gArrays`

**Constants:** `klColDimension=1`, `klRowDimension=2`.

| Method | Description |
|--------|-------------|
| `IsArrayDimensioned(r_vArray) As Integer` | Returns PMTrue if array is dimensioned (tests `GetLowerBound(0)`) |
| `GetArrayBounds(r_vArray, r_lDimension, r_lLower, r_lUpper) As Boolean` | Returns lower/upper bounds for 1-indexed dimension number |

**Deps:** `gPMConstants`

---
## b* — Business Components

### bACTFunc

**File:** `Components\bACTFunc.vb` | **Module:** `bACTFunc` | **ProgId:** `bACTFunc_NET.bACTFunc`

Accounting lookup helpers bridging Orion account data.

| Method | Description |
|--------|-------------|
| `GetLedgerIDFromShortName(v_oDatabase, v_sLedgerShortName, r_lLedgerID) As Integer` | Executes `spu_ACT_Get_LedgerID_From_ShortName` with ledger_short_name param, returns PMTrue or PMError |
| `GetLedgerID(v_oDatabase, v_sBranchCode, v_sLedgerShortName, r_lLedgerID) As Integer` | Overload with branch_code param; returns ledger_id from `spu_ACT_Get_LedgerID_From_ShortName` |
| `GetSubBranchID(v_oDatabase, v_sBranchCode, v_lSourceID, r_lSubBranchID) As Integer` | Executes `spu_ACT_Get_Sub_Branch_id`; returns sub_branch_id and PMTrue/PMError/PMNotFound |

**SPs:** `spu_ACT_Get_LedgerID_From_ShortName`, `spu_ACT_Get_Sub_Branch_id`

---

### bGEMFunc

**File:** `Components\bGEMFunc.vb` | **Module:** `bGEMFunc` | **ProgId:** `bGEMFunc_NET.bGEMFunc`

Gemini database and business-object resolver using Reflection.

| Method | Description |
|--------|-------------|
| `GetGeminiDatabase(v_lBranchNo, r_oDatabase) As Integer` | Creates DB object via `gPMComponentServices.CheckDatabase`; returns PMTrue/PMError |
| `GetGISDatabase(v_lBranchNo, r_oDatabase) As Integer` | Creates GIS DB using component services; returns PMTrue/PMError |
| `GetGeminiBusiness(v_sBranchCode, r_oBusiness) As Integer` | Uses Reflection to obtain GemBusiness object from the running GII/Gemini process |
| `GetSiriusArchitectureDatabase(v_lBranchNo, r_oDatabase) As Integer` | Creates Sirius Architecture DB object using component services |

**Deps:** `gPMComponentServices`, `gPMConstants`, `gPMFunctions`, `iGISSharedConstants`, `bPMFunc`, Reflection

---

### bGISTemp

**File:** `Components\bGISTemp.vb` | **Module:** `bGISTemp` | **ProgId:** `bGISTemp_NET.bGISTemp`

GIS policy linking transaction step constants and update method.

**Constants:** NB transaction type steps `GISNBTransType*` (0–18 named steps: CheckSchemeEDILink=0, CreateGISQuote=1 … SetInsurerRef=18, `GISNBTransTypeStart="NB"`). MTA transaction type steps `GISMTATransType*` (101–118, `GISMTATransTypeStart="MTA"`).

| Method | Description |
|--------|-------------|
| `UpdatePolicyLinkTransact(v_oDatabase, v_lInsuranceFileKey, v_sTransactionType, v_iStep, v_lGISPolicyKey, v_lGISQuoteKey) As Integer` | Executes `spu_gis_policy_link_transact_upd` with all 5 parameters; returns PMTrue/PMError |

**SPs:** `spu_gis_policy_link_transact_upd`

---

### bPMAddParameter

**File:** `Components\bPMAddParameter.vb` | **Module:** `bPMAddParameter` | **ProgId:** `bPMAddParameter_NET.bPMAddParameter`

Stored procedure parameter builder that constructs WHERE and INSERT clause SQL strings.

| Method | Description |
|--------|-------------|
| `AddParameter(v_sCurrentSQL, v_sFieldName, v_sValue, v_sMode, r_sNewSQL, [v_sTable], [v_bForceJoin])` | Appends WHERE condition or INSERT field to SQL string depending on `v_sMode` ("WHERE"/"INSERT") |
| `AddParameterLite(v_sCurrentSQL, v_sFieldName, v_sValue, r_sNewSQL)` | Simplified overload building WHERE-style clause only |

**Deps:** `gPMConstants`

---

### bPMDocFunctions

**File:** `Components\bPMDocFunctions.vb` | **Module:** `bPMDocFunctions` | **ProgId:** `bPMDocFunctions_NET.bPMDocFunctions`

Document management and file system utilities. **Properties:** `IsCalledFromBatchProcess As Boolean`, `Username As String`.

| Method | Description |
|--------|-------------|
| `GetWordVersion() As String` | Returns installed MS Word version from Registry |
| `StartWord(oWordApp, ...) As Integer` | Launches Word (visible or hidden) |
| `CloseWord(oWordApp) As Integer` | Quits Word application |
| `ClearDirectory(v_sPath) As Integer` | Deletes all files in a folder |
| `MoveFolderContents(v_sSource, v_sTarget) As Integer` | Moves all files/subfolders between directories |
| `CreateFolderTree(v_sFolderPath) As Integer` | Creates directory tree recursively |
| `GetDocumentDirectory([v_sBranchCode]) As String` | Returns root document storage path from Registry |
| `GetClientDirectory(v_lClientKey, ...) As String` | Returns client-level document path |
| `GetUniqueName(v_sName, v_sExtension) As String` | Appends timestamp to ensure unique filename |
| `GetExportDirectory([v_sBranchCode]) As String` | Returns export folder from Registry |
| `GetZipDirectory([v_sBranchCode]) As String` | Returns ZIP work folder from Registry |
| `UnZip(v_sZipFile, v_sTargetFolder) As Integer` | Extracts ZIP archive |
| `GetFileNameAndType(v_sFullPath, r_sName, r_sType)` | Splits path into filename and extension |
| `DelDirectory(v_sPath) As Integer` | Deletes a directory and all contents |
| `IsFolderExists(v_sPath) As Boolean` | Checks folder existence |
| `IsFileExists(v_sPath) As Boolean` | Checks file existence |
| `CreateFolder(v_sPath) As Integer` | Creates a single folder |
| `DeleteFile(v_sFilePath) As Integer` | Deletes a file |
| `CopyFile(v_sSource, v_sTarget) As Integer` | Copies a file |
| `CopyFolder(v_sSource, v_sTarget) As Integer` | Copies a folder tree |
| `NoOfFilesInDirectory(v_sPath) As Long` | Returns file count in directory |
| `Zip(v_sSourceFolder, v_sZipFile) As Integer` | Creates a ZIP archive |
| `ConvertDocumentUsingSiriusDocumentUtility(...)` | Converts document format via Sirius Document Utility |
| `PrintDocumentUsingSiriusDocumentUtility(...)` | Prints via Sirius Document Utility |
| `DocumentTitleCheckUsingSiriusDocumentUtility(...)` | Validates document title via Sirius Document Utility |
| `S3GetDocumentFromBucketUsingSiriusDocumentUtility(...)` | Retrieves document from AWS S3 bucket |
| `S3PutDocumentInBucketUsingSiriusDocumentUtility(...)` | Uploads document to AWS S3 bucket |

**Deps:** `gPMConstants`, `gPMFunctions`, `bPMFunc`, Microsoft.Office.Interop.Word, `System.IO.Compression`

---

### bPMFunc

**File:** `Components\bPMFunc.vb` | **Module:** `bPMFunc` | **ProgId:** `bPMFunc_NET.bPMFunc`

The primary general-purpose platform function library. Provides logging, encryption, system-option retrieval, currency utilities, and cross-cutting helpers.

| Method | Description |
|--------|-------------|
| `LogError(v_sMessage, [eLevel], [sException])` | Writes to Enterprise Library log |
| `LogMessage(...)` | 11 overloads with varying context params (branch, policy key, event key, etc.) |
| `ConvertWildCard(v_sText) As String` | Translates `*`/`?` wildcards to SQL `%`/`_` |
| `Encrypt(v_sData) As String` | XOR-shift cipher (legacy) |
| `Decrypt(v_sData) As String` | Reverses `Encrypt` |
| `LicenceEncrypt(v_sData) As String` | Alternate licence-key cipher |
| `BCryptEncrypt(v_sData) As String` | BCrypt hash using `BCrypt.Net` |
| `BCryptCheckPassword(v_sPlain, v_sHash) As Boolean` | Validates BCrypt hashed password |
| `AESEncryptPassword(v_sData) As String` | AES-CBC 128-bit encryption, Base64 output |
| `AESDecryptPassword(v_sEncData) As String` | AES-CBC decryption |
| `TripleDESGetOVal(v_sData) As String` | 3DES encrypt via `GetTripleDESCryptoServiceProvider` |
| `TripleDESGetEVal(v_sData) As String` | 3DES decrypt |
| `retrieveProductOptions(v_sBranchCode, v_sProductCode, r_aProductOptions()) As Integer` | Returns product option flags array |
| `RetrieveSingleSystemOption(v_oBusiness, v_lSystemOptionID, r_vValue) As Integer` | Reads single system option from database |
| `GetSystemSecurityModel(v_oBusiness, r_lSecurityModel) As Integer` | Returns security model type for branch |
| `GetBranchBaseCurrency(v_oDatabase, v_sBranchCode, r_sCurrencyCode) As Integer` | Inline SQL on currency tables |
| `GetBranchCurrencies(v_oDatabase, v_sBranchCode, r_sCurrencies(,)) As Integer` | Returns all currencies for branch |
| `GetCurrencyAuthorities(v_oDatabase, v_sBranchCode, v_sUserID, r_oCurrencies(,)) As Integer` | Returns user currency authority limits |
| `TransposeArray(v_aInput(,)) As Object(,)` | Transposes a 2D array (swap rows/columns) |
| `UniqueTableName(v_sTableName) As String` | Appends GUID to ensure unique temp-table name |
| `StripHTMLTags(v_sText) As String` | Removes HTML tags from string |
| `IsValidEmail(v_sEmail) As Boolean` | Regex email validation |
| `GetAllUsers(v_oDatabase, r_aUsers()) As Integer` | Returns all system users |
| `CalculateAge(v_dtDOB, v_dtAt) As Integer` | Age in whole years |
| `GetHSBCBankSortCode(v_sValue) As String` | Returns HSBC sort code from value string |

**Deps:** `gPMConstants`, `gSIRLibrary`, Enterprise Library Logging, `BCrypt.Net`, `System.Security.Cryptography`, `System.Net.Mail`

---

### bPMNavConstants

**File:** `Components\bPMNavConstants.vb` | **Module:** `NavigatorConstants` ⚠ | **ProgId:** `bPMNavConstants_NET.bPMNavConstants`

Constants and enums for the Navigator process/screen framework.

**Enums:**
- `ACEProcDetsColPos` — 9 procedure detail column positions (key, text, order, group, etc.)
- `ACEMapDetsColPos` — 4 map detail column positions
- `ACEMapStepsColPos` — 19 map step column positions (NodeCode, ScreenID, Condition, Action, etc.)
- `ACEMapStepsKeyColPos` — 3 map step key column positions
- `ACENavigatorVersion` — version flags (Version1=1/Version2=2/Version3=3)

No methods.

---

### bPMShellFunc

**File:** `Components\bPMShellFunc.vb` | **Module:** `ShellFunc` ⚠ | **ProgId:** `bPMShellFunc_NET.bPMShellFunc`

Synchronous shell execution via Win32 process exit-code polling. **Constants:** `PROCESS_QUERY_INFORMATION=&H400`, `STATUS_PENDING=&H103`. **Win32:** `GetExitCodeProcess` (kernel32).

One method: `ShellWait(v_sCommandLine)` — shells `v_sCommandLine`; polls `GetExitCodeProcess` in a DoEvents loop until exit code != STATUS_PENDING; logs on error.

**Deps:** `gPMConstants`, `gPMFunctions`, `iPMFunc.LogMessage`

---

### bPMWrkTaskInstance

**File:** `Components\bPMWrkTaskInstance.vb` | **Module:** `bPMWrkTaskInstance` | **ProgId:** `bPMWrkTaskInstance_NET.bPMWrkTaskInstance`

Enum and converters for Work Manager record-lock names.

**Enum `LockName`:** 17 members: InsFile=1 (insurance file), Party=2, NewBusiness=3, MTA=4, RenewalFile=5, ClaimFile=6, Covnote=7, CoverNote=8, Complaint=9, Task=10, PremFinPlans=11, PremFinScheme=12, FinancialTransaction=13, CashDeposit=14/PartyBankKey=15 (⚠ same value=15 for two names), ClaimCovnote=16, InvalidValue=17.

**Properties:**
- `LockNameString(v_eLockName) As String` — maps enum member to DB lock-name string
- `LockNameStringToEnum(v_sLockName) As LockName` — maps DB string back to enum; returns InvalidValue for unknown strings

---

### bSIRPremFinConst

**File:** `Components\bSIRPremFinConst.vb` | **Module:** `bSIRPremFinConst` | **ProgId:** `bSIRPremFinConst_NET.bSIRPremFinConst`

Premium Finance plan/scheme/instalment array-position constants and state. **Module-level state:** `m_sClientRef As String`, `m_bAllowMultiPlanSelect As Boolean`.

**Key constant groups:**
- Plan array (198 fields): `k_PFPlan*` positions 0–197 (PlanKey, SchemeKey, ClientRef, StartDate, EndDate, InterestRate, SetupFee, MonthlyFee, MaxFinanceAmt, TotalAmountPayable, APR, Status, InsuranceFileKey, etc.)
- Scheme array (68 fields): `k_PFScheme*` positions 0–67 (SchemeCode, SchemeName, MaxDuration, DefaultDepositPct, MinInterestRate, MaxInterestRate, etc.)
- Instalment array (25 fields): `PFInstalment*` positions (DueDate, Amount, Status, PaidDate, PaidAmount, etc.)
- Status indicators: `PFStatusIndDeleted="000", PFStatusIndActive="001" … PFStatusIndCancelled="999"`
- Integer status codes 1–10; finance type IDs (1=Standard/2=Single/3=Credit/4=Debit)
- MTA type IDs, instalment MTA types, Stargate list type constants

| Method | Description |
|--------|-------------|
| `GetClientRef() As String` | Returns `m_sClientRef` |
| `SetClientRef(v_sValue)` | Sets `m_sClientRef` |
| `GetAllowMultiPlanSelect() As Boolean` | Returns `m_bAllowMultiPlanSelect` |

---

### bTempFunc

**File:** `Components\bTempFunc.vb` | **Module:** `TempFunc` ⚠ | **ProgId:** `bTempFunc_NET.bTempFunc`

Temporary bridge functions. Note: `RetrieveSingleSystemOption` is **permanently stubbed** (returns PMFalse with no database call — replace callers with `bPMFunc.RetrieveSingleSystemOption`).

| Method | Description |
|--------|-------------|
| `LogMessage(v_sMessage, ...)` | Forwards to `gPMFunctions.LogMessageToFile`; swallows all exceptions |
| `RetrieveSingleSystemOption(...)` | ⚠ Stub — always returns PMFalse; does NOT query database |
| `LogMessageFile(v_sMessage, ...)` | Direct-to-file log (no Enterprise Library) |

---

### bUnderwritingBranchFunc

**File:** `Components\bUnderwritingBranchFunc.vb` | **Module:** `bUnderwritingBranchFunc` | **ProgId:** `bUnderwritingBranchFunc_NET.bUnderwritingBranchFunc`

Underwriting-branch specific lookups and scheme/party helpers.

| Method | Description |
|--------|-------------|
| `GetSchemeDetailsFromExternalSchemeNo(v_oDatabase, v_sBranchCode, v_sExternalSchemeNo, r_aSchemeDetails(,)) As Integer` | Executes `spu_GIS_Scheme_EDI_Link_STS_sel`; returns scheme array or PMNotFound |
| `GetPartyCntFromBrokerAbiId(v_oDatabase, v_sBranchCode, v_sBrokerAbiId, r_lPartyCount) As Integer` | Executes `spu_party_agent_broker_abi_id_sel`; returns party count |
| `GetUnderwritingBranchDetails(v_oDatabase, v_sBranchCode, r_sUnderwritingBranch) As Integer` | Checks `SIROPTUnderwritingBranchEnabled`; runs inline SQL `SELECT underwriting_branch_ind FROM source WHERE source_code=@branch_code`; returns PMTrue/PMNotFound |

**SPs:** `spu_GIS_Scheme_EDI_Link_STS_sel`, `spu_party_agent_broker_abi_id_sel`

---
## Swift — Swift Financial Planning Modules

### Registry (Swift)

**File:** `Swift\Registry.vb` | **Module:** `MRegistry` | No ProgId

Registry helpers scoped to the Swift FactFind application path (`HKLM/SOFTWARE/Sirius/Applications/FactFind`).

**Constants:** `ksRegPathPlanner = ksRegPathApplications + "\FactFind"`, `ksRegPathApplications = "\SOFTWARE\Sirius\Applications"`

| Method | Description |
|--------|-------------|
| `RegReadMachineSetting(sName) As String` | Reads value from `HKLM\...\FactFind` |
| `RegReadUserSetting(sName) As String` | Reads value from `HKCU\...\FactFind` |
| `RegWriteMachineSetting(sName, sValue)` | Writes value to `HKLM\...\FactFind` |
| `RegWriteUserSetting(sName, sValue)` | Writes value to `HKCU\...\FactFind` |

**Deps:** `gPMRegConst` (HKEY handles), `gPMFunctions` (GetPMRegSetting/SetPMRegSetting)

---

### SecurityConstants (Swift)

**File:** `Swift\SecurityConstants.vb` | **Module:** `MSecurityConstants` | No ProgId ⚠ (explicitly marked: DO NOT expose as COM)

Security model constants for the Swift suite product/module/right system.

**Key constant groups:**
- **Client type flags** (23 constants): `knBFUnknown=0, knBFNRM=1, knBFNBS=2, knBFNatWest=3, knBFUCB=5, knBFBarclays=6, knBFBlackHorse=7, knBFHargreaves=8, knBFCordros=9, knBFNemesis=10, knBFGuardian=11, knBFBrightGrey=12, knBFLegal&General=13, knBFAegon=14, knBFPO=15, knBFResolve=16, knBFAXA=17, knBFPartnerships=18, knBFPartnershipX=19, knBFFriends=20, knBFOnePulse=21, knBFPentagon=22, knBFCooperativeBank=23`
- **Product IDs** (10 constants): `knProductSwift=165, knProductAdvisory=9001, knProductSwiftRPServer=9005, knProductSwiftDocumentCreation=9006, knProductSwiftCashFlowAnalysis=9007, knProductIFPTools=9010, knProductIFPOnline=9011, knProductPureInsurance=9012, knProductUnknown=9999, knModulePersonalClients=10005, knModuleBusinessClients=10006, knModulePotentialClients=10007, knModuleClientManager=10008, knModuleDataStores=10009`
- **Application IDs** (5 constants): `knApplicationSwiftClientManager=1001, knApplicationSwiftReportingServer=1002, knApplicationSwiftDocumentCreation=1003, knApplicationSwiftCashFlowAnalysis=1004, knApplicationIFPTools=1005`
- **Module status codes** (3): Active/Inactive/Demo values
- **Module array indices** (10): `knMFKey=0` through `knMFDisplayName=9`
- **Module change codes** (5): `knMCActivate, knMCDeactivate, knMCDemo, knMCReplaceID, knMCDelete`
- **User Access Rights** (59 bit positions): `knUAFirst=0` through `knUALast=58`. Selected rights: IsManager=0, IsWritable=1, CanAccessHistory=2, CommissionModule=3, NewClient=4, DeleteClient=5, DeletePolicy=6, RunSwiftAccounts=7, ImportData=8, PrintDocuments=9, CanBuildScenarios=10 … CanAccessQualifications=39, CanAccessFactFind=40, UserDisabled=56, UserArchived=57, UserSupervisor=58
- **Fee precision constants:** `knDecimalPlaces=2`, `knRoundingMethod=MidpointRounding.AwayFromZero`

| Method | Description |
|--------|-------------|
| `SecIsProduct(id) As Boolean` | Returns True if 100 ≤ id ≤ 999 |
| `SecIsApplication(id) As Boolean` | Returns True if 1000 ≤ id ≤ 9999 |
| `TransModuleIDToIndex(id) As Integer` | Maps module ID to array index (0-based) |
| `TransModuleIndexToID(index) As Integer` | Maps array index to module ID |

---

## ComponentServices Directory

### gPMComponentServices

**File:** `ComponentServices\gPMComponentServices.vb` | **Module:** `gPMComponentServices` | No ProgId

COM+ component factory and session manager.

| Method | Description |
|--------|-------------|
| `CheckDatabase(v_sBranchCode, r_oDatabase) As Integer` | Creates database session object for branch via COM+ component |
| `NewDatabase(v_sBranchCode) As Object` | Returns new database session instance |
| `CreateBusinessObject(v_sProgID, r_oBusiness) As Integer` | Late-binds a COM+ business component by ProgId |
| `UpdateUserProperty(v_oBusiness, v_sName, v_oValue) As Integer` | Sets named property on the current user session |
| `GetUserProperty(v_oBusiness, v_sName, r_oValue) As Integer` | Gets named property from the current user session |

**Constants referenced:** `spu_get_sys_admin_status` (SQL constant, not called here directly). **Deps:** `gPMConstants`, `gPMFunctions`, `iPMFunc.LogMessage`

---

## ControlHelpers Directory

### ListViewItemComparer

**File:** `ControlHelpers\ListViewItemComparer.vb` | **Class:** `ListViewItemComparer` | **Implements:** `System.Collections.IComparer`

Type-aware `ListView` column sorter. Resolution order: DateTime → Decimal → Integer → String (case-insensitive).

| Method | Description |
|--------|-------------|
| `Compare(x, y) As Integer` | Compares two `ListViewItem` objects by the column set in constructor; auto-detects type |

**Constructor:** `New(column As Integer, order As SortOrder)` — stores column index and ascending/descending order.

---

## Consolidated Stored Procedure Cross-Reference

| Stored Procedure | Component | Method |
|------------------|-----------|--------|
| `spu_ACT_Get_LedgerID_From_ShortName` | bACTFunc | GetLedgerIDFromShortName, GetLedgerID |
| `spu_ACT_Get_Sub_Branch_id` | bACTFunc | GetSubBranchID |
| `spu_gis_policy_link_transact_upd` | bGISTemp | UpdatePolicyLinkTransact |
| `spu_GIS_Scheme_EDI_Link_STS_sel` | bUnderwritingBranchFunc | GetSchemeDetailsFromExternalSchemeNo |
| `spu_party_agent_broker_abi_id_sel` | bUnderwritingBranchFunc | GetPartyCntFromBrokerAbiId |
| `spu_Get_Party_History_Schema` | PartyFunc | CreateAndSavePartyHistorySchema |
| `spu_PB_script_sel` | PBReadScriptColumn | ReadScriptColumn |
| `spu_PM_SelAll_Source` | SiriusCoreFunc | GetBranches |
| `spu_sub_branch_sel` | SiriusCoreFunc | GetSubBranches |
| `spu_get_sys_admin_status` | gPMComponentServices | (referenced as constant only) |
| `spe_GIS_screen_sel` | PBDatabaseConsts | (SQL string constant ACGetAllScreenHeaderSQL only) |
| `spu_Scheme_Details_Select` | GIIConst | (inline SQL string constant only) |
| Inline SQL — currency tables | bPMFunc | GetBranchBaseCurrency, GetBranchCurrencies, GetCurrencyAuthorities |
| Inline SQL — `SELECT FROM address, country` | PBGetAddressFromAddressCnt | GetAddressFromAddressCnt |
| Inline SQL — `SELECT underwriting_branch_ind FROM source` | bUnderwritingBranchFunc | GetUnderwritingBranchDetails |
| Inline SQL — `SELECT source_id FROM source` | SiriusCoreFunc | (branch list supplement) |
| *bSIRPremFinConst comments reference* `spu_PFPremiumFinance_addnew`, `addnewversion`, `sel_latest_valid_plan`, `sel_single`, `sel_single_rec`, `sel_SingleFromInsuranceFile`, `update` | bSIRPremFinConst | (constants only — consuming SPs documented in comments) |

---

*End of Shared Files Components Reference*
