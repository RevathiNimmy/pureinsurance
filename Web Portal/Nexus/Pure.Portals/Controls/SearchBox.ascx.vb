Namespace Nexus
    Partial Class controls_SearchBox
        Inherits System.Web.UI.UserControl

        Public Event PerformSearch(ByVal sSearch As String)
        Public Event PerformAdvancedSearch(ByVal sSearch As String, ByVal ShowPerPage As Integer, _
            ByVal ShowPages As Boolean, ByVal ShowNews As Boolean, ByVal ShowMedia As Boolean)

        Public WriteOnly Property Query() As String
            Set(ByVal value As String)
                txtSearch.Text = value
            End Set
        End Property

        Public Property Mode() As String
            Get
                Return ViewState("SearchMode")
            End Get
            Set(ByVal value As String)
                ViewState("SearchMode") = value
                SwitchView()
            End Set
        End Property

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            If txtSearch.Text.Trim <> "" Then RaiseEvent PerformSearch(txtSearch.Text)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SwitchView()
        End Sub

        Private Sub SwitchView()
            If Mode Is Nothing Then Mode = "simple"
            Select Case Mode.ToLower
                Case "simple", ""
                    MultiView1.ActiveViewIndex = 0
                    cmdShowAdv.Visible = False
                Case "standard"
                    MultiView1.ActiveViewIndex = 0
                Case "advanced"
                    MultiView1.ActiveViewIndex = 1
            End Select
        End Sub

        Protected Sub cmdShowAdv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdShowAdv.Click
            Mode = "advanced"
        End Sub

        Protected Sub cmdAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdvSearch.Click
            If txtAdvSearchKeywords.Text.Trim <> "" Then _
                RaiseEvent PerformAdvancedSearch(txtAdvSearchKeywords.Text, txtPerPage.Text, _
                        chkIncludeInSearch.Items(0).Selected, _
                        False, _
                        chkIncludeInSearch.Items(1).Selected)
        End Sub

    End Class
End Namespace