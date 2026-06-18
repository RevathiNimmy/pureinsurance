Imports System.IO
Imports System.Web
Imports System.Web.Configuration

Namespace Media

    Public Class funcLibrary


        Public Shared Function GetLibraries() As String()
            Dim strMediaRoot As String = "~/" & WebConfigurationManager.AppSettings("MediaRoot")
            Dim strTrimedValues As String = String.Empty
            Dim intIndex As Integer = 0

            For Each strDirectory As String In Directory.GetDirectories(HttpContext.Current.Server.MapPath(strMediaRoot))
                Dim i As Integer = strDirectory.LastIndexOf("\")
                strTrimedValues = strTrimedValues & Right(strDirectory, ((strDirectory.Length - i) - 1)) & ";"
            Next
            strTrimedValues = Left(strTrimedValues, strTrimedValues.Length - 1)
            Return strTrimedValues.Split(";")
        End Function


    End Class

End Namespace