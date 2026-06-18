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
    ' Class Name    :BusinessSQL
    ' Date          :16/08/2000
    ' Description   :Contains the SQL Statements required by the
    '               bCLMPerilType.Business class.
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectPeril"
    ' Public Const ACSelectSQL = "SELECT * FROM Peril WHERE Peril_id = {Peril_id}"

    ' Select All GetClmForResvType SQL
    Public Const ACGetClmForResvTypeStored As Boolean = True
    Public Const ACGetClmForResvTypeName As String = "GetClmForResvType"

    'Developer Guide No: 39
    Public Const ACGetClmForResvTypeSQL As String = "spu_get_clm_for_resv_type"

    ' Select All ResvTypes SQL
    Public Const ACGetPerilTypesStored As Boolean = True
    Public Const ACGetPerilTypesName As String = "GetPerilTypes"

    'Developer Guide No: 39
    Public Const ACGetPerilTypesSQL As String = "spu_get_Peril_types"

    ' Select All ChkPerilTypeNameExists SQL
    Public Const ACChkPerilTypeNameExistsStored As Boolean = True
    Public Const ACChkPerilTypeNameExistsName As String = "ChkPerilTypeNameExists"

    'Developer Guide No: 39
    Public Const ACChkPerilTypeNameExistsSQL As String = "spu_chk_Peril_type_name_exists"



    '*******************************
    '' Check ID SQL
    'Public Const ACCheckIDStored = True
    'Public Const ACCheckIDName = "CheckCLMPerilTypeID"
    'Public Const ACCheckIDSQL = "{call spe_CLMPerilType_check_id (?)}"
End Module