Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_FindBank
        Inherits Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
            
                'To set the Focus
                Page.SetFocus(txtShortName)

            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click

            Dim oBankCollection As New NexusProvider.BankCollection
            Dim oBankSearchCriteria As New NexusProvider.BankSearchCriteria

            Try
                'Obtaining data from controls if search criteria is entered and assigning the data to objects 
                If Not txtShortName.Text.Trim.Length = 0 Then
                    oBankSearchCriteria.ShortCode = txtShortName.Text.Trim()
                End If

                If Not txtName.Text.Trim.Length = 0 Then
                    oBankSearchCriteria.BankName = txtName.Text.Trim()
                End If

                'to limit the search return from SAM
                oBankSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

                oWebService = New NexusProvider.ProviderManager().Provider
                oBankCollection = oWebService.FindBank(oBankSearchCriteria)

                ' checks if the collections returns any value and assigns the values to Grid
                ViewState.Remove(CNSearchBankResult)
                ViewState.Add(CNSearchBankResult, oBankCollection)
                grdvBankResult.Visible = True
                grdvBankResult.AllowPaging = True
                grdvBankResult.DataSource = oBankCollection
                grdvBankResult.DataBind()

                'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                If oBankCollection.Count >= oPortal.MaxSearchResults Then
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
                'disposes the created objects
                oBankSearchCriteria = Nothing
                oBankCollection = Nothing
                oWebService = Nothing
            End Try
        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            'clearing all the values for entering new search criteria
            txtShortName.Text = Nothing
            txtName.Text = Nothing
            grdvBankResult.Visible = False
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            'code to close the screen on thickbox implementation 
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub grdvBankResult_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvBankResult.DataBound
            If grdvBankResult.Rows.Count = 0 Or grdvBankResult.PageCount = 1 Then
                grdvBankResult.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvBankResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvBankResult.PageIndexChanging
            grdvBankResult.PageIndex = e.NewPageIndex
            grdvBankResult.DataSource = CType(ViewState.Item(CNSearchBankResult), NexusProvider.BankCollection)
            grdvBankResult.DataBind()
        End Sub

        Protected Sub grdvBankResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBankResult.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("BankKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Bank).BankKey)

            End If
        End Sub
    End Class
End Namespace
