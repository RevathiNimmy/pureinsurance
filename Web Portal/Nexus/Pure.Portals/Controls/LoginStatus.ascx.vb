Imports System.Web.HttpContext
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Web

Namespace Nexus

    Partial Class LoginStatus : Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            If Current.User.Identity.IsAuthenticated Then

                Select Case Session.Item(CNLoginType)

                    Case LoginType.Agent

                        oWebService = New NexusProvider.ProviderManager().Provider
                        Dim oUserDetail As NexusProvider.UserDetails = Session(CNAgentDetails)

                        With CType(Session.Item(CNAgentDetails), NexusProvider.UserDetails)

                            If Not IsWindowsAuthentication() Then
                                lbtnChangePassword.Visible = True
                            Else
                                lbtnChangePassword.Visible = False
                            End If
                        End With
                        Dim sBranchName As String = ""
                        Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                        If oBranchs IsNot Nothing Then
                            For Each oBranch As NexusProvider.Branch In oBranchs
                                If oBranch.Code = Session(CNBranchCode) Then
                                    sBranchName = oBranch.Description
                                    Session("BranchName") = sBranchName
                                    Exit For
                                End If
                            Next
                        End If
                        If oBranchs.Count > 1 Then
                            Dim HideFolderName As String = "/PRODUCTS/"
                            If Not Request.CurrentExecutionFilePath.ToUpper.Contains(HideFolderName.ToUpper) Then
                                hypChangeBranchName.Visible = True
                            Else
                                hypChangeBranchName.Visible = False
                            End If
                            hypChangeBranchName.NavigateUrl = "~/Modal/SelectBranch.aspx?PostbackTo=" & PnlBranchName.ClientID.ToString & "&FromPage=" & Session(CNClientType) & "&modal=true&KeepThis=true&TB_iframe=true&height=400&width=750"
                        End If
                End Select

            Else

                'Not logged in, so hide control
                Me.Visible = False

            End If

        End Sub

        Protected Sub lbtnChangePassword_Click(sender As Object, e As EventArgs) Handles lbtnChangePassword.Click

            Dim modules As HttpModuleCollection = HttpContext.Current.ApplicationInstance.Modules
            Dim authModule As IHttpModule = modules.Get("AuthHttpModule")
            If authModule IsNot Nothing Then
                Response.Redirect(AppSettings("AuthDomain").ToString & "/account/password/", False)
            Else
                Response.Redirect("~/secure/ChangePassword.aspx", False)
            End If
        End Sub
    End Class

End Namespace
