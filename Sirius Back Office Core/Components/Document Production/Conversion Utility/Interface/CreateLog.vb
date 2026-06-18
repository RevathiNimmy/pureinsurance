Option Strict Off
Option Explicit On

Imports SharedFiles

<System.Runtime.InteropServices.ProgId("CreateLog_NET.CreateLog")> Public NotInheritable Class CreateLog

    Private Const ACClass As String = "CreateLog"

    Private m_sFileName As String
    Private m_sDirectory As String
    Private m_lErrorNumber As Integer
    Private m_sErrorDesc As String
    Private m_iFileNum As Short

    'Defaults
    Private Const m_sDEFAULT_FILE As String = "PMLogFile.log"
    Private Const m_sDEFAULT_DIRECTORY As String = "c:\temp\PMLog\"
    Private Const m_sDEFAULT_ARCHIVE As String = "c:\temp\PMLog\Archive\"
    Private m_sArchiveDirectory As String
    Private Const m_lMAX_FILE_SIZE As Integer = 2000000


    Public Property ArchiveDirectory() As String
        Get
            ArchiveDirectory = m_sArchiveDirectory
        End Get
        Set(ByVal Value As String)
            m_sArchiveDirectory = Value
        End Set
    End Property


    Public ReadOnly Property ErrorDesc() As String
        Get
            ErrorDesc = m_sErrorDesc
        End Get
    End Property


    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ErrorNumber = m_lErrorNumber
        End Get
    End Property


    Public Property FileName() As String
        Get
            FileName = Trim(m_sFileName)
        End Get
        Set(ByVal Value As String)
            m_sFileName = Value
        End Set
    End Property

    Public Property Directory() As String
        Get
            Directory = Trim(m_sDirectory)
        End Get
        Set(ByVal Value As String)
            m_sDirectory = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: WriteLog
    '
    ' Description: Opens log file and writes out log message.
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function WriteLog(ByRef sMsg As String, Optional ByRef bError As Boolean = False) As Integer
        Dim sDateTime As String

        Try

            WriteLog = gPMConstants.PMEReturnCode.PMTrue

            OpenLog()

            sDateTime = VB6.Format(Now, "hh:mm:ss dd/mm/yyyy")
            If (bError = True) Then
                sMsg = "!!!!!! ERROR - " & sMsg
            End If

            WriteLine(m_iFileNum, sDateTime, sMsg)

            FileClose(m_iFileNum)

            Exit Function

        Catch ex As Exception

            WriteLog = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteLog", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenLog
    '
    ' Description:
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OpenLog() As Integer
        Dim sFile As String
        Dim lFileSize As Integer


        OpenLog = gPMConstants.PMEReturnCode.PMTrue

        'Check whether directory and file name have been specified. If not, use
        'defaults.
        If (Trim(m_sDirectory) = "") Then
            sFile = m_sDEFAULT_DIRECTORY
            m_sDirectory = m_sDEFAULT_DIRECTORY
        Else
            sFile = m_sDirectory
        End If

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If (Dir(sFile) = "") Then
            MkDir(sFile)
        End If

        If (Trim(m_sFileName) = "") Then
            sFile = sFile & m_sDEFAULT_FILE
            m_sFileName = m_sDEFAULT_FILE
        Else
            sFile = sFile & m_sFileName
        End If

        'If file already exists, check it's size. If it is greater than max. allowed
        'then archive it and start new file. This prevents us ending up with some
        'ridiculous huge file that takes half an hour to open for viewing.
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If (Dir(sFile) <> "") Then
            If (FileLen(sFile) > m_lMAX_FILE_SIZE) Then
                ArchiveFile(sFile)
            End If
        End If
        m_iFileNum = FreeFile()

        FileOpen(m_iFileNum, sFile, OpenMode.Append)

        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name: ArchiveFile
    '
    ' Description: Archives log file as unique time-stamped name.
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function ArchiveFile(ByRef sFile As String) As Integer
        Dim sArchiveFile As String


        ArchiveFile = gPMConstants.PMEReturnCode.PMTrue

        If CBool(Trim(CStr(m_sArchiveDirectory = ""))) Then
            sArchiveFile = m_sDEFAULT_ARCHIVE
        Else
            sArchiveFile = m_sArchiveDirectory
        End If

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If (Dir(sArchiveFile) = "") Then
            MkDir(sArchiveFile)
        End If

        'Give archive file unique name that indicates when it was archived.
        sArchiveFile = sArchiveFile & VB6.Format(Now, "yyyymmddhhmmss") & ".log"

        FileCopy(sFile, sArchiveFile)

        Kill(sFile)

        Exit Function

    End Function
End Class
