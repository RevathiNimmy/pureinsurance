<Serializable()> Public Class ClaimReceiptWarning

    Private iCode As Integer
    Private sDescription As String

    '''<remarks/>
    Public Property Code() As Integer
        Get
            Return Me.iCode
        End Get
        Set(ByVal value As Integer)
            Me.iCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
End Class
<Serializable()> Public Class ClaimReceiptWarningCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Document object to the collection
    ''' </summary>
    ''' <param name="v_oClaimReceipt">The Document object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oClaimReceipt As ClaimReceiptWarning) As Integer
        Return List.Add(v_oClaimReceipt)
    End Function

    ''' <summary>
    ''' Remove an ClaimReceiptType object from the collection
    ''' </summary>
    ''' <param name="v_oClaimReceipt">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oClaimReceipt As ClaimReceiptWarning)
        List.Remove(v_oClaimReceipt)
    End Sub

    ''' <summary>
    ''' Remove an ClaimReceiptType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Document object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ClaimReceiptType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement ClaimReceiptType object</value>
    ''' <returns>The ClaimReceiptType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ClaimReceiptWarning
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimReceiptWarning)
            List(i) = value
        End Set
    End Property

End Class