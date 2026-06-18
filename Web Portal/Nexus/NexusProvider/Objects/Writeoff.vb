<Serializable()> Public Class Writeoff
#Region "Private Fileds"
    Private iDocumentKey As Integer

    Private iAccountKey As Integer

    Private dWriteOffAmount As Decimal
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
        sbPrint.AppendLine("Document Key : " & iDocumentKey & "<br />")
        sbPrint.AppendLine("Account Key : " & iAccountKey & "<br />")
        sbPrint.AppendLine("Write Off Amount : " & dWriteOffAmount & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    Public Property DocumentKey() As Integer
        Get
            Return Me.iDocumentKey
        End Get
        Set(ByVal value As Integer)
            Me.iDocumentKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property WriteOffAmount() As Decimal
        Get
            Return Me.dWriteOffAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dWriteOffAmount = value
        End Set
    End Property
#End Region

End Class
