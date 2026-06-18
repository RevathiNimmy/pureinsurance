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
    ' Date: 20/07/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRuleLookup.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    'Developer Guide No 39
    Public Const ACAddLookupDataStored As Boolean = True
    Public Const ACAddLookupDataName As String = "AddLookupData"
    Public Const ACAddLookupDataSQL As String = "spu_AddLookupData"

    Public Const ACUpdLookupDataStored As Boolean = True
    Public Const ACUpdLookupDataName As String = "UpdLookupData"
    Public Const ACUpdLookupDataSQL As String = "spu_UpdLookupData"

    Public Const ACAddLookupHeaderStored As Boolean = True
    Public Const ACAddLookupHeaderName As String = "AddLookupHeader"
    Public Const ACAddLookupHeaderSQL As String = "spu_AddLookupHeader"

    Public Const ACUpdLookupHeaderStored As Boolean = True
    Public Const ACUpdLookupHeaderName As String = "UpdLookupHeader"
    Public Const ACUpdLookupHeaderSQL As String = "spu_UpdLookupHeader"

    Public Const ACGetAllLookupHeaderStored As Boolean = True
    Public Const ACGetAllLookupHeaderName As String = "GetAllLookupHeader"
    Public Const ACGetAllLookupHeaderSQL As String = "spu_GetAllLookupHeader"

    Public Const ACDelLookupHeaderStored As Boolean = True
    Public Const ACDelLookupHeaderName As String = "DelLookupHeader"
    Public Const ACDelLookupHeaderSQL As String = "spu_DelGisLookupHeader"

    Public Const ACGetAllLookupDataStored As Boolean = True
    Public Const ACGetAllLookupDataName As String = "GetAllLookupData"
    Public Const ACGetAllLookupDataSQL As String = "spu_GetAllLookupData"

    Public Const ACGetNextLineKeyStored As Boolean = True
    Public Const ACGetNextLineKeyName As String = "GetNextLineKeySQL"
    Public Const ACGetNextLineKeySQL As String = "spu_GetNextLineKey"

    Public Const ACDelLookupDataStored As Boolean = True
    Public Const ACDelLookupDataName As String = "DelLookupData"
    Public Const ACDelLookupDataSQL As String = "spu_DelGisLookupData"

    Public Const ACGetDataModelCodeStored As Boolean = True
    Public Const ACGetDataModelCodeName As String = "GetDataModelCode"
    Public Const ACGetDataModelCodeSQL As String = "spu_getDataModelCode"
    'ends

    Public Const ACLookupNameUniqueAddStored As Boolean = False
    Public Const ACLookupNameUniqueAddName As String = "LookupNameUniqueAdd"
    Public Const ACLookupNameUniqueAddSQL As String = "SELECT '' FROM GIS_lookup_header WHERE Lookup_name = {lookup_name}"

    Public Const ACLookupNameUniqueEditStored As Boolean = False
    Public Const ACLookupNameUniqueEditName As String = "LookupNameUniqueEdit"
    Public Const ACLookupNameUniqueEditSQL As String = "SELECT '' FROM GIS_lookup_header WHERE Lookup_name = {lookup_name} AND lookup_key <> {lookup_key}"

    Public Const ACGetSingleLookupHeaderStored As Boolean = True
    Public Const ACGetSingleLookupHeaderName As String = "GetSingleLookupHeader"
    Public Const ACGetSingleLookupHeaderSQL As String = "spu_Get_SingleLookupHeader"

    Public Const ACSingleAllLookupDataStored As Boolean = True
    Public Const ACSingleAllLookupDataName As String = "GetSingleLookupData"
    Public Const ACSingleAllLookupDataSQL As String = "spu_Get_SingleLookupData"

End Module