Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmAddParty
    Inherits System.Windows.Forms.Form
	Private m_sFullName As String = ""
	Private m_sAddress1 As String = ""
	Private m_sAddress2 As String = ""
	Private m_sAddress3 As String = ""
	Private m_sAddress4 As String = ""
	Private m_sPostCode As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	
	Public Property FullName() As String
		Get
			Return m_sFullName
		End Get
		Set(ByVal Value As String)
			m_sFullName = Value
		End Set
	End Property
	
	
	Public Property Address1() As String
		Get
			Return m_sAddress1
		End Get
		Set(ByVal Value As String)
			m_sAddress1 = Value
		End Set
	End Property
	
	
	Public Property Address2() As String
		Get
			Return m_sAddress2
		End Get
		Set(ByVal Value As String)
			m_sAddress2 = Value
		End Set
	End Property
	
	
	Public Property Address3() As String
		Get
			Return m_sAddress3
		End Get
		Set(ByVal Value As String)
			m_sAddress3 = Value
		End Set
	End Property
	
	
	Public Property Address4() As String
		Get
			Return m_sAddress4
		End Get
		Set(ByVal Value As String)
			m_sAddress4 = Value
		End Set
	End Property
	
	
	Public Property PostCode() As String
		Get
			Return m_sPostCode
		End Get
		Set(ByVal Value As String)
			m_sPostCode = Value
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		If txtFullName.Text.Trim() <> "" Then
			If uctAddress.AddressLine1.Trim() <> "" And uctAddress.PostCode.Trim() <> "" Then
				
				m_sFullName = txtFullName.Text
				m_sAddress1 = uctAddress.AddressLine1
				m_sAddress2 = uctAddress.AddressLine2
				m_sAddress3 = uctAddress.AddressLine3
				m_sAddress4 = uctAddress.AddressLine4
				m_sPostCode = uctAddress.PostCode
				
				m_lStatus = gPMConstants.PMEReturnCode.PMOK
				
				Hide()
			End If
		End If
	End Sub
End Class