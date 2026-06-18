SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_RI_Arrangement_Get_SumInsuredAndPremium_ByRisk'
GO


CREATE PROCEDURE spu_RI_Arrangement_Get_SumInsuredAndPremium_ByRisk
    @risk_cnt int
AS

    SELECT  SUM(ra.sum_insured),
            SUM(ra.premium)
    FROM    ri_arrangement ra
    WHERE   ra.risk_cnt = @risk_cnt
    AND     ra.ri_band_id IS NOT NULL
    GROUP BY ra.risk_cnt 

GO

