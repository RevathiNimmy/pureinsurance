
EXEC DDLDropProcedure 'spu_ACT_DO_YearEND_Allocations'
GO

CREATE PROCEDURE spu_ACT_DO_YearEND_Allocations
	@document_id INT,
	@company_id INT,
	@Period_id INT,
	@currency_id INT,
	@dtDocAccountingDate DateTime,
	@user_id INT,
	@AllocationBatchId INT,
	@account_Id INT,
	@document_sequence INT OUTPUT,
	@running_total NUMERIC(19,4) OUTPUT

AS


SET NOCOUNT ON

DECLARE @NoOfRecords INT
DECLARE @branch_base_currency_id INT
SELECT @branch_base_currency_id = base_currency_Id FROM Source WHERE Source_id = @Company_id
SET @running_total = 0.0
DECLARE @idocSeq INT = @document_sequence
DECLARE @nBatchSize INT
Declare @nStart  INT
Declare @nEnd INT 


CREATE TABLE #OutStandingTransactions
(
 account_id INT,
 transdetail_id INT,
 outstanding_amount NUMERIC(17,2),
 outstanding_currency_amount  NUMERIC(17,2),
 total_outstanding  NUMERIC(17,2),
 rank INT,
 fully_matched BIT,
 is_primary BIT,
 currency_id SMALLINT,
 amount NUMERIC(19,4),
 base_amount_unrounded  NUMERIC(19,4),
 currency_amount  NUMERIC(19,4),
 currency_amount_unrounded  NUMERIC(19,4),
 currency_base_xrate  NUMERIC(19,4),
 document_id INT,
 document_type_id INT,
 document_ref VARCHAR(25),
 document_date datetime,
 document_sequence SMALLINT,
 spare VARCHAR(30),
 account_amount  NUMERIC(19,4),
 account_amount_unrounded  NUMERIC(19,4),
 system_amount  NUMERIC(19,4),
 system_amount_unrounded  NUMERIC(19,4),
 account_base_xrate  NUMERIC(19,4),
 system_base_xrate  NUMERIC(19,4),
 AllocBaseAmount  NUMERIC(19,4),
 AllocCCyAmount NUMERIC(19,4),
 AllocAccountAmount NUMERIC(19,4),
 AllocSystemAmount NUMERIC(19,4),
 cBaseMatchAmount NUMERIC(19,4),
 cCurrencyMatchAmount  NUMERIC(19,4),
 OSBaseAmount NUMERIC(19,4),
 OSCcyAmount NUMERIC(19,4),
 ID INT IDENTITY(1,1)
)

CREATE  CLUSTERED INDEX IX_id ON #OutStandingTransactions(ID);

Create table #AllocDetail
(
 transdetail_id INT Primary KEY,
 alloc_base_amount_total NUMERIC(19,4),
 alloc_ccy_amount_total NUMERIC(19,4)
)

 INSERT INTO #AllocDetail
  (
   transdetail_id,
   alloc_base_amount_total,
   alloc_ccy_amount_total
  )

 SELECT td.transdetail_id,
  SUM(alloc_base_amount),
  SUM(alloc_ccy_amount)
 FROM    Transdetail td
 JOIN allocationdetail ald
  ON ALD.Transdetail_id = td.Transdetail_id
 WHERE
  td.company_id = @company_id
  AND td.outstanding_amount <> 0
  AND  td.Period_id < = @period_id
  AND  ISNULL(ald.is_reversed, 0) = 0
  AND  td.account_id = @account_Id
 Group by td.transdetail_id

DECLARE @is_primary_trans INT =1

 TRUNCATE TABLE #OutStandingTransactions

 INSERT INTO #OutStandingTransactions
 (
  account_id,
  transdetail_id,
  outstanding_amount,
  outstanding_currency_amount,
  total_outstanding,
  rank,
  fully_matched,
  currency_id,
  amount,
  base_amount_unrounded,
  currency_amount,
  currency_amount_unrounded,
  currency_base_xrate,
  document_id,
  document_sequence,
  spare,
  account_amount,
  account_amount_unrounded,
  system_amount,
  system_amount_unrounded,
  account_base_xrate,
  system_base_xrate,
  document_type_id,
  document_ref,
  document_date,
  cBaseMatchAmount,
  cCurrencyMatchAmount,
AllocBaseAmount,
AllocAccountAmount,
AllocSystemAmount,
OSBaseAmount,
OSCcyAmount,
AllocCcyAmount
 )
 SELECT td.account_id,
  td.transdetail_id,
  td.outstanding_amount,
  td.outstanding_currency_amount  , 0.0
  ,1
  ,TD.fully_matched, TD.currency_id,
  TD.amount,TD.amount,
  TD.currency_amount,
  TD.currency_amount,
  TD.currency_base_xrate,
  TD.document_id,
  TD.document_sequence,
  TD.spare,
  TD.account_amount,
  TD.account_amount_unrounded,
  TD.system_amount,
  TD.system_amount_unrounded,
  TD.account_base_xrate,
  TD.system_base_xrate,
  D.documentType_id,
  D.Document_ref,
  d.document_date,
  ISNULL(alloc_base_amount_total,0),
  ISNULL(alloc_ccy_amount_total,0),
  td.outstanding_amount,
  td.outstanding_amount/td.account_base_xrate,
  td.outstanding_amount/td.system_base_xrate,
  td.amount - ISNULL(alloc_base_amount_total,0),
  td.currency_amount - ISNULL(alloc_ccy_amount_total,0),
  CASE WHEN td.currency_id = @branch_base_currency_id THEN td.outstanding_amount END
 FROM   Transdetail td
  JOIN Document D on TD.Document_id = D.Document_id
  LEFT JOIN #AllocDetail ad ON td.Transdetail_id = ad.Transdetail_id
  WHERE
   td.company_id = @company_id
   and td.Account_id = @account_Id
   AND td.outstanding_amount <> 0
   AND  td.Period_id < = @period_id
 DECLARE
  @out_currency_id     INT,
  @out_currency_rate   NUMERIC(19, 8),
  @out_system_rate     NUMERIC(19, 8),
  @return_status       INT,
  @base_decimals   INT,
  @system_decimals  INT
 DECLARE currency CURSOR FOR
 SELECT DISTINCT CURRENCY_ID FROM #OutStandingTransactions O
  WHERE O.currency_id <> @branch_base_currency_id
 OPEN currency
 FETCH NEXT FROM currency INTO @out_currency_id
 WHILE @@FETCH_STATUS = 0
 BEGIN
  EXEC spu_ACT_Do_Currency_Conversion
             @company_id = @company_id,
             @currency_id =  @out_currency_id,
             @currency_amount_unrounded = 100.00,
             @mode = 'ALL',
             @currency_base_xrate = @out_currency_rate OUTPUT,
             @system_base_xrate = @out_system_rate OUTPUT,
             @base_decimals = @base_decimals OUTPUT,
             @system_decimals = @system_decimals OUTPUT,
             @return_status = @return_status OUTPUT
  UPDATE O
   SET O.AllocCcyAmount =
   CASE O.currency_base_xRate
    WHEN 1 THEN  ROUND(o.outstanding_amount ,@base_decimals)
   ELSE ROUND(o.outstanding_amount / @out_currency_rate,@base_decimals)
    END
  FROM #OutStandingTransactions O
  WHERE O.Currency_id = @out_currency_id
  FETCH NEXT FROM currency INTO @out_currency_id
 END
 CLOSE currency
 DEALLOCATE currency

 
 UPDATE O
 SET
  O.fully_matched =1
 FROM #OutStandingTransactions AS O
 WHERE OSBaseAmount = AllocBaseAmount OR OSCcyAmount = AllocCCyAmount
 DECLARE @transdetail_id INT, @total_outstanding NUMERIC(19,2)
 DECLARE @Today DATETIME
 SET @Today = GETDATE()
 SET @dtDocAccountingDate = Convert(VARCHAR,@dtDocAccountingDate,106)
 SELECT @NoOfRecords = Count(*) from #OutStandingTransactions

 IF @NoOfRecords > 0
 BEGIN
  SELECT @total_outstanding = -1 * SUM(OutStanding_amount) FROM #OutStandingTransactions

  SET @running_total = @running_total + (@total_outstanding* -1)

  EXEC spu_ACT_add_TransDetail @transdetail_id=@transdetail_id OUTPUT,
    @account_id = @account_Id,
    @postingstatus_id=3,
    @company_id=@company_id, 
	@currency_id=@currency_id,
    @period_id=@Period_id, 
	@document_id=@document_id,
    @document_sequence= @idocSeq,
    @accounting_date=@dtDocAccountingDate,
    @amount=@total_outstanding,
    @base_amount_unrounded=@total_outstanding,
    @fully_matched = 1, @currency_amount=@total_outstanding,
    @currency_amount_unrounded= @total_outstanding,
    @euro_currency_id=NULL, @euro_amount=0.0000,
    @euro_base_xrate=1.0000,@euro_ccy_xrate=1.0000,
    @comment='Year End Retained Profit',
    @insurance_ref=NULL,
	@operator_id=@user_id,
    @purchase_order_no=NULL,
	@purchase_invoice_no=NULL,
    @department=NULL,@spare='',
	@ref_date=@dtDocAccountingDate,
    @ref_amount=0.0000,
	@ref_quantity=0.0000,
	@ref_units='0',
    @insurance_ref_index=NULL,
	@department_id=NULL,
    @underwriting_year_id=NULL,
	@currency_base_xrate=1,
	@currency_base_date=@dtDocAccountingDate,
    @account_base_xrate=1,
    @account_base_date=@dtDocAccountingDate,
    @system_base_xrate=1,
    @system_base_date=@dtDocAccountingDate,
    @transdetail_type_id=NULL,
	@reference=NULL,@type_code=NULL,@tax_group_id=0,@tax_band_id=0,@claim_ref=NULL,
    @balance_type=NULL,@risk_transfer=NULL,@due_date=NULL,@FeeType=NULL

SET @idocSeq = ISNULL(@idocSeq,0) +1

  INSERT INTO #OutStandingTransactions
  (
   account_id,
   transdetail_id,
   outstanding_amount,
   outstanding_currency_amount,
   total_outstanding,
   rank,
   is_primary,
   fully_matched,
   currency_id,
   amount,
   base_amount_unrounded,
   currency_amount,
   currency_amount_unrounded,
   currency_base_xrate,
   document_id,
   document_sequence,
   spare,
   account_amount,
   account_amount_unrounded,
   system_amount,
   system_amount_unrounded,
   account_base_xrate,
   system_base_xrate,
   document_type_id,
   document_ref,
   document_date,
   AllocBaseAmount,
   AllocAccountAmount,
   AllocSystemAmount,
   AllocCCyAmount,
   OSBaseAmount,
   OSCcyAmount
  )

  SELECT  @account_Id, @transdetail_id,
   @total_outstanding, @total_outstanding,
   @total_outstanding, @idocSeq ,
   @is_primary_trans,
   1, TD.currency_id,
   TD.amount,TD.amount,
   TD.currency_amount,TD.currency_amount,
   TD.currency_base_xrate,
   TD.document_id,
   TD.document_sequence,TD.spare,
   TD.account_amount,TD.account_amount_unrounded,
   TD.system_amount,
   TD.system_amount_unrounded,
   TD.account_base_xrate,
   TD.system_base_xrate,
   D.documentType_id,
   D.Document_ref,
   d.document_date,
   @total_outstanding,
   @total_outstanding / TD.account_base_xrate,
   @total_outstanding / TD.system_base_xrate,
   @total_outstanding,
   TD.amount,
   TD.currency_amount
  FROM TransDetail TD
  JOIN Document D ON TD.Document_ID = D.Document_ID
  WHERE TD.Transdetail_id = @transdetail_id
  SET @is_primary_trans = 0
  DECLARE @kAllocationStatusAllocated INT = 3
  DECLARE @Allocation_id INT

  EXEC spu_ACT_add_Allocation
   @company_id=@company_id,
   @account_id=@account_Id,
   @user_id=@user_id,
   @allocation_date=@Today,
   @allocationstatus_id=@kAllocationStatusAllocated,
   @nAllocationbatch_id=@AllocationBatchId,
   @Allocation_id=@Allocation_id OUTPUT

SELECT @nStart=0
SELECT @nBatchSize = 20000
SELECT @nEnd = @nBatchSize

WHILE @nBatchSize <> 0
BEGIN

INSERT INTO AllocationDetail
  (
    allocation_id,
   original_currency,
   transdetail_id,  
   documenttype_id,
   document_ref,    
   accounting_date,
   original_date,   
   allocate_to_base,
   orig_base_amount, 
   orig_base_amount_unrounded,
   orig_ccy_amount, 
   orig_ccy_amount_unrounded,
   orig_xrate,    effective_xrate,
   os_base_amount,
   os_ccy_amount,
   alloc_base_amount, 
   alloc_ccy_amount,
   fully_matched,
   new_os_ccy_amount,
   new_os_base_amount,
   loss_gain_amount,
   is_primary,
   euro_currency_id, 
   euro_amount,
   euro_base_xrate,  
   euro_ccy_xrate,
   alloc_account_amount,
   alloc_system_amount)

  SELECT
    @Allocation_id,
   T.Currency_id ,
   T.transdetail_id,
   T.Document_type_id,
   T.Document_ref, 
   @Today,
   T.document_date,
   0,T.amount, 
   T.amount,
   currency_amount,
   currency_amount_unrounded,
   currency_base_xrate , 
   currency_base_xrate,
   OSBaseAmount,
   OSCcyAmount,
   AllocBaseAmount, 
   AllocCCyAmount,
   fully_matched,
   OSCcyAmount - AllocCCyAmount ,
   OSBaseAmount - AllocBaseAmount,
   0,
   ISNULL(is_primary,0) ,
   0, 
   0.0, 
   1, 
   1,
   AllocAccountAmount,
   AllocSystemAmount
  FROM #OutStandingTransactions T  WHERE ID >=@nStart AND ID < @nEnd
  SET @nBatchSize = @@rowcount 
  SET @nStart = @nStart + @nBatchSize
  SET @nEnd =   @nEnd + @nBatchSize
END
END

 SELECT @nBatchSize=20000
 SELECT @nStart=0
 SELECT @nEnd=@nBatchSize
 
 WHILE @nBatchSize <> 0
 BEGIN
 UPDATE T
 		SET T.outstanding_amount = 
		CASE WHEN O.Outstanding_amount - AllocBaseAmount = 0 THEN 0.0  
		     WHEN O.Amount - (O.Outstanding_amount - AllocBaseAmount) = 0 THEN T.amount
		     ELSE T.outstanding_amount - AllocBaseAmount end,
		T.outstanding_currency_amount = 
		CASE WHEN O.Outstanding_amount - AllocBaseAmount = 0 THEN 0.0 
		     WHEN O.Amount - (O.Outstanding_amount - AllocBaseAmount) = 0 THEN T.currency_amount
		     ELSE T.outstanding_currency_amount - AllocCCyAmount  end,
		T.outstanding_account_amount =
		CASE WHEN O.Outstanding_amount - AllocBaseAmount = 0 THEN 0.0 
		     WHEN O.Amount - (O.Outstanding_amount - AllocBaseAmount) = 0 THEN T.account_amount
		     ELSE T.outstanding_account_amount - AllocAccountAmount end,
		T.outstanding_system_amount =
		CASE WHEN O.Outstanding_amount - AllocBaseAmount = 0 THEN 0.0 
		     WHEN O.Amount - (O.Outstanding_amount - AllocBaseAmount) = 0 THEN T.system_amount
		     ELSE T.outstanding_system_amount - AllocSystemAmount end,
		T.amount_updated = Getdate(),
		T.Fully_matched =1 	 		
	
 FROM TransDetail T JOIN #OutStandingTransactions O
 ON T.Transdetail_id = O.TransDetail_id
 WHERE O.ID >= @nStart and O.ID < @nEnd
 SET @nBatchSize = @@rowcount 

  SET @nStart = @nStart + @nBatchSize
  SET @nEnd =   @nEnd + @nBatchSize
  END
 
 SET @document_sequence = @idocSeq

GO
