Option Strict Off
Option Explicit On
'developer guide no 129. 
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Account_NET.Account")>
Public NotInheritable Class Account
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Account
    ' Date: 23-07-1997
    ' Description: Describes the Account attributes.
    '
    ' Edit History: TF131198 - adjustments for new account_key field
    '               CF030399 - adjustments for account_status field
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Account"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    ' PRIVATE Data Members (Begin)
    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lAccountID As Integer
    Private m_iPurgefrequencyID As Integer
    Private m_iCurrencyID As Integer
    Private m_iAccounttypeID As Integer
    Private m_iLedgerID As Integer
    Private m_iPaymenttypeID As Integer
    Private m_sAccountName As New StringsHelper.FixedLengthString(60)
    Private m_sShortCode As New StringsHelper.FixedLengthString(30) 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Private m_iRestrictEnquiry As Integer
    Private m_iRestrictUpdate As Integer
    Private m_iDeleteAtPurge As Integer
    Private m_sContactName As New StringsHelper.FixedLengthString(60)
    Private m_sAddress1 As New StringsHelper.FixedLengthString(40)
    Private m_sAddress2 As New StringsHelper.FixedLengthString(40)
    Private m_sAddress3 As New StringsHelper.FixedLengthString(40)
    Private m_sAddress4 As New StringsHelper.FixedLengthString(40)
    Private m_sPostalCode As New StringsHelper.FixedLengthString(20)
    Private m_iAddressCountry As Integer
    Private m_sPhoneAreaCode As New StringsHelper.FixedLengthString(10)
    Private m_sPhoneNumber As New StringsHelper.FixedLengthString(15)
    Private m_sPhoneExtension As New StringsHelper.FixedLengthString(6)
    Private m_sFaxAreaCode As New StringsHelper.FixedLengthString(10)
    Private m_sFaxNumber As New StringsHelper.FixedLengthString(15)
    Private m_sFaxExtension As New StringsHelper.FixedLengthString(6)
    Private m_sPaymentName As New StringsHelper.FixedLengthString(60)
    Private m_sPaymentAccountCode As New StringsHelper.FixedLengthString(60)
    Private m_sPaymentBranchCode As New StringsHelper.FixedLengthString(30)
    Private m_dtPaymentExpiryDate As Date
    Private m_sPaymentReference1 As New StringsHelper.FixedLengthString(30)
    Private m_sPaymentReference2 As New StringsHelper.FixedLengthString(30)
    Private m_lProofListReportID As Integer ' RAW 17/12/2002 : PS187 : Added
    Private m_lBordereauReportID As Integer ' RAW 17/12/2002 : PS187 : Added
    Private m_vdCreditLimit As Decimal
    Private m_vdDiscountPercentage As Decimal
    Private m_iSettlementPeriod As Integer
    Private m_sBankName As New StringsHelper.FixedLengthString(60)
    Private m_sBankAddress1 As New StringsHelper.FixedLengthString(40)
    Private m_sBankAddress2 As New StringsHelper.FixedLengthString(40)
    Private m_sBankAddress3 As New StringsHelper.FixedLengthString(40)
    Private m_sBankAddress4 As New StringsHelper.FixedLengthString(40)
    Private m_sBankPostalCode As New StringsHelper.FixedLengthString(20)
    Private m_iBankCountry As Integer
    Private m_sBankPhoneAreaCode As New StringsHelper.FixedLengthString(10)
    Private m_sBankPhoneNumber As New StringsHelper.FixedLengthString(15)
    Private m_sBankPhoneExtension As New StringsHelper.FixedLengthString(6)
    Private m_sBankFaxAreaCode As New StringsHelper.FixedLengthString(10)
    Private m_sBankFaxNumber As New StringsHelper.FixedLengthString(15)
    Private m_sBankFaxExtension As New StringsHelper.FixedLengthString(6)
    Private m_sComments As New StringsHelper.FixedLengthString(255)
    Private m_sBIC As String = String.Empty
    Private m_sIBAN As String = String.Empty
    ' RDC 12112003
    Private m_bAllowElectronicPayment As Boolean
    Private m_sUsername As String = ""

    ' TF131198
    Private m_lAccountKey As Integer
    ' CF150199
    Private m_lNominalAccountID As Integer
    ' CF030399
    Private m_iAccountStatusID As Integer
    'ek110400
    'MultiBranch
    Private m_iCompanyID As Integer
    'PWF 09/10/2002: Multi-branch
    Private m_lSubBranchID As Integer
    'SD 09/01/2003
    Private m_bIsTakenOffHold As Boolean
    Private m_iMoneyCalcAccType As Integer
    Private m_vClientBankAccType As String = ""

    Private m_sMerchantId As String = ""

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


    Public Property AccountID() As Integer
        Get

            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            m_lAccountID = Value

        End Set
    End Property

    Public Property PurgefrequencyID() As Integer
        Get

            Return m_iPurgefrequencyID

        End Get
        Set(ByVal Value As Integer)

            m_iPurgefrequencyID = Value

        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_iCurrencyID = Value

        End Set
    End Property
    'ek110400
    'Multi Branch
    Public Property CompanyID() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

        End Set
    End Property

    'PWF 09/10/2002: Multi-branch
    Public Property SubBranchID() As Integer
        Get
            Return m_lSubBranchID
        End Get
        Set(ByVal Value As Integer)

            m_lSubBranchID = CInt(Value)
        End Set
    End Property


    Public Property AccounttypeID() As Integer
        Get

            Return m_iAccounttypeID

        End Get
        Set(ByVal Value As Integer)

            m_iAccounttypeID = Value

        End Set
    End Property

    Public Property LedgerID() As Integer
        Get

            Return m_iLedgerID

        End Get
        Set(ByVal Value As Integer)

            m_iLedgerID = Value

        End Set
    End Property

    Public Property PaymenttypeID() As Integer
        Get

            Return m_iPaymenttypeID

        End Get
        Set(ByVal Value As Integer)

            m_iPaymenttypeID = Value

        End Set
    End Property

    Public Property AccountName() As String
        Get

            Return m_sAccountName.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sAccountName.Value = Value.Trim()


        End Set
    End Property

    Public Property ShortCode() As String
        Get

            Return m_sShortCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sShortCode.Value = Value.Trim()

        End Set
    End Property

    Public Property RestrictEnquiry() As Integer
        Get

            Return m_iRestrictEnquiry

        End Get
        Set(ByVal Value As Integer)

            m_iRestrictEnquiry = Value

        End Set
    End Property

    Public Property RestrictUpdate() As Integer
        Get

            Return m_iRestrictUpdate

        End Get
        Set(ByVal Value As Integer)

            m_iRestrictUpdate = Value

        End Set
    End Property

    Public Property DeleteAtPurge() As Integer
        Get

            Return m_iDeleteAtPurge

        End Get
        Set(ByVal Value As Integer)

            m_iDeleteAtPurge = Value

        End Set
    End Property

    Public Property ContactName() As String
        Get

            Return m_sContactName.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sContactName.Value = Value.Trim()

        End Set
    End Property

    Public Property Address1() As String
        Get

            Return m_sAddress1.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sAddress1.Value = Value.Trim()

        End Set
    End Property

    Public Property Address2() As String
        Get

            Return m_sAddress2.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sAddress2.Value = Value.Trim()

        End Set
    End Property

    Public Property Address3() As String
        Get

            Return m_sAddress3.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sAddress3.Value = Value.Trim()

        End Set
    End Property

    Public Property Address4() As String
        Get

            Return m_sAddress4.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sAddress4.Value = Value.Trim()

        End Set
    End Property

    Public Property PostalCode() As String
        Get

            Return m_sPostalCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPostalCode.Value = Value.Trim()

        End Set
    End Property

    'developer guide no. 33
    Public Property AddressCountry() As Object
        Get

            Return m_iAddressCountry

        End Get
        Set(ByVal Value As Object)

            m_iAddressCountry = Value

        End Set
    End Property

    Public Property PhoneAreaCode() As String
        Get

            Return m_sPhoneAreaCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPhoneAreaCode.Value = Value.Trim()

        End Set
    End Property

    Public Property PhoneNumber() As String
        Get

            Return m_sPhoneNumber.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPhoneNumber.Value = Value.Trim()

        End Set
    End Property

    Public Property PhoneExtension() As String
        Get

            Return m_sPhoneExtension.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPhoneExtension.Value = Value.Trim()

        End Set
    End Property

    Public Property FaxAreaCode() As String
        Get

            Return m_sFaxAreaCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sFaxAreaCode.Value = Value.Trim()

        End Set
    End Property

    Public Property FaxNumber() As String
        Get

            Return m_sFaxNumber.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sFaxNumber.Value = Value.Trim()

        End Set
    End Property

    Public Property FaxExtension() As String
        Get

            Return m_sFaxExtension.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sFaxExtension.Value = Value.Trim()

        End Set
    End Property

    Public Property PaymentName() As String
        Get

            Return m_sPaymentName.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPaymentName.Value = Value.Trim()

        End Set
    End Property

    Public Property PaymentAccountCode() As String
        Get

            Return m_sPaymentAccountCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPaymentAccountCode.Value = Value.Trim()

        End Set
    End Property

    Public Property PaymentBranchCode() As String
        Get

            Return m_sPaymentBranchCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPaymentBranchCode.Value = Value.Trim()

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

            Return m_sPaymentReference1.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPaymentReference1.Value = Value.Trim()

        End Set
    End Property

    Public Property PaymentReference2() As String
        Get

            Return m_sPaymentReference2.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sPaymentReference2.Value = Value.Trim()

        End Set
    End Property

    'RAW 17/12/2002 : PS187 : Added
    Public Property ProofListReportID() As Integer
        Get

            Return m_lProofListReportID

        End Get
        Set(ByVal Value As Integer)

            m_lProofListReportID = Value

        End Set
    End Property

    Public Property BordereauReportID() As Integer
        Get

            Return m_lBordereauReportID

        End Get
        Set(ByVal Value As Integer)

            m_lBordereauReportID = Value

        End Set
    End Property
    'RAW 17/12/2002 : PS187 : End
    Public Property CreditLimit() As Double
        Get

            Return m_vdCreditLimit

        End Get
        Set(ByVal Value As Double)


            m_vdCreditLimit = CDec(Value)


        End Set
    End Property

    Public Property DiscountPercentage() As Double
        Get

            Return m_vdDiscountPercentage

        End Get
        Set(ByVal Value As Double)


            m_vdDiscountPercentage = CDec(Value)
            'm_vdDiscountPercentage = Value

        End Set
    End Property

    Public Property SettlementPeriod() As Integer
        Get

            Return m_iSettlementPeriod

        End Get
        Set(ByVal Value As Integer)

            m_iSettlementPeriod = Value

        End Set
    End Property

    Public Property BankName() As String
        Get

            Return m_sBankName.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankName.Value = Value.Trim()

        End Set
    End Property

    Public Property BankAddress1() As String
        Get

            Return m_sBankAddress1.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankAddress1.Value = Value.Trim()

        End Set
    End Property

    Public Property BankAddress2() As String
        Get

            Return m_sBankAddress2.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankAddress2.Value = Value.Trim()

        End Set
    End Property

    Public Property BankAddress3() As String
        Get

            Return m_sBankAddress3.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankAddress3.Value = Value.Trim()

        End Set
    End Property

    Public Property BankAddress4() As String
        Get

            Return m_sBankAddress4.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankAddress4.Value = Value.Trim()

        End Set
    End Property

    Public Property BankPostalCode() As String
        Get

            Return m_sBankPostalCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankPostalCode.Value = Value.Trim()

        End Set
    End Property

    Public Property BankCountry() As Integer
        Get

            Return m_iBankCountry

        End Get
        Set(ByVal Value As Integer)

            m_iBankCountry = Value

        End Set
    End Property

    Public Property BankPhoneAreaCode() As String
        Get

            Return m_sBankPhoneAreaCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankPhoneAreaCode.Value = Value.Trim()

        End Set
    End Property

    Public Property BankPhoneNumber() As String
        Get

            Return m_sBankPhoneNumber.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankPhoneNumber.Value = Value.Trim()

        End Set
    End Property

    Public Property BankPhoneExtension() As String
        Get

            Return m_sBankPhoneExtension.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankPhoneExtension.Value = Value.Trim()

        End Set
    End Property

    Public Property BankFaxAreaCode() As String
        Get

            Return m_sBankFaxAreaCode.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankFaxAreaCode.Value = Value.Trim()

        End Set
    End Property

    Public Property BankFaxNumber() As String
        Get

            Return m_sBankFaxNumber.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankFaxNumber.Value = Value.Trim()

        End Set
    End Property

    Public Property BankFaxExtension() As String
        Get

            Return m_sBankFaxExtension.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sBankFaxExtension.Value = Value.Trim()

        End Set
    End Property

    Public Property Comments() As String
        Get

            Return m_sComments.Value.Trim()

        End Get
        Set(ByVal Value As String)

            m_sComments.Value = Value.Trim()

        End Set
    End Property

    ' TF131198
    Public Property AccountKey() As Integer
        Get

            Return m_lAccountKey

        End Get
        Set(ByVal Value As Integer)

            m_lAccountKey = Value

        End Set
    End Property

    ' CF150199

    Public Property NominalAccountID() As Integer
        Get

            Return m_lNominalAccountID

        End Get
        Set(ByVal Value As Integer)

            m_lNominalAccountID = Value

        End Set
    End Property

    ' CF030399

    Public Property AccountStatusID() As Integer
        Get

            Return m_iAccountStatusID

        End Get
        Set(ByVal Value As Integer)

            m_iAccountStatusID = Value

        End Set
    End Property

    'SD 09/01/2003 Start

    Public Property IsTakenOffHold() As Boolean
        Get

            Return m_bIsTakenOffHold

        End Get
        Set(ByVal Value As Boolean)

            m_bIsTakenOffHold = Value

        End Set
    End Property
    'SD 09/01/2003 End
    ' RDC 12112003

    Public Property AllowElectronicPayment() As Boolean
        Get
            Return m_bAllowElectronicPayment
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowElectronicPayment = Value
        End Set
    End Property


    Public Property MoneyCalcAccType() As Integer
        Get

            Return m_iMoneyCalcAccType

        End Get
        Set(ByVal Value As Integer)

            m_iMoneyCalcAccType = Value

        End Set
    End Property


    Public Property ClientBankAccType() As String
        Get

            Return m_vClientBankAccType

        End Get
        Set(ByVal Value As String)


            m_vClientBankAccType = CStr(Value.Trim())

        End Set
    End Property

    Public Property MerchantId() As String
        Get

            Return m_sMerchantId

        End Get
        Set(ByVal Value As String)

            m_sMerchantId = Value.Trim()

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
        Set(ByVal Value As String)
            m_sBIC = Value
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
        Set(ByVal Value As String)
            m_sIBAN = Value
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
            'Developer Guide No 98
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Class_Initialize"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
