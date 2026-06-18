SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Perils'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Claim_Perils

@claim_id integer

AS

select claim_peril_id, peril_type.code, base_claim_peril_id 
from claim_peril
inner join peril_type on 
	claim_peril.peril_type_id = peril_type.peril_type_id
where claim_id = @claim_id



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
