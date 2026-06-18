Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports System.IO
Imports Nexus.Constants
Imports System.Resources

Namespace Nexus

    Partial Class Modal_PlanCancel
        Inherits Frontend.clsCMSPage
        Dim nFinancePlanKey As Integer
        Dim nFinancePlanVersion As Integer
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                nFinancePlanKey = Request.QueryString("FinancePlanKey")
                nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
            End If
            If Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET") = "RedirectFinancePlan" Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updated('RedirectFinancePlan','RedirectFinancePlan');", True)
            End If
            Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "OnSubmitScript", "beforeSubmit();")
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim oInstalment As New BaseInstalment
            Dim bFlagError As Boolean = False
            Try
                oInstalment.CancelPremiumFinancePlan(nFinancePlanKey, nFinancePlanVersion, ddlCancelReason.Value, CancelPFPlanType.CancelPlan, 0)
            Catch ex As NexusProvider.NexusException
                bFlagError = True
                HandleException(ex)
            End Try
            If Not bFlagError Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myScript", "javascript:RedirectToPolicyCancel('" + GetLocalResourceObject("lbl_Confirmation") + "');", True)
            End If

        End Sub


        Public Sub HandleException(ByVal ex As NexusProvider.NexusException)
            Dim iCount As Integer = 0
            Dim sErrorDesc As New StringBuilder
            For Each nError As NexusProvider.NexusError In ex.Errors()
                If nError IsNot Nothing AndAlso (nError.Code = "1000153" OrElse nError.Code = "1000150" OrElse nError.Code = "1000151" OrElse nError.Code = "1000163" OrElse nError.Code = "1000166") Then
                    sErrorDesc.AppendLine(nError.Description)
                Else
                    Throw ex
                    Exit Sub
                End If
            Next
            Dim opage = TryCast(HttpContext.Current.CurrentHandler, System.Web.UI.Page)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "CancelPremiumFinancePlan", _
             "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" & sErrorDesc.ToString().Trim() & "'); return false;});</script>", False)

        End Sub

    End Class
End Namespace
