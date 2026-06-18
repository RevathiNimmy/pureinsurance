
Namespace Nexus

    Partial Class Controls_ReportControls_ClaimsExperience : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then

                'set default value of 'DateExtracedTo' to current date.
                RP__END_DATE.Text = Date.Now.ToShortDateString()
                'set validations for 'DateExtracedTo'.
                rngvldDateExtracedTo.MinimumValue = Date.MinValue.ToShortDateString()
                rngvldDateExtracedTo.MaximumValue = Date.MaxValue.ToShortDateString()
            End If
        End Sub

        Protected Sub RP__CORPORATECLIENT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RP__CORPORATECLIENT.TextChanged
            If RP__CORPORATECLIENT.Text.Length > 0 And RP__POLICYNUMBER.Text.Length = 0 Then
                rqdClient.Enabled = False
                rqdPolicy.Enabled = False
            ElseIf RP__CORPORATECLIENT.Text.Length = 0 And RP__POLICYNUMBER.Text.Length = 0 Then
                rqdClient.Enabled = True
            End If
        End Sub

        Protected Sub RP__POLICYNUMBER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RP__POLICYNUMBER.TextChanged
            If RP__CORPORATECLIENT.Text.Length = 0 And RP__POLICYNUMBER.Text.Length > 0 Then
                rqdClient.Enabled = False
                rqdPolicy.Enabled = False
            ElseIf RP__CORPORATECLIENT.Text.Length = 0 And RP__POLICYNUMBER.Text.Length = 0 Then
                rqdClient.Enabled = True
            End If
        End Sub
    End Class

End Namespace

