SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_update_coinsurance'
GO

CREATE PROCEDURE spu_update_coinsurance  
    @PartyID numeric,  
    @Share INT,  
    @Share_Value currency,  
    @ClaimID int  
AS  
  
BEGIN
	UPDATE Claim_Party  
	SET Share=@Share,Share_Value=@Share_Value  
	WHERE party_id=@PartyID  
	AND claim_id=@ClaimID AND insurer_type=0  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
