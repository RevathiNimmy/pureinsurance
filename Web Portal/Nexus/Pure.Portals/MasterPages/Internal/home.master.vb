Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class MasterPages_Internal_home : Inherits CMSMasterPage
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			If Not IsNothing(Session(CNAgentDetails)) Then
                lnkLogOut.Text = "LogOut"
                lnkLogOut.PostBackUrl = "~/logout.aspx"
            Else
                lnkLogOut.Text = "LogIn"
                lnkLogOut.PostBackUrl = "~/Login.aspx"
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Uncomment line below in order to display debug information
            'Controls.Add(New LiteralControl(NexusProvider.ErrorFormatter.GetSessionAsHtml()))
        End Sub

        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            Dim validatorOverrideScripts As String = ("<script src='" & (ResolveClientUrl("~/App_Themes/Internal/js/validators.js") & "' type='text/javascript'></script>"))
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "ValidatorOverrideScripts", validatorOverrideScripts, False)
            MyBase.Render(writer)
        End Sub
    End Class
End Namespace
