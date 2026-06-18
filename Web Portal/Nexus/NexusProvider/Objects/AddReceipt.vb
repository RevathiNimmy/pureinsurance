<Serializable()> Public Class AddReceipt
#Region "Private Fileds"
    Private iPartyKey As Integer

    Private oReceipt As ReceiptType
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        oReceipt = New ReceiptType

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("iPartyKey : " & iPartyKey & "<br />")
       
        sbPrint.AppendLine("ReceiptType ---------------><br />")

        If oReceipt IsNot Nothing Then
            sbPrint.AppendLine(oReceipt.Print())
        End If
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Receipt() As ReceiptType
        Get
            Return Me.oReceipt
        End Get
        Set(ByVal value As ReceiptType)
            Me.oReceipt = value
        End Set
    End Property
#End Region

End Class
<Serializable()> Public Class AddPayNowReceipt : Inherits AddReceipt

End Class
<Serializable()> Public Class AddPayNowReceiptCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each OAddPayNowReceipt As AddPayNowReceipt In List
            sbPrint.AppendLine(OAddPayNowReceipt.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_OAddPayNowReceipt As AddPayNowReceipt) As Integer
        Return List.Add(v_OAddPayNowReceipt)
    End Function

    Public Sub Remove(ByVal v_OAddPayNowReceipt As AddPayNowReceipt)
        List.Remove(v_OAddPayNowReceipt)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AddPayNowReceipt
        Get
            Return List(i)
        End Get
        Set(ByVal value As AddPayNowReceipt)
            List(i) = value
        End Set
    End Property

End Class