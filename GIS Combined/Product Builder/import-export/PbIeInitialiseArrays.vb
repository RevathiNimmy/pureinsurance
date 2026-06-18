Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles

Module PbIeInitialiseArrays

    Private Const ACClass As String = conEmptyString
    Private m_lReturn As gPMConstants.PMEReturnCode

    ''' <summary>
    ''' InitialiseControlArrays
    ''' </summary>
    ''' <param name="r_oDatabase"></param>
    ''' <param name="r_lDataModelId"></param>
    ''' <param name="r_sDataModelCode"></param>
    ''' <param name="v_generateObjectConstants"></param>
    ''' <param name="r_lSiriusUserId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InitialiseControlArrays(ByRef r_oDatabase As dPMDAO.Database, _
                                            ByRef r_lDataModelId As Integer, _
                                            ByRef r_sDataModelCode As String, _
                                            ByVal v_generateObjectConstants As Boolean, _
                                            ByRef r_lSiriusUserId As Integer) As Integer
 

        Dim nResult As Integer = 0 
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".InitialiseControlArrays")

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' PW271003 - CQ1359 - Find out whether we're running 1.8 or 1.9 as
            ' this lovely code is all shared between the two
            Dim sVersionNumber As String = "" 
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Version", r_sSettingValue:=sVersionNumber), gPMConstants.PMEReturnCode)

            'default user interface
            objFrmMainForm.txtDebug.Text = conEmptyString
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Initialising"
            objFrmMainForm.StatusBar_TextWrite("Initialiasing", 1)
            objFrmMainForm.ProgressBar1(0).Value = 1
            objFrmMainForm.ProgressBar1(0).Visible = True
            objFrmMainForm.ProgressBar1(1).Value = 1
            objFrmMainForm.ProgressBar1(1).Visible = True

            Dim r_vResults(,) As Object 
            Dim atemp() As Object, aTemp2() As Object 

            g_lExportMode = buildExportMode()

            'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Initialising"
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Control array"
            objFrmMainForm.StatusBar_TextWrite("Initialising", 1)
            objFrmMainForm.StatusBar_TextWrite("Control array", 2)

            'clear global arrays incase we re-run
            Erase g_aIeControl
            Erase g_aIeTableDefinitions

            addToArray(g_aIeControl, New Object() {pbIeDbt_Header, "header", pbIeOt_Header, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_pmlogicaldatabase, "pmlogicaldatabase", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '   write out the table formats, exported with header

            addToArray(g_aIeControl, New Object() {pbIeDbt_FixedTableColumns, "fixed_table_columns", pbIeOt_FixedTableColumns, pbIeMode_DoNotExport, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'registry settings
            ' Use <DMC> contained in conDMC constant for any registry key paths instead of the data model code here
            ' Use <MACH_NAME> contained in conMachineName for any registry key paths containing the machine name as this will change during an import to another machine.

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_1, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,DataSetsPath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_2, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,Insurers", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_3, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,LookupsPath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_4, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,QEMMethodsVersionNum", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_5, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,RulePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_6, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>\ListManagement,ServerListFileCompressed", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_7, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>\ListManagement,ServerListFilePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_8, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>\ListManagement,ServerListPrefVersion", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_9, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>\ListManagement,ServerListVersion", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_10, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,BOMRequired", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_11, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,UseRiskTypeID", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_12, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>\NB,SaveOnQuote", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_13, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\NB\<DMC>,RulePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_14, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\NB\<DMC>,SchemePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_15, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\NB\<DMC>,DictPath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_data_model, "gis_data_model", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 2, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_qem_usage, "gis_qem_usage", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_qem, "gis_qem", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_qem_usage, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_data_model_business, "gis_data_model_business", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_business_type, "gis_business_type", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model_business, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_group, "gis_scheme_group", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_business_type, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_object, "gis_object", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_property, "gis_property", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_object, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_property_lookup, "gis_property_lookup", pbIeOt_GPLookup, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_property, conEmptyString, conEmptyString, 0, 0})

            'this section writes the gis screens including child screens

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_screen, "gis_screen", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_screen_detail, "gis_screen_detail", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, pbIeControl_Flags__deleteBeforeAdd0 + pbIeControl_Flags__dontTrimStrings, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0, 0})

            'user defined lists/rate tables

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_header, "gis_user_def_header", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_header_inds, "gis_user_def_header_inds", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_header, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_header_rates, "gis_user_def_header_rates", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_header, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_detail, "gis_user_def_detail", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_header, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_detail_rates, "gis_user_def_detail_rates", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_detail, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_detail_inds, "gis_user_def_detail_inds", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_detail, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_product, "product", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_product_risk_type_group, "product_risk_type_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type, "risk_type", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_rule_set, "risk_type_rule_set", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_risk_type, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_group, "risk_type_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_usage, "risk_type_usage", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Risk_Group, "Risk_Group", pbIeOt_RiskGroupsCodes, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Risk_Code, "Risk_Code", pbIeOt_RiskGroupsCodes, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_claim_lookup, "claim_lookup", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_claim_party_type, "claim_party_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_data_definition, "risk_data_definition", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_service_type, "service_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_expert_service, "expert_service", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_expert_service, "risk_type_expert_service", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'scheme support

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme, "gis_scheme", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_qem_usage, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_quote_engine, "gis_quote_engine", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_audit, "gis_scheme_audit", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_cobol_linkage, "gis_scheme_cobol_linkage", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_data, "gis_scheme_data", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_group_member, "gis_scheme_group_member", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_property, "gis_scheme_property", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_risk_group_link, "gis_scheme_risk_group_link", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_DerivedRatingRuleFile, "DerivedRatingRuleFile", pbIeOt_DerivedRatingRuleFile, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0})

            'gis lists

            addToArray(g_aIeControl, New Object() {pbIeDbt_GisList, "gis_lists", pbIeOt_GisList, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '3d ratings (gulp)

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_items, "gis_list_items", pbIeOt_dbTable_fixed, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_type_usage, "gis_list_type_usage", pbIeOt_dbTable_fixed, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_type, "gis_list_type", pbIeOt_dbTable_fixed, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_grouping, "gis_list_grouping", pbIeOt_dbTable_child, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_grouping_items, "gis_list_grouping_items", pbIeOt_dbTable_child, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_rate_type, "gis_rate_type", pbIeOt_dbTable_child, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, pbIeDbt_gis_scheme, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_rate_items, "gis_rate_items", pbIeOt_dbTable_child, pbIeMode_3DLookups, 0, 0, 0, False, 0, 0, pbIeDbt_gis_rate_type, conEmptyString, conEmptyString, 0, 0})

            'screen support

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_find_mapping, "gis_find_mapping", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, "1,2,3", conEmptyString, 0, 0})

            'gis lookups

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_lookup_header, "gis_lookup_header", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, "1,2,3", conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_lookup_data, "gis_lookup_data", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_lookup_header, "1,2", conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_DerivedDefAndValRuleFile, "DerivedDefAndValRuleFile", pbIeOt_DerivedDefAndValRuleFile, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0})

            'document templates

            addToArray(g_aIeControl, New Object() {pbIeDbt_document_template, "document_template", pbIeOt_dbTable_fixed, pbIeMode_Documents, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_DocumentTemplateFile, "DocumentTemplateFile", pbIeOt_DocumentTemplate, pbIeMode_Documents, 0, 0, 0, False, 0, 0, pbIeDbt_document_template, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_UserDefinedList, "UDL_%", pbIeOt_UserDefinedList, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'are these underwriting only?

            addToArray(g_aIeControl, New Object() {pbIeDbt_rate_type, "rate_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_peril_type, "peril_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_peril_group, "peril_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_peril_type_usage, "peril_type_usage", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_rating_section_type, "rating_section_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_sum_insured_type, "sum_insured_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_commission_band, "commission_band", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_policy_section_type, "policy_section_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '   these definitions do not drive the export. They are just output/input definitions

            addToArray(g_aIeControl, New Object() {pbIeDbt_RuleFile, "RuleFile", pbIeOt_RuleFile, pbIeMode_DoNotExport, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_UserDefinedListHeader, "UDL_header", pbIeOt_UserDefinedListHeader, pbIeMode_DoNotExport, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'others

            addToArray(g_aIeControl, New Object() {pbIeDbt_class_of_business, "class_of_business", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'don't export gis_Insurer but must be here to update unique_number if we add a DEFAULT insurer on import

            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_insurer, "gis_insurer", pbIeOt_dbTable_fixed, pbIeMode_DoNotExport, 0, 0, 0, pbIeControl_Flags__uniqueNumber, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'these are architecture migration only. These were specifically added to support GAJ and may be a subset

            addToArray(g_aIeControl, New Object() {pbIeDbt_Broking_companies, "Broking_companies", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Broking_user, "Broking_user", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Fields, "Fields", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Queries, "Queries", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Currency, "Currency", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Language, "Language", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Country, "Country", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Source, "Source", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Transaction_Type, "Transaction_Type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct_Client_Install, "PMProduct_Client_Install", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct_Lookup, "PMProduct_Lookup", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct_Update_History, "PMProduct_Update_History", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group, "PMUser_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser, "PMUser", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group_Activity, "PMUser_Group_Activity", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group_Group, "PMUser_Group_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group_User, "PMUser_Group_User", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            ' PW271003 - CQ1359 - user_source table has changed to user_source_allowed
            ' in 1.9. Ordinarily we could have added the new one and left the old
            ' one 'cos the code ignores tables that do not exist. However, in 1.9
            ' PMUser_Source is now a View, instead of a table which messes things
            ' up when the import tries to Amend it.
            If Not sVersionNumber.StartsWith("1.9") Then

                addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Source, "PMUser_Source", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            Else

                addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Source, "PMUser_Source_Allowed", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            End If

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMSystem, "PMSystem", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNumber_Group, "PMNumber_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNumber, "PMNumber", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNumber_Range, "PMNumber_Range", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch, "PMNav_Batch", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Component, "PMNav_Component", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Key, "PMNav_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Map, "PMNav_Map", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMProc_Lock_Group, "PMProc_Lock_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Category, "PMWrk_Task_Category", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Group, "PMWrk_Task_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_websites, "PMWrk_websites", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'addToArray g_aIeControl, Array(pbIeDbt_PMMessage, "PMMessage", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'addToArray g_aIeControl, Array(pbIeDbt_pmcaption, "pmcaption", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            addToArray(g_aIeControl, New Object() {pbIeDbt_unique_number, "unique_number", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'addToArray g_aIeControl, Array(pbIeDbt_PMLock, "PMLock", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Key, "PMNav_Batch_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Set, "PMNav_Batch_Set", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Key_Value, "PMNav_Batch_Key_Value", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Record, "PMNav_Batch_Record", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Get_Component_Key, "PMNav_Get_Component_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Get_Process_Key, "PMNav_Get_Process_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Get_Step_Key, "PMNav_Get_Step_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Process, "PMNav_Process", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Component_Key, "PMNav_Set_Component_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Map_Key, "PMNav_Set_Map_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Process_Key, "PMNav_Set_Process_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Step_Key, "PMNav_Set_Step_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Step, "PMNav_Step", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task, "PMWrk_Task", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Group_Task, "PMWrk_Task_Group_Task", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Inst_Key, "PMWrk_Task_Inst_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Inst_Log, "PMWrk_Task_Inst_Log", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Instance, "PMWrk_Task_Instance", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'these are broking migration only. These were specifically added to support GAJ and may be a subset

            addToArray(g_aIeControl, New Object() {pbIeDbt_treaty, "treaty", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_band, "tax_band", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_band_rate, "tax_band_rate", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_rates, "tax_rates", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_type, "tax_type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_type_band, "tax_type_band", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'this is now included but not for export just for unique_number processing

            addToArray(g_aIeControl, New Object() {pbIeDbt_accumulation, "accumulation", pbIeOt_dbTable_fixed, pbIeMode_DoNotExport, 0, 0, 0, pbIeControl_Flags__uniqueNumber, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_area, "area", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_catastrophe_code, "catastrophe_code", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_authority_level_type, "authority_level_type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_license_type, "license_type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'addToArray g_aIeControl, Array(pbIeDbt_PMWrk_User_Quick_Start, "PMWrk_User_Quick_Start", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            'do not ship pmproduct as this will destroy the DSN settings but need its definition for setting the caption values
            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct, "PMProduct", pbIeOt_dbTable_fixed, 0, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            'reports

            addToArray(g_aIeControl, New Object() {pbIeDbt_report, "report", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_report_group, "report_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_report_group_contents, "report_group_contents", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_report_group_user_groups, "report_group_user_groups", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'pm lookups

            addToArray(g_aIeControl, New Object() {pbIeDbt_PMProduct_Lookup, "PMProduct_Lookup", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, "where lookup_table_name like 'UDL%'", 0, 0})

            'document_type

            addToArray(g_aIeControl, New Object() {pbIeDbt_DocumentType, "document_type", pbIeOt_dbTable_fixed, pbIeMode_Documents, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            ' PW020703 - CQ1359

            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_16, "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server\GIS\<DMC>,LoadSaveDBMode", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_document_template_version, "document_template_version", pbIeOt_dbTable_fixed, pbIeMode_Documents, 0, 0, 0, False, 0, 0, pbIeDbt_document_template, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_product_claims_workflow, "product_claims_workflow", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_product, conEmptyString, conEmptyString, 0, 0})


            addToArray(g_aIeControl, New Object() {pbIeDbt_product_claims_workflow, "product_claims_workflow", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_product, conEmptyString, conEmptyString, 0, 0})

            'open database and all files
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Table definitions"
            objFrmMainForm.StatusBar_TextWrite("Table definitions", 1)

            Dim sName As String = "" 
            Dim iRegSettingId As Integer 
            For iTableIndex As Integer = 0 To g_aIeControl.GetUpperBound(0)

                objFrmMainForm.ProgressBar1(1).Value = 100 / ((g_aIeControl.GetUpperBound(0) + 1) / (iTableIndex + 1))

                If iTableIndex = 142 Then
                End If

                If g_bStopProcessing Then
                    Return nResult
                Else
                    If Not v_generateObjectConstants Then

                        If iTableIndex <> (g_aIeControl(iTableIndex)(pbIeControl_objectId)) Then

                            MessageBox.Show("element=" & iTableIndex & " <> object id=" & CStr(g_aIeControl(iTableIndex)(pbIeControl_objectId)), "Fatal Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)
                            Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_child, pbIeOt_RuleFile, pbIeOt_GSDLookup, pbIeOt_GPLookup

                                sName = CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName))
                            Case pbIeOt_RegSetting
                                iRegSettingId += 1
                                sName = "RegSetting_" & iRegSettingId
                            Case pbIeOt_DerivedDefAndValRuleFile
                                sName = "DerivedDefAndValRuleFile"
                            Case pbIeOt_UserDefinedList
                                sName = "UserDefinedList"
                            Case pbIeOt_Header
                                sName = "Header"
                            Case pbIeOt_DerivedRatingRuleFile
                                sName = "DerivedRatingRuleFile"
                            Case pbIeOt_DocumentTemplate
                                sName = "DocumentTemplateFile"
                            Case pbIeOt_UserDefinedListHeader
                                sName = "UserDefinedListHeader"
                            Case pbIeOt_FixedTableColumns
                                sName = "FixedTableColumns"
                            Case pbIeOt_GisList
                                sName = "GisList"
                            Case Else
                                sName = "XXXXXXXXXXXXXXXXXXXXXXX"
                        End Select
                        writeToDebugBox("Public Const pbIeDbt_" & sName & " = " & CStr(iTableIndex))

                    End If

                    'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName))
                    objFrmMainForm.StatusBar_TextWrite(CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName)), 2)
                    'clear the temporary attribute array
                    Erase atemp
                    Erase aTemp2

                    'is this a table object
                    Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)
                        Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined, pbIeOt_dbTable_child, pbIeOt_RiskGroupsCodes

                            'get the column information for this table

                            m_lReturn = CType(GetColumnDetails(r_oDatabase:=r_oDatabase, sObjectName:=CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName)), v_iTableIndex:=iTableIndex, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, v_sColumnFilter:=""), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If g_lExportMode And pbIeMode_Migration Then

                                    writeToStatusBox("No table defintion found for: " & CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName)))
                                End If
                                'probably a 1.8.6/1.9 difference so signal as don't do anything with this

                                g_aIeControl(iTableIndex)(pbIeControl_objectType) = pbIeOt_Ignore

                                addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), 0, ""})
                            End If

                        Case pbIeOt_FixedTableColumns

                            addToArray(atemp, New Object() {"table_id", "int", 4})

                            addToArray(atemp, New Object() {"columns", "varchar", 500})

                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), atemp.GetUpperBound(0) + 1, atemp})

                        Case pbIeOt_RegSetting

                            addToArray(atemp, New Object() {"key", "varchar", 255})

                            addToArray(atemp, New Object() {"name", "varchar", 255})

                            addToArray(atemp, New Object() {"value", "varchar", 255})

                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), atemp.GetUpperBound(0) + 1, atemp})

                        Case pbIeOt_RuleFile, pbIeOt_DerivedDefAndValRuleFile, pbIeOt_DocumentTemplate, pbIeOt_DerivedRatingRuleFile, pbIeOt_GisList

                            addToArray(atemp, New Object() {"filename", "varchar", 255})

                            addToArray(atemp, New Object() {"length", "int", 4})

                            addToArray(atemp, New Object() {"contents", "varchar", 255})

                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), atemp.GetUpperBound(0) + 1, atemp})

                        Case pbIeOt_UserDefinedListHeader

                            addToArray(atemp, New Object() {"list_name", "varchar", 255})

                            addToArray(atemp, New Object() {"table_definition", "varchar", 255})

                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), atemp.GetUpperBound(0) + 1, atemp})

                        Case pbIeOt_UserDefinedList, pbIeOt_GSDLookup, pbIeOt_GPLookup

                            addToArray(atemp, New Object() {"UDL_Area_id", "smallint", 2})

                            addToArray(atemp, New Object() {"caption_id", "int", 4})

                            addToArray(atemp, New Object() {"code", "char", 10})

                            addToArray(atemp, New Object() {"Description", "varchar", 255})

                            addToArray(atemp, New Object() {"is_deleted", "tinyint", 1})

                            addToArray(atemp, New Object() {"effective_date", "datetime", 8})

                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), atemp.GetUpperBound(0) + 1, atemp})

                        Case pbIeOt_Header
                            ' Any extra elements required here to be appended rather than inserted.
                            ' The import is assuming a certain order to these elements and references these
                            ' by the numbers shown below.
                            '0 Export Mode (Schema, Data Model or screen)

                            addToArray(atemp, New Object() {"mode", "int", 4})
                            '1 Data Model Id

                            addToArray(atemp, New Object() {"LinesWritten", "int", 4})
                            '2 Lines written

                            addToArray(atemp, New Object() {"DataModelId", "int", 4})
                            '3 Data Model Code

                            addToArray(atemp, New Object() {"DataModelCode", "varchar", 255})
                            '4 System (Underwriting or Broking)

                            addToArray(atemp, New Object() {"system", "int", 4})
                            '5 Export Tool Version Number

                            addToArray(atemp, New Object() {"VersionNumber", "varchar", 255})
                            '6 Export Comments

                            addToArray(atemp, New Object() {"comment", "varchar", 255})
                            '7 Database Versions
                            addToArray(atemp, New Object() {"DBVersions", "varchar", 255})
                            'Mode to Write in Export file
                            addToArray(atemp, New Object() {"ExportMode", "int", 4})

                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), atemp.GetUpperBound(0) + 1, atemp})
                    End Select
                End If
            Next

            'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = conEmptyString
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = conEmptyString
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 0)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 1)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

            If (g_lExportMode And pbIeMode_Migration) And g_iImportExport = 1 Then
                r_lDataModelId = CInt(objFrmMainForm.txtdatamodelId.Text)

            ElseIf (g_lExportMode And pbIeMode_Migration) And g_iImportExport = 0 Then
                r_lDataModelId = -1
                writeToStatusBox("No data model specified")
                r_sDataModelCode = ""
            ElseIf g_iImportExport = 0 Then
                r_lDataModelId = -1 'read from file
            Else
                'r_lDataModelId = objFrmMainForm.cboDataModel.ItemId
            End If
            If r_lDataModelId > -1 Then
                m_lReturn = r_oDatabase.SQLSelect(sSQL:="select code from gis_data_model where gis_data_model_id = " & r_lDataModelId, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(r_vResults) Then

                    'r_sDataModelCode = CStr(r_vResults(0, 0)).Trim()
                    r_sDataModelCode = Convert.ToString(r_vResults(0, 0)).Trim()

                Else
                    nResult = gPMConstants.PMEReturnCode.PMError
                    writeToStatusBox("Could not find data model with id=" & r_lDataModelId)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
            End If

            'get the sirius user id

            r_vResults = Nothing
            m_lReturn = r_oDatabase.SQLSelect(sSQL:="select user_id from pmuser where username='sirius'", sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            If Information.IsArray(r_vResults) Then

                r_lSiriusUserId = CInt(CStr(r_vResults(0, 0)).Trim())
            Else
                nResult = gPMConstants.PMEReturnCode.PMError
                writeToStatusBox("Could not find sirius user in database")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ".InitialiseControlArrays")

            Return nResult

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ".InitialiseControlArrays")

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseControlArrays Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="InitialiseControlArrays", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetColumnDetails
    '
    ' Description:
    '
    ' History: 09/09/2002 CLG - Created.
    '          03/10/2002 SJP - Added error trapping around database
    '                           calls
    '
    ' ***************************************************************** '
    Public Function GetColumnDetails(ByRef r_oDatabase As dPMDAO.Database, ByVal sObjectName As String, ByVal v_iTableIndex As Integer, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_sColumnFilter As String) As Integer 

        Dim result As Integer = 0 
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".GetColumnDetails")

        Try

            Dim vResults(,) As Object 
            Dim atemp() As Object 
            Dim iCaptionAndDescriptionDetected As Integer 
            Dim bListStarted, bIgnorePKData As Boolean 
            Dim iColumnIndex, iCaptionColumn As Integer 
            Dim sExportedColumns As New StringBuilder 
            Dim aColumns As Object 

            result = gPMConstants.PMEReturnCode.PMTrue

            'ignore the migration tables

            If (r_aIeControl(v_iTableIndex)(pbIeControl_operationMode)) = pbIeMode_Migration Then

                addToArray(r_aIeTableDefinitions, New Object() {sObjectName, 0, "", ""})
                Return result
            End If

            With r_oDatabase

                m_lReturn = .SQLSelect(sSQL:="select name from sysobjects where name ='" & sObjectName & "' and type='u'", sSQLName:="Pre GetDMTableColumnList check", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                If Information.IsArray(vResults) And m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    .Parameters.Clear()
                    m_lReturn = .Parameters.Add(sName:="tablename", vValue:=sObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = .SQLSelect(sSQL:=ACMSGetDMTableColumnsSQL, sSQLName:=ACMSGetDMTableColumnsName, bStoredProcedure:=ACMSGetDMTableColumnsStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                    End If
                Else
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            'clear "special columns" flags
            iCaptionAndDescriptionDetected = 0 'reset caption
            bListStarted = False

            'walk the columns

            'before looking at columns check if we need to use pre-defined PK definition

            bIgnorePKData = (Strings.Len(CStr(r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns))) <> 0)

            Dim bFoundColumnFilter As Boolean 
            If Information.IsArray(vResults) Then
                If v_sColumnFilter <> "" Then

                    aColumns = v_sColumnFilter.Split(","c)

                    'PM9403 PB import/export tool displays unnecessary warnings
                    'don't bother reporting problems check tables we are not importing
                    '            If r_aIeControl(v_iTableIndex)(pbIeControl_operationMode) And g_lExportMode Then

                    If (r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount)) > aColumns.GetUpperBound(0) + 1 Then

                        writeToStatusBox("Warning: Column count differs for table " & sObjectName & ". Source=" & CStr(aColumns.GetUpperBound(0) + 1) & " Destination=" & CStr(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount)))
                    End If

                    If (r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount)) < aColumns.GetUpperBound(0) + 1 Then

                        writeToStatusBox("Error: Column count differs for table " & sObjectName & ". Source=" & CStr(aColumns.GetUpperBound(0)) & " Destination=" & CStr(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount)))
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    '            Else
                    '            End If

                    iColumnIndex = 0

                    For iColumnFilterLoop As Integer = 0 To aColumns.GetUpperBound(0)
                        bFoundColumnFilter = False

                        If aColumns(iColumnFilterLoop).Equals(vResults(sp_msHelpColumns_columnName, iColumnIndex)) Then
                            bFoundColumnFilter = True
                        End If

                        If Not bFoundColumnFilter Then

                            For iColumnIndex = 0 To vResults.GetUpperBound(1)
                                If Not bFoundColumnFilter Then

                                    If CStr(vResults(pbIeTableDefinitions_columnName, iColumnIndex)).ToLower() = CStr(aColumns(iColumnFilterLoop)).ToLower() Then
                                        bFoundColumnFilter = True

                                        ProcessColumnDefinition(r_oDatabase, r_aIeControl, vResults, sObjectName, v_iTableIndex, iColumnIndex, iCaptionAndDescriptionDetected, iCaptionColumn, bIgnorePKData, bListStarted)
                                    End If

                                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iColumnIndex) = New Object() {vResults(sp_msHelpColumns_columnName, iColumnIndex), vResults(sp_msHelpColumns_columnType, iColumnIndex), vResults(sp_msHelpColumns_columnSize, iColumnIndex)}
                                Else
                                    Exit For
                                End If
                            Next

                            If bFoundColumnFilter And iColumnIndex > vResults.GetUpperBound(1) Then
                                'stop error if found field was last one
                                iColumnIndex = 0
                            End If

                        End If
                        If Not bFoundColumnFilter Then
                            'imported column not found in target
                            'don't bother reporting problems on tables we are not importing

                            If CBool(r_aIeControl(v_iTableIndex)(pbIeControl_operationMode) And g_lExportMode) Then

                                writeToStatusBox("Error: Could not find column " & CStr(aColumns(iColumnFilterLoop)) & " in table " & sObjectName & " on the target database")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            iColumnIndex = 0
                        End If
                    Next

                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_exportedColumns) = v_sColumnFilter
                    'resize the array

                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount) = aColumns.GetUpperBound(0)

                    aColumns = r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)

                    ReDim Preserve aColumns(CInt(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount))) 

                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray) = aColumns
                    'redim preserve r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(UBound(aColumns))

                    Return result
                End If

                For iColumnIndex = 0 To vResults.GetUpperBound(1)

                    ProcessColumnDefinition(r_oDatabase, r_aIeControl, vResults, sObjectName, v_iTableIndex, iColumnIndex, iCaptionAndDescriptionDetected, iCaptionColumn, bIgnorePKData, bListStarted)

                    If CStr(r_aIeControl(v_iTableIndex)(pbIeTableDefinitions_columnType)) <> gTIMESTAMP And v_sColumnFilter = "" Then
                        If sExportedColumns.ToString() = "" Then

                            sExportedColumns = New StringBuilder(CStr(vResults(pbIeTableDefinitions_columnName, iColumnIndex)).ToLower())
                        Else

                            sExportedColumns.Append("," & CStr(vResults(pbIeTableDefinitions_columnName, iColumnIndex)).ToLower())
                        End If
                    Else
                    End If

                    'add column name, type and size to array

                    addToArray(atemp, New Object() {vResults(sp_msHelpColumns_columnName, iColumnIndex), vResults(sp_msHelpColumns_columnType, iColumnIndex), vResults(sp_msHelpColumns_columnSize, iColumnIndex)})
                Next

                Select Case iCaptionAndDescriptionDetected
                    Case 1 ' description
                    Case 2 ' caption id without description
                        If sObjectName <> "pmcaption" Then
                            MessageBox.Show("caption id and description miss-match", Application.ProductName)
                        End If
                    Case 3 'caption id and description

                        vResults(pbIeTableDefinitions_columnType, iCaptionColumn) = "captionText"
                    Case Else
                End Select

                addToArray(r_aIeTableDefinitions, New Object() {sObjectName, vResults.GetUpperBound(1) + 1, atemp, sExportedColumns.ToString()})

            Else
                If g_iImportExport = 1 Then
                    'Stick a message out to identify the table not found in source database
                    writeToStatusBox("Table " & sObjectName & " has not been found in database")
                End If

                addToArray(r_aIeTableDefinitions, New Object() {sObjectName, 0, atemp})
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ".GetColumnDetails")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ".GetColumnDetails")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetColumnDetails Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="GetColumnDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ProcessColumnDefinition
    '
    ' Description:
    '
    ' History: 18/10/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessColumnDefinition(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl() As Object, ByRef r_vResults(,) As Object, ByVal v_sObjectName As String, ByVal v_iTableIndex As Integer, ByVal v_iColumnIndex As Integer, ByRef r_iCaptionAndDescriptionDetected As Integer, ByRef r_iCaptionColumn As Integer, ByVal v_bIgnorePKData As Boolean, ByRef r_bListStarted As Boolean) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ProcessColumnDefinition")

        

            Dim vResults2 As Object

            result = gPMConstants.PMEReturnCode.PMTrue
            'search for special columns we have to do something with

            Select Case CStr(r_vResults(pbIeTableDefinitions_columnName, v_iColumnIndex)).ToLower()
                Case "gis_data_model_id"

                    r_aIeControl(v_iTableIndex)(pbIeControl_DataModelIdColumn) = v_iColumnIndex + 1
                    Debug.WriteLine("DataModelId " & v_sObjectName)
                Case "caption_id"
                    If v_sObjectName <> "pmcaption" Then 'no caption processing on pmcaption
                        r_iCaptionAndDescriptionDetected += 2

                        r_aIeControl(v_iTableIndex).SetValue(CInt(r_aIeControl(v_iTableIndex)(pbIeControl_Flags)) Or pbIeControl_Flags__IsCaption, pbIeControl_Flags)
                    End If
                Case "description"
                    r_iCaptionAndDescriptionDetected += 1
                    r_iCaptionColumn = v_iColumnIndex
                    Debug.WriteLine("Caption " & v_sObjectName)
                Case "code"
                    'if this column is called 'code' signal as "code used for search" unless flagged as ignore

                    If (r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn)) = pbIgnoreFlag Then

                        r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn) = 0
                    Else

                        r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn) = v_iColumnIndex + 1
                        Debug.WriteLine("Code " & v_sObjectName)
                    End If
                Case "filename", "file_name"

                    If (r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename)) <> 0 And (r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename)) <> v_iColumnIndex + 1 Then
                        MessageBox.Show("whoops more than 1 filename", Application.ProductName)
                    Else

                        r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename) = v_iColumnIndex + 1
                    End If

                    Debug.WriteLine("filename " & v_sObjectName)
                Case "parent_id", "parentid"

                    r_aIeControl(v_iTableIndex)(pbIeControl_ParentIdColumn) = v_iColumnIndex + 1

                    Debug.WriteLine("filename " & v_sObjectName)

                Case "created_by_id", "modified_by_id"

                    r_aIeControl(v_iTableIndex).SetValue(CInt(r_aIeControl(v_iTableIndex)(pbIeControl_Flags)) Or pbIeControl_Flags__CreatedByOrModifiedBy, pbIeControl_Flags)

                Case "gis_insurer_id"

                    r_aIeControl(v_iTableIndex)(pbIeControl_GisInsurerIdColumn) = v_iColumnIndex + 1
            End Select

            ' Control array may already have primary key details contained in the list so ignore
            ' the next section if this is the case.

            If Not v_bIgnorePKData Then

                If CBool(r_vResults(sp_msHelpColumns_columnFlags, v_iColumnIndex) And conPrimaryKey) Then
                    If r_bListStarted Then

                        r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns) = CStr(r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns)) & "," & CStr(v_iColumnIndex + 1)
                    Else

                        r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns) = v_iColumnIndex + 1
                        r_bListStarted = True
                    End If
                End If
            End If

            'check the first column to see if it is an identity column
            'If v_iColumnIndex = 0 Then

            m_lReturn = r_oDatabase.SQLSelect(sSQL:="SELECT COLUMNPROPERTY( OBJECT_ID('" & v_sObjectName & "'),'" & _
                        CStr(r_vResults(pbIeTableDefinitions_columnName, v_iColumnIndex)) & "','IsIdentity')", sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults2)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (vResults2(0, 0)) = 1 Then

                r_vResults(pbIeTableDefinitions_columnType, v_iColumnIndex) = "identity"

                r_aIeControl(v_iTableIndex)(pbIeControl_IsIdentity) = True
                Debug.WriteLine("Identity " & v_sObjectName)
            End If
            'End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".ProcessColumnDefinition")

            Return result

    End Function
End Module

