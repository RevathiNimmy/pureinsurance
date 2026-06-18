Imports CMS.Library
Imports System.Data
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus.Library


Partial Class Claims_FindCase
    Inherits Frontend.clsCMSPage

    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            txtCaseOpenDate.Text = DateTime.Now.ToShortDateString
            'create a unique key and add this to viewstate
            'this will be used to cache the results of the SAM call
            Dim CasepageCacheID As Guid
            CasepageCacheID = Guid.NewGuid()
            ViewState.Add("CasepageCacheID", CasepageCacheID.ToString)
            FillRiskType()
            'If this page is loaded as modal for select/search case number, then need not to display opne new case button
            If Request.QueryString("FindCase") = "1" Or Request.QueryString("modal") = "true" Then
                btnNewCase.Visible = False
                If Request.QueryString("CaseRef") <> "" Then
                    txtCaseNumber.Text = Request.QueryString("CaseRef")
                    FindCase() ' load grid
                End If
            End If
        Else
            If chkCaseOpenDate.Checked = False Then
                txtCaseOpenDate.Text = DateTime.Now.ToShortDateString
            End If
            lblInformation.Visible = True
            lblInformation.Text = String.Empty
        End If

       

        'if request came from quick search control and dosearch is true
        If Not IsPostBack AndAlso Request.QueryString("dosearch") IsNot Nothing Then
            If CType(Request.QueryString("dosearch"), Boolean) = True Then

                Dim oMaster As ContentPlaceHolder
                Dim txtControl As TextBox
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                'Assign values to controls from query string. these values will be used to search the claims
                For iCt As Integer = 0 To Request.QueryString.AllKeys.Length - 3
                    txtControl = CType(oMaster.FindControl(Request.QueryString.GetKey(iCt)), TextBox)
                    If txtControl IsNot Nothing Then
                        txtControl.Text = Request.QueryString(iCt)
                    End If
                Next
                'find the clase for given filter
                'FindCase()

            End If
        End If

        If Request("__EVENTARGUMENT") = "CloseCase" Then
            CloseCase()
        End If
    End Sub

    ''' <summary>
    ''' Button to search Cases, this will only be searched if the page is valid.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Page.IsValid Then
            Cache.Remove(ViewState("CasepageCacheID"))
            FindCase()
        End If
    End Sub

    Protected Sub grdvSearchResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.DataBound
        If grdvSearchResults.Rows.Count = 0 Or grdvSearchResults.PageCount = 1 Then
            grdvSearchResults.AllowPaging = False
        End If
    End Sub

    Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
        grdvSearchResults.PageIndex = e.NewPageIndex
        FindCase()
    End Sub

    Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
        If e.CommandName <> "Sort" And e.CommandName <> "Page" Then
            Dim currentRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim iCaseKey, iBaseCaseKey As Integer
            If e.CommandName = "edit" Then
                ClearCase()
                Integer.TryParse(grdvSearchResults.DataKeys(currentRow.RowIndex).Values(0), iBaseCaseKey)
                Integer.TryParse(grdvSearchResults.DataKeys(currentRow.RowIndex).Values(1), iCaseKey)

                Session(CNCaseKey) = iCaseKey
                Session(CNBaseCaseKey) = iBaseCaseKey
                Response.Redirect("~/Claims/ClaimCase.aspx?CaseKey=" & iCaseKey)
                'btnEditButton.PostBackUrl = "~/Claims/ClaimCase.aspx?CaseKey=" & oItem.BaseCaseKey
            ElseIf e.CommandName = "Select" Then
                Integer.TryParse(grdvSearchResults.DataKeys(currentRow.RowIndex).Values(1), iCaseKey)
                Dim sSelectFunction As String = "self.parent.setCaseReference('" & iCaseKey.ToString() & "','" & e.CommandArgument & "');"
                ScriptManager.RegisterStartupScript(Me, GetType(String), "closeThickBox", sSelectFunction, True)
            End If
        End If
    End Sub

    '''' <summary>
    '''' This is fired on the row data bound of the Grid View.
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim liEdit As HtmlGenericControl = CType(e.Row.FindControl("lnkEdit"), HtmlGenericControl)
            Dim liSelect As HtmlGenericControl = CType(e.Row.FindControl("liSelect"), HtmlGenericControl)
            Dim liClose As HtmlGenericControl = CType(e.Row.FindControl("liClose"), HtmlGenericControl)
            Dim oItem As NexusProvider.CaseDetails = CType(e.Row.DataItem, NexusProvider.CaseDetails)

            If Request.QueryString("modal") = "true" Then
                liSelect.Visible = True
                liEdit.Visible = False
                liClose.Visible = False
            Else

                If Nexus.UserCanDoTask("CaseManagement") Then
                    liEdit.Visible = True
                Else
                    liEdit.Visible = False
                End If

                'Btn_Close Functionality
                Dim btnClose As LinkButton = CType(e.Row.FindControl("btnClose"), LinkButton)
                Dim sUrl As String = Nothing
                sUrl = "' " & AppSettings("WebRoot") & "Modal/ClaimCaseChange.aspx?Button=CloseCase&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700'"
                Dim sMessage As String = GetLocalResourceObject("msg_CloseConfirm")
                sMessage = sMessage.Replace("#CaseNumber", oItem.CaseNumber)
                btnClose.OnClientClick = "javascript:CloseCaseConfirmation('" & sMessage & "'," & oItem.CaseKey & "," & oItem.BaseCaseKey & "," & sUrl & "); return false;"

                '        'NOTE - this will need to be changed to give each row a unique id
                '        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CaseKey") %>")
                e.Row.Attributes.Add("id", oItem.CaseKey)

                e.Row.Cells(5).Text = New Money(oItem.TotalIndemnity, oItem.CurrencyCode).Formatted
                e.Row.Cells(6).Text = New Money(oItem.TotalExpense, oItem.CurrencyCode).Formatted
                e.Row.Cells(7).Text = New Money(oItem.TotalExcess, oItem.CurrencyCode).Formatted

                If Nexus.GetCodeForKey(NexusProvider.ListType.PMLookup, CInt(Nexus.GetKeyForDescription(NexusProvider.ListType.PMLookup, oItem.CaseProgressDescription, "case_progress")), "case_progress", True) = "CLOSED" Then
                    liClose.Visible = False
                End If

            End If
        End If
    End Sub
    Protected Sub grdvSearchResults_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvSearchResults.Sorting
        'sort the Quote & Policy according to the column clicked
        'we need to store the current sort order in viewstate, and reverse it each time
        Dim oCaseCollection As NexusProvider.CaseCollection = CType(Cache.Item(ViewState("CasepageCacheID")), NexusProvider.CaseCollection)
        If oCaseCollection Is Nothing Then
            FindCase()
            oCaseCollection = CType(Cache.Item(ViewState("CasepageCacheID")), NexusProvider.CaseCollection)
        End If
        oCaseCollection.SortColumn = e.SortExpression
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
        oCaseCollection.SortingOrder = _sortDirection
        oCaseCollection.Sort()
        CType(sender, GridView).DataSource = oCaseCollection
        CType(sender, GridView).DataBind()
    End Sub

#Region "Protected Methods"
    Sub FillRiskType()
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim sRiskCode As String = Nothing
        Dim oRisk As NexusProvider.RiskCollection = Nothing
        Dim oTempRisk As New NexusProvider.RiskCollection
        'Fill the RiskType dropdownlist.
        oRisk = oWebService.GetRiskByProduct()
        If oRisk IsNot Nothing Then
            If oRisk.Count > 0 Then
                For iCount As Integer = 0 To oRisk.Count - 1
                    oTempRisk.Add(oRisk(iCount))
                Next
            End If
        End If

        'Sorting to the risk collection on the basis of description
        oTempRisk.SortColumn = "Description"
        oTempRisk.SortingOrder = SortDirection.Ascending
        oTempRisk.Sort()

        'binding to risk dropdown with sorted collection
        drpRiskType.DataSource = oTempRisk
        drpRiskType.DataTextField = "Description"
        drpRiskType.DataValueField = "RiskTypeCode"
        drpRiskType.DataBind()
        drpRiskType.Items.Insert(0, New ListItem(GetLocalResourceObject("drp_Default"), ""))
        drpRiskType.SelectedIndex = 0
    End Sub
    Sub CloseCase()
        Try
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oWebService.CloseCase(hBaseCaseKey.Value, hCaseKey.Value, hDesc.Value)
            Cache.Remove(ViewState("CasepageCacheID"))
            FindCase()
        Catch ex As NexusProvider.NexusException
            If ex.Errors(0).Code = "300" Then
                'lblInformation.Visible = True
                'lblInformation.Text = ex.Errors(0).Detail

                'javascript message
                Dim sMessage As String
                sMessage = ex.Errors(0).Detail
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "showcloseerrmsg", "alert('" & sMessage & "');", True)
                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "showcloseerrmsg", _
                '                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowCloseErrMsg('" & sMessage & "');});</script>")


            Else
                Cache.Remove(ViewState("CasepageCacheID"))
                FindCase()
            End If
        End Try

    End Sub

    Private Sub FindCase()
        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oCaseSearchCriteria As New NexusProvider.CaseDetails
        Dim oCaseCollection As NexusProvider.CaseCollection = CType(Cache.Item(ViewState("CasepageCacheID")), NexusProvider.CaseCollection)
        Dim dCaseOpenDate As Date

        If oCaseCollection Is Nothing Or (oCaseCollection IsNot Nothing AndAlso oCaseCollection.Count = 0) Then
            'Initializing the values
            If chkCaseOpenDate.Checked = True Then
                If String.IsNullOrEmpty(txtCaseOpenDate.Text) = False AndAlso IsDate(txtCaseOpenDate.Text) Then
                    dCaseOpenDate = txtCaseOpenDate.Text
                    oCaseSearchCriteria.CaseOpenDate = dCaseOpenDate
                End If
            End If
            oCaseSearchCriteria.CaseNumber = Trim(txtCaseNumber.Text)
            oCaseSearchCriteria.ClaimNumber = Trim(txtClaimNumber.Text)

            If String.IsNullOrEmpty(drpProgressStatus.Value) = False Then
                oCaseSearchCriteria.ProgressStatusCode = drpProgressStatus.Value
            End If

            If String.IsNullOrEmpty(drpRiskType.SelectedValue) = False Then
                oCaseSearchCriteria.RiskType = drpRiskType.SelectedValue
            End If

            oCaseSearchCriteria.RiskIndex = Trim(txtRiskIndex.Text)

            'to limit the search return from SAM
            oCaseSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

            'Sam Call with serch criteria
            oCaseCollection = oWebservice.FindCase(oCaseSearchCriteria)
            Cache.Insert(ViewState("CasepageCacheID"), oCaseCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            ' validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
            If oCaseCollection IsNot Nothing AndAlso oCaseCollection.Count >= oPortal.MaxSearchResults Then
                'create a custom validator
                Dim cstMaxResults As New CustomValidator
                cstMaxResults.IsValid = False
                'look for a validation message in the page resources, but if there is not one defined add a default message
                cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstMaxResults)
            End If
        End If

        'Populating the session with search results
        grdvSearchResults.Visible = True
        grdvSearchResults.AllowPaging = True
        grdvSearchResults.DataSource = oCaseCollection
        grdvSearchResults.DataBind()
    End Sub
#End Region

    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        grdvSearchResults.Visible = False
        grdvSearchResults.DataSource = Nothing
        grdvSearchResults.DataBind()
        txtCaseNumber.Text = String.Empty
        drpProgressStatus.Value = ""
        txtCaseOpenDate.Text = DateTime.Now.ToShortDateString
        chkCaseOpenDate.Checked = False
        txtClaimNumber.Text = String.Empty
        drpRiskType.SelectedIndex = 0
        txtRiskIndex.Text = String.Empty
    End Sub

    Protected Sub VldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles VldDate.ServerValidate
        If chkCaseOpenDate.Checked = True Then
            If String.IsNullOrEmpty(txtCaseOpenDate.Text) Then
                args.IsValid = False
            ElseIf String.IsNullOrEmpty(txtCaseOpenDate.Text) = False AndAlso IsDate(txtCaseOpenDate.Text) = False Then
                args.IsValid = False
            ElseIf String.IsNullOrEmpty(txtCaseOpenDate.Text) = False AndAlso IsDate(txtCaseOpenDate.Text) = True Then
                If CDate(txtCaseOpenDate.Text) < CDate("01/01/1900") Or CDate(txtCaseOpenDate.Text) >= CDate("12/12/8898") Then
                    args.IsValid = False
                End If
            End If
        End If
    End Sub

    Protected Sub custValidate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValidate.ServerValidate
        If args.IsValid = True AndAlso String.IsNullOrEmpty(txtRiskIndex.Text) = False Then
            If txtRiskIndex.Text.Trim.Contains("%") Then
                args.IsValid = False
                custValidate.ErrorMessage = GetLocalResourceObject("lbl_RiskIndex_Error")
            End If
        End If
    End Sub

    Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.QueryString("modal") IsNot Nothing Then
            If Request.QueryString("modal").ToLower() = "true" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If
        End If
    End Sub
End Class
