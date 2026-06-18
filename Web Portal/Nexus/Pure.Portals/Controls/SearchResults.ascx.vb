Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Xml
Imports System
Namespace Nexus


    Partial Class controls_SearchResults
        Inherits System.Web.UI.UserControl
        'Inherits CMS.Library.Frontend.CMSMasterPage

        Private dtPageAndNews As DataTable
        Private bSearchNews As Boolean = False
        Private bSearchPages As Boolean = True
        Private bSearchMedia As Boolean = False

        Public Property SearchPages() As Boolean
            Get
                Return bSearchPages
            End Get
            Set(ByVal value As Boolean)
                bSearchPages = value
            End Set
        End Property

        Public Property SearchNews() As Boolean
            Get
                Return bSearchNews
            End Get
            Set(ByVal value As Boolean)
                bSearchNews = value
            End Set
        End Property

        Public Property SearchMedia() As Boolean
            Get
                Return bSearchMedia
            End Get
            Set(ByVal value As Boolean)
                bSearchMedia = value
            End Set
        End Property

        Public Property CurrentSearchString() As String
            Get
                Return ViewState("CurrentSearchString")
            End Get
            Set(ByVal value As String)
                ViewState("CurrentSearchString") = value
            End Set
        End Property

        Public Sub DoSearch(ByVal sSearch As String)
            'Call this to start search
            gvResultsPages.PageIndex = 0
            gvResultsNews.PageIndex = 0
            gvMedia.PageIndex = 0
            CurrentSearchString = sSearch
            If SearchPages Then
                BindPageResults(sSearch)
                mvPages.ActiveViewIndex = 0
            Else
                mvPages.ActiveViewIndex = -1
            End If
            If SearchNews Then
                BindNewsResults(sSearch)
                mvNews.ActiveViewIndex = 0
            Else
                mvNews.ActiveViewIndex = -1
            End If
            If SearchMedia Then
                BindMediaResults(sSearch)
                mvMedia.ActiveViewIndex = 0
            Else
                mvMedia.ActiveViewIndex = -1
            End If
        End Sub

        Public Sub DoSearch(ByVal sSearch As String, ByVal PerPage As Integer, ByVal ShowPages As Boolean, ByVal ShowNews As Boolean, ByVal ShowMedia As Boolean)
            SearchPages = ShowPages
            SearchNews = ShowNews
            SearchMedia = ShowMedia
            gvMedia.PageSize = PerPage
            gvResultsNews.PageSize = PerPage
            gvResultsPages.PageSize = PerPage
            DoSearch(sSearch)
        End Sub

        Private Function GetPageNewsData(ByVal sSearch As String) As DataTable
            If dtPageAndNews Is Nothing Then
                'dtPageAndNews = CMS.Library.SiteMap.funcSiteMap.SearchLiveContent(sSearch, HttpContext.Current.User.Identity.IsAuthenticated)
                dtPageAndNews = CMS.Library.SiteMap.SearchLiveContent(sSearch, HttpContext.Current.User.Identity.IsAuthenticated)
            End If
            Return dtPageAndNews
        End Function

        Private Sub BindPageResults(ByVal sSearch As String)
            BindResults(GetPageNewsData(sSearch), mvPages, gvResultsPages, "Page_Type_id=2")
        End Sub

        Private Sub BindNewsResults(ByVal sSearch As String)
            BindResults(GetPageNewsData(sSearch), mvNews, gvResultsNews, "Page_Type_id=3")
        End Sub

        Private Sub BindMediaResults(ByVal sSearch As String)
            'BindResults(CMS.Media.funcMedia.SearchMedia(sSearch), mvMedia, gvMedia, "")
            BindResults(CMS.Library.Media.SearchMedia(sSearch), mvMedia, gvMedia, "")
        End Sub

        Private Sub BindResults(ByVal dt As DataTable, ByVal mv As MultiView, ByVal gv As GridView, ByVal filter As String)
            Dim dv As New DataView(dt, filter, "", DataViewRowState.OriginalRows)
            With gv
                .DataSource = dv
                .DataBind()
            End With
            'mv.Visible = (dv.Count > 0)
            Dim cmd As LinkButton = CType(mv.Views(0).Controls(1), LinkButton)
            cmd.Enabled = (dv.Count <> 0)
            Select Case dv.Count
                Case 0
                    cmd.Text = "No matches found."
                Case 1
                    cmd.Text = "1 match found. Click here to view."
                Case Else
                    cmd.Text = dv.Count & " matches found. Click here to view."
            End Select
        End Sub

        Public Function NavURL(ByVal xmlcontent As String, ByVal pagetype_id As String, ByVal siteid As String, ByVal parentid As String) As String

            Dim xmldoc As New XmlDocument
            xmldoc.LoadXml(xmlcontent)
            Dim root As XmlElement = xmldoc.DocumentElement
            Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(xmldoc.NameTable)
            nsmgr.AddNamespace("pns", "http://tempuri.org/page1.xsd")
            Select Case pagetype_id
                Case "2"
                    If parentid = 0 Then
                        Return AppSettings("WebRoot") & "default.aspx"
                    Else
                        Dim sPageTemplate As String = root.SelectSingleNode("pns:Page/pns:PageTemplate", nsmgr).InnerText

                        If sPageTemplate <> "" Then
                            Return AppSettings("WebRoot") & sPageTemplate & "?sitemap_id=" & siteid
                        Else
                            Return AppSettings("WebRoot") & "main.aspx?sitemap_id=" & siteid
                        End If
                    End If
                Case Else
                    Return AppSettings("WebRoot") & "default.aspx"
            End Select

        End Function

        Public Function MediaPath(ByVal filename As String, ByVal category_id As String) As String
            'Dim catpath As String = CMS.Media.funcCategory.GetCategoryPath(category_id)
            'Return AppSettings("WebRoot") & "media/" & catpath & "/" & filename

            Dim catpath As String = CMS.Library.Media.GetCategoryPath(category_id)
            Return AppSettings("WebRoot") & "MediaLibrary/" & catpath & "/" & filename
        End Function

        Public Function MediaPreview(ByVal filename As String, ByVal category_id As String) As String
            'Dim catpath As String = CMS.Media.funcCategory.GetCategoryPath(category_id)
            'Return CMS.Media.funcMedia.CreateMediaPreviewForFrontEnd(catpath & "/" & filename, 120, 140)
            Dim catpath As String = CMS.Library.Media.GetCategoryPath(category_id)
            Return CMS.Library.Media.CreateMediaPreviewForFrontEnd(catpath & "/" & filename, 120, 140)
        End Function

        Public Function MediaAlt(ByVal alt As String) As String
            If alt.Length > 30 Then
                Return alt.Substring(0, 30) & ".."
            Else
                Return alt
            End If
        End Function

        Public Function SummaryText(ByVal xmlcontent As String, ByVal pagetype_id As String) As String
            Try
                Dim xmldoc As New XmlDocument
                xmldoc.LoadXml(xmlcontent)
                Dim root As XmlElement = xmldoc.DocumentElement
                Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(xmldoc.NameTable)
                nsmgr.AddNamespace("pns", "http://tempuri.org/page1.xsd")
                Select Case pagetype_id
                    Case "2"
                        Return root.SelectSingleNode("pns:Page/pns:Description", nsmgr).InnerText
                    Case "3"
                        Return root.SelectSingleNode("pns:News/pns:Summary", nsmgr).InnerText
                    Case Else
                        Return "..."
                End Select
            Catch ex As System.Exception
                Throw New ArgumentException("...") 'Return "..."
            End Try
        End Function

        Protected Sub gvResultsPages_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResultsPages.Load
            If gvResultsPages.PageCount = 1 Then
                gvResultsPages.AllowPaging = False
            End If
        End Sub

        Protected Sub gvResultsPages_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResultsPages.PageIndexChanging
            gvResultsPages.PageIndex = e.NewPageIndex
            BindPageResults(CurrentSearchString)
        End Sub

        Protected Sub gvResultsNews_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResultsNews.Load
            If gvResultsNews.PageCount = 1 Then
                gvResultsNews.AllowPaging = False
            End If
        End Sub

        Protected Sub gvResultsNews_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResultsNews.PageIndexChanging
            gvResultsNews.PageIndex = e.NewPageIndex
            BindNewsResults(CurrentSearchString)
        End Sub

        Protected Sub gvMedia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMedia.Load
            If gvMedia.PageCount = 1 Then
                gvMedia.AllowPaging = False
            End If
        End Sub

        Protected Sub gvMedia_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMedia.PageIndexChanging
            gvMedia.PageIndex = e.NewPageIndex
            BindMediaResults(CurrentSearchString)
        End Sub

        Protected Sub cmdShowPages_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdShowPages.Click
            mvPages.ActiveViewIndex = 1
        End Sub

        Protected Sub cmdShowNews_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdShowNews.Click
            mvNews.ActiveViewIndex = 1
        End Sub

        Protected Sub cmdShowMedia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdShowMedia.Click
            mvMedia.ActiveViewIndex = 1
        End Sub

    End Class

End Namespace
