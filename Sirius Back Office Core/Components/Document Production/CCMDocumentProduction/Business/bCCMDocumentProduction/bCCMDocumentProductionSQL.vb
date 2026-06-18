
Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL

    Public Const ACGetCoreFieldsStored As Boolean = True
    Public Const ACGetCoreFieldsName As String = "GetCoreFieldsForDataBackbone"
    Public Const ACGetCoreFieldsSQL As String = "spu_get_Core_Fields_for_DataBackbone"

    Public Const ACGetDataModelCodeStored As Boolean = True
    Public Const ACGetDataModelCodeName As String = "GetDataModelCode"
    Public Const ACGetDataModelCodeSQL As String = "spu_get_Data_Model_Code_for_DataBackbone"

    Public Const ACGetWPFieldsStored As Boolean = True
    Public Const ACGetWPFieldsName As String = "GetWPFields"
    Public Const ACGetWPFieldsSQL As String = "spu_get_WPFields_for_DataBackbone"

    Public Const ACGetSubGrpStored As Boolean = True
    Public Const ACGetSubGrpName As String = "GetSubGrp"
    Public Const ACGetSubGrpSQL As String = "spu_get_Sub_Group_for_DataBackbone"

    Public Const ACGetCCMDocTemplateStored As Boolean = True
    Public Const ACGetCCMDocTemplateName As String = "GetCCMDocTemplate"
    Public Const ACGetCCMDocTemplateSQL As String = "spu_get_CCM_Doc_Template"

    Public Const ACGetCCMFieldsForFieldSetStored As Boolean = True
    Public Const ACGetCCMFieldsForFieldSetName As String = "GetCCMFieldsForFieldSet"
    Public Const ACGetCCMFieldsForFieldSetSQL As String = "spu_get_CCM_Fields_For_FieldSet"

    Public Const ACGetCCMCoreFieldsForFieldSetStored As Boolean = True
    Public Const ACGetCCMCoreFieldsForFieldSetName As String = "GetCCMCoreFieldsForFieldSet"
    Public Const ACGetCCMCoreFieldsForFieldSetSQL As String = "spu_get_CCM_Core_Fields_For_FieldSet"

    Public Const ACUpdateCoreFieldSetsStored As Boolean = True
    Public Const ACUpdateCoreFieldSetsName As String = "UpdateCoreFieldSets"
    Public Const ACUpdateCoreFieldSetsSQL As String = "spu_update_core_fieldsets"

    Public Const ACUpdateDataStructureNameStored As Boolean = True
    Public Const ACUpdateDataStructureName As String = "UpdateDataStructureName"
    Public Const ACUpdateDataStructureNameSQL As String = "spu_update_DataStructure_name"

    Public Const ACGetSWColumnNameStored As Boolean = True
    Public Const ACGetSWColumnName As String = "GetSWColumnName"
    Public Const ACGetSWColumnNameSQL As String = "spu_get_SW_ColumnName"

    Public Const ACGetWPTableNameSQL As String = "spu_get_WPTableName_DataStructureName"

    Public Const ACGetWPFieldsForDatamodelsStored As Boolean = True
    Public Const ACGetWPFieldsForDatamodelsName As String = "GetWPFieldsFor_ForAnalysisMode"
    Public Const ACGetWPFieldsForDatamodelsSQL As String = "spu_GetWPFieldsFor_ForAnalysisMode"

    Public Const ACUpdateDocumentFieldsetFieldListStored As Boolean = True
    Public Const ACUpdateDocumentFieldsetFieldListName As String = "UpdateDocumentFieldsetFieldList"
    Public Const ACUpdateDocumentFieldsetFieldListSQL As String = "spu_update_Document_FieldsetFieldList"

    Public Const ACGetDocumentFieldsetFieldListStored As Boolean = True
    Public Const ACGetDocumentFieldsetFieldListName As String = "GetDocumentFieldsetFieldList"
    Public Const ACGetDocumentFieldsetFieldListSQL As String = "spu_get_Document_FieldsetFieldList"

    Public Const ACGetCCMTemplatesFromDBStored As Boolean = True
    Public Const ACGetCCMTemplatesFromDBName As String = "GetCCMTemplatesFromDB"
    Public Const ACGetCCMTemplatesFromDBSQL As String = "spu_get_Document_Template_Details"

    Public Const ACRefreshCCMTemplatesStored As Boolean = True
    Public Const ACRefreshCCMTemplatesName As String = "RefreshCCMTemplates"
    Public Const ACRefreshCCMTemplatesSQL As String = "spu_Refresh_CCM_Templates"

    Public Const ACGetDistinctLoop1ValuesStored As Boolean = True
    Public Const ACGetDistinctLoop1ValuesName As String = "GetDistinctLoop1Values"
    Public Const ACGetDistinctLoop1ValuesSQL As String = "spu_get_distinct_loop1_values"

    Public Const ACGetDistinctLoop2ValuesStored As Boolean = True
    Public Const ACGetDistinctLoop2ValuesName As String = "GetDistinctLoop2Values"
    Public Const ACGetDistinctLoop2ValuesSQL As String = "spu_get_distinct_loop2_values"

    Public Const ACGetDistinctLoop3ValuesStored As Boolean = True
    Public Const ACGetDistinctLoop3ValuesName As String = "GetDistinctLoop3Values"
    Public Const ACGetDistinctLoop3ValuesSQL As String = "spu_get_distinct_loop3_values"

    Public Const ACUpdateEndorsementDSNameStored As Boolean = True
    Public Const ACUpdateEndorsementDSName As String = "UpdateEndorsementDataStructureName"
    Public Const ACUpdateEndorsementDSNameSQL As String = "spu_update_endorsement_datastructure_name"

    Public Const ACGetSubGroupForEndorsementStored As Boolean = True
    Public Const ACGetSubGroupForEndorsementName As String = "GetSubGroupForEndorsement"
    Public Const ACGetSubGroupForEndorsementSQL As String = "spu_get_subgroup_for_endorsement"

    Public Const ACHandleSpecialCaseForCoreStored As Boolean = True
    Public Const ACHandleSpecialCaseForCoreName As String = "HandleSpecialCaseForCore"
    Public Const ACHandleSpecialCaseForCoreSQL As String = "spu_handle_special_case_for_core"

End Module
