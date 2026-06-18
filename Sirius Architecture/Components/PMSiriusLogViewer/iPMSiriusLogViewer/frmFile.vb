Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmFile
	Inherits System.Windows.Forms.Form
	
	Private m_sFilename As String = ""
	
	Public WriteOnly Property Filename() As String
		Set(ByVal Value As String)
			
			Try 
				
				' Set the filename
				m_sFilename = Value
				
				' Load the file
				rtfText.LoadFile(Value)
			
			Catch 
			End Try
			
			
			
			
		End Set
	End Property
    Private Const vbFormControlMenu As Integer = 0
	Private Sub frmFile_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Dont let the user close the window, just minimise it

        If UnloadMode = vbFormControlMenu Then
            Cancel = 1
            Me.WindowState = FormWindowState.Minimized
        End If

        eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmFile_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsY(Me.Height) > 400 Then
			If VB6.PixelsToTwipsX(Me.Width) > 120 Then
				
				With Me
					.rtfText.Left = 0
					.rtfText.Top = 0
					.rtfText.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(.Width) - 120)
					.rtfText.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(.Height) - 400)
				End With
				
			End If
		End If
		
	End Sub
End Class