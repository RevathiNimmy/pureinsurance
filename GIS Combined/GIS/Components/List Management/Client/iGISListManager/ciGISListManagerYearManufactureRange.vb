Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class cYearManufactureRange 
	Implements iRange
	
	
	Private lYearFrom As Integer
	Private lYearTo As Integer
	
	Private WriteOnly Property iRange_EndValue() As Integer Implements iRange.EndValue
		Set(ByVal Value As Integer)
			lYearTo = Value
		End Set
	End Property
	
	Private WriteOnly Property iRange_StartValue() As Integer Implements iRange.StartValue
		Set(ByVal Value As Integer)
			lYearFrom = Value
		End Set
	End Property
	
	Private Function iRange_Includes(ByVal v_Value As Integer) As Boolean Implements iRange.Includes
		
		Dim result As Boolean = False
		If ((v_Value >= lYearFrom And v_Value <= lYearTo) Or (lYearFrom = 0 And v_Value <= lYearTo) Or (v_Value >= lYearFrom And lYearTo = 0) Or (lYearFrom = 0 And lYearTo = 0)) Or ((v_Value + 1 >= lYearFrom And v_Value + 1 <= lYearTo) Or (lYearFrom = 0 And v_Value + 1 <= lYearTo) Or (v_Value + 1 >= lYearFrom And lYearTo = 0) Or (lYearFrom = 0 And lYearTo = 0)) Or ((v_Value - 1 >= lYearFrom And v_Value - 1 <= lYearTo) Or (lYearFrom = 0 And v_Value - 1 <= lYearTo) Or (v_Value - 1 >= lYearFrom And lYearTo = 0) Or (lYearFrom = 0 And lYearTo = 0)) Then
			
			result = True
			
		End If
		
		Return result
	End Function
End Class
