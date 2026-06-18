Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports SharedFiles
Friend NotInheritable Class RiskTotalizer 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "RiskTotalizer"
	
	
	' ***************************************************************** '
	'                       PUBLIC PROPERTIES
	' ***************************************************************** '
	Public SharePercent As Double
	Public SumInsured As Decimal
	Public Premium As Decimal
	Public Tax As Decimal
	Public Commission As Decimal
	Public CommTax As Decimal
	
	'-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN66155)
	Private Const kBandTotalIndex As Integer = 0
	
	' Add a grid line into the totals
	Public Sub Add(ByVal xArray As XArrayHelper, ByVal lRow As Integer)
		
		' Add values from specified row to totals
        SharePercent += gPMFunctions.ToSafeDouble(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare))
        SumInsured += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured))
        Premium += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium))
        Tax += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRITax))
        Commission += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission))
        CommTax += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommTax))
    End Sub

    ' Combine totals with another set
    Public Sub Combine(ByVal oTotals As RiskTotalizer)

        SharePercent += oTotals.SharePercent
        SumInsured += oTotals.SumInsured
        Premium += oTotals.Premium
        Tax += oTotals.Tax
        Commission += oTotals.Commission
        CommTax += oTotals.CommTax

    End Sub

    ' Clear totals
    Public Sub Clear()
        ' Reset totals
        SharePercent = 0
        SumInsured = 0
        Premium = 0
        Tax = 0
        Commission = 0
        CommTax = 0
    End Sub

    ' Write totals back to grid
    Public Sub Store(ByVal xArray As XArrayHelper, ByVal lRow As Integer)

        ' Store summaries
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = SharePercent
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = SumInsured
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = Premium
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRITax) = Tax
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission) = Commission
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommTax) = CommTax
    End Sub
    'Start -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
    Public Sub StoreNetLine(ByVal xArray As XArrayHelper, ByVal lRow As Integer)

        ' Store summaries
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = 1 - SharePercent
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = xArray(kBandTotalIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) - SumInsured
        xArray(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = xArray(kBandTotalIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) - Premium
    End Sub
    'End -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
    Public Sub New()
        MyBase.New()
        ' Ensure totals are clear
        Clear()
    End Sub
End Class
