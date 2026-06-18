SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Copy_Rating_Section_For_Void_Transaction'
GO 


CREATE PROCEDURE spu_Copy_Rating_Section_For_Void_Transaction  
    @OldRiskCnt int,  
    @NewRiskCnt int  
AS  
  
/*******************************************************************************************/  
/*  Copy Rating_Section records for one    risk to another                      */   
/*******************************************************************************************/  
INSERT INTO Rating_Section (risk_cnt,    
    rating_section_id,    
    rating_section_type_id,    
    policy_section_type_id,    
    sequence_number,    
    description,    
    rate_type_id,    
    annual_rate,    
    sum_insured,    
    annual_premium,    
    this_premium,    
    original_flag,    
    currency_id,    
    is_amended,    
    calculated_premium,    
    override_reason,    
    earning_pattern_id,    
    state_id,    
    country_id )    
SELECT  @NewRiskCnt,    
    rating_section_id,    
    rating_section_type_id,    
    policy_section_type_id,    
    sequence_number,    
    description,    
    rate_type_id,    
    annual_rate,    
    sum_insured,    
    annual_premium,    
     this_premium * -1,    
    1,    
    currency_id,    
    is_amended,    
    calculated_premium * -1,    
    override_reason, earning_pattern_id,    
    state_id,    
    country_id    
FROM    Rating_Section    
WHERE   risk_cnt = @OldRiskCnt    
AND original_flag = 0   
  
INSERT INTO Rating_Section (risk_cnt,    
    rating_section_id,    
    rating_section_type_id,    
    policy_section_type_id,    
    sequence_number,    
    description,    
    rate_type_id,    
    annual_rate,    
    sum_insured,    
    annual_premium,    
    this_premium,    
    original_flag,    
    currency_id,    
    is_amended,    
    calculated_premium,    
    override_reason,    
    earning_pattern_id,    
    state_id,    
    country_id )    
SELECT  @NewRiskCnt,    
    rating_section_id,    
    rating_section_type_id,    
    policy_section_type_id,    
    sequence_number,    
    description,    
    rate_type_id,    
    annual_rate,    
    sum_insured,    
    annual_premium,    
 this_premium * (-1),    
    0,    
    currency_id,    
    is_amended,    
    calculated_premium * -1,    
    override_reason, earning_pattern_id,    
    state_id,    
    country_id    
FROM    Rating_Section    
WHERE   risk_cnt = @OldRiskCnt    
AND original_flag = 1 