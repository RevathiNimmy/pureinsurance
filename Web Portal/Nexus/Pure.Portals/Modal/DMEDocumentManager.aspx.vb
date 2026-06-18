Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_DMEDocumentManager : Inherits System.Web.UI.Page

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Clear the session if not then it will load left bar controls (cliamInfo)
            'Session(CNMode) = Nothing
        End Sub
        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
    End Class
End Namespace