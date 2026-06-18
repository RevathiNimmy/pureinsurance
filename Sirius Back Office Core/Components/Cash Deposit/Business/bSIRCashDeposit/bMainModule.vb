Option Strict Off
Option Explicit On
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
    Public Const ACApp As String = "bSIRCashDeposit"

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

    Public Enum ENPMLookups
        Id = 0
        Description = 1
        uboundeNPMLookups = ENPMLookups.Description
    End Enum

    Public Const ACTLookupLedgerType As String = "LedgerType"
    Public Const ACTLedgerTypeGeneral As Integer = 1
    Public Const ACTLedgerTypeDebtor As Integer = 2
    Public Const ACTLedgerTypeCreditor As Integer = 3

    Public Const ACTLookupPurgeFrequency As String = "PurgeFrequency"
    Public Const ACTPurgeFreqNever As Integer = 1
    Public Const ACTPurgeFreqYearly As Integer = 2
    Public Const ACTPurgeFreqPeriodEnd As Integer = 3

    Public Const ACTLookupAccountType As String = "AccountType"
    Public Const ACTAccountTypeIncome As Integer = 1
    Public Const ACTAccountTypeExpense As Integer = 2
    Public Const ACTAccountTypeAsset As Integer = 3
    Public Const ACTAccountTypeLiability As Integer = 4
    Public Const ACTAccountTypeGLSuspense As Integer = 5

    ' CF040399
    Public Const ACTLookupAccountStatus As String = "AccountStatus"
    Public Const ACTAccountStatusActive As Integer = 1
    Public Const ACTAccountStatusStopped As Integer = 2


    Public Const kCashDepositTitle As String = "Cash Deposit Account"
End Module