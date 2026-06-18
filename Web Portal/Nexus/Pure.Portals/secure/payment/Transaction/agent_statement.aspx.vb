Imports Nexus

Namespace Nexus


    Partial Class secure_payment_Transaction_agent_statement : Inherits BasePayment

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                'Session(CNPaid) = True
                'Response.Redirect("~/secure/TransactionConfirmation.aspx")
                'session value and redirect not handled in page any more! replaced bya call to the function below:
                Me.SetPaymentTakenAndRedirect()
                'in other pages (credti card for example) you would call the above method on the button click event
            End If
        End Sub
    End Class
End Namespace