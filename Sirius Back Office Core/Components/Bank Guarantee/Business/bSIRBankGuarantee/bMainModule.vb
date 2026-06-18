Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  18/06/2007
	'
	' Description: Main Module.
	'
	' Edit History:VB
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRPartyBank"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' Constants for the search data array indexes.
	' ResultArray
	Public Const kICaseID As Integer = 0
	Public Const kICaseNumber As Integer = 1
	Public Const kICaseOpenedDate As Integer = 2
	Public Const kICaseVersion As Integer = 3
	Public Const kICaseProgressStatusID As Integer = 4
	Public Const kICaseAnaystID As Integer = 5
	Public Const kICaseAssistantID As Integer = 6
	Public Const kIBaseCaseID As Integer = 7
	Public Const kIUserID As Integer = 8
	
	'Constants for UnderWriting and Broking
	Public Const ACBroking As String = "A"
	Public Const ACUnderwriting As String = "U"
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "dddd , mmmm d ,yyyy"
	Public Const ACShortDate As String = "short date"
	Public Const ACDateReverse As String = "yyyy/mm/dd"
	
	Public Const kLookId As Integer = 0
	Public Const kLookDesc As Integer = 1
	
	Public Enum ENBankGuarantee
		RowStatus = 0
		RowIndex = 1
		BGId = 2
		BankNameId = 3
		BankBranch = 4
		PartyCnt = 5
		BGRef = 6
		BGCurrencyId = 7
		BGLimit = 8
		AvailableBal = 9
		ExpiryDate = 10
		IsPolicyLock = 11
		BGStatusId = 12
		CustodyBranchId = 13
		IssueDate = 14
		Branches = 15
		Products = 16
		IsDeleted = 17
		Shortname = 18
		Resolved_Name = 19
		uBoundBankGuarantee = ENBankGuarantee.Resolved_Name
	End Enum
	
	Public Enum ENPartyBankHistory
		ActionCode = 0
		PartyBankId = 1
		AccountId = 2
		BankPaymentTypeId = 3
		BankAccountTypeId = 4
		AccountHolderName = 5
		AccountNumber = 6
		BankNameId = 7
		BankBranch = 8
		BankBranchCode = 9
		BankAdd1 = 10
		BankAdd2 = 11
		BankAdd3 = 12
		BankTown = 13
		BankPCode = 14
		BankRegion = 15
		BankCountry = 16
		CCNum = 17
		CCStartDate = 18
		CCExpiryDate = 19
		CCIssueNum = 20
		CCPIN = 21
		IsRegistered = 22
		CCAdd1 = 23
		CCAdd2 = 24
		CCAdd3 = 25
		CCTown = 26
		CCPCode = 27
		CCCountry = 28
		UserID = 29
		DateModified = 30
		uBoundPartyBankHistory = ENPartyBankHistory.DateModified
	End Enum
	
	Public Enum ENPMLookups
		Id = 0
		Description = 1
		uboundeNPMLookups = ENPMLookups.Description
	End Enum
	
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Const kBGStatusActive As String = "Active"
	Public Const kBGStatusIssued As String = "Issued"
	Public Const kBGStatusInvoked As String = "Invoked"
	Public Const kBGStatusDeleted As String = "Deleted"
	Public Const kBGStatusExpired As String = "Expired"
	'End - Sankar - Bank Guarantee Bug Fixing
	
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Enum ENBGStatus
		Active = 1
		Issued = 2
		Invoked = 3
		Deleted = 4
		Expired = 5
	End Enum
	'End - Sankar - Bank Guarantee Bug Fixing
End Module