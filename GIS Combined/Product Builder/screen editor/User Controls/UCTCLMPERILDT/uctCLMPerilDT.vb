Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctCLMPerilDT_NET.uctCLMPerilDT")> _
Public Partial Class uctCLMPerilDT
	Inherits System.Windows.Forms.UserControl
	
	
	Public Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
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
		
        Dim lngWidth As Integer = CInt(VB6.PixelsToTwipsX(lvwPerils.Width) / 4)
        ' insert the column headers
        lvwPerils.Columns.Insert(0, "Description", CInt(VB6.TwipsToPixelsX(lngWidth)))
		lvwPerils.Columns.Item(0).TextAlign = HorizontalAlignment.Left
		lvwPerils.Columns.Insert(1, "Peril Description", CInt(VB6.TwipsToPixelsX(lngWidth)))
		lvwPerils.Columns.Item(1).TextAlign = HorizontalAlignment.Left
		lvwPerils.Columns.Insert(2, "Sum Insured", CInt(VB6.TwipsToPixelsX(lngWidth)))
		lvwPerils.Columns.Item(2).TextAlign = HorizontalAlignment.Right
		lvwPerils.Columns.Insert(3, "Current Reserve", CInt(VB6.TwipsToPixelsX(lngWidth)))
		lvwPerils.Columns.Item(3).TextAlign = HorizontalAlignment.Right
	End Sub
	
	Private Sub lvwPerils_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPerils.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
	End Sub
	
	Private Sub uctCLMPerilDT_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		Dim lNewWidth As Integer = CInt(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(fraButtons.Width))
		Dim lNewHeight As Integer = CInt(VB6.PixelsToTwipsY(Me.Height))
		Dim lNewLeft As Integer = CInt(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(fraButtons.Width))
		
		If lNewWidth > 0 Then
			lvwPerils.Width = VB6.TwipsToPixelsX(lNewWidth)
		End If
		
		lvwPerils.Height = VB6.TwipsToPixelsY(lNewHeight)
		
		If lNewLeft > 0 Then
			fraButtons.Left = MyBase.Width - fraButtons.Width
        End If
        'developer guide no. 131
        If (lvwPerils.Columns.Count > 0) Then
            SetListviewWidth()
        End If
	End Sub
	
	Private Sub SetListviewWidth()
		
		Dim lngWidth As Integer = CInt(VB6.PixelsToTwipsX(lvwPerils.Width) / 4)
		

        For intCol As Integer = 1 To 4
            lvwPerils.Columns.Item(intCol - 1).Width = CInt(VB6.TwipsToPixelsX(lngWidth))
        Next intCol
	End Sub
End Class