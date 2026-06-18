Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles


Module pbImportExportHeader

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

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
    Public Function ExtractHeader(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Short, ByRef v_alDataModelID() As Integer, ByRef v_asDataModelCode() As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByVal v_sVersionNumber As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Richard Clarke November 2008 - PIE enhancements
        'function declarations modified
        'was byval v_lDataModelID
        'was byval v_sDataModelCode
        'Richard Clarke November 2008 - PIE enhancements

        Dim v_lDataModelID As String
        Dim v_sDataModelCode As String
        Dim iCounter As Short

        'Define the counters to traverse the array sucture
        Dim myarray(7, 0) As Object
        Dim sDBVersion As String

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractHeader")

            ExtractHeader = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Header"
            objFrmMainForm.StatusBar_TextWrite("Header", 1)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = r_aIeControl(iTableIndex)(pbIeControl_objectName)
            objFrmMainForm.StatusBar_TextWrite(r_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
            '***************************************************************** '

            'Define and populate a local array to hold the screen based information prior to
            'writing it to the binary file

            'Set the mode
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(0, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(0, 0) = 0

            'set a dummy value for number of lines
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(1, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(1, 0) = v_iMode

            'Richard Clarke November 2008 - PIE enhancements
            'loop the arrays of datamodelids and datamodelcodes so we have a comma separated list
            'in the export file header row
            For iCounter = 0 To UBound(v_alDataModelID) - 1
                v_lDataModelID = v_lDataModelID & CStr(v_alDataModelID(iCounter)) & ","
                v_sDataModelCode = v_sDataModelCode & v_asDataModelCode(iCounter) & ","
            Next iCounter
            'remove the trailing commas from the strings
            v_lDataModelID = Left(v_lDataModelID, Len(v_lDataModelID) - 1)
            v_sDataModelCode = Left(v_sDataModelCode, Len(v_sDataModelCode) - 1)
            'Richard Clarke November 2008 - PIE enhancements

            'set the data model id
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(2, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(2, 0) = v_lDataModelID

            'set the data model code
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(3, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(3, 0) = v_sDataModelCode

            'Set the type
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(4, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(4, 0) = g_bIsUnderwriting

            'Set the version number
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(5, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(5, 0) = v_sVersionNumber

            'Set the comment
            'UPGRADE_WARNING: Couldn't resolve default property of object myarray(6, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myarray(6, 0) = objFrmMainForm.txtExportHeaderComment.Text

            m_lReturn = GetDatabaseVersion(r_oDatabase:=r_oDatabase, r_sDBVersion:=sDBVersion)

            ' Contains comma separated list of database versions for each system
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object myarray(7, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                myarray(7, 0) = sDBVersion
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object myarray(7, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                myarray(7, 0) = conEmptyString
            End If

            '***************************************************************** '

            'Write the header ionformation ot the binary file
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=r_aIeControl(iTableIndex)(0), i_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=myarray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ExtractHeader = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            '***************************************************************** '

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractHeader")

            Exit Function

        Catch ex As Exception

            ExtractHeader = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractHeader")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractHeader", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function ExtractGisListsFile(ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Object

        Dim sPathName As String
        Dim sSubKey As String
        Dim oFSO As Scripting.FileSystemObject
        Dim oFolder As Scripting.Folder
        Dim oFiles As Scripting.Files
        Dim oFile As Scripting.File

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractGisListsFile")

            'UPGRADE_WARNING: Couldn't resolve default property of object ExtractGisListsFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ExtractGisListsFile = gPMConstants.PMEReturnCode.PMTrue

            oFSO = New Scripting.FileSystemObject

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "GIS Lists"
            objFrmMainForm.StatusBar_TextWrite("GIS Lists", 1)

            sSubKey = GISSharedConstants.ACOIMGISSubKey & conBackSlash & v_sDataModelCode & conBackSlash & "ListManagement"

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sPathName, v_sSubKey:=sSubKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object ExtractGisListsFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ExtractGisListsFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'If sPathName <> conEmptyString And Len(Dir(sPathName)) > 0 Then
            'If sPathName <> conEmptyString And FileExists(sPathName) = True Then
            If sPathName <> conEmptyString AndAlso FolderExists(sPathName) = True Then

                oFolder = oFSO.GetFolder(sPathName)
                oFiles = oFolder.Files

                For Each oFile In oFiles
                    If LCase(Left(oFile.Name, Len(v_sDataModelCode))) = LCase(v_sDataModelCode) Then
                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sPathName & oFile.Name
                        objFrmMainForm.StatusBar_TextWrite(sPathName & oFile.Name, 2)
                        If ExtractFile(iTableIndex, r_aIeControl, r_aIeTableDefinitions, sPathName & AddRequiredBackslash(sPathName) & oFile.Name, oFile.Name, v_iFileNumber, r_lTotalLinesWritten) = gPMConstants.PMEReturnCode.PMNotFound Then
                            writeToStatusBox("Could not access " & sPathName & oFile.Name)
                        End If
                    End If
                Next oFile

            End If
            'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFile = Nothing
            'UPGRADE_NOTE: Object oFiles may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFiles = Nothing
            'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFolder = Nothing
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractGisListsFile")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractGisListsFile")

            'UPGRADE_WARNING: Couldn't resolve default property of object ExtractGisListsFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ExtractGisListsFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractGisListsFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractGisListsFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".buildExportMode")

        Try

            buildExportMode = 0 'for debug

            'If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_DataModel) = True Then buildExportMode = buildExportMode + pbIeMode_DataModel
            'If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Screen) = True Then buildExportMode = buildExportMode + pbIeMode_Screen
            'If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Scheme) = True Then buildExportMode = buildExportMode + pbIeMode_Scheme
            buildExportMode = buildExportMode + pbIeMode_DataModel
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Registry).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_Registry
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Documents).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_Documents
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Rulefiles).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_RuleFiles
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_AuthRules).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_UARs
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUICCS).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_UserStoredProcedure
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUReports).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_UserReports
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_ReportSPU).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_UserReportsSPU
            If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_UDLs).CheckState = 1 Then buildExportMode = buildExportMode + pbIeMode_UDLs

            'If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Migration).value = True Then
            '    buildExportMode = buildExportMode + pbIeMode_Migration
            '    If objFrmMainForm.txtdatamodelId.Text > 0 Then
            '        buildExportMode = buildExportMode + pbIeMode_DataModel
            '    End If
            'End If
            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".buildExportMode")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".buildExportMode")

            buildExportMode = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="buildExportMode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="buildExportMode", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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

        Dim sSQL As String
        Dim vRetrievedData(,) As Object
        Dim iLoop As Short

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & ".GetDatabaseVersion")


        GetDatabaseVersion = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT name, version FROM PMLogicalDatabase"
        r_sDBVersion = conEmptyString

        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDBVersions", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRetrievedData)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GetDatabaseVersion = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If IsArray(vRetrievedData) Then
            For iLoop = 0 To UBound(vRetrievedData, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData(conVersion_Renamed, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_sDBVersion = r_sDBVersion & vRetrievedData(conName, iLoop) & conComma & vRetrievedData(conVersion_Renamed, iLoop) & conComma
            Next iLoop
            'Remove last comma
            r_sDBVersion = Left(r_sDBVersion, Len(r_sDBVersion) - 1)
        Else
            ' No database versions found in table, what now?
            ' Nothing to export, or check against when it comes to the import.
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ".GetDatabaseVersion")

        Exit Function

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
    Public Function ExtractTableDefinitions(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByVal v_sVersionNumber As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim lTableIndex As Integer
        Dim myarray(2, 0) As Object

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractTableDefinitions")

            ExtractTableDefinitions = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Table Definitions"
            objFrmMainForm.StatusBar_TextWrite("Table Definitions", 1)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = r_aIeControl(iTableIndex)(pbIeControl_objectName)
            objFrmMainForm.StatusBar_TextWrite(r_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
            '***************************************************************** '

            For lTableIndex = 0 To UBound(r_aIeTableDefinitions)
                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(lTableIndex)(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If g_aIeControl(lTableIndex)(pbIeControl_objectType) = pbIeOt_dbTable_fixed Or g_aIeControl(lTableIndex)(pbIeControl_objectType) = pbIeOt_dbTable_child Or g_aIeControl(lTableIndex)(pbIeControl_objectType) = pbIeOt_RiskGroupsCodes Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(lTableIndex)(pbIeTableDefinitions_exportedColumns). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_aIeTableDefinitions(lTableIndex)(pbIeTableDefinitions_exportedColumns) <> "" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object myarray(0, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        myarray(0, 0) = lTableIndex
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object myarray(1, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        myarray(1, 0) = r_aIeTableDefinitions(lTableIndex)(pbIeTableDefinitions_exportedColumns)
                        'Write the header ionformation ot the binary file
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeDbt_FixedTableColumns)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=r_aIeControl(pbIeDbt_FixedTableColumns)(0), i_aDataDefinition:=r_aIeTableDefinitions(pbIeDbt_FixedTableColumns)(pbIeTableDefinitions_columnArray), i_aData:=myarray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ExtractTableDefinitions = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    End If
                End If
            Next

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractTableDefinitions")

            Exit Function

        Catch ex As Exception

            ExtractTableDefinitions = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractTableDefinitions")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractTableDefinitions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractTableDefinitions", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractUserSpusFile
    '
    ' Description: Processes the extraction of user spus to the binary
    '              file.
    '
    ' History:     November 2008 - Richard Clarke PIE enhancements - Created.
    '
    ' ***************************************************************** '

    Public Function ExtractUserSPUsFile(ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Object


        Dim sPathName As String
        Dim sSubKey As String
        Dim oFSO As Scripting.FileSystemObject
        Dim oFolder As Scripting.Folder
        Dim oFiles As Scripting.Files
        Dim oFile As Scripting.File

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractUserSpusFile")

            'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserSPUsFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ExtractUserSPUsFile = gPMConstants.PMEReturnCode.PMTrue

            oFSO = New Scripting.FileSystemObject

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User SPU"
            objFrmMainForm.StatusBar_TextWrite("User SPU", 1)

            'sSubKey = ACOIMGISSubKey & conBackSlash & v_sDataModelCode & conBackSlash & "ListManagement"

            'm_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
            ''v_lPMEProductFamily:=pmePFSiriusSolutions, _
            ''v_lPMERegSettingLevel:=pmeRSLServer, _
            ''v_sSettingName:="ServerListFilePath", _
            ''r_sSettingValue:=sPathName, _
            ''v_sSubKey:=sSubKey)

            'ToDo: Modify this to use the reg setting for backup path
            'see backupusersettings for reg key
            If objFrmMainForm.txtBackupDir.Text.Trim <> "" Then
                sPathName = objFrmMainForm.txtBackupDir.Text & "spuICCS\"
            Else
                sPathName = objFrmMainForm.txtFilePath(1).Text & "\spuICCS\"
            End If

            If sPathName <> conEmptyString AndAlso FolderExists(sPathName) = True Then

                oFolder = oFSO.GetFolder(sPathName)
                oFiles = oFolder.Files

                For Each oFile In oFiles
                    'If LCase(Left(oFile.Name, Len(v_sDataModelCode))) = LCase(v_sDataModelCode) Then
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sPathName & oFile.Name
                    objFrmMainForm.StatusBar_TextWrite(sPathName & oFile.Name, 2)
                    If ExtractFile(iTableIndex, r_aIeControl, r_aIeTableDefinitions, sPathName & AddRequiredBackslash(sPathName) & oFile.Name, oFile.Name, v_iFileNumber, r_lTotalLinesWritten) = gPMConstants.PMEReturnCode.PMNotFound Then
                        writeToStatusBox("Could not access " & sPathName & oFile.Name)
                    End If
                    'End If
                Next oFile

            End If
            'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFile = Nothing
            'UPGRADE_NOTE: Object oFiles may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFiles = Nothing
            'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFolder = Nothing
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractUserSPUsFile")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractUserSPUsFile")

            'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserSPUsFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ExtractUserSPUsFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractUserSPUsFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractUserSPUsFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserSPUsFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ExtractUserSPUsFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractUserSpusFile
    '
    ' Description: Processes the extraction of user spus to the binary
    '              file.
    '
    ' History:     November 2008 - Richard Clarke PIE enhancements - Created.
    '
    ' ***************************************************************** '

    Public Function ExtractUserCrystalReportFile(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Object


        Dim sPathName As String
        Dim sSubKey As String
        '    Dim oFSO As scripting.FileSystemObject
        '    Dim oFolder As scripting.Folder
        '    Dim oSubFolders As scripting.Folder
        '    Dim oSubFolder As scripting.Folder
        '    Dim oFiles As scripting.Files
        '    Dim oFile As scripting.File
        Dim sSQL As String
        Dim vResults(,) As Object
        Dim iCount As Short
        'Dim sFullPath As String
        Dim sFullPathCol As New System.Collections.Generic.List(Of String)

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractUserCrystalReportFile")

            'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserCrystalReportFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ExtractUserCrystalReportFile = gPMConstants.PMEReturnCode.PMTrue

            'Set oFSO = New scripting.FileSystemObject

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User Crystal Report"
            objFrmMainForm.StatusBar_TextWrite("User Crystal Report", 1)
            'see backupusersettings for reg key
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Reports", r_sSettingValue:=sPathName)

            'we only need the reports that are present in the reports table
            'Check this with Danny / Sarah - which crystal reports?
            'is_deleted = 0 (report & report group table).
            sSQL = "select report_name from report "


            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ExtractUserCrystalReportFile", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            'need an array of results from the query we can then loop around
            If IsArray(vResults) Then
                'loop our filenames traversing the folder struct to find the file
                For iCount = 0 To UBound(vResults, 2)
                    'ToDo: get the path from the registry
                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sFullPathCol = FindFile(sPathName, CStr(vResults(0, iCount)))
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    For Each sFullPath As String In sFullPathCol
                        objFrmMainForm.StatusBar_TextWrite(sFullPath, 2)
                        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sFullPath
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If ExtractFile(iTableIndex, r_aIeControl, r_aIeTableDefinitions, sFullPath, CStr(vResults(0, iCount)), v_iFileNumber, r_lTotalLinesWritten) = gPMConstants.PMEReturnCode.PMNotFound Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            writeToStatusBox("Could not access " & CStr(vResults(0, iCount)))
                        End If
                    Next
                Next iCount
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractUserSPUsFile")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractUserSPUsFile")

            'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserCrystalReportFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ExtractUserCrystalReportFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractUserCrystalReportFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractUserCrystalReportFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserCrystalReportFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ExtractUserCrystalReportFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End Try
    End Function
End Module