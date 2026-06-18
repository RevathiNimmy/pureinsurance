Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend NotInheritable Class PMNavField 
	' ***************************************************************** '
	' Class Name: PMNavField
	'
	' Date: 04/01/1999
	'
	' Description: Describes the PMNavField attributes.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "PMNavField"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	'Field name
	Private m_sName As String = ""
	
	'Field value
	Private m_vValue As String = ""
	
	'Field validation
	Private m_iValidation As Integer
	
	'Field Mandatory
	Private m_iMandatory As Integer
	
	'Field Length
	Private m_iLength As Integer
	
	'The interface control tied to this field
	Private m_InterfaceControl As Control
	
	Public Property InterfaceControl() As Control
		Get
			
			Return m_InterfaceControl
			
		End Get
		Set(ByVal Value As Control)
			m_InterfaceControl = Value
		End Set
	End Property
	
	
	Public Property Length() As Integer
		Get
			
			Return m_iLength
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iLength = Value
			
		End Set
	End Property
	
	
	Public Property Name() As String
		Get
			
			Return m_sName
			
		End Get
		Set(ByVal Value As String)
			m_sName = Value
		End Set
	End Property
	
	
	Public Property Value() As String
		Get
			Return m_vValue
		End Get
		Set(ByVal Value As String)

			m_vValue = CStr(Value)
		End Set
	End Property
	
	
	Public Property Mandatory() As Integer
		Get
			Return m_iMandatory
		End Get
		Set(ByVal Value As Integer)
			m_iMandatory = Value
		End Set
	End Property
	
	
	Public Property Validation() As Integer
		Get
			Return m_iValidation
		End Get
		Set(ByVal Value As Integer)
			m_iValidation = Value
		End Set
	End Property
End Class
