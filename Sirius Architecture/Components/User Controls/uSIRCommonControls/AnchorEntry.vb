Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend NotInheritable Class AnchorEntry 
	
	' The control
	Public Control As Control
	
	' The anchor properties
	Public Anchor As uctAnchor.ControlAnchorEnum
	
	' A shortcut to a control key
	Public ReadOnly Property Key() As String
		Get
			Try 
				
				Return CStr(CDbl(Me.Control.Name & "(") + ContainerHelper.GetControlIndex(Me.Control) + CDbl(")"))
			
			Catch 
				
				Return Me.Control.Name
				
				' All done
			End Try
		End Get
	End Property
End Class
