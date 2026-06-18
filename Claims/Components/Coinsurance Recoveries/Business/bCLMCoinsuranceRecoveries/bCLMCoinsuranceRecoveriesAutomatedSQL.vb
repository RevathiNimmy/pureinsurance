Option Strict Off
Option Explicit On
Imports System
Module AutomatedSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: AutomatedSQL
	'
	' Date: 08-June-2000
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMCoInsuranceRecoveries.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Delete All coinsurance for this work claim.
	Public Const kDeleteCoinsuranceName As String = "delete the coinsurer entries in claim party for the specified claim"
	Public Const kDeleteCoinsuranceSQL As String = "spu_delete_coinsurance"
End Module