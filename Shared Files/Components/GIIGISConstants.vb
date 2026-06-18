Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("GIIGISConstants_NET.GIIGISConstants")> _
 Public Module GIIGISConstants
	' ***************************************************************** '
	'
	' GIS Object & Property name constants for Gemini II
	' In Alphabetical order by Object
	'
	' ***************************************************************** '
	
	Public Const GIIMGemPolicy As String = "GIIMGemPolicy"
	'******* Duplicate entry *******
	Public Const GIIGEMPOLICY As String = "GIIMGEMPOLICY"
	
	Public Const MTATypeTemporary As String = "Temporary"
	Public Const MTATypePermanent As String = "Permanent"
	Public Const MTAAdjMainDet As String = "Adjust Main Details"
	Public Const MTAConvPending As String = "Convert Pending"
	Public Const MTACancellation As String = "Cancellation"
	Public Const PMKeyNamePassState As String = "PassState"
	
	'PM 18/02/2005 Broker Details Constants
	Public Const GIIMBroker_Details As String = "Broker_Details"
	Public Const GIIMBroker_DetailsICCS_No As String = "ICCS_No"
	
	'BSJ240300 Calculated Result Constants
	Public Const GIIMCalculatedResult As String = "Calculated_Result"
	Public Const GIIMCalculatedResultGIIMGemPolicyid As String = "GIIMGemPolicy_id"
	Public Const GIIMCalculatedResultGIIMCalculatedResultid As String = "GIIMCalculated_Result_id"
	Public Const GIIMCalculatedResultAdjustmentPremiumexclIPT As String = "Adjustment_Premium_excl_IPT"
	Public Const GIIMCalculatedResultAdjustmentPremiuminclIPT As String = "Adjustment_Premium_incl_IPT"
	Public Const GIIMCalculatedResultCalculatedOverridePct As String = "Calculated_Override_Pct"
	Public Const GIIMCalculatedResultCompulsoryXs As String = "Compulsory_Xs"
	Public Const GIIMCalculatedResultCoverCommenceDate As String = "Cover_Commence_Date"
	Public Const GIIMCalculatedResultCoverPeriod As String = "Cover_Period"
	Public Const GIIMCalculatedResultCoverPeriodUnits As String = "Cover_Period_Units"
	Public Const GIIMCalculatedResultDaysOfCoverSinceInception As String = "Days_Of_Cover_Since_Inception"
	Public Const GIIMCalculatedResultDaysSinceIPTStart As String = "Days_Since_IPT_Start"
	Public Const GIIMCalculatedResultDaysOfCoverThisPeriod As String = "Days_Of_Cover_This_Period"
	Public Const GIIMCalculatedResultDerivedOverridePremiumexclIPT As String = "Derived_Override_Premium_excl_IPT"
	Public Const GIIMCalculatedResultDerivedOverridePremiuminclIPT As String = "Derived_Override_Premium_incl_IPT"
	Public Const GIIMCalculatedResultIPTAmount As String = "IPT_Amount"
	Public Const GIIMCalculatedResultIPTPct As String = "IPT_Pct"
	Public Const GIIMCalculatedResultNew_RiskCalculatedPremium As String = "New_Risk_Calculated_Premium"
	Public Const GIIMCalculatedResultOnPremium As String = "On_Premium"
	Public Const GIIMCalculatedResultOnPremiumVehiclecount As String = "On_Premium_Vehicle_count"
	Public Const GIIMCalculatedResultOffPremium As String = "Off_Premium"
	Public Const GIIMCalculatedResultOffPremiumVehiclecount As String = "Off_Premium_Vehicle_count"
	Public Const GIIMCalculatedResultOriginalPremium As String = "Original_Premium"
	Public Const GIIMCalculatedResultOverrideAmtexclIPT As String = "Override_Amt_excl_IPT"
	Public Const GIIMCalculatedResultOverrideAmtinclIPT As String = "Override_Amt_incl_IPT"
	Public Const GIIMCalculatedResultOverridePct As String = "Override_Pct"
	Public Const GIIMCalculatedResultPremiumexclIPT As String = "Premium_excl_IPT"
	Public Const GIIMCalculatedResultPremiuminclIPT As String = "Premium_incl_IPT"
	Public Const GIIMCalculatedResultPolicyDesc As String = "Policy_Desc"
	Public Const GIIMCalculatedResultQuoteGuaranteedInd As String = "Quote_Guaranteed_Ind"
	Public Const GIIMCalculatedResultRatedArea As String = "Rated_Area"
	Public Const GIIMCalculatedResultRatedTerms As String = "Rated_Terms"
	Public Const GIIMCalculatedResultRatedCategory As String = "Rated_Category"
	Public Const GIIMCalculatedResultRecalculatedOldRiskPremium As String = "Recalculated_Old_Risk_Premium"
	Public Const GIIMCalculatedResultTotalXs As String = "Total_Xs"
	Public Const GIIMCalculatedResultShortPeriodPercentage As String = "Short_Period_Percentage"
	
	'Claim Constants
	Public Const GIIMClaim As String = "Claim"
	Public Const GIIMClaimBodilyInjuryCausedInd As String = "Bodily_Injury_Caused_Ind"
	Public Const GIIMClaimDate As String = "Claim_Date"
	Public Const GIIMClaimType As String = "Claim_Type"
	Public Const GIIMClaimCosts3rdParty As String = "Costs_3rd_Party"
	Public Const GIIMClaimCostsInsd As String = "Costs_Insd"
	Public Const GIIMClaimCostsTotal As String = "Costs_Total"
	Public Const GIIMClaimDriverAtFaultInd As String = "Driver_At_Fault_Ind"
	Public Const GIIMClaimNCDLostInd As String = "NCD_Lost_Ind"
	Public Const GIIMClaimNCBProtectedAtClaim As String = "NCB_Protected_At_Claim"
	
	'Conviction constants
	Public Const GIIMConviction As String = "Conviction"
	Public Const GIIMConvictionAlcoholReading As String = "Alcohol_Reading"
	Public Const GIIMConvictionAlcoholReadingType As String = "Alcohol_Reading_Type"
	Public Const GIIMConvictionDescription As String = "Addl_Desc"
	Public Const GIIMConvictionCode As String = "Code"
	Public Const GIIMConvictionDate As String = "Date"
	Public Const GIIMConvictionPenaltyAmt As String = "Penalty_Amt"
	Public Const GIIMConvictionPenaltyPts As String = "Penalty_Pts"
	Public Const GIIMConvictionDisqualifiedMonths As String = "Suspension_Period"
	
	'Cover Constants
	Public Const GIIMCover As String = "Cover"
	Public Const GIIMCoverYoungestDriverAge As String = "Age_of_Youngest_Driver_for_Open_Driving"
	Public Const GIIMCoverCode As String = "Code"
	Public Const GIIMCoverCNoteContinueInd As String = "CNote_Continuation_Ind"
	Public Const GIIMCoverCNoteDaysDuration As String = "CNote_Days_Duration"
	Public Const GIIMCoverCNoteExpiryDate As String = "CNote_Expiry_Date"
	Public Const GIIMCoverCNoteExpiryTime As String = "CNote_Expiry_Time"
	Public Const GIIMCoverCNoteIssueDesc As String = "CNote_Issue_Desc"
	Public Const GIIMCoverCNoteIssueReason As String = "CNote_Issue_Reason"
	Public Const GIIMCoverCNoteNo As String = "CNote_No"
	Public Const GIIMCoverCNotePrem As String = "CNote_Prem"
	Public Const GIIMCoverCNotePrevNo As String = "CNote_Prev_No"
	Public Const GIIMCoverCNoteStartDate As String = "CNote_Start_Date"
	Public Const GIIMCoverCNoteStartTime As String = "CNote_Start_Time"
	Public Const GIIMCoverCNoteNoOfCNotesIssued As String = "Count_of_Cnotes_Issued"
	Public Const GIIMCoverDriversAllowed As String = "Drivers_Allowed"
	Public Const GIIMCoverRequiredDrivers As String = "Required_Drivers"
	Public Const GIIMCoverPeriod As String = "Period"
	Public Const GIIMCoverPeriodUnits As String = "Period_Units"
	Public Const GIIMCoverStartTime As String = "Start_Date"
	Public Const GIIMCoverPermittedDrivers As String = "Permitted_Drivers"
	Public Const GIIMCoverVolXsAllowed As String = "Vol_Xs_Allowed"
	
	' Declines Constants
	Public Const GIIMDeclines As String = "Declines"
	
	' Decline_Reasons Constants
	Public Const GIIMDeclineReasons As String = "Decline_Reasons"
	Public Const GIIMDeclineReasonsReason As String = "Reason"
	Public Const GIIMDeclineReasonsCode As String = "Code"
	Public Const GIIMDeclineReasonsText As String = "Text"
	
	'Driver Constants
	Public Const GIIMDriver As String = "Driver"
	Public Const GIIMDriverAge As String = "Age"
	Public Const GIIMDriverAnnualMileage As String = "Annual_Mileage"
	Public Const GIIMDriverCountryOfBirth As String = "BirthPlace"
	'JSB - This maped to here because there is nowhere else to map it to, OK!
	Public Const GIIMDRiverBusinessMileage As String = "Annual_Kilometres"
	Public Const GIIMDriverClaimsInd As String = "Claims_Ind"
	Public Const GIIMDriverClassOfUse As String = "Class_Of_Use"
	Public Const GIIMDriverConvictionsInd As String = "Convictions_Ind"
	Public Const GIIMDriverDateOfBirth As String = "Date_Of_Birth"
	Public Const GIIMDriverDisabledBadgeHolderInd As String = "Disabled_Badge_Holder_Ind"
	Public Const GIIMDriverDrivesVehicle1 As String = "Drives_vehicle_1_Ind"
	Public Const GIIMDriverFirstName As String = "Forename_Initial_1"
	Public Const GIIMDriverLicenceDate As String = "Licence_Date"
	Public Const GIIMDriverLicenceType As String = "Licence_Type"
	Public Const GIIMDriverLicenceShownInd As String = "Licence_Validated_Ind"
	Public Const GIIMDriverMaritalStatus As String = "Marital_Status"
	Public Const GIIMDriverMedicalConditionInd As String = "Medical_Condition_Ind"
	Public Const GIIMDriverOnMedicationInd As String = "Medicn_Taken_Ind"
	Public Const GIIMDriverMotoringOrg As String = "Motoring_Organisation_Membership"
	Public Const GIIMDriverClaimsTotal As String = "No_Claims"
	Public Const GIIMDriverConvictionsTotal As String = "No_Convictions"
	Public Const GIIMDriverMedicalConditionsTotal As String = "No_Medical_Conditions"
	Public Const GIIMDriverNoOfChildren As String = "No_of_Children"
	Public Const GIIMDriverNoOfOtherVehiclesOwned As String = "No_of_Other_Vehicles_Owned"
	Public Const GIIMDriverNonMotoringConvictionInd As String = "Non_Motoring_Conviction_Ind"
	Public Const GIIMDriverOtherVehicleOwnedInd As String = "Other_Vehicle_Owned_Ind"
	Public Const GIIMDriverOtherVehicleNCD As String = "Other_Vehicle_Years_Claim_Free"
	Public Const GIIMDriverTermsInd As String = "Prevly_Applied_Incrsd_Prem_Ind"
	Public Const GIIMDriverPermanentlyResidentInd As String = "Permanently_Resident_Ind"
	Public Const GIIMDriverProsecutionPendingInd As String = "Prosecution_Pending_Ind"
	Public Const GIIMDriverPrevTermsDesc As String = "Prev_Terms_Desc"
	Public Const GIIMDriverRefusedCoverDesc As String = "Refused_Cover_Desc"
	'rplaice 12/1/2006 - Add field for Zenith problem PN18690
	Public Const GIIMDriverRefusedCoverDate As String = "Refused_Cover_Date"
	'rplaice 12/1/2006 - end
	Public Const GIIMDriverRefusedCoverInd As String = "Refused_Cover_Ind"
	Public Const GIIMDriverRegdDisabledInd As String = "Regd_Disabled_Ind"
	Public Const GIIMDriverRelationshipToProposer As String = "Relationship_To_Proposer"
	Public Const GIIMDriverSex As String = "Sex"
	Public Const GIIMDriverSmokerInd As String = "Smoker_Ind"
	Public Const GIIMDriverSurname As String = "Surname"
	Public Const GIIMDriverTitle As String = "Title"
	'rplaice 18/1/2006 - Add field for Chaucer Development
	Public Const GIIMDriverTitleDesc As String = "Title_Desc"
	'rplaice 18/1/2006 - end
	Public Const GIIMDriverResidencyDate As String = "UK_Residency_Date"
	Public Const GIIMDriverTypeOfDwelling As String = "Type_Of_Dwelling"
	'RDT021000
	Public Const GIIMDriverNoOfYearsUKResidency As String = "No_Of_Years_UK_Residency"
	Public Const GIIMDriverInsuredForOtherVehiclesInd As String = "Insured_For_Other_Vehicles_Ind"
	
	
	' Endorsements Breakdown Constants
	Public Const GIIMEndorsements_Breakdown As String = "Endorsements_Breakdown"
	Public Const GIIMEndorsements_BreakdownTitle As String = "Title"
	
	'Excess Breakdown constants
	Public Const GIIMExcess_Breakdown As String = "Excess_Breakdown"
	Public Const GIIMExcess_BreakdownAmt As String = "Amt"
	Public Const GIIMExcess_BreakdownDescription As String = "Description"
	Public Const GIIMExcess_BreakdownSectionCode As String = "Section_Code"
	
	'Gem Quote Comfiguration Constants
	Public Const GIIMGemQuoteConfiguration As String = "GemQuoteConfiguration"
	Public Const GIIMGemQuoteConfigCoverType1 As String = "Cover_Type_1"
	Public Const GIIMGemQuoteConfigCoverType2 As String = "Cover_Type_2"
	Public Const GIIMGemQuoteConfigCoverType3 As String = "Cover_Type_3"
	Public Const GIIMGemQuoteConfigProtectedNCB As String = "Protected_NCB"
	Public Const GIIMGemQuoteConfigRequestedExcess As String = "Excess_Level_Req"
	
	'Selected Endorsements constants
	Public Const GIIMSelected_Endorsement As String = "ENDORSEMENTS_SELECTED"
	Public Const GIIM_Endorsement_MileageCode As String = "GIIM_ENDORSEMENT_MILEAGECODE"
	Public Const GIIM_Endorsement_SecurityCode As String = "GIIM_ENDORSEMENT_SECURITYCODE"
	Public Const GIIM_Endorsement_TotalCode As String = "GIIM_ENDORSEMENT_TOTALCODE"
	Public Const GIIM_Endorsement_AgeCode As String = "GIIM_ENDORSEMENT_AGECODE"
	
	'Incident Constants
	Public Const GIIMIncident As String = "Incident"
	Public Const GIIMIncidentDate As String = "Date"
	Public Const GIIMIncidentDescription As String = "Description"
	Public Const GIIMIncidentPendingProsecutionInd As String = "Pending_Prosecution_Ind"
	
	' LegacyDriver
	'Insurer Specific Policy properties not part of Polaris model
	' These are part of the GIIMLegacyDriver Object
	Public Const GIIMLegacyDriver As String = "Legacy_Driver"
	Public Const GIIMLegacyDriverCreditCard As String = "Credit_Card"
	Public Const GIIMLegacyDriverHasCompanyCarInd As String = "Has_Company_Car_Ind"
	Public Const GIIMLegacyDriverLivesAtParentsAddressInd As String = "Lives_At_Parents_Address_Ind"
	Public Const GIIMLegacyDriverNIGTeetotal As String = "NIG_Teetotal"
	Public Const GIIMLegacyDriverTempUKResident As String = "Temp_UK_Resident_YN"
	Public Const GIIMLegacyDriverOtherVehicleInsurer As String = "Other_Vehicle_Details"
	Public Const GIIMLegacyDriverOtherVehicleDetails As String = "Other_Vehicle_Details"
	Public Const GIIMLegacyDriverOwnsOtherVehicle As String = "Owns_Other_Vehicle_Details"
	Public Const GIIMLegacyDriverOtherVehcileUse As String = "Use_Other_Vehicle_Details"
	Public Const GIIMLegacyDriverYrsUKDrivExp As String = "Years_UK_Driving_Experience"
	Public Const GIIMLegacyDriverCountryOfBirth As String = "County_of_Birth"
	Public Const GIIMLegacyDriverNumVehiclesOwned As String = "Num_Vehicles_owned"
	
	'Legacy Policy
	'Insurer Specific Policy properties not part of Polaris model
	' This is part of the GIIMLegacyPolicy Object
	Public Const GIIMLegacyPolicy As String = "Legacy_Policy"
	Public Const GIIMLegacy_Policy As String = "Legacy_Policy"
	Public Const GIIMLegacyPolicyAccomStatus As String = "Accommodation_Status"
	Public Const GIIMLegacyPolicyBrkOvrOption As String = "broke_ovr_option"
	Public Const GIIMLegacyPolicyCCJYN As String = "ccj_yn"
	Public Const GIIMlegacyPolicyConvOrPendProsecutionYN As String = "conv_or_pend_prosecution_yn"
	Public Const GIIMLegacyPolicyDaytimeParkingLocation As String = "Day_Time_Parking_locn"
	Public Const GIIMLegacyPolicyHirePurchase As String = "ibex_hire_purchase_yn"
	Public Const GIIMLegacyPolicyImobiliserFitted As String = "Immobiliser_Fitted"
	Public Const GIIMLegacyPolicyResident3YrsPlus As String = "lg_dri_resi_yn"
	Public Const GIIMLegacyPolicyReplacement4CompCar As String = "lg_replace_company_car"
	Public Const GIIMLegacyPolicyNumDoorsOnVehicle As String = "lg_veh_doors"
	Public Const GIIMLegacyPolicyDerivedField_1 As String = "lp_derived_value_1"
	Public Const GIIMLegacyPolicyDerivedField_2 As String = "lp_derived_value_2"
	Public Const GIIMLegacyPolicyDerivedField_3 As String = "lp_derived_value_3"
	Public Const GIIMLegacyPolicyDerivedField_4 As String = "lp_derived_value_4"
	Public Const GIIMLegacyPolicyDerivedField_5 As String = "lp_derived_value_5"
	Public Const GIIMLegacyPolicyDerivedField_6 As String = "lp_derived_value_6"
	Public Const GIIMLegacyPolicyDerivedField_7 As String = "lp_derived_value_7"
	Public Const GIIMLegacyPolicyDerivedField_8 As String = "lp_derived_value_8"
	Public Const GIIMLegacyPolicyMotoringOrgDesc As String = "zu_motoring_org_desc"
	Public Const GIIMLegacyPolicyNCDEarnedOnThisVehicle As String = "NCD_Earned_On_This_Vehicle"
	Public Const GIIMLegacyPolicyNoOfYearsAtPresentAddress As String = "No_Of_Years_At_Present_Address"
	Public Const GIIMLegacyPolicyNURestrictAmt As String = "nu_restrict_amount"
	Public Const GIIMLegacyPolicySecurity2BFittedIn30Days As String = "nu_security_in30_yn"
	Public Const GIIMLegacyPolicyNuYngDrivTxt As String = "nu_yng_driv_txt"
	Public Const GIIMLegacyPolicyNuYngDrivVeh As String = "Nu_Yng_Driv_Veh"
	Public Const GIIMLegacyPolicyNumVehiclesOwned As String = "Num_Vehicles_owned"
	Public Const GIIMLegacyPolicyProvidTypeHome As String = "provid_type_home"
	Public Const GIIMLegacyPolicyMobilePhone As String = "prp_mobile_phone_no"
	Public Const GIIMLegacyPolicyPreviouslyWithSyndInd As String = "prev_with_syn_yn"
	Public Const GIIMPolicyReasonForGapInCover As String = "Reason_for_gap_in_cover"
	Public Const GIIMLegacyPolicyRestrictMileageInd As String = "Restrict_Mileage_yn"
	Public Const GIIMLegacyPolicyTrailerDesc As String = "ri_trailer_desc"
	Public Const GIIMLegacyPolicyTrailerCoverReq As String = "ri_trailer_yn"
	Public Const GIIMLegacyPolicyLivestockTrailerReq As String = "ri_trailer_type2_yn"
	Public Const GIIMLegacyPolicyTrailerValue As String = "ri_trailer_value"
	Public Const GIIMLegacyPolicyLivestockTrailFttedLgly As String = "ri_trailer_type1_yn"
	Public Const GIIMLegacyPolicyVehicleUsedInCarriageOfGoods As String = "use_extra_1"
	Public Const GIIMLegacyPolicyOTVMakeModel As String = "ri_otv_mk_mod"
	Public Const GIIMLegacyPolicyOTVRegMark As String = "ri_otv_regn"
	Public Const GIIMLegacyPolicyOTVInsurer As String = "ri_otv_ins"
	Public Const GIIMLegacyPolicyOTVPolicyNo As String = "ri_otv_pol_no"
	Public Const GIIMLegacyPolicyOTVRenewalDate As String = "ri_otv_ren_date"
	Public Const GIIMLegacyPolicyOTVPolicyHolderName As String = "ri_otv_pol_hold"
	Public Const GIIMLegacyPolicyOTVMainDriverName As String = "ri_otv_main_driv"
	Public Const GIIMLegacyPolicyUsedInCarriageOfGoodsDesc As String = "ri_use_goods_desc"
	Public Const GIIMLegacyPolicyUsedForBusinessByOthers As String = "use_bus_oth"
	Public Const GIIMLegacyPolicyBusinessUseDesc As String = "ri_use_bus_desc"
	Public Const GIIMLegacyPolicyUsedForCommercialTravel As String = "use_com_travl"
	Public Const GIIMLegacyPolicyCommercialTravelDesc As String = "ri_use_com_desc"
	Public Const GIIMLegacyPolicyMotorTradeUse As String = "use_mot_trade"
	Public Const GIIMLegacyPolicyUsedForHireOrReward As String = "use_hire_reward"
	Public Const GIIMLegacyPolicyImportedVehicleInd As String = "zen_imported_yn"
	Public Const GIIMLegacyPolicyMadeForEuroUseInd As String = "zen_orig_european_yn"
	Public Const GIIMLegacyPolicyMotoringOrgCode As String = "zu_motoring_org_code"
	Public Const GIIMLegacyPolicyMotoringOrgRenewalDate As String = "zu_motoring_org_renewal"
	Public Const GIIMLegacyPolicyOverridePremiumYN As String = "zu_override_prem_yn"
	' 04/04/01 TB - Unlisted vehicle
	Public Const GIIMLegacyPolicyUnlistVehMkDesc As String = "Unlist_Veh_Mk_Desc"
	Public Const GIIMLegacyPolicyUnlistVehModelDesc As String = "Unlist_Veh_Model_Desc"
	Public Const GIIMLegacyPolicyVehUnlistedYN As String = "Veh_Unlisted_YN"
	Public Const GIIMLegacyPolicyVehSelfBuiltYN As String = "Veh_Self_Built_YN"
	'JSB 14/05/01
	Public Const GIIMLegacyPolicyLicenceIssueCountry As String = "Licence_Issue_Country"
	'JSB 30/08/01
	Public Const GIILegacyPolicyShowDiscount As String = "Show_Discount_YN"
	Public Const GIIMLegacyPolicyPropNumOtherVehOwnUse As String = "prp_Num_other_Veh_own"
	Public Const GIIMLegacyPolicyVehicleUKModelYN As String = "Vehicle_UK_Model_yn"
	'JSB 7/11/01
	Public Const GIIMLegacyPolicyVehicleOwnerDesc As String = "Vehicle_Owner_Desc"
	Public Const GIIMLegacyPolicyVehicleKeeperDesc As String = "vehicle_keeper_desc"
	'JSB 26/11/01
	Public Const GIIMLegacyPolicyNCDEarnedThroughCompanyCarInd As String = "NCD_Gained_Comp_Car_Use_YN"
	Public Const GIIMLegacyPolicyICESerialNumber As String = "ICE_Serial_No"
	Public Const GIIMLegacyPolicyWorkOutsideNIInd As String = "Work_Outside_NI_YN"
	Public Const GIIMLegacyPolicyNCDReduced As String = "NCD_Reduced_YN"
	Public Const GIIMLegacyPolicyForeignUse3Months As String = "Use_Outsie_UK_For_3_Months"
	Public Const GIIMLegacyPolicyPreviousMotInsHeld As String = "Previously_Held_Motor_Ins_Ind"
	Public Const GIIMLegacyPolicySpecTermsAppliedDate As String = "Special_Terms_Applied_Date"
	Public Const GIIMLegacyPolicySyndicateDesc As String = "Prev_Synd_Desc"
	' TB 13/12/01 - MTA Date constants
	Public Const GIIMLegacyPolicyMTAStartDate As String = "MTA_Start_Date"
	Public Const GIIMLegacyPolicyMTAStartTime As String = "MTA_Start_Time"
	Public Const GIIMLegacyPolicyMTAEndDate As String = "MTA_End_Date"
	Public Const GIIMLegacyPolicyMTAEndTime As String = "MTA_End_Time"
	Public Const GIIMLegacyPolicyPropOwnUseAnotherVeh As String = "Own_Use_Another_Vehicle_YN"
	Public Const GIIMLegacyPolicyCompCarDisclaimerYN As String = "lg_comp_car_disclaimer_YN"
	Public Const GIIMLegacyPolicyDwellingType As String = "qmm_dwelling_type"
	'JSB 1/11/02 - BD Other vehicle questions
	Public Const GIIMLegacyPolicyBDNumOtherVehOwned As String = "bd_num_other_veh_owned"
	Public Const GIIMLegacyPolicyBDNumOtherVehUsed As String = "bd_num_other_veh_used"
	Public Const GIIMLegacyPolicyClaimSettledYN As String = "mta_claim_settled_yn"
	Public Const GIIMLegacyPolicyBrokeOvrTargPrem As String = "broke_ovr_targ_prem"
	Public Const GIIMLegacyPolicyBrokeCommisOverride As String = "broke_commis_override"
	Public Const GIIMLegacyPolicyBrokeTargCommis As String = "broke_targ_commis"
	Public Const GIIMLegacyPolicyBrokeTargPremium As String = "broke_targ_premium"
	Public Const GIIMLegacyPolicyBrokerDirectData As String = "broker_direct_data"
	'SJD 12/1/2005 - CMIB
	Public Const GIIMLegacyPolicyCornCmibAgreedyn As String = "corn_cmib_agreed_yn"
	Public Const GIIMLegacyPolicyCornCmibAgreedVal As String = "corn_cmib_agreed_val"
	Public Const GIIMLegacyPolicyCornCmibReinst25 As String = "corn_cmib_reinst_25"
	Public Const GIIMLegacyPolicyCornCmibReinst50 As String = "corn_cmib_reinst_50"
	Public Const GIIMLegacyPolicyCornCmibExpiryDate As String = "corn_cmib_expiry_date"
	'rplaice 11/4/2005 - F175944 - RSA criminal conviction fields
	Public Const GIIMLegacyPolicyRSACrimConvsYN As String = "ri_crim_convs_yn"
	Public Const GIIMLegacyPolicyRSACrimConvsDesc As String = "ri_crim_convs_desc"
	'rplaice 07/09/2005 - RFC1379 - NIG endorsement fields
	Public Const GIIMLegacyPolicyNIGEndSalvageRetn As String = "nig_end_salvage_retn"
	Public Const GIIMLegacyPolicyNIGEndAgreedValue As String = "nig_end_agreed_value"
	'SJD 15/11/2005
	Public Const GIIMLegacyPolicyMtaAtRenewalYN As String = "mta_at_renewal_yn"
	Public Const GIIMLegacyPolicyNuGoldAdjPrem As String = "nu_gold_adj_prem"
	Public Const GIIMLegacyPolicyNuEnhancedMtaInd As String = "nu_enhanced_mta_ind"
	Public Const GIIMLegacyPolicyERSEndE886 As String = "ers_end_e886_yn"
	Public Const GIIMLegacyPolicyERSEndE888 As String = "ers_end_e888_yn"
	Public Const GIIMLegacyPolicyERSEndE889 As String = "ers_end_e889_yn"
	Public Const GIIMLegacyPolicySchemeTransferredInd As String = "scheme_transferred_ind"
	
	'Licence Restrictions
	Public Const GIIMLicenceRestrictions As String = "Licence_Restrictions"
	Public Const GIIMLicenceRestrictionsReason As String = "Reason"
	
	'Medical condition constants
	Public Const GIIMMedicalCondition As String = "Medical_Condition"
	Public Const GIIMMedicalConditionDescription As String = "Addl_Desc"
	Public Const GIIMMedicalConditionDateSustained As String = "Date_Of_Onset"
	Public Const GIIMMedicalConditionInfirmityCode As String = "Description"
	Public Const GIIMMedicalconditionDVLCNotifiedInd As String = "DVLA_Advised_Ind"
	Public Const GIIMMedicalConditionLastOccurred As String = "Last_Occurrence_Date"
	Public Const GIIMMedicalRestrictionsReason As String = "Restrict_Reason"
	Public Const GIIMMedicalRestrictionsMonths As String = "Months_Restricted"
	
	'Modification Constants
	Public Const GIIMModification As String = "Modifications"
	Public Const GIIMModificationDescription As String = "Description"
	Public Const GIIMModificationForMedicalConditionInd As String = "For_Medical_Condition_Ind"
	
	'NCD Constants
	Public Const GIIMNCD As String = "NCD"
	Public Const GIIMNCDClaimedYears As String = "Claimed_Years"
	Public Const GIIMNCDClaimedDiscountType As String = "Claimed_Discount_Type"
	Public Const GIIMNCDClaimedPreviousInsurer As String = "Claimed_Previous_Insurer"
	Public Const GIIMNCDClaimedPrevPolExpryDate As String = "Claimed_Previous_Policy_Expiry_date"
	Public Const GIIMNCDClaimedPrevPolNumber As String = "Claimed_Previous_Policy_Number"
	Public Const GIIMNCDClaimedProtectionReqdInd As String = "Claimed_Protection_Reqd_Ind"
	Public Const GIIMNCDHowEarned As String = "Claimed_Entitlement_Reason"
	Public Const GIIMNCDClaimedProvenInd As String = "Claimed_Proven_Ind"
	Public Const GIIMNCDClaimedProtectedYears As String = "Claimed_Protected_Years"
	'RDT021000
	Public Const GIIMNCDClaimedProtectedInd As String = "Claimed_Protected_Ind"
	Public Const GIIMNCDGrantedDiscountType As String = "Granted_Discount_Type"
	Public Const GIIMNCDGrantedEntitlementReason As String = "Granted_Entitlement_Reason"
	Public Const GIIMNCDGrantedPct As String = "Granted_Pct"
	Public Const GIIMNCDGrantedProtectedInd As String = "Granted_Protected_Ind"
	Public Const GIIMNCDGrantedProtectedYears As String = "Granted_Protected_Years"
	Public Const GIIMNCDGrantedYears As String = "Granted_Years"
	
	' Notes Breakdown constants
	Public Const GIIMNotes_Breakdown As String = "Notes_Breakdown"
	Public Const GIIMNotes_BreakdownValue1 As String = "Value1"
	Public Const GIIMNotes_BreakdownValue2 As String = "Value2"
	
	'Occupation constants
	Public Const GIIMOccupation As String = "Occupation"
	Public Const GIIMOccupationCode As String = "Code"
	Public Const GIIMOccupationEmploymentStatus As String = "Employment_Type"
	Public Const GIIMOccupationEmployersBusiness As String = "Employers_Business"
	Public Const GIIMOccupationFullTimeEmploymentInd As String = "Full_Time_Employment_Ind"
	
	'Other Insurances constants
	Public Const GIIMOtherInsurances As String = "Other_Insurances"
	Public Const GIIMOtherInsurancesInsuranceCurrentInd As String = "Currently_Insd_Ind"
	Public Const GIIMOtherInsurancesInsurer As String = "Insurer"
	Public Const GIIMOtherInsurancesClaimFreeYears As String = "No_of_Years_Claim_Free"
	Public Const GIIMOtherInsurancesType As String = "Type"
	
	'Payment Bank
	Public Const GIIMPayment_Bank As String = "Payment_Bank"
	Public Const GIIMPBAmt_First_Instalment As String = "amt_first_instalment"
	Public Const GIIMPBAnnual_Premium_Calcd_By_Sender As String = "annual_prem_calcd_by_sender"
	Public Const GIIMPBBBS_Account_No As String = "BBS_Account_No"
	Public Const GIIMPBBBS_Address_Line_1 As String = "BBS_Address_Line_1"
	Public Const GIIMPBBBS_Address_Line_2 As String = "BBS_Address_Line_2"
	Public Const GIIMPBBBS_Address_Line_3 As String = "BBS_Address_Line_3"
	Public Const GIIMPBBBS_Address_Line_4 As String = "BBS_Address_Line_4"
	' 23/01/2001 PSA
	Public Const GIIMPBBBS_Address_Line_5 As String = "BBS_Address_Line_5"
	Public Const GIIMPBBBS_Address_Line_6 As String = "BBS_Address_Line_6"
	' 23/01/2001 PSA
	Public Const GIIMPBBBS_Branch As String = "BBS_Branch"
	Public Const GIIMPBBBS_Name As String = "BBS_Name"
	Public Const GIIMPBBBS_Post_Code As String = "BBS_Post_Code"
	Public Const GIIMPBBBS_Sort_Code As String = "BBS_Sort_Code"
	Public Const GIIMPBCCard_Expiry_Date As String = "CCard_Expiry_Date"
	Public Const GIIMPBCCard_Holder_Name As String = "CCard_Holder_Name"
	Public Const GIIMPBCCard_Holder_Tel_No As String = "CCard_Holder_Tel_No"
	Public Const GIIMPBCCard_Issue_No As String = "CCard_Issue_No"
	Public Const GIIMPBCCard_No As String = "CCard_No"
	Public Const GIIMPBCRA_Signed_on_Premises_Ind As String = "CRA_Signed_on_Premises_Ind"
	Public Const GIIMPBCredit_Charge As String = "Credit_Charge"
	Public Const GIIMPBDDM_Signed As String = "DDM_Signed"
	Public Const GIIMPBDeposit As String = "Deposit"
	Public Const GIIMPBDeposit_Received As String = "Deposit_Received"
	Public Const GIIMPBInsr_Pmt_Type As String = "Insr_Pmt_Type"
	Public Const GIIMPBInsr_Pmt_Method_Type As String = "Insr_Pmt_Method_Type"
	Public Const GIIMPBInsr_Pmt_Method_Type_Org As String = "Insr_Pmt_Method_Type_Org"
	Public Const GIIMPBNext_Instalment_Date As String = "Next_Instalment_Date"
	Public Const GIIMPBNo_of_Instalments As String = "No_of_Instalments"
	' 12042000 - PSA ****************************************
	Public Const GIIMPBPay_Pref_Instal_Day As String = "Pay_Pref_Instal_Day"
	' 12042000 - PSA ****************************************
	
	'Payment Type
	Public Const ACPayTypeCash As String = "Cash"
	Public Const ACPayTypeCheque As String = "Cheque"
	Public Const ACPayTypeCreditCard As String = "Credit Card"
	Public Const ACPayTypeDebitCard As String = "Debit Card"
	Public Const ACPayTypeDirectDebit As String = "Direct Debit"
	' 09052000 - PSA ****************************************
	Public Const ACPayTypeInsurersOwn As String = "Insurers Own"
	' 09052000 - PSA ****************************************
	Public Const ACPayTypeStandingOrder As String = "Standing Order"
	
	'Policy constants
	Public Const GIIMPolicy As String = "Policy"
	Public Const GIIMPolicyEffectiveStartDate As String = "Effective_Start_Date"
	Public Const GIIMPolicyEffectiveStartTime As String = "Effective_Start_Time"
	Public Const GIIMPolicyExpiryDate As String = "Expiry_Date"
	Public Const GIIMPolicyPrevExpiryDate As String = "Prev_Expiry_Date"
	Public Const GIIMPolicyPrevInsr As String = "Prev_Insr"
	Public Const GIIMPolicyPrevPolicyNo As String = "Prev_Policy_No"
	Public Const GIIMPolicyPolicyNo As String = "Policy_No"
	Public Const GIIMPolicyPrevInsPayFreq As String = "Pre_Ins_Pay_Freq"
	Public Const GIIMPolicyStatus As String = "Status"
	Public Const GIIMPolicyExpiryTime As String = "Expiry_Time"
	Public Const GIIMPolicyDtiReportingCode As String = "Dti_Reporting_Code"
	
	' Premium Analysis Constants
	Public Const GIIMPremiumAnalysis As String = "Premium_Analysis"
	Public Const GIIMPremiumAnalysisAmount As String = "Amount"
	Public Const GIIMPremiumAnalysisCode As String = "Code"
	Public Const GIIMPremiumAnalysisDescription As String = "Description"
	Public Const GIIMPremiumAnalysisRunningTotal As String = "Running_Total"
	Public Const GIIMPremiumAnalysisID As String = "GIIMPREMIUM_ANALYSIS_ID"
	
	'BB181199
	' Proposer
	Public Const GIIMPropPolicyholder As String = "Proposer_Policyholder"
	'******* Duplicate entry *******
	Public Const GIIMProposer As String = "Proposer_Policyholder"
	Public Const GIIMProposerAddressLine1 As String = "Address_Line_1"
	Public Const GIIMProposerAddressLine2 As String = "Address_Line_2"
	Public Const GIIMProposerAddressLine3 As String = "Address_Line_3"
	Public Const GIIMProposerAddressLine4 As String = "Address_Line_4"
	Public Const GIIMProposerAddressPostCode As String = "Address_Post_Code"
	Public Const GIIMProposerCountryOfBirth As String = "BirthPlace"
	Public Const GIIMProposerDateOfBirth As String = "Date_of_Birth"
	Public Const GIIMProposerFirstName As String = "Forename_Initial_1"
	Public Const GIIMProposerHomeOwnerInd As String = "Homeowner_Ind"
	Public Const GIIMProposerMaritalStatus As String = "Marital_Status"
	Public Const GIIMProposerNumVehInFamily As String = "No_of_Vehicles_Available_to_Family"
	Public Const GIIMProposerSex As String = "Sex"
	Public Const GIIMProposerSurname As String = "Surname"
	Public Const GIIMProposerHomePhone As String = "Tel_No_Home"
	Public Const GIIMProposerWorkPhone As String = "Tel_No_Work"
	Public Const GIIMProposerTitle As String = "Title_Code"
	Public Const GIIMProposerTitleText As String = "Title_Text"
	Public Const GIIMProposerType As String = "Type"
	Public Const GIIMProposerYearsAtAddress As String = "Years_at_Home_Address"
	
	
	
	'BB251199 Insurer Specific Proposer properties not part of Polaris model
	' These are part of the GIIMLegacyPolicy Object see above
	Public Const GIIMProposerNCD4YearsInd As String = "NCD_4_Years_Ind"
	Public Const GIIMProposerInsurer3YearsInd As String = "Insurer_3_Years_Ind"
	Public Const GIIMProposerAgent3YearsInd As String = "Agent_3_Years_Ind"
	Public Const GIIMProposerNUHousehold3YearsInd As String = "NU_Household_3_Years_Ind"
	Public Const GIIMProposerOtherVehicle5YearsInd As String = "Other_Vehicle_5_Years_Ind"
	'sj 18/01/00 - start
	Public Const GIIMProposerInsInrDesc As String = "Prp_Ins_Inr_Desc"
	Public Const GIIMProposerInsInrCode1 As String = "Prp_Ins_Inr_Code_1"
	'sj 18/01/00 - end
	
	'BSJ240300 Quick Quote Result Constants
	Public Const GIIMQuickQuoteResult As String = "Quick_Quote_Result"
	Public Const GIIMQuickQuoteResultCommissionamount As String = "Commission_amount"
	Public Const GIIMQuickQuoteResultCommissionOverrideAmt As String = "Commission_Override_Amt"
	Public Const GIIMQuickQuoteResultCommissionOverridePct As String = "Commission_Override_Pct"
	Public Const GIIMQuickQuoteResultCommissionrate As String = "Commission_rate"
	Public Const GIIMQuickQuoteResultCompulsoryexcess As String = "Compulsory_excess"
	Public Const GIIMQuickQuoteResultCompulsoryNonADDesc As String = "Compulsory_Non_AD_Desc"
	Public Const GIIMQuickQuoteResultCompulsoryNonADExcess As String = "Compulsory_Non_AD_Excess"
	Public Const GIIMQuickQuoteResultCovertype As String = "Cover_type"
	Public Const GIIMQuickQuoteResultIPT As String = "IPT"
	Public Const GIIMQuickQuoteResultNCBprotected As String = "NCB_protected"
	Public Const GIIMQuickQuoteResultNCDDiscount As String = "NCD_Discount"
	Public Const GIIMQuickQuoteResultNCDYears As String = "NCD_Years"
	Public Const GIIMQuickQuoteResultOverrideAmount As String = "Override_Amount"
	Public Const GIIMQuickQuoteResultOverrideAuthorisationCode As String = "Override_Authorisation_Code"
	Public Const GIIMQuickQuoteResultOverrideGrossPremium As String = "Override_Gross_Premium"
	Public Const GIIMQuickQuoteResultOverridePercent As String = "Override_Percent"
	Public Const GIIMQuickQuoteResultOverridePremiumFromPot As String = "Override_Premium_From_Pot"
	Public Const GIIMQuickQuoteResultPremium As String = "Premium"
	Public Const GIIMQuickQuoteResultQuotetype As String = "Quote_type"
	Public Const GIIMQuickQuoteResultRatesDate As String = "Rates_Date"
	Public Const GIIMQuickQuoteResultInsurerMnemonic As String = "Insurer_Mnemonic"
	Public Const GIIMQuickQuoteResultSchemeID As String = "Scheme_ID"
	Public Const GIIMQuickQuoteResultSchemeDesc As String = "Scheme_Desc"
	Public Const GIIMQuickQuoteResultSchemeType As String = "Scheme_Type"
	Public Const GIIMQuickQuoteResultTotalexcess As String = "Total_excess"
	Public Const GIIMQuickQuoteResultVehicleArea As String = "Vehicle_Area"
	Public Const GIIMQuickQuoteResultVehicleGroup As String = "Vehicle_Group"
	Public Const GIIMQuickQuoteResultVoluntaryexcess As String = "Voluntary_excess"
	Public Const GIIMQuickQuoteResultWindscreenExcess As String = "Windscreen_Excess"
	Public Const GIIMQuickQuoteResultWindscreenLimit As String = "Windscreen_Limit"
	Public Const GIIMQuickQuoteResultYoungDriverExcess As String = "Young_Driver_Excess"
	Public Const GIIMQuickQuoteResultCommissionMinimum As String = "Commission_Minimum"
	Public Const GIIMQuickQuoteResultOverrideReason As String = "Override_Reason"
	
	' SJD 17/6/2005
	Public Const GIIMQuickQuoteResultOverrideCompulsoryexcess As String = "Override_Compulsory_excess"
	Public Const GIIMQuickQuoteResultOverrideCompulsoryNonADExcess As String = "Override_Compulsory_Non_AD_Excess"
	Public Const GIIMQuickQuoteResultOverrideVoluntaryexcess As String = "Override_Voluntary_excess"
	Public Const GIIMQuickQuoteResultOverrideWindscreenExcess As String = "Override_Windscreen_Excess"
	Public Const GIIMQuickQuoteResultOverrideYoungDriverExcess As String = "Override_Young_Driver_Excess"
	
	'Reffer Reasons Constants
	Public Const GIIMRefer_Reason As String = "Refer_Reasons"
	
	'Refferal Constants
	Public Const GIIMReferrals As String = "Referrals"
	
	'Qualification Constants
	Public Const GIIMQualification As String = "Qualification"
	Public Const GIIMQualificationDescription As String = "Description"
	Public Const GIIMQualificationDate As String = "Date"
	
	' Saved_Add_On Constants
	Public Const GIIMSaved_Add_On As String = "Saved_Add_On"
	Public Const GIIMSaved_Add_On_Desc As String = "Description"
	Public Const GIIMSaved_Add_On_Grs_Cost As String = "Gross_Cost"
	Public Const GIIMSaved_Add_On_Net_Cost As String = "Net_Cost"
	Public Const GIIMSaved_Add_On_Date As String = "Add_On_Date"
	' JSB 030101 - Added following constant here even though its not in the polaris dictionairy
	Public Const GIIMAdd_On_Def_ID As String = "add_on_def_id"
	
	'Saved Analysis Constants
	Public Const GIIMSaved_Analysis As String = "Saved_Analysis"
	
	'Saved Excess constants
	Public Const GIIMSaved_Excess As String = "Saved_Excess"
	
	'Saved Notes Constants
	Public Const GIIMSaved_Notes As String = "Saved_Notes"
	
	'Saved Quote Constants
	Public Const GIIMSaved_Quote As String = "Saved_Quote"
	Public Const GIIMSaved_Quote_Commis_Amt As String = "Commission_amount"
	Public Const GIIMSaved_Quote_Commis_Ovrd_Amt As String = "Commission_Override_Amt"
	Public Const GIIMSaved_Quote_Commis_Ovrd_Pct As String = "Commission_Override_Pct"
	Public Const GIIMSaved_Quote_Commis_Rate As String = "Commission_rate"
	Public Const GIIMSaved_Quote_Comp_Xs As String = "Compulsory_excess"
	Public Const GIIMSaved_Quote_Comp_Non_AD_Desc As String = "Compulsory_Non_AD_Desc"
	Public Const GIIMSaved_Quote_Comp_Non_AD_Xs As String = "Compulsory_Non_AD_Excess"
	Public Const GIIMSaved_Quote_Cover_Type As String = "Cover_type"
	Public Const GIIMSaved_Quote_IPT As String = "IPT"
	Public Const GIIMSaved_Quote_NCB_Protected As String = "NCB_protected"
	Public Const GIIMSaved_Quote_NCD_Discount As String = "NCD_Discount"
	Public Const GIIMSaved_Quote_NCD_Years As String = "NCD_Years"
	Public Const GIIMSaved_Quote_Ovrd_Amt As String = "Override_Amount"
	Public Const GIIMSaved_Quote_Ovrd_Auth_Code As String = "Override_Authorisation_Code"
	Public Const GIIMSaved_Quote_Ovrd_GP As String = "Override_Gross_Premium"
	Public Const GIIMSaved_Quote_Ovrd_Pct As String = "Override_Percent"
	Public Const GIIMSaved_Quote_Ovrd_Prem_From_Pot As String = "Override_Premium_From_Pot"
	Public Const GIIMSaved_Quote_Premium As String = "Premium"
	Public Const GIIMSaved_Quote_Quote_Type As String = "Quote_Type"
	Public Const GIIMSaved_Quote_Rates_Date As String = "Rates_Date"
	Public Const GIIMSaved_Quote_Scheme_Desc As String = "Scheme_Desc"
	Public Const GIIMSaved_Quote_Scheme_ID As String = "Scheme_ID"
	Public Const GIIMSaved_Quote_Scheme_Type As String = "Scheme_Type"
	Public Const GIIMSaved_Quote_Total_Xs As String = "Total_Xs"
	Public Const GIIMSaved_Quote_Vehicle_Area As String = "Vehicle_Area"
	Public Const GIIMSaved_Quote_Vehicle_Group As String = "Vehicle_Group"
	Public Const GIIMSaved_Quote_Vol_Xs As String = "Voluntary_excess"
	Public Const GIIMSaved_Quote_Windscreen_Xs As String = "Windscreen_Excess"
	Public Const GIIMSaved_Quote_Windscreen_Limit As String = "Windscreen_Limit"
	Public Const GIIMSaved_Quote_Young_Driver_Xs As String = "Young_Driver_Excess"
	Public Const GIIMSaved_Quote_Commis_Minimum As String = "Commission_Minimum"
	' KB 100604
	Public Const GIIMSaved_Quote_Insurer_Mnemonic As String = "Insurer_Mnemonic"
	Public Const GIIMSaved_Quote_New_Risk_CP As String = "New_Risk_Calculated_Premium"
	
	
	' SJD 17/6/2005
	Public Const GIIMSaved_Quote_Override_Comp_Xs As String = "Override_Compulsory_excess"
	Public Const GIIMSaved_Quote_Override_Comp_Non_AD_Xs As String = "Override_Compulsory_Non_AD_Excess"
	Public Const GIIMSaved_Quote_Override_Vol_Xs As String = "Override_Voluntary_excess"
	Public Const GIIMSaved_Quote_Override_Windscreen_Xs As String = "Override_Windscreen_Excess"
	Public Const GIIMSaved_Quote_Override_Young_Driver_Xs As String = "Override_Young_Driver_Excess"
	
	'Security constants
	Public Const GIIMSecurity As String = "Security"
	Public Const GIIMSecurityCertSeenInd As String = "Cert_Seen_Ind"
	Public Const GIIMSecurityCertNo As String = "Cert_Tracker_No"
	Public Const GIIMSecurityFittedDate As String = "Fitted_Date"
	Public Const GIIMSecurityInstaller As String = "Installer"
	Public Const GIIMSecuritySystemMakeAndModel As String = "System_Make_And_Model"
	'JRD Added constant for BS6803Install
	Public Const GIIMSecurityBS6803Install As String = "BS6803_CERT_IND"
	
	'Selected add on constants
	Public Const GIIMSelected_Add_On As String = "Selected_Add_On"
	Public Const GIIMSelected_Add_On_Desc As String = "Description"
	Public Const GIIMSelected_Add_On_Grs_Cost As String = "Gross_Cost"
	Public Const GIIMSelected_Add_On_Net_Cost As String = "Net_Cost"
	Public Const GIIMSelected_Add_On_Date As String = "Add_On_Date"
	
	' Uses Constants
	Public Const GIIMUses As String = "Uses"
	Public Const GIIMUsesUse As String = "Use"
	Public Const GIIMUsesABICode As String = "ABI_Code"
	
	'SK Selected Endorsements constants
	Public Const GIIM_Selected_Endorsement As String = "ENDORSEMENTS_SELECTED"
	Public Const GIIMEndorsement_MileageCode As String = "ENDORSEMENT_MILEAGECODE"
	Public Const GIIMEndorsement_SecurityCode As String = "ENDORSEMENT_SECURITYCODE"
	Public Const GIIMEndorsement_TotalCode As String = "ENDORSEMENT_TOTALCODE"
	Public Const GIIMEndorsement_AgeCode As String = "ENDORSEMENT_AGECODE"
	
	'Vehicle Constants
	Public Const GIIMVehicle As String = "Vehicle"
	Public Const GIIMVehicleAccessoriesValue As String = "Accessories_Value"
	Public Const GIIMVehicleAnnualMileage As String = "Annual_Mileage"
	Public Const GIIMVehicleBodyType As String = "Body_type"
	Public Const GIIMVehicleClassOfUse As String = "Class_Of_Use"
	Public Const GIIMVehicleColour As String = "Colour"
	Public Const GIIMVehicleCC As String = "Cubic_Capacity"
	Public Const GIIMVehicleDateFirstRegd As String = "Date_First_Regd"
	Public Const GIIMVehicleFinish As String = "Finish"
	Public Const GIIMVehicleInterestedParty1Name As String = "Interested_Party_1_Name"
	Public Const GIIMVehicleInterestedParty1Type As String = "Interested_Party_1_Type"
	Public Const GIIMVehicleKeeper As String = "Keeper"
	Public Const GIIMVehicleLeftOrRightHandDrive As String = "Left_or_Right_Hand_Drive"
	Public Const GIIMVehicleRestrictMileage As String = "Limited_Mileage"
	Public Const GIIMVehicleLocationKeptOvernight As String = "Location_Kept_Overnight"
	Public Const GIIMVehicleMake As String = "Manufacturer"
	Public Const GIIMVehicleMileometerReading As String = "Mileometer_Reading"
	Public Const GIIMVehicleMileometerReadingDate As String = "Mileometer_Reading_Date"
	Public Const GIIMVehicleModel As String = "Model"
	Public Const GIIMVehicleModelName As String = "Model_Name"
	Public Const GIIMVehicleModifiedInd As String = "Modified_Ind"
	Public Const GIIMVehicleMOTCertificateDate As String = "MOT_Certificat_Date"
	Public Const GIIMVehicleMOTCertificateNumber As String = "MOT_Certificate_Number"
	Public Const GIIMVehicleMOTMileage As String = "MOT_Mileage"
	Public Const GIIMVehicleNo_Drivers As String = "No_Drivers"
	Public Const GIIMVehicleDriversTotal As String = "No_Drivers"
	Public Const GIIMVehicleNumSeats As String = "No_Of_Seats"
	Public Const GIIMVehicleApprovedSecurityDevicesTotal As String = "Num_Of_Approved_Security_Devices"
	Public Const GIIMVehicleOwnership As String = "Ownership"
	Public Const GIIMVehiclePricePaid As String = "Price_Paid"
	Public Const GIIMVehiclePostCode As String = "Post_Code"
	Public Const GIIMVehiclePurchaseDate As String = "Purchase_Date"
	Public Const GIIMVehicleRatingPostCode As String = "Rated_Postcode_Area"
	Public Const GIIMVehicleRegNo As String = "Reg_No"
	Public Const GIIMVehicleSecurityDeviceFittedInd As String = "Security_Device_Fitted_Ind"
	Public Const GIIMVehicleTransmission As String = "Transmission_Type"
	Public Const GIIMVehicleFuel As String = "Type_Of_Fuel"
	Public Const GIIMVehicleUsedAddress1 As String = "Used_Address_Line_1"
	Public Const GIIMVehicleUsedAddress2 As String = "Used_Address_Line_2"
	Public Const GIIMVehicleUsedAddress3 As String = "Used_Address_Line_3"
	Public Const GIIMVehicleUsedAddress4 As String = "Used_Address_Line_4"
	Public Const GIIMVehicleAccidentsInd As String = "Vehicle_Accidents_Ind"
	Public Const GIIMVehicleValue As String = "Value"
	Public Const GIIMVehicleInsurerDerivedClassOfUse As String = "Insurer_Derived_Class_Of_Use"
	Public Const GIIMVehicleRatedCategory As String = "Rated_Category"
	
	'RDT021000
	Public Const GIIMVehicleDateManufactured As String = "Date_Manufactured"
	Public Const GIIMVehicleQPlateInd As String = "Q_Plate_Ind"
	'JSB 11/05/2001
	Public Const GIIMVehicleRegisteredAbroadYN As String = "Veh_Org_Reg_Abroad"
	'JSB 22/10/2002
	Public Const GIIMVehiclePrevDecTotalLossInd As String = "Previously_totalled"
	'SPW 230205
	Public Const GIIMBrokerDetails As String = "Broker_Details"
	Public Const GIIMBrokerDetailsICCSNO As String = "ICCS_NO"
	
	'Edi Party Controls
	Public Const GIIMEdiPartyControls As String = "Edi_Party_Controls"
	Public Const GIIMEdiPartyControlsLastEventTypeReceived As String = "last_event_type_received"
	Public Const GIIMEdiPartyControlsEventTypeSent As String = "event_type_sent"
	Public Const GIIMEdiPartyControlsLastMessageCountReceived As String = "last_message_count_received"
	Public Const GIIMEdiPartyControlsMessageCountSent As String = "message_count_sent"
	
	'Communication Data
	Public Const GIIMQuoteCommunicationData As String = "Quote_Communication_Data"
	Public Const GIIMSavedCommunicationData As String = "Saved_Communication_Data"
	Public Const GIIMCommunicationData As String = "Communication_Data"
	
	Public Const GIIMCommunicationDataDescription As String = "Description"
	Public Const GIIMCommunicationDataTotal As String = "Total"
	Public Const GIIMCommunicationDataCode As String = "Code"
	
	Public Const GIIMControlBlock As String = "Control_Block"
	Public Const GIIMControlBlockLivePolicyLapsedDate As String = "Live_Policy_Lapsed_Date"
End Module