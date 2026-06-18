SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_delete_claim_party_claim'
GO

CREATE PROCEDURE spu_delete_claim_party_claim  
    @PartyClaimId int,  
    @ClaimId int  
AS  
  
BEGIN
    DELETE FROM Claim_Party_Claim  
    WHERE Claim_Id = @ClaimId  
    AND party_claim_id = @PartyClaimId 
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
