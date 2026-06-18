Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants.Session
Imports Nexus.Constants
Imports Nexus

Namespace Nexus
    Partial Class Claims_CheckUnPaidStatus
        Inherits CMS.Library.Frontend.clsCMSPage

        Dim PolictransactionsCacheID As Guid
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            grdPolicyTransactions.AllowPaging = True
            grdPolicyTransactions.PageSize = 10
            grdPolicyTransactions.PageIndex = 0

            If Not Page.IsPostBack Then
                'population of the grid with Policy OutStandingTransactions claim
                PolictransactionsCacheID = Guid.NewGuid()
                ViewState.Add("PolictransactionsCacheID", PolictransactionsCacheID.ToString)
                ViewState.Add("PolictransactionsCacheID", PolictransactionsCacheID.ToString)

                chkIsShowOutstandingOnly.Checked = True
                If Session(CNClaim) IsNot Nothing Then
                    BindPolicyTransactions()

                End If
            End If

        End Sub
        Private Function BindPolicyTransactions() As NexusProvider.TransactionCollection
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oCheckUnPaidDetails As New NexusProvider.CheckUnPaidDetails
            Dim otempPolicyTransactions As New NexusProvider.TransactionCollection
            Dim sPolicyref As String = ""
            Dim sClaimnumber As String = ""

            If Cache.Item(ViewState("PolictransactionsCacheID")) IsNot Nothing Then
                otempPolicyTransactions = CType(Cache.Item(ViewState("PolictransactionsCacheID")), NexusProvider.TransactionCollection)
            End If

            If otempPolicyTransactions IsNot Nothing AndAlso otempPolicyTransactions.Count > 0 Then
                oCheckUnPaidDetails.PolicyTransactions = otempPolicyTransactions
                oCheckUnPaidDetails.InstalmentOverdue= hdndueinstalments.value
              
            Else
                If Session(CNPolicyNumber) IsNot Nothing Then
                    sPolicyref = Session(CNPolicyNumber)
                End If

                If oOpenClaim IsNot Nothing Then
                    sClaimnumber = oOpenClaim.ClaimNumber
                End If
                'Make call to get the Policy Transactions
                oCheckUnPaidDetails = oWebservice.CheckUnpaidPremium(v_sInsuranceRef:=sPolicyref, sClaimNumber:=sClaimnumber)
                hdndueinstalments.value=oCheckUnPaidDetails.InstalmentOverdue
                'Add Transaction details to cache
                Cache.Insert(ViewState("PolictransactionsCacheID"), oCheckUnPaidDetails.PolicyTransactions, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            If oCheckUnPaidDetails IsNot Nothing Then

                txtPolicyNo.Text = oQuote.InsuranceFileRef
                txtClaimNumber.Text = oOpenClaim.ClaimNumber

                If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Then
                    If Session(CNLossDate) IsNot Nothing Then
                        txtClaimDate.Text = CDate(Session(CNLossDate)).ToShortDateString
                    End If
                ElseIf CType(Session.Item(CNMode), Mode) = Mode.PayClaim
                    txtClaimDate.Text = (oOpenClaim.LossFromDate).ToShortDateString
                End If

                If oQuote IsNot Nothing Then
                    txtClientName.Text = oQuote.InsuredName
                End If

                txtOverdueInstalments.Text = oCheckUnPaidDetails.InstalmentOverdue
               
                If oCheckUnPaidDetails.PolicyTransactions IsNot Nothing Then
                    Dim oOutstandingPolicyTransactions As New NexusProvider.TransactionCollection
                    If chkIsShowOutstandingOnly.Checked Then

                        For Each otransaction As NexusProvider.Transaction In oCheckUnPaidDetails.PolicyTransactions
                            If otransaction.OutstandingAmount > 0 Then
                                oOutstandingPolicyTransactions.Add(otransaction)
                            End If
                        Next
                        oCheckUnPaidDetails.PolicyTransactions = oOutstandingPolicyTransactions
                        grdPolicyTransactions.DataSource = oOutstandingPolicyTransactions
                        grdPolicyTransactions.DataBind()

                    Else
                        grdPolicyTransactions.DataSource = oCheckUnPaidDetails.PolicyTransactions
                        grdPolicyTransactions.DataBind()
                    End If

                End If
            End If

            Return oCheckUnPaidDetails.PolicyTransactions
        End Function
        Private Sub chkIsShowOutstandingOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsShowOutstandingOnly.CheckedChanged
            BindPolicyTransactions()
        End Sub
        ''' <summary>
        ''' On Next Button. If logged user has overriding authority
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click
            'Overriding the Duplicate Claim

            If CType(Session.Item(CNMode), Mode) = Mode.NewClaim OrElse CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                RedirectOnSubmit()
            End If
        End Sub
        Private Sub btnAbort_Click(sender As Object, e As EventArgs) Handles btnAbort.Click
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Page.Validate()
            If Page.IsValid Then
                oWebservice.DeletAbandonClaim(oOpenClaim.ClaimKey, Nothing)
                If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Then

                    Response.Redirect("~/Claims/findInsuranceFile.aspx", False)
                ElseIf CType(Session.Item(CNMode), Mode) = Mode.PayClaim
                    Response.Redirect("~/Claims/FindClaim.aspx", False)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Redirect to next page based on overview page logic
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub RedirectOnSubmit()
            Dim sURL As String
            If Request.QueryString("Mode") IsNot Nothing Then

                If Request.QueryString("Mode").ToUpper() = "COMPLETE" Then
                    Response.Redirect("~/Claims/Complete.aspx", False)
                ElseIf Request.QueryString("Mode").ToUpper() = "CLAIMBUILDER" Then
                    'Checking of the Claim Builder
                    sURL = CheckClaimBuilder()
                    Response.Redirect(sURL, False)
                End If
            End If

        End Sub

        Private Sub grdPolicyTransactions_Load(sender As Object, e As EventArgs) Handles grdPolicyTransactions.Load
            If grdPolicyTransactions.PageCount = 1 Then
                grdPolicyTransactions.AllowPaging = False
            Else
                grdPolicyTransactions.AllowPaging = True
                grdPolicyTransactions.PageSize = 10
                grdPolicyTransactions.PageIndex = 0
            End If
        End Sub

        Protected Sub grdPolicyTransactions_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPolicyTransactions.PageIndexChanging
            grdPolicyTransactions.PageIndex = e.NewPageIndex
            BindPolicyTransactions()
        End Sub

        Protected Sub grdPolicyTransactions_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdPolicyTransactions.Sorting
            'sort the Payment details according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim otempPolicyTransactions As New NexusProvider.TransactionCollection

            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection

            Try

                If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                    _sortDirection = SortDirection.Descending
                Else
                    _sortDirection = SortDirection.Ascending
                End If

                otempPolicyTransactions = BindPolicyTransactions()

                otempPolicyTransactions.SortColumn = e.SortExpression
                'store the current sortdirection for comparison on the next sort
                ViewState("SortDirection") = _sortDirection
                'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
                ViewState("SortExpression") = e.SortExpression
                otempPolicyTransactions.SortingOrder = _sortDirection
                otempPolicyTransactions.SortObjectType = GetType(NexusProvider.Transaction)
                otempPolicyTransactions.Sort()
                CType(sender, GridView).DataSource = otempPolicyTransactions
                CType(sender, GridView).DataBind()

            Catch ex As Exception
                Throw ex
            Finally

                otempPolicyTransactions = Nothing
                _sortDirection = Nothing
            End Try


        End Sub
    End Class
End Namespace

