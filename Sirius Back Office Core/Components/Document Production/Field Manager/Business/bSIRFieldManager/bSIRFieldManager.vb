Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  28/05/1997
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRFieldManager"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Log Level

    'Bookmark array values
    Public Const BookmarkCode As Integer = 0
    Public Const BookmarkName As Integer = 1
    Public Const BookmarkValue As Integer = 2
    Public Const BookmarkType As Integer = 3
    Public Const BookmarkInstance1 As Integer = 4
    Public Const BookmarkInstance2 As Integer = 5
    Public Const BookmarkInstance3 As Integer = 6
    'ED 26092002 - Removed constanct InstanceTotal
    'RWH(26/09/2000) RSAIB Process 28.
    'AJM 08/03/01 - get document description also
    Public Const ACGetDocumentTemplateSQL As String = "spu_get_document_template_saa"
    Public Const ACGetDocumentTemplateName As String = "GetDocumentTemplate"
    'developer guide no.39
    'start
    Public Const ACGetSubDocumentTemplateSQL As String = "spu_get_sub_document_template_saa"
    Public Const ACGetSubDocumentTemplateName As String = "GetSubDocumentTemplate"
    Public Const ACGetSubDocumentTemplateStored As Boolean = True

    Public Const ACGetRiskClauseInfoSQL As String = "spu_get_risk_clause_info"
    Public Const ACGetRiskClauseInfoName As String = "GetRiskClauseInfo"
    Public Const ACGetRiskClauseInfoStored As Boolean = True

    Public Const kGetAllRiskClauseInfoSQL As String = "spu_Get_AllRisk_Clauses"
    Public Const kGetAllRiskClauseInfoName As String = "GetAllRiskClauses"
    Public Const kGetAllRiskClauseInfoStored As Boolean = True




    Public Const ACGetRiskClausesSQL As String = "spu_get_risk_clauses"
    Public Const ACGetRiskClausesName As String = "GetRiskClauses"
    Public Const ACGetRiskClausesStored As Boolean = True
    'end
    'Plico 21 ( Get Policy Effective Date )
    Public Const ACGetPolicyEffectiveDateSQL As String = "spu_SIR_Select_Policy_EffectiveDate"
    Public Const ACGetPolicyEffectiveDateName As String = "GetPolicyEffectiveDate"
    Public Const ACGetDocumentCodeAndDescTemplateSQL As String = "spu_get_codeandDescdocument_template_saa"
    Public Const ACGetDocumentCodeAndDescTemplateName As String = "GetDocumentCodeAndDescTemplate"
    Public Const ACGetPolicyEffectiveDateStored As Boolean = True

    Public Const ACGetPreviousPolicyDetailSQL As String = "spu_GetPreviousLivePolicy"
    Public Const ACGetPreviousPolicyDetailName As String = "GetPreviousLivePolicyDetails"
    Public Const ACGetPreviousPolicyDetailStored As Boolean = True

    Public Const ACFields_FieldName As Integer = 0
    Public Const ACFields_SQL As Integer = 1
    Public Const ACFields_ColumnName As Integer = 2
    Public Const ACFields_ColumnType As Integer = 3
    Public Const ACFields_MainGroup As Integer = 4
    Public Const ACFields_SubGroup As Integer = 5
    Public Const ACFields_DisplayName As Integer = 6
    Public Const ACFields_IsDisplayed As Integer = 7
    Public Const ACFields_Loop1 As Integer = 8
    Public Const ACFields_Loop2 As Integer = 9
    Public Const ACFields_Loop3 As Integer = 10
    Public Const ACFields_Loop4 As Integer = 11
    Public Const ACFields_ProductFamily As Integer = 12
    Public Const ACFields_DataModel As Integer = 13
    Public Const ACFields_PropertyID As Integer = 14
    Public Const ACFields_SubGroup2 As Integer = 15
    Public Const ACFields_SubGroup3 As Integer = 16
    Public Const ACFields_SubGroup4 As Integer = 17
    Public Const ACFields_TableName As Integer = 19

    Sub Main_Renamed()

    End Sub
End Module
