Option Strict Off
Option Explicit On
Imports System
Module NavigatorSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: NavigatorSQL
	'
	' Date: 03/04/1997
	'
	' Description: Contains the Navigator SQL Statements.
	'
	' Edit History:
	' ***************************************************************** '
	' Note: If the column order of any of the following SQL statements
	'       is altered, then the column position enums in Navigator
	'       Constants must be amended to reflect the chanfe.
	' ***************************************************************** '
	
	'SQL Statements
	
	' Process SQL
	' *************
	
	' Get Process ID SQL
	Public Const ACGetProcessIDStored As Boolean = True
    Public Const ACGetProcessIDName As String = "GetProcessID"
    'developer guide no. 39
    Public Const ACGetProcessIDSQL As String = "spu_pmnav_get_process_id"
	
	' Select Process SQL
	Public Const ACSelProcessStored As Boolean = True
    Public Const ACSelProcessName As String = "SelectProcess"
    'developer guide no. 39
    Public Const ACSelProcessSQL As String = "spu_pmnav_process_sel"
	
	' Select Process Keys
	Public Const ACSelProcessSetKeysStored As Boolean = True
    Public Const ACSelProcessSetKeysName As String = "SelectProcessSetKeys"
    'developer guide no. 39
    Public Const ACSelProcessSetKeysSQL As String = "spu_pmnav_set_process_key_sel"
	
	Public Const ACSelProcessGetKeysStored As Boolean = True
    Public Const ACSelProcessGetKeysName As String = "SelectProcessGetKeys"
    'developer guide no. 39
    Public Const ACSelProcessGetKeysSQL As String = "spu_pmnav_get_process_key_sel"
	
	' Map SQL
	' *******
	
	' Select Map SQL
	Public Const ACSelMapStored As Boolean = True
    Public Const ACSelMapName As String = "SelectMap"
    'developer guide no. 39
    Public Const ACSelMapSQL As String = "spu_pmnav_map_sel"
	
	' Select Map Keys
	Public Const ACSelMapSetKeysStored As Boolean = True
    Public Const ACSelMapSetKeysName As String = "SelectMapSetKeys"
    'developer guide no. 39
    Public Const ACSelMapSetKeysSQL As String = "spu_pmnav_set_map_key_sel"
	
	
	' Step SQL
	' ********
	
	' Select Map Steps SQL
	Public Const ACSelMapStepsStored As Boolean = True
    Public Const ACSelMapStepsName As String = "SelectMapSteps"
    'developer guide no. 39
    Public Const ACSelMapStepsSQL As String = "spu_pmnav_map_steps_sel"
	
	' Select Map Steps Set Keys
	Public Const ACSelMapStepsSetKeysStored As Boolean = True
    Public Const ACSelMapStepsSetKeysName As String = "SelectMapStepsSetKeys"
    'developer guide no. 39
    Public Const ACSelMapStepsSetKeysSQL As String = "spu_pmnav_set_step_key_sel"
	
	' Select Map Steps Get Keys
	Public Const ACSelMapStepsGetKeysStored As Boolean = True
    Public Const ACSelMapStepsGetKeysName As String = "SelectMapStepsGetKeys"
    'developer guide no. 39
    Public Const ACSelMapStepsGetKeysSQL As String = "spu_pmnav_get_step_key_sel"
End Module