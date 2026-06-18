SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ClmReinsuranceTreaty'
GO


CREATE PROCEDURE spu_wp_ClmReinsuranceTreaty
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
    	SELECT 	t.description AS Reinsurer_Treaty,
    			ral.this_share_percent AS Share_Percent,
    			ral.reserve - ral.payment AS Share_Value,
                ral.sum_insured AS Sum_Insured,
                ral.reserve as Reserve,
                ral.payment as Payment
    	FROM 	claim_ri_arrangement ra
    	JOIN    claim_ri_arrangement_line ral 
                ON ral.ri_arrangement_id = ra.ri_arrangement_id 
                AND ral.claim_id = ra.claim_id
    	JOIN	treaty t ON t.treaty_id = ral.treaty_id
    	WHERE 	ra.risk_cnt = @RiskId
    	AND	    ra.claim_id = @ClaimCnt
    	AND     ral.type IN ('R', 'T','TX','TFS')
    
    OPEN reinsurance_result
    
    FETCH ABSOLUTE @Instance1
    FROM reinsurance_result
    
    CLOSE reinsurance_result
    DEALLOCATE reinsurance_result

GO
