Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '

	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iPMURIModel"

	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"


	' ***************************************************************** '
	'                         RI MODEL ARRAY
	' ***************************************************************** '
	Public Enum RIModelEnum
		DBMRIModelID
		DBMCode
		DBMDescription
		DBMIsDeleted
		DBMEffectiveDate
		DBMExpiryDate
		DBMRIModelType
		DBMFACPremiumType
		DBMClaimAllocationType
		DBMCurrencyID
		DBMCurrencyDescription
		DBMXOLClmRIModelID
		DBMXOLClmLimit
		DBMXOLCatRIModelID
		DBMXOLCatLimit
		DBMXOLCatReinstatements
		DBMTreatyPremiumType
	End Enum
	Public DBMMax As RIModelEnum = RIModelEnum.DBMTreatyPremiumType

	' ***************************************************************** '
	'                      RI MODEL LINE ARRAY
	' ***************************************************************** '
	Public Enum RIModelLineEnum
		DBMLRIModelLineID
		DBMLRIModelID
		DBMLPriority
		DBMLNumberOfLines
		DBMLLineLimit
		DBMLTreatyID
		DBMLDescription
		DBMLSharePercent
		DBMLLowerLimit
		DBMLCedingrate
		DBMLTreatyTypeId
		' Start( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.1)
		DBMLRITypeID
		' End( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.1)
		'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
		DBMLRIIsObligatory
		DBMLCedePremiumOnly    'E005 Part1
		'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)		
		DBMLTreatyCode
		DBMLReinsuranceTypeCode
		DBMLTreatyTypeCode
		DBMLeffective_date
		DBMLexpiry_date
		DBMLPremiumCalculationBasis
		'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
		DBMLRIIsVariableQuotaShare

	End Enum
	Public Enum RIModelLineQuotaShareEnum
		DBMLVariableQuotaShareId
		DBMLSALowerLimit
		DBMLSAUpperLimit
		DBMLSharePercent
		DBMLTreatyLimit
		DBMLTreatyId
		DBMLRIModelId
		DBMLRIModelLineID
	End Enum
	Public DBMVMax As RIModelLineQuotaShareEnum = RIModelLineQuotaShareEnum.DBMLRIModelLineID
	'Start( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.1)
	'Changed the DBMLMAX from DBMLTreatyTypeId to DBMLRITypeID
	'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
	'Note:- DBMLMax is changed to DBMLRIsObligatory
	'Public Const DBMLMax = DBMLRITypeID
	Public DBMLMax As RIModelLineEnum = RIModelLineEnum.DBMLRIIsVariableQuotaShare
	'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
	' End( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.1)

	' Start( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.1)
	Public Const ACProportional As Integer = 1
	Public Const ACtreatyExcessofLoss As Integer = 2
	Public Const ACQuotaShare As Integer = 2
	Public Const ACExcessofLoss As Integer = 5
	Public Const ACRetained As Integer = 9
	Public Const ACSurplus As Integer = 6
	Public Const ACSecondSurplus As Integer = 7
	Public Const ACThirdSurplus As Integer = 8
	Public Const ACProportionalXOL As Integer = 13
	Public Const ACQuotaShareRetained As Integer = 14
	Public Const ACCat As Integer = 12

	' ***************************************************************** '
	'                   PREMIUM CALCULATION BASIS ENUM
	' ***************************************************************** '
	Public Enum PremiumCalculationBasisEnum
		PROPGRSS = 1
		PROPGRFAC = 2
		PRGRFACCAT = 3
		PRGRFACXOL = 4
		PRGFACXCAT = 5
		PROPRETND = 6
		XOLRATEGRO = 7
		XOLGRSFAC = 8
		XOLFACPRI = 9
		XOLPRICAT = 10
		XOLFACCAT = 11
		CATRATEGRO = 12
		CATGRSFAC = 13
		CATFACPRI = 14
		CATPRIXOL = 15
		CATFACXOL = 16
		PROPNTXOPX = 17
		PROPNTXPC = 18
		CATNTPRXO = 19
		CATNTXOPX = 20
		PXGRS = 21
		PXGRSFAC = 22
		PXFACPRP = 23
		PXFPRPXOL = 24
		PXFPRPCAT = 25
		PXFPRCATXL = 26
		PXFACCAT = 27
		PXFACCATXL = 28
		' New Retained (RET) entries - reinsurance_type_id = 9
		T9PROPGRSS = 29
		T9PROPGRFC = 30
		T9PRGRFCCT = 31
		T9PRGRFCXL = 32
		T9PRGFXCAT = 33
		T9PRNTXOPX = 34
		T9PRNTXPCT = 35
		T9PRGRFCPR = 36
		T9PRFCPRXL = 37
		T9PRFCPRCT = 38
		T9PRFCPRPX = 39
	End Enum

	' ***************************************************************** '
	'                   TREATY PREMIUM TYPE ENUM
	' ***************************************************************** '
	Public Enum TreatyPremiumTypeEnum
		Standard = 0
		VariableCessionOrder = 1
	End Enum
	' End( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.1)

	' ***************************************************************** '
	'                        GLOBAL VARIABLES
	' ***************************************************************** '
	' Public source and language ID's from the Object Manager.
	'developer guide no. 107
	<ThreadStatic()>
	Public g_iSourceID As Integer
	'developer guide no. 107
	<ThreadStatic()>
	Public g_iLanguageID As Integer

	' Public instance of the object manager.
	'developer guide no. 107
	<ThreadStatic()>
	Public g_oObjectManager As bObjectManager.ObjectManager
End Module