Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module bGISPremiumFinanceConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	'START OF BUSINESS (SHARED) CONSTANTS
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	''Public constant declaration area
	'***********************************************
	'MainArray definition
	Public Const m_cCompanyName As Integer = 0
	Public Const m_cSchemeName As Integer = 1
	Public Const m_cSchemeNo As Integer = 2
	Public Const m_cSchemeVer As Integer = 3
	Public Const m_cStartDate As Integer = 4
	Public Const m_cEndDate As Integer = 5
	Public Const m_cProdClass As Integer = 6
	Public Const m_cTransType As Integer = 7
	Public Const m_cAmountToFinance As Integer = 8
	Public Const m_cAPR As Integer = 9
	Public Const m_cIntRate As Integer = 10
	Public Const m_cDaysDelay As Integer = 11
	Public Const m_cNoOfInstalments As Integer = 12
	Public Const m_cFirstInstalment As Integer = 13
	Public Const m_cOthInstalments As Integer = 14
	Public Const m_cCostOfProtect As Integer = 15
	Public Const m_cDeposit As Integer = 16
	Public Const m_cNetAmount As Integer = 17
	Public Const m_cTotalCost As Integer = 18
	Public Const m_cInterestCost As Integer = 19
	Public Const m_cMinFinanceCharge As Integer = 20
	Public Const m_cPayProtection As Integer = 21
	Public Const m_cQDocPath As Integer = 22
	Public Const m_cQDocName As Integer = 23
	Public Const m_cBDocPath As Integer = 24
	Public Const m_cBDocName As Integer = 25
	Public Const m_cCompanyNo As Integer = 26
	Public Const m_cClient As Integer = 27
	Public Const m_cClntAddr1 As Integer = 28
	Public Const m_cClntAddr2 As Integer = 29
	Public Const m_cClntAddr3 As Integer = 30
	Public Const m_cClntPCode As Integer = 31
	Public Const m_cClntAddr4 As Integer = 32
	Public Const m_cClntRegion As Integer = 33
	Public Const m_cClntAreaCode As Integer = 34
	Public Const m_cClntPhone As Integer = 35
	Public Const m_cClntExtn As Integer = 36
	Public Const m_cClntFaxCode As Integer = 37
	Public Const m_cClntFax As Integer = 38
	Public Const m_cClntCountry As Integer = 39
	Public Const m_cBankName As Integer = 40
	Public Const m_cBankSortCode As Integer = 41
	Public Const m_cBankAccountNo As Integer = 42
	Public Const m_cBankAccountName As Integer = 43
	Public Const m_cBankBranch As Integer = 44
	Public Const m_cBankAddr1 As Integer = 45
	Public Const m_cBankAddr2 As Integer = 46
	Public Const m_cBankAddr3 As Integer = 47
	Public Const m_cBankTown As Integer = 48
	Public Const m_cBankPCode As Integer = 49
	Public Const m_cBankAddr4 As Integer = 50
	Public Const m_cBankCountry As Integer = 51
	Public Const m_cBankAreaCode As Integer = 52
	Public Const m_cBankPhoneNo As Integer = 53
	Public Const m_cBankPhoneExt As Integer = 54
	Public Const m_cBankFaxAreaCode As Integer = 55
	Public Const m_cBankFaxNo As Integer = 56
	Public Const m_cBrkName As Integer = 57
	Public Const m_cBrkAddr1 As Integer = 58
	Public Const m_cBrkAddr2 As Integer = 59
	Public Const m_cBrkAddr3 As Integer = 60
	Public Const m_cBrkPCode As Integer = 61
	Public Const m_cBrkAddr4 As Integer = 62
	Public Const m_cBasisOfCalc As Integer = 63
	Public Const m_cArrangementFee As Integer = 64
	Public Const m_cDepositPercent As Integer = 65
	
	Public Const m_cCDocPath As Integer = 68
	Public Const m_cCdocName As Integer = 69
	Public Const m_cClientId As Integer = 70
	
	Public Const m_cPaymentMethod As Integer = 72
	Public Const m_cAutoGenPlanRef As Integer = 73
	Public Const m_cFinCollPlanRef As Integer = 74
	Public Const m_cPolicyCnt As Integer = 75
	Public Const m_cPremFinCnt As Integer = 76
	'The constant below should be set to the value of the last constant above
	'as this is used for dimensioning within the class module clsPremFinance.
	Public Const m_cMainArray As Integer = 76
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public m_sClientRef As String
	'***********************************************
	Public Const m_cNoFinanceRate As gPMConstants.PMEReturnCode = 9999
	Public Const m_cInvalidParty As gPMConstants.PMEReturnCode = 9998
	
	'Constant for whether you can select multiple rows in the plan selection
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public m_bAllowMultiPlanSelect As Boolean
	'***********************************************
	
	'END OF BUSINESS (SHARED) CONSTANTS
	
	Public Const ACApp As String = "bGISPremiumFinance"
End Module