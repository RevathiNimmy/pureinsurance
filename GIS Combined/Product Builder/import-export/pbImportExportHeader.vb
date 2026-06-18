Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportExportHeader

    Private Const ACClass As String = conEmptyString
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name:        ExtractHeader
    '
    ' Description: Processes the extraction of fixed tables to the binary
    '              file.  Recieves the current control file array row and
    '              the file number of hte file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              04/10/2002 SJP - Changed to be a function so can pass
    '                               back if successful or not
    '              07/10/2002 SJP - Included Data Model Id in Header Row
    '
    ' ***************************************************************** '
    Public Function ExtractHeader(ByRef r_oDatabase As dPMDAO.Database, _
                                  ByVal v_iFileNumber As Integer, _
                                  ByVal v_iMode As Integer, _
                                  ByVal v_lDataModelId As Integer, _
                                  ByVal v_sDataModelCode As String, _
                                  ByRef r_aIeControl() As Object, _
                                  ByRef r_aIeTableDefinitions() As Object, _
                                  ByVal iTableIndex As Integer, _
                                  ByVal v_sVersionNumber As String, _
                                  ByRef r_lTotalLinesWritten As Integer) As Integer


        'Define the counters to traverse the array sucture
        Dim nResult As Integer = 0
        Dim oMyarray(8, 0) As Object
        Dim sDBVersion As String = String.Empty

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractHeader")

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Header"

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName))

            '***************************************************************** '

            'Define and populate a local array to hold the screen based information prior to
            'writing it to the binary file

            'Set the mode

            oMyarray(0, 0) = v_iMode

            'set a dummy value for number of lines

            oMyarray(1, 0) = 0

            'set the data model id

            oMyarray(2, 0) = v_lDataModelId

            'set the data model code

            oMyarray(3, 0) = v_sDataModelCode

            'Set the type

            oMyarray(4, 0) = g_bIsUnderwriting

            'Set the version number

            oMyarray(5, 0) = v_sVersionNumber

            'Set the comment

            oMyarray(6, 0) = objFrmMainForm.txtExportHeaderComment.Text

            m_lReturn = CType(GetDatabaseVersion(r_oDatabase:=r_oDatabase, r_sDBVersion:=sDBVersion), gPMConstants.PMEReturnCode)

            ' Contains comma separated list of database versions for each system
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                oMyarray(7, 0) = sDBVersion
            Else

                oMyarray(7, 0) = conEmptyString
            End If
            'Set the buildExportMode
            oMyarray(8, 0) = buildExportMode()

            '***************************************************************** '

            'Write the header ionformation ot the binary file

            m_lReturn = CType(WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(r_aIeControl(iTableIndex)(0)), i_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=oMyarray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***************************************************************** '

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractHeader")

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractHeader")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractHeader", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ExtractGisListsFile
    '
    ' Description:
    '
    ' History: 04/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractGisListsFile(ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sPathName, sSubKey As String
        Dim oFSO As Object
        Dim oFolder As DirectoryInfo
        Dim oFiles As Object
        Dim oFile As Object

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractGisListsFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            ' oFSO = New Object()
            oFSO = New Scripting.FileSystemObject()
            'Set the panel to indicate the  type of extract
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "GIS Lists"

            sSubKey = GISSharedConstants.ACOIMGISSubKey & conBackSlash & v_sDataModelCode & conBackSlash & "ListManagement"

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sPathName, v_sSubKey:=sSubKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(sPathName, 1) <> "\" Then
                sPathName += "\"
            End If

            If sPathName <> conEmptyString And gPMFunctions.FolderExists(sPathName) Then

                oFolder = New DirectoryInfo(sPathName)
                'oFiles = oFolder.Files
                'oFolder = oFSO.GetFolder(sPathName)
                oFiles = oFolder.GetFiles()

                'For Each oFile2 As FileInfo In oFiles
                For Each oFile In oFiles
                    If Left(oFile.Name, v_sDataModelCode.Length).ToLower() = v_sDataModelCode.ToLower() Then
                        objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sPathName & oFile.Name
                        If ExtractFile(iTableIndex, r_aIeControl, r_aIeTableDefinitions, sPathName & oFile.Name, oFile.Name, v_iFileNumber, r_lTotalLinesWritten) = gPMConstants.PMEReturnCode.PMNotFound Then
                            writeToStatusBox("Could not access " & sPathName & oFile.Name)
                        End If
                    End If
                Next

            End If
            oFile = Nothing
            oFiles = Nothing
            oFolder = Nothing
            oFSO = Nothing

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractGisListsFile")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractGisListsFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractGisListsFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractGisListsFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: buildExportMode
    '
    ' Description:
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function buildExportMode() As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".buildExportMode")

        Try
            'for debug

            If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_DataModel).Checked Then result += pbIeMode_DataModel
            If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Screen).Checked Then result += pbIeMode_Screen
            If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Scheme).Checked Then result += pbIeMode_Scheme
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Registry).CheckState = CheckState.Checked Then result += pbIeMode_Registry
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Documents).CheckState = CheckState.Checked Then result += pbIeMode_Documents
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Rulefiles).CheckState = CheckState.Checked Then result += pbIeMode_RuleFiles
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_RiskGroupsCodes).CheckState = CheckState.Checked Then result += pbIeMode_RiskGroupsCodes
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_3DRatings).CheckState = CheckState.Checked Then result += pbIeMode_3DLookups
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).CheckState = CheckState.Checked Then result += pbIeMode_PBDocsOnly
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_UDLs).CheckState = CheckState.Checked Then result += pbIeMode_UDLs

            If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Migration).Checked Then
                result += pbIeMode_Migration
                If (objFrmMainForm.txtdatamodelId.Text) > 0 Then
                    result += pbIeMode_DataModel
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".buildExportMode")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".buildExportMode")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="buildExportMode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="buildExportMode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDatabaseVersion
    '
    ' Description:
    '
    ' History: 01/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Private Function GetDatabaseVersion(ByRef r_oDatabase As dPMDAO.Database, ByRef r_sDBVersion As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vRetrievedData(,) As Object

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".GetDatabaseVersion")



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT name, version FROM PMLogicalDatabase"
        r_sDBVersion = conEmptyString

        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDBVersions", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRetrievedData)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Information.IsArray(vRetrievedData) Then

            For iLoop As Integer = 0 To vRetrievedData.GetUpperBound(1)

                r_sDBVersion = r_sDBVersion & CStr(vRetrievedData(conName, iLoop)) & conComma & CStr(vRetrievedData(conVersion, iLoop)) & conComma
            Next iLoop
            'Remove last comma
            r_sDBVersion = r_sDBVersion.Substring(0, r_sDBVersion.Length - 1)
        Else
            ' No database versions found in table, what now?
            ' Nothing to export, or check against when it comes to the import.
        End If

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ".GetDatabaseVersion")

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name:        ExtractTableDefinitions
    '
    ' Description: Processes the extraction of fixed tables to the binary
    '              file.  Recieves the current control file array row and
    '              the file number of hte file being processed
    '
    ' History:     18/10/2002 CG  - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractTableDefinitions(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByVal v_sVersionNumber As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim myarray(2, 0) As Object

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractTableDefinitions")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Table Definitions"

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName))

            '***************************************************************** '

            For lTableIndex As Integer = 0 To r_aIeTableDefinitions.GetUpperBound(0)

                If (g_aIeControl(lTableIndex)(pbIeControl_objectType)) = pbIeOt_dbTable_fixed Or (g_aIeControl(lTableIndex)(pbIeControl_objectType)) = pbIeOt_dbTable_child Or (g_aIeControl(lTableIndex)(pbIeControl_objectType)) = pbIeOt_RiskGroupsCodes Then

                    If CStr(r_aIeTableDefinitions(lTableIndex)(pbIeTableDefinitions_exportedColumns)) <> "" Then

                        myarray(0, 0) = lTableIndex

                        myarray(1, 0) = r_aIeTableDefinitions(lTableIndex)(pbIeTableDefinitions_exportedColumns)
                        'Write the header ionformation ot the binary file

                        m_lReturn = CType(WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(r_aIeControl(pbIeDbt_FixedTableColumns)(0)), i_aDataDefinition:=r_aIeTableDefinitions(pbIeDbt_FixedTableColumns)(pbIeTableDefinitions_columnArray), i_aData:=myarray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            Next

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractTableDefinitions")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractTableDefinitions")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractTableDefinitions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractTableDefinitions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
