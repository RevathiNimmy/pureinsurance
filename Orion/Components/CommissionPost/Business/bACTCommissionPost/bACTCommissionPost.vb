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
    ' Date:  07/04/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTCommissionPost"
    Public Const CommissionEarnedPath As String = "\Nominal Ledger\Profit and Loss\Income\Commission Earned\"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const ACTTransDetail_ID As Integer = 0
    Public Const ACTTransAccount_ID As Integer = 1
    Public Const ACTTransPostingStatus As Integer = 2
    Public Const ACTTransCompany_ID As Integer = 3
    Public Const ACTTransCurrency_ID As Integer = 4
    Public Const ACTTransPeriod_ID As Integer = 5
    Public Const ACTTransDocument_ID As Integer = 6
    Public Const ACTTransDocSequence As Integer = 7
    Public Const ACTTransAccountingDate As Integer = 8
    Public Const ACTTransAmount As Integer = 9
    Public Const ACTTransBaseAmountUnrounded As Integer = 10
    Public Const ACTTransFully_Matched As Integer = 11
    Public Const ACTTransCurrencyAmount As Integer = 12
    Public Const ACTTransCurrencyAmountUnrounded As Integer = 13
    Public Const ACTTransCurrency_Base_Xrate As Integer = 14
    Public Const ACTTransEuroCurrency_ID As Integer = 15
    Public Const ACTTransEuroAmount As Integer = 16
    Public Const ACTTransEuroBaseXrate As Integer = 17
    Public Const ACTTransEuroCcyXrate As Integer = 18
    Public Const ACTTransComment As Integer = 19
    Public Const ACTTransInsuranceRef As Integer = 20
    Public Const ACTTransOperator_ID As Integer = 21
    Public Const ACTTransPurchaseOrderNo As Integer = 22
    Public Const ACTTransPurchaseInvoiceNo As Integer = 23
    Public Const ACTTransDepartment As Integer = 24
    Public Const ACTTransSpare As Integer = 25
    Public Const ACTTransRefDate As Integer = 26
    Public Const ACTTransRefAmount As Integer = 27
    Public Const ACTTransRefQuantity As Integer = 28
    Public Const ACTTransRefUnits As Integer = 29
    Public Const ACTTransDepartment_ID As Integer = 30
    Public Const ACTTransSubBranch_ID As Integer = 31
    Public Const ACTTransMediaType_ID As Integer = 32
    Public Const ACTTransBankAccountCode As Integer = 33
    Public Const ACTTransBankAccountNumber As Integer = 34
    Public Const ACTTransCreditCardNumber As Integer = 35
    Public Const ACTTransCreditCardExDate As Integer = 36
    Public Const ACTTransCreditCardStartDate As Integer = 37
    Public Const ACTTransCreditCardIssue As Integer = 38
    Public Const ACTTransMediaTypeValidation_Code As Integer = 39
    Public Const ACTTransPartyCnt As Integer = 40
    Public Const ACTTransTotalAmount As Integer = 41

    'PSL 05/03/2003 Issue 2520
    Public Const ACTCahshListItem_PaymentsStatus_Issued As Integer = 1



    Public Sub Main()
    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module