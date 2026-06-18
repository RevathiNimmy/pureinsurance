Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_PolicyCancel
        Inherits Frontend.clsCMSPage
        Dim nFinancePlanKey As Integer
        Dim nFinancePlanVersion As Integer
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                    nFinancePlanKey = Request.QueryString("FinancePlanKey")
                    nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                End If
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim oInstalment As New BaseInstalment
            Dim bFlagError As Boolean = False
            If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                nFinancePlanKey = Request.QueryString("FinancePlanKey")
                nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
            End If
            Try
                oInstalment.CancelPFPolicies(nFinancePlanKey, nFinancePlanVersion, ddlCancelReason.Value, chkSpoolPolicy.Checked, chkWriteOff.Checked, txtPolicyLapseDate.Text)
            Catch ex As NexusProvider.NexusException
                bFlagError = True
                HandleException(ex)
            End Try
            If Not bFlagError Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.RedirecttoFindTransactions();", True)
            End If

        End Sub
        ''' <summary>
        ''' To Handle exceptions
        ''' </summary>
        ''' <param name="ex"></param>
        ''' <remarks></remarks>
        Public Sub HandleException(ByVal ex As NexusProvider.NexusException)
            Dim sErrorDesc As New StringBuilder
            For Each nError As NexusProvider.NexusError In ex.Errors()
                If nError IsNot Nothing AndAlso nError.Code = "1000148" Then
                    sErrorDesc.AppendLine(nError.Description)
                ElseIf nError IsNot Nothing AndAlso nError.Code = "1000161" Then 'Code : 1000161 :: Description: Auto Allocation failed
                    If CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote) IsNot Nothing AndAlso Convert.ToDateTime(txtPolicyLapseDate.Text) < CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).CoverStartDate Then
                        sErrorDesc.AppendLine(GetLocalResourceObject("msgBackDatedPolicyLapseDate"))
                    Else
                        sErrorDesc.AppendLine(IIf(GetLocalResourceObject("msgAllocationDeclined") Is Nothing, "Payment transaction has not been auto-allocated.", GetLocalResourceObject("msgAllocationDeclined")))
                    End If
                ElseIf nError IsNot Nothing AndAlso nError.Code = "1000162" Then 'Code : 1000162 :: Description: Auto Allocation failed
                    sErrorDesc.AppendLine(IIf(GetLocalResourceObject("msgWriteOffAuthorityDisabled") Is Nothing, "Write off attempt was unsuccessful. You do not have write off authority.", GetLocalResourceObject("msgWriteOffAuthorityDisabled")))
                ElseIf nError IsNot Nothing AndAlso nError.Code = "1000163" Then 'Code : 1000163 :: Description: Auto Allocation failed
                    sErrorDesc.AppendLine(IIf(GetLocalResourceObject("msgWriteOffAuthorityAmountExceeded") Is Nothing, "Write off attempt was unsuccessful. The outstanding balance amount was not within your write off limit.", GetLocalResourceObject("msgWriteOffAuthorityAmountExceeded")))
                Else
                    Throw ex
                    Exit Sub
                End If
            Next
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "CancelPolicy",
             "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" & sErrorDesc.ToString().Trim() & "'); return false;});</script>", False)

        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.RedirecttoFindTransactions();", True)

        End Sub
    End Class
End Namespace
