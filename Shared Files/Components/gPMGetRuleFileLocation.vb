Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("gPMGetRuleFileLocation_NET.gPMGetRuleFileLocation")> _
 Public Module gPMGetRuleFileLocation
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Function Name : GetRuleFileLocation
	' Date Created  : 30-JAN-2001
	' Author        : Ram Chandrabose
	' Description   : Method to Fetch the Rule File Location from the
	'                 Registry
	'               : uses data model code and optionaly business type code to build the key
	' Edit History  :
	' Note          : This Function should be replaced to get data from
	'                 the database of other source. NOT from Registry
	' ***************************************************************** '
	Public Function GetRuleFileLocation(ByVal v_sGisBusinessTypeCode As String, ByVal v_sDataModelCode As String) As String
		
		Dim result As String = String.Empty
		Dim sSubKey As String = ""
		
		Try 
			
			v_sGisBusinessTypeCode = v_sGisBusinessTypeCode.Trim()
			sSubKey = GISSharedConstants.ACOIMGISSubKey & "\"
			If v_sGisBusinessTypeCode <> "" Then
				sSubKey = sSubKey & v_sGisBusinessTypeCode & "\"
			End If
			sSubKey = sSubKey & v_sDataModelCode.Trim() ' CL120899
			
			' CTAF 021101 - Changed from RuleFilePath to RulePath. (auth. by BSJ)
			gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", r_sSettingValue:=result, v_sSubKey:=sSubKey)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = ""
			
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Rule File Location Failed", vApp:=ACApp, vClass:="", vMethod:="GetRuleFileLocation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			
		End Try
	End Function
End Module