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
    Public Class BaseCorporateClient : Inherits Nexus.BaseClient

        'Retreive mandatory fields information from Portal Configuration
        Private sCCMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).CCMandatoryFields

        Private Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		Session(CNPaymentHubDetails)=Nothing
		Session(CNCardDetails)=Nothing
		Session(CNCashListItem)=Nothing
            ' Check for mandatory fields in Portal configuration.
            If Not String.IsNullOrEmpty(sCCMandatoryFields) Then
                If sCCMandatoryFields.Contains("Main") Then
                    'Only when the Control is found will be made visible.
                    If oMaster.FindControl("PnlRegisterCC") IsNot Nothing Then
                        For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                            Select Case oControl.ID
                                Case "lblMainContactRequired"
                                    CType(oControl, HtmlGenericControl).Visible = True
                                Case "vldMainContact"
                                    CType(oControl, RequiredFieldValidator).Enabled = True

                                    Dim s_MainContact As String = CType(oControl, RequiredFieldValidator).ControlToValidate
                                    DirectCast(oMaster.FindControl("PnlRegisterCC").FindControl(s_MainContact), TextBox).CssClass = "field-medium field-mandatory"

                            End Select
                        Next
                    End If
                End If
            End If

            'Only when control is present, will be made to visible
            If oMaster.FindControl("PnlRegisterCC") IsNot Nothing Then
                For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                    Select Case oControl.ID
                        Case "liFileCode"
                            'show the File Code textbox only if EnableFileCodeSearch=true in web.config <portal> specific
                            CType(oControl, HtmlGenericControl).Visible = CBool(CType(WebConfigurationManager.GetSection("NexusFrameWork"),
                            Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID).AllowFileCodeField)
                        Case "liBranchCode"
                            CType(oControl, HtmlGenericControl).Visible = CBool(CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1)

                    End Select
                Next
            End If

            'Config Visiblity Of Filecode from Web.config file 25-Feb-2014
            If oMaster.FindControl("liFileCode") IsNot Nothing Then
                Dim oControl As HtmlGenericControl = CType(oMaster.FindControl("liFileCode"), HtmlGenericControl)
                oControl = oMaster.FindControl("liFileCode")
                oControl.Visible = CBool(CType(WebConfigurationManager.GetSection("NexusFrameWork"),  _
                            Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID).AllowFileCodeField)
            End If
        End Sub

        ''' <summary>
        ''' Page Load Event for Corporate Client.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'To set Focus on txtCompanyName Control
                If oMaster.FindControl("txtCompanyName") IsNot Nothing Then
                    CType(oMaster.FindControl("txtCompanyName"), TextBox).Focus()
                End If

                'Check system option 5011 for Blacklisting Reason and Renewal Stop Code visibility
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

            If sCCMandatoryFields.Contains("Main") Then
                'Only when the Control is found will be made visible.
                If oMaster.FindControl("PnlRegisterCC") IsNot Nothing Then
                    For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                        Select Case oControl.ID
                            Case "lblMainContactRequired"
                                CType(oControl, HtmlGenericControl).Visible = True
                            Case "vldMainContact"
                                CType(oControl, RequiredFieldValidator).Enabled = True
                                Dim s_MainContact As String = CType(oControl, RequiredFieldValidator).ControlToValidate
                                DirectCast(oMaster.FindControl("PnlRegisterCC").FindControl(s_MainContact), TextBox).CssClass = "field-mandatory form-control"
                        End Select
                    Next
                End If
            Else
                'Only when the Control is found will be made validation disabled.
                If oMaster.FindControl("PnlRegisterCC") IsNot Nothing Then
                    For Each oControl As Control In oMaster.FindControl("PnlRegisterCC").Controls
                        Select Case oControl.ID
                            Case "lblMainContactRequired"
                                CType(oControl, HtmlGenericControl).Visible = True
                            Case "vldMainContact"
                                CType(oControl, RequiredFieldValidator).Enabled = False
                                Dim s_MainContact As String = CType(oControl, RequiredFieldValidator).ControlToValidate
                                DirectCast(oMaster.FindControl("PnlRegisterCC").FindControl(s_MainContact), TextBox).CssClass = "form-control"
                        End Select
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' AddClient event resets all the session values to add new client.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Sub BtnAddClientClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?mode=add", True)
        End Sub

    End Class

End Namespace

