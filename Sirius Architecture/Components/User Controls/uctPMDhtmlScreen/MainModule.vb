Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  02/03/1999
	'
	' Description: Main Module.
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "DhtmlDataScreen"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
	'Name of default URL
	Public Const ACDefaultURL As String = "C:\"
	
	'Name of default NavigateOnShow setting
	Public Const ACNavigateOnShow As Boolean = False
	
	'Name of default Silent Setting
	Public Const ACSilent As Boolean = False
	
	'Name of default Context Menu Setting
	Public Const ACContextMenu As Boolean = True
	
	'Name of default TimeOut Setting
	Public Const ACTimeOut As Integer = 60
	
	'Name of the main frame
	Public Const ACMainFrame As String = "Main"
	
	'Submit and Reset Buttons
	Public Const ACSubmitButton As String = "SUBMIT"
	Public Const ACResetButton As String = "RESET"
	
	'***************************************************************************************
	'DHTML Control Type Constants
	Public Const ACTextControl As String = "text"
	Public Const ACCheckboxControl As String = "checkbox"
	Public Const ACRadioControl As String = "radio"
	Public Const ACSelectOne As String = "select-one"
	Public Const ACSelectMultiple As String = "select-multiple"
	Public Const ACPasswordControl As String = "password"
	Public Const ACButtonControl As String = "button"
	Public Const ACImageControl As String = "Image"
	Public Const ACTextAreaControl As String = "textarea"
	Public Const ACSubmitControl As String = "submit"
	Public Const ACResetControl As String = "Reset"
	Public Const ACTableControl As String = "Table"
	Public Const ACActiveXControl As String = "ActiveX"
End Module