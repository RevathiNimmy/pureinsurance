Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus
    Partial Class Controls_ReportControls_OutstandingClaims : Inherits BaseReport

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim dtMinDate As New DateTime
            If IsPostBack Then

                RP__Start_Date.Text = Date.Now.ToShortDateString()
                RP__End_Date.Text = Date.Now.ToShortDateString()

                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                dtMinDate = DateTime.MinValue
                rngvldStartDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldStartDate.MaximumValue = Date.MaxValue.ToShortDateString()

                rngvldEndDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldEndDate.MaximumValue = Date.MaxValue.ToShortDateString()

                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key <> 0 Then
                        If oUserDetails.PartyType = "OTTPA" Then ' TPA
                            RP__TPACode.Value = oUserDetails.PartyCode
                        End If
                    End If
                End If

            End If
        End Sub
    End Class
End Namespace