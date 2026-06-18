SET QUOTED_IDENTIFIER OFF 
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountBal'
GO

/*If bank then use currency amount*/    
/*If only one base currency then use base amount*/    
/*If no account then use system amount, as accounts might have different currencies and their transactions could be across two branches with different base rates.*/    
/*Otherwise use account amount, as transactions for account could be across two branches with different base rates.*/    
   
CREATE PROCEDURE spu_ACT_Select_AccountBal    
    @company_id INT = NULL,
    @account_id INT = NULL,
    @accounting_date DATETIME = NULL,
    @postingstatus_id SMALLINT = NULL,
    @sub_branch_id INT = NULL,
--Start (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.4)
    @restrict_to_non_policy_transactions BIT = 0
--End (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.4)
AS

DECLARE @isbank as TINYINT
DECLARE @currency_id SMALLINT
DECLARE @TypeOfRates TINYINT
DECLARE @SQL VARCHAR(1000)
DECLARE @SQLWhere VARCHAR(1000)
DECLARE @SQLAnd VARCHAR(20)
DECLARE @SQLGroup VARCHAR(100)
DECLARE @party_cnt INT

DECLARE @Account_EntryExists SMALLINT
DECLARE @Party_type VARCHAR(10)
DECLARE @IsFloatBalanceAccount SMALLINT
DECLARE @IsOverdraftAccount SMALLINT
DECLARE @FloatBalance MONEY
DECLARE @Overdraft MONEY

SELECT @SQLAnd='WHERE '
SELECT @SQLWhere=''
SELECT @SQLGroup=''

SELECT @Account_EntryExists  =1
-- Determine whether the account is a bank or not
SELECT @isbank = 0

IF @account_ID IS NOT NULL
 SELECT @isbank = 1, @currency_id=currency_id
 FROM   bankaccount
 WHERE  account_id = @account_id

EXEC spu_ACT_GetTypeOfRates @TypeOfRates

SELECT @party_cnt=Account.Account_key FROM Account WHERE account_id=@account_id

SELECT @party_type =party_type.code,@isFloatBalanceAccount=is_float_balance_account,
@isOverdraftAccount =is_overdraft_account
FROM Party
JOIN Party_Type ON
Party.party_type_id=Party_type.party_type_id
JOIN party_agent ON party.party_cnt=party_agent.party_cnt
WHERE party.party_cnt=@party_cnt

IF @isbank=1
BEGIN
 SELECT @SQL='SELECT ISNULL(SUM(ROUND(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)), 2)), 0) AS sum_amount, '+CONVERT(VARCHAR(20),@currency_id)+'currency_id ' 

 IF RTRIM(@party_type)='AG' and ( @isFloatBalanceAccount=1 or @isOverdraftAccount=1)
     BEGIN
         SELECT @floatBalance=ISNULL(Party_agent.float_balance_limit,0)
         -ISNULL((SELECT SUM(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)))FROM transdetail td WHERE balance_type='FB'
         and account_id=@account_id ),0) FROM party_agent
         WHERE party_cnt =@Party_cnt

         SELECT @OverDraft=ISNULL(Party_agent.Overdraft_limit,0)
         -ISNULL((SELECT SUM(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)))FROM transdetail td WHERE balance_type='OD'   
         and account_id=@account_id ),0) FROM party_agent
         WHERE party_cnt =@Party_cnt

         SELECT @SQL=@SQL + ','+  convert(varchar,@floatBalance) +' FloatBalance ,'+  convert(varchar,@Overdraft) +' Overdraft '
      END
      ELSE
      BEGIN
         SELECT @SQL=@SQL + ', NULL as FloatBalance , NULL as Overdraft '

      END
END

ELSE
BEGIN
 IF @TypeOfRates = 1 OR @company_id IS NOT NULL
 BEGIN
  SELECT @SQL='SELECT ISNULL(SUM(ROUND(CAST(ISNULL(td.outstanding_amount,0) AS DECIMAL(19,4)),2)), 0) AS sum_amount, 0 currency_id '
  IF RTRIM(@party_type)='AG' and ( @isFloatBalanceAccount=1 or @isOverdraftAccount=1)
     BEGIN
         SELECT @floatBalance=ISNULL(Party_agent.float_balance_limit,0)
         -ISNULL((SELECT SUM(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)))FROM transdetail td WHERE balance_type='FB'   
         and account_id=@account_id ),0) FROM party_agent
         WHERE party_cnt =@Party_cnt

         SELECT @OverDraft=ISNULL(Party_agent.Overdraft_limit,0)
         -ISNULL((SELECT SUM(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)))FROM transdetail td WHERE balance_type='OD'
         and account_id=@account_id ),0) FROM party_agent
         WHERE party_cnt =@Party_cnt

         SELECT @SQL=@SQL + ','+  convert(varchar,@floatBalance) +' FloatBalance ,'+  convert(varchar,@Overdraft) +' Overdraft '
     END
     ELSE
     BEGIN
         SELECT @SQL=@SQL + ', NULL as FloatBalance , NULL as Overdraft '
     END

 END
 ELSE
 BEGIN
  IF @account_id IS NULL BEGIN
   SELECT @SQL='SELECT ISNULL(SUM(ROUND(CAST(ISNULL(td.outstanding_system_amount,0) AS DECIMAL(19,4)),2)), 0) AS sum_amount, '
   SELECT @SQL=@SQL+'(SELECT currency_id FROM PMSystem WHERE system_id = 1) AS currency_id '
   SELECT @SQL=@SQL + ','+  NULL +' FloatBalance ,'+  NULL +' Overdraft '
  END
  ELSE
   SELECT @SQL='SELECT ISNULL(SUM(ROUND(CAST(ISNULL(td.outstanding_account_amount,0) AS DECIMAL(19,4)),2)), 0) AS sum_amount, account_currency_id currency_id '
   IF RTRIM(@party_type)='AG' and ( @isFloatBalanceAccount=1 or @isOverdraftAccount=1)
     BEGIN
     IF EXISTS (SELECT 1 FROM transdetail WHERE account_id =@account_id )
     BEGIN
         SELECT @Account_EntryExists =1
     END
     ELSE
     BEGIN
         SELECT @Account_EntryExists =0
         SELECT @SQL='SELECT 0 AS sum_amount, (Select Currency_id from account where Account_id= ' +  convert(varchar(50),@Account_id)+') Currency_ID '
     END
         SELECT @floatBalance=ISNULL(Party_agent.float_balance_limit,0)
		 -ISNULL((SELECT SUM(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)))FROM transdetail td WHERE balance_type='FB'   
         and account_id=@account_id ),0) FROM party_agent
         WHERE party_cnt =@Party_cnt

         SELECT @OverDraft=ISNULL(Party_agent.Overdraft_limit,0)
         -ISNULL((SELECT SUM(CAST(ISNULL(outstanding_account_amount,0) AS DECIMAL(19,4)))FROM transdetail td WHERE balance_type='OD'
         and account_id=@account_id ),0) FROM party_agent
         WHERE party_cnt =@Party_cnt
         SELECT @SQL=@SQL + ','+  convert(varchar,@floatBalance) +' FloatBalance ,'+  convert(varchar,@Overdraft) +' Overdraft '
     END
     ELSE
     BEGIN
	     IF EXISTS (SELECT 1 FROM transdetail WHERE account_id =@account_id )
	     BEGIN
	         SELECT @Account_EntryExists =1
	     END
	     ELSE
	     BEGIN
	         SELECT @Account_EntryExists =0
	         SELECT @SQL='SELECT 0 AS sum_amount, (Select Currency_id from account where Account_id= ' +  convert(varchar(50),@Account_id)+') Currency_ID '
	     END
         SELECT @SQL=@SQL + ', NULL as FloatBalance , NULL as Overdraft '
     END
 END
END
IF @Account_EntryExists=1
SELECT @SQL=@SQL+'FROM transdetail td inner join document d on td.document_id=d.document_id '

IF @account_id IS NOT NULL AND @Account_EntryExists=1 BEGIN
 SELECT @SQLWhere=@SQLWhere+@SQLAnd+'td.account_id = '+CONVERT(VARCHAR(50),@account_id)+' '
 SELECT @SQLAnd='AND '
END

IF @postingstatus_id IS NOT NULL AND @Account_EntryExists=1 BEGIN
 SELECT @SQLWhere=@SQLWhere+@SQLAnd+'td.postingstatus_id = '+CONVERT(VARCHAR(50),@postingstatus_id)+' '
 SELECT @SQLAnd='AND '
END

IF @company_id IS NOT NULL AND @Account_EntryExists=1 BEGIN
 SELECT @SQLWhere=@SQLWhere+@SQLAnd+'td.company_id = '+CONVERT(VARCHAR(50),@company_id)+' '
 SELECT @SQLAnd='AND '
END

IF @accounting_date IS NOT NULL AND @Account_EntryExists=1 BEGIN
 SELECT @SQLWhere=@SQLWhere+@SQLAnd+'d.document_date <= ''' + CONVERT(varchar(30), CONVERT(datetime, @accounting_date, 106) + ' 23:59:59:000', 120) + ''' '
 SELECT @SQLAnd='AND '
END
--Start (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.4)


IF @restrict_to_non_policy_transactions = 1 AND @Account_EntryExists=1 BEGIN  
 SELECT @SQLWhere=@SQLWhere+@SQLAnd+'d.insurance_file_cnt IS NULL '  
 SELECT @SQLAnd='AND '  
END
--End (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.4)
IF @sub_branch_id IS NOT NULL AND @Account_EntryExists=1
 SELECT @SQLWhere=@SQLWhere+@SQLAnd+'td.sub_branch_id = '+CONVERT(VARCHAR(50),@sub_branch_id)+' '

IF @account_id IS NOT NULL AND @isbank=0 AND @Account_EntryExists=1
 SELECT @SQLGroup='GROUP BY td.account_currency_id'

SELECT @SQL=@SQL+@SQLWhere+@SQLGroup

--EXECUTE (@SQL)

DECLARE @count VARCHAR

DECLARE  @table2 Table
	(
		 sum_amount   DECIMAL,
		 currency_id  FLOAT,
		 Floatbalance FLOAT,
		 overdraft    FLOAT
	)

INSERT INTO @table2
			(sum_amount,
			 currency_id,
			 Floatbalance,
			 overdraft)
EXECUTE (@SQL)

SELECT @count = (SELECT COUNT(*)
				 FROM   @table2)

IF @count > 1
BEGIN
	DECLARE @Client_Currency INT

	SELECT @Client_Currency = Currency_id
	FROM   party
	WHERE  party_cnt = @party_cnt

	DECLARE @Currency_id1 INT
	
	DECLARE Table2_Cursor CURSOR FAST_FORWARD FOR
		SELECT currency_id
		FROM   @table2

	OPEN Table2_Cursor

	FETCH NEXT FROM Table2_Cursor INTO @currency_id1

	WHILE ( @@FETCH_STATUS = 0 )
	BEGIN
		IF @currency_id1 <> @Client_Currency
			DECLARE @Rates MONEY

		DECLARE @BaseRates MONEY

		SELECT @Rates = R.rate_against_base
		FROM   CurrencyRate R
		WHERE  ( R.company_id = @company_id
				  OR @company_id IS NULL
					 AND R.currency_id = @currency_id1 )
			   AND R.effective_from = (SELECT MAX(effective_from)
									   FROM   CurrencyRate
									   WHERE  effective_from <= ISNULL(@accounting_date, GETDATE())
											  AND company_id = R.company_id
											  AND currency_id = R.currency_id)
		ORDER  BY R.currency_id,
				  R.company_id DESC

		SELECT @baseRates = system_base_xrate
		FROM   transdetail
		WHERE  account_id = @account_id

		UPDATE @table2
		SET    Sum_amount = Sum_Amount * isnull(@baseRates, 1)
		WHERE  CURRENCY_ID = @currency_id1

		UPDATE @table2
		SET    SUM_AMOUNT = SUM_AMOUNT * isnull(@baseRates, 1)
		WHERE  CURRENCY_ID = @currency_id1

		FETCH NEXT FROM Table2_Cursor INTO @currency_id1
	END

	CLOSE Table2_Cursor

	DEALLOCATE Table2_Cursor

	DECLARE @SUM_AMOUNT MONEY

	SELECT @SUM_AMOUNT = isnull(SUM(SUM_AMOUNT), 0)
	FROM   @table2

	SELECT @SUM_AMOUNT  AS Sum_amount,
		   currency_id=@Client_Currency,
		   Floatbalance AS FloatBalance,
		   Overdraft    AS Overdraft
	FROM   @table2
END
ELSE
BEGIN
	EXECUTE (@SQL)
END



GO 


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
