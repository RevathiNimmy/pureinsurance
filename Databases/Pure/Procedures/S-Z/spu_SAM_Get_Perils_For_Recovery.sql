SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Perils_For_Recovery
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.2.2)
CREATE  PROCEDURE spu_SAM_Get_Perils_For_Recovery
    @claim_id int,
    @is_salvage int
AS

BEGIN
    SELECT
        Recovery.claim_peril_id AS ClaimPerilKey,
        Claim_Peril.Description AS Description,
        SUM(Recovery.Initial_reserve) AS InitialRecovery,
        SUM(Recovery.received_to_date) AS ReceiptedAmount,
        SUM(Recovery.revised_reserve) AS RevisedRecovery,
        SUM(Recovery.Initial_reserve) -
        SUM(Recovery.received_to_date) +
        SUM(Recovery.revised_reserve) AS CurrentRecovery
    FROM Claim_Peril
        INNER JOIN  Recovery ON
            Claim_Peril.claim_peril_id = Recovery.claim_peril_id
        INNER JOIN  Recovery_type ON
            Recovery.recovery_type_id = Recovery_type.recovery_type_id
    WHERE Claim_Peril.Claim_id = @claim_id
        AND  Recovery_type.is_salvage = @is_salvage
    GROUP BY Recovery.claim_peril_id, Claim_Peril.Description
END
--End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.2.2)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
