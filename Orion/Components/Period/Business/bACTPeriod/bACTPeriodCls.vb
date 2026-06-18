Option Strict Off
Option Explicit On
'developer guide no 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Period_NET.Period")>
Public NotInheritable Class Period
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Period
    '
    ' Date: 31/07/1997
    '
    ' Description: Describes the Period attributes.
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
    Private Const ACClass As String = "Period"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lPeriodID As Integer
    Private m_lCompanyID As Integer
    Private m_lSubBranchID As Integer
    Private m_sYearName As New StringsHelper.FixedLengthString(20)
    Private m_sPeriodName As New StringsHelper.FixedLengthString(15)
    Private m_dtPeriodEndDate As Date
    Private m_iPeriodEndComplete As Integer


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property PeriodEndComplete() As Integer
        Get
            Return m_iPeriodEndComplete
        End Get
        Set(ByVal Value As Integer)
            m_iPeriodEndComplete = Value
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


    Public Property PeriodID() As Integer
        Get

            Return m_lPeriodID

        End Get
        Set(ByVal Value As Integer)

            m_lPeriodID = Value

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

    Public Property YearName() As String
        Get

            Return m_sYearName.Value

        End Get
        Set(ByVal Value As String)

            m_sYearName.Value = Value

        End Set
    End Property

    Public Property PeriodName() As String
        Get

            Return m_sPeriodName.Value

        End Get
        Set(ByVal Value As String)

            m_sPeriodName.Value = Value

        End Set
    End Property

    Public Property PeriodEndDate() As Date
        Get

            Return m_dtPeriodEndDate

        End Get
        Set(ByVal Value As Date)

            m_dtPeriodEndDate = Value

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
