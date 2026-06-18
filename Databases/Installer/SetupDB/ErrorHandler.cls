VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CErrorHandler"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Error handler
' Shared:   MultiUse, Yes (RESTRICTED)
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Enumerations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Enum EErrorButtons
    ebOK
    ebRetryCancel
    ebRetryCancelIgnore
    ebRetryCancelIgnoreIgnoreAll
End Enum

Public Enum EErrorResults
    erOK = VBA.VbMsgBoxResult.vbOK
    erRetry = VBA.VbMsgBoxResult.vbRetry
    erCancel = VBA.VbMsgBoxResult.vbCancel
    erIgnore = VBA.VbMsgBoxResult.vbIgnore
    erIgnoreAll
End Enum

Public Enum EErrorNumbers
    knErrAbort = 32767
    knErrInvalidProcedureCall = 5
    knErrOverflow = 6
    knErrOutOfMemory = 7
    knErrSubscriptOutOfRange = 9
    knErrTypeMismatch = 13
    knErrFileNotFound = 53
    knErrDeviceIO = 57
    knErrDeviceUnavailable = 68
    knErrPermissionDenied = 70
    knErrPathFileAccess = 75
    knErrPathNotFound = 76
    knErrObjectVariableNotSet = 91
    knErrInvalidUseOfNull = 94
    knErrInvalidPropertyValue = 380
    knErrInvalidPropertyArrayIndex = 381
    knErrPropertyCannotWriteAtRunTime = 382
    knErrPropertyReadOnly = 383
    knErrPropertyCannotReadAtRunTime = 393
    knErrPropertyWriteOnly = 394
    knErrCannotCreateObject = 429
    knErrRDOFirst = 40000
    knErrRDOLast = 40999
End Enum

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private Declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' This constant would normally be in the MMain module.
' It has been moved here to facilitate sharing this code
' module with the setupdb project. If you wish to use this
' constant in another module within the iSWErrorHandler project,
' please see Chris Hayter first.
Private Const ksRegPathProject = ksRegPathSwift & "\ErrorHandler"

Private Const ksDefaultTitle = "Swift"

Private m_sAppTitle As String
Private m_sAppVersion As String
Private m_bLogToWindowsEventLog As Boolean
Private m_sLogFile As String
Private m_sDiagnosticsProgram As String
Private m_sSystemInfoProgram As String
Private m_sStandardMessage As String
Private m_bShowMoreInfo As Boolean
Private m_bShowSystemInfo As Boolean
Private m_bDiagnostics As Boolean
Private m_sEmailToName As String
Private m_sEmailToAddress As String
Private m_sEmailCcName As String
Private m_sEmailCcAddress As String
Private m_bUnattendedExecution As Boolean
Private m_bUseBothErrorParams As Boolean
Private m_sLastErrorsText As String

' Windows API functions.
Private Declare Sub Sleep Lib "kernel32.dll" (ByVal dwMilliseconds As Long)

Public Property Get AppTitle() As String
    AppTitle = m_sAppTitle
End Property

Public Property Let AppTitle(ByVal sAppTitle As String)
    m_sAppTitle = sAppTitle
End Property

Public Property Get AppVersion() As String
    AppVersion = m_sAppVersion
End Property

Public Property Let AppVersion(ByVal sAppVersion As String)
    m_sAppVersion = sAppVersion
End Property

Public Property Get LogToWindowsEventLog() As Boolean
    LogToWindowsEventLog = m_bLogToWindowsEventLog
End Property

Public Property Let LogToWindowsEventLog(ByVal bLogToWindowsEventLog As Boolean)
    ' If the event log is not available on this O/S, then
    ' don't allow the flag to be set. This allows the caller
    ' to set a "fallback log" using the LogFile property.
    m_bLogToWindowsEventLog = bLogToWindowsEventLog And MNTEventLog.LoggingAvailable()
End Property

Public Property Get LogFile() As String
    LogFile = m_sLogFile
End Property

Public Property Let LogFile(ByVal sLogFile As String)
    ' For backward compatibility *ONLY*: if the filename is
    ' "{nteventlog}", then set the log property instead,
    ' otherwise, update this property as normal. If the event
    ' log is not available on this O/S, then the net effect
    ' will be to leave both properties unchanged. That means
    ' it is now impossible to log to a physical file called
    ' "{nteventlog}."
    If sLogFile = "{nteventlog}" Then
        LogToWindowsEventLog = True
    Else
        m_sLogFile = sLogFile
    End If
End Property

Public Property Get StandardMessage() As String
    StandardMessage = m_sStandardMessage
End Property

Public Property Let StandardMessage(ByVal sStandardMessage As String)
    m_sStandardMessage = sStandardMessage
End Property

Public Property Get ShowMoreInfo() As Boolean
    ShowMoreInfo = m_bShowMoreInfo
End Property

Public Property Let ShowMoreInfo(ByVal bShowMoreInfo As Boolean)
    m_bShowMoreInfo = bShowMoreInfo
End Property

Public Property Get ShowSystemInfo() As Boolean
    ShowSystemInfo = m_bShowSystemInfo
End Property

Public Property Let ShowSystemInfo(ByVal bShowSystemInfo As Boolean)
    m_bShowSystemInfo = bShowSystemInfo
End Property

Public Property Get Diagnostics() As Boolean
    Diagnostics = m_bDiagnostics
End Property

Public Property Let Diagnostics(ByVal bDiagnostics As Boolean)
    m_bDiagnostics = bDiagnostics
End Property

Public Property Get EmailToName() As String
    EmailToName = m_sEmailToName
End Property

Public Property Let EmailToName(ByVal sEmailToName As String)
    m_sEmailToName = sEmailToName
End Property

Public Property Get EmailToAddress() As String
    EmailToAddress = m_sEmailToAddress
End Property

Public Property Let EmailToAddress(ByVal sEmailToAddress As String)
    m_sEmailToAddress = sEmailToAddress
End Property

Public Property Get EmailCcName() As String
    EmailCcName = m_sEmailCcName
End Property

Public Property Let EmailCcName(ByVal sEmailCcName As String)
    m_sEmailCcName = sEmailCcName
End Property

Public Property Get EmailCcAddress() As String
    EmailCcAddress = m_sEmailCcAddress
End Property

Public Property Let EmailCcAddress(ByVal sEmailCcAddress As String)
    m_sEmailCcAddress = sEmailCcAddress
End Property

Public Property Get UnattendedExecution() As Boolean
    UnattendedExecution = m_bUnattendedExecution
End Property

Public Property Let UnattendedExecution(ByVal bUnattendedExecution As Boolean)
    m_bUnattendedExecution = bUnattendedExecution
End Property

Public Property Get UseBothErrorParams() As Boolean
    UseBothErrorParams = m_bUseBothErrorParams
End Property

Public Property Let UseBothErrorParams(ByVal bUseBothErrorParams As Boolean)
    m_bUseBothErrorParams = bUseBothErrorParams
End Property

Public Property Get LastErrorsText() As String
    LastErrorsText = m_sLastErrorsText
End Property

Public Function Dialog(ByVal sMessage As String, _
    Optional ByVal oVBError As VBA.ErrObject = Nothing, _
    Optional ByVal sDBErrors As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal nButtons As EErrorButtons = ebRetryCancel, _
    Optional ByVal bCritical As Boolean = False, _
    Optional ByVal sLogInfo As String = "") As EErrorResults
Attribute Dialog.VB_Description = "Displays an error message dialog and returns the user response. This version is now obsolete - DialogV2 should be used instead."

    If oVBError Is Nothing Then
        Dialog = DialogV2(sMessage, _
            sDBErrors:=sDBErrors, _
            sTitle:=sTitle, _
            nButtons:=nButtons, _
            bCritical:=bCritical, _
            sLogInfo:=sLogInfo)
    Else
        Dialog = DialogV2(sMessage, _
            nErrNumber:=oVBError.Number, _
            sErrSource:=oVBError.Source, _
            sErrDescription:=oVBError.Description, _
            sDBErrors:=sDBErrors, _
            sTitle:=sTitle, _
            nButtons:=nButtons, _
            bCritical:=bCritical, _
            sLogInfo:=sLogInfo)
    End If

End Function

Public Function DialogV2(ByVal sMessage As String, _
    Optional ByVal nErrNumber As Long = 0, _
    Optional ByVal sErrSource As String = "", _
    Optional ByVal sErrDescription As String = "", _
    Optional ByVal sDBErrors As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal nButtons As EErrorButtons = ebRetryCancel, _
    Optional ByVal bCritical As Boolean = False, _
    Optional ByVal sLogInfo As String = "") As EErrorResults
Attribute DialogV2.VB_Description = "Displays an error message dialog and returns the user response. This is now the recommended method to use."

    Dim frm As FError
    Dim sErrorLines As String
    Dim sUserInfo As String
    Dim sSystemInfoProgram As String

    ' If the error is the special abort error, then return
    ' "Cancel" immediately without doing anything.
    If nErrNumber = knErrAbort Then
        DialogV2 = erCancel
        Exit Function
    End If

    ' Make sure we always have valid titles.
    If sTitle = "" Then
        sTitle = m_sAppTitle
    End If

    sErrorLines = GetErrorLines(nErrNumber, sErrSource, sErrDescription, sDBErrors)
    sUserInfo = GetUserInfoLine()
    sLogInfo = GetLogInfoLine(sLogInfo)

    ' Save error text for the calling program's benefit.
    m_sLastErrorsText = sUserInfo & _
        GetMessageLine(sMessage) & _
        sErrorLines & _
        sLogInfo

    #If SiriusDatabaseInstaller Then
        ' If we are in batch mode, then don't display any UI.
        If MMain.BatchMode Then
            ' Return the most appropriate option.
            Select Case nButtons
            Case ebRetryCancelIgnoreIgnoreAll, ebRetryCancelIgnore
                If MMain.AllowUserToIgnore Then
                    DialogV2 = erIgnore
                ElseIf MMain.AllowUserToCancelDialog Then
                    DialogV2 = erCancel
                Else
                    ' Inconsistent options - we have no choice but to ignore.
                    DialogV2 = erIgnore
                End If
            Case ebRetryCancel
                DialogV2 = erCancel
            Case Else ' ebOK
                DialogV2 = erOK
            End Select

            ' Write to log.
            WriteToStorage GetTitleLine(sTitle) & m_sLastErrorsText

            Exit Function
        End If
    #End If

    ' Determine if the system information button should be shown on the dialog.
    If m_bShowSystemInfo Then
        sSystemInfoProgram = m_sSystemInfoProgram
    Else
        ' Set the location of the system info program to blank string therefore
        ' the program cannot be found and the system info button on the dialog
        ' will be made invisible.
        sSystemInfoProgram = ""
    End If

    ' Display dialog (if the user interface is not disabled).
    ' Wrap it in an error handler for complete robustness.
    On Error GoTo EH_DisplayDialog

    If Not m_bUnattendedExecution Then
        Set frm = New FError
        DialogV2 = frm.Dialog(sMessage, _
            sErrorLines, _
            bCritical, _
            nButtons, _
            sTitle, _
            sLogInfo, _
            m_sAppTitle, _
            m_sAppVersion, _
            m_sDiagnosticsProgram, _
            sSystemInfoProgram, _
            m_sStandardMessage, _
            m_bShowMoreInfo, _
            m_bDiagnostics, _
            m_sEmailToName, _
            m_sEmailToAddress, _
            m_sEmailCcName, _
            m_sEmailCcAddress)
        Set frm = Nothing
    End If
    On Error GoTo 0

    ' Write to log.
    WriteToStorage GetTitleLine(sTitle) & m_sLastErrorsText

    Exit Function

EH_DisplayDialog:
    SimpleInternalError "A program error has occurred, but the error information cannot be displayed."
    Exit Function

End Function

Public Sub Report(ByVal sMessage As String, _
    Optional ByVal oVBError As VBA.ErrObject = Nothing, _
    Optional ByVal sDBErrors As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")
Attribute Report.VB_Description = "Logs the error message to disk without displaying any user interface. This version is now obsolete - ReportV2 should be used instead."

    If oVBError Is Nothing Then
        ReportV2 sMessage, _
            sDBErrors:=sDBErrors, _
            sTitle:=sTitle, _
            sLogInfo:=sLogInfo
    Else
        ReportV2 sMessage, _
            nErrNumber:=oVBError.Number, _
            sErrSource:=oVBError.Source, _
            sErrDescription:=oVBError.Description, _
            sDBErrors:=sDBErrors, _
            sTitle:=sTitle, _
            sLogInfo:=sLogInfo
    End If

End Sub

Public Sub ReportV2(ByVal sMessage As String, _
    Optional ByVal nErrNumber As Long = 0, _
    Optional ByVal sErrSource As String = "", _
    Optional ByVal sErrDescription As String = "", _
    Optional ByVal sDBErrors As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")
Attribute ReportV2.VB_Description = "Logs the error message to disk without displaying any user interface. This is now the recommended method to use."

    ' If the error is the special abort error, then return
    ' "Cancel" immediately without doing anything.
    If nErrNumber = knErrAbort Then
        Exit Sub
    End If

    ' Make sure we always have valid titles.
    If sTitle = "" Then
        sTitle = m_sAppTitle
    End If

    ' Save error text for the calling program's benefit.
    m_sLastErrorsText = GetUserInfoLine() & _
        GetMessageLine(sMessage) & _
        GetErrorLines(nErrNumber, sErrSource, sErrDescription, sDBErrors) & _
        GetLogInfoLine(sLogInfo)

    ' Write to log.
    WriteToStorage GetTitleLine(sTitle) & m_sLastErrorsText

End Sub

' Store current properties in the registry so that
' the next error object will pick them up.
Public Sub StoreSettings()

    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "StandardMessage", m_sStandardMessage
    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "Diagnostics", m_bDiagnostics
    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailToName", m_sEmailToName
    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailToAddress", m_sEmailToAddress
    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailCcName", m_sEmailCcName
    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailCcAddress", m_sEmailCcAddress

End Sub

Private Sub Class_Initialize()

    ' We must not let failure to set the class data ruin object creation.
    On Error Resume Next

    m_sAppTitle = ksDefaultTitle
    m_sAppVersion = ""
    m_bUnattendedExecution = False
    m_bUseBothErrorParams = True
    m_sLastErrorsText = ""

    m_sLogFile = FileErrorLog()
    m_sDiagnosticsProgram = FileDiagnostics()
    m_sSystemInfoProgram = FileSystemInfo()

    m_sStandardMessage = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "StandardMessage", "")
    m_bShowMoreInfo = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "ShowMoreInfo", False)
    m_bShowSystemInfo = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "ShowSystemInfo", True)
    m_bDiagnostics = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "Diagnostics", False)
    m_sEmailToName = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailToName", "")
    m_sEmailToAddress = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailToAddress", "")
    m_sEmailCcName = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailCcName", "")
    m_sEmailCcAddress = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "EmailCcAddress", "")

End Sub

Private Function GetTitleLine(ByVal sTitle As String) As String

    Dim sText As String

    sText = IIf(m_sAppTitle <> "", m_sAppTitle, "(Unknown Application)") & " " & _
        IIf(m_sAppVersion <> "", m_sAppVersion, "(Unknown Version)") & " " & _
        Now()
    If sTitle <> "" And sTitle <> m_sAppTitle Then
        sText = sText & " [" & sTitle & "]"
    End If
    GetTitleLine = sText & vbCrLf

End Function

Private Function GetUserInfoLine() As String

    Dim sComputerName As String
    Dim sUserName As String
    Dim sText As String

    sComputerName = GetWindowsComputerName()
    If sComputerName = "" Then
        sComputerName = "(Unknown)"
    End If

    sUserName = GetWindowsUserName()
    If sUserName = "" Then
        sUserName = "(Unknown)"
    End If

    sText = "Computer Name: " & sComputerName & "  User Name: " & sUserName

    GetUserInfoLine = sText & vbCrLf

End Function

Private Function GetMessageLine(ByVal sMessage As String) As String

    GetMessageLine = RemoveChars(sMessage, vbCrLf) & vbCrLf

End Function

Private Function GetErrorLines( _
    ByVal nErrNumber As Long, _
    ByVal sErrSource As String, _
    ByVal sErrDescription As String, _
    ByVal sDBErrors As String) As String

    Dim sVBError As String

    sVBError = ""
    If nErrNumber <> 0 Then
        sVBError = Trim$(RemoveChars(sErrDescription, vbCrLf)) & " (" & nErrNumber & ") (" & sErrSource & ")" & vbCrLf
    End If

    If sDBErrors <> "" Then
        sDBErrors = RemoveChars(sDBErrors, vbCrLf) & vbCrLf
    End If

    If m_bUseBothErrorParams Then
        GetErrorLines = sDBErrors & sVBError
    ElseIf sDBErrors = "" Then
        GetErrorLines = sVBError
    Else
        GetErrorLines = sDBErrors
    End If

End Function

Private Function GetLogInfoLine(ByVal sLogInfo As String) As String

    If sLogInfo <> "" Then
        GetLogInfoLine = RemoveChars(sLogInfo, vbCrLf) & vbCrLf
    Else
        GetLogInfoLine = ""
    End If

End Function

Private Sub WriteToStorage(ByVal sText As String)

    If m_bLogToWindowsEventLog Then
        ' Send to the application event log.
        WriteToEventLog sText
    ElseIf m_sLogFile <> "" Then
        ' Send to the specified file.
        WriteToFile sText
    End If

End Sub

Private Sub WriteToFile(ByVal sText As String)

    Const knMaxTries = 10
    Const knPauseBetweenTries = 100 ' milliseconds

    Dim nTry As Integer
    Dim hLog As Integer

    On Error GoTo EH_Handler

    nTry = 0
    hLog = FreeFile()
    Open m_sLogFile For Append Access Write Lock Write As hLog

    Print #hLog, sText

EX_NormalExit:
    On Error Resume Next
    If hLog <> 0 Then
        Close hLog
    End If
    Exit Sub

EH_Handler:
    If Err.Number = knErrPermissionDenied Then
        If nTry <= knMaxTries Then
            Sleep knPauseBetweenTries
            nTry = nTry + 1
            Resume
        End If
    End If
    SimpleInternalError "Cannot write this error to the log file. " & _
        "Please check that the folder exists and the file has read/write permissions.", _
        sLogInfo:="File: " & m_sLogFile
    Resume EX_NormalExit

End Sub

Private Sub WriteToEventLog(ByVal sText As String)

    Const knErrEventLogFull = &H5DE + vbObjectError
    Dim sMessage As String

    On Error GoTo EH_Handler
    sMessage = "Cannot write this error to the Windows Application Event Log."

    ' Write the event with a fixed severity level.
    MNTEventLog.LogEvent knLogError, sText

    Exit Sub

EH_Handler:
    If Err.Number = knErrEventLogFull Then
        sMessage = sMessage & " The event log is full. No more errors will be logged until more space is made available."
    End If
    SimpleInternalError sMessage
    Exit Sub

End Sub

Private Sub SimpleInternalError(ByVal sMessage As String, _
    Optional ByVal sLogInfo As String = "")

    ' Only display a message box if the user interface is not disabled.
    If Not m_bUnattendedExecution Then
        sMessage = sMessage & vbCrLf & vbCrLf & _
            Trim$(RemoveChars(Err.Description, vbCrLf)) & " (" & Err.Number & ") (" & Err.Source & ")"

        If sLogInfo <> "" Then
            sMessage = sMessage & vbCrLf & vbCrLf & sLogInfo
        End If

        MsgBox sMessage, vbExclamation, m_sAppTitle
    End If

End Sub

Private Function FileErrorLog() As String

    Dim sFolder As String

    sFolder = FolderLogs()
    If sFolder <> "" Then
        FileErrorLog = AddSlash(sFolder) & "Errors.log"
    Else
        FileErrorLog = ""
    End If

End Function

Private Function FileDiagnostics() As String

    Dim sFolder As String

    sFolder = FolderPrograms()
    If sFolder <> "" Then
        FileDiagnostics = AddSlash(sFolder) & "Diagnost.exe"
    Else
        FileDiagnostics = ""
    End If

End Function

Private Function FileSystemInfo() As String

    Dim sProgram As String
    Dim bFileFound As Boolean

    sProgram = RegRead(HKEY_LOCAL_MACHINE, _
        "SOFTWARE\Microsoft\Shared Tools\MSInfo", _
        "Path", "")
    If sProgram <> "" Then
        ' Found file.
        bFileFound = True
    Else
        ' If not found, look somewhere else.
        sProgram = RegRead(HKEY_LOCAL_MACHINE, _
            "SOFTWARE\Microsoft\Shared Tools Location", _
            "MSINFO", "")
        If sProgram <> "" Then
            ' This only contains the path, so add the filename.
            sProgram = AddSlash(sProgram) & "MSINFO32.EXE"
            bFileFound = True
        Else
            ' If not found, then give up.
            bFileFound = False
        End If
    End If

    If bFileFound Then
        FileSystemInfo = sProgram
    Else
        FileSystemInfo = ""
    End If

End Function
