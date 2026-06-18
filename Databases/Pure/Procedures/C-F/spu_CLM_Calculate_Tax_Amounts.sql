SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_Calculate_Tax_Amounts'
GO
CREATE PROCEDURE spu_CLM_Calculate_Tax_Amounts  

@company_id int,  

@tax_group_id int,  

@transtype VARCHAR(20),  

@currency_id int,  

@loss_currency_id int,  

@amount Money,  

@tax_currency_amount Money OUTPUT,  

@tax_loss_amount Money OUTPUT,  

@tax_base_amount Money OUTPUT,  

@claim_peril_id int,  

@claim_payment_id int = NULL,  

@claim_receipt_id int = NULL,  

@claim_payment_item_id int = NULL,  

@claim_receipt_item_id int = NULL,  
@calculate_only tinyint = 0  ,
@lReserveID INT=0,
@nGetSavedTaxOfPeril INT =0

AS  



DECLARE @tax_rate_is_value int  

DECLARE @tax_rate money  

DECLARE @tax_currency_id int  

DECLARE @effective_date datetime  

DECLARE @individual_tax_amount money  

DECLARE @tax_type_id int  

DECLARE @tax_band_id int  

DECLARE @tax_calculation_cnt int  

DECLARE @tax_sequence int  

DECLARE @tax_rate_allow_tax_credit int  

DECLARE @tax_rate_country_id int  

DECLARE @tax_rate_state_id int  

DECLARE @tax_rate_class_of_business_id int  

DECLARE @tmp_claim_receipt_id int  

DECLARE @total_value_tax money  

DECLARE @total_tax_rate float

DECLARE @payment_amount numeric(19,4) 

DECLARE @calc_basis float

DECLARE @is_Gross_Claim_Payment_Amount tinyint 
DECLARE @is_include_tax_in_instalments tinyint -- PN 77923 
DECLARE @SalvageAndTPRecoveryReservesExcludeTax INT  
DECLARE @EnumSalvageAndTPRecoveryReservesExcludeTax INT = 5067
 
If @nGetSavedTaxOfPeril>0 
BEGIN
		SELECT  TC.tax_group_id,
		TC.tax_band_id,
		currency.code,
		TC.percentage,
		TC.value,
		TC.is_value,
		TC.class_of_business_id,
		TC.sequence,
		0 is_manually_changed,
		tg.description,
		tb.description,
		tb.code 'band_code',
		tg.code 'tax_group_code'

		FROM tax_calculation  TC
		JOIN    currency        ON TC.currency_id = currency.currency_id
		JOIN    tax_group tg    ON tg.tax_group_id = TC.tax_group_id
		JOIN    tax_band tb     ON tb.tax_band_id = TC.tax_band_id
		WHere TC.claim_peril_id= @nGetSavedTaxOfPeril
		AND TC.claim_payment_id IN (Select max(claim_payment_id) from Claim_Payment Where claim_peril_id=@nGetSavedTaxOfPeril)
		--WE HAVE GOT SAVED INFORMATION SO RETURN
		RETURN

END

SELECT  @tax_currency_amount = 0,  
@tax_base_amount = 0,  
@tax_loss_amount = 0,  
@total_value_tax = 0,  
@total_tax_rate = 0,
@payment_amount = @amount  


IF ISNULL(@claim_receipt_id,0) <> 0 BEGIN
	SELECT @is_Gross_Claim_Payment_Amount=value  from System_Options WHERE option_number=5067 and branch_id=@company_id
	if ISNULL (@is_Gross_Claim_Payment_Amount,0)=0 
	select @is_Gross_Claim_Payment_Amount= 0
	else
	select @is_Gross_Claim_Payment_Amount= 1
END
ELSE
BEGIN
   SELECT @is_Gross_Claim_Payment_Amount=ISNULL(is_Gross_Claim_Payment_Amount,0) from product p join insurance_file ifi on p.product_id=ifi.product_id
  join claim c on ifi.insurance_file_cnt=c.policy_id
  join claim_peril cp on cp.claim_id=c.claim_id
  Where claim_peril_id=@claim_peril_id
END

SELECT @SalvageAndTPRecoveryReservesExcludeTax = ISNULL(value,0)
FROM System_Options 
WHERE option_number = @EnumSalvageAndTPRecoveryReservesExcludeTax


-- cannot calculate tax when there is no tax group so just exit...  
IF ISNULL(@tax_group_id, 0) =0  
RETURN  

-- get the effective date  
SELECT @effective_date = GetDate()  

IF @calculate_only = 1 BEGIN  
	-- create temporary table to hold tax calculation entries  
	CREATE TABLE #Tax_Calculation (  
	tax_calculation_cnt int IDENTITY (1, 1) NOT NULL,  
	claim_peril_id int NULL ,  
	claim_payment_id int NULL ,  
	claim_receipt_id int NULL ,  
	claim_payment_item_id int NULL ,  
	claim_receipt_item_id int NULL ,  
	tax_band_id int NULL ,  
	premium money NULL ,  
	percentage float NULL ,  
	value money NULL ,  
	is_value tinyint NOT NULL ,  
	currency_id smallint NULL ,  
	class_of_business_id int NULL ,  
	tax_group_id int NULL ,  
	sequence tinyint NULL ,  
	transtype varchar (10) NULL)  
END  

-- create temporary table to hold rates  
CREATE TABLE #Rates (  
tax_type_id int,  
description varchar(255),  
tax_band_id int,  
description1 varchar(255),  
is_value int,  
rate money,  
currency_id int,  
code varchar(10),  
sequence int,  
allow_tax_credit tinyint,  
country_id smallint ,  
state_id smallint,  
class_of_business_id int, 
calc_basis int,
tax_band_rate_id int,
is_suspended tinyint,  
is_include_tax_in_instalments tinyint
)      

-- save rates into temporary table  
INSERT INTO #Rates  
EXEC spu_Get_Tax_Types_and_Bands  
@tax_group_id =  @tax_group_id,  
@effective_date = @effective_date,  
@transtype = @transtype  

DECLARE CURSOR_Tax_Rates  CURSOR FAST_FORWARD FOR  

SELECT  tax_type_id,  

tax_band_id,  

is_value,  

rate,  

currency_id,  

sequence,  

allow_tax_credit,  

country_id,  

state_id,  
class_of_business_id, calc_basis,is_include_tax_in_instalments    

FROM #Rates  



-- If we are back calculating we have a bit of work to do...  

IF ISNULL(@claim_receipt_id,0) <> 0 BEGIN  

	-- ... basically we need to know what the net premium is as you cannot  

	-- reverse calculate a single tax from a group of non-compounded taxes.  

	-- So we do the following:  

	-- 1) Reduce the gross amount by the total of fixed rate taxes  

	-- 2) Reduce the remaining amount by reverse calculating on the SUM of all rate based tax  

	-- Once we've done this we can use the standard tax routine ro calculate the individual amounts.  



	-- We need a preliminary run through the cursor  

	OPEN CURSOR_Tax_Rates  

	FETCH NEXT FROM CURSOR_Tax_Rates INTO  

	@tax_type_id,  

	@tax_band_id,  

	@tax_rate_is_value,  

	@tax_rate,  

	@tax_currency_id,  

	@tax_sequence,  

	@tax_rate_allow_tax_credit,  

	@tax_rate_country_id,  

	@tax_rate_state_id,  
 @tax_rate_class_of_business_id, @calc_basis ,	@is_include_tax_in_instalments     



	-- We first need to work out the original net value then recalculate forwards!!  

	WHILE @@FETCH_STATUS = 0 BEGIN  

		-- if this is a value rather than a percentage  

		IF @tax_rate_is_value = 1 BEGIN  

			-- Get tax value in premium currency  

			EXEC spu_ACT_Do_Currency_To_Currency_Conversion  

			@currency_id_from =  @tax_currency_id,  

			@currency_amount_from = @tax_rate,  

			@company_id =  @company_id,  

			@currency_id_to=  @currency_id,  

			@currency_amount_to = @individual_tax_amount OUTPUT  



			-- Keep running total  

			SELECT  @total_value_tax = @total_value_tax + ISNULL(@individual_tax_amount, 0)  

		END ELSE BEGIN  

			-- Keep simple total of tax percentages  

			SELECT  @total_tax_rate = @total_tax_rate + ISNULL(@tax_rate, 0)  

		END  



		-- Next record  

		FETCH NEXT FROM CURSOR_Tax_Rates INTO  

		@tax_type_id,  

		@tax_band_id,  

		@tax_rate_is_value,  

		@tax_rate,  

		@tax_currency_id,  

		@tax_sequence,  

		@tax_rate_allow_tax_credit,  

		@tax_rate_country_id,  

		@tax_rate_state_id,  
  @tax_rate_class_of_business_id, @calc_basis  ,	@is_include_tax_in_instalments      

	END  



	-- Adjust GROSS amount, first by lump sum taxes, then rate based  

	  SELECT  @amount =Convert(float, @amount) /(1+ @total_tax_rate / 100) -- convert for max.precision  

	  SELECT  @amount = @amount - @total_value_tax  



	-- Close it but don't deallocate  

	CLOSE CURSOR_Tax_Rates  

END  



-- Open cursor for calculations  

OPEN CURSOR_Tax_Rates  

FETCH NEXT FROM CURSOR_Tax_Rates INTO  

@tax_type_id,  

@tax_band_id,  

@tax_rate_is_value,  

@tax_rate,  

@tax_currency_id,  

@tax_sequence,  

@tax_rate_allow_tax_credit,  

@tax_rate_country_id,  

@tax_rate_state_id,  
@tax_rate_class_of_business_id, @calc_basis   ,	@is_include_tax_in_instalments   



-- Now process normally  

WHILE @@FETCH_STATUS = 0 BEGIN  

	-- if this is a value rather than a percentage  

	IF @tax_rate_is_value = 1 BEGIN  

		-- the routine needs to convert the tax value  

		-- into the same currency as the premium  

		-- before it is added to the total tax amount  

		EXEC spu_ACT_Do_Currency_To_Currency_Conversion  

		@currency_id_from =  @tax_currency_id,  

		@currency_amount_from = @tax_rate,  

		@company_id =  @company_id,  

		@currency_id_to=  @currency_id,  

		@currency_amount_to = @individual_tax_amount OUTPUT  

	END ELSE BEGIN  

		-- the tax amount is simply a percentage of the passed in amount  
		--@SalvageAndTPRecoveryReservesExcludeTax should only have an effect on Salavage and TP Receovery Transactions
		IF ISNULL(@claim_receipt_id,0) <> 0 BEGIN
			IF @SalvageAndTPRecoveryReservesExcludeTax = 1 AND ISNULL(@claim_payment_id,0) = 0 BEGIN

			SET @individual_tax_amount = @payment_amount * (@tax_rate/100)  

		END ELSE BEGIN  

			 SET @individual_tax_amount = @payment_amount / (1 + @tax_rate/100)  

			 SET @individual_tax_amount = @individual_tax_amount * ( @tax_rate/100)  

		END

	END  

		 -- @is_Gross_Claim_Payment_Amount should only have an effect on Claim Payment Transactions
		IF ISNULL(@claim_receipt_id,0) = 0 BEGIN
			 IF ISNULL(@is_Gross_Claim_Payment_Amount,0)=0 BEGIN 
				SET @individual_tax_amount = @payment_amount * (@tax_rate/100)  
			END ELSE BEGIN  
				 SET @individual_tax_amount = @payment_amount / (1 + @tax_rate/100)  
				 SET @individual_tax_amount = @individual_tax_amount * ( @tax_rate/100)  
			END
		END
	END  


	SET @tax_currency_amount = @tax_currency_amount + @individual_tax_amount  



	-- if there is a claim receipt id and a claim payment id  

	-- then is actually a gross claim payment so dont actually save a claim receipt id  

	-- against the tax calculation item just set it to null  

	IF ISNULL(@claim_receipt_id,0) <> 0  

	AND ISNULL(@claim_payment_id,0) <> 0 BEGIN  

		SET @tmp_claim_receipt_id = NULL  

	END ELSE BEGIN  

		SET @tmp_claim_receipt_id = @claim_receipt_id  

	END  



	-- UI can request to just get a tax calculation without  

	-- writing back to the database  

	IF @individual_tax_amount <> 0 BEGIN  

		IF (@calculate_only = 0) BEGIN  

			INSERT INTO tax_calculation (  

			claim_peril_id,  

			claim_payment_id,  

			claim_receipt_id,  

			claim_payment_item_id,  

			claim_receipt_item_id,  

			tax_band_id,  

			premium,  

			percentage,  

			value,  

			is_value,  

			currency_id,  

			class_of_business_id,  

			tax_group_id,  

			sequence,  
			transtype,	include_tax_in_instalments  )  

			VALUES (@claim_peril_id,  

			@claim_payment_id,  

			@tmp_claim_receipt_id,  

			@claim_payment_item_id,  

			@claim_receipt_item_id,  

			@tax_band_id,  

			@amount,  

			CASE WHEN @tax_rate_is_value <> 1 THEN @tax_rate ELSE 0 END,  

			@individual_tax_amount,  

			@tax_rate_is_value,  

			@currency_id,  

			@tax_rate_class_of_business_id,  

			@tax_group_id,  

			@tax_sequence,  
			@transtype,
			@is_include_tax_in_instalments)  

		END ELSE BEGIN  

			INSERT INTO #tax_calculation (  

			claim_peril_id,  

			claim_payment_id,  

			claim_receipt_id,  

			claim_payment_item_id,  

			claim_receipt_item_id,  

			tax_band_id,  

			premium,  

			percentage,  

			value,  

			is_value,  

			currency_id,  

			class_of_business_id,  

			tax_group_id,  

			sequence,  
			transtype
			 )  

			VALUES (@claim_peril_id,  

			@claim_payment_id,  

			@tmp_claim_receipt_id,  

			@claim_payment_item_id,  

			@claim_receipt_item_id,  

			@tax_band_id,  

			@amount,  

			CASE WHEN @tax_rate_is_value = 0 THEN @tax_rate ELSE 0 END,  

			@individual_tax_amount,  

			@tax_rate_is_value,  

			@currency_id,  

			@tax_rate_class_of_business_id,  

			@tax_group_id,  

			@tax_sequence,  
			@transtype
			)  

		END  

	END  



	FETCH NEXT FROM CURSOR_Tax_Rates INTO  

	@tax_type_id,  

	@tax_band_id,  

	@tax_rate_is_value,  

	@tax_rate,  

	@tax_currency_id,  

	@tax_sequence,  

	@tax_rate_allow_tax_credit,  

	@tax_rate_country_id,  

	@tax_rate_state_id,  
 @tax_rate_class_of_business_id, @calc_basis  ,  	@is_include_tax_in_instalments  

END  



CLOSE CURSOR_Tax_Rates  

DEALLOCATE CURSOR_Tax_Rates  



DROP TABLE #Rates  



-- get the tax details in the base amount  

EXEC spu_ACT_Do_Currency_Conversion  

@company_id = @company_id,  

@currency_id = @currency_id,  

@currency_amount_unrounded = @tax_currency_amount,  

@currency_base_date = @effective_date,  

@mode ='1',  

@base_amount_unrounded = @tax_base_amount OUTPUT,  

@return_status = 1  



-- get the tax details in the loss currency  

EXEC spu_ACT_Do_Currency_To_Currency_Conversion  

@currency_id_from =  @currency_id,  

@currency_amount_from = @tax_currency_amount,  

@company_id =  @company_id,  

@currency_id_to=  @loss_currency_id,  

@currency_amount_to = @tax_loss_amount OUTPUT  



IF @calculate_only = 1 BEGIN  

	SELECT  wtc.tax_group_id,  

	wtc.tax_band_id,  

	currency.code,  

	wtc.percentage,  

	wtc.value,  

	wtc.is_value,  

	wtc.class_of_business_id,  

	wtc.sequence,  

	0 is_manually_changed,  

	tg.description,  

	tb.description,

    tb.code 'band_code',

    tg.code 'tax_group_code'

	FROM #tax_calculation  wtc  

	JOIN    currency        ON wtc.currency_id = currency.currency_id  

	JOIN    tax_group tg    ON tg.tax_group_id = wtc.tax_group_id  

	JOIN    tax_band tb     ON tb.tax_band_id = wtc.tax_band_id  

	If @lReserveID>0 
	BEGIN
	execute spu_CLM_Clear_TaxBandInfo @lReserveID

	Insert into tblTaxBandInfo(ReserveID,TaxBandID,Rate,IsValue,ClassOfBusinessID,TaxAmount)
	SELECT  @lReserveID,		
	wtc.tax_band_id,
	wtc.percentage,
	wtc.is_value,
	wtc.class_of_business_id,
	wtc.value
	FROM #tax_calculation  wtc
	END
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


