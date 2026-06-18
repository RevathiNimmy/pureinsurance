# RI Bug #39513 — Lessons Learned

## Bug Summary
Tax for Obligatory not calculated, and for retained and manual Quota share calculated incorrectly on RI screen in pure portal.

## Key Discoveries

### 1. Tax is purely a portal-layer concern
- No stored procedure calculates tax
- `spu_RI_Arrangement_calc` only computes premium_value, commission_value, sum_insured
- Tax is calculated by SAM `GetRITreatyPartyDetailsWithTax` and stored to DB on save
- On load, tax comes from `premium_tax` column in `ri_arrangement_line`

### 2. PartyKey is always 0 for treaty nodes
- The RI XML has `PartyKey="0"` for all treaty lines
- SAM resolves the party internally from `TreatyCode`/`TreatyID`
- `CalculateRITax` (which needs PartyKey > 0) CANNOT be used
- Must use `GetRITreatyPartyDetailsWithTax` instead

### 3. Order-of-operations is critical
- `GetTaxPercentage(node, "Premium", "Tax")` reads the node's current attributes
- If Premium was already overwritten with a new value but Tax still holds old value → wrong ratio
- ALWAYS derive the rate BEFORE overwriting the base value

### 4. ManuallyAdded="False" is NOT the same as ManuallyAdded not existing
- The attribute exists on ALL nodes, even when "False"
- `If oNode.Attributes("ManuallyAdded") IsNot Nothing` is TRUE for ALL nodes
- Must parse the boolean value with `Boolean.TryParse`

### 5. Tax only existed for treaties that were previously user-edited
- On NB first setup, all treaty nodes start with `premium_tax=0` in DB
- Tax was only calculated when a user happened to edit a field (Premium, ThisPerc etc.)
- Obligatory treaties were never user-edited (auto-calculated) → tax stayed 0 forever

### 6. CalculateObligatoryTax must run after EVERY Recalculate
- Not just on page load — every field edit triggers Recalculate which can zero tax
- Must be called after txtThisPerc, txtSumInsured, txtCommissionPerc, txtPremium, txtFACPropPremiumPerc

### 7. SAM returns amounts, not percentages
- `GetRITreatyPartyDetailsWithTax` returns `PremiumTax = 131.25` (not 10%)
- Portal writes the amount directly to the XML `Tax` attribute
- For proportional preservation, the rate must be derived: `rate = tax_amount / premium * 100`

## Debugging Approach That Worked

1. Get the RI XML (from session/debug) — see exact attribute values
2. Check the DB (`ri_arrangement_line`) — compare what's stored vs displayed
3. Trace the code path: which handler → which Recalculate path → which tax derivation
4. Verify PartyKey value — determines which SAM method can be used
5. Check ManuallyAdded, IsEditedDB, IsPremiumEdited flags — determines code path

## Files Modified

| File | What was fixed |
|------|---------------|
| `Reinsurance2007.ascx.vb` | Added CalculateObligatoryTax, fixed txtPremium ManuallyAdded check, added call sites |
| `Reinsurance.vb` | Fixed order-of-operations in CalculateFACQSH and IsEditedDB block |
