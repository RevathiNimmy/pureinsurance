Option Strict Off
Option Explicit On
Imports System
Module bPMPFEDIConsts
	
	Public Const ACApp As String = "bSIRPFEDIMessage"
	
	'Main EDI Message Array
	Public Const k_EDIBrokerMailboxNumber As Integer = 0
	Public Const k_EDIFinanceMailboxNumber As Integer = 1
	Public Const k_EDINoOfMessagesSent As Integer = 2
	Public Const k_EDIDtOfEDIMessageCreation As Integer = 3
	Public Const k_EDITmOfEDIMessageCreation As Integer = 4
	Public Const k_EDIBrokerABICodelist1Ref As Integer = 5
	Public Const k_EDIPMAssFinCmpyCodelist1Addr As Integer = 6
	Public Const k_EDIBrokerContactName As Integer = 7
	Public Const k_EDIBrokerClientRef As Integer = 8
	Public Const k_EDIBrokerUniqueKey As Integer = 9
	Public Const k_EDIBrokerICCSNo As Integer = 10
	Public Const k_EDIBrokerName As Integer = 11
	Public Const k_EDIBrokerAddr1 As Integer = 12
	Public Const k_EDIBrokerAddr2 As Integer = 13
	Public Const k_EDIBrokerAddr3 As Integer = 14
	Public Const k_EDIBrokerAddr4 As Integer = 15
	Public Const k_EDIBrokerPostCode As Integer = 16
	Public Const k_EDIBrokerPhoneNo As Integer = 17
	Public Const k_EDIClientName As Integer = 18
	Public Const k_EDIClientAddr1 As Integer = 19
	Public Const k_EDIClientAddr2 As Integer = 20
	Public Const k_EDIClientAddr3 As Integer = 21
	Public Const k_EDIClientAddr4 As Integer = 22
	Public Const k_EDIClientPCode As Integer = 23
	Public Const k_EDIClientPhoneNo As Integer = 24
	Public Const k_EDIClientContactName As Integer = 25
	Public Const k_EDIFinanceRate As Integer = 26
	Public Const k_EDIFinanceAPR As Integer = 27
	Public Const k_EDIBusinessType As Integer = 28
	Public Const k_EDIBankAccountName As Integer = 29
	Public Const k_EDIBankAccountNumber As Integer = 30
	Public Const k_EDIBankSortCode As Integer = 31
	Public Const k_EDIBankName As Integer = 32
	Public Const k_EDIBankBranchName As Integer = 33
	Public Const k_EDIAutoGenPlanRef As Integer = 34
	Public Const k_EDIBankAddr1 As Integer = 35
	Public Const k_EDIBankAddr2 As Integer = 36
	Public Const k_EDIBankAddr3 As Integer = 37
	Public Const k_EDIBankPCode As Integer = 38
	Public Const k_EDITotalGrossPremium As Integer = 39
	Public Const k_EDIDaysDelay As Integer = 40
	Public Const k_EDINoOfInstalments As Integer = 41
	Public Const k_EDIInterestValue As Integer = 42
	Public Const k_EDIRateStyle As Integer = 43
	Public Const k_EDINoOfTransactions As Integer = 44
	Public Const k_EDIDateOfFirstPayment As Integer = 45
	Public Const k_EDIReTransmitFlag As Integer = 46
	Public Const k_EDIPlanRefNumber As Integer = 47
	Public Const k_EDIPlanVersionNumber As Integer = 48
	Public Const k_EDIPreferredCustomerDate As Integer = 49
	Public Const k_EDIFinanceCollatedPlanRef As Integer = 50
	Public Const k_EDISchemeName As Integer = 51
	Public Const k_EDIRateCode As Integer = 52
	Public Const k_EDIPlanDepositAmount As Integer = 53
	Public Const k_EDIPlanDepositPercent As Integer = 54
	Public Const k_EDIClientDOB As Integer = 55
	Public Const k_EDIClientTitle As Integer = 56
	Public Const k_EDIClientForenames As Integer = 57
	Public Const k_EDIClientSurname As Integer = 58
	Public Const k_EDICompanyReg As Integer = 59
	Public Const k_EDICCNumber As Integer = 60
	Public Const k_EDICCExpiry As Integer = 61
	Public Const k_EDIPaymentProtection As Integer = 62
	Public Const k_EDIProviderReference As Integer = 63
	Public Const k_EDIAuthCode As Integer = 64
	Public Const k_EDIVehicleReg As Integer = 65
	Public Const k_EDIVehicleMake As Integer = 66
	Public Const k_EDIVehicleModel As Integer = 67
	Public Const k_EDIVehicleYear As Integer = 68
	Public Const k_EDIInsurerName As Integer = 69
	Public Const k_EDIPolicyExcess As Integer = 70
	Public Const k_EDIPolicyNumber As Integer = 71
	Public Const k_EDIProductType As Integer = 72
	Public Const k_EDIPolicyCoverType As Integer = 73
	Public Const k_EDIExtraValue As Integer = 74
	Public Const k_EDIPolicyStartDate As Integer = 75
	'PN12594
	Public Const k_EDIBusinessCode As Integer = 76
	'PN13915
	Public Const k_EDI_PC_Special1 As Integer = 77
	Public Const k_EDI_PC_Special2 As Integer = 78
	Public Const k_EDINoOfElements As Integer = 78
	
	'EDI Transaction Array
	Public Const k_EDITransactionFromDate As Integer = 0
	Public Const k_EDITransactionInsurer As Integer = 1
	Public Const k_EDITransactionRiskDescription As Integer = 2
	Public Const k_EDITransactionFees As Integer = 3
	Public Const k_EDITransactionExtras As Integer = 4
	Public Const k_EDITransactionToDate As Integer = 5
	Public Const k_EDITransactionType As Integer = 6
	Public Const k_EDITransactionUniquePolicyNumber As Integer = 7
	Public Const k_EDITransactionPolicyNumber As Integer = 8
	Public Const k_EDITransactionAmount As Integer = 9
	Public Const k_EDITransactionInsurerABICode As Integer = 10
	Public Const k_EDITransactionRiskABICode As Integer = 11
	
	Public Const k_EDINoOfTransElements As Integer = 11
	
	'Insurer Array
	Public Const k_EDIPol_InsurerRef As Integer = 0
	Public Const k_EDIPol_CoverStartDate As Integer = 1
	Public Const k_EDIPol_ExpiryDate As Integer = 2
	Public Const k_EDIPol_RenewalDate As Integer = 3
	Public Const k_EDIPol_Insurer As Integer = 4
	Public Const k_EDIPol_RiskCode As Integer = 5
	Public Const k_EDIPol_Product As Integer = 6
	Public Const k_EDIPol_TransDescription As Integer = 7
	Public Const k_EDIPol_TotalPremium As Integer = 8
	Public Const k_EDIPol_Fees As Integer = 9
	Public Const k_EDIPol_Extras As Integer = 10
	Public Const k_EDIPol_Insurer_ABI As Integer = 11
	Public Const k_EDIPol_Business_ABI As Integer = 12
	
	'Field Definition Array
	Public Const k_EDIDef_OutputIndex As Integer = 0
	Public Const k_EDIDef_ArrayIndex As Integer = 1
	
	
	Public Const k_EDIDef_ColumnName As Integer = 2
	Public Const k_EDIDef_ColumnSize As Integer = 3
	Public Const k_EDIDef_ColumnType As Integer = 4
	Public Const k_EDIDef_DecimalAccuracy As Integer = 5
	Public Const k_EDIDef_SignedField As Integer = 6
	
	
	'Steve Watton 01/06/2004, Add in constants for the additional client info needed
	'for the accident care additional info array.
	
	Public Const k_EDIInfo_Title As Integer = 0
	Public Const k_EDIInfo_Forenames As Integer = 1
	Public Const k_EDIInfo_Surname As Integer = 2
	Public Const k_EDIInfo_DOB As Integer = 3
	Public Const k_EDIInfo_Address1 As Integer = 4
	Public Const k_EDIInfo_PostCode As Integer = 5
	Public Const k_EDIInfo_Telephone As Integer = 6
	Public Const k_EDIInfo_InsurerName As Integer = 7
	Public Const k_EDIInfo_PolicyNumber As Integer = 8
	Public Const k_EDIInfo_PolicyPremium As Integer = 9
	Public Const k_EDIInfo_ACType As Integer = 10
	Public Const k_EDIInfo_ACPremium As Integer = 11
	Public Const k_EDIInfo_PolicyStartDate As Integer = 12
	Public Const k_EDIInfo_Address2 As Integer = 13
	Public Const k_EDIInfo_Address3 As Integer = 14
	Public Const k_EDIInfo_Address4 As Integer = 15
	'DC300604 PN12139 added fields for GII telphone numbers
	Public Const k_EDIInfo_GIIHomeTelephone As Integer = 16
	Public Const k_EDIInfo_GIIWorkTelephone As Integer = 17
	'DC050804 PN13913 added field for GII date of birth
	Public Const k_EDIInfo_GIIDOB As Integer = 18
End Module