Option Strict Off
Option Explicit On
Imports System
Module PolicyNumConst
	
	Public Const knIsReadOnly As Integer = 2
	Public Const knMaskCode As Integer = 3
	
	Public Enum enuNumberingSchemeFields
		enuNSF_SCHEME_ID = 0
		enuNSF_CAPTION_ID = 1
		enuNSF_CODE = 2
		enuNSF_DESCRIPTION = 3
		enuNSF_IS_DELETED = 4
		enuNSF_EFFECTIVE_DATE = 5
		enuNSF_TYPE = 6
		enuNSF_SCHEME = 7
		enuNSF_IS_GENERATED = 8
		enuNSF_MASK_CODE = 9
		enuNSF_FIXED_CODE = 10
		enuNSF_NEXT_NUMBER = 11
		enuNSF_HIGHEST_NUMBER = 12
		enuNSF_STEP = 13
		enuNSF_IS_REUSE = 14
		enuNSF_TYPE_DESCRIPTION = 15
		enuNSF_IS_READ_ONLY = 16
		enuNSF_PARTY_TYPE_ID = 17
		enuNSF_IS_DELETED_TEMP = 18
		'Start - Renuka - (WPR87 Paralleling)
		enuNSF_IS_RESET_NUMBER = 21
		enuNSF_NUMBER_OF_DATA_FIELDS = 22
		'End - Renuka - (WPR87 Paralleling)
		enuNSF_NUMBER_OF_FIELDS_IN_LIST = 6
		enuNSF_IS_RESET_DAILY = 20
	End Enum
End Module