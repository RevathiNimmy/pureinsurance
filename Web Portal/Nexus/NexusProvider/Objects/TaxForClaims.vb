<Serializable()> Public Class TaxForClaims

    Private sCompanyCodeField As String

    Private sTaxGroupCodeField As String

    Private sCurrencyCodeField As String

    Private sLossCurrencyCodeField As String

    Private dAmountField As Double

    Private dTaxCurrencyAmountField As Double

    Private dTaxLossAmountField As Double

    Private dTaxBaseAmountField As Double

    Private sPayeeName As String
    Private bIsExcess As Boolean
    Private bAdvancedTaxScriptOptionOn As Boolean
    Private oPaymentAdvancedTaxDetails As PaymentAdvancedTaxDetails
    Private oReceiptAdvancedTaxDetails As ClaimReceiptAdvancedTaxDetails

    Public Property ReceiptAdvancedTaxDetails() As ClaimReceiptAdvancedTaxDetails
        Get
            Return Me.oReceiptAdvancedTaxDetails
        End Get
        Set(ByVal value As ClaimReceiptAdvancedTaxDetails)
            Me.oReceiptAdvancedTaxDetails = value
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
    Public Property AdvancedTaxScriptOptionOn() As Boolean
        Get
            Return Me.bAdvancedTaxScriptOptionOn
        End Get
        Set(ByVal value As Boolean)
            Me.bAdvancedTaxScriptOptionOn = value
        End Set
    End Property
    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsExcess() As Boolean
        Get
            Return Me.bIsExcess
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExcess = value
        End Set
    End Property
    Private taxItemsField As ClaimPaymentTaxItemCollection

    Private sTransactionTypeCode As String

    Public Property TaxItems() As ClaimPaymentTaxItemCollection
        Get
            Return Me.taxItemsField
        End Get
        Set(ByVal value As ClaimPaymentTaxItemCollection)
            Me.taxItemsField = value
        End Set
    End Property
    Private oReceiptTaxItem As TaxItemTypeCollection
    Public Property ReceiptTaxItem() As TaxItemTypeCollection
        Get
            Return Me.oReceiptTaxItem
        End Get
        Set(ByVal value As TaxItemTypeCollection)
            Me.oReceiptTaxItem = value
        End Set
    End Property
    '''<remarks/>
    Public Property TaxCurrencyAmount() As Double
        Get
            Return Me.dTaxCurrencyAmountField
        End Get
        Set(ByVal value As Double)
            Me.dTaxCurrencyAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxLossAmount() As Double
        Get
            Return Me.dTaxLossAmountField
        End Get
        Set(ByVal value As Double)
            Me.dTaxLossAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxBaseAmount() As Double
        Get
            Return Me.dTaxBaseAmountField
        End Get
        Set(ByVal value As Double)
            Me.dTaxBaseAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CompanyCode() As String
        Get
            Return Me.sCompanyCodeField
        End Get
        Set(ByVal value As String)
            Me.sCompanyCodeField = value
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
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCodeField
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossCurrencyCode() As String
        Get
            Return Me.sLossCurrencyCodeField
        End Get
        Set(ByVal value As String)
            Me.sLossCurrencyCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return Me.dAmountField
        End Get
        Set(ByVal value As Double)
            Me.dAmountField = value
        End Set
    End Property
    Private iClaimPerilID As Integer
    Public Property ClaimPerilID() As Integer
        Get
            Return Me.iClaimPerilID
        End Get
        Set(ByVal value As Integer)
            Me.iClaimPerilID = value
        End Set
    End Property

    Public Property TransactionTypeCode() As String
        Get
            Return Me.sTransactionTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTransactionTypeCode = value
        End Set
    End Property

    Private sReserveType As String
    Public Property ReserveType() As String
        Get
            Return Me.sReserveType
        End Get
        Set(ByVal value As String)
            Me.sReserveType = value
        End Set
    End Property
    Private sReserveKey As Integer
    Public Property ReserveKey() As Integer
        Get
            Return Me.sReserveKey
        End Get
        Set(ByVal value As Integer)
            Me.sReserveKey = value
        End Set
    End Property
    Private bIsSalvageRecovery As Boolean
    Public Property IsSalvageRecovery() As Boolean
        Get
            Return Me.bIsSalvageRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSalvageRecovery = value
        End Set
    End Property
    Private sMode As String
    Public Property Mode() As String
        Get
            Return Me.sMode
        End Get
        Set(ByVal value As String)
            Me.sMode = value
        End Set
    End Property

    Private sRecoveryType As String
    Public Property RecoveryType() As String
        Get
            Return Me.sRecoveryType
        End Get
        Set(ByVal value As String)
            Me.sRecoveryType = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Company Key: " & sCompanyCodeField & "<br />")
        sbPrint.AppendLine("TaxGroup Key: " & sTaxGroupCodeField & "<br />")
        sbPrint.AppendLine("Currency Key: " & sCurrencyCodeField & "<br />")
        sbPrint.AppendLine("Loss Currency Key: " & sLossCurrencyCodeField & "<br />")
        sbPrint.AppendLine("Amount: " & dAmountField & "<br />")
        sbPrint.AppendLine("Tax Currency Amount: " & dTaxCurrencyAmountField & "<br />")
        sbPrint.AppendLine("Tax Loss Amount: " & dTaxLossAmountField & "<br />")
        sbPrint.AppendLine("Tax Base Amount: " & dTaxBaseAmountField & "<br />")

        Return sbPrint.ToString()

    End Function

End Class
