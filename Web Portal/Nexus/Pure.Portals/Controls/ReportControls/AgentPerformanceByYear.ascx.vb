Imports Nexus.Constants.Session
Imports Nexus.Constants


Namespace Nexus
    Partial Class Controls_ReportControls_AgentPerformanceByYear : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                'set default value of

                'Lookup code starts here
                Dim oWebService As NexusProvider.ProviderBase = Nothing
                oWebService = New NexusProvider.ProviderManager().Provider

                'Querying(Lookup) the database to fetch the Source table's information
                Dim oLookUP As New NexusProvider.LookupListCollection
                oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")

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
                'If GetLocalResourceObject("ddl_Branchlst_defaulttext").ToString().Trim.Length <> 0 Then
                '    'if client has give any default value than only add
                '    RP__BRANCH_ID.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Branchlst_defaulttext"), "ALL"))
                'End If


                'Dim iBranchKey As Integer = 0
                'For iBranchCount As Integer = 0 To oLookUP.Count - 1
                '    RP__BRANCH_ID.Items.Add(New ListItem(oLookUP(iBranchCount).Description, oLookUP(iBranchCount).Key))
                'Next


            End If
        End Sub

        ''' <summary>
        ''' Validation to check if the Agent Group is correct.
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
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

