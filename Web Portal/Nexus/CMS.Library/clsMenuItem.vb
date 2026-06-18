
Public Class MenuItem

    Private sName As String
    Private sTemplate As String
    Private bEnabled As Boolean

    Public Sub New(ByVal pName As String, ByVal pTemplate As String, Optional ByVal pEnabled As Boolean = True)

        sName = pName
        sTemplate = pTemplate
        bEnabled = pEnabled

    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return sName
        End Get
    End Property

    Public ReadOnly Property Template() As String
        Get
            Return sTemplate
        End Get
    End Property

    Public ReadOnly Property Enabled() As Boolean
        Get
            Return bEnabled
        End Get
    End Property
End Class