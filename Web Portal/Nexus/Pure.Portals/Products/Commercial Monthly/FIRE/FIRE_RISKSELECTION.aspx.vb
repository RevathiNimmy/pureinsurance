
Namespace Nexus

		Partial Class FIRE_RISKSELECTION : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			SetPageProgress(3)
		End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
	End Class
	
End Namespace
		