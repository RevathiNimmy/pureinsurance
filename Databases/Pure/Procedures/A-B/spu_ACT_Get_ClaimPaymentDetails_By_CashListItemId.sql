EXECUTE DDLDropProcedure 'spu_ACT_Get_ClaimPaymentDetails_By_CashListItemId '
GO

CREATE PROCEDURE spu_ACT_Get_ClaimPaymentDetails_By_CashListItemId(
                 @CashListItem_id INT)
AS
BEGIN
	SELECT CP.document_id,CCL.claim_payment_id FROM Claim_Payment CP
		JOIN CashListItem_Claim_Link CCL 
		ON CCL.claim_payment_id=CP.claim_payment_id
		WHERE CCL.cashlistitem_id=@CashListItem_id

END
GO