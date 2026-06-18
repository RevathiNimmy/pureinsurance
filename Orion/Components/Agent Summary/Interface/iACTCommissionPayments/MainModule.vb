Option Strict Off
Option Explicit On
Module MainModule
	
	Public Const ACApp As String = "iACTCommissionPayments"
	Private Const ACClass As String = "MainModule"
	
	Public Const ScreenHelpID As Short = 4
	'UPGRADE_NOTE: g_sProductFamily was changed from a Constant to a Variable. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C54B49D7-5804-4D48-834B-B3D81E4C2F13"'
    Public g_sProductFamily As SharedFiles.gPMConstants.PMEProductFamily = SharedFiles.gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	' Public source and language IDs from the
	' Object Manager.
	Public g_iSourceID As Short
	Public g_iLanguageID As Short
	
	Public g_sUsername As New VB6.FixedLengthString(12)
	Public g_iUserID As Short
	
	Public g_oBusiness As Object
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACInterfaceTitle As Short = 100
	Public Const ACFraTitle1 As Short = 101
	Public Const ACFraTitle2 As Short = 102
	Public Const ACStatementDate As Short = 103
	Public Const ACTransDateFrom As Short = 104
	Public Const ACTransDateTo As Short = 105
	Public Const ACCurrency As Short = 106
	Public Const ACProduct As Short = 107
	Public Const ACBranch As Short = 108
	Public Const ACAuthCurrency As Short = 109
	Public Const ACLimitAmount As Short = 110
	Public Const ACTransAuthLimit As Short = 111
	'List Columns
	Public Const ACListTitle1 As Short = 112
	Public Const ACListTitle2 As Short = 113
	Public Const ACListTitle3 As Short = 114
	Public Const ACListTitle4 As Short = 115
	Public Const ACListTitle5 As Short = 116
	
	Public Const PMKeySRCHStatementDate As String = "statementdate"
	Public Const PMKeySRCHTransDateFrom As String = "transdetefrom"
	Public Const PMKeySRCHTransDateTo As String = "transdeteto"
	Public Const PMKeySRCHCurrencyItemID As String = "currencyitemid"
	Public Const PMKeySRCHProductItemID As String = "productitemid"
	Public Const PMKeySRCHBranchItemID As String = "branchitemid"
	Public Const PMKeySRCHTransAuthLimit As String = "transauthlimit"
	Public Const PMKeySRCHAutoSearch As String = "autosearch"
	
	' Buttons
	Public Const ACNextButton As Short = 200
	Public Const ACCancelButton As Short = 201
	Public Const ACFindNowButton As Short = 202
	Public Const ACNewSearchButton As Short = 203
	Public Const ACPreviewButton As Short = 204
	
	''Message
	Public Const ACClearDetailsTitle As Short = 300
	Public Const ACClearDetails As Short = 301
	
	Public Const ACCancelDetailsTitle As Short = 302
	Public Const ACCancelDetails As Short = 303
	
	Public Const ACStatusSearching As Short = 304
	Public Const ACStatusProcessing As Short = 305
	
	Public Const ACHasPaymentsAuthority As Short = 10
	Public Const ACPaymentsCurrencyID As Short = 16
	Public Const ACPaymentsAmount As Short = 11
	
	Public Const ACLockName As String = "CommissionPayment"
	
	'''For Array index
	'' Constants for the search data array indexes.
	Public Const k_AccountId As Short = 0
	Public Const k_Agent As Short = 1
	Public Const k_AgentName As Short = 2
	Public Const k_TotalComm As Short = 3
	Public Const k_Currency As Short = 4
	Public Const k_CurrencyID As Short = 5
    Public im As Integer = 0
	Public Const k_AgentAccountID As Short = 0
	Public Const k_AgentAddress As Short = 1
	Public Const k_AgentMedia_Type As Short = 2
	Public Const k_Bank_Account_Id As Short = 3
	
	'''For header index
    Public Const k_CommissonColHIndexCheckBox As Short = 0
    Public Const k_CommissonColHIndexAccountId As Short = 1
    Public Const k_CommissonColHIndexAgent As Short = 2
    Public Const k_CommissonColHIndexAgentName As Short = 3
    Public Const k_CommissonColHIndexTotalComm As Short = 4
    Public Const k_CommissonColHIndexCurrency As Short = 5
    Public Const k_CommissonColHIndexAuthLimit As Short = 6
	
	Public Enum ListViewCommissionEnum
		ACLTCheckBox
		ACLTAccountId
		ACLTAgent
		ACLTAgentName
		ACLTTotalComm
		ACLTCurrency
		ACLTAuthLimit
	End Enum
	
	Public Enum ACListPaymentSummary
		PSMediaType = 0
		PSPaymentCount = 1
		PSPaymentValue = 2
	End Enum
	
	Public Enum eCashListItem
		CashlistitemID
		AllocationstatusID
		MediaTypeID
		MediaTypeIssuerID
		CashlistID
		AccountId
		MediaRef
		OurRef
		TheirRef
		Amount
		TransdetailID
		ContactName
		Address1
		Address2
		Address3
		Address4
		PostalCode
		AddressCountry
		PaymentName
		PaymentAccountCode
		PaymentBranchCode
		PaymentExpiryDate
		PaymentReference1
		PaymentReference2
		Letter
		Batch_id
		pmuser_id
		Transaction_Date
		Original_Amount
		Amount_Tendered
		Change
		CashListItem_receipt_type_id
		CashListItem_receipt_status_id
		CashListItem_bank_id
		Cheque_Date
		CC_Name
		CC_Number
		CC_Expiry_Date
		CC_Start_Date
		CC_Issue
		CC_Pin
		CC_Auth_Code
		CC_Customer
		CC_Manual_Auth_Code
		CC_Transaction_Code
		Receipt_Details
		CashListItem_Reverse_PMUser_id
		CashListItem_Reverse_Reason_id
		CashListItem_Payment_Type_id
		CashListItem_Payment_Status_id
		Date_Presented
		Cheque_in_Possession
		Stop_Requested_Date
		Stop_Printed_Date
		Stop_Confirmation_Date
		Reason
		Replaces_CashListItem_id
		XML_Object
		InstalmentArray
		SalvageArray
		CLMUSRecoveryArray
		CLMRVRecoveryArray
		UnderwritingYearID
		CurrencyBaseDate
		CurrencyBaseXrate
		AccountBaseDate
		AccountBaseXrate
		SystemBaseDate
		SystemBaseXrate
		OverrideReason
		CashListItem_Comments_Array
		PartyBankId
		CollectionDate
		Comments
		BGPolicies
        CashListItem_bank
        SplitTotal
        ChequeTypeId
        CCBankId
        CardTypeId
        CardTransSlipNo
        ChequeClearingTypeId
        IsLeadAccount
        BankLocation
        BankBranch
        TaxBandId
        TaxAmount
        BIC
        IBAN
        LastItem
	End Enum
End Module