Option Strict On

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS'''

#Region " Imports "

Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure
Imports Sirius.Architecture.ExceptionHandling
Imports System.Collections.Generic

'Imports SiriusFS.SAM.ServiceAgent.PMEReturnCode

#End Region

Namespace BaseImplementationTypes

    Public Class BaseCDTRequestType
        Inherits BaseRequestType

        Private claimField As BaseCDTClaimType
        Public Property Claim() As BaseCDTClaimType
            Get
                Return Me.claimField
            End Get
            Set(ByVal value As BaseCDTClaimType)
                Me.claimField = value
            End Set
        End Property

        Private useFullClaimVersioningField As Boolean
        Public Property UseFullClaimVersioning() As Boolean
            Get
                Return Me.useFullClaimVersioningField
            End Get
            Set(ByVal value As Boolean)
                Me.useFullClaimVersioningField = value
            End Set
        End Property

        Private useFullClaimVersioningFieldSpecified As Boolean
        Public Property UseFullClaimVersioningSpecified() As Boolean
            Get
                Return Me.useFullClaimVersioningFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.useFullClaimVersioningFieldSpecified = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            Claim.Validate(oErrorCollection)

        End Sub


    End Class

    Public Class BaseCDTClaimType

        Private transactionDateField As Date
        Public Property TransactionDate() As Date
            Get
                Return Me.transactionDateField
            End Get
            Set(ByVal value As Date)
                Me.transactionDateField = value
            End Set
        End Property

        Private transactionDateFieldSpecified As Boolean
        Public Property TransactionDateSpecified() As Boolean
            Get
                Return Me.transactionDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.transactionDateFieldSpecified = value
            End Set
        End Property

        Private versionNoField As Integer
        Public Property VersionNo() As Integer
            Get
                Return Me.versionNoField
            End Get
            Set(ByVal value As Integer)
                Me.versionNoField = value
            End Set
        End Property

        Private versionNoFieldSpecified As Boolean
        Public Property VersionNoSpecified() As Boolean
            Get
                Return Me.versionNoFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.versionNoFieldSpecified = value
            End Set
        End Property

        Private samStagingClaimKeyField As Integer


        Private _siriusClaimKey As Integer
        Public Property SiriusClaimKey() As Integer
            Get
                Return _siriusClaimKey
            End Get
            Set(ByVal value As Integer)
                _siriusClaimKey = value
            End Set
        End Property

        Private _siriusBaseClaimKey As Integer
        Public Property SiriusBaseClaimKey() As Integer
            Get
                Return _siriusBaseClaimKey
            End Get
            Set(ByVal value As Integer)
                _siriusBaseClaimKey = value
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

        Private siriusInsuranceFileKeyField As Integer

        Private siriusRiskKeyField As Integer

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

        Private commentsField As String

        Private claimVersionDescriptionField As String

        Private currencyCodeField As String

        Private underwritingYearCodeField As String

        Private xMLDATASETField As String

        Private claimPerilField() As BaseCDTClaimPerilType

        Private claimReinsuranceField As BaseCDTClaimReinsuranceType

        Private claimReinsuranceFieldForDTU As BaseCDTClaimReinsuranceTypeForDTU

        Private claimNumberField As String

        Public Property ClaimNumber() As String
            Get
                Return claimNumberField
            End Get
            Set(ByVal value As String)
                claimNumberField = value
            End Set
        End Property

        Public Property SAMStagingClaimKey() As Integer
            Get
                Return Me.sAMStagingClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimKeyField = value
            End Set
        End Property


        Public Property SiriusInsuranceFileKey() As Integer
            Get
                Return Me.siriusInsuranceFileKeyField
            End Get
            Set(ByVal value As Integer)
                Me.siriusInsuranceFileKeyField = value
            End Set
        End Property


        Public Property SiriusRiskKey() As Integer
            Get
                Return Me.siriusRiskKeyField
            End Get
            Set(ByVal value As Integer)
                Me.siriusRiskKeyField = value
            End Set
        End Property


        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property


        Public Property ProgressStatusCode() As String
            Get
                Return Me.progressStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.progressStatusCodeField = value
            End Set
        End Property


        Public Property PrimaryCauseCode() As String
            Get
                Return Me.primaryCauseCodeField
            End Get
            Set(ByVal value As String)
                Me.primaryCauseCodeField = value
            End Set
        End Property

        Public Property LossFromDate() As Date
            Get
                Return Me.lossFromDateField
            End Get
            Set(ByVal value As Date)
                Me.lossFromDateField = value
            End Set
        End Property

        Public Property ReportedDate() As Date
            Get
                Return Me.reportedDateField
            End Get
            Set(ByVal value As Date)
                Me.reportedDateField = value
            End Set
        End Property

        Public Property HandlerCode() As String
            Get
                Return Me.handlerCodeField
            End Get
            Set(ByVal value As String)
                Me.handlerCodeField = value
            End Set
        End Property

        Public Property InfoOnly() As Boolean
            Get
                Return Me.infoOnlyField
            End Get
            Set(ByVal value As Boolean)
                Me.infoOnlyField = value
            End Set
        End Property

        Public Property LikelyClaim() As Boolean
            Get
                Return Me.likelyClaimField
            End Get
            Set(ByVal value As Boolean)
                Me.likelyClaimField = value
            End Set
        End Property

        Public Property SecondaryCauseCode() As String
            Get
                Return Me.secondaryCauseCodeField
            End Get
            Set(ByVal value As String)
                Me.secondaryCauseCodeField = value
            End Set
        End Property

        Public Property CatastropheCode() As String
            Get
                Return Me.catastropheCodeField
            End Get
            Set(ByVal value As String)
                Me.catastropheCodeField = value
            End Set
        End Property

        Public Property LossToDate() As Date
            Get
                Return Me.lossToDateField
            End Get
            Set(ByVal value As Date)
                Me.lossToDateField = value
            End Set
        End Property

        Public Property LossToDateSpecified() As Boolean
            Get
                Return Me.lossToDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.lossToDateFieldSpecified = value
            End Set
        End Property

        Public Property Location() As String
            Get
                Return Me.locationField
            End Get
            Set(ByVal value As String)
                Me.locationField = value
            End Set
        End Property

        Public Property TownCode() As String
            Get
                Return Me.townCodeField
            End Get
            Set(ByVal value As String)
                Me.townCodeField = value
            End Set
        End Property

        Public Property Comments() As String
            Get
                Return Me.commentsField
            End Get
            Set(ByVal value As String)
                Me.commentsField = value
            End Set
        End Property

        Public Property ClaimVersionDescription() As String
            Get
                Return Me.claimVersionDescriptionField
            End Get
            Set(ByVal value As String)
                Me.claimVersionDescriptionField = value
            End Set
        End Property

        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        Public Property UnderwritingYearCode() As String
            Get
                Return Me.underwritingYearCodeField
            End Get
            Set(ByVal value As String)
                Me.underwritingYearCodeField = value
            End Set
        End Property

        Public Property XMLDATASET() As String
            Get
                Return Me.xMLDATASETField
            End Get
            Set(ByVal value As String)
                Me.xMLDATASETField = value
            End Set
        End Property

        Public Property ClaimPeril() As BaseCDTClaimPerilType()
            Get
                Return Me.claimPerilField
            End Get
            Set(ByVal value As BaseCDTClaimPerilType())
                Me.claimPerilField = value
            End Set
        End Property

        Public Property ClaimReinsurance() As BaseCDTClaimReinsuranceType
            Get
                Return Me.claimReinsuranceField
            End Get
            Set(ByVal value As BaseCDTClaimReinsuranceType)
                Me.claimReinsuranceField = value
            End Set
        End Property

        Public Property ClaimReinsuranceForDTU() As BaseCDTClaimReinsuranceTypeForDTU
            Get
                Return Me.claimReinsuranceFieldForDTU
            End Get
            Set(ByVal value As BaseCDTClaimReinsuranceTypeForDTU)
                Me.claimReinsuranceFieldForDTU = value
            End Set
        End Property


        Public Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If SiriusInsuranceFileKey = 0 Then
                oSAMErrorCollection.AddInvalidData( _
                    SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                 "SiriusInsuranceFileKey")
            End If

            If SiriusRiskKey = 0 Then
                oSAMErrorCollection.AddInvalidData( _
                    SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                 "SiriusRiskKey")
            End If

            If String.IsNullOrEmpty(ClaimNumber) Then
                oSAMErrorCollection.AddInvalidData( _
                    SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                 "ClaimNumber")
            End If

        End Sub

    End Class



    Public Class BaseCDTClaimPerilType

        Private samStagingBaseClaimPerilKeyField As Integer

        Private sAMStagingClaimPerilKeyField As Integer

        Private typeCodeField As String

        Private descriptionField As String

        Private claimPaymentField() As BaseCDTClaimPaymentType

        Private claimReceiptField() As BaseCDTClaimReceiptType

        Private reserveField() As BaseCDTReserveType

        Private recoveryField() As BaseCDTRecoveryType

        Private _siriusClaimPerilKey As Integer
        Public Property SiriusClaimPerilKey() As Integer
            Get
                Return _siriusClaimPerilKey
            End Get
            Set(ByVal value As Integer)
                _siriusClaimPerilKey = value
            End Set
        End Property

        Private _siriusBaseClaimPerilKey As Integer
        Public Property SiriusBaseClaimPerilKey() As Integer
            Get
                Return _siriusBaseClaimPerilKey
            End Get
            Set(ByVal value As Integer)
                _siriusBaseClaimPerilKey = value
            End Set
        End Property

        Public Property SAMStagingBaseClaimPerilKey() As Integer
            Get
                Return Me.samStagingBaseClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.samStagingBaseClaimPerilKeyField = value
            End Set
        End Property

        Public Property SAMStagingClaimPerilKey() As Integer
            Get
                Return Me.sAMStagingClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimPerilKeyField = value
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


        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property



        Public Property ClaimPayment() As BaseCDTClaimPaymentType()
            Get
                Return Me.claimPaymentField
            End Get
            Set(ByVal value As BaseCDTClaimPaymentType())
                Me.claimPaymentField = value
            End Set
        End Property



        Public Property ClaimReceipt() As BaseCDTClaimReceiptType()
            Get
                Return Me.claimReceiptField
            End Get
            Set(ByVal value As BaseCDTClaimReceiptType())
                Me.claimReceiptField = value
            End Set
        End Property



        Public Property Reserve() As BaseCDTReserveType()
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As BaseCDTReserveType())
                Me.reserveField = value
            End Set
        End Property


        Public Property Recovery() As BaseCDTRecoveryType()
            Get
                Return Me.recoveryField
            End Get
            Set(ByVal value As BaseCDTRecoveryType())
                Me.recoveryField = value
            End Set
        End Property
    End Class



    Public Class BaseCDTClaimPaymentType

        Private sAMStagingClaimPaymentKeyField As Integer

        Private partyKeyField As Integer

        Private paymentPartyTypeField As ClaimPaymentPartyTypeType

        Private currencyCodeField As String

        Private claimPaymentItemField() As BaseCDTClaimPaymentItemType

        Private payeeField As BaseClaimPayeeType

        Private claimReinsuranceField As BaseCDTClaimReinsuranceType

        Private claimReinsuranceFieldForDTU As BaseCDTClaimReinsuranceTypeForDTU

        Private transactionDateField As Date

        Public Property TransactionDate() As Date
            Get
                Return transactionDateField
            End Get
            Set(ByVal value As Date)
                transactionDateField = value
            End Set
        End Property

        Private transactionDateFieldSpecified As Boolean

        Public Property TransactionDateSpecified() As Boolean
            Get
                Return transactionDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                transactionDateFieldSpecified = value
            End Set
        End Property

        Public Property SAMStagingClaimPaymentKey() As Integer
            Get
                Return Me.sAMStagingClaimPaymentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimPaymentKeyField = value
            End Set
        End Property


        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property


        Public Property PaymentPartyType() As ClaimPaymentPartyTypeType
            Get
                Return Me.paymentPartyTypeField
            End Get
            Set(ByVal value As ClaimPaymentPartyTypeType)
                Me.paymentPartyTypeField = value
            End Set
        End Property


        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property



        Public Property ClaimPaymentItem() As BaseCDTClaimPaymentItemType()
            Get
                Return Me.claimPaymentItemField
            End Get
            Set(ByVal value As BaseCDTClaimPaymentItemType())
                Me.claimPaymentItemField = value
            End Set
        End Property


        Public Property Payee() As BaseClaimPayeeType
            Get
                Return Me.payeeField
            End Get
            Set(ByVal value As BaseClaimPayeeType)
                Me.payeeField = value
            End Set
        End Property


        Public Property ClaimReinsurance() As BaseCDTClaimReinsuranceType
            Get
                Return Me.claimReinsuranceField
            End Get
            Set(ByVal value As BaseCDTClaimReinsuranceType)
                Me.claimReinsuranceField = value
            End Set
        End Property

        Public Property ClaimReinsuranceForDTU() As BaseCDTClaimReinsuranceTypeForDTU
            Get
                Return Me.claimReinsuranceFieldForDTU
            End Get
            Set(ByVal value As BaseCDTClaimReinsuranceTypeForDTU)
                Me.claimReinsuranceFieldForDTU = value
            End Set
        End Property
    End Class

    Public Class BaseCDTClaimPaymentItemType

        Private sAMStagingClaimPaymentItemKeyField As Integer

        Private reserveTypeCodeField As String

        Private taxGroupCodeField As String

        Private paymentAmountField As Decimal

        Private reverseExcessField As Boolean

        Private _siriusBaseReserveKey As Integer

        Public Property SiriusBaseReserveKey() As Integer
            Get
                Return _siriusBaseReserveKey
            End Get
            Set(ByVal value As Integer)
                _siriusBaseReserveKey = value
            End Set
        End Property

        Public Property SAMStagingClaimPaymentItemKey() As Integer
            Get
                Return Me.sAMStagingClaimPaymentItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimPaymentItemKeyField = value
            End Set
        End Property


        Public Property ReserveTypeCode() As String
            Get
                Return Me.reserveTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.reserveTypeCodeField = value
            End Set
        End Property


        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property


        Public Property PaymentAmount() As Decimal
            Get
                Return Me.paymentAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentAmountField = value
            End Set
        End Property


        Public Property ReverseExcess() As Boolean
            Get
                Return Me.reverseExcessField
            End Get
            Set(ByVal value As Boolean)
                Me.reverseExcessField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTClaimReinsuranceType

        Private claimRIArrangementField As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement)


        Public Property ClaimRIArrangement() As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement)
            Get
                Return Me.claimRIArrangementField
            End Get
            Set(ByVal value As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement))
                Me.claimRIArrangementField = value
            End Set
        End Property
    End Class

    Public Class BaseCDTClaimReinsuranceTypeClaimRIArrangement

        Private claimRIArrangmentLineField() As BaseCDTClaimRIArrangmentLineType

        Private sAMStagingClaimRIArrangementKeyField As Integer

        Private rIArrangementKeyField As Integer

        Private rIBandCodeField As String

        Private rIModelCodeField As String

        Private rIBandIDField As Integer

        Private rIModelIDField As Integer

        Private claimAllocationTypeField As Integer

        Private sumInsuredField As Decimal

        Private reserveField As Decimal

        Private paymentField As Decimal

        Private salvageField As Decimal

        Private recoveryField As Decimal

        Private thisReserveField As Decimal

        Private thisPaymentField As Decimal

        Private thisSalvageField As Decimal

        Private thisRecoveryField As Decimal

        Private _claimRIArrangementId As Integer
        Public Property ClaimRIArrangementId() As Integer
            Get
                Return _claimRIArrangementId
            End Get
            Set(ByVal value As Integer)
                _claimRIArrangementId = value
            End Set
        End Property

        Private _riskKey As Integer
        Public Property RiskKey() As Integer
            Get
                Return _riskKey
            End Get
            Set(ByVal value As Integer)
                _riskKey = value
            End Set
        End Property

        Private _claimKey As Integer
        Public Property ClaimKey() As Integer
            Get
                Return _claimKey
            End Get
            Set(ByVal value As Integer)
                _claimKey = value
            End Set
        End Property

        Public Property ClaimRIArrangmentLine() As BaseCDTClaimRIArrangmentLineType()
            Get
                Return Me.claimRIArrangmentLineField
            End Get
            Set(ByVal value As BaseCDTClaimRIArrangmentLineType())
                Me.claimRIArrangmentLineField = value
            End Set
        End Property


        Public Property SAMStagingClaimRIArrangementKey() As Integer
            Get
                Return Me.sAMStagingClaimRIArrangementKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimRIArrangementKeyField = value
            End Set
        End Property


        Public Property RIArrangementKey() As Integer
            Get
                Return Me.rIArrangementKeyField
            End Get
            Set(ByVal value As Integer)
                Me.rIArrangementKeyField = value
            End Set
        End Property


        Public Property RIBandCode() As String
            Get
                Return Me.rIBandCodeField
            End Get
            Set(ByVal value As String)
                Me.rIBandCodeField = value
            End Set
        End Property


        Public Property RIModelCode() As String
            Get
                Return Me.rIModelCodeField
            End Get
            Set(ByVal value As String)
                Me.rIModelCodeField = value
            End Set
        End Property


        Public Property RIBandID() As Integer
            Get
                Return rIBandIDField
            End Get
            Set(ByVal value As Integer)
                rIBandIDField = value
            End Set
        End Property


        Public Property RIModelID() As Integer
            Get
                Return rIModelIDField
            End Get
            Set(ByVal value As Integer)
                rIModelIDField = value
            End Set
        End Property


        Public Property ClaimAllocationType() As Integer
            Get
                Return Me.claimAllocationTypeField
            End Get
            Set(ByVal value As Integer)
                Me.claimAllocationTypeField = value
            End Set
        End Property


        Public Property SumInsured() As Decimal
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Decimal)
                Me.sumInsuredField = value
            End Set
        End Property


        Public Property Reserve() As Decimal
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As Decimal)
                Me.reserveField = value
            End Set
        End Property


        Public Property Payment() As Decimal
            Get
                Return Me.paymentField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentField = value
            End Set
        End Property


        Public Property Salvage() As Decimal
            Get
                Return Me.salvageField
            End Get
            Set(ByVal value As Decimal)
                Me.salvageField = value
            End Set
        End Property


        Public Property Recovery() As Decimal
            Get
                Return Me.recoveryField
            End Get
            Set(ByVal value As Decimal)
                Me.recoveryField = value
            End Set
        End Property


        Public Property ThisReserve() As Decimal
            Get
                Return Me.thisReserveField
            End Get
            Set(ByVal value As Decimal)
                Me.thisReserveField = value
            End Set
        End Property


        Public Property ThisPayment() As Decimal
            Get
                Return Me.thisPaymentField
            End Get
            Set(ByVal value As Decimal)
                Me.thisPaymentField = value
            End Set
        End Property


        Public Property ThisSalvage() As Decimal
            Get
                Return Me.thisSalvageField
            End Get
            Set(ByVal value As Decimal)
                Me.thisSalvageField = value
            End Set
        End Property


        Public Property ThisRecovery() As Decimal
            Get
                Return Me.thisRecoveryField
            End Get
            Set(ByVal value As Decimal)
                Me.thisRecoveryField = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If Me.SumInsured = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                                   "SumInsured")
            End If

            'If Me.Recovery = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Recovery")
            'End If

            'If Me.Reserve = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Reserve")
            'End If

            'If Me.Payment = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Payment")
            'End If

            'If Me.Salvage = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Salvage")
            'End If

            'If Me.ThisRecovery = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisRecovery")
            'End If

            'If Me.ThisReserve = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisReserve")
            'End If

            'If Me.ThisPayment = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisPayment")
            'End If

            'If Me.ThisSalvage = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisSalvage")
            'End If

        End Sub
    End Class



    Public Class BaseCDTClaimRIArrangmentLineType

        Private sAMStagingClaimRIArrangementLineKeyField As Integer

        Private sAMStagingClaimRIArrangementKeyField As Integer

        Private typeField As String

        Private treatyCodeField As String

        Private treatyIDField As Integer

        Private partyKeyField As Integer

        Private retainedField As Decimal

        Private defaultSharePercentField As Decimal

        Private thisSharePercentField As Decimal

        Private agreementCodeField As String

        Private priorityField As Integer

        Private numberOfLinesField As Integer

        Private lowerLimitField As Decimal

        Private lineLimitField As Decimal

        Private sumInsuredField As Decimal

        Private reserveField As Decimal

        Private paymentField As Decimal

        Private salvageField As Decimal

        Private recoveryField As Decimal

        Private thisReserveField As Decimal

        Private thisPaymentField As Decimal

        Private thisSalvageField As Decimal

        Private thisRecoveryField As Decimal

        Private participationPercentField As Decimal

        Private groupingField As Integer


        Public Property SAMStagingClaimRIArrangementLineKey() As Integer
            Get
                Return Me.sAMStagingClaimRIArrangementLineKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimRIArrangementLineKeyField = value
            End Set
        End Property


        Public Property SAMStagingClaimRIArrangementKey() As Integer
            Get
                Return Me.sAMStagingClaimRIArrangementKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimRIArrangementKeyField = value
            End Set
        End Property


        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set(ByVal value As String)
                Me.typeField = value
            End Set
        End Property


        Public Property TreatyCode() As String
            Get
                Return Me.treatyCodeField
            End Get
            Set(ByVal value As String)
                Me.treatyCodeField = value
            End Set
        End Property


        Public Property TreatyID() As Integer
            Get
                Return Me.treatyIDField
            End Get
            Set(ByVal value As Integer)
                Me.treatyIDField = value
            End Set
        End Property


        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property


        Public Property Retained() As Decimal
            Get
                Return Me.retainedField
            End Get
            Set(ByVal value As Decimal)
                Me.retainedField = value
            End Set
        End Property


        Public Property DefaultSharePercent() As Decimal
            Get
                Return Me.defaultSharePercentField
            End Get
            Set(ByVal value As Decimal)
                Me.defaultSharePercentField = value
            End Set
        End Property


        Public Property ThisSharePercent() As Decimal
            Get
                Return Me.thisSharePercentField
            End Get
            Set(ByVal value As Decimal)
                Me.thisSharePercentField = value
            End Set
        End Property


        Public Property AgreementCode() As String
            Get
                Return Me.agreementCodeField
            End Get
            Set(ByVal value As String)
                Me.agreementCodeField = value
            End Set
        End Property


        Public Property Priority() As Integer
            Get
                Return Me.priorityField
            End Get
            Set(ByVal value As Integer)
                Me.priorityField = value
            End Set
        End Property


        Public Property NumberOfLines() As Integer
            Get
                Return Me.numberOfLinesField
            End Get
            Set(ByVal value As Integer)
                Me.numberOfLinesField = value
            End Set
        End Property


        Public Property LowerLimit() As Decimal
            Get
                Return Me.lowerLimitField
            End Get
            Set(ByVal value As Decimal)
                Me.lowerLimitField = value
            End Set
        End Property


        Public Property LineLimit() As Decimal
            Get
                Return Me.lineLimitField
            End Get
            Set(ByVal value As Decimal)
                Me.lineLimitField = value
            End Set
        End Property


        Public Property SumInsured() As Decimal
            Get
                Return Me.sumInsuredField
            End Get
            Set(ByVal value As Decimal)
                Me.sumInsuredField = value
            End Set
        End Property


        Public Property Reserve() As Decimal
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As Decimal)
                Me.reserveField = value
            End Set
        End Property


        Public Property Payment() As Decimal
            Get
                Return Me.paymentField
            End Get
            Set(ByVal value As Decimal)
                Me.paymentField = value
            End Set
        End Property


        Public Property Salvage() As Decimal
            Get
                Return Me.salvageField
            End Get
            Set(ByVal value As Decimal)
                Me.salvageField = value
            End Set
        End Property


        Public Property Recovery() As Decimal
            Get
                Return Me.recoveryField
            End Get
            Set(ByVal value As Decimal)
                Me.recoveryField = value
            End Set
        End Property


        Public Property ThisReserve() As Decimal
            Get
                Return Me.thisReserveField
            End Get
            Set(ByVal value As Decimal)
                Me.thisReserveField = value
            End Set
        End Property


        Public Property ThisPayment() As Decimal
            Get
                Return Me.thisPaymentField
            End Get
            Set(ByVal value As Decimal)
                Me.thisPaymentField = value
            End Set
        End Property


        Public Property ThisSalvage() As Decimal
            Get
                Return Me.thisSalvageField
            End Get
            Set(ByVal value As Decimal)
                Me.thisSalvageField = value
            End Set
        End Property


        Public Property ThisRecovery() As Decimal
            Get
                Return Me.thisRecoveryField
            End Get
            Set(ByVal value As Decimal)
                Me.thisRecoveryField = value
            End Set
        End Property


        Public Property ParticipationPercent() As Decimal
            Get
                Return Me.participationPercentField
            End Get
            Set(ByVal value As Decimal)
                Me.participationPercentField = value
            End Set
        End Property


        Public Property Grouping() As Integer
            Get
                Return Me.groupingField
            End Get
            Set(ByVal value As Integer)
                Me.groupingField = value
            End Set
        End Property


        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If Me.SumInsured = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                                   "SumInsured")
            End If

            'If Me.Recovery = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Recovery")
            'End If

            'If Me.Reserve = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Reserve")
            'End If

            'If Me.Payment = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Payment")
            'End If

            'If Me.Salvage = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "Salvage")
            'End If

            'If Me.ThisRecovery = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisRecovery")
            'End If

            'If Me.ThisReserve = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisReserve")
            'End If

            'If Me.ThisPayment = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisPayment")
            'End If

            'If Me.ThisSalvage = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "ThisSalvage")
            'End If

        End Sub
    End Class


    Public Class BaseCDTClaimReceiptType

        Private sAMStagingClaimReceiptKeyField As Integer

        Private partyKeyField As Integer

        Private receiptPartyTypeField As ClaimReceiptPartyTypeType

        Private currencyCodeField As String

        Private claimReceiptItemField() As BaseCDTReceiptItemType

        Private payeeField As BaseClaimPayeeType

        Private claimReinsuranceField As BaseCDTClaimReinsuranceType

        Private claimReinsuranceFieldForDTU As BaseCDTClaimReinsuranceTypeForDTU

        Private isSalvageRecoveryField As Boolean
        Public Property IsSalvageRecovery() As Boolean
            Get
                Return Me.isSalvageRecoveryField
            End Get
            Set(ByVal value As Boolean)
                Me.isSalvageRecoveryField = value
            End Set
        End Property

        Private transactionDateField As Date
        Public Property TransactionDate() As Date
            Get
                Return transactionDateField
            End Get
            Set(ByVal value As Date)
                transactionDateField = value
            End Set
        End Property

        Private transactionDateFieldSpecified As Boolean

        Public Property TransactionDateSpecified() As Boolean
            Get
                Return transactionDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                transactionDateFieldSpecified = value
            End Set
        End Property

        Public Property SAMStagingClaimReceiptKey() As Integer
            Get
                Return Me.sAMStagingClaimReceiptKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimReceiptKeyField = value
            End Set
        End Property

        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        Public Property ReceiptPartyType() As ClaimReceiptPartyTypeType
            Get
                Return Me.receiptPartyTypeField
            End Get
            Set(ByVal value As ClaimReceiptPartyTypeType)
                Me.receiptPartyTypeField = value
            End Set
        End Property

        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(ByVal value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        Public Property ClaimReceiptItem() As BaseCDTReceiptItemType()
            Get
                Return Me.claimReceiptItemField
            End Get
            Set(ByVal value As BaseCDTReceiptItemType())
                Me.claimReceiptItemField = value
            End Set
        End Property

        Public Property Payee() As BaseClaimPayeeType
            Get
                Return Me.payeeField
            End Get
            Set(ByVal value As BaseClaimPayeeType)
                Me.payeeField = value
            End Set
        End Property

        Public Property ClaimReinsurance() As BaseCDTClaimReinsuranceType
            Get
                Return Me.claimReinsuranceField
            End Get
            Set(ByVal value As BaseCDTClaimReinsuranceType)
                Me.claimReinsuranceField = value
            End Set
        End Property

        Public Property ClaimReinsuranceForDTU() As BaseCDTClaimReinsuranceTypeForDTU
            Get
                Return Me.claimReinsuranceFieldForDTU
            End Get
            Set(ByVal value As BaseCDTClaimReinsuranceTypeForDTU)
                Me.claimReinsuranceFieldForDTU = value
            End Set
        End Property

    End Class

    Public Class BaseCDTReceiptItemType

        Private sAMStagingClaimReceiptItemKeyField As Integer

        Private recoveryTypeCodeField As String

        Private taxGroupCodeField As String

        Private receiptAmountField As Decimal

        Private _siriusBaseRecoveryKey As Integer
        Public Property SiriusBaseRecoveryKey() As Integer
            Get
                Return _siriusBaseRecoveryKey
            End Get
            Set(ByVal value As Integer)
                _siriusBaseRecoveryKey = value
            End Set
        End Property

        Public Property SAMStagingClaimReceiptItemKey() As Integer
            Get
                Return Me.sAMStagingClaimReceiptItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimReceiptItemKeyField = value
            End Set
        End Property

        Public Property RecoveryTypeCode() As String
            Get
                Return Me.recoveryTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.recoveryTypeCodeField = value
            End Set
        End Property


        Public Property TaxGroupCode() As String
            Get
                Return Me.taxGroupCodeField
            End Get
            Set(ByVal value As String)
                Me.taxGroupCodeField = value
            End Set
        End Property


        Public Property ReceiptAmount() As Decimal
            Get
                Return Me.receiptAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.receiptAmountField = value
            End Set
        End Property
    End Class



    Public Class BaseCDTReserveType

        Private samStagingReserveKeyField As Integer

        Private typeCodeField As String

        Private revisionAmountField As Decimal

        Private _siriusBaseReserveKey As Integer

        Public Property SiriusBaseReserveKey() As Integer
            Get
                Return _siriusBaseReserveKey
            End Get
            Set(ByVal value As Integer)
                _siriusBaseReserveKey = value
            End Set
        End Property

        Public Property SAMStagingReserveKey() As Integer
            Get
                Return Me.samStagingReserveKeyField
            End Get
            Set(ByVal value As Integer)
                Me.samStagingReserveKeyField = value
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


        Public Property RevisionAmount() As Decimal
            Get
                Return Me.revisionAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.revisionAmountField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTRecoveryType

        Private samStagingRecoveryKeyField As Integer

        Private typeCodeField As String

        Private revisionAmountField As Decimal

        Private _siriusBaseRecoveryKey As Integer

        Public Property SiriusBaseRecoveryKey() As Integer
            Get
                Return _siriusBaseRecoveryKey
            End Get
            Set(ByVal value As Integer)
                _siriusBaseRecoveryKey = value
            End Set
        End Property

        Public Property SAMStagingRecoveryKey() As Integer
            Get
                Return Me.samStagingRecoveryKeyField
            End Get
            Set(ByVal value As Integer)
                Me.samStagingRecoveryKeyField = value
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


        Public Property RevisionAmount() As Decimal
            Get
                Return Me.revisionAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.revisionAmountField = value
            End Set
        End Property
    End Class



    Public Class BaseCDTClaimTypeClaimReinsurance
        Inherits BaseCDTClaimReinsuranceType
    End Class




    Public Class BaseCDTRecoveryResponseType

        Private sAMStagingRecoveryKeyField As Object

        Private siriusRecoveryKeyField As Object


        Public Property SAMStagingRecoveryKey() As Object
            Get
                Return Me.sAMStagingRecoveryKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingRecoveryKeyField = value
            End Set
        End Property


        Public Property SiriusRecoveryKey() As Object
            Get
                Return Me.siriusRecoveryKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusRecoveryKeyField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTClaimReceiptResponseType

        Private sAMStagingClaimReceiptKeyField As Object

        Private siriusClaimReceiptKeyField As Object

        Private claimReceiptItemField() As BaseCDTClaimReceiptItemResponseType


        Public Property SAMStagingClaimReceiptKey() As Object
            Get
                Return Me.sAMStagingClaimReceiptKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingClaimReceiptKeyField = value
            End Set
        End Property


        Public Property SiriusClaimReceiptKey() As Object
            Get
                Return Me.siriusClaimReceiptKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusClaimReceiptKeyField = value
            End Set
        End Property



        Public Property ClaimReceiptItem() As BaseCDTClaimReceiptItemResponseType()
            Get
                Return Me.claimReceiptItemField
            End Get
            Set(ByVal value As BaseCDTClaimReceiptItemResponseType())
                Me.claimReceiptItemField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTClaimReceiptItemResponseType

        Private sAMStagingClaimReceiptItemKeyField As Object

        Private siriusClaimReceiptItemKeyField As Object

        Private claimRiArrangementField As BaseCDTClaimRiArrangementResponseType


        Public Property SAMStagingClaimReceiptItemKey() As Object
            Get
                Return Me.sAMStagingClaimReceiptItemKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingClaimReceiptItemKeyField = value
            End Set
        End Property


        Public Property SiriusClaimReceiptItemKey() As Object
            Get
                Return Me.siriusClaimReceiptItemKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusClaimReceiptItemKeyField = value
            End Set
        End Property


        Public Property ClaimRiArrangement() As BaseCDTClaimRiArrangementResponseType
            Get
                Return Me.claimRiArrangementField
            End Get
            Set(ByVal value As BaseCDTClaimRiArrangementResponseType)
                Me.claimRiArrangementField = value
            End Set
        End Property
    End Class



    Public Class BaseCDTClaimRiArrangementResponseType

        Private sAMStagingClaimRIArrangementKeyField As Object

        Private siriusClaimRIArrangementKeyField As Object

        Private claimRIArrangmentLineField() As BaseCDTClaimRIArrangmentLineResponseType


        Public Property SAMStagingClaimRIArrangementKey() As Object
            Get
                Return Me.sAMStagingClaimRIArrangementKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingClaimRIArrangementKeyField = value
            End Set
        End Property


        Public Property SiriusClaimRIArrangementKey() As Object
            Get
                Return Me.siriusClaimRIArrangementKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusClaimRIArrangementKeyField = value
            End Set
        End Property



        Public Property ClaimRIArrangmentLine() As BaseCDTClaimRIArrangmentLineResponseType()
            Get
                Return Me.claimRIArrangmentLineField
            End Get
            Set(ByVal value As BaseCDTClaimRIArrangmentLineResponseType())
                Me.claimRIArrangmentLineField = value
            End Set
        End Property
    End Class



    Public Class BaseCDTClaimRIArrangmentLineResponseType

        Private sAMStagingClaimRIArrangementLineKeyField As Object

        Private siriusClaimRIArrangementLineKeyField As Object


        Public Property SAMStagingClaimRIArrangementLineKey() As Object
            Get
                Return Me.sAMStagingClaimRIArrangementLineKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingClaimRIArrangementLineKeyField = value
            End Set
        End Property


        Public Property SiriusClaimRIArrangementLineKey() As Object
            Get
                Return Me.siriusClaimRIArrangementLineKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusClaimRIArrangementLineKeyField = value
            End Set
        End Property
    End Class



    Public Class BaseCDTClaimPaymentResponseType

        Private sAMStagingClaimPaymentKeyField As Object

        Private siriusClaimPaymentKeyField As Object

        Private claimPaymentItemField() As BaseCDTClaimPaymentItemResponseType


        Public Property SAMStagingClaimPaymentKey() As Object
            Get
                Return Me.sAMStagingClaimPaymentKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingClaimPaymentKeyField = value
            End Set
        End Property


        Public Property SiriusClaimPaymentKey() As Object
            Get
                Return Me.siriusClaimPaymentKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusClaimPaymentKeyField = value
            End Set
        End Property



        Public Property ClaimPaymentItem() As BaseCDTClaimPaymentItemResponseType()
            Get
                Return Me.claimPaymentItemField
            End Get
            Set(ByVal value As BaseCDTClaimPaymentItemResponseType())
                Me.claimPaymentItemField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTClaimPaymentItemResponseType

        Private sAMStagingClaimPaymentItemKeyField As Object

        Private siriusClaimPaymentItemKeyField As Object

        Private claimRiArrangementField As BaseCDTClaimRiArrangementResponseType


        Public Property SAMStagingClaimPaymentItemKey() As Object
            Get
                Return Me.sAMStagingClaimPaymentItemKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingClaimPaymentItemKeyField = value
            End Set
        End Property


        Public Property SiriusClaimPaymentItemKey() As Object
            Get
                Return Me.siriusClaimPaymentItemKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusClaimPaymentItemKeyField = value
            End Set
        End Property


        Public Property ClaimRiArrangement() As BaseCDTClaimRiArrangementResponseType
            Get
                Return Me.claimRiArrangementField
            End Get
            Set(ByVal value As BaseCDTClaimRiArrangementResponseType)
                Me.claimRiArrangementField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTReserveResponseType

        Private sAMStagingReserveKeyField As Object

        Private siriusReserveKeyField As Object


        Public Property SAMStagingReserveKey() As Object
            Get
                Return Me.sAMStagingReserveKeyField
            End Get
            Set(ByVal value As Object)
                Me.sAMStagingReserveKeyField = value
            End Set
        End Property


        Public Property SiriusReserveKey() As Object
            Get
                Return Me.siriusReserveKeyField
            End Get
            Set(ByVal value As Object)
                Me.siriusReserveKeyField = value
            End Set
        End Property
    End Class





    Public Class BaseCDTClaimPerilResponseType

        Private sAMStagingClaimPerilKeyField As Integer

        Private siriusClaimPerilKeyField As Integer

        Private reserveField() As BaseCDTReserveResponseType

        Private recoveryField() As BaseCDTRecoveryResponseType

        Private claimPaymentField() As BaseCDTClaimPaymentResponseType

        Private claimReceiptField() As BaseCDTClaimReceiptResponseType


        Public Property SAMStagingClaimPerilKey() As Integer
            Get
                Return Me.sAMStagingClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimPerilKeyField = value
            End Set
        End Property


        Public Property SiriusClaimPerilKey() As Integer
            Get
                Return Me.siriusClaimPerilKeyField
            End Get
            Set(ByVal value As Integer)
                Me.siriusClaimPerilKeyField = value
            End Set
        End Property



        Public Property Reserve() As BaseCDTReserveResponseType()
            Get
                Return Me.reserveField
            End Get
            Set(ByVal value As BaseCDTReserveResponseType())
                Me.reserveField = value
            End Set
        End Property



        Public Property Recovery() As BaseCDTRecoveryResponseType()
            Get
                Return Me.recoveryField
            End Get
            Set(ByVal value As BaseCDTRecoveryResponseType())
                Me.recoveryField = value
            End Set
        End Property



        Public Property ClaimPayment() As BaseCDTClaimPaymentResponseType()
            Get
                Return Me.claimPaymentField
            End Get
            Set(ByVal value As BaseCDTClaimPaymentResponseType())
                Me.claimPaymentField = value
            End Set
        End Property



        Public Property ClaimReceipt() As BaseCDTClaimReceiptResponseType()
            Get
                Return Me.claimReceiptField
            End Get
            Set(ByVal value As BaseCDTClaimReceiptResponseType())
                Me.claimReceiptField = value
            End Set
        End Property
    End Class


    Public Class BaseCDTClaimResponseType

        Private sAMStagingClaimKeyField As Integer

        Private siriusClaimKeyField As Integer

        Private claimPerilField() As BaseCDTClaimPerilResponseType

        Private claimRiArrangementField As BaseCDTClaimRiArrangementResponseType


        Public Property SAMStagingClaimKey() As Integer
            Get
                Return Me.sAMStagingClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimKeyField = value
            End Set
        End Property


        Public Property SiriusClaimKey() As Integer
            Get
                Return Me.siriusClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.siriusClaimKeyField = value
            End Set
        End Property



        Public Property ClaimPeril() As BaseCDTClaimPerilResponseType()
            Get
                Return Me.claimPerilField
            End Get
            Set(ByVal value As BaseCDTClaimPerilResponseType())
                Me.claimPerilField = value
            End Set
        End Property


        Public Property ClaimRiArrangement() As BaseCDTClaimRiArrangementResponseType
            Get
                Return Me.claimRiArrangementField
            End Get
            Set(ByVal value As BaseCDTClaimRiArrangementResponseType)
                Me.claimRiArrangementField = value
            End Set
        End Property
    End Class

    Public Class BaseCDTResponseType
        Inherits BaseResponseType

        Private claimField As BaseCDTClaimResponseType

        Public Property Claim() As BaseCDTClaimResponseType
            Get
                Return Me.claimField
            End Get
            Set(ByVal value As BaseCDTClaimResponseType)
                Me.claimField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Partial Public Class BaseRiskRatingSectionType

        Private ratingSectionTypeCodeField As String

        Private ratingSectionTypeIDField As Integer

        Private sequenceNumberField As Integer

        Private rateTypeCodeField As String

        Private rateTypeIDField As Integer

        Private annualRateField As Double

        Private sumInsuredField As Double

        Private sumInsuredFieldSpecified As Boolean

        Private annualPremiumField As Double

        Private annualPremiumFieldSpecified As Boolean

        Private thisPremiumField As Double

        Private thisPremiumFieldSpecified As Boolean

        Private countryCodeField As String

        Private countryIDField As Integer

        Private stateCodeField As String

        Private stateIDField As Integer

        Private originalFlagField As Boolean

        '''<remarks/>
        Public Property RatingSectionTypeCode() As String
            Get
                Return Me.ratingSectionTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.ratingSectionTypeCodeField = value
            End Set
        End Property


        '''<remarks/>
        Public Property RatingSectionTypeId() As Integer
            Get
                Return Me.ratingSectionTypeIDField
            End Get
            Set(ByVal value As Integer)
                Me.ratingSectionTypeIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SequenceNumber() As Integer
            Get
                Return Me.sequenceNumberField
            End Get
            Set(ByVal value As Integer)
                Me.sequenceNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RateTypeCode() As String
            Get
                Return Me.rateTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.rateTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RateTypeID() As Integer
            Get
                Return Me.rateTypeIDField
            End Get
            Set(ByVal value As Integer)
                Me.rateTypeIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AnnualRate() As Double
            Get
                Return Me.annualRateField
            End Get
            Set(ByVal value As Double)
                Me.annualRateField = value
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
        Public Property SumInsuredSpecified() As Boolean
            Get
                Return Me.sumInsuredFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.sumInsuredFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AnnualPremium() As Double
            Get
                Return Me.annualPremiumField
            End Get
            Set(ByVal value As Double)
                Me.annualPremiumField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AnnualPremiumSpecified() As Boolean
            Get
                Return Me.annualPremiumFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.annualPremiumFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisPremium() As Double
            Get
                Return Me.thisPremiumField
            End Get
            Set(ByVal value As Double)
                Me.thisPremiumField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThisPremiumSpecified() As Boolean
            Get
                Return Me.thisPremiumFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.thisPremiumFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property CountryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set(ByVal value As String)
                Me.countryCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CountryID() As Integer
            Get
                Return Me.countryIDField
            End Get
            Set(ByVal value As Integer)
                Me.countryIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set(ByVal value As String)
                Me.stateCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StateID() As Integer
            Get
                Return Me.stateIDField
            End Get
            Set(ByVal value As Integer)
                Me.stateIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OriginalFlag() As Boolean
            Get
                Return Me.originalFlagField
            End Get
            Set(ByVal value As Boolean)
                Me.originalFlagField = value
            End Set
        End Property

        Public Property IsPremiumOverride As Boolean = False

        Public Property OverridePremium As Double = 0.0

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(Me.RatingSectionTypeCode) = True Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                                       "RatingSectionTypeCode")
            End If

            If Me.SequenceNumber = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                                   "SequenceNumber")
            End If

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseClientDataImportRequestType
        Inherits BaseRequestType

        Private agentKeyField As Integer

        Private agentKeyFieldSpecified As Boolean

        Private itemField As BasePartyType


        'Start (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (7.1.4.1)
        Private siriusPartyKeyField As Integer

        Private siriusPartyKeyFieldSpecified As Boolean
        'End (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (7.1.4.1)





        Private partyXMLDataSetField As String

        '''<remarks/>
        Public Property PartyXMLDataSet() As String
            Get
                Return Me.partyXMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.partyXMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Integer
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.agentKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property AgentKeySpecified() As Boolean
            Get
                Return Me.agentKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.agentKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CorporateClient", GetType(BasePartyCCType)), _
         System.Xml.Serialization.XmlElementAttribute("PersonalClient", GetType(BasePartyPCType))> _
        Public Property Party() As BasePartyType
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As BasePartyType)
                Me.itemField = value
            End Set
        End Property

        'Start (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (7.1.4.1)
        '''<remarks/>
        Public Property SiriusPartyKey() As Integer
            Get
                Return Me.siriusPartyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.siriusPartyKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property SiriusPartyKeySpecified() As Boolean
            Get
                Return Me.siriusPartyKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.siriusPartyKeyFieldSpecified = value
            End Set
        End Property
        'End (Prakash C Varghese) - (Tech Spec - UIICWR52 - DTU - Duplicate Client Append.doc) - (7.1.4.1)


        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(BranchCode) Then
                oSAMErrorCollection.AddInvalidData( _
                    SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                 "BranchCode")
            End If

            If Party Is Nothing Then
                oSAMErrorCollection.AddInvalidData( _
                    SAMConstants.SAMInvalidData.MandatoryInputMissing, _
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
                                 "Party")
            End If

        End Sub

    End Class

    '''<remarks/>
    Public Class BaseClientDataImportResponseType
        Inherits BaseResponseType

        Private partyKeyField As Integer

        Private policyVersionField() As BaseClientDataImportResponseTypePolicyVersion

        Private accountsDocumentsField() As BaseClientDataImportResponseTypeAccountsDocuments

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
        Public Property PolicyVersion() As BaseClientDataImportResponseTypePolicyVersion()
            Get
                Return Me.policyVersionField
            End Get
            Set(ByVal value As BaseClientDataImportResponseTypePolicyVersion())
                Me.policyVersionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountsDocuments() As BaseClientDataImportResponseTypeAccountsDocuments()
            Get
                Return Me.accountsDocumentsField
            End Get
            Set(ByVal value As BaseClientDataImportResponseTypeAccountsDocuments())
                Me.accountsDocumentsField = value
            End Set
        End Property
    End Class

    Public Class BaseClientDataImportResponseTypePolicyVersion

        Private sAMStagingPolicyKeyField As Integer

        Private insuranceFolderKeyField As Integer

        Private insuranceFileKeyField As Integer

        Private insuranceRefField As String

        Private risksField() As BaseClientDataImportResponseTypePolicyVersionRisks

        Private claimField() As BaseClientDataImportResponseTypePolicyVersionClaim

        '''<remarks/>
        Public Property SAMStagingPolicyKey() As Integer
            Get
                Return Me.sAMStagingPolicyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingPolicyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFolderKey() As Integer
            Get
                Return Me.insuranceFolderKeyField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFolderKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileKey() As Integer
            Get
                Return Me.insuranceFileKeyField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileKeyField = value
            End Set
        End Property

        Public Property InsuranceRef() As String
            Get
                Return Me.insuranceRefField
            End Get
            Set(ByVal value As String)
                Me.insuranceRefField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Risks")> _
        Public Property Risks() As BaseClientDataImportResponseTypePolicyVersionRisks()
            Get
                Return Me.risksField
            End Get
            Set(ByVal value As BaseClientDataImportResponseTypePolicyVersionRisks())
                Me.risksField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Claim")> _
        Public Property Claim() As BaseClientDataImportResponseTypePolicyVersionClaim()
            Get
                Return Me.claimField
            End Get
            Set(ByVal value As BaseClientDataImportResponseTypePolicyVersionClaim())
                Me.claimField = value
            End Set
        End Property
    End Class

    Public Class BaseClientDataImportResponseTypePolicyVersionClaim

        Private sAMStagingClaimKeyField As Integer

        Private claimKeyField As Integer

        '''<remarks/>
        Public Property SAMStagingClaimKey() As Integer
            Get
                Return Me.sAMStagingClaimKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingClaimKeyField = value
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
    End Class

    Public Class BaseClientDataImportResponseTypePolicyVersionRisks

        Private sAMStagingRiskKeyField As Integer

        Private riskFolderKeyField As Integer

        Private riskKeyField As Integer

        '''<remarks/>
        Public Property SAMStagingRiskKey() As Integer
            Get
                Return Me.sAMStagingRiskKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingRiskKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskFolderKey() As Integer
            Get
                Return Me.riskFolderKeyField
            End Get
            Set(ByVal value As Integer)
                Me.riskFolderKeyField = value
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
    End Class

    Partial Public Class BaseClientDataImportResponseTypeAccountsDocuments

        Private documentRefField As String

        '''<remarks/>
        Public Property DocumentRef() As String
            Get
                Return Me.documentRefField
            End Get
            Set(ByVal value As String)
                Me.documentRefField = value
            End Set
        End Property
    End Class

    Partial Public Class BasePolicyDataImportRequestType
        Inherits BaseRequestType

        Private policyVersionField As BaseQuoteRiskMsgType

        '''<remarks/>
        Public Property PolicyVersion() As BaseQuoteRiskMsgType
            Get
                Return Me.policyVersionField
            End Get
            Set(ByVal value As BaseQuoteRiskMsgType)
                Me.policyVersionField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            PolicyVersion.Validate(oErrorCollection)

        End Sub

    End Class

    Partial Public Class BasePolicyDataImportResponseType
        Inherits BaseResponseType

        Private sAMStagingPolicyKeyField As Integer

        Private insuranceFolderKeyField As Integer

        Private insuranceFileKeyField As Integer

        Private risksField() As BasePolicyDataImportResponseTypeRisks

        '''<remarks/>
        Public Property SAMStagingPolicyKey() As Integer
            Get
                Return Me.sAMStagingPolicyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingPolicyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFolderKey() As Integer
            Get
                Return Me.insuranceFolderKeyField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFolderKeyField = value
            End Set
        End Property

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
        Public Property Risks() As BasePolicyDataImportResponseTypeRisks()
            Get
                Return Me.risksField
            End Get
            Set(ByVal value As BasePolicyDataImportResponseTypeRisks())
                Me.risksField = value
            End Set
        End Property
    End Class

    Partial Public Class BasePolicyDataImportResponseTypeRisks

        Private sAMStagingRiskKeyField As Integer

        Private riskFolderKeyField As Integer

        Private riskKeyField As Integer

        '''<remarks/>
        Public Property SAMStagingRiskKey() As Integer
            Get
                Return Me.sAMStagingRiskKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingRiskKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskFolderKey() As Integer
            Get
                Return Me.riskFolderKeyField
            End Get
            Set(ByVal value As Integer)
                Me.riskFolderKeyField = value
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
    End Class

    Partial Public Class BaseDocumentDataImportRequestType
        Inherits BaseRequestType

        Private documentField As BasePostDocumentRequestType

        Private sAMStagingDocumentKeyField As Integer

        '''<remarks/>
        Public Property Document() As BasePostDocumentRequestType
            Get
                Return Me.documentField
            End Get
            Set(ByVal value As BasePostDocumentRequestType)
                Me.documentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SAMStagingDocumentKey() As Integer
            Get
                Return Me.sAMStagingDocumentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.sAMStagingDocumentKeyField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            Document.Validate(oErrorCollection)

        End Sub

        Private claimRIArrangementField As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement)

        Public Property ClaimRIArrangement() As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement)
            Get
                Return Me.claimRIArrangementField
            End Get
            Set(ByVal value As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement))
                Me.claimRIArrangementField = value
            End Set
        End Property
    End Class
    Partial Public Class BaseDocumentDataImportResponseType
        Inherits BaseResponseType
    End Class


    Public Class BaseCDTClaimReinsuranceTypeForDTU

        Private claimRIArrangementField As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement)

        Public Property ClaimRIArrangement() As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement)
            Get
                Return Me.claimRIArrangementField
            End Get
            Set(ByVal value As List(Of BaseCDTClaimReinsuranceTypeClaimRIArrangement))
                Me.claimRIArrangementField = value
            End Set
        End Property

    End Class

    Partial Public Class BaseDocumentDataImportResponseType
        Inherits BaseResponseType
    End Class
End Namespace
