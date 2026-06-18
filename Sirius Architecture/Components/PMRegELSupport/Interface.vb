Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class Interface_Renamed
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		Me.Close()
		
	End Sub
End Class