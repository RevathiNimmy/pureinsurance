'*************************************************************************************
'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
'Note:this is the new file which is created on 04th June 2008
'*************************************************************************************
Option Strict On

#Region " Imports "

Imports System
Imports System.IO
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure
Imports Sirius.Architecture.ExceptionHandling
Imports System.Collections.Generic

#End Region


Namespace BaseImplementationTypes

#Region "Public Class - BaseAllocationType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.9)
    ''' <summary>
    '''  This class contain the allocation details
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseAllocationType

        Private branchCodeField As String

        Private sourceIDField As Integer

        Private accountKeyField As Integer

        Private leadAllocatingTransField As BaseTransDetailType

        Private otherAllocatingTransField As BaseTransDetailType()

        Private autoAllocateField As Boolean

        Private allocationKeyField As Integer

        Private allocatedTransField As BaseAllocationDetailType()

        Private isValidatedField As Boolean

        '''<remarks/>
        Public Property isValidated() As Boolean
            Get
                Return Me.isValidatedField
            End Get
            Set(ByVal value As Boolean)
                Me.isValidatedField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AutoAllocate() As Boolean
            Get
                Return Me.autoAllocateField
            End Get
            Set(ByVal value As Boolean)
                Me.autoAllocateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BranchCode() As String
            Get
                Return Me.branchCodeField
            End Get
            Set(ByVal value As String)
                Me.branchCodeField = value
            End Set
        End Property
        '''<remarks/>
        Public Property SourceID() As Integer
            Get
                Return Me.sourceIDField
            End Get
            Set(ByVal value As Integer)
                Me.sourceIDField = value
            End Set
        End Property
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
        Public Property LeadAllocatingTrans() As BaseTransDetailType
            Get
                Return Me.leadAllocatingTransField
            End Get
            Set(ByVal value As BaseTransDetailType)
                Me.leadAllocatingTransField = value
            End Set
        End Property
        '''<remarks/>
        Public Property OtherAllocatingTrans() As BaseTransDetailType()
            Get
                Return Me.otherAllocatingTransField
            End Get
            Set(ByVal value As BaseTransDetailType())
                Me.otherAllocatingTransField = value
            End Set
        End Property
        '''<remarks/>
        Public Property AllocationKey() As Integer
            Get
                Return Me.allocationKeyField
            End Get
            Set(ByVal value As Integer)
                Me.allocationKeyField = value
            End Set
        End Property
        '''<remarks/>
        Public Property AllocatedTrans() As BaseAllocationDetailType()
            Get
                Return Me.allocatedTransField
            End Get
            Set(ByVal value As BaseAllocationDetailType())
                Me.allocatedTransField = value
            End Set
        End Property
        '''<remarks/>
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(Me.BranchCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "BranchCode")
            End If
            If SourceID = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "SourceID")
            End If
            If AccountKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "AccountKey")
            End If
            If LeadAllocatingTrans Is Nothing Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "LeadAllocatingTrans")
            End If

            Dim iCount As Integer
            Dim bFlagPositive As Boolean = False
            Dim bFlagNagative As Boolean = False

            For iCount = 0 To OtherAllocatingTrans.Length - 1
                If (OtherAllocatingTrans(iCount).Amount < 0) Then
                    bFlagNagative = True
                ElseIf (OtherAllocatingTrans(iCount).Amount > 0) Then
                    bFlagPositive = True
                End If
            Next

            If (LeadAllocatingTrans.Amount < 0) Then
                bFlagNagative = True
            ElseIf (LeadAllocatingTrans.Amount > 0) Then
                bFlagPositive = True
            End If

            If (bFlagPositive = False Or bFlagNagative = False) Then
                oSAMErrorCollection.AddInvalidData(
                               SAMConstants.SAMInvalidData.InvalidFormat,
                               "Cannot allocate without a mix of credit and debit transactions",
                               "AllocatingTrans")
            End If

        End Sub

    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.9)
#End Region
#Region "Public Class - BaseAllocationDetailType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.10)
    ''' <summary>
    '''  This class creates the reference for the class BaseTransDetailType
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseAllocationDetailType

        Private allocationDetailKeyField As Integer

        Private transactionField As BaseTransDetailType


        '''<remarks/>
        Public Property AllocationDetailKey() As Integer
            Get
                Return Me.allocationDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.allocationDetailKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Transaction() As BaseTransDetailType
            Get
                Return Me.transactionField
            End Get
            Set(ByVal value As BaseTransDetailType)
                Me.transactionField = value
            End Set
        End Property
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If AllocationDetailKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "AllocationDetailKey")
            End If
            If Transaction Is Nothing Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "Transaction")
            End If
        End Sub

    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.10)
#End Region
#Region "Public Class - BaseTransDetailType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.11)
    ''' <summary>
    '''  This class contains TransDetailKey,cashlistitemkey and amount
    '''</summary>    
    ''' <remarks></remarks>

    Partial Public Class BaseTransDetailType

        Private transDetailKeyField As Integer

        Private cashListItemKeyField As Integer

        Private amountField As Decimal

        '''<remarks/>
        Public Property TransDetailKey() As Integer
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.transDetailKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
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

        Public Property AmountCurrencyId() As Integer = 0

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If TransDetailKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "TransDetailKey")
            End If

        End Sub

    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.11)
#End Region
#Region "Partial Public Class BaseCashListResponseTypeWarnings"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.12)
    ''' <summary>
    '''While storing the warnings, two values are considered, they are Code and Description.
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseCashListResponseTypeWarnings

        Private codeField As Integer

        Private descriptionField As String

        '''<remarks/>
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
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.12)
#End Region
#Region "Partial Public Class BaseCashListResponseType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.12)
    ''' <summary>
    ''' This class will get the cashListKey,Version and timeStamp as a reponse and all the warnings that are
    ''' related to cashlist will gets added into the object "warnings"
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseCashListResponseType
        Inherits BaseResponseType

        Private cashListKeyField As Integer

        Private versionField As Integer

        Private timeStampField() As Byte

        Private warningsField() As BaseCashListResponseTypeWarnings

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
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

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warnings")>
        Public Property Warnings() As BaseCashListResponseTypeWarnings()
            Get
                Return Me.warningsField
            End Get
            Set(ByVal value As BaseCashListResponseTypeWarnings())
                Me.warningsField = value
            End Set
        End Property
    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.12)
#End Region
#Region "Public Class - BaseBankReceiptType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.8)
    ''' <summary>
    '''  This class collects the Bank Receipt related data
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseBankReceiptType

        Private bankCodeField As String

        Private payerNameField As String

        Private chequeDateField As Date

        Private bankKeyField As Integer

        'Start (Prakash C Varghese)-(PartyBank functionality)
        Private partyBankKeyField As Integer
        'End (Prakash C Varghese)-(PartyBank functionality)

        Private bankLocationField As String

        Private bankBranchField As String

        Private chequeTypeCodeField As String

        Private chequeTypeIDField As Integer

        Private chequeClearingTypeCodeField As String

        Private chequeClearingTypeIDField As Integer

        '''<remarks/>
        Public Property BankKey() As Integer
            Get
                Return Me.bankKeyField
            End Get
            Set(ByVal value As Integer)
                Me.bankKeyField = value
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
        Public Property PayerName() As String
            Get
                Return Me.payerNameField
            End Get
            Set(ByVal value As String)
                Me.payerNameField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property ChequeDate() As Date
            Get
                Return Me.chequeDateField
            End Get
            Set(ByVal value As Date)
                Me.chequeDateField = value
            End Set
        End Property

        'Start (Prakash C Varghese)-(PartyBank functionality)
        '''<remarks/>
        Public Property PartyBankKey() As Integer
            Get
                Return Me.partyBankKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyBankKeyField = value
            End Set
        End Property
        'End (Prakash C Varghese)-(PartyBank functionality)



        '''<remarks/>
        Public Property BankLocation() As String
            Get
                Return Me.bankLocationField
            End Get
            Set(ByVal value As String)
                Me.bankLocationField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankBranch() As String
            Get
                Return Me.bankBranchField
            End Get
            Set(ByVal value As String)
                Me.bankBranchField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ChequeTypeCode() As String
            Get
                Return Me.chequeTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.chequeTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ChequeTypeID() As Integer
            Get
                Return Me.chequeTypeIDField
            End Get
            Set(ByVal value As Integer)
                Me.chequeTypeIDField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ChequeClearingTypeCode() As String
            Get
                Return Me.chequeClearingTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.chequeClearingTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ChequeClearingTypeID() As Integer
            Get
                Return Me.chequeClearingTypeIDField
            End Get
            Set(ByVal value As Integer)
                Me.chequeClearingTypeIDField = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)


            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(Me.PayerName) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "PayerName")
            End If
            If Me.ChequeDate = Date.MinValue Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "ChequeDate")
            End If
        End Sub

    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.8)
#End Region
#Region " Public Class - BaseReceiptCashListItemType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.5)
    ''' <summary>
    '''  This Class contains the Properties of Receipt Cash List items 
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseReceiptCashListItemType
        Inherits BaseCoreCashListItemType

        Private typeCodeField As String

        Private statusCodeField As String

        Private creditCardField As BaseCreditCardType

        Private bankField As BaseBankReceiptType

        Private typeKeyField As Integer

        Private bankKeyField As Integer

        Private statusKeyField As Integer

        'Start (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
        Private allocationTypeField As AllocationType

        Private allocationTypeFieldSpecified As Boolean

        Private policiesField() As BaseReceiptCashListItemTypePolicies
        'End (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
        Private nDebitTransdetailKey As Integer

        '''<remarks/>
        Public Property StatusKey() As Integer
            Get
                Return Me.statusKeyField
            End Get
            Set(ByVal value As Integer)
                Me.statusKeyField = value
            End Set
        End Property
        '''<remarks/>
        Public Property TypeKey() As Integer
            Get
                Return Me.typeKeyField
            End Get
            Set(ByVal value As Integer)
                Me.typeKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankKey() As Integer
            Get
                Return Me.bankKeyField
            End Get
            Set(ByVal value As Integer)
                Me.bankKeyField = value
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
        Public Property StatusCode() As String
            Get
                Return Me.statusCodeField
            End Get
            Set(ByVal value As String)
                Me.statusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CreditCard() As BaseCreditCardType
            Get
                Return Me.creditCardField
            End Get
            Set(ByVal value As BaseCreditCardType)
                Me.creditCardField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Bank() As BaseBankReceiptType
            Get
                Return Me.bankField
            End Get
            Set(ByVal value As BaseBankReceiptType)
                Me.bankField = value
            End Set
        End Property
        'Start (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
        '''<remarks/>
        Public Property AllocationType() As AllocationType
            Get
                Return Me.allocationTypeField
            End Get
            Set(ByVal value As AllocationType)
                Me.allocationTypeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AllocationTypeSpecified() As Boolean
            Get
                Return Me.allocationTypeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.allocationTypeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Policies() As BaseReceiptCashListItemTypePolicies()
            Get
                Return Me.policiesField
            End Get
            Set(ByVal value As BaseReceiptCashListItemTypePolicies())
                Me.policiesField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DebitTransdetailKey() As Integer
            Get
                Return Me.nDebitTransdetailKey
            End Get
            Set(ByVal value As Integer)
                Me.nDebitTransdetailKey = value
            End Set
        End Property

        'End (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            MyBase.Validate(oErrorCollection)
            If String.IsNullOrEmpty(Me.TypeCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                "CashList Type Code is invalid for receipts",
                "TypeCode")
            End If
            If (Bank IsNot Nothing) Then
                Bank.Validate(oErrorCollection)
            End If
            'Commenting this code for NIA WPR11 where they dont want to store CC details
            'If (CreditCard IsNot Nothing) Then

            'CreditCard.Validate(oErrorCollection)
            'End If

        End Sub
    End Class
    'Start (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
    '''<remarks/>

    Partial Public Class BaseReceiptCashListItemTypePolicies

        Private insuranceFileKeyField As Integer

        Private documentRefField As String

        Private writeOffReasonKeyField As Integer

        Private writeOffAmountField As Decimal

        Private isCurrencyWriteOffField As Boolean

        Private amountTobeAllocatedField As Decimal

        Private bGKeyField As Integer

        Private timeStampField() As Byte
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
        Public Property DocumentRef() As String
            Get
                Return Me.documentRefField
            End Get
            Set(ByVal value As String)
                Me.documentRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WriteOffReasonKey() As Integer
            Get
                Return Me.writeOffReasonKeyField
            End Get
            Set(ByVal value As Integer)
                Me.writeOffReasonKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WriteOffAmount() As Decimal
            Get
                Return Me.writeOffAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.writeOffAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsCurrencyWriteOff() As Boolean
            Get
                Return Me.isCurrencyWriteOffField
            End Get
            Set(ByVal value As Boolean)
                Me.isCurrencyWriteOffField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmountTobeAllocated() As Decimal
            Get
                Return Me.amountTobeAllocatedField
            End Get
            Set(ByVal value As Decimal)
                Me.amountTobeAllocatedField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
        '''<remarks/>
        Public Property BGKey() As Integer
            Get
                Return Me.bGKeyField
            End Get
            Set(ByVal value As Integer)
                Me.bGKeyField = value
            End Set
        End Property
    End Class


    '''<remarks/>

    Public Enum AllocationType

        '''<remarks/>
        BankGuarantee
        '''<remarks/>
        PremiumFinance
    End Enum
    'End (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.5)
#End Region
#Region "Public Class - BaseReceiptCashListType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.2)
    ''' <summary>
    '''  This class will create the object for the class "BaseReceiptCashListItemType"
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseReceiptCashListType
        Inherits BaseCoreCashListType

        Private receiptItemField() As BaseReceiptCashListItemType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ReceiptItem")>
        Public Property ReceiptItem() As BaseReceiptCashListItemType()
            Get
                Return Me.receiptItemField
            End Get
            Set(ByVal value As BaseReceiptCashListItemType())
                Me.receiptItemField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            MyBase.Validate(oErrorCollection)
            If Me.TypeCode <> "R" Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                "CashList Type Code is invalid for receipts",
                "TypeCode")
            End If
            If (ReceiptItem IsNot Nothing) Then
                Dim iCount As Integer = 0
                For iCount = 0 To ReceiptItem.Length - 1
                    ReceiptItem(iCount).Validate(oErrorCollection)
                Next
            End If
        End Sub
    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.2)
#End Region
#Region "Public Class - BaseCoreCashListType"
    ''' <summary>
    '''  In this class for most of the codes , keys are created
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseCoreCashListType
        ''' <summary>
        ''' TransDetail key created for Cash List Transaction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TransDetailKey As Integer
        ''' <summary>
        ''' Value set during the Cash list Generation
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isValidated As Boolean
        ''' <summary>
        ''' Branch Code of the logged in user branch 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BranchCode As String
        ''' <summary>
        ''' Source Id during the Cash List Geneartion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
        ''' <summary>
        ''' Cash List Type Key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeKey As Integer
        ''' <summary>
        ''' Currency key used during the claim payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CurrencyKey As Integer
        ''' <summary>
        ''' Claim Status Key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatusKey As Integer
        ''' <summary>
        ''' Bank Account Key set during the Cash List Process
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BankAccountKey As Integer
        ''' <summary>
        ''' Cash List Key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CashListKey As Integer
        ''' <summary>
        ''' Cash List Type Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeCode As String
        ''' <summary>
        ''' Cash List Listing Date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property ListDate As Date
        ''' <summary>
        ''' Bank Account Code used during Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BankAccountCode As String
        ''' <summary>
        ''' Currency Code used during Claim Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CurrencyCode As String
        ''' <summary>
        ''' Reference if any
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Reference As String
        ''' <summary>
        ''' Claim Status Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatusCode As String
        ''' <summary>
        ''' Bank Account NAme used during Claim Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BankAccountName As String

        Public Property SubBranchCode As String

        Public Property SubBranchID As Integer


        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(Me.BranchCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "BranchCode")
            End If
            If SourceID = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "SourceID")
            End If
            If String.IsNullOrEmpty(Me.TypeCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "TypeCode")
            End If
            If String.IsNullOrEmpty(Me.BankAccountCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "BankAccountCode")
            End If

            If String.IsNullOrEmpty(Me.CurrencyCode.ToString) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "CurrencyCode")
            End If

            If Me.ListDate = Date.MinValue Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "ListDate")
            End If
            Me.isValidated = True
        End Sub
    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.3)
#End Region
#Region "Public Class - BasePaymentCashListType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.1)
    ''' <summary>
    '''  This class will create the object for the class "BasePaymentCashListItemType"
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BasePaymentCashListType
        Inherits BaseCoreCashListType


        Private paymentItemField() As BasePaymentCashListItemType

        Private isValidatedField As Boolean

        '''<remarks/>
        Public Shadows Property isValidated() As Boolean
            Get
                Return Me.isValidatedField
            End Get
            Set(ByVal value As Boolean)
                Me.isValidatedField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentItem")>
        Public Property PaymentItem() As BasePaymentCashListItemType()
            Get
                Return Me.paymentItemField
            End Get
            Set(ByVal value As BasePaymentCashListItemType())
                Me.paymentItemField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            If (MyBase.isValidated = False) Then
                MyBase.Validate(oErrorCollection)
            End If
            If String.IsNullOrEmpty(Me.TypeCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                "CashList Type Code is invalid for payments",
                "TypeCode")
            End If
            If (PaymentItem IsNot Nothing) Then
                Dim iCount As Integer = 0
                If (PaymentItem IsNot Nothing) Then
                    For iCount = 0 To PaymentItem.Length - 1
                        If (PaymentItem(iCount).isValidated = False) Then
                            PaymentItem(iCount).Validate(oErrorCollection)
                        End If
                    Next
                End If
            End If
            Me.isValidated = True
        End Sub
    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.1)
#End Region
#Region " Public Class - BaseCoreCashListItemType"
    ''' <summary>
    '''  This Class contains the Core Cash list items
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseCoreCashListItemType
        ''' <summary>
        ''' Document type set as product or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsProduceDocument As Boolean
        ''' <summary>
        ''' Bank Reference if any
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BankReference As String
        ''' <summary>
        ''' Boolean set to true during recommendation in case of Multistep Approval
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SkipPosting As Boolean
        ''' <summary>
        ''' Details of Instalment Plans
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute("InstalmentPlanDetails")>
        Public Property InstalmentPlanDetails As BaseCoreCashListItemTypeInstalmentPlanDetails()
        ''' <summary>
        ''' The Request is validated or Not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isValidated As Boolean
        ''' <summary>
        ''' details of Allocation status
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllocationDetails As BaseAllocationType
        ''' <summary>
        ''' payment Media type key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MediaTypeKey As Integer
        ''' <summary>
        ''' Key of the Account for which payment is made
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountKey As Integer
        ''' <summary>
        ''' Party Key during payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartyKey As Integer
        ''' <summary>
        ''' The status of the Claim Payment Allocation
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllocationStatusKey As Integer
        ''' <summary>
        ''' Key of the Cash List Item during Payment authorization
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CashListItemKey As Integer
        ''' <summary>
        ''' Transaction Detail key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TransDetailKey As Integer
        ''' <summary>
        ''' Details of Allocation during PAyemnt
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllocationDetailKey As Integer
        ''' <summary>
        ''' Payment Media Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MediaTypeCode As String
        ''' <summary>
        ''' Cheque Date if payment made through Cheque
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property ChequeDate As Date
        ''' <summary>
        ''' Cash List Generation Transaction Date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property TransactionDate As Date
        ''' <summary>
        ''' Party Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountShortCode As String
        ''' <summary>
        ''' PAyment Amount
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Amount As Decimal
        ''' <summary>
        ''' Code of the Allocation Status
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllocationStatusCode As String
        ''' <summary>
        ''' Payment Media Reference
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MediaReference As String
        ''' <summary>
        ''' Other Reference if any
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OurReference As String
        ''' <summary>
        ''' Any Other reference if any
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TheirReference As String
        ''' <summary>
        ''' Contact Name of the payee
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContactName As String
        ''' <summary>
        ''' Payee Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContactAddress As BaseSimpleAddressType
        ''' <summary>
        ''' Any other details
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FurtherDetails As String
        ''' <summary>
        ''' Tax Band used during Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaxBandCode As String
        ''' <summary>
        ''' Tax Amount during Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaxAmount As Decimal
        ''' <summary>
        ''' Tax Amount Specified
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TaxAmountSpecified As Boolean
        ''' <summary>
        ''' Tax BAnd Key used during the Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaxBandKey As Integer
        ''' <summary>
        ''' Tax Band Key Specified
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TaxBandKeySpecified As Boolean
        ''' <summary>
        ''' Amount_Tendered used during the Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Amount_Tendered As Decimal
        ''' <summary>
        ''' Original_Amount used during the Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Original_Amount As Decimal
        ''' <summary>
        ''' Collection_Date used during the Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Collection_Date As Date
        ''' <summary>
        ''' branch Code
        ''' </summary>
        ''' <returns></returns>
        Public Property BranchCode As String
        ''' <summary>
        ''' IsViaBulkClaimPayment used during the Payment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsViaBulkClaimPayment As Boolean
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If TransactionDate = Date.MinValue Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "TransactionDate")
            End If
            If String.IsNullOrEmpty(Me.MediaTypeCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "MediaTypeCode")
            End If
            If String.IsNullOrEmpty(Me.AccountShortCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "AccountShortCode")
            End If
            If Me.Amount = 0.0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "Amount")
            End If

            Me.isValidated = True
        End Sub
    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.6)
#End Region
#Region " Public Class - BaseCoreCashListItemTypeInstalmentPlanDetails"
    'Start (Sriram p)Tech Spec - PGR013 - SAM Cash Receipt.doc sec(7.2.3.1)
    ''' <summary>
    '''  This Class contains the Properties of BaseCoreCashListItemTypeInstalmentPlanDetails class
    '''</summary>    
    ''' <remarks></remarks>

    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429")>
    Partial Public Class BaseCoreCashListItemTypeInstalmentPlanDetails

        Private financePlanKeyField As Integer

        Private financePlanVersionField As Integer

        Private instalmentDetailsField() As BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails

        '''<remarks/>
        Public Property FinancePlanKey() As Integer
            Get
                Return Me.financePlanKeyField
            End Get
            Set(ByVal value As Integer)
                Me.financePlanKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FinancePlanVersion() As Integer
            Get
                Return Me.financePlanVersionField
            End Get
            Set(ByVal value As Integer)
                Me.financePlanVersionField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InstalmentDetails")>
        Public Property InstalmentDetails() As BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails()
            Get
                Return Me.instalmentDetailsField
            End Get
            Set(ByVal value As BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails())
                Me.instalmentDetailsField = value
            End Set
        End Property
    End Class
    'End (Sriram p)Tech Spec - PGR013 - SAM Cash Receipt.doc sec(7.2.3.1)
#End Region
#Region " Public Class - BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails"
    'Start (Sriram p)Tech Spec - PGR013 - SAM Cash Receipt.doc sec(7.2.3.1)
    ''' <summary>
    '''  This Class contains the Properties of BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails class
    '''</summary>    
    ''' <remarks></remarks>

    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42"),
     System.SerializableAttribute(),
     System.Diagnostics.DebuggerStepThroughAttribute(),
     System.ComponentModel.DesignerCategoryAttribute("code"),
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429")>
    Partial Public Class BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails

        Private instalmentNumberField As Integer

        Private amountField As Decimal

        Private ipfInstalmentIDField As Integer

        Public Property IsPartialPayment As Boolean

        Public Property IsWriteOffPayment As Boolean

        Public Property WriteOffReasonID As Integer

        Public Property OverPaymentWriteOffAmount As Double

        Public Property ActualAmount As Double
        '''<remarks/>
        Public Property InstalmentNumber() As Integer
            Get
                Return Me.instalmentNumberField
            End Get
            Set(ByVal value As Integer)
                Me.instalmentNumberField = value
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

        '''<remarks/>
        Public Property iPFInstalmentID() As Integer
            Get
                Return Me.ipfInstalmentIDField
            End Get
            Set(ByVal value As Integer)
                Me.ipfInstalmentIDField = value
            End Set
        End Property
    End Class


    'End (Sriram p)Tech Spec - PGR013 - SAM Cash Receipt.doc sec(7.2.3.1)
#End Region
#Region "Public Class - BasePaymentCashListItemType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.4)
    ''' <summary>
    '''  This class  contains the properties of Payment CashListItem 
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BasePaymentCashListItemType
        Inherits BaseCoreCashListItemType

        Private typeCodeField As String

        Private statusCodeField As String

        Private creditCardField As BaseCreditCardType

        Private bankField As BaseBankPaymentType

        Private TypeKeyField As Integer

        Private statusKeyField As Integer

        Private bankKeyField As Integer

        Private isValidatedField As Boolean

        Private userIdField As Integer

        Private userNameField As String

        Private policiesField() As BasePaymentCashListItemTypePolicies

        '''<remarks/>
        Public Shadows Property isValidated() As Boolean
            Get
                Return Me.isValidatedField
            End Get
            Set(ByVal value As Boolean)
                Me.isValidatedField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StatusKey() As Integer
            Get
                Return Me.statusKeyField
            End Get
            Set(ByVal value As Integer)
                Me.statusKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeKey() As Integer
            Get
                Return Me.TypeKeyField
            End Get
            Set(ByVal value As Integer)
                Me.TypeKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankKey() As Integer
            Get
                Return Me.bankKeyField
            End Get
            Set(ByVal value As Integer)
                Me.bankKeyField = value
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
        Public Property StatusCode() As String
            Get
                Return Me.statusCodeField
            End Get
            Set(ByVal value As String)
                Me.statusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CreditCard() As BaseCreditCardType
            Get
                Return Me.creditCardField
            End Get
            Set(ByVal value As BaseCreditCardType)
                Me.creditCardField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Bank() As BaseBankPaymentType
            Get
                Return Me.bankField
            End Get
            Set(ByVal value As BaseBankPaymentType)
                Me.bankField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserId() As Integer
            Get
                Return Me.userIdField
            End Get
            Set(ByVal value As Integer)
                Me.userIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("Policies")>
        Public Property Policies() As BasePaymentCashListItemTypePolicies()
            Get
                Return Me.policiesField
            End Get
            Set(ByVal value As BasePaymentCashListItemTypePolicies())
                Me.policiesField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            If (MyBase.isValidated = False) Then
                MyBase.Validate(oErrorCollection)
            End If

            If String.IsNullOrEmpty(Me.TypeCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "TypeCode")
            End If
            If creditCardField IsNot Nothing Then
                If (creditCardField.isValidated = False) Then
                    creditCardField.Validate(oErrorCollection)

                End If
            End If

            Me.isValidated = True

        End Sub
    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.4)
#End Region
#Region "Public Class - BaseBankPaymentType"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.7)
    ''' <summary>
    '''  This class collects BankPayment related details
    '''</summary>    
    ''' <remarks></remarks>
    Partial Public Class BaseBankPaymentType

        Private payeeNameField As String

        Private accountCodeField As String

        Private branchCodeField As String

        Private expiryDateField As Date

        Private expiryDateFieldSpecified As Boolean

        Private reference1Field As String

        Private reference2Field As String

        Private partyBankKeyField As Integer

        Private sBICField As String

        Private sIBANField As String



        '''<remarks/>
        Public Property PayeeName() As String
            Get
                Return Me.payeeNameField
            End Get
            Set(ByVal value As String)
                Me.payeeNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountCode() As String
            Get
                Return Me.accountCodeField
            End Get
            Set(ByVal value As String)
                Me.accountCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BranchCode() As String
            Get
                Return Me.branchCodeField
            End Get
            Set(ByVal value As String)
                Me.branchCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property ExpiryDate() As Date
            Get
                Return Me.expiryDateField
            End Get
            Set(ByVal value As Date)
                Me.expiryDateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpiryDateSpecified() As Boolean
            Get
                Return Me.expiryDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.expiryDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Reference1() As String
            Get
                Return Me.reference1Field
            End Get
            Set(ByVal value As String)
                Me.reference1Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property Reference2() As String
            Get
                Return Me.reference2Field
            End Get
            Set(ByVal value As String)
                Me.reference2Field = value
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

    End Class
    'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.3.7)
#End Region
#Region "Public Class- BaseGetPaymentCashListItemsRequestType"
    'Start (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Items.doc) - (7.1.4.1) 
    '''<summary>
    ''' This class BaseGetPaymentCashListItemsRequestType contains all the members and its properties defined here
    '''</summary>
    '''<remarks></remarks>
    Partial Public Class BaseGetPaymentCashListItemsRequestType
        Inherits BaseRequestType
        ''' <summary>
        ''' Cash List Key sent during the PAyment Cash List Items Request
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CashListKey As Integer

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If CashListKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, "CashListKey")
            End If
        End Sub
    End Class
    'End (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Items.doc) - (7.1.4.1) 
#End Region
#Region "Public Class- BaseGetPaymentCashListItemsResponseType"
    'Start (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Items.doc) - (7.1.4.3) 
    '''<summary>
    ''' This class BaseGetPaymentCashListItemsResponseType contains all the members and its properties defined here
    '''</summary>
    '''<remarks></remarks>
    Partial Public Class BaseGetPaymentCashListItemsResponseType
        Inherits BaseResponseType
        Private resultDatasetField As System.Xml.XmlElement

        '''<remarks/>
        Public Property ResultDataset() As System.Xml.XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
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


    'End (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Items.doc) - (7.1.4.3) 
#End Region
#Region "Public Class- BaseGetReceiptCashListDetailsRequestType"
    'Start (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Details.doc) - (7.1.4.1)
    '''<summary>
    ''' This class BaseGetReceiptCashListDetailsRequestType contains all the members and its properties defined here
    '''</summary>
    '''<remarks></remarks>

    Partial Public Class BaseGetReceiptCashListDetailsRequestType
        Inherits BaseRequestType

        Private cashListKeyField As Integer

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If CashListKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, "CashListKey")
            End If
        End Sub
    End Class

    'End (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Details.doc) - (7.1.4.1)
#End Region
#Region "Public Class- BaseGetReceiptCashListDetailsResponseType"
    'Start (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Details.doc) - (7.1.4.3)
    '''<summary>
    ''' This class BaseGetReceiptCashListDetailsResponseType contains all the members and its properties defined here
    '''</summary>
    '''<remarks></remarks>

    Partial Public Class BaseGetReceiptCashListDetailsResponseType
        Inherits BaseResponseType

        Private receiptCashListField As BaseReceiptCashListType

        '''<remarks/>
        Public Property ReceiptCashList() As BaseReceiptCashListType
            Get
                Return Me.receiptCashListField
            End Get
            Set(ByVal value As BaseReceiptCashListType)
                Me.receiptCashListField = value
            End Set
        End Property
    End Class



    'End (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Details.doc) - (7.1.4.3)
#End Region

#Region "BaseCreatePaymentCashListWithItemsResponseType"
    'Start (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.3)  
    '''<summary>
    ''' This Class inherits the class BaseResponseType and return the CashListKey of the new CashList
    '''</summary>
    Partial Public Class BaseCreatePaymentCashListWithItemsResponseType
        Inherits BaseResponseType

        Private cashListKeyField As Integer

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property


        Private cashListItemField() As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem
        '''<remarks/>
        Public Property CashListItem() As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem()
            Get
                Return Me.cashListItemField
            End Get
            Set(ByVal value As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem())
                Me.cashListItemField = value
            End Set
        End Property


    End Class
    'End (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.3)  

    Partial Public Class BaseCreatePaymentCashListWithItemsResponseTypeCashListItem

        Private cashListItemKeyField As Integer

        Private transDetailKeyField As Integer
        Private bAutoAllocatePaymentSuccessful As Boolean
        Private accountShortCodeField As String
       

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TransDetailKey() As Integer
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.transDetailKeyField = value
            End Set
        End Property

        Public Property AutoAllocatePaymentSuccessful() As Boolean
            Get
                Return Me.bAutoAllocatePaymentSuccessful
            End Get
            Set(ByVal value As Boolean)
                Me.bAutoAllocatePaymentSuccessful = value
            End Set
        End Property
        Public Property AccountShortCode() As String
            Get
                Return Me.accountShortCodeField
            End Get
            Set(ByVal value As String)
                Me.accountShortCodeField = value
            End Set
        End Property

        Public Property DocumentRef As String 

        Public Property DocumentCode As String 

    End Class

#End Region
#Region "BaseCreatePaymentCashListWithItemsRequestType"
    'Start (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.1)  
    '''<summary>
    ''' This class inherits the class BaseRequestType and is used to pass the CashList and Cash list item details.
    '''</summary>
    Partial Public Class BaseCreatePaymentCashListWithItemsRequestType
        Inherits BaseRequestType

        Private paymentCashListField As BasePaymentCashListType

        '''<remarks/>
        Public Property PaymentCashList() As BasePaymentCashListType
            Get
                Return Me.paymentCashListField
            End Get
            Set(ByVal value As BasePaymentCashListType)
                Me.paymentCashListField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            PaymentCashList.Validate(oErrorCollection)



        End Sub
    End Class
    'End (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.1)  
#End Region
#Region "BaseCreatePaymentCashListItemRequestType"
    ''Start (Saurabh Agrawal) - (UIIC WR62 – Cash Cheque Receipt – Create Payment Cash List Item) - (7.1.4.1)
    '''<summary>
    ''' It inherits the class BaseRequestType.
    '''</summary>

    Partial Public Class BaseCreatePaymentCashListItemRequestType
        Inherits BaseRequestType

        Private cashListKeyField As Integer

        Private paymentItemField() As BasePaymentCashListItemType

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentItem() As BasePaymentCashListItemType()
            Get
                Return Me.paymentItemField
            End Get
            Set(ByVal value As BasePaymentCashListItemType())
                Me.paymentItemField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            MyBase.Validate(oErrorCollection)
            If CashListKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "CashListKey")
            End If

            If PaymentItem IsNot Nothing Then
                For Each oCashListItem As BasePaymentCashListItemType In PaymentItem
                    oCashListItem.Validate(oErrorCollection)
                Next
            End If

        End Sub
    End Class

    '' End (Saurabh Agrawal) - (UIIC WR62 – Cash Cheque Receipt – Create Payment Cash List Item) - (7.1.4.1)
#End Region
#Region "BaseCreatePaymentCashListItemResponseType"
    ''Start (Saurabh Agrawal) - (UIIC WR62 – Cash Cheque Receipt – Create Payment Cash List Item) - (7.1.4.3) 
    '''<summary>
    ''' It inherits the class BaseResponseType.
    '''</summary>
    Partial Public Class BaseCreatePaymentCashListItemResponseType
        Inherits BaseResponseType

        Private cashListItemKeyField() As Integer

        '''<remarks/>

        Public Property CashListItemKey() As Integer()
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer())
                Me.cashListItemKeyField = value
            End Set
        End Property
        'Start (Sriram P)As per the gap  analysis on 19082008 as confirmted  by rahul 


        Private transDetailKeyField() As Integer

        '''<remarks/>
        Public Property TransDetailKey() As Integer()
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer())
                Me.transDetailKeyField = value
            End Set
        End Property
        '   End (Sriram P)As per the gap  analysis on 19082008 as confirmted  by rahul 
    End Class
    ''End (Saurabh Agrawal) - (UIIC WR62 – Cash Cheque Receipt – Create Payment Cash List Item) - (7.1.4.3) 
#End Region

#Region "Public Class - BaseGetReceiptCashListItemDetailsRequestType"
    'Start (Udaya Bhaskar K ) -(Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Item Details) -(7.1.4.1)
    '''<summary>
    ''' This class is auto generated from BaseTypeBV2.xsd. 
    '''</summary>
    Partial Public Class BaseGetReceiptCashListItemDetailsRequestType
        Inherits BaseRequestType

        Private cashListItemKeyField As Integer

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)
            MyBase.Validate(oErrorCollection)

            If Me.CashListItemKey <= 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                      SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                      "CashListItemKey")
            End If

        End Sub
    End Class
    'End (Udaya Bhaskar K ) -(Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Item Details) -(7.1.4.1)
#End Region

#Region "Public Class - BaseGetReceiptCashListItemDetailsResponseType"
    'Start (Udaya Bhaskar K ) -(Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Item Details) -(7.1.4.3)
    '''<summary>
    ''' This class is auto generated from BaseTypeBV2.xsd. 
    '''</summary>

    Partial Public Class BaseGetReceiptCashListItemDetailsResponseType
        Inherits BaseResponseType

        Private cashListReceiptField As BaseReceiptCashListItemType

        '''<remarks/>
        Public Property CashListReceipt() As BaseReceiptCashListItemType
            Get
                Return Me.cashListReceiptField
            End Get
            Set(ByVal value As BaseReceiptCashListItemType)
                Me.cashListReceiptField = value
            End Set
        End Property
    End Class
    'End (Udaya Bhaskar K ) -(Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Item Details) -(7.1.4.3)
#End Region


#Region "Public Class - BaseCreateReceiptCashListWithItemsResponseType "
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.3)
    '''<summary>
    ''' This class is auto generated from BaseTypesBV2.xsd
    '''</summary>
    '''<remarks/>
    Partial Public Class BaseCreateReceiptCashListWithItemsResponseType
        Inherits BaseResponseType
      
        Private cashListKeyField As Integer

        Private cashListItemField() As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CashListItem")>
        Public Property CashListItem() As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem()
            Get
                Return Me.cashListItemField
            End Get
            Set(ByVal value As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem())
                Me.cashListItemField = value
            End Set
        End Property
    End Class
    'End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.3)
#End Region

#Region "Public Class - BaseCreateReceiptCashListWithItemsResponseTypeCashListItem "
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.3)
    '''<summary>
    ''' This class is auto generated from BaseTypesBV2.xsd
    '''</summary>
    '''<remarks/>
    Partial Public Class BaseCreateReceiptCashListWithItemsResponseTypeCashListItem

        Private cashListItemKeyField As Integer

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
            End Set
        End Property
        'Start (Sriram P)As per the gap  analysis on 24082008 as confirmed  by rahul 

        Private transDetailKeyField As Integer

        '''<remarks/>
        Public Property TransDetailKey() As Integer
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.transDetailKeyField = value
            End Set

        End Property
        'End (Sriram P)As per the gap  analysis on 24082008 as confirmed  by rahul 

        Private accountShortCodeField As String

        Public Property AccountShortCode() As String
            Get
                Return Me.accountShortCodeField
            End Get
            Set(ByVal value As String)
                Me.accountShortCodeField = value
            End Set
        End Property

        Public Property DocumentRef As String

        Public Property DocumentCode As String

        Public Property AutoAllocatePaymentSuccessful As Boolean

    End Class
    'End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.3)
#End Region

#Region "Public Class - BaseCreateReceiptCashListWithItemsRequestType "
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.1)
    '''<summary>
    ''' This class is auto generated from  BaseTypesBV2.xsd
    '''</summary>
    '''<remarks/>    
    Partial Public Class BaseCreateReceiptCashListWithItemsRequestType
        Inherits BaseRequestType

        Private receiptCashListField As BaseReceiptCashListType

        '''<remarks/>
        Public Property ReceiptCashList() As BaseReceiptCashListType
            Get
                Return Me.receiptCashListField
            End Get
            Set(ByVal value As BaseReceiptCashListType)
                Me.receiptCashListField = value
            End Set
        End Property
    End Class
    'End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.1)
#End Region

#Region "Public Class - BaseCreateReceiptCashListItemRequestType "
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.1)
    '''<summary>
    ''' This class is auto generated from BaseTypesBV2.xsd
    '''</summary>
    '''<remarks/>

    Partial Public Class BaseCreateReceiptCashListItemRequestType
        Inherits BaseRequestType

        Private cashListKeyField As Integer

        Private receiptCashListItemField() As BaseReceiptCashListItemType

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ReceiptCashListItem")>
        Public Property ReceiptCashListItem() As BaseReceiptCashListItemType()
            Get
                Return Me.receiptCashListItemField
            End Get
            Set(ByVal value As BaseReceiptCashListItemType())
                Me.receiptCashListItemField = value
            End Set
        End Property
    End Class
    'End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.1)
#End Region

#Region "Public Class - BaseCreateReceiptCashListItemResponseType "
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.3)
    '''<summary>
    ''' This class is auto generated from BaseTypesBV2.xsd
    '''</summary>
    '''<remarks/>   
    Partial Public Class BaseCreateReceiptCashListItemResponseType
        Inherits BaseResponseType

        Private cashListItemField() As BaseCreateReceiptCashListItemResponseTypeCashListItem

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CashListItem")>
        Public Property CashListItem() As BaseCreateReceiptCashListItemResponseTypeCashListItem()
            Get
                Return Me.cashListItemField
            End Get
            Set(ByVal value As BaseCreateReceiptCashListItemResponseTypeCashListItem())
                Me.cashListItemField = value
            End Set
        End Property
    End Class
    'End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.3)
#End Region
#Region "Public Class - BaseCreateReceiptCashListItemResponseTypeCashListItem "
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.3)
    '''<summary>
    ''' This class is auto generated from SAMForInsuranceBV2.xsd
    '''</summary>
    '''<remarks/>

    Partial Public Class BaseCreateReceiptCashListItemResponseTypeCashListItem

        Private cashListItemKeyField As Integer

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
            End Set
        End Property
        ' Start (Sriram P)As per the gap  analysis on 23082008 as confirmted  by rahul

        Private transDetailKeyField As Integer

        '''<remarks/>
        Public Property TransDetailKey() As Integer
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.transDetailKeyField = value
            End Set

        End Property

        Private accountShortCodeField As String

        Public Property AccountShortCode() As String
            Get
                Return Me.accountShortCodeField
            End Get
            Set(ByVal value As String)
                Me.accountShortCodeField = value
            End Set
        End Property

        'End (Sriram P)As per the gap  analysis on 23082008 as confirmted  by rahul
        'Start (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
        Private insuranceFileKeyField() As Integer

        Private allocationStatusField() As String
        'End (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)

        'Start (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InsuranceFileKey")>
        Public Property InsuranceFileKey() As Integer()
            Get
                Return Me.insuranceFileKeyField
            End Get
            Set(ByVal value As Integer())
                Me.insuranceFileKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AllocationStatus")>
        Public Property AllocationStatus() As String()
            Get
                Return Me.allocationStatusField
            End Get
            Set(ByVal value As String())
                Me.allocationStatusField = value
            End Set
        End Property
        'End (Udaya Bhaskar K) - (Tech Spec - UIICWR6 - Allocation Against Policy)
    End Class
    'End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.3)
#End Region

#Region "Public Class - BaseGetPaymentCashListDetailsRequestType"
    'Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details.doc)-(7.1.4.1)
    '''<summary>
    ''' To add  new Request types
    '''</summary>
    '''<remarks></remarks>    
    Partial Public Class BaseGetPaymentCashListDetailsRequestType
        Inherits BaseRequestType

        Private cashListKeyField As Integer

        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)
            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            If CashListKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "CashListKey")
            End If
        End Sub
    End Class
    'End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details.doc)-(7.1.4.1)
#End Region
#Region "Public Class - BaseGetPaymentCashListDetailsResponseType"
    'Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details.doc)-(7.1.4.3)
    '''<summary>
    ''' This class contains the response objects
    '''</summary>
    '''<remarks></remarks>    
    Partial Public Class BaseGetPaymentCashListDetailsResponseType
        Inherits BaseResponseType

        Private paymentCashListField As BasePaymentCashListType

        '''<remarks/>
        Public Property PaymentCashList() As BasePaymentCashListType
            Get
                Return Me.paymentCashListField
            End Get
            Set(ByVal value As BasePaymentCashListType)
                Me.paymentCashListField = value
            End Set
        End Property
    End Class
    'End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details.doc)-(7.1.4.3)
#End Region

#Region "Public Class - BaseGetPaymentCashListItemDetailsRequestType"
    'Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Item Details.doc)-(7.1.4.1)
    '''<summary>
    ''' To add  new Request types
    '''</summary>
    '''<remarks></remarks>    
    Partial Public Class BaseGetPaymentCashListItemDetailsRequestType
        Inherits BaseRequestType

        Private cashListItemKeyField As Integer

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)
            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            If CashListItemKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "CashListItemKey")
            End If
        End Sub
    End Class
    'End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Item Details.doc)-(7.1.4.1)
#End Region
#Region "Public Class - BaseGetPaymentCashListItemDetailsResponseType"
    'Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details.doc)-(7.1.4.3)
    '''<summary>
    ''' This class contains the response objects
    '''</summary>
    '''<remarks></remarks>
    Partial Public Class BaseGetPaymentCashListItemDetailsResponseType
        Inherits BaseResponseType

        Private cashListPaymentField As BasePaymentCashListItemType

        '''<remarks/>
        Public Property CashListPayment() As BasePaymentCashListItemType
            Get
                Return Me.cashListPaymentField
            End Get
            Set(ByVal value As BasePaymentCashListItemType)
                Me.cashListPaymentField = value
            End Set
        End Property
    End Class
    'End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details.doc)-(7.1.4.3)
#End Region

#Region "Public Class BaseGetReceiptCashListItemsRequestType"
    'Start (Muthukumari,Abhishek) - (Tech Spec - UIIC WR62 – Cash Cheque Receipt – Get Receipt Cash List Items.doc) - (7.1.4.1)
    '''<summary>
    ''' This class is auto generated from BaseTypesBV2.xsd. 
    ''' It defines the request object of GetReceiptCashListItems.
    '''</summary>
    '''<remarks/>

    Partial Public Class BaseGetReceiptCashListItemsRequestType
        Inherits BaseRequestType

        Private cashListKeyField As Integer


        '''<remarks/>
        Public Property CashListKey() As Integer
            Get
                Return Me.cashListKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListKeyField = value
            End Set
        End Property
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If CashListKey = 0 Then
                oSAMErrorCollection.AddInvalidData(
                    SAMConstants.SAMInvalidData.MandatoryInputMissing,
                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                "CashListKey")
            End If
        End Sub
    End Class
    'End (Muthukumari,Abhishek) - (Tech Spec - UIIC WR62 – Cash Cheque Receipt – Get Receipt Cash List Items.doc) - (7.1.4.1)
#End Region
#Region "Public Class BaseGetReceiptCashListItemsResponseType and Related Classes"
    'Start (Muthukumari,Abhishek) - (Tech Spec - UIIC WR62 – Cash Cheque Receipt – Get Receipt Cash List Items.doc) - (7.1.4.3)
    '''<summary>
    ''' This class is auto generated from BaseTypesBV2.xsd. 
    ''' It defines the reponse object of GetReceiptCashListItems and some associated classes.
    '''</summary>
    '''<remarks/>

    Partial Public Class BaseGetReceiptCashListItemsResponseTypeReceiptCashListItemsRow

        Private cashListItemKeyField As Integer

        Private mediaReferenceField As String

        Private mediaTypeField As String

        Private amountField As Double

        Private accountShortCodeField As String

        Private statusField As String

        Private letterField As Boolean

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
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
        Public Property MediaType() As String
            Get
                Return Me.mediaTypeField
            End Get
            Set(ByVal value As String)
                Me.mediaTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amount() As Double
            Get
                Return Me.amountField
            End Get
            Set(ByVal value As Double)
                Me.amountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountShortCode() As String
            Get
                Return Me.accountShortCodeField
            End Get
            Set(ByVal value As String)
                Me.accountShortCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set(ByVal value As String)
                Me.statusField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Letter() As Boolean
            Get
                Return Me.letterField
            End Get
            Set(ByVal value As Boolean)
                Me.letterField = value
            End Set
        End Property
    End Class

    '''<remarks/>

    Partial Public Class BaseGetReceiptCashListItemsResponseTypeReceiptCashListItems

        Private rowField() As BaseGetReceiptCashListItemsResponseTypeReceiptCashListItemsRow

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Row")>
        Public Property Row() As BaseGetReceiptCashListItemsResponseTypeReceiptCashListItemsRow()
            Get
                Return Me.rowField
            End Get
            Set(ByVal value As BaseGetReceiptCashListItemsResponseTypeReceiptCashListItemsRow())
                Me.rowField = value
            End Set
        End Property
    End Class
    '''<remarks/>

    Partial Public Class BaseGetReceiptCashListItemsResponseType
        Inherits BaseResponseType

        Private resultDatasetField As System.Xml.XmlElement

        '''<remarks/>
        Public Property ResultDataset() As System.Xml.XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
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
    'End (Muthukumari,Abhishek) - (Tech Spec - UIIC WR62 – Cash Cheque Receipt – Get Receipt Cash List Items.doc) - (7.1.4.3)
#End Region

    Partial Public Class BasePaymentCashListItemTypePolicies

        Private insuranceFileKeyField As Integer

        Private documentRefField As String

        Private writeOffReasonKeyField As Integer

        Private writeOffAmountField As Decimal

        Private isCurrencyWriteOffField As Boolean

        Private amountTobeAllocatedField As Decimal


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
        Public Property DocumentRef() As String
            Get
                Return Me.documentRefField
            End Get
            Set(ByVal value As String)
                Me.documentRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WriteOffReasonKey() As Integer
            Get
                Return Me.writeOffReasonKeyField
            End Get
            Set(ByVal value As Integer)
                Me.writeOffReasonKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WriteOffAmount() As Decimal
            Get
                Return Me.writeOffAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.writeOffAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsCurrencyWriteOff() As Boolean
            Get
                Return Me.isCurrencyWriteOffField
            End Get
            Set(ByVal value As Boolean)
                Me.isCurrencyWriteOffField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmountTobeAllocated() As Decimal
            Get
                Return Me.amountTobeAllocatedField
            End Get
            Set(ByVal value As Decimal)
                Me.amountTobeAllocatedField = value
            End Set
        End Property

    End Class

    '''<remarks/>
    Partial Public Class BaseFindPaymentDetailsRequestType
        Inherits BaseRequestType

        Private maxRowsToFetchField As Integer

        Private maxRowsToFetchFieldSpecified As Boolean

        Private paymentBranchField As String

        Private payeeNameField As String

        Private paymentTypeField As String

        Private paymentStatusField As String

        Private batchReferenceField As String

        Private bankAccountField As String

        Private clientCodeField As String

        Private clientAccountNumberField As String

        Private policyClaimNumberField As String

        Private mediaTypeField As String

        Private mediaReferenceFromField As String

        Private mediaReferenceToField As String

        Private amountRangeFromField As Decimal

        Private amountRangeFromFieldSpecified As Boolean

        Private amountrangeToField As Decimal

        Private amountrangeToFieldSpecified As Boolean

        Private dateFromField As Date

        Private dateFromFieldSpecified As Boolean

        Private dateToField As Date

        Private dateToFieldSpecified As Boolean

        Private showOnlyOutStandingField As String

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
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxRowsToFetchSpecified() As Boolean
            Get
                Return Me.maxRowsToFetchFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.maxRowsToFetchFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentBranch() As String
            Get
                Return Me.paymentBranchField
            End Get
            Set(ByVal value As String)
                Me.paymentBranchField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeeName() As String
            Get
                Return Me.payeeNameField
            End Get
            Set(ByVal value As String)
                Me.payeeNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentType() As String
            Get
                Return Me.paymentTypeField
            End Get
            Set(ByVal value As String)
                Me.paymentTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentStatus() As String
            Get
                Return Me.paymentStatusField
            End Get
            Set(ByVal value As String)
                Me.paymentStatusField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BatchReference() As String
            Get
                Return Me.batchReferenceField
            End Get
            Set(ByVal value As String)
                Me.batchReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankAccount() As String
            Get
                Return Me.bankAccountField
            End Get
            Set(ByVal value As String)
                Me.bankAccountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientCode() As String
            Get
                Return Me.clientCodeField
            End Get
            Set(ByVal value As String)
                Me.clientCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientAccountNumber() As String
            Get
                Return Me.clientAccountNumberField
            End Get
            Set(ByVal value As String)
                Me.clientAccountNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PolicyClaimNumber() As String
            Get
                Return Me.policyClaimNumberField
            End Get
            Set(ByVal value As String)
                Me.policyClaimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaType() As String
            Get
                Return Me.mediaTypeField
            End Get
            Set(ByVal value As String)
                Me.mediaTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaReferenceFrom() As String
            Get
                Return Me.mediaReferenceFromField
            End Get
            Set(ByVal value As String)
                Me.mediaReferenceFromField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaReferenceTo() As String
            Get
                Return Me.mediaReferenceToField
            End Get
            Set(ByVal value As String)
                Me.mediaReferenceToField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmountRangeFrom() As Decimal
            Get
                Return Me.amountRangeFromField
            End Get
            Set(ByVal value As Decimal)
                Me.amountRangeFromField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountRangeFromSpecified() As Boolean
            Get
                Return Me.amountRangeFromFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amountRangeFromFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmountrangeTo() As Decimal
            Get
                Return Me.amountrangeToField
            End Get
            Set(ByVal value As Decimal)
                Me.amountrangeToField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountrangeToSpecified() As Boolean
            Get
                Return Me.amountrangeToFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amountrangeToFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateFrom() As Date
            Get
                Return Me.dateFromField
            End Get
            Set(ByVal value As Date)
                Me.dateFromField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateFromSpecified() As Boolean
            Get
                Return Me.dateFromFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateFromFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateTo() As Date
            Get
                Return Me.dateToField
            End Get
            Set(ByVal value As Date)
                Me.dateToField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateToSpecified() As Boolean
            Get
                Return Me.dateToFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateToFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ShowOnlyOutStanding() As String
            Get
                Return Me.showOnlyOutStandingField
            End Get
            Set(ByVal value As String)
                Me.showOnlyOutStandingField = value
            End Set
        End Property
    End Class
#Region "Class BaseFindPaymentDetailsResponseType"

    ''' <summary>
    '''This class  BaseGetEventDetailsResponse Type contains all members declaration and its property defined here.
    '''</summary>
    '''<remarks></remarks>

    Partial Public Class BaseFindPaymentDetailsResponseType
        Inherits BaseResponseType

        Private resultDatasetField As System.Xml.XmlElement

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Row")>
        Public Property ResultDataset() As System.Xml.XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property

    End Class
#End Region

    Partial Public Class BaseCancelPaymentRequestType
        Inherits BaseRequestType

        Private transDetailKeyField As Integer

        Private reverseReasonKeyField As Integer

        Private cashListItemKeyField As Integer

        Private insuranceFileKeyField As Integer

        Private insuranceFileKeyFieldSpecified As Boolean

        '''<remarks/>
        Public Property TransDetailKey() As Integer
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.transDetailKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReverseReasonKey() As Integer
            Get
                Return Me.reverseReasonKeyField
            End Get
            Set(ByVal value As Integer)
                Me.reverseReasonKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
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
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property InsuranceFileKeySpecified() As Boolean
            Get
                Return Me.insuranceFileKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.insuranceFileKeyFieldSpecified = value
            End Set
        End Property
    End Class

    Partial Public Class BaseCancelPaymentResponseType
        Inherits BaseResponseType

        Private warningsField As BaseGeneralWarningResponseType()

        Public Property Warnings() As BaseGeneralWarningResponseType()
            Get
                Return Me.warningsField
            End Get
            Set(ByVal value As BaseGeneralWarningResponseType())
                Me.warningsField = value
            End Set
        End Property
    End Class

    Partial Public Class BaseFindReceiptDetailsRequestType
        Inherits BaseRequestType

        Private maxRowsToFetchField As Integer

        Private maxRowsToFetchFieldSpecified As Boolean

        Private receiptBranchField As String

        Private payeeNameField As String

        Private documentReferenceField As String

        Private receiptStatusField As String

        Private batchReferenceField As String

        Private bankAccountField As String

        Private clientCodeField As String

        Private clientAccountNumberField As String

        Private policyClaimNumberField As String

        Private mediaTypeField As String

        Private mediaReferenceFromField As String

        Private mediaReferenceToField As String

        Private amountRangeFromField As Decimal

        Private amountRangeFromFieldSpecified As Boolean

        Private amountrangeToField As Decimal

        Private amountrangeToFieldSpecified As Boolean

        Private dateFromField As Date

        Private dateFromFieldSpecified As Boolean

        Private dateToField As Date

        Private dateToFieldSpecified As Boolean

        Private showOnlyOutStandingField As String

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
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxRowsToFetchSpecified() As Boolean
            Get
                Return Me.maxRowsToFetchFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.maxRowsToFetchFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptBranch() As String
            Get
                Return Me.receiptBranchField
            End Get
            Set(ByVal value As String)
                Me.receiptBranchField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PayeeName() As String
            Get
                Return Me.payeeNameField
            End Get
            Set(ByVal value As String)
                Me.payeeNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DocumentReference() As String
            Get
                Return Me.documentReferenceField
            End Get
            Set(ByVal value As String)
                Me.documentReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReceiptStatus() As String
            Get
                Return Me.receiptStatusField
            End Get
            Set(ByVal value As String)
                Me.receiptStatusField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BatchReference() As String
            Get
                Return Me.batchReferenceField
            End Get
            Set(ByVal value As String)
                Me.batchReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BankAccount() As String
            Get
                Return Me.bankAccountField
            End Get
            Set(ByVal value As String)
                Me.bankAccountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientCode() As String
            Get
                Return Me.clientCodeField
            End Get
            Set(ByVal value As String)
                Me.clientCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientAccountNumber() As String
            Get
                Return Me.clientAccountNumberField
            End Get
            Set(ByVal value As String)
                Me.clientAccountNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PolicyClaimNumber() As String
            Get
                Return Me.policyClaimNumberField
            End Get
            Set(ByVal value As String)
                Me.policyClaimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaType() As String
            Get
                Return Me.mediaTypeField
            End Get
            Set(ByVal value As String)
                Me.mediaTypeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaReferenceFrom() As String
            Get
                Return Me.mediaReferenceFromField
            End Get
            Set(ByVal value As String)
                Me.mediaReferenceFromField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MediaReferenceTo() As String
            Get
                Return Me.mediaReferenceToField
            End Get
            Set(ByVal value As String)
                Me.mediaReferenceToField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmountRangeFrom() As Decimal
            Get
                Return Me.amountRangeFromField
            End Get
            Set(ByVal value As Decimal)
                Me.amountRangeFromField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountRangeFromSpecified() As Boolean
            Get
                Return Me.amountRangeFromFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amountRangeFromFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmountrangeTo() As Decimal
            Get
                Return Me.amountrangeToField
            End Get
            Set(ByVal value As Decimal)
                Me.amountrangeToField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountrangeToSpecified() As Boolean
            Get
                Return Me.amountrangeToFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amountrangeToFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateFrom() As Date
            Get
                Return Me.dateFromField
            End Get
            Set(ByVal value As Date)
                Me.dateFromField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateFromSpecified() As Boolean
            Get
                Return Me.dateFromFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateFromFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateTo() As Date
            Get
                Return Me.dateToField
            End Get
            Set(ByVal value As Date)
                Me.dateToField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateToSpecified() As Boolean
            Get
                Return Me.dateToFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateToFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ShowOnlyOutStanding() As String
            Get
                Return Me.showOnlyOutStandingField
            End Get
            Set(ByVal value As String)
                Me.showOnlyOutStandingField = value
            End Set
        End Property
    End Class

    Partial Public Class BaseCancelReceiptRequestType
        Inherits BaseRequestType

        Private transDetailKeyField As Integer

        Private reverseReasonKeyField As Integer

        Private cashListItemKeyField As Integer

        Private insuranceFileKeyField As Integer

        Private insuranceFileKeyFieldSpecified As Boolean

        Private timeStampField() As Byte

        Private reverseReasonCodeField As String

        '''<remarks/>
        Public Property TransDetailKey() As Integer
            Get
                Return Me.transDetailKeyField
            End Get
            Set(ByVal value As Integer)
                Me.transDetailKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReverseReasonKey() As Integer
            Get
                Return Me.reverseReasonKeyField
            End Get
            Set(ByVal value As Integer)
                Me.reverseReasonKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CashListItemKey() As Integer
            Get
                Return Me.cashListItemKeyField
            End Get
            Set(ByVal value As Integer)
                Me.cashListItemKeyField = value
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
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property InsuranceFileKeySpecified() As Boolean
            Get
                Return Me.insuranceFileKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.insuranceFileKeyFieldSpecified = value
            End Set
        End Property

        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReverseReasonCode() As String
            Get
                Return Me.reverseReasonCodeField
            End Get
            Set(ByVal value As String)
                Me.reverseReasonCodeField = value
            End Set
        End Property
    End Class

    Partial Public Class BaseFindReceiptDetailsResponseType
        Inherits BaseResponseType

        Private resultDatasetField As System.Xml.XmlElement

        '''<remarks/>
        Public Property ResultDataset() As System.Xml.XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property

    End Class

    Partial Public Class BaseCancelReceiptResponseType
        Inherits BaseResponseType

        Private warningsField As BaseGeneralWarningResponseType()
        Private timeStampField() As Byte

        Public Property Warnings() As BaseGeneralWarningResponseType()
            Get
                Return Me.warningsField
            End Get
            Set(ByVal value As BaseGeneralWarningResponseType())
                Me.warningsField = value
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

    Partial Public Class BaseGetPaymentTypeCashListItemRequestType
        Inherits BaseRequestType

        ''' <summary>
        ''' CashListItemKey
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CashListItemKey As Integer

    End Class

    Partial Public Class BaseGetPaymentTypeCashListItemResponseType
        Inherits BaseResponseType

        ''' <summary>
        ''' CashList Collection
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute("PaymentItem")>
        Public Property CashList As BasePaymentCashListType()

    End Class

    Partial Public Class BaseGetUserPreferredColumnListRequestType
        Inherits BaseRequestType
        Public Property InterfaceName() As String

    End Class

    Partial Public Class BaseGetUserPreferredColumnListResponseType
        Inherits BaseResponseType

        Public Property ColumnList() As String

    End Class

    Partial Public Class BaseUpdateUserPreferredColumnListRequestType
        Inherits BaseRequestType

        Public Property InterfaceName() As String
        Public Property ColumnList() As String
        Public Property UserName() As String

    End Class


    Partial Public Class BaseUpdateUserPreferredColumnListResponseType
        Inherits BaseResponseType

    End Class

    
    'start Manual Journal

    Partial Public Class BaseGetListofManualJournalTransactionsRequestType
        Inherits BaseRequestType
        Private _manualjournalId As String
       
    Private _dateFrom As Date
    Private _dateTo As Date
    Private _accountCode As String
    Private _journalTypeCode As String
    Public Property DateFrom() As Date
        Get
            Return _dateFrom
        End Get
        Set(value As Date)
            _dateFrom = value
        End Set
    End Property
    Public Property DateTo() As Date
        Get
            Return _dateTo
        End Get
        Set(value As Date)
            _dateTo = value
        End Set
    End Property
    Public Property AccountCode() As String
        Get
            Return _accountCode
        End Get
        Set(value As String)
            _accountCode = value
        End Set
    End Property
    Public Property JournalTypeCode() As String
        Get
            Return _journalTypeCode
        End Get
        Set(value As String)
            _journalTypeCode = value
        End Set
    End Property
    End Class
    Partial Public Class BaseGetListofManualJournalTransactionsResponseType
        Inherits BaseResponseType
        Private resultDatasetField As DataSet

        Public Property ResultDataset() As DataSet
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As DataSet)
                Me.resultDatasetField = value
            End Set
        End Property

    End Class

    
     Partial Public Class BaseGetListOfManualJournalTransactionMasterRequestType
        Inherits BaseRequestType

        Public Property ManualJournalId() As Integer

    End Class

    Partial Public Class BaseGetListOfManualJournalTransactionMasterResponseType
        Inherits BaseResponseType

        Private resultMasterDataSetField As DataSet


        Public Property ResultMasterDataSet() As DataSet
            Get
                Return Me.resultMasterDataSetField
            End Get
            Set(ByVal value As DataSet)
                Me.resultMasterDataSetField = value
                 End Set
        End Property

    End Class
    Partial Public Class BaseGetListOfManualJournalTransactionDetailsRequestType
        Inherits BaseRequestType

        Public Property ManualJournalId() As Integer

    End Class

    Partial Public Class BaseGetListOfManualJournalTransactionDetailsResponseType
        Inherits BaseResponseType

        Private resultDetailDataSetField As DataSet

        Public Property ResultDetailDataSet() As DataSet
        Get
                Return Me.resultDetailDataSetField
            End Get
            Set(ByVal value As DataSet)
                Me.resultDetailDataSetField = value
            End Set
        End Property

    End Class

    Partial Public Class BaseUpdateManualJournalApproversCommentRequestType
        Inherits BaseRequestType

        Public Property ManualJournalId() As Integer
        Public Property Comment() As String

    End Class
    Partial Public Class BaseUpdateManualJournalApproversCommentResponseType
        Inherits BaseResponseType

    End Class

'End Manual Journal
     Partial Public Class BaseGetListofUnapprovedPaymentRequestType
        Inherits BaseRequestType
        Private _cashListItemKey As String
        Private _payeeName As String
        Private _date As Date
        Private _createdBy As String
        Private _assignedTo As String
        Private _Branch As String
        Private _dateTo As Date
        Private _paymentType As String
        Private _showAllOtherPayments As String
        Public Property DateFrom() As Date
            Get
                Return _date
            End Get
            Set(value As Date)
                _date = value
            End Set
        End Property
        Public Property DateTo() As Date
            Get
                Return _dateTo
            End Get
            Set(value As Date)
                _dateTo = value
            End Set
        End Property
        Public Property CreatedBy() As String
            Get
                Return _createdBy
            End Get
            Set(value As String)
                _createdBy = value
            End Set
        End Property
        Public Property AssignedTo() As String
            Get
                Return _assignedTo
            End Get
            Set(value As String)
                _assignedTo = value
            End Set
        End Property
        Public Property Branch() As String
            Get
                Return _Branch
            End Get
            Set(value As String)
                _Branch = value
            End Set
        End Property
        Public Property PaymentType() As String
            Get
                Return _paymentType
            End Get
            Set(value As String)
                _paymentType = value
            End Set
        End Property
        Public Property ShowAllOtherPayments() As String
            Get
                Return _showAllOtherPayments
            End Get
            Set(value As String)
                _showAllOtherPayments = value
            End Set
        End Property
        Public Property PayeeName() As String
            Get
                Return _payeeName
            End Get
            Set(value As String)
                _payeeName = value
            End Set
        End Property
        Public Property CashListItemKey() As String
            Get
                Return _cashListItemKey
            End Get
            Set(value As String)
                _cashListItemKey = value
            End Set
        End Property
    End Class

    Partial Public Class BaseGetListofUnapprovedPaymentResponseType
        Inherits BaseResponseType
        Private BranchField As String
        Private CurrencyFiled As String
        Private BankAccountFiled As String
        Private TransactionDateFiled As DateTime
        Private PaymentTypeFiled As String
        Private MediaTypeFiled As String
        Private MediaRefFiled As String
        Private PolicyRefFiled As String
        Private ClaimRefFiled As String
        Private PayeeAccountNameFiled As String
        Private AmountFiled As Decimal
        Private CreatedByFiled As String
        Private AssignedtoFiled As String
        Private DateAssignedFiled As DateTime
        Private BaseCurrencyAmountFiled As Decimal
        Private StatusFiled As String
        Private resultDatasetField As DataSet

        Public Property ResultDataset() As DataSet
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As DataSet)
                Me.resultDatasetField = value
            End Set
        End Property

        Public Property Branch() As String
            Get
                Return Me.BranchField
            End Get
            Set(ByVal value As String)
                Me.BranchField = value
            End Set
        End Property
        Public Property Currency() As String
            Get
                Return CurrencyFiled
            End Get
            Set(value As String)
                CurrencyFiled = value
            End Set
        End Property
        Public Property BankAccount() As String
            Get
                Return BankAccountFiled
            End Get
            Set(value As String)
                BankAccountFiled = value
            End Set
        End Property
        Public Property TransactionDate() As DateTime
            Get
                Return TransactionDateFiled
            End Get
            Set(value As DateTime)
                TransactionDateFiled = value
            End Set
        End Property
        Public Property PaymentType() As String
            Get
                Return PaymentTypeFiled
            End Get
            Set(value As String)
                PaymentTypeFiled = value
            End Set
        End Property
        Public Property MediaType() As String
            Get
                Return MediaTypeFiled
            End Get
            Set(value As String)
                MediaTypeFiled = value
            End Set
        End Property
        Public Property MediaRef() As String
            Get
                Return MediaRefFiled
            End Get
            Set(value As String)
                MediaRefFiled = value
            End Set
        End Property
        Public Property PolicyRef() As String
            Get
                Return PolicyRefFiled
            End Get
            Set(value As String)
                PolicyRefFiled = value
            End Set
        End Property
        Public Property ClaimRef() As String
            Get
                Return ClaimRefFiled
            End Get
            Set(value As String)
                ClaimRefFiled = value
            End Set
        End Property
        Public Property PayeeAccountName() As String
            Get
                Return PayeeAccountNameFiled
            End Get
            Set(value As String)
                PayeeAccountNameFiled = value
            End Set
        End Property
        Public Property Amount() As Decimal
            Get
                Return AmountFiled
            End Get
            Set(value As Decimal)
                AmountFiled = value
            End Set
        End Property
        Public Property CreatedBy() As String
            Get
                Return CreatedByFiled
            End Get
            Set(value As String)
                CreatedByFiled = value
            End Set
        End Property
        Public Property Status() As String
            Get
                Return StatusFiled
            End Get
            Set(value As String)
                StatusFiled = value
            End Set
        End Property
        Public Property Assignedto() As String
            Get
                Return AssignedtoFiled
            End Get
            Set(value As String)
                AssignedtoFiled = value
            End Set
        End Property
        Public Property DateAssigned() As DateTime
            Get
                Return DateAssignedFiled
            End Get
            Set(value As DateTime)
                DateAssignedFiled = value
            End Set
        End Property
        Public Property BaseCurrencyAmount() As Decimal
            Get
                Return BaseCurrencyAmountFiled
            End Get
            Set(value As Decimal)
                BaseCurrencyAmountFiled = value
            End Set
        End Property

    End Class
    'End Manual Journal

    Partial Public Class BaseUpdateAuthorizationCommentRequestType
        Inherits BaseRequestType

        Public Property CashListItem_id() As Integer
        Public Property Comment() As String

    End Class
    Partial Public Class BaseUpdateAuthorizationCommentResponseType
        Inherits BaseResponseType

    End Class

    Partial Public Class BaseGetAuthorizationCommentRequestType
        Inherits BaseRequestType

        Public Property CashListItem_id() As Integer

    End Class

    Partial Public Class BaseGetAuthorizationCommentResponseType
        Inherits BaseResponseType

        Public Property Authorization_Comment() As String
    End Class

    Partial Public Class BaseValidateAuthorizationStepsRequestType
        Inherits BaseRequestType

        Public Property ManualJournalId() As Integer
        Public Property IsApproved() As Boolean

    End Class

    Partial Public Class BaseValidateAuthorizationStepsResponseType
        Inherits BaseResponseType
        Private resultDatasetField As System.Xml.XmlElement

        '''<remarks/>
        Public Property ResultDataset() As System.Xml.XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property
        Private resultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.resultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.resultdatafield = value
            End Set
        End Property
    End Class



End Namespace


