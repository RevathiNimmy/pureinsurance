<Serializable()> Public Class ForgotPassword
#Region "PrivateFields"
    Private iMode As Integer
    Private iClaimKey As Integer
    Private sTransactionType As String
    Private sParameterXML As String
    Private bOutputAsHTML As Boolean
    Private bOutputAsPDF As Boolean
                                              
#End Region

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bOutputAsHTML = False
        bOutputAsPDF = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Mode : " & iMode & "<br />")
        sbPrint.AppendLine("Claim Key : " & iClaimKey & "<br />")
        sbPrint.AppendLine("Transaction Type : " & sTransactionType & "<br />")
        sbPrint.AppendLine("Parameter XML : " & sParameterXML & "<br />")
        sbPrint.AppendLine("Output As HTML: " & IIf(bOutputAsHTML, "true", "false") & "<br />")
        sbPrint.AppendLine("Output As PDF: " & IIf(bOutputAsPDF, "true", "false") & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    Public Property Mode() As Integer
        Get
            Return iMode
        End Get
        Set(ByVal value As Integer)
            iMode = value
        End Set
    End Property
    Public Property ClaimKey() As Integer
        Get
            Return iClaimKey
        End Get
        Set(ByVal value As Integer)
            iClaimKey = value
        End Set
    End Property
    Public Property TransactionType() As String
        Get
            Return sTransactionType
        End Get
        Set(ByVal value As String)
            sTransactionType = value
        End Set
    End Property
    Public Property ParameterXML() As String
        Get
            Return sParameterXML
        End Get
        Set(ByVal value As String)
            sParameterXML = value
        End Set
    End Property
    Public Property OutputAsHTML() As Boolean
        Get
            Return bOutputAsHTML
        End Get
        Set(ByVal value As Boolean)
            bOutputAsHTML = value
        End Set
    End Property
    Public Property OutputAsPDF() As Boolean
        Get
            Return bOutputAsPDF
        End Get
        Set(ByVal value As Boolean)
            bOutputAsPDF = value
        End Set
    End Property
#End Region

End Class
