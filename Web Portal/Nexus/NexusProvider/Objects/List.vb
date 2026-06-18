Public Class List

    Private sListType As String

    Private sListCode As String

    Private bExcludeDeletedRecords As Boolean

    Private bExcludeEffectiveDate As Boolean

    Private sParentFieldName As String

    Private iParentFieldValue As Integer

    
    Public Property ListType() As String
        Get
            Return sListType
        End Get
        Set(ByVal value As String)
            sListType = value
        End Set
    End Property
    Public Property ListCode() As String
        Get
            Return sListCode
        End Get
        Set(ByVal value As String)
            sListCode = value
        End Set
    End Property
    Public Property ExcludeDeletedRecords() As Boolean
        Get
            Return bExcludeDeletedRecords
        End Get
        Set(ByVal value As Boolean)
            bExcludeDeletedRecords = value
        End Set
    End Property
    Public Property ExcludeEffectiveDate() As Boolean
        Get
            Return bExcludeEffectiveDate
        End Get
        Set(ByVal value As Boolean)
            bExcludeEffectiveDate = value
        End Set
    End Property
    Public Property ParentFieldName() As String
        Get
            Return sParentFieldName
        End Get
        Set(ByVal value As String)
            sParentFieldName = value
        End Set
    End Property
    Public Property ParentFieldValue() As String
        Get
            Return iParentFieldValue
        End Get
        Set(ByVal value As Integer)
            iParentFieldValue = value
        End Set
    End Property
End Class
