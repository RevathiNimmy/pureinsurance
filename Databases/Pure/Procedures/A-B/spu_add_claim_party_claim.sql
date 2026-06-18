SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_claim_party_claim'
GO

CREATE PROCEDURE spu_add_claim_party_claim  
    @PartyClaimId int,  
    @ClaimId int  
AS  

BEGIN
    INSERT INTO claim_party_claim(Claim_ID, Party_Claim_ID)  
	    VALUES(@ClaimId,@PartyClaimId)  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
