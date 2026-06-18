Imports CMS.library
Imports System.Data
Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class ForgottenPassword
        Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Page.SetFocus(txtUserName)
        End Sub

        ''' <summary>
        ''' Method to handle password reset submit button click event. 
        ''' - If local user is found then password is reset via Membership provider. 
        ''' - Otherwise a password reset is attempted via the Nexus provider.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            'ForgottenPassword will accept only userName and not password.
            Dim sUser As String = txtUserName.Text
            Dim oUser As MembershipUser = Membership.GetUser(sUser)
            Dim sNewPassword As String = String.Empty
            sNewPassword = oUser.ResetPassword()
            If HttpContext.Current.Session(CNIsAgent) = LoginType.Agent Then 'If agent user
                Session.Remove(CNIsAgent)
                If String.IsNullOrEmpty(sNewPassword) Then 'If sNewPassword is not empty for agent user it means there is an exception 
                    pnlForgottenPassword.Visible = False
                    divSubmitArea.Visible = False
                    pnlConfirmation.Visible = True
                Else
                    custvldUserExists.ErrorMessage = GetLocalResourceObject("lbl_Err_UserExists")
                    custvldUserExists.IsValid = False
                End If
            Else
                If sNewPassword = "The user account has been locked out." Then
                    custvldUserExists.ErrorMessage = GetLocalResourceObject("lbl_Account_Locked")
                    custvldUserExists.IsValid = False
                Else
                    pnlForgottenPassword.Visible = False
                    divSubmitArea.Visible = False
                    pnlConfirmation.Visible = True

                    'SEND EMAIL
                    Dim dtEmailDetails As New DataTable
                    Dim EmailTemplates As New Nexus.Library.Config.EmailTemplates
                    EmailTemplates = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).EmailTemplates

                    Dim sSenderAddress As String = String.Empty
                    Dim sRecipient As String = oUser.Email
                    Dim sTemplatePath As String = String.Empty

                    dtEmailDetails.Columns.Add("ID")
                    dtEmailDetails.Columns.Add("Code")
                    dtEmailDetails.Columns.Add("Path")

                    Dim drEmailDetails As DataRow
                    For i As Integer = 0 To EmailTemplates.Count - 1
                        drEmailDetails = dtEmailDetails.NewRow()
                        With EmailTemplates.EmailTemplate(i)
                            drEmailDetails(0) = .ID
                            drEmailDetails(1) = .Path
                            drEmailDetails(2) = .Sender
                        End With

                        If drEmailDetails(0) = "ForgottenPassword" Then
                            sTemplatePath = drEmailDetails(1)
                            sSenderAddress = drEmailDetails(2)
                            Exit For
                        End If
                    Next

                    'SET UP HASH TABLE FOR DATA THAT NEEDS TO BE COLLECTED FOR EMAIL
                    Dim EmailDetails As New Hashtable
                    EmailDetails.Add("[!TITLE!]", GetLocalResourceObject("lbl_Email_Title").ToString()) 'Forgotten Password
                    EmailDetails.Add("[!HEADER!]", GetLocalResourceObject("lbl_Email_Title").ToString()) 'Forgotten Password
                    EmailDetails.Add("[!USERNAME!]", sUser)
                    EmailDetails.Add("[!PASSWORD!]", sNewPassword)
                    sRecipient = oUser.Email
                    'SEND FORGOTTEN PASSWORD EMAIL
                    SendEmail(sSenderAddress, sRecipient, GetLocalResourceObject("lbl_Email_Title").ToString(), "", EmailDetails, sTemplatePath, Nothing, Nothing, Nothing,
                    Nothing)

                End If
            End If
        End Sub

    End Class
End Namespace