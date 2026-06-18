Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("GIIHConstants_NET.GIIHConstants")> _
 Public Module GIIHConstants
	
	'******************************************************************************
	' GEMINI II HOUSEHOLD SPECIFIC CONSTANT DEFINITIONS
	'******************************************************************************
	
	'*****************************************************************************
	' Objects and Properties
	'*****************************************************************************
	'Moved to GIIConstants
	'    Public Const ACScreenTypeNoQuotes = 1
	'    Public Const ACScreenTypeLoadInterface = 2
	
	' Broker Details
	Public Const GIIHBrokerDetails As String = "Broker_Details"
	
	Public Const GIIHBDSendMailBox As String = "Send_Mail_Box"
	Public Const GIIHBDSendNetworkId As String = "Send_Network_id"
	Public Const GIIHBDBrokerName As String = "Broker_Name"
	Public Const GIIHBDICCSNo As String = "ICCS_No"
	
	
	' GIIHGemPolicy
	Public Const GIIHGIIHGemPolicy As String = "GIIHGemPolicy"
	
	Public Const GIIHGisPolicyLinkId As String = "gis_policy_link_id"
	
	
	
	' qh_client_dets
	Public Const GIIHQhClientDets As String = "qh_client_dets"
	
	Public Const GIIHCliCode As String = "cli_code"
	Public Const GIIHCliName As String = "cli_name"
	Public Const GIIHCliTitle As String = "cli_title"
	Public Const GIIHCliForename As String = "cli_forename"
	Public Const GIIHCliSurname As String = "cli_surname"
	Public Const GIIHCliAdd1 As String = "cli_add1"
	Public Const GIIHCliAdd2 As String = "cli_add2"
	Public Const GIIHCliAdd3 As String = "cli_add3"
	Public Const GIIHCliAdd4 As String = "cli_add4"
	Public Const GIIHCliPcod As String = "cli_pcod"
	Public Const GIIHCliHomeTeln As String = "cli_home_teln"
	Public Const GIIHCliWorkTeln As String = "cli_work_teln"
	Public Const GIIHCliWorkTelExt As String = "cli_work_tel_ext"
	Public Const GIIHCliSex As String = "cli_sex"
	Public Const GIIHCliMarital As String = "cli_marital"
	Public Const GIIHCliDob As String = "cli_dob"
	Public Const GIIHCliAge As String = "cli_age"
	Public Const GIIHCliOccStat As String = "cli_occ_stat"
	Public Const GIIHCliOccBnet As String = "cli_occ_bnet"
	Public Const GIIHCliBusBnet As String = "cli_bus_bnet"
	Public Const GIIHCliOccFulltimeYn As String = "cli_occ_fulltime_yn"
	Public Const GIIHCliOthOccStat As String = "cli_oth_occ_stat"
	Public Const GIIHCliOthOccBnet As String = "cli_oth_occ_bnet"
	Public Const GIIHCliOthBusBnet As String = "cli_oth_bus_bnet"
	Public Const GIIHCliOthOccFulltimeYn As String = "cli_oth_occ_fulltime_yn"
	Public Const GIIHCliPrevTermYn As String = "cli_prev_term_yn"
	Public Const GIIHCliClaimYn As String = "cli_claim_yn"
	Public Const GIIHCliNdrivConvYn As String = "cli_ndriv_conv_yn"
	Public Const GIIHClaNum As String = "cla_num"
	Public Const GIIHCliNameKey As String = "cli_name_key"
	Public Const GIIHCliPcodKey As String = "cli_pcod_key"
	Public Const GIIHCliDataOrigin As String = "cli_data_origin"
	Public Const GIIHCliMoreClaYn As String = "cli_more_cla_yn"
	
	
	' qh_policy
	Public Const GIIHQhPolicy As String = "qh_policy"
	
	Public Const GIIHPolNumPbItems As String = "pol_num_pb_items"
	Public Const GIIHPolNumCycles As String = "pol_num_cycles"
	Public Const GIIHPolNumHrItems As String = "pol_num_hr_items"
	Public Const GIIHSelectedInsurerCode As String = "selected_insurer_code"
	Public Const GIIHSelectedSchemeNo As String = "selected_scheme_no"
	Public Const GIIHSelectedSchemeFlag As String = "selected_scheme_flag"
	Public Const GIIHPolicyStatus As String = "policy_status"
	Public Const GIIHPartyCnt As String = "party_cnt"
	Public Const GIIHInsuranceFileCnt As String = "insurance_file_cnt"
	Public Const GIIHPolPolnum As String = "pol_polnum"
	Public Const GIIHPolScraftCoverYn As String = "pol_scraft_cover_yn"
	Public Const GIIHPolPersAccCoverYn As String = "pol_pers_acc_cover_yn"
	Public Const GIIHPolLegalCoverYn As String = "pol_legal_cover_yn"
	Public Const GIIHPolAnalysisCode As String = "pol_analysis_code"
	Public Const GIIHPolAnalysisDesc As String = "pol_analysis_desc"
	Public Const GIIHPolJointInsuredYn As String = "pol_joint_insured_yn"
	Public Const GIIHPolEffDate As String = "pol_eff_date"
	Public Const GIIHPolBuildingsCoverYn As String = "pol_buildings_cover_yn"
	Public Const GIIHPolContentsCoverYn As String = "pol_contents_cover_yn"
	Public Const GIIHPolHrContentsYn As String = "pol_hr_contents_yn"
	Public Const GIIHPolPersBelCoverYn As String = "pol_pers_bel_cover_yn"
	Public Const GIIHPolCyclesCoverYn As String = "pol_cycles_cover_yn"
	Public Const GIIHPolCaravanCoverYn As String = "pol_caravan_cover_yn"
	Public Const GIIHPolLoadDate As String = "pol_load_date"
	
	' qh_contents
	Public Const GIIHQhContents As String = "qh_contents"
	
	Public Const GIIHPolContentsSi As String = "pol_contents_si"
	Public Const GIIHPolHighRiskSi As String = "pol_high_risk_si"
	Public Const GIIHPolContentsCover As String = "pol_contents_cover"
	Public Const GIIHPolContentsSetlType As String = "pol_contents_setl_type"
	Public Const GIIHPolDelContentsXsYn As String = "pol_del_contents_xs_yn"
	Public Const GIIHPolContentsVolxs As String = "pol_contents_volxs"
	Public Const GIIHPolContentsExtnsYn As String = "pol_contents_extns_yn"
	Public Const GIIHPolContentsPinsBnet As String = "pol_contents_pins_bnet"
	Public Const GIIHPolContentsPinsPnum As String = "pol_contents_pins_pnum"
	Public Const GIIHPolContentsPinsYrs As String = "pol_contents_pins_yrs"
	Public Const GIIHPolContentsTotalYrs As String = "pol_contents_total_yrs"
	Public Const GIIHPolContentsNcdYrs As String = "pol_contents_ncd_yrs"
	Public Const GIIHPolFreezer As String = "pol_freezer"
	Public Const GIIHPolMoney As String = "pol_money"
	Public Const GIIHPolCredit As String = "pol_credit"
	Public Const GIIHPolContentsPins406Bnet As String = "pol_contents_pins406_bnet"
	Public Const GIIHPolTotCycles As String = "pol_tot_cycles"
	Public Const GIIHPolTotSpecItems As String = "pol_tot_spec_items"
	Public Const GIIHPolScraftSi As String = "pol_scraft_si"
	
	
	' qh_pers_bel
	Public Const GIIHQhPersBel As String = "qh_pers_bel"
	
	Public Const GIIHPolPbItemNum As String = "pol_pb_item_num"
	Public Const GIIHPolPbBnet As String = "pol_pb_bnet"
	Public Const GIIHPolPbSpecYn As String = "pol_pb_spec_yn"
	Public Const GIIHPolPbValue As String = "pol_pb_value"
	Public Const GIIHPolPbItemDesc As String = "pol_pb_item_desc"
	
	
	' qh_buildings
	Public Const GIIHQhBuildings As String = "qh_buildings"
	
	Public Const GIIHProBuildSi As String = "pro_build_si"
	Public Const GIIHPolBuildCover As String = "pol_build_cover"
	Public Const GIIHPolDelBuildXsYn As String = "pol_del_build_xs_yn"
	Public Const GIIHPolBuildVolxs As String = "pol_build_volxs"
	Public Const GIIHPolBuildPinsBnet As String = "pol_build_pins_bnet"
	Public Const GIIHPolBuildPinsPnum As String = "pol_build_pins_pnum"
	Public Const GIIHPolBuildPinsYrs As String = "pol_build_pins_yrs"
	Public Const GIIHPolBuildTotalYrs As String = "pol_build_total_yrs"
	Public Const GIIHPolBuildNcdYrs As String = "pol_build_ncd_yrs"
	Public Const GIIHPolBuildPins406Bnet As String = "pol_build_pins406_bnet"
	
	
	' qh_caravan
	Public Const GIIHQhCaravan As String = "qh_caravan"
	
	Public Const GIIHPolCaravanValue As String = "pol_caravan_value"
	Public Const GIIHPolCaravanContents As String = "pol_caravan_contents"
	Public Const GIIHPolCaravanChassisNo As String = "pol_caravan_chassis_no"
	Public Const GIIHPolCaravanVinNo As String = "pol_caravan_vin_no"
	Public Const GIIHPolCaravanMakeAbi As String = "pol_caravan_make_abi"
	Public Const GIIHPolCaravanMakeDesc As String = "pol_caravan_make_desc"
	Public Const GIIHPolCaravanModel As String = "pol_caravan_model"
	Public Const GIIHPolCaravanTypeBnet As String = "pol_caravan_type_bnet"
	Public Const GIIHPolCaravanYear As String = "pol_caravan_year"
	Public Const GIIHPolCaravanHeatingYn As String = "pol_caravan_heating_yn"
	Public Const GIIHPolCaravanPrice As String = "pol_caravan_price"
	Public Const GIIHPolCaravanPurchDate As String = "pol_caravan_purch_date"
	Public Const GIIHPolCaravanHireYn As String = "pol_caravan_hire_yn"
	Public Const GIIHPolCaravanPermHomeYn As String = "pol_caravan_perm_home_yn"
	Public Const GIIHPolCaravanLocationBnet As String = "pol_caravan_location_bnet"
	
	
	' qh_caravan_address
	Public Const GIIHQhCaravanAddress As String = "qh_caravan_address"
	
	Public Const GIIHPolCaravanAdd1 As String = "pol_caravan_add1"
	Public Const GIIHPolCaravanAdd2 As String = "pol_caravan_add2"
	Public Const GIIHPolCaravanAdd3 As String = "pol_caravan_add3"
	Public Const GIIHPolCaravanAdd4 As String = "pol_caravan_add4"
	Public Const GIIHPolCaravanPcod As String = "pol_caravan_pcod"
	
	
	' qh_claims
	Public Const GIIHQhClaims As String = "qh_claims"
	
	Public Const GIIHClaDate As String = "cla_date"
	Public Const GIIHClaPeril As String = "cla_peril"
	Public Const GIIHClaSection As String = "cla_section"
	Public Const GIIHClaValue As String = "cla_value"
	Public Const GIIHClaClaimedYn As String = "cla_claimed_yn"
	Public Const GIIHClaTextDesc As String = "cla_text_desc"
	Public Const GIIHClaInsBnet As String = "cla_ins_bnet"
	Public Const GIIHClaPolNum As String = "cla_pol_num"
	
	
	' qh_construction_location
	Public Const GIIHQhConstructionLocation As String = "qh_construction_location"
	
	Public Const GIIHProWallConBnet As String = "pro_wall_con_bnet"
	Public Const GIIHProRoofConBnet As String = "pro_roof_con_bnet"
	Public Const GIIHProFlatRoofYn As String = "pro_flat_roof_yn"
	Public Const GIIHProFlatRoofPerc As String = "pro_flat_roof_perc"
	Public Const GIIHProFlatRoofYear As String = "pro_flat_roof_year"
	Public Const GIIHProGargTypeBnet As String = "pro_garg_type_bnet"
	Public Const GIIHProGoodRepYn As String = "pro_good_rep_yn"
	Public Const GIIHProFloodFreeYn As String = "pro_flood_free_yn"
	Public Const GIIHProSubsFreeYn As String = "pro_subs_free_yn"
	Public Const GIIHProSelfContYn As String = "pro_self_cont_yn"
	Public Const GIIHProUnderpinnedYn As String = "pro_underpinned_yn"
	Public Const GIIHProNearCliffYn As String = "pro_near_cliff_yn"
	Public Const GIIHProDistToCliff As String = "pro_dist_to_cliff"
	Public Const GIIHProListedBnet As String = "pro_listed_bnet"
	Public Const GIIHProListedFreeText As String = "pro_listed_free_text"
	
	
	' qh_cycles
	Public Const GIIHQhCycles As String = "qh_cycles"
	
	Public Const GIIHPolCycleNum As String = "pol_cycle_num"
	Public Const GIIHPolCycleDesc As String = "pol_cycle_desc"
	Public Const GIIHPolCycleValue As String = "pol_cycle_value"
	
	
	' qh_partner
	Public Const GIIHQhPartner As String = "qh_partner"
	
	Public Const GIIHParTitle As String = "par_title"
	Public Const GIIHParForename As String = "par_forename"
	Public Const GIIHParSurname As String = "par_surname"
	Public Const GIIHParSex As String = "par_sex"
	Public Const GIIHParRelatToCli As String = "par_relat_to_cli"
	Public Const GIIHParDob As String = "par_dob"
	Public Const GIIHParAge As String = "par_age"
	Public Const GIIHParWorkTeln As String = "par_work_teln"
	Public Const GIIHParOccStat As String = "par_occ_stat"
	Public Const GIIHParOccBnet As String = "par_occ_bnet"
	Public Const GIIHParBusBnet As String = "par_bus_bnet"
	Public Const GIIHParOccFulltimeYn As String = "par_occ_fulltime_yn"
	Public Const GIIHParOthOccStat As String = "par_oth_occ_stat"
	Public Const GIIHParOthOccBnet As String = "par_oth_occ_bnet"
	Public Const GIIHParOthBusBnet As String = "par_oth_bus_bnet"
	Public Const GIIHParOthOccFulltimeYn As String = "par_oth_occ_fulltime_yn"
	
	
	' qh_property
	Public Const GIIHQhProperty As String = "qh_property"
	
	Public Const GIIHProAdd1 As String = "pro_add1"
	Public Const GIIHProAdd2 As String = "pro_add2"
	Public Const GIIHProAdd3 As String = "pro_add3"
	Public Const GIIHProAdd4 As String = "pro_add4"
	Public Const GIIHProPcode As String = "pro_pcode"
	Public Const GIIHProFtbYn As String = "pro_ftb_yn"
	Public Const GIIHProTypeBnet As String = "pro_type_bnet"
	Public Const GIIHProNoBeds As String = "pro_no_beds"
	Public Const GIIHProBuilt As String = "pro_built"
	Public Const GIIHProResStatBnet As String = "pro_res_stat_bnet"
	Public Const GIIHProOwnerBnet As String = "pro_owner_bnet"
	Public Const GIIHProInterestBnet As String = "pro_interest_bnet"
	Public Const GIIHProChangeAddrYn As String = "pro_change_addr_yn"
	
	
	' qh_residency
	Public Const GIIHQhResidency As String = "qh_residency"
	
	Public Const GIIHProHrsUn As String = "pro_hrs_un"
	Public Const GIIHProNightHrsUn As String = "pro_night_hrs_un"
	Public Const GIIHProDaysUn As String = "pro_days_un"
	Public Const GIIHProOcuTotalNum As String = "pro_ocu_total_num"
	Public Const GIIHProOcuNonfamYn As String = "pro_ocu_nonfam_yn"
	Public Const GIIHProOcuUnder17Num As String = "pro_ocu_under_17_num"
	Public Const GIIHProOcuSmokerYn As String = "pro_ocu_smoker_yn"
	Public Const GIIHProBusUseYn As String = "pro_bus_use_yn"
	Public Const GIIHProBusUseClericalYn As String = "pro_bus_use_clerical_yn"
	Public Const GIIHProBusStockYn As String = "pro_bus_stock_yn"
	Public Const GIIHProBusStockValue As String = "pro_bus_stock_value"
	Public Const GIIHProBusEquipValue As String = "pro_bus_equip_value"
	Public Const GIIHProBusVisitYn As String = "pro_bus_visit_yn"
	
	
	' qh_security
	Public Const GIIHQhSecurity As String = "qh_security"
	
	Public Const GIIHProNwatchYn As String = "pro_nwatch_yn"
	Public Const GIIHProSecLightsYn As String = "pro_sec_lights_yn"
	Public Const GIIHProBarCodedYn As String = "pro_bar_coded_yn"
	Public Const GIIHProSafeYn As String = "pro_safe_yn"
	Public Const GIIHProSmokeDetYn As String = "pro_smoke_det_yn"
	Public Const GIIHProFireBlanketYn As String = "pro_fire_blanket_yn"
	Public Const GIIHProFireExtingYn As String = "pro_fire_exting_yn"
	Public Const GIIHProDogKeptYn As String = "pro_dog_kept_yn"
	Public Const GIIHProSecuExitBnet As String = "pro_secu_exit_bnet"
	Public Const GIIHProSecuSingBnet As String = "pro_secu_sing_bnet"
	Public Const GIIHProSecuDoubBnet As String = "pro_secu_doub_bnet"
	Public Const GIIHProSecuSlidBnet As String = "pro_secu_slid_bnet"
	Public Const GIIHProSecuWindBnet As String = "pro_secu_wind_bnet"
	Public Const GIIHProSecuGargBnet As String = "pro_secu_garg_bnet"
	Public Const GIIHProAlarmBnet As String = "pro_alarm_bnet"
	Public Const GIIHProAlarmProfInstYn As String = "pro_alarm_prof_inst_yn"
	Public Const GIIHProAlarmInstBnet As String = "pro_alarm_inst_bnet"
	Public Const GIIHProAlarmInstDate As String = "pro_alarm_inst_date"
	Public Const GIIHProAlarmMaintYn As String = "pro_alarm_maint_yn"
	
	
	' pol_hr_items
	Public Const GIIHPolHrItems As String = "pol_hr_items"
	
	Public Const GIIHPolHrItemNum As String = "pol_hr_item_num"
	Public Const GIIHPolHrBnet As String = "pol_hr_bnet"
	Public Const GIIHPolHrSpecYn As String = "pol_hr_spec_yn"
	Public Const GIIHPolHrValue As String = "pol_hr_value"
	Public Const GIIHPolHrItemDesc As String = "pol_hr_item_desc"
	
	
	' qh_policy_control
	Public Const GIIHQhPolicyControl As String = "qh_policy_control"
	
	Public Const GIIHConPropSigned As String = "con_prop_signed"
	Public Const GIIHConProofNcdYn As String = "con_proof_ncd_yn"
	Public Const GIIHConModiDate As String = "con_modi_date"
	Public Const GIIHConModiTime As String = "con_modi_time"
	Public Const GIIHConPrinDate As String = "con_prin_date"
	Public Const GIIHConPrinTime As String = "con_prin_time"
	Public Const GIIHPolInsurerName As String = "pol_insurer_name"
	Public Const GIIHPolInsurerCode As String = "pol_insurer_code"
	Public Const GIIHPolSchemeNum As String = "pol_scheme_num"
	Public Const GIIHPolInsVbsNo As String = "pol_ins_vbs_no"
	Public Const GIIHPolTotalAnnIpt As String = "pol_total_ann_ipt"
	Public Const GIIHPolTotalAnnPremium As String = "pol_total_ann_premium"
	Public Const GIIHPolStatus As String = "pol_status"
	Public Const GIIHPolStartDate As String = "pol_start_date"
	Public Const GIIHPolStartTime As String = "pol_start_time"
	Public Const GIIHPolEndDate As String = "pol_end_date"
	Public Const GIIHPolEndTime As String = "pol_end_time"
	Public Const GIIHPolPeriodMonths As String = "pol_period_months"
	Public Const GIIHPolBuildILP As String = "pol_build_ILP"
	Public Const GIIHPolContentsILP As String = "pol_contents_ILP"
	Public Const GIIHPolUnspecItemsILP As String = "pol_unspec_items_ILP"
	Public Const GIIHPolSpecItemsILP As String = "pol_spec_items_ILP"
	Public Const GIIHConEdiLastRcvEvent As String = "con_edi_last_rcv_event"
	Public Const GIIHConEdiSndEvent As String = "con_edi_snd_event"
	Public Const GIIHConEdiNewVerNo As String = "con_edi_new_ver_no"
	Public Const GIIHConEdiOldVerNo As String = "con_edi_old_ver_no"
	Public Const GIIHDocAddRenSchedule As String = "doc_add_ren_schedule"
	
	' screen
	Public Const GIIHScreen As String = "screen"
	
	Public Const GIIHScreenName As String = "screen_name"
	Public Const GIIHScreenStatus As String = "screen_status"
	
	
	'*****************************************************************************
	' Quote Area
	'*****************************************************************************
	Public Const GIIHQuoteBinder As String = "quote_binder"
	' qh_quote_output
	Public Const GIIHQhQuoteOutput As String = "qh_quote_output"
	
	Public Const GIIHStoreInsurerName As String = "store_insurer_name"
	'Public Const STORE_INSURER_NAME = "O518P10258"
	Public Const STORE_INSURER_NAME As String = GIIHQhQuoteOutput & ":" & GIIHStoreInsurerName
	Public Const GIIHStoreSchemeName As String = "store_scheme_name"
	'Public Const STORE_SCHEME_NAME = "O518P10259"
	Public Const STORE_SCHEME_NAME As String = GIIHQhQuoteOutput & ":" & GIIHStoreSchemeName
	Public Const GIIHStoreInsurerCode As String = "store_insurer_code"
	'Public Const STORE_INSURER_CODE = "O518P10260"
	Public Const STORE_INSURER_CODE As String = GIIHQhQuoteOutput & ":" & GIIHStoreInsurerCode
	Public Const GIIHStoreSchemeNumber As String = "store_scheme_number"
	'Public Const STORE_SCHEME_NUMBER = "O518P10261"
	Public Const STORE_SCHEME_NUMBER As String = GIIHQhQuoteOutput & ":" & GIIHStoreSchemeNumber
	Public Const GIIHStoreGStatus As String = "store_g_status"
	'Public Const STORE_G_STATUS = "O518P10262"
	Public Const STORE_G_STATUS As String = GIIHQhQuoteOutput & ":" & GIIHStoreGStatus
	Public Const GIIHStoreVbsStatus As String = "store_vbs_status"
	'Public Const STORE_VBS_STATUS = "O518P10263"
	Public Const STORE_VBS_STATUS As String = GIIHQhQuoteOutput & ":" & GIIHStoreVbsStatus
	Public Const GIIHStoreTotalAnnPremium As String = "store_total_ann_premium"
	'Public Const STORE_TOTAL_ANN_PREMIUM = "O518P10264"
	Public Const STORE_TOTAL_ANN_PREMIUM As String = GIIHQhQuoteOutput & ":" & GIIHStoreTotalAnnPremium
	Public Const GIIHStoreContentsArAnnPrem As String = "store_contents_ar_ann_prem"
	'Public Const STORE_CONTENTS_AR_ANN_PREM = "O518P10265"
	Public Const STORE_CONTENTS_AR_ANN_PREM As String = GIIHQhQuoteOutput & ":" & GIIHStoreContentsArAnnPrem
	Public Const GIIHStoreBuildingsAnnPrem As String = "store_buildings_ann_prem"
	'Public Const STORE_BUILDINGS_ANN_PREM = "O518P10266"
	Public Const STORE_BUILDINGS_ANN_PREM As String = GIIHQhQuoteOutput & ":" & GIIHStoreBuildingsAnnPrem
	Public Const GIIHStoreBuildingsSi As String = "store_buildings_si"
	'Public Const STORE_BUILDINGS_SI = "O518P10267"
	Public Const STORE_BUILDINGS_SI As String = GIIHQhQuoteOutput & ":" & GIIHStoreBuildingsSi
	Public Const GIIHStoreContentsSi As String = "store_contents_si"
	'Public Const STORE_CONTENTS_SI = "O518P10268"
	Public Const STORE_CONTENTS_SI As String = GIIHQhQuoteOutput & ":" & GIIHStoreContentsSi
	Public Const GIIHStoreBuildingsExcess As String = "store_buildings_excess"
	'Public Const STORE_BUILDINGS_EXCESS = "O518P10269"
	Public Const STORE_BUILDINGS_EXCESS As String = GIIHQhQuoteOutput & ":" & GIIHStoreBuildingsExcess
	Public Const GIIHStoreContentsExcess As String = "store_contents_excess"
	'Public Const STORE_CONTENTS_EXCESS = "O518P10270"
	Public Const STORE_CONTENTS_EXCESS As String = GIIHQhQuoteOutput & ":" & GIIHStoreContentsExcess
	Public Const GIIHStoreBuildingsFlag As String = "store_buildings_flag"
	'Public Const STORE_BUILDINGS_FLAG = "O518P10271"
	Public Const STORE_BUILDINGS_FLAG As String = GIIHQhQuoteOutput & ":" & GIIHStoreBuildingsFlag
	Public Const GIIHStoreContentsFlag As String = "store_contents_flag"
	'Public Const STORE_CONTENTS_FLAG = "O518P10272"
	Public Const STORE_CONTENTS_FLAG As String = GIIHQhQuoteOutput & ":" & GIIHStoreContentsFlag
	Public Const GIIHGisSchemeId As String = "gis_scheme_id"
	'Public Const GIS_SCHEME_ID_SURVEY = "O518P20696"
	Public Const GIS_SCHEME_ID_SURVEY As String = GIIHQhQuoteOutput & ":" & GIIHGisSchemeId
	
	' qh_quote_out
	Public Const GIIHQhQuoteOut As String = "qh_quote_out"
	
	Public Const GIIHOutInsurerName As String = "out_insurer_name"
	'Public Const OUT_INSURER_NAME = "O519P10273"
	Public Const OUT_INSURER_NAME As String = GIIHQhQuoteOut & ":" & GIIHOutInsurerName
	Public Const GIIHOutSchemeName As String = "out_scheme_name"
	'Public Const OUT_SCHEME_NAME = "O519P10274"
	Public Const OUT_SCHEME_NAME As String = GIIHQhQuoteOut & ":" & GIIHOutSchemeName
	Public Const GIIHOutSchemeNum As String = "out_scheme_num"
	'Public Const OUT_SCHEME_NUM = "O519P10455"
	Public Const OUT_SCHEME_NUM As String = GIIHQhQuoteOut & ":" & GIIHOutSchemeNum
	Public Const GIIHOutInsurerCode As String = "out_insurer_code"
	'Public Const OUT_INSURER_CODE = "O519P10452"
	Public Const OUT_INSURER_CODE As String = GIIHQhQuoteOut & ":" & GIIHOutInsurerCode
	Public Const GIIHQhoVbsStatus As String = "qho_vbs_status"
	'Public Const QHO_VBS_STATUS = "O519P10453"
	Public Const QHO_VBS_STATUS As String = GIIHQhQuoteOut & ":" & GIIHQhoVbsStatus
	Public Const GIIHQhoGStatus As String = "qho_g_status"
	'Public Const QHO_G_STATUS = "O519P10454"
	Public Const QHO_G_STATUS As String = GIIHQhQuoteOut & ":" & GIIHQhoGStatus
	
	Public Const GIIHOutNumSections As String = "out_num_sections"
	'Public Const OUT_NUM_SECTIONS = "O519P10275"
	Public Const OUT_NUM_SECTIONS As String = GIIHQhQuoteOut & ":" & GIIHOutNumSections
	Public Const GIIHOutNumAnalysis As String = "out_num_analysis"
	'Public Const OUT_NUM_ANALYSIS = "O519P10276"
	Public Const OUT_NUM_ANALYSIS As String = GIIHQhQuoteOut & ":" & GIIHOutNumAnalysis
	Public Const GIIHOutNumMessages As String = "out_num_messages"
	'Public Const OUT_NUM_MESSAGES = "O519P10277"
	Public Const OUT_NUM_MESSAGES As String = GIIHQhQuoteOut & ":" & GIIHOutNumMessages
	Public Const GIIHOutTotalAnnGross As String = "out_total_ann_gross"
	'Public Const OUT_TOTAL_ANN_GROSS = "O519P10278"
	Public Const OUT_TOTAL_ANN_GROSS As String = GIIHQhQuoteOut & ":" & GIIHOutTotalAnnGross
	Public Const GIIHOutContentsAnnGross As String = "out_contents_ann_gross"
	'Public Const OUT_CONTENTS_ANN_GROSS = "O519P10279"
	Public Const OUT_CONTENTS_ANN_GROSS As String = GIIHQhQuoteOut & ":" & GIIHOutContentsAnnGross
	Public Const GIIHOutBuildingsAnnGross As String = "out_buildings_ann_gross"
	'Public Const OUT_BUILDINGS_ANN_GROSS = "O519P10280"
	Public Const OUT_BUILDINGS_ANN_GROSS As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsAnnGross
	Public Const GIIHOutTotalAnnIpt As String = "out_total_ann_ipt"
	'Public Const OUT_TOTAL_ANN_IPT = "O519P10281"
	Public Const OUT_TOTAL_ANN_IPT As String = GIIHQhQuoteOut & ":" & GIIHOutTotalAnnIpt
	Public Const GIIHOutContentsAnnIpt As String = "out_contents_ann_ipt"
	'Public Const OUT_CONTENTS_ANN_IPT = "O519P10282"
	Public Const OUT_CONTENTS_ANN_IPT As String = GIIHQhQuoteOut & ":" & GIIHOutContentsAnnIpt
	Public Const GIIHOutBuildingsAnnIpt As String = "out_buildings_ann_ipt"
	'Public Const OUT_BUILDINGS_ANN_IPT = "O519P10283"
	Public Const OUT_BUILDINGS_ANN_IPT As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsAnnIpt
	Public Const GIIHOutTotalAnnPremium As String = "out_total_ann_premium"
	'Public Const OUT_TOTAL_ANN_PREMIUM = "O519P10284"
	Public Const OUT_TOTAL_ANN_PREMIUM As String = GIIHQhQuoteOut & ":" & GIIHOutTotalAnnPremium
	Public Const GIIHOutContentsAnnPrem As String = "out_contents_ann_prem"
	'Public Const OUT_CONTENTS_ANN_PREM = "O519P10285"
	Public Const OUT_CONTENTS_ANN_PREM As String = GIIHQhQuoteOut & ":" & GIIHOutContentsAnnPrem
	Public Const GIIHOutBuildingsAnnPrem As String = "out_buildings_ann_prem"
	'Public Const OUT_BUILDINGS_ANN_PREM = "O519P10286"
	Public Const OUT_BUILDINGS_ANN_PREM As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsAnnPrem
	Public Const GIIHOutContentsOnlyAnnPrem As String = "out_contents_only_ann_prem"
	'Public Const OUT_CONTENTS_ONLY_ANN_PREM = "O519P10287"
	Public Const OUT_CONTENTS_ONLY_ANN_PREM As String = GIIHQhQuoteOut & ":" & GIIHOutContentsOnlyAnnPrem
	Public Const GIIHOutBuildingsOnlyAnnPrem As String = "out_buildings_only_ann_prem"
	'Public Const OUT_BUILDINGS_ONLY_ANN_PREM = "O519P10288"
	Public Const OUT_BUILDINGS_ONLY_ANN_PREM As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsOnlyAnnPrem
	Public Const GIIHOutBuildingsExcess As String = "out_buildings_excess"
	'Public Const OUT_BUILDINGS_EXCESS = "O519P10289"
	Public Const OUT_BUILDINGS_EXCESS As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsExcess
	Public Const GIIHOutContentsExcess As String = "out_contents_excess"
	'Public Const OUT_CONTENTS_EXCESS = "O519P10290"
	Public Const OUT_CONTENTS_EXCESS As String = GIIHQhQuoteOut & ":" & GIIHOutContentsExcess
	Public Const GIIHOutSubsidenceExcess As String = "out_subsidence_excess"
	'Public Const OUT_SUBSIDENCE_EXCESS = "O519P10291"
	Public Const OUT_SUBSIDENCE_EXCESS As String = GIIHQhQuoteOut & ":" & GIIHOutSubsidenceExcess
	Public Const GIIHOutBuildVolExcess As String = "out_build_vol_excess"
	'    Public Const OUT_BUILD_VOL_EXCESS = "O519P10292"
	Public Const OUT_BUILD_VOL_EXCESS As String = GIIHQhQuoteOut & ":" & GIIHOutBuildVolExcess
	Public Const GIIHOutContVolExcess As String = "out_cont_vol_excess"
	'Public Const OUT_CONT_VOL_EXCESS = "O519P10293"
	Public Const OUT_CONT_VOL_EXCESS As String = GIIHQhQuoteOut & ":" & GIIHOutContVolExcess
	Public Const GIIHOutBuildingsFlag As String = "out_buildings_flag"
	'Public Const OUT_BUILDINGS_FLAG = "O519P10294"
	Public Const OUT_BUILDINGS_FLAG As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsFlag
	Public Const GIIHOutContentsFlag As String = "out_contents_flag"
	'    Public Const OUT_CONTENTS_FLAG = "O519P10295"
	Public Const OUT_CONTENTS_FLAG As String = GIIHQhQuoteOut & ":" & GIIHOutContentsFlag
	Public Const GIIHOutBuildArea As String = "out_build_area"
	'    Public Const OUT_BUILD_AREA = "O519P10296"
	Public Const OUT_BUILD_AREA As String = GIIHQhQuoteOut & ":" & GIIHOutBuildArea
	Public Const GIIHOutContArea As String = "out_cont_area"
	'    Public Const OUT_CONT_AREA = "O519P10297"
	Public Const OUT_CONT_AREA As String = GIIHQhQuoteOut & ":" & GIIHOutContArea
	Public Const GIIHOutBuildRateDate As String = "out_build_rate_date"
	'Public Const OUT_BUILD_RATE_DATE = "O519P10298"
	Public Const OUT_BUILD_RATE_DATE As String = GIIHQhQuoteOut & ":" & GIIHOutBuildRateDate
	Public Const GIIHOutContRateDate As String = "out_cont_rate_date"
	'    Public Const OUT_CONT_RATE_DATE = "O519P10299"
	Public Const OUT_CONT_RATE_DATE As String = GIIHQhQuoteOut & ":" & GIIHOutContRateDate
	Public Const GIIHOutBuildSubsExcess As String = "out_build_subs_excess"
	'    Public Const OUT_BUILD_SUBS_EXCESS = "O519P10300"
	Public Const OUT_BUILD_SUBS_EXCESS As String = GIIHQhQuoteOut & ":" & GIIHOutBuildSubsExcess
	Public Const GIIHOutBuildQuoteStatus As String = "out_build_quote_status"
	'Public Const OUT_BUILD_QUOTE_STATUS = "O519P10301"
	Public Const OUT_BUILD_QUOTE_STATUS As String = GIIHQhQuoteOut & ":" & GIIHOutBuildQuoteStatus
	Public Const GIIHOutContSecuClass As String = "out_cont_secu_class"
	'Public Const OUT_CONT_SECU_CLASS = "O519P10302"
	Public Const OUT_CONT_SECU_CLASS As String = GIIHQhQuoteOut & ":" & GIIHOutContSecuClass
	Public Const GIIHOutContQuoteStatus As String = "out_cont_quote_status"
	' Public Const OUT_CONT_QUOTE_STATUS = "O519P10303"
	Public Const OUT_CONT_QUOTE_STATUS As String = GIIHQhQuoteOut & ":" & GIIHOutContQuoteStatus
	Public Const GIIHOutContMinSecuFlag As String = "out_cont_min_secu_flag"
	'Public Const OUT_CONT_MIN_SECU_FLAG = "O519P10304"
	Public Const OUT_CONT_MIN_SECU_FLAG As String = GIIHQhQuoteOut & ":" & GIIHOutContMinSecuFlag
	'    Public Const GIS_SCHEME_ID_BREAKDOWN = "O518P20697"
	Public Const GIS_SCHEME_ID_BREAKDOWN As String = GIIHQhQuoteOut & ":" & GIIHGisSchemeId
	
	Public Const GIIHOutBuildingsSi As String = "out_buildings_si"
	Public Const GIIHOutContentsSi As String = "out_contents_si"
	Public Const GIIHOutCyclesSi As String = "out_cycles_si"
	Public Const GIIHOutMoneySi As String = "out_money_si"
	Public Const GIIHOutCreditSi As String = "out_credit_si"
	Public Const GIIHOutFreezerSi As String = "out_freezer_si"
	Public Const GIIHOutCaravanSi As String = "out_caravan_si"
	Public Const GIIHOutLegalSi As String = "out_legal_si"
	Public Const GIIHOutPbSpecSi As String = "out_pb_spec_si"
	Public Const GIIHOutPbUnspecSi As String = "out_pb_unspec_si"
	Public Const GIIHOutHrSi As String = "out_hr_si"
	
	'SJ 09/11/2004 - start
	Public Const GIIHOutLegalAnnPremium As String = "out_legal_ann_premium"
	Public Const GIIHOutAdminAnnPremium As String = "out_admin_ann_premium"
	Public Const GIIHOutPbAnnPremium As String = "out_pb_ann_premium"
	Public Const GIIHOutSportsSi As String = "out_sports_si"
	Public Const GIIHOutMoneyExcess As String = "out_money_excess"
	Public Const GIIHOutCreditExcess As String = "out_credit_excess"
	Public Const GIIHOutFreezerExcess As String = "out_freezer_excess"
	Public Const GIIHOutCaravanExcess As String = "out_caravan_excess"
	Public Const GIIHOutLegalExcess As String = "out_legal_excess"
	Public Const GIIHOutCyclesExcess As String = "out_cycles_excess"
	Public Const GIIHOutBuildFloodExcess As String = "out_build_flood_excess"
	Public Const GIIHOutContsFloodExcess As String = "out_conts_flood_excess"
	Public Const GIIHOutOccupationFlag As String = "out_occupation_flag"
	Public Const GIIHOutNcdPercentage As String = "out_ncd_percentage"
	Public Const GIIHOutIntroPercentage As String = "out_intro_percentage"
	Public Const GIIHOutUslSpecRate As String = "out_usl_spec_rate"
	Public Const GIIHOutUslUnSpectRate As String = "out_usl_unspec_rate"
	Public Const GIIHOutLocksPercentage As String = "out_locks_percentage"
	Public Const GIIHOutAlarmPercentage As String = "out_alarm_percentage"
	Public Const GIIHOutAlarmRequiredFlag As String = "out_alarm_required_flag"
	Public Const GIIHOutAlarmInstalledFlag As String = "out_alarm_installed_flag"
	Public Const GIIHOutLocksRequiredFlag As String = "out_locks_required_flag"
	Public Const GIIHOutLocksInstalledFlag As String = "out_locks_installed_flag"
	Public Const GIIHOutBuildNcdPerc As String = "out_build_ncd_perc"
	Public Const GIIHOutContsNcdPerc As String = "out_conts_ncd_perc"
	Public Const GIIHOutCombinedPerc As String = "out_combined_perc"
	Public Const GIIHOutSafeRequiredFlag As String = "out_safe_required_flag"
	Public Const GIIHOutValPerc As String = "out_val_perc"
	Public Const GIIHOutItemPerc As String = "out_item_perc"
	Public Const GIIHQhoPolarisStatus As String = "QHO_POLARIS_STATUS"
	
	Public Const out_legal_ann_premium As String = GIIHQhQuoteOut & ":" & GIIHOutLegalAnnPremium
	Public Const out_admin_ann_premium As String = GIIHQhQuoteOut & ":" & GIIHOutAdminAnnPremium
	Public Const out_pb_ann_premium As String = GIIHQhQuoteOut & ":" & GIIHOutPbAnnPremium
	Public Const out_sports_si As String = GIIHQhQuoteOut & ":" & GIIHOutSportsSi
	Public Const out_money_excess As String = GIIHQhQuoteOut & ":" & GIIHOutMoneyExcess
	Public Const out_credit_excess As String = GIIHQhQuoteOut & ":" & GIIHOutCreditExcess
	Public Const out_freezer_excess As String = GIIHQhQuoteOut & ":" & GIIHOutFreezerExcess
	Public Const out_caravan_excess As String = GIIHQhQuoteOut & ":" & GIIHOutCaravanExcess
	Public Const out_legal_excess As String = GIIHQhQuoteOut & ":" & GIIHOutLegalExcess
	Public Const out_cycles_excess As String = GIIHQhQuoteOut & ":" & GIIHOutCyclesExcess
	Public Const out_build_flood_excess As String = GIIHQhQuoteOut & ":" & GIIHOutBuildFloodExcess
	Public Const out_conts_flood_excess As String = GIIHQhQuoteOut & ":" & GIIHOutContsFloodExcess
	Public Const out_occupation_flag As String = GIIHQhQuoteOut & ":" & GIIHOutOccupationFlag
	Public Const out_ncd_percentage As String = GIIHQhQuoteOut & ":" & GIIHOutNcdPercentage
	Public Const out_intro_percentage As String = GIIHQhQuoteOut & ":" & GIIHOutIntroPercentage
	Public Const out_usl_spec_rate As String = GIIHQhQuoteOut & ":" & GIIHOutUslSpecRate
	Public Const out_usl_unspec_rate As String = GIIHQhQuoteOut & ":" & GIIHOutUslUnSpectRate
	Public Const out_locks_percentage As String = GIIHQhQuoteOut & ":" & GIIHOutLocksPercentage
	Public Const out_alarm_percentage As String = GIIHQhQuoteOut & ":" & GIIHOutAlarmPercentage
	Public Const out_alarm_required_flag As String = GIIHQhQuoteOut & ":" & GIIHOutAlarmRequiredFlag
	Public Const out_alarm_installed_flag As String = GIIHQhQuoteOut & ":" & GIIHOutAlarmInstalledFlag
	Public Const out_locks_required_flag As String = GIIHQhQuoteOut & ":" & GIIHOutLocksRequiredFlag
	Public Const out_locks_installed_flag As String = GIIHQhQuoteOut & ":" & GIIHOutLocksInstalledFlag
	Public Const out_build_ncd_perc As String = GIIHQhQuoteOut & ":" & GIIHOutBuildNcdPerc
	Public Const out_conts_ncd_perc As String = GIIHQhQuoteOut & ":" & GIIHOutContsNcdPerc
	Public Const out_combined_perc As String = GIIHQhQuoteOut & ":" & GIIHOutCombinedPerc
	Public Const out_safe_required_flag As String = GIIHQhQuoteOut & ":" & GIIHOutSafeRequiredFlag
	Public Const out_val_perc As String = GIIHQhQuoteOut & ":" & GIIHOutValPerc
	Public Const out_item_perc As String = GIIHQhQuoteOut & ":" & GIIHOutItemPerc
	Public Const out_commission_rate As String = GIIHQhQuoteOut & ":" & "commission_rate"
	Public Const out_commission_value As String = GIIHQhQuoteOut & ":" & "commission_value"
	Public Const out_commission_minimum_total As String = GIIHQhQuoteOut & ":" & "commission_minimum_total"
	'SJ 09/11/2004 - end
	
	
	
	'    Public Const OUT_BUILDINGS_SI = "O519P10381"
	'    Public Const OUT_CONTENTS_SI = "O519P10382"
	'    Public Const OUT_CYCLES_SI = "O519P10383"
	'    Public Const OUT_MONEY_SI = "O519P10384"
	'    Public Const OUT_CREDIT_SI = "O519P10385"
	'    Public Const OUT_FREEZER_SI = "O519P10386"
	'    Public Const OUT_CARAVAN_SI = "O519P10387"
	'    Public Const OUT_LEGAL_SI = "O519P10388"
	'    Public Const OUT_PB_SPEC_SI = "O519P10389"
	'    Public Const OUT_PB_UNSPEC_SI = "O519P10390"
	'    Public Const OUT_HR_SI = "O519P10391"
	
	Public Const OUT_BUILDINGS_SI As String = GIIHQhQuoteOut & ":" & GIIHOutBuildingsSi
	Public Const OUT_CONTENTS_SI As String = GIIHQhQuoteOut & ":" & GIIHOutContentsSi
	Public Const OUT_CYCLES_SI As String = GIIHQhQuoteOut & ":" & GIIHOutCyclesSi
	Public Const OUT_MONEY_SI As String = GIIHQhQuoteOut & ":" & GIIHOutMoneySi
	Public Const OUT_CREDIT_SI As String = GIIHQhQuoteOut & ":" & GIIHOutCreditSi
	Public Const OUT_FREEZER_SI As String = GIIHQhQuoteOut & ":" & GIIHOutFreezerSi
	Public Const OUT_CARAVAN_SI As String = GIIHQhQuoteOut & ":" & GIIHOutCaravanSi
	Public Const OUT_LEGAL_SI As String = GIIHQhQuoteOut & ":" & GIIHOutLegalSi
	Public Const OUT_PB_SPEC_SI As String = GIIHQhQuoteOut & ":" & GIIHOutPbSpecSi
	Public Const OUT_PB_UNSPEC_SI As String = GIIHQhQuoteOut & ":" & GIIHOutPbUnspecSi
	Public Const OUT_HR_SI As String = GIIHQhQuoteOut & ":" & GIIHOutHrSi
	
	Public Const GIIHOutFreezerAnnPremium As String = "out_freezer_ann_premium"
	Public Const GIIHOutCaravanAnnPremium As String = "out_caravan_ann_premium"
	Public Const GIIHOutMoneyAnnPremium As String = "out_money_ann_premium"
	Public Const GIIHOutCreditAnnPremium As String = "out_credit_ann_premium"
	Public Const GIIHOutCyclesAnnPremium As String = "out_cycles_ann_premium"
	
	'    Public Const OUT_FREEZER_ANN_PREMIUM = "O519P10418"
	'    Public Const OUT_CARAVAN_ANN_PREMIUM = "O519P10419"
	'    Public Const OUT_MONEY_ANN_PREMIUM = "O519P10420"
	'    Public Const OUT_CREDIT_ANN_PREMIUM = "O519P10421"
	'    Public Const OUT_CYCLES_ANN_PREMIUM = "O519P10422"
	
	Public Const OUT_FREEZER_ANN_PREMIUM As String = GIIHQhQuoteOut & ":" & GIIHOutFreezerAnnPremium
	Public Const OUT_CARAVAN_ANN_PREMIUM As String = GIIHQhQuoteOut & ":" & GIIHOutCaravanAnnPremium
	Public Const OUT_MONEY_ANN_PREMIUM As String = GIIHQhQuoteOut & ":" & GIIHOutMoneyAnnPremium
	Public Const OUT_CREDIT_ANN_PREMIUM As String = GIIHQhQuoteOut & ":" & GIIHOutCreditAnnPremium
	Public Const OUT_CYCLES_ANN_PREMIUM As String = GIIHQhQuoteOut & ":" & GIIHOutCyclesAnnPremium
	Public Const OriginalPremium As String = "Original_Premium"
	Public Const RecalculatedOldRiskPremium As String = "Recalculated_Old_Risk_Premium"
	Public Const NewRiskCalculatedPremium As String = "New_Risk_Calculated_Premium"
	Public Const AdjustmentPremiumInclIPT As String = "Adjustment_Premium_incl_IPT"
	Public Const AdjustmentPremiumExclIPT As String = "Adjustment_Premium_excl_IPT"
	
	Public Const GIIHOutGisSchemeId As String = "out_gis_scheme_id"
	'Public Const OUT_GIS_SCHEME_ID = "O519P10451"
	Public Const OUT_GIS_SCHEME_ID As String = GIIHQhQuoteOut & ":" & GIIHOutGisSchemeId
	
	' qho_section_line
	Public Const GIIHQhoSectionLine As String = "qho_section_line"
	
	Public Const GIIHOutSlSectCode As String = "out_sl_sect_code"
	'Public Const OUT_SL_SECT_CODE = "O520P10305"
	Public Const OUT_SL_SECT_CODE As String = GIIHQhoSectionLine & ":" & GIIHOutSlSectCode
	Public Const GIIHOutSlCompXs As String = "out_sl_comp_xs"
	'Public Const OUT_SL_COMP_XS = "O520P10306"
	Public Const OUT_SL_COMP_XS As String = GIIHQhoSectionLine & ":" & GIIHOutSlCompXs
	Public Const GIIHOutSlVolXs As String = "out_sl_vol_xs"
	'Public Const OUT_SL_VOL_XS = "O520P10307"
	Public Const OUT_SL_VOL_XS As String = GIIHQhoSectionLine & ":" & GIIHOutSlVolXs
	Public Const GIIHOutSlSumIns As String = "out_sl_sum_ins"
	'Public Const OUT_SL_SUM_INS = "O520P10308"
	Public Const OUT_SL_SUM_INS As String = GIIHQhoSectionLine & ":" & GIIHOutSlSumIns
	Public Const GIIHOutSlPremium As String = "out_sl_premium"
	'Public Const OUT_SL_PREMIUM = "O520P10309"
	Public Const OUT_SL_PREMIUM As String = GIIHQhoSectionLine & ":" & GIIHOutSlPremium
	Public Const GIIHOutSlStatusFlag As String = "out_sl_status_flag"
	'Public Const OUT_SL_STATUS_FLAG = "O520P10310"
	Public Const OUT_SL_STATUS_FLAG As String = GIIHQhoSectionLine & ":" & GIIHOutSlStatusFlag
	
	' qho_analysis_line
	Public Const GIIHQhoAnalysisLine As String = "qho_analysis_line"
	
	Public Const GIIHOutAlSectCode As String = "out_al_sect_code"
	'Public Const OUT_AL_SECT_CODE = "O521P10311"
	Public Const OUT_AL_SECT_CODE As String = GIIHQhoAnalysisLine & ":" & GIIHOutAlSectCode
	Public Const GIIHOutAlDescCode As String = "out_al_desc_code"
	Public Const GIIHOutAlDescription As String = "OUT_AL_DESCRIPTION"
	'Public Const OUT_AL_DESC_CODE = "O521P10312"
	Public Const OUT_AL_DESC_CODE As String = GIIHQhoAnalysisLine & ":" & GIIHOutAlDescCode
	Public Const GIIHOutA1Amount As String = "out_a1_amount"
	'Public Const OUT_A1_AMOUNT = "O521P10313"
	Public Const OUT_A1_AMOUNT As String = GIIHQhoAnalysisLine & ":" & GIIHOutA1Amount
	Public Const GIIHOutA1Total As String = "out_a1_total"
	'Public Const OUT_A1_TOTAL = "O521P10314"
	Public Const OUT_A1_TOTAL As String = GIIHQhoAnalysisLine & ":" & GIIHOutA1Total
	
	
	' qho_message_line
	Public Const GIIHQhoMessageLine As String = "qho_message_line"
	
	Public Const GIIHOutMlSectCode As String = "out_ml_sect_code"
	'Public Const OUT_ML_SECT_CODE = "O522P10315"
	Public Const OUT_ML_SECT_CODE As String = GIIHQhoMessageLine & ":" & GIIHOutMlSectCode
	Public Const GIIHOutMlMessCode As String = "out_ml_mess_code"
	Public Const GIIHOutMlMessDescription As String = "OUT_ML_MESS_DESCRIPTION"
	'Public Const OUT_ML_MESS_CODE = "O522P10316"
	Public Const OUT_ML_MESS_CODE As String = GIIHQhoMessageLine & ":" & GIIHOutMlMessCode
	Public Const GIIHOutMlMessAmount As String = "out_ml_mess_amount"
	'Public Const OUT_ML_MESS_AMOUNT = "O522P10317"
	Public Const OUT_ML_MESS_AMOUNT As String = GIIHQhoMessageLine & ":" & GIIHOutMlMessAmount
	Public Const GIIHOutMlStatusFlag As String = "out_ml_status_flag"
	'Public Const OUT_ML_STATUS_FLAG = "O522P10318"
	Public Const OUT_ML_STATUS_FLAG As String = GIIHQhoMessageLine & ":" & GIIHOutMlStatusFlag
	
	' out_cycles
	Public Const GIIHOutCycles As String = "out_cycles"
	
	Public Const GIIHOutCycleValue As String = "out_cycle_value"
	Public Const GIIHOutCyclePremium As String = "out_cycle_premium"
	Public Const GIIHOutCycleSpecYn As String = "out_cycle_spec_yn"
	Public Const GIIHOutCycleStatusFlag As String = "out_cycle_status_flag"
	'    Public Const OUT_CYCLE_VALUE = "O529P10403"
	'    Public Const OUT_CYCLE_PREMIUM = "O529P10404"
	'    Public Const OUT_CYCLE_SPEC_YN = "O529P10405"
	'    Public Const OUT_CYCLE_STATUS_FLAG = "O529P10406"
	'
	Public Const OUT_CYCLE_VALUE As String = GIIHOutCycles & ":" & GIIHOutCycleValue
	Public Const OUT_CYCLE_PREMIUM As String = GIIHOutCycles & ":" & GIIHOutCyclePremium
	Public Const OUT_CYCLE_SPEC_YN As String = GIIHOutCycles & ":" & GIIHOutCycleSpecYn
	Public Const OUT_CYCLE_STATUS_FLAG As String = GIIHOutCycles & ":" & GIIHOutCycleStatusFlag
	
	'out_hr_items
	Public Const GIIHOutHrItems As String = "out_hr_items"
	
	Public Const GIIHOutHrBnet As String = "out_hr_bnet"
	Public Const GIIHOutHrValue As String = "out_hr_value"
	Public Const GIIHOutHrSpecYn As String = "out_hr_spec_yn"
	Public Const GIIHOutHrPremium As String = "out_hr_premium"
	Public Const GIIHOutHrStatusFlag As String = "out_hr_status_flag"
	'    Public Const OUT_HR_BNET = "O530P10407"
	'    Public Const OUT_HR_VALUE = "O530P10408"
	'    Public Const OUT_HR_SPEC_YN = "O530P10409"
	'    Public Const OUT_HR_PREMIUM = "O530P10410"
	'    Public Const OUT_HR_STATUS_FLAG = "O530P10411"
	
	Public Const OUT_HR_BNET As String = GIIHOutHrItems & ":" & GIIHOutHrBnet
	Public Const OUT_HR_VALUE As String = GIIHOutHrItems & ":" & GIIHOutHrValue
	Public Const OUT_HR_SPEC_YN As String = GIIHOutHrItems & ":" & GIIHOutHrSpecYn
	Public Const OUT_HR_PREMIUM As String = GIIHOutHrItems & ":" & GIIHOutHrPremium
	Public Const OUT_HR_STATUS_FLAG As String = GIIHOutHrItems & ":" & GIIHOutHrStatusFlag
	
	'out_PB_items
	Public Const GIIHOutPbItems As String = "out_pb_items"
	
	Public Const GIIHOutPbBnet As String = "out_pb_bnet"
	Public Const GIIHOutPbValue As String = "out_pb_value"
	Public Const GIIHOutPbCompXs As String = "out_pb_comp_xs"
	Public Const GIIHOutPbSpecYn As String = "out_pb_spec_yn"
	Public Const GIIHOutPbPremium As String = "out_pb_premium"
	Public Const GIIHOutPbStatusFlag As String = "out_pb_status_flag"
	'    Public Const OUT_PB_BNET = "O531P10412"
	'    Public Const OUT_PB_VALUE = "O531P10413"
	'    Public Const OUT_PB_COMP_XS = "O531P10414"
	'    Public Const OUT_PB_SPEC_YN = "O531P10415"
	'    Public Const OUT_PB_PREMIUM = "O531P10416"
	'    Public Const OUT_PB_STATUS_FLAG = "O531P10417"
	
	Public Const OUT_PB_BNET As String = GIIHOutPbItems & ":" & GIIHOutPbBnet
	Public Const OUT_PB_VALUE As String = GIIHOutPbItems & ":" & GIIHOutPbValue
	Public Const OUT_PB_COMP_XS As String = GIIHOutPbItems & ":" & GIIHOutPbCompXs
	Public Const OUT_PB_SPEC_YN As String = GIIHOutPbItems & ":" & GIIHOutPbSpecYn
	Public Const OUT_PB_PREMIUM As String = GIIHOutPbItems & ":" & GIIHOutPbPremium
	Public Const OUT_PB_STATUS_FLAG As String = GIIHOutPbItems & ":" & GIIHOutPbStatusFlag
	
	'selected_add_on
	Public Const GIIHSelectedAddOn As String = "selected_add_on"
	
	Public Const GIIHAddOnDefId As String = "add_on_def_id"
	Public Const GIIHAddOnDesc As String = "add_on_desc"
	Public Const GIIHAddOnNetCost As String = "add_on_net_cost"
	Public Const GIIHAddOnGrossCost As String = "add_on_gross_cost"
	Public Const GIIHAddOnScreen As String = "add_on_screen"
	
	' save_quote_out
	Public Const GIIHSaveQuoteOut As String = "save_quote_out"
	
	' save_section_line
	Public Const GIIHSaveSectionLine As String = "save_section_line"
	
	' save_analysis_line
	Public Const GIIHSaveAnalysisLine As String = "save_analysis_line"
	
	' save_message_line
	Public Const GIIHSaveMessageLine As String = "save_message_line"
	
	' save_cycles
	Public Const GIIHSaveCycles As String = "save_cycles"
	
	'save_hr_items
	Public Const GIIHSaveHrItems As String = "save_hr_items"
	
	'save_PB_items
	Public Const GIIHSavePbItems As String = "save_pb_items"
	
	'saved_add_on
	Public Const GIIHSavedAddOn As String = "saved_add_on"
	
	' Quote_Error
	Public Const GIIHQuoteError As String = "Quote_Error"
	
	Public Const GIIHSchemeId As String = "Scheme_Id"
	'Public Const SCHEME_ID = "O527P10373"
	Public Const SCHEME_ID As String = GIIHQuoteError & ":" & GIIHSchemeId
	Public Const GIIHSchemeDescription As String = "Scheme_Description"
	'Public Const SCHEME_DESCRIPTION = "O527P10374"
	Public Const SCHEME_DESCRIPTION As String = GIIHQuoteError & ":" & GIIHSchemeDescription
	
	' Quote_Error_Breakdown
	Public Const GIIHQuoteErrorBreakdown As String = "Quote_Error_Breakdown"
	
	Public Const GIIHLevel As String = "Level"
	'Public Const LEVEL = "O528P10375"
	Public Const LEVEL As String = GIIHQuoteErrorBreakdown & ":" & GIIHLevel
	Public Const GIIHEScreenName As String = "Screen_Name"
	'Public Const SCREEN_NAME = "O528P10376"
	Public Const SCREEN_NAME As String = GIIHQuoteErrorBreakdown & ":" & GIIHEScreenName
	Public Const GIIHDescription As String = "Description"
	'Public Const DESCRIPTION = "O528P10377"
	Public Const DESCRIPTION As String = GIIHQuoteErrorBreakdown & ":" & GIIHDescription
	
	' Underwriting notes
	Public Const GIIHQhUndn As String = "qh_undn"
	
	Public Const GIIHQhUndnText As String = "qh_undn_text"
	'Public Const QhUndnText = "O1949P20571"
	Public Const QhUndnText As String = GIIHQhUndn & ":" & GIIHQhUndnText
	Public Const GIIHSaveUndn As String = "save_undn"
	
	' Endorsement lists
	Public Const GIIHEndorseList As String = "Endorse_List"
	Public Const GIIHEndorseCode As String = "endorse_code"
	Public Const GIIHEndorseText As String = "endorse_text"
	
	Public Const GIIHOutPaymentPlan As String = "Out_Payment_Plan"
	Public Const GIIHSavePaymentPlan As String = "Save_Payment_Plan"
	
	Public Const GIIHOutEndorsement As String = "Out_Endorsement"
	Public Const GIIHSaveEndorsement As String = "Save_Endorsement"
	
	Public Const GIIHOutExcessBreakdown As String = "Out_Excess_Breakdown"
	Public Const GIIHOutExcessBreakdownDescription As String = "Description"
	Public Const GIIHOutExcessBreakdownAmount As String = "Amount"
	Public Const GIIHOutExcessBreakdownCode As String = "Code"
	Public Const GIIHOutExcessBreakdownCoverCode As String = "cover_code"
	Public Const GIIHOutExcessBreakdownXsType As String = "xs_type"
	Public Const GIIHSaveExcessBreakdown As String = "Save_Excess_Breakdown"
	
	'*****************************************************************************
	' Post Quote Common
	'*****************************************************************************
	
	
	' qh_interested_party
	Public Const GIIHQhInterestedParty As String = "qh_interested_party"
	
	Public Const GIIHInterestedPartyNum As String = "interested_party_num"
	Public Const GIIHIntPartyName As String = "int_party_name"
	Public Const GIIHIntPartyAdd1 As String = "int_party_add1"
	Public Const GIIHIntPartyAdd2 As String = "int_party_add2"
	Public Const GIIHIntPartyAdd3 As String = "int_party_add3"
	Public Const GIIHIntPartyAdd4 As String = "int_party_add4"
	Public Const GIIHIntPartyPcod As String = "int_party_pcod"
	Public Const GIIHIntPartyBnet As String = "int_party_bnet"
	'plaice - 6/9/2004 - new field for Interested Party name dropdown list
	Public Const GIIHIntPartyNameBnet As String = "int_party_name_bnet"
	'plaice - 6/9/2004 - end
	Public Const GIIHIntPartySecBnet As String = "int_party_sec_bnet"
	Public Const GIIHIntPartyRefCode As String = "int_party_ref_code"
	Public Const GIIHIntPartyClauseYn As String = "int_party_clause_yn"
	
	
	' qh_terms
	Public Const GIIHQhTerms As String = "qh_terms"
	
	Public Const GIIHTermsImposedYn As String = "terms_imposed_yn"
	Public Const GIIHTermsTextDesc As String = "terms_text_desc"
	Public Const GIIHInsuranceRefusedYn As String = "insurance_refused_yn"
	Public Const GIIHRefusedTextDesc As String = "refused_text_desc"
	
	
	' qh_convictions
	Public Const GIIHQhConvictions As String = "qh_convictions"
	
	Public Const GIIHConvictionDate As String = "conviction_date"
	Public Const GIIHConvictionCodeBnet As String = "conviction_code_bnet"
	Public Const GIIHSentenceYn As String = "sentence_yn"
	Public Const GIIHSentenceFine As String = "sentence_fine"
	
	
	' qh_endorse
	Public Const GIIHQhEndorse As String = "qh_endorse"
	'
	'    Public Const GIIHEndorseCode = "endorse_code"
	Public Const GIIHEndorseDate As String = "endorse_date"
	
	
	' qh_authorise
	Public Const GIIHQhAuthorise As String = "qh_authorise"
	
	Public Const GIIHAutCode As String = "aut_code"
	Public Const GIIHAutPremLoadPercent As String = "aut_prem_load_percent"
	Public Const GIIHAutPremLoadAmount As String = "aut_prem_load_amount"
	Public Const GIIHAutSubsExcess As String = "aut_subs_excess"
	Public Const GIIHAutAdjustSectionYn As String = "aut_adjust_section_yn"
	Public Const GIIHAutAdjustItemYn As String = "aut_adjust_item_yn"
	Public Const GIIHAutReferEdtYn As String = "aut_refer_edt_yn"
	Public Const GIIHItemAdjustNum As String = "item_adjust_num"
	Public Const GIIHAutUwNote1 As String = "aut_uw_note1"
	Public Const GIIHAutUwNote2 As String = "aut_uw_note2"
	Public Const GIIHAutUwNote3 As String = "aut_uw_note3"
	Public Const GIIHRendMoreYn As String = "rend_more_yn"
	Public Const GIIHIntPartyMoreYn As String = "int_party_more_yn"
	Public Const GIIHAutItemMoreYn As String = "aut_item_more_yn"
	Public Const GIIHAutOvrReason As String = "aut_ovr_reason"
	Public Const GIIHRendNum As String = "rend_num"
	Public Const GIIHAutPremLoadedPremium As String = "aut_prem_loaded_premium"
	Public Const GIIHEccCode As String = "ecc_code"
	
	' section_adjustments
	Public Const GIIHSectionAdjustments As String = "section_adjustments"
	
	Public Const GIIHAutSectionBnet As String = "aut_section_bnet"
	Public Const GIIHAutSecPremLoadPercent As String = "aut_sec_prem_load_percent"
	Public Const GIIHAutSecPremLoadAmount As String = "aut_sec_prem_load_amount"
	Public Const GIIHAutSecPremLoadedPremium As String = "aut_sec_prem_loaded_premium"
	
	
	' item_adjustments
	Public Const GIIHItemAdjustments As String = "item_adjustments"
	
	Public Const GIIHAutItemNum As String = "aut_item_num"
	Public Const GIIHAutItemPremLoadPercent As String = "aut_item_prem_load_percent"
	Public Const GIIHAutItemPremLoadAmount As String = "aut_item_prem_load_amount"
	Public Const GIIHAutItemPremLoadedPremium As String = "aut_item_prem_loaded_premium"
	
	
	' qh_refer_endorsements
	Public Const GIIHQhReferEndorsements As String = "qh_refer_endorsements"
	
	Public Const GIIHRendCode As String = "rend_code"
	Public Const GIIHRendSectionBnet As String = "rend_section_bnet"
	Public Const GIIHRendItemNum As String = "rend_item_num"
	Public Const GIIHRendExcess As String = "rend_excess"
	Public Const GIIHRendEffDate As String = "rend_eff_date"
	
	
	' rend_free_text
	Public Const GIIHRendFreeText As String = "rend_free_text"
	
	Public Const GIIHRendFreeTextLine As String = "rend_free_text_line"
	
	
	' qh_pay
	Public Const GIIHQhPay As String = "qh_pay"
	
	Public Const GIIHIptRate As String = "ipt_rate"
	Public Const GIIHPayMethod As String = "pay_method"
	Public Const GIIHPayMethodOrg As String = "pay_method_org"
	Public Const GIIHPayCardAccName As String = "pay_card_acc_name"
	Public Const GIIHPayCardType As String = "pay_card_type"
	Public Const GIIHPayCardNo As String = "pay_card_no"
	Public Const GIIHPayCardDate As String = "pay_card_date"
	Public Const GIIHPayBankName As String = "pay_bank_name"
	Public Const GIIHPayBankCode As String = "pay_bank_code"
	Public Const GIIHPayBankAcname As String = "pay_bank_acname"
	Public Const GIIHPayBankAcnt As String = "pay_bank_acnt"
	Public Const GIIHPayBankAddr1 As String = "pay_bank_addr1"
	Public Const GIIHPayBankAddr2 As String = "pay_bank_addr2"
	Public Const GIIHPayBankAddr3 As String = "pay_bank_addr3"
	Public Const GIIHPayBankAddr4 As String = "pay_bank_addr4"
	Public Const GIIHPayBankPcode As String = "pay_bank_pcode"
	Public Const GIIHPayInstalYn As String = "pay_instal_yn"
	Public Const GIIHPayInstalNo As String = "pay_instal_no"
	Public Const GIIHPayInstalDate As String = "pay_instal_date"
	Public Const GIIHPayInstalAmnt As String = "pay_instal_amnt"
	Public Const GIIHPayInstalTot As String = "pay_instal_tot"
	Public Const GIIHPayCreditCharge As String = "pay_credit_charge"
	Public Const GIIHPayDepositYn As String = "pay_deposit_yn"
	Public Const GIIHPayDepositAmnt As String = "pay_deposit_amnt"
	Public Const GIIHPayDepositMethod As String = "pay_deposit_method"
	Public Const GIIHPayCardNumber As String = "pay_card_number"
	Public Const GIIHPayCardIssueNo As String = "pay_card_issue_no"
	Public Const GIIHPayDdmSignedYn As String = "pay_ddm_signed_yn"
	Public Const GIIHHolsIptRate As String = "hols_ipt_rate"
	Public Const GIIHHolsIpt As String = "hols_ipt"
	
	
	' qh_mta_data
	Public Const GIIHQhMtaData As String = "qh_mta_data"
	
	Public Const GIIHMtaType As String = "mta_type"
	Public Const GIIHMtaStartDate As String = "mta_start_date"
	Public Const GIIHMtaEndDate As String = "mta_end_date"
	Public Const GIIHMtaCancelPrevClaimYn As String = "mta_cancel_prev_claim_yn"
	Public Const GIIHMtaPremiumIpt As String = "mta_premium_ipt"
	Public Const GIIHMtaPremium As String = "mta_premium"
	Public Const GIIHMtaReferPremiumGross As String = "mta_refer_premium_gross"
	Public Const GIIHMtaRnwlPremium As String = "mta_rnwl_premium"
	Public Const GIIHMtaCancelDeceasedYn As String = "mta_cancel_deceased_yn"
	Public Const GIIHMtaCancelSoldYn As String = "mta_cancel_sold_yn"
	Public Const GIIHMtaCancelTransferInsYn As String = "mta_cancel_transfer_ins_yn"
	Public Const GIIHMtaCancelTransferCliYn As String = "mta_cancel_transfer_cli_yn"
	Public Const GIIHMtaCancelInsRequestYn As String = "mta_cancel_ins_request_yn"
	Public Const GIIHMtaCancelOtherYn As String = "mta_cancel_other_yn"
	Public Const GIIHMtaDetailsChanged As String = "mta_details_changed"
	Public Const GIIHMtaMtaAtRenewalYN As String = "mta_at_renewal_yn"
	
	' mta_reason_bnet
	'Public Const GIIHMtaReasonBnet = "mta_reason_bnet"
	
	Public Const GIIHMtaReasonBnet As String = "mta_reason_bnet"
	
	
	' qh_prev_address
	Public Const GIIHQhPrevAddress As String = "qh_prev_address"
	
	Public Const GIIHYearsAtCurrentAddress As String = "years_at_current_address"
	Public Const GIIHMonthsAtCurrentAddress As String = "months_at_current_address"
	Public Const GIIHSchPadYrs As String = "sch_pad_yrs"
	Public Const GIIHSchPadMon As String = "sch_pad_mon"
	Public Const GIIHPadAddr1 As String = "pad_addr_1"
	Public Const GIIHPadAddr2 As String = "pad_addr_2"
	Public Const GIIHPadAddr3 As String = "pad_addr_3"
	Public Const GIIHPadAddr4 As String = "pad_addr_4"
	Public Const GIIHPadPcode As String = "pad_pcode"
	
	
	' qh_bank_details
	Public Const GIIHQhBankDetails As String = "qh_bank_details"
	
	Public Const GIIHQhBankName As String = "qh_bank_name"
	Public Const GIIHQhBankAddr1 As String = "qh_bank_addr1"
	Public Const GIIHQhBankAddr2 As String = "qh_bank_addr2"
	Public Const GIIHQhBankAddr3 As String = "qh_bank_addr3"
	Public Const GIIHQhBankPcod As String = "qh_bank_pcod"
	
	
	' qh_rebuild
	Public Const GIIHQhRebuild As String = "qh_rebuild"
	
	Public Const GIIHProRebuildWidth As String = "pro_rebuild_width"
	Public Const GIIHProRebuildLength As String = "pro_rebuild_length"
	Public Const GIIHProRebuildArea As String = "pro_rebuild_area"
	Public Const GIIHProRebuildStorey As String = "pro_rebuild_storey"
	Public Const GIIHProRebuildAdval As String = "pro_rebuild_adval"
	
	
	' policy_discounts
	Public Const GIIHPolicyDiscounts As String = "policy_discounts"
	
	Public Const GIIHSurveyControl As String = "survey_control"
	Public Const GIIHSurveyInsurerCode As String = "survey_insurer_code"
	Public Const GIIHSurveySchemeNum As String = "survey_scheme_num"
	
	' Generic post quote
	
	Public Const GIIHGenInsurer1 As String = "GenInsurer1"
	
	Public Const GIIHfield_1_1 As String = "field_1_1"
	Public Const GIIHfield_1_2 As String = "field_1_2"
	Public Const GIIHfield_1_3 As String = "field_1_3"
	Public Const GIIHfield_1_4 As String = "field_1_4"
	Public Const GIIHfield_1_5 As String = "field_1_5"
	Public Const GIIHfield_1_6 As String = "field_1_6"
	Public Const GIIHfield_1_7 As String = "field_1_7"
	Public Const GIIHfield_1_8 As String = "field_1_8"
	Public Const GIIHfield_1_9 As String = "field_1_9"
	Public Const GIIHfield_1_10 As String = "field_1_10"
	Public Const GIIHfield_1_11 As String = "field_1_11"
	Public Const GIIHfield_1_12 As String = "field_1_12"
	Public Const GIIHfield_1_13 As String = "field_1_13"
	Public Const GIIHfield_1_14 As String = "field_1_14"
	Public Const GIIHfield_1_15 As String = "field_1_15"
	Public Const GIIHfield_1_16 As String = "field_1_16"
	Public Const GIIHfield_1_17 As String = "field_1_17"
	Public Const GIIHfield_1_18 As String = "field_1_18"
	Public Const GIIHfield_1_19 As String = "field_1_19"
	Public Const GIIHfield_1_20 As String = "field_1_20"
	Public Const GIIHfield_1_21 As String = "field_1_21"
	Public Const GIIHfield_1_22 As String = "field_1_22"
	Public Const GIIHfield_1_23 As String = "field_1_23"
	Public Const GIIHfield_1_24 As String = "field_1_24"
	Public Const GIIHfield_1_25 As String = "field_1_25"
	
	Public Const GIIHGenInsurer11 As String = "GenInsurer11"
	
	Public Const GIIHfield_11_1 As String = "field_11_1"
	Public Const GIIHfield_11_2 As String = "field_11_2"
	Public Const GIIHfield_11_3 As String = "field_11_3"
	Public Const GIIHfield_11_4 As String = "field_11_4"
	Public Const GIIHfield_11_5 As String = "field_11_5"
	
	Public Const GIIHGenInsurer2 As String = "GenInsurer2"
	
	Public Const GIIHfield_2_1 As String = "field_2_1"
	Public Const GIIHfield_2_2 As String = "field_2_2"
	Public Const GIIHfield_2_3 As String = "field_2_3"
	Public Const GIIHfield_2_4 As String = "field_2_4"
	Public Const GIIHfield_2_5 As String = "field_2_5"
	Public Const GIIHfield_2_6 As String = "field_2_6"
	Public Const GIIHfield_2_7 As String = "field_2_7"
	Public Const GIIHfield_2_8 As String = "field_2_8"
	Public Const GIIHfield_2_9 As String = "field_2_9"
	Public Const GIIHfield_2_10 As String = "field_2_10"
	Public Const GIIHfield_2_11 As String = "field_2_11"
	Public Const GIIHfield_2_12 As String = "field_2_12"
	Public Const GIIHfield_2_13 As String = "field_2_13"
	Public Const GIIHfield_2_14 As String = "field_2_14"
	Public Const GIIHfield_2_15 As String = "field_2_15"
	Public Const GIIHfield_2_16 As String = "field_2_16"
	Public Const GIIHfield_2_17 As String = "field_2_17"
	Public Const GIIHfield_2_18 As String = "field_2_18"
	Public Const GIIHfield_2_19 As String = "field_2_19"
	Public Const GIIHfield_2_20 As String = "field_2_20"
	Public Const GIIHfield_2_21 As String = "field_2_21"
	Public Const GIIHfield_2_22 As String = "field_2_22"
	Public Const GIIHfield_2_23 As String = "field_2_23"
	Public Const GIIHfield_2_24 As String = "field_2_24"
	Public Const GIIHfield_2_25 As String = "field_2_25"
	Public Const GIIHfield_2_26 As String = "field_2_26"
	Public Const GIIHfield_2_27 As String = "field_2_27"
	Public Const GIIHfield_2_28 As String = "field_2_28"
	Public Const GIIHfield_2_29 As String = "field_2_29"
	Public Const GIIHfield_2_30 As String = "field_2_30"
	Public Const GIIHfield_2_31 As String = "field_2_31"
	Public Const GIIHfield_2_32 As String = "field_2_32"
	Public Const GIIHfield_2_33 As String = "field_2_33"
	Public Const GIIHfield_2_34 As String = "field_2_34"
	Public Const GIIHfield_2_35 As String = "field_2_35"
	Public Const GIIHfield_2_36 As String = "field_2_36"
	Public Const GIIHfield_2_37 As String = "field_2_37"
	Public Const GIIHfield_2_38 As String = "field_2_38"
	Public Const GIIHfield_2_39 As String = "field_2_39"
	Public Const GIIHfield_2_40 As String = "field_2_40"
	Public Const GIIHfield_2_41 As String = "field_2_41"
	Public Const GIIHfield_2_42 As String = "field_2_42"
	Public Const GIIHfield_2_43 As String = "field_2_43"
	Public Const GIIHfield_2_44 As String = "field_2_44"
	Public Const GIIHfield_2_45 As String = "field_2_45"
	Public Const GIIHfield_2_46 As String = "field_2_46"
	Public Const GIIHfield_2_47 As String = "field_2_47"
	Public Const GIIHfield_2_48 As String = "field_2_48"
	Public Const GIIHfield_2_49 As String = "field_2_49"
	Public Const GIIHfield_2_50 As String = "field_2_50"
	Public Const GIIHfield_2_51 As String = "field_2_51"
	Public Const GIIHfield_2_52 As String = "field_2_52"
	Public Const GIIHfield_2_53 As String = "field_2_53"
	Public Const GIIHfield_2_54 As String = "field_2_54"
	Public Const GIIHfield_2_55 As String = "field_2_55"
	Public Const GIIHfield_2_56 As String = "field_2_56"
	Public Const GIIHfield_2_57 As String = "field_2_57"
	Public Const GIIHfield_2_58 As String = "field_2_58"
	Public Const GIIHfield_2_59 As String = "field_2_59"
	Public Const GIIHfield_2_60 As String = "field_2_60"
	Public Const GIIHfield_2_61 As String = "field_2_61"
	Public Const GIIHfield_2_62 As String = "field_2_62"
	Public Const GIIHfield_2_63 As String = "field_2_63"
	Public Const GIIHfield_2_64 As String = "field_2_64"
	Public Const GIIHfield_2_65 As String = "field_2_65"
	
	Public Const GIIHGenInsurer21 As String = "GenInsurer21"
	
	Public Const GIIHfield_21_1 As String = "field_21_1"
	Public Const GIIHfield_21_2 As String = "field_21_2"
	Public Const GIIHfield_21_3 As String = "field_21_3"
	Public Const GIIHfield_21_4 As String = "field_21_4"
	Public Const GIIHfield_21_5 As String = "field_21_5"
	
	'Insurer specific constants for Co
	Public Const GIIHGenInsurerCo As String = "GenInsurer1"
	Public Const GIIHGenInsurerCoEndorse As String = "GenInsurer11"
	
	Public Const GIIHCoPayInstalDeptYn As String = "field_1_1"
	Public Const GIIHCoSightValCertYn As String = "field_1_3"
	Public Const GIIHCoValuerName As String = "field_1_4"
	Public Const GIIHCoValueDate As String = "field_1_5"
	Public Const GIIHCoExtendedYn As String = "field_1_15"
	Public Const GIIHCoExtendedDate As String = "field_1_16"
	Public Const GIIHCoCcjBankruptYn As String = "field_1_9"
	Public Const GIIHCoDeclineTermsYn As String = "field_1_10"
	Public Const GIIHCoSubsRepairYn As String = "field_1_11"
	Public Const GIIHCoSafeFittedYn As String = "field_1_14"
	Public Const GIIHCoBuildPrevPolno As String = "field_1_6"
	Public Const GIIHCoContPrevPolno As String = "field_1_12"
	Public Const GIIHCoAnnualTravelYn As String = "field_1_20"
	Public Const GIIHCoStandardCovYn As String = "field_1_21"
	Public Const GIIHCoWinterCovYn As String = "field_1_22"
	Public Const GIIHCoStdHazActivYn As String = "field_1_23"
	Public Const GIIHCoWinHazActivYn As String = "field_1_24"
	Public Const GIIHCoIndividualFamily As String = "field_1_1"
	Public Const GIIHCoExistCondYn As String = "field_1_25"
	Public Const GIIHCoGeogLimits As String = "field_1_2"
	Public Const GIIHCoGeogLimitsDesc As String = "field_1_8"
	Public Const GIIHCoHolsPremiumIncIpt As String = "field_1_7"
	Public Const GIIHCoEndorseNum As String = "field_1_13"
	Public Const GIIHCoEndorseCode As String = "field_11_1"
	Public Const GIIHCoEndorseDate As String = "field_11_2"
	Public Const GIIHCoEndItemNum As String = "field_11_3"
	Public Const GIIHCoEndLevel As String = "field_11_4"
	Public Const GIIHCoEndPbItemNum As String = "field_11_5"
	Public Const GIIHCoEndNewAtRenewal As String = "field_11_6"
	Public Const GIIHPolAlarmMaintBnet As String = "field_1_13"
	
	'Insurer specific constants for Heath
	Public Const GIIHGenInsurerHeath As String = "GenInsurer1"
	Public Const GIIHGenInsurerHeathTrees As String = "GenInsurer11"
	
	Public Const GIIHHeathSubsidenceYn As String = "field_1_1"
	Public Const GIIHHeathMovementSurveyYn As String = "field_1_2"
	Public Const GIIHHeathExtendedYn As String = "field_1_3"
	Public Const GIIHHeathTreesYn As String = "field_1_4"
	Public Const GIIHHeathTreeSpecies As String = "field_11_1"
	Public Const GIIHHeathTreeDistance As String = "field_11_2"
	Public Const GIIHHeathTreeHeight As String = "field_11_3"
	Public Const GIIHHeathNhbcCertYn As String = "field_1_5"
	Public Const GIIHHeathChequeNo As String = "field_1_6"
	
	'Insurer specific constants for Zu
	Public Const GIIHGenInsurerZu As String = "GenInsurer1"
	Public Const GIIHGenInsurerZuValuation As String = "GenInsurer11"
	
	Public Const GIIHZuInfoReqd As String = "field_1_1"
	Public Const GIIHZuPayInstalDeptYn As String = "field_1_2"
	Public Const GIIHZuPayInstalDept As String = "field_1_3"
	Public Const GIIHZuEntertainYn As String = "field_1_5"
	Public Const GIIHZuEntertainDesc As String = "field_1_6"
	Public Const GIIHZuOccupiedYn As String = "field_1_7"
	Public Const GIIHZuOccupiedDesc As String = "field_1_8"
	Public Const GIIHZuGasfiredYn As String = "field_1_9"
	Public Const GIIHZuBuildingPremium As String = "field_1_10"
	Public Const GIIHZuContentsPremium As String = "field_1_11"
	Public Const GIIHZuLegalPremium As String = "field_1_12"
	Public Const GIIHZuDiscountCode As String = "field_1_17"
	Public Const GIIHZuDiscountAllowed As String = "field_1_18"
	Public Const GIIHZuRevisedPremium As String = "field_1_19"
	Public Const GIIHZuValSuppliedYn As String = "field_11_1"
	Public Const GIIHZuChangeAddYn As String = "field_1_20"
	Public Const GIIHZuAddExpDate As String = "field_1_21"
	Public Const GIIHZuValOver40Si As String = "field_1_22"
	
	'Insurer specific constants for Ro
	Public Const GIIHGenInsurerRo As String = "GenInsurer1"
	
	Public Const GIIHRoLegActYn As String = "field_1_1"
	Public Const GIIHRoLegActDets As String = "field_1_2"
	Public Const GIIHRoAddrLen As String = "field_1_3"
	Public Const GIIHRoPrevAddr1 As String = "field_1_4"
	Public Const GIIHRoPrevAddr2 As String = "field_1_5"
	Public Const GIIHRoPrevAddr3 As String = "field_1_6"
	Public Const GIIHRoPrevAddr4 As String = "field_1_7"
	Public Const GIIHRoPrevPcod As String = "field_1_8"
	Public Const GIIHRoValnRecent As String = "field_1_9"
	Public Const GIIHRoValnDate As String = "field_1_10"
	Public Const GIIHRoValnWho As String = "field_1_11"
	Public Const GIIHRoDamageFreeYn As String = "field_1_12"
	Public Const GIIHRoEverSufferedYn As String = "field_1_13"
	Public Const GIIHRoSoleControlYn As String = "field_1_14"
	Public Const GIIHRoVoidInsYn As String = "field_1_15"
	Public Const GIIHRoPendingLegalClaimsYn As String = "field_11_16"
	Public Const GIIHRoPastLegalClaimsYn As String = "field_11_17"
	Public Const GIIHRoCaravanTvSi As String = "field_11_18"
	Public Const GIIHRoCoverDeclinedYn As String = "field_11_19"
	
	'Insurer specific constants for Nu
	Public Const GIIHGenInsurerNu As String = "GenInsurer2"
	
	Public Const GIIHNuNetInstalAmnt As String = "field_2_1"
	Public Const GIIHNuMonthlyAdminFee As String = "field_2_2"
	Public Const GIIHNuMaintHome As String = "field_2_3"
	Public Const GIIHNuSiFull As String = "field_2_4"
	Public Const GIIHNuSiMaint As String = "field_2_5"
	Public Const GIIHNuSecDevOp As String = "field_2_6"
	Public Const GIIHNuDamageSignYn As String = "field_2_7"
	Public Const GIIHNuPrevDamageYn As String = "field_2_8"
	Public Const GIIHNuExtLegCovYn As String = "field_2_9"
	Public Const GIIHNuSubsidenceAreaYn As String = "field_2_60"
	Public Const GIIHNuStructuralYn As String = "field_2_10"
	Public Const GIIHNuHomeCracksYn As String = "field_2_11"
	Public Const GIIHNuSiteCracksYn As String = "field_2_12"
	Public Const GIIHNuCrackWidth As String = "field_2_13"
	Public Const GIIHNuBayWindowYn As String = "field_2_14"
	Public Const GIIHNuExtensionYn As String = "field_2_15"
	Public Const GIIHNuExtDets As String = "field_21_1"
	Public Const GIIHNuBuildDate As String = "field_21_2"
	Public Const GIIHNuStoreys As String = "field_21_3"
	Public Const GIIHNuLiveTreesYn As String = "field_2_16"
	Public Const GIIHNuLtSpecies As String = "field_2_17"
	Public Const GIIHNuLtDistance As String = "field_2_18"
	Public Const GIIHNuLtHeight As String = "field_2_19"
	Public Const GIIHNuDeadTreesYn As String = "field_2_20"
	Public Const GIIHNuDtSpecies As String = "field_2_21"
	Public Const GIIHNuDtDistance As String = "field_2_22"
	Public Const GIIHNuDtHeight As String = "field_2_23"
	Public Const GIIHNuSlopingSiteYn As String = "field_2_24"
	Public Const GIIHNuEndTerraceYn As String = "field_2_25"
	Public Const GIIHNuTerracePassageYn As String = "field_2_26"
	Public Const GIIHNuSlopeTopYn As String = "field_2_27"
	Public Const GIIHNuSlopeMiddleYn As String = "field_2_28"
	Public Const GIIHNuSlopeBottomYn As String = "field_2_29"
	Public Const GIIHNuSemiBottomSlopeYn As String = "field_2_30"
	Public Const GIIHNuPcarPolicyYn As String = "field_2_31"
	Public Const GIIHNuSqaSubsidencePoints As String = "field_2_13"
	Public Const GIIHNuSqbSubsidencePoints As String = "field_2_14"
	Public Const GIIHNuSqdSubsidencePoints As String = "field_2_24"
	Public Const GIIHNuTravelTypeBnet As String = "field_2_32"
	Public Const GIIHNuStandardCov As String = "field_2_33"
	Public Const GIIHNuWinterCov As String = "field_2_34"
	Public Const GIIHNuOnePersCov As String = "field_2_35"
	Public Const GIIHNuHazSportsCode As String = "field_2_36"
	Public Const GIIHNuHazSportsYn As String = "field_2_37"
	Public Const GIIHNuHazSportsDesc As String = "field_2_36"
	Public Const GIIHNuOthHazSportsDesc As String = "field_2_38"
	Public Const GIIHNuHolsStartDate As String = "field_2_39"
	Public Const GIIHNuHolsEndDate As String = "field_2_40"
	Public Const GIIHNuHolsPremium As String = "field_2_41"
	Public Const GIIHNuMtaHolsPremium As String = "field_2_42"
	Public Const GIIHNuAdjHolsPremium As String = "field_2_65"
	Public Const GIIHNuAuthCode As String = "field_2_43"
	Public Const GIIHNuOtherAuthCode As String = "field_2_44"
	Public Const GIIHNuAddRemCover As String = "field_2_45"
	Public Const GIIHNuAnnTravCovStatus As String = "field_2_46"
	Public Const GIIHNuOrioleCovYn As String = "field_2_47"
	Public Const GIIHNuPayingGuestsNum As String = "field_2_48"
	Public Const GIIHNuEmpLiabilityCovYn As String = "field_2_49"
	Public Const GIIHNuExtLegOldYn As String = "field_2_61"
	Public Const GIIHNuMtaGoldPrem As String = "field_2_52"
	Public Const GIIHNuOseasWeddYn As String = "field_2_53"
	Public Const GIIHNuVehTheftIncYn As String = "field_2_54"
	Public Const GIIHNuVehTheftPrem As String = "field_2_54"
	Public Const GIIHNuEurope As String = "field_2_55"
	Public Const GIIHNuEuropePlusWinter As String = "field_2_56"
	Public Const GIIHNuWorld As String = "field_2_57"
	Public Const GIIHNuWorldPlusWinter As String = "field_2_58"
	Public Const GIIHNuTravelPremium As String = "field_2_59"
	Public Const GIIHNuAdditionalClauseYn As String = "field_2_62"
	Public Const GIIHNuBrokerOwnPayYn As String = "field_2_63"
	Public Const GIIHNuRnwlHolsPremium As String = "field_2_64"
	
	'Insurer specific constants for Fol
	Public Const GIIHGenInsurerFol As String = "GenInsurer1"
	
	Public Const GIIHFolSightValCertYn As String = "field_1_1"
	Public Const GIIHFolRicsSurveyYn As String = "field_1_2"
	Public Const GIIHFolBankruptYn As String = "field_1_3"
	Public Const GIIHFolFloodYn As String = "field_1_4"
	Public Const GIIHFolRepairedYn As String = "field_1_5"
	Public Const GIIHFolSubsFreeYn As String = "field_1_6"
	
	'Insurer specific constants for Cong
	Public Const GIIHGenInsurerCong As String = "GenInsurer1"
	
	Public Const GIIHCongOutbuildingsYn As String = "field_1_1"
	Public Const GIIHCongNearbySubsYn As String = "field_1_2"
	Public Const GIIHCongCcjYn As String = "field_1_3"
	Public Const GIIHCongBankruptYn As String = "field_1_4"
	Public Const GIIHCongYearsCover As String = "field_1_5"
	Public Const GIIHCongLastClaimDate As String = "field_1_6"
	Public Const GIIHCongAddDiffReason As String = "field_1_7"
	Public Const GIIHCongStormDamageYn As String = "field_1_7"
	Public Const GIIHCongValCertYn As String = "field_1_8"
	
	'Insurer specific constants for Sg
	Public Const GIIHGenInsurerSg As String = "GenInsurer1"
	
	Public Const GIIHSgItemsAtUniv As String = "field_1_1"
	Public Const GIIHSgItemsAtHoli As String = "field_1_2"
	Public Const GIIHSgItemsElsewhere As String = "field_1_3"
	Public Const GIIHSgInfDetails As String = "field_1_4"
	Public Const GIIHSgSightValCertYn As String = "field_1_5"
	Public Const GIIHSgDateValCert As String = "field_1_6"
	Public Const GIIHSgAlarmMaintCertYn As String = "field_1_7"
	Public Const GIIHSgAlarmMaintCertNum As String = "field_1_8"
	Public Const GIIHSgCcAddressYn As String = "field_1_9"
	Public Const GIIHSgCcAdd1 As String = "field_1_10"
	Public Const GIIHSgCcAdd2 As String = "field_1_11"
	Public Const GIIHSgCcAdd3 As String = "field_1_12"
	Public Const GIIHSgCcAdd4 As String = "field_1_13"
	Public Const GIIHSgCcPostcode As String = "field_1_14"
	Public Const GIIHSgDdAddressYn As String = "field_1_15"
	Public Const GIIHSgDdAdd1 As String = "field_1_16"
	Public Const GIIHSgDdAdd2 As String = "field_1_17"
	Public Const GIIHSgDdAdd3 As String = "field_1_18"
	Public Const GIIHSgDdAdd4 As String = "field_1_19"
	Public Const GIIHSgDdPostcode As String = "field_1_20"
	Public Const GIIHSgLoadDiscReason As String = "field_1_21"
	Public Const GIIHSgSectionReason1 As String = "field_1_22"
	Public Const GIIHSgSectionReason2 As String = "field_1_23"
	Public Const GIIHSgSectionReason3 As String = "field_1_24"
	
	'Insurer specific constants for Axa
	Public Const GIIHGenInsurerAxa As String = "GenInsurer2"
	
	Public Const GIIHAxaSightValCertYn As String = "field_2_1"
	Public Const GIIHAxaYearOccupied As String = "field_2_17"
	Public Const GIIHAxaHomeExtendedYn As String = "field_2_18"
	Public Const GIIHAxaExtensionYear As String = "field_2_19"
	Public Const GIIHAxaDiffAddr As String = "field_2_22"
	Public Const GIIHAxaBankruptcyYn As String = "field_2_21"
	Public Const GIIHAxaChildName1 As String = "field_2_1"
	Public Const GIIHAxaChildName2 As String = "field_2_2"
	Public Const GIIHAxaChildName3 As String = "field_2_3"
	Public Const GIIHAxaChildName4 As String = "field_2_4"
	Public Const GIIHAxaChildName5 As String = "field_2_5"
	Public Const GIIHAxaChildName6 As String = "field_2_6"
	Public Const GIIHAxaMedicalHistory1 As String = "field_2_7"
	Public Const GIIHAxaMedicalHistory2 As String = "field_2_8"
	Public Const GIIHAxaMedicalHistory3 As String = "field_2_9"
	Public Const GIIHAxaMedicalHistory4 As String = "field_2_10"
	Public Const GIIHAxaHazardousSports1 As String = "field_2_11"
	Public Const GIIHAxaHazardousSports2 As String = "field_2_12"
	Public Const GIIHAxaSmokersName1 As String = "field_2_13"
	Public Const GIIHAxaSmokersName2 As String = "field_2_14"
	Public Const GIIHAxaSmokersName3 As String = "field_2_15"
	Public Const GIIHAxaSmokersName4 As String = "field_2_16"
	Public Const GIIHAxaStaAdd1 As String = "field_2_26"
	Public Const GIIHAxaStaAdd2 As String = "field_2_27"
	Public Const GIIHAxaStaAdd3 As String = "field_2_28"
	Public Const GIIHAxaStaAdd4 As String = "field_2_29"
	Public Const GIIHAxaStaPcod As String = "field_2_30"
	Public Const GIIHAxaMaterialFactsLine1 As String = "field_2_29"
	Public Const GIIHAxaMaterialFactsLine2 As String = "field_2_30"
	Public Const GIIHAxaMaterialFactsLine3 As String = "field_2_31"
	Public Const GIIHAxaStatCliAddrYn As String = "field_2_25"
	Public Const GIIHAxaDamageYn As String = "field_2_20"
	Public Const GIIHAxaCaravanItem As String = "field_2_34"
	Public Const GIIHAxaCaravanDiffAddr As String = "field_2_35"
	Public Const GIIHAxaWheelClampYn As String = "field_2_36"
	Public Const GIIHAxaOtherSecurity As String = "field_2_37"
	Public Const GIIHAxaSecurityDiscountYn As String = "field_2_23"
	Public Const GIIHAxaAlarmDiscountYn As String = "field_2_24"
	Public Const GIIHAxaMtaActionType As String = "field_2_38"
	Public Const GIIHAxaStoredSecDiscountYn As String = "field_2_39"
	Public Const GIIHAxaStoredAlmDiscountYn As String = "field_2_40"
	
	'Insurer specific constants for Lg
	Public Const GIIHGenInsurerLg As String = "GenInsurer1"
	
	Public Const GIIHLgValCertYn As String = "field_1_1"
	Public Const GIIHLgAlarmSpecYn As String = "field_1_2"
	Public Const GIIHLgDiryValCertReq As String = "field_1_19"
	Public Const GIIHLgDiryAlarmSpecReq As String = "field_1_20"
	Public Const GIIHLgFlatFloor As String = "field_1_3"
	Public Const GIIHLgStructDamage As String = "field_1_4"
	Public Const GIIHLgDamageDetails As String = "field_1_5"
	Public Const GIIHLgUnderpinned As String = "field_1_6"
	Public Const GIIHLgUnderpinDetails As String = "field_1_7"
	Public Const GIIHLgLouveGlass As String = "field_1_8"
	Public Const GIIHLgGlassGlue As String = "field_1_9"
	Public Const GIIHLgSecDevUsed As String = "field_1_10"
	Public Const GIIHLgKeysHidden As String = "field_1_11"
	Public Const GIIHLgLockAtNight As String = "field_1_12"
	Public Const GIIHLgAnnualTravel As String = "field_1_13"
	Public Const GIIHLgAtrGroupType As String = "field_1_14"
	Public Const GIIHLgAtrForename As String = "field_11_1"
	Public Const GIIHLgAtrSurname As String = "field_11_2"
	Public Const GIIHLgAtrDob As String = "field_11_3"
	Public Const GIIHLgAtrMedicalCurrent As String = "field_1_15"
	Public Const GIIHLgAtrMedicalHistory As String = "field_1_16"
	Public Const GIIHLgAtrPremium As String = "field_1_17"
	Public Const GIIHLgHomeSurveyedYn As String = "field_1_18"
	Public Const GIIHLgPrintInterestedPartySchedule As String = "field_1_19"
	Public Const GIIHLgPrintContinuationSchedule As String = "field_1_20"
	
	'Insurer specific constants for Ecc
	Public Const GIIHGenInsurerEcc As String = "GenInsurer2"
	
	Public Const GIIHEccLodgersYn As String = "field_2_1"
	Public Const GIIHEccBedBreakfastYn As String = "field_2_2"
	Public Const GIIHEccFireBrigYn As String = "field_2_3"
	Public Const GIIHEccFireWaterSupplyYn As String = "field_2_4"
	Public Const GIIHEccFireAccessYn As String = "field_2_5"
	Public Const GIIHEccAtCurrentYears As String = "field_2_6"
	Public Const GIIHEccAtCurrentMonths As String = "field_2_7"
	Public Const GIIHEccPropertyMovementYn As String = "field_2_8"
	Public Const GIIHEccPropertyFloodingYn As String = "field_2_9"
	Public Const GIIHEccBuildingSiFullYn As String = "field_2_10"
	Public Const GIIHEccExternalDoorsYn As String = "field_2_11"
	Public Const GIIHEccGarageDoorsYn As String = "field_2_12"
	Public Const GIIHEccWindowLockYn As String = "field_2_13"
	Public Const GIIHEccWindowLouvreYn As String = "field_2_14"
	Public Const GIIHEccWindowGluedYn As String = "field_2_15"
	Public Const GIIHEccAluminumDoorYn As String = "field_2_16"
	Public Const GIIHEccAluminumDoorDeadlockYn As String = "field_2_17"
	Public Const GIIHEccSingleLeafYn As String = "field_2_18"
	Public Const GIIHEccSingleLeafSecureYn As String = "field_2_19"
	Public Const GIIHEccDoubleLeafYn As String = "field_2_20"
	Public Const GIIHEccDoubleLeafSecureAYn As String = "field_2_21"
	Public Const GIIHEccFlushBoltsYn As String = "field_2_22"
	Public Const GIIHEccDoubleLeafSecureBYn As String = "field_2_23"
	Public Const GIIHEccOtherAluminumDoorYn As String = "field_2_24"
	Public Const GIIHEccOtherAluminumSecureYn As String = "field_2_25"
	Public Const GIIHEccPatioDoorYn As String = "field_2_26"
	Public Const GIIHEccPatioDoorSecureYn As String = "field_2_27"
	Public Const GIIHEccFrenchDoorYn As String = "field_2_28"
	Public Const GIIHEccFrenchDoorSecureYn As String = "field_2_29"
	Public Const GIIHEccGroundFloorWindowsYn As String = "field_2_30"
	Public Const GIIHEccWindowLouvre2Yn As String = "field_2_31"
	Public Const GIIHEccWindowGlued2Yn As String = "field_2_32"
	Public Const GIIHEccSignedSubsidence As String = "field_2_33"
	Public Const GIIHEccRevisedPrem As String = "field_2_34"
	Public Const GIIHEccOverridePremiumYn As String = "field_2_35"
	Public Const GIIHEccNumberEmployees As String = "field_2_39"
	Public Const GIIHEccCompanyName As String = "field_2_40"
	Public Const GIIHEccBankruptYn As String = "field_2_36"
	Public Const GIIHEccCcjYn As String = "field_2_37"
	Public Const GIIHEccEnd25 As String = "field_2_41"
	Public Const GIIHEccAshTen6MonYn As String = "field_2_41"
	Public Const GIIHEccAgentLetYn As String = "field_2_42"
	Public Const GIIHEccLetLocAuthYn As String = "field_2_43"
	Public Const GIIHEccFteUkResYn As String = "field_2_44"
	Public Const GIIHEccOccIncepYn As String = "field_2_45"
	Public Const GIIHEcc30DayUnoccYn As String = "field_2_46"
	Public Const GIIHEccTotOwnProps As String = "field_2_47"
	Public Const GIIHEccTimeAtAddrYy As String = "field_2_48"
	Public Const GIIHEccTimeAtAddrMm As String = "field_2_49"
	Public Const GIIHEccPrevRiskAdd1 As String = "field_2_50"
	Public Const GIIHEccPrevRiskAdd2 As String = "field_2_51"
	Public Const GIIHEccPrevRiskAdd3 As String = "field_2_52"
	Public Const GIIHEccPrevRiskAdd4 As String = "field_2_53"
	Public Const GIIHEccPrevRiskPostcode As String = "field_2_54"
	Public Const GIIHEccPrevCorrAdd1 As String = "field_2_55"
	Public Const GIIHEccPrevCorrAdd2 As String = "field_2_56"
	Public Const GIIHEccPrevCorrAdd3 As String = "field_2_57"
	Public Const GIIHEccPrevCorrAdd4 As String = "field_2_58"
	Public Const GIIHEccPrevCorrPostcode As String = "field_2_59"
	Public Const GIIHEccInsuranceDeclinedYn As String = "field_2_60"
	Public Const GIIHEccNonDrivConvictsYn As String = "field_2_61"
	
	'Insurer specific constants for Gan
	Public Const GIIHGenInsurerGan As String = "GenInsurer1"
	Public Const GIIHGenInsurerGanOccupants As String = "GenInsurer11"
	
	Public Const GIIHGanOtherTitle As String = "field_11_1"
	Public Const GIIHGanOtherInitials As String = "field_11_2"
	Public Const GIIHGanOtherSurname As String = "field_11_3"
	Public Const GIIHGanOtherAge As String = "field_11_4"
	Public Const GIIHGanExtendedYn As String = "field_1_9"
	Public Const GIIHGanBusinessFlatYn As String = "field_1_10"
	Public Const GIIHGanSlopingSiteYn As String = "field_1_1"
	Public Const GIIHGanTreesYn As String = "field_1_2"
	Public Const GIIHGanHomeSurveyedYn As String = "field_1_3"
	Public Const GIIHGanHomeDefectYn As String = "field_1_4"
	Public Const GIIHGanHomeDrainRepairYn As String = "field_1_5"
	Public Const GIIHGanHomeHolidayYn As String = "field_1_6"
	Public Const GIIHGanHomeSeaYn As String = "field_1_7"
	Public Const GIIHGanBankruptcyYn As String = "field_1_8"
	
	'Insurer specific constants for Bish
	Public Const GIIHGenInsurerBish As String = "GenInsurer1"
	
	Public Const GIIHBishInsRefYn As String = "field_1_4"
	Public Const GIIHBishBankruptYn As String = "field_1_5"
	Public Const GIIHBishHighvalOutYn As String = "field_1_6"
	Public Const GIIHBishProfValuedYn As String = "field_1_8"
	Public Const GIIHBishProfValuedDate As String = "field_1_9"
	Public Const GIIHBishProfValue As String = "field_1_10"
	Public Const GIIHBishCrackingYn As String = "field_1_1"
	Public Const GIIHBishRemedialWorkYn As String = "field_1_2"
	Public Const GIIHBishSurveyedYn As String = "field_1_3"
	Public Const GIIHBishFreezerAge As String = "field_1_7"
	Public Const GIIHBishSpiUpdatedFlag As String = "field_1_11"
	
	'Insurer specific constants for Ind
	Public Const GIIHGenInsurerInd As String = "GenInsurer1"
	
	Public Const GIIHIndSecurityDevicesYn As String = "field_1_1"
	Public Const GIIHIndNacossApprovedYn As String = "field_1_2"
	Public Const GIIHIndAlarmMaintenanceYn As String = "field_1_3"
	Public Const GIIHIndAlarmContractName As String = "field_1_4"
	Public Const GIIHIndMtaActionType As String = "field_11_1"
	
	'Insurer specific constants for Aua
	Public Const GIIHGenInsurerAua As String = "GenInsurer1"
	Public Const GIIHGenInsurerAuaTrees As String = "GenInsurer11"
	
	Public Const GIIHAuaNoFlats As String = "field_1_1"
	Public Const GIIHAuaFlNo As String = "field_1_2"
	Public Const GIIHAuaSlopSiteYn As String = "field_1_3"
	Public Const GIIHAuaMinAreaYn As String = "field_1_4"
	Public Const GIIHAuaDrainsYn As String = "field_1_5"
	Public Const GIIHAuaTreesYn As String = "field_1_6"
	Public Const GIIHAuaSpecies As String = "field_11_1"
	Public Const GIIHAuaDistance As String = "field_11_2"
	Public Const GIIHAuaHeight As String = "field_11_3"
	Public Const GIIHAuaOwnLandYn As String = "field_11_4"
	Public Const GIIHAuaBankruptYn As String = "field_1_7"
	Public Const GIIHAuaTenAgreeYn As String = "field_1_8"
	Public Const GIIHAuaTenType As String = "field_1_9"
	Public Const GIIHAuaPeriod As String = "field_1_10"
	Public Const GIIHAuaBedsitYn As String = "field_1_11"
	Public Const GIIHAuaHostelYn As String = "field_1_12"
	Public Const GIIHAuaCommunalYn As String = "field_1_13"
	Public Const GIIHAuaCookingYn As String = "field_1_14"
	Public Const GIIHAuaHeatingYn As String = "field_1_15"
	Public Const GIIHAuaMoreThanYn As String = "field_1_16"
	Public Const GIIHAuaBldRent As String = "field_1_17"
	Public Const GIIHAuaContsRent As String = "field_1_18"
	
	'Insurer specific constants for Alb
	Public Const GIIHGenInsurerAlb As String = "GenInsurer1"
	Public Const GIIHGenInsurerAlbTrees As String = "GenInsurer11"
	
	Public Const GIIHAlbTreesYn As String = "field_1_1"
	Public Const GIIHAlbSpecies As String = "field_11_1"
	Public Const GIIHAlbDistance As String = "field_11_2"
	Public Const GIIHAlbHeight As String = "field_11_3"
	Public Const GIIHAlbLastPruned As String = "field_11_4"
	Public Const GIIHAlbHomeExtendedYn As String = "field_1_2"
	Public Const GIIHAlbHomeReportYn As String = "field_1_3"
	Public Const GIIHAlbOwnPropertyYn As String = "field_1_4"
	Public Const GIIHAlbFloodRiskYn As String = "field_1_5"
	
	'Insurer specific constants for Tow
	Public Const GIIHGenInsurerTow As String = "GenInsurer1"
	
	Public Const GIIHTowUnoccupiedYn As String = "field_1_1"
	Public Const GIIHTowStudentsYn As String = "field_1_2"
	Public Const GIIHTowNumberOccupants As String = "field_1_3"
	Public Const GIIHTowSurveyReportYn As String = "field_1_4"
	Public Const GIIHTowLossRentYn As String = "field_1_5"
	Public Const GIIHTowMonthlyRent As String = "field_1_6"
	Public Const GIIHTowAddPrem As String = "field_1_7"
	Public Const GIIHTowOffencesYn As String = "field_1_8"
	
	'Insurer specific constants for Nig
	Public Const GIIHGenInsurerNig As String = "GenInsurer1"
	
	Public Const GIIHNigBankruptcyYn As String = "field_1_1"
	Public Const GIIHNigCcjYn As String = "field_1_2"
	Public Const GIIHNigExtendedYn As String = "field_1_3"
	Public Const GIIHNigMatchingSets As String = "field_1_4"
	Public Const GIIHNigAdjBuildingVal As String = "field_1_4"
	
	'Insurer specific constants for Zen
	Public Const GIIHGenInsurerZen As String = "GenInsurer1"
	
	Public Const GIIHZenConvictionYn As String = "field_1_1"
	Public Const GIIHZenExtendedYn As String = "field_1_2"
	Public Const GIIHZenYearExtended As String = "field_1_3"
	Public Const GIIHZenBankruptcyYn As String = "field_1_4"
	Public Const GIIHZenStormfloodRiskYn As String = "field_1_5"
	
	'Insurer specific constants for Euclid
	Public Const GIIHGenInsurerEuclid As String = "GenInsurer1"
	
	Public Const GIIHEuclidDocPrivatehouseYn As String = "field_1_5"
	Public Const GIIHEuclidDocProfessionYn As String = "field_1_6"
	Public Const GIIHEuclidGuernseyLoanYn As String = "field_1_7"
	Public Const GIIHEuclidMaintHome As String = "field_1_1"
	Public Const GIIHEuclidSiFull As String = "field_1_2"
	Public Const GIIHEuclidSiMaint As String = "field_1_3"
	Public Const GIIHEuclidSecDevOp As String = "field_1_4"
	
	Public Const GIIHBuildingsSectionBnet As String = "B05"
	Public Const GIIHContentsSectionBnet As String = "C13"
End Module