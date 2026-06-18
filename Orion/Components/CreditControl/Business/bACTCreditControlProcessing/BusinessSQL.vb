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
	'
	' Date: 10-02-2005
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCreditControlProcessing.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Select Account SQL
	Public Const kIsBranchValidName As String = "Check that the specified branch code is valid"
	Public Const kIsBranchValidSQL As String = "{call spu_PM_Select_Source_By_Code (?)}"
End Module