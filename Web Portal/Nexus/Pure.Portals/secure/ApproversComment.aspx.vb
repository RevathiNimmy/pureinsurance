Imports CMS.Library
Imports Nexus.Constants.Session

Partial Class secure_ApproversComment : Inherits Frontend.clsCMSPage
    Dim oWebservice As NexusProvider.ProviderBase
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("Mode") = "VP" Then
            txtNewComments.Visible = False

        End If

    End Sub
    Public Sub UpdateManualJournalApproversComment()
        Dim oSamProvider As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()
        Dim oUpdateAuthorizationComment As NexusProvider.UpdateManualJournalApproversComment
        Dim sBranchCode As String = Session(CNBranchCode).ToString()
        oUpdateAuthorizationComment = New NexusProvider.UpdateManualJournalApproversComment
        Dim iManualJournalId As Integer
        If Request.QueryString("ManualJournalKey") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("ManualJournalKey")) Then
            iManualJournalId = CType(Request.QueryString("ManualJournalKey").Trim, Integer)
        End If
        oUpdateAuthorizationComment.ManualJournalId = iManualJournalId
        oUpdateAuthorizationComment.Comment = txtNewComments.Text

        oSamProvider.UpdateManualJournalApproversComment(oUpdateAuthorizationComment, sBranchCode)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        UpdateManualJournalApproversComment()
        Dim smode As String = ""

        If (Request.QueryString.HasKeys() = True) Then
            smode = Request.QueryString("mode").Trim.ToUpper()

        End If
        Dim message As String = ""
        If (Session("aaprovalmsg") IsNot Nothing) Then
            message = Session("aaprovalmsg").ToString()
        End If

        If (Session("furtheraaprovalmsg") IsNot Nothing) Then
            message = Session("furtheraaprovalmsg").ToString()
        End If

        If (message <> "") Then
            If smode = "WM" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Authorise", "alert('" & message & "');window.location.href = 'workmanager.aspx'", True)

            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Authorise", "alert('" & message & "');window.location.href = 'AuthorizeManualJournal.aspx'", True)
            End If
        Else
            If smode = "WM" Then
                Response.Redirect("~/secure/workmanager.aspx")
            Else
                Response.Redirect("~/secure/AuthorizeManualJournal.aspx")
            End If

        End If
        Session.Remove("aaprovalmsg")
        Session.Remove("furtheraaprovalmsg")

    End Sub
End Class
