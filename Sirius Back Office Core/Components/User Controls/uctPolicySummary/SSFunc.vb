Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("SSfunc_NET.SSfunc")> _
 Public Module SSfunc
	
	Public Const HelpContext As Integer = 1
	
	Private Const ACClass As String = "SSFunc"
	
	
	
	'******************************************************************************
	' ShowHelp
	'
	'******************************************************************************

	Public Function ShowHelp(ByRef dlgHelp As Object, ByRef lContextID As Integer) As Integer
		
		Dim result As Integer = 0
		Dim sHelpFile As String = ""
		Dim m_lReturn As gPMConstants.PMEReturnCode
		
		
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		
		Try 
			eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
			eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
			eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
			
			'MsgBox sValue
			m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegsettinglevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("IT DONT WORK", Application.ProductName)
				Return result
			End If
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Fire up the Common Dialog Control Help with the specified context ID
			
			
			With dlgHelp
				

				.HelpFile = sHelpFile

				.HelpCommand = HelpContext

				.HelpContext = lContextID

				.result()
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in ShowHelp", vApp:="", vClass:=ACClass, vMethod:="ShowHelp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
