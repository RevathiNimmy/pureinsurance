Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module pbIDoImport

    Private m_lReturn As gPMConstants.PMEReturnCode

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
    Public Function doImport(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef g_aIeControl() As Object, ByRef g_aIeTableDefinitions() As Object, ByVal v_bCheckHeaderOnly As Boolean, ByVal v_sVersionNumber As String, ByVal v_bImportRegistry As Boolean) As Integer 

        'Local definitions
        Dim result As Integer = 0 
        Dim lIntegerData, lRowCount, lTotalLines As Long 
        Dim iImportMode As Integer 

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".doImport")

            'Assume the function will be succesful
            result = gPMConstants.PMEReturnCode.PMTrue

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
            If v_bCheckHeaderOnly Then
                objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Import"
            Else
                objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Import (0 of ?)"
            End If
            objFrmMainForm.ProgressBar1(1).Visible = False

            If Not v_bCheckHeaderOnly Then
                m_lReturn = DropConstraints()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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
                If g_aIeControl.GetUpperBound(0) < lIntegerData Then
                    writeToStatusBox("Possible corrupt or invalid export file")
                    Return gPMConstants.PMEReturnCode.PMError
                End If

                'Determine the type of object that we're dealing with, to enable the rest of
                'the row to be returned in the correct format.  Sequence of imports matches
                'the sequence of exports defined in InitialiseArray

                Select Case g_aIeControl(lIntegerData)(pbIeControl_objectType)
                    Case pbIeOt_Header
                        m_lReturn = ImportHeader(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData, r_lDataModelId:=v_lDataModelId, r_sDataModelCode:=v_sDataModelCode, r_iMode:=iImportMode, v_sVersionNumber:=v_sVersionNumber, r_lTotalLines:=lTotalLines)
                        'we've got line information so we can update the progress bar
                        objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Import (" & lRowCount & " of " & CStr(lTotalLines) & ")"
                        objFrmMainForm.ProgressBar1(0).Value = 100 / (lTotalLines / 1)

                    Case pbIeOt_FixedTableColumns
                        m_lReturn = ImportFixedTableDefinition(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iImportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=lIntegerData)

                    Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined, pbIeOt_dbTable_child, pbIeOt_RiskGroupsCodes

                        m_lReturn = ImportFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iImportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=lIntegerData)

                    Case pbIeOt_RegSetting
                        m_lReturn = ImportRegistrySettings(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData, v_sDataModelCode:=v_sDataModelCode, v_bImportRegistry:=v_bImportRegistry)

                    Case pbIeOt_RuleFile

                        m_lReturn = ImportRuleFile(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData)

                    Case pbIeOt_DocumentTemplate

                        m_lReturn = ImportDocumentTemplate(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_iIntegerData:=lIntegerData)

                    Case pbIeOt_UserDefinedListHeader, pbIeOt_UserDefinedList

                        m_lReturn = ImportUserDefinedList(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iImportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl(lIntegerData), r_aIeTableDefinitions:=g_aIeTableDefinitions(lIntegerData), iTableIndex:=lIntegerData)

                    Case pbIeOt_GisList

                        m_lReturn = ImportGisList(v_iFileNumber:=v_iFileNumber, v_aIeControl:=g_aIeControl, v_aIeTableDefinitions:=g_aIeTableDefinitions, v_lIntegerData:=lIntegerData, v_sDataModelCode:=v_sDataModelCode)

                    Case Else

                        MessageBox.Show("Attempting to load an unknown type of " & CStr(g_aIeControl(lIntegerData)(pbIeControl_objectType)), Application.ProductName)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMError
                End If

                If v_bCheckHeaderOnly And (g_aIeControl(lIntegerData)(pbIeControl_objectType)) = pbIeOt_Header Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                'Increment the row count for debugging purposes
                lRowCount += 1
                
                If lRowCount Mod 100 = 0 Then
                    objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Import (" & lRowCount & " of " & CStr(lTotalLines) & ")"
                    objFrmMainForm.ProgressBar1(0).Value = IIf((100 / ((lTotalLines + 1) / (lRowCount + 1))) > 100, 100, (100 / ((lTotalLines + 1) / (lRowCount + 1))))
                    Application.DoEvents()
                End If
            Loop

            'update unique_number table
            Dim sSQL As String = "" 
            Dim vResults(,) As Object 
            Dim lNextNumber As Integer 
            For lRowCount = 0 To g_aIeControl.GetUpperBound(0)

                If g_aIeControl(lRowCount)(pbIeControl_Flags) And pbIeControl_Flags__uniqueNumber Then
                    'found a table that needs to update unique_number
                    sSQL = "select max(" & CStr(g_aIeControl(lRowCount)(pbIeControl_objectName)) & "_id" & ")+1 from " & CStr(g_aIeControl(lRowCount)(pbIeControl_objectName))
                    m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="updating unique_number 1", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults, bKeepNulls:=True)
                    If Information.IsArray(vResults) Then
                        Dim auxVar As Object = vResults(0, 0) 

                        If Not (IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                            lNextNumber = CInt(vResults(0, 0))
                            'the table that affects unique_number has at least one entry so add/update unique_number

                            vResults = Nothing

                            sSQL = "select next_number from unique_number where table_name='" & CStr(g_aIeControl(lRowCount)(pbIeControl_objectName)) & "'"
                            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="updating unique_number 2", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                            If Information.IsArray(vResults) Then

                                sSQL = "update unique_number set next_number=" & lNextNumber & " where table_name='" & CStr(g_aIeControl(lRowCount)(pbIeControl_objectName)) & "'"
                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GenericInsert", bStoredProcedure:=False)
                            Else

                                sSQL = "insert into unique_number values('" & CStr(g_aIeControl(lRowCount)(pbIeControl_objectName)) & "'," & CStr(lNextNumber) & ")"
                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GenericInsert", bStoredProcedure:=False)
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

            r_oDatabase.SQLBeginTrans()

            'add/change data model derived tables
            If ImportUserTables(r_oDatabase:=r_oDatabase, v_sGISDMCode:=v_sDataModelCode, v_lDataModelId:=v_lDataModelId) = gPMConstants.PMEReturnCode.PMTrue Then
                r_oDatabase.SQLCommitTrans()
            Else
                'really ought to inform the user that importusertables failed here
                objFrmMainForm.txtWarning_TextWrite("Import failed, rolling back changes", 1)
                r_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            're-generate the XML
            m_lReturn = CreateDataSet(v_sGISDataModel:=v_sDataModelCode)

            'create the lookup flat files
            m_lReturn = CreateLookupFile(r_oDatabase:=r_oDatabase, v_sDataModelCode:=v_sDataModelCode)

            'do we need to do any registry processing
            If objFrmMainForm.optAdditionalImportOptions(optAdditionalImportOptions_DefultRegistry).Checked Then
                m_lReturn = CreateRegistrySettings(v_sGISDataModel:=v_sDataModelCode)
            End If

            '*****************************************************************
            'Re-add the constraints on all the child tables following completion of
            'all database actions

            If g_UserSPU IsNot Nothing AndAlso IsArray(g_UserSPU) Then
                For iCounter As Integer = 0 To UBound(g_UserSPU)
                    objFrmMainForm.StatusBar_TextWrite("Import", 0)
                    objFrmMainForm.StatusBar_TextWrite("User-defined SPU", 1)
                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                    m_lReturn = RunSPUScript(r_oDatabase, g_UserSPU(iCounter))
                Next
            End If


            m_lReturn = AddConstraints()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                m_lReturn = CloseBinaryFile(v_iFileNumber:=v_iFileNumber)
                Return result
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".doImport")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".doImport")
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="doImport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="doImport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

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

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".DropConstraints")

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '*****************************************************************
            'Drop the constraints on all the child tables prior to attempting any
            'database actions.  This is because the import file will probably try to
            'insert records into child tables before the parent record is present.

            'Constraints will be re-added at the end, this assumes that the overall
            'integrity of the data contained in the import file is OK.

            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Dropping contraints"
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = ""
            objFrmMainForm.StatusBar_TextWrite("Dropping constraints", 1)
            objFrmMainForm.StatusBar_TextWrite("", 2)

            Application.DoEvents()

            For iLoop As Integer = 0 To g_aIeControl.GetUpperBound(0)

                If ((g_aIeControl(iLoop)(pbIeControl_objectType)) = pbIeOt_dbTable_fixed) Or ((g_aIeControl(iLoop)(pbIeControl_objectType)) = pbIeOt_dbTable_child) Then

                    objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(g_aIeControl(iLoop)(pbIeControl_objectName))
                    Application.DoEvents()

                    m_lReturn = CType(DisableEnableConstraints(r_oDatabase:=g_oDatabase, v_sTableName:=CStr(g_aIeControl(iLoop)(pbIeControl_objectName)), v_ProcessType:=conDisable), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next iLoop

            '*****************************************************************

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".DropConstraints")

            Return result

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

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".AddConstraints")

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '*****************************************************************
            'Re-add the constraints on all the child tables following completion of
            'all database actions

            For iLoop As Integer = 0 To g_aIeControl.GetUpperBound(0)

                If (g_aIeControl(iLoop)(pbIeControl_objectType)) = pbIeOt_dbTable_fixed Or ((g_aIeControl(iLoop)(pbIeControl_objectType)) = pbIeOt_dbTable_child) Then

                    m_lReturn = CType(DisableEnableConstraints(r_oDatabase:=g_oDatabase, v_sTableName:=CStr(g_aIeControl(iLoop)(pbIeControl_objectName)), v_ProcessType:=conEnable), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next iLoop

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".AddConstraints")

            Return result

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

        Dim result As Integer = 0 
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RegenerateCaptions")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vTablesWithCaptions, vResults, vResults2(,) As Object 
            Dim icaptionIdColumn, iCaptionDescriptionColumn As Integer 
            Dim vLocal_aIeControl() As Object, vLocal_aIeTableDefinitions() As Object 

            'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Re-builiding captions"
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = ""
            objFrmMainForm.StatusBar_TextWrite("Re-building captions", 0)
            objFrmMainForm.StatusBar_TextWrite("", 1)

            result = r_oDatabase.SQLSelect(sSQL:="select name from sysobjects where id in (select id from syscolumns where name='caption_id') order by name", sSQLName:="Get tables with captions", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTablesWithCaptions)

            For iTablesWithCaptionsLoop As Integer = 0 To vTablesWithCaptions.GetUpperBound(1)

                If (CStr(vTablesWithCaptions(0, iTablesWithCaptionsLoop)).ToLower().IndexOf("pmcaption") + 1) = 0 Then

                    objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(vTablesWithCaptions(0, iTablesWithCaptionsLoop))
                    Application.DoEvents()

                    Erase vLocal_aIeControl
                    Erase vLocal_aIeTableDefinitions

                    addToArray(vLocal_aIeControl, New Object() {0, "header", vTablesWithCaptions(0, iTablesWithCaptionsLoop), 0, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0})

                    result = GetColumnDetails(r_oDatabase:=r_oDatabase, sObjectName:=CStr(vTablesWithCaptions(0, iTablesWithCaptionsLoop)), v_iTableIndex:=0, r_aIeControl:=vLocal_aIeControl, r_aIeTableDefinitions:=vLocal_aIeTableDefinitions, v_sColumnFilter:="")

                    icaptionIdColumn = findColumn("caption_id", vLocal_aIeTableDefinitions(0))

                    iCaptionDescriptionColumn = findColumn("description", vLocal_aIeTableDefinitions(0))

                    If icaptionIdColumn = -1 Or iCaptionDescriptionColumn = -1 Or (CStr(vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName)).ToLower().IndexOf("id") + 1) = 0 Then
                    Else

                        result = r_oDatabase.SQLSelect(sSQL:="select " & CStr(vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName)) & _
                                 ",caption_id,description from " & CStr(vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_objectName)), sSQLName:="Get ", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                        If Information.IsArray(vResults) Then

                            For iLoop As Integer = 0 To vResults.GetUpperBound(1)

                                With r_oDatabase
                                    With .Parameters
                                        'Clear any existing parameters
                                        .Clear()

                                        'Add the required parameters
                                        m_lReturn = .Add(sName:="language_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                                        m_lReturn = .Add(sName:="caption", vValue:=CStr(vResults(2, iLoop)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                        m_lReturn = .Add(sName:="caption_id", vValue:=CStr(9), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                                    End With

                                    'Retrieve the appropriate caption_id for the description
                                    m_lReturn = .SQLSelect(sSQL:=ACGetPMCaptionSQL, sSQLName:=ACGetPMCaptionName, bStoredProcedure:=ACGetPMCaptionStored, vResultArray:=vResults2)

                                    'Ensure that the query ran successfully
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ' Log Error Message
                                        result = gPMConstants.PMEReturnCode.PMFalse

                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="r_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductByAgent")

                                        Return result
                                    End If

                                    'Update the array with the correct value returned in the parameter

                                    vResults(1, iLoop) = r_oDatabase.Parameters.Item("caption_id").Value

                                    'now update the  table caption ids

                                    result = r_oDatabase.SQLSelect(sSQL:="update " & CStr(vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_objectName)) & " set caption_id=" & _
                                             CStr(vResults(1, iLoop)) & " where " & CStr(vLocal_aIeTableDefinitions(0)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName)) & "=" & CStr(vResults(0, iLoop)), sSQLName:="Update  caption_id", bStoredProcedure:=False)

                                End With
                            Next

                        End If
                    End If
                End If
            Next
            objFrmMainForm.StatusBar1(0).Items.Item(0).Text = ""
            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = ""

            'r_aIeTableDefinitions(v_itable_id)(pbIeTableDefinitions_columnArray)(0)(pbIeTableDefinitions_columnName)
            'r_aIeTableDefinitions(v_itable_id)(pbIeTableDefinitions_columnArray)(icaptionIdColumn)(pbIeTableDefinitions_columnName)
            'r_aIeTableDefinitions(v_itable_id)(pbIeTableDefinitions_columnArray)(iCaptionDescriptionColumn)(pbIeTableDefinitions_columnName)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RegenerateCaptions")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RegenerateCaptions")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegenerateCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RegenerateCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module

