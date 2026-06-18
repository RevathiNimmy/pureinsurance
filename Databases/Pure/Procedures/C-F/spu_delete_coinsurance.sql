SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_delete_coinsurance'
GO

CREATE PROCEDURE spu_delete_coinsurance  
	@ClaimId int , 
	@PartyID int = 0
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
BEGIN  
 DELETE FROM Claim_Party  
 WHERE ((@partyid = 0) OR (Party_id = @PartyID))  
 AND claim_id = @claimID
 AND insurer_type=0  
END  




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
