'Imports System.Data
'Imports CMS.library
'Imports MM.Utils
'Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    Partial Class _Search
        Inherits CMS.Library.Frontend.clsCMSPage
        Public lRowCount As Long
        Public lCurrEndRecCount As Long
        Public lCurrStartRecCount As Long = 0
        Public SearchString As String

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            CType(Me.Master.FindControl("PageSubTitle"), Literal).Text = "Search"
            'CType(Master, CMSMasterPage).Title = "Search"

            If Request.QueryString("Query") <> "" Then
                SearchBox.Query = Request.QueryString("Query")
                SearchBox_PerformSearch(Request.QueryString("Query"))
            End If
        End Sub
        Protected Sub SearchBox_PerformAdvancedSearch(ByVal sSearch As String, ByVal ShowPerPage As Integer, ByVal ShowPages As Boolean, ByVal ShowNews As Boolean, ByVal ShowMedia As Boolean) Handles SearchBox.PerformAdvancedSearch
            SearchResults.DoSearch(sSearch, ShowPerPage, ShowPages, ShowNews, ShowMedia)
        End Sub

        Protected Sub SearchBox_PerformSearch(ByVal sSearch As String) Handles SearchBox.PerformSearch
            SearchResults.DoSearch(sSearch)
        End Sub

    End Class

End Namespace
