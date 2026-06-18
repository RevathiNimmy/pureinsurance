
<Serializable()> Partial Public Class RiskFees

    Private iInsuranceFileKey As Integer
    Private sClientCode As String
    Private sInsuranceFileReference As String
    Private sCurrency As String

    Private oFeeCollection As FeeCollection


    Private Sub New()
        oFeeCollection = New FeeCollection
    End Sub

    ''' <summary>
    ''' Insurance FileKey
    ''' </summary>
    ''' <value>Insurance FileKey</value>
    ''' <returns>Insurance FileKey</returns>
    Public Property FeeCollection() As FeeCollection
        Get
            Return oFeeCollection
        End Get
        Set(ByVal value As FeeCollection)
            oFeeCollection = value
        End Set
    End Property
    ''' <summary>
    ''' Insurance FileKey
    ''' </summary>
    ''' <value>Insurance FileKey</value>
    ''' <returns>Insurance FileKey</returns>
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property

    ''' <summary>
    ''' Client Code
    ''' </summary>
    ''' <value>Client Code</value>
    ''' <returns>Client Code</returns>
    Public Property ClientCode() As String
        Get
            Return sClientCode
        End Get
        Set(ByVal value As String)
            sClientCode = value
        End Set
    End Property


    ''' <summary>
    ''' Insurance File Reference
    ''' </summary>
    ''' <value>Insurance File Reference</value>
    ''' <returns>Insurance File Reference</returns>
    Public Property InsuranceFileReference() As String
        Get
            Return sInsuranceFileReference
        End Get
        Set(ByVal value As String)
            sInsuranceFileReference = value
        End Set
    End Property


    ''' <summary>
    ''' Currency
    ''' </summary>
    ''' <value>Currency</value>
    ''' <returns>Currency</returns>
    Public Property Currency() As String
        Get
            Return sCurrency
        End Get
        Set(ByVal value As String)
            sCurrency = value
        End Set
    End Property


    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("InsuranceFileKey   : " & iInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("ClientCode   : " & sClientCode & "<br />")
        sbPrint.AppendLine("InsuranceFileReference   : " & sInsuranceFileReference & "<br />")
        sbPrint.AppendLine("Currency    : " & sCurrency & "<br />")
        sbPrint.AppendLine("<br />")

        sbPrint.AppendLine("FeeCollection ---------------><br />")

        If oFeeCollection IsNot Nothing Then
            sbPrint.AppendLine(oFeeCollection.Print())
        End If

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString

    End Function

End Class



