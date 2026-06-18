SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Journals'
GO

CREATE PROCEDURE spu_Report_Audit_Journals

    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @ledger_type varchar(20)

AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @ledger_type = 'ALL'
BEGIN
	SELECT @ledger_type = ''
END

SELECT 
    D.document_ref,
    D.document_date,
    A.short_code,
    (
        CASE L.ledger_short_name
            WHEN 'NO' THEN 1
            WHEN 'SA' THEN 2
            WHEN 'PU' THEN 3
            WHEN 'IN' THEN 4 
            WHEN 'AG' THEN 5
            WHEN 'RF' THEN 6
            WHEN 'FE' THEN 7 
            WHEN 'DI' THEN 8
            WHEN 'CO' THEN 9
            WHEN 'UB' THEN 10
            WHEN 'TR' THEN 11
            ELSE 0 
        END
    ) 'Ledger',
    T.transdetail_id 'TransdetailID',
    T.amount 'Amount',
    (
        SELECT ISNULL(MAX(short_code),'')
        FROM Account A1
        JOIN BankAccount B
            ON B.account_id = A1.account_id
        WHERE A1.short_code = A.short_code
    ) 'BankAccountCode',
    C.company_id 'BranchID',
    C.code 'BranchCode',
    C.description 'BranchDesc',
    D.Comment
FROM TransDetail T
JOIN Document D
    ON T.document_id = D.document_id
JOIN Account A
    ON T.account_id = A.account_id
JOIN Ledger L
    ON L.ledger_id = A.ledger_id
JOIN Company C
    ON C.company_id = D.company_id
WHERE D.documenttype_id IN (1, 8, 10, 11, 12, 20, 21)
AND D.document_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND (@ledger_type = '' OR 
	( @ledger_type <> '' AND EXISTS 	(	
			SELECT * 
			FROM transdetail td2 
			JOIN account a2 on td2.account_id = a2.account_id
			JOIN ledger l2 on l2.ledger_id = a2.ledger_id
			WHERE td2.document_id = D.document_id
			AND rtrim(l2.ledger_Name) = rtrim(@ledger_type)
			)
	))
		
ORDER BY 
    C.company_id,
    D.document_ref, 
    A.short_code


GO


