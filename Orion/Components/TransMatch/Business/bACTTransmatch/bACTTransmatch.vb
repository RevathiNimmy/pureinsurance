Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Transmatch_NET.Transmatch")>
Public NotInheritable Class Transmatch
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Transmatch
    '
    ' Date: 04/10/1997
    '
    ' Description: Describes the Transmatch attributes.
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
    Private Const ACClass As String = "Transmatch"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lTransmatchID As Integer
    Private m_lAllocationdetailID As Integer
    Private m_lTransdetailID As Integer
    Private m_lMatchID As Integer
    Private m_cBaseMatchAmount As Decimal
    Private m_cCurrencyMatchAmount As Decimal
    Private m_vdCurrencyMatchXrate As Decimal


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


    Public Property TransmatchID() As Integer
        Get

            Return m_lTransmatchID

        End Get
        Set(ByVal Value As Integer)

            m_lTransmatchID = Value

        End Set
    End Property

    Public Property AllocationdetailID() As Integer
        Get

            Return m_lAllocationdetailID

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationdetailID = Value

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

    Public Property MatchID() As Integer
        Get

            Return m_lMatchID

        End Get
        Set(ByVal Value As Integer)

            m_lMatchID = Value

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

    Public Property BaseMatchAmount() As Decimal
        Get

            Return m_cBaseMatchAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cBaseMatchAmount = Value

        End Set
    End Property

    Public Property CurrencyMatchAmount() As Decimal
        Get

            Return m_cCurrencyMatchAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cCurrencyMatchAmount = Value

        End Set
    End Property

    Public Property CurrencyMatchXrate() As Double
        Get

            Return m_vdCurrencyMatchXrate

        End Get
        Set(ByVal Value As Double)


            m_vdCurrencyMatchXrate = CDec(Value)

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
