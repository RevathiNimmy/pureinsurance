Option Strict Off
Option Explicit On
Imports System
Module AutoNumberSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: AutoNumberSQL
	'
	' Date: 20th August 1997
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) create a unique id
	'
	' Edit History: 20/08/1997  Original created
	' ***************************************************************** '
	
	'SQL Statements
	
	' Get a pool number
	Public Const ACSelectPoolNumberStored As Boolean = True
	Public Const ACSelectPoolNumberName As String = "AllocatePoolNumber"
    'eck170500

    'Developer Guide No 39
    Public Const ACSelectPoolNumberSQL As String = "spe_ACTNumber_pool_sel"
	
	' Delete pool number
	Public Const ACDeletePoolNumberStored As Boolean = True
	Public Const ACDeletePoolNumberName As String = "DeletePoolNumber"
    'eck170500

    'Developer Guide No 39
    Public Const ACDeletePoolNumberSQL As String = "spe_ACTNumber_pool_del"
	
	' Update User ID & Date for Number stored in the pool
	Public Const ACUpdateNumberStored As Boolean = True
	Public Const ACUpdateNumberName As String = "UpdateNumber"
    'eck170500

    'Developer Guide No 39
    Public Const ACUpdateNumberSQL As String = "spe_ACTnumber_upd"
	
	
	' Allocate New Number
	Public Const ACAllocateNumberStored As Boolean = True
	Public Const ACAllocateNumberName As String = "AllocateNumber"
    'eck170500

    'Developer Guide No 39
    Public Const ACAllocateNumberSQL As String = "spe_ACTnumber_add"
	
	' Add a pool number
	Public Const ACAddPoolNumberStored As Boolean = True
	Public Const ACAddPoolNumberName As String = "AddPoolNumber"
    'eck170500

    'Developer Guide No 39
    Public Const ACAddPoolNumberSQL As String = "spe_ACTNumber_pool_add"
	
	' GetNumberRageID SQL...
	Public Const ACGetNumberRangeIDStored As Boolean = True
    Public Const ACGetNumberRangeIDName As String = "GetNumberRangeID"

    'Developer Guide No 39
    Public Const ACGetNumberRangeIDSQL As String = "spu_ACT_get_number_range"
	
	'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
	Public Const ACAllocateUniqueNumberStored As Boolean = True
	Public Const ACAllocateUniqueNumberName As String = "AllocateUniqueNumber"
	Public Const ACAllocateUniqueNumberSQL As String = "spu_ACT_Generate_Next_Unique_Document_Reference"
	
	Public Const ACGetNumberRangeFromCodeStored As Boolean = True
	Public Const ACGetNumberRangeFromCodeName As String = "ACGetNumberRangeFromCode"
	Public Const ACGetNumberRangeFromCodeSQL As String = "spu_ACT_Get_Number_Range_From_Code"
	'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
End Module