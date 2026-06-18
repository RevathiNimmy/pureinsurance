SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Recovery_Details'
GO

CREATE  PROCEDURE spu_SAM_Get_Claim_Recovery_Details
  
@claim_id integer  
  
AS  
  
SELECT  
  
 	r.Recovery_id,  
 	r.claim_Peril_id,  
 	r.Recovery_type_id,  
 	r.Currency_id,  
 	cur.code as Currency_code,  
 	r.Initial_reserve,  
 	r.revised_reserve,  
 	r.received_to_date,  
 	r.revision_count,  
 	r.Tax_Amount,  
 	r.base_recovery_id,  
 	r.version_id,  
 	rt.code,  
	--Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc) - (5.1.2)  
	--Line that had been  Added  
	rt.is_salvage,  
 	--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc) - (5.1.2)  
	--Start (PBI #36525 - Idea 72 - Claim Recovery Multi-Party)
	r.recovery_party_type_id,
	r.recovery_party_cnt,
	p.ShortName,
	rpt.code as recovery_party_type_code
	--End (PBI #36525 - Idea 72 - Claim Recovery Multi-Party)
   
  
FROM recovery r  
  
 	LEFT JOIN currency cur ON  
  	cur.currency_id = r.currency_id  
  
 	LEFT JOIN recovery_type rt ON  
  	rt.recovery_type_id = r.recovery_type_id
	--Start (PBI #36525 - Idea 72 - Claim Recovery Multi-Party)
	LEFT JOIN party p ON p.party_cnt = r.recovery_party_cnt
	LEFT JOIN Recovery_Party_Type rpt ON rpt.Recovery_Party_Type_id = r.recovery_party_type_id
	--End (PBI #36525 - Idea 72 - Claim Recovery Multi-Party)
  
WHERE claim_peril_id in (Select claim_peril_id from claim_peril where claim_id = @claim_id)  





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
