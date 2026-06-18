Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Transdetail_NET.Transdetail")>
Public NotInheritable Class Transdetail
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Transdetail
    '
    ' Date: 08/08/1997
    '
    ' Description: Describes the Transdetail attributes.
    '
    ' Edit History: TF191198 - amendments for EMU database changes
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 26/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyIDx As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "TransDetail"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lTransdetailID As Integer
    Private m_lAccountID As Integer
    Private m_iPostingstatusID As Integer
    Private m_iCompanyID As Integer
    Private m_lPeriodID As Integer
    Private m_lDocumentID As Integer
    Private m_iDocumentSequence As Integer
    Private m_dtAccountingDate As Date
    Private m_iFullyMatched As Integer
    Private m_iEuroCurrencyId As Integer
    Private m_cEuroAmount As Decimal

    'Private m_vdEuroBaseXrate As Byte
    'Private m_vdEuroCcyXrate As Byte
    Private m_vdEuroBaseXrate As Object
    Private m_vdEuroCcyXrate As Object
    Private m_sComment As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_iOperatorID As Integer
    Private m_sPurchaseOrderNo As String = ""
    Private m_sPurchaseInvoiceNo As String = ""
    Private m_sDepartment As String = ""
    Private m_sSpare As String = ""
    Private m_dtRefDate As Date
    Private m_cRefAmount As Decimal

    Private m_vdRefQuantity As Object
    Private m_sRefUnits As New StringsHelper.FixedLengthString(30)

    Private m_vUnderwritingYearID As Object
    Private m_lDepartmentID As Integer
    'Developre Guide No 101
    Private m_vdCurrencyBaseXrate As Object
    Private m_dtCurrencyBaseDate As Date 'Write Only - Not stored on transdetail record.
    'Developr Guide No 101
    Private m_vdAccountBaseXrate As Object
    Private m_dtAccountBaseDate As Date 'Write Only - Not stored on transdetail record.

    Private m_vdSystemBaseXrate As Object
    Private m_dtSystemBaseDate As Date 'Write Only - Not stored on transdetail record.
    Private m_iTransdetailTypeID As Integer
    Private m_sReference As String = ""
    Private m_sTypeCode As String = ""

    Private m_vlInsuranceRefIndex As Object
    ' RDC 25102005
    Private m_lTaxGroupID As Integer
    Private m_lTaxBandID As Integer

    'Currency IDs on Transdetail
    Private m_iCurrencyID As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_iAccountCurrencyID As Integer
    Private m_iSystemCurrencyID As Integer

    'Currency Amounts on Transdetail
    Private m_cCurrencyAmount As Decimal

    Private m_vdCurrencyAmountUnrounded As Object
    Private m_cAmount As Decimal

    Private m_vdBaseAmountUnrounded As Object
    Private m_cAccountAmount As Decimal

    Private m_vdAccountAmountUnrounded As Object
    Private m_cSystemAmount As Decimal

    Private m_vdSystemAmountUnrounded As Object

    'Outstanding Amounts on Transdetail

    Private m_vdOutstandingCurrencyAmount As Object
    Private m_vdOutstandingAmount As Object
    Private m_vdOutstandingAccountAmount As Object
    Private m_vdOutstandingSystemAmount As Object
    Private m_dtAmountUpdated As Date
    'S4B Claim Enhancements R&D 2005
    Private m_vsClaimReference As Object
    Private m_vBalanceType As Object
    Private m_vlRiskTransfer As Object
    Private m_dtDueDate As Date
    Private m_oFeeType As Object
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property FeeType() As Object
        Get
            Return m_oFeeType
        End Get
        Set(ByVal Value As Object)
            m_oFeeType = Value
        End Set
    End Property
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

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

    Public Property AccountID() As Integer
        Get

            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            m_lAccountID = Value

        End Set
    End Property

    Public Property PostingstatusID() As Integer
        Get

            Return m_iPostingstatusID

        End Get
        Set(ByVal Value As Integer)

            m_iPostingstatusID = Value

        End Set
    End Property

    Public Property CompanyID() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

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

    Public Property PeriodID() As Integer
        Get

            Return m_lPeriodID

        End Get
        Set(ByVal Value As Integer)

            m_lPeriodID = Value

        End Set
    End Property

    Public Property DocumentID() As Integer
        Get

            Return m_lDocumentID

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentID = Value

        End Set
    End Property

    Public Property DocumentSequence() As Integer
        Get

            Return m_iDocumentSequence

        End Get
        Set(ByVal Value As Integer)

            m_iDocumentSequence = Value

        End Set
    End Property

    Public Property AccountingDate() As Date
        Get

            Return m_dtAccountingDate

        End Get
        Set(ByVal Value As Date)

            m_dtAccountingDate = Value

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

    Public Property BaseAmountUnrounded() As Object
        Get

            Return m_vdBaseAmountUnrounded

        End Get
        Set(ByVal Value As Object)

            m_vdBaseAmountUnrounded = Value

        End Set
    End Property

    Public Property FullyMatched() As Integer
        Get

            Return m_iFullyMatched

        End Get
        Set(ByVal Value As Integer)

            m_iFullyMatched = Value

        End Set
    End Property

    Public Property CurrencyAmount() As Decimal
        Get

            Return m_cCurrencyAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cCurrencyAmount = Value

        End Set
    End Property

    Public Property CurrencyAmountUnrounded() As Object
        Get

            Return m_vdCurrencyAmountUnrounded

        End Get
        Set(ByVal Value As Object)

            m_vdCurrencyAmountUnrounded = Value

        End Set
    End Property

    Public Property EuroCurrencyId() As Integer
        Get

            Return m_iEuroCurrencyId

        End Get
        Set(ByVal Value As Integer)

            m_iEuroCurrencyId = Value

        End Set
    End Property

    Public Property EuroAmount() As Decimal
        Get

            Return m_cEuroAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cEuroAmount = Value

        End Set
    End Property

    Public Property EuroBaseXrate() As Object
        Get

            Return m_vdEuroBaseXrate

        End Get
        Set(ByVal Value As Object)

            m_vdEuroBaseXrate = Value

        End Set
    End Property

    Public Property EuroCcyXrate() As Object
        Get

            Return m_vdEuroCcyXrate

        End Get
        Set(ByVal Value As Object)

            m_vdEuroCcyXrate = Value

        End Set
    End Property

    Public Property Comment() As String
        Get

            Return m_sComment

        End Get
        Set(ByVal Value As String)

            m_sComment = Value

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

    Public Property OperatorID() As Integer
        Get

            Return m_iOperatorID

        End Get
        Set(ByVal Value As Integer)

            m_iOperatorID = Value

        End Set
    End Property

    Public Property PurchaseOrderNo() As String
        Get

            Return m_sPurchaseOrderNo

        End Get
        Set(ByVal Value As String)

            m_sPurchaseOrderNo = Value

        End Set
    End Property

    Public Property PurchaseInvoiceNo() As String
        Get

            Return m_sPurchaseInvoiceNo

        End Get
        Set(ByVal Value As String)

            m_sPurchaseInvoiceNo = Value

        End Set
    End Property

    Public Property Department() As String
        Get

            Return m_sDepartment

        End Get
        Set(ByVal Value As String)

            m_sDepartment = Value

        End Set
    End Property

    Public Property Spare() As String
        Get

            Return m_sSpare

        End Get
        Set(ByVal Value As String)

            m_sSpare = Value

        End Set
    End Property

    Public Property RefDate() As Date
        Get

            Return m_dtRefDate

        End Get
        Set(ByVal Value As Date)

            m_dtRefDate = Value

        End Set
    End Property

    Public Property RefAmount() As Decimal
        Get

            Return m_cRefAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cRefAmount = Value

        End Set
    End Property

    Public Property RefQuantity() As Object
        Get

            Return m_vdRefQuantity

        End Get
        Set(ByVal Value As Object)

            m_vdRefQuantity = Value

        End Set
    End Property

    Public Property RefUnits() As String
        Get

            Return m_sRefUnits.Value

        End Get
        Set(ByVal Value As String)

            m_sRefUnits.Value = Value

        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return m_lDepartmentID
        End Get
        Set(ByVal Value As Integer)
            m_lDepartmentID = Value
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

    Public Property CurrencyBaseXrate() As Object
        Get
            Return m_vdCurrencyBaseXrate
        End Get
        Set(ByVal Value As Object)

            m_vdCurrencyBaseXrate = Value
        End Set
    End Property

    Public Property CurrencyBaseDate() As Date
        Get
            Return m_dtCurrencyBaseDate
        End Get
        Set(ByVal Value As Date)
            m_dtCurrencyBaseDate = Value
        End Set
    End Property

    'Develoepr Guide No 101

    Public Property AccountBaseXrate() As Object
        Get
            Return m_vdAccountBaseXrate
        End Get
        Set(ByVal Value As Object)

            m_vdAccountBaseXrate = Value
        End Set
    End Property

    Public Property AccountBaseDate() As Date
        Get
            Return m_dtAccountBaseDate
        End Get
        Set(ByVal Value As Date)
            m_dtAccountBaseDate = Value
        End Set
    End Property

    Public Property SystemBaseXrate() As Object
        Get
            Return m_vdSystemBaseXrate
        End Get
        Set(ByVal Value As Object)

            m_vdSystemBaseXrate = Value
        End Set
    End Property

    Public Property SystemBaseDate() As Date
        Get
            Return m_dtSystemBaseDate
        End Get
        Set(ByVal Value As Date)
            m_dtSystemBaseDate = Value
        End Set
    End Property

    Public Property TransdetailTypeID() As Integer
        Get
            Return m_iTransdetailTypeID
        End Get
        Set(ByVal Value As Integer)
            m_iTransdetailTypeID = Value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return m_sReference
        End Get
        Set(ByVal Value As String)
            m_sReference = Value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return m_sTypeCode
        End Get
        Set(ByVal Value As String)
            m_sTypeCode = Value
        End Set
    End Property

    Public Property BaseCurrencyID() As Integer
        Get
            Return m_iBaseCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iBaseCurrencyID = Value
        End Set
    End Property

    Public Property AccountCurrencyID() As Integer
        Get
            Return m_iAccountCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iAccountCurrencyID = Value
        End Set
    End Property

    Public Property SystemCurrencyID() As Integer
        Get
            Return m_iSystemCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iSystemCurrencyID = Value
        End Set
    End Property

    Public Property AccountAmount() As Decimal
        Get
            Return m_cAccountAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cAccountAmount = Value
        End Set
    End Property

    Public Property AccountAmountUnrounded() As Object
        Get
            Return m_vdAccountAmountUnrounded
        End Get
        Set(ByVal Value As Object)

            m_vdAccountAmountUnrounded = Value
        End Set
    End Property

    Public Property SystemAmount() As Decimal
        Get
            Return m_cSystemAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cSystemAmount = Value
        End Set
    End Property

    Public Property SystemAmountUnrounded() As Object
        Get
            Return m_vdSystemAmountUnrounded
        End Get
        Set(ByVal Value As Object)

            m_vdSystemAmountUnrounded = Value
        End Set
    End Property

    Public Property OSCurrencyAmount() As Object
        Get
            Return m_vdOutstandingCurrencyAmount
        End Get
        Set(ByVal Value As Object)

            m_vdOutstandingCurrencyAmount = Value
        End Set
    End Property

    Public Property OSBaseAmount() As Object
        Get
            Return m_vdOutstandingAmount
        End Get
        Set(ByVal Value As Object)

            m_vdOutstandingAmount = Value
        End Set
    End Property

    Public Property OSAccountAmount() As Object
        Get
            Return m_vdOutstandingAccountAmount
        End Get
        Set(ByVal Value As Object)

            m_vdOutstandingAccountAmount = Value
        End Set
    End Property

    Public Property OSSystemAmount() As Object
        Get
            Return m_vdOutstandingSystemAmount
        End Get
        Set(ByVal Value As Object)

            m_vdOutstandingSystemAmount = Value
        End Set
    End Property

    Public Property AmountUpdated() As Date
        Get
            Return m_dtAmountUpdated
        End Get
        Set(ByVal Value As Date)
            m_dtAmountUpdated = Value
        End Set
    End Property

    Public Property DueDate() As Date
        Get
            Return m_dtDueDate
        End Get
        Set(ByVal value As Date)
            m_dtDueDate = value
        End Set
    End Property

    Public Property InsuranceRefIndex() As Object
        Get
            Return m_vlInsuranceRefIndex
        End Get
        Set(ByVal Value As Object)

            m_vlInsuranceRefIndex = Value
        End Set
    End Property

    ' RDC 25102005

    Public Property TaxGroupID() As Integer
        Get
            Return m_lTaxGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lTaxGroupID = Value
        End Set
    End Property

    Public Property TaxBandID() As Integer
        Get
            Return m_lTaxBandID
        End Get
        Set(ByVal Value As Integer)
            m_lTaxBandID = Value
        End Set
    End Property

    'S4B Claim Enhancements R&D 2005
    'Developre Guide No 101

    Public Property ClaimReference() As Object
        Get
            Return m_vsClaimReference
        End Get
        Set(ByVal Value As Object)

            m_vsClaimReference = Value
        End Set
    End Property

    Public Property BalanceType() As Object
        Get
            Return m_vBalanceType
        End Get
        Set(ByVal Value As Object)

            m_vBalanceType = Value
        End Set
    End Property

    Public Property RiskTransfer() As Object
        Get
            Return m_vlRiskTransfer
        End Get
        Set(ByVal Value As Object)

            m_vlRiskTransfer = Value
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyIDx = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Initialisation Code.

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        '

        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        ''

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
