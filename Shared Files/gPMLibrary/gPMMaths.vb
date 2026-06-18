Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
<System.Runtime.InteropServices.ProgId("gPMMaths_NET.gPMMaths")> _
 Public Module gPMMaths
    Private m_vDecWorkingValue1 As Decimal
	Private m_vDecWorkingValue2 As Decimal
	Private m_vDecWorkingValue3 As Decimal
	Private m_vDecWorkingValue4 As Decimal
	Private m_vDecWorkingValue5 As Decimal

	Private m_lReturn As Integer
	
	Private Property DecWorkingValue1() As Double
		Get
			Return m_vDecWorkingValue1
		End Get
		Set(ByVal Value As Double)

			m_vDecWorkingValue1 = CDec(Value)
		End Set
	End Property

	Private Property DecWorkingValue2() As Double
		Get
			Return m_vDecWorkingValue2
		End Get
		Set(ByVal Value As Double)

			m_vDecWorkingValue2 = CDec(Value)
		End Set
	End Property
	
	
	Private Property DecWorkingValue3() As Decimal
		Get
			Return m_vDecWorkingValue3
		End Get
		Set(ByVal Value As Decimal)

			m_vDecWorkingValue3 = CDec(Value)
		End Set
	End Property
	
	
	Private Property DecWorkingValue4() As Decimal
		Get
			Return m_vDecWorkingValue4
		End Get
		Set(ByVal Value As Decimal)

			m_vDecWorkingValue4 = CDec(Value)
		End Set
	End Property
	
	
	Private Property DecWorkingValue5() As Double
		Get
			Return m_vDecWorkingValue5
		End Get
		Set(ByVal Value As Double)

			m_vDecWorkingValue5 = CDec(Value)
		End Set
	End Property
	
	Public Function PMTruncateVDecimal(ByVal v_vWholeValue As Double, ByVal v_eNumberOfDP As gPMConstants.PMEVDecimalNoOfDP) As Decimal
        Dim sValueString As String = ""
		Dim lDPPosition As Integer
		
		Try 
            ' assign working value to member variable
			DecWorkingValue1 = v_vWholeValue
			
			' check number of decimal places is valid
			If (v_eNumberOfDP < gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPZero) Then v_eNumberOfDP = gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPZero
			If (v_eNumberOfDP > gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPSix) Then v_eNumberOfDP = gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPSix
			
			' Convert value to string
			sValueString = Conversion.Str(DecWorkingValue1)

			' Add dp if not present
			If (sValueString.IndexOf("."c) + 1) = 0 Then
				sValueString = sValueString & "."
			End If
			
			' Right justify string with zeros
			sValueString = sValueString & "000000"
			
			' Truncate string to required number of dp
			lDPPosition = (sValueString.IndexOf("."c) + 1)
			sValueString = sValueString.Substring(0, Math.Min(sValueString.Length, lDPPosition + v_eNumberOfDP))
			
			' Convert string to numeric 4dp
			sValueString = sValueString.TrimEnd()
			
			' set the returning parameter
			
            Return ToSafeDecimal(sValueString)
		
		Catch excep As System.Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Function
        End Try
	End Function
	
	' ***************************************************************** '
	' Name: PMRoundupValueVDecimal
	'
	' Description: Rounds up a given value, using supplied factor
	'
    ' ***************************************************************** '

	Public Function PMRoundupValueVDecimal(ByVal v_vWholeValue As Double, ByVal v_eNumberOfDP As gPMConstants.PMEVDecimalNoOfDP, ByVal v_eRoundingFactor As gPMConstants.PMERoundupFactor) As Decimal
        Try
            ' assign working value to member variable
            DecWorkingValue2 = v_vWholeValue

            ' check number of decimal places is valid
            If (v_eNumberOfDP < gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPZero) Then v_eNumberOfDP = gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPZero
            If (v_eNumberOfDP > gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPSix) Then v_eNumberOfDP = gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPSix

            ' Validate rounding factor
            If Not (v_eRoundingFactor > gPMConstants.PMERoundupFactor.pmeRFactor00Up) And Not (v_eRoundingFactor < gPMConstants.PMERoundupFactor.pmeRFactor99Up) Then
                v_eRoundingFactor = gPMConstants.PMERoundupFactor.pmeRFactor00Up
            End If

            ' Perform rounding if required
            If v_eRoundingFactor > 0 Then

                ' Calculate rounding factor to 8 dp
                Select Case v_eNumberOfDP
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPZero
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 2
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPOne
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 3
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPTwo
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 4
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPThree
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 5
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPFour
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 6
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPFive
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 7
                    Case gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPFour
                        DecWorkingValue3 = v_eRoundingFactor / 10 ^ 8

                End Select

                ' Add to original number (or subtract if negative)
                If DecWorkingValue2 < 0 Then
                    DecWorkingValue2 -= DecWorkingValue3
                Else
                    DecWorkingValue2 += DecWorkingValue3
                End If

            End If

            ' Truncate to number_of_dp
            If v_vWholeValue = 0 Then
                Return 0
            Else
                Return PMTruncateVDecimal(v_vWholeValue:=DecWorkingValue2, v_eNumberOfDP:=v_eNumberOfDP)
            End If
        Catch excep As System.Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Function
        End Try
	End Function
	
	' ***************************************************************** '
	' Name: PMCalcPercentValueVDecimal
	'
	' Description: Calculates the value of a percentage of a given value
	'
	' ***************************************************************** '
	Public Function PMCalcPercentValueVDecimal(ByVal v_vTotalValue As Decimal, ByVal v_vPercentage As Object, ByVal v_eNumberOfDP As gPMConstants.PMEVDecimalNoOfDP, ByVal v_eRoundingFactor As gPMConstants.PMERoundupFactor) As Decimal
        Dim sValueString As String = ""
		
		Try 
            ' assign working value to member variable
			DecWorkingValue4 = v_vTotalValue
            ' Perform calculation to 8dp
            DecWorkingValue5 = (DecWorkingValue4 * CDec(v_vPercentage)) / 100
			
			' Process rounding/truncation
            Return PMRoundupValueVDecimal(v_vWholeValue:=DecWorkingValue5, v_eNumberOfDP:=v_eNumberOfDP, v_eRoundingFactor:=v_eRoundingFactor)
        Catch excep As System.Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Function
        End Try
	End Function
	
    ' ***************************************************************** '
    ' Name: PMTruncateCurrency
    '
    ' Description: Truncates given string to given number of DPs
    '
    ' ***************************************************************** '
	Public Function PMTruncateCurrency(ByVal v_vWholeValue As Decimal, ByVal v_eNumberOfDP As gPMConstants.PMECurrencyNoOfDP) As Decimal
		
		Dim sValueString As String = ""
		Dim lDPPosition As Integer
		Dim cCurWorkingValue1 As Decimal
		
		Try 

			' assign the passed value to currency datatyped variable
			cCurWorkingValue1 = v_vWholeValue
			
			' check number of decimal places is valid
			If (v_eNumberOfDP < gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero) Then v_eNumberOfDP = gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero
			If (v_eNumberOfDP > gPMConstants.PMECurrencyNoOfDP.pmeCurDPFour) Then v_eNumberOfDP = gPMConstants.PMECurrencyNoOfDP.pmeCurDPFour
			
			' Convert value to string
			sValueString = Conversion.Str(cCurWorkingValue1)
			
			' Add dp if not present
			If (sValueString.IndexOf("."c) + 1) = 0 Then
				sValueString = sValueString & "."
			End If

			' Right justify string with zeros
			sValueString = sValueString & "000000"
			
			' Truncate string to required number of dp
			lDPPosition = (sValueString.IndexOf("."c) + 1)
			sValueString = sValueString.Substring(0, Math.Min(sValueString.Length, lDPPosition + v_eNumberOfDP))
			
			' Convert string to numeric 4dp
            sValueString = sValueString.TrimEnd()

            If sValueString.Trim() = "." OrElse sValueString.Trim() = "-." Then
                sValueString = "0"
            End If

            ' set the returning parameter


            Return CDec(sValueString)
        Catch excep As System.Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Function
        End Try
	End Function
	
	' ***************************************************************** '
	' Name: PMRoundupValueCurrency
	'
	' Description: Rounds up a given value, using supplied factor
	'
	' ***************************************************************** '
	Public Function PMRoundupValueCurrency(ByVal v_vWholeValue As Decimal, ByVal v_eNumberOfDP As gPMConstants.PMECurrencyNoOfDP, ByVal v_eRoundingFactor As gPMConstants.PMERoundupFactor) As Decimal
        Dim result As Decimal = 0
		Dim vCurWorkingValueDEC1 As Decimal
		Dim vCurWorkingValueDEC2 As Decimal
		
		Try 
            ' assign value to member variable
			vCurWorkingValueDEC2 = v_vWholeValue
			
			' check number of decimal places is valid
			If (v_eNumberOfDP < gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero) Then v_eNumberOfDP = gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero
			If (v_eNumberOfDP > gPMConstants.PMECurrencyNoOfDP.pmeCurDPFour) Then v_eNumberOfDP = gPMConstants.PMECurrencyNoOfDP.pmeCurDPFour
			
			' Validate rounding factor
			If Not (v_eRoundingFactor > 0) And Not (v_eRoundingFactor < 99) Then
				v_eRoundingFactor = gPMConstants.PMERoundupFactor.pmeRFactor00Up
			End If
			
			' Perform rounding if required
			If v_eRoundingFactor > 0 Then
				
				' Calculate rounding factor to 8 dp
                Select Case v_eNumberOfDP
                    Case gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero
                        vCurWorkingValueDEC1 = v_eRoundingFactor / 10 ^ 2
                    Case gPMConstants.PMECurrencyNoOfDP.pmeCurDPOne
                        vCurWorkingValueDEC1 = v_eRoundingFactor / 10 ^ 3
                    Case gPMConstants.PMECurrencyNoOfDP.pmeCurDPTwo
                        vCurWorkingValueDEC1 = v_eRoundingFactor / 10 ^ 4
                    Case gPMConstants.PMECurrencyNoOfDP.pmeCurDPThree
                        vCurWorkingValueDEC1 = v_eRoundingFactor / 10 ^ 5
                    Case gPMConstants.PMECurrencyNoOfDP.pmeCurDPFour
                        vCurWorkingValueDEC1 = v_eRoundingFactor / 10 ^ 6
                End Select
				
				' Add to original number (or subtract if negative)
				If vCurWorkingValueDEC2 < 0 Then
					vCurWorkingValueDEC2 -= vCurWorkingValueDEC1
				Else
					vCurWorkingValueDEC2 += vCurWorkingValueDEC1
				End If
            End If
			
			' Truncate to number_of_dp
			result = PMTruncateCurrency(v_vWholeValue:=vCurWorkingValueDEC2, v_eNumberOfDP:=v_eNumberOfDP)
            Return result
        Catch excep As System.Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Return result
        End Try
	End Function
	
	' ***************************************************************** '
	' Name: PMCalcPercentValueCurrency
	'
	' Description: Calculates the value of a percentage of a given value
	'
	' ***************************************************************** '
	Public Function PMCalcPercentValueCurrency(ByVal v_vTotalValue As Object, ByVal v_vPercentage As Object, ByVal v_eNumberOfDP As gPMConstants.PMECurrencyNoOfDP, ByVal v_eRoundingFactor As gPMConstants.PMERoundupFactor) As Decimal
        Dim vCurWorkingValueDEC1 As Decimal
		Dim vCurWorkingValueDEC2 As Double
		Dim sValueString As String = ""
		
        Try
            vCurWorkingValueDEC1 = CDec(v_vTotalValue)
            ' Perform calculation to 8dp

            vCurWorkingValueDEC2 = (vCurWorkingValueDEC1 * CDec(v_vPercentage)) / 100

            ' Process rounding/truncation

            Return PMRoundupValueCurrency(v_vWholeValue:=vCurWorkingValueDEC2, v_eNumberOfDP:=v_eNumberOfDP, v_eRoundingFactor:=v_eRoundingFactor)
        Catch excep As System.Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
            Exit Function
        End Try
	End Function
	
    ' ***************************************************************** '
    ' Returns the smallest integer greater than, or equal to, the given
    ' numeric expression.
    '
    ' Note: Return value conforms to SQL Server Ceiling function.
    ' ***************************************************************** '
	Public Function Ceiling(ByVal Expression As Double) As Double
		' Only process numeric values
		Dim result As Double = 0
		Dim dbNumericTemp As Double
		If Double.TryParse(CStr(Expression), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			result = IIf(Expression \ 1 = Expression, Expression, Math.Floor(Expression) + 1)
		Else
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", Ceiling, Expression must be numeric")
		End If
		Return result
	End Function
	
	' ***************************************************************** '
	' Returns the largest integer less than or equal to the given numeric
	' expression.
	'
	' Note: Return value conforms to SQL Server Floor function.
	' ***************************************************************** '
	Public Function Floor(ByVal Expression As Double) As Double
		' Only process numeric values
		Dim result As Double = 0
		Dim dbNumericTemp As Double
		If Double.TryParse(CStr(Expression), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			' Equivalent to Int() function but included for completeness
			result = Math.Floor(Expression)
		Else
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", Floor, Expression must be numeric")
		End If
		Return result
	End Function
End Module