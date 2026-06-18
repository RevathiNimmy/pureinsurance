SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_main_share_reinsurance'
GO

CREATE PROCEDURE spu_get_main_share_reinsurance  
    @ClaimID numeric,  
    @PolicyID numeric  
AS  
  
BEGIN

	DECLARE @ShareValue numeric  
	DECLARE @Reserve numeric  
  
	SELECT @ShareValue = SUM(Claim_Party.Share_Value) 
	FROM 	Claim_Party, 
		Party 
	WHERE Claim_Party.Claim_id = @ClaimID 
	AND  Claim_Party.Party_id = Party.Party_id 
	AND  Party.Party_id IN 
		(
			SELECT Party_id 
			FROM Party 
			WHERE insurer_type=0
		)  

	SELECT  @Reserve = sum(Sum_insured)  
	FROM    Peril p,  
    		Insurance_File_Risk_Link ifr  
	WHERE   ifr.Insurance_file_cnt = @policyID  
	AND ifr.status_flag <> 'D'  
	AND ifr.risk_cnt = p.risk_cnt  

	SELECT @Reserve, @ShareValue  

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
