
Namespace Nexus

		Partial Class FIRE_OVERVIEW : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)

            Dim chkfire As String = GetValue("GROUP_FIRE", "FIRE")
            Dim chkOC As String = GetValue("GROUP_FIRE", "OFFICE_CONTENTS")

            If chkfire = "1" Then

                SEC_OVRW__FIRE_TOTAL_SI.Text = GetValue("FIRE", "FOV_TOTAL_SI")
                SEC_OVRW__FIRE_RISK_PREM.Text = GetValue("FIRE", "FOV_TOTAL_PREM")
                SEC_OVRW__FIRE_EXT_PREM.Text = Convert.ToDecimal(GetValue("FIRE", "CLAIM_PREPCOST_PREM")) + Convert.ToDecimal(GetValue("FIRE", "DISPOSAL_SALV_PREM")) + Convert.ToDecimal(GetValue("FIRE", "LEAKAGE_PREM")) + Convert.ToDecimal(GetValue("FIRE", "RIOT_STRIKE_PREM")) + Convert.ToDecimal(GetValue("FIRE", "LANDSLIP_PREM"))
                SEC_OVRW__FIRE_TOTAL_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__FIRE_RISK_PREM.Text <> "", SEC_OVRW__FIRE_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__FIRE_EXT_PREM.Text <> "", SEC_OVRW__FIRE_EXT_PREM.Text, 0))

                SEC_OVRW__RE_FIRE_TOTAL_SI.Text = GetValue("RI_EXP", "TOTAL_FIRE_RI_SI")
                SEC_OVRW__RE_FIRE_TARGET_RISK_SI.Text = GetValue("RI_EXP", "TOTAL_FIRE_RI_RISK_S")
                SEC_OVRW__RE_FIRE_RI_EXPOSURE.Text = GetValue("RI_EXP", "TOTAL_FIRE_RI_EXPOSURE")
                SEC_OVRW__RE_FIRE_RI_EXP_VAT_EXL.Text = (Convert.ToDecimal(IIf(SEC_OVRW__RE_FIRE_RI_EXPOSURE.Text <> "", SEC_OVRW__RE_FIRE_RI_EXPOSURE.Text, 0)) / 1.14)

            Else
                SEC_OVRW__FIRE_TOTAL_SI.Text = "0.0"
                SEC_OVRW__FIRE_RISK_PREM.Text = "0.0"
                SEC_OVRW__FIRE_EXT_PREM.Text = "0.0"
                SEC_OVRW__FIRE_TOTAL_PREM.Text = "0.0"

                SEC_OVRW__RE_FIRE_TOTAL_SI.Text = "0.0"
                SEC_OVRW__RE_FIRE_TARGET_RISK_SI.Text = "0.0"
                SEC_OVRW__RE_FIRE_RI_EXPOSURE.Text = "0.0"
                SEC_OVRW__RE_FIRE_RI_EXP_VAT_EXL.Text = "0.0"

            End If

            If chkOC = "1" Then
                SEC_OVRW__OFF_TOTAL_SI.Text = GetValue("OC", "CF_TOTAL_SI")
                SEC_OVRW__OFF_RISK_PREM.Text = GetValue("OC", "CF_TOTAL_PREMIUM")
                SEC_OVRW__OFF_EXT_PREM.Text = Convert.ToDecimal(GetValue("OC", "EF_MDMG_PREMIUM")) + Convert.ToDecimal(GetValue("OC", "EF_RS_PREMIUM")) + Convert.ToDecimal(GetValue("OC", "EF_AICW_PREMIUM")) + Convert.ToDecimal(GetValue("OC", "EF_CPC_PREMIUM"))
                SEC_OVRW__OFF_TOTAL_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__OFF_RISK_PREM.Text <> "", SEC_OVRW__OFF_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__OFF_EXT_PREM.Text <> "", SEC_OVRW__OFF_EXT_PREM.Text, 0))

                SEC_OVRW__RE_OFFC_TOTAL_SI.Text = GetValue("OC", "TRE_TOTAL_SI")
                SEC_OVRW__RE_OFFC_TARGET_RISK_SI.Text = GetValue("OC", "TRE_TARGET_RISK_SI")
                SEC_OVRW__RE_OFFC_RI_EXPOSURE.Text = GetValue("OC", "TRE_RI_EXPOSURE")
                SEC_OVRW__RE_OFFC_EXP_VAT_EXL.Text = (Convert.ToDecimal(IIf(SEC_OVRW__RE_OFFC_RI_EXPOSURE.Text <> "", SEC_OVRW__RE_OFFC_RI_EXPOSURE.Text, 0)) / 1.14)

            Else
                SEC_OVRW__OFF_TOTAL_SI.Text = "0.0"
                SEC_OVRW__OFF_RISK_PREM.Text = "0.0"
                SEC_OVRW__OFF_EXT_PREM.Text = "0.0"
                SEC_OVRW__OFF_TOTAL_PREM.Text = "0.0"

                SEC_OVRW__RE_OFFC_TOTAL_SI.Text = "0.0"
                SEC_OVRW__RE_OFFC_TARGET_RISK_SI.Text = "0.0"
                SEC_OVRW__RE_OFFC_RI_EXPOSURE.Text = "0.0"
                SEC_OVRW__RE_OFFC_EXP_VAT_EXL.Text ="0.0"
            End If





           
          
           
            SEC_OVRW__BLDG_TOTAL_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__BLDG_RISK_PREM.Text <> "", SEC_OVRW__BLDG_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__BLDG_EXT_PREM.Text <> "", SEC_OVRW__BLDG_EXT_PREM.Text, 0))

            SEC_OVRW__BI_TOTAL_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__BI_RISK_PREM.Text <> "", SEC_OVRW__BI_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__ACCT_EXT_PREM.Text <> "", SEC_OVRW__ACCT_EXT_PREM.Text, 0))

            SEC_OVRW__ACCT_TOTAL_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__ACCT_RISK_PREM.Text <> "", SEC_OVRW__ACCT_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__BI_EXT_PREM.Text <> "", SEC_OVRW__BI_EXT_PREM.Text, 0))

          
            SEC_OVRW__ACC_TOTAL_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__ACC_RISK_PREM.Text <> "", SEC_OVRW__ACC_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__ACC_EXT_PREM.Text <> "", SEC_OVRW__ACC_EXT_PREM.Text, 0))



            SEC_OVRW__TSI_TOTAL_SUM_INSURED.Text = Convert.ToDecimal(IIf(SEC_OVRW__FIRE_TOTAL_SI.Text <> "", SEC_OVRW__FIRE_TOTAL_SI.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__OFF_TOTAL_SI.Text <> "", SEC_OVRW__OFF_TOTAL_SI.Text, 0))

            SEC_OVRW__TSI_RISK_PREMIUM.Text = Convert.ToDecimal(IIf(SEC_OVRW__FIRE_RISK_PREM.Text <> "", SEC_OVRW__FIRE_RISK_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__OFF_RISK_PREM.Text <> "", SEC_OVRW__OFF_RISK_PREM.Text, 0))

            SEC_OVRW__TSI_EXTENSION_PREM.Text = Convert.ToDecimal(IIf(SEC_OVRW__FIRE_EXT_PREM.Text <> "", SEC_OVRW__FIRE_EXT_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__OFF_EXT_PREM.Text <> "", SEC_OVRW__OFF_EXT_PREM.Text, 0))

            SEC_OVRW__TSI_TOTAL_PREMIUM.Text = Convert.ToDecimal(IIf(SEC_OVRW__FIRE_TOTAL_PREM.Text <> "", SEC_OVRW__FIRE_TOTAL_PREM.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__OFF_TOTAL_PREM.Text <> "", SEC_OVRW__OFF_TOTAL_PREM.Text, 0))











       

       
            SEC_OVRW__CRI_TOTAL_SUM_INSURED.Text = Convert.ToDecimal(IIf(SEC_OVRW__RE_FIRE_TOTAL_SI.Text <> "", SEC_OVRW__RE_FIRE_TOTAL_SI.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__RE_OFFC_TOTAL_SI.Text <> "", SEC_OVRW__RE_OFFC_TOTAL_SI.Text, 0))

            SEC_OVRW__CRI_TARGET_RISK_SI.Text = Convert.ToDecimal(IIf(SEC_OVRW__RE_FIRE_TARGET_RISK_SI.Text <> "", SEC_OVRW__RE_FIRE_TARGET_RISK_SI.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__RE_OFFC_TARGET_RISK_SI.Text <> "", SEC_OVRW__RE_OFFC_TARGET_RISK_SI.Text, 0))

            SEC_OVRW__CRI_RI_EXPOSURE.Text = Convert.ToDecimal(IIf(SEC_OVRW__RE_FIRE_RI_EXPOSURE.Text <> "", SEC_OVRW__RE_FIRE_RI_EXPOSURE.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__RE_OFFC_RI_EXPOSURE.Text <> "", SEC_OVRW__RE_OFFC_RI_EXPOSURE.Text, 0))

            SEC_OVRW__CRI_RI_EXPOSURE_VAT_EXCL.Text = Convert.ToDecimal(IIf(SEC_OVRW__RE_FIRE_RI_EXP_VAT_EXL.Text <> "", SEC_OVRW__RE_FIRE_RI_EXP_VAT_EXL.Text, 0)) + Convert.ToDecimal(IIf(SEC_OVRW__RE_OFFC_EXP_VAT_EXL.Text <> "", SEC_OVRW__RE_OFFC_EXP_VAT_EXL.Text, 0))

        End Sub


		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
	End Class
	
End Namespace
		