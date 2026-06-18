# Sirius For Underwriting — UI Interfaces & Controls Reference

> Detailed reference for all interface (`iPMU*`, `iPMB*`, `iSIR*`) and user control (`uct*`, `cSIR*`) projects in `Sirius For Underwriting\Components\`.
> These are VB.NET Windows Forms projects that provide the underwriting-specific back-office UI layer.
> Referenced from `.github/copilot-instructions.md`.
> **See also:** `.github/docs/underwriting-components-reference.md` for the business components these interfaces call.

---

## Overview

The Sirius For Underwriting UI layer contains **~81 projects** across two categories:

1. **Interface projects (`iPMU*`, `iPMB*`, `iSIR*`)** — Full-screen forms hosted in the Client Manager MDI shell, following the Navigator V3 pattern.
2. **User control projects (`cSIR*`, `uct*`)** — Embeddable controls for reinsurance and clause selection, reused across underwriting screens.

### Naming Convention

| Prefix | Layer | Purpose |
|--------|-------|---------|
| `iPMU*` | **Interface (Underwriting)** | Underwriting-specific screens — the primary UI for this subsystem |
| `iPMB*` | **Interface (Broker)** | Broker/agency portal screens (shared commission level) |
| `iSIR*` | **Interface (Sirius)** | Core Sirius interface screens (peril allocation, commission, MID, etc.) |
| `cSIR*` | **Control (Sirius)** | Reinsurance embedded controls |
| `uct*` | **User Control** | Reusable embedded controls (clause selection) |

### Navigator V3 Pattern

All interface projects follow the standard lifecycle:

```vb
Sub Initialise(sUsername, sPassword, iUserID, iSourceID, ...)
Sub Dispose()
Sub SetKeys(vKeyArray(,))
Sub GetKeys(vKeyArray(,))
Sub GetSummary(vSummaryArray)
Sub SetProcessModes(vTask, vNavigate, vProcessMode, vTransactionType, vEffectiveDate)
Sub Start()

' Navigator V3 wrappers (delegate to above)
Sub NavigatorV3_SetKeys(...)
Sub NavigatorV3_GetKeys(...)
Sub NavigatorV3_GetSummary(...)
Sub NavigatorV3_SetProcessModes(...)
Sub NavigatorV3_Start(...)
```

### How Interfaces Are Hosted

```
Client Manager (iPMBCMManager)
  ??? Navigator V3 (task-based navigation)
        ??? iPMU* Interface Form (SetKeys ? Start ? user interaction ? GetKeys)
              ??? cSIR*/uct* Control (embedded reinsurance/clause UI)
                    ??? bSIR* Business Component (GetDetails, Update, etc.)
```

---

## Complete Interface & Control Inventory

### Interface Projects by Component

| Component | Interface Projects |
|-----------|--------------------|
| **Accumulations** | `iPMUAccumulationValues` |
| **Agent Commission** | `iPMBCommissionLevel`, `ISirAgentCommission` |
| **Auto MTA** | `iPMUAutoMTA` |
| **Batch Renewals** | `iPMUBatchRenewalJobs` |
| **Change Policy Status** | `iPMUChangePolicyStatus` |
| **Chase Cycle** | `iPMUChaseCycleMaint`, `iPMUChaseCycleProcessing` |
| **Claims Stats** | `iPMUClaimsStats` |
| **Client Transfer** | `iSIRClientTransClientSel`, `iSIRClientTransPolicySel` |
| **Clone RI Transfer Auto** | `iPMUCloneRIBatchProcess` |
| **Clone RI Transfer Manual** | `iPMUClonedRIManual` |
| **Coinsurance** | `iPMUCoinsurance` |
| **Commission Rate** | `iSIRCommissionRate` |
| **Cover Note** | `iPMUCoverNote`, `iPMUFindCoverNote` |
| **Data Take On** | `iPMDataAccess` |
| **Deferred RI Auto** | `iPMUDeferredRIAuto` |
| **Deferred RI Manual** | `iPMUDeferredRIManual` |
| **Documentation** | `iPMUGetDocument` |
| **Earning Pattern** | `iSIRRSTEarningPattern` |
| **Find FAC Party** | `iPMUFindRIParty` |
| **Find Policy By Product** | `iPMUPolicyByProduct` |
| **Find Product Type** | `iPMUFindProductType` |
| **Find Risk Type** | `iPMUFindRiskType` |
| **Find Screen** | `iPMUFindScreen` |
| **Follow Up Tasks** | `iPMUFollowUpTasks` |
| **Index Linking** | `iPMUIndexLinkingDetail` |
| **List Policy** | `iPMUListPolicy` |
| **List Policy Version** | `iPMUListPolicyVersion` |
| **List Risks** | `iPMUListRisks` |
| **Lookup Detail** | `iPMULookupDetail`, `iPMULookupDetailRate`, `iPMULookupDetailRates`, `iPMULookupDetails` |
| **Lookup Header** | `iPMULookupHeader`, `iPMULookupHeaderRate`, `iPMULookupHeaderRates`, `iPMULookupHeaders` |
| **MID Maintenance** | `iSIRMIDMaintenance` |
| **Pay Now Options** | `iPMUPayNowOptions` |
| **Peril Allocation** | `iSIRPerilAllocation` |
| **Peril Type Usage** | `iPMUPerilTypeUsage` |
| **Policy** | `iPMUPolicy` |
| **Primary Cause Risk Type** | `iPMUPrimCauseRiskType` |
| **Produce Certificate** | `iPMUProduceCertificates` |
| **Product** | `iPMUProduct`, `iPMUProductMaint` |
| **Quote Collection Process** | `iPMUQuoteCollectionProcess` |
| **Reinsurance** | `iPMUReinsurance` |
| **Renewal** | `iPMURenewal`, `iPMURenewalLaunch`, `iPMURenewalProcess`, `iPMURenInvitePrint`, `iPMURenPreList`, `iPMURenSelection` |
| **Renewal Catch Up** | `iPMURenewalCatchUp` |
| **Repost Transaction** | `iPMURepostTransaction` |
| **RI Band Version** | `iPMURIBandVersion` |
| **RI Model** | `iPMURIModel` |
| **RI Model Usage** | `iPMURIModelUsage` |
| **RI Portfolio Transfer** | `iPMURIPortfolioTransfer`, `iPMURIManPortfolioTransfer` |
| **Risk Type** | `iPMURiskType`, `iPMURiskTypeMaint`, `iPMURiskTypeRuleSet` |
| **Risk Type RI Limits** | `iPMURiskTypeRILimits` |
| **Risk Type RI Values** | `iPMURiskTypeRIValues` |
| **Short Period Rate** | `iPMUShortPeriodRate`, `iPMUShortPeriodRateFind` |
| **Source Defaults** | `iPMUSourceDefaults` |
| **Statistics** | `iPMUStats` |
| **Tax Band Rate** | `iPMUTaxBandRate` |
| **Tax Calculation** | `iPMURITax` |
| **Tax Group Bands** | `iPMUTaxGroupBands` |
| **Treaty** | `iPMUTreaty` |
| **Underwriting Authority** | `iPMUMaintainAURule`, `iPMUMaintainAuthority` |

### User Control Projects

| Component | Control Projects |
|-----------|-----------------|
| **User Controls** | `cSIRRIControls` (reinsurance controls library), `uctSIRSelectClausesControl` (clause selection) |

---

## Interface Project Reference

### Policy & Risk Screens

#### iPMUPolicy
**Purpose:** **Underwriting policy screen** — the main policy-level view for underwriters. Manages policy header, initiates quote/MTA/cancellation workflows, handles silent quotes and policy write (make-live).

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the policy form. |
| `ProcessQuoteAllRisks` / `ProcessQuoteSingleRisks` | Quote all or individual risks. |
| `ProcessSaveQuote` | Save current quote state. |
| `ProcessWritePolicy` | Execute make-live (bind) workflow. |
| `QuoteRisk` | Quote a specific risk. |
| `ReloadRiskTotals` | Refresh premium totals. |
| `SetupPolicyDiscount` | Configure policy-level discount. |

**Business Reference:** `bPMUPolicy`, `bSIRListRisks`

#### iPMUListRisks
**Purpose:** **Risk list screen** — displays all risks on a policy with quote/status management, MTA copy, and risk-level operations.

**Business Reference:** `bSIRListRisks`

#### iPMUListPolicy / iPMUListPolicyVersion
**Purpose:** **Policy list / version list screens** — browse policies for a party, navigate between policy versions.

#### iPMUChangePolicyStatus
**Purpose:** **Change policy status dialog** — cancel, reinstate, or void a policy with accounting period validation.

**Key Methods:** `Start`, `BusinessToInterface`, `GetBusiness`

**Business Reference:** `bSIRChangePolicyStatus`

---

### Renewal Screens

#### iPMURenewal
**Purpose:** **Main renewal screen** — shows renewal list for a policy/agent with accept, amend, lapse, delete, and rerate actions.

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the renewal form. |
| `ShowRenewals` | Load and display renewal list. |
| `ShowPolicyDetail` | Show selected policy detail. |
| `ProcessAccept` / `ProcessLapse` / `ProcessAmendment` / `ProcessDelete` | Renewal workflow actions. |
| `Rerate` | Re-rate the renewal quote. |
| `ValidateCertificateYear` / `ValidateTMPPolicy` | Validation. |
| `PopulateAgentCbo` / `PopulateBranchCbo` | Populate filter dropdowns. |

**Business Reference:** `bSIRRenewal`

#### iPMURenSelection
**Purpose:** **Renewal selection screen** — select policies for renewal, create renewal policy versions, handle TMP anniversary renewals, apply discounts.

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the selection form. |
| `SilentRenewal` | Execute silent (auto) renewal selection. |
| `CreateRenewalPolicyWrapper` | Create renewal policy version. |
| `CreateTMPAnniversaryRenewal` | Create TMP anniversary renewal. |
| `ApplyPolicyDiscount` | Apply discount to renewal. |
| `LockPolicy` | Lock policy during renewal processing. |
| `GetTrueMonthlyPolicyDates` / `GetClosestDate` | TMP date calculations. |

**Business Reference:** `bSIRRenSelection`, `bSIRRenewal`

#### iPMURenewalProcess
**Purpose:** **Renewal batch processing screen** — process multiple renewals with accept/invite/lapse/transfer/set-status actions.

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the processing form. |
| `ProcessAccept` / `ProcessWrite` | Accept/write renewals. |
| `UpdateListView` | Refresh the renewal list. |
| Menu actions: | `mnuRenewalProcessAccept_Click`, `mnuRenewalProcessInvite_Click`, `mnuRenewalProcessLapse_Click`, `mnuRenewalProcessTransfer_Click`, `mnuRenewalProcessDelete_Click`, `mnuRenewalProcessSetStatus_Click`, `mnuRenewalProcessSelectAll_Click` |

**Business Reference:** `bSIRRenewalProcess`

#### iPMURenewalLaunch
**Purpose:** **Renewal launch screen** — initiates batch renewal processing.

**Key Methods:** `Start`, `Main`

**Business Reference:** `bSIRRenewalLaunch`

#### iPMURenInvitePrint
**Purpose:** **Renewal invitation print screen** — print/reprint renewal invitation documents.

**Key Methods:** `Start`, `RePrintRenewalInvite`

**Business Reference:** `bSIRRenInvitePrint`

#### iPMURenPreList
**Purpose:** **Renewal pre-list screen** — preview list of policies due for renewal before processing.

**Business Reference:** `bSIRRenSelection`

#### iPMURenewalCatchUp
**Purpose:** **Renewal catch-up screen** — process overdue/missed renewals.

---

### Reinsurance Screens

#### iPMUReinsurance
**Purpose:** **Reinsurance arrangement screen** — view and manage RI arrangements on a risk, apply auto-RI, process silent RI quotes.

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the reinsurance form. |
| `ProcessAutoRI` | Apply automatic reinsurance. |
| `ProcessOKClickForSilentQuote` | Process RI in silent mode. |
| `IsDisplayRI` | Check if RI should be displayed. |
| `BusinessToInterface` / `DataToBusiness` | Data binding. |

**Business Reference:** `bSIRReinsurance`, `bSIRReinsuranceRI2007`

#### iPMURIModel
**Purpose:** **RI model editor** — create/edit reinsurance models, model lines, currency rates.

**Business Reference:** `bSIRRIModel`

#### iPMURIModelUsage
**Purpose:** **RI model usage screen** — map RI models to risk types, validate assignments.

**Business Reference:** `bSIRRIModelUsage`

#### iPMURIBandVersion
**Purpose:** **RI band version screen** — manage RI band version records.

**Business Reference:** `bSIRRIBandVersion`

#### iPMURIPortfolioTransfer / iPMURIManPortfolioTransfer
**Purpose:** **RI portfolio transfer screens** — `iPMURIPortfolioTransfer` for automated transfers, `iPMURIManPortfolioTransfer` for manual portfolio transfers.

**Business Reference:** `bSIRRIPortfolioTransfer`

#### iPMURiskTypeRILimits
**Purpose:** **Risk type RI limits screen** — configure RI limits per risk type, manage limit versions.

**Business Reference:** `bSIRRiskTypeRILimits`

#### iPMURiskTypeRIValues
**Purpose:** **Risk type RI values screen** — configure RI values per risk type.

**Business Reference:** `bSIRRiskTypeRIValues`

#### iPMUCloneRIBatchProcess
**Purpose:** **Clone RI batch process screen** — UI for triggering cloned RI transfer batch processing.

**Business Reference:** `bSIRCloneRIBatchProcess`

#### iPMUClonedRIManual
**Purpose:** **Clone RI manual screen** — manual execution of cloned RI transfers.

#### iPMUDeferredRIAuto
**Purpose:** **Deferred RI auto screen** — trigger automated deferred RI processing.

**Business Reference:** `bSIRDeferredRIAuto`

#### iPMUDeferredRIManual
**Purpose:** **Deferred RI manual screen** — manual deferred RI processing.

---

### Auto MTA Screen

#### iPMUAutoMTA
**Purpose:** **Automated MTA screen** — the most method-rich interface (44 methods). Orchestrates auto-MTA workflows including backdated MTAs, cancellations, reinstatements, NCD changes, and risk changes. Manages business object creation, agent commission processing, RI recalculation, and tax processing.

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the auto-MTA form. |
| `AutoRunMTA` / `AutoRunReinstatement` | Execute automated MTA/reinstatement. |
| `AutoBackdatedMTA` | Process backdated MTA. |
| `AutoCancelMTA` / `AutoCancelPolicyVersions` | Auto-cancel MTA. |
| `AutoReinstateMTA` / `AutoReinstateRisk` | Auto-reinstate. |
| `AutoNCDChangeMTA` / `AutoRiskChangeMTA` | NCD and risk change MTAs. |
| `QuoteMTA` / `QuoteCancellation` / `QuoteReinstatement` / `QuoteReinstateRisk` | Generate MTA quotes. |
| `TransactPolicyVersions` | Commit MTA transaction. |
| `CopyPolicy` | Copy policy for MTA version. |
| `ProcessAgentCommission` | Recalculate commission. |
| `ProcessRiskReinsurance` | Recalculate reinsurance. |
| `ProcessRITax` | Recalculate RI tax. |
| `MergeExistingMTAChanges` | Merge changes from prior MTA. |
| `CreateBusinessObject` / `CreateBusinessObjectsLocal` / `CreateBusinessObjectsServer` | Create required business components. |

**Business Reference:** `bSIRAutoMTA`, `bSIRListRisks`, `bSIRRITax`, `BSirAgentCommission`

---

### Rating & Tax Screens

#### iSIRPerilAllocation
**Purpose:** **Peril allocation screen** — the core rating interface. Displays rating sections and perils, allows premium entry/override, processes silent quotes, validates annual premiums.

**Key Methods:**
| Method | Description |
|--------|-------------|
| `Start` | Open the peril allocation form. |
| `ProcessOKClickForSilentQuote` | Process rating in silent mode. |
| `ProcessCopiedRisks` | Handle copied risk rating sections. |
| `CheckPremiumOverride` | Validate premium override permissions. |
| `ValidateAnnualPremium` | Validate annual premium value. |
| `GetNetAmount` | Calculate net premium amount. |

**Business Reference:** `bSirPerilAllocation`

#### iPMURITax
**Purpose:** **RI tax screen** — display and edit reinsurance/insurance tax calculations at policy and risk level.

**Key Methods:** `Start`, `BusinessToInterface`, `InterfaceToBusiness`, `GetBusiness`

**Business Reference:** `bSIRRITax`

#### iPMUTaxBandRate / iPMUTaxGroupBands
**Purpose:** Tax configuration screens — `iPMUTaxBandRate` manages tax band rates, `iPMUTaxGroupBands` manages tax group band membership.

---

### Product & Risk Type Configuration Screens

#### iPMUProduct
**Purpose:** **Product editor** — edit product details, risk type groups, causation codes, claims workflow.

**Key Methods:** `Start`, `BusinessToInterface`, `InterfaceToBusiness`, `GetBusiness`, `SetFieldValidation`

**Business Reference:** `bSIRProduct`

#### iPMUProductMaint
**Purpose:** **Product maintenance list** — browse/search products, launch product editor.

**Business Reference:** `bSIRProduct`

#### iPMURiskType
**Purpose:** **Risk type editor** — edit risk type details, GIS screen assignments, clauses.

**Business Reference:** `bSIRRiskType`

#### iPMURiskTypeMaint
**Purpose:** **Risk type maintenance list** — browse/search risk types.

**Business Reference:** `bSIRRiskType`

#### iPMURiskTypeRuleSet
**Purpose:** **Risk type rule set editor** — manage rating rule sets for risk types.

**Business Reference:** `bSIRRiskType`

---

### Commission Screens

#### iPMBCommissionLevel
**Purpose:** **Commission level screen** — view and edit commission levels by product/agent/branch.

**Key Methods:** `Start`, `BusinessToInterface`, `InterfaceToBusiness`, `DisplayLookupDetails`

**Business Reference:** `BSirCommissionRate`

#### ISirAgentCommission
**Purpose:** **Agent commission screen** — view and manage commission entries on a policy.

**Business Reference:** `BSirAgentCommission`

---

### Statistics & Transaction Screens

#### iPMUStats
**Purpose:** **Statistics/transaction posting screen** — processes and posts policy transaction statistics.

**Key Methods:** `Start`, `GetOption`

**Business Reference:** `bControlTrans`

#### iPMUClaimsStats
**Purpose:** **Claims statistics screen** — processes and posts claims transaction statistics.

**Business Reference:** `bControlTransClaims`

#### iPMURepostTransaction
**Purpose:** **Repost transaction screen** — reprocess failed or incorrect transactions.

**Business Reference:** `bSIRRepostTransaction`

---

### Cover Note Screens

#### iPMUCoverNote
**Purpose:** **Cover note editor** — manage cover note books and sheets, assign to policies.

**Key Methods:** `Start`, `BusinessToInterface`, `OnColumnClick`

**Business Reference:** `bSIRCoverNote`

#### iPMUFindCoverNote
**Purpose:** **Cover note search screen** — find cover notes by book, sheet, or policy.

**Key Methods:** `Start`, `DataToInterface`, `DataToProperties`, `GetBusiness`, `DisplayLookupDetails`

**Business Reference:** `bSIRCoverNote`

---

### Chase Cycle Screens

#### iPMUChaseCycleMaint
**Purpose:** **Chase cycle maintenance screen** — configure chase cycle definitions, steps, rules, and timing.

**Business Reference:** `bSIRChaseCycle`

#### iPMUChaseCycleProcessing
**Purpose:** **Chase cycle processing screen** — execute chase cycle steps, auto-cancel policies, produce letters.

**Business Reference:** `bSIRChaseCycle`

---

### Lookup & Configuration Screens

#### iPMULookupDetail / iPMULookupDetailRate / iPMULookupDetailRates / iPMULookupDetails
**Purpose:** **Lookup detail editors** — manage lookup table detail rows. Multiple variants for indicator lookups, single-rate lookups, and multi-rate lookups.

**Business Reference:** `bSIRLookupDetail`, `bSIRLookupDetailRates`

#### iPMULookupHeader / iPMULookupHeaderRate / iPMULookupHeaderRates / iPMULookupHeaders
**Purpose:** **Lookup header editors** — manage lookup table headers. Variants for indicator, single-rate, and multi-rate lookup headers.

**Business Reference:** `bSIRLookupHeader`, `bSIRLookupHeaderRates`

#### iPMUSourceDefaults
**Purpose:** **Source/branch defaults screen** — configure default settings per branch (agents, products).

**Key Methods:** `Start`, `BusinessToInterface`, `InterfaceToBusiness`, `SetFieldValidation`

**Business Reference:** `bPMUSourceDefaults`

#### iPMUShortPeriodRate / iPMUShortPeriodRateFind
**Purpose:** **Short period rate screens** — `iPMUShortPeriodRate` edits rate tables, `iPMUShortPeriodRateFind` searches for rate entries.

**Business Reference:** `bSIRShortPeriodRate`

#### iPMUIndexLinkingDetail
**Purpose:** **Index linking detail screen** — manage index-linked premium adjustment details.

**Business Reference:** `bSIRIndexLinkingDetail`

---

### Underwriting Authority Screens

#### iPMUMaintainAuthority
**Purpose:** **Underwriting authority screen** — manage user authority levels and thresholds.

**Key Methods:** `Start`, `BusinessToInterface`, `GetBusiness`, `GetUserDetails`, `SetFieldValidation`

**Business Reference:** `bSIRMaintainAuthority`

#### iPMUMaintainAURule
**Purpose:** **Underwriting authority rule editor** — create/edit authority rules with GIS data dictionary integration.

**Key Methods:** `Start`, `BusinessToInterface`, `InterfaceToBusiness`, `UpdateBusiness`, `ValidateForm`, `DisplayLookupDetails`

**Business Reference:** `bSIRMaintainAURule`

---

### Other Interfaces

#### iPMUBatchRenewalJobs
**Purpose:** **Batch renewal job configuration screen** — configure and manage batch renewal jobs.

**Business Reference:** `bSIRBatchRenewalJobs`

#### iPMUAccumulationValues
**Purpose:** **Accumulation values screen** — view and repopulate risk accumulation values.

**Key Methods:** `Start`, `GetPolicyInfo`, `RepopulateValues`

**Business Reference:** `bSIRAccumulateValues`

#### iPMUFollowUpTasks
**Purpose:** **Follow-up tasks screen** — create and manage follow-up work manager tasks.

**Business Reference:** `bPMUFollowUpTasks`

#### iPMUPayNowOptions
**Purpose:** **Pay now options screen** — configure payment options for immediate payment scenarios.

**Key Methods:** `Start`, `GetAccountDetails`, `BusinessToInterface`, `InterfaceToData`, `SetUpControls`

**Business Reference:** `bSIRPayNowOptions`

#### iPMUCoinsurance
**Purpose:** **Coinsurance screen** — view and edit coinsurance arrangements.

**Business Reference:** `bSIRCoinsurance`

#### iPMUPerilTypeUsage
**Purpose:** **Peril type usage screen** — manage peril type assignments and earning patterns per product.

**Business Reference:** `bSIRPerilTypeUsage`

#### iPMUPrimCauseRiskType
**Purpose:** **Primary cause risk type screen** — map primary causes to risk type groups for claims.

**Business Reference:** `bSIRPrimCauseRiskType`

#### iPMUProduceCertificates
**Purpose:** **Produce certificates screen** — find policies and generate insurance certificates.

**Business Reference:** `bPMUProduceCertificates`

#### iPMUGetDocument
**Purpose:** **Documentation screen** — check document suppression settings for agents.

**Business Reference:** `bSIRGetDocument`

#### iPMUFindRIParty
**Purpose:** **Find FAC party screen** — search for reinsurance parties, manage broker participants and placement RI lines.

**Business Reference:** `bSIRFindRIParty`

#### iPMUFindProductType / iPMUFindRiskType / iPMUFindScreen
**Purpose:** **Find screens** — search interfaces for product types, risk types, and GIS screens.

**Business Reference:** `bSIRFindProductType`, `bSIRFindRiskType`, `bSIRFindScreen`

#### iPMUPolicyByProduct
**Purpose:** **Find policy by product screen** — search policies filtered by product type.

#### iPMUQuoteCollectionProcess
**Purpose:** **Quote collection process screen** — manage quote collection workflows.

#### iPMDataAccess
**Purpose:** **Data take-on screen** — UI for bulk data import/migration.

#### iSIRMIDMaintenance
**Purpose:** **MID maintenance screen** — manage Motor Insurance Database rules and work manager task configuration.

**Business Reference:** `bSIRMIDMaintenance`

#### iSIRCommissionRate
**Purpose:** **Commission rate screen** — view and edit commission rate arrangements.

**Business Reference:** `BSirCommissionRate`

#### iSIRClientTransClientSel / iSIRClientTransPolicySel
**Purpose:** **Client transfer screens** — `iSIRClientTransClientSel` selects the client, `iSIRClientTransPolicySel` selects policies to transfer.

**Business Reference:** `bSIRClientTransPolicySel`

#### iSIRRSTEarningPattern
**Purpose:** **Earning pattern screen** — manage reinsurance earning pattern configurations.

#### iPMUTreaty
**Purpose:** **Treaty screen** — manage reinsurance treaties, treaty parties, and broker participants.

**Key Methods:** `Start`, `BusinessToInterface`, `GetBusiness`, `Clear`, `GetProperties`, `SetProperties`

**Business Reference:** `bSIRTreaty`

---

## User Control Reference

### cSIRRIControls (Reinsurance Controls Library)
**Directory:** `User Controls/cSIRReinsuranceControls/`
**Purpose:** **Reinsurance embedded controls library.** Contains multiple controls and helper classes for displaying and editing reinsurance data within the policy/claims screens.

**Embedded Controls:**
| Control | Purpose |
|---------|---------|
| `uctRiskReinsuranceControl` | Risk-level reinsurance arrangement display/editor |
| `uctRiskReinsuranceControlRI2007` | RI2007-standard version of risk reinsurance control |
| `uctClaimReinsuranceControl` | Claim-level reinsurance display/editor |
| `uctClaimReinsuranceControlRI2007` | RI2007-standard version of claim reinsurance control |
| `uctRIModelSummaryControl` | RI model summary display |

**Helper Classes:**
| Class | Purpose |
|-------|---------|
| `RiskTotalizer` | Calculates risk totals across rating sections |
| `RiskTotalizer2007` | RI2007-standard risk total calculations |
| `ClaimTotalizer` | Calculates claim totals for reinsurance |
| `ClaimTotalizerRI2007` | RI2007-standard claim total calculations |
| `RiskRIArrangement` | Data class for a risk's RI arrangement details |
| `RIModelCache` | Caches RI model data to avoid repeated DB calls |

**Used By:** `iPMUReinsurance`, `iPMURIModel`, claims screens

---

### uctSIRSelectClausesControl
**Directory:** `User Controls/uctSIRSelectClauses/`
**Purpose:** **Clause selection control.** Embeddable control for selecting and managing clauses linked to products and risk types. Includes a clause selection dialog form.

**Components:**
| Component | Purpose |
|-----------|---------|
| `uctSIRSelectClauses` | Main clause selection user control |
| `frmClauseSelection` | Modal dialog for clause picker |
| `iSIRSelectClause` | Interface definition for clause selection |

**Business Reference:** `bSIRSelectClausesBusiness`

**Used By:** `iPMURiskType`, `iPMUProduct`

---

## Cross-Reference: Interface ? Business Component

| Interface | Business Component | Purpose |
|-----------|--------------------|---------|
| `iPMUPolicy` | `bPMUPolicy`, `bSIRListRisks` | Policy underwriting |
| `iPMUListRisks` | `bSIRListRisks` | Risk list management |
| `iPMUAutoMTA` | `bSIRAutoMTA`, `bSIRListRisks`, `bSIRRITax` | Auto MTA processing |
| `iSIRPerilAllocation` | `bSirPerilAllocation` | Rating/premium calculation |
| `iPMURenewal` | `bSIRRenewal` | Renewal management |
| `iPMURenSelection` | `bSIRRenSelection`, `bSIRRenewal` | Renewal selection |
| `iPMURenewalProcess` | `bSIRRenewalProcess` | Batch renewal processing |
| `iPMURenewalLaunch` | `bSIRRenewalLaunch` | Renewal batch launch |
| `iPMURenInvitePrint` | `bSIRRenInvitePrint` | Renewal invite printing |
| `iPMUReinsurance` | `bSIRReinsurance`, `bSIRReinsuranceRI2007` | Reinsurance arrangements |
| `iPMURIModel` | `bSIRRIModel` | RI model editor |
| `iPMURIModelUsage` | `bSIRRIModelUsage` | RI model-to-risk mapping |
| `iPMURIPortfolioTransfer` | `bSIRRIPortfolioTransfer` | RI portfolio transfer |
| `iPMUCloneRIBatchProcess` | `bSIRCloneRIBatchProcess` | Clone RI batch |
| `iPMUDeferredRIAuto` | `bSIRDeferredRIAuto` | Deferred RI processing |
| `iPMURITax` | `bSIRRITax` | Tax calculation |
| `iPMUProduct` / `iPMUProductMaint` | `bSIRProduct` | Product configuration |
| `iPMURiskType` / `iPMURiskTypeMaint` | `bSIRRiskType` | Risk type configuration |
| `iPMURiskTypeRuleSet` | `bSIRRiskType` | Rule set management |
| `iPMURiskTypeRILimits` | `bSIRRiskTypeRILimits` | RI limits per risk type |
| `iPMURiskTypeRIValues` | `bSIRRiskTypeRIValues` | RI values per risk type |
| `iPMUStats` | `bControlTrans` | Transaction posting |
| `iPMUClaimsStats` | `bControlTransClaims` | Claims transaction posting |
| `iPMURepostTransaction` | `bSIRRepostTransaction` | Transaction repost |
| `iPMUChangePolicyStatus` | `bSIRChangePolicyStatus` | Cancel/reinstate/void |
| `iPMBCommissionLevel` | `BSirCommissionRate` | Commission levels |
| `ISirAgentCommission` | `BSirAgentCommission` | Agent commission |
| `iSIRCommissionRate` | `BSirCommissionRate` | Commission rates |
| `iPMUCoverNote` / `iPMUFindCoverNote` | `bSIRCoverNote` | Cover notes |
| `iPMUChaseCycleMaint` / `iPMUChaseCycleProcessing` | `bSIRChaseCycle` | Chase cycle |
| `iPMUBatchRenewalJobs` | `bSIRBatchRenewalJobs` | Batch renewal config |
| `iPMUAccumulationValues` | `bSIRAccumulateValues` | Accumulations |
| `iPMUFollowUpTasks` | `bPMUFollowUpTasks` | Follow-up tasks |
| `iPMUPayNowOptions` | `bSIRPayNowOptions` | Pay now options |
| `iPMUSourceDefaults` | `bPMUSourceDefaults` | Branch defaults |
| `iPMUMaintainAuthority` / `iPMUMaintainAURule` | `bSIRMaintainAuthority`, `bSIRMaintainAURule` | Underwriting authority |
| `iPMUTreaty` | `bSIRTreaty` | Treaty management |
| `iPMUCoinsurance` | `bSIRCoinsurance` | Coinsurance |
| `iPMUPerilTypeUsage` | `bSIRPerilTypeUsage` | Peril type usage |
| `iPMULookupDetail*` | `bSIRLookupDetail`, `bSIRLookupDetailRates` | Lookup details |
| `iPMULookupHeader*` | `bSIRLookupHeader`, `bSIRLookupHeaderRates` | Lookup headers |
| `iPMUShortPeriodRate*` | `bSIRShortPeriodRate` | Short period rates |
| `iPMUIndexLinkingDetail` | `bSIRIndexLinkingDetail` | Index linking |
| `iPMUPrimCauseRiskType` | `bSIRPrimCauseRiskType` | Primary cause mapping |
| `iPMUProduceCertificates` | `bPMUProduceCertificates` | Certificate production |
| `iSIRMIDMaintenance` | `bSIRMIDMaintenance` | MID rules |
| `iSIRClientTransClientSel` / `iSIRClientTransPolicySel` | `bSIRClientTransPolicySel` | Client transfer |

---

## Cross-Reference: User Control ? Business Component

| Control | Business Component | Hosted In |
|---------|--------------------|-----------|
| `uctRiskReinsuranceControl` | `bSIRReinsurance` | `iPMUReinsurance` |
| `uctRiskReinsuranceControlRI2007` | `bSIRReinsuranceRI2007` | `iPMUReinsurance` |
| `uctClaimReinsuranceControl` | `bSIRReinsurance` | Claims screens |
| `uctClaimReinsuranceControlRI2007` | `bSIRReinsuranceRI2007` | Claims screens |
| `uctRIModelSummaryControl` | `bSIRRIModel` | `iPMURIModel` |
| `uctSIRSelectClauses` | `bSIRSelectClausesBusiness` | `iPMURiskType`, `iPMUProduct` |
