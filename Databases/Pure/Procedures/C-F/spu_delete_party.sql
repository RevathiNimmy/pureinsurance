SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_delete_party'
GO

CREATE PROCEDURE spu_delete_party  
    @partyclaimid int  
AS  
  
BEGIN
	DELETE FROM Peril_Party 
	WHERE party_claim_id=@partyclaimid  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
