Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("UserControl1_NET.UserControl1")> _
Public Partial Class UserControl1
	Inherits System.Windows.Forms.UserControl
	<Browsable(False)> _
	Public WriteOnly Property StartBar() As Boolean
		Set(ByVal Value As Boolean)
			If Value Then
				ProgressBar1.Value = 0
				Timer1.Enabled = True
				Timer1_Tick(Timer1, New EventArgs())
			End If
		End Set
	End Property
	
	<Browsable(False)> _
	Public WriteOnly Property StopBar() As Boolean
		Set(ByVal Value As Boolean)
			If Value Then
				Timer1.Enabled = False
			End If
		End Set
	End Property
	
	<Browsable(False)> _
	Public WriteOnly Property Interval() As Integer
		Set(ByVal Value As Integer)
			If Value > 0 Then
				If Value = 0 Then
					Timer1.Enabled = False
				Else
                    Timer1.Interval = Value
					Timer1.Enabled = True
				End If
			Else
				Timer1.Interval = 300
				Timer1.Enabled = True
			End If
			
		End Set
	End Property
	Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick
		
		ProgressBar1.Value += 1
        Timer1.Interval = 1600
		If ProgressBar1.Value >= 30 Then
            ProgressBar1.Value = 0
        End If
        If Timer1.Interval > 1600 Then
            Timer1.Interval = 0
        End If
    End Sub
End Class