Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Web.HttpContext

Imports Nexus.Utils
Imports Nexus.Utils.Cache
Imports Nexus.Utils.Formatting


Imports System.Web
Imports System.Web.UI
Imports System.io

Imports System.Security
Imports System.Security.Permissions

Namespace Banner

    Public Module Functions

#Region "Helpers"

        ''' <summary>
        ''' Clears all banner chache
        ''' </summary>
        Public Sub ClearCache()

            'ToDO: Caching
            For Each cacheDictionaryItem As DictionaryEntry In Current.Cache

                If Not Current.Cache(cacheDictionaryItem.Key.ToString()) Is Nothing Then
                    If cacheDictionaryItem.Key.ToString.Contains("BannerList") Then
                        Current.Cache.Remove(cacheDictionaryItem.Key.ToString())
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Reads elements from a data reader, checks for null's
        ''' and exceptions 
        ''' </summary>
        ''' <param name="pDr">Reader to check, SqlDataReader, required</param>
        ''' <param name="pField">Name of field to read, String, required</param>
        ''' <returns>Value as an object</returns>
        Public Function ReadItem(ByVal pDr As SqlDataReader, ByVal pField As String) As Object
            'on the defensive - put something similar (a more generic version) in utils somewhere?
            Try
                Dim o As Object = pDr(pField)
                If o.GetType.Name.ToLower = "dbnull" Then
                    Return Nothing
                Else
                    Return o
                End If
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Used to retrieve a BannerExposureLimit object, from the numerical keys stored 
        ''' enum. e.g. 1 returns BannerExposureLimit.TimeLimit
        ''' </summary>
        ''' <param name="key">Underlying enumerator value, Integer, Required</param>
        ''' <returns>BannerExposureLimit.[Value]</returns>
        Public Function GetExposureLimit(ByVal key As Integer) As BannerExposureLimit
            Select Case key
                Case 0
                    Return BannerExposureLimit.NoLimit
                Case 1
                    Return BannerExposureLimit.TimeLimit
                Case 2
                    Return BannerExposureLimit.CountLimit
                Case 3
                    Return BannerExposureLimit.ExposuresPerDay
                Case 4
                    Return BannerExposureLimit.ExposuresPerWeek
                Case 5
                    Return BannerExposureLimit.ExposuresPerMonth
            End Select
        End Function

        ''' <summary>
        ''' Builds banner object from data reader
        ''' </summary>
        ''' <param name="pDataReader">Reader that contains valid banner information, SqlDataReader, required</param>
        ''' <returns>A populated CMS.Banner.Banner object</returns>
        Private Function GetBannerFromDataReader(ByVal pDataReader As SqlDataReader) As Banner
            Return _
                New Banner( _
                    ReadItem(pDataReader, "banner_id"), _
                    ReadItem(pDataReader, "banner_type_id"), _
                    ReadItem(pDataReader, "banner_cat_id"), _
                    ReadItem(pDataReader, "banner_name"), _
                    ReadItem(pDataReader, "banner_url"), _
                    ReadItem(pDataReader, "banner_source"), _
                    ReadItem(pDataReader, "banner_image_path"), _
                    ReadItem(pDataReader, "banner_priority"), _
                    ReadItem(pDataReader, "banner_code_snippet"), _
                    ReadItem(pDataReader, "banner_exposure_limit_target"), _
                    ReadItem(pDataReader, "banner_exposure_target_remaining"), _
                    GetExposureLimit(ReadItem(pDataReader, "banner_exposure_limit")), _
                    GetCategoryPath(ReadItem(pDataReader, "banner_cat_id")), _
                    ReadItem(pDataReader, "banner_exposure_total"), _
                    IIf(IsDBNull(pDataReader("banner_exposure_last")), Nothing, pDataReader("banner_exposure_last")), _
                    ReadItem(pDataReader, "banner_exposure_live_date"), _
                    ReadItem(pDataReader, "banner_exposure_expiry_date"), _
                    ReadItem(pDataReader, "banner_3rd_party_use"), _
                    ReadItem(pDataReader, "admin_guid"), _
                    ReadItem(pDataReader, "action"), _
                    ReadItem(pDataReader, "published"), _
                    ReadItem(pDataReader, "submitted"), _
                    ReadItem(pDataReader, "edit_date"))
        End Function


#End Region

#Region "Banners"

        ''' <summary>
        ''' Gets a range of banner.banner objects as an array
        ''' </summary>
        ''' <param name="pPublished">Only return published, Integer (-1 = no:1 = yes), Optional </param>
        ''' <param name="pBannerCategoryID">Only return banners with categoryID, Integer Optional</param>
        ''' <param name="pBannerTypeID">Only return banners with typeID, Integer, Optional</param>
        ''' <returns>Array of Banner.Banner</returns>
        Function GetBanners(Optional ByVal pPublished As Integer = -1, _
                            Optional ByVal pBannerCategoryID As Integer = -1, Optional ByVal pBannerTypeID As Integer = -1) As ArrayList

            'ToDo: Caching
            ClearCache()
            Dim arTmp As New ArrayList
            Dim strCacheKey = "BannerList_" & pPublished.ToString & "_" & pBannerCategoryID.ToString & "_" & pBannerTypeID.ToString
            'Retrieve from List of Portals from cache if available
            If Current.Cache.Item(strCacheKey) Is Nothing Then
                Dim command As New SqlCommand("usp_GetBanners")
                With command.Parameters
                    .Add("@banner_id", SqlDbType.Int).Value = 0
                    .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
                    If pPublished <> -1 Then .Add("@published", SqlDbType.Int).Value = pPublished
                    If pBannerTypeID <> -1 Then .Add("@banner_type_id", SqlDbType.Int).Value = pBannerTypeID
                    If pBannerCategoryID <> -1 Then .Add("@banner_cat_id", SqlDbType.Int).Value = pBannerCategoryID
                End With

                Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

                While sdrTmp.Read
                    arTmp.Add(GetBannerFromDataReader(sdrTmp))
                End While

                sdrTmp.Close()
                command.Dispose()

                Current.Cache.Insert(strCacheKey, arTmp, Nothing, CacheExpiration(CacheLengthTypes.CacheMedium), TimeSpan.Zero)
            Else
                arTmp = CType(Current.Cache.Item(strCacheKey), ArrayList)
            End If

            Return arTmp

        End Function

        ''' <summary>
        ''' Get a single banner.banner object
        ''' </summary>
        ''' <param name="pBannerID">Banner Identity, Integer, Required</param>
        ''' <returns>Banner.Banner</returns>
        Function GetBanner(ByVal pBannerID As Integer) As Banner

            Dim command As New SqlCommand("usp_GetBanners")
            With command.Parameters
                .Add("@banner_id", SqlDbType.Int).Value = pBannerID
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            Dim objBanner As Banner = Nothing
            If sdrTmp.Read Then
                objBanner = GetBannerFromDataReader(sdrTmp)
            End If

            sdrTmp.Close()
            command.Dispose()

            Return objBanner


        End Function

        ''' <summary>
        ''' Get a single banner.banner by name
        ''' </summary>
        ''' <param name="pBannerName">Name of banner</param>
        ''' <returns>Banner.Banner</returns>
        Function GetBannerFromName(ByVal pBannerName As String) As Banner

            'since all banners are cached just return from cache:
            Dim b As Banner
            For Each b In GetBanners()
                If b.Name = pBannerName Then Return b
            Next
            Return Nothing

        End Function

        Function ExposeBanner(ByVal pID As Integer) As Banner
            Dim command As New SqlCommand("dbo.usp_expose_banner")
            With command.Parameters
                .Add("@BannerID", SqlDbType.Int).Value = pID
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            Dim objBanner As Banner = Nothing
            If sdrTmp.Read Then
                objBanner = GetBannerFromDataReader(sdrTmp)
            End If

            sdrTmp.Close()
            command.Dispose()

            Return objBanner
        End Function

        ''' <summary>
        ''' Get a randomly selected banner.banner
        ''' </summary>
        ''' <returns>Banner.Banner</returns>
        Function GetRandomBanner() As Banner
            Dim banners As ArrayList = GetBanners()
            If banners.Count > 0 Then
                Dim b As Banner
                Dim r As New Random
                Dim countcheck As Integer = 0
                Do
                    b = CType(banners(r.Next(0, banners.Count)), Banner)
                    countcheck += 1
                Loop Until b.Published Or countcheck > 100 '? make sure no infinite loop occurs if no published banners are available
                If Not b.Published Then b = Nothing
                Return b
            Else
                Return Nothing
            End If
        End Function


        Function ExposeBannerContent(ByVal pBannerID As Integer) As String
            Return GetBannerContent(ExposeBanner(pBannerID))
        End Function
        ''' <summary>
        ''' Get Random Banner content
        ''' </summary>
        ''' <returns>Content as a string</returns>
        Function GetRandomBannerContent() As String
            Return GetBannerContent(GetRandomBanner())
        End Function

        ''' <summary>
        ''' Get specific banner content
        ''' </summary>
        ''' <param name="BannerID">Banner identity, Integer, required</param>
        ''' <returns>Content as string</returns>
        Function GetBannerContent(ByVal BannerID As Integer) As String
            Return GetBannerContent(GetBanner(BannerID))
        End Function

        ''' <summary>
        ''' Get banner content 
        ''' </summary>
        ''' <param name="b">Banner to generate content for, Banner.Banner, Required</param>
        ''' <returns>content as string</returns>
        Function GetBannerContent(ByVal b As Banner) As String
            If b IsNot Nothing Then
                If b.CodeSnippet <> "" Then
                    Return b.CodeSnippet
                Else
                    Dim sb As New System.Text.StringBuilder
                    With sb
                        .Append("<a id=""banner")
                        .Append(b.ID)
                        .Append(""" href=""")
                        .Append(b.Url)
                        .Append(""" ><img src=""")
                        .Append(b.ImagePath)
                        .Append(""" border=""0"" alt=""")
                        .Append(b.Name)
                        .Append(""" />")
                        .Append("</a>")
                    End With
                    Return sb.ToString
                End If
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Add/Edit Banner content
        ''' </summary>
        ''' <param name="pBannerID">Banner Identity, Integer, Required</param>
        ''' <param name="pName">Banner Name, String, Required</param>
        ''' <param name="pUrl">Banner Click Through URL, string, required</param>
        ''' <param name="pExposureLive">Begin exposures on, datetime, required</param>
        ''' <param name="pExposureExpire">End exposures on, datetime, required</param>
        ''' <param name="pTypeID">Banner type identifier, integer, required</param>
        ''' <param name="pCategoryID">Banner category identifier, integer, required</param>
        ''' <param name="pSource">Banner source, string (I = Internal:E = External), required</param>
        ''' <param name="pImagePath">Banner image path, string, required</param>
        ''' <param name="pPriority">Banner exposure priority, integer (1-5), required</param>
        ''' <param name="pExposureLimit">Banner maximun number of exposures, integer, required</param>
        ''' <param name="p3rdPartyUse">Banner allow 3rd party use, boolean, required</param>
        ''' <param name="pCodeSnippet">Banner 3rd party code snippet, string, reuired</param>
        ''' <returns>Banner identity as integer</returns>
        Function AddEditBanner(ByVal pBannerID As Integer, ByVal pName As String, ByVal pUrl As String, _
                            ByVal pExposureLive As DateTime, ByVal pExposureExpire As DateTime, ByVal pTypeID As Integer, ByVal pCategoryID As Integer, ByVal pSource As String, ByVal pImagePath As String, _
                            ByVal pPriority As Integer, ByVal pExposureTarget As Integer, ByVal pExposureLimit As BannerExposureLimit, ByVal p3rdPartyUse As Boolean, ByVal pCodeSnippet As String) As Integer

            Dim command As New SqlCommand("usp_AddEditBanner")

            With command.Parameters
                .Add("@banner_id", SqlDbType.Int).Value = pBannerID
                .Item("@banner_id").Direction = ParameterDirection.InputOutput
                .Add("@Portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@name", SqlDbType.VarChar, 50).Value = pName.TrimEnd
                .Add("@url", SqlDbType.VarChar, 250).Value = pUrl.TrimEnd
                .Add("@type_id", SqlDbType.Int).Value = pTypeID
                .Add("@cat_id", SqlDbType.Int).Value = pCategoryID
                .Add("@source", SqlDbType.NChar, 1).Value = pSource
                .Add("@image_path", SqlDbType.VarChar, 250).Value = pImagePath
                .Add("@priority", SqlDbType.Int).Value = pPriority
                .Add("@code_snippet", SqlDbType.NText).Value = pCodeSnippet
                .Add("@third_party_use", SqlDbType.Bit).Value = p3rdPartyUse
                .Add("@exposure_limit", SqlDbType.TinyInt).Value = CInt(pExposureLimit)
                .Add("@exposure_limit_target", SqlDbType.Int).Value = pExposureTarget
                If pExposureLimit = BannerExposureLimit.TimeLimit Then
                    .Add("@exposure_limit_live", SqlDbType.DateTime).Value = pExposureLive
                    .Add("@exposure_limit_expire", SqlDbType.DateTime).Value = pExposureExpire
                End If
                .Add("@admin_guid", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey
            End With

            funcDB.ExecNonQuery(command, "CMS")

            Dim iBannerID As Integer = command.Parameters("@banner_id").Value

            command.Dispose()

            'Dispose of cache
            ClearCache()

            Return iBannerID

        End Function

        ''' <summary>
        ''' Publish/unPublish banner
        ''' </summary>
        ''' <param name="pBannerID">Banner identity, integer, required</param>
        ''' <param name="pPublish">Publish settings, boolean, required</param>
        ''' <remarks></remarks>
        Sub Publish(ByVal pBannerID As Integer, ByVal pPublish As Boolean)

            Dim command As New SqlCommand("usp_PublishBanner")

            With command
                .Parameters.Add("@banner_id", SqlDbType.Int).Value = pBannerID
                .Parameters.Add("@publish", SqlDbType.Int).Value = pPublish
                .Parameters.Add("@admin_guid", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

                funcDB.ExecNonQuery(command, "CMS")

                .Dispose()
            End With

            Current.Cache.Remove("BannerList")

        End Sub

        ''' <summary>
        ''' Delete banner
        ''' </summary>
        ''' <param name="pBannerID">Banner identity, integer, required</param>
        ''' <remarks></remarks>
        Sub DeleteBanner(ByVal pBannerID As Integer)
            Dim command As New SqlCommand("usp_DeleteBanner")

            With command
                .Parameters.Add("@banner_id", SqlDbType.Int).Value = pBannerID
                .Parameters.Add("@admin_guid", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

                funcDB.ExecNonQuery(command, "CMS")

                .Dispose()
            End With

            Current.Cache.Remove("BannerList")
        End Sub

#End Region

#Region "Banner Types"
        Function GetBannerTypes() As ArrayList
            'ToDo: Caching
            If Current.Cache.Item("BannerTypes") Is Nothing Then
                Dim command As New SqlCommand("usp_GetBannerTypes")
                Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
                Dim arTmp As New ArrayList
                While sdrTmp.Read
                    arTmp.Add(New BannerType(sdrTmp("banner_type_id"), sdrTmp("name"), sdrTmp("width"), sdrTmp("height"), sdrTmp("details")))
                End While
                Current.Cache.Item("BannerTypes") = arTmp
                sdrTmp.Close()
                command.Dispose()
            End If
            Return Current.Cache.Item("BannerTypes")
        End Function

        Function GetBannerType(ByVal pTypeID As Integer) As BannerType

            'since all banners are cached just return from cache:
            Dim bt As BannerType
            For Each bt In GetBannerTypes()
                If bt.ID = pTypeID Then Return bt
            Next
            Return Nothing

        End Function
#End Region

#Region "Banner Categories"

        Function GetBannerCategories(ByVal pPortalID As Nullable(Of Integer)) As ArrayList
            If pPortalID.HasValue And Not pPortalID.Value = -1 Then
                Return GetBannerCategoriesByPortal(pPortalID.Value)
            Else
                Return GetBannerCategoriesAll()
            End If
        End Function

        Function GetBannerChildCategories(ByVal pCategoryID As Integer, ByVal pPortalID As Integer) As ArrayList
            Dim command As New SqlCommand("usp_GetBannerChildCategories")
            With command.Parameters
                .Add("@Category_ID", SqlDbType.Int).Value = pCategoryID
                .Add("@portal_id", SqlDbType.Int).Value = pPortalID
            End With
            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
            Dim arTmp As New ArrayList
            While sdrTmp.Read
                arTmp.Add(New BannerCategory(sdrTmp("banner_cat_id"), sdrTmp("name"), sdrTmp("portal_id"), sdrTmp("parent_id"), sdrTmp("Depth"), sdrTmp("Position"), sdrTmp("NoTotal"), sdrTmp("NoPublished")))
            End While
            sdrTmp.Close()
            command.Dispose()
            Return arTmp
        End Function

        Function GetBannerCategoriesAll() As ArrayList
            'ToDo: Caching
            If Current.Cache.Item("BannerCategories") Is Nothing Then
                Dim command As New SqlCommand("usp_GetBannerCategories")
                Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
                Dim arTmp As New ArrayList
                While sdrTmp.Read
                    arTmp.Add(New BannerCategory(sdrTmp("banner_cat_id"), sdrTmp("name"), sdrTmp("portal_id"), sdrTmp("parent_id"), sdrTmp("Depth"), sdrTmp("Position"), sdrTmp("NoTotal"), sdrTmp("NoPublished")))
                End While
                Current.Cache.Item("BannerCategories") = arTmp
                sdrTmp.Close()
                command.Dispose()
            End If
            Return Current.Cache.Item("BannerCategories")
        End Function

        Function GetBannerCategoriesByPortal(ByVal pPortalID As Integer, Optional ByVal iTopID As Integer = 0, _
                    Optional ByVal iDepthBelow As Integer = 0, Optional ByVal bShowRoot As Boolean = True) As ArrayList



            Dim command As New SqlCommand("usp_GetBannerCategoriesByPortal")
            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = pPortalID
                .Add("@top_id", SqlDbType.Int).Value = iTopID
                .Add("@depth_below_parent", SqlDbType.Int).Value = iDepthBelow
                .Add("@show_root", SqlDbType.Bit).Value = bShowRoot
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
            Dim arTmp As New ArrayList
            While sdrTmp.Read
                arTmp.Add(New BannerCategory(sdrTmp("banner_cat_id"), sdrTmp("name"), sdrTmp("portal_id"), sdrTmp("parent_id"), sdrTmp("Depth"), sdrTmp("Position"), sdrTmp("NoTotal"), sdrTmp("NoPublished")))
            End While
            sdrTmp.Close()
            command.Dispose()
            Return arTmp
        End Function

        Function GetBannerCategory(ByVal pCategoryID As Integer) As BannerCategory

            'since all banners are cached just return from cache:
            Dim bc As BannerCategory
            Dim pPortalID As Integer = Nothing
            For Each bc In GetBannerCategoriesAll()
                If bc.ID = pCategoryID Then Return bc
            Next
            Return Nothing

        End Function

        Sub RemoveBannerCategory(ByVal iCategoryId As Integer)

            Dim command As New SqlCommand("usp_DeleteBannerCategory")

            With command.Parameters
                .Add("@id", SqlDbType.Int).Value = iCategoryId
            End With

            funcDB.ExecNonQuery(command, "CMS")
            command.Dispose()

        End Sub


        Function GetCategoryPath(ByVal iCategory_id As Integer, Optional ByVal tmpPath As String = "") As String

            Dim command As New SqlCommand("usp_GetBannerCategoryPath")

            With command.Parameters
                .Add("@category_id", SqlDbType.Int).Value = iCategory_id
                .Add("@parent_id", SqlDbType.Int).Value = 0
                command.Parameters("@parent_id").Direction = ParameterDirection.Output
                .Add("@category_name", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")

            Dim iParent As Integer = 0

            If Not IsDBNull(command.Parameters("@parent_id")) Then
                iParent = CInt(command.Parameters("@parent_id").Value)
            End If



            Dim sPath As String
            If tmpPath = "" Then
                sPath = Trim(command.Parameters("@category_name").Value.ToString)
            Else
                sPath = Trim(command.Parameters("@category_name").Value.ToString) & "/" & tmpPath
            End If

            command.Dispose()

            If iParent <> 0 Then
                sPath = GetCategoryPath(iParent, sPath)
            End If

            Return sPath

        End Function


        Function GetCategoryBanners(ByVal iCategory_id As Integer) As ArrayList

            Return GetBanners(, iCategory_id, )

        End Function


        Function CreateCategory(ByVal iParent_id As Integer, Optional ByVal sCategoryName As String = "", _
                         Optional ByVal pPortalID As Integer = 0) As Integer

            'first strip out any non alphanumeric chars
            sCategoryName = StripAlphaNumeric(sCategoryName)

            Dim iDepth, iPosition As Integer

            If iParent_id <= 0 Then
                iDepth = 1
                iPosition = 1
            Else
                'first we need the depth
                Dim BannerDepth As Pair = GetDepthAndPosition(iParent_id)

                iDepth = CInt(BannerDepth.First) + 1

                If CInt(BannerDepth.Second) = 0 Or IsDBNull(BannerDepth.Second) Then
                    iPosition = 1
                Else
                    iPosition = CInt(BannerDepth.Second) + 1
                End If
            End If

            'now create the category in the database
            Dim command As New SqlCommand("usp_CreateBannerCategory")
            With command.Parameters
                .Add("@parent_id", SqlDbType.Int).Value = iParent_id
                .Add("@depth", SqlDbType.Int).Value = iDepth
                If pPortalID > 0 Then
                    'New portal has been created, need default folders
                    .Add("@portal_id", SqlDbType.Int).Value = pPortalID
                Else
                    .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
                End If
                .Add("@position", SqlDbType.Int).Value = iPosition
                .Add("@category_name", SqlDbType.VarChar, 250).Value = sCategoryName
                .Add("@error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .Add("@NewId", SqlDbType.Int).Direction = ParameterDirection.Output
            End With
            funcDB.ExecNonQuery(command, "CMS")

            Dim iCategoryID As Integer = CInt(command.Parameters("@NewId").Value)
            command.Dispose()

            Return iCategoryID

        End Function

        Function GetDepthAndPosition(ByVal category_id As Integer) As Pair

            Dim command As New SqlCommand("usp_GetBannerCategoryDepth")

            With command.Parameters
                .Add("@category_id", SqlDbType.Int).Value = category_id
                .Add("@depth", SqlDbType.Int).Direction = ParameterDirection.Output
                .Add("@position", SqlDbType.Int).Direction = ParameterDirection.Output
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            Dim iDepth As Integer = CInt(command.Parameters("@depth").Value)
            Dim iPosition As Integer = CInt(command.Parameters("@position").Value)
            sdrTmp.Close()
            command.Dispose()

            Dim DepthPos As New Pair
            DepthPos.First = iDepth
            DepthPos.Second = iPosition
            Return DepthPos

        End Function
#End Region

    End Module

End Namespace