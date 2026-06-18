Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("History_NET.History")>
Public NotInheritable Class History
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: History
    '
    ' Date: 11/02/1998
    '
    ' Description: Describes the History attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "History"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lHistoryID As Integer
    Private m_iTask As Integer
    Private m_sCabinetcode As New StringsHelper.FixedLengthString(20)
    Private m_sCabinetname As New StringsHelper.FixedLengthString(30)
    Private m_sDrawercode As New StringsHelper.FixedLengthString(20)
    Private m_sDrawername As New StringsHelper.FixedLengthString(30)
    Private m_sFoldercode As New StringsHelper.FixedLengthString(20)
    Private m_sFoldername As New StringsHelper.FixedLengthString(70)
    Private m_sDocref As New StringsHelper.FixedLengthString(9)
    Private m_sRequestDate As New StringsHelper.FixedLengthString(8)
    Private m_sRequestTime As New StringsHelper.FixedLengthString(6)
    Private m_sEventtype As New StringsHelper.FixedLengthString(1)
    Private m_sDescription As New StringsHelper.FixedLengthString(30)
    Private m_sVolume As New StringsHelper.FixedLengthString(20)
    Private m_sPagefile As New StringsHelper.FixedLengthString(10)
    Private m_sDoctype As New StringsHelper.FixedLengthString(3)
    Private m_sFiller As New StringsHelper.FixedLengthString(3)
    Private m_sHderror As New StringsHelper.FixedLengthString(1)
    Private m_dtCreateDate As Date
    Private m_sProcessed As New StringsHelper.FixedLengthString(1)


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
    Public Property HistoryID() As Integer
        Get

            Return m_lHistoryID

        End Get
        Set(ByVal Value As Integer)

            m_lHistoryID = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public Property Cabinetcode() As String
        Get

            Return m_sCabinetcode.Value

        End Get
        Set(ByVal Value As String)

            m_sCabinetcode.Value = Value

        End Set
    End Property

    Public Property Cabinetname() As String
        Get

            Return m_sCabinetname.Value

        End Get
        Set(ByVal Value As String)

            m_sCabinetname.Value = Value

        End Set
    End Property

    Public Property Drawercode() As String
        Get

            Return m_sDrawercode.Value

        End Get
        Set(ByVal Value As String)

            m_sDrawercode.Value = Value

        End Set
    End Property

    Public Property Drawername() As String
        Get

            Return m_sDrawername.Value

        End Get
        Set(ByVal Value As String)

            m_sDrawername.Value = Value

        End Set
    End Property

    Public Property Foldercode() As String
        Get

            Return m_sFoldercode.Value

        End Get
        Set(ByVal Value As String)

            m_sFoldercode.Value = Value

        End Set
    End Property

    Public Property Foldername() As String
        Get

            Return m_sFoldername.Value

        End Get
        Set(ByVal Value As String)

            m_sFoldername.Value = Value

        End Set
    End Property

    Public Property Docref() As String
        Get

            Return m_sDocref.Value

        End Get
        Set(ByVal Value As String)

            m_sDocref.Value = Value

        End Set
    End Property

    Public Property RequestDate() As String
        Get

            Return m_sRequestDate.Value

        End Get
        Set(ByVal Value As String)

            m_sRequestDate.Value = Value

        End Set
    End Property

    Public Property RequestTime() As String
        Get

            Return m_sRequestTime.Value

        End Get
        Set(ByVal Value As String)

            m_sRequestTime.Value = Value

        End Set
    End Property

    Public Property Eventtype() As String
        Get

            Return m_sEventtype.Value

        End Get
        Set(ByVal Value As String)

            m_sEventtype.Value = Value

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

    Public Property Volume() As String
        Get

            Return m_sVolume.Value

        End Get
        Set(ByVal Value As String)

            m_sVolume.Value = Value

        End Set
    End Property

    Public Property Pagefile() As String
        Get

            Return m_sPagefile.Value

        End Get
        Set(ByVal Value As String)

            m_sPagefile.Value = Value

        End Set
    End Property

    Public Property Doctype() As String
        Get

            Return m_sDoctype.Value

        End Get
        Set(ByVal Value As String)

            m_sDoctype.Value = Value

        End Set
    End Property

    Public Property Filler() As String
        Get

            Return m_sFiller.Value

        End Get
        Set(ByVal Value As String)

            m_sFiller.Value = Value

        End Set
    End Property

    Public Property Hderror() As String
        Get

            Return m_sHderror.Value

        End Get
        Set(ByVal Value As String)

            m_sHderror.Value = Value

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

    Public Property Processed() As String
        Get

            Return m_sProcessed.Value

        End Get
        Set(ByVal Value As String)

            m_sProcessed.Value = Value

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