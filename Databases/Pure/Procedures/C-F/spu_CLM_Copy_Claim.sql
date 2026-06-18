SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Copy_Claim'
GO

CREATE PROCEDURE spu_CLM_Copy_Claim    
 @claim_id int,    
 @transaction_type_code varchar(20),    
 @created_by_id int,    
 @copy_claim_id int OUTPUT,    
 @status int OUTPUT,    
 @Version_id int = NULL OUTPUT ,
 @isBaseInfoOnly int = NULL OUTPUT    
AS    
    
/****************************************************************************************************/    
/* sp_copy_claim copies all the claim details to the next version of the claim  */    
/*                                                                                                      */    
/* 1 parameter is passed in - @claim_id  ( original )                 */    
/* 1 parameter is returned - @copy_claim_id   ( copy )             */    
/*                                                                                                      */    
/* A failure in this procedure will be passed back to the calling procedure.        */    
/****************************************************************************************************/    
/* Revision Description of Modification     Date        Who         */    
/* --------         ---------------------------         ----        ---     */    
/* 1.0      Original                    26/04/2001  RWH */    
/***************************************************************************************************/    
DECLARE @Return_Status int    
DECLARE @transaction_type_id int    
DECLARE @claim_ri_arrangement_id int    
IF @claim_id <> 0    
BEGIN    
 ----------------------------    
 -- get the version of claim we are copying    
 -----------------------------    
 EXEC spu_CLM_Get_Claim_Version    
   @claim_id = @claim_id,    
   @version_id = @version_id OUTPUT    
   Select @isBaseInfoOnly = Info_Only FROM CLAIM WHERE Claim_id= @claim_id
    
    
 SET @version_id = @version_id + 1    
    
 SELECT @transaction_type_id  = transaction_type_id    
 FROM transaction_type    
 WHERE code = @transaction_type_code    
 -----------------------------    
 -- Copy Claim table.    
 -----------------------------    
 INSERT INTO Claim    
 (    
  Policy_id,    
  Policy_Number,    
  Claim_Number,    
  Description,    
  Claim_Status_id,    
  Progress_Status_id,    
  Primary_Cause_id,    
  Secondary_Cause_id,    
  Catastrophe_code_id,    
  Coinsurance_treatment_id,    
  Loss_from_date,    
  Loss_to_date,    
  Reported_date,    
  Reported_to_date,    
  Last_modified_date,    
  Handler_id,    
  Currency_id,    
  Info_only,    
  Likely_claim,    
  Location,    
  Town,    
  Risk_type_id,    
  Client_name,    
  Client_address,    
  Client_tel_no,    
  Client_fax_no,    
  Client_mobile_no,    
  Client_email,    
  Client_claim_number,    
  Insurer_name,    
  insurer_address,    
  insurer_tel_no,    
  insurer_fax_no,    
  insurer_email,    
  insurer_claim_number,    
  Insurer_Contact,    
  VAT_registered,    
  VAT_reg_no,    
  Comments,    
  Claims_status_date,    
  Client_short_name,    
  Insurer_short_name,    
  Client_tel_no_off,    
  user_defined_field_A,    
  user_defined_field_B,    
  user_defined_field_C,    
  user_defined_field_D,    
  user_defined_field_E,    
  Client_id,    
  underwriting_year_id,    
  exchange_rate_override_reason_id,    
  currency_base_xrate,    
  currency_base_date,    
  account_base_xrate,    
  account_base_date,    
  system_base_xrate,    
  system_base_date,    
  suppress_reserves,    
  suppress_payments,    
  suppress_recoveries,    
  base_claim_id,    
  version_id,    
  is_dirty,    
  claim_folder_id,    
  transaction_type_id,    
  created_by_id,    
  create_date, gis_Screen_id,
  base_case_id,
  other_party_id    
  )    
 SELECT    
  Policy_id,    
  Policy_Number,    
  Claim_Number,    
  Description,    
  Claim_Status_id,    
  Progress_Status_id,    
  Primary_Cause_id,    
  Secondary_Cause_id,    
  Catastrophe_code_id,    
  Coinsurance_treatment_id,    
  Loss_from_date,    
  Loss_to_date,    
  Reported_date,    
  Reported_to_date,    
  getDate(),   
  Handler_id,    
  Currency_id,    
  Info_only,    
  Likely_claim,    
  Location,    
  Town,    
  Risk_type_id,    
  Client_name,    
  Client_address,    
  Client_tel_no,    
  Client_fax_no,    
  Client_mobile_no,    
  Client_email,    
  Client_claim_number,    
  Insurer_name,    
  insurer_address,    
  insurer_tel_no,    
  insurer_fax_no,    
  insurer_email,    
  insurer_claim_number,    
  Insurer_Contact,    
  VAT_registered,    
  VAT_reg_no,    
  Comments,    
  Claims_status_date,    
  Client_short_name,    
  Insurer_short_name,    
  Client_tel_no_off,    
  user_defined_field_A,    
  user_defined_field_B,    
  user_defined_field_C,    
  user_defined_field_D,    
  user_defined_field_E,    
  Client_id,    
  underwriting_year_id,    
  exchange_rate_override_reason_id,    
  currency_base_xrate,    
  currency_base_date,    
  account_base_xrate,    
  account_base_date,    
  system_base_xrate,    
  system_base_date,    
  suppress_reserves,    
  suppress_payments,    
  suppress_recoveries,    
  base_claim_id,    
  @version_id,    
  1 is_dirty,    
  claim_folder_id,    
  @transaction_type_id,    
  @created_by_id,    
  GetDate(), gis_screen_id,
  base_case_id,
  other_party_id    
    
 FROM Claim WITH (NOLOCK)    
 WHERE Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -- Get generated id.    
 SELECT @copy_claim_id = @@IDENTITY    
    
 ---------------------------------------    
 -- Copy Claim_Party table - coinsurance    
 ---------------------------------------    
 INSERT INTO Claim_Party (    
  Party_id,    
  Claim_id,    
  Share,    
  Share_Value,    
  insurer_type,    
  base_claim_party_id,    
  version_id)    
    
 SELECT    
  Party_id,    
  @copy_claim_id,    
  Share,    
  Share_Value,    
  insurer_type,    
  base_claim_party_id,    
  @version_id    
    
 FROM    Claim_Party WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -----------------------------------------------------    
 -- Copy Claim_ri_arrangement table - reinsurance    
 -----------------------------------------------------    
    
 INSERT INTO Claim_ri_arrangement (    
  claim_id,    
  ri_arrangement_id,    
  risk_cnt,    
  ri_band_id,    
  ri_model_id,    
  claim_allocation_type,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  this_reserve,    
  this_payment,    
  this_salvage,    
  this_recovery,    
  is_modified,    
  base_claim_ri_arrangement_id,    
  version_id,    
  original_ri_arrangement_id,    
  ri_arrangement_version,
  Cloned,
  xol_ri_model_id,
  incurred_to_date ,
  reserve_to_date ,
  payment_to_date ,
  salvage_to_date,
  recovery_to_date ,
  extended_limit_amount )  
 SELECT    
  @copy_claim_id,    
  ri_arrangement_id,    
  risk_cnt,    
  ri_band_id,    
  ri_model_id,    
  claim_allocation_type,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  0, 0, 0, 0, -- Only zero the new 'this' amounts    
  is_modified,    
  base_claim_ri_arrangement_id,    
  @version_id,    
  original_ri_arrangement_id,    
  ri_arrangement_version,
  Cloned,
  xol_ri_model_id,
  incurred_to_date ,
  reserve_to_date ,
  payment_to_date ,
  salvage_to_date,
  recovery_to_date ,
  extended_limit_amount  
 FROM    Claim_ri_arrangement WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 Update Claim_ri_arrangement    
 SET ri_arrangement_id = claim_ri_arrangement_id    
 where claim_id = @copy_claim_id    
    
Select @claim_ri_arrangement_id=claim_ri_arrangement_id from Claim_ri_arrangement WITH (NOLOCK) where claim_id=@copy_claim_id  
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -----------------------------------------------------    
 -- Copy Claim_xol_arrangement table - reinsurance    
 -----------------------------------------------------    
 INSERT INTO claim_xol_arrangement (    
  xol_arrangement_id,    
  claim_id,    
  ri_arrangement_id,    
  catastrophe_code_id,    
  layer,    
  ri_model_id,    
  trigger_limit,    
  base_claim_xol_arrangement_id,    
  version_id)    
 SELECT    
  xol_arrangement_id,    
  @copy_claim_id,    
  copy_claim_ri_arrangement.ri_arrangement_id,    
  claim_xol_arrangement.catastrophe_code_id,    
  claim_xol_arrangement.layer,    
  claim_xol_arrangement.ri_model_id,    
  claim_xol_arrangement.trigger_limit,    
  claim_xol_arrangement.base_claim_xol_arrangement_id,    
  @version_id    
 FROM    claim_xol_arrangement WITH (NOLOCK)    
    
 LEFT JOIN claim_ri_arrangement WITH (NOLOCK)  ON    
  claim_xol_arrangement.ri_arrangement_id = claim_ri_arrangement.ri_arrangement_id    
    
  LEFT JOIN (SELECT ri_arrangement_id, version_id, base_claim_ri_arrangement_id    
      FROM claim_ri_arrangement WITH (NOLOCK)    
      WHERE version_id = @version_id    
      AND claim_id = @copy_claim_id)  copy_claim_ri_arrangement ON    
   copy_claim_ri_arrangement.base_claim_ri_arrangement_id = claim_ri_arrangement.base_claim_ri_arrangement_id    
    
 WHERE   claim_xol_arrangement.claim_id = @claim_id    
    
 UPDATE claim_xol_arrangement    
 SET xol_arrangement_id = claim_xol_arrangement_id    
 WHERE claim_id = @copy_claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -----------------------------------------------------    
 -- Copy Claim_ri_arrangement_line table - reinsurance    
 -----------------------------------------------------    
    
 INSERT INTO Claim_ri_arrangement_line (    
  claim_id,    
  ri_arrangement_line_id,    
  ri_arrangement_id,    
  type,    
  treaty_id,    
  party_cnt,    
  xol_arrangement_id,    
  default_share_percent,    
  this_share_percent,    
  agreement_code,    
  priority,    
  number_of_lines,    
  line_limit,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  this_reserve,    
  this_payment,    
  this_salvage,    
  this_recovery,    
  base_claim_ri_arrangement_line_id,    
  version_id,    
  original_ri_arrangement_line_id,    
  lower_limit,    
  Retained,    
  participation_percent,    
  Grouping,  
  is_obligatory ,  
  ri_model_line_id,
  reserve_to_date ,
  payment_to_date ,
  salvage_to_date ,
  recovery_to_date ,
  claim_incurred_to_date ,
  is_pt_archive)  
 SELECT  @copy_claim_id,    
  Claim_ri_arrangement_line.ri_arrangement_line_id,    
  copy_claim_ri_arrangement.ri_arrangement_id,    
  Claim_ri_arrangement_line.type,    
  Claim_ri_arrangement_line.treaty_id,    
  Claim_ri_arrangement_line.party_cnt,    
  copy_claim_xol_arrangement.xol_arrangement_id,    
  Claim_ri_arrangement_line.default_share_percent,    
  Claim_ri_arrangement_line.this_share_percent,    
  Claim_ri_arrangement_line.agreement_code,    
  Claim_ri_arrangement_line.priority,    
  Claim_ri_arrangement_line.number_of_lines,    
  Claim_ri_arrangement_line.line_limit,    
  Claim_ri_arrangement_line.sum_insured,    
  Claim_ri_arrangement_line.reserve,    
  Claim_ri_arrangement_line.payment,    
  Claim_ri_arrangement_line.salvage,    
  Claim_ri_arrangement_line.recovery,    
  0, 0, 0, 0, -- Only zero the new 'this' amounts    
  Claim_ri_arrangement_line.base_claim_ri_arrangement_line_id,    
  @version_id,    
  original_ri_arrangement_line_id,    
  Claim_ri_arrangement_line.lower_limit,    
  Claim_ri_arrangement_line.Retained,    
  Claim_ri_arrangement_line.participation_percent,    
  Claim_ri_arrangement_line.Grouping,  
  Claim_ri_arrangement_line.Is_Obligatory ,  
  Claim_ri_arrangement_line.ri_model_line_id ,
  Claim_ri_arrangement_line.reserve_to_date ,
  Claim_ri_arrangement_line.payment_to_date ,
  Claim_ri_arrangement_line.salvage_to_date ,
  Claim_ri_arrangement_line.recovery_to_date ,
  Claim_ri_arrangement_line.claim_incurred_to_date,
  Claim_ri_arrangement_line.is_pt_archive  
 FROM    Claim_ri_arrangement_line WITH (NOLOCK)    
    
 LEFT JOIN claim_ri_arrangement WITH (NOLOCK) ON    
  Claim_ri_arrangement_line.ri_arrangement_id = claim_ri_arrangement.ri_arrangement_id    
  AND Claim_ri_arrangement.Claim_id = @claim_id
  LEFT JOIN (SELECT ri_arrangement_id, version_id, base_claim_ri_arrangement_id    
      FROM claim_ri_arrangement WITH (NOLOCK)    
      WHERE version_id = @version_id    
      AND claim_id = @copy_claim_id)  copy_claim_ri_arrangement ON    
   copy_claim_ri_arrangement.base_claim_ri_arrangement_id = claim_ri_arrangement.base_claim_ri_arrangement_id    
    
 LEFT JOIN claim_xol_arrangement WITH (NOLOCK) ON    
  Claim_ri_arrangement_line.xol_arrangement_id = claim_xol_arrangement.xol_arrangement_id    
    
  LEFT JOIN (SELECT xol_arrangement_id, version_id, base_claim_xol_arrangement_id    
      FROM claim_xol_arrangement WITH (NOLOCK)    
      WHERE version_id = @version_id    
      AND claim_id = @copy_claim_id)  copy_claim_xol_arrangement ON    
   copy_claim_xol_arrangement.base_claim_xol_arrangement_id = claim_xol_arrangement.base_claim_xol_arrangement_id    
WHERE   Claim_ri_arrangement_line.Claim_id = @claim_id    
    
UPDATE claim_ri_arrangement_line    
SET ri_arrangement_line_id = claim_ri_arrangement_line_id    
WHERE claim_id = @copy_claim_id    
    
 exec Spu_CLM_Copy_Broker_Details_to_Claim @claim_ri_arrangement_id  
/*    
  INSERT INTO Claim_ri_arrangement (    
  claim_id,    
  ri_arrangement_id,    
  risk_cnt,    
  ri_band_id,    
  ri_model_id,    
  claim_allocation_type,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  this_reserve,    
  this_payment,    
  this_salvage,    
  this_recovery,    
  is_modified,    
  base_claim_ri_arrangement_id,    
  version_id)    
 SELECT  @copy_claim_id,    
  ri_arrangement_id,    
  risk_cnt,    
  ri_band_id,    
  ri_model_id,    
  claim_allocation_type,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  0, 0, 0, 0, -- Only zero the new 'this' amounts    
  is_modified,    
  base_claim_ri_arrangement_id,    
  @version_id    
 FROM    Claim_ri_arrangement    
 WHERE   Claim_id = @claim_id    

 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -----------------------------------------------------    
 -- Copy Claim_xol_arrangement table - reinsurance    
 -----------------------------------------------------    
 INSERT INTO claim_xol_arrangement (    
  xol_arrangement_id,    
  claim_id,    
  ri_arrangement_id,    
  catastrophe_code_id,    
  layer,    
  ri_model_id,    
  trigger_limit,    
  base_claim_xol_arrangement_id,    
  version_id)    
 SELECT    
  0,    
  @copy_claim_id,    
  ri_arrangement_id,    
  catastrophe_code_id,    
  layer,    
  ri_model_id,    
  trigger_limit,    
  base_claim_xol_arrangement_id,    
  @version_id    
 FROM    claim_xol_arrangement    
 WHERE   claim_id = @claim_id    
    
 UPDATE claim_xol_arrangement    
 SET xol_arrangement_id = claim_xol_arrangement_id    
 WHERE xol_arrangement_id = 0    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -----------------------------------------------------    
 -- Copy Claim_ri_arrangement_line table - reinsurance    
 -----------------------------------------------------    
    
 INSERT INTO Claim_ri_arrangement_line (    
  claim_id,    
  ri_arrangement_line_id,    
  ri_arrangement_id,    
  type,    
  treaty_id,    
  party_cnt,    
  xol_arrangement_id,    
  default_share_percent,    
  this_share_percent,    
  agreement_code,    
  priority,    
  number_of_lines,    
  line_limit,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  this_reserve,    
  this_payment,    
  this_salvage,    
  this_recovery,    
  base_claim_ri_arrangement_line_id,    
  version_id)    
 SELECT  @copy_claim_id,    
  ri_arrangement_line_id,    
  ri_arrangement_id,    
  type,    
  treaty_id,    
  party_cnt,    
  xol_arrangement_id,    
  default_share_percent,    
  this_share_percent,    
  agreement_code,    
  priority,    
  number_of_lines,    
  line_limit,    
  sum_insured,    
  reserve,    
  payment,    
  salvage,    
  recovery,    
  0, 0, 0, 0, -- Only zero the new 'this' amounts    
  base_claim_ri_arrangement_line_id,    
  @version_id    
 FROM    Claim_ri_arrangement_line    
 WHERE   Claim_id = @claim_id    
    
*/    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -------------------------------------------------    
 -- Copy Claim_Party_Link table    
 -------------------------------------------------    
 INSERT INTO Claim_Party_Link(    
  Claim_id,    
  Party_Cnt,    base_claim_party_link_id,    
  version_id)    
    
 SELECT    
  @copy_claim_id,    
  party_cnt,    
  base_claim_party_link_id,    
  @version_id    
 FROM    Claim_Party_Link WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
-------------------------------------    
 -- Copy Claim_Risk table    
 --------------------------------------    
 INSERT INTO Claim_Risk (    
  Risk_type_id,    
  Claim_id,    
  Description,    
  Comments,    
  base_claim_risk_id,    
  version_id)    
    
 SELECT    
  Risk_type_id,    
  @copy_claim_id,    
  Description,    
  Comments,    
  base_claim_risk_id,    
  @version_id    
 FROM    Claim_Risk WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 --------------------------------------------------------------------    
 -- Copy claim_user_defined_risk_data table    
 --------------------------------------------------------------------    
 INSERT INTO claim_user_defined_risk_data (    
  Claim_id,    
  risk_data_defn_id,    
  Value,    
  base_claim_user_defined_risk_data_id,    
  version_id)        
 SELECT  @copy_claim_id,    
  risk_data_defn_id,    
  Value,    
  base_claim_user_defined_risk_data_id,    
  @version_id    
 FROM    claim_user_defined_risk_data WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 ------------------------------------------------------    
 -- Copy claim_expert_service table    
 ------------------------------------------------------    
 INSERT INTO claim_expert_service (    
  Claim_id,    
  Expert_Service_Id,    
  Party_Claim_id,    
  Service_type_id,    
  Service,    
  Description,    
  Reference,    
  Contact,    
  Date_requested,    
  Date_critical,    
  Date_received,    
  base_claim_expert_service_id,    
  version_id)    
 SELECT    
  @copy_claim_id,    
  Expert_Service_Id,    
  Party_Claim_id,    
  Service_type_id,    
  Service,    
  Description,    
  Reference,    
  Contact,    
  Date_requested,    
  Date_critical,    
  Date_received,    
  base_claim_expert_service_id,    
  version_id    
 FROM    claim_expert_service WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 ------------------------------------------------------    
 -- Copy user_defined_peril_data table    
 ------------------------------------------------------    
 INSERT INTO user_defined_peril_data (    
  Claim_id,    
  Peril_data_defn_id,    
  Value,    
  base_user_defined_peril_data_id,    
  version_id)    
    
 SELECT  @copy_claim_id,    
  Peril_data_defn_id,    
  Value,    
  base_user_defined_peril_data_id,    
  @version_id    
 FROM    user_defined_peril_data WITH (NOLOCK)    
 WHERE   Claim_id = @claim_id    
    
 IF @@ERROR <> 0    
  GOTO Error_Routine    
    
 -------------------------------------------------------------------------    
 -- copy claim_peril, peril_party, reserve, recovery, payment and receipt    
 -------------------------------------------------------------------------    
 EXEC spu_CLM_copy_claim_peril    
  @claim_id,    
  @copy_claim_id,    
  @version_id,    
  @status OUTPUT    
    
 IF @Status <> 0    
  GOTO Error_Routine    
    
END    
    
SELECT  @status = 0    
RETURN    
    
Error_Routine:    
    
 --  PRINT "COPY CLAIM TO WORK FAILED"    
 SELECT  @status = -1    
 RETURN    
    
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON    
  


GO
