SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Outstanding_Transaction'
GO

CREATE PROCEDURE spu_Report_Outstanding_Transaction
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type	VARCHAR(50),
    @group_by VARCHAR(50),
    @then_by VARCHAR(50)
AS

DECLARE @client_shortname CHAR(20)
DECLARE @client_name VARCHAR(255)
DECLARE @effective_date DATETIME
DECLARE @policy_number VARCHAR(30)
DECLARE @document_id INT
DECLARE @document_ref VARCHAR(25)
DECLARE @transdetail_id INT
DECLARE @transaction_amount MONEY
DECLARE @transdetail_account_id INT
DECLARE @settled_amount MONEY
DECLARE @SubAgent_transdetail_id INT
DECLARE @SubAgent_amount MONEY
DECLARE @SubAgent_settled MONEY
DECLARE @Plan_Transdetail_ID INT
DECLARE @Amount_To_Finance MONEY
DECLARE @Total_Cost MONEY
DECLARE @Client_Transaction_Amount MONEY
DECLARE @Client_Percentage MONEY
DECLARE @Finance_Rate MONEY     
DECLARE @Deposit_Transdetail_ID INT
DECLARE @Deposit_Amount MONEY
DECLARE @Finance_transdetail_id INT 
DECLARE @Transaction_Section INT 

/*Validate input parameters*/
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @group_by ='None' OR @group_by=''
	BEGIN
		SELECT @then_by= 'None'
	END

IF @then_by= @group_by
	BEGIN
		SELECT @then_by= 'None'
	END

IF @date_type =''  OR @date_type=NULL
BEGIN
   SELECT @date_type='Transaction Date'
END

CREATE TABLE #ClientTransactionsTemp
(
    client_shortname CHAR(20),
    client_name VARCHAR(255),
    effective_date DATETIME,
    transaction_date DATETIME,
    policy_number VARCHAR(30),
    document_id INT,
    document_ref VARCHAR(25),
    transdetail_id INT, 
    transaction_amount MONEY,
    transdetail_account_id INT,
    settled_amount MONEY,
    fsa_disabled BIT,
    Transaction_Section INT
)

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    
    INSERT INTO #ClientTransactionsTemp
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT 
        *,'' as group_code, '' as group_desc,'' as then_code,'' as then_desc 
    FROM #ClientTransactionsTemp
    
    DROP TABLE #ClientTransactionsTemp
        
    RETURN  
END

INSERT INTO #ClientTransactionsTemp
SELECT 
    p.shortname,  
    p.resolved_name,
    t.ref_date,
    d.document_date,
    --ISNULL(tef.cover_start_date,D.document_date),
    (select insurance_ref from  insurance_file i
    where i.insurance_file_cnt = d.insurance_file_cnt),
    d.document_id,
    d.document_ref,
    t.transdetail_id, 
    t.currency_amount,
    t.account_id,
    0,
    NULL,
    1
FROM document d
JOIN transdetail T
    ON t.document_id = d.document_id
JOIN account a
    ON a.account_id = t.account_id
JOIN party p
    ON p.party_cnt = a.account_key  
WHERE d.documenttype_id IN (4,5,15,16,17,18,31,32,35,36,30) /*SND,SNC,SRD,SRC,SED,SEC,SHD,SHC,TRD,TRC,FEE*/
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND
    (
       (
	@date_type = 'Transaction Date'
	AND
	d.document_date BETWEEN @start_date AND @end_date
      )
      OR
      (
	@date_type = 'Effective Date'
	AND
	t.ref_date BETWEEN @start_date AND @end_date
      )
    )
AND t.document_sequence=1 
ORDER BY 
     d.document_date 
--Main client will always be the client transdetail record 
--with the lowest sequence in the transaction(doc sequence=1)
--------------------------------

DECLARE ctt_cursor CURSOR FAST_FORWARD FOR
    SELECT
        client_shortname,
        client_name,
        effective_date,
        policy_number,
        document_id,
        document_ref,
        transdetail_id, 
        transaction_amount,
        transdetail_account_id,
        settled_amount,
	Transaction_Section
    FROM #ClientTransactionsTemp

OPEN ctt_cursor

FETCH NEXT FROM ctt_cursor INTO
    @client_shortname,
    @client_name,
    @effective_date,
    @policy_number,
    @document_id,
    @document_ref,
    @transdetail_id,
    @transaction_amount,
    @transdetail_account_id,
    @settled_amount,
    @Transaction_Section 
    
WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @Subagent_transdetail_id = 0
    SELECT @Plan_Transdetail_ID = 0
    SELECT @Finance_Transdetail_ID = 0
    SELECT @Deposit_Transdetail_id = 0
    
    SELECT 
        @settled_amount = ISNULL(SUM(currency_match_amount),0)
    FROM transmatch
    WHERE transdetail_id =  @transdetail_id
     
         
    /* Check SubAgent */
    SELECT 
        @subagent_transdetail_id = ISNULL(t.transdetail_id,0)  
    FROM document d  
    JOIN transdetail t 
        ON t.document_id = d.document_id
    JOIN account a 
        ON a.account_id = t.account_id
    JOIN ledger l 
        ON l.ledger_id = a.ledger_id
    WHERE l.ledger_short_name = 'UB'
    AND d.Document_id = @document_id
    AND (
            SELECT SUM(currency_amount) 
            FROM transdetail  
            WHERE document_id = d.document_id
            AND account_id = @transdetail_account_id
        ) = 0            
    
    IF @subagent_transdetail_id <> 0
    BEGIN
        SELECT 
            @subagent_amount = currency_amount 
        FROM transdetail
        WHERE transdetail_id = @subagent_transdetail_id

        SELECT 
            @Settled_amount = ISNULL(SUM(currency_match_amount),0) 
        FROM transmatch
        WHERE transdetail_id = @subagent_transdetail_id
        
        SELECT @Transaction_Section =2 
    END
    
    /* In House Instalment */
    
    SELECT  
        @Plan_Transdetail_ID = ISNULL(PFPF.PlanTransaction_id,0),
        @Amount_To_Finance= ISNULL(PFPF.AmountToFinance,0),
        @Total_Cost = ISNULL(PFPF.totalcost,0) + ISNULL(PFPF.deposit,0) 
    FROM PFTransaction_Id PFT
    JOIN PFPremiumFinance PFPF 
        ON PFPF.pfprem_finance_cnt = PFT.pfprem_finance_cnt
    JOIN PFScheme PFS 
        ON PFS.CompanyNo = PFPF.CompanyNo
        AND PFS.SchemeNo = PFPF.SchemeNo
        AND PFS.SchemeVersion = PFPF.SchemeVersion
    JOIN PFScheme_type PFST 
        ON PFST.PFSCheme_type_id = PFS.PFScheme_type_id
        AND PFPF.pfprem_finance_version = PFT.pfprem_finance_version
    WHERE PFT.pftransaction_id = @Transdetail_Id
    AND PFST.code = 'IH'
 
     
    IF @Plan_transdetail_id <> 0
    BEGIN
         
        SELECT  
            @Client_Percentage = (T.currency_amount / @Amount_To_Finance),
            @Client_Transaction_Amount = T.currency_amount   
        FROM TransDetail T 
        WHERE T.transdetail_id = @Transdetail_ID

        SELECT @Finance_Rate = ((@Total_Cost - @Amount_To_Finance) * 100 / @Amount_To_Finance )+ 100

        SELECT 
            @Deposit_Transdetail_ID = ISNULL(t2.transdetail_id,0)
        FROM transmatch tm
        JOIN transmatch tm2 
            ON tm2.match_id = tm.match_id
        JOIN transdetail t 
            ON t.transdetail_id = tm2.transdetail_id
        JOIN transdetail t2 
            ON t.document_id = t2.document_id
        WHERE tm.transdetail_id = @Transdetail_Id
        AND t.comment = 'Deposit'
        AND t2.transdetail_id <> t.transdetail_id
        
        IF @Total_Cost = 
            (
                SELECT 
                    ISNULL(SUM(tm.currency_match_amount) ,0) 
                FROM transdetail t
                JOIN transmatch tm
                    ON tm.transdetail_id = t.transdetail_id
                WHERE t.transdetail_id IN (@Plan_Transdetail_Id, @Deposit_Transdetail_Id)
                AND tm.match_id NOT IN 
                    (
                        SELECT match_id 
                        FROM transmatch
                        WHERE transdetail_id = @Transdetail_Id
                    )
            )
        BEGIN         
            SELECT @settled_amount = @transaction_amount
        END
        ELSE
        BEGIN    
            /*Sum up the allocations excluding the deposit against the original debit for in-house*/
        
            SELECT 
                @settled_amount = ROUND((ISNULL(SUM(tm.currency_match_amount) ,0) * @Client_Percentage / @Finance_Rate) * 100,2)
            FROM transdetail t
            JOIN transmatch tm
                ON tm.transdetail_id = t.transdetail_id
            WHERE t.transdetail_id IN (@Plan_Transdetail_Id, @Deposit_Transdetail_Id)
            AND tm.match_id NOT IN
                (
                    SELECT match_id 
                    FROM transmatch
                    WHERE transdetail_id = @Transdetail_Id
                )
        END
    END


    /* Premium Finance*/
     
    SELECT
        @Finance_Transdetail_ID = ISNULL(PFPF.PlanTransaction_id,0) 
    FROM PFTransaction_Id PFT
    JOIN PFPremiumFinance PFPF 
        ON PFPF.pfprem_finance_cnt = PFT.pfprem_finance_cnt
    JOIN PFScheme PFS 
        ON PFS.CompanyNo = PFPF.CompanyNo
        AND PFS.SchemeNo = PFPF.SchemeNo
        AND PFS.SchemeVersion = PFPF.SchemeVersion
    JOIN PFScheme_type PFST 
        ON PFST.PFSCheme_type_id = PFS.PFScheme_type_id
        AND PFPF.pfprem_finance_version = PFT.pfprem_finance_version
    WHERE PFT.pftransaction_id = @Transdetail_Id
    AND PFST.code <> 'IH'
    
    IF @Finance_transdetail_id <> 0
    BEGIN        
        SELECT 
            @Deposit_Transdetail_ID = ISNULL(t6.transdetail_id,0),
            @Deposit_Amount =  ISNULL(t6.currency_amount,0)
        FROM transdetail t1 
        JOIN account a
            ON a.account_id = t1.account_id
        JOIN transmatch tm1 
            ON tm1.transdetail_id = t1.transdetail_id
        JOIN transmatch tm2 
            ON tm1.match_id = tm2.match_id
        JOIN transdetail t3 
            ON tm2.transdetail_id = t3.transdetail_id
        JOIN document d1 
            ON t3.document_id = d1.document_id
        JOIN transdetail t4 
            ON t4.document_id = d1.document_id
        JOIN transmatch tm3 
            ON tm3.transdetail_id = t4.transdetail_id
        JOIN transmatch tm4 
            ON tm4.match_id = tm3.match_id
        JOIN transdetail t5 
            ON t5.transdetail_id = tm4.transdetail_id
        JOIN document d2 
            ON d2.document_id = t5.document_id
        JOIN transdetail t6 
            ON t6.document_id = t5.document_id
        WHERE t1.transdetail_id = @Transdetail_ID
        AND tm1.transdetail_id <> tm2.transdetail_id
        AND t4.transdetail_id <> t3.transdetail_id
        AND t5.transdetail_id <> t4.transdetail_id
        AND t6.transdetail_id <> t5.transdetail_id
        AND t6.account_id = t1.account_id
        
        IF @Deposit_Transdetail_ID <> 0
        BEGIN
            SELECT 
                @settled_amount = @settled_amount - @Deposit_Amount + ISNULL(SUM(currency_match_amount),0) 
            FROM transmatch
            WHERE transdetail_id = @Deposit_Transdetail_ID
        END             
	SELECT @Transaction_Section =3
    END
          
    UPDATE #ClientTransactionsTemp
    SET settled_amount = @settled_amount,
	Transaction_Section =  @Transaction_Section   
    WHERE transdetail_id = @transdetail_id

    IF @Transaction_Section = 2
    BEGIN 
        UPDATE #ClientTransactionsTemp
        SET transaction_amount = @subagent_amount
        WHERE transdetail_id = @transdetail_id 
    END    

    /* Check Instalment */
    FETCH NEXT FROM ctt_cursor INTO
        @client_shortname,
        @client_name,
        @effective_date,
        @policy_number,
        @document_id,
        @document_ref,
        @transdetail_id,
        @transaction_amount,
        @transdetail_account_id,
        @settled_amount, 
	@Transaction_Section
END

CLOSE ctt_cursor
DEALLOCATE ctt_cursor
 

/*Delete fully paid transactions */
DELETE 
FROM #ClientTransactionsTemp
WHERE transaction_amount = settled_amount

SELECT
    t.*,
    (
        CASE @group_by
	    WHEN 'None' Then
		''
            WHEN 'Branch' THEN
                (
                    SELECT ISNULL(S.code,'')
                    FROM document D
		    JOIN Source S
			ON S.source_id=D.company_id
		    WHERE D.document_id=t.document_id
                )
	    WHEN 'Client' THEN
		t.client_shortname
	    WHEN 'Transaction Type' THEN
		  (
		     SELECT
		     (
        		SELECT CASE
        			WHEN D.documenttype_id IN (4,5,30) THEN 'New Business'
        			WHEN D.documenttype_id IN (15,16) THEN 'Renewals'
        			WHEN D.documenttype_id IN (17,18) THEN 'Adjustments'
        			WHEN D.documenttype_id IN (31,32) THEN 'Short Term'
        			WHEN D.documenttype_id IN (35,36) THEN 'Transfers'
        		END
		      )
		      FROM document D
		      WHERE D.document_id=t.document_id 
    		  )
	    WHEN 'Transaction Date' THEN
	    	ISNULL(CONVERT (varchar,t.transaction_date,103 ),'')
	    WHEN 'Effective Date' THEN
		ISNULL(CONVERT (varchar,t.effective_date,103 ),'')
	END	
     )group_code,	
     (
        CASE @group_by
	    WHEN 'None' Then
		''
            WHEN 'Branch' THEN   
                (
                    SELECT ISNULL(S.description,'')
                    FROM document D
		    JOIN Source S
			ON S.source_id=D.company_id
		    WHERE D.document_id=t.document_id
                )
	    WHEN 'Client' THEN
		t.client_name
	    WHEN 'Transaction Type' THEN
		''
	    WHEN 'Transaction Date' THEN
		ISNULL(CONVERT (varchar,t.transaction_date,103 ),'')
	    WHEN 'Effective Date' THEN
		ISNULL(CONVERT (varchar,t.effective_date,103 ),'')
	END	
     )group_desc,
--------------------------------------
    (
        CASE @then_by
	    WHEN 'None' Then
		''
            WHEN 'Branch' THEN
                (
                    SELECT ISNULL(S.code,'')
                    FROM document D
		    JOIN Source S
			ON S.source_id=D.company_id
		    WHERE D.document_id=t.document_id
                )
	    WHEN 'Client' THEN
		t.client_shortname
	    WHEN 'Transaction Type' THEN
		  (
		     SELECT
		     (
        		SELECT CASE
        			WHEN D.documenttype_id IN (4,5,30) THEN 'New Business'
        			WHEN D.documenttype_id IN (15,16) THEN 'Renewals'
        			WHEN D.documenttype_id IN (17,18) THEN 'Adjustments'
        			WHEN D.documenttype_id IN (31,32) THEN 'Short Term'
        			WHEN D.documenttype_id IN (35,36) THEN 'Transfers'
        		END
		      )
		      FROM document D
		      WHERE D.document_id=t.document_id 
    		  )
	     WHEN 'Transaction Date' THEN
	     	ISNULL(CONVERT (varchar,t.transaction_date,103 ),'')
	     WHEN 'Effective Date' THEN
		ISNULL(CONVERT (varchar,t.effective_date,103 ),'')
         END	
     )then_code,	
     (
        CASE @then_by
	    WHEN 'None' Then
		''
            WHEN 'Branch' THEN   
                (
                    SELECT ISNULL(S.description,'')
                    FROM document D
		    JOIN Source S
			ON S.source_id=D.company_id
		    WHERE D.document_id=t.document_id
                )
	    WHEN 'Client' THEN
		t.client_name
	    WHEN 'Transaction Type' THEN
		''
	    WHEN 'Transaction Date' THEN
		ISNULL(CONVERT (varchar,t.transaction_date,103 ),'')
	    WHEN 'Effective Date' THEN
		ISNULL(CONVERT (varchar,t.effective_date,103 ),'')
	END	
     )then_desc
	
	
FROM #ClientTransactionsTemp t 

DROP TABLE #ClientTransactionsTemp

GO
