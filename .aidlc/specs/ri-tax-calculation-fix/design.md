# Design — Bug #39513 (Issue 1): RI Tax Calculation Fix

## Affected Files

| File | Changes |
|------|---------|
| `Web Portal/Nexus/Pure.Portals/Controls/Reinsurance2007.ascx.vb` | CalculateObligatoryTax method, txtPremium fix, call sites, ApplyEditedRowHighlighting revert fix |
| `Web Portal/Nexus/Nexus.Reinsurance/Reinsurance.vb` | Order-of-operations fixes in CalculateFACQSH, Recalculate write-back loop, and IsEditedDB block |

## Fix 1: CalculateObligatoryTax — SAM call on page load and after edits

**File**: `Reinsurance2007.ascx.vb`

Method that calls SAM `GetRITreatyPartyDetailsWithTax` for **ALL** treaty nodes with non-zero Premium. SAM is the single source of truth for tax amounts.

**XPath**: All nodes with `@IsDeleted!='True' and @TreatyCode!='' and @Type!='' and @Premium!='0' and @Premium!='0.00'`

**No skip condition**: Every treaty node with non-zero premium is always sent to SAM. SAM resolves the party from `TreatyCode`/`TreatyID`, checks `party_tax_group`, and returns the correct `PremiumTax` and `CommissionTax` amounts. These are written directly to the XML node unconditionally (even if SAM returns 0 for parties without tax group).

**Call sites** (after every `Session(CNRIXMLData) = oXMLData`, followed by `oXMLData = Convert.ToString(Session(CNRIXMLData))` refresh):
- After page load Recalculate (in `PopulateGrid`)
- After `txtThisPerc_TextChanged` Recalculate
- After `txtSumInsured_TextChanged` Recalculate
- After `txtCommissionPerc_TextChanged` Recalculate
- After `txtPremium_TextChanged` Recalculate
- After `txtFACPropPremiumPerc_TextChanged` Recalculate

**Critical**: After `CalculateObligatoryTax()` updates `Session(CNRIXMLData)`, the local `oXMLData` variable must be re-read from session before binding to the grid. Otherwise the grid displays stale (pre-SAM) tax values.

## Fix 2: Order-of-operations in CalculateFACQSH

**File**: `Reinsurance.vb`

Moved `GetTaxPercentage` calls BEFORE `Premium` is overwritten:

```vb
' Derive rates from OLD values FIRST:
Dim dTQS_taxperc = GetTaxPercentage(oTSHNode, "Premium", "Tax")
Dim dTQS_Commperc = Convert.ToDouble(oTSHNode.Attributes("CommissionPerc").Value)
Dim dTQS_CommTaxperc = GetTaxPercentage(oTSHNode, "Commission", "CommissionTax")

' THEN overwrite with new values:
oTSHNode.Attributes("Premium").Value = dthis_premium
oTSHNode.Attributes("Tax").Value = (dTQS_taxperc * dthis_premium) / 100
oTSHNode.Attributes("Commission").Value = (dthis_premium * dTQS_Commperc) / 100
oTSHNode.Attributes("CommissionTax").Value = (newCommission * dTQS_CommTaxperc) / 100
```

## Fix 3: Order-of-operations in Recalculate write-back loop

**File**: `Reinsurance.vb`

In the main Recalculate loop that writes calculated premiums back to XML (non-QSR, non-IsPremiumEdited nodes), derive tax/commission rates BEFORE overwriting Premium:

```vb
Dim dCalcPremium As Double = Convert.ToDouble(row("Premium"))

' Derive tax rates BEFORE overwriting Premium (order-of-operations fix)
Dim dTaxPerc As Double = GetTaxPercentage(oNode, "Premium", "Tax")
Dim dCommPerc As Double = Convert.ToDouble(oNode.Attributes("CommissionPerc").Value)
Dim dCommTaxPerc As Double = GetTaxPercentage(oNode, "Commission", "CommissionTax")

' NOW overwrite Premium
oNode.Attributes("Premium").Value = dCalcPremium.ToString()

' Apply derived rates to new premium
oNode.Attributes("Tax").Value = ((dTaxPerc * dCalcPremium) / 100).ToString()
oNode.Attributes("Commission").Value = ((dCalcPremium * dCommPerc) / 100).ToString()
oNode.Attributes("CommissionTax").Value = ((newCommission * dCommTaxPerc) / 100).ToString()
```

**Why this matters**: `GetTaxPercentage` derives rate as `Tax*100/Premium`. If Premium is overwritten first, the ratio becomes `oldTax/newPremium` = wrong percentage. By reading before overwrite, we get the correct `oldTax/oldPremium` ratio.

## Fix 4: txtPremium_TextChanged — ManuallyAdded check

**File**: `Reinsurance2007.ascx.vb`

Parse `bIsManuallyAdded` boolean FIRST, then branch:

```vb
Dim bIsManuallyAdded As Boolean = False
If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
    Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
End If

If bIsManuallyAdded Then
    ' Call GetTaxPercentage (SAM) for tax
    dTaxPercentage = GetTaxPercentage(dPremium, 0, oXMLDoc.OuterXml, RIBANDID, sPremiumToolTip, sTreatyCode)
    oNode.Attributes("Tax").Value = Format((dTaxPercentage * dPremium) / 100, "0.00")
Else
    ' Preserve tax proportionally from existing Tax/Premium ratio
    Dim dOldPremium = oNode.Attributes("Premium").Value  ' OLD value before edit
    dTaxPercentage = If(dOldPremium <> 0, oldTax * 100 / dOldPremium, 0)
    oNode.Attributes("Tax").Value = Format((dTaxPercentage * dPremium) / 100, "0.00")
    ' Preserve CommissionTax proportionally from existing Commission/CommissionTax ratio
    ' Recalculate Commission from new premium and CommissionPerc
End If
```

**Previous bug**: The old code entered the `If oNode.Attributes("ManuallyAdded") IsNot Nothing` block for ALL nodes (including `ManuallyAdded="False"`), called `GetTaxPercentage` which returned 0 for non-manual treaties with PartyKey=0, then overwrote Tax to 0.

**Additional fix in `ElseIf Not bIsManuallyAdded` block**: Removed the second `GetTaxPercentage` call. The `dTaxPercentage` derived proportionally in the `Else` branch above is reused, preventing it from being zeroed again.

## Fix 5: ApplyEditedRowHighlighting — Revert to original colour

**File**: `Reinsurance2007.ascx.vb`

When a row's value is reverted to its original (causing `ClearRowEditedFlags` to set `IsUserEdited=False` and `IsEditedDB=False`), the `ri-edited-row` CSS class must be removed for ALL row types, not just Retained rows.

**Before (broken)**:
```vb
ElseIf bIsRetained AndAlso Not (bIsEdited OrElse bIsDBEdited) Then
    ' Only removed highlight for Retained rows
```

**After (fixed)**:
```vb
ElseIf Not (bIsEdited OrElse bIsDBEdited) AndAlso Not bIsManuallyAdded Then
    ' Remove highlight for ALL reverted rows
    sCss = sCss.Replace("ri-edited-row", "").Trim()
    If bIsRetained AndAlso Not sCss.Contains("ri-retained-row") Then sCss &= " ri-retained-row"
End If
```

## Impact Analysis

- **All treaties with tax group**: Tax and CommissionTax always calculated via SAM `GetRITreatyPartyDetailsWithTax` after every Recalculate
- **Treaties without tax group**: SAM returns 0, Tax correctly stays at 0
- **Premium edits (non-manual)**: Tax preserved proportionally in txtPremium handler; then SAM overwrites with correct amount in CalculateObligatoryTax
- **SumInsured edits**: Recalculate derives new premium, tax rate correctly derived from OLD values before overwrite; then SAM finalizes via CalculateObligatoryTax
- **Commission % edits**: Commission recalculated, then SAM provides correct CommissionTax via CalculateObligatoryTax
- **Obligatory (CalculateFACQSH)**: Tax rate derived before Premium overwrite
- **Row highlighting**: Reverted rows correctly lose `ri-edited-row` class for all row types
- **Performance**: SAM calls for every treaty node on every Recalculate (required for correctness since Recalculate changes premiums)
