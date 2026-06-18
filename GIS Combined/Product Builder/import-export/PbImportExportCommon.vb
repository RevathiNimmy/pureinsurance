Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module PbImportExportCommon

    'this is used to detect shift key for debug purposes
    Public Declare Function GetKeyState Lib "user32" (ByVal lngVirtKey As Integer) As Short

    'Public Const conVersionNumber As String = "1.5"
    'Public Const conVersionNumber As String = "1.6"  'modified key variables from ints to longs due to scalability issues.
    'Public Const conVersionNumber As String = "1.7"  ' RAW 02/09/2003 : CQ2158 : added functions to build "ON DELETE CASCADE" text - but only if DB supports it
    'Public Const conVersionNumber As String = "1.7.1" ' CLG 08/10/2003 scalability changes
    'From now on only increment the second number if there is a database or major functional change
    'Public Const conVersionNumber As String = "1.7.2" ' RAG 09/10/2003 a couple of fixes for Folgate (search for ' RAG 2003-10-09)
    'Public Const conVersionNumber As String = "1.7.3" ' CLG 20/10/2003 Add table specific flag to prevent strings being trimmed. 1.8.5 use spaces for formatting on screens
    'Public Const conVersionNumber As String = "1.7.4" ' PW271003 - CQ1359 - pmuser_source has become pmuser_source_allowed
    'Public Const conVersionNumber As String = "1.7.5" ' CLG 17/12/2003 - 1.8.6/1.8.5 split re-build
    'Public Const conVersionNumber As String = "1.7.6" ' CLG 04/02/2004 - Added warning message and supervisor only check
    'Public Const conVersionNumber As String = "1.7.7" ' CLG 16/03/2004 - Improved error checking and reporting
    'Public Const conVersionNumber As String = "1.7.8" ' CLG 17/05/2004 - Fixed export of GIS lists
    Public Const conVersionNumber As String = "1.7.9" ' CLG 16/12/2004 - Fixed accumulations and use of unique_number

    ' Username.
    Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New FixedLengthString(30)

    ' User ID
    Public g_iUserID As Integer

    ' Calling Application
    Public g_sCallingAppName As String = ""
    ' Source ID
    Public g_iSourceID As Integer
    ' Language ID
    Public g_iLanguageID As Integer
    ' Currency ID
    Public g_iCurrencyID As Integer
    ' LogLevel
    Public g_iLogLevel As Integer

    'database connection
    Public g_oDatabase As dPMDAO.Database

    ' RAW 02/09/2003 : CQ2158 : added
    Private g_lSQLServerVersion As Integer

    'true if keys are cloned between databases
    Public g_CloneDbKeys As Boolean

    'underwriting/broking flag
    Public g_bIsUnderwriting As Boolean

    'Binary File Number
    Public g_iFileNumber As Integer

    ' Flag to stop any further processing if user chooses to cancel
    Public g_bStopProcessing As Boolean
    Public g_bExportInProgress As Boolean

    'import=0 export=1
    Public g_iImportExport As Integer

    Public g_DataModelId As Integer = 0

    Public g_DataModelCode As String = ""
    Public g_cParentPK As Collection 'dictionary collection of import parent keys
    Public g_lSiriusUserId As Integer

    Public g_lExportMode As gPMConstants.PMEReturnCode

    Public Const conDisable As Integer = 0
    Public Const conEnable As Integer = 1

    'holds the array that controls export of objects
    Public g_aIeControl() As Object
    Public Const pbIeControl_objectId As Integer = 0
    Public Const pbIeControl_objectName As Integer = 1
    Public Const pbIeControl_objectType As Integer = 2
    Public Const pbIeControl_operationMode As Integer = 3
    Public Const pbIeControl_DataModelCodeColumn As Integer = 4
    Public Const pbIeControl_IsIdentity As Integer = 5
    Public Const pbIeControl_DataModelIdColumn As Integer = 6
    Public Const pbIeControl_Flags As Integer = 7
    Public Const pbIeControl_CodeColumn As Integer = 8
    Public Const pbIeControl_HasFilename As Integer = 9
    Public Const pbIeControl_RelatedObjectId As Integer = 10
    Public Const pbIeControl_PrimaryKeyColumns As Integer = 11
    Public Const pbIeControl_WhereClause As Integer = 12
    Public Const pbIeControl_ParentIdColumn As Integer = 13
    Public Const pbIeControl_GisInsurerIdColumn As Integer = 14

    'pbIeControl_Flags values
    Public Const pbIeControl_Flags__IsCaption As Integer = 1 'this table has a caption
    Public Const pbIeControl_Flags__uniqueNumber As Integer = 2 'need to update unique_number for this
    Public Const pbIeControl_Flags__deleteBeforeAdd0 As Integer = 4 'delete the contents before adding, use column 0
    Public Const pbIeControl_Flags__CreatedByOrModifiedBy As Integer = 8
    Public Const pbIeControl_Flags__dontTrimStrings As Integer = 16 'don't trim strings (strings are by default trimmed to save size)

    'Special UDL Table Definition Array
    Public Const udlTable_columnName As Integer = 0
    Public Const udlTable_columnDatatype As Integer = 1
    Public Const udlTable_columnSize As Integer = 2
    Public Const udlTable_columnPrecision As Integer = 3
    Public Const udlTable_columnScale As Integer = 4
    Public Const udlTable_columnNullability As Integer = 5
    Public Const udlTable_columnIsIdentity As Integer = 6
    Public Const udlTable_columnPKFlag As Integer = 7

    'ignore flag for column definitions
    Public Const pbIgnoreFlag As Integer = -1

    'holds each table's column definitions
    Public g_aIeTableDefinitions() As Object '
    Public Const pbIeTableDefinitions_objectName As Integer = 0
    Public Const pbIeTableDefinitions_columnCount As Integer = 1
    Public Const pbIeTableDefinitions_columnArray As Integer = 2
    Public Const pbIeTableDefinitions_exportedColumns As Integer = 3

    Public Const pbIeTableDefinitions_columnName As Integer = 0
    Public Const pbIeTableDefinitions_columnType As Integer = 1
    Public Const pbIeTableDefinitions_columnSize As Integer = 2

    'sp_msHelpColumns constants
    Public Const sp_msHelpColumns_columnName As Integer = 0
    Public Const sp_msHelpColumns_columnType As Integer = 2
    Public Const sp_msHelpColumns_columnSize As Integer = 3
    Public Const sp_msHelpColumns_columnPrecision As Integer = 4
    Public Const sp_msHelpColumns_columnScale As Integer = 5
    Public Const sp_msHelpColumns_columnNull As Integer = 9
    Public Const sp_msHelpColumns_columnIdentity As Integer = 10
    Public Const sp_msHelpColumns_columnFlags As Integer = 11

    Public Const conPrimaryKey As Integer = 4
    Public Const conPKColumn As Integer = 0

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
    Public Const pbIeMode_3DLookups As Integer = 2048
    Public Const pbIeMode_PBDocsOnly As Integer = 4096
    Public Const pbIeMode_UDLs As Integer = 8192

    Public Const pbIeMode_All As Integer = &HFFFFFFFF
    Public Const pbIeMode_Default As Integer = pbIeMode_DataModel + pbIeMode_Registry + pbIeMode_RuleFiles + pbIeMode_Documents + pbIeMode_RiskGroupsCodes + pbIeMode_3DLookups

    'object types
    Public Const pbIeOt_dbTable_fixed As Integer = 0
    Public Const pbIeOt_dbTable_userdefined As Integer = 1
    Public Const pbIeOt_dbTable_child As Integer = 2
    Public Const pbIeOt_RegSetting As Integer = 3
    Public Const pbIeOt_DerivedDefAndValRuleFile As Integer = 4
    Public Const pbIeOt_DerivedRatingRuleFile As Integer = 5
    Public Const pbIeOt_Header As Integer = 6
    Public Const pbIeOt_RuleFile As Integer = 7
    Public Const pbIeOt_DocumentTemplate As Integer = 8
    Public Const pbIeOt_UserDefinedList As Integer = 9
    Public Const pbIeOt_UserDefinedListHeader As Integer = 10
    Public Const pbIeOt_DatabaseValue As Integer = 11
    Public Const pbIeOt_GisList As Integer = 12
    Public Const pbIeOt_GSDLookup As Integer = 13
    Public Const pbIeOt_GPLookup As Integer = 14
    Public Const pbIeOt_Ignore As Integer = 15
    Public Const pbIeOt_FixedTableColumns As Integer = 16
    Public Const pbIeOt_RiskGroupsCodes As Integer = 17

    'database types
    Public Const pbIeDbt_Header As Integer = 0
    Public Const pbIeDbt_pmlogicaldatabase As Integer = 1
    Public Const pbIeDbt_FixedTableColumns As Integer = 2
    Public Const pbIeDbt_RegSetting_1 As Integer = 3
    Public Const pbIeDbt_RegSetting_2 As Integer = 4
    Public Const pbIeDbt_RegSetting_3 As Integer = 5
    Public Const pbIeDbt_RegSetting_4 As Integer = 6
    Public Const pbIeDbt_RegSetting_5 As Integer = 7
    Public Const pbIeDbt_RegSetting_6 As Integer = 8
    Public Const pbIeDbt_RegSetting_7 As Integer = 9
    Public Const pbIeDbt_RegSetting_8 As Integer = 10
    Public Const pbIeDbt_RegSetting_9 As Integer = 11
    Public Const pbIeDbt_RegSetting_10 As Integer = 12
    Public Const pbIeDbt_RegSetting_11 As Integer = 13
    Public Const pbIeDbt_RegSetting_12 As Integer = 14
    Public Const pbIeDbt_RegSetting_13 As Integer = 15
    Public Const pbIeDbt_RegSetting_14 As Integer = 16
    Public Const pbIeDbt_RegSetting_15 As Integer = 17
    Public Const pbIeDbt_gis_data_model As Integer = 18
    Public Const pbIeDbt_gis_qem_usage As Integer = 19
    Public Const pbIeDbt_gis_qem As Integer = 20
    Public Const pbIeDbt_gis_data_model_business As Integer = 21
    Public Const pbIeDbt_gis_business_type As Integer = 22
    Public Const pbIeDbt_gis_scheme_group As Integer = 23
    Public Const pbIeDbt_gis_object As Integer = 24
    Public Const pbIeDbt_gis_property As Integer = 25
    Public Const pbIeDbt_gis_property_lookup As Integer = 26
    Public Const pbIeDbt_gis_screen As Integer = 27
    Public Const pbIeDbt_gis_screen_detail As Integer = 28
    Public Const pbIeDbt_gis_user_def_header As Integer = 29
    Public Const pbIeDbt_gis_user_def_header_inds As Integer = 30
    Public Const pbIeDbt_gis_user_def_header_rates As Integer = 31
    Public Const pbIeDbt_gis_user_def_detail As Integer = 32
    Public Const pbIeDbt_gis_user_def_detail_rates As Integer = 33
    Public Const pbIeDbt_gis_user_def_detail_inds As Integer = 34
    Public Const pbIeDbt_product As Integer = 35
    Public Const pbIeDbt_product_risk_type_group As Integer = 36
    Public Const pbIeDbt_risk_type As Integer = 37
    Public Const pbIeDbt_risk_type_rule_set As Integer = 38
    Public Const pbIeDbt_risk_type_group As Integer = 39
    Public Const pbIeDbt_risk_type_usage As Integer = 40
    Public Const pbIeDbt_Risk_Group As Integer = 41
    Public Const pbIeDbt_Risk_Code As Integer = 42
    Public Const pbIeDbt_claim_lookup As Integer = 43
    Public Const pbIeDbt_claim_party_type As Integer = 44
    Public Const pbIeDbt_risk_data_definition As Integer = 45
    Public Const pbIeDbt_service_type As Integer = 46
    Public Const pbIeDbt_expert_service As Integer = 47
    Public Const pbIeDbt_risk_type_expert_service As Integer = 48
    Public Const pbIeDbt_gis_scheme As Integer = 49
    Public Const pbIeDbt_gis_quote_engine As Integer = 50
    Public Const pbIeDbt_gis_scheme_audit As Integer = 51
    Public Const pbIeDbt_gis_scheme_cobol_linkage As Integer = 52
    Public Const pbIeDbt_gis_scheme_data As Integer = 53
    Public Const pbIeDbt_gis_scheme_group_member As Integer = 54
    Public Const pbIeDbt_gis_scheme_property As Integer = 55
    Public Const pbIeDbt_gis_scheme_risk_group_link As Integer = 56
    Public Const pbIeDbt_DerivedRatingRuleFile As Integer = 57
    Public Const pbIeDbt_GisList As Integer = 58
    Public Const pbIeDbt_gis_list_items As Integer = 59
    Public Const pbIeDbt_gis_list_type_usage As Integer = 60
    Public Const pbIeDbt_gis_list_type As Integer = 61
    Public Const pbIeDbt_gis_list_grouping As Integer = 62
    Public Const pbIeDbt_gis_list_grouping_items As Integer = 63
    Public Const pbIeDbt_gis_rate_type As Integer = 64
    Public Const pbIeDbt_gis_rate_items As Integer = 65
    Public Const pbIeDbt_gis_find_mapping As Integer = 66
    Public Const pbIeDbt_gis_lookup_header As Integer = 67
    Public Const pbIeDbt_gis_lookup_data As Integer = 68
    Public Const pbIeDbt_DerivedDefAndValRuleFile As Integer = 69
    Public Const pbIeDbt_document_template As Integer = 70
    Public Const pbIeDbt_DocumentTemplateFile As Integer = 71
    Public Const pbIeDbt_UserDefinedList As Integer = 72
    Public Const pbIeDbt_rate_type As Integer = 73
    Public Const pbIeDbt_peril_type As Integer = 74
    Public Const pbIeDbt_peril_group As Integer = 75
    Public Const pbIeDbt_peril_type_usage As Integer = 76
    Public Const pbIeDbt_rating_section_type As Integer = 77
    Public Const pbIeDbt_sum_insured_type As Integer = 78
    Public Const pbIeDbt_commission_band As Integer = 79
    Public Const pbIeDbt_policy_section_type As Integer = 80
    Public Const pbIeDbt_RuleFile As Integer = 81
    Public Const pbIeDbt_UserDefinedListHeader As Integer = 82
    Public Const pbIeDbt_class_of_business As Integer = 83
    Public Const pbIeDbt_gis_insurer As Integer = 84
    Public Const pbIeDbt_Broking_companies As Integer = 85
    Public Const pbIeDbt_Broking_user As Integer = 86
    Public Const pbIeDbt_Fields As Integer = 87
    Public Const pbIeDbt_Queries As Integer = 88
    Public Const pbIeDbt_Currency As Integer = 89
    Public Const pbIeDbt_Language As Integer = 90
    Public Const pbIeDbt_Country As Integer = 91
    Public Const pbIeDbt_Source As Integer = 92
    Public Const pbIeDbt_Transaction_Type As Integer = 93
    Public Const pbIeDbt_PMUser_Group As Integer = 94
    Public Const pbIeDbt_PMUser As Integer = 95
    Public Const pbIeDbt_PMUser_Group_Activity As Integer = 96
    Public Const pbIeDbt_PMUser_Group_Group As Integer = 97
    Public Const pbIeDbt_PMUser_Group_User As Integer = 98
    Public Const pbIeDbt_PMUser_Source As Integer = 99
    Public Const pbIeDbt_PMSystem As Integer = 100
    Public Const pbIeDbt_PMNumber_Group As Integer = 101
    Public Const pbIeDbt_PMNumber As Integer = 102
    Public Const pbIeDbt_PMNumber_Range As Integer = 103
    Public Const pbIeDbt_PMNav_Batch As Integer = 104
    Public Const pbIeDbt_PMNav_Component As Integer = 105
    Public Const pbIeDbt_PMNav_Key As Integer = 106
    Public Const pbIeDbt_PMNav_Map As Integer = 107
    Public Const pbIeDbt_PMProc_Lock_Group As Integer = 108
    Public Const pbIeDbt_PMWrk_Task_Category As Integer = 109
    Public Const pbIeDbt_PMWrk_Task_Group As Integer = 110
    Public Const pbIeDbt_PMWrk_websites As Integer = 111
    Public Const pbIeDbt_unique_number As Integer = 112
    Public Const pbIeDbt_PMNav_Batch_Key As Integer = 113
    Public Const pbIeDbt_PMNav_Batch_Set As Integer = 114
    Public Const pbIeDbt_PMNav_Batch_Key_Value As Integer = 115
    Public Const pbIeDbt_PMNav_Batch_Record As Integer = 116
    Public Const pbIeDbt_PMNav_Get_Component_Key As Integer = 117
    Public Const pbIeDbt_PMNav_Get_Process_Key As Integer = 118
    Public Const pbIeDbt_PMNav_Get_Step_Key As Integer = 119
    Public Const pbIeDbt_PMNav_Process As Integer = 120
    Public Const pbIeDbt_PMNav_Set_Component_Key As Integer = 121
    Public Const pbIeDbt_PMNav_Set_Map_Key As Integer = 122
    Public Const pbIeDbt_PMNav_Set_Process_Key As Integer = 123
    Public Const pbIeDbt_PMNav_Set_Step_Key As Integer = 124
    Public Const pbIeDbt_PMNav_Step As Integer = 125
    Public Const pbIeDbt_PMWrk_Task As Integer = 126
    Public Const pbIeDbt_PMWrk_Task_Group_Task As Integer = 127
    Public Const pbIeDbt_PMWrk_Task_Inst_Key As Integer = 128
    Public Const pbIeDbt_PMWrk_Task_Inst_Log As Integer = 129
    Public Const pbIeDbt_PMWrk_Task_Instance As Integer = 130
    Public Const pbIeDbt_treaty As Integer = 131
    Public Const pbIeDbt_tax_band As Integer = 132
    Public Const pbIeDbt_tax_band_rate As Integer = 133
    Public Const pbIeDbt_tax_rates As Integer = 134
    Public Const pbIeDbt_tax_type As Integer = 135
    Public Const pbIeDbt_tax_type_band As Integer = 136
    Public Const pbIeDbt_accumulation As Integer = 137
    Public Const pbIeDbt_area As Integer = 138
    Public Const pbIeDbt_catastrophe_code As Integer = 139
    Public Const pbIeDbt_authority_level_type As Integer = 140
    Public Const pbIeDbt_license_type As Integer = 141
    Public Const pbIeDbt_report As Integer = 142
    Public Const pbIeDbt_report_group As Integer = 143
    Public Const pbIeDbt_report_group_contents As Integer = 144
    Public Const pbIeDbt_report_group_user_groups As Integer = 145
    Public Const pbIeDbt_PMProduct_Lookup As Integer = 146
    Public Const pbIeDbt_DocumentType As Integer = 147

    ' PW020703 - CQ1359
    Public Const pbIeDbt_RegSetting_16 As Integer = 148
    Public Const pbIeDbt_document_template_version As Integer = 149
    Public Const pbIeDbt_product_claims_workflow As Short = 150

    'i/io constants
    Public Const EndOfLineChar As Integer = 64
    Public Const ReadAccess As String = "Read"
    Public Const WriteAccess As String = "Write"
    Public Const WriteRandom As String = "WriteRandom"
    Public Const DefaultStringValue As String = "<NULL>"
    Public Const DefaultIntegerValue As Integer = -9999
    Public Const DefaultCurrencyValue As Double = -99999.99

    Public Const conKeyPath As Integer = 0
    Public Const conKeyName As Integer = 1
    Public Const conKeyValue As Integer = 2

    Public Const conDMC As String = "<DMC>"
    Public Const conMachineName As String = "<MACH_NAME>"
    Public Const conComma As String = ","
    Public Const conCommaSpace As String = ", "
    Public Const conEmptyString As String = ""
    Public Const conDot As String = "."
    Public Const conBackSlash As String = "\"

    Public Const conName As Integer = 0
    Public Const conVersion As Integer = 1

    Public Const conServer As String = "\SERVER\"
    Public Const conClient As String = "\CLIENT\"
    Public Const conSetUp As String = "\SETUP\"
    Public Const conCommon As String = "\COMMON\"

    'general program constants
    Public Const ACApp As String = "Product Builder Import/Export"
    Private Const ACClass As String = conEmptyString

    'checkbox/radio button constants
    Public Const radioExportBasedOn_DataModel As Integer = 0
    Public Const radioExportBasedOn_Scheme As Integer = 1
    Public Const radioExportBasedOn_Screen As Integer = 2
    Public Const radioExportBasedOn_Migration As Integer = 3

    'export
    Public Const chkAdditionalExportOptions_Registry As Integer = 0
    Public Const chkAdditionalExportOptions_Documents As Integer = 1
    Public Const chkAdditionalExportOptions_Rulefiles As Integer = 2
    Public Const chkAdditionalExportOptions_RiskGroupsCodes As Integer = 3
    Public Const chkAdditionalExportOptions_3DRatings As Integer = 4
    Public Const chkAdditionalExportOptions_PBDocsOnly As Integer = 5
    Public Const chkAdditionalExportOptions_UDLs As Integer = 6
    'import
    Public Const optAdditionalImportOptions_ImportRegistry As Integer = 0
    Public Const optAdditionalImportOptions_DefultRegistry As Integer = 1
    Public Const optAdditionalImportOptions_IgnoreRegistry As Integer = 2

    '*************
    ' MEvans : 08-11-2003 : CQ3049
    Public Const ACSPGenDataTempDataModelName As String = "XXXDATAMODELNAME"
    Public Const ACSPGenDataTempDataModelTableName As String = "XXXDATAMODELTABLENAME"
    Public Const ACSPGenDataTempDataModelTableAlias As String = "XXXDATAMODELTABLEALIAS"

    Public g_UserSPU() As Object

    Public Const kMaxUMLDetailsRecordsToFetch As Integer = 150
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

        Dim Index As Integer = -1
        Try
            If Not (r_vGrowingArray Is Nothing) Then
                Index = r_vGrowingArray.GetUpperBound(0)
            End If
        Catch
        End Try
        Index += 1
        ReDim Preserve r_vGrowingArray(Index)

        r_vGrowingArray(Index) = v_vToAdd

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

        objFrmMainForm.txtWarning(g_iImportExport).Text = objFrmMainForm.txtWarning(g_iImportExport).Text & v_sText & Environment.NewLine
        Application.DoEvents()

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

        objFrmMainForm.txtDebug.Text = objFrmMainForm.txtDebug.Text & v_sText & Environment.NewLine
        Application.DoEvents()

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
    Public Function BuildHeaderConfirmationText(ByRef txtConfirmationText As Control, _
                                                Optional ByVal v_sVersionNumber As String = conEmptyString, _
                                                Optional ByVal v_sComments As String = "", _
                                                Optional ByVal v_vVersionError() As Object = Nothing, _
                                                Optional ByVal v_lTotalLines As Object = Nothing, _
                                                Optional nExportedMode As Integer = 0) As Integer


        Dim nResult As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".BuildHeaderConfirmationText")

        Try

            Dim nExportMode As gPMConstants.PMEReturnCode

            nResult = gPMConstants.PMEReturnCode.PMTrue
            If nExportedMode > 0 Then
                nExportMode = nExportedMode
            Else
                nExportMode = buildExportMode()
            End If

            txtConfirmationText.Text = conEmptyString

            Dim nDataModelId As Integer
            If nExportMode And pbIeMode_Migration Then
                nDataModelId = CInt(objFrmMainForm.txtdatamodelId.Text)

                txtConfirmationText.Text = txtConfirmationText.Text & "Data migration" & Strings.Chr(13) & Strings.Chr(10)

                txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel, "
                If nDataModelId > -1 Then

                    txtConfirmationText.Text = txtConfirmationText.Text & "ID=" & CStr(nDataModelId) & Strings.Chr(13) & Strings.Chr(10)
                Else

                    txtConfirmationText.Text = txtConfirmationText.Text & "none" & Strings.Chr(13) & Strings.Chr(10)
                End If

            Else

                If g_iImportExport = 1 Then
                    If objFrmMainForm.cboDataModel.TableName = "None" Then
                        objFrmMainForm.cboDataModel.TableName = "gis_data_model"
                        objFrmMainForm.cboDataModel.RefreshList()
                    End If
                End If

                txtConfirmationText.Text = txtConfirmationText.Text & "System type is: " & (IIf(g_bIsUnderwriting, "Underwriting", "Broking")) & Strings.Chr(13) & Strings.Chr(10)
                If g_iImportExport = 0 Then

                    txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel, Code=" & g_DataModelCode & Strings.Chr(13) & Strings.Chr(10)
                Else

                    txtConfirmationText.Text = txtConfirmationText.Text & "Content is based upon: Datamodel, " & objFrmMainForm.cboDataModel.ItemCaption & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If
            If Not (v_sVersionNumber = conEmptyString) Then

                txtConfirmationText.Text = txtConfirmationText.Text & "Export Tool Version Number is: " & v_sVersionNumber & Strings.Chr(13) & Strings.Chr(10)
                If v_sVersionNumber <> conVersionNumber Then

                    txtConfirmationText.Text = txtConfirmationText.Text & "    This differs to the Import Tool Version Number of " & conVersionNumber & Strings.Chr(13) & Strings.Chr(10)

                    txtConfirmationText.Text = txtConfirmationText.Text & "    If in doubt, do not proceed further with this import." & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If

            txtConfirmationText.Text = txtConfirmationText.Text & "Registry settings are: " & (IIf(nExportMode And pbIeMode_Registry, "Included", "Excluded")) & Strings.Chr(13) & Strings.Chr(10)

            txtConfirmationText.Text = txtConfirmationText.Text & "Document templates are: " & (IIf(nExportMode And pbIeMode_Documents, "Included", "Excluded")) & Strings.Chr(13) & Strings.Chr(10)

            txtConfirmationText.Text = txtConfirmationText.Text & "Rule files: " & (IIf(nExportMode And pbIeMode_RuleFiles, "Included", "Excluded")) & Strings.Chr(13) & Strings.Chr(10)

            txtConfirmationText.Text = txtConfirmationText.Text & "Risk Groups/Codes: " & (IIf(nExportMode And pbIeMode_RiskGroupsCodes, "Included", "Excluded")) & Strings.Chr(13) & Strings.Chr(10)

            txtConfirmationText.Text = txtConfirmationText.Text & "3d Lookups: " & (IIf(nExportMode And pbIeMode_3DLookups, "Included", "Excluded")) & Strings.Chr(13) & Strings.Chr(10)

            txtConfirmationText.Text = txtConfirmationText.Text & "User Defined Lists: " & (IIf(nExportMode And pbIeMode_UDLs, "Included", "Excluded")) & Strings.Chr(13) & Strings.Chr(10)

            If Not Information.IsNothing(v_vVersionError) Then

                txtConfirmationText.Text = txtConfirmationText.Text & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

                txtConfirmationText.Text = txtConfirmationText.Text & "These systems have database version mis-matches between" & Strings.Chr(13) & Strings.Chr(10)

                txtConfirmationText.Text = txtConfirmationText.Text & "the source export file and the target database:" & Strings.Chr(13) & Strings.Chr(10)
                For Each v_vVersionError_item As Object In v_vVersionError

                    txtConfirmationText.Text = txtConfirmationText.Text & CStr(v_vVersionError_item) & Strings.Chr(13) & Strings.Chr(10)
                Next v_vVersionError_item

                txtConfirmationText.Text = txtConfirmationText.Text & Strings.Chr(13) & Strings.Chr(10)

                txtConfirmationText.Text = txtConfirmationText.Text & "If in doubt, do not proceed further with this import." & Strings.Chr(13) & Strings.Chr(10)
            End If

            If Not Information.IsNothing(v_lTotalLines) Then

                txtConfirmationText.Text = txtConfirmationText.Text & CStr(v_lTotalLines) & " total lines to import" & Strings.Chr(13) & Strings.Chr(10)
            End If

            If Not Information.IsNothing(v_vVersionError) Then
                If Not (v_sComments = "") Then

                    txtConfirmationText.Text = txtConfirmationText.Text & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

                    txtConfirmationText.Text = txtConfirmationText.Text & "Additional Export Comments:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

                    txtConfirmationText.Text = txtConfirmationText.Text & v_sComments
                End If
            End If

            'set import options
            objFrmMainForm.optAdditionalImportOptions(optAdditionalImportOptions_ImportRegistry).Enabled = nExportMode And pbIeMode_Registry

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".BuildHeaderConfirmationText")

            Return nResult

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".BuildHeaderConfirmationText")

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildHeaderConfirmationText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildHeaderConfirmationText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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
    Public Function DoGuiDefaults(ByVal v_iImportExport As Integer, ByVal v_bClearWarning As Boolean) As Integer

        Dim result As Integer = 0
        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".DoGuiDefaults")

            result = gPMConstants.PMEReturnCode.PMTrue

            'don't clear if defaulting at end of import/export
            If v_bClearWarning Then
                objFrmMainForm.txtWarning(v_iImportExport).Text = conEmptyString
                objFrmMainForm.txtImportConfirmation.Text = conEmptyString
            End If
            'objFrmMainForm.txtDebug = conEmptyString

            objFrmMainForm.ProgressBar1(0).Visible = False
            objFrmMainForm.ProgressBar1(0).Maximum = 100
            objFrmMainForm.ProgressBar1(1).Visible = False
            objFrmMainForm.ProgressBar1(1).Maximum = 100
            objFrmMainForm.StatusBar1(0).Items.Item(0).Text = conEmptyString
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = conEmptyString
            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString

            Application.DoEvents()

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".DoGuiDefaults")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".DoGuiDefaults")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DoGuiDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DoGuiDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As String = String.Empty
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddRequiredBackslash")

        Try

            result = ""

            If Not v_sPath.EndsWith("\") Then
                result = "\"
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddRequiredBackslash")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddRequiredBackslash")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRequiredBackslash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRequiredBackslash", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
        Dim nPosition As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            If g_lSQLServerVersion <= 0 Then 'just read once

                g_lSQLServerVersion = -1

                lReturn = r_oDatabase.SQLSelect(sSQL:=ACMSGetVersionSQL, sSQLName:=ACMSGetVersionName, bStoredProcedure:=ACMSGetVersionStored, vResultArray:=vSQLSvrVersion)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Function
                End If

                nPosition = (CStr(vSQLSvrVersion(0, 0)).IndexOf("."c) + 1)

                g_lSQLServerVersion = CInt(CStr(vSQLSvrVersion(0, 0)).Substring(0, nPosition - 1))

            End If

            Return g_lSQLServerVersion

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSQLServerVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSQLServerVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
            Return " ON DELETE CASCADE "
        Else
            Return ""
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