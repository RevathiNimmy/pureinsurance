
Public Class SPResponse
    Private iNewId As Integer
    Private sErrorCode As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal pNewID As Integer, Optional ByVal pErrorCode As String = Nothing)
        iNewId = pNewID
        sErrorCode = pErrorCode
    End Sub

    Public ReadOnly Property NewID() As Integer
        Get
            Return iNewId
        End Get
    End Property

    Public ReadOnly Property ErrorCode() As String
        Get
            Return sErrorCode
        End Get
    End Property
End Class
