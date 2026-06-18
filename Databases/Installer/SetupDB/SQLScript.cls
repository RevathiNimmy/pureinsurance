VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CSQLScript"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Execute an SPP file
' Shared:   MultiUse, Yes (RESTRICTED)
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Events
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Event ProcessFile( _
    ByVal sFile As String)

Public Event ProcessStatement( _
    ByVal nStatement As Long, _
    ByVal sSQL As String, _
    ByRef r_bProcess As Boolean)

Public Event PromptForValue( _
    ByVal sPromptText As String, _
    ByVal vDefault As Variant, _
    ByRef r_vValue As Variant)

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private Declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Script logging states.
Private Enum EScriptStatuses
    knProcessing
    knCompleted
    knAbortedWithError
    knCancelledByUser
End Enum
#Const knProcessing = 0
#Const knCompleted = 0
#Const knAbortedWithError = 0
#Const knCancelledByUser = 0

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Properties which can be changed by the calling program
' and/or the script before or during execution
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Enable or disable the Ignore/Ignore All buttons.
' ACCESS: COM Property
' DEFAULT: True
Private m_bAllowUserToIgnore As Boolean

' Automatically drop views or procedures before creating them.
' ACCESS: COM Property & #option
' DEFAULT: True
Private m_bAutoDrop As Boolean

' Cancel execution at the next safe point?
' ACCESS: COM Property & #option
' DEFAULT: False
Private m_bCancelled As Boolean

' Can we use "--" as a statement separator?
' ACCESS: COM Property & #option
' DEFAULT: True
Private m_bCommentAsSeparator As Boolean

' Treat unrecognised metacommands as SQL or flag them as syntax errors.
' ACCESS: COM Property & #option
' DEFAULT: False
Private m_bErrorOnBadMetaCommands As Boolean

' Raise an error when a wildcard search returns no files?
' ACCESS: COM Property & #option
' DEFAULT: False
Private m_bErrorOnFileNotFound As Boolean

' If set, the events are raised to this object instead of the normal COM event system.
' ACCESS: COM Property
' DEFAULT: Nothing
Private m_oEvents As ISQLScriptEvents

' Ignore all errors?
' ACCESS: COM Property & #option
' DEFAULT: False
Private m_bIgnoreErrors As Boolean

' Log the exact text sent to ADO (for debugging purposes)?
' ACCESS: COM Property & #option
' DEFAULT: False
Private m_bLogActions As Boolean

' Location of log files (for debugging purposes).
' ACCESS: COM Property & #option
' DEFAULT: ""
Private m_sLogActionsTo As String

' Automatically strip all comments from the text?
' ACCESS: COM Property & #option
' DEFAULT: True
Private m_bRemoveComments As Boolean

' Name of the Swift user, for script logging.
' ACCESS: COM Property
' DEFAULT: ""
Private m_sUserName As String

' Data version that the calling code is upgrading from, for script logging.
' ACCESS: COM Property
' DEFAULT: ""
Private m_sVersionFrom As String

' Data version that the calling code is upgrading to, for script logging.
' ACCESS: COM Property
' DEFAULT: ""
Private m_sVersionTo As String

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Internal control variables
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' The database connection.
Private m_db As CDatabase

' The list of public variables. At the moment, this module treats
' all variables created in SPP files as "public", that is,
' accessible from all #include'd SPP files during the application's
' run-time. This may change in future, but this module's
' interface will not be affected.
Private m_varsPublic As CVariables

' The global "current statement" number. This is an invisible
' counter that is incremented once per SQL statement, once per
' CSV line, and not at all for a hash command. The only purpose of
' this is to implement a "restart from here" facility. The code
' to handle restarting must be supplied within the callback
' procedures. This module's only responsibility is to keep an
' accurate counter.
Private m_nStatement As Long

' Has the object been initialised?
Private m_bInitialised As Boolean

' ID of the script logging entry currently open.
Private m_lLoggingID As Long

' Keep track of the file name and line number we are currently
' processing, for use by error reporting and macro expansion.
Private m_sCurrentPath As String
Private m_sCurrentFile As String
Private m_nCurrentLine As Long

' Has the user pressed "Ignore All" in DoExecuteStatement?
Private m_bIgnoreAllInExecuteStatement As Boolean

' The current file number for the SQL action log files.
Private m_nLogSequence As Long

' Cached database commands.
Private m_cmdProcedureExists As CCommandHelper
Private m_parProcedureExistsName As ADODB.Parameter

Private m_bIntLogScriptStart As Boolean
Private m_cmdIntLogScriptStart As CCommandHelper

Private m_bIntLogScriptFinish As Boolean
Private m_cmdIntLogScriptFinish As CCommandHelper

Private m_bIntLogScriptError As Boolean
Private m_cmdIntLogScriptError As CCommandHelper

Private m_bSetupDBRunOnce As Boolean
Private m_cmdSetupDBRunOnceInsert As CCommandHelper
Private m_parSetupDBRunOnceInsertCode As ADODB.Parameter
Private m_parSetupDBRunOnceInsertDescription As ADODB.Parameter
Private m_rsSetupDBRunOnce As ADODB.Recordset

Private m_bSetupDBScriptLog As Boolean
Private m_cmdSetupDBScriptLogInsert As CCommandHelper
Private m_parSetupDBScriptLogInsertVersionFrom As ADODB.Parameter
Private m_parSetupDBScriptLogInsertVersionTo As ADODB.Parameter
Private m_parSetupDBScriptLogInsertFileName As ADODB.Parameter
Private m_parSetupDBScriptLogInsertID As ADODB.Parameter

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Error declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Error declarations.
Private Const ksErrSource = "iSWSQLScript.CSQLScript"
Private Const knErrNotInitialised = vbObjectError + 1
Private Const ksErrNotInitialised = "Library is not initialised."
Private Const knErrBadDataVersion = vbObjectError + 2
'rivate Const ksErrBadDataVersion text always variable.
Private Const knErrHashIfTypeMismatch = 13
Private Const ksErrHashIfTypeMismatch = "Type mismatch - boolean or integer required in #if."
Private Const knErrHashSetSyntax = vbObjectError + 3
Private Const ksErrHashSetSyntax = "Syntax error - unrecognised data type in #set."
Private Const knErrHashReadDataSyntax = vbObjectError + 4
Private Const ksErrHashReadDataSyntax = "Syntax error - unrecognised data type in #readdata."
Private Const knErrBadOpeningComment = vbObjectError + 5
Private Const ksErrBadOpeningComment = "Syntax error - opening comment without closing comment."
Private Const knErrBadClosingComment = vbObjectError + 6
Private Const ksErrBadClosingComment = "Syntax error - closing comment without opening comment."
Private Const knErrRunOnceCodeMissing = vbObjectError + 7
Private Const ksErrRunOnceCodeMissing = "Syntax error - missing code in #runoncebegin."
Private Const knErrRunOnceCodeTooLong = vbObjectError + 8
Private Const ksErrRunOnceCodeTooLong = "Syntax error - code too long in #runoncebegin."
Private Const knErrRunOnceDescriptionMissing = vbObjectError + 9
Private Const ksErrRunOnceDescriptionMissing = "Syntax error - missing description in #runoncebegin."
Private Const knErrRunOnceDescriptionTooLong = vbObjectError + 10
Private Const ksErrRunOnceDescriptionTooLong = "Syntax error - description too long in #runoncebegin."
Private Const knAssertBadStatementType = vbObjectError + 11
Private Const ksAssertBadStatementType = "Assertion failure - bad statement type would result in infinite loop."
Private Const knErrRunOnceSupportRequired = vbObjectError + 12
Private Const ksErrRunOnceSupportRequired = "#runonce stored procedures not found in the database."

Public Property Get AllowUserToIgnore() As Boolean
    AllowUserToIgnore = m_bAllowUserToIgnore
End Property

Public Property Let AllowUserToIgnore(ByVal bAllowUserToIgnore As Boolean)
    m_bAllowUserToIgnore = bAllowUserToIgnore
End Property

Public Property Get AutoDrop() As Boolean
    AutoDrop = m_bAutoDrop
End Property

Public Property Let AutoDrop(ByVal bAutoDrop As Boolean)
    m_bAutoDrop = bAutoDrop
End Property

Public Property Get Cancelled() As Boolean
    Cancelled = m_bCancelled
End Property

Public Property Let Cancelled(ByVal bCancelled As Boolean)
    m_bCancelled = bCancelled
End Property

Public Property Get CommentAsSeparator() As Boolean
    CommentAsSeparator = m_bCommentAsSeparator
End Property

Public Property Let CommentAsSeparator(ByVal bCommentAsSeparator As Boolean)
    m_bCommentAsSeparator = bCommentAsSeparator
End Property

Public Property Get ErrorOnBadMetaCommands() As Boolean
    ErrorOnBadMetaCommands = m_bErrorOnBadMetaCommands
End Property

Public Property Let ErrorOnBadMetaCommands(ByVal bErrorOnBadMetaCommands As Boolean)
    m_bErrorOnBadMetaCommands = bErrorOnBadMetaCommands
End Property

Public Property Get ErrorOnFileNotFound() As Boolean
    ErrorOnFileNotFound = m_bErrorOnFileNotFound
End Property

Public Property Let ErrorOnFileNotFound(ByVal bErrorOnFileNotFound As Boolean)
    m_bErrorOnFileNotFound = bErrorOnFileNotFound
End Property

Public Property Get Events() As ISQLScriptEvents
    Set Events = m_oEvents
End Property

Public Property Set Events(ByVal oEvents As ISQLScriptEvents)
    Set m_oEvents = oEvents
End Property

Public Property Get IgnoreErrors() As Boolean
    IgnoreErrors = m_bIgnoreErrors
End Property

Public Property Let IgnoreErrors(ByVal bIgnoreErrors As Boolean)
    m_bIgnoreErrors = bIgnoreErrors
End Property

Public Property Get LogActions() As Boolean
    LogActions = m_bLogActions
End Property

Public Property Let LogActions(ByVal bLogActions As Boolean)
    m_bLogActions = bLogActions
End Property

Public Property Get LogActionsTo() As String
    LogActionsTo = m_sLogActionsTo
End Property

Public Property Let LogActionsTo(ByVal sLogActionsTo As String)
    m_sLogActionsTo = sLogActionsTo
End Property

Public Property Get RemoveComments() As Boolean
    RemoveComments = m_bRemoveComments
End Property

Public Property Let RemoveComments(ByVal bRemoveComments As Boolean)
    m_bRemoveComments = bRemoveComments
End Property

Public Property Get UserName() As String
    UserName = m_sUserName
End Property

Public Property Let UserName(ByVal sUserName As String)
    m_sUserName = sUserName
End Property

Public Property Get VersionFrom() As String
    VersionFrom = m_sVersionFrom
End Property

Public Property Let VersionFrom(ByVal sVersionFrom As String)
    m_sVersionFrom = sVersionFrom
End Property

Public Property Get VersionTo() As String
    VersionTo = m_sVersionTo
End Property

Public Property Let VersionTo(ByVal sVersionTo As String)
    m_sVersionTo = sVersionTo
End Property

' Call once at start of application to set up the global variables.
' This allows you to run one SPP file from the user interface to
' define some variables, then another SPP file that uses them.
Public Sub Initialise(ByVal db As CDatabase)

    ' Store the database connection.
    Set m_db = db

    ' Set up pre-defined variables.
    Set m_varsPublic = New CVariables
    With m_varsPublic
        ' Set the variable list to throw an error if a variable
        ' is used before it's defined, but the act of assigning
        ' to it is sufficient to define it.
        .RequireDeclaration = False
        .RequireInitialisation = True
        ' Type of database we are connected to.
        Select Case m_db.DBMSType
        Case knDBMSMicrosoftSQLServer
            .Add "MicrosoftSQLServer", True
            .Add "SybaseAdaptiveServer", False
            .Add "SybaseSQLAnywhere", False
            .Add "TransactSQL", True
            .Add "WatcomSQL", False
        Case knDBMSSybaseAdaptiveServer
            .Add "MicrosoftSQLServer", False
            .Add "SybaseAdaptiveServer", True
            .Add "SybaseSQLAnywhere", False
            .Add "TransactSQL", True
            .Add "WatcomSQL", False
        Case knDBMSSybaseSQLAnywhere
            .Add "MicrosoftSQLServer", False
            .Add "SybaseAdaptiveServer", False
            .Add "SybaseSQLAnywhere", True
            .Add "TransactSQL", False
            .Add "WatcomSQL", True
        End Select
        ' The name is used to switch between our database and the
        ' "master" database in SASE and MSS.
        .Add "DatabaseName", m_db.Connection.DefaultDatabase
        ' Hard disk locations.
        .Add "WindowsPath", FolderWindows()
        .Add "SystemPath", FolderWindowsSystem()
        ' Product-specific hard disk locations.
        Select Case m_db.DataAccessMethod
        Case knDAMSwift6Version2, knDAMSwift6Version1, knDAMSwift6Original, _
            knDAMSwift5Version1, knDAMSwift5Original
            .Add "BasePath", FolderBase()
            .Add "ProgramPath", FolderPrograms()
            .Add "HelpPath", FolderHelp()
            .Add "ReportPath", FolderReports()
            .Add "LogPath", FolderLogs()
            .Add "SQLPath", FolderSQL()
            .Add "UserDocumentPath", FolderUserDocuments()
        End Select
    End With

    ' Detect the existence of the script logging procedures.
    DetectLoggingProcedures True

    ' Library is initialised.
    m_bInitialised = True

End Sub

' Call once at end of application to clean up.
Public Sub Terminate()

    On Error Resume Next

    m_bCancelled = False

    DisposeRecordset m_rsSetupDBRunOnce

    ' Destroy variables.
    Set m_oEvents = Nothing
    Set m_varsPublic = Nothing
    Set m_cmdProcedureExists = Nothing
    Set m_parProcedureExistsName = Nothing
    Set m_cmdIntLogScriptStart = Nothing
    Set m_cmdIntLogScriptFinish = Nothing
    Set m_cmdIntLogScriptError = Nothing
    Set m_cmdSetupDBRunOnceInsert = Nothing
    Set m_parSetupDBRunOnceInsertCode = Nothing
    Set m_parSetupDBRunOnceInsertDescription = Nothing
    Set m_rsSetupDBRunOnce = Nothing
    Set m_cmdSetupDBScriptLogInsert = Nothing
    Set m_parSetupDBScriptLogInsertVersionFrom = Nothing
    Set m_parSetupDBScriptLogInsertVersionTo = Nothing
    Set m_parSetupDBScriptLogInsertFileName = Nothing
    Set m_parSetupDBScriptLogInsertID = Nothing
    Set m_db = Nothing

    ' Library is not initialised.
    m_bInitialised = False

End Sub

' The entry point to execute an SPP file.
Public Sub Run(ByVal sFile As String)

    Dim oFind As CFileSearch

    ' Safety check.
    If Not m_bInitialised Then
        Err.Raise knErrNotInitialised, ksErrSource, ksErrNotInitialised
    End If

    ' Reset default values for control variables.
    m_bCancelled = False
    m_nStatement = 1
    m_bIgnoreAllInExecuteStatement = False
    m_nLogSequence = 0

    On Error GoTo EH_Run

    ' Start logging.
    ScriptLoggingStart sFile

    ' Treat the file as a file-matching pattern. If it is not,
    ' then the search will only find one file and the behaviour
    ' will be as intended.
    Set oFind = New CFileSearch
    oFind.ErrorOnFileNotFound = m_bErrorOnFileNotFound
    oFind.FindFirst sFile
    Do While oFind.Searching
        If oFind.IsFile Then
            ' Process file.
            ProcessFile oFind.FullyQualifiedName
        End If
        oFind.FindNext
    Loop
    Set oFind = Nothing

    ' Stop logging.
    If m_bCancelled Then
        ScriptLoggingStop knCancelledByUser
    Else
        ScriptLoggingStop knCompleted
    End If

    Exit Sub

EH_Run:
    Select Case ScriptErrorDialog("Cannot find or open file(s) """ & sFile & """.")
    Case erRetry
        Resume
    Case Else
        Set oFind = Nothing
        ' Abort logging.
        ScriptLoggingStop knAbortedWithError
        ' Throw error to calling application.
        Err.Raise knErrAbort
    End Select

End Sub

' Test for the existence of the script logging procedures.
' This is automatically done whenever the class is initialised, however,
' some programs may want to call it explicitly during execution.
Public Sub DetectLoggingProcedures(ByVal bAlwaysReTest As Boolean)

    If bAlwaysReTest Then
        m_bIntLogScriptStart = ProcedureExists("IntLogScriptStart")
        m_bIntLogScriptFinish = ProcedureExists("IntLogScriptFinish")
        m_bIntLogScriptError = ProcedureExists("IntLogScriptError")
        m_bSetupDBRunOnce = ProcedureExists("spu_SetupDB_Run_Once_Insert") And ProcedureExists("spu_SetupDB_Run_Once_Select")
        m_bSetupDBScriptLog = ProcedureExists("spu_SetupDB_Script_Log_Insert")
    Else
        If Not m_bIntLogScriptStart Then
            m_bIntLogScriptStart = ProcedureExists("IntLogScriptStart")
        End If
        If Not m_bIntLogScriptFinish Then
            m_bIntLogScriptFinish = ProcedureExists("IntLogScriptFinish")
        End If
        If Not m_bIntLogScriptError Then
            m_bIntLogScriptError = ProcedureExists("IntLogScriptError")
        End If
        If Not m_bSetupDBRunOnce Then
            m_bSetupDBRunOnce = ProcedureExists("spu_SetupDB_Run_Once_Insert") And ProcedureExists("spu_SetupDB_Run_Once_Select")
        End If
        If Not m_bSetupDBScriptLog Then
            m_bSetupDBScriptLog = ProcedureExists("spu_SetupDB_Script_Log_Insert")
        End If
    End If

End Sub

' RECURSIVE sub to process an SPP file. The calling sub needs to
' initialise any status displays, etc. If the user aborts from an
' error, this sub throws it all the way out, so make sure that you
' surround this sub with an error handler.
Private Sub ProcessFile(ByVal sFile As String)

    Dim hFile As Integer
    Dim sSQL As CStringBuilder
    Dim sLine As String
    Dim sAnalysedLine As String
    Dim sMetaKeyword As String
    Dim sMetaParameters As String
    Dim lScriptLogID As Long
    Dim oIfStack As CIfEndifStack

    If Len(sFile) = 0 Then Exit Sub
    If m_bCancelled Then Exit Sub
    If m_oEvents Is Nothing Then
        RaiseEvent ProcessFile(sFile)
    Else
        m_oEvents.ProcessFile sFile
    End If
    If m_bCancelled Then Exit Sub

    On Error GoTo EH_ProcessFileOpen

    ' Open the file.
    hFile = FreeFile()
    Open sFile For Input Access Read Lock Write As hFile

    ' Initialise information about the current file.
    m_sCurrentPath = ParsePathFile(sFile, "")
    m_sCurrentFile = sFile
    m_nCurrentLine = 0
    Set oIfStack = New CIfEndifStack

    On Error GoTo EH_ProcessFileAudit

    ' Create audit entry for the current file.
    lScriptLogID = SetupDBScriptLogInsert(sFile)

EX_ProcessFileAudit:

    On Error GoTo EH_ProcessFileParse

    Set sSQL = New CStringBuilder
    Do Until EOF(hFile)
        m_nCurrentLine = m_nCurrentLine + 1
        Line Input #hFile, sLine
        sAnalysedLine = Trim$(Replace$(sLine, vbTab, " "))
        If IsMetaCommand(sAnalysedLine, sMetaKeyword, sMetaParameters) Then
            ' Interpret the metacommand.
            Select Case LCase$(sMetaKeyword)
            Case "if"
                oIfStack.DoIf EvaluateTest(sMetaParameters)
            Case "elseif"
                oIfStack.DoElseIf EvaluateTest(sMetaParameters)
            Case "else"
                oIfStack.DoElse
            Case "endif"
                oIfStack.DoEndIf
            Case "runoncefile"
                ' This is a quick alternative to enclosing the whole file in a #runoncebegin block.
                ' If the test returns false, then jump to the end of the file.
                DoRunOnceBegin oIfStack, sMetaParameters
                If Not oIfStack.Execute Then
                    Exit Do
                End If
            Case "runoncebegin"
                DoRunOnceBegin oIfStack, sMetaParameters
            Case "runonceend"
                DoRunOnceEnd oIfStack
            Case "debug"
                If oIfStack.Execute Then
                    ScriptErrorReport sMetaParameters, sTitle:="SPP File Debugging"
                End If
            Case "error"
                If oIfStack.Execute Then
                    ScriptErrorDialog sMetaParameters, sTitle:="SPP File Error", nButtons:=ebOK
                End If
            Case "import"
                If oIfStack.Execute Then
                    DoImportCSVFile sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "include"
                If oIfStack.Execute Then
                    DoIncludeSPPFile sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "prompt"
                If oIfStack.Execute Then
                    DoPromptForVariable sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "run"
                If oIfStack.Execute Then
                    DoExecuteProgram sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "set"
                If oIfStack.Execute Then
                    DoAssignToVariable sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "option"
                If oIfStack.Execute Then
                    DoSetOption sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "readdata"
                If oIfStack.Execute Then
                    DoSelectIntoVariable sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case "checkdv"
                If oIfStack.Execute Then
                    DoCheckDataVersion sMetaParameters
                    If m_bCancelled Then GoTo EX_ProcessFileParse
                End If
            Case Else
                ' Unrecognised command.
                If m_bErrorOnBadMetaCommands Then
                    ' Treat as a comment, but record it in the error log.
                    If oIfStack.Execute Then
                        ScriptErrorReport "Unknown command: " & sAnalysedLine, sTitle:="SPP File Error"
                    End If
                Else
                    ' Treat as part of the SQL.
                    If oIfStack.Execute Then
                        sSQL.Append sLine
                        sSQL.Append vbCrLf
                    End If
                End If
            End Select
        ElseIf IsIgnorableLine(sAnalysedLine) Then
            ' Ignore this line.
        ElseIf IsBatchTerminator(sAnalysedLine) Then
            ' Execute the last statement.
            If oIfStack.Execute Then
                DoExecuteStatement sSQL
                sSQL.Dispose
                If m_bCancelled Then GoTo EX_ProcessFileParse
            End If
        Else
            ' Add this line to the current statement.
            If oIfStack.Execute Then
                sSQL.Append sLine
                sSQL.Append vbCrLf
            End If
        End If
    Loop
    ' If there is still a pending statement, then execute it.
    If sSQL.length > 0 Then
        If oIfStack.Execute Then
            DoExecuteStatement sSQL
            sSQL.Dispose
            If m_bCancelled Then GoTo EX_ProcessFileParse
        End If
    End If
    ' If there are any open #if or #runonce blocks, then explicitly close them now.
    ' This code is required to support #runonce properly.
    Do While oIfStack.NestLevel > 0
        Select Case oIfStack.StatementType
        Case knBlockIf
            oIfStack.DoEndIf
        Case knBlockRunOnce
            DoRunOnceEnd oIfStack
        Case Else
            ' This should never occur, but if it does, then the code would go
            ' into an infinite loop, so make it bomb out instead for safety.
            Err.Raise knAssertBadStatementType, ksErrSource, ksAssertBadStatementType
        End Select
    Loop

EX_ProcessFileParse:
    CloseAnyway hFile
    Exit Sub

EH_ProcessFileOpen:
    Select Case ScriptErrorDialog("Cannot open file """ & sFile & """.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

EH_ProcessFileAudit:
    Select Case ScriptErrorDialog("Cannot audit the processing of file """ & sFile & """.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_ProcessFileAudit
    Case Else
        Err.Raise knErrAbort
    End Select

EH_ProcessFileParse:
    ' We don't expect this error handler to be called
    ' except via a lower-level abort error, so we only
    ' provide a generic error message just in case.
    Select Case ScriptErrorDialog("Error occurred while parsing file """ & sFile & """.", _
        sLogInfo:="Meta: #" & Trim$(sMetaKeyword & " " & sMetaParameters) & vbCrLf & _
            "SQL: " & sSQL & vbCrLf)
    Case erRetry
        Resume
    Case Else
        CloseAnyway hFile
        Err.Raise knErrAbort
    End Select

End Sub

Private Function IsBatchTerminator(ByVal sLine As String) As Boolean

    If LCase$(sLine) = "go" Then
        ' The official batch terminator.
        IsBatchTerminator = True
    ElseIf sLine = "--" Then
        ' Also recognise a blank comment if the appropriate backward compatibility switch is on.
        IsBatchTerminator = m_bCommentAsSeparator
    End If

End Function

Private Function IsIgnorableLine(ByVal sLine As String) As Boolean

    If Len(sLine) = 0 Then
        ' Ignore blank lines if the appropriate backward compatibility switch is on.
        IsIgnorableLine = m_bRemoveComments
    ElseIf Left$(sLine, 2) = "--" Then
        ' Ignore comment lines if the appropriate backward compatibility switch is on.
    End If

End Function

Private Function IsMetaCommand(ByVal sLine As String, ByRef o_sKeyword As String, ByRef o_sParameters As String) As Boolean

    If Left$(sLine, 1) = "#" Then
        ' The official metacommand prefix.
        IsMetaCommand = True
        o_sKeyword = ParseSep(Mid$(sLine, 2), o_sParameters, " ")
    ElseIf Left$(sLine, 4) = "---#" Then
        ' The alternative metacommand prefix for compatibility with Query Analyzer.
        IsMetaCommand = True
        o_sKeyword = ParseSep(Mid$(sLine, 5), o_sParameters, " ")
    End If

End Function

' Assign a value of the specified data type to a variable. You can
' use this to overwrite the value of any existing variable, even the
' built-in ones if you really want to.
Private Sub DoAssignToVariable(ByVal sMetaParameters As String)

    Dim sMessage As String
    Dim sError As String
    Dim sVariable As String
    Dim sType As String
    Dim sValue As String
    Dim vValue As Variant

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #set <variable>, <type>, <value>
    sVariable = ParseCSV(sMetaParameters, sMetaParameters)
    sType = ParseCSV(sMetaParameters, sMetaParameters)
    sValue = MacroExpand(ParseCSV(sMetaParameters, sMetaParameters))

    On Error GoTo EH_DoAssignToVariable

    ' Translate the value into the correct data type for storage.
    Select Case LCase$(sType)
    Case "boolean"
        vValue = ToBoolean(sValue)
    Case "byte", "tinyint"
        vValue = ToByte(sValue)
    Case "integer", "smallint"
        vValue = ToInteger(sValue)
    Case "long", "longint"
        vValue = ToLong(sValue)
    Case "double", "single"
        vValue = ToDouble(sValue)
    Case "currency", "money"
        vValue = ToCurrency(sValue)
    Case "string", "char", "varchar"
        vValue = sValue
    Case "date", "time", "datetime"
        vValue = ToDateTime(sValue)
    Case "empty", "null"
        vValue = Empty
    Case Else
        Err.Raise knErrHashSetSyntax, ksErrSource, ksErrHashSetSyntax
    End Select

    ' Assign to the variable.
    m_varsPublic(sVariable) = vValue

EX_DoAssignToVariable:
    Exit Sub

EH_DoAssignToVariable:
    sMessage = "Cannot assign to the variable '" & sVariable & "'."
    Select Case ScriptErrorDialog(sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_DoAssignToVariable
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Execute an SQL SELECT statement and assign the result to a variable
' of the specified data type.
Private Sub DoSelectIntoVariable(ByVal sMetaParameters As String)

    Dim bProcess As Boolean
    Dim sMessage As String
    Dim sError As String
    Dim sVariable As String
    Dim sType As String
    Dim sSQL As String
    Dim vValue As Variant
    Dim rs As ADODB.Recordset

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #readdata <variable>, <type>, <SQL>
    sVariable = ParseCSV(sMetaParameters, sMetaParameters)
    sType = ParseCSV(sMetaParameters, sMetaParameters)
    sSQL = MacroExpand(ParseCSV(sMetaParameters, sMetaParameters))

    ' "Restart from here" check. This is NOT a statement, but is
    ' only executed if the currently-active statement is also being
    ' executed.
    bProcess = True
    If m_oEvents Is Nothing Then
        RaiseEvent ProcessStatement(m_nStatement, "", bProcess)
    Else
        m_oEvents.ProcessStatement m_nStatement, "", bProcess
    End If
    If m_bCancelled Then
        GoTo EX_DoSelectIntoVariable
    ElseIf Not bProcess Then
        GoTo EX_DoSelectIntoVariable
    End If

    On Error GoTo EH_DoSelectIntoVariable

    ' Execute the query and retrieve the result.
    Set rs = m_db.OpenRecordset(sSQL)
    If Not rs.EOF Then
        vValue = rs(0)
    Else
        vValue = Null
    End If
    rs.Close
    If IsNull(vValue) Then
        vValue = Empty
    End If

    ' Translate the value into the correct data type for storage.
    Select Case LCase$(sType)
    Case "boolean"
        vValue = ToBoolean(vValue)
    Case "byte", "tinyint"
        vValue = ToByte(vValue)
    Case "integer", "smallint"
        vValue = ToInteger(vValue)
    Case "long", "longint"
        vValue = ToLong(vValue)
    Case "double", "single"
        vValue = ToDouble(vValue)
    Case "currency", "money"
        vValue = ToCurrency(vValue)
    Case "string", "char", "varchar"
        vValue = ToString(vValue)
    Case "date", "time", "datetime"
        vValue = ToDateTime(vValue)
    Case "empty", "null"
        vValue = Empty
    Case Else
        Err.Raise knErrHashReadDataSyntax, ksErrSource, ksErrHashReadDataSyntax
    End Select

    ' Assign to the variable.
    m_varsPublic(sVariable) = vValue

EX_DoSelectIntoVariable:
    Exit Sub

EH_DoSelectIntoVariable:
    sMessage = "Cannot assign to the variable '" & sVariable & "'."
    Select Case ScriptErrorDialog(sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_DoSelectIntoVariable
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' TEMPORARY ADDITION FOR SWIFT 6.
' Checks the data version and generates a fatal error if it is not correct.
Private Sub DoCheckDataVersion(ByVal sMetaParameters As String)

    Dim bProcess As Boolean
    Dim sVersionKeyword As String
    Dim sVersionNumber As String
    Dim nVersion As Long

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #checkdv <version keyword>, <version number>
    sVersionKeyword = MacroExpand(ParseCSV(sMetaParameters, sMetaParameters))
    sVersionNumber = MacroExpand(ParseCSV(sMetaParameters, sMetaParameters))

    ' "Restart from here" check. This is NOT a statement, but is
    ' only executed if the currently-active statement is also being
    ' executed.
    bProcess = True
    If m_oEvents Is Nothing Then
        RaiseEvent ProcessStatement(m_nStatement, "", bProcess)
    Else
        m_oEvents.ProcessStatement m_nStatement, "", bProcess
    End If
    If m_bCancelled Then
        GoTo EX_DoCheckDataVersion
    ElseIf Not bProcess Then
        GoTo EX_DoCheckDataVersion
    End If

    On Error GoTo EH_DoCheckDataVersion

    nVersion = m_db.SettingRead("Database", sVersionKeyword, 0)

    On Error GoTo 0

    If nVersion <> ToLongFixed(sVersionNumber) Then
        Err.Raise knErrBadDataVersion, ksErrSource, "Database version " & sVersionNumber & " expected, cannot continue."
    End If

EX_DoCheckDataVersion:
    Exit Sub

EH_DoCheckDataVersion:
    ' This error handler CANNOT ALLOW the user to ignore.
    Select Case ScriptErrorDialog("Cannot check the database version.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Evaluates a boolean expression and returns the result. In this
' version, all it does is assume the expression is a variable name.
' Future versions may handle expressions if it becomes necessary.
Private Function EvaluateTest(ByVal sExpression As String) As Boolean

    Dim vValue As Variant

    EvaluateTest = False

    vValue = m_varsPublic(sExpression)
    Select Case VarType(vValue)
    Case vbEmpty, vbNull
        ' Empty - return False.
    Case vbBoolean
        ' Boolean - return the value directly.
        EvaluateTest = vValue
    Case vbByte, vbInteger, vbLong
        ' Integer - use the standard "nonzero = true" rule.
        EvaluateTest = (vValue <> 0)
    Case Else
        ' String or Date - return False and raise an error.
        Err.Raise knErrHashIfTypeMismatch, ksErrSource, ksErrHashIfTypeMismatch
    End Select

End Function

' Prompt the user for a value and assign the result to a variable.
' You can use this to overwrite the value of any existing variable,
' even the built-in ones if you really want to.
Private Sub DoPromptForVariable(ByVal sMetaParameters As String)

    Dim sVariable As String
    Dim sPromptText As String
    Dim vValue As Variant

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #prompt <variable>, <prompt>
    sVariable = ParseCSV(sMetaParameters, sMetaParameters)
    sPromptText = MacroExpand(ParseCSV(sMetaParameters, sMetaParameters))

    ' Get a response from the user and assign to the variable.
    ' How this is done is entirely up to the calling program.
    vValue = m_varsPublic(sVariable)
    If m_oEvents Is Nothing Then
        RaiseEvent PromptForValue(sPromptText, m_varsPublic(sVariable), vValue)
    Else
        m_oEvents.PromptForValue sPromptText, m_varsPublic(sVariable), vValue
    End If
    If m_bCancelled Then
        GoTo EX_DoPromptForVariable
    End If

    ' Has the calling program changed the parameter?
    If m_varsPublic(sVariable) <> vValue Then
        m_varsPublic(sVariable) = vValue
    End If

EX_DoPromptForVariable:
    ' This label is only defined for consistency of code with
    ' other procedures.

End Sub

' Execute a command line.
Private Sub DoExecuteProgram(ByVal sMetaParameters As String)

    Dim sMessage As String
    Dim sError As String
    Dim sCommand As String

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #run <command>
    sCommand = MacroExpand(sMetaParameters)

    On Error GoTo EH_DoExecuteProgram

    StartExecutable sCommand, nWindowStyle:=vbMinimizedNoFocus, bWaitUntilFinished:=True

EX_DoExecuteProgram:
    Exit Sub

EH_DoExecuteProgram:
    sMessage = "Cannot execute the command '" & sCommand & "'."
    Select Case ScriptErrorDialog(sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_DoExecuteProgram
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Clean up and execute an SQL statement. Apply macro expansion
' to it so that we can embed variable contents in the SQL.
' Update the statement counter.
Private Sub DoExecuteStatement(ByVal sSQL As CStringBuilder)

    Const knCharsToShowInError = 150

    Dim bProcess As Boolean
    Dim sMessage As CStringBuilder
    Dim sError As String
    Dim sKeyword As String
    Dim sRemainder As String
    Dim sObjectName As String
    Dim sHiddenSQL As CStringBuilder

    If sSQL.length = 0 Then Exit Sub

    ' "Restart from here" check. This is a real statement that
    ' updates the restart info.
    bProcess = True
    If m_oEvents Is Nothing Then
        RaiseEvent ProcessStatement(m_nStatement, sSQL, bProcess)
    Else
        m_oEvents.ProcessStatement m_nStatement, sSQL, bProcess
    End If
    If m_bCancelled Then
        GoTo EX_DoExecuteStatement
    ElseIf Not bProcess Then
        GoTo EX_DoExecuteStatement
    End If

    ' Activate the error handler now because we expect errors
    ' to be raised during string processing.
    On Error GoTo EH_DoExecuteStatement

    ' Evaluate all macros in the SQL string.
    MacroExpandSB sSQL

    ' Remove all comments and clean up the SQL string.
    CleanUpTextSB sSQL

    ' If it's now blank, we can safely ignore it.
    If sSQL.length = 0 Then Exit Sub

    ' Now we have a genuine SQL statement to execute, detect
    ' whether the command is one that requires the old object
    ' to be dropped first. If it is, then carry out the
    ' appropriate extra actions.
    '
    ' For restarting purposes, we treat this whole bit of code
    ' as executing a single SQL statement, regardless of how
    ' many actually get executed.

    sKeyword = ""
    If m_bAutoDrop Then
        If DetectCommand(sSQL, "create procedure", sRemainder) Then
            sKeyword = "procedure"
        ElseIf DetectCommand(sSQL, "create view", sRemainder) Then
            sKeyword = "view"
        ElseIf DetectCommand(sSQL, "sw_add_procedure", sRemainder) Then
            ' Backward compatibility only.
            sKeyword = "procedure"
        ElseIf DetectCommand(sSQL, "sw_add_view", sRemainder) Then
            ' Backward compatibility only.
            sKeyword = "view"
        End If
    End If

    If Len(sKeyword) <> 0 Then
        sObjectName = ParseSepFirstChar(sRemainder, "", " (")
        ' Execute the drop object statement first, ignoring all
        ' errors.
        On Error Resume Next
        Set sHiddenSQL = New CStringBuilder
        sHiddenSQL.Append "drop "
        sHiddenSQL.Append sKeyword
        sHiddenSQL.Append " "
        sHiddenSQL.Append sObjectName
        LogAction sHiddenSQL
        m_db.Execute sHiddenSQL
        Err.Clear
        Set sHiddenSQL = Nothing
        ' Now execute the create object statement, using the
        ' normal error handler.
        On Error GoTo EH_DoExecuteStatement
        Set sHiddenSQL = New CStringBuilder
        sHiddenSQL.Append "create "
        sHiddenSQL.Append sKeyword
        sHiddenSQL.Append " "
        sHiddenSQL.Append sRemainder
        LogAction sHiddenSQL
        m_db.Execute sHiddenSQL
        Set sHiddenSQL = Nothing
    Else
        ' Execute statement without further alteration.
        On Error GoTo EH_DoExecuteStatement
        LogAction sSQL
        m_db.Execute sSQL
    End If

EX_DoExecuteStatement:
    ' This bit must ALWAYS be executed regardless of what actually
    ' happened in this procedure. The only exception is aborting
    ' with a fatal error.
    UpdateRestartInfo
    Exit Sub

EH_DoExecuteStatement:
    Set sMessage = New CStringBuilder
    sMessage.Append "Cannot execute the SQL statement:"
    sMessage.Append vbCrLf
    If sSQL.length < knCharsToShowInError Then
        sMessage.Append sSQL
    Else
        sMessage.Append sSQL.Left(knCharsToShowInError) & "..."
    End If
    ' In this particular error handler ONLY, allow us to turn off
    ' error handling if we so wish. The error still gets written
    ' to the error log so that we have a permanent record. You can
    ' alter this flag with the #option command in the SPP file.
    If m_bIgnoreErrors Then
        ScriptErrorReport sMessage
        Resume EX_DoExecuteStatement
    ElseIf m_bIgnoreAllInExecuteStatement Then
        ScriptErrorReport sMessage
        Resume EX_DoExecuteStatement
    Else
        Select Case ScriptErrorDialog(sMessage, _
            nButtons:=ebRetryCancelIgnoreIgnoreAll)
        Case erRetry
            Resume
        Case erIgnore
            Resume EX_DoExecuteStatement
        Case erIgnoreAll
            m_bIgnoreAllInExecuteStatement = True
            Resume EX_DoExecuteStatement
        Case Else
            Err.Raise knErrAbort
        End Select
    End If

End Sub

' Detects a particular command at the beginning of an SQL
' statement. Also parses and returns the remainder of the
' statement by reference.
Private Function DetectCommand(ByVal sSQL As CStringBuilder, _
    ByVal sCommand As String, _
    ByRef o_sRemainder As String) As Boolean

    sCommand = LCase$(sCommand) & " "

    If LCase$(sSQL.Left(Len(sCommand))) = sCommand Then
        DetectCommand = True
        o_sRemainder = sSQL.Mid(Len(sCommand) + 1)
    Else
        DetectCommand = False
        o_sRemainder = ""
    End If

End Function

' Import a whole CSV file in a single statement.
Private Sub DoImportCSVFile(ByVal sMetaParameters As String)

    Dim sMessage As String
    Dim sError As String
    Dim bIgnoreOneLine As Boolean
    Dim sTable As String
    Dim sFile As String
    Dim hFile As Integer
    Dim sLine As String
    Dim sSQLPattern As String   ' the non-recurring part of the SQL
    Dim sSQL As CStringBuilder  ' the full SQL to be executed

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #import <table>, <file>
    sTable = MacroExpand(ParseCSV(sMetaParameters, sMetaParameters))
    sFile = AddPath(m_sCurrentPath, MacroExpand(ParseCSV(sMetaParameters, sMetaParameters)))

    On Error GoTo EH_DoImportCSVFile
    bIgnoreOneLine = False

    hFile = FreeFile()
    Open sFile For Input Access Read Lock Write As hFile

    ' Read field names from the first line.
    sSQLPattern = ""
    If Not EOF(hFile) Then
        Line Input #hFile, sLine
        sSQLPattern = "insert into " & sTable & "(" & sLine & ") values("
    End If
    If Len(sSQLPattern) = 0 Then
        GoTo EX_DoImportCSVFile
    End If

    ' Read the data from the rest of the file.
    Set sSQL = New CStringBuilder
    bIgnoreOneLine = True
    Do Until EOF(hFile)
        Line Input #hFile, sLine
        ' Perform macro expansion on the values line first. This
        ' allows us to include variables and functions in the data.
        sLine = MacroExpand(sLine)
        sSQL.Append sSQLPattern
        sSQL.Append sLine
        sSQL.Append ")"
        LogAction sSQL
        m_db.Execute sSQL
        sSQL.Dispose
    Loop
    bIgnoreOneLine = False

EX_DoImportCSVFile:
    CloseAnyway hFile
    Exit Sub

EH_DoImportCSVFile:
    sMessage = "Cannot read records from the data file '" & sFile & "'."
    Select Case ScriptErrorDialog(sMessage, nButtons:=ebRetryCancelIgnore, sLogInfo:="SQL: " & sSQL)
    Case erRetry
        Resume
    Case erIgnore
        If bIgnoreOneLine Then
            Resume Next
        Else
            Resume EX_DoImportCSVFile
        End If
    Case Else
        CloseAnyway hFile
        Err.Raise knErrAbort
    End Select

End Sub

' Include another SQL file at this point in the processing.
' THIS FUNCTION CALLS ProcessFile() RECURSIVELY.
Private Sub DoIncludeSPPFile(ByVal sMetaParameters As String)

    Dim sMessage As String
    Dim sError As String
    Dim oFind As CFileSearch
    Dim sFile As String
    Dim sFullyQualifiedName As String
    Dim sCallingPath As String
    Dim sCallingFile As String
    Dim nCallingLine As Long

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #include <file>
    sFile = AddPath(m_sCurrentPath, MacroExpand(ParseCSV(sMetaParameters, sMetaParameters)))

    ' Save current file processing state before recursion.
    ' This method avoids having to pass state variables
    ' into every function called by the recursive function.
    sCallingPath = m_sCurrentPath
    sCallingFile = m_sCurrentFile
    nCallingLine = m_nCurrentLine

    On Error GoTo EH_DoIncludeSPPFile

    ' Treat the file as a file-matching pattern. If it is not,
    ' then the search will only find one file and the behaviour
    ' will be as intended.
    Set oFind = New CFileSearch
    oFind.ErrorOnFileNotFound = m_bErrorOnFileNotFound
    oFind.FindFirst sFile
    Do While oFind.Searching
        If oFind.IsFile Then
            sFullyQualifiedName = oFind.FullyQualifiedName
            ' Simple check against infinite recursion. Don't bother
            ' with a complete check because that would require explicit
            ' coding, and is really the script writer's fault.
            If sFullyQualifiedName <> m_sCurrentFile Then
                ' Recursive call to original processing function.
                ProcessFile sFullyQualifiedName
            End If
        End If
        oFind.FindNext
    Loop
    Set oFind = Nothing

    ' Restore original file processing state after recursion.
    ' This method avoids having to pass state variables
    ' into every function called by the recursive function.
    m_sCurrentPath = sCallingPath
    m_sCurrentFile = sCallingFile
    m_nCurrentLine = nCallingLine

EX_DoIncludeSPPFile:
    Exit Sub

EH_DoIncludeSPPFile:
    ' Restore original file processing state after recursion.
    ' This method avoids having to pass state variables
    ' into every function called by the recursive function.
    m_sCurrentPath = sCallingPath
    m_sCurrentFile = sCallingFile
    m_nCurrentLine = nCallingLine

    sMessage = "Cannot find or open file(s) """ & sFile & """."
    Select Case ScriptErrorDialog(sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_DoIncludeSPPFile
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Set a general-purpose option that affects SPP file execution.
Private Sub DoSetOption(ByVal sMetaParameters As String)

    Dim sProperty As String
    Dim sValue As String
    Dim sMessage As String
    Dim sError As String

    If Len(sMetaParameters) = 0 Then Exit Sub

    ' Interpret the parameters as follows:
    ' #option <property> = <value>
    sProperty = Trim$(ParseSep(sMetaParameters, sMetaParameters, "="))
    sValue = MacroExpand(Trim$(sMetaParameters))

    On Error GoTo EH_DoSetOption

    Select Case LCase$(sProperty)
    Case "autodrop"
        m_bAutoDrop = ToBoolean(sValue)
    Case "cancelled"
        m_bCancelled = ToBoolean(sValue)
    Case "commentasseparator"
        m_bCommentAsSeparator = ToBoolean(sValue)
    Case "erroronbadmetacommands"
        m_bErrorOnBadMetaCommands = ToBoolean(sValue)
    Case "erroronfilenotfound"
        m_bErrorOnFileNotFound = ToBoolean(sValue)
    Case "ignoreerrors"
        m_bIgnoreErrors = ToBoolean(sValue)
    Case "logactions"
        m_bLogActions = ToBoolean(sValue)
    Case "logactionsto"
        m_sLogActionsTo = sValue
    Case "removecomments"
        m_bRemoveComments = ToBoolean(sValue)
    Case "commandtimeout", "querytimeout"
        m_db.Connection.CommandTimeout = ToLong(sValue)
    Case Else
        ' Unrecognised property. Ignore it, but record it in the error log.
        ScriptErrorReport "Unknown option: " & sProperty, sTitle:="SPP File Error"
    End Select

EX_DoSetOption:
    Exit Sub

EH_DoSetOption:
    sMessage = "Cannot set the option '" & sProperty & "'"
    Select Case ScriptErrorDialog(sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_DoSetOption
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DoRunOnceBegin(ByVal oIfStack As CIfEndifStack, ByVal sMetaParameters As String)

    ' Essential safety check. We cannot provide a "safe" default action if the procs
    ' are missing, so we must abort.
    If Not m_bSetupDBRunOnce Then
        Err.Raise knErrRunOnceSupportRequired, ksErrSource, ksErrRunOnceSupportRequired
    End If

    Dim sMessage As String
    Dim bExecute As Boolean
    Dim sCode As String
    Dim sDescription As String

    ' Interpret the parameters as follows:
    ' #runoncebegin <code>, <description>
    sCode = ParseCSV(sMetaParameters, sMetaParameters)
    sDescription = ParseCSV(sMetaParameters, sMetaParameters)

    On Error GoTo EH_Handler

    ' Validate the parameters.
    If Len(sCode) = 0 Then
        Err.Raise knErrRunOnceCodeMissing, ksErrSource, ksErrRunOnceCodeMissing
    End If
    If Len(sCode) > 256 Then
        Err.Raise knErrRunOnceCodeTooLong, ksErrSource, ksErrRunOnceCodeTooLong
    End If
    If Len(sDescription) > 256 Then
        Err.Raise knErrRunOnceDescriptionTooLong, ksErrSource, ksErrRunOnceDescriptionTooLong
    End If

    ' Execute the contents of the #runonce block only if an entry is not already in the table.
    bExecute = Not SetupDBRunOnceExists(sCode)

    ' Begin the #runonce block.
    oIfStack.DoRunOnceBegin bExecute, sCode, sDescription

EX_NormalExit:
    Exit Sub

EH_Handler:
    ' This error handler CANNOT ALLOW the user to ignore.
    sMessage = "Cannot tell whether the code block '" & sCode & "' has already been run or not."
    Select Case ScriptErrorDialog(sMessage)
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DoRunOnceEnd(ByVal oIfStack As CIfEndifStack)

    ' Essential safety check. We cannot provide a "safe" default action if the procs
    ' are missing, so we must abort.
    If Not m_bSetupDBRunOnce Then
        Err.Raise knErrRunOnceSupportRequired, ksErrSource, ksErrRunOnceSupportRequired
    End If

    Dim sMessage As String
    Dim bExecute As Boolean
    Dim sCode As String
    Dim sDescription As String

    On Error GoTo EH_Handler

    ' End the #runonce block.
    oIfStack.DoRunOnceEnd bExecute, sCode, sDescription

    ' Write an entry to the table only if the #runonce block was executed.
    If bExecute Then
        SetupDBRunOnceInsert sCode, sDescription
    End If

EX_NormalExit:
    Exit Sub

EH_Handler:
    ' This error handler CANNOT ALLOW the user to ignore.
    sMessage = "Cannot mark the code block '" & sCode & "' as having been run."
    Select Case ScriptErrorDialog(sMessage)
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Perform macro expansion on the specified string and return the result. Macros are based
' on the syntax used in VB Setup Wizard and NMAKE.EXE, and the actual values are fetched
' from variables of the same name. This is a wrapper round the real routine to allow us
' to perform macro expansion on small strings in complex expressions.
Private Function MacroExpand(ByVal sText As String) As String

    Dim sNewText As CStringBuilder

    If Len(sText) <> 0 Then
        Set sNewText = New CStringBuilder
        sNewText.Text = sText
        MacroExpandSB sNewText
        MacroExpand = sNewText.Text
    End If

End Function

' The routine that actually does the macro expansion. The text passed in is modified.
Private Sub MacroExpandSB(ByRef r_sText As CStringBuilder)

    Dim sNewText As CStringBuilder
    Dim iLastChunkEnd As Long
    Dim iMacroStart As Long
    Dim iMacroEnd As Long
    Dim sMacro As String
    Dim sMacroKeyword As String
    Dim sMacroParameters As String
    Dim sMacroResult As String
    Dim sURNSeries As String

    If r_sText.length = 0 Then Exit Sub

    Set sNewText = New CStringBuilder
    iLastChunkEnd = 0
    Do
        ' Find the next macro, and look up its value in a variable.
        sMacro = ""
        sMacroResult = ""
        iMacroStart = InStr(iLastChunkEnd + 1, r_sText.Text, "$(")
        If iMacroStart <> 0 Then
            iMacroEnd = InStr(iMacroStart + 2, r_sText.Text, ")")
            If iMacroEnd <> 0 Then
                ' We have found a matching pair of macro delimiters.
                ' Now we must interpret the contents.
                sMacro = r_sText.Mid(iMacroStart + 2, iMacroEnd - iMacroStart - 2)
                sMacroKeyword = ParseSep(sMacro, sMacroParameters, " ")
                Select Case LCase$(sMacroKeyword)
                Case "currpath"
                    ' Hard-coded function.
                    sMacroResult = m_sCurrentPath
                Case "statement"
                    ' Hard-coded function.
                    sMacroResult = ToStringFixed(m_nStatement)
                Case "urn"
                    ' Hard-coded function. Parameters follow after a space character.
                    sURNSeries = ParseCSV(sMacroParameters, "")
                    sMacroResult = ToStringFixed(m_db.URNGetNext(sURNSeries))
                Case Else
                    ' Variable name. If the variable is not defined, accessing it here
                    ' will raise a trappable error to be displayed to the user.
                    sMacroResult = m_varsPublic(sMacroKeyword)
                End Select
                If Len(sMacroResult) <> 0 Then
                    ' Now that we have the macro result, if the next character is a slash,
                    ' treat it like a pathname being added onto a filename and add a slash
                    ' if required.
                    If r_sText.length > iMacroEnd Then
                        If r_sText.Char(iMacroEnd + 1) = "\" Then
                            iMacroEnd = iMacroEnd + 1
                            sMacroResult = AddSlash(sMacroResult)
                        End If
                    End If
                End If
            End If
        End If
        ' If a valid macro was found, add the macro result to the result string and
        ' continue looping. Otherwise, just add the remainder of the string and exit
        ' looping.
        If Len(sMacro) <> 0 Then
            sNewText.Append r_sText.Mid(iLastChunkEnd + 1, iMacroStart - iLastChunkEnd - 1)
            sNewText.Append sMacroResult
            iLastChunkEnd = iMacroEnd
        ElseIf iLastChunkEnd <> 0 Then
            sNewText.Append r_sText.Mid(iLastChunkEnd + 1)
            Exit Do
        Else
            Set sNewText = r_sText
            Exit Do
        End If
    Loop
    Set r_sText = sNewText

End Sub

' Close a file handle without giving an error. Use this to close
' files easily from within an error handler.
Private Sub CloseAnyway(ByVal hFile As Integer)

    If hFile <> 0 Then
        On Error Resume Next
        Close hFile
        On Error GoTo 0
    End If

End Sub

' This code is a separate procedure purely to emphasise that it
' is just for restart tracking and nothing else. The only
' procedures that should call this one are those that are counted
' as a "statement" for restarting purposes.
Private Sub UpdateRestartInfo()

    m_nStatement = m_nStatement + 1

End Sub

Private Sub Class_Initialize()

    ' Library is NOT initialised.
    m_bInitialised = False

    ' Set default values for public properties.
    m_bAllowUserToIgnore = True
    m_bAutoDrop = True
    m_bCancelled = False
    m_bCommentAsSeparator = True
    m_bErrorOnBadMetaCommands = False
    m_bErrorOnFileNotFound = False
    Set m_oEvents = Nothing
    m_bIgnoreErrors = False
    m_bLogActions = False
    m_sLogActionsTo = ""
    m_bRemoveComments = True
    m_sUserName = ""

End Sub

Private Sub Class_Terminate()

    ' In case the user forgot to call it.
    Terminate

End Sub

' This function must be used for all error dialogs in the project.
' It automatically adds the filename & line information and also
' forces the database error handler to be used instead of an
' independent internal one.
Private Function ScriptErrorDialog(ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal nButtons As EErrorButtons = ebRetryCancel, _
    Optional ByVal bCritical As Boolean = False, _
    Optional ByVal sLogInfo As String = "") As EErrorResults

    Dim nErrNumber As Long
    Dim sErrSource As String
    Dim sErrDescription As String
    Dim bIgnored As Boolean

    ' Save the error data first.
    nErrNumber = Err.Number
    sErrSource = Err.Source
    sErrDescription = Err.Description

    ' Override the nButtons value if required.
    If Not m_bAllowUserToIgnore Then
        Select Case nButtons
        Case ebRetryCancelIgnore, ebRetryCancelIgnoreIgnoreAll
            nButtons = ebRetryCancel
        End Select
    End If

    ' Add extra debugging information to sLogInfo.
    If Len(sLogInfo) <> 0 Then
        sLogInfo = sLogInfo & vbCrLf
    End If
    sLogInfo = sLogInfo & _
        "Statement: " & m_nStatement & vbCrLf & _
        "File: " & m_sCurrentFile & vbCrLf & _
        "Line: " & m_nCurrentLine

    ' Call the error handler.
    ScriptErrorDialog = m_db.ErrorHandlerDialog(sMessage, _
        nErrNumber:=nErrNumber, _
        sErrSource:=sErrSource, _
        sErrDescription:=sErrDescription, _
        sTitle:=sTitle, _
        nButtons:=nButtons, _
        bCritical:=bCritical, _
        sLogInfo:=sLogInfo)

    ' Did the user ignore the error?
    Select Case ScriptErrorDialog
    Case erIgnore, erIgnoreAll
        bIgnored = True
    Case Else
        bIgnored = False
    End Select

    ' Also write the error to the log tables.
    ScriptLoggingError sMessage, _
        nErrNumber:=nErrNumber, _
        sErrSource:=sErrSource, _
        sErrDescription:=sErrDescription, _
        sLogInfo:=sLogInfo, _
        bIgnored:=bIgnored

    #If SiriusDatabaseInstaller Then
        ' If the user ignored this error, log that fact to the install log.
        If bIgnored Then
            WriteToLog "ERROR IGNORED: Statement " & m_nStatement & ", Line " & m_nCurrentLine & ", File " & m_sCurrentFile
            SetAppTasksDone knAppErrorIgnored
        End If
    #End If

End Function

' This function must be used for all error reporting in the project.
' It automatically adds the filename & line information and also
' forces the database error handler to be used instead of an
' independent internal one.
Private Sub ScriptErrorReport(ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    Dim nErrNumber As Long
    Dim sErrSource As String
    Dim sErrDescription As String

    ' Save the error data first.
    nErrNumber = Err.Number
    sErrSource = Err.Source
    sErrDescription = Err.Description

    ' Add extra debugging information to sLogInfo.
    If Len(sLogInfo) <> 0 Then
        sLogInfo = sLogInfo & vbCrLf
    End If
    sLogInfo = sLogInfo & _
        "Statement: " & m_nStatement & vbCrLf & _
        "File: " & m_sCurrentFile & vbCrLf & _
        "Line: " & m_nCurrentLine

    ' Call the error handler.
    m_db.ErrorHandlerReport sMessage, _
        nErrNumber:=nErrNumber, _
        sErrSource:=sErrSource, _
        sErrDescription:=sErrDescription, _
        sTitle:=sTitle, _
        sLogInfo:=sLogInfo

    ' Also write the error to the log tables.
    ScriptLoggingError sMessage, _
        nErrNumber:=nErrNumber, _
        sErrSource:=sErrSource, _
        sErrDescription:=sErrDescription, _
        sLogInfo:=sLogInfo, _
        bIgnored:=False

End Sub

Private Sub ScriptLoggingStart(ByVal sFile As String)

    Dim sPath As String

    On Error GoTo EH_Handler
    m_lLoggingID = 0

    ' If the stored procedure does not exist, then this code will be skipped.
    If Not m_bIntLogScriptStart Then
        Exit Sub
    End If

    If m_cmdIntLogScriptStart Is Nothing Then
        Set m_cmdIntLogScriptStart = m_db.NewCommandHelper("IntLogScriptStart", True)
        With m_cmdIntLogScriptStart
            .CreateParameter "@sUserName", adVarChar, , 20
            .CreateParameter "@sWinComputerName", adVarChar, , 31
            .CreateParameter "@sWinUserName", adVarChar, , 255
            .CreateParameter "@sScriptPath", adVarChar, , 255
            .CreateParameter "@sScriptFile", adVarChar, , 255
            .CreateParameter "@r_lID", adInteger, adParamOutput
        End With
    End If

    sPath = ParsePathFile(sFile, sFile)

    m_cmdIntLogScriptStart("@sUserName") = m_sUserName
    m_cmdIntLogScriptStart("@sWinComputerName") = GetWindowsComputerName()
    m_cmdIntLogScriptStart("@sWinUserName") = GetWindowsUserName()
    m_cmdIntLogScriptStart("@sScriptPath") = sPath
    m_cmdIntLogScriptStart("@sScriptFile") = sFile
    m_cmdIntLogScriptStart.Execute
    m_lLoggingID = ToLong(m_cmdIntLogScriptStart("@r_lID"))

    Exit Sub

EH_Handler:
    Select Case ScriptErrorDialog("Cannot log the script.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub ScriptLoggingStop(ByVal nStatus As EScriptStatuses)

    On Error Resume Next

    ' If the stored procedure does not exist, then this code will be skipped.
    If Not m_bIntLogScriptFinish Then
        Exit Sub
    End If

    ' If ScriptLoggingStart() failed, then this code will be skipped.
    If m_lLoggingID = 0 Then
        Exit Sub
    End If

    If m_cmdIntLogScriptFinish Is Nothing Then
        Set m_cmdIntLogScriptFinish = m_db.NewCommandHelper("IntLogScriptFinish", True)
        With m_cmdIntLogScriptFinish
            .CreateParameter "@lID", adInteger
            .CreateParameter "@nStatus", adTinyInt
        End With
    End If

    m_cmdIntLogScriptFinish("@lID") = m_lLoggingID
    m_cmdIntLogScriptFinish("@nStatus") = nStatus
    m_cmdIntLogScriptFinish.Execute

End Sub

Private Sub ScriptLoggingError(ByVal sMessage As String, _
    ByVal nErrNumber As Long, _
    ByVal sErrSource As String, _
    ByVal sErrDescription As String, _
    ByVal sLogInfo As String, _
    ByVal bIgnored As Boolean)

    Dim sDBErrors As String
    Dim sText As CStringBuilder

    On Error Resume Next

    ' If the stored procedure does not exist, then this code will be skipped.
    If Not m_bIntLogScriptError Then
        Exit Sub
    End If

    ' If ScriptLoggingStart() failed, then this code will be skipped.
    If m_lLoggingID = 0 Then
        Exit Sub
    End If

    If m_cmdIntLogScriptError Is Nothing Then
        Set m_cmdIntLogScriptError = m_db.NewCommandHelper("IntLogScriptError", True)
        With m_cmdIntLogScriptError
            .CreateParameter "@lID", adInteger
            .CreateParameter "@bIgnored", adTinyInt
            .CreateParameter "@sText", adLongVarChar, , 2147483647
        End With
    End If

    ' If the last DB error is equal to the VB error, then blank out the VB error to avoid
    ' unnecessary duplication in the error logs.
    If m_db.LastErrorWasVB(nErrNumber, sErrSource) Then
        nErrNumber = 0
        sErrSource = ""
        sErrDescription = ""
    End If

    sDBErrors = m_db.LastErrorsText()

    Set sText = New CStringBuilder
    sText.InitialiseSeparator vbCrLf
    sText.AppendSeparator
    sText.Append sMessage
    sText.TrimRight
    If Len(sDBErrors) <> 0 Then
        sText.AppendSeparator
        sText.Append sDBErrors
        sText.TrimRight
    End If
    If nErrNumber <> 0 Then
        sText.AppendSeparator
        sText.Append sErrDescription
        sText.TrimRight
        sText.Append " ("
        sText.Append nErrNumber
        sText.Append ") ("
        sText.Append sErrSource
        sText.Append ")"
    End If
    If Len(sLogInfo) <> 0 Then
        sText.AppendSeparator
        sText.Append sLogInfo
        sText.TrimRight
    End If

    m_cmdIntLogScriptError("@lID") = m_lLoggingID
    m_cmdIntLogScriptError("@bIgnored") = IIf(bIgnored, 1, 0)
    m_cmdIntLogScriptError("@sText") = sText.Text
    m_cmdIntLogScriptError.Execute

End Sub

' Clean the text passed in using the current style settings.
Private Sub CleanUpTextSB(ByRef r_sSQL As CStringBuilder)

    Const ks2NewLines = vbCrLf & vbCrLf
    Const ks3NewLines = vbCrLf & vbCrLf & vbCrLf

    Dim sTextOriginal As CStringBuilder
    Dim sTextWorking As CStringBuilder
    Dim sTextChunk As CStringBuilder
    Dim sTextFinal As CStringBuilder
    Dim sCommentMark As String
    Dim nNestLevel As Long
    Dim sLine As CStringBuilder

    If m_bRemoveComments Then

        ' Remove "/* */" comments from the text. Assume they can be
        ' nested even if SQL does not, because we would only get nested
        ' comments if the developer intended them to be nested and
        ' didn't realise that wouldn't work. We therefore go with the
        ' developer's intention.
        Set sTextOriginal = r_sSQL
        Set sTextWorking = New CStringBuilder
        nNestLevel = 0
        Do
            ' Find next comment mark.
            Set sTextChunk = ParseBlockCommentSB(sTextOriginal, sCommentMark)
            If sCommentMark = "/*" Then
                ' Comment start found. Preserve all text up to the
                ' start of the outermost comment only.
                If nNestLevel = 0 Then
                    sTextWorking.Append sTextChunk
                End If
                nNestLevel = nNestLevel + 1
            ElseIf sCommentMark = "*/" Then
                ' Comment end found.
                nNestLevel = nNestLevel - 1
                If nNestLevel < 0 Then
                    ' Nesting is out of step, raise a syntax error.
                    Err.Raise knErrBadClosingComment, ksErrSource, ksErrBadClosingComment
                End If
            Else
                ' No more comments in the text. Keep the remainder
                ' of the text in its entirety if we are not currently
                ' inside a comment, then exit the loop.
                If nNestLevel > 0 Then
                    ' Nesting is out of step, raise a syntax error.
                    Err.Raise knErrBadOpeningComment, ksErrSource, ksErrBadOpeningComment
                End If
                sTextWorking.Append sTextChunk
                Exit Do
            End If
        Loop

        ' Remove "--" comments, right-trim every line, and close up blank lines.
        Set sTextFinal = New CStringBuilder
        Do
            Set sLine = ParseSepSB(sTextWorking, vbCrLf)
            ParseEOLCommentSB sLine
            If sLine.length <> 0 Then
                sTextFinal.Append sLine
                sTextFinal.Append vbCrLf
            End If
        Loop Until sTextWorking.length = 0

    Else

        Set sTextWorking = r_sSQL

        ' Right-trim every line.
        Set sTextFinal = New CStringBuilder
        Do
            Set sLine = ParseSepSB(sTextWorking, vbCrLf)
            sLine.TrimRight
            sTextFinal.Append sLine
            sTextFinal.Append vbCrLf
        Loop Until sTextWorking.length = 0

        ' Remove leading and trailing whitespace, including blank lines.
        sTextFinal.TrimLeft
        sTextFinal.TrimRight

        ' Close up multiple blank lines into one.
        sTextFinal.Replace ks3NewLines, ks2NewLines

        ' Append one newline at the end of the text. This is not really necessary for
        ' correct script execution, and I would like to leave it out, but it is required
        ' for proving that this new code produces identical results to the old code.
        sTextFinal.Append vbCrLf

    End If

    ' Return the final result.
    Set r_sSQL = sTextFinal

End Sub

' Find and parse out the next block comment marker. At the moment, this code does a simple
' text search for the comment markers. If we ever need to tokenise the text to find only
' syntactically-valid comments, then we only need to modify this function and the calling
' code will still work.
Private Function ParseBlockCommentSB(ByRef r_sText As CStringBuilder, _
    ByRef o_sCommentMark As String) As CStringBuilder

    Dim iPosCommentStart As Long
    Dim iPosCommentEnd As Long
    Dim iPosSplit As Long

    ' Search for the comment start and end markers in the text.
    iPosCommentStart = InStr(r_sText.Text, "/*")
    iPosCommentEnd = InStr(r_sText.Text, "*/")

    If iPosCommentStart > 0 And iPosCommentEnd > 0 Then
        iPosSplit = IIf(iPosCommentStart > iPosCommentEnd, iPosCommentEnd, iPosCommentStart)
    ElseIf iPosCommentStart > 0 Then
        iPosSplit = iPosCommentStart
    ElseIf iPosCommentEnd > 0 Then
        iPosSplit = iPosCommentEnd
    Else
        iPosSplit = 0
    End If

    If iPosSplit > 0 Then
        o_sCommentMark = r_sText.Mid(iPosSplit, 2)
        Set ParseBlockCommentSB = New CStringBuilder
        ParseBlockCommentSB.Text = r_sText.Left$(iPosSplit - 1)
        r_sText.Text = r_sText.Mid(iPosSplit + 2)
    Else
        o_sCommentMark = ""
        Set ParseBlockCommentSB = r_sText
        Set r_sText = New CStringBuilder
    End If

End Function

' Find and parse out any line comment in this text. At the moment, this code does a
' simple text search for the comment marker. If we ever need to tokenise the text to
' find only syntactically-valid comments, then we only need to modify this function
' and the calling code will still work.
Private Sub ParseEOLCommentSB(ByVal sText As CStringBuilder)

    Dim iPos As Long

    ' Search for the comment marker in the text.
    iPos = InStr(sText.Text, "--")

    If iPos > 0 Then
        sText.Text = sText.Left(iPos - 1)
        sText.TrimRight
    End If

End Sub

' A copy of the library function ParseSep() that works directly on a CStringBuilder
' for extra speed.
Private Function ParseSepSB(ByRef r_sText As CStringBuilder, _
    ByVal sSeparator As String) As CStringBuilder

    Dim iPos As Long

    ' Search for the separator in the text.
    iPos = InStr(r_sText.Text, sSeparator)

    If iPos > 0 Then
        Set ParseSepSB = New CStringBuilder
        ParseSepSB.Text = r_sText.Left(iPos - 1)
        r_sText.Text = r_sText.Mid(iPos + Len(sSeparator))
    Else
        Set ParseSepSB = r_sText
        Set r_sText = New CStringBuilder
    End If

End Function

Private Sub LogAction(ByVal sSQL As CStringBuilder)

    Dim sFile As String
    Dim hFile As Integer

    If Not m_bLogActions Then Exit Sub
    If Len(m_sLogActionsTo) = 0 Then Exit Sub

    On Error GoTo EH_LogAction

    m_nLogSequence = m_nLogSequence + 1
    sFile = AddSlash(m_sLogActionsTo) & Format$(m_nLogSequence, "0000000000") & ".sql"
    hFile = FreeFile()
    Open sFile For Output Access Write Lock Write As hFile
    Print #hFile, sSQL.Text

EX_LogAction:
    On Error Resume Next
    Close hFile
    Exit Sub

EH_LogAction:
    Select Case ScriptErrorDialog("Cannot log this action to disk.", _
        nButtons:=ebRetryCancelIgnoreIgnoreAll, _
        sLogInfo:="File: " & sFile)
    Case erRetry
        Resume
    Case erIgnore
        Resume EX_LogAction
    Case erIgnoreAll
        m_bLogActions = False
        Resume EX_LogAction
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Function ProcedureExists(ByVal sName As String) As Boolean

    Dim rs As ADODB.Recordset

    On Error Resume Next
    ProcedureExists = False

    If m_cmdProcedureExists Is Nothing Then
        Set m_cmdProcedureExists = m_db.NewCommandHelper( _
            "select objectproperty(object_id(?), 'IsProcedure')", False, True)
        With m_cmdProcedureExists
            Set m_parProcedureExistsName = .CreateParameter("@name", adVarWChar, , 128)
        End With
    End If
    m_parProcedureExistsName.Value = sName
    Set rs = m_cmdProcedureExists.OpenRecordset()
    ProcedureExists = Not IsNull(rs(0))
    rs.Close

End Function

' Detect whether the specified code is already present in the run once table.
Private Function SetupDBRunOnceExists(ByVal sCode As String) As Boolean

    On Error GoTo EH_Handler

    ' If the stored procedure does not exist, then this code will be skipped (returning false by default).
    If Not m_bSetupDBRunOnce Then
        Exit Function
    End If

    ' Cache all the run once data in memory.
    SetupDBRunOnceSelect

    ' Search for the code in the cache.
    m_rsSetupDBRunOnce.Filter = "code = " & ToJSQL(sCode)
    SetupDBRunOnceExists = Not m_rsSetupDBRunOnce.BOF And Not m_rsSetupDBRunOnce.EOF

    Exit Function

EH_Handler:
    ' This error handler CANNOT ALLOW the user to ignore.
    Select Case ScriptErrorDialog("Cannot read from the run once table.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Function

' Record the specified code in the run once table.
Private Sub SetupDBRunOnceInsert(ByVal sCode As String, ByVal sDescription As String)

    On Error GoTo EH_Handler

    ' If the stored procedure does not exist, then this code will be skipped.
    If Not m_bSetupDBRunOnce Then
        Exit Sub
    End If

    ' Cache all the run once data in memory.
    SetupDBRunOnceSelect

    If m_cmdSetupDBRunOnceInsert Is Nothing Then
        Set m_cmdSetupDBRunOnceInsert = m_db.NewCommandHelper("spu_SetupDB_Run_Once_Insert", True)
        With m_cmdSetupDBRunOnceInsert
            Set m_parSetupDBRunOnceInsertCode = .CreateParameter("@code", adVarChar, , 256)
            Set m_parSetupDBRunOnceInsertDescription = .CreateParameter("@description", adVarChar, , 256)
        End With
    End If

    ' Write the data to the database.
    m_parSetupDBRunOnceInsertCode = sCode
    m_parSetupDBRunOnceInsertDescription = ToString(sDescription, Null)
    m_cmdSetupDBRunOnceInsert.Execute

    ' Add the data to the cache.
    m_rsSetupDBRunOnce.AddNew
    m_rsSetupDBRunOnce("code") = sCode
    m_rsSetupDBRunOnce("description") = ToString(sDescription, Null)
    m_rsSetupDBRunOnce.Update

    Exit Sub

EH_Handler:
    ' This error handler CANNOT ALLOW the user to ignore.
    Select Case ScriptErrorDialog("Cannot write to the run once table.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Cache all the run once data in memory.
Private Sub SetupDBRunOnceSelect()

    On Error GoTo EH_Handler

    ' If the stored procedure does not exist, then this code will be skipped.
    If Not m_bSetupDBRunOnce Then
        Exit Sub
    End If

    If m_rsSetupDBRunOnce Is Nothing Then
        Dim cmdSetupDBRunOnceSelect As CCommandHelper
        Set cmdSetupDBRunOnceSelect = m_db.NewCommandHelper("spu_SetupDB_Run_Once_Select", True)
        Dim rs As ADODB.Recordset
        Set rs = cmdSetupDBRunOnceSelect.OpenRecordset()
        CopyRecordset rs, m_rsSetupDBRunOnce
        rs.Close
        m_rsSetupDBRunOnce.Sort = "code"
    End If

    Exit Sub

EH_Handler:
    ' This error handler CANNOT ALLOW the user to ignore.
    Select Case ScriptErrorDialog("Cannot read from the run once table.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Record the specified file in the script log table.
Private Function SetupDBScriptLogInsert(ByVal sFileName As String) As Long

    On Error GoTo EH_Handler

    ' If the stored procedure does not exist, then this code will be skipped.
    If Not m_bSetupDBScriptLog Then
        Exit Function
    End If

    If m_cmdSetupDBScriptLogInsert Is Nothing Then
        Set m_cmdSetupDBScriptLogInsert = m_db.NewCommandHelper("spu_SetupDB_Script_Log_Insert", True)
        With m_cmdSetupDBScriptLogInsert
            Set m_parSetupDBScriptLogInsertVersionFrom = .CreateParameter("@version_from", adVarChar, , 30)
            Set m_parSetupDBScriptLogInsertVersionTo = .CreateParameter("@version_to", adVarChar, , 30)
            Set m_parSetupDBScriptLogInsertFileName = .CreateParameter("@file_name", adVarChar, , 1024)
            Set m_parSetupDBScriptLogInsertID = .CreateParameter("@id", adInteger, adParamOutput)
        End With
    End If

    m_parSetupDBScriptLogInsertVersionFrom = m_sVersionFrom
    m_parSetupDBScriptLogInsertVersionTo = m_sVersionTo
    m_parSetupDBScriptLogInsertFileName = sFileName
    m_cmdSetupDBScriptLogInsert.Execute
    SetupDBScriptLogInsert = ToLong(m_parSetupDBScriptLogInsertID)

    Exit Function

EH_Handler:
    Select Case ScriptErrorDialog("Cannot write to the script log table.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Exit Function
    Case Else
        Err.Raise knErrAbort
    End Select

End Function
