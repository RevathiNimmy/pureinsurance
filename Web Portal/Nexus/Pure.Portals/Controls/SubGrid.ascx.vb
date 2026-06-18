Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Partial Class Controls_SubGrid
    Inherits System.Web.UI.UserControl

    Private oPolCol As NexusProvider.PolicyCollection

    Public Property PolicyCollection() As NexusProvider.PolicyCollection
        Get
            Return oPolCol
        End Get
        Set(ByVal value As NexusProvider.PolicyCollection)
            oPolCol = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FillGridView()
    End Sub

    Protected Sub FillGridView()
        Dim oBrokerPolicies As NexusProvider.PolicyCollection = oPolCol
        grdQuoteReferences.DataSource = oBrokerPolicies
        grdQuoteReferences.DataBind()
    End Sub
    'Private _QuoteRef As String

    'Public WriteOnly Property QuoteRef() As String
    '    Set(ByVal value As String)
    '        _QuoteRef = value
    '    End Set
    'End Property

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    'this would get the sub quotes and bind to the grid, we're just showing a label for POC
    '    'lblQuoteRef.Text = "Quote ref : " & _QuoteRef
    '    lblQuoteRef.Text = "Quote ref : "
    'End Sub
End Class
