<Serializable()> Public Class Reserve
#Region "Private Fields"
    Private sTypeCodeField As String

    Private dRevisionAmountField As Decimal
#End Region

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("TypeCodeField : " & sTypeCodeField & "<br />")
        sbPrint.AppendLine("RevisionAmountField: " & dRevisionAmountField & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    ''' 
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sTypeCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property RevisionAmount() As Decimal
        Get
            Return Me.dRevisionAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisionAmountField = value
        End Set
    End Property
#End Region

End Class
