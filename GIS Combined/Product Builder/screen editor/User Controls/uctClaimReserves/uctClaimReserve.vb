Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctClaimReserve_NET.uctClaimReserve")> _
Public Partial Class uctClaimReserve
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
		
		Dim sngWidth As Single = VB6.PixelsToTwipsX(lvwReserve.Width) / 8
		
		' insert the column headers
		lvwReserve.Columns.Insert(0, "               ", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(0).TextAlign = HorizontalAlignment.Left
		lvwReserve.Columns.Insert(1, "Initial Reserve", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(1).TextAlign = HorizontalAlignment.Right
		lvwReserve.Columns.Insert(2, "Revision Amount", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(2).TextAlign = HorizontalAlignment.Right
		lvwReserve.Columns.Insert(3, "This Revision", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(3).TextAlign = HorizontalAlignment.Right
		lvwReserve.Columns.Insert(4, "Current Reserve", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(4).TextAlign = HorizontalAlignment.Right
		lvwReserve.Columns.Insert(5, "Incurred", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(5).TextAlign = HorizontalAlignment.Right
		lvwReserve.Columns.Insert(6, "Sum Insured", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(6).TextAlign = HorizontalAlignment.Right
		lvwReserve.Columns.Insert(7, "Average", CInt(VB6.TwipsToPixelsX(sngWidth)))
		lvwReserve.Columns.Item(7).TextAlign = HorizontalAlignment.Right
		
	End Sub
	
	Private Sub uctClaimReserve_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		If VB6.PixelsToTwipsY(MyBase.Height) < MinimumHeight Then
			MyBase.Height = VB6.TwipsToPixelsY(MinimumHeight)
		End If
		
		If VB6.PixelsToTwipsX(MyBase.Width) < MinimumWidth Then
			MyBase.Width = VB6.TwipsToPixelsX(MinimumWidth)
		End If
		
		lvwReserve.Height = MyBase.Height - VB6.TwipsToPixelsY(100)
		lvwReserve.Width = MyBase.Width - VB6.TwipsToPixelsX(1400)
		
		cmdEdit.Left = MyBase.Width - VB6.TwipsToPixelsX(1200)
		cmdEdit.Top = lvwReserve.Top
		
		MinimumWidth = 2000
		MinimumHeight = 2000
		
		Dim sngWidth As Single = (VB6.PixelsToTwipsX(lvwReserve.Width) - 100) / 8
		
		For iCol As Integer = 1 To lvwReserve.Columns.Count
			lvwReserve.Columns.Item(iCol - 1).Width = CInt(VB6.TwipsToPixelsX(sngWidth))
		Next iCol
	End Sub
End Class