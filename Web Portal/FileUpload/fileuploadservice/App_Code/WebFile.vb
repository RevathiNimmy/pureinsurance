''' <summary>
''' Class describes a file which may be uploaded to the upload service
''' </summary>
''' <remarks></remarks>
Public Class WebFile
    Dim _FileName As String
    Dim _FileContent As Byte()

    ''' <summary>
    ''' The name of the file
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FileName As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    ''' <summary>
    ''' The contents of the file, as a byte array
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FileContent As Byte()
        Get
            Return _FileContent
        End Get
        Set(ByVal value As Byte())
            _FileContent = value
        End Set
    End Property
End Class