Imports System.Xml.Linq
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports CMS.Library
Imports NexusProvider
Imports System.Web.Services
Imports Microsoft.Practices.ObjectBuilder
Imports NexusProvider.Bank


Namespace Nexus
    Partial Class PaymentAuthorizationView : Inherits Frontend.clsCMSPage

        Dim oWebservice As NexusProvider.ProviderBase
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("Mode") = "VP" Then
                txtNewComments.Visible = False
                lbl_NewComments.Visible = False
            End If

        End Sub

        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewBtn_Next.Click 
        
            Dim nCashListItemId As Integer
            ' Dim nUserID As Integer = 4406
            Dim sComments As String = ""
            Dim sMode As String = "VP"

            If Request.QueryString("CashListItemKey") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                nCashListItemId = CType(Request.QueryString("CashListItemKey").Trim, Integer)
            End If
            If Request.QueryString("Mode") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("Mode")) Then
                sMode = CType(Request.QueryString("Mode").Trim, String)
            End If

            If Not String.IsNullOrEmpty(txtNewComments.Text.Trim) Then
                sComments = txtNewComments.Text.Trim
                UpdateCommentAuthorization()

            End If            
            Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & nCashListItemId & "& Mode = " & Session("ModeValue") & "")

        End Sub
        Public Sub UpdateCommentAuthorization()

            Dim oSamProvider As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()
            Dim oUpdateAuthorizationComment As NexusProvider.UpdateAuthorizationComment
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            oUpdateAuthorizationComment = New NexusProvider.UpdateAuthorizationComment
            Dim nCashListItem_Id As Integer
            If Request.QueryString("CashListItemKey") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                nCashListItem_Id = CType(Request.QueryString("CashListItemKey").Trim, Integer)
            End If
            oUpdateAuthorizationComment.CashListItem_id = nCashListItem_Id
            oUpdateAuthorizationComment.Comment = txtNewComments.Text

            oSamProvider.UpdateAuthorizationComment(oUpdateAuthorizationComment, sBranchCode)
        End Sub

        
    End Class
End Namespace

