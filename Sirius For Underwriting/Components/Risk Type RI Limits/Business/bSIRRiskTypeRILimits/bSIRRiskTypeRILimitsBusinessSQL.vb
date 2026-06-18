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
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 09/06/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRRiskTypeRILimits.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'start
	' Select Risk Type RI Limit SQL
	Public Const ACSelectRiskTypeRILimitsStored As Boolean = True
	Public Const ACSelectRiskTypeRILimitsName As String = "SelectRiskTypeRILimits"
    Public Const ACSelectRiskTypeRILimitsSQL As String = "spu_risk_type_ri_limit_saa"
	
	' Delete Risk Type RI Limit SQL
	Public Const ACDeleteRiskTypeRILimitsStored As Boolean = True
	Public Const ACDeleteRiskTypeRILimitsName As String = "DeleteRiskTypeRILimits"
    Public Const ACDeleteRiskTypeRILimitsSQL As String = "spe_risk_type_ri_propertie_dar"
	
	' Insert Risk Type RI Limit SQL
	Public Const ACInsertRiskTypeRILimitsStored As Boolean = True
	Public Const ACInsertRiskTypeRILimitsName As String = "InsertRiskTypeRILimits"
    Public Const ACInsertRiskTypeRILimitsSQL As String = "spe_risk_type_ri_propertie_add"
	
	' Select Available GIS Properties SQL
	Public Const ACSelectGISPropertiesStored As Boolean = True
	Public Const ACSelectGISPropertiesName As String = "SelectGISProperties"
    Public Const ACSelectGISPropertiesSQL As String = "spu_ri_limit_gis_properties"
	
	' Delete Risk Type RI Value SQL
	Public Const ACDeleteRiskTypeRIValuesStored As Boolean = True
	Public Const ACDeleteRiskTypeRIValuesName As String = "DeleteRiskTypeRIValues"
    Public Const ACDeleteRiskTypeRIValuesSQL As String = "spe_risk_type_ri_values_dar"
    'end
    Public Const ACSelectRiskTypeRILimitsVersionStored As Boolean = True
    Public Const ACSelectRiskTypeRILimitsVersionName As String = "SelectRiskTypeRILimitsVersion"
    Public Const ACSelectRiskTypeRILimitsVersionSQL As String = "spu_risk_type_ri_limit_version_saa"

    Public Const ACCopyRiskTypeRILimitsVersionStored As Boolean = True
    Public Const ACCopyRiskTypeRILimitsVersionName As String = "CopyRiskTypeRILimitsVersion"
    Public Const ACCopyRiskTypeRILimitsSQL As String = "spu_risk_type_ri_limit_version_copy"


    ' Update Risk Type RI Model Usage SQL
    Public Const ACUpdateRiskTypeRILimitsVersionStored As Boolean = True
    Public Const ACUpdateRiskTypeRILimitsVersionName As String = "UpdateRiskTypeRILimitVersion"
    Public Const ACUpdateRiskTypeRILimitsVersionSQL As String = "spu_risk_type_ri_limit_version_upd"

    Public Const ACInsertRiskTypeRILimitsVersionStored As Boolean = True
    Public Const ACInsertRiskTypeRILimitsVersionName As String = "UpdateRiskTypeRILimitVersion"
    Public Const ACInsertRiskTypeRILimitsVersionSQL As String = "spu_risk_type_ri_limit_version_add"

    Public Const ACDeleteRiskTypeRILimitVersionStored As Boolean = True
    Public Const ACDeleteRiskTypeRILimitVersionName As String = "DeleteRiskTypeRILimitVersion"
    Public Const ACDeleteRiskTypeRILimitVersionSQL As String = "spu_risk_type_ri_limit_version_del"


End Module