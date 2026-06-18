Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
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
    ' JH061098 - Data Merge changes
    '
    ' JH260399 - ability to use multiple history roots, added
    ' the m_sHistoryRoot at the end of the caption so user
    ' sees it switching.
    '
    ' DN270802 - Change embedded SQL to reflect table changes
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)

    ' PUBLIC Data Members (End)

    Private Const m_cRebuildRemoteDB As String = "RebuildRemoteDB"

    ' PRIVATE Data Members (Begin)
    Private m_sHistoryRoot As String = ""

    ' flag used in rebuilding of the remote history database
    Private m_bRebuild As Boolean

    'Denotes we are running in accelerated mode - ie zippier ADDINDEX logic
    Private m_bAccelerated As Boolean

    'For login messages to PMB log
    Private m_sLogMess() As String

    'file handles
    Private fhJournalErrorlog As Integer
    Private fhErrorlog As Integer
    Private fhJournalBad As Integer
    Private g_iIndexData As Integer

    Private g_sCurrentJournalPos As String = ""
    Private g_iExternalHDB As Integer
    Private g_iIndexAccessLevel As Integer
    Private objfrmInterface As New frmInterface

    'file process flags...
    Private Const PM_NONE As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
    Private Const PM_NORMAL As gPMConstants.PMEReturnCode = 2
    Private Const PM_RECOVER As gPMConstants.PMEReturnCode = 3
    Private Const PM_ERRRETRY As gPMConstants.PMEReturnCode = 4

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    ' Install directory
    Private m_sDMEDIR As String = ""

    ' Instance of Log Object
    Private m_oLog As Object

    'Generic API object
    'Private m_oAPI As bDOCAPI.API
    Private m_oAPI As bDOCAPI.API 'RAM20030102 : Change it for late binding

    'PMB Message logging object
    Private m_oPMBLog As Object

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceID As Integer

    ' JH071098 log file name
    Private m_sLogFileName As String = ""

    ' DMS History API routines.
    'Declare Function GetPrivateProfileInt Lib "kernel32" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Long, ByVal lpFileName As String) As Long
    Private Declare Sub NewCab Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub NewDrw Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub NewFld Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub NewDoc Lib "bDOChstry.dll" (ByVal a As String)

    Private Declare Sub DelCab Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub DelDrw Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub DelFld Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub DelDoc Lib "bDOChstry.dll" (ByVal a As String)

    Private Declare Sub ModCab Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub ModDrw Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub ModFld Lib "bDOChstry.dll" (ByVal a As String)
    Private Declare Sub ModDoc Lib "bDOChstry.dll" (ByVal a As String)

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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername = sUserName
            g_sPassword.Value = sPassword

            ' Set UserID
            g_iUserID = iUserID

            ' Set Calling Application
            g_sCallingAppName = sCallingAppName

            ' Set Language ID
            g_iLanguageID = iLanguageID

            ' Set Source ID
            g_iSourceID = iSourceID

            ' Set Currency ID
            g_iCurrencyID = iCurrencyID

            ' Set Log Level
            g_iLogLevel = iLogLevel

            ' Have we a valid Database Object Reference?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New dPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If


            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            'Get the DocuMaster Generic API object reference for writing to the DB

            'Set m_oAPI = New bDOCAPI.API
            m_oAPI = New bDOCAPI.API() ' RAM20030102 : Change it for late binding

            m_lReturn = m_oAPI.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF - Create Log Object
            m_oLog = New bDOCPMBLog.Log()


            m_lReturn = m_oLog.Initialise(sUserName:=g_sUsername)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_bRebuild = False

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
                m_oAPI = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub




    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    Function GetJournalPos(ByRef sJournalName As String) As String

        Dim result As String = String.Empty
        Dim JournalPos As Integer
        Dim sJournalPos As String = ""

        result = ""

        Try

            ' Find free file number and open
            JournalPos = FileSystem.FreeFile()
            FileSystem.FileOpen(JournalPos, m_sHistoryRoot & "data\" & sJournalName, OpenMode.Input)

            ' Read current journal position
            sJournalPos = FileSystem.LineInput(JournalPos)

            'Close journal file
            FileSystem.FileClose(JournalPos)

            Return sJournalPos.Trim()

        Catch



            JournalErrorLog("GetJournalPos", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return ""
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    Function CheckJournalFile() As Integer

        'This function will check for the existance of work files.
        'If work files exist it indicates that the API fell over while processing.
        '
        'If a journal.rty exists the the API fell over while retying import,
        'otherwise it fell over during normal processing...

        Dim sJournalName, sJournalCurrentName As String

        Try

            ' Check if journal.cur exists
            sJournalCurrentName = FileSystem.Dir(m_sHistoryRoot & "data\journal.cur", FileAttribute.Normal)

            If sJournalCurrentName <> "" Then
                g_sCurrentJournalPos = GetJournalPos(sJournalCurrentName)
            Else
                g_sCurrentJournalPos = ""
            End If

            ' Does journal.rty already exist, were we processing the retry's ??
            'sJournalName = Dir$(m_sHistoryRoot & "data\journal.rty", vbNormal)

            If FileSystem.Dir(m_sHistoryRoot & "data\journal.rty", FileAttribute.Normal) <> "" Then

                '    If (sJournalName <> "") Then
                ' Recover from failure
                '
                Return PM_ERRRETRY

            ElseIf FileSystem.Dir(m_sHistoryRoot & "data\journal.wrk", FileAttribute.Normal) <> "" Then

                ' Does journal.wrk already exist, did we crash  ??
                'sJournalName = Dir$(m_sHistoryRoot & "data\journal.wrk", vbNormal)

                'If (sJournalName <> "") Then
                ' Recover from failure
                '
                '            ' Does journal.dms exist
                '            sJournalName = Dir$(m_sHistoryRoot & "data\journal.dms", vbNormal)

                Return PM_RECOVER

                'End If
            Else

                ' Does journal.dms exist
                sJournalName = FileSystem.Dir(m_sHistoryRoot & "data\journal.dms", FileAttribute.Normal)

                If sJournalName.Trim() = "" Then
                    ' Journal.dms doesn't exist
                    Return PM_NONE
                Else
                    Return PM_NORMAL
                End If

            End If

        Catch
        End Try



        Return gPMConstants.PMEReturnCode.PMFalse

    End Function

    ' PRIVATE Methods (End)

    ' ***************************************************************** '
    ' Name: OpenJournalErrorLog (Private)
    '
    ' Description: Opens journal error log on PMB system
    '
    ' ***************************************************************** '
    Private Function OpenJournalErrorLog() As Integer


        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue



        fhJournalErrorlog = FileSystem.FreeFile()
        FileSystem.FileOpen(fhJournalErrorlog, m_sHistoryRoot & "data\journal.log", OpenMode.Append)

        Return result

    End Function

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
        'iPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Sub Start(ByRef oPMBLog As Object, ByRef bAccelerated As Boolean, ByRef bRetryImports As Boolean, ByRef bRetryExports As Boolean)


        Try

            'store log message object reference and the progress form reference
            m_oPMBLog = oPMBLog

            m_lReturn = GetDOCRegSettings(vHistoryRoot:=m_sHistoryRoot)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set accelerated mode flag
            m_bAccelerated = bAccelerated

            'initialise cancelled and paused flags
            g_bCancelProcessing = False
            g_bPauseProcessing = False

            'Instance the progess interface
            objfrmInterface.Show()
            objfrmInterface.fraProgress.Text = "Importing Data from " & m_sHistoryRoot

            'Get the DME install directory
            If m_sDMEDIR.Trim() = "" Then
                m_lReturn = GetDMEDIR(m_sDMEDIR)
            End If

            ' Call main process
            ProcessMain(False)

            'update progress form
            objfrmInterface.fraProgress.Text = "Exporting Data to " & m_sHistoryRoot
            objfrmInterface.panCurrentFile.Visible = False

            'Run the update history function
            If Not (CommitHDB(False)) Then
                '     Something must have gone wrong, but
                '     we won 't bother to report it at the moment.
            End If


            ' Check if any errors to retry
            ' RI=Retry Import, RE=Retry Export, RB=Retry Both

            If bRetryImports Then

                'update progress form
                objfrmInterface.fraProgress.Text = "Retrying Import Errors from " & m_sHistoryRoot
                objfrmInterface.panCurrentFile.Visible = True

                ProcessMain(True)
            End If

            If bRetryExports Then

                'update progress form
                objfrmInterface.fraProgress.Text = "Retrying Export Errors from " & m_sHistoryRoot
                objfrmInterface.panCurrentFile.Visible = False

                If Not (CommitHDB(True)) Then
                    ' Something must have gone wrong, but
                    ' we won't bother to report it at the moment.
                End If
            End If

            'JH260399 swap the history root drive if necessary

            m_lReturn = SwapHistoryRoot()

            'trash the progress interface
            objfrmInterface.Close()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub ProcessMain(ByRef iRetryErrors As Integer)

        Dim sJournalData(0) As String
        Dim iStatus As gPMConstants.PMEReturnCode
        Dim iOK As Integer




        ' Open the Journal error log file
        m_lReturn = OpenJournalErrorLog()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Check status of journal work files...
        iStatus = CType(CheckJournalFile(), gPMConstants.PMEReturnCode)
        Select Case iStatus
            Case PM_RECOVER
                ' Journal.wrk exists, we need to recover from a previous attempt
                If GetJournalData(sJournalData, False) Then 'True = retry ...
                    If Not (ProcessJournalData(sJournalData, False)) Then
                        ' Failed
                    End If
                End If

            Case PM_NONE
                'No .DMS or .WRK file exists, there's nothing to do !!!
                'unless we need to retry errors...
                If iRetryErrors Then
                    ' Check if journal.bad exist's
                    If CheckJournalBadFile() Then
                        ' Journal.bad exists
                        If RenameJournalBadFile() Then
                            ' Renamed journal.bad file to journal.rty,
                            ' do the biz
                            '                If Not (OpenJournalBad()) Then
                            '                    ' Failed to open journal error file
                            '                    Debug.Print "Failed to open Journal error log"
                            '                Else
                            If GetJournalData(sJournalData, True) Then
                                If Not (ProcessJournalData(sJournalData, True)) Then
                                    ' Failed
                                End If
                            End If
                            '                End If
                        End If
                    End If
                End If

            Case PM_ERRRETRY
                'we need to recover processing the .rty file
                If GetJournalData(sJournalData, True) Then 'True = retry ...
                    If Not (ProcessJournalData(sJournalData, True)) Then
                        ' Failed
                    End If
                End If

            Case PM_NORMAL
                'All is OK, we can process as normal

                iOK = True
                If iRetryErrors Then
                    ' Check if journal.bad exist's
                    If CheckJournalBadFile() Then
                        ' Journal.bad exists
                        If Not (RenameJournalBadFile()) Then
                            ' Failed to rename journal.bad file to journal.rty,
                            ' see you later
                            iOK = False
                        End If
                        '            If Not (OpenJournalBad()) Then
                        '                ' Failed to open journal error file
                        '                Debug.Print "Failed to open Journal error log"
                        '                iOK = False
                        '            End If
                    Else
                        ' No journal.bad exists
                        iOK = False
                    End If

                Else
                    ' Journal.wrk doesn't exist, we are
                    ' safe to continue
                    If RenameJournalFile() Then
                        If Not (CheckLockFile()) Then
                            ' Failed, something went wrong
                            iOK = False
                        End If
                    End If
                End If

                If iOK Then
                    ' Open the Journal bad file
                    If GetJournalData(sJournalData, iRetryErrors) Then
                        If Not (ProcessJournalData(sJournalData, iRetryErrors)) Then
                            ' Failed
                        End If
                    End If
                End If

            Case gPMConstants.PMEReturnCode.PMFalse
                'OOOOOPS, somthing went wrong
        End Select

        ' Close the Journal error log file
        CloseJournalErrorLog()

        ' Close the Journal bad file
        'CloseJournalBad


    End Sub
    Private Function CheckJournalBadFile() As Integer

        Dim sJournalName As String = ""

        Try

            ' Does journal.bad exist
            sJournalName = FileSystem.Dir(m_sHistoryRoot & "data\journal.bad", FileAttribute.Normal)


            Return sJournalName <> ""

        Catch
        End Try



        Return False

    End Function
    Private Function RenameJournalBadFile() As Integer

        Dim result As Integer = 0
        result = True



        ' Rename journal.bad to journal.rty
        FileSystem.Rename(m_sHistoryRoot & "data\journal.bad", m_sHistoryRoot & "data\journal.rty")

        Return result


    End Function
    Function RenameJournalFile() As Integer

        Dim result As Integer = 0
        result = True

        Try

            ' Rename journal.dms to journal.wrk
            If Rename(m_sHistoryRoot & "data\journal.dms", m_sHistoryRoot & "data\journal.wrk") = gPMConstants.PMEReturnCode.PMFalse Then
                JournalErrorLog("RenameJournalFile", m_sHistoryRoot & "data\journal.dms" & " to " & m_sHistoryRoot & "data\journal.wrk" & "Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
                result = False
            End If

            Return result

        Catch



            JournalErrorLog("RenameJournalFile", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function

    Function Rename(ByRef sFrom As String, ByRef sTo As String) As Integer

        Try

            FileSystem.Rename(sFrom, sTo)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function


    Function CheckLockFile() As Integer

        Dim result As Integer = 0
        Dim sLockName, sJournalName As String

        result = True
        Try

            For iWaitLoop As Integer = 1 To 2
                ' Does the lock.dms file exist
                sLockName = FileSystem.Dir(m_sHistoryRoot & "data\lock.dms", FileAttribute.Normal)

                If sLockName <> "" Then
                    ' Lock file exists
                    '
                    ' Check if the journal.dms file exists
                    sJournalName = FileSystem.Dir(m_sHistoryRoot & "data\journal.dms", FileAttribute.Normal)

                    If sJournalName = "" Then
                        ' Journal.dms file doesn't exist
                        '
                        ' Wait 10 seconds, and try again
                        pause(10)
                    Else
                        ' Journal.dms file exists
                        Exit For
                    End If
                Else
                    ' lock.dms file doesn't exist
                    Exit For
                End If
            Next iWaitLoop

            Return result

        Catch



            JournalErrorLog("CheckLockFile", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
            Return False
        End Try

    End Function
    Private Sub pause(ByRef iNumberSeconds As Integer)



        For iTmp As Integer = 1 To iNumberSeconds

            For iTmp2 As Integer = 0 To 25000
            Next iTmp2

            Application.DoEvents()
        Next iTmp

    End Sub
    Private Function ProcessJournalData(ByRef sJournalData() As String, ByRef iRetryErrors As Integer) As Integer

        Dim sControlName As String = ""


        Dim ReturnOK As Integer = True



        ReDim m_sLogMess(1)

        'Update progress form
        If sJournalData.GetUpperBound(0) = 1 Then
            objfrmInterface.proProgress.Minimum = 0
            objfrmInterface.proProgress.Maximum = sJournalData.GetUpperBound(0)
        ElseIf (sJournalData.GetUpperBound(0) > 1) Then
            objfrmInterface.proProgress.Minimum = 1
            objfrmInterface.proProgress.Maximum = sJournalData.GetUpperBound(0)
        End If

        ' Process all control files
        For iCntr As Integer = 1 To sJournalData.GetUpperBound(0)

            Application.DoEvents()

            sControlName = m_sHistoryRoot & "tmp\" & sJournalData(iCntr) & ".ctl"

            'Update progress form

            'To Do
            'objfrmInterface.panCurrentFile.Caption = "Currently Processing Control file " & sJournalData(iCntr) & ".ctl"

            ' Open Error log for control file
            If OpenErrorLog(sJournalData(iCntr)) Then
                ' Update current journal control position
                UpdateCurrentPos(sJournalData(iCntr))

                'salvo - Check if cancel has been chosen, in which case leave neatly and promptly
                '       (leave a .cur behind and everything as it is for resumption next API run)
                If g_bCancelProcessing Then


                    m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to log message
                        ErrorLog("ProcessJournalData", "Failed to open log file")
                        Return ReturnOK
                    End If

                    m_sLogMess(1) = "DocuMaster API Worker Daemon - Processing Cancelled"

                    m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)


                    m_oPMBLog.CloseLogFile(m_sHistoryRoot)

                    Return ReturnOK
                End If

                'Check if daemon has been paused, in which case pause.
                If g_bPauseProcessing Then
                    MessageBox.Show("You have paused the API Daemon." & Strings.Chr(10).ToString() & "Click OK to continue.", "DocuMaster API Daemon Paused.")
                    g_bPauseProcessing = False
                End If

                If ProcessControlData(sControlName) Then

                    ' Delete current control journal position file
                    If Not (DeleteCurrentPos()) Then
                        Debug.WriteLine("Failed to delete journal position file")
                    End If

                    ' Delete control files
                    If Not (DeleteControlFiles(sJournalData(iCntr))) Then
                        ' Failed to delete control files
                        Debug.WriteLine("Failed to delete control files")
                    End If

                    ' Close Error log for control file
                    CloseErrorLog(sJournalData(iCntr))

                    ' Delete control log file, if one exists
                    DeleteErrorLog(sJournalData(iCntr))
                Else

                    ' Update the journal bad file
                    If Not (UpdateJournalBad(sJournalData(iCntr))) Then
                        ' Failed to write bad control file
                        Debug.WriteLine("Failed to write bad control file")
                    End If

                    ' Delete current control journal position file
                    If Not (DeleteCurrentPos()) Then
                        Debug.WriteLine("Failed to delete journal position file")
                    End If

                    ' Append a err message to todays log file

                    m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to log message
                        ErrorLog("ProcessJournalData", "Failed to open log file")
                        Exit Function
                    End If

                    m_sLogMess(1) = "DocuMaster API Daemon - Error occurred in the control file, " & sJournalData(iCntr)

                    m_oPMBLog.DOCiPMFunc.LogMessage(LERR, "DMSAPI", m_sLogMess)


                    m_oPMBLog.CloseLogFile(m_sHistoryRoot)

                    ReturnOK = False

                    ' Close Error log for control file
                    CloseErrorLog(sJournalData(iCntr))
                End If
            Else
                ' Failed to open error log
                Debug.WriteLine("Failed to open error log file")
            End If

            objfrmInterface.proProgress.Value = iCntr

            Application.DoEvents()
        Next iCntr

        objfrmInterface.proProgress.Value = objfrmInterface.proProgress.Maximum

        If Not (DeleteJournalFile(iRetryErrors)) Then
            ' Failed to delete journal.wrk/rty file
            Debug.WriteLine("Failed to delete the journal.wrk/rty file")
        End If

        Return ReturnOK


    End Function
    Private Function DeleteJournalFile(ByRef iRetryErrors As Integer) As Integer

        Dim result As Integer = 0
        result = True



        If iRetryErrors Then
            If KillFile(m_sHistoryRoot & "data\journal.rty") = gPMConstants.PMEReturnCode.PMFalse Then
                ErrorLog("DeleteJournalFile - ", m_sHistoryRoot & "data\journal.rty. Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
                result = False
            End If
        Else
            If KillFile(m_sHistoryRoot & "data\journal.wrk") = gPMConstants.PMEReturnCode.PMFalse Then
                ErrorLog("DeleteJournalFile - ", m_sHistoryRoot & "data\journal.wrk. Failed on error, " & CStr(Information.Err().Number) & ": " & Conversion.ErrorToString())
                result = False
            End If

        End If

        Return result


    End Function


    Private Sub CloseJournalErrorLog()

        Try

            If FileSystem.LOF(fhJournalErrorlog) = 0 Then
                FileSystem.FileClose(fhJournalErrorlog)
                If KillFile(m_sHistoryRoot & "data\journal.log") = gPMConstants.PMEReturnCode.PMFalse Then
                End If
            Else
                FileSystem.FileClose(fhJournalErrorlog)
            End If

        Catch



            Exit Sub
        End Try


    End Sub
    Private Function CloseIndexListData() As Integer

        Dim result As Integer = 0
        result = True

        Try

            ' Close journal file
            FileSystem.FileClose(g_iIndexData)

            Return result

        Catch



            Return False
        End Try

    End Function


    Sub CloseJournalBad()

        Try

            If FileSystem.LOF(fhJournalBad) = 0 Then
                FileSystem.FileClose(fhJournalBad)

                If KillFile(m_sHistoryRoot & "data\journal.bad") = gPMConstants.PMEReturnCode.PMFalse Then
                End If
            Else
                FileSystem.FileClose(fhJournalBad)
            End If

        Catch



            Exit Sub
        End Try


    End Sub


    Private Function KillFile(ByRef sFile As String) As Integer


        Try

            File.Delete(sFile)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function
    Private Sub CloseErrorLog(ByRef ControlName As String)

        Try

            If FileSystem.LOF(fhErrorlog) = 0 Then
                FileSystem.FileClose(fhErrorlog)
                If KillFile(m_sHistoryRoot & "tmp\" & ControlName & ".log") = gPMConstants.PMEReturnCode.PMFalse Then
                End If
            Else
                FileSystem.FileClose(fhErrorlog)
            End If

        Catch



            Exit Sub
        End Try


    End Sub

    Private Sub DeleteErrorLog(ByRef ControlName As String)



        File.Delete(m_sHistoryRoot & "tmp\" & ControlName & ".log")



    End Sub


    Private Function UpdateJournalBad(ByRef sControlName As String) As Integer

        Dim result As Integer = 0
        Dim sJournalPos As String = ""

        result = True



        ' Report that there is a bad control file

        If OpenJournalBad() = gPMConstants.PMEReturnCode.PMTrue Then

            ' Write current bad control file
            FileSystem.PrintLine(fhJournalBad, sControlName)

            CloseJournalBad()
        Else

            JournalErrorLog("UpdateJournalBad", "Failed to Open Journal.bad")
            result = False

        End If

        Return result


    End Function
    Private Function OpenJournalBad() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            fhJournalBad = FileSystem.FreeFile()
            FileSystem.FileOpen(fhJournalBad, m_sHistoryRoot & "data\journal.bad", OpenMode.Append)

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function


    Private Function DeleteCurrentPos() As Integer



        File.Delete(m_sHistoryRoot & "data\journal.cur")

        Return True


    End Function
    Private Sub ErrorLog(ByRef FunctionName As String, ByRef ErrorMessage As String)

        Try

            FileSystem.PrintLine(fhErrorlog, DateTime.Now.ToString("dd/MM/yy  HH:MM:ss") & " - " & "Func: " & FunctionName & ", " & ErrorMessage)

        Catch

            'err.description
            Exit Sub
        End Try


    End Sub
    Private Function DeleteControlFiles(ByRef sControlName As String) As Integer

        Dim result As Integer = 0
        Dim utControlData As MainModule.g_utControlData = MainModule.g_utControlData.CreateInstance()
        Dim sTmpControlName As String = ""

        result = True



        sTmpControlName = m_sHistoryRoot & "tmp\" & sControlName & ".ctl"

        If Not (GetControlData(sTmpControlName, utControlData)) Then
            ErrorLog("DeleteControlFiles", "Failed to get control data")
            Return False
        End If

        Select Case (utControlData.task)
            Case "ADD", "LOG"
                If KillFile(m_sHistoryRoot & "tmp\" & sControlName & ".ctl") = gPMConstants.PMEReturnCode.PMFalse Then
                    ErrorLog("DeleteControlFile" & m_sHistoryRoot & "tmp\" & sControlName & ".ctl", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
                    result = False
                End If

            Case "ADDINDEX", "DELINDEX", "MERGEINDEX"
                If KillFile(m_sHistoryRoot & "tmp\" & sControlName & ".ctl") = gPMConstants.PMEReturnCode.PMFalse Then
                    ErrorLog("DeleteControlFile" & m_sHistoryRoot & "tmp\" & sControlName & ".ctl", "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
                    result = False
                End If
                If KillFile(utControlData.filename) = gPMConstants.PMEReturnCode.PMFalse Then
                    ErrorLog("DeleteControlFile" & (utControlData.filename), "Failed on error, " & Information.Err().Number & ": " & Conversion.ErrorToString())
                    result = False
                End If
        End Select

        Return result


    End Function

    Private Function GetControlData(ByRef sControlName As String, ByRef utControlData As MainModule.g_utControlData) As Integer

        Dim result As Integer = 0
        Dim iDataLen As Integer
        Dim sTmpString As String = ""

        result = True



        ' TASK
        utControlData.task = New String(" "c, 128)
        Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi("TASK")
        Try
            iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr, "", utControlData.task, 128, sControlName)
        Finally
            Marshal.FreeHGlobal(tmpPtr)
        End Try
        utControlData.task = utControlData.task.Substring(0, iDataLen).Trim()

        If iDataLen = 0 Then
            ErrorLog("GetControlData", "Failed to get TASK from control file, " & sControlName)
            result = False

        End If

        g_iExternalHDB = True

        Dim sTmp As String = ""
        Select Case utControlData.task.ToUpper()
            Case "ADD", "POPUP"
                'Is the call using internal or external references (external as default)
                sTmp = New String(" "c, 128)
                Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi("EXTERN")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr2, "", sTmp, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr2)
                End Try
                sTmp = sTmp.Substring(0, iDataLen).Trim()

                If iDataLen = 0 Then
                    utControlData.external = True
                    g_iExternalHDB = True
                Else
                    If sTmp.ToUpper().IndexOf("FALSE") + 1 Then
                        utControlData.external = False
                        g_iExternalHDB = False
                    Else
                        utControlData.external = True
                        g_iExternalHDB = True
                    End If
                End If

                ' CABINET
                utControlData.cabinetname = New String(" "c, 128)
                Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi("CABINET")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr3, "", utControlData.cabinetname, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr3)
                End Try
                utControlData.cabinetname = utControlData.cabinetname.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get CABINET from control file, " & sControlName)
                    result = False
                End If

                ' DRAWER
                utControlData.drawername = New String(" "c, 128)
                Dim tmpPtr4 As IntPtr = Marshal.StringToHGlobalAnsi("DRAWER")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr4, "", utControlData.drawername, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr4)
                End Try
                utControlData.drawername = utControlData.drawername.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get DRAWER from control file, " & sControlName)
                    result = False
                End If

                ' FOLDER
                utControlData.foldername = New String(" "c, 128)
                Dim tmpPtr5 As IntPtr = Marshal.StringToHGlobalAnsi("FOLDER")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr5, "", utControlData.foldername, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr5)
                End Try
                utControlData.foldername = utControlData.foldername.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get FOLDER from control file, " & sControlName)
                    result = False
                End If

                ' LINKFOLDER
                utControlData.linkfolder = New String(" "c, 128)
                Dim tmpPtr6 As IntPtr = Marshal.StringToHGlobalAnsi("LINKFOLDER")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr6, "", utControlData.linkfolder, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr6)
                End Try
                utControlData.linkfolder = utControlData.linkfolder.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    utControlData.linkfolder = ""
                End If

                ' DOCUMENT
                utControlData.documentname = New String(" "c, 128)
                Dim tmpPtr7 As IntPtr = Marshal.StringToHGlobalAnsi("DOCUMENT")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr7, "", utControlData.documentname, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr7)
                End Try
                utControlData.documentname = utControlData.documentname.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get DOCUMENT from control file, " & sControlName)
                    result = False
                End If

                ' KEYWORDS
                utControlData.keywords = New String(" "c, 128)
                Dim tmpPtr8 As IntPtr = Marshal.StringToHGlobalAnsi("KEYWORDS")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr8, "", utControlData.keywords, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr8)
                End Try
                utControlData.keywords = utControlData.keywords.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    utControlData.keywords = ""
                End If

                ' DOCTYPE
                utControlData.doctype = New String(" "c, 128)
                Dim tmpPtr9 As IntPtr = Marshal.StringToHGlobalAnsi("DOCTYPE")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr9, "", utControlData.doctype, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr9)
                End Try
                utControlData.doctype = utControlData.doctype.Substring(0, iDataLen).Trim().ToUpper()
                If iDataLen = 0 Then
                    utControlData.doctype = "TXT"
                End If

                ' EVENT
                utControlData.event_Renamed = New String(" "c, 128)
                Dim tmpPtr10 As IntPtr = Marshal.StringToHGlobalAnsi("EVENT")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr10, "", utControlData.event_Renamed, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr10)
                End Try
                utControlData.event_Renamed = utControlData.event_Renamed.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then

                    Select Case utControlData.doctype.ToUpper()
                        Case "TXT"
                            utControlData.event_Renamed = "L" 'letter
                        Case "RTF"
                            utControlData.event_Renamed = "R" 'Rich Text Document
                        Case "TIF"
                            utControlData.event_Renamed = "I" 'Image
                        Case Else
                            ErrorLog("GetControlData", "Failed to get EVENT from control file, " & sControlName)
                            result = False
                    End Select

                End If

                ' FILENAME
                utControlData.filename = New String(" "c, 128)
                Dim tmpPtr11 As IntPtr = Marshal.StringToHGlobalAnsi("FILENAME")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr11, "", utControlData.filename, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr11)
                End Try

                ' ... the API needs to append the Journal root to the file name
                utControlData.filename = m_sHistoryRoot & utControlData.filename.Substring(0, iDataLen).Trim()

                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get FILENAME from control file, " & sControlName)
                    result = False
                Else
                    utControlData.filename = ConvertSlashes(utControlData.filename)
                End If

                ' ANNOTATION
                utControlData.annotation = New String(" "c, 128)
                Dim tmpPtr12 As IntPtr = Marshal.StringToHGlobalAnsi("ANNOTATION")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr12, "", utControlData.annotation, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr12)
                End Try
                utControlData.annotation = utControlData.annotation.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    utControlData.annotation = ""
                End If

                ' USERNAME
                utControlData.username = New String(" "c, 128)
                Dim tmpPtr13 As IntPtr = Marshal.StringToHGlobalAnsi("USERNAME")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr13, "", utControlData.username, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr13)
                End Try
                utControlData.username = utControlData.username.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get USERNAME from control file, " & sControlName)
                    result = False
                Else
                    'sp todo - need new userexists function, in mean time dont do any verification
                    'If (UserExists((utControlData.username)) = -1) Then
                    ' User not found in system
                    '    utControlData.username = "DMSAPI"
                    'End If
                End If

                g_sUsername = utControlData.username
                utControlData.access = 9

            Case "ADDINDEX", "DELINDEX", "MERGEINDEX"
                ' FILENAME
                utControlData.filename = New String(" "c, 128)
                Dim tmpPtr14 As IntPtr = Marshal.StringToHGlobalAnsi("FILENAME")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr14, "", utControlData.filename, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr14)
                End Try

                utControlData.filename = m_sHistoryRoot & utControlData.filename.Substring(0, iDataLen).Trim()

                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get FILENAME from control file, " & sControlName)
                    result = False
                Else
                    utControlData.filename = ConvertSlashes(utControlData.filename)
                End If

                ' USERNAME
                utControlData.username = New String(" "c, 128)
                Dim tmpPtr15 As IntPtr = Marshal.StringToHGlobalAnsi("USERNAME")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr15, "", utControlData.username, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr15)
                End Try
                utControlData.username = utControlData.username.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get USERNAME from control file, " & sControlName)
                    result = False
                Else
                    'sp todo - need new userexists function, in mean time dont do any verification
                    '                If (UserExists((utControlData.username)) = -1) Then
                    '                    ' User not found in system
                    '                    utControlData.username = "DMSAPI"
                    '                End If

                End If

                utControlData.access = 9
                g_iIndexAccessLevel = utControlData.access
                g_sUsername = utControlData.username

                ' EMPTYONLY
                sTmpString = New String(" "c, 128)
                Dim tmpPtr16 As IntPtr = Marshal.StringToHGlobalAnsi("EMPTYONLY")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr16, "", sTmpString, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr16)
                End Try
                sTmpString = sTmpString.Substring(0, iDataLen).Trim()
                utControlData.emptyonly = Not (sTmpString = "FALSE")
                If iDataLen = 0 Then
                    utControlData.emptyonly = True
                End If

            Case "LOG"
                ' MESSAGE
                utControlData.message = New String(" "c, 128)
                Dim tmpPtr17 As IntPtr = Marshal.StringToHGlobalAnsi("MESSAGE")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr17, "", utControlData.message, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr17)
                End Try
                utControlData.message = utControlData.message.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get MESSAGE from control file, " & sControlName)
                    result = False
                End If

                ' USERNAME
                utControlData.username = New String(" "c, 128)
                Dim tmpPtr18 As IntPtr = Marshal.StringToHGlobalAnsi("USERNAME")
                Try
                    iDataLen = GetPrivateProfileString("DMSAPI", tmpPtr18, "", utControlData.username, 128, sControlName)
                Finally
                    Marshal.FreeHGlobal(tmpPtr18)
                End Try
                utControlData.username = utControlData.username.Substring(0, iDataLen).Trim()
                If iDataLen = 0 Then
                    ErrorLog("GetControlData", "Failed to get USERNAME from control file, " & sControlName)
                    result = False
                Else
                    'sp todo - need new userexists function, in mean time dont do any verification
                    '                If (UserExists((utControlData.username)) = -1) Then
                    '                    ' User not found in system
                    '                    utControlData.username = "DMSAPI"
                    '                End If

                    ' Get access level from user name
                    '                utControlData.access = UserExists((utControlData.username))
                    '                If (utControlData.access = -1) Then
                    ' User not found!
                    '                    utControlData.username = "DMSAPI"
                    '                    utControlData.access = 9
                    '                End If
                End If

                ' Default new items to lowest access level
                g_iIndexAccessLevel = 9 ' utControlData.access
                g_sUsername = utControlData.username
        End Select

        Return result

    End Function
    Private Function ConvertSlashes(ByRef sString As String) As String


        Dim sTmpStr As New StringBuilder

        For iCntr As Integer = 1 To sString.Length
            If sString.Substring(iCntr - 1, 1) = "/" Then
                sTmpStr.Append("\")
            Else
                sTmpStr.Append(sString.Substring(iCntr - 1, 1))
            End If
        Next iCntr

        Return sTmpStr.ToString()

    End Function


    Private Sub UpdateCurrentPos(ByRef sCurrentControl As String)

        Dim JournalPos As Integer
        Dim sJournalPos As String = ""



        ' Find free file number and open
        JournalPos = FileSystem.FreeFile()

        ' Check if file already exists
        sJournalPos = FileSystem.Dir(m_sHistoryRoot & "data\journal.cur", FileAttribute.Normal)

        '    If (sJournalPos <> "") Then
        '        Open m_sHistoryRoot & "data\journal.cur" For Append As #JournalPos
        '    Else
        FileSystem.FileOpen(JournalPos, m_sHistoryRoot & "data\journal.cur", OpenMode.Output)
        '    End If

        ' Write current journal position
        FileSystem.PrintLine(JournalPos, sCurrentControl)

        'Close journal file
        FileSystem.FileClose(JournalPos)



    End Sub

    Private Function OpenErrorLog(ByRef ControlName As String) As Integer

        Dim result As Integer = 0
        result = True

        Try

            fhErrorlog = FileSystem.FreeFile()
            FileSystem.FileOpen(fhErrorlog, m_sHistoryRoot & "tmp\" & ControlName & ".log", OpenMode.Append)

            Return result

        Catch



            Return False
        End Try

    End Function

    Private Function GetJournalData(ByRef sJournalData() As String, ByRef iRetryErrors As Integer) As Integer

        Dim result As Integer = 0
        Dim JournalData As Integer
        Dim sFilename, sJournalNumber, sExists As String

        result = True



        ' Filename of journal file
        If iRetryErrors Then
            sFilename = m_sHistoryRoot & "data\journal.rty"
        Else
            sFilename = m_sHistoryRoot & "data\journal.wrk"
        End If

        ' Find free file number and open
        JournalData = FileSystem.FreeFile()
        FileSystem.FileOpen(JournalData, sFilename, OpenMode.Input)

        ' Read through all of journal file
        While Not FileSystem.EOF(JournalData)
            sJournalNumber = FileSystem.LineInput(JournalData)

            If sJournalNumber.Trim().Length > 15 Then
                ' Possible error with journal number,
                ' can't be greater than 8 chars long
                JournalErrorLog("GetJournalData", "Possible error with journal number, greater than 15 characters")
                result = False

                'Close journal file
                FileSystem.FileClose(JournalData)

                Return result
            End If

            If g_sCurrentJournalPos = "" Then
                If sJournalNumber.Trim().Length > 1 Then
                    ReDim Preserve sJournalData(sJournalData.GetUpperBound(0) + 1)
                    sJournalData(sJournalData.GetUpperBound(0)) = sJournalNumber
                End If
            Else
                ' If current journal position exists, start at that point

                'check to see if a retry journal exists
                sExists = FileSystem.Dir(m_sHistoryRoot & "data\journal.rty", FileAttribute.Normal)

                If g_sCurrentJournalPos = sJournalNumber.Trim() Then
                    If sJournalNumber.Trim().Length > 1 Then
                        ReDim Preserve sJournalData(sJournalData.GetUpperBound(0) + 1)
                        sJournalData(sJournalData.GetUpperBound(0)) = sJournalNumber
                        g_sCurrentJournalPos = ""
                    End If
                End If
            End If
        End While

        ' Close journal file
        FileSystem.FileClose(JournalData)

        Return result


    End Function
    Private Sub JournalErrorLog(ByRef FunctionName As String, ByRef ErrorMessage As String)

        FileSystem.PrintLine(fhJournalErrorlog, DateTime.Now.ToString("dd/MM/yy  HH:MM:ss") & " - " & "Func: " & FunctionName & ", " & ErrorMessage)

    End Sub
    ' ***************************************************************** '
    ' Name: CheckPaused
    '
    ' Description:  JH071098 - checks if the daemon has been paused
    '               this is Salvo's code copied from ProcessControlData
    '
    '
    ' ***************************************************************** '
    Private Function CheckPaused() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'salvo - Check if daemon has been paused, in which case pause.
        'Also close the log file so it can be viewed
        If g_bPauseProcessing Then

            m_oPMBLog.CloseLogFile(m_sHistoryRoot)

            MessageBox.Show("You have paused the API Daemon." & Strings.Chr(10).ToString() & "Click OK to continue.", "DocuMaster API Daemon Paused.")


            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorLog("CheckPaused", "Failed to open log file")
                result = False

                ' Close the index file
                If Not (CloseIndexListData()) Then
                    ErrorLog("CheckPaused", "Failed to close the index list data file, " & m_sLogFileName)
                End If
                Return result
            End If

            g_bPauseProcessing = False

        End If


        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CheckLogDate - private
    '
    ' Description:  Checks the date as the log file may need
    '               changing - for daemon runs over night
    '               (copied from ProcessControlData
    '
    ' ***************************************************************** '
    Private Function CheckLogDate() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check to see if the log filename needs to
        ' be changed
        If m_sLogFileName <> DateTime.Now.ToString("ddMMyy") Then
            ' Date must have changed

            ' We must now close the log file, and reopen
            ' so it can create the new log filename

            m_oPMBLog.CloseLogFile(m_sHistoryRoot)


            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorLog("CheckLogDate", "Failed to open log file")
                result = False

                ' Close the index file
                If Not (CloseIndexListData()) Then
                    ErrorLog("CheckLogDate", "Failed to close the index list data file, " & m_sLogFileName)
                End If
                Return result
            End If

            m_sLogFileName = DateTime.Now.ToString("ddMMyy")

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Function Name: ProcessControlData - private
    '
    ' Description:  Very long inefficient case statement
    '               imported from VB3 DMS 2.0 - this accepts
    '               tasks given from PM and processes.
    '               NB. if m_bAccelerated = true then it's a
    '               sysop 40 run - ie setting up after install.
    '
    ' Edit History:
    ' JH071098 -    Changed a few bits by copying them out to
    '               other functions to reduce the size of the
    '               case statement.  Commented out old bits.
    '
    ' JH061098 -    Data Merge.  New task "MergeIndex", borrowed
    '               most of code from AddIndex.
    '
    ' ***************************************************************** '


    Private Function ProcessControlData(ByRef sControlName As String) As Integer

        Dim result As Integer = 0
        Dim utControlData As MainModule.g_utControlData = MainModule.g_utControlData.CreateInstance()
        'Dim utCabinet As g_utCabinet
        'Dim utDMSHistData As g_utDMSHistData
        Dim sPageName As String = ""

        Dim iBadCode As Integer
        Dim iIndexFlag As gPMConstants.PMEReturnCode
        'Dim sLogFileName As String
        'Dim utIndexListData As g_utIndexListData
        Dim sActiveVolume As String = ""

        Dim sKeywords(0) As String, sDocLinks(0) As String
        'ReDim utIndexCabinet(0) As g_utIndexCabinet

        Dim vIndexListData As Object

        Dim iRecsRead, iFailedTot, iNumberMerged As Integer
        Dim sType As String = ""
        Dim fhMergelog As Integer



        result = True


        If Not (GetControlData(sControlName, utControlData)) Then

            ErrorLog("ProcessControlData", "Failed to get control data")
            Return False

        End If

        ' JH071098 - this new case statement added with code moved up
        '            from below rather than repeating the whole lot as before.



        Select Case utControlData.task.ToUpper()
            Case "LOG"
                ' Task LOG

                m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ErrorLog("ProcessControlData", "Failed to open log file")
                    Return False
                End If

                m_sLogMess(1) = utControlData.message

                m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)

                ' Close the log file

                m_oPMBLog.CloseLogFile(m_sHistoryRoot)

            Case Else
                'ridiculous to repeat this for every case


                m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ErrorLog("ProcessControlData", "Failed to open log file")
                    Return False
                End If

                m_sLogFileName = DateTime.Now.ToString("ddMMyy")

                m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, " & utControlData.task.ToUpper()

                m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)

        End Select

        If utControlData.task.ToUpper().IndexOf("INDEX") + 1 Then
            ' ie if ADDINDEX DELINDEX or MERGEINDEX
            ' this reduces the case statement below

            If Not (OpenIndexListData(utControlData.filename)) Then
                ErrorLog("ProcessControlData", "Failed to open the index list data file, " & utControlData.filename)
                Return False
            End If

            iIndexFlag = gPMConstants.PMEReturnCode.PMTrue

            ' Get a line the the index file


            iIndexFlag = CType(GetIndexListData(vIndexListData), gPMConstants.PMEReturnCode)

        End If

        ' now the huge case statement (reduced somewhat)


        Select Case utControlData.task.ToUpper()
            Case "ADDINDEX"
                ' Task ADDINDEX


                '            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                '
                '            If (m_lReturn <> PMTrue) Then
                '                ErrorLog "ProcessControlData", "Failed to open log file"
                '                ProcessControlData = False
                '                Exit Function
                '            End If
                '
                '            sLogFileName = Format$(Now, "ddmmyy")
                '
                '            m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, ADDINDEX"
                '            m_oPMBLog.DOCiPMFunc.LogMessage LLOG, "DMSAPI", m_sLogMess()
                '
                ' Open the index file
                '            If Not (OpenIndexListData(utControlData.filename)) Then
                '                ErrorLog "ProcessControlData", "Failed to open the index list data file, " & utControlData.filename
                '                ProcessControlData = False
                '                Exit Function
                '            End If
                '
                '
                '            iIndexFlag = PMTrue
                '
                '            ' Get a line the the index file
                '
                '            iIndexFlag = GetIndexListData(vIndexListData)

                'Start loop
                While (iIndexFlag = gPMConstants.PMEReturnCode.PMTrue)

                    '                'salvo - Check if daemon has been paused, in which case pause.
                    '                'Also close the log file so it can be viewed
                    m_lReturn = CheckPaused()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ErrorLog("ProcessControlData", "Failed to check if daemon paused")
                        Return False
                    End If

                    '                If g_bPauseProcessing Then
                    '
                    '                    frmInterface.aviDaemon.AutoPlay = False
                    '
                    '                    m_oPMBLog.CloseLogFile m_sHistoryRoot$
                    '
                    '                    MsgBox "You have paused the API Daemon." & Chr(10) & "Click OK to continue.", , "DocuMaster API Daemon Paused."
                    '
                    '                    m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                    '
                    '                    If (m_lReturn <> PMTrue) Then
                    '                        ErrorLog "ProcessControlData", "Failed to open log file"
                    '                        ProcessControlData = False
                    '
                    '                        ' Close the index file
                    '                        If Not (CloseIndexListData()) Then
                    '                            ErrorLog "ProcessControlData", "Failed to close the index list data file, " & utControlData.filename
                    '                        End If
                    '                        Exit Function
                    '                    End If
                    '
                    '                    frmInterface.aviDaemon.AutoPlay = True
                    '
                    '                    g_bPauseProcessing = False
                    '
                    '                End If

                    iBadCode = False
                    ' First we check if there are any naughty external codes

                    For i As Integer = vIndexListData.GetLowerBound(1) To vIndexListData.GetUpperBound(1)


                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vIndexListData(0, i)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And Conversion.Val(CStr(vIndexListData(0, i))) = 0 Then
                            iBadCode = True

                            ErrorLog("ProcessControlData", "Invalid external code found, " & CStr(vIndexListData(0, i)))
                        End If

                    Next i


                    If iBadCode Then

                        ' Append a log message to todays log file
                        m_sLogMess(1) = "DocuMaster API Daemon - Invalid code found. Index = "

                        For i As Integer = vIndexListData.GetLowerBound(1) To vIndexListData.GetUpperBound(1)



                            m_sLogMess(1) = m_sLogMess(1) & CStr(vIndexListData(0, i)) & "|" &
                                            CStr(vIndexListData(1, i)) & "|"

                        Next i


                        m_oPMBLog.DOCiPMFunc.LogMessage(LERR, "DMSAPI", m_sLogMess)

                    Else

                        'Now hit the generic API to update the DB


                        m_lReturn = m_oAPI.AddIndex(vIndexArray:=vIndexListData, oPMBLog:=m_oPMBLog, bAccelerated:=m_bAccelerated)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Log failure to addindex
                            m_sLogMess(1) = "Failed in AddIndex, check PMMessage table."

                            ErrorLog("ProcessControlData", m_sLogMess(1))

                            result = gPMConstants.PMEReturnCode.PMFalse

                            'If we are doing an accelerated run for sysop 40, we want
                            'to continue - the errors can be sorted later. !
                            If Not m_bAccelerated Then
                                Return result
                            End If
                        End If

                    End If

                    m_lReturn = CheckLogDate()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ErrorLog("ProcessControlData", "Failed to check log date")

                        Return False
                    End If

                    '                ' Check to see if the log filename needs to
                    '                ' be changed
                    '                If (sLogFileName <> Format$(Now, "ddmmyy")) Then
                    '                    ' Date must have changed
                    '
                    '                    ' We must now close the log file, and reopen
                    '                    ' so it can create the new log filename
                    '                    m_oPMBLog.CloseLogFile m_sHistoryRoot$
                    '
                    '                    m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                    '
                    '                    If (m_lReturn <> PMTrue) Then
                    '                        ErrorLog "ProcessControlData", "Failed to open log file"
                    '                        ProcessControlData = False
                    '
                    '                        ' Close the index file
                    '                        If Not (CloseIndexListData()) Then
                    '                            ErrorLog "ProcessControlData", "Failed to close the index list data file, " & utControlData.filename
                    '                        End If
                    '                        Exit Function
                    '                    End If
                    '
                    '                    sLogFileName = Format$(Now, "ddmmyy")
                    '
                    '                End If

                    ' Get a line from the index file

                    iIndexFlag = CType(GetIndexListData(vIndexListData), gPMConstants.PMEReturnCode)

                    Application.DoEvents()

                End While

                ' Close the index file
                If Not (CloseIndexListData()) Then
                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                End If

                ' Close the log file

                m_oPMBLog.CloseLogFile(m_sHistoryRoot)

                If iIndexFlag = gPMConstants.PMEReturnCode.PMError Then
                    ' A Error accurred getting the index data

                    ErrorLog("ProcessControlData", "Failed to get index list data from file, " & utControlData.filename)
                    Return False
                End If

            Case "ADD"

                ' Task ADD

                '            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                '
                '            If (m_lReturn <> PMTrue) Then
                '                ErrorLog "ProcessControlData", "Failed to open log file"
                '                ProcessControlData = False
                '                Exit Function
                '            End If
                '
                '            m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, ADD"
                '            m_oPMBLog.DOCiPMFunc.LogMessage LLOG, "DMSAPI", m_sLogMess()

                iBadCode = False
                ' First we check if there are any naughty internal/external codes
                Dim dbNumericTemp2 As Double
                If Double.TryParse(utControlData.cabinetname, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And Conversion.Val(utControlData.cabinetname) = 0 Then
                    iBadCode = True
                    ErrorLog("ProcessControlData", "Invalid Cabinet code found, " & utControlData.cabinetname)
                End If

                Dim dbNumericTemp3 As Double
                If Double.TryParse(utControlData.drawername, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) And Conversion.Val(utControlData.drawername) = 0 Then
                    iBadCode = True
                    ErrorLog("ProcessControlData", "Invalid Drawer code found, " & utControlData.drawername)
                End If

                Dim dbNumericTemp4 As Double
                If Double.TryParse(utControlData.foldername, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) And Conversion.Val(utControlData.foldername) = 0 Then
                    iBadCode = True
                    ErrorLog("ProcessControlData", "Invalid Folder code found, " & utControlData.foldername)
                End If

                If iBadCode Then
                    Return False
                End If

                'Set up index array from codes that Generic API will like to recieve
                ReDim vIndexListData(1, 2)

                vIndexListData(0, 0) = utControlData.cabinetname

                vIndexListData(1, 0) = utControlData.cabinetname

                vIndexListData(0, 1) = utControlData.drawername

                vIndexListData(1, 1) = utControlData.drawername

                vIndexListData(0, 2) = utControlData.foldername

                vIndexListData(1, 2) = utControlData.foldername

                'Get Keywords and put into array
                GetKeywordsData(utControlData.keywords, sKeywords)


                'Now hit the generic API to update the DB

                m_lReturn = m_oAPI.Add(vIndexArray:=vIndexListData, sDocName:=utControlData.documentname, sFilename:=utControlData.filename, sDocType:=utControlData.event_Renamed, sPageType:=utControlData.doctype, iAccessLevel:=utControlData.access, sUsername:=utControlData.username, sKeywords:=sKeywords, sAnnotation:=utControlData.annotation, oPMBLog:=m_oPMBLog)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'Log failure to add
                    m_sLogMess(1) = "Failed to ADD Document, check PMMessage table."

                    ErrorLog("ProcessControlData", m_sLogMess(1))

                    result = gPMConstants.PMEReturnCode.PMFalse

                End If

                Application.DoEvents()

                ' Close the log file

                m_oPMBLog.CloseLogFile(m_sHistoryRoot)

            Case "DELINDEX"
                ' Task DELINDEX

                '            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                '
                '            If (m_lReturn <> PMTrue) Then
                '                ErrorLog "ProcessControlData", "Failed to open log file"
                '                ProcessControlData = False
                '                Exit Function
                '            End If
                '
                '            sLogFileName = Format$(Now, "ddmmyy")
                '
                '            m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster TASK, DELINDEX"
                '            m_oPMBLog.DOCiPMFunc.LogMessage LLOG, "DMSAPI", m_sLogMess()

                ' Open the index file
                '            If Not (OpenIndexListData(utControlData.filename)) Then
                '                ErrorLog "ProcessControlData", "Failed to open the index list data file, " & utControlData.filename
                '                ProcessControlData = False
                '                Exit Function
                '            End If
                '
                '
                '            iIndexFlag = PMTrue
                '
                '            ' Get a line from the index file
                '            iIndexFlag = GetIndexListData(vIndexListData)

                'Start loop
                While (iIndexFlag = gPMConstants.PMEReturnCode.PMTrue)

                    'salvo - Check if daemon has been paused, in which case pause.
                    'Also close the log file so it can be viewed
                    m_lReturn = CheckPaused()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ErrorLog("ProcessControlData", "Failed to check if daemon paused")
                        Return False
                    End If

                    '                If g_bPauseProcessing Then
                    '
                    '                    frmInterface.aviDaemon.AutoPlay = False
                    '
                    '                    m_oPMBLog.CloseLogFile m_sHistoryRoot$
                    '
                    '                    MsgBox "You have paused the API Daemon." & Chr(10) & "Click OK to continue.", , "DocuMaster API Daemon Paused."
                    '
                    '                    m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                    '
                    '                    If (m_lReturn <> PMTrue) Then
                    '                        ErrorLog "ProcessControlData", "Failed to open log file"
                    '                        ProcessControlData = False
                    '
                    '                        ' Close the index file
                    '                        If Not (CloseIndexListData()) Then
                    '                            ErrorLog "ProcessControlData", "Failed to close the index list data file, " & utControlData.filename
                    '                        End If
                    '                        Exit Function
                    '                    End If
                    '
                    '                    frmInterface.aviDaemon.AutoPlay = True
                    '
                    '                    g_bPauseProcessing = False
                    '
                    '                End If

                    iBadCode = False
                    ' First we check if there are any naughty external codes

                    For i As Integer = vIndexListData.GetLowerBound(1) To vIndexListData.GetUpperBound(1)


                        Dim dbNumericTemp5 As Double
                        If Double.TryParse(CStr(vIndexListData(0, i)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) And Conversion.Val(CStr(vIndexListData(0, i))) = 0 Then
                            iBadCode = True

                            ErrorLog("ProcessControlData", "Invalid external code found, " & CStr(vIndexListData(0, i)))
                        End If

                    Next i


                    If iBadCode Then

                        ' Append a log message to todays log file
                        m_sLogMess(1) = "DocuMaster API Daemon - Invalid code found. Index = "

                        For i As Integer = vIndexListData.GetLowerBound(1) To vIndexListData.GetUpperBound(1)



                            m_sLogMess(1) = m_sLogMess(1) & CStr(vIndexListData(0, i)) & "|" &
                                            CStr(vIndexListData(1, i)) & "|"

                        Next i


                        m_oPMBLog.DOCiPMFunc.LogMessage(LERR, "DMSAPI", m_sLogMess)

                    Else

                        'Now hit the generic API to update the DB

                        m_lReturn = m_oAPI.DelIndex(vIndexArray:=vIndexListData, iEmptyOnly:=utControlData.emptyonly, iAccessLevel:=utControlData.access, oPMBLog:=m_oPMBLog)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Log failure to addindex
                            m_sLogMess(1) = "Failed in DELINDEX, check PMMessage table."

                            ErrorLog("ProcessControlData", m_sLogMess(1))


                            Return gPMConstants.PMEReturnCode.PMFalse

                        End If

                    End If

                    ' Check to see if the log filename needs to
                    ' be changed

                    m_lReturn = CheckLogDate()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ErrorLog("ProcessControlData", "Failed to check log date")

                        Return False
                    End If

                    '                If (sLogFileName <> Format$(Now, "ddmmyy")) Then
                    '                    ' Date must have changed
                    '
                    '                    ' We must now close the log file, and reopen
                    '                    ' so it can create the new log filename
                    '                    m_oPMBLog.CloseLogFile m_sHistoryRoot$
                    '
                    '                    m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                    '
                    '                    If (m_lReturn <> PMTrue) Then
                    '                        ErrorLog "ProcessControlData", "Failed to open log file"
                    '                        ProcessControlData = False
                    '
                    '                        ' Close the index file
                    '                        If Not (CloseIndexListData()) Then
                    '                            ErrorLog "ProcessControlData", "Failed to close the index list data file, " & utControlData.filename
                    '                        End If
                    '                        Exit Function
                    '                    End If
                    '
                    '                    sLogFileName = Format$(Now, "ddmmyy")
                    '
                    '                End If

                    ' Get a line from the index file

                    iIndexFlag = CType(GetIndexListData(vIndexListData), gPMConstants.PMEReturnCode)

                    Application.DoEvents()

                End While

                ' Close the index file
                If Not (CloseIndexListData()) Then
                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                End If

                ' Close the log file

                m_oPMBLog.CloseLogFile(m_sHistoryRoot)

                If iIndexFlag = gPMConstants.PMEReturnCode.PMError Then
                    ' A Error accurred getting the index data

                    ErrorLog("ProcessControlData", "Failed to get index list data from file, " & utControlData.filename)
                    Return False
                End If

            Case "MERGEINDEX"
                ' Task MergeIndex

                fhMergelog = FileSystem.FreeFile()
                FileSystem.FileOpen(fhMergelog, m_sHistoryRoot & "logs\MERGE.log", OpenMode.Append)

                ' Initialise the totals
                iRecsRead = 0
                iFailedTot = 0
                iNumberMerged = 0

                ' merge data is transferred to the array from the file in this format
                'vIndexListData(0, 0) ' old cabinet code
                'vIndexListData(0, 1) ' old drawer code
                'vIndexListData(0, 2) ' old foldercode
                'vIndexListData(1, 0) ' new cabinet code
                'vIndexListData(1, 1) ' new drawer code
                'vIndexListData(1, 2) ' new folder code

                ' the drawer and folder codes may be missing so re-dim the array
                ' and thus any missing drawer or folder external codes will be auto blanked!
                ReDim Preserve vIndexListData(1, 2)

                'Start loop
                While (iIndexFlag = gPMConstants.PMEReturnCode.PMTrue)

                    ' total no. of records read
                    iRecsRead += 1

                    'Update progress form

                    'To Do
                    'objfrmInterface.panCurrentFile.Caption = "Currently Processing Client Merge Record " & iRecsRead

                    iBadCode = False
                    ' First we check if there are any naughty external codes
                    ' JH061098 this bit has been changed to check all elements


                    For i As Integer = vIndexListData.GetLowerBound(1) To vIndexListData.GetUpperBound(1)


                        If Not Object.Equals(vIndexListData(0, i), Nothing) Then  '  valid only if it is not empty

                            Dim dbNumericTemp6 As Double
                            If Double.TryParse(CStr(vIndexListData(0, i)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) And Conversion.Val(CStr(vIndexListData(0, i))) = 0 Then
                                iBadCode = True
                            End If
                        End If


                        If Not Object.Equals(vIndexListData(1, i), Nothing) Then  '  valid only if it is not empty

                            Dim dbNumericTemp7 As Double
                            If Double.TryParse(CStr(vIndexListData(1, i)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) And Conversion.Val(CStr(vIndexListData(1, i))) = 0 Then
                                iBadCode = True
                            End If
                        End If
                    Next i

                    ' both cabinet code must exist


                    If CStr(vIndexListData(0, 0)) = "" Or CStr(vIndexListData(1, 0)) = "" Then
                        iBadCode = True
                    End If

                    'both clients must exist


                    If CStr(vIndexListData(0, 1)) = "" Or CStr(vIndexListData(1, 1)) = "" Then
                        iBadCode = True
                    End If

                    ' both folder codes must exist


                    If CStr(vIndexListData(0, 2)) = "" Or CStr(vIndexListData(1, 2)) = "" Then
                        iBadCode = True
                    End If


                    If iBadCode Then

                        iFailedTot += 1

                        If iFailedTot = 1 Then

                            FileSystem.PrintLine(fhMergelog, "")
                            FileSystem.PrintLine(fhMergelog, "Merge run time: " & DateTime.Now.ToString("dd/MM/yy  HH:MM:ss"))
                            FileSystem.PrintLine(fhMergelog, "---------------------------------------------------")
                            FileSystem.PrintLine(fhMergelog, "DME DOES NOT RECOGNISE SOME OR ALL OF THE EXTERNAL ")
                            FileSystem.PrintLine(fhMergelog, "CODES, THE FOLLOWING FOLDERS FAILED ON MERGE")
                            FileSystem.PrintLine(fhMergelog, "---------------------------------------------------")
                            FileSystem.PrintLine(fhMergelog, "")

                            ' print the cabinet codes only first time round


                            FileSystem.PrintLine(fhMergelog, CStr(iFailedTot) & " - " & CStr(vIndexListData(0, 0)) & "|" &
                                                 CStr(vIndexListData(1, 0)) & "|" & " (Source Cabinet Code|Target Cabinet Code)") '@A MS 120900

                            FileSystem.PrintLine(fhMergelog, "")
                            FileSystem.PrintLine(fhMergelog, "the following lines are details of the Drawer And Folder external codes")
                            FileSystem.PrintLine(fhMergelog, "")
                            FileSystem.PrintLine(fhMergelog, "(Source Co. ex code|Target Co. ex code)")
                            FileSystem.PrintLine(fhMergelog, "")
                        End If


                        For i As Integer = vIndexListData.GetLowerBound(1) + 1 To vIndexListData.GetUpperBound(1)

                            If i = 1 Then
                                sType = "Drawer"
                            Else
                                sType = "Folder"
                            End If



                            FileSystem.PrintLine(fhMergelog, CStr(iFailedTot) & " - " & CStr(vIndexListData(0, i)) & "|" & CStr(vIndexListData(1, i)) & " " & sType)
                        Next i


                    Else

                        'Now hit the generic API to update the DB


                        m_lReturn = m_oAPI.MergeIndex(vIndexArray:=vIndexListData, oPMBLog:=m_oPMBLog)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            ' keep a running total of failed merges if any
                            iFailedTot += 1


                            'Log failure to mergeindex
                            m_sLogMess(1) = "Failed in MergeIndex, check PM Message Viewer."

                            ErrorLog("ProcessControlData", m_sLogMess(1))

                            result = gPMConstants.PMEReturnCode.PMFalse

                            'If we are doing an accelerated run for sysop 40, we want
                            'to continue - the errors can be sorted later. !
                            If Not m_bAccelerated Then
                                Return result
                            End If
                        End If

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            ' successful merge completed
                            iNumberMerged += 1
                        End If

                    End If


                    ' Check to see if the log filename needs to
                    ' be changed

                    m_lReturn = CheckLogDate()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ErrorLog("ProcessControlData", "Failed to check log date")

                        Return False
                    End If

                    ' Get a line from the index file

                    iIndexFlag = CType(GetIndexListData(vIndexListData), gPMConstants.PMEReturnCode)

                    ' the cabinet and folder codes may be missing so re-dim the array
                    ' and thus any missing drawer or folder external codes will be auto blanked!
                    ReDim Preserve vIndexListData(1, 2)

                End While


                'To Do
                'objfrmInterface.panCurrentFile.Caption = "Client Merge Process Completed"

                ' Close the index file
                If Not (CloseIndexListData()) Then
                    ErrorLog("ProcessControlData", "Failed to close the index list data file, " & utControlData.filename)
                End If


                ReDim m_sLogMess(6)
                m_sLogMess(1) = "DocuMaster API Daemon - DATA MERGE COMPLETED"
                m_sLogMess(2) = "============================================"
                m_sLogMess(3) = "Total number of records read   " & iRecsRead
                m_sLogMess(4) = "Total number of records merged " & iNumberMerged
                m_sLogMess(5) = "Total number failed to merge   " & iFailedTot
                m_sLogMess(6) = "============================================="

                m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)

                If iFailedTot > 0 Then
                    ReDim m_sLogMess(4)
                    m_sLogMess(1) = "See the following files for any errors: "
                    m_sLogMess(2) = "--------------------------------------"
                    m_sLogMess(3) = m_sHistoryRoot & "tmp\Merge.log"
                    m_sLogMess(3) = m_sHistoryRoot & "logs\<ddmmyy>.log"
                    m_sLogMess(4) = "Sirius log file on the Server (or PM Message Viewer)"

                    m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)
                End If


                ' Close the log file

                m_oPMBLog.CloseLogFile(m_sHistoryRoot)

                If iIndexFlag = gPMConstants.PMEReturnCode.PMError Then
                    ' A Error accurred getting the index data

                    ErrorLog("ProcessControlData", "Failed to get index list data from file, " & utControlData.filename)
                    Return False
                End If


                FileSystem.PrintLine(fhMergelog, "")
                FileSystem.PrintLine(fhMergelog, DateTime.Now.ToString("dd/MM/yy  HH:MM:ss"))
                FileSystem.PrintLine(fhMergelog, "DocuMaster API Daemon - DATA MERGE statistics")
                FileSystem.PrintLine(fhMergelog, "=============================================")
                FileSystem.PrintLine(fhMergelog, "Total number of records read   " & iRecsRead)
                FileSystem.PrintLine(fhMergelog, "Total number of records merged " & iNumberMerged)
                FileSystem.PrintLine(fhMergelog, "Total number failed to merge   " & iFailedTot)
                FileSystem.PrintLine(fhMergelog, "=============================================")
                FileSystem.PrintLine(fhMergelog, "")

                'close merge error file
                FileSystem.FileClose(fhMergelog)

            Case "REBUILDREMOTEDB"

                m_lReturn = RebuildRemoteDB()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Log failure to mergeindex
                    m_sLogMess(1) = "Rebuild Remote History Database Failed. Check PM Message Viewer"
                    ErrorLog("ProcessControlData", m_sLogMess(1))

                    ' MS 15/06/01    if it errors do not set to PMFALSE otherwise after the
                    'Remote data rebuild the control files will not get deleted. The error
                    'log will log lots of errors anyway and will be stored in log file or PM Message table
                    '   ProcessControlData = PMFalse
                    '   Exit Function

                End If

                ' log just errors when updating remote history database
                m_bRebuild = True

            Case "LOG"
                ' Task LOG
                ' do nothing as this is done at the top now.

                '            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                '
                '            If (m_lReturn <> PMTrue) Then
                '                ErrorLog "ProcessControlData", "Failed to open log file"
                '                ProcessControlData = False
                '                Exit Function
                '            End If
                '
                '            m_sLogMess(1) = utControlData.message
                '            m_oPMBLog.DOCiPMFunc.LogMessage LLOG, "DMSAPI", m_sLogMess()
                '
                '            ' Close the log file
                '            m_oPMBLog.CloseLogFile m_sHistoryRoot

        End Select

        Return result


    End Function
    ' *********************************************************************************** '
    ' Name: RebuildRemoteDB

    ' Description:
    '
    '       This function rebuilds the Remote History Database (RHD) from scratch.
    '       Adds cabinets, drawers, folders and documents
    '       Documents in folder level will be added only into RHD
    '       It is assumed the RHD on the UNIX is cleared down first before this is run
    '
    ' *********************************************************************************** '

    Public Function RebuildRemoteDB() As Integer


        Dim result As Integer = 0
        Dim vFolderArray(,) As Object
        Dim vDocArray(,) As Object
        Dim vCabinetArray(,) As Object
        Dim vArray(,) As Object
        Dim vDrawerArray(,) As Object
        Dim sDocName, sDocRef, sPageType, sPageName, sTmp, sEventType As String
        Dim dDocDate As Date
        Dim j As Integer
        Dim lLink, lTmp, lParentNum, lMaxFolderNum, lFolderNum, lDrawerCount, lTotalDrawers, lCabNum As Integer
        Dim m_sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the history business object reference for writing to the DB
            Dim m_oHistory As New bDOCHistory.Form()

            m_lReturn = m_oHistory.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get instance of miscellaneous class
            Dim m_oMisc As New bDOCAPI.Miscellaneous()

            m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase, vHistory:=m_oHistory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oMisc = New bDOCAPI.Miscellaneous()

            m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase, vHistory:=m_oHistory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get cabinet records

            ' cabinet parent is always gonna be a 0
            ' get external cabinets only
            m_sSQL = " SELECT ex_code, folder_name, folder_num  FROM DOC_folder"
            m_sSQL = m_sSQL & " WHERE ex_code <> '' AND folder_level = " & CStr(DOCCabinet)
            m_sSQL = m_sSQL & " AND parent_num = 0"


            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetFoldersCABINET", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vCabinetArray), gPMConstants.PMEReturnCode)


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vCabinetArray)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot get Cabinet folder records from database : " & m_sSQL, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            ' write cabinet records to history database

            For i As Integer = vCabinetArray.GetLowerBound(1) To vCabinetArray.GetUpperBound(1)

                'Update the history database


                m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDCABINET, vCabinetcode:=CStr(vCabinetArray(0, i)), vCabinetname:=CStr(vCabinetArray(1, i)))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse


                    bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database for Cabinet. Index = " &
                               CStr(vCabinetArray(0, i)) & "|" &
                               CStr(vCabinetArray(1, i)) & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            Next i

            'developer guide  no. 50
            Dim frmProgress As New frmProgress
            ' set up the progress bar
            'Dim tempLoadForm As frmProgress = frmProgress
            frmProgress.Show()

            ' get drawer records for each cabinet

            For i As Integer = vCabinetArray.GetLowerBound(1) To vCabinetArray.GetUpperBound(1)

                frmProgress.Refresh()

                frmProgress.lblCabinet.Text = "Total Company Cabinets : " & vCabinetArray.GetUpperBound(1) + 1 &
                                              "     currently updating : " & CStr(i + 1)


                lCabNum = CInt(vCabinetArray(2, i))

                ' get total entries of the current  drawer in order to update progress bar
                m_sSQL = "SELECT COUNT(*) FROM DOC_folder"
                m_sSQL = m_sSQL & " WHERE ex_code <>  '' AND parent_num = " & CStr(lCabNum)
                ' Fire SQL

                m_lReturn = CType(m_oDatabase.SQLSelect(m_sSQL, "Get Row Count of Drawers for Cabinet" & lParentNum, False, 1, vArray), gPMConstants.PMEReturnCode)
                If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (Information.IsArray(vArray)) Then

                    lTotalDrawers = CInt(vArray(0, 0))
                End If

                ' get maximum value of the current drawer
                m_sSQL = "SELECT MAX(folder_num) FROM DOC_folder"
                m_sSQL = m_sSQL & " WHERE ex_code <> '' AND parent_num = " & CStr(lCabNum)

                ' Fire SQL
                m_lReturn = CType(m_oDatabase.SQLSelect(m_sSQL, "Get Maximum Child Folder Number for Cabinet" & lParentNum, False, 1, vArray), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vArray) Then

                    'set lMaxFolderNum one more than the actual maximum record in order to include it

                    lMaxFolderNum = CInt(CDbl(vArray(0, 0)) + 1)
                    lFolderNum = 0
                    lDrawerCount = 0

                    ' process 500 client/drawers at a time for current company
                    Do While lFolderNum < lMaxFolderNum

                        m_sSQL = " SELECT ex_code, folder_name, folder_num FROM DOC_folder"
                        m_sSQL = m_sSQL & " WHERE ex_code <> '' AND parent_num = " & CStr(lCabNum)
                        m_sSQL = m_sSQL & " AND  folder_num > " & CStr(lFolderNum) & " ORDER BY folder_num"

                        'Hit the DB
                        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetFoldersDRAWER", bStoredProcedure:=False, lNumberRecords:=500, vResultArray:=vDrawerArray), gPMConstants.PMEReturnCode)


                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot get Drawer folder records from database: " & m_sSQL, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If

                        If Not Information.IsArray(vDrawerArray) Then
                            ' No more records for this drawer so exit loop and do next drawer
                            Exit Do
                        End If

                        If Information.IsArray(vDrawerArray) Then

                            ' write drawer records to history db

                            For j = vDrawerArray.GetLowerBound(1) To vDrawerArray.GetUpperBound(1)

                                lDrawerCount += 1

                                frmProgress.Refresh()
                                frmProgress.lblDrawer.Text = "Total Client Drawers   : " & lTotalDrawers &
                                                             "     currently updating : " & CStr(lDrawerCount)

                                'Update the history database




                                m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDRAWER, vCabinetcode:=CStr(vCabinetArray(0, i)), vCabinetname:=CStr(vCabinetArray(1, i)), vDrawercode:=CStr(vDrawerArray(0, j)), vDrawername:=CStr(vDrawerArray(1, j)))

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse




                                    bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database for Drawers. Index = " &
                                               CStr(vCabinetArray(0, i)) & "|" &
                                               CStr(vCabinetArray(1, i)) & "|" &
                                               CStr(vDrawerArray(0, j)) & "|" &
                                               CStr(vDrawerArray(1, j)), vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                End If

                                ' get all folder records belonging to this drawer

                                lParentNum = CInt(vDrawerArray(2, j))

                                m_sSQL = " SELECT ex_code, folder_name, folder_num FROM DOC_folder"
                                m_sSQL = m_sSQL & " WHERE ex_code <> '' AND parent_num = " & CStr(lParentNum)

                                'Hit the DB
                                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetFoldersFOLDER", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vFolderArray), gPMConstants.PMEReturnCode)


                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vFolderArray)) Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot get Folder record from database : " & m_sSQL, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                End If



                                If Information.IsArray(vFolderArray) Then


                                    ' add folders and get document records for each folder

                                    For k As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                                        'Update the history database






                                        m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDFOLDER, vCabinetcode:=CStr(vCabinetArray(0, i)), vCabinetname:=CStr(vCabinetArray(1, i)), vDrawercode:=CStr(vDrawerArray(0, j)), vDrawername:=CStr(vDrawerArray(1, j)), vFoldercode:=CStr(vFolderArray(0, k)), vFoldername:=CStr(vFolderArray(1, k)))


                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            result = gPMConstants.PMEReturnCode.PMFalse







                                            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database. Index = " &
                                                       CStr(vCabinetArray(0, i)) & "|" &
                                                       CStr(vCabinetArray(1, i)) & "|" &
                                                       CStr(vDrawerArray(0, j)) & "|" &
                                                       CStr(vDrawerArray(1, j)) & "|" &
                                                       CStr(vFolderArray(0, k)) & "|" &
                                                       CStr(vFolderArray(1, k)), vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                        End If


                                        lParentNum = CInt(vFolderArray(2, k))

                                        ' create  document records

                                        m_sSQL = " SELECT doc_num FROM DOC_document WHERE folder_num = " & lParentNum

                                        'Hit DB
                                        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTDOCUMENT", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocArray), gPMConstants.PMEReturnCode)
                                        If Information.IsArray(vDocArray) Then


                                            For l As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

                                                ' NOTE: if any errors occur in calls to oMisc, it'll get logged in Sirius log
                                                ' so need to worry about error logging just continue

                                                'get doc date

                                                m_lReturn = CType(m_oMisc.GetDocDate(lDocNum:=CInt(vDocArray(0, l)), dDocDate:=dDocDate), gPMConstants.PMEReturnCode)

                                                'get other doc details

                                                m_lReturn = CType(m_oMisc.GetDocDetails(lDocNum:=CInt(vDocArray(0, l)), sDocName:=sDocName, sDocType:=sEventType), gPMConstants.PMEReturnCode)


                                                m_lReturn = CType(m_oMisc.ConstructDocRef(CInt(vDocArray(0, l)), sDocRef), gPMConstants.PMEReturnCode)

                                                'get the page file    (first check for links though)

                                                m_lReturn = CType(m_oMisc.GetDocLink(CInt(vDocArray(0, l)), lLink), gPMConstants.PMEReturnCode)

                                                If lLink <> 0 Then
                                                    lTmp = lLink
                                                Else

                                                    lTmp = CInt(vDocArray(0, l))
                                                End If

                                                m_lReturn = CType(m_oMisc.GetPageName(lDocNum:=lTmp, sPageName:=sPageName), gPMConstants.PMEReturnCode)

                                                'remove the obliques and soliduses
                                                m_lReturn = CType(StripSlashes(sPageName, sTmp), gPMConstants.PMEReturnCode)

                                                'get the page type
                                                m_lReturn = CType(m_oMisc.GetPageType(lDocNum:=lTmp, sPageType:=sPageType), gPMConstants.PMEReturnCode)

                                                'hit the history table






                                                m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT, vCabinetcode:=CStr(vCabinetArray(0, i)), vCabinetname:=CStr(vCabinetArray(1, i)), vDrawercode:=CStr(vDrawerArray(0, j)), vDrawername:=CStr(vDrawerArray(1, j)), vFoldercode:=CStr(vFolderArray(0, k)), vFoldername:=CStr(vFolderArray(1, k)), vDocref:=sDocRef, vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"), vEventtype:=sEventType, vDescription:=sDocName, vVolume:=DOCHD1_NAME, vPagefile:=sTmp, vDoctype:=sPageType)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    result = gPMConstants.PMEReturnCode.PMFalse






                                                    bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database for document. Index = " &
                                                               CStr(vCabinetArray(0, i)) & "|" &
                                                               CStr(vCabinetArray(1, i)) & "|" &
                                                               CStr(vDrawerArray(0, j)) & "|" &
                                                               CStr(vDrawerArray(1, j)) & "|" &
                                                               CStr(vFolderArray(0, k)) & "|" &
                                                               CStr(vFolderArray(1, k)) & "|" &
                                                               sDocName & "|" & sDocRef, vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                                End If

                                            Next l

                                        End If ' If IsArray(vDocArray)

                                    Next k

                                End If 'IsArray Folder


                            Next j
                            ' read the last array index
                            ' the above Next statememt adds an extra 1 so need to take 1 away

                            lFolderNum = CInt(vDrawerArray(2, j - 1))

                        End If ' IsArray Drawer

                    Loop

                End If 'if PMTrue and IsArray

            Next i

            frmProgress.Close()

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unexpected error " & gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildRemoteDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub GetKeywordsData(ByRef sKeywords As String, ByRef sKeywordsArr() As String)

        Dim iArrCntr As Integer



        iArrCntr = 1
        For iStrCntr As Integer = 1 To sKeywords.Length
            If sKeywords.Substring(iStrCntr - 1, 1) = "|" Then
                iArrCntr += 1
            Else
                ReDim Preserve sKeywordsArr(iArrCntr)
                sKeywordsArr(iArrCntr) = sKeywordsArr(iArrCntr) & sKeywords.Substring(iStrCntr - 1, 1)
            End If
        Next iStrCntr



    End Sub

    '***************************************************************************************************
    '  Edit History:
    '
    '  MS 19/03/2001   Amended code to cater for remote history rebuild
    '                  To speed up the process, log errors only instead of logging
    '                  everything when in rebuild mode
    '                  Process history records 500 at a time in case of sheer volume
    '
    '

    Private Function CommitHDB(ByRef iRetryErrors As Integer) As Integer
        Dim ErrFillStruct As Boolean = False
        Dim ErrCommitHDB As Boolean = False

        Dim result As Integer = 0
        Dim utDMSHistParams As MainModule.g_utDMSHistParams = MainModule.g_utDMSHistParams.CreateInstance()
        Dim sTaskDesc As String = ""

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object
        Dim l As Integer
        Dim sReturnCode As New FixedLengthString(4)
        Dim sFileStatus As New FixedLengthString(2)
        Dim vArray As Object
        Dim lHistRecNum, lMaxHistRecNum, lCurrentRec As Integer
        Dim lHistoryRowCount As Integer

        Try
            ErrCommitHDB = True
            ErrFillStruct = False

            result = True


            'open log file

            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                result = False

                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open PMB log file", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitHDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            ' get total history entries in order to update the progress bar
            sSQL = "SELECT COUNT(*) FROM DOC_history"
            ' Fire SQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL, "Retrieveing History Records Count", False, 500, vArray)
            ' If SQL fails

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
                ' Exit here
                Return result
            End If


            lHistoryRowCount = CInt(vArray(0, 0))


            ' get maximum value of the history id
            sSQL = "SELECT MAX(history_id) FROM DOC_history"

            ' Fire SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL, "Retrieveing History Records Total", False, 500, vArray)

            ' If SQL fails
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then

                ' Exit here
                Return result
            End If



            lMaxHistRecNum = CInt(vArray(0, 0))
            lCurrentRec = 1


            'update progress form
            objfrmInterface.proProgress.Minimum = 0

            If lMaxHistRecNum = lCurrentRec Then
                'only 1 so just set to 100%
                objfrmInterface.proProgress.Maximum = objfrmInterface.proProgress.Minimum + 1
                objfrmInterface.proProgress.Value = objfrmInterface.proProgress.Maximum
            Else
                objfrmInterface.proProgress.Maximum = lHistoryRowCount
                objfrmInterface.proProgress.Value = objfrmInterface.proProgress.Minimum
            End If

            lHistRecNum = 1


            'Main process


            ' process the records 500 hundred at a time
            Do While lHistRecNum <= lMaxHistRecNum

                'Construct SQL
                sSQL = "SELECT history_id, task, cabinetcode, cabinetname, drawercode, " & _
                       "drawername, foldercode, foldername, docref, request_date, " & _
                       "request_time, eventtype, description, volume, pagefile, doctype, " & _
                       "hderror "


                If iRetryErrors Then
                    sSQL = sSQL & " FROM DOC_history WHERE history_id > " & CStr(lHistRecNum) & " ORDER BY history_id"
                Else
                    sSQL = sSQL & " FROM DOC_history WHERE history_id > " & CStr(lHistRecNum) & " AND hderror = 'N' ORDER BY history_id"
                End If

                'Get contents of history database
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETHISTORYDATA", bStoredProcedure:=False, lNumberRecords:=500, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return False
                End If

                If Not Information.IsArray(vResultArray) Then
                    Return result
                End If

                'lCnt1& = 0

                utDMSHistParams.DMSDir.Value = m_sHistoryRoot

                ' Process all history records

                For l = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    'salvo - Check if cancel has been chosen, in which case leave neatly and promptly
                    If g_bCancelProcessing Then

                        ReDim m_sLogMess(1)

                        m_sLogMess(1) = "DocuMaster API Worker Daemon - Processing Cancelled"


                        m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)

                        ' Close the log file

                        m_oPMBLog.CloseLogFile(m_sHistoryRoot)
                        Return result
                    End If

                    'salvo - Check if daemon paused, in which case pause.
                    m_lReturn = CheckPaused()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        result = False

                        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open PMB log file", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitHDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                    '        If g_bPauseProcessing Then
                    '
                    '            frmInterface.aviDaemon.AutoPlay = False
                    '
                    '            ' Close the log file
                    '            m_oPMBLog.CloseLogFile m_sHistoryRoot
                    '
                    '            MsgBox "You have paused the API Daemon." & Chr(10) & "Click OK to continue.", , "DocuMaster API Daemon Paused."
                    '
                    '            'open log file
                    '            m_lReturn = m_oPMBLog.OpenLogFile(m_sHistoryRoot$)
                    '
                    '            If (m_lReturn& <> PMTrue) Then
                    '
                    '                ' Log Error Message
                    '                CommitHDB = False
                    '
                    '                iPMFunc.LogMessage sUsername:="", _
                    ''                    iType:=PMLogError, _
                    ''                    sMsg:="Failed to open PMB log file", _
                    ''                    vApp:=ACApp, _
                    ''                    vClass:=ACClass, _
                    ''                    vMethod:="CommitHDB", _
                    ''                    vErrNo:=Err.Number, _
                    ''                    vErrDesc:=Err.description
                    '
                    '                Exit Function
                    '
                    '            End If

                    '            frmInterface.aviDaemon.AutoPlay = True
                    '            g_bPauseProcessing = False
                    '
                    '        End If

                    ' DoEvents

                    If Not m_bRebuild Then

                        ReDim m_sLogMess(5)

                        'Wha task is it ?
                        Select Case (vResultArray(1, l))
                            Case DOCADDCABINET
                                sTaskDesc = "ADDCABINET"
                            Case DOCDELCABINET
                                sTaskDesc = "DELCABINET"
                            Case DOCMODCABINET
                                sTaskDesc = "MODCABINET"
                            Case DOCADDDRAWER
                                sTaskDesc = "ADDDRAWER"
                            Case DOCDELDRAWER
                                sTaskDesc = "DELDRAWER"
                            Case DOCMODDRAWER
                                sTaskDesc = "MODDRAWER"
                            Case DOCADDFOLDER
                                sTaskDesc = "ADDFOLDER"
                            Case DOCDELFOLDER
                                sTaskDesc = "DELFOLDER"
                            Case DOCMODFOLDER
                                sTaskDesc = "MODFOLDER"
                            Case DOCADDDOCUMENT
                                sTaskDesc = "ADDDOCUMENT"
                            Case DOCDELDOCUMENT
                                sTaskDesc = "DELDOCUMENT"
                            Case DOCMODDOCUMENT
                                sTaskDesc = "MODDOCUMENT"
                        End Select

                        m_sLogMess(1) = "DocuMaster API Daemon - REMOTE HISTORY TASK, " & sTaskDesc

                        m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)

                    End If

                    ErrFillStruct = True
                    ErrCommitHDB = False

                    ' Fill structue from record details

                    utDMSHistParams.NewRec.cabinetcode.Value = vResultArray(2, l)

                    utDMSHistParams.NewRec.cabinetname.Value = vResultArray(3, l)

                    utDMSHistParams.NewRec.drawercode.Value = vResultArray(4, l)

                    utDMSHistParams.NewRec.drawername.Value = vResultArray(5, l)

                    utDMSHistParams.NewRec.foldercode.Value = vResultArray(6, l)

                    utDMSHistParams.NewRec.foldername.Value = vResultArray(7, l)

                    utDMSHistParams.NewRec.docref.Value = vResultArray(8, l)

                    utDMSHistParams.NewRec.date_Renamed.Value = vResultArray(9, l)

                    utDMSHistParams.NewRec.time.Value = vResultArray(10, l)

                    utDMSHistParams.NewRec.eventtype.Value = vResultArray(11, l)

                    utDMSHistParams.NewRec.description.Value = vResultArray(12, l)

                    utDMSHistParams.NewRec.volume.Value = vResultArray(13, l)

                    utDMSHistParams.NewRec.pagefile.Value = vResultArray(14, l)

                    utDMSHistParams.NewRec.doctype.Value = vResultArray(15, l)
                    utDMSHistParams.NewRec.filler.Value = New String(" "c, 13)

                    Try

                        ConvertParamToString(utDMSHistParams, g_sHistParam.Value)

                        Select Case (vResultArray(1, l))
                            Case DOCADDCABINET
                                ' Add Cabinet to history database
                                NewCab(g_sHistParam.Value)
                            Case DOCDELCABINET
                                ' Delete Cabinet to history database
                                DelCab(g_sHistParam.Value)
                            Case DOCMODCABINET
                                ' Modify Cabinet to history database
                                ModCab(g_sHistParam.Value)
                            Case DOCADDDRAWER
                                ' Add Drawer to history database
                                NewDrw(g_sHistParam.Value)
                            Case DOCDELDRAWER
                                ' Delete Drawer to history database
                                DelDrw(g_sHistParam.Value)
                            Case DOCMODDRAWER
                                ' Modify Drawer to history database
                                ModDrw(g_sHistParam.Value)
                            Case DOCADDFOLDER
                                ' Add Folder to history database
                                NewFld(g_sHistParam.Value)
                            Case DOCDELFOLDER
                                ' Delete Folder to history database
                                DelFld(g_sHistParam.Value)
                            Case DOCMODFOLDER
                                ' Modify Folder to history database
                                ModFld(g_sHistParam.Value)
                            Case DOCADDDOCUMENT
                                ' Add Document to history database
                                NewDoc(g_sHistParam.Value)
                            Case DOCDELDOCUMENT
                                ' Delete Document to history database
                                DelDoc(g_sHistParam.Value)
                            Case DOCMODDOCUMENT
                                ' Modify Document to history database
                                ModDoc(g_sHistParam.Value)
                        End Select

                        Debug.WriteLine("History Return Code : " & sReturnCode.Value)

                        'DoEvents

                        'Get the return codes
                        ExtractReturnCodesFromParam(sReturnCode.Value, sFileStatus.Value, g_sHistParam.Value)

                        If sFileStatus.Value.StartsWith("9") Then
                            Debug.WriteLine("History FileStatus Code : " & sFileStatus.Value.Substring(0, 1) & ", " & CStr(Strings.Asc(sFileStatus.Value.Substring(1, 1)(0))))
                        Else
                            Debug.WriteLine("History FileStatus Code : " & sFileStatus.Value)
                        End If

                        If Not m_bRebuild Then
                            Select Case (vResultArray(1, l))
                                Case DOCADDCABINET, DOCMODCABINET


                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l)) & ", " & CStr(vResultArray(3, l))
                                Case DOCDELCABINET

                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))
                                Case DOCADDDRAWER, DOCMODDRAWER

                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))


                                    m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l)) & ", " & CStr(vResultArray(5, l))
                                Case DOCDELDRAWER

                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))

                                    m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l))
                                Case DOCADDFOLDER, DOCMODFOLDER

                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))

                                    m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l))


                                    m_sLogMess(4) = "Folder - " & CStr(vResultArray(6, l)) & ", " & CStr(vResultArray(7, l))
                                Case DOCDELFOLDER

                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))

                                    m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l))

                                    m_sLogMess(4) = "Folder - " & CStr(vResultArray(6, l))
                                Case DOCADDDOCUMENT, DOCDELDOCUMENT, DOCMODDOCUMENT

                                    m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))

                                    m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l))

                                    m_sLogMess(4) = "Folder - " & CStr(vResultArray(6, l))


                                    m_sLogMess(5) = "Document - " & CStr(vResultArray(8, l)) & ", " & CStr(vResultArray(12, l))
                            End Select
                        End If

                        If sReturnCode.Value = "0000" Then
                            ' Append a log message to todays log file

                            If Not m_bRebuild Then

                                Select Case (vResultArray(1, l))
                                    Case DOCADDCABINET
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet created"
                                    Case DOCDELCABINET
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet deleted"
                                    Case DOCMODCABINET
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet modified"
                                    Case DOCADDDRAWER
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer created"
                                    Case DOCDELDRAWER
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer deleted"
                                    Case DOCMODDRAWER
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer modified"
                                    Case DOCADDFOLDER
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder created"
                                    Case DOCDELFOLDER
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder deleted"
                                    Case DOCMODFOLDER
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder modified"
                                    Case DOCADDDOCUMENT
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document created"
                                    Case DOCDELDOCUMENT
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document deleted"
                                    Case DOCMODDOCUMENT
                                        m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document modified"
                                End Select


                                m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)

                            End If

                            'OK, so delete the history record

                            DeleteHistoryRecord(CInt(vResultArray(0, l)))

                        Else
                            ' History DLL failed, log message
                            '
                            ' Append a log message to todays log file

                            Select Case (vResultArray(1, l))
                                Case DOCADDCABINET
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet create failed. "
                                Case DOCDELCABINET
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet delete failed. "
                                Case DOCMODCABINET
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet modify failed. "
                                Case DOCADDDRAWER
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer create failed. "
                                Case DOCDELDRAWER
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer delete failed. "
                                Case DOCMODDRAWER
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer modify failed. "
                                Case DOCADDFOLDER
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder create failed. "
                                Case DOCDELFOLDER
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder delete failed. "
                                Case DOCMODFOLDER
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder modify failed. "
                                Case DOCADDDOCUMENT
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document create failed. "
                                Case DOCDELDOCUMENT
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document delete failed. "
                                Case DOCMODDOCUMENT
                                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document modify failed. "
                            End Select

                            If sFileStatus.Value.StartsWith("9") Then
                                m_sLogMess(1) = m_sLogMess(1) & "RC : " & sReturnCode.Value & "  FS : " & sFileStatus.Value.Substring(0, 1) & ", " & CStr(Strings.Asc(sFileStatus.Value.Substring(1, 1)(0)))
                            Else
                                m_sLogMess(1) = m_sLogMess(1) & "RC : " & sReturnCode.Value & "  FS : " & sFileStatus.Value
                            End If

                            'salvo (091296) - If filestatus is 22, ie duplicate key, log as a 'LOG' instead of 'ERR' and delete the record
                            'salvo (270197) - If return code is 0002, 0007, 0012, 0017 (ie read cab, draw, fold or doc failed)
                            '                 because of FS 23 (ie key not found) then make an equivalent ADD request
                            '                 instead of failing.

                            If sFileStatus.Value = "22" Then

                                If Not m_bRebuild Then

                                    m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)
                                End If
                                ' Delete record now finished processing

                                DeleteHistoryRecord(CInt(vResultArray(0, l)))

                            Else
                                If sFileStatus.Value = "23" And (sReturnCode.Value = "0002" Or sReturnCode.Value = "0007" Or sReturnCode.Value = "0012" Or sReturnCode.Value = "0017") Then

                                    If Not m_bRebuild Then
                                        'salvo (270197) - this actually ok, just do as ADD instead

                                        m_oPMBLog.DOCiPMFunc.LogMessage(LLOG, "DMSAPI", m_sLogMess)
                                    End If


                                    If AddToRHDBInstead(utDMSHistParams, vResultArray, l) Then

                                        ' Delete record now finished processing

                                        DeleteHistoryRecord(CInt(vResultArray(0, l)))

                                    Else

                                        ' Set hderror flag to true to say
                                        ' that an error has occurred

                                        m_lReturn = SetHistoryRecordError(CInt(vResultArray(0, l)))

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            result = False
                                        End If

                                    End If

                                Else


                                    m_oPMBLog.DOCiPMFunc.LogMessage(LERR, "DMSAPI", m_sLogMess)

                                    ' Set hderror flag to true to say
                                    ' that an error has occurred

                                    m_lReturn = SetHistoryRecordError(CInt(vResultArray(0, l)))

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = False
                                    End If

                                End If

                            End If

                        End If

                    Catch
                    End Try



                    'iCount = iCount + 1

                    ' keep a running total of all records processed
                    lCurrentRec += 1

                    ' update progress form (every 5 recs)
                    'lCnt1& = lCnt1& + 1
                    ' If lCnt1& > 4 Then
                    '     lCnt1& = 0
                    objfrmInterface.proProgress.Value = lCurrentRec

                    'To Do
                    'objfrmInterface.panCurrentFile.Caption = "History Records Processed : " & _
                    'lCurrentRec & " of " & CStr(lHistoryRowCount)
                    ' End If

                Next l

                'read last record
                'the Next statement above adds an extra 1 tp l& so need to take it away

                lHistRecNum = CInt(vResultArray(0, l - 1))

            Loop

            ' Close the log file

            m_oPMBLog.CloseLogFile(m_sHistoryRoot)

            'update progress form
            objfrmInterface.proProgress.Value = objfrmInterface.proProgress.Maximum

            Return result

        Catch excep As System.Exception
            If Not ErrFillStruct And Not ErrCommitHDB Then
                Throw excep
            End If


            If ErrFillStruct Then


                Return False

            End If
            If ErrCommitHDB Or ErrFillStruct Then


                result = False
                ' Log Error Message

                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CommitHDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
        End Try
    End Function


    Private Function AddToRHDBInstead(ByRef utDMSHistParams As MainModule.g_utDMSHistParams, ByRef vResultArray(,) As Object, ByRef l As Integer) As Integer

        'salvo(270197) - This section is called when a MOD request fails because the key was not found.
        '                It requests to ADD the index instead, on the assumption that if its not there,
        '                it should be. It is passed the array of history records, and the index (l) of
        '                the one to be re-added

        Dim result As Integer = 0
        Dim sReturnCode As New FixedLengthString(4)
        Dim sFileStatus As New FixedLengthString(2)




        result = True

        ReDim m_sLogMess(5)

        ' Fill structure from record details

        utDMSHistParams.NewRec.cabinetcode.Value = vResultArray(2, l)

        If CStr(vResultArray(3, l)).Trim() <> "" Then

            utDMSHistParams.NewRec.cabinetname.Value = vResultArray(3, l)
        Else
            utDMSHistParams.NewRec.cabinetname.Value = utDMSHistParams.NewRec.cabinetcode.Value
        End If


        utDMSHistParams.NewRec.drawercode.Value = vResultArray(4, l)

        If CStr(vResultArray(5, l)).Trim() <> "" Then

            utDMSHistParams.NewRec.drawername.Value = vResultArray(5, l)
        Else
            utDMSHistParams.NewRec.drawername.Value = utDMSHistParams.NewRec.drawername.Value
        End If


        utDMSHistParams.NewRec.foldercode.Value = vResultArray(6, l)

        If CStr(vResultArray(7, l)).Trim() <> "" Then

            utDMSHistParams.NewRec.foldername.Value = vResultArray(7, l)
        Else
            utDMSHistParams.NewRec.foldername.Value = utDMSHistParams.NewRec.foldername.Value
        End If


        utDMSHistParams.NewRec.docref.Value = vResultArray(8, l)

        utDMSHistParams.NewRec.date_Renamed.Value = vResultArray(9, l)

        utDMSHistParams.NewRec.time.Value = vResultArray(10, l)

        utDMSHistParams.NewRec.eventtype.Value = vResultArray(11, l)

        utDMSHistParams.NewRec.description.Value = vResultArray(12, l)

        utDMSHistParams.NewRec.volume.Value = vResultArray(13, l)

        utDMSHistParams.NewRec.pagefile.Value = vResultArray(14, l)

        utDMSHistParams.NewRec.doctype.Value = vResultArray(15, l)
        utDMSHistParams.NewRec.filler.Value = New String(" "c, 13)


        ConvertParamToString(utDMSHistParams, g_sHistParam.Value)

        Select Case (vResultArray(1, l))
            Case DOCMODCABINET
                ' Add Cabinet to history database
                NewCab(g_sHistParam.Value)
            Case DOCMODDRAWER
                ' Add Drawer to history database
                NewDrw(g_sHistParam.Value)
            Case DOCMODFOLDER
                ' Add Folder to history database
                NewFld(g_sHistParam.Value)
            Case DOCMODDOCUMENT
                ' Add Document to history database
                NewDoc(g_sHistParam.Value)
        End Select

        ExtractReturnCodesFromParam(sReturnCode.Value, sFileStatus.Value, g_sHistParam.Value)

        Select Case (vResultArray(1, l))
            Case DOCMODCABINET


                m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l)) & ", " & CStr(vResultArray(3, l))
            Case DOCMODDRAWER

                m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))


                m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l)) & ", " & CStr(vResultArray(5, l))
            Case DOCMODFOLDER

                m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))

                m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l))


                m_sLogMess(4) = "Folder - " & CStr(vResultArray(6, l)) & ", " & CStr(vResultArray(7, l))
            Case DOCMODDOCUMENT

                m_sLogMess(2) = "Cabinet - " & CStr(vResultArray(2, l))

                m_sLogMess(3) = "Drawer - " & CStr(vResultArray(4, l))

                m_sLogMess(4) = "Folder - " & CStr(vResultArray(6, l))


                m_sLogMess(5) = "Document - " & CStr(vResultArray(8, l)) & ", " & CStr(vResultArray(12, l))
        End Select

        If sReturnCode.Value = "0000" Then

            ' Append a log message to todays log file
            Select Case (vResultArray(1, l))
                Case DOCMODCABINET
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet created instead of modified"
                Case DOCMODDRAWER
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer created instead of modified"
                Case DOCMODFOLDER
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder created instead of modified"
                Case DOCMODDOCUMENT
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document created instead of modified"
            End Select


            m_lReturn = m_oLog.DOCiPMFunc.LogMessage(LERR, "DMSAPI", m_sLogMess)

        Else
            ' History DLL failed, log message
            '
            ' Append a log message to todays log file
            Select Case (vResultArray(1, l))
                Case DOCMODCABINET
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Cabinet create instead of modify failed. "
                Case DOCMODDRAWER
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Drawer create instead of modify failed. "
                Case DOCMODFOLDER
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Folder create instead of modify failed. "
                Case DOCMODDOCUMENT
                    m_sLogMess(1) = "DocuMaster API Daemon - Remote History Document create instead of modify failed. "
            End Select

            If sFileStatus.Value.StartsWith("9") Then
                m_sLogMess(1) = m_sLogMess(1) & "RC : " & sReturnCode.Value & "  FS : " & sFileStatus.Value.Substring(0, 1) & ", " & CStr(Strings.Asc(sFileStatus.Value.Substring(1, 1)(0)))
            Else
                m_sLogMess(1) = m_sLogMess(1) & "RC : " & sReturnCode.Value & "  FS : " & sFileStatus.Value
            End If


            m_lReturn = m_oLog.DOCiPMFunc.LogMessage(LERR, "DMSAPI", m_sLogMess)

            result = False

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Function Name: GetIndexListData
    '
    ' Description:  Reads a line from the index file and saves
    '               the contents in an array to be passed out
    '               for use.
    '
    ' Edit History:
    ' JH061098 -    Data Merge.  No actual changes but same function is
    '               used for merge index which uses different info
    '               in the piped file so lots of comments added.
    '
    ' ***************************************************************** '

    Private Function GetIndexListData(ByRef vIndexListData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sIndexDesc As String = ""
        Dim iStrCntr As Integer

        Dim cabinetcode As New StringBuilder 'refers to old code if mergeindex
        Dim cabinetname As New StringBuilder 'refers to new code if mergeindex
        Dim drawercode As New StringBuilder 'refers to old code if mergeindex
        Dim drawername As New StringBuilder 'refers to new code if mergeindex
        Dim foldercode As New StringBuilder 'refers to old code if mergeindex
        Dim foldername As New StringBuilder 'refers to new code if mergeindex

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Check if we are at the end of the
            ' journal file
            If FileSystem.EOF(g_iIndexData) Then
                ' Reached end of file
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the next line from the journal file
            sIndexDesc = FileSystem.LineInput(g_iIndexData)

            cabinetcode = New StringBuilder("")
            cabinetname = New StringBuilder("")
            drawercode = New StringBuilder("")
            drawername = New StringBuilder("")
            foldercode = New StringBuilder("")
            foldername = New StringBuilder("")

            If sIndexDesc.Trim().Length > 1 Then
                iStrCntr = 1

                ' Get cabinet code (old code)
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    cabinetcode.Append(sIndexDesc.Substring(iStrCntr - 1, 1))
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get cabinet name (new code)
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    cabinetname.Append(sIndexDesc.Substring(iStrCntr - 1, 1))
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get drawer code (old code)
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    drawercode.Append(sIndexDesc.Substring(iStrCntr - 1, 1))
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get drawer name (new code)
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    drawername.Append(sIndexDesc.Substring(iStrCntr - 1, 1))
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get folder code (old code - may be blank)
                While (sIndexDesc.Substring(iStrCntr - 1, 1) <> "|")
                    foldercode.Append(sIndexDesc.Substring(iStrCntr - 1, 1))
                    iStrCntr += 1
                End While
                iStrCntr += 1

                ' Get folder name (new code - may be blank)
                For iStrCntr = iStrCntr To sIndexDesc.Length
                    foldername.Append(sIndexDesc.Substring(iStrCntr - 1, 1))
                Next iStrCntr


                'Set up the codes in an array the new API will understand
                If cabinetcode.ToString() <> "" Then

                    ReDim vIndexListData(1, 0)

                    vIndexListData(0, 0) = cabinetcode.ToString()

                    vIndexListData(1, 0) = cabinetname.ToString()

                    If drawercode.ToString() <> "" Then

                        ReDim Preserve vIndexListData(1, 1)

                        vIndexListData(0, 1) = drawercode.ToString()

                        vIndexListData(1, 1) = drawername.ToString()

                        If foldercode.ToString() <> "" Then 'may be blank

                            ReDim Preserve vIndexListData(1, 2)

                            vIndexListData(0, 2) = foldercode.ToString()

                            vIndexListData(1, 2) = foldername.ToString()

                        End If

                    End If

                End If

            End If


            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Private Function OpenIndexListData(ByRef sIndexListName As String) As Integer

        Dim result As Integer = 0
        result = True

        Try

            ' Find free file number and open
            g_iIndexData = FileSystem.FreeFile()
            FileSystem.FileOpen(g_iIndexData, sIndexListName, OpenMode.Input)

            Return result

        Catch



            Return False
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: ConvertParamToString (Standard Method)
    '
    ' Description: This proc converts the history dll's parameters to
    ' to a single string. This is required as VB5 handles user types
    ' differently to VB3 and cannot pass them to the new 32 bit cobol
    ' history DLL.
    '
    '   Expected Linkage length = 376 Bytes.
    '
    ' ***************************************************************** '
    Private Sub ConvertParamToString(ByRef utParams As MainModule.g_utDMSHistParams, ByRef sParam As String)




        sParam = ""

        'Set up the parameter string from the old user defined type
        sParam = utParams.NewRec.cabinetcode.Value
        sParam = sParam & utParams.NewRec.cabinetname.Value
        sParam = sParam & utParams.NewRec.drawercode.Value
        sParam = sParam & utParams.NewRec.drawername.Value
        sParam = sParam & utParams.NewRec.foldercode.Value
        sParam = sParam & utParams.NewRec.foldername.Value
        sParam = sParam & utParams.NewRec.docref.Value
        sParam = sParam & utParams.NewRec.date_Renamed.Value
        sParam = sParam & utParams.NewRec.time.Value
        sParam = sParam & utParams.NewRec.eventtype.Value
        sParam = sParam & utParams.NewRec.description.Value
        sParam = sParam & utParams.NewRec.volume.Value
        sParam = sParam & utParams.NewRec.pagefile.Value
        sParam = sParam & utParams.NewRec.doctype.Value
        sParam = sParam & New String(" "c, 13)
        sParam = sParam & utParams.DMSDir.Value
        sParam = sParam & utParams.ReturnCode.Value
        sParam = sParam & utParams.FileStatus.Value


    End Sub
    ' ***************************************************************** '
    ' Name: ExtractReturnCodesFromParam (Standard Method)
    '
    ' Description: This proc extracts the return code and file status
    ' from the parameter after a call to the history dll
    '
    '   Expected Linkage length = 376 Bytes.
    '
    '   Return Code Position    = 371-374
    '   File Status Position    = 375-376
    ' ***************************************************************** '
    Private Sub ExtractReturnCodesFromParam(ByRef sReturnCode As String, ByRef sFileStatus As String, ByRef sParam As String)

        Const iRCStartPos As Integer = 371
        Const iRCLength As Integer = 4
        Const iFSStartPos As Integer = 375
        Const iFSLength As Integer = 2




        sReturnCode = ""
        sFileStatus = ""

        'Pull out what we want to see
        sReturnCode = sParam.Substring(iRCStartPos - 1, Math.Min(sParam.Length, iRCLength))
        sFileStatus = sParam.Substring(iFSStartPos - 1, Math.Min(sParam.Length, iFSLength))


    End Sub



    ' ***************************************************************** '
    ' Name: DeleteHistoryRecord (Standard Method)
    '
    ' Description: After successful processing of a history record, this
    ' procedure deletes it.
    '
    ' ***************************************************************** '
    Private Sub DeleteHistoryRecord(ByRef lHistoryID As Integer)

        Dim sSQL As String = ""




        'Construct SQL
        sSQL = "DELETE FROM DOC_history WHERE history_id = " & lHistoryID

        'Hit DB
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEHISTORYRECORD", bStoredProcedure:=False)

        'Log failed message
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete History Record in PMDAO.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteHistoryRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End If


    End Sub
    ' ***************************************************************** '
    ' Name: SetHistoryRecordError (Standard Method)
    '
    ' Description: After successful processing of a history record, this
    ' procedure deletes it.
    '
    ' ***************************************************************** '
    Private Function SetHistoryRecordError(ByRef lHistoryID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        'Construct SQL
        sSQL = "UPDATE DOC_history SET hderror = 'Y' WHERE history_id = " & lHistoryID

        'Hit DB
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SETHISTORYRECORDERROR", bStoredProcedure:=False)

        'Log failed message
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update history record in PMDAO.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetHistoryRecordError", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Function Name: SwapHistoryRoot - private
    '
    ' JH260399
    '
    ' Description:  Goodhealth want the ability to use
    ' multiple history roots.  Store the list of DMS drives
    ' in registry, this function takes the next drive from
    ' the list and changes the 'nextdrive' registry entry.
    ' Also if the registry entry doesn't exist then this
    ' calls the function to set it.
    '
    ' Edit History:
    '
    '
    ' ***************************************************************** '

    Private Function SwapHistoryRoot() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sNextDrive, sTmp As String
        Dim iButton As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily




        result = gPMConstants.PMEReturnCode.PMTrue

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryRootKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCDaemonSection)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'there isn't a registry setting, so no
            'swapping drive letters is needed
        Else
            If sTmp <> "Y" Then
                'so they can turn off without deleting key
                Return result
            End If
        End If

        'get NextDrive from registry

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryNextDriveKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCMultipleHistoryRootSection)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            While (sTmp = "")

                'lost next drive setting or need to set

                iButton = MessageBox.Show("Do you wish to use multiple Remote History drive(s)?", "Remote History Drives", MessageBoxButtons.YesNoCancel)

                If iButton = System.Windows.Forms.DialogResult.Yes Then

                    m_lReturn = GetDOCRegSettings(vHistoryRoot:=m_sHistoryRoot, bOverWrite:=True)

                    'get NextDrive from registry

                    m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryNextDriveKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCMultipleHistoryRootSection)
                Else
                    'if cancel then just exit this swapping function, it
                    'will run again next time round, if no then
                    'exit after turning off registry setting
                    If iButton = System.Windows.Forms.DialogResult.No Then
                        m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryRootKey, v_sSettingValue:="N", v_sSubKey:=DOCDaemonSection)

                    End If

                    Return result

                End If
            End While

        End If

        sNextDrive = sTmp

        'get value of NextDrive

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sNextDrive, r_sSettingValue:=sTmp, v_sSubKey:=DOCMultipleHistoryRootSection)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            'something wrong so don't swap

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Journal Root", vApp:=ACApp, vClass:=ACClass, vMethod:="SwapHistoryRoot", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result

        Else

            m_sHistoryRoot = sTmp

        End If

        'save to registry
        m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCHistoryRootKey, v_sSettingValue:=m_sHistoryRoot, v_sSubKey:=DOCDaemonSection)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Journal Root", vApp:=ACApp, vClass:=ACClass, vMethod:="SwapHistoryRoot", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If

        'get drive after NextDrive
        'take the drive number (eg. Drive1) and add 1
        'if none then go back to Drive1

        sNextDrive = "Drive" & Conversion.Val(sNextDrive.Substring(5, sNextDrive.Length - 5)) + 1
        'need to use len -5 in case we get into double digits
        'eg. Drive10

        'so if this variable exists in the registry then
        'save to 'NextDrive', if not then save 'Drive1'

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sNextDrive, r_sSettingValue:=sTmp, v_sSubKey:=DOCMultipleHistoryRootSection)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sNextDrive = "Drive1"

        Else
            If sTmp = "" Then
                sNextDrive = "Drive1"
            End If
        End If

        'change NextDrive to the correct next one
        'save to registry
        m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryNextDriveKey, v_sSettingValue:=sNextDrive, v_sSubKey:=DOCMultipleHistoryRootSection)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Journal Root", vApp:=ACApp, vClass:=ACClass, vMethod:="SwapHistoryRoot", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If


        Return result

    End Function
End Class