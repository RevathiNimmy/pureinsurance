Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports SharedFiles
Imports System.IO.Compression
Imports Ionic.Zip

Module pbImportExportTables

    Private Const ACClass As String = conEmptyString
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sDocPath As String
    ' ***************************************************************** '
    '
    ' Name:        ExtractFixedTables
    '
    ' Description: Processes the extraction of fixed tables to the binary
    '              file.  Recieves the current control file array row and
    '              the file number of hte file being processed
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractFixedTables(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer, ByVal v_iExportMode As Object) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim vResults(,) As Object
        Dim sStatusText As String = ""
        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractFixedTables")

            result = gPMConstants.PMEReturnCode.PMTrue

            If UCase(Left(r_aIeControl(iTableIndex)(pbIeControl_objectName), 12)) = "GIS_USER_DEF" AndAlso objFrmMainForm.chkIncludeUMLs.Checked Then
                ExtractFixedTables = gPMConstants.PMEReturnCode.PMTrue
                Exit Function
            End If

            'Set the panel to indicate the  type of extract
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Database table"
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName))
            objFrmMainForm.StatusBar_TextWrite("Database table", 1)
            objFrmMainForm.StatusBar_TextWrite(CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName)), 2)
            objFrmMainForm.ProgressBar1(1).Visible = True
            'objFrmMainForm.ProgressBar1(1).Value = 1
            objFrmMainForm.ProgressBar1(1).PerformStep()

            If ReadRecordsFromDatabase(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iTableIndex, v_sObjectName:=CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName)), v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, r_vResults:=vResults, v_sConstrainColumns:="*", v_sStatusText:=sStatusText) = gPMConstants.PMEReturnCode.PMTrue Then

                'ensure GUI is updated
                ' Too much!  objFrmMainForm.StatusBar1(2).Panels(1).Text = sStatusText :  DoEvents
                If Information.IsArray(vResults) Then

                    For lResultIndex As Integer = 0 To vResults.GetUpperBound(1)

                        objFrmMainForm.ProgressBar1(1).Value = 100 / ((vResults.GetUpperBound(1) + 1) / (lResultIndex + 1))
                        If lResultIndex Mod 100 = 0 Then

                            If vResults.GetUpperBound(1) > 1000 Or lResultIndex = 0 Then
                                If lResultIndex > 0 Then

                                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sStatusText & " (" & CStr(lResultIndex) & " of " & CStr(vResults.GetUpperBound(1)) & ")"
                                    objFrmMainForm.StatusBar_TextWrite(sStatusText & " (" & CStr(lResultIndex) & " of " & CStr(vResults.GetUpperBound(1)) & ")", 2)
                                Else
                                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sStatusText
                                    objFrmMainForm.StatusBar_TextWrite(sStatusText, 2)
                                End If
                            End If
                            Application.DoEvents()
                        End If

                        'Write the row passing an array containing the actual data and an
                        'array containing the description of the data in array 1.  Also
                        'pass the file number of the current extract file
                        'add type here!!!!

                        m_lReturn = CType(WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(r_aIeControl(iTableIndex)(0)), i_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=vResults, rowIndex:=lResultIndex, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If g_bStopProcessing Then
                            Return result
                        Else
                            'now we need to look down the whole control array to see if we have any childern of this object
                            'we only need to go 1 level deep (now) as we are checking the whole of the array so check if
                            'v_vParentResults is not null and if it is don't go any further
                            For iChildIndex As Integer = 0 To g_aIeControl.GetUpperBound(0)

                                If CBool(r_aIeControl(iChildIndex)(pbIeControl_RelatedObjectId) = iTableIndex And r_aIeControl(iChildIndex)(pbIeControl_operationMode) And v_iExportMode) Then
                                    If iChildIndex = 71 Then
                                        Debug.WriteLine("Found doc template")
                                    End If
                                    Select Case r_aIeControl(iChildIndex)(pbIeControl_objectType)
                                        Case pbIeOt_dbTable_child
                                            m_lReturn = CType(ExtractFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iChildIndex, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_iExportMode:=v_iExportMode), gPMConstants.PMEReturnCode)
                                            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sStatusText
                                            Application.DoEvents()
                                        Case pbIeOt_DerivedRatingRuleFile
                                            If v_iMode And pbIeMode_RuleFiles Then

                                                m_lReturn = CType(ExtractDerivedRatingRuleFile(v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vResults:=vResults, lParentResultIndex:=lResultIndex, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)
                                            End If
                                        Case pbIeOt_DocumentTemplate
                                            If v_iMode And pbIeMode_Documents Then

                                                m_lReturn = CType(ExtractDocumentTemplateFile(iTableIndex:=iTableIndex, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, r_oDatabase:=r_oDatabase, v_iMode:=v_iMode), gPMConstants.PMEReturnCode)
                                            End If
                                        Case pbIeOt_DerivedDefAndValRuleFile
                                            If v_iMode And pbIeMode_RuleFiles Then
                                                'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Derived rule file"
                                                'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
                                                objFrmMainForm.StatusBar_TextWrite("Derived rule file", 1)
                                                objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

                                                m_lReturn = CType(ExtractDerivedRuleFiles(v_iParentIndex:=iTableIndex, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)
                                            End If
                                        Case pbIeOt_GPLookup

                                            m_lReturn = CType(ExtractPropertyDerivedLookups(r_oDatabase:=r_oDatabase, v_iParentIndex:=iTableIndex, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                                        Case Else
                                    End Select

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                End If
                            Next iChildIndex
                        End If

                        If (r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) <> 0 Then

                            If CStr(vResults((r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) - 1, lResultIndex)) <> conEmptyString And CStr(vResults((r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) - 1, lResultIndex)) <> "<NULL>" Then 'Or |                        'r_aIeControl(iTableIndex)(pbIeControl_objectName) = "gis_scheme" Then
                                'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "rule file"
                                objFrmMainForm.StatusBar_TextWrite("rule file", 1)
                                objFrmMainForm.StatusBar_TextWrite(CStr(vResults((r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) - 1, lResultIndex)), 2)

                                'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(vResults((r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) - 1, lResultIndex))

                                m_lReturn = CType(ExtractRuleFile(iTableIndex:=iTableIndex, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=CStr(vResults((r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) - 1, lResultIndex)), v_sNewFilename:=CStr(vResults((r_aIeControl(iTableIndex)(pbIeControl_HasFilename)) - 1, lResultIndex)), v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=CInt(vResults(0, lResultIndex))), gPMConstants.PMEReturnCode)

                                '                        If m_lReturn <> PMTrue Then
                                '                            ExtractFixedTables = PMFalse
                                '                            Exit Function
                                '                        End If
                            End If
                        End If
                    Next lResultIndex
                Else
                    'no results found
                End If
            Else
                'read returned pmfalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractFixedTables")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractFixedTables")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractFixedTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractFixedTables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractDerivedRuleFiles
    '
    ' Description: Processes the extraction of the rule files derived from screen codes
    '
    ' History:     09/06/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractDerivedRuleFiles(ByVal v_iParentIndex As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef v_vParentResults(,) As Object, ByVal lParentResultIndex As Integer, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim iColumnDocumentCode As Integer
        Dim sFileName As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractDerivedRuleFiles")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Screen derived file extract"
            objFrmMainForm.StatusBar_TextWrite("Screen derived file extract", 1)

            If Information.IsArray(v_vParentResults) Then
                'need to find the columns we are interested in

                iColumnDocumentCode = findColumn("code", r_aIeTableDefinitions(v_iParentIndex))

                If iColumnDocumentCode <> -1 Then

                    'build up the document template file name from the document template data
                    'Type + document_type_id + '\doc ' + document_template_id + '.zip'

                    'create default file name and extract

                    sFileName = CStr(v_vParentResults(iColumnDocumentCode, lParentResultIndex)).Trim() & "def.rul"
                    'Set the panel to indicate the  type of extract
                    objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sFileName
                    m_lReturn = CType(ExtractRuleFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=sFileName, v_sNewFilename:=sFileName, v_sDataModelName:=v_sDataModelName, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=0), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'create validation file name and extract

                    sFileName = CStr(v_vParentResults(iColumnDocumentCode, lParentResultIndex)).Trim() & "val.rul"
                    'Set the panel to indicate the  type of extract
                    objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sFileName
                    m_lReturn = CType(ExtractRuleFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=sFileName, v_sNewFilename:=sFileName, v_sDataModelName:=v_sDataModelName, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=0), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    writeToStatusBox("Could not identify screen column for derived file export")
                End If
            Else
                'no results found
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractDerivedRuleFiles")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractDerivedRuleFiles")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractDerivedRuleFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractDerivedRuleFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ExtractRuleFile
    '
    ' Description:
    '
    ' History: 04/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractRuleFile(ByVal iTableIndex As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_sOriginalFilename As String, ByVal v_sNewFilename As String, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Integer, ByRef r_lTotalLinesWritten As Integer, ByVal v_lSchemeId As Integer) As Integer

        Dim result As Integer = 0
        Dim iFileNumber As gPMConstants.PMEReturnCode
        Dim sPathName As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractRuleFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            '    If r_aIeControl(iTableIndex)(pbIeControl_objectName) = "gis_scheme" Then
            ':
            '        Dim sSQL As String
            '        Dim vResults As Variant
            '        g_oDatabase.Parameters.Clear
            '
            '        sSQL = "{call spu_GIS_Get_Rule_Filename()}"
            '
            '        AddParameter g_oDatabase, sSQL, m_lReturn, "Schemeid", v_lSchemeId, PMParamInput, PMInteger
            '        m_lReturn = g_oDatabase.SQLSelect(sSQL:=sSQL, _
            ''                                  sSQLName:=sSQL, _
            ''                                  bStoredProcedure:=True, vResultArray:=vResults)
            '        If m_lReturn = PMTrue Then
            '            v_sOriginalFilename = vResults(0, 0)
            '        Else
            '            g_oDatabase.Parameters.Clear
            '
            '            sSQL = "{call sp_GIS_Get_Rule_Filename()}"
            '
            '            AddParameter g_oDatabase, sSQL, m_lReturn, "Schemeid", v_lSchemeId, PMParamInput, PMInteger
            '            m_lReturn = g_oDatabase.SQLSelect(sSQL:=sSQL, _
            ''                                      sSQLName:=sSQL, _
            ''                                      bStoredProcedure:=True, vResultArray:=vResults)
            '            If m_lReturn = PMTrue Then
            '                v_sOriginalFilename = CInt(g_oDatabase.Parameters.Item("Schemeid").value)
            '            End If
            '
            '        End If
            '
            '    End If

            iFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

            sPathName = sPathName & AddRequiredBackslash(sPathName) & v_sOriginalFilename

            m_lReturn = CType(ExtractFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sFilenameToRead:=sPathName, v_sFilenameToStore:=v_sNewFilename, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'writeToStatusBox "Error : Could not extract " & v_sOriginalFilename
                'to much moaning about missing non mandatory file 'don't stop the export
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractRuleFile")

            Return result

        Catch excep As System.Exception

            FileSystem.FileClose(iFileNumber)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractRuleFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractRuleFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ReadRecordsFromDatabase
    '
    ' Description:
    '
    ' History: 05/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ReadRecordsFromDatabase(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByVal v_sObjectName As String, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_vResults(,) As Object, ByVal v_sConstrainColumns As String, Optional ByRef v_sStatusText As String = "") As Integer
        Dim result As Integer = 0
        Dim iPrimaryKeySearch As Integer

        Dim sSQLSelect, sSQLWhere As String
        Dim sStatusTextWhereClause As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ReadRecordsFromDatabase")

            result = gPMConstants.PMEReturnCode.PMTrue

            'determine what we need to use on the Where clause
            'these are in order of preference so do not change!
            '1) child
            '2) data model code
            '3) data model id
            '4) any additional specified where clause

            v_sStatusText = v_sObjectName

            Dim sParentKeyColumn As String = ""
            If (r_aIeControl(iTableIndex)(pbIeControl_objectType)) = pbIeOt_dbTable_child Then
                : iPrimaryKeySearch = -1

                'get the primary key column from the parent
                'start at 0 and search till (2)
                sSQLSelect = conEmptyString
                sSQLWhere = conEmptyString

                Do While iPrimaryKeySearch < ((r_aIeTableDefinitions(CInt(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId)))(pbIeTableDefinitions_columnCount)) - 1) 'And sSQL = conEmptyString
                    iPrimaryKeySearch += 1

                    sParentKeyColumn = CStr(r_aIeTableDefinitions(CInt(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId)))(2)(iPrimaryKeySearch)(0))
                    'find this column in the child

                    For iColumnIndex As Integer = 0 To ((r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnCount)) - 1)

                        If sParentKeyColumn = CStr(r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(iColumnIndex)(0)) Then
                            sSQLSelect = "SELECT * FROM " & v_sObjectName

                            sSQLWhere = "WHERE " & sParentKeyColumn & " = " & CStr(v_vParentResults(CInt(iPrimaryKeySearch), lParentResultIndex))

                            sStatusTextWhereClause = " (" & sParentKeyColumn & " = " & CStr(v_vParentResults(CInt(iPrimaryKeySearch), lParentResultIndex)) & ")"
                            'if match on the first column on the parent get out now
                            If iPrimaryKeySearch = 0 Then

                                iPrimaryKeySearch = ((r_aIeTableDefinitions(CInt(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId)))(pbIeTableDefinitions_columnCount)) - 1)
                                Exit For
                            End If

                            If sParentKeyColumn = CStr(r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(0)(0)) Then
                                Exit For
                            End If
                        End If
                    Next
                Loop
            ElseIf (r_aIeControl(iTableIndex)(pbIeControl_DataModelCodeColumn)) <> 0 Then
                sSQLSelect = "SELECT " & v_sConstrainColumns & " FROM " & v_sObjectName

                sSQLWhere = "WHERE " & CStr(r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)((r_aIeControl(iTableIndex)(pbIeControl_DataModelCodeColumn)) - 1)(pbIeTableDefinitions_columnName)) & " = '" & v_sDataModelCode & "'"
            ElseIf (r_aIeControl(iTableIndex)(pbIeControl_DataModelIdColumn)) <> 0 Then

                If (r_aIeControl(iTableIndex)(pbIeControl_objectId)) = pbIeDbt_gis_screen Then

                    m_lReturn = CType(GetSelfReferencingData(r_oDatabase:=r_oDatabase, r_vResults:=r_vResults, v_sTableName:=v_sObjectName, v_sConstrainColumns:=v_sConstrainColumns, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_iTableIndex:=iTableIndex, v_lDataModelId:=v_lDataModelId), gPMConstants.PMEReturnCode)
                    sSQLSelect = conEmptyString 'pdone processing

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    sSQLSelect = "SELECT " & v_sConstrainColumns & " FROM " & v_sObjectName

                    sSQLWhere = "WHERE  " & CStr(r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)((r_aIeControl(iTableIndex)(pbIeControl_DataModelIdColumn)) - 1)(pbIeTableDefinitions_columnName)) & " = " & CStr(v_lDataModelId)
                End If
            Else
                sSQLSelect = "SELECT " & v_sConstrainColumns & " FROM " & v_sObjectName
                'This fix avoids edited copies of Templates being taken over and overwriting live edits
                'Edited Copies have negative template ids
                If v_sObjectName = "document_template" Then
                    sSQLWhere = "WHERE document_template_id > 0"
                End If
            End If

            'create status bar text
            v_sStatusText = v_sStatusText & sStatusTextWhereClause

            'append derived where clause
            If sSQLSelect <> "" And sSQLWhere <> "" Then
                sSQLSelect = sSQLSelect & " " & sSQLWhere
            End If

            'append and pre defined where clause

            If CStr(r_aIeControl(iTableIndex)(pbIeControl_WhereClause)) <> conEmptyString Then
                If sSQLWhere <> conEmptyString Then
                    sSQLSelect = sSQLSelect & " and"
                End If

                sSQLSelect = sSQLSelect & " " & CStr(r_aIeControl(iTableIndex)(pbIeControl_WhereClause))
            End If

            'only do this if there is SQL
            'derived screens will have already fetched reults
            If sSQLSelect <> conEmptyString Then

                'get the data for the tablecolumn information for this table
                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSelect, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'if we have some results.....
                If Information.IsArray(r_vResults) Then

                    writeToDebugBox(sSQLSelect & " (" & CStr(r_vResults.GetUpperBound(1) + 1) & " rows)")

                    'sanity check, if we've fetched * then check that received rows have the same number of elements
                    'Raise an error if this is not the case
                    If v_sConstrainColumns = "*" Then

                        If ((r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnCount)) - 1) <> r_vResults.GetUpperBound(0) Then
                            writeToStatusBox("Cannot export " & v_sObjectName & ", definition and data arrays are different sizes")
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Else
                    writeToDebugBox(sSQLSelect & " (0 rows)")
                    'no results found
                End If
            Else
                'no SQL generated
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ReadRecordsFromDatabase")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ReadRecordsFromDatabase")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReadRecordsFromDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReadRecordsFromDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ExtractFile
    '
    ' Description:
    '
    ' History: 04/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractFile(ByVal iTableIndex As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_sFilenameToRead As String, ByVal v_sFilenameToStore As String, ByVal v_iFileNumber As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim result As Integer = 0
        Dim fileToImport As String

        Try

            Dim iFileNumber As Integer
            Dim vDataArray(2, 0) As Object
            Dim sPathName As String = ""

            Dim sTheData As String = ""
            Dim oPMZipper As bPMZipper.Business
            Dim lFilelength As Integer
            Dim bDocTemplate As Boolean
            Dim sTemplateFullFileName As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            If System.IO.File.Exists(v_sFilenameToRead) Then
                'this doesn't work first time for some files????
                ' If gPMFunctions.FileExists(v_sFilenameToRead) Then

                'only do this for document templates
                If Path.GetExtension(v_sFilenameToRead) = ".zip" Then
                    bDocTemplate = True
                    'get our zip folders / file into the zipper
                    Dim zFile As ZipFile
                    zFile = ZipFile.Read(v_sFilenameToRead)
                    zFile.FlattenFoldersOnExtract = True


                    'we need the new file path and name
                    Dim filenamescoll As ICollection
                    Dim sTemplateFileName As String = ""
                    filenamescoll = zFile.EntryFileNames
                    Dim fileNameStruct As String()

                    For Each s As String In filenamescoll
                        sTemplateFullFileName = s
                        fileNameStruct = Strings.Split(s, "/")
                    Next

                    'we need to split it out and just use the filename
                    sTemplateFileName = fileNameStruct(UBound(fileNameStruct))
                    fileToImport = m_sDocPath + "\" + sTemplateFileName
                    If FileExists(fileToImport) Then
                        File.Delete(fileToImport)
                    End If

                    'extract it to the root of the PMDocs folder - this is just a temp holding place as we're going to delete it once we've read it into the PIE                    
                    zFile.ExtractAll(m_sDocPath)
                    zFile.Dispose()
                    zFile = Nothing

                    'new file read and compression method
                    Dim reader As New StreamReader(fileToImport, True)
                    Dim inf As System.IO.FileInfo
                    inf = My.Computer.FileSystem.GetFileInfo(fileToImport)
                    lFilelength = inf.Length
                    sTheData = reader.ReadToEnd()
                    reader.Close()
                    reader.Dispose()
                    reader = Nothing
                    'uncomment if we're going to resolve the compression issue
                    'result = Compress(sTheData)

                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractFile")
                        writeToStatusBox("Could not compress " & v_sFilenameToRead)
                        result = gPMConstants.PMEReturnCode.PMError
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If

                    If bDocTemplate Then
                        v_sFilenameToStore = v_sFilenameToStore + "|" + sTemplateFullFileName
                    End If

                    vDataArray(0, 0) = v_sFilenameToStore
                    vDataArray(1, 0) = lFilelength
                    vDataArray(2, 0) = sTheData

                    m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(r_aIeControl(iTableIndex)(0)), _
                                                   i_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), _
                                                   i_aData:=vDataArray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If bDocTemplate Then
                        'now delete the XML file from the temp location
                        File.Delete(fileToImport)
                    End If

                Else
                    'not a document template
                    fileToImport = v_sFilenameToRead
                    oPMZipper = New bPMZipper.Business()

                    iFileNumber = FileSystem.FreeFile()

                    'Open for read and lock for our use                    
                    FileSystem.FileOpen(iFileNumber, v_sFilenameToRead, OpenMode.Binary, OpenAccess.Read)
                    lFilelength = FileSystem.LOF(iFileNumber)
                    sTheData = New String(" ", lFilelength)
                    FileSystem.FileGet(iFileNumber, sTheData, -1)

                    'result = oPMZipper.CompressString(TheString:=sTheData) 'the compression is breaking things so removed it for now

                    vDataArray(0, 0) = v_sFilenameToStore
                    vDataArray(1, 0) = lFilelength
                    vDataArray(2, 0) = sTheData
                    writeToDebugBox(v_sFilenameToRead & " (" & CStr(lFilelength) & " bytes)")

                    'Dim file_ext As String = Path.GetExtension(v_sFilenameToRead) - this is not being used so commented it out
                    m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(r_aIeControl(iTableIndex)(0)), i_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=vDataArray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    FileSystem.FileClose(iFileNumber)
                End If

            Else
                writeToDebugBox(v_sFilenameToRead & " (0 bytes)")
                'The following line cancels the export if the file isn't found
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception

            'not sure we should always be deleting the file here as this function is called on EXPORT (why would we delete the source file?)
            File.Delete(fileToImport)
            writeToStatusBox("Could not access " & v_sFilenameToRead)
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: ExtractDocumentTemplateFile
    '
    ' Description:
    '
    ' History: 04/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractDocumentTemplateFile(ByVal iTableIndex As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef v_vParentResults(,) As Object, ByVal lParentResultIndex As Integer, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Integer, ByRef r_lTotalLinesWritten As Integer, ByRef r_oDatabase As dPMDAO.Database, ByVal v_iMode As Integer) As Integer

        Dim result As Integer = 0
        Dim iFileNumber As gPMConstants.PMEReturnCode
        Dim sPathName As String = ""
        Dim iColumnDocumentTypeId, iColumnDocumentTemplateId As Integer
        Dim sFileName, sSQLSearch As String
        Dim bExtractThisOne As Boolean

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractDocumentTemplateFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Document template file"
            objFrmMainForm.StatusBar_TextWrite("Document template file", 1)

            If Information.IsArray(v_vParentResults) Then
                'need to find the columns we are interested in

                iColumnDocumentTypeId = findColumn("document_type_id", r_aIeTableDefinitions(iTableIndex))

                iColumnDocumentTemplateId = findColumn("document_template_id", r_aIeTableDefinitions(iTableIndex))

                If iColumnDocumentTypeId <> -1 And iColumnDocumentTemplateId <> -1 Then

                    'AR 06/03/2003 check if this document is part of the 'PBDocs' type
                    If v_iMode And pbIeMode_PBDocsOnly Then
                        sSQLSearch = "SELECT document_type_id FROM document_type WHERE code like 'PBDocs'"
                        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Existing Data Check", bStoredProcedure:=False)

                        If r_oDatabase.Records.Count() <= 0 Then
                            'ExtractDocumentTemplateFile = PMNotFound
                            'writeToStatusBox "Error: Could not access 'PBDocs' Type"
                            Return result
                        End If

                        'temporarily comment out next line, Chris Branes told me to do it!
                        If v_vParentResults(iColumnDocumentTypeId, lParentResultIndex) = r_oDatabase.Records.Item(1).Fields()(0) Then
                            bExtractThisOne = True
                        End If
                    Else
                        bExtractThisOne = True
                    End If

                    If bExtractThisOne Then

                        'build up the document template file name from the document template data
                        'Type + document_type_id + '\doc ' + document_template_id + '.zip'
                        sFileName = "Type " & CStr(v_vParentResults(iColumnDocumentTypeId, lParentResultIndex))
                        sFileName = sFileName & "\doc " & CStr(v_vParentResults(iColumnDocumentTemplateId, lParentResultIndex))
                        sFileName = sFileName & ".zip"

                        objFrmMainForm.StatusBar1(2).Items.Item(0).Text = sFileName

                        iFileNumber = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", v_sSubKey:=conEmptyString, r_sSettingValue:=sPathName)
                        'module level document root folder setting
                        m_sDocPath = sPathName

                        sPathName = sPathName & AddRequiredBackslash(sPathName) & sFileName
                        m_lReturn = ExtractFile(iTableIndex:=pbIeDbt_DocumentTemplateFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sFilenameToRead:=sPathName, v_sFilenameToStore:=sFileName, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            writeToStatusBox("Could not access " & sPathName)
                        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            writeToStatusBox("Problems occurred while accessing " & sPathName)
                        End If
                    End If '
                Else
                    writeToStatusBox("Could not identify columns for document template file export")
                End If
            Else
                'no results found
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractDocumentTemplateFile")

            Return result

        Catch excep As System.Exception

            FileSystem.FileClose(iFileNumber)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractDocumentTemplateFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractDocumentTemplateFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractDocumentTemplateFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractUserDefinedLists
    '
    ' Description: Processes the extraction of the user defined lists
    '
    ' History:     09/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractUserDefinedLists(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim vResults(,) As Object
        Dim sSQL As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractUserDefinedLists")

            result = gPMConstants.PMEReturnCode.PMTrue

            'default progress bar
            objFrmMainForm.ProgressBar1(1).Value = 1

            'get a list of the user defined lists
            'TBD do we do this only for the lists that properties reference or for all of them

            sSQL = "select name from sysobjects where xtype='U' and name like '" & CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName)) & "%'"
            'get the user defined table names
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResults) Then

                For lResultIndex As Integer = 0 To vResults.GetUpperBound(1)

                    m_lReturn = CType(ExtractUserDefinedList(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, v_sTableName:=CStr(vResults(0, lResultIndex)), r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next lResultIndex
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractUserDefinedLists")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractUserDefinedLists")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractUserDefinedLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractUserDefinedLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractDerivedRatingRuleFile
    '
    ' Description: Processes the extraction of rating files derived from scheme data
    '
    ' History:     17/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractDerivedRatingRuleFile(ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByRef v_vResults(,) As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim vResults(,) As Object
        Dim sOriginalFileName, sNewFileName As String
        Dim iColumnIndexGisInsurerId, iColumnIndexSchemeNo, iColumnIndexVersion, iColumnSchemeDesc As Integer

        Dim iColumnSchemeId As Integer
        Dim sSQL As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractDerivedRatingRuleFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Scheme derived rating rule"
            objFrmMainForm.StatusBar_TextWrite("System derived rating rule", 1)
            objFrmMainForm.StatusBar_TextWrite(CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName)), 2)

            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(r_aIeControl(iTableIndex)(pbIeControl_objectName))

            If Information.IsArray(v_vResults) Then

                'get the original scheme filename using the sp

                iColumnSchemeId = findColumn("gis_Scheme_id", r_aIeTableDefinitions(iTableIndex))
                If iColumnSchemeId <> -1 Then
                    g_oDatabase.Parameters.Clear()

                    'sSQL = "{call spu_gis_get_rule_filename()}"
                    sSQL = "spu_gis_get_rule_filename"
                    bPMAddParameter.AddParameter(g_oDatabase, sSQL, m_lReturn, "Schemeid", v_vResults(iColumnSchemeId, lParentResultIndex), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ExtractDerivedRatingRuleFile get filename", bStoredProcedure:=True, vResultArray:=vResults)
                    If Information.IsArray(vResults) Then

                        sOriginalFileName = CStr(vResults(0, 0))
                    End If
                End If

                'need to find the columns we are interested in

                iColumnIndexGisInsurerId = findColumn("gis_insurer_id", r_aIeTableDefinitions(iTableIndex))

                iColumnIndexSchemeNo = findColumn("scheme_no", r_aIeTableDefinitions(iTableIndex))

                iColumnIndexVersion = findColumn("scheme_ver", r_aIeTableDefinitions(iTableIndex))

                iColumnSchemeDesc = findColumn("Scheme_Desc", r_aIeTableDefinitions(iTableIndex))

                If iColumnIndexGisInsurerId <> -1 And iColumnIndexSchemeNo <> -1 And iColumnIndexVersion <> -1 And iColumnSchemeDesc <> -1 And sOriginalFileName <> "" Then

                    'build up the new rating file name from the scheme data
                    'scheme_dataModelCode_<scheme_description>_<scheme_version>_<insurer_code>.rul
                    sNewFileName = "scheme_"
                    sNewFileName = sNewFileName & v_sDataModelCode & "_"

                    sNewFileName = sNewFileName & "[" & CStr(v_vResults(iColumnSchemeDesc, lParentResultIndex)) & "]_V"

                    sNewFileName = sNewFileName & CStr(v_vResults(iColumnIndexVersion, lParentResultIndex)) & "_"

                    g_oDatabase.SQLSelect(sSQL:="select code from gis_insurer where gis_insurer_id=" & CStr(v_vResults(iColumnIndexGisInsurerId, lParentResultIndex)), sSQLName:="ExtractDerivedRatingRuleFile get insurer code", bStoredProcedure:=False, vResultArray:=vResults)

                    If Information.IsArray(vResults) Then

                        sNewFileName = sNewFileName & CStr(vResults(0, 0)).TrimEnd()
                    Else
                        sNewFileName = sNewFileName & "?"
                    End If

                    sNewFileName = sNewFileName & ".rul"

                    m_lReturn = CType(ExtractRuleFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=sOriginalFileName, v_sNewFilename:=sNewFileName, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=0), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    writeToStatusBox("Could not identify scheme columns for scheme ID " + v_vResults(iColumnSchemeId, lParentResultIndex))
                End If
            Else
                'no results found
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractDerivedRatingRuleFile")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractDerivedRatingRuleFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractDerivedRatingRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractDerivedRatingRuleFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        findColumn
    '
    ' Description: Returns column number for required column name
    '
    ' History:     ??/??/???? ??? - Created.
    '
    ' ***************************************************************** '
    Public Function findColumn(ByVal v_sColumnName As Object, ByRef r_aIeTableDefinitions() As Object) As Integer

        Dim result As Integer = 0
        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".findColumn")

            For result = 0 To r_aIeTableDefinitions(pbIeTableDefinitions_columnArray).GetUpperBound(0)

                If CStr(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(result)(pbIeTableDefinitions_columnName)).ToLower() = CStr(v_sColumnName).ToLower() Then
                    Return result
                End If
            Next
            result = -1

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".findColumn")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".findColumn")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="findColumn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="findColumn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        GetSelfReferencingData
    '
    ' Description: Returns records from a table which references itself
    '
    ' History:     17/09/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Private Function GetSelfReferencingData(ByRef r_oDatabase As dPMDAO.Database, ByRef r_vResults(,) As Object, ByVal v_sTableName As String, ByVal v_sConstrainColumns As String, ByRef r_aIeControl As Object, ByRef r_aIeTableDefinitions As Object, ByVal v_iTableIndex As Integer, Optional ByVal v_lDataModelId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vRetrievedData(,) As Object
        Dim vResults(,) As Object
        Dim iTotalRecordCount, iRetrievedRecordCount, iColumnCount, iPKColumn, iParentIDColumn, iNewTotal, iRow As Integer
        Dim sPKNotIn As New StringBuilder
        Dim sParentIn As New StringBuilder



        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetSelfReferencingData")

        result = gPMConstants.PMEReturnCode.PMTrue

        'Assumes table has only 1 Primary key column, as Parent ID column is only likely to reference 1 primary key column.
        'Currently this procedure is only called for GIS Screen table which has only 1 Primary Key (gis_screen_Id)
        iPKColumn = CInt(r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns))

        iParentIDColumn = CInt(r_aIeControl(v_iTableIndex)(pbIeControl_ParentIdColumn))

        Do

            vRetrievedData = Nothing
            If v_lDataModelId <> 0 Then

                sSQL = "SELECT " & v_sConstrainColumns & " FROM " & v_sTableName & " WHERE " & CStr(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)((r_aIeControl(v_iTableIndex)(pbIeControl_DataModelIdColumn)) - 1)(pbIeTableDefinitions_columnName)) & " = " & CStr(v_lDataModelId)
                ' Data Model Id has been used now so set to 0 to force next
                ' loop through other half of conditional statement from now on.
                v_lDataModelId = 0
            Else
                If Information.IsArray(vResults) Then
                    'iPKColumn = gPMFunctions.ToSafeInteger(r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns))

                    ' If iPKColumn > vResults.GetUpperBound(0) Then
                    'iPKColumn = 1
                    'End If
                    'iParentIDColumn = CInt(r_aIeControl(v_iTableIndex)(pbIeControl_ParentIdColumn))
                    sParentIn = New StringBuilder("(")
                    sPKNotIn = New StringBuilder("(")
                    For iLoop As Integer = 0 To iTotalRecordCount - 1

                        sParentIn.Append(CStr(vResults(iPKColumn - 1, iLoop)) & ", ")

                        sPKNotIn.Append(CStr(vResults(iPKColumn - 1, iLoop)) & ", ")

                    Next
                    sParentIn = New StringBuilder(sParentIn.ToString().Substring(0, sParentIn.ToString().Length - 2) & ")")
                    sPKNotIn = New StringBuilder(sPKNotIn.ToString().Substring(0, sPKNotIn.ToString().Length - 2) & ")")

                    sSQL = "SELECT " & v_sConstrainColumns & " FROM " & v_sTableName & " WHERE " & CStr(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iParentIDColumn - 1)(pbIeTableDefinitions_columnName)) & " IN " & sParentIn.ToString() & _
                           " AND " & CStr(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iPKColumn - 1)(pbIeTableDefinitions_columnName)) & " NOT IN " & sPKNotIn.ToString()
                Else
                    sSQL = conEmptyString
                End If
            End If

            If sSQL <> conEmptyString Then 'just for debugging!
                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRetrievedData)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vRetrievedData) Then

                writeToDebugBox(sSQL & " (" & CStr(vRetrievedData.GetUpperBound(1) + 1) & " rows)")

                iRetrievedRecordCount = vRetrievedData.GetUpperBound(1)

                iColumnCount = vRetrievedData.GetUpperBound(0)
                iNewTotal = iRetrievedRecordCount + iTotalRecordCount
                ReDim Preserve vResults(iColumnCount, iNewTotal)
                For iRecordLoop As Integer = 0 To iRetrievedRecordCount
                    For iColLoop As Integer = 0 To iColumnCount
                        iRow = iTotalRecordCount + iRecordLoop

                        vResults(iColLoop, iRow) = vRetrievedData(iColLoop, iRecordLoop)
                    Next iColLoop
                Next iRecordLoop
                iTotalRecordCount = vResults.GetUpperBound(1) + 1
            End If
        Loop Until Not Information.IsArray(vRetrievedData)

        If iTotalRecordCount <> 0 Then
            r_vResults = vResults
        Else

            r_vResults = Nothing
        End If

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetSelfReferencingData")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractUserDefinedList
    '
    ' Description: Processes the extraction of the user defined lists
    '              Because some UDL are of a non standard column format we write out the column format
    '              Some UDLs are standard but for simplicity write the format of all of them
    '
    ' History:     09/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractUserDefinedList(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Integer, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByVal v_sTableName As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim vResults2(,) As Object
        Dim vDataArray(1, 0) As Object
        Dim vTableDetails(,) As Object
        Dim sTableDefinition As New StringBuilder
        Dim atemp() As Object
        Dim iDefinitionIndex As Integer

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractUserDefinedList")

            result = gPMConstants.PMEReturnCode.PMTrue

            sTableDefinition = New StringBuilder(conEmptyString)

            If v_sTableName <> "" And v_sTableName <> "<NULL>" Then

                'default progress bar
                objFrmMainForm.ProgressBar1(1).Value = 1

                'get a list of the user defined lists
                'TBD do we do this only for the lists that properties reference or for all of them
                objFrmMainForm.StatusBar1(2).Items.Item(0).Text = v_sTableName

                With r_oDatabase
                    With .Parameters
                        .Clear()

                        m_lReturn = .Add(sName:="tablename", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    End With
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = .SQLSelect(sSQL:=ACMSGetDMTableColumnsSQL, sSQLName:=ACMSGetDMTableColumnsName, bStoredProcedure:=ACMSGetDMTableColumnsStored, vResultArray:=vTableDetails)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'writeToStatusBox("Could not find table definition for user defined list: " & v_sTableName)
                            objFrmMainForm.txtWarning_TextWrite("Could not find table definition for user defined list: " & v_sTableName, 1)
                            Return result
                        End If
                    End If
                End With
            End If
            If Information.IsArray(vTableDetails) Then
                Erase atemp

                For iLoop As Integer = 0 To vTableDetails.GetUpperBound(1)

                    sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnName, iLoop)) & conCommaSpace)

                    sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnType, iLoop)) & conCommaSpace)

                    sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnSize, iLoop)) & conCommaSpace)

                    If CStr(vTableDetails(sp_msHelpColumns_columnPrecision, iLoop)) = conEmptyString Then
                        sTableDefinition.Append("NULL, ")
                    Else

                        sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnPrecision, iLoop)) & conCommaSpace)
                    End If

                    If CStr(vTableDetails(sp_msHelpColumns_columnScale, iLoop)) = conEmptyString Then
                        sTableDefinition.Append("NULL, ")
                    Else

                        sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnScale, iLoop)) & conCommaSpace)
                    End If

                    sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnNull, iLoop)) & conCommaSpace)

                    sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnIdentity, iLoop)) & conCommaSpace)

                    sTableDefinition.Append(CStr(vTableDetails(sp_msHelpColumns_columnFlags, iLoop)) & conCommaSpace)

                    addToArray(atemp, New Object() {vTableDetails(sp_msHelpColumns_columnName, iLoop), vTableDetails(sp_msHelpColumns_columnType, iLoop), vTableDetails(sp_msHelpColumns_columnSize, iLoop)})
                Next iLoop

                ' Knock off last comma from string table definition
                sTableDefinition = New StringBuilder(sTableDefinition.ToString().Substring(0, sTableDefinition.ToString().Length - conCommaSpace.Length))

                addToArray(r_aIeTableDefinitions, New Object() {v_sTableName, vTableDetails.GetUpperBound(1) + 1, atemp})

                r_aIeTableDefinitions(pbIeDbt_UserDefinedList).SetValue(vTableDetails.GetUpperBound(1) + 1, pbIeTableDefinitions_columnCount)

                iDefinitionIndex = r_aIeTableDefinitions.GetUpperBound(0)

                '        addToArray r_aIeControl, Array(iDefinitionIndex, v_sTableName, pbIeOt_UserDefinedList, 0, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Using new data definition array, try again to read in data.

                    m_lReturn = CType(ReadRecordsFromDatabase(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=pbIeDbt_UserDefinedList, v_sObjectName:=v_sTableName, v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, r_vResults:=vResults2, v_sConstrainColumns:="*"), gPMConstants.PMEReturnCode)
                End If
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Because there are so many entries for a UDL we write the name of the UDL as
                'header record before the actual entries

                'this used only to be called if there were some entries in the table
                'no we write the UDL entries into pmproduct_lookup we need to create UDL tables
                'even if they are empty

                vDataArray(0, 0) = v_sTableName

                vDataArray(1, 0) = sTableDefinition.ToString()

                m_lReturn = CType(WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=pbIeDbt_UserDefinedListHeader, i_aDataDefinition:=r_aIeTableDefinitions(pbIeDbt_UserDefinedListHeader)(pbIeTableDefinitions_columnArray), i_aData:=vDataArray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResults2) Then

                    For lResultIndex2 As Integer = 0 To vResults2.GetUpperBound(1)

                        'keep progress and status bar current
                        If lResultIndex2 Mod 100 = 0 Then

                            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = v_sTableName & " (" & CStr(lResultIndex2) & " of " & CStr(vResults2.GetUpperBound(1)) & ")"

                            objFrmMainForm.ProgressBar1(1).Value = 100 / ((vResults2.GetUpperBound(1) + 1) / (lResultIndex2 + 1))
                            Application.DoEvents()
                        End If

                        'Write the row passing an array containing the actual data and an
                        'array containing the description of the data in array 1.  Also
                        'pass the file number of the current extract file
                        'add type here!!!!

                        m_lReturn = CType(WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(r_aIeControl(pbIeDbt_UserDefinedList)(0)), i_aDataDefinition:=r_aIeTableDefinitions(iDefinitionIndex)(pbIeTableDefinitions_columnArray), i_aData:=vResults2, rowIndex:=lResultIndex2, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Next lResultIndex2
                Else
                    'no results found
                End If
            Else
                'read returned pmfalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractUserDefinedList")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractUserDefinedList")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractUserDefinedList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractUserDefinedList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractPropertyDerivedLookups
    '
    ' Description: Processes the extraction of the rule files derived from screen codes
    '
    ' History:     09/06/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractPropertyDerivedLookups(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iParentIndex As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef v_vParentResults(,) As Object, ByVal lParentResultIndex As Integer, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Integer, ByVal v_iMode As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim result As Integer = 0
        Dim iSpecialsType, iSpecialsTypeReference, iPmLoopup As Integer
        Dim sTableName As String = ""
        Dim iPropertyName As Integer

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractPropertyDerivedLookups")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract

            If Information.IsArray(v_vParentResults) Then
                'need to find the columns we are interested in

                iSpecialsType = findColumn("specials_type", r_aIeTableDefinitions(v_iParentIndex))

                iSpecialsTypeReference = findColumn("specials_type_reference", r_aIeTableDefinitions(v_iParentIndex))

                iPmLoopup = findColumn("pmlookup_table_name", r_aIeTableDefinitions(v_iParentIndex))

                iPropertyName = findColumn("property_name", r_aIeTableDefinitions(v_iParentIndex))

                sTableName = conEmptyString
                If iSpecialsType <> -1 Then

                    If (v_vParentResults(iSpecialsType, lParentResultIndex)) = GISSharedPropertyConstants.ACOPMLookupTableName Then

                        sTableName = CStr(v_vParentResults(iSpecialsTypeReference, lParentResultIndex)).Trim()
                        If sTableName.IndexOf(" "c) >= 0 Then

                            writeToStatusBox("Property (" & CStr(v_vParentResults(iPropertyName, lParentResultIndex)).Trim() & ") references invalid table name (" & sTableName & "). This would cause a fatal import error.")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = CType(CheckFileNameNotAlreadyBeingExported(v_iMode:=v_iMode, r_sTableName:=sTableName, r_aIeTableDefinitions:=r_aIeTableDefinitions), gPMConstants.PMEReturnCode)
                    End If
                End If

                If iPmLoopup <> -1 Then

                    sTableName = CStr(v_vParentResults(iPmLoopup, lParentResultIndex)).Trim()
                    m_lReturn = CType(CheckFileNameNotAlreadyBeingExported(v_iMode:=v_iMode, r_sTableName:=sTableName, r_aIeTableDefinitions:=r_aIeTableDefinitions), gPMConstants.PMEReturnCode)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Added claim_version_status exception which is not an actual lookup table
                If sTableName <> conEmptyString And sTableName <> "claim_version_status" Then
                    'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Derived lookup"
                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
                    objFrmMainForm.StatusBar_TextWrite("claim_version_status", 1)
                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

                    m_lReturn = CType(ExtractUserDefinedList(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=v_iParentIndex, v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, v_sTableName:=sTableName, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractPropertyDerivedLookups")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractPropertyDerivedLookups")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractPropertyDerivedLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractPropertyDerivedLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: checkFileNameNotAlreadyBeingExported
    '
    ' Description: Checks if a table name will be exported as a normal table.
    '              If it is found the name is cleared
    '              TBD: should check the mode we are running in
    '
    ' History: 26/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function CheckFileNameNotAlreadyBeingExported(ByVal v_iMode As Integer, ByRef r_sTableName As String, ByRef r_aIeTableDefinitions() As Object) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".CheckFileNameNotAlreadyBeingExported")



        result = gPMConstants.PMEReturnCode.PMTrue

        If r_sTableName = conEmptyString Then Return result

        If r_sTableName.ToLower().IndexOf("udl_") >= 0 Then
            r_sTableName = conEmptyString
            Return result
        End If

        For iTableNameIndex As Integer = 0 To r_aIeTableDefinitions.GetUpperBound(0)

            If CStr(r_aIeTableDefinitions(iTableNameIndex)(0)).ToLower() = r_sTableName.ToLower() Then
                r_sTableName = conEmptyString
                Return result
            End If
        Next

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".CheckFileNameNotAlreadyBeingExported")

        Return result

    End Function

    Function ExportUMLScript(ByRef r_oDatabase As dPMDAO.Database) As gPMConstants.PMEReturnCode
        Dim aoResult(,) As Object = Nothing
        Dim sb As StringBuilder
        Dim sbHeaderInfo As StringBuilder
        Dim nMaxNumberofUMLDetailIds As Integer
        Try
            sbHeaderInfo = New StringBuilder

            'FIRST GET THE HEADER INFORMATION
            r_oDatabase.Parameters.Clear()
            If r_oDatabase.SQLSelect(sSQL:=kGenerateUMLHeadersSQL, _
                                        sSQLName:=kGenerateUMLHeadersName, _
                                        bStoredProcedure:=kGenerateUMLHeadersStored, _
                                        lNumberRecords:=gPMConstants.PMAllRecords, _
                                        vResultArray:=aoResult) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Get Valid UML codes")
            End If

            'WRITE ALL HEADER INFORMATION TO FILE
            If aoResult IsNot Nothing Then
                objFrmMainForm.StatusBar_TextWrite("Creating  UMLHeaders.sql", 1)
                sb = New StringBuilder
                For nIndex As Integer = 0 To UBound(aoResult, 2)
                    sb.AppendLine(aoResult(0, nIndex).ToString)
                Next

                WriteUMLScript(sb, "UMLHeaders.sql", sbHeaderInfo)

                sb = Nothing
            End If

            'GET THE MAX NUMBER OF ROWS IN USER DEF DETAILS 
            r_oDatabase.Parameters.Clear()
            If r_oDatabase.SQLSelect(sSQL:=kGetMaxNumberInUMLDetailsSQL, _
                                        sSQLName:=kGetMaxNumberInUMLDetailsName, _
                                        bStoredProcedure:=kGetMaxNumberInUMLDetailsStored, _
                                        lNumberRecords:=gPMConstants.PMAllRecords, _
                                        vResultArray:=aoResult) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to " & kGetMaxNumberInUMLDetailsName)
            End If

            If aoResult IsNot Nothing Then
                nMaxNumberofUMLDetailIds = ToSafeInteger(aoResult(0, 0))
            End If

            If nMaxNumberofUMLDetailIds > 0 Then
                For nStartId As Integer = 1 To nMaxNumberofUMLDetailIds
                    r_oDatabase.Parameters.Clear()
                    If r_oDatabase.Parameters.Add(sName:="nFromGisUserDefId", vValue:=nStartId, _
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                               iDataType:=gPMConstants.PMEDataType.PMLong) <> PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("Failed to Add Parameter for " & kGenerateUMLDetailsSQL)
                    End If

                    If r_oDatabase.Parameters.Add(sName:="nToGisUserDefId", vValue:=nStartId + kMaxUMLDetailsRecordsToFetch, _
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                               iDataType:=gPMConstants.PMEDataType.PMLong) <> PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("Failed to Add Parameter for " & kGenerateUMLDetailsSQL)
                    End If

                    If r_oDatabase.SQLSelect(sSQL:=kGenerateUMLDetailsSQL, _
                                            sSQLName:=kGenerateUMLDetailsName, _
                                            bStoredProcedure:=kGenerateUMLDetailsStored, _
                                            lNumberRecords:=gPMConstants.PMAllRecords, _
                                            vResultArray:=aoResult) <> PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("Failed to Get UML Script")
                    End If


                    'WRITE THIS DETAIL INFORMATION TO FILE
                    If aoResult IsNot Nothing Then
                        objFrmMainForm.StatusBar_TextWrite("Creating UMLDetails" & nStartId & ".sql", 1)

                        sb = New StringBuilder
                        For nIndex As Integer = 0 To UBound(aoResult, 2)
                            sb.AppendLine(aoResult(0, nIndex).ToString)
                        Next

                        WriteUMLScript(sb, "UMLDetails" & nStartId & ".sql", sbHeaderInfo)
                        sb = Nothing
                    End If

                    nStartId += kMaxUMLDetailsRecordsToFetch
                Next
            End If
            objFrmMainForm.StatusBar_TextWrite("Updating & " & kUMLHeaderInfo, 1)
            WriteUMLScript(sbHeaderInfo, kUMLHeaderInfo)
            Return PMEReturnCode.PMTrue
        Catch excep As ApplicationException
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed to Get UML Script", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="GetUMLScript",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return PMEReturnCode.PMError
        Finally
            sbHeaderInfo = Nothing
        End Try
    End Function


    Function ExecuteScript(ByRef r_oDatabase As dPMDAO.Database, ByVal sExecuteScriptSQL As String) As PMEReturnCode
        Try

            r_oDatabase.Parameters.Clear()
            If r_oDatabase.SQLAction(sSQL:=sExecuteScriptSQL, _
                                        sSQLName:="ExecuteScript", _
                                        bStoredProcedure:=False) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException
            End If

            Return PMEReturnCode.PMTrue
        Catch excep As ApplicationException
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                              sMsg:="Failed to Execute Script", vApp:=ACApp,
                              vClass:=ACClass, vMethod:="ExecuteScript",
                              vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return PMEReturnCode.PMError
        End Try

    End Function

    Function ImportUMLScript(ByRef r_oDatabase As dPMDAO.Database) As PMEReturnCode
        Dim sAbsolutePath As String
        Dim srHeaderInfo As System.IO.StreamReader = Nothing
        Dim sr As System.IO.StreamReader = Nothing
        Dim sFileName As String
        Dim sExecuteScriptSQL As String
        Dim cFileNameCol As New Collection

        sAbsolutePath = GetUMLDirectoryPath() & "\" & kUMLHeaderInfo
        If File.Exists(sAbsolutePath) Then
            Try
                srHeaderInfo = New StreamReader(sAbsolutePath)
                While Not srHeaderInfo.EndOfStream
                    cFileNameCol.Add(srHeaderInfo.ReadLine)
                End While

                objFrmMainForm.ProgressBar1(0).Visible = True
                objFrmMainForm.ProgressBar1(0).Maximum = cFileNameCol.Count

                For iItemIndex As Integer = 1 To cFileNameCol.Count
                    sFileName = cFileNameCol.Item(iItemIndex).ToString
                    If Not String.IsNullOrEmpty(sFileName) Then
                        objFrmMainForm.StatusBar_TextWrite("Running " & sFileName, 1)
                        sAbsolutePath = GetUMLDirectoryPath() & "\" & sFileName

                        sr = New StreamReader(sAbsolutePath)
                        sExecuteScriptSQL = sr.ReadToEnd()
                        r_oDatabase.SQLBeginTrans()

                        If ExecuteScript(r_oDatabase, sExecuteScriptSQL) = PMEReturnCode.PMTrue Then
                            r_oDatabase.SQLCommitTrans()
                        Else
                            r_oDatabase.SQLRollbackTrans()
                            writeToStatusBox("Failed to Run UML Script " & sAbsolutePath)
                            Return PMEReturnCode.PMFail
                        End If
                        objFrmMainForm.ProgressBar1(0).Value = iItemIndex
                        objFrmMainForm.StatusBar_TextWrite("Imported: " & iItemIndex & " Remaining: " & cFileNameCol.Count - iItemIndex, 0)
                    End If
                Next
                ' End While
                Return PMEReturnCode.PMTrue
            Catch ex As Exception
                writeToStatusBox("Failed to Run UML Script " & sAbsolutePath)
                Return PMEReturnCode.PMError
            Finally
                sr.Close()
                sr.Dispose()
                srHeaderInfo.Close()
                srHeaderInfo.Dispose()
                cFileNameCol = Nothing
            End Try
        Else
            Return PMEReturnCode.PMTrue
        End If
    End Function

    Function WriteUMLScript(ByVal sData As StringBuilder, ByVal sFileName As String, _
                            Optional ByRef sbHeaderInfo As StringBuilder = Nothing) As PMEReturnCode
        Dim sAbsolutePath As String
        Dim sPath As String
        Dim sw As System.IO.StreamWriter = Nothing
        Try
            sPath = GetUMLDirectoryPath()

            If Not Directory.Exists(sPath) Then
                Directory.CreateDirectory(sPath)
            End If
            sAbsolutePath = sPath & "\" & sFileName
            sw = New System.IO.StreamWriter(File.Open(sAbsolutePath, FileMode.CreateNew))

            sw.WriteLine(sData.ToString)
            If sbHeaderInfo IsNot Nothing Then
                sbHeaderInfo.AppendLine(sFileName)
            End If

            Return PMEReturnCode.PMTrue
        Catch excep As ApplicationException
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                             sMsg:="Failed to Get UML Script", vApp:=ACApp,
                             vClass:=ACClass, vMethod:="GetUMLScript",
                             vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return PMEReturnCode.PMError
        Finally
            sw.Close()
            sw.Dispose()
        End Try

    End Function


End Module
