Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_Confirmation
        Inherits System.Web.UI.Page

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setDecline('" + txtConfirmation.Text + "');", True)
        End Sub
    End Class
End Namespace