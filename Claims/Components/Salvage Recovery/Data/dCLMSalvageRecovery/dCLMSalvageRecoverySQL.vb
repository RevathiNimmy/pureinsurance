Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: 24/08/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRContact class.
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    ' Select CLMSalvagerecovery SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleCLMRecovery"
    Public Const ACSelectSingleSQL As String = "{call spu_SalvageRecovery_sel (?)}"

    ' Add CLMSalvagerecovery SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCLMRecovery"
    Public Const ACAddSQL As String = "{call spu_SalvageRecovery_add (?,?,?,?,?,?,?,?,?)}"

    ' Delete CLMSalvagerecovery SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCLMRecovery"
    Public Const ACDeleteSQL As String = "{call spu_SalvageRecovery_del (?)}"

    ' Update CLMSalvagerecovery SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCLMRecovery"
    Public Const ACUpdateSQL As String = "{call spu_SalvageRecovery_upd (?,?,?,?,?,?,?,?,?)}"

    ' Add CLMReceiptSQL
    Public Const ACAddStoredReceipt As Boolean = True
    Public Const ACAddNameReceipt As String = "AddCLMReceipt"
    Public Const ACAddSQLReceipt As String = "{call spu_SalvageReceipt_add (?,?,?,?,?,?,?,?,?,?,?,?)}"

    ' Update CLMReceiptSQL
    Public Const ACUpdateStoredReceipt As Boolean = True
    Public Const ACUpdateNameReceipt As String = "UpdateCLMReceipt"
    Public Const ACUpdateSQLReceipt As String = "{call spu_SalvageReceipt_upd (?,?,?,?,?,?,?,?,?,?,?,?)}"

    ' Add CLMPaymentSQL
    Public Const ACAddStoredPayment As Boolean = True
    Public Const ACAddNamePayment As String = "AddCLMPayment"
    Public Const ACAddSQLPayment As String = "{call spu_Payment_add (?,?,?,?,?,?,?,?,?,?)}"
End Module