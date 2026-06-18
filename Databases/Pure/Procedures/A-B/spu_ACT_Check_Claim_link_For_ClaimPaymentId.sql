EXECUTE DDLDropProcedure 'spu_ACT_Check_Claim_link_For_ClaimPaymentId'
GO

CREATE PROCEDURE spu_ACT_Check_Claim_link_For_ClaimPaymentId(
                 @Claimpayment_id INT)
AS
BEGIN
	SELECT cashlistitem_claim_link_id FROM CashListItem_Claim_Link WHERE claim_payment_id=@Claimpayment_id AND ISNULL(is_deleted,0)=0
END
GO
