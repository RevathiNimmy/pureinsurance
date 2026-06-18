Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Page_NET.Page")>
Public NotInheritable Class Page
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Page
    '
    ' Date: 20/01/1998
    '
    ' Description: Describes the Page attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Page"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_sPageName As New StringsHelper.FixedLengthString(255) '15 SOB210699 Please note the lenght of a file name can be 254 char
    Private m_lDocNum As Integer
    Private m_lPageNum As Integer
    Private m_sPageType As New StringsHelper.FixedLengthString(255) '3 SOB210699 The lenght of a file extension can be 254 char
    Private m_lPageSize As Integer
    Private m_dtCreateDate As Date
    Private m_lVolumeID As Integer


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


    Public Property PageName() As String
        Get

            Return m_sPageName.Value

        End Get
        Set(ByVal Value As String)

            m_sPageName.Value = Value

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

    Public Property PageNum() As Integer
        Get

            Return m_lPageNum

        End Get
        Set(ByVal Value As Integer)

            m_lPageNum = Value

        End Set
    End Property

    Public Property PageType() As String
        Get

            Return m_sPageType.Value

        End Get
        Set(ByVal Value As String)

            m_sPageType.Value = Value

        End Set
    End Property
    Public Property PageSize() As Integer
        Get

            Return m_lPageSize

        End Get
        Set(ByVal Value As Integer)

            m_lPageSize = Value

        End Set
    End Property


    Public Property CreateDate() As Date
        Get

            Return m_dtCreateDate

        End Get
        Set(ByVal Value As Date)

            m_dtCreateDate = Value

        End Set
    End Property

    Public Property VolumeID() As Integer
        Get

            Return m_lVolumeID

        End Get
        Set(ByVal Value As Integer)

            m_lVolumeID = Value

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