Imports System.IO
Module Module1

    Sub Main()

        Dim sGUID As String
        Dim sfileLoc As String = AppDomain.CurrentDomain.BaseDirectory + "\WCFSecurityTokenFile.txt"
        sGUID = System.Guid.NewGuid.ToString()
        Dim fs As FileStream = Nothing
        If (Not File.Exists(sfileLoc)) Then
            fs = File.Create(sfileLoc)
            fs.Close()
        End If
        Using sw As StreamWriter = New StreamWriter(sfileLoc)
            sw.Write(sGUID)
        End Using

    End Sub

End Module
