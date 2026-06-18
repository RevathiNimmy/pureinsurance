Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class Controls_ClientClaims : Inherits System.Web.UI.UserControl

        Private sUserName As String
        Dim bDisplayCaseOption As Boolean = False

        Public Shared CNSortDirection As String = ""
        Public Shared CNSortExpression As String = ""
        Public Property UserName() As String
            Set(ByVal value As String)
                sUserName = value
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oClaims As NexusProvider.ClaimCollection
                Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria

                Try
                    If value IsNot Nothing Then
                        oClaimSearchCriteria.ClientShortName = value

                        Dim oClaimCol As New NexusProvider.ClaimCollection
                        Dim oClaim As NexusProvider.Claim
                        Dim TempVar As Integer

                        If (Me.chkViewAllClaims.Checked) Then
                            oClaimSearchCriteria.IncludeClosedClaim = True
                            oClaims = oWebService.FindClaim(oClaimSearchCriteria)

                            For TempVar = 0 To oClaims.Count - 1

                                oClaim = New NexusProvider.Claim
                                oClaim = oClaims.Item(TempVar)

                                oClaim.CaseKey = oClaims.Item(TempVar).CaseKey
                                oClaim.CaseNumber = oClaims.Item(TempVar).CaseNumber
                                oClaim.ClaimNumber = oClaims.Item(TempVar).ClaimNumber
                                oClaim.InsuranceRef = oClaims.Item(TempVar).InsuranceRef
                                oClaim.ClientName = oClaims.Item(TempVar).ClientName
                                oClaim.ProductDescription = oClaims.Item(TempVar).ProductDescription
                                oClaim.LossDateFrom = oClaims.Item(TempVar).LossDateFrom
                                If (oClaims.Item(TempVar).ClaimStatusID <> 3 AndAlso oClaims.Item(TempVar).ClaimStatusID <> 5) And oClaims.Item(TempVar).ProgressStatusDescription.ToUpper = "CLOSED" Then
                                    oClaims.Item(TempVar).ProgressStatusDescription = "Open"
                                End If
                                oClaim.ProgressStatusCode = oClaims.Item(TempVar).ProgressStatusDescription
                                oClaimCol.Add(oClaim)
                            Next

                        Else

                            oClaims = oWebService.FindClaim(oClaimSearchCriteria)

                            For TempVar = 0 To oClaims.Count - 1
                                If oClaims.Item(TempVar).ProgressStatusDescription.ToUpper <> "CLOSED" Then
                                    oClaim = New NexusProvider.Claim
                                    oClaim = oClaims.Item(TempVar)
                                    oClaim.ClaimNumber = oClaims.Item(TempVar).ClaimNumber
                                    oClaim.InsuranceRef = oClaims.Item(TempVar).InsuranceRef
                                    oClaim.ClientName = oClaims.Item(TempVar).ClientName
                                    oClaim.ProductDescription = oClaims.Item(TempVar).ProductDescription
                                    oClaim.LossDateFrom = oClaims.Item(TempVar).LossDateFrom
                                    oClaim.ProgressStatusCode = oClaims.Item(TempVar).ProgressStatusDescription
                                    oClaim.ClaimRiskField = oClaims.Item(TempVar).ClaimRiskField
                                    oClaimCol.Add(oClaim)
                                ElseIf oClaims.Item(TempVar).ProgressStatusDescription.ToUpper = "CLOSED" AndAlso oClaims.Item(TempVar).ClaimStatusID <> 3 Then
                                    oClaim = New NexusProvider.Claim
                                    oClaim = oClaims.Item(TempVar)
                                    oClaim.ClaimNumber = oClaims.Item(TempVar).ClaimNumber
                                    oClaim.InsuranceRef = oClaims.Item(TempVar).InsuranceRef
                                    oClaim.ClientName = oClaims.Item(TempVar).ClientName
                                    oClaim.ProductDescription = oClaims.Item(TempVar).ProductDescription
                                    oClaim.LossDateFrom = oClaims.Item(TempVar).LossDateFrom
                                    oClaims.Item(TempVar).ProgressStatusDescription = "Open"
                                    oClaim.ProgressStatusCode = oClaims.Item(TempVar).ProgressStatusDescription
                                    oClaim.ClaimRiskField = oClaims.Item(TempVar).ClaimRiskField
                                    oClaimCol.Add(oClaim)
                                End If
                            Next

                        End If


                        If ViewState("ClaimCollCacheID") Is Nothing Then
                            Dim ClaimCollCacheID As Guid
                            ClaimCollCacheID = Guid.NewGuid()
                            ViewState.Add("ClaimCollCacheID", ClaimCollCacheID.ToString)
                        End If

                        Cache.Insert(ViewState("ClaimCollCacheID"), oClaimCol, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                        grdvClientClaims.Visible = True
                        grdvClientClaims.AllowPaging = True
                        If bDisplayCaseOption Then
                            'Default Search - CaseNumber Descending
                            oClaimCol.SortColumn = "CaseNumber"
                            oClaimCol.SortingOrder = SortDirection.Descending
                            oClaimCol.Sort()
                        End If
                        grdvClientClaims.DataSource = oClaimCol
                        grdvClientClaims.DataBind()
                    End If

                Finally
                    oWebService = Nothing
                    oClaims = Nothing
                    oClaimSearchCriteria = Nothing
                End Try
            End Set
            Get
                Return sUserName
            End Get
        End Property

        Private Sub chkViewAllClaims_CheckedChanged() Handles chkViewAllClaims.CheckedChanged
            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                UserName = oParty.UserName
            End If
        End Sub
        ''' <summary>
        ''' As per system option, make CaseNumber column visible false.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvClientClaims_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvClientClaims.DataBinding
            If bDisplayCaseOption Then
                grdvClientClaims.Columns(0).Visible = True
            Else
                grdvClientClaims.Columns(0).Visible = False
            End If
        End Sub
        Protected Sub grdvClientClaims_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvClientClaims.Load
            If grdvClientClaims.PageCount = 1 Then
                grdvClientClaims.AllowPaging = False
            End If
            'IIf(grdvClientClaims.PageCount = 1, grdvClientClaims.AllowPaging = False, grdvClientClaims.AllowPaging = True)
        End Sub

        Protected Sub grdvClientClaims_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvClientClaims.PageIndexChanging
            Dim oClaims As NexusProvider.ClaimCollection = CType(Cache.Item(ViewState("ClaimCollCacheID")), NexusProvider.ClaimCollection)
            If oClaims Is Nothing OrElse (oClaims IsNot Nothing AndAlso oClaims.Count = 0) Then
                If ViewState("ClaimCollCacheID") Is Nothing Then
                    Dim ClaimCollCacheID As Guid
                    ClaimCollCacheID = Guid.NewGuid()
                    ViewState.Add("ClaimCollCacheID", ClaimCollCacheID.ToString)
                End If

                If Session(CNParty) IsNot Nothing AndAlso String.IsNullOrEmpty(Me.UserName) AndAlso Session(CNClientMode) = Mode.View _
                AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 AndAlso Session(CNIsNewClient) Is Nothing Then
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    UserName = oParty.UserName
                End If
            End If
            If CNSortExpression <> "" Then
                oClaims.SortColumn = CNSortExpression
                oClaims.SortingOrder = CNSortDirection
                oClaims.Sort()
            End If
            grdvClientClaims.DataSource = CType(Cache.Item(ViewState("ClaimCollCacheID")), NexusProvider.ClaimCollection)
            grdvClientClaims.PageIndex = e.NewPageIndex
            grdvClientClaims.DataBind()
        End Sub

        Protected Sub grdvClientClaims_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvClientClaims.RowCommand
            If e.CommandName <> "Sort" Then
                If e.CommandName = "Select" Then
                    If Page.IsValid Then
                        If Not String.IsNullOrEmpty(Request.QueryString("Page")) And Request.QueryString("Page") = "AP" Then
                            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setClaimReference('" + e.CommandArgument.ToString + "');", True)
                        Else
                            If Not String.IsNullOrEmpty(e.CommandArgument) Then
                                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                                Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
                                Dim oQuote As NexusProvider.Quote = Nothing
                                Dim oBaseParty As NexusProvider.BaseParty = Nothing
                                Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
                                Dim sClaimNumber As String = CStr(e.CommandArgument)

                                Try
                                    oClaimVersions = oWebservice.GetVersionsForClaim(sClaimNumber, sBranchCode)
                                    If oClaimVersions IsNot Nothing Then
                                        oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey, sBranchCode)
                                        If oQuote IsNot Nothing Then
                                            Dim oAddress As NexusProvider.Address
                                            oAddress = oWebservice.GetAddress(Nothing, oQuote.PartyKey, Nothing)
                                            Session.Item(CNRisks) = oQuote.Risks
                                            Session.Item(CNRenewalDate) = oQuote.RenewalDate
                                            Session.Item(CNAddress) = oAddress.Address1 & ", " & oAddress.Address4
                                            Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                                            Session(CNInsurer_Header) = oQuote.InsuredName
                                            Session(CNClaimQuote) = oQuote
                                        End If
                                        Session(CNClaimNumber) = sClaimNumber
                                        Session(CNClaimVersion) = oClaimVersions
                                        Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                                        Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                                        Response.Redirect("~/Claims/FindClaim.aspx?ReDirectPage=ClientClaims", False)
                                    End If
                                Finally
                                    oWebservice = Nothing
                                    oUserDetails = Nothing
                                End Try
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Initializing control with system option.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting
            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)

            If oOptionSettings IsNot Nothing AndAlso Not String.IsNullOrEmpty(oOptionSettings.OptionValue) Then
                If oOptionSettings.OptionValue(0) <> "0" Then
                    bDisplayCaseOption = True
                Else
                    bDisplayCaseOption = False
                End If
            Else
                bDisplayCaseOption = False
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        End Sub
        Protected Sub grdvClientClaims_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvClientClaims.Sorting
            'sort the Claim according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oClaimsCollection As NexusProvider.ClaimCollection = CType(Cache.Item(ViewState("ClaimCollCacheID")), NexusProvider.ClaimCollection)
            If (ViewState("ClaimCollCacheID") Is Nothing OrElse Cache.Item(ViewState("ClaimCollCacheID")) Is Nothing) AndAlso Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                UserName = oParty.UserName
            End If

            oClaimsCollection = CType(Cache.Item(ViewState("ClaimCollCacheID")), NexusProvider.ClaimCollection)

            If oClaimsCollection Is Nothing OrElse (oClaimsCollection IsNot Nothing AndAlso oClaimsCollection.Count = 0) Then
                If ViewState("ClaimCollCacheID") Is Nothing Then
                    Dim ClaimCollCacheID As Guid
                    ClaimCollCacheID = Guid.NewGuid()
                    ViewState.Add("ClaimCollCacheID", ClaimCollCacheID.ToString)
                End If

                If Session(CNParty) IsNot Nothing AndAlso String.IsNullOrEmpty(Me.UserName) AndAlso Session(CNClientMode) = Mode.View _
                AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 AndAlso Session(CNIsNewClient) Is Nothing Then
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    UserName = oParty.UserName
                End If
            End If

            oClaimsCollection.SortColumn = e.SortExpression
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
            oClaimsCollection.SortColumn = e.SortExpression
            oClaimsCollection.SortingOrder = _sortDirection
            oClaimsCollection.Sort()
            CType(sender, GridView).DataSource = oClaimsCollection
            CType(sender, GridView).DataBind()
            CNSortDirection = _sortDirection
            CNSortExpression = e.SortExpression
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not IsPostBack AndAlso Me.Visible = True Then
                If ViewState("ClaimCollCacheID") Is Nothing Then
                    Dim ClaimCollCacheID As Guid
                    ClaimCollCacheID = Guid.NewGuid()
                    ViewState.Add("ClaimCollCacheID", ClaimCollCacheID.ToString)
                End If

                Dim oClaims As NexusProvider.ClaimCollection = CType(Cache.Item(ViewState("ClaimCollCacheID")), NexusProvider.ClaimCollection)
                If oClaims Is Nothing Or (oClaims IsNot Nothing AndAlso oClaims.Count = 0) Then
                    If Session(CNParty) IsNot Nothing AndAlso String.IsNullOrEmpty(Me.UserName) AndAlso Session(CNClientMode) = Mode.View _
                    AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 AndAlso Session(CNIsNewClient) Is Nothing Then
                        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                        UserName = oParty.UserName
                    End If
                End If
            End If
        End Sub

        Protected Sub grdvClientClaims_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdvClientClaims.RowDataBound

          If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.FindControl("lblDescription") IsNot Nothing Then

                    Dim oLabelDesc As Label = e.Row.FindControl("lblDescription")
                    Dim sdesccription As String = oLabelDesc.Text
                    If (sdesccription.Length > 30) Then
                        sdesccription = sdesccription.Substring(0, 30)
                    End If

                    e.Row.Attributes.Add("title", sdesccription)
                End If
            End If
        End Sub
    End Class

End Namespace
