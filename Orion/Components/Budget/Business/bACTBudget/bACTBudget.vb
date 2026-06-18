Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("ACTBudget_NET.ACTBudget")> _
Public NotInheritable Class ACTBudget
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ACTBudget
    '
    ' Date: 20/10/1998
    '
    ' Description: Describes the ACTBudget attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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
    Private Const ACClass As String = "ACTBudget"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lBudgetID As Integer
    Private m_sBudgetRef As New FixedLengthString(20)
    Private m_lPeriodID As Integer
    Private m_sBudgetDescription As New FixedLengthString(255)
    Private m_sPeriodYearName As New FixedLengthString(20)
    Private m_lRevisesBudgetID As Integer
    Private m_iBudgetStatusID As Integer


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


    Public Property BudgetID() As Integer
        Get

            Return m_lBudgetID

        End Get
        Set(ByVal Value As Integer)

            m_lBudgetID = Value

        End Set
    End Property

    Public Property BudgetRef() As String
        Get

            Return m_sBudgetRef.Value

        End Get
        Set(ByVal Value As String)

            m_sBudgetRef.Value = Value

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

    Public Property BudgetDescription() As String
        Get

            Return m_sBudgetDescription.Value

        End Get
        Set(ByVal Value As String)

            m_sBudgetDescription.Value = Value

        End Set
    End Property

    Public Property PeriodYearName() As String
        Get

            Return m_sPeriodYearName.Value

        End Get
        Set(ByVal Value As String)

            m_sPeriodYearName.Value = Value

        End Set
    End Property

    Public Property RevisesBudgetID() As Integer
        Get

            Return m_lRevisesBudgetID

        End Get
        Set(ByVal Value As Integer)

            m_lRevisesBudgetID = Value

        End Set
    End Property

    Public Property BudgetStatusID() As Integer
        Get

            Return m_iBudgetStatusID

        End Get
        Set(ByVal Value As Integer)

            m_iBudgetStatusID = Value

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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