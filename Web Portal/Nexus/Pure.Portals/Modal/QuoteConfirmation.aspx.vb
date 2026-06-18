Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Modal_QuoteConfirmation
        Inherits System.Web.UI.Page

        Protected Sub btnComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComplete.Click
            'add javascript to call script in parent page which will close modal dialog
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Complete") & ";"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ParentPostBack", PostBackStr, True)
            'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
        End Sub

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            'add javascript to call script in parent page which will close modal dialog
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Delete") & ";"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ParentPostBack", PostBackStr, True)
            'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class
End Namespace

