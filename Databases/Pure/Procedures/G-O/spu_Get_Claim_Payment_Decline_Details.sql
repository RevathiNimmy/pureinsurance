EXECUTE DDLDropProcedure 'spu_Get_Claim_Payment_Decline_Details'
GO

CREATE PROCEDURE spu_Get_Claim_Payment_Decline_Details 
      @nClaimKey INT ,
	  @bIsPaymentDeclined TINYINT OUTPUT,
	  @nDeclinedClaimKey INT OUTPUT,
	  @nPreviousValidClaimKey INT OUTPUT  
AS  
BEGIN

DECLARE @lBaseClaimKey INT
Declare @lPreviousClaimKey INT

SELECT @lBaseClaimKey=base_claim_id FROM Claim  WHERE claim_id =@nClaimKey 

select top 1 @lPreviousClaimKey = Claim_id
from Claim
where base_claim_id =@lBaseClaimKey
and is_dirty=0
AND claim_id<@nClaimKey
order by claim_id desc


SELECT TOP 1 @bIsPaymentDeclined=1
FROM Claim_Payment
WHERE is_referred=2
AND claim_id =@lPreviousClaimKey

IF ISNULL(@bIsPaymentDeclined,0)= 1
BEGIN

	select  TOP 1 @nPreviousValidClaimKey=C.claim_id 
	from claim C
	where C.base_claim_id=@lBaseClaimKey
	and C.Claim_id<@lPreviousClaimKey
	and is_dirty=0
	and  NOT EXISTS (SELECT CLAIM_ID FROM claim_payment WHERE claim_id=C.claim_id and is_referred=2)
	order by claim_id desc	
	
	Set @nDeclinedClaimKey= @lPreviousClaimKey
END

SET @bIsPaymentDeclined= ISNULL(@bIsPaymentDeclined,0)
SET @nDeclinedClaimKey =ISNULL(@nDeclinedClaimKey,0) 
SET @nPreviousValidClaimKey =ISNULL(@nPreviousValidClaimKey,0) 

END
SET QUOTED_IDENTIFIER OFF
GO
