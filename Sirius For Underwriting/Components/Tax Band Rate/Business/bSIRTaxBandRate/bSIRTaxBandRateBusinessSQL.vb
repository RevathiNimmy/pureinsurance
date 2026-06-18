Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 03/01/2001
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRTaxBandRate.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Select TBR Arrangement SQL
	Public Const ACSelectTaxBandRateStored As Boolean = True
	Public Const ACSelectTaxBandRateName As String = "SelectTaxBandRate"
	Public Const ACSelectTaxBandRateSQL As String = "spu_Tax_Band_Rate_saa"
	
	' Delete TBR Arrangement SQL
	Public Const ACDeleteTaxBandRateStored As Boolean = True
	Public Const ACDeleteTaxBandRateName As String = "DeleteTaxBandRate"
	Public Const ACDeleteTaxBandRateSQL As String = "spu_Tax_Band_Rate_del"
	
	' Insert TBR Arrangement SQL
	Public Const ACInsertTaxBandRateStored As Boolean = True
	Public Const ACInsertTaxBandRateName As String = "InsertTaxBandRate"
	Public Const ACInsertTaxBandRateSQL As String = "spu_Tax_Band_Rate_add"
	
	' Select COBRating Arrangement SQL
	Public Const ACSelectCOBRatingSectionsForRiskStored As Boolean = True
	Public Const ACSelectCOBRatingSectionsForRiskName As String = "SelectCOBRatingSectionsForRisk"
    Public Const ACSelectCOBRatingSectionsForRiskSQL As String = "spu_risk_tax_usage_sel"

    Public Const kUpdateTaxBandRateStored As Boolean = True
    Public Const kUpdateTaxBandRateName As String = "UpdateTaxBandRate"
    Public Const kUpdateTaxBandRateSQL As String = "spu_Tax_Band_Rate_update"
End Module