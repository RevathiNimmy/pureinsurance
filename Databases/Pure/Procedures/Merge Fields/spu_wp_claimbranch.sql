SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_claimbranch'
GO

CREATE PROCEDURE spu_wp_claimbranch
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
BEGIN

    SELECT 
		cu.description 'base_currency',
		so.code 'branch_code',
		so.description 'branch_desc'
    FROM claim cl
        JOIN insurance_file ins ON ins.insurance_file_cnt = cl.policy_id
        JOIN source so ON so.source_id = ins.source_id
        JOIN currency cu ON cu.currency_id = so.base_currency_id
    WHERE cl.claim_id = @ClaimCnt
END
GO
