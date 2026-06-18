Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles


Module pbIDoImport

    Private m_lReturn As Integer

    Public g_xmlDocument As MSXML2.DOMDocument
    Private oPI As MSXML2.IXMLDOMProcessingInstruction
    Private rootNode As MSXML2.IXMLDOMElement

    Private Const ACClass As String = conEmptyString
    ' ***************************************************************** '
    '
    ' Name: doImport
    '
    ' Description:
    '
    ' History: 03/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function doImport(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByRef v_alDataModelID() As Integer, ByRef v_asDataModelCode() As String, ByRef g_aIeControl() As Object, ByRef g_aIeTableDefinitions() As Object, ByVal v_bCheckHeaderOnly As Boolean, ByVal v_sVersionNumber As String, ByVal v_bImportRegistry As Boolean) As Integer

        'Richard Clarke November 2008 - PIE enhancements
        'altered function definition to use the arrays now
        'ByVal v_lDataModelID As Long,
        'ByVal v_sDataModelCode As String,
        'Richard Clarke November 2008 - PIE enhancements

        'Local definitions
        Dim lIntegerData As Integer
        Dim lRowCount As Integer
        Dim lTotalLines As Integer
        Dim iImportMode As Integer
        Dim sSQL As String

        Dim bHeaderImported As Boolean
        Dim iCounter As Integer = 0

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".doImport")

            'Assume the function will be succesful
            doImport = gPMConstants.PMEReturnCode.PMTrue

            'for now we only support cloning of db keys
            g_CloneDbKeys = True

            lRowCount = 0
            If v_bCheckHeaderOnly = False Then
                objFrmMainForm.StatusBar_TextWrite("Import UMLs. Please Wait..", 0)
                writeToStatusBox(CStr(TimeOfDay) & " - Processing UMLs Scripts..")
                objFrmMainForm.ProgressBar1(1).Visible = False
                objFrmMainForm.ProgressBar1(0).Value = 0
                If ImportUMLScript(r_oDatabase) <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException
                End If
            End If

            If objFrmMainForm.chkImportUMLScriptsOnly.Checked Then
                Return PMEReturnCode.PMTrue
            End If

            'indicate the status on the main page
            If v_bCheckHeaderOnly = True Then
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'

                'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Import"
                objFrmMainForm.StatusBar_TextWrite("Import", 0)
            Else
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Import (0 of ?)"
                objFrmMainForm.StatusBar_TextWrite("Import (0 of ?)", 0)
            End If
            objFrmMainForm.ProgressBar1(1).Visible = False
            'This is being done below Ln:163
            If v_bCheckHeaderOnly = False Then
                m_lReturn = DropConstraints()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    doImport = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            '--------------------------------
            'Richard Clarke November 2008 - PIE Enhancements
            'set up the XML document for use later in SetNewIDs
            If v_bCheckHeaderOnly = False Then

                g_xmlDocument = New MSXML2.DOMDocument
                oPI = g_xmlDocument.createProcessingInstruction("xml", "version=""1.0""")
                rootNode = g_xmlDocument.createElement("NewPrimaryKeyValueInformation")

                'UPGRADE_WARNING: Couldn't resolve default property of object oPI. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_xmlDocument.insertBefore(oPI, g_xmlDocument.firstChild)
                'UPGRADE_WARNING: Couldn't resolve default property of object rootNode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_xmlDocument.appendChild(rootNode)
            End If
            'Richard Clarke November 2008 - PIE Enhancements - END
            '--------------------------------

            'we need to read the header file earlier than this function
            'do import header here, and then reset the file!
            GetLongData(iFileNumber:=v_iFileNumber, lDataElement:=lIntegerData, i_aDataDefinition:=g_aIeTableDefinitions)

            m_lReturn = ImportHeader(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData, r_lDataModelId:=v_alDataModelID, r_sDataModelCode:=v_asDataModelCode, r_iMode:=iImportMode, v_sVersionNumber:=v_sVersionNumber, r_lTotalLines:=lTotalLines)

            bHeaderImported = True

            'Richard Clarke November 2008 - PIE enhancements
            'loop round the data models
            'For iCounter = 0 To UBound(v_alDataModelID)
            'second iteration, the file is closed, re open it
            'writeToStatusBox(CStr(TimeOfDay) & " - Processing " & CStr(iCounter + 1) & "/" & CStr(UBound(v_alDataModelID) + 1))
            If v_bCheckHeaderOnly = False Then
                writeToStatusBox(CStr(TimeOfDay) & " - Processing all Datamodels") ' & CStr(iCounter + 1) & "/" & CStr(UBound(v_alDataModelID) + 1))
            End If
            If iCounter > 0 Then
                OpenBinaryFile(i_AccessType:=ReadAccess, i_sFilePath:=objFrmMainForm.txtFilePath(0).Text, i_sFileName:=objFrmMainForm.txtFileName(0).Text, i_sFileExtension:=objFrmMainForm.txtFileExtension(0).Text, o_iFileNumber:=v_iFileNumber)

                GetLongData(iFileNumber:=v_iFileNumber, lDataElement:=lIntegerData, i_aDataDefinition:=g_aIeTableDefinitions)

                m_lReturn = ImportHeader(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData, r_lDataModelId:=v_alDataModelID, r_sDataModelCode:=v_asDataModelCode, r_iMode:=iImportMode, v_sVersionNumber:=v_sVersionNumber, r_lTotalLines:=lTotalLines)

            End If

            ' BSJ Sep 2009 - For multiple source to single we can't do updates because of new pk id's on target
            ' Simply NUKE the existing data model first!
            ' JB Jun 10 Just taking risk to nuke in any migration
            ' If (g_bNukeDataModel = True) Then
            If v_bCheckHeaderOnly = False Then

                For iInnerCounter As Integer = 0 To UBound(v_alDataModelID)

                    r_oDatabase.SQLBeginTrans()

                    m_lReturn = BuildNukeSQL(sSQL, v_asDataModelCode(iInnerCounter))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        doImport = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    g_sLastSQLCommandGenerated = sSQL

                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete Data Model", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_oDatabase.SQLRollbackTrans()
                        doImport = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function

                    Else
                        r_oDatabase.SQLCommitTrans()

                        sSQL = conEmptyString

                        ' Enable all constraints
                        sSQL = sSQL & "exec sp_msforeachtable " & """" & "ALTER TABLE ? CHECK CONSTRAINT all" & """"
                        sSQL = sSQL & "exec sp_msforeachtable " & """" & "ALTER TABLE ? ENABLE TRIGGER  all" & """"

                        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Enable Constraints", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            doImport = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    End If
                Next
                iCounter = 0
                ' Drop constraints on certain tables
                m_lReturn = DropConstraints()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    doImport = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If
            ' End If
            Erase g_UserSPU

            Dim sSQLDelete As String = String.Empty
            Dim lRecordAffected As Integer

            If v_bCheckHeaderOnly = True And bHeaderImported = True Then
            Else
                'PM040123 Added delete statement because, If the [report_group_id],[report_id] exits in the source table and the
                'same guid exit for different [report_group_id],[report_id] in target table in that case update statement 
                'generate runtime error.
                sSQLDelete = "DELETE FROM report_group_contents "
                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQLDelete, sSQLName:="delete existing Report_Group_Contents setting record", bStoredProcedure:=False, lRecordsAffected:=lRecordAffected)

                sSQLDelete = "DELETE FROM report_group_user_groups "
                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQLDelete, sSQLName:="delete existing report_group_user_groups setting record", bStoredProcedure:=False, lRecordsAffected:=lRecordAffected)
                'Deleted setting records---
            End If

            '**NB - Assumption is that the first element of each row is an integer
            'Retrieve the first element of a row.  If a read fails then assume we
            'are at the end of the file
            Do Until GetLongData(iFileNumber:=v_iFileNumber, lDataElement:=lIntegerData, i_aDataDefinition:=g_aIeTableDefinitions) = gPMConstants.PMEReturnCode.PMFalse

                '********************************************************
                'Clear the global UDL data definition array ready to if the
                'current record type is not a UDL.
                If lIntegerData <> pbIeDbt_UserDefinedList Then
                    Erase g_sUDLCompleteTableDefinition
                End If
                '********************************************************

                'lets do a few sanity checks to ensure the file is valid
                If UBound(g_aIeControl) < lIntegerData Then
                    writeToStatusBox(("Possible corrupt or invalid export file"))
                    doImport = gPMConstants.PMEReturnCode.PMError
                    Exit Function
                End If

                'Determine the type of object that we're dealing with, to enable the rest of
                'the row to be returned in the correct format.  Sequence of imports matches
                'the sequence of exports defined in InitialiseArray

                ' JB Jun 10 It is illogical to import PMUser_Authority_Level as user might not be defined on
                ' target so would stop this table from being imported
                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(g_aIeControl(lIntegerData)(pbIeControl_objectName)) = "PMUser_Authority_Level" Then
                    Exit Function
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(g_aIeControl(lIntegerData)(pbIeControl_objectName)) = "numbering_scheme" Then
                    Debug.Print("foo")
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(g_aIeControl(lIntegerData)(pbIeControl_objectName)) = "report_group_user_groups" Then
                    Debug.Print("foo")
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Debug.Print(LCase(g_aIeControl(lIntegerData)(pbIeControl_objectName)))
                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(g_aIeControl(lIntegerData)(pbIeControl_objectName))) = "peril_type_usage" Then
                    Debug.Print("foo")
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Trim(LCase(g_aIeControl(lIntegerData)(pbIeControl_objectName))) = "gis_data_model" Then
                    Debug.Print("foo")
                End If

                ' BSJ Sep 2009 - Do not write information messages to the error log - It was 300mb in RGICL's case!
                'gPMFunctions.LogMessageToFile sUsername:="", _
                ''                iType:=PMLogOnError, _
                ''                sMsg:="Processing table " & g_aIeControl(lIntegerData)(pbIeControl_objectName), _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass, _
                ''                vMethod:="RSC", _
                ''                vErrNo:=0, _
                ''                vErrDesc:="RSC"
                If lIntegerData = 18 Then
                    Debug.Print("Print")
                End If

                Debug.Print(g_aIeControl(lIntegerData)(1))
                Select Case g_aIeControl(lIntegerData)(pbIeControl_objectType)

                    Case pbIeOt_Header
                        If bHeaderImported = False Then
                            m_lReturn = ImportHeader(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData, r_lDataModelId:=v_alDataModelID, r_sDataModelCode:=v_asDataModelCode, r_iMode:=iImportMode, v_sVersionNumber:=v_sVersionNumber, r_lTotalLines:=lTotalLines)
                            'we've got line information so we can update the progress bar
                            objFrmMainForm.StatusBar_TextWrite("Import (" & lRowCount & " of " & lTotalLines & ")", 0)
                            objFrmMainForm.ProgressBar1(0).Value = 100 / (lTotalLines / 1)
                            bHeaderImported = True
                        End If

                    Case pbIeOt_FixedTableColumns
                        m_lReturn = ImportFixedTableDefinition(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iImportMode, v_lDataModelID:=v_alDataModelID(iCounter), v_sDataModelCode:=v_asDataModelCode(iCounter), r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=lIntegerData)
                        'ToDo: check that I can change this back to just use the specific data model
                    Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined, pbIeOt_dbTable_child, pbIeOt_RiskGroupsCodes

                        m_lReturn = ImportFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iImportMode, v_lDataModelID:=v_alDataModelID(iCounter), v_sDataModelCode:=v_asDataModelCode(iCounter), r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=lIntegerData)

                    Case pbIeOt_RegSetting
                        m_lReturn = ImportRegistrySettings(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData, v_sDataModelCode:=v_asDataModelCode(iCounter), v_bImportRegistry:=v_bImportRegistry)

                    Case pbIeOt_RuleFile

                        m_lReturn = ImportRuleFile(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData)

                    Case pbIeOt_DocumentTemplate

                        m_lReturn = ImportDocumentTemplate(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData)

                    Case pbIeOt_UserDefinedListHeader, pbIeOt_UserDefinedList

                        m_lReturn = ImportUserDefinedList(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iImportMode, v_lDataModelID:=v_alDataModelID(iCounter), v_sDataModelCode:=v_asDataModelCode(iCounter), r_aIeControl:=g_aIeControl(lIntegerData), r_aIeTableDefinitions:=g_aIeTableDefinitions(lIntegerData), iTableIndex:=lIntegerData)

                    Case pbIeOt_GisList

                        m_lReturn = ImportGisList(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData, v_sDataModelCode:=v_asDataModelCode(iCounter))

                        'Richard Clarke November 2008 - PIE Enhancements
                    Case pbIeOt_UserSPU
                        m_lReturn = ImportUserSPU(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData)

                    Case pbIeOt_UserCrystalReport
                        m_lReturn = ImportCrystalReport(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData)

                        'Richard Clarke November 2008 - PIE Enhancements END
                    Case pbIeOt_UserCrystalReportSPU
                        m_lReturn = ImportCrystalReportSPU(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData)

                    Case Else
                        'MsgBox "Attempting to load an unknown type of " & g_aIeControl(lIntegerData)(pbIeControl_objectType)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    doImport = gPMConstants.PMEReturnCode.PMError
                    Exit Function
                End If

                '---------------------------
                'Richard Clarke November 2008 - PIE enhancements
                'If v_bCheckHeaderOnly = True And _
                ''   g_aIeControl(lIntegerData)(pbIeControl_objectType) = pbIeOt_Header Then
                'added check on bheaderimported as this test fails now due to the fact we've already
                'imported the header on the first run
                'so can't check the object type here now as it won't be pbIeOt_Header
                If v_bCheckHeaderOnly = True And bHeaderImported = True Then
                    'exit the function
                    doImport = gPMConstants.PMEReturnCode.PMTrue
                    Exit Function
                End If

                'Increment the row count for debugging purposes
                lRowCount = lRowCount + 1

                If lRowCount Mod 100 = 0 Then
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Import (" & lRowCount & " of " & lTotalLines & ")"
                    objFrmMainForm.StatusBar_TextWrite("Import (" & lRowCount & " of " & lTotalLines & ")", 0)
                    Try
                        objFrmMainForm.ProgressBar1(0).Value = 100 / ((lTotalLines + 1) / (lRowCount + 1))
                    Catch ex As Exception
                        Debug.Print("foo")
                    End Try
                    System.Windows.Forms.Application.DoEvents()
                End If
                'if an error occurred, don't continue
                If g_bErrorInImport = True Then
                    Exit Do
                End If

            Loop
            'CloseBinaryFile v_iFileNumber:=iFileNumber
            'need to close the file and then reopen it as we're at the end of the file
            'need to move back to the start of the file
            CloseBinaryFile(v_iFileNumber)
            lRowCount = 0
            'if an error occurred, don't continue
            If g_bErrorInImport = True Then
                'Exit For
            End If
            'Next iCounter 'Richard Clarke November 2008 - PIE enhancements

            '----------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'no point continuing if an error occurred, just let the user review and then rollback
            If g_bErrorInImport = True Then
                objFrmMainForm.SSTab3.SelectedIndex = 1
                doImport = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'Richard Clarke November 2008 - PIE enhancements
            '----------------------------

            'update unique_number table
            Dim lRecordsAffected As Integer
            Dim vResults As Object
            Dim vResultsChk As Object
            Dim lNextNumber As Integer
            Dim bFlagUnique As Boolean
            For lRowCount = 0 To UBound(g_aIeControl)
                If g_aIeControl(lRowCount)(pbIeControl_Flags) And pbIeControl_Flags__uniqueNumber Then
                    'If code not contain any string value then max + 1 query will be executed.
                    bFlagUnique = True
                    'PM038738 - To Check Column containg string value then (Max+1) Code will not be executed to update unique number
                    sSQL = "select top 1 " & g_aIeTableDefinitions(lRowCount)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName) & " from " & g_aIeControl(lRowCount)(pbIeControl_objectName) &
                           " where " & g_aIeTableDefinitions(lRowCount)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName) & " LIKE '%[a-z]%'"
                    m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Checking Numeric Value", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultsChk, bKeepNulls:=True)
                    If IsArray(vResultsChk) Then
                        If Not IsDBNull(vResultsChk(0, 0)) Then
                            bFlagUnique = False
                        End If
                    End If
                    'found a table that needs to update unique_number

                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(lRowCount)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'

                    If bFlagUnique Then
                        sSQL = "select max(" & g_aIeTableDefinitions(lRowCount)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName) & ")+1 from " & g_aIeControl(lRowCount)(pbIeControl_objectName)
                        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="updating unique_number 1", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults, bKeepNulls:=True)
                        If IsArray(vResults) Then
                            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                            If Not IsDBNull(vResults(0, 0)) Then
                                'JB Oct 2009: Converted lNextNumber from integer to long to hold the larger values
                                'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                lNextNumber = vResults(0, 0)
                                'the table that affects unique_number has at least one entry so add/update unique_number
                                'UPGRADE_WARNING: Couldn't resolve default property of object vResults. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                vResults = Nothing
                                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                sSQL = "select next_number from unique_number where table_name='" & g_aIeControl(lRowCount)(pbIeControl_objectName) & "'"
                                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="updating unique_number 2", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                                If IsArray(vResults) Then
                                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    sSQL = "update unique_number set next_number=" & lNextNumber & " where table_name='" & g_aIeControl(lRowCount)(pbIeControl_objectName) & "'"
                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GenericInsert", bStoredProcedure:=False)
                                Else
                                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    sSQL = "insert into unique_number values('" & g_aIeControl(lRowCount)(pbIeControl_objectName) & "'," & lNextNumber & ")"
                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GenericInsert", bStoredProcedure:=False)
                                End If
                            End If
                        End If
                    End If

                End If
            Next

            'regenerate caption seed
            m_lReturn = r_oDatabase.SQLSelect(sSQL:="spu_pm_caption_id_reseed", sSQLName:="spu_pm_caption_id_reseed", bStoredProcedure:=True)

            'regenerate captions on pmproduct
            'this seems to break navigator captions so don't call it!
            'm_lReturn = RegenerateCaptions(r_oDatabase)

            '--------------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'loop over the data models and do the remaining processing
            Dim r_vResults(,) As Object
            Dim lDataModelId As Integer
            For iCounter = 0 To UBound(v_alDataModelID)
                'Richard Clarke November 2008 - PIE enhancements
                Debug.Print(CStr(iCounter) & " / " & CStr(UBound(v_alDataModelID)))
                ' BSJ Sep 2009 - Use the correct Datamodel code and ID
                'Setup the SQL for retreival
                sSQL = conEmptyString
                sSQL = sSQL & "SELECT gis_data_model_id FROM gis_data_model "
                sSQL = sSQL & "WHERE code = '" & v_asDataModelCode(iCounter) & "'"

                'get the information
                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    doImport = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                If IsArray(r_vResults) Then
                    'Set the global variables for use by the general replacement sub
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    lDataModelId = r_vResults(0, 0)
                Else
                    lDataModelId = v_alDataModelID(iCounter)
                End If
                r_oDatabase.SQLBeginTrans()
                'add/change data model derived tables
                If ImportUserTables(r_oDatabase:=r_oDatabase, v_sGISDMCode:=v_asDataModelCode(iCounter), v_lDataModelID:=lDataModelId) = gPMConstants.PMEReturnCode.PMTrue Then
                    r_oDatabase.SQLCommitTrans()
                Else
                    LogPIEError("", True, False, "", False, v_asDataModelCode(iCounter), "ImportUserTables")
                    r_oDatabase.SQLRollbackTrans()
                End If

                're-generate the XML
                m_lReturn = CreateDataSet(v_sGISDataModel:=v_asDataModelCode(iCounter))

                'create the lookup flat files
                m_lReturn = CreateLookupFile(r_oDatabase:=r_oDatabase, v_sDataModelCode:=v_asDataModelCode(iCounter))

                'do we need to do any registry processing
                If objFrmMainForm.optAdditionalImportOptions(optAdditionalImportOptions_DefultRegistry).Checked = True Then
                    m_lReturn = CreateRegistrySettings(v_sGISDataModel:=v_asDataModelCode(iCounter))
                End If

                ' BSJ Oct 2009 deal with gis_screen_detail
                m_lReturn = SetNewIDsForGisScreenDetail(r_oDatabase, lDataModelId)
            Next iCounter 'loop around data models added
            'Richard Clarke November 2008 - PIE enhancements
            'Start-PM038738 - Run SPU when all userDefined table import completed.
            If g_UserSPU IsNot Nothing Then
                For iCounter = 0 To UBound(g_UserSPU)
                    objFrmMainForm.StatusBar_TextWrite("Import", 0)
                    objFrmMainForm.StatusBar_TextWrite("User-defined SPU", 1)
                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

                    m_lReturn = RunSPUScript(r_oDatabase, g_UserSPU(iCounter))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        doImport = gPMConstants.PMEReturnCode.PMError
                        Exit Function
                    End If
                Next
            End If
            'End-PM038738

            'Start-PM038738 - Run SPU when all userDefined table import completed.
            If g_UserSPU IsNot Nothing AndAlso IsArray(g_UserSPU) Then
                For iCounter = 0 To UBound(g_UserSPU)
                    objFrmMainForm.StatusBar_TextWrite("Import", 0)
                    objFrmMainForm.StatusBar_TextWrite("User-defined SPU", 1)
                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                    m_lReturn = RunSPUScript(r_oDatabase, g_UserSPU(iCounter))
                Next
            End If
            'End-PM038738
            '*****************************************************************
            'Re-add the constraints on all the child tables following completion of
            'all database actions
            If AddConstraints() <> gPMConstants.PMEReturnCode.PMTrue Then
                doImport = gPMConstants.PMEReturnCode.PMError
                writeToStatusBox("Failed to Add Constraints. Please check error log for details")
            End If
            'PM032939 - Need DataModelRebuild to run during import.
            Dim proc As Process = New Process()
            proc.StartInfo.FileName = "DatamodelRebuild.exe"
            proc.StartInfo.Arguments = "PBIE"
            proc.Start()
            proc.WaitForExit()
            Dim exitCode As VariantType = proc.ExitCode
            proc.Close()

            'PM032939 - Need DataModelRebuild to run during import.
            Dim proc As Process = New Process()
            proc.StartInfo.FileName = "DatamodelRebuild.exe"
            proc.StartInfo.Arguments = "PBIE"
            proc.Start()
            proc.WaitForExit()
            Dim exitCode As VariantType = proc.ExitCode
            proc.Close()

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".doImport")
            writeToStatusBox(CStr(TimeOfDay))
            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".doImport")

            doImport = gPMConstants.PMEReturnCode.PMError

            LogPIEError("Error in doImport", True, False, "", False, v_asDataModelCode(iCounter), "")


            Exit Function
        End Try

    End Function
    Private Function BuildNukeSQL(ByRef r_sSQL As String, ByVal v_DMCode As String) As Integer

        Try

            BuildNukeSQL = gPMConstants.PMEReturnCode.PMTrue

            r_sSQL = conEmptyString

            r_sSQL = r_sSQL & "exec sp_msforeachtable " & """" & "ALTER TABLE ? NOCHECK CONSTRAINT all" & """"
            r_sSQL = r_sSQL & "exec sp_msforeachtable " & """" & "ALTER TABLE ? DISABLE TRIGGER  all" & """"

            r_sSQL = r_sSQL & vbCrLf

            ''''    r_sSQL = r_sSQL & "DELETE gis_property "
            ''''    r_sSQL = r_sSQL & "FROM gis_property "
            ''''    r_sSQL = r_sSQL & "INNER JOIN gis_object ON gis_property.gis_object_id = gis_object.gis_object_id "
            ''''    r_sSQL = r_sSQL & "INNER JOIN gis_data_model ON gis_object.gis_data_model_id = gis_data_model.gis_data_model_id "
            ''''    r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & v_DMCode & "'" & vbCrLf
            ''''
            ''''    r_sSQL = r_sSQL & "DELETE gis_object "
            ''''    r_sSQL = r_sSQL & "FROM gis_object "
            ''''    r_sSQL = r_sSQL & "INNER JOIN gis_data_model ON gis_object.gis_data_model_id = gis_data_model.gis_data_model_id "
            ''''    r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & v_DMCode & "'" & vbCrLf

            ''r_sSQL = r_sSQL & "DELETE gis_screen_detail "
            ''r_sSQL = r_sSQL & "FROM gis_screen_detail "
            ''r_sSQL = r_sSQL & "INNER JOIN gis_screen ON gis_screen_detail.gis_screen_id = gis_screen.gis_screen_id "
            ''r_sSQL = r_sSQL & "INNER JOIN gis_data_model ON gis_screen.gis_data_model_id = gis_data_model.gis_data_model_id "
            ''r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & Trim(v_DMCode) & "'" & vbCrLf

            ''''    r_sSQL = r_sSQL & "DELETE gis_screen "
            ''''    r_sSQL = r_sSQL & "FROM gis_screen "
            ''''    r_sSQL = r_sSQL & "INNER JOIN gis_data_model ON gis_screen.gis_data_model_id = gis_data_model.gis_data_model_id "
            ''''    r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & v_DMCode & "'" & vbCrLf
            ''''
            ''''    r_sSQL = r_sSQL & "DELETE GIS_QEM_Usage "
            ''''    r_sSQL = r_sSQL & "FROM GIS_QEM_Usage "
            ''''    r_sSQL = r_sSQL & "INNER JOIN gis_data_model ON GIS_QEM_Usage.gis_data_model_id = gis_data_model.gis_data_model_id "
            ''''    r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & v_DMCode & "'" & vbCrLf
            ''''
            ''''    r_sSQL = r_sSQL & "DELETE GIS_Data_Model_Business "
            ''''    r_sSQL = r_sSQL & "FROM GIS_Data_Model_Business "
            ''''    r_sSQL = r_sSQL & "INNER JOIN gis_data_model ON GIS_Data_Model_Business.gis_data_model_id = gis_data_model.gis_data_model_id "
            ''''    r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & v_DMCode & "'" & vbCrLf
            ''''
            ''''    r_sSQL = r_sSQL & "DELETE gis_data_model "
            ''''    r_sSQL = r_sSQL & "WHERE gis_data_model.code = '" & v_DMCode & "'" & vbCrLf

            ' Exit before error trap
            Exit Function
        Catch ex As Exception
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".BuildNukeSQL")

            BuildNukeSQL = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildNukeSQL Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildNukeSQL", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try

    End Function
    ' ***************************************************************** '
    '
    ' Name: DropConstraints
    '
    ' Description:
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function DropConstraints() As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".DropConstraints")

        Try

            DropConstraints = gPMConstants.PMEReturnCode.PMTrue

            Dim iLoop As Short

            '*****************************************************************
            'Drop the constraints on all the child tables prior to attempting any
            'database actions.  This is because the import file will probably try to
            'insert records into child tables before the parent record is present.

            'Constraints will be re-added at the end, this assumes that the overall
            'integrity of the data contained in the import file is OK.

            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Dropping contraints"
            objFrmMainForm.StatusBar_TextWrite("Dropping constraints", 1)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = ""
            objFrmMainForm.StatusBar_TextWrite("", 2)
            System.Windows.Forms.Application.DoEvents()


            For iLoop = 0 To UBound(g_aIeControl)
                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iLoop)(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (g_aIeControl(iLoop)(pbIeControl_objectType) = pbIeOt_dbTable_fixed) Or (g_aIeControl(iLoop)(pbIeControl_objectType) = pbIeOt_dbTable_child) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iLoop)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = g_aIeControl(iLoop)(pbIeControl_objectName)
                    objFrmMainForm.StatusBar_TextWrite(g_aIeControl(iLoop)(pbIeControl_objectName), 2)
                    System.Windows.Forms.Application.DoEvents()
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    m_lReturn = DisableEnableConstraints(r_oDatabase:=g_oDatabase, v_sTableName:=g_aIeControl(iLoop)(pbIeControl_objectName), v_ProcessType:=conDisable)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        DropConstraints = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            Next iLoop

            '*****************************************************************

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".DropConstraints")

            Exit Function
        Catch ex As Exception
            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".DropConstraints")

            DropConstraints = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DropConstraints Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DropConstraints", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddConstraints
    '
    ' Description:
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function AddConstraints() As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".AddConstraints")

        Try


            AddConstraints = gPMConstants.PMEReturnCode.PMTrue

            Dim iLoop As Short

            '*****************************************************************
            'Re-add the constraints on all the child tables following completion of
            'all database actions

            For iLoop = 0 To UBound(g_aIeControl)
                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iLoop)(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If g_aIeControl(iLoop)(pbIeControl_objectType) = pbIeOt_dbTable_fixed Or (g_aIeControl(iLoop)(pbIeControl_objectType) = pbIeOt_dbTable_child) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    m_lReturn = DisableEnableConstraints(r_oDatabase:=g_oDatabase, v_sTableName:=g_aIeControl(iLoop)(pbIeControl_objectName), v_ProcessType:=conEnable)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        AddConstraints = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            Next iLoop

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".AddConstraints")

            Exit Function
        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".AddConstraints")

            AddConstraints = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddConstraints Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddConstraints", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try

    End Function
    ' ***************************************************************** '
    '
    ' Name: RegenerateCaptions
    '
    ' Description:
    '
    ' History: 23/10/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function RegenerateCaptions(ByRef r_oDatabase As dPMDAO.Database) As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".RegenerateCaptions")

        Try

            RegenerateCaptions = gPMConstants.PMEReturnCode.PMTrue

            Dim vTablesWithCaptions(,) As Object
            Dim vResults(,) As Object
            Dim vResults2(,) As Object
            Dim iTablesWithCaptionsLoop As Object
            Dim iLoop As Short
            Dim icaptionIdColumn As Short
            Dim iCaptionDescriptionColumn As Short
            Dim vLocal_aIeControl() As Object
            Dim vLocal_aIeTableDefinitions() As Object

            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Re-builiding captions"
            objFrmMainForm.StatusBar_TextWrite("Re-builiding captions", 0)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = ""
            objFrmMainForm.StatusBar_TextWrite("", 1)

            RegenerateCaptions = r_oDatabase.SQLSelect(sSQL:="select name from sysobjects where id in (select id from syscolumns where name='caption_id') order by name", sSQLName:="Get tables with captions", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTablesWithCaptions)

            For iTablesWithCaptionsLoop = 0 To UBound(vTablesWithCaptions, 2)

                'UPGRADE_WARNING: Couldn't resolve default property of object vTablesWithCaptions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If InStr(LCase(vTablesWithCaptions(0, iTablesWithCaptionsLoop)), "pmcaption") = 0 Then

                    'UPGRADE_WARNING: Couldn't resolve default property of object vTablesWithCaptions(0, iTablesWithCaptionsLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTablesWithCaptions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = vTablesWithCaptions(0, iTablesWithCaptionsLoop)
                    objFrmMainForm.StatusBar_TextWrite(vTablesWithCaptions(0, iTablesWithCaptionsLoop), 2)
                    System.Windows.Forms.Application.DoEvents()

                    Erase vLocal_aIeControl
                    Erase vLocal_aIeTableDefinitions
                    'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    addToArray(vLocal_aIeControl, New Object() {0, "header", vTablesWithCaptions(0, iTablesWithCaptionsLoop), 0, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0})


                    'UPGRADE_WARNING: Couldn't resolve default property of object vTablesWithCaptions(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    RegenerateCaptions = GetColumnDetails(r_oDatabase:=r_oDatabase, sObjectName:=vTablesWithCaptions(0, iTablesWithCaptionsLoop), v_iTableIndex:=0, r_aIeControl:=vLocal_aIeControl, r_aIeTableDefinitions:=vLocal_aIeTableDefinitions, v_sColumnFilter:="")

                    icaptionIdColumn = findColumn("caption_id", vLocal_aIeTableDefinitions(0))
                    iCaptionDescriptionColumn = findColumn("description", vLocal_aIeTableDefinitions(0))

                    'UPGRADE_WARNING: Couldn't resolve default property of object vLocal_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If icaptionIdColumn = -1 Or iCaptionDescriptionColumn = -1 Or InStr(LCase(vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName)), "id") = 0 Then
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vLocal_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        RegenerateCaptions = r_oDatabase.SQLSelect(sSQL:="select " & vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName) & ",caption_id,description from " & vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_objectName), sSQLName:="Get ", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                        If IsArray(vResults) Then
                            For iLoop = 0 To UBound(vResults, 2)

                                With r_oDatabase
                                    With .Parameters
                                        'Clear any existing parameters
                                        .Clear()

                                        'Add the required parameters
                                        m_lReturn = .Add(sName:="language_id", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                                        m_lReturn = .Add(sName:="caption", vValue:=vResults(2, iLoop), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                        m_lReturn = .Add(sName:="caption_id", vValue:=9, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                                    End With

                                    'Retrieve the appropriate caption_id for the description
                                    m_lReturn = .SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=vResults2)

                                    'Ensure that the query ran successfully
                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                        ' Log Error Message
                                        RegenerateCaptions = gPMConstants.PMEReturnCode.PMFalse

                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="r_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductByAgent")

                                        Exit Function
                                    End If

                                    'Update the array with the correct value returned in the parameter
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    vResults(1, iLoop) = CInt(r_oDatabase.Parameters.Item("caption_id").Value)

                                    'now update the  table caption ids
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(0, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(1, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vLocal_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    RegenerateCaptions = r_oDatabase.SQLSelect(sSQL:="update " & vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_objectName) & " set caption_id=" & vResults(1, iLoop) & " where " & vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName) & "=" & vResults(0, iLoop), sSQLName:="Update  caption_id", bStoredProcedure:=False)


                                End With
                            Next

                        End If
                    End If
                End If
            Next
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = ""
            objFrmMainForm.StatusBar_TextWrite("", 0)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = ""
            objFrmMainForm.StatusBar_TextWrite("", 2)

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".RegenerateCaptions")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & "." & ACClass & ".RegenerateCaptions")

            RegenerateCaptions = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegenerateCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RegenerateCaptions", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function
End Module