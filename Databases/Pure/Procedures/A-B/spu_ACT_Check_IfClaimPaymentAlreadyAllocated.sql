SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_ACT_Check_IfClaimPaymentAlreadyAllocated'
GO

CREATE PROCEDURE spu_ACT_Check_IfClaimPaymentAlreadyAllocated
 @claim_payment_id INT
AS

BEGIN

	SELECT COUNT(1)
	FROM AllocationDetail  ad

    JOIN TransDetail td ON
    ad.transdetail_id  = td.transdetail_id

    JOIN Claim_Payment cp ON
    td.document_id = cp.document_id
		
	WHERE CP.base_claim_payment_id = CP.claim_payment_id
	AND CP.base_claim_payment_id = @claim_payment_id
END

GO