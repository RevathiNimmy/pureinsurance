Imports System.Xml
Imports System.Xml.XPath
Imports Nexus.library.Config
Imports CMS.Library.Portal
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Navigator
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim tmpHTMLstr1 As String = String.Empty
            Dim sfolder As String = String.Empty
            If Not Page.IsPostBack Then

                Dim oNexusConfig As NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), NexusFrameWork)
                Dim sProductCode As String = CType(Session(CNQuote), NexusProvider.Quote).ProductCode
                Dim oProductConfig As Product = oNexusConfig.Portals.Portal(GetPortalID()).Products.Product(sProductCode)
                Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
                sfolder = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & oRiskType.Path
                Dim sXMLPath As String = Server.MapPath(sfolder & "\")
                Select Case CType(Session.Item(CNQuoteMode), QuoteMode)
                    Case QuoteMode.QuickQuote
                        sXMLPath = sXMLPath & oProductConfig.QuickQuoteConfig
                    Case QuoteMode.FullQuote
                        sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                    Case QuoteMode.MTAQuote ''added by sbhatia on dated 05-march
                        sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                    Case QuoteMode.ReQuote ''added by sbhatia on dated 05-march
                        sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                End Select

                Dim nav As XPathNavigator
                Dim docNav As XPathDocument
                docNav = New XPathDocument(sXMLPath)
                nav = docNav.CreateNavigator
                nav.MoveToRoot()
                'Move to the first tab
                nav.MoveToFirstChild()
                nav.MoveToFirstChild()
                Do
                    'Find the first element.
                    If nav.NodeType = XPathNodeType.Element Then
                        'if children exist
                        If nav.HasChildren Then
                            'Tabholder control appear as tabs using cssclass TabContainer
                            tmpHTMLstr1 = "<div class='TabContainer'>"
                            tmpHTMLstr1 = tmpHTMLstr1 & "<ul>"
                            'Move to the first child.
                            nav.MoveToFirstChild()
                            'Loop through all the children.
                            Do
                                'Check for attributes. the name and the url
                                If nav.HasAttributes Then

                                    If LCase((nav.GetAttribute("maindetails", String.Empty))) = "true" Then
                                        tmpHTMLstr1 = tmpHTMLstr1 & "<li><a href='" & AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & nav.GetAttribute("url", String.Empty) & "'>" _
                                                                               & nav.GetAttribute("name", String.Empty) & "</a></li>"
                                    Else
                                        tmpHTMLstr1 = tmpHTMLstr1 & "<li><a href='" & sfolder & "/" & nav.GetAttribute("url", String.Empty) & "'>" _
                                                                               & nav.GetAttribute("name", String.Empty) & "</a></li>"
                                    End If

                                End If
                            Loop While nav.MoveToNext

                        End If
                    End If
                Loop While nav.MoveToNext
                tmpHTMLstr1 = tmpHTMLstr1 & "</ul>"
                tmpHTMLstr1 = tmpHTMLstr1 & "</div>"
                tabholder.InnerHtml = tmpHTMLstr1
                nav = Nothing
                docNav = Nothing
                oProductConfig = Nothing
                oNexusConfig = Nothing
            End If
        End Sub
    End Class
End Namespace
