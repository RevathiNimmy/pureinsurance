Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmStartComponent
	Inherits System.Windows.Forms.Form
	' Event to Tell the Controlling Class to Start the Component.
	Public Event StartComponent()
	
	Private Sub tmrStartComponent_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrStartComponent.Tick
		
		' Disable the Timer so that we do not keep
		' calling the Start Component
		tmrStartComponent.Enabled = False
		
		' Raise the Start Component Event
		RaiseEvent StartComponent()
		
	End Sub
End Class