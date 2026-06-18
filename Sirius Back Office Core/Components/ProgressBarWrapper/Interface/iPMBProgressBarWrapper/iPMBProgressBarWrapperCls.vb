Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("Wrapper_NET.Wrapper")> _
Public NotInheritable Class Wrapper 
    'Developer Guide No. 69
    Public frmLoadAVI As New frmLoadAVI
	Private m_bLoaded As Boolean
	
	Private Const ACClass As String = "Wrapper"
	
	' Interval
	Private m_lInterval As Integer
	
	' Public properties
	'******************
	' Dialog box title
	Public WriteOnly Property Caption() As String
		Set(ByVal Value As String)
			frmLoadAVI.Text = Value
		End Set
	End Property
	
	' Set interval (speed of bar)
	Public WriteOnly Property Interval() As Integer
		Set(ByVal Value As Integer)
			frmLoadAVI.UserControl11.Interval = Value
		End Set
	End Property
	
	' Start and display
	Public WriteOnly Property StartBar() As Boolean
		Set(ByVal Value As Boolean)
			
			frmLoadAVI.Show()
			m_bLoaded = True
			
			frmLoadAVI.UserControl11.StartBar = Value
			Application.DoEvents()
			
		End Set
	End Property
	
	' Stop and Hide
	Public WriteOnly Property StopBar() As Boolean
		Set(ByVal Value As Boolean)
			If m_bLoaded Then
				frmLoadAVI.UserControl11.StopBar = Value
				frmLoadAVI.Hide()
			End If
		End Set
	End Property
	
	' Dialog text
	Public WriteOnly Property Text() As String
		Set(ByVal Value As String)
			
			frmLoadAVI.lblText.Text = Value
			
		End Set
	End Property
	
	Protected Overrides Sub Finalize()
		If m_bLoaded Then
			frmLoadAVI.Close()
		End If
	End Sub
End Class
