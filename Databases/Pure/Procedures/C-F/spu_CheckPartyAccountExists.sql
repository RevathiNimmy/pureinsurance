SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CheckPartyAccountExists'  
GO

-- To Check whether Account Exists for a Party or Not.

CREATE PROCEDURE spu_CheckPartyAccountExists  
  
    @PartyCnt int,  
    @Exists BIT OUTPUT  
  
AS  
  
/*Default parameter to not existing*/  
SELECT @Exists = 0  
  
/*If account code exists in the party table then set parameter to show it as existing*/  
IF EXISTS  
    (  
        SELECT  
            NULL  
        FROM Account 
        WHERE account_key = @PartyCnt  
    )  
BEGIN  
    SELECT @Exists = 1  
END  
  