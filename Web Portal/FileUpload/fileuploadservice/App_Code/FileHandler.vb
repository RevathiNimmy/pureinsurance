
Imports System.Web.Configuration
Imports System.IO

Public Class FileHandler

    ''' <summary>
    ''' Main file handler routine. Saves uploaded file to the given upload directory
    ''' </summary>
    ''' <param name="oFile"></param>
    ''' <remarks></remarks>
    Public Shared Sub HandleFile(ByVal oFile As WebFile)
        'get the upload directory as specified in configuration
        Dim uploadDir As String = WebConfigurationManager.AppSettings("UploadDir")
        If Not String.IsNullOrEmpty(uploadDir) Then
            If (uploadDir.IndexOf("~/") = 0) Then
                'we have specified a directory inside the web root, so map the path to the folder
                uploadDir = HttpContext.Current.Server.MapPath(uploadDir)
            End If

            If (uploadDir.LastIndexOf("/") _
                        = (uploadDir.Length - 1)) Then
                'take off the trailing / if there is one
                uploadDir = uploadDir.Substring(0, (uploadDir.Length - 1))
            End If
            'create the full file name by adding the upload directory to the file name
            Dim fullFileName As String = String.Format("{0}/{1}", uploadDir, oFile.FileName)
            'if there is already a file of this name in the upload directory then add _1 to the name
            If File.Exists(fullFileName) Then
                Dim ext As String = fullFileName.Substring(fullFileName.LastIndexOf("."))
                Dim fName As String = fullFileName.Substring(0, fullFileName.LastIndexOf("."))
                fullFileName = String.Format("{0}_1{1}", fName, ext)
            End If
            'write the file to the disk
            File.WriteAllBytes(fullFileName, oFile.FileContent)
        End If
    End Sub
End Class