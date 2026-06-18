<Serializable()> Public Class ClaimReceiptType
    Private lKey As Long
    Private sPerilKey As String
    Private iPartyKey As Integer
    Private sCurrencyCode As String
    Private sClaimVersionDescription As String
    Private oReceiptItems As ClaimReceiptItemTypeCollection
    Private oRecoveryItems As RecoveryReceiptTypeCollection
    Private oTaxItems As TaxItemTypeCollection
    Private dReceiptToLossExchangeRate As Decimal
    Private oPayee As Payee

    '''<remarks/>
    ''' 
    Public Property BaseClaimKey() As Long
        Get
            Return Me.lKey
        End Get
        Set(ByVal value As Long)
            Me.lKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseClaimPerilKey() As String
        Get
            Return Me.sPerilKey
        End Get
        Set(ByVal value As String)
            Me.sPerilKey = value
        End Set
    End Property



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
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    Public Property ReceiptToLossExchangeRate() As Decimal
        Get
            Return Me.dReceiptToLossExchangeRate
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptToLossExchangeRate = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimVersionDescription() As String
        Get
            Return Me.sClaimVersionDescription
        End Get
        Set(ByVal value As String)
            Me.sClaimVersionDescription = value
        End Set
    End Property



    '''<remarks/>
    Public Property ReceiptItem() As ClaimReceiptItemTypeCollection
        Get
            Return Me.oReceiptItems
        End Get
        Set(ByVal value As ClaimReceiptItemTypeCollection)
            Me.oReceiptItems = value
        End Set
    End Property

    Public Property RecoveryItems() As RecoveryReceiptTypeCollection
        Get
            Return Me.oRecoveryItems
        End Get
        Set(ByVal value As RecoveryReceiptTypeCollection)
            Me.oRecoveryItems = value
        End Set
    End Property

    Public Property TaxItems() As TaxItemTypeCollection
        Get
            Return Me.oTaxItems
        End Get
        Set(ByVal value As TaxItemTypeCollection)
            Me.oTaxItems = value
        End Set
    End Property

    '''<remarks/>
    Public Property Payee() As Payee
        Get
            Return Me.oPayee
        End Get
        Set(ByVal value As Payee)
            Me.oPayee = value
        End Set
    End Property

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Claim Key : " & lKey.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Peril Key : " & sPerilKey & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Party Key : " & iPartyKey.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("ClaimVersion Description : " & sClaimVersionDescription & "<br />")
        sbPrint.AppendLine("<br />")
        ' sbPrint.AppendLine("ClaimVersion Description : " & oReceiptPartyType.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("Advanced Tax Details : " & oAdvancedTaxDetails & "<br />")
        sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("ReceiptItem : " & oReceiptItem & "<br />")
        sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("Payee : " & oPayee & "<br />")
        sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()

    End Function
End Class

<Serializable()> Public Class ClaimReceiptTypeCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As ClaimReceiptType In List
        '    sbPrint.AppendLine(oClaimReceiptType.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ClaimReceiptType object to the collection
    ''' </summary>
    ''' <param name="v_oClaimReceiptType">The ClaimReceiptType object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oClaimReceiptType As ClaimReceiptType) As Integer
        Return List.Add(v_oClaimReceiptType)
    End Function

    ''' <summary>
    ''' Remove an ClaimReceiptType object from the collection
    ''' </summary>
    ''' <param name="v_oClaimReceiptType">The ClaimReceiptType object to be removed</param>
    Public Sub Remove(ByVal v_oClaimReceiptType As ClaimReceiptType)
        List.Remove(v_oClaimReceiptType)
    End Sub

    ''' <summary>
    ''' Remove an ClaimReceiptType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Payee object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ClaimReceiptType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ClaimReceiptType object</param>
    ''' <value>The replacement ClaimReceiptType object</value>
    ''' <returns>The ClaimReceiptType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ClaimReceiptType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimReceiptType)
            List(i) = value
        End Set
    End Property

End Class
