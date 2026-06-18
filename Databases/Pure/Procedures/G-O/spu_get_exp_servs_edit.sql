SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_exp_servs_edit'
GO

CREATE PROCEDURE spu_get_exp_servs_edit    
    @Claim_id int    
AS    
    
--*******************************************************************************************    
-- Version      Author  Date        Desc    
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting    
--    
--*******************************************************************************************    
    SELECT 
		Service_type_id, 
		Description, 
		Claim_Expert_Service_id,    
	        Expert_Service_id, 
		ISNULL(Party_Claim_id,0), 
		Service, 
		Reference,    
	        Contact, 
		Date_requested, 
		Date_critical, 
		Date_received    
    FROM Claim_Expert_Service    
    WHERE (Claim_id = @Claim_id)    

  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
