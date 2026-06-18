SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ReinsuranceTreaty'
GO


CREATE PROCEDURE spu_wp_ReinsuranceTreaty
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
                ral.this_share_percent AS Reinsurer_Share,
        		ral.premium_value AS Reinsurer_Share_Value,
        		ral.sum_insured AS Reinsurer_Sum_Insured,
        		ral.commission_percent AS Reinsurer_Commission_Percent,
                ral.commission_value AS Reinsurer_Commission_Value,
                ral.premium_tax AS Reinsurer_Premium_Tax,
                ral.commission_tax AS Reinsurer_Commission_Tax
        FROM 	ri_arrangement rra
        JOIN    ri_arrangement_line ral 
                ON ral.ri_arrangement_id = rra.ri_arrangement_id
        JOIN	treaty t 
                ON t.treaty_id = ral.treaty_id
        WHERE 	rra.risk_cnt = @RiskId
        AND     rra.original_flag = 0
        AND     ral.type IN ('R', 'T', 'TFS', 'TX')  -- only retained or treaty

    OPEN reinsurance_result
    
    FETCH ABSOLUTE @Instance1
    FROM reinsurance_result
    
    CLOSE reinsurance_result
    DEALLOCATE reinsurance_result


GO


