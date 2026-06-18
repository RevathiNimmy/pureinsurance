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
    ' Module Name: Business SQL
    '
    ' Date:  15-11-2002
    '
    ' Description: Hold all constants for database access
    '
    ' ***************************************************************** '

    'Developer Guide No.: 39
    Public Const ACGetRuleFileNameSQL As String = "spu_SIR_Get_Rule_File_Name"

    Public Const ACGetRuleFileNameName As String = "GetRuleFileName"
    Public Const ACGetRuleFileNameStored As Boolean = True

    Public Const ACGetRiskTypeRuleSetTypeSQL As String = "spu_SIR_Get_Risk_Type_Rule_Set_Type"
    Public Const ACGetRiskTypeRuleSetTypeName As String = "GetRiskTypeRuleSetType"
    Public Const ACGetRiskTypeRuleSetTypeStored As Boolean = True

    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module