
Namespace Nexus

		Partial Class BLDGSP_GENERAL : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)

            hvAgreedRateValue.Value = GetValue("GROUP_FIRE", "FINAL_RATE_AGREED_RATE")
		End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
	End Class
	
End Namespace
		