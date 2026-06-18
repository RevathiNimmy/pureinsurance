Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 30/09/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCurrencyConvert.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Lookup Euro Rate SQL
    'developer guide no. 39
	Public Const ACLookupEuroRateStored As Boolean = False
	Public Const ACLookupEuroRateName As String = "LookupEuroRate"
	Public Const ACLookupEuroRateSQL As String = "{}"
	
	Public Const ACDoCurrencyConversionStored As Boolean = True
    Public Const ACDoCurrencyConversionName As String = "DoCurrencyConversion"
    Public Const ACDoCurrencyConversionSQL As String = "spu_ACT_Do_Currency_Conversion"
	
	Public Const ACGetInsuranceFileInformationStored As Boolean = True
	Public Const ACGetInsuranceFileInformationName As String = "GetInsuranceFileInformation"
    Public Const ACGetInsuranceFileInformationSQL As String = "spu_ACT_Get_Insurance_File_Information"
	
	Public Const ACUpdateInsuranceFileStored As Boolean = True
	Public Const ACUpdateInsuranceFileName As String = "UpdateInsuranceFile"
    Public Const ACUpdateInsuranceFileSQL As String = "spu_ACT_Update_Insurance_File"
	
	Public Const ACGetAccountIDFromPartyCntStored As Boolean = True
	Public Const ACGetAccountIDFromPartyCntName As String = "GetAccountIDFromPartyCnt"
    Public Const ACGetAccountIDFromPartyCntSQL As String = "spu_ACT_Get_AccountID_From_partyCnt"
	
	Public Const ACGetCurrencyRateStored As Boolean = True
	Public Const ACGetCurrencyRateName As String = "GetCurrencyRate"
    Public Const ACGetCurrencyRateSQL As String = "spu_ACT_Get_Currency_Rate"
	
	Public Const ACGetClaimInformationStored As Boolean = True
	Public Const ACGetClaimInformationName As String = "GetClaimInformation"
    Public Const ACGetClaimInformationSQL As String = "spu_ACT_Get_Claim_Information"
	
	Public Const ACGetClaimPaymentInformationStored As Boolean = True
	Public Const ACGetClaimPaymentInformationName As String = "GetClaimInformation"
    Public Const ACGetClaimPaymentInformationSQL As String = "spu_ACT_Get_ClaimPayment_Information"
	
	Public Const ACGetClaimReceiptInformationName As String = "Get Claim Receipt Information"
	Public Const ACGetClaimReceiptInformationSQL As String = "spu_ACT_Get_Claim_Receipt_Information"
	
	'***************************************
	'***************************************
	' AUS005 Changes
	Public Const kCurrencyToCurrencyConversionName As String = "Convert Currency Amount from Currency1 to Currency2"
	Public Const kCurrencyToCurrencyConversionSQL As String = "spu_ACT_Do_Currency_To_Currency_Conversion"
    '***************************************

    Public Const ACCurrencyBaseRateByAccountStored As Boolean = True
    Public Const ACCurrencyBaseRateByAccounttName As String = "CurrencyBaseRateByAccount"
    Public Const ACCurrencyBaseRateByAccountSQL As String = "spu_ACT_CurrencyBaseRateByAccount"

	'***************************************
    'developer guide no. 29(No Solutions)
    'Shared Sub New()
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module