Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class cCapacityRange 
	Implements iRange
	
	Private Const ACClass As String = "cCapacityRange"
	
	
	Private m_lStart As Integer
	Private m_lEnd As Integer
	
	Private Const acRangeTollerance As Integer = 50
	
	Private WriteOnly Property iRange_EndValue() As Integer Implements iRange.EndValue
		Set(ByVal Value As Integer)
			m_lEnd = Value + acRangeTollerance
		End Set
	End Property
	
	Private WriteOnly Property iRange_StartValue() As Integer Implements iRange.StartValue
		Set(ByVal Value As Integer)
			m_lStart = Value - acRangeTollerance
		End Set
	End Property
	
	Private Function iRange_Includes(ByVal v_Value As Integer) As Boolean Implements iRange.Includes
		
		Dim result As Boolean = False
		 
			
			If v_Value > m_lStart And v_Value < m_lEnd Then
				result = True
			End If
			
			Return result
		
	End Function
End Class

