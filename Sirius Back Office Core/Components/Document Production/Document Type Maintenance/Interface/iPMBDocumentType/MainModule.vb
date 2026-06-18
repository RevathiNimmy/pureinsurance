Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iPMBDocumentType"
	Private Const ACClass As String = "MainModule"
	
    ' Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyId As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRDocumentType.Business
	Public Const ACArrayDocumentTypeID As Integer = 0
	Public Const ACArrayCaptionID As Integer = 1
	Public Const ACArrayCode As Integer = 2
	Public Const ACArrayDescription As Integer = 3
	Public Const ACArrayIsDeleted As Integer = 4
	Public Const ACArrayEffectiveDate As Integer = 5
	Public Const ACArrayIsEditableAfterMerging As Integer = 6
	
	' Constants for the resource file
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACInterface2Title As Integer = 102
	Public Const ACTabTitle2 As Integer = 103
	
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACAddButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACDeleteButton As Integer = 205
	Public Const ACUnDeleteButton As Integer = 206
	Public Const ACEditButton As Integer = 207
	
	Public Const ACCodeLabel As Integer = 300
	Public Const ACDescriptionLabel As Integer = 301
	Public Const ACIsEditableAfterMergingLabel As Integer = 302
End Module