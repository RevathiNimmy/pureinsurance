<Serializable()> Public Class PaymentMethod

    Dim sKey As String
    Public Property Key() As String
        Get
            Return Me.sKey
        End Get
        Set(ByVal value As String)
            Me.sKey = value
        End Set
    End Property

    Dim sPaymentName As String
    Public Property PaymentName() As String
        Get
            Return Me.sPaymentName
        End Get
        Set(ByVal value As String)
            Me.sPaymentName = value
        End Set
    End Property

    Dim sPaymentType As String
    Public Property PaymentType() As String
        Get
            Return Me.sPaymentType
        End Get
        Set(ByVal value As String)
            Me.sPaymentType = value
        End Set
    End Property

    Dim sRedirectURL As String
    Public Property RedirectURL() As String
        Get
            Return Me.sRedirectURL
        End Get
        Set(ByVal value As String)
            Me.sRedirectURL = value
        End Set
    End Property

    Dim sDisplayName As String
    Public Property DisplayName() As String
        Get
            Return Me.sDisplayName
        End Get
        Set(ByVal value As String)
            Me.sDisplayName = value
        End Set
    End Property

    Dim sMTADisplayName As String
    Public Property MTADisplayName() As String
        Get
            Return Me.sMTADisplayName
        End Get
        Set(ByVal value As String)
            Me.sMTADisplayName = value
        End Set
    End Property

    Dim iNumberOfInstalments As Integer
    Public Property NumberOfInstalments() As Integer
        Get
            Return Me.iNumberOfInstalments
        End Get
        Set(ByVal value As Integer)
            Me.iNumberOfInstalments = value
        End Set
    End Property

    Dim dblFeePercent As Double
    Public Property FeePercent() As Double
        Get
            Return Me.dblFeePercent
        End Get
        Set(ByVal value As Double)
            Me.dblFeePercent = value
        End Set
    End Property

    Dim bUseForQuoteCollection As Boolean
    Public Property UseForQuoteCollection() As Boolean
        Get
            Return Me.bUseForQuoteCollection
        End Get
        Set(ByVal value As Boolean)
            Me.bUseForQuoteCollection = value
        End Set
    End Property
    
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        'sbPrint.AppendLine("Short Code : " & sBankKey & "<br />")
        'sbPrint.AppendLine("Bank Name : " & sBankName & "<br />")
        'sbPrint.AppendLine("Short Code : " & sCode & "<br />")
        'sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
        'sbPrint.AppendLine("Head Office : " & sHeadOffice & "<br />")
        'sbPrint.AppendLine("Bank Address : " & sBankAddress & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString

    End Function

End Class

<Serializable()> Public Class PaymentMethodCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(PaymentMethod)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPaymentMethod As PaymentMethod In List
            sbPrint.AppendLine(oPaymentMethod.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    
    Public Function Add(ByVal v_oPaymentMethod As PaymentMethod) As String
        v_oPaymentMethod.Key = List.Add(v_oPaymentMethod)
        Return v_oPaymentMethod.Key

        'v_oPaymentMethod.PaymentName = List.Add(v_oPaymentMethod)
        'Return v_oPaymentMethod.PaymentName
    End Function

    
    Public Sub Remove(ByVal v_oPaymentMethod As PaymentMethod)
        List.Remove(v_oPaymentMethod)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PaymentMethod
        Get
            Return List(i)
        End Get
        Set(ByVal value As PaymentMethod)
            List(i) = value
        End Set
    End Property

End Class
