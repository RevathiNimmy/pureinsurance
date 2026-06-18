Imports Nexus.Constants.Session
Imports Nexus.Constants

Namespace Nexus

    Partial Class Controls_ReportControls_QuoteToNewBusiness : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim dtMinDate As DateTime


            If IsPostBack Then
                'dtMinDate = Convert.ToDateTime("01/01/1753").ToString("dd/MM/yyyy")
                'set default value of 'StartDate' and 'EndDate' to current date.
                RP__START_DATE.Text = Date.Now.ToShortDateString()
                RP__END_DATE.Text = Date.Now.ToShortDateString()

                'set validations for Start and End Dates.

                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                dtMinDate = DateTime.MinValue
                rngvldStartDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldStartDate.MaximumValue = Date.MaxValue.ToShortDateString()

                rngvldEndDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldEndDate.MaximumValue = Date.MaxValue.ToShortDateString()
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key <> 0 Then
                        If oUserDetails.PartyType = "AG" Then ' Agent
                            RP__agent_code.Value = oUserDetails.PartyCode
                            liGroupBy.Attributes.Add("style", "display:none;")
                        End If
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace

