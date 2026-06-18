Option Strict Off
Option Explicit On
Module PremiumConstants

    ' ***************************************************************** '
    '
    '  Constants used by the enhanced Premium Sections UI
    '
    ' ***************************************************************** '


    Public Enum eCustomDataType
        cdtCommissionTotals = 1
        cdtSectionApplied = 2
        cdtPremiumAmount = 3
        cdtPremiumTax = 4
        cdtCoinsurers = 5
    End Enum

    Public Enum eDataChange
        dcPremiumApplied = 1
        dcPremiumAmount = 2
        dcCommissionAmount = 3
        dcCoinsurerCommission = 4
        dcAddOnCommission = 5
        dcAddOnPremium = 6
        dcThirdPartyCommission = 7
    End Enum

    Public Enum eGridSortOrder
        gsoUnknown = 0
        gsoAscending = 1
        gsoDescending = 2
    End Enum

    Public Enum eCellState
        csEnabled = 1
        csDisabled = 2
    End Enum

    Public Enum eCellType
        ctText = 0
        ctNumericAmount = 1
        ctNumericPercent = 2
        ctCheckbox = 3
        ctDropDown = 4
        ctButton = 5
    End Enum

    Public Enum ePersistState
        psUnknown = 0
        psInsert = 1
        psUpdate = 2
        psDelete = 3
    End Enum

    Public Enum eIntegrityCheckType
        ictAddition = 0
        ictSubtraction = 1
        ictMultiplication = 2
        ictDivision = 3
    End Enum

    Public Enum eCellValidation
        cvNotBlank = 1
        cvNumeric = 2
        cvPositive = 4
        cvNegative = 8
        cvZeroOrPositive = 16
        cvZeroOrNegative = 32
        cvTrimValue = 64
        cvLessThan = 128
        cvLessThanOrEqual = 256
        cvMoreThan = 512
        cvMoreThanOrEqual = 1024
    End Enum


    'Private AC_COLOUR_DISABLED As Integer = ColorTranslator.ToOle(SystemColors.Control)

    Public Const AC_XARRAY_ROW_DIM As Integer = 1
    Public Const AC_XARRAY_COL_DIM As Integer = 2

    Public Const AC_GRID_CHECKBOX_CHECKED As String = "-1"
    Public Const AC_GRID_CHECKBOX_UNCHECKED As String = "0"

    Public Const AC_ERROR_GRID_DATA_ACCESS As Integer = 16389

    'Array Constants
    'ST = Section Template
    'IST = Insurer Section Template
    'PS = Policy Section
    'PCS = Policy CoInsurer Section
    'PB = Premium Breakdown
    'CM = Commission
    'TP = Third Party
    'AO = Add On
    'IA = Invoice Account
    'COSD = CoInsurer Section Detail
    'COCM = CoInsurer Commission
    'CO = CoInsurer
    'CB = Commission Breakdown
    'SD = Section Breakdown

    Public Const AC_ARR_PS_LOWER As Integer = 0
    Public Const AC_ARR_PS_ID As Integer = 0
    Public Const AC_ARR_PS_INSURANCE_FILE_CNT As Integer = 1
    Public Const AC_ARR_PS_SECTION_ID As Integer = 2
    Public Const AC_ARR_PS_SECTION_NAME As Integer = 3
    Public Const AC_ARR_PS_IS_LEVY_SECTION As Integer = 4
    Public Const AC_ARR_PS_APPLIED As Integer = 5
    Public Const AC_ARR_PS_PREMIUM_EXC_TAX As Integer = 6
    Public Const AC_ARR_PS_TAX_GROUP_ID As Integer = 7
    Public Const AC_ARR_PS_TAX_GROUP As Integer = 8
    Public Const AC_ARR_PS_TAX_PERCENT As Integer = 9
    Public Const AC_ARR_PS_PREMIUM_TAX As Integer = 10
    Public Const AC_ARR_PS_PREMIUM_INC_TAX As Integer = 11
    Public Const AC_ARR_PS_COMMISSION_PERCENT As Integer = 12
    Public Const AC_ARR_PS_COMMISSION_CHARGE As Integer = 13
    Public Const AC_ARR_PS_COMMISSION_AMOUNT As Integer = 14
    Public Const AC_ARR_PS_COMMISSION_TAX_GROUP_ID As Integer = 15
    Public Const AC_ARR_PS_COMMISSION_TAX_GROUP As Integer = 16
    Public Const AC_ARR_PS_COMMISSION_TAX_PERCENT As Integer = 17
    Public Const AC_ARR_PS_COMMISSION_TAX As Integer = 18
    Public Const AC_ARR_PS_COMMISSION_INC_TAX As Integer = 19
    Public Const AC_ARR_PS_PERSIST_STATE As Integer = 20
    Public Const AC_ARR_PS_CALCULATION_BASIS As Integer = 21
    Public Const AC_ARR_PS_COMMISSION_PERCENT_DFLT As Integer = 22
    Public Const AC_ARR_PS_COMMISSION_CHARGE_DFLT As Integer = 23
    Public Const AC_ARR_PS_COMMISSION_AMOUNT_DFLT As Integer = 24
    Public Const AC_ARR_PS_OVERRIDE As Integer = 25
    Public Const AC_ARR_PS_TAX_GROUP_EDITABLE As Integer = 26
    Public Const AC_ARR_PS_UPPER As Integer = 26

    Public Const AC_ARR_PS_KEY As Integer = AC_ARR_PS_ID


    Public Const AC_ARR_PCS_LOWER As Integer = 0
    Public Const AC_ARR_PCS_ID As Integer = 0
    Public Const AC_ARR_PCS_INSURANCE_FILE_CNT As Integer = 1
    Public Const AC_ARR_PCS_PARTY_CNT As Integer = 2
    Public Const AC_ARR_PCS_PARTY_NAME As Integer = 3
    Public Const AC_ARR_PCS_SECTION_ID As Integer = 4
    Public Const AC_ARR_PCS_SECTION_NAME As Integer = 5
    Public Const AC_ARR_PCS_IS_LEVY_SECTION As Integer = 6
    Public Const AC_ARR_PCS_SECTION_APPLIED As Integer = 7
    Public Const AC_ARR_PCS_APPLIED As Integer = 8
    Public Const AC_ARR_PCS_SHARE_PERCENT As Integer = 9
    Public Const AC_ARR_PCS_PREMIUM_INC_TAX As Integer = 10
    Public Const AC_ARR_PCS_TAX_GROUP_ID As Integer = 11
    Public Const AC_ARR_PCS_TAX_GROUP As Integer = 12
    Public Const AC_ARR_PCS_TAX_PERCENT As Integer = 13
    Public Const AC_ARR_PCS_PREMIUM_TAX As Integer = 14
    Public Const AC_ARR_PCS_PREMIUM_EXC_TAX As Integer = 15
    Public Const AC_ARR_PCS_COMMISSION_PERCENT As Integer = 16
    Public Const AC_ARR_PCS_COMMISSION_CHARGE As Integer = 17
    Public Const AC_ARR_PCS_COMMISSION_AMOUNT As Integer = 18
    Public Const AC_ARR_PCS_COMMISSION_TAX_GROUP_ID As Integer = 19
    Public Const AC_ARR_PCS_COMMISSION_TAX_GROUP As Integer = 20
    Public Const AC_ARR_PCS_COMMISSION_TAX_PERCENT As Integer = 21
    Public Const AC_ARR_PCS_COMMISSION_TAX As Integer = 22
    Public Const AC_ARR_PCS_COMMISSION_INC_TAX As Integer = 23
    Public Const AC_ARR_PCS_PERSIST_STATE As Integer = 24
    Public Const AC_ARR_PCS_CALCULATION_BASIS As Integer = 25
    Public Const AC_ARR_PCS_COMMISSION_PERCENT_DFLT As Integer = 26
    Public Const AC_ARR_PCS_COMMISSION_CHARGE_DFLT As Integer = 27
    Public Const AC_ARR_PCS_COMMISSION_MINIMUM_DFLT As Integer = 28
    Public Const AC_ARR_PCS_OVERRIDE As Integer = 29
    Public Const AC_ARR_PCS_TAX_GROUP_EDITABLE As Integer = 30
    Public Const AC_ARR_PCS_WRITTEN_PERCENT As Integer = 31 ''PYV

    Public Const AC_ARR_PCS_UPPER As Integer = 31

    Public Const AC_ARR_PCS_KEY As Integer = AC_ARR_PCS_ID


    Public Const AC_ARR_PB_LOWER As Integer = 0
    Public Const AC_ARR_PB_SECTION_ID As Integer = AC_ARR_PS_SECTION_ID
    Public Const AC_ARR_PB_SECTION_NAME As Integer = AC_ARR_PS_SECTION_NAME
    Public Const AC_ARR_PB_APPLIED As Integer = AC_ARR_PS_APPLIED
    Public Const AC_ARR_PB_PREMIUM_INC_TAX As Integer = AC_ARR_PS_PREMIUM_INC_TAX
    Public Const AC_ARR_PB_TAX_GROUP As Integer = AC_ARR_PS_TAX_GROUP
    Public Const AC_ARR_PB_TAX_GROUP_ID As Integer = AC_ARR_PS_TAX_GROUP_ID
    Public Const AC_ARR_PB_TAX_PERCENT As Integer = AC_ARR_PS_TAX_PERCENT
    Public Const AC_ARR_PB_PREMIUM_TAX As Integer = AC_ARR_PS_PREMIUM_TAX
    Public Const AC_ARR_PB_PREMIUM_EXC_TAX As Integer = AC_ARR_PS_PREMIUM_EXC_TAX
    Public Const AC_ARR_PB_COMMISSION As Integer = AC_ARR_PS_COMMISSION_INC_TAX
    Public Const AC_ARR_PB_COMMISSION_CHARGE As Integer = AC_ARR_PS_COMMISSION_CHARGE
    Public Const AC_ARR_PB_TAX_GROUP_EDITABLE As Integer = AC_ARR_PS_TAX_GROUP_EDITABLE
    Public Const AC_ARR_PB_UPPER As Integer = AC_ARR_PS_UPPER

    Public Const AC_KEY_PB_ARR As Integer = AC_ARR_PB_SECTION_ID


    Public Const AC_ARR_CM_LOWER As Integer = 0
    Public Const AC_ARR_CM_SECTION_ID As Integer = AC_ARR_PS_SECTION_ID
    Public Const AC_ARR_CM_IS_LEVY_SECTION As Integer = AC_ARR_PS_IS_LEVY_SECTION
    Public Const AC_ARR_CM_SECTION_NAME As Integer = AC_ARR_PS_SECTION_NAME
    Public Const AC_ARR_CM_APPLIED As Integer = AC_ARR_PS_APPLIED
    Public Const AC_ARR_CM_PREMIUM_EXC_TAX As Integer = AC_ARR_PS_PREMIUM_EXC_TAX
    Public Const AC_ARR_CM_COMMISSION_PERCENT As Integer = AC_ARR_PS_COMMISSION_PERCENT
    Public Const AC_ARR_CM_COMMISSION_CHARGE As Integer = AC_ARR_PS_COMMISSION_CHARGE
    Public Const AC_ARR_CM_COMMISSION As Integer = AC_ARR_PS_COMMISSION_AMOUNT
    Public Const AC_ARR_CM_COMMISSION_TAX_PERCENT As Integer = AC_ARR_PS_COMMISSION_TAX_PERCENT
    Public Const AC_ARR_CM_COMMISSION_TAX As Integer = AC_ARR_PS_COMMISSION_TAX
    Public Const AC_ARR_CM_TOTAL_COMMISSION As Integer = AC_ARR_PS_COMMISSION_INC_TAX
    Public Const AC_ARR_CM_DEFAULT_COMMISSION_PERCENTAGE As Integer = AC_ARR_PS_COMMISSION_PERCENT_DFLT
    Public Const AC_ARR_CM_DEFAULT_COMMISSION_CHARGE As Integer = AC_ARR_PS_COMMISSION_CHARGE_DFLT
    Public Const AC_ARR_CM_DEFAULT_COMMISSION_MINIMUM As Integer = AC_ARR_PS_COMMISSION_AMOUNT_DFLT
    Public Const AC_ARR_CM_OVERRIDE As Integer = AC_ARR_PS_OVERRIDE
    Public Const AC_ARR_CM_UPPER As Integer = AC_ARR_PS_UPPER

    Public Const AC_KEY_CM_ARR As Integer = AC_ARR_CM_SECTION_ID


    Public Const AC_ARR_TP_LOWER As Integer = 0
    Public Const AC_ARR_TP_THIRDPARTY_ID As Integer = 0
    Public Const AC_ARR_TP_THIRDPARTY_NAME As Integer = 1
    Public Const AC_ARR_TP_THIRDPARTY_TYPE As Integer = 2
    Public Const AC_ARR_TP_AMOUNT As Integer = 3
    Public Const AC_ARR_TP_COMMISSION_PERCENT As Integer = 4
    Public Const AC_ARR_TP_COMMISSION_CHARGE As Integer = 5
    Public Const AC_ARR_TP_THIRDPARTY_COMMISSION As Integer = 6
    Public Const AC_ARR_TP_TAX_GROUP As Integer = 7
    Public Const AC_ARR_TP_TAX_RATE As Integer = 8
    Public Const AC_ARR_TP_TAX_AMOUNT As Integer = 9
    Public Const AC_ARR_TP_COMMISSION_INC_TAX As Integer = 10
    Public Const AC_ARR_TP_APPLIED_TO As Integer = 11
    Public Const AC_ARR_TP_IS_MINIMUM_BROKERAGE As Integer = 12
    Public Const AC_ARR_TP_MINIMUM_BROKERAGE As Integer = 12
    Public Const AC_ARR_TP_TAX_GROUP_ID As Integer = 13
    Public Const AC_ARR_TP_ISVIEWABLEONLY As Integer = 14
    Public Const AC_ARR_TP_OVERRIDE As Integer = 15
    Public Const AC_ARR_TP_DEFAULT_COMMISSION_PERCENT As Integer = 16
    Public Const AC_ARR_TP_DEFAULT_COMMISSION_CHARGE As Integer = 17
    Public Const AC_ARR_TP_DEFAULT_COMMISSION_MINIMUM As Integer = 18
    Public Const AC_ARR_TP_UPPER As Integer = 18

    Public Const AC_KEY_TP_ARR As Integer = AC_ARR_TP_THIRDPARTY_ID


    Public Const AC_ARR_AO_LOWER As Integer = 0
    Public Const AC_ARR_AO_ACCOUNT_ID As Integer = 0
    Public Const AC_ARR_AO_ADDON_TYPE As Integer = 1
    Public Const AC_ARR_AO_ADDON_ACCOUNT As Integer = 2
    Public Const AC_ARR_AO_PREMIUM_EXC_TAX As Integer = 3
    Public Const AC_ARR_AO_TAX_GROUP_ID As Integer = 4
    Public Const AC_ARR_AO_TAX_GROUP As Integer = 5
    Public Const AC_ARR_AO_TAX_PERCENT As Integer = 6
    Public Const AC_ARR_AO_TAX_AMOUNT As Integer = 7
    Public Const AC_ARR_AO_PREMIUM_INC_TAX As Integer = 8
    Public Const AC_ARR_AO_COMMISSION_PERCENT As Integer = 9
    Public Const AC_ARR_AO_ADDON_COMMISSION As Integer = 10
    Public Const AC_ARR_AO_INSFEE_TYPE As Integer = 11
    Public Const AC_ARR_AO_FSA_TYPE_OF_SALE_Id As Integer = 12
    Public Const AC_ARR_AO_FSA_TYPE_OF_SALE As Integer = 13
    Public Const AC_ARR_AO_PREMIUM_PERCENT As Integer = 14
    Public Const AC_ARR_AO_ADDON_ACCOUNT_CODE As Integer = 15
    Public Const AC_ARR_AO_FEE_CHARGE As Integer = 16

    Public Const AC_ARR_AO_UPPER As Integer = 16

    Public Const AC_ARR_AO_KEY As Integer = AC_ARR_AO_ACCOUNT_ID

    Public Const AC_ARR_CO_LOWER As Integer = 0
    Public Const AC_ARR_CO_INSURER_ID As Integer = 0
    Public Const AC_ARR_CO_INSURER_NAME As Integer = 1
    Public Const AC_ARR_CO_INSURER_SHARE As Integer = 2
    Public Const AC_ARR_CO_INSURER_COVER As Integer = 3
    Public Const AC_ARR_CO_INSURER_PREMIUM_INC_TAX As Integer = 4
    Public Const AC_ARR_CO_INSURER_PREMIUM_TAX As Integer = 5
    Public Const AC_ARR_CO_INSURER_PREMIUM_EXC_TAX As Integer = 6
    Public Const AC_ARR_CO_COMMISSION_INC_TAX As Integer = 7
    Public Const AC_ARR_CO_INSURER_POLICY_NUMBER As Integer = 8
    Public Const AC_ARR_CO_RISK_TRANSFER As Integer = 9
    Public Const AC_ARR_CO_LEAD_UNDERWRITER As Integer = 10
    Public Const AC_ARR_CO_BUREAU_ID As Integer = 11
    Public Const AC_ARR_CO_BUREAU_NAME As Integer = 12
    Public Const AC_ARR_CO_LINE_STANDS As Integer = 13
    Public Const AC_ARR_CO_WRITTEN As Integer = 14
    Public Const AC_ARR_CO_RISK_TRANSFER_EDITABLE As Integer = 15
    Public Const AC_ARR_CO_UPPER As Integer = 15

    Public Const AC_ARR_CO_KEY As Integer = AC_ARR_CO_INSURER_ID

    Public Const AC_ARR_COSD_LOWER As Integer = 0
    Public Const AC_ARR_COSD_SECTION_ID As Integer = AC_ARR_PCS_SECTION_ID
    Public Const AC_ARR_COSD_SECTION_NAME As Integer = AC_ARR_PCS_SECTION_NAME
    Public Const AC_ARR_COSD_APPLIED As Integer = AC_ARR_PCS_APPLIED
    Public Const AC_ARR_COSD_SHARE_PERCENT As Integer = AC_ARR_PCS_SHARE_PERCENT
    Public Const AC_ARR_COSD_PREMIUM_INC_TAX As Integer = AC_ARR_PCS_PREMIUM_INC_TAX
    Public Const AC_ARR_COSD_TAX_GROUP As Integer = AC_ARR_PCS_TAX_GROUP
    Public Const AC_ARR_COSD_TAX_PERCENT As Integer = AC_ARR_PCS_TAX_PERCENT
    Public Const AC_ARR_COSD_PREMIUM_TAX As Integer = AC_ARR_PCS_PREMIUM_TAX
    Public Const AC_ARR_COSD_PREMIUM_EXC_TAX As Integer = AC_ARR_PCS_PREMIUM_EXC_TAX
    Public Const AC_ARR_COSD_COMMISSION_INC_TAX As Integer = AC_ARR_PCS_COMMISSION_INC_TAX
    Public Const AC_ARR_COSD_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE
    Public Const AC_ARR_COSD_COMMISSION_TAX_PERCENT As Integer = AC_ARR_PCS_COMMISSION_TAX_PERCENT
    Public Const AC_ARR_COSD_TAX_GROUP_EDITABLE As Integer = AC_ARR_PCS_TAX_GROUP_EDITABLE
    Public Const AC_ARR_COSD_UPPER As Integer = AC_ARR_PCS_UPPER

    Public Const AC_ARR_COSD_KEY As Integer = AC_ARR_COSD_SECTION_ID


    Public Const AC_ARR_LISD_LOWER As Integer = 0
    Public Const AC_ARR_LISD_SECTION_ID As Integer = AC_ARR_PCS_SECTION_ID
    Public Const AC_ARR_LISD_SECTION_NAME As Integer = AC_ARR_PCS_SECTION_NAME
    Public Const AC_ARR_LISD_APPLIED As Integer = AC_ARR_PCS_APPLIED
    Public Const AC_ARR_LISD_WRITTEN_PERCENT As Integer = AC_ARR_PCS_WRITTEN_PERCENT
    Public Const AC_ARR_LISD_SIGNED_PERCENT As Integer = AC_ARR_PCS_SHARE_PERCENT
    Public Const AC_ARR_LISD_SIGNED_PREMIUM_INC_TAX As Integer = AC_ARR_PCS_PREMIUM_INC_TAX
    Public Const AC_ARR_LISD_TAX_GROUP As Integer = AC_ARR_PCS_TAX_GROUP
    Public Const AC_ARR_LISD_TAX_PERCENT As Integer = AC_ARR_PCS_TAX_PERCENT
    Public Const AC_ARR_LISD_PREMIUM_TAX As Integer = AC_ARR_PCS_PREMIUM_TAX
    Public Const AC_ARR_LISD_SIGNED_PREMIUM_EXC_TAX As Integer = AC_ARR_PCS_PREMIUM_EXC_TAX
    Public Const AC_ARR_LISD_COMMISSION_INC_TAX As Integer = AC_ARR_PCS_COMMISSION_INC_TAX
    Public Const AC_ARR_LISD_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE
    Public Const AC_ARR_LISD_COMMISSION_TAX_PERCENT As Integer = AC_ARR_PCS_COMMISSION_TAX_PERCENT
    Public Const AC_ARR_LISD_TAX_GROUP_EDITABLE As Integer = AC_ARR_PCS_TAX_GROUP_EDITABLE
    Public Const AC_ARR_LISD_UPPER As Integer = AC_ARR_PCS_UPPER

    Public Const AC_ARR_LISD_KEY As Integer = AC_ARR_LISD_SECTION_ID



    Public Const AC_ARR_LDCM_LOWER As Integer = 0
    Public Const AC_ARR_LDCM_SECTION_ID As Integer = AC_ARR_PCS_SECTION_ID
    Public Const AC_ARR_LDCM_IS_LEVY_SECTION As Integer = AC_ARR_PCS_IS_LEVY_SECTION
    Public Const AC_ARR_LDCM_SECTION_NAME As Integer = AC_ARR_PCS_SECTION_NAME
    Public Const AC_ARR_LDCM_APPLIED As Integer = AC_ARR_PCS_APPLIED
    Public Const AC_ARR_LDCM_SIGNED_PREMIUM_EXC_TAX As Integer = AC_ARR_PCS_PREMIUM_EXC_TAX
    Public Const AC_ARR_LDCM_COMMISSION_PERCENT As Integer = AC_ARR_PCS_COMMISSION_PERCENT
    Public Const AC_ARR_LDCM_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE
    Public Const AC_ARR_LDCM_COMMISSION As Integer = AC_ARR_PCS_COMMISSION_AMOUNT
    Public Const AC_ARR_LDCM_COMMISSION_TAX_PERCENT As Integer = AC_ARR_PCS_COMMISSION_TAX_PERCENT
    Public Const AC_ARR_LDCM_COMMISSION_TAX As Integer = AC_ARR_PCS_COMMISSION_TAX
    Public Const AC_ARR_LDCM_COMMISSION_INC_TAX As Integer = AC_ARR_PCS_COMMISSION_INC_TAX
    Public Const AC_ARR_LDCM_CALCULATION_BASIS As Integer = AC_ARR_PCS_CALCULATION_BASIS
    Public Const AC_ARR_LDCM_DEFAULT_COMMISSION_PERCENT As Integer = AC_ARR_PCS_COMMISSION_PERCENT_DFLT
    Public Const AC_ARR_LDCM_DEFAULT_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE_DFLT
    Public Const AC_ARR_LDCM_DEFAULT_COMMISSION_MINIMUM As Integer = AC_ARR_PCS_COMMISSION_MINIMUM_DFLT
    Public Const AC_ARR_LDCM_OVERRIDE As Integer = AC_ARR_PCS_OVERRIDE
    Public Const AC_ARR_LDCM_UPPER As Integer = AC_ARR_PCS_UPPER

    Public Const AC_ARR_COCM_LOWER As Integer = 0
    Public Const AC_ARR_COCM_SECTION_ID As Integer = AC_ARR_PCS_SECTION_ID
    Public Const AC_ARR_COCM_IS_LEVY_SECTION As Integer = AC_ARR_PCS_IS_LEVY_SECTION
    Public Const AC_ARR_COCM_SECTION_NAME As Integer = AC_ARR_PCS_SECTION_NAME
    Public Const AC_ARR_COCM_APPLIED As Integer = AC_ARR_PCS_APPLIED
    Public Const AC_ARR_COCM_PREMIUM_EXC_TAX As Integer = AC_ARR_PCS_PREMIUM_EXC_TAX
    Public Const AC_ARR_COCM_COMMISSION_PERCENT As Integer = AC_ARR_PCS_COMMISSION_PERCENT
    Public Const AC_ARR_COCM_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE
    Public Const AC_ARR_COCM_COMMISSION As Integer = AC_ARR_PCS_COMMISSION_AMOUNT
    Public Const AC_ARR_COCM_COMMISSION_TAX_PERCENT As Integer = AC_ARR_PCS_COMMISSION_TAX_PERCENT
    Public Const AC_ARR_COCM_COMMISSION_TAX As Integer = AC_ARR_PCS_COMMISSION_TAX
    Public Const AC_ARR_COCM_COMMISSION_INC_TAX As Integer = AC_ARR_PCS_COMMISSION_INC_TAX
    Public Const AC_ARR_COCM_CALCULATION_BASIS As Integer = AC_ARR_PCS_CALCULATION_BASIS
    Public Const AC_ARR_COCM_DEFAULT_COMMISSION_PERCENT As Integer = AC_ARR_PCS_COMMISSION_PERCENT_DFLT
    Public Const AC_ARR_COCM_DEFAULT_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE_DFLT
    Public Const AC_ARR_COCM_DEFAULT_COMMISSION_MINIMUM As Integer = AC_ARR_PCS_COMMISSION_MINIMUM_DFLT
    Public Const AC_ARR_COCM_OVERRIDE As Integer = AC_ARR_PCS_OVERRIDE
    Public Const AC_ARR_COCM_UPPER As Integer = AC_ARR_PCS_UPPER


    Public Const AC_KEY_COCM_ARR As Integer = AC_ARR_COCM_SECTION_ID

    Public Const AC_ARR_SB_LOWER As Integer = 0
    Public Const AC_ARR_SB_ID As Integer = AC_ARR_PCS_ID
    Public Const AC_ARR_SB_INSURER_ID As Integer = AC_ARR_PCS_INSURANCE_FILE_CNT
    Public Const AC_ARR_SB_INSURER_NAME As Integer = AC_ARR_PCS_PARTY_NAME
    Public Const AC_ARR_SB_SECTION_ID As Integer = AC_ARR_PCS_SECTION_ID
    Public Const AC_ARR_SB_SECTION_NAME As Integer = AC_ARR_PCS_SECTION_NAME
    Public Const AC_ARR_SB_PREMIUM_INC_TAX As Integer = AC_ARR_PCS_PREMIUM_INC_TAX
    Public Const AC_ARR_SB_TAX_GROUP As Integer = AC_ARR_PCS_TAX_GROUP
    Public Const AC_ARR_SB_TAX_PERCENT As Integer = AC_ARR_PCS_TAX_PERCENT
    Public Const AC_ARR_SB_PREMIUM_TAX As Integer = AC_ARR_PCS_PREMIUM_TAX
    Public Const AC_ARR_SB_PREMIUM_EXC_TAX As Integer = AC_ARR_PCS_PREMIUM_EXC_TAX
    Public Const AC_ARR_SB_UPPER As Integer = AC_ARR_PCS_UPPER

    Public Const AC_ARR_SB_KEY As Integer = AC_ARR_SB_ID

    Public Const AC_ARR_IA_LOWER As Integer = 0
    Public Const AC_ARR_IA_PARTY_ID As Integer = 0
    Public Const AC_ARR_IA_PARTY_NAME As Integer = 1
    Public Const AC_ARR_IA_SHARE_PERCENT As Integer = 2
    Public Const AC_ARR_IA_SHARE_INC_TAX As Integer = 3
    Public Const AC_ARR_IA_SHARE_TAX As Integer = 4
    Public Const AC_ARR_IA_SHARE_EXC_TAX As Integer = 5
    Public Const AC_ARR_IA_UPPER As Integer = 5

    Public Const AC_ARR_IA_KEY As Integer = AC_ARR_IA_PARTY_ID

    Public Const AC_ARR_CB_LOWER As Integer = 0
    Public Const AC_ARR_CB_ID As Integer = AC_ARR_PCS_ID
    Public Const AC_ARR_CB_INSURER_ID As Integer = AC_ARR_PCS_PARTY_CNT
    Public Const AC_ARR_CB_INSURER_NAME As Integer = AC_ARR_PCS_PARTY_NAME
    Public Const AC_ARR_CB_SECTION_ID As Integer = AC_ARR_PCS_SECTION_ID
    Public Const AC_ARR_CB_SECTION_NAME As Integer = AC_ARR_PCS_SECTION_NAME
    Public Const AC_ARR_CB_PREMIUM_EXC_TAX As Integer = AC_ARR_PCS_PREMIUM_EXC_TAX
    Public Const AC_ARR_CB_COMMISSION_PERCENT As Integer = AC_ARR_PCS_COMMISSION_PERCENT
    Public Const AC_ARR_CB_COMMISSION_CHARGE As Integer = AC_ARR_PCS_COMMISSION_CHARGE
    Public Const AC_ARR_CB_COMMISSION_EXC_TAX As Integer = AC_ARR_PCS_COMMISSION_AMOUNT
    Public Const AC_ARR_CB_TAX_GROUP As Integer = AC_ARR_PCS_COMMISSION_TAX_GROUP
    Public Const AC_ARR_CB_TAX_RATE As Integer = AC_ARR_PCS_COMMISSION_TAX_PERCENT
    Public Const AC_ARR_CB_COMMISSION_TAX As Integer = AC_ARR_PCS_COMMISSION_TAX
    Public Const AC_ARR_CB_COMMISSION_INC_TAX As Integer = AC_ARR_PCS_COMMISSION_INC_TAX
    Public Const AC_ARR_CB_UPPER As Integer = AC_ARR_PCS_UPPER

    Public Const AC_ARR_CB_KEY As Integer = AC_ARR_CB_ID

    Public Const AC_ARR_ST_LOWER As Integer = 0
    Public Const AC_ARR_ST_SECTION_ID As Integer = 0
    Public Const AC_ARR_ST_SECTION_NAME As Integer = 1
    Public Const AC_ARR_ST_IS_LEVY_SECTION As Integer = 2
    Public Const AC_ARR_ST_TAX_GROUP_ID As Integer = 3
    Public Const AC_ARR_ST_TAX_GROUP As Integer = 4
    Public Const AC_ARR_ST_TAX_PERCENT As Integer = 5
    Public Const AC_ARR_ST_INCLUDE_COMMISSION_CALC As Integer = 6
    Public Const AC_ARR_ST_INCLUDE_PREMIUM_CALC As Integer = 7
    Public Const AC_ARR_ST_CALCULATION_BASIS As Integer = 8
    Public Const AC_ARR_ST_TAX_GROUP_EDITABLE As Integer = 9
    Public Const AC_ARR_ST_UPPER As Integer = 9


    Public Const AC_ARR_ST_KEY As Integer = AC_ARR_ST_SECTION_ID

    Public Const AC_ARR_IST_LOWER As Integer = 0
    Public Const AC_ARR_IST_SECTION_ID As Integer = 0
    Public Const AC_ARR_IST_SECTION_NAME As Integer = 1
    Public Const AC_ARR_IST_IS_LEVY_SECTION As Integer = 2
    Public Const AC_ARR_IST_TAX_GROUP_ID As Integer = 3
    Public Const AC_ARR_IST_TAX_GROUP As Integer = 4
    Public Const AC_ARR_IST_TAX_PERCENT As Integer = 5
    Public Const AC_ARR_IST_COMMISSION_PERCENT As Integer = 6
    Public Const AC_ARR_IST_COMMISSION_AMOUNT As Integer = 7
    Public Const AC_ARR_IST_MINIMUM_BROKERAGE As Integer = 8
    Public Const AC_ARR_IST_CALCULATION_BASIS As Integer = 9
    Public Const AC_ARR_IST_UPPER As Integer = 9

    Public Const AC_ARR_IST_KEY As Integer = AC_ARR_IST_SECTION_ID

    Public Const AC_ARR_PET_LOWER As Integer = 0
    Public Const AC_ARR_PET_SECTION_ID As Integer = 0
    Public Const AC_ARR_PET_PREMIUM_EXC_TAX As Integer = 1
    Public Const AC_ARR_PET_UPPER As Integer = 1

    Public Const AC_ARR_APP_LOWER As Integer = 0
    Public Const AC_ARR_APP_APPLIED As Integer = 0
    Public Const AC_ARR_APP_ACTIVE As Integer = 1
    Public Const AC_ARR_APP_UPPER As Integer = 1

    'Fot Commission Totals
    Public Const AC_ARR_CMTOT_LOWER As Integer = 0
    Public Const AC_ARR_CMTOT_SECTION_ID As Integer = 0
    Public Const AC_ARR_CMTOT_COMMISSION_AMOUNT As Integer = 1
    Public Const AC_ARR_CMTOT_COMMISSION_CHARGE As Integer = 2
    Public Const AC_ARR_CMTOT_COMMISSION_TAX_PERCENT As Integer = 3
    Public Const AC_ARR_CMTOT_UPPER As Integer = 3

    Public Const AC_ARR_TOTALS_LOWER As Integer = 0
    Public Const AC_ARR_TOTALS_EXC_TAX As Integer = 0
    Public Const AC_ARR_TOTALS_TAX As Integer = 1
    Public Const AC_ARR_TOTALS_INC_TAX As Integer = 2
    Public Const AC_ARR_TOTALS_EXTRA As Integer = 3
    Public Const AC_ARR_TOTALS_TOTAL As Integer = 4
    Public Const AC_ARR_TOTALS_UPPER As Integer = 4

    Public Const AC_ARR_ORIGARRAY_LOWER As Integer = 0
    Public Const AC_ARR_ORIGARRAY_SECTION_ID As Integer = 0
    Public Const AC_ARR_ORIGARRAY_PERSIST_STATE As Integer = 1
    Public Const AC_ARR_ORIGARRAY_PCS_ID As Integer = 2
    Public Const AC_ARR_ORIGARRAY_UPPER As Integer = 2

    Public Const ACFeeTypeInsurerFeeCredit As String = "Insurer Fee Credit"
    Public Const ACFeeTypeInsurerFeeDebit As String = "Insurer Fee Debit"
End Module