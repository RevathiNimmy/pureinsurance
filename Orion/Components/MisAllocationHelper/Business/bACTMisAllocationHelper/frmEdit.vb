Option Strict Off
Option Explicit On
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Friend Partial Class frmEdit
	Inherits System.Windows.Forms.Form
	
	Private m_cOriginalAmount As Decimal
	Private m_cAllocatedAmount As Decimal
	Private m_cOSAmount As Decimal
	
	Public Property OriginalAmount() As Decimal
		Get
			Return m_cOriginalAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cOriginalAmount = Value
		End Set
	End Property
	
	Public Property AllocatedAmount() As Decimal
		Get
			Return m_cAllocatedAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cAllocatedAmount = Value
		End Set
	End Property
	
	Public Property OSAmount() As Decimal
		Get
			Return m_cOSAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cOSAmount = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim dbNumericTemp As Double
		If Not Double.TryParse(txtAllocatedAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			MessageBox.Show("Allocated amount is not numeric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub
		End If
		
		m_cAllocatedAmount = CDec(txtAllocatedAmount.Text)
		
		Me.Close()
		
	End Sub
	

	Private Sub frmEdit_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		txtOriginalAmount.Text = CStr(m_cOriginalAmount)
		txtAllocatedAmount.Text = CStr(m_cAllocatedAmount)
		txtOSAmount.Text = CStr(m_cOSAmount)
	End Sub
End Class