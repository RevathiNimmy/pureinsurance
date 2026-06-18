Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports SharedFiles
Friend NotInheritable Class ClaimTotalizerRI2007 
	
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
	Public Balance As Decimal
    Public RecoveredToDate As Decimal
    Public Incurred As Decimal
    Public IncurredToDate As Decimal
	' Add a grid line into the totals
    Public Sub Add(ByVal xArray As XArrayHelper, ByVal lRow As Integer, Optional ByVal bRecovery As Boolean = False)

        ' Add values from specified row to totals
        SharePercent += gPMFunctions.ToSafeDouble(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare))
        SumInsured += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured))
        ReserveToDate += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate))
        ThisReserve += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve))
        PaymentToDate += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate))
        RecoveredToDate += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate))
        ThisPayment += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment))
        Incurred += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred))
        IncurredToDate += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IncurredToDate))
        If Not bRecovery Then
            Balance += Math.Round(gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate)), 4) + Math.Round(gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve)), 4) - Math.Round(gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)), 4) - Math.Round(gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate)), 4)
        Else
            Balance += gPMFunctions.ToSafeCurrency(xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance))
        End If
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
        Incurred = 0
        IncurredToDate = 0
    End Sub

    ' Write totals back to grid
    Public Sub Store(ByVal xArray As XArrayHelper, ByVal lRow As Integer)
     
        If xArray.Rows.Count - 1 >= lRow Then
            ' Store summaries
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = SharePercent
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = SumInsured
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = ReserveToDate
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = ThisReserve
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = PaymentToDate
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = RecoveredToDate
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = ThisPayment
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = Balance
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = Incurred
            xArray(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IncurredToDate) = IncurredToDate
        End If
    End Sub

    Public Sub New()
        MyBase.New()
        ' Ensure totals are clear
        Clear()
    End Sub
End Class
