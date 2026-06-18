Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class controls_ClaimsProgressBar : Inherits System.Web.UI.UserControl

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Select Case Session(CNMode)
                Case Mode.ViewClaim
                    'reserves and complete tabs should be hidden
                    CompleteStyle = "hidden"
                Case Mode.NewClaim
                    hypSearchResults.Text = "Find Policy"
            End Select
            If CType(Session(CNClaimQuote), NexusProvider.Quote).Risks IsNot Nothing AndAlso CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.Count > 0 Then
                Dim sDisplayReinsurance As String
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaimQuote), NexusProvider.Quote).Risks(0).RiskTypeCode)
                If sDisplayReinsurance = "1" Then
                    liReinsurance.Visible = True
                Else
                    liReinsurance.Visible = False
                End If
            End If
        End Sub

        Public WriteOnly Property SearchResultsStyle() As String
            Set(ByVal value As String)
                'set class of list element
                liSearchResults.Attributes("class") = value
                Select Case value
                    Case "hidden"
                        'completely hide the list element
                        liSearchResults.Visible = False
                    Case "incomplete"
                        'disable hyperlink
                        hypSearchResults.Enabled = False
                    Case Else
                        hypOverview.Enabled = True
                End Select
            End Set
        End Property

        Public WriteOnly Property OverviewStyle() As String
            Set(ByVal value As String)
                'set class of list element
                liOverview.Attributes("class") = value
                Select Case value
                    Case "hidden"
                        'completely hide the list element
                        liOverview.Visible = False
                    Case "incomplete"
                        'disable hyperlink
                        hypOverview.Enabled = False
                    Case Else
                        hypOverview.Enabled = True
                End Select
            End Set
        End Property

        Public WriteOnly Property ReinsuranceStyle() As String
            Set(ByVal value As String)
                'set class of list element
                liReinsurance.Attributes("class") = value
                Select Case value
                    Case "hidden"
                        'completely hide the list element
                        liReinsurance.Visible = False
                    Case "incomplete"
                        'disable hyperlink
                        hypReinsurance.Enabled = False
                End Select
            End Set
        End Property

        Public WriteOnly Property PerilsStyle() As String
            Set(ByVal value As String)
                'set class of list element
                liPerils.Attributes("class") = value
                Select Case value
                    Case "hidden"
                        'completely hide the list element
                        liPerils.Visible = False
                    Case "incomplete"
                        'disable hyperlink
                        hypPerils.Enabled = False
                End Select
            End Set
        End Property

        Public WriteOnly Property SummaryStyle() As String
            Set(ByVal value As String)
                'set class of list element
                liSummary.Attributes("class") = value
                Select Case value
                    Case "hidden"
                        'completely hide the list element
                        liSummary.Visible = False
                    Case "incomplete"
                        'disable hyperlink
                        hypSummary.Enabled = False
                End Select
            End Set
        End Property

        Public WriteOnly Property CompleteStyle() As String
            Set(ByVal value As String)
                'set class of list element
                liComplete.Attributes("class") = value
                Select Case value
                    Case "hidden"
                        'completely hide the list element
                        liComplete.Visible = False
                    Case "incomplete"
                        'disable hyperlink
                        hypComplete.Enabled = False
                End Select
            End Set
        End Property
    End Class

End Namespace
