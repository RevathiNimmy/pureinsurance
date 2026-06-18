
<Serializable()> Public Class ClaimReceipt


    Private bTimeStamp As Byte()
    Private dReceiptToLossExchangeRate As Decimal
    Private iClaimKey As Integer
    Private iBaseClaimKey, iBaseClaimPerilKey, iPartyKey As Integer
    Private sClaimNumber, sCurrencyCode, sClaimVersionDescription As String
    Private iVersion As Integer
    Private sResultingStatus As String
    Private oReceiptPartyType As ClaimReceiptPartyTypeType
    Private oClaimReceiptWarning As ClaimReceiptWarningCollection
    Private oClaimReceiptItem As ClaimReceiptItemTypeCollection
    Private oAdvancedTaxDetails As ClaimReceiptAdvancedTaxDetails
    Private oReceiptItems As RecoveryReceiptTypeCollection
    Private oPayee As Payee
    Private bIsSalvageRecovery As Boolean
    Private oTaxItem As TaxItemTypeCollection
    Private iBaseClaimReceiptKey As Integer
    Private dReceiptAmount, dTaxAmount As Decimal
    Private dReceiptDate As Date
    Private sReceiptPartyType As String
    Private sPartyReceiptCode, sPartyReceiptName, sCurrencyDescription As String
    Private iClaimReceiptKeyField As Integer
    Private iClaimPerilKey As Integer
    Private closeClaimOnZeroReserveRecoveryBalanceField As Boolean
     Private nDoNotCreateClaimVersionOnSalvageReceipt As Integer


    Sub New()
        oClaimReceiptWarning = New ClaimReceiptWarningCollection
        oClaimReceiptItem = New ClaimReceiptItemTypeCollection
        oAdvancedTaxDetails = New ClaimReceiptAdvancedTaxDetails
        oReceiptItems = New RecoveryReceiptTypeCollection
        oPayee = New Payee
        oTaxItem = New TaxItemTypeCollection
    End Sub
    Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
        Get
            Return Me.closeClaimOnZeroReserveRecoveryBalanceField
        End Get
        Set(ByVal value As Boolean)
            Me.closeClaimOnZeroReserveRecoveryBalanceField = value
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
   
   
    Public Property ReceiptPartyType() As String
        Get
            Return Me.sReceiptPartyType
        End Get
        Set(ByVal value As String)
            Me.sReceiptPartyType = value
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
    Public Property TaxAmount() As Decimal
        Get
            Return Me.dTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTaxAmount = value
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
    Public Property BaseClaimReceiptKey() As Integer
        Get
            Return Me.iBaseClaimReceiptKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimReceiptKey = value
        End Set
    End Property
    Public Property TaxItem() As TaxItemTypeCollection
        Get
            Return Me.oTaxItem
        End Get
        Set(ByVal value As TaxItemTypeCollection)
            Me.oTaxItem = value
        End Set
    End Property
    Public Property IsSalvageRecovery() As Boolean
        Get
            Return Me.bIsSalvageRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSalvageRecovery = value
        End Set
    End Property
    Public Property ClaimVersionDescription() As String
        Get
            Return Me.sClaimVersionDescription
        End Get
        Set(ByVal value As String)
            Me.sClaimVersionDescription = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
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

    Public Property ClaimReceiptItem() As ClaimReceiptItemTypeCollection
        Get
            Return Me.oClaimReceiptItem
        End Get
        Set(ByVal value As ClaimReceiptItemTypeCollection)
            Me.oClaimReceiptItem = value
        End Set
    End Property
    Public Property ClaimReceiptWarning() As ClaimReceiptWarningCollection
        Get
            Return Me.oClaimReceiptWarning
        End Get
        Set(ByVal value As ClaimReceiptWarningCollection)
            Me.oClaimReceiptWarning = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property
    Public Property BaseClaimPerilKey() As Integer
        Get
            Return Me.iBaseClaimPerilKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimPerilKey = value
        End Set
    End Property
    Public Property BaseClaimKey() As Integer
        Get
            Return Me.iBaseClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property Version() As Integer
        Get
            Return Me.iVersion
        End Get
        Set(ByVal value As Integer)
            Me.iVersion = value
        End Set
    End Property

    '''<remarks/>
    Public Property ResultingStatus() As String
        Get
            Return Me.sResultingStatus
        End Get
        Set(ByVal value As String)
            Me.sResultingStatus = value
        End Set
    End Property
    '''<remarks/>
    'Public Property ReceiptPartyType() As ClaimReceiptPartyTypeType
    '    Get
    '        Return Me.oReceiptPartyType
    '    End Get
    '    Set(ByVal value As ClaimReceiptPartyTypeType)
    '        Me.oReceiptPartyType = value
    '    End Set
    'End Property
    '''<remarks/>
    Public Property AdvancedTaxDetails() As ClaimReceiptAdvancedTaxDetails
        Get
            Return Me.oAdvancedTaxDetails
        End Get
        Set(ByVal value As ClaimReceiptAdvancedTaxDetails)
            Me.oAdvancedTaxDetails = value
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
    '''<remarks/>
    Public Property ReceiptItem() As RecoveryReceiptTypeCollection
        Get
            Return Me.oReceiptItems
        End Get
        Set(ByVal value As RecoveryReceiptTypeCollection)
            Me.oReceiptItems = value
        End Set
    End Property
      '''<remarks/>
    ''' Used in ClaimReceipt provider layer to prevent version creation.
    Public Property DoNotCreateClaimVersionOnSalvageReceipt() As Integer
        Get
            Return Me.nDoNotCreateClaimVersionOnSalvageReceipt
        End Get
        Set(ByVal value As Integer)
            Me.nDoNotCreateClaimVersionOnSalvageReceipt = value
        End Set
    End Property
    Public Property ClaimReceiptKey() As Integer
        Get
            Return Me.iClaimReceiptKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iClaimReceiptKeyField = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimPerilKey() As Integer
        Get
            Return Me.iClaimPerilKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimPerilKey = value
        End Set
    End Property
    
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'sbPrint.AppendLine("Claim Key : " & bTimeStamp.ToString() & "<br />")
        'sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("ClaimReceipt Item : " & oClaimReceiptItem.Print & "<br />")
        'sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()

    End Function

End Class

<Serializable()> Public Class ClaimReceiptCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As Document In List
        '    sbPrint.AppendLine(oDocument.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Document object to the collection
    ''' </summary>
    ''' <param name="v_oClaimReceipt">The Document object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oClaimReceipt As ClaimReceipt) As Integer
        Return List.Add(v_oClaimReceipt)
    End Function

    ''' <summary>
    ''' Remove an ClaimReceiptType object from the collection
    ''' </summary>
    ''' <param name="v_oClaimReceipt">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oClaimReceipt As ClaimReceipt)
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
    Default Public Property Item(ByVal i As Integer) As ClaimReceipt
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimReceipt)
            List(i) = value
        End Set
    End Property

End Class
