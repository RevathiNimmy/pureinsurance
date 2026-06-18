Option Strict Off
Option Explicit On
Imports System
'Modified by Vijay Pal on 5/31/2010 5:17:23 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "PMCurrencyMaintenance"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	

	Public Sub Main()
		
        'Dim oObject As iPMCurrencyMaintenance.Interface
		
		'Set oObject = New iPMCurrencyMaintenance.Interface

        Dim oObject As Object
        oObject = CreateLateBoundObject("iPMCurrencyMaintenance.Interface_Renamed")

        'Dim oObject As Object = New iPMCurrencyMaintenance.Interface_Renamed

        'developer guide no.9
        'Dim lReturn As gPMConstants.PMEReturnCode = CType(oObject, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        Dim lReturn As gPMConstants.PMEReturnCode = oObject.Initialise()
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			oObject = Nothing
			Exit Sub
		End If
		
		lReturn = oObject.Start()
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			oObject = Nothing
			Exit Sub
		End If
		
		oObject.Dispose()
		
        oObject = Nothing
		
	End Sub
End Module
