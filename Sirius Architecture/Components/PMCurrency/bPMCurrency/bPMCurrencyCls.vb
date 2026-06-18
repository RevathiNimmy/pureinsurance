Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129

<System.Runtime.InteropServices.ProgId("Currency_NET.Currency")> _
Public NotInheritable Class Currency
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Currency
    '
    ' Date: 11/07/1997
    '
    ' Description: Describes the Currency attributes.
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
    Private Const ACClass As String = "Currency"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lCaptionID As Integer
    Private m_sIsoCode As New StringsHelper.FixedLengthString(4)
    Private m_sDescription As New StringsHelper.FixedLengthString(255)
    Private m_sMinorPart As String = ""
    Private m_sCode As New StringsHelper.FixedLengthString(10)
    Private m_sSymbol As New StringsHelper.FixedLengthString(4)
    Private m_sAlignment As New StringsHelper.FixedLengthString(1)
    Private m_iDecimalPlaces As Integer
    Private m_iIsDeleted As Integer
    Private m_dtEffectiveDate As Date
    Private m_sFormatString As New StringsHelper.FixedLengthString(255)
    Private m_iRoundToPlaces As Integer
    Private m_bIsBase As Boolean
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""


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


    Public Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_iCurrencyID = Value

        End Set
    End Property

    Public Property CaptionID() As Integer
        Get

            Return m_lCaptionID

        End Get
        Set(ByVal Value As Integer)

            m_lCaptionID = Value

        End Set
    End Property

    Public Property IsoCode() As String
        Get

            Return m_sIsoCode.Value

        End Get
        Set(ByVal Value As String)

            m_sIsoCode.Value = Value

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

    Public Property MinorPart() As String
        Get

            Return m_sMinorPart

        End Get
        Set(ByVal Value As String)

            m_sMinorPart = Value

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

    Public Property Symbol() As String
        Get

            Return m_sSymbol.Value

        End Get
        Set(ByVal Value As String)

            m_sSymbol.Value = Value

        End Set
    End Property

    Public Property Alignment() As String
        Get

            Return m_sAlignment.Value

        End Get
        Set(ByVal Value As String)

            m_sAlignment.Value = Value

        End Set
    End Property

    Public Property DecimalPlaces() As Integer
        Get

            Return m_iDecimalPlaces

        End Get
        Set(ByVal Value As Integer)

            m_iDecimalPlaces = Value

        End Set
    End Property

    Public Property IsDeleted() As Integer
        Get

            Return m_iIsDeleted

        End Get
        Set(ByVal Value As Integer)

            m_iIsDeleted = Value

        End Set
    End Property

    Public Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    Public Property FormatString() As String
        Get

            Return m_sFormatString.Value

        End Get
        Set(ByVal Value As String)

            m_sFormatString.Value = Value

        End Set
    End Property

    Public Property RoundToPlaces() As Integer
        Get

            Return m_iRoundToPlaces

        End Get
        Set(ByVal Value As Integer)

            m_iRoundToPlaces = Value

        End Set
    End Property

    Public Property IsBase() As Boolean
        Get

            Return m_bIsBase

        End Get
        Set(ByVal Value As Boolean)

            m_bIsBase = Value

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
