
Partial Class Covernote_mainteneace
    Inherits System.Web.UI.Page

    Protected Sub btnyes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnyes.Click
        Response.Redirect("GetCoverNoteBook.aspx")
    End Sub
End Class
