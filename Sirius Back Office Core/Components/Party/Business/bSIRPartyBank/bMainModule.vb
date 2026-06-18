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

    Public Enum ENPartyBank
        RowStatus = 0
        RowIndex = 1
        PartyBankId = 2
        IsBank = 3
        AccountId = 4
        BankPaymentTypeId = 5
        BankAccountTypeId = 6
        AccountHolderName = 7
        AccountNumber = 8
        BankNameId = 9
        BankBranch = 10
        BankBranchCode = 11
        BankAdd1 = 12
        BankAdd2 = 13
        BankAdd3 = 14
        BankTown = 15
        BankPCode = 16
        BankRegion = 17
        BankCountry = 18
        CCNum = 19
        CCStartDate = 20
        CCExpiryDate = 21
        CCIssueNum = 22
        CCPIN = 23
        IsRegistered = 24
        CCAdd1 = 25
        CCAdd2 = 26
        CCAdd3 = 27
        CCTown = 28
        CCPCode = 29
        CCCountry = 30
        IsDeleted = 31
        CCNameOnCard = 32
        CCManualAuthorisationNum = 33
        CLIBankName = 34
        BIC = 37
        IBAN = 38
        IsDefault = 39
        uBoundPartyBank = IsDefault
    End Enum

    Public Enum ENPartyBankHistory
        ActionCode = 0
        PartyBankId = 1
        AccountId = 2
        BankPaymentTypeId = 3
        BankAccountTypeId = 4
        AccountHolderName = 5
        AccountNumber = 6
        BankName = 7
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
        CCNameOnCard = 31
        CCManualAuthorisationNum = 32
        BIC = 33
        IBAN = 34
        uBoundPartyBankHistory = ENPartyBankHistory.IBAN
    End Enum

    Public Enum ENPMLookups
        Id = 0
        Description = 1
        uboundeNPMLookups = ENPMLookups.Description
    End Enum
End Module