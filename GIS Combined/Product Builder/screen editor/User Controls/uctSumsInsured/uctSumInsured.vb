Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctSumInsured_NET.uctSumInsured")> _
Public Partial Class uctSumInsured
	Inherits System.Windows.Forms.UserControl
	
	Private m_bShowRateAndPremium As Boolean
	Private m_bShowValuation As Boolean
	
	Private m_lMinimumWidth As Integer
	Private m_lMinimumHeight As Integer
	
	<Browsable(True)> _
	Public Property MinimumWidth() As Integer
		Get
			Return m_lMinimumWidth
		End Get
		Set(ByVal Value As Integer)
			m_lMinimumWidth = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property MinimumHeight() As Integer
		Get
			Return m_lMinimumHeight
		End Get
		Set(ByVal Value As Integer)
			m_lMinimumHeight = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property ShowRateAndPremium() As Boolean
		Get
			Return m_bShowRateAndPremium
		End Get
		Set(ByVal Value As Boolean)
			
			m_bShowRateAndPremium = Value
			
			lblRate.Visible = m_bShowRateAndPremium
			txtRate.Visible = m_bShowRateAndPremium
			lblPremium.Visible = m_bShowRateAndPremium
			pnlPremium.Visible = m_bShowRateAndPremium
			
			uctSumInsured_Resize(Me, New EventArgs())
			
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property ShowValuation() As Boolean
		Get
			Return m_bShowValuation
		End Get
		Set(ByVal Value As Boolean)
			
			m_bShowValuation = Value
			
			ValuationColumns()
			
		End Set
	End Property
	
	' ***************************************************************** '
	'
	' Name: ValuationColumns
	'
	' Description:
	'
	' History: 05/07/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Private Sub ValuationColumns()
		
		Dim lWidth As Integer
		
		Try 
			
			If m_bShowValuation Then
				lWidth = 1440
			End If
			
			lvwSumInsured.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(lWidth))
			lvwSumInsured.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(lWidth))
			
			lvwSumInsured.Refresh()
		
		Catch 
			
			
			
			
			' Log Error Message
			'    LogMessage _
			'iType:=PMLogOnError, _
			'sMsg:="ValuationColumns Failed", _
			'vApp:=ACApp, _
			'vClass:=ACClass, _
			'vMethod:="ValuationColumns", _
			'vErrNo:=Err.Number, _
			'vErrDesc:=Err.Description
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub UserControl_Initialize()
		
		MinimumWidth = 5000
		MinimumHeight = 2000
		
	End Sub
	
	Private Sub uctSumInsured_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		Dim lOffset As Integer
		
		If m_bShowRateAndPremium Then
			lOffset = 420
		End If
		
		If VB6.PixelsToTwipsY(MyBase.Height) < MinimumHeight Then
			MyBase.Height = VB6.TwipsToPixelsY(MinimumHeight)
		End If
		
		If VB6.PixelsToTwipsX(MyBase.Width) < MinimumWidth Then
			MyBase.Width = VB6.TwipsToPixelsX(MinimumWidth)
		End If
		
		lvwSumInsured.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(MyBase.Height) - 420 - lOffset)
		lvwSumInsured.Width = MyBase.Width '- 120
		
		cmdAdd.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(MyBase.Height) - 360 - lOffset)
		cmdEdit.Top = cmdAdd.Top
		cmdDelete.Top = cmdAdd.Top
		pnlTotalSumInsured.Top = cmdAdd.Top
		lblTotal.Top = cmdAdd.Top + VB6.TwipsToPixelsY(45)
		
		pnlTotalSumInsured.Left = lvwSumInsured.Left + lvwSumInsured.Width - pnlTotalSumInsured.Width
		lblTotal.Left = pnlTotalSumInsured.Left - lblTotal.Width
		
		lblRate.Top = lblTotal.Top + VB6.TwipsToPixelsY(420)
		lblPremium.Top = lblRate.Top
		txtRate.Top = pnlTotalSumInsured.Top + VB6.TwipsToPixelsY(420)
		pnlPremium.Top = txtRate.Top
		
		pnlPremium.Left = pnlTotalSumInsured.Left
		lblPremium.Left = lblTotal.Left
		
	End Sub
End Class