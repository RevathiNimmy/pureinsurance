Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus

    Partial Class Controls_ReportControls_AccountListing : Inherits BaseReport

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

                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key <> 0 Then
                        If oUserDetails.PartyType = "AG" Then ' Agent
                            RP__agent_code.Value = oUserDetails.PartyCode
                            liBranch.Attributes.Add("style", "display:none;")
                        End If
                    End If
                End If
                ' Adding the Default option for the Ledger Dropdown

                'If GetLocalResourceObject("ddl_Ledger_DefaultText").ToString().Trim.Length <> 0 Then
                '    'if client has give any default value than only add
                '    RP__LEDGERNAME.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Ledger_DefaultText"), "ALL"))
                'End If

            End If
        End Sub

    End Class

End Namespace

