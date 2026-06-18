SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ClmReinsurance'
GO
EXECUTE DDLDropProcedure 'spu_wp_ClmReinsuranceFac'
GO


CREATE PROCEDURE spu_wp_ClmReinsuranceFac
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    DECLARE reinsurance_result SCROLL CURSOR FOR
    	SELECT 	p.resolved_name AS Reinsurer_Name,
    			ral.this_share_percent AS Share_Percent,
    			ral.reserve - ral.payment AS Share_Value,
                ral.sum_insured AS Sum_Insured,
                ral.reserve as Reserve,
                ral.payment as Payment
    	FROM 	claim_ri_arrangement ra
    	JOIN    claim_ri_arrangement_line ral 
                ON ral.ri_arrangement_id = ra.ri_arrangement_id 
                AND ral.claim_id = ra.claim_id
    	JOIN	party P ON p.party_cnt = ral.party_cnt
    	WHERE 	ra.risk_cnt = @RiskId
    	AND	    ra.claim_id = @ClaimCnt
    	AND     ral.type = 'F'
    
    OPEN reinsurance_result
    
    FETCH ABSOLUTE @Instance1
    FROM reinsurance_result
    
    CLOSE reinsurance_result
    DEALLOCATE reinsurance_result

GO
