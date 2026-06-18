SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_select_payment_details'
GO


CREATE PROCEDURE spu_select_payment_details
	@ClaimCnt INT,
	@ClaimPerilID INT,
	@SequenceNo INT,
	@claimPaymentId INT 
AS

SELECT
	sum(ISNULL(pi.this_payment,0)) 'Amount', 
	(
		SELECT shortname 
		FROM party
		WHERE party_cnt = max(p.party_cnt)
		 
	) 'Party_Code',
	max(ISNULL(p.comments,'')),
	max(ISNULL(p.PayeeMediaType,' ')),
	max(ISNULL(p.PayeeName,' ')),
	max(ISNULL(p.PayeeBankName,' ')),
	max(ISNULL(p.PayeeSortCode,' ')),
	max(ISNULL(p.PayeeAccountNo,' ')),
	max(ISNULL(p.PayeeCountry,' ')),
	max(ISNULL(p.PayeeComments,' ')),
	max(ISNULL(pi.reserve_id,0))
FROM claim_payment p
JOIN claim_payment_item pi ON pi.claim_payment_id = p.claim_payment_id
WHERE p.claim_payment_id = @ClaimPaymentId
 
GROUP BY pi.reserve_id