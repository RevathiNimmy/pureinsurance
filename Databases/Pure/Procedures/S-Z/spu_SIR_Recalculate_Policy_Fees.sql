EXECUTE DDLDropProcedure 'spu_SIR_Recalculate_Policy_Fees'
GO

CREATE PROCEDURE spu_SIR_Recalculate_Policy_Fees
    @Transaction_type_id INT,
    @product_id INT,
    @insurance_file_cnt INT,
    @use_existing_fee_details INT = 0,
    @sMakeliveoptions VARCHAR(20)= NULL,
    @sPaymentDebitOrCash VARCHAR(20)= NULL,
    @sTransactionType  VARCHAR(10)=NULL,
    @nViaSAM INT = 0,
	@Is_Backdated_MTA INT = 0
AS

    -- ************************************************
    -- NB: FEE_AMOUNT IS JUST THE VALUE CARRIED THROUGH
    -- FROM THE FEE_AMOUNTS CONFIG TABLE, THE
    -- CURRENCY_AMOUNT IS THE ACTUAL FEE AMOUNT
    -- ************************************************
    DECLARE @base_currency_id SMALLINT
    DECLARE @currency_id SMALLINT
    DECLARE @currency_base_xrate FLOAT
    DECLARE @source_id INT
    DECLARE @TypeOfRates TINYINT
    DECLARE @effective_fee_date DATETIME
    DECLARE @premium MONEY
    DECLARE @currency_desc VARCHAR(255)
    DECLARE @currency_isocode varchar(10)
    DECLARE @transaction_sub_type_id int
    DECLARE @risk_cnt int
    DECLARE @temp_product_id int
    DECLARE @sMakeliveoptions_id int
    DECLARE @sPaymentDebitOrCash_id int
    DECLARE @proratarate NUMERIC(19,8)
	DECLARE @BaseInsuranceFileCnt INT=0
    DECLARE @insuranceFileType int
    DECLARE @Party_cnt int
	DECLARE @Is_Fee_Updated TINYINT =0 

    SET @risk_cnt = NULL
    SELECT @sMakeliveoptions_id =NULL
    SELECT @sPaymentDebitOrCash_id =NULL

    -- policy discount / loading requires that any manual (user led) changes
    -- to fees ( e.g edits / deletes ) are retained when a discount is applied
    -- and rolled out. For this reason we use the existing fee detail but
    -- recalculate the fee amount and tax amount as the underlying premiums will
    -- have changed

       If Exists (Select Null From mta_insurance_file_link Where new_linked_insurance_file_cnt = @insurance_file_cnt or  cancelled_linked_insurance_file_cnt = @insurance_file_cnt)
              Select @use_existing_fee_details = 1

       If @use_existing_fee_details <> 1 Begin
              DECLARE @ExistingFee TABLE
              (
              party_cnt INT,
              fee_rate_percentage NUMERIC(7, 4),
              fee_rate_amount NUMERIC(19, 4),
              transaction_type_id INT,
              tax_group_id INT,
              transaction_sub_type INT,
              fee_rate_currency_id SMALLINT,
              MakeLiveOptions_id INT,
              DoPaymentTerms_id INT,
              Calculation_Basis TINYINT,
              Is_Prorated TINYINT,
              Is_Override TINYINT,
              Is_Percent  TINYINT
              )

              -- Keep all existing fee lines before deleting
         INSERT INTO @ExistingFee (
              party_cnt,
              fee_rate_percentage,
              fee_rate_amount,
              transaction_type_id,
              tax_group_id,
              transaction_sub_type,
              fee_rate_currency_id,
              MakeLiveOptions_id,
              DoPaymentTerms_id,
              Calculation_Basis,
              Is_Prorated,
              Is_Override,
              Is_Percent)
       SELECT       party_cnt,
              fee_rate_percentage,
              fee_rate_amount,
              transaction_type_id,
              tax_group_id,
              transaction_sub_type,
              fee_rate_currency_id,
              MakeLiveOptions_id,
              DoPaymentTerms_id,
              Calculation_Basis,
              Is_Prorated,
              Is_Override,
              FeeTypePercent
       FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt IS NULL
       End

    SELECT @sPaymentDebitOrCash_id = 0
    IF ISNULL(@sPaymentDebitOrCash,'')<>''
    BEGIN
              SELECT @sPaymentDebitOrCash_id=DOPaymentTerms_id  FROM DOPaymentTerms  WHERE Code=RTRIM(LTRIM(@sPaymentDebitOrCash ))
    END

    IF @nViaSAM = 1
    BEGIN

    SELECT	@insuranceFileType = insurance_file_type_id ,
                     @product_id = product_id ,
                     @sMakeliveoptions=
                      CASE WHEN UPPER(payment_method)='INVOICE'  THEN 'INVOICE'
						  WHEN UPPER(payment_method)='INSTALMENTS'  THEN 'INST'
						  WHEN UPPER(payment_method)='PREMIUMFINANCE'  THEN 'INST'
						  WHEN UPPER(payment_method)='DIRECT DEBIT'  THEN 'INST'
						  WHEN UPPER(payment_method)='CREDIT CARD'  THEN 'INST'
						  WHEN UPPER(payment_method)='BANK GUARANTEE'  THEN 'BG'
						  WHEN UPPER(payment_method)='PAYNOW'  THEN 'PAYNOW'
						  WHEN UPPER(payment_method)='CASH DEPOSIT'  THEN 'CD'
						  WHEN UPPER(payment_method)='MARK FOR COLLECTION'  THEN 'MARKED'
					 END,
                     @sPaymentDebitOrCash_id = CASE WHEN ISNULL(@sPaymentDebitOrCash,'')<>'' THEN @sPaymentDebitOrCash_id
                                                 ELSE ISNULL(DOPaymentTerms_id,0)
									END,
			@BaseInsuranceFileCnt =base_insurance_file_cnt,
			@effective_Fee_date=cover_start_date
                     FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt

    SELECT @transaction_type_id =
    CASE WHEN @insuranceFileType IN (1,2) THEN 4
              WHEN @insuranceFileType IN (4,5,6,7) THEN 9
              WHEN @insuranceFileType IN (8,11,12) THEN 7
              WHEN @insuranceFileType IN (9,10) THEN 20
              WHEN @insuranceFileType IN (3)    THEN 10
       END

    SELECT @sTransactionType = Code FROM Transaction_Type WHERE transaction_type_id = @transaction_type_id
    END

    IF @transaction_type_id = 10 AND ISNULL(@sMakeliveoptions,'')=''
    BEGIN
       SELECT @sPaymentDebitOrCash_id=DOPaymentTerms_id ,
							   @sMakeliveoptions =   
								CASE	WHEN UPPER(payment_method)='INVOICE'  THEN 'INVOICE'  
										WHEN UPPER(payment_method)='INSTALMENTS'  THEN 'INST'  
										WHEN UPPER(payment_method)='PREMIUMFINANCE'  THEN 'INST' 
										WHEN UPPER(payment_method)='DIRECT DEBIT'  THEN 'INST'
										WHEN UPPER(payment_method)='CREDIT CARD'  THEN 'INST' 
										WHEN UPPER(payment_method)='BANK GUARANTEE'  THEN 'BG'  
										WHEN UPPER(payment_method)='PAYNOW'  THEN 'PAYNOW'  
										WHEN UPPER(payment_method)='CASH DEPOSIT'  THEN 'CD'  
										WHEN UPPER(payment_method)='MARK FOR COLLECTION'  THEN 'MARKED'  
								  END   
			    FROM insurance_file
				WHERE insurance_file_cnt = @insurance_file_cnt
    END

    SELECT @sMakeliveoptions_id = 0
    IF ISNULL(@sMakeliveoptions,'')<>''
    BEGIN
              SELECT @sMakeliveoptions_id=MakeLiveOptions_ID FROM MakeLiveOptions WHERE Code=@sMakeliveoptions
    END

    IF @use_existing_fee_details <> 1
    BEGIN

    --Get policy details
    SELECT  @effective_Fee_date = i.cover_start_date,
            @base_currency_id = s.base_currency_id,
            @currency_id = i.currency_id,
            @currency_base_xrate = i.currency_base_xrate,
            @source_id = i.source_id,
            @premium = ISNull(i.this_premium, 0),
            @currency_desc = c.description,
            @currency_isocode = c.iso_code,
            @temp_product_id = i.product_id
    FROM    insurance_file i
    JOIN    source s
            ON s.source_id = i.source_id
    JOIN    currency c
            ON c.currency_id = i.currency_id
    WHERE   i.insurance_file_cnt = @insurance_file_cnt

    -- set transaction sub_type so that by default it is ignored by the main select
    SET @transaction_sub_type_id = 0

    -- if this is an MTA with a positive premium this is transaction_sub_type : Additional MTA
    IF @premium >= 0 AND @transaction_type_id = 9
              SET @transaction_sub_type_id = 1

    -- if this is an MTA with a negative premium this is transaction_sub_type : Return MTA
    IF @premium < 0 AND @transaction_type_id = 9
        SET @transaction_sub_type_id = 2

    IF @product_id < 0
        SET @product_id = @temp_product_id

    -- Get source_id where rates are stored
    EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT
    IF @TypeOfRates = 1
        SELECT @source_id = 1

    EXEC spu_get_policy_pro_rata_rate @nInsuranceFileCnt = @insurance_file_cnt,@sTransactionType=@sTransactionType, @crproratarate =  @proratarate OUTPUT
    IF ISNULL(@proratarate,0) =0
        SELECT @proratarate = 1

    -- clear down tax calculations before policy fee records
    IF NOT EXISTS(SELECT NULL from insurance_file WHERE insurance_file_cnt=@insurance_file_cnt and out_of_sequence_replaced=1)
    BEGIN
                 DELETE  tax_calculation
                 WHERE   policy_fee_u_id IN (SELECT  policy_fee_u_id
                                                                     FROM    policy_fee_u
                                                                     WHERE Insurance_file_cnt = @insurance_file_cnt and risk_cnt is null)

    -- clear down any entries that have already been calculated
                 Delete policy_fee_u
                 WHERE Insurance_file_cnt = @insurance_file_cnt and risk_cnt is null
    END
    ELSE
    BEGIN

          DELETE  tax_calculation
                 WHERE   policy_fee_u_id IN (SELECT  policy_fee_u_id
                                                                     FROM    policy_fee_u
                                                                     WHERE Insurance_file_cnt = @insurance_file_cnt and risk_cnt is null And (DoPaymentTerms_id<>@sPaymentDebitOrCash_id AND DoPaymentTerms_id IS NOT NULL))

    -- clear down any entries that have already been calculated
                 Delete policy_fee_u
                 WHERE Insurance_file_cnt = @insurance_file_cnt and risk_cnt is null AND (DoPaymentTerms_id<>@sPaymentDebitOrCash_id AND DoPaymentTerms_id IS NOT NULL)

    END
    --********************************************************
    -- NB. When no currency is specified against the fee
    -- then the fee automatically adopts the same currency id
    -- as the insurance_file
    --********************************************************

 -- If we have any risk selected then Fee entry goes to Fee table else not
 IF EXISTS (Select Top 1 p.risk_cnt
  from Peril p,
  Risk r,
  insurance_file_risk_link ifr
  Where ifr.Insurance_file_cnt = @insurance_file_cnt
  And ifr.status_flag NOT IN ('U','R')
  And r.risk_cnt = ifr.risk_cnt
  And p.risk_cnt = r.risk_cnt
  And IsNull(p.is_levy_tax,0)=0
  And r.is_risk_selected=1
  group by p.risk_cnt)
 BEGIN

If Not Exists (Select Null From policy_fee_u Where insurance_file_cnt = @insurance_file_cnt and risk_cnt IS NULL)
BEGIN
   DECLARE
    @last_insurance_file_cnt int,
    @insurance_folder_cnt int,
	@initial_anniversary_date datetime,
	@insurance_ref varchar(100), @an_new datetime

   --Get the insurance_folder_cnt
   SELECT @insurance_folder_cnt=insurance_folder_cnt FROM Insurance_File
   WHERE insurance_file_cnt=@insurance_file_cnt

   SELECT top 1 @initial_anniversary_date=anniversary_date, @insurance_ref = insurance_ref, @an_new = anniversary_date FROM Insurance_File
   WHERE insurance_ref=(select insurance_ref from Insurance_File where insurance_file_cnt = @insurance_file_cnt)

   --GET the last insurance_file_cnt
  IF @transaction_type_id = 10 
 BEGIN
 SELECT TOP 1 @last_insurance_file_cnt=insurance_file_cnt
       FROM Insurance_File
   WHERE insurance_file_type_id in (2, 5, 8, 9)
   AND  insurance_folder_cnt=@insurance_folder_cnt
   ORDER BY insurance_file_cnt DESC
   END
   Else
   BEGIN
     SELECT TOP 1 @last_insurance_file_cnt=insurance_file_cnt
       FROM Insurance_File
   WHERE (insurance_file_type_id in (2, 5, 8, 9)
                     OR (insurance_file_type_id = 4 AND ISNULL(insurance_file_status_id, 0) <> 1)) -- OOS will require looking at previous quote
   AND  insurance_folder_cnt=@insurance_folder_cnt
   AND  cover_start_date <= (Select cover_start_date From insurance_file Where insurance_file_cnt=@insurance_file_cnt)
   AND insurance_file_cnt < case when anniversary_date > @initial_anniversary_date Then 
						(select top 1 insurance_file_cnt from  insurance_file where insurance_ref = @insurance_ref and anniversary_date =@initial_anniversary_date order by cover_start_date desc) else @insurance_file_cnt end
   ORDER BY cover_start_date DESC,insurance_file_cnt DESC
   END
 
 

END;
ELSE
BEGIN
       UPDATE policy_fee_u set Pro_rata_rate = @proratarate Where insurance_file_cnt = @insurance_file_cnt AND risk_cnt IS NULL;
END;

WITH Fee_CTE AS (
  SELECT  @insurance_file_cnt 'insurance_file_cnt',
    fa.fee_percentage,
       fa.fee_amount,
   CASE WHEN fa.currency_id IS NULL THEN @currency_id ELSE fa.currency_id END 'Fee_Currency',
    @currency_id 'currency_id',
    @source_id 'branch_id',
    @base_currency_id 'base_currency_id',
    fa.risk_type_group_id,
    fa.peril_group_id,
    fa.transaction_sub_type,
    fa.tax_group_id,
    @risk_cnt 'risk_cnt',
    fa.is_fee_applied_to_cr,
    fa.party_cnt,
    fa.product_id,
    fa.transaction_type_id 'transaction_type_id',
    --CASE WHEN fa.transaction_type_id =0 and  ISNull(@last_insurance_file_cnt, 0) = 0  THEN @transaction_type_id ELSE fa.transaction_type_id END 'transaction_type_id',
    fa.include_fee_in_instalments,
       fa.spread_fee_across_instalments,
    FA.MakeLiveOptions_id,
    FA.DoPaymentTerms_id,
    FA.Calculation_Basis,
    FA.Is_Prorated,
    CASE
    WHEN  fa.fee_percentage <> 0 THEN  1
    WHEN  (ISNULL(fa.is_Prorated,0) <> 0) THEN @proratarate
    ELSE  1
    END 'proratarate',
    fa.Is_Override,
    Case when ISNULL(fa.fee_amount,0) = 0 and fa.currency_id IS NULL Then 1
    ELSE 0
    END 'FeeTypePercent',
	fa.fee_amount_id
  FROM    Fee_Amounts fa
  LEFT JOIN policy_fee_u pfu ON pfu.party_cnt = fa.party_cnt AND pfu.insurance_file_cnt = @insurance_file_cnt AND pfu.risk_cnt IS NULL
  WHERE  ( pfu.party_cnt IS NULL OR (ISNULL(fa.Use_when_deleted,0) = 1 AND  fa.is_deleted = 1 AND ISNULL(@BaseInsuranceFileCnt,0)<>0 AND ISNULL(pfu.party_cnt ,0)<>0))
  AND    (fa.transaction_type_id = @transaction_type_id    OR ISNULL(fa.transaction_type_id,0)=0)
  AND     (@transaction_sub_type_id = 0  OR fa.transaction_sub_type = @transaction_sub_type_id OR ISNULL(fa.transaction_sub_type,0) = 0)
  AND    (fa.product_id = @product_id OR ISNULL(fa.product_id,0) = 0)
  AND     ((ISNULL(fa.Use_when_deleted,0) = 1 AND  fa.is_deleted = 1 AND ISNULL(@BaseInsuranceFileCnt,0)<>0) OR  fa.is_deleted = 0)
  AND     (ISNULL(fa.MakeLiveOptions_id, 0) = 0 OR fa.MakeLiveOptions_id = @sMakeliveoptions_id)
  AND     (ISNULL(fa.DoPaymentTerms_id, 0) = 0 OR fa.DoPaymentTerms_id = @sPaymentDebitOrCash_id)
  AND    (
                     isnull(@premium,0) >= 0 OR
                     (
                     isnull(@premium,0) < 0 AND
                           (ISNULL(fa.is_fee_applied_to_cr, 0) = 1 AND ISNULL(fa.transaction_type_id, 0) = 0)
                           OR
                           (ISNULL(fa.transaction_type_id, 0) > 0)
                     )
                )
  AND     fa.effective_date = (SELECT  MAX(effective_date)
          FROM    fee_amounts fa2
          WHERE   (fa2.transaction_type_id = @transaction_type_id    OR ISNULL(fa2.transaction_type_id,0)=0)
          AND    ( @transaction_sub_type_id = 0 OR fa2.transaction_sub_type = @transaction_sub_type_id OR ISNULL(fa2.transaction_sub_type,0) = 0)
          AND    (fa2.product_id = @product_id OR ISNULL(fa2.product_id,0) = 0)
          AND     ((ISNULL(fa.Use_when_deleted,0) = 1 AND  fa.is_deleted = 1 AND ISNULL(@BaseInsuranceFileCnt,0)<>0) OR  fa2.is_deleted = 0)
  AND     (ISNULL(fa2.MakeLiveOptions_id, 0) = 0 OR fa2.MakeLiveOptions_id = @sMakeliveoptions_id)
  AND     (ISNULL(fa2.DoPaymentTerms_id, 0) = 0 OR fa2.DoPaymentTerms_id = @sPaymentDebitOrCash_id)
          AND     fa2.effective_date <= @effective_fee_date
          AND     fa2.party_cnt = fa.party_cnt
		  AND	  fa2.product_id IS NOT NULL)
)

    -- create policy_fee_u entries
  INSERT INTO Policy_Fee_U (
    insurance_file_cnt,
    fee_rate_percentage,
    fee_rate_amount,
    fee_rate_currency_id,
    currency_id,
       branch_id,
    base_currency_id,
    risk_type_group_id,
    peril_group_id,
    transaction_sub_type,
    tax_group_id,
    risk_cnt,
    is_fee_applied_to_cr,
    party_cnt,
    product_id,
    transaction_type_id,
    include_fee_in_instalments,
       spread_fee_across_instalments,
       MakeLiveOptions_id ,
       DoPaymentTerms_id,
       Calculation_Basis,
       Is_Prorated,
       Pro_rata_rate,
       is_override,
       FeeTypePercent,
	   fee_amount_id)
 SELECT     insurance_file_cnt,
    fee_percentage,
    fee_amount,
    Fee_Currency,
    currency_id,
       branch_id,
    base_currency_id,
    risk_type_group_id,
    peril_group_id,
    cte_fee_rate.transaction_sub_type,
    tax_group_id,
    risk_cnt,
    is_fee_applied_to_cr,
    cte_fee_rate.party_cnt,
    cte_fee_rate.product_id,
    cte_fee_rate.transaction_type_id,
    include_fee_in_instalments,
       spread_fee_across_instalments,
       MakeLiveOptions_id ,
       DoPaymentTerms_id,
       Calculation_Basis,
       Is_Prorated,
       proratarate,
       is_override,
       FeeTypePercent,
	   fee_amount_id
       FROM Fee_CTE cte_fee_rate
       Inner Join (SELECT party_cnt,
                                  MAX(ISNull(transaction_type_id, 0)) 'transaction_type_id',
                                  MAX(ISNull(product_id, 0)) 'product_id'
                                  FROM Fee_CTE GROUP BY party_cnt) cte_fee_party
                     ON cte_fee_party.party_cnt = cte_fee_rate.party_cnt
                                  AND (cte_fee_party.transaction_type_id = cte_fee_rate.transaction_type_id
                                                AND cte_fee_party.product_id = cte_fee_rate.product_id)
END
 If Exists(Select Null From @ExistingFee)    begin
 SET @Is_Fee_Updated = 1
  Update policy_fee_u
   Set fee_rate_amount = exFee.fee_rate_amount,
    fee_rate_percentage = exFee.fee_rate_percentage,
    FeeTypePercent = ISNULL(exFee.Is_Percent, 0)
  From policy_fee_u pfu
   Inner Join @ExistingFee exFee
    On exFee.party_cnt = pfu.party_cnt
     And ISNULL(exFee.transaction_type_id, 0) = ISNULL(pfu.transaction_type_id, 0)
     And ISNULL(exFee.tax_group_id, 0) = ISNULL(pfu.tax_group_id, 0)
     And ISNULL(exFee.transaction_sub_type, 0) = ISNULL(pfu.transaction_sub_type, 0)
     And ISNULL(exFee.fee_rate_currency_id, 0) = ISNULL(pfu.fee_rate_currency_id, 0)
     And ISNULL(exFee.MakeLiveOptions_id, 0) = ISNULL(pfu.MakeLiveOptions_id, 0)
     And ISNULL(exFee.DoPaymentTerms_id, 0) = ISNULL(pfu.DoPaymentTerms_id, 0)
     And ISNULL(exFee.Calculation_Basis, 0) = ISNULL(pfu.Calculation_Basis, 0)
     And ISNULL(exFee.Is_Prorated, 0) = ISNULL(pfu.Is_Prorated, 0)
     And ISNULL(exFee.Is_Override, 0) = ISNULL(pfu.Is_Override, 0)
  WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt IS NULL

	END
ELSE
       BEGIN

                           DECLARE PolicyFeeCursor CURSOR FAST_FORWARD FOR
                           SELECT  Party_cnt from policy_fee_u (nolock) where insurance_file_cnt = @insurance_file_cnt
                           OPEN PolicyFeeCursor

                           FETCH NEXT FROM PolicyFeeCursor
                           INTO @Party_cnt

                           WHILE @@FETCH_STATUS = 0
                           BEGIN
						    SELECT @insurance_folder_cnt=insurance_folder_cnt FROM Insurance_File
								   WHERE insurance_file_cnt=@insurance_file_cnt

								   SELECT top 1 @initial_anniversary_date=anniversary_date, @insurance_ref = insurance_ref FROM Insurance_File
								   WHERE insurance_ref=(select insurance_ref from Insurance_File where insurance_file_cnt = @insurance_file_cnt)
								    IF @transaction_type_id = 10 
									BEGIN
										select TOP 1 @last_insurance_file_cnt = exFee.insurance_file_cnt     From policy_fee_u
                                   Inner Join policy_fee_u exFee
                                   On exFee.party_cnt = policy_fee_u.party_cnt
                                   And ISNULL(exFee.transaction_type_id, 0) = ISNULL(policy_fee_u.transaction_type_id, 0)
                                   And ISNULL(exFee.tax_group_id, 0) = ISNULL(policy_fee_u.tax_group_id, 0)
                                   And ISNULL(exFee.transaction_sub_type, 0) = ISNULL(policy_fee_u.transaction_sub_type, 0)
                                   And ISNULL(exFee.fee_rate_currency_id, 0) = ISNULL(policy_fee_u.fee_rate_currency_id, 0)
                                   And ISNULL(exFee.MakeLiveOptions_id, 0) = ISNULL(policy_fee_u.MakeLiveOptions_id, 0)
                                   And ISNULL(exFee.DoPaymentTerms_id, 0) = ISNULL(policy_fee_u.DoPaymentTerms_id, 0)
                                   And ISNULL(exFee.Calculation_Basis, 0) = ISNULL(policy_fee_u.Calculation_Basis, 0)
                                   And ISNULL(exFee.Is_Prorated, 0) = ISNULL(policy_fee_u.Is_Prorated, 0)
                                   And ISNULL(exFee.Is_Override, 0) = ISNULL(policy_fee_u.Is_Override, 0)
								   INNER JOIN Insurance_File INF ON exFee.insurance_file_cnt=INF.insurance_file_cnt
                                   WHERE policy_fee_u.insurance_file_cnt = @insurance_file_cnt AND policy_fee_u.risk_cnt IS NULL
                                   AND exFee.insurance_file_cnt < case when anniversary_date > @initial_anniversary_date Then 
								  (select top 1 insurance_file_cnt from  insurance_file where insurance_ref = @insurance_ref and anniversary_date =@initial_anniversary_date order by cover_start_date desc) else @insurance_file_cnt end 
								   AND exFee.risk_cnt IS NULL
                                   AND exFee.insurance_file_cnt IN (SELECT insurance_file_cnt FROM Insurance_File where insurance_folder_cnt = @insurance_folder_cnt and cover_start_date<=@effective_Fee_date and insurance_file_type_id in (2,5,6,8,9))
                                  AND policy_fee_u.Is_Override = 1
                                   AND exFee.party_cnt = @Party_cnt
								   ORDER BY exFee.insurance_file_cnt DESC,INF.cover_start_date DESC
								   END
								   Else
								   BEGIN
                                  select TOP 1 @last_insurance_file_cnt = exFee.insurance_file_cnt     From policy_fee_u
                                   Inner Join policy_fee_u exFee
                                   On exFee.party_cnt = policy_fee_u.party_cnt
                                   And ISNULL(exFee.transaction_type_id, 0) = ISNULL(policy_fee_u.transaction_type_id, 0)
                                   And ISNULL(exFee.tax_group_id, 0) = ISNULL(policy_fee_u.tax_group_id, 0)
                                   And ISNULL(exFee.transaction_sub_type, 0) = ISNULL(policy_fee_u.transaction_sub_type, 0)
                                   And ISNULL(exFee.fee_rate_currency_id, 0) = ISNULL(policy_fee_u.fee_rate_currency_id, 0)
                                   And ISNULL(exFee.MakeLiveOptions_id, 0) = ISNULL(policy_fee_u.MakeLiveOptions_id, 0)
                                   And ISNULL(exFee.DoPaymentTerms_id, 0) = ISNULL(policy_fee_u.DoPaymentTerms_id, 0)
                                   And ISNULL(exFee.Calculation_Basis, 0) = ISNULL(policy_fee_u.Calculation_Basis, 0)
                                   And ISNULL(exFee.Is_Prorated, 0) = ISNULL(policy_fee_u.Is_Prorated, 0)
                                   And ISNULL(exFee.Is_Override, 0) = ISNULL(policy_fee_u.Is_Override, 0)
								   INNER JOIN Insurance_File INF ON exFee.insurance_file_cnt=INF.insurance_file_cnt
                                   WHERE policy_fee_u.insurance_file_cnt = @insurance_file_cnt AND policy_fee_u.risk_cnt IS NULL
                                   AND exFee.insurance_file_cnt < case when anniversary_date > @initial_anniversary_date Then 
								  (select top 1 insurance_file_cnt from  insurance_file where insurance_ref = @insurance_ref and anniversary_date =@initial_anniversary_date order by cover_start_date desc) else @insurance_file_cnt end 
								   AND exFee.risk_cnt IS NULL
                                   AND exFee.insurance_file_cnt IN (SELECT insurance_file_cnt FROM Insurance_File where insurance_folder_cnt = @insurance_folder_cnt and cover_start_date<=@effective_Fee_date and insurance_file_type_id in (2,5,6,8,9))
                                  AND policy_fee_u.Is_Override = 1
                                   AND exFee.party_cnt = @Party_cnt
								   ORDER BY INF.cover_start_date DESC,exFee.insurance_file_cnt DESC
								   END
                                    If ISNULL(@last_insurance_file_cnt,0) <> 0
                                    BEGIN

                                           Update policy_fee_u
                                           Set fee_rate_amount = exFee.fee_rate_amount,
                                           fee_rate_percentage = exFee.fee_rate_percentage,
                                           FeeTypePercent = ISNULL(exFee.FeeTypePercent, 0)
                               From policy_fee_u
                                           Inner Join policy_fee_u exFee
                                           On exFee.party_cnt = policy_fee_u.party_cnt
                                           And ISNULL(exFee.transaction_type_id, 0) = ISNULL(policy_fee_u.transaction_type_id, 0)
                                           And ISNULL(exFee.tax_group_id, 0) = ISNULL(policy_fee_u.tax_group_id, 0)
                                           And ISNULL(exFee.transaction_sub_type, 0) = ISNULL(policy_fee_u.transaction_sub_type, 0)
                                           And ISNULL(exFee.fee_rate_currency_id, 0) = ISNULL(policy_fee_u.fee_rate_currency_id, 0)
                                           And ISNULL(exFee.MakeLiveOptions_id, 0) = ISNULL(policy_fee_u.MakeLiveOptions_id, 0)
                                            And ISNULL(exFee.DoPaymentTerms_id, 0) = ISNULL(policy_fee_u.DoPaymentTerms_id, 0)
                                           And ISNULL(exFee.Calculation_Basis, 0) = ISNULL(policy_fee_u.Calculation_Basis, 0)
                                           And ISNULL(exFee.Is_Prorated, 0) = ISNULL(policy_fee_u.Is_Prorated, 0)
                                           And ISNULL(exFee.Is_Override, 0) = ISNULL(policy_fee_u.Is_Override, 0)
                                           WHERE policy_fee_u.insurance_file_cnt = @insurance_file_cnt AND policy_fee_u.risk_cnt IS NULL
                                           AND exFee.insurance_file_cnt = @last_insurance_file_cnt AND exFee.risk_cnt IS NULL
                                           --AND policy_fee_u.Is_Override = 1
                                    END

                           FETCH NEXT FROM PolicyFeeCursor
                           INTO @Party_cnt

                           END
                     CLOSE PolicyFeeCursor
                     DEALLOCATE PolicyFeeCursor

   END

END -- use existing fee details
    -- calculate the tax amounts

    EXEC spu_SIR_Calculate_Fee_Amounts_Wrapper @insurance_file_cnt, @risk_cnt, @Is_Fee_Updated

    -- calculate the tax amounts
    EXEC spu_SIR_Calculate_Fee_Tax_Amounts_Wrapper @insurance_file_cnt, @risk_cnt
GO
