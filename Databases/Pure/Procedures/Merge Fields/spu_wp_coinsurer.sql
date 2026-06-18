SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_coinsurer'
go

CREATE PROCEDURE spu_wp_coinsurer @PartyCnt INT,
     @InsuranceFileCnt INT,
     @RiskID INT,
     @ClaimCnt INT,
     @DocumentRef VARCHAR(25),
     @Instance1 INT,
     @Instance2 INT,
     @Instance3 INT
AS

DECLARE      
    @insurer_address1 VARCHAR(60),      
    @insurer_address2 VARCHAR(60),      
    @insurer_address3 VARCHAR(60),      
    @insurer_address4 VARCHAR(60),      
    @insurer_postal_code VARCHAR(20),      
    @insurer_country VARCHAR(255),      
    @address_usage_code VARCHAR(10),      
    @insurer_name VARCHAR(60),    
 @coinsurer_shortname VARCHAR(60),    
    @insurer_branch_address1 VARCHAR(60),      
    @insurer_branch_address2 VARCHAR(60),      
    @insurer_branch_address3 VARCHAR(60),      
    @insurer_branch_address4 VARCHAR(60),      
    @insurer_branch_postal_code VARCHAR(20),      
    @insurer_branch_country VARCHAR(255),      
    @insurer_agency_number VARCHAR(255),      
    @coinsurer_percentage MONEY,      
    @coinsurer_value MONEY,      
    @coinsurer_suminsured MONEY,      
    @coinsurer_commission_rate MONEY,      
    @coinsurer_commission_amount MONEY,      
    @coinsurer_ipt_amount MONEY,      
 @coinsurer_popf_amount MONEY,      
 @coinsurer_vat_amount MONEY,      
    @coinsurer_value_incl MONEY,      
    @coinsurer_fee MONEY,      
    @risk_transfer_agreement VARCHAR(3),      
 @bureau_name VARCHAR(60),      
 @bureau_party_cnt INT,      
 @written_line_percentage NUMERIC(19,4),      
 @line_stands VARCHAR(3),      
 @signed_line_percentage NUMERIC(19,4),      
 @isleadunderwriter VARCHAR(3),      
 @policy_number VARCHAR(30),      
 @commission_charge NUMERIC(19,4),  
 @Coins_Placement  VARCHAR(30),  
 @Premium_GRS NUMERIC(19,4),  
 @BusinessType  VARCHAR(30),
 @XRate  NUMERIC(19,4)
      
 DECLARE @AgentUnderwriter varchar(1)      
   
 SELECT @Coins_Placement = ISNULL(ifi.Coins_Placement,'') , @BusinessType = BT.code , @XRate = isnull(currency_base_xrate,1)  FROM Insurance_File as ifi  
 LEFT JOIN Business_type as BT on BT.business_Type_id = ifi.business_type_id  
  where ifi.Insurance_File_cnt = @InsuranceFileCnt     
  
 SELECT @AgentUnderwriter = value      
        FROM Hidden_Options      
        WHERE branch_id = 1 AND option_number = 1      
    IF ISNULL(@AgentUnderwriter, '') = 'U' BEGIN      
        SELECT @AgentUnderwriter = 'U'      
    END      
IF @AgentUnderwriter = 'U'      
BEGIN      
 DECLARE coinsurer_cursor SCROLL CURSOR FOR      
     SELECT  party_cnt      
     FROM    coi_value      
     WHERE   insurance_file_cnt = @InsuranceFileCnt      
      
 OPEN coinsurer_cursor      
      
 --DC290605 PN22029 use instance1 not instance2      
 FETCH ABSOLUTE @Instance1 FROM coinsurer_cursor INTO @PartyCnt      
      
 CLOSE coinsurer_cursor      
      
 DEALLOCATE coinsurer_cursor      
END      
ELSE      
BEGIN      
 DECLARE coinsurer_cursor SCROLL CURSOR FOR      
     SELECT  party_cnt,      
    bureau_party_cnt      
     FROM    policy_coinsurers      
     WHERE   insurance_file_cnt = @InsuranceFileCnt      
 OPEN coinsurer_cursor      
 --DC290605 PN22029 use instance1 not instance2      
 FETCH ABSOLUTE @Instance1 FROM coinsurer_cursor INTO @PartyCnt,@bureau_party_cnt      
      
 CLOSE coinsurer_cursor      
      
 DEALLOCATE coinsurer_cursor      
END      
      
SELECT @insurer_agency_number = (select agency_number from party_insurer where party_cnt=@PartyCnt)     
   
SELECT @address_usage_code = '3131 XCO'      
      
EXEC spu_wp_get_address      
    @PartyCnt,      
    @InsuranceFileCnt,      
    @ClaimCnt,      
    @address_usage_code,      
    @insurer_address1 OUTPUT,      
    @insurer_address2 OUTPUT,      
    @insurer_address3 OUTPUT,      
    @insurer_address4 OUTPUT,      
    @insurer_postal_code OUTPUT,      
    @insurer_country OUTPUT      
      
SELECT @insurer_name =         
        p.name , @coinsurer_shortname = p.shortname    
        FROM party p      
        WHERE p.party_cnt = @PartyCnt      
        
      
SELECT @bureau_name =      
    (      
        SELECT p.name      
        FROM party p      
  WHERE p.party_cnt = @bureau_party_cnt      
    )      
      
SELECT @address_usage_code = '3131 XBA'      
      
EXEC spu_wp_get_address      
    @PartyCnt,      
    @InsuranceFileCnt,      
   @ClaimCnt,      
    @address_usage_code,      
    @insurer_branch_address1 OUTPUT,      
    @insurer_branch_address2 OUTPUT,      
    @insurer_branch_address3 OUTPUT,      
    @insurer_branch_address4 OUTPUT,      
    @insurer_branch_postal_code OUTPUT,      
    @insurer_branch_country OUTPUT      
      
IF @AgentUnderwriter <> 'U'      
 SELECT      
     @coinsurer_percentage = ROUND(pc.coinsurer_percentage,2),      
     @coinsurer_value = pc.coinsurer_value,      
     @coinsurer_commission_rate = ROUND(pc.coinsurer_commission_rate,2),      
     @coinsurer_commission_amount = pc.coinsurer_commission_amount,      
     @coinsurer_ipt_amount = pc.coinsurer_ipt_amount,      
     @coinsurer_value_incl = pc.coinsurer_value + pc.coinsurer_ipt_amount,      
     @coinsurer_fee = ISNULL(pf.fee_amount,0),      
     @risk_transfer_agreement = (SELECT      
     CASE pc.risk_transfer_agreement      
     WHEN 1 THEN 'Yes'      
     ELSE 'No'      
     END      
     ),      
  @written_line_percentage = ROUND(pc.written_line_percentage,2),      
  @line_stands = (SELECT      
     CASE pc.linestands      
     WHEN 1 THEN 'Yes'      
     ELSE 'No'      
     END      
     ),      
  @signed_line_percentage = pc.signed_line_percentage,      
  @isleadunderwriter= (SELECT      
     CASE pc.isleadunderwriter      
     WHEN 1 THEN 'Yes'      
     ELSE 'No'      
     END      
     ),      
  @policy_number = pc.coinsurer_policy_number,      
  @commission_charge = (SELECT SUM(commission_charge) FROM policy_coinsurers_section pcs WHERE pcs.party_cnt=@PartyCnt and pcs.insurance_file_cnt = @InsuranceFileCnt)      
 FROM policy_coinsurers pc      
 LEFT JOIN policy_fee pf      
     ON pf.party_cnt = pc.party_cnt      
 WHERE pc.insurance_file_cnt = @InsuranceFileCnt      
 AND pc.party_cnt = @PartyCnt      
ELSE      
 SELECT      
     @coinsurer_percentage = ROUND(pc.share_percent,2),      
    -- @coinsurer_value = pc.share_premium,      
     @coinsurer_commission_rate = ROUND(pc.commission_percent,2),      
    -- @coinsurer_commission_amount = pc.commission_value,      
    -- @coinsurer_ipt_amount = pc.premium_tax_recovery_value,      
    -- @coinsurer_value_incl = pc.share_premium + pc.premium_tax_recovery_value,      
     @coinsurer_fee = ISNULL(pf.fee_amount,0),      
     @risk_transfer_agreement = NULL      
      
 FROM  coi_value pc      
 LEFT JOIN policy_fee pf      
     ON pf.party_cnt = pc.party_cnt      
 WHERE pc.insurance_file_cnt = @InsuranceFileCnt      
 AND pc.party_cnt = @PartyCnt      
    
  
  SET @Premium_GRS = 0  
  Declare @TmpCommission NUMERIC(20,4)  
  
 IF  @BusinessType = 'COIN FOLL' AND @Coins_Placement = 'GROSS' BEGIN  
  SELECT @Premium_GRS = isnull(this_premium_original ,0) FROM Stats_Folder as SF left join Stats_Detail as SD on SD.stats_folder_cnt = Sf.stats_folder_cnt      
  Where SF.insurance_file_cnt = @InsuranceFileCnt      
  and SD.stats_detail_type = 'GRS'      
  and isnull(this_premium_original,0) <> 0   
  
 SELECT @TmpCommission = isnull(sum(SD.lead_commission_value_home/@XRate),0)  
   FROM Stats_Folder as SF left join Stats_Detail as SD on SD.stats_folder_cnt = Sf.stats_folder_cnt      
   Where SF.insurance_file_cnt = @InsuranceFileCnt      
 and SD.stats_detail_type = 'COI'      
 and SD.ri_party_cnt = @PartyCnt   and isnull(this_premium_original,0) <> 0   
 group by SD.stats_folder_cnt, SD.ri_party_cnt, SF.stats_folder_cnt , SD.stats_detail_type     
  
   SET @Premium_GRS = @Premium_GRS-@TmpCommission  
 END   
  
  
 SELECT      
     @coinsurer_suminsured= isnull(sum(SD.sum_insured_home /@XRate),0) ,      
     @coinsurer_value = sum(SD.this_premium_original) + @Premium_GRS ,    --- isnull(sum(SD.lead_commission_value_home),0)  
     @coinsurer_commission_amount = sum(SD.lead_commission_value_home /@XRate),      
     @coinsurer_ipt_amount =  (SELECT isnull(SUM(SDTax.tax_value *-1),0) FROM Stats_Detail AS SDTax where SDTax.stats_folder_cnt = SD.stats_folder_cnt      
                              and stats_detail_type = 'TAC' AND sdTax.ri_party_cnt = SD.ri_party_Cnt),      
     @coinsurer_popf_amount = (SELECT isnull(SUM(SDTax.tax_value *-1),0) FROM Stats_Detail AS SDTax where SDTax.stats_folder_cnt = SD.stats_folder_cnt      
                              and sdTax.stats_detail_type = 'TAC' and sdTax.tax_type_code = 'POPF' AND sdTax.ri_party_cnt = SD.ri_party_Cnt),      
     @coinsurer_vat_amount = (SELECT isnull(SUM(SDTax.tax_value *-1),0) FROM Stats_Detail AS SDTax where SDTax.stats_folder_cnt = SD.stats_folder_cnt      
                             and sdTax.stats_detail_type = 'TAC' and sdTax.tax_type_code = 'VAT' AND sdTax.ri_party_cnt = SD.ri_party_Cnt),      
     @coinsurer_value_incl = sum(SD.this_premium_original) + @Premium_GRS + (SELECT isnull(SUM(SDTax.tax_value *-1),0) FROM Stats_Detail AS SDTax      
                             where SDTax.stats_folder_cnt = SF.stats_folder_cnt and stats_detail_type = 'TAC'      
  AND sdTax.ri_party_cnt = SD.ri_party_Cnt) - sum(SD.lead_commission_value_home /@XRate)      
     FROM Stats_Folder as SF left join Stats_Detail as SD on SD.stats_folder_cnt = Sf.stats_folder_cnt      
 Where SF.insurance_file_cnt = @InsuranceFileCnt      
 and SD.stats_detail_type = 'COI'      
 and SD.ri_party_cnt = @PartyCnt   and isnull(this_premium_original,0) <> 0   
 group by SD.stats_folder_cnt, SD.ri_party_cnt, SF.stats_folder_cnt , SD.stats_detail_type      
      
SELECT      
    'insurer_address1' = @insurer_address1,      
    'insurer_address2' = @insurer_address2,      
    'insurer_address3' = @insurer_address3,      
    'insurer_address4' = @insurer_address4,      
    'insurer_postal_code' = @insurer_postal_code,      
    'insurer_country' = @insurer_country,      
    'insurer_name' = @insurer_name,      
 'coinsurer_shortname' = @coinsurer_shortname,  
    'insurer_branch_address1' = @insurer_branch_address1,      
    'insurer_branch_address2' = @insurer_branch_address2,      
    'insurer_branch_address3' = @insurer_branch_address3,      
    'insurer_branch_address4' = @insurer_branch_address4,      
    'insurer_branch_postal_code' = @insurer_branch_postal_code,      
    'insurer_branch_country' = @insurer_branch_country,      
    'Insurer_agency_number' = @insurer_agency_number,      
    'coinsurer_percentage' = ISNULL(@coinsurer_percentage,0),      
    'coinsurer_value' = ISNULL(@coinsurer_value,0),      
 'coinsurer_suminsured' = ISNULL(@coinsurer_suminsured,0),      
     'coinsurer_popf_amount' = ISNULL(@coinsurer_popf_amount,0),      
  'coinsurer_vat_amount' = ISNULL(@coinsurer_vat_amount,0),      
    'coinsurer_commission_rate' = ISNULL(@coinsurer_commission_rate,0),      
    'coinsurer_commission_amount' = ISNULL(@coinsurer_commission_amount,0),      
    'coinsurer_ipt_amount' = ISNULL(@coinsurer_ipt_amount,0),      
    'coinsurer_value_incl' = ISNULL(@coinsurer_value_incl,0),      
    'coinsurer_fee' =ISNULL( @coinsurer_fee,0),      
    'risk_transfer_agreement' = @risk_transfer_agreement,      
    'bureau_name'= @bureau_name,      
 'written_line_percentage'= @written_line_percentage,      
 'line_stand' = @line_stands,      
 'signed_line_percentage' = @signed_line_percentage,      
 'is_lead_underwriter' = @isleadunderwriter,      
 'policy_number' = @policy_number,      
 'commission_charge' = ISNULL(@commission_charge,0) 