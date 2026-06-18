SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Claim_Check_No'
GO

CREATE PROCEDURE spe_Claim_Check_No
        @Claim_id int
AS
Select count(*) from Claim where Claim_id = @Claim_id

GO

