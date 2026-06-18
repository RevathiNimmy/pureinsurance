DDLDROPPROCEDURE spu_SAM_Get_Claim_Payments_ById
GO
CREATE PROCEDURE spu_SAM_Get_Claim_Payments_ById

  @claim_payment_id INT

AS

BEGIN

SELECT

 claim_payment_id,

 claim_id,

 isnull(PayeeName,'') as PayeeName,

 isnull(PayeeBankName,'') as PayeeBankName,

 isnull(PayeeSortCode,'') as PayeeSortCode,

 isnull(PayeeAccountNo,'') as PayeeAccountNo,

 isnull(media_ref,'') as media_ref,

 isnull(PayeeAddress1,'') as PayeeAddress1,

 isnull(PayeeAddress2,'') as PayeeAddress2,

 isnull(PayeeAddress3,'') as PayeeAddress3,

 isnull(PayeeAddress4,'') as PayeeAddress4,

 isnull(PayeePostalCode,'') as PayeePostalCode,

 isnull(ThirdPartyReference,'') as ThirdPartyReference,

 isnull(our_ref,'') as our_ref

 FROM Claim_Payment WHERE claim_payment_id=@claim_payment_id



END
