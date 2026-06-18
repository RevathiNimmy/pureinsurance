Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Namespace Nexus


    Partial Class Modal_PremiumFinanceQuote : Inherits CMS.Library.Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'This will register a function for showing updatepanel errors as alert 
            'and also for showing busy progress(full screen) during postback from update panels
            'If Not (Page.ClientScript.IsStartupScriptRegistered("EndRequestHandlerForUpdatePanel")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "EndRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForUpdatePanel);", True)
            'End If
            'If Not (Page.ClientScript.IsStartupScriptRegistered("StartRequestHandlerForUpdatePanel")) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "StartRequestHandlerForUpdatePanel", "Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerForUpdatePanel);", True)
            'End If
            ' Script initializer call to handle the busy indicator.
            'If Not Page.ClientScript.IsOnSubmitStatementRegistered("OnSubmitScript") Then
            '    Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "OnSubmitScript", "beforeSubmit();")
            'End If
        End Sub

        ''' <summary>
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            If Page.IsValid Then
                btnSave.Enabled = False
                Try
                    If Not ucInstalments.SelectedInstalmentQuote Is Nothing Then
                        If ucInstalments.SelectedInstalmentQuote.MediaTypeDescription = "Direct Debit" And ucInstalments.SelectedAccountType Is Nothing Then
                            'Cache for instalment control has been refreshed. So inform the user and close the window.
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "alert('" & GetLocalResourceObject("msg_InstalmentCacheNotAvailable") & "');self.parent.tb_remove();", True)
                            Exit Sub
                        End If
                    Else
                        'Cache for instalment control has been refreshed. So inform the user and close the window.
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "alert('" & GetLocalResourceObject("msg_InstalmentCacheNotAvailable") & "');self.parent.tb_remove();", True)
                        Exit Sub
                    End If
                    'Required data available in session. So proceed to save
                    ucInstalments.SaveInstallmentPlan()
                    Dim oProcessPFPlanResponse As New NexusProvider.PremiumFinancePlan
                    Dim oBaseInstalment As New BaseInstalment()
                    If Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" Then

                        Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan

                        If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                            oProcessPFPlan.PFPremFinanceKey = Request.QueryString("FinancePlanKey")
                            oProcessPFPlan.PFPremFinanceVersion = Request.QueryString("FinancePlanVersion")

                        End If
                        If Request.QueryString("ShowPlan") IsNot Nothing AndAlso Request.QueryString("ShowPlan") = False Then
                            oProcessPFPlanResponse = oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, False, NexusProvider.ProcessPFPlanType.PlanMTA, NexusProvider.InstalmentType.NoAmountChange)
                        Else
                            'Call on Plan Maintenance with add New transactions to existing plan.
                            oProcessPFPlanResponse = oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, False, NexusProvider.ProcessPFPlanType.PlanMTA)
                        End If

                    ElseIf Request.QueryString("Type") IsNot Nothing AndAlso (Request.QueryString("Type") = "NewPlan" OrElse Request.QueryString("Type") = "NewPlanSED") AndAlso Session(CNInstalmentPlanMode) = InstalmentPlanType.Add Then
                        Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan
                        If Request.QueryString("Type") = "NewPlanSED" Then
                            oProcessPFPlanResponse = oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, False, NexusProvider.ProcessPFPlanType.NewPlan, , "SED")
                        Else
                            oProcessPFPlanResponse = oBaseInstalment.UpdatePremiumFinancePlan(oProcessPFPlan, Nothing, False, NexusProvider.ProcessPFPlanType.NewPlan)
                        End If

                    End If
                    If oProcessPFPlanResponse IsNot Nothing Then
                        If Session(CNMTAPlanType) Is Nothing Then
                            'Redirect Only to display the new plan in case of successfull new plan creation
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updated('RedirectFinancePlanDetails','~/PremiumFinance/FinancePlanDetails.aspx?FinancePlanKey=" & oProcessPFPlanResponse.PFPremFinanceKey & "&FinancePlanVersion=" & oProcessPFPlanResponse.PFPremFinanceVersion & " ');", True)
                        ElseIf Session(CNMTAPlanType) = NexusProvider.InstalmentType.NoAmountChange Then
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updated('RedirectFinancePlanDetails','~/PremiumFinance/FinancePlanDetails.aspx?ProcessType=MTA&FinancePlanKey=" & oProcessPFPlanResponse.PFPremFinanceKey & "&FinancePlanVersion=" & oProcessPFPlanResponse.PFPremFinanceVersion & " ');", True)
                        Else
                            'Redirect to display the new plan along with the message to user specifying the type of plan executed
                            Dim sMsgToDisplay = String.Empty
                            If Session(CNMTAPlanType) = NexusProvider.InstalmentType.AddAndSpread Then
                                sMsgToDisplay = GetLocalResourceObject("msgSpreadOver")
                            ElseIf Session(CNMTAPlanType) = NexusProvider.InstalmentType.AddToNext Then
                                sMsgToDisplay = GetLocalResourceObject("msgAddtoNext")
                            ElseIf Session(CNMTAPlanType) = NexusProvider.InstalmentType.AddToNewPlan Then
                                sMsgToDisplay = GetLocalResourceObject("msgAddtoNew")
                            End If
                            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation", "<script language=""JavaScript"" type=""text/javascript"">alert('" & sMsgToDisplay & "')</script>")
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updatedWithAlert('RedirectFinancePlanDetailsEdit','~/PremiumFinance/FinancePlanDetails.aspx?ProcessType=MTA&FinancePlanKey=" & oProcessPFPlanResponse.PFPremFinanceKey & "&FinancePlanVersion=" & oProcessPFPlanResponse.PFPremFinanceVersion & " ',' " & sMsgToDisplay.Trim() & "');", True)
                        End If
                    End If
                Catch ex As NexusProvider.NexusException
                    HandleException(ex)
                End Try
            End If

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
