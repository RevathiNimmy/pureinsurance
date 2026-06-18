Option Strict Off
Option Explicit On
Module FindAccountSql

    ' ***************************************************************** '
    ' Class Name: FindAccountSQL
    '
    ' Date: 01 April 1997
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Embedded SQL)
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectEvent"
    ' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"

    'select account from full query
    'Developer Guide No.39
    'Starts
    Public Const ACAccountFromQueryStored As Boolean = True
    Public Const ACAccountFromQueryName As String = "SelectAccountQuery"
    Public Const ACAccountFromQuerySQL As String = "spu_ACT_Do_FindAccount"

    'get AccountID from parameters SQL
    Public Const ACGetAccountIDStored As Boolean = True
    Public Const ACGetAccountIDName As String = "GetAccountID"
    Public Const ACGetAccountIDSQL As String = "spu_ACT_Do_GetAccountId"

    'select all ledgers for company
    Public Const ACGetLedgersQueryStored As Boolean = True
    Public Const ACGetLedgersQueryName As String = "GetLedgersQuery"
    Public Const ACGetLedgersQuerySQL As String = "spu_ACT_Do_SelectLedgers"

    ' CTAF 080101
    Public Const ACGetFullKeySQL As String = "spu_ACT_Select_Full_Path"
    Public Const ACGetFullKeyName As String = "GetFullKey"
    Public Const ACGetFullKeyStored As Boolean = True

    'PSL 09/06/2003 Issue 4434
    Public Const ACGetPaymentLedgersSQL As String = "spu_ACT_PaymentLedgers"
    Public Const ACGetPaymentLedgersName As String = "PaymentLedgers"
    Public Const ACGetPaymentLedgersStored As Boolean = True
    'Ends
End Module