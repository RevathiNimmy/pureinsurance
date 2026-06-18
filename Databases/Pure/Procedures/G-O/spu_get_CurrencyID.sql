SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_CurrencyID'
GO

CREATE PROCEDURE spu_get_CurrencyID    
    @ClaimID INT    
AS    
    
    SELECT  Currency_id,
	    coinsurance_treatment_id    
    FROM    Claim    
    WHERE   Claim_id= @ClaimID    




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
