Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 06/07/2004
	'
	' Description: Main module containing public variable/constants.
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMURIPortfolioTransfer"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Form
	' Buttons
	
	' Messages
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACStatusSearching As Integer = 304
	Public Const ACStatusFound As Integer = 305
	Public Const ACAmendTitle As Integer = 315
	Public Const ACAmendProcessFail As Integer = 319
	
	Public Const ACCmdFind As Integer = 320
	Public Const ACCmdTransfer As Integer = 321
	Public Const ACCmdCancel As Integer = 322
	Public Const ACLblProduct As Integer = 323
	Public Const ACLblTransferDate As Integer = 324
	Public Const ACLblPolicyNumber As Integer = 325
	Public Const ACLblClientCode As Integer = 326
	Public Const ACLblClientName As Integer = 327
	Public Const ACFormCaption As Integer = 328
	Public Const ACFrame1 As Integer = 329
	Public Const ACFrame2 As Integer = 330
	
	Public Const ACConfirm1 As Integer = 331
	Public Const ACConfirm2 As Integer = 332
	
	Public Const ACDateColumn As Integer = 2
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsuranceFileCnt As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module