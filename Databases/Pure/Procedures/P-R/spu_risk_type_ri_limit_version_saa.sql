EXECUTE DDLDropProcedure 'spu_risk_type_ri_limit_version_saa'
GO
CREATE PROCEDURE spu_risk_type_ri_limit_version_saa  
    @risk_type_id INT  
AS  
  
SELECT	[risk_type_ri_limit_version_id] ,
		[risk_type_id],     
		[description] ,
		[ri_limit_start_date] ,
		[ri_limit_end_date],
		0   --item status unchanged    
FROM    Risk_Type_RI_Limit_Version 
WHERE   risk_type_id = @risk_type_id  
ORDER BY [ri_limit_start_date]
