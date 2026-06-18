Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_ReInsuranceDetails : Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oWebService As NexusProvider.ProviderBase
            If Not IsPostBack Then
                SetPageProgress(4)
                oWebService = New NexusProvider.ProviderManager().Provider

                'Check the User Authority to display of Reinsurance
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayReinsurance
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.GetUserAuthorityValue(oUserAuthority)
                'Check for RI2007 product hidden option
                Dim oRI2007 As NexusProvider.OptionTypeSetting = Nothing
                oRI2007 = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)

                If oRI2007.OptionValue = "1" Then
                    ' if RI2007 is ON
                    ReInsurance2007Cntrl.Visible = True
                    ReInsuranceCntrl.Visible = False
                Else
                    'If RI2007 Is OFF Then
                    ReInsuranceCntrl.Visible = True
                    ReInsurance2007Cntrl.Visible = False
                End If

                Session(CNStatus) = True
                oWebService = Nothing
            End If
            btnBacktoSummary.Visible = (Session(CNMode) = Mode.View)
        End Sub

        Private Sub btnBacktoSummary_Click(sender As Object, e As EventArgs) Handles btnBacktoSummary.Click
            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty
                Session(CNRiskBackFlag) = "RiskBackFlag"
                Session(CNPolicyBackFlag) = "BackFlag"
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                        'Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        '    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub

        'Protected Sub TabIndex_TabClicked(ByVal Path As String) Handles ctrlTabIndex.TabClicked
        '    Session(CNQuoteInSync) = False
        '    Session.Remove(CNOI) ' Need to clear the CNOI from session in order to read the data using ReadContainerFromXML 
        '    Response.Redirect(Path, False)
        'End Sub


    End Class

End Namespace
