Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '

	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '

	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "bSIRRIModel"

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
	End Enum
	Public DBMMax As RIModelEnum = RIModelEnum.DBMXOLCatReinstatements

	' ***************************************************************** '
	'                      RI MODEL LINE ARRAY
	' ***************************************************************** '
	Public Enum TreatyPartyEnum
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
		DBMLTreatyTypeID
		DBMLRITypeId
		DBMLIsObligatory
		DBMLCedePremiumOnly
		DBMLTreatyCode
		DBMLReinsuranceTypeCode
		DBMLTreatyTypeCode
		DBMLeffective_date
		DBMLexpiry_date
		DBMLPremiumCalculationBasis
		DBMLRIIsVariableQuotaShare
	End Enum
	Public Enum TreatyQuotaShareEnum
		DBMLVariableQuotaShareId
		DBMLSALowerLimit
		DBMLSAUpperLimit
		DBMLSharePercent
		DBMLTreatyLimit
		DBMLTreatyId
		DBMLRIModelId
		DBMLRIModelLineID
	End Enum
	Public DBMVMax As TreatyQuotaShareEnum = TreatyQuotaShareEnum.DBMLRIModelLineID
	Public DBMLMax As TreatyPartyEnum = TreatyPartyEnum.DBMLRIIsVariableQuotaShare
End Module