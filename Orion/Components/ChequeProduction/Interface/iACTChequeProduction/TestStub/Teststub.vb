Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Module modTest
	
	Public Sub Main()
		Test()
	End Sub
	
	Sub Test()
		Dim vKeyArray As Object
		
		Dim oChequeProduction As New iACTChequeProduction.NavigatorV3
		
		Dim lError As Integer = CType(oChequeProduction, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		
		oChequeProduction.NavigatorV3_CallingAppName = "TEST"
		
		
		
		
		lError = oChequeProduction.NavigatorV3_SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
		
		
		lError = oChequeProduction.NavigatorV3_Start()
		
		Dim lStatus As Integer = oChequeProduction.NavigatorV3_Status
		
		
		oChequeProduction = Nothing
		
		Environment.Exit(0)
		
	End Sub
End Module