Option Strict Off
Option Explicit On
Module bGISSchemeBusinessConst
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    ' ***************************************************************** '
    ' Module Name: bGISSchemeBusinessConst
    '
    ' Date:  10/06/1999
    '
    ' Description: Constants needed for using business object.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "bGISSchemeBusinessConst"

    ' Constants to define  parameter names
    Public Const GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID As String = "gis_scheme_group_id"
    Public Const GISSB_PARAM_NAME_CODE As String = "code"
    Public Const GISSB_PARAM_NAME_CAPTION_ID As String = "caption_id"
    Public Const GISSB_PARAM_NAME_DESCRIPTION As String = "description"
    Public Const GISSB_PARAM_NAME_IS_DELETED As String = "is_deleted"
    Public Const GISSB_PARAM_NAME_EFFECTIVE_DATE As String = "effective_date"
    Public Const GISSB_PARAM_NAME_GIS_BUSINESS_TYPE As String = "gis_business_type_id"
    Public Const GISSB_PARAM_NAME_SOURCE_ID As String = "source_id" 'sj 26/7/2000
    Public Const GISSB_PARAM_NAME_DICT_VER As String = "dict_ver" ' TB 15/5/01
    Public Const GISSB_PARAM_NAME_CLASS_OF_BUSINESS As String = "class_of_business" ' tb 14/05/2001
    'sj 24/05/2001 - start
    Public Const GISSB_PARAM_NAME_COUNTRY_ID As String = "country_id"
    'sj 24/05/2001 - end

    Public Const GISSB_PARAM_NAME_GIS_SCHEME_ID As String = "gis_scheme_id"

    Public Const GISSB_PARAM_NAME_GIS_QUOTE_ENGINE As String = "gis_quote_engine_id"

    Public Const GISSB_PARAM_NAME_INSURER_ID As String = "gis_insurer_id"
    Public Const GISSB_PARAM_NAME_SCHEME_NO As String = "scheme_no"
    Public Const GISSB_PARAM_NAME_SCHEME_VER As String = "scheme_ver"
    Public Const GISSB_PARAM_NAME_SCHEME_STATUS As String = "scheme_status"
    Public Const GISSB_PARAM_NAME_START_DATE As String = "start_date"
    Public Const GISSB_PARAM_NAME_SCHEME_DESC As String = "scheme_desc"
    Public Const GISSB_PARAM_NAME_PRIORITY As String = "priority"
    Public Const GISSB_PARAM_NAME_AGENCY_CODE As String = "agency_code"
    Public Const GISSB_PARAM_NAME_PRODUCT_CODE As String = "product_code"
    Public Const GISSB_PARAM_NAME_ACTIVATION_LEVEL As String = "activation_level"
    Public Const GISSB_PARAM_NAME_PRINTING_PRIVILEGES As String = "printing_privileges"
    Public Const GISSB_PARAM_NAME_BROKER_GROUP As String = "broker_group"
    Public Const GISSB_PARAM_NAME_COMMISION_PERC As String = "commision_perc"
    Public Const GISSB_PARAM_NAME_QUOTE_DAY_NUM As String = "quote_day_num"
    Public Const GISSB_PARAM_NAME_SELECT_DAY_NUM As String = "selection_day_num"
    Public Const GISSB_PARAM_NAME_INVITE_DAY_NUM As String = "invite_day_num"
    Public Const GISSB_PARAM_NAME_CONFIRM_DAY_NUM As String = "confirm_day_num"
    Public Const GISSB_PARAM_NAME_LAPSE_DAY_NUM As String = "lapse_day_num"
    Public Const GISSB_PARAM_NAME_MAX_CHANGE_NUM As String = "max_change_num"
    Public Const GISSB_PARAM_NAME_MIN_CHANGE_NUM As String = "min_change_num"
    Public Const GISSB_PARAM_NAME_EXPIRY_DATE As String = "expiry_date"
    Public Const GISSB_PARAM_NAME_QM_INSURER_REF As String = "qm_insurer_ref"
    Public Const GISSB_PARAM_NAME_SCHEME_TYPE_FLAGS As String = "scheme_type_flags"
    Public Const GISSB_PARAM_NAME_FILENAME As String = "filename"
    Public Const GISSB_PARAM_NAME_EDI_MAIL_BOX As String = "edi_mail_box"
    Public Const GISSB_PARAM_NAME_REFER_EMAIL_ADDRESS As String = "refer_email_address"
    Public Const GISSB_PARAM_NAME_REFER_FAX_NUMBER As String = "refer_fax_number"
    Public Const GISSB_PARAM_NAME_SCHEME_TYPE As String = "scheme_type"
    Public Const GISSB_PARAM_NAME_SCHEME_VARIANT As String = "scheme_variant"
    'TF081001
    Public Const GISSB_PARAM_NAME_PRE_SELECT_DAY_NUM As String = "pre_selection_day_num"
    Public Const GISSB_PARAM_NAME_REMINDER_DAY_NUM As String = "reminder_day_num"

    Public Const GISSB_PARAM_NAME_GIS_POLICY_LINK_ID As String = "gis_policy_link_id"

    'sj 22/07/99 - start
    Public Const GISSB_PARAM_NAME_GIS_DATA_MODEL As String = "gis_data_model_id"
    Public Const GISSB_PARAM_NAME_GIS_QEM As String = "gis_qem_id"
    'sj 22/7/99 - end

    'sj 13/7/2000 - start
    Public Const GISSB_PARAM_NAME_LINKAGE_MAP_MAX As String = "linkage_map_max"
    Public Const GISSB_PARAM_NAME_LINKAGE_MAP_MIN As String = "linkage_map_min"
    'sj 13/7/2000 - end

    'sj 14/03/2001 - start
    'Scheme data fields
    Public Const GISSB_PARAM_NAME_ADMIN_CHARGE As String = "admin_charge"
    Public Const GISSB_PARAM_NAME_min_perm_charge As String = "min_perm_charge"
    Public Const GISSB_PARAM_NAME_min_reinst_premium As String = "min_reinst_premium"
    Public Const GISSB_PARAM_NAME_min_temp_charge As String = "min_temp_charge"
    Public Const GISSB_PARAM_NAME_mta_can_adm_charge As String = "mta_can_adm_charge"
    Public Const GISSB_PARAM_NAME_mta_can_min_value As String = "mta_can_min_value"
    Public Const GISSB_PARAM_NAME_mta_can_round_type As String = "mta_can_round_type"
    Public Const GISSB_PARAM_NAME_mta_cpd_adm_charge As String = "mta_cpd_adm_charge"
    Public Const GISSB_PARAM_NAME_mta_cpd_min_value As String = "mta_cpd_min_value"
    Public Const GISSB_PARAM_NAME_mta_cpd_round_type As String = "mta_cpd_round_type"
    Public Const GISSB_PARAM_NAME_mta_perm_adm_charge As String = "mta_perm_adm_charge"
    Public Const GISSB_PARAM_NAME_mta_perm_min_value As String = "mta_perm_min_value"
    Public Const GISSB_PARAM_NAME_mta_perm_round_type As String = "mta_perm_round_type"
    Public Const GISSB_PARAM_NAME_mta_rei_adm_charge As String = "mta_rei_adm_charge"
    Public Const GISSB_PARAM_NAME_mta_rei_min_value As String = "mta_rei_min_value"
    Public Const GISSB_PARAM_NAME_mta_rei_round_type As String = "mta_rei_round_type"
    Public Const GISSB_PARAM_NAME_mta_temp_adm_charge As String = "mta_temp_adm_charge"
    Public Const GISSB_PARAM_NAME_mta_temp_min_value As String = "mta_temp_min_value"
    Public Const GISSB_PARAM_NAME_mta_temp_round_type As String = "mta_temp_round_type"
    Public Const GISSB_PARAM_NAME_override_scr As String = "override_scr"
    Public Const GISSB_PARAM_NAME_reinst_days_with_no_rp As String = "reinst_days_with_no_rp"
    'sj 14/03/2001 - end

    Public Const GISLowDate As Date = #1/1/1900# ' CL150200

    ' Enum Constants to define type of property counts that can be extracted.
    Public Enum GISSB_GET_PROPERTY_COUNT_TYPE
        GPCT_ALL_SCHEMES_OF_TYPE = 1
        GPCT_SCHEME_GROUP_OF_TYPE = 2
        GPCT_SCHEME = 3
    End Enum

    ' Enum constants for types of scheme lists.
    Public Enum GISSB_GET_SCHEME_LISTS
        GSL_FULL_ACTIVE_OF_TYPE = 1
        GSL_SINGLE_SCHEME = 2
        GSL_ID_NAME_OF_TYPE = 3
        GSL_ID_NAME_OF_TYPE_FOR_INSURER = 4
        GSL_AGENCY = 5
        GSL_ID_NAME_OF_TYPE_FOR_GROUP = 6
        GSL_ALL_LEGACY_SCHEMES = 7
        GSL_ID_NAME_WITH_INSURER = 8
        GSL_FULL_ACTIVE_OF_TYPE_WITH_INSURER_NAME = 9
        GSL_SINGLE_SCHEME_FULL_COL_LIST = 10
        GSL_FULL_LINK_BRANCH_SCHEME = 11
        GSL_LEGACY_SCHEMES_BY_CLASS = 12
        GSL_FULL_LINK_BRANCH_CLASS = 13
        GSL_FULL_ACTIVE_OF_CLASS_WITH_INSURER_NAME = 14
    End Enum

    ' Enum constant for type of scheme group list
    Public Enum GISSB_GET_SCHEME_GROUP_LISTS
        GSGL_ALL = 1
        GSGL_BUSINESS = 2
        GSGL_SINGLE = 3
    End Enum

    Public Enum GISSB_GET_LIST_SCHEME_GROUP_MEMBER
        GLSGM_BY_GROUP_ID = 1
        GLSGM_ALL = 2
        GLSGM_BY_GROUP_CODE = 3
    End Enum

    ' Enum constant for type of scheme data list
    Public Enum GISSB_GET_SCHEME_DATA_LISTS
        GSDL_BY_SCHEME = 1
    End Enum
End Module