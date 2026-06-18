Imports Nexus.Constants.Session
Imports Nexus.Constants


Namespace Nexus
    Partial Class Controls_ReportControls_AgentPerformance : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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


                'set default value of 'DateExtracedTo' to current date.
                RP__PERIODDATE.Text = Date.Now.ToShortDateString()
                'set validations for 'DateExtracedTo'.
                'Setting the Minimum date to year 1753 as supported by SQL 2K8
                Dim dtMinDate As DateTime = DateTime.MinValue
                rngvldPeriodEndDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldPeriodEndDate.MaximumValue = Date.MaxValue.ToShortDateString()

                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key <> 0 Then
                        If oUserDetails.PartyType = "AG" Then ' Agent
                            RP__AGENT.Text = oUserDetails.PartyCode
                            RP__AGENT.Attributes.Add("readonly", "readonly")
                            liBranch.Attributes.Add("style", "display:none;")
                            liAgentGroup.Attributes.Add("style", "display:none;")
                            btnAgentCode.Enabled = False
                        End If
                    End If
                End If

                If RP__AGENTGROUPCODE.Value = "" Then
                    RP__AGENTGROUPCODE.Value = "ALL"
                End If
            End If
        End Sub
        Sub ServerValidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)

            If PartyName.PartyCode.Trim <> "" And PartyName.PartyKey = 0 Then
                args.IsValid = False
            End If
            If PartyName.PartyCode.Trim <> "" Then
                RP__AgentGroupCode.Value = PartyName.PartyCode.Trim
            Else
                RP__AgentGroupCode.Value = "ALL"
            End If
        End Sub
    End Class

End Namespace

