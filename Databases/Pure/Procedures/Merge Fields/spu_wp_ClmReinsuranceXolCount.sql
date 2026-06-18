SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ClmReinsuranceXolCount'
GO


CREATE PROCEDURE spu_wp_ClmReinsuranceXolCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT 	count(*) as how_many
    FROM 	claim_ri_arrangement ra
    JOIN    claim_ri_arrangement_line ral 
            ON ral.ri_arrangement_id = ra.ri_arrangement_id 
            AND ral.claim_id = ra.claim_id
    WHERE 	ra.risk_cnt = @RiskId
    AND	    ra.claim_id = @ClaimCnt
    AND     ral.type = 'X'


GO


