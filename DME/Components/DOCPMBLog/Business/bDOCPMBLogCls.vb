Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Log_NET.Log")> _
Public NotInheritable Class Log
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 3/12/97
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a DocMan.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Log"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceID As Integer

    Private Const LMSG As Integer = 1
    Private Const LERR As Integer = 2
    Private Const LLOG As Integer = 3
    Private Const LWRN As Integer = 4
    Private Const LDBG As Integer = 5

    Private m_fhLogFile As Integer
    Private m_sLogFileName As String = ""


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '
            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property
    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    '***VarDataEnd***

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
    Public Sub CloseLogFile(ByRef sRootName As String)

        Try

            If FileSystem.LOF(m_fhLogFile) = 0 Then
                FileSystem.FileClose(m_fhLogFile)
                If KillFile(sRootName & "logs\" & m_sLogFileName) = gPMConstants.PMEReturnCode.PMFalse Then
                End If
            Else
                FileSystem.FileClose(m_fhLogFile)
            End If

        Catch



            Exit Sub
        End Try


    End Sub

    Public Sub DOCLogMessage(ByRef ilogType As Integer, ByRef sUsername As String, ByRef sMessage() As String)

        Dim sMessageType As String = ""

        Try

            Select Case (ilogType)
                Case LMSG
                    sMessageType = "MSG"
                Case LERR
                    sMessageType = "ERR"
                Case LLOG
                    sMessageType = "LOG"
                Case LWRN
                    sMessageType = "WRN"
                Case LDBG
                    sMessageType = "DBG"
            End Select

            FileSystem.PrintLine(m_fhLogFile, StringsHelper.Format(DateTime.Now, "hh:mm:ss am/pm") & "  " & sMessageType & " - " & sUsername & ", " & sMessage(1))

            For iCntr As Integer = 2 To sMessage.GetUpperBound(0)
                If sMessage(iCntr).Trim() <> "" Then
                    FileSystem.PrintLine(m_fhLogFile, New String(" "c, sUsername.Length + 21) & sMessage(iCntr))
                End If
            Next iCntr

        Catch
        End Try



        Exit Sub

    End Sub

    Public Function OpenLogFile(ByRef sRootName As String) As Integer

        Dim result As Integer = 0
        Dim sLogName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Contruct log file name
            m_sLogFileName = DateTime.Now.ToString("ddMMyy") & ".log"

            m_fhLogFile = FileSystem.FreeFile()

            sLogName = FileSystem.Dir(sRootName & "logs\" & m_sLogFileName, FileAttribute.Normal)

            If sLogName <> "" Then
                FileSystem.FileOpen(m_fhLogFile, sRootName & "logs\" & m_sLogFileName, OpenMode.Append)
            Else
                FileSystem.FileOpen(m_fhLogFile, sRootName & "logs\" & m_sLogFileName, OpenMode.Output)
            End If

            Return result

        Catch



            If Information.Err().Number = 55 Then
                ' File is already open
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        End Try
    End Function

    Public Function Initialise(ByRef sUsername As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername = sUsername

            ' Frig Log Level
            g_iLogLevel = 4


            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Function KillFile(ByRef sFile As String) As Integer



        File.Delete(sFile)

        Return gPMConstants.PMEReturnCode.PMTrue


    End Function
End Class

