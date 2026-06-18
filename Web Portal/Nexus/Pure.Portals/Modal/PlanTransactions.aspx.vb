Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class Modal_PlanTransactions
        Inherits Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
        
    End Class
End Namespace
