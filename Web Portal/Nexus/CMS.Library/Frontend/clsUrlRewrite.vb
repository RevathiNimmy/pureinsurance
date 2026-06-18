Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils

Namespace Frontend

    Public Class clsUrlRewrite

        Private _arNavigation As ArrayList
        Private Property arNavigation() As ArrayList
            Get
                Return _arNavigation
            End Get
            Set(ByVal value As ArrayList)
                _arNavigation = value
            End Set
        End Property

        Sub SendPage()

            ' In order to make this work without having an extension on the page name (i.e. you want page instead of page.aspx)
            ' you need to create a 404 in IIS that points to a NON EXISTANT .NET PAGE.
            ' e.g. /web.ssp/nopageexists.aspx (This must be set as a URL rather than a file)
            ' This will force the global.asax to run, which will call this code.
            ' Folders with the same name as the re-written URL will also cause problems, as a folder name 
            ' does not present a 404, in which case, the IIS 404 will not run and therefore, the global.asax
            ' will not run.
            ' Also required will be TopId and ReWriteURLs keys in the Appsetting in web.config for the application/site

            Dim AllowReWrite As Boolean = False

            Try
                AllowReWrite = Convert.ToBoolean(AppSettings("ReWriteURLs"))
            Catch ex As Exception

            End Try

            If AllowReWrite Then

                Dim incoming As System.Web.HttpContext = Web.HttpContext.Current
                Dim currentURL As String = incoming.Request.Path.ToLower()

                'If Not System.IO.File.Exists(incoming.Server.MapPath("/") & currentURL) Then
                If Not System.IO.File.Exists(incoming.Server.MapPath(currentURL)) Then


                    Dim Qry As String = incoming.Server.UrlDecode(incoming.Request.QueryString.ToString())
                    Dim WorkingUrl As String = currentURL


                    If Qry.IndexOf("404;") >= 0 Then
                        'incoming.Response.Write(Qry)
                        WorkingUrl = Qry.ToLower().Replace("404;", "").Replace("http://", "").Replace("https://", "").Replace(incoming.Request.ServerVariables("server_name"), "")

                        If WorkingUrl.IndexOf(":") = 0 Then
                            ' A port number is in the string... we need to remove it.
                            WorkingUrl = WorkingUrl.Remove(0, WorkingUrl.IndexOf("/"))
                        End If

                    End If

                    If Not ( _
                    WorkingUrl.EndsWith(".css") Or _
                    WorkingUrl.EndsWith(".jpg") Or _
                    WorkingUrl.EndsWith(".gif") Or _
                    WorkingUrl.EndsWith(".png") Or _
                    WorkingUrl.EndsWith(".js")) Then

                        'Dim arNavigation As ArrayList = Functions.GetNavigation(CInt(AppSettings("TopId")), CInt(AppSettings("Depth")), CBool(AppSettings("ShowRoot")))
                        arNavigation = SiteMap.Functions.GetSiteMap(AppSettings("TopId"))


                        If arNavigation.Count > 0 Then

                            Dim TargetUrl As String = WorkingUrl.ToLower()

                            If funcUtils.WebRoot = "/" Then
                                If WorkingUrl.StartsWith("/") Then
                                    TargetUrl = WorkingUrl.Remove(0, 1)
                                End If
                            Else
                                TargetUrl = WorkingUrl.Replace(funcUtils.WebRoot.ToLower(), "")
                            End If


                            ' Need to wire in the BuildFolderHierarchy.
                            ' Problem is, it will slow the site down as the folder hierarchy is generated every url-re-written page request.
                            'Dim DT As DataTable = New DataTable("Hierarchy")
                            'DT.Columns.Add(New DataColumn("SiteMapID"))
                            'DT.Columns.Add(New DataColumn("ParentID"))
                            ''DT.Columns.Add(New DataColumn("Label"))
                            'DT.Columns.Add(New DataColumn("Depth"))
                            'DT.Columns.Add(New DataColumn("FullFolderPath"))

                            'BuildPath(TargetUrl, "", 0)

                            For Each PathRow As MMSiteMapNode In arNavigation
                                'Dim PathLabel = PathRow.Label
                                Dim PathValue As String = PathRow.FullFolderPath

                                'Dim newrow As DataRow = DT.NewRow()
                                'newrow("SiteMapID") = PathRow.SiteMapID
                                'newrow("ParentID") = PathRow.ParentID
                                'newrow("Depth") = depth
                                'newrow("Label") = PathRow.Label
                                'Dim parent As DataRow() = DT.Select("SiteMapID = " & PathRow.ParentID & " and ParentID <> 0")
                                'Dim PathValue As String = String.Empty
                                'If parent.Length > 0 Then
                                'For Each dr As DataRow In parent
                                ' Will only fall into this loop ONCE.
                                'PathValue = dr("FullFolderPath")
                                'Next
                                'End If


                                'newrow("FullFolderPath") = PathValue & PathLabel & "/"
                                'DT.Rows.Add(newrow)

                                'incoming.Response.Write(TargetUrl & " : " & PathValue & "<br />")

                                'If TargetUrl = (PathValue & PathLabel & "/").ToLower() Or TargetUrl = (PathValue & PathLabel).ToLower() Then
                                If TargetUrl.ToLower() & "/" = PathValue.ToString().ToLower() Or TargetUrl.ToLower() = PathValue.ToString().ToLower() Then

                                    'incoming.Response.Write("INSIDE : " & TargetUrl & " : " & PathValue & "<br />")

                                    If PathRow.ParentID = 0 Then
                                        incoming.RewritePath(funcUtils.WebRoot & "default.aspx", False)
                                    Else

                                        Dim CurrentContent As New SiteMap.SiteMapContent(PathRow.SiteMapID, False)
                                        Dim pagetemplate As String = CurrentContent.Element("PageTemplate").ToString

                                        If Not pagetemplate = "" Then
                                            incoming.RewritePath(funcUtils.WebRoot & CurrentContent.Element("PageTemplate").ToString _
                                                                    & "?sitemap_id=" & PathRow.SiteMapID, False)
                                        Else
                                            incoming.RewritePath(funcUtils.WebRoot & "main.aspx?sitemap_id=" & PathRow.SiteMapID, False)
                                        End If
                                    End If

                                    Exit For

                                End If
                                'incoming.Response.Write(TargetUrl & " : " & PathValue & "<br />")
                            Next

                            'incoming.Response.End()
                        End If
                    End If
                End If
                'incoming.Response.End()
            End If

        End Sub

        'private void BuildFolderHierarchy(ref DataTable orderedTable, DataRow[] members, string PathSoFar, int depth)
        '{
        '	foreach (DataRow member in members) 
        '	{
        '		string CurrentPath = PathSoFar + member["FolderName"].ToString() + "/";
        '		orderedTable.ImportRow(member); 
        '		orderedTable.Rows[orderedTable.Rows.Count - 1]["Depth"] = depth;
        '		orderedTable.Rows[orderedTable.Rows.Count - 1]["FullFolderPath"] = CurrentPath;
        '		BuildFolderHierarchy(ref orderedTable, member.GetChildRows("ParentChild"), CurrentPath, depth + 1);
        '	} 
        '}

        'Private Sub BuildPath(ByVal TargetUrl As String, ByVal CurrentPath As String, ByVal depth As Int32)

        '    Dim incoming As System.Web.HttpContext = Web.HttpContext.Current

        '    For Each PathRow As MMSiteMapNode In arNavigation
        '        Dim PathLabel = PathRow.Label
        '        'Dim newrow As DataRow = DT.NewRow()
        '        'newrow("SiteMapID") = PathRow.SiteMapID
        '        'newrow("ParentID") = PathRow.ParentID
        '        'newrow("Depth") = depth
        '        'newrow("Label") = PathRow.Label
        '        Dim parent As DataRow() = DT.Select("SiteMapID = " & PathRow.ParentID & " and ParentID <> 0")
        '        Dim PathValue As String = String.Empty
        '        If parent.Length > 0 Then
        '            For Each dr As DataRow In parent
        '                ' Will only fall into this loop ONCE.
        '                PathValue = dr("FullFolderPath")
        '            Next
        '        End If

        '        newrow("FullFolderPath") = PathValue & PathLabel & "/"
        '        DT.Rows.Add(newrow)

        '        incoming.Response.Write(TargetUrl & " : " & PathValue & PathLabel & "/<br />")

        '        If TargetUrl = (PathValue & PathLabel & "/").ToLower() Or TargetUrl = (PathValue & PathLabel).ToLower() Then

        '            incoming.Response.Write("INSIDE : " & TargetUrl & " : " & PathValue & PathLabel & "/<br />")

        '            If PathRow.ParentID = 0 Then
        '                incoming.RewritePath(funcUtils.WebRoot & "default.aspx")
        '            Else

        '                Dim CurrentContent As New SiteMap.SiteMapContent(PathRow.SiteMapID, False)
        '                Dim pagetemplate As String = CurrentContent.Element("PageTemplate").ToString

        '                If Not pagetemplate = "" Then
        '                    incoming.RewritePath(funcUtils.WebRoot & CurrentContent.Element("PageTemplate").ToString _
        '                                            & "?sitemap_id=" & PathRow.SiteMapID)
        '                Else
        '                    incoming.RewritePath(funcUtils.WebRoot & "main.aspx?sitemap_id=" & PathRow.SiteMapID)
        '                End If
        '            End If

        '            Exit For

        '        Else
        '            BuildPath(DT, TargetUrl, PathValue & PathLabel & "/", depth + 1)
        '        End If
        '        incoming.Response.Write(TargetUrl & " : " & PathValue & PathLabel & "/<br />")
        '    Next

        'End Sub

    End Class

End Namespace