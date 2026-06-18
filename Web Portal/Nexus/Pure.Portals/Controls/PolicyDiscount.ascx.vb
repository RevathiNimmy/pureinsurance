Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Controls_PolicyDiscount
        Inherits System.Web.UI.UserControl

        ' Raised when Apply Discount is clicked — host page wires this up
        Public Event ApplyDiscountClicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Public Property Enabled As Boolean
            Get
                Return pnlDiscountCheckbox.Enabled
            End Get
            Set(value As Boolean)
                pnlDiscountCheckbox.Enabled = value
                pnlDiscountFrame.Enabled = value
            End Set
        End Property

        Private ReadOnly oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

        ''' <summary>
        ''' Page_Init: load dropdown items here so they exist BEFORE ViewState is restored.
        ''' This is the key fix — ASP.NET restores the selected value from ViewState during
        ''' LoadViewState which happens after Init but before Load. If items aren't populated
        ''' by then, the selected value is lost.
        ''' </summary>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            LoadDiscountReasonLookup()
            LoadRecurringTypeLookup()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' InitialiseControl is no longer called here automatically.
            ' The host page (PremiumDisplay.aspx.vb) calls InitialiseControl explicitly
            ' with the transaction type parameter.
        End Sub

        Public Sub InitialiseControl(Optional ByVal v_sTransactionType As String = "NB")
            Dim oOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.PolicyDiscount)

            Dim bDiscountEnabled As Boolean = (oOption IsNot Nothing AndAlso oOption.OptionValue = "1")
            Session(CNPolicyDiscountEnabled) = bDiscountEnabled

            pnlDiscountCheckbox.Visible = bDiscountEnabled

            If Not bDiscountEnabled Then
                ' AC: System option OFF must not remove visibility of a previously applied discount.
                ' Read from DB (not session) so a fresh page load after navigation still shows it.
                Dim oQuote As NexusProvider.Quote = TryCast(Session(CNQuote), NexusProvider.Quote)
                If oQuote IsNot Nothing Then
                    Dim oDiscount As NexusProvider.PolicyDiscount = Nothing
                    Try
                        oDiscount = oWebService.GetPolicyDiscountInfo(oQuote.InsuranceFileKey)
                    Catch ex As Exception
                        ' Swallow — no discount to show
                    End Try
                    If oDiscount IsNot Nothing AndAlso oDiscount.IsDiscountApplied Then
                        pnlDiscountCheckbox.Visible = True
                        chkPolicyDiscount.Checked = True
                        ShowDiscountFrame(oDiscount)
                        SaveDiscountToSession(oDiscount)
                        SetFieldsEnabled(False)
                        btnApplyDiscount.Enabled = False
                    End If
                End If
                Return
            End If

            ' Transaction-type-aware branching
            If v_sTransactionType = "CANCELLATION" OrElse
               (Session(CNMTAType) IsNot Nothing AndAlso CInt(Session(CNMTAType)) = MTAType.CANCELLATION) Then
                InitialiseCancellationMode()
                Return
            End If

            ' MTA with carried discount — check session first, then fall back to insurance_file
            If Session(CNMTAType) IsNot Nothing AndAlso
               CInt(Session(CNMTAType)) <> MTAType.CANCELLATION Then

                ' AC: If Recurring = "This Transaction" (1), all fields read-only, no discount can be applied
                Dim iOrigRecurringType As Integer = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
                If iOrigRecurringType = 1 Then
                    InitialiseMtaThisTransactionMode()
                    Return
                End If

                If CBool(If(Session(CNPolicyDiscountApplied), False)) Then
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    Dim dPct As Double = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))

                    If Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") IsNot Nothing AndAlso CDec(Session("POLICY_DISCOUNT_ORIGINAL_TOTAL")) <> 0D Then
                        ' Base already known from a previous load in this session.
                        ' Use session discounted premium as-is — do NOT fetch DB or recalculate
                        ' or we compound the percentage on already-discounted values.
                        Dim crDiscounted As Decimal = CDec(If(Session(CNPolicyDiscountedPremium), 0D))
                        Dim crOriginal As Decimal = CDec(Session("POLICY_DISCOUNT_ORIGINAL_TOTAL"))

                        Dim oAppliedDiscount As New NexusProvider.PolicyDiscount()
                        oAppliedDiscount.DiscountReasonId = CInt(If(Session(CNPolicyDiscountReasonId), 0))
                        oAppliedDiscount.DiscountPercentage = dPct
                        oAppliedDiscount.DiscountedPremium = crDiscounted
                        oAppliedDiscount.TotalPremium = crOriginal
                        oAppliedDiscount.RecurringTypeId = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
                        oAppliedDiscount.IsDiscountApplied = True

                        pnlDiscountCheckbox.Visible = True
                        chkPolicyDiscount.Checked = True
                        ShowDiscountFrame(oAppliedDiscount)
                        ' AC: If discount was rolled back (Edit Risk), needs re-apply — enable Apply button
                        If Not CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False)) Then
                            btnApplyDiscount.Enabled = True
                        Else
                            btnApplyDiscount.Enabled = False
                        End If
                        Return
                    Else
                        ' First load or after requote — use session total premium set by mtareason.
                        ' Do NOT call GetPolicyDiscountTotalPremium here — it reads insurance_file.this_premium
                        ' which may not be updated yet after rating (risk grid shows correct value but
                        ' insurance_file.this_premium lags behind). Session value is always current.
                        Dim crBase As Decimal = 0D
                        If crBase = 0D Then
                            Try
                                crBase = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                            Catch
                            End Try
                        End If
                        Dim crDiscounted As Decimal = Math.Round(crBase * CDec(1 + dPct / 100), 2)
                        Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = crBase

                        Dim oMtaDiscount As New NexusProvider.PolicyDiscount()
                        oMtaDiscount.DiscountReasonId = CInt(If(Session(CNPolicyDiscountReasonId), 0))
                        oMtaDiscount.DiscountPercentage = dPct
                        oMtaDiscount.DiscountedPremium = crDiscounted
                        oMtaDiscount.TotalPremium = crBase
                        oMtaDiscount.RecurringTypeId = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
                        oMtaDiscount.IsDiscountApplied = True
                        SaveDiscountToSession(oMtaDiscount)

                        pnlDiscountCheckbox.Visible = True
                        chkPolicyDiscount.Checked = True
                        ShowDiscountFrame(oMtaDiscount)
                        ' AC: If discount was rolled back (Edit Risk), needs re-apply — enable Apply button
                        If Not CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False)) AndAlso dPct <> 0 Then
                            btnApplyDiscount.Enabled = True
                        Else
                            btnApplyDiscount.Enabled = (dPct <> 0)
                        End If
                        Return
                    End If
                Else
                    ' Session not set — check insurance_file for carried discount
                    Dim oQuote As NexusProvider.Quote = TryCast(Session(CNQuote), NexusProvider.Quote)
                    If oQuote IsNot Nothing Then
                        LoadExistingDiscount(oQuote.InsuranceFileKey)
                    End If
                    Return
                End If
            End If

            ' Renewal — handle carried discount based on recurring type
            If v_sTransactionType = "REN" Then
                InitialiseRenewalMode()
                Return
            End If

            ' NB — existing behaviour (unchanged, falls through to existing code)
            Dim oSessionDiscount As NexusProvider.PolicyDiscount = GetCurrentDiscountFromSession()
            If oSessionDiscount IsNot Nothing AndAlso oSessionDiscount.IsDiscountApplied Then
                chkPolicyDiscount.Checked = True
                ShowDiscountFrame(oSessionDiscount)
                ' AC: If discount was rolled back (Add/Edit Risk), needs re-apply — enable Apply button
                If Not CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False)) Then
                    btnApplyDiscount.Enabled = True
                End If
            End If
        End Sub

        Private Sub LoadDiscountReasonLookup()
            Dim oReasons As NexusProvider.LookupListCollection = oWebService.GetList(
                NexusProvider.ListType.PMLookup, "Discount_Reason", True, True)

            ddlDiscountReason.Items.Clear()
            ddlDiscountReason.Items.Add(New System.Web.UI.WebControls.ListItem("(None)", "0"))
            If oReasons IsNot Nothing Then
                For Each oItem As NexusProvider.LookupListItem In oReasons
                    ddlDiscountReason.Items.Add(New System.Web.UI.WebControls.ListItem(oItem.Description, oItem.Key.ToString()))
                Next
            End If
        End Sub

        Private Sub LoadRecurringTypeLookup()
            Dim oTypes As NexusProvider.LookupListCollection = oWebService.GetList(
                NexusProvider.ListType.PMLookup, "Discount_Recurring_Type", True, True)

            ddlRecurring.Items.Clear()
            ddlRecurring.Items.Add(New System.Web.UI.WebControls.ListItem("(None)", "0"))
            If oTypes IsNot Nothing Then
                For Each oItem As NexusProvider.LookupListItem In oTypes
                    ddlRecurring.Items.Add(New System.Web.UI.WebControls.ListItem(oItem.Description, oItem.Key.ToString()))
                Next
            End If
        End Sub

        Protected Sub chkPolicyDiscount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPolicyDiscount.CheckedChanged
            If chkPolicyDiscount.Checked Then
                ' AC: If discount was already applied, restore session values into the frame
                ' instead of fetching fresh from DB and resetting everything to zero.

                Dim oQuote As NexusProvider.Quote = TryCast(Session(CNQuote), NexusProvider.Quote)
                If oQuote Is Nothing Then Return
                Dim crTotalPremium As Decimal = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)

                If CBool(If(Session(CNPolicyDiscountApplied), False)) Then
                    Dim oApplied As New NexusProvider.PolicyDiscount()
                    oApplied.DiscountReasonId = CInt(If(Session(CNPolicyDiscountReasonId), 0))
                    oApplied.DiscountPercentage = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))
                    oApplied.TotalPremium = crTotalPremium
                    oApplied.DiscountedPremium = crTotalPremium
                    oApplied.RecurringTypeId = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
                    oApplied.IsDiscountApplied = True
                    ShowDiscountFrame(oApplied)
                    btnApplyDiscount.Enabled = False
                    Return
                End If

                ' AC: Store the original undiscounted total — this is the first time the
                ' frame opens, so the server total IS the undiscounted premium.
                Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = crTotalPremium

                Dim oDiscount As New NexusProvider.PolicyDiscount()
                oDiscount.TotalPremium = crTotalPremium
                oDiscount.DiscountedPremium = crTotalPremium
                oDiscount.DiscountPercentage = 0.0
                oDiscount.DiscountReasonId = 0
                oDiscount.RecurringTypeId = 0

                ShowDiscountFrame(oDiscount)
                SaveDiscountToSession(oDiscount)
                SetFieldsEnabled(False)
                btnApplyDiscount.Enabled = False
            Else
                ' AC: If discount was already applied, only hide the frame — preserve all session
                ' values so they can be restored if the user re-checks the checkbox.
                ' Always re-enable Buy/Print/Payment/docs when unchecking.
                If CBool(If(Session(CNPolicyDiscountApplied), False)) Then
                    pnlDiscountFrame.Visible = False
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableBuyOnUncheck",
                        "if (typeof policyDiscount_enableBuyAndPayment === 'function') { policyDiscount_enableBuyAndPayment(); }", True)
                    Return
                End If

                ' Discount not yet applied — safe to clear everything
                pnlDiscountFrame.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableBuyOnUncheck",
                    "if (typeof policyDiscount_enableBuyAndPayment === 'function') { policyDiscount_enableBuyAndPayment(); }", True)
                Session(CNPolicyDiscountReasonId) = 0
                Session(CNPolicyDiscountPercentage) = 0.0
                Session(CNPolicyDiscountedPremium) = 0D
                Session(CNPolicyDiscountTotalPremium) = 0D
                Session(CNPolicyDiscountRecurringTypeId) = 0
                Session(CNPolicyDiscountApplied) = False
                Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = Nothing
            End If
        End Sub

        Protected Sub ddlDiscountReason_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDiscountReason.SelectedIndexChanged
            Dim bHasReason As Boolean = (ddlDiscountReason.SelectedValue <> "0")
            Dim bDiscountAlreadyApplied As Boolean = CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False))

            ' AC: Use the stored original undiscounted total for all calculations.
            Dim crTotal As Decimal
            If Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") IsNot Nothing Then
                crTotal = CDec(Session("POLICY_DISCOUNT_ORIGINAL_TOTAL"))
            Else
                Dim oQuote As NexusProvider.Quote = TryCast(Session(CNQuote), NexusProvider.Quote)
                If oQuote Is Nothing Then Return
                crTotal = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = crTotal
            End If

            ' AC: Only reset percentage/premium fields if discount has NOT already been applied.
            ' If discount is already applied, preserve the existing field values so the user
            ' can see what was applied and the 4-field comparison works correctly.
            If Not bDiscountAlreadyApplied Then
                If Not bHasReason Then
                    txtDiscountPercentage.Text = "0.00000000"
                    txtDiscountedPremium.Text = crTotal.ToString("F2")
                    hdnTotalPremium.Value = crTotal.ToString()
                    ddlRecurring.SelectedValue = "0"
                    SetFieldsEnabled(False)
                    btnApplyDiscount.Enabled = False
                Else
                    txtDiscountedPremium.Text = crTotal.ToString("F2")
                    txtDiscountPercentage.Text = "0.00000000"
                    hdnTotalPremium.Value = crTotal.ToString()
                    SetFieldsEnabled(True)
                    btnApplyDiscount.Enabled = False
                End If
            End If

            ' AC: Compare against applied values — enable or disable Buy accordingly
            If bDiscountAlreadyApplied Then
                Dim iAppliedReason As Integer = CInt(If(hdnAppliedReasonId.Value = "", -1, hdnAppliedReasonId.Value))
                If CInt(ddlDiscountReason.SelectedValue) = iAppliedReason Then
                    EnableBuyOnClient()
                Else
                    DisableBuyOnClient()
                End If
            End If

            Dim oDiscount As NexusProvider.PolicyDiscount = GetCurrentDiscountFromSession()
            If oDiscount Is Nothing Then oDiscount = New NexusProvider.PolicyDiscount()
            oDiscount.DiscountReasonId = CInt(ddlDiscountReason.SelectedValue)
            oDiscount.TotalPremium = crTotal
            If Not bDiscountAlreadyApplied Then
                oDiscount.DiscountedPremium = crTotal
                oDiscount.DiscountPercentage = 0.0
                If Not bHasReason Then oDiscount.RecurringTypeId = 0
            End If
            SaveDiscountToSession(oDiscount)
        End Sub

        Protected Sub ddlRecurring_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRecurring.SelectedIndexChanged
            Dim oDiscount As NexusProvider.PolicyDiscount = GetCurrentDiscountFromSession()
            If oDiscount Is Nothing Then oDiscount = New NexusProvider.PolicyDiscount()
            oDiscount.RecurringTypeId = CInt(ddlRecurring.SelectedValue)
            SaveDiscountToSession(oDiscount)

            ' AC: Apply Discount only enabled when reason selected, recurring selected,
            ' AND discounted premium actually differs from total premium (i.e. % <> 0)
            Dim dPercentage As Double
            Double.TryParse(txtDiscountPercentage.Text.Trim(), dPercentage)
            If CInt(ddlDiscountReason.SelectedValue) > 0 AndAlso
               CInt(ddlRecurring.SelectedValue) > 0 AndAlso
               dPercentage <> 0 Then
                btnApplyDiscount.Enabled = True
                ' AC: If discount already applied, check if all 4 values match applied
                If CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False)) Then
                    Dim iAppliedReason As Integer = CInt(If(hdnAppliedReasonId.Value = "", -1, hdnAppliedReasonId.Value))
                    Dim iAppliedRecurring As Integer = CInt(If(hdnAppliedRecurringId.Value = "", -1, hdnAppliedRecurringId.Value))
                    If CInt(ddlDiscountReason.SelectedValue) = iAppliedReason AndAlso
                       CInt(ddlRecurring.SelectedValue) = iAppliedRecurring Then
                        EnableBuyOnClient()
                    Else
                        DisableBuyOnClient()
                    End If
                End If
            End If
        End Sub

        Protected Sub btnApplyDiscount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyDiscount.Click
            Dim dPercentage As Double
            If Not Double.TryParse(txtDiscountPercentage.Text.Trim(), dPercentage) Then Return

            Dim dPremium As Decimal
            If Not Decimal.TryParse(txtDiscountedPremium.Text.Trim(), dPremium) Then Return

            Dim oDiscount As NexusProvider.PolicyDiscount = GetCurrentDiscountFromSession()
            If oDiscount Is Nothing Then oDiscount = New NexusProvider.PolicyDiscount()
            oDiscount.DiscountReasonId = CInt(ddlDiscountReason.SelectedValue)
            oDiscount.RecurringTypeId = CInt(ddlRecurring.SelectedValue)
            oDiscount.DiscountPercentage = dPercentage
            oDiscount.DiscountedPremium = dPremium
            SaveDiscountToSession(oDiscount)

            RaiseEvent ApplyDiscountClicked(Me, e)
        End Sub

        Public Sub OnDiscountApplied()
            btnApplyDiscount.Enabled = False
            Dim oDiscount As NexusProvider.PolicyDiscount = GetCurrentDiscountFromSession()
            If oDiscount IsNot Nothing Then
                oDiscount.IsDiscountApplied = True
                Session(CNPolicyDiscountApplied) = True
                SaveDiscountToSession(oDiscount)
                ' AC: Store the applied values in hidden fields so JS can compare
                ' current field values against them to decide enable/disable of Buy etc.
                hdnAppliedPercentage.Value = oDiscount.DiscountPercentage.ToString("F8")
                hdnAppliedPremium.Value = oDiscount.DiscountedPremium.ToString("F2")
                hdnAppliedReasonId.Value = oDiscount.DiscountReasonId.ToString()
                hdnAppliedRecurringId.Value = oDiscount.RecurringTypeId.ToString()
            End If
        End Sub

        Public Sub LoadExistingDiscount(ByVal v_iInsuranceFileKey As Integer)
            Dim oDiscount As NexusProvider.PolicyDiscount = Nothing
            Try
                oDiscount = oWebService.GetPolicyDiscountInfo(v_iInsuranceFileKey)
            Catch ex As Exception
                Return
            End Try
            If oDiscount IsNot Nothing AndAlso oDiscount.IsDiscountApplied Then
                ' AC: Store the original undiscounted total for iterative discount.
                ' Back-calculate from the stored discount values.
                If oDiscount.DiscountPercentage <> 0 AndAlso oDiscount.DiscountedPremium <> 0 Then
                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = Math.Round(
                        oDiscount.DiscountedPremium / CDec(1 + oDiscount.DiscountPercentage / 100), 2)
                Else
                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = oDiscount.TotalPremium
                End If

                pnlDiscountCheckbox.Visible = True
                chkPolicyDiscount.Checked = True
                ShowDiscountFrame(oDiscount)
                SaveDiscountToSession(oDiscount)
                ' AC: Discount already applied and no changes pending — Apply stays disabled.
                ' The user must change % or premium to enable Apply (iterative discount).
                btnApplyDiscount.Enabled = False
            End If
        End Sub

        Private Sub SetFieldsEnabled(ByVal bEnabled As Boolean)
            ddlRecurring.Enabled = bEnabled
            txtDiscountPercentage.Enabled = bEnabled
            txtDiscountedPremium.Enabled = bEnabled
        End Sub

        ''' <summary>
        ''' AC: MTA with Recurring = "This Transaction" (1) — discount was one-off on NB
        ''' and does not carry forward. Show checkbox unchecked and enabled so user can
        ''' optionally apply a fresh discount. Frame stays hidden until checkbox is ticked.
        ''' </summary>
        Private Sub InitialiseMtaThisTransactionMode()
            pnlDiscountCheckbox.Visible = True
            chkPolicyDiscount.Checked = False
            chkPolicyDiscount.Enabled = True
            pnlDiscountFrame.Visible = False
        End Sub

        Private Sub InitialiseCancellationMode()
            Dim oQuote As NexusProvider.Quote = TryCast(Session(CNQuote), NexusProvider.Quote)
            If oQuote Is Nothing Then Return

            Dim oDiscount As NexusProvider.PolicyDiscount = Nothing
            Try
                oDiscount = oWebService.GetPolicyDiscountInfo(oQuote.InsuranceFileKey)
            Catch ex As Exception
                ' Log and continue — no discount to show
            End Try

            If oDiscount IsNot Nothing AndAlso oDiscount.IsDiscountApplied Then
                ' Show discount values in read-only mode
                pnlDiscountCheckbox.Visible = True
                chkPolicyDiscount.Checked = True
                chkPolicyDiscount.Enabled = False  ' Checkbox itself is disabled
                ShowDiscountFrame(oDiscount)        ' Existing method that populates fields
                SetReadOnlyMode()                   ' Disable all input fields
            Else
                ' No discount on this policy — hide everything
                pnlDiscountCheckbox.Visible = False
                pnlDiscountFrame.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' AC: Renewal discount handling based on recurring type.
        ''' Recurring = "Policy" (3): auto-populate carried values, enable iterative discount.
        ''' Recurring = "This Transaction" (1) or "This Term" (2): no discount on automatic renewal.
        ''' Manual renewal: user can add fresh discount via checkbox.
        ''' </summary>
        Private Sub InitialiseRenewalMode()
            Dim oQuote As NexusProvider.Quote = TryCast(Session(CNQuote), NexusProvider.Quote)
            If oQuote Is Nothing Then Return

            Dim iRecurringType As Integer = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))

            ' Recurring = "Policy" (3) — carry forward discount
            If iRecurringType = 3 AndAlso CBool(If(Session(CNPolicyDiscountApplied), False)) Then

                ' If discount was already applied to risks (by RenewalManager auto-apply),
                ' show session values as-is — do NOT recalculate or we double-discount.
                If CBool(If(Session("POLICY_DISCOUNT_APPLIED_TO_RISKS"), False)) Then
                    Dim oDiscount As New NexusProvider.PolicyDiscount()
                    oDiscount.DiscountReasonId = CInt(If(Session(CNPolicyDiscountReasonId), 0))
                    oDiscount.DiscountPercentage = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))
                    oDiscount.DiscountedPremium = CDec(If(Session(CNPolicyDiscountedPremium), 0D))
                    oDiscount.TotalPremium = CDec(If(Session(CNPolicyDiscountTotalPremium), 0D))
                    oDiscount.RecurringTypeId = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
                    oDiscount.IsDiscountApplied = True

                    ' Store original total for iterative discount (anti-compounding)
                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = oDiscount.TotalPremium

                    pnlDiscountCheckbox.Visible = True
                    chkPolicyDiscount.Checked = True
                    ShowDiscountFrame(oDiscount)
                    btnApplyDiscount.Enabled = False
                    Return
                End If

                ' Carried discount from session but not yet applied to renewal risks
                ' Recalculate against current renewal premium
                Dim crTotalPremium As Decimal = 0D
                Try
                    crTotalPremium = oWebService.GetPolicyDiscountTotalPremium(oQuote.InsuranceFileKey)
                Catch ex As Exception
                    crTotalPremium = CDec(If(Session(CNPolicyDiscountTotalPremium), 0D))
                End Try

                Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = crTotalPremium

                Dim dPercentage As Double = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))
                Dim crDiscountedPremium As Decimal = Math.Round(crTotalPremium * CDec(1 + dPercentage / 100), 2)

                Dim oDiscountNew As New NexusProvider.PolicyDiscount()
                oDiscountNew.DiscountReasonId = CInt(If(Session(CNPolicyDiscountReasonId), 0))
                oDiscountNew.DiscountPercentage = dPercentage
                oDiscountNew.DiscountedPremium = crDiscountedPremium
                oDiscountNew.TotalPremium = crTotalPremium
                oDiscountNew.RecurringTypeId = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
                oDiscountNew.IsDiscountApplied = True
                SaveDiscountToSession(oDiscountNew)

                pnlDiscountCheckbox.Visible = True
                chkPolicyDiscount.Checked = True
                ShowDiscountFrame(oDiscountNew)
                btnApplyDiscount.Enabled = (dPercentage <> 0)
                Return
            End If

            ' Recurring = "This Transaction" (1) or "This Term" (2) — no automatic carry
            ' AC: All fields read-only, no discount on automatic renewal.
            ' AC: On manual renewal, user can add discount via checkbox.
            If iRecurringType = 1 OrElse iRecurringType = 2 Then
                ' Show checkbox unchecked and enabled so user can optionally add fresh discount
                pnlDiscountCheckbox.Visible = True
                chkPolicyDiscount.Checked = False
                chkPolicyDiscount.Enabled = True
                pnlDiscountFrame.Visible = False
                Return
            End If

            ' No recurring type set (0) or no discount on original policy
            ' Check DB for existing discount on the renewal quote itself
            LoadExistingDiscount(oQuote.InsuranceFileKey)
        End Sub

        Private Sub SetReadOnlyMode()
            ddlDiscountReason.Enabled = False
            ddlRecurring.Enabled = False
            txtDiscountPercentage.Enabled = False
            txtDiscountedPremium.Enabled = False
            btnApplyDiscount.Enabled = False
        End Sub

        Private Sub ShowDiscountFrame(ByVal oDiscount As NexusProvider.PolicyDiscount)
            pnlDiscountFrame.Visible = True

            ' hdnTotalPremium must always hold the ORIGINAL pre-discount total so that JS
            ' calculates: newDiscountedPremium = originalTotal * (1 + newPercentage / 100)
            ' Use the stored original total from session if available (survives Apply cycles).
            ' Otherwise back-calculate from discounted premium and percentage.
            Dim crOriginalTotal As Decimal
            If Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") IsNot Nothing Then
                crOriginalTotal = CDec(Session("POLICY_DISCOUNT_ORIGINAL_TOTAL"))
            ElseIf oDiscount.DiscountPercentage <> 0 AndAlso oDiscount.DiscountedPremium <> 0 Then
                crOriginalTotal = Math.Round(oDiscount.DiscountedPremium / CDec(1 + oDiscount.DiscountPercentage / 100), 2)
            Else
                crOriginalTotal = oDiscount.TotalPremium
            End If
            hdnTotalPremium.Value = crOriginalTotal.ToString()

            txtDiscountPercentage.Text = oDiscount.DiscountPercentage.ToString("F8")
            txtDiscountedPremium.Text = oDiscount.DiscountedPremium.ToString("F2")

            If ddlDiscountReason.Items.FindByValue(oDiscount.DiscountReasonId.ToString()) IsNot Nothing Then
                ddlDiscountReason.SelectedValue = oDiscount.DiscountReasonId.ToString()
            End If

            If ddlRecurring.Items.FindByValue(oDiscount.RecurringTypeId.ToString()) IsNot Nothing Then
                ddlRecurring.SelectedValue = oDiscount.RecurringTypeId.ToString()
            End If

            Dim bHasReason As Boolean = (oDiscount.DiscountReasonId > 0)
            SetFieldsEnabled(bHasReason)

            ' AC: Populate applied hidden fields so JS can compare all 4 values
            If oDiscount.IsDiscountApplied Then
                hdnAppliedPercentage.Value = oDiscount.DiscountPercentage.ToString("F8")
                hdnAppliedPremium.Value = oDiscount.DiscountedPremium.ToString("F2")
                hdnAppliedReasonId.Value = oDiscount.DiscountReasonId.ToString()
                hdnAppliedRecurringId.Value = oDiscount.RecurringTypeId.ToString()
            Else
                hdnAppliedPercentage.Value = ""
                hdnAppliedPremium.Value = ""
                hdnAppliedReasonId.Value = ""
                hdnAppliedRecurringId.Value = ""
            End If
        End Sub

        Private Function GetCurrentDiscountFromSession() As NexusProvider.PolicyDiscount
            Dim oDiscount As New NexusProvider.PolicyDiscount()
            oDiscount.DiscountReasonId = CInt(If(Session(CNPolicyDiscountReasonId), 0))
            oDiscount.DiscountPercentage = CDbl(If(Session(CNPolicyDiscountPercentage), 0.0))
            oDiscount.DiscountedPremium = CDec(If(Session(CNPolicyDiscountedPremium), 0D))
            oDiscount.TotalPremium = CDec(If(Session(CNPolicyDiscountTotalPremium), 0D))
            oDiscount.RecurringTypeId = CInt(If(Session(CNPolicyDiscountRecurringTypeId), 0))
            oDiscount.IsDiscountApplied = CBool(If(Session(CNPolicyDiscountApplied), False))
            Return oDiscount
        End Function

        Private Sub SaveDiscountToSession(ByVal oDiscount As NexusProvider.PolicyDiscount)
            Session(CNPolicyDiscountReasonId) = oDiscount.DiscountReasonId
            Session(CNPolicyDiscountPercentage) = oDiscount.DiscountPercentage
            Session(CNPolicyDiscountedPremium) = oDiscount.DiscountedPremium
            Session(CNPolicyDiscountTotalPremium) = oDiscount.TotalPremium
            Session(CNPolicyDiscountRecurringTypeId) = oDiscount.RecurringTypeId
            Session(CNPolicyDiscountApplied) = oDiscount.IsDiscountApplied
        End Sub

        ''' <summary>
        ''' AC: Iterative discount — when user changes discount values after discount was
        ''' already applied to risks, disable Buy/Print/Payment on the host page.
        ''' Uses ScriptManager to inject client call after async postback completes.
        ''' </summary>
        Private Sub DisableBuyOnClient()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableBuyOnChange",
                "if (typeof policyDiscount_disableBuyAndPayment === 'function') { policyDiscount_disableBuyAndPayment(); }", True)
        End Sub

        Private Sub EnableBuyOnClient()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableBuyOnChange",
                "if (typeof policyDiscount_enableBuyAndPayment === 'function') { policyDiscount_enableBuyAndPayment(); }", True)
        End Sub

    End Class

End Namespace
