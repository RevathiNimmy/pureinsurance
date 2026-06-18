Imports System.Resources
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils

Namespace Nexus

    Partial Class Controls_ClaimHeader : Inherits System.Web.UI.UserControl
        Dim m_sPolicyNumber As String
        Dim m_sCurrency As String
        Dim m_sDates As String
        Dim m_sInsuredName As String
        Dim m_sStatus As String
        Dim m_sClaimNumber As String
        Dim sReturnUrl As String = String.Empty
        Dim sFolder As String = String.Empty
        Dim sFirstPage As String = String.Empty

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            lblDates.Text = Dates
            lblStatus.Text = Status
            Select Case EventMode
                Case Mode.Add
                    lblInsuredNameTitle.Visible = False
                    lblInsuredName.Visible = False
                Case Mode.Edit, Mode.View
                    lblInsuredNameTitle.Visible = True
                    lblInsuredName.Visible = True
            End Select
            lblInsuredName.Text = InsuredName
            lblCurrency.Text = Currency
            lblClaimNumber.Text = ClaimNumber.Trim.ToUpper

            Dim oInsurerDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            If oInsurerDetails.Insurer IsNot Nothing Then
                lblAgentName.Text = oInsurerDetails.Insurer.ContactName
            End If


            Dim WebRoot As String = AppSettings("WebRoot")
            sReturnUrl = Request.Path.Replace(WebRoot, "~/")
            Session(CNReturnURL) = sReturnUrl

            Me.hypFinancialdetails.NavigateUrl = "~/Claims/FinancialDetails.aspx?ReturnUrl=" + sReturnUrl
            Me.hypEvents.NavigateUrl = "~/secure/EventList.aspx?ReturnUrl=" + sReturnUrl

            If Session(CNClaimNumber).ToString.Trim.ToUpper <> "TBA" Or Session(CNMode) = Mode.EditClaim Then
                Me.liFinancialDetails.Visible = True
            End If

            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
            lblPolicyNumber.Text = oClaimQuote.InsuranceFileRef
            If oClaimQuote.BusinessTypeCode = "DIRECT" Then
                liAgent.Visible = False
            Else
                liAgent.Visible = True
            End If
            oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oPortal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            If (Session(CNClaimNumber).ToString.Trim.ToUpper <> "TBA" Or Session(CNMode) = Mode.EditClaim) AndAlso String.IsNullOrEmpty(oOpenClaim.RiskType) = False Then
                Dim sFirstPage As String = Nothing
                liRisk.Visible = True
                lblRisk.Text = oOpenClaim.RiskType
                If Session(CNClaimNumber) IsNot Nothing Then
                    If (Session(CNClaimNumber).ToString.Trim.ToUpper <> "TBA" Or Session(CNMode) = Mode.EditClaim) AndAlso CheckValidProduct(oClaimQuote.ProductCode) = True Then
                        hypRisk.Visible = True
                    End If
                End If
            End If
        End Sub

        Public ReadOnly Property PolicyNumber() As String
            Get
                Return Session(CNPolicyNumber)
            End Get
        End Property

        Public ReadOnly Property Currency() As String
            Get
                Return GetCurrencyForCode(Session(CNCurrenyCode))
            End Get
        End Property

        Public ReadOnly Property Dates() As String
            Get
                Return Session(CNDate_Header)
            End Get
        End Property

        Public ReadOnly Property InsuredName() As String
            Get
                Return Session(CNInsurer_Header)
            End Get
        End Property

        Public ReadOnly Property Status() As String
            Get
                Return Session(CNStatus)
            End Get
        End Property

        Public ReadOnly Property ClaimNumber() As String
            Get
                Return Session(CNClaimNumber)
            End Get
        End Property

        Public ReadOnly Property EventMode() As Mode
            Get
                Return CType(Session(CNMode), Mode)
            End Get
        End Property

        Protected Sub hypRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hypRisk.Click

            Dim oNexusConfig As Config.NexusFrameWork
            Dim oPortal As Nexus.Library.Config.Portal
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oPortal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            If oOpenClaim.RiskType.Trim() <> String.Empty Then
                Dim sFirstPage As String = Nothing
                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oPortal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oRiskType As New NexusProvider.RiskType
                Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.Product(oClaimQuote.ProductCode)
                Dim oRisk As Config.RiskType = oProductConfig.RiskTypes.RiskType(oOpenClaim.RiskType.Trim())
                Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/" & oRisk.Path & "/"
                If IO.File.Exists(Server.MapPath(sFolder & "/fullquote.config")) Then

                    Dim sMainDetails As String = Nothing
                    Dim sXMLPath As String = Server.MapPath(sFolder & "/fullquote.config")

                    Dim xmlds As New XmlDataSource
                    xmlds.DataFile = sXMLPath
                    xmlds.EnableCaching = False

                    Dim Navigator As XPathNavigator
                    Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                    Navigator = Doc.CreateNavigator()
                    Dim i As XPathNodeIterator

                    i = Navigator.Select("/screens/screen/tab[1]")

                    While (i.MoveNext)
                        sFirstPage = i.Current.GetAttribute("url", String.Empty)
                        sMainDetails = i.Current.GetAttribute("name", String.Empty)
                    End While
                    If String.IsNullOrEmpty(sMainDetails) = False Then
                        If sMainDetails.Trim.ToUpper = "MAINDETAILS" Then
                            i = Navigator.Select("/screens/screen/tab[2]")
                            While (i.MoveNext)
                                sFirstPage = i.Current.GetAttribute("url", String.Empty)
                            End While
                        End If
                    End If

                    If Session(CNClaimNumber) IsNot Nothing Then
                        If String.IsNullOrEmpty(Session(CNClaimNumber).ToString) = False Then
                            hypRisk.Visible = True
                            Session(CNCurrentOI) = Session(CNOI)
                            Session(CNCurrentMode) = Session(CNMode)
                            Session(CNMode) = Mode.Review
                            Dim oQuote As NexusProvider.Quote = Nothing
                            oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimQuote.InsuranceFileKey)

                            'Put highest risk key into Session
                            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                oWebservice.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote, oQuote.BranchCode)
                            Next

                            oWebservice.GetHeaderAndRisksByKey(oQuote)

                            Session(CNQuote) = oQuote

                            Session(CNCurrenyCode) = oQuote.CurrencyCode

                            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                            GetDataSetDefinition()
                            Session(CNMTAType) = Nothing
                            Session(CNQuoteInSync) = False
                            Session.Remove(CNOI)
                            Session(CNQuoteMode) = QuoteMode.FullQuote

                            'set up the risk type object from the details in config
                            oRiskType.DataModelCode = oRisk.DataModelCode
                            oRiskType.Name = oRisk.Name
                            oRiskType.RiskCode = oRisk.RiskCode
                            oRiskType.Path = oRisk.Path
                            Session(CNRiskType) = oRiskType
                            Response.Redirect(sFolder & "/" & sFirstPage, False)
                            'hypRisk.PostBackUrl = sFolder & "/" & sFirstPage
                        End If
                    End If
                End If
            End If
        End Sub
        Function CheckValidProduct(ByVal v_sProductCode As String) As Boolean
            'Check the product where it is configurent in Nexus or not
            Dim bReturn As Boolean = False
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            For Each oProduct As Config.Product In oProducts
                If v_sProductCode.Trim.ToUpper = oProduct.ProductCode.Trim.ToUpper Then
                    bReturn = True
                End If
            Next
            Return bReturn
        End Function
    End Class

End Namespace
