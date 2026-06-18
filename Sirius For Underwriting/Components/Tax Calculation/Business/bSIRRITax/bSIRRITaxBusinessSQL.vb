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
	
    ' Calculate Tax SQL
    Public Const ACCalculateTaxPreviewStored As Boolean = True
    Public Const ACCalculateTaxPreviewName As String = "CalculateTaxAmount"
    Public Const ACCalculateTaxPreviewSQL As String = "spu_SIR_Calculate_Tax_Preview"
	' Delete Insurance File Taxes
	Public Const ACDelAllInsuranceFileTaxStored As Boolean = True
	Public Const ACDelAllInsuranceFileTaxName As String = "DeleteInsuranceFileTax"
	Public Const ACDelAllInsuranceFileTaxSQL As String = "spu_Insurance_File_Tax_DelAll"
	
	' Delete Risk Taxes
	Public Const ACDelAllRiskTaxStored As Boolean = True
	Public Const ACDelAllRiskTaxName As String = "DeleteRiskTax"
	Public Const ACDelAllRiskTaxSQL As String = "spu_Risk_Tax_DelAll"
	
	
	' Get Insurance Reference SQL
	Public Const ACGetInsuranceRefStored As Boolean = True
	Public Const ACGetInsuranceRefName As String = "GetInsuranceRef"
	Public Const ACGetInsuranceRefSQL As String = "spu_Get_Insurance_Ref"
	
	' Get Risk Description SQL
	Public Const ACGetRiskDescriptionStored As Boolean = True
	Public Const ACGetRiskDescriptionName As String = "GetRiskDescription"
	Public Const ACGetRiskDescriptionSQL As String = "spu_Get_Risk_Description"
	
	
	' Select Insurance Tax SQL
	Public Const ACSelectInsuranceFileTaxStored As Boolean = True
	Public Const ACSelectInsuranceFileTaxName As String = "SelectInsuranceFileTax"
	Public Const ACSelectInsuranceFileTaxSQL As String = "spu_Insurance_File_Tax_Select"
	
	' Select Risk Tax SQL
	Public Const ACSelectRiskTaxStored As Boolean = True
	Public Const ACSelectRiskTaxName As String = "SelectRiskTax"
	Public Const ACSelectRiskTaxSQL As String = "spu_Risk_Tax_Select"
	
	
	' Should taxes be applied to product SQL
	Public Const ACTaxAppliedToProductStored As Boolean = True
	Public Const ACTaxAppliedToProductName As String = "TaxAppliedToRisk"
	Public Const ACTaxAppliedToProductSQL As String = "spu_taxes_applied_to_product"
	
	' Should taxes be applied to risk SQL
	Public Const ACTaxAppliedToRiskStored As Boolean = True
	Public Const ACTaxAppliedToRiskName As String = "TaxAppliedToRisk"
	Public Const ACTaxAppliedToRiskSQL As String = "spu_taxes_applied_to_risk"
	'Tax Totals at Risk level
	Public Const ACTaxTotalAtRiskLevelStored As Boolean = True
	Public Const ACTaxTaxTotalName As String = "TaxTotalAtRisk"
	Public Const ACTaxTaxTotalSQL As String = "spu_SIR_Get_TaxNotIncludedInInstalment"
	
	' Update Insurance Tax SQL
	Public Const ACUpdateInsuranceFileTaxStored As Boolean = True
	Public Const ACUpdateInsuranceFileTaxName As String = "UpdateInsuranceTax"
	Public Const ACUpdateInsuranceFileTaxSQL As String = "spu_Insurance_File_Tax_Upd"
	
	' Update Risk Tax SQL
	Public Const ACUpdateRiskTaxStored As Boolean = True
	Public Const ACUpdateRiskTaxName As String = "UpdateRiskTax"
    Public Const ACUpdateRiskTaxSQL As String = "spu_Risk_Tax_Upd"
    Public Const ACUpdateRiskInTaxCalculationSQL As String = "spu_Risk_Tax_Cal_Upd"
	
	Public Const kApplyCalculatedTaxName As String = "update tax calculations with initial tax values"
	Public Const kApplyCalculatedTaxSQL As String = "spu_SIR_Tax_Calculation_Value_Update"
	
	Public Const kGetExistingInsuranceFileTaxName As String = "returns the existing insurance file tax without recalculating the taxes"
	Public Const kGetExistingInsuranceFileTaxSQL As String = "spu_SIR_Get_Existing_Insurance_File_Tax"
	
	' Select Single Risk Tax (RC)
	Public Const ACSelectRiskSingleTaxName As String = "SelectRiskSingleTax"
	Public Const ACSelectRiskSingleTaxSQL As String = "spu_Risk_Single_Tax_Select"
	
	Public Const ACGetTransTypeByRiskKeyStored As Boolean = True
	Public Const ACGetTransTypeByRiskKeyxName As String = "GetTransTypeByRiskKey"
    Public Const ACGetTransTypeByRiskKeySQL As String = "spu_Get_TransType_By_RiskKey"

    ' Copy Risk Tax SQL
    Public Const kCopyRiskTaxStored As Boolean = True
    Public Const kCopyRiskTaxName As String = "UpdateRiskTax"
    Public Const kCopyRiskTaxSQL As String = "spu_Risk_Tax_Copy"

    Public Const ACGetCurrencyDetailsStored As Boolean = True
    Public Const ACGetCurrencyDetailsName As String = "GetCurrencyDetails"
    Public Const ACGetCurrencyDetailsSQL As String = "spu_ACT_Select_Currency"
End Module