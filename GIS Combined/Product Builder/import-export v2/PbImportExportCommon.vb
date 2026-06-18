Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Imports System.Collections.Generic

Module PbImportExportCommon


    'this is used to detect shift key for debug purposes
    Public Declare Function GetKeyState Lib "user32" (ByVal lngVirtKey As Integer) As Short

    Public Const conVersionNumber As String = "1.7.9" ' CLG 16/12/2004 - Fixed accumulations and use of unique_number

    ' Username.
    Public g_sUsername As String

    ' Password.
    Public g_sPassword As New VB6.FixedLengthString(30)

    ' User ID
    Public g_iUserID As Short

    ' Calling Application
    Public g_sCallingAppName As String
    ' Source ID
    Public g_iSourceID As Short
    ' Language ID
    Public g_iLanguageID As Short
    ' Currency ID
    Public g_iCurrencyID As Short
    ' LogLevel
    Public g_iLogLevel As Short

    'database connection
    Public g_oDatabase As dPMDAO.Database

    ' RAW 02/09/2003 : CQ2158 : added
    Private g_lSQLServerVersion As Integer

    'true if keys are cloned between databases
    Public g_CloneDbKeys As Boolean

    'underwriting/broking flag
    Public g_bIsUnderwriting As Boolean

    'Binary File Number
    Public g_iFileNumber As Short

    ' Flag to stop any further processing if user chooses to cancel
    Public g_bStopProcessing As Boolean
    Public g_bExportInProgress As Boolean

    'import=0 export=1
    Public g_iImportExport As Short

    'Richard Clarke November 2008 - PIE enhancement
    Public g_bFirstImport As Boolean
    Public g_sDataModelCodes As String
    Public g_sDataModelIDs As String


    Public g_DataModelId As Short
    Public g_DataModelCode As String
    Public g_cParentPK As Collection 'dictionary collection of import parent keys
    Public g_lSiriusUserId As Integer


    Public g_lExportMode As Integer

    Public Const conDisable As Short = 0
    Public Const conEnable As Short = 1

    'holds the array that controls export of objects
    Public g_aIeControl() As Object
    'JB Nov 09 to hold parent child key relationhip
    Public g_aParentForeignKeys() As Object
    Public g_aParentForeignKeysDic As New Dictionary(Of String, Object)
    'JB Aug 10 to identify whether deletion has been done for those tables which are on the base of deletion-insertion
    Public g_bOldDataDeleted As Boolean
    Public g_iDataModelType As Short
    Public g_sObjectValue As String

    Public Const pbIeControl_objectId As Short = 0
    Public Const pbIeControl_objectName As Short = 1
    Public Const pbIeControl_objectType As Short = 2
    Public Const pbIeControl_operationMode As Short = 3
    Public Const pbIeControl_DataModelCodeColumn As Short = 4
    Public Const pbIeControl_IsIdentity As Short = 5
    Public Const pbIeControl_DataModelIdColumn As Short = 6
    Public Const pbIeControl_Flags As Short = 7
    Public Const pbIeControl_CodeColumn As Short = 8
    Public Const pbIeControl_HasFilename As Short = 9
    Public Const pbIeControl_RelatedObjectId As Short = 10
    Public Const pbIeControl_PrimaryKeyColumns As Short = 11
    Public Const pbIeControl_WhereClause As Short = 12
    Public Const pbIeControl_ParentIdColumn As Short = 13
    Public Const pbIeControl_GisInsurerIdColumn As Short = 14


    'pbIeControl_Flags values
    Public Const pbIeControl_Flags__IsCaption As Short = 1 'this table has a caption
    Public Const pbIeControl_Flags__uniqueNumber As Short = 2 'need to update unique_number for this
    Public Const pbIeControl_Flags__deleteBeforeAdd0 As Short = 4 'delete the contents before adding, use column 0
    Public Const pbIeControl_Flags__CreatedByOrModifiedBy As Short = 8
    Public Const pbIeControl_Flags__dontTrimStrings As Short = 16 'don't trim strings (strings are by default trimmed to save size)


    'Special UDL Table Definition Array
    Public Const udlTable_columnName As Short = 0
    Public Const udlTable_columnDatatype As Short = 1
    Public Const udlTable_columnSize As Short = 2
    Public Const udlTable_columnPrecision As Short = 3
    Public Const udlTable_columnScale As Short = 4
    Public Const udlTable_columnNullability As Short = 5
    Public Const udlTable_columnIsIdentity As Short = 6
    Public Const udlTable_columnPKFlag As Short = 7

    'ignore flag for column definitions
    Public Const pbIgnoreFlag As Short = -1

    'holds each table's column definitions
    Public g_aIeTableDefinitions() As Object '
    Public Const pbIeTableDefinitions_objectName As Short = 0
    Public Const pbIeTableDefinitions_columnCount As Short = 1
    Public Const pbIeTableDefinitions_columnArray As Short = 2
    Public Const pbIeTableDefinitions_exportedColumns As Short = 3


    Public Const pbIeTableDefinitions_columnName As Short = 0
    Public Const pbIeTableDefinitions_columnType As Short = 1
    Public Const pbIeTableDefinitions_columnSize As Short = 2
    Public Const pbIeTableDefinitions_columnIsIdentity As Short = 3

    'sp_msHelpColumns constants
    Public Const sp_msHelpColumns_columnName As Short = 0
    Public Const sp_msHelpColumns_columnType As Short = 2
    Public Const sp_msHelpColumns_columnSize As Short = 3
    Public Const sp_msHelpColumns_columnPrecision As Short = 4
    Public Const sp_msHelpColumns_columnScale As Short = 5
    Public Const sp_msHelpColumns_columnNull As Short = 9
    Public Const sp_msHelpColumns_columnIdentity As Short = 10
    Public Const sp_msHelpColumns_columnFlags As Short = 11

    Public Const conPrimaryKey As Short = 4
    Public Const conPKColumn As Short = 0

    'modes of operation
    Public Const pbIeMode_DoNotExport As Integer = 0
    Public Const pbIeMode_DataModel As Integer = 1
    Public Const pbIeMode_Screen As Integer = 2
    Public Const pbIeMode_Scheme As Integer = 4
    Public Const pbIeMode_GenerateObjectConstants As Integer = 8
    Public Const pbIeMode_Underwriting As Integer = 16
    Public Const pbIeMode_Broking As Integer = 32
    Public Const pbIeMode_Registry As Integer = 64
    Public Const pbIeMode_RuleFiles As Integer = 128
    Public Const pbIeMode_Documents As Integer = 256
    Public Const pbIeMode_Migration As Integer = 512
    Public Const pbIeMode_RiskGroupsCodes As Integer = 1024
    Public Const pbIeMode_UserStoredProcedure As Integer = 2048
    Public Const pbIeMode_PBDocsOnly As Integer = 4096
    Public Const pbIeMode_UDLs As Integer = 8192
    Public Const pbIeMode_UARs As Integer = 16384
    Public Const pbIeMode_UserReports As Integer = 32768
    Public Const pbIeMode_UserReportsSPU As Integer = 65536 'PM040123

    Public Const pbIeMode_All As Integer = &HFFFFFFFF
    Public Const pbIeMode_Default As Double = pbIeMode_DataModel + pbIeMode_Registry + pbIeMode_RuleFiles + pbIeMode_Documents + pbIeMode_RiskGroupsCodes + pbIeMode_UserStoredProcedure

    'object types
    Public Const pbIeOt_dbTable_fixed As Short = 0
    Public Const pbIeOt_dbTable_userdefined As Short = 1
    Public Const pbIeOt_dbTable_child As Short = 2
    Public Const pbIeOt_RegSetting As Short = 3
    Public Const pbIeOt_DerivedDefAndValRuleFile As Short = 4
    Public Const pbIeOt_DerivedRatingRuleFile As Short = 5
    Public Const pbIeOt_Header As Short = 6
    Public Const pbIeOt_RuleFile As Short = 7
    Public Const pbIeOt_DocumentTemplate As Short = 8
    Public Const pbIeOt_UserDefinedList As Short = 9
    Public Const pbIeOt_UserDefinedListHeader As Short = 10
    Public Const pbIeOt_DatabaseValue As Short = 11
    Public Const pbIeOt_GisList As Short = 12
    Public Const pbIeOt_GSDLookup As Short = 13
    Public Const pbIeOt_GPLookup As Short = 14
    Public Const pbIeOt_Ignore As Short = 15
    Public Const pbIeOt_FixedTableColumns As Short = 16
    Public Const pbIeOt_RiskGroupsCodes As Short = 17

    'Richard Clarke November 2008 - PIE enhancements
    Public Const pbIeOt_UserSPU As Short = 18
    Public Const pbIeOt_UserCrystalReport As Short = 19
    'Richard Clarke November 2008 - PIE enhancements end
    Public Const pbIeOt_UserCrystalReportSPU As Short = 20 'PM040123

    'database types
    Public Const pbIeDbt_Header As Short = 0
    Public Const pbIeDbt_pmlogicaldatabase As Short = 1
    Public Const pbIeDbt_FixedTableColumns As Short = 2
    Public Const pbIeDbt_RegSetting_1 As Short = 3
    Public Const pbIeDbt_RegSetting_2 As Short = 4
    Public Const pbIeDbt_RegSetting_3 As Short = 5
    Public Const pbIeDbt_RegSetting_4 As Short = 6
    Public Const pbIeDbt_RegSetting_5 As Short = 7
    Public Const pbIeDbt_RegSetting_6 As Short = 8
    Public Const pbIeDbt_RegSetting_7 As Short = 9
    Public Const pbIeDbt_RegSetting_8 As Short = 10
    Public Const pbIeDbt_RegSetting_9 As Short = 11
    Public Const pbIeDbt_RegSetting_10 As Short = 12
    Public Const pbIeDbt_RegSetting_11 As Short = 13
    Public Const pbIeDbt_RegSetting_12 As Short = 14
    Public Const pbIeDbt_RegSetting_13 As Short = 15
    Public Const pbIeDbt_RegSetting_14 As Short = 16
    Public Const pbIeDbt_RegSetting_15 As Short = 17
    Public Const pbIeDbt_gis_data_model As Short = 18
    Public Const pbIeDbt_gis_qem_usage As Short = 19
    Public Const pbIeDbt_gis_qem As Short = 20
    Public Const pbIeDbt_gis_data_model_business As Short = 21
    Public Const pbIeDbt_gis_business_type As Short = 22
    Public Const pbIeDbt_gis_scheme_group As Short = 23
    Public Const pbIeDbt_gis_user_def_header As Short = 24
    Public Const pbIeDbt_gis_object As Short = 25
    Public Const pbIeDbt_gis_property As Short = 26
    Public Const pbIeDbt_gis_property_lookup As Short = 27
    Public Const pbIeDbt_gis_screen As Short = 28
    Public Const pbIeDbt_gis_screen_detail As Short = 29
    Public Const pbIeDbt_gis_user_def_header_inds As Short = 30
    Public Const pbIeDbt_gis_user_def_header_rates As Short = 31
    Public Const pbIeDbt_gis_user_def_detail As Short = 32
    Public Const pbIeDbt_gis_user_def_detail_rates As Short = 33
    Public Const pbIeDbt_gis_user_def_detail_inds As Short = 34
    Public Const pbIeDbt_product As Short = 35
    Public Const pbIeDbt_risk_type_group As Short = 36
    Public Const pbIeDbt_product_risk_type_group As Short = 37
    Public Const pbIeDbt_risk_type As Short = 38
    Public Const pbIeDbt_risk_type_rule_set As Short = 39
    Public Const pbIeDbt_risk_type_usage As Short = 40
    Public Const pbIeDbt_Risk_Group As Short = 41
    Public Const pbIeDbt_Risk_Code As Short = 42
    Public Const pbIeDbt_claim_lookup As Short = 43
    Public Const pbIeDbt_claim_party_type As Short = 44
    Public Const pbIeDbt_risk_data_definition As Short = 45
    Public Const pbIeDbt_service_type As Short = 46
    Public Const pbIeDbt_expert_service As Short = 47
    Public Const pbIeDbt_risk_type_expert_service As Short = 48
    Public Const pbIeDbt_GisList As Short = 49
    Public Const pbIeDbt_gis_list_items As Short = 50
    Public Const pbIeDbt_gis_list_type As Short = 51
    Public Const pbIeDbt_gis_list_type_usage As Short = 52
    Public Const pbIeDbt_gis_find_mapping As Short = 53
    Public Const pbIeDbt_gis_lookup_header As Short = 54
    Public Const pbIeDbt_gis_lookup_data As Short = 55
    Public Const pbIeDbt_DerivedDefAndValRuleFile As Short = 56
    Public Const pbIeDbt_document_template As Short = 57
    Public Const pbIeDbt_DocumentTemplateFile As Short = 58
    Public Const pbIeDbt_UserDefinedList As Short = 59
    Public Const pbIeDbt_RI_Band As Short = 60
    Public Const pbIeDbt_class_of_business As Short = 61
    Public Const pbIeDbt_tax_group As Short = 62
    Public Const pbIeDbt_commission_band As Short = 63
    Public Const pbIeDbt_rate_type As Short = 64
    Public Const pbIeDbt_peril_type As Short = 65
    Public Const pbIeDbt_peril_group As Short = 66
    Public Const pbIeDbt_peril_type_usage As Short = 67
    Public Const pbIeDbt_rating_section_type As Short = 68
    Public Const pbIeDbt_sum_insured_type As Short = 69
    Public Const pbIeDbt_policy_section_type As Short = 70
    Public Const pbIeDbt_RuleFile As Short = 71
    Public Const pbIeDbt_UserDefinedListHeader As Short = 72
    Public Const pbIeDbt_gis_insurer As Short = 73
    Public Const pbIeDbt_Broking_companies As Short = 74
    Public Const pbIeDbt_Broking_user As Short = 75
    Public Const pbIeDbt_Fields As Short = 76
    Public Const pbIeDbt_Queries As Short = 77
    Public Const pbIeDbt_Currency As Short = 78
    Public Const pbIeDbt_Language As Short = 79
    Public Const pbIeDbt_Country As Short = 80
    Public Const pbIeDbt_Source As Short = 81
    Public Const pbIeDbt_Transaction_Type As Short = 82
    Public Const pbIeDbt_PMUser_Group As Short = 83
    Public Const pbIeDbt_PMUser As Short = 84
    Public Const pbIeDbt_PMUser_Group_Activity As Short = 85
    Public Const pbIeDbt_PMUser_Group_Group As Short = 86
    Public Const pbIeDbt_PMUser_Group_User As Short = 87
    Public Const pbIeDbt_PMUser_Source As Short = 88
    Public Const pbIeDbt_PMSystem As Short = 89
    Public Const pbIeDbt_PMNumber_Group As Short = 90
    Public Const pbIeDbt_PMNumber As Short = 91
    Public Const pbIeDbt_PMNumber_Range As Short = 92
    Public Const pbIeDbt_PMNav_Batch As Short = 93
    Public Const pbIeDbt_PMNav_Component As Short = 94
    Public Const pbIeDbt_PMNav_Key As Short = 95
    Public Const pbIeDbt_PMNav_Map As Short = 96
    Public Const pbIeDbt_PMProc_Lock_Group As Short = 97
    Public Const pbIeDbt_PMWrk_Task_Category As Short = 98
    Public Const pbIeDbt_PMWrk_Task_Group As Short = 99
    Public Const pbIeDbt_PMWrk_websites As Short = 100
    Public Const pbIeDbt_unique_number As Short = 101
    Public Const pbIeDbt_PMNav_Batch_Key As Short = 102
    Public Const pbIeDbt_PMNav_Batch_Set As Short = 103
    Public Const pbIeDbt_PMNav_Batch_Key_Value As Short = 104
    Public Const pbIeDbt_PMNav_Batch_Record As Short = 105
    Public Const pbIeDbt_PMNav_Get_Component_Key As Short = 106
    Public Const pbIeDbt_PMNav_Get_Process_Key As Short = 107
    Public Const pbIeDbt_PMNav_Get_Step_Key As Short = 108
    Public Const pbIeDbt_PMNav_Process As Short = 109
    Public Const pbIeDbt_PMNav_Set_Component_Key As Short = 110
    Public Const pbIeDbt_PMNav_Set_Map_Key As Short = 111
    Public Const pbIeDbt_PMNav_Set_Process_Key As Short = 112
    Public Const pbIeDbt_PMNav_Set_Step_Key As Short = 113
    Public Const pbIeDbt_PMNav_Step As Short = 114
    Public Const pbIeDbt_PMWrk_Task As Short = 115
    Public Const pbIeDbt_PMWrk_Task_Group_Task As Short = 116
    Public Const pbIeDbt_PMWrk_Task_Inst_Key As Short = 117
    Public Const pbIeDbt_PMWrk_Task_Inst_Log As Short = 118
    Public Const pbIeDbt_PMWrk_Task_Instance As Short = 119
    Public Const pbIeDbt_treaty As Short = 120
    Public Const pbIeDbt_tax_type As Short = 121
    Public Const pbIeDbt_tax_band As Short = 122
    Public Const pbIeDbt_tax_band_rate As Short = 123
    Public Const pbIeDbt_tax_rates As Short = 124
    Public Const pbIeDbt_tax_type_band As Short = 125
    Public Const pbIeDbt_accumulation As Short = 126
    Public Const pbIeDbt_area As Short = 127
    Public Const pbIeDbt_catastrophe_code As Short = 128
    Public Const pbIeDbt_authority_level_type As Short = 129
    Public Const pbIeDbt_license_type As Short = 130
    Public Const pbIeDbt_report As Short = 131
    Public Const pbIeDbt_report_group As Short = 132
    Public Const pbIeDbt_report_group_contents As Short = 133
    Public Const pbIeDbt_report_group_user_groups As Short = 134
    Public Const pbIeDbt_PMProduct_Lookup As Short = 135
    Public Const pbIeDbt_DocumentType As Short = 136
    Public Const pbIeDbt_RegSetting_16 As Short = 137
    Public Const pbIeDbt_user_spu As Short = 138
    Public Const pbIeDbt_tax_group_tax_band As Short = 139
    Public Const pbIeDbt_tax_type_basis As Short = 140
    Public Const pbIeDbt_PMUser_authority_level As Short = 141
    Public Const pbIeDbt_rule_set As Short = 142
    Public Const pbIeDbt_PMUser_authority_rule_set_link As Short = 143
    Public Const pbIeDbt_RI_Model As Short = 144
    Public Const pbIeDbt_RI_Model_Line As Short = 145
    Public Const pbIeDbt_Sub_Commission_Band As Short = 146
    Public Const pbIeDbt_Numbering_Scheme As Short = 147
    Public Const pbIeDbt_user_crystal_report As Short = 148
    Public Const pbIeDbt_product_claims_workflow As Short = 149
    Public Const pbIeDbt_Risk_Type_Rating_Section_Type As Short = 150
    Public Const pbIeDbt_Analysis_Code As Short = 151
    Public Const pbIeDbt_Earning_Pattern_Usage As Short = 152


    '******* NEW SETTINGS ***JB Feb 10/
    Public Const pbIeDbt_Risk_Type_Rating_Section_Type As Short = 164
    Public Const pbIeDbt_Analysis_Code As Short = 165
    Public Const pbIeDbt_Earning_Pattern_Usage As Short = 166
    Public Const pbIeDbt_Report_Spu As Short = 167 'PM040123
    '******* NEW SETTINGS ***JB Feb 10/
    '''''******* NEW SETTINGS ***JB Aug 10/
    ''''Public Const pbIeDbt_state = 164
    '''''******* NEW SETTINGS ***JB Aug 10/


    Public Const EndOfLineChar As Short = 64
    Public Const ReadAccess As String = "Read"
    Public Const WriteAccess As String = "Write"
    Public Const WriteRandom As String = "WriteRandom"
    Public Const DefaultStringValue As String = "<NULL>"
    Public Const DefaultIntegerValue As Short = -9999
    Public Const DefaultCurrencyValue As Double = -99999.99

    Public Const conKeyPath As Short = 0
    Public Const conKeyName As Short = 1
    Public Const conKeyValue As Short = 2

    Public Const conDMC As String = "<DMC>"
    Public Const conMachineName As String = "<MACH_NAME>"
    Public Const conComma As String = ","
    Public Const conCommaSpace As String = ", "
    Public Const conEmptyString As String = ""
    Public Const conDot As String = "."
    Public Const conBackSlash As String = "\"

    Public Const conName As Short = 0
    'UPGRADE_NOTE: conVersion was upgraded to conVersion_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Const conVersion_Renamed As Short = 1

    Public Const conServer As String = "\SERVER\"
    Public Const conClient As String = "\CLIENT\"
    Public Const conSetUp As String = "\SETUP\"
    Public Const conCommon As String = "\COMMON\"

    'general program constants
    Public Const ACApp As String = "Product Builder Import/Export"
    Private Const ACClass As String = conEmptyString

    'checkbox/radio button constants
    Public Const radioExportBasedOn_DataModel As Short = 0
    Public Const radioExportBasedOn_Scheme As Short = 1
    Public Const radioExportBasedOn_Screen As Short = 2
    Public Const radioExportBasedOn_Migration As Short = 3

    'export
    Public Const chkAdditionalExportOptions_Registry As Short = 0
    Public Const chkAdditionalExportOptions_Documents As Short = 1
    Public Const chkAdditionalExportOptions_Rulefiles As Short = 2
    Public Const chkAdditionalExportOptions_RiskGroupsCodes As Short = 3
    Public Const chkAdditionalExportOptions_3DRatings As Short = 4
    Public Const chkAdditionalExportOptions_PBDocsOnly As Short = 5
    Public Const chkAdditionalExportOptions_UDLs As Short = 6
    'Richard Clarke November 2008 - PIE enhancements
    Public Const chkAdditionalExportOptions_AuthRules As Short = 7
    Public Const chkAdditionalExportOptions_SPUICCS As Short = 8
    Public Const chkAdditionalExportOptions_SPUReports As Short = 9
    Public Const chkAdditionalExportOptions_ReportSPU As Short = 10 'PM040123

    'import
    Public Const optAdditionalImportOptions_ImportRegistry As Short = 0
    Public Const optAdditionalImportOptions_DefultRegistry As Short = 1
    Public Const optAdditionalImportOptions_IgnoreRegistry As Short = 2

    '*************
    ' MEvans : 08-11-2003 : CQ3049
    Public Const ACSPGenDataTempDataModelName As String = "XXXDATAMODELNAME"
    Public Const ACSPGenDataTempDataModelTableName As String = "XXXDATAMODELTABLENAME"
    Public Const ACSPGenDataTempDataModelTableAlias As String = "XXXDATAMODELTABLEALIAS"


    Public g_UserSPU() As Object

    Public Const kMaxUMLDetailsRecordsToFetch As Integer = 200
    Public Const kUMLHeaderInfo As String = "HeaderInfo.dat"

    ' ***************************************************************** '
    '
    ' Name: addToArray
    '
    ' Description: Appends item to array
    '
    ' History: ??/??/???? CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub addToArray(ByRef r_vGrowingArray As Object, ByVal v_vToAdd As Object)

        Dim Index As Integer
        Index = -1
        On Error Resume Next
        Index = UBound(r_vGrowingArray)
        On Error GoTo 0
        Index = Index + 1
        ReDim Preserve r_vGrowingArray(Index)
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vToAdd. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGrowingArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_vGrowingArray(Index) = v_vToAdd
        On Error GoTo 0
    End Sub

    ' ***************************************************************** '
    '
    ' Name: writeToStatusBox
    '
    ' Description:
    '
    ' History: ??/??/???? CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub writeToStatusBox(ByRef v_sText As String)

        objFrmMainForm.txtWarning(g_iImportExport).Text = objFrmMainForm.txtWarning(g_iImportExport).Text & v_sText & vbNewLine
        System.Windows.Forms.Application.DoEvents()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: writeToDebugBox
    '
    ' Description:
    '
    ' History: ??/??/???? CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub writeToDebugBox(ByRef v_sText As String)

        objFrmMainForm.txtDebug.Text = objFrmMainForm.txtDebug.Text & v_sText & vbNewLine
        System.Windows.Forms.Application.DoEvents()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: BuildHeaderConfirmationText
    '
    ' Description:
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function BuildHeaderConfirmationText(ByRef txtConfirmationText As System.Windows.Forms.Control, Optional ByVal sDataModelCodes As String = "", Optional ByVal v_sVersionNumber As String = conEmptyString, Optional ByVal v_sComments As Object = Nothing, Optional ByVal v_vVersionError As Object = Nothing, Optional ByVal v_lTotalLines As Object = Nothing) As Integer

        'Richard Clarke November 2008 - PIE enhancements - NEW
        'added optional byval sDataModelCodes variable to function definition

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".BuildHeaderConfirmationText")

        Try

            Dim iExportMode As Long
            Dim iLoop As Short

            '----------------------------
            'Richard Clarke November 2008 - PIE enhancements - NEW
            'loop over our treeview of data models and build up the codes string
            'if it wasn't passed into this function
            If Trim(sDataModelCodes) = "" AndAlso g_iImportExport = 1 Then
                For iLoop = 2 To objFrmMainForm.tvDataModel.Nodes.Count
                    'if this node is selected then add it to the r_ldatamodelid
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    If objFrmMainForm.tvDataModel.Nodes.Item(iLoop).Checked Then
                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        sDataModelCodes = sDataModelCodes & objFrmMainForm.tvDataModel.Nodes.Item(iLoop).Text & ", "
                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        g_sDataModelIDs = g_sDataModelIDs & Mid(objFrmMainForm.tvDataModel.Nodes.Item(iLoop).Name, 6) & ","
                    End If
                Next iLoop
                If Len(sDataModelCodes) > 0 Then
                    sDataModelCodes = Left(sDataModelCodes, Len(sDataModelCodes) - 2)
                    g_sDataModelIDs = Left(g_sDataModelIDs, Len(g_sDataModelIDs) - 1)
                End If
            End If
            'Richard Clarke November 2008 - PIE enhancements - END
            '-----------------------------

            BuildHeaderConfirmationText = gPMConstants.PMEReturnCode.PMTrue

            iExportMode = buildExportMode()

            txtConfirmationText.Text = conEmptyString

            Dim lDataModelId As Integer
            If iExportMode AndAlso pbIeMode_Migration Then
                'lDataModelId = CLng(objFrmMainForm.txtdatamodelId)
                txtConfirmationText.Text = txtConfirmationText.Text & "Data migration" & vbCrLf
                txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel, "
                If lDataModelId > -1 Then
                    txtConfirmationText.Text = txtConfirmationText.Text & "ID=" & lDataModelId & vbCrLf
                Else
                    txtConfirmationText.Text = txtConfirmationText.Text & "none" & vbCrLf
                End If

            Else
                If g_iImportExport = 1 Then
                    'If objFrmMainForm.cboDataModel.TableName = "None" Then
                    '    objFrmMainForm.cboDataModel.TableName = "gis_data_model"
                    '    objFrmMainForm.cboDataModel.RefreshList
                    'End If
                End If

                txtConfirmationText.Text = txtConfirmationText.Text & "System type is: " & IIf(g_bIsUnderwriting, "Underwriting", "Broking") & vbCrLf
                If g_iImportExport = 0 Then
                    'txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel(s) Code=" & g_DataModelCode & vbCrLf
                    txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel(s) Code=" & sDataModelCodes & vbCrLf
                Else
                    'Richard Clarke November 2008 - PIE enhancements
                    'txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel(s), " & objFrmMainForm.cboDataModel.ItemCaption & vbCrLf
                    txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel(s) " & sDataModelCodes & vbCrLf

                End If
            End If
            If Not v_sVersionNumber = conEmptyString Then
                txtConfirmationText.Text = txtConfirmationText.Text & "Export Tool Version Number is: " & v_sVersionNumber & vbCrLf
                If CStr(v_sVersionNumber) <> conVersionNumber Then
                    txtConfirmationText.Text = txtConfirmationText.Text & "    This differs to the Import Tool Version Number of " & conVersionNumber & vbCrLf
                    txtConfirmationText.Text = txtConfirmationText.Text & "    If in doubt, do not proceed further with this import." & vbCrLf
                End If
            End If
            txtConfirmationText.Text = txtConfirmationText.Text & "Registry settings are: " & IIf(iExportMode And pbIeMode_Registry, "Included", "Excluded") & vbCrLf
            txtConfirmationText.Text = txtConfirmationText.Text & "Document templates are: " & IIf(iExportMode And pbIeMode_Documents, "Included", "Excluded") & vbCrLf
            txtConfirmationText.Text = txtConfirmationText.Text & "Rule files: " & IIf(iExportMode And pbIeMode_RuleFiles, "Included", "Excluded") & vbCrLf
            'txtConfirmationText.Text = txtConfirmationText.Text & "Risk Groups/Codes: " & IIf(iExportMode And pbIeMode_RiskGroupsCodes, "Included", "Excluded") & vbCrLf
            'txtConfirmationText.Text = txtConfirmationText.Text & "3d Lookups: " & IIf(iExportMode And pbIeMode_3DLookups, "Included", "Excluded") & vbCrLf
            txtConfirmationText.Text = txtConfirmationText.Text & "User Defined Lists: " & IIf(iExportMode And pbIeMode_UDLs, "Included", "Excluded") & vbCrLf
            txtConfirmationText.Text = txtConfirmationText.Text & "User Authority Rules: " & IIf(iExportMode And pbIeMode_UARs, "Included", "Excluded") & vbCrLf
            'Richard Clarke 23/02/2009 - added new options to confirmation text
            txtConfirmationText.Text = txtConfirmationText.Text & "SPU_ICCS% are: " & IIf(iExportMode And pbIeMode_UserStoredProcedure, "Included", "Excluded") & vbCrLf
            txtConfirmationText.Text = txtConfirmationText.Text & "All Reports are: " & IIf(iExportMode And pbIeMode_UserReports, "Included", "Excluded") & vbCrLf
            txtConfirmationText.Text = txtConfirmationText.Text & "SPU_Report% are: " & IIf(iExportMode And pbIeMode_UserReportsSPU, "Included", "Excluded") & vbCrLf 'PM040123


            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If Not IsNothing(v_vVersionError) Then
                txtConfirmationText.Text = txtConfirmationText.Text & vbCrLf & vbCrLf
                txtConfirmationText.Text = txtConfirmationText.Text & "These systems have database version mis-matches between" & vbCrLf
                txtConfirmationText.Text = txtConfirmationText.Text & "the source export file and the target database:" & vbCrLf
                For iLoop = 0 To UBound(v_vVersionError)
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vVersionError(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    txtConfirmationText.Text = txtConfirmationText.Text & v_vVersionError(iLoop) & vbCrLf
                Next iLoop
                txtConfirmationText.Text = txtConfirmationText.Text & vbCrLf
                txtConfirmationText.Text = txtConfirmationText.Text & "If in doubt, do not proceed further with this import." & vbCrLf
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If Not IsNothing(v_lTotalLines) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_lTotalLines. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                txtConfirmationText.Text = txtConfirmationText.Text & v_lTotalLines & " total lines to import" & vbCrLf
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If Not IsNothing(v_vVersionError) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object v_sComments. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Not v_sComments = "" Then
                    txtConfirmationText.Text = txtConfirmationText.Text & vbCrLf & vbCrLf
                    txtConfirmationText.Text = txtConfirmationText.Text & "Additional Export Comments:" & vbCrLf & vbCrLf
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_sComments. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    txtConfirmationText.Text = txtConfirmationText.Text & v_sComments
                End If
            End If

            'set import options
            objFrmMainForm.optAdditionalImportOptions(optAdditionalImportOptions_ImportRegistry).Enabled = IIf(iExportMode AndAlso pbIeMode_Registry, True, False)

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".BuildHeaderConfirmationText")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".BuildHeaderConfirmationText")

            BuildHeaderConfirmationText = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildHeaderConfirmationText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildHeaderConfirmationText", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: DoGuiDefaults
    '
    ' Description: Resets all the progress controls on the form, ready
    '              for another go.
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function DoGuiDefaults(ByVal v_iImportExport As Short, ByVal v_bClearWarning As Boolean) As Integer

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".DoGuiDefaults")

            DoGuiDefaults = gPMConstants.PMEReturnCode.PMTrue

            'don't clear if defaulting at end of import/export
            If v_bClearWarning = True Then
                objFrmMainForm.txtWarning(v_iImportExport).Text = conEmptyString
                objFrmMainForm.txtImportConfirmation.Text = conEmptyString
            End If
            'objFrmMainForm.txtDebug = conEmptyString

            objFrmMainForm.ProgressBar1(0).Visible = False
            objFrmMainForm.ProgressBar1(0).Maximum = 100
            objFrmMainForm.ProgressBar1(1).Visible = False
            objFrmMainForm.ProgressBar1(1).Maximum = 100
            'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = conEmptyString
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = conEmptyString
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 0)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 1)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)


            System.Windows.Forms.Application.DoEvents()

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".DoGuiDefaults")

            Exit Function

        Catch ex As Exception

            DoGuiDefaults = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".DoGuiDefaults")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DoGuiDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DoGuiDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddRequiredBackslash
    '
    ' Description:
    '
    ' History: 15/04/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function AddRequiredBackslash(ByVal v_sPath As String) As String

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".AddRequiredBackslash")

        Try

            AddRequiredBackslash = ""


            If Right(v_sPath, 1) <> "\" Then
                AddRequiredBackslash = "\"
            End If


            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".AddRequiredBackslash")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & "." & ACClass & ".AddRequiredBackslash")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRequiredBackslash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRequiredBackslash", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSQLServerVersion
    '
    ' Description:
    '
    ' History:
    ' RAW 02/09/2003 : CQ2158 : Created.
    ' ***************************************************************** '
    Public Function GetSQLServerVersion(ByRef r_oDatabase As dPMDAO.Database) As Integer
        Dim vSQLSvrVersion(,) As Object
        Dim nPosition As Short
        Dim lReturn As Integer

        Try

            If g_lSQLServerVersion <= 0 Then 'just read once

                g_lSQLServerVersion = -1

                lReturn = r_oDatabase.SQLSelect(sSQL:=ACMSGetVersionSQL, sSQLName:=ACMSGetVersionName, bStoredProcedure:=ACMSGetVersionStored, vResultArray:=vSQLSvrVersion)

                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Exit Function
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object vSQLSvrVersion(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                nPosition = InStr(1, vSQLSvrVersion(0, 0), ".")

                'UPGRADE_WARNING: Couldn't resolve default property of object vSQLSvrVersion(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_lSQLServerVersion = CShort(Left(vSQLSvrVersion(0, 0), nPosition - 1))

            End If

            GetSQLServerVersion = g_lSQLServerVersion
            Exit Function

        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSQLServerVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSQLServerVersion", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDeleteCascadeText
    '
    ' Description:
    '
    ' History:
    ' RAW 02/09/2003 : CQ2158 : Created.
    ' ***************************************************************** '
    Public Function GetDeleteCascadeText() As String

        'Need to know SQL Server version to see Cascade Delete is supported
        If g_lSQLServerVersion > 7 Then
            GetDeleteCascadeText = " ON DELETE CASCADE "
        Else
            GetDeleteCascadeText = ""
        End If

    End Function

    Public Function GetFilePath() As String
        Dim sPath As String = objFrmMainForm.txtFilePath(g_iImportExport).Text()
        If sPath.EndsWith("\") Then
            sPath = sPath.Remove(sPath.LastIndexOf("\"), 1)
        End If
        Return sPath
    End Function

    Public Function GetUMLDirectoryPath() As String
        Return GetFilePath() & "\UMLS"
    End Function
End Module