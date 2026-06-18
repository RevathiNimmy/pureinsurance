SET QUOTED_IDENTIFIER OFF    
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SAM_Party_Claim_list'
GO

CREATE PROCEDURE spu_SAM_Party_Claim_list
            @party_cnt int
AS

    SELECT
    	C.claim_id,
        C.claim_number,
        C.description,
        C.loss_from_date AS loss_date,
        C.reported_date,
        C.policy_number,
        P.description AS product_description,
        C.claim_status_id,
        CS.code AS claim_status_code,
        CS.description AS claim_status_description,
        CASE CS.code
	    WHEN 'PRVOPENCLM' THEN 0
	    WHEN 'LIVOPENCLM' THEN 0
	    WHEN 'REOPENED' THEN 0
	    WHEN 'CLOSED' THEN 1
	    WHEN 'RECLOSED' THEN 1
	END AS claim_closed_status
        FROM claim C
        INNER JOIN claim_status CS
        ON CS.claim_status_id = C.claim_status_id
        INNER JOIN insurance_file INF
        ON INF.insurance_file_cnt = C.policy_id
        INNER JOIN product P
        ON P.product_id = INF.product_id
        WHERE C.client_id = @party_cnt
	ORDER BY claim_closed_status
	
GO
