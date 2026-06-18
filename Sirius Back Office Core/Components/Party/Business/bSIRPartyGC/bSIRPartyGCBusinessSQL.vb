Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 12/10/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyGC.Business class.
    '
    ' Edit History:
    ' RAW 18/11/2002 : PS005 : Added loyalty scheme
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyGC SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyGC"
    'developer guide no. 39(guide)
    Public Const ACGetAllDetailsSQL As String = "spe_Party_Agent_sel"

    '' AC 28/08/2003 CQ1123 - retreive data model code from database
    Public Const ACGetDataModelCodeStored As Boolean = False
    Public Const ACGetDataModelCodeName As String = "SelectOneOption"
    Public Const ACGetDataModelCodeSQL As String = "select Code from gis_data_model " &
                                                   "where gis_data_model_id = {datamodel_id}"


    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyGCID"
    'developer guide no. 39(guide)
    Public Const ACCheckIDSQL As String = "spe_SIRPartyGC_check_id "

    ' Select All SIRPartyGC Convictions
    Public Const ACGetAllConvictionStored As Boolean = True
    Public Const ACGetAllConvictionName As String = "SelectAllPartyConviction"
    'developer guide no. 39(guide)
    Public Const ACGetAllConvictionSQL As String = "spe_Party_conviction_saa"

    ' RAW 18/11/2002 : PS005 : added
    ' Select All Loyalty Schemes
    Public Const ACGetAllLoyaltySchemeStored As Boolean = True
    Public Const ACGetAllLoyaltySchemeName As String = "SelectAllPartyLoyaltyScheme"
    ' developer guide no. 39(guide)
    Public Const ACGetAllLoyaltySchemeSQL As String = "spu_SIR_Select_PartyLoyaltyScheme"

    ' PW180303 - Get unique number
    ' PS186
    Public Const ACGetNextClientCodeStored As Boolean = True
    Public Const ACGetNextClientCodeName As String = "Get Next Client Code"
    'developer guide no. 39(guide)
    Public Const ACGetNextClientCodeSQL As String = "spu_get_unique_number"
End Module