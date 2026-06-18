Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus


    Partial Class secure_agent_FindAccountHandler
        Inherits BaseFindParty

        Private oItem As NexusProvider.PersonalParty


        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            grdvSearchResults.Visible = True
            grdvSearchResults.AllowPaging = True
            Session(CNSearchType) = PartyType.AH
            FindParty()

        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'To set the Focus
            Page.SetFocus(txtAccounthandler_code)

            If IsPostBack Then
                grdvSearchResults.DataSource = Session(CNSearchData)
                grdvSearchResults.DataBind()
            End If
        End Sub


        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            txtAccounthandler_code.Text = String.Empty
            txtName.Text = String.Empty
            txtType.Text = String.Empty
            chkIncludeClosedBranches.Checked = False
            grdvSearchResults.Visible = False
        End Sub

        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.BaseParty).Key)
            End If
        End Sub
    End Class

End Namespace
