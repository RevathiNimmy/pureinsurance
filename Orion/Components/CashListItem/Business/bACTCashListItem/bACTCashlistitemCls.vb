Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("Cashlistitem_NET.Cashlistitem")>
Public NotInheritable Class Cashlistitem
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Cashlistitem
    '
    ' Date: 07/04/1998
    '
    ' Description: Describes the Cashlistitem attributes.
    '
    ' Edit History:
    '
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Cashlistitem"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer
    ' DataBase Attributes
    Private m_lCashlistitemID As Integer
    Private m_lAllocationstatusID As Integer
    Private m_lMediaTypeID As Integer
    Private m_lMediaTypeIssuerID As Integer
    Private m_lCashlistID As Integer
    Private m_lAccountID As Integer
    Private m_sMediaRef As String = String.Empty
    Private m_sOurRef As String = String.Empty
    Private m_sTheirRef As String = String.Empty
    Private m_cAmount As Decimal
    Private m_lTransdetailID As Integer
    Private m_sContactName As String = String.Empty
    Private m_sAddress1 As String = String.Empty
    Private m_sAddress2 As String = String.Empty
    Private m_sAddress3 As String = String.Empty
    Private m_sAddress4 As String = String.Empty
    Private m_sPostalCode As String = String.Empty
    Private m_iAddressCountry As Integer
    Private m_sPaymentName As String = String.Empty
    Private m_sPaymentAccountCode As String = String.Empty
    Private m_sPaymentBranchCode As String = String.Empty
    Private m_dtPaymentExpiryDate As Date
    Private m_sPaymentReference1 As String = String.Empty
    Private m_sPaymentReference2 As String = String.Empty
    Private m_bLetter As Boolean
    Private m_lBatch_id As Integer
    Private m_lPMUser_id As Integer
    Private m_dtTransaction_Date As Date
    Private m_cOriginal_Amount As Decimal
    Private m_cAmount_Tendered As Decimal
    Private m_cChange As Decimal
    Private m_lCashListItem_receipt_type_id As Integer
    Private m_lCashListItem_receipt_status_id As Integer
    Private m_lCashListItem_bank_id As Integer
    Private m_dtCheque_Date As Date
    Private m_sCC_Name As String = ""
    Private m_sCC_Customer As String = ""
    Private m_sCC_Number As String = String.Empty
    Private m_sCC_Expiry_Date As String = ""
    Private m_sCC_Start_Date As String = ""
    Private m_sCC_Issue As String = String.Empty
    Private m_sCC_Pin As String = String.Empty
    Private m_sCC_Auth_Code As String = String.Empty
    Private m_sCC_Manual_Auth_Code As String = ""
    Private m_sCC_Transaction_Code As String = ""
    Private m_sReceipt_Details As String = String.Empty
    Private m_iCashListItem_Reverse_PMUser_id As Integer
    Private m_lCashListItem_Reverse_Reason_id As Integer
    Private m_lCashListItem_Payment_Type_id As Integer
    Private m_lCashListItem_Payment_Method_id As Integer
    Private m_lCashListItem_Payment_Status_id As Integer
    Private m_dtDate_Presented As Date
    Private m_iCheque_in_Possession As Integer
    Private m_dtStop_Requested_Date As Date
    Private m_dtStop_Printed_Date As Date
    Private m_dtStop_Confirmation_Date As Date
    Private m_sReason As String = String.Empty
    Private m_lReplaces_CashListItem_id As Integer
    Private m_sXML_Object As String = String.Empty
    Private m_vInstalmentArray As Object
    Private m_vSalvageArray As Object
    Private m_vCLMUSRecoveryArray As Object
    Private m_vCLMRVRecoveryArray As Object
    Private m_vUnderwritingYearID As Object
    Private m_vCurrencyBaseDate As Object
    Private m_vCurrencyBaseXrate As Object
    Private m_vAccountBaseDate As Object
    Private m_vAccountBaseXrate As Object
    Private m_vSystemBaseDate As Object
    Private m_vSystemBaseXrate As Object
    Private m_vOverrideReason As Object

    'Party Bank Details
    Private m_vPartyBankId As Object
    'Rahul
    Private m_dCollectionDate As Date
    Private m_sComments As String = ""
    Private m_vBGPolicies As Object
    'End

    'Start - Sankar - PN 56851
    Private m_IsInstalment As Boolean
    'End - Sankar - PN 56851

    'WPR12- Enhancement Quote Collection Process
    Private m_sBankLocation As String = ""
    Private m_sBankBranch As String = ""
    Private m_lChequeTypeId As Integer
    Private m_lCCBankId As Integer
    Private m_lCCCardTypeId As Integer
    Private m_sTransSlipNo As String = ""
    Private m_lChequeClearingTypeId As Integer


    ' WPR 51
    Private m_bIsLeadAccount As Boolean
    Private m_cSplitTotal As Decimal

    Private m_nTaxBandId As Integer
    Private m_crTaxAmount As Decimal
    Private m_sBIC As String = String.Empty
    Private m_sIBAN As String = String.Empty
    Private m_nPMNavBatchKey As Integer
    Private m_sInsuranceRef As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property


    Public Property CashlistitemID() As Integer
        Get

            Return m_lCashlistitemID

        End Get
        Set(ByVal Value As Integer)

            m_lCashlistitemID = Value

        End Set
    End Property

    Public Property AllocationstatusID() As Integer
        Get

            Return m_lAllocationstatusID

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationstatusID = Value

        End Set
    End Property

    Public Property MediaTypeID() As Integer
        Get
            Return m_lMediaTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lMediaTypeID = Value
        End Set
    End Property

    Public Property MediaTypeIssuerID() As Integer
        Get
            Return m_lMediaTypeIssuerID
        End Get
        Set(ByVal Value As Integer)
            m_lMediaTypeIssuerID = Value
        End Set
    End Property

    Public Property CashlistID() As Integer
        Get

            Return m_lCashlistID

        End Get
        Set(ByVal Value As Integer)

            m_lCashlistID = Value

        End Set
    End Property

    Public Property AccountID() As Integer
        Get

            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            m_lAccountID = Value

        End Set
    End Property

    Public Property MediaRef() As String
        Get

            Return m_sMediaRef

        End Get
        Set(ByVal Value As String)

            m_sMediaRef = Value

        End Set
    End Property

    Public Property OurRef() As String
        Get

            Return m_sOurRef

        End Get
        Set(ByVal Value As String)

            m_sOurRef = Value

        End Set
    End Property

    Public Property TheirRef() As String
        Get

            Return m_sTheirRef

        End Get
        Set(ByVal Value As String)

            m_sTheirRef = Value

        End Set
    End Property

    Public Property Amount() As Decimal
        Get

            Return m_cAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cAmount = Value

        End Set
    End Property

    Public Property TransdetailID() As Integer
        Get

            Return m_lTransdetailID

        End Get
        Set(ByVal Value As Integer)

            m_lTransdetailID = Value

        End Set
    End Property

    Public Property ContactName() As String
        Get

            Return m_sContactName

        End Get
        Set(ByVal Value As String)

            m_sContactName = Value

        End Set
    End Property

    Public Property Address1() As String
        Get

            Return m_sAddress1

        End Get
        Set(ByVal Value As String)

            m_sAddress1 = Value

        End Set
    End Property

    Public Property Address2() As String
        Get

            Return m_sAddress2

        End Get
        Set(ByVal Value As String)

            m_sAddress2 = Value

        End Set
    End Property

    Public Property Address3() As String
        Get

            Return m_sAddress3

        End Get
        Set(ByVal Value As String)

            m_sAddress3 = Value

        End Set
    End Property

    Public Property Address4() As String
        Get

            Return m_sAddress4

        End Get
        Set(ByVal Value As String)

            m_sAddress4 = Value

        End Set
    End Property

    Public Property PostalCode() As String
        Get

            Return m_sPostalCode

        End Get
        Set(ByVal Value As String)

            m_sPostalCode = Value

        End Set
    End Property

    Public Property AddressCountry() As Integer
        Get

            Return m_iAddressCountry

        End Get
        Set(ByVal Value As Integer)

            m_iAddressCountry = Value

        End Set
    End Property

    Public Property PaymentName() As String
        Get

            Return m_sPaymentName

        End Get
        Set(ByVal Value As String)

            m_sPaymentName = Value

        End Set
    End Property

    Public Property PaymentAccountCode() As String
        Get

            Return m_sPaymentAccountCode

        End Get
        Set(ByVal Value As String)

            m_sPaymentAccountCode = Value

        End Set
    End Property

    Public Property PaymentBranchCode() As String
        Get

            Return m_sPaymentBranchCode

        End Get
        Set(ByVal Value As String)

            m_sPaymentBranchCode = Value

        End Set
    End Property

    Public Property PaymentExpiryDate() As Date
        Get

            Return m_dtPaymentExpiryDate

        End Get
        Set(ByVal Value As Date)

            m_dtPaymentExpiryDate = Value

        End Set
    End Property

    Public Property PaymentReference1() As String
        Get

            Return m_sPaymentReference1

        End Get
        Set(ByVal Value As String)

            m_sPaymentReference1 = Value

        End Set
    End Property

    Public Property PaymentReference2() As String
        Get

            Return m_sPaymentReference2

        End Get
        Set(ByVal Value As String)

            m_sPaymentReference2 = Value

        End Set
    End Property
    'eck100701
    Public Property Letter() As Boolean
        Get

            Return m_bLetter

        End Get
        Set(ByVal Value As Boolean)

            m_bLetter = Value

        End Set
    End Property



    Public Property Batch_id() As Integer
        Get

            Return m_lBatch_id

        End Get
        Set(ByVal Value As Integer)

            m_lBatch_id = Value

        End Set
    End Property



    Public Property pmuser_id() As Integer
        Get

            Return m_lPMUser_id

        End Get
        Set(ByVal Value As Integer)

            m_lPMUser_id = Value

        End Set
    End Property



    Public Property Transaction_Date() As Date
        Get

            Return m_dtTransaction_Date

        End Get
        Set(ByVal Value As Date)

            m_dtTransaction_Date = Value

        End Set
    End Property


    Public Property Original_Amount() As Decimal
        Get

            Return m_cOriginal_Amount

        End Get
        Set(ByVal Value As Decimal)

            m_cOriginal_Amount = Value

        End Set
    End Property



    Public Property Amount_Tendered() As Decimal
        Get

            Return m_cAmount_Tendered

        End Get
        Set(ByVal Value As Decimal)

            m_cAmount_Tendered = Value

        End Set
    End Property



    Public Property Change() As Decimal
        Get

            Return m_cChange

        End Get
        Set(ByVal Value As Decimal)

            m_cChange = Value

        End Set
    End Property


    Public Property CashListItem_receipt_type_id() As Integer
        Get

            Return m_lCashListItem_receipt_type_id

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItem_receipt_type_id = Value

        End Set
    End Property


    Public Property CashListItem_receipt_status_id() As Integer
        Get

            Return m_lCashListItem_receipt_status_id

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItem_receipt_status_id = Value

        End Set
    End Property



    Public Property CashListItem_bank_id() As Integer
        Get

            Return m_lCashListItem_bank_id

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItem_bank_id = Value

        End Set
    End Property



    Public Property Cheque_Date() As Date
        Get

            Return m_dtCheque_Date

        End Get
        Set(ByVal Value As Date)

            m_dtCheque_Date = Value

        End Set
    End Property



    Public Property CC_Name() As String
        Get
            Return m_sCC_Name
        End Get
        Set(ByVal Value As String)
            m_sCC_Name = Value
        End Set
    End Property


    Public Property CC_Customer() As String
        Get
            Return m_sCC_Customer
        End Get
        Set(ByVal Value As String)
            m_sCC_Customer = Value
        End Set
    End Property


    Public Property CC_Number() As String
        Get
            Return m_sCC_Number
        End Get
        Set(ByVal Value As String)
            m_sCC_Number = Value
        End Set
    End Property



    Public Property CC_Expiry_Date() As String
        Get

            Return m_sCC_Expiry_Date

        End Get
        Set(ByVal Value As String)

            m_sCC_Expiry_Date = Value

        End Set
    End Property



    Public Property CC_Start_Date() As String
        Get

            Return m_sCC_Start_Date

        End Get
        Set(ByVal Value As String)

            m_sCC_Start_Date = Value

        End Set
    End Property



    Public Property CC_Issue() As String
        Get

            Return m_sCC_Issue

        End Get
        Set(ByVal Value As String)

            m_sCC_Issue = Value

        End Set
    End Property



    Public Property CC_Pin() As String
        Get

            Return m_sCC_Pin

        End Get
        Set(ByVal Value As String)

            m_sCC_Pin = Value

        End Set
    End Property



    Public Property CC_Auth_Code() As String
        Get
            Return m_sCC_Auth_Code
        End Get
        Set(ByVal Value As String)
            m_sCC_Auth_Code = Value
        End Set
    End Property



    Public Property CC_Manual_Auth_Code() As String
        Get
            Return m_sCC_Manual_Auth_Code
        End Get
        Set(ByVal Value As String)
            m_sCC_Manual_Auth_Code = Value
        End Set
    End Property


    Public Property CC_Transaction_Code() As String
        Get
            Return m_sCC_Transaction_Code
        End Get
        Set(ByVal Value As String)
            m_sCC_Transaction_Code = Value
        End Set
    End Property


    Public Property Receipt_Details() As String
        Get
            Return m_sReceipt_Details
        End Get
        Set(ByVal Value As String)
            m_sReceipt_Details = Value
        End Set
    End Property



    Public Property CashListItem_Reverse_PMUser_id() As Integer
        Get

            Return m_iCashListItem_Reverse_PMUser_id

        End Get
        Set(ByVal Value As Integer)

            m_iCashListItem_Reverse_PMUser_id = Value

        End Set
    End Property



    Public Property CashListItem_Reverse_Reason_id() As Integer
        Get

            Return m_lCashListItem_Reverse_Reason_id

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItem_Reverse_Reason_id = Value

        End Set
    End Property



    Public Property CashListItem_Payment_Type_id() As Integer
        Get

            Return m_lCashListItem_Payment_Type_id

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItem_Payment_Type_id = Value

        End Set
    End Property



    Public Property CashListItem_Payment_Status_id() As Integer
        Get

            Return m_lCashListItem_Payment_Status_id

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItem_Payment_Status_id = Value

        End Set
    End Property



    Public Property Date_Presented() As Date
        Get

            Return m_dtDate_Presented

        End Get
        Set(ByVal Value As Date)

            m_dtDate_Presented = Value

        End Set
    End Property



    Public Property Cheque_in_Possession() As Integer
        Get

            Return m_iCheque_in_Possession

        End Get
        Set(ByVal Value As Integer)

            m_iCheque_in_Possession = Value

        End Set
    End Property



    Public Property Stop_Requested_Date() As Date
        Get

            Return m_dtStop_Requested_Date

        End Get
        Set(ByVal Value As Date)

            m_dtStop_Requested_Date = Value

        End Set
    End Property



    Public Property Stop_Printed_Date() As Date
        Get

            Return m_dtStop_Printed_Date

        End Get
        Set(ByVal Value As Date)

            m_dtStop_Printed_Date = Value

        End Set
    End Property



    Public Property Stop_Confirmation_Date() As Date
        Get

            Return m_dtStop_Confirmation_Date

        End Get
        Set(ByVal Value As Date)

            m_dtStop_Confirmation_Date = Value

        End Set
    End Property



    Public Property Reason() As String
        Get

            Return m_sReason

        End Get
        Set(ByVal Value As String)

            m_sReason = Value

        End Set
    End Property



    Public Property Replaces_CashListItem_id() As Integer
        Get

            Return m_lReplaces_CashListItem_id

        End Get
        Set(ByVal Value As Integer)

            m_lReplaces_CashListItem_id = Value

        End Set
    End Property



    Public Property XML_Object() As String
        Get

            Return m_sXML_Object

        End Get
        Set(ByVal Value As String)

            m_sXML_Object = Value

        End Set
    End Property

    'sw front office receipting, add the instalment array as a property of the cashlistitem
    '02-11-2002

    Public Property Instalment_Array() As Object
        Get

            Return m_vInstalmentArray

        End Get
        Set(ByVal Value As Object)



            m_vInstalmentArray = Value

        End Set
    End Property

    'sw front office receipting, add the salvage array as a property of the cashlistitem
    '02-11-2002

    Public Property Salvage_Array() As Object
        Get

            Return m_vSalvageArray

        End Get
        Set(ByVal Value As Object)



            m_vSalvageArray = Value

        End Set
    End Property


    Public Property CLMUSRecovery_Array() As Object
        Get

            Return m_vCLMUSRecoveryArray

        End Get
        Set(ByVal Value As Object)



            m_vCLMUSRecoveryArray = Value

        End Set
    End Property



    Public Property CLMRVRecovery_Array() As Object
        Get

            Return m_vCLMRVRecoveryArray

        End Get
        Set(ByVal Value As Object)



            m_vCLMRVRecoveryArray = Value

        End Set
    End Property


    Public Property UnderwritingYearID() As Object
        Get
            Return m_vUnderwritingYearID
        End Get
        Set(ByVal Value As Object)


            m_vUnderwritingYearID = Value
        End Set
    End Property

    Public Property CurrencyBaseDate() As Object
        Get
            Return m_vCurrencyBaseDate
        End Get
        Set(ByVal Value As Object)


            m_vCurrencyBaseDate = Value
        End Set
    End Property

    Public Property CurrencyBaseXrate() As Object
        Get
            Return m_vCurrencyBaseXrate
        End Get
        Set(ByVal Value As Object)


            m_vCurrencyBaseXrate = Value
        End Set
    End Property

    Public Property AccountBaseDate() As Object
        Get
            Return m_vAccountBaseDate
        End Get
        Set(ByVal Value As Object)


            m_vAccountBaseDate = Value
        End Set
    End Property

    Public Property AccountBaseXrate() As Object
        Get
            Return m_vAccountBaseXrate
        End Get
        Set(ByVal Value As Object)


            m_vAccountBaseXrate = Value
        End Set
    End Property

    Public Property SystemBaseDate() As Object
        Get
            Return m_vSystemBaseDate
        End Get
        Set(ByVal Value As Object)


            m_vSystemBaseDate = Value
        End Set
    End Property

    Public Property SystemBaseXrate() As Object
        Get
            Return m_vSystemBaseXrate
        End Get
        Set(ByVal Value As Object)


            m_vSystemBaseXrate = Value
        End Set
    End Property

    Public Property OverrideReason() As Object
        Get
            Return m_vOverrideReason
        End Get
        Set(ByVal Value As Object)


            m_vOverrideReason = Value
        End Set
    End Property

    'Party Bank Details
    Public Property PartyBankId() As Object
        Get
            Return m_vPartyBankId
        End Get
        Set(ByVal Value As Object)


            m_vPartyBankId = Value
        End Set
    End Property
    'Rahul


    Public Property CollectionDate() As Date
        Get
            Return m_dCollectionDate
        End Get
        Set(ByVal Value As Date)
            m_dCollectionDate = Value
        End Set
    End Property



    Public Property Comments() As String
        Get
            Return m_sComments
        End Get
        Set(ByVal Value As String)
            m_sComments = Value
        End Set
    End Property


    Public Property BGPolicies() As Object
        Get
            Return m_vBGPolicies
        End Get
        Set(ByVal Value As Object)


            m_vBGPolicies = Value
        End Set
    End Property

    'Start - Sankar - PN 56851

    Public Property IsInstalment() As Boolean
        Get
            Return m_IsInstalment
        End Get
        Set(ByVal Value As Boolean)
            m_IsInstalment = Value
        End Set
    End Property
    'End - Sankar - PN 56851
    'WPR12- Enhancement Quote Collection Process

    Public Property BankLocation() As String
        Get
            Return m_sBankLocation
        End Get
        Set(ByVal Value As String)
            m_sBankLocation = Value
        End Set
    End Property


    Public Property BankBranch() As String
        Get
            Return m_sBankBranch
        End Get
        Set(ByVal Value As String)
            m_sBankBranch = Value
        End Set
    End Property


    Public Property ChequeTypeId() As Integer
        Get
            Return m_lChequeTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lChequeTypeId = Value
        End Set
    End Property


    Public Property CCBankId() As Integer
        Get
            Return m_lCCBankId
        End Get
        Set(ByVal Value As Integer)
            m_lCCBankId = Value
        End Set
    End Property


    Public Property CardTypeId() As Integer
        Get
            Return m_lCCCardTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lCCCardTypeId = Value
        End Set
    End Property


    Public Property CardTransSlipNo() As String
        Get
            Return m_sTransSlipNo
        End Get
        Set(ByVal Value As String)
            m_sTransSlipNo = Value
        End Set
    End Property


    Public Property ChequeClearingTypeId() As Integer
        Get
            Return m_lChequeClearingTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lChequeClearingTypeId = Value
        End Set
    End Property


    ' WPR 51
    Public Property IsLeadAccount() As Boolean
        Get
            Return m_bIsLeadAccount
        End Get
        Set(ByVal value As Boolean)
            m_bIsLeadAccount = value
        End Set
    End Property

    Public Property SplitTotal() As Decimal
        Get
            Return m_cSplitTotal
        End Get
        Set(ByVal value As Decimal)
            m_cSplitTotal = value
        End Set
    End Property

    Public Property TaxBandID() As Integer
        Get
            Return m_nTaxBandId
        End Get
        Set(ByVal Value As Integer)
            m_nTaxBandId = Value
        End Set
    End Property

    Public Property TaxAmount() As Decimal
        Get
            Return m_crTaxAmount
        End Get
        Set(ByVal value As Decimal)
            m_crTaxAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Business Identifier Code(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return m_sBIC
        End Get
        Set(ByVal value As String)
            m_sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return m_sIBAN
        End Get
        Set(ByVal value As String)
            m_sIBAN = value
        End Set
    End Property


    Public Property PMNavBatchKey() As Integer
        Get
            Return m_nPMNavBatchKey
        End Get
        Set(ByVal Value As Integer)
            m_nPMNavBatchKey = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get

            Return m_sInsuranceRef

        End Get
        Set(ByVal Value As String)

            m_sInsuranceRef = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Friend Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
