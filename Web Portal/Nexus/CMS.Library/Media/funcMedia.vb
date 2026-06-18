Imports Nexus.Utils
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Drawing
Imports System.Text
Imports System.Web.Security

Imports AssistedSolutions.SlickUpload.Configuration
Imports System.Web.Configuration.WebConfigurationManager

Namespace Media

    Public Module Functions

        Sub AddEditMedia(ByVal skeywords As String, ByVal sAlt As String, ByVal sType As String, ByVal sPath As String, ByVal sMimeType As String, ByVal iWidth As Integer, ByVal iHeight As Integer, ByVal iCategory_id As Integer, Optional ByVal iMedia_id As Integer = 0, Optional ByVal Size As Long = 0)

            'Add/Update media data to the databaase
            'New records have media_id of zero

            Dim command As New SqlCommand("usp_AddEditMedia")
            With command.Parameters
                .Add("@media_id", SqlDbType.Int).Value = iMedia_id
                .Add("@category_id", SqlDbType.Int).Value = iCategory_id
                .Add("@alt", SqlDbType.VarChar, 256).Value = sAlt
                .Add("@type", SqlDbType.Char, 3).Value = sType
                .Add("@path", SqlDbType.VarChar, 256).Value = sPath 'NB! path contains just the filename on db
                .Add("@width", SqlDbType.Int).Value = iWidth
                .Add("@height", SqlDbType.Int).Value = iHeight
                .Add("@mime", SqlDbType.VarChar, 50).Value = sMimeType
                .Add("@keywords", SqlDbType.VarChar, 256).Value = skeywords
                .Add("@size", SqlDbType.BigInt).Value = Size
                .Add("@admin_guid", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey
            End With
            funcDB.ExecNonQuery(command, "CMS")
            command.Dispose()

        End Sub

        Function GetMedia(ByVal pMediaID As Integer) As Media

            Dim tmpMedia As Media = Nothing
            Dim command As New SqlCommand("usp_GetMedia")
            With command.Parameters
                .Add("@media_id", SqlDbType.Int).Value = pMediaID
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            While sdrTmp.Read
                'tmpMedia = New Media(pMediaID, sdrTmp("category_id"), sdrTmp("alt"), sdrTmp("type"), _
                '        funcCategory.GetCategoryPath(sdrTmp("category_id")) & "/" & sdrTmp("path"), sdrTmp("width"), _
                '        sdrTmp("height"), sdrTmp("mimetype"), sdrTmp("keywords"), sdrTmp("Size"), sdrTmp("LastUpdate"))

                'NB calls to funcCategory.ReadItem - should move this function somewhere more generic (Nexus.Utils.Nexus?), see comment in ReadItem function

                Dim iCategory_id As Integer = Categories.ReadItem(sdrTmp, "category_id")
                tmpMedia = New Media( _
                     pMediaID, iCategory_id, _
                     Categories.ReadItem(sdrTmp, "alt"), _
                     Categories.ReadItem(sdrTmp, "type"), _
                     Categories.GetCategoryPath(iCategory_id), _
                     Categories.ReadItem(sdrTmp, "path"), _
                     Categories.ReadItem(sdrTmp, "width"), _
                     Categories.ReadItem(sdrTmp, "height"), _
                     Categories.ReadItem(sdrTmp, "mimetype"), _
                     Categories.ReadItem(sdrTmp, "keywords"), _
                     Categories.ReadItem(sdrTmp, "size"), _
                     Categories.ReadItem(sdrTmp, "LastUpdate"), _
                     ReadItem(sdrTmp, "UserName"))
            End While

            sdrTmp.Close()
            command.Dispose()

            Return tmpMedia

        End Function

        Function GetDepthAndPosition(ByVal category_id As Integer) As Pair

            Dim command As New SqlCommand("usp_GetCategoryDepth")

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

        Sub NewMedia(ByVal vSource As String, ByVal vDestinationPath As String, _
                    ByVal vFileExtension As String, ByRef rNewName As String)

            'Move media from temporary upload folder to actual location and rename with new filename,
            'check for existing files with the same name and adjust to create a unique filename

            If File.Exists(vSource) Then

                Dim sNewName As String = rNewName
                Dim x As Int32 = 1

                'Check destination file doesnt exist, if it does add an
                'increment to the filename until a unique filename is created
                While File.Exists(vDestinationPath & "\" & sNewName & "." & vFileExtension)
                    sNewName = "Copy_" & x & "_" & sNewName
                    x += 1
                End While

                rNewName = sNewName

                File.Move(vSource, vDestinationPath & "\" & rNewName & "." & vFileExtension)

            End If

        End Sub

        Sub UpdateMedia(ByVal vSource As String, ByVal vDestinationPath As String, _
                        ByVal vOriginalFileNameWithExt As String, ByRef rNewName As String, _
                        ByVal vFileExtension As String)

            Dim sNewName As String = rNewName
            Dim x As Int32 = 1

            If StrComp(vOriginalFileNameWithExt, sNewName & "." & vFileExtension) <> 0 Then
                'Check destination file doesnt exist, if it does add an
                'increment to the filename until a unique filename is created
                While File.Exists(vDestinationPath & "\" & sNewName & "." & vFileExtension)
                    sNewName = "Copy_" & x & "_" & sNewName
                    x += 1
                End While
            End If


            rNewName = sNewName

            If File.Exists(vSource) Then

                'Delete original destination file
                If File.Exists(vDestinationPath & "\" & vOriginalFileNameWithExt) Then
                    File.Delete(vDestinationPath & "\" & vOriginalFileNameWithExt)
                Else
                    'Oops, this file should exist for an update ...
                End If

                File.Move(vSource, vDestinationPath & "\" & rNewName & "." & vFileExtension)

            Else
                If File.Exists(vDestinationPath & "\" & vOriginalFileNameWithExt) Then
                    File.Move(vDestinationPath & "\" & vOriginalFileNameWithExt, _
                            vDestinationPath & "\" & rNewName & "." & vFileExtension)
                End If
            End If

        End Sub

        Function DeleteMedia(ByVal pMediaID As Int32, ByVal pCatID As Int32, ByVal pFileName As String) As Boolean

            'NEED TO ARCHIVE DELETED MEDIA AT SOME POINT

            Dim bSuccess As Boolean = True

            Try
                Dim command As New SqlCommand("usp_DeleteMedia")
                command.Parameters.Add("@media_id", SqlDbType.Int).Value = pMediaID
                funcDB.ExecNonQuery(command, "CMS")
                command.Dispose()

                Dim MediaPath As String = GetTempUploadDir() & Categories.GetCategoryPath(pCatID)
                File.Delete(MediaPath & "\" & pFileName)

            Catch ex As Exception

                bSuccess = False

            End Try

            Return bSuccess

        End Function

        Function CreateMediaPreview(ByVal pMedia As String, ByVal pMaxWidth As Int32, ByVal pMaxHeight As Int32) As String

            Dim MediaPath As String = System.Web.Configuration.WebConfigurationManager.AppSettings("MediaRoot")

            Dim sbSrc As New StringBuilder(funcUtils.WebRoot)
            Dim x As System.Web.Configuration.PagesSection = System.Web.Configuration.WebConfigurationManager.GetSection("system.web/pages")

            Select Case MediaGroupFromExt(Path.GetExtension(pMedia))
                Case MediaGroup.Image

                    sbSrc.Append("media/displayimage.aspx?img=")
                    sbSrc.Append(funcUtils.WebRoot)
                    sbSrc.Append(MediaPath)
                    sbSrc.Append("/")
                    sbSrc.Append(pMedia.Replace(" ", "%20"))
                    sbSrc.Append("&w=")
                    sbSrc.Append(pMaxWidth)
                    sbSrc.Append("&h=")
                    sbSrc.Append(pMaxHeight)
                    sbSrc.Append("&proportional=true")

                Case MediaGroup.MediaVideo, MediaGroup.Flash 'Video
                    sbSrc.Append("App_Themes/" & x.Theme & "/images/icon_video.gif")

                Case MediaGroup.MediaAudio 'Audio
                    sbSrc.Append("App_Themes/" & x.Theme & "/images/icon_music.gif")

                Case Else
                    sbSrc.Append("App_Themes/" & x.Theme & "/images/icon_document.gif")

            End Select

            Return sbSrc.ToString

        End Function

        Function CreateMediaPreviewForFrontEnd(ByVal pMedia As String, ByVal pMaxWidth As Int32, ByVal pMaxHeight As Int32) As String

            Dim MediaPath As String = funcUtils.WebRoot

            If System.Web.Configuration.WebConfigurationManager.AppSettings("DepthBelowParent") = "1" Then
                MediaPath &= "../"
            End If

            MediaPath &= System.Web.Configuration.WebConfigurationManager.AppSettings("MediaRoot") & "/"

            Dim sbSrc As New StringBuilder(funcUtils.WebRoot)

            Select Case MediaGroupFromExt(Path.GetExtension(pMedia))
                Case MediaGroup.Image                'Image
                    'Try
                    'Dim currentImage As Image = Image.FromFile(Current.Server.MapPath(MediaPath & pMedia))
                    'Dim imgHeight, imgWidth As Integer

                    'funcImage.ScaleFactorCalculations(imgHeight, imgWidth, currentImage, _
                    '    CType(pMaxHeight, Short), CType(pMaxWidth, Short))

                    'sbSrc.Append("displayimage.aspx?img=")
                    'sbSrc.Append(MediaPath)
                    'sbSrc.Append("/")
                    'sbSrc.Append(pMedia.Replace(" ", "%20"))
                    'sbSrc.Append("&w=")
                    'sbSrc.Append(imgWidth)
                    'sbSrc.Append("&h=")
                    'sbSrc.Append(imgHeight)

                    'currentImage.Dispose()

                    sbSrc.Append(MediaPath & pMedia)

                    'Catch exp As OutOfMemoryException
                    'Catch ex As Exception
                    'End Try

                Case MediaGroup.DocumentTagImage
                    sbSrc.Append("images/icon_document.gif")

                Case MediaGroup.MediaVideo 'Video
                    sbSrc.Append("images/icon_video.gif")

                Case MediaGroup.MediaAudio 'Audio
                    sbSrc.Append("images/icon_music.gif")

                Case MediaGroup.Flash 'Flash
                    sbSrc.Append("images/icon_video.gif")

                Case MediaGroup.Document
                    sbSrc.Append("images/icon_document.gif")

                Case MediaGroup.DocumentSpreadsheet
                    sbSrc.Append("images/icon_document.gif")

                Case MediaGroup.DocumentPresentation
                    sbSrc.Append("images/icon_document.gif")

                Case MediaGroup.DocumentAdobe
                    sbSrc.Append("images/icon_document.gif")

                Case MediaGroup.DocumentText
                    sbSrc.Append("images/icon_document.gif")

                Case Else
                    sbSrc.Append("images/icon_document.gif")
            End Select

            Return sbSrc.ToString

        End Function


        Function GetTempUploadDir() As String

            'Dim nvc As UploadLocationProviderConfiguration _
            '            = CType(Configuration.WebConfigurationManager.GetSection("slickUpload/uploadLocationProvider"), UploadLocationProviderConfiguration)

            'Return Current.Server.MapPath(nvc.CustomSettings.Item("location"))

        End Function

        Public Function MediaGroupFromExt(ByVal Ext As String) As MediaGroup
            If Ext.StartsWith(".") Then
                Ext = Ext.Trim(".")
            End If

            Static strImageExt, strTagImageExt, strVideoExt, strFlashExt, strAudioExt As String
            Static strTemplateExt, strDocumentExt, strArchiveExt As String
            Static strSpreadsheetExt, strPresentationExt As String
            Static strAdobeExt, strTextExt As String


            If strImageExt = "" Then
                strImageExt = UCase(AppSettings("ImageExtensions"))
                strTagImageExt = UCase(AppSettings("DocumentTagImageExtensions"))
                strVideoExt = UCase(AppSettings("VideoExtensions"))
                strFlashExt = UCase(AppSettings("FlashExtensions"))
                strAudioExt = UCase(AppSettings("AudioExtensions"))
                strTemplateExt = UCase(AppSettings("TemplateExtensions"))
                strDocumentExt = UCase(AppSettings("DocumentExtensions"))
                strArchiveExt = UCase(AppSettings("ArchiveExtensions"))
                strSpreadsheetExt = UCase(AppSettings("SpreadsheetExtensions"))
                strPresentationExt = UCase(AppSettings("PresentationExtensions"))
                strAdobeExt = UCase(AppSettings("AdobeExtensions"))
                strTextExt = UCase(AppSettings("TextExtensions"))
            End If

            Ext = UCase(Ext)

            If strImageExt.IndexOf(Ext) >= 0 Then Return MediaGroup.Image
            If strTagImageExt.IndexOf(Ext) >= 0 Then Return MediaGroup.DocumentTagImage
            If strVideoExt.IndexOf(Ext) >= 0 Then Return MediaGroup.MediaVideo
            If strFlashExt.IndexOf(Ext) >= 0 Then Return MediaGroup.Flash
            If strAudioExt.IndexOf(Ext) >= 0 Then Return MediaGroup.MediaAudio
            If strTemplateExt.IndexOf(Ext) >= 0 Then Return MediaGroup.Template
            If strDocumentExt.IndexOf(Ext) >= 0 Then Return MediaGroup.Document
            If strArchiveExt.IndexOf(Ext) >= 0 Then Return MediaGroup.Archive
            If strSpreadsheetExt.IndexOf(Ext) >= 0 Then Return MediaGroup.DocumentSpreadsheet
            If strPresentationExt.IndexOf(Ext) >= 0 Then Return MediaGroup.DocumentPresentation
            If strAdobeExt.IndexOf(Ext) >= 0 Then Return MediaGroup.DocumentAdobe
            If strTextExt.IndexOf(Ext) >= 0 Then Return MediaGroup.DocumentText

            Return -1

        End Function

        Public Enum MediaGroup
            All = 0
            Image
            DocumentTagImage
            MediaVideo
            MediaAudio
            Media 'both Video and Audio
            Flash
            Document
            Template
            Archive
            DocumentSpreadsheet
            DocumentPresentation
            DocumentAdobe
            DocumentText

        End Enum

        Function SearchMedia(ByVal sSearch As String) As DataTable

            Dim command As New SqlCommand("usp_SearchMedia")
            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@Search", SqlDbType.VarChar).Value = sSearch
            End With

            SearchMedia = funcDB.GetDataTable(command, "CMS")
            command.Dispose()

        End Function

        Function SearchMediaArrayList(ByVal sSearch As String) As ArrayList
            Dim arTmp As New ArrayList
            Dim command As New SqlCommand("usp_SearchMedia")
            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = Portal.Functions.GetPortalID()
                .Add("@Search", SqlDbType.VarChar).Value = sSearch
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            While sdrTmp.Read
                arTmp.Add(New Media( _
                    ReadItem(sdrTmp, "media_id"), _
                    ReadItem(sdrTmp, "Category_id"), _
                    ReadItem(sdrTmp, "alt"), _
                    ReadItem(sdrTmp, "type"), _
                    Categories.GetCategoryPath(ReadItem(sdrTmp, "Category_id")), _
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

    End Module

End Namespace
