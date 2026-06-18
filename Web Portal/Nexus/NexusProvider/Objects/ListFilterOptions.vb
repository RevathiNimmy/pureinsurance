<Serializable()> Partial Public Class ListFilterOptions
    Private columnNameField As String
    Private filterOperatorField As String
    Private filterValueField As String

    Public Property ColumnName() As String
        Get
            Return Me.columnNameField
        End Get
        Set(ByVal value As String)
            Me.columnNameField = value
        End Set
    End Property

    Public Property FilterOperator() As String
        Get
            Return Me.filterOperatorField
        End Get
        Set(ByVal value As String)
            Me.filterOperatorField = value
        End Set
    End Property

    Public Property FilterValue() As String
        Get
            Return Me.filterValueField
        End Get
        Set(ByVal value As String)
            Me.filterValueField = value
        End Set
    End Property
End Class
