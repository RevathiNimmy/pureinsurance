Imports Nexus.Constants.Session
Imports Nexus.Constants


Namespace Nexus
    Partial Class Controls_ReportControls_OutstandingLossReserves : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                'set default value of facility dropdown

                If GetLocalResourceObject("ddl_Facility_defaulttext").ToString().Trim.Length <> 0 Then
                    RP__Treaty.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Facility_defaulttext"), "ALL"))
                End If
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

