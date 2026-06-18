Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
Friend NotInheritable Class Document
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Document
    '
    ' Date: 20/01/1998
    '
    ' Description: Describes the Document attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Document"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lDocNum As Integer
    Private m_lFolderNum As Integer
    Private m_sDocName As New FixedLengthString(50)
    Private m_sExCode As New FixedLengthString(20)
    Private m_sDocType As New FixedLengthString(1)
    Private m_iAccessLevel As Integer
    Private m_sPassword As New FixedLengthString(12)
    Private m_sZipped As New FixedLengthString(1)
    Private m_dtCreateDate As Date
    Private m_lLink As Integer


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

    Public Property FolderNum() As Integer
        Get

            Return m_lFolderNum

        End Get
        Set(ByVal Value As Integer)

            m_lFolderNum = Value

        End Set
    End Property

    Public Property DocName() As String
        Get

            Return m_sDocName.Value

        End Get
        Set(ByVal Value As String)

            m_sDocName.Value = Value

        End Set
    End Property

    Public Property ExCode() As String
        Get

            Return m_sExCode.Value

        End Get
        Set(ByVal Value As String)

            m_sExCode.Value = Value

        End Set
    End Property


    Public Property DocType() As String
        Get

            Return m_sDocType.Value

        End Get
        Set(ByVal Value As String)

            m_sDocType.Value = Value

        End Set
    End Property

    Public Property Zipped() As String
        Get

            Return m_sZipped.Value

        End Get
        Set(ByVal Value As String)

            m_sZipped.Value = Value

        End Set
    End Property


    Public Property AccessLevel() As Integer
        Get

            Return m_iAccessLevel

        End Get
        Set(ByVal Value As Integer)

            m_iAccessLevel = Value

        End Set
    End Property

    Public Property Password() As String
        Get

            Return m_sPassword.Value

        End Get
        Set(ByVal Value As String)

            m_sPassword.Value = Value

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

    Public Property Link() As Integer
        Get

            Return m_lLink

        End Get
        Set(ByVal Value As Integer)

            m_lLink = Value

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
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    Public Sub New()
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
        'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class