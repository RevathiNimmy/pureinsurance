Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Partial Class Controls_SideInfo
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim bolIsReturn As Boolean = False
        'If Current.User.Identity.IsAuthenticated AndAlso Session.Item(CNLoginType) = LoginType.Agent Then
        '    UserInfo.Visible = True
        'Else
        '    UserInfo.Visible = False
        'End If

        If Session(CNParty) IsNot Nothing Then
            ClientInfo.Visible = True
        End If

        If Session(CNNoTrans) IsNot Nothing Then
            lblManualTransfer.Visible = True
            ManualTransfer.Visible = True
            If Session(CNNoTrans).ToString() = "NB" Then
                lblManualTransfer.Text = lblManualTransfer.Text.Replace("#", "Policy")
            Else
                lblManualTransfer.Text = lblManualTransfer.Text.Replace("#", Session(CNNoTrans).ToString())
            End If
        End If

        Select Case CType(Session(CNMode), Mode)
            Case Mode.ViewClaim, Mode.EditClaim, Mode.NewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.ViewClaimPayment, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend

                If Request.QueryString("ReturnUrl") IsNot Nothing Then
                    If Request.QueryString("ReturnUrl").ToUpper.Contains("~/CLAIMS/") Then
                        bolIsReturn = True
                    End If
                End If

                Dim AllowFolderName As String = "/CLAIMS/"
                If Not Request.CurrentExecutionFilePath.ToUpper.Contains(AllowFolderName.ToUpper) And Not bolIsReturn Then
                    'claim control should be hidden
                    ClaimInfo.Visible = False
                Else
                    If Request.CurrentExecutionFilePath.ToUpper.Contains("FINDCLAIM.ASPX") OrElse Request.CurrentExecutionFilePath.ToUpper.Contains("FINDINSURANCEFILE.ASPX") Then
                        ClaimInfo.Visible = False
                    Else
                        ClaimInfo.Visible = True
                    End If
                End If
        End Select
    End Sub


End Class
