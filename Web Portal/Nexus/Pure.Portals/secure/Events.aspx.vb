Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Library.Config
Imports CMS.Library.Portal
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class secure_Events : Inherits Frontend.clsCMSPage
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        ''' <summary>
        ''' Change master page to Modal if opened as modal page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
            If Request.QueryString("modal") = "true" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

        End Sub
    End Class
End Namespace

