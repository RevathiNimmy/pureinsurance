Option Strict Off
Option Explicit On
Imports System
Public Module QuoteConst
	'Follow constants moved to GIIConstants
	'Public Const ACListItemQuotes = "Quotes"
	'Public Const ACListItemDeclines = "Declines"
	'Public Const ACListItemReferrals = "Referrals"
	
	Public Const GIIQD_OBJ_GEMPOLICY As String = "GIIMGEMPOLICY"
	Public Const GIIQD_OBJ_QUICK_QUOTE_RESULT As String = "Quick_Quote_Result"
	Public Const GIIQD_OBJ_REFERRALS As String = "Referrals"
	Public Const GIIQD_OBJ_REFER_REASONS As String = "Refer_Reasons"
	Public Const GIIQD_OBJ_DECLINES As String = "Declines"
	Public Const GIIQD_OBJ_DECLINE_REASONS As String = "Decline_Reasons"
	Public Const GIIQD_OBJ_GIIMGEMQUOTECONFIGURATION As String = "GemQuoteConfiguration"
	Public Const GIIQD_OBJ_NCD As String = "NCD"
	Public Const GIIQD_OBJ_COVER As String = "Cover"
	Public Const GIIQD_OBJ_EXCESS_BREAKDOWN As String = "Excess_Breakdown"
	Public Const GIIQD_OBJ_NOTES_BREAKDOWN As String = "Notes_Breakdown"
	Public Const GIIQD_OBJ_ENDORSEMENTS_BREAKDOWN As String = "Endorsements_Breakdown"
	Public Const GIIQD_OBJ_PREMIUM_ANALYSIS As String = "Premium_Analysis"
	Public Const GIIQD_OBJ_VEHICLE As String = "Vehicle"
	Public Const GIIQD_OBJ_DRIVER As String = "Driver"
	Public Const GIIQD_OBJ_OCCUPATION As String = "Occupation"
	Public Const GIIQD_OBJ_POLICY As String = "Policy"
	
	Public Const GIIQD_OBJ_SAVED_QUOTE As String = "Saved_Quote"
	Public Const GIIQD_OBJ_SAVED_EXCESS As String = "Saved_Excess"
	Public Const GIIQD_OBJ_SAVED_NOTES As String = "Saved_Notes"
	Public Const GIIQD_OBJ_SAVED_ANALYSIS As String = "Saved_Analysis"
	Public Const GIIQD_OBJ_SAVED_ADD_ON As String = "Saved_Add_On"
	
	Public Const GIIAO_OBJ_SELECTED_ADD_ON As String = "Selected_Add_On"
	
	Public Const GIIQD_PROP_POLICY_EFFECTIVE_DATE As String = "Effective_Start_Date"
	Public Const GIIQD_PROP_POLICY_EXPIRY_DATE As String = "Expiry_Date"
	
	Public Const GIIQD_PROP_GIIMGEMPOLICY_ID As String = "GIIMGEMPOLICY_ID"
	Public Const GIIQD_PROP_SCHEME_ID As String = "Scheme_ID"
	Public Const GIIQD_PROP_SCHEME_DESC As String = "Scheme_Desc"
	Public Const GIIQD_PROP_COMMISSION_RATE As String = "Commission_Rate"
	Public Const GIIQD_PROP_COMMISSION_AMOUNT As String = "Commission_Amount"
	Public Const GIIQD_PROP_SCHEME_TYPE As String = "Scheme_Type"
	Public Const GIIQD_PROP_PREMIUM As String = "Premium"
	Public Const GIIQD_PROP_QUOTE_TYPE As String = "Quote Type"
	Public Const GIIQD_PROP_NCB_PROTECTED As String = "NCB_Protected"
	Public Const GIIQD_PROP_COVER_TYPE As String = "Cover_Type"
	Public Const GIIQD_PROP_TOTAL_EXCESS As String = "Total_Excess"
	Public Const GIIQD_PROP_IPT As String = "IPT"
	Public Const GIIQD_PROP_COMPULSORY_EXCESS As String = "Compulsory_Excess"
	Public Const GIIQD_PROP_VOLUNTARY_EXCESS As String = "Voluntary_Excess"
	Public Const GIIQD_PROP_RATES_DATE As String = "Rates_Date"
	Public Const GIIQD_PROP_VEHICLE_GROUP As String = "Vehicle_Group"
	Public Const GIIQD_PROP_VEHICLE_AREA As String = "Vehicle_Area"
	Public Const GIIQD_PROP_NCD_CLAIMED_YEARS As String = "Claimed_Years"
	Public Const GIIQD_PROP_NCD_DISCOUNT As String = "NCD_Discount"
	Public Const GIIQD_PROP_YOUNG_DRIVER_EXCESS As String = "Young_Driver_Excess"
	Public Const GIIQD_PROP_COMPULSORY_NON_AD_EXCESS As String = "Compulsory_Non_AD_Excess"
	Public Const GIIQD_PROP_COMPULSORY_NON_AD_DESC As String = "Compulsory_Non_AD_Desc"
	Public Const GIIQD_PROP_WINDSCREEN_EXCESS As String = "Windscreen_Excess"
	Public Const GIIQD_PROP_WINDSCREEN_LIMIT As String = "Windscreen_Limit"
	Public Const GIIQD_PROP_INSURER_MNEMONIC As String = "Insurer_Mnemonic"
	
	Public Const GIIQD_PROP_COVER_TYPE1 As String = "Cover_Type_1"
	Public Const GIIQD_PROP_COVER_TYPE2 As String = "Cover_Type_2"
	Public Const GIIQD_PROP_COVER_TYPE3 As String = "Cover_Type_3"
	Public Const GIIQD_PROP_MAX_EXCESS As String = "Max_Excess"
	Public Const GIIQD_PROP_VOL_EXCESS As String = "Vol_Excess"
	Public Const GIIQD_PROP_PROTECTED_NCB_IND As String = "Protected_NCB_Ind"
	Public Const GIIQD_PROP_PERMITTED_DRIVERS As String = "Permitted_Drivers"
	Public Const GIIQD_PROP_EXCESS_LEVEL_ONE As String = "Excess_Level_One"
	Public Const GIIQD_PROP_EXCESS_LEVEL_TWO As String = "Excess_Level_Two"
	Public Const GIIQD_PROP_EXCESS_LEVEL_THREE As String = "Excess_Level_Three"
	Public Const GIIQD_PROP_EXCESS_LEVEL_FOUR As String = "Excess_Level_Four"
	Public Const GIIQD_PROP_EXCESS_LEVEL_FIVE As String = "Excess_Level_Five"
	Public Const GIIQD_PROP_PROTECTED_NCB As String = "Protected_NCB"
	Public Const GIIQD_PROP_EXCESS_LEVEL_REQ As String = "Excess_Level_Req"
	
	
	Public Const GIIQD_PROP_REASON As String = "Reason"
	Public Const GIIQD_PROP_CODE As String = "Code"
	Public Const GIIQD_PROP_TEXT As String = "Text"
	Public Const GIIQD_PROP_VEHICLE_PRN As String = "Vehicle PRN"
	Public Const GIIQD_PROP_DRIVER_PRN As String = "Vehicle PRN"
	
	Public Const GIIQD_PROP_CLAIMED_YEARS As String = "Claimed_Years"
	Public Const GIIQD_PROP_CLAIMED_PROTECTION_IND As String = "Claimed_Protection_Reqd_Ind"
	
	Public Const GIIQD_PROP_COVER_CODE As String = "Code"
	Public Const GIIQD_PROP_DRIVERS_ALLOWED As String = "Drivers_Allowed"
	
	'Public Const GIIQD_PROP_AD_EXCESS = "AD_Excess"
	
	Public Const GIIQD_PROP_AMT As String = "Amt"
	Public Const GIIQD_PROP_SECTION_CODE As String = "Section_Code"
	Public Const GIIQD_PROP_DESCRIPTION As String = "Description"
	Public Const GIIQD_PROP_TITLE As String = "Title"
	Public Const GIIQD_PROP_VALUE1 As String = "Value1"
	Public Const GIIQD_PROP_VALUE2 As String = "Value2"
	
	Public Const GIIQD_PROP_AMOUNT As String = "Amount"
	Public Const GIIQD_PROP_RUNNING_TOTAL As String = "Running_Total"
	
	Public Const GIIQD_PROP_CLASS_OF_USE As String = "Class_of_use"
	Public Const GIIQD_PROP_POST_CODE As String = "Post_Code"
	Public Const GIIQD_PROP_VALUE As String = "Value"
	Public Const GIIQD_PROP_DATE_FIRST_REGD As String = "Date_first_regd"
	Public Const GIIQD_REG_NO As String = "Reg_No"
	Public Const GIIQD_PROP_MODEL As String = "Model"
	Public Const GIIQD_PROP_MODEL_NAME As String = "Model_Name"
	
	
	Public Const GIIQD_PROP_SEX As String = "Sex"
	Public Const GIIQD_PROP_DOB As String = "Date_of_birth"
	
	'Public Const GIIQD_PROP_DOB = "Date_of_birth"
	
	Public Const GIIPO_PROP_OVERRIDE_PERCENT As String = "Override_Percent"
	Public Const GIIPO_PROP_OVERRIDE_AMOUNT As String = "Override_Amount"
	Public Const GIIPO_PROP_OVERRIDE_GROSS_PREMIUM As String = "Override_Gross_Premium"
	Public Const GIIPO_PROP_OVERRIDE_PREMIUM_FROM_POT As String = "Override_Premium_From_Pot"
	Public Const GIIPO_PROP_OVERRIDE_AUTHORISATION_CODE As String = "Override_Authorisation_Code"
	Public Const GIIPO_PROP_COMMISSION_OVERRIDE_PCT As String = "Commission_Override_Pct"
	Public Const GIIPO_PROP_COMMISSION_OVERRIDE_AMT As String = "Commission_Override_Amt"
	
	Public Const GIIAO_PROP_ADD_ON_DEF_ID As String = "Add_On_Def_ID"
	Public Const GIIAO_PROP_DESCRIPTION As String = "Description"
	Public Const GIIAO_PROP_NET_COST As String = "Net_Cost"
	Public Const GIIAO_PROP_GROSS_COST As String = "Gross_Cost"
	
	
	'Public Const DO_QUOTE_COVER_TYPE_COMP = "Comprehensive"
	'Public Const DO_QUOTE_COVER_TYPE_TPFT = "Third Party, Fire And Theft"
	'Public Const DO_QUOTE_COVER_TYPE_TP = "Third Party"
	
	Public Const GIIPO_IPT_ANALYSIS_CODE As String = "199"
	Public Const GIIPO_IPT_ANALYSIS_DESCRIPTION As String = "Insurance Premium Tax"
	Public Const GIIPO_OVERRIDE_ANALYSIS_CODE As String = "7998"
	Public Const GIIPO_OVERRIDE_ANALYSIS_DESC As String = "Premium Override"
	Public Const GIIPO_COMM_OVERRIDE_ANALYSIS_CODE As String = "7999"
	Public Const GIIPO_COMM_OVERRIDE_ANALYSIS_DESC As String = "Commission Override"
	
	Public Const GIISCHEME_TYPE_NO_FORMS_NOR_EDI As Integer = 0
	Public Const GIISCHEME_TYPE_WITH_FORMS_OR_EDI As Integer = 1
	
	'JSB - Constants for legacy policy
	'Public Const GIIMLegacyPolicy = "Legacy_Policy"  - JSB 02/09/03 - This is already declared in GIIGISConstants
	Public Const GIIMLegacyPolicyBrkOvrOption As String = "broke_ovr_option"
	
	'Policy -
	' TB - This is a duplicte (in GIIGISConstants.bas) - removed 5/10/00 - as Pay screen won't compile
	' Public Const GIIMGemPolicy = "GIIMGemPolicy"
End Module