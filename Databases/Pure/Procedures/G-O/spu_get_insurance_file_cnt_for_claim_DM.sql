SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_insurance_file_cnt_for_claim_DM'
GO

CREATE PROCEDURE spu_get_insurance_file_cnt_for_claim_DM  
       @gis_policy_link_id  INT  
AS  

BEGIN 
	SELECT  CLM.Policy_id  
	FROM  gis_policy_link GPL  
		INNER JOIN claim CLM ON  
			GPL.claim_id = CLM.claim_id  
	WHERE  GPL.gis_policy_link_id =  @gis_policy_link_id  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
