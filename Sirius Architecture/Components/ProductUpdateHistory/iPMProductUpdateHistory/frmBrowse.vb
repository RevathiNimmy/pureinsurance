Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmBrowse
	Inherits System.Windows.Forms.Form
	
	Private m_lStatus As Integer
	Private m_sNotePath As String = ""
	
	Public WriteOnly Property NotePath() As String
		Set(ByVal Value As String)
			m_sNotePath = Value
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Me.Close()
		
	End Sub
	
	Public Sub ShowNote()
		
		Try 
			
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			wbBrowser.Stop()
			
			wbBrowser.Navigate(New URI(m_sNotePath))
		
		Catch 
			
			
			
			m_lStatus = gPMConstants.PMEReturnCode.PMError
		End Try
		
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmBrowse_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsX(Me.Width) < 4000 Then
			Me.Width = VB6.TwipsToPixelsX(4000)
		End If
		
		If VB6.PixelsToTwipsY(Me.Height) < 3000 Then
			Me.Height = VB6.TwipsToPixelsY(3000)
		End If
		
		cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOk.Height) - 120)
		cmdOk.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdOk.Width) - 120)
		
		wbBrowser.Width = CInt(Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240))
		wbBrowser.Height = CInt(cmdOk.Top - VB6.TwipsToPixelsY(240))
		
	End Sub
End Class