SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Renewal_Policy_Details'
GO
    
CREATE PROCEDURE spu_SIR_Get_Renewal_Policy_Details    
 @insurance_file_cnt INTEGER     
 AS    
 DECLARE @insurance_folder_cnt INTEGER    
 BEGIN    
 SELECT  @insurance_folder_cnt= ifi.insurance_folder_cnt   
 FROM insurance_file ifi   
 WHERE ifi.insurance_file_cnt = @insurance_file_cnt   
    
SELECT * FROM insurance_file ifi     
 INNER JOIN renewal_status rs   
 ON rs.insurance_file_cnt = ifi.insurance_file_cnt     
 AND rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt    
 WHERE ifi.insurance_file_cnt = @insurance_file_cnt    
 AND policy_version = 1  
 AND alternate_reference IS NOT NULL   
 AND EXISTS (SELECT NULL FROM insurance_file     
 WHERE insurance_folder_cnt = @insurance_folder_cnt    
 GROUP BY insurance_folder_cnt HAVING COUNT(*) = 1)    
 END    
                                                                                  
                                                                                  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
