Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 02/02/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRRiskGroup.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select All Risk By Source SQL
    'Developer Guide No 39. 
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllRiskBySource"
    'Public Const ACGetAllDetailsSQL As String = "{call spu_Risk_by_source_sel (?)}"
    Public Const ACGetAllDetailsSQL As String = "spu_Risk_by_source_sel"
	' Delete All Risk By Source SQL
	Public Const ACDeleteAllDetailsStored As Boolean = True
	Public Const ACDeleteAllDetailsName As String = "DeleteAllRiskBySource"
    Public Const ACDeleteAllDetailsSQL As String = "spu_Risk_by_source_del"
	
	' Insert Risk By Source SQL
	Public Const ACInsertDetailsStored As Boolean = True
	Public Const ACInsertDetailsName As String = "DeleteAllRiskBySource"
    Public Const ACInsertDetailsSQL As String = "spu_Risk_by_source_add"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckSIRRiskGroupID"
    Public Const ACCheckIDSQL As String = "spe_SIRRiskGroup_check_id"
	
	'CT 01/11/00 bring back details from risk_group
	' Select All Risk By Source SQL
	Public Const ACGetRiskGroupStored As Boolean = True
	Public Const ACGetRiskGroupName As String = "GetRiskGroup"
    Public Const ACGetRiskGroupSQL As String = "spu_Risk_Group_sel"
	
	'CT 02/11/00 update and insert records in risk_group table
	'FSA Phase IV Extra parameter
	'2005 Midnight Renewal Extra Parameter
	'Datasure Extra parameter Country_id
    Public Const ACAddSQL As String = "spu_risk_group_add"
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "RiskGroupAdd"
	
	'FSA Phase IV Extra parameter
	'2005 Midnight Renewal Extra Parameter
	'Datasure Extra parameter Country_id
    Public Const ACUpdateSQL As String = "spu_risk_group_update"
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "RiskGroupUpdate"
	
	'CT 02/11/00 get new caption id or eturn if it exists already
    Public Const ACGetCaptionSQL As String = "spu_pm_caption_id_return"
	Public Const ACGetCaptionStored As Boolean = True
	Public Const ACGetCaptionName As String = "GetCaptionID"
	
	'eck130901
	'Select Risk Renewal Days
	Public Const ACGetRiskRenewalDaysStored As Boolean = True
	Public Const ACGetRiskRenewalDaysName As String = "GetRiskRenewalDays"
    Public Const ACGetRiskRenewalDaysSQL As String = "spu_Risk_Renewal_days_sel"
	
	' Delete All Risk Renewal Days SQL
	Public Const ACDeleteRiskRenewalDaysStored As Boolean = True
	Public Const ACDeleteRiskRenewalDaysName As String = "DeleteAllRiskRenewalDays"
    Public Const ACDeleteRiskRenewalDaysSQL As String = "spu_Risk_Renewal_days_del"
	
	' Insert Risk Renewal Days SQL
	Public Const ACInsertRiskRenewalDaysStored As Boolean = True
	Public Const ACInsertRiskRenewalDaysName As String = "DeleteAllRiskRenewalDays"
    Public Const ACInsertRiskRenewalDaysSQL As String = "spu_Risk_Renewal_days_add"
	
	' Select Risk Screens
	'Public Const ACGetAllScreensSQL = "{call spu_GIS_Screen_sel}"
	' SET 04112002 - calling wrong stored procedure
    Public Const ACGetAllScreensSQL As String = "spu_GIS_Screen_saa2"
	
	' Get the ID of the Risk Screen
	' CTAF 20020730 - Added missing ?
    Public Const ACGetRiskScreensID As String = "spu_Get_Risk_Screen_Id"
	
	' Get the Details
    Public Const ACGetSourceSQL As String = "spu_PMB_Source_Sel"
	
	'Get Commission Accounts
    Public Const ACGetCommissionSQL As String = "spu_PMB_Get_Commission_Account"
End Module