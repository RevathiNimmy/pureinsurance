DDLDropProcedure 'spu_GetImbalaceClosedClaim'
Go

CREATE PROCEDURE spu_GetImbalaceClosedClaim
				@ClaimNumber varchar(30)
AS
BEGIN

-- closed claims with transactions do not sum to zero

	DECLARE @CLO int, 
			@CLA int, 
			@CLP int

	SELECT @CLO = transaction_type_id FROM Transaction_Type WHERE code = 'C_CO'
	SELECT @CLA = transaction_type_id FROM Transaction_Type WHERE code = 'C_CR'
	SELECT @CLP = transaction_type_id FROM Transaction_Type WHERE code = 'C_CP'

	SELECT  c.claim_id,
	        (SELECT claim_number FROM Claim WHERE claim_id = c.claim_id) ClaimNumber,
			IsNull(SUM(case when c.transaction_type_id = @CLO then this_premium_home else 0 end),0) CLO,
				IsNull((SELECT SUM(r.initial_reserve) FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id 
				WHERE cp.claim_id = c.claim_id),0) InitReserve,
			IsNull(SUM(case when c.transaction_type_id = @CLA then this_premium_home else 0 end),0) CLA,
			IsNull((SELECT SUM(r.revised_reserve) FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id 
				WHERE cp.claim_id = c.claim_id),0) RevisedReserve,
			IsNull(SUM(case when c.transaction_type_id = @CLP then this_premium_home else 0 end),0) CLP,
			IsNull((SELECT SUM(r.paid_to_date) FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id 
				WHERE cp.claim_id = c.claim_id),0) PaidToDate,
			IsNull((SELECT SUM(amount) FROM Payment WHERE claim_id = c.claim_id),0) PaymentTable
	FROM    claim c 
	JOIN    stats_folder sf on sf.loss_id = c.claim_id
	JOIN    stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
	WHERE   sd.stats_detail_type = 'GRS'
	AND     sf.transaction_type_id in (1, 2, 3) -- open, adjustment, payment
	AND     c.claim_status_id = 3
	AND     (c.claim_number = @ClaimNumber OR @ClaimNumber IS NULL)
	GROUP BY c.claim_id
	HAVING  SUM(CASE WHEN sf.transaction_type_id = 3 THEN sd.this_premium_home * -1 ELSE sd.this_premium_home END) <> 0
	ORDER BY c.claim_id

END

