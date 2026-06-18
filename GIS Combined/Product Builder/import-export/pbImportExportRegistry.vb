Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
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
    Private Function ExtractRegistrySettings(ByVal v_vResults As Object, ByVal v_iTableIndex As Integer, ByVal v_iFileNumber As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractRegistrySettings")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the second panel to indicate the type of extract
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Registry"

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(g_aIeControl(v_iTableIndex)(pbIeControl_objectName))

            If Information.IsArray(v_vResults) Then
                'Check that both received arrays have the same number of elements
                'Raise an error if this is not the case

                If ((g_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount)) - 1) <> v_vResults.GetUpperBound(0) Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    writeToStatusBox("Cannot export " & CStr(g_aIeControl(v_iTableIndex)(pbIeControl_objectName)) & ", definition and data arrays are different sizes")
                    Return result
                End If

                'write this record
                'There will be only one entry written for the registry key in g_aIeControl(v_iTableIndex).
                'This will be contained in the last element of the results array as the last record just added.

                m_lReturn = WriteBinaryFileRow(i_iFileNumber:=v_iFileNumber, i_iObjectType:=CInt(g_aIeControl(v_iTableIndex)(0)), i_aDataDefinition:=g_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray), i_aData:=v_vResults, rowIndex:=0, r_lTotalLinesWritten:=r_lTotalLinesWritten)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractRegistrySettings")
            Return result

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
    Public Function FindRegistrySettings(ByVal v_sKey As String, ByVal v_sGISDataModel As String, ByVal v_iTableIndex As Integer, ByVal v_iFileNumber As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim result As Integer = 0
        Dim sKeyPath, sKeyName, sKeyValue, sSubKeyPath As String
        Dim vDataArray As Array = Array.CreateInstance(GetType(Object), New Integer() {conKeyValue - conKeyPath + 1, 1}, New Integer() {conKeyPath, 0})
        Dim sKey, sMachineName As String

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".FindRegistrySettings")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Store machine name in local string in case any registry entries require substitution
            m_lReturn = gPMFunctions.GetSystemName(sSystemName:=sMachineName)

            'Replace Data model code substitution text
            sKey = v_sKey.Replace(conDMC, v_sGISDataModel)

            'Replace Machine Name substitution text
            sKey = sKey.Replace(conMachineName, sMachineName)

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
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sKeyName, r_sSettingValue:=sKeyValue, v_sSubKey:=sSubKeyPath)
            End If

            sKeyValue = sKeyValue.ToUpper().Replace(sMachineName.ToUpper(), conMachineName.ToUpper())

            vDataArray(conKeyPath, 0) = v_sKey

            vDataArray(conKeyName, 0) = sKeyName

            vDataArray(conKeyValue, 0) = sKeyValue

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ExtractRegistrySettings(v_vResults:=vDataArray, v_iTableIndex:=v_iTableIndex, v_iFileNumber:=v_iFileNumber, r_lTotalLinesWritten:=r_lTotalLinesWritten)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".FindRegistrySettings")
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".FindRegistrySettings")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0
        Dim nPosition As Integer

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractName")

            result = gPMConstants.PMEReturnCode.PMTrue

            nPosition = (r_sText.IndexOf(r_sSearchChar) + 1)

            If nPosition <> 0 Then
                r_sKeyName = r_sText.Substring(nPosition).Trim()
                r_sKeyPath = r_sText.Substring(0, nPosition - 1).Trim()
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractName")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractName")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0
        Dim nLength, nPosition As Integer

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ExtractSubKeyPath")

            result = gPMConstants.PMEReturnCode.PMTrue

            nPosition = (r_sFullKeyPath.ToUpper().IndexOf(conServer) + 1)
            nLength = conServer.Length

            If nPosition = 0 Then
                nPosition = (r_sFullKeyPath.ToUpper().IndexOf(conClient) + 1)
                nLength = conClient.Length
            End If

            If nPosition = 0 Then
                nPosition = (r_sFullKeyPath.ToUpper().IndexOf(conCommon) + 1)
                nLength = conCommon.Length
            End If

            If nPosition = 0 Then
                nPosition = (r_sFullKeyPath.ToUpper().IndexOf(conSetUp) + 1)
                nLength = conSetUp.Length
            End If

            If nPosition <> 0 Then
                r_sSubKeyPath = r_sFullKeyPath.Substring(nPosition + nLength - 1)
            Else
                r_sSubKeyPath = r_sFullKeyPath
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ExtractSubKeyPath")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ExtractSubKeyPath")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtractSubKeyPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractSubKeyPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
