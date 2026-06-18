Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportExportRegistry

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ExtractRegistrySettings
    '
    ' Description: Processes the extraction of registry settings to
    '              the binary file
    '
    ' History:     03/09/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Private Function ExtractRegistrySettings(ByVal v_vResults As Object, ByVal v_iTableIndex As Short, ByVal v_iFileNumber As Short, ByRef r_lTotalLinesWritten As Integer) As Integer


        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractRegistrySettings")

        ExtractRegistrySettings = gPMConstants.PMEReturnCode.PMTrue

        'Set the second panel to indicate the type of extract
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Registry"
        objFrmMainForm.StatusBar_TextWrite("Registry", 1)
        'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(v_iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = g_aIeControl(v_iTableIndex)(pbIeControl_objectName)
        objFrmMainForm.StatusBar_TextWrite(g_aIeControl(v_iTableIndex)(pbIeControl_objectName), 2)
        If IsArray(v_vResults) Then
            'Check that both received arrays have the same number of elements
            'Raise an error if this is not the case
            'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If g_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount) - 1 <> UBound(v_vResults) Then
                ExtractRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                writeToStatusBox("Cannot export " & g_aIeControl(v_iTableIndex)(pbIeControl_objectName) & ", definition and data arrays are different sizes")
                Exit Function
            End If

            'write this record
            'There will be only one entry written for the registry key in g_aIeControl(v_iTableIndex).
            'This will be contained in the last element of the results array as the last record just added.
            'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(v_iTableIndex)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=g_aIeControl(v_iTableIndex)(0), i_aDataDefinition:=g_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=v_vResults, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ExtractRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractRegistrySettings")
        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name:        FindRegistrySettings
    '
    ' Description: Retrieves registry setting from using supplied key
    '
    ' History:     03/09/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function FindRegistrySettings(ByVal v_sKey As String, ByVal v_sGISDataModel As String, ByVal v_iTableIndex As Short, ByVal v_iFileNumber As Short, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim sKeyPath As String
        Dim sKeyName As String
        Dim sKeyValue As String
        Dim sSubKeyPath As String
        'UPGRADE_WARNING: Lower bound of array vDataArray was changed from conKeyPath,0 to 0,0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim vDataArray(conKeyValue, 0) As Object
        Dim sKey As String
        Dim sMachineName As String

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".FindRegistrySettings")

            FindRegistrySettings = gPMConstants.PMEReturnCode.PMTrue

            'Store machine name in local string in case any registry entries require substitution
            m_lReturn = gPMFunctions.GetSystemName(sSystemName:=sMachineName)

            'Replace Data model code substitution text
            sKey = Replace(v_sKey, conDMC, v_sGISDataModel)

            'Replace Machine Name substitution text
            sKey = Replace(sKey, conMachineName, sMachineName)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ExtractName(r_sText:=sKey, r_sSearchChar:=conComma, r_sKeyPath:=sKeyPath, r_sKeyName:=sKeyName)
            End If

            'Subpath is required by GetPMRegSetting procedure.
            'This already appends several items to get the full key.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ExtractSubKeyPath(r_sFullKeyPath:=sKeyPath, r_sSubKeyPath:=sSubKeyPath)
            End If

            'Get the value
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sKeyName, r_sSettingValue:=sKeyValue, v_sSubKey:=sSubKeyPath)
            End If

            sKeyValue = Replace(UCase(sKeyValue), UCase(sMachineName), UCase(conMachineName))

            'UPGRADE_WARNING: Couldn't resolve default property of object vDataArray(conKeyPath, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vDataArray(conKeyPath, 0) = v_sKey
            'UPGRADE_WARNING: Couldn't resolve default property of object vDataArray(conKeyName, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vDataArray(conKeyName, 0) = sKeyName
            'UPGRADE_WARNING: Couldn't resolve default property of object vDataArray(conKeyValue, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vDataArray(conKeyValue, 0) = sKeyValue

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ExtractRegistrySettings(v_vResults:=vDataArray, v_iTableIndex:=v_iTableIndex, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                FindRegistrySettings = gPMConstants.PMEReturnCode.PMError
                Exit Function
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".FindRegistrySettings")
            Exit Function

        Catch ex As Exception
            FindRegistrySettings = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".FindRegistrySettings")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindRegistrySettings", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractName
    '
    ' Description: Finds the Key Name within the Key Path string.
    '
    ' History:     03/09/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractName(ByRef r_sText As String, ByRef r_sSearchChar As String, ByRef r_sKeyPath As String, ByRef r_sKeyName As String) As Integer

        Dim nPosition As Short

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractName")

            ExtractName = gPMConstants.PMEReturnCode.PMTrue

            nPosition = InStr(1, r_sText, r_sSearchChar)

            If nPosition <> 0 Then
                r_sKeyName = Trim(Mid(r_sText, nPosition + 1))
                r_sKeyPath = Trim(Left(r_sText, nPosition - 1))
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractName")

            Exit Function

        Catch ex As Exception

            ExtractName = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractName")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractName", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ExtractSubKeyPath
    '
    ' Description: Evaluates the required sub key path component for the
    '              GetPMRegSetting routine
    '
    ' History:     03/09/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function ExtractSubKeyPath(ByRef r_sFullKeyPath As String, ByRef r_sSubKeyPath As String) As Integer

        Dim nLength As Short
        Dim nPosition As Short

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ExtractSubKeyPath")

            ExtractSubKeyPath = gPMConstants.PMEReturnCode.PMTrue

            nPosition = InStr(1, UCase(r_sFullKeyPath), conServer)
            nLength = Len(conServer)

            If nPosition = 0 Then
                nPosition = InStr(1, UCase(r_sFullKeyPath), conClient)
                nLength = Len(conClient)
            End If

            If nPosition = 0 Then
                nPosition = InStr(1, UCase(r_sFullKeyPath), conCommon)
                nLength = Len(conCommon)
            End If

            If nPosition = 0 Then
                nPosition = InStr(1, UCase(r_sFullKeyPath), conSetUp)
                nLength = Len(conSetUp)
            End If

            If nPosition <> 0 Then
                r_sSubKeyPath = Mid(r_sFullKeyPath, nPosition + nLength)
            Else
                r_sSubKeyPath = r_sFullKeyPath
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ExtractSubKeyPath")

            Exit Function

        Catch ex As Exception

            ExtractSubKeyPath = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ExtractSubKeyPath")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractSubKeyPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractSubKeyPath", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function
End Module