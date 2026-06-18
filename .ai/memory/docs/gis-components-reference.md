# GIS Combined — Business Components Reference

**Source path:** `GIS Combined\`
**Sub-modules:** `GIS\` (core GIS components) · `Product Builder\` (product configuration and rating)
**Component prefixes:** `bGIS*` · `bPMU*` · `bSIR*` · `bPB*`
**Language:** VB.NET · .NET Framework 4.8
**Last reviewed:** March 2026

---

## Overview

The **GIS Combined** module provides the Generic Insurance System (GIS) — a data-model-driven insurance product engine. It consists of two sub-modules:

- **GIS** — core server-side components handling policy/party/claim lifecycle, renewals, EDI, and user-defined lookup management
- **Product Builder** — design-time tooling for configuring screens, data dictionaries, rating tables, and rule lookups

GIS is built around a pluggable **Back Office Mapper (BOM)** pattern. The `bGIS` façade classes dispatch all product-specific logic to `bGISBOM{DataModelCode}.*` components looked up and created at runtime via `CreateBOM()`.

---

## Architecture

```
GIS Web Portal / SAM WCF Layer
  └─► bGIS  (Core GIS façade — STS, QuotePolicy, Security, Renewals, Financial classes)
        └─► bGISBOM{DataModelCode}.{ClassName}   (product-specific BOM — late-bound via CreateBOM)
        └─► bGISSchemeBusiness                   (scheme/group/property configuration)
        └─► bGISUserDefDetail / Header            (user-defined lookup definitions)
        └─► bGISUserDefLookup                     (runtime lookup value retrieval)
        └─► bGISListManager                       (ABI/lookup list cache)

Product Builder (design-time)
  └─► bGISMaintainDataDictionary   (data model object/property editor)
  └─► bSIRMaintainScreenData       (GIS screen header + field layout editor)
  └─► bSIRListScreen               (screen list viewer)
  └─► bSIRRiskScreen               (risk screen load/save at runtime)
  └─► bGISRating                   (3D rating matrix)
  └─► bGISListGrouping             (rating list grouping)
  └─► bGISListMaint                (list type and entry maintenance)
  └─► bGISPMUExtras                (scripting helper — party/claim/currency/tax)
  └─► bSIRRuleLookup               (rule lookup header + data editor)
  └─► bPBFindControl               (find control definition editor)
  └─► bPMUList                     (binary RLDF list file generation)
  └─► bGISAddOn                    (add-on product administration and rating)
  └─► bGISPromptInterface          (Prompt premium finance gateway integration)
  └─► bGISLookupManager            (binary lookup cache file builder)
```

---

## Project Inventory

### GIS Sub-module

| # | Directory | Project | Purpose |
|---|-----------|---------|---------|
| 1 | `GIS\Components\Core GIS\Server\bGIS` | bGIS | Core GIS façade — dispatches policy, security, financial, and renewal ops to product-specific BOM components |
| 2 | `GIS\Components\Core GIS\Server\bGISAddOn` | bGISAddOn | Add-on product admin (CRUD) and premium/charge calculation |
| 3 | `GIS\Components\GIS User Def Detail\Business\bGISUserDefDetail` | bGISUserDefDetail | Manages individual code/description rows within a user-defined lookup header |
| 4 | `GIS\Components\GIS User Def Detail\Business\bGISUserDefDetailRates` | bGISUserDefDetailRates | Rate and indicator values attached to individual lookup detail rows |
| 5 | `GIS\Components\GIS User Def Header\Business\bGISUserDefHeader` | bGISUserDefHeader | Manages named GIS lookup table headers (e.g. OCCUPATION, VEHICLE_TYPE) |
| 6 | `GIS\Components\GIS User Def Header\Business\bGISUserDefHeaderRates` | bGISUserDefHeaderRates | Header-level rates and indicators for user-defined lookup tables |
| 7 | `GIS\Components\Insurer Scheme\Scheme Business\bGISSchemeBusiness` | bGISSchemeBusiness | Full scheme configuration domain — schemes, groups, properties, COBOL linkage, payment types |
| 8 | `GIS\Components\LookupManagement\bGISLookupManager` | bGISLookupManager | Builds binary lookup cache files from `gis_lookup_header` / `gis_lookup_data` tables |
| 9 | `GIS\Components\Prompt Integration\bGISPromptInterface` | bGISPromptInterface | Prompt premium finance gateway integration — XML request/response builder |
| 10 | `GIS\Components\User Defined Lookups\Business\bGISUserDefLookup` | bGISUserDefLookup | Runtime lookup value retrieval with Enterprise Library in-memory caching |

### Product Builder Sub-module

| # | Directory | Project | Purpose |
|---|-----------|---------|---------|
| 11 | `Product Builder\3D Rating\ListGrouping\Business\bGISListGrouping` | bGISListGrouping | GIS list groupings — create, update, auto-assign items to named groups within list types |
| 12 | `Product Builder\3D Rating\ListImport\bGISListMaint` | bGISListMaint | List type and entry maintenance — create types, import CSV, add/update entries, manage UDL data |
| 13 | `Product Builder\3D Rating\Rating\Business\bGISRating` | bGISRating | 3D rating matrix management — rate types with up to 3 list-type axes and matrix values |
| 14 | `Product Builder\data model editor\Business\bGISMaintainDataDictionary` | bGISMaintainDataDictionary | GIS data model dictionary editor — objects, properties, screen layouts, QEM usage, SP generation |
| 15 | `Product Builder\gis pmu extras\Business\bGISPMUExtras` | bGISPMUExtras | Core GIS runtime helper for product scripts — party, claim, currency, ATS tax, reserves, payments |
| 16 | `Product Builder\list maintenance\bPMUList` | bPMUList | Generates binary RLDF list files (.DAT/.IDX) from a text input file |
| 17 | `Product Builder\rulelookup\Business\bSIRRuleLookup` | bSIRRuleLookup | Maintains GIS rule lookup header and data rows — CRUD for rule-to-value mapping tables |
| 18 | `Product Builder\screen display\Business\bSIRRiskScreen` | bSIRRiskScreen | Risk screen load and save during policy underwriting; stateful and stateless (XML RPC) variants |
| 19 | `Product Builder\screen editor\Business\bPBFindControl` | bPBFindControl | Find/lookup control definitions — configures which DB view drives a lookup and column mappings |
| 20 | `Product Builder\screen editor\Business\bSIRListScreen` | bSIRListScreen | Retrieves and updates GIS screen header records for the screen list view |
| 21 | `Product Builder\screen editor\Business\bSIRMaintainScreenData` | bSIRMaintainScreenData | Full CRUD for GIS screen definitions — headers, field layout, child screens, screen copy with .Rul files |

---

## Component Reference

### 1. bGIS

**Directory:** `GIS\Components\Core GIS\Server\bGIS`
**VB Source Files:** `Financial.vb`, `QuotePolicy.vb`, `Security.vb`, `Renewals.vb`, `STS.vb`, `SQL.vb`, `GISActionXMLFuncs*.vb`, `GISAdditionalXMLFuncs.vb`, `MainModule.vb`, `clsPremiumFinance.vb`
**Purpose:** Core GIS entry-point façade. Financial/QuotePolicy/Security classes delegate all product-specific logic to `bGISBOM{DataModelCode}.*` components created at runtime via `CreateBOM()`. Renewals and STS classes have direct database interaction. Six GISActionXMLFuncs* modules handle XML serialisation/deserialisation of action requests.

#### Class: Financial

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init | — | `gPMComponentServices.CheckDatabase` |
| `Dispose` | `() As void` | Standard cleanup | — | — |
| `DataCash` | `(dataModelCode, datacashParams...) As Integer` | Processes a Datacash card payment transaction | `spu_gis_policy_link_qte_ref_upd` | `bGISTemp.UpdatePolicyLinkTransact`, Datacash COM |
| `BankAccountValidation` | `(dataModelCode, businessTypeCode, quoteRef, bankParams...) As Integer` | Validates a bank account via external service | — | External bank validation COM |
| `CalcPaymentMethodCharge` | `(productFamily, businessTypeCode, dataModelCode, transactionParams...) As Integer` | Calculates payment method charges (APR, instalments) | — | Payment method charge COM |
| `PremiumFinanceQuote` | `(dataModelCode, businessTypeCode, businessParams...) As Integer` | Retrieves premium finance instalment quote | — | PremiumFinance COM |
| `GetDataModelDef` | `(dataModelCode) As Integer` | Retrieves data model definition settings | — | GISSharedConstants |
| `GetInsurerABICode` | `(polarisInsurerNo, r_sABICode ByRef) As Integer` | Looks up insurer ABI code from Polaris insurer number | — | GISSharedConstants |

#### Class: QuotePolicy

All methods delegate to `bGISBOM{DataModelCode}.QuotePolicy` via `CreateBOM()`.

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init | — | `gPMComponentServices.CheckDatabase` |
| `AddParty` | `(dataModelCode, businessTypeCode, partyType, personalDetails...) As Object` | Creates a new party/client record | — | `bGISBOM{DMC}.QuotePolicy.AddParty` |
| `FindQuote` | `(dataModelCode, businessTypeCode, r_vResultArray ByRef, [filters]) As Integer` | Searches for existing quotes | — | `bGISBOM{DMC}.QuotePolicy.FindQuote` |
| `GetQuotesForParty` | `(dataModelCode, businessTypeCode, partyCnt, r_vQuoteArray ByRef) As Integer` | Returns all quotes for a party | — | `bGISBOM{DMC}.QuotePolicy.GetQuotesForParty` |
| `GetQuotesPoliciesForParty` | `(dataModelCode, businessTypeCode, partyCnt, searchType, r_vArray ByRef) As Integer` | Returns quotes and policies for a party | — | `bGISBOM{DMC}.QuotePolicy.GetQuotesPoliciesForParty` |
| `GetPolicyVersions` | `(dataModelCode, businessTypeCode, r_vArray ByRef, [fileCnt or fileRef]) As Integer` | Returns all versions of a policy | — | `bGISBOM{DMC}.QuotePolicy.GetPolicyVersions` |
| `FindParty` | `(dataModelCode, businessTypeCode, partyType, name, postcode..., r_vResultArray ByRef) As Integer` | Searches for parties by name/postcode/user criteria | — | `bGISBOM{DMC}.QuotePolicy.FindParty` |
| `FindPolicy` | `(dataModelCode, businessTypeCode, r_vResultArray ByRef, [additionalData]) As Integer` | Finds policies using additional data criteria | — | `bGISBOM{DMC}.QuotePolicy.FindPolicy` |
| `GetProductByAgent` | `(dataModelCode, businessTypeCode, agentPartyCnt, r_vResultArray ByRef) As Integer` | Returns products available for an agent | — | `bGISBOM{DMC}.QuotePolicy.GetProductByAgent` |
| `GetParty` | `(dataModelCode, businessTypeCode, partyCnt, [out party detail params]) As Integer` | Retrieves full party details | — | `bGISBOM{DMC}.QuotePolicy.GetParty` |
| `GetQuotes` | `(dataModelCode, businessTypeCode, partyCnt, r_vQuoteArray ByRef) As Integer` | Returns quotes for a party | — | `bGISBOM{DMC}.QuotePolicy.GetQuotes` |
| `GetQuoteDetails` | `(dataModelCode, businessTypeCode, insuranceFileCnt, r_vQuoteArray ByRef) As Integer` | Retrieves full quote details | — | `bGISBOM{DMC}.QuotePolicy.GetQuoteDetails` |
| `GetQuoteDetailsSBO` | `(dataModelCode, businessTypeCode, insuranceFileCnt, r_vQuoteArray ByRef) As Integer` | Retrieves SBO-level quote details | — | `bGISBOM{DMC}.QuotePolicy.GetQuoteDetailsSBO` |
| `GetQuoteRisks` | `(dataModelCode, businessTypeCode, insuranceFileCnt, r_vQuoteArray ByRef) As Integer` | Returns risk rows for a given insurance file | — | `bGISBOM{DMC}.QuotePolicy.GetQuoteRisks` |
| `GetRatingDetails` | `(dataModelCode, businessTypeCode, folderCnt, fileCnt, riskCnt, r_vSections ByRef) As Integer` | Retrieves rating section details for a risk | — | `bGISBOM{DMC}.QuotePolicy.GetRatingDetails` |
| `UpdateParty` | `(dataModelCode, businessTypeCode, partyUpdateParams...) As Integer` | Updates party contact/address details | — | `bGISBOM{DMC}.QuotePolicy.UpdateParty` |
| `ProcessAccounts` | `(dataModelCode, businessTypeCode, insuranceFileCnt, transactionType, [MTAparams]) As Integer` | Processes accounting transactions for a policy | — | `bGISBOM{DMC}.QuotePolicy.ProcessAccounts` |
| `GetSchemeList` | `(dataModelCode, businessTypeCode, r_vResultArray ByRef) As Integer` | Returns list of available schemes | — | `bGISBOM{DMC}.QuotePolicy.GetSchemeList` |
| `GetRiskByProduct` | `(dataModelCode, businessTypeCode, productID, r_vResultArray ByRef) As Integer` | Returns risks linked to a product | — | `bGISBOM{DMC}.QuotePolicy.GetRiskByProduct` |

#### Class: Security

All methods delegate to `bGISBOM{DataModelCode}.Security` via `CreateBOM()`.

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init | — | `gPMComponentServices.CheckDatabase` |
| `RegisterUser` | `(dataModelCode, businessTypeCode, personalDetails..., r_sUserID ByRef, r_sPassword ByRef, r_lPartyCnt ByRef) As Integer` | Registers a new online user/party | — | `bGISBOM{DMC}.Security.RegisterUser` |
| `LoginUser` | `(dataModelCode, businessTypeCode, userID, password, [out: partyCnt, PMUserID, details]) As Integer` | Authenticates an online end user | — | `bGISBOM{DMC}.Security.LoginUser` |
| `LoginAgent` | `(dataModelCode, businessTypeCode, username, password, [out: agent details]) As Integer` | Authenticates a broker/agent user | — | `bGISBOM{DMC}.Security.LoginAgent` |
| `LogoffAgent` | `(dataModelCode, businessTypeCode, username) As Integer` | Logs off a broker/agent session | — | `bGISBOM{DMC}.Security.LogoffAgent` |
| `UpdateAgentLogonDetails` | `(dataModelCode, businessTypeCode, username, password, newPassword) As Integer` | Updates agent password | — | `bGISBOM{DMC}.Security.UpdateAgentLogonDetails` |
| `ForgottenPassword` | `(dataModelCode, businessTypeCode, username, ipAddress, emailParams...) As Integer` | Resets and emails new password to user | — | `bGISBOM{DMC}.Security.ForgottenPassword` |

#### Class: Renewals

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Long` | Standard init; creates sub-components | — | `bGISSchemeBusiness.Business`, `bGIS.Application` |
| `PreRenSelection` | `(folderCnt, partyCnt, renewalDate, riskCodeID, dataModelCode, schemeID, productID) As Integer` | Pre-renewal selection step | — | `bGISBOM{DMC}.Renewals.PreRenSelection`, `bSIRIUSLink.Renewals` |
| `RenSelection` | `(folderCnt, partyCnt, renewalDate, riskCodeID, r_sDataModelCode ByRef) As Integer` | Renewal selection step; loads GIS dataset, calls BOM for quotation | `spu_gis_policy_link_qte_ref_upd` | `bGISBOM{DMC}.Renewals`, `bGISSchemeBusiness` |
| `RenMtaAtRenewal` | `(bTransact, schemeID, policyLinkID, premiumValues...) As Integer` | Checks and processes MTAs at renewal time | — | `bGISSchemeBusiness`, `bGIS.Application` |
| `RenQuotationInsurerLead` | `(folderParams, r_lRenewalInsuranceFileCnt ByRef, schemeID, productID, dataModelCode) As Integer` | Produces insurer-led renewal quotation | `spu_SirRen_Copy_Quote_Ins`, `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenQuotationBrokerLead` | `(folderParams, dataModelCode, schemeID, r_lRenewalInsuranceFileCnt ByRef) As Integer` | Produces broker-led renewal quotation | `spu_SirRen_Copy_Quote_Ins`, `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenInvitationInsurerLed` | `(renewalParams...) As Integer` | Sends insurer-led renewal invitation documents | `spu_STS_GetEDICommonData` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenReprintInvitationInsurerLed` | `(renewalParams...) As Integer` | Reprints insurer-led renewal invitation | `spu_STS_GetEDICommonData` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenInvitationEDI` | `(renewalParams...) As Integer` | Sends EDI renewal invitation message | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenInvitationBrokerLed` | `(renewalParams...) As Integer` | Sends broker-led renewal invitation | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenMultipleInvitationBrokerLed` | `(dataModelCode, businessTypeCode, selectedArray, r_vFailedArray ByRef) As Integer` | Batch broker-led renewal invitations | — | `RenInvitationBrokerLed` (loop) |
| `RenReprintInvitationBrokerLead` | `(renewalParams...) As Integer` | Reprints broker-led renewal invitation | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenInvitePreferredQuotes` | `(renewalParams...) As Integer` | Invites preferred insurer quotes for renewal | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenConfDocsHoldingInsurer` | `(renewalParams...) As Integer` | Sends confirmation docs for holding insurer renewal | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `ConfirmRenewal` | `(folderCnt, fileCnt, schemeID, partyCnt, dataModelCode, businessTypeCode) As Integer` | Confirms (binds) a renewal policy | `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `ConfirmRenewalBrokerLed` | `(renewalParams including policyBinderID, policyID) As Integer` | Confirms broker-led renewal | `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `ConfirmLapse` | `(folder/file/scheme/party params, dataModelCode, businessTypeCode) As Integer` | Confirms lapse of a renewal policy | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenCompLapse` | `(renewalParams, [renewalStatusCode, oldInsuranceCnt]) As Integer` | Completes renewal lapse process | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenCompHoldingInsurer` | `(renewalParams...) As Integer` | Completes holding-insurer renewal | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenCompAlternateInsurer` | `(renewalParams, businessTypeID, r_lNewInsuranceFileCnt ByRef) As Integer` | Completes alternate-insurer renewal | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `CreatePolicy` | `(oldFileCnt, partyCnt, policyRef, dataModelCode, businessTypeID, schemeID, [out IDs]) As Integer` | Creates a new renewal policy record | `spu_SirRen_Copy_Quote_Ins`, `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenGetInsurerQuoteOptions` | `(schemes, coverCode, r_vGridLayout ByRef) As Integer` | Returns insurer quote options grid for renewal UI | `spu_GIS_Insurer_GetQuoteOptions` | — |
| `RenGetPassword` | `(dataModelCode, businessTypeCode, insuranceFileCnt, r_sPassword ByRef) As Integer` | Retrieves unencrypted renewal password | — | `bGISBOM{DMC}.Renewals` |
| `GetPolicyRenewalVersion` | `(dataModelCode, businessTypeCode, folderCnt, r_vResultArray ByRef) As Integer` | Gets renewal version details | — | `bGISBOM{DMC}.Renewals` |
| `RenReminder` | `(folderCnt, partyCnt, renInsFileCnt, dataModelCode, [schemeID, batchRun]) As Integer` | Sends renewal reminder | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenReprintConfirm` | `(dataModelID, dataModelCode, r_lRenewalFileCnt ByRef, ediAuditId) As Integer` | Reprints renewal confirmation documents | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenResendEDI` | `(dataModelID, dataModelCode, r_lRenewalFileCnt ByRef, ediAuditId) As Integer` | Resends an EDI renewal message | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `WhatIfQuote` | `(insuranceFileCnt, dataModelCode, schemeID, r_lNewPolicyLinkID ByRef, [premium params]) As Integer` | Creates a what-if renewal quote for comparison | `spu_SirRen_Copy_Quote_Ins`, `spu_GIS_PolicySchemesSel_add` | `bGISBOM{DMC}.Renewals`, `bGISSchemeBusiness` |
| `LapseExistingPolicy` | `(ediAuditId, dataModelCode, businessTypeCode, folderCnt, partyCnt, renFileCnt) As Integer` | Lapses the existing in-force policy during renewal | — | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `RenCompletion` | `(full renewal completion params) As Integer` | Full renewal completion cycle | `spu_SirRen_Copy_Quote_Ins`, `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `bGISBOM{DMC}.Renewals`, `bSIRIUSLink.Renewals` |
| `ListRenewals` | `(r_vResultArray ByRef, dataModelCode, [filter params]) As Integer` | Returns list of renewals with optional filters | — | `bGISBOM{DMC}.Renewals` or `bGISSchemeBusiness` |
| `UpdateLapseReason` | `(folderCnt, lapseReasonID, lapseComment) As Integer` | Updates the lapse reason for a folder | — | `bGIS.Application` / `bGISSchemeBusiness` |
| `UpdateRenewalControl` | `(folderCnt, [optional renewal control fields]) As Integer` | Updates renewal control fields on a folder | `spu_gis_get_lookup_data` | `bGISSchemeBusiness` |
| `SelectRenewalTaskLog` | `(r_vResultArray ByRef, [start/end date, status, policyNo]) As Integer` | Retrieves renewal task log entries | — | `bGISSchemeBusiness` |
| `RenIsActiveRenewal` | `(insuranceFileCnt, insuranceRef, [out: folderCnt, status, newPolicyNo, date, insurer, scheme]) As Integer` | Checks if a policy is in an active renewal cycle | `spu_SirRen_IsActiveRenewal` | — |
| `CopyRiskData` | `(oldFileCnt, newFileCnt, r_lNewRiskCnt ByRef) As Integer` | Copies risk data from one insurance file to another | `spe_Risk_saa`, `spe_Risk_add`, `spe_insurance_file_risk_li_add` | — |
| `RenTransferPolicyToStandardRenewals` | `(folderCnt, partyCnt, transferReason, transferNotes) As Integer` | Transfers a policy back to standard renewal process | `spu_SIR_renewal_suspension_upd` | — |
| `GetLookupProperties` | `(dataModelCode, r_oLookupProperties ByRef) As Integer` | Returns Hashtable of GIS lookup table properties | `spu_gis_get_lookup_properties` | — |
| `GetPMLookupCodeFromID` | `(lookupTable, lookupId, r_sLookupCode ByRef) As Integer` | Resolves a lookup code from an ID | — | `bGISListManager` |
| `RenMultipleQuotationBrokerLead` | `(dataModelCode, businessTypeCode, selectedArray, r_vFailedArray ByRef, r_vResultArray ByRef) As Integer` | Batch broker-led renewal quotations | `spu_SirRen_Copy_Quote_Ins`, `spu_GIS_PolicySchemesSel_add`, `spu_SIRRen_copy_agents` | `RenQuotationBrokerLead` (loop) |

#### Class: STS

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init | — | `gPMComponentServices.CheckDatabase` |
| `CheckTSAndLock` | `(keyName, keyValue, timestamp, r_sLockedBy ByRef, r_bMatch ByRef) As Integer` | Checks optimistic timestamp and locks record if matching | `spu_GetRiskStatusByRisk` or `spu_GetRiskStatusFlag` | — |
| `DeleteRisk` | `(riskCnt, insuranceFileCnt, folderCnt, transactionType) As Integer` | Deletes a risk record from an insurance file | `spu_delete_insurance_file_risk_link` | — |
| `GetLastUnlockTimestamp` | `(keyName, keyValue, r_vTimestamp ByRef, r_bLocked ByRef, r_iLockedBy ByRef, r_sUser ByRef) As Integer` | Retrieves timestamp of last unlock for a record | `spu_GetRiskStatusByRisk` or `spu_GetRiskStatusFlag` | — |
| `GetList` | `(propertyId, r_vListData ByRef, [searchString]) As Integer` | Returns list items from bGISListManager | — | `bGISListManager.InterfaceNoLogin` |
| `UnlockAndGetTS` | `(keyName, keyValue, r_vTimestamp ByRef) As Integer` | Unlocks a record and returns the new timestamp | `spu_GetRiskStatusByRisk` or `spu_GetRiskStatusFlag` | — |
| `InitialiseListManager` | `(dataModel) As Integer` | Creates and initialises a bGISListManager | — | `bGISListManager.InterfaceNoLogin` |
| `GetDescriptionFromABICode` | `(propertyId, abiCode, r_sDescription ByRef) As Integer` | Looks up description for an ABI code | — | `bGISListManager` |
| `TerminateListManager` | `() As Integer` | Disposes the bGISListManager instance | — | `bGISListManager` |
| `AddMTAQuote` | `(mtaType, msgVersion, dataModelCode, effectiveDate, [out file/folderCnt, premiumParams]) As Integer` | Creates a new MTA quote version on a policy | `spu_gis_policy_link_add`, `spu_gis_policy_link_qte_ref_upd`, `spu_GIS_PolicySchemesSel_add` | — |
| `AddParty` | `(partyTypeCode, branchCode, addresses, partyDetails, contacts) As Integer` | Adds a new party with addresses and contacts | `spu_Party_agent_Branch_add` | `bGIS.Application` |
| `UpdateParty` | `(r_lPartyCnt ByRef, partyTypeCode, partyDetailParams...) As Integer` | Full update of an existing party record | — | `bGIS.Application` |
| `UpdateLastEdiMessageCountReceived` | `(dataModelCode, insuranceFileCnt, lastEdiMsgCount) As Integer` | Updates the last EDI message count on an insurance file | `spu_gis_policy_link_upd_schemeid` | — |
| `GetBackofficeData` | `(brokerAbiId, externalSchemeNo, r_lAgentCnt ByRef, [out: schemeId, insurerId, riskCodeId...]) As Integer` | Retrieves back-office config for an external scheme | `spu_GIS_Scheme_EDI_Link_STS_sel` | — |
| `GetUserDetails` | `(username, r_sUserDataXML ByRef) As Integer` | Retrieves user details as XML string | `spu_SAM_Get_User_Details` | — |
| `GetAgentCntFromBrokerID` | `(brokerAbiId, r_lAgentCnt ByRef) As Integer` | Returns agent party count from a broker ABI ID | `spu_GIS_Scheme_EDI_Link_STS_sel` | — |
| `GetDataFromExternalSchemeNo` | `(externalSchemeNo, [out: schemeId, insurerId, riskCodeId...]) As Integer` | Looks up GIS mapping data from external scheme number | `spu_GIS_Scheme_EDI_Link_STS_sel` | — |
| `RollbackEDI` | `(gisPolicyLinkID, insuranceFileCnt) As Integer` | Clears GIS data for a failed EDI transaction | `spu_EDI_ClearUp` | — |
| `GetPolicyDetailsFromAlternateReference` | `(alternateReference, [out: folderCnt, partyCnt, fileCnt, ediCount, riskFolderCnt]) As Integer` | Returns policy header details from an alternate reference | `spu_GetPolicyVersionForMtaAltReference` | — |
| `GetRenPolicyDetailsFromAltRef` | `(alternateReference, [out: renewal policy details]) As Integer` | Returns renewal policy details from alternate reference | `spu_RenewalPolicyDetailsFromAltReference` | — |
| `FindPolicies` | `(dateOfLoss, policyNumber, clientSurname, postcode, agentName, branchCode, r_vResultArray ByRef) As Integer` | Searches for policies matching claim/loss criteria | `spu_STS_FindPolicies` | — |
| `UpdateCoverDetails` | `(insuranceFileCnt, [coverStartDate, expiryDate, insuranceRef, vehicleModel, partyCnt, riskCode, insurerABICode]) As Integer` | Updates cover dates and insured details on an insurance file | `spu_gis_policy_link_upd_schemeid` | — |
| `GetCurrenciesByBranch` | `(sourceID, r_vCurrencies ByRef) As Integer` | Returns currencies available for a branch/source | — | `gPMComponentServices` |
| `GetSourceListForUser` | `(userID, r_vSources ByRef) As Integer` | Returns list of source IDs accessible to a user | — | `gPMComponentServices` |
| `GetQuoteAndSummariesByKey` | `(dataModelCode, businessTypeCode, insuranceFileCnt, [out: header + r_vResultDataset]) As Integer` | Returns quote header + risk summaries by file count | `spu_STS_RiskSummariesByKey_sel` | — |
| `GetQuoteAndSummariesByRef` | `(dataModelCode, businessTypeCode, insuranceFileRef, [out: same]) As Integer` | Returns quote header + risk summaries by file reference | `spu_STS_RiskSummariesByKey_sel` | — |
| `UpdateQuote` | `(insuranceFileCnt, coverDates, description, insuredParties, [currencyID, analysisCodeId]) As Integer` | Updates quote header data | — | `bGIS.Application` |
| `GetHeaderAndSummariesByKey` | `(insuranceFileCnt, [out: policy header + r_vResultDataset]) As Integer` | Returns policy header + risk summary dataset by file count | `spu_STS_RiskSummariesByKey_sel` | — |
| `GetHeaderAndSummariesByRef` | `(insuranceFileRef, [out: same]) As Integer` | Returns policy header + risk summary dataset by reference | `spu_STS_RiskSummariesByKey_sel` | — |
| `Login` | `(username, password, [out: agentCnt, passwordChange, lastLogin, name, email, sourceList]) As Integer` | Authenticates a STS session user | `spu_sir_passwordhistory_sel`, `Spu_SIR_IsValidPasswordForReUse` | — |
| `Logoff` | `(username) As Integer` | Logs off a STS user session | — | `bGIS.Application` |
| `ChangePassword` | `(username, password, newPassword) As Integer` | Changes a STS user's password | `spu_sir_passwordhistory_sel`, `Spu_SIR_IsValidPasswordForReUse` | — |
| `CreateEvent` | `(partyCnt, description, [filename, folderCnt, fileCnt, claimCnt, eventNotes, subjectId]) As Integer` | Creates a work event/audit trail entry | — | `bGIS.Application` |
| `DeleteRiskLink` | `(insuranceFileCnt, riskID) As Integer` | Removes the risk-to-insurance-file link | `spu_delete_insurance_file_risk_link` | — |
| `ProcessCopyRisk` | `(r_lNewRiskKey ByRef, copyType, copyParams...) As Integer` | Copies a risk record and returns new risk key | `spe_Risk_saa`, `spe_Risk_add`, `spe_insurance_file_risk_li_add` | — |
| `CopyRiskData` | `(insuranceFileCnt, folderCnt, riskCnt, r_lNewRiskCnt ByRef, r_sFailureReason ByRef) As Integer` | Copies GIS and policy risk data to a new risk record | `spe_Risk_saa`, `spe_Risk_add`, `spe_insurance_file_risk_li_add` | — |
| `CopyRiskStandardWordings` | `(oldPolicyBinderId, newPolicyBinderId, dataModelCode) As Integer` | Copies standard wordings from old to new policy binder | `spu_Copy_RISK_Standard_Wording` | — |
| `IsReusedPassword` | `(userID, newPassword, IsValid ByRef) As Integer` | Validates whether a new password has been used before | `spu_sir_passwordhistory_sel`, `Spu_SIR_IsValidPasswordForReUse` | — |
| `GetPMLookupIdFromCode` | `(lookupTable, lookupCode, r_sLookupId ByRef) As Integer` | Resolves PM lookup ID from a code value | — | `bGISListManager` |

#### XML Helper Modules (no DB calls)

| Module | Methods | Purpose |
|--------|---------|---------|
| `GISActionXMLFuncs` | `FormatActionXML` / `UnFormatActionXML` / `FormatActionReturnXML` / `UnFormatActionReturnXML` | Serialise/deserialise generic action request/return XML |
| `GISActionXMLFuncsQuotePolicy` | `FormatActionXMLQuotePolicy` / `UnFormat...` / `FormatActionReturnXMLQuotePolicy` / `UnFormat...` | Serialise/deserialise QuotePolicy action/result XML |
| `GISActionXMLFuncsRenewals` | `FormatActionXMLRenewals` / `UnFormat...` / `FormatActionReturnXMLRenewals` / `UnFormat...` | Serialise/deserialise Renewals action/result XML |
| `GISActionXMLFuncsSecurity` | `FormatActionXMLSecurity` / `UnFormat...` / `FormatActionReturnXMLSecurity` / `UnFormat...` | Serialise/deserialise Security action/result XML |
| `GISActionXMLFuncsFinancial` | `UnFormatActionXMLFinancial` / `FormatActionReturnXMLFinancial` | Serialise/deserialise Financial action/result XML |
| `GISActionXMLFuncsGeneric` | `ExecGenericActionXML` / `FormatGenericActionReturnXML` | Execute any action from XML; serialise generic method return |
| `GISAdditionalXMLFuncs` | `FormatArrayToXML` / `UnformatXMLtoArray` / `FormatAdditionalDataXML` / `UnFormatAdditionalDataXML` | Convert 2D arrays and additional data name/value arrays to/from XML |

#### bGIS — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_gis_model_objects_sel` | `SQL.vb` | Gets GIS model object definitions |
| `spu_gis_model_properties_sel` | `SQL.vb` | Gets GIS model property definitions |
| `spu_gis_policy_link_sel` | `SQL.vb` | Selects a GIS policy link record |
| `spu_gis_policy_link_sel_schid` | `SQL.vb` | Selects policy link by scheme ID |
| `spu_gis_policy_link_add` | `SQL.vb`, `STS.AddMTAQuote` | Inserts a new GIS policy link |
| `spu_gis_policy_link_upd_schid` | `SQL.vb` | Updates scheme ID on policy link |
| `spu_gis_policy_link_upd_schemeid` | `SQL.vb`, `STS.UpdateCoverDetails` | Updates scheme ID via insurance file count |
| `spu_gis_policy_link_qte_ref_upd` | `SQL.vb`, `Renewals.RenSelection`, `Financial.DataCash` | Updates quote reference on policy link |
| `spu_gis_current_scheme_id_sel` | `SQL.vb` | Gets the current scheme ID |
| `spu_SirRen_Copy_Quote_Ins` | `Renewals.*`, `SQL.vb` | Inserts a copied renewal quote |
| `spu_GIS_PolicySchemesSel_add` | `Renewals.*`, `SQL.vb` | Adds selected scheme to policy schemes |
| `spu_SIRRen_copy_agents` | `Renewals.*`, `SQL.vb` | Copies agents to renewal policy |
| `spu_EDI_ClearUp` | `STS.RollbackEDI`, `SQL.vb` | Clears GIS data for failed EDI transaction |
| `spu_STS_risk_gis_screen_id_upd` | `SQL.vb` | Updates risk with GIS screen ID |
| `spu_edi_get_pmuser_details` | `SQL.vb` | Gets PM user details for EDI |
| `spu_GIS_Get_Data_Model_Details_For_Code` | `SQL.vb` | Gets data model details for a code |
| `spu_SAM_CheckNexusPolicy` | `SQL.vb` | Checks if a policy is a Nexus risk |
| `spu_SAM_Delete_Risk_Reinsurance` | `SQL.vb` | Deletes reinsurance for a risk |
| `spu_get_prorata_flag` | `SQL.vb` | Gets pro-rata calculation flag |
| `spe_insurance_file_risk_li_sel` | `SQL.vb` | Selects insurance file risk link |
| `spu_get_gis_data_model_from_risk` | `SQL.vb` | Gets GIS data model from risk |
| `spe_Insurance_File_sel` | `SQL.vb` | Selects insurance file record |
| `spu_update_risk_values` | `SQL.vb` | Updates risk aggregate values from perils |
| `spu_delete_insurance_file_risk_link` | `SQL.vb`, `STS.DeleteRisk`, `STS.DeleteRiskLink` | Deletes insurance file risk link |
| `spu_Party_agent_Branch_add` | `STS.AddParty`, `SQL.vb` | Adds an agent branch record |
| `spe_Insurance_File_Risk_Li_sel` | `SQL.vb` | Selects insurance file risk link details |
| `spu_get_rule_file_name` | `SQL.vb` | Gets rating rule file name for a product |
| `spu_Copy_RISK_Standard_Wording` | `STS.CopyRiskStandardWordings`, `SQL.vb` | Copies standard wordings to new policy |
| `spu_gis_get_lookup_data` | `Renewals.UpdateRenewalControl`, `SQL.vb` | Gets GIS lookup data |
| `spu_Claim_sel` | `SQL.vb` | Gets claim version details |
| `spu_CLM_Get_Claim_Version_Details` | `SQL.vb` | Gets claim version reserve details |
| `spu_SAM_insurance_file_sel` | `SQL.vb` | Gets policy version details |
| `spu_SAM_Get_User_Details` | `STS.GetUserDetails`, `SQL.vb` | Gets contact/user details |
| `spu_get_party_dataset` | `SQL.vb` | Gets party dataset |
| `spu_get_EffectiveDate_PRE` | `SQL.vb` | Gets effective date for PRE |
| `spu_STS_GetEDICommonData` | `Renewals.RenInvitationInsurerLed` | Gets common EDI data |
| `spu_GIS_Insurer_GetQuoteOptions` | `Renewals.RenGetInsurerQuoteOptions` | Gets insurer quote options grid |
| `spu_SirRen_IsActiveRenewal` | `Renewals.RenIsActiveRenewal` | Checks if policy is in active renewal |
| `spe_Risk_saa` | `Renewals.CopyRiskData`, `STS.CopyRiskData` | Selects all risk data |
| `spe_Risk_add` | `Renewals.CopyRiskData`, `STS.CopyRiskData` | Inserts a copied risk record |
| `spe_insurance_file_risk_li_add` | `Renewals.CopyRiskData`, `STS.CopyRiskData` | Inserts insurance file risk link |
| `spu_SIR_renewal_suspension_upd` | `Renewals.RenTransferPolicyToStandardRenewals` | Updates renewal suspension level |
| `spu_gis_get_lookup_properties` | `Renewals.GetLookupProperties` | Gets GIS lookup table properties |
| `spu_GIS_Scheme_EDI_Link_STS_sel` | `STS.GetBackofficeData` | Gets GIS scheme EDI link |
| `spu_sir_passwordhistory_sel` | `STS.Login`, `STS.ChangePassword` | Gets password history for a user |
| `Spu_SIR_IsValidPasswordForReUse` | `STS.Login`, `STS.ChangePassword` | Checks if password can be re-used |
| `spu_GetRiskStatusByRisk` | `STS.CheckTSAndLock` | Gets risk status/timestamp by risk count |
| `spu_GetRiskStatusFlag` | `STS.CheckTSAndLock` | Gets risk status flag |
| `spu_GetPolicyVersionForMtaAltReference` | `STS.GetPolicyDetailsFromAlternateReference` | Gets policy version from alt reference |
| `spu_RenewalPolicyDetailsFromAltReference` | `STS.GetRenPolicyDetailsFromAltRef` | Gets renewal policy details from alt reference |
| `spu_STS_FindPolicies` | `STS.FindPolicies` | Searches for policies for claim processing |
| `spu_STS_RiskSummariesByKey_sel` | `STS.GetQuoteAndSummariesByKey` | Selects risk summaries by insurance file count |

#### bGIS — Component References

| Component | Usage |
|-----------|-------|
| `bGISBOM{DataModelCode}.QuotePolicy` | Quote/policy operations — late-bound via `CreateBOM()` |
| `bGISBOM{DataModelCode}.Security` | Security operations — late-bound |
| `bGISBOM{DataModelCode}.Financial` | Financial operations — late-bound |
| `bGISBOM{DataModelCode}.Renewals` | Renewals operations — late-bound |
| `bGISSchemeBusiness.Business` | GIS scheme lookup/data operations |
| `bSIRIUSLink.Renewals` | Sirius back-office renewal link |
| `bGIS.Application` | GIS Application class (self-reference within same assembly) |
| `bGISListManager.InterfaceNoLogin` | GIS lookup list manager |
| `cGISDataSetControl.Application` | GIS dataset definition controller |

---

### 2. bGISAddOn

**Directory:** `GIS\Components\Core GIS\Server\bGISAddOn`
**VB Source Files:** `Administration.vb`, `CalculateRate.vb`, `MainModule.vb`
**Purpose:** Manages GIS add-on insurance products. `Administration` handles full CRUD for add-on definitions, cover levels, and date-effective rates. `CalculateRate` calculates the add-on premium, IPT, VAT, commission, and fee for a given effective date and base premium.

#### Class: Administration

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `AddAddOn` | `(dataModelCode, r_lAddOnID ByRef, code, captionID, description, isDeleted, effectiveDate, partyCnt) As Integer` | Creates a new add-on record | `spu_gis_add_on_add` | `GISSharedConstants.CheckGISDSN` |
| `AddAddOnCoverLevel` | `(dataModelCode, addOnID, r_lAddOnCoverLevelID ByRef, code, captionID, description) As Integer` | Creates a new cover level for an add-on | `spu_gis_add_on_cover_level_add` | `GISSharedConstants.CheckGISDSN` |
| `AddAddOnRate` | `(dataModelCode, businessTypeCode, r_lAddOnRateID ByRef, addOnID, coverLevelID, fee, rate, IPTRate, effectiveDates, commission, VATRate) As Integer` | Creates a new date-effective rate row for a cover level | `spu_gis_add_on_rate_add` | `GISSharedConstants.CheckGISDSN` |
| `DeleteAddOn` | `(dataModelCode, addOnID) As Integer` | Deletes an add-on record | `spu_gis_add_on_del` | — |
| `DeleteAddOnCoverLevel` | `(dataModelCode, addOnCoverLevelID) As Integer` | Deletes a cover level record | `spu_gis_add_on_cover_level_del` | — |
| `DeleteAddOnRate` | `(dataModelCode, addOnRateID) As Integer` | Deletes a rate record | `spu_gis_add_on_cover_level_del` | — |
| `GetAddOn` | `(dataModelCode, addOnID, r_vAddOnArray ByRef) As Integer` | Retrieves all add-ons or a specific one (admin view) | `spu_gis_add_on_sel_adm` | — |
| `GetAddOnCoverLevel` | `(dataModelCode, addOnID, addOnCoverLevelID, r_vAddOnArray ByRef) As Integer` | Retrieves cover levels for an add-on | `spu_gis_add_on_cover_level_sel` | — |
| `GetAddOnDataBus` | `(dataModelCode, r_vAddOnArray ByRef) As Integer` | Retrieves valid data model / business type combinations | `spu_gis_add_on_databus` | — |
| `GetAddOnRate` | `(dataModelCode, businessType, addOnID, coverLevelID, addOnRateID, r_vAddOnArray ByRef) As Integer` | Retrieves rate records filtered by data model/type/add-on | `spu_gis_add_on_rate_sel` | — |
| `UpdateAddOn` | `(dataModelCode, addOnID, code, captionID, description, isDeleted, effectiveDate, partyCnt) As Integer` | Updates an existing add-on record | `spu_gis_add_on_upd` | — |
| `UpdateAddOnCoverLevel` | `(dataModelCode, addOnID, addOnCoverLevelID, code, captionID, description) As Integer` | Updates an existing cover level record | `spu_gis_add_on_cover_level_upd` | — |
| `UpdateAddOnRate` | `(dataModelCode, businessTypeCode, addOnRateID, addOnID, coverLevelID, fee, rate, IPTRate, dates, commission, VATRate) As Integer` | Updates an existing rate row | `spu_gis_add_on_rate_upd` | — |

#### Class: CalculateRate

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `NBCalculateAddOnPremium` | `(dataModelCode, businessTypeCode, addOnCode, coverLevelCode, effectiveDate, insurancePremium, [out: addOnPremium, IPT, fee, rate, VAT, commission, partyCnt]) As Integer` | Calculates add-on premium, IPT, VAT, fee, commission for given base premium and effective date | `spu_GIS_Add_On_sel` | `GISSharedConstants.CheckGISDSN` |

#### bGISAddOn — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_gis_add_on_add` | `AddAddOn` | Inserts a new GIS add-on record, returns generated ID |
| `spu_gis_add_on_upd` | `UpdateAddOn` | Updates an existing GIS add-on record |
| `spu_gis_add_on_del` | `DeleteAddOn` | Deletes a GIS add-on record |
| `spu_gis_add_on_sel_adm` | `GetAddOn` | Selects add-on records for administration view |
| `spu_gis_add_on_cover_level_add` | `AddAddOnCoverLevel` | Inserts a new add-on cover level, returns ID |
| `spu_gis_add_on_cover_level_upd` | `UpdateAddOnCoverLevel` | Updates an existing add-on cover level |
| `spu_gis_add_on_cover_level_del` | `DeleteAddOnCoverLevel`, `DeleteAddOnRate` | Deletes an add-on cover level record |
| `spu_gis_add_on_cover_level_sel` | `GetAddOnCoverLevel` | Selects add-on cover levels |
| `spu_gis_add_on_rate_add` | `AddAddOnRate` | Inserts a new rates row for a cover level |
| `spu_gis_add_on_rate_upd` | `UpdateAddOnRate` | Updates an existing add-on rate row |
| `spu_gis_add_on_rate_del` | (defined in SQL module) | Deletes an add-on rate row |
| `spu_gis_add_on_rate_sel` | `GetAddOnRate` | Selects rate rows for a data model / business type / add-on |
| `spu_gis_add_on_databus` | `GetAddOnDataBus` | Returns valid data model + business type combinations |
| `spu_GIS_Add_On_sel` | `NBCalculateAddOnPremium` | Returns fee, rate, IPT, VAT, commission, partyCnt for an add-on effective date |

#### bGISAddOn — Component References

| Component | Usage |
|-----------|-------|
| `GISSharedConstants` | Shared GIS utilities — `CheckGISDSN`, logging |

---

### 3. bGISUserDefDetail

**Directory:** `GIS\Components\GIS User Def Detail\Business\bGISUserDefDetail`
**VB Source Files:** `bGISUserDefDetail.vb`, `bGISUserDefDetailBusiness.vb`, `bGISUserDefDetailBusinessSQL.vb`
**Purpose:** Manages individual code/description/date-effective rows within a GIS user-defined lookup header. Supports insert, update (with caption ID creation via architecture DB), retrieval, and data take-on.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init; acquires product DB and architecture DB | — | `gPMComponentServices.CheckDatabase`, `gPMComponentServices.NewDatabase` |
| `Dispose` | `() As void` | Closes both product and architecture DB connections | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets optional process mode properties | — | — |
| `GetDetails` | `(lLookupHeaderId ByRef, vLookupDetails(,) ByRef) As Integer` | Retrieves all lookup detail rows for a given header ID | `spu_GIS_user_def_detail_saa` | — |
| `Update` | `(vLookupDetails(,) ByRef, [v_sUniqueId, v_sScreenHierarchy]) As Integer` | Upserts lookup detail rows — inserts rows with ID = −1, updates all rows; gets caption IDs; wrapped in transaction | `spe_GIS_user_def_detail_add`, `spe_GIS_user_def_detail_upd`, `spu_pm_caption_id_return` | — |
| `GetHeaderInds` | `(lLookupHeaderId ByRef, vHeaderInds(,) ByRef) As Integer` | Retrieves indicator definitions for a lookup header | `spu_GIS_user_def_header_ind_saa` | — |
| `DataTakeOn` | `(vLookupDetails(,) ByRef) As Integer` | Checks whether each detail already exists before calling Update | `spu_check_GIS_user_def_detail`, then delegates to `Update` | — |

#### bGISUserDefDetail — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_user_def_detail_saa` | `GetDetails` | Selects all detail rows for a lookup header |
| `spe_GIS_user_def_detail_add` | `Update` (insert) | Inserts a new lookup detail row, returns generated ID |
| `spe_GIS_user_def_detail_upd` | `Update` (update) | Updates an existing lookup detail row |
| `spu_pm_caption_id_return` | `Update` → private `GetCaptionID` | Gets or creates a caption_id on the architecture DB |
| `spu_GIS_user_def_header_ind_saa` | `GetHeaderInds` | Selects header indicator definitions for a lookup header |
| `spu_check_GIS_user_def_detail` | `DataTakeOn` | Checks whether a detail row exists for header ID + code |

---

### 4. bGISUserDefDetailRates

**Directory:** `GIS\Components\GIS User Def Detail\Business\bGISUserDefDetailRates`
**VB Source Files:** `bGISUserDefDetailRates.vb`, `bGISUserDefDetailRatesBusiness.vb`, `bGISUserDefDetailRatesBusinessSQL.vb`
**Purpose:** Manages rates and indicators attached to individual GIS user-defined lookup detail rows. Controlled by `RatesOrIndicators` property (`"R"` = rates, otherwise indicators). Supports retrieval and full replace (delete-all then re-insert).

**Key Properties:** `LookupDetailId` (Integer, R/W), `RatesOrIndicators` (String, R/W — `"R"` or other)

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init; acquires product DB | — | `gPMComponentServices.CheckDatabase` |
| `Dispose` | `() As void` | Closes product DB connection | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets optional process mode properties | — | — |
| `GetDetails` | `(vLookupRates(,) ByRef) As Integer` | Retrieves rates or indicators for the current `LookupDetailId`. Uses rates SP when `RatesOrIndicators = "R"`, else indicators SP | `spu_GIS_user_def_detail_rat_saa` (rates) or `spu_GIS_user_def_detail_ind_saa` (indicators) | — |
| `Update` | `(vLookupRates ByRef) As Integer` | Full replace in a transaction: deletes all existing records for `LookupDetailId` then re-inserts each row | Rates: `spu_GIS_user_def_detail_rat_del` + `spe_GIS_user_def_detail_ra_add`; Indicators: `spu_GIS_user_def_detail_ind_del` + `spe_GIS_user_def_detail_in_add` | — |

#### bGISUserDefDetailRates — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_user_def_detail_rat_saa` | `GetDetails` (rates) | Selects all rate values for a lookup detail |
| `spu_GIS_user_def_detail_rat_del` | `Update` (rates — delete) | Deletes all existing rate rows for a lookup detail |
| `spe_GIS_user_def_detail_ra_add` | `Update` (rates — insert) | Inserts a new rate value row for a detail |
| `spu_GIS_user_def_detail_ind_saa` | `GetDetails` (indicators) | Selects all indicator values for a lookup detail |
| `spu_GIS_user_def_detail_ind_del` | `Update` (indicators — delete) | Deletes all existing indicator rows for a lookup detail |
| `spe_GIS_user_def_detail_in_add` | `Update` (indicators — insert) | Inserts a new indicator value row for a detail |

---

### 5. bGISUserDefHeader

**Directory:** `GIS\Components\GIS User Def Header\Business\bGISUserDefHeader`
**VB Source Files:** `bGISUserDefHeader.vb`, `bGISUserDefHeaderBusiness.vb`, `bGISUserDefHeaderBusinessSQL.vb`
**Purpose:** Manages named GIS lookup table headers (e.g. OCCUPATION, VEHICLE_TYPE). Supports retrieval, upsert, and duplicate-code checking. Also maintains caption IDs on the architecture DB.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init; acquires product DB and architecture DB | — | `gPMComponentServices.CheckDatabase`, `gPMComponentServices.NewDatabase` |
| `Dispose` | `() As void` | Closes both DB connections | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets optional process mode properties | — | — |
| `GetDetails` | `(vLookupHeaders(,) ByRef) As Integer` | Retrieves all lookup header records from the product database | `spe_GIS_user_def_header_saa` | — |
| `Update` | `(vLookupHeaders(,) ByRef, [v_sUniqueId, v_sScreenHierarchy]) As Integer` | Upserts lookup header rows — inserts rows with ID = −1, updates existing; gets caption IDs; wrapped in transaction | `spe_GIS_user_def_header_add`, `spe_GIS_user_def_header_upd`, `spu_pm_caption_id_return` | — |
| `GetByCode` | `(lLookupHeaderId ByRef, sCode ByRef) As Integer` | Checks whether another header row exists with the given code (duplicate check) | — (inline SELECT on `GIS_user_def_header`) | — |

#### bGISUserDefHeader — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spe_GIS_user_def_header_saa` | `GetDetails` | Selects all user-defined lookup header records |
| `spe_GIS_user_def_header_add` | `Update` (insert) | Inserts a new lookup header record, returns generated ID |
| `spe_GIS_user_def_header_upd` | `Update` (update) | Updates an existing lookup header record |
| `spu_pm_caption_id_return` | `Update` → private `GetCaptionID` | Gets or creates a caption_id on the architecture DB |
| `spu_GIS_user_def_header_rat_saa` | (defined in SQL module) | Selects rate definitions for a lookup header |
| `spu_GIS_user_def_header_ind_saa` | (defined in SQL module) | Selects indicator definitions for a lookup header |

---

### 6. bGISUserDefHeaderRates

**Directory:** `GIS\Components\GIS User Def Header\Business\bGISUserDefHeaderRates`
**VB Source Files:** `bGISUserDefHeaderRates.vb`, `bGISUserDefHeaderRatesBusiness.vb`, `bGISUserDefHeaderRatesBusinessSQL.vb`
**Purpose:** Manages header-level rates and indicators for GIS user-defined lookup tables. Controlled by `RatesOrIndicators` property (`"R"` = rates, otherwise indicators). Supports retrieval and full replace on `GIS_user_def_header_rates` and `GIS_user_def_header_inds` tables.

**Key Properties:** `LookupHeaderId` (Integer, R/W), `RatesOrIndicators` (String, R/W — `"R"` or other)

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init; establishes DB via `gPMComponentServices.CheckDatabase` | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets optional process mode properties | — | — |
| `GetDetails` | `(vLookupRates(,) ByRef) As Integer` | Retrieves all header rates or indicators for the current `LookupHeaderId` | `spu_GIS_user_def_header_rat_saa` (rates) or `spu_GIS_user_def_header_ind_saa` (indicators) | — |
| `Update` | `(vLookupRates ByRef, [v_sUniqueId, v_sScreenHierarchy]) As Integer` | Delete-and-insert update within a transaction: deletes existing then re-inserts each row | Rates: `spu_GIS_user_def_header_rat_del` + `spe_GIS_user_def_header_ra_add`; Indicators: `spu_GIS_user_def_header_ind_del` + `spe_GIS_user_def_header_in_add` | — |
| `Dispose` | `() As void` | Closes DB connection | — | — |

#### bGISUserDefHeaderRates — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_user_def_header_rat_saa` | `GetDetails` (rates) | SELECT all lookup header rates for a `GIS_user_def_header_id` |
| `spu_GIS_user_def_header_ind_saa` | `GetDetails` (indicators) | SELECT all lookup header indicators for a `GIS_user_def_header_id` |
| `spu_GIS_user_def_header_rat_del` | `Update` (rates — delete) | DELETE all rates records for a given header ID |
| `spu_GIS_user_def_header_ind_del` | `Update` (indicators — delete) | DELETE all indicators records for a given header ID |
| `spe_GIS_user_def_header_ra_add` | `Update` (rates — insert) | INSERT a single header rates record |
| `spe_GIS_user_def_header_in_add` | `Update` (indicators — insert) | INSERT a single header indicators record |

---

### 7. bGISSchemeBusiness

**Directory:** `GIS\Components\Insurer Scheme\Scheme Business\bGISSchemeBusiness`
**VB Source Files:** `Business.vb`, `Scheme.vb`, `SchemeGroup.vb`, `SchemeGroupMember.vb`, `SchemeCobolLinkage.vb`, `SchemeProperty.vb`, `SchemePaymentType.vb`, `SchemeData.vb`, `QEMUsage.vb`, `bGISSchemeBusinessConst.vb`
**Purpose:** Manages the full GIS insurer scheme configuration domain — schemes, scheme groups, group members, scheme properties, COBOL linkage, payment types, scheme data (MTA charges), and QEM usage. Also retrieves available quote schemes and performs navigation profile selection management.

#### Class: Business (façade)

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Initialises DB via `bGEMFunc.GetGISDatabase` | — | `bGEMFunc.GetGISDatabase` |
| `GetPropertyCount` | `(propertyCountType, r_iCount ByRef, [businessType, schemeGroupID, schemeID]) As Integer` | Returns count of properties for all schemes of type, a group, or single scheme | `spu_GIS_Prop_Cnt_All_Type_sel`, `spu_GIS_Prop_Cnt_Group_sel`, or `spu_GIS_Prop_Cnt_Scheme_sel` | — |
| `GetNavigationProfile` | `(policyLinkID, bPostQuoteProfile, r_lSelectedSchemes() ByRef, r_vProfileArray(,) ByRef) As Integer` | Gets pre- or post-quote navigation profile for selected schemes; saves/deletes selection in DB | `spu_GIS_PolicySchemesSelAll_del`, `spu_GIS_PolicySchemesSel_add` (batch inline exec) | — |
| `SaveSelectedSchemes` | `(policyLinkID, r_lSelectedSchemes() ByRef) As Integer` | Saves array of selected scheme IDs for a policy link (delete-all then re-insert batch) | `spu_GIS_PolicySchemesSelAll_del`, `spu_GIS_PolicySchemesSel_add` | — |
| `DeleteAllSelectedSchemes` | `(policyLinkID) As Integer` | Deletes all previously selected schemes for a policy link | `spu_GIS_PolicySchemesSelAll_del` | — |
| `SelectSelectedScheme` | `(policyLinkID, r_lSchemeID ByRef) As Integer` | Retrieves the selected scheme for a policy link | `spu_GIS_Pol_Sch_Sel_Select` | — |
| `GetSchemes` | `(businessTypeCode, dataModelCode, r_vSchemesArray(,) ByRef, [policyLinkID, schemeID, effectiveDate, quoteType, calledFromSTS, realTransactionType]) As Integer` | Returns schemes available to quote for a business type/data model combination | `spu_gis_quote_param_sel` | — |
| `GetSchemesByRiskGroup` | `(businessTypeCode, dataModelCode, r_vSchemesArray(,) ByRef, [policyLinkID, schemeID, effectiveDate, quoteType, riskGroupID, calledFromSTS, realTransactionType]) As Integer` | Same as `GetSchemes` with additional `riskGroupID` filter | `spu_gis_quote_param_sel` | — |
| `GetSchemeData` | `(businessTypeCode, dataModelCode, schemeID, r_vSchemeArray ByRef) As Integer` | Gets threshold/charge data for a scheme (MTA charges etc.) | `spu_gis_scheme_data_sel` | — |
| `SQLBeginTrans` / `SQLCommitTrans` / `SQLRollbackTrans` | `() As Integer` | Transaction control | — | — |
| `Dispose` | `() As void` | Closes DB connection | — | — |

#### Class: Scheme

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `Add` | Inserts a new `GIS_Scheme` record (V1) | `spe_GIS_Scheme_add` |
| `Update` | Updates a `GIS_Scheme` record (V1) | `spe_GIS_Scheme_upd` |
| `Add_V2` | Inserts scheme with extended fields (`dict_ver`, `class_of_business`, `country_id`) | `spu_GIS_Scheme_add_V2` |
| `Update_V2` | Updates scheme with extended fields | `spu_GIS_Scheme_upd_V2` |
| `UpdateDetail` | Reads existing scheme (via `GetList`) then merges and updates supplied fields | `spu_GIS_Scheme_sel` (via GetList), `spe_GIS_Scheme_upd` |
| `UpdateDetail_V2` | Same as `UpdateDetail` with V2 proc | `spu_GIS_Scheme_sel`, `spu_GIS_Scheme_upd_V2` |
| `UpdateDetail_V3` | Same + renewal timing fields (`pre_selection_day_num`, `reminder_day_num`) | `spu_GIS_Scheme_sel`, `spu_GIS_Scheme_upd_V3` |
| `Delete` | Deletes a `GIS_Scheme` record | `spe_GIS_Scheme_del` |
| `GetList` | General-purpose list with 14 variants via `GISSB_GET_SCHEME_LISTS` enum | See SP table below |

`Scheme.GetList` enum-to-SP mapping: `GSL_FULL_ACTIVE_OF_TYPE(1)` → `spu_GIS_Scheme_All_Type_sel`; `GSL_SINGLE_SCHEME(2)` → `spu_GIS_Scheme_sel`; `GSL_ID_NAME_OF_TYPE(3)` → `spu_GIS_Scheme_Name_Type_sel`; `GSL_ID_NAME_OF_TYPE_FOR_INSURER(4)` → `spu_GIS_Scheme_Name_TypeIns_sel`; `GSL_AGENCY(5)` → `spu_GIS_Scheme_Agency_sel`; `GSL_ID_NAME_OF_TYPE_FOR_GROUP(6)` → `spu_GIS_Scheme_GrpSchemes_sel`; `GSL_ALL_LEGACY_SCHEMES(7)` → `spu_GIS_Scheme_LegacySchms_sel`; `GSL_ID_NAME_WITH_INSURER(8)` → `spu_GIS_Scheme_AllWithIns_sel`; `GSL_FULL_ACTIVE_OF_TYPE_WITH_INSURER_NAME(9)` → `spu_GIS_Scheme_Type_InsName_sel`; `GSL_FULL_LINK_BRANCH_SCHEME(11)` → `spu_GIS_Full_link_Br_Sch_sel`; `GSL_LEGACY_SCHEMES_BY_CLASS(12)` → `spu_GIS_LegacySchms_by_Class`; `GSL_FULL_LINK_BRANCH_CLASS(13)` → `spu_GIS_Full_Link_Br_Sch_New`; `GSL_FULL_ACTIVE_OF_CLASS_WITH_INSURER_NAME(14)` → `spu_GIS_Scheme_Class_InsName_sel`

#### Class: SchemeGroup

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `Add` | Adds a scheme group (gets caption ID first) | `spu_GIS_Scheme_Group_add` |
| `Delete` | Deletes a scheme group | `spu_GIS_Scheme_Group_del` |
| `Update` | Updates a scheme group (gets caption ID first) | `spu_GIS_Scheme_Group_upd` |
| `GetList` | Returns all groups, by business type, or single by ID | `spu_GIS_Scheme_Groups_All_sel` / `spu_GIS_Scheme_Groups_Bus_sel` / `spu_GIS_Scheme_group_sel` |

#### Class: SchemeGroupMember

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `Add` | Adds a scheme as a member of a scheme group | `spu_GIS_SchemeGrpMember_add` |
| `DeleteAll` | Deletes all members from a scheme group | `spu_GIS_SchemeGrpMember_All_del` |
| `GetList` | Returns members by group ID, all, or by group code | `spu_GIS_SchemeGrpMember_sel` / `spu_GIS_SchemeGrpMember_All_sel` / `spu_Gis_SchemeGrpMemByCode_sel` |

#### Class: SchemeCobolLinkage

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `AddByScheme` | Adds COBOL linkage records for a scheme | `spu_GIS_SCL_Add_By_Scheme` |
| `DeleteByScheme` | Deletes COBOL linkage records for a scheme | `spu_GIS_SCL_Del_By_Scheme` |
| `DeleteAll` | Deletes all COBOL linkage records | `spu_GIS_SCL_Del_All` |

#### Class: SchemeProperty

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `AddByScheme` | Copies scheme properties from master scheme definition; branches on `ClassOfBusiness` | `spu_GIS_SP_Add_By_Scheme` (V1) or `spu_GIS_SP_Add_By_Scheme_V2` (V2 with class_of_business) |
| `DeleteByScheme` | Deletes scheme properties for a scheme | `spu_GIS_SP_Del_By_Scheme` (V1) or `spu_GIS_SP_Del_By_Scheme_V2` (V2) |

#### Class: SchemePaymentType

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `GetListByScheme` | Gets payment types for a scheme; falls back to scheme ID 0 if no records found | `spu_GIS_Select_Sch_Pay` (called twice: for scheme ID then ID 0 as default) |

#### Class: SchemeData

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `Add` | Adds scheme data (MTA charges, min charges, rounding rules) | `spe_GIS_Scheme_Data_add` |
| `Delete` | Deletes scheme data by scheme ID | `spe_GIS_Scheme_Data_del` |

#### Class: QEMUsage

| Method | Purpose | SPs Called |
|--------|---------|------------|
| `Add` | Adds a QEM usage record linking data model, business type, scheme, and QEM ID | `spu_GIS_QEM_Usage_Add` |

#### bGISSchemeBusiness — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_Prop_Cnt_All_Type_sel` | `Business.GetPropertyCount` (type=1) | Count all scheme properties for a business type |
| `spu_GIS_Prop_Cnt_Group_sel` | `Business.GetPropertyCount` (type=2) | Count properties for a scheme group |
| `spu_GIS_Prop_Cnt_Scheme_sel` | `Business.GetPropertyCount` (type=3) | Count properties for a single scheme |
| `spu_GIS_PolicySchemesSelAll_del` | `Business.DeleteAllSelectedSchemes` | Delete all selected schemes for a policy link |
| `spu_GIS_PolicySchemesSel_add` | `Business.SaveSelectedSchemes` (inline batch) | Insert a selected scheme for a policy link |
| `spu_GIS_Pol_Sch_Sel_Select` | `Business.SelectSelectedScheme` | Select current selected scheme for a policy link |
| `spu_gis_quote_param_sel` | `Business.GetSchemes` / `GetSchemesByRiskGroup` | Get available schemes to quote |
| `spu_gis_scheme_data_sel` | `Business.GetSchemeData` | Get scheme threshold/charge data |
| `spe_GIS_Scheme_add` | `Scheme.Add` | Insert GIS_Scheme record (V1) |
| `spe_GIS_Scheme_upd` | `Scheme.Update`, `UpdateDetail` | Update GIS_Scheme record (V1) |
| `spu_GIS_Scheme_add_V2` | `Scheme.Add_V2` | Insert scheme with extended fields |
| `spu_GIS_Scheme_upd_V2` | `Scheme.Update_V2`, `UpdateDetail_V2` | Update scheme with extended fields |
| `spu_GIS_Scheme_upd_V3` | `Scheme.UpdateDetail_V3` | Update scheme with V3 renewal timing fields |
| `spe_GIS_Scheme_del` | `Scheme.Delete` | Delete GIS_Scheme record |
| `spu_GIS_Scheme_All_Type_sel` | `Scheme.GetList(1)` | Select all active schemes for a business type |
| `spu_GIS_Scheme_sel` | `Scheme.GetList(2/10)`, `UpdateDetail*` | Select single scheme |
| `spu_GIS_Scheme_Name_Type_sel` | `Scheme.GetList(3)` | Select scheme id/name for a business type |
| `spu_GIS_Scheme_Name_TypeIns_sel` | `Scheme.GetList(4)` | Select scheme id/name for a business type + insurer |
| `spu_GIS_Scheme_Agency_sel` | `Scheme.GetList(5)` | Select schemes with agency code |
| `spu_GIS_Scheme_GrpSchemes_sel` | `Scheme.GetList(6)` | Select schemes for a scheme group |
| `spu_GIS_Scheme_LegacySchms_sel` | `Scheme.GetList(7)` | Select legacy schemes for a business type |
| `spu_GIS_Scheme_AllWithIns_sel` | `Scheme.GetList(8)` | Select all schemes with insurer details |
| `spu_GIS_Scheme_Type_InsName_sel` | `Scheme.GetList(9)` | Select schemes with insurer name for a type |
| `spu_GIS_Full_link_Br_Sch_sel` | `Scheme.GetList(11)` | Full link branch-scheme select |
| `spu_GIS_LegacySchms_by_Class` | `Scheme.GetList(12)` | Legacy schemes by class of business |
| `spu_GIS_Full_Link_Br_Sch_New` | `Scheme.GetList(13)` | Full link branch-scheme by class |
| `spu_GIS_Scheme_Class_InsName_sel` | `Scheme.GetList(14)` | Schemes with insurer name by class of business |
| `spu_GIS_Scheme_Group_add` | `SchemeGroup.Add` | Insert scheme group |
| `spu_GIS_Scheme_Group_del` | `SchemeGroup.Delete` | Delete scheme group |
| `spu_GIS_Scheme_Group_upd` | `SchemeGroup.Update` | Update scheme group |
| `spu_GIS_Scheme_Groups_All_sel` | `SchemeGroup.GetList(1)` | Select all scheme groups |
| `spu_GIS_Scheme_Groups_Bus_sel` | `SchemeGroup.GetList(2)` | Select scheme groups for a business type |
| `spu_GIS_Scheme_group_sel` | `SchemeGroup.GetList(3)` | Select single scheme group |
| `spu_GIS_SchemeGrpMember_add` | `SchemeGroupMember.Add` | Add scheme to group membership |
| `spu_GIS_SchemeGrpMember_All_del` | `SchemeGroupMember.DeleteAll` | Delete all members from a group |
| `spu_GIS_SchemeGrpMember_sel` | `SchemeGroupMember.GetList(1)` | Select members by group ID |
| `spu_GIS_SchemeGrpMember_All_sel` | `SchemeGroupMember.GetList(2)` | Select all scheme group members |
| `spu_Gis_SchemeGrpMemByCode_sel` | `SchemeGroupMember.GetList(3)` | Select members by group code |
| `spu_GIS_SCL_Add_By_Scheme` | `SchemeCobolLinkage.AddByScheme` | Add COBOL linkage for a scheme |
| `spu_GIS_SCL_Del_By_Scheme` | `SchemeCobolLinkage.DeleteByScheme` | Delete COBOL linkage for a scheme |
| `spu_GIS_SCL_Del_All` | `SchemeCobolLinkage.DeleteAll` | Delete all COBOL linkage records |
| `spu_GIS_SP_Add_By_Scheme` | `SchemeProperty.AddByScheme` (V1) | Copy scheme properties for a scheme |
| `spu_GIS_SP_Del_By_Scheme` | `SchemeProperty.DeleteByScheme` (V1) | Delete scheme properties for a scheme |
| `spu_GIS_SP_Add_By_Scheme_V2` | `SchemeProperty.AddByScheme` (V2) | Copy scheme properties with class_of_business filter |
| `spu_GIS_SP_Del_By_Scheme_V2` | `SchemeProperty.DeleteByScheme` (V2) | Delete scheme properties with class_of_business filter |
| `spu_GIS_Select_Sch_Pay` | `SchemePaymentType.GetListByScheme` | Select payment types for a scheme |
| `spe_GIS_Scheme_Data_add` | `SchemeData.Add` | Insert scheme data record |
| `spe_GIS_Scheme_Data_del` | `SchemeData.Delete` | Delete scheme data by scheme ID |
| `spu_GIS_QEM_Usage_Add` | `QEMUsage.Add` | Add QEM usage mapping |

#### bGISSchemeBusiness — Component References

| Component | Usage |
|-----------|-------|
| `bGEMFunc.GetGISDatabase` | Used in `Business.Initialise` to obtain a GIS-specific database connection |
| `bPMCaption.Business` | Used in `SchemeGroup.Add` and `SchemeGroup.Update` to look up or create caption IDs |

---

### 8. bGISLookupManager

**Directory:** `GIS\Components\LookupManagement\bGISLookupManager`
**VB Source Files:** `Maintain.vb`, `MainModule.vb`
**Purpose:** Builds binary file-based lookup cache files (index, header, and data flat files) from GIS lookup data stored in `gis_lookup_header` and `gis_lookup_data` SQL tables. Called offline/on-demand to refresh lookup cache for a given data model and business type. Uses plain inline SQL — no stored procedures.

#### Class: Maintain

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `initialise` | `(oDatabase As Object ByRef) As Integer` | Sets the database object to use | — | — |
| `BuildLookupCacheFiles` | `(sModelCode ByRef, sOpenFileBusinessType ByRef, lLimitToSpecifiedBusinessType ByRef) As Integer` | Main entry point: queries all distinct lookup names, then reads headers and data rows and writes binary cache files (index/header/data) | Inline SQL on `gis_lookup_header` and `gis_lookup_data` — no SPs | `GISSharedConstants.GetLookupsPath` |
| `OpenTempFiles` | `(sModelCode ByRef, sBusinessType ByRef) As Integer` | Opens three binary cache files for writing: `{model}_LookupIndex.dat`, `{model}_LookupHeader.dat`, `{model}_LookupData.dat` | — | `GISSharedConstants.GetLookupsPath` |
| `CloseTempFiles` | `() As Integer` | Closes the three open binary cache files | — | — |

#### bGISLookupManager — Stored Procedures

None — all queries use inline SQL against `gis_lookup_header` and `gis_lookup_data`.

#### bGISLookupManager — Component References

| Component | Usage |
|-----------|-------|
| `GISSharedConstants.GetLookupsPath` | Retrieves the filesystem path for cache files from registry/configuration |

---

### 9. bGISPromptInterface

**Directory:** `GIS\Components\Prompt Integration\bGISPromptInterface`
**VB Source Files:** `Application.vb`, `MainModule.vb`
**Purpose:** Integration component for communicating with the Prompt premium finance gateway. Constructs XML request messages, submits them via HTTP to the Prompt GNet service, and parses XML responses. Covers bank account validation, premium finance transact (NB), MTA transact, cancellation, and quotation. **Note:** `ProcessRequest` is currently a stub returning `PMError` — HTTP call functionality removed (work item 96304) so all external communication is non-operational.

#### Class: Application

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Initialises user context; no DB connection | — | — |
| `BankAccountValidation` | `(senderID, coverType, gnetClientCode, businessStatus, bankName, bankNo, sortCode, dataModelCode, businessTypeCode, quoteRef, r_sStatusCode ByRef) As Integer` | Builds `BANK_ACCOUNT_VALIDATION_REQUEST` XML and sends to Prompt for bank account/sort code validation | — | External HTTP (disabled) |
| `PremiumFinanceTransact` | `(customer, policy, bank/card, premium, instalment params..., r_sStatusCode ByRef, r_sPremiumFinanceRef ByRef, r_sTransNumber ByRef) As Integer` | Builds `PREMIUM_FINANCE_TRANSACT_REQUEST` XML for new business premium finance transaction | — | External HTTP (disabled) |
| `PremiumFinanceQuote` | `(dataModelCode, businessTypeCode, businessStatus, premiumAmount, premFinRef, effectiveDate, policyNo, [out: statusCode, totalPayable, instalments, interest]) As Integer` | Builds `PREMIUM_FINANCE_QUOTE_REQUEST` XML to query finance instalment details | — | External HTTP (disabled) |
| `PremiumFinanceMTATransact` | `(dataModelCode, businessTypeCode, premiumAmount, premFinRef, effectiveDate, policyNo, [out: statusCode, totalPayable, instalments, newPremFinRef, interest]) As Integer` | Builds `PREMIUM_FINANCE_MTA_TRANSACT_REQUEST` XML for mid-term adjustment | — | External HTTP (disabled) |
| `PremiumFinanceCancellation` | `(dataModelCode, businessTypeCode, premFinRef, cancellationDate, policyNo, [out: statusCode, cancellationAmount]) As Integer` | Builds `PREMIUM_FINANCE_CANCELLATION_REQUEST` XML to cancel a premium finance agreement | — | External HTTP (disabled) |
| `Dispose` | `() As void` | Standard dispose — no DB to close | — | — |

#### bGISPromptInterface — Stored Procedures

None — this component is a pure XML/HTTP integration layer with no database access.

---

### 10. bGISUserDefLookup

**Directory:** `GIS\Components\User Defined Lookups\Business\bGISUserDefLookup`
**VB Source Files:** `bGISUserDefLookup.vb`, `bGISUserDefLookupBusiness.vb`, `bGISUserDefLookupSQL.vb`
**Purpose:** Provides runtime lookup value retrieval from GIS user-defined detail tables with Enterprise Library in-memory caching and file-based cache support. Three public resolution methods cover all lookup scenarios. Cache key format: `KEY_LOOKUP_{languageID}_{tableName}[_{effectiveDate}]`.

**Key Property:** `GISDataModelCode` (String, R/W) — when set, switches to the GIS-specific DSN for that data model.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Initialises DB and Enterprise Library cache manager; reads cache path from registry | — | `gPMComponentServices.CheckDatabase`, `CacheFactory.GetCacheManager` |
| `GetLookupValues` | `(iLookupType ByRef, vTableArray(,) ByRef, iLanguageID ByRef, dtEffectiveDate ByRef, vResultArray(,) ByRef) As Integer` | Retrieves values for one or more GIS lookup tables; checks EL cache first, then inline SQL pipeline; accumulates results for all tables | Inline SQL on `GIS_user_def_detail` + `pmcaption` | `GISSharedConstants.CheckGISDSN`, `CacheFactory` |
| `GetEffectiveIDFromCode` | `(v_sTableName, v_sCode, v_dtEffectiveDate, r_lID ByRef) As Integer` | Looks up the effective detail ID for a given code + table + effective date | `spu_pm_get_eff_id_from_code` | — |
| `GetEffectiveIDFromID` | `(v_sTableName, v_dtEffectiveDate, r_lID ByRef) As Integer` | Resolves an ID to its effective version for a table + effective date (ID is in/out) | `spu_pm_get_eff_id_from_id` | — |
| `GetCodeFromID` | `(v_sTableName, v_lID, r_sCode ByRef) As Integer` | Retrieves the code for a given table + ID | `spu_pm_get_code_from_id` | — |
| `Dispose` | `() As void` | Closes DB connection | — | — |

#### bGISUserDefLookup — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_pm_get_eff_id_from_code` | `GetEffectiveIDFromCode` | Returns effective detail ID for a code + table + effective date |
| `spu_pm_get_eff_id_from_id` | `GetEffectiveIDFromID` | Resolves ID to its effective version |
| `spu_pm_get_code_from_id` | `GetCodeFromID` | Returns code for a given table + ID |

#### bGISUserDefLookup — Component References

| Component | Usage |
|-----------|-------|
| `GISSharedConstants.CheckGISDSN` | Switches DSN when `GISDataModelCode` property is set |
| `CacheFactory` (Enterprise Library) | Cache manager initialised in `Initialise`; used in private `SelectCaptions` |

---

### 11. bGISListGrouping

**Directory:** `Product Builder\3D Rating\ListGrouping\Business\bGISListGrouping`
**VB Source Files:** `Business.vb`, `MainModule.vb`
**Purpose:** Manages GIS list groupings — creates, updates, and auto-assigns items to named groups within list types for a specific scheme. Supports auto-grouping by code value and manual group assignment.

**Key Property:** `GISSchemeID` (Integer, R/W) — scheme context for all queries.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetGroupSummary` | `(r_vResultArray(,) ByRef) As Integer` | Gets list types and group summary for the current `GISSchemeID` | `spu_GIS_List_Group_Summary` | — |
| `GetItemsSummary` | `(listTypeID As Integer, r_vResultArray(,) ByRef) As Integer` | Gets summary count of items per group for a list type | `spu_GIS_List_Group_Items_Summary` | — |
| `GetListItems` | `(listTypeID As Integer, listGroupingID As Integer, r_vResultArray(,) ByRef) As Integer` | Returns all items for a given type and grouping combination | `spu_GIS_Get_List_Group_Items` | — |
| `UpdateGroupItems` | `(code, description, listGroupingID, vDataArray(,), bCheckUsed) As Integer` | Updates code/description of a group and replaces its item assignments | Inline SQL (UPDATE `gis_list_grouping`, DELETE + INSERT `gis_list_grouping_items`) | — |
| `AddItems` | `(code, description, listTypeID, vDataArray()) As Integer` | Creates a new list grouping and inserts associated item rows | `spu_GIS_Gis_List_Grouping_Add` (returns `gis_list_grouping_id`); inline INSERT into `gis_list_grouping_items` | — |
| `AutoGroup` | `(listTypeID As Integer) As Integer` | Auto-groups list items by their code values for a list type | `spu_GIS_Auto_Group_Items` | — |
| `ProcessDeleted` | `(vDataArray(,) ByRef) As Integer` | Permanently deletes groupings flagged with `is_deleted = 1` | Inline DELETE from `gis_list_grouping` | — |

#### bGISListGrouping — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_List_Group_Summary` | `GetGroupSummary` | Returns list types and count of groups per type for a scheme |
| `spu_GIS_List_Group_Items_Summary` | `GetItemsSummary` | Returns summary of item counts within groups for a list type |
| `spu_GIS_Get_List_Group_Items` | `GetListItems` | Returns all items in a specific grouping |
| `spu_GIS_Auto_Group_Items` | `AutoGroup` | Auto-assigns list items to groups based on code |
| `spu_GIS_Gis_List_Grouping_Add` | `AddItems` | Inserts a new group record; returns new `gis_list_grouping_id` |

---

### 12. bGISListMaint

**Directory:** `Product Builder\3D Rating\ListImport\bGISListMaint`
**VB Source Files:** `bGISListMaint.vb`, `bGISListMaintCls.vb`
**Purpose:** Maintains GIS lookup list types and entries — creates list types and PM lookup tables, imports lists from CSV files, adds/updates entries, manages UDL (User Defined List) data, and validates list usage before deletion. Uses two DB connections: one for Broking (`pmePFSiriusSolutions`), one for Architecture (`pmePFSiriusArchitecture`).

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init; opens two DB connections | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetListTypes` | `(vData(,) ByRef) As Integer` | Retrieves all list types from the GIS system | `spu_GIS_List_Type_get` | — |
| `DeleteListType` | `(sListTypeID ByRef) As Integer` | Deletes a list type record by ID | `spu_GIS_List_Type_del` | — |
| `IsUnique` | `(sCode ByRef, sDescription ByRef) As Integer` | Checks whether a list type code/description is unique | `spu_GIS_List_Type_Is_Unique` | — |
| `ListInUse` | `(sListTypeID ByRef, [sMessage ByRef]) As Integer` | Checks if a list type is referenced in other tables | `spu_isinuse` | — |
| `GetListVersions` | `(sListTypeID ByRef, vData(,) ByRef) As Integer` | Returns available versions for a list type | `spu_GIS_List_Type_version` | — |
| `ListExists` | `(sTable ByRef) As Integer` | Checks if a list table exists in the database | `spu_GIS_List_Type_Exists` | — |
| `ReplaceListItem` | `(sListType, PMLookupID, vData(,), lIndex) As Integer` | Replaces a list item — deletes old, updates GIS item, updates PM lookup | `spu_GIS_List_Item_del`, `spu_GIS_List_Item_get`, `spu_GIS_List_Item_upd`, `spu_GIS_Field_Names_Get`, `spu_GIS_upd_pm_lookup` | — |
| `CreatePMLookup` | `(sLName ByRef, vFields() ByRef) As Integer` | Creates a PM lookup table with optional extra varchar fields | `spu_gis_list_pm_caption` | — |
| `AddListEntry` | `(sTable, vData(,), lIndex, dEffDate, lVersion) As Integer` | Adds a new list entry row to the specified UDL table | `spu_GIS_Field_Names_Get`, `spu_gis_listentry_add` | — |
| `AddUsage` | `(sTable, sCode, lVersion, dEffDate) As Integer` | Records that a list code is used by a rating section/version | `spu_gis_list_add_usage` | — |
| `ListItemExists` | `(sTable, sCode, [lVersion]) As Integer` | Checks whether a code already exists in a list table | `spu_GIS_List_Type_Item_Exists` | — |
| `ImportList` | `(sFile ByRef, vData(,) ByRef) As Integer` | Parses a CSV file and loads contents into 2D object array; no DB calls | — | — |
| `SaveNewListType` | `(sCode, sDescription) As Integer` | Inserts a new list type record | `spu_GIS_List_Type_add` | — |
| `GetUserDefinedCodes` | `(sListType, vData(,) ByRef) As PMEReturnCode` | Gets user-defined code items for a given list type | `spu_GIS_Get_UDC_items` | — |
| `GetColumnList` | `(sTable, vFields(,) ByRef) As Integer` | Returns the list of column names for a given list table | `spu_GIS_Field_Names_Get` | — |
| `AddColumn` | `(sLName, sField) As Integer` | Adds a new column to an existing list table | `spu_gis_list_pm_caption_update` | — |
| `UpdateListEntry` | `(sTable, vData(,), lIndex, dEffDate) As Integer` | Updates an existing list entry row in the UDL table | `spu_GIS_Field_Names_Get`, `spu_gis_listentry_update` | — |
| `GetUDLData` | `(v_sTableName, v_sCode, r_vUDLData(,) ByRef) As Integer` | Gets UDL data for a specific code from a table | `spu_UDL_Data_sel` | — |
| `UpdateUDLData` | `(v_sTableName, v_sCode, v_lCaption_id, v_sDescription, v_lVersion) As Integer` | Updates UDL data record | `spu_UDL_Data_upd` | — |
| `UpdateUDLVersion` | `(v_sTableName, ...) As Integer` | Updates UDL version number | `spu_UDL_Version_upd` | — |
| `GetMaxUDLVersion` | `(v_sTableName, ...) As Integer` | Gets maximum UDL version for a table | `spu_Get_UDL_Max_Version` | — |
| `GetGISUDLDetail` | `(v_sTableName, ...) As Integer` | Gets GIS/UDL link detail from the GIS list type table | `spu_GIS_List_TypeUDL_Exists` | — |
| `GetInsuranceFileDetails` | `(v_lInsuranceFileCnt As Long, r_vResults(,) ByRef) As Long` | Returns insurance file details for a given file count | `spu_SIR_Get_Insurance_File_Details` | — |

#### bGISListMaint — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_isinuse` | `ListInUse` | Generic check whether a field value is referenced in child tables |
| `spu_GIS_List_Type_Is_Unique` | `IsUnique` | Returns rows if code/description already exists |
| `spu_gis_list_add_usage` | `AddUsage` | Records list item code usage in rating sections |
| `spu_GIS_List_Type_add` | `SaveNewListType` | Inserts a new GIS list type record |
| `spu_GIS_List_Type_del` | `DeleteListType` | Deletes a list type by ID |
| `spu_GIS_List_Type_get` | `GetListTypes` | Selects all list types |
| `spu_GIS_Get_UDC_items` | `GetUserDefinedCodes` | Returns codes for a user-defined list type |
| `spu_GIS_List_Type_version` | `GetListVersions` | Returns version records for a list type |
| `spu_GIS_List_Type_Item_Exists` | `ListItemExists` | Checks whether a code exists in a list table |
| `spu_GIS_List_Type_Exists` | `ListExists` | Checks whether a list type table exists |
| `spu_GIS_Field_Names_Get` | `ReplaceListItem`, `AddListEntry`, `GetColumnList`, `UpdateListEntry` | Returns column names for a UDL table |
| `spu_gis_list_pm_caption` | `CreatePMLookup` | Creates PM lookup table with extra field columns |
| `spu_gis_list_pm_caption_update` | `AddColumn` | Adds a column to an existing PM lookup table |
| `spu_gis_listentry_add` | `AddListEntry` | Inserts a new row into a UDL list table |
| `spu_gis_listentry_update` | `UpdateListEntry` | Updates a row in a UDL list table |
| `spu_GIS_List_Item_del` | `ReplaceListItem` | Deletes a GIS list item by list type + new code |
| `spu_GIS_List_Item_get` | `ReplaceListItem` | Gets current code for a PM lookup ID |
| `spu_GIS_List_Item_upd` | `ReplaceListItem` | Updates the GIS list item with a new code |
| `spu_GIS_upd_pm_lookup` | `ReplaceListItem` | Updates PM lookup with new code, description, and extra fields |
| `spu_UDL_Data_sel` | `GetUDLData` | Selects UDL data for a code |
| `spu_UDL_Data_upd` | `UpdateUDLData` | Updates UDL data record |
| `spu_UDL_Version_upd` | `UpdateUDLVersion` | Updates UDL version |
| `spu_Get_UDL_Max_Version` | `GetMaxUDLVersion` | Returns maximum UDL version |
| `spu_GIS_List_TypeUDL_Exists` | `GetGISUDLDetail` | Returns GIS/UDL link detail |
| `spu_SIR_Get_Insurance_File_Details` | `GetInsuranceFileDetails` | Returns insurance file details by file count |

---

### 13. bGISRating

**Directory:** `Product Builder\3D Rating\Rating\Business\bGISRating`
**VB Source Files:** `Business.vb`, `MainModule.vb`
**Purpose:** Manages GIS 3D rating tables — rate types (up to 3 list-type axes), matrix values, and rate lookup. Supports viewing, saving, and filling the rating matrix for a given scheme.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Long` | Standard init | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetRateTypeTable` | `(r_vResultArray(,) ByRef) As Integer` | Returns all rate types defined for the current scheme | `spu_GIS_Get_Rate_Type` | — |
| `GetListTypes` | `(vData(,) ByRef) As Integer` | Returns list types available for use as rating axes | `spu_GIS_Get_Rate_Type_List` | — |
| `SaveRateType` | `(lSchemeID ByRef, sDescription ByRef, lListType1 ByRef, lListType2 ByRef, lListType3 ByRef, lRateTypeID ByRef) As Object` | Saves or updates a rate type with up to 3 list-type axes (nulls for unused) | `spu_GIS_Add_Rate_type` | — |
| `GetAxes` | `(sRateType ByRef, lSchemeID ByRef, vData(,) ByRef) As Object` | Returns the axis list (lookup codes) for a named rate type | `spu_GIS_Get_Axis_List` | — |
| `GetAxis` | `(lSchemeID ByRef, lListTypeID ByRef, vData(,) ByRef) As Object` | Returns a single axis (all items in a list type) | `spu_GIS_Get_Axis` | — |
| `GetMatrix` | `(lSchemeID ByRef, sRateType ByRef, vData(,) ByRef, [sGroupZ ByRef]) As Object` | Returns the full rating matrix, filtered by optional Z-group | `spu_GIS_Get_Matrix` | — |
| `FillMatrix` | `(lSchemeID As Integer, sRateType As String) As Object` | Populates matrix rows for a rate type (auto-creates cells) | `spu_GIS_Fill_Matrix` | — |
| `SaveRate` | `(lSchemeID ByRef, sRateType ByRef, LU1 ByRef, LU2 ByRef, LU3 ByRef, dRate ByRef) As Object` | Saves a single rate value at a specific matrix cell | `spu_GIS_Save_rate` | — |
| `RateInUse` | `(sDescription As String, v_lGISSchemeID As Integer) As Boolean` | Returns True if a rate type description is already in use for the scheme | `spu_GIS_Get_Rate_Type_Count` | — |

#### bGISRating — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_Get_Rate_Type` | `GetRateTypeTable` | Returns all rate type records for a scheme |
| `spu_GIS_Get_Rate_Type_List` | `GetListTypes` | Returns list types suitable for use as rating axes |
| `spu_GIS_Add_Rate_type` | `SaveRateType` | Inserts or updates a rate type record |
| `spu_GIS_Get_Axis_List` | `GetAxes` | Returns axis values for a named rate type |
| `spu_GIS_Get_Axis` | `GetAxis` | Returns all items in a list type (single axis) |
| `spu_GIS_Get_Matrix` | `GetMatrix` | Returns matrix rows for a rate type (optionally filtered by Z group) |
| `spu_GIS_Fill_Matrix` | `FillMatrix` | Auto-fills matrix rows for a rate type |
| `spu_GIS_Save_rate` | `SaveRate` | Saves a rate value at a specific matrix cell |
| `spu_GIS_Get_Rate_Type_Count` | `RateInUse` | Returns count of rate types matching a description in a scheme |

---

### 14. bGISMaintainDataDictionary

**Directory:** `Product Builder\data model editor\Business\bGISMaintainDataDictionary`
**VB Source Files:** `bGISMaintainDataDictionaryBusiness.vb` (~3,900 lines, partial class), `bGISMaintainDataDictionaryBusinessSQL.vb`, `bGISMaintainDataDictionaryGISObject.vb`, `bGISMaintainDataDictionaryGISProperty.vb`, `StoredProcSQL.vb`
**Purpose:** Maintains the GIS data model dictionary — creates, reads, and updates GIS data model definitions (objects, properties, screen layouts, QEM usage, WP fields). Also generates and drops stored procedures for data model tables, manages search field configuration, and supports Marketplace data model flags.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Long` | Standard init; reads SQL Server version; detects underwriting/agency setting | `sp_MSgetversion` | — |
| `Dispose` | `() As void` | Disposes DB and Swift integration object | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetDataModelDetails` | `() As Integer` | Reads `gis_data_model` for current model code; creates it if not found | `spu_pm_caption_id_return` | — |
| `GetDataModelBOMRequired` | `(v_sDataModelCode, r_sBOMRequired ByRef) As Integer` | Reads BOM required registry setting for the data model | — | — |
| `SetDataModelBOMRequired` | `(sDataModelCode, sBOMRequired) As Integer` | Writes BOM required setting to Windows registry | — | — |
| `GetObjectAndPropertyDetails` | `(r_vGISObject(,) ByRef, r_vGISProperty ByRef) As Integer` | Loads all GIS objects and properties for current data model ID | `spu_GIS_object_saa`, `spu_GIS_property_saa` | — |
| `GetOtherDetails` | `(r_vPartyType ByRef, r_vSumInsuredType ByRef, r_vGISUserDefHeader ByRef, r_vProduct ByRef, r_vIndexLinking ByRef, [r_vDocumentFilter, r_vPMLookupList]) As Integer` | Loads supporting lookup arrays (party types, sum insured types, products, document filters, PM lookup list) | `spu_get_document_filter`, `spu_pm_lookup_table_sel`, inline SELECT SQL | — |
| `Update` | `(r_vGISObject(,) ByRef, r_vGISProperty() ByRef, v_lSingleObjectId, [bFromPIE, v_sUniqueId, v_sScreenHierarchy]) As Integer` | Large transactional save — persists all changed objects and properties; inserts/updates/deletes; generates/drops SPs; updates WP fields and QEM usage | `spu_GIS_object_saa`, `spu_GIS_object_add`, `spe_GIS_object_upd`, `spu_GIS_property_saa`, `spu_GIS_property_add`, `spe_GIS_property_upd`, `spu_GIS_property_del`, `spu_pm_caption_id_return`, `spu_GIS_QEM_usage_sel`, `spu_GIS_QEM_usage_add`, `spe_wp_fields_add`, `spe_wp_fields_sel`, `DDLDropProcedure`, `DDLDropForeignKey`, `sp_mshelpColumns`, `DDLAddColumn` | — |
| `BeginTrans` / `CommitTrans` / `RollbackTrans` | `() As Integer` | Transaction control | — | — |
| `GetSwiftSpecialListTypes` | `(r_vListTypesArray(,) ByRef) As Integer` | Delegates to internal Swift integration object to get special list types | — | `m_oiSWSirius` (Swift integration) |
| `RecreateDatasets` | `(v_sGisDataModelCode As String) As Integer` | Creates `bGIS.Application` and calls `RecreateDatasets` to rebuild DSD/DS | — | `bGIS.Application` |
| `GetDataModelSearchFields` | `(v_lGisDataModelID, r_vResultArray(,) ByRef) As Integer` | Returns search field list for a data model ID | `spu_GIS_GetDataModelSearchFields` | — |
| `GetSearchFieldsSQL` | `(cSearchFields As ArrayList, r_sSQLJoins ByRef, r_sSQLWhere ByRef) As Integer` | Builds dynamic SQL JOIN/WHERE clauses from search field definitions | — | — |
| `RebuildDefaultObjects` | `(v_sPolicyBinderTable As String) As Integer` | Rebuilds default GIS object tables for a policy binder | `spu_GIS_object_add` (via GISObject helper) | — |
| `UpdateMPDataModel` | `(sDataModelCode As String, bIsMPDataModel As Boolean) As Integer` | Updates the marketplace data model flag | `spu_GIS_UpdateMPDataModel` | — |

#### bGISMaintainDataDictionary — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `sp_MSgetversion` | `Initialise` | Returns SQL Server version string |
| `spu_pm_caption_id_return` | `GetDataModelDetails`, `Update` | Returns or creates a caption ID |
| `spu_GIS_object_saa` | `GetObjectAndPropertyDetails`, `Update` | Selects all GIS objects for a data model |
| `spu_GIS_object_add` | `Update`, `RebuildDefaultObjects` | Inserts a new GIS object record |
| `spe_GIS_object_upd` | `Update` | Updates an existing GIS object record |
| `spu_GIS_property_saa` | `GetObjectAndPropertyDetails`, `Update` | Selects all GIS properties for a data model |
| `spu_GIS_property_add` | `Update` | Inserts a new GIS property record |
| `spe_GIS_property_upd` | `Update` | Updates an existing GIS property record |
| `spu_GIS_property_del` | `Update` | Deletes a GIS property record |
| `spu_GIS_QEM_usage_sel` | `Update` | Checks existing QEM usage for a property |
| `spu_GIS_QEM_usage_add` | `Update` | Adds a QEM usage entry for a property |
| `spe_wp_fields_add` | `Update` | Inserts Work Producer field definition |
| `spe_wp_fields_sel` | `Update` | Selects Work Producer fields |
| `DDLDropProcedure` | `Update` | DDL: drops a stored procedure by name |
| `DDLDropForeignKey` | `Update` | DDL: drops a foreign key constraint |
| `DDLAddColumn` | `Update` (inline EXEC) | DDL: adds a column to a table |
| `sp_mshelpColumns` | `Update` | System SP returning column list for a table |
| `spu_get_document_filter` | `GetOtherDetails` | Returns document filter configuration |
| `spu_pm_lookup_table_sel` | `GetOtherDetails` | Returns available PM lookup list tables |
| `spu_GIS_GetDataModelSearchFields` | `GetDataModelSearchFields` | Returns search field configuration for a data model |
| `spu_GIS_UpdateMPDataModel` | `UpdateMPDataModel` | Updates marketplace data model flag |
| `spu_Gis_GetObjectIdByName` | Private `GetObjectId` | Returns `gis_object_id` by object name |

#### bGISMaintainDataDictionary — Component References

| Component | Usage |
|-----------|-------|
| `bGIS.Application` | Called from `RecreateDatasets` to rebuild data model datasets |
| `m_oiSWSirius` (Swift integration) | Used by `GetSwiftSpecialListTypes` |

---

### 15. bGISPMUExtras

**Directory:** `Product Builder\gis pmu extras\Business\bGISPMUExtras`
**VB Source Files:** `bGISPMUExtrasBusiness.vb` (partial, ~7,500+ lines), `bGISPMUExtrasBusinessExt.vb`, `bGISPMUExtrasBusinessForATS.vb`, `bGISPMUExtrasBusinessSQL.vb`, `PBRiskPolicyCurrency.vb`
**Purpose:** Core GIS runtime helper component available to all GIS product scripts. Provides on-demand access to party, policy, claim, and scheme data; work manager task creation; claim reserve/payment updates; currency conversion; ATS (Advanced Tax System) tax calculation; and miscellaneous utility routines for product scripting.

#### Class: Business (public methods only)

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials..., [bStandAlone, vDatabase]) As Long` | Standard init; connects to DB; sets default task variables | — | — |
| `Dispose` | `() As void` | Disposes DB, PBRiskPolicyCurrency, RiskDataSet, PartyDataSet | — | `bGISPMUExtras.PBRiskPolicyCurrency` |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetClaimCnt` | `() As Integer` | Returns claim ID from `gis_policy_link_id`; populates `m_lClaimCnt` | `Spu_GIS_GetClaimCnt` | — |
| `GetSumInsured` | `(sSumInsuredType ByRef, cSumInsured ByRef, cMaxSumInsured ByRef, dRate ByRef, cPremium ByRef) As Integer` | Returns sum insured type details from the insurance file | `spu_GetInsuranceFileDetails_Extras` | — |
| `GetCodeAndIndicator` | `(sLookupType ByRef, lLookupValue ByRef, sCode ByRef, sIndicator ByRef) As Integer` | Gets code and indicator flag for a lookup value | `spu_GIS_Get_CodeAndIndicator` | — |
| `GetIdFromCode` | `(sLookup ByRef, sCode ByRef, lId ByRef) As Integer` | Gets the database ID for a lookup code | `spu_getUserDefDetailforID`, `spu_getUserDefDetail` | — |
| `GetAddress` | `(lAddressCnt ByRef, sAddressLine1 ByRef, ...) As Integer` | Returns address fields for an address count | `spe_Address_sel` | — |
| `IsSameScheme` | `(v_lOldSchemeId, v_lNewSchemeId, r_bIsSameScheme ByRef) As Object` | Checks whether two scheme IDs map to the same scheme | `spu_PMB_Is_Same_Scheme` | — |
| `MatchPMToGIS` | `(sPMLookupType ByRef, lPMLookupValue ByRef, sGISLookupType ByRef, lGISLookupValue ByRef) As Integer` | Maps a PM lookup entry to its equivalent GIS list entry | `spu_getUserDefDetail` | — |
| `GetPMLookupCode` | `(v_sTableName, v_lID, r_vCode ByRef) As Integer` | Returns the code string for a PM lookup ID | `spu_pm_get_code_from_id` | — |
| `GetPMLookupRate` | `(v_vSchemeId, v_sLookupName, v_vLookup1, v_vLookup2, v_vLookup3, r_iReturnValue ByRef, r_vRate ByRef, [v_dExecutionDate]) As Integer` | Looks up a 3D rate value from the PM lookup matrix | `spu_pmlookup_rate` | — |
| `GetPreviousRate` | `(v_vGisPolicyLinkID, v_vRatingSectionCode, r_vRate ByRef, [r_vRateType ByRef]) As Integer` | Returns the rate from a previous policy risk version | `spu_Get_Prev_Rate` | — |
| `GetBODetails` | `(r_vData ByRef) As Boolean` | Gets back-office details for the current `PolicyLinkId` | `spu_getBODetails` | — |
| `GetField` | `(sTable, sFieldName, sCode, ...) As Integer` | Returns a specific field value from an arbitrary lookup table | `spu_getUserDefDetail` or inline SQL | — |
| `GetPartyCntFromShortName` | `(v_sShortName ByRef, v_lPartyCnt ByRef) As Integer` | Returns party key for a party short name | `Spu_GIS_GetPartyCnt` | — |
| `GetClaimInformationDetail` | `(r_oData ByRef, [r_oClaimsPerils ByRef]) As Integer` | Returns full claim, peril, and reserve arrays | `spu_get_claim_info_detail` | — |
| `GetClaimPaymentAgainstPeril` | `(r_vData ByRef, [r_vClaimsPerils ByRef]) As Integer` | Returns claim payments aggregated at peril/reserve level | `spu_CLM_Get_Claim_Payment_AgainstReserveLevel` | — |
| `CheckInRenewal` | `(v_lInsuranceFileCnt, r_lRenewalStatus ByRef) As Integer` | Returns renewal processing status for an insurance file | `spu_check_in_renewal` | — |
| `CallNamedStoredProcedure` | `(v_lIccsCode, v_sSpDescription, r_vResults ByRef, [v_vExtraParameters]) As Integer` | Dynamically builds and calls `spu_ICCS_[code]_[description]` | `spu_ICCS_[n]_[desc]` (dynamic name) | — |
| `AddEvent` | `() As Integer` | Logs an event using `EventTypeCode` and `EventDescription` | `spe_event_log_add`, `spu_GetEventTypeGroupCode` | — |
| `NoofClaimsLodgedAfterCancelDate` | `() As Integer` | Returns number of claims lodged after a policy's cancellation date | `spu_check_claims_lodged` | — |
| `TaskAddKeys` / `TaskClearKeys` | `(sKeyName, vKeyValue) / ()` | Appends/clears name-value key pairs for work manager task | — | — |
| `CurrentCurrencyISOCode` / `Name` / `Symbol` | `() As String` | Returns ISO code/name/symbol of current policy currency | — | `bGISPMUExtras.PBRiskPolicyCurrency` |
| `PreviousCurrencyISOCode` / `Name` / `Symbol` | `() As String` | Returns ISO code/name/symbol of previous policy currency | — | `bGISPMUExtras.PBRiskPolicyCurrency` |
| `ConvertToCurrentPolicyCurrency` | `(v_vOldAmount, r_vNewAmount ByRef) As void` | Converts from previous to current policy currency | — | `bGISPMUExtras.PBRiskPolicyCurrency` |
| `ConvertBetweenCurrencies` | `(v_vOldCurrencyISOCode, v_vOldAmount, v_vNewCurrencyISOCode, r_vNewAmount ByRef) As void` | Converts between two named currencies | — | `bGISPMUExtras.PBRiskPolicyCurrency` |
| `LinkPerilClaim` | `(r_lPerilPartyId ByRef, v_lClaimId, v_lPartyCnt, v_lClaimPerilId) As Integer` | Links a claim peril to a peril party record | `spu_CLM_Claim_Peril_Link` | — |
| `GetClaimPeril` | `(v_lClaimId, v_vClaimPerilArray(,) ByRef) As Integer` | Returns all peril records for a claim | `spu_CLM_Get_Claim_Peril` | — |
| `GetABICodeFromDescription` | `(v_sListId, v_sDescription, r_vABICode ByRef) As Integer` | Returns ABI code matching a description in a list | — | — |
| `GetABIListIdOfProperty` | `(r_oDataset ByRef, v_sObjectname, v_sPropertyName, r_vABIListId ByRef) As Integer` | Returns the ABI list ID assigned to a GIS property | — | — |
| `GetFullAddress` | `(lAddressCnt ByRef) As Object` | Returns all address fields for an address count | `spe_Address_sel` | — |
| `AddDocumentTemplateReference` | `(v_sObjectname, v_sPropertyName) As Integer` | Inserts a standard wording row for the current policy binder | Inline INSERT on `[datamodel]_standard_wording` | — |
| `GetPolicyReinsurers` | `(r_vReinsurers(,) ByRef) As Integer` | Returns reinsurer details for the current policy | `spu_GIS_Scheme_sel` | — |
| `GetPolicySections` | `(r_vSectionArray(,) ByRef) As Integer` | Returns policy output section data for the risk | `spu_txn_risk_code_section_sel` | — |
| `GetInsurerTaxRate` | `(r_lPartyCnt ByRef, r_vTaxGroupId ByRef, r_vTaxGroupCode ByRef, r_vTaxRate ByRef) As Integer` | Returns tax rate details for an insurer party | `spu_txn_section_tax_rate_sel` | — |
| `CreatePolicyOutputSections` | `(r_oDataset ByRef) As Integer` | Creates policy output section records | `spu_txn_risk_code_section_sel` | — |
| `GetResolvedNameFromPartyCnt` | `(v_lPartyCnt ByRef, v_sResolvedName ByRef) As Integer` | Returns resolved party name from party key | `Spu_get_party_code` | — |
| `UpdateClaimReserves` | `(v_ClaimDetailsArray, [bPostReserves]) As Integer` | Updates reserve records for all perils; optionally posts to Orion | `spu_update_reserve_details` | `bControlTransClaims.Automated` (when `bPostReserves = True`) |
| `GetUserCanChangeReserves` | `(v_lUserID, r_bCanChangeReserves ByRef) As Integer` | Checks if a user has authority to change claim reserves | `spu_SIR_Get_UserCanChange_Reserves` | — |
| `GetAllowNegativeReserves` | `(r_bAllowNegativeReserves ByRef) As Integer` | Returns whether negative reserve values are allowed | — | — |
| `GetCaseIncurredTotals` | `(v_vCaseID, r_vTotalIndemnity ByRef, r_vTotalExpense ByRef, r_vTotalExcess ByRef) As Integer` | Returns total indemnity/expense/excess incurred for a case | `spu_Get_Case_Claim_Link` | — |
| `GetSchemeMaximumTempDrivers` / `GetSchemeTempVehicleLimitedMaxGroup` / `GetSchemeTadTavCombination` | `() As Integer` | Returns specific scheme configuration values | `spu_GIS_Scheme_sel` | — |
| `SetRenewalProductCode` / `GetRenewalProductCode` | `(sProductCode) / (vProductCode ByRef) As Integer` | Sets/gets renewal product code on the insurance file | `Spu_Sir_insurance_file_UpdateRenewalProduct` / `Spu_Sir_insurance_file_SelectRenewalProduct` | — |
| `UpdateClaimRecoveries` | `(oClaimsPerils As Object) As Integer` | Updates recovery records for claim perils | `spu_CLM_save_recovery` | — |
| `GetNewUID` | `() As String` | Returns a new GUID string | — | — |
| `GetNewClaimPaymentItem` | `(nClaimPerilID, nReserveTypeID, o_oClaimPaymentItem ByRef) As Integer` | Returns populated new claim payment item array | `spu_Get_New_Claim_Payment_item` | — |
| `GetPreviousRiskData` | `(o_oData ByRef, sObjectName, [sColumns, sUIDValue]) As Integer` | Returns risk data from previous version of a risk/claim | `spu_Get_Previous_RiskData` | — |
| `GetTableData` | `(o_oData ByRef, sTableName, sColumns, sSQLCondition) As Integer` | Safely queries any table with column + where condition (injection-protected) | `spu_Get_Table_Data` | — |
| `GetPreviousReserve` | `(nReserveID, r_crPreviousReserve ByRef) As Integer` | Returns the previous reserve amount for a reserve ID | (via `kGetPreviousReserveSQL`) | — |
| `UpdateClaimPaymentDetails` | `(oClaimDetailsArray, [oAdvancedTaxArray, aoUpdatedTaxArray, dScriptCalculatedTax, bIsSpecifiedScriptTax, bPostPayment]) As Integer` | Full claim payment save — iterates perils/reserves, applies ATS or system tax, calls payment SP; optionally posts to Orion | `spu_update_ClaimPayment_details` | `bCLMPeril.Business` (when `bPostPayment = True`) |
| `GetPaymentTaxFromATS` | `(claimPerilID, bIsExcess, partyName, paymentPartyType, paymentAmount, currencyCode, taxGroupCode, advancedTaxArray, o_crScriptedTaxAmount ByRef) As Integer` | Calculates claim payment tax using ATS rules engine script | Internal ATS calculation | — |
| `GetReceiptTaxFromATS` | `(claimPerilID, partyName, receiptAmount, currencyCode, taxGroupCode, advancedTaxArray, o_crScriptedTaxAmount ByRef) As Integer` | Calculates claim receipt tax using ATS rules engine | Internal ATS calculation | — |
| `GetSystemCalculatedTax` | `(claimPerilID, paymentAmount, taxGroupCode, [out: scriptedTaxAmount, updatedTaxArray, currencyCode, bForReceipt]) As Integer` | Calculates tax based on system tax group (without ATS scripting) | Internal `CalculateTaxAmounts` | — |

#### bGISPMUExtras — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `Spu_GIS_GetClaimCnt` | `GetClaimCnt` | Returns current claim ID from `gis_policy_link` |
| `spe_Address_sel` | `GetAddress`, `GetFullAddress` | Returns full address record by address count |
| `spu_get_claim_info` | `GetClaimInformation` (property) | Returns aggregated claim count and totals |
| `spu_get_claim_info_detail` | `GetClaimInformationDetail` | Returns full claim + peril + reserve detail arrays |
| `spu_accumulation_used_elsewhere` | `AccumulationUsedElsewhere` (property) | Returns accumulation usage count |
| `spu_Get_Prev_Rate` | `GetPreviousRate` | Returns previous risk rating section rate |
| `spu_pm_get_code_from_id` | `GetPMLookupCode` | Returns PM lookup code string from numeric ID |
| `spu_PMB_Is_Same_Scheme` | `IsSameScheme` | Checks if two scheme IDs resolve to the same scheme |
| `spu_check_in_renewal` | `CheckInRenewal` | Returns renewal processing status for an insurance file |
| `spu_check_claims_lodged` | `NoofClaimsLodgedAfterCancelDate` | Returns claim count lodged after cancellation date |
| `spu_CLM_Claim_Peril_Get_SumInsured_And_RIBand` | Internal claim peril creation | Returns sum insured and RI band for a new peril |
| `spu_CLM_Claim_Peril_Add` | Internal claim peril creation | Inserts a new claim peril record |
| `spu_CLM_Claim_Peril_Upd` | Internal claim peril update | Updates a claim peril record |
| `spu_CLM_Claim_Peril_Del` | Internal claim peril delete | Deletes a claim peril record |
| `spu_CLM_Get_Claim_Peril` | `GetClaimPeril` | Returns all peril records for a claim |
| `spu_CLM_Claim_Peril_Link` | `LinkPerilClaim` | Links a claim peril to a peril party |
| `spu_GIS_Scheme_sel` | `GetPolicyReinsurers`, `GetScheme*` methods | Returns scheme configuration details |
| `spu_SIR_Get_UserCanChange_Reserves` | `GetUserCanChangeReserves` | Returns flag whether user can modify reserves |
| `spu_update_reserve_details` | `UpdateClaimReserves` | Updates reserve record details for a claim |
| `spu_Get_Case_Claim_Link` | `GetCaseIncurredTotals` | Returns case's incurred totals |
| `spe_event_log_add` | `AddEvent` | Inserts an event log record |
| `spu_GetEventTypeGroupCode` | `AddEvent` | Returns event type group code |
| `Spu_get_party_code` | `GetResolvedNameFromPartyCnt` | Returns party code/name from party key |
| `Spu_Sir_insurance_file_UpdateRenewalProduct` | `SetRenewalProductCode` | Sets renewal product code on insurance file |
| `Spu_Sir_insurance_file_SelectRenewalProduct` | `GetRenewalProductCode` | Returns renewal product code from insurance file |
| `spu_CLM_Get_Claim_Payment_AgainstReserveLevel` | `GetClaimPaymentAgainstPeril` | Returns payments at reserve level |
| `spu_update_ClaimPayment_details` | `UpdateClaimPaymentDetails` | Saves claim payment record with tax |
| `spu_CLM_save_recovery` | `UpdateClaimRecoveries` | Saves claim recovery records |
| `spu_Get_Previous_RiskData` | `GetPreviousRiskData` | Returns risk data from prior version |
| `spu_Get_New_Claim_Payment_item` | `GetNewClaimPaymentItem` | Returns populated array for a new claim payment item |
| `spu_GIS_Get_CodeAndIndicator` | `GetCodeAndIndicator` | Returns code + indicator for a lookup value |
| `spu_getUserDefDetailforID` | `GetIdFromCode` | Returns user-defined detail by numeric ID |
| `spu_getUserDefDetail` | `GetIdFromCode`, `MatchPMToGIS` | Returns user-defined detail by code |
| `spu_GetInsuranceFileDetails_Extras` | `GetSumInsured` | Returns extra insurance file fields |
| `spu_pmlookup_rate` | `GetPMLookupRate` | Returns 3D rate value from PM lookup matrix |
| `spu_getBODetails` | `GetBODetails` | Returns back-office details for a policy link |
| `Spu_GIS_GetPartyCnt` | `GetPartyCntFromShortName` | Returns party key from short name |
| `spu_ICCS_[n]_[desc]` | `CallNamedStoredProcedure` | Dynamically named custom integration stored procedure |
| `spu_txn_risk_code_section_sel` | `GetPolicySections`, `CreatePolicyOutputSections` | Returns risk code sections for a policy |
| `spu_txn_section_tax_rate_sel` | `GetInsurerTaxRate` | Returns tax rate for a section/insurer |
| `spu_Get_Table_Data` | `GetTableData` | Safe general-purpose table query |
| `spu_SIR_Check_If_WMTask_Required` | `IsWorkManagerTaskRequiredForRenewal` (property) | Checks if a WM task is required for renewal |
| `spu_SIR_Return_Previous_Fail_Reasons` | `RenewalFailureReasons` (property) | Returns prior term renewal failure reasons |
| `spu_Get_gis_policy_link_RiskType` | `RiskTypeCode` / `RiskTypeDescription` (properties) | Returns risk type code/description |
| `spu_get_task_assignment_sub_details` | `IsProductAutoRenewable`, `PolicyBranch`, etc. (properties) | Returns policy task assignment details |

#### bGISPMUExtras — Component References

| Component | Usage |
|-----------|-------|
| `bGISPMUExtras.PBRiskPolicyCurrency` | Currency conversion helper — held as `m_oPBRiskPolicyCurrency` |
| `bCLMPeril.Business` | Called in `UpdateClaimPaymentDetails` (`bPostPayment=True`) to post payments to Orion |
| `bControlTransClaims.Automated` | Called in `UpdateClaimReserves` (`bPostReserves=True`) to post reserves to Orion |

---

### 16. bPMUList

**Directory:** `Product Builder\list maintenance\bPMUList`
**VB Source Files:** `bPMUListCreate.vb`, `bPMUListCreateCls.vb`
**Purpose:** Generates binary RLDF list files (.DAT and .IDX) used for GIS lookup list maintenance. Reads a text input file and produces two binary output files that the GIS runtime uses to render lookup lists.

**Key Properties:** `OutputFilePath` (String, R/W), `OutputFileDataModel` (String, R/W), `InputFile` (String, R/W), `OutputFile` (String, R/W)

#### Class: bPMUListCreate

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Create` | `() As Integer` | Entry point: validates `InputFile` and `OutputFile` properties, archives existing .DAT/.IDX files, then calls private `BuildOutputDatFile()` and `BuildOutputIdxFile()` to write binary RLDF output | — | — |

#### bPMUList — Stored Procedures

None — this component performs no database operations.

---

### 17. bSIRRuleLookup

**Directory:** `Product Builder\rulelookup\Business\bSIRRuleLookup`
**VB Source Files:** `bSIRRuleLookupBusiness.vb`, `bSIRRuleLookupBusinessSQL.vb`
**Purpose:** Provides full CRUD for GIS rule lookup tables — the header records that name a rule lookup table and the data rows that map codes to values within that table. Used by the Product Builder to define and maintain rule-to-value mappings driving product logic.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init; opens DB connection | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetDataModelCode` | `(lDataModelId, r_sDataModelCode ByRef) As Integer` | Returns the short code for a data model ID | `spu_getDataModelCode` | — |
| `GetNextLineKey` | `(lHeaderKey, r_lNextKey ByRef) As Integer` | Returns the next available data row key for a header | `spu_GetNextLineKey` | — |
| `GetAllLookupData` | `(lHeaderKey, r_vDataSet(,) ByRef, [bSingle, lLineKey]) As Integer` | Returns all data rows for a lookup header; or a single row when `bSingle=True` | `spu_GetAllLookupData`, `spu_Get_SingleLookupData` | — |
| `GetAllLookupHeader` | `(lDataModelId, r_vDataSet(,) ByRef, [bSingle, lHeaderKey]) As Integer` | Returns all header records for a data model; or a single header | `spu_GetAllLookupHeader`, `spu_Get_SingleLookupHeader` | — |
| `UpdateLookupData` | `(lHeaderKey, lLineKey, sCode, sDescription, cRate1, cRate2, cRate3, sInd1, sInd2, sInd3, sInd4) As Integer` | Inserts (ProcessMode=PMAdd) or updates a data row | `spu_AddLookupData` / `spu_UpdLookupData` | — |
| `UpdateLookupHeader` | `(lDataModelId, lHeaderKey, sCode, sTableDescription, lOrderBy, bRate1..Rate3, bInd1..Ind4, sRate1Desc..., sInd1Desc...) As Integer` | Inserts (PMAdd) or updates a header record | `spu_AddLookupHeader` / `spu_UpdLookupHeader` | — |

#### bSIRRuleLookup — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_getDataModelCode` | `GetDataModelCode` | Returns data model code for an ID |
| `spu_GetNextLineKey` | `GetNextLineKey` | Returns next available data row key |
| `spu_GetAllLookupData` | `GetAllLookupData` | Returns all data rows for a header |
| `spu_Get_SingleLookupData` | `GetAllLookupData` (single-row mode) | Returns a single data row |
| `spu_GetAllLookupHeader` | `GetAllLookupHeader` | Returns all header records for a data model |
| `spu_Get_SingleLookupHeader` | `GetAllLookupHeader` (single-header mode) | Returns a single header record |
| `spu_AddLookupData` | `UpdateLookupData` (PMAdd) | Inserts a new data row |
| `spu_UpdLookupData` | `UpdateLookupData` (edit) | Updates an existing data row |
| `spu_DelGisLookupData` | Direct/Admin call | Deletes a data row |
| `spu_AddLookupHeader` | `UpdateLookupHeader` (PMAdd) | Inserts a new header record |
| `spu_UpdLookupHeader` | `UpdateLookupHeader` (edit) | Updates an existing header record |
| `spu_DelGisLookupHeader` | Direct/Admin call | Deletes a header record and its data rows |

---

### 18. bSIRRiskScreen

**Directory:** `Product Builder\screen display\Business\bSIRRiskScreen`
**VB Source Files:** `bSIRRiskScreenBusiness.vb`, `bSIRRiskScreenStateless.vb`, `bSIRRiskScreenBusinessSQL.vb`
**Purpose:** Handles GIS risk screen load and save operations during policy underwriting. Provides two variants: a stateful `Business` class for interactive screen operations, and a `Stateless` class used as an XML RPC wrapper for batch or remote calls.

#### Class: Business (stateful)

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetScreenDetails` | `(lScreenId, r_vDetails(,) ByRef, [lDataModelId]) As Integer` | Returns screen field layout definitions | `spu_full_data_dictionary_sel`, `spe_GIS_screen_sel` | — |
| `Update` | `() As Integer` | Stub — screen save handled via `UpdateRisk_Stateless` | — | — |
| `GetAddress` | `(lAddressCnt, r_vAddress ByRef) As Integer` | Returns address details for a count | `spe_Address_sel` | — |
| `GetParty` | `(lPartyCnt, r_vPartyDetails ByRef) As Integer` | Returns party contact fields | `spu_get_party_dataset` | — |
| `GetPolicy` | `(lInsuranceFileCnt, r_vPolicy ByRef) As Integer` | Returns policy header fields | `spe_Insurance_File_sel` | — |
| `GetStandardWording` | `(lPolicyBinderId, r_vWordings ByRef) As Integer` | Returns standard wordings for a policy binder | `spu_Copy_Standard_Wording` | — |
| `GetSumInsured` | `(lInsuranceFileCnt, r_vSumInsured ByRef) As Integer` | Returns sum insured details | `spu_GetInsuranceFileDetails_Extras` | — |
| `GetGISPolicyLinkDetails` | `(lInsuranceFileCnt, r_vDetails ByRef) As Integer` | Returns GIS policy link supplementary details | `spu_CLM_Get_GIS_Policy_Link_Details` | — |
| `GetOldPolicyLinkId_Stateless` | `(lInsuranceFileCnt, r_lOldPolicyLinkId ByRef) As Integer` | Returns the previous policy link ID for an insurance file | `spu_GIS_Get_Old_Policy_Link_Id` | — |
| `GetBinder_Stateless` | `(r_lBinderId ByRef, r_lInsFileCnt ByRef, [lInsuranceFileCnt, lRiskCnt]) As Integer` | Returns binder for a risk or creates one | `spe_Insurance_File_sel` | — |
| `DeleteRiskCancelledOnAdd_Stateless` | `(lRiskFolderCnt, lInsuranceFileCnt, lRiskCnt) As Integer` | Removes a risk that was cancelled immediately on add | `spu_delete_risk_cancelled_on_add` | — |
| `GetRisk_Stateless` | `(lRiskFolderCnt, lInsuranceFileCnt, lRiskCnt, r_vRiskData ByRef, [bCreateRisk, product/risk/type params...]) As Integer` | Core risk load; creates risk record if `bCreateRisk=True`; returns risk field data | `spe_Risk_Folder_sel`, `spe_Risk_Type_sel`, `spe_Risk_Group_sel`, `spe_Risk_sel`, `spe_Risk_Folder_add`, `spe_Risk_add`, `spe_Insurance_File_Risk_Li_add` | — |
| `GetGISDataModel_Stateless` | `(lRiskCnt, r_lDataModelId ByRef, r_sDataModelCode ByRef) As Integer` | Returns GIS data model from a risk record | `spu_data_model_from_screen` | — |
| `GetObjectKeys` | `(r_vObjectKeys(,) ByRef) As Integer` | Returns key fields from `gis_policy_link` for current policy | `spu_GIS_Get_ObjectKeys` | — |
| `UpdateStandardWording_Stateless` | `(lPolicyBinderId, vWordingArray) As Integer` | Saves standard wordings to policy binder | `spu_Copy_Standard_Wording` | — |
| `UpdateSumInsured_Stateless` | `(lInsuranceFileCnt, sumInsuredParams...) As Integer` | Updates sum insured on an insurance file | `spu_update_insurance_file_sum_insured` | — |
| `UpdateRisk_Stateless` | `(lRiskFolderCnt, lInsuranceFileCnt, lRiskCnt, vRiskData, [bIsFromBOM, bDeleteRiskExtraData, bClearUserDef, bUseExistingTaxDetails]) As Integer` | Full risk save — updates risk folder, risk record, tax details, extra data, UDL columns, and risk link | `spe_Risk_Folder_upd`, `spe_Risk_upd`, `spe_Insurance_File_Risk_Li_upd`, `spu_DeleteTaxDetailsForBatchRenewals`, `spu_GIS_screen_detail_extra_saa`, `spu_GIS_child_screen_detail_saa` | `bGISUserDefLookup.Business` |
| `CopyExtraDetails_Stateless` | `(lFromRiskCnt, lToRiskCnt, lScreenId, dataModelCode, sUserName) As Integer` | Copies screen extra detail and UDL rows from one risk to another | `spu_copy_risk_extras` | — |

#### Class: Stateless (XML RPC wrapper)

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init; creates inner `Business` instance | — | `bSIRRiskScreen.Business` |
| `RiskScreenLoadRisk` | `(riskScreenParams XML, r_sReturnXML ByRef) As Integer` | Deserialises XML request, calls `Business.GetRisk_Stateless` and `GetScreenDetails`, serialises result | `bSIRRiskScreen.Business` methods | `bGIS.Application`, `BPMLOOKUP.Business` |
| `RiskScreenOKClick` | `(riskScreenParams XML, r_sReturnXML ByRef) As Integer` | Deserialises OK-click request, validates and calls `Business.UpdateRisk_Stateless` | `bSIRRiskScreen.Business` methods | `bGIS.Application`, `bGISUserDefLookup.Business` |
| `RiskScreenCancel` | `(riskScreenParams XML) As Integer` | Handles cancel — calls `Business.DeleteRiskCancelledOnAdd_Stateless` if new risk | `bSIRRiskScreen.Business` methods | — |
| `RiskScreenLinkToParty` | `(riskScreenParams XML, r_sReturnXML ByRef) As Integer` | Links a risk to a party; updates claim search address | `bSIRRiskScreen.Business` methods | `bCLMChangeClaimStatus.Business` |
| `UpdateRiskArrayFromGIS` | `(riskScreenParams XML, r_sReturnXML ByRef) As Integer` | Re-reads risk data from GIS and returns updated array | `bSIRRiskScreen.Business` methods | `bGIS.Application` |
| `GetPolicyTypeIDForInsFile` | `(lInsuranceFileCnt, r_lPolicyTypeId ByRef) As Integer` | Returns policy type ID for an insurance file | `spe_Insurance_File_sel` | — |

#### bSIRRiskScreen — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_full_data_dictionary_sel` | `GetScreenDetails` | Returns full data dictionary definitions |
| `spe_GIS_screen_sel` | `GetScreenDetails` | Selects GIS screen definition |
| `spu_GIS_screen_detail_extra_saa` | `UpdateRisk_Stateless` | Saves extra screen detail row for a risk |
| `spu_GIS_child_screen_detail_saa` | `UpdateRisk_Stateless` | Saves child screen detail row for a risk |
| `spu_CLM_Get_GIS_Policy_Link_Details` | `GetGISPolicyLinkDetails` | Returns GIS policy link supplementary fields |
| `spu_delete_risk_cancelled_on_add` | `DeleteRiskCancelledOnAdd_Stateless` | Deletes a risk cancelled immediately on add |
| `spe_Product_sel` | `GetRisk_Stateless` | Selects product definition record |
| `spe_Risk_Type_sel` | `GetRisk_Stateless` | Selects risk type definition |
| `spe_Risk_Group_sel` | `GetRisk_Stateless` | Selects risk group definition |
| `spe_Risk_sel` | `GetRisk_Stateless` | Selects risk record |
| `spe_Insurance_File_sel` | `GetPolicy`, `GetBinder_Stateless`, `GetPolicyTypeIDForInsFile` | Selects insurance file record |
| `spe_Risk_Folder_add` | `GetRisk_Stateless` (create mode) | Inserts a new risk folder |
| `spe_Risk_add` | `GetRisk_Stateless` (create mode) | Inserts a new risk record |
| `spe_Risk_upd` | `UpdateRisk_Stateless` | Updates risk record |
| `spe_Insurance_File_Risk_Li_add` | `GetRisk_Stateless` (create mode) | Inserts insurance file risk link |
| `spe_Insurance_File_Risk_Li_sel` | `UpdateRisk_Stateless` | Selects insurance file risk link |
| `spe_Insurance_File_Risk_Li_del` | `UpdateRisk_Stateless` | Deletes insurance file risk link |
| `spe_Insurance_File_Risk_Li_upd` | `UpdateRisk_Stateless` | Updates insurance file risk link |
| `spu_DeleteTaxDetailsForBatchRenewals` | `UpdateRisk_Stateless` | Deletes stale tax details for batch renewals |
| `spu_data_model_from_screen` | `GetGISDataModel_Stateless` | Returns data model code from screen ID for a risk |
| `spu_Copy_Standard_Wording` | `GetStandardWording`, `UpdateStandardWording_Stateless` | Reads/writes standard wordings for a policy binder |
| `spu_copy_risk_extras` | `CopyExtraDetails_Stateless` | Copies extra risk detail rows between risks |
| `spe_Risk_Folder_sel` | `GetRisk_Stateless` | Selects risk folder record |
| `spe_Risk_Folder_upd` | `UpdateRisk_Stateless` | Updates risk folder record |

#### bSIRRiskScreen — Component References

| Component | Usage |
|-----------|-------|
| `bGIS.Application` | Used in Stateless `RiskScreenLoadRisk`, `RiskScreenOKClick`, `UpdateRiskArrayFromGIS` for BOM operations |
| `BPMLOOKUP.Business` | Used in Stateless `RiskScreenLoadRisk` for PM lookup resolution |
| `bGISUserDefLookup.Business` | Used in `UpdateRisk_Stateless` and Stateless `RiskScreenOKClick` for UDL column save |
| `bCLMChangeClaimStatus.Business` | Used in Stateless `RiskScreenLinkToParty` for claim search address update |

---

### 19. bPBFindControl

**Directory:** `Product Builder\screen editor\Business\bPBFindControl`
**VB Source Files:** `bPBFindControlBusiness.vb`, `bPBFindControlBusinessSQL.vb`
**Purpose:** Manages find/lookup control definitions for GIS screens. Configures which database view drives a lookup field and column mappings between that view and GIS object properties. Opens two database connections: `m_oDatabase` (Solutions) and `m_oSADB` (SiriusArchitecture).

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Start` | `(dbConnection, saConnection) As Integer` | Initialises both database connections | — | — |
| `Initialise` | `(credentials...) As Integer` | Standard init; establishes both DB connections | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `FindData` | `(sViewName, sFilterName, sFilterValue, r_vResultset ByRef) As Integer` | Executes a dynamic `SELECT * FROM [view] WHERE [col] LIKE @filter` (parameterised) against `m_oDatabase` | — (inline SQL) | — |
| `GetMappings` | `(lFindControlId, r_vMappings ByRef) As Integer` | Returns all column-to-property mappings for a find control | — (inline SELECT on `gis_find_mapping`) | — |
| `GetViews` | `(r_vViews ByRef, [sViewPrefix]) As Integer` | Returns view names from `m_oSADB.INFORMATION_SCHEMA.VIEWS` optionally filtered by prefix | — (inline INFORMATION_SCHEMA query on `m_oSADB`) | — |
| `DeleteMappings` | `(lFindControlId) As Integer` | Deletes all mapping rows for a find control | — (inline DELETE on `gis_find_mapping`) | — |
| `GetViewFields` | `(sViewName, r_vFields ByRef) As Integer` | Returns column names for a view from `INFORMATION_SCHEMA.COLUMNS` | — (inline INFORMATION_SCHEMA query on `m_oSADB`) | — |
| `AddMappings` | `(lFindControlId, vMappings(,)) As Integer` | Loops through mapping array; calls `AddMapping` inside a transaction | `spu_GIS_Find_Mapping_Add` (via `AddMapping`) | — |
| `AddMapping` | `(lFindControlId, sColumnName, sPropertyName, lDisplayOrder, lObjectId, lPropertyId, r_lNewId ByRef) As Integer` | Inserts a single column-to-property mapping row; returns new ID | `spu_GIS_Find_Mapping_Add` | — |
| `AddInputParameter` | `(lFindControlId, sColumnName, sPropertyName, lDisplayOrder, lObjectId, lPropertyId, r_lNewId ByRef) As Integer` | Inserts an input parameter mapping row | `spu_GIS_Find_Mapping_Add` | — |
| `AddOutputParameter` | `(lFindControlId, sColumnName, sPropertyName, lDisplayOrder, lObjectId, lPropertyId, r_lNewId ByRef) As Integer` | Inserts an output parameter mapping row | `spu_GIS_Find_Mapping_Add` | — |
| `GetInsuranceFileDetails` | `(lInsuranceFileCnt, r_vDetails ByRef) As Integer` | Returns insurance file header details | `spu_SIR_Get_Insurance_File_Details` | — |

#### bPBFindControl — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_Find_Mapping_Add` | `AddMapping`, `AddInputParameter`, `AddOutputParameter` | Inserts a find control mapping row; returns `NEW_FindControl_Id` |
| `spu_SIR_Get_Insurance_File_Details` | `GetInsuranceFileDetails` | Returns insurance file header fields |

#### bPBFindControl — Component References

| Component | Usage |
|-----------|-------|
| `bCLMChangeClaimStatus.Business` | Used internally by `GetUDLEffectiveDate` (private) to resolve the effective date for UDL_ view filtering |

---

### 20. bSIRListScreen

**Directory:** `Product Builder\screen editor\Business\bSIRListScreen`
**VB Source Files:** `bSIRListScreenBusiness.vb`, `bSIRListScreenBusinessSQL.vb`
**Purpose:** Provides a read-only list of GIS screen records and a bulk update operation for the screen list view in Product Builder. Thin wrapper over two stored procedures.

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init; opens DB connection | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetScreens` | `(lDataModelId, r_vScreens(,) ByRef) As Integer` | Returns all GIS screen records for a data model | `spu_GIS_Screen_saa` | — |
| `UpdateScreens` | `(lScreenId, sScreenCode, sScreenName, lOrderBy, bActive) As Integer` | Updates a GIS screen header record | `spe_GIS_Screen_upd` | — |

#### bSIRListScreen — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_GIS_Screen_saa` | `GetScreens` | Selects all-and-any GIS screen records for a data model |
| `spe_GIS_Screen_upd` | `UpdateScreens` | Updates a single GIS screen header record |

---

### 21. bSIRMaintainScreenData

**Directory:** `Product Builder\screen editor\Business\bSIRMaintainScreenData`
**VB Source Files:** `bSIRMaintainScreenDataBusiness.vb`, `bSIRMaintainScreenDataBusinessSQL.vb`
**Purpose:** Full CRUD for GIS screen definitions in Product Builder. Manages screen headers, field layout rows, and child screen associations. Supports deep-copy of complete screen trees including associated `.Rul` script files. Opens two database connections: `m_oDatabase` (Solutions) and `m_oArcDatabase` (SiriusArchitecture captions DB).

#### Class: Business

| Method | Signature | Purpose | SPs Called | Components Called |
|--------|-----------|---------|------------|-------------------|
| `Initialise` | `(credentials...) As Integer` | Standard init; opens both DB connections | — | — |
| `SetProcessModes` | `([vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate]) As Integer` | Sets process mode properties | — | — |
| `GetDetails` | `(lDataModelId, lScreenId, r_vScreenDetails(,) ByRef, r_vFieldDetails(,) ByRef) As Integer` | Returns screen header plus all field layout rows for a given screen | `spe_GIS_screen_sel`, `spu_GIS_screen_detail_saa`, `spu_GIS_child_screen_detail_saa` | — |
| `GetNext` | `(lDataModelId, bPrevious, r_lScreenId ByRef) As Integer` | Returns the ID of the next (or previous) screen in order | `spu_data_dictionary_sel` | — |
| `EditUpdate` | `([screen header fields + field layout array]) As Integer` | Preview update — validates data and applies to in-memory state without persisting | — | — |
| `Update` | `(lDataModelId, lScreenId, screenHeaderFields, fieldLayoutArray, [bDeleteDetail, lDeleteDetailId]) As Integer` | Full screen persist — inserts/updates header (spe_GIS_screen_add or spu_GIS_Screen_upd_ex), saves/removes field detail rows, updates child screen links | `spe_GIS_screen_add`, `spu_GIS_Screen_upd_ex`, `spe_GIS_screen_detail_add`, `spu_GIS_screen_detail_del`, `spu_GIS_screen_detail_saa` | — |
| `CheckCode` | `(lDataModelId, sScreenCode, lScreenId) As Integer` | Validates that a screen code is unique within a data model | `spu_GIS_screen_code_check` | — |
| `CopyScreen` | `(lSourceScreenId, lTargetDataModelId, sNewScreenCode, sNewScreenName, bIncludeChildScreens, r_lNewScreenId ByRef) As Integer` | Deep-copies a screen definition (and optionally all child screens) including renaming and copying `.Rul` script files from the file system | `spu_GIS_Screen_cop`, `spu_GIS_Screen_detail_cop`, `spu_GIS_Screen_detail_set_child` | — |
| `GetChildScreens` | `(lParentScreenId, r_vChildScreens(,) ByRef) As Integer` | Returns child screen IDs for a parent screen; currently a stub | `spu_GIS_Screen_children` | — |
| `GetScreensByObjectType` | `(lDataModelId, lObjectTypeId, r_vScreens(,) ByRef) As Integer` | Returns screens filtered by object type ID | `spu_GIS_Screen_sel_by_type` | — |
| `GetDataModelTypeId` | `(sDataModelCode, r_lDataModelTypeId ByRef) As Integer` | Returns data model type ID for a data model code | `SPU_GIS_GET_DATA_MODEL_DETAILS_FOR_CODE` | — |

#### bSIRMaintainScreenData — Stored Procedures

| SP Name | Called From | Purpose |
|---------|-------------|---------|
| `spu_data_dictionary_sel` | `GetNext` | Returns next/prev screen ID from data dictionary |
| `spu_spec_data_dictionary_sel` | `GetDetails` (supplementary) | Returns specialised data dictionary entries |
| `spe_GIS_screen_sel` | `GetDetails` | Selects GIS screen header record |
| `spu_GIS_screen_detail_saa` | `GetDetails`, `Update` | Selects-all-and-any screen field detail rows |
| `spu_GIS_child_screen_detail_saa` | `GetDetails` | Returns child screen detail rows |
| `spu_GIS_screen_detail_del` | `Update` (delete mode) | Deletes a screen field detail row |
| `spu_pm_caption_id_return` | `Update` (on `m_oArcDatabase`) | Returns caption ID from the architecture captions DB |
| `spe_GIS_screen_add` | `Update` (PMAdd) | Inserts a new GIS screen header record |
| `spu_GIS_Screen_upd_ex` | `Update` (edit) | Updates an existing GIS screen header record |
| `spe_GIS_screen_detail_add` | `Update` | Inserts a new field detail row for a screen |
| `spu_GIS_screen_code_check` | `CheckCode` | Checks uniqueness of a screen code |
| `spu_GIS_Screen_children` | `GetChildScreens` | Returns child screen IDs for a parent |
| `spu_GIS_Screen_cop` | `CopyScreen` | Inserts the copied screen header and returns new ID |
| `spu_GIS_Screen_detail_cop` | `CopyScreen` | Copies field detail rows to the new screen |
| `spu_GIS_Screen_detail_set_child` | `CopyScreen` | Links a detail row to a child screen |
| `spu_GIS_Screen_sel_by_type` | `GetScreensByObjectType` | Returns screens filtered by object type |
| `SPU_GIS_GET_DATA_MODEL_DETAILS_FOR_CODE` | `GetDataModelTypeId` | Returns data model details for a code |
| `spu_full_data_dictionary_sel` | `Update` (layout validation) | Returns full data dictionary for validation |

---

*End of GIS Combined Business Components Reference*

