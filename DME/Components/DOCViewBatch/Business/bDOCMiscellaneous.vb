Option Strict Off
Option Explicit On
Imports System.IO
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Miscellaneous_NET.Miscellaneous")>
Public NotInheritable Class Miscellaneous
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
    ' JH051198 changed GetDataPath so that only the share name is accessed
    ' so that it is held in one place on the options screen.
    '
    ' JH301198 - changed GetDataPath validation of path put in for data share
    ' including the check for c:\ type of paths
    '
    ' JH200599 unbutcher GetDataPath to save to three strings after all
    '
    ' DN270802 - Change embedded SQL to reflect tbale changes
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Miscellaneous"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As Object
#End If
    ' Scan Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oScanDatabase As dPMDAO.Database
#Else
    Private m_oScanDatabase As dPMDAO.Database
#End If

    'History class
    Private m_oHistory As Object

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private m_iAdminLevel As Integer
    Private m_iAccessLevel As Integer

    Public WriteOnly Property ScanDatabase() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oScanDatabase = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: GetDataPath
    '
    ' Description: This function constructs the full path to a page file,
    ' excluding the actual page location in the 00 tree. It requires the
    ' volume ID from the page table.
    '
    ' It will consist of:
    '
    '       \\SERVER_UNC\SHARE(\DIRECTORY)
    '
    ' Edit History:
    ' JH051198 butchering this so that only the share name is accessed
    ' so that it is held in one place on the options screen.
    ' The share name will read \\SERVER_UNC\SHARE(\DIRECTORY)
    '
    ' JH301198
    ' or if it is server only setup it will be a path like c:\dme\data
    '
    ' JH200599 unbutcher it because we want it as three after all
    '
    '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    '
    ' MS  28/06/00  Briefcase download functionality
    '
    '               When in briefcase download mode or normal Documaster mode,
    '               read the directory path of the page file as from the db (server).
    '               When not in briefcase mode use the registry (BriefcaseDir)
    '               path if that exists to fetch and display page.
    '               (iDocOptions update the registry which ensures the correct path
    '                is read)
    ' ***************************************************************** '
    '
    Public Function GetDataPath(ByRef lVolumeID As Integer, ByRef sDataPath As String) As Integer

        Dim result As Integer = 0
        Dim lDeviceID As Integer
        Dim sDirectory, sServerUNC, sShareName As String
        Dim vResultArray(,) As Object = Nothing
        Dim sBriefcaseMode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' MS  28/06/00  >

            ' Get briefcase download status
            m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BRIEFCASEdownload", r_sSettingValue:=sBriefcaseMode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' If NOT in briefcase download mode, use Briefcase document store dir path if it exists
            ' in order to retrieve & display page files
            If sBriefcaseMode <> "ON" Then

                ' get the path for Briefcase dir path
                m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseDir", r_sSettingValue:=sDataPath), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    If sDataPath.Trim() <> "" Then
                        ' briefcase dir exists (as returned in sDataPath), exit function
                        Return result
                    End If
                End If

            End If


            ' If in briefcase download mode or normal Documaster process then get page dir
            ' path normally from the server

            ' MS  28/06/00  <


            'get device id
            m_sSQL = "SELECT device_id, directory FROM DOC_volume " & _
                     "WHERE volume_id = " & CStr(lVolumeID)

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GetDataPath1", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                'save details

                lDeviceID = CInt(vResultArray(0, 0))

                sDirectory = CStr(vResultArray(1, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get device_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataPath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            'Get device details
            m_sSQL = "SELECT server_unc, share_name FROM DOC_device " &
                     "WHERE device_id = " & CStr(lDeviceID)

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GetDataPath2", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                'save details

                sServerUNC = CStr(vResultArray(0, 0)).Trim()

                sShareName = CStr(vResultArray(1, 0)).Trim()

            Else
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get device details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataPath", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If

            'Now construct the data path

            'JH051198 if the user has just upgraded from
            'when the three used to be separate - we need to sort it

            'JH301198 fixed for when it starts with c:\

            If (Not sShareName.StartsWith("\\")) And (sShareName.Substring(1, Math.Min(sShareName.Length, 2)) <> ":\") Then

                'it's not terribly efficient I know but it'll work

                sDataPath = sServerUNC & sShareName

                If sDirectory.Length > 0 Then
                    sDataPath = sDataPath & sDirectory
                End If

                'and save this for the next time
                'JH200599 don't save it like this!

                '        m_sSQL$ = "UPDATE DOC_device SET share_name = '" & _
                ''                sDataPath$ & "' WHERE device_id = " & iDeviceID
                '
                '        'hit DB
                '        m_lReturn& = m_oDatabase.SQLAction( _
                ''            sSQL:=m_sSQL$, _
                ''            sSQLName:="GetDataPath3", _
                ''            bstoredprocedure:=False, _
                ''            lRecordsAffected:=1)
                '
                '        If (m_lReturn& <> PMTrue) Then
                '            GetDataPath = PMFalse
                '            Exit Function
                '        End If

            Else
                sDataPath = sShareName
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataPath", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetAdminLevel
    '
    ' Description: This gets and sets the administration level.
    '
    ' ***************************************************************** '
    Public Function GetAdminLevel(Optional ByRef iAdminLevel As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'get the administration level
            m_sSQL = "SELECT admin_level FROM DOC_system"

            'Hit the DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETADMINLEVEL", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'did we get anything back?
            If Informations.IsArray(vResultArray) Then

                'We have it, so store and leave

                m_iAdminLevel = CInt(vResultArray(0, 0))

                'return admin level as parameter if passed
                If True Then

                    iAdminLevel = CInt(vResultArray(0, 0))
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAdminLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oDatabase = Nothing
                m_oScanDatabase = Nothing
                m_oHistory = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing, Optional ByRef vScanDatabase As dPMDAO.Database = Nothing, Optional ByRef vHistory As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_iSourceID = v_iSourceID
            m_iLanguageID = v_iLanguageID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel

            'Store the database into the private member

            If Not Informations.IsNothing(vDatabase) Then
                m_oDatabase = vDatabase
            End If

            'Store the scan database into the private member

            If Not Informations.IsNothing(vScanDatabase) Then
                m_oScanDatabase = vScanDatabase
            End If

            'Store the history object into the private member

            If Not Informations.IsNothing(vHistory) Then
                m_oHistory = vHistory
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNextPageName
    '
    ' Description: This gets and returns the next page name from the
    ' system table, as well as updating it.
    '
    ' ***************************************************************** '
    Public Function GetNextPageName(ByRef sNextPageName As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNextPageName"

        Dim sFilename = " ", sTmp As String = " "
        Dim iDataLen = 0, iA, iB, iC, iD, iE As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim bStartTrans As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Start a transaction

            m_oDatabase.SQLBeginTrans()

            bStartTrans = True

            'Get next page name from system and open a lock for this transaction
            m_sSQL = "SELECT next_page FROM DOC_system WITH (UPDLOCK)"


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GetNextPageName", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.SQLSelect", "SELECT next_page FROM DOC_system WITH (UPDLOCK)", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then

                sFilename = CStr(vResultArray(0, 0))
            Else
                RaiseError("IsArray(vResultArray)", "SQL script did not return any lines", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sFilename.Length <> 15 Then
                RaiseError("Len(sFilename) <> 15", "Incorrect length of page name", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Return the next page name
            sNextPageName = sFilename

            'Get each of the numbers from the next page name
            iA = Val(Mid(sFilename, 2, 2))
            iB = Val(Mid(sFilename, 5, 2))
            iC = Val(Mid(sFilename, 8, 2))
            iD = Val(Mid(sFilename, 11, 2))
            iE = Val(Mid(sFilename, 14, 2))

            'Increment the next page name by 1
            iE += 1
            If iE > 99 Then
                iE = 0
                iD += 1
                If iD > 99 Then
                    iD = 0
                    iC += 1
                    If iC > 99 Then
                        iC = 0
                        iB += 1
                        If iB > 99 Then
                            iB = 0
                            iA += 1
                            If iA > 99 Then
                                RaiseError("iA > 99", "Page name exceeded maximum", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If
                End If
            End If

            'Create the new next page name
            ToSafeString(Mid(sFilename, 2, 2) = StringsHelper.Format(iA, "00"))
            ToSafeString(Mid(sFilename, 5, 2) = StringsHelper.Format(iB, "00"))
            ToSafeString(Mid(sFilename, 8, 2) = StringsHelper.Format(iC, "00"))
            ToSafeString(Mid(sFilename, 11, 2) = StringsHelper.Format(iD, "00"))
            ToSafeString(Mid(sFilename, 14, 2) = StringsHelper.Format(iE, "00"))

            'Update next page in system table
            m_sSQL = "UPDATE DOC_system SET next_page = '" & sFilename & "'"


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="UpdatePageName", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.SQLAction", "UPDATE DOC_system SET next_page = '" & sFilename & "'", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Finish the transaction (and unlock the table)

            m_lReturn = m_oDatabase.SQLCommitTrans


        Catch ex As Exception

            'Do not call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bStartTrans Then

                m_lReturn = m_oDatabase.SQLRollbackTrans
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ConstructDocRef (Standard Method)
    '
    ' Description: Given a doc num, this constructs the document reference
    ' which is used in the history database.
    '
    ' ***************************************************************** '
    Public Function ConstructDocRef(ByRef lDocNum As Integer, ByRef sDocRef As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDocRef = "000000000"

            ToSafeString(Mid(sDocRef, 10 - ((lDocNum).ToString).Trim().Length, ((lDocNum).ToString).Trim().Length) = ((lDocNum).ToString).Trim())

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ConstructDocRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteFolderTree
    '
    ' Description: Deletes a folder and all its children.
    '
    ' For each node the access level is checked. If the user does not
    ' have access level to folder, then the folder name is returned
    ' for reporting and function ends.
    '
    ' Also for each node, the ex code is checked to see if history
    ' needs updating.
    '
    ' Edit History :
    ' RAM20021212  : 1. The DOCCabinet, DOCDrawer, DOCFolder Constants are replaced
    '                   with new constants to support more than 3 levels
    '                2. Romoved code related to UpdateHistory, since we don't need it anymore
    ' ***************************************************************** '
    Private Function DeleteFolderTree(ByRef lFolderNum As Integer, ByRef sNoAccessName As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim bExternal As Boolean




        result = gPMConstants.PMEReturnCode.PMTrue

        'First get access level and ex code of folder
        sSQL = "SELECT access_level, ex_code, folder_level, folder_name " &
               "FROM DOC_folder WHERE folder_num = " & CStr(lFolderNum)

        'Hit DB

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GETACCESSEXCODE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'Check user has permission to delete this folder
        If Informations.IsArray(vResultArray) Then


            If (CDbl(vResultArray(0, 0))) < m_iAccessLevel Then


                sNoAccessName = CStr(vResultArray(3, 0))
                Return gPMConstants.PMEReturnCode.PMFalse
            Else

                bExternal = False

                'if this is an external version need to updatehistory

                If CStr(vResultArray(1, 0)).Trim() <> "" Then

                    'furthermore, if this an external version 2 folder, then
                    'make a note, as will have to update history for docs too,
                    'later

                    If CDbl(vResultArray(2, 0)) = DOCFolderLevelPolicy Then
                        bExternal = True
                    End If

                Else
                    bExternal = False
                End If
            End If

        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'Get the docs in the folder
        sSQL = "SELECT doc_num FROM DOC_document WHERE folder_num = " & lFolderNum

        'Hit DB

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GETDOCNUMS", bStoredProcedure:=False, vResultArray:=CType(vResultArray, Object))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'delete each doc in folder
        If Informations.IsArray(vResultArray) Then


            For I As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                'delete a doc

                m_lReturn = CType(DeleteDoc(lDocNum:=CInt(vResultArray(0, I)), bExternal:=bExternal, sNoAccessName:=sNoAccessName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            Next I

        End If

        'Now get the child folders
        sSQL = "SELECT folder_num FROM DOC_folder WHERE parent_num = " & lFolderNum

        'Hit DB

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GETFOLDNUMS", bStoredProcedure:=False, vResultArray:=CType(vResultArray, Object))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'loop thru array and delete each child folder
        If Informations.IsArray(vResultArray) Then

            For I As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                m_lReturn = DeleteFolderTree(lFolderNum:=CInt(vResultArray(0, I)), sNoAccessName:=sNoAccessName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            Next I
        End If

        'Now Delete the actual folder
        sSQL = "DELETE FROM DOC_folder WHERE folder_num = " & lFolderNum

        ' Execute SQL Statement

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(sSQL), sSQLName:="DELETEFOLDER", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DeleteDocKeyword
    '
    ' Description: This function deletes the document keyword.
    '
    ' ***************************************************************** '
    Public Function DeleteDocKeyword(ByRef lDocKeywordID As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "DELETE FROM DOC_doc_keyword WHERE doc_keyword_id = " & lDocKeywordID


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELDOCKW", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocKeyword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteFolders
    '
    ' Description: This function is the public method to be used for
    ' deleting a folder. It basically calls the function that actually
    ' deletes a folder and everything within.
    '
    ' ***************************************************************** '
    Public Function DeleteFolder(ByRef lFolderNum As Integer, ByRef sNoAccessName As String) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'delete the folder and all its contents
            m_lReturn = DeleteFolderTree(lFolderNum:=lFolderNum, sNoAccessName:=sNoAccessName)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetNodeParent
    '
    ' Description: Given a specific node number, this function returns its
    ' parent.
    '
    ' ***************************************************************** '
    Public Function GetNodeParent(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef lParentNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case iNodeType
                Case DOCNode_Folder
                    'Construct SQL
                    m_sSQL = "SELECT parent_num FROM DOC_folder WHERE folder_num = " & lNodeNum

                Case DOCNode_Document
                    'Construct SQL
                    m_sSQL = "SELECT folder_num FROM DOC_document WHERE doc_num = " & lNodeNum

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type - " & iNodeType, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeParent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End Select


            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETNODEPARENT", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the parent num.
            If Informations.IsArray(vResultArray) Then

                lParentNum = CInt(vResultArray(0, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeParent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetFolderTree
    '
    ' Description: Given a specific node number, this function returns
    ' the folder numbers and names of its full ancestry.
    '
    ' The first 2 elements will be the called folder num and name,
    ' and   vFolderArray(0,Ubound(vFolderArray)) = root folder num
    '       vFolderArray(1,Ubound(vFolderArray)) = root folder name
    '
    '
    ' ***************************************************************** '
    Public Function GetFolderTree(ByRef lFolderNum As Integer, ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lParent As Integer
        Dim sFolderName As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the supplied folder name
            m_lReturn = CType(GetNodeName(DOCNode_Folder, lFolderNum, sFolderName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ReDim vFolderArray(1, 0)

            'Set up first line of array

            vFolderArray(0, 0) = lFolderNum

            vFolderArray(1, 0) = sFolderName

            'Get the parent
            m_lReturn = CType(GetNodeParent(DOCNode_Folder, lFolderNum, lParent), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'loop until we have the root folder
            While lParent <> 0

                lFolderNum = lParent
                ReDim Preserve vFolderArray(1, vFolderArray.GetUpperBound(1) + 1)

                'Ge the parent name
                m_lReturn = CType(GetNodeName(DOCNode_Folder, lFolderNum, sFolderName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Set up next line of array

                vFolderArray(0, vFolderArray.GetUpperBound(1)) = lFolderNum

                vFolderArray(1, vFolderArray.GetUpperBound(1)) = sFolderName

                'Get the next parent
                m_lReturn = CType(GetNodeParent(DOCNode_Folder, lFolderNum, lParent), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End While


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderTree", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNodeExCode
    '
    ' Description: Given a specific node number, this function returns its
    ' external code. It also optionally returns the version 2 folder level.
    '
    ' ***************************************************************** '
    Public Function GetNodeExCode(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef sExCode As String, Optional ByRef iFolderLevel As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case iNodeType
                Case DOCNode_Folder

                    'Construct SQL
                    If Not True Then
                        m_sSQL = "SELECT ex_code FROM DOC_folder WHERE folder_num = " & lNodeNum
                    Else
                        m_sSQL = "SELECT ex_code, folder_level FROM DOC_folder WHERE folder_num = " & lNodeNum
                    End If

                Case DOCNode_Document

                    'Construct SQL
                    m_sSQL = "SELECT ex_code FROM DOC_document WHERE doc_num = " & lNodeNum

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type - " & iNodeType, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeExCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

            End Select


            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETNODEEXCODE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the ex code.
            If Informations.IsArray(vResultArray) Then


                sExCode = CStr(vResultArray(0, 0)).Trim()

                If Not False Then

                    iFolderLevel = CInt(vResultArray(1, 0))
                End If

            Else
                'no good at all
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Node type " & iNodeType & ", Node number " &
                           lNodeNum & " does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeExCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeExCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNodeName
    '
    ' Description: Given a specific node number, this function returns its
    ' name.
    '
    ' ***************************************************************** '
    Public Function GetNodeName(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef sNodeName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case iNodeType
                Case DOCNode_Folder
                    'Construct SQL
                    m_sSQL = "SELECT folder_name FROM DOC_folder WHERE folder_num = " & lNodeNum

                Case DOCNode_Document
                    'Construct SQL
                    m_sSQL = "SELECT doc_name FROM DOC_document WHERE doc_num = " & lNodeNum

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type - " & iNodeType, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeName", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End Select


            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETNODENAME", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the name.
            If Informations.IsArray(vResultArray) Then

                sNodeName = CStr(vResultArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetNodeAccessLevel
    '
    ' Description: Given a specific node number, this function returns its
    ' access level.
    '
    ' ***************************************************************** '
    Public Function GetNodeAccessLevel(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef iAccessLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case iNodeType
                Case DOCNode_Folder
                    'Construct SQL
                    m_sSQL = "SELECT access_level FROM DOC_folder WHERE folder_num = " & lNodeNum

                Case DOCNode_Document
                    'Construct SQL
                    m_sSQL = "SELECT access_level FROM DOC_document WHERE doc_num = " & lNodeNum

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type - " & iNodeType, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeAccessLevel", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

            End Select


            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETNODEACCESSLEVEL", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the access level.
            If Informations.IsArray(vResultArray) Then

                iAccessLevel = CInt(vResultArray(0, 0))

            Else

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Node number " & lNodeNum & " not found.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeAccessLevel", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeAccessLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RenameNode
    '
    ' Description: Given a specific node number, this function renames it
    '
    ' ***************************************************************** '
    Public Function RenameNode(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef sNewNodeName As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case iNodeType
                Case DOCNode_Folder
                    'Construct SQL to rename the folder
                    m_sSQL = "UPDATE DOC_folder " &
                             "SET folder_name = " & "'" & sNewNodeName & "' " &
                             "WHERE folder_num = " & CStr(lNodeNum)

                Case DOCNode_Document
                    'Construct SQL to rename the doc
                    m_sSQL = "UPDATE DOC_document " &
                             "SET doc_name = " & "'" & sNewNodeName & "' " &
                             "WHERE doc_num = " & CStr(lNodeNum)

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Type - " & iNodeType, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameNode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End Select

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="RENAMENODE", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameNode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RenameDoc
    '
    ' Description: This is essentially a wrapper to the rename node method
    ' that actually renames a doc - it also updates the hdb though
    '
    ' ***************************************************************** '
    Public Function RenameDoc(ByRef lDocNum As Integer, ByRef sNewName As String) As Integer

        Dim result As Integer = 0
        Dim bExternal As Boolean
        Dim dDocDate As Date
        Dim sDocRef = " ", sFoldCode = " ", sDrawCode = "", sCabCode As String = ""
        Dim lTmp1, lTmp2 As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'call function that actually does it
            m_lReturn = CType(RenameNode(iNodeType:=DOCNode_Document, lNodeNum:=lDocNum, sNewNodeName:=sNewName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'First check if destination is external version 2 folder, in which
            'case we will want to update the history database with a MODDOC
            m_lReturn = CType(AmIExternal(iNodeType:=DOCNode_Document, lNodeNum:=lDocNum, bExternal:=bExternal), gPMConstants.PMEReturnCode)



            If bExternal Then
                'it is external, so get ex codes of parent folder, draw and cab

                'get doc parent
                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=lDocNum, lParentNum:=lTmp1), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get fold ex code
                m_lReturn = CType(GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp1, sExCode:=sFoldCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get fold parent
                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp1, lParentNum:=lTmp2), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get draw ex code
                m_lReturn = CType(GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp2, sExCode:=sDrawCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get draw parent
                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp2, lParentNum:=lTmp1), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' get cab ex code
                m_lReturn = CType(GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp1, sExCode:=sCabCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get other required doc info first
                m_lReturn = CType(GetDocDate(lDocNum:=lDocNum, dDocDate:=dDocDate), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get the doc ref
                m_lReturn = CType(ConstructDocRef(lDocNum, sDocRef), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'hit the history table

                m_lReturn = m_oHistory.DirectAdd(vTask:=ToSafeInteger(DOCMODDOCUMENT), vCabinetCode:=ToSafeString(sCabCode), vCabinetName:="", vDrawerCode:=ToSafeString(sDrawCode), vDrawerName:="", vFolderCode:=ToSafeString(sFoldCode), vFolderName:="", vDocRef:=ToSafeString(sDocRef), vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"), vDescription:=ToSafeString(sNewName))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AmIExternal
    '
    ' Description: Determines whether a folder node is external or a
    ' document node is external.
    '
    ' (For a doc node, it is considered external if it belongs to an
    ' external version 2 folder)
    '
    ' ***************************************************************** '
    Public Function AmIExternal(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef bExternal As Boolean) As Integer

        Dim result As Integer = 0
        Dim lParentNum As Integer
        Dim sExCode As String = ""
        Dim iFolderLevel As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case iNodeType
                Case DOCNode_Folder

                    'Get the supplied folder ex code
                    m_lReturn = CType(GetNodeExCode(iNodeType:=iNodeType, lNodeNum:=lNodeNum, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    bExternal = (sExCode.Trim() <> "")

                Case DOCNode_Document

                    'get doc parent
                    m_lReturn = CType(GetNodeParent(iNodeType:=iNodeType, lNodeNum:=lNodeNum, lParentNum:=lParentNum), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get the  folder ex code
                    m_lReturn = CType(GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, sExCode:=sExCode, iFolderLevel:=iFolderLevel), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'only consider a doc external if it belongs to an external version
                    '2 folder
                    bExternal = ((sExCode.Trim() <> "") And (iFolderLevel = DOCFolder))

            End Select

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AmIExternal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocNames
    '
    ' Description: This returns all pre-defined document names.
    '
    ' ***************************************************************** '
    Public Function GetDocNames(ByRef vDocNamesArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'construct SQL
            m_sSQL = "SELECT doc_name, doc_name_id FROM DOC_doc_name ORDER BY doc_name"

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETDOCNAMES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=CType(vDocNamesArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocNames", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDocDateOffSets
    '
    ' Description: This the doc date offset and expiry date offset from
    ' the system table.
    '
    ' The doc date offset is a user configurable value which is
    ' subtracted from todays date to provide the doc date of scanned dgPMComponentServices.
    '
    ' The expiry date offset is a user configurable value which is
    ' added to todays date to document expiry date of scanned dgPMComponentServices.
    '
    ' ***************************************************************** '
    Public Function GetDocDateOffSets(ByRef iDocDateOffset As Integer, ByRef iExpiryDateOffset As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'construct SQL
            m_sSQL = "SELECT doc_date, expiry_date FROM DOC_system"

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETOFFSETS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                'This shouldn't be
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get dates from system table", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDateOffSets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            Else
                'return the values

                iDocDateOffset = -1 * CInt(vResultArray(0, 0))

                iExpiryDateOffset = CInt(vResultArray(1, 0))
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDateOffSets", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateHistory
    '
    ' Description: This section updates the history table for the supplied
    ' task. Its main work is to get the ancestry of external codes
    ' depending on the level.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateHistory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateHistory(ByRef iTask As Integer, ByRef lNodeNum As Integer, ByRef iNodeLevel As Integer, ByRef sNodeExCode As String, Optional ByRef dDocDate As Date = #12/30/1899#) As gPMConstants.PMEReturnCode
    '
    '
    'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    'Dim sCabExCode, sDrawExCode, sFoldExCode, sCabName, sDrawName, sFoldName, sDocRef, sDocDate, sDocTime As String
    '
    'Dim lParentNum1, lParentNum2 As Integer
    '
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'What level node ?
    '
    'Select Case iNodeLevel
    'Case DOCCabinet
    '
    'sCabExCode = sNodeExCode
    '
    'Case DOCDrawer
    '
    'sDrawExCode = sNodeExCode
    '
    'parent cabinet
    'm_lReturn = CType(GetNodeParent(DOCNode_Folder, lNodeNum, lParentNum1), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'm_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum1, sCabExCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'Case DOCFolder
    '
    'sFoldExCode = sNodeExCode
    '
    'Get parent drawer
    'm_lReturn = CType(GetNodeParent(DOCNode_Folder, lNodeNum, lParentNum1), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'm_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum1, sDrawExCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'get parent cabinet
    'm_lReturn = CType(GetNodeParent(DOCNode_Folder, lParentNum1, lParentNum2), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'm_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum2, sCabExCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'Case DOCDocument
    '
    'Work out the docref
    'm_lReturn = CType(ConstructDocRef(lNodeNum, sDocRef), gPMConstants.PMEReturnCode)
    '
    'Get parent folder details
    'm_lReturn = CType(GetNodeParent(DOCNode_Document, lNodeNum, lParentNum1), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'm_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum1, sFoldExCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'get parent drawer details
    'm_lReturn = CType(GetNodeParent(DOCNode_Folder, lParentNum1, lParentNum2), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'm_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum2, sDrawExCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'get parent cabinet details
    'm_lReturn = CType(GetNodeParent(DOCNode_Folder, lParentNum2, lParentNum1), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'm_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum1, sCabExCode), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'get the doc dates
    'sDocDate = dDocDate.ToString("yyyyMMdd")
    'sDocTime = dDocDate.ToString("HHMMss")
    '
    'Case Else
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Node Level - " & iNodeLevel, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateHistory", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    '
    'End Select
    '
    'Update the history database - note, if we are not actually linked to
    'an external database most of these params will be empty, but it matters
    'not as the call will not do anything.

    'm_lReturn = m_oHistory.DirectAdd(vTask:=iTask, vCabinetCode:=sCabExCode, vCabinetName:=sCabName, vDrawerCode:=sDrawExCode, vDrawerName:=sDrawName, vFolderCode:=sFoldExCode, vFolderName:=sFoldName, vDocRef:=sDocRef, vRequestDate:=sDocDate, vRequestTime:=sDocTime)
    '
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateHistory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: LinkOwner
    '
    ' Description: Given a doc num, this function checks if any other
    ' documents are linked to it.
    '
    ' ***************************************************************** '
    Public Function LinkOwner(ByRef lDocNum As Integer, ByRef bLinkOwner As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First doc num of linked doc
            m_sSQL = "SELECT count(doc_num) FROM DOC_document WHERE link = " & lDocNum

            'Hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="LINKOWNER", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check if anything found
            If Not Informations.IsArray(vResultArray) Then
                'no links
                bLinkOwner = False
            Else
                'other docs linked to this doc

                bLinkOwner = (CDbl(vResultArray(0, 0)) > 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="LinkOwner", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteDocInfo
    '
    ' Description: Given a doc num, this function deletes the doc info
    ' record.
    '
    ' ***************************************************************** '
    Public Function DeleteDocInfo(ByRef lDocNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "DELETE FROM DOC_doc_info WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELDOCINFO", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteDocKeywords
    '
    ' Description: Given a doc num, this function deletes its keywords
    '
    ' ***************************************************************** '
    Public Function DeleteDocKeywords(ByRef lDocNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "DELETE FROM DOC_doc_keyword WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELDOCKEYW", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocKeywords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteDocAnnotations
    '
    ' Description: Given a doc num, this function deletes its annotations
    '
    ' ***************************************************************** '
    Public Function DeleteDocAnnotations(ByRef lDocNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "DELETE FROM DOC_annotation WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELDOCANN", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocAnnotations", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DeletePages
    '
    ' Description: This physically deletes each page file for a doc.
    ' If a page is removed ok, then the page record is deleted. If
    ' the page is not removed Ok, then the record is orphaned in lieu
    ' of some housekeeping type routine
    '
    ' ***************************************************************** '
    Private Function DeletePages(ByRef lDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim sDataPath As String = ""
        Dim vPageArray() As Object = Nothing




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the physical files

        m_lReturn = CType(GetPageList(lDocNum, vPageArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'ensure we get some pages back - if we dont just carry on as we were
        'gonna delete them anyway.
        If Informations.IsArray(vPageArray) Then

            'delete each page

            For I As Integer = vPageArray.GetLowerBound(0) To vPageArray.GetUpperBound(0)


                m_lReturn = CType(KillFile(CStr(vPageArray(I))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'Maybe because off line or permissions on NT not OK
                    'So set doc_num to 0


                    m_sSQL = "UPDATE DOC_page SET doc_num = 0 WHERE page_name =  '" &
                             CStr(vPageArray(I)).Substring(Len(CStr(vPageArray(I))) - 19, Math.Min(CStr(vPageArray(I)).Length, 15)) & "'"


                    'hit DB

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="UPDATEPAGE", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else

                    'deleted OK so delete record in page table


                    m_sSQL = "DELETE FROM DOC_page WHERE page_name =  '" &
                             CStr(vPageArray(I)).Substring(Len(CStr(vPageArray(I))) - 19, Math.Min(CStr(vPageArray(I)).Length, 15)) & " '"

                    'hit DB

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELPAGE", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next I

        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetPageList
    '
    ' Description: For a given document, this function returns an array
    ' of physical file paths that constitute the document
    '
    ' It presumes any link checking has already been done, so pages
    ' should always be found.
    '
    ' ***************************************************************** '
    Public Function GetPageList(ByRef lDocNum As Integer, ByRef vPageArray() As Object) As Integer

        Dim result As Integer = 0
        Dim sDataPath As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT page_name, page_type, volume_id FROM DOC_page " &
                     "WHERE doc_num = " & CStr(lDocNum)

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETPAGELIST", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No pages ? Best log an error.
            If Not Informations.IsArray(vResultArray) Then
                ' Log Error Message
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No pages found for doc_num = " & lDocNum, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageList", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            'Get data path

            m_lReturn = CType(GetDataPath(CInt(vResultArray(2, 0)), sDataPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set up array

            ReDim vPageArray(vResultArray.GetUpperBound(1))

            'Construct path for each (assume all on same volume for now)

            For I As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)



                vPageArray(I) = sDataPath &
                                CStr(vResultArray(0, I)).Trim() &
                                "." &
                                CStr(vResultArray(1, I)).Trim()

            Next I

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPageType
    '
    ' Description: For a given document, this function returns the page
    ' type.
    '
    ' ***************************************************************** '
    Public Function GetPageType(ByRef lDocNum As Integer, ByRef sPageType As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT page_type FROM DOC_page WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETPAGETYPE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No pages ? Let calling procedure deal with that.
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Pages Found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            Else

                'return the page name

                sPageType = CStr(vResultArray(0, 0))

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPageName
    '
    ' Description: For a given document, this function returns the page
    ' name. If more than one, it only returns the first, as it's mainly
    ' for getting a page name to write to history database.
    '
    ' ***************************************************************** '
    Public Function GetPageName(ByRef lDocNum As Integer, ByRef sPageName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT page_name FROM DOC_page WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETPAGENAME", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No pages ? Let calling procedure deal with that.
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Pages Found for doc_num = " & lDocNum, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageName", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            Else

                'return the page name

                sPageName = CStr(vResultArray(0, 0))

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocDate
    '
    ' Description: For a given document, this function returns the document
    ' date from the docinfo record.
    '
    ' ***************************************************************** '
    Public Function GetDocDate(ByRef lDocNum As Integer, ByRef dDocDate As Date) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT doc_date FROM DOC_doc_info WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETDOCDATE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No records ?
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document date.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            Else

                'return the doc date

                dDocDate = CDate(vResultArray(0, 0))

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDocLink
    '
    ' Description: For a given document, this function returns its
    ' link value - which may well be 0.
    '
    ' ***************************************************************** '
    Public Function GetDocLink(ByRef lDocNum As Integer, ByRef lLinkNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT link FROM DOC_document WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETDOCLINK", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No records ?
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document link.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            Else

                'return the doc link

                lLinkNum = CInt(vResultArray(0, 0))

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocDetails
    '
    ' Description: For a given document, this function currenty returns
    ' the document name and type.
    '
    ' ***************************************************************** '
    Public Function GetDocDetails(ByRef lDocNum As Integer, ByRef sDocName As String, ByRef sDocType As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT doc_name, doc_type FROM DOC_document WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETDOCDETAILS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No records ? Why that's just bollocks.
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            Else

                'return the doc details

                sDocName = CStr(vResultArray(0, 0))

                sDocType = CStr(vResultArray(1, 0))

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteDocuments
    '
    ' Description: This function is the public method to be used for
    ' deleting an array of documents. It basically a wrapper to the
    ' function that actually deletes a document.
    '
    ' ***************************************************************** '
    Public Function DeleteDocuments(ByRef vDocArray As Object, ByRef bExternal As Boolean, ByRef sNoAccessName As String) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Not a valid array so go
            If Not Informations.IsArray(vDocArray) Then
                Return result
            End If

            'loop thru each doc, deleting it

            For I As Integer = vDocArray.GetLowerBound(0) To vDocArray.GetUpperBound(0)


                m_lReturn = CType(DeleteDoc(lDocNum:=CInt(vDocArray(I)), bExternal:=bExternal, sNoAccessName:=sNoAccessName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            Next I

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocuments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteDoc
    '
    ' Description: This function deletes a document from the document
    ' table.
    '
    ' It checks for and handles any links.
    '
    ' Also it checks the access level, returning the name of the
    ' document if access is not allowed.
    '
    ' If external, the history table is updated
    '
    ' Edit History :
    ' RAM20021212  : 1. Removed code related to History Update
    '                2. Ref. NRMA Project Changes. Sirius Process No. 189
    ' ***************************************************************** '
    Public Function DeleteDoc(ByRef lDocNum As Integer, ByRef bExternal As Boolean, ByRef sNoAccessName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim bLinkOwner, bLinked As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First get access level and ex code of document, etc
            m_sSQL = "SELECT access_level, ex_code, doc_name, DOC_doc_info.doc_date, link FROM " &
                     "DOC_document, DOC_doc_info WHERE DOC_document.doc_num = " & CStr(lDocNum) &
                     " AND DOC_doc_info.doc_num = " & CStr(lDocNum)

            'Hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GETDOCEXCODE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Check user has permission to delete this doc
            If Informations.IsArray(vResultArray) Then


                If CDbl(vResultArray(0, 0)) < m_iAccessLevel Then

                    'return name of indelible doc

                    sNoAccessName = CStr(vResultArray(2, 0))
                    Return gPMConstants.PMEReturnCode.PMFalse

                End If
            Else
                'this is wrong - cant find the document
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDoc", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Everything OK to go ahead and delete, so check the links
            m_lReturn = CType(LinkOwner(lDocNum, bLinkOwner), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'check if this is doc is linked to another
            '(as we wont want to delete the pages)

            bLinked = (CDbl(vResultArray(4, 0)) <> 0)

            If bLinkOwner Then
                'This doc has other linked to it. In this case just orphan
                'it.

                'set parent to 0
                m_sSQL = "UPDATE DOC_document SET folder_num = 0 WHERE doc_num = " & lDocNum

                'hit DB

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="UPDATEDOC", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Application.DoEvents()

                'Right, delete all the sundries now

                'delete docinfo record
                m_lReturn = CType(DeleteDocInfo(lDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'delete keywords for doc
                m_lReturn = CType(DeleteDocKeywords(lDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'delete annotation for doc
                m_lReturn = CType(DeleteDocAnnotations(lDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            Else

                'Right, delete all the sundries now

                'delete docinfo record
                m_lReturn = CType(DeleteDocInfo(lDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'delete keywords for doc
                m_lReturn = CType(DeleteDocKeywords(lDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'delete annotation for doc
                m_lReturn = CType(DeleteDocAnnotations(lDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bLinked Then

                    'delete pages (delete from page table and physical files)
                    m_lReturn = CType(DeletePages(lDocNum), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                'delete the document record
                m_sSQL = "DELETE FROM DOC_document WHERE doc_num = " & lDocNum

                'hit DB

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELETEDOC", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddDocToHistory
    '
    ' Description: This function writes an ADDDOCUMENT record to the
    ' history database for the given document details. It first has to
    ' get the parents external codes
    '
    ' ***************************************************************** '
    Public Function AddDocToHistory(ByRef lDocNum As Integer, ByRef lFoldNum As Integer, ByRef sDocName As String, ByRef dDocDate As Date, ByRef sPageName As String, ByRef sDocType As String, ByRef sPageType As String) As Integer

        Dim result As Integer = 0
        Dim sDocRef = " =", sTmpName = " ", sCabExCode = " ", sDrawExCode = " ", sFoldExCode As String = " "
        Dim lParentNum1, lParentNum2 As Integer
        Dim iFolderLevel As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First we must get external codes of parent folders

            'Get parent folder details
            m_lReturn = CType(GetNodeExCode(DOCNode_Folder, lFoldNum, sFoldExCode, iFolderLevel), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'If this is not an external version 2 folder then we wont be updating the history db,
            'so leave
            If (sFoldExCode.Trim() = "") Or (iFolderLevel <> DOCFolder) Then
                Return result
            End If

            'get parent drawer details
            m_lReturn = CType(GetNodeParent(DOCNode_Folder, lFoldNum, lParentNum1), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum1, sDrawExCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'get parent cabinet details
            m_lReturn = CType(GetNodeParent(DOCNode_Folder, lParentNum1, lParentNum2), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = CType(GetNodeExCode(DOCNode_Folder, lParentNum2, sCabExCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            'construct the document reference
            m_lReturn = CType(ConstructDocRef(lDocNum, sDocRef), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'remove the obliques and soliduses from the page name
            m_lReturn = CType(StripSlashes(sPageName, sTmpName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'hit the history table

            m_lReturn = m_oHistory.DirectAdd(vTask:=ToSafeInteger(DOCADDDOCUMENT), vCabinetCode:=ToSafeString(sCabExCode), vCabinetName:="", vDrawerCode:=ToSafeString(sDrawExCode), vDrawerName:="", vFolderCode:=ToSafeString(sFoldExCode), vFolderName:="", vDocRef:=ToSafeString(sDocRef), vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"), vEventType:=ToSafeString(sDocType), vDescription:=ToSafeString(sDocName), vVolume:=ToSafeString(DOCHD1_NAME), vPageFile:=ToSafeString(sTmpName), vdoctype:=ToSafeString(sPageType.ToUpper()))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocToHistory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteScannedDoc
    '
    ' Description: This function is provided with a scanned document
    ' number and deletes all data for this from the scan database
    '
    ' ***************************************************************** '
    Public Function DeleteScannedDoc(ByRef lScanDocNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'first the delete from document table
            m_sSQL = "DELETE FROM DOC_document WHERE doc_num = " & lScanDocNum

            m_lReturn = CType(m_oScanDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEDOC", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' delete from docinfo table
            m_sSQL = "DELETE FROM DOC_doc_info WHERE doc_num = " & lScanDocNum

            m_lReturn = CType(m_oScanDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEDOCINFO", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' delete from keyword table
            m_sSQL = "DELETE FROM DOC_doc_keyword WHERE doc_num = " & lScanDocNum

            m_lReturn = CType(m_oScanDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEKW", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' delete from annotation table
            m_sSQL = "DELETE FROM DOC_annotation WHERE doc_num = " & lScanDocNum

            m_lReturn = CType(m_oScanDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEANN", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' delete from page table
            m_sSQL = "DELETE FROM DOC_page WHERE doc_num = " & lScanDocNum

            m_lReturn = CType(m_oScanDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEPAGE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' delete from task table
            m_sSQL = "DELETE FROM DOC_task WHERE doc_num = " & lScanDocNum

            m_lReturn = CType(m_oScanDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETETASK", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteScannedDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteScannedPageFiles
    '
    ' Description: This deletes each physical page file for a scanned
    ' document
    '
    ' ***************************************************************** '
    Public Function DeleteScannedPageFiles(ByRef lScanDocNum As Integer, ByRef sScanDirectory As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sFilename As String = ""



        Try
            'This removes the last used diretory
            'Can't removed current as it is locked by current usage.  This will mean it will leave
            'the last used folder and the next doc folder will always be left on commiting
            'SOB220999
            Dim sDirectory As String = sScanDirectory & "Doc" & CStr(Math.Max(1, lScanDocNum - 1))
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory())
            'You can only remove an empty directory
            If Directory.GetFiles(sDirectory & "\*.*", FileAttribute.Normal) Is "" Then Directory.Delete(sDirectory)

            Try

                result = gPMConstants.PMEReturnCode.PMTrue
                'get all the pages first
                m_sSQL = "SELECT page_num, page_type FROM DOC_page WHERE doc_num = " & lScanDocNum

                'hit DB
                m_lReturn = CType(m_oScanDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETPAGEDETS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then

                    'This is not right. Log message and go
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="No pages exist for document " & lScanDocNum, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteScannedPageFiles", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

                'loop thru each page deleting it

                For I As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    ' Application.DoEvents()



                    sFilename = sScanDirectory & "Doc" &
                                lScanDocNum & "\" &
                                CStr(vResultArray(0, I)).Trim() & "." &
                                CStr(vResultArray(1, I))

                    m_lReturn = CType(KillFile(sFilename), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'not a problem, but continue deleting the rest
                    End If

                Next

                Return result

            Catch excep As System.Exception



                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteScannedPageFiles", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End Try

        Catch exc As System.Exception
            ' NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DeleteDocTask
    '
    ' Description: Given a doc num, this function deletes the task
    ' record.
    '
    ' ***************************************************************** '
    Public Function DeleteDocTask(ByRef lDocNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "DELETE FROM DOC_task WHERE doc_num = " & lDocNum

            'hit DB

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ToSafeString(m_sSQL), sSQLName:="DELTASK", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetSADatabase (public)
    '
    ' Description: Gets the current SA database as returns a link to it.
    '
    ' ***************************************************************** '
    Public Function GetSADatabase(ByRef vDatabase As Object) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oDatabase As dPMDAO.Database
#Else
        Dim oDatabase As Object = Nothing
#End If

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get SA database link
            m_lReturn = CType(dPMDAO.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, False, oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            vDatabase = oDatabase

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSADatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSADatabase", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = CType(CheckPMProductInstalled(m_sUsername, m_iSourceID, m_iLanguageID, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, bInstalled), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsSBOInstalled Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsSBOInstalled", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DJM 18/09/2003 : Created
    ' ***************************************************************** '
    ' Name: MergeFolders
    '
    ' Description: Moves all documents for a policy/claim into a single
    '              folder and then removes all other folders for the policy.
    '              Mainly useful when policy branch has been changed.
    '
    ' ***************************************************************** '
    Public Function MergeFolders(ByRef v_sInsuranceFileCnt As String, ByRef v_sPartyCnt As String, ByRef v_sCompanyID As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeString(v_sInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=ToSafeString(v_sPartyCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=ToSafeString(v_sCompanyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_DOC_merge_folders", sSQLName:="MergeFolders", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="MergeFolders", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetInsuranceFolderCnt
    '
    ' Description: Feching InsuranceFolderCnt using claim_id
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFolderCnt(ByRef vClaimCnt As Object, ByRef vInsuranceFolderCnt As Object) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "SELECT I.insurance_folder_cnt FROM Insurance_Folder I" &
                     " JOIN insurance_file in_f ON" &
                     " I.insurance_folder_cnt = in_f.insurance_folder_cnt" &
                     " JOIN claim C ON" &
                     " C.policy_id = in_f.insurance_file_cnt" &
                     " WHERE claim_id=" & CStr(ToSafeLong(vClaimCnt))



            'hit DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GetInsuranceFolderCnt", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ToSafeString(m_sSQL), sSQLName:="GetInsuranceFolderCnt", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=CType(vResultArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the ex code.
            If Informations.IsArray(vResultArray) Then


                vInsuranceFolderCnt = (vResultArray(0, 0))


            Else
                'no good at all
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolderCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolderCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

