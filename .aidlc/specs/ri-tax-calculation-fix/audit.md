# Audit — Bug #39513: RI Tax Calculation Fix

## 2026-06-10 / 2026-06-11

### Investigation Timeline

1. **Attempt 1**: Fix in `Reinsurance.vb` — sibling-based tax fallback in CalculateFACQSH. **Reverted** — wrong approach (would apply tax from different party's treaty).

2. **Attempt 2**: Call `CalculateRITax` (SAM) for obligatory nodes. **Failed** — `PartyKey=0` in XML; CalculateRITax requires PartyKey > 0.

3. **Root cause from XML analysis**: All treaty nodes have `PartyKey=0`. Tax must use `GetRITreatyPartyDetailsWithTax` which resolves party from TreatyCode/TreatyID.

4. **DB verification** (rdsql2025/CMMAINDEV): Confirmed `premium_tax=131.25` exists in DB for obligatory line. Issue is portal `Recalculate` resets it to 0 on page load.

5. **Widened scope**: Not just obligatory — ALL treaty nodes have Tax=0 on first setup. Surplus only had tax because someone previously edited it.

6. **txtPremium_TextChanged bug**: `ManuallyAdded` attribute exists on all nodes (even "False"). Code entered the block for all nodes, called GetTaxPercentage which returned 0 for PartyKey=0 non-manual → overwrote Tax to 0.

7. **CommissionTax bug**: In Recalculate IsEditedDB block, Commission was overwritten before CommissionTax rate was derived → wrong ratio → wrong CommissionTax.

8. **CalculateObligatoryTax skip logic**: Was skipping nodes where Tax≠0, even if CommissionTax was wrong. Fixed to only skip when Tax≠0 AND Commission=0.

9. **Portal pre-calculation removed**: Pre-setting CommissionTax in txtCommissionPerc before Recalculate caused mismatch (new CommTax vs old Commission → wrong ratio in Recalculate).

### Final Fix Summary

**`Reinsurance2007.ascx.vb`**:
- `CalculateObligatoryTax()` — calls SAM for all nodes with Tax=0 or CommissionTax=0
- Called after every Recalculate (6 call sites)
- `txtPremium_TextChanged` — parse ManuallyAdded value; preserve tax proportionally for non-manual

**`Reinsurance.vb`**:
- `CalculateFACQSH` — derive tax rates BEFORE overwriting Premium
- Recalculate IsEditedDB block — derive CommissionTax rate BEFORE overwriting Commission

### Completed — Task 1
- **Agent**: Amazon Q
- **Commit**: `54eb11c12b` (latest amend)
- **Files Changed**:
  - `Web Portal/Nexus/Pure.Portals/Controls/Reinsurance2007.ascx.vb`
  - `Web Portal/Nexus/Nexus.Reinsurance/Reinsurance.vb`
