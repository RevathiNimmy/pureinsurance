SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Base_Claim_Details'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Base_Claim_Details                  

                  
@base_claim_id integer                  
  
                  
AS                  
                  
SELECT TOP 1                   
  policy_id as insurance_file_cnt,              
  policy_number as insurance_ref,          
  risk_type_id as risk_cnt,     
  claim_number,           
  loss_from_date,               
  loss_to_date,          
  info_only,          
  client_address,         
  insurer_address,        
  claim.currency_id,      
  currency.code as currency_code,          
  client_name,           
  insurer_name,    
  claim_status.code as claim_status_code,         
  secondary_cause_id,    
  catastrophe_code_id,    
  location,     
  town,         
  client_tel_no,    
  client_fax_no,    
  client_mobile_no,    
  client_email,     
  client_claim_number,    
  insurer_tel_no,    
  insurer_fax_no,    
  insurer_email,    
  insurer_claim_number,    
  insurer_contact,    
  vat_reg_no,     
  comments,     
  client_short_name,    
  insurer_short_name,    
  client_tel_no_off,    
  User_Defined_Field_A,    
  User_Defined_Field_B,    
  User_Defined_Field_C,    
  User_Defined_Field_D,    
  User_Defined_Field_E,    
  underwriting_year_id, 
  vat_registered
  
      
FROM claim                   
            
INNER JOIN claim_status ON    
 claim.claim_status_id = claim_status.claim_status_id    
    
INNER JOIN currency ON             
 claim.currency_id = currency.currency_id            
            
WHERE base_claim_id = @base_claim_id
AND claim.is_dirty  <> 1 
ORDER BY version_id DESC         
  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
