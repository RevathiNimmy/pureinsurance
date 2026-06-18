<Serializable()> Public Class StoredProcedureParameterType

    Private ParamNameField As String

    Private ParamValueField As String

    Public Sub New()

    End Sub
    Public Property ParamName() As String
        Get
            Return Me.ParamNameField
        End Get
        Set
            Me.ParamNameField = Value
        End Set
    End Property

    Public Property ParamValue() As String
        Get
            Return Me.ParamValueField
        End Get
        Set
            Me.ParamValueField = Value
        End Set
    End Property

End Class
