<Serializable()> Public Class ClaimPayment

    Private bTimeStamp() As Byte

    Private bCloseClaimOnZeroReserveRecoveryBalance As Boolean
    Private iBaseClaimKey As Integer
    Private iBaseClaimPerilKey As Integer
    Private iPartyKey, iClientKey As Integer
    Private sCurrencyCode As String
    Private sClaimVersionDescription As String
    Private bisPaymentAuthorized As Boolean

    Private oPaymentPartyType As ClaimPaymentPartyTypeType
    Private oClaimPaymentItemType As ClaimPaymentItemTypeCollection
    Private oPayee As Payee
    Private oPaymentAdvancedTaxDetails As PaymentAdvancedTaxDetails

    Private oClaimPaymentTaxItemType As ClaimPaymentTaxItemCollection
    'List of Properties added on 30-1-2009 for the Claim Payment.Begin
    ' Private oClaimPerilReservePaymentType As ClaimPerilReservePaymentTypeCollection

    Private iBaseReserveKey As Integer
    Private sTypeCode As String
    Private dTotalReserve As Decimal
    Private dThisRevision As Decimal
    Private dPaidToDate As Decimal
    Private dPaidToDateTax As Decimal
    Private dCurrentReserve As Decimal
    Private dThisPaymentINCLTax As Decimal
    Private dThisPaymentTax As Decimal
    Private dCostToClaim As Decimal
    Private oClaimPerilReservePaymentType As ClaimPerilReservePaymentTypeCollection

    Private sRiskType As String
    Private sReserveType As String
    Private dBalance As Decimal
    Private dCurrency As Decimal
    Private iCurrencyRate As Integer
    Private dNetPayment As Decimal

    'end 
    Private oClaimPaymentItem As ClaimPaymentItemCollection

    Private oClaimPayment As ClaimPaymentCollection
    Private oCashListPayment As PaymentCashList

    Private oCashListItem As PaymentCashListItemType
    Private dPaymentAmount As Decimal
    Private bReverseExcess As Boolean
    Private sTaxGroupCode As String
    Private sOurRef As String
    Private DBaseAmountField As Decimal
    Private iBaseClaimPaymentKeyField As Integer
    Private sCurrencyDescriptionField As String
    Private bIsReferredField As Boolean
    Private dLossAmountField As Decimal
    Private sPartyPaidNameField As String
    Private dPaymentDateField As DateTime
    Private dTaxAmountField As Decimal
    Private sPartyPaidCode As String
    Private sLossCurrencyCode As String
    Private bPaymentOnly As Boolean
    Private iClaimKey As Integer
    Private sUltimatePayee As String
    Private bIsExGratia As Boolean
    Private lossAmountField As Decimal
    Private baseAmountField As Decimal
    Private lossCurrencyCodeField As String
    Private BaseCurrencyCodeField As String

    Private bAdvancedTaxScriptOptionOn As Boolean
    Private nRserveKey As Integer
    Private bViewMode As Boolean

    Private sDocumentReference As String
    Private sPaymentStatus As String
    Private sTheirReference As String

    Private iExgRateReasonId As Integer
    Private dCurrencyToBaseXRate As Decimal
    Private dCurrencyToBaseDate As DateTime
    Private dAccountToBaseXRate As Decimal
    Private dAccountToBaseDate As DateTime
    Private dSystemToBaseXRate As Decimal
    Private dSystemToBaseDate As DateTime
    Private dPaymentToLossXRate As Decimal

    ''' <remarks></remarks>
    Public Property AdvancedTaxScriptOptionOn() As Boolean
        Get
            Return Me.bAdvancedTaxScriptOptionOn
        End Get
        Set(value As Boolean)
            Me.bAdvancedTaxScriptOptionOn = value
        End Set
    End Property


    ''' <remarks></remarks>
    Public Property ViewMode() As Boolean
        Get
            Return Me.bViewMode
        End Get
        Set(ByVal value As Boolean)
            Me.bViewMode = value
        End Set
    End Property
    ''' <remarks></remarks>
    Public Property ReserveKey() As Integer
        Get
            Return Me.nRserveKey
        End Get
        Set(ByVal value As Integer)
            Me.nRserveKey = value
        End Set
    End Property


    Public Sub New()
        oPayee = New Payee
        oPaymentPartyType = New ClaimPaymentPartyTypeType
        oClaimPaymentItemType = New ClaimPaymentItemTypeCollection
        oPaymentAdvancedTaxDetails = New PaymentAdvancedTaxDetails
        oClaimPaymentTaxItemType = New ClaimPaymentTaxItemCollection
        oClaimPayment = New ClaimPaymentCollection
        oCashListPayment = New PaymentCashList
        oCashListItem = New PaymentCashListItemType
        oClaimPerilReservePaymentType = New ClaimPerilReservePaymentTypeCollection
    End Sub

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("CloseClaimOnZeroReserveRecoveryBalance : " & IIf(bCloseClaimOnZeroReserveRecoveryBalance, "true", "false") & "<br />")
        sbPrint.AppendLine("BaseClaimKey : " & iBaseClaimKey & "<br />")
        sbPrint.AppendLine("BaseClaimPerilKey : " & iBaseClaimPerilKey & "<br />")
        sbPrint.AppendLine("PartyKey : " & iPartyKey & "<br />")
        sbPrint.AppendLine("CurrencyCode : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("ClaimVersionDescription : " & sClaimVersionDescription & "<br />")
        sbPrint.AppendLine("Payee ---------------><br />")

        If Payee IsNot Nothing Then
            sbPrint.AppendLine(Payee.Print())
        End If
        Return sbPrint.ToString

    End Function
    Public Property IsThisPayment() As Boolean

    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property
    Public Property PaymentOnly() As Boolean
        Get
            Return Me.bPaymentOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bPaymentOnly = value
        End Set
    End Property
    Public Property ClientKey() As Integer
        Get
            Return Me.iClientKey
        End Get
        Set(ByVal value As Integer)
            Me.iClientKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property PaymentItems() As ClaimPaymentItemCollection
        Get
            Return oClaimPaymentItem
        End Get
        Set(ByVal value As ClaimPaymentItemCollection)
            oClaimPaymentItem = value
        End Set
    End Property
    Public Property CashListItem() As PaymentCashListItemType
        Get
            Return Me.oCashListItem
        End Get
        Set(ByVal value As PaymentCashListItemType)
            Me.oCashListItem = value
        End Set
    End Property
    Public Property ReverseExcess() As Boolean
        Get
            Return Me.bReverseExcess
        End Get
        Set(ByVal value As Boolean)
            Me.bReverseExcess = value
        End Set
    End Property
    Public Property PaymentAuthorized() As Boolean
        Get
            Return Me.bisPaymentAuthorized
        End Get
        Set(ByVal value As Boolean)
            Me.bisPaymentAuthorized = value
        End Set
    End Property
    '''<remarks/>
    Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
        Get
            Return Me.bCloseClaimOnZeroReserveRecoveryBalance
        End Get
        Set(ByVal value As Boolean)
            Me.bCloseClaimOnZeroReserveRecoveryBalance = value
        End Set
    End Property

    Public Property CloseClaimOnFinalPayment() As Boolean

    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property
    Public Property TaxGroupCode() As Decimal
        Get
            Return Me.sTaxGroupCode
        End Get
        Set(ByVal value As Decimal)
            Me.sTaxGroupCode = value
        End Set
    End Property
    Public Property PaymentAmount() As Decimal
        Get
            Return Me.dPaymentAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseClaimKey() As Integer
        Get
            Return Me.iBaseClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseClaimPerilKey() As Integer
        Get
            Return Me.iBaseClaimPerilKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimPerilKey = value
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
    Public Property PaymentPartyType() As ClaimPaymentPartyTypeType
        Get
            Return Me.oPaymentPartyType
        End Get
        Set(ByVal value As ClaimPaymentPartyTypeType)
            Me.oPaymentPartyType = value
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

    '''<remarks/>
    Public Property ClaimVersionDescription() As String
        Get
            Return Me.sClaimVersionDescription
        End Get
        Set(ByVal value As String)
            Me.sClaimVersionDescription = value
        End Set
    End Property

    Public Property PaymentDate() As DateTime
        Get
            Return Me.dPaymentDateField
        End Get
        Set(ByVal value As DateTime)
            Me.dPaymentDateField = value
        End Set
    End Property

    Public Property TaxAmount() As Decimal
        Get
            Return Me.dTaxAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dTaxAmountField = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimPaymentItem() As ClaimPaymentItemTypeCollection
        Get
            Return Me.oClaimPaymentItemType
        End Get
        Set(ByVal value As ClaimPaymentItemTypeCollection)
            Me.oClaimPaymentItemType = value
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

    Public Property BaseAmount() As Decimal
        Get
            Return Me.DBaseAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.DBaseAmountField = value
        End Set
    End Property
    Public Property LossCurrencyCode() As String
        Get
            Return Me.lossCurrencyCodeField
        End Get
        Set(ByVal value As String)
            Me.lossCurrencyCodeField = value
        End Set
    End Property
    Public Property BaseCurrencyCode() As String
        Get
            Return Me.BaseCurrencyCodeField
        End Get
        Set(ByVal value As String)
            Me.BaseCurrencyCodeField = value
        End Set
    End Property
    Public Property BaseClaimPaymentKey() As Integer
        Get
            Return Me.iBaseClaimPaymentKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimPaymentKeyField = value
        End Set
    End Property

    Public Property CurrencyDescription() As String
        Get
            Return Me.sCurrencyDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sCurrencyDescriptionField = value
        End Set
    End Property

    Public Property IsReferred() As Boolean
        Get
            Return Me.bIsReferredField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsReferredField = value
        End Set
    End Property

    Public Property LossAmount() As Decimal
        Get
            Return Me.dLossAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dLossAmountField = value
        End Set
    End Property

    Public Property PartyPaidName() As String
        Get
            Return Me.sPartyPaidNameField
        End Get
        Set(ByVal value As String)
            Me.sPartyPaidNameField = value
        End Set
    End Property

    Public Property PartyPaidCode() As String
        Get
            Return Me.sPartyPaidCode
        End Get
        Set(ByVal value As String)
            Me.sPartyPaidCode = value
        End Set
    End Property

    Public Property ClaimPayment() As ClaimPaymentCollection
        Get
            Return Me.oClaimPayment
        End Get
        Set(ByVal value As ClaimPaymentCollection)
            Me.oClaimPayment = value
        End Set
    End Property
    Public Property CashList() As PaymentCashList
        Get
            Return Me.oCashListPayment
        End Get
        Set(ByVal value As PaymentCashList)
            Me.oCashListPayment = value
        End Set
    End Property
    Public Property PaymentAdvancedTaxDetails() As PaymentAdvancedTaxDetails
        Get
            Return Me.oPaymentAdvancedTaxDetails
        End Get
        Set(ByVal value As PaymentAdvancedTaxDetails)
            Me.oPaymentAdvancedTaxDetails = value
        End Set
    End Property
    Public Property ClaimPaymentTaxItems() As ClaimPaymentTaxItemCollection
        Get
            Return Me.oClaimPaymentTaxItemType
        End Get
        Set(ByVal value As ClaimPaymentTaxItemCollection)
            Me.oClaimPaymentTaxItemType = value
        End Set
    End Property
    Public Property ClaimReserve() As ClaimPerilReservePaymentTypeCollection
        Get
            Return Me.oClaimPerilReservePaymentType
        End Get
        Set(ByVal value As ClaimPerilReservePaymentTypeCollection)
            Me.oClaimPerilReservePaymentType = value
        End Set
    End Property
    Public Property MediaType() As String
    Public Property MediaRefrenece() As String

    Public Property BaseReserveKey() As Integer
        Get
            Return Me.iBaseReserveKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseReserveKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalReserve() As Decimal
        Get
            Return Me.dTotalReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalReserve = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaidToDate() As Decimal
        Get
            Return Me.dPaidToDate
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidToDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaidToDateTax() As Decimal
        Get
            Return Me.dPaidToDateTax
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidToDateTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrentReserve() As Decimal
        Get
            Return Me.dCurrentReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrentReserve = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisPaymentINCLTax() As Decimal
        Get
            Return Me.dThisPaymentINCLTax
        End Get
        Set(ByVal value As Decimal)
            Me.dThisPaymentINCLTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisPaymentTax() As Decimal
        Get
            Return Me.dThisPaymentTax
        End Get
        Set(ByVal value As Decimal)
            Me.dThisPaymentTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property CostToClaim() As Decimal
        Get
            Return Me.dCostToClaim
        End Get
        Set(ByVal value As Decimal)
            Me.dCostToClaim = value
        End Set
    End Property

    Public Property ThisRevision() As Decimal
        Get
            Return Me.dThisRevision
        End Get
        Set(ByVal value As Decimal)
            Me.dThisRevision = value
        End Set
    End Property

    Public Property Balance() As Decimal
        Get
            Return Me.dBalance
        End Get
        Set(ByVal value As Decimal)
            Me.dBalance = value
        End Set
    End Property

    Public Property NetPayment() As Decimal
        Get
            Return Me.dNetPayment
        End Get
        Set(ByVal value As Decimal)
            Me.dNetPayment = value
        End Set
    End Property
    Public Property CurrencyRate() As Integer
        Get
            Return Me.iCurrencyRate
        End Get
        Set(ByVal value As Integer)
            Me.iCurrencyRate = value
        End Set
    End Property

    Public Property RiskType() As String
        Get
            Return Me.sRiskType
        End Get
        Set(ByVal value As String)
            Me.sRiskType = value
        End Set
    End Property
    Public Property ReserveType() As String
        Get
            Return Me.sReserveType
        End Get
        Set(ByVal value As String)
            Me.sReserveType = value
        End Set
    End Property
    Public Property Currency() As Decimal
        Get
            Return Me.dCurrency
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrency = value
        End Set
    End Property
    Public Property OurRef() As String
        Get
            Return Me.sOurRef
        End Get
        Set(ByVal value As String)
            Me.sOurRef = value
        End Set
    End Property
    Public Property UltimatePayee() As String
        Get
            Return Me.sUltimatePayee
        End Get
        Set(ByVal value As String)
            Me.sUltimatePayee = value
        End Set
    End Property
    Public Property IsExGratia() As Boolean
        Get
            Return Me.bIsExGratia
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExGratia = value
        End Set
    End Property

    Public Property DocumentReference() As String
        Get
            Return Me.sDocumentReference
        End Get
        Set(ByVal value As String)
            Me.sDocumentReference = value
        End Set
    End Property

    Public Property PaymentStatus() As String
        Get
            Return Me.sPaymentStatus
        End Get
        Set(ByVal value As String)
            Me.sPaymentStatus = value
        End Set
    End Property

    Public Property TheirReference() As String
        Get
            Return Me.sTheirReference
        End Get
        Set(ByVal value As String)
            Me.sTheirReference = value
        End Set
    End Property

    Public Property ExgRateReasonId() As Integer
        Get
            Return Me.iExgRateReasonId
        End Get
        Set(ByVal value As Integer)
            Me.iExgRateReasonId = value
        End Set
    End Property

    Public Property CurrencyToBaseXRate() As Decimal
        Get
            Return Me.dCurrencyToBaseXRate
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrencyToBaseXRate = value
        End Set
    End Property

    Public Property CurrencyToBaseDate() As DateTime
        Get
            Return Me.dCurrencyToBaseDate
        End Get
        Set(ByVal value As DateTime)
            Me.dCurrencyToBaseDate = value
        End Set
    End Property

    Public Property AccountToBaseXRate() As Decimal
        Get
            Return Me.dAccountToBaseXRate
        End Get
        Set(ByVal value As Decimal)
            Me.dAccountToBaseXRate = value
        End Set
    End Property

    Public Property AccountToBaseDate() As DateTime
        Get
            Return Me.dAccountToBaseDate
        End Get
        Set(ByVal value As DateTime)
            Me.dAccountToBaseDate = value
        End Set
    End Property

    Public Property SystemToBaseXRate() As Decimal
        Get
            Return Me.dSystemToBaseXRate
        End Get
        Set(ByVal value As Decimal)
            Me.dSystemToBaseXRate = value
        End Set
    End Property

    Public Property SystemToBaseDate() As DateTime
        Get
            Return Me.dSystemToBaseDate
        End Get
        Set(ByVal value As DateTime)
            Me.dSystemToBaseDate = value
        End Set
    End Property

    Public Property PaymentToLossXRate() As Decimal
        Get
            Return Me.dPaymentToLossXRate
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentToLossXRate = value
        End Set
    End Property

End Class
<Serializable()> Public Class ClaimPaymentCollection : Inherits SortableCollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oClaimPayment As ClaimPayment In List
            sbPrint.AppendLine(oClaimPayment.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oClaimPayment As ClaimPayment) As Integer
        Return List.Add(v_oClaimPayment)
    End Function

    Public Sub Remove(ByVal v_oClaimPayment As ClaimPayment)
        List.Remove(v_oClaimPayment)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPayment
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPayment)
            List(i) = value
        End Set
    End Property

End Class
<Serializable()> Public Class ClaimPaymentItem
#Region "PrivateFields"
    Private iBaseReserveKeyField As Integer

    Private iReserveKeyField As Integer

    Private iBaseRecoveryKeyField As Integer

    Private sTaxGroupCodeField As String

    Private dPaymentAmountField As Decimal

    Private bReverseExcessField As Boolean

    Private iBaseClaimPaymentItemKeyField As Integer

    Private dPaymentAdjustmentField As Decimal

    Private dTaxAmountField, dTotalTaxAmount As Decimal

    Private dThisRevisionField As Decimal
    Private lossAmountField As Decimal
    Private lossTaxAmountField As Decimal
    Private baseAmountField As Decimal

#End Region
#Region "Properties"

    '''<remarks/>
    Public Property ReserveKey() As Integer
        Get
            Return Me.iReserveKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iReserveKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseReserveKey() As Integer
        Get
            Return Me.iBaseReserveKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseReserveKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxGroupCode() As String
        Get
            Return Me.sTaxGroupCodeField
        End Get
        Set(ByVal value As String)
            Me.sTaxGroupCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentAmount() As Decimal
        Get
            Return Me.dPaymentAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReverseExcess() As Boolean
        Get
            Return Me.bReverseExcessField
        End Get
        Set(ByVal value As Boolean)
            Me.bReverseExcessField = value
        End Set
    End Property

    Public Property BaseClaimPaymentItemKey() As Integer
        Get
            Return Me.iBaseClaimPaymentItemKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimPaymentItemKeyField = value
        End Set
    End Property

    Public Property BaseRecoveryKey() As Integer
        Get
            Return Me.iBaseRecoveryKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseRecoveryKeyField = value
        End Set
    End Property

    Public Property PaymentAdjustment() As Decimal
        Get
            Return Me.dPaymentAdjustmentField
        End Get
        Set(ByVal value As Decimal)
            Me.dPaymentAdjustmentField = value
        End Set
    End Property

    Public Property TaxAmount() As Decimal
        Get
            Return Me.dTaxAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dTaxAmountField = value
        End Set
    End Property
    Public Property TotalTaxAmount() As Decimal
        Get
            Return Me.dTotalTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalTaxAmount = value
        End Set
    End Property

    Public Property ThisRevision() As Decimal
        Get
            Return Me.dThisRevisionField
        End Get
        Set(ByVal value As Decimal)
            Me.dThisRevisionField = value
        End Set
    End Property

    Public Property LossAmount() As Decimal
        Get
            Return Me.lossAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.lossAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseAmount() As Decimal
        Get
            Return Me.baseAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.baseAmountField = value
        End Set
    End Property
    Public Property LossTaxAmount() As Decimal
        Get
            Return Me.LossTaxAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.LossTaxAmountField = value
        End Set
    End Property
    Public Property MediaType() As String
    Public Property MediaRefrenece() As String

#End Region

End Class

Public Enum ClaimPaymentPartyTypeType

    '''<remarks/>
    CLMPAYABLE

    '''<remarks/>
    PARTY

    '''<remarks/>
    AGENT

    '''<remarks/>
    CLIENT
End Enum
<Serializable()> Public Class ClaimPaymentItemCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oClaimPaymentItem As ClaimPaymentItem) As Integer
        Return List.Add(v_oClaimPaymentItem)
    End Function

    Public Sub Remove(ByVal v_oClaimPaymentItem As ClaimPaymentItem)
        List.Remove(v_oClaimPaymentItem)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPaymentItem
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPaymentItem)
            List(i) = value
        End Set
    End Property

End Class
