Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports CMS.Library
Imports System.Xml.XPath
Imports System.Web.HttpContext
Imports System.Xml
Imports SiriusFS.SAM.Client
Imports System.IO
Imports System.Globalization
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Namespace Nexus
    Partial Class secure_NewQuote
        Inherits Frontend.clsCMSPage
        Dim oRiskType As New NexusProvider.RiskType
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
            End If

            Dim sAllowedAgent() As String
            Dim oUserDetails As NexusProvider.UserDetails = Session.Item(CNAgentDetails)
            Dim iCounter As Integer = 0
            Dim bMatched As Boolean = False
            Dim UserRoles As String
            Dim oProducts As Config.Products = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim oWebService As NexusProvider.ProviderBase
            Dim oAgentProducts As New NexusProvider.ProductCollection
            Dim oAgentToProductLinkOptionSetting As NexusProvider.OptionTypeSetting
            Dim oControl As Object
            Dim dt As New DataTable
            dt.Columns.Add("Name")
            dt.Columns.Add("ProductCode")
            dt.Columns.Add("ProductImage")

            oWebService = New NexusProvider.ProviderManager().Provider

            'Get System Option value for AgentToProductLink
            oAgentToProductLinkOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5071)

            'If System option is ON then GET All products for logged in agent
            If oAgentToProductLinkOptionSetting.OptionValue = "1" Then
                oWebService = New NexusProvider.ProviderManager().Provider
                oAgentProducts = oWebService.GetProductByAgent()
            End If

            For Each oProduct As Config.Product In oProducts
                'Retreive all the roles set for product in web.config
                UserRoles = oProduct.AllowRole
                'If oProduct.AllowAnonymousQuote = True Then
                'Roles is available
                If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True _
                AndAlso FrameWorkFunctions.IsProductAssignedToUserBranch(oProduct, CType(Session(CNAgentDetails), NexusProvider.UserDetails).AvailableUserProductsByBranch) Then
                    'if logged user is agent
                    If CType(Session(CNLoginType), LoginType) = LoginType.Agent Then
                        If oAgentToProductLinkOptionSetting.OptionValue = "1" Then
                            'Check that product is assigned to agent or not
                            'Uncomment with new build
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



                'if bMatch is True means product should be visible
                If bMatched = True Then


                    'if bMatch is True means product should be visible
                    If bMatched = True Then


                        Dim dr1 As DataRow
                        dr1 = dt.NewRow()
                        dr1("Name") = oProduct.Name
                        dr1("ProductCode") = oProduct.ProductCode.Trim
                        dr1("ProductImage") = "~/App_Themes/Products/" + oProduct.ProductCode.Trim + ".jpg"
                        dt.Rows.Add(dr1)

                    End If
                End If
                bMatched = False
            Next
            If UserCanDoTask("NewBusiness") Then
                If dt.Rows.Count = 1 And Not Page.IsPostBack Then
                    AddNewQuote(dt.Rows(0).Item("ProductCode"))
                Else
                    grdProducts.DataSource = dt
                    grdProducts.DataBind()
                End If

                'check if the postback has been triggered by the modal dialog
                If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
                    'get risk type from session
                    oRiskType = Session(CNRiskType)
                    'redirect to first risk screen for the current risk type
                    If Not String.IsNullOrEmpty(Session("ProductCode")) Then
                        SetValuesAndRedirect(Session("ProductCode").ToString())
                    End If
                End If

            Else

                grdProducts.DataSource = Nothing
                grdProducts.DataBind()


                'check if the postback has been triggered by the modal dialog
                If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
                    'get risk type from session
                    oRiskType = Session(CNRiskType)
                    'redirect to first risk screen for the current risk type
                    If Not String.IsNullOrEmpty(Session("ProductCode")) Then
                        SetValuesAndRedirect(Session("ProductCode").ToString())
                    End If

                End If
            End If
            'End If

        End Sub
        Private Sub AddNewQuote(ByVal sProductCode As String)
            'Response.Redirect("~/Secure/Agent/FindClient.aspx")
            'find the risk type associated with this product
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfiguration As Nexus.Library.Config.Product


            oProductConfiguration = oPortalConfig.Products.Product(sProductCode)
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
                SetValuesAndRedirect(sProductCode)
            ElseIf oProductConfiguration.RiskTypes.Count > 1 And oProductConfiguration.AllowMultiRisks = True Then
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

        ''' <summary>
        ''' Set required session variable, Risk details and redirect to first risk screen or maindetail page
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetValuesAndRedirect(ByVal sProductCode As String)

            Dim sFirstPage As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.Product(sProductCode)
            Dim sMainDetail As String = Nothing
            Dim oRisk As NexusProvider.RiskType = Session(CNRiskType)
            Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name

            Session.Remove(CNQuote)
            If oProductConfig.QuickQuoteConfig = String.Empty Then
                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Products/" & oProductConfig.Name & "/" & oRiskType.Path & "/fullquote.config", sMainDetail)
            Else
                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Products/" & oProductConfig.Name & "/" & oRiskType.Path & "/quickquote.config", sMainDetail)
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

        Protected Sub grdProducts_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdProducts.RowCommand
            AddNewQuote(e.CommandName)
            Session("ProductCode") = e.CommandName.ToString().Trim()
        End Sub
    End Class
End Namespace

