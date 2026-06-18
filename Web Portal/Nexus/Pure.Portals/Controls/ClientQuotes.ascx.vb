Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Xml
Imports System.Linq

Namespace Nexus

    Partial Class Controls_ClientQuotes : Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oPartySummary As NexusProvider.PartySummary
        Dim sDisplayStatus As String()
        Private iPartyKey As Integer
        Public Const ItemDeleted As String = "ItemDeleted"
        Public Const CNPolicyCollection As String = "PolicyCollection"
        Public Shared CNSortDirection As String = ""
        Public Shared CNSortExpression As String = ""
        Public Const CNBrokerCollection As String = "BrokerCollection"
        Public Const CNPolicyChildCollection As String = "PolicyChildCollection"
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
        Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
        Dim oRiskType As New NexusProvider.RiskType
        'Fix for 3509- Added hastable to show only one ReInstatement Link
        Dim hstMTACanVerion As Hashtable = New Hashtable()

        Public Property DisplayViewOnly As Boolean

        Public Property PartyKey() As Integer
            Set(ByVal value As Integer)
                iPartyKey = value
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                Try
                    Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                    Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                            .Portals.Portal(Portal.GetPortalID()).Products

                    If oPortalConfig.ViewOnlyLatestPolicyVersion = True Then

                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    oPartySummary = oWebService.GetPartyPolicies(.ClientSharedData.ShortName.Trim)
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    oPartySummary = oWebService.GetPartyPolicies(.ClientSharedData.ShortName.Trim)
                                End With
                        End Select
                        'store the data in ViewState to use again for page indexing
                        ViewState.Add(CNPolicyCollection, oPartySummary.Policies)

                        FilterRecords()
                    Else
                        getPartySummary(oParty.Key)
                        'oPartySummary = oWebService.GetPartySummary(oParty.Key)

                        ''store the data in ViewState to use again for page indexing
                        'ViewState.Add(CNPolicyCollection, oPartySummary.Policies)

                        'FilterRecords()
                    End If
                Finally
                    oWebService = Nothing
                    oPartySummary = Nothing
                End Try
            End Set
            Get
                Return iPartyKey
            End Get
        End Property

        Sub getPartySummary(ByVal iPartyKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Try
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                        .Portals.Portal(Portal.GetPortalID()).Products
                Dim lblResults As Integer

                oPartySummary = oWebService.GetPartySummary(iPartyKey)

                'store the data in ViewState to use again for page indexing
                ViewState.Add(CNPolicyChildCollection, oPartySummary.Policies)

                'If ddlRecordType.SelectedValue = "All" Then
                '    oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.ALL, ddlProductType.SelectedValue, lblResults, txtPolicyNumber.Text, txtName.Text, oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, IIf(txtAgentKey.Value = "", 0, txtAgentKey.Value))
                'ElseIf ddlRecordType.SelectedValue = "Policy" Then
                '    oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.POLICY, ddlProductType.SelectedValue, lblResults, txtPolicyNumber.Text, txtName.Text, oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, IIf(txtAgentKey.Value = "", 0, txtAgentKey.Value))
                'Else
                '    oPartySummary = oWebService.GetBrokerSummary(NexusProvider.InsuranceQuoteType.QUOTE, ddlProductType.SelectedValue, lblResults, txtPolicyNumber.Text, txtName.Text, oPortal.MaxSearchResults, Nothing, dStartDate, dQuoteDate, IIf(txtAgentKey.Value = "", 0, txtAgentKey.Value))
                'End If

                'ViewState.Add(CNBrokerChildCollection, oPartySummary.Policies)

                'oPartySummary.Policies.SortColumn = "QuoteVersion"
                'oPartySummary.Policies.SortingOrder = NexusProvider.GenericComparer.SortOrder.Descending
                'oPartySummary.Policies.Sort()

                Dim oPolicies As New NexusProvider.PolicyCollection
                Dim opolicy1 As NexusProvider.Policy
                Dim iBaseKey As Integer = 0
                Dim iCount As Integer

                'Find out the quote versions and add them to a single collection               
                Dim htQuoteVersioningCache As New Hashtable()
                If oPartySummary.Policies IsNot Nothing AndAlso oPartySummary.Policies.Count > 0 Then
                    For iCount = 0 To oPartySummary.Policies.Count - 1
                        Dim sProductCode As String = oPartySummary.Policies(iCount).ProductCode
                        Dim sIsQuoteVersioning As String
                        If htQuoteVersioningCache.ContainsKey(sProductCode) Then
                            sIsQuoteVersioning = htQuoteVersioningCache(sProductCode).ToString()
                        Else
                            sIsQuoteVersioning = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, sProductCode, "")
                            htQuoteVersioningCache(sProductCode) = If(sIsQuoteVersioning, "")
                        End If
                        If Not String.IsNullOrEmpty(sIsQuoteVersioning) AndAlso sIsQuoteVersioning.Trim = "1" Then
                            If iBaseKey <> oPartySummary.Policies(iCount).BaseInsuranceFolderKey And oPartySummary.Policies(iCount).BaseInsuranceFolderKey <> 0 And oPartySummary.Policies(iCount).InsuranceFileTypeCode.Trim.ToUpper = "QUOTE" Then
                                iBaseKey = oPartySummary.Policies(iCount).BaseInsuranceFolderKey
                                opolicy1 = New NexusProvider.Policy(oPartySummary.Policies(iCount).InsuranceFileKey)
                                With opolicy1
                                    .AccHandler = oPartySummary.Policies(iCount).AccHandler
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
                                    .LeadAgent = oPartySummary.Policies(iCount).AgentShortName
                                    .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                    .QuoteExpiryDate = oPartySummary.Policies(iCount).QuoteExpiryDate
                                    .RenewedVersion = oPartySummary.Policies(iCount).RenewedVersion
                                    .RiskStatus = oPartySummary.Policies(iCount).RiskStatus
                                    .MarkedQuoteForCollection = oPartySummary.Policies(iCount).MarkedQuoteForCollection
                                    .IsMarketPlacePolicy = oPartySummary.Policies(iCount).IsMarketPlacePolicy
                                    .AnnualPremium = oPartySummary.Policies(iCount).AnnualPremium
                                End With
                                oPolicies.Add(opolicy1)
                            ElseIf oPartySummary.Policies(iCount).InsuranceFileTypeCode.Trim.ToUpper <> "QUOTE" Then
                                opolicy1 = New NexusProvider.Policy(oPartySummary.Policies(iCount).InsuranceFileKey)
                                With opolicy1
                                    .AccHandler = oPartySummary.Policies(iCount).AccHandler
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
                                    .LeadAgent = oPartySummary.Policies(iCount).AgentShortName
                                    .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                    .QuoteExpiryDate = oPartySummary.Policies(iCount).QuoteExpiryDate
                                    .RenewedVersion = oPartySummary.Policies(iCount).RenewedVersion
                                    .RiskStatus = oPartySummary.Policies(iCount).RiskStatus
                                    .MarkedQuoteForCollection = oPartySummary.Policies(iCount).MarkedQuoteForCollection
                                    .IsMarketPlacePolicy = oPartySummary.Policies(iCount).IsMarketPlacePolicy
                                    .AnnualPremium = oPartySummary.Policies(iCount).AnnualPremium
                                End With
                                oPolicies.Add(opolicy1)
                            End If

                        Else
                            'Pure 3.0 ---- WPR 41 (Catlin Code put in else part if the Quote Numbering scheme is OFF)
                            opolicy1 = New NexusProvider.Policy(oPartySummary.Policies(iCount).InsuranceFileKey)
                            With opolicy1
                                .AccHandler = oPartySummary.Policies(iCount).AccHandler
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
                                .LeadAgent = oPartySummary.Policies(iCount).AgentShortName
                                .InsuranceFileTypeCode = oPartySummary.Policies(iCount).InsuranceFileTypeCode
                                .QuoteExpiryDate = oPartySummary.Policies(iCount).QuoteExpiryDate
                                .RenewedVersion = oPartySummary.Policies(iCount).RenewedVersion
                                .RiskStatus = oPartySummary.Policies(iCount).RiskStatus
                                .MarkedQuoteForCollection = oPartySummary.Policies(iCount).MarkedQuoteForCollection
                                .IsMarketPlacePolicy = oPartySummary.Policies(iCount).IsMarketPlacePolicy
                                .AnnualPremium = oPartySummary.Policies(iCount).AnnualPremium
                            End With
                            oPolicies.Add(opolicy1)
                        End If

                    Next
                End If
                oPartySummary.Policies = oPolicies

                ViewState.Add(CNPolicyCollection, oPartySummary.Policies)

                FilterRecords()
            Finally
                oWebService = Nothing
                oPartySummary = Nothing
                Session(ItemDeleted) = "0"
            End Try
        End Sub

        Public WriteOnly Property DisplayStatus() As String
            Set(ByVal value As String)
                'split the value entered into an array as it will contain comma separated values
                sDisplayStatus = Split(value, ",")
            End Set
        End Property

        Protected Sub grdvQuotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvQuotes.PageIndexChanging
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            If CNSortExpression <> "" Then
                oPolicyCollection.SortColumn = CNSortExpression
                oPolicyCollection.SortingOrder = CNSortDirection
                oPolicyCollection.Sort()
            End If
            grdvQuotes.DataSource = oPolicyCollection
            If grdvQuotes.PageCount <= 1 Then
                grdvQuotes.AllowPaging = False
            Else
                grdvQuotes.AllowPaging = True
            End If
            grdvQuotes.PageIndex = e.NewPageIndex
            grdvQuotes.DataBind()
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            getPartySummary(oParty.Key)
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

        Protected Sub grdvQuotes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvQuotes.RowCommand
            'This is the best place to Reset the session in case when we choose the client again 
            'and then he chooses to buy a policy or do a quote after he has done Quote Collection Process
            ClearQuoteCollectionSessionValues()
            ClearQuote()
            'Remove all anonymous quote sessions as quote has been transferred to real party
            Session.Remove(CNIsAnonymous)
            Session.Remove(CNAnonymous)
            Session.Remove(CNIsTransferQuoteRequired)
            Session.Remove(CNOnlyOriginalRating)
            Dim bExclusiveLock As Boolean = True

            If (e.CommandName.ToString.ToUpper = "VIEWMTA" Or
                e.CommandName.ToString.ToUpper = "VIEWPOLICY" Or
                e.CommandName.ToString.ToUpper = "VIEW" Or
                e.CommandName.ToString.ToUpper = "VIEWDETAILS" Or
                e.CommandName.ToString.ToUpper = "VIEWUNDERRENEWALPOLICY") Then
                bExclusiveLock = False

            ElseIf Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                'Unlock policy for same user
                Dim nInsuranceFolderKey As Integer
                Dim GridRow As GridViewRow
                GridRow = CType((e.CommandSource).NamingContainer, GridViewRow)
                Dim lblInsuranceFolderKey As Label = GridRow.FindControl("lblInsuranceFolderKey")
                nInsuranceFolderKey = CInt(lblInsuranceFolderKey.Text)
                UnlockPolicy(nInsuranceFolderKey, Session(CNBranchCode).ToString)
            End If

            If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote
                Dim iCurrentRiskKey As Integer
                Session.Remove(CNOldPremium) 'Remove the old premium from session
                Session.Remove(CNRiskType) 'Reset the Risk Type
                Session.Remove(CNCurrentRiskKey) 'Reset the current Risk Key
                ClearClaims() 'to Clear the claim session variable if any

                'Pure 3.0 ---- WPR 41
                Dim sRedirectPath As String = String.Empty
                Select Case e.CommandName

                    Case "CopyQuote"

                        'User has decided to opt "Copy" quote option
                        Dim iInsuranceFile As Integer = e.CommandArgument

                        ''call SAM method "CopyQuote" to create copy of the quote 
                        oWebService.CopyQuote(iInsuranceFile)

                        ''redirect to same page again, to refresh the page
                        If HttpContext.Current.Session.IsCookieless Then
                            sRedirectPath = Request.Url.ToString
                            Dim iIndex As Integer = sRedirectPath.IndexOf(AppSettings("WebRoot"))
                            iIndex = iIndex + Convert.ToInt16(Convert.ToString(AppSettings("WebRoot")).Length)
                            sRedirectPath = sRedirectPath.Insert(iIndex, "(S(" & Session.SessionID.ToString() + "))/")
                        Else
                            sRedirectPath = Request.Url.ToString
                        End If
                        Response.Redirect(sRedirectPath, True)

                End Select
                Try

                    oQuote = oWebService.GetHeaderAndSummariesByKey(e.CommandArgument, bExclusiveLock:=bExclusiveLock)

                    'Locking message is not required for View Mode
                    Dim bIgnoreLocking As Boolean = False
                    If e.CommandName = "viewMTA" Or e.CommandName = "viewpolicy" Then
                        bIgnoreLocking = True
                    End If

                    'Put highest risk key into Session
                    For i As Integer = 0 To oQuote.Risks.Count - 1
                        oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
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

                Catch ex As NexusProvider.NexusException
                    'Catch Policy Locking error and show error alert
                    Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                        Case "200" 'Policy Locking
                            'Show policy locking error as alert
                            Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Server.ClearError()
                            ClearQuote()
                            Exit Sub
                        Case "1000148" 'Policy Locking
                            'Show policy locking error as alert
                            Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_policylocked_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(1) + ".") + "')"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage, True)
                            Server.ClearError()
                            ClearQuote()
                            Exit Sub
                        Case Else
                            Throw
                    End Select

                Finally
                    'oWebService = Nothing
                End Try

                If e.CommandName = "editquote" OrElse e.CommandName = "editmtaquote" OrElse e.CommandName = "MTAquote" OrElse e.CommandName = "buymtaquote" OrElse
                        e.CommandName = "buyquote" OrElse e.CommandName = "viewDetails" OrElse e.CommandName = "Reinstatement" Then
                    Dim bIsPendingPortfolioTransfer, bIsPendingCloneTransfer As Boolean
                    oWebService.IsPendingTransfer(oQuote.InsuranceFileKey, bIsPendingCloneTransfer, bIsPendingPortfolioTransfer, oQuote.InsuranceFileRef, oQuote.BranchCode)
                    Dim sMessage As String = ""
                    If bIsPendingCloneTransfer OrElse bIsPendingPortfolioTransfer Then
                        If bIsPendingPortfolioTransfer Then
                            sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingPortfolioTransfer"))
                        ElseIf bIsPendingCloneTransfer Then
                            sMessage = Convert.ToString(GetLocalResourceObject("msg_PendingClonedTransfer"))
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PendingPortfolioTransfer", "alert('" + sMessage + "')", True)
                        Exit Sub
                    End If
                End If

                Session(CNCurrenyCode) = oQuote.CurrencyCode
                'QUICK QUOTE CHECK IS REQUIRED. IF QUICK_QUOTE IS "TRUE", USER WILL BE REDIRECTED TO QUICK QUOTE ELSE TO FULL QUOTE

                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

                'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                If oQuote.Risks.Count > 0 Then
                    GetDataSetDefinition()

                    DataSetFunctions.GetScreens()
                End If

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
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.CANCELLATION_MTA
                        ElseIf oQuote.InsuranceFileTypeCode = "MTA PERM" Then
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.PERMANENT_MTA
                        ElseIf oQuote.InsuranceFileTypeCode = "MTA TEMP" Then
                            'Hold the View Type of Selected InsuranceFileType
                            Session(CNViewType) = ViewType.TEMPORARY_MTA
                        End If
                        Session(CNMode) = Mode.View
                        Session.Remove(CNOI)
                        Session(CNQuoteInSync) = False
                        Session(CNQuoteMode) = QuoteMode.MTAQuote
                        'sRedirectPath = "~/secure/PremiumDisplay.aspx"
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
                        'sRedirectPath = "~/secure/PremiumDisplay.aspx"
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

                        'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                        If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                            oQuote.MarkedQuoteForCollection = False
                            oQuote.MarkedDateforCollection = Date.Now.Date
                            oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                            Session(CNQuote) = oQuote
                        Else
                            If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                oWebService.UpdateQuotev2(oQuote)
                                Session(CNQuote) = oQuote
                            End If
                        End If
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
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If
                        If oProduct.AllowMultiRisks = False Then
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
                            Dim sMainDetail As String = Nothing
                            sFirstRiskPage = GetFirstRiskScreen(sProductFolder & "/" & oRiskType.Path & "/fullquote.config", sMainDetail)
                            If sMainDetail.ToLower = "true" Then
                                sRedirectPath = sProductFolder & "/" & sFirstRiskPage
                            Else
                                sRedirectPath = sProductFolder & "/" & oRiskType.Path & "/" & sFirstRiskPage
                            End If
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If

                    Case "editmtaquote"

                        'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                        If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                            oQuote.MarkedQuoteForCollection = False
                            oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                            Session(CNQuote) = oQuote
                        Else
                            If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                oWebService.UpdateQuotev2(oQuote)
                                Session(CNQuote) = oQuote
                            End If
                        End If
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
                                If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" OrElse oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "POLICY" OrElse
                                oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" OrElse oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS" Then
                                    SelMTAQuoteStartDate = oQuote.CoverStartDate
                                    ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                                    If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                        Session(CNIsBackDatedMTA) = True
                                    End If
                                End If
                            Next
                        End If

                        Session(CNQuote) = oQuote
                        If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                            Session(CNMTAType) = MTAType.CANCELLATION
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQREINS" Then
                            Session(CNMTAType) = MTAType.REINSTATEMENT
                        Else
                            Session(CNMTAType) = MTAType.PERMANENT
                        End If
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session.Item(CNMode) = Mode.Edit
                        Session(CNQuoteInSync) = False
                        Session(CNMtaReasonSelected) = Nothing

                        'sRedirectPath = "~/secure/premiumdisplay.aspx"
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
                                If oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTA PERM" OrElse oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "POLICY" OrElse
                                oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTACAN" OrElse oPolicy.Item(TempVar).InsuranceFileTypeCode.Trim = "MTAREINS" Then
                                    SelMTAQuoteStartDate = oQuote.CoverStartDate
                                    ExistingMTAStartDate = oPolicy.Item(TempVar).CoverStartDate
                                    If SelMTAQuoteStartDate < ExistingMTAStartDate Then
                                        Session(CNIsBackDatedMTA) = True
                                    End If
                                End If
                            Next
                        End If
                        If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                            oWebService.UpdateQuotev2(oQuote)
                            Session(CNQuote) = oQuote
                        End If

                        Session(CNQuote) = oQuote
                        If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                            Session(CNMTAType) = MTAType.CANCELLATION
                        ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQREINS" Then
                            Session(CNMTAType) = MTAType.REINSTATEMENT
                        Else
                            Session(CNMTAType) = MTAType.PERMANENT
                        End If
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session.Item(CNMode) = Mode.Buy
                        Session(CNQuoteInSync) = False

                        'sRedirectPath = "~/secure/premiumdisplay.aspx"
                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If
                    Case "buyquote"
                        Session(CNRenewal) = Nothing
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        If oQuote.InsuranceFileTypeCode = "WRITTEN" And oQuote.InsuranceFileVersion > 1 Then
                            Session(CNRenewal) = True
                        End If
                        'TO Be Cross Check
                        Session(CNQuoteInSync) = False
                        Dim oRisk As NexusProvider.Risk = oQuote.Risks.FindItemByRiskKey(iCurrentRiskKey)

                        If oRisk IsNot Nothing Then

                            Select Case oRisk.StatusCode
                                Case "DECLINE"
                                    'sRedirectPath = "~/declined.aspx"
                                    If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                        Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                    Else
                                        sRedirectPath = "~/declined.aspx"
                                    End If
                                Case "REFER"
                                    'sRedirectPath = "~/referred.aspx"
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
                                                Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                            End If
                                            'Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                        ElseIf CheckDecline() = True Then
                                            Session(CNQuoteMode) = QuoteMode.FullQuote
                                            If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                            Else
                                                Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                                            End If
                                            'Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
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
                                        sRedirectPath = "~/QQPremium.aspx"
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
                                            'Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                            If DataSetFunctions.sReferScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sReferScreenURL)
                                            Else
                                                Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
                                            End If
                                        ElseIf CheckDecline() = True Then
                                            Session(CNQuoteMode) = QuoteMode.FullQuote
                                            If DataSetFunctions.sDeclineScreen.ToLower = "true" Then
                                                Response.Redirect(DataSetFunctions.sDeclineScreenURL)
                                            Else
                                                Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
                                            End If
                                            'Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
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
                                        sRedirectPath = "~/QQPremium.aspx"
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
                        'sRedirectPath = "~/secure/PremiumDisplay.aspx"
                    Case "Reinstatement"

                        'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                        If hdMarkedtext.Text = "true" And oQuote.MarkedQuoteForCollection Then
                            oQuote.MarkedQuoteForCollection = False
                            oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                            Session(CNQuote) = oQuote
                        Else
                            If (oQuote.QuoteExpiryDate < DateTime.Now) Then
                                oWebService.UpdateQuotev2(oQuote)
                                Session(CNQuote) = oQuote
                            End If
                        End If

                        Session.Remove(CNRenewal)
                        Session(CNQuote) = oQuote
                        Session(CNMTAType) = MTAType.REINSTATEMENT
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session.Remove(CNOI)
                        Session(CNInsuranceFileKey) = e.CommandArgument
                        Session(CNMtaReasonSelected) = Nothing
                        sRedirectPath = "~/secure/MTAReason.aspx"
                End Select

                Response.Redirect(sRedirectPath, False)

            End If

        End Sub
        Protected Sub grdvQuotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvQuotes.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim htReinstat As New Hashtable
                htReinstat = ViewState("htReinstat")
                Dim dExpiryDate As Date = CType(e.Row.DataItem, NexusProvider.Policy).QuoteExpiryDate

                Dim bHasVersions As Boolean = False
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
                Dim divMenuItem As HtmlGenericControl = e.Row.FindControl("divMenuItem")
                Dim lnkbtnView As LinkButton = e.Row.FindControl("lnkbtnView")

                lnkbtnView.Visible = False
                lnkbtnView.Enabled = False

                'Pure 3.0 ---- WPR 41
                Dim lnkbtnCopyQuote As LinkButton = e.Row.FindControl("lnkbtnCopyQuote")

                Dim lbl_Status As Label = e.Row.FindControl("lbl_Status")

                Dim grdvSubBroker As GridView = e.Row.FindControl("grdvSubBroker")
                Dim imgExpand As Image = e.Row.FindControl("imgExpand")
                If (hvGridIDs.Value = "") Then
                    hvGridIDs.Value = grdvSubBroker.ClientID
                Else
                    hvGridIDs.Value = hvGridIDs.Value & "," & grdvSubBroker.ClientID

                End If

                Dim dtExpiryDate As Date = CType(e.Row.DataItem, NexusProvider.Policy).QuoteExpiryDate
                'Following code has been added to fix issue 2580
                If e.Row.Cells(5).Text = "01/01/0001" Then
                    e.Row.Cells(5).Text = ""
                Else
                    Dim dt As DateTime = DateTime.Parse(e.Row.Cells(5).Text)

                    Dim dtYear As Int32 = dt.Year
                    If dtYear = 1899 Then
                        e.Row.Cells(5).Text = ""
                    End If
                End If
                'WPR63 - Find quote versions and populate the child grid
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim sRowQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, CType(e.Row.DataItem, NexusProvider.Policy).ProductCode, "")
                If CType(e.Row.DataItem, NexusProvider.Policy).DateIssued = Date.MinValue Or
                              CType(e.Row.DataItem, NexusProvider.Policy).DateIssued.ToShortDateString = "01/01/0001" Or
                              CType(e.Row.DataItem, NexusProvider.Policy).DateIssued < "01/01/1900" Then
                    e.Row.Cells(5).Text = String.Empty

                End If
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
                        sBaseInsuranceFolderKey = Convert.ToString(grdvQuotes.DataKeys(e.Row.RowIndex).Values(1))

                        Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyChildCollection)
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

                                'If (policy.QuoteExpiryDate < DateTime.Now) Then
                                '    lnkbtnSelect.Visible = False
                                '    lnkbtnSelect2.Visible = False
                                'End If

                                Dim iGracePeriod As Integer
                                If policy.QuoteExpiryDate = Date.MinValue Then
                                    iGracePeriod = IIf(GetQuoteGracePeriod(policy.ProductCode.Trim()) = "", 0, GetQuoteGracePeriod(policy.ProductCode.Trim()))
                                    dtExpiryDate = policy.CoverStartDate.AddDays(iGracePeriod).ToShortDateString()
                                Else
                                    dtExpiryDate = policy.QuoteExpiryDate
                                End If


                                'code to check if parent policy is not an Agent Pending or Pending Quote, then disable the "Buy" button
                                iCounter = iCounter + 1
                                'If (oPolicy.Count = iCounter AndAlso ((policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined) Or (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.AgentPending) Or (policy.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Pending))) Then
                                '    lnkbtnSelect2.Enabled = False
                                'End If

                            Next


                            If UserCanDoTask("NewBusiness") AndAlso CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper <> "LAP" Then

                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                                lnkbtnSelect.CommandName = "editquote"
                                'This code is added for unmarking the quote for collection
                                If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                    lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                                    lnkbtnSelect2.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
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
                            Else
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect2.CommandName = "viewpolicy"
                                lnkbtnSelect2.Visible = True
                            End If

                            If (oPolicy.Count > 1) Then
                                bHasVersions = True
                                grdvSubBroker.DataSource = oPolicy
                                If grdvSubBroker.PageCount < 1 Then
                                    grdvSubBroker.AllowPaging = False
                                Else
                                    grdvSubBroker.AllowPaging = True
                                End If
                                grdvSubBroker.DataBind()
                                imgExpand.Visible = True
                            Else
                                If (imgExpand IsNot Nothing) Then
                                    imgExpand.Visible = False
                                End If
                            End If

                        End If

                    End If

                End If
                'To remove the 'Expand' sign from the quotes which were old and didn't had any quote versions 
                If (imgExpand IsNot Nothing) Then
                    If (bHasVersions = False) Then
                        imgExpand.Visible = False
                    End If

                End If
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("InsuranceFileKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)

                'bind risk type and risk description here
                Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey)
                If oQuote IsNot Nothing Then
                    oWebService.GetHeaderAndRisksByKey(oQuote)
                    Dim sRiskType As String = String.Empty
                    Dim sRiskDescription As String = String.Empty
                    Dim sIsReferred As String = String.Empty
                    For nRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                        If (oQuote.Risks(nRiskCount).StatusCode IsNot Nothing AndAlso Trim(oQuote.Risks(nRiskCount).StatusCode).ToLower = "referred") Then
                            sIsReferred = "REFERRED"
                        End If
                        If nRiskCount = 0 Then
                            sRiskType = Trim(oQuote.Risks(nRiskCount).RiskTypeCode)
                            sRiskDescription = Trim(oQuote.Risks(nRiskCount).Description)
                        Else
                            sRiskType = sRiskType & ",</br>" & Trim(oQuote.Risks(nRiskCount).RiskTypeCode)
                            sRiskDescription = sRiskDescription & ",</br>" & Trim(oQuote.Risks(nRiskCount).Description)
                        End If
                    Next

                    CType(e.Row.DataItem, NexusProvider.Policy).RiskDescription = sRiskDescription
                    CType(e.Row.DataItem, NexusProvider.Policy).RiskTypeDescription = sRiskType

                    Dim lblRiskType As Label = CType(e.Row.FindControl("lbl_RiskType"), Label)
                    Dim lblRiskDesc As Label = CType(e.Row.FindControl("lbl_RiskDesc"), Label)
                    lblRiskType.Text = sRiskType
                    lblRiskDesc.Text = sRiskDescription
                    CType(e.Row.FindControl("lbl_IsReferred"), Label).Text = sIsReferred

                End If

                Select Case UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim())

                    Case "POLICY" 'edited by sbhatia on dated 28-Feb


                        'bind risk type and risk description here

                        If oQuote IsNot Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Dim sRiskType As String = String.Empty
                            Dim sRiskDescription As String = String.Empty
                            For nRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                                If nRiskCount = 0 Then
                                    sRiskType = Trim(oQuote.Risks(nRiskCount).RiskTypeCode)
                                    sRiskDescription = Trim(oQuote.Risks(nRiskCount).Description)
                                Else
                                    sRiskType = sRiskType & " , " & Trim(oQuote.Risks(nRiskCount).RiskTypeCode)
                                    sRiskDescription = sRiskDescription & " , " & Trim(oQuote.Risks(nRiskCount).Description)
                                End If
                            Next

                            Dim lblRiskType As Label = CType(e.Row.FindControl("lbl_RiskType"), Label)
                            Dim lblRiskDesc As Label = CType(e.Row.FindControl("lbl_RiskDesc"), Label)
                            lblRiskType.Text = sRiskType
                            lblRiskDesc.Text = sRiskDescription
                        End If

                        Dim sResFileVariableValue As String = GetLocalResourceObject("lbl_Reference")
                        GetLocalResourceObject("lbl_Reference").Equals("Policy Number")

                        '' need to check if the Policy has been CANCELLED then can't allow POLICY CHANGE again.fixed against PN:42284
                        If CType(e.Row.DataItem, NexusProvider.Policy).IsCurrent = True Then
                            If UserCanDoTask("MidTermAdjustment") Then
                                'CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).AllowMTA Then

                                'if AllowMTA then only user will be able to see option CHANGE                                

                                'Only Allow MTA if User Has Authority to do that
                                If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False Then
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

                            'If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then
                            '    e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Lapsed")
                            'End If

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
                        'Only Allow NB if User Has Authority to do that
                        'bind risk type and risk description here

                        If oQuote IsNot Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Dim sRiskType As String = String.Empty
                            Dim sRiskDescription As String = String.Empty
                            For nRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                                If nRiskCount = 0 Then
                                    sRiskType = Trim(oQuote.Risks(nRiskCount).RiskTypeCode)
                                    sRiskDescription = Trim(oQuote.Risks(nRiskCount).Description)
                                Else
                                    sRiskType = sRiskType & " , " & Trim(oQuote.Risks(nRiskCount).RiskTypeCode)
                                    sRiskDescription = sRiskDescription & " , " & Trim(oQuote.Risks(nRiskCount).Description)
                                End If
                            Next

                            Dim lblRiskType As Label = CType(e.Row.FindControl("lbl_RiskType"), Label)
                            Dim lblRiskDesc As Label = CType(e.Row.FindControl("lbl_RiskDesc"), Label)
                            lblRiskType.Text = sRiskType
                            lblRiskDesc.Text = sRiskDescription
                        End If

                        If UserCanDoTask("NewBusiness") AndAlso CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper <> "LAP" Then

                            'If (dtExpiryDate < DateTime.Today) Then
                            'lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            'lnkbtnSelect.Text = GetLocalResourceObject("lbl_view").ToString() '"edit"
                            'lnkbtnSelect.CommandName = "viewpolicy"
                            'Else
                            lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect.Text = GetLocalResourceObject("lbl_edit").ToString() '"view"
                            lnkbtnSelect.CommandName = "editquote"
                            If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                lnkbtnSelect.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                            'End If
                            'This code is added for unmarking the quote for collection
                            If CType(e.Row.DataItem, NexusProvider.Policy).MarkedQuoteForCollection Then
                                lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnMarkedConfirmation();")
                            End If
                            If CType(e.Row.DataItem, NexusProvider.Policy).IsMarketPlacePolicy Then
                                lnkbtnSelect2.Attributes.Add("OnClick", "javascript:return MarketPlacePolicyConfirmation();")
                            End If
                            'end
                            If Not UserCanDoTask("DisableBuyNow") Then 'EH023078
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_buy").ToString() '"edit"
                                lnkbtnSelect2.CommandName = "buyquote"
                            End If
                        End If

                        If UserCanDoTask("ViewQuote") Then
                            lnkbtnView.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnView.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnView.CommandName = "viewpolicy"
                            lnkbtnView.Visible = True
                            lnkbtnView.Enabled = True
                        End If

                        'Pure 3.0 ---- WPR 41
                        If Session(CNLoginType) = LoginType.Agent And UserCanDoTask("CopyQuote") Then

                            'Copy link will be available only for agents, if has Authority
                            'Make the column and link available to user
                            lnkbtnCopyQuote.Visible = True
                            lnkbtnCopyQuote.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey

                        End If
                    Case "WRITTEN"
                        If CType(e.Row.DataItem, NexusProvider.Policy).EventDesc Is Nothing Then
                            CType(e.Row.DataItem, NexusProvider.Policy).EventDesc = ""
                        End If
                        'Edit/Buy options will be available only if user has Authority
                        If UserCanDoTask("NewBusiness") And Not CType(e.Row.DataItem, NexusProvider.Policy).EventDesc.Contains("Written Renewal") Then
                            'code commented to hide the edit button for Written Policy status

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
                        End If

                    Case "MTAQUOTE", "MTAQTETEMP", "MTAQCAN"

                        'bind risk type and risk description here

                        If oQuote IsNot Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Dim sIsReferred As String = String.Empty

                            For nRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                                If (oQuote.Risks(nRiskCount).StatusCode IsNot Nothing AndAlso Trim(oQuote.Risks(nRiskCount).StatusCode).ToLower = "referred") Then
                                    sIsReferred = "REFERRED"
                                ElseIf (oQuote.Risks(nRiskCount).StatusCode IsNot Nothing AndAlso Trim(oQuote.Risks(nRiskCount).StatusCode).ToLower = "declined") Then
                                    sIsReferred = "DECLINED"
                                End If

                            Next

                            CType(e.Row.FindControl("lbl_IsReferred"), Label).Text = sIsReferred
                        End If

                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LIVE" Then
                            If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim,
                                          CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate,
                                            CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False _
                                              AndAlso UserCanDoTask("MidTermAdjustment") Then

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

                            ElseIf UserCanDoTask("ViewQuote") Then
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect2.CommandName = "viewMTA"
                            End If

                        ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LAP" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "REP" Then
                            'if the plocy has been cancelled then only one link i.e VIEW
                            If UserCanDoTask("ViewQuote") Then
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect2.CommandName = "viewMTA"

                            End If
                            If (imgExpand IsNot Nothing) Then
                                imgExpand.Visible = False
                            End If
                        End If
                    Case "MTAQREINS" '

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

                        Else
                            'if the plocy has been cancelled then only one link i.e VIEW
                            If UserCanDoTask("ViewQuote") Then
                                lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                lnkbtnSelect2.CommandName = "viewMTA"
                            End If
                            e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Lapsed")
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "MTA PERM", "MTA TEMP"

                        'bind risk type and risk description here

                        If oQuote IsNot Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Dim sIsReferred As String = String.Empty

                            For nRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                                If (oQuote.Risks(nRiskCount).StatusCode IsNot Nothing AndAlso Trim(oQuote.Risks(nRiskCount).StatusCode).ToLower = "referred") Then
                                    sIsReferred = "REFERRED"
                                ElseIf (oQuote.Risks(nRiskCount).StatusCode IsNot Nothing AndAlso Trim(oQuote.Risks(nRiskCount).StatusCode).ToLower = "declined") Then
                                    sIsReferred = "DECLINED"
                                End If

                            Next

                            CType(e.Row.FindControl("lbl_IsReferred"), Label).Text = sIsReferred
                        End If

                        Select Case UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim())
                            Case "MTA PERM"
                                If CType(e.Row.DataItem, NexusProvider.Policy).IsCurrent = True Then
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
                                        'e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Lapsed")
                                    End If
                                End If
                                If (imgExpand IsNot Nothing) Then
                                    imgExpand.Visible = False
                                End If
                            Case "MTA TEMP"

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
                                    If Not htReinstat Is Nothing AndAlso htReinstat.Count > 0 Then
                                        If (htReinstat(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFolderKey) = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) Then
                                            If UserCanDoTask("MidTermReinstatement") Then
                                                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                                lnkbtnSelect.Text = GetLocalResourceObject("lbl_Reinstatement").ToString() '"details"
                                                lnkbtnSelect.CommandName = "Reinstatement"
                                            End If
                                        End If
                                    End If
                                Else
                                    lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                    lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                                    lnkbtnSelect2.CommandName = "viewpolicy"
                                    lnkbtnSelect2.Visible = True
                                End If
                                e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Lapsed")
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
                        ElseIf (CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LIVE") Then
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewpolicy"
                            lnkbtnSelect2.Visible = True
                        End If

                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case "RENEWAL"

                        'need to show only one link i.e. "Details"
                        'Check the roles before displaying the "Details" link
                        If UserCanDoTask("Renewals") Then
                            lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect.Text = GetLocalResourceObject("lbl_details").ToString() '"details"
                            lnkbtnSelect.CommandName = "viewDetails"
                        Else
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewunderRenewalpolicy"
                        End If
                        If UserCanDoTask("ViewQuote") Then
                            lnkbtnView.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnView.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnView.CommandName = "viewpolicy"
                            lnkbtnView.Visible = True
                            lnkbtnView.Enabled = True
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If


                        ' ltStatus.Text = GetLocalResourceObject("lbl_RenewalText").ToString()
                    Case "MTACAN"
                        If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "CAN" _
                            Or CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "LAP" Then

                            'Now the Reinstatement button will only be shown if user has access to MTR/MTC
                            If IsRenewed(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim, CType(e.Row.DataItem, NexusProvider.Policy).CoverStartDate, CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) = False _
                            And IsInRenewal(CType(e.Row.DataItem, NexusProvider.Policy).Reference.Trim) = False Then
                                If Not htReinstat Is Nothing AndAlso htReinstat.Count > 0 Then
                                    If (htReinstat(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFolderKey) = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey) Then
                                        If UserCanDoTask("MidTermReinstatement") Then
                                            lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                                            lnkbtnSelect.Text = GetLocalResourceObject("lbl_Reinstatement").ToString() '"details"
                                            lnkbtnSelect.CommandName = "Reinstatement"
                                            lnkbtnSelect.Attributes.Add("OnClick", "javascript:return UnReInstatementConfirmation();")
                                        End If
                                    End If
                                End If
                            End If
                            If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Lapsed")
                            ElseIf CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim.ToUpper = "CAN" Then
                                ' e.Row.Cells(1).Text = GetLocalResourceObject("lbl_Cancelled")
                            End If
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"
                        Else 'If CType(e.Row.DataItem, NexusProvider.Policy).PolicyStatusCode.Trim = "REP" Then
                            lnkbtnSelect2.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey
                            lnkbtnSelect2.Text = GetLocalResourceObject("lbl_view").ToString() '"view"
                            lnkbtnSelect2.CommandName = "viewMTA"
                        End If
                        If (imgExpand IsNot Nothing) Then
                            imgExpand.Visible = False
                        End If
                    Case Else

                        lnkbtnSelect.Visible = False

                End Select
                Select Case UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim())
                    Case "MTA PERM"
                        lnkbtnSelect2.CommandName = "viewMTA"
                End Select
                ''Not required now since the quote status is shown with a different approach as per WPR63
                'substitute status for something more meaninful (store as a resource)
                If (lnkbtnSelect.Text = "" Or lnkbtnSelect2.Text = "") Then
                    divMenuItem.Attributes.Add("class", "rowMenuSingle")
                End If

                Dim lblStatus As Label = CType(e.Row.FindControl("lbl_Status"), Label)
                Select Case UCase(lblStatus.Text).Trim
                    Case "QUOTE"
                        lblStatus.Text = GetLocalResourceObject("QUOTE")
                    Case "POLICY"
                        lblStatus.Text = GetLocalResourceObject("POLICY")
                    Case "MTACAN"
                        lblStatus.Text = GetLocalResourceObject("MTACAN")
                    Case "MTAREINS"
                        lblStatus.Text = GetLocalResourceObject("MTAREINS")
                    Case "MTA TEMP"
                        lblStatus.Text = GetLocalResourceObject("MTA TEMP")
                    Case "MTA PERM"
                        lblStatus.Text = GetLocalResourceObject("MTA PERM")
                    Case "MTAQREINS"
                        lblStatus.Text = GetLocalResourceObject("MTAQREINS")
                    Case "MTAQUOTE"
                        lblStatus.Text = GetLocalResourceObject("MTAQUOTE")
                    Case "MTAQTETEMP"
                        lblStatus.Text = GetLocalResourceObject("MTAQTETEMP")
                End Select

                If CType(e.Row.DataItem, NexusProvider.Policy).OpenPolicyClaims = 1 Then
                    e.Row.RowState = DataControlRowState.Normal
                    e.Row.CssClass = "AspNet-GridView-OpenClaim"
                ElseIf CType(e.Row.DataItem, NexusProvider.Policy).ClosePolicyClaims = 1 Then
                    e.Row.RowState = DataControlRowState.Normal
                    e.Row.CssClass = "AspNet-GridView-CloseClaim"
                End If
                If (dExpiryDate < DateTime.Now) Then
                    'Set Expiry Date Message if the Quote and MTAQuote Expired

                    If (UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim()) = "QUOTE") Then
                        lnkbtnSelect2.Attributes.Remove("OnClick")
                        If (lnkbtnSelect2.CommandName = "buyquote") Then
                            lnkbtnSelect2.Attributes.Add("OnClick", "alert('" + GetLocalResourceObject("quoteexpire") + "');")
                        End If
                    ElseIf (UCase(CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileTypeCode.Trim()) = "MTAQUOTE") Then

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

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnMarkedConfirmation",
                        "<script language=""JavaScript"" type=""text/javascript"">function UnMarkedConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmUnMarkedCollection").ToString() & "');document.getElementById('" & hdMarkedtext.ClientID & "').value=IsConfirm;return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnReInstatementConfirmation",
                      "<script language=""JavaScript"" type=""text/javascript"">function UnReInstatementConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmReInstatement").ToString() & "');return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MarketPlacePolicyConfirmation",
                        "<script language=""JavaScript"" type=""text/javascript"">function MarketPlacePolicyConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy1").ToString() & "'); if(IsConfirm==true) { IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmMarketPlacePolicy2").ToString() & "'); return IsConfirm; } else {return IsConfirm;} }</script>")
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
                'get risk type from session
                oRiskType = Session(CNRiskType)
                'redirect to first risk screen for the current risk type
                If CType(Session(CNQuote), NexusProvider.Quote) IsNot Nothing AndAlso CType(Session(CNQuote), NexusProvider.Quote).Risks.Count = 0 Then
                    AddRiskAndRedirect()
                End If
            End If

            If Request("__EVENTARGUMENT") = "Complete" Then
                CompletePolicy()
            End If

            If Request("__EVENTARGUMENT") = "Delete" Then
                If (Session(ItemDeleted) <> "1") Then
                    DeletePolicy()
                End If
            End If

            If (sDisplayStatus IsNot Nothing) Then
                If CType(sDisplayStatus, IList).Contains("QUOTE") Or sDisplayStatus.Length = 0 Then
                    grdvQuotes.Columns(2).HeaderStyle.CssClass = "hiddencol"
                    grdvQuotes.Columns(2).ItemStyle.CssClass = "hiddencol"
                Else
                    grdvQuotes.Columns(3).HeaderStyle.CssClass = "hiddencol"
                    grdvQuotes.Columns(3).ItemStyle.CssClass = "hiddencol"
                End If
            End If

        End Sub

        Private Sub CompletePolicy()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sRedirectPath As String = String.Empty
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            DoQuoteConfirmation(oQuote.InsuranceFileKey, True)
        End Sub

        Private Sub DeletePolicy()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As New NexusProvider.Quote
            oQuote.InsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey

            oWebService.DeletePolicy(oQuote)

            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            oPartySummary = oWebService.GetPartySummary(oParty.Key)

            'store the data in ViewState to use again for page indexing
            ViewState.Add(CNPolicyCollection, oPartySummary.Policies)
            Session(ItemDeleted) = "1"

            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Path, False)
            'grdvQuotes.DataSource = oPartySummary.Policies
            'grdvQuotes.DataBind()



            Exit Sub

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

            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyChildCollection)

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
                                "<script language=""JavaScript"" type=""text/javascript"">tb_show( null,'" & sURL & "' , null);</script>")
                                Exit Sub
                            End If
                        ElseIf (Redirect = True) Then

                            'If (oQuote.Risks(iTempVar).IsRisk = True AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED") Then
                            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode) '(Session.Item(CNDataModelCode))
                            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
                            Dim sRiskFolder As String = sProductFolder & "\" & oProduct.RiskTypes.RiskType(0).Path & "/"
                            sRedirectPath = sProductFolder & oProduct.RiskTypes.RiskType(0).Path & "/" & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig)

                            If (CType(Session(CNLoginType), LoginType) = LoginType.Agent And oUserDetails.Key > 0) Then
                                Session(CNMode) = Mode.View
                                Response.Redirect(sRedirectPath, True)
                            Else
                                Response.Redirect(sRedirectPath, True)
                            End If
                            Exit Sub
                            'ElseIf (oQuote.Risks(iTempVar).IsRisk = True AndAlso (oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "QUOTED" Or oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper = "REFERRED")) Then
                            '    sRedirectPath = String.Empty
                            '    DataSetFunctions.GetScreens()
                            '    If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            '        sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                            '    Else
                            '        sRedirectPath = "~/secure/PremiumDisplay.aspx"
                            '    End If
                            '    Response.Redirect(sRedirectPath, True)
                            '    Exit Sub
                            'End If
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
            Response.Redirect(sRedirectPath, True)
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

                Dim bIgnoreLocking As Boolean = False
                If (Session(CNClientMode) = Mode.View OrElse Session(CNClientMode) = Mode.Review) Then
                    bIgnoreLocking = True
                End If

                'Put highest risk key into Session
                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, v_bIgnoreLocking:=bIgnoreLocking)
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


        Private Sub ResetTransactionInSession()
            Session.Remove(CNMTAType)
            Session.Remove(CNMTATypeDesc)
            Session.Remove(CNRenewal)
            Session.Remove(CNRenewalShowPremium)
        End Sub

        Private Function IsPolicyCancelled(ByVal PolicyRef As String, ByVal PolicyType As String, ByVal PolicyStatus As String, ByVal ncollectioncount As Integer) As Boolean
            'Policy Collection has any PolicyStatusCode="CAN" against the passed Policy 
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            Dim bStatus As Boolean = False

            'if Any Policy version has been Cancelled
            If PolicyType = "MTAQUOTE" AndAlso PolicyStatus = "CAN" Then
                'Flag set as TRUE without Check all Policy Version
                bStatus = True
            Else

                If oPolicyCollection(ncollectioncount).PolicyStatusCode IsNot Nothing Then
                    If oPolicyCollection(ncollectioncount).Reference.Trim = PolicyRef.Trim _
                        And (oPolicyCollection(ncollectioncount).PolicyStatusCode.Trim = "CAN" Or oPolicyCollection(ncollectioncount).PolicyStatusCode.Trim = "LAP") And (Not (oPolicyCollection(ncollectioncount).PolicyStatusCode.Trim = "CAN" And oPolicyCollection(ncollectioncount).InsuranceFileTypeCode.Trim = "MTAQUOTE")) Then
                        bStatus = True
                        'Yes Policy Has been Cancelled/Lapsed
                        'Check whether it has been reinstated or not
                        If IsReinstated(oPolicyCollection(ncollectioncount).Reference.Trim) = True And
                        IsRenewed(oPolicyCollection(ncollectioncount).Reference.Trim, oPolicyCollection(ncollectioncount).CoverStartDate, oPolicyCollection(ncollectioncount).InsuranceFileKey) = True Then
                            bStatus = False
                        End If
                    End If
                End If
            End If


            Return bStatus
        End Function

        Private Function IsReinstated(ByVal PolicyRef As String) As Boolean
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            Dim bStatus As Boolean = False
            Dim TempVar
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
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            Dim bStatus As Boolean = False
            Dim TempVar
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
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
            Dim bStatus As Boolean = False
            Dim TempVar
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
            If Session(CNParty) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then

                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                PartyKey = oParty.Key
            End If
        End Sub
        'This fucntion is grouped to filter the records 
        Sub FilterRecords()
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(Portal.GetPortalID()).Products
            Dim oPolCol As New NexusProvider.PolicyCollection
            Dim oPolicy As NexusProvider.Policy
            Dim TempVar As Integer
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)

            If oPolicyCollection IsNot Nothing AndAlso oPolicyCollection.Count > 0 Then
                If oPortalConfig.ViewOnlyLatestPolicyVersion = True Then

                    'Checking of the valid product and build the collection again
                    For TempVar = 0 To oPolicyCollection.Count - 1
                        If CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                            oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                            oPolicy = oPolicyCollection(TempVar)

                            If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)
                        End If
                    Next
                Else
                    If (Me.chkViewAllPolicies.Checked) Then

                        For TempVar = 0 To oPolicyCollection.Count - 1
                            If CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                                oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                                oPolicy = oPolicyCollection(TempVar)
                                If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)
                            End If
                        Next

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

                            ElseIf (oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "QUOTE" Or oPolicyCollection(TempVar).InsuranceFileTypeCode.Trim = "WRITTEN") _
                                       AndAlso oPolicyCollection(TempVar).PolicyStatusCode.Trim <> "LAP" _
                                         AndAlso CheckValidProduct(oPolicyCollection(TempVar).ProductCode) = True Then
                                oPolicy = New NexusProvider.Policy(oPolicyCollection(TempVar).Reference)
                                oPolicy = oPolicyCollection(TempVar)
                                If CheckStatus(oPolicy.InsuranceFileTypeCode) Then oPolCol.Add(oPolicy)

                            End If

                        Next
                    End If
                End If
            End If

            'store the data in ViewState to use again for page indexing
            ViewState.Add(CNPolicyCollection, oPolCol)
            If CNSortExpression <> "" Then
                oPolCol.SortColumn = CNSortExpression
                oPolCol.SortingOrder = CNSortDirection
                oPolCol.Sort()
            End If
            grdvQuotes.Visible = True

            'get max insurancefilekey of a specific insurance_folder collection
            Dim htReinstat As New Hashtable
            For nCount_opol = 0 To oPolCol.Count - 1
                If Not htReinstat.Contains(oPolCol(nCount_opol).InsuranceFolderKey) AndAlso oPolCol(nCount_opol).PolicyTypeCode.ToUpper.Trim() = "MTACAN" Then
                    htReinstat.Add(oPolCol(nCount_opol).InsuranceFolderKey, oPolCol(nCount_opol).InsuranceFileKey)
                Else
                    If (htReinstat(oPolCol(nCount_opol).InsuranceFolderKey) < oPolCol(nCount_opol).InsuranceFileKey) Then
                        htReinstat(oPolCol(nCount_opol)) = oPolCol(nCount_opol).InsuranceFileKey
                    End If
                End If
            Next
            ViewState("htReinstat") = htReinstat

            grdvQuotes.DataSource = oPolCol
            grdvQuotes.DataBind()
            If grdvQuotes.PageCount <= 1 Then
                grdvQuotes.AllowPaging = False
            Else
                grdvQuotes.AllowPaging = True
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
                    Return True
                End If
            End If
            'current status is not in filter list
            Return False
        End Function

        Protected Sub grdvQuotes_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvQuotes.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oPolicyCollection As NexusProvider.PolicyCollection = ViewState(CNPolicyCollection)
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

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not IsPostBack AndAlso Me.Visible = True Then
                If Me.PartyKey = 0 AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 _
                AndAlso Session(CNClientMode) = Mode.View AndAlso Session(CNIsNewClient) Is Nothing Then
                    'get the party key from session
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            Me.PartyKey = CType(Session(CNParty), NexusProvider.PersonalParty).Key
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            Me.PartyKey = CType(Session(CNParty), NexusProvider.CorporateParty).Key
                    End Select
                End If

                'Initialise this to false
                PanelViewAllPolicies(False)
                If UserCanDoTask("ViewOldPolicies") Then
                    PanelViewAllPolicies(True)
                Else
                    PanelViewAllPolicies(False)
                End If
            End If
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

        ReadOnly Property GetCurrentMTAType() As MTAType
            Get
                Return CType(HttpContext.Current.Session(CNMTAType), MTAType)
            End Get
        End Property

    End Class
End Namespace
