Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmOptions
	Inherits System.Windows.Forms.Form
	
	Public Function Initialise(ByRef cSystemDSN As Collection, ByRef sDSN As String) As Integer



		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Make sure that we have DSNs
			If cSystemDSN.Count < 1 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Display DSNs
			For iPtr As Integer = 1 To cSystemDSN.Count
				cboDSN.Items.Add(CStr(cSystemDSN.Item(iPtr)))
			Next iPtr
			
			'Show this form
			Me.ShowDialog()
			
			'Get the selected DSN
			sDSN = cboDSN.Text
			
			'Make sure a DSN was selected
			If sDSN = "" Then
				result = gPMConstants.PMEReturnCode.PMFalse
				Me.Close()
				Return result
			End If
			
			Me.Close()
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	
	
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Me.Hide()
		Application.DoEvents()
		
	End Sub
	
	

	Private Sub frmOptions_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Center the form
		Me.SetBounds((Screen.PrimaryScreen.Bounds.Width / 2) - (Me.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (Me.Height / 2), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
		
	End Sub
End Class
