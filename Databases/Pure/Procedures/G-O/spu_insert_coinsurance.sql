SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_insert_coinsurance'
GO

CREATE PROCEDURE spu_insert_coinsurance  
    @ClaimID INT,  
    @PartyID INT,  
    @Share numeric(12, 8),  
    @Share_Value numeric(19, 4)  
AS  
  
BEGIN
    INSERT INTO Claim_Party (Claim_id, Party_id, Share, Share_Value, insurer_type)  
    VALUES(@ClaimID, @PartyID, @Share, @Share_Value, 0)  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
