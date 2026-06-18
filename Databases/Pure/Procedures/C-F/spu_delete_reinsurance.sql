SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_delete_reinsurance'
GO

CREATE PROCEDURE spu_delete_reinsurance  
    @PartyID numeric  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
BEGIN
    	DELETE FROM Claim_Party 
	WHERE Party_id =@PartyID  
    	AND insurer_type=1  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
