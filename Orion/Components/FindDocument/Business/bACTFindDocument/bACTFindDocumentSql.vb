Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Module FindDocumentSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: FindDocumentSQL
	'
	' Date: 18th August 1997
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindDocument
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	' Select FindDocument by DocumentRef SQL
	Public Const ACDocumentLikeDocumentRefStored As Boolean = True
	Public Const ACDocumentLikeDocumentRefName As String = "FindDocumentLikeDocumentRef"
    'developer guide no. 39
    Public Const ACDocumentLikeDocumentRefSQL As String = "spu_ACT_Do_FindDocLikeDocRef"
	
	'' Select Document_id_from_DocumentRef SQL
	'Public Const ACDocumentFromShortNameStored = True
	'Public Const ACDocumentFromShortNameName = "SelectDocumentFromDocRef"
	'Public Const ACDocumentFromShortNameSQL = "{call spu_ACT_GetIdFromDocRef (?, ?, ?)}"
	'
	''select Select Document_id_from_DocumentRef SQL
	'Public Const ACDocumentFromCntStored = True
	'Public Const ACDocumentFromCntName = "SelectDocumentFromId"
	'Public Const ACDocumentFromCntSQL = "{call spu_ACT_Do_GetDocRefFromId(?, ?)}"
	
	'select Document details fromquery
	Public Const ACDocumentFromQueryStored As Boolean = False
	Public Const ACDocumentFromQueryName As String = "SelectDocumentFromQuery"
	Public Const ACDocumentFromQuerySQL As String = "{}"
  
End Module