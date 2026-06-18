Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Web.HttpContext

Namespace Nexus

    Partial Class Modal_SelectBranch : Inherits System.Web.UI.Page


        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
            If Not (Current.Cache("UserAuthority_" & oUserAuthority.UserCode.ToString & "_" & oUserAuthority.UserAuthorityOption) Is Nothing) Then
                Current.Cache.Remove("UserAuthority_" & oUserAuthority.UserCode.ToString & "_" & oUserAuthority.UserAuthorityOption)
            End If

            Session(CNBranchCode) = ddlBranchCode.SelectedValue
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','SelectBranch');", True)

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                If oUserDetails.ListOfBranches.Count > 1 Then
                    'Sort the branches
                    oUserDetails.ListOfBranches.SortColumn = "Description"
                    oUserDetails.ListOfBranches.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                    oUserDetails.ListOfBranches.Sort()
                End If
                ddlBranchCode.DataSource = oUserDetails.ListOfBranches
                ddlBranchCode.DataBind()
                ddlBranchCode.SelectedValue = Session(CNBranchCode)
            End If

        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
    End Class

End Namespace