
Partial Class UIIC_demo_HomePage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim s As String = Request.QueryString("id")
        Dim name As String = Request.QueryString("name")
        If name IsNot Nothing AndAlso name.Contains("Policy") Then
            Response.Write("<script>alert('" + s + "is created successfully')</script>")
        ElseIf name IsNot Nothing AndAlso name.Contains("ClaimPayment") Then
            Response.Write("<script>alert('Payment is made against the claim " + s + " ')</script>")


        ElseIf name IsNot Nothing AndAlso name.Contains("ViewClaim") Then
            Response.Write("<script>alert('The Claim " + s + " is Viewed ')</script>")

        ElseIf name IsNot Nothing AndAlso name.Contains("MaintainClaim") Then
            Response.Write("<script>alert('The Claim " + s + " is Maintained ')</script>")

        ElseIf name IsNot Nothing AndAlso name.Contains("Claim") Then
            Response.Write("<script>alert('The claim " + s + " is created ')</script>")

        ElseIf name IsNot Nothing AndAlso name.Contains("Account") Then
            Response.Write("<script>alert('The Account " + s + " is Allocated ')</script>")
        ElseIf name IsNot Nothing AndAlso name.Contains("Account1") Then
            Response.Write("<script>alert('The Account " + s + " is Unallocated ')</script>")
        End If
    End Sub
End Class
