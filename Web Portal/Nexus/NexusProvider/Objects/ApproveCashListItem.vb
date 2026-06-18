<Serializable()> Public Class ApproveCashListItem

    Private iCashListItemKey As Integer

    Private sComments As String

    Private bTimeStamp() As Byte

    Private bDeclined As Boolean

    Public Sub New()

    End Sub

    '''<remarks/>
    Public Property CashListItemKey() As Integer
        Get
            Return Me.iCashListItemKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListItemKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Comments() As String
        Get
            Return Me.sComments
        End Get
        Set(ByVal value As String)
            Me.sComments = value
        End Set
    End Property

    '''<remarks/>

    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property

    '''<remarks/>
    Public Property Declined() As Boolean
        Get
            Return Me.bDeclined
        End Get
        Set(ByVal value As Boolean)
            Me.bDeclined = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("CashList Item Key : " & iCashListItemKey.ToString & "<br />")
        sbPrint.AppendLine("Comments : " & sComments & "<br />")
        sbPrint.AppendLine("TimeStamp : " & bTimeStamp.ToString & "<br />")
        sbPrint.AppendLine("Declined : " & bDeclined & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString

    End Function
End Class
