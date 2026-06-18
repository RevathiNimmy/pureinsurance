SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Claim_Check_BaseReservekey'
GO


CREATE PROCEDURE spu_SAM_Claim_Check_BaseReservekey

@base_reserve_Id int, 
@base_claim_peril_Id int, 
@count int OUTPUT

AS

DECLARE @version_id int
DECLARE @claim_peril_id int

IF EXISTS(

SELECT claim_peril.claim_peril_id, claim_peril.version_id FROM claim_peril INNER JOIN 
(Select version_id, claim_peril_id from reserve where base_reserve_id = @base_reserve_id) Reserve ON
	reserve.version_id = claim_peril.version_id 
	AND reserve.claim_peril_id = claim_peril.claim_peril_id
WHERE base_claim_peril_id = @base_claim_peril_id )

SELECT @count = 1
ELSE 
SELECT @count = 0


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
