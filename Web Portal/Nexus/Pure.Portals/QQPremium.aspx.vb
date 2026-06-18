Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Utils
Imports SiriusFS.SAM.Client
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    ''' <summary>
    ''' Display the Quick Quote premium
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class QQPremium : Inherits Frontend.clsCMSPage

        Private oNexusFramework As Config.NexusFrameWork
        Private oProduct As Config.Product

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            'Load the QQSummaryCoverCntlr control on page init otherwise view state for Document manager will not get maintained
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim WebControlPath As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            WebControlPath = "~/Products/" & oProductConfig.Name & "/QQSummaryCoverCntlr.ascx"
            If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                pnlSummary.Controls.Clear()
                Dim tempControl As Control = LoadControl(WebControlPath)
                pnlSummary.Controls.Add(tempControl)
            End If

        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            SetPageProgress(1)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oProduct = oNexusFramework.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)

            If Not IsPostBack Then
                If Session(CNQuote) IsNot Nothing Then

                    If oQuote.Risks(0).PremiumDueGross > 0 Then

                        If Session.Item(CNMode) Is Nothing _
                            Or (Session.Item(CNMode) = Mode.Edit Or Session.Item(CNMode) = Mode.Buy And Session.Item(CNQuoteMode) = QuoteMode.QuickQuote) Then

                        Else
                            ProcessQuote()
                        End If

                        DisplaySummaryOfCover()

                    Else

                        Session(CNQuoteMode) = QuoteMode.QuickQuote
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        'if Risk Type is not populated then it will be defaulted to the first risk
                        If Session(CNRiskType) Is Nothing Then
                            Dim oRiskType As Config.RiskType
                            If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
                            Else
                                oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode.Trim)
                            End If

                            Dim oRisk As New NexusProvider.RiskType
                            oRisk.DataModelCode = oRiskType.DataModelCode
                            oRisk.Name = oRiskType.Name
                            oRisk.Path = oRiskType.Path
                            oRisk.RiskCode = oRiskType.RiskCode
                            Session(CNRiskType) = oRisk
                        End If
                        Response.Redirect("~/" & oNexusFramework.ProductsFolder & "/" & oProduct.Name & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" _
                            & GetFirstRiskScreen("~/" & oNexusFramework.ProductsFolder & "/" & oProduct.Name & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path _
                            & "/" & oProduct.QuickQuoteConfig))

                    End If

                End If

            End If
            Dim WebControlPath As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            WebControlPath = "~/Products/" & oProductConfig.Name & "/QQSummaryCoverCntlr.ascx"
            If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                pnlSummary.Controls.Clear()
                Dim tempControl As Control = LoadControl(WebControlPath)
                pnlSummary.Controls.Add(tempControl)
            End If
        End Sub

        Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click

            Session.Item(CNMode) = Mode.Save
            Call ProcessQuote()

        End Sub

        Protected Sub btnBuyNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuyNow.Click

            Session.Item(CNMode) = Mode.Buy
            Call ProcessQuote()

        End Sub

        Sub DisplaySummaryOfCover()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                If Session(CNLoginType) = LoginType.Customer Then
                    'Don't need to prompt the user to login or register if they already logged in
                    ltSummary.Visible = False
                ElseIf Session(CNLoginType) = LoginType.Agent And Session(CNAnonymous) = CNAnonymousUser Then
                    'check to see whether Agent is logged in and doing anonymous Quote
                    ltSummary.Text = GetLocalResourceObject("lt_Summary_Customer")
                Else
                    ltSummary.Visible = False
                End If

            Else
                ltSummary.Text = GetLocalResourceObject("lt_Summary_Customer")

            End If

            pnlSummary.Visible = True

            Dim oQuote As NexusProvider.Quote = Session.Item(CNQuote)
            oWebService.GetHeaderAndRisksByKey(oQuote)
            lblPremiumIndication.Text = New Money(oQuote.GrossTotal, Session(CNCurrenyCode)).Formatted
            'THIS IS NOT REQUIRED NOW IN CASE OF QUICK QUOTE AS THIS IS TEMP QUOTE
            'If Session(CNLoginType) IsNot Nothing Then
            '    lblPolicyRef.Text = "  (" & oQuote.InsuranceFileRef & ")"
            'End If

            oQuote = Nothing

        End Sub

        Sub ProcessQuote()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusFrameWork.Portals.Portal(Portal.GetPortalID())

            If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent _
                   And Session(CNAnonymous) IsNot Nothing And Session(CNIsAnonymous) = True Then

                'We're an agent without a client selected, so go get one
                Response.Redirect("~/secure/agent/FindClient.aspx", False)

            ElseIf Session.Item(CNLoginType) Is Nothing Then

                'Need a client to continue, so create one if w e're not logged in
                Response.Redirect("~/Register.aspx", False)
                'Check for an Anoymous Quote in Agent Login --AR

            Else

                Select Case CType(Session.Item(CNMode), Mode)
                    Case Mode.Save

                        'Save quote and remove quotemode, as we're no longer in QQ mode
                        SaveQuote()
                        If (Session.Item(CNMode) <> Mode.Save) Then
                            UpdateQuickQuoteStatus()
                        End If

                        Session.Remove(CNQuoteMode)
                        Session.Remove(CNMode)

                        If CType(Session.Item(CNLoginType), LoginType) = LoginType.Agent Then

                            Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                            Select Case True
                                Case TypeOf oParty Is NexusProvider.PersonalParty
                                    Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                                Case TypeOf oParty Is NexusProvider.CorporateParty
                                    Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName, False)
                            End Select

                            oParty = Nothing

                        Else
                            Response.Redirect("~/secure/QuoteRetrieval.aspx", False)
                        End If

                    Case Mode.Buy

                        If CType(Session.Item(CNQuoteMode), QuoteMode) = QuoteMode.QuickQuote Then

                            'If we've hit buy without save first, the quotemode will still be QQ
                            'so save and change the mode to fullquote before we redirect into the fullquote

                            SaveQuote()
                            UpdateQuickQuoteStatus()
                            Session.Item(CNQuoteMode) = QuoteMode.FullQuote

                        End If

                        'REDIRECT TO FIRST RISK SCREEN OF FULL QUOTE.                        
                        Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                        Session.Remove(CNMode)
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        Response.Redirect(sProductFolder & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" & GetFirstRiskScreen(sProductFolder & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" & oProduct.FullQuoteConfig), False)

                End Select

            End If

        End Sub

        Sub SaveQuote()

            If Session(CNIsAnonymous) IsNot Nothing Then

                If Session(CNQuote) IsNot Nothing Then

                    Dim oAnonymousQuote As NexusProvider.Quote = Session.Item(CNQuote)

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                    Dim oQuote As New NexusProvider.Quote(oAnonymousQuote.CoverStartDate, oAnonymousQuote.CoverEndDate, oAnonymousQuote.Description)
                    oQuote.PartyKey = CType(Session(CNParty), NexusProvider.BaseParty).Key
                    oQuote.ProductCode = oAnonymousQuote.ProductCode

                    Dim oRisk As New NexusProvider.Risk(Session(CNFinalScreenCode).ToString(), oAnonymousQuote.Risks(0).Description)
                    oRisk.DataModelCode = oAnonymousQuote.Risks(0).DataModelCode
                    oRisk.RiskCode = oAnonymousQuote.Risks(0).RiskCode

                    With oQuote
                        .InsuredName = oAnonymousQuote.InsuredName
                        .CurrencyCode = oAnonymousQuote.CurrencyCode
                        .SubBranchCode = oAnonymousQuote.SubBranchCode
                        .BusinessTypeCode = oAnonymousQuote.BusinessTypeCode
                        .QuoteExpiryDate = oAnonymousQuote.QuoteExpiryDate
                        .InceptionDate = oAnonymousQuote.InceptionDate
                        .RenewalDate = oAnonymousQuote.RenewalDate
                        .InceptionTPI = oAnonymousQuote.InceptionTPI
                        .FrequencyCode = oAnonymousQuote.FrequencyCode '"ANNUAL"
                        .Agent = oAnonymousQuote.Agent
                        .AccountHandlerCnt = oAnonymousQuote.AccountHandlerCnt
                        .CoverNoteBookNumber = oAnonymousQuote.CoverNoteBookNumber
                        .CoverNoteSheetNumber = oAnonymousQuote.CoverNoteSheetNumber
                    End With

                    oQuote.Risks.Add(oRisk)
                    oRisk = Nothing

                    Dim sBranchCode As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).BranchCode

                    Try
                        '30-11-07 - DH - SubBranchCode is not mandatory, but the method won't work without it
                        oWebService.AddQuoteV2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                    Finally
                        oWebService = Nothing
                    End Try
                    Dim oOptionSettings As NexusProvider.OptionTypeSetting
                    oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                    If oOptionSettings.OptionValue = "1" Then
                        oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey, v_sBranchCode:=oQuote.BranchCode, bExclusiveLock:=True)
                    End If
                    ConvertDataSet(oQuote, oAnonymousQuote.Risks(0).XMLDataset)

                    oWebService = New NexusProvider.ProviderManager().Provider

                    Try
                        oWebService.UpdateRisk(oQuote, 0, oQuote.BranchCode, oQuote.SubBranchCode)
                        Session(CNQuote) = oQuote
                    Finally
                        oWebService = Nothing
                    End Try

                    'DISPLAY SUCCESSFULLY QUOTE SAVED MESSAGE
                    pnlSummary.Visible = False
                    btnSaveQuote.Visible = False

                    Session.Remove(CNFinalScreenCode)
                    Session.Remove(CNOI)
                    Session.Remove(CNIsAnonymous)

                End If

            End If

        End Sub
        Sub UpdateQuickQuoteStatus()
            If Session(CNQuote) IsNot Nothing Then

                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"

                Doc.Load(xmlTR)
                xmlTR.Close()

                Dim xmlNodes As XmlNodeList = Doc.SelectNodes("/DATA_SET/RISK_OBJECTS/*/*[@QUICK_QUOTE|@QUICKQUOTE]")

                For Each xmlNode As XmlNode In xmlNodes
                    Try
                        If xmlNode.Attributes("QUICK_QUOTE").Value = "1" And Not String.IsNullOrEmpty(oProduct.QuickQuoteConfig) Then
                            xmlNode.Attributes("QUICK_QUOTE").Value = "0"
                            xmlNode.Attributes("US").Value = "2"
                            Exit For
                        End If
                    Catch ex As System.Exception
                        If xmlNode.Attributes("QUICKQUOTE").Value = "1" And Not String.IsNullOrEmpty(oProduct.QuickQuoteConfig) Then
                            xmlNode.Attributes("QUICKQUOTE").Value = "0"
                            xmlNode.Attributes("US").Value = "2"
                            Exit For
                        End If
                    End Try
                Next
                'this will now update the QuickQuote ELement in XML 

                Dim swContent As New System.IO.StringWriter
                Dim xmlwContent As New XmlTextWriter(swContent)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sScreenCode As String = Nothing
                Dim oRiskType As Config.RiskType
                Doc.WriteTo(xmlwContent)
                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = swContent.ToString()

                'Find the current risk code
                If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                End If

                'If screen ocde is missing then it should be picked up from fullquoteconfig
                If oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode IsNot Nothing Then
                    If oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode.Trim.Length <> 0 Then
                        sScreenCode = oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode
                    Else
                        sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                    End If
                Else
                    sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                End If

                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, oQuote.BranchCode)
                Session(CNQuote) = oQuote
                xmlwContent.Close()
                swContent.Close()
                srDataset.Dispose()

            End If
        End Sub
        ''' <summary>
        ''' Convert a QuickQuote dataset to a FullQuote dataset
        ''' </summary>
        ''' <param name="r_oNewQuote">The quote object containing the FullQuote</param>
        ''' <param name="r_sAnonymousXMLDataSet">The risks dataset of the the QuickQuote</param>
        Private Sub ConvertDataSet(ByRef r_oNewQuote As NexusProvider.Quote, ByRef r_sAnonymousXMLDataSet As String)

            'These element and attribute names are hardcoded because if the xml doesn't match the
            'code structure we're stuffed anyway, so having configurable names is next to useless

            Dim sPolicyBinderID As String = String.Empty
            Dim sGISPolicyLinkID As String = String.Empty

            Dim srDataset As New System.IO.StringReader(r_oNewQuote.Risks(0).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument

            Doc.Load(xmlTR)
            xmlTR.Close()

            Dim oDefaultNodes As New Hashtable()
            Dim oNode As XmlNode = Doc.SelectSingleNode("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER")

            If oNode IsNot Nothing Then

                'Get the ID's we need from the new 'FullQuote' dataset
                sPolicyBinderID = oNode.Attributes(Session.Item(CNDataModelCode) & "_POLICY_BINDER_ID").Value
                sGISPolicyLinkID = oNode.Attributes("GIS_POLICY_LINK_ID").Value

                'Retrieve a list of all the elements that have been created in the dataset on AddQuote,
                'as we need to update these later but don't want to recreate them in the dataset or the db
                If oNode.HasChildNodes() Then
                    GetDefaultNodes(oNode, oDefaultNodes, "/")
                End If

            End If

            srDataset = New System.IO.StringReader(r_sAnonymousXMLDataSet)
            xmlTR = New XmlTextReader(srDataset)
            Doc = New XmlDocument

            Doc.Load(xmlTR)
            xmlTR.Close()

            'Delete the output elements
            oNode = Doc.SelectSingleNode("//" & Session.Item(CNDataModelCode) & "_OUTPUT")
            If oNode IsNot Nothing Then
                oNode.ParentNode.RemoveChild(oNode)
            End If

            'Write the 'FullQuote' ID to the 'QuickQuote' dataset
            oNode = Doc.SelectSingleNode("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER")
            If oNode IsNot Nothing Then
                oNode.Attributes("GIS_POLICY_LINK_ID").Value = sGISPolicyLinkID
            End If

            'Get all nodes which contain a 'US' attrbute, these are all the nodes we need to edit
            Dim xmlNodes As XmlNodeList = Doc.SelectNodes("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER//*[@US]")

            For Each xmlNode As XmlNode In xmlNodes

                'Change the policy binder ID's to match the FQ policy and change
                'the 'US' attributes to show they are new and need adding to the db
                If xmlNode.Attributes(Session.Item(CNDataModelCode) & "_POLICY_BINDER_ID") IsNot Nothing Then
                    xmlNode.Attributes(Session.Item(CNDataModelCode) & "_POLICY_BINDER_ID").Value = sPolicyBinderID
                End If
                xmlNode.Attributes("US").Value = "1"

            Next

            'Change all the elements that are created on AddQuote to a US status of 2 (overwriting any statuses of 1),
            'as all these elements have been updated but not created as they already exist in the new dataset
            For Each oDefaultNode As DictionaryEntry In oDefaultNodes
                oNode = Doc.SelectSingleNode("//" & Session.Item(CNDataModelCode) & "_POLICY_BINDER" & oDefaultNode.Value)
                If oNode IsNot Nothing Then
                    oNode.Attributes("US").Value = "2"
                End If
            Next

            Dim swContent As New System.IO.StringWriter
            Dim xmlwContent As New XmlTextWriter(swContent)

            Doc.WriteTo(xmlwContent)
            r_oNewQuote.Risks(0).XMLDataset = swContent.ToString()

            xmlwContent.Close()
            swContent.Close()

        End Sub

        ''' <summary>
        ''' Retrieve a list of elements that create by default on the FullQuote before any data entry has taken place
        ''' </summary>
        ''' <param name="r_oNode">The node retrieve child nodes from</param>
        ''' <param name="r_oDefaultNodes">A Hashtable containing the list of node paths retrieved</param>
        ''' <param name="sPath">The xml node path of the node being inspected</param>
        ''' <remarks>This function will make nested calls to itself to work down the xml tree</remarks>
        Public Sub GetDefaultNodes(ByRef r_oNode As XmlNode, ByRef r_oDefaultNodes As Hashtable, ByVal sPath As String)

            For i As Integer = 0 To r_oNode.ChildNodes.Count - 1

                r_oDefaultNodes.Add(r_oDefaultNodes.Count, sPath & r_oNode.ChildNodes(i).Name)

                If r_oNode.ChildNodes(i).HasChildNodes Then
                    GetDefaultNodes(r_oNode.ChildNodes(i), r_oDefaultNodes, sPath & r_oNode.ChildNodes(i).Name & "/")
                End If

            Next

        End Sub

    End Class

End Namespace
