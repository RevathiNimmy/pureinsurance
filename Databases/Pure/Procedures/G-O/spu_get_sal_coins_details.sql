SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_sal_coins_details'
GO

CREATE PROCEDURE spu_get_sal_coins_details    
    @ClaimID INT    
AS    
    

    Select  
	P.Party_cnt,    
        P.name,    
        CP.Share,    
        CP.Share_Value    
    from    Claim_Party CP,    
        Party P    
    where   CP.Claim_id = @ClaimID    
    AND CP.Party_id = P.Party_cnt    
    AND insurer_type = 0    




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
