Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils

Namespace Nexus

    Partial Class FinancePlanDetails : Inherits CMS.Library.Frontend.clsCMSPage
        Dim nFinancePlanKey As Integer
        Dim nFinancePlanVersion As Integer
        Dim sPolicyNumber As String = ""
        Dim oBaseInstalment As New BaseInstalment
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


            nFinancePlanKey = Request.QueryString("FinancePlanKey")
            nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
            sPolicyNumber = Request.QueryString("PolicyNumber")
            Session(CNInsuranceRef) = sPolicyNumber
            If Not Page.IsPostBack Then
                If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then

                    oBaseInstalment.GetPremiumFinancePlanDetails(Nothing, nFinancePlanKey, nFinancePlanVersion, Nothing)
                End If
            End If
            If hvConfirmationResponse.Value = "true" Then
                Dim sURL As String
                If HttpContext.Current.Session.IsCookieless Then
                    sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PolicyCancel.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion)
                Else
                    sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/PolicyCancel.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion)
                End If

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                hvConfirmationResponse.Value = ""
            End If

            'Send "SaveOnly" as "True", in case of "DD Cancelled & Plan on Hold"
            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTARGUMENT") = "SetOnHold" Then
                Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    oProcessPFPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)
                End If

                oProcessPFPlan.PFPremFinanceKey = nFinancePlanKey
                oProcessPFPlan.PFPremFinanceVersion = nFinancePlanVersion

                oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, True, Nothing)
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx")
            End If
            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "RedirectFinancePlan" Then
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan")
            End If
            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "RedirectFinancePlanDetails" Then
                Session(CNInstalmentPlanMode) = InstalmentPlanType.View
                Response.Redirect(Request("__EVENTARGUMENT"))
            End If

            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "RedirectFinancePlanDetailsEdit" Then
                Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
                Response.Redirect(Request("__EVENTARGUMENT"))
            End If
            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "FindTransactions" Then
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan")
            End If

            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "CashList" Then
                Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=INS")
            End If

            Dim sDeleteConfirmationMessage As String = GetLocalResourceObject("lblDeleteMsg")
            btnDelete.Attributes.Add("onclick", "javascript:return DeleteConfirmation(""" & sDeleteConfirmationMessage & """);")

            Dim sReleaseConfirmationMessage As String = GetLocalResourceObject("lblReleaseMsg")
            btnRelease.Attributes.Add("onclick", "javascript:return ReleaseConfirmation(""" & sReleaseConfirmationMessage & """);")

            Dim sHoldConfirmationMessage As String = GetLocalResourceObject("lbl_PlanHoldConfirmation")
            Me.btnSave.Attributes.Add("onclick", "javascript:return ConfirmBeforePlanHold('" + sHoldConfirmationMessage + "');")

            If Not IsPostBack Then
                If Session(CNFinancePlanDetails) IsNot Nothing Then

                    Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan

                    oProcessPFPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)

                    EnableDisableButtons(CType(oProcessPFPlan.PremiumFinanceDetails.StatusInd, FinancePlanStatus))

                End If
            End If

            If Session(CNInstalmentPlanMode) = InstalmentPlanType.View Then
                btnDelete.Enabled = False
                btnCancelPlan.Enabled = False
                btnMTA.Enabled = False
                btnRelease.Enabled = False
                btnSettle.Enabled = False
                btnSave.Enabled = False
                btnBack.Visible = True
            ElseIf Session(CNInstalmentPlanMode) = InstalmentPlanType.edit AndAlso Request.QueryString("ShowButtons") Is Nothing Then
                btnMTA.Enabled = False
                btnSettle.Enabled = False
                btnCancelPlan.Enabled = False
            End If

            If Request.QueryString("ProcessType") IsNot Nothing AndAlso Request.QueryString("ProcessType") = "MTA" Then
                btnDelete.Enabled = False
                btnCancelPlan.Enabled = False
                btnMTA.Enabled = False
                btnRelease.Enabled = False
                btnSettle.Enabled = False
                btnSave.Enabled = True
            End If

            Dim hdnMediaTypeCode As HiddenField
            Dim rfvAccountName As RequiredFieldValidator
            Dim rfvAccountNumber As RequiredFieldValidator
            hdnMediaTypeCode = CType(fpPlanDetails.FindControl("hvMediaTypeCode"), HiddenField)
            rfvAccountName = CType(fpBankDetails.FindControl("rfvAccountName"), RequiredFieldValidator)
            rfvAccountNumber = CType(fpBankDetails.FindControl("rfvAccountNumber"), RequiredFieldValidator)

            If Trim(hdnMediaTypeCode.Value) = "DD" Then
                rfvAccountName.Enabled = True
                rfvAccountNumber.Enabled = True
            Else
                rfvAccountName.Enabled = False
                rfvAccountNumber.Enabled = False
            End If

            'WPR03--Check if request come after reverse instalment then need to focus on instalment tab.
            If Cache("InstalmentTabActive") IsNot Nothing AndAlso Cache("InstalmentTabActive") = "1" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "InstalmentTabActive", _
                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ $('.nav-tabs a[href=""#tab-Instalments""]').tab('show'); });</script>", False)
                Cache.Remove("InstalmentTabActive")
            End If
        End Sub
        Protected Sub EnableDisableButtons(ByRef oFinancePlanStatus As FinancePlanStatus)

            btnDelete.Enabled = False 'keep disabled in all the cases except "updated" status
            btnRelease.Enabled = False 'keep disabled in all the cases except "onhold" status
            btnReprint.Visible = False 'not required as of now
            Select Case oFinancePlanStatus
                Case FinancePlanStatus.Item010 'Saved
                Case FinancePlanStatus.Item011 'Updated
                    btnDelete.Enabled = True
                    btnCancelPlan.Enabled = False
                    btnMTA.Enabled = False
                    btnRelease.Enabled = False
                    btnSettle.Enabled = False
                    btnSave.Text = GetLocalResourceObject("lblTransact") 'In case of 'updated' plan, the "Save" button must show and work as "Transact" Button.
                Case FinancePlanStatus.Item012 'QuotePrinted
                Case FinancePlanStatus.Item040 'Live
                Case FinancePlanStatus.Item140 'OnHold
                    btnRelease.Enabled = True
                    btnDelete.Enabled = False
                    btnCancelPlan.Enabled = False
                    btnMTA.Enabled = False
                    btnSave.Enabled = False
                    btnSettle.Enabled = False
                Case FinancePlanStatus.Item900 'Completed
                    btnSettle.Enabled = False
                    btnMTA.Enabled = False
                Case FinancePlanStatus.Item990 'Superceded
                    btnMTA.Enabled = False
                    btnSettle.Enabled = False
                Case FinancePlanStatus.Item999 'Cancelled : No Button to be enabled in this case
                    btnSave.Enabled = False
                    btnSettle.Enabled = False
                    btnMTA.Enabled = False
                    btnCancelPlan.Enabled = False
                    btnBack.Visible = True
                Case Else
            End Select
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnCancelPlan.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PlanCancel.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                    oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    If oFinancePlanDetails IsNot Nothing AndAlso oFinancePlanDetails.SettlementAmount = 0 Then
                        'btnMTA.Attributes.Add("onclick", "<script type='text/javascript' language='javascript'>$(document).ready(function(){alert('" & GetLocalResourceObject("msgOutStanding") & "');return false;});</script>")
                        btnMTA.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("msgOutStanding").ToString() & "'); return false;")
                        btnSettle.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("msgNoOutStanding").ToString() & "'); return false;")
                    Else
                        btnSettle.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PFPlanSettle.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                        btnMTA.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PlanMTA.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                    End If
                Else
                    btnMTA.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PlanMTA.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                End If
            Else
                btnCancelPlan.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PlanCancel.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                    oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    If oFinancePlanDetails IsNot Nothing AndAlso oFinancePlanDetails.SettlementAmount = 0 Then
                        'btnMTA.Attributes.Add("onclick", "<script type='text/javascript' language='javascript'>$(document).ready(function(){alert('" & GetLocalResourceObject("msgOutStanding") & "');return false;});</script>")
                        btnMTA.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("msgOutStanding").ToString() & "'); return false;")
                        btnSettle.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("msgNoOutStanding").ToString() & "'); return false;")
                    Else
                        btnSettle.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PFPlanSettle.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                        btnMTA.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PlanMTA.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                    End If
                Else
                    btnMTA.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PlanMTA.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&FinancePlanKey=" + Convert.ToString(nFinancePlanKey) + "&FinancePlanVersion=" + Convert.ToString(nFinancePlanVersion) + "' , null);return false;"
                End If
            End If

        End Sub

        ''' <summary>
        ''' Update the session objects at one go for all the tabs and then send the collection to SAM Provider layer foor updation
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan
            Dim hdnMediaTypeCode As HiddenField
            hdnMediaTypeCode = CType(fpPlanDetails.FindControl("hvMediaTypeCode"), HiddenField)

            If Session(CNFinancePlanDetails) IsNot Nothing Then
                oProcessPFPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)
            End If
            If oProcessPFPlan.PremiumFinanceDetails.StatusInd = NexusProvider.FinancePlanStatus.Item140 Then
                If Page.IsValid Then

                    nFinancePlanKey = Request.QueryString("FinancePlanKey")
                    nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                    oProcessPFPlan.PFPremFinanceKey = nFinancePlanKey
                    oProcessPFPlan.PFPremFinanceVersion = nFinancePlanVersion
                    'In case of release plan, send IsDDCancelled = false and SaveOnly=true
                    oProcessPFPlan.PremiumFinanceDetails.IsDDCancelled = False
                    oProcessPFPlan.PremiumFinanceDetails.IsCCCancelled = False
                    'SAM requires status to be sent from Nexus - Live in case of Release button click
                    oProcessPFPlan.PremiumFinanceDetails.StatusInd = NexusProvider.FinancePlanStatus.Item040
                    If Trim(hdnMediaTypeCode.Value) = "DD" Then
                        fpBankDetails.BankAccountDetails_Save()
                    Else
                        fpCreditCardDetails.CreditCardDetails_Save()
                    End If
                    If oProcessPFPlan.PremiumFinanceDetails.IsDDCancelled Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myScript", "javascript:ConfirmBeforePlanHold('" + GetLocalResourceObject("lbl_PlanHoldConfirmation") + "');", True)
                    ElseIf oProcessPFPlan.PremiumFinanceDetails.IsCCCancelled Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myScript", "javascript:ConfirmBeforePlanHold('" + GetLocalResourceObject("lbl_PlanHoldCCConfirmation") + "');", True)
                    Else
                        oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, True, Nothing)
                        Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx")
                    End If
                End If

            Else
                If Page.IsValid Then
                    fpPlanDetails.PFPlanDetails_Save()

                    If Trim(hdnMediaTypeCode.Value) = "DD" Then
                        fpBankDetails.BankAccountDetails_Save()
                    Else
                        fpCreditCardDetails.CreditCardDetails_Save()
                    End If

                    If Session(CNFinancePlanDetails) IsNot Nothing Then
                        oProcessPFPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)
                    End If

                    nFinancePlanKey = Request.QueryString("FinancePlanKey")
                    nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                    oProcessPFPlan.PFPremFinanceKey = nFinancePlanKey
                    oProcessPFPlan.PFPremFinanceVersion = nFinancePlanVersion
                    Dim bFlagError As Boolean = False
                    If btnSave.Text = "Transact" Then
                        'SAM requires status to be sent from Nexus - Live in case of "Transact" button click
                        oProcessPFPlan.PremiumFinanceDetails.StatusInd = NexusProvider.FinancePlanStatus.Item040
                    End If
                    If oProcessPFPlan.PremiumFinanceDetails.IsDDCancelled OrElse oProcessPFPlan.PremiumFinanceDetails.IsCCCancelled Then
                        oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, True, Nothing)
                        Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx")
                    ElseIf oProcessPFPlan.PremiumFinanceDetails.IsCCCancelled Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myScript", "javascript:ConfirmBeforePlanHold('" + GetLocalResourceObject("lbl_PlanHoldCCConfirmation") + "');", True)
                    Else
                        Try
                            oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, True, Nothing)
                        Catch ex As NexusProvider.NexusException
                            bFlagError = True
                            HandleException(ex)
                        End Try
                        If Not bFlagError Then
                            Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx")
                        End If

                    End If
                End If
            End If


        End Sub

        Protected Sub btnRelease_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRelease.Click
            If Page.IsValid Then
                btnRelease.Enabled = False
                btnSave.Enabled = True
                Dim hdnMediaTypeCode As HiddenField
                hdnMediaTypeCode = CType(fpPlanDetails.FindControl("hvMediaTypeCode"), HiddenField)

                If Trim(hdnMediaTypeCode.Value) = "DD" Then
                    fpBankDetails.ReleasePlanCall()
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "acttab", "<script type='text/javascript' language='javascript'>$(document).ready(function() {ShowBankAccountTab();});</script>")
                Else
                    fpCreditCardDetails.ReleasePlanCall()
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "acttab", "<script type='text/javascript' language='javascript'>$(document).ready(function() {ShowCreditCardTab();});</script>")
                End If
                
            End If

        End Sub

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            nFinancePlanKey = Request.QueryString("FinancePlanKey")
            nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
            Dim oInstalment As New BaseInstalment
            oInstalment.CancelPremiumFinancePlan(nFinancePlanKey, nFinancePlanVersion, String.Empty, CancelPFPlanType.DeletePlan, 0)
        End Sub



        Public Sub HandleException(ByVal ex As NexusProvider.NexusException)
            Dim iCount As Integer = 0
            Dim sErrorDesc As New StringBuilder
            For Each nError As NexusProvider.NexusError In ex.Errors()
                If nError IsNot Nothing AndAlso (nError.Code = "1000070" OrElse nError.Code = "1000158" OrElse nError.Code = "1000154" OrElse nError.Code = "1000149" OrElse nError.Code = "1000156" OrElse nError.Code = "1000155" OrElse nError.Code = "1000149" OrElse nError.Code = "1000159" OrElse nError.Code = "1000153" OrElse nError.Code = "1000157") Then
                    sErrorDesc.AppendLine(nError.Description)
                Else
                    Throw ex
                    Exit Sub
                End If
            Next
            Dim opage = TryCast(HttpContext.Current.CurrentHandler, System.Web.UI.Page)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ProcessPFPlan", _
             "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" & sErrorDesc.ToString().Trim() & "'); return false;});</script>", False)
        End Sub
    End Class
End Namespace
