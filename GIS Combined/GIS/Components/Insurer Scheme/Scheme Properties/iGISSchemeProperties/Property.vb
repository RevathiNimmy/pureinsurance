Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class Property_Renamed 
	' ***************************************************************** '
	' Class Name: Property
	'
	' Date:  11/06/1999
	'
	' Description: Contains a collection of param objects.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "Property"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	Private m_iRequired As Integer
	
	' PRIVATE Data Members (End)
	
	' PUBLIC Properties (Begin)
	
	
	Public Property Required() As Integer
		Get
			
			Return m_iRequired
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iRequired = Value
			
		End Set
	End Property
	
	' PUBLIC Properties (End)
	
	' PUBLIC Methods (Begin)
	
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	' PRIVATE Methods (End)
	Private Shared _DefaultInstance As Property_Renamed = Nothing
	Public Shared ReadOnly Property DefaultInstance() As Property_Renamed
		Get
			If _DefaultInstance Is Nothing Then
				_DefaultInstance = New Property_Renamed
			End If
			Return _DefaultInstance
		End Get
	End Property
End Class
