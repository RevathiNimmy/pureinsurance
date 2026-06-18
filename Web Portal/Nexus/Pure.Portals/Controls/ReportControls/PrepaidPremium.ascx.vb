Imports Nexus.Constants.Session
Imports Nexus.Constants
Namespace Nexus

    Partial Class Controls_ReportControls_PrepaidPremium : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
			
			 'set default value of
				'set default value of 'DateExtracedTo' to current date.
                RP__endDTCombo.Text = Date.Now.ToShortDateString()
                'set validations for 'DateExtracedTo'.
                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                Dim dtMinDate As DateTime = DateTime.MinValue
                rngvldPeriodEndDate.MinimumValue = dtMinDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldPeriodEndDate.MaximumValue = Date.MaxValue.ToShortDateString()

				'set default value of 'DateExtracedTo' to current date.
                RP__startDTCombo.Text = Date.Now.ToShortDateString()
                'set validations for 'DateExtracedTo'.
                'Setting the Minimum date to year 1753 as supported by SQL 2K8

                Dim dtStartDate As DateTime = DateTime.MinValue
                rngvldPeriodStartDate.MinimumValue = dtStartDate.AddYears(1752).ToString("dd/MM/yyyy")
                rngvldPeriodStartDate.MaximumValue = Date.MaxValue.ToShortDateString()

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

            End If
        End Sub
      

    End Class

      

End Namespace

