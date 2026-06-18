Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 11/08/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyAH.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyAH SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyAH"
    Public Const ACGetAllDetailsSQL As String = "spe_party_account_handler_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyAHID"
    Public Const ACCheckIDSQL As String = " spe_SIRPartyAH_check_id"

    'EK 12/11/99
    ' Get Party Count SQL
    Public Const ACGetPartyCntStored As Boolean = False
    Public Const ACGetPartyCntName As String = "GETPARTYCNT"
    Public Const ACGetPartyCntSQL As String = "SELECT party_cnt FROM party WHERE shortname = "


    ''Start(Saurabh Agrawal) Tech Spec LOA008 Account Handlers
    Public Const ACGetPartyHandlerBranchListStored As Boolean = True
    Public Const ACGetPartyHandlerBranchListName As String = "GetPartyHandlerBranchList"
    Public Const ACGetPartyHandlerBranchListSQL As String = "spu_Get_Party_Handler_Branch_List"

    Public Const ACDelPartyHandlerBranchListStored As Boolean = True
    Public Const ACDelPartyHandlerBranchListName As String = "DelPartyHandlerBranchList"
    Public Const ACDelPartyHandlerBranchListSQL As String = "spu_Del_Party_Handler_Branch_List"

    Public Const ACAddPartyHandlerBranchListStored As Boolean = True
    Public Const ACAddPartyHandlerBranchListName As String = "DelPartyHandlerBranchList"
    Public Const ACAddPartyHandlerBranchListSQL As String = "spu_Add_Party_Handler_Branch_List"

    Public Const ACDelAddressStored As Boolean = True
    Public Const ACDelAddressName = "DeleteAddresses"
    Public Const ACDelAddressSQL As String = "spe_Delete_Addresses"

    ''End(Saurabh Agrawal) Tech Spec LOA008 Account Handlers
End Module