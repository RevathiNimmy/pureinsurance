SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_party'
GO

CREATE PROCEDURE spu_add_party  
    @partyclaimid int,  
    @perilid int,  
    @claimid int  
AS  
  
BEGIN
    INSERT INTO Peril_Party(  
     claim_peril_id,  
     claim_id,  
     party_claim_id) 
    VALUES(  
     @perilid,  
     @claimid,  
     @partyclaimid)  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
