Imports System.Configuration.ConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus
    Partial Class Modal_FindDocumentTemplates
        Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
        End Sub
    End Class
End Namespace

