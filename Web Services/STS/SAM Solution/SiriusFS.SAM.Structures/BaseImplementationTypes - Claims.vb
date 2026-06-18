Option Strict On

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS'''

#Region " Imports "

Imports System
Imports System.Xml.Serialization
Imports Sirius.Architecture.ExceptionHandling
Imports System.Collections.Generic

'Imports SiriusFS.SAM.ServiceAgent.PMEReturnCode
Imports System.Xml

#End Region

Namespace BaseImplementationTypes

    '''<remarks/>
    Public Class BaseClaimMTARequestType
        Inherits BaseUpdateRiskRequestType
    End Class


    Public Class BaseUpdateClaimRiskRequestType
        Inherits BaseRequestType

        Private baseclaimKeyField As Integer

        Private xMLDataSetField As String

        Private timeStampField() As Byte

        '''<remarks/>
        Public Property BaseClaimKey() As Integer
            Get
                Return Me.baseclaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseclaimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.xMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    Public Class BaseClaimReceiptRequestType
        Inherits BaseRequestType

        Private _dataTransferIsUsingFullClaimVersioning As Boolean
        Public Property DataTransferIsUsingFullClaimVersioning() As Boolean
            Get
                Return _dataTransferIsUsingFullClaimVersioning
            End Get
            Set(ByVal value As Boolean)
                _dataTransferIsUsingFullClaimVersioning = value
            End Set
        End Property

        Private _dataTransferClaimHasSpecifiedClaimRiskData As Boolean
        Public Property DataTransferClaimHasClaimRiskDataSpecified() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedClaimRiskData
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedClaimRiskData = value
            End Set
        End Property

        Private claimReceiptField As BaseClaimReceiptType

        Private timeStampField() As Byte

        Public Property GetSavedTaxOfPeril() As Integer

        Private closeClaimOnZeroReserveRecoveryBalanceField As Boolean

        Public Property PostTransaction As Boolean

        Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
            Get
                Return Me.closeClaimOnZeroReserveRecoveryBalanceField
            End Get
            Set(ByVal value As Boolean)
                Me.closeClaimOnZeroReserveRecoveryBalanceField = value
            End Set
        End Property
        '''<remarks/>
        Public Property ClaimReceipt() As BaseClaimReceiptType
            Get
                Return Me.claimReceiptField
            End Get
            Set(ByVal value As BaseClaimReceiptType)
                Me.claimReceiptField = value
            End Set
        End Property

        Public Property ClaimReceiptCollection() As System.Collections.Generic.List(Of BaseClaimReceiptType)

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        Private _ReceiptPartyAccountId As Integer
        Public Property ReceiptPartyAccountId() As Integer
            Get
                Return _ReceiptPartyAccountId
            End Get
            Set(ByVal value As Integer)
                _ReceiptPartyAccountId = value
            End Set
        End Property

        Private _ReceiptPartyAccountCode As String
        Public Property ReceiptPartyAccountCode() As String
            Get
                Return _ReceiptPartyAccountCode
            End Get
            Set(ByVal value As String)
                _ReceiptPartyAccountCode = value
            End Set
        End Property

        Private _ReceiptPartyShortCode As String
        Public Property ReceiptPartyShortCode() As String
            Get
                Return _ReceiptPartyShortCode
            End Get
            Set(ByVal value As String)
                _ReceiptPartyShortCode = value
            End Set
        End Property

        Private _dataTransferClaim As Boolean = False
        Public Property IsDataTransferClaim() As Boolean
            Get
                Return _dataTransferClaim
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaim = value
            End Set
        End Property

        Private _dataTransferClaimHasSpecifiedReinsurance As Boolean
        Public Property DataTransferClaimHasSpecifiedReinsurance() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedReinsurance
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedReinsurance = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)
            If ClaimReceiptCollection IsNot Nothing Then
                For ReceiptItemIndex As Integer = 0 To ClaimReceiptCollection.Count - 1
                    ClaimReceiptCollection(ReceiptItemIndex).Validate(oErrorCollection)
                Next
            Else
                ClaimReceipt.Validate(oErrorCollection)
            End If
        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimReceiptType

        Private baseClaimKeyField As Integer
        Private baseClaimPerilKeyField As Integer
        Private _currencyId As Integer
        Private _claimId As Integer
        Private _versionId As Integer
        Private _claimPerilId As Integer
        Private _dReceiptToLossxRate As Decimal
        Private _exchange_rate_override_reason_id As Int16
        Private _currency_base_xrate As Decimal
        Private _account_base_xrate As Decimal
        Private _system_base_xrate As Decimal
        Private _currency_base_date As DateTime
        Private _account_base_date As DateTime
        Private _system_base_date As DateTime
        Private _nDoNotCreateClaimVersionOnSalvageReceipt As Integer

        Private _AdditionalDetails As BaseAdditionalClaimRelatedDetails
        Public Property AdditionalDetails() As BaseAdditionalClaimRelatedDetails
            Get
                Return _AdditionalDetails
            End Get
            Set(ByVal value As BaseAdditionalClaimRelatedDetails)
                _AdditionalDetails = value
            End Set
        End Property

        Private receiptPartyTypeField As ClaimReceiptPartyTypeType

        Private partyKeyField As Integer

        Private currencyCodeField As String

        Private claimVersionDescriptionField As String

        Private advancedTaxDetailsField As BaseClaimReceiptAdvancedTaxDetailsType

        Private receiptItemField() As BaseClaimReceiptItemType

        Private payeeField As BaseClaimPayeeType

        Private isGetClaimReceiptTaxesTypeField As Boolean

        Private _Coinsurers As New List(Of BaseInsurerType)
        Public Property Coinsurers() As List(Of BaseInsurerType)
            Get
                Return _Coinsurers
            End Get
            Set(ByVal value As List(Of BaseInsurerType))
                _Coinsurers = value
            End Set
        End Property

        Private _Reinsurers As New List(Of BaseInsurerType)
        Public Property Reinsurers() As List(Of BaseInsurerType)
            Get
                Return _Reinsurers
            End Get
            Set(ByVal value As List(Of BaseInsurerType))
                _Reinsurers = value
            End Set
        End Property

        Public ReadOnly Property TotalReceiptAmountGross() As Decimal
            Get
                Dim _totalReceiptAmountGross As Decimal = 0

                If ReceiptItem IsNot Nothing Then
                    For Each ClaimReceiptItem As BaseClaimReceiptItemType In ReceiptItem
                        If ClaimReceiptItem.IsTPSalvageExcludeTax Then
                            _totalReceiptAmountGross += (ClaimReceiptItem.ReceiptAmount + ClaimReceiptItem.TotalTaxAmount)
                        Else
                            _totalReceiptAmountGross += ClaimReceiptItem.ReceiptAmount
                        End If
                    Next
                End If

                Return _totalReceiptAmountGross
            End Get

        End Property

        Public ReadOnly Property TotalReceiptAmountNet() As Decimal
            Get
                Dim _totalReceiptAmountNet As Decimal = 0

                If ReceiptItem IsNot Nothing Then
                    For Each ClaimReceiptItem As BaseClaimReceiptItemType In ReceiptItem
                        If ClaimReceiptItem.IsTPSalvageExcludeTax Then
                            _totalReceiptAmountNet += (ClaimReceiptItem.ReceiptAmount)
                        Else
                            _totalReceiptAmountNet += (ClaimReceiptItem.ReceiptAmount - ClaimReceiptItem.TotalTaxAmount)
                        End If
                    Next
                End If

                Return _totalReceiptAmountNet
            End Get

        End Property

        Public ReadOnly Property TotalReceiptTaxAmount() As Decimal
            Get
                Dim _totalReceiptTaxAmount As Decimal = 0

                If ReceiptItem IsNot Nothing Then
                    For Each ClaimReceiptItem As BaseClaimReceiptItemType In ReceiptItem
                        _totalReceiptTaxAmount += ClaimReceiptItem.TotalTaxAmount
                    Next
                End If

                Return _totalReceiptTaxAmount
            End Get

        End Property

        Private _baseCurrencyId As Integer
        Public Property BaseCurrencyId() As Integer
            Get
                Return _baseCurrencyId
            End Get
            Set(ByVal value As Integer)
                _baseCurrencyId = value
            End Set
        End Property

        Private _baseAmount As Decimal
        Public Property BaseAmount() As Decimal
            Get
                Return _baseAmount
            End Get
            Set(ByVal value As Decimal)
                _baseAmount = value
            End Set
        End Property

        Private _accountCurrencyId As Integer
        Public Property AccountCurrencyId() As Integer
            Get
                Return _accountCurrencyId
            End Get
            Set(ByVal value As Integer)
                _accountCurrencyId = value
            End Set
        End Property

        Private _accountAmount As Decimal
        Public Property AccountAmount() As Decimal
            Get
                Return _accountAmount
            End Get
            Set(ByVal value As Decimal)
                _accountAmount = value
            End Set
        End Property

        Private _systemCurrencyId As Integer
        Public Property SystemCurrencyId() As Integer
            Get
                Return _systemCurrencyId
            End Get
            Set(ByVal value As Integer)
                _systemCurrencyId = value
            End Set
        End Property

        Private _systemAmount As Decimal
        Public Property SystemAmount() As Decimal
            Get
                Return _systemAmount
            End Get
            Set(ByVal value As Decimal)
                _systemAmount = value
            End Set
        End Property

        Private _transactionDate As Date
        Public Property TransactionDate() As Date
            Get
                Return _transactionDate
            End Get
            Set(ByVal value As Date)
                _transactionDate = value
            End Set
        End Property

        Private _claimReceiptId As Integer
        Public Property ClaimReceiptId() As Integer
            Get
                Return _claimReceiptId
            End Get
            Set(ByVal value As Integer)
                _claimReceiptId = value
            End Set
        End Property

        Private _TransactionTypeId As Integer
        Public Property TransactionTypeId() As Integer
            Get
                Return _TransactionTypeId
            End Get
            Set(ByVal value As Integer)
                _TransactionTypeId = value
            End Set
        End Property

        Public ReadOnly Property TransactionTypeCode() As String
            Get
                Dim _transactionTypeCode As String = "C_RV"
                If IsSalvageRecovery Then
                    _transactionTypeCode = "C_SA"
                End If
                Return _transactionTypeCode
            End Get
        End Property

        Private _isSalvageRecovery As Boolean
        Public Property IsSalvageRecovery() As Boolean
            Get
                Return _isSalvageRecovery
            End Get
            Set(ByVal value As Boolean)
                _isSalvageRecovery = value
            End Set
        End Property

        Private _isAdvancedTaxSystemOptionOn As Boolean
        Public Property IsAdvancedTaxSystemOptionOn() As Boolean
            Get
                Return _isAdvancedTaxSystemOptionOn
            End Get
            Set(ByVal value As Boolean)
                _isAdvancedTaxSystemOptionOn = value
            End Set
        End Property

        '''<remarks/>
        Public Property VersionId() As Integer
            Get
                Return _versionId
            End Get
            Set(ByVal value As Integer)
                _versionId = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyId() As Integer
            Get
                Return _currencyId
            End Get
            Set(ByVal value As Integer)
                _currencyId = value
            End Set
        End Property
        '''<remarks/>
        Public Property ClaimId() As Integer
            Get
                Return _claimId
            End Get
            Set(ByVal value As Integer)
                _claimId = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPerilId() As Integer
            Get
                Return _claimPerilId
            End Get
            Set(ByVal value As Integer)
                _claimPerilId = value
            End Set
        End Property
        '''<remarks/>
        Public Property ReceiptToLossXRate() As Decimal
            Get
                Return _dReceiptToLossxRate
            End Get
            Set(ByVal value As Decimal)
                _dReceiptToLossxRate = value
            End Set
        End Property
        '''<remarks/>
        Public Property ExchangeRateOverrideReasonId() As Int16
            Get
                Return _exchange_rate_override_reason_id
            End Get
            Set(ByVal value As Int16)
                _exchange_rate_override_reason_id = value
            End Set
        End Property
        '''<remarks/>
        Public Property CurrencyToBaseXRate() As Decimal
            Get
                Return _currency_base_xrate
            End Get
            Set(ByVal value As Decimal)
                _currency_base_xrate = value
            End Set
        End Property
        '''<remarks/>
        Public Property AccountToBaseXRate() As Decimal
            Get
                Return _account_base_xrate
            End Get
            Set(ByVal value As Decimal)
                _account_base_xrate = value
            End Set
        End Property
        '''<remarks/>
        Public Property SystemToBaseXRate() As Decimal
            Get
                Return _system_base_xrate
            End Get
            Set(ByVal value As Decimal)
                _system_base_xrate = value
            End Set
        End Property

        '''<remarks/>
        Public Property SystemToBaseDate() As DateTime
            Get
                Return _system_base_date
            End Get
            Set(ByVal value As DateTime)
                _system_base_date = value
            End Set
        End Property
        '''<remarks/>
        Public Property AccountToBaseDate() As DateTime
            Get
                Return _account_base_date
            End Get
            Set(ByVal value As DateTime)
                _account_base_date = value
            End Set
        End Property
        '''<remarks/>
        Public Property CurrencyToBaseDate() As DateTime
            Get
                Return _currency_base_date
            End Get
            Set(ByVal value As DateTime)
                _currency_base_date = value
            End Set
        End Property
        '''<remarks/>
        Public Property BaseClaimKey() As Integer
            Get
                Return Me.baseClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimPerilKey() As Integer
            Get
                Return Me.baseClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimPerilKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptPartyType() As ClaimReceiptPartyTypeType
            Get
                Return Me.receiptPartyTypeField
            End Get
            Set(ByVal value As ClaimReceiptPartyTypeType)
                Me.receiptPartyTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimVersionDescription() As String
            Get
                Return Me.claimVersionDescriptionField
            End Get
            Set(ByVal value As String)
                Me.claimVersionDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdvancedTaxDetails() As BaseClaimReceiptAdvancedTaxDetailsType
            Get
                Return Me.advancedTaxDetailsField
            End Get
            Set(ByVal value As BaseClaimReceiptAdvancedTaxDetailsType)
                Me.advancedTaxDetailsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptItem() As BaseClaimReceiptItemType()
            Get
                Return Me.receiptItemField
            End Get
            Set(ByVal value As BaseClaimReceiptItemType())
                Me.receiptItemField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Payee() As BaseClaimPayeeType
            Get
                Return Me.payeeField
            End Get
            Set(ByVal value As BaseClaimPayeeType)
                Me.payeeField = value
            End Set
        End Property

        Public Property IsGetClaimReceiptTaxesType() As Boolean
            Get
                Return Me.isGetClaimReceiptTaxesTypeField
            End Get
            Set(ByVal value As Boolean)
                Me.isGetClaimReceiptTaxesTypeField = value
            End Set
        End Property
        ''' <summary>
        ''' This Property is used to pass the flag to prevent the new version during the claimReceipt
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DoNotCreateClaimVersionOnSalvageReceipt() As Integer
            Get
                Return _nDoNotCreateClaimVersionOnSalvageReceipt
            End Get
            Set(ByVal value As Integer)
                _nDoNotCreateClaimVersionOnSalvageReceipt = value
            End Set
        End Property
        Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
        '''<remarks/>
        Public Property ReceiptcashList() As BaseReceiptCashListType

        '''<remarks/>
        Public Property ReceiptPayee() As BaseClaimReceiptPayeeType

        Private _dataSalvageAndTPRecoveryReservesExcludeTaxSystemOption As Boolean
        Public Property SalvageAndTPRecoveryReservesExcludeTaxSystemOption() As Boolean


        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oClaimReceiptItemType As BaseClaimReceiptItemType

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If BaseClaimKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "BaseClaimKey", "")
            End If

            If BaseClaimPerilKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "BaseClaimPerilKey", "")
            End If

            ' the party cnt is mandatory only if the specified payment party type is Party
            If ReceiptPartyType = ClaimReceiptPartyTypeType.PARTY AndAlso PartyKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "PartyKey", "")
            End If


            ' the party cnt should always be zero where the payment party type is not PARTY
            If ReceiptPartyType <> ClaimReceiptPartyTypeType.PARTY AndAlso PartyKey <> 0 Then
                'oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.SpecifiedPartyTypeShouldHaveAPartyKeyofZero, _
                '                                        SAMConstants.SAMInvalidData.SpecifiedPartyTypeShouldHaveAPartyKeyofZero.ToString, _
                '                                        "PartyKey")
            End If

            If String.IsNullOrEmpty(CurrencyCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "CurrencyCode")
            End If

            If ReceiptItem Is Nothing OrElse ReceiptItem.Length < 1 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ReceiptItem")
            Else
                For Each oClaimReceiptItemType In ReceiptItem
                    oClaimReceiptItemType.Validate(oErrorCollection)
                Next
            End If

            If IsGetClaimReceiptTaxesType = False And ReceiptPartyType <> ClaimReceiptPartyTypeType.CLMRECEIVABLE Then
                Payee.Validate(oErrorCollection)
            End If

        End Sub

    End Class


    '''<remarks/>
    Public Class BaseClaimReceiptAdvancedTaxDetailsType

        Private isSettlementField As Boolean

        Private isSettlementFieldSpecified As Boolean

        Private isTaxExemptField As Boolean

        Private isTaxExemptFieldSpecified As Boolean

        Private receivableTaxPercentageField As Decimal

        Private receivableTaxPercentageFieldSpecified As Boolean

        Private insuredDomiciledField As Boolean

        Private insuredDomiciledFieldSpecified As Boolean

        Private insuredPercentageField As Decimal

        Private insuredPercentageFieldSpecified As Boolean

        Private insuredTaxNumberField As String

        Private paymentToCodeField As String

        '''<remarks/>
        Public Property IsSettlement() As Boolean
            Get
                Return Me.isSettlementField
            End Get
            Set(ByVal value As Boolean)
                Me.isSettlementField = value
            End Set
        End Property

        '''<remarks/>

        Public Property IsSettlementSpecified() As Boolean
            Get
                Return Me.isSettlementFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isSettlementFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentToCode() As String
            Get
                Return Me.paymentToCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentToCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsTaxExempt() As Boolean
            Get
                Return Me.isTaxExemptField
            End Get
            Set(ByVal value As Boolean)
                Me.isTaxExemptField = value
            End Set
        End Property

        '''<remarks/>

        Public Property IsTaxExemptSpecified() As Boolean
            Get
                Return Me.isTaxExemptFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isTaxExemptFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceivableTaxPercentage() As Decimal
            Get
                Return Me.receivableTaxPercentageField
            End Get
            Set(ByVal value As Decimal)
                Me.receivableTaxPercentageField = value
            End Set
        End Property

        '''<remarks/>

        Public Property ReceivableTaxPercentageSpecified() As Boolean
            Get
                Return Me.receivableTaxPercentageFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.receivableTaxPercentageFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuredDomiciled() As Boolean
            Get
                Return Me.insuredDomiciledField
            End Get
            Set(ByVal value As Boolean)
                Me.insuredDomiciledField = value
            End Set
        End Property

        '''<remarks/>

        Public Property InsuredDomiciledSpecified() As Boolean
            Get
                Return Me.insuredDomiciledFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.insuredDomiciledFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuredPercentage() As Decimal
            Get
                Return Me.insuredPercentageField
            End Get
            Set(ByVal value As Decimal)
                Me.insuredPercentageField = value
            End Set
        End Property

        '''<remarks/>

        Public Property InsuredPercentageSpecified() As Boolean
            Get
                Return Me.insuredPercentageFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.insuredPercentageFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuredTaxNumber() As String
            Get
                Return Me.insuredTaxNumberField
            End Get
            Set(ByVal value As String)
                Me.insuredTaxNumberField = value
            End Set
        End Property

        Public Property AdvancedTaxScriptOptionOn() As Boolean

        Public Property PayeeName() As String
    End Class

    '''<remarks/>
    Public Class BaseClaimReceiptItemType

        Private baseRecoveryKeyField As Long

        Private taxGroupCodeField As String

        Private _taxGroupId As Nullable(Of Integer)
        Private _recoveryTypeId As Integer
        Private _recoveryId As Integer
        Private _recoveryType As String
        Private _taxAmount As Decimal
        Private _bIsTPSalvageExcludeTax As Boolean

        'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
        ''Saurabh -- Changed Key To Code
        Private recoveryPartyTypeCodeField As String

        Private recoveryPartyTypeCodeFieldSpecified As Boolean

        Private recoveryPartyCodeField As String

        Private recoveryPartyCodeFieldSpecified As Boolean
        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

        Public Property IsTaxOverridden() As Boolean

        Public Property OverriddedTaxAmount() As Decimal

        Public ReadOnly Property NetAmount() As Decimal
            Get
                Dim _NetAmount As Decimal = 0
                If _bIsTPSalvageExcludeTax Then
                    _NetAmount = ReceiptAmount
                Else
                    _NetAmount = ReceiptAmount - TotalTaxAmount
                End If

                Return _NetAmount
            End Get

        End Property

        Private _ClaimReceiptItemId As Integer
        Public Property ClaimReceiptItemId() As Integer
            Get
                Return _ClaimReceiptItemId
            End Get
            Set(ByVal value As Integer)
                _ClaimReceiptItemId = value
            End Set
        End Property

        Private _taxCalculationItem As New List(Of BaseTaxCalculationItemType)
        Public Property TaxCalculationItem() As List(Of BaseTaxCalculationItemType)
            Get
                Return _taxCalculationItem
            End Get
            Set(ByVal value As List(Of BaseTaxCalculationItemType))
                _taxCalculationItem = value
            End Set
        End Property


        Public ReadOnly Property TotalTaxAmount() As Decimal
            Get
                Dim TotalTaxValue As Decimal = 0

                If TaxCalculationItem IsNot Nothing Then

                    For Each oItem As BaseTaxCalculationItemType In TaxCalculationItem
                        TotalTaxValue = TotalTaxValue + oItem.TaxValue
                    Next

                End If

                Return TotalTaxValue

            End Get

        End Property
        Private _taxGroupAdvancedTaxScript As String
        Public Property TaxGroupAdvancedTaxScript() As String
            Get
                Return _taxGroupAdvancedTaxScript
            End Get
            Set(ByVal value As String)
                _taxGroupAdvancedTaxScript = value
            End Set
        End Property

        Private receiptAmountField As Decimal
        Private _scriptedTaxAmount As Decimal
        '''<remarks/>
        Public Property ScriptedTaxAmount() As Decimal
            Get
                Return _scriptedTaxAmount
            End Get
            Set(ByVal value As Decimal)
                _scriptedTaxAmount = value
            End Set
        End Property


        '''<remarks/>
        Public Property TaxAmount() As Decimal
            Get
                Return _taxAmount
            End Get
            Set(ByVal value As Decimal)
                _taxAmount = value
            End Set
        End Property
        '''<remarks/>
        Public Property RecoveryTypeId() As Integer
            Get
                Return _recoveryTypeId
            End Get
            Set(ByVal value As Integer)
                _recoveryTypeId = value
            End Set
        End Property
        '''<remarks/>
        Public Property TaxGroupId() As Nullable(Of Integer)
            Get
                Return _taxGroupId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _taxGroupId = value
            End Set
        End Property
        '''<remarks/>
        Public Property RecoveryId() As Integer
            Get
                Return _recoveryId
            End Get
            Set(ByVal value As Integer)
                _recoveryId = value
            End Set
        End Property
        '''<remarks/>
        Public Property RecoveryTypeCode() As String
            Get
                Return _recoveryType
            End Get
            Set(ByVal value As String)
                _recoveryType = value

                If TaxCalculationItem IsNot Nothing Then
                    For Each oTaxCalculationItem As BaseTaxCalculationItemType In TaxCalculationItem
                        oTaxCalculationItem.RecoveryType = _recoveryType
                    Next
                End If

            End Set
        End Property
        Public Property BaseRecoveryKey() As Long
            Get
                Return Me.baseRecoveryKeyField
            End Get
            Set(ByVal value As Long)
                Me.baseRecoveryKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptAmount() As Decimal
            Get
                Return Me.receiptAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptAmountField = value
            End Set
        End Property
        'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
        '''<remarks/>
        Public Property RecoveryPartyTypeCode() As String
            Get
                Return Me.recoveryPartyTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryPartyTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoveryPartyTypeCodeSpecified() As Boolean
            Get
                Return Me.recoveryPartyTypeCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoveryPartyTypeCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryPartyCode() As String
            Get
                Return Me.recoveryPartyCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryPartyCodeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoveryPartyCodeSpecified() As Boolean
            Get
                Return Me.recoveryPartyCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoveryPartyCodeFieldSpecified = value
            End Set
        End Property


        Public Property IsTPSalvageExcludeTax() As Boolean
            Get
                Return Me._bIsTPSalvageExcludeTax
            End Get
            Set(ByVal value As Boolean)
                Me._bIsTPSalvageExcludeTax = value
            End Set
        End Property
        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If BaseRecoveryKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "BaseRecoveryKey")
            End If

            If ReceiptAmount = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ReceiptAmount")
            End If
            ''Saurabh -- If recovery partyTypeKey is specified then recoverypartykey is mandatory else it is claimreceviable
            If RecoveryPartyTypeCodeSpecified = True And RecoveryPartyCode = "" Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "RecoveryPartyType")
            End If

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseClaimPayeeType

        Private nameField As String

        Private bankNameField As String

        Private bankNumberField As String

        Private bankCodeField As String

        Private mediaTypeCodeField As String

        Private mediaReferenceField As String

        Private theirReferenceField As String

        Private addressField As BaseAddressType

        Private commentsField As String

        Private _mediaTypeId As Integer

        Private chequedatefield As Date
        'Start (Prakash C Varghese)-(PartyBank functionality)
        Private partyBankKeyField As Integer
        'End (Prakash C Varghese)-(PartyBank functionality)
        '''<remarks/>
        Public Property Chequedate() As Date
            Get
                Return chequedatefield
            End Get
            Set(ByVal value As Date)
                chequedatefield = value
            End Set
        End Property

        Private sBICField As String

        Private sIBANField As String

        Public Property MediaTypeId() As Integer
            Get
                Return _mediaTypeId
            End Get
            Set(ByVal value As Integer)
                _mediaTypeId = value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Comments() As String
            Get
                Return Me.commentsField
            End Get
            Set(ByVal value As String)
                Me.commentsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankName() As String
            Get
                Return Me.bankNameField
            End Get
            Set(ByVal value As String)
                Me.bankNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankNumber() As String
            Get
                Return Me.bankNumberField
            End Get
            Set(ByVal value As String)
                Me.bankNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankCode() As String
            Get
                Return Me.bankCodeField
            End Get
            Set(ByVal value As String)
                Me.bankCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaTypeCode() As String
            Get
                Return Me.mediaTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.mediaTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaReference() As String
            Get
                Return Me.mediaReferenceField
            End Get
            Set(ByVal value As String)
                Me.mediaReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TheirReference() As String
            Get
                Return Me.theirReferenceField
            End Get
            Set(ByVal value As String)
                Me.theirReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Address() As BaseAddressType
            Get
                Return Me.addressField
            End Get
            Set(ByVal value As BaseAddressType)
                Me.addressField = value
            End Set
        End Property


        '''<remarks/>
        Public Property PartyBankKey() As Integer
            Get
                Return Me.partyBankKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyBankKeyField = value
            End Set
        End Property

        ''' <summary>
        ''' Loss currency code for claim payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LossCurrencyCode As String

        Public Property BIC() As String
            Get
                Return Me.sBICField
            End Get
            Set(ByVal value As String)
                Me.sBICField = value
            End Set
        End Property

        Public Property IBAN() As String
            Get
                Return Me.sIBANField
            End Get
            Set(ByVal value As String)
                Me.sIBANField = value
            End Set
        End Property
        Public Property MediaTypeDesc() As String

        Public Property AccountType() As String


        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            'If String.IsNullOrEmpty(MediaReference) Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                    "MediaReference")
            'End If

            If Address IsNot Nothing Then
                Address.Validate(oErrorCollection)
            End If

        End Sub

    End Class

    Public Class BaseClaimReceiptPayeeType

        Private nameField As String

        Private bankNameField As String

        Private bankNumberField As String

        Private bankCodeField As String

        Private mediaTypeCodeField As String

        Private mediaReferenceField As String

        Private theirReferenceField As String

        Private addressField As BaseAddressType

        Private commentsField As String

        Private _mediaTypeId As Integer


        Private partyBankKeyField As Integer


        '''<remarks/>
        Public Property MediaTypeId() As Integer
            Get
                Return _mediaTypeId
            End Get
            Set(ByVal value As Integer)
                _mediaTypeId = value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Comments() As String
            Get
                Return Me.commentsField
            End Get
            Set(ByVal value As String)
                Me.commentsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankName() As String
            Get
                Return Me.bankNameField
            End Get
            Set(ByVal value As String)
                Me.bankNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankNumber() As String
            Get
                Return Me.bankNumberField
            End Get
            Set(ByVal value As String)
                Me.bankNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankCode() As String
            Get
                Return Me.bankCodeField
            End Get
            Set(ByVal value As String)
                Me.bankCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaTypeCode() As String
            Get
                Return Me.mediaTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.mediaTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaReference() As String
            Get
                Return Me.mediaReferenceField
            End Get
            Set(ByVal value As String)
                Me.mediaReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TheirReference() As String
            Get
                Return Me.theirReferenceField
            End Get
            Set(ByVal value As String)
                Me.theirReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Address() As BaseAddressType
            Get
                Return Me.addressField
            End Get
            Set(ByVal value As BaseAddressType)
                Me.addressField = value
            End Set
        End Property


        '''<remarks/>
        Public Property PartyBankKey() As Integer
            Get
                Return Me.partyBankKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyBankKeyField = value
            End Set
        End Property
        Public Property MediaTypeDesc() As String
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            If Address IsNot Nothing Then
                Address.Validate(oErrorCollection)
            End If

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseClaimPaymentRequestType
        Inherits BaseRequestType

        Private _dataTransferIsUsingFullClaimVersioning As Boolean
        Public Property DataTransferIsUsingFullClaimVersioning() As Boolean
            Get
                Return _dataTransferIsUsingFullClaimVersioning
            End Get
            Set(ByVal value As Boolean)
                _dataTransferIsUsingFullClaimVersioning = value
            End Set
        End Property

        Private _dataTransferClaimHasSpecifiedClaimRiskData As Boolean
        Public Property DataTransferClaimHasClaimRiskDataSpecified() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedClaimRiskData
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedClaimRiskData = value
            End Set
        End Property

        Private claimPaymentField As BaseClaimPaymentType

        Private timeStampField() As Byte

        Public Property GetSavedTaxOfPeril() As Integer

        '''<remarks/>
        Public Property ClaimPayment() As BaseClaimPaymentType
            Get
                Return Me.claimPaymentField
            End Get
            Set(ByVal value As BaseClaimPaymentType)
                Me.claimPaymentField = value
            End Set
        End Property



        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        Private _CLMPAYABLEAccountId As Integer
        Public Property CLMPAYABLEAccountId() As Integer
            Get
                Return _CLMPAYABLEAccountId
            End Get
            Set(ByVal value As Integer)
                _CLMPAYABLEAccountId = value
            End Set
        End Property
        Private _CLMPAYABLECurrencyId As Integer
        Public Property CLMPAYABLECurrencyId() As Integer
            Get
                Return _CLMPAYABLECurrencyId
            End Get
            Set(ByVal value As Integer)
                _CLMPAYABLECurrencyId = value
            End Set
        End Property

        Private _paymentPartyAccountId As Integer
        Public Property PaymentPartyAccountId() As Integer
            Get
                Return _paymentPartyAccountId
            End Get
            Set(ByVal value As Integer)
                _paymentPartyAccountId = value
            End Set
        End Property
        Private _paymentPartyAccountCode As String
        Public Property PaymentPartyAccountCode() As String
            Get
                Return _paymentPartyAccountCode
            End Get
            Set(ByVal value As String)
                _paymentPartyAccountCode = value
            End Set
        End Property

        Private _paymentPartyShortCode As String
        Public Property PaymentPartyShortCode() As String
            Get
                Return _paymentPartyShortCode
            End Get
            Set(ByVal value As String)
                _paymentPartyShortCode = value
            End Set
        End Property

        Private _dataTransferClaim As Boolean = False
        Public Property IsDataTransferClaim() As Boolean
            Get
                Return _dataTransferClaim
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaim = value
            End Set
        End Property



        Private _dataTransferClaimHasSpecifiedReinsurance As Boolean
        Public Property DataTransferClaimHasSpecifiedReinsurance() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedReinsurance
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedReinsurance = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            ClaimPayment.Validate(oErrorCollection)

        End Sub

        Public Property ClaimPerilPayment() As System.Collections.Generic.List(Of BaseClaimPaymentType)
    End Class

    Public Class BaseClaimPaymentType

        Private baseClaimKeyField As Integer

        Private baseClaimPerilKeyField As Integer

        Private partyKeyField As Integer

        Private paymentPartyTypeField As ClaimPaymentPartyTypeType

        Private currencyCodeField As String

        Private claimVersionDescriptionField As String

        Private advancedTaxDetailsField As BaseClaimPaymentAdvancedTaxDetailsType

        Private claimPaymentItemField() As BaseClaimPaymentItemType

        Private payeeField As BaseClaimPayeeType

        Private _PaymentToReinsurer As Boolean

        Private cashListField As BasePaymentCashListType

        Private claimPaymentWorkflowEnabledField As Boolean

        Private closeClaimOnFinalPaymentField As Boolean

        Private closeClaimOnZeroReserveRecoveryBalanceField As Boolean

        Private clientKeyField As Integer

        Private paymentOnlyField As Boolean

        Private baseCasekeyField As Integer

        Private IsExGratiaField As Boolean

        Private ultimatePayeeField As String

        Public Property IsThisPayment() As Boolean

        Public Property UltimatePayee() As String
            Get
                Return Me.ultimatePayeeField
            End Get
            Set(ByVal value As String)
                Me.ultimatePayeeField = value
            End Set
        End Property

        Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
            Get
                Return Me.closeClaimOnZeroReserveRecoveryBalanceField
            End Get
            Set(ByVal value As Boolean)
                Me.closeClaimOnZeroReserveRecoveryBalanceField = value
            End Set
        End Property

        Public Property closeClaimOnFinalPayment() As Boolean
            Get
                Return Me.closeClaimOnFinalPaymentField
            End Get
            Set(ByVal value As Boolean)
                Me.closeClaimOnFinalPaymentField = value
            End Set
        End Property

        ' End (Sriram P)- (15082008 added the missing field as per the gap analysis)

        Public Property ClaimPaymentWorkflowEnabled() As Boolean
            Get
                Return claimPaymentWorkflowEnabledField
            End Get
            Set(ByVal value As Boolean)
                claimPaymentWorkflowEnabledField = value
            End Set
        End Property
        '''<remarks/>
        Public Property CashList() As BasePaymentCashListType
            Get
                Return Me.cashListField
            End Get
            Set(ByVal value As BasePaymentCashListType)
                Me.cashListField = value
            End Set
        End Property

        'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.2.2)
        Public Property PaymentToReinsurer() As Boolean
            Get
                Return _PaymentToReinsurer
            End Get
            Set(ByVal value As Boolean)
                _PaymentToReinsurer = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentOnly() As Boolean
            Get
                Return Me.paymentOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.paymentOnlyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsExGratia() As Boolean
            Get
                Return Me.IsExGratiaField
            End Get
            Set(ByVal value As Boolean)
                Me.IsExGratiaField = value
            End Set
        End Property

        Public Property ViewMode() As Boolean

        Public Property ClaimPaymentTaxItems() As BaseClaimPaymentTaxItemType()

        Public Property SkipTaxItemExpansion() As Boolean

        Public Property ReserveKey() As Integer

        Private _sAllowNegativeReserve As String
        Private _sClaimPaymentAuthority As String
        Private _sClaimPaymentIsGross As String
        Private _lCurrencyId As Integer
        Private _lClaimId As Integer
        Private _lVersionId As Integer
        Private _lLossCurrencyId As Integer
        Private _lClaimPerilId As Integer
        Private _SourceId As Integer
        Private _AdvanceScript As String
        Private _LossXRate As Decimal
        Private _CurrBaseXRate As Decimal
        Private _CurrBaseDate As Date
        Private _AccBaseXRate As Decimal
        Private _AccBaseDate As Date
        Private _SysBaseXRate As Decimal
        Private _SysBaseDate As Date
        Private _exgRateReasonId As Integer

        Private _InsuranceFileCNT As Integer
        Private _PostClaimTaxes As Boolean
        Private _classOfBusinessID As Integer
        Private _classOfBusinessCode As String
        Private _paymentId As Nullable(Of Integer)
        Private _ClaimNumber As String
        Private _TotCurAmount As Decimal
        Private _TotTaxAmount As Decimal
        Private _TotTaxWHTAmount As Decimal
        Private _TotExcessAmount As Nullable(Of Decimal)

        Private _comments As String
        Private _IsReferred As Nullable(Of Integer)
        Private _createdBy As Integer
        Private _SequenceNo As Nullable(Of Integer)
        Private _TreatyId As Nullable(Of Integer)
        Private _ClaimPaymentToId As Nullable(Of Integer)
        Private _ClaimPaymentPartyTo As Nullable(Of Integer)
        Private _IsWhtExempt As Integer
        Private _DateOfPayment As DateTime
        Private _documentId As Nullable(Of Integer)

        Private _transactionDate As Date

        Private ourRefField As String
        Public Property TransactionDate() As Date
            Get
                Return _transactionDate
            End Get
            Set(ByVal value As Date)
                _transactionDate = value
            End Set
        End Property

        Private _TaxIsWithHoldingTax As Boolean
        Public Property TaxIsWithHoldingTax() As Boolean
            Get
                Return _TaxIsWithHoldingTax
            End Get
            Set(ByVal value As Boolean)
                _TaxIsWithHoldingTax = value
            End Set
        End Property

        Private _baseCurrencyId As Integer
        Public Property BaseCurrencyId() As Integer
            Get
                Return _baseCurrencyId
            End Get
            Set(ByVal value As Integer)
                _baseCurrencyId = value
            End Set
        End Property
        Private _baseAmount As Decimal
        Public Property BaseAmount() As Decimal
            Get
                Return _baseAmount
            End Get
            Set(ByVal value As Decimal)
                _baseAmount = value
            End Set
        End Property
        Private _accountCurrencyId As Integer
        Public Property AccountCurrencyId() As Integer
            Get
                Return _accountCurrencyId
            End Get
            Set(ByVal value As Integer)
                _accountCurrencyId = value
            End Set
        End Property
        Private _accountAmount As Decimal
        Public Property AccountAmount() As Decimal
            Get
                Return _accountAmount
            End Get
            Set(ByVal value As Decimal)
                _accountAmount = value
            End Set
        End Property
        Private _SystemCurrencyId As Integer
        Public Property SystemCurrencyId() As Integer
            Get
                Return _SystemCurrencyId
            End Get
            Set(ByVal value As Integer)
                _SystemCurrencyId = value
            End Set
        End Property
        Private _SystemAmount As Decimal
        Public Property SystemAmount() As Decimal
            Get
                Return _SystemAmount
            End Get
            Set(ByVal value As Decimal)
                _SystemAmount = value
            End Set
        End Property


        '''<remarks/>
        Public Property DocumentId() As Nullable(Of Integer)
            Get
                Return _documentId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _documentId = value
            End Set
        End Property


        ' ADD all the properties related to exchange rates
        '''<remarks/>
        Public Property AllowNegativeReserve() As String
            Get
                Return Me._sAllowNegativeReserve
            End Get
            Set(ByVal value As String)
                Me._sAllowNegativeReserve = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentAuthority() As String
            Get
                Return Me._sClaimPaymentAuthority
            End Get
            Set(ByVal value As String)
                Me._sClaimPaymentAuthority = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentIsGross() As String
            Get
                Return Me._sClaimPaymentIsGross
            End Get
            Set(ByVal value As String)
                Me._sClaimPaymentIsGross = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyId() As Integer
            Get
                Return Me._lCurrencyId
            End Get
            Set(ByVal value As Integer)
                Me._lCurrencyId = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimId() As Integer
            Get
                Return Me._lClaimId
            End Get
            Set(ByVal value As Integer)
                Me._lClaimId = value
            End Set
        End Property

        '''<remarks/>
        Public Property VersionId() As Integer
            Get
                Return Me._lVersionId
            End Get
            Set(ByVal value As Integer)
                Me._lVersionId = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossCurrencyId() As Integer
            Get
                Return Me._lLossCurrencyId
            End Get
            Set(ByVal value As Integer)
                Me._lLossCurrencyId = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPerilId() As Integer
            Get
                Return Me._lClaimPerilId
            End Get
            Set(ByVal value As Integer)
                Me._lClaimPerilId = value
            End Set
        End Property
        '''<remarks/>
        Public Property SourceId() As Integer
            Get
                Return Me._SourceId
            End Get
            Set(ByVal value As Integer)
                Me._SourceId = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdvanceScript() As String
            Get
                Return Me._AdvanceScript
            End Get
            Set(ByVal value As String)
                Me._AdvanceScript = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentToLossXRate() As Decimal
            Get
                Return Me._LossXRate
            End Get
            Set(ByVal value As Decimal)
                Me._LossXRate = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyToBaseXRate() As Decimal
            Get
                Return Me._CurrBaseXRate
            End Get
            Set(ByVal value As Decimal)
                Me._CurrBaseXRate = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyToBaseDate() As Date
            Get
                Return Me._CurrBaseDate
            End Get
            Set(ByVal value As Date)
                Me._CurrBaseDate = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountToBaseXRate() As Decimal
            Get
                Return Me._AccBaseXRate
            End Get
            Set(ByVal value As Decimal)
                Me._AccBaseXRate = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountToBaseDate() As Date
            Get
                Return Me._AccBaseDate
            End Get
            Set(ByVal value As Date)
                Me._AccBaseDate = value
            End Set
        End Property

        '''<remarks/>
        Public Property SystemToBaseXRate() As Decimal
            Get
                Return Me._SysBaseXRate
            End Get
            Set(ByVal value As Decimal)
                Me._SysBaseXRate = value
            End Set
        End Property

        '''<remarks/>
        Public Property SystemToBaseDate() As Date
            Get
                Return Me._SysBaseDate
            End Get
            Set(ByVal value As Date)
                Me._SysBaseDate = value
            End Set
        End Property

        '''<remarks/>
        Public Property ExgRateReasonId() As Integer
            Get
                Return Me._exgRateReasonId
            End Get
            Set(ByVal value As Integer)
                Me._exgRateReasonId = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileCNT() As Integer
            Get
                Return Me._InsuranceFileCNT
            End Get
            Set(ByVal value As Integer)
                Me._InsuranceFileCNT = value
            End Set
        End Property

        '''<remarks/>
        Public Property PostClaimTaxesSeperately() As Boolean
            Get
                Return Me._PostClaimTaxes
            End Get
            Set(ByVal value As Boolean)
                Me._PostClaimTaxes = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClassOfBusinessID() As Integer
            Get
                Return Me._classOfBusinessID
            End Get
            Set(ByVal value As Integer)
                Me._classOfBusinessID = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClassOfBusinessCode() As String
            Get
                Return Me._classOfBusinessCode
            End Get
            Set(ByVal value As String)
                Me._classOfBusinessCode = value
            End Set
        End Property

        Public Property PaymentId() As Nullable(Of Integer)
            Get
                Return Me._paymentId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._paymentId = value
            End Set
        End Property

        Public Property ClaimNumber() As String
            Get
                Return Me._ClaimNumber
            End Get
            Set(ByVal value As String)
                Me._ClaimNumber = value
            End Set
        End Property

        Public Property TotCurrAmount() As Decimal
            Get
                Return Me._TotCurAmount
            End Get
            Set(ByVal value As Decimal)
                Me._TotCurAmount = value
            End Set
        End Property

        Public Property TotTaxAmount() As Decimal
            Get
                Return Me._TotTaxAmount
            End Get
            Set(ByVal value As Decimal)
                Me._TotTaxAmount = value
            End Set
        End Property

        Public Property TotTaxWHTAmount() As Decimal
            Get
                Return Me._TotTaxWHTAmount
            End Get
            Set(ByVal value As Decimal)
                Me._TotTaxWHTAmount = value
            End Set
        End Property

        Public Property TotExcessAmount() As Nullable(Of Decimal)
            Get
                Return Me._TotExcessAmount
            End Get
            Set(ByVal value As Nullable(Of Decimal))
                Me._TotExcessAmount = value
            End Set
        End Property

        '''<remarks/>
        Public Property Comments() As String
            Get
                Return Me._comments
            End Get
            Set(ByVal value As String)
                Me._comments = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsReferred() As Nullable(Of Integer)
            Get
                Return Me._IsReferred
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._IsReferred = value
            End Set
        End Property

        '''<remarks/>
        Public Property CreatedBy() As Integer
            Get
                Return Me._createdBy
            End Get
            Set(ByVal value As Integer)
                Me._createdBy = value
            End Set
        End Property

        '''<remarks/>
        Public Property SequenceNo() As Nullable(Of Integer)
            Get
                Return Me._SequenceNo
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._SequenceNo = value
            End Set
        End Property

        '''<remarks/>
        Public Property TreatyId() As Nullable(Of Integer)
            Get
                Return Me._TreatyId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._TreatyId = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentToId() As Nullable(Of Integer)
            Get
                Return Me._ClaimPaymentToId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._ClaimPaymentToId = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentPartyTo() As Nullable(Of Integer)
            Get
                Return Me._ClaimPaymentPartyTo
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._ClaimPaymentPartyTo = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsWhtExempt() As Integer
            Get
                Return Me._IsWhtExempt
            End Get
            Set(ByVal value As Integer)
                Me._IsWhtExempt = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateOfPayment() As DateTime
            Get
                Return Me._DateOfPayment
            End Get
            Set(ByVal value As DateTime)
                Me._DateOfPayment = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimKey() As Integer
            Get
                Return Me.baseClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimPerilKey() As Integer
            Get
                Return Me.baseClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimPerilKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentPartyType() As ClaimPaymentPartyTypeType
            Get
                Return Me.paymentPartyTypeField
            End Get
            Set(ByVal value As ClaimPaymentPartyTypeType)
                Me.paymentPartyTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimVersionDescription() As String
            Get
                Return Me.claimVersionDescriptionField
            End Get
            Set(ByVal value As String)
                Me.claimVersionDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdvancedTaxDetails() As BaseClaimPaymentAdvancedTaxDetailsType
            Get
                Return Me.advancedTaxDetailsField
            End Get
            Set(ByVal value As BaseClaimPaymentAdvancedTaxDetailsType)
                Me.advancedTaxDetailsField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("ClaimPaymentItem")>
        Public Property ClaimPaymentItem() As BaseClaimPaymentItemType()
            Get
                Return Me.claimPaymentItemField
            End Get
            Set(ByVal value As BaseClaimPaymentItemType())
                Me.claimPaymentItemField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Payee() As BaseClaimPayeeType
            Get
                Return Me.payeeField
            End Get
            Set(ByVal value As BaseClaimPayeeType)
                Me.payeeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientKey() As Integer
            Get
                Return Me.clientKeyField
            End Get
            Set(ByVal value As Integer)
                Me.clientKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseCaseKey() As Integer
            Get
                Return Me.baseCasekeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseCasekeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OurRef() As String
            Get
                Return Me.ourRefField
            End Get
            Set(ByVal value As String)
                Me.ourRefField = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            If BaseClaimKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "BaseClaimKey")
            End If

            If BaseClaimPerilKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "BaseClaimPerilKey")
            End If

            ' the party cnt is mandatory only if the specified payment party type is PARTY
            If PaymentPartyType = ClaimPaymentPartyTypeType.PARTY AndAlso PartyKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "PartyKey")
            End If

            ' the party cnt should always be zero where the payment party type is not PARTY
            If PaymentPartyType <> ClaimPaymentPartyTypeType.PARTY AndAlso PartyKey <> 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.SpecifiedPartyTypeShouldHaveAPartyKeyofZero,
                                                    SAMConstants.SAMInvalidData.SpecifiedPartyTypeShouldHaveAPartyKeyofZero.ToString,
                                                    "PartyKey")
            End If

            If String.IsNullOrEmpty(CurrencyCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "CurrencyCode")
            End If

            'Start Praveen - Bug ID - 59
            If ClaimPaymentItem IsNot Nothing Then
                'End Praveen - Bug ID - 59
                For itemCounts As Integer = 0 To ClaimPaymentItem.GetUpperBound(0)
                    ClaimPaymentItem(itemCounts).Validate(oErrorCollection)
                Next
                'Start Praveen - Bug ID - 59
            End If
            'End Praveen - Bug ID - 59

            'Start (Arul Stephen)-(Optimization)
            If (Payee IsNot Nothing) Then
                'End (Arul Stephen)-(Optimization)
                Payee.Validate(oErrorCollection)
                'Start (Arul Stephen)-(Optimization)
            End If
            'End (Arul Stephen)-(Optimization)
            'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.2.2)
            If (CashList IsNot Nothing) Then
                If (CashList.isValidated = False) Then
                    CashList.Validate(oErrorCollection)
                End If
            End If
            'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.2.2)

        End Sub

    End Class


    '''<remarks/>
    Public Class BaseClaimPaymentAdvancedTaxDetailsType

        Private insuredDomiciledField As Boolean

        Private insuredPercentageField As Decimal

        Private insuranceTaxNumberField As String

        Private payeeDomiciledField As Boolean

        Private payeePercentageField As Decimal

        Private payeeTaxNumberField As String

        Private safeHarbourCodeField As String

        Private safeHarbourPercentageField As Nullable(Of Decimal)

        Private isTaxExemptField As Boolean

        Private isWHTExemptField As Boolean

        Private isSettlementField As Boolean

        Private _SafeHarbourID As Nullable(Of Integer)  'GAURAV

        Private sPaymentToCodeField As String
        '''<remarks/>
        Public Property InsuredDomiciled() As Boolean
            Get
                Return Me.insuredDomiciledField
            End Get
            Set(ByVal value As Boolean)
                Me.insuredDomiciledField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuredPercentage() As Decimal
            Get
                Return Me.insuredPercentageField
            End Get
            Set(ByVal value As Decimal)
                Me.insuredPercentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceTaxNumber() As String
            Get
                Return Me.insuranceTaxNumberField
            End Get
            Set(ByVal value As String)
                Me.insuranceTaxNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeeDomiciled() As Boolean
            Get
                Return Me.payeeDomiciledField
            End Get
            Set(ByVal value As Boolean)
                Me.payeeDomiciledField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeePercentage() As Decimal
            Get
                Return Me.payeePercentageField
            End Get
            Set(ByVal value As Decimal)
                Me.payeePercentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeeTaxNumber() As String
            Get
                Return Me.payeeTaxNumberField
            End Get
            Set(ByVal value As String)
                Me.payeeTaxNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SafeHarbourCode() As String
            Get
                Return Me.safeHarbourCodeField
            End Get
            Set(ByVal value As String)
                Me.safeHarbourCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SafeHarbourID() As Nullable(Of Integer)   'GAURAV
            Get
                Return Me._SafeHarbourID
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._SafeHarbourID = value
            End Set
        End Property

        '''<remarks/>
        Public Property SafeHarbourPercentage() As Nullable(Of Decimal)
            Get
                Return Me.safeHarbourPercentageField
            End Get
            Set(ByVal value As Nullable(Of Decimal))
                Me.safeHarbourPercentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsTaxExempt() As Boolean
            Get
                Return Me.isTaxExemptField
            End Get
            Set(ByVal value As Boolean)
                Me.isTaxExemptField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsWHTExempt() As Boolean
            Get
                Return Me.isWHTExemptField
            End Get
            Set(ByVal value As Boolean)
                Me.isWHTExemptField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsSettlement() As Boolean
            Get
                Return Me.isSettlementField
            End Get
            Set(ByVal value As Boolean)
                Me.isSettlementField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentToCode() As String
            Get
                Return Me.sPaymentToCodeField
            End Get
            Set(ByVal value As String)
                Me.sPaymentToCodeField = value
            End Set
        End Property

        Public Property PayeeName() As String

        Public Property IsExcess() As Boolean

        Public Property AdvancedTaxScriptOptionOn() As Boolean

        Public Property PaymentTo() As String
    End Class

    '''<remarks/>
    Public Class BaseClaimPaymentItemType

        Private baseReserveKeyField As Integer

        Private taxGroupCodeField As String

        Private paymentAmountField As Decimal

        Private reverseExcessField As Boolean

        Private _taxGroupId As Nullable(Of Integer)
        Private _IsWHTTax As Boolean
        Private _TaxAmount As Decimal
        Private _ReserveId As Nullable(Of Integer)
        Private _IsExcessField As Boolean
        Private _CurrencyAmount As Decimal
        Private _LCurrencyAmount As Decimal
        Private _CurrencyTax As Decimal
        Private _LCurrencyTax As Decimal
        Private _CurrentReserve As Decimal
        Private _RevisionAmount As Decimal
        Private _TaxAmountWHT As Decimal
        Private _LCurrencyTaxWHT As Decimal
        Private _ExcessAmount As Decimal
        Private _RecoveryId As Nullable(Of Integer)
        Private _RecoveryTypeId As Nullable(Of Integer)

        Private _claimPaymentItemId As Integer
        Private _advancedTaxDetails As Object(,)

        Public Property IsTaxOverridden() As Boolean

        Public Property OverriddedTaxAmount() As Decimal

        Public Property ClaimPaymentItemId() As Integer
            Get
                Return _claimPaymentItemId
            End Get
            Set(ByVal value As Integer)
                _claimPaymentItemId = value
            End Set
        End Property

        Private _PaymentAdjustment As Decimal
        '''<remarks/>
        Public Property PaymentAdjustment() As Decimal
            Get
                Return _PaymentAdjustment
            End Get
            Set(ByVal value As Decimal)
                _PaymentAdjustment = value
            End Set
        End Property

        '''<remarks/>
        Public Property LCurrencyTaxWHT() As Decimal
            Get
                Return Me._LCurrencyTaxWHT
            End Get
            Set(ByVal value As Decimal)
                Me._LCurrencyTaxWHT = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryId() As Nullable(Of Integer)
            Get
                Return _RecoveryId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _RecoveryId = value
            End Set
        End Property
        '''<remarks/>
        Public Property RecoveryTypeId() As Nullable(Of Integer)
            Get
                Return _RecoveryTypeId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _RecoveryTypeId = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxGroupId() As Nullable(Of Integer)
            Get
                Return Me._taxGroupId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._taxGroupId = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsWHTTax() As Boolean
            Get
                Return Me._IsWHTTax
            End Get
            Set(ByVal value As Boolean)
                Me._IsWHTTax = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmount() As Decimal
            Get
                Return Me._TaxAmount
            End Get
            Set(ByVal value As Decimal)
                Me._TaxAmount = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReserveId() As Nullable(Of Integer)
            Get
                Return Me._ReserveId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._ReserveId = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsExcess() As Boolean

            Get
                Return Me._IsExcessField
            End Get
            Set(ByVal value As Boolean)
                Me._IsExcessField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyAmount() As Decimal
            Get
                Return Me._CurrencyAmount
            End Get
            Set(ByVal value As Decimal)
                Me._CurrencyAmount = value
            End Set
        End Property

        '''<remarks/>
        Public Property LCurrencyAmount() As Decimal
            Get
                Return Me._LCurrencyAmount
            End Get
            Set(ByVal value As Decimal)
                Me._LCurrencyAmount = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyTax() As Decimal
            Get
                Return Me._CurrencyTax
            End Get
            Set(ByVal value As Decimal)
                Me._CurrencyTax = value
            End Set
        End Property

        '''<remarks/>
        Public Property LCurrencyTax() As Decimal
            Get
                Return Me._LCurrencyTax
            End Get
            Set(ByVal value As Decimal)
                Me._LCurrencyTax = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrentReserve() As Decimal
            Get
                Return Me._CurrentReserve
            End Get
            Set(ByVal value As Decimal)
                Me._CurrentReserve = value
            End Set
        End Property

        '''<remarks/>
        Public Property RevisionAmount() As Decimal
            Get
                Return Me._RevisionAmount
            End Get
            Set(ByVal value As Decimal)
                Me._RevisionAmount = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmountWHT() As Decimal
            Get
                Return Me._TaxAmountWHT
            End Get
            Set(ByVal value As Decimal)
                Me._TaxAmountWHT = value
            End Set
        End Property

        '''<remarks/>
        Public Property ExcessAmount() As Decimal
            Get
                Return Me._ExcessAmount
            End Get
            Set(ByVal value As Decimal)
                Me._ExcessAmount = value
            End Set
        End Property

        Public Sub New()
            MyBase.New()
            Me.reverseExcessField = False
        End Sub

        '''<remarks/>
        Public Property BaseReserveKey() As Integer
            Get
                Return Me.baseReserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseReserveKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentAmount() As Decimal
            Get
                Return Me.paymentAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReverseExcess() As Boolean
            Get
                Return Me.reverseExcessField
            End Get
            Set(ByVal value As Boolean)
                Me.reverseExcessField = value
            End Set
        End Property

        Public Property AdvancedTaxDetails() As Object(,)
            Get
                Return Me._advancedTaxDetails
            End Get
            Set(ByVal value As Object(,))
                Me._advancedTaxDetails = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If BaseReserveKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "BaseReserveKey")
            End If

        End Sub

    End Class
    '''<remarks/>
    Public Class BaseClaimOpenRequestType
        Inherits BaseRequestType
        Private duplicateClaimOverrideUserNameField As String
        Private duplicateClaimOverrideUserPasswordField As String
        Private _dataTransferIsUsingFullClaimVersioning As Boolean
        Private sUserName As String
        Private bIsRoundingUpToFourField As Boolean
        Public Property DataTransferIsUsingFullClaimVersioning() As Boolean
            Get
                Return _dataTransferIsUsingFullClaimVersioning
            End Get
            Set(ByVal value As Boolean)
                _dataTransferIsUsingFullClaimVersioning = value
            End Set
        End Property

        Private _dataTransferClaim As Boolean = False
        Public Property IsDataTransferClaim() As Boolean
            Get
                Return _dataTransferClaim
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaim = value
            End Set
        End Property

        Private _dataTransferClaimHasSpecifiedReinsurance As Boolean
        Public Property DataTransferClaimHasSpecifiedReinsurance() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedReinsurance
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedReinsurance = value
            End Set
        End Property

        Public Property IsRoundingUpToFour() As Boolean
            Get
                Return bIsRoundingUpToFourField
            End Get
            Set(ByVal value As Boolean)
                bIsRoundingUpToFourField = value
            End Set
        End Property

        Private claimField As BaseClaimOpenType

        '''<remarks/>
        Public Property Claim() As BaseClaimOpenType
            Get
                Return Me.claimField
            End Get
            Set(ByVal value As BaseClaimOpenType)
                Me.claimField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DuplicateClaimOverrideUserName() As String
            Get
                Return Me.duplicateClaimOverrideUserNameField
            End Get
            Set(ByVal value As String)
                Me.duplicateClaimOverrideUserNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DuplicateClaimOverrideUserPassword() As String
            Get
                Return Me.duplicateClaimOverrideUserPasswordField
            End Get
            Set(ByVal value As String)
                Me.duplicateClaimOverrideUserPasswordField = value
            End Set
        End Property
        Public Property UserName() As String
            Get
                Return Me.sUserName
            End Get
            Set(ByVal value As String)
                Me.sUserName = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)

            Claim.Validate(oErrorCollection)

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseClaimOpenType
        Inherits BaseClaimType

        Private claimPerilField() As BaseClaimPerilType

        Private underwritingYearCodeField As String

        Private ignoreWarningsField As Boolean

        '''<summary>
        '''Tech Spec - UIIC WR24 - OpenClaim - Duplicate Claims Check,6.1.3.1.3
        '''</summary>
        Private duplicateClaimOverrideUserNameField As String
        Private duplicateClaimOverrideUserPasswordField As String
        '''<remarks/>
        <XmlElement("ClaimPeril")>
        Public Property ClaimPeril() As BaseClaimPerilType()
            Get
                Return Me.claimPerilField
            End Get
            Set(ByVal value As BaseClaimPerilType())
                Me.claimPerilField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UnderwritingYearCode() As String
            Get
                Return Me.underwritingYearCodeField
            End Get
            Set(ByVal value As String)
                Me.underwritingYearCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IgnoreWarnings() As Boolean
            Get
                Return Me.ignoreWarningsField
            End Get
            Set(ByVal value As Boolean)
                Me.ignoreWarningsField = value
            End Set
        End Property
        '''<summary>
        '''Tech Spec - UIIC WR24 - OpenClaim - Duplicate Claims Check,6.1.3.1.3
        '''</summary>
        Public Property DuplicateClaimOverrideUserName() As String
            Get
                Return Me.duplicateClaimOverrideUserNameField
            End Get
            Set(ByVal value As String)
                Me.duplicateClaimOverrideUserNameField = value
            End Set
        End Property

        '''<summary>
        '''Tech Spec - UIIC WR24 - OpenClaim - Duplicate Claims Check,6.1.3.1.3
        '''</summary>
        Public Property DuplicateClaimOverrideUserPassword() As String
            Get
                Return Me.duplicateClaimOverrideUserPasswordField
            End Get
            Set(ByVal value As String)
                Me.duplicateClaimOverrideUserPasswordField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef ErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(ErrorCollection, SAMErrorCollection)
            Dim oClaimPeril As BaseClaimPerilType

            MyBase.Validate(CObj(oSAMErrorCollection))

            If ClaimPeril IsNot Nothing Then
                For Each oClaimPeril In ClaimPeril
                    oClaimPeril.Validate(CObj(oSAMErrorCollection))
                Next
            End If

            If RiskKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "RiskKey")
            End If

            If InsuranceFileKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "InsuranceFileKey")
            End If

        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimPerilType

        Private _samStagingClaimPerilKey As Integer
        Public Property SamStagingClaimPerilKey() As Integer
            Get
                Return _samStagingClaimPerilKey
            End Get
            Set(ByVal value As Integer)
                _samStagingClaimPerilKey = value
            End Set
        End Property

        Private typeCodeField As String

        Private descriptionField As String

        Private reserveField() As BaseClaimPerilReserveType

        Private recoveryField() As BaseClaimPerilRecoveryType

        Private _classOfBusinessId As Integer

        Private _gisScreenId As Integer

        Public Property GisScreenId() As Integer
            Get
                Return _gisScreenId
            End Get
            Set(ByVal value As Integer)
                _gisScreenId = value
            End Set
        End Property

        Public Property ClassOfBusinessId() As Integer
            Get
                Return _classOfBusinessId
            End Get
            Set(ByVal value As Integer)
                _classOfBusinessId = value
            End Set
        End Property

        Private _classOfBusinessCode As String

        Public Property ClassOfBusinessCode() As String
            Get
                Return _classOfBusinessCode
            End Get
            Set(ByVal value As String)
                _classOfBusinessCode = value
            End Set
        End Property

        Private _transactionAmount As Decimal

        Public Property TransactionAmount() As Decimal
            Get
                Return _transactionAmount
            End Get
            Set(ByVal value As Decimal)
                _transactionAmount = value
            End Set
        End Property

        Private _claimPerilId As Integer

        Public Property ClaimPerilId() As Integer
            Get
                Return _claimPerilId
            End Get
            Set(ByVal value As Integer)
                _claimPerilId = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Reserve() As BaseClaimPerilReserveType()
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As BaseClaimPerilReserveType())
                Me.reserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Recovery() As BaseClaimPerilRecoveryType()
            Get
                Return Me.recoveryField
            End Get
            Set(ByVal value As BaseClaimPerilRecoveryType())
                Me.recoveryField = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(TypeCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ClaimPerilTypeCode")
            End If

            If Recovery IsNot Nothing Then
                ' validate recoveries
                Dim oRecovery As BaseClaimPerilRecoveryType
                For Each oRecovery In Recovery
                    oRecovery.Validate(CObj(oSAMErrorCollection))
                Next
            End If

            If Reserve IsNot Nothing Then
                ' validate reserves
                Dim oReserve As BaseClaimPerilReserveType
                For Each oReserve In Reserve
                    oReserve.Validate(CObj(oSAMErrorCollection))
                Next
            End If

        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimPerilReserveType

        Private typeCodeField As String

        Private revisionAmountField As Decimal

        Private _reserveId As Integer

        Public Property ReserveId() As Integer
            Get
                Return _reserveId
            End Get
            Set(ByVal value As Integer)
                _reserveId = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RevisionAmount() As Decimal
            Get
                Return Me.revisionAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.revisionAmountField = value
            End Set
        End Property

        Private _samStagingReserveKey As Integer
        Public Property SamStagingReserveKey() As Integer
            Get
                Return _samStagingReserveKey
            End Get
            Set(ByVal value As Integer)
                _samStagingReserveKey = value
            End Set
        End Property

        Public Property GrossReserve() As Decimal

        Public Property Tax() As Decimal

        Public Property RevisedGrossReserve() As Decimal

        Public Property RevisedTaxReserve() As Decimal

        Public Property PaidToDateTax() As Decimal

        Public Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If TypeCode.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ReserveTypeCode")
            End If

            If RevisionAmount.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "ReserveRevisionAmount")
            End If
        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimPerilRecoveryType

        Private _samStagingRecoveryKey As Integer
        'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(7.1.1)
        Private recoveryPartyTypeKeyField As Integer

        Private recoveryPartyTypeKeyFieldSpecified As Boolean

        Private recoveryPartyTypeCodeField As String

        Private recoveryPartyCodeField As String

        Private recoveryPartyKeyField As Integer

        Private recoveryPartyKeyFieldSpecified As Boolean

        Private isNewField As Boolean

        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(7.1.1)
        Public Property SamStagingRecoveryKey() As Integer
            Get
                Return _samStagingRecoveryKey
            End Get
            Set(ByVal value As Integer)
                _samStagingRecoveryKey = value
            End Set
        End Property

        Private typeCodeField As String

        Private revisionAmountField As Decimal

        Private _recoveryTypeId As Integer

        Private initialAmountField As Decimal

        Private isDeletedRecoveryField As Boolean

        Private baseRecoveryKeyField As Integer

        Public Property Initialamount() As Decimal
            Get
                Return Me.initialAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.initialAmountField = value
            End Set
        End Property

        Public Property IsDeletedRecovery() As Boolean
            Get
                Return Me.isDeletedRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.isDeletedRecoveryField = value
            End Set
        End Property
        Public Property BaseRecoveryKey() As Integer
            Get
                Return baseRecoveryKeyField
            End Get
            Set(ByVal value As Integer)
                baseRecoveryKeyField = value
            End Set
        End Property
        Public Property RecoveryTypeId() As Integer
            Get
                Return _recoveryTypeId
            End Get
            Set(ByVal value As Integer)
                _recoveryTypeId = value
            End Set
        End Property

        Private _recoveryId As Integer

        Public Property RecoveryId() As Integer
            Get
                Return _recoveryId
            End Get
            Set(ByVal value As Integer)
                _recoveryId = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RevisionAmount() As Decimal
            Get
                Return Me.revisionAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.revisionAmountField = value
            End Set
        End Property
        'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(7.1.1)
        '''<remarks/>
        Public Property RecoveryPartyTypeCode() As String
            Get
                Return Me.recoveryPartyTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryPartyTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryPartyCode() As String
            Get
                Return Me.recoveryPartyCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryPartyCodeField = value
            End Set
        End Property

        Public Property RecoveryPartyTypeKey() As Integer
            Get
                Return Me.recoveryPartyTypeKeyField
            End Get
            Set(ByVal value As Integer)
                Me.recoveryPartyTypeKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoveryPartyTypeKeySpecified() As Boolean
            Get
                Return Me.recoveryPartyTypeKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoveryPartyTypeKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryPartyKey() As Integer
            Get
                Return Me.recoveryPartyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.recoveryPartyKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoveryPartyKeySpecified() As Boolean
            Get
                Return Me.recoveryPartyKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoveryPartyKeyFieldSpecified = value
            End Set
        End Property

        Public Property IsNew() As Boolean
            Get
                Return Me.isNewField
            End Get
            Set(ByVal value As Boolean)
                Me.isNewField = value
            End Set
        End Property

        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(7.1.1)
        Public Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(TypeCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "RecoveryTypeCode")
            End If

            If RevisionAmount.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "RecoveryRevisionAmount")
            End If

        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimType

        Private clientField As BaseClaimPartyClientType

        Private insurerField As BaseClaimPartyInsurerType

        Private descriptionField As String

        Private progressStatusCodeField As String

        Private primaryCauseCodeField As String

        Private lossFromDateField As Date

        Private reportedDateField As Date

        Private handlerCodeField As String

        Private infoOnlyField As Boolean

        Private likelyClaimField As Boolean

        Private secondaryCauseCodeField As String

        Private catastropheCodeField As String

        Private lossToDateField As Date

        Private lossToDateFieldSpecified As Boolean

        Private locationField As String

        Private townCodeField As String

        Private userDefFldACodeField As String

        Private userDefFldBCodeField As String

        Private userDefFldCCodeField As String

        Private userDefFldDCodeField As String

        Private userDefFldECodeField As String

        Private commentsField As String

        Private claimVersionDescriptionField As String

        Private _InsuranceFileKey As Integer
        ' Start (Vijayakumar Ramasamy)- (Generate Documents Claim)-(15-Sep-2008)
        Private claimVersionField As Integer
        Private claimStatusField As String
        Private claimStatusDateField As Date
        Private lastModifiedDateField As Date
        Private tpaField As Integer
        Private baseCaseKeyField As Integer
        Private bIsPolicyOutstanding As Boolean

        Public Property TPA() As Integer
            Get
                Return Me.tpaField
            End Get
            Set(ByVal value As Integer)
                Me.tpaField = value
            End Set
        End Property
        Public Property ClaimVersion() As Integer
            Get
                Return Me.claimVersionField
            End Get
            Set(ByVal value As Integer)
                Me.claimVersionField = value
            End Set
        End Property
        '''<remarks/>
        Public Property ClaimStatus() As String
            Get
                Return Me.claimStatusField
            End Get
            Set(ByVal value As String)
                Me.claimStatusField = value
            End Set
        End Property
        '''<remarks/>
        Public Property ClaimStatusDate() As Date
            Get
                Return Me.claimStatusDateField
            End Get
            Set(ByVal value As Date)
                Me.claimStatusDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LastModifiedDate() As Date
            Get
                Return Me.lastModifiedDateField
            End Get
            Set(ByVal value As Date)
                Me.lastModifiedDateField = value
            End Set
        End Property
        ' End (Vijayakumar Ramasamy)- (Generate Documents Claim)-(15-Sep-2008)

        Public Property InsuranceFileKey() As Integer
            Get
                Return _InsuranceFileKey
            End Get
            Set(ByVal value As Integer)
                _InsuranceFileKey = value
            End Set
        End Property

        Private _RiskKey As Integer
        Public Property RiskKey() As Integer
            Get
                Return _RiskKey
            End Get
            Set(ByVal value As Integer)
                _RiskKey = value
            End Set
        End Property

        Private reservesOnlyField As Boolean
        Public Property ReserveOnly() As Boolean
            Get
                Return Me.reservesOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.reservesOnlyField = value
            End Set
        End Property


        Private _sourceId As Integer
        Private _progressStatusId As Integer
        Private _underwritingYearId As Nullable(Of Integer)
        Private _handlerId As Integer
        Private _catastropheId As Nullable(Of Integer)
        Private _townId As Nullable(Of Integer)
        Private _clientAddressTypeId As Integer
        Private _insurerAddressTypeId As Integer
        Private _primaryCauseId As Integer
        Private _secondaryCauseId As Integer
        Private _userDefFldATableCode As String
        Private _userDefFldBTableCode As String
        Private _userDefFldCTableCode As String
        Private _userDefFldDTableCode As String
        Private _userDefFldETableCode As String
        Private _userDefFldAId As Nullable(Of Integer)
        Private _userDefFldBId As Nullable(Of Integer)
        Private _userDefFldCId As Nullable(Of Integer)
        Private _userDefFldDId As Nullable(Of Integer)
        Private _userDefFldEId As Nullable(Of Integer)
        Private _policyNumber As String
        Private _claimNumber As String
        Private _clientAddressId As Integer
        Private _insurerAddressId As Integer
        Private _clientTelNo As String
        Private _clientFaxNo As String
        Private _clientMobileNo As String
        Private _clientEmail As String
        Private _clientTelNoOff As String
        Private _insurerTelNo As String
        Private _insurerFaxNo As String
        Private _insurerEmail As String
        Private _clientShortName As String
        Private _insurerShortName As String
        Private _claimStatusId As Integer
        Private _clientOtherContacts As String
        Private _insurerOtherContacts As String

        Private _InsurerMobileNo As String
        Private _InsurerWebNo As String
        Private _ClientWebNo As String
        Private _samStagingClaimKey As Integer
        Private insurerContactField As String

        '''<remarks/>
        Public Property InsurerContact() As String
            Get
                Return Me.insurerContactField
            End Get
            Set(ByVal value As String)
                Me.insurerContactField = value
            End Set
        End Property


        Public Property SamStagingClaimKey() As Integer
            Get
                Return _samStagingClaimKey
            End Get
            Set(ByVal value As Integer)
                _samStagingClaimKey = value
            End Set
        End Property

        Private _insurerName As String
        Public Property InsurerName() As String
            Get
                Return _insurerName
            End Get
            Set(ByVal value As String)
                _insurerName = value
            End Set
        End Property

        Private _clientName As String
        Public Property ClientName() As String
            Get
                Return _clientName
            End Get
            Set(ByVal value As String)
                _clientName = value
            End Set
        End Property

        Private _previousSecondaryCauseId As Integer
        Public Property PreviousSecondaryCauseId() As Integer
            Get
                Return _previousSecondaryCauseId
            End Get
            Set(ByVal value As Integer)
                _previousSecondaryCauseId = value
            End Set
        End Property

        Private _previousCatastropheCodeid As Integer
        Public Property PreviousCatastropheCodeid() As Integer
            Get
                Return _previousCatastropheCodeid
            End Get
            Set(ByVal value As Integer)
                _previousCatastropheCodeid = value
            End Set
        End Property

        Private _previousLocation As String
        Public Property PreviousLocation() As String
            Get
                Return _previousLocation
            End Get
            Set(ByVal value As String)
                _previousLocation = value
            End Set
        End Property

        Private _previousTownId As Integer
        Public Property PreviousTownId() As Integer
            Get
                Return _previousTownId
            End Get
            Set(ByVal value As Integer)
                _previousTownId = value
            End Set
        End Property

        Private _previousClientTelNo As String
        Public Property PreviousClientTelNo() As String
            Get
                Return _previousClientTelNo
            End Get
            Set(ByVal value As String)
                _previousClientTelNo = value
            End Set
        End Property

        Private _previousClientFaxNo As String
        Public Property PreviousClientFaxNo() As String
            Get
                Return _previousClientFaxNo
            End Get
            Set(ByVal value As String)
                _previousClientFaxNo = value
            End Set
        End Property
        Private _previousClientMobileNo As String
        Public Property PreviousClientMobileNo() As String
            Get
                Return _previousClientMobileNo
            End Get
            Set(ByVal value As String)
                _previousClientMobileNo = value
            End Set
        End Property

        Private _previousClientEmail As String
        Public Property PreviousClientEmail() As String
            Get
                Return _previousClientEmail
            End Get
            Set(ByVal value As String)
                _previousClientEmail = value
            End Set
        End Property

        Private _previousClientClaimNumber As String
        Public Property PreviousClientClaimNumber() As String
            Get
                Return _previousClientClaimNumber
            End Get
            Set(ByVal value As String)
                _previousClientClaimNumber = value
            End Set
        End Property

        Private _previousInsurerTelNo As String
        Public Property PreviousInsurerTelNo() As String
            Get
                Return _previousInsurerTelNo
            End Get
            Set(ByVal value As String)
                _previousInsurerTelNo = value
            End Set
        End Property

        Private _previousInsurerFaxNo As String
        Public Property PreviousInsurerFaxNo() As String
            Get
                Return _previousInsurerFaxNo
            End Get
            Set(ByVal value As String)
                _previousInsurerFaxNo = value
            End Set
        End Property

        Private _previousInsurerEmail As String
        Public Property PreviousInsurerEmail() As String
            Get
                Return _previousInsurerEmail
            End Get
            Set(ByVal value As String)
                _previousInsurerEmail = value
            End Set
        End Property

        Private _previousInsurerClaimNumber As String
        Public Property PreviousInsurerClaimNumber() As String
            Get
                Return _previousInsurerClaimNumber
            End Get
            Set(ByVal value As String)
                _previousInsurerClaimNumber = value
            End Set
        End Property

        Private _previousInsurerContact As String
        Public Property PreviousInsurerContact() As String
            Get
                Return _previousInsurerContact
            End Get
            Set(ByVal value As String)
                _previousInsurerContact = value
            End Set
        End Property

        Private _previousInsurerOtherContacts As String
        Public Property PreviousInsurerOtherContacts() As String
            Get
                Return _previousInsurerOtherContacts
            End Get
            Set(ByVal value As String)
                _previousInsurerOtherContacts = value
            End Set
        End Property
        Private _previousVatRegNo As String
        Public Property PreviousVatRegNo() As String
            Get
                Return _previousVatRegNo
            End Get
            Set(ByVal value As String)
                _previousVatRegNo = value
            End Set
        End Property

        Private _previousComments As String
        Public Property PreviousComments() As String
            Get
                Return _previousComments
            End Get
            Set(ByVal value As String)
                _previousComments = value
            End Set
        End Property

        Private _previousClientTelNoOff As String
        Public Property PreviousClientTelNoOff() As String
            Get
                Return _previousClientTelNoOff
            End Get
            Set(ByVal value As String)
                _previousClientTelNoOff = value
            End Set
        End Property
        Private _previousClientOtherContacts As String
        Public Property PreviousClientOtherContacts() As String
            Get
                Return _previousClientOtherContacts
            End Get
            Set(ByVal value As String)
                _previousClientOtherContacts = value
            End Set
        End Property
        Private _previousUserDefFldAId As Integer
        Public Property PreviousUserDefFldAId() As Integer
            Get
                Return _previousUserDefFldAId
            End Get
            Set(ByVal value As Integer)
                _previousUserDefFldAId = value
            End Set
        End Property

        Private _previousUserDefFldBId As Integer
        Public Property PreviousUserDefFldBId() As Integer
            Get
                Return _previousUserDefFldBId
            End Get
            Set(ByVal value As Integer)
                _previousUserDefFldBId = value
            End Set
        End Property

        Private _previousUserDefFldCId As Integer
        Public Property PreviousUserDefFldCId() As Integer
            Get
                Return _previousUserDefFldCId
            End Get
            Set(ByVal value As Integer)
                _previousUserDefFldCId = value
            End Set
        End Property

        Private _previousUserDefFldDId As Integer
        Public Property PreviousUserDefFldDId() As Integer
            Get
                Return _previousUserDefFldDId
            End Get
            Set(ByVal value As Integer)
                _previousUserDefFldDId = value
            End Set
        End Property

        Private _previousUserDefFldEId As Integer
        Public Property PreviousUserDefFldEId() As Integer
            Get
                Return _previousUserDefFldEId
            End Get
            Set(ByVal value As Integer)
                _previousUserDefFldEId = value
            End Set
        End Property

        Private _previousClientTaxRegistered As Boolean
        Public Property PreviousClientTaxRegistered() As Boolean
            Get
                Return _previousClientTaxRegistered
            End Get
            Set(ByVal value As Boolean)
                _previousClientTaxRegistered = value
            End Set
        End Property

        Private _currencyCode As String
        Public Property CurrencyCode() As String
            Get
                Return _currencyCode
            End Get
            Set(ByVal value As String)
                _currencyCode = value
            End Set
        End Property

        Private _versionId As Integer
        Public Property VersionId() As Integer
            Get
                Return _versionId
            End Get
            Set(ByVal value As Integer)
                _versionId = value
            End Set
        End Property

        Private _previousLossFromDate As Date
        Public Property PreviousLossFromDate() As Date
            Get
                Return _previousLossFromDate
            End Get
            Set(ByVal value As Date)
                _previousLossFromDate = value
            End Set
        End Property

        Private _previousLossToDate As Date
        Public Property PreviousLossToDate() As Date
            Get
                Return _previousLossToDate
            End Get
            Set(ByVal value As Date)
                _previousLossToDate = value
            End Set
        End Property

        Private _previousInfoOnly As Boolean
        Public Property PreviousInfoOnly() As Boolean
            Get
                Return _previousInfoOnly
            End Get
            Set(ByVal value As Boolean)
                _previousInfoOnly = value
            End Set
        End Property

        Private _previousClaimStatusCode As String
        Public Property PreviousClaimStatusCode() As String
            Get
                Return _previousClaimStatusCode
            End Get
            Set(ByVal value As String)
                _previousClaimStatusCode = value
            End Set
        End Property

        Private _PaymentSuppressReserves As Integer
        Public Property PaymentSuppressReserves() As Integer
            Get
                Return _PaymentSuppressReserves
            End Get
            Set(ByVal value As Integer)
                _PaymentSuppressReserves = value
            End Set
        End Property

        Private _PaymentSuppressPayments As Integer
        Public Property PaymentSuppressPayments() As Integer
            Get
                Return _PaymentSuppressPayments
            End Get
            Set(ByVal value As Integer)
                _PaymentSuppressPayments = value
            End Set
        End Property

        Private _PaymentSuppressRecoveries As Integer
        Public Property PaymentSuppressRecoveries() As Integer
            Get
                Return _PaymentSuppressRecoveries
            End Get
            Set(ByVal value As Integer)
                _PaymentSuppressRecoveries = value
            End Set
        End Property
        Private _transactionTypeId As Integer
        Public Property TransactionTypeId() As Integer
            Get
                Return _transactionTypeId
            End Get
            Set(ByVal value As Integer)
                _transactionTypeId = value
            End Set
        End Property

        Private _transactionTypeDescription As String
        Public Property TransactionTypeDescription() As String
            Get
                Return _transactionTypeDescription
            End Get
            Set(ByVal value As String)
                _transactionTypeDescription = value
            End Set
        End Property

        Private _sourceDescription As String
        Public Property SourceDescription() As String
            Get
                Return _sourceDescription
            End Get
            Set(ByVal value As String)
                _sourceDescription = value
            End Set
        End Property

        Private _InsuredCnt As Integer
        Public Property InsuredCnt() As Integer
            Get
                Return _InsuredCnt
            End Get
            Set(ByVal value As Integer)
                _InsuredCnt = value
            End Set
        End Property

        Private _insuranceHolderCnt As Integer
        Public Property InsuranceHolderCnt() As Integer
            Get
                Return _insuranceHolderCnt
            End Get
            Set(ByVal value As Integer)
                _insuranceHolderCnt = value
            End Set
        End Property

        Private _insuranceFolderCnt As Integer
        Public Property InsuranceFolderCnt() As Integer
            Get
                Return _insuranceFolderCnt
            End Get
            Set(ByVal value As Integer)
                _insuranceFolderCnt = value
            End Set
        End Property

        Private _statsFolderId As Integer

        Public Property StatsFolderId() As Integer
            Get
                Return _statsFolderId
            End Get
            Set(ByVal value As Integer)
                _statsFolderId = value
            End Set
        End Property

        Private _currencyId As Integer

        Public Property CurrencyId() As Integer
            Get
                Return _currencyId
            End Get
            Set(ByVal value As Integer)
                _currencyId = value
            End Set
        End Property

        Private _claimId As Integer

        Public Property ClaimId() As Integer
            Get
                Return _claimId
            End Get
            Set(ByVal value As Integer)
                _claimId = value
            End Set
        End Property

        Private _productId As Integer

        Public Property ProductId() As Integer
            Get
                Return _productId
            End Get
            Set(ByVal value As Integer)
                _productId = value
            End Set
        End Property

        Private _productCode As String

        Public Property ProductCode() As String
            Get
                Return _productCode
            End Get
            Set(ByVal value As String)
                _productCode = value
            End Set
        End Property

        Private _insuranceRef As String

        Public Property InsuranceRef() As String
            Get
                Return _insuranceRef
            End Get
            Set(ByVal value As String)
                _insuranceRef = value
            End Set
        End Property

        Private _agentCnt As Integer

        Public Property AgentCnt() As Integer
            Get
                Return _agentCnt
            End Get
            Set(ByVal value As Integer)
                _agentCnt = value
            End Set
        End Property

        Public Property ClaimStatusId() As Integer
            Get
                Return _claimStatusId
            End Get
            Set(ByVal value As Integer)
                _claimStatusId = value
            End Set
        End Property

        Public Property InsurerShortName() As String
            Get
                Return _insurerShortName
            End Get
            Set(ByVal value As String)
                _insurerShortName = value
            End Set
        End Property

        Public Property ClientShortName() As String
            Get
                Return _clientShortName
            End Get
            Set(ByVal value As String)
                _clientShortName = value
            End Set
        End Property


        Public Property ClientTelNo() As String
            Get
                Return _clientTelNo
            End Get
            Set(ByVal value As String)
                _clientTelNo = value
            End Set
        End Property
        Public Property ClientFaxNo() As String
            Get
                Return _clientFaxNo
            End Get
            Set(ByVal value As String)
                _clientFaxNo = value
            End Set
        End Property
        Public Property ClientMobileNo() As String
            Get
                Return _clientMobileNo
            End Get
            Set(ByVal value As String)
                _clientMobileNo = value
            End Set
        End Property

        Public Property ClientEmail() As String
            Get
                Return _clientEmail
            End Get
            Set(ByVal value As String)
                _clientEmail = value
            End Set
        End Property
        Public Property ClientTelNoOff() As String
            Get
                Return _clientTelNoOff
            End Get
            Set(ByVal value As String)
                _clientTelNoOff = value
            End Set
        End Property
        Public Property ClientOtherContacts() As String
            Get
                Return _clientOtherContacts
            End Get
            Set(ByVal value As String)
                _clientOtherContacts = value
            End Set
        End Property
        Public Property InsurerTelNo() As String
            Get
                Return _insurerTelNo
            End Get
            Set(ByVal value As String)
                _insurerTelNo = value
            End Set
        End Property
        Public Property InsurerFaxNo() As String
            Get
                Return _insurerFaxNo
            End Get
            Set(ByVal value As String)
                _insurerFaxNo = value
            End Set
        End Property
        Public Property InsurerEmail() As String
            Get
                Return _insurerEmail
            End Get
            Set(ByVal value As String)
                _insurerEmail = value
            End Set
        End Property
        Public Property InsurerOtherContacts() As String
            Get
                Return _insurerOtherContacts
            End Get
            Set(ByVal value As String)
                _insurerOtherContacts = value
            End Set
        End Property
        Public Property InsurerMobileNo() As String
            Get
                Return _InsurerMobileNo
            End Get
            Set(ByVal value As String)
                _InsurerMobileNo = value
            End Set
        End Property
        Public Property InsurerWebNo() As String
            Get
                Return _InsurerWebNo
            End Get
            Set(ByVal value As String)
                _InsurerWebNo = value
            End Set
        End Property

        Public Property ClientWebNo() As String
            Get
                Return _ClientWebNo
            End Get
            Set(ByVal value As String)
                _ClientWebNo = value
            End Set
        End Property
        Public Property ClientAddressId() As Integer
            Get
                Return _clientAddressId
            End Get
            Set(ByVal value As Integer)
                _clientAddressId = value
            End Set
        End Property

        Public Property InsurerAddressId() As Integer
            Get
                Return _insurerAddressId
            End Get
            Set(ByVal value As Integer)
                _insurerAddressId = value
            End Set
        End Property

        Public Property PolicyNumber() As String
            Get
                Return _policyNumber
            End Get
            Set(ByVal value As String)
                _policyNumber = value
            End Set
        End Property

        Public Property ClaimNumber() As String
            Get
                Return _claimNumber
            End Get
            Set(ByVal value As String)
                _claimNumber = value
            End Set
        End Property

        Public Property UserDefFldAId() As Nullable(Of Integer)
            Get
                Return _userDefFldAId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _userDefFldAId = value
            End Set
        End Property

        Public Property UserDefFldBId() As Nullable(Of Integer)

            Get
                Return _userDefFldBId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _userDefFldBId = value
            End Set
        End Property
        Public Property UserDefFldCId() As Nullable(Of Integer)
            Get
                Return _userDefFldCId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _userDefFldCId = value
            End Set
        End Property
        Public Property UserDefFldDId() As Nullable(Of Integer)
            Get
                Return _userDefFldDId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _userDefFldDId = value
            End Set
        End Property
        Public Property UserDefFldEId() As Nullable(Of Integer)
            Get
                Return _userDefFldEId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _userDefFldEId = value
            End Set
        End Property
        Public Property UserDefFldATableCode() As String
            Get
                Return _userDefFldATableCode
            End Get
            Set(ByVal value As String)
                _userDefFldATableCode = value
            End Set
        End Property

        Public Property UserDefFldBTableCode() As String
            Get
                Return _userDefFldBTableCode
            End Get
            Set(ByVal value As String)
                _userDefFldBTableCode = value
            End Set
        End Property

        Public Property UserDefFldCTableCode() As String
            Get
                Return _userDefFldCTableCode
            End Get
            Set(ByVal value As String)
                _userDefFldCTableCode = value
            End Set
        End Property


        Public Property UserDefFldDTableCode() As String
            Get
                Return _userDefFldDTableCode
            End Get
            Set(ByVal value As String)
                _userDefFldDTableCode = value
            End Set
        End Property

        Public Property UserDefFldETableCode() As String
            Get
                Return _userDefFldETableCode
            End Get
            Set(ByVal value As String)
                _userDefFldETableCode = value
            End Set
        End Property

        Public Property SourceId() As Integer
            Get
                Return _sourceId
            End Get
            Set(ByVal value As Integer)
                _sourceId = value
            End Set
        End Property

        Public Property ProgressStatusId() As Integer
            Get
                Return _progressStatusId
            End Get
            Set(ByVal value As Integer)
                _progressStatusId = value
            End Set
        End Property

        Public Property HandlerId() As Integer
            Get
                Return _handlerId
            End Get
            Set(ByVal value As Integer)
                _handlerId = value
            End Set
        End Property

        Public Property UnderwritingYearId() As Nullable(Of Integer)
            Get
                Return _underwritingYearId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _underwritingYearId = value
            End Set
        End Property

        Public Property CatastropheId() As Nullable(Of Integer)
            Get
                Return _catastropheId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _catastropheId = value
            End Set
        End Property

        Public Property TownId() As Nullable(Of Integer)
            Get
                Return _townId
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _townId = value
            End Set
        End Property


        Public Property ClientAddressTypeId() As Integer
            Get
                Return _clientAddressTypeId
            End Get
            Set(ByVal value As Integer)
                _clientAddressTypeId = value
            End Set
        End Property

        Public Property InsurerAddressTypeId() As Integer
            Get
                Return _insurerAddressTypeId
            End Get
            Set(ByVal value As Integer)
                _insurerAddressTypeId = value
            End Set
        End Property

        Public Property PrimaryCauseId() As Integer
            Get
                Return _primaryCauseId
            End Get
            Set(ByVal value As Integer)
                _primaryCauseId = value
            End Set
        End Property

        Public Property SecondaryCauseId() As Integer
            Get
                Return _secondaryCauseId
            End Get
            Set(ByVal value As Integer)
                _secondaryCauseId = value
            End Set
        End Property

        '''<remarks/>
        Public Property Client() As BaseClaimPartyClientType
            Get
                Return Me.clientField
            End Get
            Set(ByVal value As BaseClaimPartyClientType)
                Me.clientField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Insurer() As BaseClaimPartyInsurerType
            Get
                Return Me.insurerField
            End Get
            Set(ByVal value As BaseClaimPartyInsurerType)
                Me.insurerField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProgressStatusCode() As String
            Get
                Return Me.progressStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.progressStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PrimaryCauseCode() As String
            Get
                Return Me.primaryCauseCodeField
            End Get
            Set(ByVal value As String)
                Me.primaryCauseCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossFromDate() As Date
            Get
                Return Me.lossFromDateField
            End Get
            Set(ByVal value As Date)
                Me.lossFromDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReportedDate() As Date
            Get
                Return Me.reportedDateField
            End Get
            Set(ByVal value As Date)
                Me.reportedDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property HandlerCode() As String
            Get
                Return Me.handlerCodeField
            End Get
            Set(ByVal value As String)
                Me.handlerCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InfoOnly() As Boolean
            Get
                Return Me.infoOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.infoOnlyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LikelyClaim() As Boolean
            Get
                Return Me.likelyClaimField
            End Get
            Set(ByVal value As Boolean)
                Me.likelyClaimField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SecondaryCauseCode() As String
            Get
                Return Me.secondaryCauseCodeField
            End Get
            Set(ByVal value As String)
                Me.secondaryCauseCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CatastropheCode() As String
            Get
                Return Me.catastropheCodeField
            End Get
            Set(ByVal value As String)
                Me.catastropheCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossToDate() As Date
            Get
                Return Me.lossToDateField
            End Get
            Set(ByVal value As Date)
                Me.lossToDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossToDateSpecified() As Boolean
            Get
                Return Me.lossToDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.lossToDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Location() As String
            Get
                Return Me.locationField
            End Get
            Set(ByVal value As String)
                Me.locationField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TownCode() As String
            Get
                Return Me.townCodeField
            End Get
            Set(ByVal value As String)
                Me.townCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserDefFldACode() As String
            Get
                Return Me.userDefFldACodeField
            End Get
            Set(ByVal value As String)
                Me.userDefFldACodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserDefFldBCode() As String
            Get
                Return Me.userDefFldBCodeField
            End Get
            Set(ByVal value As String)
                Me.userDefFldBCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserDefFldCCode() As String
            Get
                Return Me.userDefFldCCodeField
            End Get
            Set(ByVal value As String)
                Me.userDefFldCCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserDefFldDCode() As String
            Get
                Return Me.userDefFldDCodeField
            End Get
            Set(ByVal value As String)
                Me.userDefFldDCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserDefFldECode() As String
            Get
                Return Me.userDefFldECodeField
            End Get
            Set(ByVal value As String)
                Me.userDefFldECodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Comments() As String
            Get
                Return Me.commentsField
            End Get
            Set(ByVal value As String)
                Me.commentsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimVersionDescription() As String
            Get
                Return Me.claimVersionDescriptionField
            End Get
            Set(ByVal value As String)
                Me.claimVersionDescriptionField = value
            End Set
        End Property

        Private ignoreClaimNumberValidationField As Boolean
        Public Property IgnoreClaimNumberValidation() As Boolean
            Get
                Return Me.ignoreClaimNumberValidationField
            End Get
            Set(ByVal value As Boolean)
                Me.ignoreClaimNumberValidationField = value
            End Set
        End Property
        '''<remarks/>
        Public Property BaseCaseKey() As Integer
            Get
                Return Me.baseCaseKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseCaseKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsPolicyOutstanding() As Boolean
            Get
                Return Me.bIsPolicyOutstanding
            End Get
            Set(ByVal value As Boolean)
                Me.bIsPolicyOutstanding = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(Description) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Description")
            End If

            If String.IsNullOrEmpty(ProgressStatusCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ProgressStatusCode")
            End If


            If String.IsNullOrEmpty(PrimaryCauseCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "PrimaryCauseCode")
            End If

            If String.IsNullOrEmpty(HandlerCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "HandlerCode")
            End If

            If InfoOnly.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "InfoOnly")
            End If

            If LikelyClaim.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "LikelyClaim")
            End If

            If Client IsNot Nothing Then
                Client.Validate(CObj(oSAMErrorCollection))
            End If

            If Insurer IsNot Nothing Then
                Insurer.Validate(CObj(oSAMErrorCollection))
            End If

        End Sub

    End Class

    Public Class BaseClaimPartyClientType
        Inherits BaseClaimPartyType

        Private taxRegisteredField As Boolean

        Private taxRegistrationNumberField As String

        '''<remarks/>
        Public Property TaxRegistered() As Boolean
            Get
                Return Me.taxRegisteredField
            End Get
            Set(ByVal value As Boolean)
                Me.taxRegisteredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxRegistrationNumber() As String
            Get
                Return Me.taxRegistrationNumberField
            End Get
            Set(ByVal value As String)
                Me.taxRegistrationNumberField = value
            End Set
        End Property


        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(CObj(oSAMErrorCollection))

            If TaxRegistered.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "TaxRegistered")
            End If

            ' registration number is only mandatory when the tax registered indicator is set to true
            If TaxRegistered = True Then
                If String.IsNullOrEmpty(TaxRegistrationNumber) Then
                    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "TaxRegistrationNumber")
                End If
            End If

        End Sub
    End Class


    '''<remarks/>
    ''' 
    Public Class BaseClaimPartyType

        Private addressField As BaseAddressType

        Private partyClaimNumberField As String

        Private contactField() As BaseContactType

        Private partyKeyField As Int32

        Private partyEmailField As String

        Private partyFaxNoField As String

        Private partyMobileNoField As String

        Private partyTelNoField As String

        Private partyTelNoOffField As String

        '''<remarks/>
        Public Property Address() As BaseAddressType
            Get
                Return Me.addressField
            End Get
            Set(ByVal value As BaseAddressType)
                Me.addressField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyClaimNumber() As String
            Get
                Return Me.partyClaimNumberField
            End Get
            Set(ByVal value As String)
                Me.partyClaimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Contact() As BaseContactType()
            Get
                Return Me.contactField
            End Get
            Set(ByVal value As BaseContactType())
                Me.contactField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Int32
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Int32)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyEmail() As String
            Get
                Return Me.partyEmailField
            End Get
            Set(ByVal value As String)
                Me.partyEmailField = value
            End Set
        End Property
        '''<remarks/>
        Public Property PartyFaxNo() As String
            Get
                Return Me.partyFaxNoField
            End Get
            Set(ByVal value As String)
                Me.partyFaxNoField = value
            End Set
        End Property
        '''<remarks/>
        Public Property PartyMobileNo() As String
            Get
                Return Me.partyMobileNoField
            End Get
            Set(ByVal value As String)
                Me.partyMobileNoField = value
            End Set
        End Property
        '''<remarks/>
        Public Property PartyTelNo() As String
            Get
                Return Me.partyTelNoField
            End Get
            Set(ByVal value As String)
                Me.partyTelNoField = value
            End Set
        End Property
        '''<remarks/>
        Public Property PartyTelNoOff() As String
            Get
                Return Me.partyTelNoOffField
            End Get
            Set(ByVal value As String)
                Me.partyTelNoOffField = value
            End Set
        End Property
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If Address IsNot Nothing Then
                ' validate addresses 
                Address.Validate(CObj(oSAMErrorCollection))
            End If

            If Contact IsNot Nothing Then
                ' validate contacts 
                Dim oContact As BaseContactType
                For Each oContact In Contact
                    oContact.Validate(CObj(oSAMErrorCollection))
                Next
            End If
        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimPartyInsurerType
        Inherits BaseClaimPartyType

        Private contactNameField As String

        Private insurerShortNameField As String

        Private insurerEmailField As String

        Private insurerFaxNoField As String

        Private insurerTelNoField As String

        Private insurerContactField As String

        '''<remarks/>
        Public Property ContactName() As String
            Get
                Return Me.contactNameField
            End Get
            Set(ByVal value As String)
                Me.contactNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsurerShortName() As String
            Get
                Return Me.insurerShortNameField
            End Get
            Set(ByVal value As String)
                Me.insurerShortNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsurerEmail() As String
            Get
                Return Me.insurerEmailField
            End Get
            Set(ByVal value As String)
                Me.insurerEmailField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsurerFaxNo() As String
            Get
                Return Me.insurerFaxNoField
            End Get
            Set(ByVal value As String)
                Me.insurerFaxNoField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsurerTelNo() As String
            Get
                Return Me.insurerTelNoField
            End Get
            Set(ByVal value As String)
                Me.insurerTelNoField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsurerContact() As String
            Get
                Return Me.insurerContactField
            End Get
            Set(ByVal value As String)
                Me.insurerContactField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)

        End Sub
    End Class

    '''<remarks/>
    Public Class BaseClaimMaintainType
        Inherits BaseClaimType

        Private baseClaimKeyField As Integer

        Private claimPerilField() As BaseClaimPerilMaintainType

        Private ignoreWarningsField As Boolean

        Private externalHandlerField As Boolean
        ' Start (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        Private closeClaimOnZeroReserveRecoveryBalanceField As Boolean
        ' End (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        '''<remarks/>
        Public Property BaseClaimKey() As Integer
            Get
                Return Me.baseClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("ClaimPeril")>
        Public Property ClaimPeril() As BaseClaimPerilMaintainType()
            Get
                Return Me.claimPerilField
            End Get
            Set(ByVal value As BaseClaimPerilMaintainType())
                Me.claimPerilField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IgnoreWarnings() As Boolean
            Get
                Return Me.ignoreWarningsField
            End Get
            Set(ByVal value As Boolean)
                Me.ignoreWarningsField = value
            End Set
        End Property
        ' Start (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
            Get
                Return Me.closeClaimOnZeroReserveRecoveryBalanceField
            End Get
            Set(ByVal value As Boolean)
                Me.closeClaimOnZeroReserveRecoveryBalanceField = value
            End Set
        End Property
        'End (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        '''<remarks/>
        Public Property ExternalHandler() As Boolean
            Get
                Return Me.externalHandlerField
            End Get
            Set(ByVal value As Boolean)
                Me.externalHandlerField = value
            End Set
        End Property

        Private _InsuranceFileId As Integer
        Public Property InsuranceFileId() As Integer
            Get
                Return _InsuranceFileId
            End Get
            Set(ByVal value As Integer)
                _InsuranceFileId = value
            End Set
        End Property

        Private _RiskId As Integer
        Public Property RiskId() As Integer
            Get
                Return _RiskId
            End Get
            Set(ByVal value As Integer)
                _RiskId = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef ErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(ErrorCollection, SAMErrorCollection)
            Dim oClaimPeril As BaseClaimPerilMaintainType

            MyBase.Validate(CObj(oSAMErrorCollection))

            If Me.BaseClaimKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                                  SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                          SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                               "BaseClaimKey")
            End If

            If ClaimPeril IsNot Nothing Then
                For Each oClaimPeril In ClaimPeril
                    oClaimPeril.Validate(CObj(oSAMErrorCollection))
                Next
            End If
        End Sub

    End Class

    '''<remarks/>

    Public Class BaseClaimPerilMaintainType
        Inherits BaseClaimPerilType

        Private baseClaimPerilKeyField As Integer

        Private baseClaimPerilKeyFieldSpecified As Boolean

        '''<remarks/>
        Public Property BaseClaimPerilKey() As Integer
            Get
                Return Me.baseClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimPerilKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property BaseClaimPerilKeySpecified() As Boolean
            Get
                Return Me.baseClaimPerilKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.baseClaimPerilKeyFieldSpecified = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseClaimMaintainRequestType
        Inherits BaseRequestType

        Private _dataTransferIsUsingFullClaimVersioning As Boolean
        Public Property DataTransferIsUsingFullClaimVersioning() As Boolean
            Get
                Return _dataTransferIsUsingFullClaimVersioning
            End Get
            Set(ByVal value As Boolean)
                _dataTransferIsUsingFullClaimVersioning = value
            End Set
        End Property


        Private _dataTransferClaim As Boolean = False
        Public Property IsDataTransferClaim() As Boolean
            Get
                Return _dataTransferClaim
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaim = value
            End Set
        End Property

        Private _dataTransferClaimHasSpecifiedClaimRiskData As Boolean
        Public Property DataTransferClaimHasClaimRiskDataSpecified() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedClaimRiskData
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedClaimRiskData = value
            End Set
        End Property

        Private _dataTransferClaimHasSpecifiedReinsurance As Boolean
        Public Property DataTransferClaimHasSpecifiedReinsurance() As Boolean
            Get
                Return _dataTransferClaimHasSpecifiedReinsurance
            End Get
            Set(ByVal value As Boolean)
                _dataTransferClaimHasSpecifiedReinsurance = value
            End Set
        End Property

        Private claimField As BaseClaimMaintainType

        Private timeStampField() As Byte

        '''<remarks/>
        Public Property Claim() As BaseClaimMaintainType
            Get
                Return Me.claimField
            End Get
            Set(ByVal value As BaseClaimMaintainType)
                Me.claimField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)

            Claim.Validate(oErrorCollection)

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseGetClaimsResponseTypeClaims

        Private rowField() As BaseGetClaimsResponseTypeClaimsRow

        '''<remarks/>

        Public Property Row() As BaseGetClaimsResponseTypeClaimsRow()
            Get
                Return Me.rowField
            End Get
            Set(ByVal value As BaseGetClaimsResponseTypeClaimsRow())
                Me.rowField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimsResponseTypeClaimsRow

        Private claimKeyField As Integer

        Private claimNumberField As String

        Private policyNumberField As String

        Private claimStatusField As String

        Private regardingField As String

        Private handlerField As String

        Private primaryCauseField As String

        Private lossDateField As Date

        Private reportedDateField As Date

        Private insurerField As String

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimNumber() As String
            Get
                Return Me.claimNumberField
            End Get
            Set(ByVal value As String)
                Me.claimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PolicyNumber() As String
            Get
                Return Me.policyNumberField
            End Get
            Set(ByVal value As String)
                Me.policyNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimStatus() As String
            Get
                Return Me.claimStatusField
            End Get
            Set(ByVal value As String)
                Me.claimStatusField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Regarding() As String
            Get
                Return Me.regardingField
            End Get
            Set(ByVal value As String)
                Me.regardingField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Handler() As String
            Get
                Return Me.handlerField
            End Get
            Set(ByVal value As String)
                Me.handlerField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PrimaryCause() As String
            Get
                Return Me.primaryCauseField
            End Get
            Set(ByVal value As String)
                Me.primaryCauseField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossDate() As Date
            Get
                Return Me.lossDateField
            End Get
            Set(ByVal value As Date)
                Me.lossDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReportedDate() As Date
            Get
                Return Me.reportedDateField
            End Get
            Set(ByVal value As Date)
                Me.reportedDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Insurer() As String
            Get
                Return Me.insurerField
            End Get
            Set(ByVal value As String)
                Me.insurerField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimRiskResponseType
        Inherits BaseResponseType

        Private xMLDataSetField As String

        Private timeStampField() As Byte

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.xMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimReceiptTaxesResponseType
        Inherits BaseResponseType

        Private taxItemsField() As BaseClaimReceiptTaxItemType

        Private recoveriesField() As BaseClaimPerilRecoveryReceiptType

        Private receiptItemsField() As BaseGetClaimReceiptTaxesResponseTypeReceiptItems

        Private receiptToLossExchangeRateField As Decimal

        Public Property ReceiptToLossExchangeRate() As Decimal
            Get
                Return Me.receiptToLossExchangeRateField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptToLossExchangeRateField = value
            End Set
        End Property

        '''<remarks/>

        Public Property TaxItems() As BaseClaimReceiptTaxItemType()
            Get
                Return Me.taxItemsField
            End Get
            Set(ByVal value As BaseClaimReceiptTaxItemType())
                Me.taxItemsField = value
            End Set
        End Property

        '''<remarks/>

        Public Property Recoveries() As BaseClaimPerilRecoveryReceiptType()
            Get
                Return Me.recoveriesField
            End Get
            Set(ByVal value As BaseClaimPerilRecoveryReceiptType())
                Me.recoveriesField = value
            End Set
        End Property

        '''<remarks/>

        Public Property ReceiptItems() As BaseGetClaimReceiptTaxesResponseTypeReceiptItems()
            Get
                Return Me.receiptItemsField
            End Get
            Set(ByVal value As BaseGetClaimReceiptTaxesResponseTypeReceiptItems())
                Me.receiptItemsField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseClaimReceiptTaxItemType

        Private recoveryTypeField As String

        Private taxGroupCodeField As String

        Private taxBandCodeField As String

        Private percentageField As Decimal

        Private amountField As Decimal

        '''<remarks/>
        Public Property RecoveryType() As String
            Get
                Return Me.recoveryTypeField
            End Get
            Set(ByVal value As String)
                Me.recoveryTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxBandCode() As String
            Get
                Return Me.taxBandCodeField
            End Get
            Set(ByVal value As String)
                Me.taxBandCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Percentage() As Decimal
            Get
                Return Me.percentageField
            End Get
            Set(ByVal value As Decimal)
                Me.percentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set(ByVal value As Decimal)
                Me.amountField = value
            End Set
        End Property

        Public Property ClassOfBusinessID() As Integer

        Public Property TaxBandId() As Integer

        Public Property TaxGroupId() As Integer

        Public Property Sequence() As Integer

        Public Property IsManuallyChanges() As Integer
    End Class

    '''<remarks/>





    Public Class BaseClaimPerilRecoveryReceiptType

        Private baseRecoveryKeyField As Integer

        Private typeCodeField As String

        Private totalRecoveryAmountField As Decimal

        Private totalReceiptAmountField As Decimal

        Private thisReceiptINCLTaxAmountField As Decimal

        Private thisReceiptTaxAmountField As Decimal

        Private thisReceiptNetAmountField As Decimal

        Private balanceAmountField As Decimal

        Private _recoveryTypeId As Integer
        Public Property RecoveryTypeId() As Integer
            Get
                Return _recoveryTypeId
            End Get
            Set(ByVal value As Integer)
                _recoveryTypeId = value
            End Set
        End Property

        Private _recoveryId As Integer
        Public Property RecoveryId() As Integer
            Get
                Return _recoveryId
            End Get
            Set(ByVal value As Integer)
                _recoveryId = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseRecoveryKey() As Integer

            Get
                Return Me.baseRecoveryKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseRecoveryKeyField = value
            End Set
        End Property


        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TotalRecoveryAmount() As Decimal
            Get
                Return Me.totalRecoveryAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.totalRecoveryAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TotalReceiptAmount() As Decimal
            Get
                Return Me.totalReceiptAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.totalReceiptAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisReceiptINCLTaxAmount() As Decimal
            Get
                Return Me.thisReceiptINCLTaxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.thisReceiptINCLTaxAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisReceiptTaxAmount() As Decimal
            Get
                Return Me.thisReceiptTaxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.thisReceiptTaxAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisReceiptNetAmount() As Decimal
            Get
                Return Me.thisReceiptNetAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.thisReceiptNetAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BalanceAmount() As Decimal
            Get
                Return Me.balanceAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.balanceAmountField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimReceiptTaxesResponseTypeReceiptItems
        Inherits BaseClaimReceiptItemType
    End Class

    '''<remarks/>
    Public Class BaseGetClaimPaymentTaxesResponseType
        Inherits BaseResponseType

        Private taxItemsField() As BaseClaimPaymentTaxItemType

        Private reservesField() As BaseClaimPerilReservePaymentType

        Private paymentItemsField() As BaseGetClaimPaymentTaxesResponseTypePaymentItems


        '''<remarks/>

        Public Property TaxItems() As BaseClaimPaymentTaxItemType()
            Get
                Return Me.taxItemsField
            End Get
            Set(ByVal value As BaseClaimPaymentTaxItemType())
                Me.taxItemsField = value
            End Set
        End Property

        '''<remarks/>

        Public Property Reserves() As BaseClaimPerilReservePaymentType()
            Get
                Return Me.reservesField
            End Get
            Set(ByVal value As BaseClaimPerilReservePaymentType())
                Me.reservesField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("PaymentItems")>
        Public Property PaymentItems() As BaseGetClaimPaymentTaxesResponseTypePaymentItems()
            Get
                Return Me.paymentItemsField
            End Get
            Set(ByVal value As BaseGetClaimPaymentTaxesResponseTypePaymentItems())
                Me.paymentItemsField = value
            End Set
        End Property
    End Class

    '''<remarks/>





    Public Class BaseClaimPaymentTaxItemType

        Private reserveTypeField As String

        Private taxGroupCodeField As String

        Private taxBandCodeField As String

        Private percentageField As Decimal

        Private amountField As Decimal

        '''<remarks/>
        Public Property ReserveType() As String
            Get
                Return Me.reserveTypeField
            End Get
            Set(ByVal value As String)
                Me.reserveTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxBandCode() As String
            Get
                Return Me.taxBandCodeField
            End Get
            Set(ByVal value As String)
                Me.taxBandCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Percentage() As Decimal
            Get
                Return Me.percentageField
            End Get
            Set(ByVal value As Decimal)
                Me.percentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set(ByVal value As Decimal)
                Me.amountField = value
            End Set
        End Property

        Public Property ClassOfBusinessID() As Integer

        Public Property TaxBandId() As Integer

        Public Property TaxGroupId() As Integer

        Public Property Sequence() As Integer

        Public Property IsManuallyChanges() As Integer
    End Class

    Public Class BaseClaimPerilReservePaymentType

        Private baseReserveKeyField As Integer

        Private typeCodeField As String

        Private totalReserveField As Decimal

        Private paidToDateField As Decimal

        Private paidToDateTaxField As Decimal

        Private currentReserveField As Decimal

        Private thisPaymentINCLTaxField As Decimal

        Private thisPaymentTaxField As Decimal

        Private costToClaimField As Decimal

        '''<remarks/>
        Public Property BaseReserveKey() As Integer

            Get
                Return Me.baseReserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseReserveKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TotalReserve() As Decimal
            Get
                Return Me.totalReserveField
            End Get
            Set(ByVal value As Decimal)
                Me.totalReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaidToDate() As Decimal
            Get
                Return Me.paidToDateField
            End Get
            Set(ByVal value As Decimal)
                Me.paidToDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaidToDateTax() As Decimal
            Get
                Return Me.paidToDateTaxField
            End Get
            Set(ByVal value As Decimal)
                Me.paidToDateTaxField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrentReserve() As Decimal
            Get
                Return Me.currentReserveField
            End Get
            Set(ByVal value As Decimal)
                Me.currentReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisPaymentINCLTax() As Decimal
            Get
                Return Me.thisPaymentINCLTaxField
            End Get
            Set(ByVal value As Decimal)
                Me.thisPaymentINCLTaxField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisPaymentTax() As Decimal
            Get
                Return Me.thisPaymentTaxField
            End Get
            Set(ByVal value As Decimal)
                Me.thisPaymentTaxField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CostToClaim() As Decimal
            Get
                Return Me.costToClaimField
            End Get
            Set(ByVal value As Decimal)
                Me.costToClaimField = value
            End Set
        End Property
    End Class

    '''<remarks/>





    Public Class BaseGetClaimPaymentTaxesResponseTypePaymentItems
        Inherits BaseClaimPaymentItemType

        'Private taxAmountField As Decimal

        'Private paymentAdjustmentField As Decimal

        ''''<remarks/>
        'Public Property TaxAmount() As Decimal
        '    Get
        '        Return Me.taxAmountField
        '    End Get
        '    Set(ByVal value As Decimal)
        '        Me.taxAmountField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property PaymentAdjustment() As Decimal
        '    Get
        '        Return Me.paymentAdjustmentField
        '    End Get
        '    Set(ByVal value As Decimal)
        '        Me.paymentAdjustmentField = value
        '    End Set
        'End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimDetailsResponseType
        Inherits BaseResponseType

        Private claimDetailsField As BaseGetClaimDetailsType

        Private timeStampField() As Byte

        '''<remarks/>
        Public Property ClaimDetails() As BaseGetClaimDetailsType
            Get
                Return Me.claimDetailsField
            End Get
            Set(ByVal value As BaseGetClaimDetailsType)
                Me.claimDetailsField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimDetailsType

        Private claimDetailsField As BaseGetClaimDetailsTypeClaimDetails

        Private claimPerilField() As BaseGetClaimPerilDetailsType

        '''<remarks/>
        Public Property ClaimDetails() As BaseGetClaimDetailsTypeClaimDetails
            Get
                Return Me.claimDetailsField
            End Get
            Set(ByVal value As BaseGetClaimDetailsTypeClaimDetails)
                Me.claimDetailsField = value
            End Set
        End Property

        '''<remarks/>

        Public Property ClaimPeril() As BaseGetClaimPerilDetailsType()
            Get
                Return Me.claimPerilField
            End Get
            Set(ByVal value As BaseGetClaimPerilDetailsType())
                Me.claimPerilField = value
            End Set
        End Property
    End Class

    '''<remarks/>    
    Public Class BaseGetClaimDetailsTypeClaimDetails
        Inherits BaseClaimType

        Private baseClaimKeyField As Integer
        Private underwritingYearCodeField As String
        Private claimKeyField As Integer
        Private gisScreenCodeField As String

        Public Property GisScreenCode() As String
            Get
                Return gisScreenCodeField
            End Get
            Set(ByVal value As String)
                gisScreenCodeField = value
            End Set
        End Property

        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimKey() As Integer
            Get
                Return Me.baseClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UnderwritingYearCode() As String
            Get
                Return Me.underwritingYearCodeField
            End Get
            Set(ByVal value As String)
                Me.underwritingYearCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>

    Public Class BaseGetClaimPerilDetailsType

        Private baseClaimPerilKeyField As Integer

        Private descriptionField As String

        Private commentsField As String

        Private sumInsuredField As Decimal

        Private rIBandField As String

        Private gisScreenCodeField As String

        Private reserveField() As BaseGetClaimReserveDetailsType

        Private recoveryField() As BaseGetClaimRecoveryDetailsType

        Private claimPaymentsField() As BaseGetClaimPaymentDetailsType

        Private claimReceiptsField() As BaseGetClaimReceiptDetailsType

        Private typeCodeField As String

        Private claimPerilKeyField As Integer

        Public Property ClaimPerilKey() As Integer
            Get
                Return Me.claimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimPerilKeyField = value
            End Set
        End Property

        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimPerilKey() As Integer
            Get
                Return Me.baseClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimPerilKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Comments() As String
            Get
                Return Me.commentsField
            End Get
            Set(ByVal value As String)
                Me.commentsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SumInsured() As Decimal
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Decimal)
                Me.sumInsuredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RIBand() As String
            Get
                Return Me.rIBandField
            End Get
            Set(ByVal value As String)
                Me.rIBandField = value
            End Set
        End Property

        '''<remarks/>
        Public Property GisScreenCode() As String
            Get
                Return Me.gisScreenCodeField
            End Get
            Set(ByVal value As String)
                Me.gisScreenCodeField = value
            End Set
        End Property

        '''<remarks/>

        Public Property Reserve() As BaseGetClaimReserveDetailsType()
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As BaseGetClaimReserveDetailsType())
                Me.reserveField = value
            End Set
        End Property

        '''<remarks/>

        Public Property Recovery() As BaseGetClaimRecoveryDetailsType()
            Get
                Return Me.recoveryField
            End Get
            Set(ByVal value As BaseGetClaimRecoveryDetailsType())
                Me.recoveryField = value
            End Set
        End Property

        '''<remarks/>

        Public Property ClaimPayments() As BaseGetClaimPaymentDetailsType()
            Get
                Return Me.claimPaymentsField
            End Get
            Set(ByVal value As BaseGetClaimPaymentDetailsType())
                Me.claimPaymentsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimReceipts() As BaseGetClaimReceiptDetailsType()
            Get
                Return Me.claimReceiptsField
            End Get
            Set(ByVal value As BaseGetClaimReceiptDetailsType())
                Me.claimReceiptsField = value
            End Set
        End Property
    End Class

    '''<remarks/>





    Public Class BaseGetClaimReserveDetailsType

        Private baseReserveKeyField As Integer

        Private typeCodeField As String

        Private sumInsuredField As Decimal

        Private initialReserveField As Decimal

        Private revisedReserveField As Decimal

        Private paidAmountField As Decimal

        Private _typeKey As Integer

        Private isExcessField As Boolean

        Private isIndemnityField As Boolean

        Private isExpenseField As Boolean

        'GAURAV
        Private _ClaimPerilId As Integer

        Private canDeleteField As Boolean

        Private thisRevisionField As Decimal


        Public Property ClaimPerilId() As Integer
            Get
                Return _ClaimPerilId
            End Get
            Set(ByVal value As Integer)
                _ClaimPerilId = value
            End Set
        End Property

        Public Property TypeKey() As Integer
            Get
                Return _typeKey
            End Get
            Set(ByVal value As Integer)
                _typeKey = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseReserveKey() As Integer
            Get
                Return Me.baseReserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseReserveKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SumInsured() As Decimal
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Decimal)
                Me.sumInsuredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InitialReserve() As Decimal
            Get
                Return Me.initialReserveField
            End Get
            Set(ByVal value As Decimal)
                Me.initialReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RevisedReserve() As Decimal
            Get
                Return Me.revisedReserveField
            End Get
            Set(ByVal value As Decimal)
                Me.revisedReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaidAmount() As Decimal
            Get
                Return Me.paidAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.paidAmountField = value
            End Set
        End Property


        '''<remarks/>
        Public Property IsExcess() As Boolean
            Get
                Return Me.isExcessField
            End Get
            Set(ByVal value As Boolean)
                Me.isExcessField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsIndemnity() As Boolean
            Get
                Return Me.isIndemnityField
            End Get
            Set(ByVal value As Boolean)
                Me.isIndemnityField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsExpense() As Boolean
            Get
                Return Me.isExpenseField
            End Get
            Set(ByVal value As Boolean)
                Me.isExpenseField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CanDelete() As Boolean
            Get
                Return Me.canDeleteField
            End Get
            Set(ByVal value As Boolean)
                Me.canDeleteField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisRevision() As Decimal
            Get
                Return Me.thisRevisionField
            End Get
            Set(ByVal value As Decimal)
                Me.thisRevisionField = value
            End Set
        End Property
        ''' <summary>
        ''' Reserve type description
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeDescription() As String

        Public Property GrossReserve() As Decimal

        Public Property Tax() As Decimal

        Public Property RevisedGrossReserve() As Decimal

        Public Property RevisedTaxReserve() As Decimal

        Public Property PaidToDateTax() As Decimal

    End Class

    '''<remarks/>
    Public Class BaseGetClaimRecoveryDetailsType

        Private baseRecoveryKeyField As Integer

        Private typeCodeField As String

        Private currencyCodeField As String

        Private initialRecoveryField As Decimal

        Private revisedRecoveryField As Decimal

        Private receiptedAmountField As Decimal

        Private receiptedTaxAmountField As Decimal
        ' Start (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc)-(6.1.1.2)
        Private isSalvageField As Integer
        'End   (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc)-(6.1.1.2)

        Private recoveryPartyTypeCodeField As String

        Private recoveryPartyCodeField As String

        Private recoveryPartyTypeKeyField As Integer

        Private recoveryPartyTypeKeyFieldSpecified As Boolean

        Private recoveryPartyKeyField As Integer

        Private recoveryPartyKeyFieldSpecified As Boolean



        Private _typeKey As Integer

        'GAURAV
        Private _ClaimPerilId As Integer

        Private canDeleteField As Boolean

        Public Property ClaimPerilId() As Integer
            Get
                Return _ClaimPerilId
            End Get
            Set(ByVal value As Integer)
                _ClaimPerilId = value
            End Set
        End Property

        Public Property TypeKey() As Integer
            Get
                Return _typeKey
            End Get
            Set(ByVal value As Integer)
                _typeKey = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseRecoveryKey() As Integer
            Get
                Return Me.baseRecoveryKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseRecoveryKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InitialRecovery() As Decimal
            Get
                Return Me.initialRecoveryField
            End Get
            Set(ByVal value As Decimal)
                Me.initialRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RevisedRecovery() As Decimal
            Get
                Return Me.revisedRecoveryField
            End Get
            Set(ByVal value As Decimal)
                Me.revisedRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptedAmount() As Decimal
            Get
                Return Me.receiptedAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptedAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptedTaxAmount() As Decimal
            Get
                Return Me.receiptedTaxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptedTaxAmountField = value
            End Set
        End Property
        '''<remarks/>
        ''' Start (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc)-(6.1.1.2)

        Public Property IsSalvage() As Integer
            Get
                Return Me.isSalvageField
            End Get
            Set(ByVal value As Integer)
                Me.isSalvageField = value
            End Set
        End Property


        'End (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc)-(6.1.1.2)

        '''<remarks/>
        Public Property RecoveryPartyTypeCode() As String
            Get
                Return Me.recoveryPartyTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryPartyTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryPartyCode() As String
            Get
                Return Me.recoveryPartyCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryPartyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryPartyTypeKey() As Integer
            Get
                Return Me.recoveryPartyTypeKeyField
            End Get
            Set(ByVal value As Integer)
                Me.recoveryPartyTypeKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoveryPartyTypeKeySpecified() As Boolean
            Get
                Return Me.recoveryPartyTypeKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoveryPartyTypeKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryPartyKey() As Integer
            Get
                Return Me.recoveryPartyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.recoveryPartyKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoveryPartyKeySpecified() As Boolean
            Get
                Return Me.recoveryPartyKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoveryPartyKeyFieldSpecified = value
            End Set
        End Property


        Public Property CanDelete() As Boolean
            Get
                Return Me.canDeleteField
            End Get
            Set(ByVal value As Boolean)
                Me.canDeleteField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimPaymentDetailsTypeTaxAmount
    End Class

    '''<remarks/>
    Public Class BaseGetClaimPaymentItemDetailsType

        Private baseClaimPaymentItemKeyField As Integer

        Private baseReserveKeyField As Integer

        Private baseRecoveryKeyField As Integer

        Private paymentAmountField As Decimal

        Private taxAmountField As Decimal

        Private paymentAdjustmentField As Decimal

        Private totalTaxAmountField As Decimal

        Private reserveKeyField As Integer

        Private taxGroupCodeField As String

        Private thisRevisionField As Decimal

        Private lossAmountField As Decimal
        Private lossTaxAmountField As Decimal

        Private baseAmountField As Decimal
        '''<remarks/>
        Public Property BaseClaimPaymentItemKey() As Integer
            Get
                Return Me.baseClaimPaymentItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimPaymentItemKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseReserveKey() As Integer
            Get
                Return Me.baseReserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseReserveKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseRecoveryKey() As Integer
            Get
                Return Me.baseRecoveryKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseRecoveryKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentAmount() As Decimal
            Get
                Return Me.paymentAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmount() As Decimal
            Get
                Return Me.taxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.taxAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentAdjustment() As Decimal
            Get
                Return Me.paymentAdjustmentField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentAdjustmentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TotalTaxAmount() As Decimal
            Get
                Return Me.totalTaxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.totalTaxAmountField = value
            End Set
        End Property
        '''<remarks/>
        Public Property ReserveKey() As Integer
            Get
                Return Me.reserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.reserveKeyField = value
            End Set
        End Property
        '''<remarks/>
        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisRevision() As Decimal
            Get
                Return Me.thisRevisionField
            End Get
            Set(ByVal value As Decimal)
                Me.thisRevisionField = value
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
        '''<remarks/>
        Public Property LossTaxAmount() As Decimal
            Get
                Return Me.lossTaxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.lossTaxAmountField = value
            End Set
        End Property
        Public Property BaseCurrencyCode() As String

    End Class


    Public Class BaseGetClaimPaymentDetailsType

        Private baseClaimPaymentKeyField As Integer

        Private paymentDateField As Date

        Private currencyCodeField As String

        Private partyKeyField As Integer

        Private paymentPartyTypeField As String

        Private paymentAmountField As Decimal

        Private taxAmountField As Decimal

        Private isReferredField As Boolean

        Private advancedTaxDetailsField As BaseClaimPaymentAdvancedTaxDetailsType

        Private payeeField As BaseClaimPayeeType

        Private claimPaymentItemsField() As BaseGetClaimPaymentItemDetailsType

        Private _ClaimPerilId As Integer

        Private partyPaidCodeField As String

        Private claimKeyField As Integer

        Private ourRefField As String

        Private ultimatePayeeField As String

        Private IsExGratiaField As Boolean

        Public Property IsThisPayment() As Boolean

        Public Property DocumentReference() As String
        Public Property PaymentStatus() As String
        Public Property TheirReference() As String

        Public Property UltimatePayee() As String
            Get
                Return Me.ultimatePayeeField
            End Get
            Set(ByVal value As String)
                Me.ultimatePayeeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OurRef() As String
            Get
                Return Me.ourRefField
            End Get
            Set(ByVal value As String)
                Me.ourRefField = value
            End Set
        End Property

        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        Public Property PartyPaidCode() As String
            Get
                Return partyPaidCodeField
            End Get
            Set(ByVal value As String)
                partyPaidCodeField = value
            End Set
        End Property

        Public Property ClaimPerilId() As Integer
            Get
                Return _ClaimPerilId
            End Get
            Set(ByVal value As Integer)
                _ClaimPerilId = value
            End Set
        End Property

        'Start (Prakash C Varghese) - (Tech Spec-UIIC WR25-MaintainClaim-Payment Details.doc) - (8.1.1)
        Private currencyDescriptionField As String

        Private partyPaidNameField As String

        Private lossAmountField As Decimal

        Private baseAmountField As Decimal

        '''<remarks/>
        Public Property CurrencyDescription() As String
            Get
                Return Me.currencyDescriptionField
            End Get
            Set(ByVal value As String)
                Me.currencyDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyPaidName() As String
            Get
                Return Me.partyPaidNameField
            End Get
            Set(ByVal value As String)
                Me.partyPaidNameField = value
            End Set
        End Property

        '''<remarks/>
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
        'End (Prakash C Varghese) - (Tech Spec-UIIC WR25-MaintainClaim-Payment Details.doc) - (8.1.1)

        '''<remarks/>
        Public Property BaseClaimPaymentKey() As Integer
            Get
                Return Me.baseClaimPaymentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimPaymentKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentDate() As Date
            Get
                Return Me.paymentDateField
            End Get
            Set(ByVal value As Date)
                Me.paymentDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentPartyType() As String
            Get
                Return Me.paymentPartyTypeField
            End Get
            Set(ByVal value As String)
                Me.paymentPartyTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentAmount() As Decimal
            Get
                Return Me.paymentAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmount() As Decimal
            Get
                Return Me.taxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.taxAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsReferred() As Boolean
            Get
                Return Me.isReferredField
            End Get
            Set(ByVal value As Boolean)
                Me.isReferredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdvancedTaxDetails() As BaseClaimPaymentAdvancedTaxDetailsType
            Get
                Return Me.advancedTaxDetailsField
            End Get
            Set(ByVal value As BaseClaimPaymentAdvancedTaxDetailsType)
                Me.advancedTaxDetailsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Payee() As BaseClaimPayeeType
            Get
                Return Me.payeeField
            End Get
            Set(ByVal value As BaseClaimPayeeType)
                Me.payeeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentItems() As BaseGetClaimPaymentItemDetailsType()
            Get
                Return Me.claimPaymentItemsField
            End Get
            Set(ByVal value As BaseGetClaimPaymentItemDetailsType())
                Me.claimPaymentItemsField = value
            End Set
        End Property

        Public Property IsExGratia() As Boolean
            Get
                Return Me.IsExGratiaField
            End Get
            Set(ByVal value As Boolean)
                Me.IsExGratiaField = value
            End Set
        End Property

        Public Property LossCurrencyCode() As String
    End Class



    Public Class BaseGetClaimReceiptDetailsType

        Private baseClaimReceiptKeyField As Integer

        Private receiptDateField As Date

        Private partyKeyField As Integer

        Private receiptPartyTypeField As String

        Private currencyCodeField As String

        Private receiptAmountField As Decimal

        Private taxAmountField As Decimal

        Private payeeField As BaseClaimPayeeType

        Private receiptPartyCodeField As String

        Private advancedTaxField As BaseClaimReceiptAdvancedTaxDetailsType

        Private receiptItemField() As BaseGetClaimReceiptItemDetailsType

        Private claimReceiptKeyField As Integer

        'GAURAV
        Private _ClaimPerilId As Integer
        Public Property ClaimPerilId() As Integer
            Get
                Return _ClaimPerilId
            End Get
            Set(ByVal value As Integer)
                _ClaimPerilId = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimReceiptKey() As Integer
            Get
                Return Me.baseClaimReceiptKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimReceiptKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptDate() As Date
            Get
                Return Me.receiptDateField
            End Get
            Set(ByVal value As Date)
                Me.receiptDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptPartyType() As String
            Get
                Return Me.receiptPartyTypeField
            End Get
            Set(ByVal value As String)
                Me.receiptPartyTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptAmount() As Decimal
            Get
                Return Me.receiptAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmount() As Decimal
            Get
                Return Me.taxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.taxAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptPartyCode() As String
            Get
                Return Me.receiptPartyCodeField
            End Get
            Set(ByVal value As String)
                Me.receiptPartyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Payee() As BaseClaimPayeeType
            Get
                Return Me.payeeField
            End Get
            Set(ByVal value As BaseClaimPayeeType)
                Me.payeeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdvancedTax() As BaseClaimReceiptAdvancedTaxDetailsType
            Get
                Return Me.advancedTaxField
            End Get
            Set(ByVal value As BaseClaimReceiptAdvancedTaxDetailsType)
                Me.advancedTaxField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptItem() As BaseGetClaimReceiptItemDetailsType()
            Get
                Return Me.receiptItemField
            End Get
            Set(ByVal value As BaseGetClaimReceiptItemDetailsType())
                Me.receiptItemField = value
            End Set
        End Property
        Public Property ClaimReceiptKey() As Integer
            Get
                Return Me.claimReceiptKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimReceiptKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetClaimReceiptItemDetailsType

        Private baseClaimReceiptItemKeyField As Integer

        Private baseRecoveryKeyField As Integer

        Private baseReserveKeyField As Integer

        Private receiptAmountField As Decimal

        Private taxAmountField As Decimal

        Private receiptBaseAmountField As Decimal

        Private receiptLossAmountField As Decimal

        Private claimReceiptItemKeyField As Integer

        Private recoveryTypeCodeField As String

        Private taxGroupCodeField As String
        '''<remarks/>
        Public Property BaseClaimReceiptItemKey() As Integer
            Get
                Return Me.baseClaimReceiptItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimReceiptItemKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseRecoveryKey() As Integer
            Get
                Return Me.baseRecoveryKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseRecoveryKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseReserveKey() As Integer
            Get
                Return Me.baseReserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseReserveKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptAmount() As Decimal
            Get
                Return Me.receiptAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmount() As Decimal
            Get
                Return Me.taxAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.taxAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptBaseAmount() As Decimal
            Get
                Return Me.receiptBaseAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptBaseAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptLossAmount() As Decimal
            Get
                Return Me.receiptLossAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptLossAmountField = value
            End Set
        End Property
        Public Property ClaimReceiptItemKey() As Integer
            Get
                Return Me.claimReceiptItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimReceiptItemKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoveryTypeCode() As String
            Get
                Return Me.recoveryTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    Public Class BaseGenerateClaimsDocumentsResponseType
        Inherits BaseResponseType

        Private documentsField As BaseGenerateClaimsDocumentsResponseTypeDocuments

        '''<remarks/>
        Public Property Documents() As BaseGenerateClaimsDocumentsResponseTypeDocuments
            Get
                Return Me.documentsField
            End Get
            Set(ByVal value As BaseGenerateClaimsDocumentsResponseTypeDocuments)
                Me.documentsField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGenerateClaimsDocumentsResponseTypeDocuments

        Private rowField() As BaseGenerateClaimsDocumentsResponseTypeDocumentsRow

        '''<remarks/>

        Public Property Row() As BaseGenerateClaimsDocumentsResponseTypeDocumentsRow()
            Get
                Return Me.rowField
            End Get
            Set(ByVal value As BaseGenerateClaimsDocumentsResponseTypeDocumentsRow())
                Me.rowField = value
            End Set
        End Property
    End Class

    '''<remarks/>



    Public Class BaseGenerateClaimsDocumentsResponseTypeDocumentsRow

        Private documentNameField As String

        Private documentDescriptionField As String

        '''<remarks/>
        Public Property DocumentName() As String
            Get
                Return Me.documentNameField
            End Get
            Set(ByVal value As String)
                Me.documentNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DocumentDescription() As String
            Get
                Return Me.documentDescriptionField
            End Get
            Set(ByVal value As String)
                Me.documentDescriptionField = value
            End Set
        End Property

    End Class

    '''<remarks/>
    Public Class BaseClaimResponseType
        Inherits BaseResponseType

        Private claimKeyField As Integer

        Private baseClaimKeyField As Integer

        Private claimNumberField As String

        Private versionField As Integer

        Private timeStampField() As Byte
        Private isPaymentAuthorized As Boolean
        ' Start (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        Private resultingStatusField As String
        ' End (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        Private warningsField() As BaseClaimResponseTypeWarnings

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BaseClaimKey() As Integer
            Get
                Return Me.baseClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseClaimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimNumber() As String
            Get
                Return Me.claimNumberField
            End Get
            Set(ByVal value As String)
                Me.claimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Version() As Integer
            Get
                Return Me.versionField
            End Get
            Set(ByVal value As Integer)
                Me.versionField = value
            End Set
        End Property
        Public Property PaymentAuthorized() As Boolean
            Get
                Return Me.isPaymentAuthorized
            End Get
            Set(ByVal value As Boolean)
                Me.isPaymentAuthorized = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
        ' Start (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)
        Public Property ResultingStatus() As String
            Get
                Return Me.resultingStatusField
            End Get
            Set(ByVal value As String)
                Me.resultingStatusField = value
            End Set
        End Property
        'End (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(7.1.2.1.3)

        '''<remarks/>
        Public Property Warnings() As BaseClaimResponseTypeWarnings()
            Get
                Return Me.warningsField
            End Get
            Set(ByVal value As BaseClaimResponseTypeWarnings())
                Me.warningsField = value
            End Set
        End Property
    End Class
    '''<remarks/>
    Public Class BaseClaimMTAResponseType
        Inherits BaseResponseType

        Private insuranceFileKeyField As Integer

        Private riskKeyField As Integer

        Private xMLDataSetField As String

        Private timeStampField As Byte()

        '''<remarks/>
        Public Property InsuranceFileKey() As Integer
            Get
                Return Me.insuranceFileKeyField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskKey() As Integer
            Get
                Return Me.riskKeyField
            End Get
            Set(ByVal value As Integer)
                Me.riskKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.xMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class




    '''<remarks/>
    Public Class BaseClaimBuilderRiskType

        Private dataModelCodeField As String

        Private xMLDataSetField As String

        '''<remarks/>
        Public Property DataModelCode() As String
            Get
                Return Me.dataModelCodeField
            End Get
            Set(ByVal value As String)
                Me.dataModelCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.xMLDataSetField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    Public Class BaseFindClaimRequestType
        Inherits BaseRequestType

        Private claimNumberField As String

        Private insuranceFileRefField As String

        Private clientShortNameField As String

        Private lossDateFromField As Date

        Private lossDateFromFieldSpecified As Boolean

        Private lossDateToField As Date

        Private lossDateToFieldSpecified As Boolean

        Private riskIndexField As String
        'Start (PraveenGora) (Tech Spec-UIIC WR25-Maintain Claim-Find Claim.doc) (9)
        Private includeClosedClaimField As Boolean
        'End (PraveenGora) (Tech Spec-UIIC WR25-Maintain Claim-Find Claim.doc) (9)

        Private agentKeyField As Integer
        Private agentGroupKeyField As Integer

        Private riskKeyField As Integer

        Private maxRowsToFetchField As Integer

        Private maxRowsToFetchFieldSpecified As Boolean

        Private exactClaimOnlyField As Boolean

        Private exactClaimOnlyFieldSpecified As Boolean

        Private tpaField As String

        Private caseNumberField As String

        Private caseNumberFieldSpecified As Boolean

        Private DescriptionField As String


        Public Property Description() As String
            Get
                Return Me.DescriptionField
            End Get
            Set(ByVal value As String)
                Me.DescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CaseNumber() As String
            Get
                Return Me.caseNumberField
            End Get
            Set(ByVal value As String)
                Me.caseNumberField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property CaseNumberSpecified() As Boolean
            Get
                Return Me.caseNumberFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.caseNumberFieldSpecified = value
            End Set
        End Property

        Public Property TPA() As String
            Get
                Return Me.tpaField
            End Get
            Set(ByVal value As String)
                Me.tpaField = value
            End Set
        End Property
        Public Property AgentKey() As Integer
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.agentKeyField = value
            End Set
        End Property

        Public Property AgentGroupKey() As Integer
            Get
                Return Me.agentGroupKeyField
            End Get
            Set(ByVal value As Integer)
                Me.agentGroupKeyField = value
            End Set
        End Property
        'End (Prakash C Varghese) - (Agent Group Association) 

        '''<remarks/>
        Public Property ClaimNumber() As String
            Get
                Return Me.claimNumberField
            End Get
            Set(ByVal value As String)
                Me.claimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileRef() As String
            Get
                Return Me.insuranceFileRefField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientShortName() As String
            Get
                Return Me.clientShortNameField
            End Get
            Set(ByVal value As String)
                Me.clientShortNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossDateFrom() As Date
            Get
                Return Me.lossDateFromField
            End Get
            Set(ByVal value As Date)
                Me.lossDateFromField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property LossDateFromSpecified() As Boolean
            Get
                Return Me.lossDateFromFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.lossDateFromFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossDateTo() As Date
            Get
                Return Me.lossDateToField
            End Get
            Set(ByVal value As Date)
                Me.lossDateToField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property LossDateToSpecified() As Boolean
            Get
                Return Me.lossDateToFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.lossDateToFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskIndex() As String
            Get
                Return Me.riskIndexField
            End Get
            Set(ByVal value As String)
                Me.riskIndexField = value
            End Set
        End Property
        'Start (PraveenGora) (Tech Spec-UIIC WR25-Maintain Claim-Find Claim.doc) (11.1.1)
        '''<remarks/>
        Public Property IncludeClosedClaim() As Boolean
            Get
                Return Me.includeClosedClaimField
            End Get
            Set(ByVal value As Boolean)
                Me.includeClosedClaimField = value
            End Set
        End Property
        'End (PraveenGora) (Tech Spec-UIIC WR25-Maintain Claim-Find Claim.doc) (11.1.1)
        Public Property RiskKey() As Integer
            Get
                Return Me.riskKeyField
            End Get
            Set(ByVal value As Integer)
                Me.riskKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MaxRowsToFetch() As Integer
            Get
                Return Me.maxRowsToFetchField
            End Get
            Set(ByVal value As Integer)
                Me.maxRowsToFetchField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property MaxRowsToFetchSpecified() As Boolean
            Get
                Return Me.maxRowsToFetchFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.maxRowsToFetchFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ExactClaimOnly() As Boolean
            Get
                Return Me.exactClaimOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.exactClaimOnlyField = value
            End Set
        End Property

        <XmlIgnore()>
        Public Property ExactClaimOnlySpecified() As Boolean
            Get
                Return Me.exactClaimOnlyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.exactClaimOnlyFieldSpecified = value
            End Set
        End Property
        Public Property RetrieveAssociates() As Boolean
    End Class

    '''<remarks/>
    Public Enum ClaimReceiptPartyTypeType
        CLMRECEIVABLE
        PARTY
        AGENT
        CLIENT
    End Enum

    Public Enum ClaimPaymentPartyTypeType
        CLMPAYABLE
        PARTY
        AGENT
        CLIENT
    End Enum


    Public Class BaseClaimResponseTypeWarnings

        Private codeField As Integer

        Private descriptionField As String

        Public Property Code() As Integer
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As Integer)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property
    End Class

    Public Class BaseGetClaimRiskLinksRequestType
        Inherits BaseRequestType

        Private insuranceFileKeyField As Integer

        Private riskKeyField As Integer

        '''<remarks/>
        Public Property InsuranceFileKey() As Integer
            Get
                Return Me.insuranceFileKeyField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskKey() As Integer
            Get
                Return Me.riskKeyField
            End Get
            Set(ByVal value As Integer)
                Me.riskKeyField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If InsuranceFileKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                "InsuranceFileKey")
            End If

            If RiskKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                "RiskKey")
            End If

        End Sub
    End Class

    Public Class BaseGetClaimRiskLinksResponseType
        Inherits BaseResponseType

        Private perilTypeField() As BaseGetClaimRiskLinksResponseTypePerilType

        Public Property PerilType() As BaseGetClaimRiskLinksResponseTypePerilType()
            Get
                Return Me.perilTypeField
            End Get
            Set(ByVal value As BaseGetClaimRiskLinksResponseTypePerilType())
                Me.perilTypeField = value
            End Set
        End Property
    End Class


    Public Class BaseGetClaimRiskLinksResponseTypePerilType

        Private codeField As String

        Private descriptionField As String

        Private reserveTypeField() As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType

        Private recoveryTypeField() As BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        Private sumInsuredField As Decimal

        '''<remarks/>
        Public Property SumInsured() As Decimal
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Decimal)
                Me.sumInsuredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        Public Property ReserveType() As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType()
            Get
                Return Me.reserveTypeField
            End Get
            Set(ByVal value As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType())
                Me.reserveTypeField = value
            End Set
        End Property

        Public Property RecoveryType() As BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType()
            Get
                Return Me.recoveryTypeField
            End Get
            Set(ByVal value As BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType())
                Me.recoveryTypeField = value
            End Set
        End Property
    End Class

    Public Class BaseGetClaimRiskLinksResponseTypePerilTypeReserveType

        Private codeField As String

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        Private descriptionField As String

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

    End Class

    Public Class BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        Private codeField As String

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        Private descriptionField As String

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property
        'Start (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc)-(6.1.1.1)
        Private isSalvageField As Integer
        '''<remarks/>
        Public Property IsSalvage() As Integer
            Get
                Return Me.isSalvageField
            End Get
            Set(ByVal value As Integer)
                Me.isSalvageField = value
            End Set
        End Property
        ' End Ravikumar Pasupuleti)-(Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc)-(6.1.1.1)

    End Class

    Public Class BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax

        Private payeeDomiciledField As Boolean

        Private payeePercentageField As Decimal

        Private payeeTaxNumberField As String

        '''<remarks/>
        Public Property PayeeDomiciled() As Boolean
            Get
                Return Me.payeeDomiciledField
            End Get
            Set(ByVal value As Boolean)
                Me.payeeDomiciledField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeePercentage() As Decimal
            Get
                Return Me.payeePercentageField
            End Get
            Set(ByVal value As Decimal)
                Me.payeePercentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeeTaxNumber() As String
            Get
                Return Me.payeeTaxNumberField
            End Get
            Set(ByVal value As String)
                Me.payeeTaxNumberField = value
            End Set
        End Property

    End Class

    Public Class BaseGetClaimPaymentTaxGroupsResponseType
        Inherits BaseResponseType

        Private taxGroupField As System.Xml.XmlElement
        Private reseultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.reseultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafield = value
            End Set
        End Property
        '''<remarks/>
        Public Property TaxGroup() As XmlElement
            Get
                Return Me.taxGroupField
            End Get
            Set(ByVal value As XmlElement)
                Me.taxGroupField = value
            End Set
        End Property

    End Class


    Public Class BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup

        Private rowField As BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroupRow

        '''<remarks/>
        Public Property Row() As BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroupRow
            Get
                Return Me.rowField
            End Get
            Set(ByVal value As BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroupRow)
                Me.rowField = value
            End Set
        End Property
    End Class


    Public Class BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroupRow

        Private codeField As String

        Private descriptionField As String

        Private isWithholdingTaxField As Boolean

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsWithholdingTax() As Boolean
            Get
                Return Me.isWithholdingTaxField
            End Get
            Set(ByVal value As Boolean)
                Me.isWithholdingTaxField = value
            End Set
        End Property
    End Class

    Public Class BaseGetClaimReceiptTaxGroupsRequestType
        Inherits BaseRequestType

        Private typeCodeField As String

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property
    End Class

    Public Class BaseGetClaimReceiptTaxGroupsResponseType
        Inherits BaseGetClaimPaymentTaxGroupsResponseType
    End Class


    Public Class BaseAdditionalClaimRelatedDetails

        Private _ListOfTaxCalculationItem As List(Of BaseTaxCalculationItemType)
        Public Property ListOfTaxCalculationItem() As List(Of BaseTaxCalculationItemType)
            Get
                Return _ListOfTaxCalculationItem
            End Get
            Set(ByVal value As List(Of BaseTaxCalculationItemType))
                _ListOfTaxCalculationItem = value
            End Set
        End Property

        Private _TaxGroupTaxBandDetails As DataView
        Public Property TaxGroupTaxBandDetails() As DataView
            Get
                Return _TaxGroupTaxBandDetails
            End Get
            Set(ByVal value As DataView)
                _TaxGroupTaxBandDetails = value
            End Set
        End Property

        Private _Claim As New BaseClaimAdditionalDetailsType
        Public Property Claim() As BaseClaimAdditionalDetailsType
            Get
                Return _Claim
            End Get
            Set(ByVal value As BaseClaimAdditionalDetailsType)
                _Claim = value
            End Set
        End Property

        Private _client As New BaseClientAdditionalDetailsType
        Public Property Client() As BaseClientAdditionalDetailsType
            Get
                Return _client
            End Get
            Set(ByVal value As BaseClientAdditionalDetailsType)
                _client = value
            End Set
        End Property

        Private _Agent As BaseAgentAdditionalDetailsType
        Public Property Agent() As BaseAgentAdditionalDetailsType
            Get
                Return _Agent
            End Get
            Set(ByVal value As BaseAgentAdditionalDetailsType)
                _Agent = value
            End Set
        End Property

        Private _transferAgent As BaseAgentAdditionalDetailsType
        Public Property TransferAgent() As BaseAgentAdditionalDetailsType
            Get
                Return _transferAgent
            End Get
            Set(ByVal value As BaseAgentAdditionalDetailsType)
                _transferAgent = value
            End Set
        End Property

        Private _ClaimPeril As New BaseClaimPerilAdditionalDetailsType
        Public Property ClaimPeril() As BaseClaimPerilAdditionalDetailsType
            Get
                Return _ClaimPeril
            End Get
            Set(ByVal value As BaseClaimPerilAdditionalDetailsType)
                _ClaimPeril = value
            End Set
        End Property

        Private _InsuranceFile As New BaseInsuranceFileAdditionalDetailsType
        Public Property InsuranceFile() As BaseInsuranceFileAdditionalDetailsType
            Get
                Return _InsuranceFile
            End Get
            Set(ByVal value As BaseInsuranceFileAdditionalDetailsType)
                _InsuranceFile = value
            End Set
        End Property

        Private _Product As New BaseProductAdditionalDetailsType
        Public Property Product() As BaseProductAdditionalDetailsType
            Get
                Return _Product
            End Get
            Set(ByVal value As BaseProductAdditionalDetailsType)
                _Product = value
            End Set
        End Property

        Private _Risk As New BaseRiskAdditionalDetailsType
        Public Property Risk() As BaseRiskAdditionalDetailsType
            Get
                Return _Risk
            End Get
            Set(ByVal value As BaseRiskAdditionalDetailsType)
                _Risk = value
            End Set
        End Property

        Private _Source As New BaseSourceAdditionalDetailsType
        Public Property Source() As BaseSourceAdditionalDetailsType
            Get
                Return _Source
            End Get
            Set(ByVal value As BaseSourceAdditionalDetailsType)
                _Source = value
            End Set
        End Property

    End Class

    Public Class BaseClaimAdditionalDetailsType

        Private _lossCurrencyKey As Integer
        Public Property LossCurrencyKey() As Integer
            Get
                Return _lossCurrencyKey
            End Get
            Set(ByVal value As Integer)
                _lossCurrencyKey = value
            End Set
        End Property

        Private _losscurrencyDescription As String
        Public Property LossCurrencyDescription() As String
            Get
                Return _losscurrencyDescription
            End Get
            Set(ByVal value As String)
                _losscurrencyDescription = value
            End Set
        End Property

        Private _lossFromdate As Date
        Public Property LossFromdate() As Date
            Get
                Return _lossFromdate
            End Get
            Set(ByVal value As Date)
                _lossFromdate = value
            End Set
        End Property

        Private _claimNumber As String
        Public Property ClaimNumber() As String
            Get
                Return _claimNumber
            End Get
            Set(ByVal value As String)
                _claimNumber = value
            End Set
        End Property

    End Class

    Public Class BaseClaimPerilAdditionalDetailsType
        Private _Description As String

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Private _classOfBusinessCode As String
        Public Property ClassOfBusinessCode() As String
            Get
                Return _classOfBusinessCode
            End Get
            Set(ByVal value As String)
                _classOfBusinessCode = value
            End Set
        End Property
        Private _classOfBusinessId As Integer

        Public Property ClassOfBusinessId() As Integer
            Get
                Return _classOfBusinessId
            End Get
            Set(ByVal value As Integer)
                _classOfBusinessId = value
            End Set
        End Property
    End Class

    Public Class BaseRiskAdditionalDetailsType
        Private _riskKey As Integer
        Public Property RiskKey() As Integer
            Get
                Return _riskKey
            End Get
            Set(ByVal value As Integer)
                _riskKey = value
            End Set
        End Property

        Private _riskTypeDescription As String
        Public Property RiskTypeDescription() As String
            Get
                Return _riskTypeDescription
            End Get
            Set(ByVal value As String)
                _riskTypeDescription = value
            End Set
        End Property

        Private _postTaxEntriesSeparately As Boolean

        Public Property PostTaxEntriesSeparately() As Boolean
            Get
                Return _postTaxEntriesSeparately
            End Get
            Set(ByVal value As Boolean)
                _postTaxEntriesSeparately = value
            End Set
        End Property
    End Class

    Public Class BaseProductAdditionalDetailsType

        Private _productKey As Integer
        Public Property ProductKey() As Integer
            Get
                Return _productKey
            End Get
            Set(ByVal value As Integer)
                _productKey = value
            End Set
        End Property

        Private _PreventsCancelledAgents As Boolean
        Public Property PreventsCancelledAgentsMakingClaimPayments() As Boolean
            Get
                Return _PreventsCancelledAgents
            End Get
            Set(ByVal value As Boolean)
                _PreventsCancelledAgents = value
            End Set
        End Property

        Private _mediaTypeMandatory As Boolean
        Public Property MediaTypeMandatory() As Boolean
            Get
                Return _mediaTypeMandatory
            End Get
            Set(ByVal value As Boolean)
                _mediaTypeMandatory = value
            End Set
        End Property

    End Class

    Public Class BaseInsuranceFileAdditionalDetailsType

        Private _insuranceFileKey As Integer
        Public Property InsuranceFileKey() As Integer
            Get
                Return _insuranceFileKey
            End Get
            Set(ByVal value As Integer)
                _insuranceFileKey = value
            End Set
        End Property

    End Class

    Public Class BaseSourceAdditionalDetailsType

        Private _sourceKey As Integer
        Public Property SourceKey() As Integer
            Get
                Return _sourceKey
            End Get
            Set(ByVal value As Integer)
                _sourceKey = value
            End Set
        End Property

        Private _CurrencyKey As Integer
        Public Property CurrencyKey() As Integer
            Get
                Return _CurrencyKey
            End Get
            Set(ByVal value As Integer)
                _CurrencyKey = value
            End Set
        End Property

    End Class

    Public Class BaseClientAdditionalDetailsType
        Inherits BasePartyDetailsAdditionalDetailsType

    End Class

    Public Class BaseAgentAdditionalDetailsType
        Inherits BasePartyDetailsAdditionalDetailsType

        Private _cancelled As Boolean
        Public Property Cancelled() As Boolean
            Get
                Return _cancelled
            End Get
            Set(ByVal value As Boolean)
                _cancelled = value
            End Set
        End Property

        Private _isInTransferMode As Boolean
        Public Property IsInTransferMode() As Boolean
            Get
                Return _isInTransferMode
            End Get
            Set(ByVal value As Boolean)
                _isInTransferMode = value
            End Set
        End Property

        Private _transferToBusinessTypeCode As String
        Public Property TransferToBusinessTypeCode() As String
            Get
                Return _transferToBusinessTypeCode
            End Get
            Set(ByVal value As String)
                _transferToBusinessTypeCode = value
            End Set
        End Property
    End Class

    Public Class BasePartyDetailsAdditionalDetailsType

        Private _partyKey As Integer
        Public Property PartyKey() As Integer
            Get
                Return _partyKey
            End Get
            Set(ByVal value As Integer)
                _partyKey = value
            End Set
        End Property
        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _accountcurrencyKey As Integer
        Public Property AccountCurrencyKey() As Integer
            Get
                Return _accountcurrencyKey
            End Get
            Set(ByVal value As Integer)
                _accountcurrencyKey = value
            End Set
        End Property
        Private _domiciledForTax As Boolean
        Public Property IsDomiciledForTax() As Boolean
            Get
                Return _domiciledForTax
            End Get
            Set(ByVal value As Boolean)
                _domiciledForTax = value
            End Set
        End Property
        Private _taxnumber As String
        Public Property TaxNumber() As String
            Get
                Return _taxnumber
            End Get
            Set(ByVal value As String)
                _taxnumber = value
            End Set
        End Property
        Private _taxpercentage As Decimal
        Public Property TaxPercentage() As Decimal
            Get
                Return _taxpercentage
            End Get
            Set(ByVal value As Decimal)
                _taxpercentage = value
            End Set
        End Property
        Private _taxexempt As Boolean
        Public Property IsTaxExempt() As Boolean
            Get
                Return _taxexempt
            End Get
            Set(ByVal value As Boolean)
                _taxexempt = value
            End Set
        End Property

    End Class

    '''<remarks/>

    Public Class BaseGetClaimDetailsRequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer
        Private FetchAllVersionAmountsField As Integer
        Private bIsRoundingUpToFourField As Boolean

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FetchAllVersionAmounts() As Integer
            Get
                Return Me.FetchAllVersionAmountsField
            End Get
            Set(ByVal value As Integer)
                Me.FetchAllVersionAmountsField = value
            End Set
        End Property

        Public Property ExclusiveLock() As Boolean

        Public Property SessionValue() As String

        '''<remarks/>
        Public Property IsRoundingUpToFour() As Boolean
            Get
                Return Me.bIsRoundingUpToFourField
            End Get
            Set(ByVal value As Boolean)
                Me.bIsRoundingUpToFourField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object) 'GAURAV

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If ClaimKey.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                "ClaimKey")
            End If
        End Sub

    End Class

    Public Class BaseFindClaimResponseType
        Inherits BaseResponseType

        Private resultDatasetField As XmlElement

        '''<remarks/>
        Public Property ResultDataset() As XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property

        Private reseultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.reseultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafield = value
            End Set
        End Property





    End Class

    '''<remarks/>
    Public Class BaseFindClaimResponseTypeClaims

        Private rowField() As BaseFindClaimResponseTypeClaimsRow

        '''<remarks/>
        <XmlElement("Row")>
        Public Property Row() As BaseFindClaimResponseTypeClaimsRow()
            Get
                Return Me.rowField
            End Get
            Set(ByVal value As BaseFindClaimResponseTypeClaimsRow())
                Me.rowField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseFindClaimResponseTypeClaimsRow

        Private insuranceFileKeyField As Integer

        Private claimKeyField As Integer

        Private claimDescriptionField As String

        Private claimNumberField As String

        Private insuranceRefField As String

        Private clientShortNameField As String

        Private productDescriptionField As String

        Private lossDateFromField As Date

        Private clientNameField As String

        Private claimStatusIDField As Integer

        Private claimHandlerDescriptionField As String

        Private insurerClaimNumberField As String

        Private clientClaimNumberField As String

        Private clientTelephoneNumberField As String

        Private clientTelephoneNumberOfficeField As String

        Private primaryCauseDescriptionField As String

        Private secondaryCauseDescriptionField As String

        Private progressStatusDescriptionField As String

        Private paymentsField As Decimal

        Private reserveField As Decimal

        Private currencyISOCodeField As String

        Private isDeletedField As Boolean

        Private isAllowedClosedClaimsField As Boolean

        'Start (PraveenGora)-(Tech Spec-UIIC WR25 - ViewClaim - FindClaim (Further Changes).doc)-(7.1.1.1)
        Private infoOnlyField As Boolean
        'End (PraveenGora)-(Tech Spec-UIIC WR25 - ViewClaim - FindClaim (Further Changes).doc)-(7.1.1.1)

        Private caseNumberField As String

        Private searchResultsCol1Field As String


        Private ClaimStatusField As String
        '''<remarks/>
        Public Property InsuranceFileKey() As Integer
            Get
                Return Me.insuranceFileKeyField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimDescription() As String
            Get
                Return Me.claimDescriptionField
            End Get
            Set(ByVal value As String)
                Me.claimDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimNumber() As String
            Get
                Return Me.claimNumberField
            End Get
            Set(ByVal value As String)
                Me.claimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceRef() As String
            Get
                Return Me.insuranceRefField
            End Get
            Set(ByVal value As String)
                Me.insuranceRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientShortName() As String
            Get
                Return Me.clientShortNameField
            End Get
            Set(ByVal value As String)
                Me.clientShortNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProductDescription() As String
            Get
                Return Me.productDescriptionField
            End Get
            Set(ByVal value As String)
                Me.productDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LossDateFrom() As Date
            Get
                Return Me.lossDateFromField
            End Get
            Set(ByVal value As Date)
                Me.lossDateFromField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientName() As String
            Get
                Return Me.clientNameField
            End Get
            Set(ByVal value As String)
                Me.clientNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimStatusID() As Integer
            Get
                Return Me.claimStatusIDField
            End Get
            Set(ByVal value As Integer)
                Me.claimStatusIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimHandlerDescription() As String
            Get
                Return Me.claimHandlerDescriptionField
            End Get
            Set(ByVal value As String)
                Me.claimHandlerDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsurerClaimNumber() As String
            Get
                Return Me.insurerClaimNumberField
            End Get
            Set(ByVal value As String)
                Me.insurerClaimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientClaimNumber() As String
            Get
                Return Me.clientClaimNumberField
            End Get
            Set(ByVal value As String)
                Me.clientClaimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientTelephoneNumber() As String
            Get
                Return Me.clientTelephoneNumberField
            End Get
            Set(ByVal value As String)
                Me.clientTelephoneNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientTelephoneNumberOffice() As String
            Get
                Return Me.clientTelephoneNumberOfficeField
            End Get
            Set(ByVal value As String)
                Me.clientTelephoneNumberOfficeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PrimaryCauseDescription() As String
            Get
                Return Me.primaryCauseDescriptionField
            End Get
            Set(ByVal value As String)
                Me.primaryCauseDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SecondaryCauseDescription() As String
            Get
                Return Me.secondaryCauseDescriptionField
            End Get
            Set(ByVal value As String)
                Me.secondaryCauseDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProgressStatusDescription() As String
            Get
                Return Me.progressStatusDescriptionField
            End Get
            Set(ByVal value As String)
                Me.progressStatusDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Payments() As Decimal
            Get
                Return Me.paymentsField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Reserve() As Decimal
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As Decimal)
                Me.reserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyISOCode() As String
            Get
                Return Me.currencyISOCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyISOCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsDeleted() As Boolean
            Get
                Return Me.isDeletedField
            End Get
            Set(ByVal value As Boolean)
                Me.isDeletedField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsAllowedClosedClaims() As Boolean
            Get
                Return Me.isAllowedClosedClaimsField
            End Get
            Set(ByVal value As Boolean)
                Me.isAllowedClosedClaimsField = value
            End Set
        End Property
        'Start (PraveenGora)-(Tech Spec-UIIC WR25 - ViewClaim - FindClaim (Further Changes).doc)-(7.1.1.1)
        '''<remarks/>
        Public Property InfoOnly() As Boolean
            Get
                Return Me.infoOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.infoOnlyField = value
            End Set
        End Property
        'End (PraveenGora)-(Tech Spec-UIIC WR25 - ViewClaim - FindClaim (Further Changes).doc)-(7.1.1.1)
        '''<remarks/>
        Public Property CaseNumber() As String
            Get
                Return Me.caseNumberField
            End Get
            Set(ByVal value As String)
                Me.caseNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SearchResultsCol1() As String
            Get
                Return Me.searchResultsCol1Field
            End Get
            Set(ByVal value As String)
                Me.searchResultsCol1Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimStatus() As String
            Get
                Return Me.ClaimStatusField
            End Get
            Set(ByVal value As String)
                Me.ClaimStatusField = value
            End Set
        End Property

    End Class
    '''<remarks/>
    Public Class BaseGenerateClaimsDocumentsRequestType
        Inherits BaseRequestType

        Private modeField As Integer

        Private claimKeyField As Integer

        Private transactionTypeField As String

        Private parameterXMLField As String

        Private outputAsHTMLField As Boolean

        Private outputAsPDFField As Boolean

        '''<remarks/>
        Public Property Mode() As Integer
            Get
                Return Me.modeField
            End Get
            Set(ByVal value As Integer)
                Me.modeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TransactionType() As String
            Get
                Return Me.transactionTypeField
            End Get
            Set(ByVal value As String)
                Me.transactionTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ParameterXML() As String
            Get
                Return Me.parameterXMLField
            End Get
            Set(ByVal value As String)
                Me.parameterXMLField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OutputAsHTML() As Boolean
            Get
                Return Me.outputAsHTMLField
            End Get
            Set(ByVal value As Boolean)
                Me.outputAsHTMLField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OutputAsPDF() As Boolean
            Get
                Return Me.outputAsPDFField
            End Get
            Set(ByVal value As Boolean)
                Me.outputAsPDFField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If Mode = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Mode")
            End If
            If String.IsNullOrEmpty(TransactionType) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "TransactionType")
            End If
        End Sub

    End Class

    Public Class BaseGetClaimPaymentTaxGroupsRequestType
        Inherits BaseRequestType

        Private partyKeyField As Integer

        Private paymentPartyTypeField As ClaimPaymentPartyTypeType

        Private advancedTaxField As BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentPartyType() As ClaimPaymentPartyTypeType
            Get
                Return Me.paymentPartyTypeField
            End Get
            Set(ByVal value As ClaimPaymentPartyTypeType)
                Me.paymentPartyTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdvancedTax() As BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax
            Get
                Return Me.advancedTaxField
            End Get
            Set(ByVal value As BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax)
                Me.advancedTaxField = value
            End Set
        End Property


        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If PaymentPartyType <> ClaimPaymentPartyTypeType.CLMPAYABLE Then
                If PartyKey = 0 Then
                    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                "PartyKey", PartyKey.ToString)
                End If

            End If

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseGetClaimRiskRequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer

        Private taskField As SAMConstants.SAMComponentAction

        '''<remarks/>
        Public Property ClaimKey() As Integer

            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Task() As SAMConstants.SAMComponentAction

            Get
                Return Me.taskField
            End Get
            Set(ByVal value As SAMConstants.SAMComponentAction)
                Me.taskField = value
            End Set
        End Property

        Private _timeStamp As Byte()
        Public Property TimeStamp() As Byte()
            Get
                Return _timeStamp
            End Get
            Set(ByVal value As Byte())
                _timeStamp = value
            End Set
        End Property

        Private _baseClaimKey As Integer
        Public Property BaseClaimKey() As Integer
            Get
                Return _baseClaimKey
            End Get
            Set(ByVal value As Integer)
                _baseClaimKey = value
            End Set
        End Property


        Private _isDataTransfer As Boolean
        Public Property IsDataTransfer() As Boolean
            Get
                Return _isDataTransfer
            End Get
            Set(ByVal value As Boolean)
                _isDataTransfer = value
            End Set
        End Property
        Public Property IgnoreIsDirty() As Boolean
        Public Overrides Sub Validate(ByRef oErrorCollection As Object) 'GAURAV

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If ClaimKey.ToString.Length = 0 Then
                oSAMErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                "ClaimKey")
            End If
        End Sub

    End Class

    Public Class BaseGetClaimsRequestType
        Inherits BaseRequestType
        Private partyKeyField As Integer

        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property
    End Class

    Public Class BaseGetClaimsResponseType
        Inherits BaseResponseType

        Private resultDatasetField As XmlElement

        '''<remarks/>
        Public Property ResultDataset() As XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseUpdateClaimRiskResponseType
        Inherits BaseResponseType

        Private timeStampField() As Byte

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class

#Region "BaseGetClaimPerilSummaryRequestType"
    'Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (11.1.1)
    '''<summary>
    ''' This class is used as request type by "GetClaimPerilSummaryMethod" in CoreSAMBusiness layer. 
    ''' It inherits BaseRequestType
    '''</summary>
    '''<remarks/>
    Public Class BaseGetClaimPerilSummaryRequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer

        Private includeTotalsField As Boolean

        Private includeTPRecoveryField As Boolean

        Private includeSalvageRecoveryField As Boolean

        Private includeReserveTypesField As Boolean

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeTotals() As Boolean
            Get
                Return Me.includeTotalsField
            End Get
            Set(ByVal value As Boolean)
                Me.includeTotalsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeTPRecovery() As Boolean
            Get
                Return Me.includeTPRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.includeTPRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeSalvageRecovery() As Boolean
            Get
                Return Me.includeSalvageRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.includeSalvageRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeReserveTypes() As Boolean
            Get
                Return Me.includeReserveTypesField
            End Get
            Set(ByVal value As Boolean)
                Me.includeReserveTypesField = value
            End Set
        End Property
        'For validation of ClaimKey
        ''' <summary>
        ''' Mandatory field check for ClaimKey
        ''' </summary>
        ''' <param name="oErrorCollection"></param>
        ''' <remarks></remarks>
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If ClaimKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                "ClaimKey")
            End If
        End Sub
    End Class
    'End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (11.1.1)
#End Region

#Region "BaseGetClaimPerilSummaryResponseType"
    'Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (11.1.2)
    '''<summary>
    ''' This class is used as response type by "GetClaimPerilSummaryMethod" in CoreSAMBusiness layer. 
    ''' It inherits BaseResponseType
    '''</summary>
    '''<remarks/>
    Public Class BaseGetClaimPerilSummaryResponseType
        Inherits BaseResponseType

        Private perilTotalsField As XmlElement

        Private tPRecoveryPerilsField As XmlElement

        Private salvageRecoveryPerilsField As XmlElement

        Private reserveTypeField() As BaseGetClaimPerilSummaryResponseTypeReserveType

        Private reseultdatafieldPerilTotals As DataSet
        '''<remarks/>
        Public Property ResultDataPerilTotals() As DataSet
            Get
                Return Me.reseultdatafieldPerilTotals
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafieldPerilTotals = value
            End Set
        End Property
        Private reseultdatafieldTPRecoveryPerils As DataSet
        '''<remarks/>
        Public Property ResultDataTPRecoveryPerils() As DataSet
            Get
                Return Me.reseultdatafieldTPRecoveryPerils
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafieldTPRecoveryPerils = value
            End Set
        End Property
        Private reseultdatafieldSalvageRecoveryPerils As DataSet
        '''<remarks/>
        Public Property ResultDataSalvageRecoveryPerils() As DataSet
            Get
                Return Me.reseultdatafieldSalvageRecoveryPerils
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafieldSalvageRecoveryPerils = value
            End Set
        End Property

        '''<remarks/>
        Public Property PerilTotals() As XmlElement
            Get
                Return Me.perilTotalsField
            End Get
            Set(ByVal value As XmlElement)
                Me.perilTotalsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPRecoveryPerils() As XmlElement
            Get
                Return Me.tPRecoveryPerilsField
            End Get
            Set(ByVal value As XmlElement)
                Me.tPRecoveryPerilsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SalvageRecoveryPerils() As XmlElement
            Get
                Return Me.salvageRecoveryPerilsField
            End Get
            Set(ByVal value As XmlElement)
                Me.salvageRecoveryPerilsField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("ReserveType")>
        Public Property ReserveType() As BaseGetClaimPerilSummaryResponseTypeReserveType()
            Get
                Return Me.reserveTypeField
            End Get
            Set(ByVal value As BaseGetClaimPerilSummaryResponseTypeReserveType())
                Me.reserveTypeField = value
            End Set
        End Property
    End Class
    'End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (11.1.2)
#End Region

#Region "BaseGetClaimPerilSummaryResponseTypeReserveType"
    'Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (11.1.3)

    '''<summary>
    ''' This class defines the object memeber of BaseGetClaimPerilSummaryResponseType class.
    '''</summary>
    '''<remarks/>
    Public Class BaseGetClaimPerilSummaryResponseTypeReserveType

        Private codeField As String

        Private descriptionField As String

        Private perilsField As XmlElement
        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Perils() As XmlElement
            Get
                Return Me.perilsField
            End Get
            Set(ByVal value As XmlElement)
                Me.perilsField = value
            End Set
        End Property
    End Class
    'End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (11.1.3)
#End Region

#Region "Coinsurer / Reinsurer"

    Public Class BaseInsurerType

        Private _RecoveryId As Integer
        Public Property RecoveryId() As Integer
            Get
                Return _RecoveryId
            End Get
            Set(ByVal value As Integer)
                _RecoveryId = value
            End Set
        End Property

        Private _ThisRecoveryAmount As Decimal
        Public Property ThisRecoveryAmount() As Decimal
            Get
                Return _ThisRecoveryAmount
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoveryAmount = value
            End Set
        End Property

        Private _ThisRecoveryTaxAmount As Decimal
        Public Property ThisRecoveryTaxAmount() As Decimal
            Get
                Return _ThisRecoveryTaxAmount
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoveryTaxAmount = value
            End Set
        End Property

        Private _RecoveryTypeDescription As String
        Public Property RecoveryTypeDescription() As String
            Get
                Return _RecoveryTypeDescription
            End Get
            Set(ByVal value As String)
                _RecoveryTypeDescription = value
            End Set
        End Property

        Private _PartyCnt As Integer
        Public Property PartyCnt() As Integer
            Get
                Return _PartyCnt
            End Get
            Set(ByVal value As Integer)
                _PartyCnt = value
            End Set
        End Property

        Private _PartyName As String
        Public Property PartyName() As String
            Get
                Return _PartyName
            End Get
            Set(ByVal value As String)
                _PartyName = value
            End Set
        End Property

        Private _SharePercentage As Decimal
        Public Property SharePercentage() As Decimal
            Get
                Return _SharePercentage
            End Get
            Set(ByVal value As Decimal)
                _SharePercentage = value
            End Set
        End Property

        Private _RecoveryToDateLC As Decimal ' IMPORTANT NB : RecoveryToDate is loaded in loss currency
        Public Property RecoveryToDateLC() As Decimal
            Get
                Return _RecoveryToDateLC
            End Get
            Set(ByVal value As Decimal)
                _RecoveryToDateLC = value
            End Set
        End Property

        Private _IsTaxShared As Integer
        Public Property IsTaxShared() As Integer
            Get
                Return _IsTaxShared
            End Get
            Set(ByVal value As Integer)
                _IsTaxShared = value
            End Set
        End Property

        Private _RIArrangementLineId As Integer
        Public Property RIArrangementLineId() As Integer
            Get
                Return _RIArrangementLineId
            End Get
            Set(ByVal value As Integer)
                _RIArrangementLineId = value
            End Set
        End Property

        Private _TreatyId As Integer
        Public Property TreatyId() As Integer
            Get
                Return _TreatyId
            End Get
            Set(ByVal value As Integer)
                _TreatyId = value
            End Set
        End Property

        Private _ThisRecoverySplitAmount As Decimal
        Public Property ThisRecoverySplitAmount() As Decimal
            Get
                Return _ThisRecoverySplitAmount
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoverySplitAmount = value
            End Set
        End Property

        Private _ThisRecoverySplitAmountLC As Decimal
        Public Property ThisRecoverySplitAmountLC() As Decimal
            Get
                Return _ThisRecoverySplitAmountLC
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoverySplitAmountLC = value
            End Set
        End Property

        Private _ThisRecoveryTaxAmountLC As Decimal
        Public Property ThisRecoveryTaxAmountLC() As Decimal
            Get
                Return _ThisRecoveryTaxAmountLC
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoveryTaxAmountLC = value
            End Set
        End Property

        Private _ThisRecoveryAmountLC As Decimal
        Public Property ThisRecoveryAmountLC() As Decimal
            Get
                Return _ThisRecoveryAmountLC
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoveryAmountLC = value
            End Set
        End Property

        Private _ReceiptToLossXRate As Decimal
        Public Property ReceiptToLossXRate() As Decimal
            Get
                Return _ReceiptToLossXRate
            End Get
            Set(ByVal value As Decimal)
                _ReceiptToLossXRate = value
            End Set
        End Property

        Private _ThisRecoverySplitTaxAmountLC As Decimal
        Public Property ThisRecoverySplitTaxAmountLC() As Decimal
            Get
                Return _ThisRecoverySplitTaxAmountLC
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoverySplitTaxAmountLC = value
            End Set
        End Property

        Private _ThisRecoverySplitTaxAmount As Decimal
        Public Property ThisRecoverySplitTaxAmount() As Decimal
            Get
                Return _ThisRecoverySplitTaxAmount
            End Get
            Set(ByVal value As Decimal)
                _ThisRecoverySplitTaxAmount = value
            End Set
        End Property

    End Class



#End Region

#Region "Public Class- BaseGetUnallocatedClaimPaymentsRequestType"
    'Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc)-(7.1.5.1)
    '''<summary>
    ''' To add  new Request types 'accountKey' and 'paymentDate'
    '''</summary>
    '''<remarks></remarks>    
    Partial Public Class BaseGetUnallocatedClaimPaymentsRequestType
        Inherits BaseRequestType

        Private accountKeyField As Integer

        Private accountKeyFieldSpecified As Boolean

        Private paymentDateField As Date

        Private paymentDateFieldSpecified As Boolean

        Private shortCodeField As String

        Private shortCodeFieldSpecified As Boolean

        Private paymentDateToField As Date
        Private paymentDateToFieldSpecified As Boolean
        '''<remarks/>
        Public Property AccountKey() As Integer
            Get
                Return Me.accountKeyField
            End Get
            Set(ByVal value As Integer)
                Me.accountKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property AccountKeySpecified() As Boolean
            Get
                Return Me.accountKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.accountKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentDate() As Date
            Get
                Return Me.paymentDateField
            End Get
            Set(ByVal value As Date)
                Me.paymentDateField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property PaymentDateSpecified() As Boolean
            Get
                Return Me.paymentDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.paymentDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ShortCode() As String
            Get
                Return Me.shortCodeField
            End Get
            Set(ByVal value As String)
                Me.shortCodeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property ShortCodeSpecified() As Boolean
            Get
                Return Me.shortCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.shortCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentDateTo() As Date
            Get
                Return Me.paymentDateToField
            End Get
            Set(ByVal value As Date)
                Me.paymentDateToField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property PaymentDateToSpecified() As Boolean
            Get
                Return Me.paymentDateToFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.paymentDateToFieldSpecified = value
            End Set
        End Property
    End Class
    'End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc)-(7.1.5.1)
#End Region
#Region "Public Class- BaseGetUnallocatedClaimPaymentsResponseType"
    'Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc)-(7.1.5.3)
    '''<summary>
    ''' This contains all the Response properties for Unallocated Claim payments
    '''</summary>
    '''<remarks></remarks>   
    Partial Public Class BaseGetUnallocatedClaimPaymentsResponseType
        Inherits BaseResponseType

        Private resultDatasetField As XmlElement

        '''<remarks/>
        Public Property ResultDataset() As XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property
        Private reseultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.reseultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafield = value
            End Set
        End Property
    End Class
    'End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc)-(7.1.5.3)
#End Region

#Region " Public Enum ClaimProcessType "
    'Start (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
    '''<summary>
    ''' It defines the structure of ClaimProcessType field in BaseGetProductClaimsWorkflowOptionsRequestType class.
    '''</summary>
    '''<remarks/>
    Public Enum ClaimProcessType

        '''<remarks/>
        None

        '''<remarks/>
        OpenClaim

        '''<remarks/>
        MaintainClaim

        '''<remarks/>
        ClaimPayment
    End Enum
    'End (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
#End Region

#Region " Public Class BaseGetProductClaimsWorkflowOptionsRequestType "
    'Start (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
    '''<summary>
    ''' It defines the structure of request type object for GetProductClaimsWorkflowOptions method in the Core SAM business layer.
    '''</summary>
    '''<remarks/>
    Public Class BaseGetProductClaimsWorkflowOptionsRequestType
        Inherits BaseRequestType

        Private productCodeField As String

        Private claimProcessTypeField As ClaimProcessType

        'Additional Field for processing the request
        Private productIDField As Integer

        '''<remarks/>
        Public Property ProductID() As Integer
            Get
                Return Me.productIDField
            End Get
            Set(ByVal value As Integer)
                Me.productIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProductCode() As String
            Get
                Return Me.productCodeField
            End Get
            Set(ByVal value As String)
                Me.productCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimProcessType() As ClaimProcessType
            Get
                Return Me.claimProcessTypeField
            End Get
            Set(ByVal value As ClaimProcessType)
                Me.claimProcessTypeField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)

            Dim samErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(Me.ProductCode) Then
                samErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                    "ProductCode")
            End If

            If Me.ClaimProcessType = ClaimProcessType.None Then
                samErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "ClaimProcessType")
            End If
        End Sub
    End Class
    'End (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
#End Region

#Region " Public Class BaseGetProductClaimsWorkflowOptionsResponseType "
    'Start (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
    '''<summary>
    ''' It defines the structure of response type object for GetProductClaimsWorkflowOptions method in the core SAM business layer.
    '''</summary>
    '''<remarks/>
    Public Class BaseGetProductClaimsWorkflowOptionsResponseType
        Inherits BaseResponseType

        Private checkUnpaidStatusField As Boolean

        Private reinsuranceRecoveryField As Boolean

        Private salvageRecoveryField As Boolean

        Private thirdPartyRecoveryField As Boolean

        Private externalClaimHandlingField As Boolean

        Private descriptionForChangeInReserveField As Boolean

        Private claimNotificationDocMessageField As Boolean

        Private generateClaimNotificationDocField As Boolean

        Private claimPaymentProcessField As Boolean

        Private checkDeferredReinsuranceField As Boolean

        Private fastTrackClaimsField As Boolean

        Private reinsurancePaymentField As Boolean

        Private descriptionForChangeInPaymentField As Boolean

        Private cashPaymentProcessField As Boolean

        Private claimPaymentDocMessageField As Boolean

        Private generateClaimPaymentDocField As Boolean

        Private makeFurtherPaymentsField As Boolean

        '''<remarks/>
        Public Property CheckUnpaidStatus() As Boolean
            Get
                Return Me.checkUnpaidStatusField
            End Get
            Set(ByVal value As Boolean)
                Me.checkUnpaidStatusField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReinsuranceRecovery() As Boolean
            Get
                Return Me.reinsuranceRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.reinsuranceRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SalvageRecovery() As Boolean
            Get
                Return Me.salvageRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.salvageRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThirdPartyRecovery() As Boolean
            Get
                Return Me.thirdPartyRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.thirdPartyRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ExternalClaimHandling() As Boolean
            Get
                Return Me.externalClaimHandlingField
            End Get
            Set(ByVal value As Boolean)
                Me.externalClaimHandlingField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DescriptionForChangeInReserve() As Boolean
            Get
                Return Me.descriptionForChangeInReserveField
            End Get
            Set(ByVal value As Boolean)
                Me.descriptionForChangeInReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimNotificationDocMessage() As Boolean
            Get
                Return Me.claimNotificationDocMessageField
            End Get
            Set(ByVal value As Boolean)
                Me.claimNotificationDocMessageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property GenerateClaimNotificationDoc() As Boolean
            Get
                Return Me.generateClaimNotificationDocField
            End Get
            Set(ByVal value As Boolean)
                Me.generateClaimNotificationDocField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentProcess() As Boolean
            Get
                Return Me.claimPaymentProcessField
            End Get
            Set(ByVal value As Boolean)
                Me.claimPaymentProcessField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CheckDeferredReinsurance() As Boolean
            Get
                Return Me.checkDeferredReinsuranceField
            End Get
            Set(ByVal value As Boolean)
                Me.checkDeferredReinsuranceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FastTrackClaims() As Boolean
            Get
                Return Me.fastTrackClaimsField
            End Get
            Set(ByVal value As Boolean)
                Me.fastTrackClaimsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReinsurancePayment() As Boolean
            Get
                Return Me.reinsurancePaymentField
            End Get
            Set(ByVal value As Boolean)
                Me.reinsurancePaymentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DescriptionForChangeInPayment() As Boolean
            Get
                Return Me.descriptionForChangeInPaymentField
            End Get
            Set(ByVal value As Boolean)
                Me.descriptionForChangeInPaymentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CashPaymentProcess() As Boolean
            Get
                Return Me.cashPaymentProcessField
            End Get
            Set(ByVal value As Boolean)
                Me.cashPaymentProcessField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPaymentDocMessage() As Boolean
            Get
                Return Me.claimPaymentDocMessageField
            End Get
            Set(ByVal value As Boolean)
                Me.claimPaymentDocMessageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property GenerateClaimPaymentDoc() As Boolean
            Get
                Return Me.generateClaimPaymentDocField
            End Get
            Set(ByVal value As Boolean)
                Me.generateClaimPaymentDocField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MakeFurtherPayments() As Boolean
            Get
                Return Me.makeFurtherPaymentsField
            End Get
            Set(ByVal value As Boolean)
                Me.makeFurtherPaymentsField = value
            End Set
        End Property
    End Class
    'End (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
#End Region

#Region "UpdateClaimReservesOrPayments"

    Partial Public Class BaseUpdateClaimReservesOrPaymentsRequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer

        Private claimPerilField() As BaseClaimPerilType

        Private claimPaymentField As BaseClaimPaymentType

        Private processTypeField As Integer

        Private timeStampField() As Byte


        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("ClaimPeril")>
        Public Property ClaimPeril() As BaseClaimPerilType()
            Get
                Return Me.claimPerilField
            End Get
            Set(ByVal value As BaseClaimPerilType())
                Me.claimPerilField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPayment() As BaseClaimPaymentType
            Get
                Return Me.claimPaymentField
            End Get
            Set(ByVal value As BaseClaimPaymentType)
                Me.claimPaymentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProcessType() As Integer
            Get
                Return Me.processTypeField
            End Get
            Set(ByVal value As Integer)
                Me.processTypeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef ErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(ErrorCollection, SAMErrorCollection)
            Dim oClaimPeril As BaseClaimPerilType

            MyBase.Validate(CObj(oSAMErrorCollection))

            If Me.ClaimKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                                  SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                          SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                               "ClaimKey")
            End If

            If ClaimPeril IsNot Nothing Then
                For Each oClaimPeril In ClaimPeril
                    oClaimPeril.Validate(CObj(oSAMErrorCollection))
                Next
            End If
        End Sub
    End Class

    Partial Public Class BaseUpdateClaimReservesOrPaymentsResponseType
        Inherits BaseResponseType

        Private timeStampField() As Byte

        Private resultingStatusField As String

        Private warningsField() As BaseClaimResponseTypeWarnings

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ResultingStatus() As String
            Get
                Return Me.resultingStatusField
            End Get
            Set(ByVal value As String)
                Me.resultingStatusField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("Warnings")>
        Public Property Warnings() As BaseClaimResponseTypeWarnings()
            Get
                Return Me.warningsField
            End Get
            Set(ByVal value As BaseClaimResponseTypeWarnings())
                Me.warningsField = value
            End Set
        End Property
    End Class

#End Region


#Region "BindClaim"
    Partial Public Class BaseBindClaimRequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer

        Private processTypeField As Integer

        Private ignoreWarningsField As Boolean

        Private externalHandlerField As Boolean

        Private closeClaimOnZeroReserveRecoveryBalanceField As Boolean

        Private CloseClaimOnFinalPaymentField As Boolean

        Private claimPaymentField As BaseClaimPaymentType

        Private timeStampField() As Byte

        Private isTPASettleDirectlyField As Boolean

        Private isTPASpecifiedField As Boolean

        Private baseCaseKeyField As Integer

        Public Property SkipSaveTransaction() As Boolean

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProcessType() As Integer
            Get
                Return Me.processTypeField
            End Get
            Set(ByVal value As Integer)
                Me.processTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IgnoreWarnings() As Boolean
            Get
                Return Me.ignoreWarningsField
            End Get
            Set(ByVal value As Boolean)
                Me.ignoreWarningsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ExternalHandler() As Boolean
            Get
                Return Me.externalHandlerField
            End Get
            Set(ByVal value As Boolean)
                Me.externalHandlerField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
            Get
                Return Me.closeClaimOnZeroReserveRecoveryBalanceField
            End Get
            Set(ByVal value As Boolean)
                Me.closeClaimOnZeroReserveRecoveryBalanceField = value

            End Set
        End Property

        '''<remarks/>
        Public Property CloseClaimOnFinalPayment() As Boolean
            Get
                Return Me.CloseClaimOnFinalPaymentField
            End Get
            Set(ByVal value As Boolean)
                Me.CloseClaimOnFinalPaymentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimPayment() As BaseClaimPaymentType
            Get
                Return Me.claimPaymentField
            End Get
            Set(ByVal value As BaseClaimPaymentType)
                Me.claimPaymentField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        Public Property TPASettleDirectly() As Boolean
            Get
                Return Me.isTPASettleDirectlyField
            End Get
            Set(ByVal value As Boolean)
                Me.isTPASettleDirectlyField = value
            End Set
        End Property
        '''<remarks/>
        Public Property BaseCaseKey() As Integer
            Get
                Return Me.baseCaseKeyField
            End Get
            Set(ByVal value As Integer)
                Me.baseCaseKeyField = value
            End Set
        End Property

        Public Property IsTPASpecified() As Boolean
            Get
                Return Me.isTPASpecifiedField
            End Get
            Set(ByVal value As Boolean)
                Me.isTPASpecifiedField = value
            End Set
        End Property

        Public Property ClaimPerilPayment As List(Of BaseClaimPaymentType)

        Public Property claimReceipt() As BaseClaimReceiptType

        Public Property NoTrans() As Boolean

        Public Overrides Sub Validate(ByRef ErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(ErrorCollection, SAMErrorCollection)

            MyBase.Validate(CObj(oSAMErrorCollection))

            If Me.ClaimKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                                  SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                          SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                               "ClaimKey")
            End If

            If Me.ProcessType = 0 Then
                oSAMErrorCollection.AddInvalidData(
                                  SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                          SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                               "ProcessType")
            End If
        End Sub
    End Class

    Partial Public Class BaseBindClaimResponseType
        Inherits BaseClaimResponseType

        Private creditedAccountKeyField As Integer

        Private creditedDocumentKeyField As Integer

        Private creditedTransdetailKeyField As Integer

        Private cashListField As BaseCashListResponseType


        '''<remarks/>
        Public Property creditedAccountKey() As Integer
            Get
                Return Me.creditedAccountKeyField
            End Get
            Set(ByVal value As Integer)
                Me.creditedAccountKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property creditedDocumentKey() As Integer
            Get
                Return Me.creditedDocumentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.creditedDocumentKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property creditedTransdetailKey() As Integer
            Get
                Return Me.creditedTransdetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.creditedTransdetailKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CashList() As BaseCashListResponseType
            Get
                Return Me.cashListField
            End Get
            Set(ByVal value As BaseCashListResponseType)
                Me.cashListField = value
            End Set
        End Property

    End Class
#End Region

#Region "Claim RI Arrangement Line"
    Partial Public Class BaseUpdateClaimRIArrangementLinesRI2007RequestType
        Inherits BaseRequestType

        Private claimRIArrangementLinesField() As BaseClaimRiskRIArrangementLineType

        '''<remarks/>
        <XmlElement("ClaimRIArrangementLines")>
        Public Property ClaimRIArrangementLines() As BaseClaimRiskRIArrangementLineType()
            Get
                Return Me.claimRIArrangementLinesField
            End Get
            Set(ByVal value As BaseClaimRiskRIArrangementLineType())
                Me.claimRIArrangementLinesField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            Dim oLine As BaseClaimRiskRIArrangementLineType

            MyBase.Validate(oErrorCollection)

            For Each oLine In ClaimRIArrangementLines

                If oLine.RIArrangementLineKey = 0 And (oLine.ActionType = RowAction.DeleteRow Or oLine.ActionType = RowAction.EditRow) Then
                    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                       "RIArrangementLineKey")
                End If
                If oLine.RIArrangementKey = 0 Then
                    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                       "RIArrangementKey")
                End If
                If String.IsNullOrEmpty(oLine.Type) = True Then
                    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                           SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                           "Type")
                End If

                If String.IsNullOrEmpty(oLine.TreatyCode) And oLine.Type <> "FX" And oLine.Type <> "F" Then
                    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                           SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                           "TreatyCode or PartyKey")
                End If

            Next

        End Sub
    End Class

    Partial Public Class BaseClaimFAXParticipants

        Private rIArrangementLineKeyField As Integer

        Private partyKeyField As Integer

        Private partyCodeField As String

        Private partyNameField As String

        Private accountTypeField As String

        Private particpationPercentageField As Single

        Private sumInsuredField As Double

        Private reserveToDateField As Double

        Private thisReserveField As Double

        Private paymentToDateField As Double

        Private thisPaymentField As Double

        Private recoverToDateField As Double

        Private recoverToDateFieldSpecified As Boolean

        Private agreementCodeField As String

        Private brokerParticipantsField() As BaseBrokerParticipants

        Private actionTypeField As RowAction

        '''<remarks/>
        Public Property RIArrangementLineKey() As Integer
            Get
                Return Me.rIArrangementLineKeyField
            End Get
            Set(ByVal value As Integer)
                Me.rIArrangementLineKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyCode() As String
            Get
                Return Me.partyCodeField
            End Get
            Set(ByVal value As String)
                Me.partyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyName() As String
            Get
                Return Me.partyNameField
            End Get
            Set(ByVal value As String)
                Me.partyNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountType() As String
            Get
                Return Me.accountTypeField
            End Get
            Set(ByVal value As String)
                Me.accountTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ParticpationPercentage() As Single
            Get
                Return Me.particpationPercentageField
            End Get
            Set(ByVal value As Single)
                Me.particpationPercentageField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SumInsured() As Double
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Double)
                Me.sumInsuredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReserveToDate() As Double
            Get
                Return Me.reserveToDateField
            End Get
            Set(ByVal value As Double)
                Me.reserveToDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisReserve() As Double
            Get
                Return Me.thisReserveField
            End Get
            Set(ByVal value As Double)
                Me.thisReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentToDate() As Double
            Get
                Return Me.paymentToDateField
            End Get
            Set(ByVal value As Double)
                Me.paymentToDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisPayment() As Double
            Get
                Return Me.thisPaymentField
            End Get
            Set(ByVal value As Double)
                Me.thisPaymentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoverToDate() As Double
            Get
                Return Me.recoverToDateField
            End Get
            Set(ByVal value As Double)
                Me.recoverToDateField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoverToDateSpecified() As Boolean
            Get
                Return Me.recoverToDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoverToDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgreementCode() As String
            Get
                Return Me.agreementCodeField
            End Get
            Set(ByVal value As String)
                Me.agreementCodeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("BrokerParticipants")>
        Public Property BrokerParticipants() As BaseBrokerParticipants()
            Get
                Return Me.brokerParticipantsField
            End Get
            Set(ByVal value As BaseBrokerParticipants())
                Me.brokerParticipantsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ActionType() As RowAction
            Get
                Return Me.actionTypeField
            End Get
            Set(ByVal value As RowAction)
                Me.actionTypeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Partial Public Class BaseClaimRiskRIArrangementLineType

        Private typeField As String

        Private treatyCodeField As String

        Private treatyIDField As Integer

        Private partyKeyField As Integer

        Private partyKeyFieldSpecified As Boolean

        Private retainedField As Double

        Private retainedFieldSpecified As Boolean

        Private defaultSharePercentField As Double

        Private thisSharePercentField As Double

        Private agreementCodeField As String

        Private priorityField As Integer

        Private numberOfLinesField As Integer

        Private lowerLimitField As Double

        Private lowerLimitFieldSpecified As Boolean

        Private lineLimitField As Double

        Private sumInsuredField As Double

        Private reserveToDateField As Double

        Private thisReserveField As Double

        Private paymentToDateField As Double

        Private thisPaymentField As Double

        Private recoverToDateField As Double

        Private recoverToDateFieldSpecified As Boolean

        Private balanceField As Double

        Private balanceFieldSpecified As Boolean

        Private incurredField As Double

        Private incurredFieldSpecified As Boolean

        Private groupingField As Integer

        Private groupingFieldSpecified As Boolean

        Private rIPlacementField As String

        Private rINameField As String

        Private isDomiciledForTaxField As Boolean

        Private isDomiciledForTaxFieldSpecified As Boolean

        Private isRIBrokerField As Boolean

        Private isRIBrokerFieldSpecified As Boolean

        Private rIArrangementLineKeyField As Integer

        Private rIArrangementKeyField As Integer

        Private cedePremiumOnlyField As Boolean

        Private reinsuranceTypeCodeField As String

        Private brokerParticipantsField() As BaseBrokerParticipants

        Private fAXParticipantsField() As BaseClaimFAXParticipants

        Private actionTypeField As RowAction

        '''<remarks/>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set(ByVal value As String)
                Me.typeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TreatyCode() As String
            Get
                Return Me.treatyCodeField
            End Get
            Set(ByVal value As String)
                Me.treatyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TreatyID() As Integer
            Get
                Return Me.treatyIDField
            End Get
            Set(ByVal value As Integer)
                Me.treatyIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property PartyKeySpecified() As Boolean
            Get
                Return Me.partyKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.partyKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Retained() As Double
            Get
                Return Me.retainedField
            End Get
            Set(ByVal value As Double)
                Me.retainedField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RetainedSpecified() As Boolean
            Get
                Return Me.retainedFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.retainedFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property DefaultSharePercent() As Double
            Get
                Return Me.defaultSharePercentField
            End Get
            Set(ByVal value As Double)
                Me.defaultSharePercentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisSharePercent() As Double
            Get
                Return Me.thisSharePercentField
            End Get
            Set(ByVal value As Double)
                Me.thisSharePercentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgreementCode() As String
            Get
                Return Me.agreementCodeField
            End Get
            Set(ByVal value As String)
                Me.agreementCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Priority() As Integer
            Get
                Return Me.priorityField
            End Get
            Set(ByVal value As Integer)
                Me.priorityField = value
            End Set
        End Property

        '''<remarks/>
        Public Property NumberOfLines() As Integer
            Get
                Return Me.numberOfLinesField
            End Get
            Set(ByVal value As Integer)
                Me.numberOfLinesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LowerLimit() As Double
            Get
                Return Me.lowerLimitField
            End Get
            Set(ByVal value As Double)
                Me.lowerLimitField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property LowerLimitSpecified() As Boolean
            Get
                Return Me.lowerLimitFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.lowerLimitFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property LineLimit() As Double
            Get
                Return Me.lineLimitField
            End Get
            Set(ByVal value As Double)
                Me.lineLimitField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SumInsured() As Double
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Double)
                Me.sumInsuredField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReserveToDate() As Double
            Get
                Return Me.reserveToDateField
            End Get
            Set(ByVal value As Double)
                Me.reserveToDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisReserve() As Double
            Get
                Return Me.thisReserveField
            End Get
            Set(ByVal value As Double)
                Me.thisReserveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentToDate() As Double
            Get
                Return Me.paymentToDateField
            End Get
            Set(ByVal value As Double)
                Me.paymentToDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisPayment() As Double
            Get
                Return Me.thisPaymentField
            End Get
            Set(ByVal value As Double)
                Me.thisPaymentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RecoverToDate() As Double
            Get
                Return Me.recoverToDateField
            End Get
            Set(ByVal value As Double)
                Me.recoverToDateField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property RecoverToDateSpecified() As Boolean
            Get
                Return Me.recoverToDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.recoverToDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Balance() As Double
            Get
                Return Me.balanceField
            End Get
            Set(ByVal value As Double)
                Me.balanceField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property BalanceSpecified() As Boolean
            Get
                Return Me.balanceFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.balanceFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Incurred() As Double
            Get
                Return Me.incurredField
            End Get
            Set(ByVal value As Double)
                Me.incurredField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property IncurredSpecified() As Boolean
            Get
                Return Me.incurredFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.incurredFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Grouping() As Integer
            Get
                Return Me.groupingField
            End Get
            Set(ByVal value As Integer)
                Me.groupingField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property GroupingSpecified() As Boolean
            Get
                Return Me.groupingFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.groupingFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RIPlacement() As String
            Get
                Return Me.rIPlacementField
            End Get
            Set(ByVal value As String)
                Me.rIPlacementField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RIName() As String
            Get
                Return Me.rINameField
            End Get
            Set(ByVal value As String)
                Me.rINameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsDomiciledForTax() As Boolean
            Get
                Return Me.isDomiciledForTaxField
            End Get
            Set(ByVal value As Boolean)
                Me.isDomiciledForTaxField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property IsDomiciledForTaxSpecified() As Boolean
            Get
                Return Me.isDomiciledForTaxFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isDomiciledForTaxFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsRIBroker() As Boolean
            Get
                Return Me.isRIBrokerField
            End Get
            Set(ByVal value As Boolean)
                Me.isRIBrokerField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property IsRIBrokerSpecified() As Boolean
            Get
                Return Me.isRIBrokerFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isRIBrokerFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RIArrangementLineKey() As Integer
            Get
                Return Me.rIArrangementLineKeyField
            End Get
            Set(ByVal value As Integer)
                Me.rIArrangementLineKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RIArrangementKey() As Integer
            Get
                Return Me.rIArrangementKeyField
            End Get
            Set(ByVal value As Integer)
                Me.rIArrangementKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CedePremiumOnly() As Boolean
            Get
                Return Me.cedePremiumOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.cedePremiumOnlyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReinsuranceTypeCode() As String
            Get
                Return Me.reinsuranceTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.reinsuranceTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("BrokerParticipants")>
        Public Property BrokerParticipants() As BaseBrokerParticipants()
            Get
                Return Me.brokerParticipantsField
            End Get
            Set(ByVal value As BaseBrokerParticipants())
                Me.brokerParticipantsField = value
            End Set
        End Property

        '''<remarks/>
        <XmlElement("FAXParticipants")>
        Public Property FAXParticipants() As BaseClaimFAXParticipants()
            Get
                Return Me.fAXParticipantsField
            End Get
            Set(ByVal value As BaseClaimFAXParticipants())
                Me.fAXParticipantsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ActionType() As RowAction
            Get
                Return Me.actionTypeField
            End Get
            Set(ByVal value As RowAction)
                Me.actionTypeField = value
            End Set
        End Property

        Public Property IsObligatory() As Boolean
    End Class

    '''<remarks/>
    Partial Public Class BaseGetClaimRIArrangementLinesRI2007RequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer

        Private arrangementKeyField As Integer

        Private modeField As Integer

        Private modeFieldSpecified As Boolean

        Private isRecoveryField As Boolean

        Private isRecoveryFieldSpecified As Boolean

        '''<remarks/>
        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ArrangementKey() As Integer
            Get
                Return Me.arrangementKeyField
            End Get
            Set(ByVal value As Integer)
                Me.arrangementKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Mode() As Integer
            Get
                Return Me.modeField
            End Get
            Set(ByVal value As Integer)
                Me.modeField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property ModeSpecified() As Boolean
            Get
                Return Me.modeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.modeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsRecovery() As Boolean
            Get
                Return Me.isRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.isRecoveryField = value
            End Set
        End Property

        '''<remarks/>
        <XmlIgnore()>
        Public Property IsRecoverySpecified() As Boolean
            Get
                Return Me.isRecoveryFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isRecoveryFieldSpecified = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If Me.claimKeyField = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "ClaimKey")
            End If
            If Me.arrangementKeyField = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "ArrangementKey")
            End If
        End Sub
    End Class

    Partial Public Class BaseUpdateClaimRIArrangementLinesRI2007ResponseType
        Inherits BaseResponseType
    End Class

    '''<remarks/>
    Partial Public Class BaseGetClaimRIArrangementLinesRI2007ResponseType
        Inherits BaseResponseType

        Private rIArrangementLinesField() As BaseClaimRiskRIArrangementLineType

        '''<remarks/>
        <XmlElement("RIArrangementLines")>
        Public Property RIArrangementLines() As BaseClaimRiskRIArrangementLineType()
            Get
                Return Me.rIArrangementLinesField
            End Get
            Set(ByVal value As BaseClaimRiskRIArrangementLineType())
                Me.rIArrangementLinesField = value
            End Set
        End Property
    End Class
#End Region

#Region "WPR-85"
    Partial Public Class BaseGenerateCashListRequestType
        Inherits BaseRequestType

        Public Property ClaimId() As Integer

    End Class

    Partial Public Class BaseGenerateCashListResponseType
        Inherits BaseResponseType
    End Class

    Partial Public Class BaseGetDefaultBankAccountWithCurrencyRequestType
        Inherits BaseRequestType

        Public Property ProductCode() As String

        Public Property MediaTypeID() As Integer

        Public Property CashListTypeID() As Integer

    End Class

    Partial Public Class BaseGetDefaultBankAccountWithCurrencyResponseType
        Inherits BaseResponseType

        Public Property ResultDataset() As XmlElement

    End Class
#End Region

#Region "CheckReserveRecovery"
    Partial Public Class BaseCheckReserveRecoveryRequestType
        Inherits BaseRequestType

        Private claimKeyField As Integer


        Public Property ClaimKey() As Integer
            Get
                Return Me.claimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.claimKeyField = value
            End Set
        End Property
    End Class

    Partial Public Class BaseCheckReserveRecoveryResponseType
        Inherits BaseResponseType

        Private dCurrentReserve As Decimal
        Private dCurrentRecovery As Decimal
        Private nClaimStatus As Integer

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
        Public Property CurrentRecovery() As Decimal
            Get
                Return Me.dCurrentRecovery
            End Get
            Set(ByVal value As Decimal)
                Me.dCurrentRecovery = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimStatus() As Integer
            Get
                Return Me.nClaimStatus
            End Get
            Set(ByVal value As Integer)
                Me.nClaimStatus = value
            End Set
        End Property

    End Class
#Region "Delete Abandon Claim"
    Public Class DeleteAbandonClaimResponseType
        Inherits BaseDeleteAbandonClaimResponseType
    End Class
    Public Class BaseDeleteAbandonClaimResponseType
        Inherits BaseResponseType

    End Class
    Public Class DeleteAbandonClaimRequestType
        Inherits BaseDeleteAbandonClaimRequestType
    End Class
    Public Class BaseDeleteAbandonClaimRequestType
        Inherits BaseRequestType

        Private nClaimKeyField As Integer
        Private nTimeStampField() As Byte

        Public Property ClaimKey() As Integer
            Get
                Return Me.nClaimKeyField
            End Get
            Set(value As Integer)
                Me.nClaimKeyField = value
            End Set
        End Property

        Public Property TimeStamp() As Byte()
            Get
                Return Me.nTimeStampField
            End Get
            Set(value As Byte())
                Me.nTimeStampField = value
            End Set
        End Property
    End Class
#End Region
#End Region
End Namespace
