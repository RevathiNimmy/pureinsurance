Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports SharedFiles
Friend NotInheritable Class RiskTotalizer2007 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "RiskTotalizer2007"
	
    ' ***************************************************************** '
    '                       PUBLIC PROPERTIES
    ' ***************************************************************** '
	Public SharePercent As Double
	Public SumInsured As Decimal
	Public Premium As Decimal
	Public Tax As Decimal
	Public Commission As Decimal
	Public CommTax As Decimal
	
	' Add a grid line into the totals
	Public Sub Add(ByVal xArray As XArrayHelper, ByVal lRow As Integer)
		
		' Add values from specified row to totals
        SharePercent += gPMFunctions.ToSafeDouble(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
        SumInsured += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured))
        Premium += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium))
        Tax += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax))
        Commission += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission))
        CommTax += gPMFunctions.ToSafeCurrency(xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax))
    End Sub

    ' Combine totals with another set
    Public Sub Combine(ByVal oTotals As RiskTotalizer2007)

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

        If lRow = -1 Then
            Exit Sub
        End If

        If xArray.Rows.Count - 1 >= lRow Then
            ' Store summaries
            If xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then
                xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = ""
            ElseIf xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) <> "T" Then
                xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = SharePercent
            End If
            xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = SumInsured
            xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Premium
            xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = Tax
            xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = Commission
            xArray(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = CommTax
        End If
    End Sub

    Public Sub New()
        MyBase.New()
        ' Ensure totals are clear
        Clear()
    End Sub
End Class
