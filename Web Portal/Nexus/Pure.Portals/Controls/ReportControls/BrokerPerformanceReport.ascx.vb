Namespace Nexus
    Partial Class Controls_ReportControls_BrokerPerformanceReport : Inherits BaseReport
       
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then

                'set default value of 'Period Start Date' to current date.
                RP__PERIODSTARTDATE.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()
                'set validations for 'Period Start Date'.
                rngvldPeriodStartDate.MinimumValue = Date.MinValue.ToShortDateString()
                rngvldPeriodStartDate.MaximumValue = Date.MaxValue.ToShortDateString()
                reqvldPeriodStartDate.InitialValue = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()

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
