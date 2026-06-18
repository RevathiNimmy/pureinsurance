Imports CMS.library
Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_FindUser
        Inherits System.Web.UI.Page

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'To set the Focus
                Page.SetFocus(txtUserName)

            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        ''' <summary>
        ''' To find the Users
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
            fillGrid()
        End Sub

        ''' <summary>
        ''' To fill the Grid with the searched user
        ''' </summary>
        ''' <remarks></remarks>
        Sub fillGrid()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oFindUsersSearchCriteria As New NexusProvider.FindUsersSearchCriteria
            Dim oUserCollection As New NexusProvider.UserCollection

            Try
                With oFindUsersSearchCriteria
                    .FullName = txtFullName.Text
                    .UserName = txtUserName.Text
                End With

                'to limit the search return from SAM
                oFindUsersSearchCriteria.MaxRowsToFetch = oPortal.MaxSearchResults

                oUserCollection = oWebService.FindUsers(oFindUsersSearchCriteria)

                grdvFindUser.Visible = True
                If oUserCollection IsNot Nothing Then

                    If oUserCollection.Count > 0 Then

                        grdvFindUser.DataSource = oUserCollection
                        grdvFindUser.DataBind()

                        'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                        If oUserCollection.Count >= oPortal.MaxSearchResults Then
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
                Else
                    grdvFindUser.DataSource = Nothing
                    grdvFindUser.DataBind()
                End If

            Finally
                oWebService = Nothing
                oFindUsersSearchCriteria = Nothing
                oUserCollection = Nothing
                oNexusConfig = Nothing
                oPortal = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' For Page Index change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>

        Protected Sub grdvFindUser_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvFindUser.PageIndexChanging
            Me.grdvFindUser.PageIndex = e.NewPageIndex
            fillGrid()
        End Sub
        
        'Protected Sub grdvFindUser_RowCommand1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvFindUser.RowCommand
        '    If e.CommandName = "Select" Then
        '        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setUser('" + e.CommandArgument.ToString + "');", True)
        '    End If
        'End Sub

        ''' <summary>
        ''' For new search.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            Me.txtFullName.Text = ""
            Me.txtUserName.Text = ""
            Me.grdvFindUser.Visible = False
        End Sub

        Protected Sub grdvFindUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvFindUser.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserId") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.User).UserId)
            End If
        End Sub
    End Class

End Namespace