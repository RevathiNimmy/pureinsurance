
Namespace Nexus

		Partial Class MOTORCC_DRIVERDETAILS : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)
            If MS__DRIVER.Rows.Count > 0 Then
                txtDriverDetails.Text = "1"

            End If
		End Sub

        Public Overrides Sub PostDataSetWrite()
            If MS__DRIVER.Rows.Count = 0 Then

                txtDriverDetails.Text = "0"
            Else
                txtDriverDetails.Text = "1"

            End If
        End Sub

        Public Overrides Sub PreDataSetWrite()
           
        End Sub
		
     
    End Class
	
End Namespace
		