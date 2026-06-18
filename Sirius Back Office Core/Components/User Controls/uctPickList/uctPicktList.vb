Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  04/06/1998
	'
	' Description: Main Module.
	'
	' Edit History: TF040698 - Created
	' RAW 10/04/2003 : ENDVR633 : removed NEW clause from g_oObjectManager DIM statement
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctPickList"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' Message Texts
	Public Const ACCancelDetailsTitleText As Integer = 300
	Public Const ACCancelDetailsText As Integer = 301
	Public Const ACBusinessFailTitleText As Integer = 302
	Public Const ACBusinessFailText As Integer = 303
	
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
    
    Public g_iCountryId As Integer
	
	' Public instance of the object manager.
    ' RAW 10/04/2003 : ENDVR633 : removed NEW clause
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module