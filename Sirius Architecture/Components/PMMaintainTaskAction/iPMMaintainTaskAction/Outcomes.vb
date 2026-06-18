Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class OutcomeData 
	
	Private m_lTaskOutcomeId As Integer
	
	'****************
	
	
	Public Property TaskOutcomeId() As Integer
		Get
			Return m_lTaskOutcomeId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskOutcomeId = Value
		End Set
	End Property
	
	'****************
End Class
