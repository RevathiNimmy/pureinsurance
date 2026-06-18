Option Strict Off
Option Explicit On
Imports System
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
	Public Const ACApp As String = "iPMScreenMessage"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	
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
		
		'''Dim oPMDecision As iPMDecision.Interface
		'''Dim lError As Long
		'''
		'''    Set oPMDecision = New iPMDecision.Interface
		'''
		'''    lError& = oPMDecision.Initialise()
		'''
		'''    oPMDecision.CallingAppName = "TEST"
		'''
		'''    lError& = oPMDecision.SetProcessModes( _
		''''        vTask:=PMView, _
		''''        vNavigate:=PMNavigateDisabled, _
		''''        vProcessMode:=PMProcessModeGeneric, _
		''''        vTransactionType:=PMTransactionTypeGeneric, _
		''''        vEffectiveDate:=Now)
		'''
		'''    ReDim vKeyArray(1, 1)
		'''
		'''    vKeyArray(PMKeyName, 0) = PMKeyNameDecisionTitle
		'''    vKeyArray(PMKeyValue, 0) = "Risks"
		'''    vKeyArray(PMKeyName, 1) = PMKeyNameDecisionText
		'''    vKeyArray(PMKeyValue, 1) = "Do you wish to add any more risks?"
		'''
		'''    lError& = oPMDecision.SetKeys(vKeyArray)
		'''
		'''    lError& = oPMDecision.Start()
		'''
		'''    If (oPMDecision.Status = PMOK) Then
		'''    End If
		'''
		'''    lError& = oPMDecision.Terminate()
		'''
		'''    Set oPMDecision = Nothing
		'''
		'    End
		
	End Sub
End Module