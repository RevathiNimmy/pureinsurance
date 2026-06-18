SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Peril_Totals
GO

CREATE  PROCEDURE spu_SAM_Get_Peril_Totals
    @claim_id int
AS

BEGIN
    SELECT
        wcp.claim_peril_id AS ClaimPerilKey,
        wcp.Description AS Description,
        SUM(wr.Initial_reserve) AS InitialReserve,
        SUM(wr.Paid_to_date) AS PaidAmount,
        SUM(wr.Revised_reserve) AS RevisedReserve,
        SUM(wr.Initial_reserve) -
        SUM(wr.Paid_to_date) +
        SUM(wr.Revised_reserve) AS CurrentReserve,
        (select Claim_Peril.sum_insured from Claim_Peril where Claim_Peril.Claim_id =wcp.Claim_id and Claim_Peril.Claim_Peril_id =wcp.claim_Peril_id ) as SumInsured,
		SUM(wr.Average) AS Average
    FROM Claim_Peril wcp
        INNER JOIN Reserve wr ON
            wcp.claim_peril_id = wr.claim_peril_id
        INNER JOIN Reserve_type rt ON
            wr.Reserve_type_id = rt.Reserve_type_id
    WHERE wcp.Claim_id = @claim_id
        AND rt.Include_in_Total = 1
    GROUP BY
		wcp.Claim_id,
		wcp.claim_peril_id,
		wcp.Description,
		wcp.Sum_insured 
END

GO

SET QUOTED_IDENTIFIER OFF
GO

