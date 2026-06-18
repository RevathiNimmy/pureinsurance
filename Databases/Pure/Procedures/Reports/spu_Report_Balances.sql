SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Balances'
GO


CREATE PROCEDURE spu_Report_Balances
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(11) = NULL
    
AS

    SET NOCOUNT ON

	IF @branch_id = 0
	BEGIN
    	SELECT @branch_id = NULL
    END
    
    IF ISNULL(@date_type,'') = ''
	BEGIN
	    SELECT @branch_id = 'Transaction'
    END
    
    CREATE TABLE #TransLines1 
    (
        amount MONEY NULL,
        ledger_id SMALLINT NULL,
        ledger_name VARCHAR(30) NULL,
        code CHAR(10) NULL,
        adjustment CHAR(3) NULL,
    )
    CREATE INDEX Cover ON #TransLines1(ledger_id, ledger_name, code, adjustment, amount)

    CREATE TABLE #TransLines2 
    (
        amount MONEY NULL,
        ledger_id SMALLINT NULL,
        ledger_name VARCHAR(30) NULL,
        code CHAR(10) NULL,
        adjustment CHAR(3) NULL,
    )
    CREATE INDEX Cover ON #TransLines2(ledger_id, ledger_name, code, adjustment, amount)

    INSERT INTO #TransLines1
	SELECT 
		T.amount, 
		L.ledger_id,
		L.ledger_name,
		DT.code,
		(
			CASE 
				WHEN EXISTS 
					(
						SELECT NULL
						FROM TransDetail T1
						WHERE T1.transdetail_id = T.transdetail_id
                        AND T1.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
					) THEN 'YES' 
				ELSE 'NO' 
			END
		) AS Adjustment
	FROM Transdetail T
	JOIN Account A 
		ON A.account_id = T.account_id
	JOIN Document D 
		ON D.document_id = T.document_id
	JOIN DocumentType DT 
		ON DT.documenttype_id = D.documenttype_id
	JOIN Ledger L 
		ON L.ledger_id = A.ledger_id
	WHERE D.company_id = ISNULL(@branch_id, D.company_id)
	AND
	(
		(
			(
				(
					@date_type = 'Transaction'
					AND
					D.document_date BETWEEN @start_date AND @end_date
				)
				OR
				(
					@date_type = 'Effective'
					AND
					T.ref_date BETWEEN @start_date AND @end_date
				)
			)
            AND T.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
		)
		OR
		(
			(
				(
					@date_type = 'Transaction'
					AND
					T.ref_date BETWEEN @start_date AND @end_date
				)
				OR
				(
					@date_type = 'Effective'
					AND
					(
						SELECT ref_date
						FROM transdetail
						WHERE document_id = D.document_id
						AND document_sequence = 1
					) BETWEEN @start_date AND @end_date
				)
			)
            AND T.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
		)
	)

    INSERT INTO #TransLines1
	SELECT 
		0.0,
		L.ledger_id,
		L.ledger_name,
		'',
		'NO'
	FROM Ledger L
	WHERE L.ledger_id NOT IN (SELECT ledger_id FROM #TransLines1)

    INSERT INTO #TransLines2
	SELECT 
		SUM(ROUND(amount,2)), 
		ledger_id, 
		ledger_name, 
		code, 
		adjustment 
	FROM #TransLines1
	GROUP BY 
		ledger_id, 
		ledger_name, 
		code, 
		adjustment

    SET NOCOUNT OFF
    
    SELECT
        amount,
        ledger_id,
        ledger_name,
        code,
        adjustment,
        ISNULL(
        (
            SELECT SUM(ROUND(amount,2))
            FROM Transdetail T1
            JOIN Document D1 
            	ON D1.document_id = T1.document_id
            JOIN Account A1 
            	ON A1.account_id = T1.account_id
            WHERE A1.ledger_id = TL2.ledger_id
            AND D1.company_id = ISNULL(@branch_id, D1.company_id)
            AND
            (
                (
                    (
                        (
                        	@date_type = 'Transaction'
                        	AND 
                        	D1.document_date < @start_date 
                        )
                        OR
                        (
                        	@date_type = 'Effective'
                        	AND 
                        	T1.ref_date < @start_date 
                        )
                    )
                    AND T1.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
                OR
                (
					(
						(
							@date_type = 'Transaction'
							AND
							T1.ref_date < @start_date
						)
						OR
						(
							@date_type = 'Effective'
							AND
							(
								SELECT ref_date
								FROM transdetail
								WHERE document_id = D1.document_id
								AND document_sequence = 1
							) < @start_date 
						)
                    )
                    AND T1.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
            )
        ), 0) AS opening_balance
	FROM #TransLines2 TL2
	
    SET NOCOUNT ON

    DROP TABLE #TransLines1
    DROP TABLE #TransLines2
    
GO
