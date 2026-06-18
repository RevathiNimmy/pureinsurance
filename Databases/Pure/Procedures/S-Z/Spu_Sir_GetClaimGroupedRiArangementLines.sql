SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXEC DDLDropProcedure 'Spu_Sir_GetClaimGroupedRiArangementLines'
GO
Create Procedure Spu_Sir_GetClaimGroupedRiArangementLines  
@claim_Id int,  
@GroupingId int,  
@ProcessId int  
As  
  
if @ProcessId = 2  
Begin  
	SELECT DISTINCT  
		shortname,  
		name,  
		Acct_Type = CASE WHEN is_RI_Broker=1  
				THEN 'Broker'  
			WHEN is_Retained=1  
				THEN 'Retained'  
			ELSE 'Reinsurer' END,  
		participation_percent,  
		sum_insured,  
		agreement_code,  
		ral.party_cnt,  
		ri_arrangement_line_id, 
		reserve,
		this_reserve,
		payment,
		this_payment,
		recovery   
	FROM Claim_RI_Arrangement_Line ral  
		LEFT Join Party on party.party_cnt = ral.party_cnt  
		Join Party_insurer On ral.party_cnt = party_insurer.party_cnt  
	WHERE Party.is_deleted = 0  
		AND claim_id=@claim_Id  
		AND grouping =@GroupingId  
	ORDER BY shortname  
End  

