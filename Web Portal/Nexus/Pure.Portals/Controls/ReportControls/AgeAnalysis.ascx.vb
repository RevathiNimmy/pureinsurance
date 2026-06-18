Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus
    Partial Class Controls_ReportControls_AgeAnalysis : Inherits BaseReport
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                'set default value of

                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 0 Then
                    If GetLocalResourceObject("ddl_Branchlst_defaulttext").ToString().Trim.Length <> 0 Then
                        'if client has give any default value than only add
                        RP__BRANCH_ID.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Branchlst_defaulttext"), "ALL"))
                    End If
                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    If oBranchs IsNot Nothing Then
                        For Each oBranch As NexusProvider.Branch In oBranchs
                            RP__BRANCH_ID.Items.Add(New ListItem(oBranch.Description, oBranch.BranchKey))
                        Next
                    End If
                End If

                'set default value of 'End Date' to current date.
                RP__end_Date.Text = Date.Now.ToShortDateString()
                'set validations for 'End Date'.
                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                Dim dtMinDate As DateTime = DateTime.MinValue
                rngvldEndDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldEndDate.MaximumValue = Date.MaxValue.ToShortDateString()
            End If
        End Sub
    End Class
End Namespace