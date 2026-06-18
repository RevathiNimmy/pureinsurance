Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctSimulateCombo_NET.uctSimulateCombo")> _
Public Partial Class uctSimulateCombo
	Inherits System.Windows.Forms.UserControl
	'Event Declarations:
	Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
	
	Private m_bEnabled As Boolean
	
	Private Sub uctSimulateCombo_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.74 

        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub uctSimulateCombo_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        'Developer Guide No. 74
        Combo1.SetBounds(0, 0, (Width) - VB6.TwipsToPixelsX(100), 0, BoundsSpecified.X Or BoundsSpecified.Y Or BoundsSpecified.Width)
    End Sub
    <Browsable(True)> _
    <Description("Returns/sets the type of mouse pointer displayed when over part of an object.")> _
    Public Property MousePointer() As Cursor
        'Public Property MousePointer() As Integer
        Get
            Return Combo1.Cursor
        End Get
        Set(ByVal Value As Cursor)

            MyBase.Cursor = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property ComboWidth() As Single
        Get
            'Developer Guide No. 74
            Return (Combo1.Width)
        End Get
    End Property
	
	<Browsable(True)> _
	Public Shadows Property Enabled() As Boolean
		Get
			Return True
		End Get
		Set(ByVal Value As Boolean)
			m_bEnabled = Value
		End Set
	End Property
End Class