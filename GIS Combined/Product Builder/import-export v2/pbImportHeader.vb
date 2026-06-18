Option Strict Off
Option Explicit On
Imports SharedFiles
Module pbImportHeader

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ImportHeader
    '
    ' Description: Processes the import of Header information from the binary
    '              file.  Receives the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              07/10/2002 SJP - Included Data Model Id in the export
    '                               file, in array element 1 (after import).
    '
    ' ***************************************************************** '
    Public Function ImportHeader(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_lIntegerData As Integer, ByRef r_lDataModelId() As Integer, ByRef r_sDataModelCode() As String, ByRef r_iMode As Integer, ByVal v_sVersionNumber As String, ByRef r_lTotalLines As Integer) As Integer

        'Richard Clarke November 2008 - PIE enhancements
        'Altered function definition to use the arrays of datamodelid and datamodelcode
        'ByRef r_lDataModelId As Long,
        'ByRef r_sDataModelCode As String
        'Richard Clarke November 2008 - PIE enhancements

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object

        'Check if the header information is correct
        Dim vDBSource(,) As Object
        Dim sDBSource As String
        Dim sTemp1 As String
        Dim sTemp2 As String
        Dim vDBError As Object
        Dim nPosition As Integer
        Dim iLoop As Integer

        Dim r_sDataModelID() As String
        Dim sDataModelIDs As String
        Dim sDataModelCodes As String

        Try

            ' Debug message
            'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".ImportHeader"

            ImportHeader = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            With objFrmMainForm
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                '            .StatusBar1(1).Items.Item(1).Text = "Header"
                .StatusBar_TextWrite("Header", 1)
                '.StatusBar1(2).Items.Item(1).Text = v_aIeControl(v_lIntegerData)(pbIeControl_objectName)
                .StatusBar_TextWrite(v_aIeControl(v_lIntegerData)(pbIeControl_objectName), 2)
            End With

            'Read a row passing in the definition for the row
            'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(v_lIntegerData)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_lIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_lIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_lIntegerData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportHeader = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' ***************************************************************** '

            'Splat the header information to the import page
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_iMode = CInt(CStr(aRetrievedData(1)))
            'set the GUI
            SetGUIOptionsBasedOnMode(r_iMode)

            'total lines in file
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_lTotalLines = aRetrievedData(0)

            'Richard Clarke November 2008 - PIE enhancements
            'convert the datamodelid and datamodelcode info from the file into arrays
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_sDataModelCode = Split(aRetrievedData(3), ",") 'Richard Clarke - moved this above the next line
            ReDim r_sDataModelID(UBound(r_sDataModelCode))
            ReDim r_lDataModelId(UBound(r_sDataModelID))
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_sDataModelID = Split(aRetrievedData(2), ",")

            For iLoop = 0 To UBound(r_sDataModelID)
                r_lDataModelId(iLoop) = CInt(r_sDataModelID(iLoop))
                sDataModelIDs = sDataModelIDs & r_sDataModelID(iLoop) & ","
                sDataModelCodes = sDataModelCodes & r_sDataModelCode(iLoop) & ","
            Next iLoop

            'remove the trailing comma from the sDataModels
            sDataModelIDs = Left(sDataModelIDs, Len(sDataModelIDs) - 1)
            g_sDataModelIDs = sDataModelIDs
            sDataModelCodes = Left(sDataModelCodes, Len(sDataModelCodes) - 1)
            'r_lDataModelId = Split(aRetrievedData(2), ",")

            'Richard Clarke November 2008 - PIE enhancements

            'Find out the data model id.
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Not aRetrievedData(2) = conEmptyString Then
                'r_lDataModelId = aRetrievedData(2)
            Else
                writeToStatusBox("No data model id has been supplied in the export file")
            End If

            'Find out the data model code.
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(3). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Not aRetrievedData(3) = conEmptyString Then
                'r_sDataModelCode = aRetrievedData(3)
                'g_DataModelCode = r_sDataModelCode
            End If

            ' Find out which system is handled by export file
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            g_bIsUnderwriting = aRetrievedData(4)

            ' Find out version number of export tool used to create export file
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_sVersionNumber = aRetrievedData(5)

            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sDBSource = aRetrievedData(7)
            iLoop = 0

            If Len(sDBSource) > 0 Then
                Do
                    nPosition = InStr(1, sDBSource, conComma)
                    'UPGRADE_WARNING: Lower bound of array vDBSource was changed from conName,0 to 0,0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
                    ReDim Preserve vDBSource(conVersion_Renamed, iLoop)

                    sTemp1 = Left(sDBSource, nPosition - 1)
                    sDBSource = Mid(sDBSource, nPosition + 1)
                    nPosition = InStr(1, sDBSource, conComma)
                    If nPosition <> 0 Then
                        sTemp2 = Left(sDBSource, nPosition - 1)
                        sDBSource = Mid(sDBSource, nPosition + 1)
                    Else
                        sTemp2 = sDBSource
                    End If

                    ' System Name
                    'UPGRADE_WARNING: Couldn't resolve default property of object vDBSource(conName, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    vDBSource(conName, iLoop) = sTemp1
                    ' DB Version
                    'UPGRADE_WARNING: Couldn't resolve default property of object vDBSource(conVersion_Renamed, iLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    vDBSource(conVersion_Renamed, iLoop) = sTemp2

                    iLoop = iLoop + 1
                Loop Until nPosition = 0

                m_lReturn = CompareDBVersions(r_oDatabase:=r_oDatabase, v_vSource:=vDBSource, r_vErrorList:=vDBError)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ImportHeader = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            If IsArray(vDBError) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = BuildHeaderConfirmationText(txtConfirmationText:=objFrmMainForm.txtImportConfirmation, sDataModelCodes:=sDataModelCodes, v_sVersionNumber:=v_sVersionNumber, v_sComments:=CStr(aRetrievedData(6)), v_vVersionError:=vDBError, v_lTotalLines:=r_lTotalLines)
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = BuildHeaderConfirmationText(txtConfirmationText:=objFrmMainForm.txtImportConfirmation, sDataModelCodes:=sDataModelCodes, v_sVersionNumber:=v_sVersionNumber, v_sComments:=CStr(aRetrievedData(6)), v_lTotalLines:=r_lTotalLines)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportHeader = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' ***************************************************************** '

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".ImportHeader"

            Exit Function

        Catch ex As Exception

            ImportHeader = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            'Debug.Print Timer & ": Errored in " & ACApp & conDot & ACClass & ".ImportHeader"

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportHeader", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        CompareDBVersions
    '
    ' Description: Checks if database version information received from
    '              source database is the same as that contained in the
    '              target database.  If not, then will throw up a warning
    '              before user decides if to proceed with import.
    '
    ' History:     01/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Private Function CompareDBVersions(ByRef r_oDatabase As dPMDAO.Database, ByVal v_vSource As Object, ByRef r_vErrorList As Object) As Integer

        Dim sSQL As String
        Dim vTarget(,) As Object
        Dim iLoopSource As Short
        Dim iError As Short
        Dim vErrorList() As Object


        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & conDot & ACClass & ".CompareDBVersions"

        CompareDBVersions = gPMConstants.PMEReturnCode.PMTrue

        If IsArray(v_vSource) Then
            iError = 0
            For iLoopSource = 0 To UBound(v_vSource, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vSource(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQL = "SELECT version FROM PMLogicalDatabase" & vbCrLf & "WHERE UPPER(name) = '" & UCase(v_vSource(conName, iLoopSource)) & "'"

                m_lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDBVersions", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTarget)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CompareDBVersions = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                If IsArray(vTarget) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vTarget(0, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vSource(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Not CStr(v_vSource(conVersion_Renamed, iLoopSource)) = vTarget(0, 0) Then
                        ReDim Preserve vErrorList(iError)
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vSource(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vErrorList(iError). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        vErrorList(iError) = v_vSource(conName, iLoopSource)
                        iError = iError + 1
                    End If
                End If
            Next iLoopSource
        End If

        If iError > 0 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vErrorList. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_vErrorList = VB6.CopyArray(vErrorList)
        End If

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & conDot & ACClass & ".CompareDBVersions"

        Exit Function

    End Function
    ' ***************************************************************** '
    '
    ' Name: SetGUIOptionsBasedOnMode
    '
    ' Description:
    '
    ' History: 02/04/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub SetGUIOptionsBasedOnMode(ByVal r_iMode As Integer)

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & "." & ACClass & ".SetGUIOptionsBasedOnMode"

        Try

            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Registry).CheckState = IIf(r_iMode And pbIeMode_Registry, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Documents).CheckState = IIf(r_iMode And pbIeMode_Documents, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_Rulefiles).CheckState = IIf(r_iMode And pbIeMode_RuleFiles, 1, 0)
            'objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_RiskGroupsCodes) = IIf(r_iMode And pbIeMode_RiskGroupsCodes, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUICCS).CheckState = IIf(r_iMode And pbIeMode_UserStoredProcedure, 1, 0)
            'objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly) = IIf(r_iMode And pbIeMode_PBDocsOnly, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_UDLs).CheckState = IIf(r_iMode And pbIeMode_UDLs, 1, 0)
            'If g_bIsUnderwriting = False Then objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).Enabled = True
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUReports).CheckState = IIf(r_iMode And pbIeMode_UserReports, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_ReportSPU).CheckState = IIf(r_iMode And pbIeMode_UserReportsSPU, 1, 0)
            objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_AuthRules).CheckState = IIf(r_iMode And pbIeMode_UARs, 1, 0)
            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & "." & ACClass & ".SetGUIOptionsBasedOnMode"

            Exit Sub

        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetGUIOptionsBasedOnMode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetGUIOptionsBasedOnMode", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Sub

        End Try
    End Sub
End Module