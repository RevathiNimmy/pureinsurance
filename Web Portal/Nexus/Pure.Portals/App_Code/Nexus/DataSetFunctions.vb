Imports SiriusFS.SAM.Client
Imports System.Xml.XPath
Imports System.Web.HttpContext
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Library.Config
Imports CMS.Library.Portal
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports System.Globalization.CultureInfo
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Data
Imports System.Web.Configuration
Imports Nexus.Utils
Imports Nexus
Imports System.Xml.XmlReader
Imports NexusProvider.Quote
Imports System.Configuration
Imports System
Imports System.Linq
Imports Nexus.Constants.Constant
Imports System.Net.Mail
Imports System.IO
Imports System.Xml.Linq

Namespace Nexus

    Public Module DataSetFunctions

        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Public sSummaryOfRisk As String = String.Empty
        Public sSummaryOfCover As String = String.Empty
        Public sSummaryOfRiskURL As String = String.Empty
        Public sSummaryOfCoverURL As String = String.Empty
        Public sReferScreen As String = String.Empty
        Public sDeclineScreen As String = String.Empty
        Public sReferScreenURL As String = String.Empty
        Public sDeclineScreenURL As String = String.Empty

        Public Property SummaryOfCover() As String
            Get
                Return sSummaryOfCover
            End Get
            Set(ByVal value As String)
                sSummaryOfCover = value
            End Set
        End Property

        Public Property SummaryOfCoverURL() As String
            Get
                Return sSummaryOfCoverURL
            End Get
            Set(ByVal value As String)
                sSummaryOfCoverURL = value
            End Set
        End Property

        ''' <summary>
        ''' To check if risk is referred or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckRefer() As Boolean
            Dim bReturn As Boolean = False
            Dim oProduct As Config.Product
            Dim oNexusFramework As Config.NexusFrameWork
            If Current.Session.Item(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oProduct = oNexusFramework.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCount).XMLDataset IsNot Nothing Then
                        Dim Doc As New XmlDocument
                        Using srDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                            Dim xmlTR As New XmlTextReader(srDataset)
                            Doc.Load(xmlTR)
                            xmlTR.Close()
                        End Using
                        'Check for Refer
                        Dim oNode As XmlNode = Doc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON]")
                        If oNode IsNot Nothing Then
                            bReturn = True
                            If Current.Session(CNRiskType) Is Nothing Then
                                Dim oRiskType As Config.RiskType
                                If oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskTypeCode.Trim)
                                Else
                                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskCode.Trim)
                                End If

                                Dim oRisk As New NexusProvider.RiskType
                                oRisk.DataModelCode = oRiskType.DataModelCode
                                oRisk.Name = oRiskType.Name
                                oRisk.Path = oRiskType.Path
                                oRisk.RiskCode = oRiskType.RiskCode
                                Current.Session(CNRiskType) = oRisk
                            End If
                            Exit For
                        End If
                    ElseIf oQuote.Risks(iCount).StatusCode.Trim.ToUpper = RiskStatus.Referred OrElse oQuote.Risks(iCount).StatusCode.Trim.ToUpper = "REFERRED" Then
                        bReturn = True
                        Exit For
                    End If
                Next
            End If
            Return bReturn
        End Function

        ''' <summary>
        ''' To check if risk is declined or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckDecline() As Boolean
            Dim bReturn As Boolean = False
            Dim oProduct As Config.Product
            Dim oNexusFramework As Config.NexusFrameWork
            If Current.Session.Item(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oProduct = oNexusFramework.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1

                    If oQuote.Risks(iCount).XMLDataset IsNot Nothing Then
                        Using srDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                            Using xmlTR As New XmlTextReader(srDataset)
                                Dim Doc As New XmlDocument

                                Doc.Load(xmlTR)
                                xmlTR.Close()

                                'Check for Decline
                                Dim oNode As XmlNode = Doc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@DECLINE_REASON]")
                                If oNode IsNot Nothing Then
                                    bReturn = True
                                    If Current.Session(CNRiskType) Is Nothing Then
                                        Dim oRiskType As Config.RiskType
                                        If oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskTypeCode.Trim)
                                        Else
                                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskCode.Trim)
                                        End If

                                        Dim oRisk As New NexusProvider.RiskType
                                        oRisk.DataModelCode = oRiskType.DataModelCode
                                        oRisk.Name = oRiskType.Name
                                        oRisk.Path = oRiskType.Path
                                        oRisk.RiskCode = oRiskType.RiskCode
                                        Current.Session(CNRiskType) = oRisk
                                    End If
                                    Exit For
                                End If
                            End Using
                        End Using
                    ElseIf oQuote.Risks(iCount).StatusCode.Trim.ToUpper = RiskStatus.Declined OrElse oQuote.Risks(iCount).StatusCode.Trim.ToUpper = "DECLINED" Then
                        bReturn = True
                        Exit For
                    End If
                Next
            End If
            Return bReturn
        End Function
        Public Function CheckAndCalculateRoundOff() As Decimal
            'This method check the setting of the Round Off and return the Values
            Dim sRoundOff As String = String.Empty
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            oWebService.GetHeaderAndRisksByKey(oQuote)

            sRoundOff = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RoundOffToZero, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()
            Dim m_cRoundOffAmount As Decimal
            If sRoundOff.Equals("1") Then
                m_cRoundOffAmount = Format((Math.Round(Convert.ToDecimal(oQuote.GrossTotal), 0) - oQuote.GrossTotal) + oQuote.FeeTotal + oQuote.NetTotal + oQuote.TaxTotal, "0.00")
                If (Math.Round(Convert.ToDecimal(oQuote.GrossTotal), 0) - oQuote.GrossTotal) = -0.5 Then
                    m_cRoundOffAmount = m_cRoundOffAmount + 1
                Else
                    m_cRoundOffAmount = m_cRoundOffAmount
                End If
            Else
                If oQuote.PaymentMethod IsNot Nothing AndAlso oQuote.PaymentMethod.Trim.ToUpper = "PAYNOW" Then
                    If Current.Session(CNQuoteMode) = QuoteMode.FullQuote And Current.Session(CNMode) = Mode.Edit Then
                        m_cRoundOffAmount = oQuote.GrossTotal
                    ElseIf Current.Session(CNMode) = Mode.View Then
                        m_cRoundOffAmount = oQuote.GrossTotal
                    Else
                        m_cRoundOffAmount = Current.Session(CNAmountToPay)
                    End If
                Else
                    If oQuote.InsuranceFileTypeCode.Trim().ToUpper() = "RENEWAL" And Current.Session(CNMode) <> Mode.View And Current.Session(CNIsTrueMonthlyPolicy) = True Then
                        m_cRoundOffAmount = oWebService.GetRenewalAmountToFinance(oQuote.InsuranceFileKey)
                    Else
                        m_cRoundOffAmount = oQuote.GrossTotal
                    End If
                End If

            End If

            Current.Session(CNHasPremiumUpdated) = True

            Return m_cRoundOffAmount
        End Function
        Public Function ClaimGetDataSetDefinition() As String

            If Current.Session.Item(CNDataModelCode) Is Nothing Then

                'Read DataModelCode from DataSet if it's not already in session
                Dim sDataModelCode As String = String.Empty
                Dim Doc As XPathDocument = New XPathDocument(New IO.StringReader(Current.Session(CNDataSet)))
                Dim Navigator As XPathNavigator
                Navigator = Doc.CreateNavigator()

                Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

                While (i.MoveNext)
                    sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
                End While

                Current.Session.Item(CNDataModelCode) = sDataModelCode.ToUpper

            End If

            Dim sDataSetDefinition As String = String.Empty

            Dim sBranchCode As String = GetBranchCode()
            ' END RCD
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                sDataSetDefinition = oWebService.GetDatasetDefinition(Current.Session.Item(CNDataModelCode), sBranchCode, )
            Finally
                oWebService = Nothing
            End Try
            Return sDataSetDefinition

        End Function
        ''' <summary>
        ''' Retrieve the dataset definition for the provided DataModelCode
        ''' </summary>
        ''' <param name="v_sDataModelCode">DataModelCode of the dataset definition to be retrieved,
        ''' if no DataModelCode is provided an attempt wil be made to retrieve the DataModelCode
        ''' from the current dataset</param>
        ''' <returns>xml string representation of the dataset definition</returns>
        ''' <remarks></remarks>
        Public Function GetDataSetDefinition(Optional ByVal v_sDataModelCode As String = Nothing) As String
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            If v_sDataModelCode Is Nothing Then

                'Read DataModelCode from DataSet if it's not been passed
                If oQuote.Risks.Count > 0 AndAlso oQuote.Risks(0).XMLDataset IsNot Nothing Then
                    Dim Doc As XPathDocument = New XPathDocument(New IO.StringReader(oQuote.Risks(0).XMLDataset))
                    Dim Navigator As XPathNavigator
                    Navigator = Doc.CreateNavigator()

                    Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

                    While (i.MoveNext)
                        v_sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
                    End While

                    Current.Session.Item(CNDataModelCode) = v_sDataModelCode.ToUpper

                End If
            End If
            Dim sDataSetDefinition As String = String.Empty
            If String.IsNullOrEmpty(v_sDataModelCode) AndAlso HttpContext.Current.Session(CNDataModelCode) IsNot Nothing Then
                v_sDataModelCode = HttpContext.Current.Session(CNDataModelCode)
            End If

            If Not String.IsNullOrEmpty(v_sDataModelCode) Then
                'Load from file while the SAM method is broken
                Dim oNexusConfig As NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), NexusFrameWork)
                Dim oProductConfig As Product = oNexusConfig.Portals.Portal(GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name & "/"
                '---------------------------------------------
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    sDataSetDefinition = oWebService.GetDatasetDefinition(v_sDataModelCode, sBranchCode)
                Finally
                    oWebService = Nothing
                End Try
            End If
            Return sDataSetDefinition

        End Function

        ''' <summary>
        ''' Read all controls within a provided conntrol container and identify the risk controls and
        ''' populate from the current risk dataset
        ''' </summary>
        ''' <param name="oDoc">An XMLDocument object of the current SAM dataset</param>
        ''' <param name="v_oContainer">The control to be searched for risk controls to load data to,
        ''' this is usually the masterpage container</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <param name="sender">The object that made the request to the function, usually content placeholder</param>
        ''' <param name="InitializeOnly">Initialize the risk controls only, e.g set attributes,
        ''' hookup events but don't read the risk data into the control, mainly used on postback</param>
        ''' <remarks>This procedure is marked private because it should only ever be called by itself of from
        ''' the ReadContainerFromXML procedure</remarks>
        Public Sub LoadRiskControls(ByRef oDoc As XmlDocument,
                                ByVal v_oContainer As Control,
                                ByVal v_sOI As String,
                                ByVal sender As Object,
                                Optional ByVal InitializeOnly As Boolean = False,
                                Optional ByVal IsPostBack As Boolean = False)

            Dim oControl As Object
            Dim sControlName() As String 'Will be 2 or 3 elements, ELEMENT__ATTRIBUTE(__CONDITION)
            Dim sControlValue As String = String.Empty
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())

            For Each oControl In v_oContainer.Controls
                If oControl.id IsNot Nothing Then

                    sControlName = Regex.Split(oControl.ID.ToUpper(), "__")
                    sControlValue = String.Empty

                    If sControlName.Length > 1 Then
                        Select Case sControlName(0)

                            Case "POLICYHEADER"
                                'It should not read the oQuote again on POSTBACK
                                If IsPostBack = False Then
                                    Select Case oControl.GetType.Name.ToUpper
                                        'if controls are placed inside these container
                                        Case "PANEL"
                                            Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                            Dim Doc As New XmlDocument

                                            Doc.Load(xmlTR)
                                            xmlTR.Close()
                                            LoadRiskControls(Doc, oControl, v_sOI, sender, InitializeOnly)
                                        Case "UPDATEPANEL"
                                            Dim oCtrl As Control
                                            For Each oCtrl In oControl.Controls
                                                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                                Dim Doc As New XmlDocument

                                                Doc.Load(xmlTR)
                                                xmlTR.Close()
                                                LoadRiskControls(Doc, oCtrl, v_sOI, sender, InitializeOnly)
                                            Next
                                        Case "HTMLGENERICCONTROL"
                                            Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                            Dim Doc As New XmlDocument

                                            Doc.Load(xmlTR)
                                            xmlTR.Close()
                                            LoadRiskControls(Doc, oControl, v_sOI, sender, InitializeOnly)

                                        Case Else

                                            Select Case sControlName(1)

                                                Case "AGENT"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "HIDDENFIELD"
                                                            If oQuote IsNot Nothing AndAlso oQuote.Agent IsNot Nothing AndAlso oQuote.Agent.Trim <> "0" Then
                                                                CType(oControl, HiddenField).Value = oQuote.Agent
                                                                'Population of the cover not book if Agent key is available
                                                                CType(sender, BaseRisk).FillCoverNoteBook()
                                                            End If
                                                    End Select

                                                Case "AGENTCODE"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing AndAlso oQuote.AgentCode IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.AgentCode
                                                                'Enable of the AlternateRef based on the agent code
                                                                CType(sender, BaseRisk).EnableAlternateRef()
                                                                CType(sender, BaseRisk).AgencyCancelled()
                                                            End If
                                                            CType(oControl, TextBox).Attributes.Add("readonly", "readonly")
                                                            'Enable of the Lapse button
                                                            CType(sender, BaseRisk).EnableBtnLapseQuote()
                                                    End Select

                                                Case "CONTACT_NAME"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"

                                                            If oQuote IsNot Nothing AndAlso oQuote.Agent IsNot Nothing Then
                                                                If (oQuote.Agent IsNot Nothing AndAlso oQuote.Agent <> "" AndAlso oQuote.Agent <> 0) Then
                                                                    FillContactedDropDown(oQuote.Agent, CType(oControl, DropDownList))
                                                                End If
                                                                If (oQuote.ContactUserKey <> "0") Then

                                                                    CType(oControl, DropDownList).SelectedValue = oQuote.ContactUserKey
                                                                End If
                                                            End If
                                                    End Select

                                                Case "AGENT"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "HIDDENFIELD"
                                                            If oQuote IsNot Nothing AndAlso oQuote.HandlerCode IsNot Nothing Then
                                                                CType(oControl, HiddenField).Value = oQuote.Agent
                                                            End If
                                                    End Select

                                                Case "SUBAGENTS"
                                                    'for sub agents control
                                                    If Current.Session.Item(CNSubAgents) Is Nothing AndAlso
                                                    oControl.Visible = True Then
                                                        Dim oSubAgentResponse As New NexusProvider.SubAgentCollection
                                                        oWebService = New NexusProvider.ProviderManager().Provider
                                                        oSubAgentResponse = oWebService.GetSubAgents(oQuote.InsuranceFileKey)
                                                        Current.Session.Add(CNSubAgents, oSubAgentResponse)
                                                    End If

                                                Case "PRODUCT"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "DROPDOWNLIST"
                                                            CType(sender, BaseRisk).FillProduct()
                                                            If oQuote IsNot Nothing AndAlso oQuote.ProductCode IsNot Nothing Then
                                                                CType(oControl, DropDownList).SelectedValue = oQuote.ProductCode
                                                                CType(oControl, DropDownList).Enabled = False
                                                            End If
                                                    End Select

                                                Case "INSUREDNAME"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing AndAlso oQuote.InsuredName IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.InsuredName
                                                            End If
                                                    End Select

                                                Case "FREQUENCY"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.FrequencyCode IsNot Nothing Then
                                                                CType(oControl, DropDownList).SelectedValue = oQuote.FrequencyCode
                                                            End If

                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.FrequencyCode IsNot Nothing Then
                                                                oControl.Value = oQuote.FrequencyCode
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.FrequencyCode IsNot Nothing Then
                                                                oControl.Value = oQuote.FrequencyCode
                                                            End If
                                                    End Select

                                                Case "HANDLER"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "HIDDENFIELD"
                                                            If oQuote IsNot Nothing AndAlso oQuote.HandlerCode IsNot Nothing Then
                                                                CType(oControl, HiddenField).Value = oQuote.AccountHandlerCnt
                                                            End If

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing AndAlso oQuote.HandlerCode IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.AccountHandlerName
                                                            End If
                                                            CType(oControl, TextBox).Attributes.Add("readonly", "readonly")

                                                    End Select
                                                Case "HANDLERCODE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "HIDDENFIELD"
                                                            If oQuote IsNot Nothing AndAlso oQuote.HandlerCode IsNot Nothing Then
                                                                CType(oControl, HiddenField).Value = oQuote.HandlerCode
                                                            End If
                                                    End Select
                                                Case "ALTERNATEREF"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing AndAlso oQuote.AlternativeRef IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.AlternativeRef
                                                            End If
                                                    End Select

                                                Case "ANALYSISCODE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.AnalysisCode IsNot Nothing Then
                                                                oControl.Value = oQuote.AnalysisCode
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.AnalysisCode IsNot Nothing Then
                                                                oControl.Value = oQuote.AnalysisCode
                                                            End If
                                                    End Select
                                                Case "REGARDING"

                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing AndAlso oQuote.Regarding IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.Regarding
                                                            End If
                                                    End Select

                                                Case "BUSINESSTYPE"

                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.BusinessTypeCode IsNot Nothing Then
                                                                CType(oControl, DropDownList).SelectedValue = oQuote.BusinessTypeCode
                                                            End If

                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.BusinessTypeCode IsNot Nothing Then
                                                                CType(oControl, NexusProvider.LookupList).Value = oQuote.BusinessTypeCode
                                                            End If

                                                            'Disable for Agnet Login and Enable for Employee Login
                                                            If Current.Session(CNLoginType) = LoginType.Agent Then
                                                                Dim oUserDetails As NexusProvider.UserDetails = Current.Session(CNAgentDetails)
                                                                If oUserDetails IsNot Nothing Then
                                                                    If oUserDetails.Key > 0 Then
                                                                        oControl.enabled = False
                                                                        CType(sender, BaseRisk).FillCoverNoteBook()
                                                                    Else
                                                                        oControl.enabled = True
                                                                    End If
                                                                End If
                                                            ElseIf Current.Session(CNLoginType) = LoginType.Customer _
                                                            Or (Current.Session(CNIsAnonymous) IsNot Nothing AndAlso Current.Session(CNIsAnonymous) = True) Then
                                                                'Disable to Customer login
                                                                oControl.enabled = False
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.BusinessTypeCode IsNot Nothing Then
                                                                CType(oControl, NexusProvider.LookupListV2).Value = oQuote.BusinessTypeCode
                                                            End If

                                                            'Disable for Agnet Login and Enable for Employee Login
                                                            If Current.Session(CNLoginType) = LoginType.Agent Then
                                                                Dim oUserDetails As NexusProvider.UserDetails = Current.Session(CNAgentDetails)
                                                                If oUserDetails IsNot Nothing Then
                                                                    If oUserDetails.Key > 0 Then
                                                                        oControl.enabled = False
                                                                        CType(sender, BaseRisk).FillCoverNoteBook()
                                                                    Else
                                                                        oControl.enabled = True
                                                                    End If
                                                                End If
                                                            ElseIf Current.Session(CNLoginType) = LoginType.Customer _
                                                            Or (Current.Session(CNIsAnonymous) IsNot Nothing AndAlso Current.Session(CNIsAnonymous) = True) Then
                                                                'Disable to Customer login
                                                                oControl.enabled = False
                                                            End If
                                                    End Select

                                                Case "BRANCH"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            'Filling of the branch list
                                                            CType(sender, BaseRisk).FillBanches()
                                                            If oQuote IsNot Nothing AndAlso oQuote.BranchCode IsNot Nothing Then
                                                                If CType(oControl, DropDownList).Items.Count > 0 Then
                                                                    CType(oControl, DropDownList).SelectedValue = oQuote.BranchCode
                                                                    Current.Session(CNTransBranchCode) = oQuote.BranchCode
                                                                    'filling of the sub-branch and currency list
                                                                    CType(sender, BaseRisk).FillSubBranches(oQuote.BranchCode)
                                                                    CType(sender, BaseRisk).FillCurrency()
                                                                End If
                                                            End If
                                                    End Select

                                                Case "SUBBRANCH"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            Dim oLookUP As New NexusProvider.LookupListCollection
                                                            If oQuote IsNot Nothing AndAlso oQuote.SubBranchCode IsNot Nothing Then
                                                                If CType(oControl, DropDownList).Items.Count > 0 Then
                                                                    CType(oControl, DropDownList).SelectedValue = oQuote.SubBranchCode
                                                                End If
                                                            End If
                                                    End Select

                                                Case "CURRENCY"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.CurrencyCode IsNot Nothing Then
                                                                If CType(oControl, DropDownList).Items.Count > 0 Then
                                                                    oControl.SelectedValue = oQuote.CurrencyCode
                                                                End If
                                                            End If

                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.CurrencyCode IsNot Nothing Then
                                                                If CType(oControl, NexusProvider.LookupList).Items.Count > 0 Then
                                                                    oControl.Value = oQuote.CurrencyCode
                                                                End If
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.CurrencyCode IsNot Nothing Then
                                                                If CType(oControl, NexusProvider.LookupListV2).Items.Count > 0 Then
                                                                    oControl.Value = oQuote.CurrencyCode
                                                                End If
                                                            End If
                                                    End Select
                                                    Current.Session(CNCurrenyCode) = oQuote.CurrencyCode

                                                Case "COVERNOTEBOOKNO"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.CoverNoteBookNumber IsNot Nothing Then
                                                                CType(sender, BaseRisk).EnableDisableCNB(True)
                                                            Else
                                                                CType(sender, BaseRisk).EnableDisableCNB(False)
                                                            End If
                                                    End Select

                                                Case "COVERNOTESHEETNO"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "DROPDOWNLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.CoverNoteSheetNumber > 0 Then
                                                                CType(sender, BaseRisk).EnableDisableCNS(True)
                                                            Else
                                                                CType(sender, BaseRisk).EnableDisableCNS(False)
                                                            End If
                                                    End Select

                                                Case "RENEWALMETHOD"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.RenewalMethodCode IsNot Nothing Then
                                                                oControl.Value = oQuote.RenewalMethodCode
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.RenewalMethodCode IsNot Nothing Then
                                                                oControl.Value = oQuote.RenewalMethodCode
                                                            End If
                                                    End Select

                                                Case "LTUEXPIRYDATE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing And oQuote.LTUExpiryDate <> Date.MinValue _
                                                            And oQuote.LTUExpiryDate.ToString <> "00:00:00" And oQuote.LTUExpiryDate.Year >= 1900 Then
                                                                CType(oControl, TextBox).Text = oQuote.LTUExpiryDate
                                                            Else
                                                                CType(oControl, TextBox).Text = String.Empty
                                                            End If

                                                    End Select

                                                Case "LAPSECANCELREASON"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.LapseCancelReasonCode IsNot Nothing Then
                                                                oControl.Value = oQuote.LapseCancelReasonCode
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.LapseCancelReasonCode IsNot Nothing Then
                                                                oControl.Value = oQuote.LapseCancelReasonCode
                                                            End If
                                                    End Select

                                                Case "STOPREASON"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.StopReasonCode IsNot Nothing Then
                                                                oControl.Value = oQuote.StopReasonCode
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.StopReasonCode IsNot Nothing Then
                                                                oControl.Value = CType(sender, BaseRisk).GetListId("PMLookup", "Renewal_stop_code", oQuote.StopReasonCode)

                                                            End If
                                                    End Select

                                                Case "LAPSECANCELDATE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing And oQuote.LapseDate <> Date.MinValue _
                                                                And oQuote.LapseDate.ToString <> "00:00:00" Then
                                                                CType(oControl, TextBox).Text = oQuote.LapseDate
                                                            Else
                                                                CType(oControl, TextBox).Text = String.Empty
                                                            End If

                                                    End Select

                                                Case "REFERREDATRENEWAL"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "CHECKBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, CheckBox).Checked = oQuote.ReferredAtRenewal
                                                            End If
                                                    End Select

                                                Case "REFERREDATMTA"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "CHECKBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, CheckBox).Checked = oQuote.ReferredAtMTA
                                                            End If
                                                    End Select

                                                Case "COVERSTARTDATE"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.CoverStartDate
                                                            End If
                                                    End Select

                                                Case "COVERENDDATE"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.CoverEndDate
                                                            End If
                                                    End Select

                                                Case "INCEPTION"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.InceptionDate
                                                            End If
                                                    End Select

                                                Case "RENEWAL"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.RenewalDate
                                                            End If
                                                    End Select

                                                Case "UNIFIEDRENEWALDAY"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "DROPDOWNLIST"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, DropDownList).SelectedValue = oQuote.RenewalDayNo
                                                            End If
                                                    End Select

                                                Case "ANNIVERSARY"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.AnniversaryDate
                                                            End If
                                                    End Select

                                                Case "CONSOLIDATEDLEADAGENTCOMMISSION"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "CHECKBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, CheckBox).Checked = oQuote.ConsolidatedLeadAgentCommission
                                                            End If
                                                    End Select

                                                Case "CONSOLIDATEDLEADSUBAGENTCOMMISSION"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "CHECKBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, CheckBox).Checked = oQuote.ConsolidatedSubAgentCommission
                                                            End If
                                                    End Select

                                                Case "INCEPTIONTPI"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.InceptionTPI
                                                            End If
                                                    End Select

                                                Case "PROPOSALDATE"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.ProposalDate
                                                            End If
                                                    End Select

                                                Case "QUOTEEXPIRYDATE"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                If Current.Session(CNRenewal) IsNot Nothing AndAlso oQuote.QuoteExpiryDate = Date.MinValue Then
                                                                    Dim dQuoteExpiryDate As Date = oQuote.CoverStartDate.AddDays(1)
                                                                    CType(oControl, TextBox).Text = dQuoteExpiryDate
                                                                    CType(oControl, TextBox).Enabled = True
                                                                ElseIf oQuote IsNot Nothing And oQuote.QuoteExpiryDate <> Date.MinValue _
                                                                And oQuote.QuoteExpiryDate.ToString <> "00:00:00" Then
                                                                    CType(oControl, TextBox).Text = oQuote.QuoteExpiryDate
                                                                Else
                                                                    CType(oControl, TextBox).Text = String.Empty
                                                                End If
                                                            End If
                                                    End Select

                                                Case "POLICYNUMBER"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.InsuranceFileRef
                                                            End If
                                                    End Select

                                                Case "POLICYSTATUSCODE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing AndAlso oQuote.PolicyStatusCode IsNot Nothing Then
                                                                oControl.Value = oQuote.PolicyStatusCode
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing AndAlso oQuote.PolicyStatusCode IsNot Nothing Then
                                                                oControl.Value = oQuote.PolicyStatusCode
                                                            End If
                                                    End Select
                                                Case "UNDERWRITINGYEAR"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing Then
                                                                oControl.Value = oQuote.UnderwritingYearId
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing Then
                                                                oControl.Value = oQuote.UnderwritingYearId
                                                            End If
                                                    End Select
                                                Case "COINSURANCEPLACEMENT"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "RADIOBUTTONLIST"
                                                            If oQuote IsNot Nothing Then
                                                                oControl.SelectedIndex = oControl.Items.IndexOf(oControl.Items.FindByValue(oQuote.CoinsurancePlacement))
                                                            End If
                                                    End Select
                                                Case "OLDPOLICYNO"
                                                    Select Case oControl.GetType.Name.ToUpper

                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, TextBox).Text = oQuote.OldPolicyNumber
                                                            End If
                                                    End Select
                                                Case "CORRESPONDENCETYPE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            If oQuote IsNot Nothing Then
                                                                oControl.Value = oQuote.CorrespondenceType
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            If oQuote IsNot Nothing Then
                                                                oControl.Value = oQuote.CorrespondenceType
                                                            End If
                                                    End Select
                                                Case "DEFAULTPREFERREDCORRESPONDENCE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If oQuote IsNot Nothing Then
                                                                If Not String.IsNullOrEmpty(oQuote.DefaultPreferredCorrespondence) Then
                                                                    CType(oControl, TextBox).Text = CType(sender, BaseRisk).GetListDescription("PMLookup", "Contact_Type", oQuote.DefaultPreferredCorrespondence)
                                                                End If
                                                            End If
                                                    End Select
                                                Case "RECEIVESCLIENTCORRESPONDENCE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "HIDDENFIELD"
                                                            If oQuote IsNot Nothing Then
                                                                CType(oControl, HiddenField).Value = oQuote.IsAgentReceiveCorrespondence
                                                            End If
                                                    End Select

                                            End Select

                                    End Select
                                End If

                            Case "CLIENTDETAILS"
                                'Client Data Controls ONLY
                                'It should not read the oQuote again on POSTBACK
                                Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                If (IsPostBack = False AndAlso Not String.IsNullOrEmpty(oPortal.AnnPartyID) AndAlso CInt(oPortal.AnnPartyID) <> CInt(oParty.Key)) _
                                Or (IsPostBack = True AndAlso Not String.IsNullOrEmpty(oPortal.AnnPartyID) AndAlso CInt(oPortal.AnnPartyID) <> CInt(oParty.Key) AndAlso oQuote.PartyKey <> CInt(oParty.Key)) Then
                                    Select Case sControlName(1)
                                        Case "COMPANY_NAME"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If TypeOf oParty Is NexusProvider.CorporateParty Then
                                                        oParty = CType(oParty, NexusProvider.CorporateParty)
                                                        With CType(oParty, NexusProvider.CorporateParty)
                                                            CType(oControl, TextBox).Text = .CompanyName
                                                        End With
                                                    ElseIf TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, TextBox).Text = .TradingName
                                                        End With
                                                    End If
                                            End Select

                                        Case "TELEPHONE"
                                            'Client Contact Home Phone
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    Dim iCount As Integer
                                                    Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                                                    For iCount = 0 To oColl.Count - 1
                                                        'if party contact detail have telephone record
                                                        If oColl(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.Number AndAlso oColl(iCount).ContactType = NexusProvider.ContactType.HomePhone Then
                                                            CType(oControl, TextBox).Text = oColl(iCount).Number
                                                            Exit For
                                                        End If
                                                    Next
                                            End Select

                                        Case "E_MAIL"
                                            'Client Contact Email
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    Dim iCount As Integer
                                                    Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                                                    For iCount = 0 To oColl.Count - 1
                                                        'if party contact detail have email record
                                                        If oColl(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress AndAlso oColl(iCount).ContactType = NexusProvider.ContactType.Email Then
                                                            CType(oControl, TextBox).Text = oColl(iCount).Number
                                                            Exit For
                                                        End If
                                                    Next
                                            End Select
                                        Case "ADDRESS_LINE1"
                                            'Client Address Line1
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"

                                                    CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address1
                                                    oParty = Nothing
                                            End Select
                                        Case "ADDRESS_LINE2"
                                            'Client Address Line2
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address2
                                            End Select
                                        Case "ADDRESS_LINE3"
                                            'Client Address Line3
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address3
                                            End Select
                                        Case "ADDRESS_LINE4"
                                            'Client Address Line4
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4
                                                Case "LOOKUPLIST"
                                                    If CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                        CType(oControl, NexusProvider.LookupList).Value = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4 'GetCodeForKey(NexusProvider.ListType.PMLookup, GetKeyForDescription(NexusProvider.ListType.PMLookup, oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4, "STATE", False), "STATE", True)
                                                    ElseIf CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                        CType(oControl, NexusProvider.LookupList).Value = GetKeyForDescription(NexusProvider.ListType.PMLookup, oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4, "STATE", False)
                                                    End If
                                                Case "LOOKUPLISTV2"
                                                    If CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                        CType(oControl, NexusProvider.LookupListV2).Value = GetCodeForKey(NexusProvider.ListType.PMLookup, GetKeyForDescription(NexusProvider.ListType.PMLookup, oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4, "STATE", False), "STATE", True)
                                                    ElseIf CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                        CType(oControl, NexusProvider.LookupListV2).Value = GetKeyForDescription(NexusProvider.ListType.PMLookup, oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4, "STATE", False)
                                                    End If
                                            End Select

                                        Case "ADDRESS_LINE5"
                                            CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address5

                                        Case "ADDRESS_LINE6"
                                            CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address6

                                        Case "ADDRESS_LINE7"
                                            CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address7

                                        Case "ADDRESS_LINE8"
                                            CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address8

                                        Case "ADDRESS_LINE9"
                                            CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address9

                                        Case "ADDRESS_LINE10"
                                            CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address10


                                        Case "POSTCODE"
                                            'Client Address Postcode
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    CType(oControl, TextBox).Text = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).PostCode
                                            End Select
                                        Case "COUNTRYCODE"
                                            'Client Address Country
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "LOOKUPLIST"
                                                    If CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                        CType(oControl, NexusProvider.LookupList).Value = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode
                                                    ElseIf CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                        If oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode IsNot Nothing Then
                                                            CType(oControl, NexusProvider.LookupList).Value = GetCodeForKey(NexusProvider.ListType.PMLookup, oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode, "COUNTRY", False) 'oAddress.CountryCode
                                                        ElseIf oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey > 0 Then
                                                            CType(oControl, NexusProvider.LookupList).Value = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey
                                                        End If
                                                    End If
                                                Case "LOOKUPLISTV2"
                                                    If CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                        CType(oControl, NexusProvider.LookupListV2).Value = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode
                                                    ElseIf CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                        If oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode IsNot Nothing Then
                                                            CType(oControl, NexusProvider.LookupListV2).Value = GetCodeForKey(NexusProvider.ListType.PMLookup, oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode, "COUNTRY", False) 'oAddress.CountryCode
                                                        ElseIf oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey > 0 Then
                                                            CType(oControl, NexusProvider.LookupListV2).Value = oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey
                                                        End If
                                                    End If
                                            End Select

                                'Added from here for personal Client
                                        Case "DOB"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, TextBox).Text = .DateOfBirth
                                                        End With
                                                    End If
                                            End Select
                                        Case "TITLE"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "LOOKUPLIST"
                                                    If CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Description Then
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, NexusProvider.LookupList).Value = .Title

                                                        End With

                                                    End If
                                                Case "LOOKUPLISTV2"
                                                    If CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Description Then
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, NexusProvider.LookupListV2).Value = .Title

                                                        End With

                                                    End If
                                            End Select
                                        Case "FORENAME"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, TextBox).Text = .Forename
                                                        End With
                                                    End If
                                            End Select
                                        Case "SURNAME"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, TextBox).Text = .Lastname
                                                        End With
                                                    End If
                                            End Select
                                        Case "MOBILE"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        Dim oContacts As NexusProvider.ContactCollection = oParty.Contacts

                                                        For iCount = 0 To oContacts.Count - 1
                                                            'if party contact detail have telephone record
                                                            If oContacts(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.Number AndAlso oContacts(iCount).ContactType = NexusProvider.ContactType.Mobile Then
                                                                CType(oControl, TextBox).Text = oContacts(iCount).Number
                                                                Exit For
                                                            End If
                                                        Next
                                                    End If
                                            End Select

                                'Case "E_MAIL"
                                '    'Client Contact Email
                                '    Select Case oControl.GetType.Name.ToUpper
                                '        Case "TEXTBOX"
                                '            If TypeOf oParty Is NexusProvider.PersonalParty Then
                                '                Dim iCount As Integer
                                '                Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                                '                For iCount = 0 To oColl.Count - 1
                                '                    'if party contact detail have email record
                                '                    If oColl(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress AndAlso oColl(iCount).ContactType = NexusProvider.ContactType.Email Then
                                '                        CType(oControl, TextBox).Text = oColl(iCount).Number
                                '                        Exit For
                                '                    End If
                                '                Next
                                '            End If
                                '    End Select
                                        Case "BRANCH"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "LOOKUPLIST"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, NexusProvider.LookupList).Value = .BranchCode
                                                        End With
                                                    End If
                                                Case "LOOKUPLISTV2"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, NexusProvider.LookupListV2).Value = .BranchCode
                                                        End With
                                                    End If
                                            End Select
                                        Case "INITIAL"
                                            'Client Name
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            CType(oControl, TextBox).Text = .Initials
                                                        End With
                                                    End If
                                            End Select
                                    End Select
                                End If
                                oParty = Nothing
                            Case Else

                                'Risk Data Controls ONLY

                                Select Case oControl.GetType.Name
                        'if controls are placed inside these container
                                    Case "Panel"
                                        Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                        Dim Doc As New XmlDocument

                                        Doc.Load(xmlTR)
                                        xmlTR.Close()
                                        LoadRiskControls(Doc, oControl, v_sOI, sender, InitializeOnly)

                                    Case "UpdatePanel"
                                        Dim oCtrl As Control
                                        For Each oCtrl In oControl.Controls
                                            Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                            Dim Doc As New XmlDocument

                                            Doc.Load(xmlTR)
                                            xmlTR.Close()
                                            LoadRiskControls(Doc, oCtrl, v_sOI, sender, InitializeOnly)
                                        Next

                                    Case "HtmlGenericControl"
                                        Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                        Dim Doc As New XmlDocument

                                        Doc.Load(xmlTR)
                                        xmlTR.Close()
                                        LoadRiskControls(Doc, oControl, v_sOI, sender, InitializeOnly)

                                    Case "HtmlInputText"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then

                                                Dim dtAttribute As DateTime
                                                If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                                    InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                                    If (CDate(dtAttribute).ToShortTimeString = Date.MinValue) Then
                                                        sControlValue = dtAttribute.ToShortDateString
                                                    Else
                                                        sControlValue = dtAttribute.ToString
                                                    End If

                                                End If

                                                CType(oControl, HtmlInputText).Value = sControlValue
                                            End If
                                        End If

                                    Case "Label"
                                        If Not InitializeOnly Then
                                            'Seems odd, but can be used to display page
                                            'titles or section titles within the layout
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then


                                                Dim dtAttribute As DateTime
                                                If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                                    InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                                    If (CDate(dtAttribute).ToShortTimeString = Date.MinValue) Then
                                                        sControlValue = dtAttribute.ToShortDateString
                                                    Else
                                                        sControlValue = dtAttribute.ToString
                                                    End If

                                                End If

                                                CType(oControl, Label).Text = sControlValue
                                            End If
                                        End If

                                    Case "TextBox"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then

                                                Dim dtAttribute As DateTime
                                                If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                                    InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                                    If (CDate(dtAttribute).ToShortTimeString = Date.MinValue) Then
                                                        sControlValue = dtAttribute.ToShortDateString
                                                    Else
                                                        sControlValue = dtAttribute.ToString
                                                    End If

                                                End If

                                                sControlValue = HttpUtility.HtmlDecode(sControlValue)
                                                sControlValue = Replace(sControlValue, "&quot;", """")
                                                sControlValue = Replace(sControlValue, "&apos;", "'")
                                                CType(oControl, TextBox).Text = sControlValue
                                            End If
                                        End If

                                    Case "CheckBox"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                'if 1 then checked, anthing else not checked, including "2" unknown
                                                CType(oControl, CheckBox).Checked = IIf(sControlValue = 1, True, False)
                                            End If
                                        End If

                            'ADDED: WASN'T ABLE TO FIND "HtmlInputCheckBox" - MB - 01 JUNE 07
                                    Case "HtmlInputCheckBox"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                'if 1 then checked, anthing else not checked, including "2" unknown
                                                CType(oControl, HtmlInputCheckBox).Checked = IIf(sControlValue = 1, True, False)
                                            End If
                                        End If

                                    Case "RadioButton"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                If sControlValue = "0" Or sControlValue = "1" Then
                                                    CType(oControl, RadioButton).Checked = sControlValue
                                                End If
                                            End If
                                        End If

                                    Case "RadioButtonList"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                If sControlValue = "0" Or sControlValue = "1" Then
                                                    CType(oControl, RadioButtonList).SelectedValue = sControlValue
                                                End If
                                            End If
                                        End If

                                    Case "LookupList"
                                        'Check for a value set by a parent control, if so use as the ParentKey
                                        'on the LookupList, this will filter the results according to the ParentKey
                                        sControlValue = Current.Session.Item(oControl.ID)

                                        If sControlValue IsNot Nothing Then
                                            oControl.ParentKey = sControlValue
                                        End If

                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                oControl.Value = sControlValue
                                            End If
                                        End If
                                    Case "LookupListV2"
                                        'Check for a value set by a parent control, if so use as the ParentKey
                                        'on the LookupListV2, this will filter the results according to the ParentKey
                                        sControlValue = Current.Session.Item(oControl.ID)

                                        If sControlValue IsNot Nothing Then
                                            oControl.ParentKey = sControlValue
                                        End If

                                        If Current.Session(CNDataSet) IsNot Nothing AndAlso Current.Session(CNQuote) Is Nothing _
                                          AndAlso (Current.Session(CNMode) = Mode.NewClaim Or Current.Session(CNMode) = Mode.EditClaim Or Current.Session(CNMode) = Mode.PayClaim) Then
                                            oControl.EffectiveDate = CType((Current.Session(CNClaim)), NexusProvider.Claim).LossToDate
                                        ElseIf Current.Session(CNQuote) IsNot Nothing Then
                                            oControl.EffectiveDate = oQuote.CoverStartDate
                                        End If

                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                oControl.Value = sControlValue
                                            End If
                                        End If
                                    Case "RiskContainer"
                                        If InitializeOnly = False Then
                                            'if Initilialize is true
                                            Dim oOI As String

                                            If Current.Session(CNDataSet) IsNot Nothing AndAlso Current.Session(CNQuote) Is Nothing _
                                            AndAlso (Current.Session(CNMode) = Mode.NewClaim Or Current.Session(CNMode) = Mode.EditClaim Or Current.Session(CNMode) = Mode.PayClaim) Then
                                                'for Claim
                                                'Load the default, function is written in BaseRisk
                                                oOI = CType(sender, BaseClaim).LoadChildDefaultItem(CType(oControl, RiskContainer).ScreenCode,
                                                    sControlName(0), sControlName(1))

                                                ' Doc is again populated for only newly added child with defaults
                                                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(Current.Session(CNDataSet)))
                                                Dim Doc As New XmlDocument

                                                Doc.Load(xmlTR)
                                                xmlTR.Close()

                                                'Reading the defaults from xml
                                                CType(oControl, RiskContainer).ParentElement = sControlName(0)
                                                CType(oControl, RiskContainer).ChildElement = sControlName(1)
                                                DataSetFunctions.LoadRiskControls(Doc, oControl, oOI, sender)

                                                'Cleanup up the xml, deleting the newly added child from xml
                                                Dim srDataset As New System.IO.StringReader(Current.Session(CNDataSet))
                                                xmlTR = New XmlTextReader(srDataset)
                                                Doc = New XmlDocument
                                                Doc.Load(xmlTR)
                                                xmlTR.Close()

                                                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & oOI & "' and @US='3']")
                                                srDataset.Dispose()

                                                If oNode Is Nothing Then
                                                    oNode = Doc.SelectSingleNode("//*[@OI='" & oOI & "' and @NODEISVALID='0']")
                                                End If

                                                If oNode IsNot Nothing Then
                                                    Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                                                    oSAMClient.LoadFromXML(ClaimGetDataSetDefinition(), Current.Session(CNDataSet))
                                                    oSAMClient.DelObjectInstance(oNode.Name, oOI)
                                                    oSAMClient.ReturnAsXML(Current.Session(CNDataSet))
                                                    oSAMClient.Terminate()
                                                End If
                                            ElseIf Current.Session(CNQuote) IsNot Nothing AndAlso (Current.Session(CNMode) = Mode.Add Or Current.Session(CNMode) = Mode.Edit Or Current.Session(CNMode) = Mode.Buy) Then
                                                'for New Business
                                                'Load the default, function is written in BaseRisk
                                                oOI = CType(sender, BaseRisk).LoadChildDefaultItem(CType(oControl, RiskContainer).ScreenCode,
                                                    sControlName(0), sControlName(1))

                                                ' Doc is again populated for only newly added child with defaults
                                                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                                                Dim Doc As New XmlDocument

                                                Doc.Load(xmlTR)
                                                xmlTR.Close()
                                                'Reading the defaults from xml
                                                CType(oControl, RiskContainer).ParentElement = sControlName(0)
                                                CType(oControl, RiskContainer).ChildElement = sControlName(1)
                                                DataSetFunctions.LoadRiskControls(Doc, oControl, oOI, sender)

                                                'Cleanup up the xml, deleting the newly added child from xml
                                                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                xmlTR = New XmlTextReader(srDataset)
                                                Doc = New XmlDocument
                                                Doc.Load(xmlTR)
                                                xmlTR.Close()

                                                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & oOI & "' and @US='3']")
                                                srDataset.Dispose()

                                                If oNode IsNot Nothing Then

                                                    Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                                                    oSAMClient.LoadFromXML(GetDataSetDefinition(Current.Session(CNDataModelCode)), oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                    oSAMClient.DelObjectInstance(oNode.Name, oOI)
                                                    oSAMClient.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                    oSAMClient.Terminate()

                                                    Current.Session(CNQuote) = oQuote
                                                End If
                                            End If
                                        Else
                                            'if Initilialize is true
                                            CType(oControl, RiskContainer).ParentElement = sControlName(0)
                                            CType(oControl, RiskContainer).ChildElement = sControlName(1)
                                            DataSetFunctions.LoadRiskControls(oDoc, oControl, v_sOI, sender, True)
                                        End If


                                    Case "ItemGrid"
                                        If Current.Session(CNDataSet) IsNot Nothing And Current.Session(CNQuote) Is Nothing Then
                                            ' Child screen item grid for Claims
                                            If String.IsNullOrEmpty(CType(oControl, ItemGrid).ChildContainer) Then
                                                AddHandler CType(oControl, ItemGrid).EditItem, AddressOf CType(sender, BaseClaim).EditItem
                                            Else
                                                AddHandler CType(oControl, ItemGrid).EditItemInRiskContainer, AddressOf CType(sender, BaseClaim).EditItemInRiskContainer
                                            End If

                                            AddHandler CType(oControl, ItemGrid).AddItem, AddressOf CType(sender, BaseClaim).AddItem
                                            AddHandler CType(oControl, ItemGrid).DeleteItem, AddressOf CType(sender, BaseClaim).DeleteItem

                                        Else
                                            If String.IsNullOrEmpty(CType(oControl, ItemGrid).ChildContainer) Then
                                                AddHandler CType(oControl, ItemGrid).EditItem, AddressOf CType(sender, BaseRisk).EditItem
                                            Else
                                                AddHandler CType(oControl, ItemGrid).EditItemInRiskContainer, AddressOf CType(sender, BaseRisk).EditItemInRiskContainer
                                            End If

                                            AddHandler CType(oControl, ItemGrid).AddItem, AddressOf CType(sender, BaseRisk).AddItem
                                            AddHandler CType(oControl, ItemGrid).DeleteItem, AddressOf CType(sender, BaseRisk).DeleteItem
                                        End If
                                        'DH - 09-01-08 - Always bind the grid as even when its empty we need the empty row footer
                                        'creating, as this isn't created without a databind, so removed the InitializeOnly check
                                        'If Not InitializeOnly Then

                                        'Code to check the FilterByControl Property - Start
                                        Dim oFilter As New Hashtable
                                        If IsPostBack = True AndAlso Current.Session(CNMode) <> Mode.View And Current.Session(CNMode) <> Mode.Review And Current.Session(CNMode) <> Mode.ViewClaim Then
                                            Dim iTotalColumnCount As Integer = CType(oControl, ItemGrid).Columns.Count
                                            Dim sFilterValue As String = Nothing
                                            For iCount As Integer = 0 To iTotalColumnCount - 1
                                                'To check it's type since this attributes is available with only "RiskAttribute"
                                                If CType(oControl, ItemGrid).Columns(iCount).GetType.Name = "RiskAttribute" Then
                                                    'Check whether attributes is available or not
                                                    If CType(CType(oControl, ItemGrid).Columns(iCount), RiskAttribute).FilterByControl IsNot Nothing Then
                                                        Dim sFilterControlName As String = CType(CType(oControl, ItemGrid).Columns(iCount), RiskAttribute).FilterByControl
                                                        'Check whether mentioned control is available or not
                                                        If v_oContainer.FindControl(sFilterControlName) IsNot Nothing Then
                                                            Select Case v_oContainer.FindControl(sFilterControlName).GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    sFilterValue = CType(v_oContainer.FindControl(sFilterControlName), TextBox).Text.Trim
                                                                    If String.IsNullOrEmpty(sFilterValue) = False Then
                                                                        oFilter.Add(CType(CType(oControl, ItemGrid).Columns(iCount), RiskAttribute).DataField, sFilterValue)
                                                                    End If
                                                                Case "DROPDOWNLIST"
                                                                    If CType(v_oContainer.FindControl(sFilterControlName), DropDownList).Items.Count > 0 Then
                                                                        sFilterValue = CType(v_oContainer.FindControl(sFilterControlName), DropDownList).SelectedValue
                                                                        If String.IsNullOrEmpty(sFilterValue) = False Then
                                                                            oFilter.Add(CType(CType(oControl, ItemGrid).Columns(iCount), RiskAttribute).DataField, sFilterValue)
                                                                        End If
                                                                    End If
                                                                Case "LOOKUPLIST"
                                                                    If CType(v_oContainer.FindControl(sFilterControlName), NexusProvider.LookupList).Items.Count > 0 Then
                                                                        sFilterValue = CType(v_oContainer.FindControl(sFilterControlName), NexusProvider.LookupList).Value
                                                                        If String.IsNullOrEmpty(sFilterValue) = False Then
                                                                            oFilter.Add(CType(CType(oControl, ItemGrid).Columns(iCount), RiskAttribute).DataField, sFilterValue)
                                                                        End If
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    If CType(v_oContainer.FindControl(sFilterControlName), NexusProvider.LookupListV2).Items.Count > 0 Then
                                                                        sFilterValue = CType(v_oContainer.FindControl(sFilterControlName), NexusProvider.LookupListV2).Value
                                                                        If String.IsNullOrEmpty(sFilterValue) = False Then
                                                                            oFilter.Add(CType(CType(oControl, ItemGrid).Columns(iCount), RiskAttribute).DataField, sFilterValue)
                                                                        End If
                                                                    End If
                                                            End Select
                                                        End If
                                                    End If
                                                End If
                                            Next
                                        End If
                                        'Code to check the FilterByControl Property - End

                                        Dim oXMLSource As New XmlDataSource
                                        oXMLSource.EnableCaching = False 'why? why? why? why would you enable caching as default

                                        'v_sOI Reset by Browser back Button
                                        Dim oOI As Collections.Stack = Current.Session.Item(CNOI)
                                        If oOI.Count = 0 Then
                                            v_sOI = ""
                                            ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI)
                                        End If

                                        If ReadElementListFromXML(sControlName, oXMLSource, v_sOI, oFilter) Then
                                            oControl.DataSource = oXMLSource
                                        End If
                                        oControl.DataBind()

                                        oXMLSource = Nothing


                                    Case "HiddenField"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then

                                                Dim dtAttribute As DateTime
                                                If DateTime.TryParseExact(sControlValue, "yyyy-MM-dd HH:mm:ss",
                                                    InvariantCulture, System.Globalization.DateTimeStyles.None, dtAttribute) Then

                                                    sControlValue = dtAttribute.ToShortDateString

                                                End If

                                                CType(oControl, HiddenField).Value = sControlValue
                                            End If
                                        End If

                                    Case "DropDownList"
                                        If Not InitializeOnly Then
                                            If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                If CType(oControl, DropDownList).Items.Count > 0 Then
                                                    CType(oControl, DropDownList).SelectedValue = sControlValue.ToString
                                                End If
                                            End If
                                        End If

                                    Case Else
                                        'Added support so that control populate from XMl data, even when put in portal folder (earlier this was not functional)
                                        Select Case True
                                            Case oControl.GetType.Name.Contains("controls_addresscntrl_ascx")

                                                If Not InitializeOnly Then

                                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                        oControl.AddressKey = sControlValue
                                                        oControl.Key = sControlValue

                                                    End If
                                                End If

                                            Case oControl.GetType.Name.Contains("controls_findparty_ascx")
                                                If Not InitializeOnly Then

                                                    If ReadAttributeFromXML(oDoc, sControlName, sControlValue, v_sOI) Then
                                                        oControl.PartyKey = sControlValue
                                                    End If

                                                End If

                                            Case oControl.GetType.Name.Contains("controls_standardwordings_ascx")
                                                'If Current.Session.Item(CNTempOI) Is Nothing Then
                                                '    ReteriveOIFromXML(oDoc, sControlName)
                                                'End If
                                                If Not InitializeOnly Then
                                                    'If v_sOI.Trim.Length = 0 Then
                                                    ReteriveOIFromXML(oDoc, sControlName)
                                                    'End If
                                                    oControl.FillGrid()
                                                End If
                                        End Select
                                End Select

                        End Select

                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' The entry point for reading the risk controls from the provided control container
        ''' </summary>
        ''' <param name="v_oContainer">The control to be searched for risk controls to load data to,
        ''' this is usually the masterpage container</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <param name="sender">The object that made the request to the function, usually content placeholder</param>
        ''' <param name="InitializeOnly">Initialize the risk controls only, e.g set attributes,
        ''' hookup events but don't read the risk data into the control, mainly used on postback</param>
        ''' <remarks></remarks>
        Sub ReadContainerFromXML(ByVal v_oContainer As Control,
                                ByVal v_sOI As String,
                                ByVal sender As Object,
                                Optional ByVal InitializeOnly As Boolean = False,
                                Optional ByVal IsPostBack As Boolean = False)

            If Current.Session(CNDataSet) IsNot Nothing And Current.Session(CNQuote) Is Nothing Then
                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(Current.Session(CNDataSet)))
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()

                LoadRiskControls(Doc, v_oContainer, v_sOI, sender, InitializeOnly, IsPostBack)

            ElseIf Current.Session(CNQuote) IsNot Nothing Then

                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                If oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset IsNot Nothing Then
                    Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                    Dim Doc As New XmlDocument

                    Doc.Load(xmlTR)
                    xmlTR.Close()

                    LoadRiskControls(Doc, v_oContainer, v_sOI, sender, InitializeOnly, IsPostBack)
                End If
            End If
        End Sub
        Function ReadElementListFromXML(ByRef r_oClaim As DataSetControl.Application,
                                              ByVal v_sControlName() As String,
                                              ByRef r_oXMLDataSource As XmlDataSource,
                                              ByVal v_sOI As String) As Boolean

            r_oXMLDataSource.Data = Current.Session.Item(CNDataSet)

            'read mulitple levels down to the request element instance
            Dim sXPath As String = ".//" & v_sControlName(0)

            If v_sOI Is Nothing Or v_sOI = String.Empty Then
            Else
                'only return child elements of the element specified by the OI
                sXPath &= "[@OI='" & v_sOI & "']"
            End If

            sXPath &= "/" & v_sControlName(1)

            r_oXMLDataSource.XPath = sXPath

            Return True

        End Function
        ''' <summary>
        ''' Write all the risk controls from the provided control container to the current risk dataset
        ''' </summary>
        ''' <param name="v_oContainer">The control to be searched for risk controls to load data to,
        ''' this is usually the masterpage container</param>        
        ''' <param name="v_sScreenCode">The screencode of the current element, we need this for running the default rules</param>
        ''' <param name="v_sOI">The dataset identifier of the current element</param>
        ''' <remarks></remarks>
        Sub WriteContainerToXML(ByVal v_oContainer As Control,
                                    ByVal v_sScreenCode As String, ByVal v_sOI As String, Optional ByVal v_bStatus As Boolean = False, Optional ByVal v_sParentTab As String = Nothing, Optional ByVal sender As Object = Nothing)

            If Current.Session(CNDataSet) IsNot Nothing And Current.Session(CNQuote) Is Nothing Then
                Dim oClaimQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
                Dim sBranchCode As String = oClaimQuote.BranchCode
                Dim sDataSetDefinition As String = ClaimGetDataSetDefinition()

                Dim oClaim As New DataSetControl.Application
                Dim oControl As Object
                Dim sControlName() As String 'Will be 2 or 3 elements, ELEMENT__ATTRIBUTE(__CONDITION)
                Dim sControlValue As String = String.Empty
                Dim bSave As Boolean 'means the save can be overridden if we have a trikxy control

                Dim srClaimDataSet As New System.IO.StringReader(Current.Session(CNDataSet))
                Dim xmlClaimTR As New XmlTextReader(srClaimDataSet)
                Dim oClaimDoc As New XmlDocument
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)

                oClaimDoc.Load(xmlClaimTR)
                xmlClaimTR.Close()

                If v_sOI IsNot Nothing Then
                    Dim oClaimNode As XmlNode = oClaimDoc.SelectSingleNode("//*[@OI = '" & v_sOI & "']")
                    If oClaimNode IsNot Nothing AndAlso oClaimNode.Attributes("US").Value = "3" Then
                        oClaimNode.Attributes("US").Value = "2"
                    End If
                End If

                Dim swClaimContent As New System.IO.StringWriter
                Dim xmlwClaimContent As New XmlTextWriter(swClaimContent)

                oClaimDoc.WriteTo(xmlwClaimContent)
                Current.Session(CNDataSet) = swClaimContent.ToString()

                xmlwClaimContent.Close()
                swClaimContent.Close()

                oClaim.LoadFromXML(sDataSetDefinition, Current.Session(CNDataSet))

                sDataSetDefinition = Nothing

                For Each oControl In v_oContainer.Controls
                    If oControl.ID IsNot Nothing Then

                        sControlName = Regex.Split(oControl.ID, "__")
                        bSave = False

                        Select Case oControl.GetType.Name
                            Case "Panel"
                                'Need to store the read element in XML before moving into another container
                                oClaim.ReturnAsXML(Current.Session(CNDataSet))

                                WriteContainerToXML(oControl, v_sScreenCode, v_sOI, True)

                                'Need to load the updated dataset
                                oClaim = New DataSetControl.Application
                                sDataSetDefinition = ClaimGetDataSetDefinition()
                                oClaim.LoadFromXML(sDataSetDefinition, Current.Session(CNDataSet))
                                sDataSetDefinition = Nothing

                            Case "UpdatePanel"
                                'Need to store the read element in XML before moving into another container
                                oClaim.ReturnAsXML(Current.Session(CNDataSet))

                                Dim oCtrl As Control
                                For Each oCtrl In oControl.Controls
                                    WriteContainerToXML(oCtrl, v_sScreenCode, v_sOI, True)

                                    'Need to load the updated dataset
                                    oClaim = New DataSetControl.Application
                                    sDataSetDefinition = ClaimGetDataSetDefinition()
                                    oClaim.LoadFromXML(sDataSetDefinition, Current.Session(CNDataSet))
                                    sDataSetDefinition = Nothing
                                Next

                            Case "HtmlGenericControl"
                                'Need to store the read element in XML before moving into another container
                                oClaim.ReturnAsXML(Current.Session(CNDataSet))

                                WriteContainerToXML(oControl, v_sScreenCode, v_sOI, True)

                                'Need to load the updated dataset
                                oClaim = New DataSetControl.Application
                                sDataSetDefinition = ClaimGetDataSetDefinition()
                                oClaim.LoadFromXML(sDataSetDefinition, Current.Session(CNDataSet))
                                sDataSetDefinition = Nothing

                            Case "HiddenField"

                                sControlValue = CType(oControl, HiddenField).Value
                                bSave = True

                            Case "HtmlInputText"

                                sControlValue = CType(oControl, HtmlInputText).Value
                                bSave = True

                            Case "TextBox"

                                sControlValue = CType(oControl, TextBox).Text
                                bSave = True

                            Case "CheckBox"

                                sControlValue = IIf(CType(oControl, CheckBox).Checked, "1", "0")
                                bSave = True

                            Case "RadioButton"

                                sControlValue = IIf(CType(oControl, RadioButton).Checked, "1", "0")
                                bSave = True

                            Case "RadioButtonList"

                                If oControl.SelectedIndex = -1 Then
                                    sControlValue = 2
                                Else
                                    sControlValue = oControl.SelectedValue
                                End If

                                bSave = True

                            Case "LookupList"

                                sControlValue = oControl.Value
                                If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                    'DH - 25-01-08 - Don't save the value to the dataset if its set to the default value
                                    bSave = False
                                Else
                                    bSave = True
                                End If
                            Case "LookupListV2"

                                sControlValue = oControl.Value
                                If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                    'DH - 25-01-08 - Don't save the value to the dataset if its set to the default value
                                    bSave = False
                                Else
                                    bSave = True
                                End If
                            Case "DropDownList"

                                sControlValue = CType(oControl, DropDownList).SelectedValue
                                bSave = True
                            Case Else

                                Select Case True

                                    Case oControl.GetType.Name.Contains("controls_addresscntrl_ascx")

                                        Dim iAddressKey As Integer

                                        Try
                                            iAddressKey = oControl.AddressKey()

                                            If iAddressKey <> 0 Then
                                                sControlValue = iAddressKey
                                                bSave = True
                                            End If

                                        Catch ex As ArgumentNullException

                                            'DH - 01-02-08 - Catch and ignore any argument null exceptions from
                                            'the AddAddress method, as if the address is missing a parameter we
                                            'don't want to save it to the dataset

                                        End Try
                                    Case oControl.GetType.Name.Contains("controls_findparty_ascx")
                                        Dim iPartyKey As Integer

                                        iPartyKey = oControl.PartyKey
                                        'If iPartyKey <> 0 Then
                                        sControlValue = iPartyKey
                                        bSave = True
                                        'End If

                                    Case oControl.GetType.Name.Contains("controls_perils_ascx")
                                        If sender IsNot Nothing Then
                                            CType(sender, BaseClaim).SavePerilData()
                                        End If

                                    Case oControl.GetType.Name.Contains("controls_reserveandrecovery_ascx")
                                        If sender IsNot Nothing Then
                                            CType(sender, BasePeril).SaveReserveAndRecoveryData(oControl)
                                        End If

                                    Case oControl.GetType.Name.Contains("controls_payclaim_ascx")
                                        If sender IsNot Nothing Then
                                            CType(sender, BasePeril).SavePaymentData(oControl)
                                        End If
                                End Select



                        End Select

                        If bSave Then
                            If WriteClaimAttributeToXML(oClaimDoc, oClaim, sControlName, sControlValue, v_sOI) Then
                                'Success
                            End If
                        End If

                    End If
                Next

                oClaim.ReturnAsXML(Current.Session(CNDataSet))

                'Only continue if the rules have been successfully run.
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    Dim sTransactionTypeCode As String = String.Empty
                    Dim oPerilSummary As New NexusProvider.PerilSummary

                    If Current.Session(CNMode) = Mode.NewClaim Then
                        sTransactionTypeCode = "C_CO"
                    ElseIf Current.Session(CNMode) = Mode.EditClaim Then
                        sTransactionTypeCode = "C_CR"
                    ElseIf Current.Session(CNMode) = Mode.PayClaim Then
                        sTransactionTypeCode = "C_CP"
                    ElseIf Current.Session(CNMode) = Mode.TPRecovery Then
                        sTransactionTypeCode = "C_RV"
                    ElseIf Current.Session(CNMode) = Mode.SalvageClaim Then
                        sTransactionTypeCode = "C_SA"
                    End If
                    oPerilSummary.ClaimKey = Current.Session.Item(CNClaimKey)
                    If Current.Session.Item(CNClaimPerilKey) IsNot Nothing AndAlso String.IsNullOrEmpty(v_sScreenCode) = False Then
                        oPerilSummary.ClaimPerilKey = Current.Session.Item(CNClaimPerilKey)
                        Current.Session.Item(CNDataSet) = oWebService.RunDefaultRulesEdit(v_sScreenCode, Current.Session.Item(CNDataSet), oPerilSummary, oClaimQuote.BranchCode, v_sTransactionType:=sTransactionTypeCode)
                    ElseIf String.IsNullOrEmpty(v_sScreenCode) = False Then
                        Current.Session.Item(CNDataSet) = oWebService.RunDefaultRulesEdit(v_sScreenCode, Current.Session.Item(CNDataSet), Nothing, oClaimQuote.BranchCode, v_sTransactionType:=sTransactionTypeCode)
                    End If

                Finally
                    oWebService = Nothing
                End Try
            ElseIf Current.Session(CNDataSet) Is Nothing And Current.Session(CNQuote) Is Nothing Then
                'If Claim Builder is OFF

                For Each oControl In v_oContainer.Controls
                    Dim sControlName() As String
                    If oControl.ID IsNot Nothing Then

                        sControlName = Regex.Split(oControl.ID, "__")

                        Select Case True
                            Case oControl.GetType.Name.Contains("controls_perils_ascx")
                                If sender IsNot Nothing Then
                                    CType(sender, BaseClaim).SavePerilData()
                                End If

                            Case oControl.GetType.Name.Contains("controls_reserveandrecovery_ascx")
                                If sender IsNot Nothing Then
                                    CType(sender, BasePeril).SaveReserveAndRecoveryData(oControl)
                                End If

                            Case oControl.GetType.Name.Contains("controls_payclaim_ascx")
                                If sender IsNot Nothing Then
                                    CType(sender, BasePeril).SavePaymentData(oControl)
                                End If
                        End Select
                    End If
                Next
            Else
                If Current.Session(CNQuote) IsNot Nothing Then

                    Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
                    Dim oDataSet As New DataSetControl.Application
                    Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                    Dim oRiskType As NexusProvider.RiskType = Current.Session(CNRiskType)
                    Dim oNexusFrameWork As Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                    Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())


                    If v_sScreenCode Is Nothing Then
                        If Current.Session(CNQuoteMode) = QuoteMode.FullQuote Or Current.Session(CNQuoteMode) = QuoteMode.MTAQuote Or Current.Session(CNQuoteMode) = QuoteMode.ReQuote Then
                            v_sScreenCode = GetScreenCode(sProductFolder & "/" & oRiskType.Path & "/" & oProduct.FullQuoteConfig)
                        Else
                            v_sScreenCode = GetScreenCode(sProductFolder & "/" & oRiskType.Path & "/" & oProduct.QuickQuoteConfig)
                        End If
                    End If
                    'All newly created elements are set to deleted so they are removed if the user selects the back button,
                    'so as we're saving we need to set the status back to edit otherwise it will be removed when the rules are run.

                    Dim oDoc As New XmlDocument
                    Using srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                        Dim xmlTR As New XmlTextReader(srDataset)

                        oDoc.Load(xmlTR)
                        xmlTR.Close()
                    End Using

                    Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI = '" & v_sOI & "']")

                    'Add Mode
                    If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value = "3" Then
                        If v_sOI IsNot Nothing AndAlso v_sParentTab IsNot Nothing AndAlso String.IsNullOrEmpty(v_sParentTab) Then
                            'Parent element
                            oNode.Attributes("US").Value = "2"
                        ElseIf v_sOI IsNot Nothing AndAlso String.IsNullOrEmpty(v_sParentTab) = False Then
                            'Child element
                            oNode.Attributes("US").Value = "1"
                        End If
                    ElseIf oNode IsNot Nothing AndAlso oNode.Attributes("US").Value = "0" Then
                        'Edit Mode
                        'If v_sOI IsNot Nothing AndAlso v_sParentTab IsNot Nothing AndAlso String.IsNullOrEmpty(v_sParentTab) Then
                        '    'Parent element
                        '    oNode.Attributes("US").Value = "2"
                        'ElseIf v_sOI IsNot Nothing AndAlso String.IsNullOrEmpty(v_sParentTab) = False Then
                        '    'Child element
                        oNode.Attributes("US").Value = "2"
                        'End If
                    End If


                    Dim swContent As New System.IO.StringWriter
                    Dim xmlwContent As New XmlTextWriter(swContent)

                    oDoc.WriteTo(xmlwContent)
                    oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                    xmlwContent.Close()
                    swContent.Close()

                    oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

                    sDataSetDefinition = Nothing

                    Dim oControl As Object
                    Dim sControlName() As String 'Will be 2 or 3 elements, ELEMENT__ATTRIBUTE(__CONDITION)
                    Dim sControlValue As String = String.Empty
                    Dim bSave As Boolean 'means the save can be overridden if we have a trikxy control
                    Dim bIgnore As Boolean 'allows controls to be populate but not saved into backoffice
                    Dim bUpdateQuote As Boolean 'Allows controls to get updated via UpdateQuoteV2 Method
                    Dim bSaveParty As Boolean 'Allow party details to be updated or saved 
                    Dim r_bSkipSave As Boolean = False
                    Dim r_bMultiStdWrd As Boolean = False

                    For Each oControl In v_oContainer.Controls
                        If oControl.ID IsNot Nothing Then

                            sControlName = Regex.Split(oControl.ID.ToUpper(), "__")
                            bSave = False
                            bIgnore = False
                            'bRecursiveIgnore = False

                            If sControlName.Length > 1 Then

                                'Risk Data Controls ONLY

                                If sControlName.Length > 2 Then
                                    Select Case sControlName(2) 'CONDITION
                                        Case "HIDDEN"
                                        Case "IGNORE"
                                            bIgnore = True
                                        Case "DUPE"
                                            'Allows multiple controls to pull out the same values without
                                            'having duplicate control names, which is obviously invalid
                                    End Select
                                End If

                                If bIgnore = False Then

                                    'Select Case sControlName(1)

                                    '    Case "RISKBASE"
                                    Select Case sControlName(0)
                                        Case "POLICYHEADER"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "PANEL"
                                                    WriteContainerToXML(oControl, v_sScreenCode, v_sOI, True)
                                                Case "UPDATEPANEL"
                                                    Dim oCtrl As Control
                                                    For Each oCtrl In oControl.Controls
                                                        WriteContainerToXML(oCtrl, v_sScreenCode, v_sOI, True)
                                                    Next
                                                Case "HTMLGENERICCONTROL"
                                                    WriteContainerToXML(oControl, v_sScreenCode, v_sOI, True)
                                                Case Else
                                                    Select Case sControlName(1)
                                                        Case "BUSINESSTYPE"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.BusinessTypeCode = CType(oControl, DropDownList).SelectedValue
                                                                Case "LOOKUPLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.BusinessTypeCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    bUpdateQuote = True
                                                                    oQuote.BusinessTypeCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select


                                                        Case "BRANCH"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.BranchCode = CType(oControl, DropDownList).SelectedValue
                                                                    Current.Session(CNTransBranchCode) = oQuote.BranchCode

                                                            End Select
                                                        Case "SUBBRANCH"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.SubBranchCode = CType(oControl, DropDownList).SelectedValue
                                                            End Select


                                                        Case "CONTACT_NAME"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    If (CType(oControl, DropDownList).SelectedValue <> "") Then
                                                                        oQuote.ContactUserKey = Convert.ToInt32(CType(oControl, DropDownList).SelectedValue)
                                                                        Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                                                                        GetAgentSettingsCall(oAgentSettings, oQuote.Agent)
                                                                        Dim oUserCollection As New NexusProvider.UserCollection
                                                                        If (oAgentSettings IsNot Nothing) Then
                                                                            oUserCollection = oAgentSettings.AssociatedUsers
                                                                        End If
                                                                        oQuote.ContactUserName = oUserCollection.FindItemByUserKey(oQuote.ContactUserKey).UserName
                                                                    End If

                                                            End Select

                                                            'Case "AGENT"
                                                            '    Select Case oControl.GetType.Name.ToUpper
                                                            '        Case "HIDDENFIELD"
                                                            '            bUpdateQuote = True

                                                            '            oQuote.AgentKey = CType(oControl, HiddenField).Value

                                                            '    End Select


                                                        Case "CURRENCY"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.CurrencyCode = oControl.SelectedValue
                                                                Case "LOOKUPLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.CurrencyCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    bUpdateQuote = True
                                                                    oQuote.CurrencyCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select
                                                            Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
                                                        Case "FREQUENCY"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.FrequencyCode = CType(oControl, DropDownList).SelectedValue
                                                                Case "LOOKUPLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.FrequencyCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    bUpdateQuote = True
                                                                    oQuote.FrequencyCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select
                                                        Case "COVERSTARTDATE"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.CoverStartDate = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "COVERENDDATE"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.CoverEndDate = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "INCEPTION"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.InceptionDate = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "RENEWAL"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.RenewalDate = CType(oControl, TextBox).Text

                                                            End Select

                                                        Case "UNIFIEDRENEWALDAY"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.RenewalDayNo = CType(oControl, DropDownList).SelectedValue ' CType(oControl, TextBox).Text

                                                            End Select

                                                        Case "ANNIVERSARY"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.AnniversaryDate = CType(oControl, TextBox).Text
                                                            End Select


                                                        Case "CONSOLIDATEDLEADAGENTCOMMISSION"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "CHECKBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.ConsolidatedLeadAgentCommission = CType(oControl, CheckBox).Checked
                                                            End Select

                                                        Case "CONSOLIDATEDLEADSUBAGENTCOMMISSION"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "CHECKBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.ConsolidatedSubAgentCommission = CType(oControl, CheckBox).Checked
                                                            End Select

                                                        Case "INCEPTIONTPI"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.InceptionTPI = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "PROPOSALDATE"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.ProposalDate = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "QUOTEEXPIRYDATE"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.QuoteExpiryDate = CType(oControl, TextBox).Text

                                                            End Select

                                                        Case "AGENT"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "HIDDENFIELD"
                                                                    bUpdateQuote = True
                                                                    oQuote.Agent = CType(oControl, HiddenField).Value

                                                            End Select
                                                        Case "AGENTCODE"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.AgentCode = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "INSUREDNAME"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.InsuredName = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "COVERNOTEBOOKNO"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"

                                                                    If oControl.SelectedValue IsNot Nothing And CType(oControl, DropDownList).SelectedIndex > 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.CoverNoteBookNumber = CType(oControl, DropDownList).SelectedItem.Text
                                                                    End If
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.CoverNoteBookNumber = CType(oControl, TextBox).Text
                                                                    End If

                                                            End Select
                                                        Case "COVERNOTESHEETNO"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "DROPDOWNLIST"
                                                                    If oControl.SelectedValue IsNot Nothing And CType(oControl, DropDownList).SelectedIndex > 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.CoverNoteSheetNumber = CType(oControl, DropDownList).SelectedValue
                                                                    End If
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.CoverNoteSheetNumber = CType(oControl, TextBox).Text
                                                                    End If
                                                            End Select
                                                        Case "HANDLER"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "HIDDENFIELD"
                                                                    bUpdateQuote = True
                                                                    oQuote.AccountHandlerCnt = CType(oControl, HiddenField).Value

                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.AccountHandlerName = CType(oControl, TextBox).Text
                                                                    End If
                                                            End Select
                                                        Case "HANDLERCODE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "HIDDENFIELD"
                                                                    bUpdateQuote = True
                                                                    If CType(oControl, HiddenField).Value.Length <> 0 Then
                                                                        oQuote.HandlerCode = CType(oControl, HiddenField).Value
                                                                    End If
                                                            End Select
                                                        Case "ALTERNATEREF"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.AlternativeRef = CType(oControl, TextBox).Text
                                                                    End If
                                                            End Select

                                                        Case "ANALYSISCODE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    oQuote.AnalysisCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    oQuote.AnalysisCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select
                                                        Case "REGARDING"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        oQuote.Regarding = CType(oControl, TextBox).Text
                                                                    End If
                                                            End Select

                                                        Case "SUBAGENTS"
                                                            If Current.Session.Item(CNSubAgents) IsNot Nothing Then
                                                                Try
                                                                    oWebService.UpdateSubAgents(oQuote, Current.Session.Item(CNSubAgents))
                                                                Finally
                                                                    oWebService = Nothing
                                                                    Current.Session(CNSubAgents) = Nothing
                                                                End Try
                                                            End If
                                                        Case "RENEWALMETHOD"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    oQuote.RenewalMethodCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    oQuote.RenewalMethodCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select

                                                        Case "LTUEXPIRYDATE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text <> "" Then
                                                                        oQuote.LTUExpiryDate = CType(oControl, TextBox).Text
                                                                    End If

                                                            End Select

                                                        Case "LAPSECANCELREASON"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    oQuote.LapseCancelReasonCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    oQuote.LapseCancelReasonCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select

                                                        Case "STOPREASON"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    oQuote.StopReasonCode = oControl.Value
                                                                    If CType(oControl, NexusProvider.LookupList).Text.Equals(CType(oControl, NexusProvider.LookupList).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    oQuote.StopReasonCode = oControl.Code
                                                                    If CType(oControl, NexusProvider.LookupListV2).Text.Equals(CType(oControl, NexusProvider.LookupListV2).DefaultText) Then
                                                                        bSave = False
                                                                    Else
                                                                        bSave = True
                                                                    End If
                                                            End Select

                                                        Case "LAPSECANCELDATE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text <> "" Then
                                                                        oQuote.LapseCancelDate = CType(oControl, TextBox).Text
                                                                    End If

                                                            End Select

                                                        Case "REFERREDATRENEWAL"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "CHECKBOX"
                                                                    oQuote.ReferredAtRenewal = CType(oControl, CheckBox).Checked

                                                            End Select

                                                        Case "REFERREDATMTA"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "CHECKBOX"
                                                                    oQuote.ReferredAtMTA = CType(oControl, CheckBox).Checked

                                                            End Select

                                                        Case "POLICYSTATUSCODE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupList).Value) = False Then
                                                                        oQuote.PolicyStatusCode = oControl.Value
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupListV2).Value) = False Then
                                                                        oQuote.PolicyStatusCode = oControl.Value
                                                                    End If
                                                            End Select
                                                        Case "UNDERWRITINGYEAR"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupList).Value) = False Then
                                                                        oQuote.UnderwritingYearId = oControl.Value
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupListV2).Value) = False Then
                                                                        oQuote.UnderwritingYearId = oControl.Value
                                                                    End If
                                                            End Select
                                                        Case "COINSURANCEPLACEMENT"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "RADIOBUTTONLIST"
                                                                    bUpdateQuote = True
                                                                    oQuote.CoinsurancePlacement = CType(oControl, RadioButtonList).SelectedValue
                                                            End Select


                                                        Case "OLDPOLICYNO"
                                                            Select Case oControl.GetType.Name.ToUpper

                                                                Case "TEXTBOX"
                                                                    bUpdateQuote = True
                                                                    oQuote.OldPolicyNumber = CType(oControl, TextBox).Text

                                                            End Select
                                                        Case "CORRESPONDENCETYPE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "LOOKUPLIST"
                                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupList).Value) = False Then
                                                                        oQuote.CorrespondenceType = oControl.Value
                                                                    End If
                                                                Case "LOOKUPLISTV2"
                                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupListV2).Value) = False Then
                                                                        oQuote.CorrespondenceType = oControl.Value
                                                                    End If
                                                            End Select
                                                        Case "DEFAULTPREFERREDCORRESPONDENCE"
                                                            'Select Case oControl.GetType.Name.ToUpper
                                                            '    Case "TEXTBOX"
                                                            '        oQuote.DefaultPreferredCorrespondence = CType(oControl, TextBox).Text
                                                            'End Select

                                                        Case "RECEIVESCLIENTCORRESPONDENCE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "HIDDENFIELD"
                                                                    bUpdateQuote = True
                                                                    oQuote.IsAgentReceiveCorrespondence = CBool(CType(oControl, HiddenField).Value)

                                                            End Select
                                                        Case "POLICYHEADER__DEFAULTCORRESPONDENCECODE"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "HIDDDENFIELD"
                                                                    bUpdateQuote = True
                                                                    oQuote.DefaultPreferredCorrespondence = CBool(CType(oControl, HiddenField).Value)
                                                            End Select
                                                        Case "SENDER_EMAIL"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.SenderEmail = CType(oControl, TextBox).Text
                                                                    End If
                                                            End Select
                                                        Case "RECEIVER_EMAIL"
                                                            Select Case oControl.GetType.Name.ToUpper
                                                                Case "TEXTBOX"
                                                                    If CType(oControl, TextBox).Text.Trim.Length <> 0 Then
                                                                        bUpdateQuote = True
                                                                        oQuote.ReceiverEmail = CType(oControl, TextBox).Text
                                                                    End If
                                                            End Select

                                                    End Select

                                            End Select

                                        Case "CLIENTDETAILS"
                                            Select Case sControlName(1)
                                                Case "COMPANY_NAME"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If String.IsNullOrEmpty(CType(oControl, TextBox).Text) = False Then
                                                                Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                                bSaveParty = True
                                                                If TypeOf oParty Is NexusProvider.CorporateParty Then
                                                                    oParty = CType(oParty, NexusProvider.CorporateParty)
                                                                    With CType(oParty, NexusProvider.CorporateParty)
                                                                        .CompanyName = CType(oControl, TextBox).Text
                                                                    End With
                                                                ElseIf TypeOf oParty Is NexusProvider.PersonalParty Then
                                                                    oParty = CType(oParty, NexusProvider.PersonalParty)
                                                                    With CType(oParty, NexusProvider.PersonalParty)
                                                                        .TradingName = CType(oControl, TextBox).Text
                                                                    End With
                                                                End If
                                                                Current.Session(CNParty) = oParty
                                                                oParty = Nothing
                                                            End If
                                                    End Select
                                                Case "TELEPHONE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            Dim iCount As Integer
                                                            Dim bTelephoneFlag As Boolean = False
                                                            Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                                                            Dim oNewContact As New NexusProvider.Contact
                                                            For iCount = 0 To oColl.Count - 1
                                                                If oColl(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.Number AndAlso oColl(iCount).ContactType = NexusProvider.ContactType.HomePhone AndAlso bTelephoneFlag = False Then
                                                                    oColl(iCount).Number = CType(oControl, TextBox).Text
                                                                    bTelephoneFlag = True
                                                                End If
                                                            Next
                                                            If bTelephoneFlag = False Then
                                                                oNewContact.Number = CType(oControl, TextBox).Text
                                                                oNewContact.ContactType = NexusProvider.ContactType.HomePhone
                                                                oNewContact.ContactDetailType = NexusProvider.ItemChoiceTypes.Number
                                                                oColl.Add(oNewContact)
                                                            End If

                                                            For iCount = 0 To oColl.Count - 1
                                                                oParty.Contacts(iCount) = oColl(iCount)
                                                            Next
                                                            bSaveParty = True
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select
                                                Case "E_MAIL"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            Dim iCount As Integer
                                                            Dim bEmailFlag As Boolean = False
                                                            Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                                                            Dim oNewContact As New NexusProvider.Contact
                                                            For iCount = 0 To oColl.Count - 1
                                                                If oColl(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress AndAlso oColl(iCount).ContactType = NexusProvider.ContactType.Email AndAlso bEmailFlag = False Then
                                                                    oColl(iCount).Number = CType(oControl, TextBox).Text
                                                                    bEmailFlag = True
                                                                End If
                                                            Next
                                                            If bEmailFlag = False Then
                                                                oNewContact.Number = CType(oControl, TextBox).Text
                                                                oNewContact.ContactType = NexusProvider.ContactType.Email
                                                                oNewContact.ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress
                                                                oColl.Add(oNewContact)
                                                            End If

                                                            For iCount = 0 To oColl.Count - 1
                                                                oParty.Contacts(iCount) = oColl(iCount)
                                                            Next
                                                            bSaveParty = True
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select

                                                Case "ADDRESS_LINE1"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            If String.IsNullOrEmpty(CType(oControl, TextBox).Text) = False Then
                                                                Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                                If oParty IsNot Nothing AndAlso oPortal.AnnPartyID = oParty.Key Then
                                                                    'Anonymous Quote Clear Address information of AnnParty
                                                                    Dim oAddressColl As NexusProvider.AddressCollection = oParty.Addresses
                                                                    Dim oNewAddress As New NexusProvider.Address
                                                                    Dim iCount As Integer
                                                                    For iCount = 0 To oAddressColl.Count - 1
                                                                        If oAddressColl(iCount).AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                                                                            oNewAddress.CountryCode = oAddressColl(iCount).CountryCode
                                                                        End If
                                                                        oAddressColl.Remove(iCount)
                                                                    Next
                                                                    oNewAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                                                                    oNewAddress.Address1 = CType(oControl, TextBox).Text
                                                                    oAddressColl.Add(oNewAddress)
                                                                Else
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address1 = CType(oControl, TextBox).Text
                                                                End If
                                                                bSaveParty = True
                                                                Current.Session(CNParty) = oParty
                                                                oParty = Nothing
                                                            End If
                                                    End Select
                                                Case "ADDRESS_LINE2"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address2 = CType(oControl, TextBox).Text
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select
                                                Case "ADDRESS_LINE3"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address3 = CType(oControl, TextBox).Text
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select
                                                Case "ADDRESS_LINE4"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4 = CType(oControl, TextBox).Text
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                        Case "LOOKUPLIST"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupList).Value) = False Then
                                                                'oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4 = CType(oControl, NexusProvider.LookupList).Text
                                                                If CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4 = oControl.Value

                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateCode = oControl.Value
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateKey = GetKeyForDescription(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupList).Text, "STATE", False)
                                                                ElseIf CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4 = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupList).Value, "STATE", True)

                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateCode = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupList).Value, "STATE", True)
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateKey = CType(oControl, NexusProvider.LookupList).Value
                                                                End If
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupListV2).Value) = False Then
                                                                oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address4 = CType(oControl, NexusProvider.LookupListV2).Text
                                                                If CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateCode = oControl.Value
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateKey = GetKeyForDescription(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupListV2).Text, "STATE", False)
                                                                ElseIf CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateCode = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupListV2).Value, "STATE", False)
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).StateKey = CType(oControl, NexusProvider.LookupListV2).Value
                                                                End If
                                                            End If
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select
                                                Case "POSTCODE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "TEXTBOX"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).PostCode = CType(oControl, TextBox).Text
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select

                                                Case "COUNTRYCODE"
                                                    Select Case oControl.GetType.Name.ToUpper
                                                        Case "LOOKUPLIST"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupList).Value) = False Then
                                                                If CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode = CType(oControl, NexusProvider.LookupList).Value
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey = GetKeyForDescription(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupList).Text, "COUNTRY", False)
                                                                ElseIf CType(oControl, NexusProvider.LookupList).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupList).Value, "COUNTRY", True)
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey = CType(oControl, NexusProvider.LookupList).Value

                                                                End If
                                                            End If
                                                        Case "LOOKUPLISTV2"
                                                            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                            bSaveParty = True
                                                            If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupListV2).Value) = False Then
                                                                If CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Code Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode = CType(oControl, NexusProvider.LookupListV2).Value
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey = GetKeyForDescription(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupListV2).Text, "COUNTRY", False)
                                                                ElseIf CType(oControl, NexusProvider.LookupListV2).DataItemValue = NexusProvider.DataItemTypes.Key Then
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oControl, NexusProvider.LookupListV2).Value, "COUNTRY", True)
                                                                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).CountryKey = CType(oControl, NexusProvider.LookupListV2).Value

                                                                End If
                                                            End If
                                                            Current.Session(CNParty) = oParty
                                                            oParty = Nothing
                                                    End Select
                                                Case "ADDRESS_LINE5"
                                                    Dim tem As String = sControlName(1)
                                                    bSaveParty = SetControlValueToObject(oControl, sControlName(1))

                                                Case "ADDRESS_LINE6", "ADDRESS_LINE7", "ADDRESS_LINE8", "ADDRESS_LINE9", "ADDRESS_LINE10"

                                            End Select

                                        Case "DOB"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                    bSaveParty = True
                                                    If TypeOf oParty Is NexusProvider.PersonalParty Then
                                                        oParty = CType(oParty, NexusProvider.PersonalParty)
                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            If String.IsNullOrEmpty(CType(oControl, TextBox).Text) = False Then
                                                                If IsDate(CType(oControl, TextBox).Text) = True Then
                                                                    .DateOfBirth = DirectCast(CDate(CType(oControl, TextBox).Text), Date)
                                                                Else
                                                                    .DateOfBirth = Nothing
                                                                End If
                                                            Else
                                                                .DateOfBirth = Nothing
                                                            End If
                                                        End With
                                                    End If
                                            End Select
                                        Case "MOBILE"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                    Dim iCount As Integer
                                                    Dim bTelephoneFlag As Boolean = False
                                                    Dim oColl As NexusProvider.ContactCollection = oParty.Contacts
                                                    Dim oNewContact As New NexusProvider.Contact
                                                    For iCount = 0 To oColl.Count - 1
                                                        If oColl(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.Number AndAlso oColl(iCount).ContactType = NexusProvider.ContactType.Mobile AndAlso bTelephoneFlag = False Then
                                                            oColl(iCount).Number = CType(oControl, TextBox).Text
                                                            bTelephoneFlag = True
                                                        End If
                                                    Next
                                                    If bTelephoneFlag = False Then
                                                        oNewContact.Number = CType(oControl, TextBox).Text
                                                        oNewContact.ContactType = NexusProvider.ContactType.Mobile
                                                        oNewContact.ContactDetailType = NexusProvider.ItemChoiceTypes.Number
                                                        oColl.Add(oNewContact)
                                                    End If

                                                    For iCount = 0 To oColl.Count - 1
                                                        oParty.Contacts(iCount) = oColl(iCount)
                                                    Next
                                                    bSaveParty = True
                                                    Current.Session(CNParty) = oParty
                                                    oParty = Nothing
                                            End Select
                                        Case "BRANCHCODE"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                    bSaveParty = True
                                                    oParty.BranchCode = CType(oControl, TextBox).Text
                                                    Current.Session(CNParty) = oParty
                                                    oParty = Nothing
                                            End Select
                                        Case "FORENAME"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If String.IsNullOrEmpty(CType(oControl, TextBox).Text) = False Then
                                                        Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                        bSaveParty = True

                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            .Forename = CType(oControl, TextBox).Text
                                                        End With

                                                        Current.Session(CNParty) = oParty
                                                        oParty = Nothing
                                                    End If
                                            End Select
                                        Case "SURNAME"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If String.IsNullOrEmpty(CType(oControl, TextBox).Text) = False Then
                                                        Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                        bSaveParty = True

                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            .Lastname = CType(oControl, TextBox).Text
                                                        End With

                                                        Current.Session(CNParty) = oParty
                                                        oParty = Nothing
                                                    End If
                                            End Select

                                        Case "TITLE"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "LOOKUPLIST"
                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupList).Value) = False Then
                                                        Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                        bSaveParty = True

                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            .Title = CType(oControl, NexusProvider.LookupList).Text
                                                        End With

                                                        Current.Session(CNParty) = oParty
                                                        oParty = Nothing
                                                    End If
                                                Case "LOOKUPLISTV2"
                                                    If String.IsNullOrEmpty(CType(oControl, NexusProvider.LookupListV2).Value) = False Then
                                                        Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                        bSaveParty = True

                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            .Title = CType(oControl, NexusProvider.LookupListV2).Text
                                                        End With

                                                        Current.Session(CNParty) = oParty
                                                        oParty = Nothing
                                                    End If
                                            End Select

                                        Case "INITIAL"
                                            Select Case oControl.GetType.Name.ToUpper
                                                Case "TEXTBOX"
                                                    If String.IsNullOrEmpty(CType(oControl, TextBox).Text) = False Then
                                                        Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                                                        bSaveParty = True

                                                        With CType(oParty, NexusProvider.PersonalParty)
                                                            .Initials = CType(oControl, TextBox).Text
                                                        End With

                                                        Current.Session(CNParty) = oParty
                                                        oParty = Nothing
                                                    End If
                                            End Select
                                    End Select

                                    Select Case oControl.GetType.Name

                                        Case "Panel"
                                            'Need to store the read element in XML before moving into another container
                                            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                            oDataSet.Terminate()
                                            oDataSet = Nothing

                                            WriteContainerToXML(oControl, v_sScreenCode, v_sOI, True)

                                            'Need to load the updated dataset
                                            oDataSet = New DataSetControl.Application
                                            sDataSetDefinition = GetDataSetDefinition(Current.Session(CNDataModelCode))
                                            oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                            sDataSetDefinition = Nothing

                                        Case "UpdatePanel"
                                            'Need to store the read element in XML before moving into another container
                                            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                            oDataSet.Terminate()
                                            oDataSet = Nothing

                                            Dim oCtrl As Control
                                            For Each oCtrl In oControl.Controls

                                                WriteContainerToXML(oCtrl, v_sScreenCode, v_sOI, True)

                                                'Need to load the updated dataset
                                                oDataSet = New DataSetControl.Application
                                                sDataSetDefinition = GetDataSetDefinition(Current.Session(CNDataModelCode))
                                                oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                sDataSetDefinition = Nothing
                                            Next

                                        Case "HtmlGenericControl"
                                            'Need to store the read element in XML before moving into another container
                                            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                            oDataSet.Terminate()
                                            oDataSet = Nothing

                                            WriteContainerToXML(oControl, v_sScreenCode, v_sOI, True)

                                            'Need to load the updated dataset
                                            oDataSet = New DataSetControl.Application
                                            sDataSetDefinition = GetDataSetDefinition(Current.Session(CNDataModelCode))
                                            oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                            sDataSetDefinition = Nothing

                                        Case "HiddenField"

                                            sControlValue = CType(oControl, HiddenField).Value
                                            bSave = True

                                        Case "HtmlInputText"

                                            sControlValue = CType(oControl, HtmlInputText).Value
                                            bSave = True

                                        Case "TextBox"

                                            sControlValue = CType(oControl, TextBox).Text
                                            bSave = True

                                        Case "CheckBox"

                                            sControlValue = IIf(CType(oControl, CheckBox).Checked, "1", "0")
                                            bSave = True

                                        Case "RadioButton"

                                            sControlValue = IIf(CType(oControl, RadioButton).Checked, "1", "0")
                                            bSave = True

                                        Case "RadioButtonList"

                                            If oControl.SelectedIndex = -1 Then
                                                sControlValue = 2
                                            Else
                                                sControlValue = oControl.SelectedValue
                                            End If

                                            bSave = True

                                        Case "LookupList"

                                            sControlValue = oControl.Value
                                            bSave = True

                                        Case "LookupListV2"
                                            sControlValue = oControl.Value
                                            bSave = True
                                        Case "ChildRepeater"
                                            For Each item As RepeaterItem In oControl.items
                                                'Need to store the read element in XML before moving into another container
                                                oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                oDataSet.Terminate()
                                                oDataSet = Nothing

                                                WriteContainerToXML(item, v_sScreenCode, v_sOI, True)

                                                'Need to load the updated dataset
                                                oDataSet = New DataSetControl.Application
                                                sDataSetDefinition = GetDataSetDefinition(Current.Session(CNDataModelCode))
                                                oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                sDataSetDefinition = Nothing

                                            Next

                                        Case "EditItemGrid"
                                            'Cast the oControl to EditItemGrid
                                            Dim TempGrid As EditItemGrid = CType(oControl, EditItemGrid)
                                            Dim arrColumnNames As String()
                                            Dim arrColumnTypes As String()
                                            Dim arrColumnEditable As String()
                                            'Get the column names into array
                                            arrColumnNames = TempGrid.ChildColumnNames.Split(",")
                                            'Get the column types into array
                                            arrColumnTypes = TempGrid.ChildColumnTypes.Split(",")
                                            'Get the column Editable or not into array
                                            arrColumnEditable = TempGrid.ChildColumnIsEditable.Split(",")
                                            'Loop the each row of grid to save data of every row
                                            For iCount As Integer = 0 To TempGrid.Rows.Count - 1
                                                'Loop the each column of grid to save data of every column
                                                For cCount As Integer = 0 To arrColumnNames.Length - 1
                                                    'Check whether column is editable or not
                                                    If arrColumnEditable(cCount).Trim = "1" Then
                                                        'Cast the column to control to check the control type
                                                        Dim oCtrl As Control = TempGrid.Rows(iCount).FindControl(arrColumnNames(cCount).Trim)
                                                        If oCtrl IsNot Nothing Then
                                                            If TypeOf oCtrl Is TextBox Then
                                                                'Save the text box value to xml
                                                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(arrColumnNames(cCount), "__"), CType(oCtrl, TextBox).Text, CType(oCtrl, TextBox).ToolTip)
                                                            ElseIf TypeOf oCtrl Is DropDownList Then
                                                                'Save the dropdown value to xml
                                                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(arrColumnNames(cCount), "__"), CType(oCtrl, DropDownList).SelectedValue, CType(oCtrl, DropDownList).ToolTip)
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            Next
                                            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                        Case "EditItemGridExtended"
                                            'Cast the oControl to EditItemGridExtended
                                            Dim TempGrid As EditItemGridExtended = CType(oControl, EditItemGridExtended)
                                            'Loop the each row of grid to save data of every row
                                            For iCount As Integer = 0 To TempGrid.Rows.Count - 1
                                                'Loop the each column of grid to save data of every column
                                                For cCount As Integer = 0 To TempGrid.Columns.Count - 1
                                                    Dim strColumnName As String = String.Empty
                                                    'Check for Text box 
                                                    If TypeOf DirectCast(DirectCast(DirectCast(TempGrid.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate) Is TextBoxTemplate Then
                                                        'Get the text box column name
                                                        strColumnName = DirectCast(DirectCast(DirectCast(DirectCast(TempGrid.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate), Nexus.TextBoxTemplate).ColumnName
                                                        'Check for Drop down list 
                                                    ElseIf TypeOf DirectCast(DirectCast(DirectCast(TempGrid.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate) Is DropDownListTemplate Then
                                                        'Get the Drop down list column name
                                                        strColumnName = DirectCast(DirectCast(DirectCast(DirectCast(TempGrid.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate), Nexus.DropDownListTemplate).ColumnName
                                                    End If
                                                    'Check there is a edit table column in grid means text box or dropdown list etc.
                                                    If Not String.IsNullOrEmpty(strColumnName) Then
                                                        Dim oCtrl As Control = TempGrid.Rows(iCount).FindControl(strColumnName)
                                                        If oCtrl IsNot Nothing Then
                                                            If TypeOf oCtrl Is TextBox Then
                                                                'Save the text box value to xml
                                                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(strColumnName, "__"), CType(oCtrl, TextBox).Text, CType(oCtrl, TextBox).ToolTip)
                                                            ElseIf TypeOf oCtrl Is DropDownList Then
                                                                'Save the dropdown value to xml
                                                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(strColumnName, "__"), CType(oCtrl, DropDownList).SelectedValue, CType(oCtrl, DropDownList).ToolTip)
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            Next

                                            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                        Case "DropDownList"
                                            sControlValue = CType(oControl, DropDownList).SelectedValue
                                            bSave = True
                                        Case Else

                                            Select Case True

                                                Case oControl.GetType.Name.Contains("controls_addresscntrl_ascx")

                                                    Dim iAddressKey As Integer

                                                    Try
                                                        iAddressKey = oControl.AddressKey()

                                                        If iAddressKey <> 0 Then
                                                            sControlValue = iAddressKey
                                                            bSave = True
                                                        End If

                                                    Catch ex As ArgumentNullException

                                                        'DH - 01-02-08 - Catch and ignore any argument null exceptions from
                                                        'the AddAddress method, as if the address is missing a parameter we
                                                        'don't want to save it to the dataset

                                                    End Try
                                                    'Case oControl.GetType.Name.Contains("controls_standardwordings_ascx")
                                                    '    'Create a collection for all Standard Wording controls.So that we can update XML for these controls in last
                                                    '    Dim oStandardWordingControls As Collection
                                                    '    If Current.Session("SWOnThisRiskPage") IsNot Nothing Then
                                                    '        oStandardWordingControls = Current.Session("SWOnThisRiskPage")
                                                    '    Else
                                                    '        oStandardWordingControls = New Collection
                                                    '    End If
                                                    '    oStandardWordingControls.Add(oControl)
                                                    '    Current.Session("SWOnThisRiskPage") = oStandardWordingControls
                                                Case oControl.GetType.Name.Contains("controls_currencies_ascx")

                                                    Dim sCurrencyCode As String
                                                    sCurrencyCode = oControl.SelectedValue

                                                    If String.IsNullOrEmpty(sCurrencyCode) = False Then
                                                        oQuote.CurrencyCode = sCurrencyCode
                                                        Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
                                                        bUpdateQuote = True
                                                    End If



                                                Case oControl.GetType.Name.Contains("controls_findparty_ascx")
                                                    Dim iPartyKey As Integer
                                                    iPartyKey = oControl.PartyKey
                                                    'If iPartyKey <> 0 Then
                                                    sControlValue = iPartyKey
                                                    bSave = True
                                                    'End If

                                                Case oControl.GetType.Name.Contains("controls_standardwordings_ascx")
                                                    Dim bRiskLevel As Boolean = Convert.ToBoolean(oControl.SupportRiskLevel)
                                                    If bRiskLevel Then
                                                        'Create a collection for all Standard Wording controls.So that we can update XML for these controls in last
                                                        Dim oStandardWordingControls As Collection
                                                        If Current.Session("SWOnThisRiskPage") IsNot Nothing Then
                                                            oStandardWordingControls = Current.Session("SWOnThisRiskPage")
                                                        Else
                                                            oStandardWordingControls = New Collection
                                                        End If
                                                        oStandardWordingControls.Add(oControl)
                                                        Current.Session("SWOnThisRiskPage") = oStandardWordingControls
                                                    Else

                                                        'Need to store the read element in XML before moving into another container
                                                        oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

                                                        If r_bMultiStdWrd = False Then
                                                            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                        End If
                                                        oControl.SubmitSelections(r_bSkipSave, r_bMultiStdWrd)

                                                        oDataSet = New DataSetControl.Application
                                                        sDataSetDefinition = GetDataSetDefinition(Current.Session(CNDataModelCode))
                                                        oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                                        sDataSetDefinition = Nothing

                                                        'Create a collection for all Standard Wording controls.So that we can update XML for these controls in last
                                                    End If
                                            End Select
                                    End Select

                                    If bSave Then
                                        WriteAttributeToXML(oDoc, oDataSet, sControlName, sControlValue, v_sOI)
                                    End If

                                End If

                            End If
                        End If
                    Next

                    'if any standard wording control found on the page then need to update XML
                    If Current.Session("SWOnThisRiskPage") IsNot Nothing Then

                        oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

                        Dim oStandardWordings As Collection = Current.Session("SWOnThisRiskPage")

                        For Each oControl In oStandardWordings
                            sControlName = Regex.Split(oControl.ID.ToUpper(), "__")
                            'Save standard wording control 
                            Dim sSWOI As String = ""
                            If Current.Session.Item(CNTempOI) Is Nothing Then
                                Dim oOI As Collections.Stack
                                oOI = Current.Session.Item(CNOI)
                                Dim iPreviousOICount As Integer = oOI.Count()
                                ReteriveOIFromXML(oDoc, sControlName)
                                oOI = Current.Session.Item(CNOI)
                                'rajeev
                                If oOI.Count > 0 Then
                                    sSWOI = oOI.Peek().ToString()
                                End If
                                'sSWOI = oOI.Peek().ToString()
                                If oOI.Count > iPreviousOICount Then
                                    oOI.Pop()
                                    Current.Session.Item(CNOI) = oOI
                                End If
                            Else
                                sSWOI = Current.Session.Item(CNTempOI)
                            End If

                            oControl.SubmitSelections(r_bSkipSave, sSWOI)

                        Next
                        Current.Session.Remove("SWOnThisRiskPage")
                    End If

                    If r_bSkipSave = False Then
                        oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    End If
                    oDataSet.Terminate()
                    oDataSet = Nothing

                    Try
                        If bUpdateQuote And v_bStatus = False And Current.Session(CNMode) <> Mode.View And Current.Session(CNMode) <> Mode.Review Then
                            'if user has not supplied the Quote Expiry date in case of Renewal
                            If Current.Session(CNRenewal) IsNot Nothing AndAlso oQuote.QuoteExpiryDate = Date.MinValue Then
                                oQuote.QuoteExpiryDate = oQuote.CoverStartDate.AddDays(1)
                            End If
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        End If

                    Finally
                        oWebService = Nothing
                    End Try

                    Current.Session(CNQuote) = oQuote

                    If bSaveParty = True Then
                        If CType(Current.Session.Item(CNLoginType), LoginType) = LoginType.Agent _
                            And Current.Session(CNAnonymous) IsNot Nothing And Current.Session(CNIsAnonymous) = True And
                            CType(Current.Session(CNIsTransferQuoteRequired), Boolean) = False Then
                            'Add a new party if client detail filed captured first time from risk screen and quote is anonymous 
                            AddRealParty()

                        Else
                            If Current.Session(CNIsAnonymous) Is Nothing Or Current.Session(CNIsAnonymous) = False Then
                                'Update existing party details with new client detail fields captured from risk screens
                                UpdatePartyDetails()

                            End If
                        End If
                    End If
                End If

            End If

        End Sub

        Function CreateElementFromXML(ByVal v_sScreenCode As String,
                                    ByVal v_sParentOI As String,
                                    ByVal v_sParentElement As String,
                                    ByVal v_sChildElement As String,
                                    Optional ByVal v_bCallFromEditItemGrid As Boolean = False) As String

            Dim sOI As String = String.Empty
            If Current.Session(CNDataSet) IsNot Nothing AndAlso Current.Session(CNClaimQuote) IsNot Nothing Then
                'For Claims

                'DH - 30-01-08 - Fix screen having multiple elements at the same level with child elements.
                Dim oClaimQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
                Dim srDataset As New System.IO.StringReader(Current.Session(CNDataSet))
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()

                'Dim oDefaultNodes As New Hashtable()
                Dim oNode As XmlNode = Doc.SelectSingleNode("//" & v_sParentElement & "[@OI='" & v_sParentOI & "']")
                If oNode Is Nothing Then
                    'Parent Element doesn't match the Element Name returned by the OI
                    oNode = Doc.SelectSingleNode("//" & v_sParentElement)
                    v_sParentOI = oNode.Attributes("OI").Value
                End If


                '------------------------------------------------------------------------------------------
                srDataset = New System.IO.StringReader(Current.Session(CNDataSet))
                xmlTR = New XmlTextReader(srDataset)

                Doc.Load(xmlTR)
                xmlTR.Close()

                'While Creating the grandchild item, child item's attribute value is going as "3".
                'so we are replacing the attribute value "3" to "2" (if Exists)
                oNode = Doc.SelectSingleNode("//*[@OI = '" & v_sParentOI & "']")
                If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value = "3" Then
                    oNode.Attributes("US").Value = "2"
                End If


                Dim tempswContent As New System.IO.StringWriter
                Dim tempxmlwContent As New XmlTextWriter(tempswContent)

                Doc.WriteTo(tempxmlwContent)
                Current.Session(CNDataSet) = tempswContent.ToString()

                tempxmlwContent.Close()
                tempswContent.Close()
                '------------------------------------------------------------------------------------------

                Dim oDataset As New DataSetControl.Application

                oDataset.LoadFromXML(ClaimGetDataSetDefinition(), Current.Session(CNDataSet))
                oDataset.NewObjectInstance(v_sChildElement, sOI, v_sParentOI)
                oDataset.ReturnAsXML(Current.Session(CNDataSet))


                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    Current.Session(CNDataSet) = oWebService.RunDefaultRulesAdd(v_sScreenCode, Current.Session(CNDataSet), oClaimQuote.BranchCode, False)
                Finally
                    oWebService = Nothing
                End Try

                'Set the newly create element as being deleted
                srDataset = New System.IO.StringReader(Current.Session(CNDataSet))
                xmlTR = New XmlTextReader(srDataset)

                Doc.Load(xmlTR)
                xmlTR.Close()
                srDataset.Close()
                oNode = Doc.SelectSingleNode("//*[@OI = '" & sOI & "']")
                If oNode IsNot Nothing And v_bCallFromEditItemGrid = False Then
                    oNode.Attributes("US").Value = "3"
                End If

                'Add the UC attrinbute to identify the valid/invalid node
                Dim xUCAttr As XmlAttribute
                xUCAttr = oNode.Attributes("UC")
                If xUCAttr IsNot Nothing Then
                    xUCAttr.Value = "1"
                Else
                    xUCAttr = oNode.OwnerDocument.CreateAttribute("UC")
                    oNode.Attributes.Append(xUCAttr)
                    oNode.Attributes("UC").Value = "1"
                End If

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                Doc.WriteTo(xmlwContent)
                Current.Session(CNDataSet) = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()
            Else
                If Current.Session(CNQuote) IsNot Nothing Then
                    'For New Business
                    Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)

                    'DH - 30-01-08 - Fix screen having multiple elements at the same level with child elements.

                    Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim Doc As New XmlDocument

                    Doc.Load(xmlTR)
                    xmlTR.Close()

                    'Dim oDefaultNodes As New Hashtable()
                    Dim oNode As XmlNode = Doc.SelectSingleNode("//" & v_sParentElement & "[@OI='" & v_sParentOI & "']")
                    If oNode Is Nothing Then
                        'Parent Element doesn't match the Element Name returned by the OI
                        oNode = Doc.SelectSingleNode("//" & v_sParentElement)
                        v_sParentOI = oNode.Attributes("OI").Value
                    End If


                    '------------------------------------------------------------------------------------------
                    srDataset = New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    xmlTR = New XmlTextReader(srDataset)

                    Doc.Load(xmlTR)
                    xmlTR.Close()
                    srDataset.Close()
                    'While Creating the grandchild item, child item's attribute value is going as "3".
                    'so we are replacing the attribute value "3" to "2" (if Exists)
                    oNode = Doc.SelectSingleNode("//*[@OI = '" & v_sParentOI & "']")
                    If oNode IsNot Nothing AndAlso oNode.Attributes("US").Value = "3" Then
                        oNode.Attributes("US").Value = "2"
                    End If


                    Dim tempswContent As New System.IO.StringWriter
                    Dim tempxmlwContent As New XmlTextWriter(tempswContent)

                    Doc.WriteTo(tempxmlwContent)
                    oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = tempswContent.ToString()

                    tempxmlwContent.Close()
                    tempswContent.Close()
                    '------------------------------------------------------------------------------------------

                    Dim oDataset As New DataSetControl.Application

                    oDataset.LoadFromXML(GetDataSetDefinition(Current.Session(CNDataModelCode)), oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    oDataset.NewObjectInstance(v_sChildElement, sOI, v_sParentOI)
                    oDataset.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                    Try

                        oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesAdd(v_sScreenCode, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset, oQuote.BranchCode)

                    Finally
                        oWebService = Nothing
                    End Try

                    'Set the newly create element as being deleted
                    srDataset = New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    xmlTR = New XmlTextReader(srDataset)

                    Doc.Load(xmlTR)
                    xmlTR.Close()
                    srDataset.Close()
                    oNode = Doc.SelectSingleNode("//*[@OI = '" & sOI & "']")
                    If oNode IsNot Nothing And v_bCallFromEditItemGrid = False Then
                        oNode.Attributes("US").Value = "3"
                    End If

                    'Add the UC attrinbute to identify the valid/invalid node
                    Dim xUCAttr As XmlAttribute
                    xUCAttr = oNode.Attributes("UC")
                    If xUCAttr IsNot Nothing Then
                        xUCAttr.Value = "1"
                    Else
                        xUCAttr = oNode.OwnerDocument.CreateAttribute("UC")
                        oNode.Attributes.Append(xUCAttr)
                        oNode.Attributes("UC").Value = "1"
                    End If

                    Dim swContent As New System.IO.StringWriter
                    Dim xmlwContent As New XmlTextWriter(swContent)

                    Doc.WriteTo(xmlwContent)
                    oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                    xmlwContent.Close()
                    swContent.Close()

                    Current.Session(CNQuote) = oQuote

                End If
            End If

            Return sOI

        End Function

        ''' <summary>
        ''' Delete an element from the risk dataset
        ''' </summary>
        ''' <param name="v_sScreenCode">The screen code of the element to be removed</param>
        ''' <param name="v_sOI">The dataset identifier of the element to be removed</param>
        ''' <param name="v_sChildElement">The element name of the element to be removed</param>
        ''' <remarks></remarks>
        Sub DeleteElementFromXML(ByVal v_sScreenCode As String,
                                ByVal v_sOI As String,
                                ByVal v_sChildElement As String)

            If Current.Session(CNDataSet) IsNot Nothing Then
                Dim oClaimQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
                'Delete option for claims
                Dim oDataSet As New DataSetControl.Application
                oDataSet.LoadFromXML(ClaimGetDataSetDefinition(), Current.Session(CNDataSet))

                If v_sChildElement IsNot Nothing And v_sOI IsNot Nothing Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Try
                        oDataSet.DelObjectInstance(v_sChildElement, v_sOI)
                        oDataSet.ReturnAsXML(Current.Session(CNDataSet))
                        oDataSet.Terminate()
                        oDataSet = Nothing
                        Current.Session(CNDataSet) = oWebService.RunDefaultRulesEdit(v_sScreenCode, Current.Session(CNDataSet), Nothing, oClaimQuote.BranchCode)
                    Catch ex As Exception
                    Finally
                        oWebService = Nothing
                    End Try
                Else
                    'Child/GrandcHild is added abnormally, v_sOI and v_sChildElement is Nothing 
                    'if method is called from BaseClaim and BasePeril
                    Dim Doc As New XmlDocument
                    Using srDataset As New System.IO.StringReader(Current.Session(CNDataSet))

                        Dim xmlTR As New XmlTextReader(srDataset)

                        Doc.Load(xmlTR)
                        xmlTR.Close()
                    End Using
                    Dim xmlNodes As XmlNodeList = Doc.SelectNodes("//" & Current.Session.Item(CNDataModelCode) & "_POLICY_BINDER//*[@OI='" & v_sOI & "']")

                    For Each xmlNode As XmlNode In xmlNodes
                        Dim sID As String = Nothing
                        'NODEISVALID is 0 means Invalid Data otherwise 1 means Valid Data
                        'If xmlNode.Attributes("NODEISVALID").Value = "0" Then
                        sID &= Current.Session.Item(CNDataModelCode) & "_"
                        sID &= xmlNode.Name & "_ID"
                        'Checking the Valid attribute values
                        If (xmlNode.Attributes("" & sID.Trim & "") IsNot Nothing AndAlso
                        (String.IsNullOrEmpty(xmlNode.Attributes("" & sID.Trim & "").Value) = False _
                        And String.IsNullOrEmpty(xmlNode.Attributes("OI").Value) = False _
                        And String.IsNullOrEmpty(v_sOI) = True) _
                        And xmlNode.Attributes("US").Value = "3") Then
                            'Deleting the invalid Child
                            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                            Try
                                oDataSet.DelObjectInstance(xmlNode.Attributes("" & sID.Trim & "").Value, xmlNode.Attributes("OI").Value)
                                oDataSet.ReturnAsXML(Current.Session(CNDataSet))
                                oDataSet.Terminate()
                                oDataSet = Nothing
                                Current.Session(CNDataSet) = oWebService.RunDefaultRulesEdit(v_sScreenCode, Current.Session(CNDataSet), Nothing, oClaimQuote.BranchCode)
                            Catch ex As Exception
                            Finally
                                oWebService = Nothing
                            End Try
                        Else
                            'Child/GrandcHild is added abnormally, v_sOI and v_sChildElement is Nothing 
                            'if method is called from Back Button
                            'if user press Back Button and pass the OI value
                            If String.IsNullOrEmpty(v_sOI) = False Then
                                If (xmlNode.Attributes("" & sID.Trim & "") IsNot Nothing AndAlso (String.IsNullOrEmpty(xmlNode.Attributes("" & sID.Trim & "").Value) = False _
                                    And xmlNode.Attributes("OI").Value = v_sOI) And (xmlNode.Attributes("US").Value = "3")) Then
                                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                    'Deleting the invalid Child
                                    Try
                                        oDataSet.DelObjectInstance(xmlNode.Attributes("" & sID.Trim & "").Value, xmlNode.Attributes("OI").Value)
                                        oDataSet.ReturnAsXML(Current.Session(CNDataSet))
                                        oDataSet.Terminate()
                                        oDataSet = Nothing
                                        Current.Session(CNDataSet) = oWebService.RunDefaultRulesEdit(v_sScreenCode, Current.Session(CNDataSet), Nothing, oClaimQuote.BranchCode)
                                    Catch ex As Exception
                                    Finally
                                        oWebService = Nothing
                                    End Try
                                End If
                            End If
                        End If
                        'End If
                    Next
                End If
            ElseIf Current.Session(CNQuote) IsNot Nothing Then
                'Delete for New Business
                Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                Dim oDataSet As New DataSetControl.Application
                Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)

                oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                sDataSetDefinition = Nothing

                If v_sChildElement IsNot Nothing AndAlso v_sOI IsNot Nothing Then
                    If oProduct.AutoSave = False Then
                        oDataSet.DelObjectInstance(v_sChildElement, v_sOI)
                        oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                        oDataSet.Terminate()
                        oDataSet = Nothing

                        Dim oDoc As New XmlDocument
                        'Clear Delete Objects from XML
                        Using srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                            Dim xmlTR As New XmlTextReader(srDataset)


                            oDoc.Load(xmlTR)
                            xmlTR.Close()
                        End Using
                        Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI = 'DELETED_OBJECTS']")

                        If oNode IsNot Nothing Then

                            For Each oChildNode As XmlNode In oNode.ChildNodes
                                oNode.RemoveChild(oChildNode)
                            Next

                        End If

                        Dim tempswContent As New System.IO.StringWriter
                        Dim tempxmlwContent As New XmlTextWriter(tempswContent)

                        oDoc.WriteTo(tempxmlwContent)
                        oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = tempswContent.ToString()

                        tempxmlwContent.Close()
                        tempswContent.Close()

                        Current.Session(CNQuote) = oQuote
                    Else
                        'Save the Data on Click of the every Addition of the child
                        'To Save the child data of Inline itemgrid with saverisk method
                        Dim oDoc As New XmlDocument
                        Using srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                            Dim xmlTR As New XmlTextReader(srDataset)

                            oDoc.Load(xmlTR)
                            xmlTR.Close()
                        End Using
                        Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI = '" & v_sOI & "']")
                        If oNode IsNot Nothing Then
                            oNode.Attributes("US").Value = "3"
                        End If

                        Dim tempswContent As New System.IO.StringWriter
                        Dim tempxmlwContent As New XmlTextWriter(tempswContent)

                        oDoc.WriteTo(tempxmlwContent)
                        oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = tempswContent.ToString()

                        tempxmlwContent.Close()
                        tempswContent.Close()

                        SaveQuote()

                        'Store the deleted OI to restore the value if user presses back button
                        Dim hDeletedNodeColl As New Hashtable()
                        If Current.Session(CNDeletedNode) IsNot Nothing Then
                            hDeletedNodeColl = CType(Current.Session(CNDeletedNode), Hashtable)
                        End If

                        'Added into collection
                        hDeletedNodeColl.Add(v_sOI, "Deleted")
                        Current.Session(CNDeletedNode) = hDeletedNodeColl
                    End If

                    Current.Session(CNQuote) = oQuote

                End If
            End If
        End Sub
        ''' <summary>
        ''' It will delete the Invalid node from XML as well as from DB too
        ''' </summary>
        ''' <param name="v_sOI"></param>
        ''' <remarks></remarks>
        Sub RemoveInvalidNodeFromXML(ByVal v_sOI As String)
            If Current.Session(CNQuote) IsNot Nothing Then
                'Delete for New Business
                Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                Dim oDataSet As New DataSetControl.Application
                Dim hCurrentNodeColl As New Hashtable()
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Dim ParentNode As XmlNode = Nothing
                Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)

                Doc.Load(xmlTR)
                xmlTR.Close()

                Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & v_sOI & "' and @UC='1']")
                srDataset.Dispose()

                If oNode IsNot Nothing Then
                    ParentNode = oNode.ParentNode

                    If Current.Session(CNNode) Is Nothing Then
                        If oProduct.AutoSave = False Then
                            'if not found then it is in Add Mode, since during edit mode only OI is inserted in to hash table
                            'so we can delete this unwanted child
                            Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                            oSAMClient.LoadFromXML(GetDataSetDefinition(Current.Session(CNDataModelCode)), oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                            oSAMClient.DelObjectInstance(oNode.Name, v_sOI)
                            oSAMClient.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                            oSAMClient.Terminate()
                        Else
                            oNode.Attributes("US").Value = "3"

                            Using swContent As New System.IO.StringWriter
                                Using xmlwContent As New XmlTextWriter(swContent)
                                    Doc.WriteTo(xmlwContent)
                                    oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()
                                End Using
                            End Using

                        End If

                        Current.Session(CNQuote) = oQuote
                    Else
                        hCurrentNodeColl = CType(Current.Session(CNNode), Hashtable)
                        If hCurrentNodeColl IsNot Nothing AndAlso hCurrentNodeColl.ContainsKey(v_sOI) = True Then
                            'if current OI exist in hash table collection
                            For Each hData As DictionaryEntry In hCurrentNodeColl
                                If hData.Key = v_sOI Then
                                    Dim oOldXML As New XmlDocument
                                    oOldXML.LoadXml(hData.Value)

                                    ParentNode.ReplaceChild(Doc.ImportNode(oOldXML.ChildNodes(0), True), oNode)

                                    Exit For
                                End If
                            Next

                            'Updating the XMLDATASET
                            Dim swContent As New System.IO.StringWriter
                            Dim xmlwContent As New XmlTextWriter(swContent)

                            Doc.WriteTo(xmlwContent)
                            oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                            xmlwContent.Close()
                            swContent.Close()


                            'Clean the Used OI
                            hCurrentNodeColl.Remove(v_sOI)

                            Current.Session(CNNode) = hCurrentNodeColl
                            Current.Session(CNQuote) = oQuote

                            'if AutoSave is True, then data is deleted from DB so we need to insert the data again
                            'so i have to make all the deleted node US="1"
                            If oProduct.AutoSave = True Then
                                ReInsertDeletedRecords(v_sOI)
                            End If
                        Else
                            oNode.Attributes("US").Value = "3"

                            Dim swContent As New System.IO.StringWriter
                            Dim xmlwContent As New XmlTextWriter(swContent)
                            Doc.WriteTo(xmlwContent)
                            Current.Session(CNDataSet) = swContent.ToString()

                        End If
                    End If
                End If
            Else
                If Current.Session(CNQuote) IsNot Nothing Then
                    'Delete for New Business
                    Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
                    Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                    Dim oDataSet As New DataSetControl.Application
                    Dim hCurrentNodeColl As New Hashtable()
                    Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim Doc As New XmlDocument
                    Dim ParentNode As XmlNode = Nothing
                    Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)

                    Doc.Load(xmlTR)
                    xmlTR.Close()

                    Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & v_sOI & "' and @UC='1']")
                    srDataset.Dispose()

                    If oNode IsNot Nothing Then
                        ParentNode = oNode.ParentNode

                        If Current.Session(CNNode) Is Nothing Then
                            If oProduct.AutoSave = False Then
                                'if not found then it is in Add Mode, since during edit mode only OI is inserted in to hash table
                                'so we can delete this unwanted child
                                Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                                oSAMClient.LoadFromXML(GetDataSetDefinition(Current.Session(CNDataModelCode)), oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                oSAMClient.DelObjectInstance(oNode.Name, v_sOI)
                                oSAMClient.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                oSAMClient.Terminate()
                            Else
                                oNode.Attributes("US").Value = "3"

                                Using swContent As New System.IO.StringWriter
                                    Using xmlwContent As New XmlTextWriter(swContent)
                                        Doc.WriteTo(xmlwContent)
                                        oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()
                                    End Using
                                End Using
                            End If

                            Current.Session(CNQuote) = oQuote
                        Else
                            hCurrentNodeColl = CType(Current.Session(CNNode), Hashtable)
                            If hCurrentNodeColl IsNot Nothing AndAlso hCurrentNodeColl.ContainsKey(v_sOI) = True Then
                                'if current OI exist in hash table collection
                                For Each hData As DictionaryEntry In hCurrentNodeColl
                                    If hData.Key = v_sOI Then
                                        Dim oOldXML As New XmlDocument
                                        oOldXML.LoadXml(hData.Value)
                                        ParentNode.ReplaceChild(Doc.ImportNode(oOldXML.ChildNodes(0), True), oNode)
                                        Exit For
                                    End If
                                Next

                                'Updating the XMLDATASET
                                Dim swContent As New System.IO.StringWriter
                                Dim xmlwContent As New XmlTextWriter(swContent)

                                Doc.WriteTo(xmlwContent)
                                oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                                xmlwContent.Close()
                                swContent.Close()


                                'Clean the Used OI
                                hCurrentNodeColl.Remove(v_sOI)

                                Current.Session(CNNode) = hCurrentNodeColl
                                Current.Session(CNQuote) = oQuote

                                'if AutoSave is True, then data is deleted from DB so we need to insert the data again
                                'so i have to make all the deleted node US="1"
                                If oProduct.AutoSave = True Then
                                    ReInsertDeletedRecords(v_sOI)
                                End If
                            Else
                                If oProduct.AutoSave = False Then
                                    'if not found then it is in Add Mode, since during edit mode only OI is inserted in to hash table
                                    'so we can delete this unwanted child
                                    Dim oSAMClient As New SiriusFS.SAM.Client.DataSetControl.Application
                                    oSAMClient.LoadFromXML(GetDataSetDefinition(Current.Session(CNDataModelCode)), oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                    oSAMClient.DelObjectInstance(oNode.Name, v_sOI)
                                    oSAMClient.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                                    oSAMClient.Terminate()
                                Else

                                    oNode.Attributes("US").Value = "3"

                                    Dim swContent As New System.IO.StringWriter
                                    Dim xmlwContent As New XmlTextWriter(swContent)
                                    Doc.WriteTo(xmlwContent)
                                    oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()
                                End If

                                Current.Session(CNQuote) = oQuote
                            End If
                        End If
                        If oProduct.AutoSave = True Then
                            SaveQuote()
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' It will delete the Invalid node from XML as well as from DB too
        ''' </summary>
        ''' <param name="v_sOI"></param>
        ''' <remarks></remarks>
        Sub RemoveInvalidClaimNodeFromXML(ByVal v_sOI As String)
            'If Current.Session(CNQuote) IsNot Nothing Then
            'Delete for New Business
            'Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
            'Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oDataSet As New DataSetControl.Application
            Dim hCurrentNodeColl As New Hashtable()
            Dim srDataset As New System.IO.StringReader(DirectCast(Current.Session(CNDataSet), System.Object))
            'Dim srDataset As New System.IO.StringReader(Current.Session(CNDataSet))
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Dim ParentNode As XmlNode = Nothing
            'Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)

            Doc.Load(xmlTR)
            xmlTR.Close()

            Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
            srDataset.Dispose()

            If oNode IsNot Nothing Then
                ParentNode = oNode.ParentNode
                If Current.Session(CNNode) Is Nothing Then
                    oNode.Attributes("US").Value = "3"
                    Using swContent As New System.IO.StringWriter
                        Using xmlwContent As New XmlTextWriter(swContent)
                            Doc.WriteTo(xmlwContent)
                            Current.Session(CNDataSet) = swContent.ToString()
                        End Using
                    End Using
                Else
                    hCurrentNodeColl = CType(Current.Session(CNNode), Hashtable)
                    If hCurrentNodeColl IsNot Nothing AndAlso hCurrentNodeColl.ContainsKey(v_sOI) = True Then
                        'if current OI exist in hash table collection
                        For Each hData As DictionaryEntry In hCurrentNodeColl
                            If hData.Key = v_sOI Then
                                Dim oOldXML As New XmlDocument
                                oOldXML.LoadXml(hData.Value)
                                ParentNode.RemoveChild(oNode)
                                ParentNode.AppendChild(Doc.ImportNode(oOldXML.ChildNodes(0), True))

                                Exit For
                            End If
                        Next

                        'Updating the XMLDATASET
                        Dim swContent As New System.IO.StringWriter
                        Dim xmlwContent As New XmlTextWriter(swContent)

                        Doc.WriteTo(xmlwContent)
                        Current.Session(CNDataSet) = swContent.ToString()

                        xmlwContent.Close()
                        swContent.Close()

                        'Clean the Used OI
                        hCurrentNodeColl.Remove(v_sOI)

                        Current.Session(CNNode) = hCurrentNodeColl
                    Else
                        oNode.Attributes("US").Value = "3"

                        Using swContent As New System.IO.StringWriter
                            Using xmlwContent As New XmlTextWriter(swContent)
                                Doc.WriteTo(xmlwContent)
                                Current.Session(CNDataSet) = swContent.ToString()
                            End Using
                        End Using
                    End If
                End If
            End If
        End Sub
        Sub ReInsertDeletedRecords(ByVal v_sOI As String)
            Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oDataSet As New DataSetControl.Application
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Dim sDeletedChildOI As String = Nothing

            Doc.Load(xmlTR)
            xmlTR.Close()

            Dim oNode As XmlNode = Doc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
            srDataset.Dispose()

            Dim hDeletedNodeColl As New Hashtable()
            If Current.Session(CNDeletedNode) IsNot Nothing Then
                hDeletedNodeColl = CType(Current.Session(CNDeletedNode), Hashtable)
            End If

            If oNode IsNot Nothing And (hDeletedNodeColl IsNot Nothing AndAlso hDeletedNodeColl.Count > 0) Then
                'if AutoSave is True, then data is deleted from DB so we need to insert the data again
                'so i have to make all the deleted node US="1"
                If oNode.HasChildNodes = True Then
                    For iCount As Integer = 0 To oNode.ChildNodes.Count - 1
                        'Search the records whether it is deleted or not then only insert the data
                        If oNode.ChildNodes(iCount).Attributes("OI") IsNot Nothing Then
                            If hDeletedNodeColl.ContainsKey(oNode.ChildNodes(iCount).Attributes("OI").Value) = True Then
                                If oNode.ChildNodes(iCount).HasChildNodes Then
                                    For i2L As Integer = 0 To oNode.ChildNodes(iCount).ChildNodes.Count - 1
                                        If oNode.ChildNodes(iCount).ChildNodes(i2L).HasChildNodes Then
                                            For i3L As Integer = 0 To oNode.ChildNodes(iCount).ChildNodes(i2L).ChildNodes.Count - 1
                                                If oNode.ChildNodes(iCount).ChildNodes(i2L).ChildNodes(i3L).HasChildNodes Then
                                                    For i4L As Integer = 0 To oNode.ChildNodes(iCount).ChildNodes(i2L).ChildNodes(i3L).ChildNodes.Count - 1
                                                        oNode.ChildNodes(iCount).ChildNodes(i2L).ChildNodes(i3L).ChildNodes(i4L).Attributes("US").Value = "1"
                                                    Next
                                                End If
                                                oNode.ChildNodes(iCount).ChildNodes(i2L).ChildNodes(i3L).Attributes("US").Value = "1"
                                            Next
                                        End If
                                        oNode.ChildNodes(iCount).ChildNodes(i2L).Attributes("US").Value = "1"
                                    Next
                                End If
                                oNode.ChildNodes(iCount).Attributes("US").Value = "1"
                                sDeletedChildOI += oNode.ChildNodes(iCount).Attributes("OI").Value + ","
                            End If
                        End If

                    Next
                End If

                'Updating the XMLDATASET
                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)

                Doc.WriteTo(xmlwContent)
                oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                xmlwContent.Close()
                swContent.Close()

                'Clean the Used OI from deleted OI collection
                If sDeletedChildOI IsNot Nothing Then
                    For iDeletedOICount As Integer = 0 To sDeletedChildOI.Split(",").Length - 1
                        If String.IsNullOrEmpty(sDeletedChildOI.Split(",")(iDeletedOICount)) = False Then
                            hDeletedNodeColl.Remove(sDeletedChildOI.Split(",")(iDeletedOICount))
                        End If
                    Next
                End If

                Current.Session(CNDeletedNode) = hDeletedNodeColl

                Current.Session(CNQuote) = oQuote
            End If
        End Sub
        ''' <summary>
        ''' Read all the child elements of an element with a matching element name, this used to
        ''' popualte the ItemGrid control
        ''' </summary>
        ''' <param name="v_sControlName">The parent and child element names, as defined by the ItemGrid control ID</param>
        ''' <param name="r_oXMLDataSource">An XMLDataSource to return the retrieved data</param>
        ''' <param name="v_sOI">The dataset identifier of the parent element from which we are retrieving the child elements</param>
        ''' <returns>Success or Failure of the data retrieval</returns>
        ''' <remarks></remarks>
        Function ReadElementListFromXML(ByVal v_sControlName() As String,
                                        ByRef r_oXMLDataSource As XmlDataSource,
                                        ByVal v_sOI As String, Optional ByVal oFilter As Hashtable = Nothing) As Boolean

            Dim bSuccess As Boolean = False

            If Current.Session(CNDataSet) IsNot Nothing And Current.Session(CNQuote) Is Nothing Then

                Dim xmlTR As New XmlTextReader(New System.IO.StringReader(Current.Session(CNDataSet)))
                Dim oDoc As New XmlDocument

                oDoc.Load(xmlTR)
                xmlTR.Close()

                Dim sXPath As String = String.Empty

                Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
                If oNode IsNot Nothing Then

                    If oNode.Name.Equals(v_sControlName(0)) Then

                        'element name retrieved with the OI matches that passed in, so we are in the right element to read attributes
                        sXPath = ".//" & v_sControlName(0) & "[@OI='" & v_sOI & "']/" & v_sControlName(1)

                    Else

                        'Not matching, so we go back up the tree one level and look for the element amongst the siblings
                        Dim oOI As Collections.Stack = Current.Session.Item(CNOI)
                        v_sOI = oOI.Pop()

                        sXPath = ".//*[@OI='" & oOI.Peek() & "']/" & v_sControlName(0) & "/" & v_sControlName(1)

                        'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                        oOI.Push(v_sOI)
                        Current.Session.Item(CNOI) = oOI

                    End If

                End If

                r_oXMLDataSource.Data = Current.Session(CNDataSet)
                r_oXMLDataSource.XPath = sXPath

                bSuccess = True

            Else
                If Current.Session(CNQuote) IsNot Nothing Then

                    Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                    Dim xmlTR As New XmlTextReader(New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset))
                    Dim oDoc As New XmlDocument

                    oDoc.Load(xmlTR)
                    xmlTR.Close()

                    Dim sXPath As String = String.Empty

                    Dim oNode As XmlNode = oDoc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
                    If oNode IsNot Nothing Then

                        If oNode.Name.Equals(v_sControlName(0)) Then

                            'element name retrieved with the OI matches that passed in, so we are in the right element to read attributes
                            'if FilterByControl property is set and oFilter is not null
                            If oFilter IsNot Nothing AndAlso oFilter.Count > 0 Then
                                'It add the conditing into XPath
                                sXPath = ".//" & v_sControlName(0) & "[@OI='" & v_sOI & "']/" & v_sControlName(1) & "["
                                Dim hData As DictionaryEntry
                                Dim iCount As Integer = 0
                                'loop to retreive the condition from hash table
                                For Each hData In oFilter
                                    If iCount = 0 Then
                                        sXPath &= "@" & hData.Key & "='" & hData.Value & "' "
                                    Else
                                        sXPath &= "and @" & hData.Key & "='" & hData.Value & "' "
                                    End If
                                    iCount += 1
                                Next
                                sXPath &= "]"
                            Else
                                sXPath = ".//" & v_sControlName(0) & "[@OI='" & v_sOI & "']/" & v_sControlName(1)
                            End If


                        Else

                            'Not matching, so we go back up the tree one level and look for the element amongst the siblings
                            Dim oOI As Collections.Stack = Current.Session.Item(CNOI)
                            v_sOI = oOI.Pop()
                            'if FilterByControl property is set and oFilter is not null
                            If oFilter IsNot Nothing AndAlso oFilter.Count > 0 Then
                                sXPath = ".//*[@OI='" & oOI.Peek() & "']/" & v_sControlName(0) & "/" & v_sControlName(1) & "["
                                Dim hData As DictionaryEntry
                                Dim iCount As Integer = 0
                                'loop to retreive the condition from hash table
                                For Each hData In oFilter
                                    If iCount = 0 Then
                                        sXPath &= "@" & hData.Key & "='" & hData.Value & "' "
                                    Else
                                        sXPath &= "and @" & hData.Key & "='" & hData.Value & "' "
                                    End If
                                    iCount += 1
                                Next
                                sXPath &= "]"
                            Else
                                sXPath = ".//*[@OI='" & oOI.Peek() & "']/" & v_sControlName(0) & "/" & v_sControlName(1)
                            End If

                            'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                            oOI.Push(v_sOI)
                            Current.Session.Item(CNOI) = oOI

                        End If

                    End If

                    r_oXMLDataSource.Data = oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset
                    r_oXMLDataSource.XPath = sXPath

                    bSuccess = True
                End If
            End If

            Return bSuccess

        End Function

        ''' <summary>
        ''' Read a single attribute from the risk dataset
        ''' </summary>
        ''' <param name="r_oDoc">The XML document containing the risk dataset</param>
        ''' <param name="v_sControlName">A string array of the control ID, this will contain the parts of the
        ''' control ID representing the element name and attribute anme</param>
        ''' <param name="r_sControlValue"></param>
        ''' <param name="r_sOI">The dataset identifier of the element to read attributes from</param>
        ''' <returns>Success or Failure on the reading of the attribute</returns>
        ''' <remarks></remarks>
        Function ReadAttributeFromXML(ByRef r_oDoc As XmlDocument,
                                    ByVal v_sControlName() As String,
                                    ByRef r_sControlValue As String,
                                    Optional ByRef r_sOI As String = "",
                                    Optional ByVal v_sDataModelCode As String = Nothing) As Boolean

            Try

                r_sControlValue = String.Empty

                Dim oNode As XmlNode

                If String.IsNullOrEmpty(r_sOI) Then

                    'Ok, we'll do it the hard way ... this should only ever occur for the top level
                    'pages, as we dont know any OI's at that point, this assumes top level pages
                    'reference top level elements .. which i think is correct - DH - 16-01-07

                    If v_sDataModelCode Is Nothing Then
                        oNode = r_oDoc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString().ToUpper & "_POLICY_BINDER")
                    Else
                        oNode = r_oDoc.SelectSingleNode("//" & v_sDataModelCode.Trim & "_POLICY_BINDER")
                    End If


                    If oNode IsNot Nothing Then

                        Dim oOI As New Collections.Stack()
                        r_sOI = oNode.Attributes("OI").Value
                        oOI.Push(r_sOI)

                        oNode = oNode.SelectSingleNode("//" & v_sControlName(0))
                        If oNode IsNot Nothing Then
                            r_sOI = oNode.Attributes("OI").Value
                            oOI.Push(r_sOI)
                        End If

                        Current.Session.Item(CNOI) = oOI

                    End If

                End If

                oNode = r_oDoc.SelectSingleNode("//*[@OI='" & r_sOI & "']")

                If oNode IsNot Nothing Then

                    If oNode.Name.Equals(v_sControlName(0)) Then

                        'element name retrieved with the OI matches that passed in, so we are in the right element to read attributes
                        If oNode.Attributes(v_sControlName(1)) IsNot Nothing Then
                            r_sControlValue = oNode.Attributes(v_sControlName(1)).Value
                        Else
                            r_sControlValue = ""
                        End If
                    Else

                        'Not matching, so we go back up the tree one level are look for the element amongst the siblings
                        Dim oOI As Collections.Stack = Current.Session.Item(CNOI)
                        Dim tempoOI As Collections.Stack = Current.Session.Item(CNOI)
                        r_sOI = oOI.Pop()

                        oNode = r_oDoc.SelectSingleNode("//*[@OI='" & oOI.Peek() & "']/" & v_sControlName(0))

                        'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                        oOI.Push(r_sOI)
                        Current.Session.Item(CNOI) = oOI

                        If oNode IsNot Nothing Then
                            If oNode.Attributes(v_sControlName(1)) IsNot Nothing Then
                                r_sControlValue = oNode.Attributes(v_sControlName(1)).Value
                            Else
                                r_sControlValue = ""
                            End If
                        Else

                            oNode = r_oDoc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString().ToUpper & "_POLICY_BINDER")

                            If oNode IsNot Nothing Then
                                oOI = New Collections.Stack
                                r_sOI = oNode.Attributes("OI").Value
                                oOI.Push(r_sOI)
                                If Current.Session.Item(CNCurrentOIItem) IsNot Nothing Then
                                    oNode = oNode.SelectSingleNode("//" & v_sControlName(0) & "[@OI='" & Current.Session.Item(CNCurrentOIItem) & "']")
                                    If oNode Is Nothing Then
                                        oNode = r_oDoc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString().ToUpper & "_POLICY_BINDER")
                                        oNode = oNode.SelectSingleNode("//" & v_sControlName(0))
                                    End If
                                Else
                                    oNode = oNode.SelectSingleNode("//" & v_sControlName(0))
                                End If
                                If oNode IsNot Nothing Then
                                    r_sOI = oNode.Attributes("OI").Value
                                    oOI.Push(r_sOI)
                                Else
                                    oOI = tempoOI
                                End If
                                Current.Session(CNOI) = oOI
                                If oNode IsNot Nothing Then
                                    r_sControlValue = oNode.Attributes(v_sControlName(1)).Value
                                End If

                            End If
                        End If

                    End If

                End If

            Catch ex As Exception
                ReadAttributeFromXML = False
            End Try

            If String.IsNullOrEmpty(r_sControlValue) Then
                ReadAttributeFromXML = False
            Else
                ReadAttributeFromXML = True
            End If

        End Function
        Function WriteClaimAttributeToXML(ByRef r_oDoc As XmlDocument,
                                           ByRef r_oClaim As DataSetControl.Application,
                                           ByVal v_sControlName() As String,
                                           ByVal v_sControlValue As String,
                                           ByVal v_sOI As String) As Object

            Try
                'Almost identical to ReadAttributeToXML, but write
                'instead, so add comments if you change either!

                Dim sElement As String = v_sControlName(0)
                Dim sProperty As String = v_sControlName(1)
                Dim sValue As String = v_sControlValue
                Dim oNode As XmlNode

                If v_sOI Is Nothing Then

                    oNode = r_oDoc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString().ToUpper & "_POLICY_BINDER")

                    If oNode IsNot Nothing Then

                        Dim oOI As New Collections.Stack()
                        v_sOI = oNode.Attributes("OI").Value
                        oOI.Push(v_sOI)

                        oNode = oNode.SelectSingleNode("//" & sElement)
                        If oNode IsNot Nothing Then
                            v_sOI = oNode.Attributes("OI").Value
                            oOI.Push(v_sOI)
                        End If

                        Current.Session.Item(CNOI) = oOI

                    End If


                End If

                oNode = r_oDoc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
                If oNode IsNot Nothing Then

                    If oNode.Name.Equals(sElement) Then

                        'element name re    trieved with the OI matches that passed in, so we are in the right element to write attributes
                        r_oClaim.SetPropertyValue(sElement, sProperty, v_sOI, sValue, True)

                    Else

                        'Not matching, so we go back up the tree one level are look for the element amongst the siblings
                        Dim oOI As Collections.Stack = Current.Session.Item(CNOI)
                        v_sOI = oOI.Pop()

                        oNode = r_oDoc.SelectSingleNode("//*[@OI='" & oOI.Peek() & "']/" & sElement)

                        'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                        oOI.Push(v_sOI)
                        Current.Session.Item(CNOI) = oOI

                        If oNode IsNot Nothing Then
                            r_oClaim.SetPropertyValue(sElement, sProperty, oNode.Attributes("OI").Value, sValue, True)
                        End If

                    End If

                End If

            Catch ex As Exception
                WriteClaimAttributeToXML = False
            End Try

            If v_sControlValue = String.Empty Then
                WriteClaimAttributeToXML = False
            Else
                WriteClaimAttributeToXML = True
            End If

        End Function

        ''' <summary>
        ''' Write data to specified risk dataset attribute
        ''' </summary>
        ''' <param name="r_oDoc">Reference to the current risk dataset in an XML Document obejct</param>
        ''' <param name="r_oDataset">Reference to the current risk dataset</param>
        ''' <param name="v_sControlName">The element contained the attribute to be written</param>
        ''' <param name="v_sControlValue">The value to be written to the dataset</param>
        ''' <param name="v_sOI">The identifier from the risk dataset of the element the attribute is to be written to</param>
        ''' <returns>Returns a success or failure of the attribute being written to the dataset</returns>
        ''' <remarks>The XMl Document is used as read obejct for finding the element to write to and
        ''' validating the element name, identifer and attributes. The data write is performed against the
        ''' dataset pass to the function</remarks>
        Function WriteAttributeToXML(ByRef r_oDoc As XmlDocument,
                                    ByRef r_oDataset As DataSetControl.Application,
                                    ByVal v_sControlName() As String,
                                    ByVal v_sControlValue As String,
                                    ByVal v_sOI As String,
                                    Optional ByVal v_sDataModelCode As String = Nothing) As Boolean

            Try
                'Almost identical to ReadAttributeToXML, but write
                'instead, so add comments if you change either!

                Dim sElement As String = v_sControlName(0)
                Dim sProperty As String = v_sControlName(1)
                Dim sValue As String = v_sControlValue
                Dim oNode As XmlNode

                If v_sOI Is Nothing Then
                    If v_sDataModelCode Is Nothing Then
                        oNode = r_oDoc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString().ToUpper & "_POLICY_BINDER")
                    Else
                        oNode = r_oDoc.SelectSingleNode("//" & v_sDataModelCode.Trim & "_POLICY_BINDER")
                    End If

                    If oNode IsNot Nothing Then

                        Dim oOI As New Collections.Stack()
                        v_sOI = oNode.Attributes("OI").Value
                        oOI.Push(v_sOI)

                        oNode = oNode.SelectSingleNode("//" & sElement)
                        If oNode IsNot Nothing Then
                            v_sOI = oNode.Attributes("OI").Value
                            oOI.Push(v_sOI)
                        End If

                        Current.Session.Item(CNOI) = oOI

                    End If


                End If

                oNode = r_oDoc.SelectSingleNode("//*[@OI='" & v_sOI & "']")
                If oNode IsNot Nothing Then

                    If oNode.Name.Equals(sElement) Then

                        'element name retrieved with the OI matches that passed in, so we are in the right element to write attributes
                        r_oDataset.SetPropertyValue(sElement, sProperty, v_sOI, sValue, True)

                    Else

                        'Not matching, so we go back up the tree one level are look for the element amongst the siblings
                        Dim oOI As Collections.Stack = Current.Session.Item(CNOI)

                        v_sOI = oOI.Pop()


                        oNode = r_oDoc.SelectSingleNode("//*[@OI='" & oOI.Peek() & "']/" & sElement)

                        'DH - Copying a stack from session does it byref, so the previous OI popped needs to be pushed back in
                        oOI.Push(v_sOI)
                        Current.Session.Item(CNOI) = oOI

                        If oNode IsNot Nothing Then
                            r_oDataset.SetPropertyValue(sElement, sProperty, oNode.Attributes("OI").Value, sValue, True)
                        End If

                    End If

                End If
            Catch ex As Exception
                WriteAttributeToXML = False
            End Try

            If v_sControlValue = String.Empty Then
                WriteAttributeToXML = False
            Else
                WriteAttributeToXML = True
            End If

        End Function

        ''' <summary>
        ''' Function to check if the current risk dataset in session is in QuickQuote mode
        ''' or not.
        ''' </summary>
        ''' <returns>Returns true or false</returns>
        ''' <remarks>This function relies on the product being configured with either
        ''' a QUICK_QUOTE or QUICKQUOTE attribute in the product root element.</remarks>
        Public Function IsDataSetQuickQuote() As Boolean

            Dim bQuickQuote As Boolean = False

            If Current.Session(CNQuote) IsNot Nothing Then

                'Read Dataset into xpath document
                Dim Navigator As XPathNavigator
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                If oQuote.Risks.Count > 0 Then
                    Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    'oQuote = Nothing

                    Dim Doc As XPathDocument = New XPathDocument(New XmlTextReader(srDataset))
                    Navigator = Doc.CreateNavigator()
                    Dim i As XPathNodeIterator
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"),
                                          Config.NexusFrameWork).Portals.Portal(GetPortalID()).Products.Product(oQuote.ProductCode)
                    'Does dataset contain a quickquote attribute ?
                    i = Navigator.Select("/DATA_SET/RISK_OBJECTS/*/*[@QUICK_QUOTE|@QUICKQUOTE]")

                    While i.MoveNext()
                        'If so check its status, if 1 we're doing a quickquote, else we're not
                        If i.Current.GetAttribute("QUICK_QUOTE", String.Empty) = "1" And Not String.IsNullOrEmpty(oProduct.QuickQuoteConfig) Then
                            bQuickQuote = True
                        Else
                            If i.Current.GetAttribute("QUICKQUOTE", String.Empty) = "1" And Not String.IsNullOrEmpty(oProduct.QuickQuoteConfig) Then
                                bQuickQuote = True
                            End If
                        End If
                    End While

                    srDataset.Dispose()

                End If
            End If
            Return bQuickQuote

        End Function
        Function ReadAttributeFromXML(ByRef r_oClaim As DataSetControl.Application,
                                    ByVal v_sControlName() As String,
                                    ByRef r_sControlValue As String,
                                    ByVal v_sOI As String) As Boolean

            Try
                'These values all need to writeable for the "GetPropertyValue"
                'function so cant use the values pass in directly

                Dim sElement As String = v_sControlName(0)
                Dim sProperty As String = v_sControlName(1)
                Dim sValue As String = String.Empty

                If v_sOI Is Nothing Then

                    'Ok, we'll do it the hard way ... this should only ever occur for the top level pages on
                    'claims, as we dont know any OI's at that point, this assumes top level pages reference
                    'top level elements .. which i think is correct - DH - 16-01-07
                    sValue = r_oClaim.Risk.Item(r_oClaim.GISDataModelCode & "_POLICY_BINDER").Item(sElement).Item(sProperty).Value

                Else
                    'We can use the OI to directly access any element, no matter how deep
                    r_oClaim.GetPropertyValue(sElement, sProperty, v_sOI, sValue, True)

                End If

                r_sControlValue = sValue

            Catch ex As Exception
                ReadAttributeFromXML = False
            End Try

            If r_sControlValue = String.Empty Then
                ReadAttributeFromXML = False
            Else
                ReadAttributeFromXML = True
            End If

        End Function
        Public Sub UpdateNexusQuoteStatus()

            If Current.Session(CNQuote) IsNot Nothing Then

                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                'All newly created elements are set to deleted so they are removed if the user selects the back button,
                'so as we're saving we need to set the status back to edit otherwise it will be removed when the rules are run.



                Dim oDoc As New XmlDocument
                Using srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
                    Dim xmlTR As New XmlTextReader(srDataset)

                    oDoc.Load(xmlTR)
                    xmlTR.Close()
                End Using

                Dim oNode As XmlNode = oDoc.SelectSingleNode("/DATA_SET/RISK_OBJECTS/*/*[@NEXUSQS]")
                If oNode IsNot Nothing Then
                    oNode.Attributes("NEXUSQS").Value = "1"
                    oNode.Attributes("US").Value = "2"
                End If

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)
                Dim oRisk As NexusProvider.RiskType = Current.Session(CNRiskType)
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/" & oRisk.Path & "/"
                oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode = GetScreenCode(sProductFolder & oProduct.FullQuoteConfig)
                oDoc.WriteTo(xmlwContent)
                oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                Current.Session(CNQuote) = oQuote
                xmlwContent.Close()
                swContent.Close()

            End If

        End Sub
        Public Function NexusQuoteStatus(ByVal oRisk As NexusProvider.Risk) As Boolean

            Dim bNexusStatus As Boolean = False

            If Current.Session(CNQuote) IsNot Nothing Then

                'Read Dataset into xpath document
                Dim Navigator As XPathNavigator
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                If oRisk.XMLDataset IsNot Nothing Then
                    Dim srDataset As New System.IO.StringReader(oRisk.XMLDataset)
                    Dim oNexusFrameWork As Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                    Dim Doc As XPathDocument = New XPathDocument(New XmlTextReader(srDataset))
                    Navigator = Doc.CreateNavigator()
                    Dim i As XPathNodeIterator

                    'Does dataset contain a quickquote attribute ?
                    i = Navigator.Select("/DATA_SET/RISK_OBJECTS/*/*[@NEXUSQS]")

                    If i IsNot Nothing Then
                        While i.MoveNext()
                            'If so check its status, if 1 we're doing a quickquote, else we're not
                            If i.Current.GetAttribute("NEXUSQS", String.Empty) = "1" Then
                                bNexusStatus = True
                            End If
                        End While
                    End If

                    srDataset.Dispose()
                End If
            End If

            Return bNexusStatus

        End Function

        Public Function IsDataSetNexusQuoteStatus(ByVal iRiskKey As Integer) As Boolean

            Dim bNexusQuoteStatus As Boolean = False

            If Current.Session(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                If oQuote.Risks(iRiskKey).XMLDataset IsNot Nothing AndAlso oQuote.Risks(iRiskKey).XMLDataset <> "" Then
                    Dim srDataset As New System.IO.StringReader(oQuote.Risks(iRiskKey).XMLDataset)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim oDoc As New XmlDocument

                    oDoc.Load(xmlTR)
                    xmlTR.Close()

                    Dim oNode As XmlNode = oDoc.SelectSingleNode("/DATA_SET/RISK_OBJECTS/*/*[@NEXUSQS]")
                    Dim sQuoteStatus As String
                    If oNode IsNot Nothing Then
                        sQuoteStatus = oNode.Attributes("NEXUSQS").Value
                    End If
                    If (oNode IsNot Nothing And sQuoteStatus = 1) Then
                        bNexusQuoteStatus = True
                    Else
                        bNexusQuoteStatus = False
                    End If
                End If
            End If

            Return bNexusQuoteStatus

        End Function
        Sub SaveQuote()
            Dim oWebService As NexusProvider.ProviderBase
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)

            'call SAM method SaveRisk to save the risk to DB
            oWebService.SaveRisk(oQuote, Current.Session(CNCurrentRiskKey), Nothing)
            If CType(Current.Session(CNIsTransferQuoteRequired), Boolean) = True Then
                TransferQuoteToRealParty()
            End If
        End Sub
        ''' <summary>
        ''' To add a new party on the basis of CLIENTDETAILS__ fields captured from risk screens
        ''' </summary>
        ''' <remarks>
        ''' This will use all the information from anonymous client except the some captured from risk screens
        ''' </remarks>
        Sub AddRealParty()

            If Current.Session(CNIsAnonymous) IsNot Nothing Then

                If Current.Session(CNQuote) IsNot Nothing Then

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim sBranchCode As String
                    Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)

                    If String.IsNullOrEmpty(oParty.BranchCode) Then
                        sBranchCode = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).BranchCode
                    Else
                        sBranchCode = oParty.BranchCode
                    End If


                    Try
                        ''''''''''''''''''' 
                        ' Duplicate Check before party creation
                        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                        Dim bDuplicateClientCheck As Boolean = False
                        Dim sPartysScreenCode As String = String.Empty
                        If oPortal.DuplicateClientCheckParameters IsNot Nothing AndAlso oPortal.DuplicateClientCheckParameters.Trim.Length <> 0 AndAlso oPortal.DuplicateClientSearchType IsNot Nothing AndAlso oPortal.DuplicateClientSearchType.Trim.Length <> 0 Then
                            Dim oPartyCollection As NexusProvider.PartyCollection
                            Dim oSearchCriteria As New NexusProvider.PartySearchCriteria
                            Dim oAddressCollection As NexusProvider.AddressCollection = oParty.Addresses
                            Dim oAddress As New NexusProvider.Address
                            Dim oTempAddress As New NexusProvider.Address
                            Dim oContactCollection As NexusProvider.ContactCollection = oParty.Contacts
                            Dim oContact As New NexusProvider.Contact
                            Dim oTempContact As New NexusProvider.Contact
                            Dim sParameters() As String
                            Dim Flag_WildSearch As Boolean = False
                            Dim bLWildSearch As Boolean = False
                            Dim bRWildSearch As Boolean = False
                            Dim iDuplicateClientSearch As Integer
                            Dim sDuplicateClientSearch As String

                            Dim oDisableOptionSettings, oEnableOptionSettings As NexusProvider.OptionTypeSetting
                            oEnableOptionSettings = Nothing
                            'Disable All Wildcard Searches
                            oDisableOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5065)
                            'Enable Wildcard Searches Ending With %
                            oEnableOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5066)

                            'If SAM returns nothing for the option then set "0" -start
                            If oDisableOptionSettings.OptionValue Is Nothing Then
                                oDisableOptionSettings.OptionValue = "0"
                            End If

                            If oEnableOptionSettings.OptionValue Is Nothing Then
                                oEnableOptionSettings.OptionValue = "0"
                            End If

                            If oDisableOptionSettings IsNot Nothing AndAlso oDisableOptionSettings.OptionValue = "0" AndAlso oEnableOptionSettings IsNot Nothing AndAlso oEnableOptionSettings.OptionValue = "0" Then
                                'Allow Wildcard Searches 
                                Flag_WildSearch = True

                                If (oPortal.DuplicateClientSearchType.Trim.Substring(0, 1) = "%" And Flag_WildSearch = True) Then
                                    'Enable Wildcard Searches Starting With %
                                    bLWildSearch = True
                                End If

                                If (oPortal.DuplicateClientSearchType.Trim.Substring(oPortal.DuplicateClientSearchType.Trim.Length() - 1, 1) = "%" And Flag_WildSearch = True) Then
                                    'Enable Wildcard Searches End With %
                                    bRWildSearch = True
                                End If


                            ElseIf oEnableOptionSettings IsNot Nothing AndAlso oEnableOptionSettings.OptionValue = "1" Then
                                'Allow Wildcard Searches 
                                Flag_WildSearch = True
                                'Enable Wildcard Searches Starting With %
                                bLWildSearch = False

                                If (oPortal.DuplicateClientSearchType.Trim.Substring(oPortal.DuplicateClientSearchType.Trim.Length() - 1, 1) = "%" And Flag_WildSearch = True) Then
                                    'Enable Wildcard Searches End With %
                                    bRWildSearch = True
                                End If

                            ElseIf oDisableOptionSettings IsNot Nothing AndAlso oDisableOptionSettings.OptionValue = "1" Then
                                'Allow Wildcard Searches 
                                Flag_WildSearch = False
                                'Enable Wildcard Searches Starting With %
                                bLWildSearch = False
                                'Enable Wildcard Searches End With %
                                bRWildSearch = False
                            End If

                            If oPortal.DuplicateClientSearchType.Trim.Length() > 0 Then
                                'removed All % sign
                                sDuplicateClientSearch = Replace(oPortal.DuplicateClientSearchType.Trim, "%", "")
                                iDuplicateClientSearch = sDuplicateClientSearch.Length
                            End If
                            'End If


                            If oAddressCollection IsNot Nothing Then
                                If oAddressCollection.Count > 0 Then
                                    For iCount As Integer = 0 To oAddressCollection.Count - 1
                                        If oAddressCollection(iCount).AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                                            'oAddress retrive Client CorrespondenceAddress Info 
                                            oAddress = oAddressCollection(iCount)
                                        End If
                                    Next
                                End If
                            End If

                            If oContactCollection IsNot Nothing Then
                                If oContactCollection.Count > 0 Then
                                    For iCount As Integer = 0 To oContactCollection.Count - 1
                                        If oContactCollection(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.Number Or oContactCollection(iCount).ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress Then
                                            'oContact retrive Client Contact Info 
                                            oContact = oContactCollection(iCount)
                                        End If
                                    Next
                                End If
                            End If

                            sParameters = oPortal.DuplicateClientCheckParameters.Split(",")

                            For iCounter As Integer = 0 To sParameters.Length - 1
                                'if sParameters retrive Name option, Attached SearchCriteria with Name
                                If sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "NAME" Or sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "FIRSTNAME" Then
                                    If oParty IsNot Nothing Then
                                        Select Case True
                                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                                With CType(oParty, NexusProvider.CorporateParty)
                                                    oSearchCriteria.Name = StrSearch(.CompanyName, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                                End With
                                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                                With CType(oParty, NexusProvider.PersonalParty)
                                                    oSearchCriteria.Name = StrSearch(.TradingName, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                                End With
                                        End Select
                                    End If
                                    'if sParameters retrive ADDRESSLINE1 option, Attached SearchCriteria with ADDRESSLINE1
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE1" Then
                                    If oAddress IsNot Nothing Then

                                        oTempAddress.Address1 = StrSearch(oAddress.Address1, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive ADDRESSLINE2 option, Attached SearchCriteria with ADDRESSLINE2
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE2" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.Address2 = StrSearch(oAddress.Address2, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive ADDRESSLINE3 option, Attached SearchCriteria with ADDRESSLINE3
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE3" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.Address3 = StrSearch(oAddress.Address3, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive ADDRESSLINE4 option, Attached SearchCriteria with ADDRESSLINE4
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADDRESSLINE4" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.Address4 = StrSearch(oAddress.Address4, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive POSTCODE option, Attached SearchCriteria with POSTCODE
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "POSTCODE" Then
                                    If oAddress IsNot Nothing Then
                                        oTempAddress.PostCode = StrSearch(oAddress.PostCode, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                    End If
                                    'if sParameters retrive TELEPHONENUMBER option, Attached SearchCriteria with TELEPHONENUMBER
                                ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "TELEPHONENUMBER" Then
                                    If oContact IsNot Nothing Then
                                        If oContact IsNot Nothing Then
                                            oSearchCriteria.TelephoneNumber = StrSearch(oContact.Number, bLWildSearch, bRWildSearch, iDuplicateClientSearch)
                                        End If
                                    End If
                                End If
                            Next

                            oSearchCriteria.Address = oTempAddress

                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    With CType(oParty, NexusProvider.CorporateParty)
                                        oSearchCriteria.PartyType = "CC"
                                        oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Corporate)

                                    End With
                                    sPartysScreenCode = AppSettings("CorporateGISSCreenCode")

                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    With CType(oParty, NexusProvider.PersonalParty)
                                        oSearchCriteria.PartyType = "PC"
                                        oSearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Personal)
                                    End With
                                    sPartysScreenCode = AppSettings("PersonalGISSCreenCode")
                            End Select

                            Current.Session.Remove(CNSearchData)
                            oPartyCollection = oWebService.FindParty(oSearchCriteria)


                            If oPartyCollection IsNot Nothing Then
                                If oPartyCollection.Count > 0 Then
                                    ' storing search result in session
                                    Current.Session(CNSearchData) = oPartyCollection
                                    bDuplicateClientCheck = True
                                    Current.Session.Add(CNRiskDuplicateClientCheck, "True")
                                Else
                                    'Add New Party
                                    oWebService = New NexusProvider.ProviderManager().Provider
                                    oWebService.AddParty(oParty, sBranchCode)
                                    'Once party has created make sure the partybuilder object should be initialized.
                                    If Not sPartysScreenCode Is Nothing OrElse Trim(sPartysScreenCode) <> "" Then
                                        oParty.XMLDataset = oWebService.RunDefaultRulesAdd(sPartysScreenCode, oParty.XMLDataset, Nothing, False)
                                    End If


                                    'Set Client ShortName info with latest AddPartyResponse UserName 
                                    Select Case True
                                        Case TypeOf oParty Is NexusProvider.CorporateParty
                                            With CType(oParty, NexusProvider.CorporateParty)
                                                .ClientSharedData.ShortName = .UserName
                                            End With
                                        Case TypeOf oParty Is NexusProvider.PersonalParty
                                            With CType(oParty, NexusProvider.PersonalParty)
                                                .ClientSharedData.ShortName = .UserName
                                            End With
                                    End Select

                                    Current.Session(CNParty) = oParty
                                    Current.Session.Add(CNIsTransferQuoteRequired, "True")
                                End If
                            End If

                        Else

                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    sPartysScreenCode = AppSettings("CorporateGISSCreenCode")
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    sPartysScreenCode = AppSettings("PersonalGISSCreenCode")
                            End Select

                            'Add New Party
                            oWebService.AddParty(oParty, sBranchCode)

                            If Not sPartysScreenCode Is Nothing OrElse Trim(sPartysScreenCode) <> "" Then
                                ''Once party has created make sure the partybuilder object should be initialized.
                                oParty.XMLDataset = oWebService.RunDefaultRulesAdd(sPartysScreenCode, oParty.XMLDataset, Nothing, False)
                            End If


                            'Set Client ShortName info with latest AddPartyResponse UserName 


                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    With CType(oParty, NexusProvider.CorporateParty)
                                        .ClientSharedData.ShortName = .UserName
                                    End With
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    With CType(oParty, NexusProvider.PersonalParty)
                                        .ClientSharedData.ShortName = .UserName
                                    End With
                            End Select

                            Current.Session(CNParty) = oParty
                            Current.Session.Add(CNIsTransferQuoteRequired, "True")
                        End If

                    Finally
                        oWebService = Nothing
                    End Try
                End If
            End If

        End Sub
        ''' <summary>
        ''' To transfer an anonymous quote to a real party
        ''' </summary>
        ''' <remarks>
        ''' Anonymous quote is attached with a dummy client(set as anonymous client id in web.config). 
        ''' This quote will be transfer to a real party that is added using information retrieved from Risk screens
        ''' </remarks>
        Sub TransferQuoteToRealParty()

            If Current.Session(CNIsAnonymous) IsNot Nothing Then

                If Current.Session(CNQuote) IsNot Nothing Then

                    Dim oWebService As NexusProvider.ProviderBase
                    Dim sBranchCode As String
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                    Dim sPartysScreenCode As String = String.Empty

                    If String.IsNullOrEmpty(oParty.BranchCode) Then
                        sBranchCode = oNexusConfig.BranchCode
                    Else
                        sBranchCode = oParty.BranchCode
                    End If

                    Dim oAnonymousQuote As NexusProvider.Quote = Current.Session.Item(CNQuote)
                    Dim oQuote As New NexusProvider.Quote(oAnonymousQuote.CoverStartDate, oAnonymousQuote.CoverEndDate, oAnonymousQuote.Description)
                    Try
                        oWebService = New NexusProvider.ProviderManager().Provider

                        'if DuplicateClients Ignore button execute
                        If Current.Session(CNPartyKey) Is Nothing AndAlso (CInt(oPortal.AnnPartyID) = CInt(oParty.Key)) AndAlso CType(Current.Session(CNRiskDuplicateClientCheck), Boolean) = False AndAlso CType(Current.Session(CNIsTransferQuoteRequired), Boolean) = True Then
                            'Add New Party
                            oWebService.AddParty(oParty, sBranchCode)

                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    sPartysScreenCode = AppSettings("CorporateGISSCreenCode")
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    sPartysScreenCode = AppSettings("PersonalGISSCreenCode")
                            End Select

                            If Not sPartysScreenCode Is Nothing AndAlso Trim(sPartysScreenCode) <> "" Then
                                ''Once party has created make sure the partybuilder object should be initialized.
                                oParty.XMLDataset = oWebService.RunDefaultRulesAdd(sPartysScreenCode, oParty.XMLDataset, Nothing, False)
                            End If


                            'Set Client ShortName info using AddPartyResponse UserName 
                            Select Case True
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    With CType(oParty, NexusProvider.CorporateParty)
                                        .ClientSharedData.ShortName = .UserName
                                    End With
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    With CType(oParty, NexusProvider.PersonalParty)
                                        .ClientSharedData.ShortName = .UserName
                                    End With
                            End Select

                            Current.Session(CNParty) = oParty
                        End If


                        'Transfer quote from anonymous to real party
                        oWebService.TransferQuoteToRealParty(oAnonymousQuote.PartyKey, oParty.Key, oAnonymousQuote.InsuranceFileKey, sBranchCode)

                        'Create latest quote object by calling below 3 SAM methods
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oAnonymousQuote.InsuranceFileKey)

                        For iCount As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
                        Next

                        oWebService.GetHeaderAndRisksByKey(oQuote)

                        For iCount As Integer = 0 To oQuote.Risks.Count - 1
                            If oQuote.Risks(iCount).IsRisk = True Then
                                oQuote.Risks(iCount).IsRisk = True
                            Else
                                oQuote.Risks(iCount).IsRisk = False
                            End If
                        Next
                        'Current.Session.Remove(CNOI)

                        'if screencode is not retrived from above sam calls then set it by nexus function

                        'Start getting screen code
                        Dim oNexusFrameWork As Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                        Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
                        Dim sScreenCode As String = Nothing
                        Dim oRiskType As Config.RiskType

                        If oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                        Else
                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode.Trim)
                        End If

                        If oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode IsNot Nothing Then
                            If oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode.Trim.Length <> 0 Then
                                sScreenCode = oQuote.Risks(Current.Session(CNCurrentRiskKey)).ScreenCode
                            Else
                                sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                            End If
                        Else
                            sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                        End If

                        oQuote.Risks(0).ScreenCode = sScreenCode
                        '-End getting screen code

                        'Update session CNQuote with new quote object(retrieved after transferring to real party)
                        Current.Session(CNQuote) = oQuote
                        'Remove all anonymous quote sessions as quote has been transferred to real party
                        Current.Session.Remove(CNIsAnonymous)
                        Current.Session.Remove(CNAnonymous)
                        Current.Session.Remove(CNIsTransferQuoteRequired)
                        Current.Session.Remove(CNOI)

                    Finally
                        oWebService = Nothing
                    End Try
                End If
            End If

        End Sub
        ''' <summary>
        ''' To update the existing party with new clientdetail fields recieved from risk screens
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub UpdatePartyDetails()

            Dim oWebService As NexusProvider.ProviderBase
            'Create Party object with changed details 
            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)

            Dim sBranchCode As String
            If String.IsNullOrEmpty(oParty.BranchCode) Then
                sBranchCode = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).BranchCode
            Else
                sBranchCode = oParty.BranchCode
            End If

            'Update existing party with changed details
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                If Not UpdatePartyCall(oParty, sBranchCode) Then
                    Exit Sub
                End If
                Current.Session(CNParty) = oParty
            Finally
                oWebService = Nothing
            End Try

        End Sub
        ''' <summary>
        ''' Return String specify DuplicateClientCheckParameters characters plus wild card  using DuplicateClientSearchType option
        ''' </summary>
        ''' <remarks></remarks>
        Function StrSearch(ByVal sDuplicateControlValues As String, ByVal Left_WildSearch As Boolean, ByVal Right_WildSearch As Boolean, ByVal Config_length As Integer) As String
            Dim sDuplicateControlValue As String
            If Left_WildSearch = True Then
                sDuplicateControlValue = sDuplicateControlValue & "%"
            End If
            If (sDuplicateControlValues.Length > Config_length) Then
                sDuplicateControlValue = sDuplicateControlValue & sDuplicateControlValues.Substring(0, Config_length)
            Else
                sDuplicateControlValue = sDuplicateControlValue & sDuplicateControlValues.Substring(0)
            End If
            If Right_WildSearch = True Then
                sDuplicateControlValue = sDuplicateControlValue & "%"
            End If
            Return sDuplicateControlValue
        End Function

        Public Function IsPremiumDiplayRoute() As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            Dim sSummaryOfRisk As String = String.Empty
            Dim sSummaryOfCover As String = String.Empty

            Dim sDataModelCode As String
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(GetPortalID())
            Dim sProductPath() As String
            Dim oRiskType As NexusProvider.RiskType = Current.Session(CNRiskType)
            sProductPath = CStr(Current.Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)

            Dim oProduct As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Current.Server.UrlDecode(Current.Request.Url.Segments(
                sProductPath.Length + 1).TrimEnd("/")))

            If oProduct Is Nothing Then
                Throw New Exception("Product can NOT be found")

                'Else
                '    '1.3
                '    If CType(Current.Session(CNLoginType), LoginType) = LoginType.Agent Then
                '        Dim sAllowedAgent() As String
                '        Dim bMatched As Boolean = False
                '        Dim oUserDetails As NexusProvider.UserDetails = Current.Session.Item(CNAgentDetails)
                '        Dim iCounter As Integer = 0
                '        sAllowedAgent = oPortal.Products.GetAllowedAgentByName(Current.Server.UrlDecode( _
                '                                    Current.Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/"))).Split(",")

                '        If sAllowedAgent.Length = 1 And String.IsNullOrEmpty(sAllowedAgent(0)) Then
                '            'no agents specified, so any allowed
                '            bMatched = True
                '        Else
                '            'loop through array of allowed agents. If any match current user then set flag
                '            For iCounter = 0 To sAllowedAgent.Length - 1
                '                If sAllowedAgent(iCounter).ToUpper() = oUserDetails.PartyName.ToUpper() Then
                '                    bMatched = True
                '                    Exit For
                '                End If
                '            Next
                '        End If

                '        If bMatched = False Then
                '            Throw New Exception("Access denied to product due to configuration")
                '        End If
                '    End If

                'Need to select the datamodelcode of the selected risk
                If oRiskType IsNot Nothing Then
                    sDataModelCode = oRiskType.DataModelCode
                End If
            End If
            Dim sFirstPage As String = String.Empty
            If Not String.Equals(sDataModelCode, Current.Session(CNDataModelCode)) Then

                Current.Session(CNDataModelCode) = sDataModelCode.ToUpper

                'retrieved DataModelCode does not match that in session so we've changed
                'product, so check we are on the first page as a new quote will be created

                Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(Current.Server.UrlDecode(
                    Current.Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
                'oPortal.Products.Product(CType(Session(CNQuote), NexusProvider.Quote).ProductCode)

                Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name
                'This code will solve the purpose in case of Anonymous Agent when client is not selected
                'Session(CnriskType) is nothing so have to repopulate this here PN 60503
                If Current.Session(CNParty) Is Nothing And oRiskType Is Nothing Then
                    Dim oRisk As Config.RiskType = oProductConfig.RiskTypes.RiskType(0)
                    Dim oRiskAnonymousType As New NexusProvider.RiskType
                    oRiskAnonymousType.DataModelCode = oRisk.DataModelCode
                    oRiskAnonymousType.Name = oRisk.Name
                    oRiskAnonymousType.Path = oRisk.Path
                    oRiskAnonymousType.RiskCode = oRisk.RiskCode
                    Current.Session(CNRiskType) = oRiskAnonymousType
                    oRiskType = Current.Session(CNRiskType)
                End If
                Dim sXMLPath As String = Current.Server.MapPath(sFolder & "\" & oRiskType.Path & "\")

                If oProductConfig.QuickQuoteConfig = String.Empty Then
                    'No quickquote for product, so FullQuote
                    Current.Session.Item(CNQuoteMode) = QuoteMode.FullQuote
                Else
                    If Current.Session.Item(CNQuoteMode) Is Nothing Then
                        'No QuoteMode in session and quickquote product available, so QuickQuote
                        Current.Session.Item(CNQuoteMode) = QuoteMode.QuickQuote
                    Else
                        'Don't override QuoteMode as QQ and FQ are available for the product
                    End If
                End If

                Select Case CType(Current.Session.Item(CNQuoteMode), QuoteMode)
                    Case QuoteMode.QuickQuote
                        sXMLPath = sXMLPath & oProductConfig.QuickQuoteConfig
                    Case QuoteMode.FullQuote
                        sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                End Select

                Dim xmlds As New XmlDataSource
                xmlds.DataFile = sXMLPath
                xmlds.EnableCaching = False

                Dim Navigator As XPathNavigator
                Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                Navigator = Doc.CreateNavigator()
                Dim i As XPathNodeIterator
                Dim sMainDetail As String = String.Empty

                i = Navigator.Select("/screens/screen/tab[1]")



                While (i.MoveNext)
                    sFirstPage = i.Current.GetAttribute("url", String.Empty)
                    sMainDetail = i.Current.GetAttribute("maindetails", String.Empty)
                    sSummaryOfRisk = i.Current.GetAttribute("summaryofrisk", String.Empty)
                    sSummaryOfCover = i.Current.GetAttribute("summaryofcover", String.Empty)
                End While
                If sSummaryOfCover.ToLower = "true" Then
                    i = Navigator.Select("/screens/screen/tab[2]")
                    While (i.MoveNext)
                        sFirstPage = i.Current.GetAttribute("url", String.Empty)
                    End While
                Else
                    i = Navigator.Select("/screens/screen/tab[1]")
                End If
                If Not IsCurrentPage(sFirstPage) Then

                    'We're not on the first risk screen and we really should be

                    'stops all the processing being down again on then correct
                    'risks screen, as we've already collected all the info we need
                    'Current.Session(CNQuoteInSync) = True

                    'Current.Response.Redirect(sFolder & "/" & sFirstPage)


                End If

            Else
                sFirstPage = String.Empty
            End If
            Return sFirstPage

        End Function

        ''' <summary>
        ''' Read a single attribute from the risk dataset
        ''' </summary>
        ''' <param name="r_oDoc">The XML document containing the risk dataset</param>
        ''' <param name="v_sControlName">A string array of the control ID, this will contain the parts of the
        ''' control ID representing the element name and attribute anme</param>        
        ''' <returns>Success or Failure on the reading of the attribute</returns>
        ''' <remarks></remarks>
        Function ReteriveOIFromXML(ByRef r_oDoc As XmlDocument,
                                    ByVal v_sControlName() As String,
                                    Optional ByVal v_sDataModelCode As String = Nothing) As Boolean

            Dim oNode As XmlNode
            Dim sOI As String

            If v_sDataModelCode Is Nothing Then
                oNode = r_oDoc.SelectSingleNode("//" & Current.Session.Item(CNDataModelCode).ToString().ToUpper & "_POLICY_BINDER")
            Else
                oNode = r_oDoc.SelectSingleNode("//" & v_sDataModelCode.Trim & "_POLICY_BINDER")
            End If

            If oNode IsNot Nothing Then

                Dim oOI As Collections.Stack
                oOI = Current.Session.Item(CNOI)
                If oOI IsNot Nothing AndAlso oOI.Count > 2 Then
                    oNode = oNode.SelectSingleNode("//" & v_sControlName(0) & "[@OI='" & oOI.Peek().ToString & "']")
                Else
                    oNode = oNode.SelectSingleNode("//" & v_sControlName(0))
                End If
                If oNode IsNot Nothing Then
                    sOI = oNode.Attributes("OI").Value
                    If oOI IsNot Nothing And oOI.Count > 0 Then
                        If oOI.Peek().ToString <> sOI Then
                            oOI.Push(sOI)
                        End If
                    Else
                        oOI.Push(sOI)
                    End If
                End If

                Current.Session.Item(CNOI) = oOI

            End If


        End Function

        Public Sub GetScreens()
            Dim oRisk As Config.RiskType
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            Dim oRiskType As NexusProvider.RiskType = Current.Session(CNRiskType)
            Dim oNexusFramework As Config.NexusFrameWork
            Dim oProduct As Nexus.Library.Config.Product
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oProduct = oNexusFramework.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            If oProduct Is Nothing Then


                Throw New Exception("Product can NOT be found")
            End If

            Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.GetProductByName(oProduct.Name)
            'retrieved DataModelCode does not match that in session so we've changed
            'product, so check we are on the first page as a new quote will be created





            Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & oProductConfig.Name
            'This code will solve the purpose in case of Anonymous Agent when client is not selected
            'Session(CnriskType) is nothing so have to repopulate this here PN 60503
            If Current.Session(CNParty) Is Nothing And oRiskType Is Nothing Then
                oRisk = oProductConfig.RiskTypes.RiskType(0)
                Dim oRiskAnonymousType As New NexusProvider.RiskType
                oRiskAnonymousType.DataModelCode = oRisk.DataModelCode
                oRiskAnonymousType.Name = oRisk.Name
                oRiskAnonymousType.Path = oRisk.Path
                oRiskAnonymousType.RiskCode = oRisk.RiskCode
                Current.Session(CNRiskType) = oRiskAnonymousType
                oRiskType = Current.Session(CNRiskType)
            Else
                oRisk = oProductConfig.RiskTypes.RiskType(0)
            End If



            If Current.Session.Item(CNQuote) IsNot Nothing Then

                oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If Current.Session(CNRiskType) Is Nothing Then

                        If Current.Session(CNCurrentRiskKey) > oQuote.Risks.Count - 1 OrElse oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                            oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskTypeCode.Trim)
                        Else
                            oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskCode.Trim)
                        End If

                        Dim oRiskAnonymousType As New NexusProvider.RiskType
                        oRiskAnonymousType.DataModelCode = oRisk.DataModelCode
                        oRiskAnonymousType.Name = oRisk.Name
                        oRiskAnonymousType.Path = oRisk.Path
                        oRiskAnonymousType.RiskCode = oRisk.RiskCode
                        Current.Session(CNRiskType) = oRiskAnonymousType
                        oRiskType = Current.Session(CNRiskType)
                    End If
                    Exit For
                Next
            End If

            If oRiskType IsNot Nothing Then
                Dim sXMLPath As String = Current.Server.MapPath(sFolder & "\" & oRiskType.Path & "\")
                Dim sProductFolder As String = "~/" & oNexusFramework.ProductsFolder & "/" & oProduct.Name & "/" & oRisk.Path & "/"
                If oProductConfig.QuickQuoteConfig = String.Empty Then
                    'No quickquote for product, so FullQuote
                    Current.Session.Item(CNQuoteMode) = QuoteMode.FullQuote
                Else
                    If Current.Session.Item(CNQuoteMode) Is Nothing Then
                        'No QuoteMode in session and quickquote product available, so QuickQuote
                        Current.Session.Item(CNQuoteMode) = QuoteMode.QuickQuote
                    Else
                        'Don't override QuoteMode as QQ and FQ are available for the product
                    End If
                End If

                Select Case CType(Current.Session.Item(CNQuoteMode), QuoteMode)
                    Case QuoteMode.QuickQuote
                        sXMLPath = sXMLPath & oProductConfig.QuickQuoteConfig
                    Case QuoteMode.FullQuote, QuoteMode.MTAQuote
                        sXMLPath = sXMLPath & oProductConfig.FullQuoteConfig
                End Select

                Dim xmlds As New XmlDataSource
                xmlds.DataFile = sXMLPath
                xmlds.EnableCaching = False

                Dim Navigator As XPathNavigator
                Dim Doc As XPathDocument = New XPathDocument(sXMLPath)
                Navigator = Doc.CreateNavigator()

                'check summary of risk and cover configure as risk screens
                Dim iSrc As XPathNodeIterator
                Dim iTab As XPathNodeIterator
                iSrc = Navigator.Select("/screens/screen")
                While (iSrc.MoveNext)
                    iTab = Navigator.Select("/screens/screen/tab")
                    While (iTab.MoveNext)
                        sSummaryOfRisk = iTab.Current.GetAttribute("summaryofrisk", String.Empty)
                        If sSummaryOfRisk.ToLower = "true" Then
                            sSummaryOfRiskURL = sProductFolder & iTab.Current.GetAttribute("url", String.Empty)
                        End If
                        If sSummaryOfRisk.ToLower = "true" Then
                            Exit While
                        End If
                    End While
                    If sSummaryOfRisk.ToLower = "true" Then
                        Exit While
                    End If
                End While

                iSrc = Navigator.Select("/screens/screen")
                While (iSrc.MoveNext)
                    iTab = Navigator.Select("/screens/screen/tab")
                    While (iTab.MoveNext)
                        sSummaryOfCover = iTab.Current.GetAttribute("summaryofcover", String.Empty)
                        If sSummaryOfCover.ToLower = "true" Then

                            sSummaryOfCoverURL = sProductFolder & iTab.Current.GetAttribute("url", String.Empty)
                        End If
                        If sSummaryOfCover.ToLower = "true" Then
                            Exit While
                        End If
                    End While
                    If sSummaryOfCover.ToLower = "true" Then
                        Exit While
                    End If
                End While

                iSrc = Navigator.Select("/screens/screen")
                While (iSrc.MoveNext)
                    iTab = Navigator.Select("/screens/screen/tab")
                    While (iTab.MoveNext)
                        sReferScreen = iTab.Current.GetAttribute("referredscreen", String.Empty)
                        If sReferScreen.ToLower = "true" Then
                            sReferScreenURL = sProductFolder & iTab.Current.GetAttribute("url", String.Empty)
                        End If
                        If sReferScreen.ToLower = "true" Then
                            Exit While
                        End If
                    End While
                    If sReferScreen.ToLower = "true" Then
                        Exit While
                    End If
                End While

                iSrc = Navigator.Select("/screens/screen")
                While (iSrc.MoveNext)
                    iTab = Navigator.Select("/screens/screen/tab")
                    While (iTab.MoveNext)
                        sDeclineScreen = iTab.Current.GetAttribute("declinedscreen", String.Empty)
                        If sDeclineScreen.ToLower = "true" Then
                            sDeclineScreenURL = sProductFolder & iTab.Current.GetAttribute("url", String.Empty)
                        End If
                        If sDeclineScreen.ToLower = "true" Then
                            Exit While
                        End If
                    End While
                    If sDeclineScreen.ToLower = "true" Then
                        Exit While
                    End If
                End While
            End If
        End Sub

        Public Sub UpdateXML(ByVal sNodeQuery As String, ByVal sObjectName As String, ByVal sFieldName As String, ByVal sFieldValue As String, Optional ByVal bIsDelete As Boolean = False)
            Dim oDataSet As New DataSetControl.Application
            'get the dataset definition
            Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)

            Dim sDataSetDefinition As String = Nexus.DataSetFunctions.GetDataSetDefinition(Current.Session(CNDataModelCode))
            'load dataset into SAM client
            oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

            Dim srXMLDataset As String = oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset
            Dim oDoc As New XmlDocument
            oDoc.LoadXml(srXMLDataset)

            Dim oNode As XmlNode = oDoc.SelectSingleNode(sNodeQuery)

            Dim swContent As New System.IO.StringWriter
            Dim xmlwContent As New XmlTextWriter(swContent)
            oDataSet.SetPropertyValue(sObjectName, sFieldName, oNode.Attributes("OI").Value, sFieldValue, True)
            If bIsDelete = False Then
                oDataSet.SetPropertyValue(sObjectName, "US", oNode.Attributes("OI").Value, "2", True)
            End If
            oDoc.WriteTo(xmlwContent)
            oQuote.Risks(System.Web.HttpContext.Current.Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()
            oDataSet.ReturnAsXML(oQuote.Risks(System.Web.HttpContext.Current.Session(CNCurrentRiskKey)).XMLDataset)
            Current.Session(CNQuote) = oQuote
            xmlwContent.Close()
            swContent.Close()
        End Sub

        Public Sub CalculatePremiumAndTax(Optional ByRef dPremium As Double = 0, Optional ByRef dTax As Double = 0,
            Optional ByRef dTaxRate As Double = 0, Optional ByRef dTotalPremium As Double = 0, Optional ByRef dSumInsured As Double = 0, Optional ByRef dFee As Double = 0)

            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                oWebService.GetHeaderAndRisksByKey(oQuote)

                dPremium = oQuote.NetTotal
                dSumInsured = oQuote.TotalSumInsured
                dTotalPremium = oQuote.GrossTotal
                dTax = oQuote.TaxTotal
                dTaxRate = oQuote.TaxTotalRate
                dFee = oQuote.FeeTotal
            Finally
                'oQuote = Nothing
            End Try
            Current.Session(CNQuote) = oQuote

        End Sub


        Public Sub FillContactedDropDown(ByVal iAgentKey As Integer, ByVal POLICYHEADER__CONTACT_NAME As DropDownList)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oUserCollection As New NexusProvider.UserCollection
            Dim oUserCollectionWithCorrectedUserName As NexusProvider.UserCollection

            GetAgentSettingsCall(oAgentSettings, iAgentKey)
            If (oAgentSettings IsNot Nothing AndAlso oAgentSettings.AssociatedUsers IsNot Nothing) Then
                oUserCollection = oAgentSettings.AssociatedUsers
            End If

            If (oUserCollection.Count > 0) Then
                oUserCollectionWithCorrectedUserName = New NexusProvider.UserCollection
                Dim oCorrectedUser As NexusProvider.User
                For Each oUser As NexusProvider.User In oUserCollection
                    oCorrectedUser = New NexusProvider.User
                    oCorrectedUser = oUser
                    oCorrectedUser.FullName = IIf(oUser.FullName.ToString = "", oUser.UserName.ToString(), oUser.FullName.ToString)
                    oUserCollectionWithCorrectedUserName.Add(oCorrectedUser)
                Next
                oUserCollectionWithCorrectedUserName.SortColumn = "FullName"
                oUserCollectionWithCorrectedUserName.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                oUserCollectionWithCorrectedUserName.Sort()
                POLICYHEADER__CONTACT_NAME.DataValueField = "UserKey"
                POLICYHEADER__CONTACT_NAME.DataTextField = "FullName"
                POLICYHEADER__CONTACT_NAME.DataSource = oUserCollectionWithCorrectedUserName
                POLICYHEADER__CONTACT_NAME.DataBind()
                POLICYHEADER__CONTACT_NAME.Enabled = True
                'POLICYHEADER__CONTACT_NAME.Items.Clear()

                'For i As Integer = 0 To oUserCollection.Count - 1
                '    Dim lstUsers As New ListItem
                '    lstUsers.Text = IIf(oUserCollection.Item(i).FullName.ToString = "", oUserCollection.Item(i).UserName.ToString(), oUserCollection.Item(i).FullName.ToString)
                '    lstUsers.Value = Trim(oUserCollection.Item(i).UserKey.ToString)
                '    POLICYHEADER__CONTACT_NAME.Items.Add(lstUsers)
                'Next
                'POLICYHEADER__CONTACT_NAME.DataBind()
                'POLICYHEADER__CONTACT_NAME.Enabled = True
                'SortDDL(POLICYHEADER__CONTACT_NAME)
            Else
                POLICYHEADER__CONTACT_NAME.Items.Clear()
                POLICYHEADER__CONTACT_NAME.Enabled = False
            End If

            POLICYHEADER__CONTACT_NAME.Items.Insert(0, New ListItem("Select", ""))
        End Sub
        ''' <summary>
        ''' This method retreive the report and returns the Url to open the report
        ''' </summary>
        ''' <param name="sReportName"></param>
        ''' <param name="oParametersCollection"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReportUrl(ByVal sReportName As String, ByVal oParametersCollection As NexusProvider.ParametersCollection) As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sDocumentExtractionDirectory As String = Nothing
            Dim sUniqueDirectory As String = Guid.NewGuid.ToString
            Dim url As String = String.Empty
            Dim sFileName As String = String.Empty

            Dim sReportDirName As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                  .Portals.Portal(CMS.Library.Portal.GetPortalID()).Reports.Location

            ''set the extraction directory using a guid to ensure it is unique
            'sDocumentExtractionDirectory = sReportDirName & "/" & sUniqueDirectory

            If HttpContext.Current.Session.IsCookieless Then
                url = AppSettings("webroot") & "(S(" & Current.Session.SessionID.ToString() + "))" & "/secure/reportviewer.aspx?reportname=" & HttpUtility.UrlEncode(sReportName)
            Else
                url = AppSettings("webroot") & "secure/reportviewer.aspx?reportname=" & HttpUtility.UrlEncode(sReportName)
            End If
            Return url
        End Function

        ''' <summary>
        ''' SetControlValueToObject : Writes Address Details to Party Address Object.
        ''' </summary>
        ''' <param name="oControl"></param>
        ''' <param name="sControlName"></param>
        ''' <returns>Boolean : Returns true if Session Party Object is updated</returns>
        ''' <remarks></remarks>
        Private Function SetControlValueToObject(ByVal oControl As Object, ByVal sControlName As String) As Boolean
            Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
            Dim bReturnValue As Boolean = True
            Select Case sControlName
                Case "ADDRESS_LINE5"
                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address5 = CType(oControl, TextBox).Text
                Case "ADDRESS_LINE6"
                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address6 = CType(oControl, TextBox).Text
                Case "ADDRESS_LINE7"
                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address7 = CType(oControl, TextBox).Text
                Case "ADDRESS_LINE8"
                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address8 = CType(oControl, TextBox).Text
                Case "ADDRESS_LINE9"
                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address9 = CType(oControl, TextBox).Text
                Case "ADDRESS_LINE10"
                    oParty.Addresses(NexusProvider.AddressType.CorrespondenceAddress).Address10 = CType(oControl, TextBox).Text
                Case Else
                    bReturnValue = False
            End Select
            Current.Session(CNParty) = oParty
            oParty = Nothing
            Return bReturnValue
        End Function


        Public Function SendEmail(ByVal pSender As String,
                              ByVal pRecipient As String,
                              ByVal pSubject As String,
                              ByVal pMessage As String,
                              Optional ByVal pEmailDetails As System.Collections.Hashtable = Nothing,
                              Optional ByVal pTemplateFile As String = Nothing,
                              Optional ByVal pWebRoot As String = Nothing,
                              Optional ByVal pAttachment As String = Nothing,
                              Optional ByVal pCC As String = Nothing,
                              Optional ByVal pBCC As String = Nothing) As Boolean


            pTemplateFile = GenericEmailFormat(pTemplateFile, pEmailDetails, pWebRoot)
            Dim xlJob As XElement =
           <BACKGROUND_JOB>
               <JOB jobtype="DOCUPACK">
                   <PARAMETERS>
                       <PARAMETER name="emailTo" value=<%= pRecipient %>/>
                       <PARAMETER name="emailCc" value=<%= pCC %>/>
                       <PARAMETER name="emailSubject" value=<%= pSubject %>/>
                       <PARAMETER name="Destination" value="email"/>
                       <PARAMETER name="path" value=<%= pTemplateFile %>/>
                       <PARAMETER name="archive" value="false"/>
                       <PARAMETER name="type" value="report"/>
                       <PARAMETER name="DocumentRef" value=<%= pAttachment %>/>
                   </PARAMETERS>
                   <ITEMS>
                   </ITEMS>
               </JOB>
           </BACKGROUND_JOB>


            Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
            Dim sDescription As String = "Email Error"
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'call SAM to queue the docs for Archiving

            Try
                Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
                Return False 'Success
            Catch ex As Exception
                Return True 'Failed
            End Try

        End Function
        Private Function GenericEmailFormat(ByVal pTemplateFile As String,
                                        ByVal pEmailDetails As System.Collections.Hashtable, ByVal pWebRoot As String) As String

            'DH - 01/09/05
            'Create a string for the email content using the passed
            'template to have the keywords replace with passed values

            'Open Template
            Dim srTmp As New StreamReader(File.OpenRead(Current.Server.MapPath(pTemplateFile)))

            Dim sbTemplate As New StringBuilder(srTmp.ReadToEnd())
            srTmp.Close()

            sbTemplate.Replace("[!WEBROOT!]", pWebRoot)

            Dim Email As New StringBuilder

            With Email
                For Each sKey As String In pEmailDetails.Keys
                    sbTemplate.Replace(sKey, pEmailDetails(sKey))
                Next
            End With
            Dim sFile As String = Current.Server.MapPath(pTemplateFile)
            Dim workingDirectory As String = Left(sFile, InStrRev(sFile, "\"))
            Return SaveHtmlToEmailBodyFile(sbTemplate.ToString, workingDirectory)

        End Function
        Function SaveHtmlToEmailBodyFile(content As String, workingDirectory As String) As String
            Dim sguid As String = Guid.NewGuid().ToString("N")
            Dim filename As String = String.Concat(workingDirectory, "email_" + sguid + ".html")
            File.WriteAllText(filename, content)
            Return filename
        End Function
    End Module

End Namespace
