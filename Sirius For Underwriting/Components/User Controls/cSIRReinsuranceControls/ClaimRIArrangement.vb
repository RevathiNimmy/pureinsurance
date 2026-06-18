Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("ClaimRIArrangement_NET.ClaimRIArrangement")> _
Public NotInheritable Class ClaimRIArrangement 
	
    ' ***************************************************************** '
    '                 RISK REINSURANCE ARRAY ENUMERATOR
    ' ***************************************************************** '
	Public Enum ClaimRIArrangementLineEnum
		' Grid fields
		DBCRIName
		DBCRIDefaultShare
		DBCRIThisShare
		DBCRISumInsured
		DBCRIReserveToDate
		DBCRIThisReserve
		DBCRIPaymentToDate
		DBCRIThisPayment
		DBCRIBalance
		DBCRIAgreementCode
		' Supporting fields
		DBCRILineID
		DBCRIType
		DBCRITreatyID
		DBCRIPartyCnt
		DBCRIXolID
		DBCRIPriority
		DBCRILines
		DBCRILineLimit
		' XOL fields
		DBCRILayer
		DBCRICatastropheID
		DBCRICatastrophe
		DBCRIXolModelID
		DBCRIXolNextModelID
		DBCRIXolNextLimit
        DBCRIIsObligatory
		'Max count for array manipulation
		DBCRIMax = ClaimRIArrangementLineEnum.DBCRIIsObligatory
    End Enum
	
	Public Enum ClaimRI2007ArrangementLineEnum
		' Grid fields
		DBCRI2007Placement
		DBCRI2007Name
		DBCRI2007Retained
		DBCRI2007DefaultShare
		DBCRI2007ThisShare
		DBCRI2007LowerLimit
		DBCRI2007UpperLimit
		DBCRI2007SumInsured
		DBCRI2007ReserveToDate
		DBCRI2007ThisReserve
		DBCRI2007PaymentToDate
		DBCRI2007ThisPayment
        DBCRI2007RecoveredToDate
        DBCRI2007Balance
        DBCRI2007IncurredToDate
        DBCRI2007AgreementCode
        DBCRI2007IsReinsurerApproved
		DBCRI2007AddMode 'Manual or Automatic
		DBCRI2007GroupingID
		DBCRI2007IsRIBroker
		' Supporting fields
		DBCRI2007LineID
		DBCRI2007Type
		DBCRI2007TreatyID
		DBCRI2007PartyCnt
		DBCRI2007XolID
		DBCRI2007Priority
		DBCRI2007Lines
		DBCRI2007LineLimit
		' XOL fields
		DBCRI2007Layer
		DBCRI2007CatastropheID
		DBCRI2007Catastrophe
		DBCRI2007XolModelID
		DBCRI2007XolNextModelID
		DBCRI2007XolNextLimit
        DBCRI2007Incurred
        DBCRI2007IsObligatory
        DBCRI2007CedePremiumOnly
        DBCRI2007Max = ClaimRI2007ArrangementLineEnum.DBCRI2007CedePremiumOnly

	End Enum

	' ***************************************************************** '
	'                        PUBLIC PROPERTIES
	' ***************************************************************** '
	
	' Band financial totals
	Public SumInsured As Decimal
	Public ReserveToDate As Decimal
	Public PaymentToDate As Decimal
	Public ThisReserve As Decimal
	Public ThisPayment As Decimal
	Public CatastropheCodeID As Integer
    Public bRecovery As Boolean
	Public RecoveredToDate As Decimal
    ' XOL triggers
	Public XolClmModelID As Integer
	Public XolClmLimit As Decimal
	Public XolCatModelID As Integer
	Public XolCatLimit As Decimal
	Public XolCatReinstatements As Integer
    Public Incurred As Decimal
    ' Reinsurance lines
	Public ReinsuranceLines( ,  ) As Object
    Public IncurredToDate As Decimal
    Public ReadOnly Property Balance() As Decimal
        Get
            ' Calculate balance
            Dim result As Decimal
            If bRecovery Then
                result = (ReserveToDate + ThisReserve) - PaymentToDate
                Incurred = ReserveToDate + ThisReserve
            Else
                result = (ReserveToDate + ThisReserve) - (PaymentToDate + ThisPayment)
                Incurred = ReserveToDate + ThisReserve
            End If
            Return result
        End Get
    End Property
	
	Public ReadOnly Property Payment() As Decimal
		Get
			' Return the total payments for this band
			Return PaymentToDate + ThisPayment
		End Get
	End Property
	
	Public ReadOnly Property Reserve() As Decimal
		Get
			' Return the total reserve for this band
			Return ReserveToDate + ThisReserve
		End Get
	End Property
End Class
