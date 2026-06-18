SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Claim_Peril_Get_SumInsured_And_RIBand'
GO

CREATE PROCEDURE spu_CLM_Claim_Peril_Get_SumInsured_And_RIBand

    @RiskID int,
    @PerilTypeId int

AS
    SELECT SUM (p.rating_sum_insured) AS SumInsured, p.RI_Band
    FROM peril p, peril_type pt
    WHERE p.peril_type_id = pt.peril_type_id
    AND p.risk_cnt = @RiskID
    AND p.peril_type_id = @PerilTypeId
    GROUP BY p.peril_type_id, pt.description, p.RI_Band
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

