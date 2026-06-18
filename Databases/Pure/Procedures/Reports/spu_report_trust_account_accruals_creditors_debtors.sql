SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_report_trust_account_accruals_creditors_debtors'
GO


CREATE PROCEDURE spu_report_trust_account_accruals_creditors_debtors
    @end_date DATETIME,
    @branch_id INT,
    @insurer_debt MONEY OUTPUT,
    @insurer_credit MONEY OUTPUT,
    @client_debt MONEY OUTPUT,
    @client_credit MONEY OUTPUT 
AS

    DECLARE
        @ledger_min INT, 
        @ledger_max INT,
        @multi_company_id INT

    DECLARE
	@c_extra_int2 INT,
	@c_extra_int1 INT,
	@c_account_id INT 
         

    SET NOCOUNT ON
    
    /*Check the input parameters.*/
  
    IF @branch_id = 0
    BEGIN
        SELECT @branch_id = NULL
    END

    SELECT @end_date = ISNULL(@end_date, GETDATE())
   
    SELECT @multi_company_id=0
    SELECT @multi_company_id=value
    FROM Hidden_options
    WHERE option_number=16 AND branch_id=1

    IF (@multi_company_id=1) BEGIN
        IF @branch_id=0 BEGIN
            SELECT @multi_company_id=1
            SELECT @branch_id=1
        END
        ELSE BEGIN
            SELECT @multi_company_id=@branch_id
        END
    END
    ELSE
        SELECT @multi_company_id=1
 
 
  

    -- Create the transaction table
    CREATE TABLE #Report_Transaction
    (
        transdetail_id INT,
        amount MONEY,
        document_sequence SMALLINT,
        policy_number VARCHAR(30),
        branch_id INT,
        comment VARCHAR(60),
        document_ref VARCHAR(25),
        document_date DATETIME,
        documenttype_id INT,
        account_id INT,
        account_code CHAR(30),
        account_name VARCHAR(100),
        account_type VARCHAR(100),
        ledger_type VARCHAR(100),
        branch_name VARCHAR(100),
        period_id INT,
        record_type SMALLINT,
        transdetail_id2 INT,
        amount2 MONEY,
        document_sequence2 SMALLINT,
        policy_number2 VARCHAR (30),
        branch_id2 INT,
        comment2 VARCHAR (60),
        account_id2 INT,
        account_code2 CHAR (30),
        account_name2 VARCHAR (100),
        account_type2 VARCHAR (100),
        ledger_type2 VARCHAR (100),
        branch_name2 VARCHAR (100),
        period_id2 INT,
        record_type2 SMALLINT,
        extra_char1 VARCHAR (100),
        extra_char2 VARCHAR (255),
        extra_char3 VARCHAR (100),
        extra_char4 VARCHAR (100),
        extra_char5 VARCHAR (100),
        extra_char6 VARCHAR (100),
        extra_char7 VARCHAR (100),
        extra_int1 INT,
        extra_int2 INT,
        extra_int3 INT,
        extra_int4 INT,
        extra_int5 INT,
        extra_int6 INT,
        extar_int7 INT,
        extra_datetime1 DATETIME,
        extra_datetime2 DATETIME,
        extra_datetime3 DATETIME,
        extra_datetime4 DATETIME,
        extra_datetime5 DATETIME,
        extra_datetime6 DATETIME,
        extra_datetime7 DATETIME,
        extra_numeric1 MONEY,
        extra_numeric2 MONEY,
        extra_numeric3 MONEY,
        extra_numeric4 MONEY,
        extra_numeric5 MONEY,
        extra_numeric6 MONEY,
        extra_numeric7 MONEY
    )

/* Get Insurer Ledgers */

   SELECT @ledger_min=ledger_id 
        FROM ledger 
        WHERE ledger_short_name = 'IN'
        AND company_id=@multi_company_id

    SELECT @ledger_max=@ledger_min
    -- Get the required transactions
    INSERT INTO #Report_Transaction 
    (
        transdetail_id,     /* TransDetail.transdetail_id */
        account_id,         /* Account.account_id */
        documenttype_id,    /* Document.documenttype_id */
        document_date,      /* Document.document_date */
        extra_datetime1,    /* TransDetail.ref_date */
        amount,              /* TransDetail.amount */
	extra_int1,	     /* Document Id */
	extra_int2	     /* Document Sequence */
    )
    SELECT
        TD.transdetail_id,
        A.account_id,
        D.documenttype_id,
        D.document_date,
        TD.ref_date,
        ROUND(TD.outstanding_amount,2),
	D.document_id,
	TD.document_sequence
    FROM Account A
    JOIN TransDetail TD
        ON TD.account_id = A.account_id
    JOIN Document D
        ON D.document_id = TD.document_id
    WHERE A.ledger_id BETWEEN @ledger_min AND @ledger_max
    AND D.company_id = ISNULL(@branch_id, D.company_id)
    AND D.documenttype_id <> 23 --Discount insurer payments
    AND TD.spare NOT LIKE 'Revers%'
    AND
    (
        (
            D.document_date <= @end_date
            AND NOT
            (
                TD.spare IN ('COMM ADJ', 'AGENT ADJ')
                OR
                TD.document_sequence IN (
                    SELECT document_sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
            )
        )
        OR
        (
            TD.ref_date <= @end_date
            AND
            (
                TD.spare IN ('COMM ADJ', 'AGENT ADJ')
                OR
                TD.document_sequence IN (
                    SELECT document_sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
            )
        )
    )
 
    UPDATE RT
    SET RT.extra_numeric3 = 
             (
                SELECT SUM(ROUND(amount,2))
                FROM #Report_Transaction
                WHERE account_id = RT.account_id
  	    ) 

     FROM #Report_Transaction RT
    
     DELETE FROM #Report_Transaction
        	WHERE extra_numeric3 = 0

     UPDATE RT
     SET RT.extra_numeric1 = 
 
             (
                 SELECT SUM(ROUND(amount,2))
                 FROM #Report_Transaction  
                 WHERE account_id = RT.account_id
		 AND extra_int1 = RT.extra_int1
	     )
     FROM #Report_Transaction RT
/*Temp*/
--SELECT * FROM #Report_Transaction
/*End Temp */
DECLARE c_transactions CURSOR FORWARD_ONLY STATIC FOR
	SELECT MIN(extra_int2),extra_int1,account_id 
	FROM #Report_Transaction RT 
	GROUP BY account_id,extra_int1
OPEN c_transactions
FETCH NEXT FROM c_transactions INTO
	@c_extra_int2,
        @c_extra_int1,
	@c_account_id
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM #Report_Transaction
		WHERE extra_int1 = @c_extra_int1
		AND account_id = @c_account_id
		AND extra_int2 <> @c_extra_int2 
	FETCH NEXT FROM c_transactions INTO
		@c_extra_int2,
        	@c_extra_int1,
		@c_account_id
END

CLOSE c_transactions
DEALLOCATE c_transactions

/*Temp*/
--SELECT account_id,extra_int1,extra_int2,extra_numeric1,extra_numeric3,extra_numeric5,extra_numeric6 FROM #Report_Transaction
/*End Temp */

     UPDATE RT
     SET RT.extra_numeric5 =
	    ISNULL((
                 SELECT SUM(ROUND(extra_numeric1,2))
                 FROM #Report_Transaction  
                 WHERE account_id = RT.account_id
		 AND extra_int1 = RT.extra_int1
                 AND extra_numeric1 < 0
             ),0), /*Account_Total_CR*/
         RT.extra_numeric6 = 
	    ISNULL((
                 SELECT SUM(ROUND(extra_numeric1,2))
                 FROM #Report_Transaction  
                 WHERE account_id = RT.account_id
		 AND extra_int1 = RT.extra_int1
                 AND extra_numeric1 > 0
             ),0) /*Account_Total_DR*/


    FROM #Report_Transaction RT
 
 /*Temp*/
--select extra_int1,extra_int2,extra_numeric1,extra_numeric3,extra_numeric5,extra_numeric6 FROM #Report_Transaction RT
/*End Temp */      
    SELECT @insurer_credit = ISNULL((SELECT SUM(ISNULL(RT.extra_numeric5,0)) 
					FROM #Report_Transaction RT
					WHERE RT.transdetail_id = 
						(SELECT MIN(transdetail_id) from #Report_Transaction
							where account_id = RT.account_id
							and extra_int1 = RT.extra_int1)
			      ),0)	
    
    SELECT @insurer_debt = ISNULL((SELECT SUM(ISNULL(RT.extra_numeric6,0)) 
					FROM #Report_Transaction RT
					WHERE RT.transdetail_id = 
						(SELECT MIN(transdetail_id) from #Report_Transaction
							where account_id = RT.account_id
							and  extra_int1 = RT.extra_int1)
			    ),0)	 
			       
    DELETE FROM  #Report_Transaction

/* Get Client Ledgers */
   SELECT @ledger_min=ledger_id 
        FROM ledger 
        WHERE ledger_short_name = 'SA'
        AND company_id=@multi_company_id

    SELECT @ledger_max=@ledger_min
    -- Get the required transactions
    INSERT INTO #Report_Transaction 
    (
        transdetail_id,     /* TransDetail.transdetail_id */
        account_id,         /* Account.account_id */
        documenttype_id,    /* Document.documenttype_id */
        document_date,      /* Document.document_date */
        extra_datetime1,    /* TransDetail.ref_date */
        amount              /* TransDetail.amount */
    )
    SELECT
        TD.transdetail_id,
        A.account_id,
        D.documenttype_id,
        D.document_date,
        TD.ref_date,
        ROUND(TD.Amount,2)
    FROM Account A
    JOIN TransDetail TD
        ON TD.account_id = A.account_id
    JOIN Document D
        ON D.document_id = TD.document_id
    WHERE A.ledger_id BETWEEN @ledger_min AND @ledger_max
    AND D.company_id = ISNULL(@branch_id, D.company_id)
    AND TD.spare NOT LIKE 'Revers%'
    AND
    (
        (
            D.document_date <= @end_date
            AND NOT
            (
                TD.spare IN ('COMM ADJ', 'AGENT ADJ')
                OR
                TD.document_sequence IN (
                    SELECT document_sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
            )
        )
        OR
        (
            TD.ref_date <= @end_date
            AND
            (
                TD.spare IN ('COMM ADJ', 'AGENT ADJ')
                OR
                TD.document_sequence IN (
                    SELECT document_sequence + 1
                    FROM TransDetail
                    WHERE document_id = D.document_id
                    AND spare = 'COMM ADJ'
                )
            )
        )
    )

 

      UPDATE RT
      SET RT.extra_numeric3 = 
            ISNULL((
                SELECT SUM(ROUND(amount,2))
                FROM #Report_Transaction
                WHERE account_id = RT.account_id
            ),0), /*account_total*/
         RT.extra_numeric5 = 
             ISNULL((
                 SELECT SUM(ROUND(amount,2))
                 FROM #Report_Transaction
                 WHERE account_id = RT.account_id
                 AND amount < 0
             ),0), /*Account_Total_CR*/
         RT.extra_numeric6 = 
             ISNULL((
                SELECT SUM(ROUND(amount,2))
                FROM #Report_Transaction
                WHERE account_id = RT.account_id
                AND amount > 0
            ),0)
    	FROM #Report_Transaction RT
	
     DELETE FROM #Report_Transaction
        WHERE extra_numeric3 = 0
       
      SELECT @client_credit = ISNULL((SELECT SUM(ISNULL(RT.extra_numeric5,0)) 
					FROM #Report_Transaction RT
					WHERE RT.transdetail_id = 
						(SELECT MIN(transdetail_id) from #Report_Transaction
							where account_id = RT.account_id)
			      ),0)
    
      SELECT @client_debt = ISNULL((SELECT SUM(ISNULL(RT.extra_numeric6,0)) 
					FROM #Report_Transaction RT
					WHERE RT.transdetail_id = 
						(SELECT MIN(transdetail_id) from #Report_Transaction
							where account_id = RT.account_id)
			    ),0)	 
       
 
    SET NOCOUNT OFF

    DROP TABLE #Report_Transaction
    
GO
