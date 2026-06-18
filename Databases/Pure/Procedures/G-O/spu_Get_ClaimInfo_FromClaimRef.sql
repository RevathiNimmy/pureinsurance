SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_Get_ClaimInfo_FromClaimRef'
GO

Create Procedure spu_Get_ClaimInfo_FromClaimRef
    @Claim_Ref varchar(50)
AS
Select MAX(Claim_ID) from Claim
WHERE Claim_number=@Claim_Ref