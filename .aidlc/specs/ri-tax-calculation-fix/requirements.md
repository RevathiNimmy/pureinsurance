# Requirements — Bug #39513 (Issue 1): RI Tax Calculation Fix

## Summary

Tax and Commission Tax not calculated for treaty lines on RI screen in Pure Portal when the treaty's party has a tax group configured.

## Problem Statement

Treaty nodes on the RI2007 reinsurance screen show Tax Rs. = 0.00 and Comm Tax Rs. = 0.00 even when the treaty's party has a tax group set in the database. Additionally, when premium or commission percentage is edited, tax values are incorrectly zeroed or miscalculated.

## Root Causes

### 1. No tax calculation on page load
Tax is not calculated by any stored procedure. It is calculated exclusively at the portal layer via SAM's `GetRITreatyPartyDetailsWithTax`. No code path called this on page load — so `premium_tax` stayed 0 in the database until a user manually edited a field.

### 2. Order-of-operations in CalculateFACQSH
`GetTaxPercentage` was called AFTER `Premium` was overwritten with a new value, giving wrong Tax/Premium ratio → wrong tax.

### 3. Order-of-operations in Recalculate IsEditedDB block
`CommissionTax` rate was derived AFTER `Commission` was overwritten with new value → wrong CommissionTax/Commission ratio → wrong commission tax.

### 4. txtPremium_TextChanged zeroing tax for non-manual treaties
The `ManuallyAdded` attribute check entered for ALL nodes (including `ManuallyAdded="False"`), called `GetTaxPercentage` which returned 0 for non-manual treaties with PartyKey=0, then overwrote Tax to 0.

### 5. CalculateObligatoryTax skipping nodes with Tax≠0
When Premium Tax existed but CommissionTax was wrong, the method skipped the node entirely.

## Tax Calculation Flow (SAM GetRITreatyPartyDetailsWithTax)

1. Portal passes: `TreatyCode`, `TreatyID`, `Premium`, `Commission`, `PremiumTransType` ("TTRITP"/"TTRIFP"), `CommissionTransType` ("TTRITC"/"TTRIFC"), `IgnoreTax=False`
2. SAM resolves party via `treaty_id` → gets reinsurer `party_cnt`
3. SAM checks `party_tax_group` table for the party's tax group
4. SAM calculates tax amount using group's rates for the transaction type
5. SAM returns `PremiumTax` (amount) and `CommissionTax` (amount) — NOT percentages
6. Portal writes amounts directly to XML node's `Tax` and `CommissionTax` attributes

### Why PartyKey=0 in the XML
All treaty nodes have `PartyKey="0"`. The party is resolved by SAM from `TreatyCode`/`TreatyID`. This is why `CalculateRITax` (requires PartyKey > 0) cannot be used.

## Acceptance Criteria

1. All treaty nodes with Tax=0 or CommissionTax=0 and a valid TreatyCode must have tax calculated via SAM on page load.
2. Tax must be preserved proportionally when premium is edited on non-manually-added treaties.
3. CommissionTax must be correctly recalculated when commission percentage changes.
4. Order-of-operations: tax/commission tax rates must be derived BEFORE Premium/Commission attributes are overwritten in CalculateFACQSH and Recalculate.
5. CalculateObligatoryTax must run after EVERY Recalculate call (page load + all field edits).
6. Nodes that already have correct tax AND commission tax from DB are not re-fetched from SAM.
7. If SAM returns 0 (party has no tax group), node correctly stays at Tax=0.

## ADO Reference

- Bug: [#39513](https://dev.azure.com/SSP-Insurer/Pure%20Insurance/_workitems/edit/39513)
- Found in build: PI_Release-Main-CI-6794
- Application Area: Reinsurance
