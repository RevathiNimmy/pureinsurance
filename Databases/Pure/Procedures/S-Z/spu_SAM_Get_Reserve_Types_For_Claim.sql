SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Reserve_Types_For_Claim
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.2.4)
CREATE  PROCEDURE spu_SAM_Get_Reserve_Types_For_Claim
    @claim_id int
AS
BEGIN

    SELECT DISTINCT
        Reserve.Reserve_type_id AS [Key],
        Reserve_type.Description,
        Reserve_type.Name AS Code
    FROM claim_Peril
        INNER JOIN  Reserve ON
            claim_Peril.claim_Peril_id = Reserve.claim_Peril_id
        INNER JOIN  Reserve_type ON
            Reserve.Reserve_type_id = Reserve_type.Reserve_type_id
    WHERE claim_Peril.Claim_id = @claim_id

END
--End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.2.4)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
