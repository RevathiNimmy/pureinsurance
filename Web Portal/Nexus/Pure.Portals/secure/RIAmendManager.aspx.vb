Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Linq

Namespace Nexus

    Partial Class secure_RIAmendManager : Inherits Frontend.clsCMSPage

        Const kPolicyStatusToAccept As String = "Awaiting Update"
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                Dim bUserCanAmendThePolicy As Boolean = False
                If Request.QueryString("ClonedTransfer") = Nothing Then
                    bUserCanAmendThePolicy = UserCanDoTask("PortfolioTransferAmendment")
                Else
                    bUserCanAmendThePolicy = UserCanDoTask("ClonedReinsurancePolicyAmendment")
                End If

                If bUserCanAmendThePolicy = True Then
                    btnAccept.Visible = True
                Else
                    btnAccept.Visible = False
                End If

            End If

            If Request.QueryString("ClonedTransfer") IsNot Nothing Then
                lblPageHeader.Text = CStr(GetLocalResourceObject("lblHeader_CT"))
            Else
                lblPageHeader.Text = CStr(GetLocalResourceObject("lblHeader_PT"))
            End If

            If Not Page.IsPostBack Then
                BindGrid()
            End If
        End Sub

        Protected Sub gvRIAmendManager_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRIAmendManager.DataBound
            If gvRIAmendManager.Rows.Count = 0 Or gvRIAmendManager.PageCount = 1 Then
                gvRIAmendManager.AllowPaging = False
            End If

        End Sub

        ''' <summary>
        ''' TO handle Row Command Event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRIAmendManager_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRIAmendManager.RowCommand
            If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                If e.CommandName = "EditRI" Then
                    ClearQuoteCollectionSessionValues()
                    ClearQuote()
                    ClearClaims()
                    'Call RecalculateRIForCloneTransfer for selected policy and redirect an user to PremiumDisplay.aspx
                    Dim oQuote As NexusProvider.Quote
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    oQuote = oWebService.GetHeaderAndSummariesByKey(Convert.ToInt32((e.CommandArgument)))
                    If Request.QueryString("ClonedTransfer") IsNot Nothing Then
                        Session(CNMode) = Mode.ClonedTransferAmendment
                        'Check if any other lower policy version exists with "Awaiting Amend" status
                        'If yes then do not allow an user to proceed
                        If LowerPolicyVersionExistsUnderAmendment(oQuote.InsuranceFolderKey, oQuote.InsuranceFileKey) = True Then
                            Dim sMessage As String = Convert.ToString(GetLocalResourceObject("msgLowerPolicyVersionPending"))
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LowerPolicyVersionPending", "alert('" + sMessage + "')", True)
                            Exit Sub
                        Else
                            oWebService.RecalculateRIForCloneTransfer(oQuote, oQuote.BranchCode)
                        End If
                    Else
                        Session(CNMode) = Mode.PortFolioTransferAmendment
                        oWebService.RecalculateRIForPortfolioTransfer(oQuote, oQuote.BranchCode)
                    End If

                    Session(CNQuote) = oQuote
                    Session(CNCurrenyCode) = oQuote.CurrencyCode

                    Response.Redirect("RIAmendRiskList.aspx")
                End If
            End If
        End Sub

        ''' <summary>
        ''' To Handle Page index change event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRIAmendManager_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRIAmendManager.PageIndexChanging
            gvRIAmendManager.PageIndex = e.NewPageIndex
            gvRIAmendManager.DataSource = Session(CNSearchResults)
            gvRIAmendManager.DataBind()

            ToggleSelectAllStatus()
        End Sub

        ''' <summary>
        ''' Bind Grid with policies
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindGrid()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPolicies As New NexusProvider.PolicyCollection
            If Request.QueryString("ClonedTransfer") = Nothing Then
                oPolicies = oWebService.GetPTPoliciesForAmend()
            Else
                oPolicies = oWebService.GetClonePoliciesForAmend()
            End If

            If oPolicies IsNot Nothing AndAlso oPolicies.Count > 0 Then
               
                Session(CNSearchResults) = oPolicies

                gvRIAmendManager.Visible = True
                gvRIAmendManager.AllowPaging = True
                gvRIAmendManager.DataSource = oPolicies
                gvRIAmendManager.DataBind()
            Else
                gvRIAmendManager.DataSource = Nothing
                gvRIAmendManager.DataBind()
            End If

        End Sub

        ''' <summary>
        ''' To Enable/Disable checkbox in row if policy has been amended
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRIAmendManager_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRIAmendManager.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim bUserCanAmendThePolicy As Boolean = False
                Dim oPolicy As NexusProvider.Policy = CType(e.Row.DataItem, NexusProvider.Policy)
                Dim lnkbtnAmend As LinkButton = CType(e.Row.FindControl("lnkbtnAmend"), LinkButton)
                Dim chkSelect As CheckBox = CType(e.Row.FindControl("chkSelection"), CheckBox)
                If oPolicy.IsSelected = True Then
                    chkSelect.Checked = True
                End If
                If oPolicy.PTRIStatus <> kPolicyStatusToAccept Then
                    chkSelect.Enabled = False
                End If
                If Request.QueryString("ClonedTransfer") = Nothing Then
                    bUserCanAmendThePolicy = UserCanDoTask("PortfolioTransferAmendment")
                Else
                    bUserCanAmendThePolicy = UserCanDoTask("ClonedReinsurancePolicyAmendment")
                End If
                If bUserCanAmendThePolicy = True And lnkbtnAmend IsNot Nothing Then
                    lnkbtnAmend.Visible = True
                Else
                    lnkbtnAmend.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' For Grid columns sorting
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRIAmendManager_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvRIAmendManager.Sorting
            'sort the entries according to the column clicked
            Dim oPolicies As NexusProvider.PolicyCollection = CType(Session(CNSearchResults), NexusProvider.PolicyCollection)
            oPolicies.SortColumn = e.SortExpression

            'we need to store the current sort order in viewstate, and reverse it each time
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If CType(ViewState("SortDirection"), SortDirection) = SortDirection.Ascending AndAlso Convert.ToString(ViewState("SortExpression")) = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oPolicies.SortingOrder = CType(_sortDirection, NexusProvider.GenericComparer.SortOrder)
            oPolicies.Sort()
            Session(CNSearchResults) = oPolicies
            CType(sender, GridView).DataSource = oPolicies
            CType(sender, GridView).DataBind()
            ToggleSelectAllStatus()
        End Sub

        ''' <summary>
        ''' To Accept the policy
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
            Dim oPolicies As NexusProvider.PolicyCollection = CType(Session(CNSearchResults), NexusProvider.PolicyCollection)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            For Each oPolicy As NexusProvider.Policy In oPolicies
                If oPolicy.IsSelected = True Then
                    Dim oQuote As NexusProvider.Quote
                    oQuote = oWebService.GetHeaderAndSummariesByKey(oPolicy.InsuranceFileKey, oPolicy.BranchCode)
                    If CType(Session(CNMode), Constant.Mode) = Mode.PortFolioTransferAmendment Then
                        oWebService.CreatePostingsForReinsurance(oQuote.InsuranceFileKey, PortfolioTransfer, oQuote.TimeStamp, oQuote.BranchCode)
                    ElseIf CType(Session(CNMode), Constant.Mode) = Mode.ClonedTransferAmendment Then
                        oWebService.CreatePostingsForReinsurance(oQuote.InsuranceFileKey, Cloned, oQuote.TimeStamp, oQuote.BranchCode)
                    End If
                End If
            Next

            BindGrid()
        End Sub

        ''' <summary>
        ''' To Handle SelectAll checkbox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim chkSelectAll As CheckBox = CType(sender, CheckBox)
            Dim oPolicies As NexusProvider.PolicyCollection = CType(Session(CNSearchResults), NexusProvider.PolicyCollection)
            For Each oPolicy As NexusProvider.Policy In oPolicies
                If oPolicy.PTRIStatus = kPolicyStatusToAccept Then
                    oPolicy.IsSelected = chkSelectAll.Checked
                End If
            Next
            Session(CNSearchResults) = oPolicies
            gvRIAmendManager.DataSource = oPolicies
            gvRIAmendManager.DataBind()
            ToggleSelectAllStatus()
        End Sub

        ''' <summary>
        ''' To handle checkbox click from grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim chkSelection As CheckBox = CType(sender, CheckBox)
            Dim chkRowSelect As CheckBox = Nothing
            Dim oSelectedRow As GridViewRow = Nothing
            For Each oRow As GridViewRow In gvRIAmendManager.Rows
                chkRowSelect = CType(oRow.FindControl("chkSelection"), CheckBox)
                If chkRowSelect IsNot Nothing Then
                    If chkSelection.ClientID = chkRowSelect.ClientID Then
                        oSelectedRow = oRow
                        Exit For
                    End If
                End If
            Next

            If oSelectedRow IsNot Nothing Then
                Dim iSelectedInsuranceFileKey As Integer = CInt(gvRIAmendManager.DataKeys(oSelectedRow.RowIndex).Value)
                Dim oSelectedPolicy As NexusProvider.Policy = CType(oSelectedRow.DataItem, NexusProvider.Policy)
                Dim oPolicies As NexusProvider.PolicyCollection = CType(Session(CNSearchResults), NexusProvider.PolicyCollection)
                For Each oPolicy As NexusProvider.Policy In oPolicies
                    If oPolicy.InsuranceFileKey = iSelectedInsuranceFileKey Then
                        oPolicy.IsSelected = chkSelection.Checked
                        Exit For
                    End If
                Next
                Session(CNSearchResults) = oPolicies
            End If
            ToggleSelectAllStatus()
        End Sub

        ''' <summary>
        ''' To Check if all policy versions selected and also to enable Accept button if any row selected
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ToggleSelectAllStatus()
            Dim bAllRecordsSelected As Boolean = False
            Dim chkSelectAll As CheckBox = CType(gvRIAmendManager.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            Dim oPolicies As NexusProvider.PolicyCollection = CType(Session(CNSearchResults), NexusProvider.PolicyCollection)
            Dim oAllRecords = (From obj In oPolicies Where obj.PTRIStatus = kPolicyStatusToAccept Select obj).ToList()
            Dim oSelectedRecords = (From obj In oPolicies Where obj.PTRIStatus = kPolicyStatusToAccept And obj.IsSelected = True Select obj).ToList()
            If oSelectedRecords.Count > 0 Then
                If oAllRecords.Count = oSelectedRecords.Count Then
                    bAllRecordsSelected = True
                End If
            End If
            chkSelectAll.Checked = bAllRecordsSelected
            If oSelectedRecords.Count > 0 Then
                btnAccept.Enabled = True
            Else
                btnAccept.Enabled = False
            End If
        End Sub

        ''' <summary>
        ''' To check if any lower policy version exists under "Awaiting Amend" status
        ''' </summary>
        ''' <param name="nInsuranceFolderKey"></param>
        ''' <param name="nInsuranceFileKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function LowerPolicyVersionExistsUnderAmendment(ByVal nInsuranceFolderKey As Integer, ByVal nInsuranceFileKey As Integer) As Boolean
            Dim oPolicies As NexusProvider.PolicyCollection = CType(Session(CNSearchResults), NexusProvider.PolicyCollection)
            Dim PoliciesExists = From policy In oPolicies Where policy.InsuranceFolderKey = nInsuranceFolderKey And _
                                policy.InsuranceFileKey < nInsuranceFileKey And policy.PTRIStatus <> kPolicyStatusToAccept
            If PoliciesExists.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace
