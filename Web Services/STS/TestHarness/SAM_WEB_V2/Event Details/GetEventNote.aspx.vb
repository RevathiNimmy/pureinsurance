
Partial Class Event_Details_GetEventNote
    Inherits System.Web.UI.Page

  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtcontext.Text = Session("Context")
        txtsubject.Text = Session("subject")
        txtUserName.Text = Session("Username")
        txtEventDate.Text = Session("EventDate")
        txtText.Text = Session("Text")
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("EventList.aspx")
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("EventList.aspx")
    End Sub
End Class
