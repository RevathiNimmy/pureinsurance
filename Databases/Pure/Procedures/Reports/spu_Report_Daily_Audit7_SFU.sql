SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit7_SFU'
GO
CREATE PROCEDURE spu_Report_Daily_Audit7_SFU
    -- TF030500
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @TypeOfCurrency	Varchar(50),
    @GroupByCode	Varchar(50),
    @Media_Type         Varchar(50)
    
AS

-- TF030500
DECLARE @iBranchID     int
DECLARE @MediaType_id  int 

	SELECT @MediaType_id = mediaType_id From MediaType WHERE [Description] = @Media_type
	SELECT @MediaType_id = ISNULL(@MediaType_id, 0)

SELECT @iBranchID = ISNULL(@branch_id, 0)
/*Get System Currency Details*/
DECLARE @SystemCurrencyCode varchar(15)
DECLARE @SystemCurrencyDesc varchar(255)
	SELECT
		@SystemCurrencyCode = c.iso_code,
		@SystemCurrencyDesc = c.description
	FROM PMSystem pms
	JOIN currency c
		ON c.currency_id = pms.currency_id
	WHERE pms.system_id = 1
/*******************/
IF @iBranchID = 0
BEGIN
    SELECT D.document_ref,
        D.document_date,
        A.short_code acc_code,
--        A.ledger_id ledger,
        (CASE l.ledger_short_name
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
         ELSE 0 END) ledger,
        T.transdetail_id transdetail_id,
        --T.amount amount,
        Case @TypeOfCurrency 
			When 'System' Then ISNULL(ROUND(ISNULL(T.system_amount, 0.0),2), 0.0)
			When 'Base' Then ISNULL(ROUND(ISNULL(T.amount, 0.0),2), 0.0)
    	END Amount,
        T.company_id branch_id,C.Code CompanyCode,
        C.description branch,
		CASE @TypeOfCurrency
			WHEN 'Base' THEN cb.iso_code
			WHEN 'System' THEN @SystemCurrencyCode
		END CurrencyCode,
		CASE @TypeOfCurrency
			WHEN 'Base' THEN cb.description
			WHEN 'System' THEN @SystemCurrencyDesc
		END CurrencyDesc, 
        ISNULL( ( SELECT TOP 1 short_code
                FROM Account A1
                JOIN BankAccount B
                    ON B.account_id = A1.account_id
                WHERE A1.short_code = A.short_code
        ), '') bank_account,
        Case @GroupByCode WHEN 'Branch' Then C.company_id
				WHEN 'Branch And Company' Then C.Company_id
				ELSE ' ' 
		END 'GroupByCode',
         CLRR.Description as Cancellation_Reason,
         MT.Description as Media_Type,
         CLI.media_ref as Media_Ref,
		CLI.payment_name
    FROM TransDetail T
    JOIN Document D
        ON T.document_id = D.document_id
    JOIN Account A
        ON T.Account_id = A.Account_id
    JOIN Company C
        ON C.company_id = T.company_id
    JOIN Currency CB
	ON CB.Currency_id= C.Base_currency
    JOIN Ledger L
        ON L.Ledger_Id = A.Ledger_Id       
    LEFT JOIN CashListItem CLI
        ON CLI.cashlistitem_reversal_transdetail_id=T.Transdetail_id       
	AND (@mediatype_id =0 OR CLI.mediatype_id=@mediatype_id)
    LEFT JOIN MediaType MT
        ON CLI.mediatype_id = MT.mediatype_id
    LEFT JOIN cashlistitem_reverse_reason CLRR
	ON CLI.cashlistitem_reverse_reason_id = CLRR.cashlistitem_reverse_reason_id

    WHERE D.documenttype_id IN (1, 8, 10, 11, 12, 20, 21)
    AND ( D.document_date >= @start_date
            AND
            D.document_date <= @end_date
        ) 
    
    ORDER BY document_ref, acc_code
END
ELSE
BEGIN
    SELECT D.document_ref,
        D.document_date,
        A.short_code acc_code,
--        A.ledger_id ledger,
        (CASE l.ledger_short_name
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
         ELSE 0 END) ledger,      
        T.transdetail_id transdetail_id,
        --T.amount amount,
        Case @TypeOfCurrency 
			When 'System' Then ISNULL(ROUND(ISNULL(T.system_amount, 0.0),2), 0.0)
			When 'Base' Then ISNULL(ROUND(ISNULL(T.amount, 0.0),2), 0.0)
    	END Amount,
        T.company_id branch_id,
                C.description branch,C.Code CompanyCode,
		CASE @TypeOfCurrency
			WHEN 'Base' THEN cb.iso_code
			WHEN 'System' THEN @SystemCurrencyCode
		END CurrencyCode,
		CASE @TypeOfCurrency
			WHEN 'Base' THEN cb.description
			WHEN 'System' THEN @SystemCurrencyDesc
		END CurrencyDesc, 
        ISNULL( ( SELECT TOP 1 short_code
                FROM Account A1
                JOIN BankAccount B
                    ON B.account_id = A1.account_id
                WHERE A1.short_code = A.short_code
        ), '') bank_account,
        Case @GroupByCode 
        	WHEN 'Branch' Then C.company_id
			WHEN 'Branch And Company' Then C.Company_id
		ELSE ' ' 
		END 'GroupByCode',
        CLRR.Description as Cancellation_Reason,
        MT.Description as Media_Type,
        CLI.media_ref as Media_Ref,
        CLI.payment_name
    FROM TransDetail T
    JOIN Document D
        ON T.document_id = D.document_id
    JOIN Account A
        ON T.Account_id = A.Account_id
    JOIN Company C
        ON C.company_id = T.company_id
    JOIN Ledger L
        ON L.Ledger_Id = A.Ledger_Id
    JOIN Currency CB
	ON CB.Currency_id= C.Base_currency   
    LEFT JOIN CashListItem CLI
        ON CLI.cashlistitem_reversal_transdetail_id=T.Transdetail_id
        AND (@mediatype_id =0 OR CLI.mediatype_id=@mediatype_id)
    LEFT JOIN MediaType MT
        ON CLI.mediatype_id = MT.mediatype_id
    LEFT JOIN cashlistitem_reverse_reason CLRR
	ON CLI.cashlistitem_reverse_reason_id = CLRR.cashlistitem_reverse_reason_id

    WHERE D.documenttype_id IN (1, 8, 10, 11, 12, 20, 21)
    AND ( D.document_date >= @start_date
            AND
            D.document_date <= @end_date
        )
    AND C.company_id = @iBranchID
    ORDER BY document_ref, acc_code
END

GO

