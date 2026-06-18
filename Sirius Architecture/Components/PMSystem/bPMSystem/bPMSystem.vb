Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("PMSystem_NET.PMSystem")> _
Public NotInheritable Class PMSystem
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMSystem
    '
    ' Date: 24th October 1996
    '
    ' Description: Describes the PMSystem attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
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
    Private Const ACClass As String = "PMSystem"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_iSystemID As Integer
    Private m_iProductID As Integer
    Private m_sSystemName As New StringsHelper.FixedLengthString(40)
    Private m_iDefaultSourceID As Integer
    Private m_iHomeCountryID As Integer
    Private m_iLicenceLimit As Integer
    Private m_sLicenceKey As New StringsHelper.FixedLengthString(30)
    Private m_iPoolSize As Integer
    Private m_vTimestamp As Object
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

    Public Property SystemID() As Integer
        Get

            Return m_iSystemID

        End Get
        Set(ByVal Value As Integer)

            m_iSystemID = Value

        End Set
    End Property

    Public Property ProductID() As Integer
        Get

            Return m_iProductID

        End Get
        Set(ByVal Value As Integer)

            m_iProductID = Value

        End Set
    End Property

    Public Property SystemName() As String
        Get

            Return m_sSystemName.Value

        End Get
        Set(ByVal Value As String)

            m_sSystemName.Value = Value

        End Set
    End Property

    Public Property DefaultSourceID() As Integer
        Get

            Return m_iDefaultSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iDefaultSourceID = Value

        End Set
    End Property

    Public Property HomeCountryID() As Integer
        Get

            Return m_iHomeCountryID

        End Get
        Set(ByVal Value As Integer)

            m_iHomeCountryID = Value

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

    Public Property LanguageID() As Integer
        Get

            Return m_iLanguageID

        End Get
        Set(ByVal Value As Integer)

            m_iLanguageID = Value

        End Set
    End Property

    Public Property LicenceLimit() As Integer
        Get

            Return m_iLicenceLimit

        End Get
        Set(ByVal Value As Integer)

            m_iLicenceLimit = Value

        End Set
    End Property

    Public Property LicenceKey() As String
        Get

            Return m_sLicenceKey.Value

        End Get
        Set(ByVal Value As String)

            m_sLicenceKey.Value = Value

        End Set
    End Property

    Public Property LogLevel() As Integer
        Get

            Return m_iLogLevel

        End Get
        Set(ByVal Value As Integer)

            m_iLogLevel = Value

        End Set
    End Property

    Public Property PoolSize() As Integer
        Get

            Return m_iPoolSize

        End Get
        Set(ByVal Value As Integer)

            m_iPoolSize = Value

        End Set
    End Property

    Public Property Timestamp() As Object
        Get

            Return m_vTimestamp

        End Get
        Set(ByVal Value As Object)



            m_vTimestamp = Value

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

            '
            '    ' *******************************************************************
            '    ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            '    ' Set Username and Password
            '    m_sUsername$ = sUserName$
            '    m_sPassword$ = sPassword$
            '    m_iUserID% = iUserID%
            '    m_sCallingAppName$ = sCallingAppName$
            '    m_iLanguageID% = iLanguageID%
            '    m_iSourceID% = iSourceID%
            '    m_iCurrencyID% = iCurrencyID%
            '    m_iLogLevel% = iLogLevel%


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

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

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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

End Class
