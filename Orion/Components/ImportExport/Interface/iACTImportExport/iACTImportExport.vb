Option Strict Off
Option Explicit On

Imports System

Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iACTImportExport"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' System option number
	Public Const ACExportPathOption As Integer = 5015
	Public Const ACImportPathOption As Integer = 5013
	Public Const ACImportedPathOption As Integer = 5014
    Public Const ACGenericMessagePathOption As Integer = 5056
    Public Const ACGLExportPeriodALL As Integer = 5259
	
	' Enum header for import export file information
	Public Enum ACImportExportFileInfoEnum
		ACIEFilename
		ACIEDate
		ACIEInterface
		ACIEReference
		ACIERecords
		ACIEMax = ACImportExportFileInfoEnum.ACIERecords
	End Enum
	
	
	
	' ***************************************************************** '
	'                        GLOBAL VARIABLES
	' ***************************************************************** '
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
    Public g_sUsername As String
    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module