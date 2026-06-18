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
	Public Const ACApp As String = "iPMMsgBox"
	
	
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
		
		'Modified by milan.rawat on 5/14/2010 2:32:35 PM refer developer guide no. 88
		'Dim oPMMsgBox As New ClassInterface
        	Dim oPMMsgBox As New Object
		
		Dim lError As Integer = CType(oPMMsgBox, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		
		oPMMsgBox.CallingAppName = "TEST"
		
		lError = oPMMsgBox.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=gPMConstants.PMTypeOfBusinessGeneric, vEffectiveDate:=DateTime.Now)
		
		Dim vKeyArray(1, 1) As Object
		

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = gPMConstants.PMKeyNameDecisionTitle

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "Risks"

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = gPMConstants.PMKeyNameDecisionText

		vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = "Do you wish to add any more risks?"
		
		lError = oPMMsgBox.SetKeys(vKeyArray)
		
		lError = oPMMsgBox.Start()
		
		If oPMMsgBox.Status = gPMConstants.PMEReturnCode.PMOK Then
		End If
		
		oPMMsgBox.Dispose()
		
		
		'    End
		
	End Sub
End Module