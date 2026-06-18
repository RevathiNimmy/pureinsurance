Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
    'developer guide no. 39
    'start
	Private Const ACClass As String = "BusinessSQL"
	
    Public Const ACAddSQL As String = "spu_Contact_Type_add"
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "ContactTypeAdd"
	
    Public Const ACUpdateSQL As String = "spu_Contact_Type_update"
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "ContactTypeUpdate"
	
    Public Const ACGetCaptionSQL As String = "spu_pm_caption_id_return"
	Public Const ACGetCaptionStored As Boolean = True
	Public Const ACGetCaptionName As String = "GetCaptionID"
End Module