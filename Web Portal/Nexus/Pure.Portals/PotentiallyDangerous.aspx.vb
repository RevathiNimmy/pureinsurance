Imports CMS.Library

Namespace Nexus
    Partial Class PotentiallyDangerous : Inherits Frontend.clsCMSPage
        Private Shared prevPage As String = String.Empty
        '''' <summary>
        '''' It will redirect an user to previous page where error occured
        '''' </summary>
        '''' <param name="sender"></param>
        '''' <param name="e"></param>
        '''' <remarks></remarks>
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Response.Redirect(Request.QueryString("aspxerrorpath"))
        End Sub
    End Class
End Namespace
