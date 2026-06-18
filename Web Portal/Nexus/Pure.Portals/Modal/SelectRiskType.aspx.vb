Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library

Partial Class Modal_SelectRiskType
    Inherits System.Web.UI.Page

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub
End Class
