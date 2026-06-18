Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.HttpContext

Namespace Nexus

    Partial Class AuthoriseClaimPayments : Inherits Frontend.clsCMSPage
        Dim bDisplayCaseOption As Boolean = False
        Dim oWebservice As NexusProvider.ProviderBase
        Dim nClaimID As Integer = 0
        Dim sClaimNumber As String = String.Empty
        Dim nCounter As Integer = 0
        Const m_nIsReferredGridColumn As Integer = 12
        Const m_nRecommendedByGridColumn As Integer = 13
        Const m_nLinkButtonsGridColumn As Integer = 15
        Dim oList As New NexusProvider.LookupListCollection

        ''' <summary>
        ''' Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'To hold the resued collection in session
                Dim AuthoriseClaimPaymentspageCacheID As Guid
                AuthoriseClaimPaymentspageCacheID = Guid.NewGuid()
                ViewState.Add("AuthoriseClaimPaymentspageCacheID", AuthoriseClaimPaymentspageCacheID.ToString)
                'Checking the multi step approval hidden product option
                Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                oWebservice = New NexusProvider.ProviderManager().Provider
                oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)

                If oMultiStepApproval.OptionValue = "1" Then
                    hMultiStepApproval.Value = "1"
                Else
                    hMultiStepApproval.Value = "0"
                End If

                 If Request("__EVENTARGUMENT") = "" Then
                    Session(CNSearchCriteria) = Nothing
                    ClearTemporarySessionValues()
                End If

                'If search criterea in session
                Dim oSearchcritaria As Collection = CType(Session(CNSearchCriteria), Collection)
                If oSearchcritaria IsNot Nothing Then
                    For lCount As Integer = 0 To oSearchcritaria.Count - 1
                        txtClaimReference.Text = CType(oSearchcritaria.Item(txtClaimReference.ID), String)
                        txtPaymentDate.Text = CType(oSearchcritaria.Item(txtPaymentDate.ID), String)
                        txtPolicy.Text = CType(oSearchcritaria.Item(txtPolicy.ID), String)
                        txtClient.Text = CType(oSearchcritaria.Item(txtClient.ID), String)
                        txtCreatedBy.Text = CType(oSearchcritaria.Item(txtCreatedBy.ID), String)
                    Next
                End If
                'clear the session variable
                ClearQuote()
                ClearClaims()
                'To set the Focus
                Page.SetFocus(btnClaimReference)

                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = CType(Session(CNLoginName), String)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
                oWebservice.GetUserAuthorityValue(oUserAuthority)
                sIsAuthoriser.Value = oUserAuthority.UserAuthorityValue
                hiddenLimitAmount.Value = CType(oUserAuthority.UserAuthorityOptionalValue2, String)
                hiddenLimitBaseCurrencyAmount.Value = oUserAuthority.UserAuthorityOptionalValue3_baseAmount
                hiddenAuthorizedCurrencyId.Value = oUserAuthority.UserAuthorityOptionalValue1
                oList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "currency", True, False, "currency_id", hiddenAuthorizedCurrencyId.Value)
                ViewState.Add("AuthoriseCurrencyCode", oList.Item(0).Code)

                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsRecommender
                oWebservice.GetUserAuthorityValue(oUserAuthority)
                sIsRecommender.Value = oUserAuthority.UserAuthorityValue

                Try
                    If sIsAuthoriser.Value = "0" And sIsRecommender.Value = "0" Then
                        divHeader.Visible = False
                        divHidden.Visible = True
                        divSubmit.Visible = False
                    Else
                        If Request.QueryString("claim_key") IsNot Nothing Then
                            txtClaimReferenceKey.Value = Request.QueryString("claim_key")
                        End If
                        If Request.QueryString("claim_number") IsNot Nothing Then
                            txtClaimReference.Text = Request.QueryString("claim_number")
                        End If
                        Cache.Remove(ViewState("AuthoriseClaimPaymentspageCacheID"))
                        fillgrid()
                    End If
                Catch ex As System.Exception
                Finally
                    oWebservice = Nothing
                    oUserAuthority = Nothing
                End Try
            End If
            If Request("__EVENTARGUMENT") = "AuthorisePayment" Then
                AuthoriseClaimPayment()
            End If
        End Sub

        ''' <summary>
        ''' Find the Calim Payment Data 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If Page.IsValid Then
                Cache.Remove(ViewState("AuthoriseClaimPaymentspageCacheID"))
                grdvAuthoriseclaimpayments.PageIndex = 0
                fillgrid()
                ClearSearch()
                Dim oSearchcritaria As New Collection
                oSearchcritaria.Add(txtClaimReference.Text.Trim, txtClaimReference.ID)
                oSearchcritaria.Add(txtPaymentDate.Text.Trim, txtPaymentDate.ID)
                oSearchcritaria.Add(txtPolicy.Text.Trim, txtPolicy.ID)
                oSearchcritaria.Add(txtClient.Text.Trim, txtClient.ID)
                oSearchcritaria.Add(txtCreatedBy.Text.Trim, txtCreatedBy.ID)
                oSearchcritaria.Add(txtCaseReference.Text.Trim, txtCaseReference.ID)
                oSearchcritaria.Add(txtPayeeName.Text.Trim, txtPayeeName.ID)

                Session(CNSearchCriteria) = oSearchcritaria
            End If
        End Sub
        ''' <summary>
        ''' Fill the Grid with Claim payment Data
        ''' </summary>
        ''' <remarks></remarks>
        Sub fillgrid()
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oCashListItems As NexusProvider.CashListItemsCollection
            Dim oCashLstItem As New NexusProvider.CashListItems
            Dim sURL As String
            FindParty()
            FindUser()

            If txtClaimReference.Text.Trim.Length <> 0 Then
                oCashLstItem.ClaimNumber = txtClaimReference.Text.Trim
            Else
                oCashLstItem.ClaimNumber = Nothing
            End If

            If txtPaymentDate.Text.Trim.Length <> 0 And IsDate(txtPaymentDate.Text.Trim) = True Then
                oCashLstItem.PaymentDate = CDate(txtPaymentDate.Text.Trim)
            Else
                If txtPaymentDate.Text.Trim.Length <> 0 Then
                    oCashLstItem = Nothing
                    grdvAuthoriseclaimpayments.DataSource = Nothing
                    grdvAuthoriseclaimpayments.DataBind()
                    Exit Sub
                End If
            End If
            If txtPolicy.Text.Trim.Length <> 0 Then
                oCashLstItem.PolicyNumber = txtPolicy.Text.Trim
            Else
                oCashLstItem.PolicyNumber = Nothing
            End If
            If txtClient.Text.Trim.Length <> 0 Then
                oCashLstItem.ClientName = txtClient.Text.Trim
            Else
                oCashLstItem.ClientName = Nothing
            End If
            If txtClientKey.Value.Trim.Length <> 0 And txtClientKey.Value.Trim <> "0" Then
                oCashLstItem.ClientKey = txtClientKey.Value
            End If
            If txtCreatedBy.Text.Trim.Length <> 0 Then
                oCashLstItem.CreatedBy = txtCreatedBy.Text.Trim
            Else
                oCashLstItem.CreatedBy = Nothing
            End If

            If txtCaseReference.Text.Trim.Length <> 0 Then
                oCashLstItem.CaseNumber = txtCaseReference.Text.Trim
            Else
                oCashLstItem.CaseNumber = Nothing
            End If
            If txtCaseReference.Text.Length <> 0 Then
                oCashLstItem.CaseNumber = txtCaseReference.Text.Trim
            Else
                oCashLstItem.CaseNumber = Nothing
            End If

            If txtPayeeName.Text.Trim.Length <> 0 Then
                oCashLstItem.PayeeName = txtPayeeName.Text.Trim
            Else
                oCashLstItem.PayeeName = Nothing
            End If

            If UserCanDoTask("ClaimPaymentAuthorisation") Then
                hdnfIsAuthorize.Value = "1"
            Else
                hdnfIsAuthorize.Value = "0"
            End If
            grdvAuthoriseclaimpayments.Visible = True
            Try
                Dim sBranchCode As String = Nothing
                If ddlBranch.SelectedValue.Trim().ToUpper() <> "(All Branches)".ToUpper() Then
                    sBranchCode = ddlBranch.SelectedItem.Value
                End If

                If ddlBranch.SelectedValue.Trim().ToUpper() <> "(All Branches)".ToUpper() Then
                    'For selected branch only
                    oCashListItems = oWebservice.GetReferredPayments(v_oCashListItem:=oCashLstItem, v_sBranchCode:=Nothing, v_sReferredPaymentsBranchCode:=sBranchCode)
                Else
                    'for all branch assigned to user
                    oCashListItems = oWebservice.GetReferredPayments(oCashLstItem)
                End If

                grdvAuthoriseclaimpayments.Visible = True
                grdvAuthoriseclaimpayments.AllowPaging = True
                grdvAuthoriseclaimpayments.DataSource = oCashListItems
                grdvAuthoriseclaimpayments.Columns(0).Visible = True
                grdvAuthoriseclaimpayments.Columns(1).Visible = True
                grdvAuthoriseclaimpayments.DataBind()
                grdvAuthoriseclaimpayments.Columns(0).Visible = False
                grdvAuthoriseclaimpayments.Columns(1).Visible = False

                Cache.Insert(ViewState("AuthoriseClaimPaymentspageCacheID"), oCashListItems, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                hiddenPaymentAmount.Value = 0

            Catch ex As System.Exception
            Finally
                oWebservice = Nothing
                oCashListItems = Nothing
            End Try

        End Sub
        Protected Sub FindParty()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
            Dim oPartyCollection As NexusProvider.PartyCollection

            oPartySearchCriteria = New NexusProvider.PartySearchCriteria()

            Try
                If txtClient.Text.Trim().Length <> 0 Then
                    oPartySearchCriteria.ShortName = txtClient.Text.Trim()
                    oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.GC
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                    oPartyCollection = oWebService.FindParty(oPartySearchCriteria)
                    Me.txtClientKey.Value = oPartyCollection(0).Key
                End If

            Catch ex As System.Exception

            Finally
                oWebService = Nothing
                oPartySearchCriteria = Nothing
                oPartyCollection = Nothing
            End Try

        End Sub
        Protected Sub FindUser()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oFindUsersSearchCriteria As New NexusProvider.FindUsersSearchCriteria
            Dim oUserCollection As New NexusProvider.UserCollection

            Try
                If txtCreatedBy.Text.Trim().Length <> 0 Then
                    oFindUsersSearchCriteria.UserName = txtCreatedBy.Text.Trim()
                    oUserCollection = oWebService.FindUsers(oFindUsersSearchCriteria)
                    Me.txtCreatedByKey.Value = oUserCollection(0).UserId
                End If

            Catch ex As System.Exception

            Finally
                oWebService = Nothing
                oFindUsersSearchCriteria = Nothing
                oUserCollection = Nothing
            End Try
        End Sub
        ''' <summary>
        ''' Authorise Claim Payment Grid Data Bound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthoriseclaimpayments_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvAuthoriseclaimpayments.DataBound
            If grdvAuthoriseclaimpayments.Rows.Count = 0 Or grdvAuthoriseclaimpayments.PageCount = 1 Then
                grdvAuthoriseclaimpayments.AllowPaging = False
            End If

            grdvAuthoriseclaimpayments.Columns(0).Visible = False
            grdvAuthoriseclaimpayments.Columns(1).Visible = False


            If bDisplayCaseOption Then
                grdvAuthoriseclaimpayments.Columns(2).Visible = True
            Else
                grdvAuthoriseclaimpayments.Columns(2).Visible = False
            End If
        End Sub
        ''' <summary>
        ''' Page Index Change Event For Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthoriseclaimpayments_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvAuthoriseclaimpayments.PageIndexChanging
            grdvAuthoriseclaimpayments.PageIndex = e.NewPageIndex
            grdvAuthoriseclaimpayments.DataSource = CType(Cache.Item(ViewState("AuthoriseClaimPaymentspageCacheID")), NexusProvider.CashListItemsCollection)
            grdvAuthoriseclaimpayments.DataBind()
        End Sub
        ''' <summary>
        ''' For Opening the Cali over view of the selected Claim.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthoriseclaimpayments_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvAuthoriseclaimpayments.RowCommand
            Dim sPolicyNumber As String = String.Empty
            Dim sPayeeName As String = String.Empty
            Dim nClaimKey As Integer
            Dim dPaymentDate As Date
            Dim nClaimPaymentKey As Integer
            Dim sClaimNumber As String = String.Empty
            Dim sCreatedBy As String = String.Empty
            Dim dPaymentAmount As Double
            Dim sFailureReason As String = String.Empty
            Dim cstClaimPayment As New CustomValidator
            Dim bIsReferredforRecommend As Boolean
            Dim sRecommendBy As String = String.Empty
            Dim oCurrency As New NexusProvider.Currency
            Dim oQuote As NexusProvider.Quote = Nothing


            If e.CommandName <> "Page" AndAlso e.CommandName <> "Sort" Then
                For iTempVar As Integer = 0 To grdvAuthoriseclaimpayments.Rows.Count - 1
                    nClaimKey = grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(1).Text.Trim
                    If CInt(e.CommandArgument) = nClaimKey Then
                        nClaimPaymentKey = CInt(grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(0).Text.Trim)
                        ViewState("nClaimPaymentKey") = nClaimPaymentKey
                        sPolicyNumber = grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(4).Text.Trim
                        ViewState("sPolicyNumber") = sPolicyNumber
                        sPayeeName = grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(6).Text.Trim
                        ViewState("sPayeeName") = sPayeeName
                        dPaymentDate = CDate(grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(9).Text.Trim)
                        ViewState("dPaymentDate") = dPaymentDate
                        sClaimNumber = grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(3).Text.Trim
                        ViewState("sClaimNumber") = sClaimNumber
                        sCreatedBy = grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(10).Text.Trim
                        ViewState("sCreatedBy") = sCreatedBy

                        Dim oCashListItems As NexusProvider.CashListItemsCollection = Cache.Item(ViewState("AuthoriseClaimPaymentspageCacheID"))
                        If oCashListItems IsNot Nothing Then
                            For nIndex As Integer = 0 To oCashListItems.Count - 1
                                If (nClaimPaymentKey = CDbl(oCashListItems(nIndex).ClaimPaymentKey)) Then
                                    dPaymentAmount = CDbl(oCashListItems(nIndex).PaymentAmount)
                                    ViewState("dPaymentAmount") = dPaymentAmount
                                    Exit For
                                End If
                            Next
                        End If
                        bIsReferredforRecommend = Convert.ToBoolean(grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(m_nIsReferredGridColumn).Text.Trim())
                        sRecommendBy = IIf(grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(m_nRecommendedByGridColumn).Text.Trim() = "&nbsp;", String.Empty, grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(m_nRecommendedByGridColumn).Text.Trim())
                        Exit For
                    End If
                Next
                Session(CNClaim) = nClaimKey
                Session(CNClaimPaymentKey) = nClaimPaymentKey
                Session(CNClaimPaymentCreatedBy) = sCreatedBy
                Dim sBranchCode As String = Nothing
                sBranchCode = Session(CNBranchCode)
                oWebservice = New NexusProvider.ProviderManager().Provider
                Session.Remove(CNClaim)

                Try
                    'This is expected to through an error if claim is locked in BO
                    GetClaimDetails(nClaimKey)
                Catch ex As NexusProvider.NexusException
                    Throw
                End Try
                Dim oOpenClaim As NexusProvider.ClaimOpen
                oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                oQuote = oWebservice.GetHeaderAndSummariesByKey(oOpenClaim.InsuranceFileKey)
                If oOpenClaim.Client.PartyKey = 0 AndAlso oQuote IsNot Nothing Then
                    oOpenClaim.Client.PartyKey = oQuote.PartyKey
                    Session(CNClaim) = oOpenClaim
                End If


                'If Risk Type is missing then find it from oQuote
                If String.IsNullOrEmpty(oOpenClaim.RiskType) Then
                    If oQuote IsNot Nothing AndAlso oQuote.Risks.Count > 0 Then
                        For iCount As Integer = 0 To oQuote.Risks.Count - 1
                            If oQuote.Risks(iCount).Key = oOpenClaim.RiskKey Then
                                oOpenClaim.RiskType = oQuote.Risks(iCount).RiskTypeCode
                            End If
                        Next
                    End If
                End If
                Session(CNClaim) = oOpenClaim
                Session(CNClaimQuote) = oQuote
                Session(CNProductCode) = oQuote.ProductCode
                Session(CNRiskType) = oOpenClaim.RiskType




                If e.CommandName = "Select" Then
                    Session(CNMode) = Mode.ViewClaim
                    Session.Remove(CNClaim)
                    Dim oBaseParty As NexusProvider.BaseParty = Nothing
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                    Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

                    Try

                        For iTempVar As Integer = 0 To grdvAuthoriseclaimpayments.Rows.Count - 1
                            nClaimKey = CType(grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(1).Text.Trim, Integer)
                            If CInt(e.CommandArgument) = nClaimKey Then
                                sPolicyNumber = grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(4).Text.Trim
                                dPaymentDate = CDate(grdvAuthoriseclaimpayments.Rows(iTempVar).Cells(9).Text.Trim)
                                Exit For
                            End If
                        Next
                        Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails
                        Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = Nothing

                        oInsuranceFileSearchCriteria.InsuranceFileRef = sPolicyNumber
                        oInsuranceFileSearchCriteria.InsuranceRef = sPolicyNumber
                        oInsuranceFileSearchCriteria.SearchDate = dPaymentDate
                        oInsuranceFileSearchCriteria.LossDate = dPaymentDate

                        oInsuranceFileDetails = oWebservice.FindInsuranceFileForClaims(oInsuranceFileSearchCriteria, sBranchCode)

                        If oInsuranceFileDetails IsNot Nothing AndAlso oInsuranceFileDetails.Count > 0 Then

                            oQuote = oWebservice.GetHeaderAndSummariesByKey(oInsuranceFileDetails(0).InsuranceFileKey)
                            sBranchCode = oQuote.BranchCode
                        End If

                        If oQuote IsNot Nothing Then
                            'arch issue 268
                            oClaimDetails = GetClaimDetailsCall(e.CommandArgument, sBranchCode)
                            With oClaimDetails
                                oOpenClaim.CatastropheCode = .CatastropheCode
                                oOpenClaim.BaseClaimKey = .BaseClaimKey
                                oOpenClaim.Claim = .Claim
                                oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                                oOpenClaim.ClaimDescription = .ClaimDescription
                                oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                                oOpenClaim.ClaimKey = .ClaimKey
                                oOpenClaim.ClaimNumber = .ClaimNumber
                                oOpenClaim.ClaimPeril = .ClaimPeril
                                oOpenClaim.ClaimStatus = .ClaimStatus
                                oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                                oOpenClaim.ClaimStatusID = .ClaimStatusID
                                oOpenClaim.ClaimVersion = .ClaimVersion
                                oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                                oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                                oOpenClaim.ClientEmail = .ClientEmail
                                oOpenClaim.ClientFaxNo = .ClientFaxNo
                                oOpenClaim.ClientMobileNo = .ClientMobileNo
                                oOpenClaim.ClientName = .ClientName
                                oOpenClaim.ClientShortName = .ClientShortName
                                oOpenClaim.ClientTelNo = .ClientTelNo
                                oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                                oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                                oOpenClaim.Comments = .Comments
                                oOpenClaim.Contact = .Contact
                                oOpenClaim.CurrencyISOCode = .CurrencyCode
                                oOpenClaim.Description = .Description
                                oOpenClaim.ExternalHandler = .ExternalHandler
                                oOpenClaim.HandlerCode = .HandlerCode
                                oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                                oOpenClaim.InfoOnly = .InfoOnly
                                oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                                oOpenClaim.InsuranceRef = .InsuranceRef
                                oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                                oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                                oOpenClaim.IsDeleted = .IsDeleted
                                oOpenClaim.LastModifiedDate = .LastModifiedDate
                                oOpenClaim.LikelyClaim = .LikelyClaim
                                oOpenClaim.Location = .Location
                                oOpenClaim.LossDate = .LossDate
                                oOpenClaim.LossDateFrom = .LossDateFrom
                                oOpenClaim.LossFromDate = .LossToDate
                                oOpenClaim.LossToDate = .LossToDate
                                oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                                oOpenClaim.Payments = .Payments
                                oOpenClaim.PolicyNumber = .PolicyNumber
                                oOpenClaim.PolicyType = .PolicyType
                                oOpenClaim.PrimaryCause = .PrimaryCause
                                oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                                oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                                oOpenClaim.ProductDescription = .ProductDescription
                                oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                                oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                                oOpenClaim.ReportedDate = .ReportedDate
                                oOpenClaim.Reserve = .Reserve
                                oOpenClaim.RiskKey = .RiskKey
                                oOpenClaim.SecondaryCause = .SecondaryCause
                                oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                                oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                                oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                                oOpenClaim.TotalShare = .TotalShare
                                oOpenClaim.Town = .Town
                                oOpenClaim.TownCode = .TownCode
                                oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                                oOpenClaim.UserDefFldACode = .UserDefFldACode
                                oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                                oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                                oOpenClaim.UserDefFldDCode = .UserDefFldECode
                                oOpenClaim.UserDefFldECode = .UserDefFldECode
                                oOpenClaim.TPA = .TPA
                                Session.Item(CNClaimTimeStamp) = .TimeStamp
                                oOpenClaim.CurrencyISOCode = .CurrencyCode
                                Session.Item(CNCurrenyCode) = Trim(.CurrencyCode)
                                Session(CNInsuranceFileKey) = .InsuranceFileKey
                                oOpenClaim.Client = .Client
                                If oOpenClaim.Client.PartyKey = 0 Then
                                    oOpenClaim.Client.PartyKey = oQuote.PartyKey
                                End If
                                Session(CNClaimNumber) = .ClaimNumber
                                Session(CNStatus) = .ClaimStatus
                                oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                If oClaimRisk IsNot Nothing Then
                                    Session(CNDataSet) = oClaimRisk.XMLDataSet
                                End If
                            End With
                            Session(CNClaim) = oOpenClaim
                            oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                            Session.Item(CNParty) = oBaseParty
                            Session.Item(CNRisks) = oQuote.Risks
                            Session.Item(CNRenewalDate) = oQuote.RenewalDate
                            Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & oBaseParty.Addresses(0).Address4
                            Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                            Session(CNInsurer_Header) = oQuote.InsuredName
                            Session(CNProductCode) = oQuote.ProductCode
                            Session(CNClaimQuote) = oQuote
                            Session.Item(CNInsuranceFileKey) = oQuote.InsuranceFileKey
                            Session.Item(CNPolicyNumber) = oQuote.InsuranceFileRef

                            Session(CNParentPage) = "ACP"
                            Response.Redirect("~/Claims/Overview.aspx")
                        End If

                    Finally
                        oClaimDetails = Nothing
                        oWebservice = Nothing
                        oUserDetails = Nothing
                        oBaseParty = Nothing
                        oClaimRisk = Nothing
                    End Try
                ElseIf e.CommandName = "Recommend" Then
                    'set the mode
                    Session(CNMode) = Mode.Recommend
                    Session(CNAccountName) = oOpenClaim.ClientShortName
                    Session(CNAmountToPay) = dPaymentAmount
                    Dim dAuthorityAmount As Double
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    Try
                        Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                        Dim oUserAuthority As New NexusProvider.UserAuthority
                        oUserAuthority.UserCode = CType(Session(CNLoginName), String)
                        oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsRecommender
                        oWebservice.GetUserAuthorityValue(oUserAuthority)
                        dAuthorityAmount = oUserAuthority.UserAuthorityOptionalValue2
                        'dee
                        If oOpenClaim.CurrencyISOCode.Trim.ToUpper <> oUserAuthority.UserAuthorityOptionalValue3.Trim.ToUpper Then
                            oCurrency.AccountCode = Session(CNAccountName)
                            oCurrency.TransactionCurrencyCode = oOpenClaim.CurrencyISOCode.Trim.ToUpper
                            oCurrency.Mode = "ALL"
                            oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, oQuote.BranchCode)
                            dAuthorityAmount = Math.Round((dAuthorityAmount / oCurrency.TransactionCurrencyRate), 2)
                        End If
                    Catch ex As System.Exception
                    Finally
                        oWebservice = Nothing
                    End Try
                    'check user 
                    If LCase(sCreatedBy) = LCase(Session(CNLoginName)) Then
                        cstClaimPayment.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("authorizeinvalidusermsg") Is Nothing, "Cannot Proceed - Unable to recommend/authorise claim payment raised by yourself", GetLocalResourceObject("authorizeinvalidusermsg")), String)
                        cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstClaimPayment)
                        Exit Sub
                    ElseIf dPaymentAmount > dAuthorityAmount Then
                        cstClaimPayment.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("amountvalidationmsg") Is Nothing, "You are not within your authorisation limits for this payment type", GetLocalResourceObject("amountvalidationmsg")), String)
                        cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstClaimPayment)
                        Exit Sub
                    End If

                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    If oPortal.ForceToViewClaimPayment = False Then
                        'call recommend claim payment
                        RecommendClaimPayment(nClaimKey, oQuote.ProductCode, sFailureReason, Current.Session(CNClaimCallsTimeStamp))

                        If Not String.IsNullOrEmpty(sFailureReason) Then
                            Dim sMessage As String
                            If sFailureReason = "200" Then
                                sMessage = GetLocalResourceObject("msg_ClaimLocked").ToString()
                            ElseIf sFailureReason = "206" Then
                                sMessage = GetLocalResourceObject("msg_RecordModified").ToString()
                            Else
                                'Claim Payment Declined 
                                DeclineClaimPayment(Session(CNClaimPaymentKey), GetLocalResourceObject("lblDeclinedComments"))

                                If sFailureReason = "Bank" Then
                                    sMessage = GetLocalResourceObject("msg_BankDefault").ToString()
                                ElseIf sFailureReason = "Mandatory" Then
                                    sMessage = GetLocalResourceObject("msg_MandatoryFields").ToString()
                                End If
                            End If
                            'Give Error Messages
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DeclinePayment",
                                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){DeclinePayment('" & sMessage & "');});</script>", False)

                        End If
                        Cache.Remove(ViewState("AuthoriseClaimPaymentspageCacheID"))
                        'repopulate the gird
                        fillgrid()
                    Else

                        'call openclaimandredirect
                        OpenClaimAndRedirect(nClaimKey, sPolicyNumber, dPaymentDate)
                    End If

                ElseIf e.CommandName = "Authorise" Then

                    Session(CNMode) = Mode.Authorise
                    Dim dAuthorityAmount As Double
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    Try
                        Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                        Dim oUserAuthority As New NexusProvider.UserAuthority
                        oUserAuthority.UserCode = CType(Session(CNLoginName), String)
                        oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
                        oWebservice.GetUserAuthorityValue(oUserAuthority)
                        dAuthorityAmount = oUserAuthority.UserAuthorityOptionalValue2
                        If oOpenClaim.CurrencyISOCode.Trim.ToUpper <> oUserAuthority.UserAuthorityOptionalValue3.Trim.ToUpper Then
                            oCurrency.AccountCode = Session(CNAccountName)
                            oCurrency.TransactionCurrencyCode = oOpenClaim.CurrencyISOCode.Trim.ToUpper
                            oCurrency.Mode = "ALL"
                            dAuthorityAmount = oUserAuthority.UserAuthorityOptionalValue3_baseAmount

                            'oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, oQuote.BranchCode)
                            'dAuthorityAmount = Math.Round((dAuthorityAmount / oCurrency.TransactionCurrencyRate), 2)
                        End If
                    Catch ex As System.Exception
                    Finally
                        oWebservice = Nothing
                    End Try
                    'check user
                    If LCase(sCreatedBy) = LCase(Session(CNLoginName)) Then
                        cstClaimPayment.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("authorizeinvalidusermsg") Is Nothing, "Cannot Proceed - Unable to recommend/authorise claim payment raised by yourself", GetLocalResourceObject("authorizeinvalidusermsg")), String)
                        cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstClaimPayment)
                        Exit Sub
                    ElseIf LCase(sRecommendBy) = LCase(Session(CNLoginName)) Then
                        cstClaimPayment.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("declineinvalidusermsg") Is Nothing, "You are either the persion who entered this payment or you have authorized the previous step. You cannot authorize 2 steps on the same payment.", GetLocalResourceObject("declineinvalidusermsg")), String)
                        cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstClaimPayment)
                        Exit Sub
                    ElseIf dPaymentAmount > dAuthorityAmount Then
                        cstClaimPayment.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("amountvalidationmsg") Is Nothing, "You are not within your recommend/authorise limits for this payment type", GetLocalResourceObject("amountvalidationmsg")), String)
                        cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstClaimPayment)
                        Exit Sub
                    ElseIf bIsReferredforRecommend = True And sRecommendBy.Replace("&nbsp;", "").Trim() = String.Empty Then
                        cstClaimPayment.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("pendingrecommendationmsg") Is Nothing, "Cannot Proceed - Claim payment needs to be recommended first", GetLocalResourceObject("pendingrecommendationmsg")), String)
                        cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstClaimPayment)
                        Exit Sub
                    End If

                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    Dim DuplicateClaimPayment As Boolean = False
                    Dim oClaimOpenNew As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    Dim oClaimPaymentCollection As NexusProvider.ClaimPaymentCollection
                    If oPortal.DuplicateClaimPaymentCheckParameters IsNot Nothing Then
                        If oPortal.DuplicateClaimPaymentCheckParameters.Trim.Length <> 0 Then
                            oWebservice = New NexusProvider.ProviderManager().Provider

                            Dim oClaimVersions1 As NexusProvider.VersionsCollections = Nothing
                            oClaimVersions1 = oWebservice.GetVersionsForClaim(sClaimNumber)

                            Dim sParameters() As String
                            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                            If oClaimOpen IsNot Nothing Then
                                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                                oClaimPaymentCollection = oClaimOpen.ClaimPeril(iPeril).ClaimPayment
                                For iPerilout As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1

                                    For iPerilReserve As Integer = 0 To oClaimOpen.ClaimPeril(iPerilout).ClaimPayment.Count - 1
                                        If oClaimOpen.ClaimPeril(iPerilout).ClaimPayment(iPerilReserve).BaseAmount > 0 Then
                                            'oClaimPaymentCollection = oClaimPaymentCollection.Add(oClaimOpen.ClaimPeril(i).ClaimPayment)
                                            oClaimPaymentCollection = oClaimOpen.ClaimPeril(iPerilout).ClaimPayment
                                            Exit For
                                        End If
                                    Next
                                Next


                            End If
                            Dim iCollectionCnt As Integer = oClaimPaymentCollection.Count - 1


                            sParameters = oPortal.DuplicateClaimPaymentCheckParameters.Split(",")


                            Dim iprmcnt As Integer = 0
                            For iCounterPRM As Integer = 0 To sParameters.Length - 1
                                For iCounterinner = 0 To oClaimVersions1.Count - 1
                                    If oClaimVersions1(iCounterinner).ClaimKey <> nClaimKey AndAlso oClaimVersions1(iCounterinner).TransactionType = "Pay Claim" Then
                                        Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                                        Dim previousQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
                                        Dim spreviousBranchCode As String
                                        If previousQuote IsNot Nothing Then
                                            spreviousBranchCode = previousQuote.BranchCode
                                        End If

                                        'Retreiving the claim details details
                                        oClaimDetails = GetClaimDetailsCall(oClaimVersions1(iCounterinner).ClaimKey, sBranchCode, 0)

                                        For iCounterinner1 = 0 To oClaimDetails.ClaimPeril.Count - 1

                                            Dim oPreviousClaimPaymentCollection As NexusProvider.ClaimPaymentCollection
                                            oPreviousClaimPaymentCollection = oClaimDetails.ClaimPeril(iCounterinner1).ClaimPayment

                                            If oPreviousClaimPaymentCollection.Count > 0 Then

                                                For iCounterinner2 = 0 To oClaimDetails.ClaimPeril(iCounterinner1).ClaimPayment.Count - 1
                                                    If sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "PAYEENAME" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Name IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Name IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Name.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Name.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "PAYEENAME", oClaimPaymentCollection.Item(iCollectionCnt).Payee.Name.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "ACCNAME" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Name IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Name IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Name.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Name.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "ACCNAME", oClaimPaymentCollection.Item(iCollectionCnt).Payee.Name.Trim.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "GRSPAYAMOUNT" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount > 0 Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentAmount > 0 Then
                                                                If (oClaimPaymentCollection.Item(iCollectionCnt).TaxAmount) = oPreviousClaimPaymentCollection.Item(iCounterinner2).TaxAmount Then
                                                                    If (oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount) = oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentAmount Then

                                                                        If (oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount) + (oClaimPaymentCollection.Item(iCollectionCnt).TaxAmount) = (oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentAmount + oPreviousClaimPaymentCollection.Item(iCounterinner2).TaxAmount) Then
                                                                            DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "GRSPAYAMOUNT", oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount + oClaimPaymentCollection.Item(iCollectionCnt).TaxAmount)
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "ADD1" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address1 IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address1 IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address1.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address1.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "ADD1", oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address1.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "LOSSCURRENCY" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).LossCurrencyCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).LossCurrencyCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).LossCurrencyCode = oPreviousClaimPaymentCollection.Item(iCounterinner2).LossCurrencyCode Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "LOSSCURRENCY", oClaimPaymentCollection.Item(iCollectionCnt).LossCurrencyCode)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "PAYCURRENCY" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).CurrencyCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).CurrencyCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).CurrencyCode = oPreviousClaimPaymentCollection.Item(iCounterinner2).CurrencyCode Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "PAYCURRENCY", oClaimPaymentCollection.Item(iCollectionCnt).CurrencyCode)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "ACCNUMBER" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankNumber IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankNumber IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankNumber.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankNumber.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "ACCNUMBER", oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankNumber.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "BANKCODE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankCode.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankCode.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "BANKCODE", oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankCode.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "BANKBRANCH" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankCode.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankCode.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "BANKBRANCH", oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankCode.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "BANKNAME" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankName IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankName IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankName.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BankName.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "BANKNAME", oClaimPaymentCollection.Item(iCollectionCnt).Payee.BankName.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "MEDIATYPE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.MediaTypeCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.MediaTypeCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.MediaTypeCode = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.MediaTypeCode Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "MEDIATYPE", oClaimPaymentCollection.Item(iCollectionCnt).Payee.MediaTypeCode.Trim)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "PAYDATE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).PaymentDate.ToShortDateString() IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentDate.ToShortDateString() IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).PaymentDate.ToString("dd/MM/yyyy") = oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentDate.ToString("dd/MM/yyyy") Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "PAYDATE", oClaimPaymentCollection.Item(iCollectionCnt).PaymentDate.ToString("dd/MM/yyyy"))
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "THEIRREF" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.TheirReference IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.TheirReference IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.TheirReference.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.TheirReference.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "THEIRREF", oClaimPaymentCollection.Item(iCollectionCnt).Payee.TheirReference.ToUpper)
                                                                End If
                                                            End If
                                                        End If

                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "PARTYTYPE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).PartyPaidName = "Claim Payable" Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).PartyPaidName IsNot Nothing And oPreviousClaimPaymentCollection.Item(iCounterinner2).BaseAmount > 0 Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).PartyPaidName.Trim.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).PartyPaidName.Trim.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "PARTYTYPE", oClaimPaymentCollection.Item(iCollectionCnt).PartyPaidName.Trim.ToUpper)
                                                                End If
                                                            End If
                                                        Else
                                                            If oClaimPaymentCollection.Item(iCollectionCnt).PartyPaidCode IsNot Nothing Then
                                                                If oPreviousClaimPaymentCollection.Item(iCounterinner2).PartyPaidCode IsNot Nothing Then
                                                                    If oClaimPaymentCollection.Item(iCollectionCnt).PartyPaidCode.Trim.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).PartyPaidCode.Trim.ToUpper Then
                                                                        DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "PARTYTYPE", oClaimPaymentCollection.Item(iCollectionCnt).PartyPaidCode.Trim.ToUpper)
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "ULTIMATEPAYEE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).UltimatePayee IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).UltimatePayee IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).UltimatePayee.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).UltimatePayee.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "ULTIMATEPAYEE", oClaimPaymentCollection.Item(iCollectionCnt).UltimatePayee.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "ACCOUNTTYPE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.PartyBankKey > 0 Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.PartyBankKey > 0 Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.PartyBankKey = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.PartyBankKey Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "ACCOUNTTYPE", oClaimPaymentCollection.Item(iCollectionCnt).Payee.PartyBankKey)
                                                                End If
                                                            End If
                                                        Else
                                                            DuplicateClaimPayment = True
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "NETPAYAMOUNT" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount > 0 Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentAmount > 0 Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount = oPreviousClaimPaymentCollection.Item(iCounterinner2).PaymentAmount Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "NETPAYAMOUNT", oClaimPaymentCollection.Item(iCollectionCnt).PaymentAmount)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "MEDIAREF" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.MediaReference IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.MediaReference IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.MediaReference.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.MediaReference.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "MEDIAREF", oClaimPaymentCollection.Item(iCollectionCnt).Payee.MediaReference.ToUpper)
                                                                End If
                                                            End If
                                                        End If

                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "OURREF" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).OurRef IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).OurRef IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).OurRef.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).OurRef.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "OURREF", oClaimPaymentCollection.Item(iCollectionCnt).OurRef.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "BIC" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BIC IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BIC IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.BIC.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.BIC.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "BIC", oClaimPaymentCollection.Item(iCollectionCnt).Payee.BIC.ToUpper)
                                                                End If
                                                            End If
                                                        Else
                                                            DuplicateClaimPayment = True
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "IBAN" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.IBAN IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.IBAN IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.IBAN.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.IBAN.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "IBAN", oClaimPaymentCollection.Item(iCollectionCnt).Payee.IBAN.ToUpper)
                                                                End If
                                                            End If
                                                        Else
                                                            DuplicateClaimPayment = True
                                                        End If

                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "POSTCODE" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.PostCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.PostCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.PostCode.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.PostCode.ToUpper Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "POSTCODE", oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.PostCode.ToUpper)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "COUNTRY" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.CountryCode IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.CountryCode IsNot Nothing Then
                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.CountryCode.Trim() = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.CountryCode.Trim Then
                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "COUNTRY", oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.CountryCode.Trim)
                                                                End If
                                                            End If
                                                        End If
                                                    ElseIf sParameters(iCounterPRM).Trim.Length <> 0 And sParameters(iCounterPRM).Trim.ToUpper = "COMPLETEADD" Then
                                                        If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address1 IsNot Nothing Then
                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address2 IsNot Nothing Then
                                                                If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address3 IsNot Nothing Then
                                                                    If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address4 IsNot Nothing Then
                                                                        If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.PostCode IsNot Nothing Then
                                                                            If oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.CountryCode IsNot Nothing Then
                                                                                If oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address1.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address2.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address3.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address4.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.PostCode.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.CountryCode.ToUpper = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address1.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address2.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address3.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address4.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.PostCode.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.CountryCode.ToUpper Then
                                                                                    ViewState("COMPLETEADD") = oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address1.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address2.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address3.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.Address4.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.PostCode.ToUpper + oPreviousClaimPaymentCollection.Item(iCounterinner2).Payee.Address.CountryCode.ToUpper.Trim
                                                                                    DuplicateClaimPayment = MatchDuplicateClaimPayment(oPreviousClaimPaymentCollection.Item(iCounterinner2), "COMPLETEADD", oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address1.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address2.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address3.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.Address4.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.PostCode.ToUpper + oClaimPaymentCollection.Item(iCollectionCnt).Payee.Address.CountryCode.ToUpper.Trim)
                                                                                End If
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                            End If
                                        Next
                                        If DuplicateClaimPayment = True Then
                                            iprmcnt += 1
                                            DuplicateClaimPayment = False
                                            Exit For
                                        End If
                                    End If
                                Next
                                If iprmcnt = sParameters.Length Then
                                    DuplicateClaimPayment = True
                                    Exit For
                                End If
                            Next

                        End If
                    End If

                    If DuplicateClaimPayment = True Then
                        Session(CNDuplicateClaimPayment) = True
                        Session(CNDuplicateClaimPaymentReason) = "DuplicateClaimPayment"
                        Dim sURL As String
                        If HttpContext.Current.Session.IsCookieless Then
                            sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/DuplicateClaimPaymentWarningMessage.aspx?modal=true&RequestBy=Authorise&Riskcheck=true&KeepThis=true&TB_iframe=true&height=200&width=200"
                        Else
                            sURL = AppSettings("WebRoot") & "/Modal/DuplicateClaimPaymentWarningMessage.aspx?modal=true&RequestBy=Authorise&Riskcheck=true&KeepThis=true&TB_iframe=true&height=200&width=200"
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "PaymentDetails", "tb_show(null , '" & sURL & "' , null);", True)
                        Exit Sub
                    End If
                    AuthoriseClaimPayment()
                ElseIf e.CommandName = "Recommend Decline" Then
                    'set the mode
                    Session(CNMode) = Mode.DeclinePayment
                    'check user payment authority
                    If hMultiStepApproval.Value = "1" Then
                        Dim dAuthorityAmount As Double
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        Try
                            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                            Dim oUserAuthority As New NexusProvider.UserAuthority
                            oUserAuthority.UserCode = CType(Session(CNLoginName), String)

                            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsRecommender
                            oWebservice.GetUserAuthorityValue(oUserAuthority)
                            dAuthorityAmount = oUserAuthority.UserAuthorityOptionalValue2
                        Catch ex As System.Exception
                        Finally
                            oWebservice = Nothing
                        End Try

                        If LCase(sCreatedBy) = LCase(Session(CNLoginName)) Then
                            cstClaimPayment.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("authorizeinvalidusermsg") Is Nothing, "Cannot Proceed - Unable to recommend/authorise claim payment raised by yourself", GetLocalResourceObject("authorizeinvalidusermsg")), String)
                            cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstClaimPayment)
                            Exit Sub
                        ElseIf dPaymentAmount > dAuthorityAmount Then
                            cstClaimPayment.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("amountvalidationmsg") Is Nothing, "You are not within your recommend/authorise limits for this payment type", GetLocalResourceObject("amountvalidationmsg")), String)
                            cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstClaimPayment)
                            Exit Sub
                        End If
                    Else

                    End If
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    If oPortal.ForceToViewClaimPayment = False Then
                        'open new form for comments
                        Response.Redirect("ClaimPaymentDoc.aspx?FromPage=ACP&ClaimPaymentKey=" & nClaimPaymentKey & "&ProductCode=" & oQuote.ProductCode & "&FunctionalArea=4", False)
                    Else
                        'call openclaimandredirect
                        OpenClaimAndRedirect(nClaimKey, sPolicyNumber, dPaymentDate)
                    End If
                ElseIf e.CommandName = "Authorise Decline" Then
                    'set the mode
                    Session(CNMode) = Mode.DeclinePayment
                    If hMultiStepApproval.Value = "1" Then
                        Dim dAuthorityAmount As Double
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        Try
                            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                            Dim oUserAuthority As New NexusProvider.UserAuthority
                            oUserAuthority.UserCode = CType(Session(CNLoginName), String)

                            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
                            oWebservice.GetUserAuthorityValue(oUserAuthority)
                            dAuthorityAmount = oUserAuthority.UserAuthorityOptionalValue2
                        Catch ex As System.Exception
                        Finally
                            oWebservice = Nothing
                        End Try
                        'check user payment authority
                        If LCase(sCreatedBy) = LCase(Session(CNLoginName)) Then
                            cstClaimPayment.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("authorizeinvalidusermsg") Is Nothing, "Cannot Proceed - Unable to recommend/authorise claim payment raised by yourself", GetLocalResourceObject("authorizeinvalidusermsg")), String)
                            cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstClaimPayment)
                            Exit Sub
                        ElseIf dPaymentAmount > dAuthorityAmount Then
                            cstClaimPayment.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstClaimPayment.ErrorMessage = CType(IIf(GetLocalResourceObject("amountvalidationmsg") Is Nothing, "You are not within your recommend/authorise limits for this payment type", GetLocalResourceObject("amountvalidationmsg")), String)
                            cstClaimPayment.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstClaimPayment)
                            Exit Sub
                        End If
                    Else

                    End If
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    If oPortal.ForceToViewClaimPayment = False Then
                        'open new form for comments
                        Response.Redirect("ClaimPaymentDoc.aspx?FromPage=ACP&ClaimPaymentKey=" & nClaimPaymentKey & "&ProductCode=" & oQuote.ProductCode & "&FunctionalArea=4", False)
                    Else
                        'call openclaimandredirect
                        OpenClaimAndRedirect(nClaimKey, sPolicyNumber, dPaymentDate)
                    End If
                End If

            End If
        End Sub
        Protected Function MatchDuplicateClaimPayment(ByVal oClaimPayment As NexusProvider.ClaimPayment, ByVal searchParameter As String, ByVal searchValue As String) As Boolean

            Select Case searchParameter
                Case "ACCNAME"
                    If oClaimPayment.Payee.Name.Trim().ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False
                    End If
                Case "ACCNUMBER"
                    If oClaimPayment.Payee.BankNumber.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If
                Case "BANKCODE"
                    If oClaimPayment.Payee.BankCode.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If

                Case "BANKBRANCH"
                    If oClaimPayment.Payee.BankCode.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If

                Case "BANKNAME"
                    If oClaimPayment.Payee.BankName.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If
                Case "GRSPAYAMOUNT"
                    If (oClaimPayment.PaymentAmount) + (oClaimPayment.TaxAmount) = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "MEDIATYPE"
                    If oClaimPayment.Payee.MediaTypeCode.Trim() = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
                Case "PAYDATE"
                    If Convert.ToDateTime(oClaimPayment.PaymentDate.ToShortDateString()).ToShortDateString() = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "PAYEENAME"
                    If oClaimPayment.Payee.Name.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If


                Case "THEIRREF"
                    If oClaimPayment.Payee.TheirReference.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If


                Case "PARTYTYPE"

                    If searchValue = "CLAIM PAYABLE" Then
                        If oClaimPayment.PartyPaidName.Trim.ToUpper = searchValue.ToUpper Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        If oClaimPayment.PartyPaidCode.Trim.ToUpper = searchValue.ToUpper Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Case "ULTIMATEPAYEE"
                    If oClaimPayment.UltimatePayee.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If


                Case "NETPAYAMOUNT"
                    If oClaimPayment.PaymentAmount = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "PAYCURRENCY"
                    If oClaimPayment.CurrencyCode = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "LOSSCURRENCY"
                    If oClaimPayment.LossCurrencyCode = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "ACCOUNTTYPE"
                    If oClaimPayment.Payee.PartyBankKey = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "MEDIAREF"
                    If oClaimPayment.Payee.MediaReference.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If
                Case "OURREF"
                    If oClaimPayment.OurRef.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If


                Case "BIC"
                    If oClaimPayment.Payee.BIC.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If


                Case "IBAN"
                    If oClaimPayment.Payee.IBAN.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If
                Case "ADD1"
                    If oClaimPayment.Payee.Address.Address1.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If

                Case "POSTCODE"
                    If oClaimPayment.Payee.Address.PostCode.ToUpper = searchValue.ToUpper Then
                        Return True
                    Else
                        Return False

                    End If

                Case "COUNTRY"
                    If oClaimPayment.Payee.Address.CountryCode.Trim() = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
                Case "COMPLETEADD"
                    If ViewState("COMPLETEADD") = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

            End Select
        End Function

        Protected Sub AuthoriseClaimPayment()
            Dim sPolicyNumber As String = String.Empty
            Dim nClaimKey As Integer
            Dim dPaymentDate As Date
            Dim nClaimPaymentKey As Integer
            Dim sClaimNumber As String = String.Empty
            Dim sCreatedBy As String = String.Empty
            Dim dPaymentAmount As Double
            Dim sFailureReason As String = String.Empty
            Dim cstClaimPayment As New CustomValidator
            Dim sRecommendBy As String = String.Empty
            Dim oCurrency As New NexusProvider.Currency
            Dim oQuote As NexusProvider.Quote = Nothing
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())

            Session(CNDuplicateClaimPayment) = True
            Session(CNDuplicateClaimPaymentReason) = hdnDuplicateClaimPaymentReason.Value
            nClaimPaymentKey = ViewState("nClaimPaymentKey")
            sClaimNumber = ViewState("sClaimNumber")
            dPaymentDate = ViewState("dPaymentDate")
            dPaymentAmount = ViewState("dPaymentAmount")
            If oPortal.ForceToViewClaimPayment = False Then
                'open new form for comments
                Response.Redirect("ClaimPaymentDoc.aspx?FromPage=ACP&ClaimPaymentKey=" & nClaimPaymentKey & "&ClaimNumber=" & sClaimNumber & "&PaymentDate=" & dPaymentDate & "&PaymentAmount=" & dPaymentAmount & "&ProductCode=" & Session(CNProductCode) & "&FunctionalArea=4", False)
            Else
                'call openclaimandredirect
                OpenClaimAndRedirect(nClaimKey, sPolicyNumber, dPaymentDate)
            End If
        End Sub

        ''' <summary>
        ''' Authorise Claim Payment Grid Row Data Bound event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthoriseclaimpayments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvAuthoriseclaimpayments.RowDataBound
            If e.Row.RowType = DataControlRowType.Header Then
                grdvAuthoriseclaimpayments.Columns(0).Visible = True
                grdvAuthoriseclaimpayments.Columns(1).Visible = True
                grdvAuthoriseclaimpayments.Columns(11).Visible = True
                grdvAuthoriseclaimpayments.Columns(12).Visible = True

                If UserCanDoTask("ClaimPaymentAuthorisation") Then
                    grdvAuthoriseclaimpayments.Columns(m_nLinkButtonsGridColumn).Visible = True
                Else
                    grdvAuthoriseclaimpayments.Columns(m_nLinkButtonsGridColumn).Visible = False
                End If


                If bDisplayCaseOption Then
                    grdvAuthoriseclaimpayments.Columns(2).Visible = True
                Else
                    grdvAuthoriseclaimpayments.Columns(2).Visible = False
                End If
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then

                If String.IsNullOrEmpty(sClaimNumber) Or sClaimNumber <> e.Row.Cells(3).Text.Trim Then
                    sClaimNumber = e.Row.Cells(3).Text.Trim
                    nClaimID = Convert.ToInt32(e.Row.Cells(0).Text.Trim)
                    nCounter = 0
                Else
                    nCounter = nCounter + 1
                End If

                'set the visibility of links in grid as per user authorities
                Dim lnkDeclineButton As LinkButton = CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("lnkDecline"), LinkButton)
                Dim lnkAuthoriseButton As LinkButton = CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("lnkAuthorise"), LinkButton)
                Dim lnkRecommedButton As LinkButton = CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("lnkRecommend"), LinkButton)
                If e.Row.Cells(3).Text.Trim = sClaimNumber And nClaimID < e.Row.Cells(0).Text.Trim Then
                    Dim sDeclineMessage As String = CType(GetLocalResourceObject("msg_InvalidDecline"), String)
                    Dim sAuthoriseMessage As String = CType(GetLocalResourceObject("msg_InvalidAuthorise"), String)
                    Dim sRecommendMessage As String = CType(GetLocalResourceObject("msg_InvalidRecommed"), String)
                    sDeclineMessage = sDeclineMessage.Replace("##", nCounter.ToString)
                    sAuthoriseMessage = sAuthoriseMessage.Replace("##", nCounter.ToString)
                    sRecommendMessage = sRecommendMessage.Replace("##", nCounter.ToString)
                    lnkDeclineButton.Attributes.Add("onclick", "alert('" & sDeclineMessage & "');return false;")
                    lnkAuthoriseButton.Attributes.Add("onclick", "alert('" & sAuthoriseMessage & "');return false;")
                    'lnkRecommedButton.Attributes.Add("onclick", "alert('" & sRecommendMessage & "');return false;")
                Else
                    lnkDeclineButton.Attributes.Add("onclick", CType(("return confirm('" & GetLocalResourceObject("msg_DeclineConfirmation") & "');"), String))
                    lnkAuthoriseButton.Attributes.Add("onclick", CType(("return confirm('" & GetLocalResourceObject("lbl_AuthorizeMsg") & "');"), String))
                End If

                CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liDecline"), HtmlGenericControl).Visible = True
                CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liView"), HtmlGenericControl).Visible = True
                'set the visibility of links in grid as per user authorities
                If sIsAuthoriser.Value = "1" Then
                    CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liAuthorise"), HtmlGenericControl).Visible = True
                    lnkDeclineButton.CommandName = "Authorise Decline"
                    'If pending recommendation do not show authorize button
                    If e.Row.Cells(m_nIsReferredGridColumn).Text.Trim() = "True" And e.Row.Cells(m_nRecommendedByGridColumn).Text.Replace("&nbsp;", "").Trim() = String.Empty Then
                        CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liAuthorise"), HtmlGenericControl).Visible = False
                    End If
                Else
                    CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liAuthorise"), HtmlGenericControl).Visible = False
                End If
                If sIsRecommender.Value = "1" Then
                    CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liRecommend"), HtmlGenericControl).Visible = False
                    lnkDeclineButton.CommandName = "Recommend Decline"
                    'If pending recommendation only then show the recommend button
                    If e.Row.Cells(m_nIsReferredGridColumn).Text.Trim() = "True" And e.Row.Cells(m_nRecommendedByGridColumn).Text.Replace("&nbsp;", "").Trim() = String.Empty Then
                        CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liRecommend"), HtmlGenericControl).Visible = True
                    End If
                Else
                    CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liRecommend"), HtmlGenericControl).Visible = False
                End If

                'set the visibility of view link by checking ForceToViewClaimPayment configuration
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                Dim oItem As NexusProvider.CashListItems = CType(e.Row.DataItem, NexusProvider.CashListItems)
                Try
                    If oPortal.ForceToViewClaimPayment = False Then
                        CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liView"), HtmlGenericControl).Visible = True
                    Else
                        CType(e.Row.Cells(m_nLinkButtonsGridColumn).FindControl("liView"), HtmlGenericControl).Visible = False
                    End If

                    e.Row.Cells(8).Text = New Money(oItem.PaymentAmount, oItem.CurrencyCode).Formatted

                Catch ex As System.Exception
                Finally
                    oItem = Nothing
                    oNexusConfig = Nothing
                    oPortal = Nothing
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Method to Open the Claim and redirect to the overview page
        ''' </summary>
        ''' <param name="iClaimKey"></param>
        ''' <param name="sPolicyNumber"></param>
        ''' <param name="dPaymentDate"></param>
        ''' <remarks></remarks>
        Private Sub OpenClaimAndRedirect(ByVal iClaimKey As Integer, ByVal sPolicyNumber As String, ByVal dPaymentDate As Date)

            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oBaseParty As NexusProvider.BaseParty = Nothing
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oQuote.BranchCode

            Try
                If oQuote IsNot Nothing Then
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                    Dim oOptionType As New NexusProvider.OptionTypeSetting
                    oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)

                    'Checking of the Claim Builder
                    If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                    AndAlso oOptionType.OptionValue = "1" Then
                        Session(CNClaimBuilder) = True
                        'get the claim risk details and save to session
                        oClaimRisk = oWebservice.GetClaimRisk(oOpenClaim.BaseClaimKey, oOpenClaim.ClaimKey, sBranchCode)
                        Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                        Session(CNDataSet) = oClaimRisk.XMLDataSet
                    End If

                    oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                    Session.Item(CNParty) = oBaseParty
                    Session.Item(CNRenewalDate) = oQuote.RenewalDate
                    Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                    Session(CNInsurer_Header) = oQuote.InsuredName
                    Session(CNProductCode) = oQuote.ProductCode
                    Session(CNClaimQuote) = oQuote
                    Session.Item(CNInsuranceFileKey) = oQuote.InsuranceFileKey
                    Session.Item(CNPolicyNumber) = oQuote.InsuranceFileRef
                    'set the session for parent page
                    Session(CNParentPage) = "ACP"
                    'redirect to overview page
                    Response.Redirect("~/Claims/Overview.aspx", False)
                    oClaimRisk = Nothing
                End If
            Finally
                oOpenClaim = Nothing
                oBaseParty = Nothing
                oQuote = Nothing
                oWebservice = Nothing
                oClaimDetails = Nothing
                oUserDetails = Nothing
            End Try
        End Sub

        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            txtClaimReference.Text = String.Empty
            txtClaimReferenceKey.Value = String.Empty
            txtClient.Text = String.Empty
            txtClientKey.Value = String.Empty
            txtCreatedBy.Text = String.Empty
            txtCreatedByKey.Value = String.Empty
            txtPaymentDate.Text = String.Empty
            txtPolicy.Text = String.Empty
            txtPolicyKey.Value = String.Empty
            txtCaseReference.Text = String.Empty
            txtPayeeName.Text = String.Empty
            ddlBranch.SelectedIndex = 0
            grdvAuthoriseclaimpayments.Visible = False
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnClaimReference.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Claims/FindClaim.aspx?Page=AP&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=800' , null);return false;"
                btnPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Claims/FindInsuranceFile.aspx?Page=RS&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=800' , null);return false;"
                btnCreatedBy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindUser.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=700' , null);return false;"
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
                btnCaseNumber.OnClientClick = "tb_show(null ,'../Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&TB_iframe=true&height=550&width=750' , null);return false;"
            Else
                btnClaimReference.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Claims/FindClaim.aspx?Page=AP&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=800' , null);return false;"
                btnPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Claims/FindInsuranceFile.aspx?Page=RS&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=800' , null);return false;"
                btnCreatedBy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindUser.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=700' , null);return false;"
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
                btnCaseNumber.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Claims/FindCase.aspx?modal=true&KeepThis=true&Page=EL&FindCase=1&TB_iframe=true&height=550&width=750' , null);return false;"
            End If
        End Sub
        ''' <summary>
        ''' In Page Initializing event following activites are done
        ''' 1. Bind ddlBranch with sorted branch collection.
        ''' 2. If ‘single branch is assigned to user , then control should be disabled. 
        ''' 3. Default Branch should be ‘picked from resource file.
        '''
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)

            If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue IsNot Nothing AndAlso oOptionSettings.OptionValue(0) <> "0" Then
                liCaseNumber.Visible = True
                bDisplayCaseOption = True
            End If

            'Fill Btranches
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oBranchCollection As New NexusProvider.BranchCollection
            oBranchCollection = oUserDetails.ListOfBranches
            If oBranchCollection.Count > 1 Then
                'Sort the branches
                oBranchCollection.SortColumn = "Description"
                oBranchCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                oBranchCollection.Sort()
            End If

            'Add a default item
            ddlBranch.DataSource = oBranchCollection
            ddlBranch.DataTextField = "Description"
            ddlBranch.DataValueField = "Code"
            ddlBranch.DataBind()

            'Add a default item for (All Branches)
            ddlBranch.Items.Insert(0, "(All Branches)")

            'If single branch is assigned to user, then control should be disabled
            If oBranchCollection.Count = 1 Then
                ddlBranch.Enabled = False
                ddlBranch.SelectedIndex = 1
            End If

            If oBranchCollection.Count > 1 Then
                ddlBranch.Enabled = True
                'HeadOffice is not transaction branch.
                'So if user have selected any other branch then it should be default selected
                If Session(CNBranchCode) IsNot Nothing AndAlso CType(Session(CNBranchCode), String).ToUpper <> "HEADOFF" Then
                    ddlBranch.SelectedValue = CType(Session(CNBranchCode), String).ToUpper
                End If
            End If

        End Sub
        ''' <summary>
        ''' sort the Authorise claim payments grid according to column click.
        ''' we need to store the current sort order in viewstate, and reverse it each time.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthoriseclaimpayments_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvAuthoriseclaimpayments.Sorting

            Dim oCashListItems As NexusProvider.CashListItemsCollection = Cache.Item(ViewState("AuthoriseClaimPaymentspageCacheID"))
            oCashListItems.SortColumn = e.SortExpression
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
            oCashListItems.SortingOrder = _sortDirection
            oCashListItems.Sort()
            grdvAuthoriseclaimpayments.Columns(0).Visible = True
            grdvAuthoriseclaimpayments.Columns(1).Visible = True
            CType(sender, GridView).DataSource = oCashListItems
            CType(sender, GridView).DataBind()
            grdvAuthoriseclaimpayments.Columns(0).Visible = False
            grdvAuthoriseclaimpayments.Columns(1).Visible = False

        End Sub
    End Class

End Namespace
