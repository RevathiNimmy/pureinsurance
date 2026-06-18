Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module PbEdoEXport

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Const ACClass As String = conEmptyString

    Public objFrmMainForm As mainForm
    ' ***************************************************************** '
    '
    ' Name: doExport
    '
    ' Description:
    '
    ' History: 03/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function doExport(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Integer, ByVal v_lDataModelId As Integer, ByVal v_sDataModelCode As String, ByRef g_aIeControl() As Object, ByRef g_aIeTableDefinitions() As Object, ByVal v_sVersionNumber As String, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim result As Integer = 0
        Dim iExportMode As Integer

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".doExport")

        Try


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


            result = gPMConstants.PMEReturnCode.PMTrue
            objFrmMainForm.StatusBar_TextWrite("Exporting", 0)
            'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Exporting"
            objFrmMainForm.ProgressBar1(0).Visible = True
            objFrmMainForm.ProgressBar1(0).Maximum = g_aIeControl.GetUpperBound(0) + 1

            objFrmMainForm.ProgressBar1(0).Value = 1

            iExportMode = buildExportMode()

            For iTableIndex As Integer = 0 To g_aIeControl.GetUpperBound(0)
                If g_bStopProcessing Then
                    Return result
                Else

                    If g_aIeControl(iTableIndex)(pbIeControl_operationMode) And iExportMode Then
                        objFrmMainForm.ProgressBar1(0).Value = iTableIndex + 1

                        'is this a table object

                        Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)
                            Case pbIeOt_Header
                                m_lReturn = CType(ExtractHeader(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_sVersionNumber:=v_sVersionNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                                'we have no extracted the header so this would be a good time to
                                'extract the table definitions
                                m_lReturn = CType(ExtractTableDefinitions(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_sVersionNumber:=v_sVersionNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                            Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined

                                If (iExportMode And pbIeMode_UDLs) = False And (Left(g_aIeControl(iTableIndex)(pbIeControl_objectName), 12) = "gis_user_def" Or Left(g_aIeControl(iTableIndex)(pbIeControl_objectName), 10) = "gis_lookup") Then

                                ElseIf Not objFrmMainForm.chkIncludeUMLs.Checked AndAlso UCase(Left(g_aIeControl(iTableIndex)(pbIeControl_objectName), 12)) = "GIS_USER_DEF" Then

                                Else
                                    'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Database table"
                                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite("Database table", 1)
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = CType(ExtractFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=Nothing, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_iExportMode:=iExportMode), gPMConstants.PMEReturnCode)
                                End If
                            Case pbIeOt_RiskGroupsCodes
                                If iExportMode And pbIeMode_RiskGroupsCodes Then
                                    'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Database table"
                                    ' objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite("Database table", 1)
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = CType(ExtractFixedTables(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten, v_iExportMode:=iExportMode), gPMConstants.PMEReturnCode)
                                End If
                            Case pbIeOt_RegSetting
                                If iExportMode And pbIeMode_Registry Then
                                    'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Registry setting"
                                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName))
                                    objFrmMainForm.StatusBar_TextWrite("Registry setting", 1)
                                    objFrmMainForm.StatusBar_TextWrite(CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName)), 2)

                                    m_lReturn = CType(FindRegistrySettings(v_sKey:=CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName)), v_sGISDataModel:=v_sDataModelCode, v_iTableIndex:=iTableIndex, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)

                                End If
                            Case pbIeOt_UserDefinedList
                                If iExportMode And pbIeMode_UDLs Then
                                    'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "User defined list"
                                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
                                    objFrmMainForm.StatusBar_TextWrite("User defined list", 1)
                                    objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                    m_lReturn = CType(ExtractUserDefinedLists(r_oDatabase:=r_oDatabase, v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten), gPMConstants.PMEReturnCode)
                                End If

                            Case pbIeOt_GisList
                                'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Gis list"
                                'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
                                objFrmMainForm.StatusBar_TextWrite("Gis list", 1)
                                objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
                                m_lReturn = ExtractGisListsFile(v_iFileNumber:=v_iFileNumber, v_iMode:=iExportMode, v_lDataModelId:=v_lDataModelId, v_sDataModelCode:=v_sDataModelCode, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, iTableIndex:=iTableIndex, v_vParentResults:=0, lParentResultIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)
                        End Select
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMError
                        End If
                    Else
                    End If
                End If
            Next

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".doExport")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".doExport")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="doExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="doExport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
