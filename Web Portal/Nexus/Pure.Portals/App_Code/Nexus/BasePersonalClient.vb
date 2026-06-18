Imports CMS.library
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
    Public Class BasePersonalClient : Inherits BaseClient

        Private sPCMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).PCMandatoryFields

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		Session(CNPaymentHubDetails)=Nothing
		Session(CNCardDetails)=Nothing
		Session(CNCashListItem)=Nothing
            'According to Portal configuration DOB fields mandatory option is enabled.
            If Not String.IsNullOrEmpty(sPCMandatoryFields) Then
                If sPCMandatoryFields.Contains("DOB") Then
                    Dim vldDateOfBirthRequired As RequiredFieldValidator = New RequiredFieldValidator()
                    vldDateOfBirthRequired.ID = "vldDateOfBirthRequired"
                    vldDateOfBirthRequired.Display = ValidatorDisplay.None
                    vldDateOfBirthRequired.ControlToValidate = "txtDOB"
                    vldDateOfBirthRequired.ErrorMessage = GetLocalResourceObject("lb_RequiredValidator")
                    vldDateOfBirthRequired.SetFocusOnError = True
                    vldDateOfBirthRequired.Enabled = False
                    oMaster.FindControl("PnlRegisterPC").Controls.Add(vldDateOfBirthRequired)
                    'Only when the Control is found will be made visible.
                    If oMaster.FindControl("PnlRegisterPC") IsNot Nothing Then
                        For Each oControl As Control In oMaster.FindControl("PnlRegisterPC").Controls
                            Select Case oControl.ID
                                Case "lblDateOfBirth"
                                    CType(oControl, Label).Visible = False
                                Case "lblDateOfBirthRequired"
                                    CType(oControl, Label).Visible = True
                                Case "vldDateOfBirthRequired"
                                    CType(oControl, RequiredFieldValidator).Enabled = True
                                    CType(oControl, RequiredFieldValidator).Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                                Case "txtDOB"
                                    CType(oControl, TextBox).CssClass = CType(oControl, TextBox).CssClass & " field-mandatory"
                            End Select
                        Next
                    End If
                End If
            End If

            'Only when control is present, will be made to visible
            If oMaster.FindControl("PnlRegisterPC") IsNot Nothing Then
                For Each oControl As Control In oMaster.FindControl("PnlRegisterPC").Controls
                    Select Case oControl.ID
                        Case "liFileCode"
                            'show the File Code textbox only if EnableFileCodeSearch=true in web.config <portal> specific
                            CType(oControl, HtmlGenericControl).Visible = CBool(CType(WebConfigurationManager.GetSection("NexusFrameWork"), _
                            Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID).AllowFileCodeField)
                        Case "liBranchCode"
                            CType(oControl, HtmlGenericControl).Visible = CBool(CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1)

                    End Select
                Next
            End If

            If Not IsPostBack Then
                If oMaster.FindControl("txtDOB") IsNot Nothing Then 'If DOB field is there than only it should try to set
                    CType(oMaster.FindControl("txtDOB"), TextBox).Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                End If

                Dim oWebService As NexusProvider.ProviderBase
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oOptionSettings As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, "5011")
                Dim bShowFields As Boolean = (oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue = "1")

                If oMaster.FindControl("lblBlacklistingReason") IsNot Nothing Then
                    CType(oMaster.FindControl("lblBlacklistingReason"), Label).Visible = bShowFields
                End If
                If oMaster.FindControl("ddlBlacklistingReason") IsNot Nothing Then
                    CType(oMaster.FindControl("ddlBlacklistingReason"), NexusProvider.LookupList).Visible = bShowFields
                End If
                If oMaster.FindControl("vldBlacklistingReason") IsNot Nothing Then
                    CType(oMaster.FindControl("vldBlacklistingReason"), RequiredFieldValidator).Enabled = bShowFields
                End If
                If oMaster.FindControl("lblRenewalStopCode") IsNot Nothing Then
                    CType(oMaster.FindControl("lblRenewalStopCode"), Label).Visible = bShowFields
                End If
                If oMaster.FindControl("ddlRenewalStopCode") IsNot Nothing Then
                    CType(oMaster.FindControl("ddlRenewalStopCode"), NexusProvider.LookupList).Visible = bShowFields
                End If
                If oMaster.FindControl("vldRenewalStopCode") IsNot Nothing Then
                    CType(oMaster.FindControl("vldRenewalStopCode"), RequiredFieldValidator).Enabled = bShowFields
                End If
            End If
        End Sub

        Private Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

           

        End Sub

        Private Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
#Region " ADD CLIENT "
        ''' <summary>
        ''' AddClient event resets all the session values to add new client.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnAddClientClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?mode=add", True)
        End Sub
#End Region
#Region " DOB - Server Validation "
        Protected Sub custrngvldDateOfBirth_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
            Dim sDOB As String = Nothing
            Dim dStartDate As Date = Date.Today.AddYears(-109)
            Dim dEndDate As Date = Date.Today
            If oMaster.FindControl("PnlRegisterPC") IsNot Nothing Then
                For Each oControl As Control In oMaster.FindControl("PnlRegisterPC").Controls
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
                                Else
                                    If Not IsDate(sDOB) Then
                                        CType(source, CustomValidator).ErrorMessage = GetLocalResourceObject("lbl_InvalidDOB")
                                        args.IsValid = False
                                        CType(oControl, TextBox).Focus()
                                        Exit Sub
                                    End If
                                End If
                            End If
                    End Select
                Next
            End If
        End Sub
#End Region
    End Class

End Namespace
