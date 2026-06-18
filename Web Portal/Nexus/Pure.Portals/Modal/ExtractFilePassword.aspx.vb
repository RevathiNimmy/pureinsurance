Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.IO
Imports System.Web.UI
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Xml

Partial Class Modal_ExtractFilePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub

    Protected Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If Page.IsValid Then

            'PBI 39413: Server-side authority re-validation
            Try
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanExtractClientData
                Dim oAuthWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oAuthWebService.GetUserAuthorityValue(oUserAuthority)
                If oUserAuthority.UserAuthorityValue <> "1" Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "NoAuth",
                                                        "alert('" & GetLocalResourceObject("err_NoExtractAuthority").ToString().Replace("'", "\'") & "')", True)
                    Exit Sub
                End If
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "AuthError",
                                                    "alert('" & GetLocalResourceObject("err_AuthorityCheckFailed").ToString().Replace("'", "\'") & "')", True)
                Exit Sub
            End Try

            Dim oWebservice As NexusProvider.ProviderBase = Nothing
            Dim sPassword As String = String.Empty
            Dim abClientDataExtract As Byte() = Nothing

            Try
                If Session(CNParty) IsNot Nothing Then
                    sPassword = txtPassword.Text.Trim()
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    abClientDataExtract = oWebservice.GetClientDataExtract(DirectCast(Session(CNParty), NexusProvider.BaseParty).Key, sPassword)

                    If abClientDataExtract Is Nothing OrElse abClientDataExtract.Length = 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "Extract File",
                                                            "alert('There was an error generating data file. Please try again.')", True)
                        Exit Sub
                    End If

                'PBI 39544: Log both a client event and a system event after successful extraction
                    Dim oParty As NexusProvider.BaseParty = DirectCast(Session(CNParty), NexusProvider.BaseParty)
                    Dim iPartyKey As Integer = oParty.Key
                    Dim sClientCode As String = If(oParty.UserName, String.Empty)

                    'Path A — Client Events tab (event_log + party_public_text)
                    Try
                        Dim iEventTypeKey As Integer = 0
                        Try
                            Dim oEventTypeList As NexusProvider.LookupListCollection = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Event_type", False, False)
                            For iListCount As Integer = 0 To oEventTypeList.Count - 1
                                If oEventTypeList(iListCount).Code = "CLIEXTRACT" Then
                                    iEventTypeKey = oEventTypeList(iListCount).Key
                                    Exit For
                                End If
                            Next
                        Catch
                        End Try

                        If iEventTypeKey > 0 Then
                            Dim oEventDetails As New NexusProvider.EventDetails
                            With oEventDetails
                                .PartyKey = iPartyKey
                                .EventTypeKey = iEventTypeKey
                                .EventDate = Now()
                                .RtfText = "Client Data Extracted"
                                .UserName = Session(CNLoginName)
                            End With
                            oWebservice.AddEvent(oEventDetails)
                        End If
                        Catch
                            'Fail silently — event logging must never block the file download
                        End Try
                        'Path B — System Events page (configuration_audit_master + configuration_audit_details)
                        Try
                            oWebservice.AddClientDataExtractAuditTrail(iPartyKey, sClientCode)
                        Catch
                            'Fail silently — event logging must never block the file download
                        End Try

                    Response.Clear()
                    Response.ClearContent()
                    Response.ClearHeaders()
                    Response.Buffer = True
                    Response.ContentType = "application/zip"
                    Response.AddHeader("Content-Disposition", "inline; filename=ClientDataExtract.zip")
                    Response.BinaryWrite(abClientDataExtract)
                    Response.Flush()
                    Response.End()
                End If

                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Refresh") & ";"
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ParentPostBack", PostBackStr, True)
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove(); return false;", True)
            Finally
                oWebservice = Nothing
            End Try

        End If
    End Sub

    Protected Sub custPasswordLength_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custPasswordLength.ServerValidate
        Dim sPassword As String = String.Empty
        sPassword = txtPassword.Text.Trim()
        If sPassword.Length > 15 Then
            args.IsValid = False
            custPasswordLength.ErrorMessage = GetLocalResourceObject("lbl_InvalidPasswordLength")
            txtPassword.Focus()
        End If
    End Sub

End Class
