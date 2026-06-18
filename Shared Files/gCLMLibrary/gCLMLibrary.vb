Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("gCLMLibrary_NET.gCLMLibrary")> _
 Public Module gCLMLibrary
	' ***************************************************************** '
	' Module Name: gCLMLibrary
	'
	' Date: 10th January 2003
	'
	' Description: Created to store all general functions for Claims
	'
	' Edit History:
	'   AMB - 10/01/03 - Created
	' ***************************************************************** '
	
	Private Const ACClass As String = "gCLMLibrary"
    'Modified by Alkesh Kumar on 10/05/2010 17:11:58 refer developer guide no. 
    Public g_iSourceID As Integer
	'**********************************************************************
	' Function Name:    GetEnableClaimVersions
	' Author:           Alix Bergeret
	' Date:             20/12/2002
	' Description:      -
	'**********************************************************************
	Public Function GetEnableClaimVersions() As Boolean
        Dim result As Boolean = False
        Dim vResult As Object = Nothing
		
		Try 
			
			result = True
			
			iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableClaimVersions, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vResult))
			
			
			Return vResult = 1
		
		Catch 
		End Try
		
		
		
		Return False
		
	End Function
	
	'**********************************************************************
	' Function Name:    GetMaxVCV
	' Author:           Alix Bergeret
	' Date:             18/12/2002
	' Description:      -
	'**********************************************************************
	Public Function GetMaxVCV(ByRef r_sValue As String) As Integer
		
		Dim result As Integer = 0
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		Dim m_lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
			eProductFamily = g_sProductFamily
			eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
			
			m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="MaxVCV", r_sSettingValue:=r_sValue), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_sValue.Trim() = "" Then
				
				' default
				r_sValue = "2"
				
				' If key not in registry, add it
				m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="MaxVCV", v_sSettingValue:=r_sValue), gPMConstants.PMEReturnCode)
				
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	'**********************************************************************
	' Function Name:    GetCurrVCV
	' Author:           Alix Bergeret
	' Date:             18/12/2002
	' Description:      -
	'**********************************************************************
	Public Function GetCurrVCV(ByRef r_sValue As String) As Integer
		
		Dim result As Integer = 0
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		Dim m_lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
			eProductFamily = g_sProductFamily
			eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
			
			m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="CurrVCV", r_sSettingValue:=r_sValue), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_sValue.Trim() = "" Then
				
				' default
				r_sValue = "0"
				
				' If key not in registry, add it
				m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="CurrVCV", v_sSettingValue:=r_sValue), gPMConstants.PMEReturnCode)
				
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	'**********************************************************************
	' Function Name:    SetCurrVCV
	' Author:           Alix Bergeret
	' Date:             18/12/2002
	' Description:      -
	'**********************************************************************
	Public Function SetCurrVCV(ByRef v_sValue As String) As Integer
		
		Dim result As Integer = 0
		Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
		Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
		Dim eProductFamily As gPMConstants.PMEProductFamily
		Dim m_lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
			eProductFamily = g_sProductFamily
			eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
			
			m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="CurrVCV", v_sSettingValue:=v_sValue), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Module