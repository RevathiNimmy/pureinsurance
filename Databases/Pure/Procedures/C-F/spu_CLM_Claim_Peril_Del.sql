SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Claim_Peril_Del'
GO

CREATE PROCEDURE spu_CLM_Claim_Peril_Del

    @claim_peril_id int

AS

    DELETE FROM Work_Claim_Peril
    WHERE claim_peril_id = @claim_peril_id

    DELETE FROM Work_Peril_Party
    WHERE claim_peril_id = @claim_peril_id
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

