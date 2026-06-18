Imports Microsoft.VisualBasic
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext

Namespace Nexus

    Public Module QuotePolicyActions
        ''' <summary>
        ''' To view a quote or policy
        ''' </summary>
        ''' <param name="v_sCommandName"></param>
        ''' <param name="v_nInsuranceFileKey"></param>
        ''' <remarks></remarks>
        Sub ViewQuoteAndPolicy(ByVal v_sCommandName As String, ByVal v_nInsuranceFileKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Current.Session.Remove(CNOldPremium) 'Remove the old premium from session
            Current.Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            'Pure 3.0 ---- WPR 41
            Dim sRedirectPath As String = String.Empty
            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(v_nInsuranceFileKey)
                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, v_bIgnoreLocking:=True)
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Current.Session(CNQuote) = oQuote

                Dim oParty As NexusProvider.BaseParty
                If Current.Session(CNParty) Is Nothing Then
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                Else
                    oParty = CType(Current.Session(CNParty), NexusProvider.BaseParty)
                    If oParty.Key <> oQuote.PartyKey Then
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                    End If
                End If
                Current.Session(CNParty) = oParty

            Catch ex As NexusProvider.NexusException
                'Catch Policy Locking error and show error alert
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(oPage.Page, oPage.GetType(), "policylocked", sMessage, True)
                        Current.Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try
            Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
                DataSetFunctions.GetScreens()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Current.Session(CNMTAType) = Nothing
            Current.Session(CNViewType) = Nothing
            Select Case v_sCommandName
                Case "viewMTA"
                    Current.Session(CNRenewal) = Nothing
                    If oQuote.InsuranceFileTypeCode.Trim = "MTACAN" Then
                        Current.Session(CNMTAType) = MTAType.CANCELLATION
                        'Hold the View Type of Selected InsuranceFileType
                        Current.Session(CNViewType) = ViewType.CANCELLATION_MTA
                    ElseIf RTrim(oQuote.InsuranceFileTypeCode).Trim = "MTA PERM" Then
                        'Hold the View Type of Selected InsuranceFileType
                        Current.Session(CNViewType) = ViewType.PERMANENT_MTA
                        Current.Session(CNMTAType) = MTAType.PERMANENT
                    ElseIf oQuote.InsuranceFileTypeCode.Trim = "MTA TEMP" Then
                        'Hold the View Type of Selected InsuranceFileType
                        Current.Session(CNViewType) = ViewType.TEMPORARY_MTA
                        Current.Session(CNMTAType) = MTAType.TEMPORARY
                    End If
                    Current.Session(CNMode) = Mode.View
                    Current.Session.Remove(CNOI)
                    Current.Session(CNQuoteInSync) = False
                    Current.Session(CNQuoteMode) = QuoteMode.MTAQuote
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                    Else
                        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    End If
                Case "viewpolicy"
                    Current.Session(CNRenewal) = Nothing
                    Current.Session(CNMode) = Mode.View
                    Current.Session.Remove(CNOI)
                    Current.Session.Remove(CNQuoteMode)
                    Current.Session(CNQuoteInSync) = False
                    Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                    'WILL IT BE PREMIUM DISPLAY FOR FULL QUOTE ALWAYS????
                    'DO WE NEED TO ADD POLICY DETAILS TO SESSION? HOW WILL THE PREMIUM DISPLAY PAGE GET THE POLICY NUMBER FOR WHICH THE DETAILS NEEDS TO FETCH?
                    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                        sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                    Else
                        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    End If
            End Select
            Current.Response.Redirect(sRedirectPath, False)
        End Sub

        ''' <summary>
        ''' To Edit a Quote
        ''' </summary>
        ''' <param name="v_sCommandName"></param>
        ''' <param name="v_nInsuranceFileKey"></param>
        ''' <param name="v_bIsMarketPlacePolicy"></param>
        ''' <remarks></remarks>
        Sub EditQuote(ByVal v_sCommandName As String, ByVal v_nInsuranceFileKey As Integer, ByVal v_bIsMarketPlacePolicy As Boolean, ByVal v_bIsMarkedQuoteForCollection As Boolean)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Current.Session.Remove(CNOldPremium) 'Remove the old premium from session
            Current.Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Try
                If v_bIsMarketPlacePolicy = False Then
                    oWebService.UpdateMarketplacePolicyStatus(v_nInsuranceFileKey, v_bIsMarketPlacePolicy)
                End If
                oQuote = oWebService.GetHeaderAndSummariesByKey(v_nInsuranceFileKey)

                'Locking message is required for edit Mode
                Dim bIgnoreLocking As Boolean = False

                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(0).Key, i, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                    Next
                    Current.Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)

                Current.Session(CNQuote) = oQuote

                Dim oParty As NexusProvider.BaseParty
                If Current.Session(CNParty) Is Nothing Then
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                Else
                    oParty = CType(Current.Session(CNParty), NexusProvider.BaseParty)
                    If oParty.Key <> oQuote.PartyKey Then
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                    End If
                End If
                Current.Session(CNParty) = oParty

            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        'Show policy locking error as alert
                        Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(oPage.Page, oPage.GetType(), "policylocked", sMessage, True)
                        Current.Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try
            If v_sCommandName.ToUpper = "EDITQUOTE" Or v_sCommandName.ToUpper = "EDITMTAQUOTE" Then

                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                    Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(oPage.Page, oPage.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Current.Session(CNMTAType) = Nothing
            Select Case v_sCommandName.ToUpper

                Case "EDITQUOTE"

                    'Check product option for quote versioning on edit
                    Dim sQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                    If Not String.IsNullOrEmpty(sQuoteVersioning) AndAlso sQuoteVersioning.Trim = "1" Then
                        Dim iNewInsuranceFileKey As Integer = oQuote.InsuranceFileKey
                        Dim iNewInsuranceFolderKey As Integer = 0
                        oWebService.CopyQuote(iNewInsuranceFileKey, iNewInsuranceFolderKey, oQuote.BranchCode, v_bIsQuoteVersioning:=True)

                        oQuote = oWebService.GetHeaderAndSummariesByKey(iNewInsuranceFileKey)

                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next

                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        Current.Session(CNQuote) = oQuote
                        Current.Session(CNInsuranceFileKey) = iNewInsuranceFileKey
                    End If

                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If v_bIsMarkedQuoteForCollection And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oQuote.MarkedDateforCollection = Date.Now.Date
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Current.Session(CNQuote) = oQuote
                    Else
                        If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                            oWebService.UpdateQuotev2(oQuote)
                            Current.Session(CNQuote) = oQuote
                        End If
                    End If

                    Current.Session(CNRenewal) = Nothing
                    Current.Session(CNMode) = Mode.Edit
                    Current.Session(CNQuoteInSync) = False
                    Current.Session.Remove(CNOI)
                    Current.Session(CNInsuranceFileKey) = v_nInsuranceFileKey
                    Current.Session(CNQuoteInSync) = False


                    If IsDataSetQuickQuote() = False Then
                        Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                    Else
                        Current.Session(CNQuoteMode) = QuoteMode.QuickQuote

                    End If
                    sRedirectPath = "~/secure/PremiumDisplay.aspx"

                Case "EDITMTAQUOTE"

                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If v_bIsMarkedQuoteForCollection And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Current.Session(CNQuote) = oQuote
                    End If
                    Current.Session(CNRenewal) = Nothing
                    'before proceding BUY MTAQUOTE we need to check if the policy already have existing MTA
                    Current.Session(CNMtaReasonSelected) = Nothing
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
                                'if yes then Backdated is true
                                Current.Session(CNIsBackDatedMTA) = True
                                Exit For
                            End If
                        End If
                    Next
                    Current.Session(CNQuote) = oQuote
                    If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                        Current.Session(CNMTAType) = MTAType.CANCELLATION
                    ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQREINS" Then
                        Current.Session(CNMTAType) = MTAType.REINSTATEMENT
                        Current.Session.Remove(CNIsBackDatedMTA)
                    Else
                        Current.Session(CNMTAType) = MTAType.PERMANENT
                    End If
                    Current.Session.Remove(CNOI)
                    Current.Session(CNInsuranceFileKey) = v_nInsuranceFileKey
                    Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                    Current.Session.Item(CNMode) = Mode.Edit
                    Current.Session(CNQuoteInSync) = False
                    Current.Session(CNMtaReasonSelected) = Nothing

                    sRedirectPath = "~/secure/premiumdisplay.aspx"

            End Select

            Current.Response.Redirect(sRedirectPath, False)
        End Sub

        ''' <summary>
        ''' To do MTA on a policy version
        ''' </summary>
        ''' <param name="v_sCommandName"></param>
        ''' <param name="v_nInsuranceFileKey">Latest live version insurance file key</param>
        ''' <param name="v_bIsMarketPlacePolicy"></param>
        ''' <param name="v_bIsMarkedQuoteForCollection"></param>
        ''' <remarks></remarks>
        Sub MTA(ByVal v_sCommandName As String, ByVal v_nInsuranceFileKey As Integer, ByVal v_bIsMarketPlacePolicy As Boolean, ByVal v_bIsMarkedQuoteForCollection As Boolean)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Current.Session.Remove(CNOldPremium) 'Remove the old premium from session
            Current.Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Try

                If v_bIsMarketPlacePolicy = False Then
                    oWebService.UpdateMarketplacePolicyStatus(v_nInsuranceFileKey, v_bIsMarketPlacePolicy)
                End If
                oQuote = oWebService.GetHeaderAndSummariesByKey(v_nInsuranceFileKey)
                'Put highest risk key into Session
                For nCt As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(nCt).Key, nCt, oQuote)
                Next
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Current.Session(CNQuote) = oQuote

                Dim oParty As NexusProvider.BaseParty
                If Current.Session(CNParty) Is Nothing Then
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                Else
                    oParty = CType(Current.Session(CNParty), NexusProvider.BaseParty)
                    If oParty.Key <> oQuote.PartyKey Then
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                    End If
                End If
                Current.Session(CNParty) = oParty

            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(oPage, oPage.GetType(), "policylocked", sMessage, True)
                        Current.Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally
            End Try
            If v_sCommandName = "MTAquote" Then
                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                    Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(oPage, oPage.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Current.Session(CNMTAType) = Nothing
            Select Case v_sCommandName
                Case "MTAquote"
                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If v_bIsMarkedQuoteForCollection And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Current.Session(CNQuote) = oQuote
                    End If
                    Current.Session(CNMode) = Mode.Edit
                    Current.Session.Remove(CNOI)
                    Current.Session(CNRenewal) = Nothing
                    Current.Session(CNInsuranceFileKey) = v_nInsuranceFileKey
                    Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                    Current.Session(CNQuoteInSync) = False
                    Current.Session(CNMtaReasonSelected) = Nothing
                    sRedirectPath = "~/secure/MTAReason.aspx"
            End Select

            Current.Response.Redirect(sRedirectPath, False)
        End Sub

        ''' <summary>
        ''' To renew a quote
        ''' </summary>
        ''' <param name="v_sCommandName"></param>
        ''' <param name="v_nInsuranceFileKey"></param>
        ''' <remarks></remarks>
        Sub RenewQuote(ByVal v_sCommandName As String, ByVal v_nInsuranceFileKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Current.Session.Remove(CNOldPremium) 'Remove the old premium from session
            Current.Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(v_nInsuranceFileKey)

                If oQuote.MarkedQuoteForCollection = True Then
                    oQuote.MarkedQuoteForCollection = False
                    oQuote.MarkedDateforCollection = Date.Now.Date
                    oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                    Current.Session(CNQuote) = oQuote
                End If

                'Locking message is required for details Mode
                Dim bIgnoreLocking As Boolean = False
                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                    oWebService.GetRisk(oQuote.Risks(0).Key, 0, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                    Current.Session(CNCurrentRiskKey) = 0
                End If
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Current.Session(CNQuote) = oQuote

                Dim oParty As NexusProvider.BaseParty
                If Current.Session(CNParty) Is Nothing Then
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                Else
                    oParty = CType(Current.Session(CNParty), NexusProvider.BaseParty)
                    If oParty.Key <> oQuote.PartyKey Then
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                    End If
                End If
                Current.Session(CNParty) = oParty

            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(oPage, oPage.GetType(), "policylocked", sMessage, True)
                        Current.Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally

            End Try
            If v_sCommandName = "viewDetails" Then
                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer = True Or bIsPendingPortfolioTransfer = True Then
                    Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(oPage, oPage.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Current.Session(CNMTAType) = Nothing
            Select Case v_sCommandName
                Case "viewDetails" 'Renewal Policy is being viewed
                    Current.Session.Remove(CNMTAType)
                    Current.Session.Remove(CNMTATypeDesc)
                    Current.Session.Remove(CNRenewal)
                    Current.Session.Remove(CNRenewalShowPremium)

                    Current.Session(CNMode) = Mode.Buy
                    Current.Session.Remove(CNOI)
                    Current.Session(CNRenewal) = True
                    Current.Session.Remove(CNQuoteMode)
                    Current.Session(CNQuoteInSync) = False
                    Current.Session(CNQuoteMode) = QuoteMode.FullQuote

                    sRedirectPath = "~/secure/PremiumDisplay.aspx"
            End Select

            Current.Response.Redirect(sRedirectPath, False)
        End Sub

        ''' <summary>
        ''' To reinstate a policy
        ''' </summary>
        ''' <param name="v_sCommandName"></param>
        ''' <param name="v_nInsuranceFileKey"></param>
        ''' <param name="v_bIsMarkedQuoteForCollection"></param>
        ''' <remarks></remarks>
        Sub Reinstate(ByVal v_sCommandName As String, ByVal v_nInsuranceFileKey As Integer, ByVal v_bIsMarkedQuoteForCollection As Boolean)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Current.Session.Remove(CNOldPremium) 'Remove the old premium from session
            Current.Session.Remove(CNRiskType) 'Reset the Risk Type
            ClearClaims() 'to Clear the claim session variable if any

            Try
                oQuote = oWebService.GetHeaderAndSummariesByKey(v_nInsuranceFileKey)

                'Locking message is required for reinstatement Mode
                Dim bIgnoreLocking As Boolean = False

                ' Put highest risk key into Session
                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                    oWebService.GetRisk(oQuote.Risks(0).Key, 0, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                    Current.Session(CNCurrentRiskKey) = 0
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)
                Current.Session(CNQuote) = oQuote

                Dim oParty As NexusProvider.BaseParty
                If Current.Session(CNParty) Is Nothing Then
                    oParty = oWebService.GetParty(oQuote.PartyKey)
                Else
                    oParty = CType(Current.Session(CNParty), NexusProvider.BaseParty)
                    If oParty.Key <> oQuote.PartyKey Then
                        oParty = oWebService.GetParty(oQuote.PartyKey)
                    End If
                End If
                Current.Session(CNParty) = oParty

            Catch ex As NexusProvider.NexusException
                'Policy locking error
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Policy Locking
                        Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                        'Show policy locking error as alert
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(oPage, oPage.GetType(), "policylocked", sMessage, True)
                        Current.Server.ClearError()
                        ClearQuote()
                        Exit Sub
                    Case Else
                        Throw
                End Select
            Finally
                'oWebService = Nothing
            End Try
            If v_sCommandName = "Reinstatement" Then
                Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                Dim sMessage As String = ""
                If bIsPendingCloneTransfer Or bIsPendingPortfolioTransfer Then
                    Dim oPage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                    If bIsPendingPortfolioTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingPortfolioTransfer")
                    ElseIf bIsPendingCloneTransfer = True Then
                        sMessage = HttpContext.GetLocalResourceObject(oPage.AppRelativeVirtualPath, "msg_PendingClonedTransfer")
                    End If
                    ScriptManager.RegisterStartupScript(oPage, oPage.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                    Exit Sub
                End If
            End If
            Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
            'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            If oQuote.Risks.Count > 0 Then
                GetDataSetDefinition()
            End If
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sRedirectPath As String = String.Empty
            'this will need to be set to nothing in case after doing MTA process user selects client
            ' and then choses to buy a Quote 
            Current.Session(CNMTAType) = Nothing
            Select Case v_sCommandName
                Case "Reinstatement"

                    'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                    If v_bIsMarkedQuoteForCollection And oQuote.MarkedQuoteForCollection Then
                        oQuote.MarkedQuoteForCollection = False
                        oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        Current.Session(CNQuote) = oQuote
                    End If
                    Current.Session.Remove(CNRenewal)
                    Current.Session.Remove(CNIsBackDatedMTA)
                    Current.Session(CNQuote) = oQuote
                    Current.Session(CNMTAType) = MTAType.REINSTATEMENT
                    Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                    Current.Session.Remove(CNOI)
                    Current.Session(CNInsuranceFileKey) = v_nInsuranceFileKey
                    Current.Session(CNMtaReasonSelected) = Nothing
                    sRedirectPath = "~/secure/MTAReason.aspx"
            End Select

            Current.Response.Redirect(sRedirectPath, False)
        End Sub
    End Module

End Namespace
