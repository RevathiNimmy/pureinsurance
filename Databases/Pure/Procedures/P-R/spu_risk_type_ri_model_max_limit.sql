EXECUTE DDLDropProcedure 'spu_risk_type_ri_model_max_limit'
GO

CREATE PROCEDURE spu_risk_type_ri_model_max_limit      
@risk_type_id INT,      
@risk_type_ri_limit_version_id INT      
AS      
BEGIN      
DECLARE @EffectiveDate DATE , @ExpiryDate DATE 
     
SELECT @EffectiveDate = ri_limit_start_date ,      
    @ExpiryDate= ri_limit_end_date      
FROM Risk_Type_RI_Limit_Version      
WHERE risk_type_id = @risk_type_id      
	AND risk_type_ri_limit_version_id =@risk_type_ri_limit_version_id      
    
SELECT ISNULL(max(rml.line_limit),0)      
FROM RI_Model_line rml      
INNER JOIN Treaty tty ON tty.treaty_id = rml.treaty_id 
LEFT OUTER JOIN  Reinsurance_type RT ON RT.reinsurance_type_id =TTY.reinsurance_type_id    
WHERE ri_model_id IN      
    (SELECT ri_model_id  FROM Risk_Type_RI_Model_Usage      
     WHERE risk_type_id =@risk_type_id      
      AND effective_date > = @EffectiveDate      
      AND expiry_date <=@ExpiryDate )      
	AND RT.CODE in ('QUO','001')      
END 
