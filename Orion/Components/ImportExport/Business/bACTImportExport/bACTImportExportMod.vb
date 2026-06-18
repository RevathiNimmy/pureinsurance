Option Strict Off
Option Explicit On
Imports System
Imports System.Xml
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "bACTImportExport"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	Private Const ACIEIMediaTypeCode As String = "MEDIA_TYPE_CODE"
	Private Const ACIEIBanksAccountName As String = "BANK_ACCOUNT_NAME"
	Private Const ACIEIAutoPost As String = "AUTOPOST"
	Private Const ACIEILeadDays As String = "LEAD_DAYS"
	Private Const ACIEIBatchID As String = "BATCH_ID"
	
	
	' Enum header for import export file information
	Public Enum ACImportExportFileInfoEnum
		ACIEFilename
		ACIEDate
		ACIEInterface
		ACIEReference
		ACIERecords
		ACIEMax = ACImportExportFileInfoEnum.ACIERecords
	End Enum
	
	
	' Get an attribute value if we can
	Public Function TryGetAttribute(ByVal oNode As XmlElement, ByVal sAttribute As String, Optional ByVal vDefault As String = "") As String
		
		Try 

			Return CStr(oNode.GetAttribute(sAttribute))
		
		Catch 
			
			Return vDefault
		End Try
	End Function
	
	
	' Set an attribute value if we can, if not create it and set it
	Public Sub TrySetAttribute(ByVal oNode As XmlElement, ByVal sAttribute As String, ByVal vValue As String)
		
		Try 
			oNode.SetAttribute(sAttribute, vValue)
		
		Catch 
		End Try
		
	End Sub
End Module