Option Strict Off
Option Explicit On
Imports System

<System.Runtime.InteropServices.ProgId("cClaimDetails_NET.cClaimDetails")> _
Public NotInheritable Class cClaimDetails 
	
	Private m_vClaimVersionDetails As Object
	Private m_sClaimDescription As String = ""
	Private m_sInsuranceRef As String = ""
	Private m_sCaseNumber As String = ""
	
	
	Public Property ClaimVersionDetails() As Object
		Get
			Return m_vClaimVersionDetails
		End Get
		Set(ByVal Value As Object)


			m_vClaimVersionDetails = Value
		End Set
	End Property
	
	
	Public Property ClaimDescription() As String
		Get
			Return m_sClaimDescription
		End Get
		Set(ByVal Value As String)
			m_sClaimDescription = Value
		End Set
	End Property
	
	
	Public Property InsuranceRef() As String
		Get
			Return m_sInsuranceRef
		End Get
		Set(ByVal Value As String)
			m_sInsuranceRef = Value
		End Set
	End Property
	
	Public Property CaseNumber() As String
		Get
			Return m_sCaseNumber
		End Get
		Set(ByVal Value As String)
			m_sCaseNumber = Value
		End Set
	End Property
	
	Friend Sub New()
		MyBase.New()
		
		' Create an instance of the object manager.
        'g_oObjectManager = New bObjectManager.ObjectManager()
		
	End Sub
End Class
