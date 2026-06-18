SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

--this will return the count of all commission types for the lead agent for this policy

EXECUTE DDLDropProcedure 'spu_wp_GetCommissionBreakdownCount'
GO

CREATE PROCEDURE spu_wp_GetCommissionBreakdownCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
SELECT        COUNT(1)
FROM   agent_commission ac 
JOIN commission_band cb ON ac.commission_band_id = cb.commission_band_id
WHERE insurance_file_cnt = @InsuranceFileCnt
AND ac.is_lead_agent = 1
GO