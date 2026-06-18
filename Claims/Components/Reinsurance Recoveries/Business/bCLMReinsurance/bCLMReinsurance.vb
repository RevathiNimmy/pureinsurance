Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "bCLMReinsurance"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' ***************************************************************** '
	'                   REINSURANCE ARRAY ENUMERATORS
	' ***************************************************************** '
	Public Enum RIArrangementEnum
		DBCRIArrangementID
		DBCRIBandID
		DBCRIModelID
		DBCRISumInsured
		DBCRIReserveToDate
		DBCRIPaymentToDate
		DBCRIThisReserve
		DBCRIThisPayment
		DBCRIIsModified
		DBCRICatastropheCodeID
		DBCRIXolClmModelID
		DBCRIXolClmLimit
		DBCRIXolCatModelID
		DBCRIXolCatLimit
		DBCRIXolCatReinstatements
		' Max count for array manipulation
		DBCRIMax = RIArrangementEnum.DBCRIIsModified
	End Enum
	
	Public Enum RIArrangementLineEnum
		' Grid fields
		DBCRILName
		DBCRILDefaultShare
		DBCRILThisShare
		DBCRILSumInsured
		DBCRILReserveToDate
		DBCRILThisReserve
		DBCRILPaymentToDate
		DBCRILThisPayment
		DBCRILBalance
		DBCRILAgreementCode
		' Supporting fields
		DBCRILLineID
		DBCRILType
		DBCRILTreatyID
		DBCRILPartyCnt
		DBCRILXOLID
		DBCRILPriority
		DBCRILLines
		DBCRILLineLimit
		' XOL fields
		DBCRILLayer
		DBCRILCatastropheID
		DBCRILCatastrophe
		DBCRILXOLModelID
		DBCRILXOLNextModelID
		DBCRILXOLNextLimit
		' Max count for array manipulation
		'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
		DBCRILIsObligatory
		DBCRILMax = RIArrangementLineEnum.DBCRILIsObligatory
		'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
	End Enum
End Module