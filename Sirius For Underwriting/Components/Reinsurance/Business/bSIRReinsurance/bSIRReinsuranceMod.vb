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
	Public Const ACApp As String = "bSIRReinsurance"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' Tax transaction type constants
	Public Const TREATYPREMIUMTAXTYPE As String = "TTRITP"
	Public Const TREATYCOMMISSIONTAXTYPE As String = "TTRITC"
	Public Const FACPREMIUMTAXTYPE As String = "TTRIFP"
	Public Const FACCOMMISSIONTAXTYPE As String = "TTRIFC"
	
	
	
	' ***************************************************************** '
	'                   REINSURANCE ARRAY ENUMERATORS
	' ***************************************************************** '
	Public Enum RIArrangementEnum
		DBRIArrangementID
		DBRIBandID
		DBRIModelID
		DBRISumInsured
		DBRIPremium
		DBRIOriginalFlag
		DBRIIsModified
		DBRIFacPremiumType
		' Max count for array manipulation
		DBRIMax = RIArrangementEnum.DBRIFacPremiumType
	End Enum
	
	Public Enum RIArrangementLineEnum
		' Grid fields
		DBRILName
		DBRILDefaultShare
		DBRILThisShare
		DBRILSumInsured
		DBRILPremium
		DBRILPremiumTax
		DBRILCommPercent
		DBRILCommission
		DBRILCommissionTax
		DBRILAgreementCode
		' Supporting fields
		DBRILLineID
		DBRILType
		DBRILTreatyID
		DBRILPartyCnt
		DBRILPriority
		DBRILLines
		DBRILLineLimit
		DBRILPremiumPercent
		DBRILIsCommissionModified
		' Max count for array manipulation
		DBRILMax = RIArrangementLineEnum.DBRILIsCommissionModified
	End Enum
End Module