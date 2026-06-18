<Serializable()> Public Class ClaimReceiptItemType
    Private lRecoveryKey As Long
    Private sTaxGroupCode, sRecoveryTypeCodeField As String
    Private dReceiptAmount As Decimal
    Private dTaxAmount As Decimal
    Private iBaseClaimReceiptItemKey, iBaseRecoveryKey, iBaseReserveKey, iClaimReceiptItemKeyField
    Private dLossAmount, dBaseAmount As Decimal
    Private sPartyReceiptCode, sPartyReceiptName, sCurrencyDescription As String
    Private dReceiptDate As Date
    Private oPayee As Payee

    Sub New()
        oPayee = New Payee
    End Sub

    Public Property Payee() As Payee
        Get
            Return Me.oPayee
        End Get
        Set(ByVal value As Payee)
            Me.oPayee = value
        End Set
    End Property

    Public Property ReceiptDate() As Date
        Get
            Return Me.dReceiptDate
        End Get
        Set(ByVal value As Date)
            Me.dReceiptDate = value
        End Set
    End Property
    Public Property CurrencyDescription() As String
        Get
            Return Me.sCurrencyDescription
        End Get
        Set(ByVal value As String)
            Me.sCurrencyDescription = value
        End Set
    End Property
    Public Property PartyReceiptName() As String
        Get
            Return Me.sPartyReceiptName
        End Get
        Set(ByVal value As String)
            Me.sPartyReceiptName = value
        End Set
    End Property
    Public Property PartyReceiptCode() As String
        Get
            Return Me.sPartyReceiptCode
        End Get
        Set(ByVal value As String)
            Me.sPartyReceiptCode = value
        End Set
    End Property
    Public Property BaseAmount() As Decimal
        Get
            Return Me.dBaseAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dBaseAmount = value
        End Set
    End Property
    Public Property LossAmount() As Decimal
        Get
            Return Me.dLossAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dLossAmount = value
        End Set
    End Property
    Public Property BaseReserveKey() As Integer
        Get
            Return Me.iBaseReserveKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseReserveKey = value
        End Set
    End Property
    Public Property BaseRecoveryKey() As Integer
        Get
            Return Me.iBaseRecoveryKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseRecoveryKey = value
        End Set
    End Property
    Public Property BaseClaimReceiptItemKey() As Integer
        Get
            Return Me.iBaseClaimReceiptItemKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimReceiptItemKey = value
        End Set
    End Property
    Public Property RecoveryKey() As Long
        Get
            Return Me.lRecoveryKey
        End Get
        Set(ByVal value As Long)
            Me.lRecoveryKey = value
        End Set
    End Property

    Public Property TaxGroupCode() As String
        Get
            Return Me.sTaxGroupCode
        End Get
        Set(ByVal value As String)
            Me.sTaxGroupCode = value
        End Set
    End Property

    Public Property ReceiptAmount() As Decimal
        Get
            Return Me.dReceiptAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptAmount = value
        End Set
    End Property

    Public Property TaxAmount() As Decimal
        Get
            Return Me.dTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTaxAmount = value
        End Set
    End Property
    Public Property ClaimReceiptItemKey() As Integer
        Get
            Return Me.iClaimReceiptItemKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iClaimReceiptItemKeyField = value
        End Set
    End Property

    Public Property RecoveryTypeCode() As String
        Get
            Return Me.sRecoveryTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sRecoveryTypeCodeField = value
        End Set
    End Property
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("RecoveryKey : " & lRecoveryKey.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("TaxGroupCode : " & sTaxGroupCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("ReceiptAmount : " & dReceiptAmount.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()
    End Function

End Class
''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ClaimReceiptItemTypeCollection : Inherits SortableCollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As ClaimReceiptItemType In List
        '    sbPrint.AppendLine(oClaimReceiptItemType.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ClaimReceiptItemType object to the collection
    ''' </summary>
    ''' <param name="v_oClaimReceiptItemType">The ClaimReceiptItemType object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oClaimReceiptItemType As ClaimReceiptItemType) As Integer
        Return List.Add(v_oClaimReceiptItemType)
    End Function

    ''' <summary>
    ''' Remove an ClaimReceiptItemType object from the collection
    ''' </summary>
    ''' <param name="v_oClaimReceiptItemType">The ClaimReceiptItemType object to be removed</param>
    Public Sub Remove(ByVal v_oClaimReceiptItemType As ClaimReceiptItemType)
        List.Remove(v_oClaimReceiptItemType)
    End Sub

    ''' <summary>
    ''' Remove an ClaimReceiptItemType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Payee object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ClaimReceiptItemType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ClaimReceiptItemType object</param>
    ''' <value>The replacement ClaimReceiptItemType object</value>
    ''' <returns>The ClaimReceiptItemType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ClaimReceiptItemType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimReceiptItemType)
            List(i) = value
        End Set
    End Property

End Class