Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Imports System.IO
Imports System.IO.Compression
Imports Ionic.Zip
Imports System.Text
Imports System.Collections.Generic

Module pbImportExportTables

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer
    Private m_sDocPath As String

    Const kValidUMLHeaderID As Integer = 0
    Const kValidUMLCode As Integer = 1
    Const kValidUMLParentID As Integer = 2
    Const kValidUMLIsExported As Integer = 3
    ' ***************************************************************** '
    '
    ' Name:        ExtractFixedTables
    '
    ' Description: Processes the extraction of fixed tables to the binary
    '              file.  Recieves the current control file array row and
    '              the file number of hte file being processed
    '
    ' History:     26/06/2010 JB - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractFixedTables(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer, ByVal v_iExportMode As Object) As Integer

        'Define the counters to traverse the array sucture
        Dim lResultIndex As Integer
        Dim vResults As Object
        Dim iChildIndex As Short
        Dim sStatusText As String
        Try

            ' Debug message
            'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ExtractFixedTables"

            ExtractFixedTables = gPMConstants.PMEReturnCode.PMTrue


            'JB Nov 2009 Exclude PMUser_Authority_Level table as it is linked with users which can not be known at the time of migration
            ' and include authority_level_type as it is required for linking rules with authority level type
            If objFrmMainForm.chkAdditionalExportOptions(7).CheckState <> 1 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeControl(iTableIndex)(pbIeControl_objectName) = "authority_level_type" Or r_aIeControl(iTableIndex)(pbIeControl_objectName) = "PMUser_Authority_Rule_Set_Link" Or r_aIeControl(iTableIndex)(pbIeControl_objectName) = "Rule_Set" Then
                    ExtractFixedTables = gPMConstants.PMEReturnCode.PMTrue
                    Exit Function
                End If
            End If

            If UCase(Left(r_aIeControl(iTableIndex)(pbIeControl_objectName), 12)) = "GIS_USER_DEF" AndAlso objFrmMainForm.chkIncludeUMLs.Checked Then
                ExtractFixedTables = gPMConstants.PMEReturnCode.PMTrue
                Exit Function
            End If

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Database table"
            objFrmMainForm.StatusBar_TextWrite("Database table", 1)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = r_aIeControl(iTableIndex)(pbIeControl_objectName)
            objFrmMainForm.StatusBar_TextWrite(r_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
            objFrmMainForm.ProgressBar1(1).Visible = True
            objFrmMainForm.ProgressBar1(1).Value = 1

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If ReadRecordsFromDatabase(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iTableIndex, v_sObjectName:=r_aIeControl(iTableIndex)(pbIeControl_objectName), v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, r_vResults:=vResults, v_sConstrainColumns:="*", v_sStatusText:=sStatusText) = gPMConstants.PMEReturnCode.PMTrue Then

                'ensure GUI is updated JAIDEEP MONSTER
                ' Too much!  objFrmMainForm.StatusBar1(2).Panels(1).Text = sStatusText :  DoEvents
                If IsArray(vResults) Then

                    For lResultIndex = 0 To UBound(vResults, 2)

                        objFrmMainForm.ProgressBar1(1).Value = 100 / ((UBound(vResults, 2) + 1) / (lResultIndex + 1))
                        If lResultIndex Mod 100 = 0 Then
                            If UBound(vResults, 2) > 1000 Or lResultIndex = 0 Then
                                If lResultIndex > 0 Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sStatusText & " (" & lResultIndex & " of " & UBound(vResults, 2) & ")"
                                    objFrmMainForm.StatusBar_TextWrite(sStatusText & " (" & lResultIndex & " of " & UBound(vResults, 2) & ")", 2)
                                Else
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sStatusText
                                    objFrmMainForm.StatusBar_TextWrite(sStatusText, 2)
                                End If
                            End If
                            System.Windows.Forms.Application.DoEvents()
                        End If

                        'Write the row passing an array containing the actual data and an
                        'array containing the description of the data in array 1.  Also
                        'pass the file number of the current extract file
                        'add type here!!!!
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=r_aIeControl(iTableIndex)(0), i_aDataDefinition:=r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=vResults, rowIndex:=lResultIndex, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ExtractFixedTables = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_HasFilename). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If r_aIeControl(iTableIndex)(pbIeControl_HasFilename) <> 0 Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If (vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex) <> conEmptyString And vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex) <> "<NULL>") Then 'Or |                        'r_aIeControl(iTableIndex)(pbIeControl_objectName) = "gis_scheme" Then
                                'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "rule file"
                                objFrmMainForm.StatusBar_TextWrite("rule file", 1)
                                'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex)
                                objFrmMainForm.StatusBar_TextWrite(vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex), 2)
                                m_lReturn = ExtractRuleFile(iTableIndex:=iTableIndex, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex), v_sNewFilename:=vResults(r_aIeControl(iTableIndex)(pbIeControl_HasFilename) - 1, lResultIndex), v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=vResults(0, lResultIndex))
                            End If
                        End If
                    Next lResultIndex

                    For lResultIndex = 0 To UBound(vResults, 2)

                        If g_bStopProcessing Then
                            Exit Function
                        Else
                            'now we need to look down the whole control array to see if we have any childern of this object
                            'we only need to go 1 level deep (now) as we are checking the whole of the array so check if
                            'v_vParentResults is not null and if it is don't go any further
                            For iChildIndex = 0 To UBound(g_aIeControl)
                                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iChildIndex)(pbIeControl_RelatedObjectId). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                If r_aIeControl(iChildIndex)(pbIeControl_RelatedObjectId) = iTableIndex And r_aIeControl(iChildIndex)(pbIeControl_operationMode) And v_iExportMode Then
                                    If iChildIndex = 71 Then
                                        Debug.Print("Found doc template")
                                    End If
                                    Select Case r_aIeControl(iChildIndex)(pbIeControl_objectType)
                                        Case pbIeOt_dbTable_child
                                            m_lReturn = ExtractFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iChildIndex, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_iExportMode:=v_iExportMode)
                                            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sStatusText
                                            objFrmMainForm.StatusBar_TextWrite(sStatusText, 2)
                                            System.Windows.Forms.Application.DoEvents()
                                            'WE NEED TO CATER RULES DIFFERENTLY FOR PURE
                                            'Case pbIeOt_DerivedRatingRuleFile
                                            '    If v_iMode And pbIeMode_RuleFiles Then
                                            '        m_lReturn = ExtractDerivedRatingRuleFile(v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vResults:=vResults, lParentResultIndex:=lResultIndex, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                            '    End If
                                        Case pbIeOt_DocumentTemplate 'only do once
                                            If v_iMode And pbIeMode_Documents And g_bDocumentTemplateExtracted = False Then
                                                m_lReturn = ExtractDocumentTemplateFile(iTableIndex:=iTableIndex, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, r_oDatabase:=r_oDatabase, v_iMode:=v_iMode)
                                                'g_bDocumentTemplateExtracted = True
                                            End If
                                        Case pbIeOt_DerivedDefAndValRuleFile
                                            If v_iMode And pbIeMode_RuleFiles Then
                                                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                                'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Derived rule file"
                                                objFrmMainForm.StatusBar_TextWrite("Derived rule file", 1)
                                                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                                'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                                objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                                m_lReturn = ExtractDerivedRuleFiles(v_iParentIndex:=iTableIndex, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                            End If
                                        Case pbIeOt_GPLookup 'only do once
                                            If g_bPropertyDerivedLookupExtracted = False Then

                                                m_lReturn = ExtractPropertyDerivedLookups(r_oDatabase:=r_oDatabase, v_iParentIndex:=iTableIndex, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_vParentResults:=vResults, lParentResultIndex:=lResultIndex, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                                g_bPropertyDerivedLookupExtracted = True
                                            End If
                                        Case Else
                                    End Select

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ExtractFixedTables = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If

                                End If
                            Next iChildIndex
                        End If
                    Next lResultIndex
                End If
            Else
                'read returned pmfalse
            End If

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ExtractFixedTables"

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractFixedTables")

            ExtractFixedTables = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractFixedTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractFixedTables", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function


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
    Public Function ExtractDerivedRuleFiles(ByVal v_iParentIndex As Short, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Short, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim iColumnDocumentCode As Short
        Dim sFilename As String

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractDerivedRuleFiles")

            ExtractDerivedRuleFiles = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Screen derived file extract"
            objFrmMainForm.StatusBar_TextWrite("Screen derived file extract", 1)
            If IsArray(v_vParentResults) Then
                'need to find the columns we are interested in

                iColumnDocumentCode = findColumn("code", r_aIeTableDefinitions(v_iParentIndex))

                If iColumnDocumentCode <> -1 Then

                    'build up the document template file name from the document template data
                    'Type + document_type_id + '\doc ' + document_template_id + '.zip'

                    'create default file name and extract
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sFilename = Trim(v_vParentResults(iColumnDocumentCode, lParentResultIndex)) & "def.rul"
                    'Set the panel to indicate the  type of extract
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sFilename
                    objFrmMainForm.StatusBar_TextWrite(sFilename, 2)
                    m_lReturn = ExtractRuleFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=sFilename, v_sNewFilename:=sFilename, v_sDataModelName:=v_sDataModelName, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ExtractDerivedRuleFiles = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    'create validation file name and extract
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sFilename = Trim(v_vParentResults(iColumnDocumentCode, lParentResultIndex)) & "val.rul"
                    'Set the panel to indicate the  type of extract
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sFilename
                    objFrmMainForm.StatusBar_TextWrite(sFilename, 2)
                    m_lReturn = ExtractRuleFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=sFilename, v_sNewFilename:=sFilename, v_sDataModelName:=v_sDataModelName, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ExtractDerivedRuleFiles = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                Else
                    writeToStatusBox(("Could not identify screen column for derived file export"))
                End If
            Else
                'no results found
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractDerivedRuleFiles")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractDerivedRuleFiles")

            ExtractDerivedRuleFiles = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractDerivedRuleFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractDerivedRuleFiles", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function


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
    Public Function ExtractRuleFile(ByVal iTableIndex As Short, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_sOriginalFilename As String, ByVal v_sNewFilename As String, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Short, ByRef r_lTotalLinesWritten As Integer, ByVal v_lSchemeId As Integer) As Integer

        Dim iFileNumber As Short
        Dim sPathName As String

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractRuleFile")

            ExtractRuleFile = gPMConstants.PMEReturnCode.PMTrue


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

            iFileNumber = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName)

            sPathName = sPathName & AddRequiredBackslash(sPathName) & v_sOriginalFilename

            m_lReturn = ExtractFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sFilenameToRead:=sPathName, v_sFilenameToStore:=v_sNewFilename, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'writeToStatusBox "Error : Could not extract " & v_sOriginalFilename
                'to much moaning about missing non mandatory file
                ExtractRuleFile = gPMConstants.PMEReturnCode.PMTrue 'don't stop the export
                Exit Function
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractRuleFile")

            Exit Function
        Catch ex As Exception

            FileClose(iFileNumber)

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractRuleFile")

            ExtractRuleFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractRuleFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function ReadRecordsFromDatabase(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Short, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByVal v_sObjectName As String, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_vResults(,) As Object, ByVal v_sConstrainColumns As String, Optional ByRef v_sStatusText As String = "") As Integer



        Dim sSQLSelect As String
        Dim sSQLWhere As String
        Dim iResultIndex As Short
        Dim sStatusTextWhereClause As String

        Dim sCustomWhereClause As String 'Richard Clarke November 2008 - PIE enhancements
        Dim sGISListsWhere As String
        Dim iRecord As Short


        Try


            ' Debug message
            'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ReadRecordsFromDatabase"

            ReadRecordsFromDatabase = gPMConstants.PMEReturnCode.PMTrue


            'determine what we need to use on the Where clause
            'these are in order of preference so do not change!

            '1) child
            '2) data model code
            '3) data model id
            '4) any additional specified where clause

            v_sStatusText = v_sObjectName

            ''If Mode is Not Data Migration And we have Migration Tables -Ignore them
            If g_lExportMode <> pbIeMode_Migration AndAlso r_aIeControl(iTableIndex)(pbIeControl_operationMode) = pbIeMode_Migration Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Dim sParentKeyColumn As String
            Dim iColumnIndex As Object
            Dim iPrimaryKeySearch As Object
            If r_aIeControl(iTableIndex)(pbIeControl_objectType) = pbIeOt_dbTable_child Then

                'UPGRADE_WARNING: Couldn't resolve default property of object iPrimaryKeySearch. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iPrimaryKeySearch = -1

                'get the primary key column from the parent
                'start at 0 and search till (2)
                sSQLSelect = conEmptyString
                sSQLWhere = conEmptyString

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId))(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId))(pbIeTableDefinitions_columnCount) - 1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object iPrimaryKeySearch. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Do While iPrimaryKeySearch < r_aIeTableDefinitions(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId))(pbIeTableDefinitions_columnCount) - 1 'And sSQL = conEmptyString

                    'UPGRADE_WARNING: Couldn't resolve default property of object iPrimaryKeySearch. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    iPrimaryKeySearch = iPrimaryKeySearch + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sParentKeyColumn = r_aIeTableDefinitions(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId))(2)(iPrimaryKeySearch)(0)

                    'find this column in the child

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    For iColumnIndex = 0 To r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnCount) - 1

                        ' BSJ Sep 2009 - Exit for new columns
                        If LCase(sParentKeyColumn) = g_PIEGuidCol Or LCase(sParentKeyColumn) = g_PIELastUpdatedCol Then

                            Exit For
                        End If

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(iColumnIndex)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If sParentKeyColumn = r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(iColumnIndex)(0) Then

                            sSQLSelect = "SELECT * FROM " & v_sObjectName

                            'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sSQLWhere = "WHERE " & sParentKeyColumn & " = " & v_vParentResults(iPrimaryKeySearch, lParentResultIndex)

                            'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sStatusTextWhereClause = " (" & sParentKeyColumn & " = " & v_vParentResults(iPrimaryKeySearch, lParentResultIndex) & ")"

                            'if match on the first column on the parent get out now

                            'UPGRADE_WARNING: Couldn't resolve default property of object iPrimaryKeySearch. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If iPrimaryKeySearch = 0 Then

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                'UPGRADE_WARNING: Couldn't resolve default property of object iPrimaryKeySearch. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                iPrimaryKeySearch = r_aIeTableDefinitions(r_aIeControl(iTableIndex)(pbIeControl_RelatedObjectId))(pbIeTableDefinitions_columnCount) - 1

                                Exit For

                            End If

                            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(0)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If sParentKeyColumn = r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(0)(0) Then

                                Exit For
                            End If
                        End If
                    Next
                Loop

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_DataModelCodeColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf r_aIeControl(iTableIndex)(pbIeControl_DataModelCodeColumn) <> 0 Then

                sSQLSelect = "SELECT " & v_sConstrainColumns & " FROM " & v_sObjectName
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_DataModelCodeColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQLWhere = "WHERE " & r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(r_aIeControl(iTableIndex)(pbIeControl_DataModelCodeColumn) - 1)(pbIeTableDefinitions_columnName) & " = '" & v_sDataModelCode & "'"

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_DataModelIdColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ElseIf r_aIeControl(iTableIndex)(pbIeControl_DataModelIdColumn) <> 0 Then

                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectId). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeControl(iTableIndex)(pbIeControl_objectId) = pbIeDbt_gis_screen Then

                    m_lReturn = GetSelfReferencingData(r_oDatabase:=r_oDatabase, r_vResults:=r_vResults, v_sTableName:=v_sObjectName, v_sConstrainColumns:=v_sConstrainColumns, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_iTableIndex:=iTableIndex, v_lDataModelID:=v_lDataModelID)

                    sSQLSelect = conEmptyString 'pdone processing



                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ReadRecordsFromDatabase = gPMConstants.PMEReturnCode.PMFalse

                        Exit Function
                    End If
                Else

                    sSQLSelect = "SELECT " & v_sConstrainColumns & " FROM " & v_sObjectName
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_DataModelIdColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQLWhere = "WHERE  " & r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnArray)(r_aIeControl(iTableIndex)(pbIeControl_DataModelIdColumn) - 1)(pbIeTableDefinitions_columnName) & " = " & v_lDataModelID
                End If
            Else
                sSQLSelect = "SELECT " & v_sConstrainColumns & " FROM " & v_sObjectName
            End If



            'create status bar text
            v_sStatusText = v_sStatusText & sStatusTextWhereClause



            'append derived where clause
            If sSQLSelect <> "" AndAlso sSQLWhere <> "" Then

                sSQLSelect = sSQLSelect & " " & sSQLWhere
            End If



            'append and pre defined where clause
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_WhereClause). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeControl(iTableIndex)(pbIeControl_WhereClause) <> conEmptyString Then

                If sSQLWhere <> conEmptyString Then

                    sSQLSelect = sSQLSelect & " and"
                End If
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQLSelect = sSQLSelect & " " & r_aIeControl(iTableIndex)(pbIeControl_WhereClause)
            End If



            'Richard Clarke November 2008 - PIE enhancements
            'we need to use special where clause for gis_list_items and gis_list_type_usage
            'as we only want the ones the user has chosen

            If v_sObjectName = "gis_list_items" Then

                If UBound(s_aUDLSelected) >= 0 Then

                    sGISListsWhere = sGISListsWhere & " code='" & s_aUDLSelected(0) & "'"

                    For iRecord = 1 To UBound(s_aUDLSelected)

                        sGISListsWhere = sGISListsWhere & " OR code = '" & s_aUDLSelected(iRecord) & "'"
                    Next iRecord
                End If

                sCustomWhereClause = " WHERE "
                sCustomWhereClause = sCustomWhereClause & " gis_list_items_id IN (SELECT gis_list_items_id From GIS_List_Type_Usage Where"
                sCustomWhereClause = sCustomWhereClause & " gis_list_type_id IN(SELECT gis_list_type_id From GIS_List_Type Where "
                sCustomWhereClause = sCustomWhereClause & sGISListsWhere & "))"
                sSQLSelect = sSQLSelect & sCustomWhereClause 'append to the original query
            End If



            If v_sObjectName = "gis_list_type_usage" Then

                If UBound(s_aUDLSelected) >= 0 Then

                    sGISListsWhere = sGISListsWhere & " code='" & s_aUDLSelected(0) & "'"

                    For iRecord = 1 To UBound(s_aUDLSelected)

                        sGISListsWhere = sGISListsWhere & " OR code = '" & s_aUDLSelected(iRecord) & "'"
                    Next iRecord
                End If

                sCustomWhereClause = " WHERE "
                sCustomWhereClause = sCustomWhereClause & " gis_list_type_id IN (SELECT gis_list_type_id From GIS_List_Type Where "
                sCustomWhereClause = sCustomWhereClause & sGISListsWhere & ")"
                sSQLSelect = sSQLSelect & sCustomWhereClause 'append to the original query
            End If

            'Richard Clarke November 2008 - PIE enhancements - END

            'JB Aug 10
            If v_sObjectName = "peril_type" Then
                sCustomWhereClause = " WHERE "
                sCustomWhereClause = sCustomWhereClause & " (gis_screen_id IS NULL OR gis_screen_id IN (SELECT gis_screen_id From gis_screen Where"
                sCustomWhereClause = sCustomWhereClause & " gis_data_model_id IN(SELECT gis_data_model_id From gis_data_model Where code = "
                sCustomWhereClause = sCustomWhereClause & "'" & v_sDataModelCode & "'" & "))) ORDER BY peril_type_id"
                sSQLSelect = sSQLSelect & sCustomWhereClause 'append to the original query
            End If

            'JB Aug 10
            If v_sObjectName = "peril_type_usage" Then
                sCustomWhereClause = " WHERE "
                sCustomWhereClause = sCustomWhereClause & " peril_type_id IN (SELECT peril_type_id From peril_type Where"
                sCustomWhereClause = sCustomWhereClause & " gis_screen_id IS NULL OR gis_screen_id IN (SELECT gis_screen_id From gis_screen Where"
                sCustomWhereClause = sCustomWhereClause & " gis_data_model_id IN(SELECT gis_data_model_id From gis_data_model Where code = "
                sCustomWhereClause = sCustomWhereClause & "'" & v_sDataModelCode & "'" & "))) ORDER BY peril_type_id"
                sSQLSelect = sSQLSelect & sCustomWhereClause 'append to the original query
            End If


            'only do this if there is SQL
            'derived screens will have already fetched reults

            If sSQLSelect <> conEmptyString Then


                'get the data for the tablecolumn information for this table

                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSelect, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ReadRecordsFromDatabase = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If


                'if we have some results.....

                If IsArray(r_vResults) Then

                    writeToDebugBox(sSQLSelect & " (" & UBound(r_vResults, 2) + 1 & " rows)")

                    'sanity check, if we've fetched * then check that received rows have the same number of elements
                    'Raise an error if this is not the case

                    If v_sConstrainColumns = "*" Then

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If r_aIeTableDefinitions(iTableIndex)(pbIeTableDefinitions_columnCount) - 1 <> UBound(r_vResults) Then

                            writeToStatusBox("Cannot export " & v_sObjectName & ", definition and data arrays are different sizes")
                            ReadRecordsFromDatabase = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Else

                    writeToDebugBox(sSQLSelect & " (0 rows)")

                    'no results found

                End If
            Else
                'no SQL generated

            End If


            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ReadRecordsFromDatabase"

            Exit Function


        Catch ex As Exception


            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ReadRecordsFromDatabase")

            ReadRecordsFromDatabase = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReadRecordsFromDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReadRecordsFromDatabase", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function


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
    Public Function ExtractFile(ByVal iTableIndex As Short, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_sFilenameToRead As String, ByVal v_sFilenameToStore As String, ByVal v_iFileNumber As Short, ByRef r_lTotalLinesWritten As Integer) As Integer

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
                    'Dim filenamescoll As ICollection
                    Dim sTemplateFileName As String = ""
                    'filenamescoll = zFile.EntryFileNames
                    Dim fileNameStruct As String()

                    For Each s As String In zFile.EntryFileNames
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

                    Dim sFileFol As New IO.FileInfo(v_sFilenameToRead)
                    If sFileFol.Directory.Name.Length > 0 AndAlso InStr(1, v_sFilenameToRead, ".rpt") > 0 _
                       AndAlso UCase(sFileFol.Directory.Name) <> "REPORTS" Then
                        vDataArray(0, 0) = v_sFilenameToStore & "|" & sFileFol.Directory.Name
                    Else
                        vDataArray(0, 0) = v_sFilenameToStore
                    End If

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

        Catch

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
    Public Function ExtractDocumentTemplateFile(ByVal iTableIndex As Short, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Short, ByRef r_lTotalLinesWritten As Integer, ByRef r_oDatabase As dPMDAO.Database, ByVal v_iMode As Integer) As Integer

        Dim iFileNumber As Short
        Dim sPathName As String
        Dim iColumnDocumentTypeId As Short
        Dim iColumnDocumentTemplateId As Short
        Dim sFilename As String
        Dim sSQLSearch As String
        Dim bExtractThisOne As Boolean

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractDocumentTemplateFile")

            ExtractDocumentTemplateFile = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Document template file"
            objFrmMainForm.StatusBar_TextWrite("Document template file", 1)

            If IsArray(v_vParentResults) Then
                'need to find the columns we are interested in

                iColumnDocumentTypeId = findColumn("document_type_id", r_aIeTableDefinitions(iTableIndex))
                iColumnDocumentTemplateId = findColumn("document_template_id", r_aIeTableDefinitions(iTableIndex))

                If iColumnDocumentTypeId <> -1 AndAlso iColumnDocumentTemplateId <> -1 Then

                    'AR 06/03/2003 check if this document is part of the 'PBDocs' type
                    If v_iMode And pbIeMode_PBDocsOnly Then
                        sSQLSearch = "SELECT document_type_id FROM document_type WHERE code like 'PBDocs'"
                        m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Existing Data Check", bStoredProcedure:=False)

                        If r_oDatabase.Records.Count <= 0 Then
                            'ExtractDocumentTemplateFile = PMNotFound
                            'writeToStatusBox "Error: Could not access 'PBDocs' Type"
                            Exit Function
                        End If

                        'temporarily comment out next line, Chris Branes told me to do it!
                        If v_vParentResults(iColumnDocumentTypeId, lParentResultIndex) = r_oDatabase.Records.Item(1).Fields.Item(0).value Then
                            bExtractThisOne = True
                        End If
                    Else
                        bExtractThisOne = True
                    End If

                    If bExtractThisOne = True Then
                        'build up the document template file name from the document template data
                        'Type + document_type_id + '\doc ' + document_template_id + '.zip'

                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sFilename = "Type " & v_vParentResults(iColumnDocumentTypeId, lParentResultIndex)
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sFilename = sFilename & "\doc " & v_vParentResults(iColumnDocumentTemplateId, lParentResultIndex)
                        sFilename = sFilename & ".zip"

                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sFilename
                        objFrmMainForm.StatusBar_TextWrite(sFilename, 2)
                        System.Diagnostics.Debugger.Launch()
                        iFileNumber = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", v_sSubKey:=conEmptyString, r_sSettingValue:=sPathName)
                        'module level document root folder setting
                        m_sDocPath = sPathName

                        sPathName = sPathName & AddRequiredBackslash(sPathName) & sFilename

                        m_lReturn = ExtractFile(iTableIndex:=pbIeDbt_DocumentTemplateFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sFilenameToRead:=sPathName, v_sFilenameToStore:=sFilename, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            writeToStatusBox("Could not access " & sPathName)
                        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            writeToStatusBox("Problems occurred while accessing " & sPathName)
                        End If
                    End If '
                Else
                    ' BSJ Sep 2009 - don't display if underwriting
                    If g_bIsUnderwriting = False Then
                        writeToStatusBox(("Could not identify columns for document template file export"))
                    End If
                End If
            Else
                'no results found
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractDocumentTemplateFile")

            Exit Function
        Catch ex As Exception

            FileClose(iFileNumber)

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractDocumentTemplateFile")

            ExtractDocumentTemplateFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractDocumentTemplateFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractDocumentTemplateFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function ExtractUserDefinedLists(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim lResultIndex As Integer
        Dim vResults(,) As Object
        Dim sSQL As String
        Dim arrIndex As Short

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractUserDefinedLists")

            ExtractUserDefinedLists = gPMConstants.PMEReturnCode.PMTrue

            'default progress bar
            objFrmMainForm.ProgressBar1(1).Value = 1

            'get a list of the user defined lists
            'TBD do we do this only for the lists that properties reference or for all of them
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSQL = "select name from sysobjects where xtype='U' and name like '" & r_aIeControl(iTableIndex)(pbIeControl_objectName) & "%'"

            '--------------------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'we only want the ones the user has selected (UDLs that are required by other tables
            'should be exported elsewhere (see calls to ExtractUserDefinedList derived lookup).
            If s_aUDLSelected(0) = "" Then
                'no udls to export?
                ExtractUserDefinedLists = gPMConstants.PMEReturnCode.PMTrue
                Exit Function
            Else
                sSQL = "select name from sysobjects where xtype='U' and name IN ("
                For arrIndex = 0 To objFrmMainForm.tvUDL.Nodes(0).Nodes.Count - 1
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvUDL.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    If objFrmMainForm.tvUDL.Nodes(0).Nodes.Item(arrIndex).Checked Then
                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvUDL.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        sSQL = sSQL & "'" & Trim(objFrmMainForm.tvUDL.Nodes(0).Nodes.Item(arrIndex).Text) & "',"
                    End If
                Next arrIndex
                'trim trailing comma and add the closing bracket
                sSQL = Left(sSQL, Len(sSQL) - 1)
                sSQL = sSQL & ")"
            End If
            'Richard Clarke November 2008 - PIE enhancements END
            '-----------------------------------

            'get the user defined table names
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ExtractUserDefinedLists = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(vResults) Then
                For lResultIndex = 0 To UBound(vResults, 2)

                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    m_lReturn = ExtractUserDefinedList(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, v_sTableName:=vResults(0, lResultIndex), r_lTotalLinesWritten:=r_lTotalLinesWritten)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ExtractUserDefinedLists = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                Next lResultIndex
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractUserDefinedLists")

            Exit Function
        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractUserDefinedLists")

            ExtractUserDefinedLists = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractUserDefinedLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractUserDefinedLists", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function ExtractDerivedRatingRuleFile(ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vResults As Object, ByVal lParentResultIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim vResults(,) As Object
        Dim sOriginalFileName As String
        Dim sNewFileName As String
        Dim iColumnIndexGisInsurerId As Short
        Dim iColumnIndexSchemeNo As Short
        Dim iColumnIndexVersion As Short
        Dim iColumnSchemeDesc As Short

        Dim iColumnSchemeId As Short
        Dim sSQL As String


        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractDerivedRatingRuleFile")

            ExtractDerivedRatingRuleFile = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Scheme derived rating rule"
            objFrmMainForm.StatusBar_TextWrite("Scheme derived rating rule", 1)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = r_aIeControl(iTableIndex)(pbIeControl_objectName)
            objFrmMainForm.StatusBar_TextWrite(r_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
            If IsArray(v_vResults) Then

                'get the original scheme filename using the sp
                iColumnSchemeId = findColumn("gis_Scheme_id", r_aIeTableDefinitions(iTableIndex))
                If iColumnSchemeId <> -1 Then
                    g_oDatabase.Parameters.Clear()
                    sSQL = "spu_gis_get_rule_filename"
                    AddParameter(g_oDatabase, sSQL, m_lReturn, "Schemeid", v_vResults(iColumnSchemeId, lParentResultIndex), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ExtractDerivedRatingRuleFile get filename", bStoredProcedure:=True, vResultArray:=vResults)
                    If IsArray(vResults) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sOriginalFileName = vResults(0, 0)
                    End If
                End If

                'need to find the columns we are interested in
                iColumnIndexGisInsurerId = findColumn("gis_insurer_id", r_aIeTableDefinitions(iTableIndex))
                iColumnIndexSchemeNo = findColumn("scheme_no", r_aIeTableDefinitions(iTableIndex))
                iColumnIndexVersion = findColumn("scheme_ver", r_aIeTableDefinitions(iTableIndex))
                iColumnSchemeDesc = findColumn("Scheme_Desc", r_aIeTableDefinitions(iTableIndex))

                If iColumnIndexGisInsurerId <> -1 AndAlso iColumnIndexSchemeNo <> -1 AndAlso iColumnIndexVersion <> -1 AndAlso iColumnSchemeDesc <> -1 AndAlso sOriginalFileName <> "" Then

                    'build up the new rating file name from the scheme data
                    'scheme_dataModelCode_<scheme_description>_<scheme_version>_<insurer_code>.rul
                    sNewFileName = "scheme_"
                    sNewFileName = sNewFileName & v_sDataModelCode & "_"
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sNewFileName = sNewFileName & "[" & v_vResults(iColumnSchemeDesc, lParentResultIndex) & "]_V"
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sNewFileName = sNewFileName & v_vResults(iColumnIndexVersion, lParentResultIndex) & "_"

                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    g_oDatabase.SQLSelect(sSQL:="select code from gis_insurer where gis_insurer_id=" & v_vResults(iColumnIndexGisInsurerId, lParentResultIndex), sSQLName:="ExtractDerivedRatingRuleFile get insurer code", bStoredProcedure:=False, vResultArray:=vResults)

                    If IsArray(vResults) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sNewFileName = sNewFileName & RTrim(vResults(0, 0))
                    Else
                        sNewFileName = sNewFileName & "?"
                    End If

                    sNewFileName = sNewFileName & ".rul"


                    m_lReturn = ExtractRuleFile(iTableIndex:=pbIeDbt_RuleFile, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, v_sOriginalFilename:=sOriginalFileName, v_sNewFilename:=sNewFileName, v_sDataModelName:=v_sDataModelCode, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_lSchemeId:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ExtractDerivedRatingRuleFile = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                Else
                    writeToStatusBox(("Could not identify scheme columns for document template file export"))
                End If
            Else
                'no results found
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractDerivedRatingRuleFile")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractDerivedRatingRuleFile")

            ExtractDerivedRatingRuleFile = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractDerivedRatingRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractDerivedRatingRuleFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function findColumn(ByVal v_sColumnName As Object, ByRef r_aIeTableDefinitions As Object) As Integer

        Try

            ' Debug message
            '    Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".findColumn"

            For findColumn = 0 To UBound(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray))
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If LCase(r_aIeTableDefinitions(pbIeTableDefinitions_columnArray)(findColumn)(pbIeTableDefinitions_columnName)) = LCase(v_sColumnName) Then
                    Exit Function
                End If
            Next
            findColumn = -1

            ' Debug message
            '   Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".findColumn"

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".findColumn")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="findColumn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="findColumn", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        IsValueInArray
    '
    ' Description: Returns true if value already exists in the array else false
    '
    ' History:     JB Nov 23 2009 - Created.
    '
    ' ***************************************************************** '
    Public Function IsValueInArray(ByVal v_sColumnName As Object, ByVal v_sNewValue As Object, ByVal v_sOldValue As Object, ByRef r_aParentForeignKeys() As Object) As Integer

        Try

            IsValueInArray = -1

            Dim lCounter As Integer
            If r_aParentForeignKeys IsNot Nothing Then
                If UBound(r_aParentForeignKeys) > 0 Then
                    For lCounter = 0 To UBound(r_aParentForeignKeys)
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_sNewValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If LCase(r_aParentForeignKeys(lCounter)(pbIeTableDefinitions_columnName)) = LCase(v_sColumnName) AndAlso Trim(r_aParentForeignKeys(lCounter)(1)) = Trim(v_sNewValue) AndAlso Trim(r_aParentForeignKeys(lCounter)(2)) = Trim(v_sOldValue) Then
                            If Err.Number = 9 Then Exit Function
                            IsValueInArray = lCounter
                            Exit Function
                        End If
                    Next
                End If
            End If
            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".IsValueInArray")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsValueInArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValueInArray", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name:        GetValueFromArray
    '
    ' Description: Gets the value from an array
    '
    ' History:     JB Nov 23 2009 - Created.
    '
    ' ***************************************************************** '
    Public Function GetValueFromArray(ByVal v_sColumnName As Object, ByVal v_sOldValue As Object, ByRef r_aParentForeignKeysDic As Dictionary(Of String, Object)) As String

        'On Error GoTo Err_GetValueFromArray
        GetValueFromArray = ""
        Try
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "lead_commission_band" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "commission_band_id"
            End If

            'JB June 2010 To maintain referential integrity for parent_object_id in gis_object
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "parent_object_id" Or LCase(Trim(v_sColumnName)) = "default_object_id" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "gis_object_id"
            End If

            'JB June 2010 To maintain referential integrity for parent_object_id in gis_property
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "default_property_id" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "gis_property_id"
            End If

            'JB June 2010 To maintain referential integrity for parent_object_id in gis_property
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "specials_type_reference" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "gis_user_def_header_id"
            End If

            'JB June 2010 To maintain referential integrity for tax_group in peril_type
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "tax_group" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "tax_group_id"
            End If

            'JB June 2010 To maintain referential integrity for tax_group in peril_type
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "ri_band" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "ri_band_id"
            End If

            'JB Aug 2010 To maintain referential integrity for gis_screen in gis_screen and gis_screen_detail
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(Trim(v_sColumnName)) = "child_screen_id" Or LCase(Trim(v_sColumnName)) = "parent_id" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                v_sColumnName = "gis_screen_id"
            End If

            Dim sDictionaryPartialKey As String = v_sColumnName.ToString().Trim() + "_" + v_sOldValue.ToString().Trim() + "_"
            Dim fullMatchingKeys As IEnumerable(Of String) = r_aParentForeignKeysDic.Keys.Where(Function(currentKey) currentKey.Contains(sDictionaryPartialKey))
            If fullMatchingKeys.Count() > 0 Then
                GetValueFromArray = fullMatchingKeys.First()
            Else
                GetValueFromArray = ""
            End If

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".GetValueFromArray")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsValueInArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValueInArray", vErrNo:=Err.Number, vErrDesc:=Err.Description)
        End Try
    End Function
    Public Function GetValueFromArray(ByVal v_sColumnName As Object, ByVal v_sOldValue As Object, ByRef r_aParentForeignKeys() As Object) As Integer

        On Error GoTo Err_GetValueFromArray
        GetValueFromArray = -1

        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "lead_commission_band" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "commission_band_id"
        End If

        'JB June 2010 To maintain referential integrity for parent_object_id in gis_object
        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "parent_object_id" OrElse LCase(Trim(v_sColumnName)) = "default_object_id" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "gis_object_id"
        End If

        'JB June 2010 To maintain referential integrity for parent_object_id in gis_property
        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "default_property_id" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "gis_property_id"
        End If

        'JB June 2010 To maintain referential integrity for parent_object_id in gis_property
        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "specials_type_reference" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "gis_user_def_header_id"
        End If

        'JB June 2010 To maintain referential integrity for tax_group in peril_type
        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "tax_group" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "tax_group_id"
        End If

        'JB June 2010 To maintain referential integrity for tax_group in peril_type
        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "ri_band" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "ri_band_id"
        End If

        'JB Aug 2010 To maintain referential integrity for gis_screen in gis_screen and gis_screen_detail
        'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If LCase(Trim(v_sColumnName)) = "child_screen_id" OrElse LCase(Trim(v_sColumnName)) = "parent_id" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sColumnName = "gis_screen_id"
        End If

        Dim lCounter As Integer
        On Error Resume Next
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If r_aParentForeignKeys IsNot Nothing Then
            If Not IsDBNull(v_sOldValue) Then
                For lCounter = 0 To UBound(r_aParentForeignKeys)
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_sOldValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aParentForeignKeys()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_sColumnName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If LCase(r_aParentForeignKeys(lCounter)(pbIeTableDefinitions_columnName)) = LCase(v_sColumnName) AndAlso Trim(r_aParentForeignKeys(lCounter)(2)) = Trim(v_sOldValue) Then
                        If Err.Number = 9 Then Exit Function
                        GetValueFromArray = lCounter
                        Exit Function
                    End If
                Next
            End If
        End If
        GetValueFromArray = -1
        Exit Function

Err_GetValueFromArray:

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".GetValueFromArray")

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsValueInArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValueInArray", vErrNo:=Err.Number, vErrDesc:=Err.Description)

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
    Private Function GetSelfReferencingData(ByRef r_oDatabase As dPMDAO.Database, ByRef r_vResults As Object, ByVal v_sTableName As String, ByVal v_sConstrainColumns As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_iTableIndex As Short, Optional ByVal v_lDataModelID As Integer = 0) As Integer

        Dim sSQL As String
        Dim vRetrievedData(,) As Object
        Dim vResults(,) As Object
        Dim iTotalRecordCount As Short
        Dim iRetrievedRecordCount As Short
        Dim iRecordLoop As Short
        Dim iColumnCount As Short
        Dim iColLoop As Short
        Dim iPKColumn As Short
        Dim iParentIDColumn As Short
        Dim iNewTotal As Short
        Dim iRow As Short
        Dim iLoop As Short
        Dim sParentIn As String
        Dim sPKNotIn As String


        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".GetSelfReferencingData")

        GetSelfReferencingData = gPMConstants.PMEReturnCode.PMTrue

        'Assumes table has only 1 Primary key column, as Parent ID column is only likely to reference 1 primary key column.
        'Currently this procedure is only called for GIS Screen table which has only 1 Primary Key (gis_screen_Id)
        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iPKColumn = r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns)

        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iParentIDColumn = r_aIeControl(v_iTableIndex)(pbIeControl_ParentIdColumn)

        Do
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vRetrievedData = Nothing
            If v_lDataModelID <> 0 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(v_iTableIndex)(pbIeControl_DataModelIdColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQL = "SELECT " & v_sConstrainColumns & " FROM " & v_sTableName & " WHERE " & r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(r_aIeControl(v_iTableIndex)(pbIeControl_DataModelIdColumn) - 1)(pbIeTableDefinitions_columnName) & " = " & v_lDataModelID
                ' Data Model Id has been used now so set to 0 to force next
                ' loop through other half of conditional statement from now on.
                v_lDataModelID = 0
            Else
                If IsArray(vResults) Then
                    sParentIn = "("
                    sPKNotIn = "("
                    For iLoop = 0 To iTotalRecordCount - 1
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sParentIn = sParentIn & vResults(iPKColumn - 1, iLoop) & ", "
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sPKNotIn = sPKNotIn & vResults(iPKColumn - 1, iLoop) & ", "
                    Next
                    sParentIn = Left(sParentIn, Len(sParentIn) - 2) & ")"
                    sPKNotIn = Left(sPKNotIn, Len(sPKNotIn) - 2) & ")"

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iPKColumn - 1)(pbIeTableDefinitions_columnName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sSQL = "SELECT " & v_sConstrainColumns & " FROM " & v_sTableName & " WHERE " & r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iParentIDColumn - 1)(pbIeTableDefinitions_columnName) & " IN " & sParentIn & " AND " & r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iPKColumn - 1)(pbIeTableDefinitions_columnName) & " NOT IN " & sPKNotIn
                Else
                    sSQL = conEmptyString
                End If
            End If

            If sSQL <> conEmptyString Then 'just for debugging!
                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRetrievedData)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetSelfReferencingData = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            If IsArray(vRetrievedData) Then
                writeToDebugBox(sSQL & " (" & UBound(vRetrievedData, 2) + 1 & " rows)")
                iRetrievedRecordCount = UBound(vRetrievedData, 2)
                iColumnCount = UBound(vRetrievedData, 1)
                iNewTotal = iRetrievedRecordCount + iTotalRecordCount
                ReDim Preserve vResults(iColumnCount, iNewTotal)
                For iRecordLoop = 0 To iRetrievedRecordCount
                    For iColLoop = 0 To iColumnCount
                        iRow = iTotalRecordCount + iRecordLoop
                        'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(iColLoop, iRow). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        vResults(iColLoop, iRow) = vRetrievedData(iColLoop, iRecordLoop)
                    Next iColLoop
                Next iRecordLoop
                iTotalRecordCount = UBound(vResults, 2) + 1
            End If
        Loop Until Not IsArray(vRetrievedData)

        If iTotalRecordCount <> 0 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_vResults = VB6.CopyArray(vResults)
        Else
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_vResults = System.DBNull.Value
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".GetSelfReferencingData")

        Exit Function

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
    Public Function ExtractUserDefinedList(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal iTableIndex As Short, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByVal v_sTableName As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim lResultIndex2 As Integer
        Dim vResults2 As Object
        Dim vDataArray(1, 0) As Object
        Dim vTableDetails(,) As Object
        Dim iLoop As Short
        Dim sTableDefinition As String
        Dim atemp() As Object
        Dim iDefinitionIndex As Short

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractUserDefinedList")

            ExtractUserDefinedList = gPMConstants.PMEReturnCode.PMTrue


            sTableDefinition = conEmptyString

            If v_sTableName <> "" AndAlso v_sTableName <> "<NULL>" Then

                'default progress bar
                objFrmMainForm.ProgressBar1(1).Value = 1

                'get a list of the user defined lists
                'TBD do we do this only for the lists that properties reference or for all of them
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = v_sTableName
                objFrmMainForm.StatusBar_TextWrite(v_sTableName, 2)
                m_lReturn = CheckSingleTableGuid(r_oDatabase, v_sTableName)


                With r_oDatabase
                    With .Parameters
                        .Clear()

                        m_lReturn = .Add(sName:="tablename", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    End With
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = .SQLSelect(sSQL:=ACMSGetDMTableColumnsSQL, sSQLName:=ACMSGetDMTableColumnsName, bStoredProcedure:=ACMSGetDMTableColumnsStored, vResultArray:=vTableDetails)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            writeToStatusBox("Could not find table definition for user defined list: " & v_sTableName)
                            Exit Function
                        End If
                    End If
                End With
            End If
            If IsArray(vTableDetails) Then
                Erase atemp
                For iLoop = 0 To UBound(vTableDetails, 2)
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnName, iLoop) & conCommaSpace
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnType, iLoop) & conCommaSpace
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnSize, iLoop) & conCommaSpace
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(sp_msHelpColumns_columnPrecision, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If vTableDetails(sp_msHelpColumns_columnPrecision, iLoop) = conEmptyString Then
                        sTableDefinition = sTableDefinition & "NULL, "
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnPrecision, iLoop) & conCommaSpace
                    End If

                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(sp_msHelpColumns_columnScale, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If vTableDetails(sp_msHelpColumns_columnScale, iLoop) = conEmptyString Then
                        sTableDefinition = sTableDefinition & "NULL, "
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnScale, iLoop) & conCommaSpace
                    End If

                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnNull, iLoop) & conCommaSpace
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnIdentity, iLoop) & conCommaSpace
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTableDetails(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableDefinition = sTableDefinition & vTableDetails(sp_msHelpColumns_columnFlags, iLoop) & conCommaSpace

                    'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    addToArray(atemp, New Object() {vTableDetails(sp_msHelpColumns_columnName, iLoop), vTableDetails(sp_msHelpColumns_columnType, iLoop), vTableDetails(sp_msHelpColumns_columnSize, iLoop)})
                Next iLoop

                ' Knock off last comma from string table definition
                sTableDefinition = Left(sTableDefinition, Len(sTableDefinition) - Len(conCommaSpace))

                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                addToArray(r_aIeTableDefinitions, New Object() {v_sTableName, UBound(vTableDetails, 2) + 1, atemp})
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeTableDefinitions(pbIeDbt_UserDefinedList)(pbIeTableDefinitions_columnCount) = UBound(vTableDetails, 2) + 1

                iDefinitionIndex = UBound(r_aIeTableDefinitions)

                '        addToArray r_aIeControl, Array(iDefinitionIndex, v_sTableName, pbIeOt_UserDefinedList, 0, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Using new data definition array, try again to read in data.
                    m_lReturn = ReadRecordsFromDatabase(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=pbIeDbt_UserDefinedList, v_sObjectName:=v_sTableName, v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, r_vResults:=vResults2, v_sConstrainColumns:="*")
                End If
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Because there are so many entries for a UDL we write the name of the UDL as
                'header record before the actual entries

                'this used only to be called if there were some entries in the table
                'now we write the UDL entries into pmproduct_lookup we need to create UDL tables
                'even if they are empty

                'UPGRADE_WARNING: Couldn't resolve default property of object vDataArray(0, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vDataArray(0, 0) = v_sTableName
                'UPGRADE_WARNING: Couldn't resolve default property of object vDataArray(1, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vDataArray(1, 0) = sTableDefinition

                m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=pbIeDbt_UserDefinedListHeader, i_aDataDefinition:=r_aIeTableDefinitions(pbIeDbt_UserDefinedListHeader)(pbIeTableDefinitions_columnArray), i_aData:=vDataArray, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ExtractUserDefinedList = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                If IsArray(vResults2) Then

                    For lResultIndex2 = 0 To UBound(vResults2, 2)

                        'keep progress and status bar current
                        If lResultIndex2 Mod 100 = 0 Then
                            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = v_sTableName & " (" & lResultIndex2 & " of " & UBound(vResults2, 2) & ")"
                            objFrmMainForm.StatusBar_TextWrite(v_sTableName & " (" & lResultIndex2 & " of " & UBound(vResults2, 2) & ")", 2)
                            objFrmMainForm.ProgressBar1(1).Value = 100 / ((UBound(vResults2, 2) + 1) / (lResultIndex2 + 1))
                            System.Windows.Forms.Application.DoEvents()
                        End If


                        'Write the row passing an array containing the actual data and an
                        'array containing the description of the data in array 1.  Also
                        'pass the file number of the current extract file
                        'add type here!!!!

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(pbIeDbt_UserDefinedList)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=r_aIeControl(pbIeDbt_UserDefinedList)(0), i_aDataDefinition:=r_aIeTableDefinitions(iDefinitionIndex)(pbIeTableDefinitions_columnArray), i_aData:=vResults2, rowIndex:=lResultIndex2, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ExtractUserDefinedList = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    Next lResultIndex2
                Else
                    'no results found
                End If
            Else
                'read returned pmfalse
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractUserDefinedList")

            Exit Function

        Catch ex As Exception

            ExtractUserDefinedList = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractUserDefinedList")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractUserDefinedList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractUserDefinedList", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
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
    Public Function ExtractPropertyDerivedLookups(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iParentIndex As Short, ByVal v_lDataModelID As Integer, ByVal v_sDataModelCode As String, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByRef v_vParentResults As Object, ByVal lParentResultIndex As Integer, ByVal v_sDataModelName As String, ByVal v_iFileNumber As Short, ByVal v_iMode As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        'Define the counters to traverse the array sucture
        Dim iSpecialsType As Short
        Dim iSpecialsTypeReference As Short
        Dim iPmLoopup As Short
        Dim sTableName As String
        Dim iPropertyName As Short

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractPropertyDerivedLookups")

            ExtractPropertyDerivedLookups = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of extract

            If IsArray(v_vParentResults) Then
                'need to find the columns we are interested in

                iSpecialsType = findColumn("specials_type", r_aIeTableDefinitions(v_iParentIndex))
                iSpecialsTypeReference = findColumn("specials_type_reference", r_aIeTableDefinitions(v_iParentIndex))
                iPmLoopup = findColumn("pmlookup_table_name", r_aIeTableDefinitions(v_iParentIndex))
                iPropertyName = findColumn("property_name", r_aIeTableDefinitions(v_iParentIndex))

                sTableName = conEmptyString
                If iSpecialsType <> -1 Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(iSpecialsType, lParentResultIndex). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If v_vParentResults(iSpecialsType, lParentResultIndex) = ACOPMLookupTableName Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sTableName = Trim(v_vParentResults(iSpecialsTypeReference, lParentResultIndex))
                        If InStr(sTableName, " ") <> 0 Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            writeToStatusBox("Property (" & Trim(v_vParentResults(iPropertyName, lParentResultIndex)) & ") references invalid table name (" & sTableName & "). This would cause a fatal import error.")
                            ExtractPropertyDerivedLookups = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    End If
                End If

                If iPmLoopup <> -1 Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vParentResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTableName = Trim(v_vParentResults(iPmLoopup, lParentResultIndex))
                    m_lReturn = CheckFileNameNotAlreadyBeingExported(v_iMode:=v_iMode, r_sTableName:=sTableName, r_aIeTableDefinitions:=r_aIeTableDefinitions)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ExtractPropertyDerivedLookups = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                'Added claim_version_status exception which is not an actual lookup table
                If sTableName <> conEmptyString AndAlso sTableName <> "claim_version_status" Then

                    '---------------------------------
                    'Richard Clarke November 2008 - PIE enhancements
                    'some tables aren't getting their guid columns created if they are child
                    'tables from gis_property, so check and add them here.
                    m_lReturn = CheckSingleTableGuid(r_oDatabase, sTableName)
                    'Richard Clarke November 2008 - PIE enhancements - END
                    '---------------------------------

                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Derived lookup"
                    objFrmMainForm.StatusBar_TextWrite("Derived lookup", 1)
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                    m_lReturn = ExtractUserDefinedList(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=v_iMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=r_aIeControl, r_aIeTableDefinitions:=r_aIeTableDefinitions, iTableIndex:=v_iParentIndex, v_vParentResults:=v_vParentResults, lParentResultIndex:=lParentResultIndex, v_sTableName:=sTableName, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ExtractPropertyDerivedLookups = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractPropertyDerivedLookups")

            Exit Function

        Catch ex As Exception

            ExtractPropertyDerivedLookups = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractPropertyDerivedLookups")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractPropertyDerivedLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractPropertyDerivedLookups", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".CheckFileNameNotAlreadyBeingExported")


        CheckFileNameNotAlreadyBeingExported = gPMConstants.PMEReturnCode.PMTrue

        Dim iTableNameIndex As Object
        If r_sTableName = conEmptyString Then Exit Function

        If InStr(LCase(r_sTableName), "udl_") > 0 Then
            r_sTableName = conEmptyString
            Exit Function
        End If

        For iTableNameIndex = 0 To UBound(r_aIeTableDefinitions)
            'UPGRADE_WARNING: Couldn't resolve default property of object iTableNameIndex. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If LCase(r_aIeTableDefinitions(iTableNameIndex)(0)) = LCase(r_sTableName) Then
                r_sTableName = conEmptyString
                Exit Function
            End If
        Next

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".CheckFileNameNotAlreadyBeingExported")

        Exit Function

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
                objFrmMainForm.StatusBar_TextWrite("Creating  UMLHeaders.dat", 1)
                sb = New StringBuilder
                For nIndex As Integer = 0 To UBound(aoResult, 2)
                    sb.AppendLine(aoResult(0, nIndex).ToString)
                Next

                WriteUMLScript(sb, "UMLHeaders.dat", sbHeaderInfo)

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
                        objFrmMainForm.StatusBar_TextWrite("Creating UMLDetails" & nStartId & ".dat", 1)

                        sb = New StringBuilder
                        For nIndex As Integer = 0 To UBound(aoResult, 2)
                            sb.AppendLine(aoResult(0, nIndex).ToString)
                        Next

                        WriteUMLScript(sb, "UMLDetails" & nStartId & ".dat", sbHeaderInfo)
                        sb = Nothing
                    End If

                    nStartId += kMaxUMLDetailsRecordsToFetch
                Next
            End If
            objFrmMainForm.StatusBar_TextWrite("Creating & " & kUMLHeaderInfo, 1)
            WriteUMLScript(sbHeaderInfo, kUMLHeaderInfo)
            Return PMEReturnCode.PMTrue
        Catch ex As ApplicationException
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                               sMsg:="Failed to Get UML Script", vApp:=ACApp, _
                               vClass:=ACClass, vMethod:="GetUMLScript", _
                               oException:=ex)
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
        Catch ex As ApplicationException
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                               sMsg:="Failed to Execute Script", vApp:=ACApp, _
                               vClass:=ACClass, vMethod:="ExecuteScript", _
                               oException:=ex)
            Return PMEReturnCode.PMError
        End Try

    End Function

    Function ImportUMLScript(ByRef r_oDatabase As dPMDAO.Database) As PMEReturnCode
        Dim sAbsolutePath As String
        Dim srHeaderInfo As System.IO.StreamReader
        Dim sr As System.IO.StreamReader
        Dim sFileName As String
        Dim sExecuteScriptSQL As String

        sAbsolutePath = GetUMLDirectoryPath() & "\" & kUMLHeaderInfo
        If File.Exists(sAbsolutePath) Then
            Try
                srHeaderInfo = New StreamReader(sAbsolutePath)
                While Not srHeaderInfo.EndOfStream
                    sFileName = srHeaderInfo.ReadLine()
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
                    End If
                End While
                Return PMEReturnCode.PMTrue
            Catch ex As Exception
                writeToStatusBox("Failed to Run UML Script " & sAbsolutePath)
                Return PMEReturnCode.PMError
            Finally
                sr.Close()
                sr.Dispose()
                srHeaderInfo.Close()
                srHeaderInfo.Dispose()
            End Try
        End If
    End Function

    Function WriteUMLScript(ByVal sData As StringBuilder, ByVal sFileName As String, _
                            Optional ByRef sbHeaderInfo As StringBuilder = Nothing)
        Dim sAbsolutePath As String
        Dim sPath As String
        Dim sw As System.IO.StreamWriter
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
        Catch ex As ApplicationException
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                               sMsg:="Failed to Get UML Script", vApp:=ACApp, _
                               vClass:=ACClass, vMethod:="GetUMLScript", _
                               oException:=ex)
            Return PMEReturnCode.PMError
        Finally
            sw.Close()
            sw.Dispose()
        End Try

    End Function

End Module