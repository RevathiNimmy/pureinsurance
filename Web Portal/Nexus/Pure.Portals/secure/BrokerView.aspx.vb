Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Reflection
Imports System.Linq

Namespace Nexus
    Partial Class secure_BrokerView
        Inherits Frontend.clsCMSPage
        Dim oWebService As NexusProvider.ProviderBase
        Dim oPartySummary As NexusProvider.PartySummary
        Public Shared CNSortDirection As String = ""
        Public Shared CNSortExpression As String = ""
        Dim sDisplayStatus As String()
        Dim iQuoteCount As Integer
        Dim iOffset As Integer = 2
        Public Const CNBrokerCollection As String = "BrokerCollection"
        Public Const CNBrokerChildCollection As String = "BrokerChildCollection"
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
        Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
        Dim oRiskType As New NexusProvider.RiskType
        'Fix for 3509- Added hastable to show only one ReInstatement Link
        Dim hstMTACanVerion As Hashtable = New Hashtable()
        Dim sTxtAgentName As String
        Public WriteOnly Property DisplayStatus() As String
            Set(ByVal value As String)
                'split the value entered into an array as it will contain comma separated values
                sDisplayStatus = Split(value, ",")
            End Set
        End Property

        Protected Sub grdvBroker_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvBroker.Load

            grdvBroker.AllowPaging = True

            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            If (oUserDetails IsNot Nothing AndAlso oUserDetails.Key <> 0 AndAlso oUserDetails.PartyType = "AG") Then
                hvIsBroker.Value = 1
            Else
                hvIsBroker.Value = 0
            End If
            LoadPageFromSessionFilters()
            'IIf(grdvQuotes.PageCount = 1, grdvQuotes.AllowPaging = False, grdvQuotes.AllowPaging = True)
        End Sub

        Protected Sub grdvBroker_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvBroker.PageIndexChanging
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
            If CNSortExpression <> "" Then
                oPolicyCollection.SortColumn = CNSortExpression
                oPolicyCollection.SortingOrder = CNSortDirection
                oPolicyCollection.Sort()
            End If
            grdvBroker.DataSource = oPolicyCollection
            If grdvBroker.PageCount <= 1 Then
                grdvBroker.AllowPaging = False
            Else
                grdvBroker.AllowPaging = True
            End If
            grdvBroker.PageIndex = e.NewPageIndex
            grdvBroker.DataBind()

        End Sub

        Private Sub LoadPageFromSessionFilters()
            If Session(CNPolicyBackButton) IsNot Nothing AndAlso Session(CNPolicyBackButton) = "BackButton" Then
                If Session(CNFindPolicySearchData) IsNot Nothing Then
                    Dim sPolicySearch As New NexusProvider.PolicySearchCriteria
                    sPolicySearch = CType(Session(CNFindPolicySearchData), NexusProvider.PolicySearchCriteria)
                    Dim QuoteDate As String = sPolicySearch.QuoteDate
                    QuoteDate = IIf(QuoteDate = "00:00:00", "", QuoteDate)
                    Dim StartDate As String = sPolicySearch.StartDate
                    StartDate = IIf(StartDate = "00:00:00", "", StartDate)
                    txtPolicyNumber.Text = sPolicySearch.PolicyNumber
                    ddlProductType.SelectedValue = sPolicySearch.product
                    ddlRecordType.SelectedValue = sPolicySearch.RecordType
                    txtAgentName.Text = sPolicySearch.AgentName
                    txtQuoteDate.Text = QuoteDate
                    txtStartDate.Text = StartDate
                    txtName.Text = sPolicySearch.InsuredName
                    GetbrokerSummary()
                End If
            End If


        End Sub

#Region " CLEAR SESSION VALUE "

        ''' <summary>
        ''' Clear QuoteCollection SessionValues .
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub ClearQuoteCollectionSessionValues()
            Session.Remove(CNQuoteCollectionFiles)
            Session.Remove(CNTotalForQuoteCollection)
            Session.Remove(CNPolicySummaryCollection)
            hstMTACanVerion.Clear()
        End Sub

#End Region

        Protected Sub grdvBroker_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvBroker.RowCommand
            'This is the best place to Reset the session in case when we choose the client again 
            'and then he chooses to buy a policy or do a quote after he has done Quote Collection Process
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Session.Remove(CNOnlyOriginalRating)
            Dim bExclusiveLock As Boolean = True
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            If (e.CommandName.ToString.ToUpper = "VIEWMTA" Or
                e.CommandName.ToString.ToUpper = "VIEWPOLICY" Or
                e.CommandName.ToString.ToUpper = "VIEW" Or
                e.CommandName.ToString.ToUpper = "VIEWUNDERRENEWALPOLICY") Then
                bExclusiveLock = False
            ElseIf Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                'Unlock policy for same user
                Dim nInsuranceFolderKey As Integer
                Dim GridRow As GridViewRow
                GridRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)

                UnlockPolicy(nInsuranceFolderKey)
            End If
            If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                Dim oQuote As NexusProvider.Quote
                Dim iCurrentRiskKey As Integer
                Dim sRedirectPath As String = String.Empty
                Session.Remove(CNOldPremium) 'Remove the old premium from session
                Session.Remove(CNRiskType) 'Reset the Risk Type
                Session.Remove(CNCurrentRiskKey) 'Reset the Risk Key
                ClearClaims() 'to Clear the claim session variable if any
                Dim nInsuranceFileKeyForAction As Integer = Convert.ToInt32(e.CommandArgument)

                Select Case e.CommandName
                    Case "viewMTA", "viewpolicy"

                    Case Else
                        If e.CommandName = "VoidPolicy" Then
                            Dim iInsuranceFolderKey As Integer
                            Dim GridRow As GridViewRow
                            GridRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                            If GridRow IsNot Nothing Then
                                Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                                If lblInsuranceFolderKey IsNot Nothing Then
                                    iInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                                    SetPolicyVersionVoid(e.CommandArgument, iInsuranceFolderKey)
                                End If
                            End If
                        End If
                            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                        If oExclusiveLocking.OptionValue = "1" Then

                            Dim oInsuranceFiles As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
                            Dim oPolicyDetail = (From oInsuranceFile In oInsuranceFiles
                                                 Where oInsuranceFile.InsuranceFileKey = nInsuranceFileKeyForAction
                                                 Select oInsuranceFile).FirstOrDefault

                            Dim sUserName As String = CheckLock(oPolicyDetail.InsuranceFolderKey, Session(CNBranchCode).ToString)
                            If sUserName.Trim.Length > 0 Then
                                Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", sUserName + ".") + "')"
                                scriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                                Return
                            End If

                            oQuote = oWebService.GetHeaderAndSummariesByKey(nInsuranceFileKeyForAction, bExclusiveLock:=True)
                            'oWebService = Nothing
                            ' oPolicyDetail = Nothing
                        End If
                End Select
                'Copy Quote needs to be handled separately first to aviod unnecessary SAM calls
                Select Case e.CommandName

                    Case "CopyQuote"

                End Select

                Try
                    If hvMarketPlacePolicy.Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(hvMarketPlacePolicy.Value) AndAlso hvMarketPlacePolicy.Value.ToString.ToUpper = "FALSE" Then
                        oWebService.UpdateMarketplacePolicyStatus(e.CommandArgument, Convert.ToBoolean(hvMarketPlacePolicy.Value))
                    End If
                    oQuote = oWebService.GetHeaderAndSummariesByKey(e.CommandArgument)
                    'Put Party information into CNParty Session
                    Dim oFindParty As NexusProvider.BaseParty
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oFindParty = oWebService.GetParty(oQuote.PartyKey)
                    Session(CNParty) = oFindParty

                    'Locking message is not required for View Mode
                    Dim bIgnoreLocking As Boolean = False
                    If e.CommandName = "viewMTA" Or e.CommandName = "viewpolicy" Then
                        bIgnoreLocking = True
                    End If
                    'Put highest risk key into Session
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                        iCurrentRiskKey = oQuote.Risks(i).Key
                    Next
                    If oQuote.Risks.Count = 0 AndAlso Session(CNRiskType) Is Nothing Then

                        'find the risk type associated with this product
                        Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                        Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
                        Dim oProductConfiguration As Nexus.Library.Config.Product
                        oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)
                        Session(CNQuote) = oQuote
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
                            AddRiskAndRedirect()
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
                            AddRiskAndRedirect()
                        ElseIf iRiskCount > 1 AndAlso oProductConfiguration.AllowMultiRisks = True Then
                            'more than one risk type so we need to open the modal dialog
                            Dim sUrl As String = String.Empty
                            If HttpContext.Current.Session.IsCookieless Then
                                sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                            Else
                                sUrl = AppSettings("WebRoot") & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                            End If

                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                        End If
                        Exit Sub
                    End If

                    oWebService.GetHeaderAndRisksByKey(oQuote)

                    Session(CNQuote) = oQuote
                    Session(CNCurrentRiskKey) = 0
                    DataSetFunctions.GetScreens()
                Catch ex As NexusProvider.NexusException

                    Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                        Case "200" 'Policy Locking
                            'Show policy locking error as alert
                            Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                            scriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Server.ClearError()
                            ClearQuote()
                            Exit Sub
                        Case "1000148" 'Policy Locking
                            'Show policy locking error as alert
                            Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(1) + ".") + "')"
                            scriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Server.ClearError()
                            ClearQuote()
                            Exit Sub
                        Case Else
                            Throw ex
                    End Select
                Finally
                    'oWebService = Nothing
                End Try

                Session(CNCurrenyCode) = oQuote.CurrencyCode
                'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

                'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                GetDataSetDefinition()

                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"

                'this will need to be set to nothing in case after doing MTA process user selects client
                ' and then choses to buy a Quote 
                Session(CNMTAType) = Nothing
                Session(CNViewType) = Nothing
                Select Case e.CommandName
                    Case "viewMTA" ''added by sbhatia on dated 27-feb
                        Session(CNRenewal) = Nothing
                        If (oQuote.InsuranceFileTypeCode).Trim = "MTACAN" Then
                            Session(CNMTAType) = MTAType.CANCELLATION
                        ElseIf (oQuote.InsuranceFileTypeCode).Trim = "MTA PERM" Then
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.PERMANENT_MTA
                        ElseIf (oQuote.InsuranceFileTypeCode).Trim = "MTA TEMP" Then
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.TEMPORARY_MTA
                        Else
                            Session(CNMTAType) = MTAType.PERMANENT
                        End If


                        Session(CNMode) = Mode.View
                        Session.Remove(CNOI)
                        Session(CNQuoteInSync) = False
                        Session(CNQuoteMode) = QuoteMode.MTAQuote
                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If

                    Case "viewpolicy"
                        Session(CNRenewal) = Nothing
                        Session(CNMode) = Mode.View
                        Session.Remove(CNOI)
                        Session.Remove(CNQuoteMode)
                        Session(CNQuoteInSync) = False
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        'WILL IT BE PREMIUM DISPLAY FOR FULL QUOTE ALWAYS????
                        'DO WE NEED TO ADD POLICY DETAILS TO SESSION? HOW WILL THE PREMIUM DISPLAY PAGE GET THE POLICY NUMBER FOR WHICH THE DETAILS NEEDS TO FETCH?

                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If
                    Case "viewunderRenewalpolicy"
                        Session(CNRenewal) = True
                        Session(CNMode) = Mode.View
                        Session.Remove(CNOI)
                        Session.Remove(CNQuoteMode)
                        Session(CNQuoteInSync) = False
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        'WILL IT BE PREMIUM DISPLAY FOR FULL QUOTE ALWAYS????
                        'DO WE NEED TO ADD POLICY DETAILS TO SESSION? HOW WILL THE PREMIUM DISPLAY PAGE GET THE POLICY NUMBER FOR WHICH THE DETAILS NEEDS TO FETCH?
                        'sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If
                    Case "editquote"
                        Session(CNRenewal) = Nothing
                        Session(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNQuoteInSync) = False


                        If IsDataSetQuickQuote() = False Then
                            Session(CNQuoteMode) = QuoteMode.FullQuote
                        Else
                            Session(CNQuoteMode) = QuoteMode.QuickQuote
                        End If


                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        ElseIf oProduct.AllowMultiRisks = False Then
                            Dim oRiskType As New NexusProvider.RiskType
                            Dim oRisk As Config.RiskType
                            Dim sFirstRiskPage As String

                            If oQuote.Risks(0).RiskCode Is Nothing Then
                                oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
                            Else
                                oRisk = oProduct.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
                            End If

                            oRiskType.DataModelCode = oRisk.DataModelCode
                            oRiskType.Name = oRisk.Name
                            oRiskType.Path = oRisk.Path
                            oRiskType.RiskCode = oRisk.RiskCode
                            Session(CNRiskType) = oRiskType
                            'Get first risk page 
                            sFirstRiskPage = GetFirstRiskScreen(sProductFolder & "/" & oRiskType.Path & "/fullquote.config")
                            sRedirectPath = sProductFolder & "/" & oRiskType.Path & "/" & sFirstRiskPage
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If

                    Case "editmtaquote"
                        Session(CNRenewal) = Nothing
                        'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                        Session(CNMtaReasonSelected) = Nothing
                        Dim oPolicy As NexusProvider.PolicyCollection
                        Dim TempVar As Integer
                        Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                        For TempVar = 0 To oPolicy.Count - 1
                            If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "POLICY" Or
                           oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS" Then
                                SelMTAQuoteStartDate = oQuote.CoverStartDate
                                ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                                If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                    Session(CNIsBackDatedMTA) = True
                                End If
                            End If
                            If oPolicy.Item(oPolicy.Count - 1).InsuranceFileTypeCode.Trim.ToUpper() = "MTAQREINS" Then
                                Session.Remove(CNIsBackDatedMTA)
                                Exit For
                            End If
                        Next
                        If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                            oWebService.UpdateQuotev2(oQuote)
                        End If
                        Session(CNQuote) = oQuote
                        If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                            Session(CNMTAType) = MTAType.CANCELLATION
                        Else
                            Session(CNMTAType) = MTAType.PERMANENT
                        End If
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session.Item(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session(CNMtaReasonSelected) = Nothing

                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If

                    Case "MTAquote"
                        Session(CNMode) = Mode.Edit
                        Session.Remove(CNOI)
                        Session(CNRenewal) = Nothing
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session(CNQuoteInSync) = False
                        Session(CNMtaReasonSelected) = Nothing
                        sRedirectPath = "~/secure/MTAReason.aspx"

                    Case "buymtaquote"
                        Session(CNRenewal) = Nothing
                        'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                        Session(CNMtaReasonSelected) = Nothing
                        Dim oPolicy As NexusProvider.PolicyCollection
                        Dim TempVar As Integer
                        Dim SelMTAQuoteStartDate, ExistingMTAStartDate As Date
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oPolicy = oWebService.GetAllPolicyVersions(oQuote.InsuranceFolderKey)
                        SetCurrentMTATypeSession()
                        If Not GetCurrentMTAType = MTAType.TEMPORARY Then
                            For TempVar = 0 To oPolicy.Count - 1
                                If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "POLICY" Or
                               oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" Or oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS" Then
                                    SelMTAQuoteStartDate = oQuote.CoverStartDate
                                    ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                                    If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                        Session(CNIsBackDatedMTA) = True
                                    End If
                                End If
                                If oPolicy.Item(oPolicy.Count - 1).InsuranceFileTypeCode.Trim.ToUpper() = "MTAQREINS" Then
                                    Session.Remove(CNIsBackDatedMTA)
                                    Exit For
                                End If
                            Next
                        End If
                        If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                            oWebService.UpdateQuotev2(oQuote)
                        End If
                        Session(CNQuote) = oQuote
                        If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                            Session(CNMTAType) = MTAType.CANCELLATION
                        Else
                            Session(CNMTAType) = MTAType.PERMANENT
                        End If
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session.Item(CNMode) = Mode.Buy
                        Session(CNQuoteInSync) = False

                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If


                    Case "buyquote"
                        Session(CNRenewal) = Nothing
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        'TO Be Cross Check
                        Session(CNQuoteInSync) = False
                        Dim oRisk As NexusProvider.Risk = oQuote.Risks.FindItemByRiskKey(iCurrentRiskKey)

                        If oRisk IsNot Nothing Then

                            Select Case oRisk.StatusCode
                                Case "DECLINE"
                                    If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                        Response.Redirect(DataSetFunctions.sDeclineScreenURL, False)
                                    Else
                                        sRedirectPath = "~/declined.aspx"
                                    End If
                                Case "REFER"
                                    If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                        Response.Redirect(DataSetFunctions.sReferScreenURL)
                                    Else
                                        sRedirectPath = "~/referred.aspx"
                                    End If
                                Case "QUOTED"
                                    Session.Item(CNMode) = Mode.Buy
                                    Session(CNCurrentRiskKey) = 0
                                    If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                        oWebService.UpdateQuotev2(oQuote)
                                        Session(CNQuote) = oQuote
                                    End If
                                    If IsDataSetQuickQuote() = True Then
                                        If CheckRefer() = True Then
                                            Session(CNQuoteMode) = QuoteMode.FullQuote
                                            If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sReferScreenURL)
                                            Else
                                                Response.Redirect("~/referred.aspx")
                                            End If

                                        ElseIf CheckDecline() = True Then
                                            Session(CNQuoteMode) = QuoteMode.FullQuote
                                            If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                            Else
                                                Response.Redirect("~/declined.aspx")
                                            End If

                                        Else
                                            Session(CNQuoteMode) = QuoteMode.FullQuote
                                            If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                                sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                            Else
                                                sRedirectPath = "~/secure/PremiumDisplay.aspx"
                                            End If
                                        End If
                                    Else
                                        Session(CNQuoteMode) = QuoteMode.QuickQuote
                                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                        Else
                                            sRedirectPath = "~/QQPremium.aspx"
                                        End If
                                    End If
                                Case Else
                                    Session.Item(CNMode) = Mode.Buy
                                    Session(CNCurrentRiskKey) = 0
                                    If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                        oWebService.UpdateQuotev2(oQuote)
                                        Session(CNQuote) = oQuote
                                    End If
                                    If IsDataSetQuickQuote() = False Then
                                        If CheckRefer() = True Then
                                            Session(CNQuoteMode) = QuoteMode.FullQuote

                                            If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sReferScreenURL)
                                            Else
                                                Response.Redirect("~/referred.aspx")
                                            End If
                                        ElseIf CheckDecline() = True Then
                                            Session(CNQuoteMode) = QuoteMode.FullQuote

                                            If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                            Else
                                                Response.Redirect("~/declined.aspx")
                                            End If
                                        Else
                                            Session(CNQuoteMode) = QuoteMode.FullQuote
                                            If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                                sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                            Else
                                                sRedirectPath = "~/secure/PremiumDisplay.aspx"
                                            End If
                                        End If
                                    Else
                                        Session(CNQuoteMode) = QuoteMode.QuickQuote
                                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                        Else
                                            sRedirectPath = "~/QQPremium.aspx"
                                        End If
                                    End If
                            End Select

                            oRisk = Nothing

                        End If
                    Case "viewDetails" 'Renewal Policy is being viewed
                        ResetTransactionInSession()
                        Session(CNMode) = Mode.Buy
                        Session.Remove(CNOI)
                        Session(CNRenewal) = True
                        Session.Remove(CNQuoteMode)
                        Session(CNQuoteInSync) = False
                        Session(CNQuoteMode) = QuoteMode.FullQuote

                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If

                    Case "Reinstatement"

                        Session.Remove(CNRenewal)
                        Session(CNQuote) = oQuote
                        Session(CNMTAType) = MTAType.REINSTATEMENT
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNMtaReasonSelected) = Nothing
                        Session.Remove(CNIsBackDatedMTA)
                        sRedirectPath = "~/secure/MTAReason.aspx"

                End Select

                Response.Redirect(sRedirectPath, False)

            End If

        End Sub

        Protected Sub grdvBroker_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBroker.RowCreated
            If oPortal.EnableMasterClientAssociate = False Then
                If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow) Then
                    grdvBroker.Columns(9).Visible = False
                End If
            End If
        End Sub

        Protected Sub grdvBroker_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBroker.RowDataBound
            Dim htReinstat As New Hashtable
            htReinstat = ViewState("htReinstat")

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oSearchCriteria As NexusProvider.PartySearchCriteria = Session.Item(CNClientSearchCriteria)
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                If oSearchCriteria IsNot Nothing Then
                    If e.Row.Cells(0).Text.Trim = oSearchCriteria.PolicyRef.Trim Then
                        ' To highlight the row for matched condition
                        e.Row.RowState = DataControlRowState.Selected
                    End If
                End If

                Dim iCounter As Integer = 0
                Dim lnkbtnSelect As LinkButton = e.Row.FindControl("lnkbtnSelect")
                Dim lnkbtnSelect2 As LinkButton = e.Row.FindControl("lnkbtnSelect2")
                Dim lnkbtnCopyQuote As LinkButton = e.Row.FindControl("lnkbtnCopyQuote")
                Dim lnkbtnVoid As LinkButton = e.Row.FindControl("lnkbtnVoid")
                Dim lbl_Status As Label = e.Row.FindControl("lbl_Status")
                Dim bShowVoidButton As Boolean = False
                Dim grdvSubBroker As GridView = e.Row.FindControl("grdvSubBroker")
                Dim imgExpand As Image = e.Row.FindControl("imgExpand")
                If (hvGridIDs.Value = "") Then
                    hvGridIDs.Value = grdvSubBroker.ClientID
                Else
                    hvGridIDs.Value = hvGridIDs.Value & "," & grdvSubBroker.ClientID
                End If
                Dim dExpiryDate As Date = CType(e.Row.DataItem, NexusProvider.Policy).QuoteExpiryDate
                If oPortal.EnableMasterClientAssociate = True Then
                    If e.Row.RowType = DataControlRowType.DataRow Then
                        Dim xmldoc As New System.Xml.XmlDocument
                        If e.Row.DataItem IsNot Nothing Then
                            'If Not (CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients Is Nothing) Then
                            If ((CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients IsNot Nothing) AndAlso Not (String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients))) Then
                                xmldoc.InnerXml = CType(e.Row.DataItem, NexusProvider.Policy).AssociatedClients

                                Dim rptrFolderNavigation As Repeater = e.Row.FindControl("rptrAssociateClient")
                                If rptrFolderNavigation IsNot Nothing Then
                                    rptrFolderNavigation.DataSource = xmldoc.SelectNodes("/Associates/Associate")
                                    rptrFolderNavigation.DataBind()
                                End If
                            End If
                        End If
                    End If
                End If
                'WPR63 - Find quote versions and populate the child grid
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim sRowQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, CType(e.Row.DataItem, NexusProvider.Policy).ProductCode, "")
                If (Not String.IsNullOrEmpty(sRowQuoteVersioning) AndAlso sRowQuoteVersioning.Trim = "1") Then
                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    If (CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion > 0) Then
                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "QUOTE" Or (CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "POLICY" And CType(e.Row.DataItem, NexusProvider.Policy).RenewedVersion = 0) Then
                            If (oUserDetails IsNot Nothing And oUserDetails.Key = 0) Then
                                If (lbl_Status IsNot Nothing) Then
                                    If (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Issued) Then
                                        lbl_Status.Text = "Issued V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                    ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                                        lbl_Status.Text = "Pending V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                    ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending And CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "QUOTE") Then
                                        lbl_Status.Text = "Agent Pending V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                    ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentComplete) Then
                                        lbl_Status.Text = "Agent Complete V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                    ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined) Then
                                        lbl_Status.Text = "Declined V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion

                                    ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Live) Then
                                        lbl_Status.Text = "Made Live V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                    End If
                                End If
                            ElseIf (oUserDetails IsNot Nothing And oUserDetails.Key <> 0) Then
                                If (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentComplete Or CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Issued) Then
                                    lbl_Status.Text = "Complete V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                                    lbl_Status.Text = "Referred V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending And CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "QUOTE") Then
                                    lbl_Status.Text = "Incomplete V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined) Then
                                    lbl_Status.Text = "Declined V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Live) Then
                                    lbl_Status.Text = "Made Live V." & CType(e.Row.DataItem, NexusProvider.Policy).QuoteVersion
                                End If
                            End If
                            If (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined) Or (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending) Or (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                                lnkbtnSelect2.Enabled = False
                            End If
                            If (oUserDetails IsNot Nothing AndAlso oUserDetails.Key <> 0) Then
                                If (lnkbtnSelect IsNot Nothing) Then
                                    If (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                                        lnkbtnSelect.Enabled = False
                                    End If
                                End If
                            End If
                        End If


                        Dim sBaseInsuranceFolderKey As String
                        sBaseInsuranceFolderKey = Convert.ToString(grdvBroker.DataKeys(e.Row.RowIndex).Values(1))

                        Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerChildCollection)
                        Dim oPolicy = (From ps In oPolicyCollection Where ps.BaseInsuranceFolderKey = sBaseInsuranceFolderKey And (ps.PolicyTypeCode.ToString.Trim.ToUpper = "QUOTE") Order By ps.quoteversion Descending)

                        'If UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim()) = "QUOTE" Or _
                        '(UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim()) = "POLICY") Then
                        If UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim()) = "QUOTE" Then
                            For Each policy As NexusProvider.Policy In oPolicy
                                If (oUserDetails IsNot Nothing And oUserDetails.Key = 0) Then
                                    If (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Issued) Then
                                        policy.InsuranceFileTypeCode = "Issued V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                                        policy.InsuranceFileTypeCode = "Pending V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending And policy.PolicyTypeCode.Trim.ToUpper = "QUOTE") Then
                                        policy.InsuranceFileTypeCode = "Agent Pending V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentComplete) Then
                                        policy.InsuranceFileTypeCode = "Agent Complete V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined) Then
                                        policy.InsuranceFileTypeCode = "Declined V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Live) Then
                                        policy.InsuranceFileTypeCode = "Made Live V." & policy.QuoteVersion
                                    End If
                                ElseIf (oUserDetails IsNot Nothing And oUserDetails.Key <> 0) Then
                                    If (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Issued Or policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentComplete) Then
                                        policy.InsuranceFileTypeCode = "Complete V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                                        policy.InsuranceFileTypeCode = "Referred V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending And policy.PolicyTypeCode.Trim.ToUpper = "QUOTE") Then
                                        policy.InsuranceFileTypeCode = "Incomplete V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined) Then
                                        policy.InsuranceFileTypeCode = "Declined V." & policy.QuoteVersion
                                    ElseIf (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Live) Then
                                        policy.InsuranceFileTypeCode = "Made Live V." & policy.QuoteVersion
                                    End If
                                End If

                                Dim iGracePeriod As Integer

                                If policy.QuoteExpiryDate = Date.MinValue Then
                                    iGracePeriod = IIf(GetQuoteGracePeriod(policy.ProductCode.Trim()) = "", 0, GetQuoteGracePeriod(policy.ProductCode.Trim()))
                                    dExpiryDate = policy.CoverStartDate.AddDays(iGracePeriod).ToShortDateString()
                                Else
                                    dExpiryDate = policy.QuoteExpiryDate
                                End If
                                If (dExpiryDate < DateTime.Today) Then
                                    'lnkbtnSelect.Enabled = False
                                    lnkbtnSelect2.Enabled = False
                                End If

                                'code to check if parent policy is not an Agent Pending or Pending Quote, then disable the "Buy" button
                                iCounter = iCounter + 1


                            Next

                            If (oPolicy.Count > 1) Then
                                grdvSubBroker.DataSource = oPolicy
                                If grdvSubBroker.PageCount <= 1 Then
                                    grdvSubBroker.AllowPaging = False
                                Else
                                    grdvSubBroker.AllowPaging = True
                                End If
                                grdvSubBroker.DataBind()
                            Else
                                If (imgExpand IsNot Nothing) Then
                                    imgExpand.Visible = False
                                End If
                            End If

                        End If

                    End If
                Else
                    grdvSubBroker.Visible = False
                    imgExpand = e.Row.FindControl("imgExpand")
                    imgExpand.Visible = False
                End If

                Dim bIsReferred As Boolean = Convert.ToBoolean(CType(e.Row.DataItem, NexusProvider.Policy).RiskStatus.ToString.Trim.ToUpper = "REFERRED")
                Select Case UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim())


                    Case "POLICY"
                        ' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again
                        If CType(e.Row.DataItem, NexusProvider.Policy).IsCurrent = True Then
                            bShowVoidButton = ShowHideVoidButton(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)
                            If bShowVoidButton AndAlso lnkbtnVoid IsNot Nothing Then
                                lnkbtnVoid.Visible = True
                                lnkbtnVoid.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnVoid.Attributes.Add("OnClick", "VoidPolicyVersionConfirmation();")
                            End If
                            If UserCanDoTask("MidTermAdjustment") Then
                                'Only Allow MTA if User Has Authority to do that
                                If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False And CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim <> "REP" Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                        lnkbtnSelect.CommandName = "MTAquote"
                                        'This code is added for unmarking the quote for collection
                                        If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                    End If
                                End If
                            End If
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REN" Then
                            'if the plocy has been cancelled then only one link i.e VIEW
                            If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REN" Then
                                If IsInRenewal(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = True Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                        lnkbtnSelect.CommandName = "MTAquote"
                                        'This code is added for unmarking the quote for collection
                                        If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                        'end
                                    End If
                                End If
                            End If

                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"

                        ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then

                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True

                            If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                e.Row.Cells(3).Text = GetLocalResourceObject("lbl_Lapsed")
                            End If

                        ElseIf IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = True Then
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        Else
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "QUOTE"

                        'Edit/Buy options will be available only if user has Authority
                        If UserCanDoTask("NewBusiness") Then

                            lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                            lnkbtnSelect.CommandName = "editquote"
                            If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy AndAlso Not bIsReferred Then
                                lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                            If (dExpiryDate < DateTime.Today) Then
                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_view").ToString() '"edit"
                                lnkbtnSelect.CommandName = "viewpolicy"
                            Else
                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                                lnkbtnSelect.CommandName = "editquote"
                            End If

                            'This code is added for unmarking the quote for collection
                            If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            End If

                            'end 
                            If Not UserCanDoTask("DisableBuyNow") Then
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_buy").ToString() '"edit"
                                lnkbtnSelect2.CommandName = "buyquote"
                            End If

                            If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy AndAlso Not bIsReferred Then
                                lnkbtnSelect2.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                        End If

                        If UserCanDoTask("ViewQuote") Then
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        End If

                        If Session(CNLoginType) = LoginType.Agent And UserCanDoTask("CopyQuote") Then

                            'Copy link will be available only for agents, if has Authority
                            'Make the column and link available to user
                            grdvBroker.Columns(7).Visible = True
                            lnkbtnCopyQuote.Visible = True
                            lnkbtnCopyQuote.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey

                        End If

                        e.Row.Cells(1).Text = CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatus

                    Case "WRITTEN"

                        'Edit/Buy options will be available only if user has Authority
                        If UserCanDoTask("NewBusiness") Then
                            'code commented to hide the edit button for Written Policy status
                            'lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            'lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                            'lnkbtnSelect.CommandName = "editquote"
                            'This code is added for unmarking the quote for collection
                            If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            End If
                            If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                lnkbtnSelect2.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                            'end
                            If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_buy").ToString() '"edit"
                                lnkbtnSelect2.CommandName = "buyquote"
                            End If
                            If UserCanDoTask("ViewQuote") Then
                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect.CommandName = "viewpolicy"
                                lnkbtnSelect.Visible = True
                            End If
                        End If

                    Case "MTAQUOTE", "MTAQTETEMP", "MTAQCAN"

                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LIVE" Then
                            If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False Then
                                If UserCanDoTask("MidTermAdjustment") Then
                                    lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                                    lnkbtnSelect.CommandName = "editmtaquote"
                                    'This code is added for unmarking the quote for collection
                                    If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                        lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                    End If
                                    If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                        lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        lnkbtnSelect2.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                    End If
                                    'end
                                    If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                                        lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect2.Text = GetLocalResourceObject("lbl_buy").ToString() '"edit"
                                        lnkbtnSelect2.CommandName = "buymtaquote"
                                    End If
                                End If
                            End If
                        ElseIf UserCanDoTask("ViewQuote") Then
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"


                        ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LAP" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "REP" Then
                            'if the plocy has been cancelled then only one link i.e VIEW
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "MTAQREINS" '' edited by SB on 3 march

                        'Only Allow MTA if User Has Authority to do that

                        '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.fixed against PN:42284
                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LIVE" Then

                            If UserCanDoTask("MidTermReinstatement") Then
                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                                lnkbtnSelect.CommandName = "editmtaquote"
                                'This code is added for unmarking the quote for collection
                                If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                End If
                                If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy AndAlso Not bIsReferred Then
                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                    lnkbtnSelect2.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                End If
                                'end
                                If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                                    lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnSelect2.Text = GetLocalResourceObject("lbl_buy").ToString() '"edit"
                                    lnkbtnSelect2.CommandName = "buymtaquote"
                                End If
                            End If
                        Else
                            'if the plocy has been cancelled then only one link i.e VIEW
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"

                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "MTA PERM", "MTA TEMP"
                        Select Case UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim())
                            Case "MTA PERM"
                                If CType(e.Row.DataItem, NexusProvider.Policy).IsCurrent = True And CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim <> "REP" Then
                                    bShowVoidButton = ShowHideVoidButton(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)
                                    If bShowVoidButton AndAlso lnkbtnVoid IsNot Nothing Then
                                        lnkbtnVoid.Visible = True
                                        lnkbtnVoid.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnVoid.Attributes.Add("OnClick", "VoidPolicyVersionConfirmation();")
                                    End If
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                        lnkbtnSelect.CommandName = "MTAquote"
                                        If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                    End If
                                ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                                Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REN" Then
                                    'if the plocy has been cancelled then only one link i.e VIEW
                                    If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REN" Then
                                        If IsInRenewal(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = True Then
                                            If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                                lnkbtnSelect.CommandName = "MTAquote"
                                                'This code is added for unmarking the quote for collection
                                                If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                                End If
                                                If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                                End If
                                                'end
                                            End If
                                        End If
                                    End If
                                ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then

                                    lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                    lnkbtnSelect2.CommandName = "viewpolicy"
                                    lnkbtnSelect2.Visible = True

                                    If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                        e.Row.Cells(3).Text = GetLocalResourceObject("lbl_Lapsed")
                                    End If
                                End If
                                If (imgExpand IsNot Nothing) Then
                                    imgExpand.Visible = False
                                End If
                            Case "MTA TEMP"
                                bShowVoidButton = ShowHideVoidButton(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)
                                If bShowVoidButton AndAlso lnkbtnVoid IsNot Nothing Then
                                    lnkbtnVoid.Visible = True
                                    lnkbtnVoid.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnVoid.Attributes.Add("OnClick", "VoidPolicyVersionConfirmation();")
                                End If
                        End Select

                        If oPortalConfig.ViewOnlyLatestPolicyVersion = True _
                        And UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim()) = "MTA PERM" Then
                            If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                lnkbtnSelect.CommandName = "MTAquote"
                                'Call this for UnMarking the Quote For collection
                                'This code is added for unmarking the quote for collection
                                If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                End If
                                If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                End If
                                'end
                            End If
                        End If

                        lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                        lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                        lnkbtnSelect2.CommandName = "viewMTA"
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "MTAREINS"
                        '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.fixed against PN:42284
                        If CType(e.Row.DataItem, NexusProvider.Policy).IsCurrent = True Then
                            bShowVoidButton = ShowHideVoidButton(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)
                            If bShowVoidButton AndAlso lnkbtnVoid IsNot Nothing Then
                                lnkbtnVoid.Visible = True
                                lnkbtnVoid.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnVoid.Attributes.Add("OnClick", "VoidPolicyVersionConfirmation();")
                            End If
                            If UserCanDoTask("MidTermAdjustment") Then
                                'CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).AllowMTA Then
                                If IsInRenewal(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = True _
                                    Or IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False _
                                    Or IsReinstated(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = True Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                        lnkbtnSelect.CommandName = "MTAquote"
                                        'This code is added for unmarking the quote for collection
                                        If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                        'end
                                    End If
                                End If
                            End If
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True

                        ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                          Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" _
                          Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REN" _
                          Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REP" Then
                            'if the plocy has been cancelled then only one link i.e VIEW

                            If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False Then
                                    If UserCanDoTask("MidTermReinstatement") Then
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_Reinstatement").ToString() '"details"
                                        lnkbtnSelect.CommandName = "Reinstatement"
                                    End If
                                Else
                                    lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                    lnkbtnSelect2.CommandName = "viewpolicy"
                                    lnkbtnSelect2.Visible = True
                                End If
                                e.Row.Cells(3).Text = GetLocalResourceObject("lbl_Lapsed")
                            ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "REN" Then
                                If IsInRenewal(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = True Then
                                    If UserCanDoTask("MidTermAdjustment") Or UserCanDoTask("MidTermReinstatement") Or UserCanDoTask("MidTermCancellation") Then
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_MTAchange").ToString() '"edit"
                                        lnkbtnSelect.CommandName = "MTAquote"
                                        'This code is added for unmarking the quote for collection
                                        If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                        End If
                                        If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                                        End If
                                        'end
                                    End If

                                    lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                    lnkbtnSelect2.CommandName = "viewpolicy"
                                    lnkbtnSelect2.Visible = True
                                End If
                            Else
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect2.CommandName = "viewpolicy"
                                lnkbtnSelect2.Visible = True
                            End If
                        Else
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "RENEWAL"
                        bShowVoidButton = ShowHideVoidButton(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)
                        If bShowVoidButton AndAlso lnkbtnVoid IsNot Nothing Then
                            lnkbtnVoid.Visible = True
                            lnkbtnVoid.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnVoid.Attributes.Add("OnClick", "VoidPolicyVersionConfirmation();")
                        End If
                        'need to show only one link i.e. "Details"
                        'Check the roles before displaying the "Details" link
                        If UserCanDoTask("Renewals") Then
                            If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect2.CommandName = "viewpolicy"
                                lnkbtnSelect2.Visible = True
                                e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Lapsed")
                            Else
                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_details").ToString() '"details"
                                lnkbtnSelect.CommandName = "viewDetails"
                                lbl_Status.Text = GetLocalResourceObject("lbl_RenewalQuote").ToString() '"RENEWAL QUOTE"
                            End If
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                        If UserCanDoTask("ViewQuote") Then
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        End If
                    Case "MTACAN"
                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LAP" Then

                            'Now the Reinstatement button will only be shown if user has access to MTR/MTC
                            If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False _
                            And IsInRenewal(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = False Then
                                'Fix for 3509
                                If hstMTACanVerion.Item(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) Is Nothing Then
                                    If UserCanDoTask("MidTermReinstatement") Then
                                        hstMTACanVerion.Add(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, "1")
                                        lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                        lnkbtnSelect.Text = GetLocalResourceObject("lbl_Reinstatement").ToString() '"details"
                                        lnkbtnSelect.CommandName = "Reinstatement"
                                    End If
                                End If
                            End If
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"
                        Else
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "VOID", "VOIDREP", "VOIDRENREP"
                        lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                        lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                        lnkbtnSelect2.CommandName = "viewpolicy"
                        lnkbtnSelect2.Visible = True
                    Case Else

                        lnkbtnSelect.Visible = False

                End Select

                ''Not required now since the quote status is shown with a different approach as per WPR63
                ''substitute status for something more meaninful (store as a resource)
                'Select Case UCase(e.Row.Cells(1).Text).Trim
                '    Case "QUOTE"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("QUOTE")
                '    Case "WRITTEN"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("WRITTEN")
                '    Case "POLICY"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("POLICY")
                '    Case "MTACAN"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTACAN")
                '    Case "MTAREINS"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTAREINS")
                '    Case "MTA TEMP"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTA TEMP")
                '    Case "MTA PERM"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTA PERM")
                '    Case "MTAQREINS"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTAQREINS")
                '    Case "MTAQUOTE"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTAQUOTE")
                '    Case "MTAQTETEMP"
                '        e.Row.Cells(1).Text = GetLocalResourceObject("MTAQTETEMP")
                'End Select

                If CType(e.Row.DataItem, NexusProvider.Policy).OpenPolicyClaims = 1 Then
                    e.Row.RowState = DataControlRowState.Normal
                    e.Row.CssClass = "AspNet-GridView-OpenClaim"
                ElseIf CType(e.Row.DataItem, NexusProvider.Policy).ClosePolicyClaims = 1 Then
                    e.Row.RowState = DataControlRowState.Normal
                    e.Row.CssClass = "AspNet-GridView-CloseClaim"
                End If
                If (dExpiryDate < DateTime.Now) Then
                    'Set Expiry Date Message if the Policy Expire
                    If CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "QUOTE" Then
                        lnkbtnSelect2.Attributes.Remove("OnClick")
                        If (lnkbtnSelect2.CommandName = "buyquote") Then
                            lnkbtnSelect2.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                        End If
                    ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "MTAQUOTE" Then
                        lnkbtnSelect2.Attributes.Remove("OnClick")
                        lnkbtnSelect.Attributes.Remove("OnClick")

                        If (lnkbtnSelect2.CommandName = "buymtaquote") Then
                            lnkbtnSelect2.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                        End If

                        If (lnkbtnSelect.CommandName = "editmtaquote") Then
                            lnkbtnSelect.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                        End If
                    End If
                End If

                If Not CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                    'Set Expiry Date Message if the Policy Expire
                    lnkbtnSelect.Attributes.Remove("OnClick")
                    If (dExpiryDate < DateTime.Now) Then
                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "QUOTE" Then
                            lnkbtnSelect2.Attributes.Remove("OnClick")
                            If (lnkbtnSelect2.CommandName = "buyquote") Then
                                lnkbtnSelect2.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                            End If
                        ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyTypeCode.Trim.ToUpper = "MTAQUOTE" Then
                            lnkbtnSelect2.Attributes.Remove("OnClick")
                            lnkbtnSelect.Attributes.Remove("OnClick")

                            If (lnkbtnSelect2.CommandName = "buymtaquote") Then
                                lnkbtnSelect2.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                            End If

                            If (lnkbtnSelect.CommandName = "editmtaquote") Then
                                lnkbtnSelect.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                            End If
                        End If
                    End If
                End If

            End If
        End Sub
        ''' <summary>
        ''' Will return the quote grace period for the product whose ProductCode is passed
        ''' </summary>
        ''' <param name="sProductCode"></param>
        ''' <remarks></remarks>
        Protected Function GetQuoteGracePeriod(ByVal sProductCode As String) As String
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
            Dim sProductPath() As String
            sProductPath = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                       .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            Dim oProduct As Config.Product = CType(GetSection("NexusFrameWork"),
             Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products.GetProductByName(Server.UrlDecode(
             Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))
            Dim iGracePeriod As String = ""
            iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, sProductCode, "")
            Return iGracePeriod
        End Function

        Protected Sub grdvSubBroker_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lnkbtnDetails As LinkButton = e.Row.FindControl("lnkbtnDetails")
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If (oUserDetails IsNot Nothing AndAlso oUserDetails.Key <> 0) Then
                    If (lnkbtnDetails IsNot Nothing) Then
                        If (CType(e.Row.DataItem, NexusProvider.Policy).QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending) Then
                            lnkbtnDetails.Visible = False
                        End If

                    End If
                End If
                'Dim iGracePeriod As Integer
                'Dim dExpiryDate As Date
                'iGracePeriod = GetQuoteGracePeriod(CType(e.Row.DataItem, NexusProvider.Policy).ProductCode.Trim())
                'dExpiryDate = CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate.AddDays(iGracePeriod).ToShortDateString()
                'If (dExpiryDate < DateTime.Now) Then
                '    lnkbtnDetails.Enabled = False
                'End If
            End If
            'If lbl_ChildStatus IsNot Nothing Then

            'Dim Grid As GridView = Me.FindControl("ctl00$cntMainBody$grdvBroker")
            'If (Grid IsNot Nothing) Then
            '    Dim grid1 As GridView = Grid.NamingContainer.FindControl("grdvSubBroker")
            '    If (grid1 IsNot Nothing) Then
            '        Dim label As Label = grid1.NamingContainer.FindControl("lbl_ChildStatus")
            '    End If
            'End If

            'Dim lbl1 As Label = Me.FindControl("ctl00$cntMainBody$grdvBroker$ctl08$grdvSubBroker$ctl03$lbl_ChildStatus")

            'If lbl1 IsNot Nothing Then

            'End If
        End Sub


        Sub PanelViewAllPolicies(ByVal bStatus As Boolean)
            chkViewAllPolicies.Visible = bStatus
            lbl_ViewAllPolicies.Visible = bStatus
        End Sub

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnReInstatementConfirmation",
                      "<script language=""JavaScript"" type=""text/javascript"">function UnReInstatementConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmReInstatement").ToString() & "');return IsConfirm;}</script>")
        End Sub

        ''' <summary>
        ''' Fill Products assigned for an agent as per WPR 14 - Agency Management Logic
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        '''                
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
                'get risk type from session
                oRiskType = Session(CNRiskType)
                'redirect to first risk screen for the current risk type
                If CType(Session(CNQuote), NexusProvider.Quote) IsNot Nothing AndAlso CType(Session(CNQuote), NexusProvider.Quote).Risks.Count = 0 Then
                    AddRiskAndRedirect()
                End If
            End If


            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            sTxtAgentName = txtAgentName.Text
            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()

                'Initialize void button and hidden fields
                If hvVoidConfirm IsNot Nothing Then
                    hvVoidConfirm.Value = "False"
                End If
                Dim sAllowedAgent() As String
                Dim iCounter As Integer = 0
                Dim bMatched As Boolean = False
                Dim UserRoles As String
                Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                Dim oWebService As NexusProvider.ProviderBase
                Dim oAgentProducts As New NexusProvider.ProductCollection
                Dim oAgentToProductLinkOptionSetting As NexusProvider.OptionTypeSetting
                HttpContext.Current.Session(CNAnonymous) = Nothing

                oWebService = New NexusProvider.ProviderManager().Provider

                'Get System Option value for AgentToProductLink
                oAgentToProductLinkOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5088)
                'If System option is ON then GET All products for logged in agent
                If oAgentToProductLinkOptionSetting.OptionValue = "1" And oUserDetails.Key > 0 Then
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oAgentProducts = oWebService.GetProductByAgent()
                End If

                For Each oProduct As Config.Product In oProducts
                    'Retreive all the roles set for product in web.config
                    UserRoles = oProduct.AllowRole
                    'Roles is available
                    If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True Then
                        'if logged user is agent
                        If CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0 Then
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
                        ddlProductType.Items.Add(New ListItem(oProduct.Name, oProduct.ProductCode))
                    End If
                    bMatched = False
                Next

                If Session(CNRiskViewStartPoint) IsNot Nothing AndAlso Session(CNRiskViewStartPoint) = "ClientManager" Then
                    Session.Remove(CNRiskViewStartPoint)
                End If
            End If

            'If an underwriter (non-agency user) is logged, Result Type Dropdown field will not display
            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then
                li_logintype.Visible = False
            End If
            'check if the postback has been triggered by the modal dialog
            If Request("__EVENTARGUMENT") = "Complete" Then
                CompletePolicy()
            End If
            If Request("__EVENTARGUMENT") = "Delete" Then
                DeletePolicy()
            End If



        End Sub
        Private Sub CompletePolicy()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sRedirectPath As String = String.Empty
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

            'Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            'Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            'Dim sRiskFolder As String = sProductFolder & "\" & oProduct.RiskTypes.RiskType(0).Path & "/"
            'sRedirectPath = sProductFolder & oProduct.RiskTypes.RiskType(0).Path & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig)

            'If (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending And CType(Session(CNLoginType), LoginType) <> LoginType.Agent) Then
            '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation", _
            '            "<script language=""JavaScript"" type=""text/javascript"">if (confirm('The agent will not be able to view this quote version untill you Issue it. Are you sure you wish to continue?') == true ) return true; else return false; </script>")
            '    oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Pending
            '    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))


            '    Response.Redirect(sRedirectPath, False)
            'ElseIf (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Issued And CType(Session(CNLoginType), LoginType) <> LoginType.Agent) Then
            '    Session(CNMode) = Mode.View
            '    Response.Redirect(sRedirectPath, False)
            'ElseIf (CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0) Then
            '    Session(CNMode) = Mode.View
            '    Response.Redirect(sRedirectPath, False)
            'Else
            '    Response.Redirect(sRedirectPath, False)
            'End If
            DoQuoteConfirmation(oQuote.InsuranceFileKey, True)
        End Sub

        Private Sub DeletePolicy()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As New NexusProvider.Quote
            oQuote.InsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
            oWebService.DeletePolicy(oQuote)
            GetbrokerSummary()
        End Sub


        Private Sub ResetTransactionInSession()
            Session.Remove(CNMTAType)
            Session.Remove(CNMTATypeDesc)
            Session.Remove(CNRenewal)
            Session.Remove(CNRenewalShowPremium)
        End Sub

        Private Function IsPolicyCancelled(ByVal PolicyRef As String, ByVal PolicyType As String, ByVal PolicyStatus As String, ByVal ncollectioncount As Integer) As Boolean
            'Policy Collection has any PolicyStatusCode="CAN" against the passed Policy 
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
            Dim bStatus As Boolean = False

            'if Any Policy version has been Cancelled
            If (PolicyType = "MTAQUOTE" OrElse PolicyType = "MTAQTETEMP") AndAlso PolicyStatus = "CAN" Then
                'Flag set as TRUE without Check all Policy Version
                bStatus = True
            Else
                If oPolicyCollection(ncollectioncount).PolicyStatusCode IsNot Nothing Then
                    If oPolicyCollection(ncollectioncount).Reference.Trim = PolicyRef.Trim _
                        AndAlso (oPolicyCollection(ncollectioncount).PolicyStatusCode.Trim = "CAN" OrElse oPolicyCollection(ncollectioncount).PolicyStatusCode.Trim = "LAP") AndAlso
                        (Not (oPolicyCollection(ncollectioncount).PolicyStatusCode.Trim = "CAN" AndAlso oPolicyCollection(ncollectioncount).InsuranceFileTypeCode.Trim = "MTAQUOTE" OrElse oPolicyCollection(ncollectioncount).InsuranceFileTypeCode.Trim = "MTAQTETEMP")) Then
                        bStatus = True
                        'Yes Policy Has been Cancelled/Lapsed
                        'Check whether it has been reinstated or not
                        If IsReinstated(oPolicyCollection(ncollectioncount).Reference.Trim) = True OrElse
                        IsRenewed(oPolicyCollection(ncollectioncount).Reference.Trim, oPolicyCollection(ncollectioncount).CoverStartDate, oPolicyCollection(ncollectioncount).InsuranceFileKey) = True OrElse IsInRenewal(oPolicyCollection(ncollectioncount).Reference.Trim) = True Then
                            bStatus = False
                        End If
                    End If
                End If
            End If

            Return bStatus
        End Function


        Private Function IsReinstated(ByVal PolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = PolicyRef.Trim _
                    And oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "MTAREINS" Then
                        bStatus = True
                        'Yes Policy Has been Cancelled/Lapsed
                        'Check whether it has been reinstated or not
                    End If
                End If
            Next
            Return bStatus
        End Function

        Private Function IsRenewed(ByVal PolicyRef As String, ByVal CoverStartDate As Date, ByVal iInsuranceFileKey As Integer) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = PolicyRef.Trim And oPolicyCollection(TempVar).CoverStartDate > CoverStartDate _
                And (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Or
                oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "MTAREINS") _
                And oPolicyCollection(TempVar).InsuranceFileKey <> iInsuranceFileKey _
                And oPolicyCollection(TempVar).PolicyStatusCode.Trim.ToUpper <> "CAN" Then
                        bStatus = True
                        'Yes Policy Has been Renewed
                    End If
                End If
            Next
            Return bStatus
        End Function

        Private Function IsInRenewal(ByVal PolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
            Dim bStatus As Boolean = False
            Dim TempVar As Integer
            For TempVar = 0 To oPolicyCollection.Count - 1
                If oPolicyCollection(TempVar).InsuranceFileTypeCode IsNot Nothing Then
                    If oPolicyCollection(TempVar).Reference.Trim = PolicyRef.Trim _
                    And oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim.ToUpper = "RENEWAL" Then
                        bStatus = True
                        'Yes Policy Has been Renewed
                    End If
                End If
            Next
            Return bStatus
        End Function

        Function CheckValidProduct(ByVal v_sProductCode As String) As Boolean
            'Check the product where it is configurent in Nexus or not
            Dim bReturn As Boolean = False
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            For Each oProduct As Config.Product In oProducts
                If v_sProductCode.Trim.ToUpper = oProduct.ProductCode.Trim.ToUpper Then
                    bReturn = True
                    Exit For
                End If
            Next
            Return bReturn
        End Function

        Protected Sub chkViewAllPolicies_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkViewAllPolicies.CheckedChanged
            GetbrokerSummary()
        End Sub

        'This fucntion is grouped to filter the records 
        Sub FilterRecords()
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(Portal.GetPortalID()).Products
            Dim oPolCol As New NexusProvider.PolicyCollection
            Dim oPolicy As NexusProvider.Policy
            Dim TempVar As Integer
            Dim htReinstat As New Hashtable

            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)

            pnl_grdvBroker.Visible = True 'make the panel containing the search results visible regardless

            If oPortalConfig.ViewOnlyLatestPolicyVersion = True Then
                'Checking of the valid product and build the collection again
                For TempVar = 0 To oPolicyCollection.Count - 1
                    If CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                        oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                        oPolicy = oPolicyCollection(TempVar)

                        If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)
                    End If
                Next

                'store the data in ViewState to use again for page indexing
                ViewState.Add(CNBrokerCollection, oPolCol)

                'get max insurancefilekey of a specific insurance_folder collection
                For nCount_opol = 0 To oPolCol.Count - 1
                    If Not htReinstat.Contains(oPolCol(nCount_opol).InsuranceFolderKey) Then
                        htReinstat.Add(oPolCol(nCount_opol).InsuranceFolderKey, oPolCol(nCount_opol).InsuranceFileKey)
                    Else
                        If (htReinstat(oPolCol(nCount_opol).InsuranceFolderKey) < oPolCol(nCount_opol).InsuranceFileKey) Then
                            htReinstat(oPolCol(nCount_opol)) = oPolCol(nCount_opol).InsuranceFileKey
                        End If
                    End If
                Next
                ViewState("htReinstat") = htReinstat


                grdvBroker.DataSource = oPolCol
                If grdvBroker.PageCount <= 1 Then
                    grdvBroker.AllowPaging = False
                Else
                    grdvBroker.AllowPaging = True
                End If
                grdvBroker.DataBind()
            Else
                If (Me.chkViewAllPolicies.Checked) Then

                    For TempVar = 0 To oPolicyCollection.Count - 1
                        If CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                            oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                            oPolicy = oPolicyCollection(TempVar)
                            If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)
                        End If
                    Next

                    'store the data in ViewState to use again for page indexing
                    ViewState.Add(CNBrokerCollection, oPolCol)

                    'get max insurancefilekey of a specific insurance_folder collection

                    For nCount_opol = 0 To oPolCol.Count - 1
                        If Not htReinstat.Contains(oPolCol(nCount_opol).InsuranceFolderKey) Then
                            htReinstat.Add(oPolCol(nCount_opol).InsuranceFolderKey, oPolCol(nCount_opol).InsuranceFileKey)
                        Else
                            If (htReinstat(oPolCol(nCount_opol).InsuranceFolderKey) < oPolCol(nCount_opol).InsuranceFileKey) Then
                                htReinstat(oPolCol(nCount_opol)) = oPolCol(nCount_opol).InsuranceFileKey
                            End If
                        End If
                    Next
                    ViewState("htReinstat") = htReinstat


                    grdvBroker.DataSource = oPolCol
                    If grdvBroker.PageCount <= 1 Then
                        grdvBroker.AllowPaging = False
                    Else
                        grdvBroker.AllowPaging = True
                    End If
                    grdvBroker.DataBind()
                Else
                    For TempVar = 0 To oPolicyCollection.Count - 1
                        If (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "POLICY" _
                          Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" _
                          Or (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTAQUOTE" Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTAQCAN" _
                          And IsRenewed(oPolicyCollection(TempVar).Reference.Trim, oPolicyCollection(TempVar).CoverStartDate, oPolicyCollection(TempVar).InsuranceFileKey) = False) _
                          Or (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTAQTETEMP" _
                          And IsRenewed(oPolicyCollection(TempVar).Reference.Trim, oPolicyCollection(TempVar).CoverStartDate, oPolicyCollection(TempVar).InsuranceFileKey) = False) _
                          Or (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTAQREINS" _
                          And IsRenewed(oPolicyCollection(TempVar).Reference.Trim, oPolicyCollection(TempVar).CoverStartDate, oPolicyCollection(TempVar).InsuranceFileKey) = False) _
                          Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "RENEWAL" _
                          Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTA TEMP" _
                          Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS") Then

                            If IsPolicyCancelled(oPolicyCollection(TempVar).Reference, oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim, oPolicyCollection(TempVar).PolicyStatusCode.Trim, TempVar) = False _
                             AndAlso CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                                oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                                oPolicy = oPolicyCollection(TempVar)

                                If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)
                            End If

                        ElseIf oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "QUOTE" _
                        Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "WRITTEN" _
                         AndAlso CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                            oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                            oPolicy = oPolicyCollection(TempVar)
                            If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)
                        End If
                    Next
                    'store the data in ViewState to use again for page indexing
                    ViewState.Add(CNBrokerCollection, oPolCol)
                    If CNSortExpression <> "" Then
                        oPolCol.SortColumn = CNSortExpression
                        oPolCol.SortingOrder = CNSortDirection
                        oPolCol.Sort()
                    End If

                    'get max insurancefilekey of a specific insurance_folder collection
                    For nCount_opol = 0 To oPolCol.Count - 1
                        If Not htReinstat.Contains(oPolCol(nCount_opol).InsuranceFolderKey) Then
                            htReinstat.Add(oPolCol(nCount_opol).InsuranceFolderKey, oPolCol(nCount_opol).InsuranceFileKey)
                        Else
                            If (htReinstat(oPolCol(nCount_opol).InsuranceFolderKey) < oPolCol(nCount_opol).InsuranceFileKey) Then
                                htReinstat(oPolCol(nCount_opol)) = oPolCol(nCount_opol).InsuranceFileKey
                            End If
                        End If
                    Next
                    ViewState("htReinstat") = htReinstat

                    grdvBroker.DataSource = oPolCol
                    grdvBroker.DataBind()
                    If grdvBroker.PageCount <= 1 Then
                        grdvBroker.AllowPaging = False
                    Else
                        grdvBroker.AllowPaging = True
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' check the status of the policy is in the array of statuses being shown
        ''' </summary>
        ''' <param name="sPolicyStatus"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckStatus(ByVal sPolicyStatus As String) As Boolean
            If sDisplayStatus Is Nothing Then
                'no filtering applied, so just return true
                Return True
            Else
                If CType(sDisplayStatus, IList).Contains(Trim(sPolicyStatus)) Or sDisplayStatus.Length = 0 Then
                    'status is contained in filter list
                    iQuoteCount = iQuoteCount + 1
                    Return True
                End If
            End If
            'current status is not in filter list
            Return False
        End Function

        Protected Sub grdvBroker_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvBroker.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerCollection)
            oPolicyCollection.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oPolicyCollection.SortingOrder = _sortDirection
            oPolicyCollection.Sort()
            CType(sender, GridView).DataSource = oPolicyCollection
            CType(sender, GridView).DataBind()
            CNSortDirection = _sortDirection
            CNSortExpression = e.SortExpression
        End Sub

        Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
            'added following lines to fix issue no 1967
            txtAgentName.Text = hvAgentName.Value
            If txtAgentName.Text <> Nothing Then
                txtAgentKey.Value = hvAgentKey.Value
            End If
            GetbrokerSummary()
            'Initialise this to false
            PanelViewAllPolicies(False)
            If UserCanDoTask("ViewOldPolicies") Then
                PanelViewAllPolicies(True)
            Else
                PanelViewAllPolicies(False)
            End If
        End Sub

        Protected Sub grdvSubBroker_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
            If (e.CommandName.Equals("Details")) Then
                DoQuoteConfirmation(e.CommandArgument, False)
            End If
        End Sub
        ''' <summary>
        ''' Perform QuoteConfirmation Task on the basis of Insurance File Key Provided
        ''' </summary>
        ''' <param name="iInsuranceFileKey"></param>
        ''' <param name="Redirect"></param>
        ''' <remarks></remarks>
        Protected Sub DoQuoteConfirmation(ByVal iInsuranceFileKey As Integer, ByVal Redirect As Boolean)
            ClearQuoteCollectionSessionValues()
            ClearQuote()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote

            Session.Remove(CNOldPremium) 'Remove the old premium from session
            Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            'Copy Quote needs to be handled separately first to aviod unnecessary SAM calls



            'Dim sBaseInsuranceFolderKey As String
            ''sBaseInsuranceFolderKey = Convert.ToString(e.CommandArgument)

            'Dim sInsuranceFileKey As String
            'sInsuranceFileKey = Convert.ToString(iInsuranceFileKey)

            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNBrokerChildCollection)

            'Dim oPolicyTemp = (From ps In oPolicyCollection Where ps.InsuranceFileKey = sInsuranceFileKey).ToList()
            'sBaseInsuranceFolderKey = (From ps In oPolicyCollection Where ps.InsuranceFileKey = sInsuranceFileKey Select ps.BaseInsuranceFolderKey).SingleOrDefault()

            'Dim tempPolicy As NexusProvider.Policy = DirectCast(oPolicyTemp, NexusProvider.Policy)

            Dim oPolicy = (From ps In oPolicyCollection Where ps.InsuranceFileKey = iInsuranceFileKey).SingleOrDefault()
            Dim sRedirectPath As String = String.Empty
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim policy As NexusProvider.Policy
            policy = DirectCast(oPolicy, NexusProvider.Policy)
            If (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending Or policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending) Then

                oQuote = FillSessionValues(policy.InsuranceFileKey)

                Dim oRiskT As New NexusProvider.RiskType
                'if Risk is UNQUOTED then Buy Now should throw a message


                For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks IsNot Nothing Then
                        If (Redirect = False) Then
                            If (oQuote.Risks(iTempVar).IsRisk = True AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED" AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED") Then
                                Dim sURL As String
                                If HttpContext.Current.Session.IsCookieless Then
                                    sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/QuoteConfirmation.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                                Else
                                    sURL = AppSettings("WebRoot") & "/Modal/QuoteConfirmation.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                                End If

                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                                Exit Sub
                            End If
                        ElseIf (Redirect = True) Then

                            If (oQuote.Risks(iTempVar).IsRisk = True AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED") Then
                                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
                                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
                                Dim sRiskFolder As String = sProductFolder & "\" & oProduct.RiskTypes.RiskType(0).Path & "/"
                                sRedirectPath = sProductFolder & oProduct.RiskTypes.RiskType(0).Path & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig)

                                If (CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0) Then
                                    Session(CNMode) = Mode.View
                                    Response.Redirect(sRedirectPath, False)
                                Else
                                    Response.Redirect(sRedirectPath, False)
                                End If
                                Exit Sub
                            ElseIf (oQuote.Risks(iTempVar).IsRisk = True AndAlso (oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "QUOTED" Or oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "REFERRED")) Then
                                sRedirectPath = String.Empty
                                DataSetFunctions.GetScreens()
                                If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                                    sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                                Else
                                    sRedirectPath = "~/secure/PremiumDisplay.aspx"
                                End If
                                Response.Redirect(sRedirectPath, False)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If

            oQuote = FillSessionValues(policy.InsuranceFileKey)
            sRedirectPath = String.Empty
            DataSetFunctions.GetScreens()
            If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
            Else
                sRedirectPath = "~/secure/PremiumDisplay.aspx"
            End If
            Response.Redirect(sRedirectPath, False)
        End Sub

        ''' <summary>
        ''' Fill SessionValues 
        ''' </summary>
        ''' <param name="iInsuranceFileKey"></param>
        ''' <remarks></remarks>
        Protected Function FillSessionValues(ByVal iInsuranceFileKey As Integer) As NexusProvider.Quote
            Dim oQuote As NexusProvider.Quote
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iCurrentRiskKey As Integer
            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                'Put Party information into CNParty Session
                Dim oFindParty As NexusProvider.BaseParty
                oFindParty = oWebService.GetParty(oQuote.PartyKey)
                Session(CNParty) = oFindParty

                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                    iCurrentRiskKey = oQuote.Risks(i).Key
                Next

                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote


            Finally
                'oWebService = Nothing
            End Try
            Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE


            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            GetDataSetDefinition()

            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Session(CNMTAType) = Nothing

            Session(CNRenewal) = Nothing
            Session(CNMode) = Mode.Edit
            Session(CNQuoteInSync) = False
            Session.Remove(CNOI)
            Session(CNInsuranceFileKey) = iInsuranceFileKey
            Session(CNQuoteInSync) = False


            If IsDataSetQuickQuote() = False Then
                Session(CNQuoteMode) = QuoteMode.FullQuote
            Else
                Session(CNQuoteMode) = QuoteMode.QuickQuote
            End If
            Return oQuote
        End Function

        Sub GetbrokerSummary()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                        .Portals.Portal(Portal.GetPortalID()).Products
                Dim lblResults As Integer
                Dim oUserDetails As NexusProvider.UserDetails = Session.Item(CNAgentDetails)
                If oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 And ddlResults.SelectedValue = "All" Then
                    lblResults = 0
                ElseIf oUserDetails IsNot Nothing AndAlso oUserDetails.Key > 0 And ddlResults.SelectedValue = "UserOnly" Then
                    lblResults = 1
                Else
                    lblResults = -1
                End If
                Dim dStartDate As Date
                If Not String.IsNullOrEmpty(txtStartDate.Text) Then
                    dStartDate = CDate(txtStartDate.Text)
                End If
                Dim dQuoteDate As Date
                If Not String.IsNullOrEmpty(txtQuoteDate.Text) Then
                    dQuoteDate = CDate(txtQuoteDate.Text)
                End If

                'Added following if condition to fix issue 1761
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key = 0 Then
                        'set logic for Underwriter
                        txtAgentKey.Value = hvAgentKey.Value
                    Else
                        'Added following line to fix issue 1696
                        txtAgentKey.Value = oUserDetails.Key
                    End If
                End If

                If Not String.IsNullOrEmpty(txtRiskIndex.Text) Then
                    grdvBroker.Columns(8).Visible = True
                    grdvBroker.Columns(9).Visible = True
                Else
                    grdvBroker.Columns(8).Visible = False
                    grdvBroker.Columns(9).Visible = False
                End If

                If ddlRecordType.SelectedValue = "All" Then
                    oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.ALL, ddlProductType.SelectedValue, lblResults, txtPolicyNumber.Text.Trim(), txtName.Text.Trim(), oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, IIf(txtAgentKey.Value = "", 0, txtAgentKey.Value), txtRiskIndex.Text)
                ElseIf ddlRecordType.SelectedValue = "Policy" Then
                    oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.POLICY, ddlProductType.SelectedValue, lblResults, txtPolicyNumber.Text.Trim(), txtName.Text.Trim(), oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, IIf(txtAgentKey.Value = "", 0, txtAgentKey.Value), txtRiskIndex.Text)
                Else
                    oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.QUOTE, ddlProductType.SelectedValue, lblResults, txtPolicyNumber.Text.Trim(), txtName.Text.Trim(), oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, IIf(txtAgentKey.Value = "", 0, txtAgentKey.Value), txtRiskIndex.Text)
                End If

                ViewState.Add(CNBrokerChildCollection, oPartySummary.Policies)

                Dim oPolicies As New NexusProvider.PolicyCollection
                Dim opolicy1 As NexusProvider.Policy
                Dim iBaseKey As Integer = 0
                Dim iCount As Integer
                Dim iAnonPartyID As Integer = CType(oPortal.AnnPartyID, Integer)

                'Find out the quote versions and add them to a single collection
                If oPartySummary.Policies IsNot Nothing Then
                    For iCount = 0 To oPartySummary.Policies.Count - 1
                        If iBaseKey <> oPartySummary.Policies(iCount).BaseInsuranceFolderKey And oPartySummary.Policies(iCount).BaseInsuranceFolderKey <> 0 And oPartySummary.Policies(iCount).InsuranceFileTypeCode.Trim.ToUpper = "QUOTE" And oPartySummary.Policies(iCount).PartyKey <> iAnonPartyID Then
                            iBaseKey = oPartySummary.Policies(iCount).BaseInsuranceFolderKey
                            opolicy1 = New NexusProvider.Policy(oPartySummary.Policies(iCount).InsuranceFileKey)
                            With opolicy1
                                .AccHandler = oPartySummary.Policies(iCount).AccHandler
                                .AssociatedClients = oPartySummary.Policies(iCount).AssociatedClients
                                .AgentCode = oPartySummary.Policies(iCount).AgentCode
                                .InsuranceFileKey = oPartySummary.Policies(iCount).InsuranceFileKey
                                .Reference = oPartySummary.Policies(iCount).Reference
                                .InsuranceFolderKey = oPartySummary.Policies(iCount).InsuranceFolderKey
                                .DateIssued = oPartySummary.Policies(iCount).DateIssued
                                .CoverStartDate = oPartySummary.Policies(iCount).CoverStartDate
                                .ExpiryDate = oPartySummary.Policies(iCount).ExpiryDate
                                .PartyKey = oPartySummary.Policies(iCount).PartyKey
                                .ProductCode = oPartySummary.Policies(iCount).ProductCode
                                .ProductDescription = oPartySummary.Policies(iCount).ProductDescription
                                .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                .ClientCode = oPartySummary.Policies(iCount).ClientCode
                                .PolicyStatusCode = oPartySummary.Policies(iCount).PolicyStatusCode
                                .PolicyTypeCode = oPartySummary.Policies(iCount).PolicyTypeCode
                                .PolicyTypeDescription = oPartySummary.Policies(iCount).PolicyTypeDescription
                                .PolicyStatus = oPartySummary.Policies(iCount).PolicyStatus
                                .IsCurrent = oPartySummary.Policies(iCount).IsCurrent
                                .ClientName = oPartySummary.Policies(iCount).ClientName
                                .BaseInsuranceFolderKey = oPartySummary.Policies(iCount).BaseInsuranceFolderKey
                                .QuoteVersion = oPartySummary.Policies(iCount).QuoteVersion
                                .QuoteStatusKey = oPartySummary.Policies(iCount).QuoteStatusKey
                                .LeadAgentKey = oPartySummary.Policies(iCount).LeadAgentKey
                                .LeadAgent = oPartySummary.Policies(iCount).LeadAgent
                                .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                .QuoteExpiryDate = oPartySummary.Policies(iCount).QuoteExpiryDate
                                .RenewedVersion = oPartySummary.Policies(iCount).RenewedVersion
                                .RiskStatus = oPartySummary.Policies(iCount).RiskStatus
                                .IsMarketPlacePolicy = oPartySummary.Policies(iCount).IsMarketPlacePolicy
                                .BaseInsuranceFileKey = oPartySummary.Policies(iCount).BaseInsuranceFileKey
                                .IsReinstateLinkVersion = oPartySummary.Policies(iCount).IsReinstateLinkVersion
                                .RiskNumber = oPartySummary.Policies(iCount).RiskNumber
                                .RiskDescription = oPartySummary.Policies(iCount).RiskDescription
                            End With
                            oPolicies.Add(opolicy1)
                        ElseIf oPartySummary.Policies(iCount).InsuranceFileTypeCode.Trim.ToUpper <> "QUOTE" And oPartySummary.Policies(iCount).PartyKey <> iAnonPartyID Then
                            opolicy1 = New NexusProvider.Policy(oPartySummary.Policies(iCount).InsuranceFileKey)
                            With opolicy1
                                .AccHandler = oPartySummary.Policies(iCount).AccHandler
                                .AssociatedClients = oPartySummary.Policies(iCount).AssociatedClients
                                .AgentCode = oPartySummary.Policies(iCount).AgentCode
                                .InsuranceFileKey = oPartySummary.Policies(iCount).InsuranceFileKey
                                .Reference = oPartySummary.Policies(iCount).Reference
                                .InsuranceFolderKey = oPartySummary.Policies(iCount).InsuranceFolderKey
                                .DateIssued = oPartySummary.Policies(iCount).DateIssued
                                .CoverStartDate = oPartySummary.Policies(iCount).CoverStartDate
                                .ExpiryDate = oPartySummary.Policies(iCount).ExpiryDate
                                .PartyKey = oPartySummary.Policies(iCount).PartyKey
                                .ProductCode = oPartySummary.Policies(iCount).ProductCode
                                .ProductDescription = oPartySummary.Policies(iCount).ProductDescription
                                .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                .ClientCode = oPartySummary.Policies(iCount).ClientCode
                                .PolicyStatusCode = oPartySummary.Policies(iCount).PolicyStatusCode
                                .PolicyTypeCode = oPartySummary.Policies(iCount).PolicyTypeCode
                                .PolicyTypeDescription = oPartySummary.Policies(iCount).PolicyTypeDescription
                                .PolicyStatus = oPartySummary.Policies(iCount).PolicyStatus
                                .IsCurrent = oPartySummary.Policies(iCount).IsCurrent
                                .ClientName = oPartySummary.Policies(iCount).ClientName
                                .BaseInsuranceFolderKey = oPartySummary.Policies(iCount).BaseInsuranceFolderKey
                                .QuoteVersion = oPartySummary.Policies(iCount).QuoteVersion
                                .QuoteStatusKey = oPartySummary.Policies(iCount).QuoteStatusKey
                                .LeadAgentKey = oPartySummary.Policies(iCount).LeadAgentKey
                                .LeadAgent = oPartySummary.Policies(iCount).LeadAgent
                                .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                .QuoteExpiryDate = oPartySummary.Policies(iCount).QuoteExpiryDate
                                .RenewedVersion = oPartySummary.Policies(iCount).RenewedVersion
                                .RiskStatus = oPartySummary.Policies(iCount).RiskStatus
                                .IsMarketPlacePolicy = oPartySummary.Policies(iCount).IsMarketPlacePolicy
                                .BaseInsuranceFileKey = oPartySummary.Policies(iCount).BaseInsuranceFileKey
                                .IsReinstateLinkVersion = oPartySummary.Policies(iCount).IsReinstateLinkVersion
                                .RiskNumber = oPartySummary.Policies(iCount).RiskNumber
                                .RiskDescription = oPartySummary.Policies(iCount).RiskDescription
                            End With
                            oPolicies.Add(opolicy1)
                        End If
                    Next
                End If

                oPartySummary.Policies = oPolicies

                Dim oTempPolCol As New NexusProvider.PolicyCollection
                For iCount = 0 To oPartySummary.Policies.Count - 1
                    If oPartySummary.Policies(iCount).PartyKey <> iAnonPartyID Then
                        oTempPolCol.Add(oPartySummary.Policies(iCount))
                    End If
                Next
                oPartySummary.Policies = oTempPolCol

                ViewState.Add(CNBrokerCollection, oPartySummary.Policies)

                'Maintain policy search form data and result set
                Dim sPolicySearch As New NexusProvider.PolicySearchCriteria

                sPolicySearch.RecordType = ddlRecordType.SelectedValue
                sPolicySearch.PolicyNumber = txtPolicyNumber.Text.Trim()
                sPolicySearch.product = ddlProductType.SelectedValue
                sPolicySearch.AgentName = txtAgentName.Text
                sPolicySearch.QuoteDate = dQuoteDate
                sPolicySearch.StartDate = dStartDate
                sPolicySearch.InsuredName = txtName.Text

                Session(CNFindPolicySearchData) = sPolicySearch
                Session(CNPolicyBackButton) = "BrokerView"

                FilterRecords()
            Finally
                oWebService = Nothing
                oPartySummary = Nothing
            End Try
        End Sub

        'Added following if Button  to fix issue 1761
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            Session.Remove(CNPolicyBackButton)
            Session.Remove(CNFindPolicySearchData)
            Response.Redirect("~\secure\BrokerView.aspx", False)
        End Sub

        Protected Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarketPlacePolicyConfirmation",
                        "<script language=""JavaScript"" type=""text/javascript"">function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); if(IsConfirm==true) { document.getElementById('" & hvMarketPlacePolicy.ClientID & "').value = false; } return IsConfirm; } else {return IsConfirm;} }</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "VoidPolicyVersionConfirmation",
                       "<script language=""JavaScript"" type=""text/javascript"">function VoidPolicyVersionConfirmation(){ var sVoidConfirmationMessage = document.getElementById('" & hvVoidMessage.ClientID & "').value; if (confirm(sVoidConfirmationMessage) == true) {document.getElementById('" & hvVoidConfirm.ClientID & "').value=true; return true;} else {document.getElementById('" & hvVoidConfirm.ClientID & "').value=false;} }</script>")


        End Sub

        Private Sub AddRiskAndRedirect()
            'Sub sets session variables and redirects to the correct screen for current risk type
            'This is either called from:
            'a - the add risk button click or
            'b - if there is more than one risk type for this product then called when postback if triggered from modal dialog

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Session(CNMode) = Mode.Add

            Session(CNQuoteInSync) = False
            Session.Remove(CNOI)
            Session(CNQuoteInSync) = False
            Session(CNQuoteMode) = QuoteMode.FullQuote
            If Session(CNRiskType) IsNot Nothing Then
                Dim sRiskType As NexusProvider.RiskType = Session(CNRiskType)

                Dim sRiskFolder As String = sProductFolder & "/" & sRiskType.Path & "/"
                Dim sScreenCode As String = GetScreenCode(sRiskFolder & "/" & oProduct.FullQuoteConfig)

                'set up risk object and add a new risk to the quote
                Dim oRisk As New NexusProvider.Risk(sScreenCode, oRiskType.Name)
                oRisk.DataModelCode = oRiskType.DataModelCode
                oRisk.RiskCode = sRiskType.RiskCode
                oQuote.Risks = New NexusProvider.RiskCollection

                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oQuote.Risks.Remove(i)
                Next
                'oQuote.Risks.Remove(
                oQuote.Risks.Add(oRisk)
                Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
                oWebService.AddRisk(oQuote, 0)
                Session(CNQuote) = oQuote
                Session.Remove(CNPolicyAllTaxesColl)
                'Redirect to correct risk screen
                Response.Redirect(sRiskFolder & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)

            End If
        End Sub

        Private Sub UnlockPolicy(ByVal nInsuranceFileKey As Integer)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty
            Dim bMaintainedSuccess As Boolean = False
            Dim bLogout As Boolean = False
            Dim bAllClear As Boolean = False
            Dim oLock As New NexusProvider.Locks
            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(Session(CNBranchCode).ToString())

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If HttpContext.Current.User.Identity.Name.ToLower().Trim() = oLockItem.LockUserName.ToLower().Trim() AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFileKey Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                    Exit For
                End If
            Next
        End Sub

        Private Function ShowHideVoidButton(ByVal v_lInsuranceFileKey As Integer) As Boolean
            Dim bShowVoidButton As Boolean = False
            Dim bInstalmentExists As Boolean = False
            Dim bQuoteExists As Boolean = False
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sb As New StringBuilder
            Dim sMessage As String = ""
            sb.Append(GetLocalResourceObject("msg_ConfirmVoidPolicyVersion").ToString.Replace("\n", Environment.NewLine))
            If v_lInsuranceFileKey = 0 Then
                v_lInsuranceFileKey = Session(CNInsuranceFileKey)
            End If
            If v_lInsuranceFileKey > 0 Then
                oWebService.IsVoidPolicyVersion(v_lInsuranceFileKey, bShowVoidButton, bInstalmentExists, bQuoteExists)
            End If
            If bShowVoidButton Then
                If bInstalmentExists OrElse bQuoteExists Then
                    sb.Append("WARNING: ")
                End If
                If bInstalmentExists Then
                    sb.Append(GetLocalResourceObject("msg_InsPlanExists").ToString.Replace("\n", Environment.NewLine))
                End If
                If bQuoteExists Then
                    sb.Append(GetLocalResourceObject("msg_QuoteExists").ToString.Replace("\n", Environment.NewLine))
                End If
                hvVoidMessage.Value = sb.ToString()
            End If
            Return bShowVoidButton
        End Function

        Protected Sub SetPolicyVersionVoid(ByVal v_lInsuranceFileKey As Integer, ByVal v_lInsuranceFolderKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim bCreated As Boolean
            Dim sMessage As String
            If hvVoidConfirm.Value.ToUpper = "TRUE" Then
                If v_lInsuranceFileKey > 0 Then
                    ' Make the latest live version void
                    oWebService.CreateVoidPolicyVersion(v_iInsuranceFileKey:=v_lInsuranceFileKey, v_iInsuranceFolderKey:=v_lInsuranceFolderKey,
                                                    r_bIsCreatedVoidPolicyVersion:=bCreated, r_sFailureMessage:=sMessage)
                    If bCreated Then
                        GetbrokerSummary()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CallRenewalConfirmation", "alert('" & GetLocalResourceObject("msg_VoidPolicyFailure").ToString & "');", True)
                    End If

                End If
            Else
                'do nothing 
            End If
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('','text');", True)
        End Sub
    End Class
End Namespace
