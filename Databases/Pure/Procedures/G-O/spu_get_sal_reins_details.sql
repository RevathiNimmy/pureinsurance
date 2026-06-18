SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_sal_reins_details'
GO

CREATE PROCEDURE spu_get_sal_reins_details    
    @ClaimID int    
AS    
    
    Select  
	P.Party_cnt,    
        P.name,    
        CP.Share,    
        CP.Share_Value    
    from    
	Claim_Party CP,    
        Party P,    
        Party_Insurer PIn    
    where   CP.Claim_id = @ClaimID    
    AND CP.Party_id = P.Party_cnt    
    AND P.Party_cnt = PIn.Party_cnt    
    AND PIn.is_reinsurer = 1    
    AND insurer_type=1    




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
