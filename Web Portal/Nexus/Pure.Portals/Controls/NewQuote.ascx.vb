Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Namespace Nexus

    Partial Class Controls_NewQuote
        Inherits System.Web.UI.UserControl

        Dim oRiskType As New NexusProvider.RiskType

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oPartyCheck As NexusProvider.BaseParty = Session(CNParty)
            If oPartyCheck IsNot Nothing AndAlso Not String.IsNullOrEmpty(oPartyCheck.BlacklistReasonCode) Then
                Dim sBLackListReason As String = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oPartyCheck.BlacklistReasonCode, "BlackList_Reason")
                btnNewQuote.OnClientClick = "return showBlacklistConfirm('" & sBLackListReason.Replace("'", "\'") & "','" & hfBlacklistConfirmed.ClientID & "');"
            End If
            If Not IsPostBack Or Request("__EVENTARGUMENT") = "SelectBranch" Then
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

                'On refresh clear the dropdown before repopulating
                ddlProductlst.Items.Clear()

                Dim lstSortedProducts As SortedList = New SortedList()
                lstSortedProducts.Add("(Please Select)", "")
                For Each oProduct As Config.Product In oProducts
                    'Retreive all the roles set for product in web.config
                    UserRoles = oProduct.AllowRole
                    'Roles is  available
                    If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True _
                    AndAlso FrameWorkFunctions.IsProductAssignedToUserBranch(oProduct, CType(Session(CNAgentDetails), NexusProvider.UserDetails).AvailableUserProductsByBranch) Then
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
                    'if bMatch is True means product will be added
                    If bMatched = True Then
                        lstSortedProducts.Add(oProduct.Name, oProduct.ProductCode)
                    End If
                    bMatched = False
                Next

                'Bind sorted product list
                ddlProductlst.DataSource = lstSortedProducts
                ddlProductlst.DataValueField = "Value"
                ddlProductlst.DataTextField = "Key"
                ddlProductlst.DataBind()

                If ddlProductlst.Items.Count = 1 Then
                    'only one product so hide the drop down list
                    lblSelectProduct.Visible = False
                    ddlProductlst.Visible = False
                ElseIf ddlProductlst.Items.Count = 0 Then
                    lblSelectProduct.Visible = False
                    ddlProductlst.Visible = False
                    btnNewQuote.Visible = False
                Else
                    lblSelectProduct.Visible = True
                    ddlProductlst.Visible = True
                    btnNewQuote.Visible = True
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

        Protected Sub btnNewQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewQuote.Click
            'newquote is used to reset the quote's value.
            ClearClaims()
            ClearHeader()
            ClearQuote()
            Session.Remove(CNRiskType)
            Session(CNUnAllocatedClaimPayment) = Nothing
            If ddlProductlst.Items.Count <= 0 OrElse ddlProductlst.SelectedValue.Trim() = String.Empty Then
                Exit Sub
            End If
            'find the risk type associated with this product
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfiguration As Nexus.Library.Config.Product
            oProductConfiguration = oPortalConfig.Products.Product(ddlProductlst.SelectedValue)

            Dim oPartyCheck As NexusProvider.BaseParty = TryCast(Session(CNParty), NexusProvider.BaseParty)
            If oPartyCheck IsNot Nothing AndAlso Not String.IsNullOrEmpty(oPartyCheck.BlacklistReasonCode) Then
                If hfBlacklistConfirmed.Value <> "yes" Then
                    Exit Sub
                End If
            End If
            hfBlacklistConfirmed.Value = String.Empty
            'count the risk minus IsMandatory=true
            Dim iRiskCount As Integer = 0
            For Each oRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                If oRisk.IsMandatory = False Then
                    iRiskCount += 1
                End If
            Next

            'Check RiskTypes for selected product and for more than one RiskType open the Modal dialog Box
            If oProductConfiguration.RiskTypes.Count = 1 AndAlso iRiskCount = 0 Then
                'if only risk is there and it is mandatory 
                Dim oRisk As Nexus.Library.Config.RiskType = oProductConfiguration.RiskTypes.RiskType(0)
                ''set up the risk type object from the details in config
                oRiskType.DataModelCode = oRisk.DataModelCode
                oRiskType.Name = oRisk.Name
                oRiskType.Path = oRisk.Path
                oRiskType.RiskCode = oRisk.RiskCode
                Session(CNRiskType) = oRiskType
                'now redirect
                SetValuesAndRedirect()
            ElseIf iRiskCount = 1 Or oProductConfiguration.AllowMultiRisks = False Then
                'there's only one risk type so add this risk type to session
                For Each oRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                    If oRisk.IsMandatory = False Then
                        ''set up the risk type object from the details in config
                        oRiskType.DataModelCode = oRisk.DataModelCode
                        oRiskType.Name = oRisk.Name
                        oRiskType.Path = oRisk.Path
                        oRiskType.RiskCode = oRisk.RiskCode
                        Session(CNRiskType) = oRiskType
                        Exit For
                    End If
                Next

                'now redirect
                SetValuesAndRedirect()
            ElseIf iRiskCount > 1 AndAlso oProductConfiguration.AllowMultiRisks = True Then
                'more than one risk type so we need to open the modal dialog
                Dim sUrl As String = String.Empty
                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                Else
                    sUrl = AppSettings("WebRoot") & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                End If

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
            End If
        End Sub

        Private Sub SetValuesAndRedirect()
            Dim sFirstPage As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & ddlProductlst.SelectedItem.Text
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.Product(ddlProductlst.SelectedValue)
            Session.Remove(CNQuote)
            Dim sMainDetail As String = Nothing
            Dim oRisk As NexusProvider.RiskType = Session(CNRiskType)
            If oProductConfig.QuickQuoteConfig = String.Empty Then
                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Products/" & ddlProductlst.SelectedItem.Text & "/" & oRiskType.Path & "/fullquote.config", sMainDetail)
            Else
                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Products/" & ddlProductlst.SelectedItem.Text & "/" & oRiskType.Path & "/quickquote.config", sMainDetail)
            End If

            If sMainDetail.ToLower = "true" Then
                Response.Redirect(sFolder & "/" & sFirstPage & "?newquote=true", False)
            Else
                Response.Redirect(sFolder & "/" & oRisk.Path & "/" & sFirstPage & "?newquote=true", False)
            End If
        End Sub
    End Class
End Namespace
