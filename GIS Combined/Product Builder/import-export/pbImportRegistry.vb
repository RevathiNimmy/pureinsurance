Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
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
	Public Function ImportRegistrySettings(ByVal v_iFileNumber As Integer, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_iIntegerData As Integer, ByVal v_sDataModelCode As String, ByVal v_bImportRegistry As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim vRetrievedData As Object
		Dim sKey, sKeyPath, sSubKeyPath, sKeyName, sKeyValue, sMachineName As String

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportRegistrySettings")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Read a row at a time passing in the definition for the row

            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(v_aIeControl(v_iIntegerData)(0)), r_aDataDefinition:=v_aIeTableDefinitions(v_iIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=vRetrievedData, rowIndex:=v_iIntegerData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_bImportRegistry Then
                If Information.IsArray(vRetrievedData) Then

                    sKey = CStr(vRetrievedData(conKeyPath)).Replace(conDMC, v_sDataModelCode)

                    m_lReturn = gPMFunctions.GetSystemName(sSystemName:=sMachineName)

                    ' Replace any occurrences of machine name within the registry key path.
                    sKey = sKey.Replace(conMachineName, sMachineName)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = ExtractName(r_sText:=sKey, r_sSearchChar:=conComma, r_sKeyPath:=sKeyPath, r_sKeyName:=sKeyName)
                    End If

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = ExtractSubKeyPath(r_sFullKeyPath:=sKeyPath, r_sSubKeyPath:=sSubKeyPath)
                    End If

                    sKeyValue = CStr(vRetrievedData(conKeyValue))

                    sKeyValue = sKeyValue.ToUpper().Replace(conMachineName.ToUpper(), sMachineName.ToUpper())
                    'Get the value
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=CStr(vRetrievedData(conKeyName)), v_sSettingValue:=sKeyValue, v_sSubKey:=sSubKeyPath)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ImportRegistrySettings")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportRegistrySettings")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
	End Function
End Module
