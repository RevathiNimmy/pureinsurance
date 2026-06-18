SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Payment_Item_Reserve_Update'
GO

CREATE PROCEDURE spu_CLM_Claim_Payment_Item_Reserve_Update  
 @reserve_id int,
 @this_revision money,
 @this_payment money,
 @nSkipUpdateReserve int = 0,
 @transaction_type varchar(10)='',
 @tax_amount as money = 0,
 @nDataTransfer int = 0
AS  
  
 BEGIN  
 declare @ClaimsReservesareGross int =0

 if exists (select null from system_options where description ='Claims Reserves are Gross' AND value = 1)
 BEGIN
 set @ClaimsReservesareGross =1
 end
 DECLARE 
        @nround_to_palce int,
		@ncurrency_id int
BEGIN

SELECT @ncurrency_id = currency_id FROM claim_payment_item
WHERE  reserve_id= @reserve_id

SELECT  @nround_to_palce = ISNULL(round_to_places,0) FROM currency
WHERE   currency_id = @ncurrency_id

IF (@nround_to_palce = 0)
BEGIN

	SET	@nround_to_palce = 2
END
	
	DECLARE @PrevGrossReserve as money = 0
	SELECT @PrevGrossReserve = Gross_Reserve FROM Reserve WHERE reserve_id = @reserve_id

 IF @nSkipUpdateReserve = 0
 BEGIN
 IF @transaction_type = 'C_CP' AND @nDataTransfer = 1
 BEGIN
	UPDATE reserve
	SET this_revision = 0.0,
		this_payment = @this_payment,
		revised_reserve = ISNULL(revised_reserve,0) - this_revision + @this_revision,
		paid_to_date = ISNULL(paid_to_date,0) + @this_payment,
		revision_count = ISNULL(revision_count,0) + 1,
		average =  CASE
			WHEN ISNULL(sum_insured,0) <> 0 THEN
			 ((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) + @this_revision) / ISNULL(sum_insured,0)) * 100
			ELSE 0
			END,
		paid_to_date_tax = case when @ClaimsReservesareGross =0 then 0 else ISNULL(paid_to_date_tax,0) + ISNULL(@tax_amount,0) end,
	    Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else Gross_Reserve - (@this_payment + @tax_amount) end,
	    tax = case when @ClaimsReservesareGross =0 then 0 else tax - @tax_amount end
 
  WHERE reserve_id = @reserve_id
  END
  ELSE
  BEGIN
		UPDATE reserve
		Set this_revision = CASE WHEN ISNULL(@this_revision,0)=0 THEN this_revision ELSE @this_revision END,
			this_payment = @this_payment,  
			revised_reserve = ISNULL(revised_reserve,0) + @this_revision,  
			paid_to_date = ROUND(ISNULL(paid_to_date,0) + @this_payment,@nround_to_palce),
			revision_count = ISNULL(revision_count,0) + 1,  
			average =  CASE 
						WHEN ISNULL(sum_insured,0) <> 0
						THEN
						((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) + @this_revision) / ISNULL(sum_insured,0)) * 100 
						ELSE 0 
						END,
			paid_to_date_tax = case when @ClaimsReservesareGross =0 then 0 else ISNULL(paid_to_date_tax,0) + ISNULL(@tax_amount,0) end,
		    Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else Gross_Reserve - (@this_payment + @tax_amount) end,
		    tax = case when @ClaimsReservesareGross =0 then 0 else tax - @tax_amount end

		WHERE reserve_id = @reserve_id  
  END
 END
  ELSE
 BEGIN
		SET  @this_revision = 0.0
		UPDATE reserve
		SET this_payment = @this_payment,
		paid_to_date = ISNULL(paid_to_date,0) + @this_payment,
		average =  CASE
		WHEN ISNULL(sum_insured,0) <> 0 THEN
		((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) + @this_revision) / ISNULL(sum_insured,0)) * 100
		ELSE 0
		END,
		paid_to_date_tax = case when @ClaimsReservesareGross =0 then 0 else ISNULL(paid_to_date_tax,0) + ISNULL(@tax_amount,0) end,
		Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else Gross_Reserve - (@this_payment + @tax_amount) end,
		tax = case when @ClaimsReservesareGross =0 then 0 else tax - @tax_amount end
		WHERE reserve_id = @reserve_id
 END
  UPDATE reserve SET initial_reserve = @this_revision
  WHERE reserve_id = @reserve_id AND initial_reserve IS NULL
  
  IF @this_payment <> 0
  BEGIN
	  UPDATE reserve 
	  SET 	Revised_Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else Revised_Gross_Reserve - (@this_payment + @tax_amount) end,
			Revised_Tax_Reserve = case when @ClaimsReservesareGross =0 then 0 else Revised_Tax_Reserve - @tax_amount end
	  WHERE reserve_id = @reserve_id
  END 
	IF (EXISTS (SELECT 1 from System_Options WHERE option_number = 5239 and value = 1) AND -- Claim Reserves Are Gross
	ISNULL(@tax_amount,0) = 0 AND @PrevGrossReserve = @this_payment )
	BEGIN
		UPDATE Reserve 
		SET		Tax = 0,
				Revised_Tax_Reserve = 0
		WHERE reserve_id = @reserve_id
			AND (Tax <> 0 OR Revised_Tax_Reserve <> 0)
	END

 END  
 END
GO
SET QUOTED_IDENTIFIER OFF
GO
