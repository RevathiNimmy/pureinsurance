SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
SET NOCOUNT ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_CoInsurer_Split'
GO

CREATE PROCEDURE spu_CLM_Get_CoInsurer_Split
	@InsuranceFileCnt int = NULL,   
	@ClaimId int = NULL
     
AS    
    
BEGIN
	
	IF ISNULL(@ClaimId, 0) = 0 
	BEGIN
		IF EXISTS ( SELECT * FROM policy_coinsurers WHERE insurance_file_cnt = @InsuranceFileCnt )
		BEGIN
			SELECT 	p.resolved_name,
			c.coinsurer_percentage,
			c.party_cnt,
			p.shortname
 		    FROM policy_coinsurers c
		    JOIN party p ON p.party_cnt = c.party_cnt
		    WHERE c.insurance_file_cnt=@InsuranceFileCnt
		END
		ELSE
		BEGIN
			SELECT p.resolved_name,
			100,
			i.lead_insurer_cnt,
			p.shortname
			FROM insurance_file i
			JOIN party p ON p.party_cnt = i.lead_insurer_cnt
			WHERE i.insurance_file_cnt=@InsuranceFileCnt
		END
	END
	ELSE
	BEGIN
		IF EXISTS ( SELECT * FROM claim_coinsurers WHERE claim_id = @ClaimId )
		BEGIN
			SELECT 	p.resolved_name,
			c.coinsurer_percentage,
			c.party_cnt,
			p.shortname
 		      	FROM claim_coinsurers c
		      	JOIN party p ON p.party_cnt = c.party_cnt
		        WHERE c.claim_id=@ClaimId
		END
		ELSE
		BEGIN
			SELECT p.resolved_name,
			100,
			i.lead_insurer_cnt,
			p.shortname
			FROM insurance_file i
			JOIN party p ON p.party_cnt = i.lead_insurer_cnt
			WHERE i.insurance_file_cnt=@InsuranceFileCnt
		END
	END
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET NOCOUNT OFF