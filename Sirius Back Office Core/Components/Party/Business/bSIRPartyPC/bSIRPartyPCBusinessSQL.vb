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
    '              bSIRPartyPC.Business class.
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

    ' Select All SIRPartyPC SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyPC"
    'developer guide no. 39 (Guide)


    Public Const ACGetAllDetailsSQL As String = "spe_Party_Personal_Client_sel"
    '' AC 28/08/2003 CQ1123 - retreive data model code from database
    Public Const ACGetDataModelCodeStored As Boolean = False
    Public Const ACGetDataModelCodeName As String = "SelectOneOption"
    Public Const ACGetDataModelCodeSQL As String = "select Code from gis_data_model " &
                                                   "where gis_data_model_id = {datamodel_id}"


    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyPCID"
    'developer guide no. 39 (Guide)



    Public Const ACCheckIDSQL As String = "spe_SIRPartyPC_check_id"
    ' Get Employer details SQL
    Public Const ACGetEmployerDetailsStored As Boolean = False
    Public Const ACGetEmployerDetailsName As String = "GETEmployerDETAILS"
    Public Const ACGetEmployerDetailsSQL As String = "SELECT shortname FROM party WHERE party_cnt = "

    ' Select All SIRPartyPC Convictions
    Public Const ACGetAllConvictionStored As Boolean = True
    Public Const ACGetAllConvictionName As String = "SelectAllPartyConviction"
    'developer guide no. 39 (Guide)


    Public Const ACGetAllConvictionSQL As String = "spe_Party_conviction_saa"
    ' Select All SIRPartyPC Lifestyles
    Public Const ACGetAllLifestyleStored As Boolean = True
    Public Const ACGetAllLifestyleName As String = "SelectAllPartyLifestyle"
    'developer guide no. 39 (Guide)


    Public Const ACGetAllLifeStyleSQL As String = "spe_Party_lifestyle_saa"
    ' RAW 18/11/2002 : PS005 : added
    ' Select All Loyalty Schemes
    Public Const ACGetAllLoyaltySchemeStored As Boolean = True
    Public Const ACGetAllLoyaltySchemeName As String = "SelectAllPartyLoyaltyScheme"
    'developer guide no. 39 (Guide)


    Public Const ACGetAllLoyaltySchemeSQL As String = "spu_SIR_Select_PartyLoyaltyScheme"
    ' Get Hidden Option
    Public Const ACGetHiddenOptionStored As Boolean = False
    Public Const ACGetHiddenOptionName As String = "ACGetHiddenOption"
    Public Const ACGetHiddenOptionSQL As String = "select value from hidden_options"

    ' PW180303 - Get unique number
    ' PS186
    Public Const ACGetNextClientCodeStored As Boolean = True
    Public Const ACGetNextClientCodeName As String = "Get Next Client Code"
    'developer guide no. 39 (Guide)

    Public Const ACGetNextClientCodeSQL As String = "spu_get_unique_number"
End Module