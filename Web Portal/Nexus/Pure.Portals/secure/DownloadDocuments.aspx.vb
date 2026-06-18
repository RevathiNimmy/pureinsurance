Imports System.IO
Imports Ionic.Zip

Namespace Nexus

    Partial Class secure_DownloadDocuments
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim sDocNums As String = Request.QueryString("docNums")
            If String.IsNullOrEmpty(sDocNums) Then
                Return
            End If

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim selectedDocNums As String() = sDocNums.Split(","c)

            ' Get document names passed from the grid
            Dim docNamesList As String() = Nothing
            If Not String.IsNullOrEmpty(Request.QueryString("docNames")) Then
                docNamesList = Server.UrlDecode(Request.QueryString("docNames")).Split("|"c)
            End If

            ' Build name dictionary from query string names
            Dim dictDocNames As New System.Collections.Generic.Dictionary(Of Integer, String)
            If docNamesList IsNot Nothing AndAlso docNamesList.Length = selectedDocNums.Length Then
                For i As Integer = 0 To selectedDocNums.Length - 1
                    Dim parsedDocNum As Integer
                    If Integer.TryParse(selectedDocNums(i), parsedDocNum) Then
                        dictDocNames(parsedDocNum) = docNamesList(i).Trim()
                    End If
                Next
            Else
                ' If names not passed, use doc number as fallback name
                For i As Integer = 0 To selectedDocNums.Length - 1
                    Dim parsedDocNum As Integer
                    If Integer.TryParse(selectedDocNums(i), parsedDocNum) Then
                        If Not dictDocNames.ContainsKey(parsedDocNum) Then
                            dictDocNames(parsedDocNum) = "Document_" & parsedDocNum.ToString()
                        End If
                    End If
                Next
            End If

            ' Single document - download directly
            If selectedDocNums.Length = 1 Then
                Dim iDocNum As Integer
                If Not Integer.TryParse(selectedDocNums(0), iDocNum) Then
                    Return
                End If
                Dim odoc As New NexusProvider.Document
                odoc.DocNum = iDocNum
                oWebService.GetDocument(odoc)

                Dim sExtension As String = odoc.FileExtension
                If Not String.IsNullOrEmpty(sExtension) AndAlso Not sExtension.StartsWith(".") Then
                    sExtension = "." & sExtension
                End If

                Dim sDocName As String = "document"
                If dictDocNames.ContainsKey(iDocNum) Then
                    sDocName = dictDocNames(iDocNum)
                End If
                ' Remove invalid filename characters
                For Each c As Char In Path.GetInvalidFileNameChars()
                    sDocName = sDocName.Replace(c, "_"c)
                Next

                Response.ClearHeaders()
                Response.ClearContent()
                Response.Buffer = True
                Response.ContentType = "application/octet-stream"
                If odoc.PdfDocument Is Nothing OrElse odoc.PdfDocument.Length = 0 Then
                    Return
                End If
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & sDocName & sExtension & """")
                Response.AddHeader("Content-Length", odoc.PdfDocument.Length.ToString())
                Response.BinaryWrite(odoc.PdfDocument)
                Response.Flush()
                Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                Return
            End If

            ' Multiple documents - zip and download
            Dim sTempFolder As String = Server.MapPath("~/") & Guid.NewGuid().ToString()
            Directory.CreateDirectory(sTempFolder)

            Try
                ' Track used filenames to handle duplicates
                Dim usedNames As New System.Collections.Generic.HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

                For Each sDocNum As String In selectedDocNums
                    Dim iDocNum As Integer
                    If Not Integer.TryParse(sDocNum, iDocNum) Then Continue For
                    Dim odoc As New NexusProvider.Document
                    odoc.DocNum = iDocNum
                    oWebService.GetDocument(odoc)

                    If odoc.PdfDocument IsNot Nothing Then
                        Dim sExtension As String = odoc.FileExtension
                        If Not String.IsNullOrEmpty(sExtension) AndAlso Not sExtension.StartsWith(".") Then
                            sExtension = "." & sExtension
                        End If

                        Dim sDocName As String = "Document_" & sDocNum
                        If dictDocNames.ContainsKey(iDocNum) Then
                            sDocName = dictDocNames(iDocNum)
                        End If
                        ' Remove invalid filename characters
                        For Each c As Char In Path.GetInvalidFileNameChars()
                            sDocName = sDocName.Replace(c, "_"c)
                        Next

                        Dim sFileName As String = sDocName & sExtension
                        ' Handle duplicate filenames
                        Dim iCounter As Integer = 1
                        While usedNames.Contains(sFileName)
                            sFileName = sDocName & "_" & iCounter & sExtension
                            iCounter += 1
                        End While
                        usedNames.Add(sFileName)

                        Dim sFilePath As String = Path.Combine(sTempFolder, sFileName)
                        File.WriteAllBytes(sFilePath, odoc.PdfDocument)
                    End If
                Next

                ' Build ZIP filename: Documents_<FolderName>_<YYYYMMDD>.zip
                Dim sFolderName As String = Request.QueryString("folderName")
                If String.IsNullOrEmpty(sFolderName) Then
                    sFolderName = "Documents"
                End If
                ' Remove invalid filename characters from folder name
                For Each c As Char In Path.GetInvalidFileNameChars()
                    sFolderName = sFolderName.Replace(c, "_"c)
                Next
                Dim sZipFileName As String = "Documents_" & sFolderName & "_" & DateTime.Now.ToString("yyyyMMdd") & ".zip"

                Response.Clear()
                Response.BufferOutput = False
                Response.ContentType = "application/zip"
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & sZipFileName & """")

                Using zip As New ZipFile()
                    zip.AddDirectory(sTempFolder, "")
                    zip.Save(Response.OutputStream)
                End Using

                Response.End()
            Catch ex As Exception
            Finally
                If Directory.Exists(sTempFolder) Then Directory.Delete(sTempFolder, True)
            End Try
        End Sub

    End Class

End Namespace
