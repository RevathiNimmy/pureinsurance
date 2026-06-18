Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module PbIeInitialiseArrays

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name: InitialiseControlArrays
    '
    ' Description:
    '
    ' History: 30/08/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function InitialiseControlArrays(ByRef r_oDatabase As dPMDAO.Database, ByRef r_lDataModelId() As Integer, ByRef r_sDataModelCode() As String, ByVal v_generateObjectConstants As Boolean, ByRef r_lSiriusUserId As Integer) As Integer

        'ByRef r_lDataModelId As Long
        'ByRef r_sDataModelCode as String ' Richard Clarke November 2008 - PIE enhancements

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & ".InitialiseControlArrays")

        Try

            InitialiseControlArrays = gPMConstants.PMEReturnCode.PMTrue

            ' PW271003 - CQ1359 - Find out whether we're running 1.8 or 1.9 as
            ' this lovely code is all shared between the two
            Dim sVersionNumber As String
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Version", r_sSettingValue:=sVersionNumber)

            'default user interface
            objFrmMainForm.txtDebug.Text = conEmptyString
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Initialising"
            objFrmMainForm.StatusBar_TextWrite("Initialiasing", 0)
            objFrmMainForm.ProgressBar1(0).Value = 1
            objFrmMainForm.ProgressBar1(0).Visible = True
            objFrmMainForm.ProgressBar1(1).Value = 1
            objFrmMainForm.ProgressBar1(1).Visible = True

            Dim r_vResults(,) As Object
            Dim r_vResults2 As Object
            Dim atemp() As Object
            Dim aTemp2() As Object
            Dim iTableIndex As Short
            Dim iColumnIndex As Short

            g_lExportMode = buildExportMode()

            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Initialising"
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Control array"

            objFrmMainForm.StatusBar_TextWrite("Initialising", 0)
            objFrmMainForm.StatusBar_TextWrite("Control array", 1)

            'clear global arrays incase we re-run
            Erase g_aIeControl
            Erase g_aIeTableDefinitions
            Erase g_aParentForeignKeys

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Header, "header", pbIeOt_Header, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_pmlogicaldatabase, "pmlogicaldatabase", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '   write out the table formats, exported with header
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_FixedTableColumns, "fixed_table_columns", pbIeOt_FixedTableColumns, pbIeMode_DoNotExport, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'registry settings
            ' Use <DMC> contained in conDMC constant for any registry key paths instead of the data model code here
            ' Use <MACH_NAME> contained in conMachineName for any registry key paths containing the machine name as this will change during an import to another machine.
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_1, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,DataSetsPath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_2, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,Insurers", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_3, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,LookupsPath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_4, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,QEMMethodsVersionNum", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_5, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,RulePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_6, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>\ListManagement,ServerListFileCompressed", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_7, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>\ListManagement,ServerListFilePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_8, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>\ListManagement,ServerListPrefVersion", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_9, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>\ListManagement,ServerListVersion", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_10, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,BOMRequired", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_11, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,UseRiskTypeID", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_12, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>\NB,SaveOnQuote", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_13, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\NB\<DMC>,RulePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_14, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\NB\<DMC>,SchemePath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_15, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\NB\<DMC>,DictPath", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_data_model, "gis_data_model", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 2, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_qem_usage, "gis_qem_usage", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_qem, "gis_qem", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_qem_usage, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_data_model_business, "gis_data_model_business", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_business_type, "gis_business_type", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model_business, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_scheme_group, "gis_scheme_group", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_business_type, conEmptyString, conEmptyString, 0, 0})

            'user defined lists/rate tables
            'JB Jun 10 Changed the table order as per hierarchy, parent first and then child
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_header, "gis_user_def_header", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_object, "gis_object", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_data_model, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_property, "gis_property", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_object, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_property_lookup, "gis_property_lookup", pbIeOt_GPLookup, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_property, conEmptyString, conEmptyString, 0, 0})

            'this section writes the gis screens including child screens
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_screen, "gis_screen", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'Richard Clarke November 2008 - PIE enhancements - OLD
            'addToArray g_aIeControl, Array(pbIeDbt_gis_screen_detail, "gis_screen_detail", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, pbIeControl_Flags__deleteBeforeAdd0 + pbIeControl_Flags__dontTrimStrings, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_screen_detail, "gis_screen_detail", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, pbIeControl_Flags__dontTrimStrings, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0, 0})

            'user defined lists/rate tables
            '    addToArray g_aIeControl, Array(pbIeDbt_gis_user_def_header, "gis_user_def_header", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_header_inds, "gis_user_def_header_inds", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_header, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_header_rates, "gis_user_def_header_rates", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_header, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_detail, "gis_user_def_detail", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_header, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_detail_rates, "gis_user_def_detail_rates", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_detail, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_user_def_detail_inds, "gis_user_def_detail_inds", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_user_def_detail, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_product, "product", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'JB Aug 10 Need to export before product_risk_type_group
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_group, "risk_type_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_product_risk_type_group, "product_risk_type_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'JB Feb 10
            '    addToArray g_aIeControl, Array(pbIeDbt_risk_type, "risk_type", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0, 0)
            '    addToArray g_aIeControl, Array(pbIeDbt_risk_type_rule_set, "risk_type_rule_set", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_risk_type, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type, "risk_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_rule_set, "risk_type_rule_set", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '    addToArray g_aIeControl, Array(pbIeDbt_risk_type_group, "risk_type_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_usage, "risk_type_usage", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Risk_Group, "Risk_Group", pbIeOt_RiskGroupsCodes, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Risk_Code, "Risk_Code", pbIeOt_RiskGroupsCodes, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_claim_lookup, "claim_lookup", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_claim_party_type, "claim_party_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_data_definition, "risk_data_definition", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_service_type, "service_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_expert_service, "expert_service", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_risk_type_expert_service, "risk_type_expert_service", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})


            'gis lists
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_GisList, "gis_lists", pbIeOt_GisList, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '3d ratings (gulp)
            'pbIeMode_3DLookups
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_items, "gis_list_items", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'JB Jun 10 Changed the table order as per hierarchy, parent first and then child
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_type, "gis_list_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_list_type_usage, "gis_list_type_usage", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'addToArray g_aIeControl, Array(pbIeDbt_gis_list_type, "gis_list_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)

            'screen support
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_find_mapping, "gis_find_mapping", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, "1,2,3", conEmptyString, 0, 0})

            'gis lookups
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_lookup_header, "gis_lookup_header", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, "1,2,3", conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_lookup_data, "gis_lookup_data", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_lookup_header, "1,2", conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_DerivedDefAndValRuleFile, "DerivedDefAndValRuleFile", pbIeOt_DerivedDefAndValRuleFile, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_gis_screen, conEmptyString, conEmptyString, 0})

            'document templates
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_document_template, "document_template", pbIeOt_dbTable_fixed, pbIeMode_Documents, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_DocumentTemplateFile, "DocumentTemplateFile", pbIeOt_DocumentTemplate, pbIeMode_Documents, 0, 0, 0, False, 0, 0, pbIeDbt_document_template, conEmptyString, conEmptyString, 0, 0})

            'UDL% tables
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_UserDefinedList, "UDL_%", pbIeOt_UserDefinedList, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '    'others ['JB Nov 09 Changed the table order as per hierarchy, parent first and then child]
            ' RI Band/Tax group is needed as it is referentially integrated with peril_type so it has to be imported first
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_RI_Band, "RI_Band", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_class_of_business, "class_of_business", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_group, "Tax_Group", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'are these underwriting only?
            '[JB Nov 09 Changed the table order as per hierarchy, parent first and then child]
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_commission_band, "commission_band", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_rate_type, "rate_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_peril_type, "peril_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_peril_group, "peril_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_peril_type_usage, "peril_type_usage", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_rating_section_type, "rating_section_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_sum_insured_type, "sum_insured_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            '    addToArray g_aIeControl, Array(pbIeDbt_commission_band, "commission_band", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_policy_section_type, "policy_section_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '   these definitions do not drive the export. They are just output/input definitions
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_RuleFile, "RuleFile", pbIeOt_RuleFile, pbIeMode_DoNotExport, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_UserDefinedListHeader, "UDL_header", pbIeOt_UserDefinedListHeader, pbIeMode_DoNotExport, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '    'others
            '    addToArray g_aIeControl, Array(pbIeDbt_class_of_business, "class_of_business", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)

            'don't export gis_Insurer but must be here to update unique_number if we add a DEFAULT insurer on import
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_gis_insurer, "gis_insurer", pbIeOt_dbTable_fixed, pbIeMode_DoNotExport, 0, 0, 0, pbIeControl_Flags__uniqueNumber, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'these are architecture migration only. These were specifically added to support GAJ and may be a subset
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Broking_companies, "Broking_companies", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Broking_user, "Broking_user", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Fields, "Fields", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Queries, "Queries", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Currency, "Currency", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Language, "Language", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Country, "Country", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Source, "Source", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Transaction_Type, "Transaction_Type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct_Client_Install, "PMProduct_Client_Install", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct_Lookup, "PMProduct_Lookup", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct_Update_History, "PMProduct_Update_History", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group, "PMUser_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser, "PMUser", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group_Activity, "PMUser_Group_Activity", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group_Group, "PMUser_Group_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Group_User, "PMUser_Group_User", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            ' PW271003 - CQ1359 - user_source table has changed to user_source_allowed
            ' in 1.9. Ordinarily we could have added the new one and left the old
            ' one 'cos the code ignores tables that do not exist. However, in 1.9
            ' PMUser_Source is now a View, instead of a table which messes things
            ' up when the import tries to Amend it.
            If Left(sVersionNumber, 3) <> "1.9" Then
                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Source, "PMUser_Source", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            Else
                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_Source, "PMUser_Source_Allowed", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            End If

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMSystem, "PMSystem", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNumber_Group, "PMNumber_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNumber, "PMNumber", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNumber_Range, "PMNumber_Range", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch, "PMNav_Batch", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Component, "PMNav_Component", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Key, "PMNav_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Map, "PMNav_Map", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMProc_Lock_Group, "PMProc_Lock_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Category, "PMWrk_Task_Category", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Group, "PMWrk_Task_Group", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_websites, "PMWrk_websites", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'addToArray g_aIeControl, Array(pbIeDbt_PMMessage, "PMMessage", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'addToArray g_aIeControl, Array(pbIeDbt_pmcaption, "pmcaption", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_unique_number, "unique_number", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'addToArray g_aIeControl, Array(pbIeDbt_PMLock, "PMLock", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Key, "PMNav_Batch_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Set, "PMNav_Batch_Set", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Key_Value, "PMNav_Batch_Key_Value", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Batch_Record, "PMNav_Batch_Record", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Get_Component_Key, "PMNav_Get_Component_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Get_Process_Key, "PMNav_Get_Process_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Get_Step_Key, "PMNav_Get_Step_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Process, "PMNav_Process", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Component_Key, "PMNav_Set_Component_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Map_Key, "PMNav_Set_Map_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Process_Key, "PMNav_Set_Process_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Set_Step_Key, "PMNav_Set_Step_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMNav_Step, "PMNav_Step", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task, "PMWrk_Task", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Group_Task, "PMWrk_Task_Group_Task", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Inst_Key, "PMWrk_Task_Inst_Key", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Inst_Log, "PMWrk_Task_Inst_Log", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMWrk_Task_Instance, "PMWrk_Task_Instance", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'these are broking migration only. These were specifically added to support GAJ and may be a subset

            'JB Jun 10 Fix for migrating treaty
            'addToArray g_aIeControl, Array(pbIeDbt_treaty, "treaty", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_treaty, "treaty", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'JB Aug 10 Fix for migrating tax_type
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_type, "tax_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'JB Jun 10 Fix for migrating tax_band
            'addToArray g_aIeControl, Array(pbIeDbt_tax_band, "tax_band", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_band, "tax_band", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'JB Jun 10 Fix for migrating tax_band_rate
            'addToArray g_aIeControl, Array(pbIeDbt_tax_band_rate, "tax_band_rate", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_band_rate, "tax_band_rate", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_rates, "tax_rates", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'addToArray g_aIeControl, Array(pbIeDbt_tax_type, "tax_type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_type_band, "tax_type_band", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'this is now included but not for export just for unique_number processing
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_accumulation, "accumulation", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, pbIeControl_Flags__uniqueNumber, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_area, "area", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_catastrophe_code, "catastrophe_code", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'addToArray g_aIeControl, Array(pbIeDbt_authority_level_type, "authority_level_type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'JB Dec 09
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_authority_level_type, "authority_level_type", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_license_type, "license_type", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'addToArray g_aIeControl, Array(pbIeDbt_PMWrk_User_Quick_Start, "PMWrk_User_Quick_Start", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            'do not ship pmproduct as this will destroy the DSN settings but need its definition for setting the caption values
            'addToArray g_aIeControl, Array(pbIeDbt_PMProduct, "PMProduct", pbIeOt_dbTable_fixed, 0, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString,0,0)

            'reports
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_report, "report", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_report_group, "report_group", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_report_group_contents, "report_group_contents", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_report_group_user_groups, "report_group_user_groups", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme + pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'pm lookups
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMProduct_Lookup, "PMProduct_Lookup", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, "where lookup_table_name like 'UDL%'", 0, 0})

            'document_type
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_DocumentType, "document_type", pbIeOt_dbTable_fixed, pbIeMode_Documents, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            ' PW020703 - CQ1359
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_RegSetting_16, "HKEY_LOCAL_MACHINE\SOFTWARE\Pure\PureInstallation\Server\GIS\<DMC>,LoadSaveDBMode", pbIeOt_RegSetting, pbIeMode_Registry, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'

            '-------------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'user spu_ICCS%
            'we need to add the user spus to the array.
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_user_spu, "user_spus", pbIeOt_UserSPU, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'tax group / tax type
            '    addToArray g_aIeControl, Array(pbIeDbt_tax_group, "Tax_Group", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_group_tax_band, "Tax_Group_Tax_Band", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_tax_type_basis, "Tax_Type_Basis", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'additional tables required by export process
            'user authority rules (only actually exported if import user authority tick box ticked)
            ' JB Jun 10 It is illogical to import PMUser_Authority_Level as user might not be defined on target so would stop this table from being imported
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_authority_level, "PMUser_Authority_Level", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            ' JB Dec 09 Changed the table order as per hierarchy, parent first and then child
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_rule_set, "Rule_Set", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_PMUser_authority_rule_set_link, "PMUser_Authority_Rule_Set_Link", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            '    addToArray g_aIeControl, Array(pbIeDbt_PMUser_authority_rule_set_link, "PMUser_Authority_Rule_Set_Link", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            '    addToArray g_aIeControl, Array(pbIeDbt_rule_set, "Rule_Set", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)

            'reinsurance
            '    addToArray g_aIeControl, Array(pbIeDbt_RI_Band, "RI_Band", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_RI_Model, "RI_Model", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_RI_Model_Line, "RI_Model_Line", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'others
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Sub_Commission_Band, "Sub_Commission_Band", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            ' BSJ Sep 09
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_Numbering_Scheme, "numbering_scheme", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            'crystal reports
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_user_crystal_report, "user_crystal", pbIeOt_UserCrystalReport, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'Richard Clarke November 2008 - PIE enhancements end
            '-------------------------------

            'JB Feb 10
            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            addToArray(g_aIeControl, New Object() {pbIeDbt_product_claims_workflow, "product_claims_workflow", pbIeOt_dbTable_child, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, pbIeDbt_product, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Risk_Type_Rating_Section_Type, "Risk_Type_Rating_Section_Type", pbIeOt_dbTable_fixed, pbIeMode_DataModel + pbIeMode_Screen + pbIeMode_Scheme, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Analysis_Code, "Analysis_Code", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})

            addToArray(g_aIeControl, New Object() {pbIeDbt_Earning_Pattern_Usage, "Earning_Pattern_Usage", pbIeOt_dbTable_fixed, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            ''''    'JB Aug 10
            ''''    addToArray g_aIeControl, Array(pbIeDbt_state, "State", pbIeOt_dbTable_fixed, pbIeMode_Migration, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0)
            'PM040123
            addToArray(g_aIeControl, New Object() {pbIeDbt_Report_Spu, "spu_report_%", pbIeOt_UserCrystalReportSPU, pbIeMode_DataModel, 0, 0, 0, False, 0, 0, 0, conEmptyString, conEmptyString, 0, 0})
            'open database and all files
            objFrmMainForm.StatusBar_TextWrite("Table definitions", 1)
            Dim sName As String
            Dim iRegSettingId As Short
            For iTableIndex = 0 To UBound(g_aIeControl)

                objFrmMainForm.ProgressBar1(1).Value = 100 / ((UBound(g_aIeControl) + 1) / (iTableIndex + 1))

                If g_bStopProcessing Then
                    Exit Function
                Else
                    If v_generateObjectConstants <> True Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iTableIndex)(pbIeControl_objectId). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If iTableIndex <> g_aIeControl(iTableIndex)(pbIeControl_objectId) Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            MsgBox("element=" & iTableIndex & " <> object id=" & g_aIeControl(iTableIndex)(pbIeControl_objectId), MsgBoxStyle.Critical, "Fatal Initialisation Error")
                            InitialiseControlArrays = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    Else
                        Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)
                            Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_child, pbIeOt_RuleFile, pbIeOt_GSDLookup, pbIeOt_GPLookup
                                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                sName = g_aIeControl(iTableIndex)(pbIeControl_objectName)
                            Case pbIeOt_RegSetting
                                iRegSettingId = iRegSettingId + 1
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
                            Case pbIeOt_UserSPU
                                sName = "User SPUs"
                            Case Else
                                sName = "XXXXXXXXXXXXXXXXXXXXXXX"
                        End Select
                        writeToDebugBox("Public Const pbIeDbt_" & sName & " = " & iTableIndex)

                    End If

                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = g_aIeControl(iTableIndex)(pbIeControl_objectName)
                    objFrmMainForm.StatusBar_TextWrite(CStr(g_aIeControl(iTableIndex)(pbIeControl_objectName)), 2)

                    'clear the temporary attribute array
                    Erase atemp
                    Erase aTemp2

                    'is this a table object
                    Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)
                        Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined, pbIeOt_dbTable_child, pbIeOt_RiskGroupsCodes

                            'get the column information for this table
                            'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            m_lReturn = GetColumnDetails(r_oDatabase:=r_oDatabase, sObjectName:=g_aIeControl(iTableIndex)(pbIeControl_objectName), v_iTableIndex:=iTableIndex, r_aIeControl:=g_aIeControl, r_aIeTableDefinitions:=g_aIeTableDefinitions, v_sColumnFilter:="")

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If g_lExportMode And pbIeMode_Migration Then
                                    'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    writeToStatusBox("No table defintion found for: " & g_aIeControl(iTableIndex)(pbIeControl_objectName))
                                End If
                                'probably a 1.8.6/1.9 difference so signal as don't do anything with this
                                'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                g_aIeControl(iTableIndex)(pbIeControl_objectType) = pbIeOt_Ignore
                                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                                addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), 0, ""})
                            End If

                        Case pbIeOt_FixedTableColumns
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"table_id", "int", 4})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"columns", "varchar", 500})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), UBound(atemp) + 1, atemp})

                        Case pbIeOt_RegSetting
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"key", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"name", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"value", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), UBound(atemp) + 1, atemp})
                            '--------------------------
                            'Richard Clarke November 2008 - PIE enhancements - OLD
                            'Case pbIeOt_RuleFile, pbIeOt_DerivedDefAndValRuleFile, _
                            ''   pbIeOt_DocumentTemplate, pbIeOt_DerivedRatingRuleFile, pbIeOt_GisList
                            'Richard Clarke November 2008 - PIE enhancements - NEW
                        Case pbIeOt_RuleFile, pbIeOt_DerivedDefAndValRuleFile, pbIeOt_DocumentTemplate, pbIeOt_DerivedRatingRuleFile, pbIeOt_GisList, pbIeOt_UserSPU, pbIeOt_UserCrystalReport, pbIeOt_UserCrystalReportSPU
                            'Richard Clarke November 2008 - PIE enhancements - END
                            '--------------------------
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"filename", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"length", "int", 4})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"contents", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), UBound(atemp) + 1, atemp})

                        Case pbIeOt_UserDefinedListHeader
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"list_name", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"table_definition", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), UBound(atemp) + 1, atemp})

                        Case pbIeOt_UserDefinedList, pbIeOt_GSDLookup, pbIeOt_GPLookup
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"UDL_Area_id", "smallint", 2})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"caption_id", "int", 4})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"code", "char", 10})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"Description", "varchar", 255})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"is_deleted", "tinyint", 1})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"effective_date", "datetime", 8})
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), UBound(atemp) + 1, atemp})

                        Case pbIeOt_Header
                            ' Any extra elements required here to be appended rather than inserted.
                            ' The import is assuming a certain order to these elements and references these
                            ' by the numbers shown below.
                            '0 Export Mode (Schema, Data Model or screen)
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"mode", "int", 4})
                            '1 Data Model Id
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"LinesWritten", "int", 4})
                            '2 Lines written
                            '----------------------------------------
                            'Richard Clarke November 2008 - PIE enhancements
                            'datamodelid is now string too
                            'addToArray atemp, Array("DataModelId", "int", 4)
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"DataModelId", "varchar", 255})
                            'Richard Clarke November 2008 - PIE enhancements
                            '---------------------------------------
                            '3 Data Model Code
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"DataModelCode", "varchar", 255})
                            '4 System (Underwriting or Broking)
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"system", "int", 4})
                            '5 Export Tool Version Number
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"VersionNumber", "varchar", 255})
                            '6 Export Comments
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"comment", "varchar", 255})
                            '7 Database Versions
                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(atemp, New Object() {"DBVersions", "varchar", 255})

                            'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            addToArray(g_aIeTableDefinitions, New Object() {g_aIeControl(iTableIndex)(pbIeControl_objectName), UBound(atemp) + 1, atemp})
                    End Select
                End If
            Next

            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = conEmptyString
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = conEmptyString
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString

            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 0)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 1)
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

            '-----------------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'need to account for multiple data models being selected by user in tree control
            'clear the arrays down as this could be the second time we called initialisecontrolarrays
            ReDim r_lDataModelId(0)
            ReDim r_sDataModelCode(0)

            '-----------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'have to filter this by import / export
            'EXPORT means read from the form
            'IMPORT means read from the file
            Dim iDataModelindex As Short
            If g_iImportExport = 1 Then

                Dim iNodeCount As Integer = objFrmMainForm.tvDataModel.Nodes(0).Nodes.Count - 1

                For iDataModelindex = 0 To iNodeCount
                    'if this node is selected then add it to the r_ldatamodelid
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    If objFrmMainForm.tvDataModel.Nodes(0).Nodes.Item(iDataModelindex).Checked Then
                        ReDim Preserve r_lDataModelId(CInt(UBound(r_lDataModelId) + 1))
                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        r_lDataModelId(CInt(UBound(r_lDataModelId) - 1)) = CInt(Mid(objFrmMainForm.tvDataModel.Nodes(0).Nodes.Item(iDataModelindex).Name, 6)) 'objFrmMainForm.tvDataModel.Nodes(iDataModelIndex).Key
                    End If
                Next iDataModelindex

                If UBound(r_lDataModelId) >= 0 Then
                    'loop round data model id array and get the code

                    For iDataModelindex = 0 To UBound(r_lDataModelId) - 1
                        m_lReturn = r_oDatabase.SQLSelect(sSQL:="select code from gis_data_model where gis_data_model_id = " & r_lDataModelId(iDataModelindex), sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            InitialiseControlArrays = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                        'add it to our data model code string array
                        If IsArray(r_vResults) Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            r_sDataModelCode(iDataModelindex) = Trim(r_vResults(0, 0))
                            ReDim Preserve r_sDataModelCode(UBound(r_sDataModelCode) + 1)
                        Else
                            InitialiseControlArrays = gPMConstants.PMEReturnCode.PMError
                            writeToStatusBox("Could not find data model with id=" & r_lDataModelId(iDataModelindex))
                            InitialiseControlArrays = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    Next iDataModelindex
                    'Else 'Richard Clarke November 2008 PIE enhancements - OLD: This Else did nothing so i have commented it out
                End If

                'get the sirius user id
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_vResults = Nothing
                m_lReturn = r_oDatabase.SQLSelect(sSQL:="select user_id from pmuser where username='sirius'", sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

                If IsArray(r_vResults) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_lSiriusUserId = CInt(Trim(r_vResults(0, 0)))
                Else
                    InitialiseControlArrays = gPMConstants.PMEReturnCode.PMError
                    writeToStatusBox("Could not find sirius user in database")
                    InitialiseControlArrays = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            Else
                'import - get data model from header file?
                Debug.Print("IMPORT")
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ".InitialiseControlArrays")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ".InitialiseControlArrays")

            InitialiseControlArrays = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseControlArrays Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="InitialiseControlArrays", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function GetColumnDetails(ByRef r_oDatabase As dPMDAO.Database, ByVal sObjectName As String, ByVal v_iTableIndex As Short, ByRef r_aIeControl() As Object, ByRef r_aIeTableDefinitions() As Object, ByVal v_sColumnFilter As String) As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & ".GetColumnDetails")

        Try

            Dim vResults(,) As Object
            Dim atemp() As Object
            Dim iCaptionAndDescriptionDetected As Short
            Dim bListStarted As Boolean
            Dim bIgnorePKData As Boolean
            Dim iColumnIndex As Short
            Dim iCaptionColumn As Short
            Dim sExportedColumns As String
            Dim aColumns As Object

            GetColumnDetails = gPMConstants.PMEReturnCode.PMTrue

            'ignore the migration tables
            'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(v_iTableIndex)(pbIeControl_operationMode). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If r_aIeControl(v_iTableIndex)(pbIeControl_operationMode) = pbIeMode_Migration Then
                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                addToArray(r_aIeTableDefinitions, New Object() {sObjectName, 0, "", ""})
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If sObjectName = "claim_party_type" Then
                Debug.Print("blah")
            End If

            With r_oDatabase
                m_lReturn = .SQLSelect(sSQL:="select name from sysobjects where name ='" & sObjectName & "' and type='u'", sSQLName:="Pre GetDMTableColumnList check", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, bKeepNulls:=True, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                If IsArray(vResults) And m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    .Parameters.Clear()
                    m_lReturn = .Parameters.Add(sName:="tablename", vValue:=sObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = .SQLSelect(sSQL:=ACMSGetDMTableColumnsSQL, sSQLName:=ACMSGetDMTableColumnsName, bStoredProcedure:=ACMSGetDMTableColumnsStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                    End If
                Else
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetColumnDetails = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

            'clear "special columns" flags
            iCaptionAndDescriptionDetected = 0 'reset caption
            bListStarted = False

            'walk the columns

            'before looking at columns check if we need to use pre-defined PK definition
            bIgnorePKData = (Len(r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns)) <> 0)


            Dim iColumnFilterLoop As Short
            Dim bFoundColumnFilter As Boolean
            If IsArray(vResults) Then
                If v_sColumnFilter <> "" Then

                    'UPGRADE_WARNING: Couldn't resolve default property of object aColumns. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    aColumns = Split(v_sColumnFilter, ",")

                    'PM9403 PB import/export tool displays unnecessary warnings
                    'don't bother reporting problems check tables we are not importing
                    '            If r_aIeControl(v_iTableIndex)(pbIeControl_operationMode) And g_lExportMode Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount) > UBound(aColumns) + 1 Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        writeToStatusBox("Warning: Column count differs for table " & sObjectName & ". Source=" & UBound(aColumns) + 1 & " Destination=" & r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount))
                    End If
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount) < UBound(aColumns) + 1 Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        writeToStatusBox("Error: Column count differs for table " & sObjectName & ". Source=" & UBound(aColumns) & " Destination=" & r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount))
                        '                GetColumnDetails = PMFalse
                        '                Exit Function
                    End If
                    '            Else
                    '            End If


                    iColumnIndex = 0
                    For iColumnFilterLoop = 0 To UBound(aColumns)
                        bFoundColumnFilter = False
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(sp_msHelpColumns_columnName, iColumnIndex). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object aColumns(iColumnFilterLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If aColumns(iColumnFilterLoop) = vResults(sp_msHelpColumns_columnName, iColumnIndex) Then
                            bFoundColumnFilter = True
                        End If


                        If bFoundColumnFilter = False Then
                            For iColumnIndex = 0 To UBound(vResults, 2)
                                If bFoundColumnFilter = False Then
                                    'UPGRADE_WARNING: Couldn't resolve default property of object aColumns(iColumnFilterLoop). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    If LCase(vResults(pbIeTableDefinitions_columnName, iColumnIndex)) = aColumns(iColumnFilterLoop) Then
                                        bFoundColumnFilter = True
                                        ProcessColumnDefinition(r_oDatabase, r_aIeControl, vResults, sObjectName, v_iTableIndex, iColumnIndex, iCaptionAndDescriptionDetected, iCaptionColumn, bIgnorePKData, bListStarted)
                                    End If
                                    'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(iColumnFilterLoop) = New Object() {vResults(sp_msHelpColumns_columnName, iColumnIndex), vResults(sp_msHelpColumns_columnType, iColumnIndex), vResults(sp_msHelpColumns_columnSize, iColumnIndex), vResults(sp_msHelpColumns_columnIdentity, iColumnIndex)}
                                Else
                                    Exit For
                                End If
                            Next
                            If bFoundColumnFilter = True And iColumnIndex > UBound(vResults, 2) Then
                                'stop error if found field was last one
                                iColumnIndex = 0
                            End If

                        End If
                        If bFoundColumnFilter = False Then
                            'imported column not found in target
                            'don't bother reporting problems on tables we are not importing
                            If r_aIeControl(v_iTableIndex)(pbIeControl_operationMode) And g_lExportMode Then
                                'UPGRADE_WARNING: Couldn't resolve default property of object aColumns(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                writeToStatusBox("Error: Could not find column " & aColumns(iColumnFilterLoop) & " in table " & sObjectName & " on the target database")
                                GetColumnDetails = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                            iColumnIndex = 0
                        End If
                    Next
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_exportedColumns) = v_sColumnFilter
                    'resize the array
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount) = UBound(aColumns) + 1
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object aColumns. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    aColumns = r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    ReDim Preserve aColumns(r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnCount) - 1)
                    'UPGRADE_WARNING: Couldn't resolve default property of object aColumns. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeTableDefinitions()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray) = aColumns
                    'redim preserve r_aIeTableDefinitions(v_iTableIndex)(pbIeTableDefinitions_columnArray)(UBound(aColumns))

                    Exit Function
                End If
                For iColumnIndex = 0 To UBound(vResults, 2)
                    ProcessColumnDefinition(r_oDatabase, r_aIeControl, vResults, sObjectName, v_iTableIndex, iColumnIndex, iCaptionAndDescriptionDetected, iCaptionColumn, bIgnorePKData, bListStarted)

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(v_iTableIndex)(pbIeTableDefinitions_columnType). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_aIeControl(v_iTableIndex)(pbIeTableDefinitions_columnType) <> gTIMESTAMP And v_sColumnFilter = "" Then
                        If sExportedColumns = "" Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sExportedColumns = LCase(vResults(pbIeTableDefinitions_columnName, iColumnIndex))
                        Else
                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            sExportedColumns = sExportedColumns & "," & LCase(vResults(pbIeTableDefinitions_columnName, iColumnIndex))
                        End If
                    Else
                    End If

                    'add column name, type and size to array (+ add is identity flag for each column)
                    'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    addToArray(atemp, New Object() {vResults(sp_msHelpColumns_columnName, iColumnIndex), vResults(sp_msHelpColumns_columnType, iColumnIndex), vResults(sp_msHelpColumns_columnSize, iColumnIndex), vResults(sp_msHelpColumns_columnIdentity, iColumnIndex)})
                Next

                Select Case iCaptionAndDescriptionDetected
                    Case 1 ' description
                    Case 2 ' caption id without description
                        If sObjectName <> "pmcaption" Then
                            MsgBox("caption id and description mis-match")
                        End If
                    Case 3 'caption id and description
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        vResults(pbIeTableDefinitions_columnType, iCaptionColumn) = "captionText"
                    Case Else
                End Select

                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                addToArray(r_aIeTableDefinitions, New Object() {sObjectName, UBound(vResults, 2) + 1, atemp, sExportedColumns})

            Else
                If g_iImportExport = 1 Then
                    'Stick a message out to identify the table not found in source database
                    writeToStatusBox(("Table " & sObjectName & " has not been found in database"))
                End If

                'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                addToArray(r_aIeTableDefinitions, New Object() {sObjectName, 0, atemp})
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ".GetColumnDetails")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ".GetColumnDetails")

            GetColumnDetails = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetColumnDetails Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="GetColumnDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function


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
    Private Function ProcessColumnDefinition(ByRef r_oDatabase As dPMDAO.Database, ByRef r_aIeControl() As Object, ByRef r_vResults As Object, ByVal v_sObjectName As String, ByVal v_iTableIndex As Short, ByVal v_iColumnIndex As Short, ByRef r_iCaptionAndDescriptionDetected As Short, ByRef r_iCaptionColumn As Short, ByVal v_bIgnorePKData As Boolean, ByRef r_bListStarted As Boolean) As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".ProcessColumnDefinition")


        Dim vResults2(,) As Object

        ProcessColumnDefinition = gPMConstants.PMEReturnCode.PMTrue
        'search for special columns we have to do something with
        'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Select Case LCase(r_vResults(pbIeTableDefinitions_columnName, v_iColumnIndex))
            Case "gis_data_model_id"
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeControl(v_iTableIndex)(pbIeControl_DataModelIdColumn) = v_iColumnIndex + 1
                Debug.Print("DataModelId " & v_sObjectName)
            Case "caption_id"
                If v_sObjectName <> "pmcaption" Then 'no caption processing on pmcaption
                    r_iCaptionAndDescriptionDetected = r_iCaptionAndDescriptionDetected + 2
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(v_iTableIndex)(pbIeControl_Flags) = r_aIeControl(v_iTableIndex)(pbIeControl_Flags) Or pbIeControl_Flags__IsCaption
                End If
            Case "description"
                r_iCaptionAndDescriptionDetected = r_iCaptionAndDescriptionDetected + 1
                r_iCaptionColumn = v_iColumnIndex
                Debug.Print("Caption " & v_sObjectName)
            Case "code"
                'if this column is called 'code' signal as "code used for search" unless flagged as ignore
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn) = pbIgnoreFlag Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn) = 0
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(v_iTableIndex)(pbIeControl_CodeColumn) = v_iColumnIndex + 1
                    Debug.Print("Code " & v_sObjectName)
                End If
            Case "filename", "file_name"
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename) <> 0 And r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename) <> v_iColumnIndex + 1 Then
                    MsgBox("whoops more than 1 filename")
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(v_iTableIndex)(pbIeControl_HasFilename) = v_iColumnIndex + 1
                End If

                Debug.Print("filename " & v_sObjectName)
            Case "parent_id", "parentid"
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeControl(v_iTableIndex)(pbIeControl_ParentIdColumn) = v_iColumnIndex + 1

                Debug.Print("filename " & v_sObjectName)

            Case "created_by_id", "modified_by_id"
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeControl(v_iTableIndex)(pbIeControl_Flags) = r_aIeControl(v_iTableIndex)(pbIeControl_Flags) Or pbIeControl_Flags__CreatedByOrModifiedBy

            Case "gis_insurer_id"
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeControl(v_iTableIndex)(pbIeControl_GisInsurerIdColumn) = v_iColumnIndex + 1
        End Select

        ' Control array may already have primary key details contained in the list so ignore
        ' the next section if this is the case.

        If Not v_bIgnorePKData Then
            If r_vResults(sp_msHelpColumns_columnFlags, v_iColumnIndex) And conPrimaryKey Then
                If r_bListStarted Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns) = r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns) & "," & v_iColumnIndex + 1
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_aIeControl(v_iTableIndex)(pbIeControl_PrimaryKeyColumns) = v_iColumnIndex + 1
                    r_bListStarted = True
                End If
            End If
        End If

        'check the first column to see if it is an identity column
        ' BSJ Sep 2009 - Do the check if table name is numbering_scheme as its identity is further along (column 14)!
        If (v_iColumnIndex = 0) Or (LCase(CStr(v_sObjectName)) = "numbering_scheme") Or (LCase(CStr(v_sObjectName)) = "accumulation") Or (LCase(CStr(v_sObjectName)) = "tax_band_rate") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = r_oDatabase.SQLSelect(sSQL:="SELECT COLUMNPROPERTY( OBJECT_ID('" & v_sObjectName & "'),'" & r_vResults(pbIeTableDefinitions_columnName, v_iColumnIndex) & "','IsIdentity')", sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults2)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ProcessColumnDefinition = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object vResults2(0, 0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If vResults2(0, 0) = 1 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_vResults(pbIeTableDefinitions_columnType, v_iColumnIndex) = "identity"
                'UPGRADE_WARNING: Couldn't resolve default property of object r_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_aIeControl(v_iTableIndex)(pbIeControl_IsIdentity) = True
                Debug.Print("Identity " & v_sObjectName)
            End If
        End If


        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".ProcessColumnDefinition")

        Exit Function

    End Function
End Module