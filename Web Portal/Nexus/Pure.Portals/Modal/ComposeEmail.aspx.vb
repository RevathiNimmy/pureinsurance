Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library

Partial Class Modal_ComposeEmail
    Inherits CMS.Library.Frontend.clsCMSPage

    Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub
End Class
