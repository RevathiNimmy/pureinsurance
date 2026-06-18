Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus
    Partial Class Controls_ReportControls_BusinessPerformanceascx : Inherits BaseReport

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim dtMinDate As New DateTime

            If IsPostBack Then

                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                dtMinDate = DateTime.MinValue

                rngvldPeriodDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldPeriodDate.MaximumValue = Date.MaxValue.ToShortDateString()


                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 0 Then
                    If GetLocalResourceObject("ddl_Branchlst_defaulttext").ToString().Trim.Length <> 0 Then

                        RP__BRANCH_ID.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Branchlst_defaulttext"), "ALL"))
                    End If
                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    If oBranchs IsNot Nothing Then
                        For Each oBranch As NexusProvider.Branch In oBranchs
                            RP__BRANCH_ID.Items.Add(New ListItem(oBranch.Description, oBranch.BranchKey))
                        Next
                    End If
                End If


            End If
        End Sub
    End Class
End Namespace