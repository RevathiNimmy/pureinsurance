Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class TagEntry 
	
	Private m_sTag As String = ""
	Private m_sReplacement As String = ""
	
	Public Property Tag() As String
		Get
			Return m_sTag
		End Get
		Set(ByVal Value As String)
			m_sTag = Value
		End Set
	End Property
	
	Public Property Replacement() As String
		Get
			Return m_sReplacement
		End Get
		Set(ByVal Value As String)
			m_sReplacement = Value
		End Set
	End Property
End Class
