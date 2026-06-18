SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_RiskTax_get_keys'
GO


CREATE PROCEDURE spu_wp_RiskTax_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT DISTINCT tax_band_id
    FROM Tax_Calculation
    WHERE risk_cnt = @riskid AND transtype = 'TTR'
GO


