SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_GET_PartyLoyaltyScheme
GO
--Start (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.20) 
create procedure spu_SAM_GET_PartyLoyaltyScheme
 @party_cnt int  
AS  
SELECT  
    	party_cnt,  
    	party_loyalty_scheme_id,
    	Party_Loyalty_Scheme.loyalty_scheme_id,
	membership_number,
	other_reference,
	start_date,
	end_date,
	main_membership_number,
	is_active,
	Loyalty_Scheme.description
   
FROM Party_Loyalty_Scheme
JOIN Loyalty_Scheme ON  Loyalty_Scheme.loyalty_scheme_id = Party_Loyalty_Scheme.loyalty_scheme_id
WHERE party_cnt = @party_cnt  
ORDER BY party_loyalty_scheme_id ASC  
--End (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.20) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


  
