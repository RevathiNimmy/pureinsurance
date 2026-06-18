Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils

Namespace Nexus
    ''' <summary>
    ''' Page load for anonymous quote control
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Controls_NewAnonymousQuote
        Inherits System.Web.UI.UserControl
        Dim oRiskType As New NexusProvider.RiskType
        ''' <summary>
        ''' This will show and hide product selection drop down and new quote button as per the requirement
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

                Dim sAllowedAgent() As String
                Dim oUserDetails As NexusProvider.UserDetails = Session.Item(CNAgentDetails)
                Dim iCounter As Integer = 0
                Dim bMatched As Boolean = False
                Dim UserRoles As String
                Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                Dim oWebService As NexusProvider.ProviderBase
                Dim oAgentProducts As New NexusProvider.ProductCollection
                Dim oAgentToProductLinkOptionSetting As NexusProvider.OptionTypeSetting

                oWebService = New NexusProvider.ProviderManager().Provider

                'Get System Option value for AgentToProductLink
                oAgentToProductLinkOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5088)
                'If System option is ON then GET All products for logged in agent
                If oAgentToProductLinkOptionSetting.OptionValue = "1" Then
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oAgentProducts = oWebService.GetProductByAgent()
                End If

                For Each oProduct As Config.Product In oProducts
                    'Retreive all the roles set for product in web.config
                    UserRoles = oProduct.AllowRole
                    If oProduct.AllowAnonymousQuote = True Then
                        'Roles is available
                        If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True Then
                            'if logged user is agent
                            If CType(Session(CNLoginType), LoginType) = LoginType.Agent Then
                                If oAgentToProductLinkOptionSetting.OptionValue = "1" Then
                                    'Check that product is assigned to agent or not
                                    If FrameWorkFunctions.IsProductAssignedToAgent(oProduct, oAgentProducts) Then
                                        bMatched = True
                                    End If
                                Else
                                    If String.IsNullOrEmpty(oProduct.AllowedAgent.Trim) Then
                                        bMatched = True
                                    Else
                                        sAllowedAgent = oProduct.AllowedAgent.Split(",")
                                        For iCounter = 0 To sAllowedAgent.Length - 1
                                            If sAllowedAgent(iCounter).ToUpper() = oUserDetails.PartyName.ToUpper() Then
                                                bMatched = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            Else
                                'for Direct Customer
                                bMatched = True
                            End If
                        Else
                            'Roles is not available
                            bMatched = False
                        End If
                    End If

                    'if bMatch is True means product will be added
                    If bMatched = True Then
                        ddlProductlst.Items.Add(New ListItem(oProduct.Name, oProduct.ProductCode))
                    End If
                    bMatched = False
                Next

                If ddlProductlst.Items.Count = 1 Then
                    'only one product so hide the drop down list
                    pnlNewQuote.Visible = False
                End If

                If ddlProductlst.Items.Count = 0 Then
                    'no product, so disable New Quote Button.
                    btnNewQuote.Enabled = False
                End If
            Else
                'check if the postback has been triggered by the modal dialog
                If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
                    'get risk type from session
                    oRiskType = Session(CNRiskType)
                    'redirect to first risk screen for the current risk type
                    SetValuesAndRedirect()
                End If
            End If
        End Sub

        ''' <summary>
        ''' On click on new quote button,if there is only single risk associated to product.
        ''' Otherwise user will be redirected for risk selection(On a model page) and after selecting a risk
        ''' user will be redirected to first risk screen
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>

        Protected Sub btnNewQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewQuote.Click

            'find the risk type associated with this product
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfiguration As Nexus.Library.Config.Product
            oProductConfiguration = oPortalConfig.Products.Product(ddlProductlst.SelectedValue)
            'Check RiskTypes for selected product and for more than one RiskType open the Modal dialog Box

            If oProductConfiguration.RiskTypes.Count = 1 Or oProductConfiguration.AllowMultiRisks = False Then
                'there's only one risk type so add this risk type to session
                Dim oRisk As Config.RiskType = oProductConfiguration.RiskTypes.RiskType(0)
                oRiskType.DataModelCode = oRisk.DataModelCode
                oRiskType.Name = oRisk.Name
                oRiskType.Path = oRisk.Path
                oRiskType.RiskCode = oRisk.RiskCode
                Session(CNRiskType) = oRiskType

                'now redirect
                SetValuesAndRedirect()
            ElseIf oProductConfiguration.RiskTypes.Count > 1 And oProductConfiguration.AllowMultiRisks = True Then
                Dim sUrl As String
                'more than one risk type so we need to open the modal dialog
                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                Else
                    sUrl = AppSettings("WebRoot") & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                End If

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
            End If
        End Sub
        ''' <summary>
        ''' Set required session variable, Risk details and redirect to first risk screen or maindetail page
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetValuesAndRedirect()

            Dim sFirstPage As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & ddlProductlst.SelectedItem.Text
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.Product(ddlProductlst.SelectedValue)
            Dim sMainDetail As String = Nothing
            Dim oRisk As NexusProvider.RiskType = Session(CNRiskType)

            Session.Remove(CNQuote)
            If oProductConfig.QuickQuoteConfig = String.Empty Then
                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/" & oNexusConfig.ProductsFolder & "/" & ddlProductlst.SelectedItem.Text & "/" & oRiskType.Path & "/fullquote.config", sMainDetail)
            Else
                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/" & oNexusConfig.ProductsFolder & "/" & ddlProductlst.SelectedItem.Text & "/" & oRiskType.Path & "/quickquote.config", sMainDetail)
            End If
            'newquote is used to reset the quote's value.
            ClearClaims()
            ClearHeader()
            ClearQuote()

            Session(CNIsAnonymous) = True

            If sMainDetail.ToLower = "true" Then
                Response.Redirect(sFolder & "/" & sFirstPage & "?newquote=true&newclient=true", False)
            Else
                Response.Redirect(sFolder & "/" & oRisk.Path & "/" & sFirstPage & "?newquote=true&newclient=true", False)
            End If
        End Sub

    End Class

End Namespace
