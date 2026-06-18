SET QUOTED_IDENTIFIER OFF 
GO

EXECUTE DDLDropProcedure 'spu_update_ClaimPayment_details'
GO
CREATE  PROCEDURE spu_update_ClaimPayment_details
	@ReserveID int,  
    @This_Payment Currency,
    @Payee_Type INT,
    @Payee_Short_code varchar(100),
    @Media_Type varchar(10),
    @Party_Bank_Id INT,
	@Tax_Group_Code varchar(10),
	@sMedia_Ref  varchar(30)=NULL,
	@nClaimPaymentToID INT=NULL,
	@bOverrideTax TINYINT=0,
	@bIsGrossClaimPaymentAmount TINYINT=0,
	@o_nClaimPaymentID INT OUTPUT,
	@o_crTotalPayment Currency OUTPUT,
	@o_crTotalTax Currency OUTPUT,
	@nIsExGratia TINYINT=0,
	@nUserId INT = 0,
	@CreatedBy INT = NULL
	
AS    
BEGIN


DECLARE @lTaxGroupID INT  
DECLARE @lMediaTypeID INT  
DECLARE @nPartyCnt INT  
DECLARE @lClaim_payment_id INT  
DECLARE @lClaim_peril_id INT  
DECLARE @lClaim_ID INT  
DECLARE @lbase_currency_id INT  
DECLARE @lAccountID INT  
DECLARE @lCountryID INT  
declare @lSourceID INT  
DECLARE @dCurrency_base_xrate float  
DECLARE @dtCurrency_base_date datetime  
DECLARE @dAccount_base_xrate float  
DECLARE @dtAccount_base_date datetime  
DECLARE @dSystem_base_xrate float  
DECLARE @dtSystem_base_date datetime  
DECLARE @dPercentage FLOAT  
DECLARE @nCOB INT  
DECLARE @lTax_Band_ID int  
DECLARE @nTax_calc_cnt INT  
DECLARE @bIsWithHoldingTax TINYINT
DECLARE @crTax_amount_WHT CURRENCY
DECLARE @crTax_amount CURRENCY
Declare @dtCurrentDateTime as DateTime
Declare @sPayeename as Varchar(255)

set @dtCurrentDateTime= GetDate()

    DECLARE @tax_base_amount FLOAT, @tax_currency_amount Float, @tax_loss_amount Float

IF @Tax_Group_Code<>''  
	SELECT @lTaxGroupID = Tax_Group_ID,@bIsWithHoldingTax= ISNULL(is_withholding_tax,0) FROM tax_group WHERE Code= @Tax_Group_Code  

    IF @Media_Type IS NOT NULL 
	Select @lMediaTypeID = MediaType_Id from MediaType WHERE Code=@Media_Type  
    
IF @Payee_Type = 3      -- i.e. Client  
        SET @Payee_Type = 8

IF @Payee_Type = 4      --i.e. Agent  
        SET @Payee_Type = 4

    IF @Payee_Short_code<>''
	BEGIN
		SELECT @nPartyCnt = Party_Cnt FROM PARTY Where ShortName=@Payee_Short_code		 
		EXEC spu_ACT_Get_AccountID_From_partyCnt @companyid = @lSourceID, @PartyCnt = @nPartyCnt, @AccountID = @lAccountID output, @SubBranchID = 1  
	END
ELSE
	BEGIN
		SELECT @nPartyCnt=NULL,@lAccountID= account_id FROM  account where short_code ='CLMPAYABLE'
	END


    IF @Party_Bank_Id =0 
        Set @Party_Bank_Id = NULL

SELECT @lClaim_peril_id = Claim_Peril_id FROM Reserve WHERE Reserve_Id = @ReserveID  
SELECT @lClaim_payment_id = Claim_payment_id FROM Claim_payment WHERE Claim_Peril_id=@lClaim_peril_id  
Select @lClaim_ID =Claim_Id FROM Claim_Peril WHERE Claim_Peril_ID=@lClaim_peril_id  

SELECT   @lbase_currency_id = s.base_currency_id,@lCountryID= s.Country_id,@lSourceID=s.Source_ID
    FROM source s  
        JOIN insurance_file i  
        ON i.source_id = s.source_id  
        JOIN claim c  
        ON c.policy_id = i.insurance_file_cnt  
WHERE c.claim_id = @lClaim_ID  



    UPDATE Claim  
SET Last_modified_date = @dtCurrentDateTime 
        WHERE Claim_id = (SELECT DISTINCT Claim_id From Claim_Peril  
        Where Claim_Peril_id IN  
        (SELECT distinct Claim_Peril_id  
        FROM Reserve  
WHERE  reserve_id = @ReserveID ))  
    
--DECLARE @lClaim_payment_id INT  
--  Do we have a Entry in Claim_Payment if No Create a Entry

IF NOT EXISTS(SELECT 1 FROM CLAIM_PAYMENT WHERE Claim_Peril_id =@lClaim_peril_id and claim_payment_id=base_claim_payment_id)  
    BEGIN
		EXEC spu_CLM_Claim_Payment_Add  @Claim_payment_id = @lClaim_payment_id output,  
		@Claim_ID = @lClaim_ID, @Claim_peril_id = @lClaim_peril_id, @date_of_payment = @dtCurrentDateTime,  
		@amount = 0, @tax_amount = $0.0000, @tax_amount_wht = 0, @Party_Cnt = @nPartyCnt,  
		@comments = NULL, @is_referred = NULL, @created_by = @CreatedBy, @payeemediatype = NULL, @payeename = NULL, @payeebankname = NULL,@payment_party_to = @Payee_Type,  
         @payeesortcode = NULL, @payeeaccountno = NULL, @payeecomments = NULL, @sequenceno = NULL,@treaty_id = NULL, @claim_payment_to_id = NULL, 
         @insured_domiciled = NULL, @insured_percentage = NULL, @insured_tax_number = NULL, @payee_domiciled = 0, @payee_percentage = NULL, @payee_tax_number = NULL, @safe_harbour_id = NULL, @safe_harbour_percentage = NULL, 
         @is_tax_exempt = 0, @is_wht_exempt = 0, @is_settlement = 0, @document_id = NULL, @media_ref = NULL, 
		@currency_id = @lbase_currency_id, @excess_amount = $0.0000,  
         @PayeeAddress1 = NULL, @PayeeAddress2 = NULL, @PayeeAddress3 = NULL, @PayeeAddress4 = NULL, 
         @PayeePostalCode = '__', @payeecountry = NULL, @ThirdPartyReference = NULL, 
		@cheque_date = @dtCurrentDateTime, @Party_Bank_ID = @Party_Bank_Id  
    END
    ELSE
    BEGIN
		SELECT  @lClaim_payment_id=Claim_Payment_ID FROM CLAIM_PAYMENT WHERE Claim_Peril_id =@lClaim_peril_id  and  claim_payment_id=base_claim_payment_id 
    END

CREATE TABLE #TemptblTaxBandInfo
(ID INT IDENTITY, ReserveID INT,  TaxBandID INT,  Rate FLOAT,  IsValue TINYINT,  ClassOfBusinessID INT, TaxAmount MONEY)

If @bOverrideTax=0 
BEGIN
          EXEC spu_CLM_Calculate_Tax_Amounts 
	@company_id = @lSourceID, @Tax_Group_ID = @lTaxGroupID, @transtype = 'TTCP', @currency_id = @lbase_currency_id,  
	@loss_currency_id = @lbase_currency_id, @amount = @This_Payment, @Claim_peril_id = @lClaim_peril_id, @calculate_only = 1,
	@lReserveID = @ReserveID,  
				@tax_currency_amount = @tax_currency_amount output,
				@tax_loss_amount = @tax_loss_amount output, @tax_base_amount = @tax_base_amount output
END


Insert into #TemptblTaxBandInfo(ReserveID,TaxBandID,Rate,IsValue,ClassOfBusinessID,TaxAmount)
Select ReserveID,TaxBandID,Rate,IsValue,NULLIF(ClassOfBusinessID,0),TaxAmount from tblTaxBandInfo WITH(NOLOCK) WHERE ReserveID=@ReserveID

Select @tax_base_amount =ISNULL(Sum(TaxAmount),0) from  #TemptblTaxBandInfo

IF ISNULL(@bIsGrossClaimPaymentAmount,0)=1
SET @This_Payment=@This_Payment - @tax_base_amount  


IF @bIsWithHoldingTax=1
SELECT @crTax_amount_WHT=@tax_base_amount,@crTax_amount=0.00
ELSE
SELECT @crTax_amount_WHT=0.00,@crTax_amount=@tax_base_amount
 
        EXEC spu_ACT_Do_Currency_Conversion  
@Account_ID = @lAccountID, @company_id = @lSourceID, @currency_id = @lbase_currency_id,  
            @currency_amount_unrounded = @This_Payment, @mode = 'ALL', 
@Currency_base_xrate = @dCurrency_base_xrate output, @Currency_base_date = @dtCurrency_base_date output,  
@Account_base_xrate = @dAccount_base_xrate output, @Account_base_date = @dtAccount_base_date output, @System_base_xrate = @dSystem_base_xrate output,  
@System_base_date = @dtSystem_base_date output,@return_status =1  

Select  @dCurrency_base_xrate, @dtCurrency_base_date, @dAccount_base_xrate, @dtAccount_base_date, @dSystem_base_xrate, @dtSystem_base_date  

DECLARE @Claim_Payment_Item_ID INT  

SELECT @Claim_Payment_Item_ID = claim_payment_item_id FROM Claim_Payment_Item WHERE Claim_Payment_ID = @lClaim_payment_id AND Reserve_ID=@ReserveID

Declare @crtax_value currency
Declare @bisValue TINYINT
Declare @nTaxBands INT
Declare @ID INT
select @ID=1,@nTaxBands=max(ID) from #TemptblTaxBandInfo

IF @Claim_Payment_Item_ID IS NULL
        BEGIN
            EXEC spu_CLM_Claim_Payment_Item_Add @claim_payment_item_id = @Claim_Payment_Item_ID output, 
		@Claim_payment_id = @lClaim_payment_id, @reserve_id = @ReserveID,  
                @recovery_id = NULL, @recovery_type_id = NULL,
		@currency_id = @lbase_currency_id, @Tax_Group_ID = @lTaxGroupID, @This_Payment = @This_Payment,  
		@tax_amount = @crTax_amount, @tax_amount_wht = @crTax_amount_WHT,  
                @exchange_rate_override_reason_id = 0, 
		@Currency_base_xrate = @dCurrency_base_xrate, @Currency_base_date = @dtCurrency_base_date,  
		@Account_base_xrate = @dAccount_base_xrate, @Account_base_date = @dtAccount_base_date,  
		@System_base_xrate =   @dSystem_base_xrate, @System_base_date = @dtSystem_base_date,  
		@payment_loss_xrate = 1.0, @payment_adjustment = 0.0000  
		

		WHILE(@ID<=@nTaxBands)
                BEGIN
		

			Select @lTax_Band_ID= TaxBandID,
				@dPercentage=rate,
				@crtax_value=TaxAmount,
				@bisValue=IsValue,
				@nCOB=ClassOfBusinessID		
			 from #TemptblTaxBandInfo where ID=@ID
		
			EXEC spu_CLM_Tax_Calculation_Add @tax_calculation_cnt = @nTax_calc_cnt output,  
			@Claim_peril_id = @lClaim_peril_id, @Claim_payment_id = @lClaim_payment_id, @claim_receipt_id = NULL, @claim_payment_item_id = @Claim_Payment_Item_ID,  
			@claim_receipt_item_id = NULL, @Tax_Band_ID = @lTax_Band_ID, @premium = @This_Payment , @Percentage = @dPercentage,  
			@value = @crtax_value, @is_value =@bisValue, @currency_id = @lbase_currency_id,  
			@class_of_business_id = @nCOB, @Tax_Group_ID = @lTaxGroupID, @sequence = 1, @transtype = 'TTCP', @is_manually_changed = 0  

			SET @ID=@ID+1
		END  
                END  
ELSE  
	BEGIN  
		WHILE (@ID <= @nTaxBands)
				BEGIN
					SELECT @lTax_Band_ID = TaxBandID
						,@dPercentage = rate
						,@crtax_value = TaxAmount
						,@bisValue = IsValue
						,@nCOB = ClassOfBusinessID
					FROM #TemptblTaxBandInfo
					WHERE ID = @ID

					IF NOT EXISTS (
					SELECT 1
					FROM Tax_Calculation
					WHERE claim_payment_item_id = @Claim_Payment_Item_ID
						AND claim_payment_id = @lClaim_payment_id
						And tax_band_id=@lTax_Band_ID
					)
					BEGIN
					EXEC spu_CLM_Tax_Calculation_Add @tax_calculation_cnt = @nTax_calc_cnt OUTPUT
						,@Claim_peril_id = @lClaim_peril_id
						,@Claim_payment_id = @lClaim_payment_id
						,@claim_receipt_id = NULL
						,@claim_payment_item_id = @Claim_Payment_Item_ID
						,@claim_receipt_item_id = NULL
						,@Tax_Band_ID = @lTax_Band_ID
						,@premium = @This_Payment
						,@Percentage = @dPercentage
						,@value = @crtax_value
						,@is_value = @bisValue
						,@currency_id = @lbase_currency_id
						,@class_of_business_id = @nCOB
						,@Tax_Group_ID = @lTaxGroupID
						,@sequence = 1
						,@transtype = 'TTCP'
						,@is_manually_changed = 0
					END
					SET @ID = @ID + 1			
			END
		 UPDATE  TC
		 SET TC.percentage=IT.Rate,
		 TC.value=IT.TaxAmount,
		 TC.is_value=IT.IsValue,
		 TC.class_of_business_id=IT.ClassOfBusinessID,
		 TC.premium=  @This_Payment
		 FROM Tax_Calculation TC
		 JOIN #TemptblTaxBandInfo IT
		 ON TC.tax_band_id=IT.TaxBandID
		 WHERE TC.claim_payment_item_id=@Claim_Payment_Item_ID
		 AND IT.ReserveID=@ReserveID
  
        END

--IN CASE WE HAVE SET THIS PAYMENT VIA RULES PREVIOUSLY THEN  REVERSE THAT CALCULATION
IF @This_Payment<>0
        BEGIN
	Update Reserve 
	SET Paid_to_date= Paid_to_date-this_payment,
	this_payment=0,
	paid_to_date_tax = ISNULL(paid_to_date_tax,0) + ISNULL(@crTax_amount,0)
	WHERE Reserve_id=@ReserveID
         END

EXEC spu_CLM_Claim_Payment_Item_Reserve_Update @reserve_id = @ReserveID, @this_revision = 0, @This_Payment = @This_Payment  
	
UPDATE Claim_Payment_Item Set this_payment = @This_Payment,tax_amount_wht=@crTax_amount_WHT,tax_amount=@crTax_amount ,tax_group_id=@lTaxGroupID
WHERE  Reserve_Id = @ReserveID  and claim_payment_id=@lClaim_payment_id
        
            UPDATE Claim_Payment SET Amount =
(SELECT Sum(This_payment) From Claim_Payment_item WHERE Claim_payment_id = @lClaim_payment_id),  
Tax_amount=(SELECT Sum(Tax_amount) From Claim_Payment_item WHERE Claim_payment_id = @lClaim_payment_id),  
Tax_amount_wht=(SELECT Sum(Tax_amount_Wht) From Claim_Payment_item WHERE Claim_payment_id = @lClaim_payment_id),  
                Party_Bank_Id = @Party_Bank_Id,
Party_Cnt=@nPartyCnt,  
PayeeMediaType=@lMediaTypeID,
media_ref=@sMedia_Ref,
PayeeName= @sPayeename,
is_ex_gratia = @nIsExGratia,
claim_payment_to_id= @nClaimPaymentToID,
Payment_Party_to=@Payee_Type
WHERE Claim_payment_id= @lClaim_payment_id  
        
SELECT @o_nClaimPaymentID=Claim_payment_id,
 @o_crTotalPayment=Amount,
 @o_crTotalTax=Tax_amount +Tax_amount_wht
 FROM Claim_Payment WITH(NOLOCK)
 WHERE Claim_payment_id= @lClaim_payment_id  

 DROP TABLE #TemptblTaxBandInfo
 EXECUTE spu_CLM_Clear_TaxBandInfo @ReserveID
END  
GO

