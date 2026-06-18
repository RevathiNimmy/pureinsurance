Option Strict Off
Option Explicit On
Imports SSP.Shared
Imports SSP.Shared.StringsHelper
'developer guide no.129
<System.Runtime.InteropServices.ProgId("CashList_NET.CashList")>
Public NotInheritable Class CashList
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CashList
    '
    ' Date: 03/09/1997
    '
    ' Description: Describes the CashList attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 20/10/2003
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
    Private Const ACClass As String = "CashList"

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lCashlistID As Integer
    Private m_lBankaccountID As Integer
    Private m_lCashlisttypeID As Integer
    Private m_lCashliststatusID As Integer
    Private m_sCashlistRef As New FixedLengthString(25)
    Private m_iCompanyID As Integer
    Private m_dtListDate As Date
    Private m_cControlTotal As Decimal
    Private m_lItemCount As Integer
    'pkh 07/10/2002 - New variables to support Front Office Receipting
    Private m_lCashList_drawer_id As Integer
    Private m_lBatch_id As Integer
    Private m_iPMUser_id As Integer
    Private m_iConfirm_pmuser_id As Integer
    Private m_iConfirm2_pmuser_id As Integer
    Private m_dtDate_Approved As Date
    Private m_cBanking_Total As Decimal
    Private m_cCash_Float_Amount As Decimal
    'pkh 07/10/2002 - ends
    'sw someone forgot this field
    Private m_dtDepositDate As Date
    ' KG 12/06/03 - Sub Branh ID
    Private m_iSubBranchID As Integer
    Private m_bIsSplitReceipt As Boolean

    ' PUBLIC Property Procedures (Begin)
    'add this pair of PPP's sw 07/01/2003
    Public Property DepositDate() As Date
        Get
            Return m_dtDepositDate
        End Get
        Set(ByVal Value As Date)
            m_dtDepositDate = Value
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
    Public Property CashListID() As Integer
        Get
            Return m_lCashlistID
        End Get
        Set(ByVal Value As Integer)
            m_lCashlistID = Value
        End Set
    End Property
    Public Property CashListStatusID() As Integer
        Get
            Return m_lCashliststatusID
        End Get
        Set(ByVal Value As Integer)
            m_lCashliststatusID = Value
        End Set
    End Property
    Public Property CashListTypeID() As Integer
        Get
            Return m_lCashlisttypeID
        End Get
        Set(ByVal Value As Integer)
            m_lCashlisttypeID = Value
        End Set
    End Property
    Public Property CashListRef() As String
        Get
            Return m_sCashlistRef.Value
        End Get
        Set(ByVal Value As String)
            m_sCashlistRef.Value = Value
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

    Public Property SubBranchID() As Integer
        Get
            Return m_iSubBranchID
        End Get
        Set(ByVal Value As Integer)
            m_iSubBranchID = Value
        End Set
    End Property


    Public Property BankAccountID() As Integer
        Get
            Return m_lBankaccountID
        End Get
        Set(ByVal Value As Integer)
            m_lBankaccountID = Value
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
    Public Property ListDate() As Date
        Get
            Return m_dtListDate
        End Get
        Set(ByVal Value As Date)
            m_dtListDate = Value
        End Set
    End Property
    Public Property ControlTotal() As Decimal
        Get
            Return m_cControlTotal
        End Get
        Set(ByVal Value As Decimal)
            m_cControlTotal = Value
        End Set
    End Property
    Public Property ItemCount() As Integer
        Get
            Return m_lItemCount
        End Get
        Set(ByVal Value As Integer)
            m_lItemCount = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property CashList_drawer_id() As Integer
        Get
            Return m_lCashList_drawer_id
        End Get
        Set(ByVal Value As Integer)
            m_lCashList_drawer_id = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property Batch_id() As Integer
        Get
            Return m_lBatch_id
        End Get
        Set(ByVal Value As Integer)
            m_lBatch_id = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property PMUser_id() As Integer
        Get
            Return m_iPMUser_id
        End Get
        Set(ByVal Value As Integer)
            m_iPMUser_id = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property Confirm_pmuser_id() As Integer
        Get
            Return m_iConfirm_pmuser_id
        End Get
        Set(ByVal Value As Integer)
            m_iConfirm_pmuser_id = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property Confirm2_pmuser_id() As Integer
        Get
            Return m_iConfirm2_pmuser_id
        End Get
        Set(ByVal Value As Integer)
            m_iConfirm2_pmuser_id = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property Date_Approved() As Date
        Get
            Return m_dtDate_Approved
        End Get
        Set(ByVal Value As Date)
            m_dtDate_Approved = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property Banking_Total() As Decimal
        Get
            Return m_cBanking_Total
        End Get
        Set(ByVal Value As Decimal)
            m_cBanking_Total = Value
        End Set
    End Property
    'pkh 07/10/2002 - New Properties to support Front Office Receipting
    Public Property Cash_Float_Amount() As Decimal
        Get
            Return m_cCash_Float_Amount
        End Get
        Set(ByVal Value As Decimal)
            m_cCash_Float_Amount = Value
        End Set
    End Property

    Public Property IsSplitReceipt() As Boolean
        Get
            Return m_bIsSplitReceipt
        End Get
        Set(ByVal value As Boolean)
            m_bIsSplitReceipt = value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    ' Description: Entry point for any initialisation code for this
    '              object.
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            '  m_sUsername$ = sUsername$
            '  m_sPassword$ = sPassword$
            '  m_iUserID% = iUserID%
            '  m_sCallingAppName$ = sCallingAppName$
            '  m_iLanguageID% = iLanguageID%
            '  m_iSourceID% = iSourceID%
            '  m_iCurrencyID% = iCurrencyID%
            '  m_iLogLevel% = iLogLevel%


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Initialise"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Class_Initialize"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
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