Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports SharedFiles
Friend NotInheritable Class ClaimTotalizer 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "ClaimTotalizer"

	' ***************************************************************** '
	'                       PUBLIC PROPERTIES
	' ***************************************************************** '
	Public SharePercent As Double
	Public SumInsured As Decimal
	Public ReserveToDate As Decimal
	Public ThisReserve As Decimal
	Public PaymentToDate As Decimal
	Public ThisPayment As Decimal
    Public RecoveredToDate As Decimal
    Public Balance As Decimal
	
    Private Const kBandTotalIndex As Integer = 0
	' Add a grid line into the totals
    Public Sub Add(ByVal xArray As XArrayHelper, ByVal lRow As Long)

        ' Add values from specified row to totals
        SharePercent = SharePercent + ToSafeDouble(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare))
        SumInsured = SumInsured + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured))
        ReserveToDate = ReserveToDate + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate))
        ThisReserve = ThisReserve + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve))
        PaymentToDate = PaymentToDate + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate))
        RecoveredToDate = RecoveredToDate + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate))
        ThisPayment = ThisPayment + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment))
        Balance = Balance + ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance))
    End Sub

    ' Clear totals
    Public Sub Clear()
        ' Reset totals
        SharePercent = 0
        SumInsured = 0
        ReserveToDate = 0
        ThisReserve = 0
        PaymentToDate = 0
        RecoveredToDate = 0
        ThisPayment = 0
        Balance = 0
    End Sub

    ' Write totals back to grid
    Public Sub Store(ByVal xArray As XArrayHelper, ByVal lRow As Integer)
        ' Store summaries
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = SharePercent
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) = SumInsured
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate) = ReserveToDate
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) = ThisReserve
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate) = PaymentToDate
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) = ThisPayment
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) = Balance

    End Sub

    Public Sub StoreNetLine(ByVal xArray As XArrayHelper, ByVal lRow As Long)
        ' Store summaries
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = 1 - SharePercent
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) = xArray(kBandTotalIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) - SumInsured
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate) = xArray(kBandTotalIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate) - ReserveToDate
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) = xArray(kBandTotalIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) - ThisReserve
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate) = xArray(kBandTotalIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate) - PaymentToDate
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) = xArray(kBandTotalIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) - ThisPayment
        xArray(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) = xArray(kBandTotalIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) - Balance

    End Sub
    ' Combine totals with another set
    Public Sub Combine(ByVal oTotals As ClaimTotalizer)

        SharePercent += oTotals.SharePercent
        SumInsured += oTotals.SumInsured
        ReserveToDate += oTotals.ReserveToDate
        ThisReserve += oTotals.ThisReserve
        PaymentToDate += oTotals.PaymentToDate
        RecoveredToDate += oTotals.RecoveredToDate
        ThisPayment += oTotals.ThisPayment
        Balance += oTotals.Balance

    End Sub
    Public Sub New()
        MyBase.New()
        ' Ensure totals are clear
        Clear()
    End Sub
End Class
