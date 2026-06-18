SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_RiskTax'
GO


CREATE PROCEDURE spu_wp_RiskTax
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT risk_tax_value = SUM(rt.value),
    risk_tax_type = tb.description
    FROM Tax_Calculation rt
    INNER JOIN tax_band tb ON tb.tax_band_id = rt.tax_band_id
    WHERE rt.risk_cnt = @riskid
    AND rt.tax_band_id = @Instance2
	AND rt.transtype = 'TTR'
	GROUP BY tb.description
GO


