Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/07/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTBank"
	Public Const AccountImage As String = "AccountImage"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACLabTitle1 As Integer = 104
	Public Const ACLabTitle2 As Integer = 105
	Public Const ACLabTitle3 As Integer = 106
	Public Const ACLabTitle4 As Integer = 107
	Public Const ACLabTitle5 As Integer = 108
	Public Const ACLabTitle6 As Integer = 109
	Public Const ACLabTitle7 As Integer = 110
	Public Const ACLabTitle8 As Integer = 111
	Public Const ACLabTitle9 As Integer = 112
	Public Const ACLabTitle10 As Integer = 113
	Public Const ACLabTitle11 As Integer = 114
	Public Const ACLabTitle12 As Integer = 115
	Public Const ACLabTitle13 As Integer = 116
	Public Const ACLabTitle14 As Integer = 117
	Public Const ACLabTitle15 As Integer = 118
	Public Const ACLabTitle16 As Integer = 119
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACHeadOfficeButton As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
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
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCompanyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
    End Sub

End Module