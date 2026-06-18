Option Strict Off
Option Explicit On
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
	
	' Select GIS Property SQL
	Public Const ACSelectGISPropertyStored As Boolean = True
	Public Const ACSelectGISPropertyName As String = "SelectGISPropertyName"
    Public Const ACSelectGISPropertySQL As String = "spe_risk_type_ri_propertie_sel"
	
	' Select Risk Type RI Value SQL
	Public Const ACSelectRiskTypeRIValuesStored As Boolean = True
	Public Const ACSelectRiskTypeRIValuesName As String = "SelectRiskTypeRIValues"
    Public Const ACSelectRiskTypeRIValuesSQL As String = "spe_risk_type_ri_values_sel"
	
	' Insert Risk Type RI Value SQL
	Public Const ACInsertRiskTypeRIValuesStored As Boolean = True
	Public Const ACInsertRiskTypeRIValuesName As String = "InsertRiskTypeRIValues"
    Public Const ACInsertRiskTypeRIValuesSQL As String = "spe_risk_type_ri_values_add"
	
	' Select Available GIS Properties SQL
	Public Const ACSelectGISPropertiesStored As Boolean = True
	Public Const ACSelectGISPropertiesName As String = "SelectGISProperties"
    Public Const ACSelectGISPropertiesSQL As String = "spu_ri_limit_gis_properties"
	
	' Delete Risk Type RI Value SQL
	Public Const ACDeleteRiskTypeRIValuesStored As Boolean = True
	Public Const ACDeleteRiskTypeRIValuesName As String = "DeleteRiskTypeRIValues"
    Public Const ACDeleteRiskTypeRIValuesSQL As String = "spe_risk_type_ri_values_dar"
    ' Select upper limit of ri model SQL
    Public Const ACSelectRIModelUpperLimitStored As Boolean = True
    Public Const ACSelectRIModelUpperLimitName As String = "SelectRIModelUpperLimitName"
    Public Const ACSelectRIModelUpperLimitSQL As String = "spu_risk_type_ri_model_max_limit"
    'end
End Module