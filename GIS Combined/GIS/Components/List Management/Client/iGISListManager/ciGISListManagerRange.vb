Option Strict Off
Option Explicit On
Imports System
Interface iRange
	WriteOnly Property StartValue As Integer
	WriteOnly Property EndValue As Integer
	Function Includes(ByVal v_Value As Integer) As Boolean
End Interface
Friend NotInheritable Class iRange_CoClass 
	Implements iRange
	
	Public WriteOnly Property StartValue() As Integer Implements iRange.StartValue
		Set(ByVal Value As Integer)
			
		End Set
	End Property
	
	Public WriteOnly Property EndValue() As Integer Implements iRange.EndValue
		Set(ByVal Value As Integer)
			
		End Set
	End Property
	
	Public Function Includes(ByVal v_Value As Integer) As Boolean Implements iRange.Includes
		
	End Function
End Class
