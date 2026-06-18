Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportRegistry
	
	Private Const ACClass As String = conEmptyString
	
	Private m_lReturn As Integer
	
	' ***************************************************************** '
	'
	' Name:        ImportRegistrySettings
	'
	' Description: Sets any registry settings retrieved from the binary
	'              file into an array which is then passed to be added
	'              to the registry.
	'
	' History:     05/09/2002 SJP - Created.
	'
	' ***************************************************************** '
	Public Function ImportRegistrySettings(ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_iIntegerData As Short, ByVal v_sDataModelCode As String, ByVal v_bImportRegistry As Boolean) As Integer
		
		
		
		Dim vRetrievedData As Object
		Dim sKey As String
		Dim sKeyPath As String
		Dim sSubKeyPath As String
		Dim sKeyName As String
		Dim sKeyValue As String
		Dim sMachineName As String
		
		' Debug message
		Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportRegistrySettings")
		
		Try
		
		ImportRegistrySettings = gPMConstants.PMEReturnCode.PMTrue
		
		'Read a row at a time passing in the definition for the row
		'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(v_iIntegerData)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_iIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_iIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=vRetrievedData, rowIndex:=v_iIntegerData)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			ImportRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		If v_bImportRegistry Then
			If IsArray(vRetrievedData) Then
				'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				sKey = Replace(vRetrievedData(conKeyPath), conDMC, v_sDataModelCode)
				
                m_lReturn = gPMFunctions.GetSystemName(sSystemName:=sMachineName)
				
				' Replace any occurrences of machine name within the registry key path.
				sKey = Replace(sKey, conMachineName, sMachineName)
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					m_lReturn = ExtractName(r_sText:=sKey, r_sSearchChar:=conComma, r_sKeyPath:=sKeyPath, r_sKeyName:=sKeyName)
				End If
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					m_lReturn = ExtractSubKeyPath(r_sFullKeyPath:=sKeyPath, r_sSubKeyPath:=sSubKeyPath)
				End If
				
				'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				sKeyValue = vRetrievedData(conKeyValue)
				
				sKeyValue = Replace(UCase(sKeyValue), UCase(conMachineName), UCase(sMachineName))
				'Get the value
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					'UPGRADE_WARNING: Couldn't resolve default property of object vRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=vRetrievedData(conKeyName), v_sSettingValue:=sKeyValue, v_sSubKey:=sSubKeyPath)
				End If
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					ImportRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
					Exit Function
				End If
				
			End If
		End If
		
		' Debug message
		Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ImportRegistrySettings")
		
		Exit Function
		
		Catch ex As Exception
		
		' Debug message
		Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportRegistrySettings")
		
		ImportRegistrySettings = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportRegistrySettings", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
		End Try
	End Function
End Module
