
Imports CMS.Library

Namespace Nexus

    Partial Class Products_UIIC_SummaryOfRisk : Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, "../Internal/modal.master")
        End Sub

		Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshSOC") & ";"
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
       
    End Class

End Namespace