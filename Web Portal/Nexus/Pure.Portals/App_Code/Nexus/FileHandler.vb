'Imports System
'Imports System.Collections.Generic
'Imports System.Linq
Imports System.Web
Imports System.Web.Configuration
Imports System.IO
Imports Nexus.Constants
Imports Nexus.Library
Imports Nexus.Utils
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext
Public Class FileHandler

    
    ''' <summary>
    ''' Main file handler routine. Saves uploaded file to the given upload directory
    ''' </summary>
    ''' <param name="oFile"></param>
    ''' <remarks></remarks>
    Public Shared Sub HandleFile(ByVal oFile As Stream, ByVal fileName As String, ByVal sFileType As String, Optional ByVal bIsExternal As Boolean = False)
        'get the upload directory as specified in configuration
        Try
            Dim uploadDir As String = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString

            If Not String.IsNullOrEmpty(uploadDir) Then
                'create the upload directory - this will be a new directory as we used a guid in the path
                Directory.CreateDirectory(uploadDir)

                'create the full file name by adding the upload directory to the file name
                Dim fullFileName As String = String.Format("{0}\{1}", uploadDir, ReplaceString(fileName))
                'if there is already a file of this name in the upload directory then add _1 to the name
                If File.Exists(fullFileName) Then
                    Dim ext As String = fullFileName.Substring(fullFileName.LastIndexOf("."))
                    Dim fName As String = fullFileName.Substring(0, fullFileName.LastIndexOf("."))
                    fullFileName = String.Format("{0}_1{1}", fName, ext)
                End If

                Dim parser = New MultipartParser(oFile)
                Using ms = New FileStream(fullFileName, FileMode.CreateNew)
                    ms.Write(parser.FileContents, 0, parser.FileContents.Length)
                End Using


                'get the current document collection out of session
                Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection
                oDocumentCollection = HttpContext.Current.Session(CNCurrentDocumentCollection)
                If oDocumentCollection Is Nothing Then
                    'this shouldn't happen, but is useful for testing and doesn't do any harm
                    oDocumentCollection = New NexusProvider.DocumentDefaultsCollection
                End If

                'set up a document object to add to the collection
                Dim oDocument As New NexusProvider.DocumentDefaults
                oDocument.FileLocation = fullFileName
                oDocument.IsUpload = True
                oDocument.DocumentName = ReplaceString(fileName)
                oDocument.FileType = sFileType
                oDocument.Key = oDocumentCollection.Count
                oDocument.IsExternal = bIsExternal
                'add document to the collection and save this back to session
                oDocumentCollection.Add(oDocument)
                HttpContext.Current.Session(CNCurrentDocumentCollection) = oDocumentCollection
            End If
        Catch ex As Exception
            Current.Session(CNFileUploadError) = "FilehandlerException " + ex.Message
        End Try
    End Sub

    Private Shared Function ReplaceString(ByRef str As String) As String
        Dim illegalChars As Char() = ":~""#%&*:<>?/\{}|.".ToCharArray()
        Dim ext As String
        Dim fName As String
        If str.LastIndexOf(".") = -1 Then
            fName = str
            ext = ""
        Else
            ext = str.Substring(str.LastIndexOf("."))
            fName = str.Substring(0, str.LastIndexOf("."))
        End If


        Dim sb As New System.Text.StringBuilder

        For Each ch As Char In fName
            If Array.IndexOf(illegalChars, ch) = -1 Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString() & IIf(ext.Length > 1, ext, "")
    End Function

End Class
