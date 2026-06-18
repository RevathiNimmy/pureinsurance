Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class Controls_ClientSummary : Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            Dim oParty As NexusProvider.BaseParty
            Select Case True
                Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                    oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    If oParty.Associate IsNot Nothing AndAlso oParty.Associate.Count <> 0 Then
                        lblAssociatedClientSummaryTitle.Visible = True
                    Else
                        lblAssociatedClientSummaryTitle.Visible = False
                    End If

                Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                    oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    If oParty.Associate IsNot Nothing AndAlso oParty.Associate.Count <> 0 Then
                        lblAssociatedClientSummaryTitle.Visible = True
                    Else
                        lblAssociatedClientSummaryTitle.Visible = False
                    End If

            End Select
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Catlin Performance Fix
            If CType(Session(CNClientMode), Mode) = Mode.View AndAlso Me.Visible = True AndAlso Session(CNIsNewClient) Is Nothing Then
                'If CType(Session(CNClientMode), Mode) = Mode.View Then
                Dim oParty As NexusProvider.BaseParty = Nothing
                Dim dAccountBalance As Decimal
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        dAccountBalance = CType(Session(CNParty), NexusProvider.PersonalParty).ClientSharedData.AccountBalance
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        dAccountBalance = CType(Session(CNParty), NexusProvider.CorporateParty).ClientSharedData.AccountBalance
                    Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select

                lblAccountBalance.Text = New Money(dAccountBalance, oParty.Currency).Formatted.ToString
                lblNoofPolicies.Text = oParty.NoofPolicies
                lblNoofOpenClaims.Text = oParty.NoofOpenClaims
                lblNoofClosedClaims.Text = oParty.NoofClosedClaims
            End If
        End Sub
    End Class

End Namespace