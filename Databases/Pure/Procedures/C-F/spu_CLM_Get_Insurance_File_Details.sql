SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Insurance_File_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Insurance_File_Details          
          
@insurance_file_cnt int          
          
AS          
          
BEGIN          
          
 SELECT i.insurance_folder_cnt,         
  i.insurance_ref,         
  s.last_trans_description,        
  p.product_id,           
  p.code product_code,       
  c.currency_id,       
  c.code currency_code,     
  ifolder.insurance_folder_cnt,     
  ifolder.insurance_holder_cnt,     
  i.source_id,     
  i.insured_cnt,     
  source.description,    
  p.suppress_reserves,    
  p.suppress_payments,    
  p.suppress_recoveries, 
  lead_agent_cnt,
  ifolder.inception_date,
  TT.code [Last Trans Type],
  ISNULL(i.underwriting_year_id,0) 'underwriting_year_id'  
  
 FROM insurance_file i          
        
 JOIN currency c ON       
 c.currency_id = i.currency_id      
      
 JOIN product p ON         
 p.product_id = i.product_id        
        
 JOIN insurance_file_system s ON          
  s.insurance_file_cnt = i.insurance_file_cnt     
    
 JOIN insurance_folder ifolder ON          
  ifolder.insurance_folder_cnt = i.insurance_folder_cnt    
    
 JOIN source ON     
 source.source_id = i.source_id    
 
 LEFT JOIN Transaction_Type TT ON 
	S.last_trans_type_id = TT.transaction_type_id       
 WHERE i.insurance_file_cnt = @insurance_file_cnt          
          
END        
 