SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLDropProcedure spu_SAM_ACT_Select_AccountBal
GO
/*If bank then use currency amount*/
/*If only one base currency then use base amount*/  
/*If no account then use system amount, as accounts might have different currencies and their transactions could be across two branches with different base rates.*/  
/*Otherwise use account amount, as transactions for account could be across two branches with different base rates.*/  
  
CREATE PROCEDURE spu_SAM_ACT_Select_AccountBal  
    @party_cnt int  
AS  
BEGIN

	DECLARE @account_id int  
	SELECT @account_id=account.account_id 
	FROM account 
	WHERE account_key=@party_cnt  
	
	DECLARE @isbank as TINYINT  
	DECLARE @currency_id SMALLINT  
	DECLARE @TypeOfRates TINYINT  
	DECLARE @SQL VARCHAR(1000)  
	DECLARE @SQLWhere VARCHAR(1000)  
	DECLARE @SQLAnd VARCHAR(20)  
	DECLARE @SQLGroup VARCHAR(100)  
	 
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
	  
	--SELECT @party_cnt=Account.Account_key FROM Account WHERE account_id=@account_id  
	  
	SELECT  @party_type =party_type.code,
            @isFloatBalanceAccount=is_float_balance_account,  
	        @isOverdraftAccount =is_overdraft_account  
	FROM     Party  
	JOIN     Party_Type 
        ON Party.party_type_id=Party_type.party_type_id  
	JOIN party_agent 
        ON party.party_cnt=party_agent.party_cnt  
	WHERE party.party_cnt=@party_cnt  
	  
	IF @isbank=1  
	BEGIN  
	    SELECT @SQL='SELECT ISNULL(SUM(ROUND(outstanding_account_amount, 2)), 0) AS sum_amount, '+CONVERT(VARCHAR(20),@currency_id)+'currency_id '  
	  
	    IF RTRIM(@party_type)='AG' and ( @isFloatBalanceAccount=1 or @isOverdraftAccount=1)  
	    BEGIN  
	        SELECT @floatBalance=ISNULL(Party_agent.float_balance_limit,0)  
            -ISNULL((SELECT SUM(outstanding_account_amount)FROM transdetail td WHERE balance_type='FB'  
            and account_id=@account_id ),0) FROM party_agent  
            WHERE party_cnt =@Party_cnt  
            
            SELECT @OverDraft=ISNULL(Party_agent.Overdraft_limit,0)  
            -ISNULL((SELECT SUM(outstanding_account_amount)FROM transdetail td WHERE balance_type='OD'  
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
        IF @TypeOfRates = 1 
        BEGIN  
            SELECT @SQL='SELECT ISNULL(SUM(ROUND(td.outstanding_amount,2)), 0) AS sum_amount, 0 currency_id '  
            IF RTRIM(@party_type)='AG' and ( @isFloatBalanceAccount=1 or @isOverdraftAccount=1)  
            BEGIN  
                SELECT @floatBalance=ISNULL(Party_agent.float_balance_limit,0)  
                -ISNULL((SELECT SUM(outstanding_account_amount)FROM transdetail td WHERE balance_type='FB'  
                and account_id=@account_id ),0) FROM party_agent  
                WHERE party_cnt =@Party_cnt  
            
                SELECT @OverDraft=ISNULL(Party_agent.Overdraft_limit,0)  
                -ISNULL((SELECT SUM(outstanding_account_amount)FROM transdetail td WHERE balance_type='OD'  
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
            IF @account_id IS NULL 
            BEGIN  
                SELECT @SQL='SELECT ISNULL(SUM(ROUND(td.outstanding_system_amount,2)), 0) AS sum_amount, '  
                SELECT @SQL=@SQL+'(SELECT currency_id FROM PMSystem WHERE system_id = 1) AS currency_id '  
                SELECT @SQL=@SQL + ','+  NULL +' FloatBalance ,'+  NULL +' Overdraft '  
            END  
            ELSE  
                SELECT @SQL='SELECT ISNULL(SUM(ROUND(td.outstanding_account_amount,2)), 0) AS sum_amount, account_currency_id currency_id '  
                IF RTRIM(@party_type)='AG' and ( @isFloatBalanceAccount=1 or @isOverdraftAccount=1)  
                BEGIN  
                    IF EXISTS (SELECT 1 FROM transdetail WHERE account_id =@account_id )  
                    BEGIN  
                        SELECT @Account_EntryExists =1  
                    END  
                    ELSE  
                    BEGIN  
                        SELECT @Account_EntryExists =0  
                        SELECT @SQL='SELECT DISTINCT 0 AS sum_amount, (Select Currency_id from account where Account_id= ' +  convert(varchar(50),@Account_id)+') CurrencyID '  
                    END  
                    SELECT @floatBalance=ISNULL(Party_agent.float_balance_limit,0)  
                    -ISNULL((SELECT SUM(outstanding_account_amount)FROM transdetail td WHERE balance_type='FB'  
                    and account_id=@account_id ),0) FROM party_agent  
                    WHERE party_cnt =@Party_cnt  
        
                    SELECT @OverDraft=ISNULL(Party_agent.Overdraft_limit,0)  
                    -ISNULL((SELECT SUM(outstanding_account_amount)FROM transdetail td WHERE balance_type='OD'  
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
                    SELECT @SQL='SELECT DISTINCT 0 AS sum_amount, (Select Currency_id from account where Account_id= ' +  convert(varchar(50),@Account_id)+') CurrencyID '  
                END  
                SELECT @SQL=@SQL + ', NULL as FloatBalance , NULL as Overdraft '  
            END  
        END  
	END  
	IF @Account_EntryExists = 1
	SELECT @SQL=@SQL+'FROM transdetail td inner join document d on td.document_id=d.document_id '  
	  
	IF @account_id IS NOT NULL AND @Account_EntryExists=1 
    BEGIN  
	    SELECT @SQLWhere=@SQLWhere+@SQLAnd+'td.account_id = '+CONVERT(VARCHAR(50),@account_id)+' '  
	    SELECT @SQLAnd='AND '  
	END  
	  
	IF @account_id IS NOT NULL AND @isbank=0 and @Account_EntryExists =1   
	    SELECT @SQLGroup='GROUP BY td.account_currency_id'  
	  
	SELECT @SQL=@SQL+@SQLWhere+@SQLGroup  
	  
	--EXECUTE (@SQL)  
	  
	DECLARE @count INT  
	  
	CREATE TABLE #table2  
	(  
	sum_amount MONEY,  
	currency_id FLOAT,  
	Floatbalance FLOAT,  
	overdraft FLOAT  
	)  
	  
	INSERT INTO  #table2(  
	sum_amount,  
	currency_id,  
	Floatbalance,  
	overdraft)  
	EXECUTE (@SQL)  
	  
	SELECT @count=(SELECT COUNT(*) FROM #table2)  
	  
	IF @count >1  
	BEGIN  
    	  
    	DECLARE @Client_Currency INT  
    	  
    	SELECT @Client_Currency =Currency_id FROM party WHERE party_cnt=@party_cnt  
    	  
    	DECLARE @Currency_id1 INT  
    	DECLARE Table2_Cursor CURSOR FAST_FORWARD FOR  
    	SELECT  currency_id FROM #table2  
    	  
    	OPEN Table2_Cursor  
    	  
    	FETCH NEXT FROM  Table2_Cursor INTO  @currency_id1  
    	
        WHILE (@@Fetch_Status = 0)  
    	BEGIN  
    	  
    	    IF @currency_id1<>@Client_Currency  
    	    BEGIN
        	    DECLARE @Rates MONEY  
        	    DECLARE @BaseRates MONEY  
        	  
        	    SELECT @Rates=R.rate_against_base  
        	    FROM CurrencyRate R  
        	    WHERE R.currency_id=@currency_id1 
        	        AND R.effective_from = (  
        	                                SELECT MAX(effective_from)  
        	                                FROM CurrencyRate  
        	                                WHERE effective_from <= GETDATE()  
        	                           	    AND company_id = R.company_id  
        	                              	AND currency_id = R.currency_id  
        	                               )  
                ORDER BY R.currency_id, R.company_id DESC  
        	  
        	    SELECT  @baseRates= system_base_xrate 
                FROM transdetail 
                WHERE account_id = @account_id  
        	  
        	    UPDATE #table2 
                SET Sum_amount=Sum_Amount*@baseRates  
        	    WHERE CURRENCY_ID=@currency_id1  

        	    UPDATE #table2 
                SET SUM_AMOUNT=SUM_AMOUNT*@Rates 
                WHERE CURRENCY_ID=@currency_id1  
            END
    	  
    	    FETCH NEXT FROM Table2_Cursor INTO  @currency_id1  
    	END  
    	  
    	CLOSE Table2_Cursor  
    	DEALLOCATE  Table2_Cursor  
    	  
    	SELECT  SUM(SUM_AMOUNT) As SumAmount,
                Currency.code AS CurrencyCode,
                NULL as FloatBalance , 
                NULL as Overdraft,
				currency.currency_id  
    	FROM #table2  
    	INNER JOIN currency 
    		ON currency.currency_id = @Client_Currency
        GROUP BY Currency.code, currency.currency_id 
    END  
	ELSE  
	BEGIN  
	    SELECT  Sum_amount AS SumAmount,
                Currency.code AS CurrencyCode,
                FloatBalance ,
                Overdraft,
				currency.currency_id  
	    FROM #table2  
	    INNER JOIN currency 
		    ON currency.currency_id =#Table2.currency_ID 
        
	END  
	  
	DROP TABLE #table2  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO