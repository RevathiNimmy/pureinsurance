Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("ACTFindInvoice_NET.ACTFindInvoice")> _
Public NotInheritable Class ACTFindInvoice
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ACTFindInvoice
    '
    ' Date: 20/10/1998
    '
    ' Description: Describes the ACTFindInvoice attributes.
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
    Private Const ACClass As String = "ACTFindInvoice"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lInvoiceID As Integer
    Private m_sInvoiceRef As New FixedLengthString(20)
    Private m_lPeriodID As Integer
    Private m_sInvoiceDescription As New FixedLengthString(255)
    Private m_sPeriodYearName As New FixedLengthString(20)
    Private m_lRevisesInvoiceID As Integer
    Private m_iInvoiceStatusID As Integer


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


    Public Property InvoiceID() As Integer
        Get

            Return m_lInvoiceID

        End Get
        Set(ByVal Value As Integer)

            m_lInvoiceID = Value

        End Set
    End Property

    Public Property InvoiceRef() As String
        Get

            Return m_sInvoiceRef.Value

        End Get
        Set(ByVal Value As String)

            m_sInvoiceRef.Value = Value

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

    Public Property InvoiceDescription() As String
        Get

            Return m_sInvoiceDescription.Value

        End Get
        Set(ByVal Value As String)

            m_sInvoiceDescription.Value = Value

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

    Public Property RevisesInvoiceID() As Integer
        Get

            Return m_lRevisesInvoiceID

        End Get
        Set(ByVal Value As Integer)

            m_lRevisesInvoiceID = Value

        End Set
    End Property

    Public Property InvoiceStatusID() As Integer
        Get

            Return m_iInvoiceStatusID

        End Get
        Set(ByVal Value As Integer)

            m_iInvoiceStatusID = Value

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
