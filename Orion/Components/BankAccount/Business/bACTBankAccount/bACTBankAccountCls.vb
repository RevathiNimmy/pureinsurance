Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("BankAccount_NET.BankAccount")> _
Public NotInheritable Class BankAccount
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: BankAccount
    '
    ' Date: 09/09/1997
    '
    ' Description: Describes the BankAccount attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 07/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "BankAccount"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_iBankAccountID As Integer
    Private m_iCompanyID As Integer
    Private m_lAccountID As Integer
    Private m_iBankID As Integer
    Private m_sCode As New StringsHelper.FixedLengthString(10)
    Private m_sBankAccountNo As New StringsHelper.FixedLengthString(30)
    Private m_sBankAccountName As New StringsHelper.FixedLengthString(60)
    Private m_sDescription As New StringsHelper.FixedLengthString(255)
    Private m_lNextChequeNumber As Integer
    Private m_vReconciledDate As Object
    Private m_lDefaultBankAccountID As Integer
    Private m_iBankAccountTypeId As Integer
    Private m_iIsCashReceiveInThisCurrencyOnly As Integer
    Private m_sStartChequeNumber As String = ""
    'Start (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.2.1.1)
    Private m_sFinancialInstitutionCode As String = ""
    Private m_sDirectDebitSupplierName As String = ""
    Private m_lDirectDebitSupplierID As Integer
    Private m_sRemitter As String = ""
    Private m_iProcessingDays As Integer
    Private m_sBIC As String = ""
    Private m_sIBAN As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""


    Public Property FinancialInstitutionCode() As String
        Get
            Return m_sFinancialInstitutionCode
        End Get
        Set(ByVal Value As String)
            m_sFinancialInstitutionCode = Value
        End Set
    End Property

    Public Property DirectDebitSupplierName() As String
        Get
            Return m_sDirectDebitSupplierName
        End Get
        Set(ByVal Value As String)
            m_sDirectDebitSupplierName = Value
        End Set
    End Property

    Public Property DirectDebitSupplierID() As Integer
        Get
            Return m_lDirectDebitSupplierID
        End Get
        Set(ByVal Value As Integer)
            m_lDirectDebitSupplierID = Value
        End Set
    End Property

    Public Property Remitter() As String
        Get
            Return m_sRemitter
        End Get
        Set(ByVal Value As String)
            m_sRemitter = Value
        End Set
    End Property

    Public Property ProcessingDays() As Integer
        Get
            Return m_iProcessingDays
        End Get
        Set(ByVal Value As Integer)
            m_iProcessingDays = Value
        End Set
    End Property

    ''' <summary>
    ''' Business Identifier Codes(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return Me.m_sBIC
        End Get
        Set(ByVal value As String)
            Me.m_sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return Me.m_sIBAN
        End Get
        Set(ByVal value As String)
            Me.m_sIBAN = value
        End Set
    End Property

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


    Public Property BankAccountID() As Integer
        Get

            Return m_iBankAccountID

        End Get
        Set(ByVal Value As Integer)

            m_iBankAccountID = Value

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

    Public Property CompanyID() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

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

    Public Property BankID() As Integer
        Get

            Return m_iBankID

        End Get
        Set(ByVal Value As Integer)

            m_iBankID = Value

        End Set
    End Property

    Public Property Code() As String
        Get

            Return m_sCode.Value

        End Get
        Set(ByVal Value As String)

            m_sCode.Value = Value

        End Set
    End Property

    Public Property BankAccountNo() As String
        Get

            Return m_sBankAccountNo.Value

        End Get
        Set(ByVal Value As String)

            m_sBankAccountNo.Value = Value

        End Set
    End Property

    Public Property BankAccountName() As String
        Get

            Return m_sBankAccountName.Value

        End Get
        Set(ByVal Value As String)

            m_sBankAccountName.Value = Value

        End Set
    End Property

    Public Property Description() As String
        Get

            Return m_sDescription.Value

        End Get
        Set(ByVal Value As String)

            m_sDescription.Value = Value

        End Set
    End Property

    Public Property NextChequeNumber() As Integer
        Get

            Return m_lNextChequeNumber

        End Get
        Set(ByVal Value As Integer)

            m_lNextChequeNumber = Value

        End Set
    End Property

    Public Property ReconciledDate() As Object
        Get

            Return m_vReconciledDate

        End Get
        Set(ByVal Value As Object)



            m_vReconciledDate = Value

        End Set
    End Property
    Public Property StartChequeNumber() As String
        Get

            Return m_sStartChequeNumber

        End Get
        Set(ByVal Value As String)

            m_sStartChequeNumber = Value

        End Set
    End Property


    Public Property DefaultBankAccountID() As Integer
        Get
            Return m_lDefaultBankAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lDefaultBankAccountID = Value
        End Set
    End Property


    Public Property BankAccountTypeId() As Integer
        Get
            Return m_iBankAccountTypeId
        End Get
        Set(ByVal Value As Integer)
            m_iBankAccountTypeId = Value
        End Set
    End Property


    Public Property IsCashReceiveInThisCurrencyOnly() As Integer
        Get
            Return m_iIsCashReceiveInThisCurrencyOnly
        End Get
        Set(ByVal Value As Integer)
            m_iIsCashReceiveInThisCurrencyOnly = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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