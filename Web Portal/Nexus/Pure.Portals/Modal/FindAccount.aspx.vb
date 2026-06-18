Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_FindAccount
        Inherits Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())


        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click

            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria

            Try
                'Obtaining data from controls if search criteria is entered and assigning the data to objects 
                If Not txtShortName.Text.Trim.Length() = 0 Then
                    oAccountSearchCriteria.ShortCode = txtShortName.Text.Trim()
                End If

                If Not txtName.Text.Trim.Length() = 0 Then
                    oAccountSearchCriteria.AccountName = txtName.Text.Trim()
                End If

                If chkIsShowAccountBalance.Checked Then
                    oAccountSearchCriteria.ShowBalance = chkIsShowAccountBalance.Checked
                    oAccountSearchCriteria.ShowBalanceSpecified = True
                Else
                    oAccountSearchCriteria.ShowBalanceSpecified = False
                End If

                If chkShowDeletedAccounts.Checked Then
                    oAccountSearchCriteria.ShowDeleted = chkShowDeletedAccounts.Checked
                    oAccountSearchCriteria.ShowDeletedSpecified = True
                Else
                    oAccountSearchCriteria.ShowDeletedSpecified = False
                End If

                If Not txtinsuranceRef.Text.Trim.Length = 0 Then
                    oAccountSearchCriteria.InsuranceRef = txtinsuranceRef.Text.Trim()
                End If

                If Not txtPurchaseOrderNo.Text.Trim.Length = 0 Then
                    oAccountSearchCriteria.PurchaseOrderNo = txtPurchaseOrderNo.Text.Trim()
                End If

                If Not txtPurchaseInvoiceNo.Text.Trim.Length = 0 Then
                    oAccountSearchCriteria.PurchaseInvoiceNo = txtPurchaseInvoiceNo.Text.Trim()
                End If

                If Not ddlAccount.Value = Nothing Then
                    oAccountSearchCriteria.AccountTypeCode = ddlAccount.Value.Trim()
                End If

                If Not ddlOperator.SelectedItem.Text.Trim() = "(all)" Then
                    oAccountSearchCriteria.OperatorKey = ddlOperator.SelectedValue.Trim()
                    oAccountSearchCriteria.OperatorKeySpecified = True
                End If

                oAccountSearchCriteria.ExcludeInsurerAgents = False
                oAccountSearchCriteria.IncludeInsurerAgents = False
                
                If Not ddlLedger.SelectedItem.Text.Trim() = "(All types)" Then
                    oAccountSearchCriteria.LedgerCode = ddlLedger.SelectedValue.Trim()
                End If

                'to limit the search return from SAM
                oAccountSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

                oWebService = New NexusProvider.ProviderManager().Provider
                oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)

                ' checks if the collections returns any value and assigns the values to Grid
                Session(CNSearchAccountResult) = oAccountSearchResultCollection
                grdvFindAccount.Visible = True
                grdvFindAccount.AllowPaging = True
                grdvFindAccount.DataSource = oAccountSearchResultCollection
                grdvFindAccount.DataBind()

                'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                If oAccountSearchResultCollection IsNot Nothing AndAlso oAccountSearchResultCollection.Count >= oPortal.MaxSearchResults Then
                    'create a custom validator
                    Dim cstMaxResults As New CustomValidator
                    cstMaxResults.IsValid = False
                    'look for a validation message in the page resources, but if there is not one defined add a default message
                    cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                    cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                    'add the validator to the page, this will have the effect of making the page invalid
                    Page.Validators.Add(cstMaxResults)
                End If
            Finally
                'cleaning up
                oWebService = Nothing
                oAccountSearchCriteria = Nothing
                oAccountSearchResultCollection = Nothing
                oNexusConfig = Nothing
                oPortal = Nothing
            End Try

        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            ' clearing all the values for entering new search criteria
            txtShortName.Text = Nothing
            txtName.Text = Nothing
            'txtCode.Text = Nothing
            ddlAccount.Value = ""
            chkIsShowAccountBalance.Checked = False
            chkShowDeletedAccounts.Checked = False
            txtinsuranceRef.Text = Nothing
            ddlOperator.SelectedItem.Text = "(all)"
            txtPurchaseOrderNo.Text = Nothing
            txtPurchaseInvoiceNo.Text = Nothing
            grdvFindAccount.Visible = False
            Session(CNSearchAccountResult) = Nothing
        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim nAgentCnt As Integer = 0
            If Not IsNothing(Current.Session(Nexus.Constants.CNAgentDetails)) Then
                 nAgentCnt = ctype(Current.Session(Nexus.Constants.CNAgentDetails),NexusProvider.UserDetails).Key
            End If
            'To set the difference between Account and Insured Account
            hType.Value = Request("FromPage")

            If hType.Value.Trim.ToUpper() = "ACC" AndAlso nAgentCnt > 0 Then
                ddlLedger.SelectedValue = "SA"
                ddlLedger.Enabled = False
            End If

            If Not IsPostBack Then
                'Not to display the Ledger control in case of Insurer Payments
                If Request.QueryString("Page") IsNot Nothing And Request.QueryString("Page") = "IP" Then
                    liLedger.Visible = False
                Else
                    liLedger.Visible = True
                End If

                PopulateOperator() 'fill the list of operators in the drop down

                'To set the Focus
                Page.SetFocus(txtShortName)

            End If

        End Sub

        Protected Sub PopulateOperator()

            Dim oUser As NexusProvider.UserCollection

            Try
                'fill the list of operators in the drop down
                oWebService = New NexusProvider.ProviderManager().Provider
                oUser = oWebService.GetUserGroupUsers("", DateTime.Now, True, True)
                ddlOperator.DataSource = oUser
                ddlOperator.DataTextField = "UserName"
                ddlOperator.DataValueField = "UserId"
                ddlOperator.DataBind()

                ddlOperator.SelectedItem.Text = "(all)"

            Finally
                'cleaning up
                oWebService = Nothing
                oUser = Nothing
            End Try

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub grdvFindAccount_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvFindAccount.DataBound
            If grdvFindAccount.Rows.Count = 0 Or grdvFindAccount.PageCount = 1 Then
                grdvFindAccount.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvFindAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvFindAccount.PageIndexChanging

            grdvFindAccount.PageIndex = e.NewPageIndex
            grdvFindAccount.DataSource = CType(Session(CNSearchAccountResult), NexusProvider.AccountSearchResultCollection)
            grdvFindAccount.DataBind()

        End Sub

        Protected Sub grdvFindAccount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvFindAccount.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ShortCode") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AccountSearchResult).ShortCode)

                If chkIsShowAccountBalance.Checked = True Then
                    e.Row.Cells(7).Visible = True
                Else
                    e.Row.Cells(7).Visible = False
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If chkIsShowAccountBalance.Checked = True Then
                    e.Row.Cells(7).Visible = True
                Else
                    e.Row.Cells(7).Visible = False
                End If
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
            If Not IsPostBack Then
                If (Request.QueryString("shortcode") IsNot Nothing AndAlso Request.QueryString("shortcode").Length > 0) Then
                    txtShortName.Text = Request.QueryString("shortcode").Trim()
                    btnFindNow_Click(sender, e)
                End If
            End If
        End Sub
    End Class

End Namespace
