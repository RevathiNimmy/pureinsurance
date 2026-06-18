' WPR 64 - Commission Maintenance - Modal Dialog
Imports System.Web.Configuration.WebConfigurationManager
Imports NexusProvider
Imports Nexus.Constants
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_EditCommission : Inherits System.Web.UI.Page

        Dim oWebservice As NexusProvider.ProviderBase
        Dim oAgentCommissionTax As TaxForClaims
        Dim iInsuranceFile As Integer
        Dim sAgent As String = String.Empty
        Dim iIndex As Integer
        Dim sRiskType As String = String.Empty
        Dim sCommissionBand As String = String.Empty
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                Dim oAgentCommission As EditAgentCommission
                Session("sWarningmsg") = Session(CNCommissionWarning)
                If Request.QueryString("InsuranceFileKey") IsNot Nothing And Request.QueryString("Agent") IsNot Nothing Then

                    Try
                        iInsuranceFile = CInt(Request.QueryString("InsuranceFileKey"))
                        sAgent = CStr(Request.QueryString("Agent"))
                        iIndex = CInt(Request.QueryString("Index"))
                        If Request.QueryString("RiskType") IsNot Nothing Then
                            sRiskType = CStr(Request.QueryString("RiskType"))
                        End If
                        If Request.QueryString("CommissionBand") IsNot Nothing Then
                            sCommissionBand = CStr(Request.QueryString("CommissionBand")).Trim
                        End If
                        Dim oOverrideAgentTaxGroupAllowed As NexusProvider.OptionTypeSetting = Nothing
                        'make SAM call to check if system Option "Override Agent Tax Group Allowed" is checked
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oOverrideAgentTaxGroupAllowed = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5081)

                        If String.IsNullOrEmpty(oOverrideAgentTaxGroupAllowed.OptionValue) = False AndAlso oOverrideAgentTaxGroupAllowed.OptionValue = "1" Then
                            'Tax Group and Tax Values fields should be enabled
                            txtTaxValue.Enabled = True
                            ddlTaxGroup.Enabled = True
                        Else
                            txtTaxValue.Enabled = False
                            ddlTaxGroup.Enabled = False
                        End If

                        'make SAM call to get the Agent Commission and set on appropriate fields
                        oAgentCommission = oWebservice.GetAgentCommission(iInsuranceFile, v_sRiskType:=sRiskType, v_sCommissionBand:=sCommissionBand)

                        If oAgentCommission IsNot Nothing Then
                            With oAgentCommission
                                Dim oSelectAgentCommission As AgentCommission = .AgentCommission(iIndex)
                                ltAgentValue.Text = oSelectAgentCommission.Agent
                                ltAgentTypeValue.Text = oSelectAgentCommission.AgentType
                                ltCommissionBandValue.Text = oSelectAgentCommission.CommissionBand
                                ltPremiumValue.Text = oSelectAgentCommission.Premium
                                ltRiskTypeValue.Text = oSelectAgentCommission.RiskType
                                txtCommission.Text = oSelectAgentCommission.CommissionRate
                                hdCommissionRate.Value = oSelectAgentCommission.CommissionRate
                                txtCommissionValue.Text = Format((oSelectAgentCommission.CommissionValue), "#0.00")
                                hdCommissionValue.Value = oSelectAgentCommission.CommissionValue
                                ddlTaxGroup.Value = oSelectAgentCommission.TaxGroup
                                txtTaxValue.Text = Format((oSelectAgentCommission.TaxValue), "#0.00")
                                txthiddenIsValue.Text = oSelectAgentCommission.IsValue
                                txtOldTaxValue.Text = txtTaxValue.Text
                                txtOverride.Text = oSelectAgentCommission.OverRideReason

                                hdCommissionRate.Value = oSelectAgentCommission.CommissionRate
                                hdnOldCommission.Value = txtCommission.Text
                                hdnOldTaxValue.Value = txtTaxValue.Text
                                hdCommissionValue.Value = oSelectAgentCommission.CommissionValue

                                If Session("OldCommission") Is Nothing Then
                                    Session("OldCommission") = hdnOldCommission.Value
                                End If
                                If Session("OldTaxValue") Is Nothing Then
                                    Session("OldTaxValue") = hdnOldTaxValue.Value
                                End If

                                If oSelectAgentCommission.IsValue Then
                                    'To set the Focus
                                    Page.SetFocus(txtCommissionValue)
                                Else
                                    'To set the Focus
                                    Page.SetFocus(txtCommission)

                                End If
                                If Session(CNAgentCommissions) IsNot Nothing Then
                                    Dim oOriginalAgentCommcol As New NexusProvider.AgentCommissionsCollection
                                    oOriginalAgentCommcol = CType(Session(CNAgentCommissions), NexusProvider.AgentCommissionsCollection)
                                    If oOriginalAgentCommcol.Count > 0 Then
                                        For icnt As Integer = 0 To oOriginalAgentCommcol.Count - 1
                                            If oOriginalAgentCommcol(icnt).AgentCode = sAgent Then
                                                hdnOCommissionValue.Value = oOriginalAgentCommcol(icnt).CommissionValue
                                                hdnOCommissionRate.Value = oOriginalAgentCommcol(icnt).CommissionRate
                                                hdnOTaxValue.Value = oOriginalAgentCommcol(icnt).TaxValue
                                                hdnOTaxGroup.Value = oOriginalAgentCommcol(icnt).TaxGroup
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        hdnOCommissionValue.Value = oSelectAgentCommission.CommissionValue
                                        hdnOCommissionRate.Value = oSelectAgentCommission.CommissionRate
                                        hdnOTaxValue.Value = oSelectAgentCommission.TaxValue
                                        hdnOTaxGroup.Value = oSelectAgentCommission.TaxGroup
                                    End If
                                Else
                                    hdnOCommissionValue.Value = oSelectAgentCommission.CommissionValue
                                    hdnOCommissionRate.Value = oSelectAgentCommission.CommissionRate
                                    hdnOTaxValue.Value = oSelectAgentCommission.TaxValue
                                    hdnOTaxGroup.Value = oSelectAgentCommission.TaxGroup
                                End If
                            End With
                            Session(CNAgentCommission) = oAgentCommission
                        End If
                    Finally
                        oWebservice = Nothing
                    End Try

                    txtCommission.Attributes.Add("OnBlur", "javascript:return CalculateCommission(this,'" + txtCommissionValue.ClientID + "','" + ltPremiumValue.Text + "','" + "Commission')")
                    txtCommissionValue.Attributes.Add("OnBlur", "javascript:return CalculateCommission(this,'" + txtCommission.ClientID + "','" + ltPremiumValue.Text + "','" + "CommissionValue')")
                End If
                If hdnIsSuppressDecimals.Value Is Nothing OrElse Trim(hdnIsSuppressDecimals.Value) = "" Then
                    Dim oWebService As NexusProvider.ProviderBase = Nothing
                    Dim oSuppressDecimalOptionType As New NexusProvider.OptionTypeSetting
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oSuppressDecimalOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.SuppressDecimalValues)
                    If oSuppressDecimalOptionType IsNot Nothing Then
                        hdnIsSuppressDecimals.Value = oSuppressDecimalOptionType.OptionValue
                        If Trim(hdnIsSuppressDecimals.Value) = "1" Then
                            txtCommissionValue.Attributes.Add("onpaste", "javascript:return false;")
                            txtTaxValue.Attributes.Add("onpaste", "javascript:return false;")
                        End If


                    End If
                End If
            End If
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Private Function CalculateCommission(ByVal rate As Double, ByVal value As Double, ByVal premium As Double, ByVal IsValue As Boolean) As Double
            Dim commission As Double
            If IsValue Then
                If hdnIsSuppressDecimals.Value = "1" Then
                    commission = Math.Round(((100 * value / premium) * 100) / 100, 0, MidpointRounding.AwayFromZero)
                Else
                    commission = Math.Round((100 * value / premium) * 100) / 100
                End If

            Else
                If hdnIsSuppressDecimals.Value = "1" Then
                    commission = Math.Round(((premium * rate / 100) * 100) / 100, 0, MidpointRounding.AwayFromZero)
                Else
                    commission = Math.Round((premium * rate / 100) * 100) / 100
                End If

            End If
            Return commission
        End Function
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                Dim oAgentCommission As EditAgentCommission
                oWebservice = New NexusProvider.ProviderManager().Provider
                oAgentCommission = DirectCast(Session(CNAgentCommission), EditAgentCommission)
                iInsuranceFile = CInt(Request.QueryString("InsuranceFileKey"))
                sAgent = CStr(Request.QueryString("Agent"))
                iIndex = CInt(Request.QueryString("Index"))
                txtCommission.Text = hdCommissionRate.Value
                txtCommissionValue.Text = hdCommissionValue.Value
                Dim sbVar As StringBuilder = New StringBuilder
                With oAgentCommission
                    .InsuranceFileKey = iInsuranceFile
                    .LeadAgentNet = oAgentCommission.LeadAgentNet
                    .LeadAgentTotalCommission = oAgentCommission.LeadAgentTotalCommission
                    .LeadAgentTotalTax = oAgentCommission.LeadAgentTotalTax
                    .SubAgentNet = oAgentCommission.SubAgentNet
                    .SubAgentTotalCommission = oAgentCommission.SubAgentTotalCommission
                    .SubAgentTotalTax = oAgentCommission.SubAgentTotalTax

                    If .AgentCommission(iIndex).Agent IsNot Nothing AndAlso sAgent IsNot Nothing AndAlso .AgentCommission(iIndex).Agent.Trim.ToUpper = sAgent.Trim.ToUpper Then
                        If .AgentCommission(iIndex).MaximumRate <> 0.0 Then
                            If .AgentCommission(iIndex).IsValue = False And .AgentCommission(iIndex).MaximumRate < CDbl(hdCommissionRate.Value) Then
                                vldMaximumCommission.Enabled = True
                                vldMaximumCommission.IsValid = False
                                vldMaximumCommission.ErrorMessage = GetLocalResourceObject("lbl_vldMaximumCommission") + .AgentCommission(iIndex).MaximumRate.ToString()
                                vldMaximumCommission.ErrorMessage += sbVar.Append(".00%").ToString
                                txtCommission.Text = .AgentCommission(iIndex).MaximumRate
                                hdCommissionRate.Value = .AgentCommission(iIndex).MaximumRate
                                hdCommissionValue.Value = CalculateCommission(CDbl(txtCommission.Text), CDbl(txtCommissionValue.Text), .AgentCommission(iIndex).Premium, .AgentCommission(iIndex).IsValue)
                                txtCommissionValue.Text = hdCommissionValue.Value
                                Exit Try
                            ElseIf .AgentCommission(iIndex).IsValue = True And .AgentCommission(iIndex).MaximumRate < CDbl(hdCommissionValue.Value) Then
                                vldMaximumCommission.Enabled = True
                                vldMaximumCommission.IsValid = False
                                vldMaximumCommission.ErrorMessage = GetLocalResourceObject("lbl_vldMaximumCommissionValue") + .AgentCommission(iIndex).MaximumRate.ToString()
                                vldMaximumCommission.ErrorMessage += sbVar.Append(".00%").ToString
                                txtCommissionValue.Text = .AgentCommission(iIndex).MaximumRate
                                hdCommissionValue.Value = .AgentCommission(iIndex).MaximumRate
                                hdCommissionRate.Value = CalculateCommission(CDbl(txtCommission.Text), CDbl(txtCommissionValue.Text), .AgentCommission(iIndex).Premium, .AgentCommission(iIndex).IsValue)
                                txtCommission.Text = hdCommissionRate.Value
                                Exit Try
                            End If
                        End If
                        If Math.Abs(.AgentCommission(iIndex).Premium) < CDbl(hdCommissionValue.Value) Then
                            vldPremiumExceeded.Enabled = True
                            vldPremiumExceeded.IsValid = False
                            Exit Try
                        End If

                        If .AgentCommission(iIndex).IsValue Then
                            hdCommissionRate.Value = CalculateCommission(CDbl(txtCommission.Text), CDbl(txtCommissionValue.Text), .AgentCommission(iIndex).Premium, .AgentCommission(iIndex).IsValue)
                            txtCommission.Text = hdCommissionRate.Value
                        Else
                            hdCommissionValue.Value = CalculateCommission(CDbl(txtCommission.Text), CDbl(txtCommissionValue.Text), .AgentCommission(iIndex).Premium, .AgentCommission(iIndex).IsValue)
                            txtCommissionValue.Text = hdCommissionValue.Value
                        End If
                        .AgentCommission(iIndex).CalculatedCommissionValue = hdCommissionValue.Value
                        .AgentCommission(iIndex).CalculatedCommissionValueSpecified = True
                        If .AgentCommission(iIndex).IsValue = False Then
                            .AgentCommission(iIndex).CommissionRate = hdCommissionRate.Value
                        Else
                            .AgentCommission(iIndex).CommissionRate = hdCommissionValue.Value
                        End If
                        .AgentCommission(iIndex).CommissionValue = hdCommissionValue.Value
                        .AgentCommission(iIndex).OverRideReason = txtOverride.Text
                        .AgentCommission(iIndex).IsAmended = True
                    Else
                        .AgentCommission(iIndex).CalculatedCommissionValue = .AgentCommission(iIndex).CommissionValue
                        If .AgentCommission(iIndex).IsValue Then
                            .AgentCommission(iIndex).CommissionRate = .AgentCommission(iIndex).CommissionValue
                        End If
                        .AgentCommission(iIndex).CalculatedCommissionValueSpecified = True
                    End If

                    'Update the changed(override) Tax Value
                    'make SAM call to check if system Option "Override Agent Tax Group Allowed" is checked
                    Dim oOverrideAgentTaxGroupAllowed As NexusProvider.OptionTypeSetting = Nothing
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    oOverrideAgentTaxGroupAllowed = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5081)

                    If String.IsNullOrEmpty(oOverrideAgentTaxGroupAllowed.OptionValue) = False AndAlso oOverrideAgentTaxGroupAllowed.OptionValue = "1" Then
                        If String.IsNullOrEmpty(ddlTaxGroup.Value) = False Then
                            .AgentCommission(iIndex).TaxGroup = ddlTaxGroup.Value
                        End If
                        .AgentCommission(iIndex).TaxValue = txtTaxValue.Text
                        .AgentCommission(iIndex).IsTaxAmended = True
                    End If
                End With

                oAgentCommission = oWebservice.UpdateAgentCommission(oAgentCommission)
                Session(CNAgentCommission) = oAgentCommission
                'Close the thickbox and update the collection

                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                Session(CNCommissionWarning) = Session("sWarningmsg")

                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','RefreshAgentCommission');", True)

                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            Catch ex As NexusException
                vldMaximumCommission.Enabled = True
                vldMaximumCommission.IsValid = False
            End Try
        End Sub

        Protected Sub ddlTaxGroup_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxGroup.SelectedIndexChange
            'ReCalculate the TaxValue based on the change
            Dim oAgentCommission As EditAgentCommission
            Dim sTaxgroup As String = Nothing
            If String.IsNullOrEmpty(ddlTaxGroup.Value) Or String.IsNullOrEmpty(txtCommissionValue.Text) Or _
             txtCommissionValue.Text.Trim = "0" Then
                txtTaxValue.Text = "0.00"
            Else
                iInsuranceFile = CInt(Request.QueryString("InsuranceFileKey"))
                sAgent = CStr(Request.QueryString("Agent"))
                iIndex = CInt(Request.QueryString("Index"))
                oAgentCommission = DirectCast(Session(CNAgentCommission), EditAgentCommission)
                oWebservice = New NexusProvider.ProviderManager().Provider
                oAgentCommissionTax = oWebservice.GetAgentCommissionTax(iInsuranceFile, CDbl(txtCommissionValue.Text), Session(CNCurrenyCode), ddlTaxGroup.Value)
                txtTaxValue.Text = Format(Val(oAgentCommissionTax.TaxCurrencyAmount), "#0.00")
                hdnamdTaxValue.Value = txtTaxValue.Text
                hdnNTaxGroup.Value = ddlTaxGroup.Value
                sTaxgroup = hdnNTaxGroup.Value
                CheckIsTaxCommisionAmend("TaxgroupAmend")
            End If

            ddlTaxGroup.Attributes.Add("onpropertyChange", "javascript:CheckTaxAmendanother('" & Trim(sTaxgroup) & "')")
        End Sub

        Protected Sub txtCommissionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCommissionValue.TextChanged
            'ReCalculate the TaxValue based on the change
            Dim oAgentCommission As EditAgentCommission
            If String.IsNullOrEmpty(ddlTaxGroup.Value) = False Then
                Dim dcommissionValue, dPremium, dCommissionRate As Double
                dcommissionValue = 0
                dPremium = 0
                dCommissionRate = 0
                If CDbl(txtCommission.Text) <> 0 AndAlso txtCommission.Text.Trim <> "Infinity" Then
                    dCommissionRate = CDbl(txtCommission.Text)
                End If

                If CDbl(txtCommissionValue.Text) <> 0 AndAlso txtCommissionValue.Text.Trim <> "Infinity" Then
                    dcommissionValue = CDbl(txtCommissionValue.Text)
                    hdnCommissionValue.Value = CDbl(txtCommissionValue.Text)
                End If

                iInsuranceFile = CInt(Request.QueryString("InsuranceFileKey"))
                sAgent = CStr(Request.QueryString("Agent"))
                iIndex = CInt(Request.QueryString("Index"))
                oAgentCommission = DirectCast(Session(CNAgentCommission), EditAgentCommission)

                oWebservice = New NexusProvider.ProviderManager().Provider
                If dcommissionValue <> 0 Then
                    oAgentCommissionTax = oWebservice.GetAgentCommissionTax(iInsuranceFile, dcommissionValue, Session(CNCurrenyCode), ddlTaxGroup.Value)
                    txtTaxValue.Text = oAgentCommissionTax.TaxCurrencyAmount.ToString("F2")
                    hdnOldTaxValue.Value = txtTaxValue.Text
                Else
                    txtTaxValue.Text = dcommissionValue.ToString("F2")
                    hdnOldTaxValue.Value = txtTaxValue.Text
                End If
                CheckIsTaxCommisionAmend("CommissionAmend")
            End If
        End Sub

        Protected Sub txtCommission_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCommission.TextChanged
            'ReCalculate the TaxValue based on the change
            Dim oAgentCommission As EditAgentCommission
            If (txtCommission.Text.Trim() = "") Then
                txtCommission.Text = 0
            End If
            If CInt(txtCommission.Text.Trim()) = 0 Then
                txtTaxValue.Text = "0.00"
            End If
            If String.IsNullOrEmpty(ddlTaxGroup.Value) = False Then
                Dim dcommissionValue, dPremium, dCommissionRate As Double
                dcommissionValue = 0
                dPremium = 0
                dCommissionRate = 0
                dPremium = CDbl(ltPremiumValue.Text)

                If CDbl(txtCommission.Text) <> 0 AndAlso txtCommission.Text.Trim <> "Infinity" Then
                    dCommissionRate = CDbl(txtCommission.Text)
                End If

                If CDbl(txtCommissionValue.Text) <> 0 AndAlso txtCommissionValue.Text.Trim <> "Infinity" Then
                    dcommissionValue = CDbl(txtCommissionValue.Text)
                End If

                If dPremium <> 0 Then

                    'Round off up to zero decimals if Suppress Decimal is ON.
                    If Trim(hdnIsSuppressDecimals.Value) = "1" Then
                        dcommissionValue = Math.Round(dcommissionValue, 0, MidpointRounding.AwayFromZero)
                    Else
                        dcommissionValue = Math.Round((dPremium * dCommissionRate / 100) * 100) / 100
                    End If
                End If

            

                hdCommissionRate.Value = dCommissionRate
                hdCommissionValue.Value = dcommissionValue
                iInsuranceFile = CInt(Request.QueryString("InsuranceFileKey"))
                sAgent = CStr(Request.QueryString("Agent"))
                iIndex = CInt(Request.QueryString("Index"))
                oAgentCommission = DirectCast(Session(CNAgentCommission), EditAgentCommission)

                oWebservice = New NexusProvider.ProviderManager().Provider
                If dcommissionValue <> 0 Then
                    oAgentCommissionTax = oWebservice.GetAgentCommissionTax(iInsuranceFile, dcommissionValue, Session(CNCurrenyCode), ddlTaxGroup.Value)
                    txtTaxValue.Text = oAgentCommissionTax.TaxCurrencyAmount.ToString("F2")
                End If
                txtCommissionValue.Text = Format(dcommissionValue, "#0.00")
                CheckIsTaxCommisionAmend("CommissionAmend")
            End If
        End Sub

        Protected Sub txtTaxValue_TextChanged(sender As Object, e As EventArgs) Handles txtTaxValue.TextChanged
            CheckIsTaxCommisionAmend("TaxAmend")
        End Sub
        ''' <summary>
        ''' check for warning message for Tax and Commission ammendent
        ''' </summary>
        ''' <param name="sname"></param>
        ''' <remarks></remarks>
        Private Sub CheckIsTaxCommisionAmend(ByVal sname As String)
            sAgent = CStr(Request.QueryString("Agent"))
            If Session(CNAgentCommissions) IsNot Nothing Then
                Dim oOriginalAgentCommcol As New NexusProvider.AgentCommissionsCollection
                oOriginalAgentCommcol = CType(Session(CNAgentCommissions), NexusProvider.AgentCommissionsCollection)
                Dim sWarningString As String = ""
                For icnt As Integer = 0 To oOriginalAgentCommcol.Count - 1
                    If oOriginalAgentCommcol(icnt).AgentCode = sAgent Then
                        hdnOCommissionValue.Value = oOriginalAgentCommcol(icnt).CommissionValue
                        hdnOCommissionRate.Value = oOriginalAgentCommcol(icnt).CommissionRate
                        hdnOTaxValue.Value = oOriginalAgentCommcol(icnt).TaxValue
                        hdnOTaxGroup.Value = oOriginalAgentCommcol(icnt).TaxGroup

                        If sname = "CommissionAmend" Then
                            If ((oOriginalAgentCommcol(icnt).CommissionValue <> Convert.ToDecimal(txtCommissionValue.Text) OrElse oOriginalAgentCommcol(icnt).CommissionRate <> Convert.ToDecimal(txtCommission.Text)) OrElse Convert.ToDecimal(txtCommission.Text) = 0 AndAlso sname = "CommissionAmend") Then
                                Session("sWarningmsg") = Session("sWarningmsg") + "CommissionAmend"
                            ElseIf oOriginalAgentCommcol(icnt).CommissionValue = Convert.ToDecimal(txtCommissionValue.Text) Then
                                If Session(CNCommissionWarning) IsNot Nothing Then
                                    If Session(CNCommissionWarning).ToString.Contains("CommissionAmend") Then
                                        Session("sWarningmsg") = Session("sWarningmsg").ToString.Replace("CommissionAmend", "")
                                    End If
                                End If
                            ElseIf oOriginalAgentCommcol(icnt).CommissionRate = Convert.ToDecimal(txtCommission.Text) Then
                                'If Session(CNCommissionWarning).ToString.Contains("CommissionAmend") Then
                                '    Session(CNCommissionWarning) = Session(CNCommissionWarning).ToString.Replace("CommissionAmend", "")
                                'End If

                            End If
                        End If
                        If sname = "TaxAmend" Then
                            If ((oOriginalAgentCommcol(icnt).TaxValue <> Convert.ToDecimal(txtTaxValue.Text) OrElse oOriginalAgentCommcol(icnt).TaxGroup = ddlTaxGroup.Text) AndAlso sname = "TaxAmend") Then
                                Session("sWarningmsg") = Session("sWarningmsg") + "TaxAmend"
                            ElseIf oOriginalAgentCommcol(icnt).TaxValue = Convert.ToDecimal(txtTaxValue.Text) Then
                                If Session(CNCommissionWarning) IsNot Nothing Then
                                    If Session(CNCommissionWarning).ToString.Contains("TaxAmend") Then
                                        Session("sWarningmsg") = Session("sWarningmsg").ToString.Replace("TaxAmend", "")
                                    End If
                                End If
                            End If

                            Exit For
                        End If
                        If sname = "TaxgroupAmend" Then
                            If oOriginalAgentCommcol(icnt).TaxGroup <> ddlTaxGroup.Value Then
                                Session("sWarningmsg") = Session("sWarningmsg") + "TaxGroupAmend"
                            ElseIf oOriginalAgentCommcol(icnt).TaxGroup = ddlTaxGroup.Value Then
                                If Session(CNCommissionWarning) IsNot Nothing Then
                                    Session("sWarningmsg") = Session("sWarningmsg").ToString.Replace("TaxGroupAmend", "")
                                End If
                            End If
                        End If
                    End If

                Next

            End If
        End Sub
    End Class
End Namespace
