SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Perils_For_Reserve
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.2.3)
CREATE  PROCEDURE spu_SAM_Get_Perils_For_Reserve
    @claim_id int,
    @Reserve_type_id int
AS

BEGIN
    SELECT wcp.Description AS Description,
        wcp.claim_Peril_id AS ClaimPerilKey,
        SUM(wr.Initial_reserve) AS InitialReserve,
        SUM(wr.Paid_to_date) AS PaidAmount,
        SUM(wr.Revised_reserve) AS RevisedReserve,
        SUM(wr.Sum_insured) AS SumInsured,
        SUM(wr.Average) AS Average,
        SUM(wr.Initial_reserve)-
        SUM(wr.Paid_to_date)+
        SUM(wr.Revised_reserve) AS CurrentReserve
    FROM claim_Peril wcp 
        INNER JOIN Reserve wr ON
            wcp.claim_Peril_id = wr.claim_Peril_id 
        INNER JOIN Reserve_type rt ON
            wr.Reserve_type_id = rt.Reserve_type_id
    WHERE wcp.Claim_id = @claim_id
        AND wr.Reserve_type_id = @Reserve_type_id
    GROUP BY wcp.claim_Peril_id,
        wcp.Description
END
--End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.2.3)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO
 