SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_RiskTaxCount'
GO


CREATE PROCEDURE spu_wp_RiskTaxCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT count(tax_band_id) "how_many"
    FROM Tax_Calculation
    WHERE risk_cnt = @riskid AND transtype = 'TTR'
GO


