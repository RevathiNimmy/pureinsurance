Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Partial Class Controls_BranchInfo
    Inherits System.Web.UI.UserControl

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Current.User.Identity.IsAuthenticated Then
            If Session.Item(CNLoginType) = LoginType.Agent Then
                PnlBranchName.Visible = True
                Dim sBranchName As String = ""
                Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                If oBranchs IsNot Nothing Then
                    For Each oBranch As NexusProvider.Branch In oBranchs
                        If oBranch.Code = Session(CNBranchCode) Then
                            sBranchName = oBranch.Description
                            Exit For
                        End If
                    Next
                End If
                lblBranchName.Text = sBranchName
                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
                    hypChangeBranchName.Visible = True
                    hypChangeBranchName.NavigateUrl = "~/Modal/SelectBranch.aspx?PostbackTo=" & PnlBranchName.ClientID.ToString & "&FromPage=" & Session(CNClientType) & "&modal=true&KeepThis=true&TB_iframe=true&height=400&width=750"
                End If
            End If
        Else
            'Not logged in, so hide control
            Me.Visible = False
        End If
    End Sub
End Class
