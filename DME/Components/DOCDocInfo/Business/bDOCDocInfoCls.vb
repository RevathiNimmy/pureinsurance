Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("DocInfo_NET.DocInfo")>
Public NotInheritable Class DocInfo
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: DocInfo
    '
    ' Date: 30/01/1998
    '
    ' Description: Describes the DocInfo attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "DocInfo"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lDocNum As Integer
    Private m_dtExpiryDate As Date
    Private m_sScanUser As New StringsHelper.FixedLengthString(16)
    Private m_dtDocDate As Date
    Private m_sLastUser As New StringsHelper.FixedLengthString(16)
    Private m_dtLastDate As Date


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


    Public Property DocNum() As Integer
        Get

            Return m_lDocNum

        End Get
        Set(ByVal Value As Integer)

            m_lDocNum = Value

        End Set
    End Property

    Public Property ExpiryDate() As Date
        Get

            Return m_dtExpiryDate

        End Get
        Set(ByVal Value As Date)

            m_dtExpiryDate = Value

        End Set
    End Property

    Public Property ScanUser() As String
        Get

            Return m_sScanUser.Value

        End Get
        Set(ByVal Value As String)

            m_sScanUser.Value = Value

        End Set
    End Property

    Public Property DocDate() As Date
        Get

            Return m_dtDocDate

        End Get
        Set(ByVal Value As Date)

            m_dtDocDate = Value

        End Set
    End Property

    Public Property LastUser() As String
        Get

            Return m_sLastUser.Value

        End Get
        Set(ByVal Value As String)

            m_sLastUser.Value = Value

        End Set
    End Property

    Public Property LastDate() As Date
        Get

            Return m_dtLastDate

        End Get
        Set(ByVal Value As Date)

            m_dtLastDate = Value

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
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class