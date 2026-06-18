SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ReinsuranceTreatyCount'
GO


CREATE PROCEDURE spu_wp_ReinsuranceTreatyCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT 	Count(*) AS how_many
    FROM 	ri_arrangement rra
    JOIN    ri_arrangement_line ral 
            ON ral.ri_arrangement_id = rra.ri_arrangement_id
    JOIN	treaty t 
            ON t.treaty_id = ral.treaty_id
    WHERE 	rra.risk_cnt = @RiskId
    AND     rra.original_flag = 0
    AND     ral.type IN ('R', 'T','TX','TFS')  -- only retained or treaty


go 

