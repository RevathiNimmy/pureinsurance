Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class Form1
	Inherits System.Windows.Forms.Form
	Private Sub cmdload_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdload.Click
		uctPartyTax1.TaxPercentage = 17.5
		uctPartyTax1.TaxExempt = True
		uctPartyTax1.TaxNumber = "steerer"
		uctPartyTax1.IsDomiciledForTax = True
	End Sub
End Class