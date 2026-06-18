Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant
Namespace Nexus

		Partial Class MOTOR_FAMILYDETAILS : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            If Not IsPostBack Then
                If (VEHDET__TPL_LIMIT_IDEMNITY.Text = "" Or oQuote.InsuranceFileTypeCode.Trim <> "MTAQUOTE") Then

                    VEHDET__DLC_THIRD_PARTY_LIABILITY.Checked = True
                    If VEHDET__DLC_THIRD_PARTY_LIABILITY.Checked = True Then
                        VEHDET__TPL_LIMIT_IDEMNITY.Enabled = True
                        VEHDET__TPL_RATEN.Enabled = True
                        VEHDET__TPL_LIMIT_IDEMNITY.Text = "2500000"
                    Else
                        VEHDET__TPL_LIMIT_IDEMNITY.Enabled = False
                        VEHDET__TPL_RATEN.Enabled = False

                        VEHDET__TPL_LIMIT_IDEMNITY.Text = ""
                        VEHDET__TPL_RATEN.Text = ""
                    End If


                End If
            End If


        End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
  
        Protected Sub VEHDET__DLC_THIRD_PARTY_LIABILITY_CheckedChanged(sender As Object, e As EventArgs) Handles VEHDET__DLC_THIRD_PARTY_LIABILITY.CheckedChanged
            
        End Sub

        

        Protected Sub MS__DLC_CR_SHRTFALL_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_CR_SHRTFALL.CheckedChanged
           
        End Sub

        Protected Sub MS__DLC_WRECKAGE_REMOVAL_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_WRECKAGE_REMOVAL.CheckedChanged
          
        End Sub

        Protected Sub MS__DLC_CAR_HIRE_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_CAR_HIRE.CheckedChanged
         
        End Sub

        Protected Sub MS__DLC_CONTIGENT_LIAB_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_CONTIGENT_LIAB.CheckedChanged
          
        End Sub

    End Class
	
End Namespace
		