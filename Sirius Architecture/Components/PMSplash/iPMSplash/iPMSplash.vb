Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 13/12/1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMSplash"
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
		' Main entry point for the component
		
		'    test
		
	End Sub
	
	Sub test()
		
        'developer guide no. 88
        Dim oPMSplash As New iPMSplash.Interface_Renamed
		oPMSplash.CallingAppName = "TEST APP"
		oPMSplash.TitleName = "Insurance Navigator"
		
		Dim lErrorValue As Integer = oPMSplash.Start()
		
		lErrorValue = oPMSplash.Finish()
		
		'   End
		
	End Sub
End Module