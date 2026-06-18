Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Module MainModule
	'***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {17/2/98}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	'               JH021198 - moved setting_type declaration
	'               + 2 registry constants to DOCGeneralFunc
	'               for use by other modules
	'               JH051198 - added MaxAutoExpand to Manager_Options
	'               for enhanced folder processing
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCOptions"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' RDC 23062005
	' database V2 option names (for table doc_options)
	Public Const OPTIONS_VIEWER_ALLOW_CUT_PASTE As String = "ALLOW_COPY_PASTE"
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Documaster access level
	Public g_iDOCaccesslevel As Integer
	
	'Start folders for search
	Public g_lStartFoldNum As Integer
	Public g_sStartFoldName As String = ""
	
	'store found docs
	Public g_vDocsFound As Object
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	Private m_lReturn As Integer
	
	Public g_bUserIsAdmin As Boolean
	
	' Setting can be either database or registry
	Public Const SOURCE_DATABASE As Integer = 1
	Public Const SOURCE_REGISTRY As Integer = 2
	' RDC 23062005
	Public Const SOURCE_DBASEV2 As Integer = 3
	
	Public Const DOC_DEFAULT_DOCDATE As String = "2"
	Public Const DOC_DEFAULT_EXPIRYDATE As String = "365"
	
	' manager options
	Public Structure ManagerOptions_Type
		Dim MaxFolders As DOCGeneralFunc.Setting_Type
		Dim MaxFilters As DOCGeneralFunc.Setting_Type
		Dim MaxAutoExpand As DOCGeneralFunc.Setting_Type 'JH051198
		Dim StartHome As DOCGeneralFunc.Setting_Type
		Dim DisplayFolders As DOCGeneralFunc.Setting_Type
		Dim WANOptimise As DOCGeneralFunc.Setting_Type 'JH301198
		Dim ShowKeyWords As DOCGeneralFunc.Setting_Type
		Dim ShowAnnotations As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As ManagerOptions_Type
			Dim result As New ManagerOptions_Type
			result.MaxFolders = Setting_Type.CreateInstance()
			result.MaxFilters = Setting_Type.CreateInstance()
			result.MaxAutoExpand = Setting_Type.CreateInstance()
			result.StartHome = Setting_Type.CreateInstance()
			result.DisplayFolders = Setting_Type.CreateInstance()
			result.WANOptimise = Setting_Type.CreateInstance()
			result.ShowKeyWords = Setting_Type.CreateInstance()
            result.ShowAnnotations = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' RDC 23062005
	Public Structure ViewerOptions_Type
		Dim AllowCopyPaste As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As ViewerOptions_Type
			Dim result As New ViewerOptions_Type
			result.AllowCopyPaste = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' configuration options
	Public Structure Config_Type
		Dim CacheLocation As DOCGeneralFunc.Setting_Type
		'JH281098 just have one string for path
		'JH051198 but keep the server and directory settings in case of upgrade
		Dim DocuServer As DOCGeneralFunc.Setting_Type
		Dim DocuShare As DOCGeneralFunc.Setting_Type
		Dim DocuDir As DOCGeneralFunc.Setting_Type
		'JH281098 word options + WAN
		Dim ViewWord As DOCGeneralFunc.Setting_Type
		Dim PrintWord As DOCGeneralFunc.Setting_Type
		Dim AutoKeyword As DOCGeneralFunc.Setting_Type ' MS250900
		Public Shared Function CreateInstance() As Config_Type
			Dim result As New Config_Type
			result.CacheLocation = Setting_Type.CreateInstance()
			result.DocuServer = Setting_Type.CreateInstance()
			result.DocuShare = Setting_Type.CreateInstance()
			result.DocuDir = Setting_Type.CreateInstance()
			result.ViewWord = Setting_Type.CreateInstance()
			result.PrintWord = Setting_Type.CreateInstance()
			result.AutoKeyword = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' document options
	Public Structure Document_Type
		Dim DocumentData As DOCGeneralFunc.Setting_Type
		Dim DocumentExpiry As DOCGeneralFunc.Setting_Type
		Dim ImageViewer As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As Document_Type
			Dim result As New Document_Type
			result.DocumentData = Setting_Type.CreateInstance()
			result.DocumentExpiry = Setting_Type.CreateInstance()
			result.ImageViewer = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' warning options
	Public Structure Warning_Type
		Dim ScanExternal As DOCGeneralFunc.Setting_Type
		Dim MoveDocument As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As Warning_Type
			Dim result As New Warning_Type
			result.ScanExternal = Setting_Type.CreateInstance()
			result.MoveDocument = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' declare all the options
	Public g_ManagerOptions As ManagerOptions_Type = ManagerOptions_Type.CreateInstance()
	' RDC 23062005
	Public g_ViewerOptions As ViewerOptions_Type = ViewerOptions_Type.CreateInstance()
	Public g_ConfigOptions As Config_Type = Config_Type.CreateInstance()
	Public g_DocumentOptions As Document_Type = Document_Type.CreateInstance()
	Public g_WarningOptions As Warning_Type = Warning_Type.CreateInstance()
	
	Public g_bOKPressed As Boolean
	
	Sub Main_Renamed()
		
	End Sub
End Module