Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module PbEdoEXport
	
	Private m_lReturn As Integer
    Public objFrmMainForm As mainForm

    Private Const ACClass As String = conEmptyString


    ' ***************************************************************** '
    '
    ' Name: doExport
    '
    ' Description:
    '
    ' History: 03/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function doExport(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByRef v_alDataModelID() As Integer, ByRef v_asDataModelCode() As String, ByRef g_aIeControl() As Object, ByRef g_aIeTableDefinitions() As Object, ByVal v_sVersionNumber As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim iTableIndex As Short
        Dim iExportMode As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".doExport")

        Try

        doExport = gPMConstants.PMEReturnCode.PMTrue

        'Richard Clarke November 2008 - get the data model code and id
        Dim v_lDataModelID As Integer
        Dim v_sDataModelCode As String
        Dim iDataModelindex As Short

        Dim bHeaderExtracted As Boolean
        Dim bTableDefsExtracted As Boolean

        Dim bUDLsExtracted As Boolean
        Dim bReportsExtracted As Boolean
        Dim bReportsSPUExtracted As Boolean
        Dim bDocTemplatesExtracted As Boolean
        Dim bUserSPUExtracted As Boolean
        Dim bGISListExtracted As Boolean


        If objFrmMainForm.chkGenerateUMLScript.Checked Then
            Dim sFilePath As String = objFrmMainForm.txtFilePath(1).Text

            objFrmMainForm.StatusBar_TextWrite("Generating UML Script", 0)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 1)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
            objFrmMainForm.ProgressBar1(0).Visible = True

            If ExportUMLScript(r_oDatabase) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Get UML Script")
            End If
        End If

        'Richard Clarke November 2008 - end of PIE changes

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Exporting"
        objFrmMainForm.StatusBar_TextWrite("Exporting", 0)
        objFrmMainForm.ProgressBar1(0).Visible = True
        objFrmMainForm.ProgressBar1(0).Maximum = UBound(g_aIeControl) + 1

        objFrmMainForm.ProgressBar1(0).Value = 1

        iExportMode = buildExportMode()

        '------------------------------
        'Richard Clarke November 2008 - PIE enhancements
        'added outer for loop to loop all the data models the user has chosen.
        For iDataModelindex = 0 To UBound(v_alDataModelID) - 1
            v_lDataModelID = v_alDataModelID(iDataModelindex)
            v_sDataModelCode = v_asDataModelCode(iDataModelindex)
            '-------------------------------
            For iTableIndex = 0 To UBound(g_aIeControl)
                If g_bStopProcessing Then
                    Exit Function
                Else
                    If g_aIeControl(iTableIndex)(pbIeControl_operationMode) And iExportMode Then
                        objFrmMainForm.ProgressBar1(0).Value = iTableIndex + 1

                        'is this a table object
                        Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)

                            Case pbIeOt_Header
                                If bHeaderExtracted = False Then
                                    m_lReturn = ExtractHeader(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_alDataModelID:=v_alDataModelID, v_asDataModelCode:=v_asDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_sVersionNumber:=v_sVersionNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                    bHeaderExtracted = True
                                End If

                                'we have now extracted the header so this would be a good time to
                                'extract the table definitions
                                If bTableDefsExtracted = False Then
                                    m_lReturn = ExtractTableDefinitions(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_sVersionNumber:=v_sVersionNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                    bTableDefsExtracted = True
                                End If

                            Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined
                                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                If (iExportMode And pbIeMode_UDLs) = False And (Left(g_aIeControl(iTableIndex)(pbIeControl_objectName), 12) = "gis_user_def" Or Left(g_aIeControl(iTableIndex)(pbIeControl_objectName), 10) = "gis_lookup") Then

                                ElseIf Not objFrmMainForm.chkIncludeUMLs.Checked AndAlso UCase(Left(g_aIeControl(iTableIndex)(pbIeControl_objectName), 12)) = "GIS_USER_DEF" Then

                                Else
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Database table"
                                    objFrmMainForm.StatusBar_TextWrite("Database table", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = ExtractFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_iExportMode:=iExportMode)
                                End If
                            Case pbIeOt_RiskGroupsCodes
                                If iExportMode And pbIeMode_RiskGroupsCodes Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Database table"
                                    objFrmMainForm.StatusBar_TextWrite("Database table", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = ExtractFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_iExportMode:=iExportMode)
                                End If
                            Case pbIeOt_RegSetting
                                If iExportMode And pbIeMode_Registry Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Registry setting"
                                    objFrmMainForm.StatusBar_TextWrite("Registry setting", 1)
                                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = g_aIeControl(iTableIndex)(pbIeControl_objectName)
                                    objFrmMainForm.StatusBar_TextWrite(g_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
                                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    m_lReturn = FindRegistrySettings(v_sKey:=g_aIeControl(iTableIndex)(pbIeControl_objectName), v_sGISDataModel:=v_sDataModelCode, v_iTableIndex:=iTableIndex, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                                End If
                            Case pbIeOt_UserDefinedList 'only do once
                                If iExportMode And pbIeMode_UDLs And Not bUDLsExtracted Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User defined list"
                                    objFrmMainForm.StatusBar_TextWrite("User defined list", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = ExtractUserDefinedLists(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                    bUDLsExtracted = True
                                End If

                            Case pbIeOt_GisList 'only do once
                                If bGISListExtracted = False Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Gis list"
                                    objFrmMainForm.StatusBar_TextWrite("Gis list", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    'UPGRADE_WARNING: Couldn't resolve default property of object ExtractGisListsFile(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    m_lReturn = ExtractGisListsFile(v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                                    bGISListExtracted = True
                                End If

                                'Richard Clarke November 2008 - PIE enhancements
                                'do I need a check here to see if the user has selected the spu tickbox?
                            Case pbIeOt_UserSPU 'only do once
                                If bUserSPUExtracted = False Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User SPU"
                                    objFrmMainForm.StatusBar_TextWrite("User SPU", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserSPUsFile(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    m_lReturn = ExtractUserSPUsFile(v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                    bUserSPUExtracted = True
                                End If

                            Case pbIeOt_UserCrystalReport 'only do once
                                If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUReports).CheckState = 1 And Not bReportsExtracted Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User Crystal Reports"
                                    objFrmMainForm.StatusBar_TextWrite("User Crystal Reports", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    'UPGRADE_WARNING: Couldn't resolve default property of object ExtractUserCrystalReportFile(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    m_lReturn = ExtractUserCrystalReportFile(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                    bReportsExtracted = True
                                End If
                                'Richard Clarke November 2008 - PIE enhancements END
                            Case pbIeOt_UserCrystalReportSPU 'only do once
                                If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_ReportSPU).CheckState = 1 And Not bReportsSPUExtracted Then
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "User defined list"
                                    objFrmMainForm.StatusBar_TextWrite("User Crystal Reports SPU", 1)
                                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = ExtractUserReportSPU(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelID:=v_lDataModelID, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                                    bReportsSPUExtracted = True
                                End If
                        End Select
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            doExport = gPMConstants.PMEReturnCode.PMError
                            Exit Function
                        End If
                    Else
                    End If
                End If
            Next
            'this is the end of the data model array loop
        Next  'Richard Clarke November 2008 - PIE enhancements


        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".doExport")

        Exit Function

        Catch ex As Exception

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".doExport")

        doExport = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="doExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="doExport", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function
End Module
