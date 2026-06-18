
Namespace Nexus

		Partial Class MOTOR_FAMILYDETAILS : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)
            If Not IsPostBack = True Then
                VEHDET__TPL_LIMIT_IDEMNITY.Enabled = False
                VEHDET__TPL_RATEN.Enabled = False

                MS__DLC_CR_LIMIT_IDEMN.Enabled = False
                MS__DLC_CR_RATEN.Enabled = False

                MS__DLC_WR_LIMIT_IDEMN.Enabled = False
                MS__DLC_WR_RATEN.Enabled = False

                MS__DLC_CHR_LIMIT_IDEMN.Enabled = False
                MS__DLC_CHR_RATEN.Enabled = False

                MS__DLC_CL_LIMIT_IDEMN.Enabled = False
                MS__DLC_CL_RATEN.Enabled = False

            End If

            'MS__DLC_CHR_PREM.Enabled = False
            'MS__DLC_CL_PREM.Enabled = False
            'MS__DLC_CR_PREM.Enabled = False
            'MS__DLC_WR_PREM.Enabled = False

            'If Not IsPostBack = True Then
            '    MS__DLC_CONTIGENT_LIAB.Checked = False
            '    MS__DLC_CR_SHRTFALL.Checked = False
            '    MS__DLC_WRECKAGE_REMOVAL.Checked = False

            '    MS__DLC_CHR_LIMIT_IDEMN.Enabled = False
            '    MS__DLC_CHR_RATEN.Enabled = False
            '    MS__DLC_CHR_PREM.Enabled = False

            '    MS__DLC_CL_LIMIT_IDEMN.Enabled = False
            '    MS__DLC_CL_RATEN.Enabled = False
            '    MS__DLC_CL_PREM.Enabled = False

            '    MS__DLC_CR_LIMIT_IDEMN.Enabled = False
            '    MS__DLC_CR_RATEN.Enabled = False
            '    MS__DLC_CR_PREM.Enabled = False

            '    MS__DLC_WR_LIMIT_IDEMN.Enabled = False
            '    MS__DLC_WR_LIMIT_IDEMN.Enabled = False
            '    MS__DLC_WR_RATEN.Enabled = False
            'End If

            'DisableControls(VEHDET__TPL_PREMIUM)
            'DisableControls(MS__DLC_WR_PREM)

        End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
  
        Protected Sub VEHDET__DLC_THIRD_PARTY_LIABILITY_CheckedChanged(sender As Object, e As EventArgs) Handles VEHDET__DLC_THIRD_PARTY_LIABILITY.CheckedChanged
            If VEHDET__DLC_THIRD_PARTY_LIABILITY.Checked = True Then
                VEHDET__TPL_LIMIT_IDEMNITY.Enabled = True
                VEHDET__TPL_RATEN.Enabled = True
            Else
                VEHDET__TPL_LIMIT_IDEMNITY.Enabled = False
                VEHDET__TPL_RATEN.Enabled = False
            End If
        End Sub

        

        Protected Sub MS__DLC_CR_SHRTFALL_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_CR_SHRTFALL.CheckedChanged
            If MS__DLC_CR_SHRTFALL.Checked = True Then
                MS__DLC_CR_LIMIT_IDEMN.Enabled = True
                MS__DLC_CR_RATEN.Enabled = True
            Else
                MS__DLC_CR_LIMIT_IDEMN.Enabled = False
                MS__DLC_CR_RATEN.Enabled = False

                MS__DLC_CR_LIMIT_IDEMN.Text = ""
                MS__DLC_CR_RATEN.Text = ""

            End If
        End Sub

        Protected Sub MS__DLC_WRECKAGE_REMOVAL_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_WRECKAGE_REMOVAL.CheckedChanged
            If MS__DLC_WRECKAGE_REMOVAL.Checked = True Then
                MS__DLC_WR_LIMIT_IDEMN.Enabled = True
                MS__DLC_WR_RATEN.Enabled = True

            Else
                MS__DLC_WR_LIMIT_IDEMN.Enabled = False
                MS__DLC_WR_RATEN.Enabled = False

                MS__DLC_WR_LIMIT_IDEMN.Text = ""
                MS__DLC_WR_RATEN.Text = ""
            End If
        End Sub

        Protected Sub MS__DLC_CAR_HIRE_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_CAR_HIRE.CheckedChanged
            If MS__DLC_CAR_HIRE.Checked = True Then
                MS__DLC_CHR_LIMIT_IDEMN.Enabled = True
                MS__DLC_CHR_RATEN.Enabled = True
            Else
                MS__DLC_CHR_LIMIT_IDEMN.Enabled = False
                MS__DLC_CHR_RATEN.Enabled = False

                MS__DLC_CHR_LIMIT_IDEMN.Text = ""
                MS__DLC_CHR_RATEN.Text = ""
 
            End If
        End Sub

        Protected Sub MS__DLC_CONTIGENT_LIAB_CheckedChanged(sender As Object, e As EventArgs) Handles MS__DLC_CONTIGENT_LIAB.CheckedChanged
            If MS__DLC_CONTIGENT_LIAB.Checked = True Then
                MS__DLC_CL_LIMIT_IDEMN.Enabled = True
                MS__DLC_CL_RATEN.Enabled = True
            Else
                MS__DLC_CL_LIMIT_IDEMN.Enabled = False
                MS__DLC_CL_RATEN.Enabled = False

                MS__DLC_CL_LIMIT_IDEMN.Text = ""
                MS__DLC_CL_RATEN.Text = ""
 
            End If
        End Sub

    End Class
	
End Namespace
		