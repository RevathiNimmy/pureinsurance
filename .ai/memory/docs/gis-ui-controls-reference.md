# GIS Combined UI Interface & User Controls Reference

> This document covers all Interface_Renamed components and user controls found in
> `GIS Combined\GIS\Components\` and `GIS Combined\Product Builder\`.
> For business component (bGIS\*/bPMU\*/bSIR\*/bPB\*) details including stored procedures,
> see `.github/docs/gis-components-reference.md`.

---

## 1. Architecture Overview

### Interface Pattern

All GIS/Product Builder UI components follow the **Navigator XM Interface Pattern**:

```
Interface_Renamed (public entry point)
  └─ frmInterface / frmXxx (Windows Form — hidden implementation)
        └─ bXxx.Business (business component via ObjectManager.GetInstance)
              └─ SQL Server Stored Procedures
```

Standard lifecycle:
1. `Initialise()` — creates `bObjectManager`, gets language/source IDs
2. `SetProcessModes(task, navigate, processMode, transactionType, effectiveDate)` — sets mode flags
3. `SetKeys(vKeyArray(,))` — passes in primary keys
4. `Start()` — shows the form; blocks until the user closes it
5. `GetKeys(vKeyArray(,))` — retrieves output keys/values after close
6. `GetSummary(vSummaryArray(,))` — retrieves a summary display string
7. `Dispose()` — releases the `bObjectManager`

**Business object injection pattern (GIS User Def):** Several User Def Header/Detail interfaces
do *not* create their own business component. Instead they receive a pre-created business object
via a `Business` write-only property from their parent form.

### Standard Properties on All Interfaces

| Property | Direction | Description |
|---|---|---|
| `PMProductFamily` | RO | Product family ID constant |
| `PMAuthorityLevel` | WO | Authority level bitmask for this session |
| `CallingAppName` | WO | Name of the calling application for logging |
| `Status` | RO | Return status after `Start` completes |
| `StepStatus` | RW | Navigator step status string |

---

## 2. Component Index

### GIS Area (`GIS\Components\`)

| Interface | Folder | Business Component | Description |
|---|---|---|---|
| `iGISUserDefDetail` | GIS User Def Detail | (received via `Business` WO) | Add/edit a single user-defined lookup detail |
| `iGISUserDefDetailRate` | GIS User Def Detail | (received via `Business` WO) | Add/edit a single rate/indicator value for a detail |
| `iGISUserDefDetailRates` | GIS User Def Detail | (received via `Business` WO) | Browse list of rates/indicators for a detail |
| `iGISUserDefDetails` | GIS User Def Detail | (received via `Business` WO) | Browse list of user-defined details |
| `iGISUserDefHeader` | GIS User Def Header | (received via `Business` WO) | Add/edit a single user-defined lookup header |
| `iGISUserDefHeaderRate` | GIS User Def Header | (received via `Business` WO) | Add/edit a single rate/indicator for a header |
| `iGISUserDefHeaderRates` | GIS User Def Header | (received via `Business` WO) | Browse list of rates/indicators for a header |
| `iGISUserDefHeaders` | GIS User Def Header | (received via `Business` WO) | Browse list of user-defined headers |
| `iGISSchemeProperties` | Insurer Scheme | `bGISSchemeBusiness.Business` | Maintain GIS scheme configuration properties |
| `iGISListManager` | List Management | (direct bObjectManager usage) | Client-side GIS list code/description lookup API |
| `iGISSellerToolApplication` | Core GIS SellerTool | `bGISSellerTool*` (internal) | Programmatic API for quote/risk/MTA operations |
| `iGISSellerToolFinancial` | Core GIS SellerTool | (internal) | Financial operations: bank validation, premium finance |
| `iGISSellerToolQuotePolicy` | Core GIS SellerTool | (internal) | Party, quote, and policy search/retrieval operations |
| `iGISSellerToolRenewals` | Core GIS SellerTool | (internal) | Renewal invitation, confirmation, and lapse operations |
| `iGISSellerToolSecurity` | Core GIS SellerTool | (internal) | User registration, login, and agent authentication |

### Product Builder (`Product Builder\`)

| Interface | Folder | Business Component | Description |
|---|---|---|---|
| `iGISMaintainDataDictionary` | data model editor | `bGISMaintainDataDictionary.Business` | GIS data model maintenance and object/property management |
| `iGISObject` | data model editor | (passed by parent) | Add/edit a GIS data model object definition |
| `iGISProperty` | data model editor | (passed by parent) | Add/edit a GIS object property definition |
| `iGISRating` | 3D Rating | `bGISRating.Business` | 3D rating scheme editor |
| `GroupImport` | 3D Rating | `bGisGroupImport.Business` | Import group rating data |
| `iGISListGrouping` | 3D Rating | `bGISListGrouping.Business` | Maintain list groupings for 3D rating |
| `iGISListMaint` | 3D Rating ListImport | (internal) | Import and maintain rating lists |
| `iPMURuleEditor` | rule editor | (self-contained editor) | Edit product rules for a data model |
| `iPMURuleLookupData` | rule lookup | `bSIRRuleLookup.Business` | Look up rule data values |
| `iPMURuleLookupHeader` | rule lookup | `bSIRRuleLookup.Business` | Look up rule header definitions |
| `iPMUListMaint` | list maintenance | `bPMUList.bPMUListCreate`, `bGISListManager.Form` | Maintain GIS list data |
| `iGISSumInsured` | screen editor | (data-only, no business) | Capture sum insured details on a risk screen |
| `iPMUListScreens` | screen editor | `bSIRListScreen.Business`, `bPMLock.User` | Browse and select screen definitions |
| `iPMUMaintainScreenData` | screen editor | `bSIRMaintainScreenData.Business` | Design and maintain the layout of a GIS screen |
| `iPMUAddress` | screen display | `bSIRAddress.Business` | Display and capture address details on a risk screen |
| `iPMURisk` | screen display | `bPMUPolicy.Business`, `bCLMRiskDetails.Business`, + claims interfaces | Main risk entry/display form with dynamic GIS fields |
| `iPMURiskWrapper` | screen display | (delegates to `iPMURisk`) | Lightweight wrapper to launch `iPMURisk` from legacy code |
| `iPMUScreenControl` | screen display | `bGISPMUExtras.Business`, `bSIRRiskGroup.Business`, `bSIRRiskScreen.Business` | Core dynamic GIS field rendering engine |

### User Controls

| Control | Folder | Description |
|---|---|---|
| `iPBMaskedInputBox` | screen display\User Controls | Modal masked-input dialog for sensitive data entry |

---

## 3. GIS Area Components

### 3.1 GIS User Def Detail

These four interfaces manage the *detail* (child row) entries within a user-defined lookup hierarchy.
The parent caller (typically a back-office form or `iGISMaintainDataDictionary`) creates the
business object and passes it in via the `Business` WO property.

---

#### `iGISUserDefDetail`

**Files:**
- Interface: `GIS\Components\GIS User Def Detail\Interface\iGISUserDefDetail\iGISUserDefDetailInterface.vb` (22.9 KB)
- Form: `iGISUserDefDetailFrm.vb` (50.6 KB)

**Purpose:** Add, edit, or view a single user-defined lookup detail record.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Business` | WO | Object | Pre-created business object passed in by parent |
| `LookupHeaderId` | RW | Integer | ID of the parent lookup header |
| `LookupParentId` | RW | Integer | ID of the parent lookup detail (for nesting) |
| `LookupDetailId` | RW | Integer | Primary key of this detail record |
| `Code` | RW | String | Short code for the detail item |
| `Description` | RW | String | Display description |
| `ParentId` | RW | Integer | Parent detail ID |
| `ParentCode` | RW | String | Parent detail code (for display) |
| `AllowedParents` | WO | Object | Array of valid parent detail items |
| `GISUserDefHeaderIndsId` | RW | Integer | ID of the associated header indicator record |
| `GISUserDefHeaderIndsCode` | RW | String | Code of the associated header indicator |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Form Public Methods (frmInterface):**

| Method | Description |
|---|---|
| `SetFieldValidation()` | Applies validation rules to UI fields based on process mode |
| `GetBusiness()` | Returns the current business object reference |
| `BusinessToInterface()` | Loads data from the business object into form UI fields |
| `InterfaceToBusiness()` | Writes form UI field values back to the business object |

**Business Component:** Business object received via `Business` property. The form then launches
`iGISUserDefDetailRates.Interface_Renamed` as a child interface to manage rates/indicators.

---

#### `iGISUserDefDetailRate`

**Files:**
- Interface: `GIS\Components\GIS User Def Detail\Interface\iGISUserDefDetailRate\iGISUserDefDetailRateInterface.vb` (21.1 KB)
- Form: `iGISUserDefDetailRateFrm.vb` (42.5 KB)

**Purpose:** Add, edit, or view a single rate or indicator value associated with a specific detail record.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `LookupDetailId` | RW | Integer | ID of the parent detail record |
| `Detail` | RW | String | Detail code or description |
| `Code` | RW | String | The rate code |
| `Description` | RW | String | Description of the rate |
| `RatesOrIndicators` | RW | Integer | Flag: 0=Rates, 1=Indicators |
| `Value` | RW | Object | The rate/indicator value |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent interface.

---

#### `iGISUserDefDetailRates`

**Files:**
- Interface: `GIS\Components\GIS User Def Detail\Interface\iGISUserDefDetailRates\iGISUserDefDetailRatesInterface.vb` (20.2 KB)
- Form: `iGISUserDefDetailRatesFrm.vb` (49.5 KB)

**Purpose:** Browse and manage the list of rate/indicator values for a given detail record. Child interfaces
open `iGISUserDefDetailRate` for individual add/edit.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `LookupDetailId` | RW | Integer | Primary key of the parent detail record |
| `Detail` | RW | String | Detail display code |
| `RatesOrIndicators` | RW | Integer | Specifies whether list shows rates or indicators |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent.

---

#### `iGISUserDefDetails`

**Files:**
- Interface: `GIS\Components\GIS User Def Detail\Interface\iGISUserDefDetails\iGISUserDefDetailsInterface.vb` (20.8 KB)
- Form: `iGISUserDefDetails.vb` (64.2 KB)

**Purpose:** Browse the full list of user-defined detail records under a given header. Launches
`iGISUserDefDetail` for add/edit.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `LookupParentId` | RW | Integer | Optional parent detail filter (for hierarchical lookups) |
| `LookupHeaderId` | RW | Integer | Header whose details are listed |
| `Header` | RW | String | Display code for the parent header |
| `UniqueId` | RW | String | Navigator unique identifier for this form instance |
| `ScreenHierarchy` | RW | String | Navigator hierarchy path |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent.

---

### 3.2 GIS User Def Header

These four interfaces manage the *header* (parent row) entries in a user-defined lookup table.
They mirror the structure of the User Def Detail interfaces. The business object is again
passed in from the calling form.

---

#### `iGISUserDefHeader`

**Files:**
- Interface: `GIS\Components\GIS User Def Header\Interface\iGISUserDefHeader\iGISUserDefHeaderInterface.vb` (22.5 KB)
- Form: `iGISUserDefHeaderFrm.vb` (59.3 KB)

**Purpose:** Add, edit, or view a single user-defined lookup header record.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Business` | WO | Object | Pre-created business object from parent caller |
| `LookupHeaderId` | RW | Integer | Primary key of the header being edited |
| `Code` | RW | String | Short code for the header |
| `Description` | RW | String | Display description |
| `ParentId` | RW | Integer | Optional parent header ID (for nested headers) |
| `ParentCode` | RW | String | Parent header code |
| `AllowedParents` | WO | Object | Valid parent header array |
| `ExistingHeaders` | WO | Object | Existing header collection for duplicate checking |
| `UniqueId` | RW | String | Navigator unique identifier |
| `ScreenHierarchy` | RW | String | Navigator hierarchy path |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent. Form launches child interfaces:
- `iGISUserDefHeaderRates.Interface_Renamed` — rates/indicators for this header
- `iGISUserDefDetails.Interface_Renamed` — detail rows under this header

---

#### `iGISUserDefHeaderRate`

**Files:**
- Interface: `GIS\Components\GIS User Def Header\Interface\iGISUserDefHeaderRate\iGISUserDefHeaderRateInterface.vb` (21.7 KB)
- Form: `iGISUserDefHeaderRateFrm.vb` (41.1 KB)

**Purpose:** Add, edit, or view a single rate/indicator record for a lookup header.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `LookupHeaderId` | RW | Integer | ID of the parent header |
| `Header` | RW | String | Header display code |
| `Code` | RW | String | Rate code |
| `Description` | RW | String | Rate description |
| `RatesOrIndicators` | RW | Integer | 0=Rates, 1=Indicators |
| `UniqueId` | RW | String | Navigator unique identifier |
| `ScreenHierarchy` | RW | String | Navigator hierarchy path |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent.

---

#### `iGISUserDefHeaderRates`

**Files:**
- Interface: `GIS\Components\GIS User Def Header\Interface\iGISUserDefHeaderRates\iGISUserDefHeaderRatesInterface.vb` (20.9 KB)
- Form: `iGISUserDefHeaderRatesFrm.vb` (57.1 KB)

**Purpose:** Browse and manage the list of rate/indicator values for a given lookup header.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `LookupHeaderId` | RW | Integer | Parent header ID |
| `Header` | RW | String | Header display code |
| `RatesOrIndicators` | RW | Integer | Specifies rates or indicators list |
| `UniqueId` | RW | String | Navigator unique identifier |
| `ScreenHierarchy` | RW | String | Navigator hierarchy path |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent.

---

#### `iGISUserDefHeaders`

**Files:**
- Interface: `GIS\Components\GIS User Def Header\Interface\iGISUserDefHeaders\iGISUserDefHeadersInterface.vb` (19.2 KB)
- Form: `iGISUserDefHeadersFrm.vb` (66.4 KB)

**Purpose:** Browse the full list of user-defined lookup headers. Launches `iGISUserDefHeader`
for add/edit of individual records.

**Properties:** No domain-specific properties beyond the standard set (`PMProductFamily`, `PMAuthorityLevel`,
`CallingAppName`, `Status`, `StepStatus`).

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Business object received from parent.

---

### 3.3 Insurer Scheme — `iGISSchemeProperties`

**Files:**
- `GIS\Components\Insurer Scheme\Scheme Properties\iGISSchemeProperties\Properties.vb` — main class (`Properties`)
- `Propertys.vb` — collection wrapper

**Class name:** `Properties` (not `Interface_Renamed`). Implements `ILocalInterface`.

**Purpose:** Maintain GIS scheme configuration properties on an insurer scheme. Used during the
policy binding/scheme selection workflow to store and retrieve navigation profiles and required
property flags.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISSchemeId` | RO | Integer | The GIS scheme whose properties are being maintained |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates the object manager and initialises it |
| `Dispose()` | — | Releases the object manager |
| `StoreNavigationProfile` | `v_lPolicyLinkID`, `v_bPostQuoteProfile`, `r_lSelectedSchemes`, `v_bUseSelectedScheme?`, `v_bViewQuote?` | Persists a navigation profile for the specified policy link, recording selected schemes and whether a quote view is needed |
| `Required` | `r_iRequired`, `v_lPropertyID?`, `v_sObjectName?`, `v_sPropertyName?` | Checks or sets whether a given scheme property is required; populates `r_iRequired` |

**`Propertys` Collection Class:**

| Method/Property | Description |
|---|---|
| `Add(v_iRequired, v_sKey)` | Adds a new property requirement entry with the given key |
| `Remove(v_vIndex)` | Removes an entry by index or key |
| `Item(v_vIndex)` | Returns the `Properties` object at the given index or key |
| `Count` | Number of items in the collection |
| `GetEnumerator()` | Enables `For Each` iteration |

**Business Component:** `bGISSchemeBusiness.Business`

---

### 3.4 List Management — `iGISListManager`

**Files:**
- `GIS\Components\List Management\Client\iGISListManager\iGISListManagerCls.vb` — main class
- `iGISListManagerCommon.vb` — shared helper methods
- `iGISListManagerNoLogon.vb` — variant without logon requirement

**Class name:** `Interface_Renamed`. Does **not** implement `ILocalInterface`; this is a
programmatic API class, not a Navigator-pattern windowed interface.

**Purpose:** Client-side API for resolving GIS list codes and descriptions. Used by SSP product
screens and third-party portals to look up ABI-coded values without needing direct database access.

**Public Fields:**

| Field | Type | Description |
|---|---|---|
| `g_oObjectManager` | Object | The object manager instance |
| `g_iLanguageID` | Integer | Language ID for multi-language list lookups |
| `g_iSourceID` | Integer | Source/portal identifier |

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `MaxListItems` | WO | Integer | Maximum number of items returned in list queries (0 = unlimited) |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates and initialises the `bObjectManager`; stores language and source IDs |
| `Dispose()` | — | Releases the object manager |
| `GetListIdsAndNames` | `r_vResultArray(,)` | Returns all available list IDs and their names as a 2D array |
| `GetDescriptionFromABICode` | `v_sPropertyId`, `v_sABICode`, `r_sDescription` | Resolves an ABI code to its plain text description for the given property |
| `GetABICodeFromDescription` | `v_sPropertyId`, `v_sDescription`, `r_sABICode` | Reverse-lookup: resolves a description string back to its ABI code |
| `GetListAndCodes` | `v_sPropertyId`, `r_vListData`, `r_vListDataCode`, `v_vSearchString?` | Returns parallel arrays of display values and their codes for the given property's list |
| `GetList` | `v_sPropertyId`, `r_vListData`, `v_vSearchString?` | Returns display-only list values (no codes) |
| `PopulateListControl` | `v_sPropertyId`, `r_oControl`, `v_vSearchString?` | Directly populates a Windows Forms list/combo control with values from the specified list |
| `CheckListVersions` | `v_sGisDataModelCode`, `v_sSellerCode?` | Validates that locally cached list data is current; triggers a refresh if the server version is newer |
| `GetDescription` | `sPropertyId`, `sABICodeTarget`, `sDescription` | Utility overload — resolves a specific ABI code to its description |

---

### 3.5 Core GIS — SellerTool

The SellerTool is a **programmatic API** (not a visual Navigator interface) intended for use by
SSP-connected third-party portals (e.g. price-comparison websites, direct-sell portals). It
consists of five separate `NotInheritable` classes compiled into one assembly.

All five classes follow the same pattern:
- `PMProductFamily` (RO), `CallingAppName` (WO), `Initialise()`, `Dispose()`

---

#### `iGISSellerToolApplication` (class: `Application`)

**File:** `GIS\Components\Core GIS\SellerTool\iGISSellerToolApplication.vb`

**Purpose:** Core quote creation, risk management, and GIS data-set API for external portals.

**Key Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Risk` | RO | `cGISDataSetControl.Node` | Current risk node in the in-memory GIS data set |
| `Quote(v_lQuoteNum)` | RO | `cGISDataSetControl.Node` | Quote output node by index |
| `QuoteCount` | RO | Integer | Number of quotation outputs in the current data set |
| `LookupsRequiredInsurerNo` | RW | Integer | Insurer number to use for list lookups |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `AddQuote` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_dtEffectiveDate`, `v_dtExpirationDate`, `v_sInsuredName`, `v_lPartyCnt`, `v_lAgentCnt`, `r_lInsuranceFolderCnt`, `r_lInsuranceFileCnt`, `v_sInsuranceFolderDescription?`, `r_vAdditionalDataArray?` | Creates a new quote (insurance file) for a party |
| `AddRisk` | `v_sBackOfficeMapperCode`, `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lInsuranceFolderCnt`, `v_lInsuranceFileCnt`, `v_lPartyCnt`, `v_lRiskTypeId`, `v_lRiskScreenId`, `v_sRiskDescription`, `v_lProductID`, `r_lRiskFolderCnt`, `r_lRiskCnt`, `r_sXMLDataSetDef`, `r_sXMLDataset`, `r_vPolicyLinkID`, `r_vTopOIKey`, `r_vQuoteRef`, `r_vQuoteRefPassword`, `r_vAdditionalDataArray?` | Adds a new risk to an existing quote; returns XML data set definitions |
| `NBStart` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `r_vPolicyLinkID`, `r_vTopOIKey`, `r_vQuoteRef`, `r_vQuoteRefPassword`, `v_lPartyCnt?`, `r_vInsuranceFileCnt?`, `r_vAdditionalDataArray?` | Initialises a new-business transaction in memory |
| `MTAStart` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_iType`, `v_dtCoverStartDate`, `v_dtExpiryDate`, `v_lPolicyVersion`, `v_lOldPolicyLinkID?`, `v_lOldInsuranceFileCnt?`, `r_vNewPolicyLinkID?`, `r_vNewInsuranceFileCnt?`, `r_vAdditionalDataArray?` | Initialises a mid-term adjustment (MTA) in memory |
| `NBQuote` | `v_lQuoteType`, `v_sGISBusinessTypeCode`, `v_dtEffectiveDate`, `v_lGISSchemeID?`, `r_vAdditionalDataArray?` | Runs a new-business rating/quote calculation |
| `MTAQuote` | `v_lQuoteType`, `v_sGISBusinessTypeCode`, `v_dtEffectiveDate`, `v_sXMLOldRisk`, `v_lGISSchemeID?`, `r_vAdditionalDataArray?` | Runs an MTA rating/quote calculation |
| `NBTransact` | `v_lGISSchemeID`, `v_sGISBusinessTypeCode?`, `r_vAdditionalDataArray?` | Binds a new-business quote to create an active policy |
| `MTATransact` | `v_lQuoteType`, `v_sGISBusinessTypeCode`, `v_dtEffectiveDate`, `v_sXMLOldRisk`, `v_lGISSchemeID?`, `r_vAdditionalDataArray?` | Completes an MTA transaction against the policy |
| `NewDataSet` | `v_sGisDataModelCode`, `r_vPolicyLinkID`, `r_vTopOIKey`, `r_vQuoteRef`, `r_vQuoteRefPassword`, `v_vInsuranceFileCnt?` | Creates an empty GIS data set in memory |
| `LoadFromDB` | `v_sGisDataModelCode`, `v_sQuoteRef`, `v_sQuoteRefPassword`, `r_vTopOIKey`, `r_vGuaranteedQuoteDate`, `v_lInsuranceFileCnt?`, `v_lRiskID?` | Loads an existing GIS data set from the database by quote reference |
| `SaveToDB` | — | Persists the current in-memory GIS data set to the database |
| `LoadFromXML` | `v_sGisDataModelCode`, `v_sXMLDataSet` | Loads a GIS data set from an XML string |
| `ReturnAsXML` | `r_vXMLDataSet` | Exports the current GIS data set as an XML string |
| `SetPropertyValue` | `v_sObjectName`, `v_sPropertyName`, `v_sOIKey`, `v_vPropertyValue`, `v_bIsAssumedInfo?` | Sets a single GIS property value on a specific object instance |
| `GetPropertyValue` | `v_sObjectName`, `v_sPropertyName`, `v_sOIKey`, `r_vPropertyValue`, `r_bIsAssumedInfo?` | Gets a single GIS property value from a specific object instance |
| `NewObjectInstance` | `v_sObjectName`, `r_vOIKey`, `v_sParentOIKey?` | Creates a new instance of a GIS object (row) in the in-memory data set |
| `DelObjectInstance` | `v_sObjectName`, `v_sOIKey` | Deletes an object instance from the in-memory data set |
| `ClearObjectByKey` | `v_sObjectName`, `v_sOIKey` | Clears all property values for an object instance without deleting it |
| `ClearAllObjects` | — | Clears all object instances from the data set |
| `GetAllOIKey` | `v_sObjectName`, `r_vOIKeyArray(,)` | Returns all instance keys for a given object type |
| `GetChildOIKey` | `v_sParentObjectName`, `v_sParentOIKey`, `v_sChildObjectName`, `r_vChildOIKeyArray(,)` | Returns all child instance keys for a given parent instance |
| `GetListAndCodes` | `v_sObjectName`, `v_sPropertyName`, `r_vListData`, `r_vListDataCodes`, `v_vSearchString?`, `v_bMultiSearch?`, `r_vGISListID?` | Returns the list data and codes for a list-type property |
| `GetVehicleList` | `v_sObjectName`, `v_sPropertyName`, `r_vListData`, `v_vMake`, `v_vModelChosen?`, `v_vYear?`, `v_vCC?`, `v_vDoors?`, `v_vFuelType?`, `v_vTransType?` | Returns matching vehicle data for vehicle-property lookups |
| `GetCodeDescription` | `v_sObjectName`, `v_sPropertyName`, `v_sCode`, `r_sDescription` | Resolves a GIS property code to its display description |
| `InitialiseLookups` | `v_sGisDataModelCode`, `v_sBusinessTypeCode`, `v_dtProcessDate`, `v_lStatus` | Prepares the lookup tables for a specific data model and business type |
| `InitialiseListMgmt` | `v_sGisDataModelCode`, `v_sSellerCode?` | Initialises list management for the given data model |
| `PostcodeSearch` | `v_sNameNum`, `v_sPostcode`, `r_vMatchArray(,)` | Performs a postcode address lookup |
| `VehicleLookup` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_sRegistrationNumber`, `r_vAdditionalDataArray?` | Looks up vehicle details by registration number |
| `GetInstanceHierarchy` | `v_sObjectName`, `r_vObjectInstanceArray(,)`, `r_lMaxInstances` | Returns the hierarchical instance structure for an object type |
| `GetObjectDefDetails` | `v_sObjectName`, + many optional `r_` params | Returns the definition details of a GIS object (table name, SQL, properties, etc.) |
| `GetObjectIdentity` | `v_sObjectName`, `v_sOIKey`, `r_vPropertyArray(,)` | Returns the identifying property values for a specific object instance |
| `GetRatingDetails` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lInsuranceFolderCnt`, `v_lInsuranceFileCnt`, `v_lRiskCnt`, `r_vRatingSections` | Returns rating section details after a quote calculation |
| `PolicyLinkID()` | — | Returns the current policy link ID from the in-memory data set |
| `NewQuoteOutput` | `r_lQEMNumber`, `r_sInsurer`, `r_lInsurerID`, `r_sScheme`, `r_lSchemeID`, `r_lSchemeVer`, `r_sQuoteKey` | Retrieves the primary quote output (insurer and scheme) after rating |
| `NewQuoteOutputSaveDB` | `v_lGISSchemeID`, `r_vQuoteKey`, `r_vTopQuoteOIKey` | Saves a specific quote output selection to the database |
| `UpdateQuoteRef` | `v_sQuoteRef`, `v_sQuoteRefPassword` | Updates the quote reference and password on the current policy |
| `UpdatePartyCnt` | `v_lPartyCnt`, `v_lInsuranceFileCnt` | Updates the primary party on the current policy |
| `Refer` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_sInsurerCode`, `r_vAdditionalDataArray?` | Refers the current quote to an insurer for manual review |
| `SendEmail` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lEMailType`, `v_sEMailFrom`, `v_sEMailTo`, `v_sEMailCC`, `v_sEMailSubject`, `v_sEMailText`, `r_vAdditionalDataArray?` | Sends a system email (e.g. confirmation, referral notification) |
| `PrintForm` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lFormNumber`, `v_lGISSchemeID` | Prints a pre-defined output form for the current risk |
| `SaveToFile` | `v_sDataSetDefFile?`, `v_sDataSetFile?` | Saves the data set definition and data to files (for offline use) |
| `GetRegSetting` | `v_lPMERegSettingRoot`, `v_lPMEProductFamily`, `v_lPMERegSettingLevel`, `v_sSettingName`, `r_sSettingValue`, `v_sSubKey?` | Retrieves a registry/configuration setting value |
| `GetAddressFromAddressCnt` | `v_lAddressCnt`, `r_vAddressArray(,)` | Returns the full address for a given party address counter |
| `IsInsurerQMM` | `v_sGisDataModelCode`, `v_vInsurerNo`, `r_bIsInsurerQMM` | Checks whether an insurer uses QMM (Quote Management Module) quoting |

---

#### `iGISSellerToolFinancial` (class: `Financial`)

**File:** `GIS\Components\Core GIS\SellerTool\iGISSellerToolFinancial.vb`

**Purpose:** Financial services operations for external portals — bank account validation, premium
finance quotation, payment method charge calculation, and Datacash card transactions.

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `BankAccountValidation` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_sSenderID`, `v_sCoverType`, `v_sGnetClientCode`, `v_sBusinessStatus`, `v_sBankAccountName`, `v_sBankAccountNo`, `v_sBankSortCode`, `v_sQuoteReference`, `r_vStatusCode` | Validates a bank account for direct debit setup; returns a status code |
| `PremiumFinanceQuote` | `v_vDataModelCode`, `v_vBusinessTypeCode`, `v_vBusinessStatus`, `v_vPremiumAmount`, `v_vPremiumFinanceRef`, `v_vEffectiveDate`, `v_vPolicyNo`, `r_vStatusCode`, `r_vStatusExplanation`, `r_vTotalPayable`, `r_vNumberOfInstalmentsLeft`, `r_vFirstInstalAmt`, `r_vSubsequentInstalAmt`, `r_vActualPaymentDate`, `r_vInterestAmount` | Gets a premium finance quotation with instalment breakdown |
| `CalcPaymentMethodCharge` | `v_sProductFamily`, `v_sBusinessTypeCode`, `v_sDataModelCode`, `v_sTransactionType`, `v_sPaymentMethod`, `v_sStartDate`, `v_sAmountToFinance`, `v_sNoOfInstalments`, `v_sActionType`, `v_sRequestedDepositPercent`, + many `r_v` output params | Calculates payment method charges — APR, interest, instalment amounts, deposit, fees |
| `Datacash` | `v_sDataModelCode`, `v_sRequestType`, `v_sRef`, `v_sCardNum`, `v_iExpMonth`, `v_iExpYear`, `v_sAmt`, `v_sSwitchExtraInfo`, `v_sAuthCode`, `v_sTransactionType`, `r_vResponseArray`, optional card/billing params | Performs a Datacash card transaction (authorise, capture, refund) |

---

#### `iGISSellerToolQuotePolicy` (class: `QuotePolicy`)

**File:** `GIS\Components\Core GIS\SellerTool\iGISSellerToolQuotePolicy.vb`

**Purpose:** Party, quote, and policy retrieval/search operations for external portals.

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `AddParty` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_sPartyTypeCode`, `v_sForename`, `v_sSurname`, `v_sDateOfBirth`, `v_sEmailAddress`, `v_sCurrentRenewalDate`, address fields, `r_lPartyCnt`, optional fields | Creates a new customer party record; returns `r_lPartyCnt` |
| `FindParty` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_sPartyType`, `v_sShortname`, `v_sResolvedName`, `v_sUserID`, `v_sTelephoneNumber`, `v_sPostcode`, `r_vResultArray(,)`, optional params | Searches for existing party records matching the supplied criteria |
| `GetParty` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lPartyCnt`, + many `r_s` output params | Retrieves full party details by party counter |
| `UpdateParty` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lPartyCnt`, optional update fields | Updates an existing party record with new details |
| `FindQuote` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `r_vResultArray(,)`, optional filter params | Searches for quotes matching optional criteria (ref, date, description, agent) |
| `GetQuotesForParty` | `v_sDataModelCode`, `v_lPartyCnt`, `r_vQuoteArray(,)`, `v_sPolicyTypeCode?` | Gets all quotes for a specific party |
| `GetQuotes` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lPartyCnt`, `r_vQuoteArray(,)` | Gets all quotes for a party within a data model / business type |
| `GetQuoteDetails` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lInsuranceFileCnt`, `r_vQuoteArray(,)` | Gets detailed quote information for a specific insurance file |
| `GetQuoteRisks` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lInsuranceFileCnt`, `r_vQuoteArray(,)` | Returns risk-level details for a given quote |
| `GetQuotesAndPoliciesForParty` | `v_sDataModelCode`, `v_lPartyCnt`, `r_vQuotePolicyArray(,)`, `v_sPolicyTypeCode?` | Combined retrieval of both quotes and policies for a party |
| `GetPoliciesForParty` | `v_sDataModelCode`, `v_lPartyCnt`, `r_vPolicyArray(,)`, `v_sPolicyTypeCode?` | Gets all active policies for a party |
| `GetPolicyVersions` | `v_sDataModelCode`, `r_vPolicyVersionArray(,)`, `v_lInsuranceFileCnt?`, `v_sInsuranceFileRef?` | Returns all version history records for a policy |
| `FindPolicy` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `r_vResultArray(,)`, `r_vAdditionalDataArray?` | Searches for policies using various criteria |
| `GetProductByAgent` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lAgentPartyCnt`, `r_vResultArray(,)`, `r_vAdditionalDataArray?` | Returns available products for a given agent party |
| `GetRatingDetails` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lInsuranceFolderCnt`, `v_lInsuranceFileCnt`, `v_lRiskCnt`, `r_vRatingSections` | Returns rating section details for a specific risk |
| `GetRiskByProduct` | `v_sGisDataModelCode`, `v_sGISBusinessTypeCode`, `v_lProductID`, `r_vResultArray`, `r_vAdditionalDataArray?` | Returns risk records matching a product ID |

---

#### `iGISSellerToolRenewals` (class: `Renewals`)

**File:** `GIS\Components\Core GIS\SellerTool\iGISSellerToolRenewals.vb`

**Purpose:** Renewal workflow operations for external portals — invitation, confirmation, lapse,
and status management.

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `RenGetPassword` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_lInsuranceFileCnt`, `r_sUnencryptedPassword` | Retrieves the renewal password for a given policy |
| `UpdateRenewalControl` | many optional params | Updates the renewal control record — status, scheme, suspension level, dates |
| `ListRenewals` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `r_vResultArray`, optional filter params | Returns a list of renewals matching optional criteria (status, date range, policy number, insurer, etc.) |
| `GetPolicyRenewalVersion` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_lInsuranceFolderCnt`, `r_vResultArray` | Returns available renewal version records for a policy folder |
| `ConfirmLapse` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_lInsuranceFolderCnt`, `v_lInsuranceFileCnt`, `v_lSchemeID`, `v_lPartyCnt` | Confirms that a policy has lapsed (non-renewed) |
| `ConfirmRenewal` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_lInsuranceFolderCnt`, `v_lInsuranceFileCnt`, `v_lSchemeID`, `v_lPartyCnt`, `v_bIsWhatIfQ?`, `v_bAutoConfirm?` | Confirms acceptance of a renewal offer |
| `ConfirmRenewalBrokerLed` | same + `v_lPolicyBinderID?`, `v_lPolicyID?`, `v_lOldInsurerNo?`, `v_lNewInsurerNo?`, `v_lSchemeNo?`, `v_sCoverCode?` | Broker-led renewal confirmation with insurer switch support |
| `RenQuotationBrokerLead` | many params | Requests a broker-led renewal quotation for a specific scheme and product |
| `RenMultipleQuotationBrokerLead` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `r_vSelectedArray`, `r_vFailedArray`, `r_vResultArray` | Batch broker-led renewal quotation request |
| `RenCompAlternateInsurer` | many params | Computer-led competitive renewal with an alternative insurer |
| `RenCompHoldingInsurer` | many params | Computer-led competitive renewal retained with current insurer |
| `RenCompLapse` | many params | Computer-led competitive renewal resulting in a lapse |
| `RenConfDocsHoldingInsurer` | many params | Sends confirmation documents for a holding-insurer renewal |
| `RenMultipleInvitationBrokerLed` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `r_vSelectedArray`, `r_vFailedArray` | Batch broker-led renewal invitation |
| `RenInvitePreferredQuotes` | many params | Invites preferred quotation schemes for renewal |
| `RenReminder` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_lInsuranceFolderCnt`, `v_lPartyCnt`, `v_lRenewalInsuranceFileCnt` | Sends a renewal reminder notification |
| `RenReprintConfirm` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_lDataModelID`, `v_lRenewalEdiAuditId`, `r_lRenewalInsuranceFileCnt` | Reprints renewal confirmation documents |
| `RenReprintInvitationBrokerLead` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_vSelectedArray`, `r_vFailedArray` | Reprints renewal invitation documents in batch |
| `RenResendEDI` | `v_sBusinessTypeCode`, `v_sDataModelCode`, `v_lDataModelID`, `v_lRenewalEdiAuditId`, `r_lRenewalInsuranceFileCnt` | Resends EDI renewal data to the insurer |
| `RenSelection` | `v_sBusinessTypeCode`, `v_sDataModelCode`, `v_lInsuranceFolderCnt`, `v_lPartyCnt`, `v_dtRenewalDate`, `v_lRiskCodeID`, `r_sDataModelCode` | Runs the renewal selection algorithm to identify the appropriate renewal path |
| `UpdateLapseReason` | `v_lInsuranceFolderCnt`, `v_lLapseReasonID`, `v_sLapseComment` | Records the lapse reason on a lapsed renewal |
| `RenGetInsurerQuoteOptions` | `v_vSchemes`, `v_vCoverCode`, `r_vGridLayout` | Returns insurer quote scheme options in grid layout format |
| `RenCustLogin` | `v_sGisDataModelCode`, `v_sPolicyNo`, `v_sPassword`, `r_lInsuranceFolderCnt`, `r_lInsuranceFileCnt`, `r_sMenuURL` | Authenticates a customer for self-service renewal |
| `SelectRenewalTaskLog` | `r_vResultArray`, optional filter params | Returns the renewal task log for audit and monitoring |
| `RenIsActiveRenewal` | `v_lInsuranceFileCnt`, `v_sInsuranceRef`, `r_lInsuranceFolderCnt`, `r_sStatus`, `r_sNewPolicyNo`, `r_dRenewalDate`, `r_sInsurer`, `r_sScheme` | Checks whether a policy has an active renewal in progress |

---

#### `iGISSellerToolSecurity` (class: `Security`)

**File:** `GIS\Components\Core GIS\SellerTool\iGISSellerToolSecurity.vb`

**Purpose:** User registration, login, and agent authentication for external portals.

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `RegisterUser` | `v_sDataModelCode`, personal detail fields, `r_sUserID`, `r_sPassword`, `r_sPartyCnt`, optional fields | Registers a new portal user, creating a party record and returning login credentials |
| `LoginUser` | `v_sDataModelCode`, `v_sUserID`, `v_sPassword`, `r_lPartyCnt`, optional output params | Authenticates a portal customer and returns their party counter |
| `LoginAgent` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_sUsername`, `v_sPassword`, `r_lAgentCnt`, `r_lPMUserID`, optional output params | Authenticates a back-office agent with full login details |
| `LogoffAgent` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_sUsername`, `r_vAdditionalDataArray?` | Logs off an agent, updating their last-login timestamp |
| `UpdateAgentLogonDetails` | `v_sDataModelCode`, `v_sBusinessTypeCode`, `v_sUsername`, `v_sPassword`, `v_sNewPassword`, `r_vAdditionalDataArray?` | Changes an agent's password |

---

## 4. Product Builder Components

### 4.1 Data Model Editor

These three interfaces together constitute the GIS data model editor — a tool for defining the
objects, properties and data types that form a GIS schema.

---

#### `iGISMaintainDataDictionary`

**Files:**
- Interface: `Product Builder\data model editor\Interface\iGISMaintainDataDictionary\iGISMaintainDataDictionaryInterface.vb` (23.9 KB)
- Form: `iGISMaintainDataDictionaryFrm.vb` (219.1 KB)

**Purpose:** Top-level data model editor. Browse, create, and edit GIS data models (schemas).
Launches child interfaces to manage individual objects and properties within each model.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISDataModelId` | RW | Integer | Primary key of the data model being edited |
| `GISDataModel` | RW | String | Data model code |
| `GISDataModelDescription` | RW | String | Data model display name |
| `SwiftIntegration` | WO | Boolean | Enables SWIFT messaging fields in the data model |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Components / Dependencies:**

| Component | Purpose |
|---|---|
| `bGISMaintainDataDictionary.Business` | All data model CRUD operations and validation |
| `bPMLock.User` | Record locking to prevent concurrent edits |
| `iGISObject.Interface_Renamed` | Launched as child to add/edit GIS objects |
| `iGISProperty.Interface_Renamed` | Launched as child to add/edit GIS object properties |
| `iPMUListMaint.Interface_Renamed` | Launched to maintain GIS list data from within the editor |

---

#### `iGISObject`

**Files:**
- Interface: `Product Builder\data model editor\Interface\iGISObject\iGISObjectInterface.vb` (24.6 KB)
- Form: `iGISObjectFrm.vb` (66.8 KB)

**Purpose:** Add, edit, or view a single GIS object definition within a data model. A GIS
*object* corresponds to a database table in the product schema.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISObjectId` | RW | Integer | Primary key of the object record |
| `GISDataModelId` | RW | Integer | Parent data model ID |
| `GISDataModel` | RW | String | Parent data model code |
| `ObjectName` | RW | String | Object code name |
| `TableName` | RW | String | Physical database table name |
| `MaxInstances` | RW | Object | Maximum number of object instances per risk (null = unlimited) |
| `IsQuoteObject` | RW | Boolean | If True, object data is stored per quote rather than per policy |
| `ParentObjectId` | RW | String | ID of this object's parent object (for hierarchy) |
| `PolarisObjectId` | RW | Object | Linked Polaris object ID |
| `IsSelectableForScreen` | RW | Boolean | Whether this object can be placed on a screen layout |
| `ObjectType` | RW | Integer | Object type code |
| `DataModelType` | RW | Integer | Data model type code |
| `EditFlags` | RW | Integer | Bitmask of allowed edit operations |
| `ParentCode` | RW | String | Display code for the parent object |
| `AllowedParents` | WO | Object(,) | 2D array of valid parent object choices |
| `SQLServerVersion` | WO | Integer | SQL Server version flag for compatible SQL generation |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** The object's business component is created by `iGISMaintainDataDictionary`
and passed in via the parent form.

---

#### `iGISProperty`

**Files:**
- Interface: `Product Builder\data model editor\Interface\iGISProperty\iGISPropertyInterface.vb` (33.2 KB)
- Form: `iGISPropertyFrm.vb` (126.8 KB)

**Purpose:** Add, edit, or view a single GIS property (column) definition on a GIS object.
This is the most property-rich interface in Product Builder.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISPropertyId` | RW | Integer | Primary key of the property record |
| `GISObjectId` | RW | Integer | Parent object ID |
| `PropertyName` | RW | String | Property code name |
| `ColumnName` | RW | String | Physical database column name |
| `DataType` | RW | Integer | Data type code (string, integer, date, list, etc.) |
| `IsInputProperty` | RW | Boolean | Whether the property accepts user input |
| `IsIdentifyingProperty` | RW | Boolean | Whether the property identifies an object instance |
| `IsPrimaryKey` | RW | Boolean | Whether this is the primary key column |
| `GISListId` | RW | Object | ID of the GIS list associated with this property (for list-type properties) |
| `PolarisPropertyId` | RW | Object | Linked Polaris property ID |
| `IsDeleted` | RW | CheckState | Soft-delete flag |
| `IsSearchProperty` | RW | CheckState | Whether this property appears in search screens |
| `SpecialsType` | RW | Integer | Code for special property behaviours (address, sum insured, etc.) |
| `EditFlags` | RW | Integer | Allowed edit bitmask |
| `IsChaseCycleProperty` | RW | CheckState | Whether this property participates in the chase cycle |
| `GISDataModelTypeID` | WO | Integer | Data model type for this property's parent |
| `GisDataModel` | WO | String | Parent data model code |
| `PMLookupTableName` | RW | Object | Name of a PM lookup table for this property |
| `PartyTypeId` | RW | Object | Party type this property is linked to |
| `PMUSumInsuredType` | RW | Object | Sum insured type code |
| `PMUStdWordingType` | RW | Object | Standard wording document type |
| `GISUserDefHeaderId` | RW | Object | User-defined header linked to this property |
| `PMUProductId` | RW | Object | Product link for product-specific properties |
| `IndexLinkingId` | RW | Integer | ID of an index-linking association |
| `PartyTypeArray` | RW | Array | Available party type values |
| `SumInsuredTypeArray` | RW | Array | Available sum insured type values |
| `DocumentFilterArray` | RW | Array | Available document filter types |
| `PMLookupList` | RW | Array | Available PM lookup table names |
| `GISUserDefHeaderArray` | RW | Array | Available user-defined headers |
| `ProductArray` | RW | Array | Available products |
| `IndexLinkingArray` | RW | Array | Available index-linking records |
| `IsNonGIS` | RW | Boolean | Flags a calculated/virtual property not stored in GIS |
| `GISProperty` | RW | Object | Full GIS property definition object |
| `IsInMISExport` | RW | Boolean | Whether this property is included in MIS exports |
| `IsFormattedText` | RW | Boolean | Whether this property uses formatted (RTF/HTML) text |
| `SwiftIntegration` | WO | Boolean | Enables SWIFT field mapping |
| `IsClaim360Display` | RW | Boolean | Whether this property is displayed in the Claims 360 view |
| `DisableClaim360Display` | RW | Boolean | Prevents the Claims 360 flag being changed on this property |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Created and passed in by the `iGISMaintainDataDictionary` parent form.

---

### 4.2 Screen Editor

The screen editor interfaces are used in the Product Builder to design and maintain the UI
layouts that appear at runtime inside `iPMUScreenControl`.

---

#### `iGISSumInsured`

**Files:**
- Interface: `Product Builder\screen editor\Interface\iGISSumInsured\iGISSumInsuredInterface.vb` (22.4 KB)
- Form: `iGISSumInsuredFrm.vb` (50.0 KB)

**Purpose:** Captures and edits sum insured details (description, value, dates, valuation
requirement) for a risk item on a product screen. This is a data-only interface — all values
are held in private members; it does not connect to a separate business component.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Description` | RW | Object | Description of the sum insured item |
| `Reference` | RW | Object | Reference field for the sum insured |
| `SumInsured` | RW | Object | The monetary sum insured value |
| `DateAdded` | RW | Object | Date this sum insured item was added |
| `DateDeleted` | RW | Object | Date this sum insured item was deleted |
| `IsValuationRequired` | RW | String | Whether a property valuation is required ("Y"/"N") |
| `ValuationDate` | RW | Object | Date of the last/next valuation |
| `IsValuation` | RW | Integer | Valuation flag integer code |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** None — data is stored entirely in the interface's private fields and
exposed through properties.

---

#### `iPMUListScreens`

**Files:**
- Interface: `Product Builder\screen editor\Interface\iPMUListScreens\iPMUListScreensInterface.vb` (21.0 KB)
- Form: `iPMUListScreensFrm.vb` (67.1 KB)

**Purpose:** Browse and select screen definitions from the Product Builder screen library.
Provides a list of available screens filtered by risk code. The calling process reads the
selection back from `RiskCodeId` / `RiskCodeDescription` / `RiskCodesList`.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `SwiftIntegration` | WO | Boolean | Include SWIFT screens in the listing |
| `RiskCodeId` | WO | Integer | Pre-selected risk code ID |
| `RiskCodeDescription` | WO | String | Pre-selected risk code description |
| `RiskCodesList` | RW | Object | Array of available risk codes to filter by |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Components:**

| Component | Purpose |
|---|---|
| `bSIRListScreen.Business` | Queries the screen library for available screens |
| `bPMLock.User` | Record locking during screen selection |

---

#### `iPMUMaintainScreenData`

**Files:**
- Interface: `Product Builder\screen editor\Interface\iPMUMaintainScreenData\iPMUMaintainScreenDataInterface.vb` (23.0 KB)
- Form: `iPMUMaintainScreenDataFrm.vb` (511.2 KB — largest form in Product Builder)

**Purpose:** Drag-and-drop screen layout editor for GIS product screens. Allows the user to
add GIS property fields and controls to a screen, configure their display properties, and
manage screen rules. This is the primary Product Builder design tool.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `ScreenType` | RW | Integer | Type code for the screen (risk, object, sub-screen, etc.) |
| `GISDMType` | WO | Integer | Data model type code for the product family |
| `ScreenId` | RW | Integer | Primary key of the screen being edited |
| `ScreenDesc` | RW | String | Screen display description |
| `SourceId` | RW | Integer | Portal/source identifier |
| `GISDataModelId` | RW | Integer | Parent data model ID |
| `GISDataModelCode` | RW | String | Parent data model code |
| `GISObjectId` | RW | Integer | Object within the data model this screen displays |
| `ParentId` | RW | Integer | Parent screen ID (for sub-screens) |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Components:**

| Component | Purpose |
|---|---|
| `bSIRMaintainScreenData.Business` | All screen layout CRUD operations, field placement, validation rules |
| `iPMURuleEditor.Interface_Renamed` | Launched as child to edit screen rules and expressions |

---

### 4.3 Screen Display

The screen display interfaces are the runtime rendering components used to display and capture
GIS product fields during policy transactions.

---

#### `iPMUAddress`

**Files:**
- Interface: `Product Builder\screen display\Interface\iPMUAddress\iPMUAddressInterface.vb` (32.0 KB)
- Form: `iPMUAddress.vb` (65.9 KB)

**Purpose:** Displays and captures a structured postal address on a risk or party screen.
Supports multiple address fields, usage type (correspondence, risk location, etc.),
and GIS accumulation zone IDs.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Task` | RO | Integer | Current task (view/add/edit) |
| `Navigate` | RO | Integer | Navigator button status |
| `ProcessMode` | RO | Integer | Current process mode |
| `TransactionType` | RO | String | Transaction type (NB/MTA/Renewal) |
| `EffectiveDate` | RO | Date | Transaction effective date |
| `StepStatus` | RO | String | Navigator step status |
| `AddressCnt` | RW | Integer | Address record counter (party address link) |
| `Address1` | RW | String | Address line 1 |
| `Address2` | RW | String | Address line 2 |
| `Address3` | RW | String | Address line 3 |
| `Address4` | RW | String | Address line 4 (town/city) |
| `PostalCode` | RW | String | Postcode |
| `Country` | RW | String | Country code |
| `Reference` | WO | String | Free-text reference field |
| `PostCode` | WO | String | Alias for PostalCode (WO variant for initial set) |
| `AddressUsageType` | RW | String | Address usage type code (e.g. correspondence, risk) |
| `AddressUsageTypeID` | RW | Integer | Address usage type ID |
| `AccumulationIds` | RW | Object | Array of GIS accumulation zone IDs for this address |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Initialises the object manager |
| `Dispose()` | — | Releases the object manager |
| `GetOption` | `v_iOptionNumber`, `r_sOptionValue` | Retrieves a named option value from the form configuration |
| `SetKeys` | `vKeyArray(,)` | Sets the input keys |
| `GetKeys` | `vKeyArray(,)` | Retrieves output keys |
| `GetSummary` | `vSummaryArray(,)` | Returns a summary display string |
| `SetProcessModes` | standard params | Sets task, navigation, and process mode |
| `SetStatus` | `sProcessStatus`, `sMapStatus`, `sStepStatus` | Overrides status values directly |
| `Start()` | — | Displays the address form |

**Business Component:** `bSIRAddress.Business`

---

#### `iPMURisk`

**Files:**
- Interface: `Product Builder\screen display\Interface\iPMURisk\Interface.vb` (119.5 KB)
- Support file: `iPMURisk.vb`

**Purpose:** The main risk entry and display form used during all policy transactions (new
business, MTA, renewal, claims). It coordinates the `iPMUScreenControl` engine for dynamic
GIS field display, and integrates claims sub-screens (loss schedule, financial summary) when
the transaction is claims-related.

**Key Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `Task` | RW | Integer | Current task (add/edit/view) |
| `Status` | RW | Integer | Form return status |
| `PartyCnt` | RW | Integer | Party counter of the insured |
| `ShortName` | RW | String | Party short name |
| `InsuranceFolderCnt` | RW | Integer | Insurance folder counter |
| `InsuranceFileCnt` | RW | Integer | Insurance file (quote/policy) counter |
| `RiskId` | RW | Integer | Risk record ID |
| `RiskTypeId` | RW | Integer | Risk type ID |
| `ProductId` | RW | Integer | Product ID |
| `ScreenId` | RW | Integer | GIS screen ID to render |
| `SubScreen` | RW | Boolean | Whether this is a sub-screen call |
| `ParentOIKey` | RW | String | Object instance key of the parent object |
| `ChildOIKey` | RW | String | Object instance key of the child object |
| `ParentObjectName` | RW | String | Parent GIS object code name |
| `ChildObjectName` | RW | String | Child GIS object code name |
| `GISObjectName` | RW | String | Current rendering object name |
| `GIS` | RW | Object | The current GIS data set (in-memory risk data) |
| `GISOriginal` | RW | Object | Original GIS data set snapshot (for change detection) |
| `ScreenValues` | RW | Object | Screen field values array |
| `IsRiAtRiskLevel` | RW | Boolean | Whether reinsurance operates at risk level |
| `IsAutoReinsured` | RW | Boolean | Whether this risk is auto-reinsured |
| `ClaimID` | RW | Integer | Claims: claim record ID |
| `PerilID` | RW | Integer | Claims: peril record ID |
| `ClaimPerilID` | RW | Integer | Claims: claim-peril link ID |
| `WorkClaimID` | RW | Integer | Claims: working claim ID |
| `WorkClaimPerilID` | RW | Integer | Claims: working claim-peril ID |
| `ClaimTransactionType` | RW | Integer | Claims: transaction type code |
| `ClaimInsFileCnt` | RW | Integer | Claims: insurance file counter |
| `ClaimRiskId` | RW | Integer | Claims: risk ID for the claim |
| `LossSchedule` | RW | Integer | Loss schedule ID |
| `LossScheduleTypeId` | RW | Integer | Loss schedule type |
| `PerilTypeId` | RW | Integer | Peril type ID |
| `XMLDataSet` | RO | Object | Exports the current GIS data set as XML |
| `ShowModeLessForm` | WO | Boolean | Whether to show the risk form as modeless |
| `CopyRisk` | WO | Boolean | Copies risk data from a source risk |
| `GISPolicyLinkID` | WO | Integer | Policy link ID for direct data set access |
| `SourceId` | RW | Short | Portal/source identifier |
| `CaseID` | RW | Integer | Claims case ID |
| `BaseCaseID` | RW | Integer | Base case ID for case hierarchy |
| `CaseNumber` | RW | String | Human-readable case number |
| `IsSilentQuote` | WO | Boolean | Suppresses UI during automated silent quoting |
| `ExceededReserve` | RW | Decimal | Amount by which the reserve limit was exceeded |
| `ReserveLimitExceeded` | RW | Boolean | Whether the reserve limit was exceeded |
| `List` | WO | Collection | Pre-loaded list data for screen controls |
| `vArray` | WO | Object() | Additional data array for screen initialisation |
| `ChildDataFromParent` | WO | Object | Child screen data passed from the parent call |
| `ChildIndex` | WO | Integer | Index of the child object instance |
| `RiskTypeDetails` | WO | Object | Risk type configuration details |
| `ObjectType` | RW | Integer | Object type code |
| `PMAuthorityLevel` | RW | Integer | Authority level bitmask |
| `CallingAppName` | RW | String | Calling application name |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Initialises the object manager |
| `Dispose()` | — | Releases the object manager |
| `Start()` | — | Displays the risk form |
| `GetKeys` | `vKeyArray(,)` | Retrieves output key/value pairs |
| `SetKeys` | `vKeyArray(,)` | Sets input key/value pairs |
| `GetSummary` | `vSummaryArray(,)` | Returns a risk summary string |
| `SetProcessModes` | standard params | Sets task/navigation/process mode |
| `EncodeTransactionScreenAndType` | `r_lEncoded`, `r_lTransactionType`, `r_lGISScreenId`, `r_lQuoteType` | Encodes transaction, screen, and quote type into a combined integer for screen routing |
| `GetTransactionType` | `r_llTransactionType` | Returns the current transaction type code from the data set |
| `GetPolicyTypeIDForInsFile` | `v_lInsuranceFileCnt`, `r_lPolicyTypeID` | Retrieves the policy type ID for a given insurance file counter |
| `SwitchTo()` | — | Brings the risk form window to the foreground if already open |

**Business Components / Dependencies:**

| Component | Purpose |
|---|---|
| `bPMUPolicy.Business` | Policy operations (bind, update, version) |
| `bCLMRiskDetails.Business` | Claims: risk detail retrieval and update |
| `iCLMLossSchedule.Interface_Renamed` | Claims: loss schedule sub-screen |
| `bOpenClaim.Business` | Claims: open claim operations |
| `iCLMFinSumm.Interface_Renamed` | Claims: financial summary sub-screen |
| `iPMBListEvents.Interface_Renamed` | Policy: list events for a risk |
| `iCLMInfoChklst.Interface_Renamed` | Claims: information checklist |
| `iPMWrkTaskInstance.Interface_Renamed` | Work manager: task instance screen |
| `iCLMCaseHistory.Interface_Renamed` | Claims: case history sub-screen |

---

#### `iPMURiskWrapper`

**File:** `Product Builder\screen display\Interface\iPMURiskWrapper\Interface.vb` (11.0 KB)

**Purpose:** A lightweight wrapper interface used by legacy back-office code that cannot directly
use `iPMURisk`. It accepts the same key risk identifiers and internally delegates to
`iPMURisk.Interface_Renamed` to display the risk form.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `Status` | RO | Integer | Return status after `Start` |
| `InsuranceFolderCnt` | WO | Integer | Insurance folder counter |
| `InsuranceFileCnt` | WO | Integer | Insurance file counter |
| `ProductId` | WO | Integer | Product ID |
| `RiskId` | WO | Integer | Risk record ID |
| `RiskTypeId` | WO | Integer | Risk type ID |
| `ScreenId` | WO | Integer | GIS screen ID to render |

**Methods:**

| Method | Description |
|---|---|
| `Initialise()` | Initialises the wrapper and the underlying `iPMURisk` interface |
| `Dispose()` | Releases resources |
| `Start()` | Delegates to `iPMURisk.Start()` |
| `SwithTo()` | [sic — typo in source] Brings the risk window to the foreground |

---

#### `iPMUScreenControl`

**Files:**
- Interface: `Product Builder\screen display\Interface\iPMUScreenControl\iPMUScreenControlInterface.vb` (137.1 KB)
- The form is embedded within the interface class itself, not a separate file

**Purpose:** The core dynamic GIS screen rendering engine. This is a Windows Forms `UserControl`
that dynamically builds and populates UI fields from a GIS screen definition at runtime.
It is embedded into risk forms (both within `iPMURisk` and within back-office components)
to display, validate, and capture GIS property values.

**Key Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `ScreenId` | RW | Integer | The GIS screen ID to render |
| `Screen` | RW | String | Screen code |
| `GISDataModelId` | RW | Integer | Data model ID for this screen's schema |
| `GISDataModel` | RW | String | Data model code |
| `SourceId` | RW | Integer | Portal/source identifier |
| `PartyCnt` | RW | Integer | Primary party counter |
| `ShortName` | RW | String | Party short name |
| `InsuranceFolderCnt` | RW | Integer | Insurance folder counter |
| `InsuranceFileCnt` | RW | Integer | Insurance file counter |
| `RiskId` | RW | Integer | Risk record ID |
| `RiskTypeId` | RW | Integer | Risk type ID |
| `ProductId` | RW | Integer | Product ID |
| `Task` | RW | Integer | Current task (add/edit/view) |
| `ObjectType` | RW | Integer | Type of object being displayed |
| `XMLDataSet` | RO | String | Current GIS data set serialised as XML |
| `ScreenValues` | RW | Object | Screen field value array (parallel with screen definition) |
| `GIS` | RW | Object | The live in-memory GIS data set |
| `FromEvent` | RW | Boolean | Whether this render was triggered by a screen event |
| `SubScreen` | RW | Boolean | Whether this is a sub-screen call |
| `ParentOIKey` | RW | String | Parent object instance key |
| `ChildOIKey` | RW | String | Child object instance key |
| `ParentObjectName` | RW | String | Parent GIS object code name |
| `ChildObjectName` | RW | String | Child GIS object code name |
| `GISObjectName` | RW | String | Current rendering GIS object name |
| `ReferReasons` | RW | String | Serialised refer reason string from rating |
| `DeclineReasons` | RW | String | Serialised decline reason string from rating |
| `Messages` | RW | String | Serialised messages from rating/rules |
| `QuoteType` | RW | String | Quote type code |
| `ChildAddStatus` | RO | Boolean | Whether a child add operation succeeded |
| `SwiftIntegration` | WO | Boolean | Enables SWIFT field mapping on the screen |
| `CopyRisk` | WO | Boolean | Whether to copy risk data from a source |
| `GISPolicyLinkID` | WO | Integer | Policy link ID for direct data set loading |
| `LossSchedule` | RW | Integer | Claims: loss schedule ID |
| `LossScheduleTypeId` | RW | Integer | Claims: loss schedule type |
| `PerilTypeId` | RW | Integer | Claims: peril type |
| `ClaimPerilID` | WO | Integer | Claims: claim-peril link ID |
| `PerilID` | WO | Integer | Claims: peril ID |
| `ClaimTransactionType` | WO | Integer | Claims: transaction type |
| `ClaimInsFileCnt` | WO | Integer | Claims: insurance file counter |
| `ClaimRiskId` | WO | Integer | Claims: risk ID |
| `ClaimId` | WO | Integer | Claims: claim ID |
| `CaseID` | WO | Integer | Claims: case ID |
| `ChildIndex` | WO | Integer | Index of the child object instance |

**Methods:**

| Method | Parameters | Description |
|---|---|---|
| `Initialise()` | — | Creates and initialises the object manager |
| `Dispose()` | — | Releases the object manager |
| `SetKeys` | `vKeyArray(,)` | Sets input keys |
| `GetKeys` | `vKeyArray(,)` | Retrieves output keys |
| `GetSummary` | `vSummaryArray(,)` | Returns a screen summary string |
| `SetProcessModes` | standard params | Sets task/navigation/process mode |
| `GetScreenDetails` | `r_vDataDictionary`, + optional field arrays | Returns the screen field definitions (labels, data types, list IDs, validation rules) from the GIS schema |
| `GetScreenValues` | `r_vScreenValues`, + optional params | Reads the current GIS property values into a screen values array |
| `Update` | `r_vScreenValues` | Writes screen values array back to the in-memory GIS data set |
| `DisplaySubScreen` | `lScreenId`, `sParentOIKey`, `sChildOIKey`, `sParentObjectName`, `sChildObjectName`, `r_vMyScreenValues`, `r_vSubScreenValues`, `vRiskTypeDetails`, `vData?`, `r_lStatus?`, `r_bReserveLimitExceeded?`, `r_dExceededReserve?` | Renders and displays a child sub-screen within the current risk context |
| `DelObjectInstance` | `sObjectName`, `sOIKey` | Deletes a GIS object instance (row) from the in-memory data set |
| `RunScreenRule` | `iPBCQemQuoteType`, `lScreenId`, `sChildOIKey`, `v_dtCoverStartDate?`, `lTransactionType?` | Executes a Product Builder screen rule expression to apply conditional field logic |
| `LoadGisFromScreenValues` | `r_vScreenValues` | Rebuilds the in-memory GIS data set from a screen values array |
| `RefreshScreenValuesFromGIS` | `r_vScreenValues` | Refreshes the screen values array from the current in-memory GIS data set |
| `SaveOnCancel` | `v_iTask`, `v_bRevertBackRisk` | Determines what to save or discard when the user cancels the screen |
| `DeleteQuote` | `v_lInsuranceFileCnt` | Deletes the current quote/policy record from the database on explicit user cancellation |

**Business Components:**

| Component | Purpose |
|---|---|
| `bGISPMUExtras.Business` | Screen-level GIS extras: code/description resolution, accumulation, index linking |
| `bSIRRiskGroup.Business` | Risk group operations for Risk Group screens |
| `bSIRRiskScreen.Business` | Screen runtime operations, rule execution, field validation |
| `bSIRRiskScreen.Stateless` | Stateless variant of screen operations for read-only resolution calls |

---

### 4.4 3D Rating

The 3D Rating interfaces support the scheme-level rating list editor and group import tools for
actuarial rate table management.

---

#### `iGISRating`

**Files:**
- Interface: `Product Builder\3D Rating\Rating\Interface\iGISRating\Interface.vb`

**Purpose:** Main entry point for the 3D Rating scheme editor. Allows users to view and maintain
3D rating structures (list-based premium tables) for a given scheme.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISSchemeID` | RW | Integer | The rating scheme being edited |

**Standard Methods:** `Initialise`, `Dispose`, `Start`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`

Additional: `DefaultInstance` — returns the singleton instance of this interface.

**Business Component:** `bGISRating.Business`

---

#### `GroupImport` (interface class: `Interface_Renamed`)

**File:** `Product Builder\3D Rating\Group Import\Interface\Interface.vb`

**Purpose:** Imports group rating data from an external source into the 3D rating scheme structures.

**Struct:** `GroupRecord` — defines a single group record; has a `CreateInstance()` factory method.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | WO | String | Calling application name |
| `PMAuthorityLevel` | WO | Integer | Authority level |
| `Status` | RO | Integer | Return status |

**Methods:**

| Method | Description |
|---|---|
| `Initialise()` | Creates object manager |
| `Dispose()` | Releases object manager |
| `Start()` | Shows the import form |
| `SetProcessModes` | Sets mode flags |
| `SetKeys` / `GetKeys` | Input/output key transfer |
| `DoImport()` | Executes the group import operation; returns Boolean success |

**Business Component:** `bGisGroupImport.Business`

---

#### `iGISListGrouping`

**Files:**
- Interface: `Product Builder\3D Rating\ListGrouping\Interface\iGISListGrouping\Interface.vb`
- Forms: `frmMain.vb`, `frmGroupSummary.vb`, `frmItems.vb`

**Purpose:** Maintains the grouping definitions used to aggregate list codes within a 3D rating
scheme. Groups control how individual list values are associated with rating cells.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISSchemeID` | RW | Integer | The scheme whose list groupings are being maintained |

**Standard Methods:** `Initialise`, `Dispose`, `Start`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`

**Business Component:** `bGISListGrouping.Business`

---

#### `iGISListMaint` (3D Rating ListImport)

**Files:**
- Interface: `Product Builder\3D Rating\ListImport\iGISListMaint\iGISListMaint.vb`
- Supporting classes: `iGISColumns.vb`, `iGISImport.vb`, `iGISListType.vb`, `iGISMap.vb`, `iGISNewListType.vb`, `iGISView.vb`

**Purpose:** Import, map, and maintain GIS list data files for 3D rating schemes. Supports
defining column mappings, list types, and import transformations.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `CallingAppName` | RW | String | Calling application name |
| `PMAuthorityLevel` | RW | Integer | Authority level |
| `Status` | RW | Integer | Return status |

**Standard Methods:** `Initialise`, `Dispose`, `Start`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`

**Supporting Classes:**
- `iGISColumns` — defines column structure for an import file
- `iGISImport` — manages the import execution
- `iGISListType` — defines the list type for the import
- `iGISMap` — field mapping configuration between import columns and GIS list codes
- `iGISNewListType` — creates a new list type definition
- `iGISView` — read-only view of the imported list

---

### 4.5 Rule Editor — `iPMURuleEditor`

**Files:**
- Interface: `Product Builder\ruleeditor\Interface\iPMURuleEditor\iPMURuleEditor.vb`
- Public field: `frmInterface As frmInterface` (direct form reference exposed)

**Purpose:** Expression rule editor for Product Builder screen rules. Used as a child interface
launched from `iPMUMaintainScreenData` to create and edit conditional rules that drive screen
behaviour (visibility, mandatory fields, default values, validation).

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `EffectiveDate` | RW | Date | Date from which the rule is effective |
| `RuleFileName` | RW | String | Name of the rule file |
| `RulePath` | RW | String | File system path to the rules directory |
| `FixedFile` | RW | Boolean | Whether the rule file is fixed (read-only) |
| `DataModelCode` | RW | String | GIS data model code for property lookups |
| `DataModelId` | RW | Integer | GIS data model ID |
| `SchemeID` | WO | Integer | Rating scheme ID (for scheme-specific rules) |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Component:** Self-contained rule editor; rule files are managed on the file system
rather than through a dedicated business component.

---

### 4.6 Rule Lookup

These two interfaces provide a UI for looking up the rule definitions and data values used by
the Product Builder rule engine.

---

#### `iPMURuleLookupData`

**Files:**
- Interface: `Product Builder\rulelookup\Interface\iPMURuleLookupData\iPMURuleLookupDataInterface.vb` (19.9 KB)
- Form: `iPMURuleLookupData.vb`

**Purpose:** Displays the data value table for a specific rule lookup key — allows the user to
see which codes map to which values in rule expressions.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `LookupKey` | WO | Integer | The rule lookup key whose data values are displayed |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

Additional: `DefaultInstance` — singleton accessor.

**Business Component:** `bSIRRuleLookup.Business`

---

#### `iPMURuleLookupHeader`

**Files:**
- Interface: `Product Builder\rulelookup\Interface\iPMURuleLookupHeader\iPMURuleLookupHeaderInterface.vb` (20.3 KB)
- Form: `iPMURuleLookupHeader.vb`

**Purpose:** Displays the list of all rule lookup definitions (headers) — allows the user to
find and select a rule lookup entry for use in a rule expression.

**Properties:** No domain-specific properties beyond the standard set.

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

Additional: `DefaultInstance` — singleton accessor.

**Business Component:** `bSIRRuleLookup.Business`

---

### 4.7 List Maintenance — `iPMUListMaint`

**Files:**
- Interface: `Product Builder\list maintenance\iPMUListMaint\iPMUListMaintInterface.vb`
- Public field: `frmInterface As Object`
- Forms: `iPMUListMaintain.vb`, `iPMUListSel.vb`

**Purpose:** GIS list data maintenance — add, edit, import, and validate list entries used
by list-type GIS properties across all products. Supports filtering by data model and task mode.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `GISDataModelCode` | RW | String | Filters the list maintenance to a specific data model |
| `Task` | RW | Integer | Current task (view/add/edit) |
| `Navigate` | RO | Integer | Navigator button status |
| `ProcessMode` | RO | Integer | Current process mode |
| `TransactionType` | RO | String | Transaction type |
| `EffectiveDate` | RO | Date | Effective date context |
| `StepStatus` | RO | String | Navigator step status |

**Standard Methods:** `Initialise`, `Dispose`, `SetKeys`, `GetKeys`, `GetSummary`, `SetProcessModes`, `Start`

**Business Components:**

| Component | Purpose |
|---|---|
| `bPMUList.bPMUListCreate` | Creates and updates PMU list data records |
| `bGISListManager.Form` | GIS list data retrieval and code validation |

---

## 5. User Controls

### 5.1 `iPBMaskedInputBox`

**File:** `Product Builder\screen display\User Controls\iPBMaskedInputBox\Interface.vb`

**Class:** `Interface_Renamed` (Windows Forms `UserControl`)

**Purpose:** A modal popup dialog for capturing sensitive or formatted user input on a GIS screen.
Displays a title-bar caption, a labelled input box, and a masked input field so that the entered
text is not shown on screen (e.g. for passwords or secure reference codes). Designed for use
by `iPMUScreenControl` when a screen field is flagged as requiring masked input.

**Properties:**

| Property | Direction | Type | Description |
|---|---|---|---|
| `TitleBarCaption` | RW | String | The text shown in the dialog title bar |
| `MaskedInputBoxCaption` | RW | String | The label displayed above the masked input field |
| `InputCaptured` | RO | String | The text entered by the user after the dialog is dismissed |

**Methods:**

| Method | Returns | Description |
|---|---|---|
| `DisplayMaskedInputBox()` | `gPMConstants.PMEReturnCode` | Displays the modal masked-input dialog and blocks until the user submits or cancels; returns `PMTrue` if submitted, `PMFalse` if cancelled |

---

## 6. File Inventory

### GIS Area (`GIS Combined\GIS\Components\`)

| Component | Folder | Key Files |
|---|---|---|
| iGISUserDefDetail | `GIS User Def Detail\Interface\iGISUserDefDetail\` | `iGISUserDefDetailInterface.vb`, `iGISUserDefDetailFrm.vb` |
| iGISUserDefDetailRate | `GIS User Def Detail\Interface\iGISUserDefDetailRate\` | `iGISUserDefDetailRateInterface.vb`, `iGISUserDefDetailRateFrm.vb` |
| iGISUserDefDetailRates | `GIS User Def Detail\Interface\iGISUserDefDetailRates\` | `iGISUserDefDetailRatesInterface.vb`, `iGISUserDefDetailRatesFrm.vb` |
| iGISUserDefDetails | `GIS User Def Detail\Interface\iGISUserDefDetails\` | `iGISUserDefDetailsInterface.vb`, `iGISUserDefDetails.vb` |
| iGISUserDefHeader | `GIS User Def Header\Interface\iGISUserDefHeader\` | `iGISUserDefHeaderInterface.vb`, `iGISUserDefHeaderFrm.vb` |
| iGISUserDefHeaderRate | `GIS User Def Header\Interface\iGISUserDefHeaderRate\` | `iGISUserDefHeaderRateInterface.vb`, `iGISUserDefHeaderRateFrm.vb` |
| iGISUserDefHeaderRates | `GIS User Def Header\Interface\iGISUserDefHeaderRates\` | `iGISUserDefHeaderRatesInterface.vb`, `iGISUserDefHeaderRatesFrm.vb` |
| iGISUserDefHeaders | `GIS User Def Header\Interface\iGISUserDefHeaders\` | `iGISUserDefHeadersInterface.vb`, `iGISUserDefHeadersFrm.vb` |
| iGISSchemeProperties | `Insurer Scheme\Scheme Properties\iGISSchemeProperties\` | `Properties.vb`, `Propertys.vb` |
| iGISListManager | `List Management\Client\iGISListManager\` | `iGISListManagerCls.vb`, `iGISListManagerCommon.vb`, `iGISListManagerNoLogon.vb` |
| SellerTool | `Core GIS\SellerTool\` | `iGISSellerToolApplication.vb`, `iGISSellerToolFinancial.vb`, `iGISSellerToolQuotePolicy.vb`, `iGISSellerToolRenewals.vb`, `iGISSellerToolSecurity.vb` |

### Product Builder (`GIS Combined\Product Builder\`)

| Component | Folder | Key Files |
|---|---|---|
| iGISMaintainDataDictionary | `data model editor\Interface\iGISMaintainDataDictionary\` | `iGISMaintainDataDictionaryInterface.vb`, `iGISMaintainDataDictionaryFrm.vb` |
| iGISObject | `data model editor\Interface\iGISObject\` | `iGISObjectInterface.vb`, `iGISObjectFrm.vb` |
| iGISProperty | `data model editor\Interface\iGISProperty\` | `iGISPropertyInterface.vb`, `iGISPropertyFrm.vb` |
| iGISRating | `3D Rating\Rating\Interface\iGISRating\` | `Interface.vb` |
| GroupImport | `3D Rating\Group Import\Interface\` | `Interface.vb` |
| iGISListGrouping | `3D Rating\ListGrouping\Interface\iGISListGrouping\` | `Interface.vb`, `frmMain.vb`, `frmGroupSummary.vb`, `frmItems.vb` |
| iGISListMaint (3D) | `3D Rating\ListImport\iGISListMaint\` | `iGISListMaint.vb`, `iGISColumns.vb`, `iGISImport.vb`, `iGISListType.vb`, `iGISMap.vb`, `iGISNewListType.vb`, `iGISView.vb` |
| iPMURuleEditor | `ruleeditor\Interface\iPMURuleEditor\` | `iPMURuleEditor.vb` |
| iPMURuleLookupData | `rulelookup\Interface\iPMURuleLookupData\` | `iPMURuleLookupDataInterface.vb` |
| iPMURuleLookupHeader | `rulelookup\Interface\iPMURuleLookupHeader\` | `iPMURuleLookupHeaderInterface.vb`, `iPMURuleLookupHeader.vb` |
| iPMUListMaint | `list maintenance\iPMUListMaint\` | `iPMUListMaintInterface.vb`, `iPMUListMaintain.vb`, `iPMUListSel.vb` |
| iGISSumInsured | `screen editor\Interface\iGISSumInsured\` | `iGISSumInsuredInterface.vb`, `iGISSumInsuredFrm.vb` |
| iPMUListScreens | `screen editor\Interface\iPMUListScreens\` | `iPMUListScreensInterface.vb`, `iPMUListScreensFrm.vb` |
| iPMUMaintainScreenData | `screen editor\Interface\iPMUMaintainScreenData\` | `iPMUMaintainScreenDataInterface.vb`, `iPMUMaintainScreenDataFrm.vb` |
| iPMUAddress | `screen display\Interface\iPMUAddress\` | `iPMUAddressInterface.vb`, `iPMUAddress.vb` |
| iPMURisk | `screen display\Interface\iPMURisk\` | `Interface.vb`, `iPMURisk.vb` |
| iPMURiskWrapper | `screen display\Interface\iPMURiskWrapper\` | `Interface.vb` |
| iPMUScreenControl | `screen display\Interface\iPMUScreenControl\` | `iPMUScreenControlInterface.vb` |
| iPBMaskedInputBox | `screen display\User Controls\iPBMaskedInputBox\` | `Interface.vb` |
