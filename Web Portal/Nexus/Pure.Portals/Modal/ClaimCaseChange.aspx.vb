Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus


    Partial Class Modal_ClaimCaseChange
        Inherits Frontend.clsCMSPage

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            'set up javascript to postback the parent page
            'this will trigger a partial postback and close the thickbox
            'Take the Value of TextBox on this screen in Session and Pass it into the “EventDescription” parameter of CloseCase/SaveCase SAM Method in case of Close Case/Edit Case funtionality
            If Request.QueryString("Button") = "Submit" Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.SaveCase('" & txtChangeDescription.Text & "');", True)
            Else
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.CloseCase('" & txtChangeDescription.Text & "');", True)
            End If

        End Sub

    End Class
End Namespace
