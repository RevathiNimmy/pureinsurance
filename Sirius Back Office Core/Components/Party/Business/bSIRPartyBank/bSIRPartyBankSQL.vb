Option Strict Off
Option Explicit On
Imports System
Module bSIRPartyBankSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	' ***************************************************************** '
	' Class Name: bSIRPartyBankSQL
	'
	' Date: 15/08/2007
	'
	' Description: Contains the SQL Statements to (Stored Procedures)
	'
	' Edit History: Gaurav Arora
	' ***************************************************************** '
	
	
	Public Const ACSELPartyBankDetailsSQL As String = "spu_PartyBank_Details_Sel"
	Public Const ACSELPartyBankDetailsName As String = "Select Party Bank Details"
	
	Public Const ACADDPartyBankDetailsSQL As String = "spu_PartyBank_Details_Add"
	Public Const ACADDPartyBankDetailsName As String = "Add Party Bank Details"
	
	Public Const ACUPDPartyBankDetailsSQL As String = "spu_PartyBank_Details_Upd"
	Public Const ACUPDPartyBankDetailsName As String = "Update Party Bank Details"
	
	Public Const ACDELPartyBankDetailsSQL As String = "spu_PartyBank_Details_DelUndel"
	Public Const ACDELPartyBankDetailsName As String = "Delete Party Bank Details"
	
	Public Const ACADDPartyBankHistorySQL As String = "spu_PartyBank_History_Add"
	Public Const ACADDPartyBankHistoryName As String = "Add Party Bank History"
	
	Public Const ACSELPartyBankHistorySQL As String = "spu_PartyBank_History_Sel"
	Public Const ACSELPartyBankHistoryName As String = "Get Party Bank History"
	
	Public Const ACSELPartyBankDetailsByIdSQL As String = "spu_PartyBank_Details_ByID"
	Public Const ACSELPartyBankDetailsByIdName As String = "Select Party Bank Details By ID"
	
	Public Const ACGetLookupsByEffectiveDateName As String = "Returns lookups by effective date"
	Public Const ACGetLookupsByEffectiveDateSQL As String = "spu_SIR_Get_Lookup_Values_By_Effective_Date"
	
	Public Const ACSELLiveInsOnPayTypeSQL As String = "spu_LiveInstalmentsOnPaymentType_Sel"
	Public Const ACSELLiveInsOnPayTypeName As String = "Is Exists Live Instalments on Payment Type"
	
	Public Const ACGetEventLogSQL As String = "spe_Party_Public_Text_saa"
	Public Const ACGetEventLogName As String = "Get Event Logs Of Party"
	
	Public Const ACAddEventLogSQL As String = "spe_Party_Public_Text_add"
	Public Const ACAddEventLogName As String = "Add Event Logs For Party"
	
	Public Const ACDELDBPartyBankDetailsSQL As String = "spu_PartyBank_Details_DelDB"
	Public Const ACDELDBPartyBankDetailsName As String = "Delete From DB Party Bank Details"
	
	Public Const ACSELPartyAccountsTypeSQL As String = "spu_ACT_Get_Party_AccountsType"
	Public Const ACSELPartyAccountsTypeName As String = "Get Party AccountsType"
	
	Public Const ACSELPaymentTypeSQL As String = "spu_PaymentType_Exist"
	Public Const ACSELPaymentTypeName As String = "Get Payment Type Exists"
	
	Public Const ACSELGetPartyNameSQL As String = "spu_Get_PartyName_PartyCntOrAccountId"
	Public Const ACSELGetPartyName As String = "Get Party Name With PartyCnt Or AccountID"
	
	Public Const ACIspartyBankLinkedWithInstalmentsSQL As String = "spu_isParty_bank_Linked_With_Instalment"
	Public Const ACIspartyBankLinkedWithInstalmentsName As String = "is Party Bank Linked with instalments"
	
	Public Const ACPartyBankActiveTransactionsSQL As String = "spu_PartyBankActiveTransactions"
	Public Const ACPartyBankActiveTransactionsName As String = "Do we have PartyBank ActiveTransactions"

    Public Const kCheckManualAuthCodeSQL As String = "spu_ACT_Check_CC_Manual_Auth_Code"
    Public Const kCheckManualAuthCodeName As String = "CheckManualAuthCode"
End Module