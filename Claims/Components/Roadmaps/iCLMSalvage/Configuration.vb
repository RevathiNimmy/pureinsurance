Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module Configuration
	
	Private Const ACClass As String = "Configuration"
	
	Private m_lReturn As Integer
	
	' "Roadmap" name
	Public m_sRoadmap As String = ""
	
	' End the map or not?
	Public m_bEndMap As Boolean
	
	' Auto close the map or not?
	Public m_bAutoClose As Boolean
	
	' Navigator or user driven?
	Public m_bNavigatorDriven As Boolean
	
	' CTAF 230502 - Used for SetProcessModes
	Public g_vTransactionType As String = ""
	Public g_vProcessMode As gPMConstants.PMEProcessMode
	
	Public Const ACApp As String = "iCLMSalvage"
	
	' Work Manager Task code - THIS NEEDS TO BE EXACT!!!!!!!!!!!!!!!!!!!!!!
	Public Const ACWMTaskCode As String = "WMCLMSR"
	
	Public Const ACTaskDescription As String = "Salvage"
	
	' Carole Nash web site
	Public Const ACURL As String = "http://www.siriusgroup.co.uk"
	
	' Title of the form
	Public Const ACTitle As String = "Claims"
	
	' ***************** CHANGE THESE (0 based) ************************
	Public Const ACStepFindClaim As Integer = 0
	Public Const ACStepClaimDetails As Integer = 1
	Public Const ACStepSalvageReceipt As Integer = 2
	
	Public Const ACMaxSteps As Integer = 2
	
	' ***************************************************************** '
	'
	' Name: SetupSteps
	'
	' Description: Use this to set up your road map
	'
	' History: 21/08/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function SetupSteps() As Integer
		
		Dim result As Integer = 0
		Dim iMaxSteps As Integer

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Use this function to set up the steps and roadmap name
			' DO NOT USE IT FOR ANYTHING ELSE!!!
			
			' The constants are defined in MainModule in the CHANGE THESE section
			
			' ProcessModes - CTAF These were what they were before
			'                     seems dodgy to me...
			
			g_vTransactionType = gPMConstants.PMTransactionTypeNB
			g_vProcessMode = gPMConstants.PMEProcessMode.PMProcessModeNBLive
			
			' Name of the roadmap
			m_sRoadmap = "Salvage"
			
			' Auto close?
			m_bAutoClose = False
			
			' Navigator or User Driven?
			m_bNavigatorDriven = False
			
			iMaxSteps = ACMaxSteps
			
			ReDim m_vSteps(iMaxSteps)
			
			' Call AddStep for each key
			m_lReturn = AddStep(v_vDescription:="Find Claim", v_vComponent:="iCLMFindClaim.Interface", v_vType:=gPMConstants.PMNavComponentFindForm, v_vOKAction:=gPMConstants.PMNavActionForwardOne, v_vCancelAction:=gPMConstants.PMNavActionAbortProcess, v_vComponentAction:=gPMConstants.PMEComponentAction.PMEdit, v_vServerSide:=False, v_vCreateWorkManagerTask:=False)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lError = gPMConstants.PMEReturnCode.PMTrue
				Return result
			End If
			
			m_lReturn = AddStep(v_vDescription:="Claim Details", v_vComponent:="iOpenClaim.Interface", v_vType:=gPMConstants.PMNavComponentDataForm, v_vOKAction:=gPMConstants.PMNavActionForwardOne, v_vCancelAction:=gPMConstants.PMNavActionAbortProcess, v_vOKSteps:=0, v_vCancelSteps:=0, v_vComponentAction:=gPMConstants.PMEComponentAction.PMEdit, v_vServerSide:=False, v_vCreateWorkManagerTask:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lError = gPMConstants.PMEReturnCode.PMTrue
				Return result
			End If
			
			m_lReturn = AddStep(v_vDescription:="Salvage Receipt", v_vComponent:="iCLMSalvageRecovery.Interface", v_vType:=gPMConstants.PMNavComponentDataForm, v_vOKAction:=gPMConstants.PMNavActionCompleteProcess, v_vCancelAction:=gPMConstants.PMNavActionAbortProcess, v_vOKSteps:=0, v_vCancelSteps:=0, v_vComponentAction:=gPMConstants.PMEComponentAction.PMEdit, v_vServerSide:=False, v_vCreateWorkManagerTask:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lError = gPMConstants.PMEReturnCode.PMTrue
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupSteps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupSteps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: SetupKeys
	'
	' Description: Use this to set initial key values
	'
	' History: 22/08/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function SetupKeys() As Integer
		
		Dim result As Integer = 0

		Try 
			
			
			' Set the keys on the steps here
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: AddStep
	'
	' Description: Adds a new step
	'
	' History: 29/08/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function AddStep(Optional ByVal v_vDescription As Object = Nothing, Optional ByVal v_vComponent As Object = Nothing, Optional ByVal v_vType As Object = Nothing, Optional ByVal v_vOKAction As Object = Nothing, Optional ByVal v_vCancelAction As Object = Nothing, Optional ByVal v_vOKSteps As Object = Nothing, Optional ByVal v_vCancelSteps As Object = Nothing, Optional ByVal v_vComponentAction As Object = Nothing, Optional ByVal v_vServerSide As Object = Nothing, Optional ByVal v_vDefaultKeys As Object = Nothing, Optional ByVal v_vCreateWorkManagerTask As Object = Nothing, Optional ByVal v_vProcessMode As Object = Nothing, Optional ByVal v_vResumeStep As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Static iIndex As Integer
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Description

			If Not Information.IsNothing(v_vDescription) Then


				m_vSteps(iIndex).Description = CStr(v_vDescription)
			End If
			
			' Component

			If Not Information.IsNothing(v_vComponent) Then


				m_vSteps(iIndex).Component = CStr(v_vComponent)
			End If
			
			' Type

			If Not Information.IsNothing(v_vType) Then


				m_vSteps(iIndex).Type = CStr(v_vType)
			End If
			
			' OK Action

			If Not Information.IsNothing(v_vOKAction) Then


				m_vSteps(iIndex).OKAction = CStr(v_vOKAction)
			End If
			
			' OK Steps

			If Not Information.IsNothing(v_vOKSteps) Then


				m_vSteps(iIndex).OKSteps = CInt(v_vOKSteps)
			Else

				m_vSteps(iIndex).OKSteps = 1
			End If
			
			' Cancel Action

			If Not Information.IsNothing(v_vCancelAction) Then


				m_vSteps(iIndex).CancelAction = CStr(v_vCancelAction)
			End If
			
			' Cancel Steps

			If Not Information.IsNothing(v_vCancelSteps) Then


				m_vSteps(iIndex).CancelSteps = CInt(v_vCancelSteps)
			Else

				m_vSteps(iIndex).CancelSteps = 1
			End If
			
			' Component Action

			If Not Information.IsNothing(v_vComponentAction) Then


				m_vSteps(iIndex).ComponentAction = CInt(v_vComponentAction)
			End If
			
			' Server Side

			If Not Information.IsNothing(v_vServerSide) Then


				m_vSteps(iIndex).ServerSide = CBool(v_vServerSide)
			End If
			
			' Default Keys

			If Not Information.IsNothing(v_vDefaultKeys) Then


				m_vSteps(iIndex).DefaultKeys = v_vDefaultKeys
			End If
			
			' Create Work Manager Task

			If Not Information.IsNothing(v_vCreateWorkManagerTask) Then


				m_vSteps(iIndex).CreateWorkManagerTask = CBool(v_vCreateWorkManagerTask)
			Else

				m_vSteps(iIndex).CreateWorkManagerTask = False
			End If
			
			' Resume Step

			If Not Information.IsNothing(v_vResumeStep) Then


				m_vSteps(iIndex).ResumeStep = CInt(v_vResumeStep)
			Else

				m_vSteps(iIndex).ResumeStep = ACResumeStepCurrent
			End If
			
			' Next step
			iIndex += 1
			
			Return result
		
	End Function
End Module

