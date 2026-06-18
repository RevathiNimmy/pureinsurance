Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared
'<System.Runtime.InteropServices.ProgId("PMUserSource_NET.PMUserSource")> _
Public NotInheritable Class PMUserSource
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMUserSource
    '
    ' Date: 14th April 2000
    '
    ' Description: Describes the PMUserSource attributes.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 02/04/2007
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
    Private Const ACClass As String = "PMUserSource"

    ' PUBLIC Data Members (Begin)
    'PWF 10/10/2002: Is item saved?
    Public IsSaved As Boolean

    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' SourceID
    ' SourceCode
    Private m_sSourceCode As String = ""
    'DAK220500
    ' Description
    Private m_sDescription As String = ""
    ' CountryID
    Private m_lCountryID As Integer

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public Property SourceCode() As String
        Get
            Return m_sSourceCode
        End Get
        Set(ByVal Value As String)
            m_sSourceCode = Value
        End Set
    End Property

    'DAK220500
    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
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

    Public Property CountryID() As Integer
        Get
            Return m_lCountryID
        End Get
        Set(ByVal Value As Integer)
            m_lCountryID = Value
        End Set
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initiaslisation code for this
    '              object.
    '
    ' History: 14/04/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' History: 14/04/2000 DAK - Created.
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


    ' ***************************************************************** '
    '
    ' Name: Class_Initialize
    '
    ' Description:
    '
    ' History: 14/04/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: Class_Terminate
    '
    ' Description:
    '
    ' History: 14/04/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
