Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Commit_NET.Commit")> _
Public NotInheritable Class Commit
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
    '
    ' DN270802 - Change embedded SQL to reflect table changes
    '
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As DPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    ' Write history record object
    Private m_oHistory As bDOCHistory.Form

    ' Miscellaneous Class
    Private m_oMisc As bDOCCommitServer.Miscellaneous

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceID As Integer

    Private m_oPMWrk As bPMWrkTaskInstance.FormClass

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


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
    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '
    Public ReadOnly Property PMProductFamily() As Integer
        Get

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername = sUsername
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

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New DPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = gPMComponentServices.NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            'Get the commit business object that writes to history table
            m_oHistory = New bDOCHistory.Form()

            m_lReturn = m_oHistory.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Instantantiate miscellaneous class
            m_oMisc = New bDOCCommitServer.Miscellaneous()

            m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase, vHistory:=m_oHistory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckCommitLock
    '
    ' Description: Check registry to see if another app currently
    ' commiting.
    '
    ' ***************************************************************** '
    Public Function CheckCommitLock(ByRef bLocked As Boolean) As Integer

        Dim result As Integer = 0
        Dim sTmp As String = ""
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            'get setting
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCCommitLockKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCScanSection)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCCommitLockKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCommitLock", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            bLocked = (sTmp = "Y")

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCommitLock", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RemoveCommitLock
    '
    ' Description: Remove lock in registry.
    '
    ' ***************************************************************** '
    Public Function RemoveCommitLock() As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            'write to registry
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCCommitLockKey, v_sSettingValue:="N", v_sSubKey:=DOCScanSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCCommitLockKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveCommitLock", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveCommitLock", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PlaceCommitLock (Private)
    '
    ' Description: Write a lock to registry so dont have scanners
    '               commiting simultaneously.
    '
    ' ***************************************************************** '
    Public Function PlaceCommitLock() As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            'write to registry
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCCommitLockKey, v_sSettingValue:="Y", v_sSubKey:=DOCScanSection)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set " & DOCCommitLockKey & " in Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="PlaceCommitLock", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PlaceCommitLock", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNextPageName
    '
    ' Description: Wrapper call to main GetNextPageName procedure.
    '
    ' ***************************************************************** '
    Public Function GetNextPageName(ByRef sNextPageName As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oMisc.GetNextPageName(sNextPageName:=sNextPageName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(gPMConstants.PMELogLevel.PMLogError), vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPageName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oMisc = Nothing
                m_oHistory = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                m_oMisc = Nothing
                m_oHistory = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)


    ' ***************************************************************** '
    ' Name: BeginTrans (public)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetScanFolderNum
    '
    ' Description: Get number of stand alone scan folder- return 0 if
    ' doesn't exist
    '
    ' ***************************************************************** '
    Public Function GetScanFolderNum(ByRef lFolderNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "SELECT folder_num FROM DOC_folder WHERE folder_name = '" &
                     DOCDefaultScanFolder & "'"

            ' hit db
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETSCANFOLDNUM", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return folder num
            If Informations.IsArray(vResultArray) Then

                lFolderNum = CInt(vResultArray(0, 0))
            Else
                lFolderNum = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetScanFolderNum", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (public)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDataPath
    '
    ' Description: This function is a wrapper call to function proper
    ' in the miscellaneous class.
    '
    ' ***************************************************************** '
    Public Function GetDataPath(ByRef lVolumeID As Integer, ByRef sDataPath As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oMisc.GetDataPath(lVolumeID:=lVolumeID, sDataPath:=sDataPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataPath", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddDocToHistory
    '
    ' Description: This is wrapper to the function proprer in the
    ' miscellaneous class.
    '
    ' ***************************************************************** '
    Public Function AddDocToHistory(ByRef lDocNum As Integer, ByRef lFoldNum As Integer, ByRef sDocName As String, ByRef dDocDate As Date, ByRef sPageName As String, ByRef sDocType As String, ByRef sPageType As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'call misc function
            m_lReturn = m_oMisc.AddDocToHistory(lDocNum:=lDocNum, lFoldNum:=lFoldNum, sDocName:=sDocName, dDocDate:=dDocDate, sPageName:=sPageName, sDocType:=sDocType, sPageType:=sPageType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocToHistory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RollbackTrans (public)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


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
        'bPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetSADatabase (public)
    '
    ' Description: Gets the current SA database as returns a link to it.
    '
    ' ***************************************************************** '
    Public Function GetSADatabase(ByRef vDatabase As Object) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oDatabase As DPMDAO.Database
#Else
        Dim oDatabase As Object = Nothing
#End If

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get SA database link
            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=False, r_oCheckedDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            vDatabase = oDatabase

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSADatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSADatabase", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: IsSBOInstalled (public)
    '
    ' Description: Gets the current SA database as returns a link to it.
    '
    ' ***************************************************************** '
    Public Function IsSBOInstalled(ByRef bInstalled As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get SA database link
            m_lReturn = CheckPMProductInstalled(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bInstalled:=bInstalled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsSBOInstalled Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsSBOInstalled", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : TaskCommit (public)
    '
    ' Description   : Function to Commit the Task.
    ' Edit History  :
    ' RAM20021218   : Changed the r_lPMWrkTaskInstanceCnt parameter type
    '                   from ByVal To ByRef
    '                 Ref. NRMA Project Changes. Sirius Process No.189
    '                 Note : Changes made by Kevin Renshaw (CMG),
    '                        Checked-in by Ram Chandrabose
    ' ***************************************************************** '
    Public Function TaskCommit(ByVal v_lPMWrkTaskID As Integer, ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_dtDateCreated As Date, ByVal v_iCreatedByID As Integer, ByVal v_iUserID As Integer, ByVal v_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vDatabase As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' create task object
            m_oPMWrk = New bPMWrkTaskInstance.FormClass()

            ' get link to sa database
            m_lReturn = GetSADatabase(vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create instance of bPMWrkTaskInstance.business", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' initlise object with sa database
            m_lReturn = m_oPMWrk.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=CShort(g_iUserID), iSourceID:=CShort(g_iSourceID), iLanguageID:=CShort(g_iLanguageID), iCurrencyID:=CShort(g_iCurrencyID), iLogLevel:=CShort(g_iLogLevel), sCallingAppName:=g_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create instance of bPMWrkTaskInstance.business", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' create new scheduled task
            m_lReturn = m_oPMWrk.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=CShort(v_iTaskStatus), v_iIsUrgent:=CShort(v_iIsUrgent), r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_dtDateCreated:=v_dtDateCreated, v_iCreatedByID:=CShort(v_iCreatedByID), v_iUserID:=CShort(v_iUserID), v_vKeyArray:=v_vKeyArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create new scheduled task", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error in Task Commit", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    'developer guide no.101
    Public Function GetExCode(ByRef v_vExCode As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get max document number from documaster db
            sSQL = "select max(doc_num) from DOC_document"
            m_lReturn = m_oDatabase.SQLSelect(sSQL, "GetExCode", False, 50, vResultArray)
            'check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to create excode", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'assign excode as max doc number + 1
            ' CTAF 20031201 - Fix for empty databases - Start
            If Informations.IsArray(vResultArray) Then

                'developer guide no.101
                v_vExCode = (CDbl(vResultArray(0, 0)) + 1)
            Else
                v_vExCode = 1
            End If
            ' CTAF 20031201 - End

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error in GetExCode", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class