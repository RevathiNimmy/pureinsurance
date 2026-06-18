DDLDropProcedure 'spu_GetClaimPosting'
Go

CREATE PROCEDURE spu_GetClaimPosting @ClaimNumber Varchar(30)

AS

SELECT c.policy_number,
		c.claim_number,
		sf.document_ref,
		sf.document_date,
		sd.stats_detail_type,
		sd.this_premium_home,
		sf.posting_period_number
FROM Claim c JOIN Stats_Folder sf ON c.claim_id = sf.loss_id
JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
WHERE c.claim_number = @ClaimNumber
AND sd.stats_detail_type IN ('GRS','NET','FAC','TTY')
ORDER BY sf.document_date DESC
