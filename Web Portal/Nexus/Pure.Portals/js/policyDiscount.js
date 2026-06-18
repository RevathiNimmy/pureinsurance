// Policy Discount / Loading — bidirectional calculation
// AC: Percentage supports up to 8 decimal places
// AC: Premium supports up to 2 decimal places
// AC: On tab-out, recalculation occurs and Apply Discount button state is managed

// AC: Checks if all 4 current values match the already-applied values.
// If they all match — re-enable Buy/Print/Payment/docs (discount still valid).
// If any differ, or no discount applied yet — disable them.
function policyDiscount_checkAgainstApplied(hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, currentPct, currentPrem, ddlReasonId, ddlRecurringId) {
    var hdnPct = document.getElementById(hdnAppliedPctId)
    var hdnPrem = document.getElementById(hdnAppliedPremId)
    var hdnReason = document.getElementById(hdnAppliedReasonId)
    var hdnRecurring = document.getElementById(hdnAppliedRecurringId)
    if (!hdnPct || !hdnPrem || !hdnReason || !hdnRecurring) {
        if (typeof policyDiscount_disableBuyAndPayment === 'function') policyDiscount_disableBuyAndPayment()
        return
    }
    // No discount applied yet — always disable
    if (hdnPct.value === '' || hdnPrem.value === '' || hdnReason.value === '' || hdnRecurring.value === '') {
        if (typeof policyDiscount_disableBuyAndPayment === 'function') policyDiscount_disableBuyAndPayment()
        return
    }
    var dAppliedPct = parseFloat(hdnPct.value)
    var dAppliedPrem = parseFloat(hdnPrem.value)
    var dCurrentPct = parseFloat(currentPct)
    var dCurrentPrem = parseFloat(currentPrem)
    if (isNaN(dAppliedPct) || isNaN(dAppliedPrem) || isNaN(dCurrentPct) || isNaN(dCurrentPrem)) {
        if (typeof policyDiscount_disableBuyAndPayment === 'function') policyDiscount_disableBuyAndPayment()
        return
    }
    var ddlReason = document.getElementById(ddlReasonId)
    var ddlRecurring = document.getElementById(ddlRecurringId)
    var currentReasonVal = ddlReason ? ddlReason.value : ''
    var currentRecurringVal = ddlRecurring ? ddlRecurring.value : ''
    var pctMatch = Math.abs(dCurrentPct - dAppliedPct) < 0.000000005
    var premMatch = Math.abs(dCurrentPrem - dAppliedPrem) < 0.005
    var reasonMatch = currentReasonVal === hdnReason.value
    var recurringMatch = currentRecurringVal === hdnRecurring.value
    if (pctMatch && premMatch && reasonMatch && recurringMatch) {
        if (typeof policyDiscount_enableBuyAndPayment === 'function') policyDiscount_enableBuyAndPayment()
    } else {
        if (typeof policyDiscount_disableBuyAndPayment === 'function') policyDiscount_disableBuyAndPayment()
    }
}

// Called on any input change to enable Apply Discount button immediately
function policyDiscount_enableApplyOnChange(btnApplyId) {
    var btnApply = document.getElementById(btnApplyId)
    if (btnApply) {
        btnApply.disabled = false
    }
    // Always disable Buy/Print/Payment/docs on any input change.
    // Re-enable only happens on blur after full recalculation via checkAgainstApplied.
    if (typeof policyDiscount_disableBuyAndPayment === 'function') {
        policyDiscount_disableBuyAndPayment()
    }
}

// Called on tab-out from the Percentage field
function policyDiscount_onPercentageBlur(txtPercentage, premiumClientId, hdnTotalPremiumId, btnApplyId, hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, ddlReasonId, ddlRecurringId) {
    var sPercentage = txtPercentage.value.trim()
    var dPercentage = parseFloat(sPercentage)
    var dTotalPremium = parseFloat(document.getElementById(hdnTotalPremiumId).value)

    if (isNaN(dPercentage) || isNaN(dTotalPremium)) {
        return
    }

    var txtPremium = document.getElementById(premiumClientId)
    var btnApply = document.getElementById(btnApplyId)

    txtPercentage.value = dPercentage.toFixed(8)

    if (dPercentage === 0) {
        txtPremium.value = dTotalPremium.toFixed(2)
        btnApply.disabled = true
        return
    }

    var dDiscountedPremium = dTotalPremium * (1 + dPercentage / 100)
    txtPremium.value = dDiscountedPremium.toFixed(2)
    btnApply.disabled = false

    // Full 4-field comparison after recalculation
    policyDiscount_checkAgainstApplied(hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, dPercentage.toFixed(8), dDiscountedPremium.toFixed(2), ddlReasonId, ddlRecurringId)
}

// Called on tab-out from the Discounted/Loaded Premium field
function policyDiscount_onPremiumBlur(txtPremium, percentageClientId, hdnTotalPremiumId, btnApplyId, hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, ddlReasonId, ddlRecurringId) {
    var sPremium = txtPremium.value.trim()
    var dDiscountedPremium = parseFloat(sPremium)
    var dTotalPremium = parseFloat(document.getElementById(hdnTotalPremiumId).value)

    if (isNaN(dDiscountedPremium) || isNaN(dTotalPremium) || dTotalPremium === 0) {
        return
    }

    var txtPercentage = document.getElementById(percentageClientId)
    var btnApply = document.getElementById(btnApplyId)

    txtPremium.value = dDiscountedPremium.toFixed(2)

    var dPercentage = ((dDiscountedPremium - dTotalPremium) / dTotalPremium) * 100
    txtPercentage.value = dPercentage.toFixed(8)

    if (Math.abs(dDiscountedPremium - dTotalPremium) > 0.001) {
        btnApply.disabled = false
        // Full 4-field comparison after recalculation
        policyDiscount_checkAgainstApplied(hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, dPercentage.toFixed(8), dDiscountedPremium.toFixed(2), ddlReasonId, ddlRecurringId)
    } else {
        btnApply.disabled = true
    }
}
