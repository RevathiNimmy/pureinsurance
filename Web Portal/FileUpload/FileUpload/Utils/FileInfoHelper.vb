Imports System.IO

Public Class FileInfoHelper

    Public Shared Function GetFileBytes(ByVal fInfo As FileInfo) As Byte()
        Dim fileContent() As Byte
        Dim fileStream As Stream = fInfo.OpenRead
        Dim bin As BinaryReader = New BinaryReader(fileStream)
        fileContent = bin.ReadBytes(CType(fInfo.Length, Integer))
        bin.Dispose()
        Return fileContent
    End Function
End Class