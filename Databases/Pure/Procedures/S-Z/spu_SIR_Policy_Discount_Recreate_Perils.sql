SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Recreate_Perils'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Recreate_Perils  
  
@insurance_file_cnt int  
  
AS  
  
BEGIN  
  
 DECLARE @rating_section_id int   
 DECLARE @rating_section_type_id int      
 DECLARE @policy_section_type_id int      
 DECLARE @risk_cnt int      
 DECLARE @sum_insured numeric(19,4)      
 DECLARE @annual_rate numeric(19,4)      
 DECLARE @annual_premium numeric(19,4)      
 DECLARE @this_premium numeric(19,4)      
 DECLARE @rate_type_id int      
 DECLARE @insurance_file_no_of_dp smallint      
 DECLARE @original_flag smallint      
 DECLARE @currency_id smallint      
 DECLARE @country_id int      
 DECLARE @state_id int      
 DECLARE @is_amended tinyint      
 DECLARE @calculated_premium money      
 DECLARE @override_reason varchar(255)     
   
 -- create cursor for rating sections  
 DECLARE rating_section_cursor CURSOR FOR    
 SELECT   
  rating_section_id,   
  rating_section_type_id,   
  policy_section_type_id,   
  insurance_file_cnt,   
  rs.risk_cnt,   
  sum_insured,   
  annual_rate,   
  annual_premium,   
  this_premium,   
  rate_type_id,   
  4,   
  original_flag,   
  currency_id,   
  country_id,    
  state_id,   
  is_amended,   
  calculated_premium,   
  override_reason  
   
 FROM rating_section rs  
   
 INNER JOIN risk r ON   
  r.risk_cnt = rs.risk_cnt  
   
 INNER JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES pdvrs ON   
  r.risk_status_id = pdvrs.risk_status_id  
   
 INNER JOIN insurance_file_risk_link ifrl ON   
  ifrl.risk_cnt = r.risk_cnt  
   
 WHERE 
 rs.original_flag = 0  
 AND ifrl.insurance_file_cnt = @insurance_file_cnt  
 AND r.is_risk_selected = 1  
   
 -- open cursor  
 OPEN rating_section_cursor  
   
 -- get first rating section   
 FETCH NEXT FROM rating_section_cursor   
 INTO  @rating_section_id,   
  @rating_section_type_id,   
  @policy_section_type_id,   
  @insurance_file_cnt,   
  @risk_cnt,   
  @sum_insured,   
  @annual_rate,   
  @annual_premium,   
  @this_premium,   
  @rate_type_id,   
  @insurance_file_no_of_dp,   
  @original_flag,   
  @currency_id,   
  @country_id,    
  @state_id,   
  @is_amended,   
  @calculated_premium,   
  @override_reason  
   
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
   
  -- remove existing entries from the table  
  DELETE FROM peril   
  WHERE risk_cnt = @risk_cnt  
  AND rating_section_id = @rating_section_id

  -- for now save original entries for data comparison  
  --UPDATE Peril set risk_cnt = 999  
  --WHERE risk_cnt = @risk_cnt  
  
  -- recreate the perils for the current rating section  
  EXEC spu_sir_peril_allocation    
       @rating_section_type_id,    
       @policy_section_type_id,    
       @insurance_file_cnt,    
       @risk_cnt,    
       @sum_insured,    
       @annual_rate,    
       @annual_premium,    
       @this_premium,    
       @rate_type_id,    
       @insurance_file_no_of_dp,    
       @original_flag,    
       @currency_id,    
       @country_id,    
       @state_id,    
       @is_amended,    
       @calculated_premium,    
       @override_reason,  
       1,   
       @rating_section_id       
   
     -- Get the next rating section from the.  
  FETCH NEXT FROM rating_section_cursor   
  INTO  @rating_section_id,   
   @rating_section_type_id,   
   @policy_section_type_id,   
   @insurance_file_cnt,   
   @risk_cnt,   
   @sum_insured,   
   @annual_rate,   
   @annual_premium,   
   @this_premium,   
   @rate_type_id,   
   @insurance_file_no_of_dp,   
   @original_flag,   
   @currency_id,   
   @country_id,    
   @state_id,   
   @is_amended,   
   @calculated_premium,   
   @override_reason  
   
 END  
   
 -- close and deallocate the cursor  
 CLOSE rating_section_cursor  
 DEALLOCATE rating_section_cursor  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
