Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("RiskRIArrangement_NET.RiskRIArrangement")> _
Public NotInheritable Class RiskRIArrangement 
    ' ***************************************************************** '
    '                       PROPERTY ENUMERATORS
    ' ***************************************************************** '
	Public Enum FACPremiumEnum
		FACPremiumIsProportional
		FACPremiumIsNotProportional
	End Enum
	
	' ***************************************************************** '
	'                 RISK REINSURANCE ARRAY ENUMERATOR
	' ***************************************************************** '
	Public Enum RiskReinsuranceEnum
		' Grid fields
		DBRIName
		DBRIDefaultShare
		DBRIThisShare
		DBRISumInsured
		DBRIPremium
		DBRITax
		DBRICommPercent
		DBRICommission
		DBRICommTax
		DBRIAgreementCode
		' Supporting fields
		DBRILineID
		DBRIType
		DBRITreatyID
		DBRIPartyCnt
		DBRIPriority
		DBRILines
		DBRILineLimit
		DBRIPremiumPercent
		DBRIIsCommissionModified
        DBRIIsObligatorty
        DBRITreatyCode
		'Max count for array manipulation
        DBRIMax = RiskReinsuranceEnum.DBRITreatyCode
    End Enum
	
	' Enum designed to support the new RI2007 Reinsurance model
	Public Enum RiskReinsuranceEnumRI2007
		' Grid fields
		DBRI2007Placement
		DBRI2007Name
		DBRI2007Retained
		DBRI2007DefaultShare
		DBRI2007ThisShare
		DBRI2007LowerLimit
		DBRI2007UpperLimit
		DBRI2007SumInsured
		DBRI2007Premium
		DBRI2007Tax
		DBRI2007CommPercent
		DBRI2007Commission
		DBRI2007CommTax
        DBRI2007AgreementCode
        DBRI2007IsReinsurerApproved
		DBRI2007AddedMode
		DBRI2007GroupingID
		DBRI2007IsRIBroker
		' Supporting fields
		DBRI2007LineID
		DBRI2007type
		DBRI2007TreatyID
		DBRI2007PartyCnt
		DBRI2007Priority
		DBRI2007Lines
		DBRI2007PremiumPercent
		DBRI2007IsCommissionModified
        DBRI2007IsObligatory
        DBRI2007CedePremiumOnly
        'Max count for array manipulation
        DBRI2007ReinsuranceType
        DBRI2007Max = RiskReinsuranceEnumRI2007.DBRI2007ReinsuranceType
    End Enum

	' ***************************************************************** '
	'                        PUBLIC PROPERTIES
	' ***************************************************************** '
	
	' Header properties
	Public IsOriginal As Boolean
	
	' Band financial totals
	Public SumInsured As Decimal
	Public Premium As Decimal
	
	' Reinsurance lines
	Public ReinsuranceLines As Object
	
	' Model configuration properties
	Public FACPremiumMethod As FACPremiumEnum
	
	Public TransactionType As String = ""
    'Extended Limits
    Public IsExtendedLimitApplied As Boolean
    Public ExtendedLimitamount As Decimal
    Private m_iRIManualPremiumAdjustments As Integer
    Public XOLRIModelId As Integer
    Public RIModelId As Integer
	Public Property RIManualPremiumAdjustments() As Integer
		Get
			Return m_iRIManualPremiumAdjustments
		End Get
		Set(ByVal Value As Integer)
			m_iRIManualPremiumAdjustments = Value
		End Set
	End Property

End Class
