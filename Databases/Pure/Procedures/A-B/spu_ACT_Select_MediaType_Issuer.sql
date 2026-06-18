SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_MediaType_Issuer'
GO

-- Get a list of MediaType_Issuer records for a given mediatype_id.
-- We exclude ones that have is_deleted = 1 and effective_date > system
-- date. We also ensure that is_allowed is 1. If this is a claim type 
-- payment then we also ensure is_allowed_for_claims is 1.

CREATE PROCEDURE spu_ACT_Select_MediaType_Issuer 
    @mediatype_id 		int,
    @is_claim_type_payment      tinyint
AS

SELECT mediatype_issuer_id,
       description
FROM   mediatype_issuer 
WHERE  mediatype_id = @mediatype_id
AND    is_deleted = 0
AND    effective_date <= getdate()
AND    is_allowed = 1
AND    ((@is_claim_type_payment = 0)
    OR
       (@is_claim_type_payment = 1 AND is_allowed_for_claims=1))
ORDER BY description

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO



