# Tasks — Bug #39513 (Issue 1): RI Tax Calculation Fix

## Task 1: Fix RI tax calculation across portal and calculation engine

- **ADO ID**: 39513
- **Priority**: 1
- **Status**: Done
- **Files Modified**:
  - `Web Portal/Nexus/Pure.Portals/Controls/Reinsurance2007.ascx.vb`
  - `Web Portal/Nexus/Nexus.Reinsurance/Reinsurance.vb`
- **Changes**:
  1. Added `CalculateObligatoryTax()` method using `GetRITreatyPartyDetailsWithTax` for all treaty nodes with Tax=0 or CommissionTax=0
  2. Called `CalculateObligatoryTax()` after every Recalculate (page load + all field edit handlers)
  3. Fixed order-of-operations in `CalculateFACQSH` — derive tax rates BEFORE overwriting Premium
  4. Fixed order-of-operations in Recalculate IsEditedDB block — derive CommissionTax rate BEFORE overwriting Commission
  5. Fixed `txtPremium_TextChanged` — parse ManuallyAdded boolean value before branching; preserve tax proportionally for non-manual treaties
