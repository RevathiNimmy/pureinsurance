Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctStandardWording_NET.uctStandardWording")> _
Public Partial Class uctStandardWording
	Inherits System.Windows.Forms.UserControl
	
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
	
	Private Sub UserControl_Initialize()
		
		MinimumWidth = 2000
		MinimumHeight = 2000
		
	End Sub
	
	Private Sub uctStandardWording_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		If VB6.PixelsToTwipsY(MyBase.Height) < 2000 Then
			MyBase.Height = VB6.TwipsToPixelsY(2000)
		End If
		
		If VB6.PixelsToTwipsX(MyBase.Width) < 2000 Then
			MyBase.Width = VB6.TwipsToPixelsX(2000)
		End If
		
		lvwStandardWording.Height = MyBase.Height - VB6.TwipsToPixelsY(480)
		lvwStandardWording.Width = MyBase.Width - VB6.TwipsToPixelsX(615)
		
		cmdAdd.Top = MyBase.Height - VB6.TwipsToPixelsY(360)
		cmdDelete.Top = cmdAdd.Top
		
		cmdUp.Left = MyBase.Width - VB6.TwipsToPixelsX(555)
		cmdDown.Left = cmdUp.Left
		lblMove.Left = cmdUp.Left
		
		cmdUp.Top = lvwStandardWording.Top
		cmdDown.Top = lvwStandardWording.Top + lvwStandardWording.Height - cmdDown.Height
		lblMove.Top = (cmdUp.Top + cmdDown.Top + lblMove.Height) / 2
		
	End Sub
End Class