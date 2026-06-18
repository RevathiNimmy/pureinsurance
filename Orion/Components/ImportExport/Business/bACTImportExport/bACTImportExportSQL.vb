Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	' ***************************************************************** '
	
	
	' Table: RI Model
	' Delete
	Public Const ACDeleteRIModelStored As Boolean = True
	Public Const ACDeleteRIModelName As String = "DeleteRIModel"
	Public Const ACDeleteRIModelSQL As String = "spu_RI_Model_del"
	
	' Insert
	Public Const ACInsertRIModelStored As Boolean = True
	Public Const ACInsertRIModelName As String = "InsertRIModel"
	Public Const ACInsertRIModelSQL As String = "spu_RI_Model_add"
	
	' Select
	Public Const ACSelectRIModelStored As Boolean = True
	Public Const ACSelectRIModelName As String = "SelectRIModel"
	Public Const ACSelectRIModelSQL As String = "spu_RI_Model_saa"
	
	' Update
	Public Const ACUpdateRIModelStored As Boolean = True
	Public Const ACUpdateRIModelName As String = "InsertRIModel"
	Public Const ACUpdateRIModelSQL As String = "spu_RI_Model_upd"
	
	
	
	' Table: RI Model Line
	' Delete
	Public Const ACDeleteRIModelLineStored As Boolean = True
	Public Const ACDeleteRIModelLineName As String = "DeleteRIModelLine"
	Public Const ACDeleteRIModelLineSQL As String = "spu_RI_Model_Line_del"
	
	' Insert
	Public Const ACInsertRIModelLineStored As Boolean = True
	Public Const ACInsertRIModelLineName As String = "InsertRIModelLine"
	Public Const ACInsertRIModelLineSQL As String = "spu_RI_Model_Line_add"
	
	' Select
	Public Const ACSelectRIModelLineStored As Boolean = True
	Public Const ACSelectRIModelLineName As String = "SelectRIModelLine"
    Public Const ACSelectRIModelLineSQL As String = "spu_RI_Model_Line_saa"

    ' Select
    Public Const ACSelectPeriodStored As Boolean = True
    Public Const ACSelectPeriodName As String = "SelectAllPeriod"
    Public Const ACSelectPeriodSQL As String = "Spu_Select_AllperiodEnddates"

    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllLedger"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_Ledger"

    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAllPeriod"
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Period"

    Public Const ACGetBranchDetailsStored As Boolean = True
    Public Const ACGetBranchDetailsName As String = "SelectSource"
    Public Const ACGetBranchDetailsSQL As String = "spu_PM_Select_Source"

End Module