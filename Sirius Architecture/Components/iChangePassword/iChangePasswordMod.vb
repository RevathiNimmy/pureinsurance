Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 21/01/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iChangePassword"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	
	Public Const ACTitle As Integer = 101
	Public Const ACOldPassword As Integer = 102
	Public Const ACNewPassword As Integer = 103
	Public Const ACConfirmPassword As Integer = 104
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	
	' Messages
	Public Const ACOldPasswordFailTitle As Integer = 300
	Public Const ACOldPasswordFail As Integer = 301
	Public Const ACConfirmPasswordFailTitle As Integer = 302
	Public Const ACConfirmPasswordFail As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	

	Public Sub Main()
		
		' Main entry point for the component.
		
		'    test
		
	End Sub
	
	Sub test()
		
        'Developer Guide No : 88
        Dim oChangePassword As New Interface_Renamed
		
		oChangePassword.LanguageID = 1
		oChangePassword.OldPassword = "simonb"
		
		Dim lError As Integer = oChangePassword.Start()
		
		If oChangePassword.Cancelled Then
			MessageBox.Show("Cancelled", Application.ProductName)
		Else
			MessageBox.Show("New Password: " & oChangePassword.NewPassword, Application.ProductName)
		End If
		
		
		'    End
		
	End Sub

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module