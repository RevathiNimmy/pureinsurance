Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus
    Partial Class SelectBranch : Inherits Frontend.clsCMSPage

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If Request.QueryString("Mode") = "IP" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            End If
        End Sub
    End Class
End Namespace
