Option Strict Off
Option Explicit On
Imports System
Module MainModule
	'********************************************************************************
	'Created By:Arul Stephen
	'Tech Spec:TechSpec WR6ClauseGrouping.doc
	'********************************************************************************
	
	
	Public Const ACApp As String = "iSIRPickDocTemplate"
	
	' Buttons
	Public Const ACYestButton As Integer = 6
	
	'Default Values
	Public Const ACProductTypeId As Integer = 0
	Public Const ACRiskTypeId As Integer = 0
	Public Const ACDefault As Integer = 0
	Public Const ACArrayRowPosition As Integer = 2
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    'Start Arul PN 63627
    Public Const ACisDeleted = 3
    Public Const ACDocumentTypeId = 4
    Public Const ACDocumentTypeCode = 5
    Public Const ACDocumentTypeDecription = 6
    Public Const ACDocumentTypeEffectiveDate = 7
    Public Const ACDocumentMaxId = 7
    'End Arul PN 63627



	
	'
	'Constants for UnderWriting and Broking
	Public Const ACBroking As String = "A"
	Public Const ACUnderWriting As String = "U"
	
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    
    Public g_iSourceID As Integer
    
    Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRFindDocTemplate.Form
    
    Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	Private m_lReturn As Integer
	
	Public Enum ENSelectClause
		Id = 0
		Code = 1
		Description = 2
		Selected = 3
		Default_Renamed = 4
		Branch = 5
	End Enum
	
	Public Enum EnClauseType
		ProductType = 1
		RiskType = 2
	End Enum
	
	Public Enum ENSelectedClauseArray
		Id = 0
		Code = 1
		Description = 2
		Edited = 3
	End Enum
	
	Sub Main_Renamed()
	End Sub
End Module