SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Claim_Peril_Upd'
GO

CREATE PROCEDURE spu_CLM_Claim_Peril_Upd

    @claim_peril_id int,
    @description varchar(50) = Null,
    @comments varchar(255) = Null

AS

    UPDATE Work_Claim_Peril
    SET description = @description, comments = @comments
    WHERE claim_peril_id = @claim_peril_id
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

