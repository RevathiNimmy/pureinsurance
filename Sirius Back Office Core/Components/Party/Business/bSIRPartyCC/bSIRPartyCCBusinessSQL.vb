Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 12/10/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyCC.Business class.
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

    ' Select All SIRPartyCC SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyCC"
    Public Const ACGetAllDetailsSQL As String = "spe_Party_Agent_sel"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyCCID"
    Public Const ACCheckIDSQL As String = "spe_SIRPartyCC_check_id"

    'EK 9/12/99
    ' Select All SIRPartyPC Convictions
    Public Const ACGetAllConvictionStored As Boolean = True
    Public Const ACGetAllConvictionName As String = "SelectAllPartyConviction"
    Public Const ACGetAllConvictionSQL As String = "spe_Party_conviction_saa"

    ' RAW 18/11/2002 : PS005 : added
    ' Select All Loyalty Schemes
    Public Const ACGetAllLoyaltySchemeStored As Boolean = True
    Public Const ACGetAllLoyaltySchemeName As String = "SelectAllPartyLoyaltyScheme"
    Public Const ACGetAllLoyaltySchemeSQL As String = "spu_SIR_Select_PartyLoyaltyScheme"

    ' PW180303 - Get unique number
    ' PS186
    Public Const ACGetNextClientCodeStored As Boolean = True
    Public Const ACGetNextClientCodeName As String = "Get Next Client Code"
    Public Const ACGetNextClientCodeSQL As String = "spu_get_unique_number"
End Module