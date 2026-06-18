Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Web.Services

Namespace Nexus
    Partial Class Modal_OverridePolicyNumber : Inherits Frontend.clsCMSPage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            txtPolicyNumber.Focus()
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        <WebMethod()> _
    Public Shared Function CheckDuplicateRecord(ByVal sPolicyNo As String) As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim bReturnStatus As Boolean = True
            Dim oPartyColl As NexusProvider.PartyCollection
            Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria
            oPartySearchCriteria.PolicyRef = sPolicyNo
            oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.PC
            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Personal)
            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Corporate)

            oPartyColl = oWebService.FindParty(oPartySearchCriteria)
            If oPartyColl IsNot Nothing AndAlso oPartyColl.Count > 0 Then
                bReturnStatus = False
            End If

            Return bReturnStatus
        End Function

        Protected Sub custValidator_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValidator.ServerValidate
            If txtPolicyNumber.Text.Trim.Length <> 0 Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim bReturnStatus As Boolean = True
                Dim oPartyColl As NexusProvider.PartyCollection
                Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria
                oPartySearchCriteria.PolicyRef = txtPolicyNumber.Text
                oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.PC
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Personal)
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Corporate)

                oPartyColl = oWebService.FindParty(oPartySearchCriteria)
                If oPartyColl IsNot Nothing AndAlso oPartyColl.Count > 0 Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                    Dim PostBackStr As String = "self.parent.setPolicyNumber('" & txtPolicyNumber.Text.Trim & "')" & ";"
                    Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                End If
            Else
                args.IsValid = True
                Dim PostBackStr As String = "self.parent.setPolicyNumber('" & txtPolicyNumber.Text.Trim & "')" & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
        End Sub
    End Class
End Namespace