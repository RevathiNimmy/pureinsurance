Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "iPMBMaintainRelation"
	Private Const ACClass As String = "MainModule"
	'CMG/PB 09/07/2002
	Public Const SIRLookupPartyRelationshipGroup As String = "Party_Relationship_Group"
	
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCurrencyId As Integer
    'Modified,add the g_sProductFamily for helpfile
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRMaintainRelation.Business
	
	Public Const ACArrayRelationShipTypeID As Integer = 0
	Public Const ACArrayCaptionID As Integer = 1
	Public Const ACArrayCode As Integer = 2
	Public Const ACArrayDescription As Integer = 3
	Public Const ACArrayIsDeleted As Integer = 4
	Public Const ACArrayEffectiveDate As Integer = 5
	Public Const ACArrayComplementaryTypeID As Integer = 6
	Public Const ACArrayPartyRelationshipGroupID As Integer = 7
	
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
	Public Const ACCodeLabel As Integer = 208
	Public Const ACDescriptionLabel As Integer = 209
    Public Const ACPartnerLabel As Integer = 210
    'Modified,add it as per vb code at the place of helpcontextid
    Public Const ScreenHelpID As Integer = 9005
End Module