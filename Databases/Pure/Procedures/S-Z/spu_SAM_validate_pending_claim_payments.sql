
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_validate_pending_claim_payments'
GO

CREATE Procedure spu_SAM_validate_pending_claim_payments 
    @claim_ID INT,
    @has_xol INT OUTPUT
AS
BEGIN
    DECLARE @xol_count INT,@pending_count INT

    SELECT @xol_count = Count(*) FROM Claim_RI_Arrangement_line  
    WHERE Claim_id IN (SELECT clmb.Claim_id
		FROM claim clm 
		INNER JOIN Transaction_Type tt ON clm.transaction_type_id = tt.transaction_type_id 
		INNER JOIN Claim clmb ON clm.base_claim_id = clmb.base_claim_id 
		INNER JOIN Claim_Payment clp ON clmb.Claim_id = clp.claim_id 
		WHERE clm.Claim_id = @claim_ID AND tt.code = 'C_CP' AND clp.is_referred = 1)  
	AND Type IN ('FX','TX','X') AND this_payment <> 0

	Select @pending_count = COUNT(*) from Claim_Payment Where is_referred = 1 AND Claim_id IN (SELECT claim_id FROM claim 
					WHERE transaction_type_id  =(SELECT transaction_type_id FROM Transaction_Type 
												WHERE code = 'C_CP') 
					AND base_claim_id = (SELECT base_claim_id FROM claim 
										WHERE claim_id = @claim_id))

    If @xol_count > 0 AND @pending_count > 0
    BEGIN		
		UPDATE reserve 
		SET this_revision = 0, this_payment = 0 
		WHERE claim_peril_id  IN (SELECT cp.claim_peril_id FROM claim c 
				INNER JOIN claim_peril cp ON c.claim_id = cp.claim_id
				WHERE c.claim_id = @claim_id)

		UPDATE claim_payment 
		SET amount = 0, tax_amount = 0 
		WHERE claim_id = @claim_id
		
		UPDATE claim_payment_item 
		SET this_payment = 0, tax_amount = 0 
		WHERE claim_payment_id IN (SELECT cpi.claim_payment_id FROM claim_payment cp 
				INNER JOIN claim_payment_item cpi ON cp.claim_payment_id = cpi.claim_payment_id
				WHERE cp.claim_id = @claim_id)
		
		UPDATE claim_ri_arrangement 
		SET this_reserve = 0, this_payment = 0 
		WHERE claim_id = @claim_id
		
		UPDATE claim_ri_arrangement_line 
		SET this_reserve = 0, this_payment = 0 
		WHERE claim_id = @claim_id
		
		UPDATE Claim 
		SET transaction_type_id = 3 
		WHERE claim_id = @claim_id  
		
		DELETE stats_detail 
		WHERE stats_folder_Cnt IN (SELECT stats_folder_cnt FROM stats_folder 
								WHERE loss_id = @claim_id)
		DELETE stats_folder 
		WHERE loss_id = @claim_id
		
		SET @has_xol = 1
   END
   ELSE
		SET @has_xol = 0   
END

