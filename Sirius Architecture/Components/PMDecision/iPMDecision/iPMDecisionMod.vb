Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 25/04/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMDecision"
	Public Const PMKeyNameDisplayClaimDocMessage As String = "display_claim_doc_message"
	Public Const PMKeyNameClaimWorkflowId As String = "claim_workflow_id"
	Public Const PMKeyNameDecisionResult As String = "decision_result"
	Public Const PMKeyNameDisplayMakeFurtherPayments As String = "display_make_further_payments"
	'Start - Sankar - PN 61432
	Public Const PMKeyNameClaimClosed As String = "claim_closed"
	'End - Sankar - PN 61432
	
	'Public Const PMKeyNameDisplayClaimDocMessage = "display_claim_doc_message"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	
	' Messages
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	Sub Main_Renamed()
		
		' Main entry point for the component.
		
		'    test
		
	End Sub
	
	Sub test()
		
        'Developer Guide No. 88
        Dim oPMDecision As New Interface_Renamed
		
		Dim lError As Integer = CType(oPMDecision, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		
		oPMDecision.CallingAppName = "TEST"
		
		lError = oPMDecision.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=gPMConstants.PMTransactionTypeGeneric, vEffectiveDate:=DateTime.Now)
		
		Dim vKeyArray(1, 1) As Object
		

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = gPMConstants.PMKeyNameDecisionTitle

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "Risks"

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = gPMConstants.PMKeyNameDecisionText

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = "Do you wish to add any more risks?"
		
		lError = oPMDecision.SetKeys(vKeyArray)
		
		lError = oPMDecision.Start()
		
		If oPMDecision.Status = gPMConstants.PMEReturnCode.PMOK Then
		End If
		
		oPMDecision.Dispose()
		
		
		'    End
		
	End Sub
End Module