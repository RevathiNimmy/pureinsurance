Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

Friend NotInheritable Class Reinsurance
    ' ***************************************************************** '
    ' Class Name: Reinsurance
    '
    ' Date: 05/05/1997
    '
    ' Description: Describes the Reinsurance attributes.
    '
    ' Edit History:
    '   Peter Finney - 11/06/2003
    '       Simplified all declarations and set correct datatypes for
    '       currency fields
    '   Peter Finney - 08/09/2005
    '       Simplified entire class for new simple ri structure
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Reinsurance"

    ' Update Status
    Public DatabaseStatus As gPMConstants.PMEComponentAction

    ' RI Band Attributes
    Public RIBand As Integer
    Public RIModelID As Integer
    Public ArrangementID As Integer

    ' Main reinsurance details
    Public SumInsured As Decimal
    Public Premium As Decimal
    Public RILines(,) As Object
    Public FacPremiumMethod As Integer

    ' Original reinsurance details
    Public OriginalSumInsured As Decimal
    Public OriginalPremium As Decimal
    Public OriginalRILines(,) As Object
    'Extended limits
    Public IsExtendedLimitApplied As Boolean
    Public ExtendedLimitAmount As Decimal

    ' E007
    Public XOLRIModelId As Long

    ' ***************************************************************** '
    ' Rollup Values for current Treaty
    ' ***************************************************************** '
    Public ReadOnly Property TotalCommission() As Decimal
        Get

            Dim cCommission As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cCommission += CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount))
                Next
            End If

            ' Return total
            Return cCommission
        End Get
    End Property

    Public ReadOnly Property TotalCommissionTax() As Decimal
        Get

            Dim cCommissionTax As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cCommissionTax += CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007CommissionTax, lCount))
                Next
            End If

            ' Return total
            Return cCommissionTax
        End Get
    End Property

    Public ReadOnly Property TotalPremium() As Decimal
        Get

            Dim cPremium As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cPremium += CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount))
                Next
            End If

            ' Return total
            Return cPremium
        End Get
    End Property

    Public ReadOnly Property TotalPremiumTax() As Decimal
        Get

            Dim cPremiumTax As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cPremiumTax += CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007PremiumTax, lCount))
                Next
            End If

            ' Return total
            Return cPremiumTax
        End Get
    End Property


    Public ReadOnly Property TotalSumInsured() As Decimal
        Get

            Dim cSumInsured As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cSumInsured += CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount))
                Next
            End If

            ' Return total
            Return cSumInsured
        End Get
    End Property

    Public ReadOnly Property TotalSharePercent() As Double
        Get
            ' Calculate this value based on totals not rollup. This will ensure proper values
            If SumInsured = 0 Then
                Return 100
            Else
                Return (TotalSumInsured / SumInsured) * 100
            End If
        End Get
    End Property


    ' ***************************************************************** '
    ' Rollup Values for original Treaty
    ' ***************************************************************** '
    Public ReadOnly Property OriginalTotalCommission() As Decimal
        Get

            Dim cCommission As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(OriginalRILines) Then
                For lCount As Integer = OriginalRILines.GetLowerBound(1) To OriginalRILines.GetUpperBound(1)
                    cCommission += CDbl(OriginalRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Commission, lCount))
                Next
            End If

            ' Return total
            Return cCommission
        End Get
    End Property

    Public ReadOnly Property OriginalTotalPremium() As Decimal
        Get

            Dim cPremium As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(OriginalRILines) Then
                For lCount As Integer = OriginalRILines.GetLowerBound(1) To OriginalRILines.GetUpperBound(1)
                    cPremium += CDbl(OriginalRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lCount))
                Next
            End If

            ' Return total
            Return cPremium
        End Get
    End Property

    Public ReadOnly Property OriginalTotalSumInsured() As Decimal
        Get

            Dim cSumInsured As Decimal

            ' If we have an array add up our values
            If Informations.IsArray(OriginalRILines) Then
                For lCount As Integer = OriginalRILines.GetLowerBound(1) To OriginalRILines.GetUpperBound(1)
                    cSumInsured += CDbl(OriginalRILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lCount))
                Next
            End If

            ' Return total
            Return cSumInsured
        End Get
    End Property

    Public ReadOnly Property OriginalTotalSharePercent() As Double
        Get
            ' Calculate this value based on totals not rollup. This will ensure proper values
            If OriginalSumInsured = 0 Then
                Return 100
            Else
                Return (OriginalTotalSumInsured / OriginalSumInsured) * 100
            End If
        End Get
    End Property



    ' ***************************************************************** '
    ' Rounds any minor discrepencies to allow 100% allocation of RI
    ' ***************************************************************** '
    Public Function Round() As Integer

        Dim result As Integer = 0
        Const Tolerance As Double = 0.01

        Dim lRounding As Integer
        Dim cDiff As Decimal

        Const kMethodName As String = "Round"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have no treaty array we have nothing to do!
            If Not Informations.IsArray(RILines) Then
                Return result
            End If

            ' Initialise rows
            lRounding = -1

            ' Find best lines for allocating rounding amounts
            For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                Select Case RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Type, lCount)
                    Case "R"
                        ' First ret row is ideal, set rounding row and exit loop
                        lRounding = lCount
                        Exit For
                    Case "T"
                        ' Keep track of first treaty in case no ret row is found
                        lRounding = Math.Max(lCount, lRounding)
                    Case "FX", "TX", "F"
                        lRounding = lCount
                End Select
            Next lCount

            ' Check for tiny allocation discrepencies and sneak it into our rounding line
            cDiff = SumInsured - TotalSumInsured
            If Math.Abs(cDiff) < Tolerance Then
                RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lRounding) = CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007SumInsured, lRounding)) + cDiff
            End If

            ' Premium juggling, only if sum insured is correct
            If SumInsured = TotalSumInsured Then
                cDiff = Premium - TotalPremium
                If Math.Abs(cDiff) < Tolerance Then
                    RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lRounding) = CDbl(RILines(MainModule.RIArrangementLineEnumRI2007.DBRIL2007Premium, lRounding)) + cDiff
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
End Class
