Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus

    Partial Class Controls_ReportControls_SumInsuredGrossToNet: Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                'set default value of
				'set default value of 'DateExtracedTo' to current date.
                RP__PERIODDATE.Text = Date.Now.ToShortDateString()
                'set validations for 'DateExtracedTo'.
                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                Dim dtMinDate As DateTime = DateTime.MinValue
                rngvldPeriodEndDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldPeriodEndDate.MaximumValue = Date.MaxValue.ToShortDateString()

                

            End If
        End Sub

      

    End Class



End Namespace

