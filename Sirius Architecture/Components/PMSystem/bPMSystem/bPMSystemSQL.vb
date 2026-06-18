Option Strict Off
Option Explicit On
Module PMSystemSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: PMSystemSQL
    '
    ' Date: 17 October 1996
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an PMSystem
    '
    ' Edit History:
    ' RFC250398 - Stored Procedure Calls replaced by embedded sql to
    ' RFC250398 - allow Architecture components to work in db's that
    ' RFC250398 - don't support stored procedures.
    ' RFC080399 - Update Licence Limit added.
    ' PN23684 - adding sAD_OU_Path,sAD_OU_Domain to GetValidSystem
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectPMSystem"
    ' Public Const ACSelectSQL = "SELECT * FROM PMSystem WHERE PMSystem_id = {PMSystem_id}"

    ' Select Number of Licences In Use
    ' RDC 18122002 "<> NULL" changed to "IS NOT NULL"
    Public Const ACGetNoLicencesInUseStored As Boolean = False
    Public Const ACGetNoLicencesInUseName As String = "GetNoLicencesInUse"
    Public Const ACGetNoLicencesInUseSQL As String = "SELECT COUNT(user_id) AS user_count " &
                                                     "FROM pmuser " &
                                                     "WHERE logged_on_at_client IS NOT NULL"

    ' Select Valid PMSystem SQL
    Public Const ACGetValidSystemStored As Boolean = False
    Public Const ACGetValidSystemName As String = "GetValidSystem"
    Public Const ACGetValidSystemSQL As String = "SELECT system_id, product_id, system_name, default_source_id, home_country_id, " &
                                                 "currency_id, language_id, licence_limit, licence_key, log_level, " &
                                                 "pool_size, timestamp, AD_OU_Path, AD_OU_Domain " &
                                                 "FROM pmsystem, pmproduct " &
                                                 "WHERE system_name = {system_name} " &
                                                 "AND code = {product_code} " &
                                                 "AND product_id = pmproduct_id"

    'RFC080399 - Update Licence Limit added.
    Public Const ACUpdateLimitStored As Boolean = False
    Public Const ACUpdateLimitName As String = "UpdateLimit"
    Public Const ACUpdateLimitSQL As String = "UPDATE pmsystem SET licence_limit = {licence_limit}, " &
                                              "licence_key = {licence_key} " &
                                              "WHERE system_name = {system_name}"

    'That was the only one done as the rest do not exist in Gemini

    ' Select PMSystem SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectPMSystem"
    'developer guide no. 39 (Guide)
    Public Const ACGetDetailsSQL As String = "spu_select_PMSystem_by_id"

    ' Add PMSystem SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMSystem"
    'developer guide no. 39 (Guide)
    Public Const ACAddSQL As String = "spu_add_PMSystem"

    ' Delete PMSystem SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePMSystem"
    'developer guide no. 39 (guide)
    Public Const ACDeleteSQL As String = "spu_delete_Message"

    ' Update PMSystem SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMSystem"
    'developer guide no. 39 (guide)
    Public Const ACUpdateSQL As String = "spu_update_PMSystem"

    ' Get ICCS
    Public Const ACGetICCSStored As Boolean = True
    Public Const ACGetICCSName As String = "GetICCS"
    'developer guide no. 39 (guide)
    Public Const ACGetICCSSQL As String = "spu_pm_iccs"

    Public Const ACGetTransactionsExistStored As Boolean = True
    Public Const ACGetTransactionsExistName As String = "GetTransactionsExist"
    'developer guide no. 39 (guide)
    Public Const ACGetTransactionsExistSQL As String = "spu_GetTransactionsExist"
End Module