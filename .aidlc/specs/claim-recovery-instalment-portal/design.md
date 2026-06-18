# Application Design — Claim Recovery Instalment Plan - Portal Workflow

**Feature**: Claim Recovery Instalment Plan - Portal Workflow
**Spec ID**: SPEC-39472
**Source PBI**: ADO #24690
**Date**: 2026-05-07
**Last Updated**: 2026-06-05

---

## 1. Architecture Overview

This feature adds instalment plan creation to the existing recovery receipt workflow (`PerilDetails.aspx`). It reuses the existing `Instalments.ascx` user control (same control used by Premium Finance) embedded within `PayClaim.ascx`, filtered to show only Claim Recovery ("CR") schemes.

```
+------------------------------------------------------------------+
|                    PORTAL UI LAYER (ASP.NET Web Forms)            |
|                                                                  |
|  +----------------------------------------------------------+   |
|  | Claims/PerilDetails.aspx + PerilDetails.aspx.vb          |   |
|  | (inherits BasePeril)                                      |   |
|  |                                                           |   |
|  | [NEW] btnInstalments — visibility via product config      |   |
|  | [NEW] btnInstalments_Click — orchestrates instalment flow  |   |
|  | [NEW] Page_Init — CNQuote session management for recovery  |   |
|  | [NEW] SetInstalmentsTabVisibility — product option check   |   |
|  +----------------------------------------------------------+   |
|        |                                                         |
|        v                                                         |
|  +----------------------------------------------------------+   |
|  | Controls/PayClaim.ascx + PayClaim.ascx.vb                 |   |
|  |                                                           |   |
|  | [EXISTING] tab-thispayment (This Receipt tab)             |   |
|  | [NEW] tab-recovery-instalments (Instalments tab pane)     |   |
|  |   - liInstalments (tab link, Visible=false by default)    |   |
|  |   - pnlInstalments panel                                  |   |
|  |   - ucInstalments (Instalments.ascx control)              |   |
|  |   - btnSaveInstalmentPlan (Save Plan button)              |   |
|  |   - lblInstalmentMessage (status/error label)             |   |
|  | [MODIFIED] IsPaymentReceived_ServerValidate — skips       |   |
|  |   validation when Instalments tab active                   |   |
|  +----------------------------------------------------------+   |
|        |                                                         |
|        v                                                         |
|  +----------------------------------------------------------+   |
|  | Controls/Instalments.ascx + Instalments.ascx.vb           |   |
|  | (REUSED — same control as Premium Finance)                 |   |
|  |                                                           |   |
|  | [EXISTING] grdInstallmentQuotes — scheme grid              |   |
|  | [EXISTING] pnlPlanSummary — Summary/Instalments/Breakdown  |   |
|  |   /Finance Details/Deposit sub-tabs                        |   |
|  | [EXISTING] ddlDayinMonth, ddlFirstPaymentDate              |   |
|  | [EXISTING] ShowDetailsForScheme — populates all fields      |   |
|  | [EXISTING] SaveInstallmentPlan — creates payment object     |   |
|  | [MODIFIED] ShowDetailsForScheme — Try/Catch around          |   |
|  |   GetHeaderAndPolicyTaxByKey, GetHeaderAndPolicyFeesByKey,  |   |
|  |   GetAgentCommission (defaults to zero on failure)          |   |
|  | [NEW] UpdateInstalmentQuotesCache — public method for       |   |
|  |   external cache replacement (CR filtering)                 |   |
|  | [NEW] ShowDetailsForSelectedScheme — public wrapper          |   |
|  +----------------------------------------------------------+   |
|                                                                  |
+------------------------------------------------------------------+
              |
              v
+------------------------------------------------------------------+
|              API LAYER (Policy Microservice)                      |
|                                                                  |
|  [REUSE] GET /policy/instalmentQuotes (GetInstalmentQuotes)      |
|    - sProcessPFMode = "SR" (Salvage) / "TPR" (Third Party)       |
|  [REUSE] POST /policy/savePremiumFinanceDetails                  |
|  [REUSE] GET /claims/recoveryInstalment/validate                 |
|                                                                  |
+------------------------------------------------------------------+
              |
              v
+------------------------------------------------------------------+
|                    DATABASE LAYER                                 |
|                                                                  |
|  [EXISTING from Epic #39336]                                     |
|  - PFScheme.scheme_type (filter for "CR" schemes)                |
|  - PFRate.transaction_type                                       |
|  - RiskType.recovery_instalments_enabled                         |
|  - Instalment plan tables (existing PF structure)                |
|                                                                  |
+------------------------------------------------------------------+
```

---

## 2. Component Design

### 2.1 PerilDetails.aspx.vb — Orchestration Layer

**File**: `Claims/PerilDetails.aspx.vb`
**Class**: `Claims_PerilDetails` (inherits `BasePeril`)
**Change Type**: New event handlers and session management

| Handler / Method | Purpose | Status |
|---------|---------|--------|
| `Page_Init` | Product config check + CNQuote session management (clear on non-instalment postbacks to prevent WriteContainerToXML failure) | ✅ Implemented |
| `SetInstalmentsTabVisibility` | Checks `RecoveryInstalmentsEnabled` product option; shows `btnInstalments` if enabled | ✅ Implemented |
| `btnInstalments_Click` | Full orchestration: duplicate check → session setup → bindInstalments → CR filter → ShowDetails → fallback cache | ✅ Implemented |
| `cvMediaTypeAndDefaultBankAccountForReciept_ServerValidate` | Skips receipt validation when Instalments tab active | ✅ Implemented |
| `Page_PreRender` | Keeps Instalments tab active on postback; hides This Receipt tab | ✅ Implemented |

---

### 2.2 PayClaim.ascx — Instalments Tab Container

**File**: `Controls/PayClaim.ascx` and `Controls/PayClaim.ascx.vb`

| Element | Type | Purpose | Status |
|---------|------|---------|--------|
| `liInstalments` | `<li>` (runat=server) | Tab link for Instalments (Visible=false default) | ✅ Added |
| `tab-recovery-instalments` | `<div>` tab pane | Container for instalment content (unique ID to avoid collision with Instalments.ascx sub-tabs) | ✅ Added |
| `pnlInstalments` | ASP.NET Panel | Wraps ucInstalments + save button | ✅ Added |
| `ucInstalments` | User Control | Reused `Instalments.ascx` control | ✅ Registered |
| `btnSaveInstalmentPlan` | LinkButton | Saves plan (CausesValidation=false, ValidationGroup=InstalmentPlanSave) | ✅ Added |
| `lblInstalmentMessage` | Label | Status/error messages | ✅ Added |
| `btnSaveInstalmentPlan_Click` | Event handler | Validates SelectedInstalmentQuote then calls SaveInstallmentPlan | ✅ Implemented |
| `IsPaymentReceived_ServerValidate` | Modified | Skips receipt/MediaType validation when `liInstalments.Visible = True` | ✅ Modified |

**Key Design Decision — Tab ID**: The outer instalment tab pane uses `id="tab-recovery-instalments"` (not `"tab-instalments"`) to avoid collision with the `#tab-instalments` sub-tab inside `Instalments.ascx` which would cause Bootstrap tab navigation conflicts.

---

### 2.3 Instalments.ascx.vb — Shared Control Modifications

**File**: `Controls/Instalments.ascx.vb`
**Change Type**: Defensive error handling + new public methods

| Change | Purpose | Status |
|--------|---------|--------|
| `ShowDetailsForScheme` — Try/Catch around `GetHeaderAndPolicyTaxByKey` | Defaults to empty Quote (zero tax) when SAM fails for claim InsuranceFileKey | ✅ Implemented |
| `ShowDetailsForScheme` — Try/Catch around `GetHeaderAndPolicyFeesByKey` | Defaults to empty Quote (zero fees) when SAM fails for claim InsuranceFileKey | ✅ Implemented |
| `ShowDetailsForScheme` — Try/Catch around `GetAgentCommission` | Defaults to zero commission when SAM fails for claim InsuranceFileKey | ✅ Implemented |
| `UpdateInstalmentQuotesCache` (new public method) | Allows external callers to replace the instalment quotes cache with a filtered collection | ✅ Implemented |
| `ShowDetailsForSelectedScheme` (new public method) | Public wrapper around private `ShowDetailsForScheme` for external invocation | ✅ Implemented |

---

### 2.4 Session Management Strategy

**Critical Design Constraint**: The claims workflow uses `Session(CNDataSet)` with `Session(CNQuote) = Nothing` for `WriteContainerToXML` (Next button). The Instalments control requires `Session(CNQuote)` to be set. These conflict.

**Solution**: Conditional session management in `Page_Init`:

```
Page_Init:
  IF IsPostBack AND Session(CNQuote) IsNot Nothing AND Mode is SA/TPR THEN
    IF __EVENTTARGET contains "instalment"/"installment" or related controls THEN
      ' Keep Session(CNQuote) — Instalments control needs it
    ELSE
      ' Clear Session(CNQuote) — WriteContainerToXML needs it to be Nothing
      Session.Remove(CNQuote)
    END IF
  END IF
```

This ensures:
- **Instalment postbacks** (scheme selection, day change, save): `Session(CNQuote)` preserved → Instalments control works
- **Non-instalment postbacks** (Next button): `Session(CNQuote)` cleared → `WriteContainerToXML` enters claims branch correctly

---

### 2.5 Orchestration Flow (btnInstalments_Click)

```
btnInstalments_Click:
  1. Duplicate plan check (GetFinancePlanDetails via InsuranceFileKey)
     → IF active plan exists: show blocking message, EXIT
  2. Get receipt amount (txtGrossPayment or claim GrossTotal)
     → IF zero: alert user, EXIT
  3. Session setup:
     - Session(CNAmountToPay) = receipt amount
     - Session.Remove(CNSelectedPaymentIndex)
     - Session(CNParty) — ensure set (for bank details)
     - Session(CNQuote) = Session(CNClaimQuote) with:
       • Risks = New RiskCollection() if Nothing
       • CoverStartDate/CoverEndDate defaults if MinValue
     - Session("PFProcessMode") = "SR" or "TPR"
     - Session(CNAgentType) = "" if Nothing
     - Session("PFClaimId") = BaseClaimKey
  4. Show liInstalments tab
  5. Invoke ucInstalments.bindInstalments() via reflection
  6. Filter grid to CR schemes only (SchemeTypeCode = "CR")
  7. Update cache with filtered quotes (UpdateInstalmentQuotesCache)
  8. Populate ddlDayinMonth (1-28) and ddlFirstPaymentDate (31 days) if empty
  9. Invoke ShowDetailsForSelectedScheme for first CR scheme
  10. Ensure pnlPlanSummary.Visible = True
  11. Fallback: if SelectedInstalmentQuote still Nothing, manually cache first quote
  12. Activate Instalments tab via JS; hide This Receipt tab
```

---

### 2.6 Validation Bypass (FR-003 Compliance)

When Instalments tab is active (This Receipt hidden):
- `IsPaymentReceived_ServerValidate` → returns `IsValid = True` immediately
- `cvMediaTypeAndDefaultBankAccountForReciept_ServerValidate` → returns `IsValid = True` immediately
- `btnSaveInstalmentPlan` uses `ValidationGroup="InstalmentPlanSave"` to isolate from default validators

---

## 3. Files Modified

| File | Changes |
|------|---------|
| `Claims/PerilDetails.aspx.vb` | New: btnInstalments_Click, SetInstalmentsTabVisibility, Page_Init CNQuote management, cvMediaType bypass, Page_PreRender tab persistence |
| `Controls/PayClaim.ascx` | New: liInstalments tab, tab-recovery-instalments pane, pnlInstalments, ucInstalments, btnSaveInstalmentPlan, lblInstalmentMessage |
| `Controls/PayClaim.ascx.vb` | New: btnSaveInstalmentPlan_Click. Modified: IsPaymentReceived_ServerValidate bypass |
| `Controls/Instalments.ascx.vb` | Modified: ShowDetailsForScheme (Try/Catch for tax/fees/commission). New: UpdateInstalmentQuotesCache, ShowDetailsForSelectedScheme |

---

## 4. Security Considerations

| Rule | Applicability | Notes |
|------|--------------|-------|
| SECURITY-03 | Applicable | Plan creation logged via SavePremiumFinanceDetails SAM |
| SECURITY-05 | Applicable | All data access via API/stored procedures |
| SECURITY-08 | Applicable | Existing claim recovery RBAC — no new roles |
| SECURITY-09 | Applicable | Error messages generic; inner exceptions not exposed to UI |
| SECURITY-15 | Applicable | All SAM calls wrapped in error handling; fail closed |

---

## 5. Testing Strategy

| Level | Scope |
|-------|-------|
| Unit Tests | Button visibility, scheme filtering (CR only), duplicate validation, session management |
| Integration Tests | End-to-end: button click → scheme selection → plan summary populated → save → verify DB |
| Regression Tests | Existing PF plan creation unaffected; existing recovery Next button workflow unaffected; WriteContainerToXML works correctly |
| UI Tests | Tab switching (This Receipt hidden, Instalments shown), sub-tab navigation (Summary/Breakdown don't redirect), save button validation bypass |

---

*Updated to reflect actual implementation. Traceability: requirements.md → design.md → tasks.md*
