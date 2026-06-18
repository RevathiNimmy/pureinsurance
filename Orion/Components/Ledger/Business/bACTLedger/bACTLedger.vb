Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Ledger_NET.Ledger")>
Public NotInheritable Class Ledger
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Ledger
    '
    ' Date: 23/07/1997
    '
    ' Description: Describes the Ledger attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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
    Private Const ACClass As String = "Ledger"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_iLedgerID As Integer
    Private m_lCompanyID As Integer
    Private m_lSubBranchID As Integer
    Private m_sLedgerName As New StringsHelper.FixedLengthString(30)
    Private m_sLedgerShortName As New StringsHelper.FixedLengthString(2)
    Private m_lMappingID As Integer
    Private m_iLedgertypeID As Integer
    Private m_iIsDeletable As Integer
    Private m_lCurrentPeriodID As Integer
    Private m_iSequence As Integer


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


    Public Property CurrentPeriodID() As Integer
        Get

            Return m_lCurrentPeriodID

        End Get
        Set(ByVal Value As Integer)

            m_lCurrentPeriodID = Value

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


    Public Property Sequence() As Integer
        Get

            Return m_iSequence

        End Get
        Set(ByVal Value As Integer)

            m_iSequence = Value

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

    Public Property CompanyID() As Integer
        Get

            Return m_lCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_lCompanyID = Value

        End Set
    End Property

    Public Property SubBranchID() As Integer
        Get

            Return m_lSubBranchID

        End Get
        Set(ByVal Value As Integer)

            m_lSubBranchID = Value

        End Set
    End Property

    Public Property LedgerName() As String
        Get

            Return m_sLedgerName.Value

        End Get
        Set(ByVal Value As String)

            m_sLedgerName.Value = Value

        End Set
    End Property

    Public Property LedgerShortName() As String
        Get

            Return m_sLedgerShortName.Value

        End Get
        Set(ByVal Value As String)

            m_sLedgerShortName.Value = Value

        End Set
    End Property

    Public Property MappingID() As Integer
        Get

            Return m_lMappingID

        End Get
        Set(ByVal Value As Integer)

            m_lMappingID = Value

        End Set
    End Property

    Public Property LedgertypeID() As Integer
        Get

            Return m_iLedgertypeID

        End Get
        Set(ByVal Value As Integer)

            m_iLedgertypeID = Value

        End Set
    End Property

    Public Property IsDeletable() As Integer
        Get

            Return m_iIsDeletable

        End Get
        Set(ByVal Value As Integer)

            m_iIsDeletable = Value

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
