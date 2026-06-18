Imports Nexus.Utils
Imports Nexus.Utils.Formatting

Imports System.Web.HttpContext
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.io

Imports System.Security
Imports System.Security.Permissions

Namespace Media

    Public Module Categories

        Function GetBannerChildCategories(ByVal pParentID As Integer) As ArrayList
            Dim command As New SqlCommand("usp_GetMediaCategoryChildren")
            With command.Parameters
                .Add("@ParentID", SqlDbType.Int).Value = pParentID
            End With
            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
            Dim arTmp As New ArrayList
            While sdrTmp.Read
                arTmp.Add(New MediaCategory(sdrTmp("category_id"), sdrTmp("category_name"), sdrTmp("parent_id"), _
                                                Portal.Functions.GetPortalID, sdrTmp("depth"), sdrTmp("NoItems")))
            End While
            sdrTmp.Close()
            command.Dispose()
            Return arTmp
        End Function

        Function GetCategories(Optional ByVal iTopID As Integer = 0, _
                                        Optional ByVal iDepthBelow As Integer = 0, _
                                        Optional ByVal bShowRoot As Boolean = True) As ArrayList
            'returns the media categories for the portal

            Dim arTmp As New ArrayList
            Dim command As New SqlCommand("usp_GetMediaCategories")

            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
                .Add("@top_id", SqlDbType.Int).Value = iTopID
                .Add("@depth_below_parent", SqlDbType.Int).Value = iDepthBelow
                .Add("@show_root", SqlDbType.Bit).Value = bShowRoot
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            While sdrTmp.Read
                arTmp.Add(New MediaCategory(sdrTmp("category_id"), sdrTmp("category_name"), sdrTmp("parent_id"), _
                                                Portal.Functions.GetPortalID, sdrTmp("depth"), sdrTmp("NoItems")))
            End While

            sdrTmp.Close()
            command.Dispose()

            Return arTmp


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
                Dim MediaDepth As Pair = GetDepthAndPosition(iParent_id)

                iDepth = CInt(MediaDepth.First) + 1

                If CInt(MediaDepth.Second) = 0 Or IsDBNull(MediaDepth.Second) Then
                    iPosition = 1
                Else
                    iPosition = CInt(MediaDepth.Second) + 1
                End If
            End If

            'now create the category in the database
            Dim command As New SqlCommand("usp_CreateCategory")
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

            Dim f As New FileIOPermission(PermissionState.Unrestricted, Functions.GetTempUploadDir)
            f.AllLocalFiles = FileIOPermissionAccess.AllAccess

            If iCategoryID > 0 Then
                'create the physical folder
                Directory.CreateDirectory(Functions.GetTempUploadDir & "\" & GetCategoryPath(command.Parameters("@NewID").Value))
            End If

            Return iCategoryID

        End Function

        Sub RemoveMediaCategory(ByVal iCategoryId As Integer)
            'remove category from the db and archive the physical folder

            'we need to move the folder to the archive folder
            Dim strOriginalDir As String = Functions.GetTempUploadDir & GetCategoryPath(iCategoryId)
            Dim strDestinationDir As String = Functions.GetTempUploadDir & Portal.Functions.GetPortal(Portal.Functions.GetPortalID).Name & "\ARCHIVE\" & GetCategoryPath(iCategoryId)

            Directory.Move(strOriginalDir, strDestinationDir)

            Dim command As New SqlCommand("usp_DeleteCategory")

            With command.Parameters
                .Add("@category_id", SqlDbType.Int).Value = iCategoryId
            End With

            funcDB.ExecNonQuery(command, "CMS")
            command.Dispose()

        End Sub

        Function GetCategoryPath(ByVal iCategory_id As Integer) As String

            Dim command As New SqlCommand("usp_GetCategoryPath")

            With command.Parameters
                .Add("@category_id", SqlDbType.Int).Value = iCategory_id
                .Add("@path", SqlDbType.VarChar, 250).Value = ""
                command.Parameters("@path").Direction = ParameterDirection.Output
            End With

            funcDB.ExecNonQuery(command, "CMS")

            Dim sPath As String = Trim(command.Parameters("@path").Value.ToString)

            command.Dispose()

            Return sPath

        End Function

        Function GetCategoryMedia(ByVal iCategory_id As Integer) As ArrayList

            'returns all media from the database within selected category
            Dim arTmp As New ArrayList
            Dim command As New SqlCommand("usp_GetCategoryMedia")
            With command.Parameters
                .Add("@category_id", SqlDbType.Int).Value = iCategory_id
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            While sdrTmp.Read
                arTmp.Add(New Media( _
                    ReadItem(sdrTmp, "media_id"), iCategory_id, _
                    ReadItem(sdrTmp, "alt"), _
                    ReadItem(sdrTmp, "type"), _
                    ReadItem(sdrTmp, "full_path"), _
                    ReadItem(sdrTmp, "path"), _
                    ReadItem(sdrTmp, "width"), _
                    ReadItem(sdrTmp, "height"), _
                    ReadItem(sdrTmp, "mimetype"), _
                    ReadItem(sdrTmp, "keywords"), _
                    ReadItem(sdrTmp, "size"), _
                    ReadItem(sdrTmp, "LastUpdate"), _
                    ReadItem(sdrTmp, "UserName")))
            End While

            sdrTmp.Close()
            command.Dispose()

            Return arTmp

        End Function

        Public Function ReadItem(ByVal dr As SqlDataReader, ByVal strField As String) As Object
            'on the defensive - put something similar (a more generic version) in utils somewhere?
            Try
                Dim o As Object = dr(strField)
                If o.GetType.Name.ToLower = "dbnull" Then
                    Return Nothing
                Else
                    Return o
                End If
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

    End Module

End Namespace