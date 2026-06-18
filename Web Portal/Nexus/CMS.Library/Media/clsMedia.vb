Namespace Media

    Public Class Media
        Private iMedia_id As Integer
        Private iCategory_id As Integer
        Private sAlt As String
        Private sType As String
        Private sPath As String
        Private sFileName As String
        Private iWidth As Integer
        Private iHeight As Integer
        Private sMimeType As String
        Private sKeywords As String
        Private lSize As Long
        Private dtLastUpdate As Date
        Private sAdminName As String

        Public Sub New(ByVal media_id As Integer, ByVal category_id As Integer, ByVal alt As String, _
                        ByVal type As String, ByVal path As String, ByVal pFileName As String, ByVal width As Integer, ByVal height As Integer, _
                        ByVal mimetype As String, ByVal pKeywords As String, _
                        ByVal pSize As Long, ByVal pLastUpdate As Date, ByVal pAdminName As String)

            iMedia_id = media_id
            iCategory_id = category_id
            sAlt = alt
            sType = type
            sPath = path
            iWidth = width
            iHeight = height
            sMimeType = mimetype
            sKeywords = pKeywords
            lSize = pSize
            dtLastUpdate = pLastUpdate
            sAdminName = pAdminName
            sFileName = pFileName
        End Sub

        Public ReadOnly Property media_id() As Integer
            Get
                Return iMedia_id
            End Get
        End Property

        Public ReadOnly Property category_id() As Integer
            Get
                Return iCategory_id
            End Get
        End Property

        Public ReadOnly Property Alt() As String
            Get
                Return sAlt
            End Get
        End Property

        Public ReadOnly Property Type() As String
            Get
                Return sType
            End Get
        End Property

        'Public ReadOnly Property Pathold() As String
        '    Get
        '        Return sPath
        '    End Get
        'End Property

        Public ReadOnly Property FullPath() As String
            Get
                Return sPath & "/" & sFileName
            End Get
        End Property

        Public ReadOnly Property FileName() As String
            Get
                Return sFileName
            End Get
        End Property

        Public ReadOnly Property Width() As Integer
            Get
                Return iWidth
            End Get
        End Property

        Public ReadOnly Property Height() As Integer
            Get
                Return iHeight
            End Get
        End Property

        Public ReadOnly Property MimeType() As String
            Get
                Return sMimeType
            End Get
        End Property

        Public ReadOnly Property Keywords() As String
            Get
                Return sKeywords
            End Get
        End Property

        Public ReadOnly Property Size() As Long
            Get
                Return lSize
            End Get
        End Property

        Public ReadOnly Property SizeInKb() As Long
            Get
                Return lSize / 1024
            End Get
        End Property

        Public ReadOnly Property LastUpdate() As Date
            Get
                Return dtLastUpdate
            End Get
        End Property

        Public ReadOnly Property AdminName() As String
            Get
                Return sAdminName
            End Get
        End Property

    End Class

End Namespace