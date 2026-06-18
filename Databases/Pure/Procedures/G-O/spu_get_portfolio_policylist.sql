SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_get_portfolio_policylist
GO

CREATE PROCEDURE spu_get_portfolio_policylist  
AS  
    
	SELECT iptl.insurance_file_cnt , ifi.insurance_ref ,p.shortname ,p.resolved_name  
	FROM insurance_file_pt_log iptl 
		INNER JOIN Insurance_file ifi 
			ON ifi.insurance_file_cnt = iptl.insurance_file_cnt  
		INNER JOIN Party p  
			ON ifi.insured_cnt = p.party_cnt 
	WHERE iptl.status_id = 2  
   
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO