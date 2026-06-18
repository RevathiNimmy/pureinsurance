Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports System.Text
Imports System.IO
Imports System.xml
Imports System.Web.Security
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.UI.WebControls

'Imports MM
Imports Nexus.Utils
Imports Nexus.Utils.Cache
'Imports Nexus.Utils.Nexus

Namespace SiteMap

    Public Module Functions

        Const CustomPropertiesXMLFile As String = "~/../configuration/ContentCustomProperties.xml"

        Sub GetCurrentContent(ByRef pContent As SiteMap.SiteMapContent, ByVal pSiteMapID As Integer)

            'Gets a reference to the sitemap content specified by the sitemapid and caches it

            If Current.Cache.Item("Draft_Content_" & pSiteMapID) Is Nothing Then
                pContent = New SiteMap.SiteMapContent(pSiteMapID, True)
                Current.Cache.Insert("Draft_Content_" & pSiteMapID, pContent, Nothing, CacheExpiration(CacheLengthTypes.CacheShort), TimeSpan.Zero)
            Else
                pContent = CType(Current.Cache.Item("Draft_Content_" & pSiteMapID), SiteMap.SiteMapContent)
            End If

        End Sub

        Function GetSiteMap(Optional ByVal pTopCatID As Integer = 0, _
                        Optional ByVal pDepth As Integer = -1, _
                        Optional ByVal pPublished As Integer = 1, _
                        Optional ByVal pShowRoot As Boolean = True) As ArrayList

            'Returns the whole sitemap or part, down to a specified depth, in either
            'draft or live, whether the root node is return is also optional

            Dim command As New SqlCommand("usp_GetSiteMap")

            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@top_id", SqlDbType.Int).Value = pTopCatID
                .Add("@depth_below_parent", SqlDbType.Int).Value = pDepth
                .Add("@published", SqlDbType.Int).Value = pPublished
                .Add("@show_root", SqlDbType.Bit).Value = pShowRoot
                '.Add("@user_id", SqlDbType.Int).Value = funcUtils.NullToZero(HttpContext.Current.User.Identity.Name) 'frontend userid
                '.Add("@user_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey
            End With

            Dim arTmp As ArrayList = New ArrayList
            'Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
            Dim DT As DataTable = funcDB.GetDataTable(command, "CMS")

            DT.Columns.Add(New DataColumn("FullFolderPath", Type.GetType("System.String")))

            Dim sdtTmp As DataTable = DT.Clone()

            'While sdrTmp.Read
            Dim sdrTmp As DataRow

            If Convert.ToBoolean(AppSettings("ReWriteURLs")) Then
                'Only build hierarchy if we need to. We only need to if we are allowing URL ReWriting.
                Dim StartPath As String = String.Empty

                If pTopCatID > 0 Then
                    'Construct the parent path...
                    Dim rwCommand As New SqlCommand("usp_GetSiteMapToRoot")

                    With rwCommand.Parameters
                        .Add("@ParentID", SqlDbType.Int).Value = pTopCatID
                    End With
                    Dim dr As SqlDataReader = funcDB.ExecSql(rwCommand, "CMS")


                    While dr.Read
                        StartPath = dr.Item(0).ToString()
                    End While

                    dr.Close()
                    rwCommand.Dispose()
                End If

                BuildFolderHierarchy(sdtTmp, DT, pTopCatID, StartPath)
            Else
                sdtTmp = DT
            End If


            For Each sdrTmp In sdtTmp.Rows

                Dim dLiveDate, dExpiryDate As DateTime
                Dim sFullFolderPath As String

                If IsDBNull(sdrTmp("live_date")) Then
                    dLiveDate = DateTime.MinValue
                Else
                    dLiveDate = CDate(sdrTmp("live_date"))
                End If

                If IsDBNull(sdrTmp("expiry_date")) Then
                    dExpiryDate = DateTime.MinValue
                Else
                    dExpiryDate = CDate(sdrTmp("expiry_date"))
                End If

                If IsDBNull(sdrTmp("FullFolderPath")) Then
                    sFullFolderPath = String.Empty
                Else
                    sFullFolderPath = sdrTmp("FullFolderPath")
                End If

                arTmp.Add(New MMSiteMapNode(sdrTmp("sitemap_id"), sdrTmp("parent_id"), sdrTmp("depth"), sdrTmp("page_type_name"), _
                                    sdrTmp("label"), dLiveDate, dExpiryDate, sdrTmp("hidden"), sdrTmp("authenticatedOnly"), sdrTmp("anonymousOnly"), _
                                    sdrTmp("submitted"), sdrTmp("approved"), sdrTmp("published"), sdrTmp("deleted"), sFullFolderPath))
            Next

            sdtTmp.Dispose()    '.Close()
            command.Dispose()

            Return arTmp

        End Function

        Private Sub BuildFolderHierarchy(ByRef dTable As DataTable, ByVal SourceTable As DataTable, ByVal Parent As Int32, ByVal PathSoFar As String)

            Dim members As DataRow() = SourceTable.Select("parent_id = " & Parent)

            Dim member As DataRow

            If PathSoFar.Length > 1 Then
                PathSoFar += "/"
            End If
            For Each member In members
                Dim CurrentPath As String = String.Empty
                If member("parent_id") > 0 Then
                    CurrentPath = PathSoFar + member("label").ToString()
                End If
                dTable.ImportRow(member)
                dTable.Rows(dTable.Rows.Count - 1)("FullFolderPath") = CurrentPath
                BuildFolderHierarchy(dTable, SourceTable, member("sitemap_id"), CurrentPath)
            Next

        End Sub

        Function AddNode(ByVal pLabel As String, ByVal pPageTypeName As String, Optional ByVal pParentID As Integer = 0, _
                        Optional ByVal pPortalID As Integer = 0) As Integer

            'Add a new node to the sitemap

            'Create SiteMap table entry
            'Create Live Content table entry
            'Create Draft Content table entry
            'Create 'Creation' entry in History table

            'Draft is a child of Live and there can be multiple children

            Dim command As New SqlCommand("usp_AddSiteMapNode")

            With command.Parameters
                .Add("@parent_id", SqlDbType.Int).Value = pParentID
                If pPortalID <= 0 Then
                    .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                Else
                    .Add("@portal_id", SqlDbType.Int).Value = pPortalID
                End If

                .Add("@label", SqlDbType.VarChar, 30).Value = pLabel.TrimEnd
                .Add("@page_type_name", SqlDbType.VarChar, 30).Value = pPageTypeName
                .Add("@admin_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey
                .Add("@sitemap_id", SqlDbType.Int).Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")
            Dim iSiteMapID As Integer = command.Parameters("@sitemap_id").Value
            command.Dispose()

            Return iSiteMapID

        End Function

        Sub DeleteNode(ByVal pSiteMapID As Integer, ByVal pDelete As Boolean)

            'Not actually a delete, just sets the delete flag, this way it can be undeleted

            Dim command As New SqlCommand("usp_DeleteSiteMap")

            With command
                .Parameters.Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
                .Parameters.Add("@admin_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey
                .Parameters.Add("@delete", SqlDbType.Bit).Value = pDelete

                funcDB.ExecNonQuery(command, "CMS")

                .Dispose()
            End With

        End Sub

        Sub PublishNode(ByVal pSiteMapID As Integer, ByVal pPublish As Boolean)

            Dim command As New SqlCommand("usp_PublishSiteMap")

            With command
                .Parameters.Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
                .Parameters.Add("@publish", SqlDbType.Int).Value = pPublish
                .Parameters.Add("@admin_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

                funcDB.ExecNonQuery(command, "CMS")

                .Dispose()
            End With

        End Sub

        Sub ApproveNode(ByVal pSiteMapID As Integer)

            Dim command As New SqlCommand("usp_ApproveSiteMap")

            With command
                .Parameters.Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
                .Parameters.Add("@admin_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

                funcDB.ExecNonQuery(command, "CMS")

                .Dispose()
            End With

        End Sub

        Sub ChangeNodeDepth(ByVal pSourceSiteMapID As Integer, ByVal pDestinationSiteMapID As Integer)

            Dim command As New SqlCommand("usp_ChangeSiteMapDepth")

            With command
                .Parameters.Add("@source_sitemap_id", SqlDbType.Int).Value = pSourceSiteMapID
                .Parameters.Add("@dest_sitemap_id", SqlDbType.Int).Value = pDestinationSiteMapID
                .Parameters.Add("@admin_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

                funcDB.ExecNonQuery(command, "CMS")
                .Dispose()
            End With

        End Sub

        Function GetNodeHistoryDT(ByVal pSiteMapID As Integer) As DataTable

            Dim command As New SqlCommand("usp_GetContentHistory")
            command.Parameters.Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID

            Dim dtNodeHistory As DataTable = funcDB.GetDataTable(command, "CMS")
            command.Dispose()

            Return dtNodeHistory

        End Function

        Function GetNodeHistory(ByVal pSiteMapID As Integer) As ArrayList

            Dim arTmp As New ArrayList
            Dim command As New SqlCommand("usp_GetContentHistory")

            command.Parameters.Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command)

            While sdrTmp.Read
                arTmp.Add(New ContentHistory(sdrTmp("history_id"), sdrTmp("edit_date"), _
                                            sdrTmp("admin_name"), sdrTmp("action_label")))
            End While

            sdrTmp.Close()
            command.Dispose()

            Return arTmp

        End Function

        Function GetNodeHistoryPaged(ByVal pSiteMapID As Integer, ByVal pCurrentPage As Integer, ByVal pPageSize As Integer) As ArrayList

            Dim arTmp As New ArrayList
            Dim command As New SqlCommand("usp_GetContentHistoryPaged")

            command.Parameters.Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
            command.Parameters.Add("@currentPage", SqlDbType.Int).Value = pCurrentPage
            command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pPageSize

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command)

            While sdrTmp.Read
                arTmp.Add(New ContentHistory(sdrTmp("history_id"), sdrTmp("edit_date"), _
                                            sdrTmp("admin_name"), sdrTmp("action_label")))
            End While

            sdrTmp.Close()
            command.Dispose()

            Return arTmp

        End Function

        Function GetContentHistorySummary() As DataTable

            Dim command As New SqlCommand("usp_GetContentHistorySummary")
            command.Parameters.Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()

            GetContentHistorySummary = GetDataTable(command, "CMS")
            command.Dispose()

        End Function

        Function GetContentToPublishSummary() As DataTable

            Dim command As New SqlCommand("usp_GetContentForPublish")
            command.Parameters.Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()

            GetContentToPublishSummary = GetDataTable(command, "CMS")
            command.Dispose()

        End Function

        Function GetContentToApproveSummary() As DataTable

            Dim command As New SqlCommand("usp_GetContentForApproval")
            command.Parameters.Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()

            GetContentToApproveSummary = GetDataTable(command, "CMS")
            command.Dispose()

        End Function

        Function GetParentID(ByVal pSiteMapID As Integer) As Integer

            Dim iParentID As Integer

            Dim command As New SqlCommand("usp_GetParentID")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
                .Add("@parent_id", SqlDbType.Int).Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")

            iParentID = command.Parameters("@parent_id").Value

            command.Dispose()
            Return iParentID

        End Function

        Sub RollBackNode(ByVal pSiteMapID As Integer, ByVal pHistoryID As Integer)

            'Replace a current sitemap item with one from the archive, means you can undo changes

            Dim command As New SqlCommand("usp_RollBackSiteMap")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
                .Add("@history_id", SqlDbType.Int).Value = pHistoryID
                .Add("@admin_guid", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

            End With

            funcDB.ExecNonQuery(command, "CMS")
            command.Dispose()

        End Sub

        Function GetHomeLabel() As String
            Dim command As New SqlCommand("usp_GetPortalHomePageTitle")

            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@label", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")

            Dim sLabel As String
            If IsDBNull(command.Parameters("@label").Value) Then
                sLabel = ""
            Else
                sLabel = command.Parameters("@label").Value
            End If

            command.Dispose()

            Return sLabel

        End Function

        Function ValidateParentChild(ByVal pParentID As Integer, ByVal pChildNodeTitle As String) As Integer

            Dim iSiteMapID As Integer = 0
            Dim command As New SqlCommand("usp_IsValidSiteMapRelationship")

            With command.Parameters
                .Add("@parent_id", SqlDbType.Int).Value = pParentID
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@child_label", SqlDbType.VarChar, 250).Value = pChildNodeTitle
                .Add("@sitemap_id", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With

            funcDB.ExecNonQuery(command, "CMS")
            iSiteMapID = CInt(command.Parameters("@sitemap_id").Value)
            command.Dispose()

            Return iSiteMapID

        End Function

        Function IsDescendent(ByVal ParentID As Integer, ByVal CheckChildID As Integer) As Boolean
            Dim arlbranch As ArrayList = GetSiteMap(ParentID, , 0, False)
            Dim checknode As MMSiteMapNode
            For Each checknode In arlbranch
                If checknode.SiteMapID = CheckChildID Then
                    Return True
                End If
            Next
            Return False
        End Function

        Sub MoveNodeUp(ByVal pSiteMapID As Integer)

            Dim command As New SqlCommand("usp_movesitemapup")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
            End With

            funcDB.ExecNonQuery(command, "CMS")
            command.Dispose()

        End Sub

        Sub MoveNodeDown(ByVal pSiteMapID As Integer)

            Dim command As New SqlCommand("usp_movesitemapdown")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSiteMapID
            End With

            funcDB.ExecNonQuery(command, "CMS")
            command.Dispose()

        End Sub

        Function CreateGUID() As String

            'This needs reworking as when theres alot of history a db call
            'will be needed for each piece of history just to store the guid

            Dim sGUID As String
            Dim command As New SqlCommand("usp_AddPreviewGUID")

            With command.Parameters
                .Add("@guid", SqlDbType.VarChar, 40).Value = Guid.NewGuid().ToString()

                funcDB.ExecNonQuery(command, "CMS")

                sGUID = .Item("@guid").Value.ToString
            End With

            command.Dispose()
            Return sGUID

        End Function

        Function IsValidGUID(ByVal pGUID As String) As Boolean

            Dim bValidGUID As Boolean = False

            'cant be valid if not supplied !
            If Not pGUID Is Nothing Then
                Dim command As New SqlCommand("usp_CheckPreviewGUID")

                With command.Parameters
                    .Add("@guid", SqlDbType.VarChar, 40).Value = pGUID
                    .Add("@valid", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                End With

                funcDB.ExecNonQuery(command, "CMS")
                bValidGUID = CBool(command.Parameters("@valid").Value)
                command.Dispose()
            End If

            Return bValidGUID

        End Function

        Function GetNews(ByVal pNumberToReturn As Integer, ByVal pNewsCategoryID As Integer) As ArrayList

            'Returns an arraylist of content objects, all type news
            '(frontend use only, as published, restrictions and live/expiry dates are checked)

            Dim arTmp As New ArrayList
            Dim command As New SqlCommand("usp_GetNews")

            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@number_to_return", SqlDbType.Int).Value = pNumberToReturn
                .Add("@news_category_id", SqlDbType.Int).Value = pNewsCategoryID
                '.Add("@user_id", SqlDbType.Int).Value = funcUtils.NullToZero(HttpContext.Current.User.Identity.Name) 'frontend userid
                '.Add("@user_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

            End With

            'Dim sdrTm1p As SqlDataReader = funcDB.ExecSql(command, "CMS")
            Dim sdrTmp As SqlDataReader = Nexus.Utils.ExecSql(command, "CMS")
            Dim dLiveDate, dExpiryDate As Date


            While sdrTmp.Read

                'Correct any null dates
                If IsDBNull(sdrTmp("live_date")) Then
                    dLiveDate = DateTime.MinValue
                Else
                    dLiveDate = CType(sdrTmp("live_date"), DateTime)
                End If

                If IsDBNull(sdrTmp("expiry_date")) Then
                    dExpiryDate = DateTime.MinValue
                Else
                    dExpiryDate = CType(sdrTmp("expiry_date"), DateTime)
                End If

                arTmp.Add(New SiteMap.SiteMapContent(sdrTmp("sitemap_id"), sdrTmp("parent_id"), sdrTmp("depth"), _
                                            sdrTmp("label"), dLiveDate, dExpiryDate, sdrTmp("hidden"), sdrTmp("anonymousOnly"), sdrTmp("authenticatedOnly"), _
                                            sdrTmp("restricted"), sdrTmp("xml_content"), sdrTmp("page_type_id"), _
                                            sdrTmp("xsd_schema")))

            End While

            sdrTmp.Close()
            command.Dispose()

            Return arTmp

        End Function

        Function SearchLiveContent(ByVal sSearch As String, ByVal LoggedIn As Boolean) As DataTable

            Dim command As New SqlCommand("usp_SearchLiveContent")
            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@Search", SqlDbType.VarChar).Value = sSearch
                .Add("@Restricted", SqlDbType.Bit).Value = LoggedIn
            End With

            SearchLiveContent = funcDB.GetDataTable(command, "CMS")
            command.Dispose()

        End Function

        Function GetPageTemplates() As XmlDataSource

            Dim sDataFile As String = "~/Configuration/" & AppSettings("ClientConfigurationFolder") & "/PageTemplates.xml"
            Dim xmlds As New XmlDataSource
            xmlds.EnableCaching = False
            xmlds.DataFile = Current.Server.MapPath(sDataFile)
            xmlds.XPath = "PageTemplates/portal[@id= " & Portal.Functions.GetPortalID() & "]/template"

            Return xmlds

        End Function

        Function GetCustomPropertyNames() As DataSet

            Dim ds As New DataSet
            Dim fn As String = HttpContext.Current.Server.MapPath(CustomPropertiesXMLFile)
            ds.ReadXml(fn, XmlReadMode.ReadSchema)
            Return ds

        End Function

        Sub AddCustomPropertyName(ByVal Name As String)

            Dim ds As New DataSet
            Dim fn As String = HttpContext.Current.Server.MapPath(CustomPropertiesXMLFile)
            ds.ReadXml(fn, XmlReadMode.ReadSchema)
            Dim dr() As DataRow = ds.Tables(0).Select("Name='" & Name & "'")
            If dr.Length = 0 Then
                Dim values() As String = {Name}
                ds.Tables(0).Rows.Add(values)
                ds.WriteXml(fn, XmlWriteMode.WriteSchema)
            End If

        End Sub

        Sub RemoveCustomPropertyName(ByVal Name As String)

            Dim ds As New DataSet
            Dim fn As String = HttpContext.Current.Server.MapPath(CustomPropertiesXMLFile)
            ds.ReadXml(fn, XmlReadMode.ReadSchema)
            Dim dr() As DataRow = ds.Tables(0).Select("Name='" & Name & "'")
            If dr.Length > 0 Then
                For Each r As DataRow In dr
                    ds.Tables(0).Rows.Remove(r)
                Next
                ds.WriteXml(fn, XmlWriteMode.WriteSchema)
            End If

        End Sub

        Sub BindPageTemplatesList(ByVal ddlTemplate As System.Web.UI.WebControls.DropDownList)
            If ddlTemplate.Items.Count = 0 Then
                With ddlTemplate
                    .DataSource = GetPageTemplates()
                    .DataTextField = "name"
                    .DataValueField = "filename"
                    .DataBind()
                    .SelectedIndex = -1

                    If .Items.Count <= 1 Then
                        ddlTemplate.Parent.Visible = False
                    End If
                End With
            End If
        End Sub

        Function GetRoot(ByVal pSitemap_id As Integer, _
                               Optional ByVal pTree_level As Integer = 2) As Integer
            Dim iRoot_id As Integer = 0

            Dim command As New SqlCommand("usp_GetRoot")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSitemap_id
                .Add("@tree_level", SqlDbType.Int).Value = pTree_level
                .Add("@root_id", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With

            funcDB.ExecNonQuery(command, "CMS")

            iRoot_id = command.Parameters("@root_id").Value
            command.Dispose()
            Return iRoot_id

        End Function

        Function GetRootLabel(ByVal pSitemap_id As Integer)

            Dim sRootLabel As String
            Dim command As New SqlCommand("usp_GetRootLabel")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSitemap_id
                .Add("@label", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")

            sRootLabel = command.Parameters("@label").Value
            command.Dispose()
            Return sRootLabel

        End Function

        Function GetParentLabel(ByVal pSitemap_id As Integer)

            Dim sParentLabel As String

            Dim command As New SqlCommand("usp_GetParentLabel")

            With command.Parameters
                .Add("@sitemap_id", SqlDbType.Int).Value = pSitemap_id
                .Add("@label", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")

            sParentLabel = command.Parameters("@label").Value
            If sParentLabel <> "" Then
                sParentLabel = "/" & sParentLabel
            End If

            command.Dispose()
            Return sParentLabel

        End Function

        Function GetSiteMapIcon(ByVal pSubmitted As Boolean, _
                                ByVal pApproved As Boolean, _
                                ByVal pPublished As Boolean) As String

            Dim sImageType As String = String.Empty

            If pSubmitted = True Then
                If pApproved = True Then
                    If pPublished = True Then
                        'Approve | Hide
                        sImageType = "unapproved_published.gif"
                    Else
                        'Approve | Publish
                        sImageType = "unapproved_unpublished.gif"
                    End If
                Else
                    If pPublished = True Then
                        'Approve | Hide
                        sImageType = "unapproved_published.gif"
                    Else
                        'Approve
                        sImageType = "unapproved_unpublished.gif"
                    End If
                End If
            Else
                If pApproved = True Then
                    If pPublished = True Then
                        'Hide
                        sImageType = "approved_published.gif"
                    Else
                        'Publish
                        sImageType = "approved_unpublished.gif"
                    End If
                Else
                    'Invalid
                End If
            End If

            Return sImageType

        End Function

        Function GetContentPath(ByVal SiteMapID As Int32) As String

            Dim arNavigation As ArrayList = SiteMap.Functions.GetSiteMap(, , 0, )
            Dim ReturnString As String = String.Empty
            Dim iCurrentSiteMapID As Integer = SiteMapID

            For i As Integer = (arNavigation.Count - 1) To 0 Step -1

                If CType(arNavigation(i), MMSiteMapNode).SiteMapID = iCurrentSiteMapID Then
                    ReturnString = CType(arNavigation(i), MMSiteMapNode).Label & "/" & ReturnString
                    iCurrentSiteMapID = CType(arNavigation(i), MMSiteMapNode).ParentID
                End If

            Next

            Return ReturnString.TrimEnd("/")

        End Function

    End Module

End Namespace