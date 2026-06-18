# RI Tax Calculation — How It Works

## Overview

Tax on reinsurance treaty lines is **never** calculated by stored procedures. It is calculated exclusively at the **portal application layer** via SAM (Service Access Manager) web service calls.

## Tax Calculation Method

### GetRITreatyPartyDetailsWithTax (Primary method)

Used for ALL treaty nodes because `PartyKey=0` in the RI XML.

```
Portal → SAM.GetRITreatyPartyDetailsWithTax(request)
  Input:
    - TreatyCode (e.g. "2014QSH")
    - TreatyID (e.g. 24)
    - Premium (amount to calculate tax on)
    - Commission (amount to calculate commission tax on)
    - PremiumTransType = "TTRITP" (treaty) or "TTRIFP" (FAC)
    - CommissionTransType = "TTRITC" (treaty) or "TTRIFC" (FAC)
    - RiskID, InsuranceFileID
    - IgnoreDetails = True
    - IgnoreTax = False
  
  SAM internally:
    1. treaty_id → resolves party_cnt (the reinsurer)
    2. party_cnt → party_tax_group table → tax_group_id
    3. tax_group rates for the transaction type → calculates tax
  
  Output:
    - PremiumTax (amount, NOT percentage)
    - CommissionTax (amount, NOT percentage)
    - If no tax group → returns 0
```

### CalculateRITax (Alternative — NOT used for treaty nodes)

Requires `PartyKey > 0`. Only works when the party is directly known. Treaty nodes have `PartyKey=0` so this method CANNOT be used for them.

## When Tax Is Calculated

### On Page Load
- SAM returns RI arrangement data with `premium_tax` from the database
- If `premium_tax=0` in DB (never previously calculated), portal calls `CalculateObligatoryTax()` after `Recalculate`
- `CalculateObligatoryTax()` iterates all nodes with TreatyCode and Tax=0, calls SAM for each

### On Field Edit (Premium, SumInsured, ThisPerc, CommPerc)
1. Portal's text changed handler fires
2. For manually added treaties → calls `GetTaxPercentage` which calls SAM
3. For non-manually-added treaties → preserves tax proportionally from existing ratio
4. Calls `Recalculate` (recalculates all nodes)
5. Calls `CalculateObligatoryTax()` after Recalculate as safety net

### On Save (OK button)
- Portal reads `Tax` and `CommissionTax` attributes from XML
- Passes them to `spu_RI_Arrangement_Line_add` stored procedure
- Stored procedure simply INSERT/UPDATEs the values — no tax calculation

## Tax Preservation During Recalculate

`Reinsurance.vb` Recalculate method processes all nodes. For tax preservation:

### Critical Rule: Order of Operations
```
ALWAYS: Read tax rate → THEN overwrite Premium/Commission → THEN apply rate to new values
NEVER:  Overwrite Premium → THEN read tax rate (gets wrong ratio)
```

### In CalculateFACQSH (Obligatory nodes)
```vb
' 1. Derive rates from OLD values
Dim dTQS_taxperc = GetTaxPercentage(oTSHNode, "Premium", "Tax")
Dim dTQS_CommTaxperc = GetTaxPercentage(oTSHNode, "Commission", "CommissionTax")
' 2. Overwrite with new calculated values
oTSHNode.Attributes("Premium").Value = dthis_premium
' 3. Apply rates to new values
oTSHNode.Attributes("Tax").Value = (dTQS_taxperc * dthis_premium) / 100
```

### In Priority Loop — IsEditedDB Guard
```vb
' 1. Derive CommissionTax rate from OLD commission
Dim dOldCommForTax = CDbl(node.Attributes("Commission").Value)
Dim dCommTaxPerc = CommissionTax * 100 / dOldCommForTax
' 2. Overwrite Commission
node.Attributes("Commission").Value = newCommission
' 3. Apply rate
node.Attributes("CommissionTax").Value = newCommission * dCommTaxPerc / 100
```

### In Priority Loop — Normal Proportional
```vb
' Rate derived before Premium overwrite (line 980)
dPT_taxperc = Tax * 100 / Premium  ' OLD values
' Then Premium is overwritten (line 988)
' Then Tax is recalculated (line 993)
oTreatyNode.Attributes("Tax").Value = (dPT_taxperc * dthis_premium) / 100
```

## Database Tables

| Table | Role |
|-------|------|
| `ri_arrangement_line` | Stores `premium_tax`, `commission_tax` per line |
| `treaty` | Maps `treaty_id` → `code`, links to party |
| `party` | The reinsurer (`party_cnt`) |
| `party_tax_group` | Links party to tax group |
| `tax_group` | Defines tax rates per transaction type |

## Key Files

| File | Responsibility |
|------|---------------|
| `Reinsurance2007.ascx.vb` | Portal UI, field handlers, `CalculateObligatoryTax()`, calls SAM |
| `Reinsurance.vb` | Calculation engine, `Recalculate`, `CalculateFACQSH`, `GetTaxPercentage` |
| `ProviderSAMForInsuranceV2.Reinsurance2007.vb` | SAM provider, `GetRITreatyPartyDetailsWithTax`, `CalculateRITax` |
| `ProviderSAMForInsuranceV2.Policy.vb` | Save/load RI lines, reads `Tax` from XML on save |
| `RiskReinsuranceArrangementLinesRI2007UoWRepository.cs` | REST API reads `premium_tax` from DB |
| `spu_RI_Arrangement_Line_add.sql` | Inserts `premium_tax` as passed (no calculation) |
| `spu_RI_Arrangement_calc.sql` | Calculates premium/commission only (NO tax) |
