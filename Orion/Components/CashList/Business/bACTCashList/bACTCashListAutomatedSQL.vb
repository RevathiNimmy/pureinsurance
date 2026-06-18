Option Strict Off
Option Explicit On
Module AutomatedSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: AutomatedSQL
    '
    ' Date: 03/09/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTCashList.Automated class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACAutoSelectStored = False
    ' Public Const ACAutoSelectName = "SelectRisk"
    ' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select CashList SQL
    Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectCashList"
    'Developer Guide No. 39
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_Select_CashList"

    ' Select All CashList SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllCashList"
    'Developer Guide No. 39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_SelAll_CashList"

    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckCashListID"
    'Developer Guide No. 39
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_Check_CashList"

    ' Add CashList SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddCashList"
    'Developer Guide No. 39
    Public Const ACAutoAddSQL As String = "spu_ACT_Add_CashList"

    ' Delete CashList SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteCashList"
    'Developer Guide No. 39
    Public Const ACAutoDeleteSQL As String = "spu_ACT_Delete_CashList"

    ' Update CashList SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateCashList"
    'Developer Guide No. 39
    Public Const ACAutoUpdateSQL As String = "spu_ACT_Update_CashList"

    ' Select Bank Account Default SQL
    Public Const ACAutoGetBankDefaultStored As Boolean = True
    Public Const ACAutoGetBankDefaultName As String = "SelectBankDefaultCashList"
    'Developer Guide No. 39
    Public Const ACAutoGetBankDefaultSQL As String = "spu_ACT_bank_default_Cashlist"

End Module