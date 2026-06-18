Imports Nexus.Utils
Imports Nexus.Utils.Cache

Imports System.Text
Imports System.Web
Imports System.Configuration
Imports System.Web.HttpContext
Imports System.Web.Security
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Caching

Namespace Frontend

    Public Module Functions

        Sub SetTheme(ByRef sender As System.Web.UI.Page, Optional ByVal pTemplate As String = Nothing)

            Select Case LCase(AppSettings("CMS"))
                Case "none"

                    Dim pagesSection As System.Web.Configuration.PagesSection
                    pagesSection = CType(System.Web.Configuration.WebConfigurationManager.GetSection("system.web/pages"), System.Web.Configuration.PagesSection)

                    If pTemplate Is Nothing Then

                        sender.MasterPageFile = pagesSection.MasterPageFile
                        sender.Theme = pagesSection.Theme
                    Else
                        sender.MasterPageFile = pTemplate
                        sender.Theme = pagesSection.Theme
                    End If

                Case "portal", "full"

                    Dim CurrentPortal As Portal.Portal
                    Dim iPortalID As Integer = Portal.Functions.GetPortalID

                    If Current.Cache.Item("Portal_" & iPortalID) Is Nothing Then
                        'Get from db
                        CurrentPortal = Portal.Functions.GetPortal(iPortalID)
                        If Not CurrentPortal Is Nothing Then
                            Current.Cache.Insert("Portal_" & iPortalID, CurrentPortal, Nothing, _
                                        CacheExpiration(CacheLengthTypes.CacheLong), TimeSpan.Zero)
                        End If
                    Else
                        'Get from cache
                        CurrentPortal = CType(Current.Cache.Item("Portal_" & iPortalID), Portal.Portal)
                    End If

                    If CurrentPortal Is Nothing Then
                        sender.MasterPageFile = "~/default.master"
                        sender.Theme = ""
                    Else
                        If CurrentPortal.MasterPageFolder <> "" AndAlso Current.Session.Item("UserIdentityName") IsNot Nothing Then

                            If pTemplate Is Nothing Then
                                sender.MasterPageFile = "~/" & CurrentPortal.MasterPageFolder & "/" & AppSettings("SubPageTemplate")
                            Else
                                sender.MasterPageFile = "~/" & CurrentPortal.MasterPageFolder & "/" & pTemplate
                            End If

                            sender.Theme = CurrentPortal.Theme

                            With CType(sender, System.Web.UI.Page).Master.Attributes
                                .Add("PortalID", CurrentPortal.ID.ToString)
                                .Add("PortalName", CurrentPortal.Name)
                            End With

                        Else
                            sender.MasterPageFile = "~/default.master"
                            sender.Theme = ""
                        End If
                    End If

                Case Else

                    sender.MasterPageFile = "~/default.master"
                    sender.Theme = ""

            End Select

        End Sub

        Public Function GetFrontEndPage(ByVal pSiteMapID As Integer, ByVal pHistoryID As Integer, _
                                    ByVal pPreview As String, Optional ByVal pGUID As String = "") As SiteMap.SiteMapContent

            Dim tmpContent As SiteMap.SiteMapContent = Nothing
            Dim MyCache As System.Web.Caching.Cache
            Dim sCacheKey As String = ""

            'Dim bError As Boolean = True 'always assume error

            If IsNumeric(pSiteMapID) Then

                If pPreview Is Nothing Then
                    'Published Content
                    MyCache = System.Web.HttpContext.Current.Cache
                    sCacheKey = "Content_" & pSiteMapID
                    If MyCache.Item(sCacheKey) Is Nothing = False Then
                        tmpContent = CType(MyCache.Item(sCacheKey), SiteMap.SiteMapContent)
                    Else
                        tmpContent = New SiteMap.SiteMapContent(pSiteMapID, False, False)
                        MyCache.Insert(sCacheKey, tmpContent, Nothing)

                    End If

                    If tmpContent.Restricted Then
                        If Current.User.Identity.IsAuthenticated Then
                            'Check for permission from roles
                            Dim HasPermission As Boolean = False
                            Dim sRole As String
                            Dim tmpUserRoles() As String = tmpContent.Roles.Split(",")
                            If tmpUserRoles.Length > 0 Then
                                For Each sRole In tmpUserRoles
                                    If sRole.Trim <> "" Then
                                        If Roles.IsUserInRole(sRole) Then HasPermission = True
                                    End If
                                Next
                            End If
                            If Not HasPermission Then
                                'change for CMS content for unauthorised page
                                'hold sitemapID in config?
                                Current.Response.Redirect("unauthorised.aspx")
                            End If
                        Else
                            Dim config As System.Web.Configuration.AuthenticationSection = CType(System.Web.Configuration.WebConfigurationManager.GetSection("system.web/authentication"), System.Web.Configuration.AuthenticationSection)

                            Current.Response.Redirect(config.Forms.LoginUrl & "?ReturnUrl=" _
                                            & Current.Request.RawUrl)
                        End If
                    End If

                    'BODGE - This will be replaced with XSLT, when we get round to it ...
                    Select Case tmpContent.SchemaID
                        Case 1 ' Fastlink
                            Dim sUrl As String = String.Empty
                            Select Case tmpContent.Element("TypeOfLink").ToString
                                Case "0" 'External
                                    sUrl = tmpContent.Element("Url").ToString
                                Case "1" 'Internal
                                    sUrl = funcUtils.WebRoot & tmpContent.Element("Url").ToString
                            End Select

                            If tmpContent.Element("Target").ToString = "_blank" Then
                                HttpContext.Current.Response.Write("<script language=""Javascript"">open('" _
                                    & sUrl & "');</script>")
                            Else
                                'redirect
                                HttpContext.Current.Response.Redirect(sUrl, False)
                            End If
                        Case 2, 5 'Page, News_Category

                            'Check current page matchs the set template for the content, if not redirect to correct template

                            Dim pagetemplate As String = tmpContent.Element("PageTemplate")
                            If pagetemplate IsNot Nothing Then
                                If pagetemplate <> "" Then

                                    Dim iNestedPrefix As Integer = AppSettings("WebRoot").Split("/").Length - 1
                                    Dim iNestedDirectories As Integer = (Current.Request.Url.Segments.Length - 1) - iNestedPrefix
                                    Dim CurrentPage As String = String.Empty
                                    For x As Integer = iNestedDirectories To 0 Step -1

                                        CurrentPage &= Current.Request.Url.Segments(Current.Request.Url.Segments.Length - (x + 1))

                                    Next

                                    'CurrentPage &= HttpContext.Current.Request.Url.Segments(HttpContext.Current.Request.Url.Segments.Length - 1)

                                    If pagetemplate <> CurrentPage Then
                                        Current.Response.Redirect(pagetemplate & "?sitemap_id=" & pSiteMapID)
                                    End If
                                End If
                            End If
                        Case 3 'News

                            'use parent container page template to display content
                            'if available, otherwise display on the current page
                    End Select

                Else
                    'Preview Draft Content
                    If SiteMap.Functions.IsValidGUID(pGUID) Then
                        tmpContent = New SiteMap.SiteMapContent(pSiteMapID, True)

                        'Check current page matchs the set template for the content, if not redirect to correct template
                        Dim CurrentPage As String = HttpContext.Current.Request.Url.Segments(HttpContext.Current.Request.Url.Segments.Length - 1)
                        Dim pagetemplate As String = tmpContent.Element("PageTemplate").ToString
                        If pagetemplate IsNot Nothing Then
                            If pagetemplate <> "" Then
                                If pagetemplate <> CurrentPage Then
                                    Current.Response.Redirect(pagetemplate & "?sitemap_id=" _
                                        & pSiteMapID & "&preview=true&guid=" & pGUID)
                                End If
                            End If
                        End If
                    Else
                        'Error, handled by the final if statement
                        tmpContent = New SiteMap.SiteMapContent()
                    End If
                End If
            Else
                If IsNumeric(pHistoryID) Then
                    'Archived Content
                    If SiteMap.Functions.IsValidGUID(pGUID) Then
                        Dim iHistoryID As Integer = CInt(pHistoryID)
                        tmpContent = New SiteMap.SiteMapContent(iHistoryID)

                        'Check current page matchs the set template for the content, if not redirect to correct template
                        Dim CurrentPage As String = HttpContext.Current.Request.Url.Segments(HttpContext.Current.Request.Url.Segments.Length - 1)
                        Dim pagetemplate As String = tmpContent.Element("PageTemplate").ToString
                        If pagetemplate IsNot Nothing Then
                            If pagetemplate <> "" Then
                                If pagetemplate <> CurrentPage Then
                                    Current.Response.Redirect(pagetemplate & "?archive_id=" _
                                        & pHistoryID & "&guid=" & pGUID)
                                End If
                            End If
                        End If
                    Else
                        'Error, handled by the final if statement
                        tmpContent = New SiteMap.SiteMapContent()
                    End If
                Else
                    'Error, handled by the final if statement
                    tmpContent = New SiteMap.SiteMapContent()
                End If
            End If

            Return tmpContent

        End Function

        Function GetNavigation(ByVal pTopID As Integer, ByVal pDepth As Integer, ByVal pShowRoot As Boolean) As ArrayList

            Dim alTmp As New ArrayList

            Dim UserIsAuthenticated As Boolean = False
            Try
                UserIsAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated
            Catch ex As Exception

            End Try

            Dim AllowReWrite As Boolean = False

            Try
                AllowReWrite = Convert.ToBoolean(AppSettings("ReWriteURLs"))
            Catch ex As Exception

            End Try


            'Check for cache copy and use it if found
            If Not HttpContext.Current.Cache("Nav_" & pTopID & UserIsAuthenticated) Is Nothing Then
                alTmp = CType(HttpContext.Current.Cache("Nav_" & pTopID & UserIsAuthenticated), ArrayList)
            Else
                'otherwise create from db

                Dim arTmp As ArrayList = SiteMap.Functions.GetSiteMap(pTopID, pDepth, 1, pShowRoot)   'Only ever show published nodes on the tree
                Dim SiteMapTmp As MMSiteMapNode

                Dim dLiveDate As DateTime = Date.MaxValue
                Dim dExpiryDate As DateTime = Date.MaxValue

                Dim tmpNavLink As NavLink

                Dim bNoCache As Boolean = False

                Dim enumTmp As IEnumerator = arTmp.GetEnumerator
                While enumTmp.MoveNext

                    'HACK: More BODGE - all replaced with XSLT

                    Select Case CType(enumTmp.Current, MMSiteMapNode).PageTypeName
                        Case "page", "news_category"

                            SiteMapTmp = CType(enumTmp.Current, MMSiteMapNode)
                            tmpNavLink = New NavLink(SiteMapTmp.Label)

                            If SiteMapTmp.LiveDate < dLiveDate Then
                                dLiveDate = SiteMapTmp.LiveDate

                            End If
                            If SiteMapTmp.ExpiryDate < dExpiryDate Then
                                dExpiryDate = SiteMapTmp.ExpiryDate
                            End If

                            If SiteMapTmp.ParentID = 0 Then
                                'Home node always goes to default.aspx
                                tmpNavLink.Url = funcUtils.WebRoot & "default.aspx"
                            Else
                                Dim CurrentContent As New SiteMap.SiteMapContent(SiteMapTmp.SiteMapID, False)
                                Dim pagetemplate As String = CurrentContent.Element("PageTemplate").ToString
                                If Not pagetemplate = "" Then
                                    tmpNavLink.Url = funcUtils.WebRoot & CurrentContent.Element("PageTemplate").ToString _
                                                            & "?sitemap_id=" & SiteMapTmp.SiteMapID
                                Else
                                    If AllowReWrite Then
                                        tmpNavLink.Url = funcUtils.WebRoot & SiteMapTmp.FullFolderPath
                                    Else
                                        tmpNavLink.Url = funcUtils.WebRoot & "main.aspx?sitemap_id=" & SiteMapTmp.SiteMapID
                                    End If

                                End If
                            End If

                            tmpNavLink.SiteID = SiteMapTmp.SiteMapID
                            tmpNavLink.Depth = SiteMapTmp.Depth

                            tmpNavLink.Title = SiteMapTmp.Label

                            ' David Colliver.
                            ' Required Menu ID in order to create drop down menus.
                            tmpNavLink.MenuID = SiteMapTmp.SiteMapID

                            Dim addLink As Boolean = False

                            If SiteMapTmp.AnonymousOnly And SiteMapTmp.AuthenticatedOnly Then
                                addLink = True
                            ElseIf Not SiteMapTmp.AnonymousOnly And SiteMapTmp.AuthenticatedOnly Then
                                If UserIsAuthenticated Then
                                    addLink = True
                                Else
                                    addLink = False
                                End If
                            ElseIf SiteMapTmp.AnonymousOnly And Not SiteMapTmp.AuthenticatedOnly Then
                                If Not UserIsAuthenticated Then
                                    addLink = True
                                Else
                                    addLink = False
                                End If
                            End If
                            If addLink Then
                                alTmp.Add(tmpNavLink)
                            End If

                        Case "fastlink"
                            SiteMapTmp = CType(enumTmp.Current, MMSiteMapNode)
                            tmpNavLink = New NavLink(SiteMapTmp.Label)

                            If SiteMapTmp.LiveDate < dLiveDate Then
                                dLiveDate = SiteMapTmp.LiveDate
                            End If
                            If SiteMapTmp.ExpiryDate < dExpiryDate Then
                                dExpiryDate = SiteMapTmp.ExpiryDate
                            End If

                            Dim CurrentContent As New SiteMap.SiteMapContent(SiteMapTmp.SiteMapID, False)

                            Select Case CurrentContent.Element("TypeOfLink").ToString
                                Case "0" 'External
                                    tmpNavLink.Url = CurrentContent.Element("Url").ToString
                                Case "1" 'Internal
                                    tmpNavLink.Url = funcUtils.WebRoot & CurrentContent.Element("Url").ToString
                            End Select

                            tmpNavLink.Target = CurrentContent.Element("Target").ToString


                            Dim addLink As Boolean = False

                            If SiteMapTmp.AnonymousOnly And SiteMapTmp.AuthenticatedOnly Then
                                addLink = True
                            ElseIf Not SiteMapTmp.AnonymousOnly And SiteMapTmp.AuthenticatedOnly Then
                                If UserIsAuthenticated Then
                                    addLink = True
                                Else
                                    addLink = False
                                End If
                            ElseIf SiteMapTmp.AnonymousOnly And Not SiteMapTmp.AuthenticatedOnly Then
                                If Not UserIsAuthenticated Then
                                    addLink = True
                                Else
                                    addLink = False
                                End If
                            End If
                            If addLink Then
                                alTmp.Add(tmpNavLink)
                            End If
                        Case "news"
                            'News is NOT visible on the menu, maybe make this configurable ...
                    End Select

                End While

                If alTmp IsNot Nothing Then

                    Current.Cache.Insert("Nav_" & pTopID & UserIsAuthenticated, alTmp, Nothing, _
                        CacheExpiration(CacheLengthTypes.CacheLong, dLiveDate, dExpiryDate), TimeSpan.Zero)
                End If

            End If

            Return alTmp

        End Function

        Function GetNewsSummaries(ByVal pNumberToShow As Integer, Optional ByVal pNewsCategoryID As Integer = 0) As ArrayList

            'Just call the actual function, to try and seperate presentation from data, will use XSLT at some point ...
            Return SiteMap.Functions.GetNews(pNumberToShow, pNewsCategoryID)

        End Function

        Function GetTopNewsStory(ByVal pNewsCategoryID As Integer) As SiteMap.SiteMapContent

            Dim tmpSiteMapContent As SiteMap.SiteMapContent
            Dim arTmp As ArrayList = SiteMap.Functions.GetNews(1, pNewsCategoryID)
            If arTmp.Count = 1 Then
                tmpSiteMapContent = CType(arTmp(0), SiteMap.SiteMapContent)
            Else
                'Not enough (or too many, but unlikely)
                tmpSiteMapContent = Nothing
            End If

            Return tmpSiteMapContent

        End Function

        Function GetNewsSubCategories(ByVal pNewsCategoryID As Integer) As ArrayList

            Dim arTmp As New ArrayList
            Dim tmpNode As MMSiteMapNode

            For Each tmpNode In SiteMap.Functions.GetSiteMap(pNewsCategoryID, 1, 1, False)
                If tmpNode.PageTypeName = "news_category" Then
                    arTmp.Add(tmpNode)
                End If
            Next

            Return arTmp

        End Function

        Function GetBanner() As String
            Return Banner.Functions.GetRandomBannerContent
        End Function

        Function ResetCache(ByVal pGUID As String, ByVal pSiteMapID As String, _
                            ByVal pParentID As String, Optional ByVal pKey As String = "") As Boolean

            Dim bSuccess As Boolean = False

            'Secure this page using a guid
            If pGUID = System.Web.Configuration.WebConfigurationManager.AppSettings("ResetCacheGUID") Then
                If IsNumeric(pSiteMapID) Then
                    'Reset cache for requested page
                    Current.Cache.Remove("Content_" & pSiteMapID)

                    If IsNumeric(pParentID) Then
                        'Reset Navigation for page, referenced by the parent_id
                        Current.Cache.Remove("Nav_" & SiteMap.Functions.GetParentID(pParentID) & "true")
                        Current.Cache.Remove("Nav_" & SiteMap.Functions.GetParentID(pParentID) & "false")
                    End If
                Else
                    'Reset cache for requested key
                    If pKey <> "" Then
                        Current.Cache.Remove(pKey)
                    End If
                End If

                bSuccess = True

            End If

            Return bSuccess

        End Function

    End Module

End Namespace