Option Strict Off
Option Explicit On
'Developer Guide no. 129
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
    Public ReserveToDate As Decimal
    Public PaymentToDate As Decimal
    Public ThisReserve As Decimal
    Public ThisPayment As Decimal
    Public CatastropheCodeID As Integer

    ' XOL triggers
    Public XolClmModelID As Integer
    Public XolClmLimit As Decimal
    Public XolCatModelID As Integer
    Public XolCatLimit As Decimal
    Public XolCatReinstatements As Integer

    ' Reinsurance lines
    Public RILines(,) As Object
    Public XOLArrangement As Object


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


    ' ***************************************************************** '
    ' Rollup Values for current Treaty
    ' ***************************************************************** '
    Public ReadOnly Property TotalThisPayment() As Decimal
        Get

            Dim cPaymentValue As Decimal

            ' If we have a treaty array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cPaymentValue += CDbl(RILines(MainModule.RIArrangementLineEnum.DBCRILThisPayment, lCount))
                Next
            End If

            ' Return total
            Return cPaymentValue
        End Get
    End Property

    Public ReadOnly Property TotalThisReserve() As Decimal
        Get

            Dim cReserveValue As Decimal

            ' If we have a treaty array add up our values
            If Informations.IsArray(RILines) Then
                For lCount As Integer = RILines.GetLowerBound(1) To RILines.GetUpperBound(1)
                    cReserveValue += CDbl(RILines(MainModule.RIArrangementLineEnum.DBCRILThisReserve, lCount))
                Next
            End If

            ' Return total
            Return cReserveValue
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
                Select Case RILines(MainModule.RIArrangementLineEnum.DBCRILType, lCount)
                    Case "R"
                        ' First ret row is ideal, set rounding row and exit loop
                        lRounding = lCount
                        Exit For
                    Case "T"
                        ' Keep track of first treaty in case no ret row is found
                        If lRounding >= lCount Then
                            lRounding = lCount
                        End If

                End Select
            Next lCount

            ' Check for tiny allocation discrepencies and sneak it into our rounding line
            cDiff = ThisReserve - TotalThisReserve
            If Math.Abs(cDiff) < Tolerance Then
                RILines(MainModule.RIArrangementLineEnum.DBCRILThisReserve, lRounding) = CDbl(RILines(MainModule.RIArrangementLineEnum.DBCRILThisReserve, lRounding)) + cDiff
            End If

            ' Check payment rounding
            cDiff = ThisPayment - TotalThisPayment
            If Math.Abs(cDiff) < Tolerance Then
                RILines(MainModule.RIArrangementLineEnum.DBCRILThisPayment, lRounding) = CDbl(RILines(MainModule.RIArrangementLineEnum.DBCRILThisPayment, lRounding)) + cDiff
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
