SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_policyfeeucount'
GO

CREATE PROCEDURE spu_wp_policyfeeucount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT
	SUM(1) 'how_many'
FROM policy_fee_u pf
JOIN party p
	ON p.party_cnt = pf.party_cnt
WHERE pf.insurance_file_cnt = @InsuranceFileCnt


GO