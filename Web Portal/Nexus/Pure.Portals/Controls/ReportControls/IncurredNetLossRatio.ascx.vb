
Namespace Nexus

    Partial Class Controls_ReportControls_IncurredNetLossRatio : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then

                'set validations for 'Period End Date'.
                RP__PERIODENDDATE.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                'set validations for 'Period end Date'.
                rngvldPeriodEndDate.MinimumValue = Date.MinValue.ToShortDateString()
                rngvldPeriodEndDate.MaximumValue = Date.MaxValue.ToShortDateString()
                reqdvldPeriodEndDate.InitialValue = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()

            End If
        End Sub

    End Class

End Namespace

