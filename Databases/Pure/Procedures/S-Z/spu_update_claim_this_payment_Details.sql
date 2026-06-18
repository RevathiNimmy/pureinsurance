EXECUTE DDLDropProcedure 'spu_update_claim_this_payment_Details'
GO
CREATE PROCEDURE spu_update_claim_this_payment_Details
	@nClaimID AS INT,
	@nClaimPaymentID AS INT=NULL,
	@nClaimPerilID AS INT=NULL,
	@nPayeeMediaType AS INT,
	@smedia_ref AS VARCHAR(30),
	@sPayeeName AS VARCHAR(255)=NULL,
	@sPayeeBankName AS VARCHAR(255)=NULL,
	@sPayeeSortCode AS VARCHAR(8)=NULL,
	@sPayeeAccountNo AS VARCHAR(30)=NULL,
	@nPayeeCountry AS INT=NULL,
	@sPayeeComments AS VARCHAR(255)=NULL,
	@sPayeeAddress1 AS VARCHAR(60)=NULL,
	@sPayeeAddress2 AS VARCHAR(60)=NULL,
	@sPayeeAddress3 AS VARCHAR(60)=NULL,
	@sPayeeAddress4 AS VARCHAR(60)=NULL,
	@sPayeePostalCode AS VARCHAR(20)=NULL,
	@sThirdPartyReference AS VARCHAR(30)=NULL,
	@sOur_ref AS VARCHAR(30) =NULL
As
BEGIN

IF ISNULL(@nClaimPaymentID,0)=0 
SELECT @nClaimPaymentID =Claim_Payment_Id FROM claim_payment
WHERE claim_id=@nClaimID AND claim_peril_id=@nClaimPerilID
AND claim_payment_id=base_claim_payment_id
	

UPDATE Claim_Payment SET
PayeeMediaType = @nPayeeMediaType,
media_ref = @smedia_ref,
PayeeName = @sPayeeName,
PayeeBankName = @sPayeeBankName,
PayeeSortCode = @sPayeeSortCode,
PayeeAccountNo = @sPayeeAccountNo,
PayeeCountry = @nPayeeCountry,
PayeeComments = @sPayeeComments,
PayeeAddress1 = @sPayeeAddress1,
PayeeAddress2 = @sPayeeAddress2,
PayeeAddress3 = @sPayeeAddress3,
PayeeAddress4 = @sPayeeAddress4,
PayeePostalCode = @sPayeePostalCode,
ThirdPartyReference = @sThirdPartyReference,
our_ref = @sOur_ref

WHERE 
claim_id= @nClaimID AND 
claim_payment_id= @nClaimPaymentID


END
GO


