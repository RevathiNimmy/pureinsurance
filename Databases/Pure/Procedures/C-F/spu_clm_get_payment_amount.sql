SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_get_payment_amount'
GO

CREATE PROCEDURE  spu_clm_get_payment_amount  
 

@claim_id int,

@user_id int



AS

 BEGIN

  DECLARE @ThisPaymentAmount money

  DECLARE @ThisUsersClaimPayments money

  DECLARE @CurrencyId int

  DECLARE @OriginalPayment money

  DECLARE @OriginalCurrencyID INT



  SELECT @ThisUsersClaimPayments =  SUM (((ISNULL(cpi.this_payment,0) + ISNULL(cpi.tax_amount,0) + ISNULL(cpi.tax_amount_WHT,0))* ISNULL(cpi.currency_base_xrate, 1)))

  from Claim_Payment_item cpi

	INNER JOIN claim_payment cp ON

		cp.claim_payment_id = cpi.claim_payment_id

  Where created_by = @user_id

  and claim_id = @claim_id



  SELECT @ThisPaymentAmount = Sum(This_Payment)

  FROM Reserve

  WHERE claim_peril_id in (

	Select Claim_Peril_Id

	from Claim_Peril

	Where Claim_Id=@claim_id)



  SELECT @CurrencyId = base_currency_id

  FROM source

  WHERE source_id in (

	SELECT source_id

	FROM insurance_file

	WHERE InsurancE_file_cnt in (

		SELECT Policy_id

		FROM Claim

		WHERE claim_id =@Claim_Id))



--For Original payment



  SELECT @OriginalPayment =  SUM (((ISNULL(cpi.this_payment,0) + ISNULL(cpi.tax_amount,0) + ISNULL(cpi.tax_amount_WHT,0)))), @OriginalCurrencyID=MAX(cpi.currency_id) 

  from Claim_Payment_item cpi

	INNER JOIN claim_payment cp ON

		cp.claim_payment_id = cpi.claim_payment_id and cp.claim_payment_id=cp.base_claim_payment_id

  Where claim_id = @claim_id and cpi.this_payment<>0



 SELECT ISNULL(@ThisUsersClaimPayments, 0),ISNULL(@ThisPaymentAmount,0), ISNULL(@CurrencyID,26),ISNULL(@OriginalPayment,0),ISNULL(@OriginalCurrencyID,26)



END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
