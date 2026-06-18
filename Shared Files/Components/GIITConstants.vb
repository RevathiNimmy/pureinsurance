Option Strict Off
Option Explicit On
Imports System
Public Module GIITConstants
	
	
	' GIITGemPolicy
	Public Const GIITGemPolicy As String = "GIITGemPolicy"
	
	Public Const GIITGisPolicyLinkId As String = "gis_policy_link_id"
	
	
	' truck_client_details
	Public Const GIITTruckClientDetails As String = "truck_client_details"
	
	Public Const GIITTkCliCode As String = "tk_cli_code"
	Public Const GIITTkCliTitle As String = "tk_cli_title"
	Public Const GIITTkCliForename As String = "tk_cli_forename"
	Public Const GIITTkCliSurname As String = "tk_cli_surname"
	Public Const GIITTkCliSex As String = "tk_cli_sex"
	Public Const GIITTkCliName As String = "tk_cli_name"
	Public Const GIITTkCliHomeTelno As String = "tk_cli_home_telno"
	Public Const GIITTkCliWorkTelno As String = "tk_cli_work_telno"
	Public Const GIITTkCliWorkTelnoExt As String = "tk_cli_work_telno_ext"
	Public Const GIITTkCliAddLine1 As String = "tk_cli_add_line1"
	Public Const GIITTkCliAddLine2 As String = "tk_cli_add_line2"
	Public Const GIITTkCliAddLine3 As String = "tk_cli_add_line3"
	Public Const GIITTkCliAddLine4 As String = "tk_cli_add_line4"
	Public Const GIITTkCliPcode As String = "tk_cli_pcode"
	
	
	' truck_policy_data
	Public Const GIITTruckPolicyData As String = "truck_policy_data"
	
	Public Const GIITTkPolicyNo As String = "tk_policy_no"
	Public Const GIITTkAnalCode As String = "tk_anal_code"
	Public Const GIITTkAnalDesc As String = "tk_anal_desc"
	Public Const GIITConCreaDate As String = "con_crea_date"
	Public Const GIITConModiDate As String = "con_modi_date"
	Public Const GIITPolTotPremium As String = "pol_tot_premium"
	Public Const GIITPolStatus As String = "pol_status"
	Public Const GIITPolStartDate As String = "pol_start_date"
	Public Const GIITPolStartTime As String = "pol_start_time"
	Public Const GIITPolEndDate As String = "pol_end_date"
	Public Const GIITPolEndTime As String = "pol_end_time"
	Public Const GIITPolPeriodMonths As String = "pol_period_months"
	Public Const GIITPolDateKey As String = "pol_date_key"
	Public Const GIITMtaOldPremium As String = "mta_old_premium"
	Public Const GIITMtaNewPremium As String = "mta_new_premium"
	Public Const GIITMtaEffectiveDate As String = "mta_effective_date"
	Public Const GIITMtaNewSelCover As String = "mta_new_sel_cover"
	Public Const GIITTkPolDate As String = "tk_pol_date"
	
	
	' truck_vehicle
	Public Const GIITTruckVehicle As String = "truck_vehicle"
	
	Public Const GIITTkVehDesc As String = "tk_veh_desc"
	Public Const GIITTkVehGvw As String = "tk_veh_gvw"
	Public Const GIITTkVehGvwUnits As String = "tk_veh_gvw_units"
	Public Const GIITTkVehCarryCapac As String = "tk_veh_carry_capac"
	Public Const GIITTkVehCarryUnits As String = "tk_veh_carry_units"
	Public Const GIITTkVehCc As String = "tk_veh_cc"
	Public Const GIITTkVehCtryOrig As String = "tk_veh_ctry_orig"
	Public Const GIITTkVehFuelType As String = "tk_veh_fuel_type"
	Public Const GIITTkVehBodyType As String = "tk_veh_body_type"
	Public Const GIITTkVehRegNo As String = "tk_veh_reg_no"
	Public Const GIITTkVehQplate As String = "tk_veh_qplate"
	Public Const GIITTkVehRegDate As String = "tk_veh_reg_date"
	Public Const GIITTkVehPurchDate As String = "tk_veh_purch_date"
	Public Const GIITTkVehValue As String = "tk_veh_value"
	Public Const GIITTkVehHanded As String = "tk_veh_handed"
	Public Const GIITTkVehSeats As String = "tk_veh_seats"
	Public Const GIITTkVehModified As String = "tk_veh_modified"
	Public Const GIITTkVehMedModif As String = "tk_veh_med_modif"
	Public Const GIITTkVehSecDevYn As String = "tk_veh_sec_dev_yn"
	Public Const GIITTkVehTrailYn As String = "tk_veh_trail_yn"
	Public Const GIITTkVehModel As String = "tk_veh_model"
	Public Const GIITTkVehMake As String = "tk_veh_make"
	Public Const GIITTkVehMakeModel As String = "tk_veh_make_model"
	Public Const GIITTkNumSecu As String = "tk_num_secu"
	Public Const GIITTkNumMods As String = "tk_num_mods"
	Public Const GIITTkTotTrail As String = "tk_tot_trail"
	Public Const GIITTkABICode As String = "tk_abi_code"
	Public Const GIITTkCurrMileage As String = "tk_curr_mileage"
	' KB - 280102 - More contants for Truck POS Documents
	Public Const GIITTrkOwnUseAnotherVehDesc As String = "trk_own_use_another_veh_desc"
	Public Const GIITTrkOwnUseAnotherVehYN As String = "trk_own_use_another_veh_YN"
	Public Const GIITTkAddOthVehYN As String = "tk_add_oth_veh_yn"
	
	' tk_security_details
	Public Const GIITTkSecurityDetails As String = "tk_security_details"
	
	Public Const GIITTkSecuDevCode As String = "tk_secu_dev_code"
	Public Const GIITTkSecuBsYn As String = "tk_secu_bs_yn"
	Public Const GIITTkSecuInstaller As String = "tk_secu_installer"
	Public Const GIITTkSecuInstDate As String = "tk_secu_inst_date"
	
	
	' tk_modif_details
	Public Const GIITTkModifDetails As String = "tk_modif_details"
	
	Public Const GIITTkModificCode As String = "tk_modific_code"
	
	
	' tk_trailer_details
	Public Const GIITTkTrailerDetails As String = "tk_trailer_details"
	
	Public Const GIITTkTrailDesc As String = "tk_trail_desc"
	Public Const GIITTkTrailCapac As String = "tk_trail_capac"
	Public Const GIITTkTrailWeight As String = "tk_trail_weight"
	Public Const GIITTkTrailValue As String = "tk_trail_value"
	Public Const GIITTkTrailCover As String = "tk_trail_cover"
	Public Const GIITTkTrailDetach As String = "tk_trail_detach"
	Public Const GIITTkCompanyInd As String = "tk_company_ind"
	Public Const GIITTkCompanyName As String = "tk_company_name"
	
	
	' tk_other_insurances
	Public Const GIITTkOtherInsurances As String = "tk_other_insurances"
	
	Public Const GIITTkOthInsType As String = "tk_oth_ins_type"
	Public Const GIITTkOthInsInsr As String = "tk_oth_ins_insr"
	Public Const GIITTkOthInsCurr As String = "tk_oth_ins_curr"
	Public Const GIITTkOthInsNcd As String = "tk_oth_ins_ncd"
	
	
	' tk_risk
	Public Const GIITTkRisk As String = "tk_risk"
	
	Public Const GIITTkNcdYears As String = "tk_ncd_years"
	Public Const GIITTkNcdType As String = "tk_ncd_type"
	Public Const GIITTkNcdProtected As String = "tk_ncd_protected"
	Public Const GIITTkCurrentInsurer As String = "tk_current_insurer"
	Public Const GIITTkInsuranceExpiry As String = "tk_insurance_expiry"
	Public Const GIITTkRenewalDate As String = "tk_renewal_date"
	Public Const GIITTkOtherInsurYn As String = "tk_other_insur_yn"
	Public Const GIITTkRskGaragChar2 As String = "tk_rsk_garag_char2"
	Public Const GIITTkRskPcGaraged As String = "tk_rsk_pc_garaged"
	Public Const GIITTkRskVehOwner As String = "tk_rsk_veh_owner"
	Public Const GIITTkRskVehKeeper As String = "tk_rsk_veh_keeper"
	Public Const GIITTkRskAnnMiles As String = "tk_rsk_ann_miles"
	Public Const GIITTkRskOpYn As String = "tk_rsk_op_yn"
	Public Const GIITTkRskClUse As String = "tk_rsk_cl_use"
	Public Const GIITTkRskRadUse As String = "tk_rsk_rad_use"
	Public Const GIITTkRskFrgnUse As String = "tk_rsk_frgn_use"
	Public Const GIITTkRskHazSubs As String = "tk_rsk_haz_subs"
	Public Const GIITTkRskAirYn As String = "tk_rsk_air_yn"
	Public Const GIITTkRskHazSite As String = "tk_rsk_haz_site"
	Public Const GIITTkRskDriRes As String = "tk_rsk_dri_res"
	Public Const GIITTkRskMinAge As String = "tk_rsk_min_age"
	Public Const GIITTkNumOins As String = "tk_num_oins"
	Public Const GIITTkNumDrivs As String = "tk_num_drivs"
	' KB 16/01/02 New constants for Truck Point Of Sale Documents
	Public Const GIITTrkNonMotaConvTxt As String = "trk_non_mota_conv_txt"
	Public Const GIITTrkLicSuspendedTxt As String = "trk_lic_suspended_txt"
	Public Const GIITTkAddLicRefusedYN As String = "tk_add_lic_refused_yn"
	Public Const GIITTkAddLicRefusedDesc As String = "tk_add_lic_refused_desc"
	Public Const GIITTkRskGaragDay As String = "tk_rsk_garag_day"
	' KB 28/01/02 More Truck POS changes
	Public Const GIITTrkPassengerVehicleYN As String = "trk_passenger_vehicle_yn"
	Public Const GIITTkRskPassengersYN As String = "tk_rsk_passengers_yn"
	' tk_cover
	Public Const GIITTkCover As String = "tk_cover"
	
	Public Const GIITTkCompCovYn As String = "tk_comp_cov_yn"
	Public Const GIITTkTpftCovYn As String = "tk_tpft_cov_yn"
	Public Const GIITTkTpoCovYn As String = "tk_tpo_cov_yn"
	Public Const GIITTkVolXs As String = "tk_vol_xs"
	Public Const GIITTkSurCoy As String = "tk_sur_coy"
	Public Const GIITTkSurEffDate As String = "tk_sur_eff_date"
	Public Const GIITTkSelCover As String = "tk_sel_cover"
	
	
	' truck_driver
	Public Const GIITTruckDriver As String = "truck_driver"
	
	Public Const GIITTkDrName As String = "tk_dr_name"
	Public Const GIITTkDrSex As String = "tk_dr_sex"
	Public Const GIITTkDrBirthDate As String = "tk_dr_birth_date"
	Public Const GIITTkDrResDate As String = "tk_dr_res_date"
	Public Const GIITTkDrMarStat As String = "tk_dr_mar_stat"
	Public Const GIITTkDrRelProp As String = "tk_dr_rel_prop"
	Public Const GIITTkDrLicType As String = "tk_dr_lic_type"
	Public Const GIITTkDrLicDate As String = "tk_dr_lic_date"
	Public Const GIITTkDrVehUse As String = "tk_dr_veh_use"
	Public Const GIITTkDrEmpStat As String = "tk_dr_emp_stat"
	Public Const GIITTkDrFtPt As String = "tk_dr_ft_pt"
	Public Const GIITTkDrOccCode As String = "tk_dr_occ_code"
	Public Const GIITTkDrEmpCode As String = "tk_dr_emp_code"
	Public Const GIITTkDrOempStat As String = "tk_dr_oemp_stat"
	Public Const GIITTkDrOFtPt As String = "tk_dr_o_ft_pt"
	Public Const GIITTkDrOoccCode As String = "tk_dr_oocc_code"
	Public Const GIITTkDrOempCode As String = "tk_dr_oemp_code"
	Public Const GIITTkDrNumClaims As String = "tk_dr_num_claims"
	Public Const GIITTkDrNumConv As String = "tk_dr_num_conv"
	Public Const GIITTkDrNumPends As String = "tk_dr_num_pends"
	Public Const GIITTkDrNumMeds As String = "tk_dr_num_meds"
	Public Const GIITTkDrInsRefu As String = "tk_dr_ins_refu"
	Public Const GIITTkDrDisConv As String = "tk_dr_dis_conv"
	Public Const GIITTkDrMedRest As String = "tk_dr_med_rest"
	Public Const GIITTkDrMedPeriod As String = "tk_dr_med_period"
	' KB 16/01/02 - Constants for Point Of Sale Docs
	Public Const GIITTkDrOthVehYN As String = "tk_dr_oth_veh_yn" ' 28/01/02
	Public Const GIITTkDrNonMotorConv As String = "tk_dr_non_motor_conv"
	Public Const GIITTkDrLostLic As String = "tk_dr_lost_lic"
	
	
	' tk_dr_claim
	Public Const GIITTkDrClaim As String = "tk_dr_claim"
	
	Public Const GIITTkDrClaimDate As String = "tk_dr_claim_date"
	Public Const GIITTkDrClaimInsCost As String = "tk_dr_claim_ins_cost"
	Public Const GIITTkDrClaimTpCost As String = "tk_dr_claim_tp_cost"
	Public Const GIITTkDrClaimTotCost As String = "tk_dr_claim_tot_cost"
	Public Const GIITTkDrClaimFaultYn As String = "tk_dr_claim_fault_yn"
	Public Const GIITTkDrClaimNcdLost As String = "tk_dr_claim_ncd_lost"
	Public Const GIITTkDrClaimPi As String = "tk_dr_claim_pi"
	Public Const GIITTkDrClaimMedRel As String = "tk_dr_claim_med_rel"
	Public Const GIITTkDrClaimConRel As String = "tk_dr_claim_con_rel"
	Public Const GIITTkDrClaimType As String = "tk_dr_claim_type"
	
	
	' tk_dr_conviction
	Public Const GIITTkDrConviction As String = "tk_dr_conviction"
	
	Public Const GIITTkDrConDate As String = "tk_dr_con_date"
	Public Const GIITTkDrConOffDate As String = "tk_dr_con_off_date"
	Public Const GIITTkDrConOffCode As String = "tk_dr_con_off_code"
	Public Const GIITTkDrConFine As String = "tk_dr_con_fine"
	Public Const GIITTkDrConDisqual As String = "tk_dr_con_disqual"
	Public Const GIITTkDrConPenPts As String = "tk_dr_con_pen_pts"
	Public Const GIITTkDrConAccRel As String = "tk_dr_con_acc_rel"
	Public Const GIITTkDrConAccRelNo As String = "tk_dr_con_acc_rel_no"
	Public Const GIITTkDrConAlcType As String = "tk_dr_con_alc_type"
	Public Const GIITTkDrConAlcAmt As String = "tk_dr_con_alc_amt"
	
	
	' tk_dr_pending
	Public Const GIITTkDrPending As String = "tk_dr_pending"
	
	Public Const GIITTkDrPendIncDate As String = "tk_dr_pend_inc_date"
	Public Const GIITTkDrPendDueDate As String = "tk_dr_pend_due_date"
	Public Const GIITTkDrPendOffCode As String = "tk_dr_pend_off_code"
	
	
	' tk_dr_medical
	Public Const GIITTkDrMedical As String = "tk_dr_medical"
	
	Public Const GIITTkDrMedSustDate As String = "tk_dr_med_sust_date"
	Public Const GIITTkDrMedLastDate As String = "tk_dr_med_last_date"
	Public Const GIITTkDrMedCode As String = "tk_dr_med_code"
	Public Const GIITTkDrMedOnMed As String = "tk_dr_med_on_med"
	Public Const GIITTkDrMedDvlcYn As String = "tk_dr_med_dvlc_yn"
	
	
	' KBrown 16/12/02 New Constants for new tables
	' Payment_Bank
	
	Public Const GIITPayment_Bank As String = "tk_Payment_Bank"
	Public Const GIITPaymentBankId As String = "GIITPayment_Bank_id"
	Public Const GIITIptRate As String = "ipt_rate"
	Public Const GIITPayMethod As String = "pay_method"
	Public Const GIITPayCardName As String = "pay_card_name"
	Public Const GIITPayCardType As String = "pay_card_type"
	Public Const GIITPayCardNo As String = "pay_card_no"
	Public Const GIITPayCardIssueNo As String = "pay_card_issue_no"
	Public Const GIITPayCardStartDate As String = "pay_card__start_date"
	Public Const GIITPayCardExpDate As String = "pay_card__exp_date"
	Public Const GIITPayBankName As String = "pay_bank_name"
	Public Const GIITPayBankBranch As String = "pay_bank_branch"
	Public Const GIITPayBankSortCode As String = "pay_bank_sort_code"
	Public Const GIITPayBankAccName As String = "pay_bank_acc_name"
	Public Const GIITPayBankAccNo As String = "pay_bank_acc_no"
	Public Const GIITPayBankAddr1 As String = "pay_bank_addr1"
	Public Const GIITPayBankAddr2 As String = "pay_bank_addr2"
	Public Const GIITPayBankAddr3 As String = "pay_bank_addr3"
	Public Const GIITPayBankAddr4 As String = "pay_bank_addr4"
	Public Const GIITPayBankPCode As String = "pay_bank_pcode"
	Public Const GIITPayInstalAmnt As String = "pay_instal_amnt"
	Public Const GIITPayInstallDesc As String = "pay_install_desc"
	Public Const GIITPayInstalYN As String = "pay_instal_yn"
	Public Const GIITPayInstalNo As String = "pay_instal_no"
	Public Const GIITPayInstalDate As String = "pay_instal_date"
	Public Const GIITPayInstallMonthlyAmt As String = "pay_install_monthly_amt"
	Public Const GIITPayInstallNo As String = "pay_install_no"
	Public Const GIITPayInstallDeptYN As String = "pay_instal_dept_yn"
	Public Const GIITPayInstalDept As String = "pay_instal_dept"
	Public Const GIITPayPrefInstalDay As String = "pay_pref_instal_day"
	Public Const GIITPayInstallFirst As String = "pay_install_first"
	Public Const GIITPayInstalTot As String = "pay_instal_tot"
	Public Const GIITPayCreditGe As String = "pay_credit_ge"
	Public Const GIITPayDDMSignedYN As String = "pay_ddm_signed_yn"
	Public Const GIITPayDepositYN As String = "pay_deposit_yn"
	Public Const GIITPayDepositAmnt As String = "pay_deposit_amnt"
	Public Const GIITPayDepositMethod As String = "pay_deposit_method"
	Public Const GIITPayTotPrem As String = "pay_tot_prem"
	Public Const GIITPayTotPremIncIPT As String = "pay_tot_prem_inc_IPT"
	
	' MTA_Details table
	
	Public Const GIITMTADetails As String = "tk_MTA_Details"
	Public Const GIITMTADetailsId As String = "GIITMTA_Details_id"
	Public Const GIITMtaType As String = "mta_type"
	Public Const GIITMtaStatus As String = "mta_status"
	Public Const GIITMtaStartDate As String = "mta_start_date"
	Public Const GIITMtaEndDate As String = "mta_end_date"
	Public Const GIITMtaPeriod As String = "mta_period"
	Public Const GIITMtaCancelPrevClaimYN As String = "mta_cancel_prev_claim_yn"
	Public Const GIITMtaPremiumIPT As String = "mta_premium_ipt"
	Public Const GIITMtaPremium As String = "mta_premium"
	Public Const GIITMtaPremiumPlusAddons As String = "mta_premium_plus_addons"
	Public Const GIITMtaGrossPremium As String = "mta_gross_premium"
	Public Const GIITMtaAnnPremOldGross As String = "mta_ann_prem_old_gross"
	Public Const GIITMtaFutureAnnPremGross As String = "mta_future_ann_prem_gross"
	Public Const GIITMtaReferPremiumGross As String = "mta_refer_premium_gross"
	Public Const GIITMtaAdjPercent As String = "mta_adj_percent"
	Public Const GIITMtaRnwlPremium As String = "mta_rnwl_premium"
	Public Const GIITMtaCancelDeceasedYN As String = "mta_cancel_deceased_yn"
	Public Const GIITMtaCancelSoldYN As String = "mta_cancel_sold_yn"
	Public Const GIITMtaCancelTransferInsYN As String = "mta_cancel_transfer_ins_yn"
	Public Const GIITMtaCancelTransferCliYN As String = "mta_cancel_transfer_cli_yn"
	Public Const GIITMtaCancelInsRequestYN As String = "mta_cancel_ins_request_yn"
	Public Const GIITMtaCancelOtherYN As String = "mta_cancel_other_yn"
	Public Const GIITMtaDetailsChanged As String = "mta_details_changed"
	Public Const GIITMtaRenewalInvite As String = "mta_renewal_invite"
	Public Const GIITMtaRequotePrem As String = "mta_requote_prem"
	Public Const GIITMtaAdminAmount As String = "mta_admin_amount"
	Public Const GIITMtaAdminSwitch As String = "mta_admin_switch"
	Public Const GIITMtaCanRequest As String = "mta_can_request"
	Public Const GIITMtaCancelDate As String = "mta_cancel_date"
	Public Const GIITMtaCancelTime As String = "mta_cancel_time"
	
	
	' GIITLegacy_Policy table
	
	Public Const GIITLegacy_Policy As String = "tk_legacy_policy"
	Public Const GIITLegacyPolicyID As String = "GIITLegacy_Policy_id"
	Public Const GIITTrkBackDateCENum As String = "trk_backdate_ce_num"
	Public Const GIITTrkBackDateAuthCode As String = "trk_backdate_auth_code"
	Public Const GIITTrkNewPrem As String = "trk_new_prem"
	Public Const GIITTrkOvrdPercent As String = "trk_ovrd_percent"
	Public Const GIITTrkOvrdPremium As String = "trk_ovrd_premium"
	Public Const GIITTrkOverrideYN As String = "trk_override_yn"
	Public Const GIITTrkOrigPremiumSave As String = "trk_orig_premium_save"
	Public Const GIITTrkReplVehYN As String = "trk_repl_veh_yn"
	Public Const GIITTrkReplVehPremium As String = "trk_repl_veh_premium"
	Public Const GIITTrkBrkdCovYN As String = "trk_brkd_cov_yn"
	Public Const GIITTrkBrkdCovPremium As String = "trk_brkd_cov_premium"
	Public Const GIITTrkBrkdCovVehAge As String = "trk_brkd_cov_veh_age"
	Public Const GIITTrkReplVehYNSV As String = "trk_repl_veh_yn_sv"
	Public Const GIITTrkBrkdCovPaidYN As String = "trk_brkd_cov_paid_yn"
	Public Const GIITTrkReplVehPrem As String = "trk_repl_veh_prem"
	Public Const GIITTrkBrkdCovPrem As String = "trk_brkd_cov_prem"
	Public Const GIITTrkCoverOut As String = "trk_cover_out"
	Public Const GIITTrkPremIncIPT As String = "trk_prem_inc_ipt"
	Public Const GIITTrkIPTPrem As String = "trk_ipt_prem"
	Public Const GIITTrkNCDClmPrevPolNo As String = "trk_ncd_clm_prev_pol_no"
	Public Const GIITTrkNCDClmProofYN As String = "trk_ncd_clm_proof_yn"
	Public Const GIITTrkEDILastRcvEvent As String = "trk_edi_last_rcv_event"
	Public Const GIITTrkEDILastSndEvent As String = "trk_edi_last_snd_event"
	Public Const GIITTrkEDILastMsgCount As String = "trk_edi_last_msg_count"
	Public Const GIITTrkEDINewVerNo As String = "trk_edi_new_ver_no"
	Public Const GIITTrkGarAddLine1 As String = "trk_gar_add_line1"
	Public Const GIITTrkGarAddLine2 As String = "trk_gar_add_line2"
	Public Const GIITTrkGarAddLine3 As String = "trk_gar_add_line3"
	Public Const GIITTrkGarAddLine4 As String = "trk_gar_add_line4"
	
	' KB - 16/12/02 - End
End Module