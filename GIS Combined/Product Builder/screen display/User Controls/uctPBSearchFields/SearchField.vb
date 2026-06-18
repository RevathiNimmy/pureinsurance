Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("SearchField_NET.SearchField")> _
Public NotInheritable Class SearchField 
	Private m_lGISObjectID As Integer
	Private m_lGISPropertyID As Integer
	Private m_vSearchValue As Object
	
	
	Public Property GISObjectID() As Integer
		Get
			Return m_lGISObjectID
		End Get
		Set(ByVal Value As Integer)
			m_lGISObjectID = Value
		End Set
	End Property
	
	
	Public Property GISPropertyID() As Integer
		Get
			Return m_lGISPropertyID
		End Get
		Set(ByVal Value As Integer)
			m_lGISPropertyID = Value
		End Set
	End Property
	
	
	Public Property SearchValue() As Object
		Get
			
		End Get
		Set(ByVal Value As Object)
			
		End Set
	End Property
End Class
