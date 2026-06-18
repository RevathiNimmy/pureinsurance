DDLDropprocedure 'spu_SAM_GetClaimFromRIArrangement'
go

CREATE PROCEDURE spu_SAM_GetClaimFromRIArrangement 
@Claim_ri_arrangement_line_id int  
AS  
Select ri.claim_id, tt.code
From claim_ri_arrangement ri 
INNER JOIN claim_ri_arrangement_line ril
	ON ri.ri_arrangement_id = ril.ri_arrangement_id AND ri.claim_id = ril.claim_id
INNER JOIN claim c
	ON c.claim_id = ri.claim_id
INNER JOIN transaction_type tt
	ON c.transaction_type_id = tt.transaction_type_id
WHERE Claim_ri_arrangement_line_id = @Claim_ri_arrangement_line_id  
GO