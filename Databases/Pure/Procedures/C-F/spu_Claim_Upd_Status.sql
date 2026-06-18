SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Upd_Status'
GO
CREATE PROCEDURE spu_Claim_Upd_Status
    @Claim_id int
AS
    UPDATE CLAIM SET
    Claim_Status_id = 4
    WHERE Claim_id = @Claim_id 