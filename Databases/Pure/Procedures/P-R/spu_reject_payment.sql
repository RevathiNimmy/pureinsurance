SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_reject_payment'
GO

CREATE PROCEDURE spu_reject_payment
    @ClaimPerilID INT,
    @SequenceNo INT,
    @ClaimPaymentId INT
AS

/*Remove payment from reserve table */
UPDATE r
SET r.paid_to_date = r.paid_to_date - pi.this_payment
FROM reserve r
JOIN claim_payment_item pi
	ON pi.reserve_id = r.reserve_id
JOIN claim_payment p
	ON p.claim_payment_id = pi.claim_payment_id
WHERE p.claim_peril_id = @ClaimPerilID
--AND p.SequenceNo = @SequenceNo
AND pi.claim_payment_id = @ClaimPaymentId

/*Remove payment from reserve table */
DELETE claim_payment_item
WHERE claim_payment_id = @ClaimPaymentId
DELETE claim_payment
WHERE claim_peril_id = @ClaimPerilID
--AND SequenceNo = @SequenceNo
AND  claim_payment_id = @ClaimPaymentId

GO