Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class ActionType 
	
	Private m_lTaskActionTypeId As Integer
	
	'****************
	
	
	Public Property TaskActionTypeId() As Integer
		Get
			Return m_lTaskActionTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskActionTypeId = Value
		End Set
	End Property
	
	'****************
End Class
