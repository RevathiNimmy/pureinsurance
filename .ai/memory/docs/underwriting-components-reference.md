# Sirius For Underwriting � Components Reference

> Detailed reference for all business components in `Sirius For Underwriting\Components\`.
> These VB.NET components implement underwriting, rating, renewal, reinsurance, and policy lifecycle business logic.
> Referenced from `.github/copilot-instructions.md`.
> **See also:** `.github/docs/back-office-components-reference.md` for Sirius Back Office Core components. `.github/docs/underwriting-ui-controls-reference.md` for all underwriting UI interfaces and controls.

---

## Overview

The `Sirius For Underwriting\Components\` directory contains **~74 business projects** across **42 component directories**. These components handle underwriting-specific logic that sits between the web portal/WCF layer and the core back-office components.

### How Components Are Called

```
Web Portal / WCF SAM / REST API
  ? ProviderSAMForInsuranceV2
      ? bSIR* Underwriting Component (this layer)
          ? bSIR* Back Office Core Component (Insurance File, Party, Event, etc.)
              ? SQL Server Stored Procedures
```

### Key Relationships to Back Office Core

| This Layer | Calls Into | Purpose |
|------------|-----------|---------|
| `bSIRListRisks` | `bSIRInsuranceFile`, `bSIRRiskScreen`, `bSIRRITax` | Risk management calls policy + tax |
| `bSIRRenewal` | `bSIRInsuranceFile`, `bSIRPolicyNumMaint`, `bSIREvent`, `bSIROptions` | Renewal calls core policy + numbering |
| `bSIRAutoMTA` | `bSIRInsuranceFile` | Auto-MTA calls policy management |
| `bSIRRITax` | (standalone) | Tax calculation used by many components |
| `bSIRProduct` | (standalone) | Product configuration |
| `bSIRReinsurance` | (standalone) | RI arrangement management |
| `bControlTrans` | `bACTAccount`, `bACTTransdetail`, `bPMBTransactions` | Statistics calls Orion accounting |

---

## Complete Project Inventory

| Component | Business Projects (`b*`) |
|-----------|--------------------------|
| **Accumulations** | `bSIRAccumulateValues`, `bSIRAccumulationLookup` |
| **Agent Commission** | `BSirAgentCommission` |
| **Auto MTA** | `bSIRAutoMTA` |
| **Batch Controller** | `BatchRenewalWinController`, `BatchRenewalWinControllerCore`, `Sirius.BatchRenewal` |
| **Batch Notification** | `BatchNotificationDocGen`, `BatchNotificationExport` |
| **Batch Quote Deletion** | `bSIRBatchQuoteDeletion` |
| **Batch Renewals** | `bSIRBatchRenewalJobs` |
| **Change Policy Status** | `bSIRChangePolicyStatus` |
| **Chase Cycle** | `bSIRChaseCycle`, `ChaseCycleCLI` |
| **Claims Stats** | `bControlTransClaims` |
| **Client Transfer** | `bSIRClientTransPolicySel` |
| **Clone RI Transfer Auto** | `bSIRCloneRIBatchProcess` |
| **Coinsurance** | `bSIRCoinsurance` |
| **Commission Rate** | `BSirCommissionRate` |
| **Cover Note** | `bSIRCoverNote` |
| **Deferred RI Auto** | `bSIRDeferredRIAuto` |
| **Documentation** | `bSIRGetDocument` |
| **Find FAC Party** | `bSIRFindRIParty` |
| **Find Product Type** | `bSIRFindProductType` |
| **Find Risk** | `bSIRFindRisk` |
| **Find Risk Type** | `bSIRFindRiskType` |
| **Find Screen** | `bSIRFindScreen` |
| **Follow Up Tasks** | `bPMUFollowUpTasks` |
| **Index Linking** | `bSIRIndexLinkingDetail` |
| **List Risks** | `bSIRListRisks` |
| **Lookup Detail** | `bSIRLookupDetail`, `bSIRLookupDetailRates` |
| **Lookup Header** | `bSIRLookupHeader`, `bSIRLookupHeaderRates` |
| **MID Maintenance** | `bSIRMIDMaintenance` |
| **Pay Now Options** | `bSIRPayNowOptions` |
| **Payment Hub Wrapper** | `bSIRPaymentHubWrapper` |
| **Peril Allocation** | `bSirPerilAllocation` |
| **Peril Type Usage** | `bSIRPerilTypeUsage` |
| **Policy** | `bPMUPolicy` |
| **Primary Cause Risk Type** | `bSIRPrimCauseRiskType` |
| **Produce Certificate** | `bPMUProduceCertificates` |
| **Product** | `bSIRProduct` |
| **Quote Engine** | `bGISQEMCOMPILED`, `bGISQEMDRE`, `BGISQEMPMU` |
| **Reinsurance** | `bSIRReinsurance`, `bSIRReinsuranceRI2007` |
| **Reinsurance Transfer** | `ReinsuranceTransfer` |
| **Renewal** | `bSIRAutomaticRenewalsAccept`, `bSIRAutomaticRenewalsInvite`, `bSIRAutomaticRenewalsSel`, `bSIRBatchRenewalController`, `bSIRLaunchAutoRenewalsAccept`, `bSIRLaunchAutoRenewalsInvite`, `bSIRLaunchAutoRenewalsSel`, `bSIRRenewal`, `bSIRRenewalAcceptAgentEmail`, `bSIRRenewalLaunch`, `bSIRRenewalProcess`, `bSIRRenInvitePrint`, `bSIRRenSelection` |
| **Repost Transaction** | `bSIRRepostTransaction` |
| **RI Band Version** | `bSIRRIBandVersion` |
| **RI Model** | `bSIRRIModel` |
| **RI Model Usage** | `bSIRRIModelUsage` |
| **RI Portfolio Transfer** | `bSIRRIPortfolioTransfer` |
| **Risk** | `bSIRRiskData` |
| **Risk Type** | `bSIRRiskType` |
| **Risk Type RI Limits** | `bSIRRiskTypeRILimits` |
| **Risk Type RI Values** | `bSIRRiskTypeRIValues` |
| **Select Clauses** | `bSIRSelectClausesBusiness` |
| **Short Period Rate** | `bSIRShortPeriodRate` |
| **Source Defaults** | `bPMUSourceDefaults` |
| **Statistics** | `bControlTrans` |
| **Tax Band Rate** | `bSIRTaxBandRate` |
| **Tax Calculation** | `bSIRRITax` |
| **Tax Group Bands** | `bSIRTaxGroupBands` |
| **Treaty** | `bSIRTreaty` |
| **Underwriting Authority** | `bSIRMaintainAURule`, `bSIRMaintainAuthority` |

### Interface-Only Directories (no `b*`/`d*` projects)

The following component directories contain only interface (`i*`) projects and have no business logic projects:

`Clone RI Transfer Manual` (`iPMUClonedRIManual`), `Data Take On` (`iPMDataAccess`), `Deferred RI Manual` (`iPMUDeferredRIManual`), `Earning Pattern` (`iSIRRSTEarningPattern`), `Find Policy By Product` (`iPMUPolicyByProduct`), `List Policy` (`iPMUListPolicy`), `List Policy Version` (`iPMUListPolicyVersion`), `Quote Collection Process` (`iPMUQuoteCollectionProcess`), `Renewal Catch Up` (`iPMURenewalCatchUp`)

---

## Component Reference

### Accumulations
**Directory:** `Accumulations/`
**Projects:** `bSIRAccumulateValues`, `bSIRAccumulationLookup`
**Purpose:** Manages risk accumulation values � calculates and stores accumulated risk exposures by address/postcode/region for catastrophe and concentration risk management.

**Business Methods — `bSIRAccumulateValues`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `RepopulateAccumValues` | `() As Integer` | Recalculate all accumulation values across all policies |
| `DeleteValues` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Remove accumulation values for a policy |
| `AddValues` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Add accumulation values for a policy |
| `BuildAccumulationKeys` | `(ByRef r_vKeys(,) As Object) As Integer` | Build lookup keys for accumulation matching |

**Business Methods — `bSIRAccumulationLookup`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `GetCodeFromID` | `(ByVal v_lID As Integer, ByRef r_sCode As String) As Integer` | Get accumulation code from ID |
| `GetEffectiveIDFromCode` | `(ByVal v_sCode As String, ByRef r_lID As Integer) As Integer` | Get effective ID from code |
| `GetEffectiveIDFromID` | `(ByVal v_lID As Integer, ByRef r_lEffectiveID As Integer) As Integer` | Get effective ID from ID |
| `GetLookupValues` | `(ByRef r_vResultArray(,) As Object) As Integer` | Get accumulation lookup data |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_accumulation_by_level_saa` | `GetLookupValues` | Select all accumulation values by level |
| `spu_pm_get_eff_id_from_code` | `GetEffectiveIDFromCode` | Get effective ID from code |
| `spu_pm_get_code_from_id` | `GetCodeFromID` | Get code from ID |
| `spu_pm_get_eff_id_from_id` | `GetEffectiveIDFromID` | Get effective ID from ID |

**References:** `bSIRReinsurance`

---

### Agent Commission
**Directory:** `Agent Commission/`
**Project:** `BSirAgentCommission`
**Purpose:** Manages agent commission calculations and records � add, edit, delete commission entries, calculate commission rates and tax.

**Business Methods — `BSirAgentCommission`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bPMLookup |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `AddAgentCommission` | `(ByRef v_vCommissionArray(,) As Object) As Integer` | Add commission record |
| `EditAgentCommission` | `(ByRef v_vCommissionArray(,) As Object) As Integer` | Edit commission record |
| `DeleteAgentCommission` | `(ByVal v_lCommissionId As Integer) As Integer` | Delete commission record |
| `CalculateAgentCommission` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vCommissionData(,) As Object) As Integer` | Calculate commission amount for an agent |
| `CalculateAgentTax` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vTaxData(,) As Object) As Integer` | Calculate tax on commission |
| `CheckDisplayCommission` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bDisplay As Boolean) As Integer` | Check if commission should be shown |
| `CopyPolicyCommission` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer` | Copy commission to a new policy version |
| `UpdateLeadCommission` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef v_vCommissionData(,) As Object) As Integer` | Update the lead agent's commission |
| `GetallParties` | `(ByRef r_vParties(,) As Object) As Integer` | Get all party records for commission assignment |
| `GetPolicyHeaderDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vHeader(,) As Object) As Integer` | Get policy header for commission display |
| `GetAgentCommission` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vCommission(,) As Object) As Integer` | Load existing commission data |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_agent_commission_sel` | `GetAgentCommission` | Select commission records |
| `spu_sir_agent_commission_add` | `AddAgentCommission` | Add commission |
| `spu_sir_agent_commission_upd` | `EditAgentCommission` | Update commission |
| `spu_sir_agent_commission_del` | `DeleteAgentCommission` | Delete commission |
| `spu_sir_agent_commission_calc` | `CalculateAgentCommission` | Calculate commission |
| `spu_sir_lead_commission_upd` | `UpdateLeadCommission` | Update lead commission |
| `spu_check_display_commission` | `CheckDisplayCommission` | Check display flag |
| `spu_Policy_Commission_Copy` | `CopyPolicyCommission` | Copy commission |
| `spu_SIR_Calculate_Tax_Amounts` | `CalculateAgentTax` | Calculate tax |
| `spu_sir_get_all_parties` | `GetallParties` | Get all parties |

**References:** `bSIRInsuranceFile`, `bSIRParty`, `bPMLookup`

---

### Auto MTA
**Directory:** `Auto MTA/`
**Project:** `bSIRAutoMTA`
**Purpose:** **Automated mid-term adjustment engine.** Handles backdated MTAs, out-of-sequence processing, policy version management, auto-cancellation, and reinstatement. Called by the Instalments component when plan changes require policy amendments.

**Business Methods — `bSIRAutoMTA`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `AutoCancelMTA` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Automatically cancel an MTA |
| `TransactPolicyVersions` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vVersions(,) As Object) As Integer` | Commit MTA transaction across versions |
| `GetBackdatedPolicyVersions` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vVersions(,) As Object) As Integer` | Get versions affected by backdated MTA |
| `GetBackdatedVersions` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vVersions(,) As Object) As Integer` | Get backdated versions for folder |
| `IsBackdatedMTARequired` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_bRequired As Boolean) As Integer` | Check if backdated MTA processing is needed |
| `DeletePolicyVersions` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Remove transient policy versions |
| `GetOverlapQuotes` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuotes(,) As Object) As Integer` | Get overlapping quotes |
| `GetSavedOOSQuotes` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuotes(,) As Object) As Integer` | Get saved out-of-sequence quotes |
| `RestoreAutoRunMTA` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Restore auto-run MTA state after failure |
| `SetNillPremiumRefund` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Set zero-premium refund flag |
| `MarkRiskAsUnquoted` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Reset risk quote status |
| `MultipleVersionsExist` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_bExists As Boolean) As Integer` | Check if multiple versions exist |
| `QuoteMTA` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Generate auto-MTA quote |
| `QuoteCancellation` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Generate auto-cancellation quote |
| `QuoteReinstatement` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Generate auto-reinstatement quote |

**Stored Procedures (34):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_DeleteAllQuotes` | `DeletePolicyVersions` | Delete all quotes |
| `spu_DeleteQuote` | `DeletePolicyVersions` | Delete single quote |
| `spu_Get_AffectedBackDatedMTAVersions` | `GetBackdatedVersions` | Get affected versions |
| `spu_Get_out_of_sequence_mta_details` | `GetSavedOOSQuotes` | Get OOS MTA details |
| `spu_Get_TransactionID_For_Reversal` | `TransactPolicyVersions` | Get transaction for reversal |
| `spu_GetPreviousInsuranceFileCnt` | internal | Get previous ins file |
| `spu_SIR_BackDated_IsDirty_Update` | `TransactPolicyVersions` | Update dirty flag |
| `spu_SIR_Get_BackdatedPolicyVersions` | `GetBackdatedPolicyVersions` | Get backdated versions |
| `spu_SIR_GetFuturePolicyVersions` | internal | Get future versions |
| `spu_SIR_Lapse_OOS_versions` | `TransactPolicyVersions` | Lapse OOS versions |
| `spu_SIR_Restore_MTA_Link` | `RestoreAutoRunMTA` | Restore MTA link |
| `spu_Update_Insurance_File_Details` | internal | Update ins file details |
| `spu_update_risk_sel_status` | `MarkRiskAsUnquoted` | Update risk selection status |

**References:** `bSIRInsuranceFile`

---

### Batch Controller
**Directory:** `BatchController/`
**Projects:** `BatchRenewalWinController`, `BatchRenewalWinControllerCore`, `Sirius.BatchRenewal`
**Purpose:** **Batch renewal execution engine.** Windows application that drives batch renewal processing � retrieves renewal job configurations, loads target insurance files, executes batch renewal steps (selection/invitation/acceptance), and tracks run status.

**Business Methods — `BatchRenewalWinController`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Start` | `() As Integer` | Launch batch renewal processing |
| `RetrieveRenewalJobFromJobCode` | `(ByVal v_sJobCode As String) As Integer` | Load a renewal job configuration by code |
| `RetrieveTargetInsuranceFilesForJob` | `(ByVal v_lJobId As Integer, ByRef r_vFiles(,) As Object) As Integer` | Get target policies for the job |
| `ExecuteInsertBatch` | `(ByRef r_lBatchId As Integer) As Integer` | Insert batch record into accounting |
| `ExecuteInsertInsuranceFolder` | `(ByVal v_lBatchId As Integer, ByVal v_lInsuranceFolderCnt As Integer) As Integer` | Register insurance folder for batch run |
| `InsertRisk` | `(ByVal v_lBatchId As Integer, ByVal v_lRiskId As Integer) As Integer` | Register risk for batch processing |
| `LoadInsuranceFoldersStillOutstandingForBatch` | `(ByVal v_lBatchId As Integer, ByRef r_vFolders(,) As Object) As Integer` | Get folders not yet processed |
| `UpdateBatchTask` | `(ByVal v_lBatchId As Integer, ByVal v_lStatus As Integer) As Integer` | Update batch task status |
| `CheckServiceLevel` | `() As Boolean` | Validate WCF service level |
| `BeginTransaction` | `() As Integer` | Begin DB transaction |
| `CommitTransaction` | `() As Integer` | Commit DB transaction |
| `RollBackTransaction` | `() As Integer` | Rollback DB transaction |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SIRRen_Get_Job_Details` | `RetrieveRenewalJobFromJobCode` | Get job details |
| `spu_SIRRen_Get_Renewal_Selection_Policy_List` | `RetrieveTargetInsuranceFilesForJob` | Get selection policies |
| `spu_SIRRen_Get_Renewal_Invitation_Policy_List` | `RetrieveTargetInsuranceFilesForJob` | Get invitation policies |
| `spu_SIRRen_Get_Renewal_Acceptance_Policy_List` | `RetrieveTargetInsuranceFilesForJob` | Get acceptance policies |
| `spu_Get_Batch_type_id_From_Code` | `ExecuteInsertBatch` | Get batch type |
| `spu_ACT_Add_Batch` | `ExecuteInsertBatch` | Add batch record |
| `spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_add` | `ExecuteInsertInsuranceFolder` | Add folder to run |
| `spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_Outstanding_sel` | `LoadInsuranceFoldersStillOutstandingForBatch` | Get outstanding folders |
| `spu_SIR_Batch_Renewal_Job_Run_Risk_add` | `InsertRisk` | Add risk to run |
| `spu_Update_RenewalBatchTask` | `UpdateBatchTask` | Update batch task |

**References:** `bSIRAutomaticRenewalsAccept`

---

### Batch Notification
**Directory:** `BatchNotification/`
**Projects:** `BatchNotificationDocGen`, `BatchNotificationExport`
**Purpose:** **Batch notification processing.** Two standalone executables: `BatchNotificationDocGen` generates batch notification documents (invoices, statements), `BatchNotificationExport` exports notification data to external systems. Both use direct DB access for batch items.

**Business Methods — `BatchNotificationDocGen`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `(ByVal v_sUsername As String, v_sPassword As String, v_iUserID As Integer, v_iSourceID As Integer, v_sDBConnection As String)` | Constructor with SAM credentials |
| `ProcessDocumentGeneration` | `() As Integer` | Generate documents for all pending batch notification items |

**Business Methods — `BatchNotificationExport`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `(ByVal v_dtStartDate As Date, v_dtEndDate As Date, v_sDBConnection As String)` | Constructor with date range |
| `ProcessExport` | `() As Integer` | Export notifications to external system |
| `GetSystemOption` | `(ByVal v_iOptionNumber As Integer, ByRef r_vValue As Object) As Integer` | Read system option for export config |
| `CloseDBConnection` | `() As Integer` | Close database connection |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_BatchNotification_Batch_sel` | `ProcessDocumentGeneration` | Select batch items |
| `spu_BatchNotification_Batch_summary` | `ProcessDocumentGeneration` | Batch summary |
| `spu_BatchNotification_Batch_Item_add` | `ProcessDocumentGeneration` | Add batch item |
| `spu_BatchNotification_Batch_Item_Status_upd` | `ProcessDocumentGeneration` | Update item status |
| `spu_BatchNotification_Batch_reactivate` | `ProcessDocumentGeneration` | Reactivate batch |
| `spu_BatchNotification_Batch_Failure_sel` | `ProcessDocumentGeneration` | Get failures |
| `spu_BatchNotification_Purge` | `ProcessExport` | Purge old notifications |
| `spu_ACT_Import_CreateBatch` | `ProcessExport` | Create import batch |
| `spu_ACT_Select_Batch_FromBatchRef` | `ProcessExport` | Get batch from ref |

**References:** `bSIROptions`

---

### Batch Quote Deletion
**Directory:** `Batch Quote Deletion/`
**Project:** `bSIRBatchQuoteDeletion`
**Purpose:** Automated batch process to delete expired/old quotes. Runs as a scheduled job.

**Business Methods — `bSIRBatchQuoteDeletion`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `QuoteDeletion` | `() As Integer` | Main entry — deletes expired/old quotes via WCF proxy |
| `CloseDBConnection` | `() As Integer` | Close database connection |
| `CleanUpInterops` | `() As Integer` | Clean up COM interop objects |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_get_quotes_for_auto_delete` | `QuoteDeletion` | Get quotes for auto-deletion |
| `spu_Insert_insurance_file_delete_log` | `QuoteDeletion` | Log deleted insurance files |

**References:** WCF Proxy (`ProxyWS.SAMForInsuranceV2`)

---

### Batch Renewals
**Directory:** `Batch Renewals/`
**Project:** `bSIRBatchRenewalJobs`
**Purpose:** Manages batch renewal job configuration � add, edit, delete jobs, configure agents/branches/products, check active configurations.

**Business Methods — `bSIRBatchRenewalJobs`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `DirectAdd` | `(ByRef v_vJobData(,) As Object) As Integer` | Add renewal job definition |
| `DirectDelete` | `(ByVal v_lJobId As Integer) As Integer` | Delete renewal job |
| `DirectUpdate` | `(ByRef v_vJobData(,) As Object) As Integer` | Update renewal job |
| `GetRenewalJobs` | `(ByRef r_vJobs(,) As Object) As Integer` | List all configured renewal jobs |
| `GetRenewalConfigurationResults` | `(ByVal v_lJobId As Integer, ByRef r_vResults(,) As Object) As Integer` | Get result totals per job |
| `GetJobCode` | `(ByRef r_sJobCode As String) As Integer` | Get unique job code |
| `SuspendJobs` | `() As Integer` | Suspend active renewal jobs |
| `CheckTwoActiveConfigurations` | `(ByRef r_bConflict As Boolean) As Integer` | Validate no conflicting active configs |
| `AddAgent` | `(ByVal v_lJobId As Integer, ByRef v_vAgentData(,) As Object) As Integer` | Link agents to a batch renewal job |
| `PickListLoad` | `(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer` | Load branch/product pick-list |
| `PickListSave` | `(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer` | Save branch/product pick-list |

**Stored Procedures (19):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SIR_Batch_Renewal_Job_add` | `DirectAdd` | Add job |
| `spu_SIR_Batch_Renewal_Job_del` | `DirectDelete` | Delete job |
| `spu_SIR_Batch_Renewal_Job_sel` | `GetRenewalJobs` | Select jobs |
| `spu_SIR_Batch_Renewal_Job_upd` | `DirectUpdate` | Update job |
| `spu_SIR_Add_Agents_Linked_With_BatchRenewal` | `AddAgent` | Add agent link |
| `spu_SIR_Add_Branches_Linked_With_BatchRenewal` | `PickListSave` | Add branch link |
| `spu_SIR_Add_Products_Linked_With_BatchRenewal` | `PickListSave` | Add product link |
| `spu_SIR_Del_Agents_Linked_With_BatchRenewal` | `DirectDelete` | Delete agent links |
| `spu_SIR_Del_Branches_Linked_With_BatchRenewal` | `DirectDelete` | Delete branch links |
| `spu_SIR_Del_Products_Linked_With_BatchRenewal` | `DirectDelete` | Delete product links |
| `spu_SIRRen_Get_Renewal_Selection_Policy_Totals` | `GetRenewalConfigurationResults` | Get selection totals |
| `spu_SIRRen_Get_Renewal_Invitation_Policy_Totals` | `GetRenewalConfigurationResults` | Get invitation totals |
| `spu_SIRRen_Get_Renewal_Acceptance_Policy_Totals` | `GetRenewalConfigurationResults` | Get acceptance totals |

---

### Change Policy Status
**Directory:** `Change Policy Status/`
**Project:** `bSIRChangePolicyStatus`
**Purpose:** Changes policy status (cancel, reinstate, void). Validates accounting periods, renumbers risks, updates premiums.

**Business Methods — `bSIRChangePolicyStatus`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `ChangePolicyStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewStatus As Integer, Optional ByVal v_lReason As Integer = 0, Optional ByVal v_sNotes As String = "", Optional ByVal v_dtEffectiveDate As Date = Nothing, Optional ByVal v_bForceCancel As Boolean = False) As Integer` | Execute the status change (cancel, reinstate, void) |
| `CheckPeriodStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bOpen As Boolean) As Integer` | Validate accounting period is open |
| `GetAccountingPeriodForCoverStartDate` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPeriodId As Integer) As Integer` | Find the relevant posting period |
| `GetPolicySummary` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vSummary(,) As Object) As Integer` | Get policy summary for confirmation |
| `GetRisksByStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lStatusId As Integer, ByRef r_vRisks(,) As Object) As Integer` | Get risks filtered by status |
| `DeleteRisks` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Clean up risks after cancellation |
| `RenumberRisks` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Renumber risks after deletion |
| `UpdatePolicyPremium` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Recalculate policy premium total |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_cancel_all_versions` | `ChangePolicyStatus` | Cancel all versions |
| `spu_reset_all_versions` | `ChangePolicyStatus` | Reset all versions |
| `spu_delete_risk` | `DeleteRisks` | Delete risk |
| `spu_get_policy_summary` | `GetPolicySummary` | Get policy summary |
| `spu_get_risks_by_status` | `GetRisksByStatus` | Get risks by status |
| `spu_renumber_risks` | `RenumberRisks` | Renumber risks |
| `spu_Upd_Policy_Premium` | `UpdatePolicyPremium` | Update premium |

**References:** `bACTPeriod`, `bSIRInsuranceFile`, `bSIRPolicyNumMaint`

---

### Chase Cycle
**Directory:** `ChaseCycle/`
**Project:** `bSIRChaseCycle`
**Purpose:** **Credit control chase cycle engine.** Manages chase cycle definitions (steps, rules, timing), auto-cancel policies, create work manager tasks, and produce client letters. Supports configurable multi-step escalation.

**Business Methods — `bSIRChaseCycle`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `DirectAdd` | `(ByRef v_vItemData(,) As Object) As Integer` | Add chase cycle item |
| `DirectEdit` | `(ByRef v_vItemData(,) As Object) As Integer` | Edit chase cycle item |
| `DirectDelete` | `(ByVal v_lItemId As Integer) As Integer` | Delete chase cycle item |
| `DirectAddRule` | `(ByRef v_vRuleData(,) As Object) As Integer` | Add chase cycle rule |
| `DirectEditRule` | `(ByRef v_vRuleData(,) As Object) As Integer` | Edit chase cycle rule |
| `DirectDeleteRule` | `(ByVal v_lRuleId As Integer) As Integer` | Delete chase cycle rule |
| `DirectAddStep` | `(ByRef v_vStepData(,) As Object) As Integer` | Add chase cycle step |
| `DirectEditStep` | `(ByRef v_vStepData(,) As Object) As Integer` | Edit chase cycle step |
| `DirectDeleteStep` | `(ByVal v_lStepId As Integer) As Integer` | Delete chase cycle step |
| `GetDetails` | `(ByRef r_vDetails(,) As Object) As Integer` | Load chase cycle details |
| `GetList` | `(ByRef r_vList(,) As Object) As Integer` | Load chase cycle list |
| `GetRuleDetails` | `(ByVal v_lRuleId As Integer, ByRef r_vRule(,) As Object) As Integer` | Load rule configuration |
| `GetRuleList` | `(ByRef r_vRules(,) As Object) As Integer` | Load rules list |
| `GetStepDetails` | `(ByVal v_lStepId As Integer, ByRef r_vStep(,) As Object) As Integer` | Load step configuration |
| `GetStepList` | `(ByRef r_vSteps(,) As Object) As Integer` | Load steps list |
| `AutoCancel` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Auto-cancel policy at end of chase |
| `CancelPolicy` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Cancel a specific policy |
| `CreateTask` | `(ByRef v_vTaskData(,) As Object) As Integer` | Create follow-up task |
| `CreateWorkManagerTask` | `(ByRef v_vTaskData(,) As Object) As Integer` | Create work manager task |
| `ProduceClientLetters` | `(ByVal v_lItemId As Integer) As Integer` | Generate chase letters via document production |

**Stored Procedures (33):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SIR_Add_Chase_Cycle_Item` | `DirectAdd` | Add item |
| `spu_SIR_Update_Chase_Cycle_Item` | `DirectEdit` | Update item |
| `spu_SIR_Delete_Chase_Cycle_Item` | `DirectDelete` | Delete item |
| `spu_SIR_Select_Chase_Cycle_Item` | `GetDetails` | Select item |
| `spu_SIR_Add_Chase_Cycle_Rule` | `DirectAddRule` | Add rule |
| `spu_SIR_Update_Chase_Cycle_Rule` | `DirectEditRule` | Update rule |
| `spu_SIR_Delete_Chase_Cycle_Rule` | `DirectDeleteRule` | Delete rule |
| `spu_SIR_Select_Chase_Cycle_Rule` | `GetRuleDetails` | Select rule |
| `spu_SIR_Add_Chase_Cycle_Step` | `DirectAddStep` | Add step |
| `spu_SIR_Update_Chase_Cycle_Step` | `DirectEditStep` | Update step |
| `spu_SIR_Delete_Chase_Cycle_Step` | `DirectDeleteStep` | Delete step |
| `spu_SIR_Select_Chase_Cycle_Step` | `GetStepDetails` | Select step |
| `spu_SIR_Select_Chase_Cycle_AutoCancel` | `AutoCancel` | Get auto-cancel config |
| `spu_SIR_Get_Chase_Cycle_Doc_IDs` | `ProduceClientLetters` | Get document IDs |

**Sub-Component — `ChaseCycleCLI`:**
| Method | Description |
|--------|-------------|
| `DBConnect` | Open database connection |
| `DBDisconnect` | Close database connection |
| SP: `spu_pm_get_lookups` | Get lookup values |

**References:** `bSIRAutoMTA`, `bSIRDocManagerWrapper`, `bSIREvent`

---

### Claims Stats
**Directory:** `Claims Stats/`
**Project:** `bControlTransClaims`
**Purpose:** Creates claims statistics and financial postings � stats folders, stats details, transaction records, journal processing for claim payments/reserves.

**Business Methods — `bControlTransClaims`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Start` | `() As Integer` | Main entry point — process claims statistics |
| `CreateStatsDetails` | `(ByRef v_vStatsData(,) As Object) As Integer` | Create statistics detail records |
| `CreateStatsFolder` | `(ByRef r_lStatsFolderCnt As Integer) As Integer` | Create statistics folder |
| `CreateStatsForCoinsReins` | `(ByVal v_lStatsFolderCnt As Integer) As Integer` | Create coinsurance/reinsurance stats |
| `CreateTransactions` | `(ByRef v_vTransData(,) As Object) As Integer` | Create financial transactions |
| `ProcessTransactions` | `() As Integer` | Post financial transactions |
| `ProcessJournal` | `() As Integer` | Post journal entries |
| `FinaliseStats` | `(ByVal v_lStatsFolderCnt As Integer) As Integer` | Complete stats processing |
| `GetAccountId` | `(ByVal v_sAccountCode As String, ByRef r_lAccountId As Integer) As Integer` | Resolve account identifier |
| `GetLedgerID` | `(ByVal v_sLedgerCode As String, ByRef r_lLedgerId As Integer) As Integer` | Resolve ledger identifier |
| `ReverseStats` | `(ByVal v_lStatsFolderCnt As Integer) As Integer` | Reverse a previous claim transaction |
| `GetBaseCurrencyAmount` | `(ByVal v_cAmount As Decimal, ByRef r_cBaseAmount As Decimal) As Integer` | Currency conversion |

**Stored Procedures (17):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_add_stats_folder_claims` | `CreateStatsFolder` | Create claims stats folder |
| `spu_add_stats_details_claims` | `CreateStatsDetails` | Add claims stats details |
| `spu_add_claims_stats_details_coins` | `CreateStatsForCoinsReins` | Add coinsurance stats |
| `spu_add_claims_stats_details_reins` | `CreateStatsForCoinsReins` | Add reinsurance stats |
| `spu_add_trans_claims_control` | `CreateTransactions` | Add claim transactions |
| `spu_CLM_Finalise_stats` | `FinaliseStats` | Finalise claim stats |
| `spu_CLM_Get_Account_ID` | `GetAccountId` | Get account ID |
| `spu_CLM_Get_Ledger_ID` | `GetLedgerID` | Get ledger ID |

**References:** `bACTPeriod`, `bPMBTransactions`

---

### Client Transfer
**Directory:** `Client Transfer/`
**Project:** `bSIRClientTransPolicySel`
**Purpose:** Transfers policies between clients � selects policies and executes the transfer.

**Business Methods — `bSIRClientTransPolicySel`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `TransferClientPolicies` | `(ByVal v_lSourcePartyCnt As Integer, ByVal v_lTargetPartyCnt As Integer) As Integer` | Transfer policies from source client to target client |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SIR_Transfer_Policy` | `TransferClientPolicies` | Execute policy transfer |

**References:** `bSIREvent`

---

### Clone RI Transfer Auto
**Directory:** `Clone RI Transfer Auto/`
**Project:** `bSIRCloneRIBatchProcess`
**Purpose:** **Automated cloned reinsurance transfer.** Processes batch transfers of cloned RI arrangements � copies policies, risks, ratings, creates stats, handles claims, and updates RI models.

**Business Methods — `bSIRCloneRIBatchProcess`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bSIRRenSelection, bSIRReinsurance, bSIRRITax, bSIRRiskData |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `ProcessSingleClonedRIPolicy` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Process a single cloned RI policy |
| `ProcessSingleClonedRIClaim` | `(ByVal v_lClaimCnt As Integer) As Integer` | Process a single cloned RI claim |
| `CopyPolicyHeader` | `(ByVal v_lOldInsFileCnt As Integer, ByRef r_lNewInsFileCnt As Integer) As Integer` | Copy policy header for RI clone |
| `CopyRisk` | `(ByVal v_lOldInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy risks to cloned policy |
| `CopyClonedRIRisks` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Copy RI-specific risk data |
| `CopyRatings` | `(ByVal v_lOldInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy rating sections |
| `CopyRatingSectionsAndPerils` | `(ByVal v_lOldInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy perils and sections |
| `CreateAndPostStats` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Create and post stats folders |
| `RecalculateRI` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Recalculate RI arrangements |
| `RecalculateRIQuote` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Recalculate RI quote |
| `ValidateBands` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Validate RI band configuration |
| `FinaliseClaimDetails` | `(ByVal v_lClaimCnt As Integer) As Integer` | Finalise claim details |
| `DeletePolicy` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Delete cloned policy |
| `SetPolicyStatus` | `(ByVal v_lInsFileCnt As Integer, ByVal v_lStatus As Integer) As Integer` | Set policy status |
| `UpdateRiskRIModel` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Update risk RI model assignment |
| `CreateEvent` | `(ByVal v_lInsFileCnt As Integer, ByVal v_sDescription As String) As Integer` | Create event log entry |

**Stored Procedures (39):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_Check_Valid_Insurance_File_For_Clone` | `ProcessSingleClonedRIPolicy` | Validate for clone |
| `spu_Risks_Cloned_RI_Status_Sel` | `ProcessSingleClonedRIPolicy` | Get clone RI risk status |
| `spu_recalculate_RI_for_Clone` | `RecalculateRI` | Recalculate RI for clone |
| `spu_Ins_File_Cloned_RI_Usage_sel` | internal | Get clone RI usage |
| `spu_Ins_File_Cloned_RI_Usage_upd` | internal | Update clone RI usage |
| `spu_Claim_Cloned_RI_Usage_sel` | `ProcessSingleClonedRIClaim` | Get claim clone usage |
| `spu_Claim_Cloned_RI_Usage_upd` | `ProcessSingleClonedRIClaim` | Update claim clone usage |

**References:** `bSIRInsuranceFile`, `bSIRRenSelection`, `bSIRReinsurance`, `bSIRRiskData`, `bSIRRITax`

---

### Coinsurance
**Directory:** `Coinsurance/`
**Project:** `bSIRCoinsurance`
**Purpose:** Manages coinsurance arrangements � CRUD for coinsurance records, default values, and retain flags.

**Business Methods — `bSIRCoinsurance`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `GetCoinsurance` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vCoinsurance(,) As Object) As Integer` | Get coinsurance arrangements for policy |
| `GetRetainFlag` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bRetain As Boolean) As Integer` | Get coinsurance retain flag |
| `Update` | `(ByRef v_vCoinsuranceData(,) As Object) As Integer` | Save coinsurance changes |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_COI_Arrangement_add` | `Update` (add) | Add arrangement |
| `spe_COI_Arrangement_del` | `Update` (delete) | Delete arrangement |
| `spe_COI_Arrangement_sel` | `GetCoinsurance` | Select arrangement |
| `spe_COI_Value_add` | `Update` (add) | Add value |
| `spu_COI_Default_saa` | internal | Get defaults |
| `spu_COI_Value_del` | `Update` (delete) | Delete value |
| `spu_COI_Value_saa` | `GetCoinsurance` | Get all values |

---

### Commission Rate
**Directory:** `Commission Rate/`
**Project:** `BSirCommissionRate`
**Purpose:** Manages commission rate arrangements � add, edit, delete rates, calculate rates from configuration, manage commission levels.

**Business Methods — `BSirCommissionRate`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `AddCommissionArrangement` | `(ByRef v_vArrangementData(,) As Object) As Integer` | Add commission rate arrangement |
| `EditCommissionArrangement` | `(ByRef v_vArrangementData(,) As Object) As Integer` | Edit commission rate arrangement |
| `DeleteCommissionArrangement` | `(ByVal v_lArrangementId As Integer) As Integer` | Delete commission rate arrangement |
| `UnDeleteCommissionArrangement` | `(ByVal v_lArrangementId As Integer) As Integer` | Restore soft-deleted arrangement |
| `CalculateCommissionRate` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_dRate As Double) As Integer` | Calculate commission rate for a policy |
| `GetAllCommissionArrangement` | `(ByRef r_vArrangements(,) As Object) As Integer` | Load all arrangements |
| `GetCommissionArrangement` | `(ByVal v_lArrangementId As Integer, ByRef r_vArrangement(,) As Object) As Integer` | Load single arrangement |
| `GetConfiguredCommissionLevel` | `(ByRef r_vLevels(,) As Object) As Integer` | Get configured commission levels |

**Stored Procedures:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_Commission_add` | `AddCommissionArrangement` | Add arrangement |
| `spu_sir_Commission_upd` | `EditCommissionArrangement` | Update arrangement |
| `spu_sir_Commission_del` | `DeleteCommissionArrangement` | Delete arrangement |
| `spu_sir_commission_undel` | `UnDeleteCommissionArrangement` | Undelete |
| `spu_sir_Commission_sel` | `GetCommissionArrangement` | Select arrangement |
| `spu_sir_commission_sel_all` | `GetAllCommissionArrangement` | Select all |
| `spu_sir_Calc_Commission_rate` | `CalculateCommissionRate` | Calculate rate |
| `spu_sir_select_commission_level` | `GetConfiguredCommissionLevel` | Get levels |

---

### Cover Note
**Directory:** `Cover Note/`
**Project:** `bSIRCoverNote`
**Purpose:** Manages cover notes (temporary proof of insurance) � book management, sheet assignment, product linking, validation.

**Business Methods — `bSIRCoverNote`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `AddCoverNoteBook` | `(ByRef v_vBookData(,) As Object) As Integer` | Add a cover note book |
| `EditCoverNoteBook` | `(ByRef v_vBookData(,) As Object) As Integer` | Edit cover note book |
| `DeleteCoverNoteBook` | `(ByVal v_lBookId As Integer) As Integer` | Delete cover note book |
| `SelectCoverNoteBook` | `(ByVal v_lBookId As Integer, ByRef r_vBook(,) As Object) As Integer` | Load cover note book details |
| `FindCoverNoteBook` | `(ByRef r_vBooks(,) As Object) As Integer` | Search for cover note books |
| `AddCoverNoteSheet` | `(ByRef v_vSheetData(,) As Object) As Integer` | Add cover note sheet |
| `EditCoverNoteSheet` | `(ByRef v_vSheetData(,) As Object) As Integer` | Edit cover note sheet |
| `DeleteCoverNoteSheet` | `(ByVal v_lSheetId As Integer) As Integer` | Delete cover note sheet |
| `SelectCoverNoteSheet` | `(ByVal v_lSheetId As Integer, ByRef r_vSheet(,) As Object) As Integer` | Load sheet details |
| `AssignCoverNoteSheet` | `(ByVal v_lSheetId As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Assign sheet to policy/risk |
| `ValidateCoverNoteSheet` | `(ByVal v_sSheetNumber As String, ByRef r_bValid As Boolean) As Integer` | Validate sheet number |
| `GetCoverNoteProducts` | `(ByVal v_lBookId As Integer, ByRef r_vProducts(,) As Object) As Integer` | Get products linked to a book |

**Stored Procedures (17):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SIR_Cover_Note_Book_Add` | `AddCoverNoteBook` | Add book |
| `spu_SIR_Cover_Note_Book_Upd` | `EditCoverNoteBook` | Update book |
| `spu_SIR_Cover_Note_Book_Find` | `FindCoverNoteBook` | Search books |
| `spu_SIR_Cover_Note_Book_Sel` | `SelectCoverNoteBook` | Select book |
| `spu_SIR_Cover_Note_Sheet_Add` | `AddCoverNoteSheet` | Add sheet |
| `spu_SIR_Cover_Note_Sheet_Upd` | `EditCoverNoteSheet` | Update sheet |
| `spu_SIR_Cover_Note_Sheet_Del` | `DeleteCoverNoteSheet` | Delete sheet |
| `spu_SIR_Cover_Note_Sheet_Sel` | `SelectCoverNoteSheet` | Select sheet |
| `spu_SIR_Cover_Note_Sheet_Get` | `SelectCoverNoteSheet` | Get sheet detail |
| `spu_SIR_Cover_Note_Sheet_Assign` | `AssignCoverNoteSheet` | Assign sheet |
| `spu_SIR_Cover_Note_Sheet_Validate` | `ValidateCoverNoteSheet` | Validate sheet |
| `spu_SIR_CoverNoteProducts_sel` | `GetCoverNoteProducts` | Get products |
| `spu_SIR_CoverNoteProducts_add` | internal | Add product link |
| `spu_SIR_CoverNoteProducts_del` | internal | Delete product link |

---

### Deferred RI Auto
**Directory:** `Deferred RI Auto/`
**Project:** `bSIRDeferredRIAuto`
**Purpose:** **Automated deferred reinsurance processing.** Processes deferred RI policy versions � copies policies, risks, recalculates RI, handles claims, creates stats.

**Business Methods — `bSIRDeferredRIAuto`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bSIRRenSelection, bSIRReinsurance, bSIRRITax, bSIREvent |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `ProcessSingleDefRIPolicy` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Process a single deferred RI policy |
| `CopyPolicyHeader` | `(ByVal v_lOldInsFileCnt As Integer, ByRef r_lNewInsFileCnt As Integer) As Integer` | Copy policy header for deferred RI |
| `CopyRisk` | `(ByVal v_lOldInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy risks |
| `CopyDefRIRisks` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Copy deferred RI risk data |
| `CopyRatingSectionsAndPerils` | `(ByVal v_lOldInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy perils and sections |
| `RecalculateRIQuote` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Recalculate RI quote for deferred |
| `CreateAndPostStats` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Create and post stats |
| `CreateEvent` | `(ByVal v_lInsFileCnt As Integer, ByVal v_sDescription As String) As Integer` | Create event log entry |
| `FinaliseClaimDetails` | `(ByVal v_lClaimCnt As Integer) As Integer` | Finalise claim details |
| `DeletePolicy` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Delete deferred policy |
| `SetPolicyStatus` | `(ByVal v_lInsFileCnt As Integer, ByVal v_lStatus As Integer) As Integer` | Set policy status |
| `UpdateDeferredRI_Renewal_Status` | `(ByVal v_lInsFileCnt As Integer) As Integer` | Update deferred RI renewal status |

**Stored Procedures (30):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_Deferred_RI_Change_RI_Model` | `ProcessSingleDefRIPolicy` | Change RI model |
| `spu_Risks_Deferred_RI_Status_Sel` | `ProcessSingleDefRIPolicy` | Get deferred risk status |
| `spu_recalculate_RI_Quote_For_Deferred` | `RecalculateRIQuote` | Recalculate RI |
| `spu_Insurance_File_Deferred_RI_Usage_sel` | internal | Get deferred RI usage |
| `spu_Insurance_File_Deferred_RI_Usage_upd` | internal | Update deferred RI usage |

**References:** `bSIREvent`, `bSIRInsuranceFile`, `bSIRReinsurance`, `bSIRRenSelection`, `bSIRRiskData`, `bSIRRITax`

---

### Find Risk
**Directory:** `Find Risk/`
**Project:** `bSIRFindRisk`
**Purpose:** **Risk search engine.** Searches risks by index, reference, vehicle, or GIS query. Also manages cover note attachment/detachment.

**Business Methods — `bSIRFindRisk`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `SearchByQuery` | `(ByRef r_vResultArray As Object, Optional ByVal v_vInsuranceRef As Object, Optional ByVal v_vInsFileType As Object, Optional ByVal v_vShortName As Object, Optional ByVal v_vVehicleRegNo As Object) As Integer` | Multi-criteria risk search |
| `SearchAll` | `(ByRef r_vResultArray(,) As Object, Optional ByVal v_vInsuranceFileCnt As Integer = 0) As Integer` | Search all risks for policy |
| `SearchInsuranceFile` | `(ByRef r_vResultArray(,) As Object, Optional ByVal v_vInsuranceFileCnt As String = "") As Integer` | Search risks within insurance file |
| `SearchAllGIIM` | `(ByRef r_vResultArray As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lPolicyTypeId As Integer = 0) As Integer` | GII Market search |
| `SearchAllByType` | `(ByRef r_vResultArray As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sPolicyType As String = "", Optional ByVal v_IFSTInsuranceFileType As InsuranceFileSearchType = 0, Optional ByVal v_bIncludeLapsedAndCancelled As Boolean = False) As Integer` | Type-filtered search |
| `FindLikeRef` | `(ByRef sInsuranceRef As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray As Object) As Integer` | Search by insurance reference |
| `FindLikeRefAndHolder` | `(ByRef sInsuranceRef As String, ByRef lInsuranceHolderCnt As Integer, ByRef lNumberOfRecords As Integer, ByRef vResultArray As Object) As Integer` | Search by ref + holder |
| `FindLikeVehicle` | `(ByRef sRegistration As String, ByRef vResultArray As Object) As Integer` | Search by vehicle registration |
| `FindLikeIndex` | `(ByRef sIndex As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray As Object) As Integer` | Search by index |
| `GetInsuranceFolder` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer) As PMEReturnCode` | Get folder for insurance file |
| `GetVersionArray` | `(ByRef r_lInsuranceFileCnt As Integer, ByRef r_vResultArray As Object, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sPolicyNumber As Object = Nothing) As Integer` | Get all versions for policy |
| `GetVersionByDate` | `(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer` | Get version at date |
| `calccombinedkey` | `(ByVal v_lSourceID As Integer, ByVal v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer` | Calculate combined source+key ID |
| `GetPolicyInterface` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sClassName As String) As PMEReturnCode` | Get policy interface class name |
| `SetDefaultSearchFields` | `(ByRef r_sInsRef As String, ByRef r_sShortName As String, Optional ByVal v_lInsuranceFileCnt As Object = Nothing, Optional ByVal v_lInsuranceHolderCnt As Object = Nothing) As Integer` | Set default search fields |
| `GetRiskDescription` | `(ByVal v_sBankingCode As String, ByVal v_sRetailCode As String, ByVal v_sTradeCode As String, ByRef r_vResultArray(,) As Object) As Integer` | Get risk description by codes |
| `GetRiskTypes` | `(ByRef r_sBankingCode As String, ByRef r_sRetailCode As String, ByRef r_sTradeCode As String) As Integer` | Get risk type codes |
| `getUnderwritingOrAgency` | `() As Integer` | Check UW or agency mode |
| `DeleteRisk` | `(ByRef lInsuranceFileCnt As Integer, ByRef lRiskId As Integer) As Integer` | Delete risk from policy |
| `IsRIAtRiskLevel` | `(ByVal lRiskTypeID As Integer) As Integer` | Check RI at risk level |
| `GetTransactionCurrency` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_iCurrencyID As Integer) As Integer` | Get transaction currency |
| `GetOption` | `(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer` | Get system option value |
| `GetHasCurrencyChanged` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bHasCurrencyChanged As Boolean) As Integer` | Check if currency changed |
| `AttachCoverNotes` | `(ByRef r_vCoverNoteArray(,) As Object, ByVal iCounter As Integer) As Integer` | Attach cover notes to risks |
| `DetachCoverNotes` | `(ByVal v_vCoverNoteArray(,) As Object, ByVal iCounter As Integer) As Integer` | Detach cover notes from risks |

**Stored Procedures (17):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_findins_like_ref` | `FindLikeRef` | Search by reference |
| `spu_findins_like_index` | `FindLikeIndex` | Search by index |
| `spu_findins_like_ref_and_holder` | `FindLikeRefAndHolder` | Search by ref + holder |
| `spu_findins_like_vehicle` | `FindLikeVehicle` | Search by vehicle |
| `spu_gii_findins_risk_details` | `SearchAll` | GII risk details |
| `spu_gii_get_vehicle_details` | `SearchAll` | Vehicle details |
| `spe_Insurance_File_Risk_Li_sel` | `SearchInsuranceFile` | Select risk link |
| `spe_Insurance_File_Risk_Li_upd` | `SearchInsuranceFile` | Update risk link |
| `spu_delete_insurance_file_risk_link` | `DeleteRisk` | Delete risk link |
| `spu_GetPolicyRisks` | `SearchAll` | Get policy risks |
| `spu_GetRiskDescription` | `GetRiskDescription` | Get risk description |
| `spu_GetTransactionCurrency` | `GetTransactionCurrency` | Get currency |
| `spu_Get_Has_Currency_Changed` | `GetHasCurrencyChanged` | Currency change check |
| `spu_Attach_Cover_Notes` | `AttachCoverNotes` | Attach cover notes |
| `spu_Detach_Cover_Notes` | `DetachCoverNotes` | Detach cover notes |

**References:** `bSIROptions`

---

### List Risks
**Directory:** `List Risks/`
**Project:** `bSIRListRisks`
**Purpose:** **The largest underwriting component (86 methods, 86 SPs).** Core risk list management � add/delete/copy risks, MTA processing, policy "make live" workflow, discount management, fee/tax recalculation, risk status management, instalment validation, and user authority checks.

**Business Methods — `bSIRListRisks`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bSIRReinsurance, bSirPerilAllocation |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `GetInsuranceFileDetails` | `(ByVal v_lInsuranceFileCnt As Object, ByRef r_vResults(,) As Object, Optional ByRef v_lOriginalInsuranceFileCnt As Object, Optional ByVal bIsSelectLivePlan As Boolean = False) As Integer` | Load insurance file details |
| `AddInputParameter` | `(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer` | Add parameter for dynamic queries |
| `AddRisk` | `(ByVal v_lRiskTypeId As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lProductID As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_lRiskCnt As Integer) As Long` | Add new risk to policy |
| `AddPerils` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Recreate perils after discount |
| `CopyRisksMTA` | `(ByVal v_lInsuranceFileCnt As Long, Optional ByVal v_lCreateLinkType As Long = 1, Optional ByVal Is_SAM_Copy_Quote As Boolean = False, Optional ByVal bFromSAM As Boolean = False, Optional bCopyRiskMTA As Boolean = False) As Integer` | Copy risks for MTA |
| `CopyRisksMTAEx` | `(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal bFromSAM As Boolean = False, Optional ByVal v_lCreateLinkType As Long = 1, Optional ByVal Is_SAM_Copy_Quote As Boolean = False, Optional ByVal v_lOnlyRiskCnt As Integer = 0, Optional ByRef r_lLastNewRiskCnt As Integer = 0, Optional v_bCopyRiskOnMTA As Boolean = False) As Integer` | Extended MTA risk copy |
| `CopyRIDetailsMTA` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal r_lRiskCnt As Integer, ByVal r_lOriginalRiskCnt As Integer) As Integer` | Copy RI details for MTA |
| `CopyGISRiskScreenDetails` | `(ByVal r_lOriginalRiskCnt As Integer, ByVal r_lNewRiskCnt As Integer, ByVal r_lInsFolderCnt As Integer) As Integer` | Copy GIS screen data |
| `CopyRatingSectionsAndPerils` | `(ByVal r_vResultArray(,) As Object, ByVal i_ThisPremiumSign As Integer, ByVal i_OriginalFlag As Integer, ByVal m_lInsuranceFileCnt As Integer, ByVal m_lRiskCnt As Integer, ByRef iIndex As Integer, Optional ByRef dProrata As Double = 0) As Integer` | Copy rating sections and perils |
| `ProcessPolicyMakeLive` | `(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_bIsValid As Boolean = False) As Integer` | Make-live workflow entry point |
| `ProcessPolicyMakeLiveRisks` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Process risks for make-live |
| `ProcessPolicyPreMakeLive` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer, ByRef r_sInvalidRiskMessage As String, ByVal v_lPolicyDiscountStatus As Integer) As Integer` | Pre make-live validation |
| `ProcessApplyDiscount` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer, ByRef r_sFailureReason As String, Optional ByVal crAppliedDiscountPremium As Decimal = 0, Optional ByVal dAppliedDiscountPercentage As Double = 0, Optional ByVal lAppliedMatchDiscountPremium As Integer = 0, Optional ByVal lAppliedDiscountReasonId As Integer = 0) As Integer` | Apply premium discount |
| `ProcessRollbackDiscount` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer) As Integer` | Rollback premium discount |
| `UpdatePolicyDiscounts` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal crDiscountPremium As Decimal, ByVal dDiscountPercentage As Double, ByVal lDiscountReasonId As Integer, ByVal lMatchDiscountPremium As Integer) As Integer` | Update discount values |
| `IsDiscountApplied` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults As Object) As Integer` | Check if discount applied |
| `ClearRatingsDiscountRelatedDetails` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Clear discount-related ratings |
| `GetPolicyDiscountRisks` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get risks with discounts |
| `GetPolicyDiscountTotalPremium` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get total discounted premium |
| `GetInvalidPolicyDiscountRisks` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get risks with invalid discounts |
| `GetPrePaymentOptionValue` | `(ByVal v_lproductid As Integer, ByRef r_Prepayment(,) As Object) As Integer` | Get pre-payment option |
| `RecalculateRiskFees` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTransactionTypeId As Integer) As Integer` | Recalculate risk fees |
| `RecalculateRiskTaxes` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate risk taxes |
| `RecalculatePolicyTaxes` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate policy taxes |
| `RecalculatePolicyFees` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, Optional ByVal v_bUseExistingFeeDetails As Boolean = True) As Integer` | Recalculate policy fees |
| `UpdatePolicyPremium` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update policy premium totals |
| `UpdatePolicyDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPutOnNextInstalmentRenewal As Integer, Optional ByVal v_sPaymentMethod As String = "", Optional ByVal v_lMarkedForCollection As Integer = 0, Optional ByVal v_nCollectionFrequency As Integer = 0, Optional ByVal v_nDOPaymentTerms As Integer = 0) As Integer` | Update policy details |
| `UpdatePolicyPostingPeriod` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPostingPeriodID As Integer) As Integer` | Update posting period |
| `UpdateTaxPremium` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update tax premium totals |
| `UpdateRiskStatus` | `(ByVal v_lRiskCnt As Integer, ByVal v_sRiskStatusCode As String) As Integer` | Update risk status |
| `UpdateRiskSelection` | `(ByVal v_lRiskCnt As Integer, ByVal v_vIsRiskSelected As Object) As Integer` | Update risk selection |
| `UpdateRiskSelectionStatus` | `(ByVal v_vSelectionArray(,) As Object) As Integer` | Batch update risk selections |
| `UpdateRiskVarNo` | `(ByVal v_lRiskNumber As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update risk variation number |
| `UpdateRiskNo` | `(ByVal v_lRiskCnt As Integer, ByRef v_lRiskNumber As Integer) As Integer` | Update risk number |
| `UpdateRiskFolder` | `(ByVal v_lRiskCnt As Long) As Long` | Update risk folder |
| `UpdateRiskDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lIsDiscounted As Integer) As Integer` | Update risk discount flag |
| `UpdateFlaggedQuote` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sFollowUpNote As String, ByVal v_sReferredTo As String, ByVal v_bIsHotQuote As Object) As Integer` | Flag quote for follow-up |
| `UpdateIFRLInkRisk` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer) As Integer` | Update IFRL risk edited flag |
| `UpdateMandatoryRisk` | `(ByVal v_lRiskId As Long) As Long` | Update mandatory risk |
| `GetRiskStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risk statuses for policy |
| `SetRiskStatusArray` | `(ByVal v_vRiskStatus As Object) As Integer` | Set risk statuses from array |
| `ResetRiskStatusForPolicyID` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Reset all risk statuses |
| `SetRiskSelectedValue` | `(ByVal v_lRiskCnt As Integer, ByVal v_iIsSelect As Integer) As Integer` | Set risk selected value |
| `UnquoteMandatoryRisk` | `(ByVal v_lInsuranceFileCnt As Long, ByVal v_lRiskId As Long) As Long` | Reset mandatory risk quote |
| `UnquoteRisksForward` | `(ByVal nRiskCnt As Integer) As Integer` | Unquote forward risks |
| `CheckClaimOnRisk` | `(ByVal v_lRiskId As Long, ByRef v_bRiskHasClaim As Boolean) As Long` | Check if risk has claims |
| `GetNextRiskNo` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRiskNumber As Integer) As Integer` | Get next risk number |
| `GetAttachedInstalmentPlans` | `(ByVal v_nInsurance_FileKey As Integer, ByRef r_oActivePlan As Object) As Integer` | Get attached instalment plans |
| `CheckInstallmentSchemesforMTA` | `(ByRef r_bSchemesExists As Boolean) As Integer` | Check instalment schemes for MTA |
| `GetMTAPaymentTerms` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_bInstalmentsEnabled As Boolean) As Integer` | Get MTA payment terms |
| `GetPaymentTerms` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPMUserID As Integer, ByRef r_bInvoiceEnabled As Boolean, ByRef r_bInstalmentsEnabled As Boolean, ByRef r_bPayNowEnabled As Boolean, Optional ByRef r_bBankGuaranteeEnabled As Boolean = False, Optional ByRef r_bCashDepositEnabled As Boolean = False) As Integer` | Get payment terms |
| `GetPaymentType` | `(ByVal v_lProduct_Id As Integer, ByRef r_PaymentType(,) As Object) As Integer` | Get payment types |
| `GetPaymentMethod` | `(ByVal m_lOriginalInsFileCnt As Integer, ByRef v_Result(,) As Object) As Integer` | Get payment method |
| `GetUserAuthorityDisplayReinsurance` | `(ByVal v_nUserID As Integer, ByRef r_bDisplayReinsurance As Boolean) As Integer` | Check RI display authority |
| `GetUserCanOverridePostingPeriod` | `(ByVal v_lUserID As Integer, ByRef r_bCanOverride As Boolean) As Integer` | Check posting override authority |
| `GetOpenPostingPeriods` | `(ByVal v_dtEffectiveDate As Date, ByRef r_vOpenPostingPeriods(,) As Object) As Integer` | Get open posting periods |
| `GetPolicyVersionDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get policy version details |
| `GetMTAQuotePolicyVersions` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get MTA quote versions |
| `GetPremiumDetailsForAllPolicyVersions` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get premium for all versions |
| `GetTotalPremiumAmountForALLPolicyVersions` | `(ByVal sInsuranceRef As String, ByVal nInsuranceCnt As Integer, ByRef dTotalPremium As Decimal, Optional ByRef dTotalTaxNotAppliedToClient As Decimal = 0.00) As Integer` | Get total premium all versions |
| `GetAutoRenewalFlag` | `(ByVal v_lInsfileCnt As Integer, ByRef r_bAutoRenFlag As Boolean) As Integer` | Get auto-renewal flag |
| `GetAgentDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get agent details |
| `GetAgentType` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get agent type |
| `GetAndValidateSubAgentDetails` | `(ByRef r_bIsValid As Boolean, Optional ByVal v_sPartyCode As String = "", Optional ByVal v_dtCoverStartDate As Date = Nothing, Optional ByVal v_dtCoverEndDate As Date = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer` | Validate sub-agent |
| `GetAndValidateSubAgentDetailsViaInsFile` | `(ByRef r_bIsValid As Boolean, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Validate sub-agent via ins file |
| `GetNoOfPoliciesOnAgent` | `(ByVal v_lLeadAgentCnt As Long, ByRef r_lNoOfPolicies As Long) As Long` | Count policies on agent |
| `GetTransNBAccountId` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get NB account ID |
| `GetRiskTypeDetails` | `(ByVal v_lRiskTypeId As Integer, ByRef r_vArray As Integer) As Long` | Get risk type details |
| `GetCurRiskIdtForOriginalRiskId` | `(ByVal v_lOriginalRiskId As Integer, ByRef r_vCurRiskId(,) As Object) As Integer` | Get current risk for original |
| `GetPMWrkTaskID` | `(ByVal v_sTaskCode As String, ByRef r_vTaskId(,) As Object) As Integer` | Get work task ID |
| `GetUserGroupId` | `(ByVal sUserGroup As String, ByRef o_nUserGroupId As Integer) As Integer` | Get user group ID |
| `GetLookupsByEffectiveDate` | `(ByVal v_sTableName As String, ByRef r_vResults(,) As Object) As Integer` | Get lookups by effective date |
| `GetPolicyRisksForNoChange` | `(ByVal nInsuranceFileCnt As Integer, ByVal v_nRiskId As Integer, ByRef r_oResults(,) As Object) As Integer` | Get risks for no-change MTA |
| `GetPolicyRisksForAutoQuote` | `(ByVal nInsuranceFileCnt As Integer, ByRef r_oResults(,) As Object) As Integer` | Get risks for auto-quote |
| `ValidateCertificateYear` | `(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer` | Validate certificate year |
| `ProcessPolicyReceiptMediaTypeStatus` | `(ByVal v_lInsuranceFileId As Integer, ByRef r_bProceed As Boolean) As Integer` | Check receipt media type |
| `IsSubsequentRiskVersionsEdited` | `(ByVal v_lRiskID As Integer, ByVal v_dtMTAEffectiveDate As Date) As Integer` | Check if subsequent versions edited |
| `CreateWorkTask` | `(ByVal v_sTaskCode As String, ByVal v_sDescription As String, ByRef r_vKeyArray(,) As Object, Optional ByVal v_lUserGroupID As Integer = 0, Optional ByVal v_lUserID As Object = Nothing) As Integer` | Create work manager task |
| `RemoveBlankKeys` | `(ByRef r_vKeyArray(,) As Object) As Integer` | Remove blank keys from array |
| `DeletePFPremiumFinance` | `(ByVal nInsuranceFileCnt As Integer) As Integer` | Delete PF premium finance |

**Stored Procedures (86):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_get_next_risk_no` | `GetNextRiskNo` | Get next risk number |
| `spu_update_risk_no` | `UpdateRiskNo` | Update risk number |
| `spu_get_next_risk_var_no` | `UpdateRiskVarNo` | Get next variation number |
| `spu_update_risk_var_no` | `UpdateRiskVarNo` | Update variation number |
| `spu_update_risk_sel_status` | `UpdateRiskSelectionStatus` | Bulk update selection status |
| `spu_update_flagged_quote` | `UpdateFlaggedQuote` | Update flagged quote |
| `spu_get_follow_up_time_frame` | `UpdateFlaggedQuote` | Get follow-up time frame |
| `spu_Update_IFRLink_Risk_Edited` | `UpdateIFRLInkRisk` | Update risk edited flag |
| `spu_SIR_Get_Insurance_File_Details` | `GetInsuranceFileDetails` | Get insurance file details |
| `spu_SIR_Risk_Selection_Status_Update` | `UpdateRiskSelection` | Update risk selection |
| `spu_SIR_Update_Policy_Details` | `UpdatePolicyDetails` | Update policy details |
| `spu_SIR_Policy_Discount_Get_Risks` | `GetPolicyDiscountRisks` | Get discount risks |
| `spu_SIR_Policy_Discount_Recalculate_Risk_Fees` | `RecalculateRiskFees` | Recalculate risk fees |
| `spu_SIR_Policy_Discount_Get_Total_Premium` | `GetPolicyDiscountTotalPremium` | Get total premium |
| `spu_SIR_Policy_Discount_Apply` | `ProcessApplyDiscount` | Apply discount |
| `spu_SIR_Policy_Discount_Adjust` | `ProcessApplyDiscount` | Adjust discount |
| `spu_Upd_Policy_Premium` | `UpdatePolicyPremium` | Update policy premium |
| `spu_SIR_Policy_Discount_Update_Policy_Risk_Values` | `ProcessApplyDiscount` | Update risk values |
| `spu_SIR_Policy_Discount_Adjust_Values_Fees` | `ProcessApplyDiscount` | Adjust fees |
| `spu_SIR_Policy_Discount_Adjust_Values_Taxes` | `ProcessApplyDiscount` | Adjust taxes |
| `spu_SIR_Policy_Discount_Get_Required_Info` | `ProcessApplyDiscount` | Get required info |
| `spu_SIR_Policy_Discount_Update_Risk_Details` | `UpdateRiskDetails` | Update risk details |
| `spu_SIR_Policy_Discount_Rollback` | `ProcessRollbackDiscount` | Rollback discount |
| `spu_SIR_Policy_Discount_Process_Make_Live_Ratings` | `ClearRatingsDiscountRelatedDetails` | Clear discount ratings |
| `spu_SIR_Policy_Discount_Process_Make_Live_Risks` | `ProcessPolicyMakeLiveRisks` | Process make-live risks |
| `spu_SIR_Policy_Discount_Recreate_Perils` | `AddPerils` | Recreate perils |
| `spu_SIR_Policy_Discount_Get_Invalid_Risks` | `GetInvalidPolicyDiscountRisks` | Get invalid risks |
| `spu_SIR_Update_Risk_Status` | `UpdateRiskStatus` | Update risk status |
| `spu_SIR_Policy_Discount_Update_Risk_Tax_Premium` | `UpdateTaxPremium` | Update risk tax premium |
| `spu_SIR_Policy_Discount_Update_Policy_Tax_Premium` | `UpdateTaxPremium` | Update policy tax premium |
| `spu_SIR_Get_Lookup_Values_By_Effective_Date` | `GetLookupsByEffectiveDate` | Get lookups |
| `spu_update_risk_status_unquoted_mtc` | `ResetRiskStatusForPolicyID` | Reset risk statuses |
| `spu_RI_arrangement_duplicate` | `CopyRIDetailsMTA` | Duplicate RI arrangement |
| `spu_sir_rating_section_sel_original` | `CopyRatingSectionsAndPerils` | Select original rating sections |
| `spu_sir_peril_allocation` | `CopyRatingSectionsAndPerils` | Peril allocation |
| `spu_get_pro_rata_rate` | `CopyRatingSectionsAndPerils` | Get pro-rata rate |
| `spu_get_prorata_flag` | `CopyRatingSectionsAndPerils` | Get pro-rata flag |
| `spu_get_ProrataRate_for_UneditedRisk` | `CopyRatingSectionsAndPerils` | Pro-rata for unedited risk |
| `spu_update_risk_values` | `CopyRatingSectionsAndPerils` | Update risk values |
| `spu_RI_Arrangement_Details_upd` | `CopyRIDetailsMTA` | Update RI details |
| `spu_SIR_Get_Payment_Terms` | `GetPaymentTerms` | Get payment terms |
| `spu_SIR_Get_MTA_Payment_Terms` | `GetMTAPaymentTerms` | Get MTA payment terms |
| `spu_Get_Payment_Method` | `GetPaymentMethod` | Get payment method |
| `spu_Update_Insurance_File_Discount` | `UpdatePolicyDiscounts` | Update discount |
| `spu_IsDiscountApplied` | `IsDiscountApplied` | Check discount |
| `spu_SIR_Select_Open_Posting_Periods` | `GetOpenPostingPeriods` | Get open periods |
| `spu_SIR_Update_Policy_Posting_Period` | `UpdatePolicyPostingPeriod` | Update posting period |
| `spu_SIR_Get_UserCanOverride_PostingPeriod` | `GetUserCanOverridePostingPeriod` | Check override authority |
| `spu_SIR_Select_Policy_Version` | `GetPolicyVersionDetails` | Select policy version |
| `Spu_Get_PolicyCoverDatesAndProductIDFromRisk` | `CopyRisksMTAEx` | Get cover dates |
| `spu_get_stats_folder_count` | `ProcessPolicyMakeLive` | Get stats folder count |
| `spu_SIR_GetAutoRenewalFlag` | `GetAutoRenewalFlag` | Get auto-renewal flag |
| `spu_Get_Agent` | `GetAgentDetails` | Get agent details |
| `spu_SIR_Get_MTAQuotePolicyVersions` | `GetMTAQuotePolicyVersions` | Get MTA versions |
| `spu_get_PMWrk_task_ID` | `GetPMWrkTaskID` | Get work task ID |
| `spu_Get_Premium_Details_For_All_Policy_Versions` | `GetPremiumDetailsForAllPolicyVersions` | Get premiums |
| `spu_SIR_Check_Policy_Receipt_MediaType_Status` | `ProcessPolicyReceiptMediaTypeStatus` | Check media type |
| `spe_Risk_sel` | `GetRiskTypeDetails` | Select risk |
| `spe_Risk_Folder_sel` | `UpdateRiskFolder` | Select risk folder |
| `spe_Risk_Folder_add` | `UpdateRiskFolder` | Add risk folder |
| `spu_update_risk_folder_for_risk` | `UpdateRiskFolder` | Update risk folder |
| `spu_Get_Sub_Agent_Detail` | `GetAndValidateSubAgentDetails` | Get sub-agent detail |
| `spu_Select_SubAgents` | `GetAndValidateSubAgentDetailsViaInsFile` | Select sub-agents |
| `spu_Get_No_Of_Policies_On_Agent` | `GetNoOfPoliciesOnAgent` | Count policies |
| `spu_Get_PrePaymentOptionValue` | `GetPrePaymentOptionValue` | Get pre-payment option |
| `spe_Risk_Type_sel` | `GetRiskTypeDetails` | Select risk type |
| `spu_SIR_Mandatory_Risk_Sel` | `AddRisk` | Select mandatory risk |
| `spu_SIR_Update_GIS_Policy_Link` | `CopyGISRiskScreenDetails` | Update GIS link |
| `spu_SIR_Update_Mandatory_Risk` | `UpdateMandatoryRisk` | Update mandatory risk |
| `spu_SIR_Update_Mandatory_Risk_Details` | `UpdateMandatoryRisk` | Update mandatory details |
| `spu_SIR_Get_Mandatory_Risk` | `AddRisk` | Get mandatory risk |
| `spu_get_claims_cnt_on_risk` | `CheckClaimOnRisk` | Check claims on risk |
| `spu_Unquote_Risks_Forward` | `UnquoteRisksForward` | Unquote forward risks |
| `spu_Get_User_Group_Id_For_WrkTask_Instance` | `GetUserGroupId` | Get user group ID |
| `spu_Get_Policy_Risks_For_No_Change` | `GetPolicyRisksForNoChange` | Get no-change risks |
| `spe_User_Authorities_sel` | `GetUserAuthorityDisplayReinsurance` | Select user authorities |
| `spu_get_policy_live_plans` | `GetAttachedInstalmentPlans` | Get live plans |
| `spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions` | `GetTotalPremiumAmountForALLPolicyVersions` | Get total premium |
| `spu_Get_Policy_Risks_For_AutoQuote` | `GetPolicyRisksForAutoQuote` | Get auto-quote risks |
| `spu_Delete_PFPremiumFinance` | `DeletePFPremiumFinance` | Delete PF finance |

**References:** `bSIRReinsurance` / `bSIRReinsuranceRI2007`, `bSirPerilAllocation`, `bSIRRITax`, `bSIRPartyFee`, `bSIRRoadmap`, `bSIRRiskData`, `bSIRFindInsurance`, `bGIS`, `bSIRRiskScreen`s_SAM_Copy_Quote As Boolean = False, Optional ByVal v_lOnlyRiskCnt As Integer = 0, Optional ByRef r_lLastNewRiskCnt As Integer = 0, Optional v_bCopyRiskOnMTA As Boolean = False) As Integer` | Extended MTA risk copy |
| `CopyRIDetailsMTA` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal r_lRiskCnt As Integer, ByVal r_lOriginalRiskCnt As Integer) As Integer` | Copy RI details for MTA |
| `CopyGISRiskScreenDetails` | `(ByVal r_lOriginalRiskCnt As Integer, ByVal r_lNewRiskCnt As Integer, ByVal r_lInsFolderCnt As Integer) As Integer` | Copy GIS screen data |
| `CopyRatingSectionsAndPerils` | `(ByVal r_vResultArray(,) As Object, ByVal i_ThisPremiumSign As Integer, ByVal i_OriginalFlag As Integer, ByVal m_lInsuranceFileCnt As Integer, ByVal m_lRiskCnt As Integer, ByRef iIndex As Integer, Optional ByRef dProrata As Double = 0) As Integer` | Copy rating sections and perils |
| `ProcessPolicyMakeLive` | `(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_bIsValid As Boolean = False) As Integer` | Make-live workflow entry point |
| `ProcessPolicyMakeLiveRisks` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Process risks for make-live |
| `ProcessPolicyPreMakeLive` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer, ByRef r_sInvalidRiskMessage As String, ByVal v_lPolicyDiscountStatus As Integer) As Integer` | Pre make-live validation |
| `ProcessApplyDiscount` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer, ByRef r_sFailureReason As String, Optional ByVal crAppliedDiscountPremium As Decimal = 0, Optional ByVal dAppliedDiscountPercentage As Double = 0, Optional ByVal lAppliedMatchDiscountPremium As Integer = 0, Optional ByVal lAppliedDiscountReasonId As Integer = 0) As Integer` | Apply premium discount |
| `ProcessRollbackDiscount` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer) As Integer` | Rollback premium discount |
| `UpdatePolicyDiscounts` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal crDiscountPremium As Decimal, ByVal dDiscountPercentage As Double, ByVal lDiscountReasonId As Integer, ByVal lMatchDiscountPremium As Integer) As Integer` | Update discount values |
| `IsDiscountApplied` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults As Object) As Integer` | Check if discount applied |
| `ClearRatingsDiscountRelatedDetails` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Clear discount-related ratings |
| `GetPolicyDiscountRisks` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get risks with discounts |
| `GetPolicyDiscountTotalPremium` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get total discounted premium |
| `GetInvalidPolicyDiscountRisks` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get risks with invalid discounts |
| `GetPrePaymentOptionValue` | `(ByVal v_lproductid As Integer, ByRef r_Prepayment(,) As Object) As Integer` | Get pre-payment option |
| `RecalculateRiskFees` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTransactionTypeId As Integer) As Integer` | Recalculate risk fees |
| `RecalculateRiskTaxes` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate risk taxes |
| `RecalculatePolicyTaxes` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate policy taxes |
| `RecalculatePolicyFees` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, Optional ByVal v_bUseExistingFeeDetails As Boolean = True) As Integer` | Recalculate policy fees |
| `UpdatePolicyPremium` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update policy premium totals |
| `UpdatePolicyDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPutOnNextInstalmentRenewal As Integer, Optional ByVal v_sPaymentMethod As String = "", Optional ByVal v_lMarkedForCollection As Integer = 0, Optional ByVal v_nCollectionFrequency As Integer = 0, Optional ByVal v_nDOPaymentTerms As Integer = 0) As Integer` | Update policy details |
| `UpdatePolicyPostingPeriod` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPostingPeriodID As Integer) As Integer` | Update posting period |
| `UpdateTaxPremium` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update tax premium totals |
| `UpdateRiskStatus` | `(ByVal v_lRiskCnt As Integer, ByVal v_sRiskStatusCode As String) As Integer` | Update risk status |
| `UpdateRiskSelection` | `(ByVal v_lRiskCnt As Integer, ByVal v_vIsRiskSelected As Object) As Integer` | Update risk selection |
| `UpdateRiskSelectionStatus` | `(ByVal v_vSelectionArray(,) As Object) As Integer` | Batch update risk selections |
| `UpdateRiskVarNo` | `(ByVal v_lRiskNumber As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update risk variation number |
| `UpdateRiskNo` | `(ByVal v_lRiskCnt As Integer, ByRef v_lRiskNumber As Integer) As Integer` | Update risk number |
| `UpdateRiskFolder` | `(ByVal v_lRiskCnt As Long) As Long` | Update risk folder |
| `UpdateRiskDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lIsDiscounted As Integer) As Integer` | Update risk discount flag |
| `UpdateFlaggedQuote` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sFollowUpNote As String, ByVal v_sReferredTo As String, ByVal v_bIsHotQuote As Object) As Integer` | Flag quote for follow-up |
| `UpdateIFRLInkRisk` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer) As Integer` | Update IFRL risk edited flag |
| `UpdateMandatoryRisk` | `(ByVal v_lRiskId As Long) As Long` | Update mandatory risk |
| `GetRiskStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risk statuses for policy |
| `SetRiskStatusArray` | `(ByVal v_vRiskStatus As Object) As Integer` | Set risk statuses from array |
| `ResetRiskStatusForPolicyID` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Reset all risk statuses |
| `SetRiskSelectedValue` | `(ByVal v_lRiskCnt As Integer, ByVal v_iIsSelect As Integer) As Integer` | Set risk selected value |
| `UnquoteMandatoryRisk` | `(ByVal v_lInsuranceFileCnt As Long, ByVal v_lRiskId As Long) As Long` | Reset mandatory risk quote |
| `UnquoteRisksForward` | `(ByVal nRiskCnt As Integer) As Integer` | Unquote forward risks |
| `CheckClaimOnRisk` | `(ByVal v_lRiskId As Long, ByRef v_bRiskHasClaim As Boolean) As Long` | Check if risk has claims |
| `GetNextRiskNo` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRiskNumber As Integer) As Integer` | Get next risk number |
| `GetAttachedInstalmentPlans` | `(ByVal v_nInsurance_FileKey As Integer, ByRef r_oActivePlan As Object) As Integer` | Get attached instalment plans |
| `CheckInstallmentSchemesforMTA` | `(ByRef r_bSchemesExists As Boolean) As Integer` | Check instalment schemes for MTA |
| `GetMTAPaymentTerms` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_bInstalmentsEnabled As Boolean) As Integer` | Get MTA payment terms |
| `GetPaymentTerms` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPMUserID As Integer, ByRef r_bInvoiceEnabled As Boolean, ByRef r_bInstalmentsEnabled As Boolean, ByRef r_bPayNowEnabled As Boolean, Optional ByRef r_bBankGuaranteeEnabled As Boolean = False, Optional ByRef r_bCashDepositEnabled As Boolean = False) As Integer` | Get payment terms |
| `GetPaymentType` | `(ByVal v_lProduct_Id As Integer, ByRef r_PaymentType(,) As Object) As Integer` | Get payment types |
| `GetPaymentMethod` | `(ByVal m_lOriginalInsFileCnt As Integer, ByRef v_Result(,) As Object) As Integer` | Get payment method |
| `GetUserAuthorityDisplayReinsurance` | `(ByVal v_nUserID As Integer, ByRef r_bDisplayReinsurance As Boolean) As Integer` | Check RI display authority |
| `GetUserCanOverridePostingPeriod` | `(ByVal v_lUserID As Integer, ByRef r_bCanOverride As Boolean) As Integer` | Check posting override authority |
| `GetOpenPostingPeriods` | `(ByVal v_dtEffectiveDate As Date, ByRef r_vOpenPostingPeriods(,) As Object) As Integer` | Get open posting periods |
| `GetPolicyVersionDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get policy version details |
| `GetMTAQuotePolicyVersions` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get MTA quote versions |
| `GetPremiumDetailsForAllPolicyVersions` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get premium for all versions |
| `GetTotalPremiumAmountForALLPolicyVersions` | `(ByVal sInsuranceRef As String, ByVal nInsuranceCnt As Integer, ByRef dTotalPremium As Decimal, Optional ByRef dTotalTaxNotAppliedToClient As Decimal = 0.00) As Integer` | Get total premium all versions |
| `GetAutoRenewalFlag` | `(ByVal v_lInsfileCnt As Integer, ByRef r_bAutoRenFlag As Boolean) As Integer` | Get auto-renewal flag |
| `GetAgentDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get agent details |
| `GetAgentType` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get agent type |
| `GetAndValidateSubAgentDetails` | `(ByRef r_bIsValid As Boolean, Optional ByVal v_sPartyCode As String = "", Optional ByVal v_dtCoverStartDate As Date = Nothing, Optional ByVal v_dtCoverEndDate As Date = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer` | Validate sub-agent |
| `GetAndValidateSubAgentDetailsViaInsFile` | `(ByRef r_bIsValid As Boolean, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Validate sub-agent via ins file |
| `GetNoOfPoliciesOnAgent` | `(ByVal v_lLeadAgentCnt As Long, ByRef r_lNoOfPolicies As Long) As Long` | Count policies on agent |
| `GetTransNBAccountId` | `(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get NB account ID |
| `GetRiskTypeDetails` | `(ByVal v_lRiskTypeId As Integer, ByRef r_vArray As Integer) As Long` | Get risk type details |
| `GetCurRiskIdtForOriginalRiskId` | `(ByVal v_lOriginalRiskId As Integer, ByRef r_vCurRiskId(,) As Object) As Integer` | Get current risk for original |
| `GetPMWrkTaskID` | `(ByVal v_sTaskCode As String, ByRef r_vTaskId(,) As Object) As Integer` | Get work task ID |
| `GetUserGroupId` | `(ByVal sUserGroup As String, ByRef o_nUserGroupId As Integer) As Integer` | Get user group ID |
| `GetLookupsByEffectiveDate` | `(ByVal v_sTableName As String, ByRef r_vResults(,) As Object) As Integer` | Get lookups by effective date |
| `GetPolicyRisksForNoChange` | `(ByVal nInsuranceFileCnt As Integer, ByVal v_nRiskId As Integer, ByRef r_oResults(,) As Object) As Integer` | Get risks for no-change MTA |
| `GetPolicyRisksForAutoQuote` | `(ByVal nInsuranceFileCnt As Integer, ByRef r_oResults(,) As Object) As Integer` | Get risks for auto-quote |
| `ValidateCertificateYear` | `(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer` | Validate certificate year |
| `ProcessPolicyReceiptMediaTypeStatus` | `(ByVal v_lInsuranceFileId As Integer, ByRef r_bProceed As Boolean) As Integer` | Check receipt media type |
| `IsSubsequentRiskVersionsEdited` | `(ByVal v_lRiskID As Integer, ByVal v_dtMTAEffectiveDate As Date) As Integer` | Check if subsequent versions edited |
| `CreateWorkTask` | `(ByVal v_sTaskCode As String, ByVal v_sDescription As String, ByRef r_vKeyArray(,) As Object, Optional ByVal v_lUserGroupID As Integer = 0, Optional ByVal v_lUserID As Object = Nothing) As Integer` | Create work manager task |
| `RemoveBlankKeys` | `(ByRef r_vKeyArray(,) As Object) As Integer` | Remove blank keys from array |
| `DeletePFPremiumFinance` | `(ByVal nInsuranceFileCnt As Integer) As Integer` | Delete PF premium finance |

**Stored Procedures (86):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_get_next_risk_no` | `GetNextRiskNo` | Get next risk number |
| `spu_update_risk_no` | `UpdateRiskNo` | Update risk number |
| `spu_get_next_risk_var_no` | `UpdateRiskVarNo` | Get next variation number |
| `spu_update_risk_var_no` | `UpdateRiskVarNo` | Update variation number |
| `spu_update_risk_sel_status` | `UpdateRiskSelectionStatus` | Bulk update selection status |
| `spu_update_flagged_quote` | `UpdateFlaggedQuote` | Update flagged quote |
| `spu_get_follow_up_time_frame` | `UpdateFlaggedQuote` | Get follow-up time frame |
| `spu_Update_IFRLink_Risk_Edited` | `UpdateIFRLInkRisk` | Update risk edited flag |
| `spu_SIR_Get_Insurance_File_Details` | `GetInsuranceFileDetails` | Get insurance file details |
| `spu_SIR_Risk_Selection_Status_Update` | `UpdateRiskSelection` | Update risk selection |
| `spu_SIR_Update_Policy_Details` | `UpdatePolicyDetails` | Update policy details |
| `spu_SIR_Policy_Discount_Get_Risks` | `GetPolicyDiscountRisks` | Get discount risks |
| `spu_SIR_Policy_Discount_Recalculate_Risk_Fees` | `RecalculateRiskFees` | Recalculate risk fees |
| `spu_SIR_Policy_Discount_Get_Total_Premium` | `GetPolicyDiscountTotalPremium` | Get total premium |
| `spu_SIR_Policy_Discount_Apply` | `ProcessApplyDiscount` | Apply discount |
| `spu_SIR_Policy_Discount_Adjust` | `ProcessApplyDiscount` | Adjust discount |
| `spu_Upd_Policy_Premium` | `UpdatePolicyPremium` | Update policy premium |
| `spu_SIR_Policy_Discount_Update_Policy_Risk_Values` | `ProcessApplyDiscount` | Update risk values |
| `spu_SIR_Policy_Discount_Adjust_Values_Fees` | `ProcessApplyDiscount` | Adjust fees |
| `spu_SIR_Policy_Discount_Adjust_Values_Taxes` | `ProcessApplyDiscount` | Adjust taxes |
| `spu_SIR_Policy_Discount_Get_Required_Info` | `ProcessApplyDiscount` | Get required info |
| `spu_SIR_Policy_Discount_Update_Risk_Details` | `UpdateRiskDetails` | Update risk details |
| `spu_SIR_Policy_Discount_Rollback` | `ProcessRollbackDiscount` | Rollback discount |
| `spu_SIR_Policy_Discount_Process_Make_Live_Ratings` | `ClearRatingsDiscountRelatedDetails` | Clear discount ratings |
| `spu_SIR_Policy_Discount_Process_Make_Live_Risks` | `ProcessPolicyMakeLiveRisks` | Process make-live risks |
| `spu_SIR_Policy_Discount_Recreate_Perils` | `AddPerils` | Recreate perils |
| `spu_SIR_Policy_Discount_Get_Invalid_Risks` | `GetInvalidPolicyDiscountRisks` | Get invalid risks |
| `spu_SIR_Update_Risk_Status` | `UpdateRiskStatus` | Update risk status |
| `spu_SIR_Policy_Discount_Update_Risk_Tax_Premium` | `UpdateTaxPremium` | Update risk tax premium |
| `spu_SIR_Policy_Discount_Update_Policy_Tax_Premium` | `UpdateTaxPremium` | Update policy tax premium |
| `spu_SIR_Get_Lookup_Values_By_Effective_Date` | `GetLookupsByEffectiveDate` | Get lookups |
| `spu_update_risk_status_unquoted_mtc` | `ResetRiskStatusForPolicyID` | Reset risk statuses |
| `spu_RI_arrangement_duplicate` | `CopyRIDetailsMTA` | Duplicate RI arrangement |
| `spu_sir_rating_section_sel_original` | `CopyRatingSectionsAndPerils` | Select original rating sections |
| `spu_sir_peril_allocation` | `CopyRatingSectionsAndPerils` | Peril allocation |
| `spu_get_pro_rata_rate` | `CopyRatingSectionsAndPerils` | Get pro-rata rate |
| `spu_get_prorata_flag` | `CopyRatingSectionsAndPerils` | Get pro-rata flag |
| `spu_get_ProrataRate_for_UneditedRisk` | `CopyRatingSectionsAndPerils` | Pro-rata for unedited risk |
| `spu_update_risk_values` | `CopyRatingSectionsAndPerils` | Update risk values |
| `spu_RI_Arrangement_Details_upd` | `CopyRIDetailsMTA` | Update RI details |
| `spu_SIR_Get_Payment_Terms` | `GetPaymentTerms` | Get payment terms |
| `spu_SIR_Get_MTA_Payment_Terms` | `GetMTAPaymentTerms` | Get MTA payment terms |
| `spu_Get_Payment_Method` | `GetPaymentMethod` | Get payment method |
| `spu_Update_Insurance_File_Discount` | `UpdatePolicyDiscounts` | Update discount |
| `spu_IsDiscountApplied` | `IsDiscountApplied` | Check discount |
| `spu_SIR_Select_Open_Posting_Periods` | `GetOpenPostingPeriods` | Get open periods |
| `spu_SIR_Update_Policy_Posting_Period` | `UpdatePolicyPostingPeriod` | Update posting period |
| `spu_SIR_Get_UserCanOverride_PostingPeriod` | `GetUserCanOverridePostingPeriod` | Check override authority |
| `spu_SIR_Select_Policy_Version` | `GetPolicyVersionDetails` | Select policy version |
| `Spu_Get_PolicyCoverDatesAndProductIDFromRisk` | `CopyRisksMTAEx` | Get cover dates |
| `spu_get_stats_folder_count` | `ProcessPolicyMakeLive` | Get stats folder count |
| `spu_SIR_GetAutoRenewalFlag` | `GetAutoRenewalFlag` | Get auto-renewal flag |
| `spu_Get_Agent` | `GetAgentDetails` | Get agent details |
| `spu_SIR_Get_MTAQuotePolicyVersions` | `GetMTAQuotePolicyVersions` | Get MTA versions |
| `spu_get_PMWrk_task_ID` | `GetPMWrkTaskID` | Get work task ID |
| `spu_Get_Premium_Details_For_All_Policy_Versions` | `GetPremiumDetailsForAllPolicyVersions` | Get premiums |
| `spu_SIR_Check_Policy_Receipt_MediaType_Status` | `ProcessPolicyReceiptMediaTypeStatus` | Check media type |
| `spe_Risk_sel` | `GetRiskTypeDetails` | Select risk |
| `spe_Risk_Folder_sel` | `UpdateRiskFolder` | Select risk folder |
| `spe_Risk_Folder_add` | `UpdateRiskFolder` | Add risk folder |
| `spu_update_risk_folder_for_risk` | `UpdateRiskFolder` | Update risk folder |
| `spu_Get_Sub_Agent_Detail` | `GetAndValidateSubAgentDetails` | Get sub-agent detail |
| `spu_Select_SubAgents` | `GetAndValidateSubAgentDetailsViaInsFile` | Select sub-agents |
| `spu_Get_No_Of_Policies_On_Agent` | `GetNoOfPoliciesOnAgent` | Count policies |
| `spu_Get_PrePaymentOptionValue` | `GetPrePaymentOptionValue` | Get pre-payment option |
| `spe_Risk_Type_sel` | `GetRiskTypeDetails` | Select risk type |
| `spu_SIR_Mandatory_Risk_Sel` | `AddRisk` | Select mandatory risk |
| `spu_SIR_Update_GIS_Policy_Link` | `CopyGISRiskScreenDetails` | Update GIS link |
| `spu_SIR_Update_Mandatory_Risk` | `UpdateMandatoryRisk` | Update mandatory risk |
| `spu_SIR_Update_Mandatory_Risk_Details` | `UpdateMandatoryRisk` | Update mandatory details |
| `spu_SIR_Get_Mandatory_Risk` | `AddRisk` | Get mandatory risk |
| `spu_get_claims_cnt_on_risk` | `CheckClaimOnRisk` | Check claims on risk |
| `spu_Unquote_Risks_Forward` | `UnquoteRisksForward` | Unquote forward risks |
| `spu_Get_User_Group_Id_For_WrkTask_Instance` | `GetUserGroupId` | Get user group ID |
| `spu_Get_Policy_Risks_For_No_Change` | `GetPolicyRisksForNoChange` | Get no-change risks |
| `spe_User_Authorities_sel` | `GetUserAuthorityDisplayReinsurance` | Select user authorities |
| `spu_get_policy_live_plans` | `GetAttachedInstalmentPlans` | Get live plans |
| `spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions` | `GetTotalPremiumAmountForALLPolicyVersions` | Get total premium |
| `spu_Get_Policy_Risks_For_AutoQuote` | `GetPolicyRisksForAutoQuote` | Get auto-quote risks |
| `spu_Delete_PFPremiumFinance` | `DeletePFPremiumFinance` | Delete PF finance |

**References:** `bSIRReinsurance` / `bSIRReinsuranceRI2007`, `bSirPerilAllocation`, `bSIRRITax`, `bSIRPartyFee`, `bSIRRoadmap`, `bSIRRiskData`, `bSIRFindInsurance`, `bGIS`, `bSIRRiskScreen`

---

### Peril Allocation
**Directory:** `Peril Allocation/`
**Project:** `bSirPerilAllocation`
**Purpose:** **Core rating engine.** Manages rating sections, perils, premium calculation, coinsurance application, and risk status updates. The central component for premium computation.

**Business Methods — `bSirPerilAllocation`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `CalculatePremium` | `(ByVal v_lRatingSectionTypeId As Integer, ByVal v_cSumInsured As Decimal, ByRef v_cAnnualPremium As Decimal, ByRef v_cThisPremium As Decimal, ByRef v_cAnnualRate As Decimal) As Integer` | Calculate premium for rating section |
| `AddSectionAndPerils` | `(ByVal v_lRatingSectionTypeId As Integer, ByVal v_lPolicySectionTypeId As Integer, ByVal v_cAnnualPremium As Decimal, ByVal v_cThisPremium As Decimal, ByVal v_cAnnualRate As Double, ByVal v_cSumInsured As Decimal, ByVal v_lRateTypeId As Integer, ByVal v_lOriginalFlag As Integer, ByVal v_iDefinedCurrencyID As Integer, ByVal v_lCountryID As Integer, ByVal v_lStateID As Integer, ByVal v_iIsAmended As Integer, ByVal v_cCalculatedPremium As Decimal, ByVal v_sOverrideReason As String, ByVal v_lEarningPatternId As Integer) As Integer` | Add rating section with perils |
| `AddSectionAndPerils` (overload) | `(..., ByVal v_iAutoCalculated As Integer, ByVal v_lEarningPatternId As Integer) As Integer` | Add section with auto-calculated flag |
| `DeleteSectionAndPerils` | `() As Integer` | Delete current rating section and perils |
| `GetInsuranceHeaderDetails` | `(ByRef r_sInsuranceHolderShortName As String, ByRef r_sInsuranceHolderName As String, ByRef r_sInsuranceHolderResolvedName As String, ByRef r_sInsuranceRef As String, ByRef r_sInsuranceFolderDescription As String, ByRef r_sInsuranceCurrencyCode As String, ByRef r_sInsuranceCurrencyCaption As String, ByRef r_iInsuranceCurrencyID As Integer, ByRef r_lInsuranceCompanyID As Integer) As Integer` | Get insurance header details |
| `GetDataModel` | `(ByRef sGISDataModel As String) As Integer` | Get GIS data model for risk |
| `PopulateRatingSections` | `(ByRef r_vResultArray As Object) As Integer` | Load rating sections |
| `PopulateRatingSections` (overload) | `(ByRef r_vResultArray As Object, ByRef v_bIsBackdatedMTA As Boolean) As Integer` | Load with backdated MTA flag |
| `PopulateRatingSections` (overload) | `(ByRef r_vResultArray As Object, ByRef v_bIsBackdatedMTA As Boolean, ByRef r_lPostChangeRiskCnt As Integer, ByRef v_dtMTADateCurrent As Date, ByRef v_bExistsPreAndPost As Boolean, v_sRiskMergeStatus As String) As Integer` | Load with MTA merge details |
| `PopulateRatingSections` (overload) | `() As Integer` | Load (no output param) |
| `PopulateRatingSectionsFromExistingSections` | `(ByVal v_lPreviousDeletedRiskInsuranceFileCnt As Integer, ByRef r_vResultArray As Object) As Integer` | Load from deleted risk sections |
| `PopulateRatingSectionsFromExistingSections` (overload) | `(..., ByVal v_bIsBackdatedMTA As Boolean) As Integer` | Load from deleted with MTA flag |
| `GetOriginalAnnualPremium` | `(ByRef lRiskRatingSectionId As Integer, ByRef lPolicyRatingSectionId As Integer, ByRef cOriginalAnnualPremium As Decimal) As Integer` | Get original annual premium |
| `GetOriginalAnnualPremium` (overload) | `(..., ByRef lRatingSectionCnt As Integer) As Integer` | Get with section count |
| `GetRatingSections` | `(ByRef vResultArray(,) As Object) As Integer` | Get all rating sections |
| `GetRatingSectionTypes` | `(ByRef vResultArray(,) As Object) As Integer` | Get rating section types |
| `GetRatingSectionType_ForRiskType` | `(ByVal lMode As Integer, ByVal lRiskCnt As Integer, ByVal lRatingSectionTypeId As Integer, ByVal lOriginalRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get section types for risk type |
| `GetRateTypes` | `(ByRef vResultArray(,) As Object) As Integer` | Get rate types |
| `GetRatingSectionTypeTax` | `(ByVal v_lRatingSectionTypeId As Integer, ByRef r_dTaxRate As Double) As Integer` | Get tax rate for section type |
| `GetPerilAllocationSecurity` | `(ByVal lRiskCnt As Integer, ByVal iUserID As Integer, ByRef r_bUserAllowRatingSectionAddDelete As Boolean, ByRef r_bUserAllowRatingSectionEdit As Boolean, ByRef r_bAllowRatingSectionAdd As Boolean, ByRef r_bAllowRatingSectionEdit As Boolean, ByRef r_bAllowRatingSectionDelete As Boolean, ByRef r_bAllowEditRatingSectionRateType As Boolean, ByRef r_bAllowEditRatingSectionRate As Boolean, ByRef r_bAllowEditRatingSectionSumInsured As Boolean, ByRef r_bAllowEditRatingSectionThisPremium As Boolean) As Integer` | Get user security permissions |
| `GetRisksBilledPremium` | `(ByVal v_lRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get billed premium for risk |
| `GetTransactionType` | `() As Integer` | Get current transaction type |
| `GetUWProductOptions` | `(ByVal v_lProductID As Integer) As Integer` | Get UW product options |
| `GetRoundingSectionAmounts` | `(ByRef r_cAnnualPremium As Decimal, ByRef r_cThisPremium As Decimal) As Integer` | Get rounding amounts |
| `GetRoundingSectionAmounts` (overload) | `(..., ByVal v_lOriginal_Flag As Integer) As Integer` | Get rounding with original flag |
| `GetProRataRate` | `(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double) As Integer` | Get pro-rata rate |
| `GetProRataRate` (overload) | `(..., ByRef v_dtInceptionDate As Date) As Integer` | Get pro-rata with inception |
| `RecalculatePremium` | `(ByRef r_vRatingSection As Object, ByRef r_cTotalAnnualTax As Decimal) As Integer` | Recalculate premium |
| `RecalculatePremium` (overload) | `(ByRef r_vRatingSection As Object, ByVal v_vRateTypes As Object, ByRef r_cReturnPremium As Decimal, ... 18 more ByRef params..., ByRef r_bCancelledPolicy As Boolean) As Integer` | Full recalculate with all amounts |
| `CLngRounding` | `(ByVal cValue As Decimal, ByRef cReturn As Decimal) As Integer` | Rounding utility |
| `IsRiskACopy` | `(ByVal v_lRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Check if risk is a copy |
| `ApplyCoinsurance` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Apply coinsurance to all perils |
| `ApplyCoinsuranceToRisk` | `() As Integer` | Apply coinsurance to current risk |
| `CheckMTCRatingRules` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsMTCRatingRulesEnabled As Boolean) As Integer` | Check MTC rating rules |
| `CheckMandatoryRisk` | `(ByVal m_lRiskCnt As Long, ByRef r_bIsMandatoryRisk As Boolean) As Integer` | Check if risk is mandatory |
| `UpdateRisk` | `() As Integer` | Update risk values |
| `UpdateRiskStatus` | `(ByVal v_lRiskCnt As Integer, ByVal v_lRiskStatusId As Integer) As Integer` | Update risk status |
| `GetTMPStatus` | `() As Boolean` | Get TMP status |

**Stored Procedures (33):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_sir_peril_allocation` | `AddSectionAndPerils` | Add section and perils |
| `spu_sir_Calc_Premium` | `CalculatePremium` | Calculate premium |
| `spu_sir_rating_section_sel` | `PopulateRatingSections` | Select rating sections |
| `spu_sir_rating_section_del` | `DeleteSectionAndPerils` | Delete rating section |
| `spu_sir_peril_del` | `DeleteSectionAndPerils` | Delete perils |
| `spu_get_all_rating_section_types` | `GetRatingSectionTypes` | Get section types |
| `spu_get_gis_data_model_from_risk` | `GetDataModel` | Get GIS data model |
| `spu_pm_get_eff_id_from_code` | internal | Get effective ID from code |
| `spu_update_risk_values` | `UpdateRisk` | Update risk values |
| `spe_insurance_file_risk_li_sel` | internal | Select IFRL |
| `spu_select_Insurance_FileCnt` | internal | Select ins file cnt |
| `spe_Insurance_File_sel` | `GetInsuranceHeaderDetails` | Select insurance file |
| `spu_get_prorata_flag` | `GetProRataRate` | Get pro-rata flag |
| `spu_apply_coinsurance_to_peril` | `ApplyCoinsurance` | Apply coinsurance |
| `spu_apply_risk_coinsurance_to_peril` | `ApplyCoinsuranceToRisk` | Apply risk coinsurance |
| `spu_get_unused_rating_sections` | `PopulateRatingSections` | Get unused sections |
| `spu_Get_Original_Rating_Sections` | `PopulateRatingSectionsFromExistingSections` | Get original sections |
| `spu_Get_Rounding_Section_Amounts` | `GetRoundingSectionAmounts` | Get rounding amounts |
| `spu_sir_get_rating_section_type_tax` | `GetRatingSectionTypeTax` | Get section type tax |
| `spu_sir_is_risk_a_copy` | `IsRiskACopy` | Check risk copy |
| `spu_SIR_Get_RatingSectionTypes` | `GetRatingSectionType_ForRiskType` | Get section types for risk type |
| `spu_SIR_GetPerilAllocationSecurity` | `GetPerilAllocationSecurity` | Get security permissions |
| `spu_SIR_Check_Mandatory_Que` | `CheckMandatoryRisk` | Check mandatory risk |
| `spu_SIR_Get_Risks_Billed_Premium` | `GetRisksBilledPremium` | Get billed premium |
| `spu_SIR_Get_GIS_Output` | internal | Get GIS output |

---

### Policy
**Directory:** `Policy/`
**Project:** `bPMUPolicy`
**Purpose:** Policy-level underwriting operations � validates backdated MTAs, manages risk statuses, handles sub-agent validation, renewal frequency, grace periods.

**Business Methods — `bPMUPolicy`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `BackDatedMTAsAllowed` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRecordsAffected(,) As Object) As Integer` | Check backdated MTA permissions |
| `BackDatedCanAllowed` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef vvalue(,) As Object) As Integer` | Check backdated cancellation |
| `IsBackdatedMTARequired` | `(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lNewInsuranceFileCnt As Integer) As Boolean` | Check if backdated MTA required |
| `DeleteRisksRI` | `(ByVal nInsuranceFileCnt As Integer, ByRef r_nRecordsAffected As Integer) As Integer` | Delete RI for all risks |
| `GetAgentCancellationDetails` | `(ByVal AgentCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get agent cancellation details |
| `GetAssociatedAgent` | `(ByVal r_iUserID As Integer, ByRef m_vAgentArray(,) As Object) As Integer` | Get user's associated agent |
| `GetAssociatedAgentWithBranch` | `(ByVal r_iUserID As Integer, ByRef m_vAgentArray(,) As Object, Optional ByVal r_iSourceID As Integer = 0) As Integer` | Get agent with branch |
| `GetAssosiatedAgentBranch` | `(ByVal r_iSourceID As Integer, ByVal v_vLeadAgentCnt As Object, ByVal v_sTransactionType As String, ByRef m_vBranchArray(,) As Object) As Long` | Get agent branch |
| `GetDefaultBranchAgent` | `(ByVal r_iSourceID As Integer, ByRef m_vAgentArray(,) As Object) As Integer` | Get default branch agent |
| `GetClientCode` | `(ByVal v_iPartyID As Integer, ByRef r_vClientarray(,) As Object) As Integer` | Get client code |
| `GetGracePeriod` | `(ByVal v_lProductID As Object, ByRef r_lGracePeriodDays As Integer) As Integer` | Get grace period days |
| `GetRenewalFrequencyDetail` | `(ByVal v_lFrequencyID As Integer, ByRef r_vResult(,) As Object) As Integer` | Get renewal frequency |
| `GetLookUp` | `(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_vResultArray(,) As Object) As Integer` | Generic lookup |
| `SetRisksInceptionDate` | `(ByVal v_lInsuranceFileCnt As Long, ByVal v_dtCoverFromDate As Date) As Long` | Set inception date for all risks |
| `SetRisksQuoteStatus` | `(ByVal v_lInsuranceFileCnt As Object, ByVal v_iIsMTA As Byte, ByVal v_sRiskCode As Object, ByRef r_lRecordsAffected As Integer) As Integer` | Set quote status for risks |
| `SetRisksStatus` | `(ByVal v_lInsuranceFileCnt As Object, ByRef r_sStatusCode As String, ByRef r_lRecordsAffected As Integer) As Integer` | Set risk status for all risks |
| `SetRisksUnquoted` | `(ByVal v_lInsuranceFileCnt As Object, ByRef r_lRecordsAffected As Integer) As Integer` | Set all risks to unquoted |
| `ValidateLeadAgent` | `(ByVal v_vLeadAgentCnt As Integer) As Integer` | Validate lead agent |
| `CreateBusinessObject` | `(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer` | Create business object by class name |

**Stored Procedures (18):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_get_grace_period` | `GetGracePeriod` | Get grace period |
| `spu_update_risk_status_unquoted` | `SetRisksUnquoted` | Set unquoted status |
| `spu_update_risk_status_Quote_StatusNB` | `SetRisksQuoteStatus` | NB quote status |
| `spu_update_risk_status_Quote_StatusMTA` | `SetRisksQuoteStatus` | MTA quote status |
| `spu_update_risk_status` | `SetRisksStatus` | Update risk status |
| `spu_get_ClientCode` | `GetClientCode` | Get client code |
| `spe_BackdatedCan_Allowed` | `BackDatedCanAllowed` | Check backdated cancellation |
| `spe_BackdatedMTAs_Allowed` | `BackDatedMTAsAllowed` | Check backdated MTA |
| `spu_SIR_GetInsuranceFileStatus` | `IsBackdatedMTARequired` | Get insurance file status |
| `spu_Set_Risks_Inception_Date` | `SetRisksInceptionDate` | Set inception date |
| `spu_sir_del_risks_ri` | `DeleteRisksRI` | Delete risks RI |
| `spe_Agent_PLLSource` | `GetAssosiatedAgentBranch` | Get agent PLL source |
| `spu_Get_out_of_sequence_mta_details` | `IsBackdatedMTARequired` | Out-of-sequence MTA |
| `spu_Is_Retain_Renewal_Quote_On_MTA` | `IsBackdatedMTARequired` | Retain renewal on MTA |

**References:** `bSIRFindInsurance` (via `gPMComponentServices.CreateBusinessObject`)

---

### Product
**Directory:** `Product/`
**Project:** `bSIRProduct`
**Purpose:** **Product configuration management.** Full CRUD for insurance products � risk type groups, causation codes, claims workflow, transaction suppression, payment methods, and product-level options.

**Business Methods — `bSIRProduct`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `GetAllProducts` | `(ByRef r_vResultArray(,) As Object) As Integer` | Load all products |
| `GetProductDetails` | `(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Load product details |
| `GetProductDetailsForPolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vProductArray(,) As Object) As Integer` | Get product for policy |
| `GetProductValue` | `(ByVal v_lProductId As Integer, ByVal v_sColumnName As String, ByRef r_vProductArray(,) As Object) As Integer` | Get single product value |
| `GetProductid` | `(ByVal ifilecnt As Integer, ByRef vProduct_id As Integer) As Integer` | Get product ID from file |
| `UpdateProduct` | `(ByVal v_iTask As Object, ByRef r_lProductID As Object, ByRef r_vParamArray As Object, ByRef r_vAllowedRiskTypeGroup As Object, ByRef r_vAllowedCausation As Object, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Add/edit product |
| `DelProduct` | `(ByVal v_lProductId As Integer, ByVal v_iIsDeleted As Integer, Optional ByVal v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Delete product |
| `CheckIfProduced` | `(ByVal v_lProductId As Integer, ByRef r_vProducedArray(,) As Object) As Integer` | Check if product used |
| `GetAllRiskTypeGroup` | `(ByRef r_vResultArray(,) As Object) As Integer` | Get all risk type groups |
| `GetAllowedRiskTypeGroup` | `(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get allowed risk type groups |
| `GetAllowedCausation` | `(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get allowed causation codes |
| `GetAvailableCausation` | `(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get available causation codes |
| `GetClaimWorkflow` | `(ByRef r_vResults(,) As Object, ByVal v_lProductId As Integer) As Integer` | Get claim workflow |
| `GetClaimWorkflow` (overload) | `(..., ByVal v_lWorkflowID As Integer) As Integer` | Get specific workflow |
| `GetClaimWorkflowForClaim` | `(ByRef r_vResults(,) As Object, ByVal v_lClaimID As Integer, ByVal v_lWorkflowID As Integer) As Integer` | Get workflow for claim |
| `UpdateClaimWorkflow` | `(ByVal v_iTask As Object, ByVal v_lProductId As Object, ByVal vWorkflowArray As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Update claim workflow |
| `GetProductLevelOptionsForClaim` | `(ByVal v_lClaimID As Integer, Optional ByRef r_bIs_Multiple_claims_payments As Boolean = False, ... 21 more Optional ByRef params) As Integer` | Get all claim options for product |
| `GetSuspendedTransaction` | `(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Check suspended transactions |
| `CreateTransSuppressionNotificationTask` | `(ByVal v_sDescription As String) As Integer` | Create suppression task |
| `ValidateAccountCode` | `(ByVal sAccountCode As String, ByRef bFound As Boolean) As Integer` | Validate Orion account code |
| `UpdateProductPaymentMethod` | `(ByVal v_lProductId As Integer, ByVal v_sPaymentMethod As String) As Integer` | Set payment method |
| `GetAllowCurrencyChange` | `(ByVal v_lProductId As Integer, ByRef r_lAllowCurrencyChange As Integer, ByRef r_lAllowLossCurrencyChange As Integer) As Integer` | Get currency change permission |
| `IsAllowedStandardWordingEdit` | `(ByVal v_lProductId As Integer, ByRef r_lAllowedStandardWordingEdit As Integer) As Integer` | Check standard wording edit |
| `GetNoOfPoliciesOnProduct` | `(ByVal v_lProductId As Object, ByRef r_vResults(,) As Object) As Integer` | Count policies on product |
| `GetComboDetails` | `(ByVal v_lNumberingSchemeTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get combo/lookup data |
| `GetLookUp` | `(ByVal v_sTableName As String, ByRef r_vResultArray(,) As Object) As Integer` | Generic lookup |
| `AddInputParameter` | `(ByVal v_sName As Object, ByVal v_vValue As Object, ByVal v_iType As Object) As Integer` | Add input parameter |
| `PickListLoad` | `(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer` | Load pick list |
| `PickListSave` | `(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer` | Save pick list |
| `BeginTrans` / `CommitTrans` / `RollbackTrans` | `() As Integer` | Transaction management |

**Stored Procedures (33):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spe_Product_Add` | `UpdateProduct` | Add product |
| `spe_Product_upd` | `UpdateProduct` | Update product |
| `spe_Product_Del` | `DelProduct` | Delete product |
| `spe_Product_Sel` | `GetProductDetails` | Select product |
| `spu_Product_Saa` | `GetAllProducts` | Select all products |
| `spu_pm_caption_id_return` | `UpdateProduct` | Get caption ID |
| `spe_Risk_Type_Group_saa` | `GetAllRiskTypeGroup` | Select all risk type groups |
| `spu_Product_Risk_Type_Group_Sel` | `GetAllowedRiskTypeGroup` | Select product risk groups |
| `spu_Product_Risk_Type_Group_del` | `UpdateProduct` | Delete risk group link |
| `spu_Product_Risk_Type_Group_add` | `UpdateProduct` | Add risk group link |
| `spe_Product_suspend` | `GetSuspendedTransaction` | Check suspended |
| `spu_Product_Causation_add` | `UpdateProduct` | Add causation |
| `spu_Product_Causation_Del` | `UpdateProduct` | Delete causation |
| `spu_getDocumentsFlag` | `CheckIfProduced` | Get doc produce flag |
| `spu_Product_Get_No_Of_Policies` | `GetNoOfPoliciesOnProduct` | Count policies |
| `spu_SIR_ProductRisk_Required_Task_Params_Select` | `CreateTransSuppressionNotificationTask` | Get task params |
| `spu_Get_Product_Details_For_Claim` | `GetProductLevelOptionsForClaim` | Get claim options |
| `spu_SIR_Product_Claims_Workflow_Sel` | `GetClaimWorkflow` | Select workflow |
| `spu_SIR_Product_Claims_Workflow_Add` | `UpdateClaimWorkflow` | Add workflow |
| `spu_SIR_Product_Claims_Workflow_Upd` | `UpdateClaimWorkflow` | Update workflow |
| `spu_SIR_Product_Claims_Workflow_Sel_By_Claim` | `GetClaimWorkflowForClaim` | Select by claim |
| `spu_SAM_Product_sel` | `GetProductValue` | SAM product select |
| `spu_Get_Product_Values_From_Insurance_File_Cnt` | `GetProductDetailsForPolicy` | Get from ins file |
| `spu_SIR_Save_ProductSource` | `PickListSave` | Save product source |
| `spu_SIR_SaveCausation` | `PickListSave` | Save causation |

---

### Quote Engine
**Directory:** `Quote Engine/`
**Projects:** `bGISQEMCOMPILED`, `bGISQEMDRE`, `BGISQEMPMU`
**Purpose:** **GIS quote engine** � compiled rule execution engine that runs underwriting rating rules. `bGISQEMCOMPILED` executes pre-compiled rule sets, `bGISQEMDRE` is the dynamic rule engine, `BGISQEMPMU` is the PMU-specific engine wrapper.

**Business Methods — `bGISQEMPMU` / `bGISQEMCOMPILED` / `bGISQEMDRE`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `InitialiseEngine` | `(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer` | Initialise quote engine for data model |
| `NBQuote` (PMU) | `(ByVal v_lQuoteType As Integer, ByRef r_oDataset As Object, ByVal v_dtEffectiveDate As Date, Optional ByRef r_vAdditionalDataArray As Object, Optional ByVal v_vRatingInfo As Object, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal v_bAfterPRETriggerRules As Boolean = False) As Integer` | New business quote |
| `NBQuote` (DRE) | `(ByVal v_vQEMDREAdditionalArray As Object, ByVal v_lQuoteType As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_dtEffectiveDate As Date, Optional ByRef r_vAdditionalDataArray As Object, Optional ByVal v_bAfterPRETriggerRules As Boolean = False) As Integer` | DRE new business quote |
| `Quote` | `(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object, Optional ByRef sInsurerName As Object, Optional ByRef InsurerId As Object, Optional ByRef InstanceNo As Object, Optional ByRef r_vAdditionalDataArray As Object, Optional ByVal v_lTransactionType As Integer = 4, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer` | Execute quote rules |
| `Validate` | `(ByRef oDataSet As cGISDataSetControl.Application, ... same Optional params) As Integer` | Execute validation rules |
| `Default_Renamed` | `(ByRef oDataSet As cGISDataSetControl.Application, ... same Optional params, Optional ByRef lQuoteType As Object) As Integer` | Execute default rules |
| `Renewal` | `(ByRef oDataSet As cGISDataSetControl.Application, ... same Optional params) As Integer` | Execute renewal rules |
| `RenewalLapse` | `(ByRef oDataSet As cGISDataSetControl.Application, ... same Optional params) As Integer` | Execute renewal lapse rules |
| `PREProcessRules` | `(ByVal sAssemblyClassName As String, ByVal nQuoteType As Integer, ByRef oDataset As cGISDataSetControl.Application, ByVal dtEffectiveDate As Date, ByVal bPrePRE As Boolean, ByVal bPostPRE As Boolean, ByRef oAdditionalDataArray As Object, ByVal bIsBackdatedMTA As Boolean) As Integer` | PRE-process compiled rules (COMPILED only) |
| `PrintForm` | `(ByVal v_vSchemeArray As Object, ByVal v_lFormNumber As Integer, ByVal v_sXMLDataSetDef As String, ByVal v_sXMLDataSet As String) As Integer` | Print form |
| `NBPostQuoteProcess` | `(ByVal v_vSchemeArray As Object, ByVal v_lProcessType As Integer, ByVal v_sXMLDataSetDef As String, ByRef r_sXMLDataSet As String) As Integer` | Post-quote processing |
| `MergeDataWithDoc` | `(ByRef r_oXML As Object, ByRef oDocument As Object, ByRef vData As Object, ByRef sInFile As String) As Integer` | Merge data with document |
| `IsToQuoteForTest` | `() As Boolean` | Check if test quote |
| `GetRuleFileLocation` / `SetRuleFileLocation` | `() As String` / `(ByRef sFilePath As String) As Integer` | Rule file path management |

**Stored Procedures (5):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_get_rule_file_name` | `InitialiseEngine` | Get rule file name |
| `spu_get_ual_rule_file_name` | `InitialiseEngine` | Get UAL rule file name |
| `spu_get_gis_screen_code` | `InitialiseEngine` | Get GIS screen code |
| `spe_risk_type_rule_set_sel` | `InitialiseEngine` | Select risk type rule set |
| `spu_update_risk_rule_details` | `NBQuote` | Update risk rule details |

**References:** `bGISQEMCOMPILED` (created by DRE as fallback), `DREProxy.ExecutorService` (DRE engine), `VBQuoteEngine` (PMU script executor)

---

### Reinsurance
**Directory:** `Reinsurance/`
**Projects:** `bSIRReinsurance`, `bSIRReinsuranceRI2007`
**Purpose:** **Reinsurance arrangement management.** Manages RI arrangements, lines, bands, treaty references, auto-reinsurance, premium percent calculation. `bSIRReinsuranceRI2007` adds RI2007 standard support.

**Business Methods — `bSIRReinsurance` / `bSIRReinsuranceRI2007`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `ApplyReinsurance` | `(ByRef r_bApplyReinsurance As Boolean) As Integer` | Check and apply RI to risks |
| `AutoReinsure` | `(ByRef r_bAutoReinsure As Boolean) As Integer` | Auto-apply RI arrangements |
| `CalculateRI` | `(Optional ByVal bIsPT As Boolean = False) As Integer` | Calculate RI (bSIRReinsurance); no param in RI2007 |
| `CalculateFacTax` | `(ByVal v_lArrangementLineID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_cPremium As Decimal, ByVal v_cCommission As Decimal, ByRef r_cPremiumTax As Decimal, ByRef r_cCommissionTax As Decimal) As Integer` | Calculate facultative tax |
| `CalculateTreatyTax` | `(ByVal v_lArrangementLineID As Integer, ByVal v_lTreatyID As Integer, ByVal v_cPremium As Decimal, ByVal v_cCommission As Decimal, ByRef r_cPremiumTax As Decimal, ByRef r_cCommissionTax As Decimal) As Integer` | Calculate treaty tax |
| `EditUpdate` | `(ByVal lRIBandID As Integer, ByRef vRILines(,) As Object) As Integer` | Update RI arrangement lines |
| `Update` | `() As Integer` | Save changes |
| `Cancel` | `() As Integer` | Cancel changes |
| `GetDetails` | `() As Integer` | Load RI details |
| `GetBandValues` | `(ByVal lRIBandID As Integer, ByRef cSumInsured As Decimal, ByRef cPremium As Decimal, ByRef vRILines(,) As Object, ByRef lRIModelID As Integer, ByRef iFacPremiumMethod As Integer, ByRef cOriginalSumInsured As Decimal, ByRef cOriginalPremium As Decimal, ByRef vOriginalRILines(,) As Object) As Integer` | Get band values |
| `GetBandValues` (RI2007) | `(..., ByRef bIsextendedlimitApplied As Boolean, ByRef cExtendedLimitAmount As Decimal, Optional ByRef lXOLRIModelId As Long) As Integer` | Extended with XOL params |
| `GetRIBands` | `(ByRef vBands(,) As Object) As Integer` | Get RI bands |
| `GetRiskTotals` | `(ByRef r_cRiskRetainedSI As Decimal, ByRef r_cRiskTreatySI As Decimal, ByRef r_cRiskFacSI As Decimal) As Integer` | Get risk retained/treaty/fac totals |
| `GetRiskTotals` (RI2007) | `(..., ByRef r_cRiskXOLTreatySI As Decimal, ByRef r_cRiskXOLFacSI As Decimal) As Integer` | Extended with XOL totals |
| `GetRIVersion` | `(ByRef r_oRIVersion(,) As Object) As Integer` | Get RI versions (RI2007 only) |
| `GetGroupingIDs` | `(ByRef vOriginalGroupingIDArray(,) As Object) As Integer` | Get grouping IDs (RI2007 only) |
| `GetTreatyInfo` | `(ByVal lTreatyId As Integer, ByRef sCode As String, ByRef sAgreementCode As String, ByRef dCommissionPercent As Double, ByRef bIsRetained As Boolean) As Integer` | Load treaty details |
| `GetInsurerApprovedStatus` | `(ByRef r_sInsurerApprovedStatus As String) As Integer` | Get insurer status (RI2007 only) |
| `GetInsurerApprovedStatus` (overload) | `(..., ByVal v_lPartyCnt As Integer) As Integer` | Get for specific party |
| `DeleteRILines` | `(ByVal lDeletedRILineIds As Integer) As Integer` | Delete RI lines (RI2007 only) |
| `DeleteTaxCalculationEntries` | `(ByVal v_sTransTypePremium As String, ByVal v_sTransTypeCommission As String) As Integer` | Delete tax entries |
| `DeleteTaxCalculationEntries` (overload) | `(..., ByVal v_lArrangementLineID As Integer) As Integer` | Delete for specific line |
| `UpdatePremiumPercent` | `() As Integer` | Update premium percent (RI2007 only) |
| `UpdatePremiumPercentForRIArrangement` | `(ByVal v_lRIArrangement_id As Integer) As Integer` | Update for specific arrangement (RI2007) |
| `ValidateBands` | `(ByRef r_lValid As Integer, ByRef r_lBand As Integer) As Integer` | Validate RI bands |
| `CheckIfDisplayedRI` | `(ByVal IRiskType As Integer, ByRef r_blsDisplayed As Boolean) As Integer` | Check RI display for risk type |
| `ChecktheExistenceofRIArrangement` | `(ByVal v_lRiskCnt As Integer, ByRef r_bIsRiskRIArrangementExist As Boolean) As Integer` | Check RI exists (bSIRReinsurance only) |
| `ChecktheExistenceofRIArrangement` (overload) | `(..., ByRef r_crRISumInsured As Decimal, ByRef r_crRIPremium As Decimal) As Integer` | Check with amounts |
| `IsRatingSectionDeleted` | `(ByVal nRiskCnt As Integer, ByVal nRIBand As Integer, ByRef bIsDeletedRatingSection As Boolean) As Integer` | Check section deleted (RI2007) |
| `ChangeRiskStatus` | `() As Integer` | Change risk status |
| `SetStatus` | `(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer` | Set process status |

**Stored Procedures (38):**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_RI_Arrangement_refresh` | `CalculateRI` | Refresh RI (non-RI2007) |
| `spu_RI_Arrangement_refresh_RI2007` | `CalculateRI` | Refresh RI (RI2007) |
| `spu_RI_Arrangement_saa` | `GetDetails` | Select all arrangements |
| `spu_RI_Arrangement_sel_bands` | `GetRIBands` | Select RI bands |
| `spu_RI_Arrangement_upd` | `EditUpdate` | Update arrangement |
| `spu_RI_Arrangement_Line_add` | `EditUpdate` | Add arrangement line |
| `spu_RI_Arrangement_Line_saa` | `GetDetails` | Select all lines (non-RI2007) |
| `spu_RI_Arrangement_Line_saa_RI2007` | `GetDetails` | Select all lines (RI2007) |
| `spu_RI_Arrangement_Line_upd` | `EditUpdate` | Update line (non-RI2007) |
| `spu_RI_Arrangement_Line_upd_RI2007` | `EditUpdate` | Update line (RI2007) |
| `spu_RI_Arrangement_Line_del_RI2007` | `DeleteRILines` | Delete line (RI2007) |
| `spu_Treaty_sel` | `GetTreatyInfo` | Select treaty |
| `spu_UpdateRiskStatus` | `ChangeRiskStatus` | Update risk status |
| `spu_auto_reinsure_risk` | `AutoReinsure` | Auto-reinsure risk |
| `spu_SIR_Calculate_Treaty_Tax_Amounts` | `CalculateTreatyTax` | Calculate treaty tax |
| `spu_SIR_Calculate_Treaty_Party_Tax_Amounts` | `CalculateFacTax` | Calculate fac tax |
| `spu_SIR_Delete_Tax_Calculations` | `DeleteTaxCalculationEntries` | Delete tax calcs |
| `spu_Reinsurance_Screen_sel` | `AutoReinsure` | Select RI screen |
| `spu_Specific_User_Authority_Sel` | internal | Select user authority |
| `spu_RI_Arrangement_Get_SumInsuredAndPremium_ByRisk` | `ChecktheExistenceofRIArrangement` | Get SI and premium |
| `Spu_Sir_AddBrokerParticipants` | `EditUpdate` | Add broker participants (RI2007) |
| `spu_upd_Premium_Percent_RI2007` | `UpdatePremiumPercent` | Update premium percent |
| `Spu_RI_Arrangement_GetGroupingId` | `GetGroupingIDs` | Get grouping IDs |
| `spu_SIR_GetPartyInsurerDetails` | `GetInsurerApprovedStatus` | Get insurer details |
| `spu_RI_Arrangement_sel_versions` | `GetRIVersion` | Select versions |
| `spu_Is_Rating_Section_Deleted` | `IsRatingSectionDeleted` | Check section deleted |

---

### Reinsurance Transfer
**Directory:** `ReinsuranceTransfer/`
**Project:** `ReinsuranceTransfer`
**Purpose:** **Standalone Windows Forms application for RI transfers.** Provides UI-driven reinsurance portfolio transfer and clone RI transfer processing. Wraps `bSIRRIPortfolioTransfer` and `bSIRCloneRIBatchProcess` with a desktop interface for manual execution.

**Key Classes:**
| Class | Methods | Description |
|-------|---------|-------------|
| `RIPortfolioTransferInterface` | `ProcessInterface() As Integer` | Entry point for RI2007 portfolio transfer |
| `RI2007DisabledPortfolioTransferInterface` | `ProcessInterface() As Integer` | Entry point for non-RI2007 portfolio transfer |
| `RICloneTransferInterface` | `ProcessInterface() As Integer` | Entry point for clone RI transfer |
| `frmRIPortfolioTransfer` | `TransferPolicies(ByVal instance As InstanceElement) As Integer`, `TransferClaims(ByVal instance As InstanceElement) As Integer`, `SetCancel(ByVal sStatus As Boolean)`, `SetStart(sStatus As Boolean)` | RI2007 portfolio transfer form |
| `frmCloneRITransfer` | `StartTransfer(ByVal instance As InstanceElement)`, `StartProcessClonedRIPolicies(ByVal instance As InstanceElement)`, `StartProcessClonedRIClaims(ByVal instance As InstanceElement)`, `ProcessClonedRIPolicies(ByVal instance As InstanceElement, ByVal ThreadID As Integer) As Integer`, `ProcessClonedRIClaims(ByVal instance As InstanceElement, ThreadID As Integer) As Integer`, `SetCancel(status As Boolean)`, `SetStart(status As Boolean)` | Clone RI transfer form |
| `frmRI2007DisabledPortfolioTransfer` | `TransferPolicies() As Integer` | Non-RI2007 portfolio transfer |

**Stored Procedures:** None (delegates to `bSIRCloneRIBatchProcess` and `bSIRRIPortfolioTransfer`)

**References:** `bSIRCloneRIBatchProcess`, `bSIRRIPortfolioTransfer`, `SSP.PureInsuranceRestAPIHandler.ApiClient`

---

### Renewal
**Directory:** `Renewal/`
**Projects (13):** `bSIRRenewal`, `bSIRRenSelection`, `bSIRRenewalProcess`, `bSIRRenewalLaunch`, `bSIRRenInvitePrint`, `bSIRRenewalAcceptAgentEmail`, `bSIRAutomaticRenewalsAccept`, `bSIRAutomaticRenewalsInvite`, `bSIRAutomaticRenewalsSel`, `bSIRBatchRenewalController`, `bSIRLaunchAutoRenewalsAccept`, `bSIRLaunchAutoRenewalsInvite`, `bSIRLaunchAutoRenewalsSel`
**Purpose:** **The largest component group (442 methods, 201 SPs).** Complete renewal lifecycle � selection, invitation, acceptance, policy creation, stats posting, instalment creation, index linking, GIS re-quoting, document generation, broker transfer, and lapse processing. Supports batch renewals, true monthly policies, anniversary renewals, and void transactions.

#### Sub-Project: `bSIRRenewal` (Core Renewal + Void)

**Business Methods — `bSIRRenewal.Business`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `GetRenewals` | `(ByRef r_vResultArray(,) As Object, ByRef v_lRunMode As Integer, ByRef v_lRenewalInsFileCnt As Integer, Optional ByVal v_sRenewalDate As String = "", Optional ByVal v_lProductId As Integer = 0, Optional ByVal v_lSourceID As Integer = 0, Optional ByVal v_iCompare As Integer = 0, Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0) As Integer` | Get renewal list |
| `AcceptRenewal` | `(ByRef v_lOldInsuranceFileCnt As Object, ...) As Integer` | Accept renewal |
| `ProcessRenewalAcceptance` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lBatchRenewalJobID As Integer, ByVal v_lRecordsCount As Integer, ByVal v_sGUID As String) As Integer` | Process acceptance |
| `ValidateRenewalAcceptance` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bPolicyNumberToChange As Boolean, ByRef r_bNoRenewalInstalmentPlan As Boolean, ByRef r_bPrepaymentRequired As Boolean) As Integer` | Validate acceptance |
| `SetRenewalStatus` | `(ByRef v_lRenewalCnt As Object, ByRef v_iRenewalStatus As Object) As Integer` | Set renewal status |
| `UpdateRenewalStatus` | `(ByVal v_lRenewalStatusCnt As Object, ByRef r_sMessage As String) As Integer` | Update renewal status |
| `LapseRenewal` | `(ByRef v_lRenewalCnt As Integer, ByRef v_lLivePolicyCnt As Integer, ByRef v_lStatusId As Integer, ByRef v_lReasonID As Integer, ByRef v_sReasonDesc As String, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lInsFolderCnt As Integer = 0) As Integer` | Lapse renewal |
| `LapsePolicy` | `(ByRef v_lPolicyCnt As Integer, ByRef v_lLapseId As Integer, ByRef v_sLapseDesc As String, ByRef v_lLivePolicy As Integer) As Integer` | Lapse policy |
| `DeleteRenewal` | `(ByRef v_lRenewalCnt As Object, ByRef v_lLivePolicyCnt As Object, ByRef v_lStatusId As Object) As Integer` | Delete renewal |
| `DeletePolicyFromRenewal` | `(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_bRetainAnniversaryCopy As Boolean = False) As Integer` | Delete policy from renewal |
| `DeleteOldPolicy` | `(ByRef v_lInsuranceFileCnt As Integer, ByRef v_lRenewalStatusCnt As Integer) As Integer` | Delete old policy |
| `DeleteCreditControlItem` | `(ByRef v_lInsuranceFileCnt As Integer) As Integer` | Delete credit control item |
| `Rerate` | `(ByRef lRenewalInsFileCnt As Integer, ByRef sFailureReason As String) As Integer` | Re-rate renewal |
| `ReRatePolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_vRiskIDArray(,) As Object) As Integer` | Re-rate specific risks |
| `GetLapseReasons` | `(ByRef r_vLapseReasons(,) As Object) As Integer` | Get lapse reasons |
| `GetRisks` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vRiskIDArray(,) As Object) As Integer` | Get risks for policy |
| `GetDepositAmount` | `(ByVal v_lInsuranceFileCnt As Object, ByRef r_cDepositAmount As Object) As Integer` | Get deposit amount |
| `GetPaymentMethod` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sPaymentMethod As String) As Integer` | Get payment method |
| `GetDocTypeID` | `(ByVal v_sDocCode As String, ByRef r_lDocTypeId As Integer) As Integer` | Get document type ID |
| `TransferBroker` | `(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lTransferToPartyCnt As Integer) As Integer` | Transfer broker |
| `UpdatePolicyDetails` | `(ByRef v_lInsuranceFileCnt As Integer, Optional ByRef v_sNewPolicyRef As String = "", ...) As Integer` | Update policy details |
| `CreateEvent` | `(Optional ByVal v_vEventCnt As Object, ... ~15 Optional params) As Integer` | Create event log |
| `GenerateDocument` | `(ByVal v_iDocType As Integer, ByVal v_iMode As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sSpoolDesc As String, ByVal v_sTransactionType As Object, Optional ByVal v_bCalledFromSAM As Boolean = False, Optional ByVal v_sInsuranceFileRef As String = "") As Integer` | Generate renewal document |
| `GenerateCustomerRenewalEmail` | `(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sType As String) As Integer` | Generate customer email |
| `GenerateAgentRenewalEmail` | `(ByVal v_sType As String) As Integer` | Generate agent email |
| `SendEMail` | `(ByVal v_sTo As String, ByVal v_sSubject As String, ByVal v_sMessagePath As String, ByVal v_sAttachment As String) As Integer` | Send email |
| `GIS_NBQuote` | `(ByRef v_sGisDataModelCode As String, ByRef v_lQuoteType As Integer, ByRef r_sXMLDataSet As Object, ByRef r_sXMLDataSetDef As String) As Integer` | GIS new business quote |
| `GIS_SaveToDB` | `(ByVal v_sGisDataModelCode As String) As Integer` | Save GIS to database |
| `IsQuoted` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lResult As Integer) As Integer` | Check if quoted |
| `IsInstalment` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Check if instalment |
| `LockKey` / `UnLockKey` | `(ByVal v_sKeyName As String, ByVal v_nKeyValue As Integer, ByVal v_nUserID As Integer, ...) As Integer` | Key locking |

**Business Methods — `bSIRRenewal.VoidBusiness`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `ProcessVoidVersion` | `(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nInsuranceFolderCnt As Integer, ByRef r_sMessage As String, ByRef r_nNewInsuranceFileCnt As Integer) As Integer` | Process void/reversal |
| `CreatePolicy` | `(ByVal v_lOldinsurance_file_cnt As Integer, ByRef v_lNewinsurance_file_cnt As Integer) As Integer` | Create void policy |
| `CopyAllRisk` | `(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nNewInsuranceFileCnt As Integer, ByVal v_nInsuranceFolderCnt As Integer, ByVal v_dtPolicyStartDate As Date, ...) As Integer` | Copy all risks |
| `CopyRisk` | `(ByVal v_nNewInsuranceFileCnt As Integer, ...) As Integer` | Copy single risk |
| `CopyPerilAndRating` | `(ByVal nInsuranceFileCnt As Integer, ByVal nOriginalRiskCnt As Integer, ByVal nRiskCnt As Integer, ByVal dProRataRate As Double, Optional ByVal nNew_InsuranceFileCnt As Integer = 0, Optional ByVal nInsuranceFileTypeId As Integer = 0) As Integer` | Copy perils and ratings |
| `CopyRatingSectionForVoidTransaction` | `(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer` | Copy rating sections |
| `CopyRiskTaxFee` | `(ByVal v_lSourceRiskCnt As Long, ByVal v_lSourceInsuranceFileCnt As Long, ByVal v_lNewRiskCnt As Long, ByVal v_lNewInsuranceFileCnt As Long) As Integer` | Copy risk tax/fee |
| `CreateAndPostStats` | `(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_lOldInsuranceFileCnt As Long = 0, Optional ByRef r_sMessage As String = "") As Integer` | Create and post stats |
| `Allocation` | `(ByVal v_lCancelledInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Allocate transactions |
| `ReverseAllocation` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer` | Reverse allocation |
| `CheckPolicyForVoid` | `(ByVal v_linsurance_file_cnt As Integer, ByRef v_iContinue As Integer) As Integer` | Validate policy for void |
| `UpdatePolicyStatus` / `ResetPolicyStatus` | `(ByVal v_linsurance_file_cnt As Integer, ...) As Integer` | Policy status management |

**Stored Procedures (41) — `bSIRRenewal`:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_ACT_Del_Credit_Control_Item_InsFile` | `DeleteCreditControlItem` | Delete credit control |
| `spu_Update_Renewal_Count` | `UpdateRenewalCount` | Update renewal count |
| `spu_Is_Quoted` | `IsQuoted` | Check quoted |
| `spu_Get_Deposit_Amount` | `GetDepositAmount` | Get deposit |
| `spu_Is_Instalment` | `IsInstalment` | Check instalment |
| `spu_GetPaymentMethod` | `GetPaymentMethod` | Get payment method |
| `spu_SIR_GetAllUserBranches` | `GetAllUserBranches` | Get user branches |
| `spu_UpdateRenewalStatus` | `UpdateRenewalStatus` | Update status |
| `spu_TransferBroker` | `TransferBroker` | Transfer broker |
| `spu_SIR_Get_Is_Accept_TMP_Valid_Action` | `ValidateAcceptTMPIsValidAction` | Validate TMP |
| `spu_Get_BranchAgents` | `GetAgents` | Get branch agents |
| `spu_Get_GrossTotal` | `GetPolicyGrossTotal` | Get gross total |
| `spu_GetCurrencyAndAgentType` | `GetCurrencyAndAgentType` | Get currency |
| `spu_SIR_Get_Renewal_Acceptance_Details` | `GetRenewalAcceptanceDetails` | Get acceptance |
| `spu_SIR_Get_Renewal_Policy_Details` | `GetRenewalPolicyDetails` | Get policy details |
| `spu_insurance_file_update` | `UpdatePolicyDetails` | Update ins file |
| `spu_sir_copy_policy_for_void_transaction` | `CreatePolicy` (void) | Copy for void |
| `spu_Copy_Rating_Section_For_Void_Transaction` | `CopyRatingSectionForVoidTransaction` | Copy rating sections |
| `spu_Void_Risk_Tax_Fee_Copy` | `CopyRiskTaxFee` | Copy tax/fee |
| `spu_copy_void_agent_commission` | `CopyAllRisk` (void) | Copy agent commission |
| `spu_is_policy_valid_for_void_transaction` | `CheckPolicyForVoid` | Validate for void |
| `spu_void_transaction_log_add` | `AddReversalLog` | Add void log |
| `spu_update_void_policy_status` | `UpdatePolicyStatus` (void) | Update void status |

#### Sub-Project: `bSIRRenSelection` (Renewal Selection — 100+ methods, 70+ SPs)

**Business Methods — `bSIRRenSelection`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByVal sUsername As String, ..., Optional ByVal vDatabase As Object) As Long` | Standard; creates bGIS, bSIRPremiumFinance, bSIRProduct |
| `SetProcessModes` | `(Optional ByRef vTask, ..., Optional ByRef vEffectiveDate As Object) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `GetRenewalSelection` | `(ByVal v_vProductID As Object, ByVal v_vBranchID As Object, ByVal v_dtCompareDate As Date, ByRef r_vResultArray As Object) As Integer` | Get renewal selection list |
| `GetRenewalSelectionDetails` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSourceID As Integer, ByVal v_dtCompareDate As Date, ByRef r_vResultArray(,) As Object, ...) As Integer` | Get selection details |
| `ProcessRenewalSelection` | `(ByVal v_lInsuranceFileCnt As Integer, ...) As Integer` | Process renewal selection (7 overloads) |
| `ProcessRenewalSelectionBatch` | `(ByVal v_lBatchId As Integer, ByVal v_lInsuranceFolderCnt As Integer, ...) As Integer` | Process batch renewal |
| `CreateRenewalPolicyWrapper` | `(ByRef r_vRenewalList As Object, ByVal v_lCount As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByRef r_sFailureCriterion As String) As Integer` | Create renewal policy wrapper |
| `CreateTMPAnniversaryRenewal` | `(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer) As Integer` | Create TMP anniversary renewal |
| `GetTrueMonthlyPolicyDates` | `(ByVal v_bMidnightRenewal As Boolean, ByVal v_bTMPAnniversary As Boolean, ByVal v_lCount As Integer, ByRef r_vRenewalList(,) As Object, ByRef r_dtCoverStartDate As Date, ByRef r_dtExpiryDate As Date, ByRef r_dtRenewalDate As Date, ByRef r_dtAnniversaryDate As Date, ByRef r_lRenewalDayNumber As Integer, ByRef r_lAnniversaryCopy As Integer) As Integer` | Calculate TMP dates |
| `CopyAgentCommission` | `(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy agent commission |
| `CopyCoinsurance` | `(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy coinsurance |
| `CopyPolicyStandardWordings` | `(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_dtEffectiveDate As Date) As Integer` | Copy policy wordings |
| `CopyRiskStandardWordings` | `(ByVal v_lOldPolicyBinderId As Integer, ByVal v_lNewPolicyBinderId As Integer, ByVal v_sDataModelCode As String, Optional ByVal v_dtEffectiveDate As Date) As Integer` | Copy risk wordings |
| `CopyPolicyAssociates` | `(...) As Integer` | Copy policy associates |
| `CopyInsuranceFileAgent` | `(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer` | Copy insurance file agent |
| `CopyDataSet` | `(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkId As Integer, ...) As Integer` | Copy GIS dataset |
| `ApplyIndexLink` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer, ByVal v_dtEffectiveDate As Date) As Integer` | Apply index linking |
| `GisIndexLink` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer, ByVal v_vGisScreenID As Object, ByVal v_dtEffectiveDate As Date, ByVal v_sGisDataModelCode As String) As Integer` | GIS index link |
| `GIS_NBQuote` | `(ByRef v_sGisDataModelCode As String, ByRef v_lQuoteType As Integer, ...) As Integer` | GIS NB quote |
| `GIS_SaveToDB` | `(ByVal v_sGisDataModelCode As String) As Integer` | Save GIS to DB |
| `GIS_LoadFromDB` | `(ByVal v_sGisDataModelCode As String) As Integer` | Load GIS from DB (4 overloads) |
| `GetRisk` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risks |
| `CheckRenewalCriteria` | `(ByRef vRenewalList(,) As Object, ByRef lCount As Integer) As Integer` | Check criteria (2 overloads) |
| `CheckForClaim` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Check for claims |
| `IsRIComplete` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsComplete As Integer) As Integer` | Check RI complete |
| `IsQuoted` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsQuoted As Integer) As Integer` | Check quoted |
| `IsPremiumZero` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsPremiumZero As Boolean) As Integer` | Check zero premium |
| `IsAgentCancelled` | `(ByVal v_lPartyCnt As Integer, ByRef r_lIsCancelled As Integer) As Integer` | Check agent cancelled |
| `IsInstalment` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Check instalment |
| `IsInstalmentAndActivePartyBank` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Check instalment + bank |
| `IsTrueMonthlyPolicyProduct` | `(ByVal v_lProductId As Integer, ByRef v_bIsTMP As Boolean) As Integer` | Check TMP product |
| `CreateInstalmentQuote` | `(ByVal v_lOriginalInsuranceFileCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_sFailureMessage As String) As Integer` | Create instalment quote |
| `CreateRenewalFees` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Create renewal fees |
| `ApplyPolicyDiscount` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer) As Integer` | Apply policy discount |
| `AddRenewalStatus` | `(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer, ...) As Integer` | Add renewal status (4 overloads) |
| `UpdateRenewalStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sRenewalStatusTypeCode As String) As Integer` | Update renewal status |
| `UpdatePolicyRenewalStatus` | `(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Update policy renewal status |
| `AddRenewalReport` | `(ByVal v_sReportType As String, ByVal v_vClientName As Object, ...) As Integer` | Add renewal report |
| `PrintRenewalReport` | `(ByVal v_iReportSortOrder As Integer) As Integer` | Print renewal report |
| `DelRenewalReport` | `() As Integer` | Delete renewal report |
| `AddTaskToWorkManager` | `(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date, ...) As Integer` | Add work task (6 overloads) |
| `DeleteWorkTask` | `(ByVal v_sKeyName As String, ByVal v_sKeyValue As String) As Integer` | Delete work task |
| `GetBrokerTransferPortfolioDetail` | `(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get broker transfer detail |
| `FindAnniversaryCopy` | `(ByVal v_sInsuranceRef As String, ByVal v_dtCoverStartDAte As Date, ByRef r_vResults(,) As Object) As Integer` | Find anniversary copy |
| `LockProductForRenewal` | `(ByVal v_lProductId As Integer, ByRef r_sLockedBy As String) As Integer` | Lock product |

**Stored Procedures (70+) — `bSIRRenSelection`:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_Sel_Renewal_Policies` | `GetRenewalSelection` | Select renewal policies |
| `spu_SIR_Get_Renewal_Selection_Details` | `GetRenewalSelectionDetails` | Get selection details |
| `spu_Get_Renewal_PreList` | `GetRenewalSelection` | Get pre-list |
| `spe_Renewal_Status_add` | `AddRenewalStatus` | Add renewal status |
| `spu_Del_Renewal_Status` | `DelRenewalStatusPolicies` | Delete status |
| `spu_Add_RenewalReport` | `AddRenewalReport` | Add report |
| `spu_Del_RenewalReport` | `DelRenewalReport` | Delete report |
| `spu_index_linking_detail_sel` | `ApplyIndexLink` | Select index link |
| `spu_CLM_Check_For_Claim` | `CheckForClaim` | Check claims |
| `spu_Get_Policy_For_Renewal` | `GetPolicyForRenewal` | Get policy |
| `spu_Del_Policy_Dependant` | `DeleteRenewalPolicy` | Delete dependant |
| `spe_Insurance_File_del` | `DeletePolicy` | Delete ins file |
| `spu_Policy_RI_Value` | `IsRIComplete` | Check RI value |
| `spu_Risk_RI_Value` | `IsRIComplete` | Check risk RI |
| `spu_Is_Quoted` | `IsQuoted` | Check quoted |
| `spu_copy_agent_commission` | `CopyAgentCommission` | Copy commission |
| `spu_copy_insurance_file_agent` | `CopyInsuranceFileAgent` | Copy agent |
| `spu_copy_coinsurance` | `CopyCoinsurance` | Copy coinsurance |
| `spu_IsAgentCancelled` | `IsAgentCancelled` | Check cancelled |
| `spu_Is_Instalment` | `IsInstalment` | Check instalment |
| `spu_SIR_Get_Insured_Blacklist_Reason` | `GetClientBlacklistReason` | Get blacklist |
| `spu_SIR_Find_Anniversary_Copy` | `FindAnniversaryCopy` | Find anniversary |
| `spu_Copy_RISK_Standard_Wording` | `CopyRiskStandardWordings` | Copy wordings |
| `spu_SIR_Renewal_Check_Renewal_Product` | `CheckRenewalProductSQL` | Check product |
| `Spu_Sir_insurance_file_SelectRenewalProduct` | `SelectRenewalProduct` | Select product |
| `Spu_Sir_insurance_file_UpdateRenewalProduct` | `UpdateRenewalProduct` | Update product |
| `Spu_Sir_IsTMPProduct` | `IsTrueMonthlyPolicyProduct` | Check TMP |
| `spu_SIR_Add_Batch_Renewal_Job_Runs` | `ProcessRenewalSelectionBatch` | Add batch job |
| `spu_SIRRen_Get_Batch_Job_Printing_Options` | `ProcessRenewalSelectionBatch` | Get print options |
| `spu_get_max_policy_version_no` | `CreateRenewalPolicyWrapper` | Get max version |
| `spu_copy_policy_clients` | `CreateRenewalPolicyWrapper` | Copy clients |
| `spu_get_REN_policy_details` | `CreateRenewalPolicyWrapper` | Get policy details |
| `spu_SIR_Batch_Renewal_Job_sel` | `BatchRenewalJobSelect` | Select batch job |
| `spu_Is_Instalment_and_Active_PartyBank` | `IsInstalmentAndActivePartyBank` | Check bank |
| `spu_REN_update_renewal_status` | `UpdateRenewalStatus` | Update status |
| `spu_SIR_copy_insurance_file_associates` | `CopyPolicyAssociates` | Copy associates |
| `spu_SIRRen_Get_Renewal_Selection_Policy_List` | `GetInsuranceFileFromBatch` | Get policy list |
| `spu_SIR_Batch_Renewal_Job_Start` | `LoadRenewalInsurancefolderParamaters` | Start batch |
| `spu_SIR_Batch_Renewal_Job_Completed` | `CompleteRenewalStep` | Complete batch |
| `spu_SIR_Batch_Renewal_Job_Risk_Paramaters` | `LoadRenewalRiskParamaters` | Get risk params |

#### Sub-Project: `bSIRRenewalProcess` (Renewal Process — 50 methods, 27 SPs)

**Business Methods — `bSIRRenewalProcess`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object) As Integer` | Standard (ByRef params) |
| `SetProcessModes` | `(Optional ByRef vTask, ...) As Integer` | Standard |
| `Dispose` | `() Implements IDisposable.Dispose` | Cleanup |
| `AcceptRenewal` | `(ByVal v_lOldInsuranceFileCnt As Object, ByVal v_lNewInsuranceFileCnt As Object, ByVal v_lRenewalStatusCnt As Object, Optional ByRef v_sNewPolicyRef As Object, Optional ByRef v_dNewStartDate As Date, Optional ByRef v_dNewExpiryDate As Date, Optional ByRef r_sFailureMessage As String, Optional ByRef v_lAccountId As Integer = 0) As Integer` | Accept renewal |
| `SetRenewalStatusTypeID` | `(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalStatusTypeID As Integer, Optional ByVal v_lIsInvitePrinted As Integer = 0, Optional ByVal sCreditControlEnabled As String = "") As Integer` | Set status type |
| `RollBackPolicyToPreviousStatus` | `(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer, Optional ByVal v_lBrokerXferStatusTypeID As Integer = 0) As Integer` | Rollback to previous |
| `SetPolicyStatus` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFileStatusID As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String) As Integer` | Set policy status |
| `DeleteRenewal` | `(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRenewalStatusCnt As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sEventDesc As String = "", Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String) As Integer` | Delete renewal |
| `LapseRenewal` | `(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRenewalStatusCnt As Integer, ByVal v_lLapseReasonID As Integer, ByVal v_sLapseReasonDesc As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, Optional ByVal v_bStartTransaction As Boolean, Optional ByRef r_sFailureMessage As String) As Integer` | Lapse renewal |
| `LapsePolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLapseId As Integer, ByVal v_sLapseDesc As String, ByVal v_lLivePolicy As Integer) As Integer` | Lapse policy |
| `GetRenewalPolicy` | `(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByVal v_dRenewalDate As Date, ByVal v_lProductID As Integer, ByVal v_lBranchID As Integer, ByVal v_lRenewalType As Integer, ByVal v_lLeadAgentCnt As Integer, ByVal v_lAgentcode As Integer, ByRef r_vResult(,) As Object) As Integer` | Get renewal policy |
| `GetPolicyRenewalStatus` | `(ByVal v_lRenewalStatusCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get renewal status |
| `TransferBroker` | `(ByVal v_lRenewalInsuranceFileCnt As Object, ByVal v_lTransferToPartyCnt As Object) As Integer` | Transfer broker |
| `CreateInstalmentQuote` | `(ByVal v_lOriginalInsuranceFileCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_vPlanArray(,) As Object, ByRef r_sFailureMessage As String) As Integer` | Create instalment quote |
| `CreateEvent` | `(... ~15 Optional params) As Integer` | Create event log |
| `BeginTransaction` / `CommitTransaction` / `RollBackTransaction` | `() As Integer` | Transaction management |
| `LockKey` / `UnLockKey` | `(ByVal v_sKeyName, ByVal v_lKeyValue, ByVal v_lUserID, ...) As Integer` | Key locking |
| `GIS_NBQuote` / `GIS_SaveToDB` | `(...) As Integer` | GIS operations |

**Stored Procedures (27) — `bSIRRenewalProcess`:**
| SP | Called By | Purpose |
|----|----------|---------|
| `spu_SetRenewalStatusTypeID` | `SetRenewalStatusTypeID` | Set status type |
| `spu_Rollback_Policy` | `RollBackPolicyToPreviousStatus` | Rollback policy |
| `spu_GetRenewalPolicy` | `GetRenewalPolicy` | Get renewal policy |
| `spu_GetRenewalInviteList` | internal | Get invite list |
| `spe_Last_Print_Run_add` | `AddLastPrintRun` | Add print run |
| `spu_GetPolicyRenewalStatus` | `GetPolicyRenewalStatus` | Get status |
| `spu_UpdateRenewalStatus` | `UpdateRenewalStatus` | Update status |
| `spu_TransferBroker` | `TransferBroker` | Transfer broker |
| `spu_ACT_Del_Credit_Control_Item_InsFile` | `DeleteCreditControlItem` | Delete credit control |
| `spu_ACT_Add_Credit_Control_Item_InsFile` | `AcceptRenewal` | Add credit control |
| `spu_Get_Written_Status_Used` | `IsWrittenUsed` | Check written |
| `spu_update_agent_common_renewal_date` | `AcceptRenewal` | Update renewal date |
| `spu_SIR_Add_Chase_Cycle_Item_InsFile` | `AcceptRenewal` | Add chase cycle |
| `spu_DeleteRenewalPolicyAssociates` | `DeletePolicyFromRenewal` | Delete associates |
| `spu_SIR_GetAnnivPriorVersionInsFileCnt` | `GetAnnivPriorVersionInsFileCnt` | Get anniv version |
| `spe_PFPremiumFinance_sel_single` | `GetSingleFinancePlanFromInsFileCnt` | Get PF plan |

#### Sub-Project: `bSIRRenewalLaunch` (Renewal Launch — 6 methods, 1 SP)

**Business Methods — `bSIRRenewalLaunch`:**
| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `(ByRef sUsername As String, ..., Optional ByRef vDatabase As Object) As Integer` | Standard |
| `GetClientPolicyDetails` | `(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_sInsuranceRef As String = "") As Integer` | Get client policy details |
| `GetAgents` | `(ByRef r_vAgentArray(,) As Object) As Integer` | Get agents |

**Stored Procedures (1):** `spu_get_client_policy_details` → `GetClientPolicyDetails`

#### Remaining Renewal Sub-Projects (9 wrapper/driver components)

| Sub-Project | Methods | Purpose |
|-------------|---------|---------|
| `bSIRRenInvitePrint` | 5 | Renewal invitation print wrapper — delegates to `bSIRRenewalProcess` |
| `bSIRRenewalAcceptAgentEmail` | 5 | Agent email on renewal acceptance |
| `bSIRAutomaticRenewalsAccept` | 5 | Automatic renewal acceptance driver |
| `bSIRAutomaticRenewalsInvite` | 5 | Automatic renewal invitation driver |
| `bSIRAutomaticRenewalsSel` | 5 | Automatic renewal selection driver |
| `bSIRBatchRenewalController` | 15 | Batch renewal controller (orchestrates batch runs) |
| `bSIRLaunchAutoRenewalsAccept` | 5 | Launch automatic acceptance |
| `bSIRLaunchAutoRenewalsInvite` | 5 | Launch automatic invitation |
| `bSIRLaunchAutoRenewalsSel` | 5 | Launch automatic selection |

**References (all Renewal sub-projects):** `bACTCurrencyConvert`, `bSIRAccumulationValues`, `bSIRChangePolicyStatus`, `bSIRDocManagerWrapper`, `bSIRDocTemplate`, `bSIREvent`, `bSIRFindDocTemplate`, `bSIRInsuranceFile`, `bSIRListRisks`, `bSIROptions`, `bSIRPartyFee`, `bSIRPaymentHubWrapper`, `bSIRPolicyNumMaint`, `bSIRPremiumFinance`, `bSIRProduct`, `bSIRReportPrint`, `bSIRRiskData`, `bSIRRITax`, `bSirAgentCommission`, `bSirPerilAllocation`, `bGIS`

---

### Repost Transaction
**Directory:** `RepostTransaction/`
**Project:** `bSIRRepostTransaction`
**Purpose:** **Transaction reprocessing/correction utility.** Reverses and reposts failed or incorrect policy and claim transactions — reverses stats, allocations, documents, and recreates them correctly.

**Business Methods — bSIRRepostTransaction:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor. |
| `Initialise` | `Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer` | Standard IBusiness initialisation — authenticates and connects. |
| `Terminate` | `Public Function Terminate() As Integer` | Standard cleanup. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `SendToOrion` | `Public Function SendToOrion(ByVal v_lTransactionExportFolderCnt As Integer, Optional ByRef r_lDocumentId As Integer = 0) As Integer` | Send a transaction export folder to Orion accounting. |
| `GetFailedTransaction` | `Public Function GetFailedTransaction(ByRef r_vResult(,) As Object, Optional ByVal v_lExcludeOtherDoc As Integer = 1) As Integer` | Retrieve failed policy transactions for reprocessing. |
| `DeleteTransactionExport` | `Public Function DeleteTransactionExport(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sDocumentRef As String) As Integer` | Delete a transaction export record. |
| `DeleteStatsFolder` | `Public Function DeleteStatsFolder(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer` | Delete stats folder for a policy version. |
| `DeleteStatsDetail` | `Public Function DeleteStatsDetail(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer` | Delete stats detail records for a policy version. |
| `CreateStatsDetail` | `Public Function CreateStatsDetail(ByVal v_lStatsFolderCnt As Integer) As Integer` | Create stats detail records from a stats folder. |
| `GetStatsFolderCnt` | `Public Function GetStatsFolderCnt(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sDocumentRef As String, ByRef r_lStatsFolderCnt As Integer) As Integer` | Get stats folder count for a policy version and document. |
| `CreateExportFolder` | `Public Function CreateExportFolder(ByVal v_lStatsFolderCnt As Integer, ByRef r_lExportFolderCnt As Integer) As Integer` | Create transaction export folder from stats folder. |
| `CreateExportDetail` | `Public Function CreateExportDetail(ByVal v_lStatsFolderCnt As Integer, ByVal v_lTransactionExportFolderCnt As Integer) As Integer` | Create transaction export detail from stats folder. |
| `GetTransactionExportFolderCnt` | `Public Function GetTransactionExportFolderCnt(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lTransactionExportFolderCnt As Integer) As Integer` | Get transaction export folder count for a policy version. |
| `IsDocumentInAccount` | `Public Function IsDocumentInAccount(ByVal v_sDocumentRef As String, ByRef r_lStatus As Integer) As Integer` | Check if a document reference exists in accounting. |
| `GetAllPolicyVersion` | `Public Function GetAllPolicyVersion(ByVal v_sInsuranceRef As String, ByRef r_vResultArray(,) As Object) As Integer` | Get all policy versions for an insurance reference. |
| `ExecuteSql` | `Public Function ExecuteSql(ByVal v_sSQL As String, ByRef r_vResultArray(,) As Object) As Integer` | Execute ad-hoc SQL for data fix operations. |
| `IsPolicyVersionInAccount` | `Public Function IsPolicyVersionInAccount(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sDocumentRef As String) As Integer` | Check if a policy version has been posted to accounting. |
| `DeleteDocument` | `Public Function DeleteDocument(ByVal v_sDocumentRef As String) As Integer` | Delete a document by reference. |
| `DeleteDocumentAllocation` | `Public Function DeleteDocumentAllocation(ByVal v_sDocumentRef As String) As Integer` | Delete document allocation records. |
| `GetPolicyVersionDocument` | `Public Function GetPolicyVersionDocument(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vDocumentRef(,) As Object) As Integer` | Get document references for a policy version. |
| `DeletePolicyVersion` | `Public Function DeletePolicyVersion(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Delete an entire policy version. |
| `GetClosedClaimWithNoPosting` | `Public Function GetClosedClaimWithNoPosting(ByRef r_vResultArray(,) As Object) As Integer` | Find closed claims with no transaction postings. |
| `ReprocessClaim` | `Public Function ReprocessClaim(ByRef r_sMessage As String, Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_lClaimID As Integer = 0, Optional ByVal v_cThisRevision As Decimal = 0, Optional ByVal v_cThisPayment As Decimal = 0, Optional ByVal v_lOriginalReserveID As Integer = 0, Optional ByVal v_lPaymentID As Integer = 0, Optional ByVal v_sTransactionType As String = "") As Integer` | Reprocess a claim — reverses and reposts claim transactions. |
| `LockKey` | `Public Function LockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer, ByRef r_sLockedBy As String) As Integer` | Lock a record for exclusive editing. |
| `UnLockKey` | `Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer) As Integer` | Unlock a previously locked record. |
| `GetValueFromTable` | `Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_lDataType As Integer, ByRef r_vResult As Object) As Integer` | Generic table value lookup utility. |
| `GetFailedClaimTransaction` | `Public Function GetFailedClaimTransaction(ByRef r_vResultArray(,) As Object) As Integer` | Retrieve failed claim transactions for reprocessing. |
| `GetImbalanceClosedClaim` | `Public Function GetImbalanceClosedClaim(ByRef r_vResultArray(,) As Object, Optional ByVal v_sClaimNumber As String = "") As Integer` | Find closed claims with balance imbalances. |
| `GetReserveDetail` | `Public Function GetReserveDetail(ByVal v_sClaimNumber As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lNoneZeroReserve As Integer = 0) As Integer` | Get reserve details for a claim. |
| `GetPaymentDetail` | `Public Function GetPaymentDetail(ByVal v_sClaimNumber As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lUniquePaymentPartyCode As Integer = 0) As Integer` | Get payment details for a claim. |
| `GetClaimPeril` | `Public Function GetClaimPeril(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get perils associated with a claim. |
| `UpdateWorkPayment` | `Public Function UpdateWorkPayment(ByVal v_lWorkClaimID As Integer, ByVal v_lOriginalPaymentID As Integer, ByRef r_sMessage As String) As Integer` | Update work payment linkage. |
| `GetRiskDetail` | `Public Function GetRiskDetail(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray As Object) As Integer` | Get risk details for a policy version. |
| `GetTransactionExport` | `Public Function GetTransactionExport(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray As Object) As Integer` | Get transaction export data for a policy version. |
| `AddRIToPolicy` | `Public Function AddRIToPolicy(ByVal v_sRIModelCode As String, ByVal v_sInsuranceRef As String, ByRef r_sMessage As String) As Integer` | Add reinsurance model to a policy. |
| `SetStatusPolicyVersion` | `Public Function SetStatusPolicyVersion(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFileStatusID As Integer, ByRef r_sMessage As String) As Integer` | Set the status of a policy version. |
| `PopulatePolicyStatus` | `Public Function PopulatePolicyStatus(ByRef r_vResultArray(,) As Object) As Integer` | Get all available policy statuses. |
| `DeleteClaim` | `Public Function DeleteClaim(Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_lClaimID As Integer = 0) As PMEReturnCode` | Delete a claim by number or ID. |
| `GetClaimPosting` | `Public Function GetClaimPosting(ByVal v_sClaimNumber As String, ByRef r_vResultArray(,) As Object) As Integer` | Get posting details for a claim. |
| `GetClaimBalance` | `Public Function GetClaimBalance(ByVal v_lWorkClaimID As Integer, ByRef r_cBalance As Decimal, Optional ByRef r_sMessage As String = "") As Integer` | Get claim balance amount. |
| `ChangeDateAndPeriodID` | `Public Function ChangeDateAndPeriodID(ByVal v_sDocumentRef As String, ByVal v_dDocumentDate As Date, ByVal v_lPeriodID As Integer, Optional ByRef r_sMessage As String = "") As Integer` | Change document date and accounting period. |
| `CopyRIToClaim` | `Public Function CopyRIToClaim(Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_lClaimID As Integer = 0, Optional ByRef r_sMessage As String = "") As Integer` | Copy reinsurance data to a claim. |
| `GetNoRIClaim` | `Public Function GetNoRIClaim(ByRef r_vResultArray(,) As Object, Optional ByRef r_sMessage As String = "") As Integer` | Find claims missing reinsurance records. |
| `CreateReverseStats` | `Public Function CreateReverseStats(ByVal v_nInsuranceFileCnt As Integer, ByRef r_nStatsFolderCnt As Integer, Optional ByRef v_sDocumentRef As String = "") As Integer` | Create reversal stats folder and details. |
| `ReverseDocument` | `Public Function ReverseDocument(ByVal v_sDocumentRef As String) As Integer` | Reverse a document by creating reversal entries. |
| `RIRefresh` | `Public Function RIRefresh(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String) As Integer` | Refresh reinsurance data for a policy version. |
| `CreateTransExportReverse` | `Public Function CreateTransExportReverse(ByVal v_lStatsFolderCnt As Integer, ByVal v_lTransactionExportFolderCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer` | Create transaction export reversal records. |
| `ValidateDocumentRef` | `Public Function ValidateDocumentRef(ByVal sDocumentRef As String, ByRef vResultArray As Object) As Long` | Validate a document reference exists. |
| `ReverseAllocation` | `Public Function ReverseAllocation(ByVal lTransDetailId As Long) As Long` | Reverse an allocation by trans detail ID. |
| `ProcessClaimTransactions` | `Public Function ProcessClaimTransactions(ByVal v_lClaimId As Long, ByVal v_sDocumentRef As String, ByVal v_sRefNumber As String, Optional ByVal v_bRePost As Boolean = False, Optional ByVal v_sTransactionTypeCode As String = "") As Long` | Process or repost claim transactions end-to-end. |
| `ReverseClaimStatsDetail` | `Public Function ReverseClaimStatsDetail(sDocumentRef As String, ByVal lNewStatsFolderCnt As Long) As Long` | Reverse claim stats detail for a document. |
| `ReverseClaimTransactions` | `Public Function ReverseClaimTransactions(ByVal v_lClaimId As Long, ByVal v_lStatsFolderCnt As Long, ByVal m_sTransactionCode As String, ByVal sDocumentRef As String) As Long` | Reverse all claim transactions for a claim/document. |
| `FinaliseStats` | `Public Function FinaliseStats(v_lStatsFolderCnt As Long, v_lClaimId As Long, v_lTransactionTypeID As Long, v_sTransactionTypeCode As String, v_lCloned As Integer, v_lReverseClone As Integer, Optional r_bStatsSuppressed As Boolean = False, Optional ByVal sDocumentRef As String = "") As Long` | Finalise stats folder — mark as complete, handle cloned/reversal flags. |
| `RePostClonedClaimTransaction` | `Public Function RePostClonedClaimTransaction(ByVal v_lClaimId As Long, ByVal v_sDocumentRef As String, ByVal v_sRefNumber As String, Optional ByRef v_sTransactionType As String = "") As Long` | Repost a cloned claim transaction. |
| `IsDocumentInStats` | `Public Function IsDocumentInStats(ByVal v_sDocumentRef As String, ByRef r_lStatus As Integer) As Integer` | Check if a document has stats records. |
| `AddDataFixUtilityLog` | `Public Function AddDataFixUtilityLog(ByVal v_sPMNumber As String, ByVal v_sCreatedBy As String, ByVal v_sOldDocumentRef As String, ByVal v_sNewDocumentid As Integer, ByVal v_bIsReversal As Boolean, Optional ByVal v_lInsuranceFileCnt As Long = 0, Optional ByVal v_ClaimId As Long = 0) As Long` | Log a data fix utility operation for audit. |
| `AddTransdetailEx` | `Public Function AddTransdetailEx(V_nDocumentId As Long) As Long` | Add extended transaction detail. |
| `AddReversalDocument` | `Public Function AddReversalDocument(ByVal v_lOldDocumentRef As String, ByVal v_nStatsFolderCnt As Integer, Optional ByRef r_vDocumentID As Integer = Nothing) As Integer` | Create a reversal document from an existing document. |
| `AddReversalTransdetail` | `Public Function AddReversalTransdetail(ByVal v_lOldDocumentRef As String, ByVal r_vDocumentID As Integer) As Integer` | Create reversal transaction details. |
| `UpdateSQLAction` | `Public Function UpdateSQLAction(ByVal sSQL As String) As Integer` | Execute an update SQL statement. |
| `BeginTrans` | `Public Function BeginTrans() As Long` | Begin database transaction. |
| `CommitTrans` | `Public Function CommitTrans() As Long` | Commit database transaction. |
| `RollbackTrans` | `Public Function RollbackTrans() As Long` | Rollback database transaction. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_Failed_transaction` | `GetFailedTransaction` | Retrieve failed policy transactions. |
| `spu_DeleteStatsFolder` | `DeleteStatsFolder` | Delete stats folder records. |
| `spu_DeleteStatsDetail` | `DeleteStatsDetail` | Delete stats detail records. |
| `spu_DeleteTransactionExport` | `DeleteTransactionExport` | Delete transaction export records. |
| `spu_add_stats_details_Reverse` | `CreateReverseStats` | Create reversed stats details. |
| `spu_add_stats_details` | `CreateStatsDetail` | Create stats details from folder. |
| `spu_add_trans_export_folder` | `CreateExportFolder` | Create transaction export folder. |
| `spu_add_trans_details_control` | `CreateExportDetail` | Create transaction export details. |
| `spu_GetAllPolicyVersion` | `GetAllPolicyVersion` | Get all policy versions. |
| `spu_IsPolicyVersionInAccount` | `IsPolicyVersionInAccount` | Check if policy version is in accounting. |
| `spu_DeleteDocument` | `DeleteDocument` | Delete a document. |
| `spu_DeleteDocumentAllocation` | `DeleteDocumentAllocation` | Delete document allocations. |
| `spu_DeletePolicy` | `DeletePolicyVersion` | Delete a policy version. |
| `spu_GetClosedClaimWithNoPosting` | `GetClosedClaimWithNoPosting` | Find closed claims without postings. |
| `spu_GetFailedClaimTransaction` | `GetFailedClaimTransaction` | Retrieve failed claim transactions. |
| `spu_GetImbalaceClosedClaim` | `GetImbalanceClosedClaim` | Find imbalanced closed claims. |
| `spu_DeleteClaim` | `DeleteClaim` | Delete a claim. |
| `spu_GetClaimPosting` | `GetClaimPosting` | Get claim posting details. |
| `spu_add_stats_folder_reverse` | `CreateReverseStats` | Create reversal stats folder. |
| `spu_ChangeDocDatePeriod` | `ChangeDateAndPeriodID` | Change document date and period. |
| `spu_AddRIToPolicy` | `AddRIToPolicy` | Add RI model to policy. |
| `spu_Copy_transExport_for_Reversal_By_DocumentRef` | `CreateTransExportReverse` | Copy transaction export for reversal. |
| `spu_CLM_Get_Transaction_Type_Details` | `ProcessClaimTransactions` | Get claim transaction type details. |
| `spu_CLM_Finalise_stats_Reversal` | `FinaliseStats` | Finalise stats for claim reversal. |
| `spu_CopyReversalDocument` | `AddReversalDocument` | Create reversal document copy. |
| `spu_CopyReversalTransdetail` | `AddReversalTransdetail` | Create reversal transaction detail copy. |

**Component References:**

| Component | Class | Purpose |
|-----------|-------|---------|
| `bPMBTransactions` | `bPMBTransactions.Automated` | Transaction processing. |
| `bCLMReinsurance` | `bCLMReinsurance.Form` | Claim reinsurance operations. |
| `bCLMChangeClaimStatus` | `bCLMChangeClaimStatus.Business` | Change claim status. |
| `bCLMFindClaim` | `bCLMFindClaim.Business` | Find/search claims. |
| `bControlTransClaims` | `bControlTransClaims.Automated` | Claim transaction posting. |
| `bPMLock` | `bPMLock.User` | Record locking. |
| `bACTAutoNumber` | `bACTAutoNumber.Business` | Auto-number generation. |
| `bACTDocumentReversal` | `bACTDocumentReversal.Business` | Document reversal in accounting. |
| `bSIRReinsurance` | `bSIRReinsurance.Form` | Reinsurance operations. |
| `bSIRReinsuranceRI2007` | `bSIRReinsuranceRI2007.Form` | RI 2007 reinsurance operations. |
| `bSIRCloneRIBatchProcess` | `bSIRCloneRIBatchProcess.Business` | Clone RI batch processing. |

---

### Risk
**Directory:** `Risk/`
**Project:** `bSIRRiskData`
**Purpose:** Core risk data operations — copy risks, rating sections, perils, folders, sum insured values, risk links, and renewal links.

**Business Methods — bSIRRiskData:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor. |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRisk` | `Public Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get all risks for an insurance file. |
| `CopyRisk` (overload 1) | `Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer, ByVal v_lCreateLinkType As Integer) As Integer` | Copy risk with link type control. |
| `CopyRisk` (overload 2) | `Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer) As Integer` | Copy risk with status reset. |
| `CopyRisk` (overload 3) | `Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer, ByVal v_bAutoCancellation As Boolean, ByRef v_sRiskMergeStatus As String) As Integer` | Copy risk with auto-cancellation and merge status. |
| `CopyRisk` (overload 4) | `Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer) As Integer` | Copy risk — minimal parameters. |
| `CopyRisk` (overload 5) | `Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer, ByVal v_lCreateLinkType As Integer, ByVal v_bAutoCancellation As Boolean, ByRef v_sRiskMergeStatus As String, ByVal v_iRiskSelected As Integer) As Integer` | Copy risk — full parameters with selection flag. |
| `GetGISPolicyLink` | `Public Function GetGISPolicyLink(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get GIS policy link for a risk. |
| `CopyRSASumInsured` | `Public Function CopyRSASumInsured(ByVal v_lOldPolicyLinkID As Integer, ByVal v_lNewPolicyLinkID As Integer) As Integer` | Copy RSA sum insured values between policy links. |
| `AddRiskLink` (overload 1) | `Public Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String) As Integer` | Add risk link to insurance file. |
| `AddRiskLink` (overload 2) | `Public Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String, ByVal v_lOriginalRiskCnt As Integer) As Integer` | Add risk link with original risk reference. |
| `AddRiskLink` (overload 3) | `Public Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String, ByVal v_lOriginalRiskCnt As Integer, ByVal v_lRenewedRiskCnt As Integer) As Integer` | Add risk link with original and renewed risk references. |
| `CopyRatingSection` | `Public Function CopyRatingSection(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer` | Copy rating sections from old to new risk. |
| `CopyPerils` | `Public Function CopyPerils(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer` | Copy perils from old to new risk. |
| `UpdateRiskStatus` (overload 1) | `Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Object, ByVal v_lRiskStatusID As Object) As Integer` | Update risk status by ID. |
| `UpdateRiskStatus` (overload 2) | `Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Object, ByVal v_lRiskStatusID As Object, ByVal v_sRiskStatusCode As Object) As Integer` | Update risk status by ID and code. |
| `GetRiskStatus` | `Public Function GetRiskStatus(ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer` | Get current risk status. |
| `GetRiskAllStatuses` | `Public Function GetRiskAllStatuses(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get all risk statuses for an insurance file. |
| `DeleteInsuranceFileRiskLink` (overload 1) | `Public Function DeleteInsuranceFileRiskLink(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Delete all risk links for an insurance file. |
| `DeleteInsuranceFileRiskLink` (overload 2) | `Public Function DeleteInsuranceFileRiskLink(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer` | Delete specific risk link for an insurance file. |
| `AddRiskRenewalLink` | `Public Function AddRiskRenewalLink(ByVal v_lRiskCnt As Integer, ByVal v_lRenewalOriginalRiskCnt As Integer) As Integer` | Link a risk to its renewal original. |
| `CopyRiskExtras` | `Public Function CopyRiskExtras(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer` | Copy risk extras from old to new risk. |
| `GetUncopiedRisks` | `Public Function GetUncopiedRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risks not yet copied during renewal/MTA. |
| `CopyRiskFolder` | `Public Function CopyRiskFolder(ByVal v_lRisk_folder_cnt As Long, ByVal v_lInsuranceFileCnt As Long, ByRef r_lNew_risk_folder_cnt As Long) As Long` | Copy risk folder to new insurance file. |
| `GetRenewalRisk` | `Public Function GetRenewalRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risks eligible for renewal. |
| `GetRIModelTypeByRisk` | `Public Function GetRIModelTypeByRisk(ByVal v_lRiskCnt As Integer) As Integer` | Get RI model type assigned to a risk. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_Risk_saa` | `CopyRisk` | Select-after-add — retrieve newly copied risk. |
| `spe_Risk_add` | `CopyRisk` | Insert new risk record. |
| `spu_gis_policy_link_sel` | `GetGISPolicyLink` | Select GIS policy link. |
| `spe_insurance_file_risk_li_add` | `AddRiskLink` | Insert risk-to-insurance-file link. |
| `spu_Copy_Rating_Section` | `CopyRatingSection` | Copy rating section records. |
| `spu_Copy_Perils` | `CopyPerils` | Copy peril records. |
| `spu_copy_sums_insured` | `CopyRSASumInsured` | Copy sum insured values. |
| `spu_GetRiskStatus` | `GetRiskStatus` | Get risk status. |
| `spu_SIR_Risk_By_Insurance_File_Sel` | `GetRiskAllStatuses` | Select all risks by insurance file. |
| `spe_Insurance_File_Risk_Li_del` | `DeleteInsuranceFileRiskLink` | Delete risk link(s). |
| `spu_SIR_insurance_file_risk_link_del` | `DeleteInsuranceFileRiskLink` (overload 2) | Delete specific risk link. |
| `spu_SIR_Risk_Renewal_Original_Risk_Cnt_upd` | `AddRiskRenewalLink` | Update renewal original risk link. |
| `spu_copy_risk_extras` | `CopyRiskExtras` | Copy risk extras. |
| `spu_Get_Uncopied_Risks` | `GetUncopiedRisks` | Get uncopied risks. |
| `spu_Copy_Risk_Folder` | `CopyRiskFolder` | Copy risk folder. |
| `spe_Risk_RenSel_saa` | `GetRenewalRisk` | Select renewal risks. |
| `spu_Get_RI_Model_Type` | `GetRIModelTypeByRisk` | Get RI model type for risk. |

---

### Risk Type
**Directory:** `Risk Type/`
**Project:** `bSIRRiskType`
**Purpose:** Risk type configuration — CRUD for risk types, risk type groups, rule sets, GIS screen assignments, clause linking, and rating section type configuration.

**Business Methods — bSIRRiskType:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor. |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRIModelUsageDeferredRI` | `Public Function GetRIModelUsageDeferredRI(ByVal v_lRiskTypeID As Integer, ByRef r_vRIModelUsageDeferredRI(,) As Object) As Integer` | Get deferred RI model usage for a risk type. |
| `GetRiskTypeDetails` | `Public Function GetRiskTypeDetails(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get full details for a specific risk type. |
| `GetAllRiskTypeGroup` | `Public Function GetAllRiskTypeGroup(ByRef r_vResultArray(,) As Object) As Integer` | Get all risk type groups. |
| `GetRiskTypeGroup` | `Public Function GetRiskTypeGroup(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get risk type group for a specific risk type. |
| `GetAllowedGISScreen` | `Public Function GetAllowedGISScreen(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get allowed GIS screens for a risk type. |
| `GetAllGISScreen` | `Public Function GetAllGISScreen(ByRef r_vResultArray(,) As Object) As Integer` | Get all available GIS screens. |
| `GetClauses` | `Public Function GetClauses(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get clauses linked to a risk type. |
| `GetAllRiskType` | `Public Function GetAllRiskType(ByRef r_vResultArray(,) As Object) As Integer` | Get all risk types. |
| `UpdateRiskType` | `Public Function UpdateRiskType(ByVal v_iTask As Integer, ByRef r_lRiskTypeID As Integer, ByVal v_sCode As String, ByVal v_vDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_vShareWithCoInsurer As Object, ByVal v_vShareWithReInsurer As Object, ByVal v_vSuppressPublicText As Object, ByVal v_vSuppressPrivateText As Object, ByVal v_vSuppressTaxes As Object, ByVal v_vReportPointer As Object, ByVal v_vSectionMask As Object, ByVal v_vStampDutyRate1 As Object, ByVal v_vStampDutyRate2 As Object, ByVal v_vPrimarySort As Object, ByVal v_vSecondarySort As Object, ByVal v_vHeaderClause As Object, ByVal v_vTrailerClause As Object, ByVal v_vIsRiAtRiskLevel As Object, ByVal v_vIsAutoReinsured As Object, ByVal v_vHeaderClauseId As Object, ByVal v_vTrailerClauseId As Object, ByVal v_vAccumulationLevel As Object, ByVal v_vGISScreenId As Object, ByVal v_vClauses As Object, ByRef r_vLinkedRiskTypeGroup As Object, Optional ByVal v_vIsDeferredRIPermitted As PMEReturnCode = 0, Optional ByVal v_vClaimsIsPostTaxes As Object = Nothing, Optional ByVal v_vDisplayReinsurance As Object = Nothing, Optional ByVal v_vAllowRatingSectionAdd As Object = Nothing, Optional ByVal v_vAllowRatingSectionEdit As Object = Nothing, Optional ByVal v_vAllowRatingSectionDelete As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionRateType As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionRate As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionSumInsured As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionThisPremium As Object = Nothing, Optional ByVal v_vDisplayClaimReinsurance As Object = Nothing, Optional ByVal v_lClaimsTypeBasis As Long = 0, Optional ByVal v_lClaimsCoverBasis As Long = 0, Optional ByVal oAttachClaimOutsideOfPolicyPeriod As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Add/update risk type — 40+ parameter signature covering all risk type configuration fields. |
| `DelRiskType` | `Public Function DelRiskType(ByVal v_lRiskTypeID As Integer, ByVal v_iIsDeleted As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Soft-delete a risk type. |
| `UpdateRiskTypeRuleSet` | `Public Function UpdateRiskTypeRuleSet(ByVal v_iTask As Integer, ByRef r_lRiskTypeRuleSetID As Integer, ByVal v_sCode As String, ByVal v_vDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_vRiskTypeID As Object, ByVal v_vFileName As Object, ByVal v_vLive As Object, ByVal v_vType As Object, Optional ByVal v_lRiskTypeRuleSetTypeID As Integer = 0, Optional ByVal v_sDREExecutorURL As String = "", Optional ByVal v_sDREDefaultToken As String = "", Optional ByVal v_bDREDefault As Boolean = False, Optional ByVal v_bDREQuote As Boolean = False, Optional ByVal v_bDREValidate As Boolean = False, Optional ByVal v_bPostDREVB As Boolean = False, Optional ByVal v_bPrePre As Boolean = False, Optional ByVal v_lPREVersion As String = "", Optional ByVal v_lPRERulesetEffectiveDate As String = "", Optional ByVal v_bUseChildRuleSetEffDate As Boolean = False, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Add/update risk type rule set — DRE/PRE configuration, versioning. |
| `DelRiskTypeRuleSet` | `Public Function DelRiskTypeRuleSet(ByVal v_lRiskTypeID As Integer, ByVal v_lRiskTypeRuleSetID As Integer, ByVal v_iIsDeleted As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Soft-delete a risk type rule set. |
| `GetRiskTypeRuleSet` | `Public Function GetRiskTypeRuleSet(ByVal v_lRiskTypeRuleSetID As Integer, ByVal v_vRiskTypeID As Object, ByRef r_vResultArray(,) As Object) As Integer` | Get a specific risk type rule set. |
| `GetAllRiskTypeRuleSet` | `Public Function GetAllRiskTypeRuleSet(ByVal v_lRiskTypeID As Integer, ByVal v_sType As String, ByRef r_vResultArray(,) As Object) As Integer` | Get all rule sets for a risk type and type filter. |
| `PickListLoad` | `Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer` | Load picklist data for risk type configuration. |
| `PickListSave` | `Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer` | Save picklist selections. |
| `GetRuleTypes` | `Public Function GetRuleTypes(ByRef r_oResultArray(,) As Object) As Long` | Get all available rule types. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_Risk_Type_Add` | `UpdateRiskType` (add) | Insert new risk type. |
| `spe_Risk_Type_upd` | `UpdateRiskType` (update) | Update existing risk type. |
| `spe_Risk_Type_Del` | `DelRiskType` | Soft-delete risk type. |
| `spu_Risk_Type_Sel` | `GetRiskTypeDetails` | Select risk type details. |
| `spe_Risk_Type_Saa` | `UpdateRiskType` | Select-after-add for risk type. |
| `spu_pm_caption_id_return` | `UpdateRiskType` | Get/create caption ID for description. |
| `spu_GIS_Screen_saa2` | `GetAllGISScreen` | Select all GIS screens. |
| `spu_Risk_Type_GIS_Screen_Sel` | `GetAllowedGISScreen` | Select GIS screens for risk type. |
| `spu_Risk_Type_GIS_Screen_del` | `UpdateRiskType` | Delete risk type GIS screen links. |
| `spe_Risk_Type_GIS_Screen_add` | `UpdateRiskType` | Add risk type GIS screen link. |
| `spu_Risk_Type_Usage_sel` | `GetRiskTypeGroup` | Select risk type group usage. |
| `spe_Risk_Type_Group_saa` | `GetAllRiskTypeGroup` | Select all risk type groups. |
| `spu_Risk_Type_Usage_Del` | `UpdateRiskType` | Delete risk type usage links. |
| `spu_Risk_Type_Usage_Add` | `UpdateRiskType` | Add risk type usage link. |
| `spe_risk_type_rule_set_add` | `UpdateRiskTypeRuleSet` (add) | Insert new rule set. |
| `spe_risk_type_rule_set_upd` | `UpdateRiskTypeRuleSet` (update) | Update existing rule set. |
| `spe_risk_type_rule_set_sel` | `GetRiskTypeRuleSet` | Select rule set details. |
| `spe_risk_type_rule_set_saa` | `GetAllRiskTypeRuleSet` | Select all rule sets for risk type. |
| `spu_Risk_Type_Clauses_Sel` | `GetClauses` | Select clauses for risk type. |
| `spu_Risk_Type_Clauses_del` | `UpdateRiskType` | Delete clause links. |
| `spu_Risk_Type_Clauses_add` | `UpdateRiskType` | Add clause link. |
| `spu_risk_type_ri_model_usage_saa` | `GetRIModelUsageDeferredRI` | Select RI model usage for deferred RI. |
| `spe_Rule_Type_Saa` | `GetRuleTypes` | Select all rule types. |

---

### Statistics (Transaction Posting)
**Directory:** `Statistics/`
**Project:** `bControlTrans`
**Purpose:** **Policy transaction posting engine.** Creates stats folders, posts premium/fee/tax transactions, handles currency conversion, processes instalment deposits, reverses transactions (for MTA/cancellation), and integrates with Orion accounting via credit control items and export details.

**Business Methods — bControlTrans:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor. |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `SetKeys` | `Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer` | Set key values (insurance file cnt, etc.). |
| `GetKeys` | `Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer` | Get current key values. |
| `Start` | `Public Function Start(Optional ByRef iPaymentAccountId As Integer = 0, Optional ByRef iDebitAgainst As Integer = 0, Optional ByRef vCreditTransactions As Object = Nothing, Optional ByRef lCashListID As Integer = 0, Optional ByRef lCashListItemId As Integer = 0, Optional ByRef lTransactionID As Integer = 0, Optional ByRef cTransactionAmount As Decimal = 0, Optional ByRef sOldPolicyNumber As String = "", Optional ByRef sPaymentMethod As String = "", Optional ByRef vDebitTransactions As Object = Nothing, Optional ByRef bProcessSettleTransactions As Boolean = False, Optional ByRef cRoundOffAmount As Decimal = 0) As Integer` | **Main entry point** — processes a complete transaction (stats folder, details, exports, credit control). |
| `ProcessFolder` | `Public Function ProcessFolder(ByVal v_lStatsFolderCnt As Integer, Optional ByRef v_bAutoAllocate As Boolean = True) As Integer` | Process a stats folder — create stats details + transaction exports. |
| `BeginTrans` | `Public Function BeginTrans() As Integer` | Begin database transaction. |
| `CommitTrans` | `Public Function CommitTrans() As Integer` | Commit database transaction. |
| `RollbackTrans` | `Public Function RollbackTrans() As Integer` | Rollback database transaction. |
| `GetThisPremium` | `Public Function GetThisPremium(ByRef r_cThisPremium As Decimal, Optional ByRef o_nRIRowsToPostCnt As Integer = 0) As Integer` | Get the premium amount for the current transaction. |
| `GetPFTransactions` | `Public Function GetPFTransactions(ByRef v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get premium finance transactions for an insurance file. |
| `GetPreviousInsuranceFile` | `Public Function GetPreviousInsuranceFile(ByVal v_lNewInsuranceFileCnt As Object, ByRef r_lOldInsuranceFileCnt As Object) As Integer` | Get previous insurance file count (for MTA/renewal chain). |
| `GetPlanInsuranceFile` | `Public Function GetPlanInsuranceFile(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPlanInsuranceFileCnt As Integer) As Integer` | Get plan insurance file for instalment processing. |
| `ReverseStats` | `Public Function ReverseStats(ByRef v_lInsuranceFileCnt As Integer, Optional ByRef v_bProcess As Boolean = False, Optional ByRef v_lPartyCnt As Integer = 0) As Integer` | Reverse stats for a policy version (MTA/cancellation). |
| `UpdatePFCommissionTransactionID` | `Public Function UpdatePFCommissionTransactionID(ByRef v_lInsuranceFileCnt As Integer) As Integer` | Update premium finance commission transaction IDs. |
| `CheckPF` | `Public Function CheckPF(ByRef r_bIsPF As Boolean, ByRef r_lFeeAccount As Integer, ByRef r_lTaxAccount As Integer) As Integer` | Check if policy has premium finance. |
| `GetNextOrionDocRef` | `Public Function GetNextOrionDocRef() As Integer` | Get next Orion document reference number. |
| `GetNextOrionDocRefForInstalment` | `Public Function GetNextOrionDocRefForInstalment() As Integer` | Get next Orion document reference for instalment. |
| `CheckIfInstalmentDepositRequired` | `Public Function CheckIfInstalmentDepositRequired(ByRef r_bInstalmentDepositRequired As Boolean) As Integer` | Check if instalment deposit is required. |
| `GetSystemOptionLite` | `Public Function GetSystemOptionLite(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iSourceID As Integer) As Integer` | Get a system option value. |
| `GetBaseCurrencyAmount` | `Public Function GetBaseCurrencyAmount(ByVal lCompanyId As Integer, ByVal lCurrencyID As Integer, ByRef cBaseAmount As Decimal, ByVal cCurrencyamount As Decimal) As Integer` | Convert currency amount to base currency. |
| `GetGetCurrencyIDFromTransDetail` | `Public Function GetGetCurrencyIDFromTransDetail(ByVal lTransID As Integer, ByRef iCurrencyID As Integer) As Integer` | Get currency ID from a transaction detail record. |
| `GetInsuranceRef` | `Public Function GetInsuranceRef(ByVal v_lInsuranceFileCnt As Object, ByRef r_vResults(,) As Object) As Integer` | Get insurance reference for an insurance file. |
| `GetInsuranceFileInformation` | `Public Function GetInsuranceFileInformation(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_sInsuranceRef As String = "", Optional ByRef r_lCompanyID As Integer = 0, Optional ByRef r_lAccountId As Integer = 0, Optional ByRef r_iCurrencyID As Integer = 0, Optional ByRef r_cPremium As Decimal = 0, Optional ByRef r_dCurrencyBaseXrate As Double = 0, Optional ByRef r_dtCurrencyBaseDate As Date = #12/30/1899#, Optional ByRef r_dAccountBaseXrate As Double = 0, Optional ByRef r_dtAccountBaseDate As Date = #12/30/1899#, Optional ByRef r_dSystemBaseXrate As Double = 0, Optional ByRef r_dtSystemBaseDate As Date = #12/30/1899#, Optional ByRef r_lRateOverrideReasonID As Integer = 0, Optional ByRef r_lSubBranchId As Integer = 0, Optional ByRef r_lAccount_Key As Integer = 0) As Integer` | Get comprehensive insurance file information — ref, company, account, currency, premium, exchange rates. |
| `CreateSuspenseAccount` | `Public Function CreateSuspenseAccount(ByRef r_lAccountId As Integer, ByVal v_sLedgerFlag As String, ByVal v_lSubBranchID As Integer, ByVal v_sShortCode As String, ByVal v_lCurrencyID As Integer) As Integer` | Create suspense account for incomplete postings. |
| `UpdateCashDepositPolicyLink` | `Public Function UpdateCashDepositPolicyLink(ByVal v_lCashDepositAccountId As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer` | Link cash deposit account to policy. |
| `UnLockKey` | `Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer) As Integer` | Unlock a previously locked record. |
| `IsPortfolioTransferVersion` | `Public Function IsPortfolioTransferVersion(ByVal nInsuranceFileCnt As Integer, ByRef r_bIsPT As Boolean, ByRef r_dtTransferDate As Date) As Integer` | Check if a version is a portfolio transfer. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_upd_ins_file_system` | `Start` | Update insurance file system fields. |
| `spu_add_stats_folder` | `Start` | Create stats folder. |
| `spu_add_stats_details_control` | `ProcessFolder` | Create stats details from folder. |
| `spu_add_trans_export_folder` | `ProcessFolder` | Create transaction export folder. |
| `spu_add_trans_details_control` | `ProcessFolder` | Create transaction export details. |
| `spu_PFGetTransactionsFromInsuranceFile` | `GetPFTransactions` | Get PF transactions for insurance file. |
| `spu_GetPreviousInsuranceFileCnt` | `GetPreviousInsuranceFile` | Get previous insurance file count. |
| `spu_GetPlanInsuranceFile` | `GetPlanInsuranceFile` | Get plan insurance file. |
| `spu_ACT_Add_Credit_Control_Item_InsFile` | `ProcessFolder` | Add credit control item for insurance file. |
| `spu_ACT_Del_Credit_Control_Item_InsFile` | `ReverseStats` | Delete credit control item. |
| `spu_ACT_Credit_Control_Item_Update_MTC` | `ProcessFolder` | Update credit control item for MTC. |
| `spu_ACT_Get_TransDetailID_AccountID` | `ProcessFolder` | Get trans detail ID and account ID. |
| `spu_get_payment_setting` | `Start` | Get payment settings. |
| `spu_get_insurance_folder_cnt` | `Start` | Get insurance folder count. |
| `spu_get_original_pol_cnt` | `Start` | Get original policy count. |
| `spu_SIR_Get_NextOrionDocRef` | `GetNextOrionDocRef` | Get next Orion document reference. |
| `spu_SIR_CheckInstalmentDepositRequired` | `CheckIfInstalmentDepositRequired` | Check instalment deposit requirement. |
| `spu_ACT_Get_TransDetail_For_Stats_Reversal` | `ReverseStats` | Get trans details for stats reversal. |
| `spu_ACT_Do_Currency_Conversion` | `GetBaseCurrencyAmount` | Perform currency conversion. |
| `spu_ACT_Move_Suspended_Agent_Commission` | `ProcessFolder` | Move suspended agent commission. |
| `spu_Get_AccountIdFromShortCode` | `CreateSuspenseAccount` | Get account ID from short code. |
| `spu_ACT_Get_Insurance_File_Information` | `GetInsuranceFileInformation` | Get insurance file information. |
| `spu_ACT_Add_InsuranceFilePaymentDetails` | `Start` | Add insurance file payment details. |
| `spu_ACT_Get_Document_From_Transdetail` | `ProcessFolder` | Get document from trans detail. |
| `spu_Update_CashDeposit_Policy_Link` | `UpdateCashDepositPolicyLink` | Update cash deposit policy link. |
| `spu_ACT_Sel_TransDetail_By_Doc` | `ProcessFolder` | Select trans detail by document. |
| `spu_SIR_Add_Chase_Cycle_Item_InsFile` | `ProcessFolder` | Add chase cycle item for insurance file. |
| `spu_SIR_Del_Chase_Cycle_Item_InsFile` | `ReverseStats` | Delete chase cycle item. |
| `spu_Copy_Stats_for_Cloned_Reversal` | `ReverseStats` | Copy stats for cloned reversal. |
| `spu_Copy_Stats_for_PT_Reversal` | `ReverseStats` | Copy stats for portfolio transfer reversal. |
| `spu_ACT_Add_TransDetailEx` | `ProcessFolder` | Add extended transaction detail. |
| `spu_Copy_TransExportDetail_for_Cloned_Reversal` | `ReverseStats` | Copy export detail for cloned reversal. |
| `spu_ACT_GetThisPremium` | `GetThisPremium` | Get premium amount. |
| `spu_Get_Policy_Intermediary_Agent_Account` | `Start` | Get policy intermediary agent account. |
| `spu_Copy_Stats_Reversal` | `ReverseStats` | Copy stats for reversal. |

**Component References:**

| Component | Purpose |
|-----------|---------|
| `bACTAccount` | Account operations. |
| `bACTAllocate` | Allocation processing. |
| `bACTAllocationManual` | Manual allocation. |
| `bACTAutoNumber` | Auto-number generation. |
| `bACTCashListPost` | Cash list posting. |
| `bACTDocumentPost` | Document posting. |
| `bACTExplorer` | Account explorer. |
| `bACTImportSiriusTrans` | Sirius transaction import. |
| `bACTLedger` | Ledger operations. |
| `bACTTransdetail` | Transaction detail. |
| `bPMBTransactions` | PMB transaction processing. |
| `bSIRPremiumFinance` | Premium finance operations. |

---

### Tax Calculation
**Directory:** `Tax Calculation/`
**Project:** `bSIRRITax`
**Purpose:** **Central tax calculation engine.** Calculates, applies, and manages taxes at policy and risk level — previews tax, copies risk tax, deletes tax entries, recalculates on premium change.

**Business Methods — bSIRRITax:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `ApplyTaxes` | `Public Function ApplyTaxes(ByVal v_lInsFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_bApplyTaxes As Boolean, ByRef r_bTaxesSwitchedOff As Boolean) As Integer` | Determine whether taxes should be applied for a risk on an insurance file. |
| `CalculateTax` (overload 1) | `Public Function CalculateTax(ByVal vPremium As Decimal, ByVal vSumInsured As Decimal, ByVal vSumInsuredChange As Decimal, ByVal vRunningTotal As Decimal, ByVal vCalcBasis As Integer, ByVal vIsValue As Boolean, ByVal vPercentage As Double, ByVal vFixedRate As Decimal, ByVal vBasisValue As Decimal, ByVal vIsRounded As Boolean, ByVal vAllowTaxCredit As Boolean, ByRef rTaxValue As Decimal) As Integer` | Calculate a single tax value based on premium, sum insured, and rate parameters. |
| `CalculateTax` (overload 2) | `Public Function CalculateTax(ByVal vPremium As Decimal, ByVal vSumInsured As Decimal, ByVal vSumInsuredChange As Decimal, ByVal vRunningTotal As Decimal, ByVal vCalcBasis As Integer, ByVal vIsValue As Boolean, ByVal vPercentage As Double, ByVal vFixedRate As Decimal, ByVal vBasisValue As Decimal, ByVal vIsRounded As Boolean, ByVal vAllowTaxCredit As Boolean, ByRef rTaxValue As Decimal, ByVal vCurrencyId As Integer) As Integer` | Calculate tax with currency-specific rounding. |
| `CalculateTaxes` | `Public Function CalculateTaxes(ByRef vTaxArray(,) As Object) As Integer` | Calculate multiple taxes from a tax array. |
| `DeleteTaxes` | `Public Function DeleteTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer` | Delete all taxes for a specific risk. |
| `DeleteAllTaxes` | `Public Function DeleteAllTaxes(ByVal v_lInsuranceFileCnt As Integer) As Integer` | Delete all taxes for an insurance file. |
| `GetTaxesTotalDetails` | `Public Function GetTaxesTotalDetails(ByVal v_lInsuranceFileCnt As Object, ByRef vArray(,) As Object) As Integer` | Get tax total details for display. |
| `GetInsuranceFileTax` (overload 1) | `Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String) As Integer` | Get policy-level taxes. |
| `GetInsuranceFileTax` (overload 2) | `Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String, ByRef iTask As Integer) As Integer` | Get policy-level taxes with task context. |
| `GetInsuranceFileTax` (overload 3) | `Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String, ByRef v_sTransType As String) As Integer` | Get policy-level taxes with transaction type. |
| `GetInsuranceFileTax` (overload 4) | `Public Function GetInsuranceFileTax(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String, ByRef iTask As Integer, ByRef v_sTransType As String) As Integer` | Get policy-level taxes with task and transaction type. |
| `GetInsuranceFileTaxWithoutRecalculation` (overload 1) | `Public Function GetInsuranceFileTaxWithoutRecalculation(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String) As Integer` | Get policy-level taxes without triggering recalculation. |
| `GetInsuranceFileTaxWithoutRecalculation` (overload 2) | `Public Function GetInsuranceFileTaxWithoutRecalculation(ByRef r_vInsuranceFileTax As Object, ByRef r_sDescription As String, ByRef iTask As Integer) As Integer` | Get policy-level taxes without recalculation, with task context. |
| `UpdateInsuranceFileTax` | `Public Function UpdateInsuranceFileTax(ByVal v_vInsuranceFileTax(,) As Object) As Integer` | Save updated policy-level tax values. |
| `GetRiskTax` (overload 1) | `Public Function GetRiskTax(ByRef r_vRiskTax As Object, ByRef r_sDescription As String) As Integer` | Get risk-level taxes. |
| `GetRiskTax` (overload 2) | `Public Function GetRiskTax(ByRef r_vRiskTax As Object, ByRef r_sDescription As String, ByRef iTask As Integer) As Integer` | Get risk-level taxes with task context. |
| `UpdateRiskTax` | `Public Function UpdateRiskTax(ByVal v_vRiskTax(,) As Object) As Integer` | Save updated risk-level tax values. |
| `RecalculatePolicyTaxes` (overload 1) | `Public Function RecalculatePolicyTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate all taxes for a policy. |
| `RecalculatePolicyTaxes` (overload 2) | `Public Function RecalculatePolicyTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String, ByRef r_vInsuranceFileTax(,) As Object) As Integer` | Recalculate policy taxes and return results. |
| `RecalculatePolicyRiskTaxes` (overload 1) | `Public Function RecalculatePolicyRiskTaxes(ByVal v_lRiskCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate taxes for all risks on a policy. |
| `RecalculatePolicyRiskTaxes` (overload 2) | `Public Function RecalculatePolicyRiskTaxes(ByVal v_lRiskCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String, ByRef r_vRiskTax(,) As Object) As Integer` | Recalculate policy risk taxes and return results. |
| `RecalculateSingleRiskTax` (overload 1) | `Public Function RecalculateSingleRiskTax(ByVal v_lTaxCalculationCnt As Integer, ByVal v_lApplyTaxBy As Integer, ByVal v_sTransactionType As String) As Integer` | Recalculate a single risk tax entry. |
| `RecalculateSingleRiskTax` (overload 2) | `Public Function RecalculateSingleRiskTax(ByVal v_lTaxCalculationCnt As Integer, ByVal v_lApplyTaxBy As Integer, ByVal v_sTransactionType As String, ByRef r_vRiskTax(,) As Object) As Integer` | Recalculate single risk tax and return results. |
| `GetTaxNotAppliedToClient` | `Public Function GetTaxNotAppliedToClient(ByVal lInsuranceFileCnt As Integer, ByRef r_cTaxNotAppliedToClient As Decimal) As Integer` | Get total tax amount not applied to client. |
| `GetSystemOption` | `Public Function GetSystemOption(ByVal v_iOptionNumber As Integer, ByRef r_sResult As String) As Integer` | Get system option value. |
| `PreviewTax` | `Public Function PreviewTax(ByVal v_lTaxGroupId As Long, ByVal v_iCurrencyId As Integer, ByVal v_cTaxableAmount As Decimal, ByVal v_dtEffectiveDate As Date, ByRef r_vTax(,) As Object) As Long` | Preview tax calculation without saving. |
| `CopyRiskTax` | `Public Function CopyRiskTax(ByVal v_lSourceRiskCnt As Long, ByVal v_lSourceInsuranceFileCnt As Long) As Long` | Copy risk tax from source risk/insurance file (for MTA/renewal). |
| `UpdateRiskInTaxCalculation` | `Public Function UpdateRiskInTaxCalculation(ByVal oldRiskCnt As Integer, ByVal newRiskCnt As Integer, ByVal insuranceFileCnt As Integer) As Integer` | Update risk reference in tax calculation records. |

**Public Properties:**

| Property | Type | Access | Description |
|----------|------|--------|-------------|
| `Task` | `Integer` | WriteOnly | Set current task mode. |
| `PMProductFamily` | `Integer` | ReadOnly | Get product family. |
| `PMAuthorityLevel` | `Integer` | WriteOnly | Set authority level. |
| `RiskCnt` | `Integer` | Read/Write | Current risk count. |
| `InsuranceFileCnt` | `Integer` | Read/Write | Current insurance file count. |
| `TransactionType` | `String` | WriteOnly | Set transaction type. |
| `ApplyMTATaxRatesonRen` | `String` | Read/Write | Whether to apply MTA tax rates on renewal. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_SIR_Calculate_Tax_Preview` | `PreviewTax` | Preview tax calculation. |
| `spu_Insurance_File_Tax_DelAll` | `DeleteAllTaxes` | Delete all insurance file taxes. |
| `spu_Risk_Tax_DelAll` | `DeleteTaxes` | Delete all risk taxes. |
| `spu_Get_Insurance_Ref` | `GetInsuranceFileTax` | Get insurance reference. |
| `spu_Get_Risk_Description` | `GetRiskTax` | Get risk description. |
| `spu_Insurance_File_Tax_Select` | `GetInsuranceFileTax` | Select insurance file tax records. |
| `spu_Risk_Tax_Select` | `GetRiskTax` | Select risk tax records. |
| `spu_taxes_applied_to_product` | `GetInsuranceFileTax` | Get taxes applied to product. |
| `spu_taxes_applied_to_risk` | `GetRiskTax` | Get taxes applied to risk. |
| `spu_SIR_Get_TaxNotIncludedInInstalment` | `GetTaxesTotalDetails` | Get tax not included in instalment. |
| `spu_Insurance_File_Tax_Upd` | `UpdateInsuranceFileTax` | Update insurance file tax. |
| `spu_Risk_Tax_Upd` | `UpdateRiskTax` | Update risk tax. |
| `spu_Risk_Tax_Cal_Upd` | `UpdateRiskInTaxCalculation` | Update risk in tax calculation. |
| `spu_SIR_Tax_Calculation_Value_Update` | `CalculateTaxes` | Apply calculated tax values. |
| `spu_SIR_Get_Existing_Insurance_File_Tax` | `GetInsuranceFileTax` | Get existing insurance file tax data. |
| `spu_Risk_Single_Tax_Select` | `RecalculateSingleRiskTax` | Select single risk tax. |
| `spu_Get_TransType_By_RiskKey` | `RecalculatePolicyRiskTaxes` | Get transaction type by risk key. |
| `spu_Risk_Tax_Copy` | `CopyRiskTax` | Copy risk tax records. |
| `spu_ACT_Select_Currency` | `CalculateTax` | Get currency details. |
| `spu_Policy_Tax_DelAll` | `DeleteAllTaxes` | Delete all policy taxes. |
| `spu_SIR_Get_TaxNotAppliedToClient` | `GetTaxNotAppliedToClient` | Get tax not applied to client. |

---

### Underwriting Authority
**Directory:** `Underwriting Authority/`
**Projects:** `bSIRMaintainAURule`, `bSIRMaintainAuthority`
**Purpose:** Underwriting authority rules — manage rule sets, authority levels, link rules to users, and configure authority at product/risk type level.

**Business Methods — bSIRMaintainAURule:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRuleSet` | `Public Function GetRuleSet(ByRef r_vRuleSets(,) As Object, Optional ByVal v_lRuleSetId As Integer = 0) As Integer` | Get all or specific rule sets. |
| `GetRuleType` | `Public Function GetRuleType(ByVal v_nRuleType As Integer, ByRef r_oResultArray(,) As Object) As Integer` | Get rule type details. |
| `GetRuleSetLinks` | `Public Function GetRuleSetLinks(ByRef r_vRules(,) As Object) As Integer` | Get authority rule links for a rule set. |
| `UpdateRuleSet` (overload 1) | `Public Overloads Function UpdateRuleSet(ByVal v_iTask As Integer, ByRef r_lRuleSetId As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_sFileName As String, ByVal v_iLive As Integer) As Integer` | Add/update a rule set. |
| `UpdateRuleSet` (overload 2) | `Public Overloads Function UpdateRuleSet(ByVal v_iTask As Integer, ByRef r_lRuleSetId As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_sFileName As String, ByVal v_iLive As Integer, ByVal v_lRiskTypeRuleSetTypeID As Integer) As Integer` | Add/update rule set with rule set type. |
| `Update` | `Public Function Update(ByVal v_vRuleSetLinks As Object) As Integer` | Save rule set link changes. |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef v_iLanguageID As Integer, ByRef v_dtEffectiveDate As Date, ByRef v_sTableName As String, ByRef r_vLookupArray(,) As Object) As Integer` | Get lookup values for UI picklists. |
| `GetTransactionTypeList` | `Public Function GetTransactionTypeList(ByRef r_vResultArray(,) As Object) As Integer` | Get list of transaction types. |
| `GetRuleTypes` | `Public Function GetRuleTypes(ByRef r_oResultArray(,) As Object) As Integer` | Get all available rule types. |

**Business Methods — bSIRMaintainAuthority:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByRef r_vDataDictionary As String) As Integer` | Get GIS data dictionary details for authority configuration. |
| `GetNext` | `Public Function GetNext(ByRef r_vDataDictionary As Object, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object, ByRef r_vChildScreenDetails As Object) As Integer` | Navigate to next data dictionary entry with screen details. |
| `EditUpdate` | `Public Function EditUpdate(ByRef r_vDataDictionary As Object, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object) As Integer` | Save authority screen configuration changes. |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef v_iLanguageId As Integer, ByRef v_dtEffectiveDate As Date, ByRef v_sTableName As String, ByRef r_vLookupArray(,) As Object) As Integer` | Get lookup values for UI picklists. |
| `GetGISUserDefDetail` | `Public Function GetGISUserDefDetail(ByRef v_lGISUserDefHeaderId As Integer, ByRef r_vLookupArray(,) As Object) As Integer` | Get GIS user-defined detail values. |
| `GetAuthorityLevelTypes` | `Public Function GetAuthorityLevelTypes(ByRef r_vAuthorityLevels(,) As Object) As Integer` | Get all authority level types. |
| `GetPMUsers` | `Public Function GetPMUsers(ByRef r_vPMUsers(,) As Object) As Integer` | Get all PM users. |
| `GetProducts` | `Public Function GetProducts(ByRef r_vProducts(,) As Object) As Integer` | Get all products. |
| `GetAuthorityLevelsForUser` | `Public Function GetAuthorityLevelsForUser(ByVal v_lUserId As Integer, ByRef r_vUserAuthorityLevels(,) As Object) As Integer` | Get authority levels assigned to a specific user. |
| `Update` | `Public Function Update(ByVal v_lUserId As Integer, ByVal v_vUserDetails As Object, ByVal v_vActionArray() As Object, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Save user authority level assignments. |
| `Delete` | `Public Function Delete(ByVal v_lUserId As Integer, ByVal v_lProductId As Integer, ByVal v_lAuthorityLevelTypeId As Integer) As Integer` | Delete a user authority level assignment. |

**Stored Procedures — bSIRMaintainAURule:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_pm_caption_id_return` | `UpdateRuleSet` | Get/create caption ID. |
| `spu_GIS_user_def_detail_saa` | `GetLookupValues` | Get GIS user-defined details. |
| `spu_PMUser_Auth_Rule_Link_saa` | `GetRuleSetLinks` | Select authority rule links. |
| `spe_PMUser_Auth_Rule_Link_add` | `Update` | Add authority rule link. |
| `spe_PMUser_Auth_Rule_Link_del` | `Update` | Delete authority rule link. |
| `spu_Rule_Set_add` | `UpdateRuleSet` | Insert new rule set. |
| `spu_Rule_Set_upd` | `UpdateRuleSet` | Update existing rule set. |
| `spe_Authority_Rule_del` | `Update` | Delete authority rule. |
| `spu_Rule_Set_sel` | `GetRuleSet` | Select rule sets. |
| `spe_Authority_Rule_Detail_add` | `Update` | Add authority rule detail. |
| `spe_Authority_Rule_Detail_del` | `Update` | Delete authority rule detail. |
| `spu_Authority_Rule_Detail_sel` | `Update` | Select authority rule details. |
| `spu_Auth_Rule_Det_For_Rule_del` | `Update` | Delete rule details for a rule. |
| `spu_SIR_UAL_Transaction_Type_Sel` | `GetTransactionTypeList` | Select UAL transaction types. |
| `spu_Rule_type_select_filtered` | `GetRuleTypes` | Select filtered rule types. |

**Stored Procedures — bSIRMaintainAuthority:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_data_dictionary_sel` | `GetDetails` | Select data dictionary. |
| `spu_spec_data_dictionary_sel` | `GetNext` | Select specific data dictionary entry. |
| `spu_pm_caption_id_return` | `EditUpdate` | Get/create caption ID. |
| `spu_GIS_user_def_detail_saa` | `GetGISUserDefDetail` | Get GIS user-defined details. |
| `spu_pmuser_all_users_sel` | `GetPMUsers` | Select all PM users. |
| `spe_Product_saa` | `GetProducts` | Select all products. |
| `spu_Authority_Level_Type_saa` | `GetAuthorityLevelTypes` | Select authority level types. |
| `spe_PMUser_Authority_Level_sel` | `GetAuthorityLevelsForUser` | Select user authority levels. |
| `spe_PMUser_Authority_Level_add` | `Update` | Add user authority level. |
| `spe_PMUser_Authority_Level_upd` | `Update` | Update user authority level. |
| `spe_PMUser_Authority_Level_del` | `Delete` | Delete user authority level. |

---

### Documentation
**Directory:** `Documentation/`
**Project:** `bSIRGetDocument`
**Purpose:** Check document suppression for agents.

**Business Methods — bSIRGetDocument:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `CheckIfSuppressed` | `Public Function CheckIfSuppressed(ByRef lProcessType As Integer, ByRef lInsuranceFileCnt As Integer, ByRef bSuppressed As Boolean) As Integer` | Check if document production is suppressed for agent. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_insurance_file_sel` | `CheckIfSuppressed` | Select insurance file details. |
| `spu_get_agent_docs` | `CheckIfSuppressed` | Get agent document suppression settings. |

---

### Find Product Type
**Directory:** `Find Product Type/`
**Project:** `bSIRFindProductType`
**Purpose:** Search products by agent.

**Business Methods — bSIRFindProductType:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetProductByAgent` | `Public Function GetProductByAgent(ByVal v_lAgentPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get products available for a specific agent. |
| `GetDetails` | `Public Function GetDetails(ByVal v_lProductTypeId As Integer, ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer) As Integer` | Get product type details. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_agent_product_usage_sel` | `GetProductByAgent` | Select products by agent. |

---

### Find Risk Type
**Directory:** `Find Risk Type/`
**Project:** `bSIRFindRiskType`
**Purpose:** Risk type search (delegates to data layer via inline SQL).

**Business Methods — bSIRFindRiskType:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByVal v_lProductTypeId As Integer, ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer) As Integer` | Get risk types for a product type. |

*Uses inline SQL — no stored procedures.*

---

### Find Screen
**Directory:** `Find Screen/`
**Project:** `bSIRFindScreen`
**Purpose:** GIS screen search (delegates to data layer via inline SQL).

**Business Methods — bSIRFindScreen:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByVal v_lRiskTypeId As Integer, ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer) As Integer` | Get GIS screens for a risk type. |

*Uses inline SQL — no stored procedures.*

---

### Follow Up Tasks
**Directory:** `Follow Up Tasks/`
**Project:** `bPMUFollowUpTasks`
**Purpose:** Create/manage follow-up work tasks.

**Business Methods — bPMUFollowUpTasks:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetLookUp` | `Public Function GetLookUp(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_vResultArray(,) As Object) As Integer` | Get lookup values for task configuration. |
| `CreateTasks` | `Public Function CreateTasks(ByVal v_dtRunDate As Date, ByVal v_lUserGroup As Integer, ByRef r_bFound As Boolean) As Integer` | Create follow-up work manager tasks for a date and user group. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_follow_ups` | `CreateTasks` | Get follow-up items requiring tasks. |
| `spu_del_follow_ups` | `CreateTasks` | Delete processed follow-up items. |

**Component References:**

| Component | Purpose |
|-----------|---------|
| `bPMWrkTaskInstance` | Work manager task creation. |

---

### Index Linking
**Directory:** `Index Linking/`
**Project:** `bSIRIndexLinkingDetail`
**Purpose:** Manage index-linked premium adjustments.

**Business Methods — bSIRIndexLinkingDetail:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetAllIndexLinkingDetail` | `Public Function GetAllIndexLinkingDetail(ByVal v_lIndexLinkingID As Integer, ByRef r_vIndexLinkingDetail(,) As Object) As Integer` | Get all index linking details for an index linking header. |
| `Update` | `Public Function Update(ByVal v_lIndexLinkingID As Integer, ByVal v_vIndexLinkingDetail As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Save index linking detail changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_index_linking_detail_saa` | `GetAllIndexLinkingDetail` | Select all index linking details. |
| `spu_index_linking_detail_del` | `Update` | Delete index linking details. |
| `spe_index_linking_detail_add` | `Update` | Add index linking detail. |

---

### Lookup Detail
**Directory:** `Lookup Detail/`
**Projects:** `bSIRLookupDetail`, `bSIRLookupDetailRates`
**Purpose:** Manage lookup table detail rows (indicator and rate types).

**Business Methods — bSIRLookupDetail:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByRef lLookupHeaderId As Integer, ByRef vLookupDetails(,) As Object) As Integer` | Get lookup detail rows for a header. |
| `Update` | `Public Function Update(ByRef vLookupDetails(,) As Object) As Integer` | Save lookup detail changes (add/update/delete). |

**Stored Procedures — bSIRLookupDetail:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_lookup_detail_saa` | `GetDetails` | Select lookup details. |
| `spe_lookup_Detail_upd` | `Update` | Update lookup detail. |
| `spe_lookup_Detail_add` | `Update` | Add lookup detail. |

**Business Methods — bSIRLookupDetailRates:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | *(Standard IBusiness signature)* | Standard IBusiness initialisation. |
| `SetProcessModes` | *(Standard signature)* | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByRef vLookupRates(,) As Object) As Integer` | Get lookup detail rates/indicators (uses `LookupDetailId` and `RatesOrIndicators` properties). |
| `Update` | `Public Function Update(ByRef vLookupRates As Object) As Integer` | Save lookup detail rate changes. |

**Stored Procedures — bSIRLookupDetailRates:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_lookup_detail_rates_saa` | `GetDetails` (rates) | Select lookup detail rates. |
| `spu_lookup_detail_rates_del` | `Update` (rates) | Delete lookup detail rates. |
| `spe_lookup_detail_rates_add` | `Update` (rates) | Add lookup detail rate. |
| `spu_lookup_detail_indicator_saa` | `GetDetails` (indicators) | Select lookup detail indicators. |
| `spu_lookup_detail_indicator_del` | `Update` (indicators) | Delete lookup detail indicators. |
| `spe_lookup_detail_indicato_add` | `Update` (indicators) | Add lookup detail indicator. |

---

### Lookup Header
**Directory:** `Lookup Header/`
**Projects:** `bSIRLookupHeader`, `bSIRLookupHeaderRates`
**Purpose:** Manage lookup table headers (indicator and rate types).

**Business Methods — bSIRLookupHeader:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | *(Standard IBusiness signature)* | Standard IBusiness initialisation. |
| `SetProcessModes` | *(Standard signature)* | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByRef vLookupHeaders(,) As Object) As Integer` | Get all lookup headers. |
| `Update` | `Public Function Update(ByRef vLookupHeaders(,) As Object) As Integer` | Save lookup header changes (add/update). |

**Stored Procedures — bSIRLookupHeader:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_lookup_header_saa` | `GetDetails` | Select all lookup headers. |
| `spe_lookup_header_upd` | `Update` | Update lookup header. |
| `spe_lookup_header_add` | `Update` | Add lookup header. |
| `spu_lookup_header_rate_saa` | `GetDetails` | Select lookup header rates. |
| `spu_lookup_header_indicator_saa` | `GetDetails` | Select lookup header indicators. |

**Business Methods — bSIRLookupHeaderRates:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | *(Standard IBusiness signature)* | Standard IBusiness initialisation. |
| `SetProcessModes` | *(Standard signature)* | Set task/navigation/process mode context. |
| `GetDetails` | `Public Function GetDetails(ByRef vLookupRates(,) As Object) As Integer` | Get lookup header rates/indicators (uses `LookupHeaderId` and `RatesOrIndicators` properties). |
| `Update` | `Public Function Update(ByRef vLookupRates As Object) As Integer` | Save lookup header rate changes. |

**Stored Procedures — bSIRLookupHeaderRates:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_lookup_header_rates_saa` | `GetDetails` (rates) | Select lookup header rates. |
| `spu_lookup_header_rates_del` | `Update` (rates) | Delete lookup header rates. |
| `spe_lookup_header_rates_add` | `Update` (rates) | Add lookup header rate. |
| `spu_lookup_header_indicator_saa` | `GetDetails` (indicators) | Select lookup header indicators. |
| `spu_lookup_header_indicator_del` | `Update` (indicators) | Delete lookup header indicators. |
| `spe_lookup_header_indicato_add` | `Update` (indicators) | Add lookup header indicator. |

---

### MID Maintenance
**Directory:** `MID Maintenance/`
**Project:** `bSIRMIDMaintenance`
**Purpose:** Motor Insurance Database rule management — add/edit/delete MID rules, work manager task configuration.

**Business Methods — bSIRMIDMaintenance:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `GetMIDRules` | `Public Function GetMIDRules(ByVal nSourceID As Integer, ByRef r_aoResultArray(,) As Object) As Integer` | Get all MID rules for a source/branch. |
| `AddorEditMIDRule` | `Public Function AddorEditMIDRule(ByVal nSourceID As Integer, ByVal nMIDRuleID As Integer, ByVal sCode As String, ByVal sDescription As String, ByVal dtEffectiveDate As DateTime, ByVal dtStartDate As DateTime, ByVal dtExpiryDate As DateTime, ByVal sMIDType As String, ByVal nSupplierTypeId As Integer, ByVal nSupplierid As Integer, ByVal nInsurerId As Integer, ByVal nDelegatedAuthorityID As Integer, ByVal nSiteNumber As Integer, ByVal nPMUserGroupId As Integer, ByVal nPMwrkTaskGroupid As Integer, sFilename As String, ByVal nTestIndicator As Integer, ByVal sFileSeqNumStart As String, ByVal sCurrentFileSeqNum As String) As Integer` | Add or edit a MID rule with full configuration. |
| `DeleteMIDRule` | `Public Function DeleteMIDRule(ByVal nMIDRuleId As Integer) As Integer` | Soft-delete a MID rule. |
| `UnDeleteMIDRule` | `Public Function UnDeleteMIDRule(ByVal nMIDRuleId As Integer) As Integer` | Restore a soft-deleted MID rule. |
| `GetLookupValues` | `Public Function GetLookupValues(ByRef v_vLookupTables(,) As Object, ByRef r_vLookupDetails(,) As Object) As Integer` | Get lookup values for MID rule configuration UI. |
| `GetALLPMWrkTaskGroupTasks` | `Public Function GetALLPMWrkTaskGroupTasks(ByRef r_oaResults(,) As Object) As Integer` | Get all work manager task group tasks. |
| `GetALLPMWrkTaskGroupPMUserGroups` | `Public Function GetALLPMWrkTaskGroupPMUserGroups(ByRef r_aoResults(,) As Object) As Integer` | Get all task group PM user groups. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_MID_Rule_Details_Get` | `GetMIDRules` | Get MID rule details. |
| `spu_MID_Rule_Details_Add` | `AddorEditMIDRule` (add) | Add MID rule. |
| `spu_MID_Rule_Details_Update` | `AddorEditMIDRule` (edit) | Update MID rule. |
| `spu_MID_Rule_Details_Delete` | `DeleteMIDRule` | Delete MID rule. |
| `spu_MID_Rule_Details_UnDelete` | `UnDeleteMIDRule` | Undelete MID rule. |
| `spu_ACT_PMwrk_Task_Group_Tasks_Select` | `GetALLPMWrkTaskGroupTasks` | Select task group tasks. |
| `spu_ACT_PMwrk_Task_Group_PMUserGroup_Select` | `GetALLPMWrkTaskGroupPMUserGroups` | Select task group user groups. |

**Component References:**

| Component | Purpose |
|-----------|---------|
| `bPMLookup` | Lookup value loading. |

---

### Pay Now Options
**Directory:** `Pay Now Options/`
**Project:** `bSIRPayNowOptions`
**Purpose:** Payment options for pay-now scenarios — get account IDs, agent details, unallocated credits, write-off limits.

**Business Methods — bSIRPayNowOptions:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `GetInsuranceRef` | `Public Function GetInsuranceRef(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get insurance reference for a policy version. |
| `GetAgentType` | `Public Function GetAgentType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get agent type for an insurance file. |
| `GetAgentDetailsFromAgentID` | `Public Function GetAgentDetailsFromAgentID(ByVal v_lAgentCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get agent details by agent party count. |
| `GetUnallocatedCredits` | `Public Function GetUnallocatedCredits(ByVal v_lInsuranceFileCnt As Integer, ByVal v_bIsClient As Boolean, ByRef r_vResults(,) As Object, Optional ByRef Party_cnt As Integer = 0) As Integer` | Get unallocated credit balances for a policy. |
| `GetAccountID` | `Public Function GetAccountID(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get account ID for a party. |
| `GetAccountIDFromInsuranceFile` | `Public Function GetAccountIDFromInsuranceFile(ByVal v_lInsurance_file_cnt As Integer, ByRef r_vResults(,) As Object) As Integer` | Get account ID from insurance file. |
| `GetUserWriteOffLimit` | `Public Function GetUserWriteOffLimit(ByRef r_cWriteOffLimit As Decimal) As Integer` | Get current user's write-off authority limit. |
| `GetPaymentDetails` | `Public Function GetPaymentDetails(ByVal lInsuranceFileCnt As Integer, ByRef r_lMediaTypeId As Integer, ByRef r_lPartyBankId As Integer) As Integer` | Get payment details for live policy. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_Insurance_File_sel` | `GetInsuranceRef` | Select insurance file. |
| `spu_Get_UnAllocated_Credit` | `GetUnallocatedCredits` | Get unallocated credits. |
| `spu_Get_AgentInformation` | `GetAgentDetailsFromAgentID` | Get agent information. |
| `spu_GetAccountIDfromInsuranceFileCnt` | `GetAccountIDFromInsuranceFile` | Get account ID from insurance file. |
| `spu_GetPaymentDetailsOfLivePolicy` | `GetPaymentDetails` | Get payment details of live policy. |

---

### Payment Hub Wrapper
**Directory:** `PaymentHubWrapper/`
**Project:** `bSIRPaymentHubWrapper`
**Purpose:** Payment hub integration — process card payments, add/update cash list details.

**Business Methods — bSIRPaymentHubWrapper:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `GetPaymentHubSystemOptions` | `Public Function GetPaymentHubSystemOptions(ByRef v_oPaymentHubSystemOptions(,) As Object) As Integer` | Get payment hub configuration system options. |
| `ProcessPurchase` | `Public Function ProcessPurchase(ByVal strTransactionID As String, ByVal IntegrationToken As String, ByRef TokenID As String, ByRef oPaymentHubResponseParameters As PaymentHubResponseParameters, ByVal v_dTransactionValue As Decimal, ByVal v_sTransactionCurrencyCode As String, Optional ByVal v_nPartyCnt As Integer = 0) As String` | Process a card payment purchase via the payment hub. |
| `AddAndUpdateCashListDetails` | `Public Function AddAndUpdateCashListDetails(v_nOldInsuranceFileCnt As String, v_nNewInsuranceFileCnt As String, v_sTokenId As String, dPremiumAmount As Decimal) As Integer` | Add and update cash list details for payment. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Get_Payment_HUB_Configurations` | `GetPaymentHubSystemOptions` | Get payment hub configurations. |
| `spu_Get_party_corrospondance_address_cnt` | `ProcessPurchase` | Get party correspondence address. |
| `spe_Address_sel` | `ProcessPurchase` | Select address details. |
| `spu_email_contact_select` | `ProcessPurchase` | Select email contact. |
| `spu_Add_And_Update_CashList_Details` | `AddAndUpdateCashListDetails` | Add/update cash list details. |

**Component References:**

| Component | Purpose |
|-----------|---------|
| `bACTCashListPost` | Cash list posting and allocation. |

---

### Peril Type Usage
**Directory:** `Peril Type Usage/`
**Project:** `bSIRPerilTypeUsage`
**Purpose:** Manage peril type usage and earning patterns per product.

**Business Methods — bSIRPerilTypeUsage:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetPerilTypeUsage` | `Public Function GetPerilTypeUsage(ByRef r_vPerilTypeUsage(,) As Object) As Integer` | Get all peril type usage records. |
| `Update` | `Public Function Update(ByVal v_vPerilTypeUsage(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Save peril type usage changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_peril_type_usage_saa` | `GetPerilTypeUsage` | Select peril type usage records. |
| `spu_peril_type_usage_del` | `Update` | Delete peril type usage. |
| `spe_peril_type_usage_add` | `Update` | Add peril type usage. |
| `spu_set_earning_pattern_usage` | `Update` | Set earning pattern usage. |
| `spu_get_earning_pattern_usage` | `GetPerilTypeUsage` | Get earning pattern usage. |

---

### Primary Cause Risk Type
**Directory:** `Primary Cause Risk Type/`
**Project:** `bSIRPrimCauseRiskType`
**Purpose:** Map primary causes to risk type groups for claims.

**Business Methods — bSIRPrimCauseRiskType:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `getUnderwritingOrAgency` | `Public Function getUnderwritingOrAgency() As Integer` | Determine if underwriting or agency context. |
| `GetPrimCauseRiskTypeGrp` | `Public Function GetPrimCauseRiskTypeGrp(ByRef r_vPrimCauseRiskTypeGrp(,) As Object) As Integer` | Get primary cause to risk type group mappings. |
| `Update` | `Public Function Update(ByVal v_vPrimCauseRiskTypeGrp(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Save primary cause risk type group changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_CLM_Get_PrimCause_RiskTypeGrp` | `GetPrimCauseRiskTypeGrp` | Get primary cause risk type groups. |
| `spu_CLM_Delete_PrimCause_RiskTypeGrp` | `Update` | Delete mapping. |
| `spu_CLM_Add_PrimCause_RiskTypeGrp` | `Update` | Add mapping. |
| `spe_Risk_Type_Group_saa` | `getUnderwritingOrAgency` | Select risk type groups. |

---

### Produce Certificate
**Directory:** `Produce Certificate/`
**Project:** `bPMUProduceCertificates`
**Purpose:** Find policies for certificate production.

**Business Methods — bPMUProduceCertificates:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageId As Integer, ByVal iCurrencyId As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `FindPolicies` | `Public Function FindPolicies(ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Find policies for a party for certificate production. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_PLICO_gis_search_property_find` | `FindPolicies` | Search policies by GIS property. |

---

### RI Band Version
**Directory:** `RI Band Version/`
**Project:** `bSIRRIBandVersion`
**Purpose:** RI band version CRUD.

**Business Methods — bSIRRIBandVersion:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRIBandVersion` | `Public Function GetRIBandVersion(ByRef r_vRIBandVersion(,) As Object) As Integer` | Get all RI band versions. |
| `Update` | `Public Function Update(ByVal v_vRIBands(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Save RI band version changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_RI_Band_Version_saa` | `GetRIBandVersion` | Select RI band versions. |
| `spu_RI_Band_Version_del` | `Update` | Delete RI band version. |
| `spu_RI_Band_Version_add` | `Update` | Add RI band version. |

---

### RI Model
**Directory:** `RI Model/`
**Project:** `bSIRRIModel`
**Purpose:** RI model CRUD, model lines, currency rates, variable quota share.

**Business Methods — bSIRRIModel:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `AddRIModel` | `Public Function AddRIModel(ByRef r_lRIModelID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_iRIModelType As Integer, ByVal v_iFACPremiumType As Integer, ByVal v_iClaimAllocationType As Integer, ByVal v_lCurrencyID As Integer, ByVal v_lXOLClmRIModelID As Integer, ByVal v_cXOLClmLimit As Decimal, ByVal v_lXOLCatRIModelID As Integer, ByVal v_cXOLCatLimit As Decimal, ByVal v_iXOLCatReinstatements As Integer, ByVal v_vRIModelLines(,) As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "", Optional ByVal v_iTreatyPremiumType As Integer = 0, Optional ByVal v_vRIModelLinesVariableQuotaShare(,) As Object = Nothing) As Integer` | Add a new RI model with lines, limits, VQS, and XOL configuration. |
| `DeleteRIModel` | `Public Function DeleteRIModel(ByVal v_lRIModelID As Integer, Optional ByVal v_bIsDeleted As Boolean = True, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Soft-delete an RI model. |
| `GetRIModels` | `Public Function GetRIModels(ByRef r_vRIModel(,) As Object, Optional ByVal v_lRIModelID As Integer = 0) As Integer` | Get all or specific RI models. |
| `GetRIModelLines` | `Public Function GetRIModelLines(ByVal v_lRIModelID As Integer, ByRef r_vRIModelLines(,) As Object, Optional ByVal v_iFilterType As Integer = 0, Optional ByVal v_sTreatyTypeCode As String = "", Optional ByVal v_lRIArrangementID As Long = 0) As Integer` | Get RI model lines with optional treaty/arrangement filter. |
| `UpdateRIModel` | `Public Function UpdateRIModel(ByVal v_lRIModelID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_iRIModelType As Integer, ByVal v_iFACPremiumType As Integer, ByVal v_iClaimAllocationType As Integer, ByVal v_lCurrencyID As Integer, ByVal v_lXOLClmRIModelID As Integer, ByVal v_cXOLClmLimit As Decimal, ByVal v_lXOLCatRIModelID As Integer, ByVal v_cXOLCatLimit As Decimal, ByVal v_iXOLCatReinstatements As Integer, ByVal v_vRIModelLines(,) As Object, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "", Optional ByVal v_iTreatyPremiumType As Integer = 0, Optional ByVal v_vRIModelLinesVariableQuotaShare(,) As Object = Nothing) As Integer` | Update an RI model. |
| `CheckRetainedReinsurer` | `Public Function CheckRetainedReinsurer(ByVal lTreatyId As Integer, ByRef bIsRetainedReinsurer As Boolean) As Integer` | Check if treaty has a retained reinsurer. |
| `GetRIModelAuditTrail` | `Public Function GetRIModelAuditTrail(ByVal v_lRIModelID As Integer, ByRef v_vRIModelAuditTrailArray(,) As Object) As Integer` | Get RI model change audit trail. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_RI_Model_add` | `AddRIModel` | Insert RI model. |
| `spu_RI_Model_del` | `DeleteRIModel` | Delete RI model. |
| `spu_RI_Model_saa` | `GetRIModels` | Select RI models. |
| `spu_RI_Model_upd` | `UpdateRIModel` | Update RI model. |
| `spu_RI_Model_Line_add` | `AddRIModel`, `UpdateRIModel` | Add RI model line. |
| `spu_RI_Model_Line_del` | `UpdateRIModel` | Delete RI model line. |
| `spu_RI_Model_Line_saa` | `GetRIModelLines` | Select RI model lines. |
| `spu_Check_treaty_retained_party` | `CheckRetainedReinsurer` | Check treaty retained party. |
| `Spu_GetRIModelAuditTrail` | `GetRIModelAuditTrail` | Get RI model audit trail. |
| `Spu_GetRITypeForTreaty` | `AddRIModel`, `UpdateRIModel` | Get RI type for treaty. |
| `Spu_Get_Extended_Limit_Details` | `GetRIModelLines` | Get extended limit details. |
| `spu_RI_ModelCurrency_saa` | `AddRIModel`, `UpdateRIModel` | Select model currency rates. |
| `spu_RI_ModelCurrencyRate_upd` | `UpdateRIModel` | Update model currency rate. |
| `spu_GetVariableQuotaShareConfig` | `GetRIModels` | Get VQS configuration. |
| `spu_GetRIModelVariableQuotaShareConfig` | `GetRIModelLines` | Get RI model VQS configuration. |
| `spu_SaveVariableQuotaShareConfig` | `AddRIModel`, `UpdateRIModel` | Save VQS configuration. |
| `spu_DeleteVariableQuotaShareConfig` | `UpdateRIModel` | Delete VQS configuration. |

---

### RI Model Usage
**Directory:** `RI Model Usage/`
**Project:** `bSIRRIModelUsage`
**Purpose:** Map RI models to risk types. Includes deferred RI check.

**Business Methods — bSIRRIModelUsage:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectivedate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRIModelUsage` | `Public Function GetRIModelUsage(ByRef r_vRIModelUsage(,) As Object, Optional ByVal v_lIsDeferred As Integer = PMFalse) As Integer` | Get RI model usage records, optionally filtered by deferred flag. |
| `GetRIModelIsDeferred` | `Public Function GetRIModelIsDeferred(ByRef r_vRIModelIsDeferred(,) As Object, Optional ByVal v_lIsDeferred As Integer = PMFalse) As Integer` | Get RI models with deferred RI flag. |
| `Update` | `Public Function Update(ByVal v_vRIModelUsage(,) As Object, Optional ByVal v_vIsDeferred As Object = PMFalse, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Save RI model usage changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Risk_Type_ri_model_usage_saa` | `GetRIModelUsage` | Select RI model usage. |
| `spu_Risk_Type_ri_model_usage_del` | `Update` | Delete RI model usage. |
| `spu_Risk_Type_ri_model_usage_add` | `Update` | Add RI model usage. |
| `spu_Risk_Type_ri_model_usage_upd` | `Update` | Update RI model usage. |
| `spu_Risk_Type_RI_Model_is_deferred_saa` | `GetRIModelIsDeferred` | Select deferred RI model flag. |
| `spu_SIR_ValidateRIModelUsage` | `Update` | Validate RI model usage. |

---

### RI Portfolio Transfer
**Directory:** `RI PortfolioTransfer/`
**Project:** `bSIRRIPortfolioTransfer`
**Purpose:** **RI portfolio transfer** — processes policy and claim transfers between RI arrangements, copies risks, recalculates RI, posts stats. Contains two business classes: `Business` (RI2007 enabled) and `RI2007DisabledBusiness`.

**Business Methods — bSIRRIPortfolioTransfer (Business class):**

| Method | Signature | Description |
|--------|-----------|-------------|
| `New` | `Public Sub New()` | Constructor. |
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `Dispose` | `Public Sub Dispose() Implements IDisposable.Dispose` | Cleanup and release resources. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetPoliciesPortfolioTransfer` | `Public Function GetPoliciesPortfolioTransfer(ByVal v_lProductID As Integer, ByVal v_dtTransferDate As Date, ByVal v_nBranchID As Integer, ByRef r_vPolicyArray(,) As Object) As Integer` | Get policies eligible for portfolio transfer. |
| `GetRiskStatus` | `Public Function GetRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer` | Get risk status details. |
| `ProcessSinglePolicy` | `Public Function ProcessSinglePolicy(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByVal dtStartDate As Date, ByVal dtEndDate As Date, ByVal dtInceptionDate As Date, ByVal nProductId As Integer, ByVal sInsuranceFileType As String, ByVal bSkipPosting As Boolean, ByRef r_sMessage As String) As Integer` | Process a single policy for portfolio transfer — copy header, risks, recalculate RI, post stats. |
| `CopyAllRisk` | `Public Function CopyAllRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtPolicyStartDate As Date, ByVal v_sTransactionType As String, ByRef r_sMessage As String, ByVal v_dtTransferDate As Date, Optional v_bIgnoreError As Boolean = False) As Integer` | Copy all risks from old to new insurance file. |
| `CreateAndPostStats` | `Public Function CreateAndPostStats(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String, Optional ByVal nPTInsuranceFileCnt As Integer = 0, Optional ByVal bReversePT As Boolean = False, Optional ByVal dtTransferDate As Date = #1/1/2000#, Optional ByRef r_sMessage As String = "") As Integer` | Create and post stats for portfolio transfer. |
| `DeleteInsFilePTRIUsage` | `Public Function DeleteInsFilePTRIUsage(ByVal v_lInsFilePTRIUsageID As Integer) As Integer` | Delete insurance file PT RI usage record. |
| `GetPTRIPolicy` | `Public Function GetPTRIPolicy(ByRef r_vResultArray(,) As Object) As Integer` | Get PT RI policy records for amendment. |
| `InsertInsFilePTRIUsage` | `Public Function InsertInsFilePTRIUsage(ByVal v_lInsFileCnt As Long, ByVal v_dtTransferDate As Date) As Long` | Insert insurance file PT RI usage record. |
| `SetPTRIStatus` | `Public Function SetPTRIStatus(ByVal v_lInsFilePTRIUsageID As Long, ByVal v_lInsFileCnt As Long, ByVal v_lPTRIStatusID As Long) As Long` | Set PT RI status. |
| `SetPolicyStatus` | `Public Function SetPolicyStatus(ByVal v_lInsuranceFileCnt As Long, ByVal v_lInsuranceFileStatusID As Long, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "") As Long` | Set policy version status. |
| `RelinkRisk` | `Public Function RelinkRisk(ByVal v_lOldInsuranceFileCnt As Long, ByVal v_lNewInsuranceFileCnt As Long) As Long` | Relink risks between insurance files. |
| `DeletePolicy` | `Public Function DeletePolicy(ByVal v_lInsuranceFileCnt As Long) As Long` | Delete a policy version. |
| `CopyPolicyHeader` | `Public Function CopyPolicyHeader(ByVal v_lInsuranceFileCnt As Long, ByVal v_dTransferDate As Date, ByRef r_lNewInsurancefileCnt As Long, ByRef r_sMessage As String, Optional ByRef r_lInsuranceFolderCnt As Long = 0, Optional ByRef r_dtPolicyStartDate As Date = Nothing, Optional ByVal v_sSetOldInsuranceFileStatus As String = "", Optional ByVal v_sSetNewInsuranceFileStatus As String = "", Optional ByVal v_sTransactionType As String = "") As Long` | Copy policy header to create new version for transfer. |
| `FinaliseClaimDetails` | `Public Function FinaliseClaimDetails(ByVal v_lClaimId As Long, ByVal v_sClaimVersionDescription As String) As Long` | Finalise claim details after transfer. |
| `GetPolicyType` | `Public Function GetPolicyType(ByVal v_lInsuranceFileCnt As Long, ByRef r_lInsuranceFileTypeId As Long) As Long` | Get insurance file type. |
| `GetPreviousRiskCnt` | `Public Function GetPreviousRiskCnt(ByVal v_lPreInsuranceFileCnt As Long, ByVal v_lRiskCnt As Long, ByVal v_lInsuranceFileCnt As Long, ByRef r_lPreviousRiskCnt As Long) As Long` | Get previous risk count for transfer mapping. |
| `CopyRisk` | `Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Long, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Long, ByRef r_lRiskCnt As Long, Optional ByVal v_lResetStatus As Long = 0, Optional ByVal v_lCreateLinkType As Long = 0, Optional ByVal v_bAutoCancellation As Boolean = False, Optional v_sRiskMergeStatus As String = "", Optional v_lOldRiskCnt As Long = 0) As Long` | Copy a risk for portfolio transfer. |
| `CopyRatings` | `Public Function CopyRatings(v_lInsuranceFileCnt As Long, r_lOriginalRiskCnt As Long, r_lRiskCnt As Long, dProRataRate As Double)` | Copy ratings with pro-rata calculation. |
| `CopyRatingSectionsAndPerils` | `Public Function CopyRatingSectionsAndPerils(ByVal dtResult As DataTable, ByVal i_ThisPremiumSign As Integer, ByVal i_OriginalFlag As Integer, ByVal m_lInsuranceFileCnt As Long, ByVal m_lRiskCnt As Long, iIndex As Integer, Optional dProrata As Double = 0) As Long` | Copy rating sections and perils with pro-rata. |
| `GetAllRiskStatus` | `Public Function GetAllRiskStatus(ByVal v_lInsuranceFileCnt As Long, ByRef r_bIsRisksQuoted As Boolean) As Integer` | Get all risk statuses and check if quoted. |
| `GetProRataRate` | `Public Function GetProRataRate(ByVal nProductID As Integer, ByVal dtOldStartDate As Date, ByVal dtOldEndDate As Date, ByVal dtStartDate As Date, ByVal dtEndDate As Date, ByRef o_dProRataRate As Double, Optional ByRef o_dtInceptionDate As Date = #12/30/1899#) As Integer` | Calculate pro-rata rate for transfer. |
| `RecalculateRI` (overload 1) | `Public Overloads Function RecalculateRI(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByVal dProRataRate As Double, ByVal nIsPT As Integer, ByRef r_nIsValid As Integer) As Integer` | Recalculate reinsurance for portfolio transfer. |
| `RecalculateRI` (overload 2) | `Public Overloads Function RecalculateRI(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByVal dProRataRate As Double, ByVal nIsPT As Integer, ByRef r_nIsValid As Integer, ByVal bIsForAmend As Boolean) As Integer` | Recalculate RI with amend flag. |
| `RecalculateRIQuote` | `Public Function RecalculateRIQuote(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByRef o_nIsValid As Integer) As Integer` | Recalculate RI for quote. |
| `GetProductAndBranchDetails` | `Public Function GetProductAndBranchDetails(ByRef dtProductDetails As DataTable, ByRef dtBranchDetails As DataTable) As Integer` | Get product and branch details for transfer UI. |
| `GetClaimsPortfolioTransfer` | `Public Function GetClaimsPortfolioTransfer(ByVal nProductID As Integer, ByVal nBranchID As Integer, ByVal v_dtTransferDate As Date, ByRef r_oClaimsArray(,) As Object) As Integer` | Get claims eligible for portfolio transfer. |
| `CheckRIOnClone` | `Public Function CheckRIOnClone(ByVal nInsuranceFileCnt As Integer, ByRef r_bIsCloned As Boolean) As Integer` | Check if RI exists on cloned policy. |
| `ProcessSingleClaim` | `Public Function ProcessSingleClaim(ByVal nInsuranceFileCnt As Integer, ByVal nClaimId As Integer, ByRef r_sMessage As String) As Integer` | Process a single claim for portfolio transfer. |
| `CreateClaimVersionForPT` | `Public Function CreateClaimVersionForPT(ByVal nInsuranceFileCnt As Integer, ByVal nClaimId As Integer, ByRef nNewClaimId As Integer, ByRef r_nStatsFolderCnt As Integer, ByVal nIsPreTransfer As Integer) As Integer` | Create new claim version for portfolio transfer. |
| `GetPolicyListDetails` | `Public Function GetPolicyListDetails(ByRef r_oPoliciesDetails(,) As Object) As Integer` | Get portfolio policy list details. |
| `GetPortfolioTransferDate` | `Public Function GetPortfolioTransferDate(ByRef r_dtTransferDate As Date) As Integer` | Get portfolio transfer date. |
| `GetPoliciesPortfolioTransferRI2007Off` | `Public Function GetPoliciesPortfolioTransferRI2007Off(ByVal v_nProductID As Integer, ByVal v_dtTransferDate As Date, ByRef r_oPolicyArray(,) As Object) As Integer` | Get policies for transfer when RI2007 is disabled. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_ri_portfolio_policy_Sel` | `GetPoliciesPortfolioTransfer` | Select policies for PT. |
| `spu_RI2007Disabled_Portfolio_Policy_Sel` | `GetPoliciesPortfolioTransferRI2007Off` | Select policies for PT (RI2007 off). |
| `spu_Insurance_File_Deferred_RI_Usage_upd` | `ProcessSinglePolicy` | Update deferred RI usage. |
| `spu_Insurance_File_Deferred_RI_Usage_del` | `ProcessSinglePolicy` | Delete deferred RI usage. |
| `spu_Insurance_File_Deferred_RI_Usage_ins` | `ProcessSinglePolicy` | Insert deferred RI usage. |
| `spu_Deferred_RI_Change_RI_Model` | `ProcessSinglePolicy` | Change deferred RI model on risk. |
| `spu_CLM_risk_status_sel` | `GetRiskStatus` | Select claim risk status. |
| `spu_GetRiskDeferredRIBand` | `ProcessSinglePolicy` | Get deferred RI band for risk. |
| `spu_MoveClaimToNewRisk` | `ProcessSinglePolicy` | Move claim to new risk. |
| `spu_is_risk_marked_for_portfolio_transfer` | `CopyAllRisk` | Check risk PT flag. |
| `spu_Insurance_File_PT_RI_Usage_del` | `DeleteInsFilePTRIUsage` | Delete PT RI usage. |
| `spu_DeletePolicy` | `DeletePolicy` | Delete policy. |
| `spu_Insurance_File_PT_RI_Usage_upd` | `SetPTRIStatus` | Update PT RI status. |
| `spu_Ins_File_PT_RI_Usage_sel_amend` | `GetPTRIPolicy` | Select PT RI policy for amend. |
| `spu_PortfolioTransfer_RI_Usage_ins` | `InsertInsFilePTRIUsage` | Insert PT RI usage. |
| `spu_get_all_claims_on_risk` | `ProcessSingleClaim` | Get claims on risk. |
| `spu_CLM_NetOf_Claim_Peril_Reserve` | `ProcessSingleClaim` | Net of claim peril reserve. |
| `spu_SAM_CLM_Get_Claim_Perils` | `ProcessSingleClaim` | Get claim perils. |
| `spu_CLM_Get_NetOf_Claim_Peril` | `ProcessSingleClaim` | Get netted claim perils. |
| `spu_add_stats_folder_claims` | `ProcessSingleClaim` | Add claims stats folder. |
| `spu_add_stats_details_claims` | `ProcessSingleClaim` | Add claims stats details. |
| `spu_CLM_Finalise_stats` | `ProcessSingleClaim` | Finalise claim stats. |
| `spu_CLM_Claim_Is_Dirty_Update` | `ProcessSingleClaim` | Update claim dirty flag. |
| `spu_Get_Insurance_Ref` | `ProcessSinglePolicy` | Get insurance reference. |
| `spu_get_Previous_RiskCnt_ForTransfer` | `GetPreviousRiskCnt` | Get previous risk count. |
| `spe_Risk_add` | `CopyRisk` | Add risk. |
| `spu_sir_rating_section_sel_original` | `CopyRatings` | Select original rating sections. |
| `spu_sir_peril_allocation` | `CopyRatingSectionsAndPerils` | Allocate perils. |
| `spu_all_risk_status_sel` | `GetAllRiskStatus` | Select all risk statuses. |
| `spu_sir_copy_policy_for_pt` | `CopyPolicyHeader` | Copy policy for PT. |
| `spu_recalculate_RI` | `RecalculateRI` | Recalculate RI. |
| `spu_CLM_Finalise_Claim_Details` | `FinaliseClaimDetails` | Finalise claim details. |
| `spu_Claim_portfolio_transfer_sel` | `GetClaimsPortfolioTransfer` | Select claims for PT. |
| `spu_get_max_policy_version_no` | `CopyPolicyHeader` | Get max policy version number. |
| `spu_RI_PTCheckAndCancelPolicy` | `ProcessSinglePolicy` | Check and cancel policy for PT. |
| `spu_Update_Portfolio_Renewal_Status` | `ProcessSinglePolicy` | Update portfolio renewal status. |
| `spu_check_in_renewal` | `ProcessSinglePolicy` | Check if in renewal. |
| `spu_count_policies_in_renewal` | `ProcessSinglePolicy` | Count policies in renewal. |
| `spu_recalculate_RI_Quote` | `RecalculateRIQuote` | Recalculate RI quote. |
| `spu_get_all_product_and_branch_details` | `GetProductAndBranchDetails` | Get product/branch details. |
| `spu_check_ri_on_cloned` | `CheckRIOnClone` | Check RI on cloned policy. |
| `spu_create_claim_portfolio_transfer_version` | `CreateClaimVersionForPT` | Create claim PT version. |
| `spu_get_portfolio_policylist` | `GetPolicyListDetails` | Get portfolio policy list. |
| `spu_Get_Portfolio_Transfer_Date` | `GetPortfolioTransferDate` | Get PT date. |

**Component References:**

| Component | Purpose |
|-----------|---------|
| `bSIRRenSelection` | Renewal selection operations. |
| `bSIRInsuranceFile` | Insurance file operations. |
| `bSIRReinsuranceRI2007` / `bSIRReinsurance` | Reinsurance operations. |
| `bSIRRITax` | Tax calculation. |
| `bSIRRiskData` | Risk data operations. |
| `bSirPerilAllocation` | Peril allocation. |
| `bControlTrans` | Transaction posting. |
| `bCLMReinsuranceRI2007` / `bCLMReinsurance` | Claim reinsurance. |
| `bCLMFindClaim` | Find claim. |
| `bControlTransClaims` | Claim transaction posting. |

---

### Risk Type RI Limits
**Directory:** `Risk Type RI Limits/`
**Project:** `bSIRRiskTypeRILimits`
**Purpose:** RI limit configuration per risk type — limit versions, GIS properties.

**Business Methods — bSIRRiskTypeRILimits:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRiskTypeRILimits` | `Public Function GetRiskTypeRILimits(ByRef r_vRiskTypeRILimits(,) As Object) As Integer` | Get RI limits for current risk type. |
| `GetAllowedProperties` | `Public Function GetAllowedProperties(ByRef r_vAllowedProperties(,) As Object) As Integer` | Get allowed GIS properties for RI limits. |
| `Update` | `Public Function Update(ByVal v_vRiskTypeRILimits(,) As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Save RI limit changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_risk_type_ri_limit_saa` | `GetRiskTypeRILimits` | Select RI limits. |
| `spe_risk_type_ri_propertie_dar` | `Update` | Delete RI limit properties. |
| `spe_risk_type_ri_propertie_add` | `Update` | Add RI limit property. |
| `spu_ri_limit_gis_properties` | `GetAllowedProperties` | Get GIS properties for RI. |
| `spe_risk_type_ri_values_dar` | `Update` | Delete RI values. |
| `spu_risk_type_ri_limit_version_saa` | `GetRiskTypeRILimits` | Select RI limit versions. |
| `spu_risk_type_ri_limit_version_copy` | `Update` | Copy RI limit version. |
| `spu_risk_type_ri_limit_version_upd` | `Update` | Update RI limit version. |
| `spu_risk_type_ri_limit_version_add` | `Update` | Add RI limit version. |
| `spu_risk_type_ri_limit_version_del` | `Update` | Delete RI limit version. |

---

### Risk Type RI Values
**Directory:** `Risk Type RI Values/`
**Project:** `bSIRRiskTypeRIValues`
**Purpose:** RI value configuration per risk type.

**Business Methods — bSIRRiskTypeRIValues:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRiskTypeRIValues` | `Public Function GetRiskTypeRIValues(ByRef r_vRiskTypeRIValues As Object) As Integer` | Get RI values for current risk type. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_risk_type_ri_propertie_sel` | `GetRiskTypeRIValues` | Select RI property. |
| `spe_risk_type_ri_values_sel` | `GetRiskTypeRIValues` | Select RI values. |
| `spe_risk_type_ri_values_add` | `GetRiskTypeRIValues` | Add RI values. |
| `spu_ri_limit_gis_properties` | `GetRiskTypeRIValues` | Get GIS properties. |
| `spe_risk_type_ri_values_dar` | `GetRiskTypeRIValues` | Delete RI values. |
| `spu_risk_type_ri_model_max_limit` | `GetRiskTypeRIValues` | Get RI model max limit. |

---

### Select Clauses
**Directory:** `Select Clauses/`
**Project:** `bSIRSelectClausesBusiness`
**Purpose:** Manage clause linking to products and risk types.

**Business Methods — bSIRSelectClausesBusiness:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `DelSelectedClausesProperties` | `Public Function DelSelectedClausesProperties(ByVal v_lClauseType As Integer, ByVal v_lRisk_Type_Id As Integer, ByVal v_lProduct_Type_Id As Integer, ByVal v_vSelectedClauses(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Delete selected clause properties. |
| `GetAllClauses` | `Public Function GetAllClauses(ByVal v_lClauseType As Integer, ByVal v_lRiskType As Integer, ByVal v_lProduct_id As Integer, ByRef r_vReturnValues(,) As Object) As Integer` | Get all clauses for a clause type, risk type, and product. |
| `GetAllBranches` | `Public Function GetAllBranches(ByRef r_vReturnValues(,) As Object) As Integer` | Get all branches. |
| `UpdateSelectedClausesProperties` | `Public Function UpdateSelectedClausesProperties(ByVal v_lClauseType As Integer, ByVal v_lRisk_Type_Id As Integer, ByVal v_lProduct_Type_Id As Integer, ByVal v_vSelectedClauses(,) As Object, ByVal v_vBranches(,) As Object, ByVal v_bDefaultClause As Boolean, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer` | Update selected clause properties with branch assignment. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Get_Branches` | `GetAllBranches` | Get all branches. |
| `spu_Risk_Type_Clauses_Sel` | `GetAllClauses` (risk type) | Select risk type clauses. |
| `spu_Product_Clauses_Sel` | `GetAllClauses` (product) | Select product clauses. |
| `spu_Risk_Type_Linked_Clauses_add` | `UpdateSelectedClausesProperties` | Add risk type linked clause. |
| `spu_Product_Linked_Clauses_add` | `UpdateSelectedClausesProperties` | Add product linked clause. |
| `spu_Risk_Type_Linked_Clauses_del` | `DelSelectedClausesProperties` | Delete risk type linked clause. |
| `spu_Product_Linked_Clauses_del` | `DelSelectedClausesProperties` | Delete product linked clause. |
| `spu_Risk_Type_Linked_Clauses_upd` | `UpdateSelectedClausesProperties` | Update risk type linked clause. |
| `spu_Product_Linked_Clauses_upd` | `UpdateSelectedClausesProperties` | Update product linked clause. |

---

### Short Period Rate
**Directory:** `Short Period Rate/`
**Project:** `bSIRShortPeriodRate`
**Purpose:** Short period rate tables — pro-rata and manual rates for mid-term adjustments.

**Business Methods — bSIRShortPeriodRate:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetRefund` | `Public Function GetRefund(ByVal v_vProductID As Integer, ByVal v_vType As String, ByVal v_vStartDate As Object, ByVal v_vEndDate As Object, ByVal v_vTransactDate As Date, ByVal v_vPremium As Object, ByRef r_vRefundValue As Double) As Integer` | Calculate refund value for short period. |
| `GetShortPeriodRate` | `Public Function GetShortPeriodRate(ByVal v_lProductID As Integer, ByVal v_sType As String, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByVal v_dtTransactDate As Date, ByRef r_dRefundRate As Double) As Integer` | Get the short period rate for date range. |
| `SearchAll` | `Public Function SearchAll(ByRef r_vResultArray(,) As Object, ByVal v_vProductID As Object) As Integer` | Search all short period rates for a product. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spe_Short_Period_Rates_Add` | `SearchAll` (maintenance) | Add short period rate. |
| `spu_Short_Period_Rates_Del` | `SearchAll` (maintenance) | Delete short period rate. |
| `spu_Short_Period_Rates_Sel` | `SearchAll`, `GetShortPeriodRate` | Select short period rates. |

---

### Source Defaults
**Directory:** `Source Defaults/`
**Project:** `bPMUSourceDefaults`
**Purpose:** Branch-level default settings (agents, products).

**Business Methods — bPMUSourceDefaults:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetLookUp` | `Public Function GetLookUp(ByVal v_sTable As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lSourceID As Integer = 0) As Integer` | Get lookup values for configuration. |
| `SaveSourceDefaults` | `Public Function SaveSourceDefaults(ByVal v_lSourceID As Integer, ByRef v_iDirectBusiness As Integer, ByRef v_vAgentID As Object) As Integer` | Save branch default settings. |
| `GetSourceDefaults` | `Public Function GetSourceDefaults(ByVal v_lSourceID As Integer, ByRef r_iDirectBusiness As Integer, ByRef r_lAgentID As Integer) As Integer` | Get branch default settings. |
| `GetBranchAgents` | `Public Function GetBranchAgents(ByVal v_lSourceID As Integer, ByRef r_vResultArray(,) As Object) As Integer` | Get agents for a branch. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_get_branch_defaults` | `GetSourceDefaults` | Get branch defaults. |
| `spu_save_branch_defaults` | `SaveSourceDefaults` | Save branch defaults. |
| `spu_Get_BranchAgents` | `GetBranchAgents` | Get branch agents. |

---

### Tax Band Rate
**Directory:** `Tax Band Rate/`
**Project:** `bSIRTaxBandRate`
**Purpose:** Tax band rate CRUD.

**Business Methods — bSIRTaxBandRate:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetTaxBandRate` | `Public Function GetTaxBandRate(ByRef r_vTaxBandRate(,) As Object) As Integer` | Get all tax band rates. |
| `Update` | `Public Function Update(ByVal v_vTaxBandRate(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Save tax band rate changes. |
| `GetCOBRatingSectionsForRisk` | `Public Function GetCOBRatingSectionsForRisk(ByVal v_lRiskCodeID As Integer, ByRef r_vResults(,) As Object) As Integer` | Get COB rating sections for a risk. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Tax_Band_Rate_saa` | `GetTaxBandRate` | Select tax band rates. |
| `spu_Tax_Band_Rate_del` | `Update` | Delete tax band rate. |
| `spu_Tax_Band_Rate_add` | `Update` | Add tax band rate. |
| `spu_risk_tax_usage_sel` | `GetCOBRatingSectionsForRisk` | Select risk tax usage. |
| `spu_Tax_Band_Rate_update` | `Update` | Update tax band rate. |

---

### Tax Group Bands
**Directory:** `Tax Group Bands/`
**Project:** `bSIRTaxGroupBands`
**Purpose:** Tax group band membership CRUD.

**Business Methods — bSIRTaxGroupBands:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `GetTaxGroupTaxBands` | `Public Function GetTaxGroupTaxBands(ByRef r_vTaxGroupTaxBands(,) As Object) As Integer` | Get tax group to tax band mappings. |
| `Update` | `Public Function Update(ByVal v_vTaxGroupTaxBands(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer` | Save tax group band changes. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_taxgroup_taxbands_sel` | `GetTaxGroupTaxBands` | Select tax group bands. |
| `spu_taxgroup_taxbands_del` | `Update` | Delete tax group band. |
| `spu_taxgroup_taxbands_add` | `Update` | Add tax group band. |

---

### Treaty
**Directory:** `Treaty/`
**Project:** `bSIRTreaty`
**Purpose:** Treaty CRUD — treaty parties, broker participants, effective periods.

**Business Methods — bSIRTreaty:**

| Method | Signature | Description |
|--------|-----------|-------------|
| `Initialise` | `Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long` | Standard IBusiness initialisation. |
| `SetProcessModes` | `Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer` | Set task/navigation/process mode context. |
| `AddTreaty` | `Public Function AddTreaty(ByRef r_lTreatyID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_sAgreementCode As String, ByVal v_lReinsuranceTypeID As Integer, ByVal v_lReplacesTreatyID As Object, ByVal v_vTreatyParties(,) As Object, ByVal v_dtReplacedEffectiveDt As Object, ByVal v_lReplacedByTreatyID As Object, ByVal v_dTreatyLimit As Decimal, ByVal v_lCurrencyID As Integer, ByVal v_lReinstatements As Integer, Optional ByVal v_vTreatyPartiesBrokerParticipants(,) As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Add a new treaty with parties, limits, and broker participants. |
| `DeleteTreaty` | `Public Function DeleteTreaty(ByVal v_lTreatyID As Integer, Optional ByVal v_bIsDeleted As Boolean = True, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "") As Integer` | Soft-delete a treaty. |
| `GetTreatyList` | `Public Function GetTreatyList(ByRef r_vTreaties(,) As Object) As Integer` | Get all treaties. |
| `GetTreatyPartyList` | `Public Function GetTreatyPartyList(ByVal v_lTreatyID As Integer, ByRef r_vTreatyParties(,) As Object) As Integer` | Get parties for a treaty. |
| `GetTreatyPartyTaxInfo` | `Public Function GetTreatyPartyTaxInfo(ByVal lPartyCnt As Integer, ByRef r_iIsDomiciledForTax As Integer, ByRef r_lTaxGroupID As Integer) As Integer` | Get tax domicile and tax group for a treaty party. |
| `UpdateTreaty` | `Public Function UpdateTreaty(ByVal v_lTreatyID As Integer, ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_bIsDeleted As Boolean, ByVal v_dtEffectiveDate As Date, ByVal v_dtExpiryDate As Object, ByVal v_sAgreementCode As String, ByVal v_lReinsuranceTypeID As Integer, ByVal v_lReplacesTreatyID As Object, ByVal v_vTreatyParties(,) As Object, ByVal v_dtReplacedEffectiveDate As Date, ByVal v_vlReplacedByTreatyID As Object, ByVal v_dTreatyLimit As Decimal, ByVal v_lCurrencyID As Integer, ByVal v_lReinstatements As Integer, Optional ByVal v_vTreatyPartiesBrokerParticipants(,) As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer` | Update an existing treaty. |

**Stored Procedures:**

| Stored Procedure | Called By | Purpose |
|-----------------|-----------|---------|
| `spu_Treaty_add` | `AddTreaty` | Insert treaty. |
| `spu_Treaty_del` | `DeleteTreaty` | Delete treaty. |
| `spu_Treaty_saa` | `GetTreatyList` | Select all treaties. |
| `spu_Treaty_upd` | `UpdateTreaty` | Update treaty. |
| `spu_Treaty_Party_add` | `AddTreaty`, `UpdateTreaty` | Add treaty party. |
| `spu_Treaty_Party_del` | `UpdateTreaty` | Delete treaty party. |
| `spu_Treaty_Party_saa` | `GetTreatyPartyList` | Select treaty parties. |
| `spe_Party_Insurer_sel` | `GetTreatyPartyTaxInfo` | Select party insurer. |
| `Spu_Treaty_EffectivePeriod_sel` | `GetTreatyList` | Select treaty effective periods. |
| `spu_Treaty_Party_BrokerParticipants_saa` | `GetTreatyPartyList` | Select broker participants. |
| `spu_Treaty_PartyBrokerParticipant_add` | `AddTreaty`, `UpdateTreaty` | Add broker participant. |
| `spu_Treaty_PartyBrokerParticipant_del` | `UpdateTreaty` | Delete broker participant. |

---

## Component Size Summary

| Component | Methods | Stored Procedures | References |
|-----------|---------|-------------------|------------|
| **Renewal** | 442 | 201 | 18 |
| **List Risks** | 86 | 86 | 5 |
| **Reinsurance** | 74 | 38 | 2 |
| **Repost Transaction** | 63 | 36 | 0 |
| **RI Portfolio Transfer** | 62 | 47 | 4 |
| **Clone RI Transfer Auto** | 44 | 39 | 4 |
| **Chase Cycle** | 41 | 33 | 4 |
| **Peril Allocation** | 41 | 31 | 1 |
| **Product** | 37 | 33 | 0 |
| **Deferred RI Auto** | 33 | 30 | 6 |
| **Tax Calculation (bSIRRITax)** | 32 | 21 | 0 |
| **Statistics (bControlTrans)** | 30 | 50 | 12 |
| **Find Risk** | 29 | 14 | 1 |
| **Underwriting Authority** | 29 | 27 | 0 |
| **Risk** | 29 | 17 | 0 |
| **Auto MTA** | 24 | 34 | 0 |
| **Claims Stats** | 24 | 17 | 2 |
| **Policy** | 23 | 22 | 1 |
| **Risk Type** | 21 | 27 | 0 |
| **Cover Note** | 21 | 17 | 0 |
| **RI Model** | 18 | 14 | 0 |
| **Agent Commission** | 16 | 10 | 2 |
| **Batch Renewals** | 16 | 19 | 0 |
| **Accumulations** | 15 | 0 | 1 |
| **Commission Rate** | 13 | 8 | 0 |
| **Change Policy Status** | 12 | 9 | 3 |
| **Lookup Detail** | 12 | 6 | 0 |
| **Lookup Header** | 12 | 11 | 0 |
| **Risk Type RI Limits** | 11 | 10 | 0 |
| **Select Clauses** | 11 | 9 | 0 |
| **Pay Now Options** | 10 | 6 | 0 |
| **Short Period Rate** | 9 | 3 | 0 |
| **MID Maintenance** | 9 | 7 | 0 |
| **Peril Type Usage** | 8 | 5 | 0 |
| **Source Defaults** | 8 | 3 | 0 |
| **RI Model Usage** | 8 | 6 | 0 |
| **Primary Cause Risk Type** | 8 | 4 | 0 |
| **Payment Hub Wrapper** | 7 | 5 | 1 |
| **Risk Type RI Values** | 7 | 6 | 0 |
| **Coinsurance** | 7 | 7 | 0 |
| **Follow Up Tasks** | 6 | 2 | 0 |
| **Index Linking** | 6 | 3 | 0 |
| **Tax Band Rate** | 6 | 5 | 0 |
| **Tax Group Bands** | 6 | 3 | 0 |
| **Find Product Type** | 6 | 1 | 0 |
| **RI Band Version** | 5 | 3 | 0 |
| **Find Risk Type** | 5 | 0 | 0 |
| **Find Screen** | 5 | 0 | 0 |
| **Quote Engine** | 5 | 12 | 1 |
| **Produce Certificate** | 5 | 1 | 0 |
| **Documentation** | 4 | 2 | 0 |
| **Client Transfer** | 4 | 1 | 1 |
| **Batch Quote Deletion** | 4 | 2 | 0 |
| **Treaty** | 12 | 11 | 0 |
| **Batch Controller** | 15 | 10 | 1 |
| **Batch Notification** | 13 | 9 | 1 |
| **Reinsurance Transfer** | 10 | 0 | 3 |

---

## Cross-Component Dependency Map

### Internal Dependencies (within Sirius For Underwriting)

| Component | Referenced By |
|-----------|---------------|
| `bSIRRiskData` | Clone RI Transfer, Deferred RI, List Risks, RI Portfolio Transfer |
| `bSIRRITax` | Clone RI Transfer, Deferred RI, List Risks, RI Portfolio Transfer, Renewal |
| `bSIRReinsurance` | Accumulations, Deferred RI, Find FAC Party |
| `bSIRFindInsurance` | List Risks, Policy |
| `bSIRAutoMTA` | Chase Cycle |
| `bSIRShortPeriodRate` | Peril Allocation |
| `bSIRRenSelection` | Clone RI Transfer, Deferred RI, RI Portfolio Transfer, Renewal |
| `bSIRListRisks` | Renewal |
| `bSIRChangePolicyStatus` | Renewal |
| `bSIRProduct` | Renewal |
| `bSIRCloneRIBatchProcess` | Reinsurance Transfer |
| `bSIRRIPortfolioTransfer` | Reinsurance Transfer |
| `bSIRAutomaticRenewalsAccept` | Batch Controller |

### Dependencies on Back Office Core

| External Component | Called By |
|-------------------|-----------|
| `bSIRInsuranceFile` | Agent Commission, Auto MTA, Change Policy Status, Clone RI Transfer, Deferred RI, RI Portfolio Transfer, Renewal |
| `bSIREvent` | Chase Cycle, Client Transfer, Deferred RI, Find FAC Party, Renewal |
| `bSIROptions` | Find Risk, Renewal, Batch Notification |
| `bSIRPolicyNumMaint` | Change Policy Status, Renewal |
| `bSIRParty` | Agent Commission |
| `bSIRPartyFee` | List Risks, Renewal |
| `bSIRDocManagerWrapper` / `bSIRDocTemplate` / `bSIRFindDocTemplate` | Chase Cycle, Renewal |
| `bSIRReportPrint` | Renewal |
| `bSIRPremiumFinance` | Renewal, Statistics |
| `bSIRPaymentHubWrapper` | Renewal |

### Dependencies on Orion Accounting

| External Component | Called By |
|-------------------|-----------|
| `bACTAccount` / `bACTLedger` / `bACTExplorer` | Statistics |
| `bACTTransdetail` / `bACTDocumentPost` | Statistics |
| `bACTAllocationManual` / `bACTAllocate` | Statistics |
| `bACTCurrencyConvert` | Renewal |
| `bACTPeriod` | Change Policy Status, Claims Stats |
| `bACTCashListPost` | Payment Hub Wrapper, Statistics |
| `bPMBTransactions` | Claims Stats, Statistics |
