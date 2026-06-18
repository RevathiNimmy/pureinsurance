VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CDatabase"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    A connection to a Swift database
' Shared:   MultiUse, Yes (RESTRICTED)
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
' This class recognises a conditional compilation constant
' to allow it to be shared into the setupdb project without
' pulling in any unnecessary code.
' * SiriusDatabaseInstaller = 1
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Enumerations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' These are all the database "types" that the Swift Architecture
' supports. They control things like:
' * URN generation
' * Security module & user formats
' * OLEDB connection method

Public Enum EDataAccessMethods
    ' The actual access methods.
    knDAMUnknown = 0
    knDAMSwift5Original = 1
    knDAMSwift6Original = 2
    knDAMSwift6Version1 = 3
    knDAMSwift5Version1 = 4
    knDAMSiriusOriginal = 5
    knDAMSwift6Version2 = 6
    knDAMSiriusDirect = 7
    knDAMSwiftReplication = 8
    knDAMSwiftInteractive = 9
    ' Bit mask to remove optional flags.
    knDAMMaskFlags = &HFFFF&
    ' Optional flags.
    knDAMDontTrapConnectionErrors = &H80000000
    knDAMDontAssumeDataStructures = &H40000000
End Enum

' Return values from the DBMSType property.
Public Enum EDBMSTypes
    knDBMSUnknown
    knDBMSMicrosoftSQLServer
    knDBMSSybaseAdaptiveServer
    knDBMSSybaseSQLAnywhere
    knDBMSMicrosoftJet
    knDBMSTranSoftCobol
End Enum

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private Declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' These constants would normally be in the MMain module.
' They have been moved here to facilitate sharing this code
' module with the setupdb project. If you wish to use these
' constants in another module within the iSWDatabase project,
' please see Chris Hayter first.
Private Const ksRegPathProject = ksRegPathSwift & "\Database"
Private Const ksRegPathMainReplicationDB = ksRegPathSwift & "\Database\Replication"

#Const knDAMUnknown = 0
#Const knDAMSwift5Original = 0
#Const knDAMSwift6Original = 0
#Const knDAMSwift6Version1 = 0
#Const knDAMSwift5Version1 = 0
#Const knDAMSiriusOriginal = 0
#Const knDAMSwift6Version2 = 0
#Const knDAMSwiftReplication = 0
#Const knDAMSiriusDirect = 0
#Const knDAMMaskFlags = 0
#Const knDAMDontTrapConnectionErrors = 0
#Const knDAMDontAssumeDataStructures = 0

#Const knDBMSUnknown = 0
#Const knDBMSMicrosoftSQLServer = 0
#Const knDBMSSybaseAdaptiveServer = 0
#Const knDBMSSybaseSQLAnywhere = 0
#Const knDBMSMicrosoftJet = 0
#Const knDBMSTranSoftCobol = 0

' General class properties.
Private m_sOLEDBConnect As String
Private m_sODBCDSN As String
Private m_sODBCUID As String
Private m_sODBCPWD As String
Private m_nDBMSType As EDBMSTypes
Private m_lURNTimeout As Long
Private m_nDataAccessMethod As EDataAccessMethods
Private m_nCheckDataVersion As Long
Private m_nCheckServiceRelease As Long
Private m_bDontTrapConnectionErrors As Boolean
Private m_bDontAssumeDataStructures As Boolean

' Internal error handler which can be manipulated by external code.
Private m_eh As CErrorHandler

' Cached ADO objects.
Private m_con As ADODB.Connection
Private m_procSettingRead As ADODB.Command
Private m_procSettingWrite As ADODB.Command
Private m_procURNGetNext As ADODB.Command
Private m_procBucketURNGetNext As ADODB.Command
Private m_procSettingReadAllStartup As ADODB.Command
Private m_procIntExecuteWithinTransaction As ADODB.Command

' The last SQL statement executed against the database.
Private m_sLastSQL As String

' Are we in the process of connecting?
Private m_bConnecting As Boolean

' The number of times to retry a deadlock error.
Private m_nRetryDeadlockErrors As Long

' Control information to handle automatic retries in the
' two error handler functions.
Private m_nErrorHandlerRetryCount As Long
Private m_bLastErrorWasDeadlock As Boolean

' The Swift 5 INI file. This is ONLY used for certain Swift 5 data access methods.
Const ksFileConfig = "policy.ini"

' Error declarations.
Const ksErrSource = "iSWDatabase.CDatabase"
Const knErrBadSettingType = 13
Const ksErrBadSettingType = "Type mismatch (illegal data type in Settings table)."
Const knErrURNNull = vbObjectError + 1
Const ksErrURNNull = "URN procedure returned null."
Const knErrURNZero = vbObjectError + 2
Const ksErrURNZero = "URN procedure returned zero."
Const knErrBadDAM = vbObjectError + 3
Const ksErrBadDAM = "The Data Access Method is not valid for this operation."
Const knErrNoODBCDSN = vbObjectError + 4
Const ksErrNoODBCDSN = "A valid ODBC DSN is required to connect to the database."
Const knErrBadConnectMethod = vbObjectError + 5
Const ksErrBadConnectMethod = "The Connect method is not supported for Swift code. Use ConnectV2 instead."
Const knErrBadConnectionString = vbObjectError + 6
Const ksErrBadConnectionString = "Valid server and database names are required to connect to the database."
Const knErrNoURNTimeout = vbObjectError + 7
Const ksErrNoURNTimeout = "The URNTimeout setting must be a positive integer."
Const knErrNoLockoutRetries = vbObjectError + 8
Const ksErrNoLockoutRetries = "The LockoutRetries setting must be a positive integer."

' Attributes: Default
Public Property Get ErrorHandler() As CErrorHandler
Attribute ErrorHandler.VB_UserMemId = 0
    Set ErrorHandler = m_eh
End Property

#If SiriusDatabaseInstaller Then
' Allow calling code to instantiate the error handler separately and attach it
' to the database object. This may be useful for Swift code in future, but for
' the moment it's safest to leave it out.
Friend Property Set ErrorHandler(ByVal eh As CErrorHandler)
    Set m_eh = eh
End Property
#End If

Public Property Get Connection() As ADODB.Connection
    Set Connection = m_con
End Property

Public Property Get DataAccessMethod() As EDataAccessMethods
    DataAccessMethod = m_nDataAccessMethod
End Property

Public Property Get DBMSType() As EDBMSTypes
    DBMSType = m_nDBMSType
End Property

' Read the last SQL statement executed against the database.
Public Property Get LastSQL() As String
    LastSQL = m_sLastSQL
End Property

' Allow the calling program to clear this property.
Public Property Let LastSQL(ByVal sLastSQL As String)
    m_sLastSQL = sLastSQL
End Property

Public Property Get URNTimeout() As Long
    URNTimeout = m_lURNTimeout
End Property

Public Property Let URNTimeout(ByVal lURNTimeout As Long)
    m_lURNTimeout = lURNTimeout
End Property

' Connects to the database.
Public Sub Connect( _
    Optional ByVal nDataAccessMethod As EDataAccessMethods = knDAMUnknown, _
    Optional ByVal sOLEDBConnect As String = "", _
    Optional ByVal bClientCursors As Boolean = False, _
    Optional ByVal nCheckDataVersion As Long = 0, _
    Optional ByVal bReadSettings As Boolean = False)

    If nCheckDataVersion <> 0 Then
        Err.Raise knErrBadConnectMethod, ksErrSource, ksErrBadConnectMethod
    Else
        ConnectV2 nDataAccessMethod, sOLEDBConnect, bClientCursors, _
                  nCheckDataVersion, 0, bReadSettings
    End If

End Sub

' Standard method for Swift applications to connect to the database.
Public Sub ConnectV2( _
    Optional ByVal nDataAccessMethod As EDataAccessMethods = knDAMUnknown, _
    Optional ByVal sOLEDBConnect As String = "", _
    Optional ByVal bClientCursors As Boolean = False, _
    Optional ByVal nCheckDataVersion As Long = 0, _
    Optional ByVal nCheckServiceRelease As Long = 0, _
    Optional ByVal bReadSettings As Boolean = False)

    Dim nDataVersion As Integer
    Dim nServiceRelease As Integer

    On Error GoTo EH_Connect
    m_bConnecting = True

    ' Break out special flags first.
    m_bDontTrapConnectionErrors = nDataAccessMethod And knDAMDontTrapConnectionErrors
    m_bDontAssumeDataStructures = nDataAccessMethod And knDAMDontAssumeDataStructures
    m_nDataAccessMethod = nDataAccessMethod And knDAMMaskFlags
    m_nCheckDataVersion = nCheckDataVersion
    m_nCheckServiceRelease = nCheckServiceRelease

    ' InteractiveUser stuff
    If m_nDataAccessMethod = knDAMSwiftInteractive Then
        m_bDontTrapConnectionErrors = True
        Me.ErrorHandler.UnattendedExecution = True
    End If

    ' Set the cursor library before attempting to open
    ' any connections.
    If bClientCursors Then
        m_con.CursorLocation = adUseClient
    Else
        m_con.CursorLocation = adUseServer
    End If

    ' Construct the OLEDB connection string depending on the
    ' data access method. Write the results into the four
    ' module-level variables for other procedures to use.
    BuildConnectionString sOLEDBConnect

    Select Case m_nDataAccessMethod

    Case knDAMSwift6Version2, knDAMSiriusDirect, knDAMSiriusOriginal, knDAMUnknown, _
        knDAMSwift6Original, knDAMSwift5Original, knDAMSwiftReplication, knDAMSwiftInteractive
        ' Attempt to login once.
        AttemptToLogin False

    Case knDAMSwift6Version1, knDAMSwift5Version1
        ' Attempt to login using the official login name and
        ' password. If that fails, then automatically create it
        ' and try again. If that fails, then we cannot connect.
        If Not AttemptToLogin(True) Then
            AttemptToCreateLogin
            AttemptToLogin False
        End If

    End Select

    ' Check database version if required. Some data access
    ' methods cannot do this, so ignore them.
    Select Case m_nDataAccessMethod

    Case knDAMSwift6Version2, knDAMSwift6Version1, knDAMSwift6Original, knDAMSwiftReplication, knDAMSwiftInteractive
        If m_nCheckDataVersion <> 0 And Not m_bDontAssumeDataStructures Then
            nDataVersion = SettingRead("Database", "Version6", 0)
            If nDataVersion <> m_nCheckDataVersion Then
                ErrorDialog Me, "Database version " & _
                    m_nCheckDataVersion & " expected.", _
                    nButtons:=ebOK, _
                    sLogInfo:="The database version is " & nDataVersion & "."
                GoTo EX_Connect
            End If

            nServiceRelease = SettingRead("Database", "ServiceRelease", 0)
            If nServiceRelease <> m_nCheckServiceRelease Then
                ErrorDialog Me, "Service Release " & _
                    m_nCheckServiceRelease & " expected.", _
                    nButtons:=ebOK, _
                    sLogInfo:="The service release version is " & nServiceRelease & "."
                GoTo EX_Connect
            End If
        End If

    Case knDAMSwift5Version1, knDAMSwift5Original
        If m_nCheckDataVersion <> 0 And Not m_bDontAssumeDataStructures Then
            nDataVersion = SettingRead("Database", "Version", 0)
            If nDataVersion <> m_nCheckDataVersion Then
                ErrorDialog Me, "Database version " & _
                    m_nCheckDataVersion & " expected.", _
                    nButtons:=ebOK, _
                    sLogInfo:="The database version is " & nDataVersion & "."
                GoTo EX_Connect
            End If
        End If

    Case Else
        ' Also prevent other data access methods from reading
        ' data they do not know is present.
        bReadSettings = False

    End Select

    ' The bReadSettings flag is now OBSOLETE. The calling program must
    ' instead call the separate class method SynchEnterpriseSettings()
    ' to read these values from the database. This is to improve
    ' performance when many EXEs and DLLs are connecting within a
    ' short space of time.

    ' Read settings from local registry keys, not from the database.
    m_con.CommandTimeout = RegRead(HKEY_LOCAL_MACHINE, ksRegPathDataBase, "InstallCommandTimeout", 60)
    m_lURNTimeout = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "URNTimeout", 60)
    If m_lURNTimeout <= 0 Then
        Err.Raise knErrNoURNTimeout, ksErrSource, ksErrNoURNTimeout
    End If
    m_nRetryDeadlockErrors = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "LockoutRetries", 5)
    If m_nRetryDeadlockErrors <= 0 Then
        Err.Raise knErrNoLockoutRetries, ksErrSource, ksErrNoLockoutRetries
    End If

    m_bConnecting = False
    Exit Sub

EH_Connect:
    If m_bDontTrapConnectionErrors Then
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    Select Case ErrorDialog(Me, "Cannot connect to the database.")
    Case erRetry
        Resume
    Case Else
        Resume EX_Connect
    End Select

EX_Connect:
    ' This is the abandonment code called if there was a
    ' problem during connection.
    ' Reset the error handler so that Err.Raise knErrAbort will
    ' work properly.
    On Error GoTo 0
    m_bConnecting = False

    Disconnect
    Err.Raise knErrAbort

End Sub

' Disconnects from the database.
Public Sub Disconnect()

    On Error GoTo EH_Disconnect

    ' Clear previous SQL status.
    m_sLastSQL = ""

    ' Destroy all stored procedures.
    Set m_procSettingRead = Nothing
    Set m_procSettingWrite = Nothing
    Set m_procURNGetNext = Nothing
    Set m_procBucketURNGetNext = Nothing
    Set m_procSettingReadAllStartup = Nothing
    Set m_procIntExecuteWithinTransaction = Nothing

    ' Close the connection (if it's open).
    If m_con.State <> adStateClosed Then
        m_con.Close
    End If

    ' Reset variables.
    m_sOLEDBConnect = ""
    m_nDBMSType = knDBMSUnknown
    m_lURNTimeout = 0
    m_nDataAccessMethod = knDAMUnknown
    m_nCheckDataVersion = 0
    m_bDontTrapConnectionErrors = False
    m_bDontAssumeDataStructures = False

    Exit Sub

EH_Disconnect:
    If m_bDontTrapConnectionErrors Then
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    Select Case ErrorDialog(Me, "Cannot disconnect from the database.")
    Case erRetry
        Resume
    Case Else
        Exit Sub
    End Select

End Sub

#If SiriusDatabaseInstaller = False Then
' Returns a recordset containing all the user data needed for CCrpytUser
Public Function GetUserRecord(Optional ByVal lID As Long, _
    Optional ByVal sName As String = "", _
    Optional ByVal bTemplateRecord As Boolean = False, _
    Optional ByVal lTeamFilterID As Long = 0, _
    Optional ByVal nAgentTypeFilter As Integer = 0, _
    Optional ByVal bShowInteractive As Boolean = True, _
    Optional ByVal sOSLoginUserName As String = "", _
    Optional ByVal bForceAgentLink As Boolean = False) As ADODB.Recordset

    Dim rs As ADODB.Recordset
    Dim oSQL As CSelectANSI

    ' Create the SQL Here
    Set oSQL = New CSelectANSI
    With oSQL
        ' Asterisk is required to allow this code to run on different security versions, e.g. DbSecure.
        .AddField "IntUsers.*"
        .AddField "S_AGENTS.OfficeCode"
        .AddField "S_AGENTS.AGT_TYPE"
        .AddField "AssociatedAgent.AGT_TYPE AssociatedAgentType"

        .AddTable "IntUsers"
        If bForceAgentLink And Not bTemplateRecord Then
            .AddTable "S_AGENTS", "inner", "IntUsers.PrimaryPartnerCode = S_AGENTS.CODE"
        Else
            .AddTable "S_AGENTS", "left", "IntUsers.PrimaryPartnerCode = S_AGENTS.CODE"
        End If

        .AddTable "S_AGENTS AssociatedAgent", "left", "IntUsers.AssociatedAgent = AssociatedAgent.Code"

        If lID <> 0 Then
            ' If we are filtering on the ID, then we don't need to filter on anything else...
            .AddFilter "IntUsers.ID = " & ToSQL(lID)
        Else
            If Len(sOSLoginUserName) <> 0 Then
                .AddFilter "OSLoginUserName = " & ToSQL(sOSLoginUserName)
            End If
            If lTeamFilterID <> 0 Then
                .AddFilter "ID in (select UserID from IntTeamUsers where TeamID = " & ToSQL(lTeamFilterID) & ")"
            End If
            If Len(sName) <> 0 Then
                .AddFilter "IntUsers.Name = " & ToSQL(sName)
            End If
            If bTemplateRecord Then
                .AddFilter "isnull(TemplateRecord, 0) = 1"
            Else
                .AddFilter "isnull(TemplateRecord, 0) = 0"
            End If
            If nAgentTypeFilter <> 0 Then
                .AddFilter "S_AGENTS.AGT_TYPE = " & ToSQL(nAgentTypeFilter)
            End If
            If Not bShowInteractive Then
                .AddFilter "IntUsers.Name <> 'Interactive'"
            End If
            'Add sort order on Name
            .AddSort "IntUsers.Name"
        End If
        ' Open the recordset
        Set rs = OpenRecordset(.SQL)
    End With
    Set oSQL = Nothing

    ' Return the recordset
    Set GetUserRecord = rs

End Function
#End If

' Creates a new (connected) command object.
' Errors are passed back to the caller.
Public Function NewCommand(ByVal sSQL As String, _
    Optional ByVal bProcedureName As Boolean = False) As ADODB.Command

    Dim qry As ADODB.Command

    ' Prepare the command so all you have to do is
    ' add parameters to it.
    Set qry = New ADODB.Command
    Set qry.ActiveConnection = m_con
    qry.CommandText = sSQL
    qry.CommandType = IIf(bProcedureName, adCmdStoredProc, adCmdText)
    ' Return the command for further modification.
    Set NewCommand = qry

End Function

Public Function NewCommandHelper(ByVal sText As String, _
    Optional ByVal bProcedure As Boolean = False, _
    Optional ByVal bPrepared As Boolean = False) As CCommandHelper

    Dim oCommandHelper As CCommandHelper

    Set oCommandHelper = New CCommandHelper
    oCommandHelper.Initialise Me, sText, bProcedure, bPrepared

    Set NewCommandHelper = oCommandHelper

End Function

' Executes an SQL statement and returns a server-side forward-only recordset.
' Errors are passed back to the caller.
Public Function OpenRecordset(ByVal sSQL As String) As ADODB.Recordset

    Dim rs As ADODB.Recordset

    ' Clear previous SQL and Error status.
    m_con.Errors.Clear
    m_sLastSQL = sSQL

    ' Open the recordset.
    Set rs = New ADODB.Recordset
    rs.Open sSQL, m_con, adOpenForwardOnly, adLockReadOnly, adCmdText
    ' Skip over all closed recordsets until we find an open one.
    Do
        If rs.State = adStateOpen Then Exit Do
        Set rs = rs.NextRecordset()
        If rs Is Nothing Then Exit Do
    Loop
    ' Return an open Recordset or Nothing.
    Set OpenRecordset = rs

End Function

' Executes an SQL statement and returns a client-side disconnected forward-only recordset.
' Errors are passed back to the caller.
Public Function OpenRecordsetDisconnected(ByVal sSQL As String) As ADODB.Recordset

    Dim rs As ADODB.Recordset

    ' Clear previous SQL and Error status.
    m_con.Errors.Clear
    m_sLastSQL = sSQL

    ' Open the recordset.
    Set rs = New ADODB.Recordset
    rs.CursorLocation = adUseClient
    rs.Open sSQL, m_con, adOpenForwardOnly, adLockReadOnly, adCmdText
    ' Skip over all closed recordsets until we find an open one.
    Do
        If rs.State = adStateOpen Then Exit Do
        Set rs = rs.NextRecordset()
        If rs Is Nothing Then Exit Do
    Loop
    ' Disconnect it.
    Set rs.ActiveConnection = Nothing
    ' Return an open Recordset or Nothing.
    Set OpenRecordsetDisconnected = rs

End Function

' Executes an SQL statement that doesn't return a recordset.
' Errors are passed back to the caller.
Public Sub Execute(ByVal sSQL As String)

    ' Clear previous SQL and Error status.
    m_con.Errors.Clear
    m_sLastSQL = sSQL

    ' Execute the statement and don't return data.
    m_con.Execute sSQL, , adCmdText Or adExecuteNoRecords

End Sub

' Purpose:  Executes an SQL statement and excapsulates it within database transactions.
'           Handles commit or rollback if there is an error
' Input Parameters:
'           sSQL String      - The SQL statement to execute
' Return Value: none

Public Sub ExecuteWithinTransaction(ByVal sSQL As String)

    Dim nRetry As Long

    On Error GoTo EH_ExecuteWithinTransaction
    nRetry = 0

    ' If this procedure has never been called, then generate a
    ' command and cache it for the lifetime of the database object.
    If m_procIntExecuteWithinTransaction Is Nothing Then
        Set m_procIntExecuteWithinTransaction = NewCommand("IntExecuteWithinTransaction", True)
        With m_procIntExecuteWithinTransaction
            .Parameters.Append .CreateParameter("sStatement", adVarWChar, adParamInput, 1073741823)
        End With
    End If

    ' Execute the procedure.
    m_procIntExecuteWithinTransaction.Parameters("sStatement").Value = sSQL
    m_procIntExecuteWithinTransaction.Execute Options:=adExecuteNoRecords

    Exit Sub

EH_ExecuteWithinTransaction:
    ' Allow a number of automatic retries when a process lock is
    ' encountered (defaults to 5). Once the limit has been reached
    ' then return the error back to the caller.
    If IsDeadlockError() Then
        nRetry = nRetry + 1
        If nRetry <= m_nRetryDeadlockErrors Then
            Resume
        End If
    End If
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Public Function LastError() As ADODB.Error

    Set LastError = Nothing
    If m_con Is Nothing Then Exit Function
    If m_con.Errors Is Nothing Then Exit Function

    With m_con.Errors
        If .Count > 0 Then
            Set LastError = .Item(.Count - 1)
        End If
    End With

End Function

Public Function LastErrorsText() As String

    Dim sText As String
    Dim oError As ADODB.Error

    LastErrorsText = ""
    If m_con Is Nothing Then Exit Function
    If m_con.Errors Is Nothing Then Exit Function

    sText = ""
    ' Start with the original error and work upwards.
    For Each oError In m_con.Errors
        sText = oError.Description & " (" & _
            oError.Number & ") (" & _
            oError.NativeError & ") (" & _
            oError.SQLState & ") (" & _
            oError.Source & ")" & vbCrLf & sText
    Next
    LastErrorsText = sText

End Function

Public Function LastErrorWas(ByVal nNativeError As Long, ByVal sSQLState As String) As Boolean

    LastErrorWas = False
    If m_con Is Nothing Then Exit Function
    If m_con.Errors Is Nothing Then Exit Function

    With m_con.Errors
        If .Count > 0 Then
            With .Item(.Count - 1)
                If .NativeError = nNativeError And .SQLState = sSQLState Then
                    LastErrorWas = True
                End If
            End With
        End If
    End With

End Function

Public Function LastErrorWasVB(ByVal nErrNumber As Long, ByVal sErrSource As String) As Boolean

    LastErrorWasVB = False
    If m_con Is Nothing Then Exit Function
    If m_con.Errors Is Nothing Then Exit Function

    With m_con.Errors
        If .Count > 0 Then
            With .Item(.Count - 1)
                If .Number = nErrNumber And .Source = sErrSource Then
                    LastErrorWasVB = True
                End If
            End With
        End If
    End With

End Function

#If SiriusDatabaseInstaller = 0 Then
' Create a new PublicNotCreatable object and "attach" it to this
' database connection.
' Optionally initialise it with relevant information.
Public Function NewDeletedList(Optional ByVal sTable As String = "") As CDeletedList

    Dim dl As CDeletedList

    Set dl = New CDeletedList
    dl.AttachDatabase Me
    dl.Table = sTable
    Set NewDeletedList = dl

End Function
#End If

#If SiriusDatabaseInstaller = 0 Then
' Create a new PublicNotCreatable object and "attach" it to this
' database connection.
' Optionally initialise it with relevant information.
Public Function NewFormRecord() As CFormRecord

    Dim fr As CFormRecord

    Set fr = New CFormRecord
    fr.AttachDatabase Me
    Set NewFormRecord = fr

End Function
#End If

#If SiriusDatabaseInstaller = 0 Then
' Create a new PublicNotCreatable object and "attach" it to this
' database connection.
' Optionally initialise it with relevant information.
Public Function NewRecord(Optional ByVal sTable As String = "", _
    Optional ByVal sPrimaryKey As String = "") As CRecord

    Dim rec As CRecord

    Set rec = New CRecord
    rec.AttachDatabase Me
    rec.Table = sTable
    rec.PrimaryKey = sPrimaryKey
    Set NewRecord = rec

End Function
#End If

' Reads enterprise-level settings from the database and replicates them
' into this machine's registry for future use. This allows the system
' admin to change common database and error settings and not have to go
' round each machine changing them.
Public Sub SynchWithEnterpriseSettings()

    Dim rs As ADODB.Recordset
    Dim fldSectionName As ADODB.Field
    Dim fldKeyName As ADODB.Field
    Dim fldKeyType As ADODB.Field
    Dim fldKeyValue As ADODB.Field
    Dim vValue As Variant
    Dim bSaveErrorSettings As Boolean

    On Error GoTo EH_Handler

    ' If this procedure has never been called, then generate a
    ' command and cache it for the lifetime of the database object.
    If m_procSettingReadAllStartup Is Nothing Then
        Set m_procSettingReadAllStartup = NewCommand("SettingReadAllStartup", True)
    End If

    ' Read all settings at once for extra speed.
    Set rs = m_procSettingReadAllStartup.Execute()
    Set fldSectionName = rs("SectionName")
    Set fldKeyName = rs("KeyName")
    Set fldKeyType = rs("KeyType")
    Set fldKeyValue = rs("KeyValue")
    bSaveErrorSettings = False
    Do Until rs.EOF

        ' Read the setting value in the same way as SettingRead().
        vValue = fldKeyValue
        Select Case fldKeyType
        Case vbEmpty
            vValue = Empty
        Case vbNull
            vValue = Null
        Case vbBoolean
            vValue = ToBooleanFixed(vValue)
        Case vbByte
            vValue = ToByteFixed(vValue)
        Case vbInteger
            vValue = ToIntegerFixed(vValue)
        Case vbLong
            vValue = ToLongFixed(vValue)
        Case vbDouble
            vValue = ToDoubleFixed(vValue)
        Case vbCurrency
            vValue = ToCurrencyFixed(vValue)
        Case vbString
            vValue = ToStringFixed(vValue)
        Case vbDate
            vValue = ToDateTimeFixed(vValue)
        Case Else
            Err.Raise knErrBadSettingType, ksErrSource, ksErrBadSettingType
        End Select

        ' Decide what to do with it.
        Select Case LCase$(fldSectionName)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Case "assumptions"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Copy the value directly into the registry.
            RegWrite HKEY_LOCAL_MACHINE, ksRegPathAssumptions, fldKeyName, vValue

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Case "database"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Update database class properties, and copy value to the registry
            ' for use next time the database class is instantiated.
            Select Case LCase$(fldKeyName)
                    Case "querytimeout"
                        If m_con.CommandTimeout <> vValue Then
                    m_con.CommandTimeout = vValue
                    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "CommandTimeout", vValue
                End If
            Case "urntimeout"
                If vValue <= 0 Then
                    Err.Raise knErrNoURNTimeout, ksErrSource, ksErrNoURNTimeout
                End If
                If m_lURNTimeout <> vValue Then
                    m_lURNTimeout = vValue
                    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "URNTimeout", vValue
                End If
            Case "lockoutretrys"
                If vValue <= 0 Then
                    Err.Raise knErrNoLockoutRetries, ksErrSource, ksErrNoLockoutRetries
                End If
                If m_nRetryDeadlockErrors <> vValue Then
                    m_nRetryDeadlockErrors = vValue
                    RegWrite HKEY_LOCAL_MACHINE, ksRegPathProject, "LockoutRetries", vValue
                End If
            End Select

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Case "errors"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Update error class properties, and copy value to the registry
            ' for use next time the error class is instantiated.
            Select Case LCase$(fldKeyName)
            Case "standardmessage"
                If m_eh.StandardMessage <> vValue Then
                    m_eh.StandardMessage = vValue
                    bSaveErrorSettings = True
                End If
            Case "diagnostics"
                If m_eh.Diagnostics <> vValue Then
                    m_eh.Diagnostics = vValue
                    bSaveErrorSettings = True
                End If
            Case "emailtoname"
                If m_eh.EmailToName <> vValue Then
                    m_eh.EmailToName = vValue
                    bSaveErrorSettings = True
                End If
            Case "emailtoaddress"
                If m_eh.EmailToAddress <> vValue Then
                    m_eh.EmailToAddress = vValue
                    bSaveErrorSettings = True
                End If
            Case "emailccname"
                If m_eh.EmailCcName <> vValue Then
                    m_eh.EmailCcName = vValue
                    bSaveErrorSettings = True
                End If
            Case "emailccaddress"
                If m_eh.EmailCcAddress <> vValue Then
                    m_eh.EmailCcAddress = vValue
                    bSaveErrorSettings = True
                End If
            End Select

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        End Select
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        rs.MoveNext
    Loop

    ' If any of the error class properties were changed, then save them once.
    If bSaveErrorSettings Then
        m_eh.StoreSettings
    End If

EX_NormalExit:
    On Error Resume Next
    If Not rs Is Nothing Then
        rs.Close
        Set rs = Nothing
    End If
    Exit Sub

EH_Handler:
    If m_bConnecting Then
        If m_bDontTrapConnectionErrors Then
            Err.Raise Err.Number, Err.Source, Err.Description
        End If
    End If
    Select Case ErrorDialog(Me, "Cannot synchronise with global database settings.")
    Case erRetry
        Resume
    Case Else
        If m_bConnecting Then
            Err.Raise knErrAbort
        Else
            Resume EX_NormalExit
        End If
    End Select

End Sub

' Reads value from Settings table. Returns vDefault if not found.
' Dates are stored in a locale-independent form.
' Strings are limited to 255 chars.
Public Function SettingRead(ByVal sFolder As String, _
    ByVal sAttribute As String, _
    ByVal vDefault As Variant) As Variant

    Dim nType As Integer
    Dim vValue As Variant

    On Error GoTo EH_SettingRead
    SettingRead = vDefault

    nType = VarType(vDefault)
    Select Case nType
    Case vbEmpty, vbNull, vbBoolean, vbByte, vbInteger, _
        vbLong, vbDouble, vbCurrency, vbDate, vbString
        vValue = Left$(ToStringFixed(vDefault), 255)
    Case Else
        Err.Raise knErrBadSettingType, ksErrSource, ksErrBadSettingType
    End Select

    ' If this procedure has never been called, then generate a
    ' command and cache it for the lifetime of the database object.
    If m_procSettingRead Is Nothing Then
        Set m_procSettingRead = NewCommand("SettingRead", True)
        With m_procSettingRead
            .Parameters.Append .CreateParameter("sSectionName", adVarChar, adParamInput, 40)
            .Parameters.Append .CreateParameter("sKeyName", adVarChar, adParamInput, 40)
            .Parameters.Append .CreateParameter("nDefaultType", adTinyInt, adParamInput)
            .Parameters.Append .CreateParameter("sDefaultValue", adVarChar, adParamInput, 255)
            .Parameters.Append .CreateParameter("r_nReturnType", adTinyInt, adParamInputOutput)
            .Parameters.Append .CreateParameter("r_sReturnValue", adVarChar, adParamInputOutput, 255)
        End With
    End If

    m_procSettingRead("sSectionName") = sFolder
    m_procSettingRead("sKeyName") = sAttribute
    m_procSettingRead("nDefaultType") = nType
    m_procSettingRead("sDefaultValue") = vValue
    m_procSettingRead("r_nReturnType") = 0
    m_procSettingRead("r_sReturnValue") = ""
    m_procSettingRead.Execute Options:=adExecuteNoRecords
    nType = m_procSettingRead("r_nReturnType")
    vValue = m_procSettingRead("r_sReturnValue")

    Select Case nType
    Case vbEmpty
        vValue = Empty
    Case vbNull
        vValue = Null
    Case vbBoolean
        vValue = ToBooleanFixed(vValue)
    Case vbByte
        vValue = ToByteFixed(vValue)
    Case vbInteger
        vValue = ToIntegerFixed(vValue)
    Case vbLong
        vValue = ToLongFixed(vValue)
    Case vbDouble
        vValue = ToDoubleFixed(vValue)
    Case vbCurrency
        vValue = ToCurrencyFixed(vValue)
    Case vbString
        vValue = ToStringFixed(vValue)
    Case vbDate
        vValue = ToDateTimeFixed(vValue)
    Case Else
        Err.Raise knErrBadSettingType, ksErrSource, ksErrBadSettingType
    End Select

    SettingRead = vValue
    Exit Function

EH_SettingRead:
    If m_bConnecting Then
        If m_bDontTrapConnectionErrors Then
            Err.Raise Err.Number, Err.Source, Err.Description
        End If
    End If
    Select Case ErrorDialog(Me, "Cannot read global setting value.")
    Case erRetry
        Resume
    Case Else
        If m_bConnecting Then
            Err.Raise knErrAbort
        Else
            Exit Function
        End If
    End Select

End Function

' Stores value in Settings table. Creates it if it doesn't exist.
' Dates are stored in a locale-independent form.
' Strings are limited to 255 chars.
Public Sub SettingWrite(ByVal sFolder As String, _
    ByVal sAttribute As String, _
    ByVal vValue As Variant)

    Dim nType As Integer

    On Error GoTo EH_SettingWrite

    nType = VarType(vValue)
    Select Case nType
    Case vbEmpty, vbNull, vbBoolean, vbByte, vbInteger, _
        vbLong, vbDouble, vbCurrency, vbDate, vbString
        vValue = Left$(ToStringFixed(vValue), 255)
    Case Else
        Err.Raise knErrBadSettingType, ksErrSource, ksErrBadSettingType
    End Select

    ' If this procedure has never been called, then generate a
    ' command and cache it for the lifetime of the database object.
    If m_procSettingWrite Is Nothing Then
        Set m_procSettingWrite = NewCommand("SettingWrite", True)
        With m_procSettingWrite
            .Parameters.Append .CreateParameter("sSectionName", adVarChar, adParamInput, 40)
            .Parameters.Append .CreateParameter("sKeyName", adVarChar, adParamInput, 40)
            .Parameters.Append .CreateParameter("nKeyType", adTinyInt, adParamInput)
            .Parameters.Append .CreateParameter("sKeyValue", adVarChar, adParamInput, 255)
        End With
    End If

    m_procSettingWrite("sSectionName") = sFolder
    m_procSettingWrite("sKeyName") = sAttribute
    m_procSettingWrite("nKeyType") = nType
    m_procSettingWrite("sKeyValue") = vValue
    m_procSettingWrite.Execute Options:=adExecuteNoRecords

    Exit Sub

EH_SettingWrite:
    If m_bConnecting Then
        If m_bDontTrapConnectionErrors Then
            Err.Raise Err.Number, Err.Source, Err.Description
        End If
    End If
    Select Case ErrorDialog(Me, "Cannot write global setting value.")
    Case erRetry
        Resume
    Case Else
        If m_bConnecting Then
            Err.Raise knErrAbort
        Else
            Exit Sub
        End If
    End Select

End Sub

' Deletes value from Settings table.
Public Sub SettingDelete(ByVal sFolder As String, _
    ByVal sAttribute As String)

    On Error GoTo EH_SettingDelete

    Execute "execute SettingDelete " & ToSQL(sFolder) & ", " & ToSQL(sAttribute)

    Exit Sub

EH_SettingDelete:
    If m_bConnecting Then
        If m_bDontTrapConnectionErrors Then
            Err.Raise Err.Number, Err.Source, Err.Description
        End If
    End If
    Select Case ErrorDialog(Me, "Cannot delete global setting value.")
    Case erRetry
        Resume
    Case Else
        If m_bConnecting Then
            Err.Raise knErrAbort
        Else
            Exit Sub
        End If
    End Select

End Sub

Public Function URNGetNext(ByVal sTable As String) As Long

    Const knErrSSADeadlock = -306
    Const ksErrSSADeadlock = "Cannot create a new record." & vbCrLf & _
        "This is because another user is creating a new record of the same type, " & _
        "right at this moment. " & _
        "If you re-try the operation it may work this time." & vbCrLf & _
        "As a very last resort, you could abandon the process. " & _
        "You will LOSE ALL THE INFORMATION that you have entered, " & _
        "but it will allow you to continue with your work."

    Dim dtTimeout As Date
    Dim rs As ADODB.Recordset
    Dim vlID As Variant ' Long or Null

    Select Case m_nDataAccessMethod

    Case knDAMSwift6Version2, knDAMSwift6Version1, knDAMSwift6Original
        ' If this procedure has never been called, then generate a
        ' command and cache it for the lifetime of the database object.
        If m_procURNGetNext Is Nothing Then
            On Error GoTo EH_CreateProcedure
            Set m_procURNGetNext = NewCommand("URNGetNext", True)
            With m_procURNGetNext
                .Parameters.Append .CreateParameter("TableName", adVarChar, adParamInput, 40)
                .Parameters.Append .CreateParameter("ReturnID", adInteger, adParamInputOutput)
            End With
        End If

        ' Now that we have a command object, call it to obtain an ID value.
        On Error GoTo EH_ExecuteProcedure
        URNGetNext = 0
        dtTimeout = DateAdd("s", m_lURNTimeout, Now())

        m_procURNGetNext("TableName") = sTable
        m_procURNGetNext("ReturnID") = 0
        m_procURNGetNext.Execute Options:=adExecuteNoRecords
        vlID = m_procURNGetNext("ReturnID")

        If IsNull(vlID) Then
            Err.Raise knErrURNNull, ksErrSource, ksErrURNNull
        ElseIf vlID = 0 Then
            Err.Raise knErrURNZero, ksErrSource, ksErrURNZero
        Else
            URNGetNext = vlID
        End If

    Case knDAMSwift5Version1, knDAMSwift5Original
        ' Call the stored procedure to obtain an ID value.
        On Error GoTo EH_ExecuteProcedure
        URNGetNext = 0
        dtTimeout = DateAdd("s", m_lURNTimeout, Now())

        Set rs = OpenRecordset("execute URNGetNext " & ToSQL(sTable))
        vlID = rs(0)
        rs.Close

        URNGetNext = vlID

    Case Else
        Err.Raise knErrBadDAM, ksErrSource, ksErrBadDAM

    End Select

    Exit Function

EH_CreateProcedure:
    ErrorDialog Me, "Cannot initialise the ID number generator. " & _
        "You will not be able to create new records in the database until this problem is resolved.", _
        bCritical:=True, _
        nButtons:=ebOK
    Err.Raise knErrAbort

EH_ExecuteProcedure:
    ' If we are waiting for another connection to finish getting a
    ' URN, SASE and MSS wait indefinitely to continue. However,
    ' SSA aborts with an error straight away. We therefore have to
    ' trap this and simulate an indefinite wait with timeout
    ' facility.
    If LastError().Number = knErrSSADeadlock Then
        ' If we have a locking error, retry continually until the
        ' timeout expires. Then we ask user for permission to retry.
        ' If user cancels, we abort with an error.
        If Now() < dtTimeout Then
            Resume
        Else
            ' Show message box.
            If MsgBox(ksErrSSADeadlock, _
                vbExclamation + vbRetryCancel, _
                "Creating New Records") = vbRetry Then
                ' Restart timer.
                dtTimeout = DateAdd("s", m_lURNTimeout, Now())
                Resume
            End If
        End If
    End If
    ' Generate a proper error message and give the user a chance to
    ' retry.
    Select Case ErrorDialog(Me, "Cannot create new ID for """ & sTable & """. " & _
        "If you re-try the operation it may work this time.")
    Case erRetry
        ' Restart timer for SSA's benefit.
        dtTimeout = DateAdd("s", m_lURNTimeout, Now())
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Function

Public Function URNGetNextTempFill(ByVal sTable As String) As Long

    URNGetNextTempFill = URNGetNext(sTable & "_Fill")

End Function

Public Function ErrorHandlerDialog(ByVal sMessage As String, _
    Optional ByVal nErrNumber As Long = 0, _
    Optional ByVal sErrSource As String = "", _
    Optional ByVal sErrDescription As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal nButtons As EErrorButtons = ebRetryCancel, _
    Optional ByVal bCritical As Boolean = False, _
    Optional ByVal sLogInfo As String = "") As EErrorResults

    ' If error is a deadlock
    If IsDeadlockError() Then
        ' If it's the first deadlock or we have not finished retrying
        If (Not m_bLastErrorWasDeadlock) Or (m_nErrorHandlerRetryCount <= m_nRetryDeadlockErrors) Then
            ' This error is a deadlock
            m_bLastErrorWasDeadlock = True
            ' Increment retry count
            m_nErrorHandlerRetryCount = m_nErrorHandlerRetryCount + 1
            ' Return "retry" action
            ErrorHandlerDialog = erRetry
            Exit Function
        End If
    End If

    ' This error is not a deadlock
    m_bLastErrorWasDeadlock = False
    ' Reset retry count
    m_nErrorHandlerRetryCount = 0

    ' If the last DB error is equal to the VB error, then blank out the VB error to avoid
    ' unnecessary duplication in the error logs.
    If LastErrorWasVB(nErrNumber, sErrSource) Then
        nErrNumber = 0
        sErrSource = ""
        sErrDescription = ""
    End If

    ' Handle error in the normal way
    ErrorHandlerDialog = m_eh.DialogV2(sMessage, _
        nErrNumber:=nErrNumber, _
        sErrSource:=sErrSource, _
        sErrDescription:=sErrDescription, _
        sDBErrors:=LastErrorsText(), _
        sTitle:=sTitle, _
        nButtons:=nButtons, _
        bCritical:=bCritical, _
        sLogInfo:=sLogInfo)

    ' If the user wants to retry
    If ErrorHandlerDialog = erRetry Then
        ' If error is a timeout, then prompt for new value
        If IsTimeoutError() Then
            PromptUserForNewTimeout
        End If
    End If

End Function

Public Sub ErrorHandlerReport(ByVal sMessage As String, _
    Optional ByVal nErrNumber As Long = 0, _
    Optional ByVal sErrSource As String = "", _
    Optional ByVal sErrDescription As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    ' If the last DB error is equal to the VB error, then blank out the VB error to avoid
    ' unnecessary duplication in the error logs.
    If LastErrorWasVB(nErrNumber, sErrSource) Then
        nErrNumber = 0
        sErrSource = ""
        sErrDescription = ""
    End If

    ' Handle error in the normal way
    m_eh.ReportV2 sMessage, _
        nErrNumber:=nErrNumber, _
        sErrSource:=sErrSource, _
        sErrDescription:=sErrDescription, _
        sDBErrors:=LastErrorsText(), _
        sTitle:=sTitle, _
        sLogInfo:=sLogInfo

End Sub

Private Sub Class_Initialize()

    #If SiriusDatabaseInstaller Then
        ' Allow calling code to instantiate the error handler separately and attach it
        ' to the database object. This may be useful for Swift code in future, but for
        ' the moment it's safest to leave it out.
    #Else
        ' Create and initialise an error handler.
        Set m_eh = New CErrorHandler
        m_eh.AppTitle = App.Title
        m_eh.AppVersion = AppVersion()
    #End If

    ' Create a connection.
    Set m_con = New ADODB.Connection

    ' Initialise variables.
    m_sOLEDBConnect = ""
    m_nDBMSType = knDBMSUnknown
    m_lURNTimeout = 0
    m_nDataAccessMethod = knDAMUnknown
    m_nCheckDataVersion = 0
    m_sLastSQL = ""
    m_bDontTrapConnectionErrors = False
    m_bDontAssumeDataStructures = False
    m_bConnecting = False
    m_nRetryDeadlockErrors = 0
    m_bLastErrorWasDeadlock = False
    m_nErrorHandlerRetryCount = 0

End Sub

Private Sub Class_Terminate()

    On Error Resume Next

    ' Disconnect from the database if it is still connected.
    Disconnect

    ' Destroy the connection.
    Set m_con = Nothing

    ' Destroy the error handler.
    Set m_eh = Nothing

End Sub

' Construct an OLEDB connection string depending on the
' data access method. Write the results into the four
' module-level variables for other procedures to use.
Private Sub BuildConnectionString(ByVal sOLEDBConnect As String)

    Const ksRegPMDAODataSources = ksRegPathPMRoot & "\Architecture\Server\Databases"
    Const ksRegPMDAODataSourceToUse = ksRegPMDAODataSources & "\SiriusSolutions"
    Const ksRegODBCDSNs = "Software\ODBC\ODBC.INI"
    Const ksRegODBCDSNToUse = ksRegODBCDSNs & "\Architecture"

    Dim iPosUserName As Long
    Dim iPosPassword As Long
    Dim iPosFlags As Long
    Dim sProvider As String
    Dim sServer As String
    Dim sDatabase As String

    m_sOLEDBConnect = sOLEDBConnect
    m_sODBCDSN = ""
    m_sODBCUID = ""
    m_sODBCPWD = ""

    Select Case m_nDataAccessMethod

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSwift6Version2, knDAMSwiftInteractive
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_sOLEDBConnect = "" Then
            ' Connection string not already provided, so
            ' build one from scratch using new registry keys.
            sServer = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "Server", "")
            sDatabase = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "Database", "")
            ' If a DS or IC are not provided, SQL Server will use
            ' default values. We must abort if this happens, or we
            ' cannot tell what it's actually connected to.
            If sServer = "" Or sDatabase = "" Then
                Err.Raise knErrBadConnectionString, ksErrSource, ksErrBadConnectionString
            End If
            m_sOLEDBConnect = "Provider=SQLOLEDB" & _
                ";Data Source=" & sServer & _
                ";Initial Catalog=" & sDatabase & _
                ";User ID=" & ksLoginName & _
                ";Password=" & ksLoginPassword & _
                ";OLE DB Services=-1"
        Else
            ' If string provided contains a username/password then use it.
            ' Otherwise, append the secret ones.
            iPosUserName = InStr(UCase$(m_sOLEDBConnect), "USER ID=")
            iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PASSWORD=")
            If iPosUserName = 0 Then
                iPosUserName = InStr(UCase$(m_sOLEDBConnect), "UID=")
            End If
            If iPosPassword = 0 Then
                iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PWD=")
            End If
            If iPosUserName = 0 And iPosPassword = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";User ID=" & ksLoginName & _
                    ";Password=" & ksLoginPassword
            End If
            ' If string provided contains an OLEDBServices flag
            ' then use it. Otherwise, append the standard one.
            iPosFlags = InStr(UCase$(m_sOLEDBConnect), "OLE DB SERVICES=")
            If iPosFlags = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";OLE DB Services=-1"
            End If
        End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSwiftReplication
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_sOLEDBConnect = "" Then
            ' Connection string not already provided, so
            ' build one from scratch using new registry keys.
            sServer = RegRead(HKEY_LOCAL_MACHINE, ksRegPathMainReplicationDB, "Server", "")
            sDatabase = RegRead(HKEY_LOCAL_MACHINE, ksRegPathMainReplicationDB, "Database", "")
            ' If a DS or IC are not provided, SQL Server will use
            ' default values. We must abort if this happens, or we
            ' cannot tell what it's actually connected to.
            If sServer = "" Or sDatabase = "" Then
                Err.Raise knErrBadConnectionString, ksErrSource, ksErrBadConnectionString
            End If
            m_sOLEDBConnect = "Provider=SQLOLEDB" & _
                ";Data Source=" & sServer & _
                ";Initial Catalog=" & sDatabase & _
                ";User ID=" & ksLoginName & _
                ";Password=" & ksLoginPassword & _
                ";OLE DB Services=-1"
        Else
            ' If string provided contains a username/password then use it.
            ' Otherwise, append the secret ones.
            iPosUserName = InStr(UCase$(m_sOLEDBConnect), "USER ID=")
            iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PASSWORD=")
            If iPosUserName = 0 Then
                iPosUserName = InStr(UCase$(m_sOLEDBConnect), "UID=")
            End If
            If iPosPassword = 0 Then
                iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PWD=")
            End If
            If iPosUserName = 0 And iPosPassword = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";User ID=" & ksLoginName & _
                    ";Password=" & ksLoginPassword
            End If
            ' If string provided contains an OLEDBServices flag
            ' then use it. Otherwise, append the standard one.
            iPosFlags = InStr(UCase$(m_sOLEDBConnect), "OLE DB SERVICES=")
            If iPosFlags = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";OLE DB Services=-1"
            End If
        End If


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSiriusOriginal
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_sOLEDBConnect = "" Then
            ' Connection string not already provided, so
            ' build one from scratch using new registry keys.
            sServer = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "SiriusServer", "")
            sDatabase = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "SiriusDatabase", "")
            ' If a DS or IC are not provided, SQL Server will use
            ' default values. We must abort if this happens, or we
            ' cannot tell what it's actually connected to.
            If sServer = "" Or sDatabase = "" Then
                Err.Raise knErrBadConnectionString, ksErrSource, ksErrBadConnectionString
            End If
            m_sOLEDBConnect = "Provider=SQLOLEDB" & _
                ";Data Source=" & sServer & _
                ";Initial Catalog=" & sDatabase & _
                ";User ID=" & ksSALoginName & _
                ";Password=" & ksSALoginPassword & _
                ";OLE DB Services=-1"
        Else
            ' If string provided contains a username/password then use it.
            ' Otherwise, append the secret ones.
            iPosUserName = InStr(UCase$(m_sOLEDBConnect), "USER ID=")
            iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PASSWORD=")
            If iPosUserName = 0 Then
                iPosUserName = InStr(UCase$(m_sOLEDBConnect), "UID=")
            End If
            If iPosPassword = 0 Then
                iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PWD=")
            End If
            If iPosUserName = 0 And iPosPassword = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";User ID=" & ksSALoginName & _
                    ";Password=" & ksSALoginPassword
            End If
            ' If string provided contains an OLEDBServices flag
            ' then use it. Otherwise, append the standard one.
            iPosFlags = InStr(UCase$(m_sOLEDBConnect), "OLE DB SERVICES=")
            If iPosFlags = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";OLE DB Services=-1"
            End If
        End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSiriusDirect
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_sOLEDBConnect = "" Then
            ' Connection string not already provided, so build one from scratch.
            ' Use the new PMDAO registry keys if they are defined. Otherwise,
            ' use the old ODBC DSN.
            If RegFolderExists(HKEY_LOCAL_MACHINE, ksRegPMDAODataSourceToUse) Then
                sProvider = RegRead(HKEY_LOCAL_MACHINE, ksRegPMDAODataSourceToUse, "Provider", "")
                sServer = RegRead(HKEY_LOCAL_MACHINE, ksRegPMDAODataSourceToUse, "Server", "")
                sDatabase = RegRead(HKEY_LOCAL_MACHINE, ksRegPMDAODataSourceToUse, "Database", "")
            Else
                sProvider = "SQLOLEDB"
                sServer = RegRead(HKEY_LOCAL_MACHINE, ksRegODBCDSNToUse, "Server", "")
                sDatabase = RegRead(HKEY_LOCAL_MACHINE, ksRegODBCDSNToUse, "Database", "")
            End If
            ' If a DS or IC are not provided, SQL Server will use
            ' default values. We must abort if this happens, or we
            ' cannot tell what it's actually connected to.
            If sProvider = "" Or sServer = "" Or sDatabase = "" Then
                Err.Raise knErrBadConnectionString, ksErrSource, ksErrBadConnectionString
            End If
            m_sOLEDBConnect = "Provider=" & sProvider & _
                ";Data Source=" & sServer & _
                ";Initial Catalog=" & sDatabase & _
                ";User ID=" & ksSALoginName & _
                ";Password=" & ksSALoginPassword & _
                ";OLE DB Services=-1"
        Else
            ' If string provided contains a username/password then use it.
            ' Otherwise, append the secret ones.
            iPosUserName = InStr(UCase$(m_sOLEDBConnect), "USER ID=")
            iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PASSWORD=")
            If iPosUserName = 0 Then
                iPosUserName = InStr(UCase$(m_sOLEDBConnect), "UID=")
            End If
            If iPosPassword = 0 Then
                iPosPassword = InStr(UCase$(m_sOLEDBConnect), "PWD=")
            End If
            If iPosUserName = 0 And iPosPassword = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";User ID=" & ksSALoginName & _
                    ";Password=" & ksSALoginPassword
            End If
            ' If string provided contains an OLEDBServices flag
            ' then use it. Otherwise, append the standard one.
            iPosFlags = InStr(UCase$(m_sOLEDBConnect), "OLE DB SERVICES=")
            If iPosFlags = 0 Then
                m_sOLEDBConnect = m_sOLEDBConnect & _
                    ";OLE DB Services=-1"
            End If
        End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMUnknown
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Assume DBMS type is unknown (before connection)
        ' and the full connection string is ALWAYS provided
        ' as a parameter.

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSwift6Version1
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Read DBMS type.
        Select Case RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "Engine", 0)
        Case 3
            m_nDBMSType = knDBMSMicrosoftSQLServer
        Case 2
            m_nDBMSType = knDBMSSybaseAdaptiveServer
        Case 1
            m_nDBMSType = knDBMSSybaseSQLAnywhere
        Case Else
            m_nDBMSType = knDBMSUnknown
        End Select

        ' Read data source name.
        m_sODBCDSN = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "ODBCDSN", "")
        If m_sODBCDSN = "" Then
            Err.Raise knErrNoODBCDSN, ksErrSource, ksErrNoODBCDSN
        End If

        ' Use the official login name and password if they have
        ' not been overridden by the calling program.
        If m_sODBCUID = "" Then
            m_sODBCUID = ksLoginName
            m_sODBCPWD = ksLoginPassword
        End If
        ' Build the connection string.
        m_sOLEDBConnect = "Provider=MSDASQL" & _
            ";DSN=" & m_sODBCDSN & _
            ";UID=" & m_sODBCUID & _
            ";PWD=" & m_sODBCPWD

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSwift6Original
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Read DBMS type.
        Select Case RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "Engine", 0)
        Case 3
            m_nDBMSType = knDBMSMicrosoftSQLServer
        Case 2
            m_nDBMSType = knDBMSSybaseAdaptiveServer
        Case 1
            m_nDBMSType = knDBMSSybaseSQLAnywhere
        Case Else
            m_nDBMSType = knDBMSUnknown
        End Select

        ' Read data source name.
        m_sODBCDSN = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "ODBCDSN", "")
        If m_sODBCDSN = "" Then
            Err.Raise knErrNoODBCDSN, ksErrSource, ksErrNoODBCDSN
        End If

        ' Build the full connection string based on the
        ' DBMS type.
        If m_sODBCUID = "" Then
            Select Case m_nDBMSType
            Case knDBMSMicrosoftSQLServer
                m_sODBCUID = "sa"
                m_sODBCPWD = ""
            Case knDBMSSybaseAdaptiveServer
                m_sODBCUID = "sa"
                m_sODBCPWD = ""
            Case knDBMSSybaseSQLAnywhere
                m_sODBCUID = "DBA"
                m_sODBCPWD = "SQL"
            Case Else
                ' Assume the login name and password are provided
                ' or defined in the DSN itself.
            End Select
        End If
        ' Build the connection string.
        m_sOLEDBConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN
        If m_sODBCUID <> "" Then
            m_sOLEDBConnect = m_sOLEDBConnect & ";UID=" & m_sODBCUID
        End If
        If m_sODBCPWD <> "" Then
            m_sOLEDBConnect = m_sOLEDBConnect & ";PWD=" & m_sODBCPWD
        End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSwift5Version1
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Read DBMS type.
        Select Case LCase$(IniRead(ksFileConfig, "General", "DatabaseType", ""))
        Case "mss"
            m_nDBMSType = knDBMSMicrosoftSQLServer
        Case "ase"
            m_nDBMSType = knDBMSSybaseAdaptiveServer
        Case "ssa"
            m_nDBMSType = knDBMSSybaseSQLAnywhere
        Case Else
            m_nDBMSType = knDBMSUnknown
        End Select

        ' Read data source name.
        m_sODBCDSN = IniRead(ksFileConfig, "General", "DatabaseName", "")
        If m_sODBCDSN = "" Then
            Err.Raise knErrNoODBCDSN, ksErrSource, ksErrNoODBCDSN
        End If

        ' Use the official login name and password if they have
        ' not been overridden by the calling program.
        If m_sODBCUID = "" Then
            m_sODBCUID = ksLoginName
            m_sODBCPWD = ksLoginPassword
        End If
        ' Build the connection string.
        m_sOLEDBConnect = "Provider=MSDASQL" & _
            ";DSN=" & m_sODBCDSN & _
            ";UID=" & m_sODBCUID & _
            ";PWD=" & m_sODBCPWD

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case knDAMSwift5Original
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Read DBMS type.
        Select Case LCase$(IniRead(ksFileConfig, "General", "DatabaseType", ""))
        Case "mss"
            m_nDBMSType = knDBMSMicrosoftSQLServer
        Case "ase"
            m_nDBMSType = knDBMSSybaseAdaptiveServer
        Case "ssa"
            m_nDBMSType = knDBMSSybaseSQLAnywhere
        Case Else
            m_nDBMSType = knDBMSUnknown
        End Select

        ' Read data source name.
        m_sODBCDSN = IniRead(ksFileConfig, "General", "DatabaseName", "")
        If m_sODBCDSN = "" Then
            Err.Raise knErrNoODBCDSN, ksErrSource, ksErrNoODBCDSN
        End If

        ' Build the full connection string based on the
        ' DBMS type.
        If m_sODBCUID = "" Then
            Select Case m_nDBMSType
            Case knDBMSMicrosoftSQLServer
                m_sODBCUID = "sa"
                m_sODBCPWD = ""
            Case knDBMSSybaseAdaptiveServer
                m_sODBCUID = "sa"
                m_sODBCPWD = ""
            Case knDBMSSybaseSQLAnywhere
                m_sODBCUID = "DBA"
                m_sODBCPWD = "SQL"
            Case Else
                ' Assume the login name and password are provided
                ' or defined in the DSN itself.
            End Select
        End If
        ' Build the connection string.
        m_sOLEDBConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN
        If m_sODBCUID <> "" Then
            m_sOLEDBConnect = m_sOLEDBConnect & ";UID=" & m_sODBCUID
        End If
        If m_sODBCPWD <> "" Then
            m_sOLEDBConnect = m_sOLEDBConnect & ";PWD=" & m_sODBCPWD
        End If

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Case Else
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Catch any methods that fall through the net by mistake.
        ' There should not be any.
        Err.Raise knErrBadDAM, ksErrSource, ksErrBadDAM

    End Select

End Sub

' Open a connection using the official login name and password.
' If succeeded, return True.
' (Optional) If failed due to login not existing, return False.
' Otherwise, throw an abort error.
Private Function AttemptToLogin(ByVal bTrapNonExistentLogin As Boolean) As Boolean

    On Error GoTo EH_AttemptToLogin
    AttemptToLogin = False

    ' Open database connection.
    m_con.Open m_sOLEDBConnect
    ReVerifyDBMSType

    AttemptToLogin = True
    Exit Function

EH_AttemptToLogin:
    If bTrapNonExistentLogin Then
        With LastError()
            Select Case m_nDBMSType
            Case knDBMSMicrosoftSQLServer
                ' NativeError = 18456
                ' SQLState = "28000"
                ' Source = "ODBC"
                ' Description = "[Microsoft][ODBC SQL Server Driver][SQL Server]Login failed for user 'Swift'."
                If .NativeError = 18456 And .SQLState = "28000" Then
                    ' Trap this error and return False.
                    Exit Function
                End If
            Case knDBMSSybaseAdaptiveServer
                ' NativeError = 40002
                ' SQLState = "S1000"
                ' Source = "ODBC"
                ' Description = "[INTERSOLV][ODBC SQL Server driver][SQL Server]ct_connect(): protocol specific layer: external error: The attempt to connect to the server failed."
                If .NativeError = 40002 And .SQLState = "S1000" Then
                    ' Trap this error and return False.
                    Exit Function
                End If
                ' NativeError = 44
                ' SQLState = "S1000"
                ' Source = "ODBC"
                ' Description = "[INTERSOLV][ODBC SQL Server driver][SQL Server]ct_connect(): protocol specific layer: external error: The attempt to connect to the server failed."
                If .NativeError = 44 And .SQLState = "S1000" Then
                    ' Trap this error and return False.
                    Exit Function
                End If
                ' NativeError = 4002
                ' SQLState = "08001"
                ' Source = "ODBC"
                ' Description = "[INTERSOLV][ODBC SQL Server driver][SQL Server]Login failed."
                If .NativeError = 4002 And .SQLState = "08001" Then
                    ' Trap this error and return False.
                    Exit Function
                End If
            Case knDBMSSybaseSQLAnywhere
                ' NativeError = -2147217843
                ' SQLState = "28000"
                ' Source = "ODBC"
                ' Description = "[Sybase][ODBC Driver]Invalid user authorization specification: invalid userid or password"
                If .NativeError = -2147217843 And .SQLState = "28000" Then
                    ' Trap this error and return False.
                    Exit Function
                End If
            End Select
        End With
    End If
    ' All other errors are unexpected. Handle them here.
    If m_bDontTrapConnectionErrors Then
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    Select Case ErrorDialog(Me, "Cannot connect to the database.")
    Case erRetry
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Function

' Create the official login name and password.
' Throw an abort error if a database error is encountered.
Private Sub AttemptToCreateLogin()

    Dim sConnect As String
    Dim sDatabaseName As String

    On Error GoTo EH_AttemptToCreateLogin

    ' Build the full connection string based on the database
    ' engine. Allow the user to specify an "emergency" login
    ' in order to carry out this task.
    Select Case m_nDataAccessMethod
    Case knDAMSwift6Version1
        sConnect = RegRead(HKEY_LOCAL_MACHINE, ksRegPathProject, "EmergencyLogin", "")
    Case knDAMSwift5Version1
        sConnect = IniRead(ksFileConfig, "General", "EmergencyLogin", "")
    Case Else
        Exit Sub ' the other DAMs don't need this procedure
    End Select

    If sConnect <> "" Then
        sConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN & ";" & sConnect
    Else
        Select Case m_nDBMSType
        Case knDBMSMicrosoftSQLServer
            sConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN & ";UID=sa;PWD="
        Case knDBMSSybaseAdaptiveServer
            sConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN & ";UID=sa;PWD="
        Case knDBMSSybaseSQLAnywhere
            sConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN & ";UID=DBA;PWD=SQL"
        Case Else
            ' Assume the login name and password are defined
            ' in the DSN itself.
            sConnect = "Provider=MSDASQL;DSN=" & m_sODBCDSN
        End Select
    End If

    ' Open database connection.
    m_con.Open sConnect
    ReVerifyDBMSType

    ' Now we can create the official login.
    Select Case m_nDBMSType

    Case knDBMSMicrosoftSQLServer
        sDatabaseName = m_con.DefaultDatabase

        Execute "execute sp_addlogin " & _
            ToSQL(ksLoginName) & ", " & _
            ToSQL(ksLoginPassword) & ", " & _
            ToSQL(sDatabaseName)

        Execute "execute sp_addsrvrolemember " & _
            ToSQL(ksLoginName) & ", " & _
            ToSQL("sysadmin")

        Execute "execute sp_grantdbaccess " & _
            ToSQL(ksLoginName) & ", " & _
            ToSQL(ksLoginName)

        Execute "execute sp_addrolemember " & _
            ToSQL("db_owner") & ", " & _
            ToSQL(ksLoginName)

        Execute "execute sp_changedbowner " & _
            ToSQL(ksLoginName) & ", " & _
            "true"

        Execute "checkpoint"

    Case knDBMSSybaseAdaptiveServer
        sDatabaseName = m_con.DefaultDatabase

        Execute "execute sp_addlogin " & _
            ToSQL(ksLoginName) & ", " & _
            ToSQL(ksLoginPassword) & ", " & _
            ToSQL(sDatabaseName)

        Execute "execute sp_locklogin " & _
            ToSQL(ksLoginName) & ", " & _
            ToSQL("unlock")

        Execute "execute sp_role " & _
            ToSQL("grant") & ", " & _
            ToSQL("sa_role") & ", " & _
            ToSQL(ksLoginName)

        Execute "execute sp_role " & _
            ToSQL("grant") & ", " & _
            ToSQL("sso_role") & ", " & _
            ToSQL(ksLoginName)

        Execute "execute sp_role " & _
            ToSQL("grant") & ", " & _
            ToSQL("oper_role") & ", " & _
            ToSQL(ksLoginName)

        Execute "execute sp_role " & _
            ToSQL("grant") & ", " & _
            ToSQL("replication_role") & ", " & _
            ToSQL(ksLoginName)

        Execute "execute sp_changedbowner " & _
            ToSQL(ksLoginName) & ", " & _
            "true"

        Execute "checkpoint"

    Case knDBMSSybaseSQLAnywhere
        Execute "grant group to ""DBA"""

        Execute "checkpoint"

        Execute "grant connect to " & ksLoginName & _
            " identified by " & ksLoginPassword

        Execute "grant dba to " & ksLoginName

        Execute "grant resource to " & ksLoginName

        Execute "checkpoint"

        Execute "grant membership in group ""DBA"" to " & ksLoginName

        Execute "checkpoint"

    End Select

    ' Close database connection ready for re-connecting.
    m_con.Close
    Exit Sub

EH_AttemptToCreateLogin:
    Select Case ErrorDialog(Me, "Cannot create a secure connection to the database.", _
        nButtons:=ebRetryCancelIgnore)
    Case erRetry
        Resume
    Case erIgnore
        Resume Next
    Case Else
        Err.Raise knErrAbort
    End Select

End Sub

' Re-verify the DBMS type, this time by asking OLEDB itself.
' Refuse to continue if we don't recognise it (for safety).
' This is done by throwing an abort error.
Private Sub ReVerifyDBMSType()

    Select Case m_con.Properties("DBMS Name")
    Case "Microsoft SQL Server"
        m_nDBMSType = knDBMSMicrosoftSQLServer
    Case "SQL Server"
        m_nDBMSType = knDBMSSybaseAdaptiveServer
    Case "Sybase SQL Anywhere"
        m_nDBMSType = knDBMSSybaseSQLAnywhere
    Case "MS Jet"
        m_nDBMSType = knDBMSMicrosoftJet
    Case "TSODBC"
        m_nDBMSType = knDBMSTranSoftCobol
    Case Else
        ErrorDialog Me, "Unrecognised database server type.", _
            nButtons:=ebOK
        Err.Raise knErrAbort
    End Select

End Sub

' Accurately detects the presence of a deadlock error.
Private Function IsDeadlockError() As Boolean

    IsDeadlockError = LastErrorWas(1205, "40001")

End Function

' Accurately detects the presence of a timeout error.
Private Function IsTimeoutError() As Boolean

    IsTimeoutError = LastErrorWas(0, "HYT00")

End Function

Public Sub PromptUserForNewTimeout()

    Const kdMinutesInHour = 60#

    Const kdMinValue = 0#
    Const ksMinValue = "zero"
    Const kdMaxValue = 24# * 60#
    Const ksMaxValue = "24 hours (1440 minutes)"

    Const ksTitle = "Change Timeout Value"
    Const ksPrompt1 = "Please enter the number of minutes that the program should wait " & _
        "for the database statement to complete. This timeout is currently set to "
    Const ksPrompt2 = " minutes. If you click the 'Cancel' button then the timeout " & _
        "will remain unchanged. Please note that the new timeout you set here will " & _
        "be used for ALL subsequent database actions until the application shuts down."

    Dim sDefault As String
    Dim sResult As String
    Dim dResult As Double

    On Error GoTo EH_Handler

EX_Retry:
    ' Prompt the user for a new timeout to use. For ease of use,
    ' this will be entered as minutes rather than seconds.
    sDefault = ToString(Round(ToDouble(m_con.CommandTimeout) / kdMinutesInHour, 3))
    Do
        sResult = Trim$(InputBox(ksPrompt1 & sDefault & ksPrompt2, ksTitle, sDefault))
        If sResult = "" Then
            Exit Do
        End If
        dResult = ToDouble(sResult)
        If dResult <= kdMinValue Then
            MsgBox "The timeout value cannot be less than or equal to " & ksMinValue & ". " & _
                "Click the 'Cancel' button to leave the timeout value unchanged.", _
                vbExclamation, ksTitle
        ElseIf dResult > kdMaxValue Then
            MsgBox "The timeout value cannot be greater than " & ksMaxValue & ", for safety reasons. " & _
                "Click the 'Cancel' button to leave the timeout value unchanged.", _
                vbExclamation, ksTitle
        Else
            m_con.CommandTimeout = Round(dResult * kdMinutesInHour, 0)
            Exit Do
        End If
    Loop

    Exit Sub

EH_Handler:
    Select Case ErrorDialog(Me, "Cannot change the database timeout value.")
    Case erRetry
        Resume EX_Retry ' this is more natural
    Case Else
        Exit Sub
    End Select

End Sub

Public Function BucketURNGetNext(ByVal sBucketCode As String) As Long

    Const knErrSSADeadlock = -306
    Const ksErrSSADeadlock = "Cannot create a new record." & vbCrLf & _
        "This is because another user is creating a new record of the same type, " & _
        "right at this moment. " & _
        "If you re-try the operation it may work this time." & vbCrLf & _
        "As a very last resort, you could abandon the process. " & _
        "You will LOSE ALL THE INFORMATION that you have entered, " & _
        "but it will allow you to continue with your work."

    Dim dtTimeout As Date
    Dim rs As ADODB.Recordset
    Dim vlID As Variant ' Long or Null

    Select Case m_nDataAccessMethod

    Case knDAMSwift6Version2, knDAMSwift6Version1, knDAMSwift6Original
        ' If this procedure has never been called, then generate a
        ' command and cache it for the lifetime of the database object.
        If m_procBucketURNGetNext Is Nothing Then
            On Error GoTo EH_CreateProcedure
            Set m_procBucketURNGetNext = NewCommand("BucketURNGetNext", True)
            With m_procBucketURNGetNext
                .Parameters.Append .CreateParameter("BucketCode", adVarChar, adParamInput, 40)
                .Parameters.Append .CreateParameter("ReturnID", adInteger, adParamInputOutput)
            End With
        End If

        ' Now that we have a command object, call it to obtain an ID value.
        On Error GoTo EH_ExecuteProcedure
        BucketURNGetNext = 0
        dtTimeout = DateAdd("s", m_lURNTimeout, Now())

        m_procBucketURNGetNext("BucketCode") = sBucketCode
        m_procBucketURNGetNext("ReturnID") = 0
        m_procBucketURNGetNext.Execute Options:=adExecuteNoRecords
        vlID = m_procBucketURNGetNext("ReturnID")

        If IsNull(vlID) Then
            Err.Raise knErrURNNull, ksErrSource, ksErrURNNull
        ElseIf vlID = 0 Then
            Err.Raise knErrURNZero, ksErrSource, ksErrURNZero
        Else
            BucketURNGetNext = vlID
        End If

    Case knDAMSwift5Version1, knDAMSwift5Original
        ' Call the stored procedure to obtain an ID value.
        On Error GoTo EH_ExecuteProcedure
        BucketURNGetNext = 0
        dtTimeout = DateAdd("s", m_lURNTimeout, Now())

        Set rs = OpenRecordset("execute BucketURNGetNext " & ToSQL(sBucketCode))
        vlID = rs(0)
        rs.Close

        BucketURNGetNext = vlID

    Case Else
        Err.Raise knErrBadDAM, ksErrSource, ksErrBadDAM

    End Select

    Exit Function

EH_CreateProcedure:
    ErrorDialog Me, "Cannot initialise the ID number generator. " & _
        "You will not be able to create new records in the database until this problem is resolved.", _
        bCritical:=True, _
        nButtons:=ebOK
    Err.Raise knErrAbort

EH_ExecuteProcedure:
    ' If we are waiting for another connection to finish getting a
    ' URN, SASE and MSS wait indefinitely to continue. However,
    ' SSA aborts with an error straight away. We therefore have to
    ' trap this and simulate an indefinite wait with timeout
    ' facility.
    If LastError().Number = knErrSSADeadlock Then
        ' If we have a locking error, retry continually until the
        ' timeout expires. Then we ask user for permission to retry.
        ' If user cancels, we abort with an error.
        If Now() < dtTimeout Then
            Resume
        Else
            ' Show message box.
            If MsgBox(ksErrSSADeadlock, _
                vbExclamation + vbRetryCancel, _
                "Creating New Records") = vbRetry Then
                ' Restart timer.
                dtTimeout = DateAdd("s", m_lURNTimeout, Now())
                Resume
            End If
        End If
    End If
    ' Generate a proper error message and give the user a chance to
    ' retry.
    Select Case ErrorDialog(Me, "Cannot create new ID for """ & sBucketCode & """. " & _
        "If you re-try the operation it may work this time.")
    Case erRetry
        ' Restart timer for SSA's benefit.
        dtTimeout = DateAdd("s", m_lURNTimeout, Now())
        Resume
    Case Else
        Err.Raise knErrAbort
    End Select

End Function


