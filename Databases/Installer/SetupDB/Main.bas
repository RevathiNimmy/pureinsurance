Attribute VB_Name = "MMain"
'* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
'   Program:        Sirius Database Installer
'   Author:         C Hayter - 29/1/2002
'   Swift Version:  N/A
'   Data Version:   N/A
'
'   Sirius Financial Systems plc, Sirius House, Reddicroft,
'   Sutton Coldfield, West Midlands, B73 6BN
'   Copyright (c) 2002-2005. All rights reserved worldwide.
'
'   Language:       VB 6.0 SP5
'   Libraries:
'   Project:        setupdb.vbp
'
'* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

Option Explicit

Public Const ksProjectName = "setupdb"

Public Const ksRegPMRoot = "Software\Pure"
Public Const ksRegProject = ksRegPMRoot & "\DatabaseInstaller\Server"
Public Const ksRegPMDAODataSources = ksRegPMRoot & "\Architecture\Server\Databases"
Public Const ksRegMSSQLServer = "Software\Microsoft\MSSQLServer"
Public Const ksRegODBCDSNs = "Software\ODBC\ODBC.INI"
Public Const ksRegODBCDrivers = "Software\ODBC\ODBCINST.INI"
Public Const ksRegODBCDSNList = ksRegODBCDSNs & "\ODBC Data Sources"

Public Const ksOLEDBProvider = "SQLOLEDB"
Public Const ksMasterDatabaseName = "master"

Public Const ksMasterScriptFile = "All.spp"
Public Const ksConfigFile = "Config.xml"
Public Const ksUpgradeOrderFile = "Upgrade.lst"
Public Const ksInstallLogFile = "DbInstall.log"

Public Const ksFolderAfter = "After"
Public Const ksFolderBefore = "Before"
Public Const ksFolderCreate = "Create"
Public Const ksFolderUpgrade = "Upgrade"

Public Const ksArchitectureName = "architecture"

Public Const ksInternalICCS = "4033"

' Specially-recognised parameter values.
Public Const ksParamTrue = "yes"
Public Const ksParamFalse = "no"
Public Const ksParamLastValue = "(last)"
Public Const ksParamMaxVersion = "(max)"
Public Const ksParamEmptyValue = "(none)"

' Application status flag values.
Public Enum EAppStatuses
    knAppCompleted
    knAppRunning
    knAppFatalError
    knAppCancelled
    knAppBackupFailed
End Enum

' Application tasks done flag values.
' These are bit masks and do NOT go up sequentially.
Public Enum EAppTasksDone
    knAppNothing = 0
    knAppCreatedArchitecture = 1
    knAppInternalICCS = 2
    knAppDatabaseBackup = 4
    knAppErrorIgnored = 8
End Enum

' What to do next when creating the database.
Private Enum EActions
    knContinue
    knTryAnotherServer
    knCreateSiriusLogin
    knCreateSiriusDatabase
End Enum

' Enforce a timeout of at least 10 minutes for backup and restore operations.
Private Const knMinimumTimeoutForBackupRestore = 600

' Command line.
Private m_oParameters As CParameters
Private m_bNamedParameters As Boolean

' Are we recovering or un-installing?
Private m_bRecovering As Boolean
Private m_bUnInstalling As Boolean

' Install location information.
Private m_sInstallFolder As String
Private m_sProductDataFolder As String
Private m_sServerName As String
Private m_sDatabaseName As String
Private m_sBackupData As String ' ksParamTrue or ksParamFalse
Private m_sDataFilesFolder As String
Private m_sLogFilesFolder As String
Private m_sBackupFilesFolder As String
Private m_sLoginName As String
Private m_sPassword As String

' Install control files.
Private m_sInstallConfigFile As String
Private m_sUpgradeOrderFile As String

' Application tasks done.
Private m_nAppTasksDone As EAppTasksDone

' Sirius Language ID for updating PMProduct records.
' The data type is correct (the table uses a SMALLINT).
Private m_lLanguageID As Integer

' Cached filespec of the ODBC driver.
Private m_sODBCDriverFile As String

' Logical database and install task information.
Private m_oTasks As CLogicalDatabases

' Carry out licensing tasks after installation?
Private m_bLicensingTasks As Boolean

' ICCS number (if we are licensing).
Private m_sICCS As String

' The order in which to do upgrades. This collection may be empty
' if the config file does not exist.
Private m_oUpgradeOrderRows As CUpgradeOrderRows

' The installer log file to write to.
Private m_sInstallLogFile As String

' Are we logging debugging information as well?
Private m_bDebugLogging As Boolean

' Are we using lax SQL Server security?
Private m_bLaxSQLServerSecurity As Boolean

' Gary Pagett's generic installer "developer mode" flag.
Private m_bDeveloperMode As Boolean

' Allow certain buttons in dialog boxes?
Private m_bAllowUserToCancelDialog As Boolean
Private m_bAllowUserToCancelMain As Boolean
Private m_bAllowUserToIgnore As Boolean
Private m_bBatchMode As Boolean
Private m_bAllowUserToSkipBackup As Boolean

' Temporary variable to hold the display name of the login
' currently being used to connect to the database, for use
' in error handlers.
Private m_sCurrentLoginDisplayName As String

' Data sources to create during install.
Private m_sPMDAODataSource As String
Private m_colODBCDSNs As Collection

' UI default values.
Private m_sUIDefaultODBCDSN As String
Private m_sUIDefaultServerName As String
Private m_sUIDefaultDatabaseName As String
Private m_sUIDefaultBackupData As String

' The error handler.
Private m_eh As CErrorHandler

' The database connection.
Private m_dbDatabase As CDatabase

' Cached stored procedures.
Private m_prcSetLogicalDatabaseVersion As ADODB.Command

' Error declarations.
Private Const ksErrSource = ksProjectName '& ".MMain"
Private Const knErrBadConfigFile = vbObjectError + 1
Private Const ksErrBadConfigFile = "Cannot read the configuration file: " ' add filename here
Private Const knErrODBCDriverNotFound = vbObjectError + 2
Private Const ksErrODBCDriverNotFound = "The required ODBC Driver is not installed."
Private Const knErrBadCommandLine = vbObjectError + 3
Private Const ksErrBadCommandLine = "Syntax error on command line: " ' add specifics here
Private Const knErrUOFSyntax = vbObjectError + 4
Private Const ksErrUOFSyntax = "Name\Version pair expected on line: " ' add line number here
Private Const knErrUOFOrder = vbObjectError + 5
Private Const ksErrUOFOrder = "Version not in ascending order on line: " ' add line number here
Private Const knErrNoSiriusLogFile = vbObjectError + 6
Private Const ksErrNoSiriusLogFile = "The location of the Pure error log file is not defined."
Private Const knErrSALoginUnknownPassword = vbObjectError + 7
Private Const ksErrSALoginUnknownPassword = "The 'sa' login password is unknown, and prompting the user for it is disabled."

' Only backup when we are going to be making changes to the database.
Private m_bBackUpOnlyWhenRequired As Boolean
Private m_bBackUpRequired As Boolean
Private m_bBackUpFailed As Boolean

Public Sub Main()

    Dim sAppStatus As String

    On Error GoTo EH_Handler

    ' Set the application status flags. This must be done first.
    SetAppStatus knAppRunning
    SetAppTasksDone knAppNothing

    ' Initialise error handler. This must be done second.
    InitialiseErrorHandler

    ' Show the main application window.
    MRUComboRegFolder = ksRegProject
    FMain.Show
    FMain.MousePointer = vbHourglass
    FMain.ShowText "Initialising..."

    WriteToLog "BEGIN - " & App.Title & " " & AppVersion() & " started at " & Now()

    ' Parse the first part of the command line.
    ParseCommandLine1

    ' Initialise database connection.
    InitialiseDatabase

    ' Carry out the appropriate action.
    If m_bRecovering Then
        DoRecover
    ElseIf m_bUnInstalling Then
        DoUnInstall
    Else
        DoInstall
    End If

    ' Set the application status flag.
    SetAppStatus knAppCompleted
    sAppStatus = "finished successfully"

EX_NormalExit:
    ' Close the main application window,
    ' thereby closing the application.
    Unload FMain
    Set FMain = Nothing

    WriteToLog "END - " & App.Title & " " & sAppStatus & " at " & Now() & vbCrLf

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Error occurred while installing, upgrading or un-installing the database.")
    Case erRetry
        Resume
    Case Else
        ' Set the application status flag.
        If m_bBackUpFailed Then
            SetAppStatus knAppBackupFailed
            sAppStatus = "database backup failed"
        ElseIf FMain.Cancelled Then
            SetAppStatus knAppCancelled
            sAppStatus = "cancelled by user"
        Else
            SetAppStatus knAppFatalError
            sAppStatus = "aborted with an error"
        End If
        Resume EX_NormalExit
    End Select

End Sub

Public Sub MainShutdown()

    ' However far we got, shut down everything cleanly.
    On Error Resume Next

    m_dbDatabase.Disconnect
    Set m_dbDatabase = Nothing

    Set m_oTasks = Nothing
    Set m_colODBCDSNs = Nothing

    Set m_eh = Nothing

End Sub

Public Property Get AllowUserToCancelDialog() As Boolean
    AllowUserToCancelDialog = m_bAllowUserToCancelDialog
End Property

Public Property Get AllowUserToCancelMain() As Boolean
    AllowUserToCancelMain = m_bAllowUserToCancelMain
End Property

Public Property Get AllowUserToIgnore() As Boolean
    AllowUserToIgnore = m_bAllowUserToIgnore
End Property

Public Property Get BatchMode() As Boolean
    BatchMode = m_bBatchMode
End Property

Public Property Get AllowUserToSkipBackup() As Boolean
    AllowUserToSkipBackup = m_bAllowUserToSkipBackup
End Property

Public Property Get Database() As CDatabase
    Set Database = m_dbDatabase
End Property

Public Property Get DebugLogging() As Boolean
    DebugLogging = m_bDebugLogging
End Property

' Retrieves data for display by the {CTRL+F12} debugging key.
Public Function DebugMessagePopup() As String

    Dim bNewLine As Boolean
    Dim nODBCDSNsCount As Long
    Dim oTask As CLogicalDatabase
    Dim sText As String

    On Error Resume Next

    sText = "App Version: " & AppVersion() & vbCrLf
    sText = sText & vbCrLf

    If m_oParameters Is Nothing Then
        sText = sText & "Command Line: <parser not instantiated>" & vbCrLf
    Else
        sText = sText & "Command Line: " & m_oParameters.DebugMessagePopup() & vbCrLf
    End If
    sText = sText & vbCrLf

    If m_oUpgradeOrderRows Is Nothing Then
        sText = sText & "Upgrade Order List: not yet read" & vbCrLf
        sText = sText & vbCrLf
    ElseIf m_oUpgradeOrderRows.Count = 0 Then
        sText = sText & "Upgrade Order List: not provided" & vbCrLf
        sText = sText & vbCrLf
    Else
        sText = sText & "Upgrade Order List: read OK" & vbCrLf
        sText = sText & vbCrLf
    End If

    sText = sText & "Install Folder: " & m_sInstallFolder & vbCrLf
    sText = sText & "Product Data Folder: " & m_sProductDataFolder & vbCrLf
    sText = sText & "Server Name: " & m_sServerName & vbCrLf
    sText = sText & "Database Name: " & m_sDatabaseName & vbCrLf
    sText = sText & "Backup Data? " & m_sBackupData & vbCrLf
    sText = sText & "Data Files Folder: " & m_sDataFilesFolder & vbCrLf
    sText = sText & "Log Files Folder: " & m_sLogFilesFolder & vbCrLf
    sText = sText & "Backup Files Folder: " & m_sBackupFilesFolder & vbCrLf
    sText = sText & vbCrLf

    bNewLine = False
    If Not m_oTasks Is Nothing Then
        For Each oTask In m_oTasks
            sText = sText & oTask.Name & ": " & oTask.CommandLineVersion & vbCrLf
            bNewLine = True
        Next
    End If
    If bNewLine Then
        sText = sText & vbCrLf
    End If

    If Not m_colODBCDSNs Is Nothing Then
        nODBCDSNsCount = m_colODBCDSNs.Count
    End If

    sText = sText & "Licensing Tasks? " & m_bLicensingTasks & vbCrLf
    sText = sText & "Lax SQL Server Security? " & m_bLaxSQLServerSecurity & vbCrLf
    sText = sText & "Use Event Log? " & m_eh.LogToWindowsEventLog & vbCrLf
    sText = sText & "PMDAO Data Source: " & m_sPMDAODataSource & vbCrLf
    sText = sText & "ODBC DSNs Count: " & nODBCDSNsCount & vbCrLf
    sText = sText & "UI Default ODBC DSN: " & m_sUIDefaultODBCDSN & vbCrLf
    sText = sText & "UI Default Server Name: " & m_sUIDefaultServerName & vbCrLf
    sText = sText & "UI Default Database Name: " & m_sUIDefaultDatabaseName & vbCrLf
    sText = sText & "UI Default Backup Data: " & m_sUIDefaultBackupData & vbCrLf
    sText = sText & vbCrLf

    If m_dbDatabase Is Nothing Then
        sText = sText & "Error Log File: <error handler not instantiated>" & vbCrLf
    Else
        sText = sText & "Error Log File: " & m_eh.LogFile & vbCrLf
    End If

    sText = sText & "Install Log File: " & m_sInstallLogFile & vbCrLf
    sText = sText & "Debug Logging? " & m_bDebugLogging & vbCrLf
    sText = sText & "Developer Mode? " & m_bDeveloperMode & vbCrLf
    sText = sText & "Allow User to Cancel Dialogs? " & m_bAllowUserToCancelDialog & vbCrLf
    sText = sText & "Allow User to Cancel Main? " & m_bAllowUserToCancelMain & vbCrLf
    sText = sText & "Allow User to Ignore? " & m_bAllowUserToIgnore & vbCrLf
    sText = sText & "Batch Mode? " & m_bBatchMode & vbCrLf
    sText = sText & "Allow User to Skip Backup? " & m_bAllowUserToSkipBackup & vbCrLf

    DebugMessagePopup = sText

End Function

Public Sub SetAppStatus(ByVal nAppStatus As EAppStatuses)

    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "AppStatus", nAppStatus

End Sub

Public Sub SetAppTasksDone(ByVal nAppTasksDone As EAppTasksDone)

    If nAppTasksDone = knAppNothing Then
        ' Clear all tasks done.
        m_nAppTasksDone = knAppNothing
    Else
        ' Add this task to the list.
        m_nAppTasksDone = m_nAppTasksDone Or nAppTasksDone
    End If

    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "AppTasksDone", m_nAppTasksDone

End Sub

Private Sub InitialiseErrorHandler()

    Const ksRegSiriusLogFile = ksRegPMRoot

    Dim sLogFile As String
    Dim nButtons As VbMsgBoxStyle

    ' **** WARNING ****
    ' We must be very careful with the order that things happen in here, and the error
    ' handling logic used. Various settings are read that change the way that errors
    ' are handled. Some of these settings may be modifed on the command line, but they
    ' MUST be set to reasonable safe defaults here first.
    ' **** WARNING ****

    On Error GoTo EH_Handler
    m_bAllowUserToCancelDialog = False
    m_bAllowUserToCancelMain = False
    m_bAllowUserToIgnore = False
    m_bAllowUserToSkipBackup = False

    ' Set error handling behaviour for the whole application.
    Set m_eh = New CErrorHandler
    m_eh.AppTitle = App.Title
    m_eh.AppVersion = AppVersion()
    m_eh.Diagnostics = False
    m_eh.UseBothErrorParams = True
    m_eh.LogToWindowsEventLog = True

    ' Read hidden registry flags.
    m_bDeveloperMode = ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegPathPMRoot, "DEVEL", False))
    m_bDebugLogging = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "DebugLogging", False)

    ' As an extra safety precaution requested by Gary Pagett, these two flags are
    ' dependent on the developer flag.
    m_bAllowUserToCancelDialog = m_bDeveloperMode And ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToCancelDialog", ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToCancel", False))))
    m_bAllowUserToCancelMain = m_bDeveloperMode And ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToCancelMain", ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToCancel", False))))
    m_bAllowUserToIgnore = m_bDeveloperMode And ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToIgnore", False))
    m_bBatchMode = False
    m_bAllowUserToSkipBackup = m_bDeveloperMode And ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToCancelDialog", ToBoolean(RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "AllowUserToSkipBackup", False))))

    ' Set the correct Sirius error log file.
    sLogFile = RegRead(HKEY_CURRENT_USER, ksRegSiriusLogFile, "LogFileName", "")
    If sLogFile = "" Then
        sLogFile = RegRead(HKEY_LOCAL_MACHINE, ksRegSiriusLogFile, "LogFileName", "")
    End If
    If sLogFile = "" Then
        sLogFile = AddSlash(RegRead(HKEY_LOCAL_MACHINE, ksRegPMRoot, "PMDIR", "")) & "Pure\Pure.log"
    End If
    If sLogFile = "" Then
        Err.Raise knErrNoSiriusLogFile, ksErrSource, ksErrNoSiriusLogFile
    End If
    m_eh.LogFile = sLogFile

    ' Obtain the correct installer log file. This is put in the same folder as
    ' the Sirius error log.
    m_sInstallLogFile = AddSlash(ParsePathFile(m_eh.LogFile, "")) & ksInstallLogFile

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Nothing, "Cannot initialise the error handler.")
    Case vbRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub InitialiseDatabase()

    On Error GoTo EH_Handler

    ' This goes in its own routine so that we can trap and diagnose any failure to
    ' instantiate the ADODB.Connection object.
    Set m_dbDatabase = New CDatabase

    ' Attach the separate error handler object we created earlier.
    Set m_dbDatabase.ErrorHandler = m_eh

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot initialise the database connection.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Public Function ParseBoolean(ByVal sValue As String, ByVal bDefault As Boolean) As Boolean

    Select Case LCase$(Trim$(sValue))
    Case "0", "n", ksParamFalse, "false"
        ParseBoolean = False
    Case "1", "y", ksParamTrue, "true"
        ParseBoolean = True
    Case Else
        ParseBoolean = bDefault
    End Select

End Function

Private Sub ParseCommandLine1()

    Dim sAction As String
    Dim sDataConnectionPropertyDefault As String
    Dim sLaxSQLServerSecurity As String
    Dim sLicensingTasks As String
    Dim sUseEventLog As String
    Dim sAllowUserToCancelDialog As String
    Dim sAllowUserToCancelMain As String
    Dim sAllowUserToCancel As String
    Dim sAllowUserToIgnore As String
    Dim sBatchMode As String
    Dim sBackUpOnlyWhenRequired As String
    Dim sAllowUserToSkipBackup As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ParseCommandLine1"
    End If

    ' Record the entire command line for diagnostic purposes.
    ' WriteToLog "Command line: " & Command$()

    ' First we parse the entire command line into a parameters object.
    ' This is deliberately designed to handle both old and new syntaxes
    ' and is thus ideal for simplifying the code.
    Set m_oParameters = New CParameters
    m_oParameters.Parse Command$()

    ' Next we must determine the type of command line: old or new.
    ' A new-style command line has a mandatory parameter "act". This is
    ' good for future expansion because we can add actions other than
    ' "install" or "uninstall".
    sAction = m_oParameters.ReadByName("act", m_bNamedParameters)
    m_bRecovering = False
    m_bUnInstalling = False

    If m_bNamedParameters Then
        If LCase$(sAction) = "install" Then
            ' It's a new command line for installing.

            ' Only read the fixed parameters here. Leave the
            ' remainder of the parameters for later processing.
            m_sInstallFolder = m_oParameters.ReadByName("if")
            m_sProductDataFolder = m_oParameters.ReadByName("pdf")
            m_sServerName = m_oParameters.ReadByName("sn")
            m_sDatabaseName = m_oParameters.ReadByName("dn")
            m_sBackupData = m_oParameters.ReadByName("dbk")
            m_sDataFilesFolder = m_oParameters.ReadByName("ddf")
            m_sLogFilesFolder = m_oParameters.ReadByName("dlf")
            m_sBackupFilesFolder = m_oParameters.ReadByName("dbf")
            m_sInstallConfigFile = m_oParameters.ReadByName("iff")
            m_sUpgradeOrderFile = m_oParameters.ReadByName("uof")
            sLaxSQLServerSecurity = m_oParameters.ReadByName("lss")
            sLicensingTasks = m_oParameters.ReadByName("lic")
            sUseEventLog = m_oParameters.ReadByName("evlog")
            sBackUpOnlyWhenRequired = m_oParameters.ReadByName("buowr")
			m_sLoginName = m_oParameters.ReadByName("loginname")
            m_sPassword = m_oParameters.ReadByName("loginpwd")
			
            ' Extra convenience: if the parameter "dcpd" exists, then
            ' use it to replace blank values read in above. This is only
            ' really useful for specifying "(last)".
            sDataConnectionPropertyDefault = m_oParameters.ReadByName("dcpd")
            If sDataConnectionPropertyDefault <> "" Then
                If m_sServerName = "" Then
                    m_sServerName = sDataConnectionPropertyDefault
                End If
                If m_sDatabaseName = "" Then
                    m_sDatabaseName = sDataConnectionPropertyDefault
                End If
                If m_sBackupData = "" Then
                    m_sBackupData = sDataConnectionPropertyDefault
                End If
                If m_sDataFilesFolder = "" Then
                    m_sDataFilesFolder = sDataConnectionPropertyDefault
                End If
                If m_sLogFilesFolder = "" Then
                    m_sLogFilesFolder = sDataConnectionPropertyDefault
                End If
                If m_sBackupFilesFolder = "" Then
                    m_sBackupFilesFolder = sDataConnectionPropertyDefault
                End If
            End If

        ElseIf LCase$(sAction) = "recover" Then
            ' It's a new command line for recovering an aborted install.
            m_bRecovering = True

            ' Read the location of the config.xml file.
            m_sInstallFolder = m_oParameters.ReadByName("if")
            m_sInstallConfigFile = m_oParameters.ReadByName("iff")
            sLaxSQLServerSecurity = m_oParameters.ReadByName("lss")

        ElseIf LCase$(sAction) = "uninstall" Then
            ' It's a new command line for un-installing.
            m_bUnInstalling = True

            ' Read the location of the config.xml file.
            m_sInstallFolder = m_oParameters.ReadByName("if")
            m_sInstallConfigFile = m_oParameters.ReadByName("iff")
            sLaxSQLServerSecurity = m_oParameters.ReadByName("lss")

        Else
            ' It's a badly-formed command line.
            Err.Raise knErrBadCommandLine, ksErrSource, ksErrBadCommandLine & "act=" & sAction

        End If

        ' Allow the install set creator to specify Cancel/Ignore button behaviour.
        ' These parameters are always read regardless of the action.
        sAllowUserToCancelDialog = m_oParameters.ReadByName("aucd")
        sAllowUserToCancelMain = m_oParameters.ReadByName("aucm")
        sAllowUserToCancel = m_oParameters.ReadByName("auc")
        sAllowUserToIgnore = m_oParameters.ReadByName("aui")
        sBatchMode = m_oParameters.ReadByName("batch")
        sAllowUserToSkipBackup = m_oParameters.ReadByName("ausb")
    Else
        sAction = m_oParameters.ReadFirst()
        If LCase$(sAction) = "/uninstall" Then
            ' It's an old command line for un-installing.
            m_bUnInstalling = True

            ' Read the location of the saved config.xml file.
            m_sInstallConfigFile = m_oParameters.ReadFirst()

        Else
            ' It's an old command line for installing.

            ' Only read the fixed parameters here. Leave the
            ' remainder of the parameters for later processing.
            m_sInstallFolder = sAction
            m_sProductDataFolder = m_oParameters.ReadFirst()
            m_sServerName = m_oParameters.ReadFirst()
            m_sDatabaseName = m_oParameters.ReadFirst()
            m_sBackupData = m_oParameters.ReadFirst()
            m_sDataFilesFolder = m_oParameters.ReadFirst()
            m_sLogFilesFolder = m_sDataFilesFolder
            m_sBackupFilesFolder = m_oParameters.ReadFirst()
			m_sLoginName = m_oParameters.ReadFirst()
            m_sPassword = m_oParameters.ReadFirst()
        End If
    End If

    ' If any of the fixed parameters have the value "(last)",
    ' then read the last known values from the registry. This enables
    ' installs after the first one to be done without user interaction.
    ' NB: The server and database names cannot be done here because
    ' they require values to be read from the install config file.
    If LCase$(m_sBackupData) = ksParamLastValue Then
        m_sBackupData = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "BackupData", "")
    End If
    If LCase$(m_sDataFilesFolder) = ksParamLastValue Then
        m_sDataFilesFolder = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "DataFilesFolder", "")
    End If
    If LCase$(m_sLogFilesFolder) = ksParamLastValue Then
        m_sLogFilesFolder = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "LogFilesFolder", RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "DataFilesFolder", ""))
    End If
    If LCase$(m_sBackupFilesFolder) = ksParamLastValue Then
        m_sBackupFilesFolder = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "BackupFilesFolder", "")
    End If

    ' The lax security flag defaults to False if not specified.
    m_bLaxSQLServerSecurity = ParseBoolean(sLaxSQLServerSecurity, False)

    ' The licensing override flag defaults to True if not specified.
    m_bLicensingTasks = ParseBoolean(sLicensingTasks, True)

    ' The event log flag defaults to True if not specified.
    m_eh.LogToWindowsEventLog = ParseBoolean(sUseEventLog, True)
    
    ' The flag defaults to False if not specified.
    m_bBackUpOnlyWhenRequired = ParseBoolean(sBackUpOnlyWhenRequired, False)

    ' These flags are modifed if and only if they are specified.
    If Len(sAllowUserToCancelDialog) = 0 Then
        sAllowUserToCancelDialog = sAllowUserToCancel
    End If
    If Len(sAllowUserToCancelMain) = 0 Then
        sAllowUserToCancelMain = sAllowUserToCancel
    End If
    m_bAllowUserToCancelDialog = ParseBoolean(sAllowUserToCancelDialog, m_bAllowUserToCancelDialog)
    m_bAllowUserToCancelMain = ParseBoolean(sAllowUserToCancelMain, m_bAllowUserToCancelMain)
    m_bAllowUserToIgnore = ParseBoolean(sAllowUserToIgnore, m_bAllowUserToIgnore)
    m_bBatchMode = ParseBoolean(sBatchMode, m_bBatchMode)
    m_bAllowUserToSkipBackup = ParseBoolean(sAllowUserToSkipBackup, m_bAllowUserToSkipBackup)
    
    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot read instructions from the command line.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DoInstall()

    Dim frmLocations1 As FLocations1
    Dim frmTasks As FTasks
    Dim bAnyCommandLineVersions As Boolean
    Dim oTask As CLogicalDatabase
    Dim vsName As Variant ' String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.DoInstall"
    End If

    ' If all values in this dialog were provided on the
    ' command line, then don't display it. Otherwise do so
    ' with the pre-provided values greyed out.
    If m_sInstallFolder = "" Or _
        m_sProductDataFolder = "" Then
        ' Show dialog.
        Set frmLocations1 = New FLocations1
        If Not frmLocations1.Dialog(FMain, _
            m_sInstallFolder, _
            m_sProductDataFolder) Then
            ' Cancel out of the application.
            FMain.Cancelled = True
            Err.Raise knErrAbort
        End If
        Set frmLocations1 = Nothing
        DoEvents
    End If

    ' Create the tasks collection.
    Set m_oTasks = New CLogicalDatabases

    ' Read the install configuration files.
    ReadInstallConfigFile
    ReadUpgradeOrderFile

    ' Parse the remainder of the command line.
    ParseCommandLine2

    ' Connect to the database, creating bits on-the-fly if they don't exist.
    ConnectToSiriusDatabase

    ' Create or update all data sources listed in the config file.
    CreatePMDAODataSource m_sPMDAODataSource
    For Each vsName In m_colODBCDSNs
        CreateODBCDSN vsName
    Next

    ' Find installed and installable products.
    FindInstallableProducts
    FindInstalledProducts

    ' If *any* versions were provided on the command line,
    ' then skip the next dialog. We only need to show it if
    ' none of the values are provided.
    bAnyCommandLineVersions = False
    For Each oTask In m_oTasks
        If oTask.CommandLineVersion <> "" Then
            bAnyCommandLineVersions = True
            Exit For
        End If
    Next

    If bAnyCommandLineVersions Then
        ' Select all the command line actions. If the command line
        ' specified the special version "(max)", then use the correct
        ' default maximum version.
        For Each oTask In m_oTasks
            If LCase$(oTask.CommandLineVersion) = ksParamMaxVersion Then
                oTask.Version = oTask.DefaultVersion
            Else
                oTask.Version = oTask.CommandLineVersion
            End If
        Next
        ' Validate all tasks. If silly values were passed on the
        ' command line, display the appropriate error message and
        ' abort the program.
        If Not ValidateAllTasks(False) Then
            ' Abort out of the application.
            Err.Raise knErrAbort
        End If
    Else
        ' Display tasks and ask user what to do. Refuse to continue
        ' until a completely valid set of tasks are entered. The user
        ' can abort this loop by cancelling the dialog box.
        Do
            Set frmTasks = New FTasks
            If Not frmTasks.Dialog(FMain, m_oTasks) Then
                ' Cancel out of the application.
                FMain.Cancelled = True
                Err.Raise knErrAbort
            End If
            Set frmTasks = Nothing
        Loop Until ValidateAllTasks(True)
    End If

    ' Carry out all selected tasks.
    CarryOutAllTasks

    ' Save values not already stored elsewhere, for use by other programs
    ' or as defaults next time an installer is run.
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "InstallFolder", m_sInstallFolder
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "ProductDataFolder", m_sProductDataFolder
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "BackupData", m_sBackupData
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "DataFilesFolder", m_sDataFilesFolder
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "LogFilesFolder", m_sLogFilesFolder
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "BackupFilesFolder", m_sBackupFilesFolder

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Error occurred while installing or upgrading the database.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Read settings from the install config file. When installing,
' this must be the first action taken on m_oTasks after it has been
' instantiated. When un-installing, m_oTasks is not modified.
Private Sub ReadInstallConfigFile()

    Dim sFile As String
    Dim sLogInfo As String
    Dim sXMLErrorText As String
    Dim docFile As MSXML2.DOMDocument
    Dim elmInstaller As MSXML2.IXMLDOMElement
    Dim elm As MSXML2.IXMLDOMElement

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ReadInstallConfigFile"
    End If

    FMain.ShowText "Reading install config file 1..."

    ' Get the file from the install folders on CD (unless overridden on the command line).
    If Len(m_sInstallConfigFile) <> 0 Then
        sFile = m_sInstallConfigFile
    Else
        sFile = AddSlash(m_sInstallFolder) & ksConfigFile
    End If

    ' Log the actual filespec being read.
    WriteToLog "Install config file is " & sFile

    ' Load the config file.
    sLogInfo = "Creating DOM object"
    Set docFile = New MSXML2.DOMDocument
    docFile.async = False
    sLogInfo = "Loading XML file"
    If Not docFile.Load(sFile) Then
        Err.Raise knErrBadConfigFile, ksErrSource, ksErrBadConfigFile & sFile
    End If
    sLogInfo = "Selecting root node"
    Set elmInstaller = docFile.selectSingleNode("Installer")

    ' Read the PMDAO data source to create/delete.
    sLogInfo = "Selecting PMDataSource node"
    Set elm = elmInstaller.selectSingleNode("PMDataSource")
    If Not elm Is Nothing Then
        m_sPMDAODataSource = Trim$(elm.Text)
    End If

    ' Safety check - this must always be specified.
    If Len(m_sPMDAODataSource) = 0 Then
        WarningDialog Database, "A valid data source must be specified in the install configuration file."
        Err.Raise knErrAbort
    End If

    ' When recovering, there is no need to read any more values.
    If m_bRecovering Then
        GoTo EX_NormalExit
    End If

    ' Read the ODBC DSNs to create/delete.
    Set m_colODBCDSNs = New Collection
    sLogInfo = "Selecting ODBCDSN nodes"
    For Each elm In elmInstaller.selectNodes("ODBCDSN")
        m_colODBCDSNs.Add elm.Text
    Next

    ' When un-installing, there is no need to read any more values.
    If m_bUnInstalling Then
        GoTo EX_NormalExit
    End If

    ' Read logical databases.
    sLogInfo = "Selecting Database nodes"
    For Each elm In elmInstaller.selectNodes("Database")
        m_oTasks.Add elm.getAttribute("Name"), elm.getAttribute("Abbr"), elm.Text
    Next

    ' Read general flags.
    sLogInfo = "Reading Installer attributes"
    m_bLicensingTasks = m_bLicensingTasks And ParseBoolean(elmInstaller.getAttribute("Licensed"), False)

    ' Read user interface defaults.
    sLogInfo = "Selecting UIDefaults node"
    Set elm = elmInstaller.selectSingleNode("UIDefaults")
    If Not elm Is Nothing Then
        m_sUIDefaultODBCDSN = elm.getAttribute("ODBCDSN")
        m_sUIDefaultServerName = elm.getAttribute("ServerName")
        m_sUIDefaultDatabaseName = elm.getAttribute("DatabaseName")
        m_sUIDefaultBackupData = elm.getAttribute("BackupData")
    End If

    ' If the server and database name fixed parameters have the value
    ' "(last)", then read the last known values from the registry. This
    ' enables installs after the first one to be done without user
    ' interaction. This must be done here, not in ParseCommandLine1(),
    ' because they require values from the install config file.
    If LCase$(m_sServerName) = ksParamLastValue Then
        m_sServerName = ReadFromPMDAODataSource(m_sPMDAODataSource, "Server")
    End If
    If LCase$(m_sDatabaseName) = ksParamLastValue Then
        m_sDatabaseName = ReadFromPMDAODataSource(m_sPMDAODataSource, "Database")
    End If

EX_NormalExit:
    Set elm = Nothing
    Set elmInstaller = Nothing
    Set docFile = Nothing
    Exit Sub

EH_Handler:
    sXMLErrorText = XMLErrorText(docFile)
    If Len(sXMLErrorText) <> 0 Then
        sLogInfo = sLogInfo & vbCrLf & sXMLErrorText
    End If
    Select Case ErrorDialog(Database, "Cannot read the install configuration file.", _
        sLogInfo:=sLogInfo)
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Read the upgrade order from the upgrade order file. If the file does not exist,
' do not produce an error but just create an empty collection.
Private Sub ReadUpgradeOrderFile()

    Dim sFile As String
    Dim hFile As Integer
    Dim iRow As Long
    Dim sRow As String
    Dim sName As String
    Dim sVersion As String
    Dim oTask As CLogicalDatabase

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ReadUpgradeOrderFile"
    End If

    FMain.ShowText "Reading install config file 2..."

    ' Get the file from the install folders on CD (unless overridden on the command line).
    If Len(m_sUpgradeOrderFile) <> 0 Then
        sFile = m_sUpgradeOrderFile
    Else
        sFile = AddSlash(m_sInstallFolder) & ksUpgradeOrderFile
    End If

    ' Create the collection to store this data.
    Set m_oUpgradeOrderRows = New CUpgradeOrderRows

    ' Check the file for existence first, for extra speed.
    If Not FileExists(sFile) Then Exit Sub

    ' Log the actual filespec being read.
    WriteToLog "Upgrade order file is " & sFile

    ' Open the file.
    hFile = FreeFile()
    Open sFile For Input Access Read Shared As hFile

    ' Read in the data. We validate the data for consistency as it's read in
    ' so that we don't have to worry about it later on.
    iRow = 0
    Do Until EOF(hFile)
        iRow = iRow + 1
        ' Read a line from the file and parse it.
        Line Input #hFile, sRow
        sName = ParseSep(sRow, sVersion, "\")
        sName = Trim$(sName)
        sVersion = Trim$(sVersion)
        ' If either the name or version is missing then throw a syntax error.
        If Len(sName) = 0 Or Len(sVersion) = 0 Then
            Err.Raise knErrUOFSyntax, ksErrSource, ksErrUOFSyntax & iRow
        End If
        ' Look up the name in the configured logical database collection.
        ' If it doesn't exist, skip the line silently. This enables us to
        ' copy upgrade order files from one install to another without
        ' worrying about exact contents.
        Set oTask = m_oTasks.Find(sName)
        If Not oTask Is Nothing Then
            ' Each version must be higher than the previous one for each unique
            ' logical database. If this is not true, throw an error. This also
            ' works nicely for non-numeric or other badly-formatted versions,
            ' because they will compare as if they were zero.
            If Not VersionGreaterThan(sVersion, oTask.MaxOrderVersion) Then
                Err.Raise knErrUOFOrder, ksErrSource, ksErrUOFOrder & iRow
            End If
            ' Store the version in memory and update the maximum version on the
            ' logical database.
            m_oUpgradeOrderRows.Add sName, sVersion
            oTask.MaxOrderVersion = sVersion
            Set oTask = Nothing
        End If
    Loop

EX_NormalExit:
    On Error Resume Next
    If hFile <> 0 Then
        Close hFile
    End If
    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot read the upgrade order configuration file.")
    Case erRetry
        Resume
    Case Else
        Resume EX_Abort
    End Select

EX_Abort:
    On Error Resume Next
    If hFile <> 0 Then
        Close hFile
    End If
    On Error GoTo 0
    Err.Raise knErrAbort

End Sub

' Finish parsing the remainder of the command line. This sub requires
' m_oTasks to be initialised with ReadInstallConfigFile() first.
Private Sub ParseCommandLine2()

    Dim sItem As String
    Dim bProduct As Boolean
    Dim oTask As CLogicalDatabase
    Dim bExists As Boolean
    Dim sName As String
    Dim sVersion As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ParseCommandLine2"
    End If

    FMain.ShowText "Reading task list..."

    ' Parsing of old and new command lines is subtly different.
    ' * In the new style, the logical database name and version are packed
    ' into one parameter value and must be sub-parsed.
    ' * In the old style, the logical database name and version are in separate
    ' parameters, and a boolean flag must be used to keep track of what we
    ' are reading.

    ' Un-recognised logical database names are automatically ignored by the
    ' logical database storage object, so we don't need to validate them here.

    If m_bNamedParameters Then
        Do
            sItem = Trim$(m_oParameters.ReadByName("ldv", bExists))
            If Not bExists Then
                Exit Do
            End If
            sName = ParseSep(sItem, sVersion, "\")
            Set oTask = m_oTasks.Find(Trim$(sName))
            If Not oTask Is Nothing Then
                oTask.CommandLineVersion = Trim$(sVersion)
                Set oTask = Nothing
            End If
        Loop
    Else
        bProduct = True
        Do
            sItem = Trim$(m_oParameters.ReadFirst(bExists))
            If Not bExists Then
                Exit Do
            End If
            If bProduct Then
                Set oTask = m_oTasks.Find(sItem)
            ElseIf Not oTask Is Nothing Then
                oTask.CommandLineVersion = sItem
            End If
            bProduct = Not bProduct
        Loop
        Set oTask = Nothing
    End If

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot read the list of databases to install.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub ConnectToSiriusDatabase()

    Dim frmLocations2 As FLocations2
    Dim bJustCreated As Boolean

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ConnectToSiriusDatabase"
    End If

    ' If all values in this dialog were provided on the
    ' command line, then don't display it. Otherwise do so
    ' with the pre-provided values greyed out.
    If m_sServerName = "" Or _
        m_sDatabaseName = "" Or _
        m_sBackupData = "" Then
ForcePromptUser:
        ' Show dialog.
        Set frmLocations2 = New FLocations2
        If Not frmLocations2.Dialog(FMain, _
            m_sServerName, _
            m_sDatabaseName, _
            m_sBackupData, _
            m_sPMDAODataSource, _
            m_sUIDefaultODBCDSN, _
            m_sUIDefaultServerName, _
            m_sUIDefaultDatabaseName, _
            m_sUIDefaultBackupData) Then
            ' Cancel out of the application.
            FMain.Cancelled = True
            Err.Raise knErrAbort
        End If
        Set frmLocations2 = Nothing
        DoEvents
    End If

    ' This procedure opens a valid connection to the Sirius database using the
    ' Sirius login. If either the login or the database need creating, it
    ' temporarily connects to the Master database using the Admin login to carry
    ' out that action, then immediately disconnects and tries again. This code
    ' ensures that the Sirius login does not actually need "create database"
    ' permissions, or indeed any permissions at all outside the role of db_owner
    ' within the database.

    bJustCreated = False
    Do
        ' Attempt to connect to the Sirius database.
        Select Case ConnectToSiriusDatabaseUsingSiriusLogin
        Case knTryAnotherServer
            ' Force the server name dialog to be re-shown even if the command line
            ' specified a hard-coded value. This allows the user to correct the problem
            ' if bad information was passed in.
            m_sServerName = ""
            GoTo ForcePromptUser
        Case knCreateSiriusLogin
            ' If the Sirius login is missing, create it.
            ConnectToMasterDatabaseUsingMasterLogin
            CreateSiriusLogin
            DisconnectFromDatabase
        Case knCreateSiriusDatabase
            ' If the Sirius database is missing, create it.
            If m_bLaxSQLServerSecurity Then
                ConnectToMasterDatabaseUsingSiriusLogin
            Else
                ConnectToMasterDatabaseUsingMasterLogin
            End If
            CreateSiriusDatabase
            OwnSiriusDatabase
            DisconnectFromDatabase
            bJustCreated = True
        Case Else ' knContinue
            ' Connection succeeded, carry on.
            Exit Do
        End Select
    Loop

    ' Backup the Sirius database only if:
    ' (a) it already existed, and (b) the user requested it.
    m_bBackUpRequired = False
    If Not bJustCreated Then
        If ParseBoolean(m_sBackupData, False) Then
            If m_bBackUpOnlyWhenRequired Then
                m_bBackUpRequired = True
            Else
                BackupSiriusDatabase
            End If
        End If
    End If

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Error occurred while connecting to the database.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Function ConnectToSiriusDatabaseUsingSiriusLogin() As EActions

    Dim sMessage As String
    Dim nButtons As EErrorButtons
    Dim bTryAnother As Boolean
    Dim bWindowsLogon As Boolean

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ConnectToSiriusDatabaseUsingSiriusLogin"
    End If

    FMain.ShowText "Connecting to the Pure database..."

    m_sCurrentLoginDisplayName = "the Pure login"

    'Connect using the windows logon.
    bWindowsLogon = True
    DatabaseConnect m_sServerName, m_sDatabaseName, False, True, "", ""

    ConnectToSiriusDatabaseUsingSiriusLogin = knContinue
    
    GoTo EX_NormalExit
    
ConnectUsingSiriusLogin:

    On Error GoTo EH_Handler
    
    'Connect using the sirius logon.
    bWindowsLogon = False
    DatabaseConnect m_sServerName, m_sDatabaseName, True, False, "", ""
    
    GoTo EX_NormalExit
	
ConnectUsingOtherUserLogin:

    On Error GoTo EH_Handler
   
    'Connect using the sirius logon.
    bWindowsLogon = False
    DatabaseConnect m_sServerName, m_sDatabaseName, False, False, m_sLoginName, m_sPassword
    
    GoTo EX_NormalExit
	
    
EX_NormalExit:
    Exit Function

EH_Handler:
    bTryAnother = False
    If Database.LastErrorWas(17, "08001") Then
        ' SQL Server does not exist or access denied. (-2147467259) (17) (08001)
        sMessage = "Cannot connect to the Pure database because " & _
            "the SQL Server instance does not exist, is not running, or is denying access."
        nButtons = ebRetryCancel
        bTryAnother = True
    ElseIf Database.LastErrorWas(4060, "42000") Then
        ' Cannot open database requested in login 'SIRIUS'. Login fails. (-2147467259) (4060) (42000)
        ConnectToSiriusDatabaseUsingSiriusLogin = knCreateSiriusDatabase
        Exit Function
    ElseIf bWindowsLogon Then
        Resume ConnectUsingSiriusLogin
	ElseIf bWindowsLogon = False Then
        Resume ConnectUsingOtherUserLogin	
    Else
        sMessage = "Cannot connect to the Pure database. " & _
            "Check that the correct SQL Server instance service is running on the specified computer."
        nButtons = ebRetryCancel
    End If
    Select Case ErrorDialog(Database, sMessage, nButtons:=nButtons)
    Case erRetry
        If bTryAnother Then
            ' Force the server name dialog to be re-shown even if the command line
            ' specified a hard-coded value. This allows the user to correct the problem
            ' if bad information was passed in.
            ConnectToSiriusDatabaseUsingSiriusLogin = knTryAnotherServer
            Exit Function
        Else
            Resume
        End If
    Case Else
        Err.Raise knErrAbort
    End Select

End Function

Private Sub ConnectToMasterDatabaseUsingSiriusLogin()

    On Error GoTo EH_Handler
    
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ConnectToMasterDatabaseUsingSiriusLogin"
    End If

    FMain.ShowText "Connecting to the Master database..."

    m_sCurrentLoginDisplayName = "the Pure login"
    'Connect using the windows logon.
    DatabaseConnect m_sServerName, ksMasterDatabaseName, False, True, "", ""
    
    GoTo EX_NormalExit
    
ConnectUsingSiriusLogin:

    On Error GoTo EH_Handler2
    
    m_sCurrentLoginDisplayName = "your current Windows login"
    'Connect using the Pure logon.
    DatabaseConnect m_sServerName, ksMasterDatabaseName, True, False, "", ""
    
    GoTo EX_NormalExit

EX_NormalExit:
    Exit Sub

EH_Handler:
    
    Resume ConnectUsingSiriusLogin
    
EH_Handler2:

    Select Case ErrorDialog(Database, "Cannot connect to the Master database. " & _
        "Check that the correct SQL Server instance service is running on the specified computer.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub ConnectToMasterDatabaseUsingMasterLogin()

    Dim bTrusted As Boolean
    Dim sUserName As String
    Dim sPassword As String
    Dim bOKPressed As Boolean

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ConnectToMasterDatabaseUsingMasterLogin"
    End If

    FMain.ShowText "Connecting to the Master database..."

    ' If lax security then try and connect using sa and blank pwd prior to prompting user
    If Not m_bLaxSQLServerSecurity Then
        GoTo ConnectUsingPromptedLogin
    End If

ConnectUsingSysAdminLogin:
    ' Attempt to connect using the SA login.
    m_sCurrentLoginDisplayName = "the ""sa"" login"

    DatabaseConnect m_sServerName, ksMasterDatabaseName, False, False, "sa", ""

    Exit Sub

ConnectUsingPromptedLogin:
    ' Attempt to connect using a login entered by the user.
    ' If the user pressed Cancel, then cancel the install.
    bOKPressed = GetLoginAccount(FMain, m_sServerName, bTrusted, sUserName, sPassword)
    If Not bOKPressed Then
        FMain.Cancelled = True
        Err.Raise knErrAbort
    End If
    If bTrusted Then
        m_sCurrentLoginDisplayName = "your current Windows login"
    Else
        m_sCurrentLoginDisplayName = "the """ & sUserName & """ login"
    End If

    DatabaseConnect m_sServerName, ksMasterDatabaseName, False, bTrusted, sUserName, sPassword

    Exit Sub

EH_Handler:
    If Database.LastErrorWas(18456, "42000") And Not m_bBatchMode Then
        ' Login failed for user 'SIRIUS'. (-2147217843) (18456) (42000)
        Resume ConnectUsingPromptedLogin
    End If
    Select Case ErrorDialog(Database, "Cannot connect to the Master database. " & _
        "Check that the correct SQL Server instance service is running on the specified computer.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DisconnectFromDatabase()

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.DisconnectFromDatabase"
    End If

    FMain.ShowText "Disconnecting from the database..."

    Database.Disconnect

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot disconnect from the database.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Disconnecting from database"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub BackupSiriusDatabase()

    Dim frmLocations4 As FLocations4
    Dim sBackupFileName As String
    Dim nCommandTimeout As Long
    Dim sMessage As String
    Dim nButtons As EErrorButtons
    Dim bTryAnother As Boolean

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.BackupSiriusDatabase"
    End If

    FMain.ShowText "Backing up the Pure database..."

    ' If all values in this dialog were provided on the
    ' command line, then don't display it. Otherwise do so
    ' with the pre-provided values greyed out.
    If m_sBackupFilesFolder = "" Then
ForcePromptUser:
        ' Show dialog.
        Set frmLocations4 = New FLocations4
        If Not frmLocations4.Dialog(FMain, _
            m_sBackupFilesFolder, _
            m_sServerName) Then
            ' Cancel out of the application.
            FMain.Cancelled = True
            Err.Raise knErrAbort
        End If
        Set frmLocations4 = Nothing
        DoEvents
    End If

    If m_sBackupFilesFolder = "" Then
        WriteToLog "Skipped Backup For Database " & m_sDatabaseName
        Exit Sub
    End If

    sBackupFileName = AddSlash(m_sBackupFilesFolder) & _
        m_sDatabaseName & " " & Format$(Now(), "yyyy-mm-dd hh-nn-ss") & ".dat"

    nCommandTimeout = Database.Connection.CommandTimeout
    If nCommandTimeout < knMinimumTimeoutForBackupRestore Then
        Database.Connection.CommandTimeout = knMinimumTimeoutForBackupRestore
    End If

    Database.Execute _
        "backup database " & ToSQLIdent(m_sDatabaseName) & " " & _
        "to disk = " & ToSQL(sBackupFileName)

    Database.Connection.CommandTimeout = nCommandTimeout

    ' Save the complete backup filespec for possible use by the recovery option.
    RegWrite HKEY_LOCAL_MACHINE, ksRegProject, "BackupFileName", sBackupFileName

    ' Log this fact to the caller.
    SetAppTasksDone knAppDatabaseBackup

    ' Log this fact to the log.
    WriteToLog "Database " & m_sDatabaseName & " backed up to " & sBackupFileName

    Exit Sub

EH_Handler:
    bTryAnother = False
    If DatabaseFirstErrorWas(3201, "42000") Then
        ' Cannot open backup device '****'. Device error or device off-line.
        sMessage = "Cannot back up the Pure database because " & _
            "the backup location is inaccessible or the backup file cannot be created."
        nButtons = ebRetryCancelIgnore
        bTryAnother = True
    Else
        sMessage = "Cannot back up the Pure database. " & _
            "Check that the backup location is correct, " & _
            "that sufficient time has been allowed for the operation to complete, and " & _
            "that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action."
        nButtons = ebRetryCancelIgnore
    End If
    Select Case ErrorDialog(Database, sMessage, nButtons:=nButtons)
    Case erRetry
        If bTryAnother Then
            ' Force the backup location dialog to be re-shown even if the command line
            ' specified a hard-coded value. This allows the user to correct the problem
            ' if bad information was passed in.
            Resume ForcePromptUser
        Else
            Resume
        End If
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Backing up database"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        m_bBackUpFailed = True
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub RestoreSiriusDatabase(ByVal sBackupFileName As String)

    Dim nCommandTimeout As Long

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.RestoreSiriusDatabase"
    End If

    FMain.ShowText "Restoring the Pure database..."

    nCommandTimeout = Database.Connection.CommandTimeout
    If nCommandTimeout < knMinimumTimeoutForBackupRestore Then
        Database.Connection.CommandTimeout = knMinimumTimeoutForBackupRestore
    End If

    Database.Execute _
        "restore database " & ToSQLIdent(m_sDatabaseName) & " " & _
        "from disk = " & ToSQL(sBackupFileName)

    Database.Connection.CommandTimeout = nCommandTimeout

    ' Log this fact.
    WriteToLog "Database " & m_sDatabaseName & " restored from " & sBackupFileName

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot restore the Pure database. " & _
        "Check that the restore location is correct, " & _
        "that sufficient time has been allowed for the operation to complete, and " & _
        "that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Restoring database"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CreateSiriusLogin()

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CreateSiriusLogin"
    End If

    FMain.ShowText "Creating the Pure login..."

    If ToIntegerFixed(DatabaseServerVersion()) >= 9 Then
        Database.Execute "create login " & ToSQLIdent(ksSALoginName) & " with " & _
            "password = " & ToSQL(ksSALoginPassword) & ", " & _
            "check_expiration = off, " & _
            "check_policy = off"
    Else
        Database.Execute "execute sp_addlogin " & _
            ToSQL(ksSALoginName) & ", " & _
            ToSQL(ksSALoginPassword)
    End If

    Database.Execute "execute sp_addsrvrolemember " & _
        ToSQL(ksSALoginName) & ", " & _
        ToSQL("sysadmin")

    ' Log this fact.
    WriteToLog "Login " & ksSALoginName & " created"

    Database.Execute "checkpoint"

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot create the Pure login. " & _
        "Check that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Creating " & ksSALoginName & " login"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DropSiriusLogin()

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.DropSiriusLogin"
    End If

    FMain.ShowText "Dropping the Pure login..."

    Database.Execute _
        "execute sp_droplogin " & ToSQL(ksSALoginName)

    ' Log this fact.
    WriteToLog "Login " & ksSALoginName & " dropped"

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot drop the Pure login. " & _
        "Check that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Dropping " & ksSALoginName & " login"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CreateSiriusDatabase()

    Dim frmLocations3 As FLocations3
    Dim sDataLogicalName As String
    Dim sLogLogicalName As String
    Dim sDataFileName As String
    Dim sLogFileName As String
    Dim sMessage As String
    Dim nButtons As EErrorButtons
    Dim bTryAnother As Boolean

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CreateSiriusDatabase"
    End If

    FMain.ShowText "Creating the Pure database..."

    ' If all values in this dialog were provided on the
    ' command line, then don't display it. Otherwise do so
    ' with the pre-provided values greyed out.
    If m_sDataFilesFolder = "" Or _
        m_sLogFilesFolder = "" Then
ForcePromptUser:
        ' Show dialog.
        Set frmLocations3 = New FLocations3
        If Not frmLocations3.Dialog(FMain, _
            m_sDataFilesFolder, _
            m_sLogFilesFolder, _
            m_sServerName) Then
            ' Cancel out of the application.
            FMain.Cancelled = True
            Err.Raise knErrAbort
        End If
        Set frmLocations3 = Nothing
        DoEvents
    End If

    sDataLogicalName = m_sDatabaseName & "_Data"
    sLogLogicalName = m_sDatabaseName & "_Log"
    sDataFileName = AddSlash(m_sDataFilesFolder) & sDataLogicalName & ".mdf"
    sLogFileName = AddSlash(m_sLogFilesFolder) & sLogLogicalName & ".ldf"

    Database.Execute _
        "create database " & ToSQLIdent(m_sDatabaseName) & " " & _
        "on (" & _
        "name = " & ToSQL(sDataLogicalName) & ", " & _
        "filename = " & ToSQL(sDataFileName) & ") " & _
        "log on (" & _
        "name = " & ToSQL(sLogLogicalName) & ", " & _
        "filename = " & ToSQL(sLogFileName) & ")"

    ' Log this fact.
    WriteToLog "Database " & m_sDatabaseName & " created"

    Database.Execute "checkpoint"

    Exit Sub

EH_Handler:
    bTryAnother = False
    If DatabaseFirstErrorWas(5105, "42000") Then
        ' Device activation error. The physical file name '****' may be incorrect.
        sMessage = "Cannot create the Pure database because " & _
            "the data or log location is inaccessible or the files cannot be created."
        nButtons = ebRetryCancel
        bTryAnother = True
    Else
        sMessage = "Cannot create the Pure database. " & _
            "Check that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action."
        nButtons = ebRetryCancelIgnore
    End If
    Select Case ErrorDialog(Database, sMessage, nButtons:=nButtons)
    Case erRetry
        If bTryAnother Then
            ' Force the data/log location dialog to be re-shown even if the command line
            ' specified hard-coded values. This allows the user to correct the problem
            ' if bad information was passed in.
            Resume ForcePromptUser
        Else
            Resume
        End If
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Creating " & m_sDatabaseName & " database"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DropSiriusDatabase()

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.DropSiriusDatabase"
    End If

    FMain.ShowText "Dropping the Pure database..."

    Database.Execute _
        "drop database " & ToSQLIdent(m_sDatabaseName)

    ' Log this fact.
    WriteToLog "Database " & m_sDatabaseName & " dropped"

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot drop the Pure database. " & _
        "Check that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Dropping " & m_sDatabaseName & " database"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub OwnSiriusDatabase()

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.OwnSiriusDatabase"
    End If

    FMain.ShowText "Transferring Pure database ownership..."

    Database.Execute "use " & ToSQLIdent(m_sDatabaseName)

    Database.Execute "execute sp_dropuser " & ToSQL(ksSALoginName)

    Database.Execute "execute sp_changedbowner " & ToSQL(ksSALoginName)

    ' Log this fact.
    WriteToLog "Database " & m_sDatabaseName & " ownership transferred"

    Exit Sub

EH_Handler:
    If Database.LastErrorWas(15008, "42000") Then
        ' User 'SIRIUS' does not exist in the current database.
        Resume Next
    ElseIf Database.LastErrorWas(15110, "42000") Then
        ' The proposed new database owner is already a user in the database.
        Resume Next
    End If
    Select Case ErrorDialog(Database, "Cannot transfer ownership of the Pure database. " & _
        "Check that " & m_sCurrentLoginDisplayName & " has sufficient security permissions to carry out this action.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Owning " & m_sDatabaseName & " database"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub FindInstallableProducts()

    Dim oFindProducts As CFileSearch
    Dim oFindVersions As CFileSearch
    Dim oTask As CLogicalDatabase

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.FindInstallableProducts"
    End If

    FMain.ShowText "Searching for installable products..."

    ' Find product folders in the install path.
    Set oFindProducts = New CFileSearch
    oFindProducts.ErrorOnPathNotFound = False
    oFindProducts.ErrorOnFileNotFound = False
    Set oFindVersions = New CFileSearch
    oFindVersions.ErrorOnPathNotFound = False
    oFindVersions.ErrorOnFileNotFound = False

    oFindProducts.FindFirst AddSlash(m_sInstallFolder) & "*.*"
    Do While oFindProducts.Searching
        If oFindProducts.IsFolder Then
            Set oTask = m_oTasks.Find(oFindProducts.Name)
            If Not oTask Is Nothing Then
                ' Found a recognised product folder.
                oTask.Folder = oFindProducts.Name
                ' Search for all "create" versions.
                oFindVersions.FindFirst AddSlash(oFindProducts.FullyQualifiedName) & ksFolderCreate & "\*.*"
                Do While oFindVersions.Searching
                    If oFindVersions.IsFolder Then
                        oTask.CreatableVersion = oFindVersions.Name
                    End If
                    oFindVersions.FindNext
                Loop
                ' Search for all "upgrade" versions.
                oFindVersions.FindFirst AddSlash(oFindProducts.FullyQualifiedName) & ksFolderUpgrade & "\*.*"
                Do While oFindVersions.Searching
                    If oFindVersions.IsFolder Then
                        oTask.UpgradableVersion = oFindVersions.Name
                    End If
                    oFindVersions.FindNext
                Loop
            End If
            Set oTask = Nothing
        End If
        oFindProducts.FindNext
    Loop

    Set oFindVersions = Nothing
    Set oFindProducts = Nothing

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot find installable products.")
    Case erRetry
        Resume
    Case Else
        Set oFindVersions = Nothing
        Set oFindProducts = Nothing
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub FindInstalledProducts()

    Dim rs As ADODB.Recordset
    Dim oTask As CLogicalDatabase
    Dim sCode As String
    Dim sVersion As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.FindInstalledProducts"
    End If

    FMain.ShowText "Searching for installed products..."

    ' Find rows in the PMProduct table.
    Set rs = Database.OpenRecordset("select name, version from PMLogicalDatabase")
    Do Until rs.EOF
        Set oTask = m_oTasks.Find(rs("name"))
        If Not oTask Is Nothing Then
            oTask.CurrentVersion = Trim$(ToString(rs("version")))
        End If
        Set oTask = Nothing
        rs.MoveNext
    Loop
    rs.Close
    Set rs = Nothing

    Exit Sub

EH_Handler:
    If Database.LastErrorWas(208, "42S02") Then
        ' Invalid object name 'PMLogicalDatabase'. (-2147217865) (208) (42S02)
        ' If the table doesn't exist, just ignore this code.
        Exit Sub
    End If
    Select Case ErrorDialog(Database, "Cannot find installed products.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Function ValidateAllTasks(ByVal bInteractive As Boolean) As Boolean

    Dim bAtLeastOneTaskToDo As Boolean
    Dim oTask As CLogicalDatabase
    Dim frmICCS As FICCS

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.ValidateAllTasks"
    End If
    ValidateAllTasks = False

    FMain.ShowText "Checking all selected tasks..."

    ' If there are no tasks, we cannot proceed.
    If m_oTasks.Count = 0 Then
        WarningDialog Database, "There are no products currently installed or available to be installed."
        Exit Function
    End If

    ' The action must be greater than the current version for all tasks,
    ' or we cannot proceed. Also, the action must actually exist in the
    ' list of installable versions (this checks for non-existent versions
    ' supplied on the command line).
    bAtLeastOneTaskToDo = False
    For Each oTask In m_oTasks
        If oTask.Version <> "" Then
            If Not VersionGreaterThan(oTask.Version, oTask.CurrentVersion) Then
                ' This error is expected in non-interactive mode
                ' and should not be displayed to the user.
                If bInteractive Then
                    WarningDialog Database, oTask.Description & " version " & oTask.Version & " cannot be upgraded because it is earlier than or equal to the current version."
                    Exit Function
                Else
                    WarningReport Database, oTask.Description & " version " & oTask.Version & " cannot be upgraded because it is earlier than or equal to the current version."
                    oTask.Version = "" ' don't do anything for this task
                End If
            ElseIf Not oTask.VersionCanBeInstalled() Then
                WarningDialog Database, oTask.Description & " version " & oTask.Version & " cannot be " & IIf(oTask.NeedsCreating, "created", "upgraded") & " because it is not present in the install set.", _
                    sLogInfo:="Folder not found: " & AddSlash(m_sInstallFolder) & _
                        oTask.Folder & "\" & _
                        IIf(oTask.NeedsCreating, ksFolderCreate, ksFolderUpgrade) & "\" & _
                        oTask.Version
                Exit Function
            End If
        End If
        If oTask.Version <> "" Then
            bAtLeastOneTaskToDo = True
        End If
    Next

    ' There must be at least one task to perform.
    If Not bAtLeastOneTaskToDo Then
        ' This error is expected in non-interactive mode
        ' and should not be displayed to the user.
        If bInteractive Then
            WarningDialog Database, "No installation tasks have been selected."
            Exit Function
        Else
            WarningReport Database, "No installation tasks have been selected, or all tasks are earlier than or equal to the current versions."
        End If
    End If

    ' If the Architecture logical database is defined then perform
    ' some additional tests.
    Set oTask = m_oTasks.Find(ksArchitectureName)
    m_sICCS = ""
    If Not oTask Is Nothing Then
        ' If the Architecture does not yet exist in the database, then
        ' the user must have selected a version to install, otherwise
        ' we cannot control installations.
        If oTask.NeedsCreating And oTask.Version = "" Then
            WarningDialog Database, "The Pure Architecture is required, but a version has not been selected for installation."
            Exit Function
        End If
        If m_bLicensingTasks Then
            ' If the Architecture does not yet exist in the database, then
            ' ask the user for their ICCS number. If the user cancels, then
            ' return False to bring back the previous dialog.
            If oTask.NeedsCreating Then
                Set frmICCS = New FICCS
                m_sICCS = frmICCS.Dialog(FMain)
                Set frmICCS = Nothing
                If m_sICCS = "" Then
                    Exit Function
                End If
            End If
        End If
    End If

    ' All tests have been passed.
    ValidateAllTasks = True
    Exit Function

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot validate the list of install tasks.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Function

Private Sub CarryOutAllTasks()

    Dim oCreateTasks As CLogicalDatabases
    Dim oUpgradeTasks As CLogicalDatabases
    Dim oTask As CLogicalDatabase
    Dim oRow As CUpgradeOrderRow
    Dim bBackup As Boolean
    Dim vsVersion As Variant ' String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CarryOutAllTasks"
    End If

    ' Split tasks into two separate lists of those that need creating
    ' and those that need upgrading.
    Set oCreateTasks = New CLogicalDatabases
    Set oUpgradeTasks = New CLogicalDatabases
    For Each oTask In m_oTasks
        If oTask.Version <> "" Then
            If oTask.NeedsCreating Then
                oCreateTasks.Append oTask
            Else
                oUpgradeTasks.Append oTask
            End If
        End If
    Next
    
    bBackup = False
    If oCreateTasks.Count > 0 Then
        bBackup = True
    ElseIf oUpgradeTasks.Count > 0 Then
        If m_oUpgradeOrderRows.Count = 0 Then
            For Each oTask In oUpgradeTasks
                For Each vsVersion In oTask.InstallableVersions
                    If VersionGreaterThan(vsVersion, oTask.CurrentVersion) And _
                        Not VersionGreaterThan(vsVersion, oTask.Version) Then
                        bBackup = True
                        Exit For
                    End If
                Next
                If bBackup Then
                    Exit For
                End If
            Next
        Else
            For Each oRow In m_oUpgradeOrderRows
                Set oTask = oUpgradeTasks.Find(oRow.Name)
                If Not oTask Is Nothing Then
                    If VersionGreaterThan(oRow.Version, oTask.CurrentVersion) And _
                        Not VersionGreaterThan(oRow.Version, oTask.Version) Then
                        bBackup = True
                        Exit For
                    End If
                End If
            Next
        End If
    End If
     
    'If user said to backup and we are making changes, then backup
    If bBackup And m_bBackUpRequired Then
        BackupSiriusDatabase
    End If

    ' Turn on "script running" mode.
    FMain.ScriptInitialise
    FMain.ShowText "Performing selected tasks..."

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' All before tasks.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    For Each oTask In m_oTasks
        BeforeProduct oTask
    Next

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' All create tasks.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    If oCreateTasks.Count > 0 Then
        ' Run all create scripts in the correct order.
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Tables"
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "ForeignKeys"
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Views"
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Procedures"
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Wrappers"
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Triggers"
        Next
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Data"
        Next
        If m_sProductDataFolder <> "" And m_sProductDataFolder <> ksParamEmptyValue Then
            For Each oTask In oCreateTasks
                CreateProductByFolder oTask, "Data\" & m_sProductDataFolder
            Next
        End If
        For Each oTask In oCreateTasks
            CreateProductByFolder oTask, "Patches"
        Next

        ' Update the database with the new versions.
        For Each oTask In oCreateTasks
            CreateProductFinished oTask
        Next

        If m_bLicensingTasks Then
            ' If we have just created an Architecture database,
            ' then perform additional licensing actions.
            If m_nAppTasksDone And knAppCreatedArchitecture Then
                CreateArchitectureLicensingStuff
            End If
        End If

        Set oCreateTasks = Nothing
    End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' All upgrade tasks.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    If oUpgradeTasks.Count > 0 Then
        ' If an upgrade order control file was included in the install set,
        ' then run all upgrades strictly in that order. Otherwise, fall back
        ' on the previous method of upgrading each logical database in turn.
        If m_oUpgradeOrderRows.Count = 0 Then
            For Each oTask In oUpgradeTasks
                UpgradeProduct oTask
            Next
        Else
            For Each oRow In m_oUpgradeOrderRows
                Set oTask = oUpgradeTasks.Find(oRow.Name)
                ' Only do the check if we have a valid task.
                If Not oTask Is Nothing Then
                    If VersionGreaterThan(oRow.Version, oTask.CurrentVersion) And _
                        Not VersionGreaterThan(oRow.Version, oTask.Version) Then
                        UpgradeProductVersion oTask, oRow.Version
                    End If
                    Set oTask = Nothing
                End If
            Next
        End If

        Set oUpgradeTasks = Nothing
    End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' All after tasks.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    For Each oTask In m_oTasks
        AfterProduct oTask
    Next

    ' Empty the stored procedure cache.
    Set m_prcSetLogicalDatabaseVersion = Nothing

    ' Turn off "script running" mode.
    FMain.ShowText "Finished selected tasks."
    FMain.ScriptTerminate

    ' Check the ICCS number for validity if the database is licensed.
    If m_bLicensingTasks Then
        CheckICCSNumber
    End If

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot install or upgrade the selected products.")
    Case erRetry
        Resume
    Case Else
        Resume EX_CarryOutAllTasks
    End Select

EX_CarryOutAllTasks:
    On Error Resume Next

    ' Empty the stored procedure cache.
    Set m_prcSetLogicalDatabaseVersion = Nothing

    ' Turn off "script running" mode.
    FMain.ShowText "Aborting..."
    FMain.ScriptTerminate

    On Error GoTo 0
    Err.Raise knErrAbort

End Sub

Private Sub AfterProduct(ByVal oTask As CLogicalDatabase)

    Dim sFolder As String
    Dim sFile As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.AfterProduct"
    End If

    ' If this folder is not defined (which it may not be for a
    ' particular product or logical database), then just skip
    ' over it.
    If oTask.Folder = "" Then
        Exit Sub
    End If

    FMain.ShowProduct oTask.Description & " (post-processing)"

    ' Construct the fully-qualified folder to execute.
    sFolder = AddSlash(m_sInstallFolder) & _
        oTask.Folder & "\" & _
        ksFolderAfter

    ' If this folder doesn't exist (which it may not do for a
    ' particular product or logical database), then just skip
    ' over it.
    If Not FolderExists(sFolder) Then
        Exit Sub
    End If

    ' Either simply execute all files in the folder, or if there
    ' is a "master script file", execute that instead. This will
    ' allow the user to define an explicit order of running scripts
    ' if they so choose.
    sFile = AddSlash(sFolder) & ksMasterScriptFile
    If Not FileExists(sFile) Then
        sFile = AddSlash(sFolder) & "*.sql"
    End If
    FMain.ScriptRun sFile, oTask.CurrentVersion, "", True

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot install " & oTask.Description & " post-processing.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub BeforeProduct(ByVal oTask As CLogicalDatabase)

    Dim sFolder As String
    Dim sFile As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.BeforeProduct"
    End If

    ' If this folder is not defined (which it may not be for a
    ' particular product or logical database), then just skip
    ' over it.
    If oTask.Folder = "" Then
        Exit Sub
    End If

    FMain.ShowProduct oTask.Description & " (pre-processing)"

    ' Construct the fully-qualified folder to execute.
    sFolder = AddSlash(m_sInstallFolder) & _
        oTask.Folder & "\" & _
        ksFolderBefore

    ' If this folder doesn't exist (which it may not do for a
    ' particular product or logical database), then just skip
    ' over it.
    If Not FolderExists(sFolder) Then
        Exit Sub
    End If

    ' Either simply execute all files in the folder, or if there
    ' is a "master script file", execute that instead. This will
    ' allow the user to define an explicit order of running scripts
    ' if they so choose.
    sFile = AddSlash(sFolder) & ksMasterScriptFile
    If Not FileExists(sFile) Then
        sFile = AddSlash(sFolder) & "*.sql"
    End If
    FMain.ScriptRun sFile, oTask.CurrentVersion, "", True

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot install " & oTask.Description & " pre-processing.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CreateProductByFolder(ByVal oTask As CLogicalDatabase, _
    Optional ByVal sSubFolder As String = "")

    Dim sFolder As String
    Dim sFile As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CreateProductByFolder"
    End If

    FMain.ShowProduct oTask.Description & " " & oTask.Version

    ' Construct the fully-qualified folder to execute.
    sFolder = AddSlash(m_sInstallFolder) & _
        oTask.Folder & "\" & _
        ksFolderCreate & "\" & _
        oTask.Version
    If sSubFolder <> "" Then
        sFolder = sFolder & "\" & sSubFolder
    End If

    ' If this folder doesn't exist (which it may not do for a
    ' particular product or logical database), then just skip
    ' over it.
    If Not FolderExists(sFolder) Then
        Exit Sub
    End If

    ' Either simply execute all files in the folder, or if there
    ' is a "master script file", execute that instead. This will
    ' allow the user to define an explicit order of running scripts
    ' if they so choose.
    sFile = AddSlash(sFolder) & ksMasterScriptFile
    If Not FileExists(sFile) Then
        sFile = AddSlash(sFolder) & "*.sql"
    End If
    FMain.ScriptRun sFile, oTask.CurrentVersion, oTask.Version, True

    ' If we are creating an Architecture database, then set the
    ' relevant flag in Tasks Done.
    If LCase$(oTask.Name) = ksArchitectureName Then
        SetAppTasksDone knAppCreatedArchitecture
    End If

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot install part of " & oTask.Description & " " & oTask.Version & ".")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CreateProductFinished(ByVal oTask As CLogicalDatabase)

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CreateProductFinished"
    End If

    FMain.ShowProduct oTask.Description & " " & oTask.Version

    ' Write the new version to the product table.
    SetLogicalDatabaseVersion oTask.Name, oTask.Version

    ' Log this fact.
    WriteToLog "Logical database " & oTask.Name & " created at version " & oTask.Version

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot finish installing " & oTask.Description & " " & oTask.Version & ".")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CreateArchitectureLicensingStuff()

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CreateArchitectureLicensingStuff"
    End If

    FMain.ShowProduct "Licences"

    ' Create the procedures that return the ICCS number.
    Database.Execute "set quoted_identifier on set ansi_nulls on"
    Database.Execute "execute DDLDropProcedure " & ToSQL("sp_pm_iccs")
    Database.Execute "execute DDLDropProcedure " & ToSQL("spu_pm_iccs")
    Database.Execute "create procedure spu_pm_iccs @ICCS char(4) output as select @ICCS = " & ToSQL(m_sICCS)
    Database.Execute "create procedure sp_pm_iccs @ICCS char(4) output as execute spu_pm_iccs @ICCS output"

    ' Log this fact.
    WriteToLog "ICCS procedures created (" & m_sICCS & ")"

    ' Add this computer name to the PMSystem table.
    Database.Execute "execute spu_pmsystem_add " & _
        ToSQL(GetWindowsComputerName()) & ", " & _
        ToSQL(0) & ", " & _
        ToSQL("")

    ' Log this fact.
    WriteToLog "PMSystem table updated"

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot carry out product licensing tasks.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Creating ICCS procs or adding to PMSystem table"
        SetAppTasksDone knAppErrorIgnored
        Resume Next
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub UpgradeProduct(ByVal oTask As CLogicalDatabase)

    Const kiFirstItemInList = 1

    Dim colVersionsInOrder As Collection
    Dim vsVersion As Variant ' String
    Dim sVersionFrom As String
    Dim sVersionTo As String
    Dim bFirstItem As Boolean

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.UpgradeProduct"
    End If

    ' The product exists in the database already.
    ' Upgrade to each intermediate version in turn until the
    ' target version is reached.
    Set colVersionsInOrder = New Collection

    ' Reverse-sort the relevant portion of the installable
    ' versions collection into a new collection.
    sVersionFrom = oTask.CurrentVersion
    sVersionTo = oTask.Version
    bFirstItem = True
    For Each vsVersion In oTask.InstallableVersions
        If VersionGreaterThan(vsVersion, sVersionFrom) And _
            Not VersionGreaterThan(vsVersion, sVersionTo) Then
            If bFirstItem Then
                colVersionsInOrder.Add Item:=vsVersion
                bFirstItem = False
            Else
                colVersionsInOrder.Add Item:=vsVersion, _
                    Before:=kiFirstItemInList
            End If
        End If
    Next

    ' Now just execute each one in order.
    For Each vsVersion In colVersionsInOrder
        UpgradeProductVersion oTask, vsVersion
    Next

    Set colVersionsInOrder = Nothing

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot upgrade " & oTask.Description & ".")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub UpgradeProductVersion(ByVal oTask As CLogicalDatabase, _
    ByVal sVersion As String)

    Dim sFolder As String
    Dim sFile As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.UpgradeProductVersion"
    End If

    FMain.ShowProduct oTask.Description & " " & sVersion

    ' Construct the fully-qualified base folder.
    sFolder = AddSlash(m_sInstallFolder) & _
        oTask.Folder & "\" & _
        ksFolderUpgrade & "\" & _
        sVersion

    ' Either simply execute all files in the folder, or if there
    ' is a "master script file", execute that instead. This will
    ' allow the user to define an explicit order of running scripts
    ' if they so choose.
    sFile = AddSlash(sFolder) & ksMasterScriptFile
    If Not FileExists(sFile) Then
        sFile = AddSlash(sFolder) & "*.sql"
    End If
    FMain.ScriptRun sFile, oTask.CurrentVersion, sVersion, True

    ' Write the new version to the product table.
    SetLogicalDatabaseVersion oTask.Name, sVersion

    ' Update the current version property in memory, so that the calling code
    ' doesn't have to hit the database again if they want to re-read it.
    oTask.CurrentVersion = sVersion

    ' Log this fact.
    WriteToLog "Logical database " & oTask.Name & " upgraded to version " & sVersion

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot upgrade " & oTask.Description & " to version " & sVersion & ".")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CheckICCSNumber()

    Dim prc As ADODB.Command

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.CheckICCSNumber"
    End If

    FMain.ShowProduct "Licences"

    ' Execute the ICCS procedure to read the number.
    Set prc = Database.NewCommand("spu_pm_iccs", True)
    prc.Parameters.Append prc.CreateParameter("ICCS", adChar, adParamOutput, 4)
    prc.Execute Options:=adExecuteNoRecords

    ' If it's the internal ICCS number, flag its use to the calling program.
    If ToString(prc.Parameters("ICCS")) = ksInternalICCS Then
        SetAppTasksDone knAppInternalICCS
    End If

    Exit Sub

EH_Handler:
    '' If error is "could not find stored procedure", then obviously
    '' the ICCS number is not the internal one, so just ignore it.
    'If Database.LastErrorWas(2812, "42000") Then
    '    WriteToLog "ICCS procedure does not exist in database"
    '    Exit Sub
    'End If
    Select Case ErrorDialog(Database, "Cannot carry out product licensing checks.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: Reading from ICCS procedure"
        SetAppTasksDone knAppErrorIgnored
        Exit Sub
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub SetLogicalDatabaseVersion(ByVal sName As String, ByVal sVersion As String)

    Dim prc As ADODB.Command
    Dim nReturn As Long

    ' Write the new version to the product table. For safety, check
    ' the return value from the stored proc and raise an error if it's
    ' nonzero. This is to handle the potential rare case of an error
    ' occurring but ADO not raising it.
    If m_prcSetLogicalDatabaseVersion Is Nothing Then
        Set prc = Database.NewCommand("spu_PM_SetLogicalDatabaseVersion", True)
        With prc
            .Parameters.Append .CreateParameter("nReturn", adInteger, adParamReturnValue)
            .Parameters.Append .CreateParameter("sName", adVarChar, adParamInput, 30)
            .Parameters.Append .CreateParameter("sVersion", adVarChar, adParamInput, 30)
        End With
        Set m_prcSetLogicalDatabaseVersion = prc
    End If
    m_prcSetLogicalDatabaseVersion("sName") = sName
    m_prcSetLogicalDatabaseVersion("sVersion") = sVersion
    m_prcSetLogicalDatabaseVersion.Execute Options:=adExecuteNoRecords
    nReturn = ToLong(m_prcSetLogicalDatabaseVersion("nReturn"))
    If nReturn <> 0 Then
        Err.Raise nReturn Or vbObjectError, "SQL Server", "Unknown database error " & nReturn & " occurred."
    End If

End Sub

Private Sub DoRecover()

    Dim sMessage As String
    Dim sBackupFileName As String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.DoRecover"
    End If

    sMessage = "Error occurred while recovering the database."

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Read the install config file first.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ReadInstallConfigFile

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Read registry settings saved during installation.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    m_sServerName = ReadFromPMDAODataSource(m_sPMDAODataSource, "Server")
    m_sDatabaseName = ReadFromPMDAODataSource(m_sPMDAODataSource, "Database")
    sBackupFileName = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "BackupFileName", "")

    If m_sServerName = "" Or _
        m_sDatabaseName = "" Or _
        sBackupFileName = "" Then
        WarningDialog Database, "Cannot find settings from when the database was last installed on this machine. This recovery cannot continue."
        Err.Raise knErrAbort
    End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Restore the Pure database.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    If m_bLaxSQLServerSecurity Then
        ConnectToMasterDatabaseUsingSiriusLogin
    Else
        ConnectToMasterDatabaseUsingMasterLogin
    End If
    RestoreSiriusDatabase sBackupFileName
    DisconnectFromDatabase

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: A recovery task"
        SetAppTasksDone knAppErrorIgnored
        Resume Next
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub DoUnInstall()

    Dim sMessage As String
    Dim vsName As Variant ' String

    On Error GoTo EH_Handler
    If DebugLogging Then
        WriteToLog "DEBUG: MMain.DoUnInstall"
    End If

    sMessage = "Error occurred while un-installing the database."

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Read the install config file first.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ReadInstallConfigFile

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Read registry settings saved during installation.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    m_sServerName = ReadFromPMDAODataSource(m_sPMDAODataSource, "Server")
    m_sDatabaseName = ReadFromPMDAODataSource(m_sPMDAODataSource, "Database")
    m_sBackupFilesFolder = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "BackupFilesFolder", "")

    If m_sServerName = "" Or _
        m_sDatabaseName = "" Or _
        m_sBackupFilesFolder = "" Then
        WarningDialog Database, "Cannot find settings from when the database was last installed on this machine. This un-install cannot continue."
        Err.Raise knErrAbort
    End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Back up the Pure database first (for safety),
    ' then drop everything that was originally created.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ConnectToMasterDatabaseUsingMasterLogin
    BackupSiriusDatabase
    DropSiriusDatabase
    DropSiriusLogin
    DisconnectFromDatabase

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Delete all data sources listed in the config file.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    FMain.ShowText "Removing data source definitions..."
    sMessage = "Cannot remove one or more data source definitions."

    DeletePMDAODataSource m_sPMDAODataSource
    For Each vsName In m_colODBCDSNs
        DeleteODBCDSN vsName
    Next

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, sMessage, nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        ' If the user ignored this error, log that fact to the install log.
        WriteToLog "ERROR IGNORED: An un-install task"
        SetAppTasksDone knAppErrorIgnored
        Resume Next
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

Private Sub CreatePMDAODataSource(ByVal sName As String)

    Dim sRegFolder As String

    sRegFolder = ksRegPMDAODataSources & "\" & sName
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Provider", ksOLEDBProvider
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Server", m_sServerName
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Database", m_sDatabaseName

End Sub

Private Sub DeletePMDAODataSource(ByVal sName As String)

    RegFolderDelete HKEY_LOCAL_MACHINE, ksRegPMDAODataSources, sName

End Sub

Public Function ReadFromPMDAODataSource(ByVal sName As String, ByVal sAttribute As String) As String

    Dim sRegFolder As String

    sRegFolder = ksRegPMDAODataSources & "\" & sName
    ReadFromPMDAODataSource = RegRead(HKEY_LOCAL_MACHINE, sRegFolder, sAttribute, "")

End Function

Private Sub CreateODBCDSN(ByVal sDSN As String)

    Const ksDescription = "SSP Ltd - DO NOT MODIFY"
    Const ksDriverName = "SQL Server"

    Dim sRegFolder As String

    ' Get the filespec of the driver DLL if we have not already done so.
    If m_sODBCDriverFile = "" Then
        m_sODBCDriverFile = RegRead(HKEY_LOCAL_MACHINE, ksRegODBCDrivers & "\" & ksDriverName, "Driver", "")
        If m_sODBCDriverFile = "" Then
            Err.Raise knErrODBCDriverNotFound, ksErrSource, ksErrODBCDriverNotFound
        End If
    End If

    sRegFolder = ksRegODBCDSNs & "\" & sDSN
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "AnsiNPW", "Yes"
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "AutoTranslate", "Yes"
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Database", m_sDatabaseName
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Description", ksDescription
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Driver", m_sODBCDriverFile
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "QuotedId", "Yes"
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Regional", "No"
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "Server", m_sServerName
    RegWrite HKEY_LOCAL_MACHINE, sRegFolder, "UseProcForPrepare", "0"

    RegWrite HKEY_LOCAL_MACHINE, ksRegODBCDSNList, sDSN, ksDriverName

End Sub

Private Sub DeleteODBCDSN(ByVal sDSN As String)

    RegDelete HKEY_LOCAL_MACHINE, ksRegODBCDSNList, sDSN

    RegFolderDelete HKEY_LOCAL_MACHINE, ksRegODBCDSNs, sDSN

End Sub

Public Function ReadFromODBCDSN(ByVal sDSN As String, ByVal sAttribute As String) As String

    Dim sRegFolder As String

    sRegFolder = ksRegODBCDSNs & "\" & sDSN
    ReadFromODBCDSN = RegRead(HKEY_LOCAL_MACHINE, sRegFolder, sAttribute, "")

End Function

' Connect to a specified database using the specified login credentials.
Private Sub DatabaseConnect( _
    ByVal sServerName As String, _
    ByVal sDatabaseName As String, _
    ByVal bSiriusLogin As Boolean, _
    ByVal bTrusted As Boolean, _
    ByVal sUserName As String, _
    ByVal sPassword As String)

    Const knDAMSetupdb = knDAMUnknown Or knDAMDontAssumeDataStructures Or knDAMDontTrapConnectionErrors
    Const ksOLEDBFlags = "OLE DB Services=-1"

    Dim sOLEDBConnect As String

    sOLEDBConnect = "Provider=" & ksOLEDBProvider & _
        ";Data Source=" & sServerName & _
        ";Initial Catalog=" & sDatabaseName
    If bSiriusLogin Then
        sOLEDBConnect = sOLEDBConnect & _
            ";User ID=" & ksSALoginName & _
            ";Password=" & ksSALoginPassword
    ElseIf bTrusted Then
        sOLEDBConnect = sOLEDBConnect & _
            ";Trusted_Connection=Yes"
    Else
        sOLEDBConnect = sOLEDBConnect & _
            ";User ID=" & sUserName & _
            ";Password=" & sPassword
    End If
    sOLEDBConnect = sOLEDBConnect & _
        ";" & ksOLEDBFlags

    Database.ConnectV2 knDAMSetupdb, sOLEDBConnect

End Sub

Private Function DatabaseFirstErrorWas(ByVal nNativeError As Long, ByVal sSQLState As String) As Boolean

    DatabaseFirstErrorWas = False
    If Err.Number = knErrAbort Then Exit Function
    If Database.Connection Is Nothing Then Exit Function
    If Database.Connection.Errors Is Nothing Then Exit Function

    With Database.Connection.Errors
        If .Count > 0 Then
            With .Item(0)
                If .NativeError = nNativeError And .SQLState = sSQLState Then
                    DatabaseFirstErrorWas = True
                End If
            End With
        End If
    End With

End Function

Private Function DatabaseServerVersion() As String

    Dim rs As ADODB.Recordset
    Dim sVersion As String

    Set rs = Database.OpenRecordsetDisconnected("select @@version")
    sVersion = rs(0)
    rs.Close
    ParseSep sVersion, sVersion, " - "
    DatabaseServerVersion = ParseSep(LTrim$(sVersion), "", " ")

End Function

' Writes a line of text to the install log file. This does not store errors
' as such, but a log of exactly what actions were carried out.
Public Sub WriteToLog(ByVal sLine As String)

    Dim hLog As Integer

    ' If writing to the log failed last time, we must not attempt it again,
    ' otherwise the program can never actually close down after an error of
    ' this nature!
    If m_sInstallLogFile = "" Then
        Exit Sub
    End If

    On Error GoTo EH_Handler

    hLog = FreeFile()
    Open m_sInstallLogFile For Append Access Write Lock Write As hLog

    Print #hLog, sLine

EX_NormalExit:
    On Error Resume Next
    If hLog <> 0 Then
        Close hLog
    End If
    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Database, "Cannot write to the install log file.", _
        sLogInfo:="File: " & m_sInstallLogFile)
    Case erRetry
        Resume
    Case Else
        Resume EX_Abort
    End Select

EX_Abort:
    On Error Resume Next
    If hLog <> 0 Then
        Close hLog
    End If

    ' If writing to the log failed, we must not attempt it again (see above).
    m_sInstallLogFile = ""

    On Error GoTo 0
    Err.Raise knErrAbort

End Sub

Public Function ErrorDialog(ByVal db As CDatabase, _
    ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal nButtons As EErrorButtons = ebRetryCancel, _
    Optional ByVal bCritical As Boolean = False, _
    Optional ByVal sLogInfo As String = "") As EErrorResults

    Dim nErrNumber As Long
    Dim sErrSource As String
    Dim sErrDescription As String

    nErrNumber = Err.Number
    sErrSource = Err.Source
    sErrDescription = Err.Description

    ' Go via the database if possible so we can access the ADO errors collection.
    If Not db Is Nothing Then
         ErrorDialog = db.ErrorHandlerDialog(sMessage, _
            nErrNumber:=nErrNumber, _
            sErrSource:=sErrSource, _
            sErrDescription:=sErrDescription, _
            sTitle:=sTitle, _
            nButtons:=nButtons, _
            bCritical:=bCritical, _
            sLogInfo:=sLogInfo)
    Else
        ErrorDialog = m_eh.DialogV2(sMessage, _
            nErrNumber:=nErrNumber, _
            sErrSource:=sErrSource, _
            sErrDescription:=sErrDescription, _
            sTitle:=sTitle, _
            nButtons:=nButtons, _
            bCritical:=bCritical, _
            sLogInfo:=sLogInfo)
    End If

End Function

Public Sub ErrorReport(ByVal db As CDatabase, _
    ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    Dim nErrNumber As Long
    Dim sErrSource As String
    Dim sErrDescription As String

    nErrNumber = Err.Number
    sErrSource = Err.Source
    sErrDescription = Err.Description

    ' Go via the database if possible so we can access the ADO errors collection.
    If Not db Is Nothing Then
        db.ErrorHandlerReport sMessage, _
            nErrNumber:=nErrNumber, _
            sErrSource:=sErrSource, _
            sErrDescription:=sErrDescription, _
            sTitle:=sTitle, _
            sLogInfo:=sLogInfo
    Else
        m_eh.ReportV2 sMessage, _
            nErrNumber:=nErrNumber, _
            sErrSource:=sErrSource, _
            sErrDescription:=sErrDescription, _
            sTitle:=sTitle, _
            sLogInfo:=sLogInfo
    End If

End Sub

Public Sub WarningDialog(ByVal db As CDatabase, _
    ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    If Not db Is Nothing Then
         db.ErrorHandlerDialog sMessage, _
            sTitle:=sTitle, _
            nButtons:=ebOK, _
            sLogInfo:=sLogInfo
    Else
        m_eh.DialogV2 sMessage, _
            sTitle:=sTitle, _
            nButtons:=ebOK, _
            sLogInfo:=sLogInfo
    End If

End Sub

Public Sub WarningReport(ByVal db As CDatabase, _
    ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    If Not db Is Nothing Then
        db.ErrorHandlerReport sMessage, _
            sTitle:=sTitle, _
            sLogInfo:=sLogInfo
    Else
        m_eh.ReportV2 sMessage, _
            sTitle:=sTitle, _
            sLogInfo:=sLogInfo
    End If

End Sub

Private Function ToSQLIdent(ByVal sName As String) As String

    ToSQLIdent = "[" & Replace$(sName, "]", "]]") & "]"

End Function
