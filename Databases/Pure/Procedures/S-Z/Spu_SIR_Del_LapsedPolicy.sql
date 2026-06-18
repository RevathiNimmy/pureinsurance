
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_SIR_Del_LapsedPolicy'
GO

CREATE PROCEDURE Spu_SIR_Del_LapsedPolicy    
 @Insurance_ref as varchar(100),  
 @Insurance_folder_cnt INT    
AS    
BEGIN   
    DECLARE @insurance_file_cnt AS INT   
    DECLARE @lapsed_reason_id AS INT   
  
    IF @Insurance_ref = ''   
        OR @Insurance_ref IS NULL   
      BEGIN   
          SELECT @Insurance_ref = code   
          FROM   insurance_folder   
          WHERE  insurance_folder_cnt = @Insurance_folder_cnt   
      END   
  
    SELECT @insurance_file_cnt = insurance_file_cnt,   
           @lapsed_reason_id = lapsed_reason_id   
    FROM   insurance_file   
    WHERE  insurance_ref = @Insurance_ref   
           AND lapsed_reason_id IS NOT NULL   
  
    DELETE FROM insurance_file_system   
    WHERE  insurance_file_cnt = @insurance_file_cnt   
  
    DELETE FROM insurance_file_risk_link   
    WHERE  insurance_file_cnt = @insurance_file_cnt   
  
    DELETE FROM event_log   
    WHERE  insurance_file_cnt = @insurance_file_cnt   
  
    DELETE FROM agent_commission   
    WHERE  insurance_file_cnt = @insurance_file_cnt   
  
    DELETE FROM policy_standard_wording   
    WHERE  insurance_file_cnt = @insurance_file_cnt   
  
    DELETE FROM insurance_file   
    WHERE  insurance_file_cnt = @insurance_file_cnt   
END    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
