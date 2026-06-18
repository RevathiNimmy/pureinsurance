Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class MasterPages_default : Inherits CMS.Library.Frontend.CMSMasterPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Page.ClientScript.RegisterClientScriptInclude("ThickBox", ResolveClientUrl(AppSettings("WebRoot") & "js/JQuery.js"))
            'Page.ClientScript.RegisterClientScriptInclude("ThickBox1", ResolveClientUrl(AppSettings("WebRoot") & "js/ThickBox.js"))
        End Sub
    End Class

End Namespace

