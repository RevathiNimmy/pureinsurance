SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ReinsuranceCount'
GO
EXECUTE DDLDropProcedure 'spu_wp_ReinsuranceFacCount'
GO


CREATE PROCEDURE spu_wp_ReinsuranceFacCount
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
    FROM 	ri_arrangement ra
    JOIN    ri_arrangement_line ral 
            ON ral.ri_arrangement_id = ra.ri_arrangement_id
    JOIN	party P 
            ON p.party_cnt = ral.party_cnt
    WHERE 	ra.risk_cnt = @RiskId
    AND     ra.original_flag = 0
    AND     ral.type = 'F'

Go 


