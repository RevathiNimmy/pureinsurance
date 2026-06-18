Option Strict Off
Option Explicit On
Module ClientInstallSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: ClientInstallSQL
    '
    ' Date: 21/01/1999
    '
    ' Description: SQL and Stored procedures
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select All Client Install SQL
    'developer guide no. 39
    Public Const ACSelectAllStored As Boolean = True
    Public Const ACSelectAllName As String = "SelectAllClientInstall"
    Public Const ACSelectAllSQL As String = "spu_pmproduct_client_inst_saa"

    ' Add A Client Install SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddClientInstall"
    Public Const ACAddSQL As String = "spe_pmproduct_client_insta_add"

    ' Delete A Client Install SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteClientInstall"
    Public Const ACDeleteSQL As String = "spe_pmproduct_client_insta_del"

    ' Update A Client Install SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateClientInstall"
    Public Const ACUpdateSQL As String = "spe_pmproduct_client_insta_upd"

    ' Check Client Install SQL
    Public Const ACCheckStored As Boolean = True
    Public Const ACCheckName As String = "CheckClientInstall"
    Public Const ACCheckSQL As String = "spu_pmproduct_client_inst_chk"

    ' Select Client Install SQL
    Public Const ACSelectStored As Boolean = True
    Public Const ACSelectName As String = "SelectClientInstall"
    Public Const ACSelectSQL As String = "spe_pmproduct_client_insta_sel"
End Module