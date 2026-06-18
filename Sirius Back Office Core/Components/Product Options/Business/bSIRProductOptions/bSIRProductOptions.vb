Option Strict On
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
    ' Date:  08/10/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    '   07062002 SJP - Created
    '   21062002 SJP - Added constants to file
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRProductOptions"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"





    Public Const ACGetUserIsAdminSQL As String = "spu_pmuser_is_sysadmin"
    Public Const ACGetUserIsAdminName As String = "CheckUser"
    Public Const ACGetUserIsAdminProc As Boolean = True
    Public Const ACUser_id As String = "user_id"
    Public Const ACEffective_date As String = "effective_date"

    Public Const ACGetICCSSQL As String = "spu_pm_iccs"
    Public Const ACGetICCSName As String = "Get ICCS"
    Public Const ACGetICCSProc As Boolean = True
    Public Const ACICCS As String = "iccs"

    Public Const ACDeleteOptionsSQL As String = "spe_Product_Options_del"
    Public Const ACDeleteOptionsName As String = "Delete Product Options"
    Public Const ACDeleteOptionsProc As Boolean = True
    Public Const ACBranch_id As String = "branch_id"
    Public Const ACOption_number As String = "option_number"
    Public Const ACDelete_option As String = "delete_option"

    Public Const ACFindBranchesSQL As String = "spu_PM_SelAll_Source"
    Public Const ACFindBranchesName As String = "Get All Branches"
    Public Const ACFindBranchesProc As Boolean = True

    Public Const ACFindProductOptionsSQL As String = "spe_Product_Options_get"
    Public Const ACFindProductOptionsName As String = "Find Product Options"
    Public Const ACFindProductOptionsProc As Boolean = True
    Public Const ACFind_Option As String = "find_option"

    Public Const ACInsertProductOptionSQL As String = "spe_Product_Options_add"
    Public Const ACInsertProductOptionName As String = "Insert Product Option"
    Public Const ACInsertProductOptionProc As Boolean = True
    Public Const ACUW_type As String = "uw_type"
    Public Const ACValue As String = "value"


    Sub Main_Renamed()


    End Sub
End Module