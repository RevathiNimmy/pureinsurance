Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus
    Partial Class Controls_ReportControls_AgentStatement : Inherits BaseReport


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

                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key <> 0 Then
                        If oUserDetails.PartyType = "AG" Then ' Agent
                           RP__AgentShortName.Text = oUserDetails.PartyCode
                            RP__AgentShortName.Attributes.Add("readonly", "readonly")
                            btnAgentCode.Enabled = False
                            liAgentGroup.Attributes.Add("style", "display:none;")
                            liBranch.Attributes.Add("style", "display:none;")
                            liGroupBy.Attributes.Add("style", "display:none;")
                        End If
                    End If
                End If
                If RP__AgentGroupCode.Value = "" Then
                    RP__AgentGroupCode.Value = "ALL"
                End If
                'set default value of 'DateExtracedTo' to current date.

                'set validations for 'DateExtracedTo'.
                'comvldFromDate.MinimumValue = Date.MinValue.ToShortDateString()
                'rngvldPeriodEndDate.MaximumValue = Date.MaxValue.ToShortDateString()
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