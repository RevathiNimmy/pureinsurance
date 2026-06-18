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
    ' Date: 08/07/02
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyAGG.Business class. Created from bSIRPartyAG.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyAGG SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyAGG"
    'Developer Guide No. 39
    Public Const ACGetAllDetailsSQL As String = "spe_Party_Agent_Group_sel"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyAGGID"
    'Developer Guide No. 39
    Public Const ACCheckIDSQL As String = "spe_SIRPartyAGG_check_id"

    ' Select next available shortname from agent table
    Public Const ACGetNextRefStored As Boolean = True
    Public Const ACGetNextRefName As String = "SelectNextShortname"
    'Developer Guide No. 39
    Public Const ACGetNextRefSQL As String = "spu_Next_Agent_Group_Shortname_sel"

    Public Const ACDelAddressStored As Boolean = True
    Public Const ACDelAddressName = "DeleteAddresses"
    Public Const ACDelAddressSQL As String = "spe_Delete_Addresses"
End Module