Imports CMS.Library
Imports Nexus.Library
Imports SiriusFS.SAM.Client
Imports System.Xml.XPath
Imports CMS.Library.Portal
Imports System.Xml
Imports System.Globalization.CultureInfo
Imports System.Web.UI
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Session

Namespace Nexus
    Public Class BaseOtherParty : Inherits BaseClient

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Session(CNPaymentHubDetails) = Nothing
            Session(CNCardDetails) = Nothing
            Session(CNCashListItem) = Nothing

            If Not IsPostBack Then
                If oMaster.FindControl("txtDOB") IsNot Nothing Then 'If DOB field is there than only it should try to set
                    CType(oMaster.FindControl("txtDOB"), TextBox).Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                End If
            End If
        End Sub

        Private Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim webService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim userDetail As NexusProvider.UserDetails = Session(CNAgentDetails)
            If Not IsPostBack Then
                'To set Focus on ddlTitle Control
                If oMaster.FindControl("ddlTitle") IsNot Nothing Then
                    CType(oMaster.FindControl("ddlTitle"), NexusProvider.LookupList).Focus()
                End If
                If oMaster.FindControl("ctrlLetterWriting") IsNot Nothing Then
                    If Session(CNParty) Is Nothing Then
                        CType(oMaster.FindControl("ctrlLetterWriting"), UserControl).Visible = False
                    Else
                        Dim oParty As NexusProvider.BaseParty = HttpContext.Current.Session(CNParty)
                        If oParty.Key = 0 Then
                            CType(oMaster.FindControl("ctrlLetterWriting"), UserControl).Visible = False
                        Else
                            If CType(oMaster.FindControl("ctrlLetterWriting"), UserControl) IsNot Nothing Then
                                CType(oMaster.FindControl("ctrlLetterWriting"), UserControl).Visible = True
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Protected Sub custrngvldDOB_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim sDOB As String = Nothing
            Dim dStartDate As Date = New Date("1899", Today.Month, Today.Day, Today.Hour, Today.Minute, Today.Second)
            Dim dEndDate As Date = Date.Today
            If oMaster.FindControl("PnlRegisterOP") IsNot Nothing Then
                For Each oControl As Control In oMaster.FindControl("PnlRegisterOP").Controls
                    Select Case oControl.ID
                        Case "txtDOB"
                            If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                sDOB = CType(oMaster.FindControl("txtDOB"), TextBox).Text
                                If Not (sDOB.Trim = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) Then
                                    If Not IsDate(sDOB) Then
                                        CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("lbl_InvalidDOB")
                                        args.IsValid = False
                                        CType(oControl, TextBox).Focus()
                                        Exit Sub
                                    ElseIf CType(sDOB, Date) < dStartDate Or CType(sDOB, Date) > dEndDate Then
                                        CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("lbl_InvalidDOB")
                                        args.IsValid = False
                                        CType(oControl, TextBox).Focus()
                                        Exit Sub
                                    Else
                                        args.IsValid = True
                                    End If
                                End If
                            End If
                    End Select
                Next
            End If
        End Sub

        ''' <summary>
        ''' AddClient event resets all the session values to add new client.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnAddClientClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Response.Redirect("~/secure/agent/OtherPartyDetails.aspx?mode=add", True)
        End Sub

    End Class

End Namespace
