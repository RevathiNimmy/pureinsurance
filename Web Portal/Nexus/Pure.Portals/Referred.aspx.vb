Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    ''' <summary>
    ''' Quote has been referred, show reason
    ''' </summary>
    Partial Class secure_Referred : Inherits Frontend.clsCMSPage
        Dim oQuote As NexusProvider.Quote
        ''' <summary>
        ''' Display the risk dataset output
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DetailedReferralReasons Then

                If Session.Item(CNQuote) IsNot Nothing Then

                    Dim oXMLSource As New XmlDataSource

                    oXMLSource.EnableCaching = False
                    oXMLSource.Data = oQuote.Risks(Session.Item(CNCurrentRiskKey)).XMLDataset

                    'Get all the output elements that have a referred reason
                    oXMLSource.XPath = "//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON]"
                    'oXMLSource.XPath = "DATA_SET/RISK_OBJECTS/" & Session.Item(CNDataModelCode) & "_POLICY_BINDER/" & Session.Item(CNDataModelCode) & "_OUTPUT[@REASON]"

                    grdvReferralReasons.DataSource = oXMLSource
                    grdvReferralReasons.DataBind()

                    If grdvReferralReasons.Rows.Count <= 0 Then
                        grdvReferralReasons.Visible = False
                        lblReferMsg.Visible = True
                    End If
                End If
            Else
                lblReferMsg.Visible = True
            End If

            'Update the oQuote with the latest timestamp to proceed further with links
            oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)

            'Put highest risk key into Session
            For i As Integer = 0 To oQuote.Risks.Count - 1
                oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
            Next

            oWebService.GetHeaderAndRisksByKey(oQuote)

            Session(CNQuote) = oQuote
        End Sub

        ''' <summary>
        ''' Set Session for anonymous quote and Call SaveQuote
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btn_SaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SaveQuote.Click
            If CType(Session(CNIsAnonymous), Boolean) = True Then
                Session(CNRedirectedFor) = "SaveQuote"
            End If
            SaveQuote()
        End Sub

        ''' <summary>
        ''' Redirec to premium display
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btn_SaveRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SaveRisk.Click
            Response.Redirect("~/secure/PremiumDisplay.aspx", False)
        End Sub

        ''' <summary>
        ''' Redirect user to client manager if not anonymous otherwise redirect to find client
        ''' </summary>
        ''' <remarks></remarks>
        Sub SaveQuote()
            'redirecting the user to Client details page if he clicks on Save Quote button
            ''need to check if the Login User is an Agent/Direct Registered Client
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            oQuote = Session(CNQuote)
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
            If oExclusiveLocking.OptionValue = "1" Then
                'On Save Quote unlock the Policy
                UnlockPolicy(oQuote.InsuranceFolderKey, Session(CNBranchCode).ToString)
            End If

            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                If CType(Session(CNIsAnonymous), Boolean) = False Then
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                    End Select

                    oParty = Nothing
                Else
                    'If quote is anonymous then redirect to find client screen
                    Response.Redirect("~/secure/agent/FindClient.aspx")
                End If
            Else
                Response.Redirect(oPortal.ClientStartPage.Trim, False)
            End If

        End Sub
    End Class

End Namespace

